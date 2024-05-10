// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.OreSortingPlantWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class OreSortingPlantWindowView : StaticEntityInspectorBase<OreSortingPlant>
  {
    private readonly OreSortingPlantInspector m_controller;
    private ViewsCacheHomogeneous<OutputSortedProductBufferView> m_outputBuffersCache;

    protected override OreSortingPlant Entity => this.m_controller.SelectedEntity;

    public OreSortingPlantWindowView(OreSortingPlantInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<OreSortingPlantInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      int width = 800;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_outputBuffersCache = new ViewsCacheHomogeneous<OutputSortedProductBufferView>((Func<OutputSortedProductBufferView>) (() => new OutputSortedProductBufferView(this.Builder, this.m_controller.Context.InputScheduler)));
      this.AddUpdater(this.m_outputBuffersCache.Updater);
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      string str = this.m_controller.Context.ProtosDb.First<MineTowerProto>().ValueOrNull?.Strings.Name.TranslatedString ?? "";
      this.AddSectionTitle(itemContainer, (LocStrFormatted) TrCore.OreSorter_AllowedProducts__Title, new LocStrFormatted?(TrCore.OreSorter_AllowedProducts__Tooltip.Format(str)));
      ProtosFilterEditor<ProductProto> filterView = new ProtosFilterEditor<ProductProto>(this.Builder, (IWindowWithInnerWindowsSupport) this, this.ItemsContainer, (Action<ProductProto>) (product => this.m_controller.Context.InputScheduler.ScheduleInputCmd<RemoveProductToSortCmd>(new RemoveProductToSortCmd(this.Entity, product))), (Action<ProductProto>) (product => this.m_controller.Context.InputScheduler.ScheduleInputCmd<AddProductToSortCmd>(new AddProductToSortCmd(this.Entity, product))), (Func<IEnumerable<ProductProto>>) (() => this.m_controller.Context.UnlockedProtosDbForUi.FilterUnlocked<ProductProto>(this.Entity.AllSupportedProducts.AsEnumerable())), (Func<IEnumerable<ProductProto>>) (() => this.Entity.AllowedProducts), 8, false);
      filterView.SetTextToShowWhenEmpty(TrCore.OreSorter_SelectProducts.TranslatedString);
      this.SetWidth(filterView.GetRequiredWidth() + 30f);
      int columnsCount = 2;
      int x = (width - 5 * (columnsCount - 1)) / columnsCount;
      Txt inputTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) TrCore.OreSorter_InputTitle);
      BufferWithMultipleProductsView inputBuffer = new BufferWithMultipleProductsView((IUiElement) itemContainer, this.Builder);
      inputBuffer.AppendTo<BufferWithMultipleProductsView>(itemContainer, new float?(inputBuffer.GetHeight()));
      SwitchBtn singleHaulToggle = this.Builder.NewSwitchBtn().SetText((LocStrFormatted) TrCore.OreSorter_NoSingleLoad__Toggle).AddTooltip((LocStrFormatted) TrCore.OreSorter_NoSingleLoad__Tooltip).SetOnToggleAction((Action<bool>) (isEnabled => this.m_controller.Context.InputScheduler.ScheduleInputCmd<SortingPlantNoSingleProductCmd>(new SortingPlantNoSingleProductCmd(this.Entity, isEnabled))));
      singleHaulToggle.PutToRightOf<SwitchBtn>((IUiElement) inputTitle, singleHaulToggle.GetWidth(), Offset.Right(10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.DoNotAcceptSingleProduct)).Do((Action<bool>) (isOn => singleHaulToggle.SetIsOn(isOn)));
      Lyst<ProductQuantity> productsCache = new Lyst<ProductQuantity>();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        productsCache.Clear();
        this.Entity.GetMixedInputProducts(productsCache);
        return (IIndexable<ProductQuantity>) productsCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Do((Action<Quantity, Lyst<ProductQuantity>>) ((capacity, cargo) => inputBuffer.SetProducts(cargo, capacity, false)));
      Txt outputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      GridContainer outputBuffersContainer = this.Builder.NewGridContainer("OutputGrid").SetCellSize(new Vector2((float) x, OutputSortedProductBufferView.RequiredHeight)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).AppendTo<GridContainer>(itemContainer);
      this.AddBuildingsAssignerForImport(this.m_controller.Context, new Action(((EntityInspector<OreSortingPlant, OreSortingPlantWindowView>) this.m_controller).EditOutputBuildingsClicked), (Func<IEntityAssignedAsInput>) (() => (IEntityAssignedAsInput) this.Entity), LocStrFormatted.Empty, updaterBuilder);
      updaterBuilder.Observe<ProductProto>((Func<IEnumerable<ProductProto>>) (() => this.Entity.AllowedProducts), (ICollectionComparator<ProductProto, IEnumerable<ProductProto>>) CompareFixedOrder<ProductProto>.Instance).Do((Action<Lyst<ProductProto>>) (allowed =>
      {
        filterView.UpdateFilteredProtos(allowed);
        itemContainer.SetItemVisibility((IUiElement) outputsTitle, allowed.IsNotEmpty);
        itemContainer.SetItemVisibility((IUiElement) inputTitle, allowed.IsNotEmpty);
        itemContainer.SetItemVisibility((IUiElement) inputBuffer, allowed.IsNotEmpty);
        bool flag = allowed.Count >= 8;
        filterView.SetBtnEnabled(!flag, flag ? TrCore.OreSorter_LimitReached.Format(8.ToString()) : LocStrFormatted.Empty);
      }));
      updaterBuilder.Observe<OreSortingPlant.State>((Func<OreSortingPlant.State>) (() => this.Entity.CurrentState)).Observe<bool>((Func<bool>) (() => this.Entity.CanAllProductsBeAcceptedForUi())).Do((Action<OreSortingPlant.State, bool>) ((state, canAllProductsBeAccepted) =>
      {
        switch (state)
        {
          case OreSortingPlant.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case OreSortingPlant.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case OreSortingPlant.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case OreSortingPlant.State.MissingInput:
            if (!canAllProductsBeAccepted)
            {
              statusInfo.SetStatus(Tr.EntityStatus__PartiallyStuck, StatusPanel.State.Warning);
              break;
            }
            statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Warning);
            break;
          case OreSortingPlant.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case OreSortingPlant.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
        }
      }));
      updaterBuilder.Observe<IProductBuffer>((Func<IIndexable<IProductBuffer>>) (() => this.Entity.OutputBuffers), (ICollectionComparator<IProductBuffer, IIndexable<IProductBuffer>>) CompareFixedOrder<IProductBuffer>.Instance).Do((Action<Lyst<IProductBuffer>>) (buffers =>
      {
        outputBuffersContainer.StartBatchOperation();
        outputBuffersContainer.ClearAll();
        this.m_outputBuffersCache.ReturnAll();
        foreach (IProductBuffer buffer in buffers)
        {
          OutputSortedProductBufferView view = this.m_outputBuffersCache.GetView();
          view.Show<OutputSortedProductBufferView>();
          view.AppendTo<OutputSortedProductBufferView>(outputBuffersContainer);
          view.SetOutputBuffer(buffer, this.Entity);
        }
        outputBuffersContainer.FinishBatchOperation();
        itemContainer.UpdateItemHeight((IUiElement) outputBuffersContainer, outputBuffersContainer.GetRequiredHeight());
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth((float) width);
    }
  }
}
