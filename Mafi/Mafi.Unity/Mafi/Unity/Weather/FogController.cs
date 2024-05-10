// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Weather.FogController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Camera;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Weather
{
  /// <summary>Controls fog in the game.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class FogController : IDisposable
  {
    public const string FOG_QUAD_NAME = "FogQuad";
    /// <summary>Fog density at 100% intensity.</summary>
    public const float MAX_FOG_DENSITY = 0.0012f;
    private readonly GameObject m_fogQuad;
    private static readonly int MAFI_FOG_COLOR;
    private static readonly int MAFI_FOG_DENSITY;
    private float m_fogDensity;
    private readonly FogRendererMb m_fogRendererMb;

    public float FogDensity => this.m_fogDensity;

    public Color FogColor => RenderSettings.fogColor;

    internal bool IsFogEnabled => this.m_fogQuad.gameObject.activeSelf;

    public FogController(CameraController cameraController, AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_fogQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
      this.m_fogQuad.gameObject.name = "FogQuad";
      this.m_fogQuad.transform.SetParent(cameraController.Camera.transform);
      MeshCollider component = this.m_fogQuad.GetComponent<MeshCollider>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) component);
      Shader sharedAssetOrThrow = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/Skybox/forwardFog.shader");
      this.m_fogRendererMb = this.m_fogQuad.AddComponent<FogRendererMb>();
      this.m_fogRendererMb.Initialize(cameraController, sharedAssetOrThrow);
    }

    internal void UpdateFogNow() => this.m_fogRendererMb.LateUpdate();

    internal void SetFogRenderingState(bool isEnabled)
    {
      this.m_fogQuad.SetActive(isEnabled);
      float num = isEnabled ? this.m_fogDensity : 0.0f;
      RenderSettings.fogDensity = num;
      Shader.SetGlobalFloat(FogController.MAFI_FOG_DENSITY, num);
    }

    public void SetFogIntensity(float percent) => this.SetFogDensity(percent * 0.0012f);

    public void SetFogDensity(float density)
    {
      this.m_fogDensity = density;
      if (!this.m_fogQuad.gameObject.activeSelf)
        return;
      RenderSettings.fogDensity = this.m_fogDensity;
      Shader.SetGlobalFloat(FogController.MAFI_FOG_DENSITY, this.m_fogDensity);
    }

    public void SetFogColor(Color color)
    {
      RenderSettings.fogColor = color;
      Shader.SetGlobalColor(FogController.MAFI_FOG_COLOR, color.linear);
    }

    public void Dispose()
    {
      GameObject fogQuad = this.m_fogQuad;
      if (fogQuad == null)
        return;
      fogQuad.Destroy();
    }

    static FogController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FogController.MAFI_FOG_COLOR = Shader.PropertyToID("_MafiFogColor");
      FogController.MAFI_FOG_DENSITY = Shader.PropertyToID("_MafiFogDensity");
    }
  }
}
