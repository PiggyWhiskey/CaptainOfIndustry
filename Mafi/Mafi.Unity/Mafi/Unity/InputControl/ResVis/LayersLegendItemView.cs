// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ResVis.LayersLegendItemView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.ResVis
{
  internal class LayersLegendItemView : IUiElement
  {
    private readonly ToggleBtn m_button;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public bool IsOn => this.m_button.IsOn;

    public LayersLegendItemView(
      IUiElement parent,
      UiBuilder builder,
      LocStrFormatted text,
      string iconPath,
      ColorRgba? color,
      Action<LayersLegendItemView> onClick,
      bool isOn)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      LayersLegendItemView layersLegendItemView = this;
      IUiElement element1 = this.buildButton(builder, text, iconPath, color, true);
      IUiElement element2 = this.buildButton(builder, text, iconPath, color, false);
      this.m_button = builder.NewToggleBtn("LegendItem", parent).SetGameObjectWhenOn(element2).SetGameObjectWhenOff(element1).SetButtonStyleWhenOn(builder.Style.Layers.ItemBtnStyle).SetButtonStyleWhenOff(builder.Style.Layers.ItemBtnStyle).SetOnToggleAction((Action<bool>) (x => onClick(layersLegendItemView))).SetIsOn(isOn);
    }

    private IUiElement buildButton(
      UiBuilder builder,
      LocStrFormatted text,
      string iconPath,
      ColorRgba? color,
      bool enabled)
    {
      UiStyle style = builder.Style;
      Panel parent = builder.NewPanel("Menu item");
      builder.NewIconContainer("Icon").SetIcon(iconPath, enabled ? new ColorRgba(16777215).SetA((byte) 125) : new ColorRgba(16777215)).PutToLeftMiddleOf<IconContainer>((IUiElement) parent, style.Layers.IconSize, Offset.Left(style.Panel.Padding));
      builder.NewTxt(nameof (text)).SetText(text.Value).SetTextStyle(enabled ? style.Layers.ShowLabelTextStyle : style.Layers.HideLabelTextStyle).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent, Offset.Left(2f * style.Panel.Padding + style.Layers.IconSize.x) + Offset.Right(30f));
      if (!enabled && color.HasValue)
        builder.NewPanel("Color").SetBackground(color.Value).SetBorderStyle(style.Layers.ColorBoxBorderStyle).PutToRightMiddleOf<Panel>((IUiElement) parent, style.Layers.HideIconStyle.Size, Offset.Right(style.Panel.Padding));
      if (enabled || !color.HasValue)
        builder.NewIconContainer("Icon show").SetIcon(enabled ? style.Layers.ShowIconStyle : style.Layers.HideIconStyle).PutToRightMiddleOf<IconContainer>((IUiElement) parent, style.Layers.HideIconStyle.Size, Offset.Right(style.Panel.Padding));
      return (IUiElement) parent;
    }

    public void Toggle() => this.m_button.Toggle();
  }
}
