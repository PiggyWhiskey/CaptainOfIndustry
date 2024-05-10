// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.INotificationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Notifications
{
  public interface INotificationsManager
  {
    /// <summary>Raised when notification should be added to the UI.</summary>
    event Action<INotification> NotificationAdded;

    /// <summary>Raised when notification should be removed entirely.</summary>
    event Action<INotification> NotificationRemoved;

    /// <summary>Raised when notification should be updated.</summary>
    event Action<INotification> NotificationSuppressChanged;

    /// <summary>
    /// Expensive and used only to fetch notifs from UI after game load.
    /// </summary>
    Set<INotification> FetchAllNotifications();

    /// <summary>If a supressed notification exists, unsuppress it</summary>
    void UnsuppressNotification(NotificationId notificationId);

    Option<INotification> GetFirstActiveNotificationForInspector(IEntity entity);

    NotificationId AddNotification(
      NotificationProto proto,
      Option<IEntity> entity,
      Option<object> param);

    T GetNotificationProto<T>(Proto.ID id) where T : NotificationProto;

    void RemoveNotification(NotificationId notificationId);

    void RemoveAllNotificationFor(IEntity entity, NotificationProto notificationProto);
  }
}
