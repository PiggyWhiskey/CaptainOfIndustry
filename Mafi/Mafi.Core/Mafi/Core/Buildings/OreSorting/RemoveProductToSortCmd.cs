// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.RemoveProductToSortCmd
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
  public class RemoveProductToSortCmd : InputCommand
  {
    public readonly EntityId SortingPlantId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RemoveProductToSortCmd(OreSortingPlant sortingPlant, ProductProto productProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sortingPlant.Id, productProto.Id);
    }

    private RemoveProductToSortCmd(EntityId sortingPlantId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SortingPlantId = sortingPlantId;
      this.ProductId = productId;
    }

    public static void Serialize(RemoveProductToSortCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveProductToSortCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveProductToSortCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.SortingPlantId, writer);
    }

    public static RemoveProductToSortCmd Deserialize(BlobReader reader)
    {
      RemoveProductToSortCmd productToSortCmd;
      if (reader.TryStartClassDeserialization<RemoveProductToSortCmd>(out productToSortCmd))
        reader.EnqueueDataDeserialization((object) productToSortCmd, RemoveProductToSortCmd.s_deserializeDataDelayedAction);
      return productToSortCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveProductToSortCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<RemoveProductToSortCmd>(this, "SortingPlantId", (object) EntityId.Deserialize(reader));
    }

    static RemoveProductToSortCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveProductToSortCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RemoveProductToSortCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
