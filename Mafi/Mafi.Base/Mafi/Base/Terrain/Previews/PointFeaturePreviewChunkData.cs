// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.Previews.PointFeaturePreviewChunkData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain.Generation;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.Previews
{
  public class PointFeaturePreviewChunkData : ITerrainFeaturePreview
  {
    private readonly Lyst<Tile2iSlim> m_points;

    public Chunk2i Chunk { get; private set; }

    public IReadOnlyCollection<Tile2iSlim> Points
    {
      get => (IReadOnlyCollection<Tile2iSlim>) this.m_points;
    }

    public PointFeaturePreviewChunkData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_points = new Lyst<Tile2iSlim>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void Initialize(Chunk2i chunk)
    {
      this.Chunk = chunk;
      this.m_points.Clear();
    }

    public void Clear() => this.m_points.Clear();

    public void AddPoint(Tile2iSlim point) => this.m_points.Add(point);

    public void SetDirty(bool dirty)
    {
    }
  }
}
