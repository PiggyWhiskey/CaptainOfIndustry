// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ComputingEntities.Mainframe
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.ComputingEntities
{
  [GenerateSerializer(false, null, 0)]
  public class Mainframe : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IElectricityConsumingEntity,
    IMaintainedEntity,
    IEntityWithSimUpdate,
    IComputingGenerator,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithEmission
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly MainframeProto Prototype;
    private readonly IComputingManager m_computingManager;
    private readonly IElectricityConsumer m_electricityConsumer;

    public static void Serialize(Mainframe value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Mainframe>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Mainframe.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteGeneric<IComputingManager>(this.m_computingManager);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<MainframeProto>(this.Prototype);
    }

    public static Mainframe Deserialize(BlobReader reader)
    {
      Mainframe mainframe;
      if (reader.TryStartClassDeserialization<Mainframe>(out mainframe))
        reader.EnqueueDataDeserialization((object) mainframe, Mainframe.s_deserializeDataDelayedAction);
      return mainframe;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (Mainframe.State) reader.ReadInt();
      reader.SetField<Mainframe>(this, "m_computingManager", (object) reader.ReadGenericAs<IComputingManager>());
      reader.SetField<Mainframe>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.SetField<Mainframe>(this, "Prototype", (object) reader.ReadGenericAs<MainframeProto>());
    }

    public override bool CanBePaused => true;

    public Computing MaxComputingGenerationCapacity => this.Prototype.ComputingGenerated;

    public Mainframe.State CurrentState { get; private set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => false;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Electricity PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    public float? EmissionIntensity
    {
      get => new float?(this.CurrentState == Mainframe.State.Working ? 2.5f : 0.0f);
    }

    public Mainframe(
      EntityId id,
      MainframeProto proto,
      TileTransform transform,
      EntityContext context,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IComputingManager computingManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_computingManager = computingManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
    }

    public void SimUpdate() => this.CurrentState = this.updateState();

    private Mainframe.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? Mainframe.State.Paused : Mainframe.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return Mainframe.State.NotEnoughWorkers;
      return !this.m_electricityConsumer.TryConsume() ? Mainframe.State.NotEnoughElectricity : Mainframe.State.Working;
    }

    public Computing GenerateComputing()
    {
      return this.CurrentState == Mainframe.State.Working ? this.MaxComputingGenerationCapacity : Computing.Zero;
    }

    static Mainframe()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      Mainframe.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Mainframe.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      None,
      Working,
      Paused,
      Broken,
      NotEnoughWorkers,
      NotEnoughElectricity,
    }
  }
}
