// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapOtherResourceStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MapOtherResourceStats
  {
    public readonly ProductProto.ID ProductProtoId;
    public readonly QuantityLarge Quantity;

    public static void Serialize(MapOtherResourceStats value, BlobWriter writer)
    {
      ProductProto.ID.Serialize(value.ProductProtoId, writer);
      QuantityLarge.Serialize(value.Quantity, writer);
    }

    public static MapOtherResourceStats Deserialize(BlobReader reader)
    {
      return new MapOtherResourceStats(ProductProto.ID.Deserialize(reader), QuantityLarge.Deserialize(reader));
    }

    public MapOtherResourceStats(ProductProto.ID productProtoId, QuantityLarge quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProductProtoId = productProtoId;
      this.Quantity = quantity;
    }
  }
}
