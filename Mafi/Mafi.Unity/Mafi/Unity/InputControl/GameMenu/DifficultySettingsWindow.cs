// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.DifficultySettingsWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.MainMenu.NewGame;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  public class DifficultySettingsWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly GameDifficultyApplier m_applier;
    private readonly IInputScheduler m_inputScheduler;
    private readonly AdvancedSettingsTab m_controls;
    private Option<DifficultySettingsWindow.ConfirmWindow> m_confirmWindow;

    public DifficultySettingsWindow(GameDifficultyApplier applier, IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) Tr.Menu__DifficultySettings);
      this.m_applier = applier;
      this.m_inputScheduler = inputScheduler;
      this.Size<DifficultySettingsWindow>(NewGameWindow.TARGET_WIDTH, 720.px());
      UpdaterBuilder builder = UpdaterBuilder.Start();
      Column body = this.Body;
      UiComponent[] uiComponentArray = new UiComponent[2];
      TabContainer component1 = new TabContainer();
      component1.Add<TabContainer>((Action<TabContainer>) (c => c.RootPanel()));
      component1.Add((LocStrFormatted) Tr.Menu__OpenSettings, "Assets/Unity/UserInterface/General/Working128.png", (UiComponent) (this.m_controls = new AdvancedSettingsTab(applier)));
      component1.Add(TrCore.ChangeHistory__Title.Format(applier.ChangeLog.AllChangeSets.Count.ToString()), "Assets/Unity/UserInterface/Toolbar/History.svg", (UiComponent) new DifficultySettingsWindow.HistoryLog(applier));
      uiComponentArray[0] = (UiComponent) component1;
      Row component2 = new Row(2.pt());
      component2.Add<Row>((Action<Row>) (c => c.PaddingTop<Row>(2.pt()).FlexNoShrink<Row>().JustifyItemsSpaceBetween<Row>()));
      component2.Add((UiComponent) new ButtonText((LocStrFormatted) Tr.DiscardChanges, new Action(this.handleDiscard)).FlipNotches<ButtonText>().Class<ButtonText>(Cls.bold).VisibleForRenderObserve<ButtonText>(builder, new Func<bool>(this.hasChanges)));
      component2.Add((UiComponent) new ButtonPrimary((LocStrFormatted) Tr.Save_Action, new Action(this.handleSave)).EnabledObserve<ButtonPrimary>(builder, new Func<bool>(this.hasChanges)));
      uiComponentArray[1] = (UiComponent) component2;
      body.Add(uiComponentArray);
      this.m_updater = builder.Build().SomeOption<IUiUpdater>();
    }

    public override bool InputUpdate()
    {
      DifficultySettingsWindow.ConfirmWindow valueOrNull = this.m_confirmWindow.ValueOrNull;
      return (valueOrNull != null ? (valueOrNull.InputUpdate() ? 1 : 0) : 0) != 0 || base.InputUpdate();
    }

    private bool hasChanges() => !this.m_applier.DifficultyConfig.IsSameAs(this.m_controls.Config);

    private void handleDiscard() => this.m_controls.ResetConfig();

    private void handleSave()
    {
      this.m_confirmWindow = (Option<DifficultySettingsWindow.ConfirmWindow>) new DifficultySettingsWindow.ConfirmWindow(this.m_applier, this.m_controls.Config, (Action) (() =>
      {
        this.SetVisible(false);
        this.m_inputScheduler.ScheduleInputCmd<ChangeGameDifficultyCmd>(new ChangeGameDifficultyCmd(this.m_controls.Config));
        this.RunWithBuilder((Action<UiBuilder>) (b => b.ShowSuccessNotification((LocStrFormatted) TrCore.DifficultySettingsSaved)));
      }));
      this.m_confirmWindow.Value.OnHide((Action) (() =>
      {
        this.m_confirmWindow.Value.RemoveFromHierarchy();
        this.m_confirmWindow = (Option<DifficultySettingsWindow.ConfirmWindow>) Option.None;
      }));
      this.RunWithBuilder((Action<UiBuilder>) (b => b.AddComponent((UiComponent) this.m_confirmWindow.Value)));
    }

    private class HistoryLog : Column
    {
      public HistoryLog(GameDifficultyApplier applier)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Gap<DifficultySettingsWindow.HistoryLog>(new Px?(4.pt()));
        this.AlignItemsStretch<DifficultySettingsWindow.HistoryLog>();
        this.Width<DifficultySettingsWindow.HistoryLog>(45.Percent());
        this.MarginLeftRight<DifficultySettingsWindow.HistoryLog>(Px.Auto);
        this.PaddingBottom<DifficultySettingsWindow.HistoryLog>(20.pt());
        if (applier.ChangeLog.AllChangeSets.Count <= 0)
          this.Add((UiComponent) new Label((LocStrFormatted) TrCore.ChangeHistory__EmptyLabel).AlignTextCenter<Label>().FontItalic<Label>());
        else
          this.Add((IEnumerable<UiComponent>) applier.ChangeLog.AllChangeSets.Select<GameDifficultyChangeLog.ChangeSet, Column>((Func<GameDifficultyChangeLog.ChangeSet, Column>) (change =>
          {
            Column component = new Column();
            component.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>()));
            component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Title(TrCore.DateYear__Label.Format(change.DateChanged.Year.ToString("N0"))).MarginBottom<Mafi.Unity.UiToolkit.Library.Title>(6.px()));
            component.Add((UiComponent) new DifficultySettingsWindow.HistoryChangeSet(change.Changes));
            return component;
          })));
      }
    }

    private class HistoryChangeSet : BulletedList
    {
      public HistoryChangeSet(ImmutableArray<GameDifficultyOptionChange> changes)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Add((IEnumerable<UiComponent>) changes.Select<Row>((Func<GameDifficultyOptionChange, Row>) (c1 =>
        {
          (LocStrFormatted setting2, string str3, string str4) = c1.GetStrings();
          Row component = new Row(1.pt());
          component.Add<Row>((Action<Row>) (c2 => c2.Wrap<Row>()));
          component.Add((UiComponent) new Label(string.Format("<b>{0}</b>:", (object) setting2).AsLoc()).MarginRight<Label>(1.pt()));
          component.Add((UiComponent) new Label(str3.AsLoc()));
          component.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Next.svg").Size<Icon>(12.px()));
          component.Add((UiComponent) new Label(str4.AsLoc()).Color<Label>(new ColorRgba?(c1.IsNewEasier ? Theme.PositiveColor : Theme.NegativeColor)));
          return component;
        })));
      }
    }

    private class ConfirmWindow : Mafi.Unity.UiToolkit.Library.Window
    {
      public ConfirmWindow(
        GameDifficultyApplier applier,
        GameDifficultyConfig newConfig,
        Action onSave)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((LocStrFormatted) TrCore.ChangeHistory__ConfirmTitle, darkMask: true);
        DifficultySettingsWindow.ConfirmWindow confirmWindow = this;
        this.Size<DifficultySettingsWindow.ConfirmWindow>(550.px(), 400.px());
        LocStrFormatted locStrFormatted = GameDifficultyApplier.DifficultyChangeTimeout.FormatYearsOrMonthsLong();
        ImmutableArray<GameDifficultyOptionChange> diff = applier.DifficultyConfig.GenerateDiff(newConfig);
        Column body = this.Body;
        UiComponent[] uiComponentArray = new UiComponent[2];
        Panel component1 = new Panel();
        component1.Add<Panel>((Action<Panel>) (c => c.Fill<Panel>().RootPanel()));
        ScrollColumn child = new ScrollColumn();
        child.Add((UiComponent) new Label(TrCore.ChangeHistory__ConfirmPrompt.Format(locStrFormatted)).MarginBottom<Label>(4.pt()).FontSize<Label>(16));
        child.Add((UiComponent) new DifficultySettingsWindow.HistoryChangeSet(diff).MarginBottom<DifficultySettingsWindow.HistoryChangeSet>(10.pt()));
        component1.Add((UiComponent) child);
        uiComponentArray[0] = (UiComponent) component1;
        Row component2 = new Row();
        component2.Add<Row>((Action<Row>) (c => c.FlexNoShrink<Row>().JustifyItemsSpaceBetween<Row>().PaddingTop<Row>(2.pt())));
        component2.Add((UiComponent) new ButtonText((LocStrFormatted) Tr.Cancel, (Action) (() => confirmWindow.SetVisible(false))).Class<ButtonText>(Cls.bold).FlipNotches<ButtonText>());
        component2.Add((UiComponent) new ButtonPrimary((LocStrFormatted) Tr.ApplyChanges, (Action) (() =>
        {
          onSave();
          confirmWindow.SetVisible(false);
        })));
        uiComponentArray[1] = (UiComponent) component2;
        body.Add(uiComponentArray);
      }
    }
  }
}
