// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.IMessageNotificationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Messages;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.MessageNotifications
{
  public interface IMessageNotificationsManager
  {
    /// <summary>
    /// Raised when message notification should be added to the UI.
    /// </summary>
    event Action<IMessageNotification> OnNotificationAdded;

    /// <summary>
    /// Raised when message notification should be removed from the UI.
    /// </summary>
    event Action<IMessageNotification> OnNotificationRemoved;

    IReadOnlyCollection<IMessageNotification> AllNotifications { get; }

    void AddMessage(IMessageNotification message);

    void DismissNotificationForMessageIfExists(MessageProto messageProto);

    void DismissAllNotifications();
  }
}
