// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.TreePlanterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal class TreePlanterWindowView : VehicleWindowView<TreePlanter, TreePlanterWindowView>
  {
    private readonly TreePlanterInspector m_controller;

    protected override TreePlanter Entity => this.m_controller.SelectedEntity;

    public TreePlanterWindowView(
      TreePlanterInspector controller,
      MbBasedEntitiesRenderer entitiesRenderer,
      OrbitalCameraModel orbitalCameraModel)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((VehicleInspector<TreePlanter, TreePlanterWindowView>) controller, orbitalCameraModel, entitiesRenderer);
      this.m_controller = controller.CheckNotNull<TreePlanterInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.AddHeaderTutorialButton(this.m_controller.Context, IdsCore.Messages.TutorialOnTreesPlanting);
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EntityStatus);
      Txt stateLabel = this.AddLabel(itemContainer, "");
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle);
      Panel parent = this.AddOverlayPanel(this.ItemsContainer, this.Style.BufferView.Height.RoundToInt());
      BufferView objectToPlace = this.Builder.NewBufferView((IUiElement) parent);
      objectToPlace.PutTo<BufferView>((IUiElement) parent, Offset.Right(140f));
      this.AddVehicleFuelStatus(itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddAssignedToPanel(this.m_controller.Context, itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddVehicleButtons(updaterBuilder);
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => !this.Entity.IsEmpty ? (Option<ProductProto>) this.Entity.Cargo.FirstOrPhantom.Product : Option<ProductProto>.None)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Prototype.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Cargo.TotalQuantity)).Do(new Action<Option<ProductProto>, Quantity, Quantity>(objectToPlace.UpdateState));
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => this.Entity.CurrentJobInfo)).Observe<TreePlanterState>((Func<TreePlanterState>) (() => this.Entity.State)).Observe<DrivingState>((Func<DrivingState>) (() => this.Entity.DrivingState)).Do((Action<LocStrFormatted, TreePlanterState, DrivingState>) ((jobInfo, state, drivingState) =>
      {
        if (!string.IsNullOrEmpty(jobInfo.Value))
          stateLabel.SetText(string.Format("{0} ({1}, {2})", (object) jobInfo.Value, (object) state, (object) drivingState));
        else
          stateLabel.SetText(string.Format("No job ({0}, {1})", (object) state, (object) drivingState));
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
