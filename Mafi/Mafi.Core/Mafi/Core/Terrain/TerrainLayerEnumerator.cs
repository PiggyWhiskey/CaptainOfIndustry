// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainLayerEnumerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>
  /// Allocation-free enumerator for terrain layers.
  /// This helps with efficient enumeration through all layers including overflow and bedrock.
  /// </summary>
  public struct TerrainLayerEnumerator
  {
    private readonly ReadOnlyArraySlice<TileMaterialLayerOverflow> m_overflowData;
    private readonly TileMaterialLayers m_layersData;
    private readonly TerrainMaterialSlimId m_bedrockSlimId;
    private int m_index;
    private int m_nextOverflowIndex;

    public TerrainLayerEnumerator(
      ReadOnlyArraySlice<TileMaterialLayerOverflow> overflowData,
      TileMaterialLayers layersData,
      TerrainMaterialSlimId bedrockSlimId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_nextOverflowIndex = 0;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCurrent\u003Ek__BackingField = new TerrainMaterialThicknessSlim();
      this.m_overflowData = overflowData;
      this.m_layersData = layersData;
      this.m_bedrockSlimId = bedrockSlimId;
      this.m_index = -1;
    }

    public TerrainLayerEnumerator(TerrainManager terrainManager, Tile2iIndex index)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new TerrainLayerEnumerator(terrainManager.MaterialLayersOverflowData, terrainManager.GetLayersRawData(index), terrainManager.Bedrock.SlimId);
    }

    public readonly TerrainLayerEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
      ++this.m_index;
      if (this.m_index >= this.m_layersData.Count)
      {
        this.Current = new TerrainMaterialThicknessSlim(this.m_bedrockSlimId, TerrainManager.BedrockLayerThicknessDefault);
        return this.m_index == this.m_layersData.Count;
      }
      if (this.m_index < 4)
      {
        switch (this.m_index)
        {
          case 0:
            this.Current = this.m_layersData.First;
            break;
          case 1:
            this.Current = this.m_layersData.Second;
            break;
          case 2:
            this.Current = this.m_layersData.Third;
            break;
          default:
            this.Current = this.m_layersData.Fourth;
            this.m_nextOverflowIndex = this.m_layersData.OverflowIndex;
            break;
        }
      }
      else
      {
        TileMaterialLayerOverflow materialLayerOverflow = this.m_overflowData[this.m_nextOverflowIndex];
        this.Current = materialLayerOverflow.Material;
        this.m_nextOverflowIndex = materialLayerOverflow.OverflowIndex;
      }
      return true;
    }

    public TerrainMaterialThicknessSlim Current { get; private set; }
  }
}
