// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.ResearchNotificationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;
using Mafi.Unity.InputControl.Research;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications.Handlers
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class ResearchNotificationHandler : IUnityUi
  {
    private readonly ResearchFinishedMessageWindowView m_view;

    internal ResearchNotificationHandler(
      IUnityInputMgr inputMgr,
      ResearchController researchController,
      ResearchManager researchManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_view = new ResearchFinishedMessageWindowView(inputMgr, researchController, researchManager);
    }

    public void PopulateViewFor(
      ResearchFinishedMessage notification,
      MessageNotificationView viewToPopulate)
    {
      viewToPopulate.SetData((IMessageNotification) notification, (LocStrFormatted) notification.ResearchNode.Proto.Strings.Name, "Assets/Unity/UserInterface/Toolbar/Research.svg", (Action) (() => this.onClick(notification)));
      viewToPopulate.SetDefaultBgColor();
    }

    private void onClick(ResearchFinishedMessage notification)
    {
      this.m_view.ShowNewLevelInfo(notification.ResearchNode.Proto.Strings.Name.TranslatedString, (IEnumerable<Proto>) notification.UnlockedProtos);
    }

    void IUnityUi.RegisterUi(UiBuilder builder) => this.m_view.BuildUi(builder);
  }
}
