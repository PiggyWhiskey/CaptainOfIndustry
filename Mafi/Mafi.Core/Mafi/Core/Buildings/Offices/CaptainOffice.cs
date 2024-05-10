// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Offices.CaptainOffice
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Population;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Offices
{
  [GenerateSerializer(false, null, 0)]
  public class CaptainOffice : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IElectricityConsumingEntity,
    IEntityWithSimUpdate,
    IEntityWithEmission,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private CaptainOfficeProto m_proto;
    private readonly IElectricityConsumer m_electricityConsumer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public CaptainOfficeProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public CaptainOffice.State CurrentState { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public bool IsActive { get; private set; }

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionIntensity.HasValue ? new float?() : new float?(this.IsActive ? (float) this.Prototype.EmissionIntensity.Value : 0.0f);
      }
    }

    public CaptainOffice(
      EntityId id,
      CaptainOfficeProto proto,
      TileTransform transform,
      EntityContext context,
      ILayoutEntityUpgraderFactory upgraderFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Upgrader = upgraderFactory.CreateInstance<CaptainOfficeProto, CaptainOffice>(this, this.Prototype);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.isOperational();
      this.IsActive = this.CurrentState == CaptainOffice.State.Working;
    }

    private CaptainOffice.State isOperational()
    {
      if (!this.IsEnabled)
        return CaptainOffice.State.Paused;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return CaptainOffice.State.NotEnoughWorkers;
      return !this.m_electricityConsumer.TryConsume() ? CaptainOffice.State.NotEnoughPower : CaptainOffice.State.Working;
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    public static void Serialize(CaptainOffice value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CaptainOffice>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CaptainOffice.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteBool(this.IsActive);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteGeneric<CaptainOfficeProto>(this.m_proto);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static CaptainOffice Deserialize(BlobReader reader)
    {
      CaptainOffice captainOffice;
      if (reader.TryStartClassDeserialization<CaptainOffice>(out captainOffice))
        reader.EnqueueDataDeserialization((object) captainOffice, CaptainOffice.s_deserializeDataDelayedAction);
      return captainOffice;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (CaptainOffice.State) reader.ReadInt();
      this.IsActive = reader.ReadBool();
      reader.SetField<CaptainOffice>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_proto = reader.ReadGenericAs<CaptainOfficeProto>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static CaptainOffice()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CaptainOffice.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      CaptainOffice.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      None,
      Paused,
      NotEnoughWorkers,
      NotEnoughPower,
      Working,
    }
  }
}
