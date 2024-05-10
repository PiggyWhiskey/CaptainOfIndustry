// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.EntitiesMenuView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.UiState;
using Mafi.Core.Utils;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class EntitiesMenuView : View
  {
    private StackContainer m_itemsContainer;
    private ScrollableContainer m_scrollableContainer;
    private IconContainer m_leftArrow;
    private IconContainer m_rightArrow;
    private AudioSource m_selectEntitySound;
    private ImmutableArray<EntitiesMenuItem> m_allItemsForSearch;
    private readonly Dict<EntitiesMenuItem, EntitiesMenuItemView> m_itemsCache;
    private Option<EntitiesMenuItemView> m_selectedItemView;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ExpressionEvaluator m_expressionEvaluator;
    private readonly MenuPopupController m_popupController;
    private readonly ToolbarController m_toolbarController;
    private readonly NewProtosTracker m_newProtosTracker;
    private readonly EntitiesMenuStripInfoView m_topView;
    private TxtField m_searchBox;
    private Txt m_nothingFoundInfo;
    private Option<EntitiesMenuController> m_currentController;
    private readonly Lyst<EntitiesMenuItem> m_itemsToSearchIn;
    private readonly Set<EntitiesMenuItem> m_itemsFound;

    public EntitiesMenuView(
      UnlockedProtosDbForUi unlockedProtosDb,
      ExpressionEvaluator expressionEvaluator,
      MenuPopupController popupController,
      ToolbarController toolbarController,
      NewProtosTracker newProtosTracker,
      EntitiesMenuStripInfoView topView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_itemsCache = new Dict<EntitiesMenuItem, EntitiesMenuItemView>();
      this.m_itemsToSearchIn = new Lyst<EntitiesMenuItem>();
      this.m_itemsFound = new Set<EntitiesMenuItem>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (EntitiesMenuView), SyncFrequency.OncePerSec);
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_expressionEvaluator = expressionEvaluator;
      this.m_popupController = popupController;
      this.m_toolbarController = toolbarController;
      this.m_newProtosTracker = newProtosTracker;
      this.m_topView = topView;
    }

    public void Build(UiBuilder builder, ImmutableArray<EntitiesMenuItem> allItems)
    {
      this.m_allItemsForSearch = allItems;
      this.BuildUi(builder);
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.onSomethingUnlocked);
    }

    protected override void BuildUi()
    {
      EntitiesMenuUiStyle entitiesMenu = this.Style.EntitiesMenu;
      this.m_selectEntitySound = this.Builder.AudioDb.GetSharedAudio(this.Builder.Audio.EntitySelect);
      Panel container = this.Builder.NewPanel("Bg").SetBackground(entitiesMenu.MenuBg).PutTo<Panel>((IUiElement) this);
      this.Builder.NewPanel("TopBorder").SetBackground(entitiesMenu.BorderBg).PutToTopOf<Panel>((IUiElement) container, (float) entitiesMenu.TopBorderSize, Offset.Top((float) -entitiesMenu.TopBorderSize));
      this.PutToBottomOf<EntitiesMenuView>((IUiElement) this.Builder.MainCanvas, (float) entitiesMenu.ItemHeight + this.Style.Toolbar.MainMenuStripHeight, Offset.Left(102f)).SendToBack<EntitiesMenuView>();
      this.Builder.NewPanel("BottomStripBg").SetBackground(this.Builder.Style.Global.PanelsBg).PutToBottomOf<Panel>((IUiElement) container, this.Style.Toolbar.MainMenuStripHeight);
      this.m_searchBox = this.Builder.NewTxtField("Search").SetStyle(this.Builder.Style.Global.LightTxtFieldStyle).SetPlaceholderText(Tr.Search).SetCharLimit(30);
      this.m_searchBox.SetDelayedOnEditEndListener(new Action<string>(this.search));
      this.m_toolbarController.AddSearchBox((IUiElement) this.m_searchBox, 140f);
      this.m_searchBox.Hide<TxtField>();
      Panel centerOf = this.Builder.NewPanel("NothingFoundContainer").PutToCenterOf<Panel>((IUiElement) this, 1200f);
      Txt txt = this.Builder.NewTxt("NothingFound").SetAlignment(TextAnchor.MiddleLeft);
      TextStyle text = this.Builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable1 = new int?(16);
      FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable2;
      int? fontSize = nullable1;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_nothingFoundInfo = txt.SetTextStyle(textStyle).PutToLeftTopOf<Txt>((IUiElement) centerOf, new Vector2(200f, 30f), Offset.Top(40f)).EnableRichText().Hide<Txt>();
      this.m_itemsContainer = this.Builder.NewStackContainer("StackingContainer").SetItemSpacing((float) entitiesMenu.ItemsSpacing);
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer").DisableVerticalScroll().SetScrollSensitivity((float) -(entitiesMenu.ItemWidth + entitiesMenu.ItemsSpacing)).SetDecelerationRate(0.005f).SetOnScrollAction((Action) (() => this.m_popupController.HideAll())).SetOnDragAction((Action) (() => this.m_popupController.HideAll())).PutTo<ScrollableContainer>((IUiElement) container, new Offset((float) entitiesMenu.ScrollViewSideOffset, 0.0f, (float) entitiesMenu.ScrollViewSideOffset, (float) ((double) this.Style.Toolbar.MainMenuStripHeight + (double) entitiesMenu.BottomBorderSize + 1.0)));
      this.m_scrollableContainer.AddItemCenter((IUiElement) this.m_itemsContainer);
      IconContainer leftOf = this.Builder.NewIconContainer("LeftOverlay").SetIcon(entitiesMenu.SideOverlayLeft).PutToLeftOf<IconContainer>((IUiElement) container, (float) entitiesMenu.SideOverlayWidth, Offset.Bottom(this.Style.Toolbar.MainMenuStripHeight));
      this.m_leftArrow = this.Builder.NewIconContainer("Arrow").SetIcon(entitiesMenu.SideOverlayArrowLeft).PutToLeftMiddleOf<IconContainer>((IUiElement) leftOf, entitiesMenu.SideOverlayArrowLeft.Size);
      IconContainer rightOf = this.Builder.NewIconContainer("RightOverlay").SetIcon(entitiesMenu.SideOverlayRight).PutToRightOf<IconContainer>((IUiElement) container, (float) entitiesMenu.SideOverlayWidth, Offset.Bottom(this.Style.Toolbar.MainMenuStripHeight));
      this.m_rightArrow = this.Builder.NewIconContainer("Arrow").SetIcon(entitiesMenu.SideOverlayArrowRight).PutToRightMiddleOf<IconContainer>((IUiElement) rightOf, entitiesMenu.SideOverlayArrowRight.Size);
      this.Builder.NewPanel("BottomTopShadow").SetBackground("Assets/Unity/UserInterface/General/ShadowTop32.png", new ColorRgba?(new ColorRgba(0, 150))).PutToBottomOf<Panel>((IUiElement) container, (float) entitiesMenu.BottomBorderSize, Offset.Bottom(this.Style.Toolbar.MainMenuStripHeight));
      this.OnShowStart += (Action) (() =>
      {
        if (this.m_searchBox.GetText().IsNotEmpty())
        {
          this.m_searchBox.ClearInput();
          this.search("");
        }
        this.m_searchBox.Show<TxtField>();
        this.m_topView.PutToTopOf<EntitiesMenuStripInfoView>((IUiElement) container, this.m_topView.GetHeight(), Offset.Top(-this.m_topView.GetHeight()));
        this.m_topView.Show();
      });
      this.OnHide += (Action) (() =>
      {
        this.m_popupController.HideAll();
        if (this.m_selectedItemView.HasValue)
        {
          this.m_selectedItemView.Value.SetSelected(false);
          this.m_selectedItemView = (Option<EntitiesMenuItemView>) Option.None;
        }
        this.m_searchBox.Hide<TxtField>();
        this.m_topView.Hide();
      });
    }

    private EntitiesMenuItemView getOrCreateView(EntitiesMenuItem item)
    {
      EntitiesMenuItemView element;
      if (this.m_itemsCache.TryGetValue(item, out element))
      {
        element.Show<EntitiesMenuItemView>();
        return element;
      }
      EntitiesMenuItemView view = new EntitiesMenuItemView((IUiElement) this.m_itemsContainer, this.Builder, this.m_popupController, item, new Action<EntitiesMenuItemView>(this.onItemClick), this.m_selectEntitySound);
      this.m_itemsCache.Add(item, view);
      view.IsNew = item.Proto.HasValue && this.m_newProtosTracker.IsNew(item.Proto.Value);
      return view;
    }

    private void search(string text)
    {
      if (text == null || text.IsEmpty())
      {
        this.updateItemsVisibility();
        this.m_nothingFoundInfo.Hide<Txt>();
      }
      else
      {
        string result;
        if (this.m_expressionEvaluator.TryEvaluate(text, out result))
        {
          this.m_itemsContainer.HideAll();
          this.m_nothingFoundInfo.SetText(text + " = <size=18>" + result + "</size>").Show<Txt>();
        }
        else
        {
          foreach (EntitiesMenuItem entitiesMenuItem in this.m_allItemsForSearch)
          {
            if (entitiesMenuItem.IsUnlocked(this.m_unlockedProtosDb))
              this.m_itemsToSearchIn.Add(entitiesMenuItem);
          }
          UiSearchUtils.MatchItems<EntitiesMenuItem>(text, (IIndexable<EntitiesMenuItem>) this.m_itemsToSearchIn, (Func<EntitiesMenuItem, string>) (i => i.Name.Value), this.m_itemsFound);
          this.m_itemsToSearchIn.Clear();
          this.m_itemsContainer.ClearAll(true);
          this.m_itemsContainer.StartBatchOperation();
          foreach (EntitiesMenuItem entitiesMenuItem in this.m_itemsFound)
            this.m_itemsContainer.Append((IUiElement) this.getOrCreateView(entitiesMenuItem), new float?((float) this.Style.EntitiesMenu.ItemWidth));
          this.m_itemsContainer.FinishBatchOperation();
          if (this.m_itemsFound.IsEmpty)
            this.m_nothingFoundInfo.SetText(Tr.NothingFoundFor.Format(text)).Show<Txt>();
          else
            this.m_nothingFoundInfo.Hide<Txt>();
        }
      }
    }

    private void onSomethingUnlocked()
    {
      if (!this.m_currentController.HasValue || !this.IsVisible)
        return;
      this.updateItemsVisibility();
    }

    private void updateItemsVisibility()
    {
      if (this.m_currentController.IsNone)
      {
        Log.Error("Parent controller not set");
      }
      else
      {
        IReadOnlySet<EntitiesMenuItem> thisMenuItems = this.m_currentController.Value.ThisMenuItems;
        this.m_itemsContainer.ClearAll(true);
        this.m_itemsContainer.StartBatchOperation();
        foreach (EntitiesMenuItem entitiesMenuItem in (IEnumerable<EntitiesMenuItem>) thisMenuItems)
        {
          if (entitiesMenuItem.IsUnlocked(this.m_unlockedProtosDb))
            this.m_itemsContainer.Append((IUiElement) this.getOrCreateView(entitiesMenuItem), new float?((float) this.Style.EntitiesMenu.ItemWidth));
        }
        this.m_itemsContainer.FinishBatchOperation();
      }
    }

    public void SetControllerAndShow(EntitiesMenuController controller)
    {
      this.m_currentController = (Option<EntitiesMenuController>) controller;
      this.updateItemsVisibility();
      this.Show();
    }

    private void onItemClick(EntitiesMenuItemView itemView)
    {
      if (this.m_selectedItemView == itemView)
        return;
      if (this.m_selectedItemView.HasValue)
        this.m_selectedItemView.Value.SetSelected(false);
      this.m_selectedItemView = (Option<EntitiesMenuItemView>) itemView;
      this.m_popupController.HideAll();
      this.m_currentController.ValueOrNull?.OnItemSelected(itemView.Item);
      if (!itemView.IsNew)
        return;
      itemView.IsNew = false;
      if (itemView.Item.Proto.HasValue)
        this.m_newProtosTracker.MarkProtoAsSeen(itemView.Item.Proto.Value);
      this.m_currentController.ValueOrNull?.UpdateNewItemBadge();
    }

    public void FocusSearchBox()
    {
      Assert.That<bool>(this.IsVisible).IsTrue();
      this.m_searchBox.Focus();
    }

    public void ClearSelected()
    {
      if (this.m_selectedItemView.IsNone)
        return;
      this.m_selectedItemView.Value.SetSelected(false);
      this.m_selectedItemView = (Option<EntitiesMenuItemView>) Option.None;
    }

    public void SetSelected(Proto selectedProto)
    {
      this.ClearSelected();
      foreach (EntitiesMenuItemView entitiesMenuItemView in this.m_itemsCache.Values)
      {
        if (entitiesMenuItemView.Item.Proto == selectedProto)
        {
          this.m_selectedItemView = (Option<EntitiesMenuItemView>) entitiesMenuItemView;
          entitiesMenuItemView.SetSelected(true);
          this.m_popupController.HideAll();
          break;
        }
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_leftArrow.SetVisibility<IconContainer>(this.m_scrollableContainer.CanScrollLeft());
      this.m_rightArrow.SetVisibility<IconContainer>(this.m_scrollableContainer.CanScrollRight());
    }
  }
}
