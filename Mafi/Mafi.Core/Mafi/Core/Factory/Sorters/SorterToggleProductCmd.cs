// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Sorters.SorterToggleProductCmd
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
namespace Mafi.Core.Factory.Sorters
{
  [GenerateSerializer(false, null, 0)]
  public class SorterToggleProductCmd : InputCommand
  {
    public readonly EntityId SorterId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SorterToggleProductCmd(Sorter sorter, ProductProto productProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sorter.Id, new ProductProto.ID(productProto.Id.Value));
    }

    public SorterToggleProductCmd(EntityId sorterId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SorterId = sorterId;
      this.ProductId = productId;
    }

    public static void Serialize(SorterToggleProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SorterToggleProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SorterToggleProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.SorterId, writer);
    }

    public static SorterToggleProductCmd Deserialize(BlobReader reader)
    {
      SorterToggleProductCmd toggleProductCmd;
      if (reader.TryStartClassDeserialization<SorterToggleProductCmd>(out toggleProductCmd))
        reader.EnqueueDataDeserialization((object) toggleProductCmd, SorterToggleProductCmd.s_deserializeDataDelayedAction);
      return toggleProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SorterToggleProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<SorterToggleProductCmd>(this, "SorterId", (object) EntityId.Deserialize(reader));
    }

    static SorterToggleProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SorterToggleProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SorterToggleProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
