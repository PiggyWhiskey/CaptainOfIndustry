// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.UnreachableTerrainDesignationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  /// <summary>Manages unreachable designations for vehicles.</summary>
  /// <remarks>
  /// Anyone (typically goals) can report destinations are unreachable. These are cached and can be filtered out
  /// when searching for new jobs.
  /// 
  /// Caches are cleared only after some time and only if a vehicle becomes idle. Thus it is important to call
  /// ReportVehicleIdle to update this manager. Also caches can be cleared in several phases.
  /// 
  /// </remarks>
  [MemberRemovedInSaveVersion("m_latestUnreachableReport", 140, typeof (Dict<Vehicle, SimStep>), 0, false)]
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [OnlyForSaveCompatibility(null)]
  public class UnreachableTerrainDesignationsManager : IUnreachablesManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [DoNotSave(140, null)]
    private readonly Dict<Vehicle, Set<TerrainDesignation>> m_unreachableDesignationsOld;
    [DoNotSave(140, null)]
    private readonly Dict<Vehicle, Set<TreeId>> m_unreachableTreesOld;
    [DoNotSave(140, null)]
    [NewInSaveVersion(119, null, "new()", null, null)]
    private readonly Dict<Vehicle, Set<Chunk2i>> m_unreachableTreeChunksOld;
    [DoNotSave(140, null)]
    [NewInSaveVersion(102, null, "new()", null, null)]
    private readonly Dict<Vehicle, Set<Tile2iSlim>> m_unreachableTilesOld;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private Dict<IPathFindingVehicle, UnreachableTerrainDesignationsManager.VehicleData> m_vehiclesData;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private Dict<IEntity, UnreachableTerrainDesignationsManager.UnreachableEntityData> m_entitiesData;
    private ITreesManager m_treeManager;
    private readonly ISimLoopEvents m_simLoopEvents;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IDesignation> m_designationsToPrune;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private Lyst<Vehicle> m_vehiclesToClear;
    [NewInSaveVersion(140, null, null, null, null)]
    private SimStep m_lastClearStep;
    private bool m_pruneRemovedDesignations;

    public static void Serialize(UnreachableTerrainDesignationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnreachableTerrainDesignationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnreachableTerrainDesignationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<IEntity, UnreachableTerrainDesignationsManager.UnreachableEntityData>.Serialize(this.m_entitiesData, writer);
      SimStep.Serialize(this.m_lastClearStep, writer);
      writer.WriteBool(this.m_pruneRemovedDesignations);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteGeneric<ITreesManager>(this.m_treeManager);
      Dict<IPathFindingVehicle, UnreachableTerrainDesignationsManager.VehicleData>.Serialize(this.m_vehiclesData, writer);
      Lyst<Vehicle>.Serialize(this.m_vehiclesToClear, writer);
    }

    public static UnreachableTerrainDesignationsManager Deserialize(BlobReader reader)
    {
      UnreachableTerrainDesignationsManager designationsManager;
      if (reader.TryStartClassDeserialization<UnreachableTerrainDesignationsManager>(out designationsManager))
        reader.EnqueueDataDeserialization((object) designationsManager, UnreachableTerrainDesignationsManager.s_deserializeDataDelayedAction);
      return designationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_designationsToPrune", (object) new Set<IDesignation>());
      this.m_entitiesData = reader.LoadedSaveVersion >= 140 ? Dict<IEntity, UnreachableTerrainDesignationsManager.UnreachableEntityData>.Deserialize(reader) : new Dict<IEntity, UnreachableTerrainDesignationsManager.UnreachableEntityData>();
      this.m_lastClearStep = reader.LoadedSaveVersion >= 140 ? SimStep.Deserialize(reader) : new SimStep();
      if (reader.LoadedSaveVersion < 140)
        Dict<Vehicle, SimStep>.Deserialize(reader);
      this.m_pruneRemovedDesignations = reader.ReadBool();
      reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_treeManager = reader.ReadGenericAs<ITreesManager>();
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_unreachableDesignationsOld", (object) Dict<Vehicle, Set<TerrainDesignation>>.Deserialize(reader));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_unreachableTilesOld", reader.LoadedSaveVersion >= 102 ? (object) Dict<Vehicle, Set<Tile2iSlim>>.Deserialize(reader) : (object) new Dict<Vehicle, Set<Tile2iSlim>>());
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_unreachableTreeChunksOld", reader.LoadedSaveVersion >= 119 ? (object) Dict<Vehicle, Set<Chunk2i>>.Deserialize(reader) : (object) new Dict<Vehicle, Set<Chunk2i>>());
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<UnreachableTerrainDesignationsManager>(this, "m_unreachableTreesOld", (object) Dict<Vehicle, Set<TreeId>>.Deserialize(reader));
      this.m_vehiclesData = reader.LoadedSaveVersion >= 140 ? Dict<IPathFindingVehicle, UnreachableTerrainDesignationsManager.VehicleData>.Deserialize(reader) : new Dict<IPathFindingVehicle, UnreachableTerrainDesignationsManager.VehicleData>();
      this.m_vehiclesToClear = reader.LoadedSaveVersion >= 140 ? Lyst<Vehicle>.Deserialize(reader) : new Lyst<Vehicle>();
      reader.RegisterInitAfterLoad<UnreachableTerrainDesignationsManager>(this, "initAfterLoadHigh", InitPriority.High);
      reader.RegisterInitAfterLoad<UnreachableTerrainDesignationsManager>(this, "initAfterLoadLow", InitPriority.Low);
    }

    public UnreachableTerrainDesignationsManager(
      TerrainDesignationsManager designationsManager,
      SurfaceDesignationsManager surfaceDesignationsManager,
      ITreesManager treeManager,
      EntitiesManager entitiesManager,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_vehiclesData = new Dict<IPathFindingVehicle, UnreachableTerrainDesignationsManager.VehicleData>();
      this.m_entitiesData = new Dict<IEntity, UnreachableTerrainDesignationsManager.UnreachableEntityData>();
      this.m_designationsToPrune = new Set<IDesignation>();
      this.m_vehiclesToClear = new Lyst<Vehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager;
      this.m_simLoopEvents = simLoopEvents;
      simLoopEvents.UpdateEnd.Add<UnreachableTerrainDesignationsManager>(this, new Action(this.simUpdateEnd));
      entitiesManager.EntityRemoved.Add<UnreachableTerrainDesignationsManager>(this, new Action<IEntity>(this.entityRemoved));
      designationsManager.DesignationFulfilledChanged.Add<UnreachableTerrainDesignationsManager>(this, new Action<TerrainDesignation>(this.designationFulfilledChanged));
      designationsManager.DesignationRemoved.Add<UnreachableTerrainDesignationsManager>(this, new Action<TerrainDesignation>(this.designationRemoved));
      surfaceDesignationsManager.DesignationRemoved.Add<UnreachableTerrainDesignationsManager>(this, new Action<SurfaceDesignation>(this.surfaceDesignationRemoved));
      entitiesManager.EntityEnabledChanged.Add<UnreachableTerrainDesignationsManager>(this, new Action<IEntity, bool>(this.onEntityEnabledChanged));
      treeManager.TreeRemovedFromHarvest.Add<UnreachableTerrainDesignationsManager>(this, new Action<TreeId>(this.treeRemovedFromHarvest));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.High)]
    private void initAfterLoadHigh(int saveVersion)
    {
      if (saveVersion < 110)
        this.m_unreachableDesignationsOld.Clear();
      if (saveVersion >= 140)
        return;
      foreach (Vehicle key in this.m_unreachableDesignationsOld.Keys)
      {
        UnreachableTerrainDesignationsManager.VehicleData dataFor = this.getOrCreateDataFor((IPathFindingVehicle) key, false);
        foreach (TerrainDesignation terrainDesignation in this.m_unreachableDesignationsOld[key])
          dataFor.AddDesignation((IDesignation) terrainDesignation);
      }
      foreach (Vehicle key in this.m_unreachableTreesOld.Keys)
      {
        UnreachableTerrainDesignationsManager.VehicleData dataFor = this.getOrCreateDataFor((IPathFindingVehicle) key, false);
        foreach (TreeId tree in this.m_unreachableTreesOld[key])
          dataFor.AddTree(tree);
      }
      foreach (Vehicle key in this.m_unreachableTilesOld.Keys)
      {
        UnreachableTerrainDesignationsManager.VehicleData dataFor = this.getOrCreateDataFor((IPathFindingVehicle) key, false);
        foreach (Tile2iSlim tile in this.m_unreachableTilesOld[key])
          dataFor.AddTile((Tile2i) tile);
      }
      foreach (Vehicle key in this.m_unreachableTreeChunksOld.Keys)
      {
        UnreachableTerrainDesignationsManager.VehicleData dataFor = this.getOrCreateDataFor((IPathFindingVehicle) key, false);
        foreach (Chunk2i chunk in this.m_unreachableTreeChunksOld[key])
          dataFor.AddChunk(chunk);
      }
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Low)]
    private void initAfterLoadLow(int saveVersion, DependencyResolver resolver)
    {
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values.ToArray<UnreachableTerrainDesignationsManager.VehicleData>())
      {
        if (vehicleData.IsEmpty() || vehicleData.Vehicle.IsDestroyed)
        {
          Assert.That<bool>(vehicleData.Vehicle.IsDestroyed).IsFalse("Destroyed vehicle in unreachables?");
          this.m_vehiclesData.Remove(vehicleData.Vehicle);
        }
        else
        {
          foreach (IDesignation unreachableDesignation in (IEnumerable<IDesignation>) vehicleData.UnreachableDesignations)
            ++unreachableDesignation.UnreachableVehiclesCount;
        }
      }
      foreach (UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData in this.m_entitiesData.Values.ToArray<UnreachableTerrainDesignationsManager.UnreachableEntityData>())
      {
        int newCount = 0;
        foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
          newCount += vehicleData.IsEntityUnreachable(unreachableEntityData.Entity, true) ? 1 : 0;
        unreachableEntityData.SetUnreachableCount(newCount);
        if (unreachableEntityData.UnreachableVehiclesCount <= 0 || unreachableEntityData.Entity.IsDestroyed)
        {
          Assert.That<bool>(unreachableEntityData.Entity.IsDestroyed).IsFalse("Destroyed entity in unreachables?");
          this.m_entitiesData.Remove(unreachableEntityData.Entity);
        }
      }
      if (saveVersion >= 140)
        return;
      resolver.Resolve<SurfaceDesignationsManager>().DesignationRemoved.Add<UnreachableTerrainDesignationsManager>(this, new Action<SurfaceDesignation>(this.surfaceDesignationRemoved));
      resolver.Resolve<IEntitiesManager>().EntityEnabledChanged.Add<UnreachableTerrainDesignationsManager>(this, new Action<IEntity, bool>(this.onEntityEnabledChanged));
    }

    private void onEntityEnabledChanged(IEntity entity, bool isEnabled)
    {
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData))
        return;
      unreachableEntityData.OnEntityEnabledChanged();
    }

    public void ReportVehicleIdle(Vehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      if (!this.m_vehiclesData.TryGetValue((IPathFindingVehicle) vehicle, out vehicleData) || vehicleData.IsEmpty())
        return;
      SimStep currentStep = this.m_simLoopEvents.CurrentStep;
      Duration duration = 20.Seconds() + (vehicle.NavigationFailedStreak / 10 * 1.Seconds()).Max(40.Seconds());
      if (vehicleData.LastClearingStep.HasValue && currentStep - vehicleData.LastClearingStep.Value < duration || currentStep - vehicleData.LastUnreachableReportStep < duration)
        return;
      if (!vehicleData.FirstIdleReportSinceClear.HasValue)
      {
        vehicleData.FirstIdleReportSinceClear = new SimStep?(currentStep);
      }
      else
      {
        if (currentStep - vehicleData.FirstIdleReportSinceClear.Value < duration)
          return;
        this.m_vehiclesToClear.AddIfNotPresent(vehicle);
      }
    }

    private void simUpdateEnd()
    {
      this.ensureDestroyedArePruned();
      if (this.m_designationsToPrune.IsNotEmpty)
      {
        IDesignation[] arrayPooled = this.m_designationsToPrune.ToArrayPooled();
        this.m_designationsToPrune.Clear();
        foreach (IDesignation designation in arrayPooled)
        {
          foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
            vehicleData.RemoveDesignation(designation);
        }
        arrayPooled.ReturnToPool<IDesignation>();
      }
      if ((this.m_simLoopEvents.CurrentStep - this.m_lastClearStep).Abs < 4.Seconds())
        return;
      UnreachableTerrainDesignationsManager.VehicleData vehicleData1;
      if (this.m_vehiclesToClear.IsNotEmpty && this.m_vehiclesData.TryGetValue((IPathFindingVehicle) this.m_vehiclesToClear.PopLast(), out vehicleData1))
      {
        this.m_lastClearStep = this.m_simLoopEvents.CurrentStep;
        vehicleData1.Clear(new Action<IEntity>(this.entityRemovedFromData), true);
        vehicleData1.LastClearingStep = new SimStep?(this.m_simLoopEvents.CurrentStep);
      }
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData2 in this.m_vehiclesData.Values)
      {
        Duration abs1 = (this.m_simLoopEvents.CurrentStep - vehicleData2.LastUnreachableReportStep).Abs;
        Duration abs2 = (this.m_simLoopEvents.CurrentStep - (vehicleData2.LastClearingStep ?? SimStep.Zero)).Abs;
        Duration abs3 = (this.m_simLoopEvents.CurrentStep - vehicleData2.FirstIdleReportSinceClear.GetValueOrDefault()).Abs;
        if (abs1 > 3.Minutes() && abs3 > 90.Seconds() && abs2 > 1.Minutes() && !vehicleData2.IsEmpty())
        {
          this.m_lastClearStep = this.m_simLoopEvents.CurrentStep;
          vehicleData2.LastClearingStep = new SimStep?(this.m_simLoopEvents.CurrentStep);
          vehicleData2.Clear(new Action<IEntity>(this.entityRemovedFromData), true);
          break;
        }
      }
    }

    private void ensureDestroyedArePruned()
    {
      if (!this.m_pruneRemovedDesignations)
        return;
      this.m_pruneRemovedDesignations = false;
      this.pruneDestroyedDesignations();
    }

    private void pruneDestroyedDesignations()
    {
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
        vehicleData.PruneDesignations();
    }

    private void entityRemoved(IEntity entity)
    {
      if (entity is Vehicle key)
      {
        UnreachableTerrainDesignationsManager.VehicleData vehicleData;
        if (!this.m_vehiclesData.TryGetValue((IPathFindingVehicle) key, out vehicleData))
          return;
        vehicleData.Clear(new Action<IEntity>(this.entityRemovedFromData), false);
        this.m_vehiclesData.Remove((IPathFindingVehicle) key);
        this.m_vehiclesToClear.Remove(key);
      }
      if (!this.m_entitiesData.Remove(entity))
        return;
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
        vehicleData.RemoveEntity(entity);
    }

    private void entityRemovedFromData(IEntity entity)
    {
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData))
        Log.Error(string.Format("Entity {0} not found!", (object) entity.Prototype.Strings.Name));
      else
        unreachableEntityData.SetUnreachableCount(unreachableEntityData.UnreachableVehiclesCount - 1);
    }

    private void designationFulfilledChanged(TerrainDesignation designation)
    {
      if (!designation.IsFulfilled)
        return;
      addIfNotNone(designation.PlusXNeighbor);
      addIfNotNone(designation.PlusYNeighbor);
      addIfNotNone(designation.MinusXNeighbor);
      addIfNotNone(designation.MinusYNeighbor);

      void addIfNotNone(Option<TerrainDesignation> d)
      {
        if (!d.HasValue)
          return;
        this.m_designationsToPrune.Add((IDesignation) d.Value);
      }
    }

    private void designationRemoved(TerrainDesignation designation)
    {
      this.m_pruneRemovedDesignations = true;
    }

    private void surfaceDesignationRemoved(SurfaceDesignation designation)
    {
      this.m_pruneRemovedDesignations = true;
    }

    private void treeRemovedFromHarvest(TreeId tree)
    {
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
        vehicleData.RemoveTree(tree);
    }

    public void OnVehicleRecovered(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData1;
      if (this.m_vehiclesData.TryGetValue(vehicle, out vehicleData1))
        vehicleData1.Clear(new Action<IEntity>(this.entityRemovedFromData), false);
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue((IEntity) vehicle, out unreachableEntityData))
        return;
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData2 in this.m_vehiclesData.Values)
        vehicleData2.RemoveEntity((IEntity) vehicle);
      unreachableEntityData.SetUnreachableCount(0);
    }

    /// <summary>
    /// Note: Can contain destroyed or fulfilled ones but that should not matter.
    /// </summary>
    public IReadOnlySet<IDesignation> GetUnreachableDesignationsFor(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return !this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) ? Set<IDesignation>.Empty : vehicleData.UnreachableDesignations;
    }

    public void MarkUnreachableFor(IDesignation designation, IPathFindingVehicle vehicle)
    {
      this.getOrCreateDataFor(vehicle, true).AddDesignation(designation);
    }

    public void MarkUnreachableFor(
      IEnumerable<IDesignation> designations,
      IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData dataFor = this.getOrCreateDataFor(vehicle, true);
      foreach (IDesignation designation in designations)
        dataFor.AddDesignation(designation);
    }

    public void MarkReachableFor(
      IEnumerable<IDesignation> designations,
      IPathFindingVehicle vehicle)
    {
      foreach (IDesignation designation in designations)
      {
        if (designation.UnreachableVehiclesCount > (ushort) 0)
        {
          foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
          {
            if ((Proto) vehicleData.Vehicle.Prototype == (Proto) vehicle.Prototype)
              vehicleData.RemoveDesignation(designation);
          }
        }
      }
    }

    public IReadOnlySet<TreeId> GetUnreachableTreesFor(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return !this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) ? Set<TreeId>.Empty : vehicleData.UnreachableTrees;
    }

    public void MarkUnreachableFor(TreeId tree, IPathFindingVehicle vehicle)
    {
      this.getOrCreateDataFor(vehicle, true).AddTree(tree);
    }

    public void MarkReachableFor(TreeId tree, IPathFindingVehicle vehicle)
    {
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
        vehicleData.RemoveTree(tree);
    }

    public IReadOnlySet<IEntity> GetUnreachableEntitiesFor(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return !this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) ? Set<IEntity>.Empty : vehicleData.UnreachableEntities;
    }

    public bool HasUnreachableEntity(IPathFindingVehicle vehicle, IEntity entity)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) && vehicleData.IsEntityUnreachable(entity, false);
    }

    public void MarkUnreachableFor(IEntity entity, IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData))
      {
        unreachableEntityData = new UnreachableTerrainDesignationsManager.UnreachableEntityData(entity);
        this.m_entitiesData[entity] = unreachableEntityData;
      }
      if (!this.getOrCreateDataFor(vehicle, true).AddEntity(entity))
        return;
      unreachableEntityData.SetUnreachableCount(unreachableEntityData.UnreachableVehiclesCount + 1);
    }

    public void MarkReachableFor(IEntity entity, IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData) || unreachableEntityData.UnreachableVehiclesCount <= 0)
        return;
      int num = 0;
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
      {
        if ((Proto) vehicleData.Vehicle.Prototype == (Proto) vehicle.Prototype && vehicleData.RemoveEntity(entity))
          ++num;
      }
      unreachableEntityData.SetUnreachableCount(unreachableEntityData.UnreachableVehiclesCount - num);
    }

    public void TryClearUnreachableVehiclesFor(IEntity entity)
    {
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData))
        return;
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
        vehicleData.RemoveEntity(entity);
      unreachableEntityData.SetUnreachableCount(0);
    }

    public IReadOnlySet<Chunk2i> GetUnreachableTreeChunksFor(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return !this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) ? Set<Chunk2i>.Empty : vehicleData.UnreachableChunks;
    }

    public void MarkUnreachableTreeChunkFor(Chunk2i treeChunk, IPathFindingVehicle vehicle)
    {
      this.getOrCreateDataFor(vehicle, true).AddChunk(treeChunk);
    }

    public IReadOnlySet<Tile2iSlim> GetUnreachableTilesFor(IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      return !this.m_vehiclesData.TryGetValue(vehicle, out vehicleData) ? Set<Tile2iSlim>.Empty : vehicleData.UnreachableTiles;
    }

    public void MarkUnreachableFor(Tile2i position, IPathFindingVehicle vehicle)
    {
      this.getOrCreateDataFor(vehicle, true).AddTile(position);
    }

    public void MarkReachableFor(Tile2i position, IPathFindingVehicle vehicle)
    {
      UnreachableTerrainDesignationsManager.VehicleData vehicleData;
      if (!this.m_vehiclesData.TryGetValue(vehicle, out vehicleData))
        return;
      vehicleData.RemoveTile(position);
    }

    public void GetVehiclesThatFailedToNavigateTo(
      IEntity entity,
      Lyst<IPathFindingVehicle> result,
      bool addClearedOnesOnlyIfNotified)
    {
      result.Clear();
      UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
      if (!this.m_entitiesData.TryGetValue(entity, out unreachableEntityData) || unreachableEntityData.UnreachableVehiclesCount == 0)
        return;
      foreach (UnreachableTerrainDesignationsManager.VehicleData vehicleData in this.m_vehiclesData.Values)
      {
        bool includeCleared = !addClearedOnesOnlyIfNotified || unreachableEntityData.IsNotificationActive;
        if (vehicleData.IsEntityUnreachable(entity, includeCleared))
          result.Add(vehicleData.Vehicle);
      }
    }

    private UnreachableTerrainDesignationsManager.VehicleData getOrCreateDataFor(
      IPathFindingVehicle vehicle,
      bool reportUnreachable)
    {
      UnreachableTerrainDesignationsManager.VehicleData dataFor;
      if (!this.m_vehiclesData.TryGetValue(vehicle, out dataFor))
      {
        dataFor = new UnreachableTerrainDesignationsManager.VehicleData(vehicle);
        this.m_vehiclesData[vehicle] = dataFor;
      }
      if (reportUnreachable)
        dataFor.LastUnreachableReportStep = this.m_simLoopEvents.CurrentStep;
      return dataFor;
    }

    static UnreachableTerrainDesignationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnreachableTerrainDesignationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnreachableTerrainDesignationsManager) obj).SerializeData(writer));
      UnreachableTerrainDesignationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnreachableTerrainDesignationsManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class UnreachableEntityData
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      public readonly IEntity Entity;
      private EntityNotificator m_notificator;

      public static void Serialize(
        UnreachableTerrainDesignationsManager.UnreachableEntityData value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<UnreachableTerrainDesignationsManager.UnreachableEntityData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, UnreachableTerrainDesignationsManager.UnreachableEntityData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<IEntity>(this.Entity);
        EntityNotificator.Serialize(this.m_notificator, writer);
      }

      public static UnreachableTerrainDesignationsManager.UnreachableEntityData Deserialize(
        BlobReader reader)
      {
        UnreachableTerrainDesignationsManager.UnreachableEntityData unreachableEntityData;
        if (reader.TryStartClassDeserialization<UnreachableTerrainDesignationsManager.UnreachableEntityData>(out unreachableEntityData))
          reader.EnqueueDataDeserialization((object) unreachableEntityData, UnreachableTerrainDesignationsManager.UnreachableEntityData.s_deserializeDataDelayedAction);
        return unreachableEntityData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<UnreachableTerrainDesignationsManager.UnreachableEntityData>(this, "Entity", (object) reader.ReadGenericAs<IEntity>());
        this.m_notificator = EntityNotificator.Deserialize(reader);
      }

      public bool IsNotificationActive => this.m_notificator.IsActive;

      [DoNotSave(0, null)]
      public int UnreachableVehiclesCount { get; private set; }

      public UnreachableEntityData(IEntity entity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Entity = entity;
        this.m_notificator = this.Entity.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.EntityCannotBeReached);
      }

      public void SetUnreachableCount(int newCount)
      {
        if (newCount < 0)
        {
          Log.Error("UnreachableVehiclesCount cannot be negative!");
          newCount = 0;
        }
        this.UnreachableVehiclesCount = newCount;
        this.updateNotification();
      }

      private void updateNotification()
      {
        int num = this.Entity is IPathFindingVehicle ? 2 : 4;
        this.m_notificator.NotifyIff(this.Entity.IsEnabled && this.UnreachableVehiclesCount >= num, this.Entity);
      }

      public void OnEntityEnabledChanged() => this.updateNotification();

      static UnreachableEntityData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UnreachableTerrainDesignationsManager.UnreachableEntityData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnreachableTerrainDesignationsManager.UnreachableEntityData) obj).SerializeData(writer));
        UnreachableTerrainDesignationsManager.UnreachableEntityData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnreachableTerrainDesignationsManager.UnreachableEntityData) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class VehicleData
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      private static readonly ThreadLocal<Lyst<IDesignation>> DESIGNATIONS_CACHE;
      private static readonly ThreadLocal<Lyst<TreeId>> TREES_CACHE;
      private static readonly ThreadLocal<Lyst<Chunk2i>> CHUNKS_CACHE;
      private static readonly ThreadLocal<Lyst<Tile2iSlim>> TILES_CACHE;
      public readonly IPathFindingVehicle Vehicle;
      public SimStep? FirstIdleReportSinceClear;
      public SimStep? LastClearingStep;
      public SimStep LastUnreachableReportStep;
      private int m_clearingPhase;
      private readonly Set<IEntity> m_entities;
      /// <summary>
      /// We have cleared entities set to be able to "soft" remove them. Entities in the cleared set
      /// are not returned as unreachable when searching for jobs. But they still count toward entity's
      /// unreachable entities count. This way we prevent flickering notifications when we from time to
      /// time clear unreachable entities to test them again.
      /// </summary>
      private readonly Set<IEntity> m_entitiesCleared;
      private Option<Set<IDesignation>> m_designations;
      private Option<Set<TreeId>> m_trees;
      private Option<Set<Tile2iSlim>> m_tiles;
      private Option<Set<Chunk2i>> m_chunks;

      public static void Serialize(
        UnreachableTerrainDesignationsManager.VehicleData value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<UnreachableTerrainDesignationsManager.VehicleData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, UnreachableTerrainDesignationsManager.VehicleData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteNullableStruct<SimStep>(this.FirstIdleReportSinceClear);
        writer.WriteNullableStruct<SimStep>(this.LastClearingStep);
        SimStep.Serialize(this.LastUnreachableReportStep, writer);
        Option<Set<Chunk2i>>.Serialize(this.m_chunks, writer);
        writer.WriteInt(this.m_clearingPhase);
        Option<Set<IDesignation>>.Serialize(this.m_designations, writer);
        Set<IEntity>.Serialize(this.m_entities, writer);
        Set<IEntity>.Serialize(this.m_entitiesCleared, writer);
        Option<Set<Tile2iSlim>>.Serialize(this.m_tiles, writer);
        Option<Set<TreeId>>.Serialize(this.m_trees, writer);
        writer.WriteGeneric<IPathFindingVehicle>(this.Vehicle);
      }

      public static UnreachableTerrainDesignationsManager.VehicleData Deserialize(BlobReader reader)
      {
        UnreachableTerrainDesignationsManager.VehicleData vehicleData;
        if (reader.TryStartClassDeserialization<UnreachableTerrainDesignationsManager.VehicleData>(out vehicleData))
          reader.EnqueueDataDeserialization((object) vehicleData, UnreachableTerrainDesignationsManager.VehicleData.s_deserializeDataDelayedAction);
        return vehicleData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.FirstIdleReportSinceClear = reader.ReadNullableStruct<SimStep>();
        this.LastClearingStep = reader.ReadNullableStruct<SimStep>();
        this.LastUnreachableReportStep = SimStep.Deserialize(reader);
        this.m_chunks = Option<Set<Chunk2i>>.Deserialize(reader);
        this.m_clearingPhase = reader.ReadInt();
        this.m_designations = Option<Set<IDesignation>>.Deserialize(reader);
        reader.SetField<UnreachableTerrainDesignationsManager.VehicleData>(this, "m_entities", (object) Set<IEntity>.Deserialize(reader));
        reader.SetField<UnreachableTerrainDesignationsManager.VehicleData>(this, "m_entitiesCleared", (object) Set<IEntity>.Deserialize(reader));
        this.m_tiles = Option<Set<Tile2iSlim>>.Deserialize(reader);
        this.m_trees = Option<Set<TreeId>>.Deserialize(reader);
        reader.SetField<UnreachableTerrainDesignationsManager.VehicleData>(this, "Vehicle", (object) reader.ReadGenericAs<IPathFindingVehicle>());
      }

      public IReadOnlySet<IEntity> UnreachableEntities => (IReadOnlySet<IEntity>) this.m_entities;

      public IReadOnlySet<IDesignation> UnreachableDesignations
      {
        get
        {
          return (IReadOnlySet<IDesignation>) this.m_designations.ValueOrNull ?? Set<IDesignation>.Empty;
        }
      }

      public IReadOnlySet<TreeId> UnreachableTrees
      {
        get => (IReadOnlySet<TreeId>) this.m_trees.ValueOrNull ?? Set<TreeId>.Empty;
      }

      public IReadOnlySet<Tile2iSlim> UnreachableTiles
      {
        get => (IReadOnlySet<Tile2iSlim>) this.m_tiles.ValueOrNull ?? Set<Tile2iSlim>.Empty;
      }

      public IReadOnlySet<Chunk2i> UnreachableChunks
      {
        get => (IReadOnlySet<Chunk2i>) this.m_chunks.ValueOrNull ?? Set<Chunk2i>.Empty;
      }

      public VehicleData(IPathFindingVehicle vehicle)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_entities = new Set<IEntity>();
        this.m_entitiesCleared = new Set<IEntity>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Vehicle = vehicle;
      }

      public void AddDesignation(IDesignation designation)
      {
        if (designation.IsDestroyed || designation.IsFulfilled)
          return;
        if (this.m_designations.IsNone)
          this.m_designations = (Option<Set<IDesignation>>) new Set<IDesignation>();
        if (!this.m_designations.Value.Add(designation))
          return;
        ++designation.UnreachableVehiclesCount;
      }

      public void ClearDesignations(bool clearForAnotherRetry)
      {
        if (this.m_designations.IsNone)
          return;
        Set<IDesignation> set = this.m_designations.Value;
        if (!clearForAnotherRetry || set.Count <= 10)
        {
          foreach (IDesignation designation in set)
            this.designationRemoved(designation);
          set.Clear();
        }
        else
        {
          Lyst<IDesignation> values = UnreachableTerrainDesignationsManager.VehicleData.DESIGNATIONS_CACHE.Value.ClearAndReturn();
          foreach (IDesignation designation in set)
          {
            if ((designation.OriginTileCoord.X >> 2) % 4 == this.m_clearingPhase)
            {
              values.Add(designation);
              this.designationRemoved(designation);
            }
          }
          set.RemoveRange((IEnumerable<IDesignation>) values);
          values.Clear();
        }
      }

      public void RemoveDesignation(IDesignation designation)
      {
        Set<IDesignation> valueOrNull = this.m_designations.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.Remove(designation) ? 1 : 0) : 0) == 0)
          return;
        this.designationRemoved(designation);
      }

      public void PruneDesignations()
      {
        this.m_designations.ValueOrNull?.RemoveWhere((Predicate<IDesignation>) (x => x.IsDestroyed || x.IsFulfilled), new Action<IDesignation>(this.designationRemoved));
      }

      private void designationRemoved(IDesignation designation)
      {
        if (designation.UnreachableVehiclesCount == (ushort) 0)
          Log.Error("Cannot go negative on IDesignation.UnreachableVehiclesCount!");
        else
          --designation.UnreachableVehiclesCount;
      }

      public bool IsEntityUnreachable(IEntity entity, bool includeCleared)
      {
        if (this.m_entities.Contains(entity))
          return true;
        return includeCleared && this.m_entitiesCleared.Contains(entity);
      }

      public bool AddEntity(IEntity entity)
      {
        bool flag = this.m_entitiesCleared.Remove(entity);
        return this.m_entities.Add(entity) && !flag;
      }

      private void clearEntities(Action<IEntity> onRemoved, bool clearForAnotherRetry)
      {
        this.m_entitiesCleared.ForEachAndClear(onRemoved);
        if (!clearForAnotherRetry)
          this.m_entities.ForEachAndClear(onRemoved);
        else if (this.m_entities.Count < 10)
        {
          this.m_entitiesCleared.AddRange((IEnumerable<IEntity>) this.m_entities);
          this.m_entities.Clear();
        }
        else
        {
          foreach (IEntity entity in this.m_entities)
          {
            if (entity.Id.Value % 4 == this.m_clearingPhase)
              this.m_entitiesCleared.Add(entity);
          }
          this.m_entities.RemoveRange((IEnumerable<IEntity>) this.m_entitiesCleared);
        }
      }

      public bool RemoveEntity(IEntity entity)
      {
        return this.m_entities.Remove(entity) || this.m_entitiesCleared.Remove(entity);
      }

      public void AddTree(TreeId tree)
      {
        if (this.m_trees.IsNone)
          this.m_trees = (Option<Set<TreeId>>) new Set<TreeId>();
        this.m_trees.Value.Add(tree);
      }

      public void ClearTrees(bool clearForAnotherRetry)
      {
        if (this.m_trees.IsNone)
          return;
        if (!clearForAnotherRetry || this.m_trees.Value.Count <= 10)
        {
          this.m_trees.Value.Clear();
        }
        else
        {
          Lyst<TreeId> values = UnreachableTerrainDesignationsManager.VehicleData.TREES_CACHE.Value.ClearAndReturn();
          foreach (TreeId treeId in this.m_trees.Value)
          {
            if ((int) treeId.Position.X % 4 == this.m_clearingPhase)
              values.Add(treeId);
          }
          this.m_trees.Value.RemoveRange((IEnumerable<TreeId>) values);
        }
      }

      public void RemoveTree(TreeId tree) => this.m_trees.ValueOrNull?.Remove(tree);

      public void AddTile(Tile2i tile)
      {
        if (this.m_tiles.IsNone)
          this.m_tiles = (Option<Set<Tile2iSlim>>) new Set<Tile2iSlim>();
        this.m_tiles.Value.Add(tile.AsSlim);
      }

      public void RemoveTile(Tile2i tile) => this.m_tiles.ValueOrNull?.Remove(tile.AsSlim);

      public void ClearTiles(bool clearForAnotherRetry)
      {
        if (this.m_tiles.IsNone)
          return;
        if (!clearForAnotherRetry || this.m_tiles.Value.Count <= 10)
        {
          this.m_tiles.Value.Clear();
        }
        else
        {
          Lyst<Tile2iSlim> values = UnreachableTerrainDesignationsManager.VehicleData.TILES_CACHE.Value.ClearAndReturn();
          foreach (Tile2iSlim tile2iSlim in this.m_tiles.Value)
          {
            if ((int) tile2iSlim.X % 4 == this.m_clearingPhase)
              values.Add(tile2iSlim);
          }
          this.m_tiles.Value.RemoveRange((IEnumerable<Tile2iSlim>) values);
        }
      }

      public void AddChunk(Chunk2i chunk)
      {
        if (this.m_chunks.IsNone)
          this.m_chunks = (Option<Set<Chunk2i>>) new Set<Chunk2i>();
        this.m_chunks.Value.Add(chunk);
      }

      private void clearChunks(bool clearForAnotherRetry)
      {
        if (this.m_chunks.IsNone)
          return;
        if (!clearForAnotherRetry || this.m_chunks.Value.Count <= 1)
        {
          this.m_chunks.Value.Clear();
        }
        else
        {
          Lyst<Chunk2i> values = UnreachableTerrainDesignationsManager.VehicleData.CHUNKS_CACHE.Value.ClearAndReturn();
          foreach (Chunk2i chunk2i in this.m_chunks.Value)
          {
            if (chunk2i.X % 4 == this.m_clearingPhase)
              values.Add(chunk2i);
          }
          this.m_chunks.Value.RemoveRange((IEnumerable<Chunk2i>) values);
        }
      }

      public void Clear(Action<IEntity> onEntityRemoved, bool clearForAnotherRetry)
      {
        this.ClearDesignations(clearForAnotherRetry);
        this.clearEntities(onEntityRemoved, clearForAnotherRetry);
        this.clearChunks(clearForAnotherRetry);
        this.ClearTiles(clearForAnotherRetry);
        this.ClearTrees(clearForAnotherRetry);
        if (clearForAnotherRetry)
          this.m_clearingPhase = (this.m_clearingPhase + 1) % 4;
        this.FirstIdleReportSinceClear = new SimStep?();
      }

      public bool IsEmpty()
      {
        Set<IDesignation> valueOrNull1 = this.m_designations.ValueOrNull;
        if ((valueOrNull1 != null ? (valueOrNull1.IsEmpty ? 1 : 0) : 1) != 0 && this.m_entities.IsEmpty && this.m_entitiesCleared.IsEmpty)
        {
          Set<TreeId> valueOrNull2 = this.m_trees.ValueOrNull;
          if ((valueOrNull2 != null ? (valueOrNull2.IsEmpty ? 1 : 0) : 1) != 0)
          {
            Set<Tile2iSlim> valueOrNull3 = this.m_tiles.ValueOrNull;
            if ((valueOrNull3 != null ? (valueOrNull3.IsEmpty ? 1 : 0) : 1) != 0)
            {
              Set<Chunk2i> valueOrNull4 = this.m_chunks.ValueOrNull;
              return valueOrNull4 == null || valueOrNull4.IsEmpty;
            }
          }
        }
        return false;
      }

      static VehicleData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UnreachableTerrainDesignationsManager.VehicleData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnreachableTerrainDesignationsManager.VehicleData) obj).SerializeData(writer));
        UnreachableTerrainDesignationsManager.VehicleData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnreachableTerrainDesignationsManager.VehicleData) obj).DeserializeData(reader));
        UnreachableTerrainDesignationsManager.VehicleData.DESIGNATIONS_CACHE = new ThreadLocal<Lyst<IDesignation>>((Func<Lyst<IDesignation>>) (() => new Lyst<IDesignation>()));
        UnreachableTerrainDesignationsManager.VehicleData.TREES_CACHE = new ThreadLocal<Lyst<TreeId>>((Func<Lyst<TreeId>>) (() => new Lyst<TreeId>()));
        UnreachableTerrainDesignationsManager.VehicleData.CHUNKS_CACHE = new ThreadLocal<Lyst<Chunk2i>>((Func<Lyst<Chunk2i>>) (() => new Lyst<Chunk2i>()));
        UnreachableTerrainDesignationsManager.VehicleData.TILES_CACHE = new ThreadLocal<Lyst<Tile2iSlim>>((Func<Lyst<Tile2iSlim>>) (() => new Lyst<Tile2iSlim>()));
      }
    }
  }
}
