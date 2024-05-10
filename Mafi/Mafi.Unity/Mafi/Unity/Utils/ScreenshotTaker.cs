// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ScreenshotTaker
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.ResVis;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Mine;
using Mafi.Unity.Terrain;
using Mafi.Unity.Weather;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.PostProcessing;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ScreenshotTaker
  {
    private readonly CameraController m_cameraController;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly WeatherController m_weatherController;
    private readonly LightController m_lightController;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly ObjectHighlighter m_objectHighlighter;
    private readonly TowerAreasRenderer m_towerAreasRenderer;
    private readonly ResVisBarsRenderer m_resVisBarsRenderer;
    private readonly FogController m_fogController;
    private readonly ChunkBasedRenderingManager m_chunkBasedRenderingManager;
    private readonly ProtosDb m_protosDb;
    private readonly Stopwatch m_stopwatch;
    private Option<Texture2D> m_screenshotTexture;
    private readonly Lyst<ScreenshotTaker.ScreenshotCaptureData> m_screensDataToCapture;
    private int? m_maxLodToRestore;
    private bool? m_cullingToRestore;

    public ScreenshotTaker(
      CameraController cameraController,
      IFileSystemHelper fileSystemHelper,
      IGameLoopEvents gameLoopEvents,
      WeatherController weatherController,
      LightController lightController,
      TerrainRenderer terrainRenderer,
      ObjectHighlighter objectHighlighter,
      TowerAreasRenderer towerAreasRenderer,
      ResVisBarsRenderer resVisBarsRenderer,
      FogController fogController,
      ChunkBasedRenderingManager chunkBasedRenderingManager,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_stopwatch = new Stopwatch();
      this.m_screensDataToCapture = new Lyst<ScreenshotTaker.ScreenshotCaptureData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cameraController = cameraController;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_weatherController = weatherController;
      this.m_lightController = lightController;
      this.m_terrainRenderer = terrainRenderer;
      this.m_objectHighlighter = objectHighlighter;
      this.m_towerAreasRenderer = towerAreasRenderer;
      this.m_resVisBarsRenderer = resVisBarsRenderer;
      this.m_fogController = fogController;
      this.m_chunkBasedRenderingManager = chunkBasedRenderingManager;
      this.m_protosDb = protosDb;
      gameLoopEvents.RenderUpdateEnd.AddNonSaveable<ScreenshotTaker>(this, new Action<GameTime>(this.renderUpdateLate));
    }

    [ConsoleCommand(true, false, null, null)]
    private void captureAndSaveScreenshot(
      int customWidth = 0,
      int customHeight = 0,
      bool disableSuperSmooth = false,
      bool saveAsPng = false,
      string customName = null)
    {
      Option<string> timestampedFilePath = (Option<string>) this.m_fileSystemHelper.GetTimestampedFilePath(customName == null ? "_photo" : "_" + customName, FileType.Screenshot, true);
      int customWidth1 = customWidth;
      int customHeight1 = customHeight;
      bool flag1 = !disableSuperSmooth;
      bool flag2 = !saveAsPng;
      Vector3? cameraPosition = new Vector3?();
      Quaternion? cameraRotation = new Quaternion?();
      float? verticalFieldOfView = new float?();
      Vector2? nearFarClipPlane = new Vector2?();
      int num1 = flag1 ? 1 : 0;
      int num2 = flag2 ? 1 : 0;
      float? customFogDensity = new float?();
      this.ScheduleScreenshotCapture(timestampedFilePath, customWidth: customWidth1, customHeight: customHeight1, cameraPosition: cameraPosition, cameraRotation: cameraRotation, verticalFieldOfView: verticalFieldOfView, nearFarClipPlane: nearFarClipPlane, downscaleFromDoubleRes: num1 != 0, saveAsJpg: num2 != 0, customFogDensity: customFogDensity);
    }

    private void renderUpdateLate(GameTime time)
    {
      if (!this.m_screensDataToCapture.IsNotEmpty)
        return;
      foreach (ScreenshotTaker.ScreenshotCaptureData data in this.m_screensDataToCapture)
        this.tryCaptureAndSave(data, out string _);
      this.m_screensDataToCapture.Clear();
      if (this.m_maxLodToRestore.HasValue)
      {
        this.m_chunkBasedRenderingManager.SetMaxLod(this.m_maxLodToRestore.Value);
        this.m_maxLodToRestore = new int?();
      }
      if (!this.m_cullingToRestore.HasValue)
        return;
      this.m_chunkBasedRenderingManager.SetCullingEnabled(this.m_cullingToRestore.Value);
      this.m_cullingToRestore = new bool?();
    }

    /// <summary>
    /// Schedules a screenshot capture. The actual capture is done during render-update-end phase so that all instanced
    /// geometry is properly scheduled for rendering.
    /// </summary>
    /// <remarks>
    /// If file path is provided, screenshot will be saved in a separate thread to not block execution.
    /// If encodedDataCallback is provided instead, it is called synchronously in the render-update-end phase.
    /// Path and data callback should not be set at the same time.
    /// </remarks>
    public void ScheduleScreenshotCapture(
      Option<string> filePathWithoutExt = default (Option<string>),
      Action<Option<Texture2D>, UnityEngine.Camera, string> encodedDataCallback = null,
      int customWidth = 0,
      int customHeight = 0,
      Vector3? cameraPosition = null,
      Quaternion? cameraRotation = null,
      float? verticalFieldOfView = null,
      Vector2? nearFarClipPlane = null,
      bool downscaleFromDoubleRes = false,
      bool saveAsJpg = false,
      ScreenshotConfigFlags configFlags = (ScreenshotConfigFlags) 0,
      int jpgEncodingQuality = 90,
      float? customFogDensity = null)
    {
      Mafi.Assert.That<bool>(filePathWithoutExt.HasValue).IsNotEqualTo<bool>(encodedDataCallback != null, "Either file path or data callback should be provided.");
      this.m_stopwatch.Restart();
      UnityEngine.Camera camera = this.m_cameraController.Camera;
      Option<string> filePathWithoutExt1 = filePathWithoutExt;
      int num1 = customWidth > 0 ? customWidth : camera.pixelWidth;
      int num2 = customHeight > 0 ? customHeight : camera.pixelHeight;
      Vector3 vector3 = cameraPosition ?? camera.transform.localPosition;
      Quaternion quaternion = cameraRotation ?? camera.transform.localRotation;
      float num3 = (float) ((double) verticalFieldOfView ?? (double) camera.fieldOfView);
      Vector2 vector2 = nearFarClipPlane ?? new Vector2(camera.nearClipPlane, camera.farClipPlane);
      bool flag1 = downscaleFromDoubleRes;
      bool flag2 = saveAsJpg;
      ScreenshotConfigFlags screenshotConfigFlags = configFlags;
      int num4 = jpgEncodingQuality;
      Option<Action<Option<Texture2D>, UnityEngine.Camera, string>> encodedDataCallback1 = (Option<Action<Option<Texture2D>, UnityEngine.Camera, string>>) encodedDataCallback;
      int width = num1;
      int height = num2;
      Vector3 cameraPosition1 = vector3;
      Quaternion cameraRotation1 = quaternion;
      double verticalFieldOfView1 = (double) num3;
      Vector2 nearFarClipPlane1 = vector2;
      int num5 = flag1 ? 1 : 0;
      int num6 = flag2 ? 1 : 0;
      int configFlags1 = (int) screenshotConfigFlags;
      int jpgEncodingQuality1 = num4;
      float? customFogDensity1 = customFogDensity;
      ScreenshotTaker.ScreenshotCaptureData screenshotCaptureData = new ScreenshotTaker.ScreenshotCaptureData(filePathWithoutExt1, encodedDataCallback1, width, height, cameraPosition1, cameraRotation1, (float) verticalFieldOfView1, nearFarClipPlane1, num5 != 0, num6 != 0, (ScreenshotConfigFlags) configFlags1, jpgEncodingQuality1, customFogDensity1);
      if (configFlags.IsFlagged(ScreenshotConfigFlags.ForceLod0))
      {
        if (!this.m_maxLodToRestore.HasValue)
          this.m_maxLodToRestore = new int?(this.m_chunkBasedRenderingManager.MaxLod);
        this.m_chunkBasedRenderingManager.SetMaxLod(0);
      }
      if (configFlags.IsFlagged(ScreenshotConfigFlags.ForceNoChunkCulling))
      {
        if (!this.m_cullingToRestore.HasValue)
          this.m_cullingToRestore = new bool?(!this.m_chunkBasedRenderingManager.CullingIsDisabled);
        this.m_chunkBasedRenderingManager.SetCullingEnabled(false);
      }
      this.m_screensDataToCapture.Add(screenshotCaptureData);
    }

    private bool tryCaptureAndSave(ScreenshotTaker.ScreenshotCaptureData data, out string error)
    {
      StateAssert.IsInGameState(GameLoopState.RenderUpdateEnd, "Screenshots need to be captured in 'RenderUpdateEnd' stage to render instanced geometry.");
      UnityEngine.Camera camera = this.m_cameraController.Camera;
      int cullingMask = camera.cullingMask;
      int num1 = cullingMask;
      PostProcessLayer.Antialiasing antialiasing = PostProcessLayer.Antialiasing.None;
      PostProcessLayer component;
      if (camera.gameObject.TryGetComponent<PostProcessLayer>(out component))
      {
        antialiasing = component.antialiasingMode;
        component.antialiasingMode = PostProcessLayer.Antialiasing.None;
      }
      bool weatherIsVisible = this.m_weatherController.WeatherIsVisible;
      LightController.State state1 = this.m_lightController.GetState();
      float fogDensity = this.m_fogController.FogDensity;
      Color fogColor = this.m_fogController.FogColor;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableWeather))
      {
        this.m_weatherController.SetWeatherEffectsVisibility(false);
        WeatherProto proto;
        if (this.m_protosDb.TryGetProto<WeatherProto>(IdsCore.Weather.Sunny, out proto))
        {
          this.m_lightController.SetLightColor(proto.Graphics.LightColor.AsColor());
          this.m_lightController.SetLightIntensity(proto.Graphics.LightIntensity, proto.Graphics.ShadowsIntensityAbs);
          this.m_lightController.SetExtraLightIntensity(0.0f);
          this.m_fogController.SetFogIntensity(proto.Graphics.FogIntensity);
          this.m_fogController.SetFogColor(proto.Graphics.FogColor.AsColor());
        }
        else
          Mafi.Log.Warning(string.Format("Sunny weather proto '{0}' was not found.", (object) IdsCore.Weather.Sunny));
      }
      if (data.CustomFogDensity.HasValue)
        this.m_fogController.SetFogDensity(data.CustomFogDensity.Value);
      bool isFogEnabled = this.m_fogController.IsFogEnabled;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableFog) != !isFogEnabled)
        this.m_fogController.SetFogRenderingState(!isFogEnabled);
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableIcons))
        num1 &= ~Layer.Custom13Icons.ToMask();
      bool isActive1 = this.m_towerAreasRenderer.IsActive;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableTerrainOverlays))
      {
        this.m_towerAreasRenderer.ForceSetActive(false);
        num1 &= ~Layer.Custom14TerrainOverlays.ToMask();
      }
      bool isActive2 = this.m_terrainRenderer.IsGridActive();
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableTerrainGrid))
        this.m_terrainRenderer.ForceSetGridActive(false);
      bool isActive3 = this.m_objectHighlighter.IsActive;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableHighlights))
        this.m_objectHighlighter.ForceSetActive(false);
      bool isActive4 = this.m_resVisBarsRenderer.IsActive;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableResourceBars))
        this.m_resVisBarsRenderer.ForceSetActive(false);
      Vector3 localPosition = camera.transform.localPosition;
      Quaternion localRotation = camera.transform.localRotation;
      float fieldOfView = camera.fieldOfView;
      Vector2 vector2 = new Vector2(camera.nearClipPlane, camera.farClipPlane);
      camera.cullingMask = num1;
      camera.transform.localPosition = data.CameraPosition;
      camera.transform.localRotation = data.CameraRotation;
      camera.fieldOfView = data.VerticalFieldOfView;
      camera.nearClipPlane = data.NearFarClipPlane.x;
      camera.farClipPlane = data.NearFarClipPlane.y;
      RenderTexture active = RenderTexture.active;
      RenderTexture renderTexture = (RenderTexture) null;
      float lodBias = QualitySettings.lodBias;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.ForceLod0))
        QualitySettings.lodBias = lodBias * 10f;
      int antiAliasing = QualitySettings.antiAliasing;
      if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.Force8xMsaa))
        QualitySettings.antiAliasing = 8;
      try
      {
        int num2 = data.DownscaleFromDoubleRes ? 2 : 1;
        renderTexture = RenderTexture.GetTemporary(data.Width * num2, data.Height * num2, 32);
        camera.targetTexture = renderTexture;
        this.m_fogController.UpdateFogNow();
        camera.Render();
        if (data.DownscaleFromDoubleRes)
        {
          renderTexture.filterMode = FilterMode.Bilinear;
          RenderTexture temporary = RenderTexture.GetTemporary(data.Width, data.Height);
          RenderTexture.active = temporary;
          Graphics.Blit((Texture) renderTexture, temporary);
          RenderTexture.ReleaseTemporary(renderTexture);
          renderTexture = temporary;
        }
        RenderTexture.active = renderTexture;
        Texture2D texture2D = ScreenshotTaker.ensureTextureOfSize(ref this.m_screenshotTexture, data.Width, data.Height, TextureFormat.RGBA32);
        texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), 0, 0);
        texture2D.Apply();
        if (data.FilePathWithoutExt.HasValue)
        {
          Mafi.Assert.That<Option<Action<Option<Texture2D>, UnityEngine.Camera, string>>>(data.EncodedDataCallback).IsNone<Action<Option<Texture2D>, UnityEngine.Camera, string>>("Encoded data callback not supported for file saving.");
          byte[] rawTextureData = texture2D.GetRawTextureData();
          ScreenshotTaker.ScreenSaveData state2 = new ScreenshotTaker.ScreenSaveData(data.FilePathWithoutExt.Value, data.SaveAsJpg, rawTextureData, texture2D.graphicsFormat, texture2D.width, texture2D.height, (float) this.m_stopwatch.Elapsed.TotalMilliseconds);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ThreadPool.QueueUserWorkItem(ScreenshotTaker.\u003C\u003EO.\u003C0\u003E__saveTexture ?? (ScreenshotTaker.\u003C\u003EO.\u003C0\u003E__saveTexture = new WaitCallback(ScreenshotTaker.saveTexture)), (object) state2);
        }
        else
        {
          Mafi.Assert.That<Option<Action<Option<Texture2D>, UnityEngine.Camera, string>>>(data.EncodedDataCallback).HasValue<Action<Option<Texture2D>, UnityEngine.Camera, string>>("Missing encoded data callback.");
          Action<Option<Texture2D>, UnityEngine.Camera, string> valueOrNull = data.EncodedDataCallback.ValueOrNull;
          if (valueOrNull != null)
            valueOrNull((Option<Texture2D>) texture2D, camera, "");
        }
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, "failed to render screenshot.");
        error = ex.Message;
        Action<Option<Texture2D>, UnityEngine.Camera, string> valueOrNull = data.EncodedDataCallback.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull(Option<Texture2D>.None, camera, error);
        return false;
      }
      finally
      {
        RenderTexture.active = (RenderTexture) null;
        if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null)
          RenderTexture.ReleaseTemporary(renderTexture);
        QualitySettings.antiAliasing = antiAliasing;
        QualitySettings.lodBias = lodBias;
        camera.targetTexture = (RenderTexture) null;
        camera.cullingMask = cullingMask;
        camera.transform.localPosition = localPosition;
        camera.transform.localRotation = localRotation;
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = vector2.x;
        camera.farClipPlane = vector2.y;
        this.m_fogController.UpdateFogNow();
        if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableWeather))
        {
          this.m_weatherController.SetWeatherEffectsVisibility(weatherIsVisible);
          this.m_lightController.SetState(state1);
          this.m_fogController.SetFogDensity(fogDensity);
          this.m_fogController.SetFogColor(fogColor);
        }
        if (data.CustomFogDensity.HasValue)
          this.m_fogController.SetFogDensity(fogDensity);
        this.m_fogController.SetFogRenderingState(isFogEnabled);
        if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableTerrainOverlays))
          this.m_towerAreasRenderer.ForceSetActive(isActive1);
        if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableTerrainGrid))
          this.m_terrainRenderer.ForceSetGridActive(isActive2);
        if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableHighlights))
          this.m_objectHighlighter.ForceSetActive(isActive3);
        if (data.ConfigFlags.IsFlagged(ScreenshotConfigFlags.DisableResourceBars))
          this.m_resVisBarsRenderer.ForceSetActive(isActive4);
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.antialiasingMode = antialiasing;
        RenderTexture.active = active;
      }
      error = "";
      return true;
    }

    private static Texture2D ensureTextureOfSize(
      ref Option<Texture2D> texture,
      int width,
      int height,
      TextureFormat textureFormat = TextureFormat.RGB24)
    {
      if (texture.HasValue)
      {
        if (texture.Value.width == width && texture.Value.height == height && texture.Value.format == textureFormat)
          return texture.Value;
        UnityEngine.Object.Destroy((UnityEngine.Object) texture.Value);
      }
      texture = (Option<Texture2D>) new Texture2D(width, height, textureFormat, false, false);
      return texture.Value;
    }

    private static void saveTexture(object d)
    {
      ScreenshotTaker.ScreenSaveData screenSaveData = (ScreenshotTaker.ScreenSaveData) d;
      try
      {
        byte[] bytes = screenSaveData.UseJpg ? ImageConversion.EncodeArrayToJPG((Array) screenSaveData.Data, screenSaveData.TextureGraphicsFormat, (uint) screenSaveData.TextureWidth, (uint) screenSaveData.TextureHeight, quality: 90) : ImageConversion.EncodeArrayToPNG((Array) screenSaveData.Data, screenSaveData.TextureGraphicsFormat, (uint) screenSaveData.TextureWidth, (uint) screenSaveData.TextureHeight);
        string str = screenSaveData.UseJpg ? ".jpg" : ".png";
        string path = screenSaveData.Path + str;
        for (int index = 0; index < 100 && File.Exists(path); ++index)
          path = string.Format("{0}_{1}{2}", (object) screenSaveData.Path, (object) (index + 1), (object) str);
        File.WriteAllBytes(path, bytes);
        Mafi.Log.Info(string.Format("Screenshot saved as {0}, size: {1} kB, ", screenSaveData.UseJpg ? (object) "JPG" : (object) "PNG", (object) (bytes.Length / 1024)) + "took " + screenSaveData.CaptureDurationMs.RoundToSigDigits(3, false, false, false) + " ms");
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, "Failed to encode and save image to '" + screenSaveData.Path + "'.");
      }
    }

    private class ScreenshotCaptureData
    {
      public readonly Option<string> FilePathWithoutExt;
      public readonly Option<Action<Option<Texture2D>, UnityEngine.Camera, string>> EncodedDataCallback;
      public readonly int Width;
      public readonly int Height;
      public readonly Vector3 CameraPosition;
      public readonly Quaternion CameraRotation;
      public readonly float VerticalFieldOfView;
      public readonly Vector2 NearFarClipPlane;
      public readonly bool DownscaleFromDoubleRes;
      public readonly bool SaveAsJpg;
      public readonly ScreenshotConfigFlags ConfigFlags;
      public readonly int JpgEncodingQuality;
      public readonly float? CustomFogDensity;

      public ScreenshotCaptureData(
        Option<string> filePathWithoutExt,
        Option<Action<Option<Texture2D>, UnityEngine.Camera, string>> encodedDataCallback,
        int width,
        int height,
        Vector3 cameraPosition,
        Quaternion cameraRotation,
        float verticalFieldOfView,
        Vector2 nearFarClipPlane,
        bool downscaleFromDoubleRes,
        bool saveAsJpg,
        ScreenshotConfigFlags configFlags,
        int jpgEncodingQuality,
        float? customFogDensity)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.FilePathWithoutExt = filePathWithoutExt;
        this.Width = width;
        this.Height = height;
        this.CameraPosition = cameraPosition;
        this.CameraRotation = cameraRotation;
        this.VerticalFieldOfView = verticalFieldOfView;
        this.NearFarClipPlane = nearFarClipPlane;
        this.DownscaleFromDoubleRes = downscaleFromDoubleRes;
        this.SaveAsJpg = saveAsJpg;
        this.ConfigFlags = configFlags;
        this.JpgEncodingQuality = jpgEncodingQuality;
        this.EncodedDataCallback = encodedDataCallback;
        this.CustomFogDensity = customFogDensity;
      }
    }

    private class ScreenSaveData
    {
      public readonly string Path;
      public readonly bool UseJpg;
      public readonly byte[] Data;
      public readonly GraphicsFormat TextureGraphicsFormat;
      public readonly int TextureWidth;
      public readonly int TextureHeight;
      public readonly float CaptureDurationMs;

      public ScreenSaveData(
        string path,
        bool useJpg,
        byte[] data,
        GraphicsFormat textureGraphicsFormat,
        int textureWidth,
        int textureHeight,
        float captureDurationMs)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Path = path;
        this.UseJpg = useJpg;
        this.Data = data;
        this.TextureGraphicsFormat = textureGraphicsFormat;
        this.TextureWidth = textureWidth;
        this.TextureHeight = textureHeight;
        this.CaptureDurationMs = captureDurationMs;
      }
    }
  }
}
