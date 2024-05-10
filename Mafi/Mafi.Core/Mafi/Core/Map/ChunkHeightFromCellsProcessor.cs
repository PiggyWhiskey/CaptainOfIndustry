// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.ChunkHeightFromCellsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Handles computation of smooth base tile height base on cell heights.
  /// </summary>
  public class ChunkHeightFromCellsProcessor
  {
    private static readonly Percent MIN_WEIGHT;
    private readonly RelTile1f m_smoothingRadiusPerHeightDiff;
    private readonly RelTile1f m_minSmoothingRadius;
    private readonly RelTile1f m_maxSmoothingRadius;
    private readonly Lyst<Fix32> m_smoothingRadii;
    private readonly Lyst<KeyValuePair<HeightTilesF, Percent>> m_heightsWeightsTmp;
    private readonly HeightTilesF[] m_heightSamples;
    private Chunk2i m_chunkCoord;
    private ImmutableArray<MapCell> m_cells;

    public ChunkHeightFromCellsProcessor(
      RelTile1f smoothingRadiusPerHeightDiff,
      RelTile1f minSmoothingRadius,
      RelTile1f maxSmoothingRadius)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_smoothingRadii = new Lyst<Fix32>(true);
      this.m_heightsWeightsTmp = new Lyst<KeyValuePair<HeightTilesF, Percent>>(true);
      this.m_heightSamples = new HeightTilesF[8.Squared()];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<RelTile1f>(minSmoothingRadius).IsLess(maxSmoothingRadius);
      this.m_smoothingRadiusPerHeightDiff = smoothingRadiusPerHeightDiff.CheckPositive();
      this.m_minSmoothingRadius = minSmoothingRadius.CheckPositive();
      this.m_maxSmoothingRadius = maxSmoothingRadius.CheckPositive();
    }

    /// <summary>Initializes processing of given chunk.</summary>
    /// <param name="nearbyCells">All cells that may affect the chunk.</param>
    /// <param name="chunkCoord">Chunk coordinate to initialize.</param>
    public void InitializeForChunk(ImmutableArray<MapCell> nearbyCells, Chunk2i chunkCoord)
    {
      this.m_chunkCoord = chunkCoord;
      this.m_cells = nearbyCells.CheckNotEmpty<MapCell>();
      this.m_smoothingRadii.Clear();
      foreach (MapCell cell in this.m_cells)
      {
        Fix32 fix32 = this.m_minSmoothingRadius.Value;
        foreach (Option<MapCell> neighbor in cell.Neighbors)
        {
          if (!neighbor.IsNone)
          {
            Fix32 other = this.m_smoothingRadiusPerHeightDiff.Value * (cell.GroundHeight - neighbor.Value.GroundHeight).Abs.Value;
            fix32 = fix32.Max(other);
          }
        }
        this.m_smoothingRadii.Add(fix32.Min(this.m_maxSmoothingRadius.Value));
      }
      for (int y = 0; y < 8; ++y)
      {
        int num = y * 8;
        for (int x = 0; x < 8; ++x)
        {
          HeightTilesF heightTilesF = this.computeHeightAt(this.m_chunkCoord.Tile2i + (new RelTile2i(x, y) - 1) * 16);
          heightTilesF = new HeightTilesF((heightTilesF.Value * 8).ToIntRounded().ToFix32() / 8);
          this.m_heightSamples[num + x] = heightTilesF;
        }
      }
    }

    /// <summary>
    /// Returns height of a tile relative to chunk that is currently initialized. Thread safe for currently
    /// initialized chunk.
    /// </summary>
    public HeightTilesF GetHeight(TileInChunk2i coord)
    {
      Assert.That<bool>(coord.X >= 0 && coord.Y >= 0).IsTrue();
      Vector2i vector2i = coord.Vector2i / 16;
      Percent tx = Percent.FromRatio(coord.Vector2i.X - vector2i.X * 16, 16);
      Percent ty = Percent.FromRatio(coord.Vector2i.Y - vector2i.Y * 16, 16);
      Assert.That<Percent>(tx).IsWithin0To100PercIncl();
      Assert.That<Percent>(ty).IsWithin0To100PercIncl();
      return this.m_heightSamples.BicubicInterpolate(8, (vector2i.Y + 1) * 8 + vector2i.X + 1, tx, ty);
    }

    /// <summary>
    /// Computes exact height based on distance from nearby cells. This is computationally intensive.
    /// </summary>
    private HeightTilesF computeHeightAt(Tile2i tileCoord)
    {
      Assert.That<ImmutableArray<MapCell>>(this.m_cells).IsNotEmpty<MapCell>();
      this.m_heightsWeightsTmp.Clear();
      for (int index = 0; index < this.m_cells.Length; ++index)
      {
        MapCell cell = this.m_cells[index];
        Percent cellContribution = this.getCellContribution(index, tileCoord);
        HeightTilesF key = cell.GroundHeight.HeightTilesF;
        if (cell.IsOcean)
        {
          if (cell.Contains(tileCoord))
          {
            long self = long.MaxValue;
            foreach (Option<MapCell> neighbor in cell.Neighbors)
            {
              if (neighbor.HasValue && neighbor.Value.IsNotOcean)
              {
                long num = neighbor.Value.BoundaryDistanceSqrTo(tileCoord);
                self = self.Min(num);
              }
            }
            if (self != long.MaxValue)
            {
              Fix64 fix64_1 = self.ToFix64();
              fix64_1 = fix64_1.Sqrt() / cell.OuterRadius.Value;
              Fix64 fix64_2 = fix64_1.Clamp01();
              key = new HeightTilesF(key.Value * fix64_2.SqrtToFix32());
            }
          }
          else
            continue;
        }
        this.m_heightsWeightsTmp.Add(Make.Kvp<HeightTilesF, Percent>(key, cellContribution));
      }
      Percent zero = Percent.Zero;
      foreach (KeyValuePair<HeightTilesF, Percent> keyValuePair in this.m_heightsWeightsTmp)
        zero += keyValuePair.Value;
      if (zero < ChunkHeightFromCellsProcessor.MIN_WEIGHT)
        return this.computeHeightOutsideOfMap(tileCoord);
      Fix32 fix32 = (Fix32) 0;
      foreach (KeyValuePair<HeightTilesF, Percent> keyValuePair in this.m_heightsWeightsTmp)
        fix32 += keyValuePair.Key.Value.ScaledBy(keyValuePair.Value / zero);
      return new HeightTilesF(fix32);
    }

    private HeightTilesF computeHeightOutsideOfMap(Tile2i tileCoord)
    {
      MapCell mapCell = this.m_cells.MinElement<long>((Func<MapCell, long>) (x => x.BoundaryDistanceSqrTo(tileCoord)));
      return !mapCell.IsOcean ? mapCell.GroundHeight.HeightTilesF : MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT.HeightTilesF;
    }

    private Percent getCellContribution(int cellIndex, Tile2i tileCoord)
    {
      return (Percent.Hundred - (Percent.FromRatio(this.m_cells[cellIndex].SignedBoundaryDistanceTo(tileCoord), this.m_smoothingRadii[cellIndex]).Clamp(-100.Percent(), 100.Percent()) * Percent.Tau / 4).Sin()) / 2;
    }

    /// <summary>
    /// Clears the initialization. Should be called when done with chunk to ensure <see cref="M:Mafi.Core.Map.ChunkHeightFromCellsProcessor.GetHeight(Mafi.TileInChunk2i)" /> is not
    /// called without proper initialization.
    /// </summary>
    public void Clear()
    {
      this.m_chunkCoord = Chunk2i.Zero;
      this.m_cells = ImmutableArray<MapCell>.Empty;
      this.m_heightsWeightsTmp.Clear();
    }

    static ChunkHeightFromCellsProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ChunkHeightFromCellsProcessor.MIN_WEIGHT = Percent.One;
    }
  }
}
