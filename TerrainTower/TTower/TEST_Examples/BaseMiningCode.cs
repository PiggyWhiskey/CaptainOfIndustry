using System;
using System.Collections.Generic;

using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;

namespace Mafi.Core.Vehicles.Jobs
{
    /// <summary>
    /// Represents a job in which an excavator mines material from the terrain using a terrain designation.
    /// </summary>
    [MemberRemovedInSaveVersion("m_preferredProductToMine", 103, typeof(Option<LooseProductProto>), 0, false, double.NaN)]
    [GenerateSerializer(false, null, 0)]
    public sealed class MiningJob : VehicleJob
    {
        /// <summary>
        /// How many ticks to wait after designation was mined successfully. We have this delay to wait for potential
        /// terrain falling.
        /// </summary>
        private static readonly Duration DONE_WAITING_DURATION;

        private static readonly ThicknessTilesF MAX_TOP_LAYER_FOR_MINING_BELOW;
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

        /// <summary>
        /// Duration of truck queue requests in this job. Should be long enough to cover excavator short movements
        /// during mining.
        /// </summary>
        private static readonly Duration TRUCK_QUEUE_ENABLED_DURATION;

        private static readonly Duration TRUCK_QUEUE_ENABLED_MOVING_DURATION;
        private readonly Excavator m_excavator;
        private readonly Lyst<TerrainDesignation> m_extraDesignations;
        private readonly Factory m_factory;
        private readonly Lyst<TerrainDesignation> m_fulfilledDesignations;
        private readonly RobustNavHelper m_navHelper;
        private readonly TerrainPropsManager m_terrainPropsManager;
        private readonly TickTimer m_timer;

        [NewInSaveVersion(140, null, null, typeof(TreesManager), null)]
        private readonly TreesManager m_treesManager;

        private Option<TerrainDesignation> m_designationToMine;

        private Tile2f m_driveStartPosition;

        [DoNotSave(0, null)]
        private Predicate<IDesignation> m_filterDestroyedAndUnreachablePredicate;

        private Option<TerrainDesignationVehicleGoal> m_initialNavGoal;

        private int m_iterationsToMine;

        private Option<LooseProductProto> m_lastMinedProduct;

        private Option<LooseProductProto> m_lastSeenPriorityProduct;

        private State m_previousState;

        [NewInSaveVersion(110, "m_primaryDesignationZzz", "m_primaryDesignationOld", null, null)]
        private Option<TerrainDesignation> m_primaryDesignation;

        [DoNotSave(110, null)]
        private TerrainDesignation m_primaryDesignationOld;

        private State m_state;

        [DoNotSave(0, null)]
        private Func<IDesignation, TerrainTile, RelTile2i, Fix32> m_targetTileCostFn;

        [DoNotSave(0, null)]
        private IReadOnlySet<IDesignation> m_unreachableDesignationsCache;

        private MiningJob(
            VehicleJobId id,
            TerrainPropsManager terrainPropsManager,
            TreesManager treesManager,
            Factory factory,
            Excavator excavator,
            TerrainDesignation primaryDesignation,
            IEnumerable<TerrainDesignation> extraDesignations) : base(id)
        {
            m_timer = new TickTimer();
            m_fulfilledDesignations = new Lyst<TerrainDesignation>();

            m_factory = factory.CheckNotNull();
            m_terrainPropsManager = terrainPropsManager.CheckNotNull();
            m_navHelper = new RobustNavHelper(factory.PathFindingManager);
            initSelf();
            Assert.That(excavator.AssignedTo).HasValue();
            Assert.That(primaryDesignation.IsMiningFulfilled).IsFalse();
            m_excavator = excavator.CheckNotNull();
            m_treesManager = treesManager;
            m_primaryDesignation = Option<TerrainDesignation>.Create(primaryDesignation);
            m_extraDesignations = extraDesignations.ToLyst();
            m_lastSeenPriorityProduct = m_excavator.PrioritizedProduct;
            m_excavator.EnqueueJob(this, false);
            m_previousState = State.Done;
            m_state = State.InitialPathFindingAndDesignationSelect;
        }

        private enum State
        {
            InitialPathFindingAndDesignationSelect,
            DrivingToNewMineLocation,
            DecideMiningOnReachableTile,
            PreparingToMine,
            Mining,
            WaitingForShovel,
            WaitingForFulfilled,
            Done,
            CheckAllFulfilled,
        }

        /// <summary>Whether the state changed last update.</summary>
        private bool StateChanged => m_state != m_previousState;

        private void cleanup()
        {
            if (m_excavator != null)
            {
                m_excavator.ResetCabinTarget();
                m_excavator.SetShovelState(ExcavatorShovelState.Tucked);
                m_excavator.ClearCargoImmediately();
            }
            m_navHelper.CancelAndClear();
            clearDesignationToMine();
            m_extraDesignations.Clear();
            m_fulfilledDesignations.Clear();
        }

