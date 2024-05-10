// Decompiled with JetBrains decompiler
// Type: RTG.GizmoScreenDrag
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoScreenDrag : GizmoDragSession
  {
    private bool _isSnapEnabled;
    private float _sensitivity;
    protected InputDeviceScreenDragSession _screenDragSession;

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
      get => this._screenDragSession != null && this._screenDragSession.IsActive;
    }

    protected override bool DoBeginSession()
    {
      this._screenDragSession = new InputDeviceScreenDragSession(MonoSingleton<RTInputDevice>.Get.Device);
      return this._screenDragSession.Begin();
    }

    protected override bool DoUpdateSession() => this._screenDragSession.Update();

    protected override void DoEndSession()
    {
      this._screenDragSession.End();
      this._screenDragSession = (InputDeviceScreenDragSession) null;
    }

    protected bool CanSnap() => this._isSnapEnabled;

    protected GizmoScreenDrag()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sensitivity = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
