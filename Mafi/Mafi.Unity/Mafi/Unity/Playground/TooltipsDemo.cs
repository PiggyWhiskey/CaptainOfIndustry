// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.TooltipsDemo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class TooltipsDemo
  {
    public readonly UiComponent Content;

    public TooltipsDemo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      TooltipsDemo tooltipsDemo = this;
      ColorRgba gold = (ColorRgba) 11899183;
      ScrollColumn component = new ScrollColumn();
      component.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.AlignItemsStretch<ScrollColumn>()));
      Row row = new Row();
      row.Add<Row>((Action<Row>) (c => c.AlignItemsStretch<Row>().Fill<Row>()));
      Column column1 = new Column();
      column1.Add<Column>((Action<Column>) (c => c.FlexGrow<Column>(1f).JustifyItemsSpaceBetween<Column>()));
      Column column2 = new Column();
      column2.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Size<Column>(100.px(), 50.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(20)))));
      column1.Add((UiComponent) column2);
      Column column3 = new Column();
      column3.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Size<Column>(100.px(), 50.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(30)))));
      column1.Add((UiComponent) column3);
      row.Add((UiComponent) column1);
      Column column4 = new Column();
      column4.Add<Column>((Action<Column>) (c => c.FlexGrow<Column>(1f).Gap<Column>(new Px?(200.px())).AlignItemsStretch<Column>()));
      Column column5 = new Column();
      column5.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Height<Column>(new Px?(300.px())).MarginTop<Column>(200.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(40)))));
      column4.Add((UiComponent) column5);
      Column column6 = new Column();
      column6.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).AlignItemsCenter<Column>().Gap<Column>(new Px?(40.px())).Padding<Column>(2.pt())));
      Label label1 = new Label();
      label1.Add<Label>((Action<Label>) (c => c.Background<Label>(new ColorRgba?(ColorRgba.Black)).Size<Label>(40.px(), 50.px())));
      Label comp1 = label1;
      column6.Add((UiComponent) label1);
      Label label2 = new Label();
      label2.Add<Label>((Action<Label>) (c => c.Background<Label>(new ColorRgba?(ColorRgba.Black)).Size<Label>(80.px(), 50.px())));
      Label comp2 = label2;
      column6.Add((UiComponent) label2);
      Label label3 = new Label();
      label3.Add<Label>((Action<Label>) (c => c.Background<Label>(new ColorRgba?(ColorRgba.Black)).AlignSelf<Label>(Align.Stretch).Height<Label>(new Px?(50.px()))));
      Label comp3 = label3;
      column6.Add((UiComponent) label3);
      column4.Add((UiComponent) column6);
      Column column7 = new Column();
      column7.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Height<Column>(new Px?(300.px())).MarginBottom<Column>(200.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(20)))));
      column4.Add((UiComponent) column7);
      row.Add((UiComponent) column4);
      Column column8 = new Column();
      column8.Add<Column>((Action<Column>) (c => c.FlexGrow<Column>(1f).JustifyItemsSpaceBetween<Column>().AlignItemsEnd<Column>()));
      Column column9 = new Column();
      column9.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Size<Column>(100.px(), 50.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(20)))));
      column8.Add((UiComponent) column9);
      Column column10 = new Column();
      column10.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(gold)).Size<Column>(100.px(), 50.px()).Tooltip<Column>(new LocStrFormatted?(tooltipsDemo.createStr(30)))));
      column8.Add((UiComponent) column10);
      row.Add((UiComponent) column8);
      component.Add((UiComponent) row);
      this.Content = (UiComponent) component;
      this.setTitleTooltip((UiComponent) comp1);
      this.setTitleTooltip((UiComponent) comp2);
      this.setTitleTooltip((UiComponent) comp3);
    }

    private LocStrFormatted createStr(int repeats)
    {
      return ((IEnumerable<string>) "I'm a tooltip!".Repeat<string>(repeats)).JoinStrings(" ").AsLoc();
    }

    private void setTitleTooltip(UiComponent comp)
    {
      TitleTooltipPromise titleTooltipPromise = new TitleTooltipPromise(comp, "This is a title tooltip!".AsLoc());
    }
  }
}
