// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTower
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_assignedOutputStoragesOld", 112, typeof (Set<Storage>), 107, false)]
  public class ForestryTower : 
    LayoutEntity,
    IAreaManagingTower,
    IEntity,
    IIsSafeAsHashKey,
    IAssignableToFuelStation,
    IEntityAssignedAsInput,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityAssignedAsOutput,
    IEntityAssignedWithVehicles,
    IEntityWithCloneableConfig
  {
    internal static readonly Percent NO_CUT_AT;
    public readonly ForestryTowerProto Prototype;
    internal readonly ITreesManager m_treeManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly Set<IEntityAssignedAsOutput> m_assignedOutputEntities;
    [NewInSaveVersion(112, null, "new()", null, null)]
    private readonly Set<IEntityAssignedAsInput> m_assignedInputEntities;
    private readonly Set<FuelStation> m_assignedFuelStations;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IEntityAssignedAsInput> m_assignedInputStorages;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<Storage> m_assignedOutputStorages;
    private readonly Set<TerrainDesignation> m_managedDesignations;
    /// <summary>All trees managed by this tower.</summary>
    private readonly Set<TreeId> m_trees;
    /// <summary>All stumps in this tower's area.</summary>
    [NewInSaveVersion(98, null, "new()", null, null)]
    private readonly Set<TreeId> m_stumps;
    /// <summary>Trees that can be cut by this tower.</summary>
    private readonly Set<TreeId> m_cuttableTrees;
    private Lyst<KeyValuePair<TreePlantingGroupProto, int>> m_treeTypes;
    private Percent m_targetHarvestPercent;
    private readonly AssignedVehicles<TreePlanter, TreePlanterProto> m_treePlanters;
    private readonly AssignedVehicles<TreeHarvester, TreeHarvesterProto> m_treeHarvesters;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Vehicle> m_allVehicles;
    internal readonly ITerrainDesignationsManager m_designationManager;
    private readonly TerrainManager m_terrainManager;
    private readonly ICalendar m_calendar;
    private readonly ProtosDb m_protosDb;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private ForestryTowersManager m_manager;
    private IRandom m_randomNumberGenerator;
    private EntityNotificator m_noForestryDesignationsNotif;
    private bool m_treesCheckedThisTick;
    private bool m_designationsDirty;
    private ForestryTower.ForestPlantingManager m_forestPlantingManager;
    [DoNotSaveCreateNewOnLoad("new()", 0)]
    private readonly Lyst<TreeId> m_treesToRemove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputs
    {
      get => (IReadOnlySet<IEntityAssignedAsOutput>) this.m_assignedOutputEntities;
    }

    public bool AllowNonAssignedOutput { get; set; }

    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputs
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputEntities;
    }

    public IReadOnlySet<FuelStation> AssignedFuelStations
    {
      get => (IReadOnlySet<FuelStation>) this.m_assignedFuelStations;
    }

    /// <summary>
    /// Storages that accept stuff from this tower as their input.
    /// </summary>
    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputStorages
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputStorages;
    }

    public IReadOnlySet<Storage> AssignedOutputStorages
    {
      get => (IReadOnlySet<Storage>) this.m_assignedOutputStorages;
    }

    /// <summary>Designations managed by this tower.</summary>
    public IReadOnlySet<TerrainDesignation> ManagedDesignations
    {
      get => (IReadOnlySet<TerrainDesignation>) this.m_managedDesignations;
    }

    public IReadOnlySet<TreeId> Trees => (IReadOnlySet<TreeId>) this.m_trees;

    public IReadOnlySet<TreeId> Stumps => (IReadOnlySet<TreeId>) this.m_stumps;

    /// <summary>
    /// The type of tree to plant, and the amount of instances of type to calculate a ratio.
    /// </summary>
    public IIndexable<KeyValuePair<TreePlantingGroupProto, int>> TreeTypes
    {
      get => (IIndexable<KeyValuePair<TreePlantingGroupProto, int>>) this.m_treeTypes;
    }

    /// <summary>
    /// Percent growth above which we should harvest the tree.
    /// </summary>
    public Percent TargetHarvestPercent => this.m_targetHarvestPercent;

    /// <summary>Area that is controlled by the tower.</summary>
    public RectangleTerrainArea2i Area { get; private set; }

    /// <summary>Total number of assigned tree planters.</summary>
    public int TreePlantersTotal => this.m_treePlanters.Count;

    /// <summary>Total number of assigned tree harvesters.</summary>
    public int TreeHarvestersTotal => this.m_treeHarvesters.Count;

    public IIndexable<Vehicle> AllVehicles => (IIndexable<Vehicle>) this.m_allVehicles;

    public ForestryTower(
      EntityId id,
      ForestryTowerProto forestryTowerProto,
      TileTransform transform,
      EntityContext context,
      ITreesManager treeManager,
      IVehiclesManager vehiclesManager,
      ITerrainDesignationsManager designationManager,
      TerrainManager terrainManager,
      ISimLoopEvents simLoopEvents,
      ICalendar calendar,
      ForestryTowersManager manager,
      ProtosDb protosDb,
      RandomProvider randomProvider,
      TerrainOccupancyManager occupancyManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedOutputEntities = new Set<IEntityAssignedAsOutput>();
      this.m_assignedInputEntities = new Set<IEntityAssignedAsInput>();
      this.m_assignedFuelStations = new Set<FuelStation>();
      this.m_assignedInputStorages = new Set<IEntityAssignedAsInput>();
      this.m_assignedOutputStorages = new Set<Storage>();
      this.m_managedDesignations = new Set<TerrainDesignation>();
      this.m_trees = new Set<TreeId>();
      this.m_stumps = new Set<TreeId>();
      this.m_cuttableTrees = new Set<TreeId>();
      this.m_treeTypes = new Lyst<KeyValuePair<TreePlantingGroupProto, int>>();
      this.m_allVehicles = new Lyst<Vehicle>();
      this.m_treesToRemove = new Lyst<TreeId>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) forestryTowerProto, transform, context);
      this.Prototype = forestryTowerProto;
      this.m_treeManager = treeManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_designationManager = designationManager;
      this.m_terrainManager = terrainManager;
      this.m_calendar = calendar;
      this.m_protosDb = protosDb;
      this.m_occupancyManager = occupancyManager;
      this.m_manager = manager;
      this.m_randomNumberGenerator = randomProvider.GetSimRandomFor((object) this);
      this.m_noForestryDesignationsNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoForestryDesignInTowerArea);
      this.m_designationManager.DesignationAdded.Add<ForestryTower>(this, new Action<TerrainDesignation>(this.designationAdded));
      this.m_designationManager.DesignationRemoved.Add<ForestryTower>(this, new Action<TerrainDesignation>(this.designationRemoved));
      this.m_treePlanters = new AssignedVehicles<TreePlanter, TreePlanterProto>((IEntityAssignedWithVehicles) this);
      this.m_treeHarvesters = new AssignedVehicles<TreeHarvester, TreeHarvesterProto>((IEntityAssignedWithVehicles) this);
      this.m_forestPlantingManager = new ForestryTower.ForestPlantingManager(this, randomProvider);
      this.updateNotifications();
      this.calcInitialArea();
      this.m_targetHarvestPercent = Percent.Hundred;
      simLoopEvents.UpdateEnd.Add<ForestryTower>(this, new Action(this.simUpdateEnd));
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion)
    {
      if (this.IsDestroyed && this.m_assignedInputEntities.IsNotEmpty)
        this.m_assignedInputEntities.ForEachAndClear((Action<IEntityAssignedAsInput>) (x => x.UnassignStaticOutputEntity((IEntityAssignedAsOutput) this)));
      if (saveVersion < 123)
        this.m_designationsDirty = true;
      foreach (IEntityAssignedAsOutput assignedOutputEntity in this.m_assignedOutputEntities)
      {
        if (assignedOutputEntity is FuelStation fuelStation)
          this.m_assignedFuelStations.Add(fuelStation);
        if (assignedOutputEntity is Storage storage)
          this.m_assignedOutputStorages.Add(storage);
      }
      foreach (IEntityAssignedAsInput assignedInputEntity in this.m_assignedInputEntities)
      {
        if (assignedInputEntity is Storage storage)
          this.m_assignedInputStorages.Add((IEntityAssignedAsInput) storage);
      }
      this.updateAssignedVehicles();
    }

    private void simUpdateEnd()
    {
      if (this.m_designationsDirty)
      {
        this.resetTreeState();
        this.updateNotifications();
        this.m_designationsDirty = false;
      }
      this.m_treesCheckedThisTick = false;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsNotEnabled)
      {
        foreach (Vehicle vehicle in this.m_treePlanters.All)
          vehicle.CancelAllJobsAndResetState();
        foreach (Vehicle vehicle in this.m_treeHarvesters.All)
          vehicle.CancelAllJobsAndResetState();
      }
      this.updateNotifications();
    }

    protected override void OnDestroy()
    {
      foreach (Vehicle vehicle in this.m_allVehicles.ToArray())
        this.UnassignVehicle(vehicle, true);
      this.m_designationManager.DesignationAdded.Remove<ForestryTower>(this, new Action<TerrainDesignation>(this.designationAdded));
      this.m_designationManager.DesignationRemoved.Remove<ForestryTower>(this, new Action<TerrainDesignation>(this.designationRemoved));
      this.m_assignedOutputEntities.ForEachAndClear((Action<IEntityAssignedAsOutput>) (x => x.UnassignStaticInputEntity((IEntityAssignedAsInput) this)));
      this.m_assignedInputEntities.ForEachAndClear((Action<IEntityAssignedAsInput>) (x => x.UnassignStaticOutputEntity((IEntityAssignedAsOutput) this)));
      this.m_assignedFuelStations.Clear();
      this.m_assignedOutputStorages.Clear();
      this.m_assignedInputStorages.Clear();
      foreach (TerrainDesignation designation in this.m_managedDesignations.ToArray())
      {
        this.designationRemoved(designation);
        if (designation.ManagedByTowers.IsEmpty())
          this.m_designationManager.RemoveDesignation(designation.OriginTileCoord);
      }
      Assert.That<Set<TerrainDesignation>>(this.m_managedDesignations).IsEmpty<TerrainDesignation>();
      base.OnDestroy();
    }

    public bool CanBeAssignedWithOutput(IEntityAssignedAsOutput entity)
    {
      if (this.m_assignedOutputEntities.Contains(entity))
        return false;
      switch (entity)
      {
        case FuelStation _:
          return true;
        case Storage storage:
          ProductType? productType = storage.Prototype.ProductType;
          if (!productType.HasValue)
            return true;
          productType = storage.Prototype.ProductType;
          return productType.Value == CountableProductProto.ProductType;
        default:
          return false;
      }
    }

    void IEntityAssignedAsInput.AssignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      if (!this.CanBeAssignedWithOutput(entity))
        return;
      this.m_assignedOutputEntities.Add(entity);
      if (entity is FuelStation fuelStation)
        this.m_assignedFuelStations.Add(fuelStation);
      if (!(entity is Storage storage))
        return;
      ProductType? productType = storage.Prototype.ProductType;
      int num;
      if (productType.HasValue)
      {
        productType = storage.Prototype.ProductType;
        num = productType.Value == CountableProductProto.ProductType ? 1 : 0;
      }
      else
        num = 1;
      Assert.That<bool>(num != 0).IsTrue();
      this.m_assignedOutputStorages.Add(storage);
    }

    void IEntityAssignedAsInput.UnassignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      this.m_assignedOutputEntities.Remove(entity);
      if (entity is FuelStation fuelStation)
        this.m_assignedFuelStations.Remove(fuelStation);
      if (!(entity is Storage storage))
        return;
      this.m_assignedOutputStorages.Remove(storage);
    }

    public bool CanBeAssignedWithInput(IEntityAssignedAsInput entity)
    {
      if (this.m_assignedInputEntities.Contains(entity) || !(entity is Storage storage))
        return false;
      ProductType? productType = storage.Prototype.ProductType;
      if (!productType.HasValue)
        return true;
      productType = storage.Prototype.ProductType;
      return productType.Value == CountableProductProto.ProductType;
    }

    public void AssignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      if (!this.CanBeAssignedWithInput(entity))
        return;
      this.m_assignedInputEntities.Add(entity);
      if (!(entity is Storage storage))
        return;
      ProductType? productType = storage.Prototype.ProductType;
      int num;
      if (productType.HasValue)
      {
        productType = storage.Prototype.ProductType;
        num = productType.Value == CountableProductProto.ProductType ? 1 : 0;
      }
      else
        num = 1;
      Assert.That<bool>(num != 0).IsTrue();
      this.m_assignedInputStorages.Add((IEntityAssignedAsInput) storage);
    }

    public void UnassignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      this.m_assignedInputEntities.Remove(entity);
      if (!(entity is Storage storage))
        return;
      this.m_assignedInputStorages.Remove((IEntityAssignedAsInput) storage);
    }

    public bool TryAddTree(TreeData treeData)
    {
      TreeId id = treeData.Id;
      if (!this.Area.Contains(treeData.Position))
        return false;
      TerrainDesignation valueOrNull = this.m_designationManager.GetDesignationAt((Tile2i) id.Position).ValueOrNull;
      if (valueOrNull == null || !valueOrNull.IsForestry)
        return false;
      this.m_trees.Add(id);
      if (this.IsTreeReadyForHarvest(id))
        this.m_cuttableTrees.Add(id);
      this.m_forestPlantingManager.AddTree(id);
      return true;
    }

    public bool TryRemoveTree(TreeData treeData)
    {
      TreeId id = treeData.Id;
      if (!this.m_trees.Remove(id))
        return false;
      this.m_cuttableTrees.Remove(id);
      this.m_forestPlantingManager.RemoveTree(id);
      return true;
    }

    public bool TryAddStump(TreeStumpData stumpData)
    {
      TreeId id = stumpData.Id;
      if (!this.Area.Contains(stumpData.Position))
        return false;
      TerrainDesignation valueOrNull = this.m_designationManager.GetDesignationAt((Tile2i) id.Position).ValueOrNull;
      if (valueOrNull == null || !valueOrNull.IsForestry)
        return false;
      this.m_stumps.Add(id);
      this.m_forestPlantingManager.AddStump(id);
      return true;
    }

    public bool TryRemoveStump(TreeStumpData stumpData)
    {
      TreeId id = stumpData.Id;
      if (!this.m_stumps.Remove(id))
        return false;
      this.m_forestPlantingManager.RemoveStump(id);
      return true;
    }

    private void resetTreeState()
    {
      this.m_trees.Clear();
      this.m_stumps.Clear();
      this.m_cuttableTrees.Clear();
      foreach (TreeId key in this.m_treeManager.EnumerateTreesInArea(this.Area))
      {
        TreeData treeData;
        if (!this.m_treeManager.Trees.TryGetValue(key, out treeData))
          Log.Error(string.Format("no treeData for {0}", (object) key));
        else
          this.TryAddTree(treeData);
      }
      foreach (TreeId key in this.m_treeManager.EnumerateStumpsInArea(this.Area))
      {
        TreeStumpData stumpData;
        if (!this.m_treeManager.Stumps.TryGetValue(key, out stumpData))
          Log.Error(string.Format("no treeStumpData for {0}", (object) key));
        else
          this.TryAddStump(stumpData);
      }
    }

    /// <summary>
    /// Updates the tower's internal tracking of controlled trees and controlled trees
    /// that are ready to harvest.
    /// </summary>
    private void updateTreeState()
    {
      if (this.m_treesCheckedThisTick)
        return;
      this.m_treesCheckedThisTick = true;
      this.m_treesToRemove.Clear();
      foreach (TreeId tree in this.m_trees)
      {
        if (!this.m_treeManager.HasTree(tree))
        {
          this.m_treesToRemove.Add(tree);
          this.m_cuttableTrees.Remove(tree);
        }
        else
        {
          bool flag = this.m_cuttableTrees.Contains(tree);
          if (flag && !this.IsTreeReadyForHarvest(tree))
            this.m_cuttableTrees.Remove(tree);
          else if (!flag && this.IsTreeReadyForHarvest(tree))
            this.m_cuttableTrees.Add(tree);
        }
      }
      foreach (TreeId treeId in this.m_treesToRemove)
        this.m_trees.Remove(treeId);
    }

    public void SetNewArea(RectangleTerrainArea2i area)
    {
      this.cleanManagedDesignations();
      this.m_forestPlantingManager.Reset();
      RectangleTerrainArea2i area1 = this.Area;
      Tile2i origin = TerrainDesignation.GetOrigin(area.Origin);
      Tile2i tile2i = area.PlusXyCoordExcl;
      tile2i = tile2i.AddX(area.Size.X == 0 ? 0 : -1);
      Tile2i p2 = TerrainDesignation.GetOrigin(tile2i.AddY(area.Size.Y == 0 ? 0 : -1)) + TerrainDesignation.Size - 1;
      this.Area = RectangleTerrainArea2i.FromTwoPositions(origin, p2);
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) this.m_designationManager.Designations)
        this.designationAdded(designation);
      this.resetTreeState();
      this.updateNotifications();
      this.m_manager.InvokeOnAreaChanged(this, area1);
    }

    private void calcInitialArea()
    {
      RelTile2i origin = this.Prototype.Area.Origin;
      RelTile2i relTile2i1 = new RelTile2i(0, this.Prototype.Area.InitialSize.X / 2);
      RelTile2i relTile2i2 = new RelTile2i(this.Prototype.Area.InitialSize.Y, 0);
      Tile2i absolute1 = this.relAreaToAbsolute(origin + relTile2i1);
      Tile2i absolute2 = this.relAreaToAbsolute(origin + relTile2i2 + relTile2i1);
      Tile2i absolute3 = this.relAreaToAbsolute(origin + relTile2i2 - relTile2i1);
      Tile2i absolute4 = this.relAreaToAbsolute(origin - relTile2i1);
      Tile2i tile2i = absolute1.Min(absolute2);
      tile2i = tile2i.Min(absolute3);
      this.SetNewArea(new RectangleTerrainArea2i(tile2i.Min(absolute4), this.Prototype.Area.InitialSize));
    }

    private Tile2i relAreaToAbsolute(RelTile2i tile)
    {
      return this.Prototype.Layout.Transform(tile, this.Transform);
    }

    private void designationAdded(TerrainDesignation designation)
    {
      if (!designation.IsForestry || !this.isWithinArea(designation))
        return;
      designation.AddManagingTower((IAreaManagingTower) this);
      this.m_managedDesignations.AddAndAssertNew(designation);
      this.m_forestPlantingManager.AddDesignation(designation);
      this.m_designationsDirty = true;
    }

    private void designationRemoved(TerrainDesignation designation)
    {
      if (!designation.IsForestry || !this.m_managedDesignations.Remove(designation))
        return;
      designation.RemoveManagingTower((IAreaManagingTower) this);
      this.m_forestPlantingManager.RemoveDesignation(designation);
      this.m_designationsDirty = true;
    }

    private void cleanManagedDesignations()
    {
      foreach (TerrainDesignation designation in this.m_managedDesignations.ToArray())
        this.designationRemoved(designation);
      Assert.That<Set<TerrainDesignation>>(this.m_managedDesignations).IsEmpty<TerrainDesignation>();
    }

    private bool isWithinArea(TerrainDesignation designation)
    {
      return this.Area.ContainsTile(designation.OriginTileCoord);
    }

    private void updateNotifications()
    {
      this.m_noForestryDesignationsNotif.NotifyIff(this.IsEnabled && this.m_managedDesignations.Count == 0, (IEntity) this);
    }

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicleProto)
    {
      switch (vehicleProto)
      {
        case TreePlanterProto _:
          return true;
        case TreeHarvesterProto _:
          return true;
        default:
          return false;
      }
    }

    public void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs)
    {
      switch (vehicle)
      {
        case TreePlanter vehicle1:
          this.m_treePlanters.AssignVehicle(vehicle1, doNotCancelJobs);
          break;
        case TreeHarvester vehicle2:
          this.m_treeHarvesters.AssignVehicle(vehicle2, doNotCancelJobs);
          break;
      }
      this.updateAssignedVehicles();
    }

    public void UnassignVehicle(Vehicle vehicle, bool cancelJobs = true)
    {
      switch (vehicle)
      {
        case TreePlanter vehicle1:
          this.m_treePlanters.UnassignVehicle(vehicle1, cancelJobs);
          break;
        case TreeHarvester vehicle2:
          this.m_treeHarvesters.UnassignVehicle(vehicle2, cancelJobs);
          break;
      }
      this.updateAssignedVehicles();
    }

    private void updateAssignedVehicles()
    {
      this.m_allVehicles.Clear();
      this.m_allVehicles.AddRange(this.m_treePlanters.AllUntyped);
      this.m_allVehicles.AddRange(this.m_treeHarvesters.AllUntyped);
      this.updateNotifications();
    }

    public bool HasTreeReadyToHarvest(ProductProto.ID productId)
    {
      if (!this.IsEnabled)
        return false;
      this.updateTreeState();
      foreach (TreeId cuttableTree in this.m_cuttableTrees)
      {
        TreeData treeData;
        if (!this.m_treeManager.IsTreeReserved(cuttableTree) && this.m_treeManager.Trees.TryGetValue(cuttableTree, out treeData) && !(treeData.HarvestedProductId != productId))
          return true;
      }
      return false;
    }

    public TreeId? FindClosestTreeForHarvestFor(
      Vehicle vehicle,
      ProductProto.ID productId,
      IReadOnlySet<TreeId> unreachableTrees)
    {
      if (!this.IsEnabled)
        return new TreeId?();
      Tile2i groundPositionTile2i = vehicle.GroundPositionTile2i;
      TreeId? treeForHarvestFor = new TreeId?();
      this.updateTreeState();
      long num1 = long.MaxValue;
      foreach (TreeId cuttableTree in this.m_cuttableTrees)
      {
        TreeData treeData;
        if (!this.m_treeManager.IsTreeReserved(cuttableTree) && this.m_treeManager.IsTreeSelected(cuttableTree) && !unreachableTrees.Contains(cuttableTree) && this.m_treeManager.Trees.TryGetValue(cuttableTree, out treeData) && !(treeData.HarvestedProductId != productId))
        {
          long num2 = groundPositionTile2i.DistanceSqrTo((Tile2i) cuttableTree.Position);
          if (num2 < num1)
          {
            treeForHarvestFor = new TreeId?(cuttableTree);
            num1 = num2;
          }
        }
      }
      if (treeForHarvestFor.HasValue)
        return treeForHarvestFor;
      foreach (TreeId cuttableTree in this.m_cuttableTrees)
      {
        TreeData treeData;
        if (!this.m_treeManager.IsTreeReserved(cuttableTree) && !unreachableTrees.Contains(cuttableTree) && this.m_treeManager.Trees.TryGetValue(cuttableTree, out treeData) && !(treeData.HarvestedProductId != productId))
        {
          long num3 = groundPositionTile2i.DistanceSqrTo((Tile2i) cuttableTree.Position);
          if (num3 < num1)
          {
            treeForHarvestFor = new TreeId?(cuttableTree);
            num1 = num3;
          }
        }
      }
      return treeForHarvestFor;
    }

    public bool IsValidTileForPlanting(
      Tile2i plantingTile,
      int spacing,
      Option<TreePlanter> treePlanter)
    {
      if (!this.IsEnabled || !this.Area.ContainsTile(plantingTile) || !this.m_treeManager.IsValidTileForPlanting(plantingTile, spacing))
        return false;
      Option<TerrainDesignation> designationAt = this.m_designationManager.GetDesignationAt(plantingTile);
      if (!designationAt.HasValue || !designationAt.Value.IsFulfilled || !designationAt.Value.IsForestry)
        return false;
      foreach (TreePlanter treePlanter1 in (IEnumerable<TreePlanter>) this.m_vehiclesManager.TreePlanters)
      {
        if (treePlanter1 != treePlanter.ValueOrNull)
        {
          Tile2i? reservedPlantingTile = treePlanter1.GetReservedPlantingTile();
          if (reservedPlantingTile.HasValue && reservedPlantingTile.Value.IsNear(plantingTile, TreeProto.MAX_TREE_SPACING + spacing))
            return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Check whether this tower can possibly have a place to plant a tree.
    /// </summary>
    public bool HasPossiblePlantingPos()
    {
      return this.IsEnabled && this.m_forestPlantingManager.HasPossiblePlantingPos();
    }

    /// <summary>Try to get a tile to plant a tree at.</summary>
    public bool TryFindNextTargetForPlanting(
      Tile2i srcPos,
      IReadOnlySet<Tile2iSlim> unreachableTiles,
      out Option<TreeProto> treeProto,
      out Tile2i plantingTile)
    {
      if (!this.IsEnabled || this.TreeTypes.IsEmpty<KeyValuePair<TreePlantingGroupProto, int>>() || this.m_managedDesignations.IsEmpty)
      {
        treeProto = Option<TreeProto>.None;
        plantingTile = new Tile2i();
        return false;
      }
      treeProto = (Option<TreeProto>) this.m_treeTypes[0].Key.Trees.SampleRandomOrDefault(this.m_randomNumberGenerator);
      return this.m_forestPlantingManager.TryGetNewTreePlantingPos(treeProto.Value.SpacingToOtherTree, srcPos, unreachableTiles, out plantingTile);
    }

    public bool IsTreeReadyForHarvest(TreeId treeId)
    {
      TreeData treeData;
      bool selected;
      return this.IsEnabled && this.m_trees.Contains(treeId) && this.m_treeManager.TryGetTreeAndSelected(treeId, out treeData, out selected) && (selected || !(treeData.GetScaleIgnoringBase(this.m_calendar) < this.TargetHarvestPercent));
    }

    public void AddTreeType(TreePlantingGroupProto plantingProto)
    {
      int num;
      if (this.m_treeTypes.TryGetValue<TreePlantingGroupProto, int>(plantingProto, out num))
        this.m_treeTypes.AddOrSetValue<TreePlantingGroupProto, int>(plantingProto, num + 1);
      else
        this.m_treeTypes.Add<TreePlantingGroupProto, int>(plantingProto, 1);
      this.updateNotifications();
    }

    public bool CanPlantTreeType(TreeProto treeProto)
    {
      foreach (KeyValuePair<TreePlantingGroupProto, int> treeType in this.m_treeTypes)
      {
        if (treeType.Key.Trees.Contains(treeProto))
          return true;
      }
      return false;
    }

    public void RemoveTreeType(TreePlantingGroupProto plantingProto)
    {
      int num;
      if (!this.m_treeTypes.TryGetValue<TreePlantingGroupProto, int>(plantingProto, out num))
      {
        Log.Error(string.Format("Trying to remove {0} which isn't in list", (object) plantingProto));
      }
      else
      {
        if (num > 1)
        {
          this.m_treeTypes.AddOrSetValue<TreePlantingGroupProto, int>(plantingProto, num - 1);
        }
        else
        {
          Assert.That<int>(num).IsEqualTo(1);
          this.m_treeTypes.Remove<TreePlantingGroupProto, int>(plantingProto);
        }
        this.updateNotifications();
      }
    }

    public void SetCutAtPercentage(Percent cutPercent) => this.m_targetHarvestPercent = cutPercent;

    /// <summary>
    /// Populates the passed list as a histogram with evenly spaced buckets.
    /// The list is expected to be pre-allocated with capacity &gt;= numBuckets.
    /// </summary>
    /// <param name="spacing"></param>
    /// <returns></returns>
    public void PopulateTreeGrowthHistogram(int numBuckets, Lyst<int> result)
    {
      if (result.Capacity < numBuckets)
      {
        Log.Error("List passed with less capacity than buckets");
      }
      else
      {
        for (int index = 0; index < numBuckets; ++index)
        {
          if (result.Count <= index)
            result.Add(0);
          else
            result[index] = 0;
        }
        foreach (TreeId tree in this.m_trees)
        {
          TreeData treeData;
          if (this.m_treeManager.Trees.TryGetValue(tree, out treeData))
          {
            int self = (treeData.GetScaleIgnoringBase(this.m_calendar).ToFix32() * numBuckets).ToIntFloored();
            if (self < 0)
            {
              self = 0;
              Log.WarningOnce(string.Format("Tree with negative scale? `{0}`", (object) tree.Position));
            }
            int index = self.Min(numBuckets - 1);
            result[index]++;
          }
          else
            Assert.Fail(string.Format("Tree `{0}` not found in treeManager trees.", (object) tree));
        }
      }
    }

    /// <summary>
    /// Gets the approximate amount of trees that can be planted in valid designations. It'll tend to underestimate
    /// for designations on the "edge" and overestimate for those in the "middle".
    /// </summary>
    /// <returns></returns>
    public int GetApproxMaxTreesAllowed()
    {
      Fix32 fix32_1 = (Fix32) 0;
      int num1 = 0;
      for (int index = 0; index < this.m_treeTypes.Count; ++index)
      {
        KeyValuePair<TreePlantingGroupProto, int> treeType = this.m_treeTypes[index];
        foreach (TreeProto tree in treeType.Key.Trees)
        {
          Fix32 fix32_2 = fix32_1;
          int spacingToOtherTree = tree.SpacingToOtherTree;
          treeType = this.m_treeTypes[index];
          int num2 = treeType.Value;
          Fix32 fix32_3 = (Fix32) (spacingToOtherTree * num2);
          fix32_1 = fix32_2 + fix32_3;
          int num3 = num1;
          treeType = this.m_treeTypes[index];
          int num4 = treeType.Value;
          num1 = num3 + num4;
        }
      }
      if (num1 == 0)
        return 0;
      Fix32 fix32_4 = fix32_1 / num1;
      Fix32 fix32_5 = Fix32.Tau * fix32_4 * fix32_4 / 2 * 1.25.ToFix32();
      int num5 = 0;
      foreach (TerrainDesignation managedDesignation in this.m_managedDesignations)
      {
        if (managedDesignation.IsFulfilled)
          ++num5;
      }
      return ((Fix32) (num5 * 4 * 4) / fix32_5).ToIntCeiled().Max(this.m_trees.Count);
    }

    /// <summary>
    /// Gets days until a tree is ready to be harvested. This is a fairly expensive operation intended
    /// for use in the UI.
    /// </summary>
    public Duration TimeUntilNextHarvest()
    {
      if (this.m_trees.IsEmpty || this.m_targetHarvestPercent == ForestryTower.NO_CUT_AT)
        return Duration.MaxValue;
      this.updateTreeState();
      if (this.m_cuttableTrees.IsNotEmpty)
        return Duration.Zero;
      int num = int.MaxValue;
      TreeData? nullable = new TreeData?();
      foreach (TreeId tree in this.m_trees)
      {
        TreeData treeData;
        if (this.m_treeManager.Trees.TryGetValue(tree, out treeData) && treeData.PlantedAtTick < num)
        {
          num = treeData.PlantedAtTick;
          nullable = new TreeData?(treeData);
        }
      }
      if (!nullable.HasValue)
        return Duration.MaxValue;
      Duration duration = Duration.FromTicks(this.m_calendar.RealTime.Ticks - num);
      Assert.That<Duration>(duration).IsNotNegative();
      if (this.m_targetHarvestPercent == TreeProto.Percent40)
        return nullable.Value.Proto.TreePlantingGroupProto.TimeTo40PercentGrowth - duration;
      if (this.m_targetHarvestPercent == TreeProto.Percent60)
        return nullable.Value.Proto.TreePlantingGroupProto.TimeTo60PercentGrowth - duration;
      if (this.m_targetHarvestPercent == TreeProto.Percent80)
        return nullable.Value.Proto.TreePlantingGroupProto.TimeTo80PercentGrowth - duration;
      if (this.m_targetHarvestPercent == Percent.Hundred)
        return nullable.Value.Proto.TreePlantingGroupProto.TimeTo100PercentGrowth - duration;
      Log.Error(string.Format("Unsupported target harvest percent '{0}'", (object) this.m_targetHarvestPercent));
      return Duration.MaxValue;
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.AssignedOutputs = new ImmutableArray<EntityId>?(this.AssignedOutputs.Select<IEntityAssignedAsOutput, EntityId>((Func<IEntityAssignedAsOutput, EntityId>) (x => x.Id)).ToImmutableArray<EntityId>());
      data.SetTargetHarvestPercent(this.m_targetHarvestPercent);
      data.SetTreeTypes(this.m_treeTypes);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      this.m_targetHarvestPercent = data.GetTargetHarvestPercent() ?? Percent.Hundred;
      data.TryGetTreeTypes(ref this.m_treeTypes);
      if (data.AssignedOutputs.HasValue)
      {
        foreach (IEntityAssignedAsOutput entity in this.AssignedOutputs.ToArray<IEntityAssignedAsOutput>())
        {
          entity.UnassignStaticInputEntity((IEntityAssignedAsInput) this);
          ((IEntityAssignedAsInput) this).UnassignStaticOutputEntity(entity);
        }
        foreach (EntityId id in data.AssignedOutputs.Value)
        {
          IEntityAssignedAsOutput entity;
          if (this.Context.EntitiesManager.TryGetEntity<IEntityAssignedAsOutput>(id, out entity) && entity.CanBeAssignedWithInput((IEntityAssignedAsInput) this) && this.CanBeAssignedWithOutput(entity))
          {
            entity.AssignStaticInputEntity((IEntityAssignedAsInput) this);
            ((IEntityAssignedAsInput) this).AssignStaticOutputEntity(entity);
          }
        }
      }
      this.updateNotifications();
    }

    public static void Serialize(ForestryTower value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestryTower>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestryTower.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AllowNonAssignedOutput);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      Set<FuelStation>.Serialize(this.m_assignedFuelStations, writer);
      Set<IEntityAssignedAsInput>.Serialize(this.m_assignedInputEntities, writer);
      Set<IEntityAssignedAsOutput>.Serialize(this.m_assignedOutputEntities, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      Set<TreeId>.Serialize(this.m_cuttableTrees, writer);
      writer.WriteGeneric<ITerrainDesignationsManager>(this.m_designationManager);
      writer.WriteBool(this.m_designationsDirty);
      ForestryTower.ForestPlantingManager.Serialize(this.m_forestPlantingManager, writer);
      Set<TerrainDesignation>.Serialize(this.m_managedDesignations, writer);
      ForestryTowersManager.Serialize(this.m_manager, writer);
      EntityNotificator.Serialize(this.m_noForestryDesignationsNotif, writer);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      writer.WriteGeneric<IRandom>(this.m_randomNumberGenerator);
      Set<TreeId>.Serialize(this.m_stumps, writer);
      Percent.Serialize(this.m_targetHarvestPercent, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      AssignedVehicles<TreeHarvester, TreeHarvesterProto>.Serialize(this.m_treeHarvesters, writer);
      writer.WriteGeneric<ITreesManager>(this.m_treeManager);
      AssignedVehicles<TreePlanter, TreePlanterProto>.Serialize(this.m_treePlanters, writer);
      Set<TreeId>.Serialize(this.m_trees, writer);
      writer.WriteBool(this.m_treesCheckedThisTick);
      Lyst<KeyValuePair<TreePlantingGroupProto, int>>.Serialize(this.m_treeTypes, writer);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
      writer.WriteGeneric<ForestryTowerProto>(this.Prototype);
    }

    public static ForestryTower Deserialize(BlobReader reader)
    {
      ForestryTower forestryTower;
      if (reader.TryStartClassDeserialization<ForestryTower>(out forestryTower))
        reader.EnqueueDataDeserialization((object) forestryTower, ForestryTower.s_deserializeDataDelayedAction);
      return forestryTower;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AllowNonAssignedOutput = reader.ReadBool();
      this.Area = RectangleTerrainArea2i.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_allVehicles", (object) new Lyst<Vehicle>());
      reader.SetField<ForestryTower>(this, "m_assignedFuelStations", (object) Set<FuelStation>.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_assignedInputEntities", reader.LoadedSaveVersion >= 112 ? (object) Set<IEntityAssignedAsInput>.Deserialize(reader) : (object) new Set<IEntityAssignedAsInput>());
      reader.SetField<ForestryTower>(this, "m_assignedInputStorages", (object) new Set<IEntityAssignedAsInput>());
      reader.SetField<ForestryTower>(this, "m_assignedOutputEntities", (object) Set<IEntityAssignedAsOutput>.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_assignedOutputStorages", (object) new Set<Storage>());
      if (reader.LoadedSaveVersion >= 107 && reader.LoadedSaveVersion < 112)
        Set<Storage>.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<ForestryTower>(this, "m_cuttableTrees", (object) Set<TreeId>.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_designationManager", (object) reader.ReadGenericAs<ITerrainDesignationsManager>());
      this.m_designationsDirty = reader.ReadBool();
      this.m_forestPlantingManager = ForestryTower.ForestPlantingManager.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_managedDesignations", (object) Set<TerrainDesignation>.Deserialize(reader));
      this.m_manager = ForestryTowersManager.Deserialize(reader);
      this.m_noForestryDesignationsNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.RegisterResolvedMember<ForestryTower>(this, "m_protosDb", typeof (ProtosDb), true);
      this.m_randomNumberGenerator = reader.ReadGenericAs<IRandom>();
      reader.SetField<ForestryTower>(this, "m_stumps", reader.LoadedSaveVersion >= 98 ? (object) Set<TreeId>.Deserialize(reader) : (object) new Set<TreeId>());
      this.m_targetHarvestPercent = Percent.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_treeHarvesters", (object) AssignedVehicles<TreeHarvester, TreeHarvesterProto>.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_treeManager", (object) reader.ReadGenericAs<ITreesManager>());
      reader.SetField<ForestryTower>(this, "m_treePlanters", (object) AssignedVehicles<TreePlanter, TreePlanterProto>.Deserialize(reader));
      reader.SetField<ForestryTower>(this, "m_trees", (object) Set<TreeId>.Deserialize(reader));
      this.m_treesCheckedThisTick = reader.ReadBool();
      reader.SetField<ForestryTower>(this, "m_treesToRemove", (object) new Lyst<TreeId>());
      this.m_treeTypes = Lyst<KeyValuePair<TreePlantingGroupProto, int>>.Deserialize(reader);
      reader.SetField<ForestryTower>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      reader.SetField<ForestryTower>(this, "Prototype", (object) reader.ReadGenericAs<ForestryTowerProto>());
      reader.RegisterInitAfterLoad<ForestryTower>(this, "initSelf", InitPriority.Normal);
    }

    static ForestryTower()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestryTower.NO_CUT_AT = 200.Percent();
      ForestryTower.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      ForestryTower.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    /// <summary>
    /// This class handles planting locations for the forestry towers. It tracks
    /// which trees can have new trees "extended" from them and handles generating of
    /// new positions using that information, information about designations, and information
    /// about jobs of other planters.
    /// </summary>
    [GenerateSerializer(false, null, 0)]
    internal class ForestPlantingManager
    {
      private static readonly Fix32 SPACING_RAND_RANGE;
      private readonly Set<TreeId> m_treesReadyForExtension;
      [NewInSaveVersion(98, null, "new()", null, null)]
      private readonly Lyst<KeyValuePair<int, Set<TreeId>>> m_stumpsReadyForReplacementAtRange;
      [NewInSaveVersion(98, null, null, null, null)]
      private int m_stumpRefreshCooldown;
      private readonly Set<TerrainDesignation> m_designationsReadyForExtension;
      [DoNotSaveCreateNewOnLoad("new Pair<TreeId, long>[MAX_OUTER_TRIALS]", 0)]
      private readonly Pair<TreeId, long>[] m_treesReadyForExtensionTopN;
      [DoNotSaveCreateNewOnLoad("new()", 0)]
      private readonly Lyst<TreePlanter> m_plantersCache;
      [ThreadStatic]
      private static Lyst<TreeId> s_stumpsForRemoval;
      [ThreadStatic]
      private static Lyst<TreeId> s_treesForRemoval;
      private readonly IRandom m_rng;
      private readonly ForestryTower m_forestryTower;
      [NewInSaveVersion(123, null, null, null, null)]
      private bool m_pollingThrottle;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      internal ForestPlantingManager(ForestryTower forestryTower, RandomProvider randomProvider)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_stumpsReadyForReplacementAtRange = new Lyst<KeyValuePair<int, Set<TreeId>>>();
        this.m_designationsReadyForExtension = new Set<TerrainDesignation>();
        this.m_treesReadyForExtensionTopN = new Pair<TreeId, long>[4];
        this.m_plantersCache = new Lyst<TreePlanter>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_treesReadyForExtension = new Set<TreeId>((IEnumerable<TreeId>) forestryTower.m_trees);
        this.m_forestryTower = forestryTower;
        this.m_rng = randomProvider.GetSimRandomFor((object) this, nameof (ForestPlantingManager) + this.m_forestryTower.Id.ToString());
        this.m_rng.Jump();
      }

      internal void Reset()
      {
        this.m_treesReadyForExtension.Clear();
        this.m_designationsReadyForExtension.Clear();
        this.m_stumpsReadyForReplacementAtRange.Clear();
        this.m_pollingThrottle = false;
      }

      internal void AddTree(TreeId treeId)
      {
        this.m_treesReadyForExtension.Add(treeId);
        this.m_designationsReadyForExtension.Remove(this.m_forestryTower.m_designationManager.GetDesignationAt((Tile2i) treeId.Position).ValueOrNull);
        this.m_pollingThrottle = false;
      }

      internal void RemoveTree(TreeId treeId)
      {
        this.m_treesReadyForExtension.Remove(treeId);
        this.repopulateReadyForExtensionAtPos((Tile2i) treeId.Position, TreeProto.MAX_TREE_SPACING * 2 + ForestryTower.ForestPlantingManager.SPACING_RAND_RANGE.ToIntCeiled());
      }

      internal void AddStump(TreeId stumpId)
      {
      }

      internal void RemoveStump(TreeId stumpId)
      {
        foreach (KeyValuePair<int, Set<TreeId>> keyValuePair in this.m_stumpsReadyForReplacementAtRange)
          keyValuePair.Value.Remove(stumpId);
      }

      internal void AddDesignation(TerrainDesignation designation)
      {
        this.repopulateReadyForExtensionAtPos(designation.OriginTileCoord, TreeProto.MAX_TREE_SPACING * 2 + 4 + ForestryTower.ForestPlantingManager.SPACING_RAND_RANGE.ToIntCeiled());
        this.m_designationsReadyForExtension.AddAndAssertNew(designation);
      }

      internal void RemoveDesignation(TerrainDesignation designation)
      {
        this.m_designationsReadyForExtension.Remove(designation);
        foreach (KeyValuePair<int, Set<TreeId>> keyValuePair in this.m_stumpsReadyForReplacementAtRange)
          keyValuePair.Value.Clear();
        if (ForestryTower.ForestPlantingManager.s_treesForRemoval == null)
          ForestryTower.ForestPlantingManager.s_treesForRemoval = new Lyst<TreeId>();
        else
          ForestryTower.ForestPlantingManager.s_treesForRemoval.Clear();
        foreach (TreeId treeId in this.m_treesReadyForExtension)
        {
          if (designation.Area.ContainsTile(treeId.Position))
            ForestryTower.ForestPlantingManager.s_treesForRemoval.Add(treeId);
        }
        foreach (TreeId treeId in ForestryTower.ForestPlantingManager.s_treesForRemoval)
          this.m_treesReadyForExtension.Remove(treeId);
      }

      private void repopulateReadyForExtensionAtPos(Tile2i tile2i, int distance)
      {
        foreach (TreeId tree in (IEnumerable<TreeId>) this.m_forestryTower.Trees)
        {
          if (tree.Position.AsFull.IsNear(tile2i, distance))
            this.m_treesReadyForExtension.Add(tree);
        }
        this.m_pollingThrottle = false;
      }

      internal bool HasPossiblePlantingPos()
      {
        if (this.m_pollingThrottle)
          return false;
        return this.m_treesReadyForExtension.Count > 0 || this.m_designationsReadyForExtension.Count > 0;
      }

      private bool tryPlantOnStump(
        int spacing,
        Tile2i srcPos,
        IReadOnlySet<Tile2iSlim> unreachableTiles,
        out Tile2i plantingPos)
      {
        Set<TreeId> set;
        if (!this.m_stumpsReadyForReplacementAtRange.TryGetValue<int, Set<TreeId>>(spacing, out set))
        {
          set = new Set<TreeId>();
          this.m_stumpsReadyForReplacementAtRange.Add<int, Set<TreeId>>(spacing, set);
        }
        this.m_stumpRefreshCooldown -= 10;
        if (set.IsEmpty && this.m_stumpRefreshCooldown <= 0)
        {
          foreach (TreeId stump in (IEnumerable<TreeId>) this.m_forestryTower.Stumps)
          {
            if (this.m_forestryTower.IsValidTileForPlanting((Tile2i) stump.Position, spacing, Option<TreePlanter>.None) && !unreachableTiles.Contains(stump.Position))
              set.Add(stump);
          }
          this.m_stumpRefreshCooldown = this.m_forestryTower.Stumps.Count;
        }
        if (set.IsNotEmpty)
        {
          if (ForestryTower.ForestPlantingManager.s_stumpsForRemoval == null)
            ForestryTower.ForestPlantingManager.s_stumpsForRemoval = new Lyst<TreeId>();
          else
            ForestryTower.ForestPlantingManager.s_stumpsForRemoval.Clear();
          long num1 = long.MaxValue;
          TreeId? nullable = new TreeId?();
          foreach (TreeId treeId in set)
          {
            long num2 = treeId.Position.AsFull.DistanceSqrTo(srcPos);
            if (num2 < num1)
            {
              if (!this.m_forestryTower.IsValidTileForPlanting((Tile2i) treeId.Position, spacing, Option<TreePlanter>.None))
              {
                ForestryTower.ForestPlantingManager.s_stumpsForRemoval.Add(treeId);
              }
              else
              {
                nullable = new TreeId?(treeId);
                num1 = num2;
              }
            }
          }
          if (nullable.HasValue)
          {
            if (ForestryTower.ForestPlantingManager.s_stumpsForRemoval.IsNotEmpty)
            {
              foreach (TreeId treeId in ForestryTower.ForestPlantingManager.s_stumpsForRemoval)
                set.Remove(treeId);
            }
            plantingPos = (Tile2i) nullable.Value.Position;
            return true;
          }
          set.Clear();
        }
        plantingPos = new Tile2i();
        return false;
      }

      internal bool TryGetNewTreePlantingPos(
        int spacing,
        Tile2i srcPos,
        IReadOnlySet<Tile2iSlim> unreachableTiles,
        out Tile2i plantingPos)
      {
        if (this.tryPlantOnStump(spacing, srcPos, unreachableTiles, out plantingPos))
        {
          this.m_pollingThrottle = false;
          return true;
        }
        if (this.m_treesReadyForExtension.Count == 0 && this.m_designationsReadyForExtension.Count == 0)
        {
          this.m_treesReadyForExtension.AddRange((IEnumerable<TreeId>) this.m_forestryTower.m_trees);
          this.m_pollingThrottle = true;
        }
        long num1 = long.MaxValue;
        int self = 0;
        int num2 = this.m_pollingThrottle ? 1 : 4;
        int numTrials = this.m_pollingThrottle ? 32.Min(4) : 32;
        Assert.That<int>(num2).IsLessOrEqual(10);
        foreach (TreeId first in this.m_treesReadyForExtension)
        {
          long second = first.Position.AsFull.DistanceSqrTo(srcPos);
          if (second < num1)
          {
            Pair<TreeId, long> pair = new Pair<TreeId, long>(first, second);
            if (self == 0)
            {
              this.m_treesReadyForExtensionTopN[0] = pair;
              num1 = second;
              ++self;
            }
            else
            {
              for (int index1 = self.Min(num2) - 1; index1 >= 0; --index1)
              {
                if (second < this.m_treesReadyForExtensionTopN[index1].Second)
                {
                  for (int index2 = num2 - 1; index2 > index1; --index2)
                    this.m_treesReadyForExtensionTopN[index2] = this.m_treesReadyForExtensionTopN[index2 - 1];
                  this.m_treesReadyForExtensionTopN[index1] = pair;
                  ++self;
                  num1 = this.m_treesReadyForExtensionTopN[self.Min(num2) - 1].Second;
                  break;
                }
              }
            }
          }
        }
        for (int index = 0; index < num2 && index < self; ++index)
        {
          TreeId first = this.m_treesReadyForExtensionTopN[index].First;
          TreeData treeData;
          if (this.m_forestryTower.m_treeManager.Trees.TryGetValue(first, out treeData))
          {
            Fix32 offsetRange = (Fix32) (treeData.Proto.SpacingToOtherTree + spacing);
            if (this.tryGetNewTreeFromOld((Tile2i) first.Position, spacing, offsetRange, numTrials, unreachableTiles, out plantingPos))
            {
              this.m_pollingThrottle = false;
              return true;
            }
            this.m_treesReadyForExtension.Remove(first);
          }
          else
          {
            Log.Error(string.Format("Tree not found: '{0}'", (object) first));
            this.m_treesReadyForExtension.Remove(first);
          }
        }
        if (this.m_treesReadyForExtension.IsNotEmpty)
        {
          plantingPos = new Tile2i();
          return false;
        }
        if (this.m_forestryTower.m_treePlanters.Count > 1)
        {
          Lyst<TreePlanter> cleanLyst = this.m_forestryTower.m_treePlanters.All.Where<TreePlanter>((Func<TreePlanter, bool>) (tp => tp.GetReservedPlantingTile().HasValue)).ToCleanLyst<TreePlanter>(this.m_plantersCache);
          if (cleanLyst.Count > 0)
          {
            int index = this.m_rng.NextInt(cleanLyst.Count);
            TreePlanter treePlanter = cleanLyst[index];
            Tile2i plantingPosition = treePlanter.GetReservedPlantingTile().Value;
            TreeProto jobTreeProto = treePlanter.GetJobTreeProto();
            if ((Proto) jobTreeProto == (Proto) null)
            {
              Log.Error("Planting proto unexpectedly null");
              plantingPos = new Tile2i();
              return false;
            }
            Fix32 offsetRange = (Fix32) (jobTreeProto.SpacingToOtherTree + spacing);
            if (this.tryGetNewTreeFromOld(plantingPosition, spacing, offsetRange, 4, unreachableTiles, out plantingPos))
            {
              this.m_pollingThrottle = false;
              return true;
            }
          }
        }
        for (int index = 0; index < numTrials && this.m_designationsReadyForExtension.Count != 0; ++index)
        {
          TerrainDesignation terrainDesignation = this.m_designationsReadyForExtension.MinElement<TerrainDesignation, long>((Func<TerrainDesignation, long>) (d => d.CenterTileCoord.DistanceSqrTo(this.m_forestryTower.Position2f.Tile2i)));
          Tile2i centerTileCoord = terrainDesignation.CenterTileCoord;
          if (!unreachableTiles.Contains(centerTileCoord.AsSlim))
          {
            if (this.m_forestryTower.IsValidTileForPlanting(centerTileCoord, spacing, Option<TreePlanter>.None))
            {
              plantingPos = centerTileCoord;
              this.m_pollingThrottle = false;
              return true;
            }
            this.m_designationsReadyForExtension.Remove(terrainDesignation);
          }
        }
        plantingPos = new Tile2i();
        return false;
      }

      private bool tryGetNewTreeFromOld(
        Tile2i plantingPosition,
        int spacing,
        Fix32 offsetRange,
        int numTrials,
        IReadOnlySet<Tile2iSlim> unreachableTiles,
        out Tile2i newPos)
      {
        for (int index = 0; index < numTrials; ++index)
        {
          Vector2f vector2f = this.m_rng.NextAngle().DirectionVector * (offsetRange + this.m_rng.NextFix32Between01() * ForestryTower.ForestPlantingManager.SPACING_RAND_RANGE);
          newPos = plantingPosition + new RelTile2i(vector2f.X.ToIntCeiled(), vector2f.Y.ToIntCeiled());
          if (!unreachableTiles.Contains(newPos.AsSlim) && this.m_forestryTower.IsValidTileForPlanting(newPos, spacing, Option<TreePlanter>.None))
            return true;
        }
        newPos = new Tile2i();
        return false;
      }

      public static void Serialize(ForestryTower.ForestPlantingManager value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ForestryTower.ForestPlantingManager>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ForestryTower.ForestPlantingManager.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Set<TerrainDesignation>.Serialize(this.m_designationsReadyForExtension, writer);
        ForestryTower.Serialize(this.m_forestryTower, writer);
        writer.WriteBool(this.m_pollingThrottle);
        writer.WriteGeneric<IRandom>(this.m_rng);
        writer.WriteInt(this.m_stumpRefreshCooldown);
        Lyst<KeyValuePair<int, Set<TreeId>>>.Serialize(this.m_stumpsReadyForReplacementAtRange, writer);
        Set<TreeId>.Serialize(this.m_treesReadyForExtension, writer);
      }

      public static ForestryTower.ForestPlantingManager Deserialize(BlobReader reader)
      {
        ForestryTower.ForestPlantingManager forestPlantingManager;
        if (reader.TryStartClassDeserialization<ForestryTower.ForestPlantingManager>(out forestPlantingManager))
          reader.EnqueueDataDeserialization((object) forestPlantingManager, ForestryTower.ForestPlantingManager.s_deserializeDataDelayedAction);
        return forestPlantingManager;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_designationsReadyForExtension", (object) Set<TerrainDesignation>.Deserialize(reader));
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_forestryTower", (object) ForestryTower.Deserialize(reader));
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_plantersCache", (object) new Lyst<TreePlanter>());
        this.m_pollingThrottle = reader.LoadedSaveVersion >= 123 && reader.ReadBool();
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_rng", (object) reader.ReadGenericAs<IRandom>());
        this.m_stumpRefreshCooldown = reader.LoadedSaveVersion >= 98 ? reader.ReadInt() : 0;
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_stumpsReadyForReplacementAtRange", reader.LoadedSaveVersion >= 98 ? (object) Lyst<KeyValuePair<int, Set<TreeId>>>.Deserialize(reader) : (object) new Lyst<KeyValuePair<int, Set<TreeId>>>());
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_treesReadyForExtension", (object) Set<TreeId>.Deserialize(reader));
        reader.SetField<ForestryTower.ForestPlantingManager>(this, "m_treesReadyForExtensionTopN", (object) new Pair<TreeId, long>[4]);
      }

      static ForestPlantingManager()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ForestryTower.ForestPlantingManager.SPACING_RAND_RANGE = Fix32.FromFraction(1L, 1L);
        ForestryTower.ForestPlantingManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ForestryTower.ForestPlantingManager) obj).SerializeData(writer));
        ForestryTower.ForestPlantingManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ForestryTower.ForestPlantingManager) obj).DeserializeData(reader));
      }
    }
  }
}
