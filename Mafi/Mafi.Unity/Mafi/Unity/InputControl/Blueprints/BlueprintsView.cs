// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Blueprints;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class BlueprintsView : WindowView
  {
    internal readonly BlueprintsLibrary BlueprintsLibrary;
    private readonly IUnityInputMgr m_inputMgr;
    private readonly BlueprintCreationController m_blueprintCreationController;
    private readonly CaptainOfficeManager m_captainOfficeManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly LazyResolve<BlueprintsController> m_controller;
    private DeleteBlueprintConfirmDialog m_confirmDeleteDialog;
    private BlueprintCopyPasteDialog m_blueprintCopyPasteDialog;
    private BlueprintDescriptionDialog m_blueprintDescDialog;
    private ViewsCacheHomogeneous<BlueprintView> m_viewsCache;
    private GridContainer m_gridContainer;
    private Txt m_locationView;
    private StackContainer m_rightTopBar;
    private Btn m_placeItBtn;
    private Btn m_toStrBtn;
    private Btn m_deleteBtn;
    private Btn m_updateDescBtn;
    private Txt m_saveStatusTxt;
    private Option<BlueprintView> m_selectedItem;
    private Option<BlueprintView> m_viewBeingDragged;
    private Option<BlueprintView> m_lastHoveredView;
    private int m_indexToMoveTo;
    private Panel m_insertPanel;
    private Option<IBlueprintItem> m_newItem;
    private ScrollableContainer m_scrollableContainer;
    private Panel m_officeNotAvailable;
    private Btn m_infoBtn;
    private Tooltip m_infoBtnTooltip;
    private Txt m_officeNotAvailableTxt;
    private readonly string m_captainOfficeName;

    private IBlueprintsFolder CurrentFolder { get; set; }

    internal BlueprintsView(
      IUnityInputMgr inputMgr,
      LazyResolve<BlueprintsController> controller,
      BlueprintsLibrary blueprintsLibrary,
      UnlockedProtosDbForUi unlockedProtosDb,
      ProtosDb protosDb,
      BlueprintCreationController blueprintCreationController,
      CaptainOfficeManager captainOfficeManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Blueprints");
      this.m_inputMgr = inputMgr;
      this.m_controller = controller;
      this.BlueprintsLibrary = blueprintsLibrary;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_blueprintCreationController = blueprintCreationController;
      this.m_captainOfficeManager = captainOfficeManager;
      this.CurrentFolder = this.BlueprintsLibrary.Root;
      this.m_captainOfficeName = protosDb.All<CaptainOfficeProto>().FirstOrDefault<CaptainOfficeProto>((Func<CaptainOfficeProto, bool>) (x => x.Upgrade.PreviousTier.IsNone))?.Strings.Name.TranslatedString ?? "";
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      int spacing = 2;
      int columnsCount = 5;
      Vector2 size = new Vector2((float) (BlueprintView.WIDTH * columnsCount + (columnsCount - 1) * spacing + 10 + 16), 670f);
      this.SetTitle((LocStrFormatted) Tr.Blueprints);
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      this.MakeMovable();
      this.m_confirmDeleteDialog = new DeleteBlueprintConfirmDialog(this.Builder, new Action(this.onDeleteConfirm));
      this.m_blueprintCopyPasteDialog = new BlueprintCopyPasteDialog(this.Builder, new Func<string, bool>(this.onStringImportClick));
      this.m_blueprintDescDialog = new BlueprintDescriptionDialog(this.Builder, new Action<string>(this.onDescriptionApply));
      this.m_viewsCache = new ViewsCacheHomogeneous<BlueprintView>((Func<BlueprintView>) (() => new BlueprintView((IUiElement) this.m_gridContainer, this, this.m_unlockedProtosDb, this.Builder, new Action<BlueprintView>(this.onItemSelected), new Action<BlueprintView>(this.onBlueprintDoubleClick))));
      int num1 = 30;
      int num2 = 52;
      StackContainer leftTopOf = this.Builder.NewStackContainer("TopLeftBar").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).PutToLeftTopOf<StackContainer>((IUiElement) this.GetContentPanel(), new Vector2(0.0f, 30f), Offset.All(10f));
      Btn fromSelectionBtn = this.Builder.NewBtnPrimary("FromSelection", (IUiElement) leftTopOf).SetIcon("Assets/Unity/UserInterface/General/SelectArea.svg", new Offset?(Offset.All(4f))).AddToolTip(Tr.Blueprint_NewFromSelectionTooltip).OnClick((Action) (() => this.m_blueprintCreationController.ActivateForSelection(new Action<ImmutableArray<EntityConfigData>, Lyst<TileSurfaceCopyPasteData>>(this.onNewBlueprintRequested))));
      fromSelectionBtn.AppendTo<Btn>(leftTopOf, new float?((float) num2));
      Btn newFolderBtn = this.Builder.NewBtnGeneral("NewFolder", (IUiElement) leftTopOf).SetIcon("Assets/Unity/UserInterface/General/NewFolder.svg", new Offset?(Offset.All(4f))).AddToolTip(Tr.NewFolder__Tooltip).OnClick(new Action(this.createNewFolder));
      newFolderBtn.AppendTo<Btn>(leftTopOf, new float?((float) num2));
      Btn fromStrBtn = this.Builder.NewBtnGeneral("FromString", (IUiElement) leftTopOf).SetIcon("Assets/Unity/UserInterface/General/ImportFromString.svg", new Offset?(Offset.All(4f))).AddToolTip(Tr.Blueprint_NewFromStringTooltip).OnClick((Action) (() => this.m_blueprintCopyPasteDialog.ShowForStringImport()));
      fromStrBtn.AppendTo<Btn>(leftTopOf, new float?((float) num2));
      this.m_rightTopBar = this.Builder.NewStackContainer("TopRightBar").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).PutToRightTopOf<StackContainer>((IUiElement) this.GetContentPanel(), new Vector2(0.0f, 30f), Offset.All(10f));
      this.m_placeItBtn = this.Builder.NewBtnPrimary("Place").SetIcon("Assets/Unity/UserInterface/General/Build.svg").OnClick((Action) (() =>
      {
        if (!this.m_selectedItem.HasValue || this.m_selectedItem.Value.IsLocked || !this.m_selectedItem.Value.Blueprint.HasValue)
          return;
        this.m_controller.Value.StartBlueprintPlacement(this.m_selectedItem.Value.Blueprint.Value);
      })).PlayErrorSoundWhenDisabled().AddToolTip(Tr.Blueprint_PlaceItTooltip);
      this.m_placeItBtn.AppendTo<Btn>(this.m_rightTopBar, new float?((float) num2));
      this.m_toStrBtn = this.Builder.NewBtnGeneral("ToString").SetIcon("Assets/Unity/UserInterface/General/ExportToString.svg").AddToolTip(Tr.Blueprint_ExportToStringTooltip).OnClick(new Action(this.exportSelectedItemToString));
      this.m_toStrBtn.AppendTo<Btn>(this.m_rightTopBar, new float?((float) num2));
      this.m_updateDescBtn = this.Builder.NewBtnGeneral("UpdateDesc").SetIcon("Assets/Unity/UserInterface/General/EditDescription.svg").AddToolTip(Tr.UpdateDescription__Tooltip).OnClick(new Action(this.requestDescriptionEdit));
      this.m_updateDescBtn.AppendTo<Btn>(this.m_rightTopBar, new float?((float) num2));
      this.m_deleteBtn = this.Builder.NewBtnDanger("Delete").SetIcon("Assets/Unity/UserInterface/General/Trash128.png").AddToolTip(Tr.BlueprintDelete__Tooltip).OnClick(new Action(this.startBlueprintDelete));
      this.m_deleteBtn.AppendTo<Btn>(this.m_rightTopBar, new float?((float) num2));
      this.m_infoBtn = this.Builder.NewBtnGeneral("Info").SetIcon("Assets/Unity/UserInterface/General/Info128.png").SetEnabled(false);
      this.m_infoBtnTooltip = this.m_infoBtn.AddToolTipAndReturn();
      this.m_infoBtn.AppendTo<Btn>(this.m_rightTopBar, new float?((float) num2));
      this.m_locationView = this.Builder.NewTxt("Navigation").SetTextStyle(this.Builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.GetContentPanel(), 20f, Offset.Top(50f) + Offset.Left(30f));
      this.Builder.NewPanel("Home").SetBackground("Assets/Unity/UserInterface/General/Home.svg").OnClick((Action) (() => this.CurrentFolder = this.BlueprintsLibrary.Root)).PutToLeftOf<Panel>((IUiElement) this.m_locationView, 18f, Offset.Left(-22f));
      this.m_saveStatusTxt = this.Builder.NewTxt("Status", (IUiElement) this.GetContentPanel()).SetTextStyle(this.Builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutToRightTopOf<Txt>((IUiElement) this.GetContentPanel(), new Vector2(0.0f, 20f), Offset.Top(50f) + Offset.Right(41f));
      Tooltip statusTooltip = this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Info", (IUiElement) this.m_saveStatusTxt).SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToRightMiddleOf<Panel>((IUiElement) this.m_saveStatusTxt, 15.Vector2(), Offset.Right(-18f)));
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer").AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel(), Offset.Top((float) (num1 + 40)));
      this.m_gridContainer = this.Builder.NewGridContainer("Container").SetCellSize(BlueprintView.SIZE).SetCellSpacing((float) spacing).SetDynamicHeightMode(columnsCount).SetInnerPadding(Offset.All(5f));
      this.m_scrollableContainer.AddItemTopLeft((IUiElement) this.m_gridContainer);
      this.m_officeNotAvailable = this.Builder.NewPanel("NotAvailableOverlay", (IUiElement) this.GetContentPanel()).SetBackground(new ColorRgba(2894116, 240)).PutTo<Panel>((IUiElement) this.GetContentPanel()).Hide<Panel>();
      Txt txt = this.Builder.NewTxt("Text", (IUiElement) this.m_officeNotAvailable);
      TextStyle textMediumBold = this.Builder.Style.Global.TextMediumBold;
      ref TextStyle local = ref textMediumBold;
      int? nullable = new int?(18);
      ColorRgba? color1 = new ColorRgba?(this.Builder.Style.Global.OrangeText);
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color1, fontStyle, fontSize, isCapitalized);
      this.m_officeNotAvailableTxt = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).SetText(Tr.Blueprints_BuildingRequired.Format(this.m_captainOfficeName)).AddOutline().PutTo<Txt>((IUiElement) this.m_officeNotAvailable, Offset.LeftRight(10f));
      ColorRgba colorRgba = (ColorRgba) 12158750;
      this.m_insertPanel = this.Builder.NewPanel("LeftEdge", (IUiElement) this.m_gridContainer).SetBackground(colorRgba).SetWidth<Panel>(2f).Hide<Panel>();
      this.Builder.NewIconContainer("LeftArrow", (IUiElement) this.m_insertPanel).SetIcon("Assets/Unity/UserInterface/General/Next.svg", colorRgba).PutToLeftOf<IconContainer>((IUiElement) this.m_insertPanel, 22f, Offset.Left(-32f)).AddOutline();
      this.Builder.NewIconContainer("RightArrow", (IUiElement) this.m_insertPanel).SetIcon("Assets/Unity/UserInterface/General/Return.svg", colorRgba).PutToRightOf<IconContainer>((IUiElement) this.m_insertPanel, 22f, Offset.Right(-32f)).AddOutline();
      this.GetContentPanel().OnClick((Action) (() =>
      {
        this.m_selectedItem.ValueOrNull?.CommitRenamingIfCan();
        this.clearSelectedItem();
      }));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<IBlueprintsFolder>((Func<IIndexable<IBlueprintsFolder>>) (() => this.CurrentFolder.Folders), (ICollectionComparator<IBlueprintsFolder, IIndexable<IBlueprintsFolder>>) CompareFixedOrder<IBlueprintsFolder>.Instance).Observe<IBlueprint>((Func<IIndexable<IBlueprint>>) (() => this.CurrentFolder.Blueprints), (ICollectionComparator<IBlueprint, IIndexable<IBlueprint>>) CompareFixedOrder<IBlueprint>.Instance).Do(new Action<Lyst<IBlueprintsFolder>, Lyst<IBlueprint>>(this.updateItems));
      updaterBuilder.Observe<BlueprintsLibrary.Status>((Func<BlueprintsLibrary.Status>) (() => this.BlueprintsLibrary.LibraryStatus)).Observe<int>((Func<int>) (() => this.BlueprintsLibrary.NumberOfBackupsAvailable)).Do((Action<BlueprintsLibrary.Status, int>) ((status, backupsTotal) =>
      {
        ColorRgba color2 = this.Builder.Style.Global.TextMediumBold.Color;
        ColorRgba dangerClr = this.Builder.Style.Global.DangerClr;
        bool enabled = status != BlueprintsLibrary.Status.LoadingInProgress;
        fromSelectionBtn.SetEnabled(enabled);
        newFolderBtn.SetEnabled(enabled);
        fromStrBtn.SetEnabled(enabled);
        string str1;
        switch (status)
        {
          case BlueprintsLibrary.Status.NoLibraryFound:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__Synchronized);
            str1 = "";
            setColor(color2);
            break;
          case BlueprintsLibrary.Status.LoadingInProgress:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.LoadInProgress);
            str1 = "";
            setColor(color2);
            break;
          case BlueprintsLibrary.Status.LoadFailedDueToFormat:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__FailedToLoad);
            str1 = Tr.BlueprintLibStatus__FailedToLoadOnFormat.TranslatedString;
            setColor(dangerClr);
            break;
          case BlueprintsLibrary.Status.LoadFailedNoAccess:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__FailedToLoad);
            str1 = Tr.BlueprintLibStatus__FailedToLoadOnPermission.TranslatedString;
            setColor(dangerClr);
            break;
          case BlueprintsLibrary.Status.LoadSuccess:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__Synchronized);
            str1 = "";
            setColor(color2);
            break;
          case BlueprintsLibrary.Status.SaveInProgress:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.SaveInProgress);
            str1 = "";
            setColor(color2);
            break;
          case BlueprintsLibrary.Status.SaveFailed:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__FailedToSave);
            str1 = Tr.BlueprintLibStatus__FailedToSaveTooltip.TranslatedString;
            setColor(dangerClr);
            break;
          case BlueprintsLibrary.Status.SaveDone:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__Synchronized);
            str1 = "";
            setColor(color2);
            break;
          case BlueprintsLibrary.Status.SaveDoneBackupFailed:
            this.m_saveStatusTxt.SetText((LocStrFormatted) Tr.BlueprintLibStatus__FailedToBackup);
            str1 = Tr.BlueprintLibStatus__FailedToBackupTooltip.TranslatedString;
            setColor(dangerClr);
            break;
          default:
            str1 = "";
            Log.Error(string.Format("Unknown enum state {0}", (object) status));
            break;
        }
        string str2 = Tr.FileLocation.Format(this.BlueprintsLibrary.PathToFile).Value;
        if (backupsTotal > 0)
          str2 += string.Format(" ({0})", (object) Tr.Blueprint__NumberOfBackups.Format(backupsTotal));
        statusTooltip.SetText(str1.IsEmpty() ? str2 : str1 + " " + str2);
        this.m_saveStatusTxt.SetWidth<Txt>(this.m_saveStatusTxt.GetPreferedWidth());
      }));
      updaterBuilder.Observe<Option<CaptainOffice>>((Func<Option<CaptainOffice>>) (() => this.m_captainOfficeManager.CaptainOffice)).Observe<bool>((Func<bool>) (() => this.m_captainOfficeManager.IsOfficeActive)).Do((Action<Option<CaptainOffice>, bool>) ((office, isActive) =>
      {
        this.m_officeNotAvailable.SetVisibility<Panel>(!isActive);
        if (isActive)
          return;
        string str = office.HasValue ? office.Value.Prototype.Strings.Name.TranslatedString : this.m_captainOfficeName;
        this.m_officeNotAvailableTxt.SetText(Tr.Blueprints_BuildingRequired.Format(str));
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(this.m_viewsCache.Updater);

      void setColor(ColorRgba color) => this.m_saveStatusTxt.SetColor(color);
    }

    private void requestDescriptionEdit()
    {
      if (this.m_selectedItem.IsNone)
        return;
      this.m_blueprintDescDialog.ShowForEdit(this.m_selectedItem.Value.Item.ValueOrNull?.Desc ?? "");
    }

    private void onNewBlueprintRequested(
      ImmutableArray<EntityConfigData> data,
      Lyst<TileSurfaceCopyPasteData> surfaceData)
    {
      this.m_newItem = this.BlueprintsLibrary.AddBlueprint(this.CurrentFolder, data, surfaceData).As<IBlueprintItem>();
      this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_controller.Value);
    }

    private void createNewFolder()
    {
      this.m_newItem = this.BlueprintsLibrary.AddNewFolder(this.CurrentFolder).CreateOption<IBlueprintsFolder>().As<IBlueprintItem>();
    }

    private void updateItems(
      IIndexable<IBlueprintsFolder> folders,
      IIndexable<IBlueprint> blueprints)
    {
      this.clearSelectedItem();
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.ClearAll();
      this.m_viewsCache.ReturnAll();
      if (this.CurrentFolder.ParentFolder.HasValue)
      {
        BlueprintView view = this.m_viewsCache.GetView();
        view.SetBlueprintsFolder(this.CurrentFolder.ParentFolder.Value, true);
        this.m_gridContainer.Append((IUiElement) view);
      }
      foreach (IBlueprintsFolder folder in folders)
      {
        BlueprintView view = this.m_viewsCache.GetView();
        view.SetBlueprintsFolder(folder);
        this.m_gridContainer.Append((IUiElement) view);
      }
      foreach (IBlueprint blueprint in blueprints)
      {
        BlueprintView view = this.m_viewsCache.GetView();
        view.SetBlueprint(blueprint);
        this.m_gridContainer.Append((IUiElement) view);
      }
      this.m_gridContainer.FinishBatchOperation();
      this.m_locationView.SetVisibility<Txt>(this.CurrentFolder.ParentFolder.HasValue);
      Option<IBlueprintsFolder> parentFolder = this.CurrentFolder.ParentFolder;
      if (!parentFolder.HasValue)
        return;
      string text = " > " + this.CurrentFolder.Name;
      IBlueprintsFolder blueprintsFolder = this.CurrentFolder.ParentFolder.Value;
      while (true)
      {
        parentFolder = blueprintsFolder.ParentFolder;
        if (parentFolder.HasValue)
        {
          text = " > " + blueprintsFolder.Name + text;
          blueprintsFolder = blueprintsFolder.ParentFolder.Value;
        }
        else
          break;
      }
      this.m_locationView.SetText(text);
    }

    private void onItemSelected(BlueprintView blueprintView)
    {
      this.clearSelectedItem();
      this.m_selectedItem = (Option<BlueprintView>) blueprintView;
      blueprintView.SetIsSelected(true);
      this.updateRightBars();
      this.m_placeItBtn.SetEnabled(!this.m_selectedItem.Value.IsLocked);
    }

    private void clearSelectedItem()
    {
      this.m_selectedItem.ValueOrNull?.CancelRenamingIfCan();
      this.m_selectedItem.ValueOrNull?.SetIsSelected(false);
      this.m_selectedItem = (Option<BlueprintView>) Option.None;
      this.updateRightBars();
    }

    private void updateRightBars()
    {
      this.m_rightTopBar.SetVisibility<StackContainer>(this.m_selectedItem.HasValue);
      if (this.m_selectedItem.IsNone)
        return;
      bool hasValue = this.m_selectedItem.Value.Blueprint.HasValue;
      this.m_rightTopBar.SetItemVisibility((IUiElement) this.m_placeItBtn, hasValue);
      this.m_rightTopBar.SetItemVisibility((IUiElement) this.m_infoBtn, hasValue);
      this.m_infoBtnTooltip.SetText(hasValue ? Tr.MadeInVersion.Format(this.m_selectedItem.Value.Blueprint.Value.GameVersion ?? "").Value : "");
    }

    private void onBlueprintDoubleClick(BlueprintView blueprintView)
    {
      if (blueprintView.Blueprint.HasValue)
      {
        this.m_controller.Value.StartBlueprintPlacement(blueprintView.Blueprint.Value);
      }
      else
      {
        if (!blueprintView.BlueprintsFolder.HasValue)
          return;
        this.CurrentFolder = blueprintView.BlueprintsFolder.Value;
      }
    }

    private void exportSelectedItemToString()
    {
      if (this.m_selectedItem.IsNone)
        return;
      if (this.m_selectedItem.Value.Blueprint.HasValue)
      {
        this.m_blueprintCopyPasteDialog.ShowForStringExport(this.BlueprintsLibrary.ConvertToString(this.m_selectedItem.Value.Blueprint.Value));
      }
      else
      {
        if (!this.m_selectedItem.Value.BlueprintsFolder.HasValue)
          return;
        this.m_blueprintCopyPasteDialog.ShowForStringExport(this.BlueprintsLibrary.ConvertToString(this.m_selectedItem.Value.BlueprintsFolder.Value));
      }
    }

    private void startBlueprintDelete()
    {
      if (this.m_selectedItem.IsNone)
        return;
      if (this.m_selectedItem.Value.Item.ValueOrNull is BlueprintsFolder valueOrNull && valueOrNull.IsEmpty)
        this.onDeleteConfirm();
      else
        this.m_confirmDeleteDialog.SetNameAndShow(this.m_selectedItem.Value.Title);
    }

    private bool onStringImportClick(string stringToImport)
    {
      return !string.IsNullOrEmpty(stringToImport) && this.BlueprintsLibrary.TryAddBlueprintFromString(this.CurrentFolder, stringToImport, out IBlueprintItem _);
    }

    private void onDeleteConfirm()
    {
      if (this.m_selectedItem.IsNone || !this.m_selectedItem.Value.Item.HasValue)
        return;
      this.BlueprintsLibrary.DeleteItem(this.CurrentFolder, this.m_selectedItem.Value.Item.Value);
    }

    private void onDescriptionApply(string newDescription)
    {
      if (this.m_selectedItem.IsNone || !this.m_selectedItem.Value.Item.HasValue)
        return;
      this.BlueprintsLibrary.SetDescription(this.m_selectedItem.Value.Item.Value, newDescription);
      this.m_selectedItem.Value.UpdateDesc();
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.IsVisible && this.m_newItem.HasValue)
      {
        foreach (BlueprintView allExistingOne in this.m_viewsCache.AllExistingOnes())
        {
          if (this.m_newItem == allExistingOne.Item)
          {
            this.onItemSelected(allExistingOne);
            allExistingOne.StartRenamingSession();
            this.m_newItem = (Option<IBlueprintItem>) Option.None;
            this.m_scrollableContainer.ScrollToElement(allExistingOne.RectTransform);
            break;
          }
        }
      }
      if (this.m_blueprintCopyPasteDialog.IsVisible() && this.m_blueprintCopyPasteDialog.InputUpdate())
        return true;
      if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
      {
        if (this.m_blueprintCopyPasteDialog.IsVisible())
        {
          this.m_blueprintCopyPasteDialog.Hide();
          return true;
        }
        if (this.m_confirmDeleteDialog.IsVisible())
        {
          this.m_confirmDeleteDialog.Hide();
          return true;
        }
        if (this.m_blueprintDescDialog.IsVisible())
        {
          this.m_blueprintDescDialog.Hide();
          return true;
        }
        foreach (BlueprintView allExistingOne in this.m_viewsCache.AllExistingOnes())
        {
          if (allExistingOne.CancelRenamingIfCan())
            return true;
        }
        return false;
      }
      if (this.m_blueprintCopyPasteDialog.IsVisible() || this.m_confirmDeleteDialog.IsVisible() || this.m_blueprintDescDialog.IsVisible())
        return true;
      if (UnityEngine.Input.GetKeyDown(KeyCode.Backspace) && this.CurrentFolder.ParentFolder.HasValue)
      {
        this.CurrentFolder = this.CurrentFolder.ParentFolder.Value;
        return true;
      }
      if (UnityEngine.Input.GetKey(KeyCode.Return) || UnityEngine.Input.GetKey(KeyCode.KeypadEnter))
      {
        foreach (BlueprintView allExistingOne in this.m_viewsCache.AllExistingOnes())
        {
          if (allExistingOne.CommitRenamingIfCan())
            return true;
        }
      }
      if (this.m_selectedItem.HasValue)
      {
        if (UnityEngine.Input.GetKeyDown(KeyCode.F2))
        {
          this.m_selectedItem.Value.StartRenamingSession();
          return true;
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Delete))
        {
          this.startBlueprintDelete();
          return true;
        }
      }
      return false;
    }

    public void OnDragStart(BlueprintView viewDragged)
    {
      this.m_viewBeingDragged = (Option<BlueprintView>) viewDragged;
      this.m_viewBeingDragged.Value.SetScale<BlueprintView>(new Vector2(0.7f, 0.7f));
      this.m_viewBeingDragged.Value.SetParent<BlueprintView>((IUiElement) this);
    }

    public void OnDragDone()
    {
      if (this.m_viewBeingDragged.IsNone)
        return;
      if (this.m_indexToMoveTo >= 0)
      {
        this.BlueprintsLibrary.TryReorderItem(this.m_viewBeingDragged.Value.Item.Value, this.CurrentFolder, this.m_indexToMoveTo);
        this.m_indexToMoveTo = -1;
      }
      else if (this.m_lastHoveredView.HasValue)
      {
        this.m_lastHoveredView.Value.SetHovered(false);
        Option<IBlueprintsFolder> blueprintsFolder = this.m_lastHoveredView.Value.BlueprintsFolder;
        IBlueprintsFolder valueOrNull1 = blueprintsFolder.ValueOrNull;
        if (valueOrNull1 != null)
        {
          IBlueprint valueOrNull2 = this.m_viewBeingDragged.Value.Blueprint.ValueOrNull;
          blueprintsFolder = this.m_viewBeingDragged.Value.BlueprintsFolder;
          IBlueprintsFolder valueOrNull3 = blueprintsFolder.ValueOrNull;
          if (valueOrNull2 != null)
            this.BlueprintsLibrary.MoveBlueprint(valueOrNull2, this.CurrentFolder, valueOrNull1);
          else if (valueOrNull3 != null)
            this.BlueprintsLibrary.MoveFolder(valueOrNull3, valueOrNull1);
        }
      }
      this.m_lastHoveredView = Option<BlueprintView>.None;
      this.m_viewBeingDragged = (Option<BlueprintView>) Option.None;
      this.m_insertPanel.Hide<Panel>();
      this.updateItems(this.CurrentFolder.Folders, this.CurrentFolder.Blueprints);
      this.clearSelectedItem();
    }

    public void OnDragMove(Vector2 screenPoint)
    {
      if (this.m_viewBeingDragged.IsNone)
        return;
      BlueprintView parent = this.m_viewsCache.AllExistingOnes().FirstOrDefault<BlueprintView>((Predicate<BlueprintView>) (x =>
      {
        if (!(x != this.m_viewBeingDragged))
          return false;
        return RectTransformUtility.RectangleContainsScreenPoint(x.RectTransform, screenPoint) || RectTransformUtility.RectangleContainsScreenPoint(x.RectTransform, screenPoint - new Vector2(2f, 0.0f));
      }));
      if (parent == null)
      {
        this.m_lastHoveredView.ValueOrNull?.SetHovered(false);
        this.m_lastHoveredView = (Option<BlueprintView>) Option.None;
        this.m_insertPanel.Hide<Panel>();
        this.m_indexToMoveTo = -1;
      }
      else
      {
        float num1 = screenPoint.x - parent.RectTransform.position.x;
        float num2 = (num1 - parent.RectTransform.rect.width * parent.RectTransform.lossyScale.x).Abs();
        bool flag1 = this.m_viewBeingDragged.Value.BlueprintsFolder.HasValue && parent.BlueprintsFolder.HasValue || this.m_viewBeingDragged.Value.Blueprint.HasValue && parent.Blueprint.HasValue;
        if (this.CurrentFolder.ParentFolder.HasValue && parent == this.m_viewsCache.AllExistingOnes().First<BlueprintView>())
          flag1 = false;
        int num3 = parent.BlueprintsFolder.HasValue ? 20 : 60;
        bool flag2 = (double) num1 < (double) num3;
        bool flag3 = (double) num2 < (double) num3;
        if (flag1 && flag2 | flag3)
        {
          this.m_lastHoveredView.ValueOrNull?.SetHovered(false);
          this.m_lastHoveredView = (Option<BlueprintView>) Option.None;
          this.m_indexToMoveTo = this.m_viewsCache.AllExistingOnes().IndexOf<BlueprintView>(parent);
          if (this.CurrentFolder.ParentFolder.HasValue)
            --this.m_indexToMoveTo;
          if (flag2)
          {
            this.m_insertPanel.PutToLeftOf<Panel>((IUiElement) parent, this.m_insertPanel.GetWidth(), Offset.Left(-this.m_insertPanel.GetWidth())).Show<Panel>();
          }
          else
          {
            this.m_insertPanel.PutToRightOf<Panel>((IUiElement) parent, this.m_insertPanel.GetWidth(), Offset.Right(-this.m_insertPanel.GetWidth())).Show<Panel>();
            ++this.m_indexToMoveTo;
          }
          this.m_insertPanel.SetParent<Panel>((IUiElement) this.m_gridContainer);
          this.m_insertPanel.SendToFront<Panel>();
        }
        else
        {
          this.m_indexToMoveTo = -1;
          this.m_insertPanel.Hide<Panel>();
          if (parent.BlueprintsFolder.IsNone)
          {
            this.m_lastHoveredView.ValueOrNull?.SetHovered(false);
            this.m_lastHoveredView = (Option<BlueprintView>) Option.None;
          }
          else
          {
            if (parent == this.m_lastHoveredView || !(parent != this.m_lastHoveredView))
              return;
            this.m_lastHoveredView.ValueOrNull?.SetHovered(false);
            this.m_lastHoveredView = (Option<BlueprintView>) parent;
            parent.SetHovered(true);
          }
        }
      }
    }
  }
}
