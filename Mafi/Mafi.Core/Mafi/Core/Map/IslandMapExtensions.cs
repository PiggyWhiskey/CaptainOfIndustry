// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IslandMapExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Terrain.Generation;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  public static class IslandMapExtensions
  {
    public static DebugGameMapDrawing DrawMap(this IslandMap map)
    {
      DebugGameMapDrawing debugGameMapDrawing = DebugGameRenderer.StartMapDrawing(Tile2i.Zero, new RelTile2i(map.TerrainWidth, map.TerrainHeight), 1, true);
      debugGameMapDrawing.DrawGrid(64, new ColorRgba((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 32));
      foreach (MapCell cell in map.Cells)
      {
        int index1 = cell.PerimeterIndices.Last;
        debugGameMapDrawing.DrawCross(cell.CenterTile.CenterTile2f, ColorRgba.Yellow);
        debugGameMapDrawing.DrawString(cell.CenterTile.AddY(-5).CenterTile2f, cell.Id.ToString(), ColorRgba.White, centered: true);
        for (int index2 = 0; index2 < cell.PerimeterIndices.Length; ++index2)
        {
          int perimeterIndex = cell.PerimeterIndices[index2];
          debugGameMapDrawing.DrawLine(map.CellEdgePoints[index1], map.CellEdgePoints[perimeterIndex], ColorRgba.Yellow, true);
          index1 = perimeterIndex;
        }
      }
      foreach (ITerrainResourceGenerator resourcesGenerator in map.ResourcesGenerators)
      {
        debugGameMapDrawing.DrawString(resourcesGenerator.Position.Xy.CenterTile2f, resourcesGenerator.Name, resourcesGenerator.ResourceColor, centered: true);
        debugGameMapDrawing.DrawCircle(resourcesGenerator.Position.Xy.CenterTile2f, resourcesGenerator.MaxRadius, resourcesGenerator.ResourceColor);
      }
      foreach (IVirtualTerrainResource virtualResource in map.VirtualResources)
      {
        debugGameMapDrawing.DrawString(virtualResource.Position.Xy.CenterTile2f, virtualResource.Name, virtualResource.ResourceColor, centered: true);
        debugGameMapDrawing.DrawCircle(virtualResource.Position.Xy.CenterTile2f, virtualResource.MaxRadius, virtualResource.ResourceColor);
      }
      debugGameMapDrawing.DrawCross(map.StartingLocation.Position.CenterTile2f, ColorRgba.Green);
      debugGameMapDrawing.DrawString(map.StartingLocation.Position.CenterTile2f.AddY((Fix32) -5), "Start", ColorRgba.Green, centered: true);
      debugGameMapDrawing.DrawStringTopLeft(string.Format("Map: {0} {1} x {2}, ", (object) map.MapName, (object) map.TerrainWidth, (object) map.TerrainHeight) + string.Format("cells: {0},\n", (object) map.Cells.Length) + string.Format("{0} resources: ", (object) map.ResourcesGenerators.Length) + map.ResourcesGenerators.Select<string>((Func<ITerrainResourceGenerator, string>) (x => x.Name)).JoinStrings(",") + "\n" + string.Format("{0} virtual: ", (object) map.VirtualResources.Length) + map.VirtualResources.Select<string>((Func<IVirtualTerrainResource, string>) (x => x.Name)).JoinStrings(","), ColorRgba.White);
      return debugGameMapDrawing;
    }
  }
}
