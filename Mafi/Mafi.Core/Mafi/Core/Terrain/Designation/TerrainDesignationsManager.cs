// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainDesignationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Console;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class TerrainDesignationsManager : 
    ITerrainDesignationsManagerInternal,
    ITerrainDesignationsManager,
    ICommandProcessor<AddTerrainDesignationsCmd>,
    IAction<AddTerrainDesignationsCmd>,
    ICommandProcessor<RemoveDesignationsCmd>,
    IAction<RemoveDesignationsCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// Extra search radius of 3 means all valid designations with manhattan distance 3 designations around
    /// the best designations are also returned.
    /// </summary>
    internal const int MAX_EXTRA_NEARBY_AREAS_MAX_DIST = 12;
    private readonly Event<TerrainDesignation> m_designationAdded;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Event<TerrainDesignation> m_designationUpdated;
    private readonly Event<TerrainDesignation> m_designationFulfilledChanged;
    private readonly Event<TerrainDesignation> m_designationRemoved;
    private readonly Event<TerrainDesignation> m_designationManagedTowersChanged;
    private readonly Event<TerrainDesignation> m_designationReachabilityChanged;
    private readonly Set<TerrainDesignation> m_notOwnedDesignationsWarning;
    /// <summary>
    /// All managed designations. Is mutated only on sim thread.
    /// </summary>
    private readonly Dict<Tile2i, TerrainDesignation> m_designations;
    /// <summary>All not fulfilled designations for easier search.</summary>
    private readonly Dict<Tile2i, TerrainDesignation> m_notFulfilledAreas;
    private Dict<TerrainDesignation, Lyst<IVehicleJob>> m_designationsWithJobs;
    [DoNotSave(0, null)]
    private readonly ObjectPool2<Lyst<IVehicleJob>> m_jobListsPool;
    [NewInSaveVersion(140, null, null, typeof (LazyResolve<SurfaceDesignationsManager>), null)]
    private readonly LazyResolve<SurfaceDesignationsManager> m_surfaceDesignationsManager;
    private readonly ProtosDb m_protosDb;
    private readonly LazyResolve<IVehiclePathFindingManager> m_vehiclePathFindingManager;
    private readonly IEntitiesManager m_entitiesManager;
    private Notificator m_areasWithoutTowerNotif;
    private Notificator m_areasWithoutForestryTowerNotif;
    private bool m_checkNotifyAreasWithoutTower;

    public static void Serialize(TerrainDesignationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainDesignationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainDesignationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AllowDesignationsOverEntities);
      Notificator.Serialize(this.m_areasWithoutForestryTowerNotif, writer);
      Notificator.Serialize(this.m_areasWithoutTowerNotif, writer);
      writer.WriteBool(this.m_checkNotifyAreasWithoutTower);
      Event<TerrainDesignation>.Serialize(this.m_designationAdded, writer);
      Event<TerrainDesignation>.Serialize(this.m_designationFulfilledChanged, writer);
      Event<TerrainDesignation>.Serialize(this.m_designationManagedTowersChanged, writer);
      Event<TerrainDesignation>.Serialize(this.m_designationReachabilityChanged, writer);
      Event<TerrainDesignation>.Serialize(this.m_designationRemoved, writer);
      Dict<Tile2i, TerrainDesignation>.Serialize(this.m_designations, writer);
      Dict<TerrainDesignation, Lyst<IVehicleJob>>.Serialize(this.m_designationsWithJobs, writer);
      Event<TerrainDesignation>.Serialize(this.m_designationUpdated, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Dict<Tile2i, TerrainDesignation>.Serialize(this.m_notFulfilledAreas, writer);
      Set<TerrainDesignation>.Serialize(this.m_notOwnedDesignationsWarning, writer);
      LazyResolve<SurfaceDesignationsManager>.Serialize(this.m_surfaceDesignationsManager, writer);
      LazyResolve<IVehiclePathFindingManager>.Serialize(this.m_vehiclePathFindingManager, writer);
      TerrainOccupancyManager.Serialize(this.OccupancyManager, writer);
      TerrainManager.Serialize(this.TerrainManager, writer);
      TerrainPropsManager.Serialize(this.TerrainPropsManager, writer);
      TreesManager.Serialize(this.TreesManager, writer);
    }

    public static TerrainDesignationsManager Deserialize(BlobReader reader)
    {
      TerrainDesignationsManager designationsManager;
      if (reader.TryStartClassDeserialization<TerrainDesignationsManager>(out designationsManager))
        reader.EnqueueDataDeserialization((object) designationsManager, TerrainDesignationsManager.s_deserializeDataDelayedAction);
      return designationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowDesignationsOverEntities = reader.ReadBool();
      this.m_areasWithoutForestryTowerNotif = Notificator.Deserialize(reader);
      this.m_areasWithoutTowerNotif = Notificator.Deserialize(reader);
      this.m_checkNotifyAreasWithoutTower = reader.ReadBool();
      reader.SetField<TerrainDesignationsManager>(this, "m_designationAdded", (object) Event<TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_designationFulfilledChanged", (object) Event<TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_designationManagedTowersChanged", (object) Event<TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_designationReachabilityChanged", (object) Event<TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_designationRemoved", (object) Event<TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_designations", (object) Dict<Tile2i, TerrainDesignation>.Deserialize(reader));
      this.m_designationsWithJobs = Dict<TerrainDesignation, Lyst<IVehicleJob>>.Deserialize(reader);
      reader.SetField<TerrainDesignationsManager>(this, "m_designationUpdated", reader.LoadedSaveVersion >= 140 ? (object) Event<TerrainDesignation>.Deserialize(reader) : (object) new Event<TerrainDesignation>());
      reader.SetField<TerrainDesignationsManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<TerrainDesignationsManager>(this, "m_notFulfilledAreas", (object) Dict<Tile2i, TerrainDesignation>.Deserialize(reader));
      reader.SetField<TerrainDesignationsManager>(this, "m_notOwnedDesignationsWarning", (object) Set<TerrainDesignation>.Deserialize(reader));
      reader.RegisterResolvedMember<TerrainDesignationsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TerrainDesignationsManager>(this, "m_surfaceDesignationsManager", reader.LoadedSaveVersion >= 140 ? (object) LazyResolve<SurfaceDesignationsManager>.Deserialize(reader) : (object) (LazyResolve<SurfaceDesignationsManager>) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<TerrainDesignationsManager>(this, "m_surfaceDesignationsManager", typeof (LazyResolve<SurfaceDesignationsManager>), true);
      reader.SetField<TerrainDesignationsManager>(this, "m_vehiclePathFindingManager", (object) LazyResolve<IVehiclePathFindingManager>.Deserialize(reader));
      this.OccupancyManager = TerrainOccupancyManager.Deserialize(reader);
      this.TerrainManager = TerrainManager.Deserialize(reader);
      this.TerrainPropsManager = TerrainPropsManager.Deserialize(reader);
      this.TreesManager = reader.LoadedSaveVersion >= 120 ? TreesManager.Deserialize(reader) : (TreesManager) null;
      if (reader.LoadedSaveVersion < 120)
        reader.RegisterResolvedMember<TerrainDesignationsManager>(this, "TreesManager", typeof (TreesManager), false);
      reader.RegisterInitAfterLoad<TerrainDesignationsManager>(this, "initDesignations", InitPriority.Highest);
      reader.RegisterInitAfterLoad<TerrainDesignationsManager>(this, "initAfterAllLoaded", InitPriority.Low);
    }

    /// <summary>
    /// Invoked when new designation is added. Always raised on Sim thread.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationAdded
    {
      get => (IEvent<TerrainDesignation>) this.m_designationAdded;
    }

    /// <summary>
    /// Invoked when new designation is updated. Always raised on Sim thread.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationUpdated
    {
      get => (IEvent<TerrainDesignation>) this.m_designationUpdated;
    }

    /// <summary>
    /// Invoked when the designation state is changed for example its fulfilled state. Always raised on Sim thread.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationFulfilledChanged
    {
      get => (IEvent<TerrainDesignation>) this.m_designationFulfilledChanged;
    }

    /// <summary>
    /// Invoked when new designation is removed. Always raised on Sim thread.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationRemoved
    {
      get => (IEvent<TerrainDesignation>) this.m_designationRemoved;
    }

    /// <summary>
    /// Invoked when <see cref="F:Mafi.Core.Terrain.Designation.TerrainDesignation.ManagedByTowers" /> changes.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationManagedTowersChanged
    {
      get => (IEvent<TerrainDesignation>) this.m_designationManagedTowersChanged;
    }

    /// <summary>
    /// Invoked <see cref="!:TerrainDesignation.IsReachable" /> changes. TODO: This will be killed.
    /// </summary>
    public IEvent<TerrainDesignation> DesignationReachabilityChanged
    {
      get => (IEvent<TerrainDesignation>) this.m_designationReachabilityChanged;
    }

    public IReadOnlyCollection<TerrainDesignation> Designations
    {
      get => (IReadOnlyCollection<TerrainDesignation>) this.m_designations.Values;
    }

    public IReadOnlyDictionary<Tile2i, TerrainDesignation> DesignationsDict
    {
      get => (IReadOnlyDictionary<Tile2i, TerrainDesignation>) this.m_designations;
    }

    public int Count => this.m_designations.Count;

    public bool AllowDesignationsOverEntities { get; private set; }

    public TerrainManager TerrainManager { get; private set; }

    [NewInSaveVersion(120, null, null, typeof (TreesManager), null)]
    public TreesManager TreesManager { get; private set; }

    public TerrainPropsManager TerrainPropsManager { get; private set; }

    public TerrainOccupancyManager OccupancyManager { get; private set; }

    public IEntitiesManager EntitiesManager => this.m_entitiesManager;

    [DoNotSave(0, null)]
    internal bool IsFulfillingAllDesignations { get; private set; }

    public TerrainDesignationsManager(
      TerrainManager terrainManager,
      TerrainPropsManager terrainPropsManager,
      TreesManager treesManager,
      LazyResolve<SurfaceDesignationsManager> surfaceDesignationsManager,
      ProtosDb protosDb,
      LazyResolve<IVehiclePathFindingManager> vehiclePathFindingManager,
      INotificationsManager notificationsManager,
      TerrainOccupancyManager occupancyManager,
      ISimLoopEvents simLoopEvents,
      IGameLoopEvents gameLoopEvents,
      IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_designationAdded = new Event<TerrainDesignation>();
      this.m_designationUpdated = new Event<TerrainDesignation>();
      this.m_designationFulfilledChanged = new Event<TerrainDesignation>();
      this.m_designationRemoved = new Event<TerrainDesignation>();
      this.m_designationManagedTowersChanged = new Event<TerrainDesignation>();
      this.m_designationReachabilityChanged = new Event<TerrainDesignation>();
      this.m_notOwnedDesignationsWarning = new Set<TerrainDesignation>();
      this.m_designations = new Dict<Tile2i, TerrainDesignation>();
      this.m_notFulfilledAreas = new Dict<Tile2i, TerrainDesignation>();
      this.m_designationsWithJobs = new Dict<TerrainDesignation, Lyst<IVehicleJob>>();
      this.m_jobListsPool = new ObjectPool2<Lyst<IVehicleJob>>(256, (Func<ObjectPool2<Lyst<IVehicleJob>>, Lyst<IVehicleJob>>) (p => new Lyst<IVehicleJob>()), (Action<Lyst<IVehicleJob>>) (l => l.Clear()));
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainManager = terrainManager;
      this.TerrainPropsManager = terrainPropsManager;
      this.TreesManager = treesManager;
      this.m_surfaceDesignationsManager = surfaceDesignationsManager;
      this.m_protosDb = protosDb;
      this.m_vehiclePathFindingManager = vehiclePathFindingManager;
      this.OccupancyManager = occupancyManager;
      this.m_entitiesManager = entitiesManager;
      this.m_areasWithoutTowerNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.AreasWithoutTowers);
      this.m_areasWithoutForestryTowerNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.AreasWithoutForestryTowers);
      terrainManager.HeightChanged.Add<TerrainDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileHeightChanged));
      terrainManager.TileMaterialsOnlyChanged.Add<TerrainDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileMaterialChanged));
      terrainPropsManager.PropChangedAt.Add<TerrainDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      treesManager.StumpAdded.Add<TerrainDesignationsManager>(this, new Action<TreeStumpData>(this.stumpAdded));
      treesManager.StumpRemoved.Add<TerrainDesignationsManager>(this, new Action<TreeStumpData>(this.stumpRemoved));
      occupancyManager.TileOccupancyChanged.Add<TerrainDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      simLoopEvents.Update.Add<TerrainDesignationsManager>(this, new Action(this.simUpdate));
      gameLoopEvents.Terminate.AddNonSaveable<TerrainDesignationsManager>(this, new Action(this.onTerminate));
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initDesignations()
    {
      ReflectionUtils.SetField<TerrainDesignationsManager>(this, "m_jobListsPool", (object) new ObjectPool2<Lyst<IVehicleJob>>(256, (Func<ObjectPool2<Lyst<IVehicleJob>>, Lyst<IVehicleJob>>) (p => new Lyst<IVehicleJob>()), (Action<Lyst<IVehicleJob>>) (l => l.Clear())));
      foreach (TerrainDesignation terrainDesignation in this.m_designations.Values)
        terrainDesignation.InitAfterLoad((ITerrainDesignationsManagerInternal) this);
      foreach (KeyValuePair<TerrainDesignation, Lyst<IVehicleJob>> designationsWithJob in this.m_designationsWithJobs)
        designationsWithJob.Key.InitNumberOfJobsAssigned(designationsWithJob.Value.Count);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initAfterAllLoaded(int saveVersion)
    {
      if (saveVersion < (int) sbyte.MaxValue)
      {
        LystStruct<Tile2i> lystStruct = new LystStruct<Tile2i>();
        foreach (TerrainDesignation terrainDesignation in this.m_designations.Values)
        {
          if (!this.IsDesignationAllowed(terrainDesignation.Data, out LocStrFormatted _))
            lystStruct.Add(terrainDesignation.Data.OriginTile);
        }
        foreach (Tile2i originCoord in lystStruct)
          this.RemoveDesignation(originCoord);
      }
      if (saveVersion <= 154 && !this.TreesManager.StumpRemoved.IsAdded<TerrainDesignationsManager>(this, new Action<TreeStumpData>(this.stumpRemoved)))
        this.TreesManager.StumpRemoved.Add<TerrainDesignationsManager>(this, new Action<TreeStumpData>(this.stumpRemoved));
      if (saveVersion < 164)
        this.TreesManager.StumpAdded.Add<TerrainDesignationsManager>(this, new Action<TreeStumpData>(this.stumpAdded));
      this.ValidateAndFixAllDesignationsAndReportErrors(saveVersion >= 140);
    }

    [ConsoleCommand(false, false, null, null)]
    private GameCommandResult validateAndFixTerrainDesignations()
    {
      string error = this.ValidateAndFixAllDesignationsAndReportErrors();
      return !string.IsNullOrEmpty(error) ? GameCommandResult.Error(error) : GameCommandResult.Success((object) string.Format("Validation of {0} terrain designations found no errors.", (object) this.Count));
    }

    [ConsoleCommand(false, false, null, null)]
    private string toggleTerrainDesignationsOverEntities()
    {
      this.AllowDesignationsOverEntities = !this.AllowDesignationsOverEntities;
      return "Terrain designations over entities: " + (this.AllowDesignationsOverEntities ? "allowed" : "not allowed");
    }

    internal void SetFulfillAllDesignations_TestOnly(bool enabled)
    {
    }

    private void onDesignationAdded(TerrainDesignation designation)
    {
      Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
      this.m_designationAdded.Invoke(designation);
    }

    private void onDesignationUpdated(TerrainDesignation designation)
    {
      Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
      this.m_designationUpdated.Invoke(designation);
    }

    private void onDesignationFulfilledChanged(TerrainDesignation designation)
    {
      Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
      this.m_designationFulfilledChanged.Invoke(designation);
    }

    private void onDesignationRemoved(TerrainDesignation designation)
    {
      Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
      this.m_designationRemoved.Invoke(designation);
    }

    private void onTerminate() => this.ValidateAndFixAllDesignationsAndReportErrors();

    public void SetAllowDesignationsOverEntities(bool value)
    {
      this.AllowDesignationsOverEntities = value;
    }

    public IEnumerable<TerrainDesignation> SelectDesignationsInArea(
      Tile2i fromCoord,
      Tile2i toCoord)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TerrainDesignation>) new TerrainDesignationsManager.\u003CSelectDesignationsInArea\u003Ed__83(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__fromCoord = fromCoord,
        \u003C\u003E3__toCoord = toCoord
      };
    }

    public void UpdateSelectedDesignationsInArea(
      Tile2i fromCoord,
      Tile2i toCoord,
      Action<TerrainDesignation> added,
      Action<TerrainDesignation> removed,
      Predicate<TerrainDesignation> predicate,
      Set<TerrainDesignation> designations)
    {
      TerrainDesignationsManager.GetCanonicalDesignationRange(fromCoord, toCoord, out fromCoord, out toCoord);
      Lyst<TerrainDesignation> lyst = (Lyst<TerrainDesignation>) null;
      foreach (TerrainDesignation designation in designations)
      {
        if (!(designation.OriginTileCoord >= fromCoord) || !(designation.OriginTileCoord <= toCoord))
        {
          if (lyst == null)
            lyst = new Lyst<TerrainDesignation>();
          lyst.Add(designation);
        }
      }
      if (lyst != null)
      {
        foreach (TerrainDesignation terrainDesignation in lyst)
        {
          designations.Remove(terrainDesignation);
          removed(terrainDesignation);
        }
      }
      for (int y = fromCoord.Y; y <= toCoord.Y; y += 4)
      {
        for (int x = fromCoord.X; x <= toCoord.X; x += 4)
        {
          TerrainDesignation terrainDesignation;
          if (this.m_designations.TryGetValue(new Tile2i(x, y), out terrainDesignation) && predicate(terrainDesignation) && designations.Add(terrainDesignation))
            added(terrainDesignation);
        }
      }
    }

    private void stumpAdded(TreeStumpData data)
    {
      this.tileChanged(this.TerrainManager.ExtendTileIndex(data.Id.Position));
    }

    private void stumpRemoved(TreeStumpData data)
    {
      this.tileChanged(this.TerrainManager.ExtendTileIndex(data.Id.Position));
    }

    private void tileMaterialChanged(Tile2iAndIndex tile) => this.tileChangedInternal(tile, true);

    private void tileHeightChanged(Tile2iAndIndex tile) => this.tileChangedInternal(tile, false);

    private void tileChanged(Tile2iAndIndex tile) => this.tileChangedInternal(tile, false);

    private void tileChangedInternal(Tile2iAndIndex tile, bool materialOnly)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(tile.TileCoord);
      this.onTileChanged(tile, origin, materialOnly, true);
      if (((int) tile.X & 3) == 0)
      {
        this.onTileChanged(tile, origin.AddX(-4), materialOnly, false);
        if (((int) tile.Y & 3) == 0)
          this.onTileChanged(tile, origin.AddXy(-4), materialOnly, false);
      }
      if (((int) tile.Y & 3) != 0)
        return;
      this.onTileChanged(tile, origin.AddY(-4), materialOnly, false);
    }

    private void simUpdate()
    {
      if (!this.m_checkNotifyAreasWithoutTower)
        return;
      this.m_checkNotifyAreasWithoutTower = false;
      this.m_areasWithoutTowerNotif.NotifyIff(this.m_notOwnedDesignationsWarning.Any<TerrainDesignation>((Func<TerrainDesignation, bool>) (x => !x.IsForestry && x.IsNotFulfilled)));
      this.m_areasWithoutForestryTowerNotif.NotifyIff(this.m_notOwnedDesignationsWarning.Any<TerrainDesignation>((Func<TerrainDesignation, bool>) (x => x.IsForestry)));
    }

    private void onTileChanged(
      Tile2iAndIndex tileAndIndex,
      Tile2i canonicalCoord,
      bool materialOnly,
      bool originalOrigin)
    {
      TerrainDesignation designation;
      if (!this.tryGetDesignationAtOrigin(canonicalCoord, out designation) || materialOnly && designation.Prototype.IsTerraforming)
        return;
      bool isFulfilled1 = designation.IsFulfilled;
      designation.TileChanged(tileAndIndex);
      bool isFulfilled2 = designation.IsFulfilled;
      if (isFulfilled1 == isFulfilled2)
        return;
      if (isFulfilled2)
        this.m_notFulfilledAreas.Remove(designation.OriginTileCoord);
      else
        this.m_notFulfilledAreas[designation.OriginTileCoord] = designation;
      this.onDesignationFulfilledChanged(designation);
    }

    /// <summary>
    /// Returns min and max area origin between two given coords. Size of returned area is clamped to <see cref="F:Mafi.Core.Terrain.Designation.TerrainDesignationsManager.MAX_DESIGNATION_AREA_SIZE" />.
    /// </summary>
    public static void GetCanonicalDesignationRange(
      Tile2i fromCoord,
      Tile2i toCoord,
      out Tile2i minCoord,
      out Tile2i maxCoord)
    {
      minCoord = TerrainDesignation.GetOrigin(fromCoord.Min(toCoord));
      maxCoord = TerrainDesignation.GetOrigin(fromCoord.Max(toCoord));
      RelTile2i relTile2i = maxCoord - minCoord;
      if (relTile2i.X > 192)
      {
        Log.Warning(string.Format("Too large mine area designated {0}, clamping.", (object) relTile2i));
        maxCoord = maxCoord.SetX(minCoord.X + 192);
      }
      if (relTile2i.Y <= 192)
        return;
      Log.Warning(string.Format("Too large mine area designated {0}, clamping.", (object) relTile2i));
      maxCoord = maxCoord.SetY(minCoord.Y + 192);
    }

    /// <summary>Returns designated area on given tile coordinate.</summary>
    public Option<TerrainDesignation> GetDesignationAt(Tile2i coord)
    {
      TerrainDesignation designation;
      this.tryGetDesignationAtOrigin(TerrainDesignation.GetOrigin(coord), out designation);
      return (Option<TerrainDesignation>) designation;
    }

    public bool TryGetAnyDesignationAtVertex(Tile2i coord, out TerrainDesignation designation)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(coord);
      if (this.tryGetDesignationAtOrigin(TerrainDesignation.GetOrigin(origin), out designation) || coord.X == origin.X && (this.tryGetDesignationAtOrigin(origin.AddX(-4), out designation) || coord.Y == origin.Y && this.tryGetDesignationAtOrigin(origin.AddXy(-4), out designation)) || coord.Y == origin.Y && this.tryGetDesignationAtOrigin(origin.AddY(-4), out designation))
        return true;
      designation = (TerrainDesignation) null;
      return false;
    }

    /// <summary>Whether there is any designation at given coord.</summary>
    public bool HasDesignationAt(Tile2i coord)
    {
      return this.hasDesignationAtOrigin(TerrainDesignation.GetOrigin(coord));
    }

    private bool tryGetDesignationAtOrigin(Tile2i originCoord, out TerrainDesignation designation)
    {
      return this.m_designations.TryGetValue(originCoord, out designation);
    }

    private bool hasDesignationAtOrigin(Tile2i originCoord)
    {
      Assert.That<Tile2i>(originCoord).IsEqualTo<Tile2i>(TerrainDesignation.GetOrigin(originCoord));
      return this.m_designations.ContainsKey(originCoord);
    }

    public bool AddOrReplaceDesignation(TerrainDesignationProto proto, DesignationData data)
    {
      if (!this.IsDesignationAllowed(data, out LocStrFormatted _))
        return false;
      TerrainDesignation designation = new TerrainDesignation(proto, data);
      designation.SetManager((ITerrainDesignationsManagerInternal) this);
      this.RemoveDesignation(designation.OriginTileCoord);
      this.m_surfaceDesignationsManager.Value.RemoveDesignation(designation.OriginTileCoord);
      this.m_designations.Add(designation.OriginTileCoord, designation);
      if (!designation.IsFulfilled)
        this.m_notFulfilledAreas[designation.OriginTileCoord] = designation;
      this.checkForNoTowerWarning(designation);
      this.onDesignationAdded(designation);
      return true;
    }

    public bool RemoveDesignation(Tile2i originCoord)
    {
      Assert.That<Tile2i>(originCoord).IsEqualTo<Tile2i>(TerrainDesignation.GetOrigin(originCoord));
      TerrainDesignation designation;
      if (!this.m_designations.TryRemove(originCoord, out designation))
        return false;
      this.onDesignationRemoved(designation);
      this.m_notFulfilledAreas.Remove(designation.OriginTileCoord);
      this.m_notOwnedDesignationsWarning.Remove(designation);
      this.m_checkNotifyAreasWithoutTower = true;
      designation.SetDestroyed();
      return true;
    }

    public void ClearAllDesignations()
    {
      foreach (TerrainDesignation designation in this.m_designations.Values)
      {
        this.onDesignationRemoved(designation);
        designation.SetDestroyed();
      }
      this.m_designations.Clear();
      this.m_notFulfilledAreas.Clear();
      this.m_notOwnedDesignationsWarning.Clear();
      this.m_areasWithoutTowerNotif.Deactivate();
      this.m_areasWithoutForestryTowerNotif.Deactivate();
    }

    public bool TryFindBestReadyToFulfill(
      IEnumerable<TerrainDesignation> designations,
      Tile2i position,
      Vehicle servicingVehicle,
      out TerrainDesignation bestDesignation,
      Option<LooseProductProto> productToPrefer = default (Option<LooseProductProto>),
      bool tryIgnoreReservations = false,
      Predicate<TerrainDesignation> predicate = null,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null,
      bool preferDesignationNearAssignedEntity = false,
      bool isMining = false,
      bool isDumping = false)
    {
      Assert.That<bool>(isMining | isDumping).IsTrue();
      position -= new RelTile2i(2, 2);
      Tile2i? towerPositionToHandle = new Tile2i?();
      if (preferDesignationNearAssignedEntity && servicingVehicle.AssignedTo.HasValue && servicingVehicle.AssignedTo.Value is IEntityWithPosition entityWithPosition)
        towerPositionToHandle = new Tile2i?(entityWithPosition.Position2f.Tile2i);
      if (!TerrainDesignationsManager.tryFindBestDesignation(designations, new Func<TerrainDesignation, Fix32>(scoringFn), out bestDesignation))
        return false;
      if (additionalNearbyDesignations != null)
      {
        foreach (TerrainDesignation designation in designations)
        {
          if (!designation.IsFulfilled && designation != bestDesignation && designation.OriginTileCoord.DistanceToOrtho(bestDesignation.OriginTileCoord).Value <= 12 && (predicate == null || predicate(designation)))
            additionalNearbyDesignations.Add(designation);
        }
      }
      return true;

      Fix32 scoringFn(TerrainDesignation designation)
      {
        if (designation.IsFulfilled || !designation.IsMiningReadyToBeFulfilled & isMining || !designation.IsDumpingReadyToBeFulfilled & isDumping || !designation.CanBeAssigned(tryIgnoreReservations) || predicate != null && !predicate(designation))
          return Fix32.MaxValue;
        Assert.That<bool>(designation.IsFulfilled).IsFalse();
        Fix32 fix32_1;
        if (towerPositionToHandle.HasValue)
        {
          RelTile2i other = designation.OriginTileCoord - position;
          RelTile2i relTile2i = designation.OriginTileCoord - towerPositionToHandle.Value;
          Fix32 fix32_2 = other.IsZero || relTile2i.IsZero ? (Fix32) 0 : (Fix32) 1 - relTile2i.AngleBetween(other).Degrees / 90;
          RelTile2i absValue = relTile2i.AbsValue;
          Fix32 sum1 = (Fix32) absValue.Sum;
          Fix32 fix32_3 = fix32_2;
          absValue = other.AbsValue;
          int sum2 = absValue.Sum;
          Fix32 fix32_4 = fix32_3 * sum2 / 2;
          fix32_1 = sum1 + fix32_4;
        }
        else
        {
          RelTile2i relTile2i = position - designation.OriginTileCoord;
          relTile2i = relTile2i.AbsValue;
          fix32_1 = (Fix32) (relTile2i.Sum / 2);
        }
        Fix32 fix32_5 = fix32_1 + designation.NumberOfJobsAssigned * 20.ToFix32() + (Fix32) ((25 - designation.CountFulfilledTiles()) / 2) + (int) designation.UnreachableVehiclesCount * 50.ToFix32();
        if (productToPrefer.HasValue)
        {
          Percent percent = designation.RatioOfNonFulfilledTilesWithoutProduct(productToPrefer.Value);
          if (percent >= Percent.Hundred)
            fix32_5 += (Fix32) 1000;
          fix32_5 += (Fix32) percent.Apply(100);
        }
        return fix32_5;
      }
    }

    [Pure]
    private static bool tryFindBestDesignation(
      IEnumerable<TerrainDesignation> areas,
      Func<TerrainDesignation, Fix32> scoringFn,
      out TerrainDesignation bestDesignation)
    {
      Fix32 fix32_1 = Fix32.MaxValue;
      bestDesignation = (TerrainDesignation) null;
      foreach (TerrainDesignation area in areas)
      {
        if (area == null)
        {
          Log.Warning("tryFindBestDesignation was given a null designation, skipping");
        }
        else
        {
          Fix32 fix32_2 = scoringFn(area);
          if (!(fix32_2 == Fix32.MaxValue) && fix32_2 < fix32_1)
          {
            fix32_1 = fix32_2;
            bestDesignation = area;
          }
        }
      }
      return bestDesignation != null;
    }

    public bool IsDesignationAllowed(DesignationData designationData, out LocStrFormatted error)
    {
      Tile2i originTile = designationData.OriginTile;
      if (originTile != TerrainDesignation.GetOrigin(originTile))
      {
        error = (LocStrFormatted) TrCore.DesignationError__Invalid;
        return false;
      }
      if (originTile.X < this.TerrainManager.MinOnLimits.X || originTile.Y < this.TerrainManager.MinOnLimits.Y || originTile.X + 4 > this.TerrainManager.MaxOnLimitsIncl.X || originTile.Y + 4 > this.TerrainManager.MaxOnLimitsIncl.Y)
      {
        error = (LocStrFormatted) TrCore.DesignationError__Invalid;
        return false;
      }
      if (this.AllowDesignationsOverEntities)
      {
        error = LocStrFormatted.Empty;
        return true;
      }
      for (int y = 0; y < 4; ++y)
      {
        for (int x = 0; x < 4; ++x)
        {
          Tile2iIndex tileIndex = this.TerrainManager.GetTileIndex(originTile + new RelTile2i(x, y));
          IStaticEntity entity;
          if (this.OccupancyManager.IsOccupied(tileIndex) && this.OccupancyManager.TryGetOccupyingEntity(tileIndex, (Predicate<IStaticEntity>) (p => p.Prototype is ITerrainDesignationBlockingEntityNoEdgeProto), out entity))
          {
            error = TrCore.TrAdditionError__Blocked.Format(entity.GetTitle());
            return false;
          }
        }
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public HeightTilesI? GetDesignatedHeightAtOrigin(Tile2i origin)
    {
      Assert.That<Tile2i>(origin).IsEqualTo<Tile2i>(TerrainDesignation.GetOrigin(origin));
      TerrainDesignation terrainDesignation;
      if (this.m_designations.TryGetValue(origin, out terrainDesignation))
        return new HeightTilesI?(terrainDesignation.Data.OriginTargetHeight);
      if (this.m_designations.TryGetValue(origin.AddX(-4), out terrainDesignation))
        return new HeightTilesI?(terrainDesignation.Data.PlusXTargetHeight);
      if (this.m_designations.TryGetValue(origin.AddY(-4), out terrainDesignation))
        return new HeightTilesI?(terrainDesignation.Data.PlusYTargetHeight);
      return this.m_designations.TryGetValue(origin.AddXy(-4), out terrainDesignation) ? new HeightTilesI?(terrainDesignation.Data.PlusXyTargetHeight) : new HeightTilesI?();
    }

    public bool TryGetDesignatedHeightRangeAtOrigin(
      Tile2i origin,
      out HeightTilesI minHeight,
      out HeightTilesI maxHeight)
    {
      Assert.That<Tile2i>(origin).IsEqualTo<Tile2i>(TerrainDesignation.GetOrigin(origin));
      minHeight = HeightTilesI.MaxValue;
      maxHeight = HeightTilesI.MinValue;
      TerrainDesignation terrainDesignation;
      if (this.m_designations.TryGetValue(origin, out terrainDesignation))
      {
        minHeight = minHeight.Min(terrainDesignation.Data.OriginTargetHeight);
        maxHeight = maxHeight.Max(terrainDesignation.Data.OriginTargetHeight);
      }
      if (this.m_designations.TryGetValue(origin.AddX(-4), out terrainDesignation))
      {
        minHeight = minHeight.Min(terrainDesignation.Data.PlusXTargetHeight);
        maxHeight = maxHeight.Max(terrainDesignation.Data.PlusXTargetHeight);
      }
      if (this.m_designations.TryGetValue(origin.AddY(-4), out terrainDesignation))
      {
        minHeight = minHeight.Min(terrainDesignation.Data.PlusYTargetHeight);
        maxHeight = maxHeight.Max(terrainDesignation.Data.PlusYTargetHeight);
      }
      if (this.m_designations.TryGetValue(origin.AddXy(-4), out terrainDesignation))
      {
        minHeight = minHeight.Min(terrainDesignation.Data.PlusXyTargetHeight);
        maxHeight = maxHeight.Max(terrainDesignation.Data.PlusXyTargetHeight);
      }
      return minHeight != HeightTilesI.MaxValue;
    }

    void ITerrainDesignationsManagerInternal.OnManagedTowersChanged(TerrainDesignation designation)
    {
      this.checkForNoTowerWarning(designation);
      this.m_designationManagedTowersChanged.Invoke(designation);
    }

    void ITerrainDesignationsManagerInternal.OnReachabilityChanged(TerrainDesignation designation)
    {
      this.m_designationManagedTowersChanged.Invoke(designation);
    }

    bool ITerrainDesignationsManagerInternal.TryAddAssignedJob(
      TerrainDesignation designation,
      IVehicleJob job,
      out int numberOfJobsAssigned)
    {
      Lyst<IVehicleJob> instance;
      if (!this.m_designationsWithJobs.TryGetValue(designation, out instance))
      {
        instance = this.m_jobListsPool.GetInstance();
        this.m_designationsWithJobs.Add(designation, instance);
      }
      bool flag = instance.AddIfNotPresent(job);
      numberOfJobsAssigned = instance.Count;
      return flag;
    }

    int ITerrainDesignationsManagerInternal.RemoveAssignedJob(
      TerrainDesignation designation,
      IVehicleJob job)
    {
      Lyst<IVehicleJob> lyst;
      if (!this.m_designationsWithJobs.TryGetValue(designation, out lyst))
      {
        Log.Error("Removing job for designation with no jobs.");
        return 0;
      }
      lyst.RemoveAndAssert(job);
      if (!lyst.IsEmpty)
        return lyst.Count;
      this.m_designationsWithJobs.Remove(designation);
      this.m_jobListsPool.ReturnInstance(ref lyst);
      return 0;
    }

    IIndexable<IVehicleJob> ITerrainDesignationsManagerInternal.GetAssignedJobsFor(
      TerrainDesignation designation)
    {
      Lyst<IVehicleJob> lyst;
      return this.m_designationsWithJobs.TryGetValue(designation, out lyst) ? (IIndexable<IVehicleJob>) lyst : Indexable<IVehicleJob>.Empty;
    }

    private void checkForNoTowerWarning(TerrainDesignation designation)
    {
      if (!designation.Prototype.DisplayWarningWhenNotOwned)
        return;
      if (designation.ManagedByTowers.IsEmpty() && !designation.IsDestroyed)
        this.m_notOwnedDesignationsWarning.Add(designation);
      else
        this.m_notOwnedDesignationsWarning.Remove(designation);
      this.m_checkNotifyAreasWithoutTower = true;
    }

    void IAction<AddTerrainDesignationsCmd>.Invoke(AddTerrainDesignationsCmd cmd)
    {
      TerrainDesignationProto proto;
      if (!this.m_protosDb.TryGetProto<TerrainDesignationProto>(cmd.ProtoId, out proto))
      {
        cmd.SetResultError(ImmutableArray<Tile2i>.Empty, string.Format("Failed to find proto '{0}'.", (object) cmd.ProtoId));
      }
      else
      {
        int length = cmd.Data.Length;
        ImmutableArrayBuilder<Tile2i> immutableArrayBuilder = new ImmutableArrayBuilder<Tile2i>(length);
        for (int index = 0; index < length; ++index)
        {
          DesignationData data = cmd.Data[index];
          this.AddOrReplaceDesignation(proto, data);
          immutableArrayBuilder[index] = data.OriginTile;
        }
        cmd.SetResultSuccess(immutableArrayBuilder.GetImmutableArrayAndClear());
      }
    }

    void IAction<RemoveDesignationsCmd>.Invoke(RemoveDesignationsCmd cmd)
    {
      Lyst<Tile2i> lyst = new Lyst<Tile2i>(cmd.Origins.Length);
      foreach (Tile2i origin in cmd.Origins)
        this.RemoveDesignation(origin);
      cmd.SetResultSuccess(lyst.ToImmutableArray());
    }

    public string ValidateAndFixAllDesignationsAndReportErrors(bool logErrors = true)
    {
      Lyst<KeyValuePair<Tile2i, bool>> lyst = this.ValidateAndFixAllDesignations();
      if (lyst.IsEmpty)
        return "";
      string message = "Terrain designations validation failed:\n" + ((IEnumerable<string>) lyst.Select<string>((Func<KeyValuePair<Tile2i, bool>, string>) (x => string.Format("{0} bitmap value: {1}fulfilled. ", (object) x.Key, x.Value ? (object) "" : (object) "not ") + "Function value " + (x.Value ? "not " : "") + "fulfilled."))).JoinStrings("\n");
      if (logErrors)
        Log.Error(message);
      return message;
    }

    public Lyst<KeyValuePair<Tile2i, bool>> ValidateAndFixAllDesignations()
    {
      Lyst<KeyValuePair<Tile2i, bool>> errorLocations = new Lyst<KeyValuePair<Tile2i, bool>>();
      foreach (TerrainDesignation terrainDesignation in this.m_designations.Values)
        terrainDesignation.ValidateAndFixFulfilledBitmap(errorLocations);
      return errorLocations;
    }

    static TerrainDesignationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainDesignationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainDesignationsManager) obj).SerializeData(writer));
      TerrainDesignationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainDesignationsManager) obj).DeserializeData(reader));
    }
  }
}
