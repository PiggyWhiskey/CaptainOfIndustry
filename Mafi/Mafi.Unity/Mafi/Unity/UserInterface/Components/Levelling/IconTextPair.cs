// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.Levelling.IconTextPair
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.Levelling
{
  public class IconTextPair : IUiElement
  {
    private Panel m_container;
    private IconContainer m_icon;
    private Txt m_text;
    private IconTextPair.Style m_style;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IconTextPair(UiBuilder builder, IconTextPair.Style style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_style = style;
      this.m_container = builder.NewPanel("IconTextCont");
      float iconSize = style.IconSize;
      this.m_icon = builder.NewIconContainer("Icon").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_container, new Vector2(iconSize, iconSize));
      this.m_text = builder.NewTxt("Text").AllowHorizontalOverflow().SetTextStyle(style.TextStyle).PutToLeftMiddleOf<Txt>((IUiElement) this.m_container, Vector2.zero, Offset.Left(iconSize + style.IconTextDistance));
    }

    public IconTextPair SetIcon(string iconPath)
    {
      this.m_icon.SetIcon(iconPath);
      return this;
    }

    public IconTextPair SetText(string text)
    {
      this.m_text.SetText(text);
      Vector2 preferedSize = this.m_text.GetPreferedSize();
      this.m_text.SetSize<Txt>(preferedSize);
      this.m_container.SetSize<Panel>(new Vector2(preferedSize.x + this.m_style.IconSize + this.m_style.IconTextDistance, preferedSize.y.Max(this.m_style.IconSize)));
      return this;
    }

    public class Style
    {
      public readonly float IconSize;
      public readonly float IconTextDistance;
      public readonly TextStyle TextStyle;

      public Style(float iconSize, float iconTextDistance, TextStyle textStyle)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconSize = iconSize;
        this.IconTextDistance = iconTextDistance;
        this.TextStyle = textStyle;
      }
    }
  }
}
