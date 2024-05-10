// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.QuickTradeView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class QuickTradeView : IUiElement, IDynamicSizeElement
  {
    public const int TRADE_ROW_WIDTH = 280;
    private readonly UiBuilder m_builder;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IAvailableProductsProvider m_availableProductsProvider;
    public readonly IUiUpdater Updater;
    private readonly GridContainer m_tradesContainer;
    private readonly Panel m_container;
    private Option<IInputCommand> m_tradeCmdPending;
    private Option<QuickTradeProvider> m_tradePending;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public QuickTradeView(
      IUiElement parent,
      UiBuilder builder,
      IInputScheduler inputScheduler,
      IAvailableProductsProvider availableProductsProvider,
      float widthAvailable,
      Func<IIndexable<QuickTradeProvider>> tradesProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_inputScheduler = inputScheduler;
      this.m_availableProductsProvider = availableProductsProvider;
      this.m_container = builder.NewPanel(nameof (QuickTradeView), parent);
      Txt title = builder.CreateSectionTitle((IUiElement) this.m_container, (LocStrFormatted) Tr.TradeOffers, new LocStrFormatted?((LocStrFormatted) Tr.TradeOffers__Tooltip));
      title.PutToTopOf<Txt>((IUiElement) this.m_container, builder.Style.Panel.LineHeight, new Offset(0.0f, builder.Style.Panel.SectionTitleTopPadding, builder.Style.Panel.Padding, 0.0f));
      int columnsCount = (widthAvailable / 285f).FloorToInt();
      float x1 = (widthAvailable - (float) ((columnsCount - 1) * 5)) / (float) columnsCount;
      this.m_tradesContainer = builder.NewGridContainer("Trades container").SetCellSize(new Vector2(x1, 80f)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).PutToTopOf<GridContainer>((IUiElement) this.m_container, 0.0f, Offset.Top(title.GetHeight() + builder.Style.Panel.SectionTitleTopPadding));
      Panel tradeDoneMsgContainer = builder.NewPanel("TradeDoneContainer").SetBackground(new ColorRgba(3092271)).PutToBottomOf<Panel>((IUiElement) this.m_container, 25f, Offset.Top(1f)).Hide<Panel>();
      Txt tradeDoneMsg = builder.NewTxt("TradeDone").SetText("").SetTextStyle(this.m_builder.Style.Global.TextMedium.Extend(new ColorRgba?(this.m_builder.Style.Global.GreenForDark))).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) tradeDoneMsgContainer);
      this.m_tradesContainer.SizeChanged += (Action<IUiElement>) (x =>
      {
        if (x.GetHeight().IsNearZero())
          this.m_container.SetHeight<Panel>(0.0f);
        else
          this.m_container.SetHeight<Panel>((float) ((double) x.GetHeight() + (double) title.GetHeight() + (double) tradeDoneMsg.GetHeight() + 5.0));
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      });
      ViewsCacheTracked<QuickTradeProvider, QuickTradeView.TradePairView> viewsCache = new ViewsCacheTracked<QuickTradeProvider, QuickTradeView.TradePairView>((Func<QuickTradeProvider, QuickTradeView.TradePairView>) (provider =>
      {
        QuickTradeView.TradePairView tradePairView = new QuickTradeView.TradePairView((IUiElement) this.m_tradesContainer, this);
        tradePairView.SetTradeProvider(provider);
        return tradePairView;
      }));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<QuickTradeProvider>(tradesProvider, (ICollectionComparator<QuickTradeProvider, IIndexable<QuickTradeProvider>>) CompareFixedOrder<QuickTradeProvider>.Instance).Do((Action<Lyst<QuickTradeProvider>>) (quickTradeProviders =>
      {
        this.m_tradesContainer.ClearAll();
        viewsCache.ReturnAll();
        foreach (QuickTradeProvider quickTradeProvider in quickTradeProviders)
          this.m_tradesContainer.Append((IUiElement) viewsCache.GetView(quickTradeProvider));
        title.SetVisibility<Txt>(quickTradeProviders.IsNotEmpty);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_tradeCmdPending.HasValue && this.m_tradeCmdPending.Value.ResultSet)).Observe<string>((Func<string>) (() =>
      {
        if (this.m_tradeCmdPending.HasValue && this.m_tradeCmdPending.Value.ResultSet && this.m_tradeCmdPending.Value.HasError)
          return this.m_tradeCmdPending.Value.ErrorMessage;
        return this.m_tradePending.ValueOrNull?.MessageOnDelivery.Value;
      })).Do((Action<bool, string>) ((hasResult, msg) =>
      {
        if (hasResult)
          tradeDoneMsg.SetText(msg);
        tradeDoneMsgContainer.SetVisibility<Panel>(hasResult);
      }));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(viewsCache.Updater);
    }

    public void OnHide()
    {
      this.m_tradeCmdPending = (Option<IInputCommand>) Option.None;
      this.m_tradePending = (Option<QuickTradeProvider>) Option.None;
    }

    private void doTrade(QuickTradeProvider provider)
    {
      QuickTradeCmd cmd = new QuickTradeCmd(provider.Prototype.Id);
      this.m_tradeCmdPending = (Option<IInputCommand>) (IInputCommand) cmd;
      this.m_tradePending = (Option<QuickTradeProvider>) provider;
      this.m_inputScheduler.ScheduleInputCmd<QuickTradeCmd>(cmd);
    }

    private class TradePairView : IUiElementWithUpdater, IUiElement
    {
      private readonly Panel m_panel;

      public GameObject GameObject => this.m_panel.GameObject;

      public RectTransform RectTransform => this.m_panel.RectTransform;

      public IUiUpdater Updater { get; private set; }

      private QuickTradeProvider TradeProvider { get; set; }

      public TradePairView(IUiElement parent, QuickTradeView tradeView)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        QuickTradeView.TradePairView tradePairView = this;
        UiBuilder builder = tradeView.m_builder;
        AudioSource sharedAudio = builder.AudioDb.GetSharedAudio(builder.Audio.MoneyAction);
        this.m_panel = builder.NewPanel("Product", parent).SetBackground(new ColorRgba(3092271));
        PricePanel productToPayWithView = new PricePanel(builder, builder.Style.PricePanel.QuickTradeToPayStyle, Option.Create<IAvailableProductsProvider>(tradeView.m_availableProductsProvider));
        productToPayWithView.PutToLeftTopOf<PricePanel>((IUiElement) this.m_panel, new Vector2(90f, productToPayWithView.PreferredHeight));
        IconContainer priceIncreaseIcon = builder.NewIconContainer("Arrow", (IUiElement) productToPayWithView).SetIcon("Assets/Unity/UserInterface/General/UpArrow128.png").SetColor((ColorRgba) 13327198).PutToLeftMiddleOf<IconContainer>((IUiElement) productToPayWithView, new Vector2(36f, 36f)).Hide<IconContainer>();
        PricePanel productToBuyView = new PricePanel(builder, builder.Style.PricePanel.QuickTradeToReceiveStyle, Option.Create<IAvailableProductsProvider>(tradeView.m_availableProductsProvider));
        productToBuyView.PutToRightTopOf<PricePanel>((IUiElement) this.m_panel, new Vector2(100f, productToBuyView.PreferredHeight));
        builder.NewIconContainer("arrow", (IUiElement) this.m_panel).SetIcon(builder.Style.Icons.Transform, builder.Style.Global.UpointsBtn.BackgroundClr.Value).PutToCenterMiddleOf<IconContainer>((IUiElement) this.m_panel, new Vector2(30f, 30f), Offset.Bottom(20f));
        Btn btn = builder.NewBtnUpoints("Trade").SetText((LocStrFormatted) Tr.Trade__Action).OnClick((Action) (() => tradeView.doTrade(tradePairView.TradeProvider)), sharedAudio).PlayErrorSoundWhenDisabled();
        Tooltip btnTooltip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) btn);
        btn.PutToCenterBottomOf<Btn>((IUiElement) this.m_panel, btn.GetOptimalSize(), Offset.Bottom(10f));
        CostButton tradeCostBtn = new CostButton(builder, Tr.Trade__Action.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg");
        Tooltip costBtnTooltip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) tradeCostBtn);
        tradeCostBtn.SetButtonStyle(builder.Style.Global.UpointsBtn).OnClick((Action) (() => tradeView.doTrade(tradePairView.TradeProvider)), sharedAudio).PlayErrorSoundWhenDisabled().PutToCenterBottomOf<Btn>((IUiElement) this.m_panel, btn.GetOptimalSize(), Offset.Bottom(10f));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => tradePairView.TradeProvider.GetProductToBuy())).Do((Action<ProductQuantity>) (p => productToBuyView.SetPrice(new AssetValue(p))));
        updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => tradePairView.TradeProvider.GetProductToPayWith())).Do((Action<ProductQuantity>) (p => productToPayWithView.SetPrice(new AssetValue(p))));
        LocStrFormatted error;
        updaterBuilder.Observe<Upoints>((Func<Upoints>) (() => tradePairView.TradeProvider.UpointsCost)).Observe<bool>((Func<bool>) (() => tradePairView.TradeProvider.IsSoldOut)).Observe<KeyValuePair<bool, LocStrFormatted>>((Func<KeyValuePair<bool, LocStrFormatted>>) (() => Make.Kvp<bool, LocStrFormatted>(tradePairView.TradeProvider.CanAfford(out error), error))).Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => tradePairView.TradeProvider.DescriptionOfTrade)).Do((Action<Upoints, bool, KeyValuePair<bool, LocStrFormatted>, LocStrFormatted>) ((upointsPerTrade, isSoldOut, canAffordResult, tradeDesc) =>
        {
          bool visibility = false;
          if (isSoldOut)
            btn.SetText((LocStrFormatted) Tr.Trade__SoldOut);
          else if (upointsPerTrade.IsPositive)
          {
            tradeCostBtn.SetCost(upointsPerTrade.Value.ToStringRounded(1));
            visibility = true;
          }
          else
            btn.SetText((LocStrFormatted) Tr.Trade__Action);
          btn.SetVisibility<Btn>(!visibility);
          tradeCostBtn.SetVisibility<CostButton>(visibility);
          bool key = canAffordResult.Key;
          LocStrFormatted text = key ? tradeDesc : canAffordResult.Value;
          btnTooltip.SetText(text);
          costBtnTooltip.SetText(text);
          btn.SetWidth<Btn>(btn.GetOptimalWidth());
          tradeCostBtn.SetEnabled(key);
          btn.SetEnabled(key);
        }));
        updaterBuilder.Observe<bool>((Func<bool>) (() => tradePairView.TradeProvider.HasPriceIncreased)).Do((Action<bool>) (priceIncreased => priceIncreaseIcon.SetVisibility<IconContainer>(priceIncreased)));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(productToBuyView.CreateUpdater());
        this.Updater.AddChildUpdater(productToPayWithView.CreateUpdater());
      }

      public void SetTradeProvider(QuickTradeProvider tradeProvider)
      {
        this.TradeProvider = tradeProvider;
      }
    }
  }
}
