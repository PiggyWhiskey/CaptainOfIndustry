// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.DualBar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class DualBar : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly Txt m_text;
    private readonly Panel m_barContainer;
    private readonly Panel m_barInner;
    private readonly Panel m_barOuter;

    public GameObject GameObject => this.m_barContainer.GameObject;

    public RectTransform RectTransform => this.m_barContainer.RectTransform;

    public DualBar(UiBuilder builder, IUiElement parent)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_barContainer = builder.NewPanel("Bar", parent).SetBackground(style.QuantityBar.BackgroundColor);
      this.m_barOuter = builder.NewPanel("OuterBar", (IUiElement) this.m_barContainer).SetBackground(style.QuantityBar.BarColor).PutToLeftOf<Panel>((IUiElement) this.m_barContainer, 0.0f);
      this.m_barInner = builder.NewPanel("InnerBar", (IUiElement) this.m_barContainer).SetBackground(style.QuantityBar.BarColor).PutToLeftOf<Panel>((IUiElement) this.m_barContainer, 0.0f);
      this.m_text = builder.NewTxt("Text", (IUiElement) this.m_barContainer).SetText("0").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.QuantityBar.Text).PutTo<Txt>((IUiElement) this.m_barContainer, Offset.Right(5f));
      this.m_barContainer.SetBorderStyle(style.Global.DefaultDarkBorder);
    }

    public void AlignTextToLeft(Offset offset)
    {
      this.m_text.SetAlignment(TextAnchor.MiddleLeft);
      this.m_text.PutTo<Txt>((IUiElement) this.m_barContainer, offset);
    }

    public void UpdateValues(Percent percentInner, Percent percentOuter, string str)
    {
      this.m_text.SetText(str);
      float width = this.GetWidth();
      this.m_barInner.SetWidth<Panel>(percentInner.Clamp0To100().Apply(width));
      if (percentInner.IsNear(percentOuter, 0.5.Percent()))
        this.m_barOuter.SetWidth<Panel>(0.0f);
      else
        this.m_barOuter.SetWidth<Panel>(percentOuter.Clamp0To100().Apply(width));
    }

    public DualBar SetInnerBarColor(ColorRgba color)
    {
      this.m_barInner.SetBackground(color);
      return this;
    }

    public DualBar SetOuterBarColor(ColorRgba color)
    {
      this.m_barOuter.SetBackground(color);
      return this;
    }
  }
}
