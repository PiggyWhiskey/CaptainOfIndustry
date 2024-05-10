// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.Previews.HeightmapFeaturePreviewChunkData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using System;

#nullable disable
namespace Mafi.Base.Terrain.Previews
{
  public class HeightmapFeaturePreviewChunkData : ITerrainFeaturePreview
  {
    public const int SIZE = 65;
    public HeightTilesF[] Heights;

    public Chunk2i Chunk { get; private set; }

    public RectangleTerrainArea2i Area { get; private set; }

    public bool Dirty { get; private set; }

    public HeightmapFeaturePreviewChunkData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = new RectangleTerrainArea2i(Tile2i.Zero, new RelTile2i(65, 65));
      this.Heights = new HeightTilesF[this.Area.AreaTiles];
    }

    public void Initialize(Chunk2i chunk)
    {
      this.Chunk = chunk;
      this.Area = this.Area.WithNewOrigin(chunk.Tile2i);
    }

    public HeightTilesF GetHeightAtClamped(int x, int y)
    {
      x = x.Clamp(0, 64);
      y = y.Clamp(0, 64);
      return this.Heights[y * 65 + x];
    }

    public void SetDirty(bool dirty) => this.Dirty = dirty;

    public void Load(TerrainManager terrainManager)
    {
      Tile2i tile2i = this.Chunk.Tile2i;
      if (!terrainManager.IsValidCoord(tile2i))
      {
        for (int index = 0; index < 4225; ++index)
          this.Heights[index] = HeightTilesF.MinValue;
      }
      else
      {
        HeightTilesF[] sourceArray = terrainManager.GetMutableTerrainDataRef().HeightSnapshot.ValueOrNull;
        if (sourceArray == null)
        {
          Log.Warning("No height snapshot found.");
          sourceArray = terrainManager.GetMutableTerrainDataRef().Heights;
        }
        else if (sourceArray.Length != terrainManager.TerrainArea.AreaTiles)
        {
          Log.Warning("Height snapshot size mismatch.");
          sourceArray = terrainManager.GetMutableTerrainDataRef().Heights;
        }
        bool flag1 = tile2i.X + 65 >= terrainManager.TerrainSize.X;
        bool flag2 = tile2i.Y + 65 >= terrainManager.TerrainSize.Y;
        int length = 65 - (flag1 ? 1 : 0);
        int num = 65 - (flag2 ? 1 : 0);
        for (int index = 0; index < num; ++index)
          Array.Copy((Array) sourceArray, (tile2i.Y + index) * terrainManager.TerrainWidth + tile2i.X, (Array) this.Heights, index * 65, length);
        if (flag1)
        {
          for (int index = 0; index < num; ++index)
            this.Heights[(index + 1) * 65 - 1] = this.Heights[(index + 1) * 65 - 2];
        }
        if (!flag2)
          return;
        Array.Copy((Array) sourceArray, (tile2i.Y + 64 - 1) * terrainManager.TerrainWidth + tile2i.X, (Array) this.Heights, 4160, length);
        if (!flag1)
          return;
        this.Heights[4224] = this.Heights[4223];
      }
    }
  }
}
