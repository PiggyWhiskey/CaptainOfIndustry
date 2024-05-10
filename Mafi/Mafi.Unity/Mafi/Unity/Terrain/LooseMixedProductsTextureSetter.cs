// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.LooseMixedProductsTextureSetter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Factory;
using Mafi.Core.Products;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public readonly struct LooseMixedProductsTextureSetter
  {
    public static int MAX_TEXTURE_COUNT;
    private readonly Renderer m_renderer;
    private readonly MaterialPropertyBlock m_propBlock;
    private readonly LoosePileTextureParams m_pileParams;

    public LooseMixedProductsTextureSetter(
      Renderer renderer,
      MaterialPropertyBlock propBlock,
      LoosePileTextureParams pileParams)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_renderer = renderer.CheckNotNull<Renderer>();
      this.m_propBlock = propBlock.CheckNotNull<MaterialPropertyBlock>();
      this.m_pileParams = pileParams;
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_0, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_1, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_2, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_3, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_4, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_5, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_6, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_7, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      renderer.SetPropertyBlock(propBlock);
    }

    public void SetUVs(float offset, float scale)
    {
      this.m_propBlock.SetVector(LooseProductMaterialManager.MODEL_UVS, (Vector4) new Vector2(offset, scale));
      this.m_renderer.SetPropertyBlock(this.m_propBlock);
    }

    public void SetRatios(float ratio0, float ratio1, float ratio2)
    {
      Assert.That<float>(ratio0 + ratio1 + ratio2).IsNear(1f, 1f / 1000f, string.Format("Ratios don't sum to 1: {0}, {1}, {2}", (object) ratio0, (object) ratio1, (object) ratio2));
      this.m_propBlock.SetVector(LooseProductMaterialManager.MATERIAL_RATIOS, (Vector4) new Vector3(ratio0, ratio1, ratio2));
      this.m_propBlock.SetVector(LooseProductMaterialManager.MATERIAL_RATIOS_2, Vector4.zero);
      this.m_renderer.SetPropertyBlock(this.m_propBlock);
    }

    public void SetRatios(
      float ratio0,
      float ratio1,
      float ratio2,
      float ratio3,
      float ratio4,
      float ratio5,
      float ratio6,
      float ratio7)
    {
      Assert.That<float>(ratio0 + ratio1 + ratio2 + ratio3 + ratio4 + ratio5 + ratio6 + ratio7).IsNear(1f, 1f / 1000f, string.Format("Ratios don't sum to 1: {0}, {1}, {2}, {3} {4}, {5}, {6}, {7}", (object) ratio0, (object) ratio1, (object) ratio2, (object) ratio3, (object) ratio4, (object) ratio5, (object) ratio6, (object) ratio7));
      float num = 1.001f;
      this.m_propBlock.SetVector(LooseProductMaterialManager.MATERIAL_RATIOS, new Vector4(ratio0, ratio1, ratio2, ratio3) * num);
      this.m_propBlock.SetVector(LooseProductMaterialManager.MATERIAL_RATIOS_2, new Vector4(ratio4, ratio5, ratio6, ratio7) * num);
      this.m_renderer.SetPropertyBlock(this.m_propBlock);
    }

    public void SetTexture(LooseProductSlimId slimId, int index)
    {
      if (index >= LooseMixedProductsTextureSetter.MAX_TEXTURE_COUNT)
        Log.Warning(string.Format("Index '{0}' is greater than max texture count ('{1}')", (object) index, (object) LooseMixedProductsTextureSetter.MAX_TEXTURE_COUNT));
      else if (slimId.IsPhantom)
        Log.Warning("Trying to set phantom to a texture.");
      else if (this.m_propBlock == null)
      {
        Log.Warning("Setting texture using non-initialized texture setter.");
      }
      else
      {
        switch (index)
        {
          case 0:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_0, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 1:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_1, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 2:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_2, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 3:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_3, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 4:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_4, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 5:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_5, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 6:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_6, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
          case 7:
            this.m_propBlock.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_7, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
            break;
        }
        this.m_renderer.SetPropertyBlock(this.m_propBlock);
      }
    }

    static LooseMixedProductsTextureSetter()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LooseMixedProductsTextureSetter.MAX_TEXTURE_COUNT = 8;
    }
  }
}
