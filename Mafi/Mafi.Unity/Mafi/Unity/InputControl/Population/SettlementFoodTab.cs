// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.SettlementFoodTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class SettlementFoodTab : Tab
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    public int WidthAvailable;
    private Func<Settlement> m_settlementProvider;
    private readonly ImmutableArray<FoodProto> m_food;
    private readonly ImmutableArray<FoodCategoryProto> m_foodCats;

    private Settlement Settlement => this.m_settlementProvider();

    internal SettlementFoodTab(ProtosDb protosDb, UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (SettlementFoodTab));
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_foodCats = protosDb.All<FoodCategoryProto>().ToImmutableArray<FoodCategoryProto>();
      this.m_food = protosDb.All<FoodProto>().ToImmutableArray<FoodProto>();
    }

    public void SetSettlementProvider(Func<Settlement> settlementProvider)
    {
      this.m_settlementProvider = settlementProvider;
    }

    protected override void BuildUi()
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.SetSize<SettlementFoodTab>(new Vector2((float) this.WidthAvailable, 100f));
      StackContainer container = this.Builder.NewStackContainer("Category", (IUiElement) this).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToTopOf<StackContainer>((IUiElement) this, 0.0f);
      container.SizeChanged += (Action<IUiElement>) (_ => this.SetHeight<SettlementFoodTab>(container.GetDynamicHeight()));
      Txt parent1 = this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.FoodSupplyTitle, new LocStrFormatted?((LocStrFormatted) Tr.FoodSupplyTitle__TooltipForSettlement));
      Panel parent2 = this.Builder.AddOverlayPanel(container);
      TextWithIcon foodSupply = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/Food.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent2, 0.0f, Offset.Left(20f));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Settlement.MonthsOfFood)).Do((Action<int>) (monthOfFood => foodSupply.SetPrefixText(TrCore.NumberOfMonths.Format(monthOfFood.ToString(), monthOfFood).Value)));
      Txt sectionTitle = this.Builder.CreateSectionTitle((IUiElement) parent1, (LocStrFormatted) Tr.FoodHealth__Title, new LocStrFormatted?((LocStrFormatted) Tr.FoodHealth__Tooltip));
      sectionTitle.PutToLeftOf<Txt>((IUiElement) parent1, sectionTitle.GetPreferedWidth(), Offset.Left((float) (this.WidthAvailable / 2)));
      TextWithIcon healthBonusVal = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/Health.svg");
      healthBonusVal.SetPrefixText("+10");
      healthBonusVal.PutToLeftOf<TextWithIcon>((IUiElement) parent2, healthBonusVal.GetWidth(), Offset.Left((float) (this.WidthAvailable / 2 + 10)));
      Txt healthBonusExplained = this.Builder.NewTxt("HealthTxt").SetText("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent2, Offset.Left((float) ((double) (this.WidthAvailable / 2) + (double) healthBonusVal.GetWidth() + 10.0)));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Settlement.NominalHealthLastDayFromFood)).Observe<Percent>((Func<Percent>) (() => this.Settlement.FoodCategoriesWithHealthSatisfaction)).Do((Action<Percent, Percent>) ((health, percentageSatisfied) =>
      {
        int intPercentRounded = health.ToIntPercentRounded();
        Fix64 quantity = percentageSatisfied.ToIntPercentRounded().ToFix64() / 100;
        healthBonusExplained.SetText(string.Format(" ({0})", (object) Tr.FoodCategoriesSatisfied.Format(quantity.ToStringRounded(1), quantity)));
        if (intPercentRounded > 0)
        {
          healthBonusVal.SetColor(this.Builder.Style.Global.GreenForDark);
          healthBonusVal.SetPrefixText(string.Format("+{0}", (object) intPercentRounded));
        }
        else
        {
          healthBonusVal.SetColor(this.Builder.Style.Global.TextMedium.Color);
          healthBonusVal.SetPrefixText("0");
        }
      }));
      Txt parent3 = this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.FoodInSettlement__Title, new LocStrFormatted?((LocStrFormatted) Tr.FoodInSettlement__Tooltip));
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.Text).SetPrefixText(Tr.MonthDurationLegend.Format(60.ToString()).Value).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      textWithIcon.PutToLeftMiddleOf<TextWithIcon>((IUiElement) parent3, new Vector2(textWithIcon.GetWidth(), 20f), Offset.Left((float) ((double) this.WidthAvailable - (double) textWithIcon.GetWidth() - 20.0)));
      Panel categoriesContainer = this.Builder.NewPanel("CatsContainer", (IUiElement) container).AppendTo<Panel>(container, new float?(0.0f));
      int x = (this.WidthAvailable - (this.m_foodCats.Length - 1) * 5) / this.m_foodCats.Length;
      Dict<FoodCategoryProto, StackContainer> dict = new Dict<FoodCategoryProto, StackContainer>();
      int leftOffset = 0;
      foreach (FoodCategoryProto foodCat in this.m_foodCats)
      {
        StackContainer leftTopOf = this.Builder.NewStackContainer("Category", (IUiElement) categoriesContainer).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f).PutToLeftTopOf<StackContainer>((IUiElement) categoriesContainer, new Vector2((float) x, 0.0f), Offset.Left((float) leftOffset));
        leftTopOf.SizeChanged += (Action<IUiElement>) (element => container.UpdateItemHeight((IUiElement) categoriesContainer, categoriesContainer.GetHeight().Max(element.GetHeight())));
        Panel parent4 = this.Builder.NewPanel("Title", (IUiElement) leftTopOf).SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(leftTopOf, new float?(30f));
        Txt txt = this.Builder.NewTxt("TitleStr", (IUiElement) parent4).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Builder.Style.Global.Title).PutTo<Txt>((IUiElement) parent4);
        if (foodCat.HasHealthBenefit)
          this.Builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) this.Builder.NewIconContainer("Icon", (IUiElement) parent4).SetIcon("Assets/Unity/UserInterface/General/Health.svg").PutToRightOf<IconContainer>((IUiElement) parent4, 18f, Offset.Right(5f))).SetText((LocStrFormatted) Tr.FoodHealth__CategoryTooltip);
        txt.SetText((LocStrFormatted) foodCat.Strings.Name);
        dict.Add(foodCat, leftTopOf);
        leftOffset += x + 5;
      }
      int num = 80;
      foreach (FoodProto food in this.m_food)
      {
        SettlementFoodTab.FoodView element = new SettlementFoodTab.FoodView((IUiElement) dict[food.FoodCategory], this.Builder, this.m_unlockedProtosDb, this.m_settlementProvider, food);
        this.AddUpdater(element.Updater);
        dict[food.FoodCategory].Append((IUiElement) element, new float?((float) num));
      }
      this.AddUpdater(updaterBuilder.Build());
    }

    private class FoodView : IUiElementWithUpdater, IUiElement
    {
      private readonly Panel m_container;

      public IUiUpdater Updater { get; }

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public FoodView(
        IUiElement parent,
        UiBuilder builder,
        UnlockedProtosDbForUi unlockedProtosDb,
        Func<Settlement> settlementProvider,
        FoodProto food)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        bool isUnlocked = false;
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        this.m_container = builder.NewPanel("Container", parent).SetBackground(builder.Style.Panel.ItemOverlay);
        int x = 40;
        ProductQuantityWithIcon productView = new ProductQuantityWithIcon((IUiElement) this.m_container, builder);
        productView.PutToLeftMiddleOf<ProductQuantityWithIcon>((IUiElement) this.m_container, new Vector2((float) x, 80f));
        productView.SetProduct(food.Product, Fix32.Zero);
        productView.HideProductName();
        this.m_container.OnMouseEnter((Action) (() =>
        {
          if (!isUnlocked)
            return;
          productView.ShowProductName();
        }));
        this.m_container.OnMouseLeave((Action) (() => productView.HideProductName()));
        Panel rightTopOf = builder.NewPanel("Info", (IUiElement) this.m_container).SetBackground("Assets/Unity/UserInterface/Toolbar/Stats.svg").PutToRightTopOf<Panel>((IUiElement) this.m_container, 18.Vector2(), Offset.Right(5f) + Offset.Top(5f));
        TooltipWithIcon tooltip = new TooltipWithIcon(builder, 22);
        tooltip.AttachTo<Panel>((IUiElementWithHover<Panel>) rightTopOf);
        tooltip.TextWithIcon.SetPrefixText("1");
        tooltip.TextWithIcon.SetIcon(food.Product.Graphics.IconPath);
        TextWithIcon upoints = new TextWithIcon(builder, (IUiElement) this.m_container, 16);
        upoints.Icon.SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg");
        upoints.SetTextStyle(builder.Style.Global.TextMedium).SetPrefixText("0.00 / 0.00").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(upoints.GetWidth(), 28f), Offset.Left((float) (5 + x)));
        TextWithIcon consumedView = new TextWithIcon(builder, (IUiElement) this.m_container, 18).SetTextStyle(builder.Style.Panel.TextMedium).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, 28f), Offset.Left((float) (5 + x)) + Offset.Top(26f));
        TextWithIcon monthsLeftView = new TextWithIcon(builder, (IUiElement) this.m_container).SetTextStyle(builder.Style.Panel.TextMedium).SetIcon("Assets/Unity/UserInterface/Toolbar/Storages.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, 28f), Offset.Left((float) (5 + x)) + Offset.Top(52f));
        builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) monthsLeftView.AsPanel()).SetText((LocStrFormatted) Tr.IndividualFoodSupply__Tooltip);
        productView.SendToFront<ProductQuantityWithIcon>();
        Panel lockedPanel = builder.NewPanel("Locked", (IUiElement) this.m_container).SetBackground(new ColorRgba(0, 220)).PutTo<Panel>((IUiElement) this.m_container);
        builder.NewIconContainer("Locked", (IUiElement) lockedPanel).SetIcon("Assets/Unity/UserInterface/General/Locked128.png", (ColorRgba) 11776947).PutToRightBottomOf<IconContainer>((IUiElement) lockedPanel, 20.Vector2(), Offset.BottomRight(5f, 5f));
        updaterBuilder.Observe<bool>((Func<bool>) (() => isUnlocked || unlockedProtosDb.IsUnlocked((IProto) food.Product))).Do((Action<bool>) (isUnlockedNow =>
        {
          isUnlocked = isUnlockedNow;
          lockedPanel.SetVisibility<Panel>(!isUnlocked);
        }));
        updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => settlementProvider().FoodTypesMap[food.Product].SupplyLeft)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => settlementProvider().FoodTypesMap[food.Product].EstimatedMonthlyConsumption)).Do((Action<Quantity, PartialQuantity>) ((supplyLeft, consumedPerMonth) =>
        {
          productView.SetProduct(food.Product.WithQuantity(supplyLeft));
          consumedView.SetPrefixText(consumedPerMonth.ToStringRounded(1) + " / 60");
          int intFloored = !consumedPerMonth.IsZero ? (supplyLeft.Value / consumedPerMonth.Value).ToIntFloored() : 0;
          monthsLeftView.SetPrefixText(string.Format("~{0}", (object) TrCore.NumberOfMonths.Format(intFloored.ToString(), intFloored)));
          monthsLeftView.SetVisibility<TextWithIcon>(supplyLeft.IsPositive);
        }));
        updaterBuilder.Observe<Upoints>((Func<Upoints>) (() => settlementProvider().FoodTypesMap[food.Product].UpointsGivenLastDay)).Observe<Upoints>((Func<Upoints>) (() => settlementProvider().GetMaxUnityProvidedFor(food))).Do((Action<Upoints, Upoints>) ((upointsGiven, maxUpoints) =>
        {
          if (upointsGiven.IsNear(maxUpoints))
            upoints.SetColor(builder.Style.Global.GreenForDark);
          else
            upoints.SetColor(builder.Style.Global.Text.Color);
          upoints.SetPrefixText(upointsGiven.FormatForceDigits() + " / " + maxUpoints.FormatForceDigits());
        }));
        updaterBuilder.Observe<int>((Func<int>) (() => food.GetPopDaysFromQuantity(1.Quantity(), settlementProvider().ConsumptionMultiplier))).Do((Action<int>) (daysFromQuantity =>
        {
          Fix32 fix32 = (Fix32) (daysFromQuantity / 30);
          tooltip.TextWithIcon.SetSuffixText(string.Format("{0} / 60 ({1})", (object) Tr.FoodFeedInfo.Format(fix32.ToStringRounded(1), fix32.ToFix64()), (object) Tr.OneMonth));
        }));
        this.Updater = updaterBuilder.Build();
      }
    }
  }
}
