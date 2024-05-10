// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.MainMenuScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.GameMenu;
using Mafi.Unity.InputControl.GameMenu.LoadSave;
using Mafi.Unity.InputControl.GameMenu.Settings;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.MainMenu.NewGame;
using Mafi.Unity.Playground;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class MainMenuScreen : Column
  {
    private static readonly Px HORIZ_PADDING;
    private static readonly Px VERT_PADDING;
    public static readonly string REPORT_ISSUE_URL;
    public const string COI_HUB_URL = "https://hub.coigame.com";
    private readonly IMain m_main;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly UiBuilder m_builder;
    private readonly PreInitModsAndProtos m_modsAndProtos;
    private SaveFileInfo? m_continueFile;
    private readonly Option<string> m_lastSeenVersion;
    private Option<NewGameWindow> m_newGameWindow;
    private Option<LoadWindow> m_loadWindow;
    private Option<SettingsWindow> m_settingsWindow;
    private Option<PatchNotesWindow> m_patchNotesWindow;
    private UiPlayground m_playground;
    private Set<Mafi.Unity.UiToolkit.Library.Window> m_childWindows;
    private readonly Label m_continueFileName;

    public MainMenuScreen(
      IMain main,
      IFileSystemHelper fileSystemHelper,
      UiBuilder builder,
      PreInitModsAndProtos modsAndProtos,
      DependencyResolver resolver,
      Option<string> lastSeenVersion,
      Action showCredits)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_childWindows = new Set<Mafi.Unity.UiToolkit.Library.Window>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_builder = builder;
      this.m_modsAndProtos = modsAndProtos;
      this.m_lastSeenVersion = lastSeenVersion;
      this.AbsolutePositionFillParent<MainMenuScreen>().AlignItemsCenter<MainMenuScreen>().Background<MainMenuScreen>("Assets/Unity/UserInterface/MainMenu/MenuBackground.jpg").BackgroundCover<MainMenuScreen>();
      this.m_continueFile = this.m_main.FileSystemHelper.GetLatestSaveFile();
      Tr.Version.Format("Update 2 v0.6.3a " + string.Format("(b{0})", (object) 333U));
      bool isSupporter = modsAndProtos.LoadedMods.Any((Func<IMod, bool>) (x => x.GetType().FullName == "Mafi.Supporter.SupporterMod"));
      string texturePath = "Assets/Supporter/SupporterEdition-512x56.png";
      UiComponent[] uiComponentArray = new UiComponent[4];
      Column component1 = new Column();
      component1.Add<Column>((Action<Column>) (c => c.PaddingTop<Column>(10.pt()).AlignItemsCenter<Column>()));
      component1.Add((UiComponent) new Img("Assets/Unity/UserInterface/MainMenu/CoiLogo-1024x80.png").MarginBottom<Img>(5.pt()));
      component1.Add(isSupporter ? (UiComponent) new Img(texturePath) : (UiComponent) null);
      Button button1 = new Button((Action) (() => Application.OpenURL("https://www.captain-of-industry.com/post/update2-is-out?ref=game")));
      button1.Add<Button>((Action<Button>) (c =>
      {
        Button component2 = c;
        Px? top = new Px?(0.px());
        Px? nullable = new Px?(-20.px());
        Px? right = new Px?();
        Px? bottom = new Px?();
        Px? left = nullable;
        component2.AbsolutePosition<Button>(top, right, bottom, left).Translate<Button>(120.Percent(), isSupporter ? 70.Percent() : 40.Percent()).Scale<Button>(0.55f, 0.55f).Rotate<Button>(new int?(-3));
      }));
      button1.Add((UiComponent) new Img("Assets/Unity/UserInterface/MainMenu/Update2Badge.png"));
      component1.Add((UiComponent) button1);
      uiComponentArray[0] = (UiComponent) component1;
      UiComponent component3 = new UiComponent();
      component3.Add<UiComponent>((Action<UiComponent>) (c => c.MarginTopBottom<UiComponent>(Px.Auto)));
      component3.Add(new UiComponent().Class<UiComponent>(Cls.windowShadow));
      Column column = new Column(Outer.WindowBackground);
      column.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>().Padding<Column>(4.pt()).Width<Column>(new Px?(300.px())).Gap<Column>(new Px?(3.pt()))));
      ButtonColumn buttonColumn = new ButtonColumn(new Action(this.handleContinue), Outer.ShadowCutCorner);
      buttonColumn.Add<ButtonColumn>((Action<ButtonColumn>) (c => c.Variant<ButtonColumn>(ButtonVariant.Primary).Enabled<ButtonColumn>(this.m_continueFile.HasValue).AlignItemsCenter<ButtonColumn>().Margin<ButtonColumn>(1.pt()).MarginBottom<ButtonColumn>(2.pt())));
      buttonColumn.Add((UiComponent) new Label((LocStrFormatted) Tr.Menu__Continue).UpperCase().FontSize<Label>(16).MarginBottom<Label>(1.pt()));
      buttonColumn.Add((UiComponent) (this.m_continueFileName = new Label((this.m_continueFile.HasValue ? this.m_continueFile.Value.NameNoExtension : "").AsLoc()).Color<Label>(new ColorRgba?(Theme.Text)).AlignTextCenter<Label>().FontNormal<Label>()));
      column.Add((UiComponent) buttonColumn);
      Panel panel = new Panel();
      panel.Add<Panel>((Action<Panel>) (c => c.Gap<Panel>(new Px?(3.pt())).RootPanel()));
      panel.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__NewGame, new Action(this.handleNewGame)));
      panel.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__Load, new Action(this.handleLoad)).MarginBottom<ButtonBold>(3.pt()));
      panel.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__OpenSettings, new Action(this.handleSettings)).MarginBottom<ButtonBold>(3.pt()));
      panel.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.Menu__MapEditor, new Action(this.handleMapEditor)).MarginBottom<ButtonBold>(3.pt()));
      panel.Add((UiComponent) new ButtonBold((LocStrFormatted) Tr.QuitGame, new Action(this.handleQuit)));
      column.Add((UiComponent) panel);
      column.Add((UiComponent) new Label(GameVersion.FULL_DISPLAY_VALUE).AlignTextCenter<Label>().MarginTop<Label>(1.pt()));
      component3.Add((UiComponent) column);
      uiComponentArray[1] = component3;
      Button button2 = new Button((Action) (() => Application.OpenURL("https://www.captain-of-industry.com/translate")));
      button2.Add((UiComponent) new Img("Assets/Unity/UserInterface/MainMenu/HelpTranslate-400x64.png").Class<Img>(Cls.iconClickable, Cls.growOnHover).Size<Img>(313.px(), 50.px()).MarginBottom<Img>(2.pt()));
      uiComponentArray[2] = (UiComponent) button2;
      Column component4 = new Column(Outer.Panel, gap: new Px?(2.pt()));
      component4.Add<Column>((Action<Column>) (c =>
      {
        Column component5 = c;
        Px? nullable1 = new Px?(MainMenuScreen.HORIZ_PADDING);
        Px? nullable2 = new Px?(5.pt());
        Px? top = new Px?();
        Px? right = nullable1;
        Px? bottom = nullable2;
        Px? left = new Px?();
        component5.AbsolutePosition<Column>(top, right, bottom, left).AlignItemsStretch<Column>().Padding<Column>(3.pt()).PaddingRight<Column>(10.pt()).MarginRight<Column>(-13.pt());
      }));
      component4.Add((UiComponent) new ButtonText((LocStrFormatted) Tr.PatchNotes, new Action(this.handlePatchNotes)));
      component4.Add((UiComponent) new ButtonText((LocStrFormatted) TrCore.Credits, showCredits));
      component4.Add((UiComponent) new ButtonOpenInBrowser((LocStrFormatted) TrCore.COIHub, (Action) (() => Application.OpenURL("https://hub.coigame.com"))).MarginTop<ButtonOpenInBrowser>(2.pt()));
      component4.Add((UiComponent) new ButtonOpenInBrowser((LocStrFormatted) TrCore.Suggestions, (Action) (() => Application.OpenURL("https://ideas.captain-of-industry.com/"))));
      component4.Add((UiComponent) new ButtonOpenInBrowser((LocStrFormatted) TrCore.ReportIssue, (Action) (() => Application.OpenURL(MainMenuScreen.REPORT_ISSUE_URL))));
      component4.Add((UiComponent) new ButtonOpenInBrowser((LocStrFormatted) TrCore.MailingList, (Action) (() => Application.OpenURL("https://www.captain-of-industry.com/club?ref=game"))));
      component4.Add((UiComponent) new ButtonOpenInBrowser((LocStrFormatted) Tr.Menu__Discord, (Action) (() => Application.OpenURL("https://discord.gg/JxmUbGsNRU"))));
      uiComponentArray[3] = (UiComponent) component4;
      this.Add(uiComponentArray);
      if (!this.m_lastSeenVersion.HasValue)
        return;
      this.handlePatchNotes();
    }

    public bool InputUpdate()
    {
      foreach (Mafi.Unity.UiToolkit.Library.Window childWindow in this.m_childWindows)
      {
        if (childWindow.IsVisible() && childWindow.InputUpdate())
          return true;
      }
      return false;
    }

    private void handleNewGame()
    {
      this.m_newGameWindow = (Option<NewGameWindow>) new NewGameWindow(this.m_main, this.m_modsAndProtos, new Action<Mafi.Unity.UiToolkit.Library.Window>(this.onWindowClose), true);
      this.addWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_newGameWindow.Value);
    }

    private void handleLoad()
    {
      if (this.m_loadWindow.IsNone)
      {
        this.m_loadWindow = (Option<LoadWindow>) new LoadWindow(this.m_main, darkMask: true);
        this.addWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_loadWindow.Value);
      }
      this.m_loadWindow.Value.Show<LoadWindow>();
    }

    private void handleSettings()
    {
      if (this.m_settingsWindow.IsNone)
      {
        this.m_settingsWindow = (Option<SettingsWindow>) new SettingsWindow(this.m_main, (ITutorialProgressCleaner) new StaticTutorialProgressCleaner(), Option<ShortcutsManager>.None, true);
        this.addWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_settingsWindow.Value);
      }
      this.m_settingsWindow.Value.Show<SettingsWindow>();
    }

    private void handleMapEditor()
    {
      this.m_main.StartMapEditor(ImmutableArray<IConfig>.Empty, this.m_main.Mods.FilterCoreMods());
    }

    private void handleQuit() => this.m_main.QuitGame();

    private void handlePatchNotes()
    {
      if (this.m_patchNotesWindow.IsNone)
      {
        this.m_patchNotesWindow = (Option<PatchNotesWindow>) new PatchNotesWindow(this.m_lastSeenVersion.HasValue, this.m_lastSeenVersion, true);
        this.addWindow((Mafi.Unity.UiToolkit.Library.Window) this.m_patchNotesWindow.Value);
      }
      this.m_patchNotesWindow.Value.Show<PatchNotesWindow>();
    }

    private void handlePlayground()
    {
      this.addWindow(this.m_playground.Window);
      this.m_playground.Show();
    }

    private void addWindow(Mafi.Unity.UiToolkit.Library.Window window)
    {
      this.RunWithBuilder((Action<UiBuilder>) (bld => bld.AddComponent((UiComponent) window)));
      this.m_childWindows.Add(window);
    }

    private void onWindowClose(Mafi.Unity.UiToolkit.Library.Window window)
    {
      this.m_childWindows.Remove(window);
    }

    private void handleContinue()
    {
      if (!this.m_continueFile.HasValue)
        return;
      if (!this.m_fileSystemHelper.FileExists(this.m_continueFile.Value.NameNoExtension, FileType.GameSave, subDir: this.m_continueFile.Value.GameName))
      {
        this.m_continueFile = this.m_fileSystemHelper.GetLatestSaveFile();
        Label continueFileName = this.m_continueFileName;
        ref SaveFileInfo? local = ref this.m_continueFile;
        LocStrFormatted? nullable;
        if (!local.HasValue)
        {
          nullable = new LocStrFormatted?();
        }
        else
        {
          string nameNoExtension = local.GetValueOrDefault().NameNoExtension;
          nullable = nameNoExtension != null ? new LocStrFormatted?(nameNoExtension.AsLoc()) : new LocStrFormatted?();
        }
        LocStrFormatted text = nullable ?? LocStrFormatted.Empty;
        continueFileName.Text<Label>(text);
      }
      else
      {
        Option<LoadFailInfo> errorOnFileLoad;
        ImmutableArray<ModData> modsToLoad;
        bool flag = this.isMissingMods(this.m_continueFile.Value, out errorOnFileLoad, out modsToLoad);
        if (errorOnFileLoad.HasValue)
          this.addWindow((Mafi.Unity.UiToolkit.Library.Window) new SaveFileIssueDialog(this.m_continueFile.Value, errorOnFileLoad.Value));
        else if (flag)
          this.addWindow((Mafi.Unity.UiToolkit.Library.Window) new SaveFileIssueDialog(this.m_continueFile.Value, new LoadFailInfo(LoadFailInfo.Reason.ModsMissing)));
        else
          this.m_main.LoadGame(this.m_continueFile.Value, new ImmutableArray<ModData>?(modsToLoad));
      }
    }

    private bool isMissingMods(
      SaveFileInfo saveInfo,
      out Option<LoadFailInfo> errorOnFileLoad,
      out ImmutableArray<ModData> modsToLoad)
    {
      LoadedModsStatus modsStatus = this.m_main.GetModsStatus(ModHelper.LoadModsFromSaveFile(this.m_fileSystemHelper, saveInfo, out errorOnFileLoad));
      modsToLoad = ImmutableArray<ModData>.Empty;
      if (errorOnFileLoad.HasValue || modsStatus.IsAnyUnavailableModSelected())
        return true;
      modsToLoad = modsStatus.GetSelectedAvailableThirdPartyMods();
      return false;
    }

    static MainMenuScreen()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MainMenuScreen.HORIZ_PADDING = 5.pt();
      MainMenuScreen.VERT_PADDING = 4.pt();
      MainMenuScreen.REPORT_ISSUE_URL = "https://github.com/MaFi-Games/Captain-of-Industry-issues/" + string.Format("issues/new?template=issue-report-form.yml&version=v{0}%20b{1}", (object) "0.6.3a", (object) 333U);
    }
  }
}
