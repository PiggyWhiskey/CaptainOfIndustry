// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationDismissCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class NotificationDismissCmd : InputCommand
  {
    public ImmutableArray<NotificationId> NotificationsIds;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public NotificationDismissCmd(ImmutableArray<NotificationId> notificationsIds)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NotificationsIds = notificationsIds;
    }

    public static void Serialize(NotificationDismissCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NotificationDismissCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NotificationDismissCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<NotificationId>.Serialize(this.NotificationsIds, writer);
    }

    public static NotificationDismissCmd Deserialize(BlobReader reader)
    {
      NotificationDismissCmd notificationDismissCmd;
      if (reader.TryStartClassDeserialization<NotificationDismissCmd>(out notificationDismissCmd))
        reader.EnqueueDataDeserialization((object) notificationDismissCmd, NotificationDismissCmd.s_deserializeDataDelayedAction);
      return notificationDismissCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.NotificationsIds = ImmutableArray<NotificationId>.Deserialize(reader);
    }

    static NotificationDismissCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NotificationDismissCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NotificationDismissCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
