// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapProductStats
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
  public readonly struct MapProductStats
  {
    public readonly ProductProto.ID ProductProtoId;
    public readonly string DisplayName;
    public readonly QuantityLarge Quantity;

    public static void Serialize(MapProductStats value, BlobWriter writer)
    {
      ProductProto.ID.Serialize(value.ProductProtoId, writer);
      writer.WriteString(value.DisplayName);
      QuantityLarge.Serialize(value.Quantity, writer);
    }

    public static MapProductStats Deserialize(BlobReader reader)
    {
      return new MapProductStats(ProductProto.ID.Deserialize(reader), reader.ReadString(), QuantityLarge.Deserialize(reader));
    }

    public MapProductStats(
      ProductProto.ID productProtoId,
      string displayName,
      QuantityLarge quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProductProtoId = productProtoId;
      this.DisplayName = displayName;
      this.Quantity = quantity;
    }
  }
}
