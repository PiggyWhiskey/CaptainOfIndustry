// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TextWithIcon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>[text] [icon]</summary>
  public class TextWithIcon : IUiElement, IDynamicSizeElement, IUiElementWithHover<TextWithIcon>
  {
    private readonly Panel m_container;
    private readonly IconContainer m_icon;
    private readonly Txt m_prefixText;
    private readonly Txt m_suffixText;
    private Option<IconContainer> m_prefixIcon;
    private Option<IconContainer> m_suffixIcon;
    private readonly UiBuilder m_builder;
    private readonly int m_iconSize;
    private ColorRgba m_color;
    private int m_forcedTextWidth;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IconContainer Icon => this.m_icon;

    public event Action<IUiElement> SizeChanged;

    public TextWithIcon(UiBuilder builder, int iconSize = 20)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(builder, (IUiElement) null, iconSize);
    }

    internal TextWithIcon(UiBuilder builder, IUiElement parent, int iconSize = 20)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_forcedTextWidth = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_iconSize = iconSize;
      this.m_container = builder.NewPanel(nameof (TextWithIcon), parent);
      this.m_color = builder.Style.Global.Text.Color;
      this.m_prefixText = builder.NewTxt("Text", (IUiElement) this.m_container).SetText("").SetTextStyle(builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) this.m_container, 0.0f);
      this.m_icon = builder.NewIconContainer(nameof (Icon), (IUiElement) this.m_container).PutToLeftOf<IconContainer>((IUiElement) this.m_container, (float) this.m_iconSize);
      this.m_suffixText = builder.NewTxt("Text", (IUiElement) this.m_container).SetText("").SetTextStyle(builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutToRightOf<Txt>((IUiElement) this.m_container, 0.0f);
      this.updateWidth();
    }

    public void ForceTextWidth(int width)
    {
      this.m_forcedTextWidth = width;
      this.updateWidth(true);
    }

    public TextWithIcon SetIcon(string path)
    {
      this.Icon.SetIcon(path);
      return this;
    }

    public TextWithIcon SetPrefixText(string text)
    {
      this.m_prefixText.SetText(text);
      this.updateWidth(true);
      return this;
    }

    public TextWithIcon SetSuffixText(string text)
    {
      this.m_suffixText.SetText(text);
      this.updateWidth(true);
      return this;
    }

    public TextWithIcon SetTextStyle(TextStyle textStyle)
    {
      this.m_prefixText.SetTextStyle(textStyle);
      this.m_suffixText.SetTextStyle(textStyle);
      this.updateWidth(true);
      return this;
    }

    public TextWithIcon EnableRichText()
    {
      this.m_prefixText.EnableRichText();
      this.m_suffixText.EnableRichText();
      return this;
    }

    public TextWithIcon SetColor(ColorRgba color)
    {
      this.m_color = color;
      this.setColorInternal(color);
      return this;
    }

    public void SetEnabled(bool isEnabled, ColorRgba colorWhenDisabled)
    {
      if (isEnabled)
        this.setColorInternal(this.m_color);
      else
        this.setColorInternal(colorWhenDisabled);
    }

    public Panel AsPanel() => this.m_container;

    private void setColorInternal(ColorRgba color)
    {
      this.m_icon.SetColor(color);
      this.m_suffixIcon.ValueOrNull?.SetColor(color);
      this.m_prefixText.SetColor(color);
      this.m_suffixText.SetColor(color);
    }

    public TextWithIcon SetSuffixIcon(string iconPath)
    {
      if (this.m_suffixIcon.IsNone)
        this.m_suffixIcon = (Option<IconContainer>) this.m_builder.NewIconContainer("Icon").PutToRightOf<IconContainer>((IUiElement) this.m_container, (float) this.m_iconSize);
      this.m_suffixIcon.Value.SetIcon(iconPath, this.m_color);
      this.updateWidth(true);
      return this;
    }

    public TextWithIcon SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      this.m_container.SetOnMouseEnterLeaveActions(enterAction, leaveAction);
      return this;
    }

    private void updateWidth(bool sendUpdate = false)
    {
      float width = this.m_prefixText.GetPreferedWidth();
      float preferedWidth = this.m_suffixText.GetPreferedWidth();
      if (this.m_forcedTextWidth != -1)
        width = (float) this.m_forcedTextWidth;
      this.m_prefixText.SetWidth<Txt>(width);
      this.m_suffixText.SetWidth<Txt>(preferedWidth);
      int num1 = width.IsNearZero() ? 0 : 3;
      int num2 = preferedWidth.IsNearZero() ? 0 : 3;
      int rightOffset = 0;
      if (this.m_suffixIcon.HasValue && (double) preferedWidth > 0.0)
      {
        rightOffset = 3 + this.m_iconSize;
        this.m_suffixText.PutToRightOf<Txt>((IUiElement) this.m_container, preferedWidth, Offset.Right((float) rightOffset));
      }
      this.SetWidth<TextWithIcon>(width + preferedWidth + (float) this.m_iconSize + (float) num1 + (float) num2 + (float) rightOffset);
      this.m_icon.PutToLeftOf<IconContainer>((IUiElement) this.m_container, (float) this.m_iconSize, Offset.Left(width + (float) num1));
      if (!sendUpdate)
        return;
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }
  }
}
