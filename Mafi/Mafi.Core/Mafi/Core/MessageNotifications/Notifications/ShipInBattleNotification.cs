// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.ShipInBattleNotification
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Fleet;
using Mafi.Core.World;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class ShipInBattleNotification : IMessageNotification
  {
    public readonly WorldMapLocation Location;
    public readonly IBattleState BattleState;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public ShipInBattleNotification(WorldMapLocation location, IBattleState battleState)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Location = location;
      this.BattleState = battleState;
    }

    public static void Serialize(ShipInBattleNotification value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipInBattleNotification>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipInBattleNotification.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IBattleState>(this.BattleState);
      WorldMapLocation.Serialize(this.Location, writer);
      MessageNotificationId.Serialize(this.NotificationId, writer);
    }

    public static ShipInBattleNotification Deserialize(BlobReader reader)
    {
      ShipInBattleNotification battleNotification;
      if (reader.TryStartClassDeserialization<ShipInBattleNotification>(out battleNotification))
        reader.EnqueueDataDeserialization((object) battleNotification, ShipInBattleNotification.s_deserializeDataDelayedAction);
      return battleNotification;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ShipInBattleNotification>(this, "BattleState", (object) reader.ReadGenericAs<IBattleState>());
      reader.SetField<ShipInBattleNotification>(this, "Location", (object) WorldMapLocation.Deserialize(reader));
      this.NotificationId = MessageNotificationId.Deserialize(reader);
    }

    static ShipInBattleNotification()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipInBattleNotification.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ShipInBattleNotification) obj).SerializeData(writer));
      ShipInBattleNotification.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ShipInBattleNotification) obj).DeserializeData(reader));
    }
  }
}
