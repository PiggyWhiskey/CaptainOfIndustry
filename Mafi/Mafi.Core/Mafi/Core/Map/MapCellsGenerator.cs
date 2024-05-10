// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellsGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Vornoi;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Helper class that generates map cells from given points using Vornoi segmentation algorithm. Keep in mind that
  /// points that will not form fully enclosed Vornoi cells will not end up being map cells.
  /// </summary>
  public static class MapCellsGenerator
  {
    public static ImmutableArray<MapCell> GenerateCellsFromPoints(
      IEnumerable<Tile2i> cellCenters,
      RelTile1i minEdgeLength,
      out ImmutableArray<Tile2i> perimeterPoints)
    {
      Lyst<Vector2i> lystOf = cellCenters.ToLystOf<Tile2i, Vector2i>((Func<Tile2i, Vector2i>) (x => x.Vector2i));
      Lyst<DelaunayTriangle2i> delaunayTriangulation = DelaunayBowyerWatson.ComputeDelaunayTriangulation((IIndexable<Vector2i>) lystOf);
      Lyst<Vector2i> vornoiPoints;
      Lyst<VornoiCell> vornoi = VornoiFromDelaunay.ComputeVornoi((IIndexable<Vector2i>) lystOf, (IIndexable<DelaunayTriangle2i>) delaunayTriangulation, minEdgeLength.Value, out vornoiPoints);
      ImmutableArrayBuilder<MapCell> immutableArrayBuilder = new ImmutableArrayBuilder<MapCell>(vornoi.Count);
      int?[] ptIndexToCellIndex = new int?[lystOf.Count];
      for (int index = 0; index < vornoi.Count; ++index)
      {
        VornoiCell vornoiCell = vornoi[index];
        ptIndexToCellIndex[vornoiCell.PointIndex] = new int?(index);
      }
      for (int index = 0; index < vornoi.Count; ++index)
      {
        VornoiCell vornoiCell = vornoi[index];
        MapCell mapCell = new MapCell(new MapCellId(index), vornoiCell.PointIndex, vornoiCell.Perimeter, vornoiCell.NeighborsPointIndices.Map<MapCellId?>((Func<int, MapCellId?>) (x =>
        {
          int? nullable = ptIndexToCellIndex[x];
          return !nullable.HasValue ? new MapCellId?() : new MapCellId?(new MapCellId(nullable.Value));
        })), vornoiCell.IsOnBoundary);
        immutableArrayBuilder[index] = mapCell;
      }
      perimeterPoints = vornoiPoints.ToImmutableArray<Tile2i>((Func<Vector2i, Tile2i>) (x => new Tile2i(x)));
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public static void DEBUG_DrawVornoiVisualization(
      ImmutableArray<MapCell> resultCells,
      Lyst<Vector2i> centerPoints,
      ImmutableArray<Tile2i> perimeterPoints,
      IEnumerable<DelaunayTriangle2i> triangulation = null)
    {
      Tile2i from = new Tile2i(centerPoints.Aggregate<Vector2i>((Func<Vector2i, Vector2i, Vector2i>) ((x, acc) => x.Min(acc)))).AddXy(-10);
      Tile2i tile2i1 = new Tile2i(centerPoints.Aggregate<Vector2i>((Func<Vector2i, Vector2i, Vector2i>) ((x, acc) => x.Max(acc)))).AddXy(10);
      DebugGameMapDrawing debugGameMapDrawing = DebugGameRenderer.StartMapDrawing(from, tile2i1 - from, 1, true);
      if (triangulation != null)
      {
        foreach (DelaunayTriangle2i delaunayTriangle2i in triangulation)
        {
          debugGameMapDrawing.DrawLine(new Tile2i(delaunayTriangle2i.P0), new Tile2i(delaunayTriangle2i.P1), ColorRgba.DarkGray, true);
          debugGameMapDrawing.DrawLine(new Tile2i(delaunayTriangle2i.P1), new Tile2i(delaunayTriangle2i.P2), ColorRgba.DarkGray, true);
          debugGameMapDrawing.DrawLine(new Tile2i(delaunayTriangle2i.P2), new Tile2i(delaunayTriangle2i.P0), ColorRgba.DarkGray, true);
        }
      }
      foreach (Vector2i centerPoint in centerPoints)
        debugGameMapDrawing.DrawCross(new Tile2i(centerPoint).CornerTile2f, ColorRgba.LightGray);
      foreach (MapCell resultCell in resultCells)
      {
        Tile2i tile2i2 = new Tile2i(centerPoints[resultCell.CenterPointIndex]);
        debugGameMapDrawing.DrawString(tile2i2.AddY(-5).CornerTile2f, resultCell.CenterPointIndex.ToString(), ColorRgba.White, centered: true);
        int index1 = resultCell.PerimeterIndices.Length - 1;
        for (int index2 = 0; index2 < resultCell.PerimeterIndices.Length; ++index2)
        {
          debugGameMapDrawing.DrawLine(perimeterPoints[resultCell.PerimeterIndices[index1]], perimeterPoints[resultCell.PerimeterIndices[index2]], ColorRgba.Gray, true);
          index1 = index2;
        }
      }
      debugGameMapDrawing.SaveMapAsTga("IslandMap");
    }
  }
}