        private void clearDesignationToMine()
        {
            if (!m_designationToMine.HasValue)
                return;
            m_designationToMine.Value.RemoveAssignment(this);
            m_designationToMine = Option<TerrainDesignation>.None;
        }

        private IReadOnlySet<IDesignation> filterDestroyedAndUnreachable(
            Lyst<TerrainDesignation> designations)
        {
            m_unreachableDesignationsCache = m_factory.UnreachableDesignationsManager.GetUnreachableDesignationsFor(m_excavator);
            _ = designations.RemoveWhere(m_filterDestroyedAndUnreachablePredicate);
            return m_unreachableDesignationsCache;
        }

        /// <summary>
        /// Tries to find the best tile for mining according to cost function. This functions fills up <see cref="F:Mafi.Core.Vehicles.Jobs.MiningJob.m_tileToMine" />
        /// that can be then used by <see cref="M:Mafi.Core.Vehicles.Jobs.MiningJob.handleMining" />.
        /// </summary>
        /// <returns>True if a tile was found.</returns>
        private bool findBestTileToMine(Option<LooseProductProto> product, out TerrainTile tile)
        {
            Tile2i? fulfilledTileCoord = m_designationToMine.Value.FindBestNonFulfilledTileCoord(m_targetTileCostFn, (Option<Predicate<RelTile2i>>)(r => !m_designationToMine.Value.IsMiningFulfilledAt(r)), out Fix32 minCost);
            if (fulfilledTileCoord.HasValue && minCost < Fix32.MaxValue)
            {
                Assert.That(m_designationToMine.Value.IsMiningFulfilledAt(fulfilledTileCoord.Value)).IsFalse("Already fulfilled?");
                tile = m_factory.TerrainManager[fulfilledTileCoord.Value];
                return true;
            }
            tile = new TerrainTile();
            return false;
        }

        private HeightTilesF getTargetHeight(Tile2i position)
        {
            if (m_designationToMine.Value.ContainsPosition(position))
                return m_designationToMine.Value.GetTargetHeightAt(position);
            Option<TerrainDesignation> miningDesignationAt = m_factory.MiningManager.GetMiningDesignationAt(position);
            return miningDesignationAt.HasValue ? miningDesignationAt.Value.GetTargetHeightAt(position) : HeightTilesF.MaxValue;
        }

        /// <summary>
        /// Waits a little (for terrain physics) and moves all non-fulfilled designations from
        /// `m_fulfilledDesignations` to `m_extraDesignations` and promotes the first one to be the
        /// `m_primaryDesignation`.
        ///
        /// Returns <see cref="F:Mafi.Core.Vehicles.Jobs.MiningJob.State.Done" /> if all designations are fulfilled.
        /// </summary>
        private State handleCheckAllFulfilled()
        {
            if (StateChanged)
            {
                if (m_designationToMine.HasValue)
                    clearDesignationToMine();
                m_timer.Start(DONE_WAITING_DURATION);
                return State.CheckAllFulfilled;
            }
            if (m_timer.Decrement())
                return State.CheckAllFulfilled;
            _ = m_fulfilledDesignations.RemoveWhere(x => x.IsMiningFulfilled);
            m_extraDesignations.AddRange(m_fulfilledDesignations);
            m_fulfilledDesignations.Clear();
            m_primaryDesignation = trySelectPrimaryDesignation(m_primaryDesignation);
            return m_primaryDesignation.HasValue ? State.InitialPathFindingAndDesignationSelect : State.Done;
        }

        private State handleDecideMiningOnReachableTile()
        {
            Assert.That(m_excavator.IsDriving).IsFalse();
            Assert.That(TileToMine).IsNull();
            if (m_designationToMine.IsNone)
            {
                Log.Error("No designation to mine.");
                return State.CheckAllFulfilled;
            }
            if (!isControlledByAssignedTower(m_designationToMine.Value))
                return State.Done;
            if (m_excavator.DrivingState == DrivingState.Paused)
                return State.DecideMiningOnReachableTile;
            m_excavator.KeepTruckQueueEnabled(TRUCK_QUEUE_ENABLED_MOVING_DURATION);
            if (m_excavator.IsNotEmpty)
            {
                m_excavator.UnloadToTruck();
                return State.DecideMiningOnReachableTile;
            }
            if (m_excavator.CannotWorkDueToLowFuel)
            {
                m_excavator.TruckQueue.Disable();
                return State.Done;
            }
            if (m_excavator.PrioritizedProduct.HasValue && m_lastSeenPriorityProduct != m_excavator.PrioritizedProduct)
                return State.Done;
            if (m_designationToMine.Value.IsMiningFulfilled)
                return State.WaitingForFulfilled;
            Assert.That(m_excavator.IsEmpty).IsTrue();
            Option<Truck> firstVehicle = m_excavator.TruckQueue.FirstVehicle;
            if (firstVehicle.HasValue && firstVehicle.Value.IsNotEmpty && !(firstVehicle.Value.Cargo.FirstOrPhantom.Product is LooseProductProto))
            {
                Assert.Fail("Truck came to excavator with non-loose cargo!");
                m_excavator.TruckQueue.ReleaseFirstVehicle();
                return State.DecideMiningOnReachableTile;
            }
            if (findBestTileToMine(Option<LooseProductProto>.None, out TerrainTile tile))
            {
                TileToMine = new TerrainTile?(tile);
                return State.PreparingToMine;
            }
            Assert.That(TileToMine).IsNull();
            return State.DrivingToNewMineLocation;
        }

