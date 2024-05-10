// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.OccupiedTilesExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;

#nullable disable
namespace Mafi.Core.Terrain
{
  public static class OccupiedTilesExtensions
  {
    public static bool Contains(this ImmutableArray<OccupiedTileRelative> tiles, RelTile3i t)
    {
      return TerrainOccupancyManager.OccupiedTilesContains(tiles.AsReadOnlyArray, t);
    }

    public static bool Contains(this ImmutableArray<OccupiedTileRange> tiles, Tile3i t)
    {
      return TerrainOccupancyManager.OccupiedTilesContains(tiles.AsReadOnlyArray, t);
    }

    public static ImmutableArray<Tile3i> ToTile3iArray(this ImmutableArray<OccupiedTileRange> tiles)
    {
      int num1 = 0;
      foreach (OccupiedTileRange tile in tiles)
        num1 += tile.VerticalSize.Value;
      int num2 = 0;
      ImmutableArrayBuilder<Tile3i> immutableArrayBuilder = new ImmutableArrayBuilder<Tile3i>(num1);
      foreach (OccupiedTileRange tile in tiles)
      {
        for (int index = 0; index < tile.VerticalSize.Value; ++index)
          immutableArrayBuilder[num2++] = tile.Position.AsFull.ExtendZ(tile.From.Value + index);
      }
      Assert.That<int>(num2).IsEqualTo(num1);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }
  }
}
