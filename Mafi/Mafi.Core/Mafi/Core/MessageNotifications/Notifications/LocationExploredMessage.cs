// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.LocationExploredMessage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Research;
using Mafi.Core.World;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class LocationExploredMessage : IMessageNotification
  {
    public readonly WorldMapLocation Location;
    public readonly Option<WorldMapLoot> Loot;
    public readonly ImmutableArray<TechnologyProto> UnlockedProtos;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public LocationExploredMessage(
      WorldMapLocation location,
      Option<WorldMapLoot> loot,
      ImmutableArray<TechnologyProto> unlockedProtos)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Location = location;
      this.Loot = loot;
      this.UnlockedProtos = unlockedProtos;
    }

    public static void Serialize(LocationExploredMessage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LocationExploredMessage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LocationExploredMessage.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      WorldMapLocation.Serialize(this.Location, writer);
      Option<WorldMapLoot>.Serialize(this.Loot, writer);
      MessageNotificationId.Serialize(this.NotificationId, writer);
      ImmutableArray<TechnologyProto>.Serialize(this.UnlockedProtos, writer);
    }

    public static LocationExploredMessage Deserialize(BlobReader reader)
    {
      LocationExploredMessage locationExploredMessage;
      if (reader.TryStartClassDeserialization<LocationExploredMessage>(out locationExploredMessage))
        reader.EnqueueDataDeserialization((object) locationExploredMessage, LocationExploredMessage.s_deserializeDataDelayedAction);
      return locationExploredMessage;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<LocationExploredMessage>(this, "Location", (object) WorldMapLocation.Deserialize(reader));
      reader.SetField<LocationExploredMessage>(this, "Loot", (object) Option<WorldMapLoot>.Deserialize(reader));
      this.NotificationId = MessageNotificationId.Deserialize(reader);
      reader.SetField<LocationExploredMessage>(this, "UnlockedProtos", (object) ImmutableArray<TechnologyProto>.Deserialize(reader));
    }

    static LocationExploredMessage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LocationExploredMessage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LocationExploredMessage) obj).SerializeData(writer));
      LocationExploredMessage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LocationExploredMessage) obj).DeserializeData(reader));
    }
  }
}