        private State handleDrivingToNewMineLocation()
        {
            if (m_designationToMine.IsNone)
            {
                Log.Error("No designation to mine.");
                return State.Done;
            }
            TerrainDesignation designation = m_designationToMine.Value;
            if (StateChanged)
            {
                if (!isControlledByAssignedTower(designation))
                    return State.Done;
                if (designation.IsMiningFulfilled)
                    return State.WaitingForFulfilled;
                m_navHelper.StartNavigationTo(m_excavator, m_factory.DesignationGoalFactory.Create(designation, m_excavator.Prototype.MinMiningDistance), allowSimplePathsOnly: true);
                m_excavator.SetShovelState(ExcavatorShovelState.Tucked);
                m_driveStartPosition = m_excavator.Position2f;
                return State.DrivingToNewMineLocation;
            }
            RobustNavResult robustNavResult = m_navHelper.StepNavigation();
            switch (robustNavResult)
            {
                case RobustNavResult.Navigating:
                    return State.DrivingToNewMineLocation;

                case RobustNavResult.GoalReachedSuccessfully:
                    Assert.That(m_driveStartPosition).IsNotEqualTo(m_excavator.Position2f, "Nav failed to move excavator closer to any reachable mine tiles. Adjust tolerances/retries?");
                    return State.DecideMiningOnReachableTile;

                case RobustNavResult.FailGoalUnreachable:
                    if (!designation.IsDestroyed && designation.IsMiningNotFulfilled)
                        m_factory.UnreachableDesignationsManager.MarkUnreachableFor(designation, m_excavator);
                    if (m_primaryDesignation == designation)
                        m_primaryDesignation = Option<TerrainDesignation>.None;
                    else
                        _ = m_extraDesignations.Remove(designation);
                    if (m_extraDesignations.IsEmpty)
                        return State.CheckAllFulfilled;
                    m_primaryDesignation = trySelectPrimaryDesignation(m_primaryDesignation);
                    if (!m_primaryDesignation.HasValue)
                        return State.CheckAllFulfilled;
                    clearDesignationToMine();
                    return State.InitialPathFindingAndDesignationSelect;

                default:
                    Log.Error(string.Format("Invalid state: {0}", robustNavResult));
                    goto case RobustNavResult.FailGoalUnreachable;
            }
        }

        private State handleInitialPathFindingAndDesignationSelect()
        {
            if (StateChanged || m_primaryDesignation.IsNone)
            {
                Assert.That(m_designationToMine).IsNone();
                m_primaryDesignation = trySelectPrimaryDesignation(m_primaryDesignation);
                if (m_primaryDesignation.IsNone)
                    return State.CheckAllFulfilled;
                RelTile1i tolerance = m_terrainPropsManager.ContainsPropInDesignation(m_primaryDesignation.Value) || m_treesManager.ContainsStumpInDesignation(m_primaryDesignation.Value) ? (m_excavator.Prototype.MaxMiningDistance + m_excavator.Prototype.MinMiningDistance) / 2 : m_excavator.Prototype.MinMiningDistance;
                RelTile1i relTile1i = m_excavator.Prototype.MaxMiningDistance - tolerance;
                TerrainDesignationVehicleGoal goal = m_factory.DesignationGoalFactory.Create(m_primaryDesignation.Value, tolerance, m_extraDesignations.Where(x => x.IsMiningReadyToBeFulfilled));
                m_navHelper.StartNavigationTo(m_excavator, goal, extraTolerancePerRetry: new RelTile1i?(relTile1i));
                m_initialNavGoal = (Option<TerrainDesignationVehicleGoal>)goal;
                return State.InitialPathFindingAndDesignationSelect;
            }
            TerrainDesignation d = m_primaryDesignation.Value;
            if (m_designationToMine.IsNone && m_initialNavGoal.Value.ActualGoalDesignation.ValueOrNull is TerrainDesignation valueOrNull)
            {
                if (tryAssignTo(valueOrNull))
                {
                    m_designationToMine = (Option<TerrainDesignation>)valueOrNull;
                }
                else if (valueOrNull != d && tryAssignTo(d))
                {
                    m_designationToMine = (Option<TerrainDesignation>)d;
                }
                else
                {
                    _ = m_extraDesignations.RemoveWhere(x => x.IsDestroyed);
                    foreach (TerrainDesignation extraDesignation in m_extraDesignations)
                    {
                        if (tryAssignTo(extraDesignation))
                        {
                            m_designationToMine = Option<TerrainDesignation>.Create(extraDesignation);
                            break;
                        }
                    }
                    if (m_designationToMine.IsNone)
                        return State.Done;
                }
                Assert.That(m_designationToMine).HasValue();
                Assert.That(m_designationToMine.Value.IsMiningNotFulfilled).IsTrue();
                Assert.That(m_designationToMine.Value.IsMiningReadyToBeFulfilled).IsTrue();
            }
            Assert.That(m_initialNavGoal).HasValue();
            RobustNavResult robustNavResult = m_navHelper.StepNavigation();
            switch (robustNavResult)
            {
                case RobustNavResult.Navigating:
                    return State.InitialPathFindingAndDesignationSelect;

                case RobustNavResult.GoalReachedSuccessfully:
                    if (m_designationToMine.IsNone)
                    {
                        Log.Error("Mining designation was not set.");
                        m_designationToMine = Option<TerrainDesignation>.Create(d);
                        if (!m_designationToMine.Value.TryAssignTo(this))
                            return State.Done;
                    }
                    m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
                    return State.DecideMiningOnReachableTile;

                case RobustNavResult.FailGoalUnreachable:
                    return State.CheckAllFulfilled;

                default:
                    Log.Error(string.Format("Invalid state: {0}", robustNavResult));
                    goto case RobustNavResult.FailGoalUnreachable;
            }

            bool tryAssignTo(TerrainDesignation e)
            {
                return !e.IsDestroyed && e.IsMiningNotFulfilled && e.IsMiningReadyToBeFulfilled && e.TryAssignTo(this);
            }
        }

