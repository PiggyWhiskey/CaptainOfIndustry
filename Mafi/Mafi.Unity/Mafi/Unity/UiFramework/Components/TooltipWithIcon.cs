// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TooltipWithIcon
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
  public class TooltipWithIcon : TooltipBase
  {
    public readonly TextWithIcon TextWithIcon;
    private IUiElement m_parent;

    public TooltipWithIcon(UiBuilder builder, int iconSize = 20)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      TextWithIcon textWithIcon = new TextWithIcon(builder, iconSize);
      TextStyle text = builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.TextWithIcon = textWithIcon.SetTextStyle(textStyle).PutToLeftTopOf<TextWithIcon>((IUiElement) this.Container, new Vector2(0.0f, 24f), Offset.All(10f));
    }

    private void OnParentMouseEnter()
    {
      float height = this.TextWithIcon.GetHeight() + 20f;
      this.PositionSelf(this.m_parent, this.TextWithIcon.GetWidth(), height);
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(((TooltipBase) this).onParentMouseLeave));
    }
  }
}
