// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.SaveWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
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
  public class SaveWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly IMain m_main;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly ISaveManager m_saveManager;
    private readonly IGameSaveInfoProvider m_gameSaveInfoProvider;
    private readonly ScrollColumn m_gameList;
    private readonly ScrollColumn m_saveList;
    private readonly ScrollColumn m_saveDetail;
    private readonly Mafi.Unity.UiToolkit.Library.Button m_newSaveButton;
    private readonly ConfirmButton m_saveButton;
    private readonly TxtField m_nameField;
    private readonly ConfirmButton m_deleteSaveButton;
    private GameSaveInfo m_newSaveInfo;
    private Lyst<SaveFileGroup> m_games;
    private string m_saveName;
    private int m_selectedGameIndex;
    private int m_selectedSaveIndex;

    private string GameName => this.m_saveManager.GameName;

    private SaveFileGroup? SelectedGame
    {
      get
      {
        return this.m_selectedGameIndex >= this.m_games.Count ? new SaveFileGroup?() : new SaveFileGroup?(this.m_games[this.m_selectedGameIndex]);
      }
    }

    private SaveFileInfo? SelectedSave
    {
      get
      {
        int selectedSaveIndex = this.m_selectedSaveIndex;
        SaveFileGroup? selectedGame = this.SelectedGame;
        ref SaveFileGroup? local1 = ref selectedGame;
        int? nullable = local1.HasValue ? new int?(local1.GetValueOrDefault().Saves.Length) : new int?();
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(selectedSaveIndex < valueOrDefault & nullable.HasValue))
          return new SaveFileInfo?();
        selectedGame = this.SelectedGame;
        ref SaveFileGroup? local2 = ref selectedGame;
        return !local2.HasValue ? new SaveFileInfo?() : new SaveFileInfo?(local2.GetValueOrDefault().Saves[this.m_selectedSaveIndex]);
      }
    }

    public SaveWindow(
      IMain main,
      ISaveManager saveManager,
      IGameSaveInfoProvider gameSaveInfoProvider,
      bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_newSaveInfo = GameSaveInfo.Empty;
      this.m_saveName = "";
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) Tr.Save_Title, darkMask: darkMask);
      this.m_main = main;
      this.m_fileSystemHelper = this.m_main.FileSystemHelper;
      this.m_saveManager = saveManager;
      this.m_gameSaveInfoProvider = gameSaveInfoProvider;
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
      Mafi.Unity.UiToolkit.Library.Row row4 = new Mafi.Unity.UiToolkit.Library.Row(Outer.EdgeShadowTop);
      row4.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.PaddingTop<Mafi.Unity.UiToolkit.Library.Row>(2.pt()).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Row>().AlignItemsEnd<Mafi.Unity.UiToolkit.Library.Row>()));
      row4.Add((UiComponent) (this.m_deleteSaveButton = new ConfirmButton("Assets/Unity/UserInterface/General/Trash128.png", (LocStrFormatted) Tr.BlueprintDelete__Action, LocStrFormatted.Empty, new Action(this.handleDeleteSave))));
      row4.Add((UiComponent) (this.m_newSaveButton = (Mafi.Unity.UiToolkit.Library.Button) new ButtonText((LocStrFormatted) Tr.SaveNew, (Action) (() => this.handleSaveRowClicked(0))).Class<ButtonText>(Cls.bold).MarginLeft<ButtonText>(2.pt())));
      row4.Add((UiComponent) (this.m_nameField = new TxtField()).SetLabelAlignment(Mafi.Unity.UiToolkit.Component.Align.Center).Label<TxtField>((LocStrFormatted) Tr.SaveName__Label).Width<TxtField>(50.Percent()).SelectAllOnFocus().OnValueChanged(new Action<string>(this.handleNameEntered)).OnReturn(new Action(this.handleReturnPress)).OnEscape((Action) (() => this.SetVisible(false))).FocusOnShow());
      row4.Add((UiComponent) (this.m_saveButton = new ConfirmButton((LocStrFormatted) Tr.OverwriteSave__Action, LocStrFormatted.Empty, new Action(this.handleSaveButtonClick)).Variant<ConfirmButton>(ButtonVariant.Primary).MarginLeft<ConfirmButton>(Px.Auto)));
      panel2.Add((UiComponent) row4);
      row1.Add((UiComponent) panel2);
      body.Add((UiComponent) row1);
      this.OnKeyDown<SaveWindow>((Action<KeyDownEvent>) (evt =>
      {
        if (evt.keyCode != KeyCode.Return)
          return;
        this.handleReturnPress();
      }));
      this.OnShow(new Action(this.handleOpened));
    }

    private void handleOpened()
    {
      this.m_gameSaveInfoProvider.ScheduleScreenshotRendering((Action) (() =>
      {
        this.m_newSaveInfo = this.m_gameSaveInfoProvider.CreateGameSaveInfo();
        this.m_gameSaveInfoProvider.NotifySaveDone();
        this.refreshSaveDetail();
      }));
      this.refreshGames();
    }

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

    private void handleSaveButtonClick()
    {
      this.m_saveName = this.m_saveName.Trim();
      if (this.m_saveName.IsEmpty())
        this.m_saveName = this.getDefaultSaveName();
      this.m_saveManager.RequestGameSave(this.m_saveName, (Action<SaveResult>) (res => this.handleSaveComplete(this.m_saveName, res)));
      this.RunWithBuilder((Action<UiBuilder>) (builder => builder.ShowGeneralNotification((LocStrFormatted) Tr.ManualSaveInProgress, true)));
    }

    private void handleSaveComplete(string saveName, SaveResult result)
    {
      if (!result.Error.HasValue)
      {
        this.RunWithBuilder((Action<UiBuilder>) (b => b.ShowSuccessNotification(Tr.Save__SuccessMessage.Format(saveName))));
        this.Hide<SaveWindow>();
        this.m_nameField.Text(LocStrFormatted.Empty);
        this.handleNameEntered(string.Empty);
      }
      else
      {
        string error = result.Error.Value.Value;
        if (result.Exception.HasValue)
          error += string.Format("\n\n{0}", (object) result.Exception.Value);
        this.RunWithBuilder((Action<UiBuilder>) (b => b.ShowFailureNotification((LocStrFormatted) Tr.Save__FailureMessage, error.AsLoc())));
      }
    }

    private void handleGameClick(int index)
    {
      if (index == this.m_selectedGameIndex)
        return;
      this.selectGame(index);
      this.m_selectedSaveIndex = 0;
      this.refreshSaves();
    }

    private void handleSaveRowClicked(int index)
    {
      if (index == this.m_selectedSaveIndex)
        return;
      this.selectSave(index);
      this.refreshSaveDetail();
    }

    private void handleNameEntered(string saveName)
    {
      this.m_saveName = saveName;
      this.checkNameInUse();
    }

    private void handleReturnPress()
    {
      if (this.checkNameInUse())
        this.m_saveButton.ShowConfirm();
      else
        this.handleSaveButtonClick();
    }

    private bool checkNameInUse()
    {
      this.m_saveName = this.m_saveName.Trim();
      if (this.m_saveName.IsEmpty())
        this.m_saveName = this.getDefaultSaveName();
      bool required = this.m_fileSystemHelper.FileExists(this.m_saveName, FileType.GameSave, subDir: this.GameName);
      this.m_saveButton.ConfirmRequired(required).Text<ConfirmButton>((LocStrFormatted) (!required ? Tr.Save_Action : Tr.OverwriteSave__Action));
      if (required)
        this.m_saveButton.ConfirmLabel(Tr.OverwriteSave__ConfirmPrompt.Format("<b>" + this.m_saveName + "</b>"));
      return required;
    }

    private void refreshButtons()
    {
      if (!this.SelectedSave.HasValue)
      {
        Log.Error("Save expected.");
      }
      else
      {
        bool isVisible = this.m_selectedSaveIndex == 0;
        this.m_deleteSaveButton.Visible<ConfirmButton>(!isVisible);
        this.m_newSaveButton.Visible<Mafi.Unity.UiToolkit.Library.Button>(!isVisible);
        this.m_nameField.Visible<TxtField>(isVisible);
        if (!isVisible && this.SelectedSave.HasValue)
          this.m_deleteSaveButton.ConfirmLabel(Tr.DeleteSave__Confirm.Format("<b>" + this.SelectedSave.Value.NameNoExtension + "</b>"));
        this.checkNameInUse();
      }
    }

    private void refreshGames()
    {
      this.m_games = this.m_fileSystemHelper.GetAllSavesGroupedAndSorted().ToLyst();
      int? nullable = this.m_games.FirstIndexOf<SaveFileGroup>((Predicate<SaveFileGroup>) (g => g.GameName == this.GameName));
      if (!nullable.HasValue)
      {
        this.m_games.Insert(0, new SaveFileGroup(this.GameName, (ImmutableArray<SaveFileInfo>) ImmutableArray.Empty));
        nullable = new int?(0);
      }
      this.m_selectedGameIndex = nullable.Value;
      this.m_games[this.m_selectedGameIndex] = new SaveFileGroup(this.GameName, this.m_games[this.m_selectedGameIndex].Saves.Insert(0, new SaveFileInfo(Tr.SaveNew.TranslatedString, this.GameName)));
      int maxIndex = this.m_games.Count > 1 ? this.m_games.Count : 1;
      this.m_gameList.SetChildren(this.m_games.Select<SaveFileGroup, UiComponent>((Func<SaveFileGroup, int, UiComponent>) ((g, i) => this.buildGameRow(g, i, i == maxIndex - 1))));
      this.refreshSaves();
    }

    private void refreshSaves()
    {
      SaveFileGroup? selectedGame = this.SelectedGame;
      if (selectedGame.HasValue)
      {
        SaveFileGroup saveFileGroup = selectedGame.Value;
        int maxIndex = saveFileGroup.Saves.Length - 1;
        this.m_selectedSaveIndex = 0;
        ScrollColumn saveList = this.m_saveList;
        saveFileGroup = selectedGame.Value;
        IEnumerable<UiComponent> children = saveFileGroup.Saves.Select<UiComponent>((Func<SaveFileInfo, int, UiComponent>) ((sfi, i) => this.buildSaveRow(sfi, i, i == maxIndex)));
        saveList.SetChildren(children);
      }
      this.refreshSaveDetail();
    }

    private void refreshSaveDetail()
    {
      SaveFileInfo? selectedSave = this.SelectedSave;
      if (!selectedSave.HasValue)
      {
        Log.Error("Save expected.");
      }
      else
      {
        SaveFileInfo saveFileInfo = selectedSave.Value;
        Option<LoadFailInfo> error = Option<LoadFailInfo>.None;
        GameSaveInfo gameInfo = GameSaveInfo.Empty;
        int saveVersion = 0;
        bool flag = this.m_selectedSaveIndex == 0;
        if (flag)
          gameInfo = this.m_newSaveInfo ?? GameSaveInfo.Empty;
        ImmutableArray<ModInfoRaw> modsInfo;
        if (gameInfo != GameSaveInfo.Empty || this.m_fileSystemHelper.TryLoadSaveInfo(saveFileInfo.NameNoExtension, saveFileInfo.GameName, out gameInfo, out modsInfo, out saveVersion, out error))
        {
          LoadedModsStatus? nullable = new LoadedModsStatus?();
          if (modsInfo.IsValid)
            nullable = new LoadedModsStatus?(this.m_main.GetModsStatus(modsInfo));
          byte[] thumbnailImageBytes = gameInfo.ThumbnailImageBytes;
          bool isVisible = thumbnailImageBytes != null && thumbnailImageBytes.Length != 0;
          ScrollColumn saveDetail = this.m_saveDetail;
          UiComponent[] uiComponentArray = new UiComponent[3]
          {
            isVisible ? (UiComponent) new StretchedImg(gameInfo.CreateTextureFromSaveInfo(), Outer.ShadowAll).StretchWidth().Margin<StretchedImg>(Px.Zero, 1.pt(), 1.pt(), Px.Zero) : (UiComponent) null,
            (UiComponent) new BoltDecoration().Visible<BoltDecoration>(isVisible),
            null
          };
          Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column(2.pt());
          component.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f).AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().PaddingLeftRight<Mafi.Unity.UiToolkit.Library.Column>(1.pt()).PaddingBottom<Mafi.Unity.UiToolkit.Library.Column>(5.pt())));
          component.Add(flag ? (UiComponent) null : this.buildDetailRow((LocStrFormatted) Tr.Saved__Detail, saveFileInfo.WriteTimestamp.ToString()));
          component.Add(flag ? (UiComponent) null : this.buildDetailRow((LocStrFormatted) Tr.FileSize_Title, ((float) ((double) saveFileInfo.SizeBytes / 1024.0 / 1024.0)).RoundToSigDigits(2, false, false, false) + " MB"));
          component.Add(this.buildDetailRow((LocStrFormatted) Tr.GateTime__Detail, gameInfo.GameDate.FormatLong()));
          component.Add(gameInfo.MapName.IsNotEmpty() ? this.buildDetailRow((LocStrFormatted) Tr.Map, gameInfo.MapName) : (UiComponent) null);
          LocStrFormatted population1 = (LocStrFormatted) Tr.Population;
          int population2 = gameInfo.Population;
          string str1 = population2.ToString("n0");
          component.Add(this.buildDetailRow(population1, str1));
          component.Add(this.buildDetailRow((LocStrFormatted) Tr.Research__Detail, string.Format("{0:n0} / {1:n0}", (object) gameInfo.ResearchNodesUnlocked, (object) gameInfo.ResearchNodesTotal)));
          UiComponent child;
          if (gameInfo.LaunchCount <= 0)
          {
            child = (UiComponent) null;
          }
          else
          {
            LocStrFormatted launchesDetail = (LocStrFormatted) Tr.Launches__Detail;
            population2 = gameInfo.Population;
            string str2 = population2.ToString("n0");
            child = this.buildDetailRow(launchesDetail, str2);
          }
          component.Add(child);
          component.Add(nullable.HasValue ? this.buildModRow((LocStrFormatted) Tr.ModsInSave__Detail, (LocStrFormatted) Tr.ModsInSave__Tooltip, nullable.Value.ThirdParty.Filter((Predicate<SaveModStatus>) (x => x.IsSelected))) : (UiComponent) null);
          uiComponentArray[2] = (UiComponent) component;
          saveDetail.SetChildren(uiComponentArray);
        }
        else if (error.HasValue)
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
        this.refreshButtons();
      }
    }

    private UiComponent buildGameRow(SaveFileGroup group, int index, bool isLast)
    {
      LocStrFormatted locStr = group.GameName.IsEmpty() ? (LocStrFormatted) Tr.UnsortedSaves__Title : group.GameName.AsLoc();
      ButtonRow buttonRow = new ButtonRow((Action) (() => this.handleGameClick(index)));
      buttonRow.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Area).Padding<ButtonRow>(2.pt(), 3.pt()).BorderBottom<ButtonRow>(!isLast ? 1 : 0).RelativePosition<ButtonRow>()));
      Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column();
      component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(locStr).PaddingBottom<Mafi.Unity.UiToolkit.Library.Label>(2.px()).FontBold<Mafi.Unity.UiToolkit.Library.Label>());
      string nameNoExtension = group.Saves.FirstOrDefault().NameNoExtension;
      component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(nameNoExtension != null ? nameNoExtension.AsLoc() : LocStrFormatted.Empty).FontNormal<Mafi.Unity.UiToolkit.Library.Label>().FontSize<Mafi.Unity.UiToolkit.Library.Label>(12));
      buttonRow.Add((UiComponent) component.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f));
      return (UiComponent) this.selectRow(buttonRow, index == this.m_selectedGameIndex).Enabled<Mafi.Unity.UiToolkit.Library.Button>(this.m_selectedGameIndex == index);
    }

    private UiComponent buildSaveRow(SaveFileInfo save, int index, bool isLast)
    {
      ButtonRow buttonRow = new ButtonRow((Action) (() => this.handleSaveRowClicked(index)));
      buttonRow.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Area).Padding<ButtonRow>(2.pt(), 3.pt()).BorderBottom<ButtonRow>(!isLast ? 1 : 0).RelativePosition<ButtonRow>()));
      Mafi.Unity.UiToolkit.Library.Column component1 = new Mafi.Unity.UiToolkit.Library.Column();
      component1.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(save.NameNoExtension.AsLoc()).PaddingBottom<Mafi.Unity.UiToolkit.Library.Label>(2.px()).FontBold<Mafi.Unity.UiToolkit.Library.Label>());
      component1.Add((UiComponent) new DateTimeLabel(save.WriteTimestamp).FontSize<DateTimeLabel>(12).FontNormal<DateTimeLabel>());
      buttonRow.Add((UiComponent) component1.FlexGrow<Mafi.Unity.UiToolkit.Library.Column>(1f));
      Mafi.Unity.UiToolkit.Library.Button component2 = this.selectRow(buttonRow, this.m_selectedSaveIndex == index);
      if (index > 0)
        component2.OnDoubleClick<Mafi.Unity.UiToolkit.Library.Button>(new Action(this.handleSaveButtonClick));
      return (UiComponent) component2;
    }

    private void selectGame(int index)
    {
      if (this.m_selectedGameIndex != index)
      {
        this.selectRow(this.m_gameList[this.m_selectedGameIndex] as ButtonRow, false);
        this.m_selectedGameIndex = index;
      }
      this.selectRow(this.m_gameList.ChildAtOrNone(index).ValueOrNull as ButtonRow, true);
    }

    private void selectSave(int index)
    {
      if (this.m_selectedSaveIndex != index)
      {
        this.selectRow(this.m_saveList[this.m_selectedSaveIndex] as ButtonRow, false);
        this.m_selectedSaveIndex = index;
      }
      if (this.m_selectedSaveIndex == 0)
      {
        this.m_nameField.SetText(this.m_saveName.AsLoc());
      }
      else
      {
        int selectedSaveIndex = this.m_selectedSaveIndex;
        SaveFileGroup? selectedGame = this.SelectedGame;
        ref SaveFileGroup? local1 = ref selectedGame;
        int? nullable = local1.HasValue ? new int?(local1.GetValueOrDefault().Saves.Length) : new int?();
        int valueOrDefault = nullable.GetValueOrDefault();
        if (selectedSaveIndex < valueOrDefault & nullable.HasValue)
        {
          selectedGame = this.SelectedGame;
          ref SaveFileGroup? local2 = ref selectedGame;
          this.m_saveName = local2.HasValue ? local2.GetValueOrDefault().Saves[this.m_selectedSaveIndex].NameNoExtension : (string) null;
        }
      }
      this.selectRow(this.m_saveList.ChildAtOrNone(index).ValueOrNull as ButtonRow, true);
      this.refreshButtons();
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
      ImmutableArray<SaveModStatus> mods)
    {
      if (mods.Length == 0)
        return (UiComponent) null;
      Mafi.Unity.UiToolkit.Library.Label label1 = new Mafi.Unity.UiToolkit.Library.Label(label).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?(labelTooltip));
      Mafi.Unity.UiToolkit.Library.Column component = new Mafi.Unity.UiToolkit.Library.Column(2.px());
      component.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.PaddingTop<Mafi.Unity.UiToolkit.Library.Column>(2.px())));
      component.Add(mods.Select<UiComponent>((Func<SaveModStatus, UiComponent>) (m => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(string.Format("{0} v{1}", (object) m.Name, (object) m.Version).AsLoc()).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?((LocStrFormatted) Tr.ModMissing__Tooltip), !m.IsAvailable).ClassIff<Mafi.Unity.UiToolkit.Library.Label>(Cls.warningText, !m.IsAvailable))));
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

    private string getDefaultSaveName()
    {
      return string.Format("Save {0:yyyy-MM-dd, HH-mm-ss}", (object) DateTime.Now);
    }
  }
}
