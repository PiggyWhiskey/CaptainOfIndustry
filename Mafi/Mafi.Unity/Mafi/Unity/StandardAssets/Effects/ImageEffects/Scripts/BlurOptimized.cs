// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts.BlurOptimized
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (Camera))]
  public class BlurOptimized : PostEffectsBase
  {
    internal static readonly int PARAMETER_TEX_ID;
    [Range(0.0f, 2f)]
    public int DownsamplePasses;
    public BlurOptimized.BlurTypeEnum BlurType;
    [Range(0.0f, 10f)]
    public float BlurSize;
    [Range(1f, 4f)]
    public int BlurIterations;
    private Shader m_shader;
    private Material m_blurMaterial;

    public void Initialize(Shader shader)
    {
      this.m_shader = shader;
      this.CheckResources();
    }

    public override bool CheckResources()
    {
      this.CheckSupport(false);
      this.m_blurMaterial = this.CheckShaderAndCreateMaterial(this.m_shader, this.m_blurMaterial);
      if (!this.isSupported)
      {
        this.ReportAutoDisable();
        this.gameObject.SetActive(false);
      }
      else
        Assert.That<int>(this.m_blurMaterial.passCount).IsGreaterOrEqual(3);
      return this.isSupported;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      this.PerformBlur(this.BlurSize, this.DownsamplePasses, this.BlurIterations, source, destination);
    }

    public void PerformBlur(
      float blurSize,
      int downsamplePasses,
      int blurIterations,
      RenderTexture source,
      RenderTexture destination)
    {
      if (!this.isSupported)
      {
        Graphics.Blit((Texture) source, destination);
      }
      else
      {
        float x = blurSize / (float) (1 << downsamplePasses);
        this.m_blurMaterial.SetVector(BlurOptimized.PARAMETER_TEX_ID, new Vector4(x, -x, 0.0f, 0.0f));
        source.filterMode = FilterMode.Bilinear;
        int width = source.width >> downsamplePasses;
        int height = source.height >> downsamplePasses;
        RenderTexture renderTexture1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        renderTexture1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) source, renderTexture1, this.m_blurMaterial, 0);
        int num = this.BlurType == BlurOptimized.BlurTypeEnum.StandardGauss ? 0 : 2;
        for (int index = 0; index < blurIterations; ++index)
        {
          this.m_blurMaterial.SetVector(BlurOptimized.PARAMETER_TEX_ID, new Vector4(x + (float) index, -x - (float) index, 0.0f, 0.0f));
          RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
          temporary1.filterMode = FilterMode.Bilinear;
          Graphics.Blit((Texture) renderTexture1, temporary1, this.m_blurMaterial, 1 + num);
          RenderTexture.ReleaseTemporary(renderTexture1);
          RenderTexture renderTexture2 = temporary1;
          RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
          temporary2.filterMode = FilterMode.Bilinear;
          Graphics.Blit((Texture) renderTexture2, temporary2, this.m_blurMaterial, 2 + num);
          RenderTexture.ReleaseTemporary(renderTexture2);
          renderTexture1 = temporary2;
        }
        Graphics.Blit((Texture) renderTexture1, destination);
        RenderTexture.ReleaseTemporary(renderTexture1);
      }
    }

    public BlurOptimized()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.DownsamplePasses = 1;
      this.BlurSize = 3f;
      this.BlurIterations = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BlurOptimized()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BlurOptimized.PARAMETER_TEX_ID = Shader.PropertyToID("_Parameter");
    }

    public enum BlurTypeEnum
    {
      StandardGauss,
      SgxGauss,
    }
  }
}
