// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.StaticEntityInspectorBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Ports;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// Handles title construction, pausing, and title changes.
  /// </summary>
  public abstract class StaticEntityInspectorBase<T> : EntityInspectorBase<T> where T : IStaticEntity
  {
    private StaticEntityInspectorBase<T>.ConstructionWindow m_constructionWindow;
    private readonly InspectorContext m_inspectorContext;
    protected readonly Option<EntityUpgradeView> UpgradeView;

    protected StaticEntityInspectorBase(IEntityInspector inspector)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspector);
      this.m_inspectorContext = inspector.Context;
      if (!typeof (T).IsAssignableTo(typeof (IUpgradableEntity)))
        return;
      this.UpgradeView = (Option<EntityUpgradeView>) new EntityUpgradeView(new Action(this.upgrade), this.m_inspectorContext.AssetsManager);
    }

    private void upgrade()
    {
      if (!(this.Entity is IUpgradableEntity entity))
      {
        Log.Error(string.Format("Trying to upgrade non-upgradable entity: {0}", (object) this.Entity));
      }
      else
      {
        this.m_inspectorContext.Highlighter.RemoveHighlight((IRenderedEntity) this.Entity);
        this.m_inspectorContext.InputScheduler.ScheduleInputCmd<UpgradeEntityCmd>(new UpgradeEntityCmd(entity));
      }
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      if (typeof (T).IsAssignableTo(typeof (IElectricityGeneratingEntity)))
      {
        PowerGenerationPriorityPanel generationPriorityPanel = new PowerGenerationPriorityPanel((IUiElement) this, this.m_inspectorContext.InputScheduler, this.Builder, (Func<IElectricityGeneratingEntity>) (() => (IElectricityGeneratingEntity) (object) this.Entity));
        generationPriorityPanel.PutToRightTopOf<PowerGenerationPriorityPanel>((IUiElement) this, generationPriorityPanel.GetSize(), Offset.Top(170f) + Offset.Right((float) (-(double) generationPriorityPanel.GetWidth() + 1.0)));
        this.AddUpdater(generationPriorityPanel.Updater);
      }
      if (typeof (T).IsAssignableTo(typeof (ISolarPanelEntity)))
      {
        PowerGenerationPriorityPanel generationPriorityPanel = new PowerGenerationPriorityPanel((IUiElement) this, this.Builder, 0, true);
        generationPriorityPanel.PutToRightTopOf<PowerGenerationPriorityPanel>((IUiElement) this, generationPriorityPanel.GetSize(), Offset.Top(160f) + Offset.Right((float) (-(double) generationPriorityPanel.GetWidth() + 1.0)));
      }
      this.m_constructionWindow = new StaticEntityInspectorBase<T>.ConstructionWindow("ConstrWindow", (Func<IStaticEntity>) (() => (IStaticEntity) this.Entity), this.m_inspectorContext);
      this.m_constructionWindow.BuildUi(this.Builder, (IUiElement) this);
      this.m_constructionWindow.PositionSelf((WindowView) this);
      Panel customFooter = this.Builder.NewPanel("CustomFooter", (IUiElement) this).SetBackground(StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE).SetBorderStyle(new BorderStyle(ColorRgba.Black)).PutTo<Panel>((IUiElement) this).Hide<Panel>();
      updaterBuilder.Observe<ConstructionState>((Func<ConstructionState>) (() => this.Entity.ConstructionState)).Do((Action<ConstructionState>) (state =>
      {
        bool flag = state == ConstructionState.Constructed;
        ColorRgba color;
        switch (state)
        {
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
            color = StaticEntityInspectorBase<T>.ConstructionWindow.PURPLE;
            break;
          case ConstructionState.InDeconstruction:
            color = StaticEntityInspectorBase<T>.ConstructionWindow.RED;
            break;
          default:
            color = StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE;
            break;
        }
        customFooter.SetBackground(color);
        this.m_constructionWindow.SetVisibility<StaticEntityInspectorBase<T>.ConstructionWindow>(!flag || state == ConstructionState.PreparingUpgrade);
        if (!flag)
          this.SetCustomFooter((IUiElement) customFooter);
        else
          this.HideCustomFooter((IUiElement) customFooter);
      }));
      if (typeof (T).IsAssignableTo(typeof (IEntityWithPorts)))
        this.AddShaftOverviewPanel(updaterBuilder, (Func<IEntityWithPorts>) (() => (IEntityWithPorts) (object) this.Entity), this.m_inspectorContext.ShaftManager);
      if (typeof (T).IsAssignableTo(typeof (IStaticEntityWithReservedOcean)))
        this.AddOceanAreaRecovery(this.m_inspectorContext, updaterBuilder, itemContainer, (Func<IStaticEntityWithReservedOcean>) (() => (IStaticEntityWithReservedOcean) (object) this.Entity));
      this.AddUpdater(updaterBuilder.Build());
    }

    protected override void AddCustomItemsEnd(StackContainer itemContainer)
    {
      base.AddCustomItemsEnd(itemContainer);
      if (!this.UpgradeView.HasValue)
        return;
      this.AddUpgradeView(this.UpgradeView.Value, (Func<IUpgradableEntity>) (() => (object) this.Entity as IUpgradableEntity));
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_constructionWindow.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_constructionWindow.SyncUpdate(gameTime);
    }

    public override void Show() => base.Show();

    public override void Hide() => base.Hide();

    private class ConstructionWindow : WindowView
    {
      public static readonly ColorRgba RED;
      public static readonly ColorRgba ORANGE;
      public static readonly ColorRgba PURPLE;
      public static readonly ColorRgba GRAY;
      private static readonly int CONNECTOR_HEIGHT;
      private readonly Func<IStaticEntity> m_entityProvider;
      private readonly InspectorContext m_inspectorContext;

      public ConstructionWindow(
        string id,
        Func<IStaticEntity> entityProvider,
        InspectorContext inspectorContext)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(id);
        this.m_entityProvider = entityProvider;
        this.m_inspectorContext = inspectorContext;
      }

      protected override void BuildWindowContent()
      {
        this.SetTitleBg(StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE);
        this.SetTitle("");
        Panel connectorLeft = this.Builder.NewPanel("connectorLeft", (IUiElement) this).SetBackground(StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE).SetBorderStyle(new BorderStyle(ColorRgba.Black));
        connectorLeft.PutToLeftTopOf<Panel>((IUiElement) this, new Vector2(7f, (float) (StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT + 2)), Offset.Top((float) (-StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT - 1)) + Offset.Left(25f));
        Panel connectorRight = this.Builder.NewPanel("connectorRight", (IUiElement) this).SetBackground(StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE).SetBorderStyle(new BorderStyle(ColorRgba.Black));
        connectorRight.PutToRightTopOf<Panel>((IUiElement) this, new Vector2(7f, (float) (StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT + 2)), Offset.Top((float) (-StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT - 1)) + Offset.Right(25f));
        ConstructionProgressView constructionView = new ConstructionProgressView((IUiElement) this.GetContentPanel(), this.Builder, (Func<Option<IConstructionProgress>>) (() => this.m_entityProvider().ConstructionProgress.As<IConstructionProgress>()));
        constructionView.SetBackground(this.Builder.Style.Panel.ItemOverlay).PutTo<ConstructionProgressView>((IUiElement) this.GetContentPanel(), Offset.TopBottom(5f));
        constructionView.AddPauseBtn(new Action<bool>(onPauseToggled));
        constructionView.AddQuickBuildBtn(this.m_inspectorContext.AssetsManager, this.m_inspectorContext.UpointsManager, new Action(quickBuildOrRemove));
        constructionView.AddQuickRemoveBtn(this.m_inspectorContext.AssetsManager, this.m_inspectorContext.UpointsManager, new Action(quickBuildOrRemove));
        ConstructionPriorityPanel constructionPriorityPanel = new ConstructionPriorityPanel((IUiElement) constructionView, this.m_inspectorContext.InputScheduler, this.m_inspectorContext.PrioritiesManager, this.Builder, (Func<Option<IEntityConstructionProgress>>) (() => this.m_entityProvider().ConstructionProgress));
        constructionPriorityPanel.PutToRightMiddleOf<ConstructionPriorityPanel>((IUiElement) constructionView, constructionPriorityPanel.GetSize(), Offset.Right((float) (-(double) constructionPriorityPanel.GetWidth() + 1.0)));
        this.AddUpdater(constructionPriorityPanel.Updater);
        Btn priorityBtn = this.Builder.NewBtn("Priority").SetButtonStyle(this.Builder.Style.Global.GeneralBtn).SetIcon("Assets/Unity/UserInterface/General/Priority.svg", new Offset?(Offset.All(6f))).AddToolTip(Tr.IncreasedPriority__ConstructionTooltip).OnClick((Action) (() => this.m_inspectorContext.InputScheduler.ScheduleInputCmd<ToggleConstructionPriorityCmd>(new ToggleConstructionPriorityCmd(this.m_entityProvider()))));
        priorityBtn.SetWidth<Btn>(28f);
        constructionView.AddCustomBtn(priorityBtn);
        constructionView.AddCancelBtn((Action) (() => this.m_inspectorContext.InputScheduler.ScheduleInputCmd<ToggleStaticEntityConstructionCmd>(new ToggleStaticEntityConstructionCmd(this.m_entityProvider()))), new Func<EntityValidationResult>(canBeDeconstructionCanceled));
        Txt txt = this.Builder.NewTxt("PendingDeconstructionInfo", (IUiElement) this.GetContentPanel()).SetText((LocStrFormatted) Tr.ConstructionState__WaitingForRemoval).SetAlignment(TextAnchor.MiddleCenter);
        TextStyle text1 = this.Builder.Style.Global.Text;
        ref TextStyle local = ref text1;
        int? nullable = new int?(14);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
        Txt clearingProductTxt = txt.SetTextStyle(textStyle).PutTo<Txt>((IUiElement) this.GetContentPanel());
        Btn cancelClearingBtn = this.Builder.NewBtnDanger("Cancel").SetText((LocStrFormatted) Tr.Cancel).OnClick((Action) (() => this.m_inspectorContext.InputScheduler.ScheduleInputCmd<ToggleStaticEntityConstructionCmd>(new ToggleStaticEntityConstructionCmd(this.m_entityProvider())))).Hide<Btn>();
        cancelClearingBtn.PutToCenterBottomOf<Btn>((IUiElement) this.GetContentPanel(), cancelClearingBtn.GetOptimalSize(), Offset.Bottom(10f));
        this.SetContentSize(0.0f, 135f);
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<ConstructionState>((Func<ConstructionState>) (() => this.m_entityProvider().ConstructionState)).Observe<bool>((Func<bool>) (() =>
        {
          IEntityConstructionProgress valueOrNull = this.m_entityProvider().ConstructionProgress.ValueOrNull;
          return valueOrNull != null && valueOrNull.IsPaused;
        })).Do((Action<ConstructionState, bool>) ((state, isPaused) =>
        {
          ColorRgba colorRgba;
          LocStr text2;
          switch (state)
          {
            case ConstructionState.PreparingUpgrade:
              colorRgba = StaticEntityInspectorBase<T>.ConstructionWindow.PURPLE;
              text2 = Tr.ConstrType_PreparingUpgrade;
              break;
            case ConstructionState.BeingUpgraded:
              colorRgba = StaticEntityInspectorBase<T>.ConstructionWindow.PURPLE;
              text2 = Tr.ConstrType_Upgrading;
              break;
            case ConstructionState.PendingDeconstruction:
            case ConstructionState.InDeconstruction:
              if (isPaused)
              {
                colorRgba = ColorRgba.Gray;
                text2 = Tr.ConstrType_DeconstructionPaused;
                break;
              }
              colorRgba = StaticEntityInspectorBase<T>.ConstructionWindow.RED;
              text2 = Tr.ConstrType_Deconstructing;
              break;
            default:
              if (isPaused)
              {
                colorRgba = ColorRgba.Gray;
                text2 = Tr.ConstrType_ConstructionPaused;
                break;
              }
              colorRgba = StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE;
              text2 = Tr.ConstrType_Constructing;
              break;
          }
          connectorLeft.SetBackground(colorRgba);
          connectorRight.SetBackground(colorRgba);
          this.SetTitleBg(colorRgba);
          this.SetTitle((LocStrFormatted) text2);
          constructionView.SetVisibility<ConstructionProgressView>(state != ConstructionState.PendingDeconstruction);
          clearingProductTxt.SetVisibility<Txt>(state == ConstructionState.PendingDeconstruction);
          cancelClearingBtn.SetVisibility<Btn>(state == ConstructionState.PendingDeconstruction);
        }));
        updaterBuilder.Observe<bool>((Func<bool>) (() =>
        {
          IEntityConstructionProgress valueOrNull = this.m_entityProvider().ConstructionProgress.ValueOrNull;
          return valueOrNull != null && valueOrNull.IsPriority;
        })).Do((Action<bool>) (isPriority =>
        {
          if (isPriority)
            priorityBtn.SetButtonStyle(this.Builder.Style.Global.GeneralBtnActive);
          else
            priorityBtn.SetButtonStyle(this.Builder.Style.Global.GeneralBtn);
        }));
        this.AddUpdater(updaterBuilder.Build());
        this.AddUpdater(constructionView.Updater);

        void onPauseToggled(bool isPaused)
        {
          this.m_inspectorContext.InputScheduler.ScheduleInputCmd<SetConstructionPausedCmd>(new SetConstructionPausedCmd(this.m_entityProvider().Id, new bool?(isPaused)));
        }

        void quickBuildOrRemove()
        {
          this.m_inspectorContext.InputScheduler.ScheduleInputCmd<FinishBuildOfStaticEntityCmd>(new FinishBuildOfStaticEntityCmd(this.m_entityProvider().Id, true));
        }

        EntityValidationResult canBeDeconstructionCanceled()
        {
          IStaticEntity staticEntity = this.m_entityProvider();
          return staticEntity.IsConstructed || staticEntity.ConstructionState == ConstructionState.PreparingUpgrade || staticEntity.ConstructionState == ConstructionState.BeingUpgraded ? EntityValidationResult.Success : this.m_inspectorContext.EntitiesManager.CanRemoveEntity((IEntity) staticEntity, EntityRemoveReason.Remove);
        }
      }

      public void PositionSelf(WindowView parentWindow)
      {
        this.PutToBottomOf<StaticEntityInspectorBase<T>.ConstructionWindow>((IUiElement) parentWindow, this.GetHeight(), Offset.Bottom((float) -((double) this.GetHeight() + (double) StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT)));
      }

      static ConstructionWindow()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        StaticEntityInspectorBase<T>.ConstructionWindow.RED = (ColorRgba) 9123601;
        StaticEntityInspectorBase<T>.ConstructionWindow.ORANGE = (ColorRgba) 9594374;
        StaticEntityInspectorBase<T>.ConstructionWindow.PURPLE = (ColorRgba) 7146114;
        StaticEntityInspectorBase<T>.ConstructionWindow.GRAY = (ColorRgba) 6710886;
        StaticEntityInspectorBase<T>.ConstructionWindow.CONNECTOR_HEIGHT = 15;
      }
    }
  }
}
