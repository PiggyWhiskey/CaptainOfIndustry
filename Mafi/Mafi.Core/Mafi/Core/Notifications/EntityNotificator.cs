// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.EntityNotificator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Notifications
{
  /// <summary>
  /// Convenience wrapper for handling a given notification of type <see cref="F:Mafi.Core.Notifications.NotificationType.Continuous" />. Main
  /// purpose is to reduce boilerplate code in classes using notifications while providing caching.
  /// IMPORTANT: This is a mutable struct (for perf reasons), do not keep it in readonly fields and no nullables!
  /// </summary>
  [ManuallyWrittenSerialization]
  public struct EntityNotificator
  {
    public readonly NotificationProto Prototype;

    public readonly bool IsValid => (Proto) this.Prototype != (Proto) null;

    public readonly bool IsActive => this.NotificationId.IsValid;

    public NotificationId NotificationId { get; private set; }

    public EntityNotificator(NotificationProto prototype)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<bool>(prototype.Type == NotificationType.Continuous).IsTrue(string.Format("Notification {0} is not continuous and cannot be used in Notificator.", (object) prototype.Id));
      this.NotificationId = NotificationId.Invalid;
      this.Prototype = prototype.CheckNotNull<NotificationProto>();
    }

    [LoadCtor]
    private EntityNotificator(NotificationProto prototype, NotificationId notificationId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Prototype = prototype;
      this.NotificationId = notificationId;
    }

    /// <summary>
    /// If true given it will send a notification (if it wasn't already send). If false given it will remove
    /// notification if the notification was send previously and wasn't removed yet.
    /// </summary>
    public void NotifyIff(bool shouldNotify, IEntity entity)
    {
      if (shouldNotify == this.NotificationId.IsValid)
        return;
      if (shouldNotify)
      {
        if ((Proto) this.Prototype == (Proto) null)
          Log.Error("Trying to notify on uninitialized struct.");
        else
          this.NotificationId = entity.Context.NotificationsManager.AddNotification(this.Prototype, entity.CreateOption<IEntity>(), (Option<object>) Option.None);
      }
      else
      {
        entity.Context.NotificationsManager.RemoveNotification(this.NotificationId);
        this.NotificationId = NotificationId.Invalid;
      }
    }

    public void Activate(IEntity entity) => this.NotifyIff(true, entity);

    public void Deactivate(IEntity entity)
    {
      if (!this.NotificationId.IsValid)
        return;
      entity.Context.NotificationsManager.RemoveNotification(this.NotificationId);
      this.NotificationId = NotificationId.Invalid;
    }

    [OnlyForSaveCompatibility(null)]
    public static void Serialize(EntityNotificator value, BlobWriter writer)
    {
      bool flag = (Proto) value.Prototype == (Proto) null;
      writer.WriteBool(flag);
      if (flag)
        return;
      writer.WriteGeneric<NotificationProto>(value.Prototype);
      NotificationId.Serialize(value.NotificationId, writer);
    }

    [OnlyForSaveCompatibility(null)]
    public static EntityNotificator Deserialize(BlobReader reader)
    {
      return reader.LoadedSaveVersion >= 101 && reader.ReadBool() ? new EntityNotificator() : new EntityNotificator(reader.ReadGenericAs<NotificationProto>(), NotificationId.Deserialize(reader));
    }
  }
}
