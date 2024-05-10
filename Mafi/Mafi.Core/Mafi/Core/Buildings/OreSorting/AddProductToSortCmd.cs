// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.AddProductToSortCmd
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
namespace Mafi.Core.Buildings.OreSorting
{
  [GenerateSerializer(false, null, 0)]
  public class AddProductToSortCmd : InputCommand
  {
    public readonly EntityId SortingPlantId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AddProductToSortCmd(OreSortingPlant sortingPlant, ProductProto productProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sortingPlant.Id, productProto.Id);
    }

    private AddProductToSortCmd(EntityId sortingPlantId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SortingPlantId = sortingPlantId;
      this.ProductId = productId;
    }

    public static void Serialize(AddProductToSortCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddProductToSortCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddProductToSortCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.SortingPlantId, writer);
    }

    public static AddProductToSortCmd Deserialize(BlobReader reader)
    {
      AddProductToSortCmd productToSortCmd;
      if (reader.TryStartClassDeserialization<AddProductToSortCmd>(out productToSortCmd))
        reader.EnqueueDataDeserialization((object) productToSortCmd, AddProductToSortCmd.s_deserializeDataDelayedAction);
      return productToSortCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddProductToSortCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<AddProductToSortCmd>(this, "SortingPlantId", (object) EntityId.Deserialize(reader));
    }

    static AddProductToSortCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddProductToSortCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AddProductToSortCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
