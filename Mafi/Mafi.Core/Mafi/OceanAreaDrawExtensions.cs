// Decompiled with JetBrains decompiler
// Type: Mafi.OceanAreaDrawExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class OceanAreaDrawExtensions
  {
    public static DebugGameMapDrawing DrawOceanAreas(this DebugGameMapDrawing drawing)
    {
      if (drawing.Resolver.IsNone)
        return drawing;
      StaticEntityOceanReservationManager reservationManager = drawing.Resolver.Value.Resolve<StaticEntityOceanReservationManager>();
      TerrainManager terrainManager = drawing.Resolver.Value.Resolve<TerrainManager>();
      TerrainOccupancyManager occupancyManager = drawing.Resolver.Value.Resolve<TerrainOccupancyManager>();
      Dict<Tile2i, int> dict = new Dict<Tile2i, int>();
      foreach (IOceanAreaRecord monitoredArea in reservationManager.MonitoredAreas)
      {
        foreach (Tile2iAndIndex enumerateTilesAndIndex in monitoredArea.Area.EnumerateTilesAndIndices(terrainManager))
        {
          if (drawing.IsTileOnMap(enumerateTilesAndIndex.TileCoord))
          {
            bool exists;
            ref int local = ref dict.GetRefValue(enumerateTilesAndIndex.TileCoord, out exists);
            if (!exists)
            {
              if (!terrainManager.HasAnyTileFlagSet(enumerateTilesAndIndex.Index, 4U))
                local = 1;
              else if (terrainManager.GetHeight(enumerateTilesAndIndex.Index) > StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT)
                local = 1;
              else if (occupancyManager.IsOccupied(enumerateTilesAndIndex.Index))
                local = 2;
            }
          }
        }
      }
      ColorRgba[] colorRgbaArray = new ColorRgba[3]
      {
        new ColorRgba(65280, 192),
        new ColorRgba(16776960, 192),
        new ColorRgba(16711680, 192)
      };
      foreach (KeyValuePair<Tile2i, int> keyValuePair in dict)
        drawing.HighlightTile(keyValuePair.Key, colorRgbaArray[keyValuePair.Value]);
      return drawing;
    }
  }
}
