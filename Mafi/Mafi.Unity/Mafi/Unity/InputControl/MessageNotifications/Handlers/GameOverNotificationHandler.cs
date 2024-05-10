// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.GameOverNotificationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications.Handlers
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class GameOverNotificationHandler
  {
    private static readonly ColorRgba BG_COLOR;
    private readonly GameOverMessageView m_view;

    internal GameOverNotificationHandler(GameOverMessageView view)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_view = view;
    }

    public void PopulateViewFor(
      GameOverNotification notification,
      MessageNotificationView viewToPopulate)
    {
      viewToPopulate.SetData((IMessageNotification) notification, notification.Title, "Assets/Unity/UserInterface/General/Warning128.png", (Action) (() => this.onClick(notification)));
      viewToPopulate.SetBgColor(GameOverNotificationHandler.BG_COLOR);
      this.m_view.ShowGameOverNotification(notification);
    }

    private void onClick(GameOverNotification notification)
    {
      this.m_view.ShowGameOverNotification(notification);
    }

    static GameOverNotificationHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameOverNotificationHandler.BG_COLOR = (ColorRgba) 12724525;
    }
  }
}
