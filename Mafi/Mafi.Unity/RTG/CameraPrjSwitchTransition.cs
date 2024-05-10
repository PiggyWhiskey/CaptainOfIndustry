// Decompiled with JetBrains decompiler
// Type: RTG.CameraPrjSwitchTransition
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class CameraPrjSwitchTransition
  {
    private IEnumerator _transitionCrtn;
    private MonoBehaviour _targetMono;
    private Camera _targetCamera;
    private float _camFieldOfView;
    private Vector3 _camFocusPoint;
    private Vector3 _camRestorePosition;
    private CameraPrjSwitchTransition.Type _transitionType;
    private float _durationInSeconds;
    private float _progress;

    public event CameraProjectionSwitchBeginHandler TransitionBegin;

    public event CameraProjectionSwitchUpdateHandler TransitionUpdate;

    public event CameraProjectionSwitchBeginHandler TransitionEnd;

    public MonoBehaviour TargetMono
    {
      get => this._targetMono;
      set
      {
        if (this.IsActive || !((Object) value != (Object) null))
          return;
        this._targetMono = value;
      }
    }

    public Camera TargetCamera
    {
      get => this._targetCamera;
      set
      {
        if (this.IsActive || !((Object) value != (Object) null))
          return;
        this._targetCamera = value;
      }
    }

    public CameraPrjSwitchTransition.Type TransitionType => this._transitionType;

    public float DurationInSeconds
    {
      get => this._durationInSeconds;
      set
      {
        if (this.IsActive)
          return;
        this._durationInSeconds = Mathf.Max(0.01f, Mathf.Abs(value));
      }
    }

    public float Progress => this._progress;

    public float CamFieldOfView
    {
      get => this._camFieldOfView;
      set
      {
        if (this.IsActive)
          return;
        this._camFieldOfView = Mathf.Abs(value);
      }
    }

    public Vector3 CamFocusPoint
    {
      get => this._camFocusPoint;
      set
      {
        if (this.IsActive)
          return;
        this._camFocusPoint = value;
      }
    }

    public bool IsActive => this._transitionType != 0;

    public void Begin()
    {
      if ((Object) this._targetMono == (Object) null || (Object) this._targetCamera == (Object) null)
        return;
      if (this.IsActive)
      {
        this.TargetMono.StopCoroutine(this._transitionCrtn);
        this._transitionCrtn = (IEnumerator) null;
        this._targetCamera.transform.position = this._camRestorePosition;
      }
      this.TargetMono.StartCoroutine(this._transitionCrtn = this.DoTransition());
    }

    private IEnumerator DoTransition()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CameraPrjSwitchTransition.\u003CDoTransition\u003Ed__41(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public CameraPrjSwitchTransition()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._camFieldOfView = 60f;
      this._durationInSeconds = 0.23f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum Type
    {
      None,
      ToOrtho,
      ToPerspective,
    }
  }
}
