// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.BalanceInfoView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class BalanceInfoView : IUiElement
  {
    public const int HEIGHT = 30;
    private readonly Panel m_container;
    private readonly Txt m_max;
    private readonly TextWithIcon m_diff;
    private readonly UiBuilder m_builder;
    private readonly Txt m_title;
    private readonly IconContainer m_icon;
    private readonly Tooltip m_tooltip;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public BalanceInfoView(UiBuilder builder, string icon, int textWidth = 38)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_container = builder.NewPanel("NeedInfo");
      this.m_container.SetBackground(builder.Style.Panel.ItemOverlay);
      int size = 20;
      this.m_icon = builder.NewIconContainer("ProductIcon").PutToLeftOf<IconContainer>((IUiElement) this.m_container, (float) size, Offset.Left(10f)).Hide<IconContainer>();
      this.m_title = builder.NewTxt("Title").SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) this.m_container, 200f, Offset.Left((float) (size + 10 + 5)));
      this.m_max = builder.NewTxt("Max").SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).PutToRightOf<Txt>((IUiElement) this.m_container, 65f);
      this.m_max.Hide<Txt>();
      this.m_tooltip = builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.m_container);
      this.m_diff = new TextWithIcon(builder, (IUiElement) this.m_container);
      this.m_diff.Icon.SetIcon(icon);
      this.m_diff.ForceTextWidth(textWidth);
      this.m_diff.PutToRightOf<TextWithIcon>((IUiElement) this.m_container, this.m_diff.GetWidth(), Offset.Right(75f));
    }

    public void SetTransparentBg() => this.m_container.SetBackground(ColorRgba.Empty);

    public void SetTitle(string title) => this.m_title.SetText(title);

    public void SetIcon(string iconPath)
    {
      if (iconPath == "EMPTY")
        this.m_icon.Hide<IconContainer>();
      else
        this.m_icon.SetIcon(iconPath).Show<IconContainer>();
    }

    public void SetTooltip(string tooltip) => this.m_tooltip.SetText(tooltip);

    public void SetValue(string value, string max = null)
    {
      this.m_max.SetVisibility<Txt>(max != null);
      if (max != null)
        this.m_max.SetText("/  " + max);
      this.m_diff.SetPrefixText(value);
    }

    public void SetUnityDiff(Upoints unity, Upoints? max = null)
    {
      this.m_max.SetVisibility<Txt>(max.HasValue);
      if (max.HasValue)
        this.m_max.SetText("/  " + max.Value.FormatForceDigits());
      if (unity.IsNegative)
        this.m_diff.SetPrefixText(unity.FormatForceDigits() ?? "");
      else if (unity.IsPositive)
        this.m_diff.SetPrefixText("+" + unity.FormatForceDigits());
      else
        this.m_diff.SetPrefixText(Upoints.Zero.FormatForceDigits());
    }

    public void SetPositiveClr() => this.m_diff.SetColor(this.m_builder.Style.Global.GreenForDark);

    public void SetNeutralClr() => this.m_diff.SetColor(this.m_builder.Style.Global.Text.Color);

    public void SetWarningClr() => this.m_diff.SetColor(this.m_builder.Style.Global.OrangeText);

    public void SetCriticalClr() => this.m_diff.SetColor(this.m_builder.Style.Global.DangerClr);
  }
}
