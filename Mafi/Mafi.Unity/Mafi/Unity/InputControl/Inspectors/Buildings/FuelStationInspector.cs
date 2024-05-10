// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.FuelStationInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class FuelStationInspector : EntityInspector<FuelStation, FuelStationWindowView>
  {
    private readonly FuelStationWindowView m_windowView;

    public FuelStationInspector(
      InspectorContext inspectorContext,
      BuildingsAssigner buildingsAssigner,
      VehiclesAssignerFactory vehiclesAssignerFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.SetBuildingsAssigner(buildingsAssigner);
      this.m_windowView = new FuelStationWindowView(this, vehiclesAssignerFactory.CreateNewView<TruckProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity)));
      this.m_windowView.AddUpdater(this.CreateVehiclesUpdater());
    }

    protected override FuelStationWindowView GetView() => this.m_windowView;
  }
}
