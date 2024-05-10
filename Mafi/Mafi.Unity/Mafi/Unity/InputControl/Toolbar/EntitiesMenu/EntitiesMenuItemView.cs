// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.EntitiesMenuItemView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  internal class EntitiesMenuItemView : IUiElement
  {
    public readonly EntitiesMenuItem Item;
    private bool m_isNew;
    private UiStyle m_style;
    private Txt m_title;
    private IconContainer m_icon;
    private Btn m_button;
    private bool m_isSelected;
    private readonly MenuPopupController m_popupController;
    private readonly Action<EntitiesMenuItemView> m_clickAction;
    private readonly AudioSource m_clickSound;
    private IconContainer m_newItemsIcon;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public bool IsNew
    {
      get => this.m_isNew;
      set
      {
        this.m_isNew = value;
        this.m_newItemsIcon.SetVisibility<IconContainer>(this.m_isNew);
      }
    }

    public EntitiesMenuItemView(
      IUiElement parent,
      UiBuilder builder,
      MenuPopupController popupController,
      EntitiesMenuItem item,
      Action<EntitiesMenuItemView> clickAction,
      AudioSource clickSound)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_popupController = popupController;
      this.Item = item;
      this.m_clickAction = clickAction;
      this.m_clickSound = clickSound;
      this.build(parent, builder);
    }

    private void build(IUiElement parent, UiBuilder builder)
    {
      this.m_style = builder.Style;
      EntitiesMenuUiStyle entitiesMenu = builder.Style.EntitiesMenu;
      this.m_button = builder.NewBtn("Button", parent).OnClick(new Action(this.onClick), this.m_clickSound).SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave)).PlayErrorSoundWhenDisabled();
      Panel parent1 = builder.NewPanel("IconHolder", (IUiElement) this.m_button).PutTo<Panel>((IUiElement) this.m_button, Offset.Bottom((float) (entitiesMenu.ItemTitleHeight - entitiesMenu.ItemImageBottomOffestHack)));
      this.m_icon = builder.NewIconContainer("Icon", (IUiElement) parent1).SetIcon(this.Item.IconPath, entitiesMenu.ItemIconClr).PutToCenterMiddleOf<IconContainer>((IUiElement) parent1, entitiesMenu.ItemIconSize);
      this.m_title = builder.NewTxt("Title", (IUiElement) this.m_button).SetText(this.Item.Name.Value).SetAlignment(TextAnchor.UpperCenter).SetTextStyle(entitiesMenu.ItemTitleStyle).AllowVerticalOverflow().PutToBottomOf<Txt>((IUiElement) this.m_button, (float) entitiesMenu.ItemTitleHeight);
      this.m_newItemsIcon = builder.NewIconContainer("Icon", (IUiElement) this.m_button).SetIcon("Assets/Unity/UserInterface/General/Circle.svg", (ColorRgba) 16756491).PutToRightTopOf<IconContainer>((IUiElement) this.m_button, 8.Vector2(), Offset.Top(10f) + Offset.Right(5f)).Hide<IconContainer>();
    }

    private void onMouseEnter()
    {
      if (!this.m_isSelected)
        this.colorAsHovered();
      this.m_popupController.ItemHovered((Option<EntitiesMenuItem>) this.Item, this.m_title.X);
    }

    private void onMouseLeave()
    {
      if (!this.m_isSelected)
        this.colorAsNormal();
      this.m_popupController.ItemHovered((Option<EntitiesMenuItem>) Option.None, 0.0f);
    }

    public void SetSelected(bool selected)
    {
      if (this.m_isSelected == selected)
        return;
      this.m_isSelected = selected;
      this.m_icon.SetSize<IconContainer>(this.m_isSelected ? this.m_style.EntitiesMenu.ItemIconHoveredSize : this.m_style.EntitiesMenu.ItemIconSize);
      if (!this.m_isSelected)
        this.colorAsNormal();
      else
        this.colorAsHovered();
    }

    private void onClick()
    {
      this.SetSelected(true);
      this.m_clickAction(this);
    }

    private void colorAsNormal()
    {
      this.m_icon.SetColor(this.m_style.EntitiesMenu.ItemIconClr);
      this.m_title.SetColor(this.m_style.EntitiesMenu.ItemTitleStyle.Color);
    }

    private void colorAsHovered()
    {
      this.m_icon.SetColor(this.m_style.EntitiesMenu.ItemIconHoveredClr);
      this.m_title.SetColor(this.m_style.EntitiesMenu.ItemTitleHoveredClr);
    }
  }
}
