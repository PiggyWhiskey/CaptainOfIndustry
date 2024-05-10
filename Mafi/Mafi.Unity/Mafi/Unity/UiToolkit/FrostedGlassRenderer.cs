// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.FrostedGlassRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit
{
  public class FrostedGlassRenderer : MonoBehaviour
  {
    internal static readonly int PARAMETER_TEX_ID;
    [Range(0.0f, 2f)]
    public int DownsamplePasses;
    public FrostedGlassRenderer.BlurTypeEnum BlurType;
    [Range(0.0f, 10f)]
    public float BlurSize;
    [Range(1f, 4f)]
    public int BlurIterations;
    public Shader Shader;
    public RenderTexture BlurredTexture;
    private Material m_blurMaterial;
    private float m_sampleOffset;
    private int m_passOffset;

    public event Action<RenderTexture> OnTextureChanged;

    public void OnEnable()
    {
      this.m_blurMaterial = new Material(this.Shader);
      this.BlurredTexture = new RenderTexture(1, 1, 0);
      this.NotifyParametersUpdated();
    }

    public void OnDestroy()
    {
      this.m_blurMaterial.DestroyIfNotNull();
      this.BlurredTexture.DestroyIfNotNull();
    }

    public void NotifyParametersUpdated()
    {
      this.m_sampleOffset = this.BlurSize / (float) (1 << this.DownsamplePasses);
      this.m_passOffset = this.BlurType == FrostedGlassRenderer.BlurTypeEnum.StandardGauss ? 0 : 2;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      Graphics.Blit((Texture) source, destination);
      if (this.BlurredTexture.width != source.width || this.BlurredTexture.height != source.height)
      {
        this.BlurredTexture.DestroyIfNotNull();
        Log.Warning("Making new RG for blur effect.");
        this.BlurredTexture = new RenderTexture(source.width, source.height, 0, source.format);
        this.BlurredTexture.filterMode = FilterMode.Bilinear;
        Action<RenderTexture> onTextureChanged = this.OnTextureChanged;
        if (onTextureChanged != null)
          onTextureChanged(this.BlurredTexture);
      }
      this.BlurredTexture.DiscardContents();
      this.m_blurMaterial.SetVector(FrostedGlassRenderer.PARAMETER_TEX_ID, new Vector4(this.m_sampleOffset, -this.m_sampleOffset, 0.0f, 0.0f));
      source.filterMode = FilterMode.Bilinear;
      int width = source.width >> this.DownsamplePasses;
      int height = source.height >> this.DownsamplePasses;
      RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
      renderTexture.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, renderTexture, this.m_blurMaterial, 0);
      for (int index = 0; index < this.BlurIterations; ++index)
      {
        this.m_blurMaterial.SetVector(FrostedGlassRenderer.PARAMETER_TEX_ID, new Vector4(this.m_sampleOffset + (float) index, -this.m_sampleOffset - (float) index, 0.0f, 0.0f));
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture, temporary1, this.m_blurMaterial, this.m_passOffset + 1);
        RenderTexture.ReleaseTemporary(renderTexture);
        renderTexture = temporary1;
        if (index + 1 == this.BlurIterations)
        {
          Graphics.Blit((Texture) renderTexture, this.BlurredTexture, this.m_blurMaterial, this.m_passOffset + 2);
        }
        else
        {
          RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
          temporary2.filterMode = FilterMode.Bilinear;
          Graphics.Blit((Texture) renderTexture, temporary2, this.m_blurMaterial, this.m_passOffset + 2);
          RenderTexture.ReleaseTemporary(renderTexture);
          renderTexture = temporary2;
        }
      }
      RenderTexture.ReleaseTemporary(renderTexture);
    }

    public FrostedGlassRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.DownsamplePasses = 1;
      this.BlurSize = 3f;
      this.BlurIterations = 2;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FrostedGlassRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FrostedGlassRenderer.PARAMETER_TEX_ID = Shader.PropertyToID("_Parameter");
    }

    public enum BlurTypeEnum
    {
      StandardGauss,
      SgxGauss,
    }
  }
}
