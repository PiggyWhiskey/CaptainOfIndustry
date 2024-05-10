// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMineView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public class WorldMineView : ItemDetailWindowView
  {
    private readonly InspectorContext m_inspectorContext;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly Action<WorldMapLocation, LocationVisitReason> m_onGoToClick;
    private StatusPanel m_statusInfo;

    private WorldMapMine Entity { get; set; }

    public WorldMineView(
      InspectorContext inspectorContext,
      TravelingFleetManager fleetManager,
      Action onClose,
      Action<WorldMapLocation, LocationVisitReason> onGoToClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorldMapEntityInspector", false);
      this.m_inspectorContext = inspectorContext;
      this.m_fleetManager = fleetManager;
      this.m_onGoToClick = onGoToClick;
      this.SetOnCloseButtonClickAction(onClose);
      this.EnableClippingPrevention();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      Tooltip tooltip = this.AddHelpButton();
      this.MakeMovable();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddTitleRenameButton((Func<IEntityWithCustomTitle>) (() => (IEntityWithCustomTitle) this.Entity), this.m_inspectorContext.InputScheduler);
      this.AddEnableToggleButton((Action<bool>) (isPaused => this.m_inspectorContext.InputScheduler.ScheduleInputCmd<ToggleEnabledCmd>(new ToggleEnabledCmd(this.Entity.Id))), updaterBuilder, (Func<bool>) (() => !this.Entity.IsPaused));
      this.AddUnityCostPanel(updaterBuilder, (Func<IUnityConsumingEntity>) (() => (IUnityConsumingEntity) this.Entity));
      this.AddWorkersPanel(updaterBuilder, (Func<IEntityWithWorkers>) (() => (IEntityWithWorkers) this.Entity));
      this.m_statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<string>((Func<string>) (() => this.Entity.GetTitle())).Observe<bool>((Func<bool>) (() => this.Entity.IsPaused)).Observe<bool>((Func<bool>) (() => this.Entity.Maintenance.Status.IsBroken)).Do((Action<string, bool, bool>) ((title, isPaused, isBroken) =>
      {
        this.SetTitle(title, isPaused, isBroken);
        this.m_headerText.SetWidth<Txt>(this.m_headerText.GetPreferedWidth().Min(this.m_headerHolder.GetWidth() - 22f));
      }));
      this.AddGeneralPriorityPanel(this.m_inspectorContext, (Func<IEntityWithGeneralPriority>) (() => (IEntityWithGeneralPriority) this.Entity));
      Txt sliderTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) TrCore.WorldMineProductionLvl__Title, new LocStrFormatted?((LocStrFormatted) TrCore.WorldMineProductionLvl__Tooltip));
      BufferViewOneSlider levelSlider = this.Builder.NewBufferWithOneSlider((IUiElement) this.ItemsContainer, new Action<float>(this.sliderValueChange), 1, "", customColor: new ColorRgba?((ColorRgba) 16755968)).AppendTo<BufferViewOneSlider>(this.ItemsContainer, new float?(this.Style.BufferView.HeightWithSlider));
      levelSlider.UpdateState(Option<ProductProto>.None, Percent.Zero, LocStrFormatted.Empty);
      levelSlider.SetLabelFunc((Func<int, string>) (step => string.Format("{0}x", (object) step)));
      TextWithIcon output = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.Title).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      levelSlider.ReplaceProductTextWith((IUiElement) output);
      updaterBuilder.Observe<WorldMapMineProto>((Func<WorldMapMineProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.Entity.ProductionStep)).Observe<int>((Func<int>) (() => this.Entity.MaxProductionSteps)).Observe<bool>((Func<bool>) (() => this.Builder.DurationNormalizer.IsNormalizationOn)).Do((Action<WorldMapMineProto, int, int, bool>) ((proto, shiftsCount, maxShifts, normalize) =>
      {
        ProductQuantity producedProductPerStep = proto.ProducedProductPerStep;
        Assert.That<int>(shiftsCount).IsNotNegative();
        levelSlider.SetMaxSteps(maxShifts);
        levelSlider.SetCustomIcon(producedProductPerStep.Product.Graphics.IconPath);
        levelSlider.UpdateSlider(shiftsCount);
        output.SetPrefixText(this.Builder.DurationNormalizer.NormalizeQuantityAsString(producedProductPerStep.Product.WithQuantity(producedProductPerStep.Quantity * shiftsCount), proto.ProductionDuration) + " / " + (normalize ? 60.Seconds() : proto.ProductionDuration).Seconds.ToStringRounded(0));
      }));
      Panel productionPenaltyPanel = this.AddOverlayPanel(itemContainer, 55);
      Txt penaltyTxt = this.Builder.NewTxt("PenaltyInfo").SetTextStyle(this.Style.Global.TextInc.Extend(new ColorRgba?(this.Style.Global.OrangeText))).SetAlignment(TextAnchor.MiddleCenter).EnableRichText().PutTo<Txt>((IUiElement) productionPenaltyPanel, Offset.LeftRight(10f));
      updaterBuilder.Observe<WorldMapMineProto>((Func<WorldMapMineProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.Entity.ProductionStep)).Observe<Percent>((Func<Percent>) (() => this.Entity.GetProductionPenalty())).Do((Action<WorldMapMineProto, int, Percent>) ((proto, shiftsCount, penalty) =>
      {
        itemContainer.SetItemVisibility((IUiElement) productionPenaltyPanel, penalty.IsPositive && shiftsCount > 0);
        if (!penalty.IsPositive)
          return;
        Quantity quantity = (proto.ProducedProductPerStep.Quantity * shiftsCount).ScaledBy(penalty.InverseTo100());
        penaltyTxt.SetText(Tr.WorldMine_ReducedOutput.Format("<b>" + penalty.ToStringRounded() + "</b>", string.Format("<b>{0}</b>", (object) quantity)));
      }));
      SimpleProgressBar progressBar = new SimpleProgressBar((IUiElement) levelSlider, this.Builder).SetBackgroundColor(ColorRgba.Black).AppendTo<SimpleProgressBar>(itemContainer, new float?(5f));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.ProgressDone)).Observe<bool>((Func<bool>) (() => this.Entity.WorkedLastTick)).Do((Action<Percent, bool>) ((progress, worked) =>
      {
        progressBar.SetProgress(progress);
        progressBar.SetColor(worked ? this.Builder.Style.Global.GreenForDark : this.Builder.Style.Global.OrangeText);
      }));
      Txt bufferTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.StoredProduct__Title, new LocStrFormatted?((LocStrFormatted) Tr.StoredProduct__WorldMapTooltip));
      BufferView buffer = this.AddBufferView(this.Style.BufferView.SuperCompactHeight, isCompact: true).SetAsSuperCompact();
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Buffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(buffer.UpdateState));
      Txt availableQuantityTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ReserveStatus);
      this.Builder.AddTooltipForTitle(availableQuantityTitle).SetText((LocStrFormatted) Tr.WorldMine_ReserveTooltip);
      BufferView availableQuantityBuffer = this.AddBufferView(this.Style.BufferView.CompactHeight, isCompact: true);
      Txt estimateLeft = this.Builder.NewTxt("EstimateLeft").SetTextStyle(this.Style.Panel.Text).SetAlignment(TextAnchor.MiddleLeft).PutToLeftBottomOf<Txt>((IUiElement) availableQuantityBuffer, new Vector2(200f, 25f), Offset.Left(118f));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) estimateLeft, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.WorldMine_ReserveEstimate__Tooltip);
      updaterBuilder.Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.Prototype.QuantityAvailable)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.QuantityAvailable)).Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Product)).Observe<bool>((Func<bool>) (() => this.Entity.IsRepaired)).Do((Action<Quantity?, Quantity?, ProductProto, bool>) ((capacity, quantityLeft, product, isRepaired) =>
      {
        if (capacity.HasValue && quantityLeft.HasValue)
          availableQuantityBuffer.UpdateState(product, capacity.Value, quantityLeft.Value);
        this.ItemsContainer.StartBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) availableQuantityBuffer, isRepaired && capacity.HasValue);
        this.ItemsContainer.SetItemVisibility((IUiElement) availableQuantityTitle, isRepaired && capacity.HasValue);
        this.ItemsContainer.FinishBatchOperation();
      }));
      updaterBuilder.Observe<WorldMapMineProto>((Func<WorldMapMineProto>) (() => this.Entity.Prototype)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.QuantityAvailable)).Observe<int>((Func<int>) (() => this.Entity.ProductionStep)).Do((Action<WorldMapMineProto, Quantity?, int>) ((proto, left, step) =>
      {
        if (left.HasValue)
        {
          if (left.Value == Quantity.Zero)
            estimateLeft.SetText(Tr.WorldMine_ReserveEstimate.Format("0"));
          else if (step > 0)
          {
            Fix64 fix64_1 = 1.Years().Ticks / proto.ProductionDuration.Ticks.ToFix64();
            Fix64 fix64_2 = left.Value.Value / (step * proto.ProducedProductPerStep.Quantity.Value * fix64_1);
            estimateLeft.SetText(Tr.WorldMine_ReserveEstimate.Format(fix64_2.ToStringRounded(1)));
          }
          else
            estimateLeft.SetText("");
        }
        estimateLeft.SetVisibility<Txt>(left.HasValue);
      }));
      WorldMapRepairView repairPanel = new WorldMapRepairView(this.Builder, this.m_inspectorContext.InputScheduler, (Func<WorldMapRepairableEntity>) (() => (WorldMapRepairableEntity) this.Entity));
      repairPanel.AppendTo<WorldMapRepairView>(this.ItemsContainer, new float?(repairPanel.GetHeight()));
      repairPanel.SetButtonText(TrCore.StartRepairs, new LocStr?(Tr.StartRepairs__Tooltip));
      this.AddUpdater(repairPanel.Updater);
      Txt upgradeProgressTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.UpgradeInProgress);
      ConstructionProgressView upgradeProgressView = new ConstructionProgressView((IUiElement) this.ItemsContainer, this.Builder, (Func<Option<IConstructionProgress>>) (() => this.Entity.ConstructionProgress));
      upgradeProgressView.AppendTo<ConstructionProgressView>(this.ItemsContainer, new float?(95f)).SetBackground(this.Builder.Style.Panel.ItemOverlay);
      this.AddUpdater(upgradeProgressView.Updater);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsBeingUpgraded)).Do((Action<bool>) (isBeingUpgraded =>
      {
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeProgressView, isBeingUpgraded);
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeProgressTitle, isBeingUpgraded);
      }));
      WorldEntityShipOrdersView entityShipOrdersView = new WorldEntityShipOrdersView(this.Builder, this.m_fleetManager, (Func<WorldMapRepairableEntity>) (() => (WorldMapRepairableEntity) this.Entity), this.m_onGoToClick);
      entityShipOrdersView.AppendTo<WorldEntityShipOrdersView>(this.ItemsContainer, new float?(entityShipOrdersView.GetHeight()));
      this.AddUpdater(entityShipOrdersView.Updater);
      Pair<Txt, BufferView> maintenancePanel = this.AddMaintenancePanel(updaterBuilder, (Func<IMaintainedEntity>) (() => (IMaintainedEntity) this.Entity));
      EntityUpgradeView upgradeView = new EntityUpgradeView(new Action(upgradeAction), this.m_inspectorContext.AssetsManager);
      upgradeView.Build((IUiElement) this.ItemsContainer, this.Builder).AppendTo<EntityUpgradeView>(this.ItemsContainer, new float?(upgradeView.Height), Offset.Top(10f));
      this.AddUpdater(upgradeView.CreateUpdater((Func<IUpgradableWorldEntity>) (() => (IUpgradableWorldEntity) this.Entity), (Action<bool>) (isVisible => this.ItemsContainer.SetItemVisibility((IUiElement) upgradeView, isVisible))));
      updaterBuilder.Observe<WorldMapMineProto>((Func<WorldMapMineProto>) (() => this.Entity.Prototype)).Observe<bool>((Func<bool>) (() => this.Entity.IsRepaired)).Do((Action<WorldMapMineProto, bool>) ((proto, isRepaired) =>
      {
        tooltip.SetText(!isRepaired ? Tr.WorldMineInfo__NeedsRepair.Format(proto.Strings.Name.TranslatedString) : Tr.WorldMineInfo__ProvidesResources.Format(proto.Strings.Name.TranslatedString));
        this.ItemsContainer.StartBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) repairPanel, !isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) levelSlider, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) progressBar, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) sliderTitle, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) bufferTitle, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) buffer, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) maintenancePanel.First, isRepaired);
        this.ItemsContainer.SetItemVisibility((IUiElement) maintenancePanel.Second, isRepaired);
        this.SetTopButtonsVisibility(isRepaired);
        this.ItemsContainer.FinishBatchOperation();
      }));
      updaterBuilder.Observe<WorldMapMine.State>((Func<WorldMapMine.State>) (() => this.Entity.CurrentState)).Observe<Percent>((Func<Percent>) (() => this.Entity.GetProductionPenalty())).Do(new Action<WorldMapMine.State, Percent>(this.updateStatusInfo));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth(400f);

      void upgradeAction()
      {
        this.m_inspectorContext.InputScheduler.ScheduleInputCmd<WorldMapEntityUpgradeCmd>(new WorldMapEntityUpgradeCmd(this.Entity.Id));
      }
    }

    private void sliderValueChange(float value)
    {
      this.m_inspectorContext.InputScheduler.ScheduleInputCmd<SetShiftsCountForMineCmd>(new SetShiftsCountForMineCmd(this.Entity.Id, (int) value));
    }

    private void updateStatusInfo(WorldMapMine.State state, Percent productionPenalty)
    {
      switch (state)
      {
        case WorldMapMine.State.None:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
        case WorldMapMine.State.Broken:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
          break;
        case WorldMapMine.State.Paused:
          this.m_statusInfo.SetStatusPaused();
          break;
        case WorldMapMine.State.NotEnoughWorkers:
          this.m_statusInfo.SetStatusNoWorkers();
          break;
        case WorldMapMine.State.NotEnoughUnity:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__NoUnity, StatusPanel.State.Critical);
          break;
        case WorldMapMine.State.ResourceDepleted:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__ResourceDepleted, StatusPanel.State.Critical);
          break;
        case WorldMapMine.State.FullStorage:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__FullStorage, StatusPanel.State.Critical);
          break;
        case WorldMapMine.State.Working:
          if (productionPenalty.IsPositive)
          {
            this.m_statusInfo.SetStatus(Tr.EntityStatus__WorkingPartially.Format(productionPenalty.InverseTo100().ToStringRounded()).Value, StatusPanel.State.Warning);
            break;
          }
          this.m_statusInfo.SetStatusWorking();
          break;
      }
    }

    public void SetEntity(WorldMapMine entity) => this.Entity = entity;

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.InputUpdateForRenaming();
    }
  }
}
