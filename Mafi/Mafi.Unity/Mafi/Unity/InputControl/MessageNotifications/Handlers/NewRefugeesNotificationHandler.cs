// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.NewRefugeesNotificationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Localization;
using Mafi.Unity.InputControl.Population;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications.Handlers
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class NewRefugeesNotificationHandler
  {
    private readonly NewRefugeesInfoView m_view;

    internal NewRefugeesNotificationHandler(NewRefugeesInfoView newRefugeesInfoView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_view = newRefugeesInfoView;
    }

    public void PopulateViewFor(
      NewRefugeesMessage notification,
      MessageNotificationView viewToPopulate)
    {
      viewToPopulate.SetData((IMessageNotification) notification, (LocStrFormatted) Tr.Notification__NewRefugees, "Assets/Unity/UserInterface/General/Population.svg", (Action) (() => this.onClick(notification)));
      viewToPopulate.SetDefaultBgColor();
    }

    private void onClick(NewRefugeesMessage notification)
    {
      this.m_view.ShowRefugeesMessage(notification);
    }
  }
}
