// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ConstructionCubesHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  internal static class ConstructionCubesHelper
  {
    public static ImmutableArray<KeyValuePair<Vector2i, OccupiedColumn>> ComputeOptimizedConstructionCubeColumns(
      ImmutableArray<OccupiedTileRelative> occupiedTiles,
      int optimizeThreshold = 100)
    {
      if (occupiedTiles.IsEmpty)
        return ImmutableArray<KeyValuePair<Vector2i, OccupiedColumn>>.Empty;
      int num = 0;
      ThicknessTilesI thicknessTilesI = ThicknessTilesI.MaxValue;
      Dict<Vector2i, OccupiedColumn> dict1 = new Dict<Vector2i, OccupiedColumn>(occupiedTiles.Length);
      Lyst<Vector2i> coords = new Lyst<Vector2i>(occupiedTiles.Length / 3);
      for (int index = 0; index < occupiedTiles.Length; ++index)
      {
        OccupiedTileRelative occupiedTile = occupiedTiles[index];
        num += occupiedTile.VerticalSize.Value;
        dict1.AddAndAssertNew(new Vector2i((int) occupiedTile.RelativeX, (int) occupiedTile.RelativeY), new OccupiedColumn((int) occupiedTile.RelativeFrom, (int) occupiedTile.RelativeFrom + (int) occupiedTile.VerticalSizeRaw, 1, index), "Repeated column coords are not supported.");
        if (occupiedTile.FromHeightRel < thicknessTilesI)
          thicknessTilesI = occupiedTile.FromHeightRel;
        if ((((int) occupiedTile.RelativeX | (int) occupiedTile.RelativeY) & 1) == 0)
          coords.Add(new Vector2i((int) occupiedTile.RelativeX, (int) occupiedTile.RelativeY));
      }
      Lyst<KeyValuePair<Vector2i, OccupiedColumn>> finalColumns = new Lyst<KeyValuePair<Vector2i, OccupiedColumn>>((occupiedTiles.Length / 16).Max(8));
      if (num > optimizeThreshold)
      {
        Dict<Vector2i, OccupiedColumn> dict2 = new Dict<Vector2i, OccupiedColumn>();
        Lyst<Vector2i> lyst = new Lyst<Vector2i>();
        optimizeSquareColumns(coords, 1, dict1, dict2, 3, lyst);
        finalColumns.AddRange((IEnumerable<KeyValuePair<Vector2i, OccupiedColumn>>) dict1);
        dict1.Clear();
        if (dict2.Count > optimizeThreshold)
        {
          Dict<Vector2i, OccupiedColumn> dict3 = dict1;
          optimizeSquareColumns(lyst, 2, dict2, dict3, -1, (Lyst<Vector2i>) null);
          finalColumns.AddRange((IEnumerable<KeyValuePair<Vector2i, OccupiedColumn>>) dict3);
        }
        finalColumns.AddRange((IEnumerable<KeyValuePair<Vector2i, OccupiedColumn>>) dict2);
      }
      else
        finalColumns.AddRange((IEnumerable<KeyValuePair<Vector2i, OccupiedColumn>>) dict1);
      return finalColumns.ToImmutableArray();

      void optimizeSquareColumns(
        Lyst<Vector2i> coords,
        int scale,
        Dict<Vector2i, OccupiedColumn> srcColumns,
        Dict<Vector2i, OccupiedColumn> resultSquareColumns,
        int furtherProcessingCoordsMask,
        Lyst<Vector2i> coordsForFurtherProcessing)
      {
        foreach (Vector2i coord in coords)
        {
          OccupiedColumn c1;
          OccupiedColumn c2;
          OccupiedColumn c3;
          OccupiedColumn c4;
          if (srcColumns.TryGetValue(coord, out c1) && srcColumns.TryGetValue(coord.AddX(scale), out c2) && srcColumns.TryGetValue(coord.AddY(scale), out c3) && srcColumns.TryGetValue(coord.AddXy(scale), out c4))
          {
            int num1 = c1.From.Max(c2.From).Max(c3.From).Max(c4.From);
            int num2 = c1.ToExcl.Min(c2.ToExcl).Min(c3.ToExcl).Min(c4.ToExcl);
            if (num1 < num2)
            {
              cutColumnToHeightRange(coord, c1, num1, num2);
              cutColumnToHeightRange(coord.AddX(scale), c2, num1, num2);
              cutColumnToHeightRange(coord.AddY(scale), c3, num1, num2);
              cutColumnToHeightRange(coord.AddXy(scale), c4, num1, num2);
              resultSquareColumns.Add(coord, new OccupiedColumn(num1, num2, 2 * scale, c1.IdForXySorting.Min(c2.IdForXySorting).Min(c3.IdForXySorting).Min(c4.IdForXySorting)));
              if (((coord.X | coord.Y) & furtherProcessingCoordsMask) == 0 && coordsForFurtherProcessing != null)
                coordsForFurtherProcessing.Add(coord);
            }
          }
        }

        void cutColumnToHeightRange(Vector2i coord, OccupiedColumn c, int minH, int maxHexcl)
        {
          if (c.From < minH)
            finalColumns.Add<Vector2i, OccupiedColumn>(coord, new OccupiedColumn(c.From, minH, c.Scale, c.IdForXySorting));
          if (c.ToExcl > maxHexcl)
            finalColumns.Add<Vector2i, OccupiedColumn>(coord, new OccupiedColumn(maxHexcl, c.ToExcl, c.Scale, c.IdForXySorting));
          srcColumns.Remove(coord);
        }
      }
    }

    public static ImmutableArray<ConstrCubeSpec> ConvertColumnsToCubes(
      Tile3i centerTile,
      ImmutableArray<KeyValuePair<Vector2i, OccupiedColumn>> columns,
      bool generateGroundLevelSeparately,
      out int totalCubesVolume)
    {
      Lyst<KeyValuePair<int, ConstrCubeSpec>> list = new Lyst<KeyValuePair<int, ConstrCubeSpec>>(columns.Length);
      int num1 = int.MaxValue;
      totalCubesVolume = 0;
      foreach (KeyValuePair<Vector2i, OccupiedColumn> column in columns)
      {
        totalCubesVolume += (column.Value.ToExcl - column.Value.From) * column.Value.Scale * column.Value.Scale;
        if (column.Value.From < num1)
          num1 = column.Value.From;
      }
      foreach (KeyValuePair<Vector2i, OccupiedColumn> column in columns)
      {
        int from = column.Value.From;
        int toExcl = column.Value.ToExcl;
        if (generateGroundLevelSeparately && from < 1)
        {
          int num2 = toExcl.Min(1);
          for (int scale = column.Value.Scale; scale > 0; --scale)
          {
            for (; from + scale <= num2; from += scale)
            {
              byte transitionHeightTiles = (byte) (from - num1 + scale).Min((int) byte.MaxValue);
              list.Add<int, ConstrCubeSpec>(column.Value.IdForXySorting, new ConstrCubeSpec((centerTile.Xy + new RelTile2i(column.Key)).AsSlim, (centerTile.Height + new ThicknessTilesI(from)).AsSlim, (byte) column.Value.Scale, (byte) column.Value.Scale, (byte) scale, transitionHeightTiles));
            }
          }
          if (column.Value.ToExcl > 1)
            toExcl = column.Value.ToExcl;
          else
            continue;
        }
        for (int scale = column.Value.Scale; scale > 0; --scale)
        {
          for (; from + scale <= toExcl; from += scale)
          {
            byte transitionHeightTiles = (byte) (from - num1 + scale).Min((int) byte.MaxValue);
            list.Add<int, ConstrCubeSpec>(column.Value.IdForXySorting, new ConstrCubeSpec((centerTile.Xy + new RelTile2i(column.Key)).AsSlim, (centerTile.Height + new ThicknessTilesI(from)).AsSlim, (byte) column.Value.Scale, (byte) column.Value.Scale, (byte) scale, transitionHeightTiles));
          }
        }
      }
      list.Sort((Comparison<KeyValuePair<int, ConstrCubeSpec>>) ((a, b) =>
      {
        if (a.Value.Height < b.Value.Height)
          return -1;
        if (a.Value.Height > b.Value.Height)
          return 1;
        if (a.Key < b.Key)
          return -1;
        return a.Key > b.Key ? 1 : 0;
      }));
      return list.ToImmutableArray<ConstrCubeSpec>((Func<KeyValuePair<int, ConstrCubeSpec>, ConstrCubeSpec>) (x => x.Value));
    }
  }
}
