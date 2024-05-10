// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.FuelStationWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class FuelStationWindowView : StaticEntityInspectorBase<FuelStation>
  {
    private readonly FuelStationInspector m_controller;
    private readonly VehiclesAssignerView<TruckProto> m_vehiclesAssigner;

    protected override FuelStation Entity => this.m_controller.SelectedEntity;

    public FuelStationWindowView(
      FuelStationInspector controller,
      VehiclesAssignerView<TruckProto> vehiclesAssigner)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<FuelStationInspector>();
      this.m_vehiclesAssigner = vehiclesAssigner.CheckNotNull<VehiclesAssignerView<TruckProto>>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updater = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Options);
      this.AddSwitch(itemContainer, Tr.Option_AllowRefuelInEntity.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<ToggleFuelStationTrucksAllowedToRefuelCmd>(new ToggleFuelStationTrucksAllowedToRefuelCmd(this.Entity.Id))), updater, (Func<bool>) (() => this.Entity.AllowTrucksToRefuel));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelAvailable);
      BufferView buffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      this.AddVehiclesAssigner<TruckProto>(this.m_vehiclesAssigner, (LocStrFormatted) Tr.AssignedTrucks__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTrucks__FuelStation_Tooltip));
      this.AddBuildingsAssignerForExport(this.m_controller.Context, new Action(((EntityInspector<FuelStation, FuelStationWindowView>) this.m_controller).EditInputBuildingsClicked), (Func<IEntityAssignedAsOutput>) (() => (IEntityAssignedAsOutput) this.Entity), (LocStrFormatted) Tr.AssignedForLogistics__ExportTooltipFuelStation);
      this.SetWidth(480f);
      updater.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FuelProto)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.StoredFuel)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => buffer.UpdateState(product, capacity, quantity)));
      this.AddUpdater(updater.Build());
    }
  }
}
