// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.TruckJobProviderContext
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [OnlyForSaveCompatibility(null)]
  [GenerateSerializer(false, null, 0)]
  public class TruckJobProviderContext
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly VehicleJobStatsManager VehicleJobStatsManager;
    public readonly IVehiclesManager VehiclesManager;
    public readonly ITerrainDesignationsManager TerrainDesignationManager;
    public readonly ITerrainDumpingManager TerrainDumpingManager;
    public readonly IFuelStationsManager FuelStationsManager;
    public readonly ITreesManager TreeManager;
    public readonly VehicleLastOutputBufferManager VehicleLastOutputBufferManager;
    [NewInSaveVersion(140, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
    public readonly UnreachableTerrainDesignationsManager UnreachablesManager;
    [NewInSaveVersion(140, null, null, null, null)]
    public readonly OreSortingPlantsManager OreSortingPlantsManager;
    public readonly CargoPickUpJob.Factory PickUpJobFactory;
    public readonly CargoDeliveryJob.Factory DeliveryJobFactory;
    public readonly DumpingJob.Factory DumpJobFactory;
    [NewInSaveVersion(140, null, null, typeof (SurfaceModificationJob.Factory), null)]
    public readonly SurfaceModificationJob.Factory SurfaceJobFactory;
    [NewInSaveVersion(140, null, null, typeof (ChainedNavigationJob.Factory), null)]
    public readonly ChainedNavigationJob.Factory ChainedNavJobFactory;
    public readonly NavigateToJob.Factory NavigateToJobFactory;
    public readonly WaitingJob.Factory WaitingJobFactory;
    public readonly ParkAndWaitJobFactory ParkAndWaitJobFactory;
    public readonly IVehicleBuffersRegistry VehicleBuffersRegistry;
    public readonly TreeHarvestingJob.Factory TreeHarvestingJobFactory;
    public readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;

    public static void Serialize(TruckJobProviderContext value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TruckJobProviderContext>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TruckJobProviderContext.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IFuelStationsManager>(this.FuelStationsManager);
      OreSortingPlantsManager.Serialize(this.OreSortingPlantsManager, writer);
      StaticEntityVehicleGoal.Factory.Serialize(this.StaticEntityGoalFactory, writer);
      writer.WriteGeneric<ITerrainDesignationsManager>(this.TerrainDesignationManager);
      writer.WriteGeneric<ITerrainDumpingManager>(this.TerrainDumpingManager);
      writer.WriteGeneric<ITreesManager>(this.TreeManager);
      UnreachableTerrainDesignationsManager.Serialize(this.UnreachablesManager, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.VehicleBuffersRegistry);
      VehicleJobStatsManager.Serialize(this.VehicleJobStatsManager, writer);
      VehicleLastOutputBufferManager.Serialize(this.VehicleLastOutputBufferManager, writer);
      writer.WriteGeneric<IVehiclesManager>(this.VehiclesManager);
    }

    public static TruckJobProviderContext Deserialize(BlobReader reader)
    {
      TruckJobProviderContext jobProviderContext;
      if (reader.TryStartClassDeserialization<TruckJobProviderContext>(out jobProviderContext))
        reader.EnqueueDataDeserialization((object) jobProviderContext, TruckJobProviderContext.s_deserializeDataDelayedAction);
      return jobProviderContext;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "ChainedNavJobFactory", typeof (ChainedNavigationJob.Factory), true);
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "DeliveryJobFactory", typeof (CargoDeliveryJob.Factory), true);
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "DumpJobFactory", typeof (DumpingJob.Factory), true);
      reader.SetField<TruckJobProviderContext>(this, "FuelStationsManager", (object) reader.ReadGenericAs<IFuelStationsManager>());
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "NavigateToJobFactory", typeof (NavigateToJob.Factory), true);
      reader.SetField<TruckJobProviderContext>(this, "OreSortingPlantsManager", reader.LoadedSaveVersion >= 140 ? (object) OreSortingPlantsManager.Deserialize(reader) : (object) (OreSortingPlantsManager) null);
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "ParkAndWaitJobFactory", typeof (ParkAndWaitJobFactory), true);
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "PickUpJobFactory", typeof (CargoPickUpJob.Factory), true);
      reader.SetField<TruckJobProviderContext>(this, "StaticEntityGoalFactory", (object) StaticEntityVehicleGoal.Factory.Deserialize(reader));
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "SurfaceJobFactory", typeof (SurfaceModificationJob.Factory), true);
      reader.SetField<TruckJobProviderContext>(this, "TerrainDesignationManager", (object) reader.ReadGenericAs<ITerrainDesignationsManager>());
      reader.SetField<TruckJobProviderContext>(this, "TerrainDumpingManager", (object) reader.ReadGenericAs<ITerrainDumpingManager>());
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "TreeHarvestingJobFactory", typeof (TreeHarvestingJob.Factory), true);
      reader.SetField<TruckJobProviderContext>(this, "TreeManager", (object) reader.ReadGenericAs<ITreesManager>());
      reader.SetField<TruckJobProviderContext>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<TruckJobProviderContext>(this, "UnreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      reader.SetField<TruckJobProviderContext>(this, "VehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<TruckJobProviderContext>(this, "VehicleJobStatsManager", (object) VehicleJobStatsManager.Deserialize(reader));
      reader.SetField<TruckJobProviderContext>(this, "VehicleLastOutputBufferManager", (object) VehicleLastOutputBufferManager.Deserialize(reader));
      reader.SetField<TruckJobProviderContext>(this, "VehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      reader.RegisterResolvedMember<TruckJobProviderContext>(this, "WaitingJobFactory", typeof (WaitingJob.Factory), true);
      reader.RegisterInitAfterLoad<TruckJobProviderContext>(this, "initSelf", InitPriority.Low);
    }

    public TruckJobProviderContext(
      IFuelStationsManager fuelStationsManager,
      IVehiclesManager vehiclesManager,
      ITerrainDesignationsManager terrainDesignationManager,
      ITerrainDumpingManager terrainDumpingManager,
      ITreesManager treeManager,
      UnreachableTerrainDesignationsManager unreachablesManager,
      OreSortingPlantsManager oreSortingPlantsManager,
      CargoPickUpJob.Factory pickUpJobFactory,
      CargoDeliveryJob.Factory deliveryJobFactory,
      DumpingJob.Factory dumpJobFactory,
      SurfaceModificationJob.Factory surfaceJobFactory,
      ChainedNavigationJob.Factory chainedNavJobFactory,
      NavigateToJob.Factory navigateToJobFactory,
      WaitingJob.Factory waitingJobFactory,
      ParkAndWaitJobFactory parkAndWaitJobFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      TreeHarvestingJob.Factory treeHarvestingJobFactory,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
      VehicleJobStatsManager vehicleJobStatsManager,
      VehicleLastOutputBufferManager vehicleLastOutputBufferManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleJobStatsManager = vehicleJobStatsManager;
      this.VehicleLastOutputBufferManager = vehicleLastOutputBufferManager;
      this.FuelStationsManager = fuelStationsManager;
      this.VehiclesManager = vehiclesManager;
      this.TerrainDesignationManager = terrainDesignationManager;
      this.TerrainDumpingManager = terrainDumpingManager;
      this.TreeManager = treeManager;
      this.UnreachablesManager = unreachablesManager;
      this.OreSortingPlantsManager = oreSortingPlantsManager;
      this.PickUpJobFactory = pickUpJobFactory;
      this.DeliveryJobFactory = deliveryJobFactory;
      this.DumpJobFactory = dumpJobFactory;
      this.SurfaceJobFactory = surfaceJobFactory;
      this.ChainedNavJobFactory = chainedNavJobFactory;
      this.NavigateToJobFactory = navigateToJobFactory;
      this.WaitingJobFactory = waitingJobFactory;
      this.ParkAndWaitJobFactory = parkAndWaitJobFactory;
      this.VehicleBuffersRegistry = vehicleBuffersRegistry;
      this.TreeHarvestingJobFactory = treeHarvestingJobFactory;
      this.StaticEntityGoalFactory = staticEntityGoalFactory;
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(DependencyResolver resolver)
    {
      ReflectionUtils.SetField<TruckJobProviderContext>(this, "OreSortingPlantsManager", (object) resolver.Resolve<OreSortingPlantsManager>());
    }

    static TruckJobProviderContext()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TruckJobProviderContext.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TruckJobProviderContext) obj).SerializeData(writer));
      TruckJobProviderContext.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TruckJobProviderContext) obj).DeserializeData(reader));
    }
  }
}
