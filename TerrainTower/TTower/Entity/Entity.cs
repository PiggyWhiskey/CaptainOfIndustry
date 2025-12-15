using System;
using System.Collections.Generic;
using System.Linq;

using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Utils;
using Mafi.Serialization;

using TerrainTower.Extras;

namespace TerrainTower.TTower
{
    /// <summary>
    /// <para><see cref="m_productsData"/> All mining processing, handled as <see cref="TerrainTowerProductData"/></para>
    /// <para>SortProducts = Move from Unsorted to BeingSorted</para>
    /// <para>PushResultsToBuffer = Move from BeingSorted to Sorted</para>
    /// <para>Functions separated to allow paced sorted, and to prevent abuse of extremely fast mining.</para>
    /// </summary>
    public sealed partial class TerrainTowerEntity
        : LayoutEntity,
        IMaintainedEntity,
        IElectricityConsumingEntity,
        IEntityWithWorkers,
        IEntityWithPorts,
        IAreaManagingTower,
        IEntityWithBoost,
        IUnityConsumingEntity,
        IEntityWithSimUpdate
    {
        private readonly ICalendar m_calendar;

        private readonly TerrainDesignationsManager m_designationManager;

        private readonly Set<ProductProto> m_dumpableProducts;

        /// <summary>
        /// Dict of GlobalOutputBuffers to handle Inbound Products for dumping
        /// </summary>
        private readonly Dict<ProductProto, GlobalOutputBuffer> m_dumpBuffers;

        private readonly IElectricityConsumer m_electricityConsumer;

        private readonly Set<TerrainDesignation> m_managedDesignations;

        /// <summary>
        /// All mined products and their buffers/sorting/output control and data <see cref="TerrainTowerProductData"/>
        /// </summary>
        private readonly Dict<ProductProto, TerrainTowerProductData> m_productsData;

        /// <summary>
        /// Interface used within <see cref="TerrainTowerProductData"/> to control product output to other entities
        /// </summary>
        private readonly IProductsManager m_productsManager;

        private readonly TickTimer m_sortingTimer;

        private readonly TickTimer m_terrainTimer;

        /// <summary>
        /// Used to speed up the process of finding the nearest Dumping Designation by storing only unfulfilled designations.
        /// </summary>
        private readonly Set<TerrainDesignation> m_unfulfilledDumpingDesignations;

        /// <summary>
        /// Used to speed up the process of finding the nearest Mining Designation by storing only unfulfilled designations.
        /// </summary>
        private readonly Set<TerrainDesignation> m_unfulfilledMiningDesignations;

        private EntityNotificator m_entityBoostedNotif;

        private EntityNotificator m_inputBlockedNotif;

        private EntityNotificator m_missingDesignationNotif;
        private EntityNotificator m_missingDumpItemNotif;
        private EntityNotificator m_outputBlockedNotif;

        /// <summary>
        /// Private variable for all <see cref="m_productsData"/> buffers.
        /// </summary>
        [DoNotSave(0, null)]
        private Lyst<IProductBuffer> m_outputBuffers;

        /// <summary>
        /// Chars from A to Z, 1 for each Output Port, plus a '-' for do not assign
        /// </summary>
        private ImmutableArray<char> m_outputsNames;

        private TerrainTowerProto m_prototype;
        private bool m_sortedBufferIsPositive;

        private Option<TerrainTowerManager> m_terrainTowerManager;

        [DoNotSave(0, null)]
        private TerrainDesignation m_tmpDumpDesignation;

        [DoNotSave(0, null)]
        private TerrainDesignation m_tmpMineDesignation;

        private TerrainManager ContextTerrainManager => Context.TerrainManager;

        private bool HasUnfulfilledDesignation { get; set; }

        private bool HasUnfulfilledMining { get; set; }

        /// <summary>
        /// If the Tower is missing a configured Terrain Designation, TRUE
        /// HasUnfulfilled is set from updateTerrainNotifications
        /// </summary>
        private bool IsMissingDesignation => !HasUnfulfilledDesignation && !HasUnfulfilledMining;

        /// <summary>
        /// Verifies if the Tower DOES NOT have any unfulfilled dumping designations or that the <see cref="DumpTotal"/> is Zero
        /// </summary>
        private bool ValidateDumping => !HasUnfulfilledDesignation || DumpTotal.IsZero;

        /// <summary>
        /// Verifies if the tower DOES NOT have any unfulfilled mining designations or that the <see cref="MixedCapacityLeft"/> is Zero
        /// i.e. no designations or no room to Mine
        /// </summary>
        private bool ValidateMining => !HasUnfulfilledMining || MixedCapacityLeft == Quantity.Zero;

        protected override bool IsEnabledNow => base.IsEnabledNow && Maintenance.CanWork();

        /// <summary>
        /// Products we've mined or selected to be Sorted/Outputted
        /// </summary>
        public IEnumerable<ProductProto> AllowedProducts => m_productsData.Keys;

        /// <summary>
        /// All mineable products
        /// </summary>
        public ImmutableArray<ProductProto> AllSupportedProducts { get; private set; }

        public PolygonTerrainArea2i Area { get; private set; }

        /// <summary>
        /// Unused by Terrain Tower, but required for Interface
        /// IAreaManagegingTower.AssignedFuelStations | IAssignableToFuelStation.AssignedFuelStations
        /// </summary>
        public IReadOnlySet<FuelStation> AssignedFuelStations => null;

        public Upoints? BoostCost => Prototype.BoostCost;

        public override bool CanBePaused => true;

        /// <summary>
        /// General Capacity Value, set from Prototype. Used for <see cref="MixedTotal"/>, <see cref="m_productsData"/>, and <see cref="m_dumpBuffers"/>
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity Capacity { get; internal set; }

        public State CurrentState { get; private set; }

        public IReadOnlySet<ProductProto> DumpableProducts => m_dumpableProducts;

        public IReadOnlyCollection<IProductBuffer> DumpBuffers => m_dumpBuffers.Values;

        /// <summary>
        /// Local Variable - Sum of m_dumpBuffers quantity - Cached to allow fast access
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity DumpTotal { get; private set; }

        public Option<IElectricityConsumerReadonly> ElectricityConsumer => m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();

        public bool IsBoosted { get; private set; }

        public bool IsBoostRequested { get; private set; }

        public bool IsIdleForMaintenance => CurrentState != State.Working;

        public IEntityMaintenanceProvider Maintenance { get; private set; }

        public IReadOnlySet<TerrainDesignation> ManagedDesignations => m_managedDesignations;

        public Upoints MaxMonthlyUnityConsumed => MonthlyUnityConsumed;

        /// <summary>
        /// Quantity left in <see cref="MixedTotal"/>
        /// </summary>
        public Quantity MixedCapacityLeft => (Capacity - MixedTotal).Max(Quantity.Zero);

        /// <summary>
        /// Percent: <see cref="MixedTotal"/> / <see cref="Capacity"/>
        /// </summary>
        public Percent MixedPercentFull => Capacity.IsNotPositive || MixedTotal.IsNotPositive ? Percent.Zero : !(MixedTotal > Capacity) ? Percent.FromRatio(MixedTotal.Value, Capacity.Value) : Percent.Hundred;

        /// <summary>
        /// Total of all Unsorted Quantities
        /// - Cached and updated via <see cref="updateMixedQuantity"/>
        /// - Updated during <see cref="tryMineDesignation_MineLayer"/> too
        /// </summary>
        public Quantity MixedTotal { get; private set; }

        /// <summary>
        /// If Boost is Requested, and a Boost Cost is set, return the cost, otherwise return 0
        /// </summary>
        public Upoints MonthlyUnityConsumed => IsBoostRequested && BoostCost.HasValue ? BoostCost.Value : Upoints.Zero;

        /// <summary>
        /// Buffers from private variable <see cref="m_outputBuffers"/>
        /// </summary>
        public IIndexable<IProductBuffer> OutputBuffers => m_outputBuffers;

        public new TerrainTowerProto Prototype
        {
            get => m_prototype;
            set
            {
                m_prototype = value;
                base.Prototype = value;
            }
        }

        public Random Random { get; private set; }

        public Duration SortDuration => Prototype.SortDuration;

        /// <summary>
        /// Quantity that is allowed to be sorted per SortDuration
        /// </summary>
        public Quantity SortQuantityPerDuration => Prototype.SortQuantityPerDuration;

        public Option<UnityConsumer> UnityConsumer { get; private set; }

        bool IEntityWithWorkers.HasWorkersCached { get; set; }

        MaintenanceCosts IMaintainedEntity.MaintenanceCosts => Prototype.Costs.Maintenance;

        Electricity IElectricityConsumingEntity.PowerRequired => Prototype.ElectricityConsumed;

        Proto.ID IUnityConsumingEntity.UpointsCategoryId => IdsCore.UpointsCategories.Boost;

        int IEntityWithWorkers.WorkersNeeded => Prototype.Costs.Workers;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proto"></param>
        /// <param name="transform"></param>
        /// <param name="context"></param>
        /// <param name="designationManager"></param>
        /// <param name="maintenanceProvidersFactory"></param>
        /// <param name="productsManager"></param>
        /// <param name="towersManager"></param>
        internal TerrainTowerEntity(
            EntityId id,
            TerrainTowerProto proto,
            TileTransform transform,
            EntityContext context,
            TerrainDesignationsManager designationManager,
            ICalendar calendar,
            IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
            IProductsManager productsManager,
            TerrainTowerManager towersManager,
            ITerrainDumpingManager dumpingManager) : base(id, proto, transform, context)
        {
            Prototype = proto;

            //Designation Management
            m_designationManager = designationManager;
            m_designationManager.DesignationAdded.Add(this, new Action<TerrainDesignation>(onDesignationAdded));
            m_designationManager.DesignationRemoved.Add(this, new Action<TerrainDesignation>(onDesignationRemoved));
            m_designationManager.DesignationFulfilledChanged.Add(this, new Action<TerrainDesignation>(onDesignationFulfilledChanged));

            //All Mined Products (Mixed, Unsorted, Sorted)
            m_productsData = new Dict<ProductProto, TerrainTowerProductData>();

            //Gets all allowed dumpable products from the <see cref="TerrainDumpingManager"/> as the default starting point
            m_dumpableProducts = new Set<ProductProto>(dumpingManager.ProductsAllowedToDump.Distinct());

            //Buffers from m_productsData.Values.Buffer
            m_outputBuffers = new Lyst<IProductBuffer>();

            //All Dump products
            m_dumpBuffers = new Dict<ProductProto, GlobalOutputBuffer>();

            //Product Manager used to Add/Remove products from the world production statistics
            m_productsManager = productsManager;

            m_managedDesignations = new Set<TerrainDesignation>();
            m_unfulfilledMiningDesignations = new Set<TerrainDesignation>();
            m_unfulfilledDumpingDesignations = new Set<TerrainDesignation>();

            //Timers for Sorting and Terrain manipulation
            m_sortingTimer = new TickTimer();
            m_terrainTimer = new TickTimer();

            //Tower Manager is used to show the tower area along with Mining Tower/Forestry Tower
            m_terrainTowerManager = (Option<TerrainTowerManager>)towersManager;

            //Electricity and Maintenance Consumers during production
            m_electricityConsumer = Context.ElectricityConsumerFactory.CreateConsumer(this);
            Maintenance = maintenanceProvidersFactory.CreateFor(this);

            //Unity Consumer is created when actually boosting
            UnityConsumer = Option.None;

            //Notifications for Missing Designations, Items, and Blocked Outputs
            m_missingDumpItemNotif = context.NotificationsManager.CreateNotificatorFor(CustomIds.Notifications.TerrainTowerMissingDumpItem);
            m_entityBoostedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.EntityIsBoosted);
            m_outputBlockedNotif = context.NotificationsManager.CreateNotificatorFor(CustomIds.Notifications.TerrainTowerBlockedOuput);
            m_missingDesignationNotif = context.NotificationsManager.CreateNotificatorFor(CustomIds.Notifications.TerrainTowerMissingDesignation);
            m_inputBlockedNotif = context.NotificationsManager.CreateNotificatorFor(CustomIds.Notifications.TerrainTowerFullMixedBuffer);

            //Add Calendar to track monthly sorting values
            m_calendar = calendar;
            calendar.NewMonth.Add(this, new Action(onNewMonth));

            SetNewArea(calcIntialArea());
            initData();
        }

