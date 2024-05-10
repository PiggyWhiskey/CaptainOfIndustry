// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Notifications
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class NotificationsManager : 
    INotificationsManager,
    ICommandProcessor<NotificationDismissCmd>,
    IAction<NotificationDismissCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly Dict<NotificationId, NotificationsManager.Notification> m_notificationsById;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<KeyValuePair<NotificationId, NotificationsManager.Notification>> m_notificationsTmp;
    private NotificationId m_lastNotificationId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Raised when notification should be added to the UI.</summary>
    public event Action<INotification> NotificationAdded;

    /// <summary>
    /// Raised when notification should be removed from the UI.
    /// </summary>
    public event Action<INotification> NotificationRemoved;

    /// <summary>
    /// Raised when notification should be updated in some way
    /// </summary>
    public event Action<INotification> NotificationSuppressChanged;

    public NotificationsManager(
      ProtosDb protosDb,
      ISimLoopEvents simLoopEvents,
      IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_notificationsById = new Dict<NotificationId, NotificationsManager.Notification>();
      this.m_notificationsTmp = new Lyst<KeyValuePair<NotificationId, NotificationsManager.Notification>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      simLoopEvents.Update.Add<NotificationsManager>(this, new Action(this.update));
      entitiesManager.EntityRemoved.Add<NotificationsManager>(this, new Action<IEntity>(this.entityRemoved));
      entitiesManager.OnUpgradeToBePerformed.Add<NotificationsManager>(this, new Action<IUpgradableEntity>(this.entityToBeUpgraded));
      entitiesManager.OnUpgradeJustPerformed.Add<NotificationsManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.entityUpgraded));
    }

    public NotificationId AddNotification(
      NotificationProto proto,
      Option<IEntity> entity,
      Option<object> param)
    {
      if (entity.HasValue && entity.Value.IsDestroyed)
        Log.Error(string.Format("Adding notification {0} for destroyed entity!", (object) proto.Id));
      this.m_lastNotificationId = new NotificationId(this.m_lastNotificationId.Value + 1U);
      NotificationsManager.Notification notification = new NotificationsManager.Notification(this.m_lastNotificationId, proto, entity, param);
      this.m_notificationsById.Add(this.m_lastNotificationId, notification);
      Action<INotification> notificationAdded = this.NotificationAdded;
      if (notificationAdded != null)
        notificationAdded((INotification) notification);
      return this.m_lastNotificationId;
    }

    public void UnsuppressNotification(NotificationId notificationId)
    {
      NotificationsManager.Notification notification;
      if (!this.m_notificationsById.TryGetValue(notificationId, out notification))
      {
        Log.Error("Unsuppressing unknown notification!");
      }
      else
      {
        if (!notification.IsSuppressed)
          return;
        notification.IsSuppressed = false;
        Action<INotification> notificationSuppressChanged = this.NotificationSuppressChanged;
        if (notificationSuppressChanged == null)
          return;
        notificationSuppressChanged((INotification) notification);
      }
    }

    public void RemoveNotification(NotificationId notificationId)
    {
      NotificationsManager.Notification notification;
      if (!this.m_notificationsById.TryRemove(notificationId, out notification))
        return;
      Action<INotification> notificationRemoved = this.NotificationRemoved;
      if (notificationRemoved == null)
        return;
      notificationRemoved((INotification) notification);
    }

    public void RemoveAllNotificationFor(IEntity entity, NotificationProto notificationProto)
    {
      Lyst<NotificationId> lyst = (Lyst<NotificationId>) null;
      foreach (KeyValuePair<NotificationId, NotificationsManager.Notification> keyValuePair in this.m_notificationsById)
      {
        if (keyValuePair.Value.Entity == entity && (Proto) keyValuePair.Value.Proto == (Proto) notificationProto)
        {
          if (lyst == null)
            lyst = new Lyst<NotificationId>();
          lyst.Add(keyValuePair.Key);
        }
      }
      if (lyst == null)
        return;
      foreach (NotificationId notificationId in lyst)
        this.RemoveNotification(notificationId);
    }

    public Set<INotification> FetchAllNotifications()
    {
      return this.m_notificationsById.Values.OfType<INotification>().ToSet<INotification>();
    }

    public Option<INotification> GetFirstActiveNotificationForInspector(IEntity entity)
    {
      foreach (NotificationsManager.Notification notificationForInspector in this.m_notificationsById.Values)
      {
        if (!notificationForInspector.Proto.HideInInspector && notificationForInspector.Entity == entity)
          return (Option<INotification>) (INotification) notificationForInspector;
      }
      return Option<INotification>.None;
    }

    public T GetNotificationProto<T>(Proto.ID id) where T : NotificationProto
    {
      return this.m_protosDb.GetOrThrow<T>(id);
    }

    private void entityRemoved(IEntity entity)
    {
      foreach (KeyValuePair<NotificationId, NotificationsManager.Notification> keyValuePair in this.m_notificationsById)
      {
        if (keyValuePair.Value.Entity == entity)
          this.m_notificationsTmp.Add(keyValuePair);
      }
      this.m_notificationsTmp.ForEachAndClear((Action<KeyValuePair<NotificationId, NotificationsManager.Notification>>) (x =>
      {
        this.m_notificationsById.Remove(x.Key);
        Action<INotification> notificationRemoved = this.NotificationRemoved;
        if (notificationRemoved == null)
          return;
        notificationRemoved((INotification) x.Value);
      }));
    }

    private void entityToBeUpgraded(IEntity entity)
    {
      foreach (NotificationsManager.Notification notification in this.m_notificationsById.Values)
      {
        if (notification.Entity == entity)
        {
          Action<INotification> notificationRemoved = this.NotificationRemoved;
          if (notificationRemoved != null)
            notificationRemoved((INotification) notification);
        }
      }
    }

    private void entityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      foreach (NotificationsManager.Notification notification in this.m_notificationsById.Values)
      {
        if (notification.Entity == entity)
        {
          Action<INotification> notificationAdded = this.NotificationAdded;
          if (notificationAdded != null)
            notificationAdded((INotification) notification);
        }
      }
    }

    private void update()
    {
      foreach (KeyValuePair<NotificationId, NotificationsManager.Notification> keyValuePair in this.m_notificationsById)
      {
        keyValuePair.Value.DecreaseTtlIfNeeded();
        if (keyValuePair.Value.IsTtlReached())
          this.m_notificationsTmp.Add(keyValuePair);
      }
      this.m_notificationsTmp.ForEachAndClear((Action<KeyValuePair<NotificationId, NotificationsManager.Notification>>) (x =>
      {
        this.m_notificationsById.Remove(x.Key);
        Action<INotification> notificationRemoved = this.NotificationRemoved;
        if (notificationRemoved == null)
          return;
        notificationRemoved((INotification) x.Value);
      }));
    }

    public void Invoke(NotificationDismissCmd cmd)
    {
      foreach (NotificationId notificationsId in cmd.NotificationsIds)
      {
        NotificationsManager.Notification notification;
        if (this.m_notificationsById.TryGetValue(notificationsId, out notification))
        {
          if (notification.Proto.Type == NotificationType.OneTimeOnly)
            this.m_notificationsTmp.Add(Make.Kvp<NotificationId, NotificationsManager.Notification>(notificationsId, notification));
          else if (!notification.IsSuppressed)
          {
            notification.IsSuppressed = true;
            Action<INotification> notificationSuppressChanged = this.NotificationSuppressChanged;
            if (notificationSuppressChanged != null)
              notificationSuppressChanged((INotification) notification);
          }
        }
        else
          Log.Error(string.Format("Notification '{0}' was not found.", (object) notificationsId));
      }
      this.m_notificationsTmp.ForEachAndClear((Action<KeyValuePair<NotificationId, NotificationsManager.Notification>>) (x =>
      {
        this.m_notificationsById.Remove(x.Key);
        Action<INotification> notificationRemoved = this.NotificationRemoved;
        if (notificationRemoved == null)
          return;
        notificationRemoved((INotification) x.Value);
      }));
      cmd.SetResultSuccess();
    }

    public static void Serialize(NotificationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NotificationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NotificationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      NotificationId.Serialize(this.m_lastNotificationId, writer);
      Dict<NotificationId, NotificationsManager.Notification>.Serialize(this.m_notificationsById, writer);
    }

    public static NotificationsManager Deserialize(BlobReader reader)
    {
      NotificationsManager notificationsManager;
      if (reader.TryStartClassDeserialization<NotificationsManager>(out notificationsManager))
        reader.EnqueueDataDeserialization((object) notificationsManager, NotificationsManager.s_deserializeDataDelayedAction);
      return notificationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_lastNotificationId = NotificationId.Deserialize(reader);
      reader.SetField<NotificationsManager>(this, "m_notificationsById", (object) Dict<NotificationId, NotificationsManager.Notification>.Deserialize(reader));
      reader.SetField<NotificationsManager>(this, "m_notificationsTmp", (object) new Lyst<KeyValuePair<NotificationId, NotificationsManager.Notification>>());
      reader.RegisterResolvedMember<NotificationsManager>(this, "m_protosDb", typeof (ProtosDb), true);
    }

    static NotificationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NotificationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NotificationsManager) obj).SerializeData(writer));
      NotificationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NotificationsManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Notification : INotification
    {
      [DoNotSave(0, null)]
      private LocStrFormatted? m_message;
      private readonly Option<object> m_param;
      private Duration m_remainingDuration;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public NotificationId NotificationId { get; private set; }

      public NotificationProto Proto { get; private set; }

      public LocStrFormatted Message
      {
        get
        {
          if (!this.m_message.HasValue)
            this.m_message = new LocStrFormatted?(this.Proto.GetMessage(this.m_param, this.Entity));
          return this.m_message.Value;
        }
      }

      public Option<IEntity> Entity { get; private set; }

      /// <summary>
      /// Whether the notification was dismissed by the user and shouldn't be displayed again for a while.
      /// </summary>
      public bool IsSuppressed { get; set; }

      /// <summary>
      /// Whether the notification is dismissed and that means it shouldn't show an icon.
      /// </summary>
      public bool IsEntityIconSuppressed
      {
        get => this.IsSuppressed && this.Proto.SuppressEntityIconOnSuppress;
      }

      public Notification(
        NotificationId notificationId,
        NotificationProto proto,
        Option<IEntity> entity,
        Option<object> param)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.NotificationId = notificationId;
        this.Proto = proto;
        this.m_remainingDuration = proto.TimeToLive;
        this.Entity = entity;
        this.m_param = param;
      }

      public void DecreaseTtlIfNeeded()
      {
        if (!this.Proto.IsTimeLimited)
          return;
        this.m_remainingDuration -= Duration.OneTick;
      }

      public bool IsTtlReached()
      {
        return this.Proto.IsTimeLimited && this.m_remainingDuration == Duration.Zero;
      }

      public static void Serialize(NotificationsManager.Notification value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<NotificationsManager.Notification>(value))
          return;
        writer.EnqueueDataSerialization((object) value, NotificationsManager.Notification.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Option<IEntity>.Serialize(this.Entity, writer);
        writer.WriteBool(this.IsSuppressed);
        Option<object>.Serialize(this.m_param, writer);
        Duration.Serialize(this.m_remainingDuration, writer);
        NotificationId.Serialize(this.NotificationId, writer);
        writer.WriteGeneric<NotificationProto>(this.Proto);
      }

      public static NotificationsManager.Notification Deserialize(BlobReader reader)
      {
        NotificationsManager.Notification notification;
        if (reader.TryStartClassDeserialization<NotificationsManager.Notification>(out notification))
          reader.EnqueueDataDeserialization((object) notification, NotificationsManager.Notification.s_deserializeDataDelayedAction);
        return notification;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Entity = Option<IEntity>.Deserialize(reader);
        this.IsSuppressed = reader.ReadBool();
        reader.SetField<NotificationsManager.Notification>(this, "m_param", (object) Option<object>.Deserialize(reader));
        this.m_remainingDuration = Duration.Deserialize(reader);
        this.NotificationId = NotificationId.Deserialize(reader);
        this.Proto = reader.ReadGenericAs<NotificationProto>();
      }

      static Notification()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        NotificationsManager.Notification.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NotificationsManager.Notification) obj).SerializeData(writer));
        NotificationsManager.Notification.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NotificationsManager.Notification) obj).DeserializeData(reader));
      }
    }
  }
}
