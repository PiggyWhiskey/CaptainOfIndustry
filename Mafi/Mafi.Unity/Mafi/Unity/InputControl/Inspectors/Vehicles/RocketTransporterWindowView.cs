// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.RocketTransporterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal class RocketTransporterWindowView : 
    VehicleWindowView<RocketTransporter, RocketTransporterWindowView>
  {
    private readonly RocketTransporterInspector m_controller;

    protected override RocketTransporter Entity => this.m_controller.SelectedEntity;

    public RocketTransporterWindowView(
      RocketTransporterInspector controller,
      OrbitalCameraModel orbitalCameraModel,
      MbBasedEntitiesRenderer entitiesRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((VehicleInspector<RocketTransporter, RocketTransporterWindowView>) controller, orbitalCameraModel, entitiesRenderer);
      this.m_controller = controller.CheckNotNull<RocketTransporterInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EntityStatus);
      Txt stateLabel = this.AddLabel(itemContainer, "");
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => this.Entity.CurrentJobInfo)).Observe<DrivingState>((Func<DrivingState>) (() => this.Entity.DrivingState)).Do((Action<LocStrFormatted, DrivingState>) ((jobInfo, drivingState) =>
      {
        if (!string.IsNullOrEmpty(jobInfo.Value))
          stateLabel.SetText(string.Format("{0} ({1})", (object) jobInfo.Value, (object) drivingState));
        else
          stateLabel.SetText(string.Format("No job ({0})", (object) drivingState));
      }));
      this.AddVehicleButtons(updaterBuilder, true);
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
