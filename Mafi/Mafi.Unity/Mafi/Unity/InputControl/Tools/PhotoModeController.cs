// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.PhotoModeController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using Mafi.Unity.Audio;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using Mafi.Unity.Weather;
using System;
using System.Globalization;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class PhotoModeController : IUnityInputController, IUnityUi
  {
    private readonly CameraController m_cameraController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly EntitiesIconRenderer m_entitiesIconRenderer;
    private readonly ScreenshotTaker m_screenshotTaker;
    private readonly FogController m_fogController;
    private readonly ICalendar m_calendar;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IActivator m_gridLinesActivator;
    private readonly Option<Mafi.Unity.MapEditor.MapEditor> m_mapEditor;
    private UiBuilder m_uiBuilder;
    private AudioSource m_shutterAudio;
    private AudioSource m_clickAudio;
    private Canvass m_canvas;
    private GameObject m_go;
    private Txt m_instructions;
    private string m_screenshotsDir;
    private string m_screenshotsDirAnonymous;
    private bool m_instructionsHidden;
    private bool m_useUnityCaptureSystem;
    private ScreenshotConfigFlags m_configFlags;
    private PhotoModeController.CaptureResolution m_captureResolution;
    private bool m_hideUi;
    private bool m_uiVisibleOriginal;
    private CameraMode m_cameraModeOriginal;
    private bool m_entityIconsVisibleOriginal;
    private bool m_useJpgInsteadOfPng;
    private bool m_fogEnabledOriginal;

    public ControllerConfig Config => ControllerConfig.PhotoMode;

    public PhotoModeController(
      CameraController cameraController,
      IUnityInputMgr inputManager,
      ShortcutsManager shortcutsManager,
      IFileSystemHelper fileSystemHelper,
      EntitiesIconRenderer entitiesIconRenderer,
      ScreenshotTaker screenshotTaker,
      FogController fogController,
      ICalendar calendar,
      SettlementsManager settlementsManager,
      ITerrainRenderer terrainRenderer,
      Option<Mafi.Unity.MapEditor.MapEditor> mapEditor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_configFlags = ScreenshotConfigFlags.DisableIcons | ScreenshotConfigFlags.Force8xMsaa | ScreenshotConfigFlags.ForceLod0;
      this.m_hideUi = true;
      this.m_useJpgInsteadOfPng = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cameraController = cameraController;
      this.m_shortcutsManager = shortcutsManager;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_entitiesIconRenderer = entitiesIconRenderer;
      this.m_screenshotTaker = screenshotTaker;
      this.m_fogController = fogController;
      this.m_calendar = calendar;
      this.m_settlementsManager = settlementsManager;
      this.m_gridLinesActivator = terrainRenderer.CreateGridLinesActivator();
      this.m_mapEditor = mapEditor;
      inputManager.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (m => m.TogglePhotoMode), (IUnityInputController) this);
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_uiBuilder = builder;
      this.m_shutterAudio = builder.AudioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/CameraShutter.prefab", AudioChannel.UserInterface);
      this.m_clickAudio = builder.AudioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/ButtonClick.prefab", AudioChannel.UserInterface);
      this.m_canvas = builder.NewCanvas("[`o´] Photo mode").SetRenderMode(RenderMode.ScreenSpaceOverlay).SetConstantPixelSize().SetSortOrder(5);
      this.m_go = this.m_canvas.GameObject;
      this.m_instructions = builder.NewTxt("Instructions").SetTextStyle(builder.Style.Global.Text).PutTo<Txt>((IUiElement) this.m_canvas, Offset.All(20f));
      this.m_go.SetActive(false);
    }

    public void Activate()
    {
      Mafi.Unity.MapEditor.MapEditor valueOrNull = this.m_mapEditor.ValueOrNull;
      this.m_uiVisibleOriginal = valueOrNull != null ? valueOrNull.EditorScreen.IsVisible() : this.m_uiBuilder.IsUiVisible;
      this.m_cameraModeOriginal = this.m_cameraController.CameraModel.CameraMode;
      this.m_entityIconsVisibleOriginal = this.m_entitiesIconRenderer.AllIconsVisible;
      this.m_fogEnabledOriginal = this.m_fogController.IsFogEnabled;
      this.m_screenshotsDir = this.m_fileSystemHelper.GetDirPath(FileType.Screenshot, true);
      this.m_screenshotsDirAnonymous = this.m_fileSystemHelper.AnonymizePath(this.m_screenshotsDir);
      this.m_hideUi = true;
      this.m_cameraController.CameraModel.SetMode(CameraMode.Unconstrained);
      this.m_go.SetActive(true);
      this.updateInstructions();
      this.updateSettings();
    }

    public void Deactivate()
    {
      this.restoreSettings();
      this.m_go.SetActive(false);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TakePicture))
      {
        this.captureAndSaveScreen();
        this.m_shutterAudio.Play();
        return true;
      }
      bool skipSound;
      if (!this.updateInput(out skipSound))
        return false;
      this.updateInstructions();
      this.updateSettings();
      if (!skipSound)
        this.m_clickAudio.Play();
      return true;
    }

    private void updateSettings()
    {
      if (this.m_mapEditor.HasValue)
        this.m_mapEditor.Value.EditorScreen.SetVisible(!this.m_hideUi);
      else
        this.m_uiBuilder.SetUiVisibility(!this.m_hideUi);
      this.m_entitiesIconRenderer.SetAllIconsVisibility(!this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableIcons));
      this.m_fogController.SetFogRenderingState(!this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableFog));
    }

    private void restoreSettings()
    {
      this.m_cameraController.CameraModel.SetMode(this.m_cameraModeOriginal);
      this.m_entitiesIconRenderer.SetAllIconsVisibility(this.m_entityIconsVisibleOriginal);
      this.m_fogController.SetFogRenderingState(this.m_fogEnabledOriginal);
      if (this.m_mapEditor.HasValue)
        this.m_mapEditor.Value.EditorScreen.SetVisible(this.m_uiVisibleOriginal);
      else
        this.m_uiBuilder.SetUiVisibility(this.m_uiVisibleOriginal);
      this.m_gridLinesActivator.DeactivateIfActive();
    }

    private bool updateInput(out bool skipSound)
    {
      skipSound = false;
      if (UnityEngine.Input.GetKeyDown(KeyCode.H))
      {
        this.m_instructionsHidden = !this.m_instructionsHidden;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.Print))
      {
        this.m_instructionsHidden = true;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.U))
      {
        this.m_hideUi = !this.m_hideUi;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.I))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.DisableIcons;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.N))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.DisableWeather;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.F))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.DisableFog;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.G))
      {
        this.m_gridLinesActivator.SetActive(!this.m_gridLinesActivator.IsActive);
        if (this.m_gridLinesActivator.IsActive)
          this.m_configFlags &= ~ScreenshotConfigFlags.DisableTerrainGrid;
        else if ((this.m_configFlags & (ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars)) != (ScreenshotConfigFlags) 0)
          this.m_configFlags |= ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.M))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.Force8xMsaa;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.L))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.ForceLod0;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.V))
      {
        this.m_configFlags ^= ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.J))
      {
        this.m_useJpgInsteadOfPng = !this.m_useJpgInsteadOfPng;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
      {
        this.m_useUnityCaptureSystem = !this.m_useUnityCaptureSystem;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.C))
      {
        ++this.m_captureResolution;
        if (this.m_captureResolution > PhotoModeController.CaptureResolution.R8k)
          this.m_captureResolution = PhotoModeController.CaptureResolution.Native;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
      {
        Application.OpenURL("file://" + this.m_screenshotsDir);
        return true;
      }
      OrbitalCameraModel cameraModel = this.m_cameraController.CameraModel;
      if (UnityEngine.Input.GetKeyDown(KeyCode.T))
      {
        if (cameraModel.TrackedTarget.HasValue)
        {
          cameraModel.CancelTargetTracking();
        }
        else
        {
          GameObject gameObject;
          if (CursorPickingManager.TryPickGoUnderCursor(this.m_cameraController.Camera, out gameObject, out RaycastHit _))
            cameraModel.SetTargetTracking(gameObject.transform, UnityEngine.Input.GetKey(KeyCode.LeftShift));
        }
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.R))
      {
        cameraModel.ResetPivotHeight();
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.PhotoModeRotation))
      {
        cameraModel.OrbitCamera = !cameraModel.OrbitCamera;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.LeftBracket))
      {
        cameraModel.OrbitSpeedDegPerSec *= 2;
        return true;
      }
      if (UnityEngine.Input.GetKey(KeyCode.LeftBracket))
      {
        skipSound = true;
        return true;
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.RightBracket))
      {
        cameraModel.OrbitSpeedDegPerSec /= 2;
        return true;
      }
      if (UnityEngine.Input.GetKey(KeyCode.RightBracket))
      {
        skipSound = true;
        return true;
      }
      if (!UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
        return false;
      cameraModel.OrbitSpeedDegPerSec = -cameraModel.OrbitSpeedDegPerSec;
      return true;
    }

    private void updateInstructions()
    {
      if (this.m_instructionsHidden)
      {
        this.m_instructions.SetText("");
      }
      else
      {
        Vector2i resolution = this.getResolution(this.m_captureResolution);
        string str1;
        switch (this.m_captureResolution)
        {
          case PhotoModeController.CaptureResolution.R1080:
            str1 = "1080p";
            break;
          case PhotoModeController.CaptureResolution.R1440:
            str1 = "1440p";
            break;
          case PhotoModeController.CaptureResolution.R4k:
            str1 = "4K";
            break;
          case PhotoModeController.CaptureResolution.R8k:
            str1 = "8K";
            break;
          default:
            str1 = "native";
            break;
        }
        string str2 = str1;
        OrbitalCameraModel cameraModel = this.m_cameraController.CameraModel;
        this.m_instructions.SetText(string.Format("[H] Hide this text\r\n\r\n[Q] Screenshots location: {0}\r\n[{1}] Take a picture\r\n[C] Resolution: {2} ({3} × {4})\r\n\r\n[T] Track entity under cursor (hold Shift to also track angle)\r\n[R] Reset pivot height (change by Ctrl + Alt + LMB)\r\n[{5}] Orbit camera\r\n'[' or ']' Change orbit speed: {6} deg/sec\r\n[\\] Toggle orbit direction\r\n\r\n[U] Toggle UI: {7}\r\n[I] Toggle icons: {8}\r\n[F] Toggle fog: {9}\r\n[G] Toggle grid: {10}\r\n[V] Capture overlays, grid, and highlights: {11}\r\n[M] Force 8x MSAA capture: {12}\r\n[L] Force max LOD: {13}\r\n[N] Capture as sunny: {14}\r\n[Y] Old capture system: {15}\r\n[J] Save as: {16}", (object) this.m_screenshotsDirAnonymous, (object) this.m_shortcutsManager.TakePicture.ToNiceString(), (object) str2, (object) resolution.X, (object) resolution.Y, (object) this.m_shortcutsManager.PhotoModeRotation.ToNiceString(), (object) cameraModel.OrbitSpeedDegPerSec.Degrees, this.m_hideUi ? (object) "hidden" : (object) "visible", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableIcons) ? (object) "hidden" : (object) "visible", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableFog) ? (object) "hidden" : (object) "visible", this.m_gridLinesActivator.IsActive ? (object) "force-show" : (object) "default", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars) ? (object) "no" : (object) "yes", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.Force8xMsaa) ? (object) "yes" : (object) "no", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.ForceLod0) ? (object) "yes" : (object) "no", this.m_configFlags.IsFlagged(ScreenshotConfigFlags.DisableWeather) ? (object) "yes" : (object) "no", this.m_useUnityCaptureSystem ? (object) "yes" : (object) "no", this.m_useJpgInsteadOfPng ? (object) "jpg" : (object) "png"));
      }
    }

    private Vector2i getResolution(PhotoModeController.CaptureResolution res)
    {
      Vector2i resolution;
      switch (res)
      {
        case PhotoModeController.CaptureResolution.R1080:
          resolution = new Vector2i(1920, 1080);
          break;
        case PhotoModeController.CaptureResolution.R1440:
          resolution = new Vector2i(2560, 1440);
          break;
        case PhotoModeController.CaptureResolution.R4k:
          resolution = new Vector2i(3840, 2160);
          break;
        case PhotoModeController.CaptureResolution.R8k:
          resolution = new Vector2i(7680, 4320);
          break;
        default:
          resolution = new Vector2i(this.m_cameraController.Camera.pixelWidth, this.m_cameraController.Camera.pixelHeight);
          break;
      }
      return resolution;
    }

    private void captureAndSaveScreen()
    {
      this.m_go.SetActive(false);
      string filePath = this.m_fileSystemHelper.GetFilePath(string.Format("{0}_y{1}_pop{2}", (object) DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", (IFormatProvider) CultureInfo.InvariantCulture), (object) this.m_calendar.CurrentDate.Year, (object) this.m_settlementsManager.GetTotalPopulation()), FileType.Screenshot, true);
      if (this.m_useUnityCaptureSystem)
      {
        ScreenCapture.CaptureScreenshot(filePath + ".png");
      }
      else
      {
        Vector2i resolution = this.getResolution(this.m_captureResolution);
        ScreenshotTaker screenshotTaker = this.m_screenshotTaker;
        Option<string> filePathWithoutExt = (Option<string>) filePath;
        int x = resolution.X;
        int y = resolution.Y;
        bool useJpgInsteadOfPng = this.m_useJpgInsteadOfPng;
        ScreenshotConfigFlags configFlags1 = this.m_configFlags;
        Vector3? cameraPosition = new Vector3?();
        Quaternion? cameraRotation = new Quaternion?();
        float? verticalFieldOfView = new float?();
        Vector2? nearFarClipPlane = new Vector2?();
        int num = useJpgInsteadOfPng ? 1 : 0;
        int configFlags2 = (int) configFlags1;
        float? customFogDensity = new float?();
        screenshotTaker.ScheduleScreenshotCapture(filePathWithoutExt, customWidth: x, customHeight: y, cameraPosition: cameraPosition, cameraRotation: cameraRotation, verticalFieldOfView: verticalFieldOfView, nearFarClipPlane: nearFarClipPlane, saveAsJpg: num != 0, configFlags: (ScreenshotConfigFlags) configFlags2, customFogDensity: customFogDensity);
      }
      this.m_go.SetActive(true);
    }

    private enum CaptureResolution
    {
      Native,
      R1080,
      R1440,
      R4k,
      R8k,
    }
  }
}
