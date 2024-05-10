// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.GeneratedTreesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Trees;
using Mafi.Numerics;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  public class GeneratedTreesData : ITerrainGenerationExtraData
  {
    private Chunk64Area m_area;
    private GeneratedTreesData.Chunk[] m_chunks;
    private int m_stride;
    private readonly TreesManager m_treesManager;

    public GeneratedTreesData(TreesManager treesManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treesManager = treesManager;
    }

    private int getChunkIndex(Chunk2i chunk)
    {
      Vector2i vector2i = chunk - this.m_area.Origin;
      return vector2i.X + vector2i.Y * this.m_stride;
    }

    public GeneratedTreesData.Chunk GetOrCreateChunk(Chunk2i chunk)
    {
      int chunkIndex = this.getChunkIndex(chunk);
      GeneratedTreesData.Chunk chunk1 = this.m_chunks[chunkIndex];
      if (chunk1 == null)
      {
        chunk1 = new GeneratedTreesData.Chunk();
        this.m_chunks[chunkIndex] = chunk1;
      }
      return chunk1;
    }

    void ITerrainGenerationExtraData.Initialize(Chunk64Area area)
    {
      this.m_area = area;
      this.m_stride = area.Size.X;
      this.m_chunks = new GeneratedTreesData.Chunk[area.TotalChunksCount];
    }

    void ITerrainGenerationExtraData.ApplyInArea(Chunk64Area area, bool isInMapEditor)
    {
      if (area.Intersect(this.m_area) != area)
      {
        Log.Warning("Applying data in larger area than initialized area.");
        area = area.Intersect(this.m_area);
      }
      foreach (Chunk2i enumerateChunk in area.EnumerateChunks())
      {
        GeneratedTreesData.Chunk chunk = this.m_chunks[this.getChunkIndex(enumerateChunk)];
        this.m_treesManager.ClearTreesOnChunk(enumerateChunk);
        if (chunk != null)
        {
          foreach (TreeDataBase treeData in chunk.Trees.Values)
          {
            Tile2i tile2i = treeData.Position.Tile2i;
            if (tile2i.ChunkCoord2i != enumerateChunk)
              Log.Warning(string.Format("Generated tree at {0} is in chunk {1} but ", (object) treeData.Position, (object) tile2i.ChunkCoord2i) + string.Format("it was generated to data of chunk {0}, skipping.", (object) enumerateChunk));
            else
              this.m_treesManager.TryAddGeneratedTree(treeData);
          }
        }
      }
    }

    public class Chunk
    {
      public readonly Dict<TreeId, TreeDataBase> Trees;
      public readonly Lyst<Polygon2fFast> ClearedPolygons;

      public override string ToString()
      {
        return string.Format("{0} trees, {1} clear polygons", (object) this.Trees.Count, (object) this.ClearedPolygons.Count);
      }

      public Chunk()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Trees = new Dict<TreeId, TreeDataBase>();
        this.ClearedPolygons = new Lyst<Polygon2fFast>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
