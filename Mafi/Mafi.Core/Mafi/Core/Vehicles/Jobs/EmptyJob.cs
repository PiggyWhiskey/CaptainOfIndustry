// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.EmptyJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>Empty job, mainly for testing.</summary>
  /// <remarks>This class cannot live in the test project because it needs generated serializer.</remarks>
  [GenerateSerializer(false, null, 0)]
  public sealed class EmptyJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly Vehicle m_vehicle;
    private Duration m_jobDuration;
    private readonly bool m_isTrueJob;

    public static void Serialize(EmptyJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EmptyJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EmptyJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.m_isTrueJob);
      Duration.Serialize(this.m_jobDuration, writer);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static EmptyJob Deserialize(BlobReader reader)
    {
      EmptyJob emptyJob;
      if (reader.TryStartClassDeserialization<EmptyJob>(out emptyJob))
        reader.EnqueueDataDeserialization((object) emptyJob, EmptyJob.s_deserializeDataDelayedAction);
      return emptyJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<EmptyJob>(this, "m_isTrueJob", (object) reader.ReadBool());
      this.m_jobDuration = Duration.Deserialize(reader);
      reader.SetField<EmptyJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
    }

    public override LocStrFormatted JobInfo => EmptyJob.s_jobInfo;

    public override bool IsTrueJob => this.m_isTrueJob;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.None;

    public override bool SkipNoMovementMonitoring => true;

    public EmptyJob(VehicleJobId id, Vehicle vehicle, Duration jobDuration, bool isTrueJob)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_jobDuration = jobDuration;
      this.m_isTrueJob = isTrueJob;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      Assert.That<Vehicle>(this.m_vehicle).IsNotNull<Vehicle>();
      ((IVehicleFriend) this.m_vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override bool DoJobInternal()
    {
      Assert.That<Vehicle>(this.m_vehicle).IsNotNull<Vehicle>();
      this.m_jobDuration -= Duration.OneTick;
      return this.m_jobDuration > Duration.Zero;
    }

    protected override void OnDestroy()
    {
    }

    static EmptyJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EmptyJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      EmptyJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      EmptyJob.s_jobInfo = new LocStrFormatted("Empty job.");
    }
  }
}