        /// <summary>Performs the mining operation.</summary>
        private State handleMining()
        {
            Assert.That(TileToMine).HasValue();
            Assert.That(m_excavator.IsDriving).IsFalse();
            Assert.That(m_excavator.CabinTargetDelta).IsNear(AngleDegrees1f.Zero, 10.Degrees());
            m_excavator.KeepTruckQueueEnabled(TRUCK_QUEUE_ENABLED_DURATION);
            if (StateChanged)
            {
                Assert.That(m_excavator.ShovelState).IsEqualTo(ExcavatorShovelState.PrepareToMine);
                m_excavator.SetShovelState(ExcavatorShovelState.Mine);
                m_iterationsToMine = m_excavator.Prototype.MineTimings.MineTileIterations;
                m_timer.Start(m_excavator.Prototype.MineTimings.MineIterationDuration);
            }
            if (m_timer.Decrement())
                return State.Mining;
            bool flag1 = m_terrainPropsManager.TerrainTileToProp.TryGetValue(TileToMine.Value.TileCoordSlim, out TerrainPropId key);
            bool flag2 = false;
            if (!flag1)
            {
                flag2 = m_treesManager.Stumps.TryGetValue(new TreeId(TileToMine.Value.TileCoordSlim), out TreeStumpData _);
                if (!flag2)
                    m_excavator.MineMixedAt(TileToMine.Value.CoordAndIndex, new Excavator.MineTileMixedFn(this.mineTileMixed));
            }
            --m_iterationsToMine;
            if (m_iterationsToMine > 0)
            {
                m_timer.Start(m_excavator.Prototype.MineTimings.MineIterationDuration);
                return State.Mining;
            }
            if (flag1)
            {
                if (!m_terrainPropsManager.TerrainProps.TryGetValue(key, out TerrainPropData propData))
                    Log.Warning(string.Format("Prop '{0}' can't find prop data.", key));
                else if (!m_terrainPropsManager.TryRemovePropAtTile(TileToMine.Value.TileCoord, false))
                    Log.Warning(string.Format("Failed to remove prop '{0}'.", key));
                else
                    m_excavator.MineProp(propData);
            }
            else if (flag2)
            {
                m_treesManager.RemoveStumpAtTile(TileToMine.Value.TileCoord);
            }

            TileToMine = new TerrainTile?();
            return State.WaitingForShovel;
        }

        /// <summary>Prepares shovel and cabin for mining.</summary>
        private State handlePreparingToMine()
        {
            Assert.That(TileToMine).HasValue();
            Assert.That(m_excavator.IsDriving).IsFalse();
            m_excavator.KeepTruckQueueEnabled(TRUCK_QUEUE_ENABLED_DURATION);
            if (StateChanged)
            {
                m_excavator.SetCabinTarget(TileToMine.Value.CenterTile2f, nameof(handlePreparingToMine));
                m_excavator.SetShovelState(ExcavatorShovelState.PrepareToMine);
            }
            return m_excavator.IsMoving || !m_excavator.CabinTargetDelta.IsNear(AngleDegrees1f.Zero, 10.Degrees()) || !m_excavator.IsShovelAtTarget ? State.PreparingToMine : State.Mining;
        }

