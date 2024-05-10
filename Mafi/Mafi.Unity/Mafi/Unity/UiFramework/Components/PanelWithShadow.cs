// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.PanelWithShadow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class PanelWithShadow : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly Panel m_panel;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public PanelWithShadow(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_panel = builder.NewPanel("Panel", (IUiElement) this).PutTo<Panel>((IUiElement) this);
    }

    public PanelWithShadow SetBorderStyle(BorderStyle borderStyle)
    {
      this.m_panel.SetBorderStyle(borderStyle);
      return this;
    }

    public PanelWithShadow SetBackground(Color color)
    {
      this.m_panel.SetBackground(color);
      return this;
    }

    public PanelWithShadow SetBackground(ColorRgba color)
    {
      this.m_panel.SetBackground(color);
      return this;
    }

    public PanelWithShadow SetBackground(Sprite sprite, ColorRgba? color = null, bool isTiled = false)
    {
      this.m_panel.SetBackground(sprite, color, isTiled);
      return this;
    }

    public PanelWithShadow SetBackground(string path, ColorRgba? color = null, bool isTiled = false)
    {
      this.m_panel.SetBackground(path, color, isTiled);
      return this;
    }

    public PanelWithShadow SetBackground(
      SlicedSpriteStyle spriteStyle,
      ColorRgba? color = null,
      bool isTiled = false)
    {
      this.m_panel.SetBackground(spriteStyle, color, isTiled);
      return this;
    }

    public PanelWithShadow AddBottomBorder(ColorRgba color)
    {
      this.m_panel.AddBottomBorder(color);
      return this;
    }

    public PanelWithShadow AddShadowBottom(bool large = false)
    {
      Sprite sharedSprite = this.m_builder.AssetsDb.GetSharedSprite("Assets/Unity/UserInterface/General/ShadowBottom32.png");
      int size = large ? 10 : 5;
      this.m_builder.NewPanel("shadow", (IUiElement) this).SetBackground(sharedSprite, new ColorRgba?(new ColorRgba(0, 110))).PutToBottomOf<Panel>((IUiElement) this, (float) size, Offset.Bottom((float) (-size + 1))).SendToBack<Panel>();
      return this;
    }

    public PanelWithShadow AddShadowRightBottom()
    {
      Sprite sharedSprite = this.m_builder.AssetsDb.GetSharedSprite(this.m_builder.Style.Icons.BtnShadow);
      this.m_builder.NewPanel("Shadow", (IUiElement) this).SetBackground(sharedSprite, new ColorRgba?(new ColorRgba(0, 150))).PutTo<Panel>((IUiElement) this, Offset.BottomRight(-6f, -6f)).SendToBack<Panel>().GameObject.GetComponent<Image>().fillCenter = false;
      return this;
    }

    public PanelWithShadow SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      this.m_panel.SetOnMouseEnterLeaveActions(enterAction, leaveAction);
      return this;
    }

    public PanelWithShadow OnClick(Action onClick)
    {
      this.m_panel.OnClick(onClick);
      return this;
    }

    public PanelWithShadow OnRightClick(Action onClick)
    {
      this.m_panel.OnRightClick(onClick);
      return this;
    }

    public PanelWithShadow OnMouseEnter(Action onMouseEnter)
    {
      this.m_panel.OnMouseEnter(onMouseEnter);
      return this;
    }

    public PanelWithShadow OnMouseLeave(Action onMouseLeave)
    {
      this.m_panel.OnMouseLeave(onMouseLeave);
      return this;
    }
  }
}
