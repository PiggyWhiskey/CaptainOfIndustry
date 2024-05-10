// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts.PostEffectsBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts
{
  [RequireComponent(typeof (Camera))]
  [ExecuteInEditMode]
  public class PostEffectsBase : MonoBehaviour
  {
    protected bool supportHDRTextures;
    protected bool supportDX11;
    protected bool isSupported;

    protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
    {
      if (!(bool) (Object) s)
      {
        Debug.Log((object) ("Missing shader in " + this.ToString()));
        this.enabled = false;
        return (Material) null;
      }
      if (s.isSupported && (bool) (Object) m2Create && (Object) m2Create.shader == (Object) s)
        return m2Create;
      if (!s.isSupported)
      {
        this.NotSupported();
        Debug.Log((object) ("The shader " + s.ToString() + " on effect " + this.ToString() + " is not supported on this platform!"));
        return (Material) null;
      }
      m2Create = new Material(s);
      m2Create.hideFlags = HideFlags.DontSave;
      return (bool) (Object) m2Create ? m2Create : (Material) null;
    }

    protected Material CreateMaterial(Shader s, Material m2Create)
    {
      if (!(bool) (Object) s)
      {
        Debug.Log((object) ("Missing shader in " + this.ToString()));
        return (Material) null;
      }
      if ((bool) (Object) m2Create && (Object) m2Create.shader == (Object) s && s.isSupported)
        return m2Create;
      if (!s.isSupported)
        return (Material) null;
      m2Create = new Material(s);
      m2Create.hideFlags = HideFlags.DontSave;
      return (bool) (Object) m2Create ? m2Create : (Material) null;
    }

    private void OnEnable() => this.isSupported = true;

    protected bool CheckSupport() => this.CheckSupport(false);

    public virtual bool CheckResources()
    {
      Debug.LogWarning((object) ("CheckResources () for " + this.ToString() + " should be overwritten."));
      return this.isSupported;
    }

    protected void Start() => this.CheckResources();

    protected bool CheckSupport(bool needDepth)
    {
      this.isSupported = true;
      this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
      this.supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
      if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
      {
        this.NotSupported();
        return false;
      }
      if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
      {
        this.NotSupported();
        return false;
      }
      if (needDepth)
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
      return true;
    }

    protected bool CheckSupport(bool needDepth, bool needHdr)
    {
      if (!this.CheckSupport(needDepth))
        return false;
      if (!needHdr || this.supportHDRTextures)
        return true;
      this.NotSupported();
      return false;
    }

    public bool Dx11Support() => this.supportDX11;

    protected void ReportAutoDisable()
    {
      Debug.LogWarning((object) ("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform."));
    }

    private bool CheckShader(Shader s)
    {
      Debug.Log((object) ("The shader " + s.ToString() + " on effect " + this.ToString() + " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."));
      if (s.isSupported)
        return false;
      this.NotSupported();
      return false;
    }

    protected void NotSupported()
    {
      this.enabled = false;
      this.isSupported = false;
    }

    public PostEffectsBase()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.supportHDRTextures = true;
      this.isSupported = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
