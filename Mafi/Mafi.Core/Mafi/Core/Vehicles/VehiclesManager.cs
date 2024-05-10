// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehiclesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Simulation;
using Mafi.Core.UnlockingTree;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class VehiclesManager : IVehiclesManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly Upoints VEHICLE_RECOVERY_COST;
    public readonly Event<Vehicle> m_onVehicleDespawned;
    private readonly Set<Vehicle> m_allVehicles;
    private readonly Set<Truck> m_trucks;
    private readonly Set<Excavator> m_excavators;
    private readonly Set<TreeHarvester> m_harvesters;
    private readonly Set<TreePlanter> m_planters;
    private int m_vehicleLimitUsedSoFar;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ScrapOrReplaceVehicleInDepotJob.Factory m_scrapOrReplaceVehicleJobFactory;
    private readonly RecoverVehicleJob.Factory m_recoverVehicleJobFactory;
    private readonly IUpointsManager m_upointsManager;
    private readonly Set<Vehicle> m_vehiclesToDestroy;
    private readonly Lyst<VehicleDepotBase> m_depots;

    public static void Serialize(VehiclesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehiclesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehiclesManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<Vehicle>.Serialize(this.m_allVehicles, writer);
      Lyst<VehicleDepotBase>.Serialize(this.m_depots, writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Set<Excavator>.Serialize(this.m_excavators, writer);
      Set<TreeHarvester>.Serialize(this.m_harvesters, writer);
      Event<Vehicle>.Serialize(this.m_onVehicleDespawned, writer);
      Set<TreePlanter>.Serialize(this.m_planters, writer);
      Set<Truck>.Serialize(this.m_trucks, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      writer.WriteInt(this.m_vehicleLimitUsedSoFar);
      Set<Vehicle>.Serialize(this.m_vehiclesToDestroy, writer);
      writer.WriteInt(this.MaxVehiclesLimit);
    }

    public static VehiclesManager Deserialize(BlobReader reader)
    {
      VehiclesManager vehiclesManager;
      if (reader.TryStartClassDeserialization<VehiclesManager>(out vehiclesManager))
        reader.EnqueueDataDeserialization((object) vehiclesManager, VehiclesManager.s_deserializeDataDelayedAction);
      return vehiclesManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehiclesManager>(this, "m_allVehicles", (object) Set<Vehicle>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_depots", (object) Lyst<VehicleDepotBase>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_excavators", (object) Set<Excavator>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_harvesters", (object) Set<TreeHarvester>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_onVehicleDespawned", (object) Event<Vehicle>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_planters", (object) Set<TreePlanter>.Deserialize(reader));
      reader.RegisterResolvedMember<VehiclesManager>(this, "m_recoverVehicleJobFactory", typeof (RecoverVehicleJob.Factory), true);
      reader.RegisterResolvedMember<VehiclesManager>(this, "m_scrapOrReplaceVehicleJobFactory", typeof (ScrapOrReplaceVehicleInDepotJob.Factory), true);
      reader.SetField<VehiclesManager>(this, "m_trucks", (object) Set<Truck>.Deserialize(reader));
      reader.SetField<VehiclesManager>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      this.m_vehicleLimitUsedSoFar = reader.ReadInt();
      reader.SetField<VehiclesManager>(this, "m_vehiclesToDestroy", (object) Set<Vehicle>.Deserialize(reader));
      this.MaxVehiclesLimit = reader.ReadInt();
      reader.RegisterInitAfterLoad<VehiclesManager>(this, "initAfterLoad", InitPriority.Low);
    }

    /// <summary>
    /// Register here to get notified when a vehicle is despawned.
    /// </summary>
    public IEvent<Vehicle> OnVehicleDespawned => (IEvent<Vehicle>) this.m_onVehicleDespawned;

    public IReadOnlySet<Vehicle> AllVehicles => (IReadOnlySet<Vehicle>) this.m_allVehicles;

    public IReadOnlySet<Truck> Trucks => (IReadOnlySet<Truck>) this.m_trucks;

    public IReadOnlySet<Excavator> Excavators => (IReadOnlySet<Excavator>) this.m_excavators;

    public IReadOnlySet<TreeHarvester> TreeHarvesters
    {
      get => (IReadOnlySet<TreeHarvester>) this.m_harvesters;
    }

    public IReadOnlySet<TreePlanter> TreePlanters => (IReadOnlySet<TreePlanter>) this.m_planters;

    public int VehiclesLimitLeft => this.MaxVehiclesLimit - this.m_vehicleLimitUsedSoFar;

    public int MaxVehiclesLimit { get; private set; }

    public VehiclesManager(
      ILogisticsConfig logisticsConfig,
      EntitiesManager entitiesManager,
      ISimLoopEvents simLoopEvents,
      ScrapOrReplaceVehicleInDepotJob.Factory scrapOrReplaceVehicleJobFactory,
      RecoverVehicleJob.Factory recoverVehicleJobFactory,
      IUpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onVehicleDespawned = new Event<Vehicle>();
      this.m_allVehicles = new Set<Vehicle>();
      this.m_trucks = new Set<Truck>();
      this.m_excavators = new Set<Excavator>();
      this.m_harvesters = new Set<TreeHarvester>();
      this.m_planters = new Set<TreePlanter>();
      this.m_vehiclesToDestroy = new Set<Vehicle>();
      this.m_depots = new Lyst<VehicleDepotBase>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxVehiclesLimit = logisticsConfig.InitialVehiclesCap.CheckPositive();
      this.m_entitiesManager = entitiesManager;
      this.m_scrapOrReplaceVehicleJobFactory = scrapOrReplaceVehicleJobFactory;
      this.m_recoverVehicleJobFactory = recoverVehicleJobFactory;
      this.m_upointsManager = upointsManager;
      entitiesManager.EntityAdded.Add<VehiclesManager>(this, new Action<IEntity>(this.entityAdded));
      entitiesManager.EntityRemoved.Add<VehiclesManager>(this, new Action<IEntity>(this.entityRemoved));
      simLoopEvents.Update.Add<VehiclesManager>(this, new Action(this.simUpdate));
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initAfterLoad(int saveVersion, DependencyResolver resolver)
    {
      foreach (Vehicle vehicle in this.m_allVehicles.ToArray())
      {
        if (vehicle == null)
        {
          Log.Error(string.Format("Removing null vehicle after load, save version: {0}", (object) saveVersion));
          this.m_allVehicles.Remove((Vehicle) null);
        }
        else if (vehicle.IsDestroyed)
        {
          Log.Error(string.Format("Removing destroyed vehicle after load: {0}, save version: {1}", (object) vehicle, (object) saveVersion));
          this.m_allVehicles.Remove(vehicle);
        }
        else if (!vehicle.IsSpawned && !vehicle.HasJobs)
        {
          VehicleDepotBase closestDepot;
          if (this.TryGetClosestDepotForScrapOrRecovery(vehicle, out closestDepot))
          {
            Log.Error(string.Format("Found non-spawned vehicle without jobs after load: {0},", (object) vehicle) + " has depot, respawning");
            vehicle.Spawn(closestDepot.SpawnDrivePosition, closestDepot.SpawnDirection);
          }
          else
          {
            Log.Error(string.Format("Found non-spawned vehicle without jobs after load: {0},", (object) vehicle) + " no depot, destroying");
            this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) vehicle, EntityRemoveReason.Remove);
            if (this.m_allVehicles.Contains(vehicle))
            {
              Log.Error("Failed to remove non-spawned vehicle. Removing from vehicles manager.");
              this.m_allVehicles.Remove(vehicle);
            }
          }
        }
      }
      if (saveVersion >= 109)
        return;
      int num = 60 + resolver.Resolve<ResearchManager>().AllNodes.Where((Func<ResearchNode, bool>) (x => x.State == ResearchNodeState.Researched && x.Units.Any((Func<IUnlockNodeUnit, bool>) (u => u is VehicleLimitIncreaseUnlock)))).Select<ResearchNode, VehicleLimitIncreaseUnlock>((Func<ResearchNode, VehicleLimitIncreaseUnlock>) (x => x.Units.OfType<VehicleLimitIncreaseUnlock>().First)).Sum<VehicleLimitIncreaseUnlock>((Func<VehicleLimitIncreaseUnlock, int>) (x => x.LimitIncrease));
      if (this.MaxVehiclesLimit >= num)
        return;
      this.MaxVehiclesLimit = num;
    }

    private void simUpdate()
    {
      foreach (IEntity entity in this.m_vehiclesToDestroy)
        this.m_entitiesManager.TryRemoveAndDestroyEntity(entity);
      this.m_vehiclesToDestroy.Clear();
    }

    public void IncreaseVehicleLimit(int diff)
    {
      Assert.That<int>(diff).IsPositive();
      this.MaxVehiclesLimit += diff;
    }

    private void entityAdded(IEntity entity)
    {
      switch (entity)
      {
        case VehicleDepotBase vehicleDepotBase:
          this.m_depots.AddAssertNew(vehicleDepotBase);
          break;
        case Vehicle vehicle:
          this.m_allVehicles.AddAndAssertNew(vehicle);
          this.m_vehicleLimitUsedSoFar += vehicle.Prototype.VehicleQuotaCost;
          if (this.VehiclesLimitLeft < 0)
            Log.Error(string.Format("VehiclesLimitLeft turned negative '{0}'", (object) this.VehiclesLimitLeft));
          switch (entity)
          {
            case Truck truck:
              this.m_trucks.AddAndAssertNew(truck);
              return;
            case Excavator excavator:
              this.m_excavators.AddAndAssertNew(excavator);
              return;
            case TreeHarvester treeHarvester:
              this.m_harvesters.AddAndAssertNew(treeHarvester);
              return;
            case TreePlanter treePlanter:
              this.m_planters.AddAndAssertNew(treePlanter);
              return;
            default:
              return;
          }
      }
    }

    private void entityRemoved(IEntity entity)
    {
      switch (entity)
      {
        case VehicleDepotBase vehicleDepotBase:
          this.m_depots.RemoveAndAssert(vehicleDepotBase);
          break;
        case Vehicle vehicle:
          Assert.That<bool>(vehicle.IsSpawned).IsFalse();
          Assert.That<bool>(vehicle.NeedsJob).IsTrue();
          this.m_allVehicles.RemoveAndAssert(vehicle);
          this.m_vehicleLimitUsedSoFar -= vehicle.Prototype.VehicleQuotaCost;
          if (this.m_vehicleLimitUsedSoFar < 0)
          {
            this.m_vehicleLimitUsedSoFar = 0;
            Log.Error(string.Format("m_vehicleLimitUsedSoFar turned negative '{0}'", (object) this.m_vehicleLimitUsedSoFar));
          }
          switch (entity)
          {
            case Truck truck:
              this.m_trucks.RemoveAndAssert(truck);
              return;
            case Excavator excavator:
              this.m_excavators.RemoveAndAssert(excavator);
              return;
            case TreeHarvester treeHarvester:
              this.m_harvesters.RemoveAndAssert(treeHarvester);
              return;
            case TreePlanter treePlanter:
              this.m_planters.RemoveAndAssert(treePlanter);
              return;
            default:
              return;
          }
      }
    }

    public Option<T> GetFreeVehicle<T>(DynamicEntityProto proto, Tile2f entityPosition) where T : Vehicle
    {
      IEnumerable<T> allEntitiesOfType = this.m_entitiesManager.GetAllEntitiesOfType<T>((Predicate<T>) (x => (Proto) x.Prototype == (Proto) proto && x.CanBeAssigned));
      Fix64 fix64_1 = Fix64.MaxValue;
      Option<T> freeVehicle = Option<T>.None;
      foreach (T obj in allEntitiesOfType)
      {
        Fix64 fix64_2 = obj.Position2f.DistanceSqrTo(entityPosition);
        if (obj.IsNotEnabled)
          fix64_2 += 10000000;
        if (obj is Truck truck && truck.IsNotEmpty)
          fix64_2 += 1000000;
        if (fix64_2 < fix64_1)
        {
          fix64_1 = fix64_2;
          freeVehicle = (Option<T>) obj;
        }
      }
      return freeVehicle;
    }

    public bool CanScrapVehicle(Vehicle vehicle)
    {
      return this.m_depots.Any<VehicleDepotBase>((Predicate<VehicleDepotBase>) (x => x.CanAccept((DynamicGroundEntity) vehicle)));
    }

    public bool CanUpgradeVehicle(DrivingEntityProto currentProto, DrivingEntityProto newProto)
    {
      return this.m_depots.Any<VehicleDepotBase>((Predicate<VehicleDepotBase>) (x => x.CanAcceptForUpgrade(currentProto, newProto)));
    }

    public bool CanRecoverVehicle(Vehicle vehicle, out LocStrFormatted errorReason)
    {
      if (this.m_depots.Count == 0 || this.m_depots.All<VehicleDepotBase>((Func<VehicleDepotBase, bool>) (x => !x.CanAccept((DynamicGroundEntity) vehicle))))
      {
        errorReason = (LocStrFormatted) TrCore.NoVehicleDepotAvailable;
        return false;
      }
      if (!this.m_upointsManager.CanConsume(VehiclesManager.VEHICLE_RECOVERY_COST))
      {
        errorReason = (LocStrFormatted) TrCore.TradeStatus__NoUnity;
        return false;
      }
      errorReason = LocStrFormatted.Empty;
      return true;
    }

    public void VehicleSpawned(Vehicle vehicle)
    {
    }

    public void DestroyVehicle(Vehicle vehicle)
    {
      if (vehicle.IsSpawned)
      {
        vehicle.Despawn();
        this.m_onVehicleDespawned.Invoke(vehicle);
      }
      this.m_vehiclesToDestroy.Add(vehicle);
    }

    public VehicleStats GetStats(DynamicEntityProto proto)
    {
      VehicleStats stats = new VehicleStats();
      foreach (Vehicle allVehicle in this.m_allVehicles)
      {
        if (!((Proto) allVehicle.Prototype != (Proto) proto))
        {
          ++stats.Owned;
          stats.Assignable += allVehicle.CanBeAssigned ? 1 : 0;
        }
      }
      return stats;
    }

    /// <summary>
    /// Assign a job in which the vehicle goes to its depot for despawn. Returns true if succeeded.
    /// </summary>
    public bool TryEnqueueScrapJob(Vehicle vehicle)
    {
      if (!this.CanScrapVehicle(vehicle))
        return false;
      VehicleDepotBase closestDepot;
      if (!this.TryGetClosestDepotForScrapOrRecovery(vehicle, out closestDepot))
      {
        Log.Warning(string.Format("No valid depot found for '{0}'", (object) vehicle));
        return false;
      }
      this.m_scrapOrReplaceVehicleJobFactory.EnqueueJob(vehicle, Option<DrivingEntityProto>.None, closestDepot);
      return true;
    }

    /// <summary>
    /// Assign a job in which the vehicle goes to its depot for replacement. Returns true if succeeded.
    /// </summary>
    public bool TryEnqueueReplaceJob(Vehicle vehicle, DrivingEntityProto newProto)
    {
      if (!this.CanUpgradeVehicle(vehicle.Prototype, newProto))
        return false;
      VehicleDepotBase closestDepot;
      if (!this.TryGetClosestDepotForReplacement(vehicle, newProto, out closestDepot))
      {
        Log.Warning(string.Format("No valid depot found for '{0}'", (object) vehicle));
        return false;
      }
      this.m_scrapOrReplaceVehicleJobFactory.EnqueueJob(vehicle, (Option<DrivingEntityProto>) newProto, closestDepot);
      return true;
    }

    public bool TryEnqueueRecoveryJob(Vehicle vehicle)
    {
      if (!this.CanRecoverVehicle(vehicle, out LocStrFormatted _))
        return false;
      VehicleDepotBase closestDepot;
      if (!this.TryGetClosestDepotForScrapOrRecovery(vehicle, out closestDepot))
      {
        Log.Warning(string.Format("No valid depot found for '{0}'", (object) vehicle));
        return false;
      }
      if (!this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.VehicleRecovery, VehiclesManager.VEHICLE_RECOVERY_COST))
        return false;
      this.m_recoverVehicleJobFactory.EnqueueJob(vehicle, closestDepot);
      return true;
    }

    public bool TryGetClosestDepotForScrapOrRecovery(
      Vehicle vehicle,
      out VehicleDepotBase closestDepot)
    {
      Fix64 fix64 = Fix64.MaxValue;
      closestDepot = (VehicleDepotBase) null;
      foreach (VehicleDepotBase depot in this.m_depots)
      {
        if (depot.CanAccept((DynamicGroundEntity) vehicle))
        {
          Fix64 lengthSqr = (depot.Transform.Position.Xy.CenterTile2f - vehicle.Position2f).LengthSqr;
          if (lengthSqr < fix64)
          {
            fix64 = lengthSqr;
            closestDepot = depot;
          }
        }
      }
      return closestDepot != null;
    }

    public bool TryGetClosestDepotForReplacement(
      Vehicle vehicle,
      DrivingEntityProto newProto,
      out VehicleDepotBase closestDepot)
    {
      Fix64 fix64 = Fix64.MaxValue;
      closestDepot = (VehicleDepotBase) null;
      foreach (VehicleDepotBase depot in this.m_depots)
      {
        if (depot.CanAcceptForUpgrade(vehicle.Prototype, newProto))
        {
          Fix64 lengthSqr = (depot.Transform.Position.Xy.CenterTile2f - vehicle.Position2f).LengthSqr;
          if (lengthSqr < fix64)
          {
            fix64 = lengthSqr;
            closestDepot = depot;
          }
        }
      }
      return closestDepot != null;
    }

    public void TeleportVehicleToAnyValidPosition(Vehicle vehicle)
    {
      VehicleDepotBase closestDepot;
      if (this.TryGetClosestDepotForScrapOrRecovery(vehicle, out closestDepot))
        vehicle.TeleportTo(closestDepot.SpawnDrivePosition, new AngleDegrees1f?());
      else
        Log.Error(string.Format("Failed to teleport vehicle '{0}' to the closest depot.", (object) vehicle));
    }

    static VehiclesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehiclesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehiclesManager) obj).SerializeData(writer));
      VehiclesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehiclesManager) obj).DeserializeData(reader));
      VehiclesManager.VEHICLE_RECOVERY_COST = 0.2.Upoints();
    }
  }
}
