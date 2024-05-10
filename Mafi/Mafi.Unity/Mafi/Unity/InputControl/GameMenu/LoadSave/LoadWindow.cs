// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.LoadWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.LoadSave
{
  public class LoadWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly IMain m_main;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly Option<string> m_activeGameName;
    private readonly ScrollColumn m_gameList;
    private readonly ScrollColumn m_saveList;
    private readonly ScrollColumn m_saveDetail;
    private readonly ConfirmButton m_deleteSaveButton;
    private readonly Mafi.Unity.UiToolkit.Library.Button m_loadButton;
    private readonly Mafi.Unity.UiToolkit.Library.Button m_editModsButton;
    private bool m_editMods;
    private ImmutableArray<SaveFileGroup> m_games;
    private int m_selectedGame;
    private int m_selectedSave;
    private LoadedModsStatus? m_modStatus;

    private SaveFileGroup? SelectedGame
    {
      get
      {
        return this.m_games.Length <= this.m_selectedGame ? new SaveFileGroup?() : new SaveFileGroup?(this.m_games[this.m_selectedGame]);
      }
    }

    private SaveFileInfo? SelectedSave
    {
      get
      {
        SaveFileGroup? selectedGame = this.SelectedGame;
        ref SaveFileGroup? local = ref selectedGame;
        SaveFileGroup valueOrDefault;
        int? nullable1;
        if (!local.HasValue)
        {
          nullable1 = new int?();
        }
        else
        {
          valueOrDefault = local.GetValueOrDefault();
          nullable1 = new int?(valueOrDefault.Saves.Length);
        }
        int? nullable2 = nullable1;
        int selectedSave = this.m_selectedSave;
        if (!(nullable2.GetValueOrDefault() > selectedSave & nullable2.HasValue))
          return new SaveFileInfo?();
        selectedGame = this.SelectedGame;
        valueOrDefault = selectedGame.Value;
        return new SaveFileInfo?(valueOrDefault.Saves[this.m_selectedSave]);
      }
    }

    public LoadWindow(IMain main, Option<string> activeGameName = default (Option<string>), bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) Tr.Load_Title, darkMask: darkMask);
      this.m_main = main;
      this.m_fileSystemHelper = main.FileSystemHelper;
      this.m_activeGameName = activeGameName;
      this.m_main.EnsureModsStatusInitialized();
      this.Grow();
      Mafi.Unity.UiToolkit.Library.Column body = this.Body;
      Mafi.Unity.UiToolkit.Library.Row row1 = new Mafi.Unity.UiToolkit.Library.Row(10.px());
      row1.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Fill<Mafi.Unity.UiToolkit.Library.Row>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>()));
      Mafi.Unity.UiToolkit.Library.Panel panel1 = new Mafi.Unity.UiToolkit.Library.Panel(new LocStrFormatted?((LocStrFormatted) Tr.Game__Title));
      panel1.Add<Mafi.Unity.UiToolkit.Library.Panel>((Action<Mafi.Unity.UiToolkit.Library.Panel>) (c => c.Width<Mafi.Unity.UiToolkit.Library.Panel>(30.Percent()).AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Panel>().RootPanel()));
      ScrollColumn component1 = new ScrollColumn();
      component1.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Fill<ScrollColumn>().Class<ScrollColumn>(Cls.listGroup)));
      ScrollColumn child1 = component1;
      this.m_gameList = component1;
      panel1.Add((UiComponent) child1);
      Mafi.Unity.UiToolkit.Library.Row row2 = new Mafi.Unity.UiToolkit.Library.Row(Outer.EdgeShadowTop);
      row2.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.PaddingTop<Mafi.Unity.UiToolkit.Library.Row>(2.pt()).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Row>()));
      row2.Add((UiComponent) new IconButton("Assets/Unity/UserInterface/General/OpenInFolder.svg", new Action(this.handleOpenInFolder)).Medium().MarginLeft<IconButton>(Px.Auto).Tooltip<IconButton>(new LocStrFormatted?((LocStrFormatted) Tr.ShowInExplorer)));
      panel1.Add((UiComponent) row2);
      row1.Add((UiComponent) panel1);
      Mafi.Unity.UiToolkit.Library.Panel panel2 = new Mafi.Unity.UiToolkit.Library.Panel(new LocStrFormatted?((LocStrFormatted) Tr.Save__Title));
      panel2.Add<Mafi.Unity.UiToolkit.Library.Panel>((Action<Mafi.Unity.UiToolkit.Library.Panel>) (c => c.Fill<Mafi.Unity.UiToolkit.Library.Panel>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Panel>().RootPanel()));
      Mafi.Unity.UiToolkit.Library.Row row3 = new Mafi.Unity.UiToolkit.Library.Row();
      row3.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Fill<Mafi.Unity.UiToolkit.Library.Row>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>()));
      ScrollColumn component2 = new ScrollColumn();
      component2.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Width<ScrollColumn>(40.Percent()).AlignItemsStretch<ScrollColumn>().ScrollerAlwaysVisible().Class<ScrollBase>(Cls.listGroup)));
      ScrollColumn child2 = component2;
      this.m_saveList = component2;
      row3.Add((UiComponent) child2);
      ScrollColumn component3 = new ScrollColumn();
      component3.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Width<ScrollColumn>(60.Percent()).PaddingLeft<ScrollColumn>(2.pt()).AlignItemsStretch<ScrollColumn>()));
      ScrollColumn child3 = component3;
      this.m_saveDetail = component3;
      row3.Add((UiComponent) child3);
      panel2.Add((UiComponent) row3);
      Mafi.Unity.UiToolkit.Library.Row row4 = new Mafi.Unity.UiToolkit.Library.Row(Outer.EdgeShadowTop, gap: new Px?(2.pt()));
      row4.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.PaddingTop<Mafi.Unity.UiToolkit.Library.Row>(2.pt()).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Row>()));
      row4.Add((UiComponent) (this.m_deleteSaveButton = new ConfirmButton("Assets/Unity/UserInterface/General/Trash128.png", (LocStrFormatted) Tr.BlueprintDelete__Action, LocStrFormatted.Empty, new Action(this.handleDeleteSave)).MarginRight<ConfirmButton>(Px.Auto)));
      row4.Add((UiComponent) (this.m_editModsButton = (Mafi.Unity.UiToolkit.Library.Button) new IconToggleButton("Assets/Unity/UserInterface/General/Configure.svg", new Action(this.handleEditMods)).Medium().Tooltip<IconButton>(new LocStrFormatted?((LocStrFormatted) Tr.ConfigureMods_Action))));
      row4.Add((UiComponent) (this.m_loadButton = (Mafi.Unity.UiToolkit.Library.Button) new ButtonPrimary((LocStrFormatted) Tr.Load_Action, new Action(this.handleLoadClick))));
      panel2.Add((UiComponent) row4);
      row1.Add((UiComponent) panel2);
      body.Add((UiComponent) row1);
      this.OnKeyDown<LoadWindow>((Action<KeyDownEvent>) (evt =>
      {
        if (evt.keyCode != KeyCode.Return)
          return;
        this.handleLoadClick();
      }));
      this.OnShow(new Action(this.handleOpened));
    }

    private void handleOpened() => this.refreshGames(true);

    private void handleOpenInFolder()
    {
      Application.OpenURL("file://" + this.m_fileSystemHelper.GetDirPath(FileType.GameSave, true));
    }

    private void handleDeleteSave()
    {
      SaveFileInfo? selectedSave = this.SelectedSave;
      if (!selectedSave.HasValue)
        return;
      SaveFileInfo info = selectedSave.Value;
      if (this.m_fileSystemHelper.DeleteSaveFile(info.NameNoExtension, info.GameName))
        this.RunWithBuilder((Action<UiBuilder>) (b => b.ShowSuccessNotification(Tr.DeleteSave__SuccessMessage.Format("<b>" + info.NameNoExtension + "</b>"))));
      else
        this.RunWithBuilder((Action<UiBuilder>) (b => b.ShowFailureNotification(Tr.DeleteSave__FailMessage.Format("<b>" + info.NameNoExtension + "</b>"))));
      this.refreshGames();
    }

    private void handleLoadClick()
    {
      SaveFileInfo? selectedSave = this.SelectedSave;
      if (!selectedSave.HasValue || !this.m_modStatus.HasValue || this.m_modStatus.Value.IsAnyUnavailableModSelected())
        return;
      ImmutableArray<ModData> availableThirdPartyMods = this.m_modStatus.Value.GetSelectedAvailableThirdPartyMods();
      this.m_main.LoadGame(selectedSave.Value, new ImmutableArray<ModData>?(availableThirdPartyMods));
      this.RemoveFromHierarchy();
    }

    private void handleGameClick(int index)
    {
      if (index == this.m_selectedGame)
        return;
      this.m_editMods = false;
      this.m_editModsButton.Selected<Mafi.Unity.UiToolkit.Library.Button>(this.m_editMods);
      this.selectGame(index);
      this.m_selectedSave = 0;
      this.refreshSaves();
    }

    private void handleSaveClicked(int index)
    {
      if (index == this.m_selectedSave)
        return;
      this.m_editMods = false;
      this.m_editModsButton.Selected<Mafi.Unity.UiToolkit.Library.Button>(this.m_editMods);
      this.selectSave(index);
      this.refreshSaveDetail();
    }

    private void handleEditMods()
    {
      this.m_editMods = !this.m_editMods;
      this.m_editModsButton.Selected<Mafi.Unity.UiToolkit.Library.Button>(this.m_editMods);
      this.refreshSaveDetail();
    }

    private void handleModSelected(SaveModStatus mod, bool selected)
    {
      mod.IsSelected = selected;
      this.refreshButtons();
    }

    private void refreshButtons()
    {
      if (GlobalPlayerPrefs.EnableMods)
      {
        bool enabled = this.m_modStatus.HasValue && !this.m_modStatus.Value.IsAnyUnavailableModSelected();
        this.m_loadButton.Enabled<Mafi.Unity.UiToolkit.Library.Button>(enabled).Tooltip<Mafi.Unity.UiToolkit.Library.Button>(new LocStrFormatted?((LocStrFormatted) Tr.LoadDisabled__ModsMissing), !enabled);
      }
      else
      {
        ref LoadedModsStatus? local = ref this.m_modStatus;
        bool enabled = local.HasValue && !local.GetValueOrDefault().HasAnythingSelected();
        this.m_loadButton.Enabled<Mafi.Unity.UiToolkit.Library.Button>(enabled).Tooltip<Mafi.Unity.UiToolkit.Library.Button>(new LocStrFormatted?(Tr.LoadDisabled__ModsNotEnabled.Format(Tr.Settings_Title)), !enabled);
      }
      Mafi.Unity.UiToolkit.Library.Button editModsButton = this.m_editModsButton;
      ref LoadedModsStatus? local1 = ref this.m_modStatus;
      int num = local1.HasValue ? (local1.GetValueOrDefault().HasAnythingToConfigure() ? 1 : 0) : 0;
      editModsButton.Visible<Mafi.Unity.UiToolkit.Library.Button>(num != 0);
      this.m_deleteSaveButton.VisibleForRender<ConfirmButton>(this.SelectedSave.HasValue);
      SaveFileInfo? selectedSave = this.SelectedSave;
      if (!selectedSave.HasValue)
        return;
      ConfirmButton deleteSaveButton = this.m_deleteSaveButton;
      ref readonly LocStr1 local2 = ref Tr.DeleteSave__Confirm;
      selectedSave = this.SelectedSave;
      string str = "<b>" + selectedSave.Value.NameNoExtension + "</b>";
      LocStrFormatted label = local2.Format(str);
      deleteSaveButton.ConfirmLabel(label);
    }

    private void refreshGames(bool selectActiveGame = false)
    {
      this.m_games = this.m_fileSystemHelper.GetAllSavesGroupedAndSorted();
      int maxIndex = this.m_games.Length > 1 ? this.m_games.Length : 1;
      if (selectActiveGame && this.m_activeGameName.HasValue)
        this.m_selectedGame = this.m_games.FirstIndexOf((Predicate<SaveFileGroup>) (sfg => sfg.GameName == this.m_activeGameName.Value)).GetValueOrDefault(-1);
      this.m_selectedGame = Mathf.Clamp(this.m_selectedGame, 0, maxIndex - 1);
      this.m_gameList.SetChildren(this.m_games.Select<UiComponent>((Func<SaveFileGroup, int, UiComponent>) ((g, i) => this.buildGameRow(g, i, i == maxIndex - 1))));
      this.refreshSaves();
      if (this.m_gameList.Count <= 0)
        return;
      UiComponent selected = this.m_gameList[this.m_selectedGame];
      selected.OnFirstRender<UiComponent>((Action) (() => this.m_gameList.ScrollTo(selected)));
    }

    private void refreshSaves()
    {
      SaveFileGroup? selectedGame = this.SelectedGame;
      int maxIndex = selectedGame.HasValue ? selectedGame.GetValueOrDefault().Saves.Length : 1;
      this.m_selectedSave = Mathf.Clamp(this.m_selectedSave, 0, maxIndex - 1);
      if (selectedGame.HasValue)
        this.m_saveList.SetChildren(selectedGame.Value.Saves.Select<UiComponent>((Func<SaveFileInfo, int, UiComponent>) ((sfi, i) => this.buildSaveRow(sfi, i, i == maxIndex - 1))));
      else
        this.m_saveList.Clear();
      this.refreshSaveDetail();
    }

    private void refreshSaveDetail()
    {
      SaveFileInfo? selectedSave = this.SelectedSave;
      Option<LoadFailInfo> error = Option<LoadFailInfo>.None;
      GameSaveInfo gameInfo = GameSaveInfo.Empty;
      int saveVersion = 0;
      ImmutableArray<ModInfoRaw> modsInfo;
      if (selectedSave.HasValue && this.m_fileSystemHelper.TryLoadSaveInfo(selectedSave.Value.NameNoExtension, selectedSave.Value.GameName, out gameInfo, out modsInfo, out saveVersion, out error))
      {
        LoadedModsStatus modsStatus = this.m_main.GetModsStatus(modsInfo);
        this.m_modStatus = new LoadedModsStatus?(modsStatus);
        byte[] thumbnailImageBytes = gameInfo.ThumbnailImageBytes;
        bool isVisible = thumbnailImageBytes != null && thumbnailImageBytes.Length != 0;
        bool flag = !gameInfo.IsEmpty;
        ImmutableArray<SaveModStatus> mods1 = modsStatus.ThirdParty.Filter((Predicate<SaveModStatus>) (x => x.IsSelected));
        ImmutableArray<SaveModStatus> mods2 = modsStatus.ThirdParty.Filter((Predicate<SaveModStatus>) (x => !x.IsSelected));
        ScrollColumn saveDetail = this.m_saveDetail;
        UiComponent[] uiComponentArray = new UiComponent[3]
        {
          isVisible ? (UiComponent) new StretchedImg(gameInfo.CreateTextureFromSaveInfo(), Outer.ShadowAll).StretchWidth().Margin<StretchedImg>(Px.Zero, 1.pt(), 1.pt(), Px.Zero) : (UiComponent) null,
          (UiComponent) new BoltDecoration().Visible<BoltDecoration>(isVisible),
          null
        };
        Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column(2.pt());
        component.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f).AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().PaddingLeftRight<Mafi.Unity.UiToolkit.Library.Column>(1.pt()).PaddingBottom<Mafi.Unity.UiToolkit.Library.Column>(5.pt())));
        component.Add(this.buildDetailRow((LocStrFormatted) Tr.Saved__Detail, selectedSave.Value.WriteTimestamp.ToString()));
        component.Add(this.buildDetailRow((LocStrFormatted) Tr.FileSize_Title, ((float) ((double) selectedSave.Value.SizeBytes / 1024.0 / 1024.0)).RoundToSigDigits(2, false, false, false) + " MB"));
        component.Add(this.buildDetailRow((LocStrFormatted) Tr.GateTime__Detail, gameInfo.GameDate.FormatLong()));
        component.Add(gameInfo.MapName.IsNotEmpty() ? this.buildDetailRow((LocStrFormatted) Tr.Map, gameInfo.MapName) : (UiComponent) null);
        int population1;
        UiComponent child1;
        if (!flag)
        {
          child1 = (UiComponent) null;
        }
        else
        {
          LocStrFormatted population2 = (LocStrFormatted) Tr.Population;
          population1 = gameInfo.Population;
          string str = population1.ToString("n0");
          child1 = this.buildDetailRow(population2, str);
        }
        component.Add(child1);
        component.Add(flag ? this.buildDetailRow((LocStrFormatted) Tr.Research__Detail, string.Format("{0:n0} / {1:n0}", (object) gameInfo.ResearchNodesUnlocked, (object) gameInfo.ResearchNodesTotal)) : (UiComponent) null);
        UiComponent child2;
        if (gameInfo.LaunchCount <= 0)
        {
          child2 = (UiComponent) null;
        }
        else
        {
          LocStrFormatted launchesDetail = (LocStrFormatted) Tr.Launches__Detail;
          population1 = gameInfo.Population;
          string str = population1.ToString("n0");
          child2 = this.buildDetailRow(launchesDetail, str);
        }
        component.Add(child2);
        component.Add(this.m_editMods ? (UiComponent) new WarningBanner(!GlobalPlayerPrefs.EnableMods ? Tr.LoadDisabled__ModsNotEnabled.Format(Tr.Settings_Title) : (LocStrFormatted) Tr.ConfigureMods_Warning).MarginTop<WarningBanner>(2.pt()) : (UiComponent) null);
        component.Add(this.buildModRow((LocStrFormatted) Tr.ModsInSave__Detail, (LocStrFormatted) Tr.ModsInSave__Tooltip, mods1, false));
        component.Add(this.m_editMods ? this.buildModRow((LocStrFormatted) Tr.ModsAvailable__Title, (LocStrFormatted) Tr.AddableMods__Tooltip, mods2, true) : (UiComponent) null);
        uiComponentArray[2] = (UiComponent) component;
        saveDetail.SetChildren(uiComponentArray);
      }
      else
      {
        this.m_modStatus = new LoadedModsStatus?();
        if (error.HasValue)
        {
          LocStrFormatted message;
          switch (error.Value.FailReason)
          {
            case LoadFailInfo.Reason.Version:
              message = TrCore.GameSaveLoad__VersionTooLow.Format(saveVersion.ToString());
              break;
            case LoadFailInfo.Reason.FileCorrupted:
              message = (LocStrFormatted) Tr.LoadDisabled__Corrupted;
              break;
            default:
              message = error.Value.MessageForPlayer ?? (LocStrFormatted) Tr.LoadDisabled__Error;
              break;
          }
          this.m_saveDetail.SetChildren(new UiComponent[1]
          {
            (UiComponent) new WarningBanner(message)
          });
        }
        else
          this.m_saveDetail.Clear();
      }
      this.refreshButtons();
    }

    private UiComponent buildGameRow(SaveFileGroup group, int index, bool isLast)
    {
      LocStrFormatted locStr = group.GameName.IsEmpty() ? (LocStrFormatted) Tr.UnsortedSaves__Title : group.GameName.AsLoc();
      ButtonRow buttonRow = new ButtonRow((Action) (() => this.handleGameClick(index)));
      buttonRow.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Area).Padding<ButtonRow>(2.pt(), 3.pt()).BorderBottom<ButtonRow>(!isLast ? 1 : 0).RelativePosition<ButtonRow>()));
      Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column();
      component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(locStr).PaddingBottom<Mafi.Unity.UiToolkit.Library.Label>(2.px()).FontBold<Mafi.Unity.UiToolkit.Library.Label>());
      component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(group.Saves[0].NameNoExtension.AsLoc()).FontNormal<Mafi.Unity.UiToolkit.Library.Label>().FontSize<Mafi.Unity.UiToolkit.Library.Label>(12));
      buttonRow.Add((UiComponent) component.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f));
      return (UiComponent) this.selectRow(buttonRow, index == this.m_selectedGame).OnDoubleClick<Mafi.Unity.UiToolkit.Library.Button>(new Action(this.handleLoadClick));
    }

    private UiComponent buildSaveRow(SaveFileInfo save, int index, bool isLast)
    {
      ButtonRow buttonRow = new ButtonRow((Action) (() => this.handleSaveClicked(index)));
      buttonRow.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Area).Padding<ButtonRow>(2.pt(), 3.pt()).BorderBottom<ButtonRow>(!isLast ? 1 : 0).RelativePosition<ButtonRow>()));
      Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column();
      component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(save.NameNoExtension.AsLoc()).PaddingBottom<Mafi.Unity.UiToolkit.Library.Label>(2.px()).FontBold<Mafi.Unity.UiToolkit.Library.Label>());
      component.Add((UiComponent) new DateTimeLabel(save.WriteTimestamp).FontSize<DateTimeLabel>(12).FontNormal<DateTimeLabel>());
      buttonRow.Add((UiComponent) component.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f));
      return (UiComponent) this.selectRow(buttonRow, this.m_selectedSave == index).OnDoubleClick<Mafi.Unity.UiToolkit.Library.Button>(new Action(this.handleLoadClick));
    }

    private void selectGame(int index)
    {
      if (this.m_selectedGame != index)
      {
        this.selectRow(this.m_gameList[this.m_selectedGame] as ButtonRow, false);
        this.m_selectedGame = index;
      }
      this.selectRow(this.m_gameList.ChildAtOrNone(index).ValueOrNull as ButtonRow, true);
    }

    private void selectSave(int index)
    {
      if (this.m_selectedSave != index)
      {
        this.selectRow(this.m_saveList[this.m_selectedSave] as ButtonRow, false);
        this.m_selectedSave = index;
      }
      this.selectRow(this.m_saveList.ChildAtOrNone(index).ValueOrNull as ButtonRow, true);
    }

    private Mafi.Unity.UiToolkit.Library.Button selectRow(ButtonRow row, bool selected)
    {
      if (selected && row.Count < 2)
      {
        ButtonRow buttonRow = row;
        Icon component = new Icon("Assets/Unity/UserInterface/General/ArrowRight.svg").Small();
        Px? nullable = new Px?(6.px());
        Px? top = new Px?();
        Px? right = nullable;
        Px? bottom = new Px?();
        Px? left = new Px?();
        Icon child = component.AbsolutePosition<Icon>(top, right, bottom, left).Color<Icon>(new ColorRgba?(Theme.PrimaryColor));
        buttonRow.Add((UiComponent) child);
      }
      else if (!selected && row.Count > 1)
        row.ChildAtOrNone(1).Value.RemoveFromHierarchy();
      return row == null ? (Mafi.Unity.UiToolkit.Library.Button) null : (Mafi.Unity.UiToolkit.Library.Button) row.Selected<ButtonRow>(selected);
    }

    private UiComponent buildDetailRow(LocStrFormatted label, string value)
    {
      return this.buildDetailRow(label, (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(value.AsLoc()));
    }

    private UiComponent buildDlcRow(LocStr label, ImmutableArray<SaveModStatus> mods)
    {
      if (mods.Length == 0)
        return (UiComponent) null;
      LocStrFormatted label1 = (LocStrFormatted) label;
      Mafi.Unity.UiToolkit.Library.Column column = new Mafi.Unity.UiToolkit.Library.Column(1.pt());
      column.Add((IEnumerable<UiComponent>) mods.Select<Mafi.Unity.UiToolkit.Library.Label>((Func<SaveModStatus, Mafi.Unity.UiToolkit.Library.Label>) (m => new Mafi.Unity.UiToolkit.Library.Label(m.Name.AsLoc()))));
      return this.buildDetailRow(label1, (UiComponent) column);
    }

    private UiComponent buildModRow(
      LocStrFormatted label,
      LocStrFormatted labelTooltip,
      ImmutableArray<SaveModStatus> mods,
      bool disableIfNotValid)
    {
      if (mods.Length == 0)
        return (UiComponent) null;
      Mafi.Unity.UiToolkit.Library.Label label1 = new Mafi.Unity.UiToolkit.Library.Label(label).PaddingTop<Mafi.Unity.UiToolkit.Library.Label>(this.m_editMods ? 5.px() : Px.Zero).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?(labelTooltip));
      Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column(2.px());
      component.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.PaddingTop<Mafi.Unity.UiToolkit.Library.Column>(this.m_editMods ? Px.Zero : 2.px())));
      component.Add(mods.Select<UiComponent>((Func<SaveModStatus, UiComponent>) (m => !this.m_editMods ? (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(string.Format("{0} v{1}", (object) m.Name, (object) m.Version).AsLoc()).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?((LocStrFormatted) Tr.ModMissing__Tooltip), !m.IsAvailable).ClassIff<Mafi.Unity.UiToolkit.Library.Label>(Cls.warningText, !m.IsAvailable) : (UiComponent) new Mafi.Unity.UiToolkit.Library.Toggle(true).Value(m.IsSelected).OnValueChanged((Action<bool>) (on => this.handleModSelected(m, on))).Label<Mafi.Unity.UiToolkit.Library.Toggle>(string.Format("{0} v{1}", (object) m.Name, (object) m.Version).AsLoc()).Tooltip<Mafi.Unity.UiToolkit.Library.Toggle>(new LocStrFormatted?((LocStrFormatted) Tr.ModMissing__Tooltip), !m.IsAvailable).Enabled<Mafi.Unity.UiToolkit.Library.Toggle>(m.IsAvailable || !disableIfNotValid).SetValid(m.IsAvailable))));
      return this.buildDetailRow((UiComponent) label1, (UiComponent) component);
    }

    private UiComponent buildDetailRow(LocStrFormatted label, UiComponent value)
    {
      return this.buildDetailRow((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(label), value);
    }

    private UiComponent buildDetailRow(UiComponent label, UiComponent value)
    {
      Mafi.Unity.UiToolkit.Library.Row component = new Mafi.Unity.UiToolkit.Library.Row();
      component.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.AlignItemsStart<Mafi.Unity.UiToolkit.Library.Row>()));
      component.Add(label.Width<UiComponent>(35.Percent()).FontBold<UiComponent>());
      component.Add(value.Width<UiComponent>(65.Percent()));
      return (UiComponent) component;
    }
  }
}
