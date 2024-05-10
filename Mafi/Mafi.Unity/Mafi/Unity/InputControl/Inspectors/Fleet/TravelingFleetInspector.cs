// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Fleet.TravelingFleetInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.World;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Fleet
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TravelingFleetInspector : EntityInspector<TravelingFleet, TravelingFleetWindowView>
  {
    private readonly TravelingFleetWindowView m_windowView;

    public TravelingFleetInspector(
      InspectorContext inspectorContext,
      TravelingFleetManager fleetManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new TravelingFleetWindowView(this, fleetManager);
    }

    public void ToggleRepair(EntityId shipyardId, bool isRepairing)
    {
      this.InputScheduler.ScheduleInputCmd<ShipyardSetRepairingCmd>(new ShipyardSetRepairingCmd(shipyardId, isRepairing));
    }

    protected override TravelingFleetWindowView GetView() => this.m_windowView;
  }
}
