// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.FloatingPricePopup
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  /// <summary>
  /// Tiny popup that shows money value next to the cursor. Used for instance to show price of transport that is being
  /// build.
  /// </summary>
  public class FloatingPricePopup
  {
    private bool m_isTemporarilyHiddenInFreeLook;
    private Panel m_container;
    private readonly IAssetTransactionManager m_assetTransactionsManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly CameraController m_cameraController;
    private readonly EntityCostProvider m_costProvider;
    private readonly UiBuilder m_builder;
    private PricePanel m_pricePanel;
    private IUiUpdater m_updater;
    private Panel m_errorContainer;
    private Txt m_errorText;
    private Panel m_extraTextContainer;
    private Txt m_extraText;
    private LocStrFormatted m_currentErrorMessage;
    private string m_currentExtraText;
    private Panel m_priceBg;
    private bool m_isShown;

    public bool IsTemporarilyHidden { get; private set; }

    public FloatingPricePopup(
      IAssetTransactionManager assetTransactionsManager,
      IGameLoopEvents gameLoopEvents,
      CameraController cameraController,
      EntityCostProvider costProvider,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentExtraText = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetTransactionsManager = assetTransactionsManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_cameraController = cameraController;
      this.m_costProvider = costProvider;
      this.m_builder = builder;
    }

    private void renderUpdate(GameTime obj)
    {
      Panel container = this.m_container;
      if ((container != null ? (!container.IsVisible() ? 1 : 0) : 0) != 0)
        return;
      this.m_updater.RenderUpdate();
    }

    private void syncUpdate(GameTime gameTime)
    {
      Panel container = this.m_container;
      if ((container != null ? (!container.IsVisible() ? 1 : 0) : 0) != 0)
        return;
      this.m_updater.SyncUpdate();
    }

    private void buildUiIfNeeded()
    {
      if (this.m_container != null)
        return;
      UiStyle style = this.m_builder.Style;
      ColorRgba floatingPopupBg = style.Global.FloatingPopupBg;
      this.m_container = this.m_builder.NewPanel("FloatingMoneyPopup").PutToLeftBottomOf<Panel>((IUiElement) this.m_builder.MainCanvas, new Vector2(0.0f, 0.0f));
      this.m_errorContainer = this.m_builder.NewPanel("ErrorContainer").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgRedBorder), new ColorRgba?(style.Global.ErrorTooltipBg)).PutToCenterBottomOf<Panel>((IUiElement) this.m_container, new Vector2(0.0f, 30f), Offset.Bottom(-40f)).Hide<Panel>();
      this.m_builder.NewIconContainer("Warning").SetIcon("Assets/Unity/UserInterface/General/Warning128.png").PutToLeftTopOf<IconContainer>((IUiElement) this.m_errorContainer, 18.Vector2(), Offset.TopLeft(5f, 10f));
      this.m_errorText = this.m_builder.NewTitle("Error").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.m_builder.Style.Global.ErrorTooltipText).PutTo<Txt>((IUiElement) this.m_errorContainer, Offset.Left(38f));
      this.m_priceBg = this.m_builder.NewPanel("PriceBg").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgBlackBorder), new ColorRgba?(floatingPopupBg)).PutTo<Panel>((IUiElement) this.m_container);
      this.m_pricePanel = new PricePanel(this.m_builder, this.m_builder.Style.PricePanel.MenuPricePanelStyle, (Option<IAvailableProductsProvider>) (IAvailableProductsProvider) new ProductsAvailableInStorage(this.m_assetTransactionsManager));
      this.m_pricePanel.PutToLeftTopOf<PricePanel>((IUiElement) this.m_priceBg, new Vector2(0.0f, this.m_pricePanel.PreferredCompactHeight), Offset.Top(3f));
      this.m_extraTextContainer = this.m_builder.NewPanel("ExtraTextContainer").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgBlackBorder), new ColorRgba?(floatingPopupBg)).PutToCenterBottomOf<Panel>((IUiElement) this.m_container, new Vector2(0.0f, 30f), Offset.Bottom(-40f)).Hide<Panel>();
      Txt txt = this.m_builder.NewTitle("ExtraText").SetAlignment(TextAnchor.MiddleCenter);
      TextStyle title = this.m_builder.Style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_extraText = txt.SetTextStyle(textStyle).PutTo<Txt>((IUiElement) this.m_extraTextContainer);
      this.m_container.SetHeight<Panel>(this.m_pricePanel.PreferredCompactHeight - 5f);
      this.m_updater = this.m_pricePanel.CreateUpdater();
      this.Hide();
      this.m_cameraController.FreeLookModeChanged += new Action<bool>(this.cameraFreeLookChanged);
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<FloatingPricePopup>(this, new Action<GameTime>(this.renderUpdate));
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<FloatingPricePopup>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void SetErrorMessage(LocStrFormatted errorMessage)
    {
      this.buildUiIfNeeded();
      if (this.m_currentErrorMessage == errorMessage)
        return;
      this.m_currentErrorMessage = errorMessage;
      if (errorMessage.IsNotEmpty)
      {
        this.m_errorText.SetText(errorMessage.Value);
        this.m_errorContainer.SetWidth<Panel>((float) ((double) this.m_errorText.GetPreferedWidth() + 40.0 + 28.0));
        this.m_errorContainer.SetVisibility<Panel>(true);
      }
      else
        this.m_errorContainer.SetVisibility<Panel>(false);
    }

    public bool IsVisibleWithError() => this.m_isShown && this.m_errorContainer.IsVisible();

    public void Show()
    {
      this.buildUiIfNeeded();
      this.m_isShown = true;
      this.updateVisibility();
    }

    public void Hide()
    {
      if (this.m_container == null)
        return;
      this.SetExtraText(string.Empty);
      this.m_isShown = false;
      this.updateVisibility();
    }

    public void SetExtraText(string text)
    {
      this.buildUiIfNeeded();
      if (this.m_currentExtraText != text)
      {
        this.m_currentExtraText = text;
        this.m_extraText.SetText(text);
        this.m_extraTextContainer.SetWidth<Panel>(this.m_extraText.GetPreferedWidth() + 40f);
      }
      if (string.IsNullOrWhiteSpace(text))
      {
        if (!this.m_extraTextContainer.IsVisible())
          return;
        this.m_extraTextContainer.Hide<Panel>();
        this.m_errorContainer.SetAnchoredPosition<Panel>(new Vector2(0.0f, -40f));
      }
      else
      {
        if (this.m_extraTextContainer.IsVisible())
          return;
        this.m_errorContainer.SetAnchoredPosition<Panel>(new Vector2(0.0f, -80f));
        this.m_extraTextContainer.Show<Panel>();
      }
    }

    public void SetSellPrice(AssetValue price) => this.updatePrice(price, false);

    public void SetBuyPrice(AssetValue price) => this.updatePrice(price, true);

    private void updatePrice(AssetValue price, bool isBuy)
    {
      this.buildUiIfNeeded();
      this.m_pricePanel.SetPrice(price, new TextStyle?(isBuy ? this.m_builder.Style.PricePanel.MenuPricePanelStyle.TextStyle : this.m_builder.Style.PricePanel.MenuPricePanelSellStyle.TextStyle));
      this.m_container.SetWidth<Panel>(this.m_pricePanel.GetDynamicWidth());
      this.m_priceBg.SetVisibility<Panel>(price.IsNotEmpty);
      this.Show();
    }

    public void UpdatePosition()
    {
      this.buildUiIfNeeded();
      this.m_container.SetPosition<Panel>(Input.mousePosition + new Vector3(30f, (float) (-(double) this.m_container.GetHeight() - 30.0)));
    }

    private void cameraFreeLookChanged(bool isInFreeLookMode)
    {
      this.m_isTemporarilyHiddenInFreeLook = isInFreeLookMode;
      this.updateVisibility();
    }

    public void SetTemporarilyHidden(bool isHidden)
    {
      this.buildUiIfNeeded();
      this.IsTemporarilyHidden = isHidden;
      this.updateVisibility();
    }

    private void updateVisibility()
    {
      this.m_container.SetVisibility<Panel>(this.m_isShown && !this.IsTemporarilyHidden && !this.m_isTemporarilyHiddenInFreeLook);
    }
  }
}