        private State handleState()
        {
            switch (m_state)
            {
                case State.InitialPathFindingAndDesignationSelect:
                    return handleInitialPathFindingAndDesignationSelect();

                case State.DrivingToNewMineLocation:
                    return handleDrivingToNewMineLocation();

                case State.DecideMiningOnReachableTile:
                    return handleDecideMiningOnReachableTile();

                case State.PreparingToMine:
                    return handlePreparingToMine();

                case State.Mining:
                    return handleMining();

                case State.WaitingForShovel:
                    return handleWaitingForShovel();

                case State.WaitingForFulfilled:
                    return handleWaitingForFulfilled();

                case State.CheckAllFulfilled:
                    return handleCheckAllFulfilled();

                case State.Done:
                default:
                    Assert.Fail(string.Format("Unknown/invalid mining job state '{0}'.", m_state));
                    return State.Done;
            }
        }

        /// <summary>
        /// When designation is fulfilled, we wait little bit to make sure it stays this way.
        /// </summary>
        private State handleWaitingForFulfilled()
        {
            if (StateChanged)
                m_timer.Start(DONE_WAITING_DURATION);
            if (!m_designationToMine.Value.IsMiningFulfilled)
                return State.DecideMiningOnReachableTile;
            if (!m_timer.Decrement())
                return State.Done;
            m_excavator.KeepTruckQueueEnabled(TRUCK_QUEUE_ENABLED_DURATION);
            return State.WaitingForFulfilled;
        }

        private State handleWaitingForShovel() => !m_excavator.IsShovelAtTarget ? State.WaitingForShovel : State.DecideMiningOnReachableTile;

        private bool hasReachableTilesToMine() => !m_designationToMine.IsNone && m_designationToMine.Value.FindBestNonFulfilledTileCoord((d, t, c) => (Fix64)t.TileCoord.DistanceSqrTo(m_excavator.GroundPositionTile2i), (Option<Predicate<RelTile2i>>)(r => !m_designationToMine.Value.IsMiningFulfilledAt(r)), out Fix64 minCost).HasValue && minCost <= m_excavator.Prototype.MaxMiningDistance.Squared;

        [InitAfterLoad(InitPriority.Normal)]
        private void initAfterLoad(int saveVersion) => initSelf();

        private void initSelf()
        {
            m_targetTileCostFn = new Func<IDesignation, TerrainTile, RelTile2i, Fix32>(targetTileCostFunction);
            m_filterDestroyedAndUnreachablePredicate = d => d.IsDestroyed || m_unreachableDesignationsCache.Contains(d);
        }

        private bool isControlledByAssignedTower(TerrainDesignation designation)
        {
            if (!m_excavator.AssignedTo.IsNone)
                return designation.IsManagedByTower(m_excavator.AssignedTo.Value);
            Log.Warning("Unassigned excavator trying to mine.");
            return false;
        }

        private void mineTileMixed(
            Tile2iAndIndex tileAndIndex,
            PartialMinedProductTracker partialMinedProductTracker,
            ThicknessTilesF maxThickness)
        {
            Assert.That(maxThickness).IsPositive("Max mined is not positive. Why are we even mining?");
            TerrainManager terrainManager = m_factory.TerrainManager;
            HeightTilesF targetHeight = getTargetHeight(tileAndIndex.TileCoord);
            HeightTilesF height = terrainManager.GetHeight(tileAndIndex.Index);

            if (height <= targetHeight)
                return;

            if (terrainManager.TryGetTileSurface(tileAndIndex.Index, out TileSurfaceData tileSurfaceData) && !tileSurfaceData.IsAutoPlaced && tileSurfaceData.Height > targetHeight)
                terrainManager.ClearCustomSurface(tileAndIndex);

            TerrainMaterialThickness firstLayer = terrainManager.GetFirstLayer(tileAndIndex.Index);
            PartialQuantity quantity = partialMinedProductTracker.MaxAllowedQuantityOf((ProductProto)firstLayer.Material.MinedProduct);
            TerrainMaterialThicknessSlim materialThicknessSlim = new TerrainMaterialThicknessSlim();

            if (quantity.IsPositive)
            {
                ThicknessTilesF thicknessTilesF = height - targetHeight;
                thicknessTilesF = thicknessTilesF.Min(maxThickness);
                ThicknessTilesF maxThickness1 = thicknessTilesF.Min(firstLayer.Material.QuantityToThickness(quantity));
                if (maxThickness1.IsPositive)
                {
                    materialThicknessSlim = terrainManager.MineMaterial(tileAndIndex, maxThickness1);
                    _ = terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
                }
            }

            ThicknessTilesF thicknessTilesF1 = materialThicknessSlim.Thickness;

            if (thicknessTilesF1.IsZero && terrainManager.GetFirstLayer(tileAndIndex.Index).Thickness <= MAX_TOP_LAYER_FOR_MINING_BELOW && terrainManager.GetLayersCountNoBedrock(tileAndIndex.Index) >= 1)
            {
                TerrainMaterialThickness secondLayer = terrainManager.GetSecondLayer(tileAndIndex.Index);
                quantity = partialMinedProductTracker.MaxAllowedQuantityOf((ProductProto)secondLayer.Material.MinedProduct);
                if (quantity.IsPositive)
                {
                    thicknessTilesF1 = terrainManager.GetHeight(tileAndIndex.Index) - targetHeight;
                    thicknessTilesF1 = thicknessTilesF1.Min(maxThickness);
                    ThicknessTilesF maxThickness2 = thicknessTilesF1.Min(secondLayer.Material.QuantityToThickness(quantity));
                    if (maxThickness2.IsPositive)
                    {
                        materialThicknessSlim = terrainManager.MineMaterialFromSecondLayer(tileAndIndex, maxThickness2);
                        _ = terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
                    }
                }
            }

            thicknessTilesF1 = materialThicknessSlim.Thickness;

            if (!thicknessTilesF1.IsNotZero)
                return;

            PartialProductQuantity partialProductQuantity = materialThicknessSlim.ToPartialProductQuantity(terrainManager);

            partialMinedProductTracker.AddMinedProduct(partialProductQuantity);

        }

