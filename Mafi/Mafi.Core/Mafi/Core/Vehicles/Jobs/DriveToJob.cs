// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.DriveToJob
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
  [GenerateSerializer(false, null, 0)]
  internal sealed class DriveToJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Vehicle m_vehicle;
    private readonly Tile2f m_goal;
    private readonly bool m_targetIsTerminal;
    private readonly RelTile1f m_finishedThreshold;
    private bool m_driveStarted;
    private readonly bool m_isTrueJob;
    private readonly Duration m_cancelDeadline;
    private readonly DriveToJob.Factory m_factory;

    public static void Serialize(DriveToJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DriveToJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DriveToJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Duration.Serialize(this.m_cancelDeadline, writer);
      writer.WriteBool(this.m_driveStarted);
      RelTile1f.Serialize(this.m_finishedThreshold, writer);
      Tile2f.Serialize(this.m_goal, writer);
      writer.WriteBool(this.m_isTrueJob);
      writer.WriteBool(this.m_targetIsTerminal);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static DriveToJob Deserialize(BlobReader reader)
    {
      DriveToJob driveToJob;
      if (reader.TryStartClassDeserialization<DriveToJob>(out driveToJob))
        reader.EnqueueDataDeserialization((object) driveToJob, DriveToJob.s_deserializeDataDelayedAction);
      return driveToJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<DriveToJob>(this, "m_cancelDeadline", (object) Duration.Deserialize(reader));
      this.m_driveStarted = reader.ReadBool();
      reader.RegisterResolvedMember<DriveToJob>(this, "m_factory", typeof (DriveToJob.Factory), true);
      reader.SetField<DriveToJob>(this, "m_finishedThreshold", (object) RelTile1f.Deserialize(reader));
      reader.SetField<DriveToJob>(this, "m_goal", (object) Tile2f.Deserialize(reader));
      reader.SetField<DriveToJob>(this, "m_isTrueJob", (object) reader.ReadBool());
      reader.SetField<DriveToJob>(this, "m_targetIsTerminal", (object) reader.ReadBool());
      reader.SetField<DriveToJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
    }

    public override bool IsTrueJob => this.m_isTrueJob;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public override LocStrFormatted JobInfo => new LocStrFormatted("Driving to destination");

    internal DriveToJob(
      VehicleJobId id,
      DriveToJob.Factory factory,
      Vehicle entity,
      Tile2f goal,
      bool targetIsTerminal,
      RelTile1f finishedThreshold,
      bool isTrueJob,
      Duration? cancelDeadline)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory.CheckNotNull<DriveToJob.Factory>();
      this.m_vehicle = entity.CheckNotNull<Vehicle>();
      this.m_goal = goal;
      this.m_targetIsTerminal = targetIsTerminal;
      this.m_finishedThreshold = finishedThreshold;
      this.m_isTrueJob = isTrueJob;
      this.m_cancelDeadline = cancelDeadline ?? Duration.Zero;
      this.m_driveStarted = false;
    }

    protected override bool DoJobInternal()
    {
      if (this.m_vehicle.DistanceSqrTo(this.m_goal) <= this.m_finishedThreshold.Value.Squared())
      {
        this.m_vehicle.StopDriving();
        return false;
      }
      if (this.m_driveStarted)
        return this.m_vehicle.IsDriving;
      this.m_driveStarted = true;
      this.m_vehicle.SetDrivingTarget(this.m_goal, this.m_targetIsTerminal);
      return true;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_vehicle.StopDriving();
      ((IVehicleFriend) this.m_vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return this.m_cancelDeadline;
    }

    protected override void OnDestroy()
    {
    }

    static DriveToJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DriveToJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      DriveToJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
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

      /// <summary>
      /// Creates and returns a job for driving to a given goal. If <paramref name="targetIsTerminal" /> is set then
      /// vehicle will smoothly stop at given goal. Otherwise it will go full speed until it reaches goal or gets
      /// closer than <paramref name="finishedThreshold" />.
      /// </summary>
      public VehicleJob CreateJob(
        Vehicle vehicle,
        Tile2f goal,
        bool targetIsTerminal,
        RelTile1f finishedThreshold,
        bool isTrueJob = true,
        Duration? cancelDeadline = null)
      {
        return (VehicleJob) new DriveToJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, goal, targetIsTerminal, finishedThreshold, isTrueJob, cancelDeadline);
      }
    }
  }
}
