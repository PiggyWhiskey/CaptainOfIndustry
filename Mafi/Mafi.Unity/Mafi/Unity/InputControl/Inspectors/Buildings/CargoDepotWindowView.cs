// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CargoDepotWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CargoDepotWindowView : StaticEntityInspectorBase<CargoDepot>
  {
    private readonly CargoDepotInspector m_controller;

    protected override CargoDepot Entity => this.m_controller.SelectedEntity;

    public CargoDepotWindowView(CargoDepotInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<CargoDepotInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelForShip__Title, new LocStrFormatted?((LocStrFormatted) Tr.FuelForShip__Tooltip));
      BufferViewTwoSliders parent = this.Builder.NewBufferWithTwoSliders((IUiElement) itemContainer, (Action<float>) (x => this.m_controller.ScheduleInputCmd<CargoDepotSetFuelSliderStepCmd>(new CargoDepotSetFuelSliderStepCmd(this.Entity.Id, new int?((int) x), new int?()))), (Action<float>) (x => this.m_controller.ScheduleInputCmd<CargoDepotSetFuelSliderStepCmd>(new CargoDepotSetFuelSliderStepCmd(this.Entity.Id, new int?(), new int?((int) x)))), TrCore.StoredProduct__KeepFull.TranslatedString, TrCore.StoredProduct__KeepEmpty.TranslatedString, 10).AppendTo<BufferViewTwoSliders>(itemContainer, new float?(this.Style.BufferView.HeightWithSlider));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FuelBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(((BufferView) parent).UpdateState));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.FuelBuffer.ImportUntilPercent.DivAsFix32(LogisticsBuffer.SingleStep).ToIntRounded())).Observe<int>((Func<int>) (() => this.Entity.FuelBuffer.ExportFromPercent.DivAsFix32(LogisticsBuffer.SingleStep).ToIntRounded())).Do(new Action<int, int>(parent.UpdateSliders));
      CustomPriorityPanel customPriorityPanel1 = CustomPriorityPanel.NewForShipFuelImport((IUiElement) parent, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel1.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) parent, customPriorityPanel1.GetSize(), Offset.Right((float) (-(double) customPriorityPanel1.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel1.Updater);
      CustomPriorityPanel customPriorityPanel2 = CustomPriorityPanel.NewForShipFuelExport((IUiElement) parent, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel2.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) parent, customPriorityPanel2.GetSize(), Offset.Right((float) (-(double) customPriorityPanel2.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel2.Updater);
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
