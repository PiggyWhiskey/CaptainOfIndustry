// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapResourceLocation
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
  public readonly struct MapResourceLocation
  {
    public readonly ProductProto.ID ProductProtoId;
    public readonly Tile3f Position;

    public static void Serialize(MapResourceLocation value, BlobWriter writer)
    {
      ProductProto.ID.Serialize(value.ProductProtoId, writer);
      Tile3f.Serialize(value.Position, writer);
    }

    public static MapResourceLocation Deserialize(BlobReader reader)
    {
      return new MapResourceLocation(ProductProto.ID.Deserialize(reader), Tile3f.Deserialize(reader));
    }

    public MapResourceLocation(ProductProto.ID productProtoId, Tile3f position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProductProtoId = productProtoId;
      this.Position = position;
    }
  }
}
