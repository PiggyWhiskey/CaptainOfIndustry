// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.UiAudio
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UiAudio
  {
    public AudioInfo ButtonClick;
    public AudioInfo WindowOpen;
    public AudioInfo WindowClose;
    public AudioInfo TabSwitch;
    public AudioInfo Assign;
    public AudioInfo Unassign;
    public AudioInfo Up;
    public AudioInfo Down;
    public AudioInfo Rotate;
    public AudioInfo OnMoveStart;
    public AudioInfo InvalidOp;
    public AudioInfo BuildingPlaced;
    public AudioInfo TransportBind;
    public AudioInfo TransportUnbind;
    public AudioInfo EntitySelect;
    public AudioInfo EntityUnselect;
    public AudioInfo Demolish;
    public AudioInfo TurretShot;
    public AudioInfo ShipAlarm;
    public AudioInfo MoneyAction;
    public AudioInfo Sell;
    public AudioInfo InspectorClick;

    public UiAudio()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.ButtonClick = new AudioInfo("Assets/Unity/UserInterface/Audio/ButtonClick.prefab", AudioChannel.UserInterface);
      this.WindowOpen = new AudioInfo("Assets/Unity/UserInterface/Audio/WindowOpen.prefab", AudioChannel.UserInterface);
      this.WindowClose = new AudioInfo("Assets/Unity/UserInterface/Audio/WindowClose.prefab", AudioChannel.UserInterface);
      this.TabSwitch = new AudioInfo("Assets/Unity/UserInterface/Audio/TabSwitch.prefab", AudioChannel.UserInterface);
      this.Assign = new AudioInfo("Assets/Unity/UserInterface/Audio/AssignClick.prefab", AudioChannel.UserInterface);
      this.Unassign = new AudioInfo("Assets/Unity/UserInterface/Audio/UnassignClick.prefab", AudioChannel.UserInterface);
      this.Up = new AudioInfo("Assets/Unity/UserInterface/Audio/Up.prefab", AudioChannel.UserInterface);
      this.Down = new AudioInfo("Assets/Unity/UserInterface/Audio/Down.prefab", AudioChannel.UserInterface);
      this.Rotate = new AudioInfo("Assets/Unity/UserInterface/Audio/Rotate.prefab", AudioChannel.UserInterface);
      this.OnMoveStart = new AudioInfo("Assets/Unity/UserInterface/Audio/Rotate.prefab", AudioChannel.UserInterface);
      this.InvalidOp = new AudioInfo("Assets/Unity/UserInterface/Audio/InvalidOp.prefab", AudioChannel.UserInterface);
      this.BuildingPlaced = new AudioInfo("Assets/Unity/UserInterface/Audio/BuildingPlaced.prefab", AudioChannel.UserInterface);
      this.TransportBind = new AudioInfo("Assets/Unity/UserInterface/Audio/TransportBind.prefab", AudioChannel.UserInterface);
      this.TransportUnbind = new AudioInfo("Assets/Unity/UserInterface/Audio/TransportUnbind.prefab", AudioChannel.UserInterface);
      this.EntitySelect = new AudioInfo("Assets/Unity/UserInterface/Audio/EntitySelect.prefab", AudioChannel.UserInterface);
      this.EntityUnselect = new AudioInfo("Assets/Unity/UserInterface/Audio/EntityUnselect.prefab", AudioChannel.UserInterface);
      this.Demolish = new AudioInfo("Assets/Unity/UserInterface/Audio/Demolish.prefab", AudioChannel.UserInterface);
      this.TurretShot = new AudioInfo("Assets/Unity/UserInterface/Audio/TurretShot.prefab", AudioChannel.UserInterface);
      this.ShipAlarm = new AudioInfo("Assets/Unity/UserInterface/Audio/ShipAlarm.prefab", AudioChannel.UserInterface);
      this.MoneyAction = new AudioInfo("Assets/Unity/UserInterface/Audio/MoneyAction.prefab", AudioChannel.UserInterface);
      this.Sell = new AudioInfo("Assets/Unity/UserInterface/Audio/Sell.prefab", AudioChannel.UserInterface);
      this.InspectorClick = new AudioInfo("Assets/Unity/UserInterface/Audio/InspectorClick.prefab", AudioChannel.UserInterface);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
