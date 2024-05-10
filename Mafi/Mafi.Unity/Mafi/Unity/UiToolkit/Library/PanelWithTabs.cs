// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.PanelWithTabs
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>
  /// This is a visual-only Panel that includes a tab header bar. Use a TabContainer instead if you wish
  /// to add tabs and manage tab switching and scrolling.
  /// </summary>
  public class PanelWithTabs : Panel
  {
    private readonly Action<int> m_onTabActivate;
    /// <summary>Container for tab buttons</summary>
    public readonly TabBar TabBar;
    /// <summary>Container for controls shown inline with tabs</summary>
    public readonly Row ControlsBar;

    public PanelWithTabs(Action<int> onTabActivate, bool showControlsBar = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onTabActivate = onTabActivate;
      Column body = this.Body;
      this.Body = (Column) null;
      this.Element.AddToClassList(Cls.tabbed);
      this.InsertAt(0, (UiComponent) new TabBarBackground());
      if (showControlsBar)
      {
        Row component = new Row(2.pt());
        component.Add<Row>((Action<Row>) (c => c.Class<Row>(Cls.tabContainer).Name<Row>("Header")));
        component.Add((UiComponent) (this.TabBar = new TabBar(new Action<int>(this.handleTabActivate)).Fill<TabBar>().Name<TabBar>(nameof (TabBar))));
        component.Add((UiComponent) (this.ControlsBar = new Row().Name<Row>(nameof (ControlsBar)).FlexNoShrink<Row>()));
        this.Header = (Option<Row>) component;
      }
      else
        this.Header = (Option<Row>) (Row) (this.TabBar = new TabBar(new Action<int>(this.handleTabActivate)).Class<TabBar>(Cls.tabContainer).Fill<TabBar>().Name<TabBar>(nameof (TabBar)));
      this.Add((UiComponent) this.Header.Value);
      this.Body = body;
    }

    private void handleTabActivate(int index)
    {
      Action<int> onTabActivate = this.m_onTabActivate;
      if (onTabActivate == null)
        return;
      onTabActivate(index);
    }
  }
}
