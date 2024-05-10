// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.Notificator
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
  [GenerateSerializer(false, null, 0)]
  public struct Notificator
  {
    private readonly NotificationProto m_prototype;
    private readonly INotificationsManager m_notificationsManager;

    public readonly bool IsValid => (Proto) this.m_prototype != (Proto) null;

    public readonly bool IsActive => this.NotificationId.IsValid;

    public NotificationId NotificationId { get; private set; }

    public Notificator(INotificationsManager notificationsManager, NotificationProto prototype)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<bool>(prototype.Type == NotificationType.Continuous).IsTrue(string.Format("Notification {0} is not continuous and cannot be used in Notificator.", (object) prototype.Id));
      this.NotificationId = NotificationId.Invalid;
      this.m_notificationsManager = notificationsManager.CheckNotNull<INotificationsManager>();
      this.m_prototype = prototype.CheckNotNull<NotificationProto>();
    }

    [LoadCtor]
    private Notificator(
      INotificationsManager notificationsManager,
      NotificationProto prototype,
      NotificationId notificationId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_notificationsManager = notificationsManager;
      this.m_prototype = prototype;
      this.NotificationId = notificationId;
    }

    public void NotifyIff(bool shouldNotify)
    {
      if (shouldNotify == this.NotificationId.IsValid)
        return;
      if (shouldNotify)
      {
        if ((Proto) this.m_prototype == (Proto) null)
          Log.Error("Trying to notify on uninitialized struct.");
        else
          this.NotificationId = this.m_notificationsManager.AddNotification(this.m_prototype, (Option<IEntity>) Option.None, Option<object>.None);
      }
      else
      {
        this.m_notificationsManager.RemoveNotification(this.NotificationId);
        this.NotificationId = NotificationId.Invalid;
      }
    }

    public void Activate() => this.NotifyIff(true);

    public void Deactivate() => this.NotifyIff(false);

    public static void Serialize(Notificator value, BlobWriter writer)
    {
      writer.WriteGeneric<INotificationsManager>(value.m_notificationsManager);
      writer.WriteGeneric<NotificationProto>(value.m_prototype);
      NotificationId.Serialize(value.NotificationId, writer);
    }

    public static Notificator Deserialize(BlobReader reader)
    {
      return new Notificator(reader.ReadGenericAs<INotificationsManager>(), reader.ReadGenericAs<NotificationProto>(), NotificationId.Deserialize(reader));
    }
  }
}
