// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.LocationVisitCheckResultHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.World;
using Mafi.Localization;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public static class LocationVisitCheckResultHelper
  {
    public static LocStr MapToToolTip(
      TravelingFleetManager.LocationVisitCheckResult result)
    {
      switch (result)
      {
        case TravelingFleetManager.LocationVisitCheckResult.AlreadyHeadingThereOrPresent:
          return Tr.ShipCantVisit__OnWay;
        case TravelingFleetManager.LocationVisitCheckResult.Damaged:
          return Tr.ShipCantVisit__Damaged;
        case TravelingFleetManager.LocationVisitCheckResult.ShipIsBeingModified:
          return Tr.ShipCantVisit__BeingModified;
        case TravelingFleetManager.LocationVisitCheckResult.ShipIsBeingRepaired:
          return Tr.ShipCantVisit__BeingRepaired;
        case TravelingFleetManager.LocationVisitCheckResult.NotAccessible:
          return Tr.ShipCantVisit__NoAccess;
        case TravelingFleetManager.LocationVisitCheckResult.NotEnoughFuel:
          return TrCore.ShipCantVisit__NoFuel;
        case TravelingFleetManager.LocationVisitCheckResult.NotEnoughCrew:
          return TrCore.ShipCantVisit__NoCrew;
        case TravelingFleetManager.LocationVisitCheckResult.Docking:
          return Tr.ShipCantVisit__MovingToDock;
        case TravelingFleetManager.LocationVisitCheckResult.TooFar:
          return TrCore.ShipCantVisit__TooFar;
        default:
          return Tr.ShipCantVisit__Ok;
      }
    }
  }
}