        /// <summary>
        /// Add Products to the <see cref="m_productsData"/> to control Sorting/Outputting
        /// </summary>
        /// <param name="product">ProductProto of item to add</param>
        /// <param name="port">char of the Port to control via GUI 'dash' for nothing</param>
        private void addProductDataFor(ProductProto product, ImmutableArray<char> portsNames = default)
        {
            if (portsNames.IsNotEmpty)
            {
                portsNames = portsNames.RemoveAll(p => !m_outputsNames.Contains(p));
            }
            else if (portsNames.IsEmpty)
            {
                char ch = m_outputsNames.FirstOrDefault(x => m_productsData.Values.All(p => !p.OutputPorts.Contains(x)));
                portsNames = ch == char.MinValue ? ImmutableArray<char>.Empty : ImmutableArray.Create(ch);
            }

            GlobalOutputBuffer buffer = new GlobalOutputBuffer(
                Capacity,
                product,
                m_productsManager,
                15,
                this);
            TerrainTowerProductData productData = new TerrainTowerProductData(buffer, portsNames);
            m_productsData.Add(productData.Product, productData);
            m_outputBuffers.Add(productData.Buffer);
        }

        /// <summary>
        /// Calculate initial area for the Tower based on <see cref="TerrainTowerProto"/> static properties
        /// </summary>
        private PolygonTerrainArea2i calcIntialArea()
        {
            RelTile2i initialSize = Prototype.Area.InitialSize;
            Tile2i absolute1 = relAreaToAbsolute(Prototype.Area.Origin - new RelTile2i(0, initialSize.X / 2));
            Tile2i absolute2 = relAreaToAbsolute(Prototype.Area.Origin + new RelTile2i(initialSize.Y, initialSize.X / 2));

            Tile2i tile2i = absolute1.Min(absolute2);
            RelTile2i size = absolute1.Max(absolute2) - tile2i;
            Assert.That(size > RelTile2i.Zero).IsTrue("Zero initial area size creates degenerate polygon area.");

            Tile2i minOnLimits = m_designationManager.TerrainManager.MinOnLimits;
            Tile2i maxOnLimitsExcl = m_designationManager.TerrainManager.MaxOnLimitsExcl;
            RelTile2i rhs = maxOnLimitsExcl - minOnLimits;
            Assert.That(rhs > RelTile2i.Zero).IsTrue("Zero terrain bounds size creates degenerate polygon area.");

            size = size.Min(rhs);
            return PolygonTerrainArea2i.FromRectOriginSize(tile2i.Min(maxOnLimitsExcl - size).Max(minOnLimits), size);

            Tile2i relAreaToAbsolute(RelTile2i tile)
            {
                return Prototype.Layout.Transform(tile, Transform);
            }
        }

