// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.AddProductToDumpCmd
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
namespace Mafi.Core.Buildings.Mine
{
  [GenerateSerializer(false, null, 0)]
  public class AddProductToDumpCmd : InputCommand
  {
    /// <summary>If null, product is assigned globally</summary>
    public readonly EntityId? MineTowerId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AddProductToDumpCmd(Option<MineTower> mineTower, ProductProto productProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(mineTower.ValueOrNull?.Id, new ProductProto.ID(productProto.Id.Value));
    }

    private AddProductToDumpCmd(EntityId? mineTowerId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MineTowerId = mineTowerId;
      this.ProductId = productId;
    }

    public static void Serialize(AddProductToDumpCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddProductToDumpCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddProductToDumpCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<EntityId>(this.MineTowerId);
      ProductProto.ID.Serialize(this.ProductId, writer);
    }

    public static AddProductToDumpCmd Deserialize(BlobReader reader)
    {
      AddProductToDumpCmd productToDumpCmd;
      if (reader.TryStartClassDeserialization<AddProductToDumpCmd>(out productToDumpCmd))
        reader.EnqueueDataDeserialization((object) productToDumpCmd, AddProductToDumpCmd.s_deserializeDataDelayedAction);
      return productToDumpCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddProductToDumpCmd>(this, "MineTowerId", (object) reader.ReadNullableStruct<EntityId>());
      reader.SetField<AddProductToDumpCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
    }

    static AddProductToDumpCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddProductToDumpCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AddProductToDumpCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
