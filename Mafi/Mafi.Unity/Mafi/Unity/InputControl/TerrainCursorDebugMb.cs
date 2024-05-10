// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainCursorDebugMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  internal class TerrainCursorDebugMb : MonoBehaviour
  {
    public Vector3 TileF;
    public Vector3 TileI;
    public Vector2 Chunk;
    public Vector2 TileInChunk;
    [TextArea(5, 10)]
    public string Layers;
    [TextArea(5, 10)]
    public string Flags;
    private TerrainCursor m_cursor;

    public void Initialize(TerrainCursor cursor)
    {
      this.m_cursor = cursor;
      this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      if (this.m_cursor == null)
        return;
      this.m_cursor.Activate();
    }

    private void OnDisable() => this.m_cursor.Deactivate();

    private void Update()
    {
      Assert.That<TerrainCursor>(this.m_cursor).IsNotNull<TerrainCursor>();
      Tile3f tile3f = this.m_cursor.Tile3f;
      this.TileF = new Vector3(tile3f.X.ToFloat(), tile3f.Y.ToFloat(), tile3f.Z.ToFloat());
      Tile3i tile3i = this.m_cursor.Tile3f.Tile3i;
      this.TileI = new Vector3((float) tile3i.X, (float) tile3i.Y, (float) tile3i.Z);
      Chunk2i chunkCoord2i = this.m_cursor.Tile2f.Tile2i.ChunkCoord2i;
      this.Chunk = new Vector2((float) chunkCoord2i.X, (float) chunkCoord2i.Y);
      TileInChunk2i tileInChunkCoord = this.m_cursor.Tile2f.Tile2i.TileInChunkCoord;
      this.TileInChunk = new Vector2((float) tileInChunkCoord.X, (float) tileInChunkCoord.Y);
      TerrainTile tile = this.m_cursor.Tile;
      this.Flags = tile.TerrainManager.Debug_ExplainFlags(tile.DataIndex);
      this.Layers = string.Format("{0} layers:\n", (object) tile.LayersCount);
      int num = 0;
      foreach (TerrainMaterialThicknessSlim enumerateLayer in tile.EnumerateLayers())
        this.Layers += string.Format("#{0} {1} of {2}\n", (object) num++, (object) enumerateLayer.Thickness, (object) tile.TerrainManager.ResolveSlimMaterial(enumerateLayer.SlimId).Id.Value);
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(this.m_cursor.Tile3f.ToVector3(), 0.5f);
    }

    public TerrainCursorDebugMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
