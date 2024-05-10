// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.MiscTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class MiscTab : Column
  {
    private readonly ITutorialProgressCleaner m_progressCleaner;
    private readonly Row m_tutorialRow;

    public MiscTab(ITutorialProgressCleaner progressCleaner)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(2.pt());
      this.m_progressCleaner = progressCleaner;
      this.AlignItemsStretch<MiscTab>().Fill<MiscTab>().PaddingLeft<MiscTab>(SettingsWindow.SECTION_INDENT).PaddingBottom<MiscTab>(10.pt());
      UiComponent[] uiComponentArray = new UiComponent[5]
      {
        (UiComponent) new SettingsTitle((LocStrFormatted) Tr.MiscellaneousSettings_Title),
        (UiComponent) new Dropdown<int>().Label<Dropdown<int>>((LocStrFormatted) Tr.Autosave__Interval).LabelWidth<Dropdown<int>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<int>>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<int, int, bool, UiComponent>) ((mins, _1, _2) => (UiComponent) new Label(mins == 0 ? (LocStrFormatted) Tr.Off_Option : Tr.Autosave__Interval_Minutes.Format(mins.ToString(), mins)))).SetOptions(SaveManager.AUTOSAVE_OPTIONS_MINUTES.AsEnumerable()).SetValue(GlobalPlayerPrefs.AutosaveIntervalMins).OnValueChanged((Action<int, int>) ((mins, _) => GlobalPlayerPrefs.AutosaveIntervalMins = mins)),
        null,
        null,
        null
      };
      Row row = new Row(2.pt());
      row.Add((UiComponent) new Label((LocStrFormatted) Tr.TutorialReset__Title).Tooltip<Label>(new LocStrFormatted?((LocStrFormatted) Tr.TutorialReset__Tooltip)).Width<Label>(SettingsWindow.LABEL_WIDTH).Height<Label>(new Px?(29.px())).AlignText<Label>(TextAlign.LeftMiddle));
      row.Add((UiComponent) new ButtonText((LocStrFormatted) Tr.TutorialReset__Action, new Action(this.onResetTutorials)).Fill<ButtonText>());
      Row component = row;
      this.m_tutorialRow = row;
      uiComponentArray[2] = (UiComponent) component.Width<Row>(SettingsWindow.COMBINED_WIDTH);
      uiComponentArray[3] = (UiComponent) new Toggle().JustifyItemsStart<Toggle>().Label<Toggle>((LocStrFormatted) Tr.EnableMods__ToggleLabel).Tooltip<Toggle>(new LocStrFormatted?((LocStrFormatted) Tr.EnableMods__Tooltip)).LabelWidth<Toggle>(SettingsWindow.LABEL_WIDTH).Width<Toggle>(SettingsWindow.COMBINED_WIDTH).Value(GlobalPlayerPrefs.EnableMods).OnValueChanged((Action<bool>) (on => GlobalPlayerPrefs.EnableMods = on));
      uiComponentArray[4] = (UiComponent) new Toggle().JustifyItemsStart<Toggle>().Label<Toggle>((LocStrFormatted) Tr.ErrorReporting__Title).Tooltip<Toggle>(new LocStrFormatted?((LocStrFormatted) Tr.ErrorReporting__Tooltip)).LabelWidth<Toggle>(SettingsWindow.LABEL_WIDTH).Width<Toggle>(SettingsWindow.COMBINED_WIDTH).Value(!GlobalPlayerPrefs.DisableAnonymousErrorLogs).OnValueChanged((Action<bool>) (on => GlobalPlayerPrefs.DisableAnonymousErrorLogs = !on));
      this.Add(uiComponentArray);
    }

    private void onResetTutorials()
    {
      this.m_progressCleaner.ResetTutorialProgress();
      this.m_tutorialRow[1].RemoveFromHierarchy();
      this.m_tutorialRow.Add((UiComponent) new Label((LocStrFormatted) Tr.TutorialReset__ResetDone));
    }
  }
}
