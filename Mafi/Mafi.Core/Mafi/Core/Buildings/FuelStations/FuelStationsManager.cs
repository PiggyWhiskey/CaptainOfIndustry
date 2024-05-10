// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.FuelStationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [MemberRemovedInSaveVersion("m_hasRefuelingTruck", 162, typeof (bool), 0, false)]
  public class FuelStationsManager : 
    IFuelStationsManager,
    ICommandProcessor<ToggleFuelStationTrucksAllowedToRefuelCmd>,
    IAction<ToggleFuelStationTrucksAllowedToRefuelCmd>
  {
    private readonly Lyst<FuelStation> m_fuelStations;
    private Notificator m_noFuelNotificator;
    private readonly IProductsManager m_productsManager;
    private readonly NavigateToJob.Factory m_navigateToJobFactory;
    private readonly RefuelSelfJob.Factory m_refuelVehicleJobFactory;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly CargoPickUpJob.Factory m_cargoPickUpJobFactory;
    private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
    private readonly IVehiclePathFindingManager m_pathFindingManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private readonly VehicleJobStatsManager m_jobStatsManager;
    [NewInSaveVersion(140, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
    private readonly UnreachableTerrainDesignationsManager m_unreachablesManager;
    private readonly StaticEntityVehicleGoal.Factory m_staticEntityGoalFactory;
    private readonly DynamicEntityVehicleGoal.Factory m_dynamicEntityGoalFactory;
    private readonly RefuelOtherVehicleJob.Factory m_refuelOtherVehicleJobFactory;
    private readonly Set<Vehicle> m_vehiclesNeedingFuel;
    [OnlyForSaveCompatibility(null)]
    private readonly Dict<Vehicle, Option<RefuelOtherVehicleJob>> m_vehiclesNeedingFuelTruck;
    [NewInSaveVersion(162, null, "new()", null, null)]
    private readonly Set<ProductProto> m_hasFuelTruck;
    [DoNotSave(0, null)]
    private LystStruct<Vehicle> m_vehiclesCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Vehicle> m_vehiclesToRemoveCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int VehiclesNeedingFuelCount
    {
      get => this.m_vehiclesNeedingFuel.Count + this.m_vehiclesNeedingFuelTruck.Count;
    }

    public FuelStationsManager(
      ISimLoopEvents simLoopEvents,
      IConstructionManager constructionManager,
      IVehiclesManager vehiclesManager,
      INotificationsManager notifsManager,
      IProductsManager productsManager,
      NavigateToJob.Factory navigateToJobFactory,
      RefuelSelfJob.Factory refuelVehicleJobFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      CargoPickUpJob.Factory cargoPickUpJobFactory,
      VehicleJobId.Factory vehicleJobIdFactory,
      RefuelOtherVehicleJob.Factory refuelOtherVehicleJobFactory,
      IVehiclePathFindingManager pathFindingManager,
      IFuelStatsCollector fuelStatsCollector,
      VehicleJobStatsManager jobStatsManager,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
      DynamicEntityVehicleGoal.Factory dynamicEntityGoalFactory,
      UnreachableTerrainDesignationsManager unreachablesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_fuelStations = new Lyst<FuelStation>();
      this.m_vehiclesNeedingFuel = new Set<Vehicle>();
      this.m_vehiclesNeedingFuelTruck = new Dict<Vehicle, Option<RefuelOtherVehicleJob>>();
      this.m_hasFuelTruck = new Set<ProductProto>();
      this.m_vehiclesToRemoveCache = new Lyst<Vehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_navigateToJobFactory = navigateToJobFactory;
      this.m_refuelVehicleJobFactory = refuelVehicleJobFactory;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_cargoPickUpJobFactory = cargoPickUpJobFactory;
      this.m_vehicleJobIdFactory = vehicleJobIdFactory;
      this.m_pathFindingManager = pathFindingManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_jobStatsManager = jobStatsManager;
      this.m_staticEntityGoalFactory = staticEntityGoalFactory;
      this.m_dynamicEntityGoalFactory = dynamicEntityGoalFactory;
      this.m_unreachablesManager = unreachablesManager;
      this.m_refuelOtherVehicleJobFactory = refuelOtherVehicleJobFactory;
      this.m_noFuelNotificator = notifsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughFuelToRefuel);
      constructionManager.EntityConstructed.Add<FuelStationsManager>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<FuelStationsManager>(this, new Action<IStaticEntity>(this.onEntityDeconstructionStarted));
      vehiclesManager.OnVehicleDespawned.Add<FuelStationsManager>(this, new Action<Vehicle>(this.vehicleDespawned));
      simLoopEvents.Update.Add<FuelStationsManager>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      this.m_hasFuelTruck.Clear();
      foreach (FuelStation fuelStation in this.m_fuelStations)
      {
        if (fuelStation.CanRefuelOthers)
          this.m_hasFuelTruck.Add(fuelStation.FuelProto);
      }
      foreach (Vehicle key in this.m_vehiclesNeedingFuelTruck.Keys)
      {
        if (!this.m_hasFuelTruck.Contains(key.FuelTank.ValueOrNull.Proto.Product ?? ProductProto.Phantom))
          this.m_vehiclesCache.Add(key);
      }
      foreach (Vehicle key in this.m_vehiclesCache)
      {
        key.LastRefuelRequestIssue = RefuelRequestIssue.Failed;
        this.m_vehiclesNeedingFuelTruck.Remove(key);
      }
      this.m_vehiclesCache.Clear();
      this.m_noFuelNotificator.NotifyIff(this.m_vehiclesNeedingFuel.IsNotEmpty);
    }

    public bool TryRefuelSelf(Vehicle vehicle)
    {
      Assert.That<Option<IFuelTankReadonly>>(vehicle.FuelTank).HasValue<IFuelTankReadonly>();
      IReadOnlySet<IEntityAssignedAsOutput> preferredEntities = (IReadOnlySet<IEntityAssignedAsOutput>) null;
      if (vehicle.AssignedTo.HasValue)
      {
        if (vehicle.AssignedTo.Value is MineTower mineTower)
          preferredEntities = mineTower.AssignedOutputs;
        else if (vehicle.AssignedTo.Value is ForestryTower forestryTower)
          preferredEntities = forestryTower.AssignedOutputs;
      }
      Option<RegisteredOutputBuffer> outputForVehicle = this.m_vehicleBuffersRegistry.TryGetProductOutputForVehicle(vehicle, vehicle.FuelTank.Value.Proto.Product, preferredEntities: preferredEntities, isForRefueling: true);
      if (outputForVehicle.IsNone)
      {
        this.m_vehiclesNeedingFuel.Add(vehicle);
      }
      else
      {
        this.m_vehiclesNeedingFuel.Clear();
        this.m_refuelVehicleJobFactory.EnqueueJob(vehicle, outputForVehicle.Value);
      }
      return outputForVehicle.HasValue;
    }

    public bool TryGetRefuelOtherVehicleJob(FuelStation fuelStation, Truck refuelingTruck)
    {
      if (refuelingTruck.IsEmpty)
        return false;
      Assert.That<bool>(refuelingTruck.IsDriving).IsFalse();
      Fix64 fix64_1 = Fix64.MaxIntValue;
      Option<Vehicle> option = Option<Vehicle>.None;
      bool? isThereFreeStationCache = new bool?();
      IReadOnlySet<IEntity> unreachableEntitiesFor = this.m_unreachablesManager.GetUnreachableEntitiesFor((IPathFindingVehicle) refuelingTruck);
      foreach (KeyValuePair<Vehicle, Option<RefuelOtherVehicleJob>> keyValuePair in this.m_vehiclesNeedingFuelTruck)
      {
        if (!keyValuePair.Value.HasValue)
        {
          Vehicle vehicleToRefuel = keyValuePair.Key;
          if (vehicleToRefuel.IsDestroyed || !vehicleToRefuel.IsSpawned)
          {
            Assert.Fail(string.Format("Trying to refuel destroyed or non-spawned vehicle {0}", (object) vehicleToRefuel.Prototype.Id));
            this.m_vehiclesToRemoveCache.Add(vehicleToRefuel);
            vehicleToRefuel.LastRefuelRequestIssue = RefuelRequestIssue.Failed;
          }
          else
          {
            Option<IFuelTankReadonly> fuelTank = vehicleToRefuel.FuelTank;
            if (fuelTank.IsNone || !vehicleToRefuel.NeedsRefueling)
              this.m_vehiclesToRemoveCache.Add(vehicleToRefuel);
            else if (!refuelingTruck.IsNotFull || !(refuelingTruck.Cargo.TotalQuantity < vehicleToRefuel.FuelTank.Value.GetFreeCapacity()))
            {
              ProductProto product1 = refuelingTruck.Cargo.FirstOrPhantom.Product;
              fuelTank = vehicleToRefuel.FuelTank;
              ProductProto product2 = fuelTank.ValueOrNull?.Proto.Product;
              if (!((Proto) product1 != (Proto) product2))
              {
                if (unreachableEntitiesFor.Contains((IEntity) vehicleToRefuel))
                {
                  if (vehicleToRefuel.CannotWorkDueToLowFuel)
                  {
                    this.m_vehiclesToRemoveCache.Add(vehicleToRefuel);
                    vehicleToRefuel.LastRefuelRequestIssue = RefuelRequestIssue.FailedAsUnreachable;
                  }
                }
                else
                {
                  Fix64 fix64_2 = refuelingTruck.Position2f.DistanceSqrTo(vehicleToRefuel.Position2f);
                  if (!(fix64_2 >= fix64_1) && hasToRefuel(vehicleToRefuel.FuelTank.Value.Proto.Product))
                  {
                    fix64_1 = fix64_2;
                    option = (Option<Vehicle>) vehicleToRefuel;
                  }
                }
              }
            }
          }

          bool hasToRefuel(ProductProto fuelProto)
          {
            bool flag = fuelStation.AssignedInputs.IsNotEmpty<IEntityAssignedAsInput>();
            Option<IEntityAssignedWithVehicles> assignedTo;
            if (flag)
            {
              assignedTo = vehicleToRefuel.AssignedTo;
              if (assignedTo.ValueOrNull is IEntityAssignedAsInput valueOrNull1 && fuelStation.AssignedInputs.Contains(valueOrNull1))
                return true;
            }
            assignedTo = vehicleToRefuel.AssignedTo;
            if (assignedTo.ValueOrNull is IAreaManagingTower valueOrNull2 && hasAssignedOperationalFuelStation(valueOrNull2, fuelProto))
              return false;
            if (!flag)
              return true;
            if (!isThereFreeStationCache.HasValue)
              isThereFreeStationCache = new bool?(isThereNonAssignedFuelStation(fuelProto));
            return !isThereFreeStationCache.Value;
          }
        }
      }
      if (this.m_vehiclesToRemoveCache.IsNotEmpty)
        this.m_vehiclesToRemoveCache.ForEachAndClear<bool>(new Func<Vehicle, bool>(this.m_vehiclesNeedingFuelTruck.Remove));
      if (!option.HasValue)
        return false;
      RefuelOtherVehicleJob refuelOtherVehicleJob = this.m_refuelOtherVehicleJobFactory.EnqueueJob(refuelingTruck, option.Value);
      this.m_vehiclesNeedingFuelTruck[option.Value] = (Option<RefuelOtherVehicleJob>) refuelOtherVehicleJob;
      return true;

      static bool hasAssignedOperationalFuelStation(
        IAreaManagingTower tower,
        ProductProto fuelProto)
      {
        foreach (FuelStation assignedFuelStation in (IEnumerable<FuelStation>) tower.AssignedFuelStations)
        {
          if (assignedFuelStation.CanRefuelOthers && (Proto) assignedFuelStation.FuelProto == (Proto) fuelProto)
            return true;
        }
        return false;
      }

      bool isThereNonAssignedFuelStation(ProductProto fuelProto)
      {
        foreach (FuelStation fuelStation in this.m_fuelStations)
        {
          if (fuelStation.CanRefuelOthers && (Proto) fuelStation.FuelProto == (Proto) fuelProto && fuelStation.AssignedInputs.IsEmpty<IEntityAssignedAsInput>())
            return true;
        }
        return false;
      }
    }

    /// <summary>
    /// Returns lists of vehicles that are so close to the current vehicle that they can
    /// be just refueled immediately.
    /// </summary>
    public void GetNearbyVehiclesToRefuel(
      Truck truck,
      Vehicle refueledVehicle,
      Lyst<Vehicle> result)
    {
      if (refueledVehicle.AssignedTo.IsNone || !(refueledVehicle.AssignedTo.Value is MineTower mineTower))
        return;
      foreach (Excavator assignedExcavator in mineTower.AllAssignedExcavators)
      {
        if (assignedExcavator != refueledVehicle && truck.Position2f.DistanceSqrTo(assignedExcavator.Position2f) < 400L)
          result.Add((Vehicle) assignedExcavator);
      }
    }

    public void RefuelOtherVehicleJobCancelled(RefuelOtherVehicleJob job)
    {
      if (!this.m_vehiclesNeedingFuelTruck.ContainsKey(job.VehicleToRefuel))
        return;
      this.m_vehiclesNeedingFuelTruck[job.VehicleToRefuel] = Option<RefuelOtherVehicleJob>.None;
    }

    public void RefuelOtherVehicleJobCompleted(RefuelOtherVehicleJob job)
    {
      this.m_vehiclesNeedingFuelTruck.Remove(job.VehicleToRefuel);
    }

    public bool TryRequestTruckForRefueling(Vehicle vehicle)
    {
      if (vehicle.LastRefuelRequestIssue == RefuelRequestIssue.FailedAsUnreachable)
        return false;
      ProductProto productProto = vehicle.FuelTank.ValueOrNull?.Proto.Product ?? ProductProto.Phantom;
      Option<RefuelOtherVehicleJob> option;
      if (this.m_vehiclesNeedingFuelTruck.TryGetValue(vehicle, out option))
      {
        if (option.HasValue)
        {
          Log.Error(string.Format("Vehicle {0} is already getting fuel!", (object) vehicle.Id));
          return true;
        }
        if (this.m_hasFuelTruck.Contains(productProto))
          return true;
        this.m_vehiclesNeedingFuelTruck.Remove(vehicle);
        return false;
      }
      if (!this.m_hasFuelTruck.Contains(productProto))
        return false;
      this.m_vehiclesNeedingFuelTruck.Add(vehicle, (Option<RefuelOtherVehicleJob>) Option.None);
      return true;
    }

    public void VehicleRefueledByTruck(Vehicle vehicle)
    {
      this.m_vehiclesNeedingFuelTruck.Remove(vehicle);
    }

    public void OnRecoveryPerformed(Vehicle vehicle)
    {
      if (!this.m_vehiclesNeedingFuelTruck.Remove(vehicle))
        return;
      vehicle.LastRefuelRequestIssue = RefuelRequestIssue.Failed;
    }

    private void onEntityConstructed(IStaticEntity entity)
    {
      if (!(entity is FuelStation fuelStation))
        return;
      this.m_fuelStations.Add(fuelStation);
    }

    private void onEntityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is FuelStation fuelStation))
        return;
      this.m_fuelStations.TryRemoveReplaceLast(fuelStation);
    }

    private void vehicleDespawned(Vehicle vehicle)
    {
      if (this.m_vehiclesNeedingFuel.IsNotEmpty)
        this.m_vehiclesNeedingFuel.Remove(vehicle);
      Option<RefuelOtherVehicleJob> option;
      if (!this.m_vehiclesNeedingFuelTruck.TryGetValue(vehicle, out option))
        return;
      if (option.HasValue)
        option.Value.RequestCancel();
      this.m_vehiclesNeedingFuelTruck.Remove(vehicle);
    }

    public void Invoke(ToggleFuelStationTrucksAllowedToRefuelCmd cmd)
    {
      FuelStation fuelStation = this.m_fuelStations.FirstOrDefault<FuelStation>((Predicate<FuelStation>) (x => x.Id == cmd.FuelStationId));
      if (fuelStation == null)
      {
        cmd.SetResultError(string.Format("Fuel station with id {0} not found!", (object) cmd.FuelStationId));
      }
      else
      {
        cmd.SetResultSuccess();
        fuelStation.ToggleAllowTrucksToRefuel();
      }
    }

    public static void Serialize(FuelStationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FuelStationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FuelStationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      DynamicEntityVehicleGoal.Factory.Serialize(this.m_dynamicEntityGoalFactory, writer);
      Lyst<FuelStation>.Serialize(this.m_fuelStations, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      Set<ProductProto>.Serialize(this.m_hasFuelTruck, writer);
      VehicleJobStatsManager.Serialize(this.m_jobStatsManager, writer);
      Notificator.Serialize(this.m_noFuelNotificator, writer);
      writer.WriteGeneric<IVehiclePathFindingManager>(this.m_pathFindingManager);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      StaticEntityVehicleGoal.Factory.Serialize(this.m_staticEntityGoalFactory, writer);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachablesManager, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      VehicleJobId.Factory.Serialize(this.m_vehicleJobIdFactory, writer);
      Set<Vehicle>.Serialize(this.m_vehiclesNeedingFuel, writer);
      Dict<Vehicle, Option<RefuelOtherVehicleJob>>.Serialize(this.m_vehiclesNeedingFuelTruck, writer);
    }

    public static FuelStationsManager Deserialize(BlobReader reader)
    {
      FuelStationsManager fuelStationsManager;
      if (reader.TryStartClassDeserialization<FuelStationsManager>(out fuelStationsManager))
        reader.EnqueueDataDeserialization((object) fuelStationsManager, FuelStationsManager.s_deserializeDataDelayedAction);
      return fuelStationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<FuelStationsManager>(this, "m_cargoPickUpJobFactory", typeof (CargoPickUpJob.Factory), true);
      reader.SetField<FuelStationsManager>(this, "m_dynamicEntityGoalFactory", (object) DynamicEntityVehicleGoal.Factory.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_fuelStations", (object) Lyst<FuelStation>.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      reader.SetField<FuelStationsManager>(this, "m_hasFuelTruck", reader.LoadedSaveVersion >= 162 ? (object) Set<ProductProto>.Deserialize(reader) : (object) new Set<ProductProto>());
      if (reader.LoadedSaveVersion < 162)
        reader.ReadBool();
      reader.SetField<FuelStationsManager>(this, "m_jobStatsManager", (object) VehicleJobStatsManager.Deserialize(reader));
      reader.RegisterResolvedMember<FuelStationsManager>(this, "m_navigateToJobFactory", typeof (NavigateToJob.Factory), true);
      this.m_noFuelNotificator = Notificator.Deserialize(reader);
      reader.SetField<FuelStationsManager>(this, "m_pathFindingManager", (object) reader.ReadGenericAs<IVehiclePathFindingManager>());
      reader.SetField<FuelStationsManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<FuelStationsManager>(this, "m_refuelOtherVehicleJobFactory", typeof (RefuelOtherVehicleJob.Factory), true);
      reader.RegisterResolvedMember<FuelStationsManager>(this, "m_refuelVehicleJobFactory", typeof (RefuelSelfJob.Factory), true);
      reader.SetField<FuelStationsManager>(this, "m_staticEntityGoalFactory", (object) StaticEntityVehicleGoal.Factory.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_unreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<FuelStationsManager>(this, "m_unreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      reader.SetField<FuelStationsManager>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<FuelStationsManager>(this, "m_vehicleJobIdFactory", (object) VehicleJobId.Factory.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_vehiclesNeedingFuel", (object) Set<Vehicle>.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_vehiclesNeedingFuelTruck", (object) Dict<Vehicle, Option<RefuelOtherVehicleJob>>.Deserialize(reader));
      reader.SetField<FuelStationsManager>(this, "m_vehiclesToRemoveCache", (object) new Lyst<Vehicle>());
    }

    static FuelStationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FuelStationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FuelStationsManager) obj).SerializeData(writer));
      FuelStationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FuelStationsManager) obj).DeserializeData(reader));
    }
  }
}
