// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationsManagerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Notifications
{
  public static class NotificationsManagerExtensions
  {
    public static Notificator CreateNotificatorFor(
      this INotificationsManager mgr,
      GeneralNotificationProto.ID protoId)
    {
      return new Notificator(mgr, (NotificationProto) mgr.GetNotificationProto<GeneralNotificationProto>((Proto.ID) protoId));
    }

    public static EntityNotificator CreateNotificatorFor(
      this INotificationsManager mgr,
      EntityNotificationProto.ID protoId)
    {
      return new EntityNotificator((NotificationProto) mgr.GetNotificationProto<EntityNotificationProto>((Proto.ID) protoId));
    }

    public static EntityNotificatorWithProtoParam CreateNotificatorFor(
      this INotificationsManager mgr,
      EntityNotificationProto<Proto>.ID protoId)
    {
      return new EntityNotificatorWithProtoParam(mgr.GetNotificationProto<EntityNotificationProto<Proto>>((Proto.ID) protoId));
    }

    public static NotificatorWithProtoParam CreateNotificatorFor(
      this INotificationsManager mgr,
      GeneralNotificationProto<Proto>.ID protoId)
    {
      return new NotificatorWithProtoParam(mgr, mgr.GetNotificationProto<GeneralNotificationProto<Proto>>((Proto.ID) protoId));
    }

    public static void NotifyOnce(
      this INotificationsManager mgr,
      GeneralNotificationProto.ID protoId)
    {
      GeneralNotificationProto notificationProto = mgr.GetNotificationProto<GeneralNotificationProto>((Proto.ID) protoId);
      Assert.That<NotificationType>(notificationProto.Type).IsEqualTo<NotificationType>(NotificationType.OneTimeOnly);
      mgr.AddNotification((NotificationProto) notificationProto, Option<IEntity>.None, (Option<object>) Option.None);
    }

    public static void NotifyOnce(
      this INotificationsManager mgr,
      EntityNotificationProto.ID protoId,
      IEntity entity)
    {
      EntityNotificationProto notificationProto = mgr.GetNotificationProto<EntityNotificationProto>((Proto.ID) protoId);
      Assert.That<NotificationType>(notificationProto.Type).IsEqualTo<NotificationType>(NotificationType.OneTimeOnly);
      Option<IEntity> entity1 = entity.SomeOption<IEntity>();
      mgr.AddNotification((NotificationProto) notificationProto, entity1, (Option<object>) Option.None);
    }

    public static void NotifyOnce<TParam>(
      this INotificationsManager mgr,
      GeneralNotificationProto<TParam>.ID protoId,
      TParam value)
    {
      GeneralNotificationProto<TParam> notificationProto = mgr.GetNotificationProto<GeneralNotificationProto<TParam>>((Proto.ID) protoId);
      Assert.That<NotificationType>(notificationProto.Type).IsEqualTo<NotificationType>(NotificationType.OneTimeOnly);
      mgr.AddNotification((NotificationProto) notificationProto, Option<IEntity>.None, (Option<object>) (object) value);
    }

    public static void NotifyOnce<TParam>(
      this INotificationsManager mgr,
      EntityNotificationProto<TParam>.ID protoId,
      TParam value,
      IEntity entity)
    {
      EntityNotificationProto<TParam> notificationProto = mgr.GetNotificationProto<EntityNotificationProto<TParam>>((Proto.ID) protoId);
      Assert.That<NotificationType>(notificationProto.Type).IsEqualTo<NotificationType>(NotificationType.OneTimeOnly);
      Option<IEntity> entity1 = entity.SomeOption<IEntity>();
      mgr.AddNotification((NotificationProto) notificationProto, entity1, (Option<object>) (object) value);
    }
  }
}
