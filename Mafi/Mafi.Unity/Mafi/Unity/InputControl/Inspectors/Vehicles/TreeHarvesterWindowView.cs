// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.TreeHarvesterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal class TreeHarvesterWindowView : VehicleWindowView<TreeHarvester, TreeHarvesterWindowView>
  {
    private readonly TreeHarvesterInspector m_controller;
    private readonly VehiclesAssignerView<TruckProto> m_vehiclesAssigner;

    protected override TreeHarvester Entity => this.m_controller.SelectedEntity;

    public TreeHarvesterWindowView(
      TreeHarvesterInspector controller,
      MbBasedEntitiesRenderer entitiesRenderer,
      OrbitalCameraModel orbitalCameraModel,
      VehiclesAssignerView<TruckProto> vehiclesAssigner)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((VehicleInspector<TreeHarvester, TreeHarvesterWindowView>) controller, orbitalCameraModel, entitiesRenderer);
      this.m_controller = controller;
      this.m_vehiclesAssigner = vehiclesAssigner;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.SetWidth(480f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Status);
      Txt stateLabel = this.AddLabel(itemContainer, "");
      this.AddVehicleFuelStatus(itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddVehiclesAssigner<TruckProto>(this.m_vehiclesAssigner, (LocStrFormatted) Tr.AssignedTrucks__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTrucks__TreeHarvester_Tooltip));
      this.AddVehicleButtons(updaterBuilder);
      updaterBuilder.Observe<string>((Func<string>) (() => this.Entity.CurrentJobInfo.Value)).Do((Action<string>) (jobInfo => stateLabel.SetText(jobInfo)));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
