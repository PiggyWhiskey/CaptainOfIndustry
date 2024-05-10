// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.SettlementServiceModuleWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class SettlementServiceModuleWindowView : 
    StaticEntityInspectorBase<SettlementServiceModule>
  {
    private readonly DependencyResolver m_resolver;
    private readonly SettlementServiceModuleInspector m_controller;
    private ViewsCacheHomogeneous<SettlementServiceModuleWindowView.SettlementNeedView> m_viewsCache;
    private StackContainer m_inputsContainer;
    private StackContainer m_outputsContainer;
    private Txt m_outputsTitle;
    private Txt m_inputsTitle;

    protected override SettlementServiceModule Entity => this.m_controller.SelectedEntity;

    public SettlementServiceModuleWindowView(
      SettlementServiceModuleInspector controller,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_resolver = resolver;
      this.m_controller = controller.CheckNotNull<SettlementServiceModuleInspector>();
      this.SetWindowOffsetGroup(ItemDetailWindowView.WindowOffsetGroup.LargeScreen);
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.m_viewsCache = new ViewsCacheHomogeneous<SettlementServiceModuleWindowView.SettlementNeedView>((Func<SettlementServiceModuleWindowView.SettlementNeedView>) (() => new SettlementServiceModuleWindowView.SettlementNeedView(this.Builder)));
      SettlementWindow element = this.m_resolver.Instantiate<SettlementWindow>();
      element.SetSettlementProvider((Func<Settlement>) (() => this.Entity.Settlement.Value));
      element.BuildUi(this.Builder, (IUiElement) this);
      this.AttachSidePanel((IWindow) element);
      element.Show();
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.m_inputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Consumption);
      this.m_inputsContainer = this.Builder.NewStackContainer("Items container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).AppendTo<StackContainer>(this.ItemsContainer, new float?(0.0f));
      this.m_outputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Production);
      this.m_outputsContainer = this.Builder.NewStackContainer("Items container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).AppendTo<StackContainer>(this.ItemsContainer, new float?(0.0f));
      updaterBuilder.Observe<SettlementServiceModule>((Func<SettlementServiceModule>) (() => this.Entity)).Do(new Action<SettlementServiceModule>(this.rebuildBuffers));
      updaterBuilder.Observe<SettlementServiceModule.State>((Func<SettlementServiceModule.State>) (() => this.Entity.CurrentState)).Do((Action<SettlementServiceModule.State>) (state =>
      {
        switch (state)
        {
          case SettlementServiceModule.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case SettlementServiceModule.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case SettlementServiceModule.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case SettlementServiceModule.State.MissingInput:
            statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Critical);
            break;
          case SettlementServiceModule.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case SettlementServiceModule.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case SettlementServiceModule.State.FullOutput:
            statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddUpdater(this.m_viewsCache.Updater);
      this.AddUpdater(updaterBuilder.Build());
    }

    private void rebuildBuffers(SettlementServiceModule module)
    {
      this.m_inputsContainer.ClearAll();
      this.m_outputsContainer.ClearAll();
      this.m_viewsCache.ReturnAll();
      SettlementModuleProto prototype = module.Prototype;
      SettlementServiceModuleWindowView.SettlementNeedView view1 = this.m_viewsCache.GetView();
      view1.SetProduct(module, true);
      this.m_inputsContainer.Append((IUiElement) view1, new float?(view1.GetHeight()));
      this.ItemsContainer.SetItemVisibility((IUiElement) this.m_outputsTitle, prototype.OutputProduct.HasValue);
      if (!prototype.OutputProduct.HasValue)
        return;
      SettlementServiceModuleWindowView.SettlementNeedView view2 = this.m_viewsCache.GetView();
      view2.SetProduct(module, false);
      this.m_outputsContainer.Append((IUiElement) view2, new float?(view2.GetHeight()));
    }

    private class SettlementNeedView : IUiElementWithUpdater, IUiElement, IDynamicSizeElement
    {
      private readonly StackContainer m_container;
      private readonly BufferView m_buffer;
      private bool m_isInput;
      private SettlementServiceModule m_module;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IUiUpdater Updater { get; private set; }

      public event Action<IUiElement> SizeChanged;

      public SettlementNeedView(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewStackContainer("Product").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetBackground(builder.Style.Panel.ItemOverlay);
        this.m_container.SizeChanged += (Action<IUiElement>) (x =>
        {
          Action<IUiElement> sizeChanged = this.SizeChanged;
          if (sizeChanged == null)
            return;
          sizeChanged((IUiElement) this);
        });
        TextWithIcon statsNeeded = new TextWithIcon(builder);
        statsNeeded.SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").SetTextStyle(builder.Style.Panel.Text).AppendTo<TextWithIcon>(this.m_container, new Vector2?(new Vector2(0.0f, 25f)), ContainerPosition.LeftOrTop, Offset.Left(20f));
        this.m_buffer = builder.NewBufferView((IUiElement) this.m_container, isCompact: true).AppendTo<BufferView>(this.m_container, new float?(builder.Style.BufferView.CompactHeight));
        TextWithIcon statsLastMonth = new TextWithIcon(builder, 18).SetTextStyle(builder.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) this.m_buffer, new Vector2(200f, 25f), Offset.Left(100f));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<ProductQuantity?>((Func<ProductQuantity?>) (() => !this.m_isInput ? this.m_module.OutputProduct : new ProductQuantity?(this.m_module.InputProduct))).Observe<PartialQuantity>((Func<PartialQuantity>) (() => !this.m_isInput ? this.m_module.GetTotalOutputNeedPerMonth() : this.m_module.GetTotalInputNeedPerMonth())).Do((Action<ProductQuantity?, PartialQuantity>) ((product, total) =>
        {
          statsNeeded.SetIcon(product?.Product.Graphics.IconPath);
          statsNeeded.SetPrefixText(string.Format("{0}: {1}", (object) (this.m_isInput ? Tr.TotalSettlementNeed : Tr.TotalSettlementOutput), (object) total.ToStringRounded(1)));
        }));
        updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => !this.m_isInput ? this.m_module.TotalOutputLastMonth : this.m_module.TotalInputLastMonth)).Do((Action<Quantity>) (total => statsLastMonth.SetPrefixText(string.Format("{0}: {1} / 60", (object) (this.m_isInput ? Tr.ConsumedLastMonth : Tr.ProducedLastMonth), (object) total.Value))));
        updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => !this.m_isInput ? this.m_module.OutputProduct.GetValueOrDefault() : this.m_module.InputProduct)).Observe<Quantity>((Func<Quantity>) (() => !this.m_isInput ? this.m_module.OutputProductCapacity : this.m_module.InputProductCapacity)).Do((Action<ProductQuantity, Quantity>) ((pq, capacity) => this.m_buffer.UpdateState(pq.Product, capacity, pq.Quantity)));
        this.Updater = updaterBuilder.Build();
      }

      public void SetProduct(SettlementServiceModule module, bool isInput)
      {
        this.m_module = module;
        this.m_isInput = isInput;
        if (this.m_isInput)
          this.m_buffer.UseNeutralColor();
        else
          this.m_buffer.UseNegativeColor();
      }
    }
  }
}
