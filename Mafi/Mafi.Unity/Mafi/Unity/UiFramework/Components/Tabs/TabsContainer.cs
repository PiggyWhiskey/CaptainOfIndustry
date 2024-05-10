// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Tabs.TabsContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Tabs
{
  /// <summary>Displays tabs</summary>
  public class TabsContainer : IUiElement
  {
    private int m_tabsTotal;
    private Option<Tab> m_tabToSwitchTo;
    private bool m_syncBeforeSwitchDone;
    private readonly UiBuilder m_builder;
    private readonly int m_width;
    private readonly int m_height;
    private readonly Panel m_container;
    private readonly StackContainer m_buttonsContainer;
    private readonly ScrollableContainer m_scrollableContainer;
    private readonly AudioSource m_tabSwitchSound;
    private readonly Dict<Tab, ToggleBtn> m_buttonsMap;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public Option<Tab> ActiveTab { get; private set; }

    public TabsContainer(UiBuilder builder, int width, int height, IUiElement parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_buttonsMap = new Dict<Tab, ToggleBtn>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_width = width;
      this.m_height = height;
      this.m_container = builder.NewPanel(nameof (TabsContainer), parent);
      this.m_scrollableContainer = this.m_builder.NewScrollableContainer("ScrollableContainer", (IUiElement) this.m_container).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.m_container, Offset.Top(40f));
      Panel topOf = builder.NewPanel("Tabs header", (IUiElement) this.m_container).SetBackground((ColorRgba) 3684408).PutToTopOf<Panel>((IUiElement) this.m_container, 40f);
      builder.NewPanel("Bottom border", (IUiElement) this.m_container).SetBackground((ColorRgba) 2368548).PutToTopOf<Panel>((IUiElement) this.m_container, 1f, Offset.Top(40f));
      this.m_buttonsContainer = builder.NewStackContainer("Buttons", (IUiElement) topOf).SetBackground((ColorRgba) 0).SetItemSpacing(1f).SetInnerPadding(Offset.Right(1f)).PutToLeftOf<StackContainer>((IUiElement) topOf, 0.0f);
      this.m_tabSwitchSound = builder.AudioDb.GetSharedAudio(builder.Audio.TabSwitch);
    }

    public TabsContainer AddTab(LocStrFormatted title, IconStyle? icon, Tab tab, bool hidden = false)
    {
      return this.AddTab(title.Value, icon, tab, hidden);
    }

    public TabsContainer AddTab(string title, IconStyle? icon, Tab tab, bool hidden = false)
    {
      Assert.That<Panel>(this.m_container).IsNotNull<Panel>("You must build the view before adding any tabs.");
      tab.AvailableWidth = this.m_width;
      tab.ViewportHeight = this.m_height - 40 - this.m_builder.Style.Panel.HeaderHeight;
      tab.BuildUi(this.m_builder, (IUiElement) this.m_scrollableContainer.Viewport);
      ++this.m_tabsTotal;
      Pair<Panel, float> pair1 = this.buildTabHeader(title, icon, true);
      Pair<Panel, float> pair2 = this.buildTabHeader(title, icon, false);
      ToggleBtn toggleBtn = this.m_builder.NewToggleBtn("Button tab " + this.m_tabsTotal.ToString(), (IUiElement) this.m_buttonsContainer).SetGameObjectWhenOn((IUiElement) pair1.First).SetGameObjectWhenOff((IUiElement) pair2.First, pair2.First.GameObject.GetComponent<Graphic>()).SetOnToggleAction((Action<bool>) (x => this.SwitchToTab(tab)), this.m_tabSwitchSound).AppendTo<ToggleBtn>(this.m_buttonsContainer, new float?(pair1.Second));
      this.m_buttonsContainer.SetItemVisibility((IUiElement) toggleBtn, !hidden);
      this.m_buttonsMap.Add(tab, toggleBtn);
      this.m_scrollableContainer.AddItemTop((IUiElement) tab, false);
      if (this.ActiveTab.IsNone)
      {
        this.ActiveTab = Option.Some<Tab>(tab);
        this.m_scrollableContainer.SetContentToScroll((IUiElement) this.ActiveTab.Value);
        toggleBtn.Toggle();
        tab.Show();
      }
      else
        tab.Hide();
      return this;
    }

    public void SetTabVisibility(Tab tab, bool isVisible)
    {
      ToggleBtn toggleBtn;
      if (this.m_buttonsMap.TryGetValue(tab, out toggleBtn))
        this.m_buttonsContainer.SetItemVisibility((IUiElement) toggleBtn, isVisible);
      if (isVisible || !(this.ActiveTab == tab))
        return;
      foreach (KeyValuePair<Tab, ToggleBtn> buttons in this.m_buttonsMap)
      {
        Tab tab1;
        ToggleBtn element;
        buttons.Deconstruct(ref tab1, ref element);
        Tab tab2 = tab1;
        if (element.IsVisible())
        {
          this.SwitchToTab(tab2);
          break;
        }
      }
    }

    private Pair<Panel, float> buildTabHeader(string title, IconStyle? icon, bool activeVersion)
    {
      Panel panel = this.m_builder.NewPanel("Panel").SetBackground((ColorRgba) 4408131);
      ColorRgba colorRgba = activeVersion ? (ColorRgba) 14592068 : this.m_builder.Style.Panel.HeaderText.Color;
      int coord = 18;
      if (icon.HasValue)
        this.m_builder.NewIconContainer("Icon", (IUiElement) panel).SetIcon(icon.Value.Extend(color: new ColorRgba?(colorRgba))).PutToLeftMiddleOf<IconContainer>((IUiElement) panel, coord.Vector2(), Offset.Left(8f));
      float second = (float) ((double) this.m_builder.NewTxt("Title", (IUiElement) panel).SetText(title).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.m_builder.Style.Panel.HeaderText.Extend(new ColorRgba?(colorRgba))).PutTo<Txt>((IUiElement) panel, Offset.Left((float) (8 + (icon.HasValue ? coord + 5 : 0)))).GetPreferedWidth() + 16.0 + (icon.HasValue ? (double) (coord + 5) : 0.0));
      return Pair.Create<Panel, float>(panel, second);
    }

    public void RenderUpdate(GameTime gameTime)
    {
      if (this.ActiveTab.IsNone)
        return;
      if (this.m_tabToSwitchTo.HasValue && this.m_syncBeforeSwitchDone)
      {
        this.m_tabToSwitchTo.Value.RenderUpdate(gameTime);
        if (this.ActiveTab.HasValue)
        {
          this.ActiveTab.Value.Hide();
          this.m_buttonsMap[this.ActiveTab.Value].Toggle();
        }
        this.m_tabToSwitchTo.Value.Show();
        this.m_scrollableContainer.SetContentToScroll((IUiElement) this.m_tabToSwitchTo.Value);
        this.ActiveTab = this.m_tabToSwitchTo;
        this.m_buttonsMap[this.ActiveTab.Value].Toggle();
        this.m_tabToSwitchTo = (Option<Tab>) Option.None;
      }
      else
        this.ActiveTab.Value.RenderUpdate(gameTime);
    }

    public void SyncUpdate(GameTime gameTime)
    {
      if (this.m_tabToSwitchTo.HasValue)
      {
        this.m_tabToSwitchTo.Value.SyncUpdate(gameTime);
        this.m_syncBeforeSwitchDone = true;
      }
      else
      {
        if (!this.ActiveTab.HasValue)
          return;
        this.ActiveTab.Value.SyncUpdate(gameTime);
      }
    }

    public void SwitchToTab(Tab tab)
    {
      if (this.ActiveTab == tab)
        return;
      this.m_tabToSwitchTo = Option.Some<Tab>(tab);
      this.m_syncBeforeSwitchDone = false;
    }
  }
}
