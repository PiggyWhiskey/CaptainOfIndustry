// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Nuclear.NuclearReactorView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.NuclearReactors;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
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
namespace Mafi.Unity.InputControl.Inspectors.Nuclear
{
  internal class NuclearReactorView : StaticEntityInspectorBase<NuclearReactor>
  {
    private RecipeView.Cache m_viewsCache;
    private StackContainer m_assignedRecipesContainer;
    private ScrollableContainer m_scrollableContainer;
    private IUiUpdater m_updater;
    private readonly NuclearReactorInspector m_controller;
    private readonly Lyst<RecipeView> m_assignedRecipes;
    private StatusPanel m_statusInfo;
    private readonly float m_width;
    private BufferViewOneSlider m_powerBufferSlider;

    protected override NuclearReactor Entity => this.m_controller.SelectedEntity;

    public NuclearReactorView(NuclearReactorInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_assignedRecipes = new Lyst<RecipeView>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<NuclearReactorInspector>();
      this.m_width = 640f;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.m_viewsCache = new RecipeView.Cache((IUiElement) this.m_assignedRecipesContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, new Action<IRecipeForUi>(this.recipeClicked), true);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(((EntityInspector<NuclearReactor, NuclearReactorView>) this.m_controller).Clear));
      this.m_statusInfo = this.AddStatusInfoPanel();
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      Txt parent1 = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.NuclearReactor__PowerLevelTitle, new LocStrFormatted?((LocStrFormatted) Tr.NuclearReactor__PowerLevelTooltip));
      Panel parent2 = this.Builder.NewPanel("ToggleContainer").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new Vector2?(new Vector2(this.m_width / 2f, this.Style.BufferView.HeightWithSlider + 25f)), ContainerPosition.LeftOrTop);
      this.m_powerBufferSlider = this.Builder.NewBufferWithOneSlider((IUiElement) itemContainer, new Action<float>(this.sliderValueChange), 3, "", customColor: new ColorRgba?((ColorRgba) 16755968), makeSliderAreaTransparent: true).PutTo<BufferViewOneSlider>((IUiElement) parent2, Offset.Bottom(25f));
      this.m_powerBufferSlider.Bar.SetColor((ColorRgba) 9533516);
      this.m_powerBufferSlider.ShowSliderForNoProduct();
      this.m_powerBufferSlider.UpdateState(Option<ProductProto>.None, Percent.Zero, LocStrFormatted.Empty);
      this.m_powerBufferSlider.SetCustomIcon("Assets/Unity/UserInterface/General/Power.svg");
      this.m_powerBufferSlider.SetLabelFunc((Func<int, string>) (step => step.ToStringCached() + "x"));
      SwitchBtn autoRegulationToggle = this.Builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.NuclearReactor__AutoThrottle).AddTooltip((LocStrFormatted) Tr.NuclearReactor__AutoThrottle_Tooltip).SetOnToggleAction((Action<bool>) (isEnabled => this.m_controller.Context.InputScheduler.ScheduleInputCmd<NuclearReactorToggleAutomaticRegulationCmd>(new NuclearReactorToggleAutomaticRegulationCmd(this.Entity.Id))));
      autoRegulationToggle.PutToLeftBottomOf<SwitchBtn>((IUiElement) parent2, new Vector2(autoRegulationToggle.GetWidth(), 25f), Offset.Left(30f) + Offset.Bottom(5f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsAutomaticPowerRegulationSupported)).Observe<bool>((Func<bool>) (() => this.Entity.IsAutomaticPowerRegulationEnabled)).Do((Action<bool, bool>) ((isSupported, isAutoModeEnabled) =>
      {
        autoRegulationToggle.SetIsOn(isAutoModeEnabled);
        autoRegulationToggle.SetVisibility<SwitchBtn>(isSupported);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.CurrentPowerLevel)).Observe<Percent>((Func<Percent>) (() => this.Entity.TargetPowerLevel)).Observe<NuclearReactorProto>((Func<NuclearReactorProto>) (() => this.Entity.Prototype)).Do((Action<Percent, Percent, NuclearReactorProto>) ((powerLevel, targetPowerLevel, proto) =>
      {
        this.m_powerBufferSlider.SetMaxSteps(proto.MaxPowerLevel);
        this.m_powerBufferSlider.SetCustomIconText((targetPowerLevel.RawValue / Percent.Hundred.RawValue).ToString());
        this.m_powerBufferSlider.UpdateBarOnly(powerLevel / proto.MaxPowerLevel, LocStrFormatted.Empty);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.TargetPowerLevel)).Do((Action<Percent>) (targetPowerLevel => this.m_powerBufferSlider.UpdateSlider(targetPowerLevel.IntegerPart)));
      this.Builder.CreateSectionTitle((IUiElement) parent1, (LocStrFormatted) Tr.NuclearReactorRods__StatusTitle, new LocStrFormatted?((LocStrFormatted) Tr.NuclearReactorRods__Tooltip)).PutToLeftOf<Txt>((IUiElement) parent1, 200f, Offset.Left(this.m_powerBufferSlider.GetWidth()));
      BufferWithMultipleProductsView buffer = new BufferWithMultipleProductsView((IUiElement) itemContainer, this.Builder);
      buffer.PutToLeftOf<BufferWithMultipleProductsView>((IUiElement) parent2, this.m_width / 2f, Offset.Left(this.m_width / 2f));
      QuantityBar.Marker minFuelMarker = buffer.AddMarker(this.Builder, Percent.Zero, this.Builder.Style.Global.OrangeText);
      minFuelMarker.AddTooltip(this.Builder, Tr.NuclearReactorRods__MinRequired.Format(16));
      Lyst<ProductQuantity> productsCache = new Lyst<ProductQuantity>();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.Entity.GetFuelCapacityInBuffers())).Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        this.Entity.GetFuelQuantities(productsCache);
        return (IIndexable<ProductQuantity>) productsCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Do((Action<Quantity, Lyst<ProductQuantity>>) ((capacity, quantities) =>
      {
        buffer.SetProducts(quantities, capacity, false);
        if (capacity.IsPositive)
          minFuelMarker.SetPosition(Percent.FromRatio(16, capacity.Value));
        else
          minFuelMarker.SetPosition(Percent.Zero);
      }));
      StackContainer parent3 = itemContainer;
      LocStrFormatted reactorHeatLevelTitle = (LocStrFormatted) Tr.NuclearReactor__HeatLevelTitle;
      LocStrFormatted? tooltip1 = new LocStrFormatted?();
      Offset? nullable = new Offset?();
      Offset? extraOffset1 = nullable;
      Tooltip heatTitleTooltip = this.Builder.AddTooltipForTitle(this.AddSectionTitle(parent3, reactorHeatLevelTitle, tooltip1, extraOffset1), (LocStrFormatted) Tr.NuclearReactor__HeatLevelTooltip);
      Panel parent4 = this.Builder.NewPanel("Bar container").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      QuantityBar heatBar = new QuantityBar(this.Builder).PutTo<QuantityBar>((IUiElement) parent4, Offset.TopBottom(10f) + Offset.Left(90f) + Offset.Right(20f));
      this.Builder.NewIconContainer("icon").SetIcon("Assets/Unity/UserInterface/General/Temperature.svg").PutToLeftMiddleOf<IconContainer>((IUiElement) parent4, 38.Vector2(), Offset.Left(28f));
      QuantityBar.Marker optimalHeatMarker = heatBar.AddMarker(Percent.Zero, (ColorRgba) 2995501);
      QuantityBar.Marker coolingHeatMarker = heatBar.AddMarker(Percent.FromRatio(3, 5), (ColorRgba) 13181988);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.Prototype.LeakRadiationOnMeltdown)).Do((Action<bool>) (leaksRadiation =>
      {
        if (leaksRadiation)
          heatTitleTooltip.SetText(Tr.NuclearReactor__HeatLevelTooltip.ToString() + " " + Tr.NuclearReactor__HeatLevelRadiationSuffix.ToString());
        else
          heatTitleTooltip.SetText(Tr.NuclearReactor__HeatLevelTooltip.ToString() + " " + Tr.NuclearReactor__HeatLevelNoRadiationSuffix.ToString());
      }));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.HeatAmount)).Observe<int>((Func<int>) (() => this.Entity.OptimalHeatForCurrentPower)).Observe<int>((Func<int>) (() => this.Entity.StartEmergencyCoolingAtHeat)).Observe<int>((Func<int>) (() => this.Entity.MeltdownAtHeat)).Do((Action<int, int, int, int>) ((heat, optimalHeat, coolingHeat, meltdownAtHeat) =>
      {
        optimalHeatMarker.SetPosition(Percent.FromRatio(optimalHeat, meltdownAtHeat));
        coolingHeatMarker.SetPosition(Percent.FromRatio(coolingHeat, meltdownAtHeat));
        Percent percent = Percent.FromRatio(heat, optimalHeat > 0 ? optimalHeat : meltdownAtHeat);
        heatBar.UpdateValues(Percent.FromRatio(heat, meltdownAtHeat), percent.ToStringRounded());
        heatBar.SetColor(heat < coolingHeat ? this.Style.QuantityBar.PositiveBarColor : (heat < meltdownAtHeat ? this.Style.QuantityBar.NegativeBarColor : (ColorRgba) 16711680));
      }));
      StackContainer parent5 = itemContainer;
      LocStrFormatted recipes = (LocStrFormatted) Tr.Recipes;
      nullable = new Offset?(Offset.Bottom(5f));
      LocStrFormatted? tooltip2 = new LocStrFormatted?();
      Offset? extraOffset2 = nullable;
      this.Builder.DurationNormalizer.AttachPer60ToggleToTitle(this.AddSectionTitle(parent5, recipes, tooltip2, extraOffset2), this.Builder, updaterBuilder);
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("Recipes scroll").AddVerticalScrollbar().AppendTo<ScrollableContainer>(itemContainer, new float?(0.0f));
      this.m_assignedRecipesContainer = this.Builder.NewStackContainer("Assigned recipes").SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(this.Style.Panel.ItemsSpacing);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.m_assignedRecipesContainer);
      float size = (float) ((double) this.m_width / 2.0 - 50.0);
      float num = 100f;
      Txt enrichmentTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.NuclearReactor__EnrichmentTitle, new LocStrFormatted?((LocStrFormatted) Tr.NuclearReactor__EnrichmentTooltip));
      Panel container = this.Builder.NewPanel("Container").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      BufferView enrichmentInputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).PutToLeftOf<BufferView>((IUiElement) container, size);
      this.Builder.NewIconContainer("Arrow").SetIcon("Assets/Unity/UserInterface/General/Transform128.png", ColorRgba.White).PutToLeftTopOf<IconContainer>((IUiElement) container, 22.Vector2(), Offset.Left(size + (float) (((double) num - 40.0) / 2.0)) + Offset.Top(5f));
      TextWithIcon quantityPerDuration = new TextWithIcon(this.Builder, 16);
      quantityPerDuration.SetTextStyle(this.Builder.Style.Global.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      quantityPerDuration.PutToCenterTopOf<TextWithIcon>((IUiElement) container, new Vector2(0.0f, 25f), Offset.Top(25f));
      BufferView enrichmentOutputBuffer = this.Builder.NewBufferView((IUiElement) enrichmentInputBuffer, isCompact: true).PutToLeftOf<BufferView>((IUiElement) container, size, Offset.Left(size + num));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.EnrichmentInputBuffer.ValueOrNull?.Product ?? (ProductProto) null)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.EnrichmentInputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.EnrichmentInputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) =>
      {
        itemContainer.SetItemVisibility((IUiElement) enrichmentTitle, (Proto) product != (Proto) null);
        itemContainer.SetItemVisibility((IUiElement) container, (Proto) product != (Proto) null);
        if (!((Proto) product != (Proto) null))
          return;
        enrichmentInputBuffer.UpdateState(product, capacity, quantity);
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.EnrichmentOutputBuffer.ValueOrNull?.Product ?? (ProductProto) null)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.EnrichmentOutputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.EnrichmentOutputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) =>
      {
        if (!((Proto) product != (Proto) null))
          return;
        enrichmentOutputBuffer.UpdateState(product, capacity, quantity);
      }));
      updaterBuilder.Observe<NuclearReactorProto>((Func<NuclearReactorProto>) (() => this.Entity.Prototype)).Observe<IRecipeForUi>((Func<IRecipeForUi>) (() => this.Entity.ActiveRecipes.FirstOrDefault())).Observe<bool>((Func<bool>) (() => this.Builder.DurationNormalizer.IsNormalizationOn)).Do((Action<NuclearReactorProto, IRecipeForUi, bool>) ((proto, activeRecipe, isNormalizationOn) =>
      {
        if (!proto.Enrichment.HasValue)
          return;
        quantityPerDuration.SetVisibility<TextWithIcon>(activeRecipe != null);
        if (activeRecipe == null)
          return;
        PartialQuantity processedPerLevel = proto.Enrichment.Value.ProcessedPerLevel;
        if (isNormalizationOn)
        {
          Fix32 fix32 = 60.Seconds().Ticks.ToFix32() / activeRecipe.Duration.Ticks;
          processedPerLevel *= fix32;
        }
        Duration duration = isNormalizationOn ? 60.Seconds() : activeRecipe.Duration;
        quantityPerDuration.SetPrefixText(string.Format("{0} / {1}", (object) processedPerLevel.ToStringRounded(), (object) duration.Seconds.ToIntRounded()));
      }));
      Txt inputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.NuclearReactor__EmergencyCoolingTitle, new LocStrFormatted?((LocStrFormatted) Tr.NuclearReactor__EmergencyCoolingTooltip));
      BufferView coolantInBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new Vector2?(new Vector2(this.m_width / 2f, this.Style.BufferView.SuperCompactHeight)), ContainerPosition.LeftOrTop);
      BufferView coolantOutBuffer = this.Builder.NewBufferView((IUiElement) coolantInBuffer, isCompact: true).SetAsSuperCompact().PutToLeftOf<BufferView>((IUiElement) coolantInBuffer, this.m_width / 2f, Offset.Left(this.m_width / 2f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.CoolantInBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantInBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantInBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) =>
      {
        coolantInBuffer.UpdateState(product, capacity, quantity);
        itemContainer.SetItemVisibility((IUiElement) coolantInBuffer, capacity.IsPositive);
        itemContainer.SetItemVisibility((IUiElement) inputsTitle, capacity.IsPositive);
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.CoolantOutBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantOutBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantOutBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => coolantOutBuffer.UpdateState(product, capacity, quantity)));
      updaterBuilder.Observe<IRecipeForUi>((Func<ImmutableArray<IRecipeForUi>>) (() => this.Entity.AllRecipes), (ICollectionComparator<IRecipeForUi, ImmutableArray<IRecipeForUi>>) CompareFixedOrder<IRecipeForUi>.Instance).Observe<ImmutableArray<IRecipeForUi>>((Func<ImmutableArray<IRecipeForUi>>) (() => this.Entity.ActiveRecipes)).Do((Action<Lyst<IRecipeForUi>, ImmutableArray<IRecipeForUi>>) ((allRecipes, activeCnt) => this.updateRecipes(allRecipes)));
      updaterBuilder.Observe<NuclearReactor.State>((Func<NuclearReactor.State>) (() => this.Entity.CurrentState)).Do(new Action<NuclearReactor.State>(this.updateStatusInfo));
      this.AddUpdater(this.m_updater = updaterBuilder.Build());
    }

    private void sliderValueChange(float value)
    {
      int powerLevel = (int) value;
      Assert.That<int>(powerLevel).IsWithinIncl(0, this.Entity.MaxPowerLevel);
      this.m_controller.Context.InputScheduler.ScheduleInputCmd<NuclearReactorSetPowerLevelCmd>(new NuclearReactorSetPowerLevelCmd(this.Entity, powerLevel));
    }

    private void recipeClicked(IRecipeForUi recipeForUi)
    {
      this.m_controller.InputScheduler.ScheduleInputCmd<NuclearReactorToggleAllowedFuelCmd>(new NuclearReactorToggleAllowedFuelCmd(this.Entity, new ProductProto.ID(recipeForUi.Id.Value)));
    }

    private void updateStatusInfo(NuclearReactor.State state)
    {
      switch (state)
      {
        case NuclearReactor.State.None:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
        case NuclearReactor.State.Broken:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.Paused:
          this.m_statusInfo.SetStatusPaused();
          break;
        case NuclearReactor.State.Meltdown:
          this.m_statusInfo.SetStatus(Tr.EntityStatus___NuclearReactor_Overheated, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.NotEnoughWorkers:
          this.m_statusInfo.SetStatusNoWorkers();
          break;
        case NuclearReactor.State.NotEnoughComputing:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__NoComputing, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.NotEnoughMaintenance:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.NotEnoughInput:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.OutputFull:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.NoRecipes:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__NoRecipe, StatusPanel.State.Critical);
          break;
        case NuclearReactor.State.Idle:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
        case NuclearReactor.State.Working:
          this.m_statusInfo.SetStatusWorking();
          break;
      }
    }

    private void updateRecipes(Lyst<IRecipeForUi> allRecipes)
    {
      this.m_assignedRecipesContainer.StartBatchOperation();
      this.m_assignedRecipes.ForEachAndClear((Action<RecipeView>) (x =>
      {
        this.m_updater.RemoveChildUpdater(x.Updater.Value);
        this.m_updater.RemoveChildUpdater(x.BuffersUpdater.Value);
        this.m_updater.RemoveChildUpdater(x.DurationUpdater.Value);
        x.Hide<RecipeView>();
      }));
      this.m_assignedRecipesContainer.ClearAll();
      float self = 0.0f;
      foreach (IRecipeForUi allRecipe in allRecipes)
      {
        RecipeView view = this.m_viewsCache.GetView(allRecipe);
        view.AppendTo<RecipeView>(this.m_assignedRecipesContainer, new float?(this.Style.RecipeDetail.Height), new Offset(1f, 0.0f, 1f, 0.0f));
        view.SetRecipeActive(this.Entity.ActiveRecipes.Contains(allRecipe));
        view.Show<RecipeView>();
        self = self.Max(view.GetDynamicWidth());
        this.m_assignedRecipes.Add(view);
        this.m_updater.AddChildUpdater(view.BuildProgressUpdater((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity)));
        this.m_updater.AddChildUpdater(view.BuildBufferViewUpdaters((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity)));
        this.m_updater.AddChildUpdater(view.BuildDurationUpdater((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity)));
      }
      foreach (IRecipeForUi allRecipe in allRecipes)
      {
        RecipeView view = this.m_viewsCache.GetView(allRecipe);
        view.SetLeftIndent(self - view.GetDynamicWidth());
      }
      this.m_assignedRecipesContainer.FinishBatchOperation();
      this.SetWidth(this.m_width.Max(self + 16f));
      this.updateLayout(allRecipes.Count);
    }

    private void updateLayout(int recipesCount)
    {
      float num = this.Style.RecipeDetail.Height + this.Style.Panel.ItemsSpacing;
      float a = (float) recipesCount * num;
      float self = Mathf.Min(a, num * 4.5f);
      if ((double) self < (double) a)
        this.m_assignedRecipesContainer.SetInnerPadding(Offset.Right(16f));
      else
        this.m_assignedRecipesContainer.SetInnerPadding(Offset.Zero);
      this.ItemsContainer.UpdateItemHeight((IUiElement) this.m_scrollableContainer, self.Max(num));
    }
  }
}
