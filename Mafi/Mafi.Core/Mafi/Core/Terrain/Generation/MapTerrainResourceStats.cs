// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapTerrainResourceStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MapTerrainResourceStats
  {
    public readonly Proto.ID MaterialProtoId;
    public readonly long VolumeTilesCubed;

    public static void Serialize(MapTerrainResourceStats value, BlobWriter writer)
    {
      Proto.ID.Serialize(value.MaterialProtoId, writer);
      writer.WriteLong(value.VolumeTilesCubed);
    }

    public static MapTerrainResourceStats Deserialize(BlobReader reader)
    {
      return new MapTerrainResourceStats(Proto.ID.Deserialize(reader), reader.ReadLong());
    }

    public MapTerrainResourceStats(Proto.ID materialProtoId, long volumeTilesCubed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.MaterialProtoId = materialProtoId;
      this.VolumeTilesCubed = volumeTilesCubed;
    }
  }
}
