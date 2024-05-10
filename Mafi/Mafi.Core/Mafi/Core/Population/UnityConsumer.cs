// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UnityConsumer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Notifications;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population
{
  [GenerateSerializer(false, null, 0)]
  public class UnityConsumer : 
    IEventOwner,
    IComparable<UnityConsumer>,
    IEntityObserverForPriority,
    IEntityObserver,
    IEntityObserverForEnabled,
    IEntityObserverForUpgrade
  {
    public Upoints MonthlyUnity;
    private readonly IUpointsManager m_upointsManager;
    private readonly ICalendar m_calendar;
    private readonly IUnityConsumingEntity m_entity;
    private EntityNotificator m_notEnoughUnityNotif;
    private Upoints m_unityToPayNow;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Priority => this.m_entity.GeneralPriority;

    public bool IsDestroyed => this.m_entity.IsDestroyed;

    public bool IsEnabled => this.m_entity.IsEnabled;

    public bool NotEnoughUnity { get; private set; }

    private Percent PercentOfMonthLeft
    {
      get => Percent.FromRatio(30 - (this.m_calendar.CurrentDate.Day - 1), 30);
    }

    public UnityConsumer(
      IUpointsManager upointsManager,
      ICalendar calendar,
      INotificationsManager notificationsManager,
      IUnityConsumingEntity entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upointsManager = upointsManager;
      this.m_calendar = calendar;
      this.m_entity = entity;
      entity.AddObserver((IEntityObserver) this);
      this.m_notEnoughUnityNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughUpointsForEntity);
      this.setMonthlyUnityToConsume(entity.MonthlyUnityConsumed);
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      this.m_entity.RemoveObserver((IEntityObserver) this);
      this.setMonthlyUnityToConsume(Upoints.Zero);
    }

    void IEntityObserverForPriority.OnGeneralPriorityChange(IEntity entity)
    {
      if (this.MonthlyUnity.IsNotPositive)
        return;
      this.m_upointsManager.RemoveConsumer(this);
      this.m_upointsManager.AddConsumer(this);
    }

    void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
    {
      if (!isEnabled)
        this.m_notEnoughUnityNotif.Deactivate((IEntity) this.m_entity);
      if (!isEnabled || !this.NotEnoughUnity)
        return;
      this.m_unityToPayNow = this.MonthlyUnity.ScaledBy(this.PercentOfMonthLeft);
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      this.setMonthlyUnityToConsume(this.m_entity.MonthlyUnityConsumed);
    }

    public void RechargeOnNewMonth()
    {
      this.NotEnoughUnity = true;
      if (!this.IsEnabled)
        return;
      this.NotEnoughUnity = !this.m_upointsManager.TryConsume(this.m_entity.UpointsCategoryId, this.MonthlyUnity, this.m_entity.SomeOption<IUnityConsumingEntity>().As<IEntity>(), new LocStr?(this.m_entity.Prototype.Strings.Name));
      if (this.NotEnoughUnity)
        return;
      this.m_notEnoughUnityNotif.Deactivate((IEntity) this.m_entity);
    }

    public int CompareTo(UnityConsumer other) => this.Priority.CompareTo(other.Priority);

    public void RefreshUnityConsumed()
    {
      this.setMonthlyUnityToConsume(this.m_entity.MonthlyUnityConsumed);
    }

    private void setMonthlyUnityToConsume(Upoints monthlyUnity)
    {
      if (this.MonthlyUnity.IsPositive)
        this.m_upointsManager.RemoveConsumer(this);
      Upoints upoints = this.NotEnoughUnity ? monthlyUnity.ScaledBy(this.PercentOfMonthLeft) : monthlyUnity.ScaledBy(this.PercentOfMonthLeft) - this.MonthlyUnity;
      this.MonthlyUnity = monthlyUnity.CheckNotNegative();
      if (this.MonthlyUnity.IsPositive)
        this.m_upointsManager.AddConsumer(this);
      if (upoints.IsPositive)
      {
        this.m_unityToPayNow = upoints;
        this.NotEnoughUnity = true;
      }
      else
      {
        this.m_unityToPayNow = Upoints.Zero;
        this.NotEnoughUnity = false;
        this.m_notEnoughUnityNotif.Deactivate((IEntity) this.m_entity);
      }
    }

    public bool CanWork()
    {
      if (this.m_unityToPayNow.IsPositive)
      {
        if (this.m_upointsManager.TryConsume(this.m_entity.UpointsCategoryId, this.m_unityToPayNow, this.m_entity.SomeOption<IUnityConsumingEntity>().As<IEntity>(), new LocStr?(this.m_entity.Prototype.Strings.Name)))
          this.NotEnoughUnity = false;
        this.m_unityToPayNow = Upoints.Zero;
      }
      this.m_notEnoughUnityNotif.NotifyIff(this.NotEnoughUnity, (IEntity) this.m_entity);
      return !this.NotEnoughUnity;
    }

    public void Destroy()
    {
      this.m_entity.RemoveObserver((IEntityObserver) this);
      this.setMonthlyUnityToConsume(Upoints.Zero);
    }

    public static void Serialize(UnityConsumer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnityConsumer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnityConsumer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IUnityConsumingEntity>(this.m_entity);
      EntityNotificator.Serialize(this.m_notEnoughUnityNotif, writer);
      Upoints.Serialize(this.m_unityToPayNow, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      Upoints.Serialize(this.MonthlyUnity, writer);
      writer.WriteBool(this.NotEnoughUnity);
    }

    public static UnityConsumer Deserialize(BlobReader reader)
    {
      UnityConsumer unityConsumer;
      if (reader.TryStartClassDeserialization<UnityConsumer>(out unityConsumer))
        reader.EnqueueDataDeserialization((object) unityConsumer, UnityConsumer.s_deserializeDataDelayedAction);
      return unityConsumer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UnityConsumer>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<UnityConsumer>(this, "m_entity", (object) reader.ReadGenericAs<IUnityConsumingEntity>());
      this.m_notEnoughUnityNotif = EntityNotificator.Deserialize(reader);
      this.m_unityToPayNow = Upoints.Deserialize(reader);
      reader.SetField<UnityConsumer>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      this.MonthlyUnity = Upoints.Deserialize(reader);
      this.NotEnoughUnity = reader.ReadBool();
    }

    static UnityConsumer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnityConsumer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnityConsumer) obj).SerializeData(writer));
      UnityConsumer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnityConsumer) obj).DeserializeData(reader));
    }
  }
}
