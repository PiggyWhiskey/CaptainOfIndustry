// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.NewRefugeesMessage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class NewRefugeesMessage : IMessageNotification
  {
    public readonly int AmountOfRefugeesAdded;
    public readonly ImmutableArray<ProductQuantity> Reward;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public NewRefugeesMessage(int amountOfRefugeesAdded, ImmutableArray<ProductQuantity> reward)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AmountOfRefugeesAdded = amountOfRefugeesAdded;
      this.Reward = reward;
    }

    public static void Serialize(NewRefugeesMessage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NewRefugeesMessage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NewRefugeesMessage.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.AmountOfRefugeesAdded);
      MessageNotificationId.Serialize(this.NotificationId, writer);
      ImmutableArray<ProductQuantity>.Serialize(this.Reward, writer);
    }

    public static NewRefugeesMessage Deserialize(BlobReader reader)
    {
      NewRefugeesMessage newRefugeesMessage;
      if (reader.TryStartClassDeserialization<NewRefugeesMessage>(out newRefugeesMessage))
        reader.EnqueueDataDeserialization((object) newRefugeesMessage, NewRefugeesMessage.s_deserializeDataDelayedAction);
      return newRefugeesMessage;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<NewRefugeesMessage>(this, "AmountOfRefugeesAdded", (object) reader.ReadInt());
      this.NotificationId = MessageNotificationId.Deserialize(reader);
      reader.SetField<NewRefugeesMessage>(this, "Reward", (object) ImmutableArray<ProductQuantity>.Deserialize(reader));
    }

    static NewRefugeesMessage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NewRefugeesMessage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NewRefugeesMessage) obj).SerializeData(writer));
      NewRefugeesMessage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NewRefugeesMessage) obj).DeserializeData(reader));
    }
  }
}
