// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.StorageWindowView`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class StorageWindowView<T> : StaticEntityInspectorBase<T> where T : Storage
  {
    private readonly StorageInspectorGeneric<T> m_controller;
    private readonly ProtoPicker<ProductProto> m_protoPicker;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly VehiclesAssignerView<TruckProto> m_vehiclesAssigner;
    private readonly UiUnLockerForTech m_assignedVehiclesUiUnlocker;
    private StorageAlertingPanel<Storage> m_alertPanel;

    protected override T Entity => this.m_controller.SelectedEntity;

    public StorageWindowView(
      StorageInspectorGeneric<T> controller,
      UnlockedProtosDb unlockedProtosDb,
      VehiclesAssignerView<TruckProto> vehiclesAssigner)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_controller = controller;
      this.m_vehiclesAssigner = vehiclesAssigner;
      this.m_assignedVehiclesUiUnlocker = new UiUnLockerForTech(controller.Context);
      this.m_protoPicker = new ProtoPicker<ProductProto>(new Action<ProductProto>(this.setProductToStore));
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.SetWidth(480f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.StoredProduct__Title, new LocStrFormatted?((LocStrFormatted) Tr.StoredProduct__Tooltip));
      BufferViewTwoSliders buffer = this.Builder.NewBufferWithTwoSliders((IUiElement) itemContainer, (Action<float>) (val => this.m_controller.SetImportSlider((int) val)), (Action<float>) (val => this.m_controller.SetExportSlider((int) val)), TrCore.StoredProduct__KeepFull.TranslatedString, TrCore.StoredProduct__KeepEmpty.TranslatedString, 10, new Action(this.m_controller.RemoveStoredProduct), new Action(this.plusBtnClicked)).AppendTo<BufferViewTwoSliders>(itemContainer, new float?(this.Style.BufferView.HeightWithSlider));
      buffer.SetTextToShowWhenEmpty(Tr.StoredProduct__NothingStored.TranslatedString);
      this.SetupProductPickerWithBuffer(this.m_protoPicker, (BufferView) buffer);
      if (typeof (T).IsAssignableTo(typeof (NuclearWasteStorage)))
      {
        Txt outTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle, new LocStrFormatted?((LocStrFormatted) Tr.RetiredWaste__Tooltip));
        SwitchBtn switchBtn = this.Builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.OutputThisProductOnly).AddTooltip((LocStrFormatted) Tr.OutputThisProductOnly__Tooltip).SetOnToggleAction((Action<bool>) (_ => this.m_controller.Context.InputScheduler.ScheduleInputCmd<NuclearWasteStorageToggleOutputPortCmd>(new NuclearWasteStorageToggleOutputPortCmd((Storage) this.Entity))));
        switchBtn.PutToRightOf<SwitchBtn>((IUiElement) outTitle, switchBtn.GetWidth(), Offset.Right(10f));
        updaterBuilder.Observe<bool?>((Func<bool?>) (() => !(this.Entity is NuclearWasteStorage entity1) ? new bool?() : new bool?(entity1.DoNotSendRetiredWasteToOutputPort))).Do((Action<bool?>) (doNotSendRetiredWasteToOutputPort =>
        {
          SwitchBtn switchBtn1 = switchBtn;
          bool? nullable = doNotSendRetiredWasteToOutputPort;
          bool flag = false;
          int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
          switchBtn1.SetIsOn(num != 0);
        }));
        BufferView outBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
        Txt text = this.Builder.NewTxt("Info").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.MiddleLeft).PutToLeftBottomOf<Txt>((IUiElement) outBuffer, new Vector2(200f, 25f), Offset.Left(100f));
        updaterBuilder.Observe<ProductQuantity?>((Func<ProductQuantity?>) (() =>
        {
          ProductQuantity? nullable;
          if (!(this.Entity is NuclearWasteStorage entity3))
          {
            nullable = new ProductQuantity?();
          }
          else
          {
            IProductBufferReadOnly valueOrNull = entity3.OutputBuffer.ValueOrNull;
            nullable = valueOrNull != null ? new ProductQuantity?(valueOrNull.ProductQuantity()) : new ProductQuantity?();
          }
          return nullable ?? new ProductQuantity?();
        })).Observe<Quantity>((Func<Quantity>) (() => (this.Entity is NuclearWasteStorage entity4 ? entity4.OutputBuffer.ValueOrNull?.Capacity : new Quantity?()) ?? Quantity.Zero)).Do((Action<ProductQuantity?, Quantity>) ((pq, capacity) =>
        {
          itemContainer.SetItemVisibility((IUiElement) outBuffer, pq.HasValue);
          itemContainer.SetItemVisibility((IUiElement) outTitle, pq.HasValue);
          if (!pq.HasValue)
            return;
          outBuffer.UpdateState(pq.Value.Product, capacity, pq.Value.Quantity);
        }));
        updaterBuilder.Observe<int?>((Func<int?>) (() => !(this.Entity is NuclearWasteStorage entity5) ? new int?() : new int?(entity5.YearsUntilFirstWasteGetsRetired()))).Do((Action<int?>) (years =>
        {
          Txt element = text;
          int? nullable = years;
          int num1 = 0;
          int num2 = nullable.GetValueOrDefault() > num1 & nullable.HasValue ? 1 : 0;
          element.SetVisibility<Txt>(num2 != 0);
          if (!years.HasValue)
            return;
          text.SetText(Tr.RetiredWaste__NextDisposal.Format(TrCore.NumberOfYears.Format(years.Value)));
        }));
      }
      CustomPriorityPanel customPriorityPanel1 = CustomPriorityPanel.NewForStorageImport((IUiElement) buffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel1.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) buffer, customPriorityPanel1.GetSize(), Offset.Right((float) (-(double) customPriorityPanel1.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel1.Updater);
      CustomPriorityPanel customPriorityPanel2 = CustomPriorityPanel.NewForStorageExport((IUiElement) buffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel2.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) buffer, customPriorityPanel2.GetSize(), Offset.Right((float) (-(double) customPriorityPanel2.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel2.Updater);
      this.m_alertPanel = new StorageAlertingPanel<Storage>((IUiElement) buffer, this.Builder, this.m_controller.InputScheduler, (Func<Storage>) (() => (Storage) this.Entity));
      this.m_alertPanel.PutToRightBottomOf<StorageAlertingPanel<Storage>>((IUiElement) buffer, this.m_alertPanel.GetSize(), Offset.Right((float) (-(double) this.m_alertPanel.GetWidth() + 1.0)));
      this.AddUpdater(this.m_alertPanel.Updater);
      ButtonWithTextAndIcon clearingBtn = new ButtonWithTextAndIcon(this.Builder, this.Builder.Style.Global.UpointsBtn, "QuickRemove");
      clearingBtn.TextWithIcon.SetSuffixIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetIcon("Assets/Unity/UserInterface/General/Trash128.png");
      clearingBtn.PutToRightBottomOf<ButtonWithTextAndIcon>((IUiElement) buffer, 0.Vector2());
      clearingBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<StorageQuickRemoveProductCmd>(new StorageQuickRemoveProductCmd((Storage) this.Entity)))).AddToolTip(Tr.QuickRemove__Action);
      bool canAfford;
      updaterBuilder.Observe<KeyValuePair<Upoints, bool>>((Func<KeyValuePair<Upoints, bool>>) (() => Make.Kvp<Upoints, bool>(this.Entity.GetQuickRemoveCost(out canAfford), canAfford))).Do((Action<KeyValuePair<Upoints, bool>>) (result =>
      {
        Upoints key = result.Key;
        if (key.IsPositive)
        {
          clearingBtn.TextWithIcon.SetSuffixText("  |  " + key.Value.ToStringRounded(1));
          clearingBtn.UpdateWidth();
          clearingBtn.PutToRightTopOf<ButtonWithTextAndIcon>((IUiElement) buffer, new Vector2(clearingBtn.GetWidth(), 25f), Offset.Right(this.m_alertPanel.GetToggleBtnWidth() + 20f) + Offset.Top(-30f));
        }
        clearingBtn.SetVisibility<ButtonWithTextAndIcon>(key.IsPositive);
        clearingBtn.SetEnabled(result.Value);
      }));
      this.AddBuildingsAssignerForExport(this.m_controller.Context, new Action(((EntityInspector<T, StorageWindowView<T>>) this.m_controller).EditInputBuildingsClicked), (Func<IEntityAssignedAsOutput>) (() => (IEntityAssignedAsOutput) this.Entity), (LocStrFormatted) Tr.AssignedForLogistics__ExportTooltipGeneral);
      this.AddBuildingsAssignerForImport(this.m_controller.Context, new Action(((EntityInspector<T, StorageWindowView<T>>) this.m_controller).EditOutputBuildingsClicked), (Func<IEntityAssignedAsInput>) (() => (IEntityAssignedAsInput) this.Entity), (LocStrFormatted) Tr.AssignedForLogistics__ImportTooltipGeneral, updaterBuilder);
      Txt parent = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.AssignedTrucks__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTrucks__Building_Tooltip));
      this.m_vehiclesAssigner.Build((IUiElement) this.ItemsContainer, this.Builder).AppendTo<VehiclesAssignerView<TruckProto>>(this.ItemsContainer, new float?(0.0f));
      this.AddUpdater(this.m_vehiclesAssigner.Updater);
      SwitchBtn btn = this.Builder.NewSwitchBtn().SetText(TrCore.AssignedTrucksEnforce__Title.TranslatedString).AddTooltip(TrCore.AssignedTrucksEnforce__Tooltip.TranslatedString).SetOnToggleAction((Action<bool>) (isOn => this.m_controller.Context.InputScheduler.ScheduleInputCmd<ToggleEnforceAssignedVehiclesForEntityCmd>(new ToggleEnforceAssignedVehiclesForEntityCmd((IEntityEnforcingAssignedVehicles) this.Entity, isOn))));
      btn.PutToRightOf<SwitchBtn>((IUiElement) parent, btn.GetWidth(), Offset.Right(10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.AreOnlyAssignedVehiclesAllowed)).Do((Action<bool>) (enforceOn => btn.SetIsOn(enforceOn)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.VehiclesTotal() > 0)).Do((Action<bool>) (hasAssignedVehicles => btn.SetVisibility<SwitchBtn>(hasAssignedVehicles)));
      this.m_assignedVehiclesUiUnlocker.SetupVisibilityHook(IdsCore.Technology.CustomRoutes, this.ItemsContainer, (IUiElement) parent, (IUiElement) this.m_vehiclesAssigner);
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => this.Entity.StoredProduct)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CurrentQuantity)).Do(new Action<Option<ProductProto>, Quantity, Quantity>(((BufferView) buffer).UpdateState));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.ImportUntilPercent)).Observe<Percent>((Func<Percent>) (() => this.Entity.ExportFromPercent)).Do((Action<Percent, Percent>) ((importUntil, exportFrom) =>
      {
        BufferViewTwoSliders bufferViewTwoSliders = buffer;
        Fix32 fix32 = importUntil.DivAsFix32(LogisticsBuffer.SingleStep);
        int intRounded1 = fix32.ToIntRounded();
        fix32 = exportFrom.DivAsFix32(LogisticsBuffer.SingleStep);
        int intRounded2 = fix32.ToIntRounded();
        bufferViewTwoSliders.UpdateSliders(intRounded1, intRounded2);
      }));
      Btn trashButton = buffer.TrashBtn.ValueOrNull;
      Tooltip trashButtonToolTip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) trashButton);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CleaningInProgress)).Observe<bool>((Func<bool>) (() => this.Entity.CanMoveSlider())).Do((Action<bool, bool>) ((isCleaning, canMoveSlider) =>
      {
        if (isCleaning)
        {
          trashButtonToolTip.SetText((LocStrFormatted) Tr.StoredProduct__ClearActive_Tooltip);
          trashButton.SetButtonStyle(this.Builder.Style.Global.DangerBtnActive);
        }
        else
        {
          trashButtonToolTip.SetText((LocStrFormatted) Tr.StoredProduct__Clear_Tooltip);
          trashButton.SetButtonStyle(this.Builder.Style.Global.GeneralBtnToToggle);
        }
        buffer.SetSlidersEnabled(!isCleaning | canMoveSlider);
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.OnHide += (Action) (() =>
      {
        this.m_alertPanel.HidePanel();
        this.m_protoPicker.Hide();
      });
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos(this.m_unlockedProtosDb.FilterUnlocked<ProductProto>((IEnumerable<ProductProto>) this.m_controller.SelectedEntity.Prototype.StorableProducts));
        this.m_protoPicker.Show();
      }
    }

    private void setProductToStore(ProductProto product)
    {
      this.m_controller.SetProductToStore(product);
      this.m_protoPicker.Hide();
    }
  }
}
