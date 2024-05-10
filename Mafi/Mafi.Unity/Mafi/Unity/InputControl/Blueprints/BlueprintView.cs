// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities.Blueprints;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  internal class BlueprintView : IUiElementWithUpdater, IUiElement
  {
    private readonly BlueprintsView m_owner;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    public static readonly int WIDTH;
    public static readonly Vector2 SIZE;
    private readonly Btn m_container;
    private readonly GridContainer m_gridContainer;
    private readonly ViewsCacheHomogeneous<BlueprintView.IconWithCount> m_iconsCache;
    private readonly Tooltip m_lockedIconTooltip;
    private readonly IconContainer m_bg;
    private bool m_isParentFolder;
    private bool m_isHovered;
    private readonly Set<Proto> m_lockedProtosCache;
    private readonly Dict<IProto, IProto> m_downgradesMap;
    private readonly StringBuilder m_sb;
    private bool m_isInRenamingSession;
    private readonly Txt m_title;
    private readonly Btn m_textEditBtn;
    private readonly TxtField m_txtInput;
    private readonly Btn m_textSaveBtn;
    private readonly Panel m_hoveredBorder;
    private readonly Panel m_titleBar;
    private readonly Tooltip m_descTooltip;
    private readonly IconContainer m_missingProtosIcon;
    private readonly Tooltip m_missingProtosTooltip;
    private readonly Panel m_lockedOverlay;
    private readonly Panel m_selectedBorder;
    private readonly IconContainer m_downgradeIcon;
    private readonly Tooltip m_downgradeIconTooltip;
    private static readonly ColorRgba BG_ICON_COLOR;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public Option<IBlueprintItem> Item
    {
      get
      {
        return ((IBlueprintItem) this.Blueprint.ValueOrNull ?? (IBlueprintItem) this.BlueprintsFolder.ValueOrNull).CreateOption<IBlueprintItem>();
      }
    }

    public Option<IBlueprint> Blueprint { get; private set; }

    public Option<IBlueprintsFolder> BlueprintsFolder { get; private set; }

    public bool IsLocked => this.m_lockedOverlay.IsVisible();

    public string Title => this.m_title.Text;

    public BlueprintView(
      IUiElement parent,
      BlueprintsView owner,
      UnlockedProtosDbForUi unlockedProtosDb,
      UiBuilder builder,
      Action<BlueprintView> onClick,
      Action<BlueprintView> onDoubleClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lockedProtosCache = new Set<Proto>();
      this.m_downgradesMap = new Dict<IProto, IProto>();
      this.m_sb = new StringBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BlueprintView blueprintView = this;
      this.m_owner = owner;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_container = builder.NewBtn("Container", parent).SetButtonStyle(builder.Style.Global.ListMenuBtnDarker).OnClick((Action) (() => onClick(blueprintView))).OnDoubleClick((Action) (() =>
      {
        if (blueprintView.m_lockedOverlay.IsVisible())
          return;
        onDoubleClick(blueprintView);
      })).SetOnMouseEnterLeaveActions((Action) (() =>
      {
        blueprintView.m_isHovered = true;
        blueprintView.m_textEditBtn.SetVisibility<Btn>(!blueprintView.m_isParentFolder && !blueprintView.m_isInRenamingSession);
      }), (Action) (() =>
      {
        blueprintView.m_isHovered = false;
        blueprintView.m_textEditBtn.Hide<Btn>();
      }));
      this.m_descTooltip = this.m_container.AddToolTipAndReturn();
      this.m_bg = builder.NewIconContainer("Bg").PutTo<IconContainer>((IUiElement) this.m_container, Offset.All(20f));
      this.m_hoveredBorder = builder.NewPanel("border").PutTo<Panel>((IUiElement) this.m_container).SetBorderStyle(new BorderStyle((ColorRgba) 10461087)).Hide<Panel>();
      this.m_selectedBorder = builder.NewPanel("border").PutTo<Panel>((IUiElement) this.m_container).SetBorderStyle(new BorderStyle((ColorRgba) 10123554)).Hide<Panel>();
      int coord = (BlueprintView.WIDTH - 10) / 3 - 2;
      this.m_gridContainer = builder.NewGridContainer("Container").SetCellSize(coord.Vector2()).SetCellSpacing(2f).SetDynamicHeightMode(3).SetInnerPadding(Offset.All(5f) + Offset.Top(5f)).PutToLeftTopOf<GridContainer>((IUiElement) this, Vector2.zero);
      this.m_iconsCache = new ViewsCacheHomogeneous<BlueprintView.IconWithCount>((Func<BlueprintView.IconWithCount>) (() => new BlueprintView.IconWithCount((IUiElement) blueprintView.m_gridContainer, builder)));
      this.m_lockedOverlay = builder.NewPanel("LockedOverlay").SetBackground(new ColorRgba(3355443, 120)).PutTo<Panel>((IUiElement) this.m_container);
      IconContainer centerMiddleOf = builder.NewIconContainer("LockedIcon").SetIcon("Assets/Unity/UserInterface/General/Locked128.png", new ColorRgba(11447982, 200)).PutToCenterMiddleOf<IconContainer>((IUiElement) this.m_lockedOverlay, 40.Vector2());
      this.m_lockedIconTooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) centerMiddleOf);
      this.m_titleBar = builder.NewPanel("TitleBar", (IUiElement) this.m_container).PutToCenterBottomOf<Panel>((IUiElement) this.m_container, new Vector2(0.0f, 25f), Offset.LeftRight(5f) + Offset.Bottom(5f));
      this.m_title = builder.NewTxt(nameof (Title), (IUiElement) this.m_titleBar).SetTextStyle(builder.Style.Global.TextMedium).SetAlignment(TextAnchor.LowerCenter).PutTo<Txt>((IUiElement) this.m_titleBar, Offset.Right(20f));
      this.m_textEditBtn = builder.NewBtn("Rename", (IUiElement) this.m_titleBar).SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Rename.svg", new Offset?(Offset.LeftRight(3f))).OnClick(new Action(this.StartRenamingSession)).PutToRightBottomOf<Btn>((IUiElement) this.m_titleBar, 18.Vector2()).Hide<Btn>();
      this.m_txtInput = builder.NewTxtField("SaveNameInput", (IUiElement) this.m_container).SetStyle(builder.Style.Global.LightTxtFieldStyle).SetCharLimit(60).PutToBottomOf<TxtField>((IUiElement) this.m_container, 25f, Offset.Left(5f) + Offset.Right(25f) + Offset.Bottom(5f)).Hide<TxtField>();
      this.m_missingProtosIcon = builder.NewIconContainer("MissingProtosWarning").SetIcon("Assets/Unity/UserInterface/General/Warning128.png", builder.Style.Global.OrangeText).PutToRightTopOf<IconContainer>((IUiElement) this, 22.Vector2(), Offset.TopRight(5f, 5f));
      this.m_missingProtosTooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) this.m_missingProtosIcon);
      this.m_downgradeIcon = builder.NewIconContainer("Downgrade").SetIcon("Assets/Unity/UserInterface/General/Downgrade.svg", (ColorRgba) 12352540).PutToRightTopOf<IconContainer>((IUiElement) this, 22.Vector2(), Offset.TopRight(5f, 5f));
      this.m_downgradeIconTooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) this.m_downgradeIcon);
      this.m_textSaveBtn = builder.NewBtn("Save", (IUiElement) this.m_txtInput).SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Save.svg", new Offset?(Offset.LeftRight(3f))).OnClick(new Action(this.commitRenamingSession)).PutToRightOf<Btn>((IUiElement) this.m_txtInput, 20f, Offset.Right(-22f)).Hide<Btn>();
      this.Updater = UpdaterBuilder.Start().Build();
      this.m_container.SetupDragDrop(new Action(this.onBeginDrag), new Action(this.onDrag), new Action(this.onEndDrag));
    }

    public void SetBlueprint(IBlueprint blueprint)
    {
      this.m_isParentFolder = false;
      this.SetScale<BlueprintView>(1.Vector2());
      this.m_container.SetDragEnabled(true);
      this.BlueprintsFolder = (Option<IBlueprintsFolder>) Option.None;
      this.Blueprint = blueprint.CreateOption<IBlueprint>();
      this.updateTitle();
      this.UpdateDesc();
      this.m_bg.SetIcon("Assets/Unity/UserInterface/General/Blueprint.svg", BlueprintView.BG_ICON_COLOR);
      this.m_bg.PutTo<IconContainer>((IUiElement) this.m_container, Offset.All(20f));
      this.m_lockedProtosCache.Clear();
      this.m_downgradesMap.Clear();
      this.m_sb.Clear();
      if (blueprint.ProtosThatFailedToLoad.HasValue)
      {
        this.UpdateMissingProtosTooltip();
      }
      else
      {
        foreach (Proto allDistinctProto in blueprint.AllDistinctProtos)
        {
          if (!(allDistinctProto == (Proto) null) && !this.m_unlockedProtosDb.IsUnlocked((IProto) allDistinctProto))
          {
            Option<Proto> unlockedDowngradeFor = this.m_unlockedProtosDb.GetNearestUnlockedDowngradeFor((IProto) allDistinctProto);
            if (unlockedDowngradeFor.HasValue)
              this.m_downgradesMap[(IProto) allDistinctProto] = (IProto) unlockedDowngradeFor.Value;
            else
              this.m_lockedProtosCache.Add(allDistinctProto);
          }
        }
        if (this.m_lockedProtosCache.IsNotEmpty)
        {
          this.m_sb.AppendLine(Tr.BlueprintProtosLocked__NotAvailable.TranslatedString);
          foreach (Proto proto in this.m_lockedProtosCache)
          {
            this.m_sb.Append(" - ");
            this.m_sb.AppendLine(proto.Strings.Name.TranslatedString);
          }
          this.m_lockedIconTooltip.SetText(this.m_sb.ToString());
        }
        else if (this.m_downgradesMap.IsNotEmpty)
        {
          this.m_sb.AppendLine(Tr.BlueprintProtosLocked__CanDowngrade.TranslatedString);
          foreach (KeyValuePair<IProto, IProto> downgrades in this.m_downgradesMap)
          {
            this.m_sb.Append(" - ");
            this.m_sb.Append(downgrades.Key.Strings.Name.TranslatedString);
            this.m_sb.Append(" -> ");
            this.m_sb.AppendLine(downgrades.Value.Strings.Name.TranslatedString);
          }
          this.m_downgradeIconTooltip.SetText(this.m_sb.ToString());
        }
      }
      this.m_lockedOverlay.SetVisibility<Panel>(this.m_lockedProtosCache.IsNotEmpty);
      this.m_missingProtosIcon.SetVisibility<IconContainer>(blueprint.ProtosThatFailedToLoad.HasValue);
      this.m_downgradeIcon.SetVisibility<IconContainer>(this.m_downgradesMap.IsNotEmpty && !this.m_lockedOverlay.IsVisible() && !this.m_missingProtosIcon.IsVisible());
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.ClearAll();
      this.m_iconsCache.ReturnAll();
      int num1 = 6;
      int num2 = 0;
      foreach (KeyValuePair<Proto, int> mostFrequentProto in blueprint.MostFrequentProtos)
      {
        if (num2 < num1)
        {
          Proto key = mostFrequentProto.Key;
          if (key is LayoutEntityProto layoutEntityProto)
          {
            BlueprintView.IconWithCount view = this.m_iconsCache.GetView();
            this.m_gridContainer.Append((IUiElement) view);
            view.SetIconAndCount(layoutEntityProto.Graphics.IconPath, mostFrequentProto.Value);
            ++num2;
          }
          else if (key is TransportProto transportProto)
          {
            BlueprintView.IconWithCount view = this.m_iconsCache.GetView();
            this.m_gridContainer.Append((IUiElement) view);
            view.SetIconAndCount(transportProto.Graphics.IconPath, mostFrequentProto.Value);
          }
        }
        else
          break;
      }
      this.m_gridContainer.FinishBatchOperation();
    }

    public void SetBlueprintsFolder(IBlueprintsFolder folder, bool isParentFolder = false)
    {
      this.m_isParentFolder = isParentFolder;
      this.SetScale<BlueprintView>(1.Vector2());
      this.m_container.SetDragEnabled(!this.m_isParentFolder);
      this.Blueprint = (Option<IBlueprint>) Option.None;
      this.BlueprintsFolder = folder.CreateOption<IBlueprintsFolder>();
      this.updateTitle();
      this.UpdateDesc();
      this.m_bg.SetIcon(this.m_isParentFolder ? "Assets/Unity/UserInterface/General/NavigateUp.svg" : (folder.IsEmpty ? "Assets/Unity/UserInterface/General/FolderEmpty.svg" : "Assets/Unity/UserInterface/General/Folder.svg"), BlueprintView.BG_ICON_COLOR);
      this.m_bg.PutTo<IconContainer>((IUiElement) this.m_container, Offset.All(40f));
      this.m_missingProtosIcon.Hide<IconContainer>();
      this.m_downgradeIcon.Hide<IconContainer>();
      this.m_lockedOverlay.Hide<Panel>();
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.ClearAll();
      this.m_iconsCache.ReturnAll();
      if (this.m_isParentFolder)
      {
        this.m_gridContainer.FinishBatchOperation();
      }
      else
      {
        if (folder.Blueprints.IsNotEmpty<IBlueprint>())
        {
          BlueprintView.IconWithCount view = this.m_iconsCache.GetView();
          this.m_gridContainer.Append((IUiElement) view);
          view.SetIconAndCount("Assets/Unity/UserInterface/General/Blueprint.svg", folder.Blueprints.Count);
        }
        foreach (Proto previewProto in folder.PreviewProtos)
        {
          if (previewProto is LayoutEntityProto layoutEntityProto)
          {
            BlueprintView.IconWithCount view = this.m_iconsCache.GetView();
            this.m_gridContainer.Append((IUiElement) view);
            view.SetIconAndCount(layoutEntityProto.Graphics.IconPath, 0);
          }
          else if (previewProto is TransportProto transportProto)
          {
            BlueprintView.IconWithCount view = this.m_iconsCache.GetView();
            this.m_gridContainer.Append((IUiElement) view);
            view.SetIconAndCount(transportProto.Graphics.IconPath, 0);
          }
        }
        this.m_gridContainer.FinishBatchOperation();
      }
    }

    public void SetHovered(bool isHovered) => this.m_hoveredBorder.SetVisibility<Panel>(isHovered);

    public void SetIsSelected(bool isSelected)
    {
      this.m_selectedBorder.SetVisibility<Panel>(isSelected);
    }

    private void updateTitle()
    {
      if (this.m_isInRenamingSession)
        this.stopRenamingSession();
      if (this.Item.HasValue)
        this.m_title.SetText(this.m_isParentFolder ? "" : this.Item.Value.Name);
      this.m_textEditBtn.SetVisibility<Btn>(!this.m_isParentFolder && this.m_isHovered);
      this.m_titleBar.SetWidth<Panel>(((float) ((double) this.m_title.GetPreferedWidth() + 18.0 + 2.0)).Min((float) (BlueprintView.WIDTH - 10)));
    }

    internal void UpdateDesc() => this.m_descTooltip.SetText(this.Item.ValueOrNull?.Desc ?? "");

    internal void UpdateMissingProtosTooltip()
    {
      IBlueprint valueOrNull = this.Blueprint.ValueOrNull;
      if ((valueOrNull != null ? (valueOrNull.ProtosThatFailedToLoad.HasValue ? 1 : 0) : 0) == 0)
        return;
      this.m_missingProtosTooltip.SetText(string.Format("{0}\n\n{1}\n{2}", (object) Tr.BlueprintContentMissing__Info, (object) Tr.BlueprintContentMissing__ListTitle, (object) 1).FormatInvariant((object) this.Blueprint.Value.GameVersion, (object) this.Blueprint.Value.ProtosThatFailedToLoad.Value));
    }

    private void onBeginDrag()
    {
      Assert.That<bool>(this.m_isParentFolder).IsFalse();
      this.m_missingProtosTooltip.SetText("");
      this.m_descTooltip.SetText("");
      this.m_owner.OnDragStart(this);
    }

    private void onDrag()
    {
      this.m_owner.OnDragMove((Vector2) (this.m_container.RectTransform.position - new Vector3(0.0f, this.m_container.GetHeight() / 2f)));
    }

    private void onEndDrag()
    {
      this.m_owner.OnDragDone();
      this.UpdateDesc();
      this.UpdateMissingProtosTooltip();
    }

    private void stopRenamingSession()
    {
      this.m_isInRenamingSession = false;
      this.m_txtInput.Hide<TxtField>();
      this.m_textSaveBtn.Hide<Btn>();
      this.m_textEditBtn.SetVisibility<Btn>(this.m_isHovered);
      this.m_title.Show<Txt>();
    }

    public void StartRenamingSession()
    {
      if (this.m_isInRenamingSession)
        return;
      this.m_isInRenamingSession = true;
      this.m_txtInput.Show<TxtField>();
      this.m_txtInput.Focus();
      this.m_txtInput.SetText(this.m_title.Text);
      this.m_textSaveBtn.Show<Btn>();
      this.m_textEditBtn.Hide<Btn>();
      this.m_title.Hide<Txt>();
    }

    private void commitRenamingSession()
    {
      if (this.Blueprint.HasValue)
        this.m_owner.BlueprintsLibrary.RenameItem((IBlueprintItem) this.Blueprint.Value, this.m_txtInput.GetText());
      else if (this.BlueprintsFolder.HasValue)
        this.m_owner.BlueprintsLibrary.RenameItem((IBlueprintItem) this.BlueprintsFolder.Value, this.m_txtInput.GetText());
      this.updateTitle();
    }

    internal bool CancelRenamingIfCan()
    {
      if (!this.m_isInRenamingSession)
        return false;
      this.stopRenamingSession();
      return true;
    }

    internal bool CommitRenamingIfCan()
    {
      if (!this.m_isInRenamingSession)
        return false;
      this.commitRenamingSession();
      return true;
    }

    static BlueprintView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BlueprintView.WIDTH = 160;
      BlueprintView.SIZE = new Vector2((float) BlueprintView.WIDTH, 160f);
      BlueprintView.BG_ICON_COLOR = (ColorRgba) 4348266;
    }

    private class IconWithCount : IUiElement
    {
      private readonly Panel m_container;
      private readonly IconContainer m_icon;
      private readonly Txt m_count;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IconWithCount(IUiElement parent, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("Container", parent);
        this.m_icon = builder.NewIconContainer("Icon", parent).PutTo<IconContainer>((IUiElement) this.m_container, Offset.Right(5f) + Offset.Bottom(5f));
        this.m_count = builder.NewTxt("Txt").SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.LowerLeft).AddOutline().PutToBottomOf<Txt>((IUiElement) this.m_container, 25f, Offset.Left(2f));
      }

      public void SetIconAndCount(string iconPath, int count)
      {
        this.m_icon.SetIcon(iconPath);
        this.m_count.SetVisibility<Txt>(count > 0);
        this.m_count.SetText(count.ToStringCached() + " x");
      }
    }
  }
}
