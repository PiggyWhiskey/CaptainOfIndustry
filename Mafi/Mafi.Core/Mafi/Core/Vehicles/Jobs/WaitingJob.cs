// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.WaitingJob
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
  /// <summary>Job for simple waiting.</summary>
  [GenerateSerializer(false, null, 0)]
  public class WaitingJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly TickTimer m_timer;
    private readonly Vehicle m_vehicle;
    private readonly WaitingJob.Factory m_factory;

    public static void Serialize(WaitingJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitingJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitingJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TickTimer.Serialize(this.m_timer, writer);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static WaitingJob Deserialize(BlobReader reader)
    {
      WaitingJob waitingJob;
      if (reader.TryStartClassDeserialization<WaitingJob>(out waitingJob))
        reader.EnqueueDataDeserialization((object) waitingJob, WaitingJob.s_deserializeDataDelayedAction);
      return waitingJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterResolvedMember<WaitingJob>(this, "m_factory", typeof (WaitingJob.Factory), true);
      reader.SetField<WaitingJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<WaitingJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
    }

    public override LocStrFormatted JobInfo => WaitingJob.s_jobInfo;

    public override bool IsTrueJob => false;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Idle;

    public override bool SkipNoMovementMonitoring => true;

    public WaitingJob(
      VehicleJobId id,
      WaitingJob.Factory factory,
      Vehicle vehicle,
      Duration duration)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_vehicle = vehicle;
      this.m_timer.Start(duration);
    }

    protected override bool DoJobInternal() => this.m_timer.Decrement();

    protected override Duration RequestCancelReturnDeadline() => Duration.Zero;

    protected override void OnDestroy() => this.m_timer.Reset();

    static WaitingJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitingJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      WaitingJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      WaitingJob.s_jobInfo = new LocStrFormatted("Waiting.");
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;

      public Factory(VehicleJobId.Factory vehicleJobIdFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
      }

      public void EnqueueJob(Vehicle vehicle, Duration duration)
      {
        WaitingJob job = new WaitingJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, duration);
        vehicle.EnqueueJob((VehicleJob) job);
      }
    }
  }
}
