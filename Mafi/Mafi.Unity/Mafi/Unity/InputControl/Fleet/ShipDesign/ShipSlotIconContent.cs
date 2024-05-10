// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipSlotIconContent
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

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
  public class ShipSlotIconContent : IUiElement
  {
    private readonly Btn m_container;
    private readonly Txt m_text;
    private Action m_onClick;
    private readonly BtnStyle m_btnStyle;
    private readonly BtnStyle m_btnStyleHighlighted;
    private readonly IconContainer m_icon;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ShipSlotIconContent(UiBuilder builder, string icon)
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
      this.m_icon = builder.NewIconContainer("Icon").SetIcon(icon).SetColor(this.m_btnStyle.Text.Color).PutToCenterMiddleOf<IconContainer>((IUiElement) this.m_container, 26.Vector2());
      this.m_text = builder.NewTxt("ItemDesc").SetTextStyle(builder.Style.Global.Text.Extend(new ColorRgba?(this.m_btnStyle.Text.Color))).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, Offset.Bottom(2f));
    }

    public void SetHighlighted(bool isHighlighted)
    {
      if (isHighlighted)
        this.m_container.SetButtonStyle(this.m_btnStyleHighlighted);
      else
        this.m_container.SetButtonStyle(this.m_btnStyle);
    }

    public void SetGroupName(string name) => this.m_text.SetText(name);

    public void SetOnClick(Action onClick) => this.m_onClick = onClick;

    public void SetEnabled(bool isEnabled)
    {
      this.m_container.SetEnabled(isEnabled);
      if (!isEnabled)
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
  }
}
