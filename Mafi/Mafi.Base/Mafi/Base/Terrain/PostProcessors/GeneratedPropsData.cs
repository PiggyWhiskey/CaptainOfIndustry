// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.GeneratedPropsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Numerics;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  public class GeneratedPropsData : ITerrainGenerationExtraData
  {
    private Chunk64Area m_area;
    private GeneratedPropsData.Chunk[] m_chunks;
    private int m_stride;
    private readonly TerrainPropsManager m_propsManager;

    public GeneratedPropsData(TerrainPropsManager propsManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_propsManager = propsManager;
    }

    private int getChunkIndex(Chunk2i chunk)
    {
      Vector2i vector2i = chunk - this.m_area.Origin;
      return vector2i.X + vector2i.Y * this.m_stride;
    }

    public GeneratedPropsData.Chunk GetOrCreateChunk(Chunk2i chunk)
    {
      int chunkIndex = this.getChunkIndex(chunk);
      GeneratedPropsData.Chunk chunk1 = this.m_chunks[chunkIndex];
      if (chunk1 == null)
      {
        chunk1 = new GeneratedPropsData.Chunk();
        this.m_chunks[chunkIndex] = chunk1;
      }
      return chunk1;
    }

    void ITerrainGenerationExtraData.Initialize(Chunk64Area area)
    {
      this.m_area = area;
      this.m_stride = area.Size.X;
      this.m_chunks = new GeneratedPropsData.Chunk[area.TotalChunksCount];
    }

    void ITerrainGenerationExtraData.ApplyInArea(Chunk64Area area, bool isInMapEditor)
    {
      if (area.Intersect(this.m_area) != area)
      {
        Log.Warning("Applying data in larger area than initialized area.");
        area = area.Intersect(this.m_area);
      }
      if (isInMapEditor)
        this.m_propsManager.ClearRemovedPropsForTerrainEditor();
      foreach (Chunk2i enumerateChunk in area.EnumerateChunks())
      {
        GeneratedPropsData.Chunk chunk = this.m_chunks[this.getChunkIndex(enumerateChunk)];
        this.m_propsManager.ClearPropsOnChunkForTerrainEditor(enumerateChunk);
        if (chunk != null)
        {
          foreach (KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>> andSurfaceMaterial in chunk.PropsAndSurfaceMaterials)
          {
            TerrainPropData first = andSurfaceMaterial.Value.First;
            Tile2i tile2i = first.Position.Tile2i;
            if (tile2i.ChunkCoord2i != enumerateChunk)
              Log.Warning(string.Format("Generated prop at {0} is in chunk {1} but ", (object) first.Position, (object) tile2i.ChunkCoord2i) + string.Format("it was generated to data of chunk {0}, skipping.", (object) enumerateChunk));
            else
              this.m_propsManager.TryAddProp(first);
          }
        }
      }
      if (!isInMapEditor)
        return;
      this.m_propsManager.ClearRemovedPropsForTerrainEditor();
    }

    public class Chunk
    {
      public readonly Lyst<KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>> PropsAndSurfaceMaterials;
      public readonly Lyst<Polygon2fFast> ClearedPolygons;

      public Chunk()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.PropsAndSurfaceMaterials = new Lyst<KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>>();
        this.ClearedPolygons = new Lyst<Polygon2fFast>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
