// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.AttachRocketToLaunchPadJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public sealed class AttachRocketToLaunchPadJob : 
    VehicleJob,
    IQueueTipJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private AttachRocketToLaunchPadJob.State m_state;
    private readonly RocketTransporter m_rocketTransporter;
    private readonly RocketLaunchPad m_rocketLaunchPad;
    private LocStrFormatted m_jobInfo;
    private readonly AttachRocketToLaunchPadJob.Factory m_factory;
    private AngleDegrees1f m_previousDirection;
    private AngleDegrees1f m_cumulativeRotation;
    private readonly bool m_isDeliveringRocket;

    public static void Serialize(AttachRocketToLaunchPadJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AttachRocketToLaunchPadJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AttachRocketToLaunchPadJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AngleDegrees1f.Serialize(this.m_cumulativeRotation, writer);
      writer.WriteBool(this.m_isDeliveringRocket);
      LocStrFormatted.Serialize(this.m_jobInfo, writer);
      AngleDegrees1f.Serialize(this.m_previousDirection, writer);
      RocketLaunchPad.Serialize(this.m_rocketLaunchPad, writer);
      RocketTransporter.Serialize(this.m_rocketTransporter, writer);
      writer.WriteInt((int) this.m_state);
    }

    public static AttachRocketToLaunchPadJob Deserialize(BlobReader reader)
    {
      AttachRocketToLaunchPadJob rocketToLaunchPadJob;
      if (reader.TryStartClassDeserialization<AttachRocketToLaunchPadJob>(out rocketToLaunchPadJob))
        reader.EnqueueDataDeserialization((object) rocketToLaunchPadJob, AttachRocketToLaunchPadJob.s_deserializeDataDelayedAction);
      return rocketToLaunchPadJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_cumulativeRotation = AngleDegrees1f.Deserialize(reader);
      reader.RegisterResolvedMember<AttachRocketToLaunchPadJob>(this, "m_factory", typeof (AttachRocketToLaunchPadJob.Factory), true);
      reader.SetField<AttachRocketToLaunchPadJob>(this, "m_isDeliveringRocket", (object) reader.ReadBool());
      this.m_jobInfo = LocStrFormatted.Deserialize(reader);
      this.m_previousDirection = AngleDegrees1f.Deserialize(reader);
      reader.SetField<AttachRocketToLaunchPadJob>(this, "m_rocketLaunchPad", (object) RocketLaunchPad.Deserialize(reader));
      reader.SetField<AttachRocketToLaunchPadJob>(this, "m_rocketTransporter", (object) RocketTransporter.Deserialize(reader));
      this.m_state = (AttachRocketToLaunchPadJob.State) reader.ReadInt();
    }

    public override LocStrFormatted JobInfo => this.m_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public bool WaitBehindQueueTipVehicle
    {
      get => this.m_state == AttachRocketToLaunchPadJob.State.WaitingForLaunchPad;
    }

    public override bool SkipNoMovementMonitoring
    {
      get => this.m_state == AttachRocketToLaunchPadJob.State.WaitingForLaunchPad;
    }

    private AttachRocketToLaunchPadJob(
      VehicleJobId id,
      AttachRocketToLaunchPadJob.Factory factory,
      RocketTransporter rocketTransporter,
      RocketLaunchPad rocketLaunchPad)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      Assert.That<bool>(rocketTransporter.DrivingData.CanTurnInPlace).IsTrue("Rocket transporter should be able to turn in place.");
      this.m_rocketTransporter = rocketTransporter.CheckNotNull<RocketTransporter>();
      this.m_rocketLaunchPad = rocketLaunchPad.CheckNotNull<RocketLaunchPad>();
      this.m_state = AttachRocketToLaunchPadJob.State.WaitingForLaunchPad;
      this.m_isDeliveringRocket = rocketTransporter.AttachedRocketBase.ValueOrNull is RocketEntity;
      if (this.m_isDeliveringRocket)
      {
        this.m_jobInfo = new LocStrFormatted("Will attach rocket (not started).");
        VehicleQueue<Vehicle, IStaticEntity> queue;
        if (rocketLaunchPad.TryGetVehicleQueueFor((Vehicle) rocketTransporter, out queue))
        {
          VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.QueueingJobFactory.CreateJobForStaticOwnedQueue<IStaticEntity, Vehicle>((Vehicle) rocketTransporter, queue, useCustomTarget: true);
          staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
          rocketTransporter.EnqueueJob((VehicleJob) staticOwnedQueue);
        }
        else
        {
          Assert.Fail("Failed to queue rocket transporter.");
          this.m_factory.NavigateToJobFactory.EnqueueJob((Vehicle) rocketTransporter, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create((IStaticEntity) rocketLaunchPad, true));
        }
      }
      else
      {
        this.m_jobInfo = new LocStrFormatted("Will deliver module (not started).");
        this.m_factory.NavigateToJobFactory.EnqueueJob((Vehicle) rocketTransporter, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create((IStaticEntity) rocketLaunchPad));
      }
      rocketTransporter.EnqueueJob((VehicleJob) this);
    }

    protected override bool DoJobInternal()
    {
      Assert.That<RocketTransporter>(this.m_rocketTransporter).IsNotNull<RocketTransporter>("DoJob on non-initialized instance.");
      AttachRocketToLaunchPadJob.State state;
      if (!this.m_isDeliveringRocket)
      {
        if (!this.m_rocketTransporter.TryTransferRocketTo((IRocketOwner) this.m_rocketLaunchPad))
          return true;
        Assert.That<Option<RocketEntityBase>>(this.m_rocketTransporter.AttachedRocketBase).IsNone<RocketEntityBase>();
        state = AttachRocketToLaunchPadJob.State.Done;
      }
      else
      {
        switch (this.m_state)
        {
          case AttachRocketToLaunchPadJob.State.WaitingForLaunchPad:
            this.m_previousDirection = this.m_rocketTransporter.Direction;
            state = this.handleWaitingForLaunchPad();
            break;
          case AttachRocketToLaunchPadJob.State.DrivingInFrontOfThePad:
            state = this.handleDrivingInFrontOfThePad();
            break;
          case AttachRocketToLaunchPadJob.State.AligningTowardsPad:
            state = this.handleAligningTowardsPad();
            break;
          case AttachRocketToLaunchPadJob.State.DrivingToAttachmentPoint:
            state = this.handleDrivingToAttachmentPoint();
            break;
          case AttachRocketToLaunchPadJob.State.AligningAtAttachmentPoint:
            state = this.handleAligningAtAttachmentPoint();
            break;
          case AttachRocketToLaunchPadJob.State.AttachingRocket:
            state = this.handleAttachingRocket();
            break;
          case AttachRocketToLaunchPadJob.State.DrivingBack:
            state = this.handleDrivingBack();
            break;
          case AttachRocketToLaunchPadJob.State.DrivingToExit:
            state = this.handleDrivingToExit();
            break;
          default:
            Log.Error(string.Format("Invalid state: {0}", (object) this.m_state));
            state = AttachRocketToLaunchPadJob.State.Done;
            break;
        }
        this.m_cumulativeRotation += this.m_previousDirection.AngleTo(this.m_rocketTransporter.Direction).Abs;
        this.m_previousDirection = this.m_rocketTransporter.Direction;
      }
      if (this.m_state == state)
        return true;
      this.m_state = state;
      this.m_jobInfo = new LocStrFormatted(string.Format("State: {0}", (object) state));
      if (state != AttachRocketToLaunchPadJob.State.Done)
        return true;
      this.InvokeJobDone();
      return false;
    }

    private AttachRocketToLaunchPadJob.State handleWaitingForLaunchPad()
    {
      if (!this.m_rocketTransporter.AttachedRocketBase.HasValue)
      {
        Assert.Fail("No rocket");
        return AttachRocketToLaunchPadJob.State.Done;
      }
      if (!this.m_rocketLaunchPad.TryAcceptNewRocketFrom(this.m_rocketTransporter))
        return AttachRocketToLaunchPadJob.State.WaitingForLaunchPad;
      this.m_rocketTransporter.SetDrivingTarget(this.m_rocketLaunchPad.RocketTransporterAlignGoal, true);
      return AttachRocketToLaunchPadJob.State.DrivingInFrontOfThePad;
    }

    private AttachRocketToLaunchPadJob.State handleDrivingInFrontOfThePad()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.DrivingInFrontOfThePad;
      Assert.That<Tile2f>(this.m_rocketTransporter.Position2f).IsNear(this.m_rocketLaunchPad.RocketTransporterAlignGoal, DrivingEntity.DEFAULT_DRIVING_TOLERANCE.Value);
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(210.Degrees(), "Unexpected amount of rotation during 'DrivingInFrontOfThePad' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      this.m_rocketTransporter.SetTurningTarget(this.m_rocketLaunchPad.Transform.Rotation.Angle);
      this.m_rocketLaunchPad.SetCrawlerBridgeErected(true);
      return AttachRocketToLaunchPadJob.State.AligningTowardsPad;
    }

    private AttachRocketToLaunchPadJob.State handleAligningTowardsPad()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.AligningTowardsPad;
      Assert.That<AngleDegrees1f>(this.m_rocketTransporter.Direction).IsNear(this.m_rocketLaunchPad.Transform.Rotation.Angle, 1.Degrees(), "Incorrect angle during 'AligningTowardsPad' stage.");
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(180.Degrees(), "Unexpected amount of rotation during 'AligningTowardsPad' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      this.m_rocketTransporter.SetDrivingTarget(this.m_rocketLaunchPad.RocketAnchor, true);
      this.m_rocketTransporter.SetForceFlatGround(true);
      return AttachRocketToLaunchPadJob.State.DrivingToAttachmentPoint;
    }

    private AttachRocketToLaunchPadJob.State handleDrivingToAttachmentPoint()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.DrivingToAttachmentPoint;
      Assert.That<Tile2f>(this.m_rocketTransporter.Position2f).IsNear(this.m_rocketLaunchPad.RocketAnchor, DrivingEntity.DEFAULT_DRIVING_TOLERANCE.Value);
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(10.Degrees(), "Unexpected amount of rotation during 'DrivingToAttachmentPoint' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      this.m_rocketTransporter.SetTurningTarget(this.m_rocketLaunchPad.Transform.Rotation.Angle);
      return AttachRocketToLaunchPadJob.State.AligningAtAttachmentPoint;
    }

    private AttachRocketToLaunchPadJob.State handleAligningAtAttachmentPoint()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.AligningAtAttachmentPoint;
      Assert.That<AngleDegrees1f>(this.m_rocketTransporter.Direction).IsNear(this.m_rocketLaunchPad.Transform.Rotation.Angle, 1.Degrees(), "Incorrect angle during  'AligningAtAttachmentPoint' stage.");
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(10.Degrees(), "Unexpected amount of rotation during 'AligningAtAttachmentPoint' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      if (this.m_rocketTransporter.AttachedRocketBase.IsNone)
      {
        Log.Error("No rocket!");
        return AttachRocketToLaunchPadJob.State.DrivingBack;
      }
      if (!this.m_rocketTransporter.TryTransferRocketTo((IRocketOwner) this.m_rocketLaunchPad))
        return AttachRocketToLaunchPadJob.State.DrivingBack;
      Assert.That<Option<RocketEntity>>(this.m_rocketLaunchPad.AttachedRocket).HasValue<RocketEntity>();
      Assert.That<Option<RocketEntityBase>>(this.m_rocketTransporter.AttachedRocketBase).IsNone<RocketEntityBase>();
      Assert.That<RocketLaunchPadState>(this.m_rocketLaunchPad.State).IsEqualTo<RocketLaunchPadState>(RocketLaunchPadState.AttachingRocket);
      return AttachRocketToLaunchPadJob.State.AttachingRocket;
    }

    private AttachRocketToLaunchPadJob.State handleAttachingRocket()
    {
      if (this.m_rocketLaunchPad.State == RocketLaunchPadState.AttachingRocket)
        return AttachRocketToLaunchPadJob.State.AttachingRocket;
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsZero("Unexpected amount of rotation during 'AttachingRocket' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      this.m_rocketTransporter.SetDrivingTarget(this.m_rocketLaunchPad.RocketTransporterAlignGoal, true, driveBackwards: true);
      this.m_rocketTransporter.SetRocketHolderState(false);
      return AttachRocketToLaunchPadJob.State.DrivingBack;
    }

    private AttachRocketToLaunchPadJob.State handleDrivingBack()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.DrivingBack;
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(10.Degrees(), "Unexpected amount of rotation during 'DrivingBack' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      Assert.That<Tile2f>(this.m_rocketTransporter.Position2f).IsNear(this.m_rocketLaunchPad.RocketTransporterAlignGoal, DrivingEntity.DEFAULT_DRIVING_TOLERANCE.Value);
      this.m_rocketTransporter.SetDrivingTarget(this.m_rocketLaunchPad.RocketTransporterExitGoal, true);
      this.m_rocketTransporter.SetForceFlatGround(false);
      this.m_rocketLaunchPad.SetCrawlerBridgeErected(false);
      return AttachRocketToLaunchPadJob.State.DrivingToExit;
    }

    private AttachRocketToLaunchPadJob.State handleDrivingToExit()
    {
      if (this.m_rocketTransporter.IsDriving || this.m_rocketTransporter.IsMoving)
        return AttachRocketToLaunchPadJob.State.DrivingToExit;
      Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(180.Degrees(), "Unexpected amount of rotation during 'DrivingToExit' stage.");
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
      Assert.That<Tile2f>(this.m_rocketTransporter.Position2f).IsNear(this.m_rocketLaunchPad.RocketTransporterExitGoal, DrivingEntity.DEFAULT_DRIVING_TOLERANCE.Value);
      return AttachRocketToLaunchPadJob.State.Done;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      Assert.That<RocketTransporter>(this.m_rocketTransporter).IsNotNull<RocketTransporter>("CancelJob on non-initialized instance.");
      if (this.m_state != AttachRocketToLaunchPadJob.State.WaitingForLaunchPad)
        return 2.Minutes();
      this.InvokeJobDone();
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.m_rocketTransporter?.SetForceFlatGround(false);
      this.m_previousDirection = AngleDegrees1f.Zero;
      this.m_cumulativeRotation = AngleDegrees1f.Zero;
    }

    static AttachRocketToLaunchPadJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AttachRocketToLaunchPadJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      AttachRocketToLaunchPadJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    private enum State
    {
      WaitingForLaunchPad,
      DrivingInFrontOfThePad,
      AligningTowardsPad,
      DrivingToAttachmentPoint,
      AligningAtAttachmentPoint,
      AttachingRocket,
      DrivingBack,
      DrivingToExit,
      Done,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      internal readonly NavigateToJob.Factory NavigateToJobFactory;
      internal readonly VehicleQueueJobFactory QueueingJobFactory;
      internal readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        NavigateToJob.Factory navigateToJobFactory,
        VehicleQueueJobFactory queueingJobFactory,
        StaticEntityVehicleGoal.Factory staticEntityGoalFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.NavigateToJobFactory = navigateToJobFactory;
        this.QueueingJobFactory = queueingJobFactory;
        this.StaticEntityGoalFactory = staticEntityGoalFactory;
      }

      /// <summary>
      /// Creates and enqueues a job for navigation towards given goal. If the navigation fails all other jobs will be
      /// canceled.
      /// </summary>
      public void EnqueueJob(RocketTransporter rocketTransporter, RocketLaunchPad rocketLaunchPad)
      {
        AttachRocketToLaunchPadJob rocketToLaunchPadJob = new AttachRocketToLaunchPadJob(this.m_vehicleJobIdFactory.GetNextId(), this, rocketTransporter, rocketLaunchPad);
      }
    }
  }
}
