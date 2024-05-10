// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.FarmableManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class FarmableManager
  {
    private readonly TerrainManager m_terrainManager;
    private ImmutableArray<bool> m_isFarmableLookup;

    public FarmableManager(TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
    }

    public ThicknessTilesF GetFarmableThickness(
      Tile2i tile,
      ThicknessTilesF maxValueAllowed,
      out bool surfaceInWay)
    {
      surfaceInWay = false;
      TileSurfaceData tileSurfaceData;
      if (this.m_terrainManager.TryGetTileSurface(this.m_terrainManager.GetTileIndex(tile), out tileSurfaceData))
      {
        ThicknessTilesF thicknessTilesF = this.m_terrainManager.GetHeight(tile) - tileSurfaceData.Height;
        if (thicknessTilesF <= ThicknessTilesF.Zero)
        {
          surfaceInWay = true;
          return ThicknessTilesF.Zero;
        }
        maxValueAllowed = thicknessTilesF.Min(maxValueAllowed);
      }
      if (this.m_isFarmableLookup == (ImmutableArray<bool>) (ImmutableArray.EmptyArray) null || this.m_isFarmableLookup.IsEmpty)
        this.m_isFarmableLookup = this.m_terrainManager.TerrainMaterials.Map<bool>((Func<TerrainMaterialProto, bool>) (x => x.IsFarmable));
      ThicknessTilesF zero1 = ThicknessTilesF.Zero;
      ThicknessTilesF zero2 = ThicknessTilesF.Zero;
      foreach (TerrainMaterialThicknessSlim enumerateLayer in this.m_terrainManager.EnumerateLayers(this.m_terrainManager.GetTileIndex(tile)))
      {
        if (this.m_isFarmableLookup[enumerateLayer.SlimIdRaw])
        {
          zero1 += enumerateLayer.Thickness;
          if (zero1 >= maxValueAllowed)
            return maxValueAllowed - zero2;
        }
        else
        {
          zero2 += enumerateLayer.Thickness;
          if (zero2 >= maxValueAllowed)
            return zero1;
        }
      }
      return !this.m_terrainManager.Bedrock.IsFarmable ? zero1 : maxValueAllowed - zero2;
    }
  }
}
