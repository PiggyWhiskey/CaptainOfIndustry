// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Tooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Tooltip
  {
    private readonly UiBuilder m_builder;
    private Tooltip.TooltipImpl m_tooltip;
    private IUiElement m_parent;
    private string m_text;
    private float m_extraOffsetFromBottom;

    public Tooltip(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      object obj;
      if (!builder.ElementsCache.TryGetValue("TooltipImpl", out obj))
      {
        obj = (object) new Tooltip.TooltipImpl(builder, false);
        Tooltip.TooltipImpl tooltipImpl = new Tooltip.TooltipImpl(builder, true);
        builder.ElementsCache.Add("TooltipImpl", obj);
        builder.ElementsCache.Add("TooltipImplError", (object) tooltipImpl);
      }
      this.m_tooltip = (Tooltip.TooltipImpl) obj;
    }

    public Tooltip SetText(string text)
    {
      this.m_text = (text ?? "").Replace("<bc>", "<color=#f0a926><b>");
      this.m_text = this.m_text.Replace("</bc>", "</b></color>");
      return this;
    }

    public Tooltip SetText(LocStrFormatted text) => this.SetText(text.Value);

    public Tooltip SetErrorTextStyle()
    {
      this.m_tooltip?.OnParentMouseLeave();
      this.m_tooltip = (Tooltip.TooltipImpl) this.m_builder.ElementsCache["TooltipImplError"];
      return this;
    }

    public Tooltip SetNormalTextStyle()
    {
      this.m_tooltip?.OnParentMouseLeave();
      this.m_tooltip = (Tooltip.TooltipImpl) this.m_builder.ElementsCache["TooltipImpl"];
      return this;
    }

    public void SetExtraOffsetFromBottom(float offset) => this.m_extraOffsetFromBottom = offset;

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(this.OnParentMouseLeave));
    }

    public void AttachToNoEvents(IUiElement element) => this.m_parent = element;

    internal void OnParentMouseEnter()
    {
      this.m_tooltip.SetText(this.m_text);
      this.m_tooltip.SetExtraOffsetFromBottom(this.m_extraOffsetFromBottom);
      this.m_tooltip.OnParentMouseEnter(this.m_parent);
    }

    internal void OnParentMouseLeave() => this.m_tooltip.OnParentMouseLeave();

    /// <summary>
    /// This class enables to have just 1 go of tooltip and share it with all the instance.
    /// This saves 1k tooltip instances.
    /// </summary>
    private class TooltipImpl : TooltipBase
    {
      private readonly UiBuilder m_builder;
      private readonly Txt m_textView;
      private Option<IconContainer> m_errorIcon;
      private string m_text;

      public TooltipImpl(UiBuilder builder, bool buildAsError)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_text = "";
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_builder = builder;
        Txt txt = builder.NewTxt("text");
        TextStyle text = builder.Style.Global.Text;
        ref TextStyle local = ref text;
        int? nullable = new int?(15);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
        this.m_textView = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().PutTo<Txt>((IUiElement) this.Container, Offset.All(10f));
        if (!buildAsError)
          return;
        this.setErrorTextStyle();
      }

      internal void SetText(string text) => this.m_text = text ?? "";

      private void setErrorTextStyle()
      {
        this.Container.SetBorderStyle(new BorderStyle(this.Builder.Style.Global.DangerClr, 3f));
        if (!this.m_errorIcon.IsNone)
          return;
        this.m_textView.PutTo<Txt>((IUiElement) this.Container, Offset.All(10f) + Offset.Left(28f));
        this.m_errorIcon = (Option<IconContainer>) this.Builder.NewIconContainer("Warning").SetIcon("Assets/Unity/UserInterface/General/Warning128.png", this.Builder.Style.Global.DangerClr).PutToLeftMiddleOf<IconContainer>((IUiElement) this.Container, 18.Vector2(), Offset.Left(10f));
      }

      internal void OnParentMouseEnter(IUiElement parent)
      {
        if (string.IsNullOrEmpty(this.m_text))
          return;
        this.m_textView.SetText(this.m_text);
        Vector2 preferredSize = this.m_textView.GetPreferredSize(300f, float.MaxValue);
        float x = preferredSize.x;
        if (this.m_errorIcon.HasValue)
          x += 28f;
        this.PositionSelf(parent, x + 20f, preferredSize.y + 20f);
      }

      internal void OnParentMouseLeave() => this.onParentMouseLeave();
    }
  }
}
