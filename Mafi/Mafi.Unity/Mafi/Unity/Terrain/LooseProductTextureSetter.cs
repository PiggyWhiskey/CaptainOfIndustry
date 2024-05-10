// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.LooseProductTextureSetter
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
  public readonly struct LooseProductTextureSetter
  {
    public static readonly int TEX_DATA_SHADER_ID;
    private readonly Renderer m_renderer;
    private readonly MaterialPropertyBlock m_propBlock;
    private readonly LoosePileTextureParams m_pileParams;

    public LooseProductTextureSetter(
      Renderer renderer,
      MaterialPropertyBlock propBlock,
      LoosePileTextureParams pileParams)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_renderer = renderer.CheckNotNull<Renderer>();
      this.m_propBlock = propBlock.CheckNotNull<MaterialPropertyBlock>();
      this.m_pileParams = pileParams;
      propBlock.SetVector(LooseProductTextureSetter.TEX_DATA_SHADER_ID, new Vector4(0.0f, pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
      renderer.SetPropertyBlock(propBlock);
    }

    public void SetTexture(LooseProductSlimId slimId)
    {
      if (slimId.IsPhantom)
        Log.Warning("Trying to set phantom to a texture.");
      else if (this.m_propBlock == null)
      {
        Log.Warning("Setting texture using non-initialized texture setter.");
      }
      else
      {
        this.m_propBlock.SetVector(LooseProductTextureSetter.TEX_DATA_SHADER_ID, new Vector4((float) ((int) slimId.Value - 1), this.m_pileParams.Scale, this.m_pileParams.OffsetX, this.m_pileParams.OffsetY));
        this.m_renderer.SetPropertyBlock(this.m_propBlock);
      }
    }

    public void SetTexture(LooseProductSlimId slimId, LoosePileTextureParams pileParams)
    {
      if (slimId.IsPhantom)
        Log.Warning("Trying to set phantom to a texture.");
      else if (this.m_propBlock == null)
      {
        Log.Warning("Setting texture using non-initialized texture setter.");
      }
      else
      {
        this.m_propBlock.SetVector(LooseProductTextureSetter.TEX_DATA_SHADER_ID, new Vector4((float) ((int) slimId.Value - 1), pileParams.Scale, pileParams.OffsetX, pileParams.OffsetY));
        this.m_renderer.SetPropertyBlock(this.m_propBlock);
      }
    }

    static LooseProductTextureSetter()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LooseProductTextureSetter.TEX_DATA_SHADER_ID = Shader.PropertyToID("_TexIndexScaleOffset");
    }
  }
}
