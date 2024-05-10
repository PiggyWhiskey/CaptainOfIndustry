// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.Demo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class Demo
  {
    public readonly UiComponent Content;

    public Demo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Row component = new Row();
      component.Add<Row>((Action<Row>) (c => c.Background<Row>(new ColorRgba?(ColorRgba.LightGray)).Gap<Row>(new Px?(4.pt()), new Px?(4.pt())).AlignItemsStart<Row>().JustifyItemsStart<Row>().AlignGridContent<Row>(Align.Center).Size<Row>(360.px(), 200.px()).Wrap<Row>()));
      Column column1 = new Column();
      column1.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column1);
      Column column2 = new Column();
      column2.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column2);
      Column column3 = new Column();
      column3.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column3);
      Column column4 = new Column();
      column4.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column4);
      Column column5 = new Column();
      column5.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column5);
      Column column6 = new Column();
      column6.Add<Column>((Action<Column>) (c => c.Background<Column>(new ColorRgba?(ColorRgba.Gold)).Width<Column>(new Px?(100.px())).Height<Column>(new Px?(50.px()))));
      component.Add((UiComponent) column6);
      this.Content = (UiComponent) component;
    }
  }
}