        /// <summary>
        /// Called from Constructor and <see cref="initSelf"/>. Sets initial data of calculated, but not user modified values (i.e. SupportedProducts, OutputNames)
        /// </summary>
        private void initData()
        {
            //Set Capacity
            Capacity = Prototype.BufferCapacity;

            DumpTotal = m_dumpBuffers.Sum(buff => buff.Value.Quantity.Value).Quantity();

            //Set all minable products
            AllSupportedProducts = Context.ProtosDb.All<TerrainMaterialProto>().Select(x => (ProductProto)x.MinedProduct).Distinct().ToImmutableArray();

            Random = new Random();

            //Set available Output Ports
            m_outputsNames = Ports.Where(x => x.Type == IoPortType.Output).Select(x => x.Name).OrderBy(x => x).Prepend('-').ToImmutableArray();
        }

        /// <summary>
        /// Intiation on deserialisation, also calls <see cref="initData"/>
        /// </summary>
        [InitAfterLoad(InitPriority.Low)]
        private void initSelf(int saveVersion)
        {
            //Call Universal Init (Whether Construtor or Deserialisation)
            initData();

            if (m_productsData != null)
            {
                //Create tmp Reference Buffers
                m_outputBuffers = m_productsData.Values.Select<TerrainTowerProductData, IProductBuffer>(x => x.Buffer).ToLyst();
                //Mimic SortSortingPlant Entity
                //Update Mixed Quantity

                updateMixedQuantity();
            }
            else
            {
                Logger.Warning("TerrainTowerEntity.initSelf: m_productsData is null");
                MixedTotal = Quantity.Zero;
            }

            updateTerrainNotifications();

            //Mimic Machine Entity
            if (IsBoostRequested)
            {
                UnityConsumer valueOrNull = UnityConsumer.ValueOrNull;
                if (valueOrNull != null && valueOrNull.MonthlyUnity.IsNotPositive)
                {
                    UnityConsumer.Value?.RefreshUnityConsumed();
                }
            }
        }

        /// <summary>
        /// Called on New Month to reset <see cref="TerrainTowerProductData"/> monthly values
        /// </summary>
        private void onNewMonth()
        {
            foreach (TerrainTowerProductData productData in m_productsData.Values) { productData.OnNewMonth(); }
        }

