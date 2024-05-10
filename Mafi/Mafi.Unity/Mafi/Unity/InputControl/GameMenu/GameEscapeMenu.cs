// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.GameEscapeMenu
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.SaveGame;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.GameMenu.LoadSave;
using Mafi.Unity.InputControl.GameMenu.Settings;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  public class GameEscapeMenu : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly IMain m_main;
    private readonly ISaveManager m_saveManager;
    private readonly IGameSaveInfoProvider m_gameSaveInfoProvider;
    private readonly ITutorialProgressCleaner m_progressCleaner;
    private readonly GameDifficultyApplier m_difficultyApplier;
    private readonly IInputScheduler m_inputScheduler;
    private readonly RandomProvider m_randomProvider;
    private readonly ShortcutsManager m_shortcutsManager;
    private Option<SaveWindow> m_saveWindow;
    private Option<LoadWindow> m_loadWindow;
    private Option<DifficultySettingsWindow> m_difficultyWindow;
    private Option<SettingsWindow> m_settingsWindow;
    private Option<PatchNotesWindow> m_patchNotesWindow;

    public GameEscapeMenu(
      IMain main,
      ISaveManager saveManager,
      IGameSaveInfoProvider gameSaveInfoProvider,
      ITutorialProgressCleaner progressCleaner,
      GameDifficultyApplier difficultyApplier,
      IInputScheduler inputScheduler,
      RandomProvider randomProvider,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(LocStrFormatted.Empty, true, true);
      this.m_main = main;
      this.m_saveManager = saveManager;
      this.m_gameSaveInfoProvider = gameSaveInfoProvider;
      this.m_progressCleaner = progressCleaner;
      this.m_difficultyApplier = difficultyApplier;
      this.m_inputScheduler = inputScheduler;
      this.m_randomProvider = randomProvider;
      this.m_shortcutsManager = shortcutsManager;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.Size<GameEscapeMenu>(300.px(), Px.Auto);
      this.Body.Padding<Column>(4.pt());
      Column body = this.Body;
      UiComponent[] uiComponentArray = new UiComponent[4]
      {
        (UiComponent) new ButtonPrimary((LocStrFormatted) Tr.Menu__Continue, (Action) (() => this.SetVisible(false))).Margin<ButtonPrimary>(1.pt()).MarginBottom<ButtonPrimary>(5.pt()),
        null,
        null,
        null
      };
      Column component = new Column(Outer.Panel, gap: new Px?(3.pt()));
      component.Add<Column>((Action<Column>) (c => c.Padding<Column>(4.pt()).AlignItemsStretch<Column>().FlexGrow<Column>(1f)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__Save, new Action(this.handleSave)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__Load, new Action(this.handleLoad)).MarginBottom<ButtonBold>(2.pt()));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__DifficultySettings, new Action(this.handleDifficultySettings)));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__OpenSettings, new Action(this.handleSettings)).MarginBottom<ButtonBold>(2.pt()));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.PatchNotes, new Action(this.handlePatchNotes)).MarginBottom<ButtonBold>(2.pt()));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.ExitToMainMenu, new Action(this.handleMainMenu)).Class<ButtonBold>(Cls.bold));
      component.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.QuitGame, new Action(this.handleQuit)).Class<ButtonBold>(Cls.bold));
      uiComponentArray[1] = (UiComponent) component;
      uiComponentArray[2] = (UiComponent) new Label((Tr.GameSeed.TranslatedString + ": " + this.m_randomProvider.MasterSeed).AsLoc()).AlignTextCenter<Label>().MarginTop<Label>(4.pt()).MarginBottom<Label>(1.pt());
      uiComponentArray[3] = (UiComponent) new Label(GameVersion.FULL_DISPLAY_VALUE).AlignTextCenter<Label>();
      body.Add(uiComponentArray);
      this.m_updater = updaterBuilder.Build().SomeOption<IUiUpdater>();
    }

    private void handleSave()
    {
      if (this.m_saveWindow.IsNone)
      {
        this.m_saveWindow = (Option<SaveWindow>) new SaveWindow(this.m_main, this.m_saveManager, this.m_gameSaveInfoProvider);
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_saveWindow.Value);
      }
      this.m_saveWindow.Value.Show<SaveWindow>();
    }

    private void handleLoad()
    {
      if (this.m_loadWindow.IsNone)
      {
        this.m_loadWindow = (Option<LoadWindow>) new LoadWindow(this.m_main, (Option<string>) this.m_saveManager.GameName);
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_loadWindow.Value);
      }
      this.m_loadWindow.Value.Show<LoadWindow>();
    }

    private void handleDifficultySettings()
    {
      if (this.m_difficultyWindow.IsNone)
      {
        this.m_difficultyWindow = (Option<DifficultySettingsWindow>) new DifficultySettingsWindow(this.m_difficultyApplier, this.m_inputScheduler);
        this.m_difficultyWindow.Value.OnHide((Action) (() =>
        {
          this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_difficultyWindow.Value);
          this.m_difficultyWindow = (Option<DifficultySettingsWindow>) Option.None;
        }));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_difficultyWindow.Value);
      }
      this.m_difficultyWindow.Value.Show<DifficultySettingsWindow>();
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

    private void handlePatchNotes()
    {
      if (this.m_patchNotesWindow.IsNone)
      {
        this.m_patchNotesWindow = (Option<PatchNotesWindow>) new PatchNotesWindow(false, (Option<string>) Option.None);
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_patchNotesWindow.Value);
      }
      this.m_patchNotesWindow.Value.Show<PatchNotesWindow>();
    }

    private void handleMainMenu()
    {
      if (this.m_saveManager.IsNonAutosaveInProgress())
      {
        this.RunWithBuilder((Action<UiBuilder>) (builder => builder.ShowGeneralNotification("Can't quit, save in progress".ToDoTranslate())));
        Log.Warning("Prevented game quit during save");
      }
      else if (this.m_saveManager.CanQuitGameWithoutSave())
      {
        this.toMainMenu();
      }
      else
      {
        QuitConfirmDialog dlg = new QuitConfirmDialog(QuitConfirmFlavor.ExitToMainMenu, new Action(this.toMainMenu));
        dlg.OnHide((Action) (() => this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg)));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg);
      }
    }

    private void handleQuit()
    {
      if (this.m_saveManager.IsNonAutosaveInProgress())
      {
        this.RunWithBuilder((Action<UiBuilder>) (builder => builder.ShowGeneralNotification("Can't quit, save in progress".ToDoTranslate())));
        Log.Warning("Prevented game quit during save");
      }
      else if (this.m_saveManager.CanQuitGameWithoutSave())
      {
        this.toQuit();
      }
      else
      {
        QuitConfirmDialog dlg = new QuitConfirmDialog(QuitConfirmFlavor.QuitGame, new Action(this.toQuit));
        dlg.OnHide((Action) (() => this.RemoveChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg)));
        this.AddChildWindow((Mafi.Unity.UiToolkit.Library.Window) dlg);
      }
    }

    private void toMainMenu() => this.m_main.GoToMainMenu(new MainMenuArgs());

    private void toQuit() => this.m_main.QuitGame();
  }
}
