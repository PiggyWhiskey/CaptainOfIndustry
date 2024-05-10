// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.SurfaceDesignationsManager
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
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class SurfaceDesignationsManager : 
    ISurfaceDesignationsManagerInternal,
    ISurfaceDesignationsManager,
    ICommandProcessor<AddSurfaceDesignationsCmd>,
    IAction<AddSurfaceDesignationsCmd>,
    ICommandProcessor<RemoveSurfaceDesignationsCmd>,
    IAction<RemoveSurfaceDesignationsCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static Fix32 SURFACE_HEIGHT_TOLERANCE_WHEN_PLACING;
    private readonly Event<SurfaceDesignation> m_designationAdded;
    private readonly Event<SurfaceDesignation> m_designationUpdated;
    private readonly Event<SurfaceDesignation> m_designationFulfilledChanged;
    private readonly Event<SurfaceDesignation> m_designationRemoved;
    /// <summary>
    /// All managed placing designations. Is mutated only on sim thread.
    /// </summary>
    private readonly Dict<Tile2i, SurfaceDesignation> m_placingDesignations;
    /// <summary>
    /// All managed clearing designations. Is mutated only on sim thread.
    /// </summary>
    private readonly Dict<Tile2i, SurfaceDesignation> m_clearingDesignations;
    private Dict<SurfaceDesignation, Lyst<IVehicleJobWithOwner>> m_designationsWithJobs;
    [DoNotSave(0, null)]
    private readonly ObjectPool2<Lyst<IVehicleJobWithOwner>> m_jobListsPool;
    private readonly LazyResolve<TerrainDesignationsManager> m_terrainDesignationsManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly ProtosDb m_protosDb;
    private readonly LazyResolve<IVehiclePathFindingManager> m_vehiclePathFindingManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly ITreesManager m_treesManager;
    private readonly LazyResolve<UnreachableTerrainDesignationsManager> m_unreachablesManager;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<DesignationsPerProductCache> m_designationsPerProductCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<SurfaceDesignation> m_designationsCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<KeyValuePair<long, SurfaceDesignation>> m_designationsTmp;
    [DoNotSave(0, null)]
    private bool m_areCachesValid;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<SurfaceDesignation> m_invalidDesignationsCache;

    public static void Serialize(SurfaceDesignationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SurfaceDesignationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SurfaceDesignationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<Tile2i, SurfaceDesignation>.Serialize(this.m_clearingDesignations, writer);
      Event<SurfaceDesignation>.Serialize(this.m_designationAdded, writer);
      Event<SurfaceDesignation>.Serialize(this.m_designationFulfilledChanged, writer);
      Event<SurfaceDesignation>.Serialize(this.m_designationRemoved, writer);
      Dict<SurfaceDesignation, Lyst<IVehicleJobWithOwner>>.Serialize(this.m_designationsWithJobs, writer);
      Event<SurfaceDesignation>.Serialize(this.m_designationUpdated, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      Dict<Tile2i, SurfaceDesignation>.Serialize(this.m_placingDesignations, writer);
      LazyResolve<TerrainDesignationsManager>.Serialize(this.m_terrainDesignationsManager, writer);
      writer.WriteGeneric<ITreesManager>(this.m_treesManager);
      LazyResolve<UnreachableTerrainDesignationsManager>.Serialize(this.m_unreachablesManager, writer);
      LazyResolve<IVehiclePathFindingManager>.Serialize(this.m_vehiclePathFindingManager, writer);
      TerrainManager.Serialize(this.TerrainManager, writer);
    }

    public static SurfaceDesignationsManager Deserialize(BlobReader reader)
    {
      SurfaceDesignationsManager designationsManager;
      if (reader.TryStartClassDeserialization<SurfaceDesignationsManager>(out designationsManager))
        reader.EnqueueDataDeserialization((object) designationsManager, SurfaceDesignationsManager.s_deserializeDataDelayedAction);
      return designationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SurfaceDesignationsManager>(this, "m_clearingDesignations", (object) Dict<Tile2i, SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationAdded", (object) Event<SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationFulfilledChanged", (object) Event<SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationRemoved", (object) Event<SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationsCache", (object) new Lyst<SurfaceDesignation>());
      this.m_designationsPerProductCache = new LystStruct<DesignationsPerProductCache>();
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationsTmp", (object) new Lyst<KeyValuePair<long, SurfaceDesignation>>());
      this.m_designationsWithJobs = Dict<SurfaceDesignation, Lyst<IVehicleJobWithOwner>>.Deserialize(reader);
      reader.SetField<SurfaceDesignationsManager>(this, "m_designationUpdated", (object) Event<SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      this.m_invalidDesignationsCache = new LystStruct<SurfaceDesignation>();
      reader.SetField<SurfaceDesignationsManager>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_placingDesignations", (object) Dict<Tile2i, SurfaceDesignation>.Deserialize(reader));
      reader.RegisterResolvedMember<SurfaceDesignationsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<SurfaceDesignationsManager>(this, "m_terrainDesignationsManager", (object) LazyResolve<TerrainDesignationsManager>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_treesManager", (object) reader.ReadGenericAs<ITreesManager>());
      reader.SetField<SurfaceDesignationsManager>(this, "m_unreachablesManager", (object) LazyResolve<UnreachableTerrainDesignationsManager>.Deserialize(reader));
      reader.SetField<SurfaceDesignationsManager>(this, "m_vehiclePathFindingManager", (object) LazyResolve<IVehiclePathFindingManager>.Deserialize(reader));
      this.TerrainManager = TerrainManager.Deserialize(reader);
      reader.RegisterInitAfterLoad<SurfaceDesignationsManager>(this, "initDesignations", InitPriority.Highest);
      reader.RegisterInitAfterLoad<SurfaceDesignationsManager>(this, "initAfterLoad", InitPriority.Low);
      reader.RegisterInitAfterLoad<SurfaceDesignationsManager>(this, "initAfterAllLoaded", InitPriority.Low);
    }

    /// <summary>
    /// Invoked when new designation is added. Always raised on Sim thread.
    /// </summary>
    public IEvent<SurfaceDesignation> DesignationAdded
    {
      get => (IEvent<SurfaceDesignation>) this.m_designationAdded;
    }

    /// <summary>
    /// Invoked when new designation is updated. Always raised on Sim thread.
    /// </summary>
    public IEvent<SurfaceDesignation> DesignationUpdated
    {
      get => (IEvent<SurfaceDesignation>) this.m_designationUpdated;
    }

    /// <summary>
    /// Invoked when the designation state is changed for example its fulfilled state. Always raised on Sim thread.
    /// </summary>
    public IEvent<SurfaceDesignation> DesignationFulfilledChanged
    {
      get => (IEvent<SurfaceDesignation>) this.m_designationFulfilledChanged;
    }

    /// <summary>
    /// Invoked when new designation is removed. Always raised on Sim thread.
    /// </summary>
    public IEvent<SurfaceDesignation> DesignationRemoved
    {
      get => (IEvent<SurfaceDesignation>) this.m_designationRemoved;
    }

    public IReadOnlyCollection<SurfaceDesignation> PlacingDesignations
    {
      get => (IReadOnlyCollection<SurfaceDesignation>) this.m_placingDesignations.Values;
    }

    public IReadOnlyCollection<SurfaceDesignation> ClearingDesignations
    {
      get => (IReadOnlyCollection<SurfaceDesignation>) this.m_clearingDesignations.Values;
    }

    public IReadOnlyDictionary<Tile2i, SurfaceDesignation> PlacingDesignationsDict
    {
      get => (IReadOnlyDictionary<Tile2i, SurfaceDesignation>) this.m_placingDesignations;
    }

    public IReadOnlyDictionary<Tile2i, SurfaceDesignation> ClearingDesignationsDict
    {
      get => (IReadOnlyDictionary<Tile2i, SurfaceDesignation>) this.m_clearingDesignations;
    }

    public int Count => this.m_placingDesignations.Count + this.m_clearingDesignations.Count;

    public TerrainManager TerrainManager { get; private set; }

    [DoNotSave(0, null)]
    internal bool IsFulfillingAllDesignations { get; private set; }

    public SurfaceDesignationsManager(
      TerrainManager terrainManager,
      LazyResolve<TerrainDesignationsManager> terrainDesignationsManager,
      ProtosDb protosDb,
      LazyResolve<IVehiclePathFindingManager> vehiclePathFindingManager,
      TerrainOccupancyManager occupancyManager,
      ISimLoopEvents simLoopEvents,
      IGameLoopEvents gameLoopEvents,
      IEntitiesManager entitiesManager,
      ITreesManager treesManager,
      LazyResolve<UnreachableTerrainDesignationsManager> unreachablesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_designationAdded = new Event<SurfaceDesignation>();
      this.m_designationUpdated = new Event<SurfaceDesignation>();
      this.m_designationFulfilledChanged = new Event<SurfaceDesignation>();
      this.m_designationRemoved = new Event<SurfaceDesignation>();
      this.m_placingDesignations = new Dict<Tile2i, SurfaceDesignation>();
      this.m_clearingDesignations = new Dict<Tile2i, SurfaceDesignation>();
      this.m_designationsWithJobs = new Dict<SurfaceDesignation, Lyst<IVehicleJobWithOwner>>();
      this.m_jobListsPool = new ObjectPool2<Lyst<IVehicleJobWithOwner>>(256, (Func<ObjectPool2<Lyst<IVehicleJobWithOwner>>, Lyst<IVehicleJobWithOwner>>) (p => new Lyst<IVehicleJobWithOwner>()), (Action<Lyst<IVehicleJobWithOwner>>) (l => l.Clear()));
      this.m_designationsCache = new Lyst<SurfaceDesignation>();
      this.m_designationsTmp = new Lyst<KeyValuePair<long, SurfaceDesignation>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainManager = terrainManager;
      this.m_terrainDesignationsManager = terrainDesignationsManager;
      this.m_protosDb = protosDb;
      this.m_vehiclePathFindingManager = vehiclePathFindingManager;
      this.m_occupancyManager = occupancyManager;
      this.m_entitiesManager = entitiesManager;
      this.m_treesManager = treesManager;
      this.m_unreachablesManager = unreachablesManager;
      terrainManager.HeightChanged.Add<SurfaceDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileHeightChanged));
      terrainManager.TileCustomSurfaceChanged.Add<SurfaceDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileSurfaceChanged));
      occupancyManager.TileOccupancyChanged.Add<SurfaceDesignationsManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      treesManager.TreeAdded.Add<SurfaceDesignationsManager>(this, new Action<TreeData>(this.treeAdded));
      treesManager.ManualTreePlaced.Add<SurfaceDesignationsManager>(this, new Action<TreeId>(this.manualTreePlaced));
      simLoopEvents.Update.Add<SurfaceDesignationsManager>(this, new Action(this.simUpdate));
      gameLoopEvents.Terminate.AddNonSaveable<SurfaceDesignationsManager>(this, new Action(this.onTerminate));
      this.initCaches();
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initDesignations()
    {
      ReflectionUtils.SetField<SurfaceDesignationsManager>(this, "m_jobListsPool", (object) new ObjectPool2<Lyst<IVehicleJobWithOwner>>(256, (Func<ObjectPool2<Lyst<IVehicleJobWithOwner>>, Lyst<IVehicleJobWithOwner>>) (p => new Lyst<IVehicleJobWithOwner>()), (Action<Lyst<IVehicleJobWithOwner>>) (l => l.Clear())));
      foreach (KeyValuePair<SurfaceDesignation, Lyst<IVehicleJobWithOwner>> designationsWithJob in this.m_designationsWithJobs)
        designationsWithJob.Key.InitNumberOfJobsAssigned(designationsWithJob.Value.Count);
      this.initCaches();
    }

    [InitAfterLoad(InitPriority.Low)]
    [OnlyForSaveCompatibility(null)]
    private void initAfterLoad(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      this.m_treesManager.TreeAdded.Add<SurfaceDesignationsManager>(this, new Action<TreeData>(this.treeAdded));
      this.m_treesManager.ManualTreePlaced.Add<SurfaceDesignationsManager>(this, new Action<TreeId>(this.manualTreePlaced));
    }

    private void initCaches()
    {
      foreach (TerrainTileSurfaceProto tileSurfaceProto in this.m_protosDb.All<TerrainTileSurfaceProto>())
      {
        ProductProto product = tileSurfaceProto.CostPerTile.Product;
        if (!tileSurfaceProto.CostPerTile.IsEmpty && !product.IsPhantom && !this.tryGetCacheFor(product, out DesignationsPerProductCache _))
          this.m_designationsPerProductCache.Add(new DesignationsPerProductCache(product));
      }
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initAfterAllLoaded(int saveVersion)
    {
      this.ValidateAndFixAllDesignationsAndReportErrors();
    }

    [ConsoleCommand(false, false, null, null)]
    private GameCommandResult validateAndFixSurfaceDesignations()
    {
      string error = this.ValidateAndFixAllDesignationsAndReportErrors();
      return !string.IsNullOrEmpty(error) ? GameCommandResult.Error(error) : GameCommandResult.Success((object) string.Format("Validation of {0} surface designations found no errors.", (object) this.Count));
    }

    internal void SetFulfillAllDesignations_TestOnly(bool enabled)
    {
    }

    private void onDesignationAdded(SurfaceDesignation designation)
    {
      Assert.That<SurfaceDesignation>(designation).IsNotNull<SurfaceDesignation>();
      this.m_areCachesValid = false;
      this.m_designationAdded.Invoke(designation);
    }

    private void onDesignationUpdated(SurfaceDesignation designation)
    {
      Assert.That<SurfaceDesignation>(designation).IsNotNull<SurfaceDesignation>();
      if (this.removeIfFulfilled(designation))
        return;
      if (designation.NumberOfJobsAssigned == 0)
        this.m_areCachesValid = false;
      this.m_designationUpdated.Invoke(designation);
    }

    private void onDesignationFulfilledChanged(SurfaceDesignation designation)
    {
      Assert.That<SurfaceDesignation>(designation).IsNotNull<SurfaceDesignation>();
      if (this.removeIfFulfilled(designation))
        return;
      this.m_designationFulfilledChanged.Invoke(designation);
    }

    private bool removeIfFulfilled(SurfaceDesignation designation)
    {
      if (!designation.IsFulfilled)
        return false;
      if (designation.IsPlacing)
        this.removePlacingDesignation(designation.OriginTileCoord);
      else
        this.removeClearingDesignation(designation.OriginTileCoord);
      return true;
    }

    private void onTerminate() => this.ValidateAndFixAllDesignationsAndReportErrors();

    public void UpdateSelectedDesignationsInArea(
      Tile2i fromCoord,
      Tile2i toCoord,
      Action<SurfaceDesignation> added,
      Action<SurfaceDesignation> removed,
      Set<SurfaceDesignation> designations)
    {
      SurfaceDesignationsManager.GetCanonicalDesignationRange(fromCoord, toCoord, out fromCoord, out toCoord);
      Lyst<SurfaceDesignation> lyst = (Lyst<SurfaceDesignation>) null;
      foreach (SurfaceDesignation designation in designations)
      {
        if (!(designation.OriginTileCoord >= fromCoord) || !(designation.OriginTileCoord <= toCoord))
        {
          if (lyst == null)
            lyst = new Lyst<SurfaceDesignation>();
          lyst.Add(designation);
        }
      }
      if (lyst != null)
      {
        foreach (SurfaceDesignation surfaceDesignation in lyst)
        {
          designations.Remove(surfaceDesignation);
          removed(surfaceDesignation);
        }
      }
      for (int y = fromCoord.Y; y <= toCoord.Y; y += 4)
      {
        for (int x = fromCoord.X; x <= toCoord.X; x += 4)
        {
          SurfaceDesignation surfaceDesignation1;
          if (this.m_placingDesignations.TryGetValue(new Tile2i(x, y), out surfaceDesignation1) && designations.Add(surfaceDesignation1))
            added(surfaceDesignation1);
          SurfaceDesignation surfaceDesignation2;
          if (this.m_clearingDesignations.TryGetValue(new Tile2i(x, y), out surfaceDesignation2) && designations.Add(surfaceDesignation2))
            added(surfaceDesignation2);
        }
      }
    }

    private void tileHeightChanged(Tile2iAndIndex tileAndIndex)
    {
      bool flag1 = tileAndIndex.TileCoord.X == 0;
      bool flag2 = tileAndIndex.TileCoord.Y == 0;
      Tile2iAndIndex?[] nullableArray = new Tile2iAndIndex?[4]
      {
        new Tile2iAndIndex?(tileAndIndex),
        flag1 ? new Tile2iAndIndex?() : new Tile2iAndIndex?(tileAndIndex.MinusXNeighborUnchecked),
        flag2 ? new Tile2iAndIndex?() : new Tile2iAndIndex?(tileAndIndex.MinusYNeighborUnchecked(this.TerrainManager.TerrainWidth)),
        flag1 | flag2 ? new Tile2iAndIndex?() : new Tile2iAndIndex?(tileAndIndex.MinusXMinusYNeighborUnchecked(this.TerrainManager.TerrainWidth))
      };
      foreach (Tile2iAndIndex? nullable in nullableArray)
      {
        if (nullable.HasValue)
        {
          Tile2i tileCoord = nullable.Value.TileCoord;
          Tile2i origin = SurfaceDesignation.GetOrigin(tileCoord);
          SurfaceDesignation designation1;
          if (this.m_placingDesignations.TryGetValue(origin, out designation1) && designation1.IsAssignedAt(tileCoord))
          {
            bool flag3 = designation1.IsFulfilledAt(tileCoord);
            designation1.TileChanged(nullable.Value);
            if (designation1.IsFulfilledAt(tileCoord) != flag3)
              this.onDesignationFulfilledChanged(designation1);
          }
          SurfaceDesignation designation2;
          if (this.m_clearingDesignations.TryGetValue(origin, out designation2) && designation2.IsAssignedAt(tileCoord))
          {
            bool flag4 = designation2.IsFulfilledAt(tileCoord);
            designation2.TileChanged(nullable.Value);
            if (designation2.IsFulfilledAt(tileCoord) != flag4)
              this.onDesignationFulfilledChanged(designation2);
          }
        }
      }
    }

    private void tileSurfaceChanged(Tile2iAndIndex tile) => this.onTileChanged(tile);

    private void tileChanged(Tile2iAndIndex tile) => this.onTileChanged(tile);

    private void treeAdded(TreeData treeData)
    {
      this.onTileChanged(treeData.Id.Position.ExtendIndex(this.TerrainManager));
    }

    private void manualTreePlaced(TreeId treeId)
    {
      this.onTileChanged(treeId.Position.ExtendIndex(this.TerrainManager));
    }

    private void onTileChanged(Tile2iAndIndex tileAndIndex)
    {
      Tile2i origin = SurfaceDesignation.GetOrigin(tileAndIndex.TileCoord);
      SurfaceDesignation designation1;
      if (this.m_placingDesignations.TryGetValue(origin, out designation1) && designation1.IsAssignedAt(tileAndIndex.TileCoord))
      {
        bool flag = designation1.IsFulfilledAt(tileAndIndex.TileCoord);
        designation1.TileChanged(tileAndIndex);
        if (designation1.IsFulfilledAt(tileAndIndex.TileCoord) != flag)
          this.onDesignationFulfilledChanged(designation1);
      }
      SurfaceDesignation designation2;
      if (!this.m_clearingDesignations.TryGetValue(origin, out designation2) || !designation2.IsAssignedAt(tileAndIndex.TileCoord))
        return;
      bool flag1 = designation2.IsFulfilledAt(tileAndIndex.TileCoord);
      designation2.TileChanged(tileAndIndex);
      if (designation2.IsFulfilledAt(tileAndIndex.TileCoord) == flag1)
        return;
      this.onDesignationFulfilledChanged(designation2);
    }

    private void simUpdate()
    {
    }

    /// <summary>
    /// Returns min and max area origin between two given coords. Size of returned area is clamped to <see cref="F:Mafi.Core.Terrain.Designation.SurfaceDesignationsManager.MAX_DESIGNATION_AREA_SIZE" />.
    /// </summary>
    public static void GetCanonicalDesignationRange(
      Tile2i fromCoord,
      Tile2i toCoord,
      out Tile2i minCoord,
      out Tile2i maxCoord)
    {
      minCoord = SurfaceDesignation.GetOrigin(fromCoord.Min(toCoord));
      maxCoord = SurfaceDesignation.GetOrigin(fromCoord.Max(toCoord));
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
    public Option<SurfaceDesignation> GetDesignationAt(Tile2i coord)
    {
      Tile2i origin = SurfaceDesignation.GetOrigin(coord);
      SurfaceDesignation designationAt;
      if (this.m_placingDesignations.TryGetValue(origin, out designationAt) && designationAt.IsAssignedAt(coord))
        return (Option<SurfaceDesignation>) designationAt;
      SurfaceDesignation surfaceDesignation;
      return this.m_clearingDesignations.TryGetValue(origin, out surfaceDesignation) && surfaceDesignation.IsAssignedAt(coord) ? (Option<SurfaceDesignation>) surfaceDesignation : Option<SurfaceDesignation>.None;
    }

    public bool CanPlaceSurfaceTile(Tile2i coord)
    {
      Tile2iIndex tileIndex = this.TerrainManager.GetTileIndex(coord);
      Fix32 tolerance = SurfaceDesignation.SURFACE_HEIGHT_TOLERANCE.Value;
      HeightTilesF height = this.TerrainManager.GetHeight(tileIndex);
      Fix32 fix32_1 = height.Value;
      Fix32 fractionalPart = (fix32_1 * 4).FractionalPart;
      if (!fractionalPart.IsNear((Fix32) 0, tolerance * 4) && !fractionalPart.IsNear((Fix32) 1, tolerance * 4))
        return false;
      Fix32 fix32_2 = this.TerrainManager.GetHeight(tileIndex.PlusXNeighborUnchecked).Value - fix32_1;
      Fix32 fix32_3 = this.TerrainManager.GetHeight(tileIndex.PlusYNeighborUnchecked(this.TerrainManager.TerrainWidth)).Value - fix32_1;
      Fix32 other = this.TerrainManager.GetHeight(tileIndex.PlusXPlusYNeighborUnchecked(this.TerrainManager.TerrainWidth)).Value - fix32_1;
      if (fix32_2.IsNear(Fix32.Zero, tolerance))
      {
        if (fix32_3.IsNear(other, tolerance))
        {
          if (fix32_3.IsNear(Fix32.Zero, tolerance))
            return height.Value.IsNear((Fix32) height.TilesHeightRounded.Value, tolerance);
          if (fix32_3.Abs().IsNear(Fix32.Quarter, tolerance))
            return true;
        }
        return false;
      }
      return fix32_3.IsNear(Fix32.Zero, tolerance) && fix32_2.IsNear(other, tolerance) && fix32_2.Abs().IsNear(Fix32.Quarter, tolerance);
    }

    public bool AddOrReplaceDesignation(SurfaceDesignationProto proto, SurfaceDesignationData data)
    {
      if (!this.IsDesignationAllowed(proto, data, out LocStrFormatted _))
        return false;
      if (data.UnassignedTilesBitmap != (ushort) 0)
      {
        SurfaceDesignation designation;
        bool flag = !proto.IsPlacing ? this.m_clearingDesignations.TryGetValue((Tile2i) data.OriginTile, out designation) : this.m_placingDesignations.TryGetValue((Tile2i) data.OriginTile, out designation);
        if (flag)
        {
          designation.AssignTiles((uint) ~data.UnassignedTilesBitmap, data.SurfaceProtoSlimId);
          this.onDesignationUpdated(designation);
        }
        SurfaceDesignation surfaceDesignation;
        if (!proto.IsPlacing ? this.m_placingDesignations.TryGetValue((Tile2i) data.OriginTile, out surfaceDesignation) : this.m_clearingDesignations.TryGetValue((Tile2i) data.OriginTile, out surfaceDesignation))
        {
          surfaceDesignation.UnassignTiles((uint) ~data.UnassignedTilesBitmap);
          if (surfaceDesignation.IsNotAssigned)
          {
            if (surfaceDesignation.IsPlacing)
              this.m_placingDesignations.Remove(surfaceDesignation.OriginTileCoord);
            else
              this.m_clearingDesignations.Remove(surfaceDesignation.OriginTileCoord);
            this.removeAndDestroyDesignation(surfaceDesignation);
          }
          else
            this.onDesignationUpdated(surfaceDesignation);
          if (flag)
            Assert.That<uint>((uint) (~(int) surfaceDesignation.UnassignedTilesBitmap & ~(int) designation.UnassignedTilesBitmap)).IsZero();
        }
        if (flag)
          return true;
      }
      SurfaceDesignation designation1 = new SurfaceDesignation((ISurfaceDesignationsManagerInternal) this, proto, data);
      if (data.UnassignedTilesBitmap == (ushort) 0)
        this.RemoveDesignation(designation1.OriginTileCoord);
      this.m_terrainDesignationsManager.Value.RemoveDesignation(designation1.OriginTileCoord);
      if (designation1.IsFulfilled)
        return true;
      if (proto.IsPlacing)
        this.m_placingDesignations.Add(designation1.OriginTileCoord, designation1);
      else
        this.m_clearingDesignations.Add(designation1.OriginTileCoord, designation1);
      this.onDesignationAdded(designation1);
      return true;
    }

    private void removeAndDestroyDesignation(SurfaceDesignation d)
    {
      this.m_areCachesValid = false;
      this.m_designationRemoved.Invoke(d);
      d.SetDestroyed();
    }

    private bool removeClearingDesignation(Tile2i originCoord)
    {
      SurfaceDesignation d;
      if (!this.m_clearingDesignations.TryRemove(originCoord, out d))
        return false;
      this.removeAndDestroyDesignation(d);
      return true;
    }

    private bool removePlacingDesignation(Tile2i originCoord)
    {
      SurfaceDesignation d;
      if (!this.m_placingDesignations.TryRemove(originCoord, out d))
        return false;
      this.removeAndDestroyDesignation(d);
      return true;
    }

    public bool RemoveDesignation(Tile2i originCoord)
    {
      Assert.That<Tile2i>(originCoord).IsEqualTo<Tile2i>(SurfaceDesignation.GetOrigin(originCoord));
      bool flag = this.removeClearingDesignation(originCoord);
      return this.removePlacingDesignation(originCoord) | flag;
    }

    public bool IsDesignationAllowed(
      SurfaceDesignationProto proto,
      SurfaceDesignationData designationData,
      out LocStrFormatted error)
    {
      Tile2i originTile = (Tile2i) designationData.OriginTile;
      if (originTile != SurfaceDesignation.GetOrigin(originTile))
      {
        error = (LocStrFormatted) TrCore.DesignationError__Invalid;
        return false;
      }
      if (originTile.X < this.TerrainManager.MinOnLimits.X || originTile.Y < this.TerrainManager.MinOnLimits.Y || originTile.X + 4 > this.TerrainManager.MaxOnLimitsIncl.X || originTile.Y + 4 > this.TerrainManager.MaxOnLimitsIncl.Y)
      {
        error = (LocStrFormatted) TrCore.DesignationError__Invalid;
        return false;
      }
      if (proto.IsPlacing)
      {
        TerrainTileSurfaceProto tileSurfaceProto = this.m_terrainDesignationsManager.Value.TerrainManager.ResolveSlimSurface(designationData.SurfaceProtoSlimId);
        if (!tileSurfaceProto.CanBePlacedByPlayer)
        {
          error = LocStrFormatted.Empty;
          Log.Error("This surface can't be placed by the player!");
          return false;
        }
        if (tileSurfaceProto.CostPerTile.IsEmpty || tileSurfaceProto.CostPerTile.Product.IsPhantom)
        {
          error = LocStrFormatted.Empty;
          Log.Error("Cannot place surface with no cost!");
          return false;
        }
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public bool TryGetEntityOccupyingAt(
      Tile2iAndIndex tileAndIndex,
      out OccupiedTileRelative occupiedTile,
      out IStaticEntity entity,
      out IStaticEntityProto entityProto)
    {
      entityProto = (IStaticEntityProto) null;
      ImmutableArray<OccupiedTileRelative> immutableArray;
      if (this.m_occupancyManager.TryGetOccupyingEntityAt<IStaticEntity>(tileAndIndex.TileCoord.ExtendHeight(this.TerrainManager.GetHeight(tileAndIndex.Index).TilesHeightFloored), out entity))
      {
        entityProto = (IStaticEntityProto) entity.Prototype;
        Tile3i centerTile = entity.CenterTile;
        Tile2i tileCoord = tileAndIndex.TileCoord;
        immutableArray = entity.OccupiedTiles;
        foreach (OccupiedTileRelative occupiedTileRelative in immutableArray)
        {
          if (!(centerTile.Xy + occupiedTileRelative.RelCoord != tileCoord))
          {
            occupiedTile = occupiedTileRelative;
            return true;
          }
        }
      }
      TreeData treeData;
      if (this.m_treesManager.TryGetTree(new TreeId(tileAndIndex.TileCoordSlim), out treeData))
      {
        ref OccupiedTileRelative local = ref occupiedTile;
        immutableArray = treeData.Proto.Layout.GetOccupiedTilesRelative(TileTransform.Identity);
        OccupiedTileRelative occupiedTileRelative = immutableArray.FirstOrDefault();
        local = occupiedTileRelative;
        entityProto = (IStaticEntityProto) treeData.Proto;
        return true;
      }
      if (this.m_treesManager.TryGetManualTree(new TreeId(tileAndIndex.TileCoordSlim), out treeData))
      {
        ref OccupiedTileRelative local = ref occupiedTile;
        immutableArray = treeData.Proto.Layout.GetOccupiedTilesRelative(TileTransform.Identity);
        OccupiedTileRelative occupiedTileRelative = immutableArray.FirstOrDefault();
        local = occupiedTileRelative;
        entityProto = (IStaticEntityProto) treeData.Proto;
        return true;
      }
      occupiedTile = new OccupiedTileRelative();
      return false;
    }

    private int removeAssignedJob(SurfaceDesignation designation, IVehicleJobWithOwner job)
    {
      Lyst<IVehicleJobWithOwner> lyst;
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

    IIndexable<IVehicleJobWithOwner> ISurfaceDesignationsManagerInternal.GetAssignedJobsFor(
      SurfaceDesignation designation)
    {
      Lyst<IVehicleJobWithOwner> lyst;
      return this.m_designationsWithJobs.TryGetValue(designation, out lyst) ? (IIndexable<IVehicleJobWithOwner>) lyst : Indexable<IVehicleJobWithOwner>.Empty;
    }

    void IAction<AddSurfaceDesignationsCmd>.Invoke(AddSurfaceDesignationsCmd cmd)
    {
      SurfaceDesignationProto proto;
      if (!this.m_protosDb.TryGetProto<SurfaceDesignationProto>(cmd.ProtoId, out proto))
      {
        cmd.SetResultError(ImmutableArray<Tile2i>.Empty, string.Format("Failed to find proto '{0}'.", (object) cmd.ProtoId));
      }
      else
      {
        int length = cmd.Data.Length;
        ImmutableArrayBuilder<Tile2i> immutableArrayBuilder = new ImmutableArrayBuilder<Tile2i>(length);
        for (int index = 0; index < length; ++index)
        {
          SurfaceDesignationData data = cmd.Data[index];
          this.AddOrReplaceDesignation(proto, data);
          immutableArrayBuilder[index] = (Tile2i) data.OriginTile;
        }
        cmd.SetResultSuccess(immutableArrayBuilder.GetImmutableArrayAndClear());
      }
    }

    void IAction<RemoveSurfaceDesignationsCmd>.Invoke(RemoveSurfaceDesignationsCmd cmd)
    {
      Lyst<Tile2i> lyst = new Lyst<Tile2i>(cmd.Origins.Length);
      foreach (Tile2i origin in cmd.Origins)
      {
        if (cmd.Area.FullyContains(new RectangleTerrainArea2i(origin, SurfaceDesignation.Size)))
        {
          this.RemoveDesignation(origin);
          lyst.Add(origin);
        }
        else
        {
          SurfaceDesignation designation1;
          if (this.m_clearingDesignations.TryGetValue(origin, out designation1))
          {
            designation1.UnassignArea(cmd.Area);
            this.onDesignationUpdated(designation1);
            lyst.Add(origin);
          }
          SurfaceDesignation designation2;
          if (this.m_placingDesignations.TryGetValue(origin, out designation2))
          {
            designation2.UnassignArea(cmd.Area);
            this.onDesignationUpdated(designation2);
            lyst.Add(origin);
          }
        }
      }
      cmd.SetResultSuccess(lyst.ToImmutableArray());
    }

    public string ValidateAndFixAllDesignationsAndReportErrors()
    {
      Lyst<KeyValuePair<Tile2i, bool>> lyst = this.ValidateAndFixAllDesignations();
      if (lyst.IsEmpty)
        return "";
      string message = "Terrain designations validation failed:\n" + ((IEnumerable<string>) lyst.Select<string>((Func<KeyValuePair<Tile2i, bool>, string>) (x => string.Format("{0} bitmap value: {1}fulfilled. ", (object) x.Key, x.Value ? (object) "" : (object) "not ") + "Function value " + (x.Value ? "not " : "") + "fulfilled."))).JoinStrings("\n");
      Log.Error(message);
      return message;
    }

    public Lyst<KeyValuePair<Tile2i, bool>> ValidateAndFixAllDesignations()
    {
      Lyst<KeyValuePair<Tile2i, bool>> errorLocations = new Lyst<KeyValuePair<Tile2i, bool>>();
      foreach (SurfaceDesignation surfaceDesignation in this.m_clearingDesignations.Values)
        surfaceDesignation.ValidateAndFixFulfilledBitmap(errorLocations);
      foreach (SurfaceDesignation surfaceDesignation in this.m_placingDesignations.Values)
        surfaceDesignation.ValidateAndFixFulfilledBitmap(errorLocations);
      return errorLocations;
    }

    public bool TryFindClosestReadyToPlace(
      ProductProto product,
      Tile2i position,
      Truck servicingVehicle,
      Quantity maxQuantity,
      Lyst<SurfaceDesignation> resultDesignations,
      out Quantity toExchange)
    {
      return this.tryFindClosestReady(product, position, servicingVehicle, maxQuantity, false, resultDesignations, out toExchange);
    }

    public bool TryFindClosestReadyToClear(
      ProductProto product,
      Tile2i position,
      Truck servicingVehicle,
      Quantity maxQuantity,
      Lyst<SurfaceDesignation> resultDesignations,
      out Quantity toExchange)
    {
      return this.tryFindClosestReady(product, position, servicingVehicle, maxQuantity, true, resultDesignations, out toExchange);
    }

    private bool tryFindClosestReady(
      ProductProto product,
      Tile2i position,
      Truck servicingVehicle,
      Quantity maxQuantity,
      bool isClearing,
      Lyst<SurfaceDesignation> resultDesignations,
      out Quantity toExchange)
    {
      Assert.That<Lyst<SurfaceDesignation>>(resultDesignations).IsEmpty<SurfaceDesignation>();
      DesignationsPerProductCache data;
      if (!this.TryGetDataFor(product, out data))
      {
        toExchange = Quantity.Zero;
        return false;
      }
      Quantity quantity1;
      if (isClearing)
      {
        quantity1 = data.LeftToClear;
        if (quantity1.IsNotPositive)
          goto label_6;
      }
      if (!isClearing)
      {
        quantity1 = data.LeftToPlace;
        if (quantity1.IsNotPositive)
          goto label_6;
      }
      Lyst<SurfaceDesignation> cachedDesignations = isClearing ? data.Clearing : data.Placement;
      IReadOnlySet<IDesignation> unreachableDesignationsFor = this.m_unreachablesManager.Value.GetUnreachableDesignationsFor((IPathFindingVehicle) servicingVehicle);
      if (unreachableDesignationsFor.IsNotEmpty<IDesignation>())
      {
        this.m_designationsCache.Clear();
        foreach (SurfaceDesignation surfaceDesignation in cachedDesignations)
        {
          if (!unreachableDesignationsFor.IsNotEmpty<IDesignation>() || !unreachableDesignationsFor.Contains((IDesignation) surfaceDesignation))
            this.m_designationsCache.Add(surfaceDesignation);
        }
        cachedDesignations = this.m_designationsCache;
      }
      SurfaceDesignation bestDesignation;
      if (!this.tryFindBestDesignation(position, (IEnumerable<SurfaceDesignation>) cachedDesignations, maxQuantity, product, isClearing, out toExchange, out bestDesignation))
      {
        toExchange = Quantity.Zero;
        return false;
      }
      resultDesignations.Add(bestDesignation);
      this.m_designationsTmp.Clear();
      long squared = servicingVehicle.Prototype.MaxSurfaceProcessingDistance.Squared;
      foreach (SurfaceDesignation surfaceDesignation in cachedDesignations)
      {
        if (surfaceDesignation != bestDesignation && surfaceDesignation.NumberOfJobsAssigned <= 0)
        {
          long key = surfaceDesignation.OriginTileCoord.DistanceSqrTo(bestDesignation.OriginTileCoord);
          if (key <= squared)
            this.m_designationsTmp.Add(Make.Kvp<long, SurfaceDesignation>(key, surfaceDesignation));
        }
      }
      this.m_designationsTmp.Sort((Comparison<KeyValuePair<long, SurfaceDesignation>>) ((x, y) => x.Key.CompareTo(y.Key)));
      foreach (KeyValuePair<long, SurfaceDesignation> keyValuePair in this.m_designationsTmp)
      {
        SurfaceDesignation surfaceDesignation = keyValuePair.Value;
        Quantity limit = maxQuantity - toExchange;
        if (!limit.IsNotPositive)
        {
          Quantity quantity2 = isClearing ? surfaceDesignation.GetQuantityToClear(product, limit) : surfaceDesignation.GetQuantityToPlace(product, limit);
          if (!quantity2.IsNotPositive)
          {
            toExchange += quantity2;
            resultDesignations.Add(surfaceDesignation);
          }
        }
        else
          break;
      }
      toExchange = toExchange.Min(maxQuantity);
      return true;
label_6:
      toExchange = Quantity.Zero;
      return false;
    }

    private bool tryFindBestDesignation(
      Tile2i position,
      IEnumerable<SurfaceDesignation> cachedDesignations,
      Quantity maxQuantity,
      ProductProto product,
      bool isClearing,
      out Quantity toExchange,
      out SurfaceDesignation bestDesignation)
    {
      position -= new RelTile2i(2, 2);
      Fix32 fix32_1 = Fix32.MaxValue;
      toExchange = Quantity.Zero;
      bestDesignation = (SurfaceDesignation) null;
      foreach (SurfaceDesignation cachedDesignation in cachedDesignations)
      {
        if (cachedDesignation.NumberOfJobsAssigned <= 0)
        {
          if (cachedDesignation.IsDestroyed || cachedDesignation.IsFulfilled)
          {
            Log.Error("Destroyed / fulfilled designation present?");
          }
          else
          {
            RelTile2i relTile2i = position - cachedDesignation.OriginTileCoord;
            relTile2i = relTile2i.AbsValue;
            Fix32 fix32_2 = (Fix32) (relTile2i.Sum / 2) + (Fix32) ((16 - cachedDesignation.CountFulfilledTiles()) / 2) + (int) cachedDesignation.UnreachableVehiclesCount * 50.ToFix32();
            if (!(fix32_2 >= fix32_1))
            {
              toExchange = isClearing ? cachedDesignation.GetQuantityToClear(product, maxQuantity) : cachedDesignation.GetQuantityToPlace(product, maxQuantity);
              if (!toExchange.IsNotPositive)
              {
                fix32_1 = fix32_2;
                bestDesignation = cachedDesignation;
              }
            }
          }
        }
      }
      return bestDesignation != null;
    }

    public bool TryGetDataFor(ProductProto product, out DesignationsPerProductCache data)
    {
      if (!this.tryGetCacheFor(product, out data))
        return false;
      if (!this.m_areCachesValid)
        this.updateCaches();
      return true;
    }

    /// <summary>
    /// Despite designation can require multiple products and the truck can deliver just
    /// single type, we still reserve all the products. Because we don't allow designations
    /// to be reserved by multiple trucks and if we would leave that quantity hanging it
    /// would trigger "fake" designations searches.
    /// </summary>
    public bool Reserve(IVehicleJobWithOwner job, SurfaceDesignation designation)
    {
      if (designation.IsDestroyed)
      {
        Log.Error("Adding destroyed designation?");
        return false;
      }
      if (designation.IsFulfilled)
      {
        Log.Error("Adding fulfilled designation?");
        return false;
      }
      Lyst<IVehicleJobWithOwner> instance;
      if (this.m_designationsWithJobs.TryGetValue(designation, out instance))
      {
        if (instance.IsNotEmpty && instance.Contains(job))
        {
          Log.Error("Same job already assigned!");
          return false;
        }
        if (instance.IsNotEmpty && instance.First.Vehicle != job.Vehicle)
        {
          Log.Error("Cannot assign same designation to multiple vehicles!");
          return false;
        }
      }
      else
      {
        instance = this.m_jobListsPool.GetInstance();
        this.m_designationsWithJobs.Add(designation, instance);
      }
      instance.Add(job);
      designation.NumberOfJobsAssigned = instance.Count;
      if (!this.m_areCachesValid)
        return true;
      foreach (TerrainTile terrainTile in designation.AllNonFulfilled)
      {
        if (designation.IsPlacing)
        {
          TileSurfaceData tileSurfaceData;
          if (this.TerrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData) && !tileSurfaceData.IsAutoPlaced)
            addToCache(tileSurfaceData.SurfaceSlimId, true);
          else
            addToCache(designation.GetSurfaceAt(terrainTile.TileCoord), false);
        }
        else
        {
          TileSurfaceData tileSurfaceData;
          if (this.TerrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData))
            addToCache(tileSurfaceData.SurfaceSlimId, true);
        }
      }
      return true;

      void addToCache(TileSurfaceSlimId newId, bool isClearing)
      {
        ProductQuantity costPerTile = this.TerrainManager.ResolveSlimSurface(newId).CostPerTile;
        DesignationsPerProductCache result;
        if (!this.tryGetCacheFor(costPerTile.Product, out result))
          return;
        if (isClearing)
          result.ReservedToClear += costPerTile.Quantity;
        else
          result.ReservedToPlace += costPerTile.Quantity;
      }
    }

    public void RemoveReservation(IVehicleJobWithOwner job, SurfaceDesignation designation)
    {
      designation.NumberOfJobsAssigned = this.removeAssignedJob(designation, job);
      if (!designation.IsNotFulfilled)
        return;
      this.m_areCachesValid = false;
    }

    private void updateCaches()
    {
      DesignationsPerProductCache cache = (DesignationsPerProductCache) null;
      TileSurfaceSlimId? lastSeenId = new TileSurfaceSlimId?();
      ProductQuantity lastCost = ProductQuantity.None;
      foreach (DesignationsPerProductCache designationsPerProductCache in this.m_designationsPerProductCache)
        designationsPerProductCache.Reset();
      foreach (SurfaceDesignation clearingDesignation in (IEnumerable<SurfaceDesignation>) this.ClearingDesignations)
      {
        if (isNotValid(clearingDesignation))
          this.m_invalidDesignationsCache.Add(clearingDesignation);
        else if (clearingDesignation.NumberOfJobsAssigned <= 0)
        {
          foreach (TerrainTile terrainTile in clearingDesignation.AllNonFulfilled)
          {
            TileSurfaceData tileSurfaceData;
            if (this.TerrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData))
              addToCache(tileSurfaceData.SurfaceSlimId, clearingDesignation, true);
          }
        }
      }
      foreach (SurfaceDesignation surfaceDesignation in this.m_invalidDesignationsCache)
        this.removeClearingDesignation(surfaceDesignation.OriginTileCoord);
      this.m_invalidDesignationsCache.Clear();
      foreach (SurfaceDesignation placingDesignation in (IEnumerable<SurfaceDesignation>) this.PlacingDesignations)
      {
        if (isNotValid(placingDesignation))
          this.m_invalidDesignationsCache.Add(placingDesignation);
        else if (placingDesignation.NumberOfJobsAssigned <= 0)
        {
          foreach (TerrainTile terrainTile in placingDesignation.AllNonFulfilled)
          {
            TileSurfaceData tileSurfaceData;
            if (this.TerrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData) && !tileSurfaceData.IsAutoPlaced)
              addToCache(tileSurfaceData.SurfaceSlimId, placingDesignation, true);
            else
              addToCache(placingDesignation.GetSurfaceAt(terrainTile.TileCoord), placingDesignation, false);
          }
        }
      }
      foreach (SurfaceDesignation surfaceDesignation in this.m_invalidDesignationsCache)
        this.removePlacingDesignation(surfaceDesignation.OriginTileCoord);
      this.m_invalidDesignationsCache.Clear();
      this.m_areCachesValid = true;

      static bool isNotValid(SurfaceDesignation d)
      {
        if (!d.IsDestroyed && !d.IsFulfilled)
          return false;
        Log.Error(string.Format("Removed stale {0}: ", (object) d.Prototype.Id) + " " + (d.IsFulfilled ? "fulfilled" : "") + " " + (d.IsDestroyed ? "destroyed" : "") + " " + (d.IsNotAssigned ? "notAssigned" : ""));
        return true;
      }

      void addToCache(TileSurfaceSlimId newId, SurfaceDesignation des, bool isClearing)
      {
        TileSurfaceSlimId tileSurfaceSlimId = newId;
        TileSurfaceSlimId? lastSeenId = lastSeenId;
        if ((lastSeenId.HasValue ? (tileSurfaceSlimId != lastSeenId.GetValueOrDefault() ? 1 : 0) : 1) != 0)
        {
          ProductQuantity costPerTile = this.TerrainManager.ResolveSlimSurface(newId).CostPerTile;
          DesignationsPerProductCache result;
          if (!this.tryGetCacheFor(costPerTile.Product, out result))
            return;
          cache = result;
          lastCost = costPerTile;
          lastSeenId = new TileSurfaceSlimId?(newId);
        }
        if (isClearing)
        {
          cache.TotalToClear += lastCost.Quantity;
          if (!cache.Clearing.IsEmpty && cache.Clearing.Last == des)
            return;
          cache.Clearing.Add(des);
        }
        else
        {
          cache.TotalToPlace += lastCost.Quantity;
          if (!cache.Placement.IsEmpty && cache.Placement.Last == des)
            return;
          cache.Placement.Add(des);
        }
      }
    }

    private bool tryGetCacheFor(ProductProto product, out DesignationsPerProductCache result)
    {
      foreach (DesignationsPerProductCache designationsPerProductCache in this.m_designationsPerProductCache)
      {
        if ((Proto) designationsPerProductCache.Product == (Proto) product)
        {
          result = designationsPerProductCache;
          return true;
        }
      }
      result = (DesignationsPerProductCache) null;
      return false;
    }

    public bool TryAddSurface(Tile2iAndIndex tileAndIndex, SurfaceDesignation designation)
    {
      TerrainManager terrainManager = this.TerrainManager;
      TileSurfaceSlimId surfaceAt = designation.GetSurfaceAt(tileAndIndex.TileCoord);
      Fix32 toleranceWhenPlacing = SurfaceDesignationsManager.SURFACE_HEIGHT_TOLERANCE_WHEN_PLACING;
      HeightTilesF height1 = terrainManager.GetHeight(tileAndIndex.Index);
      Tile2iAndIndex tileAndIndex1 = tileAndIndex;
      Tile2iAndIndex xneighborUnchecked = tileAndIndex.PlusXNeighborUnchecked;
      Tile2iAndIndex tileAndIndex2 = tileAndIndex.PlusYNeighborUnchecked(terrainManager.TerrainWidth);
      Tile2iAndIndex tileAndIndex3 = tileAndIndex.PlusXPlusYNeighborUnchecked(terrainManager.TerrainWidth);
      Fix32 fix32_1 = height1.Value;
      Fix32 fix32_2 = terrainManager.GetHeight(xneighborUnchecked.Index).Value - fix32_1;
      Fix32 fix32_3 = terrainManager.GetHeight(tileAndIndex2.Index).Value - fix32_1;
      Fix32 other = terrainManager.GetHeight(tileAndIndex3.Index).Value - fix32_1;
      HeightTilesF height2;
      Direction90 rampDirection;
      if (fix32_2.IsNear(Fix32.Zero, toleranceWhenPlacing) && fix32_3.IsNear(other, toleranceWhenPlacing))
      {
        if (fix32_3.IsNear(Fix32.Zero, toleranceWhenPlacing))
        {
          if (!((Fix32) height1.TilesHeightRounded.Value - height1.Value).IsNear((Fix32) 0, toleranceWhenPlacing))
          {
            Log.Error(string.Format("Unexpected concrete placement A {0} {1} {2} {3}.", (object) fix32_1, (object) fix32_2, (object) fix32_3, (object) other));
            return false;
          }
          HeightTilesI tilesHeightRounded = terrainManager.GetHeight(tileAndIndex.Index).TilesHeightRounded;
          terrainManager.SetCustomSurface(tileAndIndex, TileSurfaceData.CreateFlat(tilesHeightRounded, surfaceAt, new Direction90()));
          HeightTilesF heightTilesF = tilesHeightRounded.HeightTilesF;
          if (terrainManager.GetHeight(tileAndIndex1.Index) > heightTilesF)
            terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex1, heightTilesF);
          if (terrainManager.GetHeight(xneighborUnchecked.Index) > heightTilesF)
            terrainManager.SetHeightPreserveRelativeLayersNoPhysics(xneighborUnchecked, heightTilesF);
          if (terrainManager.GetHeight(tileAndIndex2.Index) > heightTilesF)
            terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex2, heightTilesF);
          if (terrainManager.GetHeight(tileAndIndex3.Index) > heightTilesF)
            terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex3, heightTilesF);
          return true;
        }
        height2 = terrainManager.GetHeight(tileAndIndex.TileCoord.CenterTile2f);
        if (fix32_3.IsNear(Fix32.Quarter, toleranceWhenPlacing))
          rampDirection = Direction90.PlusY;
        else if (fix32_3.IsNear(-Fix32.Quarter, toleranceWhenPlacing))
        {
          rampDirection = Direction90.MinusY;
        }
        else
        {
          Log.Error(string.Format("Unexpected concrete placement B {0} {1} {2} {3}.", (object) fix32_1, (object) fix32_2, (object) fix32_3, (object) other));
          return false;
        }
      }
      else if (fix32_3.IsNear(Fix32.Zero, toleranceWhenPlacing) && fix32_2.IsNear(other, toleranceWhenPlacing))
      {
        height2 = terrainManager.GetHeight(tileAndIndex.TileCoord.CenterTile2f);
        if (fix32_2.IsNear(Fix32.Quarter, toleranceWhenPlacing))
          rampDirection = Direction90.PlusX;
        else if (fix32_2.IsNear(-Fix32.Quarter, toleranceWhenPlacing))
        {
          rampDirection = Direction90.MinusX;
        }
        else
        {
          Log.Error(string.Format("Unexpected concrete placement C {0} {1} {2} {3}.", (object) fix32_1, (object) fix32_2, (object) fix32_3, (object) other));
          return false;
        }
      }
      else
      {
        Log.Error(string.Format("Unexpected concrete placement D {0} {1} {2} {3}.", (object) fix32_1, (object) fix32_2, (object) fix32_3, (object) other));
        return false;
      }
      HeightTilesF height3 = height2 + ThicknessTilesF.One / 8;
      HeightTilesF height4 = height2 - ThicknessTilesF.One / 8;
      switch (rampDirection.DirectionIndex)
      {
        case 0:
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex1, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(xneighborUnchecked, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex2, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex3, height3);
          break;
        case 1:
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex1, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(xneighborUnchecked, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex2, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex3, height3);
          break;
        case 2:
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex1, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(xneighborUnchecked, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex2, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex3, height4);
          break;
        case 3:
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex1, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(xneighborUnchecked, height3);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex2, height4);
          terrainManager.SetHeightPreserveRelativeLayersNoPhysics(tileAndIndex3, height4);
          break;
      }
      terrainManager.SetCustomSurface(tileAndIndex, TileSurfaceData.CreateRamp(height2, surfaceAt, new Direction90(), rampDirection));
      return true;
    }

    public void ClearCustomSurface(Tile2iAndIndex tileAndIndex, out bool placedAutoSurface)
    {
      placedAutoSurface = false;
      this.TerrainManager.ClearCustomSurface(tileAndIndex);
      OccupiedTileRelative occupiedTile;
      IStaticEntity entity;
      if (!this.m_occupancyManager.IsOccupied(tileAndIndex.Index) || !this.TryGetEntityOccupyingAt(tileAndIndex, out occupiedTile, out entity, out IStaticEntityProto _) || occupiedTile.TileSurface.IsPhantom)
        return;
      this.TerrainManager.SetCustomSurface(tileAndIndex, TileSurfaceData.CreateFlat(entity.CenterTile.Height + occupiedTile.TileSurfaceRelHeight, occupiedTile.TileSurface, new Direction90(), true));
      placedAutoSurface = true;
    }

    static SurfaceDesignationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SurfaceDesignationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SurfaceDesignationsManager) obj).SerializeData(writer));
      SurfaceDesignationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SurfaceDesignationsManager) obj).DeserializeData(reader));
      SurfaceDesignationsManager.SURFACE_HEIGHT_TOLERANCE_WHEN_PLACING = TerrainDesignation.SURFACE_HEIGHT_TOLERANCE.Value * 1.25.ToFix32();
    }
  }
}
