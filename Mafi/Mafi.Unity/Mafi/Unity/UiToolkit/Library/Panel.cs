// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Panel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Panel : Column, IComponentWithPadding
  {
    /// <summary>Container of panel's title, if given in the constructor</summary>
    public Option<Row> Header { get; protected set; }

    /// <summary>Container of the panel's contents</summary>
    public Column Body { get; protected set; }

    public Panel(LocStrFormatted? title = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<Panel>(Cls.panel);
      this.Element.AddToClassList(Cls.panel);
      if (title.HasValue)
      {
        this.Element.AddToClassList(Cls.tabbed);
        this.Add((UiComponent) new TabBarBackground());
      }
      Outer panel = Outer.Panel;
      Px? gap = new Px?();
      Column column;
      this.Add((UiComponent) (column = new Column(panel, gap: gap).Fill<Column>().AlignItemsStretch<Column>().Class<Column>(Cls.body).Name<Column>(nameof (Body))));
      if (title.HasValue)
      {
        Row component = new Row();
        component.Add<Row>((Action<Row>) (c => c.Class<Row>(Cls.tabContainer).Name<Row>(nameof (Header))));
        Row row = new Row(Outer.PanelTab);
        row.Add<Row>((Action<Row>) (c => c.WrapClass(Cls.selected).Class<UiComponentDecorated<VisualElement>>(Cls.body).Fill<UiComponentDecorated<VisualElement>>()));
        row.Add((UiComponent) new Label(title.Value).UpperCase());
        component.Add((UiComponent) row);
        this.Header = (Option<Row>) component;
        this.Add((UiComponent) this.Header.Value);
      }
      this.Body = column;
    }

    public Panel SetBoltsVisible(bool show = true)
    {
      this.Element.EnableInClassList(Cls.noBolts, !show);
      return this;
    }

    public Panel RootPanel(bool isRoot = true)
    {
      this.Element.EnableInClassList(Cls.window__rootPanel, isRoot);
      return this;
    }

    internal override VisualElement GetChildrenContainer()
    {
      return this.Body?.GetChildrenContainer() ?? base.GetChildrenContainer();
    }

    IPaddingDecorator IComponentWithPadding.GetPaddingDecorator()
    {
      return this.Body.GetPaddingDecorator();
    }
  }
}
