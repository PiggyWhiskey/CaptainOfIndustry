// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.SomethingOutOfSomethingView
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
  /// <summary>[text] [icon] / [text] [icon]</summary>
  internal class SomethingOutOfSomethingView : IUiElement, IDynamicSizeElement
  {
    private readonly StackContainer m_container;
    private readonly TextWithIcon m_first;
    private readonly TextWithIcon m_second;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public TextWithIcon First => this.m_first;

    public TextWithIcon Second => this.m_second;

    public event Action<IUiElement> SizeChanged;

    public SomethingOutOfSomethingView(IUiElement parent, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_container = builder.NewStackContainer("UnityCap", parent).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(5f).SetStackingDirection(StackContainer.Direction.LeftToRight);
      this.m_first = new TextWithIcon(builder, (IUiElement) this.m_container);
      this.m_first.AppendTo<TextWithIcon>(this.m_container, new float?(0.0f));
      Txt txt = builder.NewTxt("Delimiter", (IUiElement) this.m_container).SetText("/");
      TextStyle title = builder.Style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(this.m_container, new float?(5f));
      this.m_second = new TextWithIcon(builder, (IUiElement) this.m_container);
      this.m_second.AppendTo<TextWithIcon>(this.m_container, new float?(0.0f));
      this.m_container.SizeChanged += (Action<IUiElement>) (el =>
      {
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      });
    }

    public void AppendElement(IUiElement elementToAppend)
    {
      this.m_container.Append(elementToAppend, new float?(elementToAppend.GetWidth()));
    }
  }
}
