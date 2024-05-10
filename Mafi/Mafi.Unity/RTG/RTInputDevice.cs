// Decompiled with JetBrains decompiler
// Type: RTG.RTInputDevice
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTInputDevice : MonoSingleton<RTInputDevice>
  {
    private IInputDevice _inputDevice;

    public IInputDevice Device => this._inputDevice;

    public InputDeviceType DeviceType => this._inputDevice.DeviceType;

    public void Update_SystemCall() => this._inputDevice.Update();

    private void Awake() => this._inputDevice = (IInputDevice) new MouseInputDevice();

    public RTInputDevice()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