        /// <summary>
        /// Cost function that is responsible for choosing the best tile on the designation.
        /// </summary>
        private Fix32 targetTileCostFunction(
            IDesignation designation,
            TerrainTile tile,
            RelTile2i coord)
        {
            if (tile.TileCoord.DistanceSqrTo(m_excavator.GroundPositionTile2i) > m_excavator.Prototype.MaxMiningDistance.Squared && m_previousState != State.DrivingToNewMineLocation)
                return Fix32.MaxValue;
            RelTile2i relTile2i = (Tile2i)tile.TileCoordSlim - m_excavator.GroundPositionTile2i;
            Fix32 zero = Fix32.Zero;
            if (relTile2i.IsNotZero)
            {
                Vector2f vector2f = relTile2i.Vector2f;
                ref Vector2f local = ref vector2f;
                AngleDegrees1f direction = m_excavator.Direction;
                Vector2f directionVector = direction.DirectionVector;
                direction = local.AngleTo(directionVector);
                AngleDegrees1f abs = direction.Abs;
                zero += abs.Radians;
            }
            if (m_terrainPropsManager.TerrainTileToProp.ContainsKey(tile.TileCoordSlim))
                return zero - (Fix32)10;
            if (m_treesManager.Stumps.ContainsKey(new TreeId(tile.TileCoordSlim)))
                return zero - (Fix32)10;
            ThicknessTilesF thicknessTilesF = tile.FirstLayer.Thickness.Min(tile.Height - designation.GetTargetHeightAt(coord));
            Assert.That(thicknessTilesF).IsPositive();
            return zero - thicknessTilesF.Value.Min((Fix32)2).Squared().ToFix32();
        }

        private Option<TerrainDesignation> trySelectPrimaryDesignation(
            Option<TerrainDesignation> candidate)
        {
            IReadOnlySet<IDesignation> readOnlySet = filterDestroyedAndUnreachable(m_extraDesignations);
            _ = m_extraDesignations.RemoveWhere(x => x.IsMiningFulfilled, m_fulfilledDesignations);
            TerrainDesignation valueOrNull = candidate.ValueOrNull;
            if (valueOrNull != null)
            {
                if (!valueOrNull.IsDestroyed && valueOrNull.IsMiningNotFulfilled && valueOrNull.IsMiningReadyToBeFulfilled && valueOrNull.CanBeAssigned(false) && !readOnlySet.Contains(valueOrNull))
                    return Option<TerrainDesignation>.Create(valueOrNull);
                if (!valueOrNull.IsDestroyed && !readOnlySet.Contains(valueOrNull))
                {
                    if (valueOrNull.IsMiningFulfilled)
                        m_fulfilledDesignations.Add(valueOrNull);
                    else
                        m_extraDesignations.Add(valueOrNull);
                }
            }
            for (int index = 0; index < m_extraDesignations.Count; ++index)
            {
                TerrainDesignation extraDesignation = m_extraDesignations[index];
                if (extraDesignation.IsMiningReadyToBeFulfilled && extraDesignation.CanBeAssigned(false))
                {
                    m_extraDesignations.RemoveAt(index);
                    return Option<TerrainDesignation>.Create(extraDesignation);
                }
            }
            return Option<TerrainDesignation>.None;
        }

