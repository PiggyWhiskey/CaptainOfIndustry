// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGeneratorChunkData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public sealed class TerrainGeneratorChunkData
  {
    public const int SIZE = 64;
    public readonly HeightTilesF[] Heights;
    /// <summary>
    /// Material layers ordered from bottom to top, so that adding and removing from top is more efficient as it alters
    /// elements at the end of the list.
    /// </summary>
    public readonly LystStruct<TerrainMaterialThicknessSlim>[] MaterialLayers;
    public readonly TileSurfaceData[] Surfaces;

    public Chunk2i Chunk { get; private set; }

    public RectangleTerrainArea2i Area { get; private set; }

    public TerrainGeneratorChunkData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = new RectangleTerrainArea2i(Tile2i.Zero, new RelTile2i(64, 64));
      this.Heights = new HeightTilesF[this.Area.AreaTiles];
      this.MaterialLayers = new LystStruct<TerrainMaterialThicknessSlim>[this.Area.AreaTiles];
      this.Surfaces = new TileSurfaceData[this.Area.AreaTiles];
    }

    public void Initialize(Chunk2i chunk)
    {
      this.Chunk = chunk;
      this.Area = this.Area.WithNewOrigin(chunk.Tile2i);
      Array.Clear((Array) this.Heights, 0, this.Heights.Length);
      Array.Clear((Array) this.MaterialLayers, 0, this.MaterialLayers.Length);
      Array.Clear((Array) this.Surfaces, 0, this.Surfaces.Length);
    }

    /// <summary>
    /// Pushes new layer (or merges with existing if it has the same ID) on top of the given tile.
    /// This does not change the tile height.
    /// </summary>
    public void PushLayerOnTop(
      int tileIndex,
      TerrainMaterialSlimId slimId,
      ThicknessTilesF thickness)
    {
      if (thickness.IsNotPositive)
        return;
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      if (local.IsNotEmpty && local.Last.SlimId == slimId)
        local.Last += thickness;
      else
        local.Add(new TerrainMaterialThicknessSlim(slimId, thickness));
    }

    /// <summary>
    /// Pushes new layer (or merges with existing if it has the same ID) on top of the given tile.
    /// This increases the tile height by the given thickness.
    /// </summary>
    public void AddLayerOnTop(
      int tileIndex,
      TerrainMaterialSlimId slimId,
      ThicknessTilesF thickness)
    {
      if (thickness.IsNotPositive)
        return;
      this.Heights[tileIndex] += thickness;
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      if (local.IsNotEmpty && local.Last.SlimId == slimId)
        local.Last += thickness;
      else
        local.Add(new TerrainMaterialThicknessSlim(slimId, thickness));
    }

    public void RemoveAllLayersBelow(int tileIndex, HeightTilesF height)
    {
      ThicknessTilesF thickness = this.Heights[tileIndex] - height;
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      if (thickness.IsNotPositive)
      {
        local.Clear();
      }
      else
      {
        int num;
        for (num = local.Count - 1; num >= 0; --num)
        {
          TerrainMaterialThicknessSlim materialThicknessSlim = local[num];
          if (materialThicknessSlim.Thickness <= thickness)
          {
            thickness -= materialThicknessSlim.Thickness;
            if (thickness.IsNotPositive)
              break;
          }
          else
          {
            local[num] = local[num].WithThickness(thickness);
            break;
          }
        }
        if (num <= 0)
          return;
        local.RemoveFirstN(num);
      }
    }

    public void PushLayerOnBottom(
      int tileIndex,
      TerrainMaterialSlimId slimId,
      ThicknessTilesF thickness)
    {
      if (thickness.IsNotPositive)
        return;
      this.MaterialLayers[tileIndex].Insert(0, thickness.Of(slimId));
    }

    /// <summary>
    /// Sets top material to be the given ID within the given thickness from the top.
    /// This does not change the tile height.
    /// </summary>
    public void SetTopMaterial(
      int tileIndex,
      TerrainMaterialSlimId slimId,
      ThicknessTilesF thickness)
    {
      if (thickness.IsNotPositive)
        return;
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      if (local.IsEmpty)
      {
        local.Add(new TerrainMaterialThicknessSlim(slimId, thickness));
      }
      else
      {
        TerrainMaterialThicknessSlim last = local.Last;
        ThicknessTilesF toRemove;
        if (last.SlimId == slimId)
        {
          last = local.Last;
          if (last.Thickness >= thickness)
            return;
          ThicknessTilesF thicknessTilesF = thickness;
          last = local.Last;
          ThicknessTilesF thickness1 = last.Thickness;
          toRemove = thicknessTilesF - thickness1;
          local.PopLast();
        }
        else
          toRemove = thickness;
        this.PopThicknessFromTop(tileIndex, toRemove);
        this.PushLayerOnTop(tileIndex, slimId, thickness);
      }
    }

    /// <summary>
    /// Overrides material in the given range. This may change the tile height.
    /// </summary>
    /// 
    ///             TODO: This could use some tests.
    public void SetMaterialInRange(
      int tileIndex,
      TerrainMaterialSlimId slimId,
      HeightTilesF bottomHeight,
      ThicknessTilesF thickness)
    {
      if (thickness.IsNotPositive)
        return;
      HeightTilesF height = this.Heights[tileIndex];
      HeightTilesF rhs = bottomHeight + thickness;
      this.Heights[tileIndex] = height.Max(rhs);
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      if (local.IsEmpty)
        local.Add(thickness.Of(slimId));
      else if (bottomHeight >= height)
      {
        this.PushLayerOnTop(tileIndex, slimId, thickness);
      }
      else
      {
        int index = local.Count - 1;
        HeightTilesF heightTilesF1 = this.Heights[tileIndex];
        if (rhs > height)
        {
          this.PushLayerOnTop(tileIndex, slimId, rhs - height);
          thickness -= rhs - height;
          rhs = height;
          heightTilesF1 = height;
        }
        for (; index >= 0; --index)
        {
          TerrainMaterialThicknessSlim materialThicknessSlim = local[index];
          HeightTilesF heightTilesF2 = heightTilesF1;
          heightTilesF1 -= materialThicknessSlim.Thickness;
          if (!(rhs <= heightTilesF1))
          {
            if (!(heightTilesF2 <= bottomHeight))
            {
              if (heightTilesF1 > bottomHeight && heightTilesF2 < rhs)
                local.RemoveAt(index);
              else if (heightTilesF2 >= rhs)
              {
                ThicknessTilesF thickness1 = heightTilesF2 - rhs;
                if (thickness1.IsPositive)
                  local[index] = thickness1.Of(materialThicknessSlim.SlimId);
                else
                  local.RemoveAt(index);
                if (index < local.Count && local[index].SlimId == slimId)
                  local[index] = (local[index].Thickness + thickness).Of(slimId);
                else
                  local.Insert(index, thickness.Of(slimId));
                if (heightTilesF1 <= bottomHeight)
                {
                  thickness1 = bottomHeight - heightTilesF1;
                  if (thickness1.IsPositive)
                  {
                    local.Insert(index, thickness1.Of(materialThicknessSlim.SlimId));
                    break;
                  }
                  break;
                }
              }
              else if (heightTilesF1 <= bottomHeight)
              {
                ThicknessTilesF thickness2 = bottomHeight - heightTilesF1;
                if (thickness2.IsPositive)
                {
                  local[index] = thickness2.Of(materialThicknessSlim.SlimId);
                  break;
                }
                local.RemoveAt(index);
                break;
              }
            }
            else
              break;
          }
        }
        if (index != -1 || !(rhs <= heightTilesF1))
          return;
        local.Insert(0, thickness.Of(slimId));
      }
    }

    /// <summary>
    /// Removes certain thickness from the top but does not change the height.
    /// </summary>
    public void PopThicknessFromTop(int tileIndex, ThicknessTilesF toRemove)
    {
      ref LystStruct<TerrainMaterialThicknessSlim> local = ref this.MaterialLayers[tileIndex];
      while (local.IsNotEmpty && toRemove.IsPositive)
      {
        ThicknessTilesF thickness = local.Last.Thickness;
        if (thickness <= toRemove)
        {
          local.PopLast();
          toRemove -= thickness;
        }
        else
        {
          local.Last = local.Last.WithThickness(thickness - toRemove);
          break;
        }
      }
    }
  }
}
