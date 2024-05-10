// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.ItemDetailWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Input;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.Inspectors.Vehicles;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Components.VehiclesAssigner;
using Mafi.Unity.UserInterface.Style;
using Mafi.Unity.Vehicles;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  public abstract class ItemDetailWindowView : 
    WindowView,
    IWindowWithInnerWindowsSupport,
    IWindow,
    IDynamicSizeElement,
    IUiElement
  {
    private static readonly Offset[] s_windowOffsets;
    private ItemDetailWindowView.WindowOffsetGroup m_windowOffsetGroup;
    private static readonly string PAUSED_SUFFIX;
    private static readonly string BROKEN_SUFFIX;
    protected const float DEFAULT_WINDOW_WIDTH = 400f;
    private readonly bool m_isOnMainCanvas;
    private Panel m_topButtonsContainer;
    private StackContainer m_leftButtonsContainer;
    private StackContainer m_rightButtonsContainer;
    private Option<ToggleBtn> m_enableToggleButton;
    protected float Width;
    private Option<AlertIndicatorNotification> m_alertIndicatorNotification;
    private Option<AlertIndicator> m_alertIndicatorGeneral;
    private Option<IWindow> m_sidePanel;
    protected static int VEHICLE_BTN_SIZE;
    private Option<StackContainer> m_alertsContainer;
    private bool m_isInRenamingSession;
    private Btn m_textEditBtn;
    private Func<IEntityWithCustomTitle> m_getEntityFn;
    private TxtField m_txtInput;
    private Btn m_textSaveBtn;
    private IInputScheduler m_inputScheduler;
    private Option<ScrollableStackContainer> m_scrollableStackContainer;

    public StackContainer ItemsContainer { get; private set; }

    protected ItemDetailWindowView(string id, bool isOnMainCanvas = true)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Width = 400f;
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_isOnMainCanvas = isOnMainCanvas;
      this.ShowAfterSync = true;
    }

    protected abstract void AddCustomItems(StackContainer itemContainer);

    protected virtual void AddCustomItemsEnd(StackContainer itemContainer)
    {
    }

    protected override void BuildWindowContent()
    {
      this.ItemsContainer = this.Builder.NewStackContainer("Items container", (IUiElement) this.GetContentPanel()).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.Top(this.Style.Panel.Padding));
      this.ItemsContainer.SizeChanged += (Action<IUiElement>) (e =>
      {
        if (!this.m_scrollableStackContainer.IsNone)
          return;
        this.SetContentSize(this.Width, this.ItemsContainer.GetDynamicHeight());
      });
      this.AddCustomItems(this.ItemsContainer);
      this.AddCustomItemsEnd(this.ItemsContainer);
      if (this.m_isOnMainCanvas)
      {
        this.OnShowStart += (Action) (() => this.PositionSelfToCenter(offset: ItemDetailWindowView.s_windowOffsets[(int) this.m_windowOffsetGroup]));
        this.MakeMovable((Action<Offset>) (o => ItemDetailWindowView.s_windowOffsets[(int) this.m_windowOffsetGroup] = o));
      }
      this.OnHide += (Action) (() =>
      {
        if (!this.m_isInRenamingSession)
          return;
        this.stopRenamingSession();
      });
    }

    protected override Option<IUiElement> GetParent(UiBuilder builder)
    {
      return this.m_isOnMainCanvas ? (Option<IUiElement>) (IUiElement) builder.MainCanvas : Option<IUiElement>.None;
    }

    protected void MakeScrollableWithHeightLimit()
    {
      this.m_scrollableStackContainer = (Option<ScrollableStackContainer>) new ScrollableStackContainer(this.Builder, this.ResolveWindowSize().y - 200f, this.ItemsContainer);
      this.m_scrollableStackContainer.Value.PutToTopOf<ScrollableStackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_scrollableStackContainer.Value.SizeChanged += (Action<IUiElement>) (e => this.SetContentSize(this.Width, e.GetHeight()));
    }

    protected float GetDynamicContentHeight()
    {
      return this.m_scrollableStackContainer.HasValue ? this.m_scrollableStackContainer.Value.GetHeight() : this.ItemsContainer.GetDynamicHeight();
    }

    protected void SetWindowOffsetGroup(ItemDetailWindowView.WindowOffsetGroup group)
    {
      this.m_windowOffsetGroup = group;
    }

    protected void SetupTopButtonsContainer()
    {
      if (this.m_topButtonsContainer != null)
        return;
      this.m_topButtonsContainer = this.AddTopButtonsContainer(this.ItemsContainer);
      this.m_leftButtonsContainer = this.AddLeftVerticalButtonsContainer((IUiElement) this.m_topButtonsContainer);
      this.m_rightButtonsContainer = this.AddRightVerticalButtonsContainer((IUiElement) this.m_topButtonsContainer);
    }

    protected void SetTopButtonsVisibility(bool isVisible)
    {
      if (this.m_topButtonsContainer == null)
        return;
      this.ItemsContainer.SetItemVisibility((IUiElement) this.m_topButtonsContainer, isVisible);
    }

    protected void AttachSidePanel(IWindow element)
    {
      this.m_sidePanel = element.SomeOption<IWindow>();
      element.PutToLeftTopOf<IWindow>((IUiElement) this, element.GetSize(), Offset.Left(-element.GetWidth()));
      element.SizeChanged += (Action<IUiElement>) (el => el.PutToLeftTopOf<IUiElement>((IUiElement) this, el.GetSize(), Offset.Left(-el.GetWidth())));
      if (!(element is WindowView windowView))
        return;
      windowView.MakeMovable(elementToMove: (IUiElement) this);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      if (this.m_sidePanel.HasValue)
        this.m_sidePanel.Value.RenderUpdate(gameTime);
      this.m_alertIndicatorNotification.ValueOrNull?.RenderUpdate();
      this.m_alertIndicatorGeneral.ValueOrNull?.RenderUpdate();
      base.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      if (this.m_sidePanel.HasValue)
        this.m_sidePanel.Value.SyncUpdate(gameTime);
      base.SyncUpdate(gameTime);
    }

    protected virtual void SetWidth(float width)
    {
      this.Width = width;
      this.SetContentSize(this.Width, this.GetDynamicContentHeight());
    }

    protected void AddEnableToggleButton(
      Action<bool> action,
      UpdaterBuilder updater,
      Func<bool> observeFrom)
    {
      this.AddEnableToggleButton(action);
      updater.Observe<bool>(observeFrom).Do((Action<bool>) (s => this.m_enableToggleButton.Value.SetIsOn(s)));
    }

    protected void AddEnableToggleButton(Action<bool> action)
    {
      Assert.That<Option<ToggleBtn>>(this.m_enableToggleButton).IsNone<ToggleBtn>("Enable toggle button was already added!");
      this.m_enableToggleButton = (Option<ToggleBtn>) this.Builder.NewToggleBtn("Enable toggle", (IUiElement) this.m_leftButtonsContainer).SetButtonStyleWhenOn(this.Style.Panel.HeaderButtonNegative).SetButtonStyleWhenOff(this.Style.Panel.HeaderButtonNegativeOn).SetBtnIcon(this.Style.Icons.Enable, new Offset?(Offset.All(6f))).SetOnToggleAction(action);
      this.AddHeaderButton((IUiElement) this.m_enableToggleButton.Value);
    }

    protected void SetEnableButtonVisibility(bool isVisible)
    {
      this.SetHeaderButtonVisibility((IUiElement) this.m_enableToggleButton.Value, isVisible);
    }

    protected Tooltip AddHelpButton()
    {
      Btn btn = this.Builder.NewBtn("Help", (IUiElement) this.m_leftButtonsContainer).SetButtonStyle(this.Style.Panel.HeaderButton).SetIcon("Assets/Unity/UserInterface/General/Info128.png");
      Tooltip tooltip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) btn);
      this.AddHeaderButton((IUiElement) btn);
      return tooltip;
    }

    protected Btn AddHeaderTutorialButton(InspectorContext context, Proto.ID messageId)
    {
      Btn btnElement = this.Builder.NewBtn("OpenTutorial", (IUiElement) this.m_leftButtonsContainer).SetButtonStyle(this.Builder.Style.Panel.HeaderButton).AddToolTip(Tr.OpenTutorial).OnClick((Action) (() => context.MessagesCenter.ShowMessage(messageId))).SetIcon("Assets/Unity/UserInterface/Toolbar/Tutorials.svg", new Offset?(Offset.All(3f)));
      this.AddHeaderButton((IUiElement) btnElement);
      return btnElement;
    }

    protected Btn AddElectricityInfoPanel(
      UpdaterBuilder updaterBuilder,
      Func<IElectricityConsumingEntity> entityProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Electricity", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Consuming).SetText("").SetIcon(this.Style.Panel.ElectricityInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      Tooltip btnTooltip = btn.AddToolTipAndReturn();
      updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => entityProvider().PowerRequired)).Observe<bool>((Func<bool>) (() =>
      {
        IElectricityConsumerReadonly valueOrNull = entityProvider().ElectricityConsumer.ValueOrNull;
        return valueOrNull != null && valueOrNull.DidConsumeLastTick;
      })).Observe<bool>((Func<bool>) (() =>
      {
        IElectricityConsumerReadonly valueOrNull = entityProvider().ElectricityConsumer.ValueOrNull;
        return valueOrNull != null && valueOrNull.NotEnoughPower;
      })).Do((Action<Electricity, bool, bool>) ((el, consumedLastTick, notEnoughPower) =>
      {
        if (el.IsPositive)
        {
          btn.SetText(el.Format());
          if (consumedLastTick)
          {
            btn.SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Consuming);
            btn.SetIcon(this.Style.Panel.ElectricityInfoBoxIcon);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityElectricityConsumptionTooltip__Consuming);
          }
          else if (notEnoughPower)
          {
            btn.SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Low);
            btn.SetIcon(this.Style.Panel.ElectricityInfoBoxIcon_Low);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityElectricityConsumptionTooltip__NotEnough);
          }
          else
          {
            btn.SetButtonStyle(this.Style.Panel.ElectricityInfoBox_NotConsuming);
            btn.SetIcon(this.Style.Panel.ElectricityInfoBoxIcon_NotConsuming);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityElectricityConsumptionTooltip__NotConsuming);
          }
        }
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, el.IsPositive);
      }));
      return btn;
    }

    protected Btn AddMaxElectricityOutputPanel(
      UpdaterBuilder updaterBuilder,
      Func<Electricity> electricityProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Electricity", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.ElectricityInfoBox_Producing).SetText("").SetIcon(this.Style.Panel.ElectricityInfoBoxIcon_Producing).AddToolTip(Tr.EntityElectricityProductionTooltip).AppendTo<Btn>(this.m_rightButtonsContainer);
      updaterBuilder.Observe<Electricity>(electricityProvider).Do((Action<Electricity>) (el =>
      {
        btn.SetText(el.Format());
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, el.IsPositive);
      }));
      return btn;
    }

    public void AddWorkersPanel(
      UpdaterBuilder updaterBuilder,
      Func<IEntityWithWorkers> entityProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Workers", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.WorkersInfoBox).SetText("").SetIcon(this.Style.Panel.WorkersInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      Tooltip btnTooltip = btn.AddToolTipAndReturn();
      updaterBuilder.Observe<int>((Func<int>) (() => entityProvider().WorkersAssigned())).Observe<int>((Func<int>) (() => entityProvider().WorkersNeeded)).Do((Action<int, int>) ((assigned, needed) =>
      {
        if (assigned >= needed)
        {
          btn.SetButtonStyle(this.Style.Panel.WorkersInfoBox);
          btn.SetIcon(this.Style.Panel.WorkersInfoBoxIcon);
          btnTooltip.SetText((LocStrFormatted) Tr.EntityWorkersNeededTooltip_Assigned);
        }
        else
        {
          btn.SetButtonStyle(this.Style.Panel.WorkersInfoBox_NotAssigned);
          btn.SetIcon(this.Style.Panel.WorkersInfoBoxIcon_NotAssigned);
          btnTooltip.SetText((LocStrFormatted) Tr.EntityWorkersNeededTooltip_NotAssigned);
        }
        btn.SetText(needed.ToStringCached());
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, needed > 0);
      }));
    }

    public void AddGeneralPriorityPanel(
      InspectorContext context,
      Func<IEntityWithGeneralPriority> entityProvider)
    {
      GeneralPriorityPanel generalPriorityPanel = new GeneralPriorityPanel((IUiElement) this, context.InputScheduler, this.Builder, entityProvider);
      generalPriorityPanel.PutToRightTopOf<GeneralPriorityPanel>((IUiElement) this, generalPriorityPanel.GetSize(), Offset.Top(40f) + Offset.Right((float) (-(double) generalPriorityPanel.GetWidth() + 1.0)));
      generalPriorityPanel.SendToBack<GeneralPriorityPanel>();
      this.AddUpdater(generalPriorityPanel.Updater);
    }

    public void AddUnityCostPanel(
      UpdaterBuilder updaterBuilder,
      Func<IUnityConsumingEntity> entityProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Unity", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.ConsumedUnityInfoBox).SetText("").SetIcon(this.Style.Panel.ConsumedUnityInfoBoxIcon).AppendTo<Btn>(this.m_rightButtonsContainer);
      Tooltip btnTooltip = btn.AddToolTipAndReturn();
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().IsEnabled)).Observe<Upoints>((Func<Upoints>) (() => entityProvider().MaxMonthlyUnityConsumed)).Observe<bool>((Func<bool>) (() =>
      {
        UnityConsumer valueOrNull = entityProvider().UnityConsumer.ValueOrNull;
        return valueOrNull != null && valueOrNull.NotEnoughUnity;
      })).Observe<Upoints>((Func<Upoints>) (() =>
      {
        UnityConsumer valueOrNull = entityProvider().UnityConsumer.ValueOrNull;
        return valueOrNull == null ? Upoints.Zero : valueOrNull.MonthlyUnity;
      })).Do((Action<bool, Upoints, bool, Upoints>) ((isEnabled, upoints, notEnoughUnity, realConsumption) =>
      {
        if (isEnabled & notEnoughUnity)
        {
          btn.SetButtonStyle(this.Style.Panel.ConsumedUnityInfoBox_Low);
          btn.SetIcon(this.Style.Panel.ConsumedUnityInfoBoxIcon_Low);
          btnTooltip.SetText((LocStrFormatted) Tr.EntityMonthlyUnitTooltip__NotEnough);
        }
        else if (isEnabled && realConsumption.IsPositive)
        {
          btn.SetButtonStyle(this.Style.Panel.ConsumedUnityInfoBox);
          btn.SetIcon(this.Style.Panel.ConsumedUnityInfoBoxIcon);
          btnTooltip.SetText((LocStrFormatted) Tr.EntityMonthlyUnitTooltip__Consuming);
        }
        else
        {
          btn.SetButtonStyle(this.Style.Panel.ConsumedUnityInfoBox_NotConsuming);
          btn.SetIcon(this.Style.Panel.ConsumedUnityInfoBoxIcon_NotConsuming);
          btnTooltip.SetText((LocStrFormatted) Tr.EntityMonthlyUnitTooltip__NotConsuming);
        }
        btn.SetText(upoints.Format());
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, upoints.IsPositive);
      }));
    }

    public Pair<Txt, BufferView> AddMaintenancePanel(
      UpdaterBuilder updaterBuilder,
      Func<IMaintainedEntity> entityProvider,
      Action<IMaintainedEntity> repairAction = null)
    {
      Txt maintenanceTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.Maintenance, new LocStrFormatted?((LocStrFormatted) Tr.Maintenance__EntityTooltip));
      ButtonWithTextAndIcon quickRepairBtn = (ButtonWithTextAndIcon) null;
      if (repairAction != null)
      {
        quickRepairBtn = new ButtonWithTextAndIcon(this.Builder, this.Builder.Style.Global.UpointsBtn);
        quickRepairBtn.TextWithIcon.SetSuffixIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetIcon("Assets/Unity/UserInterface/General/QuickRepair.svg");
        quickRepairBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).PlayErrorSoundWhenDisabled().OnClick((Action) (() => repairAction(entityProvider()))).AddToolTip(Tr.EntityRepair__Tooltip);
        updaterBuilder.Observe<Upoints>((Func<Upoints>) (() => entityProvider().Maintenance.RepairCost)).Do((Action<Upoints>) (repairCost =>
        {
          quickRepairBtn.TextWithIcon.SetSuffixText("  |  " + repairCost.Value.ToStringRounded(1));
          quickRepairBtn.UpdateWidth();
          quickRepairBtn.SetVisibility<ButtonWithTextAndIcon>(repairCost.IsPositive);
        }));
      }
      BufferView bufferView = this.AddBufferView(this.Style.BufferView.CompactHeight, rightButton: (Btn) quickRepairBtn, isCompact: true);
      Txt breakdownStatus = this.Builder.NewTxt("BreakdownChance", (IUiElement) maintenanceTitle).SetAlignment(TextAnchor.MiddleRight).SetTextStyle(this.Style.Global.Title.Extend(new ColorRgba?(this.Style.Global.DangerClr))).PutToRightOf<Txt>((IUiElement) maintenanceTitle, 100f, Offset.Right(5f));
      TextWithIcon desc = new TextWithIcon(this.Builder, (IUiElement) bufferView).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) bufferView, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<MaintenanceStatus>((Func<MaintenanceStatus>) (() => entityProvider().Maintenance.Status)).Observe<MaintenanceCosts>((Func<MaintenanceCosts>) (() => entityProvider().Maintenance.Costs)).Do((Action<MaintenanceStatus, MaintenanceCosts>) ((maintenanceStatus, costs) =>
      {
        if (!costs.MaintenancePerMonth.IsPositive)
          return;
        bufferView.UpdateState((ProductProto) costs.Product, maintenanceStatus.MaintenancePointsMax.IntegerPart, maintenanceStatus.MaintenancePointsCurrent.IntegerPart);
      }));
      updaterBuilder.Observe<PartialQuantity>((Func<PartialQuantity>) (() => entityProvider().Maintenance.Costs.MaintenancePerMonth)).Do((Action<PartialQuantity>) (maintenancePerMonth =>
      {
        desc.SetPrefixText(Tr.Needs.TranslatedString + " " + maintenancePerMonth.ToStringRounded(1) + " / 60");
        this.ItemsContainer.SetItemVisibility((IUiElement) maintenanceTitle, maintenancePerMonth.IsPositive);
        this.ItemsContainer.SetItemVisibility((IUiElement) bufferView, maintenancePerMonth.IsPositive);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => entityProvider().Maintenance.Status.CurrentBreakdownChance)).Do((Action<Percent>) (breakdownChance =>
      {
        breakdownStatus.SetText(Tr.EntityBreakdownChance.Format(breakdownChance.ToStringRounded()));
        breakdownStatus.SetWidth<Txt>(breakdownStatus.GetPreferedWidth());
        breakdownStatus.SetVisibility<Txt>(breakdownChance.IsPositive);
      }));
      return Pair.Create<Txt, BufferView>(maintenanceTitle, bufferView);
    }

    protected void AddStorageLogisticsPanel(
      UpdaterBuilder updaterBuilder,
      Func<IEntityWithSimpleLogisticsControl> entityProvider,
      IInputScheduler inputScheduler)
    {
      StackContainer logisticsControlContainer = this.Builder.NewStackContainer("LogisticsControl", (IUiElement) this.ItemsContainer).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.All(1f)).SetItemSpacing(15f).AppendTo<StackContainer>(this.ItemsContainer, new Vector2?(new Vector2(0.0f, 35f)), ContainerPosition.LeftOrTop, Offset.Left(this.Style.Panel.Padding) + Offset.Top(5f));
      LogisticsModeToggle inputToggle = new LogisticsModeToggle((IUiElement) logisticsControlContainer, this.Builder, true, (Action<EntityLogisticsMode>) (m => onModeClick(m, true)), true);
      inputToggle.AppendTo<LogisticsModeToggle>(logisticsControlContainer, new float?((float) inputToggle.GetWidth().CeilToInt()));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().IsLogisticsInputDisabled)).Observe<LogisticsControl>((Func<LogisticsControl>) (() => entityProvider().LogisticsInputControl)).Do((Action<bool, LogisticsControl>) ((isDisabled, controlMode) =>
      {
        inputToggle.SetMode(isDisabled ? EntityLogisticsMode.Off : EntityLogisticsMode.On);
        inputToggle.SetIsEnabled(controlMode == LogisticsControl.Enabled);
      }));
      LogisticsModeToggle outputToggle = new LogisticsModeToggle((IUiElement) logisticsControlContainer, this.Builder, false, (Action<EntityLogisticsMode>) (m => onModeClick(m, false)), true);
      outputToggle.AppendTo<LogisticsModeToggle>(logisticsControlContainer, new float?((float) outputToggle.GetWidth().CeilToInt()));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().IsLogisticsOutputDisabled)).Observe<LogisticsControl>((Func<LogisticsControl>) (() => entityProvider().LogisticsOutputControl)).Do((Action<bool, LogisticsControl>) ((isDisabled, controlMode) =>
      {
        outputToggle.SetMode(isDisabled ? EntityLogisticsMode.Off : EntityLogisticsMode.On);
        outputToggle.SetIsEnabled(controlMode == LogisticsControl.Enabled);
      }));
      updaterBuilder.Observe<LogisticsControl>((Func<LogisticsControl>) (() => entityProvider().LogisticsInputControl)).Observe<LogisticsControl>((Func<LogisticsControl>) (() => entityProvider().LogisticsOutputControl)).Do((Action<LogisticsControl, LogisticsControl>) ((inputControlMode, outputControlMode) =>
      {
        bool isVisible1 = inputControlMode != LogisticsControl.NotAvailable;
        bool isVisible2 = outputControlMode != LogisticsControl.NotAvailable;
        this.ItemsContainer.SetItemVisibility((IUiElement) logisticsControlContainer, isVisible1 | isVisible2);
        logisticsControlContainer.SetItemVisibility((IUiElement) inputToggle, isVisible1);
        logisticsControlContainer.SetItemVisibility((IUiElement) outputToggle, isVisible2);
      }));

      void onModeClick(EntityLogisticsMode mode, bool isInput)
      {
        inputScheduler.ScheduleInputCmd<StorageDisableLogisticsToggleCmd>(new StorageDisableLogisticsToggleCmd(entityProvider().Id, isInput, mode == EntityLogisticsMode.Off));
      }
    }

    protected StackContainer AddLogisticsPanel(
      UpdaterBuilder updaterBuilder,
      Func<IEntityWithLogisticsControl> entityProvider,
      IInputScheduler inputScheduler)
    {
      StackContainer logisticsControlContainer = this.Builder.NewStackContainer("LogisticsControl", (IUiElement) this.ItemsContainer).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.All(1f)).SetItemSpacing(15f).AppendTo<StackContainer>(this.ItemsContainer, new Vector2?(new Vector2(0.0f, 35f)), ContainerPosition.LeftOrTop, Offset.Left(this.Style.Panel.Padding) + Offset.Top(5f));
      LogisticsModeToggle inputToggle = new LogisticsModeToggle((IUiElement) logisticsControlContainer, this.Builder, true, (Action<EntityLogisticsMode>) (m => onModeClick(m, true)));
      inputToggle.AppendTo<LogisticsModeToggle>(logisticsControlContainer, new float?((float) inputToggle.GetWidth().CeilToInt()));
      updaterBuilder.Observe<EntityLogisticsMode>((Func<EntityLogisticsMode>) (() => entityProvider().LogisticsInputMode)).Do((Action<EntityLogisticsMode>) (mode => inputToggle.SetMode(mode)));
      LogisticsModeToggle outputToggle = new LogisticsModeToggle((IUiElement) logisticsControlContainer, this.Builder, false, (Action<EntityLogisticsMode>) (m => onModeClick(m, false)));
      outputToggle.AppendTo<LogisticsModeToggle>(logisticsControlContainer, new float?((float) outputToggle.GetWidth().CeilToInt()));
      updaterBuilder.Observe<EntityLogisticsMode>((Func<EntityLogisticsMode>) (() => entityProvider().LogisticsOutputMode)).Do((Action<EntityLogisticsMode>) (mode => outputToggle.SetMode(mode)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().CanDisableLogisticsInput)).Observe<bool>((Func<bool>) (() => entityProvider().CanDisableLogisticsOutput)).Do((Action<bool, bool>) ((canDisableInput, canDisableOutput) =>
      {
        this.ItemsContainer.SetItemVisibility((IUiElement) logisticsControlContainer, canDisableInput | canDisableOutput);
        logisticsControlContainer.SetItemVisibility((IUiElement) inputToggle, canDisableInput);
        logisticsControlContainer.SetItemVisibility((IUiElement) outputToggle, canDisableOutput);
      }));
      return logisticsControlContainer;

      void onModeClick(EntityLogisticsMode mode, bool isInput)
      {
        inputScheduler.ScheduleInputCmd<EntityDisableLogisticsToggleCmd>(new EntityDisableLogisticsToggleCmd(entityProvider().Id, isInput, mode));
      }
    }

    public Pair<Txt, BufferView> AddShaftOverviewPanel(
      UpdaterBuilder updaterBuilder,
      Func<IEntityWithPorts> entityProvider,
      IShaftManager shaftManager)
    {
      Txt title = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.ShaftOverview, new LocStrFormatted?(Tr.ShaftOverview__Tooltip.Format(ShaftManager.MAX_SHAFT_THROUGHPUT.Format().Value)));
      BufferView bufferView = this.AddBufferView(this.Style.BufferView.CompactHeight, isCompact: true);
      bufferView.AddMarker(Shaft.STOP_OUTPUT_BELOW, ColorRgba.Red);
      Txt desc = this.Builder.NewTxt("status").SetTextStyle(this.Style.Panel.Text).SetAlignment(TextAnchor.MiddleLeft).PutToLeftBottomOf<Txt>((IUiElement) bufferView, new Vector2(200f, 25f), Offset.Left(100f));
      Txt throughput = this.Builder.NewTxt("throughput").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(this.Style.Global.Title).PutToRightOf<Txt>((IUiElement) title, 150f, Offset.Right(5f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => shaftManager.GetCurrentShaftFor((IStaticEntity) entityProvider()).InertiaBuffer.Capacity.IsPositive)).Do((Action<bool>) (show =>
      {
        this.ItemsContainer.SetItemVisibility((IUiElement) title, show);
        this.ItemsContainer.SetItemVisibility((IUiElement) bufferView, show);
      }));
      GlobalUiStyle globalStyle = this.Builder.Style.Global;
      updaterBuilder.Observe<ItemDetailWindowView.ShaftInfo>((Func<ItemDetailWindowView.ShaftInfo>) (() => new ItemDetailWindowView.ShaftInfo(shaftManager.GetCurrentShaftFor((IStaticEntity) entityProvider())))).Do((Action<ItemDetailWindowView.ShaftInfo>) (shaftInfo =>
      {
        MechPower mechPower1 = MechPower.FromQuantity(shaftInfo.Quantity);
        MechPower mechPower2 = MechPower.FromQuantity(shaftInfo.Capacity);
        bufferView.UpdateState((Option<ProductProto>) (ProductProto) shaftManager.MechPowerProto, mechPower2.IsZero ? Percent.Zero : Percent.FromRatio(mechPower1.Value, mechPower2.Value), TrCore.MwSec__Unit.Format(mechPower1.ToMwSeconds.ToStringRounded(0) + " / " + mechPower2.ToMwSeconds.ToStringRounded(0)));
        throughput.SetText(Tr.ThroughputWithParam.Format(string.Format("{0}%", (object) shaftInfo.Throughput.ToIntPercentRounded())));
        throughput.SetColor(shaftInfo.Throughput <= 80.Percent() ? globalStyle.DefaultPanelTextColor : globalStyle.DangerClr);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => shaftManager.GetCurrentShaftFor((IStaticEntity) entityProvider()).CurrentOutputScale)).Do((Action<Percent>) (outputScale => desc.SetText(Tr.Shaft__Status.Format(outputScale.ToIntPercentRounded().ToString()).Value)));
      return Pair.Create<Txt, BufferView>(title, bufferView);
    }

    protected StackContainer AddVehicleButtonsSection(StackContainer itemContainer)
    {
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Orders);
      return this.AddButtonsSection(itemContainer, ItemDetailWindowView.VEHICLE_BTN_SIZE + 10, Offset.Zero);
    }

    protected StackContainer AddButtonsSection(
      StackContainer itemContainer,
      int height,
      Offset offset)
    {
      Panel parent = this.Builder.NewPanel("Buttons").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?((float) height), offset);
      StackContainer leftMiddleOf = this.Builder.NewStackContainer("StackContainer").SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(10f).SetInnerPadding(Offset.LeftRight(20f)).PutToLeftMiddleOf<StackContainer>((IUiElement) parent, new Vector2(0.0f, (float) ItemDetailWindowView.VEHICLE_BTN_SIZE));
      leftMiddleOf.SizeChanged += (Action<IUiElement>) (element =>
      {
        if ((double) element.GetWidth() <= (double) this.GetContentPanel().GetWidth())
          return;
        this.SetWidth(element.GetWidth());
      });
      return leftMiddleOf;
    }

    protected void AddReplaceVehicleBtn(
      StackContainer container,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> vehicleProvider,
      InspectorContext context)
    {
      Panel overlay = this.AddOverlay((Action) (() => { }));
      VehicleReplaceView objectToPlace = new VehicleReplaceView(this.Builder, context, (IUiElement) container, (WindowView) this, overlay, vehicleProvider);
      this.AddUpdater(objectToPlace.Updater);
      objectToPlace.AppendTo<VehicleReplaceView>(container);
    }

    protected void AddScrapVehicleBtn(
      StackContainer container,
      UpdaterBuilder updaterBuilder,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> vehicleProvider,
      IVehiclesManager vehiclesManager,
      IInputScheduler inputScheduler)
    {
      Btn scrapBtn = this.Builder.NewBtnDanger("Scrap").SetIcon("Assets/Unity/UserInterface/General/ScrapVehicle.svg").PlayErrorSoundWhenDisabled().OnClick((Action) (() => inputScheduler.ScheduleInputCmd<ScrapVehicleCmd>(new ScrapVehicleCmd(vehicleProvider()))));
      Tooltip scrapTooltip = scrapBtn.AddToolTipAndReturn();
      scrapBtn.AppendTo<Btn>(container, new float?((float) ItemDetailWindowView.VEHICLE_BTN_SIZE));
      updaterBuilder.Observe<bool>((Func<bool>) (() => vehicleProvider().IsOnWayToDepotForScrap)).Observe<bool>((Func<bool>) (() => vehicleProvider().IsOnWayToDepotForReplacement)).Observe<bool>((Func<bool>) (() => vehiclesManager.CanScrapVehicle(vehicleProvider()))).Do((Action<bool, bool, bool>) ((isOnWayForScrap, isOnWayForReplace, canScrap) =>
      {
        scrapBtn.SetEnabled(canScrap && !isOnWayForReplace && !isOnWayForScrap);
        if (isOnWayForScrap)
          scrapTooltip.SetText((LocStrFormatted) Tr.ScrapVehicle__InProgress);
        else if (isOnWayForReplace)
          scrapTooltip.SetText((LocStrFormatted) Tr.ReplaceVehicle__OnItsWay);
        else
          scrapTooltip.SetText((LocStrFormatted) Tr.ScrapVehicle__Tooltip);
      }));
    }

    protected void AddVehicleGoToBtn(StackContainer container, Action onGoToClick)
    {
      this.Builder.NewBtnGeneral("GoTo", (IUiElement) container).SetIcon("Assets/Unity/UserInterface/General/GoTo.svg").AddToolTip(Tr.GoTo__Tooltip).OnClick(onGoToClick).AppendTo<Btn>(container, new float?((float) ItemDetailWindowView.VEHICLE_BTN_SIZE));
    }

    protected void AddRecoverVehicleBtn(
      StackContainer container,
      UpdaterBuilder updaterBuilder,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> vehicleProvider,
      IVehiclesManager vehiclesManager,
      IInputScheduler inputScheduler)
    {
      Btn recoverBtn = this.Builder.NewBtnUpoints("Recover").SetIcon("Assets/Unity/UserInterface/General/RecoverVehicle.svg", new Offset?(Offset.All(5f))).PlayErrorSoundWhenDisabled().OnClick((Action) (() => inputScheduler.ScheduleInputCmd<RecoverVehicleCmd>(new RecoverVehicleCmd(vehicleProvider())))).AppendTo<Btn>(container, new float?((float) ItemDetailWindowView.VEHICLE_BTN_SIZE));
      Tooltip recoverBtnTooltip = recoverBtn.AddToolTipAndReturn();
      string recoverTooltip = string.Format("{0} | {1} {2}\n\n{3}", (object) Tr.RecoverVehicle__Action.TranslatedString, (object) VehiclesManager.VEHICLE_RECOVERY_COST.Format(), (object) TrCore.Unity, (object) Tr.RecoverVehicle__Tooltip);
      updaterBuilder.Observe<KeyValuePair<bool, LocStrFormatted>>((Func<KeyValuePair<bool, LocStrFormatted>>) (() =>
      {
        Mafi.Core.Entities.Dynamic.Vehicle vehicle = vehicleProvider();
        LocStrFormatted errorReason = LocStrFormatted.Empty;
        return Make.Kvp<bool, LocStrFormatted>(vehiclesManager.CanRecoverVehicle(vehicle, out errorReason), errorReason);
      })).Do((Action<KeyValuePair<bool, LocStrFormatted>>) (canRecover =>
      {
        recoverBtnTooltip.SetText(canRecover.Value.IsNotEmpty ? canRecover.Value.Value : recoverTooltip);
        recoverBtn.SetEnabled(canRecover.Key);
      }));
    }

    protected StatusPanel AddStatusInfoPanel()
    {
      this.SetupTopButtonsContainer();
      return new StatusPanel((IUiElement) this.m_leftButtonsContainer, this.Builder).AppendTo<StatusPanel>(this.m_leftButtonsContainer);
    }

    protected Btn AddComputingInfoPanel(
      UpdaterBuilder updaterBuilder,
      Func<IComputingConsumingEntity> entityProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Computing", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.ComputingInfoBox_Consuming).SetText("").SetIcon(this.Style.Panel.ComputingInfoBoxIcon_Consuming).AppendTo<Btn>(this.m_rightButtonsContainer, new float?((float) this.Style.Panel.ComputingInfoBox_Consuming.Width));
      Tooltip btnTooltip = btn.AddToolTipAndReturn();
      updaterBuilder.Observe<Computing>((Func<Computing>) (() => entityProvider().ComputingRequired)).Observe<bool>((Func<bool>) (() =>
      {
        IComputingConsumerReadonly valueOrNull = entityProvider().ComputingConsumer.ValueOrNull;
        return valueOrNull != null && valueOrNull.DidConsumeLastTick;
      })).Observe<bool>((Func<bool>) (() =>
      {
        IComputingConsumerReadonly valueOrNull = entityProvider().ComputingConsumer.ValueOrNull;
        return valueOrNull != null && valueOrNull.NotEnoughComputing;
      })).Do((Action<Computing, bool, bool>) ((computing, consumedLastTick, notEnoughPower) =>
      {
        if (computing.IsPositive)
        {
          btn.SetText(computing.Format());
          if (consumedLastTick)
          {
            btn.SetButtonStyle(this.Style.Panel.ComputingInfoBox_Consuming);
            btn.SetIcon(this.Style.Panel.ComputingInfoBoxIcon_Consuming);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityComputingConsumptionTooltip__Consuming);
          }
          else if (notEnoughPower)
          {
            btn.SetButtonStyle(this.Style.Panel.ComputingInfoBox_Low);
            btn.SetIcon(this.Style.Panel.ComputingInfoBoxIcon_Low);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityComputingConsumptionTooltip__NotEnough);
          }
          else
          {
            btn.SetButtonStyle(this.Style.Panel.ComputingInfoBox_NotConsuming);
            btn.SetIcon(this.Style.Panel.ComputingInfoBoxIcon_NotConsuming);
            btnTooltip.SetText((LocStrFormatted) Tr.EntityComputingConsumptionTooltip__NotConsuming);
          }
        }
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, computing.IsPositive);
      }));
      return btn;
    }

    protected Btn AddComputingGenerationPanel(
      UpdaterBuilder updaterBuilder,
      Func<Computing> computingProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("Computing", (IUiElement) this.m_rightButtonsContainer).EnableDynamicSize().SetButtonStyle(this.Style.Panel.CreatedComputingInfoBox).SetText("").SetIcon(this.Style.Panel.CreatedComputingInfoBoxIcon).AddToolTip(Tr.EntityComputingProductionTooltip).AppendTo<Btn>(this.m_rightButtonsContainer, new float?((float) this.Style.Panel.ComputingInfoBox_Consuming.Width));
      updaterBuilder.Observe<Computing>(computingProvider).Do((Action<Computing>) (computing =>
      {
        btn.SetText(computing.Format());
        this.m_rightButtonsContainer.SetItemVisibility((IUiElement) btn, !computing.IsZero);
      }));
      return btn;
    }

    protected Btn AddNavigationOverlayPanel(
      VehiclesPathabilityOverlayRenderer overlayRenderer,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> vehicleProvider)
    {
      this.SetupTopButtonsContainer();
      Btn btn = this.Builder.NewBtn("NavOverlay", (IUiElement) this.m_leftButtonsContainer).SetButtonStyle(this.Style.Global.GeneralBtnToToggle).SetText((LocStrFormatted) Tr.EntityToggleNavigationOverlay).SetIcon("Assets/Unity/UserInterface/General/Path.svg", 20.Vector2()).AddToolTip(Tr.EntityToggleNavigationOverlay__Tooltip);
      btn.OnClick((Action) (() =>
      {
        if (overlayRenderer.IsOverlayShown)
          overlayRenderer.HideOverlay();
        else
          overlayRenderer.ShowOverlayFor(vehicleProvider().Prototype.PathFindingParams);
      }));
      btn.AppendTo<Btn>(this.m_leftButtonsContainer, new float?(btn.GetOptimalWidth()));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => overlayRenderer.IsOverlayShown)).Do((Action<bool>) (isVisible => btn.SetButtonStyle(isVisible ? this.Style.Global.GeneralBtnActive : this.Style.Global.GeneralBtnToToggle)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => vehicleProvider().IsStrugglingToNavigate)).Do((Action<bool>) (isStrugglingToNavigate =>
      {
        if (!isStrugglingToNavigate)
          return;
        overlayRenderer.ShowOverlayFor(vehicleProvider().Prototype.PathFindingParams);
      }));
      this.AddUpdater(updaterBuilder.Build());
      return btn;
    }

    protected void AddUpgradeView(
      EntityUpgradeView upgradeView,
      Func<IUpgradableEntity> entityProvider)
    {
      upgradeView.Build((IUiElement) this.ItemsContainer, this.Builder).AppendTo<EntityUpgradeView>(this.ItemsContainer, new float?(upgradeView.Height), Offset.Top(10f));
      this.AddUpdater(upgradeView.CreateUpdater(entityProvider, (Action<bool>) (isVisible => this.ItemsContainer.SetItemVisibility((IUiElement) upgradeView, isVisible))));
    }

    protected void AddClearButton(Action action)
    {
    }

    protected void AddFollowVehicleButton(
      MbBasedEntitiesRenderer entitiesRenderer,
      OrbitalCameraModel camera,
      Func<IRenderedEntity> entityProvider)
    {
      this.AddHeaderButton((IUiElement) this.Builder.NewBtn("Follow").SetButtonStyle(this.Style.Panel.HeaderButton).SetIcon("Assets/Unity/UserInterface/General/Video.svg", new Offset?(Offset.LeftRight(4f))).AddToolTip(Tr.FollowVehicleTooltip).OnClick(new Action(onClick)));

      void onClick()
      {
        EntityMb entityMb;
        if (!entitiesRenderer.TryGetMbFor(entityProvider(), out entityMb))
          return;
        if (camera.TrackedTarget == entityMb.transform)
          camera.TrackedTarget = Option<Transform>.None;
        else
          camera.TrackedTarget = (Option<Transform>) entityMb.transform;
      }
    }

    protected Btn AddButton(StackContainer parent, Action action, string title)
    {
      Btn objectToPlace = this.Builder.NewBtn(title, (IUiElement) parent).SetButtonStyle(this.Style.Global.GeneralBtn).SetText(title).OnClick(action);
      return objectToPlace.AppendTo<Btn>(parent, new Vector2?(objectToPlace.GetOptimalSize()), ContainerPosition.MiddleOrCenter, Offset.Top(5f));
    }

    protected BufferView AddBufferView(
      float height,
      Action trashAction = null,
      Action plusBtnAction = null,
      Btn rightButton = null,
      bool isCompact = false)
    {
      return this.Builder.NewBufferView((IUiElement) this.ItemsContainer, trashAction, plusBtnAction, rightButton, isCompact).AppendTo<BufferView>(this.ItemsContainer, new float?(height));
    }

    protected void AddVehiclesAssigner<T>(
      VehiclesAssignerView<T> assigner,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null)
      where T : DrivingEntityProto
    {
      this.AddSectionTitle(this.ItemsContainer, title, tooltip);
      assigner.Build((IUiElement) this.ItemsContainer, this.Builder);
      assigner.AppendTo<VehiclesAssignerView<T>>(this.ItemsContainer, new float?(assigner.GetHeight()));
      this.AddUpdater(assigner.Updater);
    }

    protected void AddBuildingsAssignerForExport(
      InspectorContext context,
      Action onBtnClick,
      Func<IEntityAssignedAsOutput> entityProvider,
      LocStrFormatted tooltipExport)
    {
      Txt title = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) TrCore.ExportRoutesTitle, new LocStrFormatted?(tooltipExport));
      AssignedBuildingsView objectToPlace = new AssignedBuildingsView((IUiElement) this.ItemsContainer, context.CameraController, context.InputScheduler, context.UnlockedProtosDbForUi, context.ProtosDb, this.Builder, onBtnClick, entityProvider, (Func<IEntityAssignedAsInput>) null);
      objectToPlace.AppendTo<AssignedBuildingsView>(this.ItemsContainer, new float?(0.0f));
      objectToPlace.SetupVisibilityHook(title, this.ItemsContainer);
      this.AddUpdater(objectToPlace.Updater);
    }

    protected void AddBuildingsAssignerForImport(
      InspectorContext context,
      Action onBtnClick,
      Func<IEntityAssignedAsInput> entityProvider,
      LocStrFormatted tooltipImport,
      UpdaterBuilder updaterBuilder)
    {
      Txt txt = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) TrCore.ImportRoutesTitle, new LocStrFormatted?(tooltipImport));
      SwitchBtn btn = this.Builder.NewSwitchBtn().SetText(Tr.ImportRoutesEnforce__Title.TranslatedString).AddTooltip(Tr.ImportRoutesEnforce__Tooltip.TranslatedString).SetOnToggleAction((Action<bool>) (_ => context.InputScheduler.ScheduleInputCmd<ToggleImportRouteEnforcementCmd>(new ToggleImportRouteEnforcementCmd(entityProvider()))));
      btn.PutToRightOf<SwitchBtn>((IUiElement) txt, btn.GetWidth(), Offset.Right(10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().AllowNonAssignedOutput)).Observe<bool>((Func<bool>) (() => entityProvider().AssignedOutputs.IsNotEmpty<IEntityAssignedAsOutput>())).Do((Action<bool, bool>) ((allowNonAssigned, hasOutputs) =>
      {
        btn.SetIsOn(!allowNonAssigned);
        btn.SetVisibility<SwitchBtn>(hasOutputs);
      }));
      AssignedBuildingsView objectToPlace = new AssignedBuildingsView((IUiElement) this.ItemsContainer, context.CameraController, context.InputScheduler, context.UnlockedProtosDbForUi, context.ProtosDb, this.Builder, onBtnClick, (Func<IEntityAssignedAsOutput>) null, entityProvider);
      objectToPlace.AppendTo<AssignedBuildingsView>(this.ItemsContainer, new float?(0.0f));
      objectToPlace.SetupVisibilityHook(txt, this.ItemsContainer);
      this.AddUpdater(objectToPlace.Updater);
    }

    protected void AddVehicleFuelStatus(
      StackContainer parent,
      UpdaterBuilder updater,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> vehicleGetter)
    {
      this.AddSectionTitle(parent, (LocStrFormatted) Tr.FuelTank_Title);
      BufferView fuelBuffer = this.AddBufferView(this.Style.BufferView.CompactHeight, isCompact: true);
      updater.Observe<FuelTank.TankInfo>((Func<FuelTank.TankInfo>) (() =>
      {
        Mafi.Core.Entities.Dynamic.Vehicle vehicle = vehicleGetter();
        return !vehicle.FuelTank.IsNone ? vehicle.FuelTank.Value.Info : FuelTank.TankInfo.NONE;
      })).Do((Action<FuelTank.TankInfo>) (x =>
      {
        if (x.IsNotNone)
          fuelBuffer.UpdateState(x.Product, x.Quantity, x.PercentFull);
        else
          fuelBuffer.UpdateState((Option<ProductProto>) Option.None, Quantity.Zero, Quantity.Zero);
      }));
    }

    protected void AddAssignedToPanel(
      InspectorContext context,
      StackContainer itemContainer,
      UpdaterBuilder updater,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> entity)
    {
      Txt assignedToTitle = this.AddSectionTitle(itemContainer, "");
      Panel assignedToContainer = this.Builder.NewPanel("AssignedToContainer").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(50f));
      AssignedElementIcon assignedToIcon = new AssignedElementIcon(this.Builder, "", (Action<int>) (_ =>
      {
        if (!(entity().AssignedTo.ValueOrNull is IEntityWithPosition valueOrNull2))
          return;
        context.CameraController.PanTo(valueOrNull2.Position2f);
      }));
      assignedToIcon.PutToLeftOf<AssignedElementIcon>((IUiElement) assignedToContainer, 40f, Offset.Left(10f));
      Btn objectToPlace = this.Builder.NewBtnGeneral("Unassign", (IUiElement) assignedToContainer).SetText((LocStrFormatted) Tr.Unassign).OnClick((Action) (() =>
      {
        if (!entity().AssignedTo.HasValue)
          return;
        context.InputScheduler.ScheduleInputCmd<UnassignVehicleCmd>(new UnassignVehicleCmd(entity().Id));
      })).AddToolTip(Tr.Unassign__VehicleTooltip);
      objectToPlace.PutToLeftMiddleOf<Btn>((IUiElement) assignedToContainer, objectToPlace.GetOptimalSize(), Offset.Left(assignedToIcon.GetWidth() + 20f));
      updater.Observe<Option<IEntityAssignedWithVehicles>>((Func<Option<IEntityAssignedWithVehicles>>) (() => entity().AssignedTo)).Do((Action<Option<IEntityAssignedWithVehicles>>) (assignedTo =>
      {
        if (assignedTo.HasValue)
        {
          if (assignedTo.Value.Prototype is LayoutEntityProto prototype4)
            assignedToIcon.SetIcon(prototype4.Graphics.IconPath);
          else if (assignedTo.Value.Prototype is DynamicGroundEntityProto prototype3)
            assignedToIcon.SetIcon(prototype3.Graphics.IconPath);
          assignedToTitle.SetText(Tr.AssignedTo.Format(assignedTo.Value.Prototype.Strings.Name));
        }
        itemContainer.StartBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) assignedToTitle, assignedTo.HasValue);
        itemContainer.SetItemVisibility((IUiElement) assignedToContainer, assignedTo.HasValue);
        itemContainer.FinishBatchOperation();
      }));
    }

    protected AlertIndicatorNotification GetOrAddAlertIndicatorNotification(InspectorContext context)
    {
      if (this.m_alertIndicatorNotification.IsNone)
      {
        StackContainer alertsContainer = this.getOrCreateAlertsContainer();
        AlertIndicatorNotification indicatorNotification = new AlertIndicatorNotification(this.Builder, context, (IUiElement) alertsContainer);
        indicatorNotification.AppendTo<AlertIndicatorNotification>(alertsContainer, new float?(indicatorNotification.GetHeight()));
        this.m_alertIndicatorNotification = (Option<AlertIndicatorNotification>) indicatorNotification;
      }
      return this.m_alertIndicatorNotification.Value;
    }

    public AlertIndicator GetOrAddAlertIndicator(InspectorContext context)
    {
      if (this.m_alertIndicatorGeneral.IsNone)
      {
        StackContainer alertsContainer = this.getOrCreateAlertsContainer();
        AlertIndicator alertIndicator = new AlertIndicator(this.Builder, context, (IUiElement) alertsContainer);
        alertIndicator.AppendTo<AlertIndicator>(alertsContainer, new float?(alertIndicator.GetHeight()));
        this.m_alertIndicatorGeneral = (Option<AlertIndicator>) alertIndicator;
      }
      return this.m_alertIndicatorGeneral.Value;
    }

    protected void SetAlertIndicatorVisibility(AlertIndicator indicator, bool isVisible)
    {
      this.m_alertsContainer.Value?.SetItemVisibility((IUiElement) indicator, isVisible);
    }

    private StackContainer getOrCreateAlertsContainer()
    {
      if (this.m_alertsContainer.HasValue)
        return this.m_alertsContainer.Value;
      this.m_alertsContainer = (Option<StackContainer>) this.Builder.NewStackContainer("LeftPanels").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(20f).PutToLeftTopOf<StackContainer>((IUiElement) this, new Vector2(50f, 0.0f), Offset.Top(40f) + Offset.Left(-49f));
      this.m_alertsContainer.Value.SendToBack<StackContainer>();
      return this.m_alertsContainer.Value;
    }

    protected void SetupAlertsIndicator(
      UpdaterBuilder updaterBuilder,
      InspectorContext context,
      Func<IEntity> entityProvider)
    {
      updaterBuilder.Observe<Option<INotification>>((Func<Option<INotification>>) (() => context.NotificationsManager.GetFirstActiveNotificationForInspector(entityProvider()))).Do((Action<Option<INotification>>) (notifMaybe =>
      {
        AlertIndicatorNotification indicatorNotification = this.GetOrAddAlertIndicatorNotification(context);
        this.m_alertsContainer.Value?.SetItemVisibility((IUiElement) indicatorNotification, notifMaybe.HasValue);
        if (!notifMaybe.HasValue)
          return;
        indicatorNotification.SetNotification(notifMaybe.Value);
      }));
    }

    protected void SetTitle(LocStrFormatted title, bool isPaused)
    {
      string text = title.Value;
      if (isPaused)
        text = text + " <color=#FC363BFF>(" + ItemDetailWindowView.PAUSED_SUFFIX + ")</color>";
      this.SetTitle(text);
    }

    protected void SetTitle(string title, bool isPaused)
    {
      if (isPaused)
        title = title + " <color=#FC363BFF>(" + ItemDetailWindowView.PAUSED_SUFFIX + ")</color>";
      this.SetTitle(title);
    }

    protected void SetTitle(string title, bool isPaused, bool isBroken)
    {
      if (isPaused)
        title = title + " <color=#FC363BFF>(" + ItemDetailWindowView.PAUSED_SUFFIX + ")</color>";
      if (isBroken)
        title = title + " <color=#FC363BFF>(" + ItemDetailWindowView.BROKEN_SUFFIX + ")</color>";
      this.SetTitle(title);
    }

    public void SetupProductPickerWithBuffer(
      ProtoPicker<ProductProto> protoPicker,
      BufferView buffer)
    {
      protoPicker.BuildUi(this.Builder);
      protoPicker.MakeMovable((Action<Offset>) (o => ItemDetailWindowView.s_windowOffsets[(int) this.m_windowOffsetGroup] = o), (IUiElement) this);
      buffer.BindProductPicker((WindowView) this, this.AddOverlay(new Action(((View) protoPicker).Hide)), protoPicker);
    }

    public void SetupInnerWindowWithButton(
      WindowView innerWindow,
      IUiElement btnHolder,
      IUiElement btn,
      Action returnBtnHolder,
      Action onExitAction)
    {
      innerWindow.BuildUi(this.Builder);
      innerWindow.MakeMovable((Action<Offset>) (o => ItemDetailWindowView.s_windowOffsets[(int) this.m_windowOffsetGroup] = o), (IUiElement) this);
      innerWindow.PutToLeftTopOf<WindowView>(btnHolder, innerWindow.GetSize(), Offset.Left(btn.GetWidth() - 1f));
      Panel overlay = this.AddOverlay(onExitAction);
      innerWindow.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        btnHolder.SetParent<IUiElement>((IUiElement) this);
      });
      innerWindow.OnHide += (Action) (() =>
      {
        overlay.Hide<Panel>();
        returnBtnHolder();
      });
    }

    protected void AddOceanAreaRecovery(
      InspectorContext context,
      UpdaterBuilder updaterBuilder,
      StackContainer itemContainer,
      Func<IStaticEntityWithReservedOcean> entityProvider)
    {
      RecoverOceanAccessCmd[] capturedCommand = new RecoverOceanAccessCmd[1];
      Txt recoveryTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ShipyardRecoverOceanAccess__Title, new LocStrFormatted?((LocStrFormatted) Tr.ShipyardRecoverOceanAccess__Tooltip));
      Panel recoveryContainer = this.Builder.NewPanel("RecoveryPanel").SetBackground(this.Style.Panel.ItemOverlayDark);
      CostButton recoverBtn = new CostButton(this.Builder, Tr.ShipyardRecoverOceanAccess__Button.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg", "Recover");
      recoverBtn.SetCost(OceanAreaRecoverHelper.RECOVERY_COST.Format());
      recoverBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).PlayErrorSoundWhenDisabled().OnClick((Action) (() => capturedCommand[0] = context.InputScheduler.ScheduleInputCmd<RecoverOceanAccessCmd>(new RecoverOceanAccessCmd(entityProvider().Id, OceanAreaRecoverHelper.DEFAULT_TILES_RECOVERED)))).PutToLeftTopOf<Btn>((IUiElement) recoveryContainer, recoverBtn.GetSize(), Offset.Left(20f) + Offset.Top(5f));
      Tooltip recoverBtnTooltip = recoverBtn.AddToolTipAndReturn();
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().ReservedOceanAreaState.HasAnyValidAreaSet)).Observe<bool>((Func<bool>) (() => context.UpointsManager.CanConsume(OceanAreaRecoverHelper.RECOVERY_COST))).Do((Action<bool, bool>) ((hasAnyValidArea, canConsume) =>
      {
        recoverBtn.SetEnabled(canConsume);
        if (canConsume)
        {
          recoverBtnTooltip.SetNormalTextStyle();
          recoverBtnTooltip.SetText((LocStrFormatted) Tr.ShipyardRecoverOceanAccess__BtnTooltip);
        }
        else
        {
          recoverBtnTooltip.SetErrorTextStyle();
          recoverBtnTooltip.SetText((LocStrFormatted) TrCore.TradeStatus__NoUnity);
        }
        bool isVisible = !hasAnyValidArea;
        itemContainer.SetItemVisibility((IUiElement) recoveryContainer, !hasAnyValidArea);
        itemContainer.StartBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) recoveryTitle, isVisible);
        itemContainer.SetItemVisibility((IUiElement) recoveryContainer, isVisible);
        itemContainer.FinishBatchOperation();
      }));
      recoveryContainer.AppendTo<Panel>(itemContainer, new float?(recoverBtn.GetHeight() + 10f));
      Panel recoveryMsgContainer = this.Builder.NewPanel("StatusContainer").SetBackground(this.Style.Panel.ItemOverlayDark).AppendTo<Panel>(itemContainer, new float?(40f));
      Txt recoveryMsg = this.Builder.NewTxt("NotifText").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Style.Global.TextControls.Extend(new ColorRgba?(this.Style.Global.OrangeText))).PutTo<Txt>((IUiElement) recoveryMsgContainer, Offset.LeftRight(20f));
      updaterBuilder.Observe<RecoverOceanAccessCmd>((Func<RecoverOceanAccessCmd>) (() => capturedCommand[0])).Observe<bool>((Func<bool>) (() =>
      {
        RecoverOceanAccessCmd recoverOceanAccessCmd = capturedCommand[0];
        // ISSUE: explicit non-virtual call
        return recoverOceanAccessCmd != null && __nonvirtual (recoverOceanAccessCmd.IsProcessedAndSynced);
      })).ObserveNoCompare<IStaticEntity>((Func<IStaticEntity>) (() =>
      {
        IStaticEntity entity;
        context.EntitiesManager.TryGetEntity<IStaticEntity>(capturedCommand[0] == null || !capturedCommand[0].IsProcessedAndSynced ? EntityId.Invalid : capturedCommand[0].Result.BlockingEntityId, out entity);
        return entity;
      })).Do((Action<RecoverOceanAccessCmd, bool, IStaticEntity>) ((cmd, isProcessed, blockingEntity) =>
      {
        if (!isProcessed)
        {
          itemContainer.HideItem((IUiElement) recoveryMsgContainer);
        }
        else
        {
          if (blockingEntity != null)
            recoveryMsg.SetText(TrCore.AdditionError__OceanBlockedBy.Format(blockingEntity.GetTitle()));
          else if (cmd.Result.BlockedByTerrainCount > 0)
            recoveryMsg.SetText(TrCore.AdditionError__OceanBlockedByTerrain.Format(cmd.Result.BlockedByTerrainCount));
          itemContainer.SetItemVisibility((IUiElement) recoveryMsgContainer, blockingEntity != null || cmd.Result.BlockedByTerrainCount > 0);
        }
      }));
      this.OnShowStart += (Action) (() => capturedCommand[0] = (RecoverOceanAccessCmd) null);
    }

    protected void AddTitleRenameButton(
      Func<IEntityWithCustomTitle> getEntityFn,
      IInputScheduler inputScheduler)
    {
      this.m_getEntityFn = getEntityFn;
      this.m_inputScheduler = inputScheduler;
      this.m_textEditBtn = this.Builder.NewBtn("Rename", (IUiElement) this.m_headerText).SetButtonStyle(this.Builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Rename.svg", new Offset?(Offset.LeftRight(3f))).OnClick(new Action(this.startRenamingSession)).PutToRightOf<Btn>((IUiElement) this.m_headerText, 20f, Offset.Right(-22f));
      this.m_txtInput = this.Builder.NewTxtField("SaveNameInput", (IUiElement) this.m_headerHolder).SetStyle(this.Style.Global.LightTxtFieldStyle).SetCharLimit(60).PutToCenterOf<TxtField>((IUiElement) this.m_headerHolder, 200f).Hide<TxtField>();
      this.m_textSaveBtn = this.Builder.NewBtn("Save", (IUiElement) this.m_txtInput).SetButtonStyle(this.Builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Save.svg", new Offset?(Offset.LeftRight(3f))).OnClick(new Action(this.commitRenamingSession)).PutToRightOf<Btn>((IUiElement) this.m_txtInput, 20f, Offset.Right(-22f)).Hide<Btn>();
      this.m_headerText.PutToCenterOf<Txt>((IUiElement) this.m_headerHolder, 0.0f);
    }

    protected void InputUpdateForRenaming()
    {
      if (!this.m_isInRenamingSession || !UnityEngine.Input.GetKey(KeyCode.Return) && !UnityEngine.Input.GetKey(KeyCode.KeypadEnter))
        return;
      this.commitRenamingSession();
    }

    private void commitRenamingSession()
    {
      this.m_inputScheduler.ScheduleInputCmd<SetEntityNameCmd>(new SetEntityNameCmd(this.m_getEntityFn().Id, this.m_txtInput.GetText()));
      this.stopRenamingSession();
    }

    private void stopRenamingSession()
    {
      this.m_isInRenamingSession = false;
      this.m_txtInput.Hide<TxtField>();
      this.m_textSaveBtn.Hide<Btn>();
      this.m_textEditBtn.Show<Btn>();
      this.m_headerText.Show<Txt>();
    }

    private void startRenamingSession()
    {
      this.m_isInRenamingSession = true;
      this.m_txtInput.Show<TxtField>();
      this.m_txtInput.Focus();
      this.m_txtInput.SetText(this.m_getEntityFn().CustomTitle.ValueOrNull ?? "");
      this.m_textSaveBtn.Show<Btn>();
      this.m_textEditBtn.Hide<Btn>();
      this.m_headerText.Hide<Txt>();
    }

    static ItemDetailWindowView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ItemDetailWindowView.s_windowOffsets = new Offset[2]
      {
        Offset.TopLeft(100f, 100f),
        Offset.TopLeft(100f, 100f)
      };
      ItemDetailWindowView.PAUSED_SUFFIX = Tr.EntityStatus__Paused.TranslatedString.ToLower();
      ItemDetailWindowView.BROKEN_SUFFIX = TrCore.EntityStatus__Broken.TranslatedString.ToLower();
      ItemDetailWindowView.VEHICLE_BTN_SIZE = 40;
    }

    protected enum WindowOffsetGroup
    {
      Inspector,
      LargeScreen,
    }

    private readonly struct ShaftInfo : IEquatable<ItemDetailWindowView.ShaftInfo>
    {
      public readonly Quantity Quantity;
      public readonly Quantity Capacity;
      public readonly Percent Throughput;

      public ShaftInfo(IShaft shaft)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Quantity = shaft.TotalAvailablePower.Quantity;
        this.Capacity = shaft.InertiaBuffer.Capacity;
        this.Throughput = shaft.ThroughputUtilization;
      }

      public bool Equals(ItemDetailWindowView.ShaftInfo other)
      {
        return this.Quantity == other.Quantity && this.Capacity == other.Capacity && this.Throughput == other.Throughput;
      }

      public override bool Equals(object obj)
      {
        return obj is ItemDetailWindowView.ShaftInfo other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return Hash.Combine<Quantity, Quantity, Percent>(this.Quantity, this.Capacity, this.Throughput);
      }
    }
  }
}
