// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RocketTransporters.RocketTransporter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Maintenance;
using Mafi.Core.PathFinding;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.RocketTransporters
{
  [GenerateSerializer(false, null, 0)]
  public class RocketTransporter : Vehicle, IRocketOwner, IRenderedEntity, IEntity, IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration JOB_GET_MIN_INTERVAL;
    public readonly RocketTransporterProto Prototype;
    private readonly ISimLoopEvents m_simLoopEvents;
    private int m_rocketHolderExtensionTicks;
    private int m_rocketHolderExtensionTicksTarget;
    private readonly IJobProvider<RocketTransporter> m_jobProvider;
    private SimStep m_lastFailedJobGet;

    public static void Serialize(RocketTransporter value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketTransporter>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketTransporter.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<RocketEntityBase>.Serialize(this.AttachedRocketBase, writer);
      writer.WriteGeneric<IJobProvider<RocketTransporter>>(this.m_jobProvider);
      SimStep.Serialize(this.m_lastFailedJobGet, writer);
      writer.WriteInt(this.m_rocketHolderExtensionTicks);
      writer.WriteInt(this.m_rocketHolderExtensionTicksTarget);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteGeneric<IRocketTransporterOwner>(this.OwningDepot);
      writer.WriteGeneric<RocketTransporterProto>(this.Prototype);
    }

    public static RocketTransporter Deserialize(BlobReader reader)
    {
      RocketTransporter rocketTransporter;
      if (reader.TryStartClassDeserialization<RocketTransporter>(out rocketTransporter))
        reader.EnqueueDataDeserialization((object) rocketTransporter, RocketTransporter.s_deserializeDataDelayedAction);
      return rocketTransporter;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AttachedRocketBase = Option<RocketEntityBase>.Deserialize(reader);
      reader.SetField<RocketTransporter>(this, "m_jobProvider", (object) reader.ReadGenericAs<IJobProvider<RocketTransporter>>());
      this.m_lastFailedJobGet = SimStep.Deserialize(reader);
      this.m_rocketHolderExtensionTicks = reader.ReadInt();
      this.m_rocketHolderExtensionTicksTarget = reader.ReadInt();
      reader.SetField<RocketTransporter>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.OwningDepot = reader.ReadGenericAs<IRocketTransporterOwner>();
      reader.SetField<RocketTransporter>(this, "Prototype", (object) reader.ReadGenericAs<RocketTransporterProto>());
    }

    public IRocketTransporterOwner OwningDepot { get; private set; }

    public Option<RocketEntityBase> AttachedRocketBase { get; private set; }

    public Percent RocketHolderExtensionPerc
    {
      get
      {
        return Percent.FromRatio(this.m_rocketHolderExtensionTicks, this.Prototype.RocketHolderExtensionDuration.Ticks);
      }
    }

    public RocketTransporter(
      EntityId id,
      RocketTransporterProto prototype,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrain,
      IVehiclePathFindingManager pathFindingManager,
      IVehiclesManager vehiclesManager,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IJobProvider<RocketTransporter> jobProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DrivingEntityProto) prototype, context, terrain, pathFindingManager, vehiclesManager, surfaceProvider, unstuckJobFactory, maintenanceProvidersFactory);
      if (!this.DrivingData.CanTurnInPlace)
        throw new InvalidProtoException("Rocket transporter should be able to turn in place.");
      this.Prototype = prototype;
      this.m_simLoopEvents = simLoopEvents;
      this.m_jobProvider = jobProvider;
    }

    public void SetOwningDepot(IRocketTransporterOwner owningDepot)
    {
      this.OwningDepot = owningDepot;
    }

    public void SetRocketHolderState(bool extended)
    {
      this.m_rocketHolderExtensionTicksTarget = extended ? this.Prototype.RocketHolderExtensionDuration.Ticks : 0;
    }

    protected override void SimUpdateInternal()
    {
      if (!this.IsSpawned && !this.HasJobs)
        return;
      if (this.m_rocketHolderExtensionTicks != this.m_rocketHolderExtensionTicksTarget)
        this.m_rocketHolderExtensionTicks += (this.m_rocketHolderExtensionTicksTarget - this.m_rocketHolderExtensionTicks).Sign();
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
      {
        this.Jobs.CancelAll();
        this.StopNavigating();
      }
      else
      {
        base.SimUpdateInternal();
        if (this.HasJobs)
        {
          this.doJob();
        }
        else
        {
          if (!this.IsEnabled)
            return;
          this.tryGetJob();
        }
      }
    }

    private void doJob()
    {
      if (this.DoJob())
        return;
      this.tryGetJob();
    }

    private void tryGetJob()
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<bool>(this.HasJobs).IsFalse();
      if (!this.IsSpawned || this.m_simLoopEvents.CurrentStep - this.m_lastFailedJobGet < RocketTransporter.JOB_GET_MIN_INTERVAL)
        return;
      if (this.m_jobProvider.TryGetJobFor(this))
        Assert.That<bool>(this.HasJobs).IsTrue();
      else
        this.m_lastFailedJobGet = this.m_simLoopEvents.CurrentStep;
    }

    public bool CanAttachRocket(RocketEntityBase rocketBase) => this.AttachedRocketBase.IsNone;

    public void AttachRocket(RocketEntityBase rocketBase)
    {
      Assert.That<Option<IRocketOwner>>(rocketBase.Owner).IsNone<IRocketOwner>("Rocket already owned by someone. Forgot to call `SetOwner`?");
      if (!this.CanAttachRocket(rocketBase))
      {
        Log.Error("Cannot attach rocket '" + rocketBase.GetType().Name + "'.");
      }
      else
      {
        this.AttachedRocketBase = (Option<RocketEntityBase>) rocketBase;
        rocketBase.SetOwner((Option<IRocketOwner>) (IRocketOwner) this);
        Assert.That<Percent>(this.RocketHolderExtensionPerc).IsEqualTo(Percent.Hundred, "Rocket holder was not extended before rocket was attached.");
      }
    }

    Option<RocketEntityBase> IRocketOwner.DetachRocket()
    {
      if (this.AttachedRocketBase.IsNone)
      {
        Log.Warning("No rocket to detach.");
        return Option<RocketEntityBase>.None;
      }
      RocketEntityBase rocketEntityBase = this.AttachedRocketBase.Value;
      rocketEntityBase.SetOwner(Option<IRocketOwner>.None);
      this.AttachedRocketBase = Option<RocketEntityBase>.None;
      return (Option<RocketEntityBase>) rocketEntityBase;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.OwningDepot?.NotifyTransporterReturned(this);
      if (!this.AttachedRocketBase.HasValue)
        return;
      ((IEntityFriend) this.AttachedRocketBase.Value).Destroy();
    }

    static RocketTransporter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketTransporter.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RocketTransporter.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      RocketTransporter.JOB_GET_MIN_INTERVAL = 1.Seconds();
    }
  }
}