        protected override void DeserializeData(BlobReader reader)
        {
            base.DeserializeData(reader);
            m_designationToMine = Option<TerrainDesignation>.Deserialize(reader);
            m_driveStartPosition = Tile2f.Deserialize(reader);
            reader.SetField(this, "m_excavator", Excavator.Deserialize(reader));
            reader.SetField(this, "m_extraDesignations", Lyst<TerrainDesignation>.Deserialize(reader));
            reader.RegisterResolvedMember(this, "m_factory", typeof(Factory), true);
            reader.SetField(this, "m_fulfilledDesignations", Lyst<TerrainDesignation>.Deserialize(reader));
            m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.Deserialize(reader);
            m_iterationsToMine = reader.ReadInt();
            m_lastMinedProduct = Option<LooseProductProto>.Deserialize(reader);
            m_lastSeenPriorityProduct = Option<LooseProductProto>.Deserialize(reader);
            reader.SetField(this, "m_navHelper", RobustNavHelper.Deserialize(reader));
            if (reader.LoadedSaveVersion < 103)
                _ = Option<LooseProductProto>.Deserialize(reader);
            m_previousState = (State)reader.ReadInt();
            if (reader.LoadedSaveVersion < 110)
                m_primaryDesignationOld = TerrainDesignation.Deserialize(reader);
            m_primaryDesignation = reader.LoadedSaveVersion >= 110 ? Option<TerrainDesignation>.Deserialize(reader) : (Option<TerrainDesignation>)m_primaryDesignationOld;
            m_state = (State)reader.ReadInt();
            reader.SetField(this, "m_terrainPropsManager", TerrainPropsManager.Deserialize(reader));
            TileToMine = reader.ReadNullableStruct<TerrainTile>();
            reader.SetField(this, "m_timer", TickTimer.Deserialize(reader));
            reader.SetField(this, "m_treesManager", reader.LoadedSaveVersion >= 140 ? TreesManager.Deserialize(reader) : null);
            if (reader.LoadedSaveVersion < 140)
                reader.RegisterResolvedMember(this, "m_treesManager", typeof(TreesManager), true);
            reader.RegisterInitAfterLoad(this, "initAfterLoad", InitPriority.Normal);
        }

        protected override bool DoJobInternal()
        {
            Assert.That(m_excavator.AssignedTo).HasValue();
            if (m_state == State.DecideMiningOnReachableTile && m_excavator.LastRefuelRequestIssue != RefuelRequestIssue.None && m_excavator.NeedsRefueling && !m_excavator.CanRunWithNoFuel)
            {
                if (m_factory.FuelStationsManager.TryRequestTruckForRefueling(m_excavator))
                {
                    m_excavator.LastRefuelRequestIssue = RefuelRequestIssue.None;
                }
                else if (m_factory.FuelStationsManager.TryRefuelSelf(m_excavator))
                {
                    m_excavator.TruckQueue.Disable();
                    cleanup();
                    return false;
                }
            }
            State state = handleState();
            m_previousState = m_state;
            m_state = state;
            if (m_state != State.Done)
                return true;
            cleanup();
            return false;
        }

        protected override void OnDestroy()
        {
            Assert.That(m_excavator).IsNotNull("ReturnToPool on non-initialized instance.");
            cleanup();
            m_primaryDesignation = Option<TerrainDesignation>.None;
            m_extraDesignations.Clear();
            TileToMine = new TerrainTile?();
            m_unreachableDesignationsCache = null;
            m_lastMinedProduct = Option<LooseProductProto>.None;
            m_timer.Reset();
        }

        protected override Duration RequestCancelReturnDeadline()
        {
            cleanup();
            ((IVehicleFriend)m_excavator).AlsoCancelAllOtherJobs(this);
            return Duration.Zero;
        }

        protected override void SerializeData(BlobWriter writer)
        {
            base.SerializeData(writer);
            Option<TerrainDesignation>.Serialize(m_designationToMine, writer);
            Tile2f.Serialize(m_driveStartPosition, writer);
            Excavator.Serialize(m_excavator, writer);
            Lyst<TerrainDesignation>.Serialize(m_extraDesignations, writer);
            Lyst<TerrainDesignation>.Serialize(m_fulfilledDesignations, writer);
            Option<TerrainDesignationVehicleGoal>.Serialize(m_initialNavGoal, writer);
            writer.WriteInt(m_iterationsToMine);
            Option<LooseProductProto>.Serialize(m_lastMinedProduct, writer);
            Option<LooseProductProto>.Serialize(m_lastSeenPriorityProduct, writer);
            RobustNavHelper.Serialize(m_navHelper, writer);
            writer.WriteInt((int)m_previousState);
            Option<TerrainDesignation>.Serialize(m_primaryDesignation, writer);
            writer.WriteInt((int)m_state);
            TerrainPropsManager.Serialize(m_terrainPropsManager, writer);
            writer.WriteNullableStruct(TileToMine);
            TickTimer.Serialize(m_timer, writer);
            TreesManager.Serialize(m_treesManager, writer);
        }

        static MiningJob()
        {
            s_serializeDataDelayedAction = (obj, writer) => ((VehicleJob)obj).SerializeData(writer);
            s_deserializeDataDelayedAction = (obj, reader) => ((VehicleJob)obj).DeserializeData(reader);
            DONE_WAITING_DURATION = 1.Seconds();
            MAX_TOP_LAYER_FOR_MINING_BELOW = 0.5.TilesThick();
            TRUCK_QUEUE_ENABLED_DURATION = 20.Seconds();
            TRUCK_QUEUE_ENABLED_MOVING_DURATION = 30.Seconds();
        }

