// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationType
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Notifications
{
  public enum NotificationType
  {
    /// <summary>
    /// Notification that is reported frequently and is also removed by the entity that reported it. It is usually
    /// about some state that holds for some amount of time like: "Not enough electricity". If the player dismisses
    /// such notification it is only internally suppressed (not removed) so it can be reported every update and still
    /// won't bother the player.
    /// </summary>
    Continuous,
    /// <summary>
    /// Notification that is reported only once. And it is not expected from the entity that reported it to remove
    /// it. E.g. "Achievement unlocked". Notification is removed on dismiss so reporting this every update would spam
    /// the user heavily.
    /// </summary>
    OneTimeOnly,
  }
}
