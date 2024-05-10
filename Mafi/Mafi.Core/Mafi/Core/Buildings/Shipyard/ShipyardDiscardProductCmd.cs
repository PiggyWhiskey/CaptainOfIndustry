// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardDiscardProductCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GenerateSerializer(false, null, 0)]
  public class ShipyardDiscardProductCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardDiscardProductCmd(EntityId shipyardId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
      this.ProductId = productId;
    }

    public static void Serialize(ShipyardDiscardProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardDiscardProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardDiscardProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipyardDiscardProductCmd Deserialize(BlobReader reader)
    {
      ShipyardDiscardProductCmd discardProductCmd;
      if (reader.TryStartClassDeserialization<ShipyardDiscardProductCmd>(out discardProductCmd))
        reader.EnqueueDataDeserialization((object) discardProductCmd, ShipyardDiscardProductCmd.s_deserializeDataDelayedAction);
      return discardProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardDiscardProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<ShipyardDiscardProductCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipyardDiscardProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardDiscardProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardDiscardProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
