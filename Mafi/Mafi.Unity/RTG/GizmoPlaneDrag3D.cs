// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoPlaneDrag3D : GizmoDragSession
  {
    private bool _isSnapEnabled;
    private float _sensitivity;
    protected InputDevicePlaneDragSession3D _planeDragSession;

    public bool IsSnapEnabled
    {
      get => this._isSnapEnabled;
      set => this._isSnapEnabled = value;
    }

    public float Sensitivity
    {
      get => this._sensitivity;
      set => this._sensitivity = Mathf.Max(0.0001f, value);
    }

    public override bool IsActive
    {
      get => this._planeDragSession != null && this._planeDragSession.IsActive;
    }

    protected override bool DoBeginSession()
    {
      this._planeDragSession = new InputDevicePlaneDragSession3D(MonoSingleton<RTInputDevice>.Get.Device, MonoSingleton<RTFocusCamera>.Get.TargetCamera);
      this._planeDragSession.Plane = this.CalculateDragPlane();
      return this._planeDragSession.Begin();
    }

    protected override bool DoUpdateSession() => this._planeDragSession.Update();

    protected override void DoEndSession()
    {
      this._planeDragSession.End();
      this._planeDragSession = (InputDevicePlaneDragSession3D) null;
    }

    protected bool CanSnap() => this._isSnapEnabled;

    protected abstract Plane CalculateDragPlane();

    protected GizmoPlaneDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sensitivity = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
