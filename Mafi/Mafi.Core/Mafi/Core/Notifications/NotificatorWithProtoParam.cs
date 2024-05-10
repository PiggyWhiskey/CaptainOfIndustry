// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificatorWithProtoParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Notifications
{
  /// <summary>
  /// Convenience wrapper for handling a given notification of type <see cref="F:Mafi.Core.Notifications.NotificationType.Continuous" />. Main
  /// purpose is to reduce boilerplate code in classes using notifications while providing caching.
  /// IMPORTANT: This is a mutable struct (for perf reasons), do not keep it in readonly fields.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public struct NotificatorWithProtoParam
  {
    private readonly INotificationsManager m_notificationsManager;
    private Option<Proto> m_parameter;

    public bool IsActive => this.NotificationId.IsValid;

    /// <summary>Notification of this notificator, if it is active.</summary>
    public NotificationId NotificationId { get; private set; }

    public NotificationProto Prototype { get; private set; }

    public NotificatorWithProtoParam(
      INotificationsManager notificationsManager,
      GeneralNotificationProto<Proto> proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<bool>(proto.Type == NotificationType.Continuous).IsTrue(string.Format("Notification {0} is not continuous and cannot be used in Notificator`1.", (object) proto.Id));
      this.m_parameter = Option<Proto>.None;
      this.NotificationId = NotificationId.Invalid;
      this.m_notificationsManager = notificationsManager.CheckNotNull<INotificationsManager>();
      this.Prototype = (NotificationProto) proto.CheckNotNull<GeneralNotificationProto<Proto>>();
    }

    [LoadCtor]
    private NotificatorWithProtoParam(
      INotificationsManager notificationsManager,
      NotificationProto prototype,
      NotificationId notificationId,
      Option<Proto> parameter)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_notificationsManager = notificationsManager;
      this.Prototype = prototype;
      this.NotificationId = notificationId;
      this.m_parameter = parameter;
    }

    /// <summary>
    /// If true given it will send a notification (if it wasn't already send). If false given it will remove
    /// notification if the notification was send previously and wasn't removed yet.
    /// </summary>
    public void NotifyIff(Proto formatParameter, bool shouldNotify)
    {
      if (shouldNotify)
        this.Activate(formatParameter);
      else
        this.Deactivate();
    }

    public void Activate(Proto formatParameter)
    {
      if (formatParameter == (Proto) null)
      {
        Log.Warning(string.Format("Trying to activate notification '{0}' with null parameter.", (object) this.Prototype.Id));
      }
      else
      {
        if (this.NotificationId.IsValid)
        {
          if (this.m_parameter.ValueOrNull == formatParameter)
            return;
          this.Deactivate();
        }
        this.NotificationId = this.m_notificationsManager.AddNotification(this.Prototype, Option<IEntity>.None, (Option<object>) (object) formatParameter);
        this.m_parameter = (Option<Proto>) formatParameter;
      }
    }

    public void Deactivate()
    {
      if (!this.IsActive)
        return;
      this.m_notificationsManager.RemoveNotification(this.NotificationId);
      this.NotificationId = NotificationId.Invalid;
      this.m_parameter = Option<Proto>.None;
    }

    public static void Serialize(NotificatorWithProtoParam value, BlobWriter writer)
    {
      writer.WriteGeneric<INotificationsManager>(value.m_notificationsManager);
      writer.WriteGeneric<NotificationProto>(value.Prototype);
      NotificationId.Serialize(value.NotificationId, writer);
      Option<Proto>.Serialize(value.m_parameter, writer);
    }

    public static NotificatorWithProtoParam Deserialize(BlobReader reader)
    {
      return new NotificatorWithProtoParam(reader.ReadGenericAs<INotificationsManager>(), reader.ReadGenericAs<NotificationProto>(), NotificationId.Deserialize(reader), Option<Proto>.Deserialize(reader));
    }
  }
}
