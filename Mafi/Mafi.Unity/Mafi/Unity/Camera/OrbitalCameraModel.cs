// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.OrbitalCameraModel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.UiState;
using Mafi.Unity.InputControl;
using Mafi.Unity.Terrain;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  /// <summary>
  /// Camera controller that implements orbital camera. Camera orbits around a pivot point at a given distance.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class OrbitalCameraModel
  {
    private static readonly RelTile1f MIN_HEIGHT_ABOVE_TERRAIN;
    private static readonly RelTile1f MIN_PIVOT_DISTANCE;
    private static readonly RelTile1f MAX_PIVOT_DISTANCE;
    private static readonly RelTile1f DEFAULT_CAMERA_PIVOT_DIST;
    private static readonly AngleDegrees1f DEFAULT_CAMERA_YAW;
    private static readonly AngleDegrees1f DEFAULT_CAMERA_PITCH;
    private static readonly AngleDegrees1f MIN_PITCH;
    private static readonly AngleDegrees1f MAX_PITCH;
    /// <summary>Pan rate.</summary>
    private float m_keyPanScale;
    private float m_keyZoomScalePerSec;
    private float m_keyRotationScalePerSec;
    /// <summary>6 degrees per pixel.</summary>
    private static readonly Fix32 MOUSE_LOOK_SCALE;
    private bool m_alsoTrackTargetRotation;
    private AngleDegrees1f m_trackedTargetAngle;
    private float m_trackingAngleVelocity;
    private Vector2 m_pivotPositionMeters;
    private float m_pivotPositionVelocityX;
    private float m_pivotPositionVelocityY;
    private float m_pivotHeightMeters;
    private float m_pivotHeightVelocity;
    private float m_orbitRadiusMeters;
    private float m_orbitRadiusVelocity;
    private float m_pitchAngleDeg;
    private float m_pitchAngleVelocity;
    private float m_yawAngleDeg;
    private float m_yawAngleVelocity;
    private ThicknessTilesF m_heightDelta;
    private bool m_isOnValidTile;
    private readonly ShortcutsManager m_shortuctsManager;
    private readonly TerrainManager m_terrainManager;
    private readonly LazyResolve<TerrainRenderer> m_terrainRendererLazy;
    private readonly UiCameraState m_state;
    private RelTile1f m_minPivotDistance;
    private RelTile1f m_maxPivotDistance;
    private AngleDegrees1f m_minPitch;
    private AngleDegrees1f m_maxPitch;
    private RelTile1f m_minHeightAboveTerrain;
    public bool IsInFreeLookMode;
    private Vector3 m_mouseFreeLookStartCursorPosition;
    private int m_mouseFreeLookStartTime;
    private Plane m_raycastFallbackPlane;
    private Plane? m_mousePanPlane;
    private Vector3 m_mousePanStartPoint;
    private int m_mousePanStartTime;
    private bool m_mousePanDisabled;
    private Vector3 m_mousePanStartCursorPosition;
    private HeightTilesF m_heightOnSim;
    private Tile2i m_positionForHeightQuery;
    private Vector2 m_currentPan;
    private bool m_disableHeightAdjustments;
    private bool m_disableHeightAdjustmentsConsole;

    private Tile2f PositionTarget
    {
      get => this.m_state.PivotPosition;
      set
      {
        this.m_state.PivotPosition = value;
        this.recomputePivotHeight();
      }
    }

    private RelTile1f OrbitRadiusTarget
    {
      get => this.m_state.OrbitRadius;
      set
      {
        this.m_state.OrbitRadius = value.Clamp(this.m_minPivotDistance, this.m_maxPivotDistance);
      }
    }

    /// <summary>
    /// Pitch is 0 when camera looks parallel with the ground. 90 looks directly down.
    /// </summary>
    private AngleDegrees1f PitchAngleTarget
    {
      get => this.m_state.PitchAngle;
      set => this.m_state.PitchAngle = value.Clamp(this.m_minPitch, this.m_maxPitch);
    }

    /// <summary>
    /// Yaw angle is 0 when camera looks in +Y direction (+Z in unity) and increases as it turns counter-clock wise.
    /// </summary>
    private AngleDegrees1f YawAngleTarget
    {
      get => this.m_state.YawAngle;
      set => this.m_state.YawAngle = value.Normalized;
    }

    public bool OrbitCamera { get; set; }

    public AngleDegrees1f OrbitSpeedDegPerSec { get; set; }

    public Vector3 TargetPosition
    {
      get
      {
        return new Vector3(this.m_pivotPositionMeters.x, this.m_pivotHeightMeters, this.m_pivotPositionMeters.y);
      }
    }

    public float OrbitRadius => this.m_orbitRadiusMeters;

    public Quaternion Rotation
    {
      get
      {
        return Quaternion.AngleAxis(this.m_yawAngleDeg, Vector3.up) * Quaternion.AngleAxis(-this.m_pitchAngleDeg, Vector3.right);
      }
    }

    public CameraMode CameraMode { get; private set; }

    public Option<Transform> TrackedTarget { get; set; }

    public UiCameraState State => this.m_state;

    public OrbitalCameraModel(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      ShortcutsManager shortcutsManager,
      TerrainManager terrainManager,
      LazyResolve<TerrainRenderer> terrainRendererLazy,
      UiCameraState cameraState)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_keyPanScale = 12f;
      this.m_keyZoomScalePerSec = 1.2f;
      this.m_keyRotationScalePerSec = 100f;
      // ISSUE: reference to a compiler-generated field
      this.\u003COrbitSpeedDegPerSec\u003Ek__BackingField = 8.Degrees();
      this.m_raycastFallbackPlane = new Plane(Vector3.up, 0.0f);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortuctsManager = shortcutsManager;
      this.m_terrainManager = terrainManager;
      this.m_terrainRendererLazy = terrainRendererLazy;
      this.m_state = cameraState;
      this.SetMode(CameraMode.DefaultGameplay);
      this.releaseDamping();
      gameLoopEvents.SyncUpdate.AddNonSaveable<OrbitalCameraModel>(this, new Action<GameTime>(this.sync));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<OrbitalCameraModel>(this, new Action(this.updateSimEnd));
    }

    [ConsoleCommand(false, false, null, null)]
    private void setKeyPanSensitivity(float value = 1f) => this.m_keyPanScale = value * 12f;

    [ConsoleCommand(false, false, null, null)]
    private void setKeyRotationSensitivity(float value = 1f)
    {
      this.m_keyRotationScalePerSec = value * 100f;
    }

    [ConsoleCommand(false, false, null, null)]
    private void setKeyZoomSensitivity(float value = 1f)
    {
      this.m_keyZoomScalePerSec = value * 1.2f;
    }

    public void SetAutoHeightAdjustEnabled(bool isEnabled)
    {
      this.m_disableHeightAdjustments = !isEnabled;
    }

    private void sync(GameTime obj)
    {
      this.m_state.PivotHeight = this.m_heightOnSim;
      this.m_positionForHeightQuery = this.m_pivotPositionMeters.ToTile2f().Tile2iRounded;
    }

    private void updateSimEnd() => this.recomputePivotHeight();

    public void ResetPose(
      Tile2f position,
      RelTile1f? pivotDistance = null,
      AngleDegrees1f? cameraPitch = null,
      AngleDegrees1f? cameraYaw = null)
    {
      this.PositionTarget = position;
      this.OrbitRadiusTarget = (pivotDistance ?? OrbitalCameraModel.DEFAULT_CAMERA_PIVOT_DIST).Clamp(this.m_minPivotDistance, this.m_maxPivotDistance);
      AngleDegrees1f? nullable = cameraPitch;
      this.PitchAngleTarget = (nullable ?? OrbitalCameraModel.DEFAULT_CAMERA_PITCH).Clamp(this.m_minPitch, this.m_maxPitch);
      nullable = cameraYaw;
      this.YawAngleTarget = nullable ?? OrbitalCameraModel.DEFAULT_CAMERA_YAW;
      this.recomputePivotHeight();
      this.releaseDamping();
    }

    public void PanTo(Tile2f position)
    {
      if (!this.isValidPosition(position))
        return;
      this.m_state.PivotPosition = position;
      this.m_isOnValidTile = true;
    }

    private void releaseDamping()
    {
      this.m_pivotPositionMeters = this.m_state.PivotPosition.ToVector2();
      this.m_pivotHeightMeters = this.m_state.PivotHeight.ToUnityUnits();
      this.m_orbitRadiusMeters = this.m_state.OrbitRadius.ToUnityUnits();
      this.m_yawAngleDeg = this.m_state.YawAngle.ToUnityAngleDegrees();
      this.m_pitchAngleDeg = this.m_state.PitchAngle.ToUnityAngleDegrees();
    }

    private void applyDamping()
    {
      this.m_orbitRadiusMeters = Mathf.SmoothDamp(this.m_orbitRadiusMeters, this.m_state.OrbitRadius.ToUnityUnits(), ref this.m_orbitRadiusVelocity, 0.05f);
      Vector2 vector2 = this.m_state.PivotPosition.ToVector2();
      if (this.m_mousePanPlane.HasValue)
      {
        this.m_pivotPositionMeters = vector2;
      }
      else
      {
        this.m_pivotPositionMeters = new Vector2(Mathf.SmoothDamp(this.m_pivotPositionMeters.x, vector2.x, ref this.m_pivotPositionVelocityX, 0.04f), Mathf.SmoothDamp(this.m_pivotPositionMeters.y, vector2.y, ref this.m_pivotPositionVelocityY, 0.04f));
        this.m_pivotHeightMeters = Mathf.SmoothDamp(this.m_pivotHeightMeters, (this.m_state.PivotHeight + this.m_heightDelta).ToUnityUnits(), ref this.m_pivotHeightVelocity, 0.1f);
      }
      this.m_yawAngleDeg = Mathf.SmoothDampAngle(this.m_yawAngleDeg, this.m_state.YawAngle.ToUnityAngleDegrees(), ref this.m_yawAngleVelocity, 0.05f);
      this.m_pitchAngleDeg = Mathf.SmoothDampAngle(this.m_pitchAngleDeg, this.m_state.PitchAngle.ToUnityAngleDegrees(), ref this.m_pitchAngleVelocity, 0.05f);
    }

    /// <summary>
    /// Computes the eye position taking account terrain. This can be relatively expensive. If you just want
    /// the camera position you should use camera.transform.localPosition.
    /// </summary>
    public Vector3 ComputeEyePosition(Vector3[] frustumCorners)
    {
      return this.AdjustEyePositionToAvoidTerrainClipping(this.TargetPosition - this.Rotation * new Vector3(0.0f, 0.0f, this.m_orbitRadiusMeters), frustumCorners);
    }

    public Vector3 AdjustEyePositionToAvoidTerrainClipping(
      Vector3 eyePosition,
      Vector3[] frustumCorners)
    {
      float nearPlane;
      this.GetNearFarClipPlanes(out nearPlane, out float _);
      float num1 = Mathf.Max(this.m_minHeightAboveTerrain.ToUnityUnits(), nearPlane * 1.25f) + 0.25f * Mathf.Sin((float) Math.PI / 180f * this.m_pitchAngleDeg).Abs();
      float self1 = heightAtEyePos(eyePosition);
      eyePosition.y = eyePosition.y.Max(self1 + num1);
      float self2 = 0.0f;
      float self3 = 0.0f;
      float self4 = 0.0f;
      float self5 = 0.0f;
      foreach (Vector3 frustumCorner in frustumCorners)
      {
        Vector3 vector3 = this.Rotation * frustumCorner;
        self2 = self2.Min(vector3.x);
        self3 = self3.Max(vector3.x);
        self4 = self4.Min(vector3.z);
        self5 = self5.Max(vector3.z);
      }
      int num2 = 16;
      float num3 = (self3 - self2) / (float) (num2 - 1);
      float num4 = (self5 - self4) / (float) (num2 - 1);
      float x = self2 + eyePosition.x;
      float num5 = 0.0f;
      while ((double) num5 < (double) num2)
      {
        float z = self4 + eyePosition.z;
        float num6 = 0.0f;
        while ((double) num6 < (double) num2)
        {
          self1 = self1.Max(heightAtEyePos(new Vector3(x, 0.0f, z)));
          ++num6;
          z += num4;
        }
        ++num5;
        x += num3;
      }
      eyePosition.y = eyePosition.y.Max(self1 + num1);
      return eyePosition;

      float heightAtEyePos(Vector3 testEyePosition)
      {
        Tile2f globalCoord = new Tile2f(((double) testEyePosition.x).Meters(), ((double) testEyePosition.z).Meters());
        Tile2i tile2iRounded = globalCoord.Tile2iRounded;
        float num;
        if (this.m_terrainManager.IsValidCoord(tile2iRounded))
        {
          Tile2iIndex tileIndex = this.m_terrainManager.GetTileIndex(tile2iRounded);
          num = this.m_terrainManager.IsOcean(tileIndex) || this.m_terrainManager.IsOnMapBoundary(tileIndex) ? 0.0f : this.m_terrainManager.GetHeight(globalCoord).ToUnityUnits();
        }
        else
          num = 0.0f;
        return num;
      }
    }

    /// <summary>
    /// Computes pivot height at given terrain coordinate based on multiple samples. If some of the terrain samples
    /// are missing current height is returned.
    /// </summary>
    private void recomputePivotHeight()
    {
      int num = 5.Squared();
      Fix32 zero = Fix32.Zero;
      for (int index1 = -2; index1 <= 2; ++index1)
      {
        for (int index2 = -2; index2 <= 2; ++index2)
        {
          Tile2i tile2i = this.m_positionForHeightQuery + new RelTile2i(4 * index2, 4 * index1);
          if (this.isValidPosition(tile2i))
          {
            Tile2iIndex tileIndex = this.m_terrainManager.GetTileIndex(tile2i);
            if (!this.m_terrainManager.IsOcean(tileIndex))
              zero += this.m_terrainManager.GetHeight(tileIndex).Value;
          }
          else
          {
            --num;
            if (num <= 5)
              return;
          }
        }
      }
      this.m_heightOnSim = new HeightTilesF(zero / num);
    }

    public bool ProcessUserInputEarly(UnityEngine.Camera camera)
    {
      bool flag = false;
      if (this.IsInFreeLookMode)
      {
        if (!this.m_shortuctsManager.IsOn(this.m_shortuctsManager.FreeLookMode))
        {
          this.IsInFreeLookMode = false;
          int num1 = Environment.TickCount - this.m_mouseFreeLookStartTime;
          float num2 = (this.m_mouseFreeLookStartCursorPosition.x - Input.mousePosition.x).Abs() + (this.m_mouseFreeLookStartCursorPosition.y - Input.mousePosition.y).Abs();
          return num1 > 300 || (double) num2 > 20.0;
        }
      }
      else if (this.m_shortuctsManager.IsDown(this.m_shortuctsManager.FreeLookMode) && !EventSystem.current.IsPointerOverGameObject())
      {
        this.IsInFreeLookMode = true;
        this.m_mouseFreeLookStartCursorPosition = Input.mousePosition;
        this.m_mouseFreeLookStartTime = Environment.TickCount;
      }
      if (this.TrackedTarget.IsNone)
        flag |= this.processMousePan(camera);
      if (this.IsInFreeLookMode)
      {
        if (this.CameraMode == CameraMode.Unconstrained && Input.GetKey(KeyCode.LeftControl))
        {
          this.m_heightDelta += new ThicknessTilesF((Input.GetAxis("MouseY") * -3f).ToFix32());
          this.recomputePivotHeight();
        }
        else
          this.processMouseRotation();
      }
      return flag;
    }

    public bool ProcessUserInput(UnityEngine.Camera camera)
    {
      bool flag = false;
      if (this.TrackedTarget.HasValue)
      {
        if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveLeft) || this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveRight) || this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveUp) || this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveDown) || this.m_shortuctsManager.IsOn(this.m_shortuctsManager.PanCamera))
          this.CancelTargetTracking();
      }
      else if (!this.m_mousePanPlane.HasValue)
        flag |= this.processKeyboardPan(Time.unscaledDeltaTime);
      if (this.IsInFreeLookMode || !EventSystem.current.IsPointerOverGameObject())
        flag |= this.processZoom();
      return flag | this.processKeyboardRotation();
    }

    public void UpdateDamping(bool isInFreeLookMode)
    {
      if (!isInFreeLookMode)
      {
        if (this.OrbitCamera)
          this.YawAngleTarget += this.OrbitSpeedDegPerSec.ScaledBy(Percent.FromFloat(Time.unscaledDeltaTime));
        else if (this.TrackedTarget.HasValue && this.m_alsoTrackTargetRotation)
        {
          AngleDegrees1f angleDegrees1f = this.m_trackedTargetAngle + AngleDegrees1f.FromDegrees(-this.TrackedTarget.Value.eulerAngles.y.ToFix32());
          this.YawAngleTarget = Mathf.SmoothDampAngle(this.YawAngleTarget.Degrees.ToFloat(), angleDegrees1f.Degrees.ToFloat(), ref this.m_trackingAngleVelocity, 0.5f).Degrees();
        }
      }
      this.applyDamping();
    }

    public void ResetPivotHeight() => this.m_heightDelta = ThicknessTilesF.Zero;

    public void SetMode(CameraMode mode)
    {
      this.CameraMode = mode;
      switch (mode)
      {
        case CameraMode.DefaultGameplay:
          this.m_minPivotDistance = OrbitalCameraModel.MIN_PIVOT_DISTANCE;
          this.m_maxPivotDistance = OrbitalCameraModel.MAX_PIVOT_DISTANCE;
          this.m_minPitch = OrbitalCameraModel.MIN_PITCH;
          this.m_maxPitch = OrbitalCameraModel.MAX_PITCH;
          this.m_minHeightAboveTerrain = OrbitalCameraModel.MIN_HEIGHT_ABOVE_TERRAIN;
          this.m_heightDelta = ThicknessTilesF.Zero;
          this.OrbitCamera = false;
          break;
        case CameraMode.Unconstrained:
          this.m_minPivotDistance = OrbitalCameraModel.MIN_PIVOT_DISTANCE / 2;
          this.m_maxPivotDistance = OrbitalCameraModel.MAX_PIVOT_DISTANCE * 4;
          this.m_minPitch = -45.Degrees();
          this.m_maxPitch = 90.Degrees();
          this.m_minHeightAboveTerrain = new RelTile1f(0);
          break;
        default:
          Assert.Fail("Unknown camera mode");
          goto case CameraMode.DefaultGameplay;
      }
      this.OrbitRadiusTarget = this.OrbitRadiusTarget;
      this.PitchAngleTarget = this.PitchAngleTarget;
      this.recomputePivotHeight();
    }

    public void GetNearFarClipPlanes(out float nearPlane, out float farPlane)
    {
      float self = (this.m_orbitRadiusMeters - OrbitalCameraModel.MIN_PIVOT_DISTANCE.ToUnityUnits()) / (OrbitalCameraModel.MAX_PIVOT_DISTANCE - OrbitalCameraModel.MIN_PIVOT_DISTANCE).ToUnityUnits();
      nearPlane = 2f.Lerp(20f, self.Max(0.0f));
      farPlane = 4000f.Lerp(10000f, self.Max(0.0f));
    }

    private bool processKeyboardPan(float dt)
    {
      float x = 0.0f;
      float y = 0.0f;
      if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveLeft))
      {
        --x;
        if ((double) this.m_currentPan.x > 0.0)
          this.m_currentPan = new Vector2(0.0f, this.m_currentPan.y);
      }
      else if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveRight))
      {
        ++x;
        if ((double) this.m_currentPan.x < 0.0)
          this.m_currentPan = new Vector2(0.0f, this.m_currentPan.y);
      }
      if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveUp))
      {
        ++y;
        if ((double) this.m_currentPan.y < 0.0)
          this.m_currentPan = new Vector2(this.m_currentPan.x, 0.0f);
      }
      else if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.MoveDown))
      {
        --y;
        if ((double) this.m_currentPan.y > 0.0)
          this.m_currentPan = new Vector2(this.m_currentPan.x, 0.0f);
      }
      this.m_currentPan = this.m_currentPan.SmoothDampToUnscaledTime(new Vector2(x, y).normalized, 0.15f);
      if (this.m_currentPan.sqrMagnitude.IsNearZero())
        return false;
      float num1 = (float) (1.0 + (double) this.m_orbitRadiusMeters / 10.0);
      float num2 = dt * num1 * this.m_keyPanScale;
      if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.PanSpeedBoost))
        num2 *= 2f;
      this.panByInCameraSpace(this.m_currentPan * num2);
      return true;
    }

    private bool processMousePan(UnityEngine.Camera camera)
    {
      if (this.m_mousePanDisabled)
      {
        this.m_mousePanPlane = new Plane?();
        return false;
      }
      if (!this.m_mousePanPlane.HasValue)
      {
        if (this.m_shortuctsManager.IsDown(this.m_shortuctsManager.PanCamera) && !EventSystem.current.IsPointerOverGameObject())
        {
          this.m_mousePanStartCursorPosition = Input.mousePosition;
          this.m_mousePanStartTime = Environment.TickCount;
          Ray ray = camera.ScreenPointToRay(this.m_mousePanStartCursorPosition);
          Tile3f? nullable = this.m_terrainRendererLazy.Value.Raycast(ray);
          if (nullable.HasValue)
          {
            this.m_mousePanStartPoint = nullable.Value.ToVector3();
          }
          else
          {
            float enter;
            if (!this.m_raycastFallbackPlane.Raycast(ray, out enter))
              return false;
            this.m_mousePanStartPoint = ray.GetPoint(enter);
          }
          this.m_mousePanPlane = new Plane?(new Plane(Vector3.up, -this.m_mousePanStartPoint.y));
        }
      }
      else
      {
        if (!this.m_shortuctsManager.IsOn(this.m_shortuctsManager.PanCamera))
        {
          this.m_mousePanPlane = new Plane?();
          int num1 = Environment.TickCount - this.m_mousePanStartTime;
          float num2 = (this.m_mousePanStartCursorPosition.x - Input.mousePosition.x).Abs() + (this.m_mousePanStartCursorPosition.y - Input.mousePosition.y).Abs();
          return num1 > 300 || (double) num2 > 20.0;
        }
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter;
        if (!this.m_mousePanPlane.Value.Raycast(ray, out enter))
          return false;
        Vector3 point = ray.GetPoint(enter);
        float x = (point.x - this.m_mousePanStartPoint.x).Clamp(-100000f, 100000f);
        float y = (point.z - this.m_mousePanStartPoint.z).Clamp(-100000f, 100000f);
        bool flag = this.isValidPosition(this.m_state.PivotPosition - new Vector2(x, y).ToRelTile2f());
        if (this.m_isOnValidTile && !flag)
          return false;
        this.m_state.PivotPosition -= new Vector2(x, y).ToRelTile2f();
        this.m_isOnValidTile = flag;
      }
      return false;
    }

    private void panByInCameraSpace(Vector2 panAmount)
    {
      Vector2 vector2 = panAmount.RotateDeg(-this.m_yawAngleDeg);
      Tile2f tile2f = (this.m_state.PivotPosition.ToVector2() + vector2).ToTile2f();
      if (this.m_isOnValidTile)
      {
        if (!this.isValidPosition(tile2f))
          return;
        this.m_state.PivotPosition = tile2f;
      }
      else
      {
        this.m_state.PivotPosition = tile2f;
        this.m_isOnValidTile = this.isValidPosition(this.m_state.PivotPosition);
      }
    }

    private bool isValidPosition(Tile2i position) => this.m_terrainManager.IsValidCoord(position);

    private bool isValidPosition(Tile2f position)
    {
      return this.m_terrainManager.IsValidCoord(position.Tile2i);
    }

    private bool processZoom()
    {
      float num1 = -1f * Input.GetAxis("MouseScroll");
      bool flag = false;
      if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.ZoomIn))
      {
        num1 -= this.m_keyZoomScalePerSec * Time.unscaledDeltaTime;
        flag = true;
      }
      else if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.ZoomOut))
      {
        num1 += this.m_keyZoomScalePerSec * Time.unscaledDeltaTime;
        flag = true;
      }
      if (num1.IsNearZero())
        return false;
      float num2 = (double) num1 >= 0.0 ? num1 + 1f : (float) (1.0 / (-(double) num1 + 1.0));
      if ((double) num2 > 1.0 && this.OrbitRadiusTarget < new RelTile1f(1))
        this.OrbitRadiusTarget = new RelTile1f(1);
      this.OrbitRadiusTarget *= num2.Clamp(0.5f, 2f).ToFix32();
      return flag;
    }

    private void processMouseRotation()
    {
      Vector2f vector2f = new Vector2f(Input.GetAxis("MouseX").ToFix32(), Input.GetAxis("MouseY").ToFix32());
      if (vector2f.IsZero)
        return;
      AngleDegrees1f angleDegrees1f = (vector2f.X * OrbitalCameraModel.MOUSE_LOOK_SCALE).Degrees();
      this.YawAngleTarget -= angleDegrees1f;
      this.m_trackedTargetAngle -= angleDegrees1f;
      this.PitchAngleTarget -= (vector2f.Y * OrbitalCameraModel.MOUSE_LOOK_SCALE).Degrees();
    }

    private bool processKeyboardRotation()
    {
      float num;
      if (this.m_shortuctsManager.IsOn(this.m_shortuctsManager.RotateClockwise))
      {
        num = this.m_keyRotationScalePerSec * Time.unscaledDeltaTime;
      }
      else
      {
        if (!this.m_shortuctsManager.IsOn(this.m_shortuctsManager.RotateCounterClockwise))
          return false;
        num = -this.m_keyRotationScalePerSec * Time.unscaledDeltaTime;
      }
      AngleDegrees1f angleDegrees1f = num.Degrees();
      this.YawAngleTarget -= num.Degrees();
      this.m_trackedTargetAngle -= angleDegrees1f;
      return true;
    }

    public void SetMousePanDisabled(bool isDisabled) => this.m_mousePanDisabled = isDisabled;

    public void SetTargetTracking(Transform transform, bool alsoTrackRotation = false)
    {
      if (!(bool) (UnityEngine.Object) transform)
      {
        this.TrackedTarget = Option<Transform>.None;
      }
      else
      {
        this.TrackedTarget = (Option<Transform>) transform;
        this.m_alsoTrackTargetRotation = alsoTrackRotation;
        this.m_trackedTargetAngle = this.YawAngleTarget - AngleDegrees1f.FromDegrees(-transform.eulerAngles.y.ToFix32());
      }
    }

    public void CancelTargetTracking()
    {
      if (this.TrackedTarget.HasValue && (bool) (UnityEngine.Object) this.TrackedTarget.Value)
      {
        this.PanTo(this.TrackedTarget.Value.transform.position.ToTile3f().Xy);
        this.releaseDamping();
      }
      this.TrackedTarget = Option<Transform>.None;
    }

    static OrbitalCameraModel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      OrbitalCameraModel.MIN_HEIGHT_ABOVE_TERRAIN = new RelTile1f(6);
      OrbitalCameraModel.MIN_PIVOT_DISTANCE = new RelTile1f(16);
      OrbitalCameraModel.MAX_PIVOT_DISTANCE = new RelTile1f(400);
      OrbitalCameraModel.DEFAULT_CAMERA_PIVOT_DIST = new RelTile1f(100);
      OrbitalCameraModel.DEFAULT_CAMERA_YAW = 0.Degrees();
      OrbitalCameraModel.DEFAULT_CAMERA_PITCH = 60.Degrees();
      OrbitalCameraModel.MIN_PITCH = 0.Degrees();
      OrbitalCameraModel.MAX_PITCH = 89.Degrees();
      OrbitalCameraModel.MOUSE_LOOK_SCALE = (Fix32) 6;
    }
  }
}
