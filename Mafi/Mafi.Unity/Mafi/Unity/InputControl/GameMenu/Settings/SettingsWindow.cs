// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.SettingsWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class SettingsWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    internal static readonly Percent LABEL_WIDTH;
    internal static readonly Percent COMBINED_WIDTH;
    internal static readonly Px SECTION_INDENT;
    internal static readonly Px SECTION_GAP;
    private readonly IMain m_main;
    private readonly TabContainer m_tabContainer;

    public SettingsWindow(
      IMain main,
      ITutorialProgressCleaner progressCleaner,
      Option<ShortcutsManager> shortcutsManager,
      bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) Tr.Settings_Title, darkMask: darkMask);
      this.m_main = main;
      this.Width<SettingsWindow>(new Px?(900.px())).GrowVertically();
      Column body = this.Body;
      TabContainer component = new TabContainer();
      component.Add<TabContainer>((Action<TabContainer>) (c => c.RootPanel().Panel.Fill<Panel>()));
      component.Add((LocStrFormatted) Tr.VideoSettings_Title, "Assets/Unity/UserInterface/General/Video.svg", (UiComponent) new VideoTab());
      component.Add((LocStrFormatted) Tr.AudioSettings_Title, "Assets/Unity/UserInterface/General/Audio.svg", (UiComponent) new AudioTab());
      component.Add((LocStrFormatted) Tr.ControlsSettings_Title, "Assets/Unity/UserInterface/General/RightClick.png", (UiComponent) new ControlsTab(shortcutsManager), Scroll.No);
      component.Add((LocStrFormatted) Tr.MiscellaneousSettings_Title, "Assets/Unity/UserInterface/General/Working128.png", (UiComponent) new MiscTab(progressCleaner));
      TabContainer tabContainer = component;
      this.m_tabContainer = component;
      TabContainer child = tabContainer;
      body.Add((UiComponent) child);
    }

    public override bool InputUpdate()
    {
      if (!this.IsVisible())
        return false;
      if (this.m_tabContainer.InputUpdate())
        return true;
      if (!Input.GetKeyDown(KeyCode.Escape))
        return false;
      this.Hide<SettingsWindow>();
      return true;
    }

    static SettingsWindow()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SettingsWindow.LABEL_WIDTH = 60.Percent();
      SettingsWindow.COMBINED_WIDTH = 66.Percent();
      SettingsWindow.SECTION_INDENT = 4.pt();
      SettingsWindow.SECTION_GAP = 5.pt();
    }
  }
}
