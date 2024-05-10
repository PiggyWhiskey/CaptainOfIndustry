// Decompiled with JetBrains decompiler
// Type: RTG.RTFocusCamera
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTFocusCamera : MonoSingleton<RTFocusCamera>
  {
    [SerializeField]
    private Camera _targetCamera;
    private Transform _targetTransform;
    [SerializeField]
    private float _fieldOfView;
    private WorldTransformSnapshot _worldTransformSnapshot;
    private CameraPrjSwitchTransition _prjSwitchTranstion;
    private bool _isDoingFocus;
    private IEnumerator _focusCrtn;
    private bool _isDoingRotationSwitch;
    private IEnumerator _genricCamTransformCrtn;
    private bool _isObjectVisibilityDirty;
    private List<GameObject> _visibleObjects;
    private float _focusPointOffset;
    private Vector3 _lastFocusPoint;
    private bool[] _moveDirFlags;
    private float _currentAcceleration;
    [SerializeField]
    private CameraSettings _settings;
    [SerializeField]
    private CameraMoveSettings _moveSettings;
    [SerializeField]
    private CameraPanSettings _panSettings;
    [SerializeField]
    private CameraLookAroundSettings _lookAroundSettings;
    [SerializeField]
    private CameraOrbitSettings _orbitSettings;
    [SerializeField]
    private CameraZoomSettings _zoomSettings;
    [SerializeField]
    private CameraFocusSettings _focusSettings;
    [SerializeField]
    private CameraRotationSwitchSettings _rotationSwitchSettings;
    [SerializeField]
    private CameraProjectionSwitchSettings _projectionSwitchSettings;
    [SerializeField]
    private CameraHotkeys _hotkeys;

    public event CameraProjectionSwitchBeginHandler PrjSwitchTransitionBegin;

    public event CameraProjectionSwitchUpdateHandler PrjSwitchTransitionUpdate;

    public event CameraProjectionSwitchBeginHandler PrjSwitchTransitionEnd;

    public event CameraCanProcessInputHandler CanProcessInput;

    public event CameraCanUseScrollWheelHandler CanUseScrollWheel;

    public Camera TargetCamera => this._targetCamera;

    public bool IsDoingProjectionSwitch => this._prjSwitchTranstion.IsActive;

    public CameraPrjSwitchTransition.Type PrjSwitchTransitionType
    {
      get => this._prjSwitchTranstion.TransitionType;
    }

    public bool IsDoingRotationSwitch => this._isDoingRotationSwitch;

    public float PrjSwitchProgress => this._prjSwitchTranstion.Progress;

    public float PrjSwitchDurationInSeconds
    {
      get => this._projectionSwitchSettings.TransitionDurationInSeconds;
    }

    public bool IsDoingFocus => this._isDoingFocus;

    public Vector3 WorldPosition
    {
      get => this._targetTransform.position;
      set
      {
        Vector3 focusPoint = this.GetFocusPoint();
        this._targetTransform.position = value;
        this.SetFocusPoint(focusPoint);
      }
    }

    public Quaternion WorldRotation
    {
      get => this._targetTransform.rotation;
      set => this._targetTransform.rotation = value;
    }

    public Vector3 Right => this._targetTransform.right;

    public Vector3 Up => this._targetTransform.up;

    public Vector3 Look => this._targetTransform.forward;

    public bool IsMovingForward => this._moveDirFlags[4];

    public bool IsMovingBackwards => this._moveDirFlags[5];

    public bool IsMovingLeft => this._moveDirFlags[0];

    public bool IsMovingRight => this._moveDirFlags[1];

    public bool IsMovingUp => this._moveDirFlags[2];

    public bool IsMovingDown => this._moveDirFlags[3];

    public CameraSettings Settings => this._settings;

    public CameraMoveSettings MoveSettings => this._moveSettings;

    public CameraPanSettings PanSettings => this._panSettings;

    public CameraLookAroundSettings LookAroundSettings => this._lookAroundSettings;

    public CameraOrbitSettings OrbitSettings => this._orbitSettings;

    public CameraZoomSettings ZoomSettings => this._zoomSettings;

    public CameraFocusSettings FocusSettings => this._focusSettings;

    public CameraRotationSwitchSettings RotationSwitchSettings => this._rotationSwitchSettings;

    public CameraProjectionSwitchSettings ProjectionSwitchSettings
    {
      get => this._projectionSwitchSettings;
    }

    public CameraHotkeys Hotkeys => this._hotkeys;

    public bool IsViewportHoveredByDevice()
    {
      Vector3 viewportPoint = this.TargetCamera.ScreenToViewportPoint((Vector3) (Vector2) MonoSingleton<RTInputDevice>.Get.Device.GetPositionYAxisUp());
      return (double) viewportPoint.x >= 0.0 && (double) viewportPoint.x <= 1.0 && (double) viewportPoint.y >= 0.0 && (double) viewportPoint.y <= 1.0;
    }

    public void SetTargetCamera(Camera camera)
    {
      if ((UnityEngine.Object) camera == (UnityEngine.Object) null || !Application.isPlaying || this.IsDoingFocus || this.IsDoingProjectionSwitch || this.IsDoingRotationSwitch)
        return;
      this._targetCamera = camera;
      this._targetTransform = camera.transform;
      this._fieldOfView = camera.fieldOfView;
      this.SetFocusPoint(this.GetFocusPoint());
      this.AdjustOrthoSizeForFocusPt();
      this._isObjectVisibilityDirty = true;
    }

    public void SetFieldOfView(float fov)
    {
      this._targetCamera.fieldOfView = fov;
      this._fieldOfView = fov;
    }

    public void SetObjectVisibilityDirty() => this._isObjectVisibilityDirty = true;

    public void GetVisibleObjects(List<GameObject> visibleObjects)
    {
      visibleObjects.Clear();
      if (this._isObjectVisibilityDirty)
      {
        this.TargetCamera.GetVisibleObjects(new CameraViewVolume(this.TargetCamera), this._visibleObjects);
        this._isObjectVisibilityDirty = false;
      }
      if (this._visibleObjects.Count == 0)
        return;
      visibleObjects.AddRange((IEnumerable<GameObject>) this._visibleObjects);
    }

    public void PerformRotationSwitch(Quaternion targetRotation)
    {
      if (this.IsDoingProjectionSwitch)
        return;
      this.StopCamTransform();
      this.StopFocus();
      if (this.RotationSwitchSettings.SwitchMode == CameraRotationSwitchMode.Constant)
        this.StartCoroutine(this._genricCamTransformCrtn = this.DoConstantRotationSwitch(targetRotation));
      else if (this.RotationSwitchSettings.SwitchMode == CameraRotationSwitchMode.Smooth)
        this.StartCoroutine(this._genricCamTransformCrtn = this.DoSmoothRotationSwitch(targetRotation));
      else if (this.RotationSwitchSettings.SwitchType == CameraRotationSwitchType.InPlace)
      {
        this._targetTransform.rotation = targetRotation;
      }
      else
      {
        Vector3 focusPoint = this.GetFocusPoint();
        this._targetTransform.rotation = targetRotation;
        this._targetTransform.position = focusPoint - this._targetTransform.forward * this._focusPointOffset;
      }
    }

    public void PerformProjectionSwitch()
    {
      this.StopCamTransform();
      this.StopFocus();
      if (this.ProjectionSwitchSettings.SwitchMode == CameraProjectionSwitchMode.Transition)
      {
        this._prjSwitchTranstion.TargetCamera = this._targetCamera;
        this._prjSwitchTranstion.CamFieldOfView = this._fieldOfView;
        this._prjSwitchTranstion.CamFocusPoint = this.GetFocusPoint();
        this._prjSwitchTranstion.DurationInSeconds = this.ProjectionSwitchSettings.TransitionDurationInSeconds;
        this._prjSwitchTranstion.Begin();
      }
      else
        this.PerformInstantProjectionSwitch();
    }

    public void Focus(List<GameObject> gameObjects)
    {
      AABB focusAABB = ObjectBounds.CalcObjectCollectionWorldAABB((IEnumerable<GameObject>) gameObjects, new ObjectBounds.QueryConfig()
      {
        NoVolumeSize = Vector3.one * 0.01f,
        ObjectTypes = GameObjectType.Mesh | GameObjectType.Terrain | GameObjectType.Sprite
      });
      if (!focusAABB.IsValid)
        return;
      this.Focus(focusAABB);
    }

    public void Focus(AABB focusAABB)
    {
      if (this._isDoingFocus || this.IsDoingProjectionSwitch || this.IsDoingRotationSwitch || !focusAABB.IsValid)
        return;
      this.StopCamTransform();
      CameraFocus.Data focusData = CameraFocus.CalculateFocusData(this.TargetCamera, focusAABB, this.FocusSettings);
      if (this.FocusSettings.FocusMode == CameraFocusMode.Instant)
        this.PerformInstantFocus(focusData);
      else if (this.FocusSettings.FocusMode == CameraFocusMode.Constant)
      {
        this.StartCoroutine(this._focusCrtn = this.DoConstantFocus(focusData));
      }
      else
      {
        if (this.FocusSettings.FocusMode != CameraFocusMode.Smooth)
          return;
        this.StartCoroutine(this._focusCrtn = this.DoSmoothFocus(focusData));
      }
    }

    public void Update_SystemCall()
    {
      if (this.CanCameraProcessInput() && MonoSingleton<RTInputDevice>.Get.DeviceType == InputDeviceType.Mouse)
        this.HandleMouseAndKeyboardInput();
      if (this._worldTransformSnapshot.SameAs(this._targetTransform))
        return;
      this.SetObjectVisibilityDirty();
      this._worldTransformSnapshot.Snaphot(this._targetTransform);
    }

    private void Awake()
    {
      this._targetCamera = Camera.main;
      if ((UnityEngine.Object) this.TargetCamera == (UnityEngine.Object) null)
      {
        Debug.Break();
        Debug.LogError((object) "RTCamera: No target camera was specified.");
      }
      this.SetTargetCamera(this.TargetCamera);
      this._worldTransformSnapshot.Snaphot(this._targetTransform);
      this._prjSwitchTranstion.TargetMono = (MonoBehaviour) this;
      this._prjSwitchTranstion.TransitionBegin += new CameraProjectionSwitchBeginHandler(this.OnPrjSwitchTransitionBegin);
      this._prjSwitchTranstion.TransitionUpdate += new CameraProjectionSwitchUpdateHandler(this.OnPrjSwitchTransitionUpate);
      this._prjSwitchTranstion.TransitionEnd += new CameraProjectionSwitchBeginHandler(this.OnPrjSwitchTransitionEnd);
    }

    private void Start()
    {
      this._lastFocusPoint = this._targetTransform.position + this._targetTransform.forward * this._focusPointOffset;
      this.SetFocusPoint(this._lastFocusPoint);
      this.AdjustOrthoSizeForFocusPt();
    }

    private void HandleMouseAndKeyboardInput()
    {
      float zoomAmount = (this._moveSettings.MoveSpeed + this._currentAcceleration) * Time.deltaTime;
      Vector3 zero = Vector3.zero;
      this._moveDirFlags[4] = this.Hotkeys.MoveForward.IsActive();
      this._moveDirFlags[5] = !this._moveDirFlags[4] && this.Hotkeys.MoveBack.IsActive();
      this._moveDirFlags[0] = this.Hotkeys.StrafeLeft.IsActive();
      this._moveDirFlags[1] = !this._moveDirFlags[0] && this.Hotkeys.StrafeRight.IsActive();
      this._moveDirFlags[2] = this.Hotkeys.MoveUp.IsActive();
      this._moveDirFlags[3] = !this._moveDirFlags[2] && this.Hotkeys.MoveDown.IsActive();
      bool flag1 = false;
      if (this.IsMovingForward)
      {
        this.Zoom(zoomAmount);
        flag1 = true;
      }
      else if (this.IsMovingBackwards)
      {
        this.Zoom(-zoomAmount);
        flag1 = true;
      }
      if (this.IsMovingLeft)
        zero -= this._targetTransform.right * zoomAmount;
      else if (this.IsMovingRight)
        zero += this._targetTransform.right * zoomAmount;
      if (this.IsMovingUp)
        zero += this._targetTransform.up * zoomAmount;
      else if (this.IsMovingDown)
        zero -= this._targetTransform.up * zoomAmount;
      bool flag2 = (double) zero.sqrMagnitude != 0.0;
      if (flag2)
        this._targetTransform.position += zero;
      if (flag2 | flag1)
        this._currentAcceleration += this.MoveSettings.AccelerationRate * Mathf.Abs(this._targetCamera.EstimateZoomFactor(this._lastFocusPoint)) * Time.deltaTime;
      else
        this._currentAcceleration = 0.0f;
      float deviceAxisX = RTInput.MouseAxisX();
      float deviceAxisY = RTInput.MouseAxisY();
      if ((double) deviceAxisX != 0.0 || (double) deviceAxisY != 0.0)
      {
        if (this._panSettings.IsPanningEnabled && this.Hotkeys.Pan.IsActive())
        {
          if (this._panSettings.PanMode == CameraPanMode.Standard)
          {
            this.Pan(this.CalculatePanAmount(deviceAxisX, deviceAxisY));
          }
          else
          {
            this.StopCamTransform();
            this.StartCoroutine(this._genricCamTransformCrtn = this.DoSmoothPan(deviceAxisX, deviceAxisY));
          }
        }
        else if (this._orbitSettings.IsOrbitEnabled && this.Hotkeys.Orbit.IsActive())
        {
          if (this._orbitSettings.OrbitMode == CameraOrbitMode.Standard)
          {
            Vector2 orbitRotation = this.CalculateOrbitRotation(deviceAxisX, deviceAxisY);
            this.Orbit(orbitRotation.x, orbitRotation.y);
          }
          else
          {
            this.StopCamTransform();
            this.StartCoroutine(this._genricCamTransformCrtn = this.DoSmoothOrbit(deviceAxisX, deviceAxisY));
          }
        }
        else if (this._lookAroundSettings.IsLookAroundEnabled && this.Hotkeys.LookAround.IsActive())
        {
          if (this._lookAroundSettings.LookAroundMode == CameraLookAroundMode.Standard)
          {
            Vector2 lookAroundRotation = this.CalculateLookAroundRotation(deviceAxisX, deviceAxisY);
            this.LookAround(lookAroundRotation.x, lookAroundRotation.y);
          }
          else
          {
            this.StopCamTransform();
            this.StartCoroutine(this._genricCamTransformCrtn = this.DoSmoothLookAround(deviceAxisX, deviceAxisY));
          }
        }
      }
      if (!this.CanUseMouseScrollWheel())
        return;
      float deviceScroll = RTInput.MouseScroll();
      if ((double) deviceScroll == 0.0 || !this._zoomSettings.IsZoomEnabled)
        return;
      if (this._zoomSettings.ZoomMode == CameraZoomMode.Standard)
      {
        this.Zoom(this.CalculateScrollZoomAmount(deviceScroll));
      }
      else
      {
        this.StopCamTransform();
        this.StartCoroutine(this._genricCamTransformCrtn = this.DoSmoothZoom(deviceScroll));
      }
    }

    private bool CanUseMouseScrollWheel() => false;

    private bool CanCameraProcessInput() => false;

    private void Zoom(float zoomAmount)
    {
      Vector3 focusPoint = this.GetFocusPoint();
      this._targetTransform.position += this._targetTransform.forward * zoomAmount;
      if (this.TargetCamera.orthographic && (double) Vector3.Dot(focusPoint - this._targetTransform.position, this._targetTransform.forward) < 0.0099999997764825821)
        this._targetTransform.position = focusPoint - this._targetTransform.forward * (1f / 1000f);
      this.SetFocusPoint(focusPoint);
      this.AdjustOrthoSizeForFocusPt();
    }

    private Vector3 GetFocusPoint()
    {
      return this._targetTransform.position + this._targetTransform.forward * this._focusPointOffset;
    }

    private float CalculateScrollZoomAmount(float deviceScroll)
    {
      float num = deviceScroll * this._zoomSettings.GetZoomSensitivity(this.TargetCamera);
      if (this._zoomSettings.InvertZoomAxis)
        num *= -1f;
      return num * this._targetCamera.EstimateZoomFactorSpherical(this._lastFocusPoint);
    }

    private void Pan(Vector2 panAmount)
    {
      this._targetTransform.position += this._targetTransform.right * panAmount.x + this._targetTransform.up * panAmount.y;
    }

    public void LookAround(float degreesLocalX, float degreesWorldY)
    {
      this._targetTransform.Rotate(Vector3.up, degreesWorldY, Space.World);
      this._targetTransform.Rotate(this._targetTransform.right, degreesLocalX, Space.World);
    }

    private void Orbit(float degreesLocalX, float degreesWorldY)
    {
      Vector3 vector3 = this._targetTransform.position + this._targetTransform.forward * this._focusPointOffset;
      this._targetTransform.RotateAround(vector3, Vector3.up, degreesWorldY);
      this._targetTransform.RotateAround(vector3, this._targetTransform.right, degreesLocalX);
      this._targetTransform.LookAt(vector3, this._targetTransform.up);
    }

    private void PerformInstantFocus(CameraFocus.Data focusData)
    {
      this._targetTransform.position = focusData.CameraWorldPosition;
      this.SetFocusPoint(focusData.FocusPoint);
      this._lastFocusPoint = focusData.FocusPoint;
      this.AdjustOrthoSizeForFocusPt();
    }

    private void PerformInstantProjectionSwitch()
    {
      this.TargetCamera.orthographic = !this.TargetCamera.orthographic;
    }

    private Vector2 CalculateLookAroundRotation(float deviceAxisX, float deviceAxisY)
    {
      Vector2 zero = Vector2.zero with
      {
        x = -deviceAxisY * this._lookAroundSettings.Sensitivity
      };
      if (this._lookAroundSettings.InvertY)
        zero.x *= -1f;
      zero.y = deviceAxisX * this._lookAroundSettings.Sensitivity;
      if (this._lookAroundSettings.InvertX)
        zero.y *= -1f;
      return zero;
    }

    private Vector2 CalculateOrbitRotation(float deviceAxisX, float deviceAxisY)
    {
      Vector2 zero = Vector2.zero with
      {
        x = -deviceAxisY * this._orbitSettings.OrbitSensitivity
      };
      if (this._orbitSettings.InvertY)
        zero.x *= -1f;
      zero.y = deviceAxisX * this._orbitSettings.OrbitSensitivity;
      if (this._orbitSettings.InvertX)
        zero.y *= -1f;
      return zero;
    }

    private Vector2 CalculatePanAmount(float deviceAxisX, float deviceAxisY)
    {
      Vector2 zero = Vector2.zero with
      {
        x = -deviceAxisX * this._panSettings.Sensitivity
      };
      if (this._panSettings.InvertX)
        zero.x *= -1f;
      zero.y = -deviceAxisY * this._panSettings.Sensitivity;
      if (this._panSettings.InvertY)
        zero.y *= -1f;
      return zero * Mathf.Abs(this._targetCamera.EstimateZoomFactorSpherical(this._lastFocusPoint));
    }

    private void StopCamTransform()
    {
      if (this._genricCamTransformCrtn == null)
        return;
      this.StopCoroutine(this._genricCamTransformCrtn);
      this._genricCamTransformCrtn = (IEnumerator) null;
    }

    private void StopFocus()
    {
      if (this._focusCrtn == null)
        return;
      this.StopCoroutine(this._focusCrtn);
      this._focusCrtn = (IEnumerator) null;
    }

    private void SetFocusPoint(Vector3 focusPoint)
    {
      this._focusPointOffset = (focusPoint - this._targetTransform.position).magnitude;
    }

    private void AdjustOrthoSizeForFocusPt()
    {
      this.TargetCamera.orthographicSize = Mathf.Max(0.5f * this.TargetCamera.GetFrustumHeightFromDistance(this._focusPointOffset), 0.0001f);
    }

    private IEnumerator DoSmoothPan(float deviceAxisX, float deviceAxisY)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothPan\u003Ed__129(0)
      {
        \u003C\u003E4__this = this,
        deviceAxisX = deviceAxisX,
        deviceAxisY = deviceAxisY
      };
    }

    private IEnumerator DoSmoothLookAround(float deviceAxisX, float deviceAxisY)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothLookAround\u003Ed__130(0)
      {
        \u003C\u003E4__this = this,
        deviceAxisX = deviceAxisX,
        deviceAxisY = deviceAxisY
      };
    }

    private IEnumerator DoSmoothOrbit(float deviceAxisX, float deviceAxisY)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothOrbit\u003Ed__131(0)
      {
        \u003C\u003E4__this = this,
        deviceAxisX = deviceAxisX,
        deviceAxisY = deviceAxisY
      };
    }

    private IEnumerator DoSmoothZoom(float deviceScroll)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothZoom\u003Ed__132(0)
      {
        \u003C\u003E4__this = this,
        deviceScroll = deviceScroll
      };
    }

    private IEnumerator DoConstantRotationSwitch(Quaternion targetRotation)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoConstantRotationSwitch\u003Ed__133(0)
      {
        \u003C\u003E4__this = this,
        targetRotation = targetRotation
      };
    }

    private IEnumerator DoSmoothRotationSwitch(Quaternion targetRotation)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothRotationSwitch\u003Ed__134(0)
      {
        \u003C\u003E4__this = this,
        targetRotation = targetRotation
      };
    }

    private IEnumerator DoConstantFocus(CameraFocus.Data focusData)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoConstantFocus\u003Ed__135(0)
      {
        \u003C\u003E4__this = this,
        focusData = focusData
      };
    }

    private IEnumerator DoSmoothFocus(CameraFocus.Data focusData)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RTFocusCamera.\u003CDoSmoothFocus\u003Ed__136(0)
      {
        \u003C\u003E4__this = this,
        focusData = focusData
      };
    }

    private void OnPrjSwitchTransitionBegin(CameraPrjSwitchTransition.Type transitionType)
    {
      if (this.PrjSwitchTransitionBegin == null)
        return;
      this.PrjSwitchTransitionBegin(transitionType);
    }

    private void OnPrjSwitchTransitionUpate(CameraPrjSwitchTransition.Type transitionType)
    {
      if (this.PrjSwitchTransitionUpdate == null)
        return;
      this.PrjSwitchTransitionUpdate(transitionType);
    }

    private void OnPrjSwitchTransitionEnd(CameraPrjSwitchTransition.Type transitionType)
    {
      if (this.PrjSwitchTransitionEnd == null)
        return;
      this.PrjSwitchTransitionEnd(transitionType);
    }

    public RTFocusCamera()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._worldTransformSnapshot = new WorldTransformSnapshot();
      this._prjSwitchTranstion = new CameraPrjSwitchTransition();
      this._isObjectVisibilityDirty = true;
      this._visibleObjects = new List<GameObject>();
      this._focusPointOffset = 5f;
      this._moveDirFlags = new bool[Enum.GetValues(typeof (RTFocusCamera.MoveDirection)).Length];
      this._settings = new CameraSettings();
      this._moveSettings = new CameraMoveSettings();
      this._panSettings = new CameraPanSettings();
      this._lookAroundSettings = new CameraLookAroundSettings();
      this._orbitSettings = new CameraOrbitSettings();
      this._zoomSettings = new CameraZoomSettings();
      this._focusSettings = new CameraFocusSettings();
      this._rotationSwitchSettings = new CameraRotationSwitchSettings();
      this._projectionSwitchSettings = new CameraProjectionSwitchSettings();
      this._hotkeys = new CameraHotkeys();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private enum MoveDirection
    {
      Left,
      Right,
      Up,
      Down,
      Forward,
      Backwards,
    }
  }
}
