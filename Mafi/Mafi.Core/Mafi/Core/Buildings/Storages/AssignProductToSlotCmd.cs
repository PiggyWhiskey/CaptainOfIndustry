// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.AssignProductToSlotCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class AssignProductToSlotCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly ProductProto.ID? ProductId;
    public readonly int Slot;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AssignProductToSlotCmd(
      IEntityWithMultipleProductsToAssign storage,
      Option<ProductProto> product,
      int slot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, product.ValueOrNull?.Id, slot);
    }

    public AssignProductToSlotCmd(EntityId entityId, ProductProto.ID? productId, int slot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.ProductId = productId;
      this.Slot = slot;
    }

    public static void Serialize(AssignProductToSlotCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignProductToSlotCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignProductToSlotCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteNullableStruct<ProductProto.ID>(this.ProductId);
      writer.WriteInt(this.Slot);
    }

    public static AssignProductToSlotCmd Deserialize(BlobReader reader)
    {
      AssignProductToSlotCmd productToSlotCmd;
      if (reader.TryStartClassDeserialization<AssignProductToSlotCmd>(out productToSlotCmd))
        reader.EnqueueDataDeserialization((object) productToSlotCmd, AssignProductToSlotCmd.s_deserializeDataDelayedAction);
      return productToSlotCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AssignProductToSlotCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<AssignProductToSlotCmd>(this, "ProductId", (object) reader.ReadNullableStruct<ProductProto.ID>());
      reader.SetField<AssignProductToSlotCmd>(this, "Slot", (object) reader.ReadInt());
    }

    static AssignProductToSlotCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignProductToSlotCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AssignProductToSlotCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
