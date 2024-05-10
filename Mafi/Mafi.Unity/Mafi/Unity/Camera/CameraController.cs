// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.CameraController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.GameLoop;
using Mafi.Core.Map;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.GameMenu;
using Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Globalization;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  /// <summary>Controls camera with user input.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CameraController : IDisposable
  {
    /// <summary>Main camera used for rendering of the scene.</summary>
    public readonly UnityEngine.Camera Camera;
    /// <summary>
    /// The single AudioListener in the scene on a sub-object of the camera. Having the AudioListener in a separate
    /// object instead of the Camera's object allows to specify where we want to listen to sounds.
    /// </summary>
    public readonly AudioListener Listener;
    public readonly OrbitalCameraModel CameraModel;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly LazyResolve<GameMenuController> m_gameMenuController;
    private readonly Cursoor m_freeLookCursor;
    private Option<UnityEngine.Camera> m_secondaryCamera;
    private readonly Transform m_cameraTransform;
    private readonly Vector3[] m_frustumCornersCache;
    private bool m_cameraIsFrozen;
    private double m_previousOrbitRadius;

    public bool IsInFreeLookMode { get; private set; }

    /// <summary>
    /// Whether the camera control is enabled (handy to disable when showing fullscreen window).
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Invoked when <see cref="P:Mafi.Unity.Camera.CameraController.IsInFreeLookMode" /> is changed. Invoked on the main thread.
    /// </summary>
    public event Action<bool> FreeLookModeChanged;

    public event Action<float> CameraFovChanged;

    public Vector3 EyePosition => this.m_cameraTransform.localPosition;

    public CameraController(
      IGameLoopEvents gameLoopEvents,
      OrbitalCameraModel cameraModel,
      CursorManager cursorManager,
      ShortcutsManager shortcutsManager,
      UiStyle style,
      IStartLocationProvider startLocationProvider,
      LazyResolve<GameMenuController> gameMenuController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIsEnabled\u003Ek__BackingField = true;
      this.m_frustumCornersCache = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      CameraController cameraController = this;
      this.CameraModel = cameraModel;
      this.m_shortcutsManager = shortcutsManager;
      this.m_gameMenuController = gameMenuController;
      this.m_freeLookCursor = cursorManager.RegisterCursor(style.Cursors.FreeLook);
      this.Camera = UnityEngine.Object.FindObjectOfType<UnityEngine.Camera>();
      this.m_cameraTransform = (bool) (UnityEngine.Object) this.Camera ? this.Camera.transform : throw new Exception("No camera found in the game!");
      this.Camera.depthTextureMode = DepthTextureMode.Depth;
      this.Listener = this.Camera.gameObject.GetComponentInChildren<AudioListener>();
      if ((bool) (UnityEngine.Object) this.Listener)
        this.Listener.transform.localPosition = new Vector3(0.0f, 0.0f, 35f);
      else
        Log.Error("No audio listener found on the camera GO!");
      gameLoopEvents.RegisterRendererInitState((object) this, (Action) (() =>
      {
        if (gameLoopEvents.GameWasLoaded)
          return;
        cameraController.ResetCamera(startLocationProvider.StartingLocation.Position.CornerTile2f);
      }));
      gameLoopEvents.RenderUpdateEnd.AddNonSaveable<CameraController>(this, new Action<GameTime>(this.renderUpdateEnd));
      this.SetVerticalFov(CameraSettingsHelper.GetCurrentFov());
      CameraSettingsHelper.OnFovChanged += new Action<float>(this.fovSettingsChanged);
    }

    public void Dispose()
    {
      CameraSettingsHelper.OnFovChanged -= new Action<float>(this.fovSettingsChanged);
    }

    [ConsoleCommand(true, false, null, null)]
    private void panCameraTo(int x, int y) => this.PanTo(new Tile2f((Fix32) x, (Fix32) y));

    [ConsoleCommand(true, false, null, null)]
    private string getCameraPosition() => this.CameraModel.State.PivotPosition.ToString();

    [ConsoleCommand(true, false, null, null)]
    private string setCameraFov(float fov)
    {
      this.SetVerticalFov(fov);
      return string.Format("Vertical FOV set to {0}", (object) this.Camera.fieldOfView);
    }

    [ConsoleCommand(true, false, null, null)]
    private string resetCameraFov()
    {
      this.SetVerticalFov(CameraSettingsHelper.GetCurrentFov());
      return string.Format("Vertical FOV reset to {0}", (object) this.Camera.fieldOfView);
    }

    private void fovSettingsChanged(float fov) => this.SetVerticalFov(fov);

    public void ResetCamera(Tile2f position) => this.CameraModel.ResetPose(position);

    public void PanTo(Tile2f position) => this.CameraModel.PanTo(position);

    public void SetMousePanDisabled(bool isDisabled)
    {
      this.CameraModel.SetMousePanDisabled(isDisabled);
    }

    public void SetVerticalFov(float fov)
    {
      this.Camera.fieldOfView = fov.Clamp(10f, 80f);
      Action<float> cameraFovChanged = this.CameraFovChanged;
      if (cameraFovChanged == null)
        return;
      cameraFovChanged(this.Camera.fieldOfView);
    }

    public bool InputUpdateEarly()
    {
      if (!this.IsEnabled)
        return false;
      bool flag = this.CameraModel.ProcessUserInputEarly(this.Camera);
      bool isInFreeLookMode = this.CameraModel.IsInFreeLookMode;
      int num = isInFreeLookMode ? 1 : 0;
      if (this.IsInFreeLookMode != isInFreeLookMode)
      {
        this.IsInFreeLookMode = isInFreeLookMode;
        if (this.IsInFreeLookMode)
          this.m_freeLookCursor.ShowTemporary();
        else
          this.m_freeLookCursor.HideTemporary();
        Action<bool> freeLookModeChanged = this.FreeLookModeChanged;
        if (freeLookModeChanged != null)
          freeLookModeChanged(this.IsInFreeLookMode);
      }
      return flag;
    }

    public bool InputUpdate()
    {
      if (!this.IsEnabled)
        return false;
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraSave1))
      {
        this.CameraModel.State.SavedPosition1 = this.CameraModel.State.PivotPosition;
        return true;
      }
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraLoad1))
      {
        if (this.CameraModel.State.SavedPosition1.IsNotZero)
          this.CameraModel.PanTo(this.CameraModel.State.SavedPosition1);
        return true;
      }
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraSave2))
      {
        this.CameraModel.State.SavedPosition2 = this.CameraModel.State.PivotPosition;
        return true;
      }
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraLoad2))
      {
        if (this.CameraModel.State.SavedPosition2.IsNotZero)
          this.CameraModel.PanTo(this.CameraModel.State.SavedPosition2);
        return true;
      }
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraSave3))
      {
        this.CameraModel.State.SavedPosition3 = this.CameraModel.State.PivotPosition;
        return true;
      }
      if (!this.m_shortcutsManager.IsUp(this.m_shortcutsManager.CameraLoad3))
        return this.CameraModel.ProcessUserInput(this.Camera);
      if (this.CameraModel.State.SavedPosition3.IsNotZero)
        this.CameraModel.PanTo(this.CameraModel.State.SavedPosition3);
      return true;
    }

    /// <summary>
    /// Update camera at the end of rendering updates, so that all other MBs are already updated.
    /// This is especially important for smooth tracking.
    /// </summary>
    private void renderUpdateEnd(GameTime time)
    {
      if ((double) this.CameraModel.OrbitRadius != this.m_previousOrbitRadius)
      {
        this.m_previousOrbitRadius = (double) this.CameraModel.OrbitRadius;
        GlobalGfxSettings.NotifyCameraDistanceChanged(this.CameraModel.OrbitRadius);
      }
      if (this.m_cameraIsFrozen)
      {
        if (!this.IsInFreeLookMode)
          return;
        this.m_cameraIsFrozen = false;
      }
      this.setCameraPose(this.CameraModel);
    }

    private void setCameraPose(OrbitalCameraModel cameraModel)
    {
      this.CameraModel.UpdateDamping(this.IsInFreeLookMode);
      this.Camera.CalculateFrustumCorners(new Rect(0.0f, 0.0f, 1f, 1f), this.Camera.nearClipPlane, this.Camera.stereoActiveEye, this.m_frustumCornersCache);
      if (cameraModel.TrackedTarget.HasValue)
      {
        if ((bool) (UnityEngine.Object) cameraModel.TrackedTarget.Value)
        {
          Vector3 eyePosition = cameraModel.TrackedTarget.Value.position - cameraModel.Rotation * new Vector3(0.0f, 0.0f, cameraModel.OrbitRadius);
          this.m_cameraTransform.localPosition = cameraModel.AdjustEyePositionToAvoidTerrainClipping(eyePosition, this.m_frustumCornersCache);
          cameraModel.PanTo(cameraModel.TrackedTarget.Value.position.ToTile2f());
        }
        else
        {
          cameraModel.CancelTargetTracking();
          this.m_cameraTransform.localPosition = cameraModel.ComputeEyePosition(this.m_frustumCornersCache);
        }
      }
      else
        this.m_cameraTransform.localPosition = cameraModel.ComputeEyePosition(this.m_frustumCornersCache);
      this.m_cameraTransform.localRotation = cameraModel.Rotation;
      float nearPlane;
      float farPlane;
      cameraModel.GetNearFarClipPlanes(out nearPlane, out farPlane);
      this.Camera.nearClipPlane = nearPlane;
      this.Camera.farClipPlane = farPlane;
    }

    [ConsoleCommand(true, false, null, null)]
    private string cameraPositionPrint()
    {
      string message = CameraController.serializeTransform(this.Camera.transform);
      Log.Info(message);
      return message;
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult cameraPositionLoadFromString(string source)
    {
      if (CameraController.tryDeserializeTransformTo(source, this.Camera.transform))
      {
        this.m_cameraIsFrozen = true;
        return GameCommandResult.Success((object) "Camera position loaded successfully.");
      }
      return this.CameraModel.State.TryDeserializeFromString(source) ? GameCommandResult.Success((object) "Warning: Loaded camera pose uses old system, re-save for more stable results.") : GameCommandResult.Error("Failed to parse '" + source + "' as camera transform.");
    }

    [ConsoleCommand(true, false, null, null)]
    private string cameraPositionQuickSave() => this.cameraPositionSaveToSlot(0);

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult cameraPositionQuickLoad() => this.cameraPositionLoadSlot(0);

    [ConsoleCommand(true, false, null, null)]
    private string cameraPositionSaveToSlot(int index)
    {
      string str = CameraController.serializeTransform(this.Camera.transform);
      PlayerPrefs.SetString(string.Format("CameraPos{0}", (object) index), str);
      PlayerPrefs.Save();
      return string.Format("Saved to slot {0}: '{1}'.", (object) index, (object) str);
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult cameraPositionLoadSlot(int index)
    {
      return this.cameraPositionLoadFromString(PlayerPrefs.GetString(string.Format("CameraPos{0}", (object) index), ""));
    }

    private static string serializeTransform(Transform t)
    {
      Vector3 position = t.position;
      Quaternion rotation = t.rotation;
      return string.Format("{0},{1},{2},{3},{4},{5},{6}", (object) position.x, (object) position.y, (object) position.z, (object) rotation.x, (object) rotation.y, (object) rotation.z, (object) rotation.w);
    }

    private static bool tryDeserializeTransformTo(string str, Transform t)
    {
      if (string.IsNullOrEmpty(str))
        return false;
      string[] strArray = str.Split(',', StringSplitOptions.None);
      if (strArray.Length != 7)
        return false;
      float[] numArray = new float[7];
      for (int index = 0; index < 7; ++index)
      {
        if (!float.TryParse(strArray[index], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out numArray[index]))
          return false;
      }
      t.position = new Vector3(numArray[0], numArray[1], numArray[2]);
      t.rotation = new Quaternion(numArray[3], numArray[4], numArray[5], numArray[6]);
      return true;
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult customCameraCreateNew(int width = 2560, int height = 1440)
    {
      if (this.m_secondaryCamera.HasValue)
        return GameCommandResult.Error("Secondary camera already exists.");
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Camera.gameObject);
      gameObject.tag = UnityTag.NoHi.ToString();
      this.m_secondaryCamera = (Option<UnityEngine.Camera>) gameObject.GetComponent<UnityEngine.Camera>();
      this.m_secondaryCamera.Value.targetTexture = new RenderTexture(width, height, 32);
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject.GetComponentInChildren<AudioListener>());
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject.GetComponentInChildren<ObjectHighlighter.ObjectHighlighterMb>());
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject.GetComponentInChildren<BlurOptimized>());
      return GameCommandResult.Success((object) string.Format("Secondary camera created, {0} x {1}, tag 'NoHi'.", (object) width, (object) height));
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult customCameraDelete()
    {
      if (this.m_secondaryCamera.IsNone)
        return GameCommandResult.Error("No secondary camera exists.");
      this.m_secondaryCamera.Value.gameObject.Destroy();
      this.m_secondaryCamera = Option<UnityEngine.Camera>.None;
      return GameCommandResult.Success((object) "Secondary camera was destroyed.");
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult customCameraSetToCurrentView()
    {
      if (this.m_secondaryCamera.IsNone)
        return GameCommandResult.Error("No secondary camera exists.");
      Transform transform = this.m_secondaryCamera.Value.transform;
      transform.position = this.Camera.transform.position;
      transform.rotation = this.Camera.transform.rotation;
      return GameCommandResult.Success((object) "Transform of secondary camera was set.");
    }
  }
}
