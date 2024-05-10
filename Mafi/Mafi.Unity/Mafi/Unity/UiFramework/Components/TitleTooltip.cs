// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TitleTooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class TitleTooltip
  {
    private readonly UiBuilder m_builder;
    private readonly Queueue<TitleTooltip.TooltipImpl> m_cache;
    private Option<TitleTooltip.TooltipImpl> m_view;
    private IUiElement m_parent;
    private string m_text;
    private int m_maxWidthOverflow;
    private int m_extraOffsetFromBottom;

    public TitleTooltip(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_maxWidthOverflow = 10;
      this.m_extraOffsetFromBottom = 5;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      object obj;
      if (!builder.ElementsCache.TryGetValue("TitleTooltipsCache", out obj))
      {
        obj = (object) new Queueue<TitleTooltip.TooltipImpl>();
        builder.ElementsCache.Add("TitleTooltipsCache", obj);
      }
      this.m_cache = (Queueue<TitleTooltip.TooltipImpl>) obj;
    }

    public TitleTooltip SetText(LocStrFormatted text)
    {
      this.m_text = text.Value;
      return this;
    }

    public void SetMaxWidthOverflow(int maxOverflow) => this.m_maxWidthOverflow = maxOverflow;

    public void SetExtraOffsetFromBottom(int extraOffsetFromBottom)
    {
      this.m_extraOffsetFromBottom = extraOffsetFromBottom;
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(this.onParentMouseLeave));
    }

    public void Show(IUiElement parent)
    {
      this.m_parent = parent;
      this.OnParentMouseEnter();
    }

    public void Hide() => this.onParentMouseLeave();

    private void OnParentMouseEnter()
    {
      if (this.m_view.IsNone)
        this.m_view = !this.m_cache.IsNotEmpty ? (Option<TitleTooltip.TooltipImpl>) new TitleTooltip.TooltipImpl(this.m_builder) : (Option<TitleTooltip.TooltipImpl>) this.m_cache.Dequeue();
      this.m_view.Value.SetText(this.m_text);
      this.m_view.Value.OnParentMouseEnter(this.m_parent, this.m_maxWidthOverflow, this.m_extraOffsetFromBottom);
    }

    private void onParentMouseLeave()
    {
      if (!this.m_view.HasValue)
        return;
      this.m_view.Value.OnParentMouseLeave();
      this.m_cache.Enqueue(this.m_view.Value);
      this.m_view = (Option<TitleTooltip.TooltipImpl>) Option.None;
    }

    private class TooltipImpl : TooltipBase
    {
      private IUiElement m_parent;
      private readonly UiBuilder m_builder;
      private readonly Txt m_textView;
      private float m_width;
      private float m_height;
      private string m_text;

      public TooltipImpl(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_text = "";
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_builder = builder;
        this.m_textView = builder.NewTxt("text").SetTextStyle(builder.Style.Global.BoldText).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.Container, Offset.All(5f));
        builder.NewIconContainer("Arrow").SetIcon("Assets/Unity/UserInterface/General/ArrowDownFull.svg", ColorRgba.Black).PutToCenterBottomOf<IconContainer>((IUiElement) this.Container, new Vector2(20f, 10f), Offset.Bottom(-10f));
      }

      internal void SetText(string text) => this.m_text = text ?? "";

      internal void OnParentMouseEnter(
        IUiElement parent,
        int maxOverflow,
        int extraOffsetFromBottom)
      {
        this.SetExtraOffsetFromBottom((float) extraOffsetFromBottom);
        this.m_parent = parent;
        if (string.IsNullOrEmpty(this.m_text))
          return;
        this.m_textView.SetText(this.m_text);
        Vector2 preferredSize = this.m_textView.GetPreferredSize((float) ((double) parent.GetWidth() + (double) maxOverflow - 10.0), float.MaxValue);
        float x = preferredSize.x;
        if (!this.Container.IsVisible())
          this.m_builder.RenderUpdate += new Action<GameTime>(this.renderUpdate);
        this.m_width = x + 10f;
        this.m_height = preferredSize.y + 10f;
        this.PositionSelf(parent, this.m_width, this.m_height, true);
      }

      internal void OnParentMouseLeave()
      {
        this.onParentMouseLeave();
        this.m_builder.RenderUpdate -= new Action<GameTime>(this.renderUpdate);
      }

      private void renderUpdate(GameTime time)
      {
        Assert.That<bool>(this.Container.IsVisible()).IsTrue();
        Vector3 corner1 = this.m_corners[1];
        this.m_parent.RectTransform.GetWorldCorners(this.m_corners);
        Vector3 corner2 = this.m_corners[1];
        if (corner1.x.IsNear(corner2.x) && corner1.y.IsNear(corner2.y))
          return;
        this.PositionSelf(this.m_parent, this.m_width, this.m_height, true);
      }
    }
  }
}
