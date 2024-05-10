// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsProductsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsProductsTab : Tab
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly StatsProductTab m_productDetailTab;
    private readonly PinnedProductsManager m_pinnedProductsManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly LazyResolve<RecipesBookController> m_recipesBookController;
    private readonly ImmutableArray<ProductStats> m_products;
    private bool m_productSetChanged;
    private readonly Dict<ProductStats, StatsProductsTab.ProductTile> m_tiles;
    private GridContainer m_gridContainer;
    private readonly Lyst<ProductStats> m_productsToShow;
    private TxtField m_searchBox;
    private Txt m_nothingFoundInfo;
    private readonly Lyst<Proto> m_protosToSearchIn;
    private readonly Set<Proto> m_protosFound;

    internal StatsProductsTab(
      IInputScheduler inputScheduler,
      IProductsManager productsManager,
      StatsProductTab productDetailTab,
      PinnedProductsManager pinnedProductsManager,
      UnlockedProtosDbForUi unlockedProtosDb,
      LazyResolve<RecipesBookController> recipesBookController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_tiles = new Dict<ProductStats, StatsProductsTab.ProductTile>();
      this.m_productsToShow = new Lyst<ProductStats>();
      this.m_protosToSearchIn = new Lyst<Proto>();
      this.m_protosFound = new Set<Proto>();
      // ISSUE: explicit constructor call
      base.\u002Ector("ProductsStats");
      this.m_inputScheduler = inputScheduler;
      this.m_productDetailTab = productDetailTab;
      this.m_pinnedProductsManager = pinnedProductsManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_recipesBookController = recipesBookController;
      this.m_products = productsManager.ProductStats.OrderBy<string>((Func<ProductStats, string>) (x => x.Product.Strings.Name.TranslatedString)).ToImmutableArray<ProductStats>();
    }

    protected override void BuildUi()
    {
      this.SetWidth<StatsProductsTab>((float) this.AvailableWidth);
      this.SetHeight<StatsProductsTab>(100f);
      this.m_searchBox = this.Builder.NewTxtField("Search", (IUiElement) this).SetStyle(this.Builder.Style.Global.LightTxtFieldStyle).SetPlaceholderText(Tr.Search).SetCharLimit(30).PutToCenterTopOf<TxtField>((IUiElement) this, new Vector2(300f, 30f), Offset.TopBottom(5f));
      this.m_searchBox.SetDelayedOnEditEndListener(new Action<string>(this.search));
      this.m_gridContainer = this.Builder.NewGridContainer("Container").SetCellSize(StatsProductsTab.ProductTile.SIZE).SetCellSpacing(2f).SetDynamicHeightMode((this.AvailableWidth - 10) / (StatsProductsTab.ProductTile.SIZE.x + 2f).FloorToInt()).SetInnerPadding(Offset.All(5f) + Offset.Top(40f)).PutToLeftTopOf<GridContainer>((IUiElement) this, Vector2.zero);
      Txt txt = this.Builder.NewTxt("NothingFound").SetAlignment(TextAnchor.MiddleCenter);
      TextStyle text = this.Builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable1 = new int?(16);
      FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable2;
      int? fontSize = nullable1;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_nothingFoundInfo = txt.SetTextStyle(textStyle).PutToTopOf<Txt>((IUiElement) this, 100f, Offset.Top(40f)).Hide<Txt>();
      this.updateRowsVisiblity();
      this.rebuild();
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.updateRowsVisiblity);
      this.OnShowStart += new Action(this.ClearSearch);
    }

    public void ClearSearch()
    {
      if (!this.m_searchBox.GetText().IsNotEmpty())
        return;
      this.m_searchBox.ClearInput();
      this.search("");
    }

    private void updateRowsVisiblity()
    {
      this.m_productsToShow.Clear();
      foreach (ProductStats product in this.m_products)
      {
        if (!product.Product.IsExcludedFromStats && product.Product.IsAvailable && this.m_unlockedProtosDb.IsUnlocked((IProto) product.Product))
          this.m_productsToShow.Add(product);
      }
      this.m_productSetChanged = true;
    }

    private void productClicked(ProductStats product)
    {
      this.m_productDetailTab.OpenProductDetails(product);
    }

    private void productRightClicked(ProductStats product)
    {
      this.m_recipesBookController.Value.OpenForProduct(product.Product);
    }

    private void rebuild()
    {
      this.m_nothingFoundInfo.Hide<Txt>();
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.ClearAll();
      foreach (ProductStats key in this.m_productsToShow)
      {
        // ISSUE: method pointer
        this.m_gridContainer.Append((IUiElement) this.m_tiles.GetOrAdd<ProductStats, StatsProductsTab.ProductTile>(key, new Func<ProductStats, StatsProductsTab.ProductTile>((object) this, __methodptr(\u003Crebuild\u003Eg__newProductTile\u007C20_0))));
      }
      this.m_gridContainer.FinishBatchOperation();
      this.SetHeight<StatsProductsTab>(this.m_gridContainer.GetHeight());
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (this.m_productSetChanged)
      {
        this.rebuild();
        this.m_productSetChanged = false;
      }
      foreach (StatsProductsTab.ProductTile productTile in this.m_tiles.Values)
        productTile.Updater.RenderUpdate();
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      foreach (StatsProductsTab.ProductTile productTile in this.m_tiles.Values)
        productTile.Updater.SyncUpdate();
    }

    private void search(string text)
    {
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.HideAllItems();
      foreach (StatsProductsTab.ProductTile productTile in this.m_tiles.Values)
        this.m_protosToSearchIn.Add((Proto) productTile.Product);
      UiSearchUtils.MatchProtos<Proto>(text, (IIndexable<Proto>) this.m_protosToSearchIn, this.m_protosFound);
      this.m_protosToSearchIn.Clear();
      foreach (StatsProductsTab.ProductTile element in this.m_tiles.Values)
      {
        if (this.m_protosFound.Contains((Proto) element.Product))
          this.m_gridContainer.ShowItem((IUiElement) element);
      }
      this.m_gridContainer.FinishBatchOperation();
      this.SetHeight<StatsProductsTab>(this.m_gridContainer.GetHeight().Max(100f));
      if (this.m_gridContainer.VisibleItemsCount == 0)
        this.m_nothingFoundInfo.SetText(Tr.NothingFoundFor.Format(text)).Show<Txt>();
      else
        this.m_nothingFoundInfo.Hide<Txt>();
    }

    private class ProductTile : IUiElement
    {
      public static readonly Vector2 SIZE;
      private readonly ProductStats m_product;
      private readonly Btn m_container;
      private IconContainer m_icon;
      private Txt m_title;
      private Txt m_stored;
      public IUiUpdater Updater;
      private string m_titleForSearch;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public ProductProto Product => this.m_product.Product;

      public ProductTile(
        IUiElement parent,
        IInputScheduler inputScheduler,
        UiBuilder builder,
        ProductStats product,
        PinnedProductsManager pinnedProductsManager,
        Action<ProductStats> onClick,
        Action<ProductStats> onRightClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        StatsProductsTab.ProductTile productTile = this;
        this.m_product = product;
        this.m_titleForSearch = product.Product.Strings.Name.TranslatedString.ToLowerInvariant();
        this.m_container = builder.NewBtn("Container", parent).SetButtonStyle(builder.Style.Global.ListMenuBtnDarker).OnClick((Action) (() => onClick(productTile.m_product))).OnRightClick((Action) (() => onRightClick(product)));
        this.m_icon = builder.NewIconContainer("Icon", (IUiElement) this.m_container).PutToLeftOf<IconContainer>((IUiElement) this.m_container, 40f, Offset.Left(5f) + Offset.TopBottom(10f));
        BtnStyle styleEnabled = builder.Style.StatusBar.PlayButtonEnabled;
        BtnStyle styleDisabled = builder.Style.StatusBar.PlayButtonDisabled;
        Btn pin = builder.NewBtn("Pin", (IUiElement) this.m_container).SetButtonStyle(styleDisabled).SetIcon("Assets/Unity/UserInterface/General/Pin.svg").OnClick((Action) (() => inputScheduler.ScheduleInputCmd<PinToggleCmd>(new PinToggleCmd(productTile.m_product.Product.Id)))).PutToRightBottomOf<Btn>((IUiElement) this.m_container, 20.Vector2(), Offset.BottomRight(5f, 5f));
        this.m_title = builder.NewTxt("Name", (IUiElement) this.m_container).SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_container, 20f, Offset.Left(50f) + Offset.Top(5f));
        Txt txt = builder.NewTxt("Stored", (IUiElement) this.m_container);
        TextStyle title = builder.Style.Global.Title;
        ref TextStyle local = ref title;
        bool? nullable = new bool?(false);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = new int?();
        bool? isCapitalized = nullable;
        TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
        this.m_stored = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, Offset.Left(50f) + Offset.Bottom(5f) + Offset.Right(25f));
        this.m_icon.SetIcon(product.Product.Graphics.IconPath);
        this.m_title.SetText((LocStrFormatted) product.Product.Strings.Name);
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<QuantityLarge>((Func<QuantityLarge>) (() => productTile.m_product.StoredQuantityTotal)).Do((Action<QuantityLarge>) (stored => productTile.m_stored.SetText(string.Format("{0} x", (object) stored))));
        updaterBuilder.Observe<bool>((Func<bool>) (() => pinnedProductsManager.IsPinned(product.Product))).Do((Action<bool>) (pinned => pin.SetButtonStyle(pinned ? styleEnabled : styleDisabled)));
        this.Updater = updaterBuilder.Build();
      }

      static ProductTile()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        StatsProductsTab.ProductTile.SIZE = new Vector2(160f, 60f);
      }
    }
  }
}
