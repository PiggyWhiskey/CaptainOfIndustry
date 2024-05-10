// Decompiled with JetBrains decompiler
// Type: RTG.TouchInputDevice
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class TouchInputDevice : InputDeviceBase
  {
    private int _maxNumberOfTouches;

    public int MaxNumberOfTouches => this._maxNumberOfTouches;

    public int TouchCount => RTInput.TouchCount;

    public override InputDeviceType DeviceType => InputDeviceType.Touch;

    public TouchInputDevice(int maxNumberOfTouches)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._maxNumberOfTouches = Mathf.Max(1, maxNumberOfTouches);
    }

    public override Vector3 GetFrameDelta()
    {
      return this.TouchCount != 0 ? (Vector3) RTInput.TouchDelta(0) : Vector3.zero;
    }

    public override Ray GetRay(Camera camera)
    {
      Ray ray = new Ray(Vector3.zero, Vector3.zero);
      if (this.TouchCount != 0)
        ray = camera.ScreenPointToRay((Vector3) RTInput.TouchPosition(0));
      return ray;
    }

    public override Vector3 GetPositionYAxisUp()
    {
      return this.TouchCount != 0 ? (Vector3) RTInput.TouchPosition(0) : Vector3.zero;
    }

    public override bool HasPointer() => this.TouchCount != 0;

    public override bool IsButtonPressed(int buttonIndex)
    {
      int touchCount = this.TouchCount;
      return buttonIndex < touchCount && touchCount <= this.MaxNumberOfTouches;
    }

    public override bool WasButtonPressedInCurrentFrame(int buttonIndex)
    {
      int touchCount = this.TouchCount;
      return buttonIndex < touchCount && touchCount <= this.MaxNumberOfTouches && RTInput.TouchBegan(buttonIndex);
    }

    public override bool WasButtonReleasedInCurrentFrame(int buttonIndex)
    {
      int touchCount = this.TouchCount;
      return buttonIndex < touchCount && touchCount <= this.MaxNumberOfTouches && RTInput.TouchEndedOrCanceled(buttonIndex);
    }

    public override bool WasMoved()
    {
      int touchCount = this.TouchCount;
      if (touchCount != 0)
      {
        for (int touchIndex = 0; touchIndex < touchCount && touchIndex < this.MaxNumberOfTouches; ++touchIndex)
        {
          if (RTInput.TouchMoved(touchIndex))
            return true;
        }
      }
      return false;
    }

    protected override void UpateFrameDeltas()
    {
    }
  }
}
