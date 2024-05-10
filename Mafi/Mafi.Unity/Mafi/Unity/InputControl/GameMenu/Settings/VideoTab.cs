// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.VideoTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class VideoTab : Column
  {
    private static readonly ImmutableArray<RenderingSetting> RENDERING_SETTINGS;
    private readonly Row m_presetContainer;
    private readonly Column m_renderingContainer;
    private readonly Dropdown<Vector2i> m_resolutionDropdown;

    public VideoTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(2.pt());
      this.AlignItemsStretch<VideoTab>().PaddingLeft<VideoTab>(SettingsWindow.SECTION_INDENT).PaddingBottom<VideoTab>(10.pt());
      Dict<FullScreenMode, LocStrFormatted> windowModes = new Dict<FullScreenMode, LocStrFormatted>()
      {
        {
          FullScreenMode.ExclusiveFullScreen,
          (LocStrFormatted) Tr.WindowMode__Option_Fullscreen
        },
        {
          FullScreenMode.FullScreenWindow,
          (LocStrFormatted) Tr.WindowMode__Option_Borderless
        },
        {
          FullScreenMode.Windowed,
          (LocStrFormatted) Tr.WindowMode__Option_Windowed
        }
      };
      List<DisplayInfo> displayInfoList = new List<DisplayInfo>();
      Screen.GetDisplayLayout(displayInfoList);
      UiComponent[] uiComponentArray = new UiComponent[17]
      {
        (UiComponent) new SettingsTitle((LocStrFormatted) Tr.ScreenSetting_Title),
        displayInfoList.Count > 1 ? (UiComponent) new Dropdown<DisplayInfo>().Label<Dropdown<DisplayInfo>>((LocStrFormatted) Tr.ActiveDisplay_Setting).LabelWidth<Dropdown<DisplayInfo>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<DisplayInfo>>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<DisplayInfo, int, bool, UiComponent>) ((d, idx, _) => (UiComponent) new Label(string.Format("#{0} ({1} x {2})", (object) (idx + 1), (object) d.width, (object) d.height).AsLoc()))).SetOptions((IEnumerable<DisplayInfo>) displayInfoList).SetValueIndex(PlayerPrefs.GetInt("UnitySelectMonitor").Clamp(0, Display.displays.Length - 1)).OnValueChanged(new Action<DisplayInfo, int>(this.onDisplayChanged)) : (UiComponent) null,
        (UiComponent) (this.m_resolutionDropdown = new Dropdown<Vector2i>().Label<Dropdown<Vector2i>>((LocStrFormatted) Tr.Resolution).LabelWidth<Dropdown<Vector2i>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<Vector2i>>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<Vector2i, int, bool, UiComponent>) ((r, _1, _2) => (UiComponent) new Label(string.Format("{0} x {1}", (object) r.X, (object) r.Y).AsLoc()))).OnValueChanged((Action<Vector2i, int>) ((res, _) => Screen.SetResolution(res.X, res.Y, Screen.fullScreenMode)))),
        (UiComponent) new Dropdown<LocStrFormatted>().Label<Dropdown<LocStrFormatted>>((LocStrFormatted) Tr.WindowMode__Title).LabelWidth<Dropdown<LocStrFormatted>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<LocStrFormatted>>(SettingsWindow.COMBINED_WIDTH).SetOptions((IEnumerable<LocStrFormatted>) windowModes.Values).SetValue(windowModes[Screen.fullScreenMode]).OnValueChanged((Action<LocStrFormatted, int>) ((_, idx) => Screen.fullScreenMode = windowModes.Keys.ElementAt<FullScreenMode>(idx))),
        (UiComponent) new VideoTab.RenderingSettingDropdown(GlobalGfxSettings.GameFpsLimitRenderingSetting),
        (UiComponent) new VideoTab.RenderingSettingDropdown(GlobalGfxSettings.MenusFpsLimitRenderingSetting),
        (UiComponent) new VideoTab.RenderingSettingDropdown(GlobalGfxSettings.BackgroundFpsLimitRenderingSetting),
        (UiComponent) new SettingsTitle((LocStrFormatted) Tr.UiSettings_Title).MarginTop<SettingsTitle>(SettingsWindow.SECTION_GAP),
        (UiComponent) new Dropdown<LocStrFormatted>().Label<Dropdown<LocStrFormatted>>(Tr.RestartRequiredSuffix.Format(Tr.Language.TranslatedString)).Tooltip<Dropdown<LocStrFormatted>>(new LocStrFormatted?((LocStrFormatted) Tr.BestEffortLocalized)).LabelWidth<Dropdown<LocStrFormatted>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<LocStrFormatted>>(SettingsWindow.COMBINED_WIDTH).SetOptions(LocalizationSettings.LanguageStrOptions.Select<LocStrFormatted>((Func<string, LocStrFormatted>) (s => s.AsLoc()))).SetValueIndex(LocalizationSettings.GetCurrentLangIndex()).OnValueChanged((Action<LocStrFormatted, int>) ((_, idx) => LocalizationSettings.SetNewLangIndex(idx))),
        (UiComponent) new Dropdown<int>().Label<Dropdown<int>>(Tr.RestartRequiredSuffix.Format(Tr.Scale.TranslatedString)).LabelWidth<Dropdown<int>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<int>>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<int, int, bool, UiComponent>) ((opt, _3, _4) => (UiComponent) new Label(string.Format("{0}%", (object) opt).AsLoc()))).SetOptions(UiScaleHelper.Options.AsEnumerable()).SetValueIndex(UiScaleHelper.GetCurrentScaleIndex()).OnValueChanged((Action<int, int>) ((_, idx) => UiScaleHelper.SetNewScaleIndex(idx))),
        (UiComponent) new SettingsTitle((LocStrFormatted) Tr.RenderingSetting_Title).MarginTop<SettingsTitle>(SettingsWindow.SECTION_GAP),
        null,
        null,
        null,
        null,
        null,
        null
      };
      Row component1 = new Row(2.pt());
      component1.Add<Row>((Action<Row>) (c => c.JustifyItemsSpaceBetween<Row>().PaddingBottom<Row>(4.pt())));
      component1.Add((UiComponent) new Label((LocStrFormatted) Tr.RenderingSettingPreset_Label));
      Row component2 = new Row(2.pt());
      component2.Add<Row>((Action<Row>) (c => c.MarginLeftRight<Row>(Px.Auto)));
      component2.Add((IEnumerable<UiComponent>) RenderingSettingsManager.Presets.Select<ButtonText>((Func<Pair<RenderingSettingPreset, LocStrFormatted>, ButtonText>) (preset => new ButtonText(preset.Second, (Action) (() => this.onPresetClick(preset.First))).Class<ButtonText>(Cls.bold).PaddingLeftRight<ButtonText>(3.pt()))));
      Row child = component2;
      this.m_presetContainer = component2;
      component1.Add((UiComponent) child);
      uiComponentArray[11] = (UiComponent) component1;
      Column column1 = new Column(2.pt());
      column1.Add((IEnumerable<UiComponent>) VideoTab.RENDERING_SETTINGS.Select<VideoTab.RenderingSettingDropdown>((Func<RenderingSetting, VideoTab.RenderingSettingDropdown>) (s => new VideoTab.RenderingSettingDropdown(s, new Action(this.refreshPresets)))));
      Column column2 = column1;
      this.m_renderingContainer = column1;
      uiComponentArray[12] = (UiComponent) column2;
      uiComponentArray[13] = (UiComponent) new SettingsTitle((LocStrFormatted) Tr.AccessibilitySetting__Title).MarginTop<SettingsTitle>(SettingsWindow.SECTION_GAP);
      uiComponentArray[14] = (UiComponent) new Toggle().JustifyItemsStart<Toggle>().Label<Toggle>((LocStrFormatted) Tr.AccessibilitySetting__Flashes).LabelWidth<Toggle>(SettingsWindow.LABEL_WIDTH).Width<Toggle>(SettingsWindow.COMBINED_WIDTH).Value(GlobalPlayerPrefs.DisableFlashes).OnValueChanged((Action<bool>) (on => GlobalPlayerPrefs.DisableFlashes = on));
      uiComponentArray[15] = (UiComponent) new SettingsTitle((LocStrFormatted) Tr.CameraSettings__Title).MarginTop<SettingsTitle>(SettingsWindow.SECTION_GAP);
      uiComponentArray[16] = (UiComponent) new Dropdown<float>().Label<Dropdown<float>>((LocStrFormatted) Tr.CameraSettings__Fov).LabelWidth<Dropdown<float>>(SettingsWindow.LABEL_WIDTH).Width<Dropdown<float>>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<float, int, bool, UiComponent>) ((opt, _5, _6) => (UiComponent) new Label(string.Format("{0}°", (object) opt).AsLoc()))).SetOptions(CameraSettingsHelper.FOV_VALUES.AsEnumerable()).SetValueIndex(CameraSettingsHelper.GetCurrentFovIndex()).OnValueChanged((Action<float, int>) ((v, _) => CameraSettingsHelper.SetAndSaveFov(v)));
      this.Add(uiComponentArray);
      this.refreshResolution();
      this.refreshPresets();
    }

    private void onPresetClick(RenderingSettingPreset preset)
    {
      RenderingSettingsManager.ApplyPreset(preset);
      this.refreshPresets();
    }

    private void refreshPresets()
    {
      for (int index = 0; index < RenderingSettingsManager.Presets.Length; ++index)
      {
        Pair<RenderingSettingPreset, LocStrFormatted> preset = RenderingSettingsManager.Presets[index];
        Button component = this.m_presetContainer.ChildAtOrDefault<Button>(index);
        bool isSelected = RenderingSettingsManager.IsPresetActive(preset.First);
        if (component != null)
          component.Variant<Button>(isSelected ? ButtonVariant.Primary : ButtonVariant.Default).Selected<Button>(isSelected);
      }
      for (int index = 0; index < VideoTab.RENDERING_SETTINGS.Length; ++index)
      {
        RenderingSetting renderingSetting = VideoTab.RENDERING_SETTINGS[index];
        ((Dropdown<RenderingSettingOption>) this.m_renderingContainer.ChildAtOrNone(index).ValueOrNull)?.SetValueIndex(renderingSetting.CurrentIndex);
      }
    }

    private void onDisplayChanged(DisplayInfo displayInfo, int displayIndex)
    {
      PlayerPrefs.SetInt("UnitySelectMonitor", displayIndex);
      PlayerPrefs.SetInt("Screenmanager Resolution Width", displayInfo.width);
      PlayerPrefs.SetInt("Screenmanager Resolution Height", displayInfo.height);
      PlayerPrefs.Save();
      this.moveToDisplay(displayInfo);
    }

    private void moveToDisplay(DisplayInfo display)
    {
      Vector2Int position = new Vector2Int(0, 0);
      if (Screen.fullScreenMode != FullScreenMode.Windowed)
      {
        position.x += display.width / 2;
        position.y += display.height / 2;
      }
      AsyncOperation op = Screen.MoveMainWindowTo(in display, position);
      waitForComplete();

      void waitForComplete()
      {
        if (op.isDone)
          this.refreshResolution();
        else
          this.Schedule.Execute(new Action(waitForComplete));
      }
    }

    private void refreshResolution()
    {
      Lyst<Vector2i> lyst = ((IEnumerable<Resolution>) Screen.resolutions).Select<Resolution, Vector2i>((Func<Resolution, Vector2i>) (x => new Vector2i(x.width, x.height))).Distinct<Vector2i>().OrderByDescending<Vector2i, int>((Func<Vector2i, int>) (r => r.X * r.Y)).ToLyst<Vector2i>();
      int? nullable = lyst.FirstIndexOf<Vector2i>((Predicate<Vector2i>) (r => r.X == Screen.width && r.Y == Screen.height));
      this.m_resolutionDropdown.SetOptions((IEnumerable<Vector2i>) lyst).SetValueIndex(nullable.GetValueOrDefault());
    }

    static VideoTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      VideoTab.RENDERING_SETTINGS = RenderingSettingsManager.AllRenderingSettings.Where((Func<RenderingSetting, bool>) (x => x.UsesRenderingPresets)).OrderBy<RenderingSetting, int>((Func<RenderingSetting, int>) (x => x.SortPriority)).ToImmutableArray<RenderingSetting>();
    }

    private class RenderingSettingDropdown : Dropdown<RenderingSettingOption>
    {
      public RenderingSettingDropdown(RenderingSetting setting, Action onChange = null)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Label<VideoTab.RenderingSettingDropdown>((LocStrFormatted) setting.Title).LabelWidth<VideoTab.RenderingSettingDropdown>(SettingsWindow.LABEL_WIDTH).Width<VideoTab.RenderingSettingDropdown>(SettingsWindow.COMBINED_WIDTH).SetOptionViewFactory((Func<RenderingSettingOption, int, bool, UiComponent>) ((opt, _1, _2) => !opt.IsSupported ? (UiComponent) new Label(opt.Name).Color<Label>(new ColorRgba?(Theme.BtnTextDisabled)).Tooltip<Label>(new LocStrFormatted?(Tr.RenderingSetting_NotSupported.Format(opt.Name))) : (UiComponent) new Label(opt.Name).Tooltip<Label>(new LocStrFormatted?(opt.Tooltip)))).SetOptions(setting.Options.AsEnumerable()).SetValue(setting.CurrentOption).OnValueChanged((Action<RenderingSettingOption, int>) ((_, idx) =>
        {
          setting.SetSettingIndex(idx);
          Action action = onChange;
          if (action == null)
            return;
          action();
        }));
      }
    }
  }
}
