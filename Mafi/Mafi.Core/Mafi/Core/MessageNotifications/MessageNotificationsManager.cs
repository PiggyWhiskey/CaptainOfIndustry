// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.MessageNotificationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Input;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Messages;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.MessageNotifications
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class MessageNotificationsManager : 
    IMessageNotificationsManager,
    ICommandProcessor<MessageNotificationDismissCmd>,
    IAction<MessageNotificationDismissCmd>
  {
    private readonly MessageNotificationId.Factory m_idFactory;
    private readonly Lyst<IMessageNotification> m_allNotifications;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Raised when message notification should be added to the UI.
    /// </summary>
    public event Action<IMessageNotification> OnNotificationAdded;

    /// <summary>
    /// Raised when message notification should be removed from the UI.
    /// </summary>
    public event Action<IMessageNotification> OnNotificationRemoved;

    public IReadOnlyCollection<IMessageNotification> AllNotifications
    {
      get => (IReadOnlyCollection<IMessageNotification>) this.m_allNotifications;
    }

    public MessageNotificationsManager(MessageNotificationId.Factory idFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_allNotifications = new Lyst<IMessageNotification>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_idFactory = idFactory;
    }

    public void AddMessage(IMessageNotification message)
    {
      Assert.That<bool>(message.NotificationId.IsValid).IsFalse();
      message.NotificationId = this.m_idFactory.GetNextId();
      this.m_allNotifications.Add(message);
      Action<IMessageNotification> notificationAdded = this.OnNotificationAdded;
      if (notificationAdded == null)
        return;
      notificationAdded(message);
    }

    public void DismissNotificationForMessageIfExists(MessageProto messageProto)
    {
      NewMessageNotification messageNotification = this.m_allNotifications.OfType<NewMessageNotification>().FirstOrDefault<NewMessageNotification>((Func<NewMessageNotification, bool>) (x => (Proto) x.Message.Proto == (Proto) messageProto));
      if (messageNotification == null)
        return;
      this.m_allNotifications.RemoveAndAssert((IMessageNotification) messageNotification);
      Action<IMessageNotification> notificationRemoved = this.OnNotificationRemoved;
      if (notificationRemoved == null)
        return;
      notificationRemoved((IMessageNotification) messageNotification);
    }

    public void Invoke(MessageNotificationDismissCmd cmd)
    {
      int? nullable = this.m_allNotifications.FirstIndexOf<IMessageNotification>((Predicate<IMessageNotification>) (x => x.NotificationId == cmd.MessageId));
      if (!nullable.HasValue)
      {
        cmd.SetResultError(string.Format("Message with id {0} could not be found", (object) cmd.MessageId));
      }
      else
      {
        IMessageNotification allNotification = this.m_allNotifications[nullable.Value];
        if (allNotification is GameOverNotification)
        {
          cmd.SetResultSuccess();
        }
        else
        {
          this.m_allNotifications.RemoveAt(nullable.Value);
          Action<IMessageNotification> notificationRemoved = this.OnNotificationRemoved;
          if (notificationRemoved != null)
            notificationRemoved(allNotification);
          cmd.SetResultSuccess();
        }
      }
    }

    public void DismissAllNotifications()
    {
      foreach (IMessageNotification allNotification in this.m_allNotifications)
      {
        Action<IMessageNotification> notificationRemoved = this.OnNotificationRemoved;
        if (notificationRemoved != null)
          notificationRemoved(allNotification);
      }
      this.m_allNotifications.Clear();
    }

    public static void Serialize(MessageNotificationsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageNotificationsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageNotificationsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<IMessageNotification>.Serialize(this.m_allNotifications, writer);
      MessageNotificationId.Factory.Serialize(this.m_idFactory, writer);
    }

    public static MessageNotificationsManager Deserialize(BlobReader reader)
    {
      MessageNotificationsManager notificationsManager;
      if (reader.TryStartClassDeserialization<MessageNotificationsManager>(out notificationsManager))
        reader.EnqueueDataDeserialization((object) notificationsManager, MessageNotificationsManager.s_deserializeDataDelayedAction);
      return notificationsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MessageNotificationsManager>(this, "m_allNotifications", (object) Lyst<IMessageNotification>.Deserialize(reader));
      reader.SetField<MessageNotificationsManager>(this, "m_idFactory", (object) MessageNotificationId.Factory.Deserialize(reader));
    }

    static MessageNotificationsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageNotificationsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageNotificationsManager) obj).SerializeData(writer));
      MessageNotificationsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageNotificationsManager) obj).DeserializeData(reader));
    }
  }
}
