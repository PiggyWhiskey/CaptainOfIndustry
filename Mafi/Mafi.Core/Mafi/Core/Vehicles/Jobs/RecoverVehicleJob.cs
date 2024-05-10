// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.RecoverVehicleJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain.Designation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public sealed class RecoverVehicleJob : 
    VehicleJob,
    IDepotJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration RECOVERY_DURATION;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly RecoverVehicleJob.Factory m_factory;
    private readonly Vehicle m_vehicle;
    private readonly VehicleDepotBase m_depot;
    private Duration m_waitDuration;

    public static void Serialize(RecoverVehicleJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RecoverVehicleJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RecoverVehicleJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<VehicleDepotBase>(this.m_depot);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
      Duration.Serialize(this.m_waitDuration, writer);
    }

    public static RecoverVehicleJob Deserialize(BlobReader reader)
    {
      RecoverVehicleJob recoverVehicleJob;
      if (reader.TryStartClassDeserialization<RecoverVehicleJob>(out recoverVehicleJob))
        reader.EnqueueDataDeserialization((object) recoverVehicleJob, RecoverVehicleJob.s_deserializeDataDelayedAction);
      return recoverVehicleJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RecoverVehicleJob>(this, "m_depot", (object) reader.ReadGenericAs<VehicleDepotBase>());
      reader.RegisterResolvedMember<RecoverVehicleJob>(this, "m_factory", typeof (RecoverVehicleJob.Factory), true);
      reader.SetField<RecoverVehicleJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
      this.m_waitDuration = Duration.Deserialize(reader);
    }

    public override LocStrFormatted JobInfo => RecoverVehicleJob.s_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.None;

    public override bool SkipNoMovementMonitoring => this.m_waitDuration.IsPositive;

    public RecoverVehicleJob(
      VehicleJobId id,
      RecoverVehicleJob.Factory factory,
      Vehicle vehicle,
      VehicleDepotBase depot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_depot = depot.CheckNotNull<VehicleDepotBase>();
      this.m_factory.VehicleRecoveryManager.AddVehicleToRecover(vehicle);
      this.m_vehicle.EnqueueJob((VehicleJob) this);
      this.m_depot.AddJob((IDepotJob) this);
      this.m_waitDuration = RecoverVehicleJob.RECOVERY_DURATION;
    }

    protected override bool DoJobInternal()
    {
      Assert.That<bool>(this.m_vehicle.IsSpawned).IsTrue();
      Assert.That<bool>(this.m_depot.IsDestroyed).IsFalse();
      this.m_waitDuration -= Duration.OneTick;
      if (this.m_waitDuration.IsPositive)
        return true;
      if (this.m_vehicle.UnreachableGoal.HasValue)
        this.m_vehicle.ClearUnreachableGoal();
      this.m_factory.UnreachablesManager.OnVehicleRecovered((IPathFindingVehicle) this.m_vehicle);
      this.m_vehicle.Despawn();
      this.m_factory.FuelStationsManager.Value.OnRecoveryPerformed(this.m_vehicle);
      this.m_factory.SpawnJobFactory.EnqueueFirstJob(this.m_vehicle, this.m_depot);
      this.m_depot.JobDone((IDepotJob) this);
      return false;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_factory.VehicleRecoveryManager.AbortRecovery(this.m_vehicle);
      this.m_depot.JobCanceled((IDepotJob) this);
      ((IVehicleFriend) this.m_vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
    }

    static RecoverVehicleJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RecoverVehicleJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      RecoverVehicleJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      RecoverVehicleJob.RECOVERY_DURATION = 1.Seconds();
      RecoverVehicleJob.s_jobInfo = new LocStrFormatted("Recovering vehicle.");
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      internal readonly SpawnJob.Factory SpawnJobFactory;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      internal readonly LazyResolve<IFuelStationsManager> FuelStationsManager;
      internal readonly VehicleRecoveryManager VehicleRecoveryManager;
      internal readonly UnreachableTerrainDesignationsManager UnreachablesManager;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        LazyResolve<IFuelStationsManager> fuelStationsManager,
        VehicleRecoveryManager vehicleRecoveryManager,
        UnreachableTerrainDesignationsManager unreachablesManager,
        SpawnJob.Factory spawnJobFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.FuelStationsManager = fuelStationsManager;
        this.UnreachablesManager = unreachablesManager;
        this.VehicleRecoveryManager = vehicleRecoveryManager;
        this.SpawnJobFactory = spawnJobFactory;
      }

      public void EnqueueJob(Vehicle vehicle, VehicleDepotBase depot)
      {
        RecoverVehicleJob recoverVehicleJob = new RecoverVehicleJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, depot);
      }
    }
  }
}
