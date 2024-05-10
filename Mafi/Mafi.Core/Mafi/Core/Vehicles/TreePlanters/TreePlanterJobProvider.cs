// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreePlanters.TreePlanterJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Mafi.Core.Vehicles.TreePlanters
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TreePlanterJobProvider : IJobProvider<TreePlanter>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThreadLocal<Set<IEntityAssignedAsOutput>> s_entitiesCache;
    private readonly ITreePlantingManager m_treeManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IFuelStationsManager m_fuelStationsManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly CargoPickUpJob.Factory m_cargoPickupJobFactory;
    private readonly TreePlantingJob.Factory m_treePlantingJobFactory;
    private readonly ParkAndWaitJobFactory m_parkAndWaitJobFactory;

    public static void Serialize(TreePlanterJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreePlanterJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreePlanterJobProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IFuelStationsManager>(this.m_fuelStationsManager);
      writer.WriteGeneric<ITreePlantingManager>(this.m_treeManager);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
    }

    public static TreePlanterJobProvider Deserialize(BlobReader reader)
    {
      TreePlanterJobProvider planterJobProvider;
      if (reader.TryStartClassDeserialization<TreePlanterJobProvider>(out planterJobProvider))
        reader.EnqueueDataDeserialization((object) planterJobProvider, TreePlanterJobProvider.s_deserializeDataDelayedAction);
      return planterJobProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<TreePlanterJobProvider>(this, "m_cargoPickupJobFactory", typeof (CargoPickUpJob.Factory), true);
      reader.SetField<TreePlanterJobProvider>(this, "m_fuelStationsManager", (object) reader.ReadGenericAs<IFuelStationsManager>());
      reader.RegisterResolvedMember<TreePlanterJobProvider>(this, "m_parkAndWaitJobFactory", typeof (ParkAndWaitJobFactory), true);
      reader.SetField<TreePlanterJobProvider>(this, "m_treeManager", (object) reader.ReadGenericAs<ITreePlantingManager>());
      reader.RegisterResolvedMember<TreePlanterJobProvider>(this, "m_treePlantingJobFactory", typeof (TreePlantingJob.Factory), true);
      reader.SetField<TreePlanterJobProvider>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<TreePlanterJobProvider>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
    }

    public TreePlanterJobProvider(
      ITreePlantingManager treeManager,
      IVehiclesManager vehiclesManager,
      IFuelStationsManager fuelStationsManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      CargoPickUpJob.Factory cargoPickupJobFactory,
      TreePlantingJob.Factory treePlantingJobFactory,
      ParkAndWaitJobFactory parkAndWaitJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_fuelStationsManager = fuelStationsManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_cargoPickupJobFactory = cargoPickupJobFactory;
      this.m_treePlantingJobFactory = treePlantingJobFactory;
      this.m_parkAndWaitJobFactory = parkAndWaitJobFactory;
    }

    public bool TryGetJobFor(TreePlanter planter)
    {
      Assert.That<bool>(planter.IsSpawned).IsTrue();
      Assert.That<bool>(planter.IsOnWayToDepotForScrap).IsFalse();
      Assert.That<bool>(planter.IsOnWayToDepotForReplacement).IsFalse();
      if (planter.HasJobs)
      {
        Log.Warning("Tree planter already has a job assigned!");
        return false;
      }
      if (planter.LastRefuelRequestIssue != RefuelRequestIssue.None)
      {
        if (this.m_fuelStationsManager.TryRequestTruckForRefueling((Vehicle) planter))
        {
          planter.LastRefuelRequestIssue = RefuelRequestIssue.None;
          return false;
        }
        if (this.m_fuelStationsManager.TryRefuelSelf((Vehicle) planter))
          return true;
      }
      if (planter.CannotWorkDueToLowFuel)
        return false;
      if (planter.Cargo.IsEmpty)
        return this.tryGetCargoPickupJob(planter);
      if (this.tryGetTreePlantingJob(planter))
        return true;
      return planter.ForestryTower.HasValue && this.m_parkAndWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) planter, (ILayoutEntity) planter.ForestryTower.Value);
    }

    /// <summary>Attempts to get a pick up job for an empty planter.</summary>
    private bool tryGetCargoPickupJob(TreePlanter planter)
    {
      IReadOnlySet<IEntityAssignedAsOutput> preferredEntities = (IReadOnlySet<IEntityAssignedAsOutput>) null;
      bool forcePreferredEntity = false;
      if (planter.AssignedTo.HasValue && planter.AssignedTo.Value is ForestryTower forestryTower && forestryTower.AssignedOutputStorages.Count > 0)
      {
        Set<IEntityAssignedAsOutput> set = TreePlanterJobProvider.s_entitiesCache.Value.ClearAndReturn();
        foreach (Storage assignedOutputStorage in (IEnumerable<Storage>) forestryTower.AssignedOutputStorages)
        {
          if ((Proto) assignedOutputStorage.StoredProduct.ValueOrNull == (Proto) planter.ProductProto)
            set.Add((IEntityAssignedAsOutput) assignedOutputStorage);
        }
        preferredEntities = (IReadOnlySet<IEntityAssignedAsOutput>) set;
        forcePreferredEntity = !forestryTower.AllowNonAssignedOutput;
      }
      Option<RegisteredOutputBuffer> outputForVehicle = this.m_vehicleBuffersRegistry.TryGetProductOutputForVehicle((Vehicle) planter, planter.ProductProto, preferredEntities: preferredEntities, forcePreferredEntity: forcePreferredEntity);
      if (outputForVehicle.HasValue)
        this.m_cargoPickupJobFactory.EnqueueJob((IVehicleForCargoJob) planter, new ProductQuantity(planter.ProductProto, planter.RemainingCapacity), outputForVehicle.Value, (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None);
      return outputForVehicle.HasValue;
    }

    /// <summary>Attempts to get a tree planting job for the planter.</summary>
    private bool tryGetTreePlantingJob(TreePlanter planter)
    {
      Assert.That<bool>(planter.IsNotEmpty).IsTrue();
      Option<ForestryTower> tmpTower = Option<ForestryTower>.None;
      IReadOnlySet<Tile2iSlim> unreachableTilesFor = this.m_treePlantingJobFactory.UnreachablesManager.GetUnreachableTilesFor((IPathFindingVehicle) planter);
      Option<TreeProto> treeProto;
      Tile2i plantingTile;
      if (planter.ForestryTower.IsNone)
      {
        TreeData? treeData;
        if (this.m_treeManager.TryGetAndReserveManualTree((Vehicle) planter, unreachableTilesFor, out treeData))
        {
          if (!treeData.HasValue)
          {
            Log.Error("treeData unexpectedly null with TryGetAndReserveManualTree returning true");
            return false;
          }
          treeProto = (Option<TreeProto>) treeData.Value.Proto;
          plantingTile = (Tile2i) treeData.Value.Id.Position;
        }
        else
        {
          tmpTower = this.m_treeManager.FindClosestTowerForPlantingFor((Vehicle) planter);
          if (tmpTower.IsNone || !tmpTower.Value.TryFindNextTargetForPlanting(planter.Position2f.Tile2i, unreachableTilesFor, out treeProto, out plantingTile))
            return false;
        }
      }
      else if (!planter.ForestryTower.Value.TryFindNextTargetForPlanting(planter.Position2f.Tile2i, unreachableTilesFor, out treeProto, out plantingTile))
        return false;
      this.m_treePlantingJobFactory.EnqueueJob(planter, treeProto.Value, plantingTile, tmpTower);
      return true;
    }

    static TreePlanterJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreePlanterJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TreePlanterJobProvider) obj).SerializeData(writer));
      TreePlanterJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TreePlanterJobProvider) obj).DeserializeData(reader));
      TreePlanterJobProvider.s_entitiesCache = new ThreadLocal<Set<IEntityAssignedAsOutput>>((Func<Set<IEntityAssignedAsOutput>>) (() => new Set<IEntityAssignedAsOutput>()));
    }
  }
}
