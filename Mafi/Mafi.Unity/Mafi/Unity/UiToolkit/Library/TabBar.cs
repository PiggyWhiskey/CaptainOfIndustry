// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TabBar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Linq;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TabBar : Row
  {
    private bool m_enableDropdownMode;
    private bool m_dropdownMode;
    private Option<Dropdown<LocStrFormatted>> m_dropdown;
    private Action<int> m_switchToTab;

    public int? ActiveIndex { get; private set; }

    public Option<TabButton> ActiveTab
    {
      get
      {
        return this.ActiveIndex.HasValue ? this.TabAt(this.ActiveIndex.Value) : (Option<TabButton>) Option.None;
      }
    }

    private int TabCount => !this.m_dropdownMode ? this.Count : this.Count - 1;

    public TabBar(Action<int> switchToTab = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(2.pt());
      this.m_switchToTab = switchToTab;
      this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkForOverflow), TrickleDown.NoTrickleDown);
    }

    /// <summary>Returns the tab button at the given index.</summary>
    public Option<TabButton> TabAt(int index)
    {
      return !(this.ChildAtOrNone(index).ValueOrNull is TabButton valueOrNull) ? (Option<TabButton>) Option.None : (Option<TabButton>) valueOrNull;
    }

    public TabBar EnableDropdownMode(bool enable)
    {
      this.m_enableDropdownMode = enable;
      this.checkForOverflow((GeometryChangedEvent) null);
      return this;
    }

    public void Add(LocStrFormatted title, string iconAsset = null, int? index = null)
    {
      TabButton tab = new TabButton(title, iconAsset);
      tab.OnClick<TabButton>((Action) (() => this.m_switchToTab(this.IndexOfChild((UiComponent) tab).Value)));
      tab.CallbackOnce<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkForOverflow));
      this.InsertAt(index ?? this.TabCount, (UiComponent) tab);
    }

    public void Remove(int index)
    {
      Option<UiComponent> option = this.ChildAtOrNone(index);
      if (option.IsNone)
        return;
      option.Value.RemoveFromHierarchy();
      int num = index;
      int? activeIndex = this.ActiveIndex;
      int valueOrDefault = activeIndex.GetValueOrDefault();
      if (num <= valueOrDefault & activeIndex.HasValue)
        this.Activate(index - 1);
      this.TabAt(0).ValueOrNull?.CallbackOnce<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkForOverflow));
    }

    public TabBar Activate(int index)
    {
      int num = index;
      int? activeIndex = this.ActiveIndex;
      int valueOrDefault = activeIndex.GetValueOrDefault();
      if (num == valueOrDefault & activeIndex.HasValue)
        return this;
      Option<TabButton> activeTab = this.ActiveTab;
      TabButton valueOrNull1 = activeTab.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.Selected<TabButton>(false);
      activeTab = this.ActiveTab;
      activeTab.ValueOrNull?.WrapClass(Cls.selected, false);
      if (index < 0 || index >= this.TabCount)
      {
        this.ActiveIndex = new int?();
      }
      else
      {
        this.ActiveIndex = new int?(index);
        activeTab = this.ActiveTab;
        TabButton valueOrNull2 = activeTab.ValueOrNull;
        if (valueOrNull2 != null)
          valueOrNull2.Selected<TabButton>();
        activeTab = this.ActiveTab;
        activeTab.ValueOrNull?.WrapClass(Cls.selected);
      }
      this.refreshDropdown();
      return this;
    }

    public TabBar Deactivate(int index)
    {
      Button valueOrNull = (Button) this.ChildAtOrNone(index).ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Selected<Button>(false);
      int num = index;
      int? activeIndex = this.ActiveIndex;
      int valueOrDefault = activeIndex.GetValueOrDefault();
      if (num == valueOrDefault & activeIndex.HasValue)
        this.Activate(-1);
      this.refreshDropdown();
      return this;
    }

    private void checkForOverflow(GeometryChangedEvent _1)
    {
      bool flag = this.m_enableDropdownMode && this.AllChildren.Any<UiComponent>((Func<UiComponent, bool>) (c => c is TabButton tabButton && tabButton.IsOverflowing));
      foreach (UiComponent allChild in this.AllChildren)
        allChild.VisibleForRender<UiComponent>(allChild is Dropdown<LocStrFormatted> == flag);
      if (this.m_dropdownMode == flag)
        return;
      this.m_dropdownMode = flag;
      if (this.m_dropdownMode && this.m_dropdown.IsNone)
      {
        this.m_dropdown = (Option<Dropdown<LocStrFormatted>>) new Dropdown<LocStrFormatted>(Outer.PanelTab);
        this.m_dropdown.Value.AbsolutePositionFillParent<Dropdown<LocStrFormatted>>().OnValueChanged((Action<LocStrFormatted, int>) ((_2, idx) => this.m_switchToTab(idx))).InnerDecorationClass(Cls.selected).WrapClass(Cls.panelTab_inputDropdown);
        this.Add((UiComponent) this.m_dropdown.Value);
        this.refreshDropdown();
      }
      else
      {
        if (this.m_dropdownMode || !this.m_dropdown.HasValue)
          return;
        this.m_dropdown.Value.RemoveFromHierarchy();
        this.m_dropdown = (Option<Dropdown<LocStrFormatted>>) Option.None;
      }
    }

    private void refreshDropdown()
    {
      if (!this.m_dropdownMode || !this.m_dropdown.HasValue)
        return;
      this.m_dropdown.Value.SetOptions(this.AllChildren.Select<UiComponent, LocStrFormatted>((Func<UiComponent, LocStrFormatted>) (c => !(c is TabButton tabButton) ? (LocStrFormatted) LocStr.Empty : tabButton.Title)).Where<LocStrFormatted>((Func<LocStrFormatted, bool>) (tb => tb != (LocStrFormatted) LocStr.Empty))).SetValueIndex(this.ActiveIndex.GetValueOrDefault());
    }
  }
}
