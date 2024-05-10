// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipSlotContent
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipSlotContent : IUiElement
  {
    internal static readonly ColorRgba DisabledFgrClr;
    private readonly Btn m_container;
    private readonly Txt m_text;
    private readonly IconContainer m_icon;
    private readonly IconContainer m_upgradeAvailableIcon;
    private Action m_onClick;
    private readonly Tooltip m_tooltip;
    private readonly Panel m_help;
    private readonly BtnStyle m_btnStyle;
    private readonly BtnStyle m_btnStyleHighlighted;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ShipSlotContent(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_btnStyle = builder.Style.Global.ListMenuBtnDarker;
      BtnStyle listMenuBtnDarker = builder.Style.Global.ListMenuBtnDarker;
      ref BtnStyle local = ref listMenuBtnDarker;
      BorderStyle? nullable = new BorderStyle?(new BorderStyle((ColorRgba) 16499716, 2f));
      TextStyle? text = new TextStyle?();
      BorderStyle? border = nullable;
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = new ColorRgba?();
      ColorRgba? hoveredClr = new ColorRgba?();
      ColorRgba? pressedClr = new ColorRgba?();
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      bool? shadow = new bool?();
      int? width = new int?();
      int? height = new int?();
      int? sidePaddings = new int?();
      Offset? iconPadding = new Offset?();
      this.m_btnStyleHighlighted = local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      this.m_container = builder.NewBtn("SlotContent").SetButtonStyle(this.m_btnStyle).OnClick((Action) (() => this.m_onClick()));
      this.m_icon = builder.NewIconContainer("Icon").SetColor(this.m_btnStyle.Text.Color).PutTo<IconContainer>((IUiElement) this.m_container, Offset.All(5f));
      this.m_text = builder.NewTxt("ItemDesc").SetTextStyle(builder.Style.Global.Text.Extend(new ColorRgba?(this.m_btnStyle.Text.Color))).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, Offset.Bottom(2f));
      this.m_upgradeAvailableIcon = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Export128.png").SetColor(new ColorRgba(6998383)).PutToLeftTopOf<IconContainer>((IUiElement) this.m_container, 18.Vector2(), Offset.TopLeft(5f, 5f));
      this.m_help = builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToRightTopOf<Panel>((IUiElement) this.m_container, 20.Vector2(), Offset.TopRight(5f, 5f));
      this.m_tooltip = builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.m_help);
    }

    public void SetItem(FleetEntityPartProto item, Action onClick)
    {
      this.m_onClick = onClick;
      this.m_icon.SetIcon(item.Graphics.IconPath);
      this.m_text.SetText((LocStrFormatted) item.Strings.Name);
      this.m_tooltip.SetText((LocStrFormatted) item.Strings.DescShort);
      this.m_help.SetVisibility<Panel>(item.Strings.DescShort.TranslatedString.IsNotEmpty());
      this.SetHighlighted(false);
      this.SetDisabled(false);
      this.SetUpgradeAvailable(false);
    }

    public void SetHighlighted(bool isHighlighted)
    {
      if (isHighlighted)
        this.m_container.SetButtonStyle(this.m_btnStyleHighlighted);
      else
        this.m_container.SetButtonStyle(this.m_btnStyle);
    }

    public void SetDisabled(bool isDisabled)
    {
      this.m_container.SetEnabled(!isDisabled);
      if (isDisabled)
      {
        this.m_icon.SetColor(ShipSlotContent.DisabledFgrClr);
        this.m_text.SetColor(ShipSlotContent.DisabledFgrClr);
      }
      else
      {
        this.m_icon.SetColor(this.m_btnStyle.Text.Color);
        this.m_text.SetColor(this.m_btnStyle.Text.Color);
      }
    }

    public void SetUpgradeAvailable(bool isAvailable)
    {
      this.m_upgradeAvailableIcon.SetVisibility<IconContainer>(isAvailable);
    }

    static ShipSlotContent()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShipSlotContent.DisabledFgrClr = (ColorRgba) 7500402;
    }
  }
}
