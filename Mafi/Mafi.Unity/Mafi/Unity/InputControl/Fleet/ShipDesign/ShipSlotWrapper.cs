// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipSlotWrapper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipSlotWrapper
  {
    public readonly FleetEntitySlot Group;
    public Option<FleetEntityPartProto> SelectedPart;

    public bool ForbidDowngrade => true;

    public ShipSlotWrapper(FleetEntitySlot slotsGroup)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Group = slotsGroup;
      this.SelectedPart = slotsGroup.ExistingPart;
    }
  }
}
