// Decompiled with JetBrains decompiler
// Type: RTG.IInputDevice
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IInputDevice
  {
    event InputDeviceDoubleTapHandler DoubleTap;

    bool DidDoubleTap { get; }

    float DoubleTapDelay { get; set; }

    InputDeviceType DeviceType { get; }

    Ray GetRay(Camera camera);

    Vector3 GetPositionYAxisUp();

    bool HasPointer();

    bool IsButtonPressed(int buttonIndex);

    bool WasButtonPressedInCurrentFrame(int buttonIndex);

    bool WasButtonReleasedInCurrentFrame(int buttonIndex);

    bool WasMoved();

    bool CreateDeltaCapture(Vector3 deltaOrigin, out int deltaCaptureId);

    void RemoveDeltaCapture(int deltaCaptureId);

    Vector3 GetCaptureDelta(int deltaCaptureId);

    Vector3 GetFrameDelta();

    void Update();
  }
}
