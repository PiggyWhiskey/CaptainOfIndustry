// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.PinnedProductReorderCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  [GenerateSerializer(false, null, 0)]
  public class PinnedProductReorderCmd : InputCommand
  {
    public readonly ProductProto.ID ProductToMove;
    public readonly int NewIndex;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PinnedProductReorderCmd(ProductProto.ID productToMove, int newIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductToMove = productToMove;
      this.NewIndex = newIndex;
    }

    public static void Serialize(PinnedProductReorderCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PinnedProductReorderCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PinnedProductReorderCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.NewIndex);
      ProductProto.ID.Serialize(this.ProductToMove, writer);
    }

    public static PinnedProductReorderCmd Deserialize(BlobReader reader)
    {
      PinnedProductReorderCmd productReorderCmd;
      if (reader.TryStartClassDeserialization<PinnedProductReorderCmd>(out productReorderCmd))
        reader.EnqueueDataDeserialization((object) productReorderCmd, PinnedProductReorderCmd.s_deserializeDataDelayedAction);
      return productReorderCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PinnedProductReorderCmd>(this, "NewIndex", (object) reader.ReadInt());
      reader.SetField<PinnedProductReorderCmd>(this, "ProductToMove", (object) ProductProto.ID.Deserialize(reader));
    }

    static PinnedProductReorderCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PinnedProductReorderCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      PinnedProductReorderCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
