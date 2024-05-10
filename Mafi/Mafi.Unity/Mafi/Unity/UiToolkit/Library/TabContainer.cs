// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TabContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TabContainer : Column, IComponentWithInputUpdate
  {
    public UiComponent Body;
    public readonly ScrollColumn Scroller;
    protected readonly PanelWithTabs m_panel;
    private LystStruct<TabContainer.TabEntry> m_contents;
    private Action m_onTabActivate;

    public int? ActiveTabIndex
    {
      get => this.TabBar.ActiveIndex;
      set
      {
        if (!value.HasValue)
          return;
        Option<UiComponent> option = this.TabAt(value.Value.Clamp(0, this.TabBar.Count));
        if (!option.HasValue)
          return;
        this.SwitchToTab(option.Value);
      }
    }

    private TabContainer.TabEntry? ActiveEntry
    {
      get
      {
        if (!this.ActiveTabIndex.HasValue)
          return new TabContainer.TabEntry?();
        if (this.ActiveTabIndex.Value < this.m_contents.Count)
          return new TabContainer.TabEntry?(this.m_contents[this.ActiveTabIndex.Value]);
        Log.Error("Invalid active tab index!");
        TabBar tabBar = this.TabBar;
        int? activeTabIndex = this.ActiveTabIndex;
        int index1 = activeTabIndex.Value.Min(this.m_contents.Count - 1);
        tabBar.Activate(index1);
        activeTabIndex = this.ActiveTabIndex;
        if (!activeTabIndex.HasValue)
          return new TabContainer.TabEntry?();
        ref LystStruct<TabContainer.TabEntry> local = ref this.m_contents;
        activeTabIndex = this.ActiveTabIndex;
        int index2 = activeTabIndex.Value;
        return new TabContainer.TabEntry?(local[index2]);
      }
    }

    public Option<UiComponent> ActiveTab
    {
      get
      {
        TabContainer.TabEntry? activeEntry = this.ActiveEntry;
        ref TabContainer.TabEntry? local = ref activeEntry;
        UiComponent tabBody = local.HasValue ? local.GetValueOrDefault().TabBody : (UiComponent) null;
        return tabBody == null ? Option<UiComponent>.None : (Option<UiComponent>) tabBody;
      }
    }

    public LocStrFormatted? ActiveTabName => this.TabBar.ActiveTab.ValueOrNull?.Title;

    /// <summary>
    /// Allows to add custom elements to the right side of the tabs.
    /// Useful in case the tab container is used without a window header.
    /// </summary>
    public Row ControlsBar => this.m_panel.ControlsBar;

    public Row Header => this.m_panel.Header.Value;

    public TabBar TabBar => this.m_panel.TabBar;

    public UiComponent InnerBody => (UiComponent) this.m_panel.Body;

    public Panel Panel => (Panel) this.m_panel;

    protected int TabsCount => this.TabBar.Count;

    public TabContainer(bool showControlsBar = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name<TabContainer>(nameof (TabContainer)).AlignItemsStretch<TabContainer>().Fill<TabContainer>();
      this.Add((UiComponent) (this.m_panel = new PanelWithTabs(new Action<int>(this.SwitchToTab), showControlsBar).Fill<PanelWithTabs>()));
      this.m_panel.Body.Add(this.Body = (UiComponent) (this.Scroller = new ScrollColumn().Fill<ScrollColumn>()));
    }

    public void Add(LocStrFormatted title, UiComponent content, Scroll scroll = Scroll.Yes)
    {
      this.AddTab(title, content, scroll: scroll == Scroll.Yes);
    }

    public void Add(
      LocStrFormatted title,
      string iconAssetPath,
      UiComponent content,
      Scroll scroll = Scroll.Yes)
    {
      this.AddTab(title, content, iconAssetPath, scroll: scroll == Scroll.Yes);
    }

    /// <summary>Returns the tab component at the given index.</summary>
    public Option<UiComponent> TabAt(int index)
    {
      return index < 0 || index >= this.m_contents.Count ? Option<UiComponent>.None : (Option<UiComponent>) this.m_contents[index].TabBody;
    }

    /// <summary>
    /// "Root" tab containers have less internal padding than nested ones.
    /// </summary>
    /// <param name="isRoot">True to mark this tab container as a root</param>
    public TabContainer RootPanel(bool isRoot = true)
    {
      this.m_panel.RootPanel(isRoot);
      return this;
    }

    public TabContainer OnTabActivate(Action onActivate)
    {
      this.m_onTabActivate = onActivate;
      return this;
    }

    /// <summary>
    /// Adds the given tab or switches to it if nothing is selected or if <paramref name="switchTo" /> is <c>true</c>.
    /// </summary>
    public void AddTab(
      LocStrFormatted title,
      UiComponent content,
      string iconAssetPath = null,
      bool switchTo = false,
      bool scroll = true,
      int? index = null)
    {
      if (this.m_contents.IndexOf<UiComponent>(content, new Func<TabContainer.TabEntry, UiComponent>(this.getTab)) < 0)
      {
        this.TabBar.Add(title, iconAssetPath, index);
        if (index.HasValue)
          this.m_contents.Insert(index.Value, new TabContainer.TabEntry(content, scroll));
        else
          this.m_contents.Add(new TabContainer.TabEntry(content, scroll));
      }
      if (!(this.ActiveTab.IsNone | switchTo))
        return;
      this.SwitchToTab(content);
    }

    public bool TryRemoveTab(int index)
    {
      return index >= 0 && index < this.m_contents.Count && this.TryRemoveTab(this.m_contents[index].TabBody);
    }

    public bool TryRemoveTab(UiComponent content)
    {
      int index = this.m_contents.IndexOf<UiComponent>(content, new Func<TabContainer.TabEntry, UiComponent>(this.getTab));
      if (index < 0)
        return false;
      if (this.ActiveTab == content)
        this.unselectCurrentTab();
      this.TabBar.Remove(index);
      this.m_contents.RemoveAt(index);
      this.OnTabRemoved();
      this.Body.MinWidth<UiComponent>(new Px?());
      if (this.m_contents.IsNotEmpty)
        this.SwitchToTab(this.m_contents.First.TabBody);
      return true;
    }

    public void RemoveTab(UiComponent content)
    {
      if (this.TryRemoveTab(content))
        return;
      Log.Error("Content does not exist");
    }

    public bool ContainsTab(UiComponent content)
    {
      return this.m_contents.IndexOf<UiComponent>(content, new Func<TabContainer.TabEntry, UiComponent>(this.getTab)) >= 0;
    }

    public IEnumerable<UiComponent> GetAllTabs()
    {
      return this.m_contents.AsEnumerable().Select<TabContainer.TabEntry, UiComponent>((Func<TabContainer.TabEntry, UiComponent>) (te => te.TabBody));
    }

    public void SwitchToTab(int index)
    {
      if (index < 0 || index >= this.m_contents.Count)
        Log.Error("Header does not exist");
      else
        this.SwitchToTab(this.m_contents[index].TabBody);
    }

    private UiComponent getTab(TabContainer.TabEntry entry) => entry.TabBody;

    public void SwitchToTab(UiComponent tab)
    {
      TabContainer.TabEntry tabEntry = this.m_contents.FirstOrDefault((Func<TabContainer.TabEntry, bool>) (te => te.TabBody == tab));
      if (tabEntry.IsEmpty)
      {
        Log.Warning("Attempt to switch to non-existent tab");
      }
      else
      {
        if (tab != this.ActiveTab)
        {
          this.unselectCurrentTab();
          if (this.Body == this.Scroller && !tabEntry.ShowScrollbar)
          {
            this.Body = (UiComponent) this.m_panel.Body;
            this.Body.Clear();
          }
          else if (this.Body != this.Scroller && tabEntry.ShowScrollbar)
          {
            this.Body.Clear();
            this.Body.Add((UiComponent) this.Scroller);
            this.Body = (UiComponent) this.Scroller;
            this.Scroller.Clear();
          }
          if (tab is ITab tab1)
            tab1.Activate();
          this.Body.Add(tab);
        }
        this.TabBar.Activate(this.m_contents.IndexOf(tabEntry));
        this.OnTabSwitch();
        Action onTabActivate = this.m_onTabActivate;
        if (onTabActivate == null)
          return;
        onTabActivate();
      }
    }

    public int IndexOfTab(UiComponent tab)
    {
      return this.m_contents.IndexOf<UiComponent>(tab, (Func<TabContainer.TabEntry, UiComponent>) (te => te.TabBody));
    }

    public int IndexOfTab(Func<UiComponent, bool> predicate)
    {
      TabContainer.TabEntry tabEntry = this.m_contents.FirstOrDefault((Func<TabContainer.TabEntry, bool>) (te => predicate(te.TabBody)));
      return !tabEntry.IsEmpty ? this.m_contents.IndexOf(tabEntry) : -1;
    }

    public bool InputUpdate()
    {
      return this.ActiveTab.ValueOrNull is IComponentWithInputUpdate valueOrNull && valueOrNull.InputUpdate();
    }

    protected void unselectCurrentTab()
    {
      int? activeTabIndex = this.ActiveTabIndex;
      if (!activeTabIndex.HasValue)
        return;
      UiComponent uiComponent = this.ActiveTab.Value;
      uiComponent.RemoveFromHierarchy();
      this.TabBar.Deactivate(activeTabIndex.Value);
      if (!(uiComponent is ITab tab))
        return;
      tab.Deactivate();
    }

    protected virtual void OnTabSwitch()
    {
    }

    protected virtual void OnTabRemoved()
    {
    }

    private readonly struct TabEntry
    {
      public readonly UiComponent TabBody;
      public readonly bool ShowScrollbar;

      public bool IsEmpty => this.TabBody == null;

      public TabEntry(UiComponent tabBody, bool showScrollbar)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.TabBody = tabBody;
        this.ShowScrollbar = showScrollbar;
      }
    }
  }
}
