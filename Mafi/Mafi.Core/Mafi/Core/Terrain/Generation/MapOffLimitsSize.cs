// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapOffLimitsSize
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MapOffLimitsSize
  {
    public readonly RelTile1i MinusX;
    public readonly RelTile1i MinusY;
    public readonly RelTile1i PlusX;
    public readonly RelTile1i PlusY;

    public static void Serialize(MapOffLimitsSize value, BlobWriter writer)
    {
      RelTile1i.Serialize(value.MinusX, writer);
      RelTile1i.Serialize(value.MinusY, writer);
      RelTile1i.Serialize(value.PlusX, writer);
      RelTile1i.Serialize(value.PlusY, writer);
    }

    public static MapOffLimitsSize Deserialize(BlobReader reader)
    {
      return new MapOffLimitsSize(RelTile1i.Deserialize(reader), RelTile1i.Deserialize(reader), RelTile1i.Deserialize(reader), RelTile1i.Deserialize(reader));
    }

    public MapOffLimitsSize(RelTile1i minusX, RelTile1i minusY, RelTile1i plusX, RelTile1i plusY)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.MinusX = minusX;
      this.MinusY = minusY;
      this.PlusX = plusX;
      this.PlusY = plusY;
    }

    public static MapOffLimitsSize Minimal
    {
      get => new MapOffLimitsSize(32.Tiles(), 32.Tiles(), 32.Tiles(), 32.Tiles());
    }

    public static MapOffLimitsSize Default
    {
      get => new MapOffLimitsSize(64.Tiles(), 64.Tiles(), 64.Tiles(), 64.Tiles());
    }
  }
}
