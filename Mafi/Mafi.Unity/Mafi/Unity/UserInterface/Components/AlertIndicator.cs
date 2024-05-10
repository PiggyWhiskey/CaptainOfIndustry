// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.AlertIndicator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class AlertIndicator : IUiElement
  {
    public const int WIDTH = 50;
    protected readonly InspectorContext Context;
    private readonly Panel m_container;
    private readonly IconContainer m_alertIcon;
    protected readonly AlertTooltip AlertTooltip;
    private int m_ticksToNextColorSwitch;
    private ColorRgba m_fromClr;
    private ColorRgba m_toClr;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public AlertIndicator(UiBuilder builder, InspectorContext context, IUiElement parent)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_fromClr = (ColorRgba) 9534805;
      this.m_toClr = (ColorRgba) 16758049;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Context = context;
      this.m_container = builder.NewPanel("Panel", parent).SetBorderStyle(new BorderStyle(ColorRgba.Black)).OnClick(new Action(this.OnClick)).SetBackground((ColorRgba) 3815994);
      this.m_container.SetSize<Panel>(new Vector2(50f, 40f));
      this.m_alertIcon = builder.NewIconContainer("Alert", (IUiElement) this.m_container).SetIcon("Assets/Unity/UserInterface/General/Warning128.png").AddOutline().SetColor((ColorRgba) 13145655).PutToCenterMiddleOf<IconContainer>((IUiElement) this.m_container, 32.Vector2());
      this.AlertTooltip = new AlertTooltip(builder);
      this.AlertTooltip.AttachTo<Panel>((IUiElementWithHover<Panel>) this.m_container);
    }

    public void SetIcon(string iconPath) => this.m_alertIcon.SetIcon(iconPath);

    public void HideTooltipIcon() => this.AlertTooltip.HideTooltipIcon();

    public void SetMessage(string message) => this.AlertTooltip.SetMessage(message);

    protected virtual void OnClick()
    {
    }

    public void RenderUpdate()
    {
      if (!this.m_container.IsVisible())
        return;
      if (this.m_ticksToNextColorSwitch <= 0)
      {
        Swap.Them<ColorRgba>(ref this.m_fromClr, ref this.m_toClr);
        this.m_ticksToNextColorSwitch = 40;
      }
      else
        --this.m_ticksToNextColorSwitch;
      this.m_alertIcon.SetUnityColor(Color.Lerp(this.m_fromClr.ToColor(), this.m_toClr.ToColor(), (float) (40 - this.m_ticksToNextColorSwitch) / 40f));
    }
  }
}
