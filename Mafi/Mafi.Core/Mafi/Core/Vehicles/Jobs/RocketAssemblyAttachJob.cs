// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.RocketAssemblyAttachJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public class RocketAssemblyAttachJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LocStrFormatted m_jobInfo;
    private readonly RocketAssemblyAttachJob.Factory m_factory;
    private RocketAssemblyAttachJob.State m_state;
    private readonly RocketTransporter m_transporter;
    private readonly RocketAssemblyBuilding m_assemblyBuilding;
    private AngleDegrees1f m_cumulativeRotation;
    private AngleDegrees1f m_previousDirection;

    public static void Serialize(RocketAssemblyAttachJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketAssemblyAttachJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketAssemblyAttachJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RocketAssemblyBuilding.Serialize(this.m_assemblyBuilding, writer);
      AngleDegrees1f.Serialize(this.m_cumulativeRotation, writer);
      LocStrFormatted.Serialize(this.m_jobInfo, writer);
      AngleDegrees1f.Serialize(this.m_previousDirection, writer);
      writer.WriteInt((int) this.m_state);
      RocketTransporter.Serialize(this.m_transporter, writer);
    }

    public static RocketAssemblyAttachJob Deserialize(BlobReader reader)
    {
      RocketAssemblyAttachJob assemblyAttachJob;
      if (reader.TryStartClassDeserialization<RocketAssemblyAttachJob>(out assemblyAttachJob))
        reader.EnqueueDataDeserialization((object) assemblyAttachJob, RocketAssemblyAttachJob.s_deserializeDataDelayedAction);
      return assemblyAttachJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RocketAssemblyAttachJob>(this, "m_assemblyBuilding", (object) RocketAssemblyBuilding.Deserialize(reader));
      this.m_cumulativeRotation = AngleDegrees1f.Deserialize(reader);
      reader.RegisterResolvedMember<RocketAssemblyAttachJob>(this, "m_factory", typeof (RocketAssemblyAttachJob.Factory), true);
      this.m_jobInfo = LocStrFormatted.Deserialize(reader);
      this.m_previousDirection = AngleDegrees1f.Deserialize(reader);
      this.m_state = (RocketAssemblyAttachJob.State) reader.ReadInt();
      reader.SetField<RocketAssemblyAttachJob>(this, "m_transporter", (object) RocketTransporter.Deserialize(reader));
    }

    public override LocStrFormatted JobInfo => this.m_jobInfo;

    public override bool IsTrueJob => true;

    public override bool SkipNoMovementMonitoring => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.None;

    private RocketAssemblyAttachJob(
      VehicleJobId id,
      RocketAssemblyAttachJob.Factory factory,
      RocketTransporter transporter,
      RocketAssemblyBuilding assemblyBuilding)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      Assert.That<Percent>(transporter.RocketHolderExtensionPerc).IsZero();
      Assert.That<Option<RocketEntityBase>>(transporter.AttachedRocketBase).IsNone<RocketEntityBase>();
      Assert.That<Option<RocketEntityBase>>(assemblyBuilding.AttachedRocketBase).HasValue<RocketEntityBase>();
      this.m_transporter = transporter.CheckNotNull<RocketTransporter>();
      this.m_assemblyBuilding = assemblyBuilding.CheckNotNull<RocketAssemblyBuilding>();
      this.m_state = RocketAssemblyAttachJob.State.OpeningRoof;
      transporter.EnqueueJob((VehicleJob) this);
      this.m_jobInfo = new LocStrFormatted("Attaching rocket");
    }

    protected override bool DoJobInternal()
    {
      Assert.That<RocketTransporter>(this.m_transporter).IsNotNull<RocketTransporter>("DoJob on non-initialized instance.");
      switch (this.m_state)
      {
        case RocketAssemblyAttachJob.State.OpeningRoof:
          if (this.m_assemblyBuilding.WaitForRoofOpen())
          {
            this.m_transporter.SetRocketHolderState(true);
            this.m_transporter.SetForceFlatGround(true);
            this.m_state = RocketAssemblyAttachJob.State.RaisingRocketToTransporter;
          }
          return true;
        case RocketAssemblyAttachJob.State.RaisingRocketToTransporter:
          this.m_assemblyBuilding.WaitForRoofOpen().AssertTrue();
          if (this.m_assemblyBuilding.WaitForRocketRaise())
            this.m_state = RocketAssemblyAttachJob.State.AttachingRocket;
          return true;
        case RocketAssemblyAttachJob.State.AttachingRocket:
          this.m_assemblyBuilding.WaitForRoofOpen().AssertTrue();
          this.m_assemblyBuilding.WaitForRocketRaise().AssertTrue();
          if (!this.m_assemblyBuilding.WaitForOpenDoors())
            return true;
          this.m_assemblyBuilding.TryTransferRocketTo((IRocketOwner) this.m_transporter);
          this.m_transporter.SetDrivingTarget(this.m_assemblyBuilding.SpawnDrivePosition, true, driveBackwards: true);
          this.m_cumulativeRotation = AngleDegrees1f.Zero;
          this.m_previousDirection = this.m_transporter.Direction;
          this.m_state = RocketAssemblyAttachJob.State.BackingOut;
          return true;
        case RocketAssemblyAttachJob.State.BackingOut:
          this.m_assemblyBuilding.WaitForRoofOpen().AssertTrue();
          this.m_assemblyBuilding.WaitForOpenDoors().AssertTrue();
          if (this.m_transporter.IsDriving || this.m_transporter.IsMoving)
          {
            this.m_cumulativeRotation += this.m_previousDirection.AngleTo(this.m_transporter.Direction).Abs;
            this.m_previousDirection = this.m_transporter.Direction;
            return true;
          }
          this.m_transporter.SetForceFlatGround(false);
          Assert.That<AngleDegrees1f>(this.m_cumulativeRotation).IsLess(180.Degrees(), "Unexpected amount of rotation of rocket transporter during backing out from rocket assembly building.");
          return false;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) this.m_state));
          return false;
      }
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      Assert.That<RocketTransporter>(this.m_transporter).IsNotNull<RocketTransporter>("CancelJob on non-initialized instance.");
      return 1.Minutes();
    }

    protected override void OnDestroy() => this.m_transporter?.SetForceFlatGround(false);

    static RocketAssemblyAttachJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketAssemblyAttachJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      RocketAssemblyAttachJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    private enum State
    {
      OpeningRoof,
      RaisingRocketToTransporter,
      AttachingRocket,
      BackingOut,
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

      public void EnqueueJob(RocketTransporter transporter, RocketAssemblyBuilding assemblyBuilding)
      {
        RocketAssemblyAttachJob assemblyAttachJob = new RocketAssemblyAttachJob(this.m_vehicleJobIdFactory.GetNextId(), this, transporter, assemblyBuilding);
      }
    }
  }
}
