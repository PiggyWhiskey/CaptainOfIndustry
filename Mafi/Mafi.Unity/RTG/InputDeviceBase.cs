// Decompiled with JetBrains decompiler
// Type: RTG.InputDeviceBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class InputDeviceBase : IInputDevice
  {
    private float _doubleTapDelay;
    private float _lastTapTime;
    private bool _didDoubleTap;
    private int _maxNumDeltaCaptures;
    private InputDeviceDeltaCapture[] _deltaCaptures;

    public event InputDeviceDoubleTapHandler DoubleTap;

    public bool DidDoubleTap => this._didDoubleTap;

    public float DoubleTapDelay
    {
      get => this._doubleTapDelay;
      set => this._doubleTapDelay = Mathf.Max(value, 0.0f);
    }

    public abstract InputDeviceType DeviceType { get; }

    public InputDeviceBase()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._doubleTapDelay = 0.5f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetMaxNumDeltaCaptures(50);
    }

    public void SetMaxNumDeltaCaptures(int maxNumDeltaCaptures)
    {
      this._maxNumDeltaCaptures = Mathf.Max(1, maxNumDeltaCaptures);
      this._deltaCaptures = new InputDeviceDeltaCapture[this._maxNumDeltaCaptures];
    }

    public bool CreateDeltaCapture(Vector3 deltaOrigin, out int deltaCaptureId)
    {
      deltaCaptureId = 0;
      while (deltaCaptureId < this._maxNumDeltaCaptures && this._deltaCaptures[deltaCaptureId] != null)
        ++deltaCaptureId;
      if (deltaCaptureId == this._maxNumDeltaCaptures)
      {
        deltaCaptureId = -1;
        return false;
      }
      InputDeviceDeltaCapture deviceDeltaCapture = new InputDeviceDeltaCapture(deltaCaptureId, deltaOrigin);
      this._deltaCaptures[deltaCaptureId] = deviceDeltaCapture;
      return true;
    }

    public void RemoveDeltaCapture(int deltaCaptureId)
    {
      if (deltaCaptureId < 0 || deltaCaptureId >= this._maxNumDeltaCaptures)
        return;
      this._deltaCaptures[deltaCaptureId] = (InputDeviceDeltaCapture) null;
    }

    public Vector3 GetCaptureDelta(int deltaCaptureId)
    {
      return deltaCaptureId >= 0 && deltaCaptureId < this._maxNumDeltaCaptures && this._deltaCaptures[deltaCaptureId] != null ? this._deltaCaptures[deltaCaptureId].Delta : Vector3.zero;
    }

    public abstract Vector3 GetFrameDelta();

    public abstract Ray GetRay(Camera camera);

    public abstract Vector3 GetPositionYAxisUp();

    public abstract bool HasPointer();

    public abstract bool IsButtonPressed(int buttonIndex);

    public abstract bool WasButtonPressedInCurrentFrame(int buttonIndex);

    public abstract bool WasButtonReleasedInCurrentFrame(int buttonIndex);

    public abstract bool WasMoved();

    public void Update()
    {
      this.UpateFrameDeltas();
      this.UpdateDeltaCaptures();
      this.DetectAndHandleDoubleTap();
    }

    protected abstract void UpateFrameDeltas();

    private void UpdateDeltaCaptures()
    {
      int index = 0;
      Vector3 positionYaxisUp = this.GetPositionYAxisUp();
      while (index < this._maxNumDeltaCaptures && this._deltaCaptures[index] != null)
        this._deltaCaptures[index++].Update(positionYaxisUp);
    }

    private void DetectAndHandleDoubleTap()
    {
      if (!this.WasButtonPressedInCurrentFrame(0))
        return;
      if ((double) Time.time - (double) this._lastTapTime < (double) this._doubleTapDelay)
      {
        this._lastTapTime = 0.0f;
        this._didDoubleTap = true;
        if (this.DoubleTap == null)
          return;
        this.DoubleTap((IInputDevice) this, (Vector2) this.GetPositionYAxisUp());
      }
      else
      {
        this._didDoubleTap = false;
        this._lastTapTime = Time.time;
      }
    }
  }
}
