// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Weather.SkyboxController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Unity.Camera;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Weather
{
  /// <summary>Controls rendering of the skybox.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class SkyboxController : IDisposable
  {
    private readonly CameraController m_cameraController;
    private readonly LightController m_lightController;
    private static readonly int CLOUD_THRESHOLD_SHADER_ID;
    private static readonly int SKY_COLOR_SHADER_ID;
    private static readonly int RENDER_COOKIE_SHADER_ID;
    private static readonly int FOG_COLOR_SHADER_ID;
    private static readonly int CURRENT_SKY_COLOR_SHADER_ID;
    private static readonly int FLOOR_COLOR_SHADER_ID;
    private static readonly int FACE_SHADER_ID;
    private static readonly int MAX_COOKIE_STRENGTH_SHADER_ID;
    private static readonly Color SKY_CLOUDS_COLOR;
    private readonly GameObject m_skyBox;
    private readonly MeshRenderer m_skyBoxMr;
    private readonly RenderTexture m_cookieTexture;
    private readonly RenderTexture m_skyboxReflectionCubemap;
    private readonly Material m_skyboxReflectionMaterial;
    private readonly Texture m_skyboxReflectionTexture;
    private readonly CommandBuffer m_cookieCommandBuffer;

    public SkyboxController(
      IGameLoopEvents gameLoopEvents,
      CameraController cameraController,
      LightController lightController,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cameraController = cameraController;
      this.m_lightController = lightController;
      this.m_skyBox = assetsDb.GetClonedPrefabOrEmptyGo("Assets/Unity/Skybox/Skybox.prefab");
      this.m_skyBoxMr = this.m_skyBox.GetComponent<MeshRenderer>();
      this.m_skyBoxMr.material.SetInt(SkyboxController.RENDER_COOKIE_SHADER_ID, 0);
      this.m_skyBoxMr.material.SetFloat(SkyboxController.MAX_COOKIE_STRENGTH_SHADER_ID, 0.4f);
      this.m_cookieTexture = new RenderTexture(4096, 4096, 0);
      this.m_cookieTexture.wrapMode = TextureWrapMode.Repeat;
      this.m_skyboxReflectionCubemap = new RenderTexture(32, 32, 0);
      this.m_skyboxReflectionCubemap.wrapMode = TextureWrapMode.Repeat;
      this.m_skyboxReflectionCubemap.dimension = TextureDimension.Cube;
      RenderSettings.customReflectionTexture = (Texture) this.m_skyboxReflectionCubemap;
      this.m_skyboxReflectionMaterial = new Material(assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/Skybox/SkyReflections.shader"));
      this.m_skyboxReflectionTexture = (Texture) new Texture2D(32, 32);
      this.m_skyboxReflectionMaterial.SetColor(SkyboxController.FLOOR_COLOR_SHADER_ID, this.m_cameraController.Camera.backgroundColor);
      Material sharedMaterial = assetsDb.GetSharedMaterial("Assets/Unity/Skybox/LightCookie.mat");
      this.m_cookieCommandBuffer = new CommandBuffer();
      this.m_cookieCommandBuffer.Blit((Texture) this.m_cookieTexture, (RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, sharedMaterial);
      Shader.SetGlobalInt("_Mafi_CloudCookieResolution", 4096);
      Shader.SetGlobalVector("_Mafi_CloudSpeed", new Vector4(0.00011f, 0.00016f, 0.0f, 0.0f));
      Shader.SetGlobalTexture("_Mafi_CookieTexture", (Texture) this.m_cookieTexture);
      this.m_lightController.AddLightCommandBuffer(LightEvent.AfterScreenspaceMask, this.m_cookieCommandBuffer);
      gameLoopEvents.RenderUpdate.AddNonSaveable<SkyboxController>(this, new Action<GameTime>(this.rendererUpdate));
    }

    public void Dispose()
    {
      this.m_cookieTexture.DestroyIfNotNull();
      this.m_skyboxReflectionCubemap.DestroyIfNotNull();
      this.m_skyboxReflectionMaterial.DestroyIfNotNull();
      this.m_skyboxReflectionTexture.DestroyIfNotNull();
      this.m_cookieCommandBuffer.Release();
    }

    public void SetCloudIntensity(float intensity)
    {
      this.m_skyBoxMr.material.SetFloat(SkyboxController.CLOUD_THRESHOLD_SHADER_ID, intensity);
      this.renderLightCookie();
    }

    public void UpdateSkyColor(Color color, float cloudIntensity)
    {
      Color from = color;
      Color color1 = (double) cloudIntensity <= 0.5 ? from : from.Lerp(SkyboxController.SKY_CLOUDS_COLOR, (float) (((double) cloudIntensity - 0.5) * 3.0));
      Shader.SetGlobalColor(SkyboxController.CURRENT_SKY_COLOR_SHADER_ID, color1.linear * 0.7f);
      for (int face = 0; face < 6; ++face)
      {
        Graphics.SetRenderTarget(this.m_skyboxReflectionCubemap, 0, (CubemapFace) face);
        this.m_skyboxReflectionMaterial.SetInt(SkyboxController.FACE_SHADER_ID, face);
        Graphics.Blit(this.m_skyboxReflectionTexture, this.m_skyboxReflectionMaterial);
      }
    }

    private void renderLightCookie()
    {
      this.m_skyBoxMr.material.SetInt(SkyboxController.RENDER_COOKIE_SHADER_ID, 1);
      RenderTexture active = RenderTexture.active;
      Graphics.Blit(this.m_skyBoxMr.material.mainTexture, this.m_cookieTexture, this.m_skyBoxMr.material);
      Graphics.SetRenderTarget(active);
      this.m_skyBoxMr.material.SetInt(SkyboxController.RENDER_COOKIE_SHADER_ID, 0);
    }

    public void SetSkyColor(Color color)
    {
      this.m_skyBoxMr.material.SetColor(SkyboxController.SKY_COLOR_SHADER_ID, color);
    }

    public void SetFogColor(Color color)
    {
      this.m_skyBoxMr.material.SetColor(SkyboxController.FOG_COLOR_SHADER_ID, color);
      this.m_skyboxReflectionMaterial.SetColor(SkyboxController.FOG_COLOR_SHADER_ID, color);
    }

    private void rendererUpdate(GameTime time)
    {
      this.m_skyBox.transform.position = new Vector3(this.m_cameraController.Camera.transform.position.x, (float) (-(double) this.m_cameraController.Camera.farClipPlane / 1000.0), this.m_cameraController.Camera.transform.position.z);
      float num = this.m_cameraController.Camera.farClipPlane * 0.95f;
      this.m_skyBox.transform.localScale = new Vector3(num * 2f, num / 2f, num * 2f);
    }

    static SkyboxController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SkyboxController.CLOUD_THRESHOLD_SHADER_ID = Shader.PropertyToID("_CloudIntensity");
      SkyboxController.SKY_COLOR_SHADER_ID = Shader.PropertyToID("_BaseSkyColor");
      SkyboxController.RENDER_COOKIE_SHADER_ID = Shader.PropertyToID("_RenderCookie");
      SkyboxController.FOG_COLOR_SHADER_ID = Shader.PropertyToID("_BaseFogColor");
      SkyboxController.CURRENT_SKY_COLOR_SHADER_ID = Shader.PropertyToID("_MafiSkyColor");
      SkyboxController.FLOOR_COLOR_SHADER_ID = Shader.PropertyToID("_FloorColor");
      SkyboxController.FACE_SHADER_ID = Shader.PropertyToID("_Face");
      SkyboxController.MAX_COOKIE_STRENGTH_SHADER_ID = Shader.PropertyToID("_MaxCookieStrength");
      SkyboxController.SKY_CLOUDS_COLOR = new Color(0.8f, 0.8f, 0.8f);
    }
  }
}
