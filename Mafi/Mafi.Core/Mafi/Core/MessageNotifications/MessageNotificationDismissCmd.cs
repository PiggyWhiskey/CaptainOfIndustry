// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.MessageNotificationDismissCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications
{
  [GenerateSerializer(false, null, 0)]
  public class MessageNotificationDismissCmd : InputCommand
  {
    public MessageNotificationId MessageId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationDismissCmd(MessageNotificationId messageId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(messageId.IsValid).IsTrue();
      this.MessageId = messageId;
    }

    public static void Serialize(MessageNotificationDismissCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageNotificationDismissCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageNotificationDismissCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      MessageNotificationId.Serialize(this.MessageId, writer);
    }

    public static MessageNotificationDismissCmd Deserialize(BlobReader reader)
    {
      MessageNotificationDismissCmd notificationDismissCmd;
      if (reader.TryStartClassDeserialization<MessageNotificationDismissCmd>(out notificationDismissCmd))
        reader.EnqueueDataDeserialization((object) notificationDismissCmd, MessageNotificationDismissCmd.s_deserializeDataDelayedAction);
      return notificationDismissCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.MessageId = MessageNotificationId.Deserialize(reader);
    }

    static MessageNotificationDismissCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageNotificationDismissCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MessageNotificationDismissCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
