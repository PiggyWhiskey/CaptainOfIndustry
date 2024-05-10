// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.BasePriorityPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  public abstract class BasePriorityPanel : IUiElementWithUpdater, IUiElement
  {
    private readonly Panel m_container;
    private readonly Tooltip m_tooltip;
    private readonly IconContainer m_icon;
    private readonly Dropdwn m_dropdown;
    private Option<Panel> m_optionsPanel;
    private Option<Btn> m_optionsBtn;
    private readonly UiBuilder m_builder;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public BasePriorityPanel(IUiElement parent, UiBuilder builder, int maxSteps, string title)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_container = builder.NewPanel("PriorityPanel", parent).SetBorderStyle(new BorderStyle(ColorRgba.Black)).SetBackground((ColorRgba) 3815994);
      this.m_container.SetHeight<Panel>(70f);
      this.m_container.SetWidth<Panel>(140f);
      Txt topOf = builder.NewTxt("txt", (IUiElement) this.m_container).SetText(title).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_container, 30f, Offset.Left(5f) + Offset.Top(2f));
      this.m_tooltip = builder.AddTooltipForTitle(topOf, "");
      List<string> options = new List<string>();
      for (int index = 0; index <= maxSteps; ++index)
        options.Add(string.Format("P{0}", (object) (index + 1)));
      this.m_icon = builder.NewIconContainer("icon", (IUiElement) this.m_container);
      this.m_icon.SetIcon("Assets/Unity/UserInterface/General/Priority.svg");
      this.m_icon.PutToLeftTopOf<IconContainer>((IUiElement) this.m_container, 16.Vector2(), Offset.Left(5f) + Offset.Top((float) (32 + (Dropdwn.HEIGHT - 16) / 2)));
      this.m_dropdown = builder.NewDropdown("Dropdown", (IUiElement) this.m_container).AddOptions(options).PutToLeftTopOf<Dropdwn>((IUiElement) this.m_container, new Vector2(80f, (float) Dropdwn.HEIGHT), Offset.Left(26f) + Offset.Top(32f));
      // ISSUE: method pointer
      this.m_dropdown.OnValueChange(new Action<int>((object) this, __methodptr(\u003C\u002Ector\u003Eg__onValueChangedInternal\u007C14_0)));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<int>(new Func<int>(this.GetCurrentPriority)).Do((Action<int>) (priority => this.m_dropdown.SetValueWithoutNotify(priority)));
      updaterBuilder.Observe<bool>(new Func<bool>(this.IsPrioritySupported)).Observe<bool>(new Func<bool>(this.IsReadonly)).Do((Action<bool, bool>) ((isSupported, isReadonly) =>
      {
        this.SetVisibility<BasePriorityPanel>(isSupported);
        this.m_dropdown.SetEnabled(!isReadonly);
      }));
      this.Updater = updaterBuilder.Build();
    }

    protected void SetPriorityWithoutNotify(int priority)
    {
      this.m_dropdown.SetValueWithoutNotify(priority);
    }

    protected void AddOptionsPanel(IUiElement content)
    {
      Panel panel = this.m_builder.NewPanel("OptionsPanel", (IUiElement) this.m_container).SetBorderStyle(new BorderStyle(ColorRgba.Black)).SetBackground((ColorRgba) 3815994).Hide<Panel>();
      this.m_optionsPanel = (Option<Panel>) panel;
      panel.SetSize<Panel>(content.GetSize() + 10.Vector2());
      content.PutTo<IUiElement>((IUiElement) panel, Offset.All(5f));
      panel.PutToLeftBottomOf<Panel>((IUiElement) this.m_container, panel.GetSize(), Offset.Bottom((float) (-(double) panel.GetHeight() + 1.0)));
      this.m_optionsBtn = (Option<Btn>) this.m_builder.NewBtn("Options").SetButtonStyle(this.m_builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Working128.png").OnClick(new Action(this.toggleOptionsMenu)).PutToRightMiddleOf<Btn>((IUiElement) this.m_dropdown, 18.Vector2(), Offset.Right(-22f));
    }

    private void toggleOptionsMenu()
    {
      if (this.m_optionsPanel.IsNone)
        return;
      this.m_optionsPanel.Value.SetVisibility<Panel>(!this.m_optionsPanel.Value.IsVisible());
    }

    protected void SetOptionBtnVisibility(bool isVisible)
    {
      Btn valueOrNull1 = this.m_optionsBtn.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.SetVisibility<Btn>(isVisible);
      if (isVisible)
        return;
      Panel valueOrNull2 = this.m_optionsPanel.ValueOrNull;
      if (valueOrNull2 == null)
        return;
      valueOrNull2.Hide<Panel>();
    }

    protected void SetHasOptionsSet(bool hasOptionsSet)
    {
      this.m_optionsBtn.ValueOrNull?.SetButtonStyle(hasOptionsSet ? this.m_builder.Style.Global.IconBtnOrange : this.m_builder.Style.Global.IconBtnWhite);
    }

    protected void AddButton(Btn button)
    {
      button.PutToBottomOf<Btn>((IUiElement) this, button.GetOptimalSize().y, Offset.Bottom(10f) + Offset.LeftRight(7f));
      button.TextBestFitEnabled();
      this.SetHeight<BasePriorityPanel>((float) ((double) this.GetHeight() + (double) button.GetOptimalSize().y + 10.0));
    }

    protected void SetCustomIcon(string iconPath) => this.m_icon.SetIcon(iconPath);

    protected void SetColor(ColorRgba color)
    {
      this.m_dropdown.SetLabelTextColor(color);
      this.m_icon.SetColor(color);
    }

    protected void SetTooltip(LocStrFormatted text)
    {
      this.m_tooltip.SetText(text.Value + " " + Tr.Priority__OrderingExplanation.TranslatedString);
    }

    protected void SetReadonly() => this.m_dropdown.SetEnabled(false);

    protected virtual bool IsReadonly() => false;

    protected abstract int GetCurrentPriority();

    protected abstract bool IsPrioritySupported();

    protected abstract void OnValueChanged(int index);
  }
}
