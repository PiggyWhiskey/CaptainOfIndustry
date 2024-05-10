// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.GameOverNotification
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class GameOverNotification : IMessageNotification
  {
    public readonly LocStrFormatted Title;
    public readonly LocStrFormatted Message;
    public readonly bool CanBeDismissed;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public GameOverNotification(
      LocStrFormatted title,
      LocStrFormatted message,
      bool canBeDismissed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Title = title;
      this.Message = message;
      this.CanBeDismissed = canBeDismissed;
    }

    public static void Serialize(GameOverNotification value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameOverNotification>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameOverNotification.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.CanBeDismissed);
      LocStrFormatted.Serialize(this.Message, writer);
      MessageNotificationId.Serialize(this.NotificationId, writer);
      LocStrFormatted.Serialize(this.Title, writer);
    }

    public static GameOverNotification Deserialize(BlobReader reader)
    {
      GameOverNotification overNotification;
      if (reader.TryStartClassDeserialization<GameOverNotification>(out overNotification))
        reader.EnqueueDataDeserialization((object) overNotification, GameOverNotification.s_deserializeDataDelayedAction);
      return overNotification;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GameOverNotification>(this, "CanBeDismissed", (object) reader.ReadBool());
      reader.SetField<GameOverNotification>(this, "Message", (object) LocStrFormatted.Deserialize(reader));
      this.NotificationId = MessageNotificationId.Deserialize(reader);
      reader.SetField<GameOverNotification>(this, "Title", (object) LocStrFormatted.Deserialize(reader));
    }

    static GameOverNotification()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameOverNotification.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameOverNotification) obj).SerializeData(writer));
      GameOverNotification.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameOverNotification) obj).DeserializeData(reader));
    }
  }
}
