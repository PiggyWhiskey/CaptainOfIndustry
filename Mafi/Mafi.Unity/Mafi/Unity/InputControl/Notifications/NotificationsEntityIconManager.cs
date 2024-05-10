// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Notifications.NotificationsEntityIconManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Notifications;
using Mafi.Unity.Entities;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Notifications
{
  /// <summary>
  /// Handles displaying icons from notifications above entities.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class NotificationsEntityIconManager
  {
    private readonly INotificationsManager m_notificationsManager;
    private readonly EntitiesIconRenderer m_entitiesIconRenderer;

    public NotificationsEntityIconManager(
      IGameLoopEvents gameLoopEvents,
      INotificationsManager notificationsManager,
      EntitiesIconRenderer entitiesIconRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_notificationsManager = notificationsManager;
      this.m_entitiesIconRenderer = entitiesIconRenderer;
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      notificationsManager.NotificationAdded += new Action<INotification>(this.notificationAdded);
      notificationsManager.NotificationSuppressChanged += new Action<INotification>(this.notificationUpdated);
      notificationsManager.NotificationRemoved += new Action<INotification>(this.notificationRemoved);
    }

    private void initState()
    {
      foreach (INotification fetchAllNotification in this.m_notificationsManager.FetchAllNotifications())
        this.notificationAdded(fetchAllNotification);
    }

    private void notificationAdded(INotification notification)
    {
      if (!notification.Proto.EntityIconSpec.HasValue || notification.Entity.IsNone || !(notification.Entity.Value is IRenderedEntity entity))
        return;
      this.m_entitiesIconRenderer.AddIcon(notification.Proto.EntityIconSpec.Value, entity);
      if (!notification.IsEntityIconSuppressed)
        return;
      this.m_entitiesIconRenderer.HideIcon(notification.Proto.EntityIconSpec.Value, entity);
    }

    private void notificationUpdated(INotification notification)
    {
      if (!notification.Proto.EntityIconSpec.HasValue || notification.Entity.IsNone || !(notification.Entity.Value is IRenderedEntity entity))
        return;
      if (notification.IsEntityIconSuppressed)
        this.m_entitiesIconRenderer.HideIcon(notification.Proto.EntityIconSpec.Value, entity);
      else
        this.m_entitiesIconRenderer.ShowIcon(notification.Proto.EntityIconSpec.Value, entity);
    }

    private void notificationRemoved(INotification notification)
    {
      if (!notification.Proto.EntityIconSpec.HasValue || notification.Entity.IsNone || !(notification.Entity.Value is IRenderedEntity entity))
        return;
      this.m_entitiesIconRenderer.RemoveIcon(notification.Proto.EntityIconSpec.Value, entity);
    }
  }
}
