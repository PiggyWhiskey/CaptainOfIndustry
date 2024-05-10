// Decompiled with JetBrains decompiler
// Type: RTG.MouseInputDevice
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MouseInputDevice : InputDeviceBase
  {
    private Vector3 _frameDelta;
    private Vector3 _mousePosInLastFrame;

    public override InputDeviceType DeviceType => InputDeviceType.Mouse;

    public MouseInputDevice()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._frameDelta = Vector3.zero;
      this._mousePosInLastFrame = RTInput.MousePosition;
    }

    public override Vector3 GetFrameDelta() => this._frameDelta;

    public override Ray GetRay(Camera camera) => camera.ScreenPointToRay(RTInput.MousePosition);

    public override Vector3 GetPositionYAxisUp() => RTInput.MousePosition;

    public override bool HasPointer()
    {
      return RTInput.IsMousePresent && !EventSystem.current.IsPointerOverGameObject();
    }

    public override bool IsButtonPressed(int buttonIndex)
    {
      return RTInput.IsMouseButtonPressed(buttonIndex);
    }

    public override bool WasButtonPressedInCurrentFrame(int buttonIndex)
    {
      return RTInput.WasMouseButtonPressedThisFrame(buttonIndex);
    }

    public override bool WasButtonReleasedInCurrentFrame(int buttonIndex)
    {
      return RTInput.WasMouseButtonReleasedThisFrame(buttonIndex);
    }

    public override bool WasMoved() => RTInput.WasMouseMoved();

    protected override void UpateFrameDeltas()
    {
      this._frameDelta = RTInput.MousePosition - this._mousePosInLastFrame;
      this._mousePosInLastFrame = RTInput.MousePosition;
    }
  }
}
