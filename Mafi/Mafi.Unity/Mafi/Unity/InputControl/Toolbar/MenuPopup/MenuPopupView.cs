// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.MenuPopupView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  public class MenuPopupView : WindowView
  {
    private readonly RecipesBookController m_recipesBookController;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private StackContainer m_mainContainer;
    private Txt m_description;
    private PricePanel m_pricePanel;
    private readonly EntityCostProvider m_costProvider;
    private ScrollableContainer m_scrollableRecipesContainer;
    private StackContainer m_assignedRecipesContainer;
    private RecipeView.Cache m_recipeViewsCache;
    private float m_recipesWidth;
    private Panel m_priceWrapper;
    private Panel m_topButtonsContainer;
    private StackContainer m_rightButtonsContainer;
    private bool m_hasItemInTopBar;
    private Btn m_consumedElectricity;
    private Btn m_producedElectricity;
    private Btn m_workersBtn;
    private Btn m_computingConsumed;
    private Btn m_upointsConsumed;
    private TextWithIcon m_throughput;
    private GridContainer m_productsGrid;
    private ViewsCacheHomogeneous<IconContainer> m_iconsCache;
    private int m_productsCount;
    private Txt m_productsTitle;
    private Btn m_maintenanceNeeded;
    private Txt m_maintenanceMult;
    private TextWithIcon m_fuelConsumption;
    private readonly IProperty<Percent> m_maintenanceMultiplier;

    public bool IsHovered { get; private set; }

    public MenuPopupView(
      RecipesBookController recipesBookController,
      IAssetTransactionManager assetTransactionManager,
      EntityCostProvider costProvider,
      IPropertiesDb propsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("MenuPopup", WindowView.FooterStyle.None);
      this.m_recipesBookController = recipesBookController;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_costProvider = costProvider;
      this.m_maintenanceMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier);
    }

    protected override void BuildWindowContent()
    {
      this.m_iconsCache = (ViewsCacheHomogeneous<IconContainer>) new IconContainer.Cache(this.Builder);
      this.OnMouseEnter((Action) (() => this.IsHovered = true));
      this.OnMouseLeave((Action) (() => this.IsHovered = false));
      this.m_mainContainer = this.Builder.NewStackContainer("Popup").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.TopBottom(5f)).SetItemSpacing(5f).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_topButtonsContainer = this.AddTopButtonsContainer(this.m_mainContainer);
      this.m_rightButtonsContainer = this.AddRightVerticalButtonsContainer((IUiElement) this.m_topButtonsContainer);
      this.m_consumedElectricity = this.Builder.NewBtn("Electricity").EnableDynamicSize().SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Consuming).SetText("").SetIcon(this.Style.Panel.ElectricityInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      this.m_producedElectricity = this.Builder.NewBtn("Electricity").EnableDynamicSize().SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Producing).SetText("").SetIcon(this.Style.Panel.ElectricityInfoBoxIcon_Producing).AppendTo<Btn>(this.m_rightButtonsContainer);
      this.m_workersBtn = this.Builder.NewBtn("Workers").EnableDynamicSize().SetButtonStyle(this.Style.Panel.WorkersInfoBox).SetText("").SetIcon(this.Style.Panel.WorkersInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      this.m_upointsConsumed = this.Builder.NewBtn("Unity").EnableDynamicSize().SetButtonStyle(this.Style.Panel.ConsumedUnityInfoBox).SetText("").SetIcon(this.Style.Panel.ConsumedUnityInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      this.m_computingConsumed = this.Builder.NewBtn("Computing").EnableDynamicSize().SetButtonStyle(this.Style.Panel.ComputingInfoBox_Consuming).SetText("").SetIcon(this.Style.Panel.ComputingInfoBoxIcon_Consuming).AppendTo<Btn>(this.m_rightButtonsContainer, new float?((float) this.Style.Panel.ComputingInfoBox_Consuming.Width));
      this.m_maintenanceNeeded = this.Builder.NewBtn("Maintenance").EnableDynamicSize().SetButtonStyle(this.Style.Panel.InfoBoxDefault).SetText("").AppendTo<Btn>(this.m_rightButtonsContainer);
      this.m_description = this.Builder.NewTxt("Desc").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Style.Global.TextMedium).AppendTo<Txt>(this.m_mainContainer, new float?(0.0f), Offset.LeftRight(10f));
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder);
      TextStyle textControls1 = this.Builder.Style.Global.TextControls;
      ref TextStyle local1 = ref textControls1;
      FontStyle? nullable = new FontStyle?(FontStyle.Bold);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = nullable;
      int? fontSize1 = new int?();
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      this.m_throughput = textWithIcon.SetTextStyle(textStyle1).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").AppendTo<TextWithIcon>(this.m_mainContainer, new float?(30f), Offset.LeftRight(10f));
      Txt txt1 = this.Builder.NewTxt("Maintenance");
      TextStyle textControls2 = this.Builder.Style.Global.TextControls;
      ref TextStyle local2 = ref textControls2;
      nullable = new FontStyle?(FontStyle.Bold);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? fontStyle2 = nullable;
      int? fontSize2 = new int?();
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      this.m_maintenanceMult = txt1.SetTextStyle(textStyle2).AppendTo<Txt>(this.m_mainContainer, new float?(30f), Offset.LeftRight(10f));
      this.m_fuelConsumption = new TextWithIcon(this.Builder).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60");
      this.m_fuelConsumption.AppendTo<TextWithIcon>(this.m_mainContainer, new Vector2?(new Vector2(0.0f, 30f)), ContainerPosition.LeftOrTop, Offset.LeftRight(10f));
      this.m_pricePanel = new PricePanel(this.Builder, this.Style.PricePanel.MenuPricePanelStyle, (Option<IAvailableProductsProvider>) (IAvailableProductsProvider) new ProductsAvailableInStorage(this.m_assetTransactionManager));
      this.m_priceWrapper = this.Builder.NewPanel("PriceWrapper").AppendTo<Panel>(this.m_mainContainer, new float?(this.m_pricePanel.PreferredHeight));
      Txt txt2 = this.Builder.NewTitle("Cost").SetText((LocStrFormatted) Tr.ConstructionCost);
      txt2.PutToLeftOf<Txt>((IUiElement) this.m_priceWrapper, txt2.GetPreferedWidth(), Offset.Left(10f));
      this.m_pricePanel.PutToLeftOf<PricePanel>((IUiElement) this.m_priceWrapper, 0.0f, Offset.Left(txt2.GetWidth()));
      this.m_productsTitle = this.Builder.NewTitle("").AppendTo<Txt>(this.m_mainContainer, new float?(20f), Offset.Left(10f));
      this.m_productsGrid = this.Builder.NewGridContainer("Products").SetCellSize(24.Vector2()).SetCellSpacing(5f).SetInnerPadding(Offset.Left(20f) + Offset.Right(5f)).AppendTo<GridContainer>(this.m_mainContainer, new Vector2?(new Vector2(0.0f, 0.0f)), ContainerPosition.LeftOrTop);
      this.m_scrollableRecipesContainer = this.Builder.NewScrollableContainer("Recipes scroll").AddVerticalScrollbar().AppendTo<ScrollableContainer>(this.m_mainContainer, new float?(0.0f));
      this.m_assignedRecipesContainer = this.Builder.NewStackContainer("Assigned recipes").SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.LeftRight(15f)).SetItemSpacing(this.Style.Panel.ItemsSpacing);
      this.m_scrollableRecipesContainer.AddItemTop((IUiElement) this.m_assignedRecipesContainer);
      this.m_recipeViewsCache = new RecipeView.Cache((IUiElement) this.m_assignedRecipesContainer, this.Builder, (Option<RecipesBookController>) this.m_recipesBookController);
      this.AddUpdater(this.m_pricePanel.CreateUpdater());
    }

    /// <summary>
    /// Hides all views used by previous item. Call this before you start populate this view with new item. Don't
    /// forget to call <see cref="M:Mafi.Unity.InputControl.Toolbar.MenuPopup.MenuPopupView.SetupFinished" /> after you are done.
    /// </summary>
    public void Reset()
    {
      this.m_mainContainer.StartBatchOperation();
      this.m_mainContainer.HideItem((IUiElement) this.m_description);
      this.m_mainContainer.HideItem((IUiElement) this.m_priceWrapper);
      this.m_mainContainer.HideItem((IUiElement) this.m_scrollableRecipesContainer);
      this.m_mainContainer.HideItem((IUiElement) this.m_throughput);
      this.m_mainContainer.HideItem((IUiElement) this.m_maintenanceMult);
      this.m_mainContainer.HideItem((IUiElement) this.m_productsGrid);
      this.m_mainContainer.HideItem((IUiElement) this.m_productsTitle);
      this.m_mainContainer.HideItem((IUiElement) this.m_fuelConsumption);
      this.m_mainContainer.FinishBatchOperation();
      this.m_rightButtonsContainer.StartBatchOperation();
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_consumedElectricity);
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_producedElectricity);
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_workersBtn);
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_upointsConsumed);
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_computingConsumed);
      this.m_rightButtonsContainer.HideItem((IUiElement) this.m_maintenanceNeeded);
      this.m_rightButtonsContainer.FinishBatchOperation();
      this.m_hasItemInTopBar = false;
      this.m_productsCount = 0;
    }

    public void SetRecipes(Lyst<IRecipeForUi> recipes)
    {
      this.m_assignedRecipesContainer.StartBatchOperation();
      this.m_assignedRecipesContainer.ClearAll(true);
      float self1 = 0.0f;
      foreach (IRecipeForUi recipe in recipes)
      {
        RecipeView view = this.m_recipeViewsCache.GetView(recipe);
        view.AppendTo<RecipeView>(this.m_assignedRecipesContainer, new float?(this.Style.RecipeDetail.Height), new Offset(1f, 0.0f, 1f, 0.0f));
        self1 = self1.Max(view.GetDynamicWidth());
        view.UpdateNormalization();
        view.Show<RecipeView>();
      }
      foreach (IRecipeForUi recipe in recipes)
      {
        RecipeView view = this.m_recipeViewsCache.GetView(recipe);
        view.SetLeftIndent(self1 - view.GetDynamicWidth());
      }
      this.m_assignedRecipesContainer.FinishBatchOperation();
      this.m_mainContainer.SetItemVisibility((IUiElement) this.m_scrollableRecipesContainer, true);
      float num = this.Style.RecipeDetail.Height + this.Style.Panel.ItemsSpacing;
      float a = (float) recipes.Count * num;
      float self2 = Mathf.Min(a, num * 4.5f);
      if ((double) self2 < (double) a)
      {
        this.m_assignedRecipesContainer.SetInnerPadding(Offset.Right(16f));
        this.m_recipesWidth = self1 + 16f;
      }
      else
      {
        this.m_assignedRecipesContainer.SetInnerPadding(Offset.Zero);
        this.m_recipesWidth = self1;
      }
      this.m_mainContainer.UpdateItemHeight((IUiElement) this.m_scrollableRecipesContainer, self2.Max(num));
    }

    public void SetProducts(Lyst<ProductProto> products, LocStrFormatted title)
    {
      this.m_productsTitle.SetText(title);
      this.m_productsGrid.StartBatchOperation();
      this.m_productsGrid.ClearAllAndHide();
      this.m_iconsCache.ReturnAll();
      foreach (ProductProto product in products)
      {
        IconContainer view = this.m_iconsCache.GetView();
        view.SetIcon(product.Graphics.IconPath);
        this.m_productsGrid.Append((IUiElement) view);
      }
      this.m_productsCount = products.Count;
      this.m_productsGrid.FinishBatchOperation();
    }

    public void SetPrice(LayoutEntityProto proto)
    {
      this.m_pricePanel.SetPrice(this.m_costProvider.GetEntityCost(proto));
      this.m_mainContainer.ShowItem((IUiElement) this.m_priceWrapper);
      if (!proto.Costs.Maintenance.MaintenancePerMonth.IsPositive)
        return;
      this.m_maintenanceNeeded.SetText(proto.Costs.Maintenance.MaintenancePerMonth.ScaledBy(this.m_maintenanceMultiplier.Value).ToStringRounded());
      Btn maintenanceNeeded = this.m_maintenanceNeeded;
      string iconPath = proto.Costs.Maintenance.Product.Graphics.IconPath;
      Vector2? nullable = new Vector2?(20.Vector2());
      ColorRgba? color = new ColorRgba?();
      Vector2? size = nullable;
      IconStyle iconStyle = new IconStyle(iconPath, color, size);
      maintenanceNeeded.SetIcon(iconStyle);
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_maintenanceNeeded);
      this.m_hasItemInTopBar = true;
    }

    public void SetPricePerTile(AssetValue pricePerTile)
    {
      this.m_pricePanel.SetPrice(pricePerTile);
      this.m_mainContainer.ShowItem((IUiElement) this.m_priceWrapper);
    }

    public void SetThroughput(PartialQuantity throughputPerTick)
    {
      Duration duration = this.Builder.DurationNormalizer.IsNormalizationOn ? 60.Seconds() : 1.Seconds();
      string str = this.Builder.DurationNormalizer.NormalizeThroughput(throughputPerTick);
      this.m_throughput.SetPrefixText(Tr.ThroughputWithParam.Format(string.Format("{0} / {1}", (object) str, (object) duration.Seconds.IntegerPart)).Value.ToUpper(LocalizationManager.CurrentCultureInfo));
      this.m_mainContainer.ShowItem((IUiElement) this.m_throughput);
    }

    public void SetThroughputPer60(Quantity quantity, Duration duration)
    {
      Duration duration1 = 60.Seconds();
      Fix32 fix32 = duration1.Ticks.ToFix32() / duration.Ticks * quantity.Value;
      this.m_throughput.SetPrefixText(Tr.ThroughputWithParam.Format(string.Format("{0} / {1}", (object) fix32.ToStringRounded(1), (object) duration1.Seconds.IntegerPart)).Value.ToUpper(LocalizationManager.CurrentCultureInfo));
      this.m_mainContainer.ShowItem((IUiElement) this.m_throughput);
    }

    public void SetFuelConsumption(PartialProductQuantity fuelConsumed)
    {
      if (fuelConsumed.IsEmpty)
        return;
      string stringRounded = fuelConsumed.Quantity.ToStringRounded(1);
      this.m_fuelConsumption.SetPrefixText(string.Format("{0}: {1}", (object) Tr.Consumption, (object) stringRounded));
      this.m_fuelConsumption.SetIcon(fuelConsumed.Product.Graphics.IconPath);
      this.m_mainContainer.ShowItem((IUiElement) this.m_fuelConsumption);
    }

    public void SetVehiclesMaintenanceMult(Percent mult)
    {
      if (!(mult < Percent.Hundred))
        return;
      this.m_maintenanceMult.SetText(string.Format("{0}: -{1}", (object) Tr.VehiclesMaintenance, (object) mult.InverseTo100().ToStringRounded()));
      this.m_mainContainer.ShowItem((IUiElement) this.m_maintenanceMult);
    }

    public void SetDescription(LocStrFormatted desc) => this.SetDescription(desc.Value);

    public void SetDescription(string desc)
    {
      if (desc.IsEmpty())
        return;
      this.m_description.SetText(desc);
      this.m_mainContainer.ShowItem((IUiElement) this.m_description);
    }

    public void AddPowerConsumption(Electricity electricity)
    {
      this.m_consumedElectricity.SetText(electricity.Format());
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_consumedElectricity);
      this.m_hasItemInTopBar = true;
    }

    public void AddPowerProduction(Electricity electricity)
    {
      this.m_consumedElectricity.SetText(string.Format("Max: {0}", (object) electricity));
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_consumedElectricity);
      this.m_hasItemInTopBar = true;
    }

    public void AddUpointsConsumption(Upoints upoints)
    {
      this.m_upointsConsumed.SetText(upoints.Format());
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_upointsConsumed);
      this.m_hasItemInTopBar = true;
    }

    public void AddWorkers(int workersCount)
    {
      this.m_workersBtn.SetText(workersCount.ToString());
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_workersBtn);
      this.m_hasItemInTopBar = true;
    }

    public void AddComputingConsumption(Computing computing)
    {
      this.m_computingConsumed.SetText(computing.Format());
      this.m_rightButtonsContainer.ShowItem((IUiElement) this.m_computingConsumed);
      this.m_hasItemInTopBar = true;
    }

    /// <summary>
    /// Finalizes the view after item was set. Call this when you are done with the setup. This will run final layout
    /// measurements.
    /// </summary>
    public void SetupFinished()
    {
      this.m_mainContainer.StartBatchOperation();
      this.m_mainContainer.SetItemVisibility((IUiElement) this.m_topButtonsContainer, this.m_hasItemInTopBar);
      float num = (this.m_priceWrapper.IsVisible() ? this.m_pricePanel.GetDynamicWidth() + 40f : 0.0f).Max(this.m_rightButtonsContainer.GetDynamicWidth()).Max(300f);
      if (this.m_scrollableRecipesContainer.IsVisible())
        num = num.Max(this.m_recipesWidth);
      this.m_mainContainer.UpdateItemHeight((IUiElement) this.m_description, this.m_description.GetPreferedHeight(num - 20f));
      this.m_mainContainer.SetItemVisibility((IUiElement) this.m_productsGrid, this.m_productsCount > 0);
      this.m_mainContainer.SetItemVisibility((IUiElement) this.m_productsTitle, this.m_productsCount > 0);
      if (this.m_productsCount > 0)
      {
        this.m_productsGrid.SetDynamicHeightMode(((float) (((double) num - 25.0) / 29.0)).FloorToInt());
        this.m_productsGrid.SetWidth<GridContainer>(this.m_productsGrid.GetRequiredWidth());
        this.m_mainContainer.UpdateItemHeight((IUiElement) this.m_productsGrid, this.m_productsGrid.GetRequiredHeight());
      }
      this.m_mainContainer.FinishBatchOperation();
      this.SetContentSize(num, this.m_mainContainer.GetDynamicHeight());
    }
  }
}
