// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.TruckWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Population;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.Commands;
using Mafi.Core.Vehicles.Trucks;
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
  internal class TruckWindowView : VehicleWindowView<Truck, TruckWindowView>
  {
    private readonly TruckInspector m_controller;
    private readonly IUpointsManager m_upointsManager;

    protected override Truck Entity => this.m_controller.SelectedEntity;

    public TruckWindowView(
      TruckInspector controller,
      OrbitalCameraModel orbitalCameraModel,
      MbBasedEntitiesRenderer entitiesRenderer,
      IUpointsManager upointsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((VehicleInspector<Truck, TruckWindowView>) controller, orbitalCameraModel, entitiesRenderer);
      this.m_upointsManager = upointsManager;
      this.m_controller = controller.CheckNotNull<TruckInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EntityStatus);
      Txt stateLabel = this.AddLabel(itemContainer, "");
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle);
      BufferWithMultipleProductsView buffer = new BufferWithMultipleProductsView((IUiElement) itemContainer, this.Builder, new Action(onTrashClick));
      itemContainer.Append((IUiElement) buffer, new float?(buffer.GetHeight()));
      Tooltip trashTooltip = buffer.TrashBtn.AddToolTipAndReturn();
      buffer.TrashBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn);
      this.AddVehicleFuelStatus(itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddAssignedToPanel(this.m_controller.Context, itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddVehicleButtons(updaterBuilder);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.HasJobs)).Observe<string>((Func<string>) (() => this.Entity.CurrentJobInfo.Value)).Do((Action<bool, string>) ((hasJobs, jobInfo) => stateLabel.SetText(hasJobs ? jobInfo : Tr.EntityStatus__NoJobs.TranslatedString)));
      string discardCargoTooltip = VehicleCommandsProcessor.COST_TO_DISCARD_CARGO.FormatWithUnitySuffix().Value + " - " + Tr.Cargo__DiscardTooltip.ToString();
      Lyst<ProductQuantity> productsCache = new Lyst<ProductQuantity>();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        productsCache.Clear();
        this.Entity.Cargo.GetCargoProducts(productsCache);
        return (IIndexable<ProductQuantity>) productsCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Observe<bool>((Func<bool>) (() => this.Entity.IsCannotDeliverNotificationActive)).Observe<bool>((Func<bool>) (() => this.m_upointsManager.CanConsume(VehicleCommandsProcessor.COST_TO_DISCARD_CARGO))).Do((Action<Quantity, Lyst<ProductQuantity>, bool, bool>) ((capacity, cargo, cannotDeliver, canConsumeUnity) =>
      {
        buffer.SetProducts(cargo, capacity, cannotDeliver);
        trashTooltip.SetText(canConsumeUnity ? discardCargoTooltip : TrCore.TradeStatus__NoUnity.TranslatedString);
      }));
      this.AddUpdater(updaterBuilder.Build());

      void onTrashClick()
      {
        this.m_controller.Context.InputScheduler.ScheduleInputCmd<DiscardVehicleCargoCmd>(new DiscardVehicleCargoCmd((Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      }
    }
  }
}