        /// <summary>
        /// Move residual products from <see cref="TerrainTowerProductData.SortedQuantity"/> to <see cref="TerrainTowerProductData.Buffer"/>
        /// </summary>
        private void pushSortedToBuffers()
        {
            m_sortedBufferIsPositive = false;
            bool HasStoredToBuffer = false;
            Assert.AssertTrue(m_productsData != null);
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                if (productData.SortedQuantity.IsNotPositive) { continue; }
                HasStoredToBuffer = true;
                Quantity quantity = productData.MoveSortedQuantityToBuffer();
                if (quantity.IsPositive) { m_productsManager.ProductCreated(productData.Product, quantity, CreateReason.MinedFromTerrain); }
                if (productData.SortedQuantity.IsPositive) { m_sortedBufferIsPositive = true; }
            }
            //If Buffer has been added to, update Full Output Notifications
            if (HasStoredToBuffer) { updateFullOutputNotifications(); }
        }

        /// <summary>
        /// Loop through all <see cref="LayoutEntity.ConnectedOutputPorts"/> and send matching products to the configured <see cref="TerrainTowerProductData.OutputPort"/>
        /// </summary>
        private void sendOutputs()
        {
            foreach (IoPortData oPort in ConnectedOutputPorts)
            {
                foreach (TerrainTowerProductData productData in m_productsData.Values)
                {
                    if (productData.OutputPorts.Contains(oPort.Name))
                    {
                        GlobalOutputBuffer buffer = productData.Buffer;
                        Quantity bufferQuantity = buffer.Quantity;
                        Quantity sentQuantity = bufferQuantity - oPort.SendAsMuchAs(buffer.Product.WithQuantity(bufferQuantity));
                        if (sentQuantity.IsPositive)
                        {
                            buffer.RemoveExactly(sentQuantity);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called from <see cref="IEntityWithSimUpdate.SimUpdate"/>
        /// - Tries to Sort Products and Push to Buffer
        /// - If 'Finished' but no Sorting, don't re-start Timer to allow instant sorting next attempt.
        /// </summary>
        /// <returns>TRUE if Sorting Occured</returns>
        private bool simStepSorting()
        {
            //Sorted Products (Between Mining/Buffer) are Positive = Push to Buffer (Left over from last Timer.IsFinished)
            if (m_sortedBufferIsPositive) { pushSortedToBuffers(); }

            //We only sort if the Timer is Finished (Unsorted -> Sorted)
            if (m_sortingTimer.IsFinished)
            {
                if (sortProducts())
                {
                    //Products have been sorted, start the Timer
                    m_sortingTimer.Start(Prototype.SortDuration);
                    //Products have been sorted, push to Buffer (For output)
                    pushSortedToBuffers();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the current State of the Tower based on current conditions
        /// </summary>
        /// <returns><see cref="State"/> to be assigned to <see cref="CurrentState"/></returns>
        private State simStepUpdateState()
        {
            //Not Enabled = No SimUpdate
            if (IsNotEnabled) { return !Maintenance.Status.IsBroken ? State.Paused : State.Broken; }

            //No Workers = No SimUpdate
            if (IsMissingWorkers(this)) { return State.MissingWorkers; }

            //No Electricity = No SimUpdate
            if (!m_electricityConsumer.CanConsume()) { return State.NotEnoughPower; }

            if (IsMissingDesignation) { return State.MissingDesignation; }

            if (ValidateMining && ValidateDumping)
            {
                return State.CantProcessTerrain;
            }

            //We have Workers, Power, and are Enabled
            //We are not Missing Designation, and can Process Terrain actions
            return State.Working;
        }

        /// <summary>
        /// Process Unsorted Products to Sorted Products and conduct Waste/Conversion Loss for configured products
        /// - Only sort if the 'Sorted Quantity' is Zero
        /// </summary>
        /// <returns>TRUE if any products were Sorted</returns>
        private bool sortProducts()
        {
            //Nothing to Sort
            if (MixedTotal.IsNotPositive) { return false; }

            Quantity sortedPerDuration = SortQuantityPerDuration;

            //Running subtotal of items sorted this round
            Quantity quantitySortedRunningTotal = Quantity.Zero;

            //Summary: Total products availble to be sorted
            Quantity quantityToSortTotal = Quantity.Zero;

            quantityToSortTotal = m_productsData.Values.Where(p => !p.SortedQuantity.IsPositive).Sum(p => p.UnsortedQuantity.Value).Quantity();

            foreach (TerrainTowerProductData plantProductData in m_productsData.Values)
            {
                //If we have sorted all we can, break
                if (quantitySortedRunningTotal >= sortedPerDuration)
                {
                    break;
                }

                //Quantity bufferQuantity = plantProductData.SortedQuantity; added to next IF statement
                if (plantProductData.SortedQuantity.IsNotPositive && plantProductData.UnsortedQuantity.IsPositive)
                {
                    Quantity tmpUnsortedQuantity = plantProductData.UnsortedQuantity;

                    //1. Scale UnsortedQuantity to even ratios with other products
                    tmpUnsortedQuantity = (plantProductData.UnsortedQuantity.Value / quantityToSortTotal.Value.ToFix32() * sortedPerDuration.Value).ToIntCeiled().Quantity();

                    tmpUnsortedQuantity = tmpUnsortedQuantity
                    //2. Min UnsortedQuantity to ensure it doesn't exceed actually available UnsortedQuantity
                        .Min(plantProductData.UnsortedQuantity)
                    //3. Cap to Sorted Products running total remaining value to ensure we're not sorting more than allowed - (Due to rounding, last product looped may have a 1/2 quantity skipped)
                        .Min(sortedPerDuration - quantitySortedRunningTotal);

                    //4. Move from Unsorted to Sorted buffers
                    plantProductData.SortQuantity(tmpUnsortedQuantity);

                    //5. Remove sorted value from mixed buffer - Enumerator prevention
                    MixedTotal -= tmpUnsortedQuantity;
                    updateMixedBufferNotifications();

                    //6. Rolling total of values sorted - after full loop = SortedPerDuration
                    quantitySortedRunningTotal += tmpUnsortedQuantity;

                    if (plantProductData.CanBeWasted)
                    {
                        //7. Add Conversion Loss to 'ToWaste' PartialQuantity buffer
                        plantProductData.ToWaste += tmpUnsortedQuantity.AsPartial.ScaledBy(Prototype.ConversionLoss);

                        //8. Round ToWaste PartialQuantity down to nearest whole number
                        Quantity wasteQuantity = plantProductData.ToWaste.IntegerPart.Min(plantProductData.SortedQuantity);
                        if (wasteQuantity.IsPositive)
                        {
                            //9. Destroy Waste
                            Context.ProductsManager.ProductDestroyed(plantProductData.Product, wasteQuantity, DestroyReason.Wasted);

                            //10. Remove Waste from SortedQuantity
                            plantProductData.SortedQuantity -= wasteQuantity;

                            //11. Remove ConversionLoss from 'ToWaste' should be between 0.00 and 0.99
                            plantProductData.ToWaste -= wasteQuantity.AsPartial;
                        }
                    }
                }
            }
            return quantitySortedRunningTotal.IsPositive;
        }

        private void updateFullOutputNotifications()
        {
            //Blocked Output notification - even when not working
            bool shouldNotify = false;
            if (IsEnabled && m_sortedBufferIsPositive)
            {
                foreach (TerrainTowerProductData productData in m_productsData.Values)
                {
                    if (productData.NotifyIfFullOutput && productData.Buffer.IsFull)
                    {
                        shouldNotify = true;
                        break;
                    }
                }
            }
            m_outputBlockedNotif.NotifyIff(shouldNotify, this);
        }

        private void updateMixedBufferNotifications()
        {
            // BUGFIX: Disable, allow user to set alerts via output full flags.
            //m_inputBlockedNotif.NotifyIff(IsEnabled && MixedTotal >= (Capacity - MAX_MINE_QUANTITY), this);
        }

        /// <summary>
        /// Update the Mixed Quantity from all <see cref="m_productsData"/>
        /// </summary>
        private void updateMixedQuantity()
        {
            Quantity val = Quantity.Zero;
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                val += productData.UnsortedQuantity;
            }
            MixedTotal = val;
            updateMixedBufferNotifications();
        }

        /// <summary>
        /// Activate/Deactivate Notifications. Trigged on Add/Remove/Fulfilled <see cref="ManagedDesignations"/>
        /// </summary>
        private void updateTerrainNotifications()
        {
            //Set unfulfilled flags
            HasUnfulfilledDesignation = m_managedDesignations.Any(desig => desig.IsDumpingNotFulfilled || desig.IsMiningFulfilled);

            //Update Notifications, if Flag is Set && has NotFulfilled Designations
            m_missingDesignationNotif.NotifyIff(!HasUnfulfilledDesignation, this);

            //If Dumping, must have > 0 Quantity. Alternative option: must have at least MAX_DUMP_QUANTITY in a buffer
            //TODO: Remove? - It's set within dumpDesignation which has far more validation/checks, just in case the tower is set to Flatten, but we don't actually have any Dump Designations.
            //m_missingDumpItemNotif.NotifyIff(TerrainConfigState.HasFlag(TerrainTowerConfigState.Dumping) && DumpTotal.IsZero, this);
        }

        protected override void DeserializeData(BlobReader reader)
        {
            base.DeserializeData(reader);
            CurrentState = (State)reader.ReadInt();
            IsBoosted = reader.ReadBool();
            IsBoostRequested = reader.ReadBool();
            m_sortedBufferIsPositive = reader.ReadBool();

            m_prototype = reader.ReadGenericAs<TerrainTowerProto>();

            Area = PolygonTerrainArea2i.Deserialize(reader);

            reader.SetField(this, nameof(m_productsData), Dict<ProductProto, TerrainTowerProductData>.Deserialize(reader));
            reader.SetField(this, nameof(m_dumpBuffers), Dict<ProductProto, GlobalOutputBuffer>.Deserialize(reader));

            reader.SetField(this, nameof(m_productsManager), reader.ReadGenericAs<IProductsManager>());

            m_terrainTowerManager = Option<TerrainTowerManager>.Deserialize(reader);

            reader.SetField(this, nameof(m_designationManager), TerrainDesignationsManager.Deserialize(reader));
            reader.SetField(this, nameof(m_managedDesignations), Set<TerrainDesignation>.Deserialize(reader));
            reader.SetField(this, nameof(m_unfulfilledDumpingDesignations), Set<TerrainDesignation>.Deserialize(reader));
            reader.SetField(this, nameof(m_unfulfilledMiningDesignations), Set<TerrainDesignation>.Deserialize(reader));

            AllSupportedProducts = ImmutableArray<ProductProto>.Deserialize(reader);

            reader.SetField(this, nameof(m_calendar), reader.ReadGenericAs<ICalendar>());
            reader.SetField(this, nameof(m_electricityConsumer), reader.ReadGenericAs<IElectricityConsumer>());
            UnityConsumer = Option<UnityConsumer>.Deserialize(reader);
            Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();

            reader.SetField(this, nameof(m_sortingTimer), TickTimer.Deserialize(reader));
            reader.SetField(this, nameof(m_terrainTimer), TickTimer.Deserialize(reader));

            m_entityBoostedNotif = EntityNotificator.Deserialize(reader);
            m_missingDumpItemNotif = EntityNotificator.Deserialize(reader);
            m_missingDesignationNotif = EntityNotificator.Deserialize(reader);
            m_outputBlockedNotif = EntityNotificator.Deserialize(reader);
            m_inputBlockedNotif = EntityNotificator.Deserialize(reader);

            reader.RegisterInitAfterLoad(this, nameof(initSelf), InitPriority.Normal);
        }

        /// <summary>
        /// Clean up Entity when Destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            //Remove Designation Manager Events
            m_designationManager.DesignationAdded.Remove(this, new Action<TerrainDesignation>(onDesignationAdded));
            m_designationManager.DesignationRemoved.Remove(this, new Action<TerrainDesignation>(onDesignationRemoved));
            m_designationManager.DesignationFulfilledChanged.Remove(this, new Action<TerrainDesignation>(onDesignationFulfilledChanged));
            removeAllManagedDesignations();
            m_terrainTowerManager = Option<TerrainTowerManager>.None;

            m_calendar.NewMonth.Remove(this, new Action(onNewMonth));
            //Remove all products from m_productsData
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                Context.ProductsManager.ProductDestroyed(productData.Buffer.Product, productData.UnsortedQuantity + productData.SortedQuantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(productData.Buffer);
            }
            foreach (GlobalOutputBuffer dumpBuffer in m_dumpBuffers.Values)
            {
                Context.ProductsManager.ProductDestroyed(dumpBuffer.Product, dumpBuffer.Quantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(dumpBuffer);
            }

            //Disable Notifications
            m_entityBoostedNotif.Deactivate(this);
            m_missingDumpItemNotif.Deactivate(this);
            m_missingDesignationNotif.Deactivate(this);
            m_outputBlockedNotif.Deactivate(this);

            base.OnDestroy();
        }

        /// <summary>
        /// Used to Start/Stop processes and external events when Entity is Paused/Restarted
        /// </summary>
        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (IsEnabled)
            {
                //Start Processes
            }
            else
            {
                //End Processes
            }
        }

        protected override void SerializeData(BlobWriter writer)
        {
            //TODO: FINISH SERIALISATION
            base.SerializeData(writer);

            writer.WriteInt((int)CurrentState);
            writer.WriteBool(IsBoosted);
            writer.WriteBool(IsBoostRequested);
            writer.WriteBool(m_sortedBufferIsPositive);

            writer.WriteGeneric(m_prototype);

            PolygonTerrainArea2i.Serialize(Area, writer);

            Dict<ProductProto, TerrainTowerProductData>.Serialize(m_productsData, writer);
            Dict<ProductProto, GlobalOutputBuffer>.Serialize(m_dumpBuffers, writer);

            writer.WriteGeneric(m_productsManager);

            Option<TerrainTowerManager>.Serialize(m_terrainTowerManager, writer);

            TerrainDesignationsManager.Serialize(m_designationManager, writer);
            Set<TerrainDesignation>.Serialize(m_managedDesignations, writer);
            Set<TerrainDesignation>.Serialize(m_unfulfilledDumpingDesignations, writer);
            Set<TerrainDesignation>.Serialize(m_unfulfilledMiningDesignations, writer);

            ImmutableArray<ProductProto>.Serialize(AllSupportedProducts, writer);

            writer.WriteGeneric(m_calendar);
            writer.WriteGeneric(m_electricityConsumer);
            Option<UnityConsumer>.Serialize(UnityConsumer, writer);
            writer.WriteGeneric(Maintenance);

            TickTimer.Serialize(m_sortingTimer, writer);
            TickTimer.Serialize(m_terrainTimer, writer);

            EntityNotificator.Serialize(m_entityBoostedNotif, writer);
            EntityNotificator.Serialize(m_missingDumpItemNotif, writer);
            EntityNotificator.Serialize(m_missingDesignationNotif, writer);
            EntityNotificator.Serialize(m_outputBlockedNotif, writer);
            EntityNotificator.Serialize(m_inputBlockedNotif, writer);
        }

        /// <summary>
        /// Add a Product to the Tower for Sorting/Output. Used automatically via SimUpdate - Vanilla by GUI
        /// </summary>
        /// <param name="product">ProductProto to add</param>
        /// <param name="port">optional port to assign</param>
        internal void AddProductToSort(ProductProto product, ImmutableArray<char> ports = default)
        {
            if (!m_productsData.ContainsKey(product)
                && m_productsData.Count < TerrainTowerProto.MAX_PRODUCTS
                && AllSupportedProducts.Contains(product))
            {
                addProductDataFor(product, ports);
            }
        }

        /// <summary>
        /// Set new Area for the Tower, controlled via <see cref="TerrainTowerInspector"/> from the GUI. Snaps outwards to Terrain Designations.
        /// </summary>
        /// <param name="newArea">Area to Set</param>
        internal void EditManagedArea(RectangleTerrainArea2i newArea)
        {
            removeAllManagedDesignations();
            PolygonTerrainArea2i oldArea = Area;
            Tile2i origin = TerrainDesignation.GetOrigin(newArea.Origin);
            Tile2i tile2i = newArea.PlusXyCoordExcl;
            tile2i = tile2i.AddX(newArea.Size.X == 0 ? 0 : -1);
            Tile2i p2 = TerrainDesignation.GetOrigin(tile2i.AddY(newArea.Size.Y == 0 ? 0 : -1)) + TerrainDesignation.Size - 1;

            //TODO: Is needed? Base MineTower doesn't use this function
            Area = PolygonTerrainArea2i.FromRectArea(RectangleTerrainArea2i.FromTwoPositions(origin, p2));

            foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>)m_designationManager.Designations)
            {
                onDesignationAdded(designation);
            }

            m_terrainTowerManager.ValueOrNull?.InvokeOnAreaChanged(this, oldArea);
        }

        /// <summary>
        /// Gets all Mixed/Mined products from <see cref="m_productsData"/> to provide to GUI
        /// </summary>
        /// <param name="result">Lyst of ProductQuantity</param>
        internal void GetMixedInputProducts(Lyst<ProductQuantity> result)
        {
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                if (productData.UnsortedQuantity.IsPositive)
                {
                    result.Add(productData.Product.WithQuantity(productData.UnsortedQuantity));
                }
            }
        }

        /// <summary>
        /// Gets all Mixed products from <see cref="m_dumpBuffers"/> to provide to GUI
        /// </summary>
        /// <param name="result"
        ///
        internal void GetMixedOutputProducts(Lyst<ProductQuantity> result)
        {
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                if (productData.UnsortedQuantity.IsPositive)
                {
                    result.Add(productData.Product.WithQuantity(productData.UnsortedQuantity));
                }
            }
        }

        internal bool GetQuickRemoveCost(out Upoints unity, Quantity quantity)
        {
            unity = Upoints.Zero;
            if (quantity.IsNotPositive) return false;
            unity = QuantityToUnityCost(quantity.Value, Context.UpointsManager.QuickActionCostMultiplier) ?? Upoints.Zero;
            return Context.UpointsManager.CanConsume(unity);
            //QuickDeliverCostHelper
            Upoints? QuantityToUnityCost(int Quantity, Percent Multiplier)
            {
                if (Quantity <= 0)
                {
                    return new Upoints?();
                }
                if (Quantity > Fix32.MaxValue)
                {
                    Quantity = Fix32.MaxIntValue.IntegerPart;
                }

                //TODO: Check if this is the correct formula, Max(0.1) would cap it to 0.1 Upoints
                return new Upoints?(
                    ((((0.47.ToFix32() * Quantity.Sqrt()) - 0.8.ToFix32()) * 10.ToFix32()).ToIntRounded() / 10.ToFix32())
                    .Upoints()
                    .ScaledBy(Multiplier)
                    .Max(0.1.Upoints()));
            }
        }

        /// <summary>
        /// Reads <see cref="m_productsData"/> to get Quantity Sorted"/>
        /// </summary>
        /// <param name="product">Product Proto to read</param>
        /// <returns>Quantity Sorted</returns>
        internal Quantity GetSortedLastMonth(ProductProto product) => m_productsData.TryGetValue(product, out TerrainTowerProductData productData) ? productData.SortedLastMonth : Quantity.Zero;

        /// <summary>
        /// Tries to confirm if the ProductBlockAlert is set for the ProductProto
        /// </summary>
        /// <param name="product">Product Proto to check</param>
        /// <returns>TRUE if Full Output Notification set</returns>
        internal bool IsProductBlockedAlertEnabled(ProductProto product) => m_productsData.TryGetValue(product, out TerrainTowerProductData plantProductData) && plantProductData.NotifyIfFullOutput;

        /// <summary>
        /// Remove a product from the Tower and destroys any remaining product. Used automatically via SimUpdate - Vanilla by GUI
        /// </summary>
        /// <param name="product"></param>
        internal void RemoveProductToSort(ProductProto product)
        {
            if (m_productsData.TryRemove(product, out TerrainTowerProductData plantProductData))
            {
                m_outputBuffers.RemoveAndAssert(plantProductData.Buffer);
                Context.ProductsManager.ProductDestroyed(plantProductData.Product, plantProductData.SortedQuantity + plantProductData.UnsortedQuantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(plantProductData.Buffer);
                MixedTotal -= plantProductData.UnsortedQuantity;
                updateMixedBufferNotifications();
            }
        }

        /// <summary>
        /// Set the Product to notify if the Output is full
        /// </summary>
        /// <param name="product">Product to Notify</param>
        /// <param name="isAlertEnabled">bool supplied by User via GUI</param>
        internal void SetProductBlockedAlert(ProductProto product, bool isAlertEnabled)
        {
            if (m_productsData.TryGetValue(product, out TerrainTowerProductData productData))
            {
                productData.NotifyIfFullOutput = isAlertEnabled;
            }
        }

        public static TerrainTowerEntity Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out TerrainTowerEntity terrainTower))
            {
                reader.EnqueueDataDeserialization(terrainTower, s_deserializeDataDelayedAction);
            }
            return terrainTower;
        }

        public static void Serialize(TerrainTowerEntity value, BlobWriter writer)
        {
            if (writer.TryStartClassSerialization(value))
            {
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }
        }

        public void AddProductPortIndex(ProductProto product, int index)
        {
            if (index >= 0
                && index < m_outputsNames.Length
                && m_productsData.TryGetValue(product, out TerrainTowerProductData towerProductData)
                && !towerProductData.OutputPorts.Contains(m_outputsNames[index]))
            {
                towerProductData.OutputPorts = towerProductData.OutputPorts.Add(m_outputsNames[index]);
            }
        }

        /// <summary>
        /// Gets all Products from <see cref="m_productsData"/> for a particular port
        /// - Used within <see cref="TerrainTowerPortProductResolver"/>
        /// </summary>
        /// <param name="port"></param>
        /// <returns>List of Products for an Output Port</returns>
        public ImmutableArray<ProductProto> GetPortProducts(IoPort port) => m_productsData.Values.Where(x => x.OutputPorts.Contains(port.Name)).Select(x => x.Buffer.Product).ToImmutableArray();

        public IEnumerable<ProductProto> GetPortProducts(int portIndex)
        {
            if (portIndex >= 0 && portIndex < m_outputsNames.Length)
            {
                char portName = m_outputsNames[portIndex];
                return m_productsData.Values
                    .Where(x => x.OutputPorts.Contains(portName))
                    .Select(x => x.Buffer.Product);
            }
            else
            {
                return Enumerable.Empty<ProductProto>();
            }
        }

        public bool IsPortSetForProduct(ProductProto product) => m_productsData.TryGetValue(product, out TerrainTowerProductData towerProductData) && towerProductData.OutputPorts.Length > 0;

        //TODO: Add quick remove functionality to view
        public bool QuickRemoveProduct(ProductQuantity productQuantity)
        {
            if (!GetQuickRemoveCost(out Upoints unity, productQuantity.Quantity) || unity.IsNotPositive) return false;

            Context.UpointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickRemove, unity);
            Context.AssetTransactionManager.StoreClearedProduct(productQuantity);
            return true;
        }

        /// <summary>
        /// Receive products to <see cref="m_dumpBuffer"/> creating new buffer entry if not already present
        /// </summary>
        /// <param name="pq">Product/Quantity to receive</param>
        /// <param name="sourcePort">Receive from port token</param>
        /// <returns>Quantity that could not fit</returns>

        public void RemoveProductPortIndex(ProductProto product, int index)
        {
            if (index >= 0
                && index < m_outputsNames.Length
                && m_productsData.TryGetValue(product, out TerrainTowerProductData plantProductData)
                && plantProductData.OutputPorts.IndexOf(m_outputsNames[index]) >= 0)
            {
                plantProductData.OutputPorts = plantProductData.OutputPorts.RemoveAt(plantProductData.OutputPorts.IndexOf(m_outputsNames[index]));
            }
        }

        public void SetNewArea(PolygonTerrainArea2i area)
        {
            removeAllManagedDesignations();
            PolygonTerrainArea2i oldArea = Area;
            Area = area;
            foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>)m_designationManager.Designations)
            {
                onDesignationAdded(designation);
            }
            if (!m_terrainTowerManager.HasValue) return;
            m_terrainTowerManager.Value.InvokeOnAreaChanged(this, oldArea);
        }

        Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
        {
            //Receive only if enabled
            if (IsNotEnabled) return pq.Quantity;

            //Buffer doesn't exist, create and add to m_dumpBuffers
            if (!m_dumpBuffers.TryGetValue(pq.Product, out GlobalOutputBuffer dumpBuffer))
            {
                dumpBuffer = new GlobalOutputBuffer(
                    Capacity,
                    pq.Product,
                    m_productsManager,
                    15,
                    this);
                if (m_dumpBuffers.TryAdd(pq.Product, dumpBuffer))
                {
                    //Successfully Added
                }
                else
                {
                    //Fail
                }
            }
            else if (dumpBuffer.IsFull)
            {
                return pq.Quantity;
            }

            Quantity quant = dumpBuffer.StoreAsMuchAs(pq.Quantity);
            DumpTotal += pq.Quantity - quant;
            return quant;
        }

        /// <summary>
        /// Set if Boost is Requested, and create a UnityConsumer if not already present
        /// </summary>
        /// <param name="isBoosted">TRUE/FALSE for if a Boost is to be Enabled or Disabeld</param>
        void IEntityWithBoost.SetBoosted(bool isBoosted)
        {
            if (isBoosted && !BoostCost.HasValue)
            {
                Assert.Fail(string.Format("Cannot boost '{0}', not allowed!", Prototype.Id));
            }
            m_entityBoostedNotif.NotifyIff(isBoosted, this);
            IsBoostRequested = isBoosted;

            if (IsBoostRequested && UnityConsumer.IsNone)
            {
                UnityConsumer = Context.UnityConsumerFactory.CreateConsumer(this);
            }
            else if (!IsBoostRequested && UnityConsumer.HasValue)
            {
                UnityConsumer.Value.Destroy();
                UnityConsumer = Option.None;
            }

            if (UnityConsumer.HasValue && UnityConsumer.Value.MonthlyUnity.IsNotPositive)
            {
                UnityConsumer.Value.RefreshUnityConsumed();
                Assert.That(UnityConsumer.Value.MonthlyUnity).IsPositive();
            }
        }

        /// <summary>
        /// Process SimUpdate including UpdateState, NotifyIfFullOutput, Mine/Dump, and SendOutputs
        /// </summary>
        void IEntityWithSimUpdate.SimUpdate()
        {
            CurrentState = simStepUpdateState();
            /*
            Paused              - No Work
            Broken              - No Work
            MissingWorkers      - No Work
            MissingDumpResource - Sorting
            NotEnoughPower      - Sorting
            MissingDesignation  - Sorting
            Working             - Terrain Work & Sorting
            */

            bool NoWork = CurrentState == State.Paused || CurrentState == State.Broken || CurrentState == State.MissingWorkers;
            bool CanTerrain = CurrentState == State.Working;
            bool CanSort = CurrentState == State.Working || CurrentState == State.CantProcessTerrain || CurrentState == State.MissingDesignation;
            bool CanOutput = !NoWork;

            //Decrement Timers i.e. Actually count down Time (Not automatic)
            if (m_sortingTimer.IsNotFinished) { m_sortingTimer.DecrementOnly(); }
            if (m_terrainTimer.IsNotFinished) { m_terrainTimer.DecrementOnly(); }

            //Absolutly no work can be done if the Tower is Paused, Broken, or Missing Workers
            if (NoWork)
            {
                return;
            }

            if (CanTerrain)
            {
                //Boosted and Can Work = Boosted
                IsBoosted = IsBoostRequested && (UnityConsumer.ValueOrNull?.CanWork() ?? true);

                if (m_electricityConsumer.TryConsume())
                {
                    //If everything is working, and we have power, continue
                    if (simStepTerrainProcessing())
                    {
                        //Terrain Actions occured
                    }
                }
            }

            //Sorting after Terrain Processing to allow in-line Mining->Sorting->Output
            //Sorting does not need Power
            if (CanSort)
            {
                if (simStepSorting())
                {
                    //STEP CORRECT
                    //TODO: Additional logic? or Void?
                }
            }

            if (CanOutput)
            {
                sendOutputs();
            }
        }
    }
}