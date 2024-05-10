// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Beacons.Beacon
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Population;
using Mafi.Core.Population.Refugees;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Beacons
{
  [GenerateSerializer(false, null, 0)]
  public class Beacon : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IUnityConsumingEntity,
    IElectricityConsumingEntity
  {
    public readonly BeaconProto Prototype;
    private readonly RefugeesManager m_refugeesManager;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly ISimLoopEvents m_simLoopEvents;
    [RenamedInVersion(140, "UnityConsumerInternal")]
    private readonly Mafi.Core.Population.UnityConsumer m_unityConsumer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public bool NotEnoughPower => this.m_electricityConsumer.NotEnoughPower;

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Upoints MaxMonthlyUnityConsumed => this.MonthlyUnityConsumed;

    public Upoints MonthlyUnityConsumed => this.Prototype.UnityMonthlyCost;

    public Proto.ID UpointsCategoryId => this.Prototype.UpointsCategory.Id;

    public Option<Mafi.Core.Population.UnityConsumer> UnityConsumer
    {
      get => (Option<Mafi.Core.Population.UnityConsumer>) this.m_unityConsumer;
    }

    public SimStep LastWorkedOnSimStep { get; private set; }

    public Beacon(
      EntityId id,
      BeaconProto beaconProto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      RefugeesManager refugeesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) beaconProto, transform, context);
      this.Prototype = beaconProto;
      this.m_simLoopEvents = simLoopEvents;
      this.m_refugeesManager = refugeesManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.m_unityConsumer = this.Context.UnityConsumerFactory.CreateConsumer((IUnityConsumingEntity) this);
    }

    public bool TryWork()
    {
      if (!this.IsEnabled)
        return false;
      if (!this.m_unityConsumer.CanWork())
      {
        this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
        return false;
      }
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this) || !this.m_electricityConsumer.TryConsume())
        return false;
      this.LastWorkedOnSimStep = this.m_simLoopEvents.CurrentStep;
      return true;
    }

    public static void Serialize(Beacon value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Beacon>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Beacon.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      SimStep.Serialize(this.LastWorkedOnSimStep, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      RefugeesManager.Serialize(this.m_refugeesManager, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteGeneric<BeaconProto>(this.Prototype);
      Mafi.Core.Population.UnityConsumer.Serialize(this.m_unityConsumer, writer);
    }

    public static Beacon Deserialize(BlobReader reader)
    {
      Beacon beacon;
      if (reader.TryStartClassDeserialization<Beacon>(out beacon))
        reader.EnqueueDataDeserialization((object) beacon, Beacon.s_deserializeDataDelayedAction);
      return beacon;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.LastWorkedOnSimStep = SimStep.Deserialize(reader);
      reader.SetField<Beacon>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<Beacon>(this, "m_refugeesManager", (object) RefugeesManager.Deserialize(reader));
      reader.SetField<Beacon>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<Beacon>(this, "Prototype", (object) reader.ReadGenericAs<BeaconProto>());
      reader.SetField<Beacon>(this, "m_unityConsumer", (object) Mafi.Core.Population.UnityConsumer.Deserialize(reader));
    }

    static Beacon()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Beacon.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Beacon.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
