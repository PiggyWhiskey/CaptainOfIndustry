// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.NewGameWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class NewGameWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    public static readonly Px TARGET_WIDTH;
    public static readonly Px TARET_HEIGHT;
    private readonly IMain m_main;
    private readonly PreInitModsAndProtos m_modsAndProtos;
    private readonly Action<Mafi.Unity.UiToolkit.Library.Window> m_onClose;
    private readonly NewGameConfigForUi m_settings;
    private readonly TabContainer m_tabContainer;
    private readonly Button m_coiHubButton;
    private readonly Button m_openMapsFolderButton;
    private readonly Button m_prevButton;
    private readonly Label m_nextButtonLabel;
    private readonly ButtonRow m_nextButton;

    public NewGameWindow(
      IMain main,
      PreInitModsAndProtos modsAndProtos,
      Action<Mafi.Unity.UiToolkit.Library.Window> onClose,
      bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) TrCore.NewGameWizard__Title, darkMask: darkMask);
      this.m_main = main;
      this.m_modsAndProtos = modsAndProtos;
      this.m_onClose = onClose;
      this.m_settings = new NewGameConfigForUi(main.FileSystemHelper)
      {
        DifficultyIndex = SelectDifficultyData.All.IndexOf(SelectDifficultyData.Captain),
        DifficultyConfig = SelectDifficultyData.Captain.CreateConfig(),
        RandomSeed = NewGameConfigForUi.GetRandomSeed()
      };
      NewGameWindow newGameWindow = this.Size<NewGameWindow>(NewGameWindow.TARGET_WIDTH, NewGameWindow.TARET_HEIGHT);
      float? nullable = new float?((float) NewGameWindow.TARET_HEIGHT);
      float? maxWidth = new float?(1600f);
      float? maxHeight = nullable;
      newGameWindow.Grow(maxWidth, maxHeight);
      Column body = this.Body;
      UiComponent[] uiComponentArray = new UiComponent[2];
      TabContainer component1 = new TabContainer();
      component1.Add<TabContainer>((Action<TabContainer>) (c => c.RootPanel().Panel.Fill<Panel>()));
      component1.Add((LocStrFormatted) Tr.ConfigureMods_Action, "Assets/Unity/UserInterface/General/Configure.svg", (UiComponent) new ModsTab(this.m_settings, main));
      component1.Add((LocStrFormatted) TrCore.NewGameWizard__MapSelection, "Assets/Unity/UserInterface/Toolbar/WorldMap.svg", (UiComponent) new MapTab(this.m_settings, main, modsAndProtos.ProtosDb, modsAndProtos.LoadedMods), Scroll.No);
      component1.Add((LocStrFormatted) TrCore.NewGameWizard__Difficulty, "Assets/Unity/UserInterface/General/Tradable128.png", (UiComponent) new DifficultyTab(this.m_settings), Scroll.No);
      component1.Add((LocStrFormatted) TrCore.NewGameWizard__Mechanics, "Assets/Unity/UserInterface/General/Build.svg", (UiComponent) new MechanicsTab(this.m_settings, new Action(this.showDifficultySettings)), Scroll.No);
      component1.Add((LocStrFormatted) TrCore.NewGameWizard__Customization, "Assets/Unity/UserInterface/General/Working128.png", (UiComponent) new CustomizationTab(this.m_settings, main.FileSystemHelper), Scroll.No);
      TabContainer tabContainer = component1;
      this.m_tabContainer = component1;
      uiComponentArray[0] = (UiComponent) tabContainer;
      Row component2 = new Row();
      component2.Add<Row>((Action<Row>) (c => c.PaddingTop<Row>(9.px()).PaddingLeftRight<Row>(2.px()).FlexNoShrink<Row>()));
      ButtonRow component3 = new ButtonRow(new Action(this.onPrevious), Outer.ShadowCutCorner);
      component3.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Default).Class<ButtonRow>(Cls.bold).Gap<ButtonRow>(new Px?(2.pt())).AlignItemsStart<ButtonRow>().FlipNotches<ButtonRow>().MarginRight<ButtonRow>(2.pt())));
      component3.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Return.svg").Small());
      component3.Add((UiComponent) new Label((LocStrFormatted) Tr.GoBack).UpperCase());
      Button child1 = (Button) component3;
      this.m_prevButton = (Button) component3;
      component2.Add((UiComponent) child1);
      component2.Add((UiComponent) (this.m_coiHubButton = (Button) new ButtonOpenInBrowser((LocStrFormatted) TrCore.OpenCoIHub, (Action) (() => Application.OpenURL("https://hub.coigame.com/maps")), true).Class<ButtonOpenInBrowser>(Cls.bold).FlexGrow<ButtonOpenInBrowser>(0.0f).MarginRight<ButtonOpenInBrowser>(2.pt()).FlipNotches<ButtonOpenInBrowser>()));
      ButtonRow component4 = new ButtonRow((Action) (() => Application.OpenURL("file://" + this.m_main.FileSystemHelper.GetDirPath(FileType.Map, true))), Outer.ShadowCutCorner);
      component4.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Default).Class<ButtonRow>(Cls.bold).Gap<ButtonRow>(new Px?(2.pt())).FlipNotches<ButtonRow>().MarginRight<ButtonRow>(Px.Auto)));
      component4.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/OpenInFolder.svg").Small());
      component4.Add((UiComponent) new Label((LocStrFormatted) Tr.ShowInExplorer));
      Button child2 = (Button) component4;
      this.m_openMapsFolderButton = (Button) component4;
      component2.Add((UiComponent) child2);
      ButtonRow component5 = new ButtonRow(new Action(this.onNext), Outer.ShadowCutCorner);
      component5.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Primary).Gap<ButtonRow>(new Px?(2.pt())).AlignItemsStart<ButtonRow>()));
      component5.Add((UiComponent) (this.m_nextButtonLabel = new Label((LocStrFormatted) Tr.GoNext).UpperCase()));
      component5.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Next.svg").Small());
      ButtonRow child3 = component5;
      this.m_nextButton = component5;
      component2.Add((UiComponent) child3);
      uiComponentArray[1] = (UiComponent) component2;
      body.Add(uiComponentArray);
      if (!GlobalPlayerPrefs.EnableMods || main.Mods.FilterThirdPartyMods().Length == 0)
        this.m_tabContainer.TryRemoveTab(0);
      this.m_tabContainer.OnTabActivate(new Action(this.onActivate));
      this.updateButtons();
    }

    public override bool InputUpdate()
    {
      if (!this.IsVisible())
        return false;
      if (this.m_tabContainer.ActiveTab.Value is MapTab mapTab && mapTab.InputUpdate())
        return true;
      if (!Input.GetKeyDown(KeyCode.Escape))
        return false;
      this.RemoveFromHierarchy();
      this.m_onClose((Mafi.Unity.UiToolkit.Library.Window) this);
      return true;
    }

    private void onActivate() => this.updateButtons();

    private void updateButtons()
    {
      int num = this.m_tabContainer.ActiveTabIndex.Value;
      Option<TabButton> option1 = this.m_tabContainer.TabBar.TabAt(num - 1);
      Option<TabButton> option2 = this.m_tabContainer.TabBar.TabAt(num + 1);
      this.m_prevButton.Visible<Button>(option1.HasValue);
      this.m_coiHubButton.VisibleForRender<Button>(this.m_tabContainer.ActiveTab.Value is MapTab);
      this.m_openMapsFolderButton.VisibleForRender<Button>(this.m_tabContainer.ActiveTab.Value is MapTab);
      this.m_nextButtonLabel.Text<Label>((LocStrFormatted) (option2.HasValue ? Tr.GoNext : TrCore.NewGameWizard__Launch));
      if (option2.IsNone && (this.m_settings.Map.IsNone || this.m_settings.IsSelectedMapCorrupted))
      {
        this.m_nextButton.Enabled<ButtonRow>(false);
        this.m_nextButton.Tooltip<ButtonRow>(new LocStrFormatted?((LocStrFormatted) TrCore.MapInvalid));
      }
      else
      {
        this.m_nextButton.Enabled<ButtonRow>(true);
        this.m_nextButton.Tooltip<ButtonRow>(new LocStrFormatted?(LocStrFormatted.Empty));
      }
    }

    private void onLaunch()
    {
      if (!this.m_settings.CanStartGame())
        this.RunWithBuilder((Action<UiBuilder>) (builder => builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp).Play()));
      else
        this.m_main.StartNewGame(this.m_settings.ToConfigs(), this.m_settings.EnabledModTypes.ToImmutableArray<ModData>());
    }

    private void onNext()
    {
      if (this.m_tabContainer.ActiveTab.Value is CustomizationTab)
      {
        this.onLaunch();
      }
      else
      {
        TabContainer tabContainer = this.m_tabContainer;
        int? activeTabIndex = tabContainer.ActiveTabIndex;
        tabContainer.ActiveTabIndex = activeTabIndex.HasValue ? new int?(activeTabIndex.GetValueOrDefault() + 1) : new int?();
      }
    }

    private void onPrevious()
    {
      TabContainer tabContainer = this.m_tabContainer;
      int? activeTabIndex = tabContainer.ActiveTabIndex;
      tabContainer.ActiveTabIndex = activeTabIndex.HasValue ? new int?(activeTabIndex.GetValueOrDefault() - 1) : new int?();
    }

    private void showDifficultySettings()
    {
      int index = this.m_tabContainer.IndexOfTab((Func<UiComponent, bool>) (t => t is AdvancedSettingsTab));
      if (index >= 0)
      {
        this.m_tabContainer.SwitchToTab(index);
      }
      else
      {
        int num = this.m_tabContainer.IndexOfTab((Func<UiComponent, bool>) (t => t is MechanicsTab));
        AdvancedSettingsTab content = new AdvancedSettingsTab(this.m_settings);
        this.m_tabContainer.AddTab((LocStrFormatted) Tr.Menu__DifficultySettings, (UiComponent) content, "Assets/Unity/UserInterface/General/Configure.svg", true, index: new int?(num + 1));
      }
    }

    static NewGameWindow()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      NewGameWindow.TARGET_WIDTH = 1400.px();
      NewGameWindow.TARET_HEIGHT = 1000.px();
    }
  }
}
