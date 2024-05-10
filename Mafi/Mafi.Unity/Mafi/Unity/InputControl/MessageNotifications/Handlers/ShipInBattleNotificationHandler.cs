// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.ShipInBattleNotificationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

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
  internal class ShipInBattleNotificationHandler
  {
    private static readonly ColorRgba BG_COLOR;
    private readonly WorldMapController m_worldMapController;

    internal ShipInBattleNotificationHandler(WorldMapController worldMapController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_worldMapController = worldMapController;
    }

    public void PopulateViewFor(
      ShipInBattleNotification notification,
      MessageNotificationView viewToPopulate)
    {
      viewToPopulate.SetData((IMessageNotification) notification, (LocStrFormatted) Tr.Notification__ShipInBattle, "Assets/Unity/UserInterface/WorldMap/PirateIcon128.png", (Action) (() => this.onClick(notification)));
      viewToPopulate.SetBgColor(ShipInBattleNotificationHandler.BG_COLOR);
    }

    private void onClick(ShipInBattleNotification notification)
    {
      this.m_worldMapController.OpenBattle();
    }

    static ShipInBattleNotificationHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShipInBattleNotificationHandler.BG_COLOR = (ColorRgba) 9439744;
    }
  }
}
