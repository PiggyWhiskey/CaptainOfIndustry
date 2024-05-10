// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.Previews.HeightmapTopBottomPreviewChunkData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;

#nullable disable
namespace Mafi.Base.Terrain.Previews
{
  public class HeightmapTopBottomPreviewChunkData : ITerrainFeaturePreview
  {
    public const int SIZE = 65;
    public readonly Pair<HeightTilesF, HeightTilesF>[] Heights;

    public Chunk2i Chunk { get; private set; }

    public RectangleTerrainArea2i Area { get; private set; }

    public bool Dirty { get; private set; }

    public HeightmapTopBottomPreviewChunkData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = new RectangleTerrainArea2i(Tile2i.Zero, new RelTile2i(65, 65));
      this.Heights = new Pair<HeightTilesF, HeightTilesF>[this.Area.AreaTiles];
    }

    public void Initialize(Chunk2i chunk)
    {
      this.Chunk = chunk;
      this.Area = this.Area.WithNewOrigin(chunk.Tile2i);
    }

    public Pair<HeightTilesF, HeightTilesF> GetHeightAtClamped(int x, int y)
    {
      x = x.Clamp(0, 64);
      y = y.Clamp(0, 64);
      return this.Heights[y * 65 + x];
    }

    public void SetDirty(bool dirty) => this.Dirty = dirty;
  }
}
