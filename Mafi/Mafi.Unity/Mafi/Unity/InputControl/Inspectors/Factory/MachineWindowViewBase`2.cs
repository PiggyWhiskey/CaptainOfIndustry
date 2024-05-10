// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.MachineWindowViewBase`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.WellPumps;
using Mafi.Core.Population;
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
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal abstract class MachineWindowViewBase<TEntity, TView> : StaticEntityInspectorBase<TEntity>
    where TEntity : Machine
    where TView : ItemDetailWindowView
  {
    private RecipeView.Cache m_viewsCache;
    private StackContainer m_assignedRecipesContainer;
    private ScrollableContainer m_scrollableContainer;
    private IUiUpdater m_recipesUpdater;
    private readonly EntityInspector<TEntity, TView> m_controller;
    private StatusPanel m_statusInfo;
    private Tooltip m_statusTooltip;
    private Machine.State m_lastSeenStatus;
    private readonly StringBuilder m_sb;
    private readonly Set<ProductProto> m_missingProducts;
    private IVirtualResourceMiningEntity m_lastSeenVirtualMiningEntity;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly float m_minWidth;
    private Lyst<RecipeProto> m_unlockedRecipesCache;

    protected override TEntity Entity => this.m_controller.SelectedEntity;

    public MachineWindowViewBase(
      EntityInspector<TEntity, TView> controller,
      UnlockedProtosDbForUi unlockedProtosDb,
      float minWidth = 480f)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_missingProducts = new Set<ProductProto>();
      this.m_unlockedRecipesCache = new Lyst<RecipeProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<EntityInspector<TEntity, TView>>();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_minWidth = minWidth;
      this.m_sb = new StringBuilder();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(this.m_controller.Clear));
      this.AddUnityCostPanel(updaterBuilder, (Func<IUnityConsumingEntity>) (() => (IUnityConsumingEntity) this.Entity));
      this.m_statusInfo = this.AddStatusInfoPanel();
      this.m_statusInfo.SetOnMouseEnterLeaveActions(new Action(this.onStatusMouseEnter), new Action(this.onStatusMouseLeave));
      this.m_statusTooltip = new Tooltip(this.Builder);
      this.m_statusTooltip.AttachToNoEvents((IUiElement) this.m_statusInfo);
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      AlertIndicator alertIndicator = this.GetOrAddAlertIndicator(this.m_controller.Context);
      alertIndicator.SetIcon("Assets/Unity/UserInterface/General/Speed.svg");
      alertIndicator.HideTooltipIcon();
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => this.Entity.GetSlowDownMessageForUi())).Do((Action<LocStrFormatted>) (msg =>
      {
        bool isNotEmpty = msg.IsNotEmpty;
        this.SetAlertIndicatorVisibility(alertIndicator, isNotEmpty);
        if (!isNotEmpty)
          return;
        alertIndicator.SetMessage(msg.Value);
      }));
      StackContainer parent = itemContainer;
      LocStrFormatted recipes = (LocStrFormatted) Tr.Recipes;
      Offset? nullable = new Offset?(Offset.Bottom(5f));
      LocStrFormatted? tooltip = new LocStrFormatted?();
      Offset? extraOffset = nullable;
      this.Builder.DurationNormalizer.AttachPer60ToggleToTitle(this.AddSectionTitle(parent, recipes, tooltip, extraOffset), this.Builder, updaterBuilder);
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("Recipes scroll").AddVerticalScrollbar().AppendTo<ScrollableContainer>(itemContainer, new float?(0.0f));
      this.m_assignedRecipesContainer = this.Builder.NewStackContainer("Assigned recipes").SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(this.Style.Panel.ItemsSpacing);
      this.m_viewsCache = new RecipeView.Cache((IUiElement) this.m_assignedRecipesContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, new Action<IRecipeForUi>(this.recipeClicked), true);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.m_assignedRecipesContainer);
      CostButton boostBtn = new CostButton(this.Builder, Tr.BoostMachine__Enable.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg");
      boostBtn.SetSuffix(string.Format("/ {0}", (object) Tr.OneMonth));
      boostBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<MachineBoostToggleCmd>(new MachineBoostToggleCmd(this.Entity.Id)))).AddToolTip(Tr.BoostMachine__Tooltip).AppendTo<Btn>(this.ItemsContainer, new Vector2?(boostBtn.GetSize()), ContainerPosition.MiddleOrCenter, Offset.Top(this.Style.Panel.ItemsSpacing));
      Btn boostDisableBtn = this.Builder.NewBtn("RemoveBoost").SetButtonStyle(this.Builder.Style.Global.UpointsBtnActive).SetText(Tr.BoostMachine__Disable.TranslatedString).EnableDynamicSize().AddToolTip(Tr.BoostMachine__Tooltip).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<MachineBoostToggleCmd>(new MachineBoostToggleCmd(this.Entity.Id))));
      Vector2 optimalSize = boostDisableBtn.GetOptimalSize();
      boostDisableBtn.AppendTo<Btn>(this.ItemsContainer, new Vector2?(new Vector2(optimalSize.x.Max(boostBtn.GetWidth()), optimalSize.y.Max(boostBtn.GetHeight()))), ContainerPosition.MiddleOrCenter, Offset.Top(this.Style.Panel.ItemsSpacing));
      updaterBuilder.Observe<Upoints?>((Func<Upoints?>) (() => this.Entity.BoostCost)).Observe<bool>((Func<bool>) (() => this.Entity.IsBoostRequested)).Do((Action<Upoints?, bool>) ((boostCost, isBoosted) =>
      {
        if (boostCost.HasValue)
        {
          boostBtn.SetCost(boostCost.ToString());
          itemContainer.UpdateItemSize((IUiElement) boostBtn, boostBtn.GetSize());
        }
        this.ItemsContainer.SetItemVisibility((IUiElement) boostDisableBtn, isBoosted);
        this.ItemsContainer.SetItemVisibility((IUiElement) boostBtn, boostCost.HasValue && !isBoosted);
      }));
      Txt virtualResourceTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ReserveStatus);
      Tooltip virtualResourceTooltip = this.Builder.AddTooltipForTitle(virtualResourceTitle);
      BufferView virtualResourceBuffer = this.AddBufferView(this.Style.BufferView.Height);
      updaterBuilder.Observe<TEntity>((Func<TEntity>) (() => this.Entity)).Do((Action<TEntity>) (machine =>
      {
        this.m_lastSeenVirtualMiningEntity = (object) machine as IVirtualResourceMiningEntity;
        if (this.m_lastSeenVirtualMiningEntity != null)
          virtualResourceTooltip.SetText(this.m_lastSeenVirtualMiningEntity.Description);
        this.ItemsContainer.StartBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) virtualResourceBuffer, this.m_lastSeenVirtualMiningEntity != null);
        this.ItemsContainer.SetItemVisibility((IUiElement) virtualResourceTitle, this.m_lastSeenVirtualMiningEntity != null);
        this.ItemsContainer.FinishBatchOperation();
      }));
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => Option.Create<ProductProto>(this.m_lastSeenVirtualMiningEntity?.ProductToMine))).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IVirtualResourceMiningEntity virtualMiningEntity = this.m_lastSeenVirtualMiningEntity;
        return virtualMiningEntity == null ? Quantity.Zero : virtualMiningEntity.CapacityOfMine;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IVirtualResourceMiningEntity virtualMiningEntity = this.m_lastSeenVirtualMiningEntity;
        return virtualMiningEntity == null ? Quantity.Zero : virtualMiningEntity.QuantityLeftToMine;
      })).Do(new Action<Option<ProductProto>, Quantity, Quantity>(virtualResourceBuffer.UpdateState));
      SwitchBtn switchBtn = this.Builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.NotifyOnLowReserve).SetOnToggleAction((Action<bool>) (isEnabled => this.m_controller.Context.InputScheduler.ScheduleInputCmd<WellPumpAlertSetEnabledCmd>(new WellPumpAlertSetEnabledCmd(this.Entity.Id, isEnabled))));
      switchBtn.PutToRightOf<SwitchBtn>((IUiElement) virtualResourceTitle, switchBtn.GetWidth(), Offset.Right(10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        IVirtualResourceMiningEntity virtualMiningEntity = this.m_lastSeenVirtualMiningEntity;
        return virtualMiningEntity != null && virtualMiningEntity.NotifyOnLowReserve;
      })).Do((Action<bool>) (notifyOn => switchBtn.SetIsOn(notifyOn)));
      updaterBuilder.Observe<MachineProto>((Func<MachineProto>) (() => this.Entity.Prototype)).Observe<RecipeProto>((Func<IIndexable<RecipeProto>>) (() => this.Entity.RecipesAssigned), (ICollectionComparator<RecipeProto, IIndexable<RecipeProto>>) CompareFixedOrder<RecipeProto>.Instance).Do((Action<MachineProto, Lyst<RecipeProto>>) ((machineProto, assignedRecipes) => this.updateRecipes(machineProto, (IIndexable<RecipeProto>) assignedRecipes)));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.Utilization)).Observe<Machine.State>((Func<Machine.State>) (() => this.Entity.CurrentState)).Observe<Percent>((Func<Percent>) (() => this.Entity.SpeedFactor)).Do(new Action<Percent, Machine.State, Percent>(this.updateStatusInfo));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(this.m_recipesUpdater = UpdaterBuilder.Start().Build());
    }

    private void onStatusMouseEnter()
    {
      if (this.m_lastSeenStatus != Machine.State.NotEnoughInput)
        return;
      this.Entity.GetAllMissingInputs(this.m_missingProducts);
      if (!this.m_missingProducts.IsNotEmpty)
        return;
      this.m_sb.Clear();
      this.m_sb.AppendLine(string.Format("{0}:", (object) Tr.EntityStatus__WaitingForProductsTooltip));
      foreach (ProductProto missingProduct in this.m_missingProducts)
      {
        this.m_sb.Append(" -");
        this.m_sb.AppendLine(missingProduct.Strings.Name.TranslatedString);
      }
      this.m_statusTooltip.SetText(this.m_sb.ToString());
      this.m_statusTooltip.OnParentMouseEnter();
    }

    private void onStatusMouseLeave() => this.m_statusTooltip.OnParentMouseLeave();

    private void recipeClicked(IRecipeForUi recipeForUi)
    {
      this.m_controller.InputScheduler.ScheduleInputCmd<MachineToggleRecipeActiveCmd>(new MachineToggleRecipeActiveCmd(this.Entity.Id, new RecipeProto.ID(recipeForUi.Id.Value)));
    }

    private void updateStatusInfo(Percent utilization, Machine.State state, Percent speedFactor)
    {
      this.m_lastSeenStatus = state;
      switch (state)
      {
        case Machine.State.None:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
        case Machine.State.Broken:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
          break;
        case Machine.State.Paused:
          this.m_statusInfo.SetStatusPaused();
          break;
        case Machine.State.NotEnoughWorkers:
          this.m_statusInfo.SetStatusNoWorkers();
          break;
        case Machine.State.NotEnoughPower:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
          break;
        case Machine.State.NotEnoughComputing:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__NoComputing, StatusPanel.State.Critical);
          break;
        case Machine.State.NotEnoughInput:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Warning);
          break;
        case Machine.State.InvalidPlacement:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__InvalidPlacement, StatusPanel.State.Critical);
          break;
        case Machine.State.OutputFull:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Warning);
          break;
        case Machine.State.NoRecipes:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__NoRecipe, StatusPanel.State.Critical);
          break;
        case Machine.State.Working:
          if (utilization < Percent.Hundred)
          {
            this.m_statusInfo.SetStatus(Tr.EntityStatus__WorkingPartially.Format(utilization.ToStringRounded()).Value, StatusPanel.State.Warning);
            break;
          }
          if (!speedFactor.IsNearHundred)
          {
            this.m_statusInfo.SetStatus(Tr.EntityStatus__WorkingPartially.Format(speedFactor.ToStringRounded()).Value, StatusPanel.State.Warning);
            break;
          }
          this.m_statusInfo.SetStatusWorking();
          break;
      }
    }

    private void onArrowClick(IRecipeForUi recipe, bool isUp)
    {
      this.m_controller.Context.InputScheduler.ScheduleInputCmd<ReorderRecipeCmd>(new ReorderRecipeCmd(this.Entity.Id, recipe.Id, isUp ? -1 : 1));
    }

    private void updateRecipes(MachineProto machineProto, IIndexable<RecipeProto> assignedRecipes)
    {
      this.m_assignedRecipesContainer.StartBatchOperation();
      this.m_recipesUpdater.ClearAllChildUpdaters();
      this.m_assignedRecipesContainer.ClearAll(true);
      float maxRecipeWidth = 0.0f;
      int num = 0;
      foreach (RecipeProto assignedRecipe in assignedRecipes)
      {
        RecipeView recipeView = addRecipe((IRecipeForUi) assignedRecipe, true);
        recipeView.UpArrow.Value.SetVisibility<Btn>(assignedRecipes.Count > 1 && num > 0);
        recipeView.DownArrow.Value.SetVisibility<Btn>(assignedRecipes.Count > 1 && num < assignedRecipes.Count - 1);
        ++num;
      }
      this.m_unlockedRecipesCache.Clear();
      foreach (RecipeProto recipe in machineProto.Recipes)
      {
        if (this.m_unlockedProtosDb.IsUnlocked((IProto) recipe))
          this.m_unlockedRecipesCache.Add(recipe);
      }
      foreach (RecipeProto recipe in this.m_unlockedRecipesCache)
      {
        if (!assignedRecipes.Contains<RecipeProto>(recipe))
        {
          RecipeView recipeView = addRecipe((IRecipeForUi) recipe, false);
          recipeView.UpArrow.Value.Hide<Btn>();
          recipeView.DownArrow.Value.Hide<Btn>();
        }
      }
      foreach (IRecipeForUi data in this.m_unlockedRecipesCache)
      {
        RecipeView view = this.m_viewsCache.GetView(data);
        view.SetLeftIndent(maxRecipeWidth - view.GetDynamicWidth());
      }
      this.m_assignedRecipesContainer.FinishBatchOperation();
      this.SetWidth(this.m_minWidth.Max(maxRecipeWidth + 16f));
      this.updateLayout(this.m_unlockedRecipesCache.Count);
      this.m_scrollableContainer.ResetToTop();

      RecipeView addRecipe(IRecipeForUi recipe, bool isActive)
      {
        RecipeView view = this.m_viewsCache.GetView(recipe);
        view.BuildMoveArrows(new Action<IRecipeForUi, bool>(this.onArrowClick));
        view.AppendTo<RecipeView>(this.m_assignedRecipesContainer, new float?(this.Style.RecipeDetail.Height), new Offset(1f, 0.0f, 1f, 0.0f));
        view.SetRecipeActive(isActive);
        view.Show<RecipeView>();
        maxRecipeWidth = maxRecipeWidth.Max(view.GetDynamicWidth());
        this.m_recipesUpdater.AddChildUpdater(view.BuildProgressUpdater((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity)));
        this.m_recipesUpdater.AddChildUpdater(view.BuildBufferViewUpdaters((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity), (Action<IRecipeForUi>) (rec => this.m_controller.Context.InputScheduler.ScheduleInputCmd<ClearRecipeProductsCmd>(new ClearRecipeProductsCmd(this.Entity.Id, rec.Id))), (IUpointsManager) this.m_controller.Context.UpointsManager));
        this.m_recipesUpdater.AddChildUpdater(view.BuildDurationUpdater((Func<IRecipeExecutorForUi>) (() => (IRecipeExecutorForUi) this.Entity)));
        return view;
      }
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
