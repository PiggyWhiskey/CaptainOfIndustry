// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardWorldEntityConstructionToggle
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GenerateSerializer(false, null, 0)]
  public class ShipyardWorldEntityConstructionToggle : InputCommand
  {
    public readonly EntityId ShipyardId;
    public readonly EntityId WorldEntityIdToConstruct;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardWorldEntityConstructionToggle(
      EntityId shipyardId,
      EntityId worldEntityIdToConstruct)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
      this.WorldEntityIdToConstruct = worldEntityIdToConstruct;
    }

    public static void Serialize(ShipyardWorldEntityConstructionToggle value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardWorldEntityConstructionToggle>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardWorldEntityConstructionToggle.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ShipyardId, writer);
      EntityId.Serialize(this.WorldEntityIdToConstruct, writer);
    }

    public static ShipyardWorldEntityConstructionToggle Deserialize(BlobReader reader)
    {
      ShipyardWorldEntityConstructionToggle constructionToggle;
      if (reader.TryStartClassDeserialization<ShipyardWorldEntityConstructionToggle>(out constructionToggle))
        reader.EnqueueDataDeserialization((object) constructionToggle, ShipyardWorldEntityConstructionToggle.s_deserializeDataDelayedAction);
      return constructionToggle;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardWorldEntityConstructionToggle>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
      reader.SetField<ShipyardWorldEntityConstructionToggle>(this, "WorldEntityIdToConstruct", (object) EntityId.Deserialize(reader));
    }

    static ShipyardWorldEntityConstructionToggle()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardWorldEntityConstructionToggle.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardWorldEntityConstructionToggle.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
