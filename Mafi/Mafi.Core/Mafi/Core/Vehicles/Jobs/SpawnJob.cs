// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.SpawnJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>Spawns vehicle and drives it out of the depot.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class SpawnJob : 
    VehicleJob,
    IDepotJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly Vehicle m_vehicle;
    private readonly VehicleDepotBase m_depot;
    private bool m_startedDriveFromDepot;
    private bool m_isPending;
    private readonly SpawnJob.Factory m_factory;

    public static void Serialize(SpawnJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SpawnJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SpawnJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<VehicleDepotBase>(this.m_depot);
      writer.WriteBool(this.m_isPending);
      writer.WriteBool(this.m_startedDriveFromDepot);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static SpawnJob Deserialize(BlobReader reader)
    {
      SpawnJob spawnJob;
      if (reader.TryStartClassDeserialization<SpawnJob>(out spawnJob))
        reader.EnqueueDataDeserialization((object) spawnJob, SpawnJob.s_deserializeDataDelayedAction);
      return spawnJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SpawnJob>(this, "m_depot", (object) reader.ReadGenericAs<VehicleDepotBase>());
      reader.RegisterResolvedMember<SpawnJob>(this, "m_factory", typeof (SpawnJob.Factory), true);
      this.m_isPending = reader.ReadBool();
      this.m_startedDriveFromDepot = reader.ReadBool();
      reader.SetField<SpawnJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
    }

    public override LocStrFormatted JobInfo => SpawnJob.s_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.None;

    public override bool SkipNoMovementMonitoring => true;

    private SpawnJob(
      VehicleJobId id,
      SpawnJob.Factory factory,
      Vehicle vehicle,
      VehicleDepotBase depot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      Assert.That<bool>(vehicle.IsSpawned).IsFalse();
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_depot = depot.CheckNotNull<VehicleDepotBase>();
      this.m_startedDriveFromDepot = false;
      this.m_isPending = true;
      this.m_vehicle.EnqueueJob((VehicleJob) this, true);
      this.m_depot.AddJob((IDepotJob) this);
    }

    protected override bool DoJobInternal()
    {
      if (!this.m_vehicle.IsSpawned)
      {
        if (!this.m_depot.TrySpawnVehicle())
          return true;
        this.m_vehicle.Spawn(this.m_depot.SpawnPosition, this.m_depot.SpawnDirection);
      }
      if (this.m_depot.IsConstructed && !this.m_depot.WaitForOpenDoors())
        return true;
      if (!this.m_startedDriveFromDepot)
      {
        this.m_vehicle.SetDrivingTarget(this.m_depot.SpawnDrivePosition, true);
        this.m_startedDriveFromDepot = true;
        return true;
      }
      if (this.m_vehicle.IsDriving)
        return true;
      this.m_isPending = false;
      this.m_depot.JobDone((IDepotJob) this);
      this.m_factory.VehicleRecoveryManager.OnVehicleSpawned(this.m_vehicle);
      return false;
    }

    protected override Duration RequestCancelReturnDeadline() => 1.Minutes();

    protected override void OnDestroy()
    {
      if (!this.m_isPending)
        return;
      if (!this.m_vehicle.IsSpawned)
      {
        Log.Error("Cancelling spawn job that did not even spawn the vehicle.");
        this.m_vehicle.Spawn(this.m_depot.SpawnPosition, this.m_depot.SpawnDirection);
      }
      this.m_isPending = false;
      this.m_depot.JobCanceled((IDepotJob) this);
    }

    static SpawnJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SpawnJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      SpawnJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      SpawnJob.s_jobInfo = new LocStrFormatted("Driving from vehicle depot.");
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      internal readonly VehicleRecoveryManager VehicleRecoveryManager;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        VehicleRecoveryManager vehicleRecoveryManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.VehicleRecoveryManager = vehicleRecoveryManager;
      }

      public void EnqueueFirstJob(Vehicle vehicle, VehicleDepotBase depot)
      {
        SpawnJob spawnJob = new SpawnJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, depot);
      }
    }
  }
}
