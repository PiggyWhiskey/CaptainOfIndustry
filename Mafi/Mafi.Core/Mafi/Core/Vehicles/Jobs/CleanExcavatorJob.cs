// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.CleanExcavatorJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Job that resets Excavator to a default state - no cargo, cabin at default position, truck queue disabled.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class CleanExcavatorJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly Excavator m_excavator;
    private CleanExcavatorJob.State m_state;
    private readonly TickTimer m_dumpTimer;
    private readonly CleanExcavatorJob.Factory m_factory;

    public static void Serialize(CleanExcavatorJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CleanExcavatorJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CleanExcavatorJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TickTimer.Serialize(this.m_dumpTimer, writer);
      Excavator.Serialize(this.m_excavator, writer);
      writer.WriteInt((int) this.m_state);
    }

    public static CleanExcavatorJob Deserialize(BlobReader reader)
    {
      CleanExcavatorJob cleanExcavatorJob;
      if (reader.TryStartClassDeserialization<CleanExcavatorJob>(out cleanExcavatorJob))
        reader.EnqueueDataDeserialization((object) cleanExcavatorJob, CleanExcavatorJob.s_deserializeDataDelayedAction);
      return cleanExcavatorJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CleanExcavatorJob>(this, "m_dumpTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<CleanExcavatorJob>(this, "m_excavator", (object) Excavator.Deserialize(reader));
      reader.RegisterResolvedMember<CleanExcavatorJob>(this, "m_factory", typeof (CleanExcavatorJob.Factory), true);
      this.m_state = (CleanExcavatorJob.State) reader.ReadInt();
    }

    public override LocStrFormatted JobInfo => CleanExcavatorJob.s_jobInfo;

    public override bool IsTrueJob => false;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public CleanExcavatorJob(
      VehicleJobId id,
      CleanExcavatorJob.Factory factory,
      Excavator excavator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_dumpTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_excavator = excavator;
      this.m_state = CleanExcavatorJob.State.Initialization;
      this.m_dumpTimer.Reset();
    }

    protected override bool DoJobInternal()
    {
      switch (this.m_state)
      {
        case CleanExcavatorJob.State.Initialization:
          this.m_state = this.handleInitialization();
          break;
        case CleanExcavatorJob.State.TrucksRelease:
          this.m_state = this.handleTrucksRelease();
          break;
        case CleanExcavatorJob.State.ArmFinishMining:
          this.m_state = this.handleArmFinishMining();
          break;
        case CleanExcavatorJob.State.ArmPrepareDump:
          this.m_state = this.handleArmPrepareDump();
          break;
        case CleanExcavatorJob.State.ArmDump:
          this.m_state = this.handleArmDump();
          break;
        case CleanExcavatorJob.State.CabinReset:
          this.m_state = this.handleCabinReset();
          break;
        default:
          Log.Error(string.Format("Unknown/invalid excavator cleaning job state '{0}'.", (object) this.m_state));
          return false;
      }
      return this.m_state != CleanExcavatorJob.State.Done;
    }

    private CleanExcavatorJob.State handleInitialization()
    {
      if (this.m_excavator.Cargo.IsNotEmpty)
        this.m_excavator.ClearCargoImmediately();
      return CleanExcavatorJob.State.TrucksRelease;
    }

    private CleanExcavatorJob.State handleTrucksRelease()
    {
      Assert.That<bool>(this.m_excavator.Cargo.IsEmpty).IsTrue();
      this.m_excavator.TruckQueue.Disable();
      return CleanExcavatorJob.State.ArmFinishMining;
    }

    private CleanExcavatorJob.State handleArmFinishMining()
    {
      if (!this.m_excavator.IsShovelAtTarget)
        return CleanExcavatorJob.State.ArmFinishMining;
      if (this.m_excavator.ShovelState == ExcavatorShovelState.PrepareToMine)
      {
        this.m_excavator.SetShovelState(ExcavatorShovelState.Mine);
        return CleanExcavatorJob.State.ArmFinishMining;
      }
      if (this.m_excavator.ShovelState != ExcavatorShovelState.Mine)
        return CleanExcavatorJob.State.ArmPrepareDump;
      this.m_excavator.SetShovelState(ExcavatorShovelState.Tucked);
      return CleanExcavatorJob.State.ArmFinishMining;
    }

    private CleanExcavatorJob.State handleArmPrepareDump()
    {
      if (!this.m_excavator.IsShovelAtTarget)
        return CleanExcavatorJob.State.ArmPrepareDump;
      if (this.m_excavator.Cargo.IsEmpty)
      {
        if (this.m_excavator.ShovelState == ExcavatorShovelState.PrepareToDump)
          this.m_excavator.SetShovelState(ExcavatorShovelState.Dump);
        return CleanExcavatorJob.State.ArmDump;
      }
      if (this.m_excavator.ShovelState != ExcavatorShovelState.PrepareToDump)
      {
        this.m_excavator.SetShovelState(ExcavatorShovelState.PrepareToDump);
        return CleanExcavatorJob.State.ArmPrepareDump;
      }
      this.m_excavator.SetShovelState(ExcavatorShovelState.Dump);
      this.m_dumpTimer.Start(this.m_excavator.Prototype.MineTimings.DumpDelay);
      return CleanExcavatorJob.State.ArmDump;
    }

    private CleanExcavatorJob.State handleArmDump()
    {
      if (this.m_dumpTimer.Decrement())
        return CleanExcavatorJob.State.ArmDump;
      if (this.m_excavator.Cargo.IsNotEmpty)
        this.m_excavator.ClearCargoImmediately();
      if (!this.m_excavator.IsShovelAtTarget)
        return CleanExcavatorJob.State.ArmDump;
      this.m_excavator.ResetCabinTarget();
      return CleanExcavatorJob.State.CabinReset;
    }

    private CleanExcavatorJob.State handleCabinReset()
    {
      return !this.m_excavator.IsCabinAtTarget ? CleanExcavatorJob.State.CabinReset : CleanExcavatorJob.State.Done;
    }

    protected override Duration RequestCancelReturnDeadline() => Duration.Zero;

    protected override void OnDestroy() => this.m_dumpTimer.Reset();

    public static bool IsClean(Excavator excavator)
    {
      return excavator.Cargo.IsEmpty && excavator.CabinDirectionRelative.Normalized == AngleDegrees1f.Zero && !excavator.TruckQueue.IsEnabled;
    }

    static CleanExcavatorJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CleanExcavatorJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      CleanExcavatorJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      CleanExcavatorJob.s_jobInfo = new LocStrFormatted("Cleaning Excavator.");
    }

    private enum State
    {
      Initialization,
      TrucksRelease,
      ArmFinishMining,
      ArmPrepareDump,
      ArmDump,
      CabinReset,
      Done,
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

      public void EnqueueJob(Excavator excavator)
      {
        CleanExcavatorJob job = new CleanExcavatorJob(this.m_vehicleJobIdFactory.GetNextId(), this, excavator);
        excavator.EnqueueJob((VehicleJob) job, false);
      }
    }
  }
}
