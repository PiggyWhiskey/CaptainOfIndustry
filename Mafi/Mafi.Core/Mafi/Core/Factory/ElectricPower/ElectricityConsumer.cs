// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityConsumer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Notifications;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  public class ElectricityConsumer : 
    IElectricityConsumerInternal,
    IElectricityConsumer,
    IElectricityConsumerReadonly,
    IComparable<IElectricityConsumer>,
    IEntityObserverForEnabled,
    IEntityObserver,
    IEntityObserverForPriority,
    IEntityObserverForUpgrade
  {
    private ElectricityManager m_electricityManager;
    private EntityNotificator m_notEnoughPowerNotif;
    private int m_ticksWithoutPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public int ProtoToken { get; set; }

    public int Priority => this.Entity.GeneralPriority;

    [DoNotSave(0, null)]
    public bool IsEnabled { get; private set; }

    public bool IsSurplusConsumer { get; private set; }

    public Electricity PowerCharged { get; private set; }

    [DoNotSave(0, null)]
    public Electricity PowerRequired { get; private set; }

    public bool DidConsumeLastTick { get; private set; }

    public bool NotEnoughPower => this.m_notEnoughPowerNotif.IsActive;

    public IElectricityConsumingEntity Entity { get; private set; }

    public ElectricityConsumer(
      IElectricityConsumingEntity entity,
      ElectricityManager electricityManager,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Entity = entity;
      this.m_electricityManager = electricityManager;
      this.PowerCharged = Electricity.Zero;
      this.PowerRequired = entity.PowerRequired;
      this.IsEnabled = entity.IsEnabled;
      this.m_notEnoughPowerNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughElectricityForEntity);
      entity.AddObserver((IEntityObserver) this);
      electricityManager.AddConsumer((IElectricityConsumerInternal) this);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf()
    {
      this.PowerRequired = this.Entity.PowerRequired;
      this.IsEnabled = this.Entity.IsEnabled;
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      this.m_electricityManager.RemoveConsumer((IElectricityConsumerInternal) this);
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      this.PowerRequired = this.Entity.PowerRequired;
    }

    public void OnPowerRequiredChanged() => this.PowerRequired = this.Entity.PowerRequired;

    void IEntityObserverForPriority.OnGeneralPriorityChange(IEntity entity)
    {
      this.m_electricityManager.RemoveConsumer((IElectricityConsumerInternal) this);
      this.m_electricityManager.AddConsumer((IElectricityConsumerInternal) this);
    }

    void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
    {
      if (!isEnabled)
      {
        this.m_ticksWithoutPower = 0;
        this.m_notEnoughPowerNotif.Deactivate((IEntity) this.Entity);
      }
      this.IsEnabled = isEnabled;
    }

    public void Recharge(Electricity powerToAdd)
    {
      Assert.That<Electricity>(powerToAdd).IsPositive();
      this.PowerCharged += powerToAdd;
      this.DidConsumeLastTick = false;
      if (!(this.PowerCharged >= this.PowerRequired))
        return;
      this.updateNotification(true);
    }

    public void RechargeSkipped() => this.DidConsumeLastTick = false;

    public void SetIsSurplusConsumer(bool isSurplusConsumer)
    {
      this.IsSurplusConsumer = isSurplusConsumer;
      if (!this.IsSurplusConsumer)
        return;
      this.m_notEnoughPowerNotif.Deactivate((IEntity) this.Entity);
    }

    public bool CanConsume(bool doNotNotify = false)
    {
      bool hadPower = this.PowerCharged >= this.PowerRequired;
      if (!doNotNotify)
        this.updateNotification(hadPower);
      return hadPower;
    }

    public bool TryConsume(bool doNotNotify = false)
    {
      if (!this.CanConsume(doNotNotify))
        return false;
      this.PowerCharged -= this.PowerRequired;
      this.DidConsumeLastTick = true;
      return true;
    }

    public int CompareTo(IElectricityConsumer other)
    {
      if (this.IsSurplusConsumer == other.IsSurplusConsumer)
        return this.Priority.CompareTo(other.Priority);
      return !this.IsSurplusConsumer ? -1 : 1;
    }

    private void updateNotification(bool hadPower)
    {
      if (this.IsSurplusConsumer)
      {
        this.m_notEnoughPowerNotif.Deactivate((IEntity) this.Entity);
      }
      else
      {
        if (hadPower)
          this.m_ticksWithoutPower = 0;
        else
          ++this.m_ticksWithoutPower;
        this.m_notEnoughPowerNotif.NotifyIff(this.m_ticksWithoutPower > 1, (IEntity) this.Entity);
      }
    }

    public static void Serialize(ElectricityConsumer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityConsumer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityConsumer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.DidConsumeLastTick);
      writer.WriteGeneric<IElectricityConsumingEntity>(this.Entity);
      writer.WriteBool(this.IsSurplusConsumer);
      ElectricityManager.Serialize(this.m_electricityManager, writer);
      EntityNotificator.Serialize(this.m_notEnoughPowerNotif, writer);
      writer.WriteInt(this.m_ticksWithoutPower);
      Electricity.Serialize(this.PowerCharged, writer);
    }

    public static ElectricityConsumer Deserialize(BlobReader reader)
    {
      ElectricityConsumer electricityConsumer;
      if (reader.TryStartClassDeserialization<ElectricityConsumer>(out electricityConsumer))
        reader.EnqueueDataDeserialization((object) electricityConsumer, ElectricityConsumer.s_deserializeDataDelayedAction);
      return electricityConsumer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.DidConsumeLastTick = reader.ReadBool();
      this.Entity = reader.ReadGenericAs<IElectricityConsumingEntity>();
      this.IsSurplusConsumer = reader.ReadBool();
      this.m_electricityManager = ElectricityManager.Deserialize(reader);
      this.m_notEnoughPowerNotif = EntityNotificator.Deserialize(reader);
      this.m_ticksWithoutPower = reader.ReadInt();
      this.PowerCharged = Electricity.Deserialize(reader);
      reader.RegisterInitAfterLoad<ElectricityConsumer>(this, "initSelf", InitPriority.Low);
    }

    static ElectricityConsumer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ElectricityConsumer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ElectricityConsumer) obj).SerializeData(writer));
      ElectricityConsumer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ElectricityConsumer) obj).DeserializeData(reader));
    }
  }
}
