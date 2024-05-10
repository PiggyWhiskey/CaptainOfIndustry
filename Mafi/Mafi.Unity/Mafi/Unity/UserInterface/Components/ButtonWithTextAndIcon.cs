// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ButtonWithTextAndIcon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class ButtonWithTextAndIcon : Btn
  {
    private readonly UiBuilder m_builder;
    private readonly BtnStyle m_style;
    public readonly TextWithIcon TextWithIcon;

    public ButtonWithTextAndIcon(UiBuilder builder, BtnStyle style, string btnName = "button")
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, btnName);
      this.m_builder = builder;
      this.m_style = style;
      this.SetText("");
      this.SetTextAlignment(TextAnchor.MiddleLeft);
      this.TextWithIcon = new TextWithIcon(builder, 16).PutToCenterOf<TextWithIcon>((IUiElement) this, 0.0f, Offset.LeftRight(5f));
      TextWithIcon textWithIcon = this.TextWithIcon;
      ref TextStyle local = ref style.Text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      textWithIcon.SetTextStyle(textStyle);
      this.TextWithIcon.SetColor(style.Text.Color);
      this.SetHeight<ButtonWithTextAndIcon>(this.GetOptimalSize().y);
    }

    public void UpdateWidth()
    {
      this.SetWidth<ButtonWithTextAndIcon>(this.TextWithIcon.GetWidth() + 20f);
    }

    public override Btn SetEnabled(bool isEnabled)
    {
      this.TextWithIcon.SetEnabled(isEnabled, this.m_style.ForegroundClrWhenDisabled ?? this.m_style.Text.Color);
      return base.SetEnabled(isEnabled);
    }
  }
}
