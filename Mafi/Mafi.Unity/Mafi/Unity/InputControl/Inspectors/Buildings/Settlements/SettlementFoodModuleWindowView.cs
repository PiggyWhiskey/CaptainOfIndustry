// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.SettlementFoodModuleWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings.Settlements
{
  internal class SettlementFoodModuleWindowView : StaticEntityInspectorBase<SettlementFoodModule>
  {
    private readonly SettlementFoodModuleInspector m_controller;
    private readonly DependencyResolver m_resolver;
    private StackContainer m_inputBuffersContainer;
    private ProductBufferViewWithPicker.Cache m_inputBuffersCache;

    protected override SettlementFoodModule Entity => this.m_controller.SelectedEntity;

    public SettlementFoodModuleWindowView(
      SettlementFoodModuleInspector controller,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_resolver = resolver;
      this.m_controller = controller.CheckNotNull<SettlementFoodModuleInspector>();
      this.SetWindowOffsetGroup(ItemDetailWindowView.WindowOffsetGroup.LargeScreen);
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      SettlementWindow settlementWindow = this.m_resolver.Instantiate<SettlementWindow>();
      settlementWindow.SetSettlementProvider((Func<Settlement>) (() => this.Entity.Settlement));
      settlementWindow.BuildUi(this.Builder, (IUiElement) this);
      this.AttachSidePanel((IWindow) settlementWindow);
      settlementWindow.Show();
      this.OnShowStart += (Action) (() => settlementWindow.OpenFoodTab());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_inputBuffersCache = new ProductBufferViewWithPicker.Cache(this.Builder, this.m_controller.Context.UnlockedProtosDbForUi, (ItemDetailWindowView) this, new Action<ProductProto, int>(onProductSet), new Action<int>(onProductRemove), (Func<IEntityWithMultipleProductsToAssign>) (() => (IEntityWithMultipleProductsToAssign) this.Entity));
      this.AddUpdater(this.m_inputBuffersCache.Updater);
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, "Inputs");
      this.m_inputBuffersContainer = this.Builder.NewStackContainer("Buffers").SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer, new float?(0.0f));
      updaterBuilder.Observe<SettlementFoodModule.State>((Func<SettlementFoodModule.State>) (() => this.Entity.CurrentState)).Do((Action<SettlementFoodModule.State>) (state =>
      {
        switch (state)
        {
          case SettlementFoodModule.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case SettlementFoodModule.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case SettlementFoodModule.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case SettlementFoodModule.State.MissingInput:
            statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Critical);
            break;
          case SettlementFoodModule.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
        }
      }));
      updaterBuilder.Observe<Option<ProductBuffer>>((Func<IIndexable<Option<ProductBuffer>>>) (() => this.Entity.BuffersPerSlot), (ICollectionComparator<Option<ProductBuffer>, IIndexable<Option<ProductBuffer>>>) CompareFixedOrder<Option<ProductBuffer>>.Instance).Do((Action<Lyst<Option<ProductBuffer>>>) (buffers =>
      {
        this.m_inputBuffersContainer.StartBatchOperation();
        this.m_inputBuffersContainer.ClearAll();
        this.m_inputBuffersCache.ReturnAll();
        int slot = 0;
        foreach (Option<ProductBuffer> buffer in buffers)
        {
          ProductBufferViewWithPicker view = this.m_inputBuffersCache.GetView();
          view.Show<ProductBufferViewWithPicker>();
          view.AppendTo<ProductBufferViewWithPicker>(this.m_inputBuffersContainer, new float?(view.RequiredHeight));
          view.SetSlot(slot);
          ++slot;
        }
        this.m_inputBuffersContainer.FinishBatchOperation();
      }));
      this.AddUpdater(updaterBuilder.Build());

      void onProductSet(ProductProto product, int slot)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<AssignProductToSlotCmd>(new AssignProductToSlotCmd((IEntityWithMultipleProductsToAssign) this.Entity, (Option<ProductProto>) product, slot));
      }

      void onProductRemove(int slot)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<AssignProductToSlotCmd>(new AssignProductToSlotCmd((IEntityWithMultipleProductsToAssign) this.Entity, Option<ProductProto>.None, slot));
      }
    }
  }
}
