// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.GetUnstuckJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public sealed class GetUnstuckJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration STUCK_RETRY_DELAY;
    private readonly Vehicle m_vehicle;
    private readonly IVehiclesManager m_vehiclesManager;
    private bool m_drivingStarted;
    private readonly TickTimer m_stuckTimer;
    private readonly GetUnstuckJob.Factory m_factory;

    public static void Serialize(GetUnstuckJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GetUnstuckJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GetUnstuckJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.m_drivingStarted);
      TickTimer.Serialize(this.m_stuckTimer, writer);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
    }

    public static GetUnstuckJob Deserialize(BlobReader reader)
    {
      GetUnstuckJob getUnstuckJob;
      if (reader.TryStartClassDeserialization<GetUnstuckJob>(out getUnstuckJob))
        reader.EnqueueDataDeserialization((object) getUnstuckJob, GetUnstuckJob.s_deserializeDataDelayedAction);
      return getUnstuckJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_drivingStarted = reader.ReadBool();
      reader.RegisterResolvedMember<GetUnstuckJob>(this, "m_factory", typeof (GetUnstuckJob.Factory), true);
      reader.SetField<GetUnstuckJob>(this, "m_stuckTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<GetUnstuckJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
      reader.SetField<GetUnstuckJob>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
    }

    public override LocStrFormatted JobInfo => new LocStrFormatted("Getting unstuck.");

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    internal GetUnstuckJob(
      VehicleJobId id,
      GetUnstuckJob.Factory factory,
      IVehiclesManager vehiclesManager,
      Vehicle vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_stuckTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_vehiclesManager = vehiclesManager;
      this.m_factory = factory.CheckNotNull<GetUnstuckJob.Factory>();
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_drivingStarted = false;
      vehicle.EnqueueJob((VehicleJob) this, true);
    }

    protected override bool DoJobInternal()
    {
      if (!this.m_drivingStarted)
      {
        this.m_drivingStarted = true;
        if (!this.m_vehicle.IsStuck)
          this.m_vehicle.DriveToValidLocation();
        Assert.That<bool>(this.m_vehicle.IsStuck).IsTrue();
      }
      return this.m_vehicle.IsStuck || this.m_vehicle.IsDriving;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_vehicle.StopNavigating();
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
    }

    static GetUnstuckJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GetUnstuckJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      GetUnstuckJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      GetUnstuckJob.STUCK_RETRY_DELAY = 5.Seconds();
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      private readonly IVehiclesManager m_vehiclesManager;

      public Factory(VehicleJobId.Factory vehicleJobIdFactory, IVehiclesManager vehiclesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.m_vehiclesManager = vehiclesManager;
      }

      public void EnqueueJob(Vehicle vehicle)
      {
        GetUnstuckJob getUnstuckJob = new GetUnstuckJob(this.m_vehicleJobIdFactory.GetNextId(), this, this.m_vehiclesManager, vehicle);
      }
    }
  }
}
