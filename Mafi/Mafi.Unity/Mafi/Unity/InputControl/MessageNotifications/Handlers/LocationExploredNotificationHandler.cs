// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.LocationExploredNotificationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Localization;
using Mafi.Unity.InputControl.World;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications.Handlers
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class LocationExploredNotificationHandler
  {
    private static readonly ColorRgba BG_COLOR;
    private readonly IInputScheduler m_inputScheduler;
    private readonly WorldMapController m_worldMapController;

    internal LocationExploredNotificationHandler(
      IInputScheduler inputScheduler,
      WorldMapController worldMapController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputScheduler = inputScheduler;
      this.m_worldMapController = worldMapController;
    }

    public void PopulateViewFor(
      LocationExploredMessage notification,
      MessageNotificationView viewToPopulate)
    {
      viewToPopulate.SetData((IMessageNotification) notification, (LocStrFormatted) Tr.Notification__LocationExplored, "Assets/Unity/UserInterface/Toolbar/WorldMap.svg", (Action) (() => this.onClick(notification)));
      viewToPopulate.SetBgColor(LocationExploredNotificationHandler.BG_COLOR);
      if (!this.m_worldMapController.IsActive)
        return;
      this.m_worldMapController.OnLocationExplored(notification);
      this.m_inputScheduler.ScheduleInputCmd<MessageNotificationDismissCmd>(new MessageNotificationDismissCmd(notification.NotificationId));
    }

    private void onClick(LocationExploredMessage notification)
    {
      this.m_worldMapController.OnLocationExplored(notification);
    }

    static LocationExploredNotificationHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LocationExploredNotificationHandler.BG_COLOR = (ColorRgba) 2105926;
    }
  }
}
