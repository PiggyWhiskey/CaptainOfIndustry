// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.StaticEntityPfTargetTiles
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Terrain;
using Mafi.Numerics;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// TODO: This could be pre-computed in proto for layout entities.
  public class StaticEntityPfTargetTiles
  {
    public static readonly StaticEntityPfTargetTiles Empty;
    [ThreadStatic]
    private static Set<Tile2i> s_tilesSetTmp;
    private readonly ImmutableArray<StaticEntityPfTargetTiles.TileWithNbrMask> m_tilesWithNbrMask;
    /// <summary>
    /// Cache for perimeter tiles at distance d. This list is indexed by d - 1.
    /// </summary>
    private readonly Lyst<ImmutableArray<Tile2i>> m_cache;

    public int TilesCount => this.m_tilesWithNbrMask.Length;

    private StaticEntityPfTargetTiles()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_cache = new Lyst<ImmutableArray<Tile2i>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tilesWithNbrMask = ImmutableArray<StaticEntityPfTargetTiles.TileWithNbrMask>.Empty;
    }

    private StaticEntityPfTargetTiles(
      ImmutableArray<StaticEntityPfTargetTiles.TileWithNbrMask> tilesWithNbrMask)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_cache = new Lyst<ImmutableArray<Tile2i>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tilesWithNbrMask = tilesWithNbrMask;
    }

    public static StaticEntityPfTargetTiles FromGroundTiles(ImmutableArray<Tile2i> groundTiles)
    {
      Lyst<StaticEntityPfTargetTiles.TileWithNbrMask> lyst = new Lyst<StaticEntityPfTargetTiles.TileWithNbrMask>();
      if (StaticEntityPfTargetTiles.s_tilesSetTmp == null)
        StaticEntityPfTargetTiles.s_tilesSetTmp = new Set<Tile2i>(128);
      Set<Tile2i> tilesSetTmp = StaticEntityPfTargetTiles.s_tilesSetTmp;
      tilesSetTmp.Clear();
      tilesSetTmp.AddRange(groundTiles);
      foreach (Tile2i groundTile in groundTiles)
      {
        uint nbrMask = 0;
        int firstNbrIndex = -1;
        int index = 0;
        while (true)
        {
          int num = index;
          ReadOnlyArray<RelTile2i> all8Neighbors = RelTile2i.All8Neighbors;
          int length = all8Neighbors.Length;
          if (num < length)
          {
            Set<Tile2i> set = tilesSetTmp;
            Tile2i tile2i1 = groundTile;
            all8Neighbors = RelTile2i.All8Neighbors;
            RelTile2i relTile2i = all8Neighbors[index];
            Tile2i tile2i2 = tile2i1 + relTile2i;
            if (!set.Contains(tile2i2))
            {
              nbrMask |= (uint) (1 << index);
              if (firstNbrIndex < 0)
                firstNbrIndex = index;
            }
            ++index;
          }
          else
            break;
        }
        if (nbrMask != 0U)
          lyst.Add(new StaticEntityPfTargetTiles.TileWithNbrMask(groundTile, nbrMask, firstNbrIndex));
      }
      return new StaticEntityPfTargetTiles(lyst.ToImmutableArray());
    }

    public ImmutableArray<Tile2i> GetPfTargetsAtDistance(
      RelTile1i distance,
      TerrainManager terrainManager)
    {
      Assert.That<RelTile1i>(distance).IsPositive();
      int index = distance.Value - 1;
      ImmutableArray<Tile2i> immutableArray;
      if (index < this.m_cache.Count)
      {
        immutableArray = this.m_cache[index];
        if (immutableArray.IsValid)
          return this.m_cache[index];
      }
      else
        this.m_cache.Count = index + 1;
      if (StaticEntityPfTargetTiles.s_tilesSetTmp == null)
        StaticEntityPfTargetTiles.s_tilesSetTmp = new Set<Tile2i>(128);
      Set<Tile2i> tilesSetTmp = StaticEntityPfTargetTiles.s_tilesSetTmp;
      tilesSetTmp.Clear();
      foreach (StaticEntityPfTargetTiles.TileWithNbrMask tileWithNbrMask in this.m_tilesWithNbrMask)
      {
        for (int firstNbrIndex = tileWithNbrMask.FirstNbrIndex; firstNbrIndex < RelTile2i.All8Neighbors.Length; ++firstNbrIndex)
        {
          if (tileWithNbrMask.ShouldExpandNeighborAt(firstNbrIndex))
          {
            Tile2i tileAtDistance1 = tileWithNbrMask.GetTileAtDistance(firstNbrIndex, distance);
            if (!terrainManager.IsOffLimitsOrInvalid(tileAtDistance1))
            {
              tilesSetTmp.Add(tileAtDistance1);
              int nbrIndex = firstNbrIndex + 1 & 7;
              if (tileWithNbrMask.ShouldExpandNeighborAt(nbrIndex))
              {
                Tile2i tileAtDistance2 = tileWithNbrMask.GetTileAtDistance(nbrIndex, distance);
                foreach (Vector2i iterateRasterizedPoint in new Line2i(tileAtDistance1.Vector2i, tileAtDistance2.Vector2i).IterateRasterizedPoints(true))
                  tilesSetTmp.Add(new Tile2i(iterateRasterizedPoint));
              }
              else
                ++firstNbrIndex;
            }
          }
        }
      }
      this.m_cache[index] = immutableArray = tilesSetTmp.ToImmutableArray<Tile2i>();
      return immutableArray;
    }

    static StaticEntityPfTargetTiles()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityPfTargetTiles.Empty = new StaticEntityPfTargetTiles();
    }

    private readonly struct TileWithNbrMask
    {
      public readonly Tile2i Position;
      /// <summary>
      /// Bit at index i is set if the original tile has no neighbor at that i-th direction.
      /// </summary>
      public readonly uint NbrMask;
      public readonly int FirstNbrIndex;

      public TileWithNbrMask(Tile2i position, uint nbrMask, int firstNbrIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Position = position;
        this.NbrMask = nbrMask;
        this.FirstNbrIndex = firstNbrIndex;
      }

      public bool ShouldExpandNeighborAt(int nbrIndex)
      {
        return (this.NbrMask & (uint) (1 << nbrIndex)) > 0U;
      }

      public Tile2i GetTileAtDistance(int nbrIndex, RelTile1i distance)
      {
        return this.Position + distance.Value * RelTile2i.All8Neighbors[nbrIndex];
      }
    }
  }
}
