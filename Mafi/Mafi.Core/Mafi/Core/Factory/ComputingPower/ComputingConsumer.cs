// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.ComputingConsumer
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
namespace Mafi.Core.Factory.ComputingPower
{
  [GenerateSerializer(false, null, 0)]
  public class ComputingConsumer : 
    IComputingConsumerInternal,
    IComputingConsumer,
    IComputingConsumerReadonly,
    IComparable<IComputingConsumer>,
    IEntityObserverForPriority,
    IEntityObserver,
    IEntityObserverForEnabled,
    IEntityObserverForUpgrade
  {
    private readonly ComputingManager m_computingManager;
    private EntityNotificator m_notEnoughComputingNotif;
    private int m_ticksWithoutPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Priority => this.Entity.GeneralPriority;

    [DoNotSave(0, null)]
    public int ProtoToken { get; set; }

    [DoNotSave(0, null)]
    public bool IsEnabled { get; private set; }

    public Computing ComputingCharged { get; private set; }

    [DoNotSave(0, null)]
    public Computing ComputingRequired { get; private set; }

    public bool DidConsumeLastTick { get; private set; }

    public bool NotEnoughComputing => this.m_notEnoughComputingNotif.IsActive;

    public IComputingConsumingEntity Entity { get; private set; }

    public ComputingConsumer(
      IComputingConsumingEntity entity,
      ComputingManager computingManager,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Entity = entity;
      this.m_computingManager = computingManager;
      this.ComputingCharged = Computing.Zero;
      this.ComputingRequired = entity.ComputingRequired;
      this.IsEnabled = entity.IsEnabled;
      this.m_notEnoughComputingNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughComputingForEntity);
      entity.AddObserver((IEntityObserver) this);
      computingManager.AddConsumer((IComputingConsumerInternal) this);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf()
    {
      this.ComputingRequired = this.Entity.ComputingRequired;
      this.IsEnabled = this.Entity.IsEnabled;
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      this.ComputingRequired = this.Entity.ComputingRequired;
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      this.m_computingManager.RemoveConsumer((IComputingConsumerInternal) this);
    }

    void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
    {
      if (!isEnabled)
      {
        this.m_ticksWithoutPower = 0;
        this.m_notEnoughComputingNotif.Deactivate((IEntity) this.Entity);
      }
      this.IsEnabled = isEnabled;
    }

    void IEntityObserverForPriority.OnGeneralPriorityChange(IEntity entity)
    {
      this.m_computingManager.RemoveConsumer((IComputingConsumerInternal) this);
      this.m_computingManager.AddConsumer((IComputingConsumerInternal) this);
    }

    public void OnComputingRequiredChanged()
    {
      this.ComputingRequired = this.Entity.ComputingRequired;
      if (!this.ComputingRequired.IsNotPositive)
        return;
      this.m_notEnoughComputingNotif.Deactivate((IEntity) this.Entity);
    }

    public void Recharge(Computing computingToAdd)
    {
      Assert.That<Computing>(computingToAdd).IsPositive();
      this.ComputingCharged += computingToAdd;
      this.DidConsumeLastTick = false;
      if (!(this.ComputingCharged >= this.ComputingRequired))
        return;
      this.updateNotification(true);
    }

    public void RechargeSkipped() => this.DidConsumeLastTick = false;

    public bool CanConsume()
    {
      bool hadPower = this.ComputingCharged >= this.ComputingRequired;
      this.updateNotification(hadPower);
      return hadPower;
    }

    public bool TryConsume()
    {
      if (!this.CanConsume())
        return false;
      this.ComputingCharged -= this.ComputingRequired;
      this.DidConsumeLastTick = true;
      return true;
    }

    public int CompareTo(IComputingConsumer other) => this.Priority.CompareTo(other.Priority);

    private void updateNotification(bool hadPower)
    {
      if (hadPower)
        this.m_ticksWithoutPower = 0;
      else
        ++this.m_ticksWithoutPower;
      this.m_notEnoughComputingNotif.NotifyIff(this.m_ticksWithoutPower > 1, (IEntity) this.Entity);
    }

    public static void Serialize(ComputingConsumer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ComputingConsumer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ComputingConsumer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Computing.Serialize(this.ComputingCharged, writer);
      writer.WriteBool(this.DidConsumeLastTick);
      writer.WriteGeneric<IComputingConsumingEntity>(this.Entity);
      ComputingManager.Serialize(this.m_computingManager, writer);
      EntityNotificator.Serialize(this.m_notEnoughComputingNotif, writer);
      writer.WriteInt(this.m_ticksWithoutPower);
    }

    public static ComputingConsumer Deserialize(BlobReader reader)
    {
      ComputingConsumer computingConsumer;
      if (reader.TryStartClassDeserialization<ComputingConsumer>(out computingConsumer))
        reader.EnqueueDataDeserialization((object) computingConsumer, ComputingConsumer.s_deserializeDataDelayedAction);
      return computingConsumer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ComputingCharged = Computing.Deserialize(reader);
      this.DidConsumeLastTick = reader.ReadBool();
      this.Entity = reader.ReadGenericAs<IComputingConsumingEntity>();
      reader.SetField<ComputingConsumer>(this, "m_computingManager", (object) ComputingManager.Deserialize(reader));
      this.m_notEnoughComputingNotif = EntityNotificator.Deserialize(reader);
      this.m_ticksWithoutPower = reader.ReadInt();
      reader.RegisterInitAfterLoad<ComputingConsumer>(this, "initSelf", InitPriority.Low);
    }

    static ComputingConsumer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ComputingConsumer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ComputingConsumer) obj).SerializeData(writer));
      ComputingConsumer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ComputingConsumer) obj).DeserializeData(reader));
    }
  }
}
