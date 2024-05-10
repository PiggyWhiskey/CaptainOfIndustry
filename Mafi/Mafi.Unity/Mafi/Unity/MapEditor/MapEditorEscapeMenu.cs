// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditorEscapeMenu
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.GameMenu;
using Mafi.Unity.InputControl.GameMenu.Settings;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class MapEditorEscapeMenu : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly IMain m_main;
    private readonly ITutorialProgressCleaner m_progressCleaner;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly Mafi.Unity.MapEditor.MapEditor m_mapEditor;
    private Option<SettingsWindow> m_settingsWindow;

    public MapEditorEscapeMenu(
      IMain main,
      ITutorialProgressCleaner progressCleaner,
      IResolver resolver,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(LocStrFormatted.Empty, true, true);
      this.m_main = main;
      this.m_progressCleaner = progressCleaner;
      this.m_shortcutsManager = shortcutsManager;
      this.m_mapEditor = resolver.Resolve<Mafi.Unity.MapEditor.MapEditor>();
      this.Size<MapEditorEscapeMenu>(300.px(), Px.Auto);
      this.Body.Padding<Column>(4.pt());
      Column body = this.Body;
      UiComponent[] uiComponentArray = new UiComponent[3]
      {
        (UiComponent) new ButtonPrimary((LocStrFormatted) Tr.Menu__Continue, (Action) (() => this.SetVisible(false))).Margin<ButtonPrimary>(1.pt()).MarginBottom<ButtonPrimary>(5.pt()),
        null,
        null
      };
      Column component = new Column(Outer.Panel, gap: new Px?(3.pt()));
      component.Add<Column>((Action<Column>) (c => c.Padding<Column>(4.pt()).AlignItemsStretch<Column>().FlexGrow<Column>(1f)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__Save, new Action(this.handleSave)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__Load, new Action(this.handleLoad)));
      component.Add((UiComponent) new ButtonBold("Publish".AsLoc(), new Action(this.handlePublish)));
      component.Add((UiComponent) new ButtonBold("New map".AsLoc(), new Action(this.handleNewMap)).MarginBottom<ButtonBold>(3.pt()));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__OpenSettings, new Action(this.handleSettings)).MarginBottom<ButtonBold>(3.pt()));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.ExitToMainMenu, new Action(this.handleMainMenu)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.QuitGame, new Action(this.handleQuit)));
      uiComponentArray[1] = (UiComponent) component;
      uiComponentArray[2] = (UiComponent) new Label(GameVersion.FULL_DISPLAY_VALUE).AlignTextCenter<Label>().MarginTop<Label>(4.pt());
      body.Add(uiComponentArray);
    }

    private void handleSave()
    {
      this.Hide<MapEditorEscapeMenu>();
      this.m_mapEditor.Save();
    }

    private void handleLoad()
    {
      this.Hide<MapEditorEscapeMenu>();
      this.m_mapEditor.ShowLoadTab();
    }

    private void handlePublish()
    {
      this.Hide<MapEditorEscapeMenu>();
      this.m_mapEditor.ShowPublishTab();
    }

    private void handleNewMap()
    {
      if (this.m_mapEditor.HasUnsavedChanges)
      {
        QuitConfirmDialog dlg = new QuitConfirmDialog(QuitConfirmFlavor.Continue, new Action(this.toNewMap));
        dlg.OnHide((Action) (() => this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg)));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg);
      }
      else
        this.toNewMap();
    }

    private void handleSettings()
    {
      if (this.m_settingsWindow.IsNone)
      {
        this.m_settingsWindow = (Option<SettingsWindow>) new SettingsWindow(this.m_main, this.m_progressCleaner, (Option<ShortcutsManager>) this.m_shortcutsManager);
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_settingsWindow.Value);
      }
      this.m_settingsWindow.Value.Show<SettingsWindow>();
    }

    private void handleMainMenu()
    {
      if (this.m_mapEditor.HasUnsavedChanges)
      {
        QuitConfirmDialog dlg = new QuitConfirmDialog(QuitConfirmFlavor.ExitToMainMenu, new Action(this.toMainMenu));
        dlg.OnHide((Action) (() => this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg)));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg);
      }
      else
        this.toMainMenu();
    }

    private void handleQuit()
    {
      if (this.m_mapEditor.HasUnsavedChanges)
      {
        QuitConfirmDialog dlg = new QuitConfirmDialog(QuitConfirmFlavor.QuitGame, new Action(this.toQuit));
        dlg.OnHide((Action) (() => this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg)));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg);
      }
      else
        this.toQuit();
    }

    private void toNewMap()
    {
      this.m_main.StartMapEditor(ImmutableArray<IConfig>.Empty, this.m_main.Mods.FilterCoreMods());
    }

    private void toMainMenu() => this.m_main.GoToMainMenu(new MainMenuArgs());

    private void toQuit() => this.m_main.QuitGame();
  }
}