        public override VehicleFuelConsumption CurrentFuelConsumption => m_excavator.State != ExcavatorState.WaitingForTruck && (m_state != State.InitialPathFindingAndDesignationSelect || m_excavator.IsDriving) ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;

        public override bool IsTrueJob => true;

        public override LocStrFormatted JobInfo
        {
            get
            {
                switch (m_state)
                {
                    case State.InitialPathFindingAndDesignationSelect:
                        Excavator excavator = m_excavator;
                        return (LocStrFormatted)((excavator != null ? (excavator.IsDriving ? 1 : 0) : 0) != 0 ? Tr.VehicleJob__DrivingToGoal : Tr.VehicleJob__SearchingForDesignation);

                    case State.DrivingToNewMineLocation:
                        return (LocStrFormatted)Tr.VehicleJob__DrivingToGoal;

                    case State.DecideMiningOnReachableTile:
                    case State.PreparingToMine:
                    case State.Mining:
                    case State.WaitingForShovel:
                    case State.WaitingForFulfilled:
                    case State.CheckAllFulfilled:
                        return (LocStrFormatted)Tr.VehicleJob__Loading;

                    case State.Done:
                    default:
                        return (LocStrFormatted)Tr.VehicleJob__InvalidState;
                }
            }
        }

        public TerrainTile? TileToMine { get; private set; }

        public static MiningJob Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out MiningJob miningJob))
                reader.EnqueueDataDeserialization(miningJob, s_deserializeDataDelayedAction);
            return miningJob;
        }

        public static void Serialize(MiningJob value, BlobWriter writer)
        {
            if (!writer.TryStartClassSerialization(value))
                return;
            writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
        }

        [GlobalDependency(RegistrationMode.AsSelf, false, false)]
        public class Factory
        {
            private readonly Lyst<TerrainDesignation> m_extraDesignationsTmp;
            private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
            public readonly TerrainDesignationVehicleGoal.Factory DesignationGoalFactory;
            public readonly IFuelStationsManager FuelStationsManager;
            public readonly ITerrainMiningManager MiningManager;
            public readonly IVehiclePathFindingManager PathFindingManager;
            public readonly TerrainManager TerrainManager;
            public readonly TerrainPropsManager TerrainPropsManager;
            public readonly TreesManager TreesManager;
            public readonly UnreachableTerrainDesignationsManager UnreachableDesignationsManager;

            public Factory(
                VehicleJobId.Factory vehicleJobIdFactory,
                ITerrainMiningManager miningManager,
                IVehiclePathFindingManager pathFindingManager,
                IFuelStationsManager fuelStationsManager,
                TerrainManager terrainManager,
                TreesManager treesManager,
                TerrainPropsManager terrainPropsManager,
                TerrainDesignationVehicleGoal.Factory designationGoalFactory,
                UnreachableTerrainDesignationsManager unreachableDesignationsManager) : base()
            {
                m_extraDesignationsTmp = new Lyst<TerrainDesignation>();
                m_vehicleJobIdFactory = vehicleJobIdFactory;
                MiningManager = miningManager;
                PathFindingManager = pathFindingManager;
                FuelStationsManager = fuelStationsManager;
                TerrainManager = terrainManager;
                TreesManager = treesManager;
                TerrainPropsManager = terrainPropsManager;
                DesignationGoalFactory = designationGoalFactory;
                UnreachableDesignationsManager = unreachableDesignationsManager;
            }

            public void EnqueueJob(
                Excavator excavator,
                TerrainDesignation designation,
                IEnumerable<TerrainDesignation> extraDesignations)
            {
                m_extraDesignationsTmp.Clear();
                _ = new MiningJob(m_vehicleJobIdFactory.GetNextId(), TerrainPropsManager, TreesManager, this, excavator, designation, extraDesignations ?? m_extraDesignationsTmp);
            }

            public bool TryCreateAndEnqueueJob(
                Excavator excavator,
                bool tryIgnoreReservations = false,
                Predicate<TerrainDesignation> predicate = null)
            {
                if (excavator.MineTower.IsNone)
                    return false;
                MineTower tower = excavator.MineTower.Value;
                if (!tower.IsEnabled)
                    return false;
                m_extraDesignationsTmp.Clear();
                if (!MiningManager.TryFindClosestReadyToMine(tower, excavator.Position2f.Tile2i, excavator, out TerrainDesignation bestDesignation, excavator.PrioritizedProduct, tryIgnoreReservations, predicate, m_extraDesignationsTmp))
                    return false;
                _ = new MiningJob(m_vehicleJobIdFactory.GetNextId(), TerrainPropsManager, TreesManager, this, excavator, bestDesignation, m_extraDesignationsTmp);
                m_extraDesignationsTmp.Clear();
                return true;
            }
        }
    }
}