// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.LooseProductMaterialManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory;
using Mafi.Core.Products;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LooseProductMaterialManager : IDisposable
  {
    private readonly Material m_sharedMaterial;
    private readonly Material m_mixedSharedMaterial;
    public readonly Texture2DArray AlbedoTexArray;
    public readonly Texture2DArray NormalsTexArray;
    public readonly Texture2DArray SmoothMetalTexArray;
    internal static readonly int TEX_DATA_SHADER_ID_0;
    internal static readonly int TEX_DATA_SHADER_ID_1;
    internal static readonly int TEX_DATA_SHADER_ID_2;
    internal static readonly int TEX_DATA_SHADER_ID_3;
    internal static readonly int TEX_DATA_SHADER_ID_4;
    internal static readonly int TEX_DATA_SHADER_ID_5;
    internal static readonly int TEX_DATA_SHADER_ID_6;
    internal static readonly int TEX_DATA_SHADER_ID_7;
    internal static readonly int MATERIAL_RATIOS;
    internal static readonly int MATERIAL_RATIOS_2;
    internal static readonly int MODEL_UVS;

    public LooseProductMaterialManager(LooseProductsSlimIdManager slimIdManager, AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ImmutableArray<LooseProductProto> protos = slimIdManager.ManagedProtos.RemoveAt(0);
      Material result;
      Material[] materialArray = protos.MapArray<Material>((Func<LooseProductProto, Material>) (x => !assetsDb.TryGetSharedAsset<Material>(x.Graphics.PileMaterialAssetPath, out result) ? (Material) null : result));
      int albedoTexId = Shader.PropertyToID("_AlbedoTex");
      int normalsTexId = Shader.PropertyToID("_NormalsTex");
      int smoothMetalTexId = Shader.PropertyToID("_SmoothMetalTex");
      Material material = ((IEnumerable<Material>) materialArray).FirstOrDefault<Material>((Func<Material, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null));
      for (int index = 0; index < materialArray.Length; ++index)
      {
        if ((UnityEngine.Object) materialArray[index] == (UnityEngine.Object) null)
          materialArray[index] = material;
      }
      this.AlbedoTexArray = TextureArrayUtils.Create2DArrayOrThrow("LoosePileAlbedoArray", materialArray.MapArray<Material, Texture2D>((Func<Material, int, Texture2D>) ((x, i) =>
      {
        if (x.HasProperty(albedoTexId))
          return (Texture2D) x.GetTexture(albedoTexId);
        Log.Error(string.Format("Failed to extract texture '_AlbedoTex' from material '{0}' of product ", (object) x) + string.Format("{0} ({1}).", (object) protos[i].Id, (object) protos[i].Graphics.PileMaterialAssetPath));
        return (Texture2D) null;
      })), false);
      this.NormalsTexArray = TextureArrayUtils.Create2DArrayOrThrow("LoosePileNormArray", materialArray.MapArray<Material, Texture2D>((Func<Material, int, Texture2D>) ((x, i) =>
      {
        if (x.HasProperty(normalsTexId))
          return (Texture2D) x.GetTexture(normalsTexId);
        Log.Error(string.Format("Failed to extract texture '_NormalsTex' from material '{0}' of product ", (object) x) + string.Format("{0} ({1}).", (object) protos[i].Id, (object) protos[i].Graphics.PileMaterialAssetPath));
        return (Texture2D) null;
      })), true);
      this.SmoothMetalTexArray = TextureArrayUtils.Create2DArrayOrThrow("LoosePileSmoothMetalArray", materialArray.MapArray<Material, Texture2D>((Func<Material, int, Texture2D>) ((x, i) =>
      {
        if (x.HasProperty(smoothMetalTexId))
          return (Texture2D) x.GetTexture(smoothMetalTexId);
        Log.Error(string.Format("Failed to extract texture '_SmoothMetalTex' from material '{0}' of product ", (object) x) + string.Format("{0} ({1}).", (object) protos[i].Id, (object) protos[i].Graphics.PileMaterialAssetPath));
        return (Texture2D) null;
      })), true);
      this.m_sharedMaterial = assetsDb.GetClonedMaterial("Assets/Base/Products/Loose/LooseMaterialPileArray.mat");
      this.m_sharedMaterial.SetTexture("_AlbedoTexArray", (Texture) this.AlbedoTexArray);
      this.m_sharedMaterial.SetTexture("_NormalsTexArray", (Texture) this.NormalsTexArray);
      this.m_sharedMaterial.SetTexture("_SmoothMetalTexArray", (Texture) this.SmoothMetalTexArray);
      this.m_sharedMaterial.SetVector("_TexIndexScaleOffset", new Vector4(0.0f, 1f, 0.0f, 0.0f));
      this.m_mixedSharedMaterial = assetsDb.GetClonedMaterial("Assets/Base/Products/Loose/MixedLooseMaterialPileArray.mat");
      this.m_mixedSharedMaterial.SetTexture("_AlbedoTexArray", (Texture) this.AlbedoTexArray);
      this.m_mixedSharedMaterial.SetTexture("_NormalsTexArray", (Texture) this.NormalsTexArray);
      this.m_mixedSharedMaterial.SetTexture("_SmoothMetalTexArray", (Texture) this.SmoothMetalTexArray);
      this.m_mixedSharedMaterial.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_0, new Vector4(0.0f, 1f, 0.0f, 0.0f));
      this.m_mixedSharedMaterial.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_1, new Vector4(0.0f, 1f, 0.0f, 0.0f));
      this.m_mixedSharedMaterial.SetVector(LooseProductMaterialManager.TEX_DATA_SHADER_ID_2, new Vector4(0.0f, 1f, 0.0f, 0.0f));
      this.m_mixedSharedMaterial.SetVector(LooseProductMaterialManager.MATERIAL_RATIOS, new Vector4(1f, 0.0f, 0.0f, 0.0f));
      this.m_mixedSharedMaterial.SetVector(LooseProductMaterialManager.MODEL_UVS, new Vector4(0.0f, 1f, 0.0f, 0.0f));
    }

    public void Dispose()
    {
      this.m_sharedMaterial.DestroyIfNotNull();
      this.m_mixedSharedMaterial.DestroyIfNotNull();
      this.AlbedoTexArray.DestroyIfNotNull();
      this.NormalsTexArray.DestroyIfNotNull();
      this.SmoothMetalTexArray.DestroyIfNotNull();
    }

    public LooseProductTextureSetter SetupSharedMaterialFor(
      Renderer renderer,
      LoosePileTextureParams pileParams = default (LoosePileTextureParams))
    {
      MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
      renderer.sharedMaterial = this.m_sharedMaterial;
      if ((double) pileParams.Scale == 0.0)
        pileParams = LoosePileTextureParams.Default;
      return new LooseProductTextureSetter(renderer, propBlock, pileParams);
    }

    public LooseProductMultiTextureSetter SetupSharedMaterialFor(
      Renderer[] renderers,
      LoosePileTextureParams pileParams = default (LoosePileTextureParams))
    {
      MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
      foreach (Renderer renderer in renderers)
        renderer.sharedMaterial = this.m_sharedMaterial;
      if ((double) pileParams.Scale == 0.0)
        pileParams = LoosePileTextureParams.Default;
      return new LooseProductMultiTextureSetter(renderers, propBlock, pileParams);
    }

    public LooseMixedProductsTextureSetter SetupMixedSharedMaterialsFor(
      Renderer renderer,
      LoosePileTextureParams pileParams = default (LoosePileTextureParams))
    {
      MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
      renderer.sharedMaterial = this.m_mixedSharedMaterial;
      if ((double) pileParams.Scale == 0.0)
        pileParams = LoosePileTextureParams.Default;
      return new LooseMixedProductsTextureSetter(renderer, propBlock, pileParams);
    }

    static LooseProductMaterialManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_0 = Shader.PropertyToID("_TexIndexScaleOffset0");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_1 = Shader.PropertyToID("_TexIndexScaleOffset1");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_2 = Shader.PropertyToID("_TexIndexScaleOffset2");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_3 = Shader.PropertyToID("_TexIndexScaleOffset3");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_4 = Shader.PropertyToID("_TexIndexScaleOffset4");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_5 = Shader.PropertyToID("_TexIndexScaleOffset5");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_6 = Shader.PropertyToID("_TexIndexScaleOffset6");
      LooseProductMaterialManager.TEX_DATA_SHADER_ID_7 = Shader.PropertyToID("_TexIndexScaleOffset7");
      LooseProductMaterialManager.MATERIAL_RATIOS = Shader.PropertyToID("_MaterialRatios");
      LooseProductMaterialManager.MATERIAL_RATIOS_2 = Shader.PropertyToID("_MaterialRatios2");
      LooseProductMaterialManager.MODEL_UVS = Shader.PropertyToID("_ModelUVs");
    }
  }
}
