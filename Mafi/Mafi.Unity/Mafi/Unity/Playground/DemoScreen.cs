// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.DemoScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.MapEditor;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class DemoScreen : Row
  {
    public readonly ObjEditorsDock LeftDock;
    public readonly ObjEditorsDock RightDock;

    public DemoScreen()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AbsolutePositionFillParent<DemoScreen>().IgnoreInputPicking<DemoScreen>().AlignItemsStretch<DemoScreen>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      Column component1 = new Column();
      component1.Add<Column>((Action<Column>) (c =>
      {
        Column component2 = c;
        Px? nullable1 = new Px?(0.px());
        Px? top = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? nullable2 = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? right = new Px?();
        Px? bottom = nullable2;
        Px? left = nullable1;
        component2.AbsolutePosition<Column>(top, right, bottom, left).AlignItemsStretch<Column>().IgnoreInputPicking<Column>();
      }));
      component1.Add((UiComponent) (this.LeftDock = new ObjEditorsDock(true)));
      uiComponentArray[0] = (UiComponent) component1;
      Column component3 = new Column();
      component3.Add<Column>((Action<Column>) (c =>
      {
        Column component4 = c;
        Px? nullable = new Px?(0.px());
        Px? top = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? right = nullable;
        Px? bottom = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? left = new Px?();
        component4.AbsolutePosition<Column>(top, right, bottom, left).AlignItemsStretch<Column>().IgnoreInputPicking<Column>();
      }));
      component3.Add((UiComponent) (this.RightDock = new ObjEditorsDock(false)));
      uiComponentArray[1] = (UiComponent) component3;
      this.Add(uiComponentArray);
      this.LeftDock.AddDockingReference(this.RightDock, "Assets/Unity/UserInterface/General/DoubleArrowsRight.svg", "Dock right".AsLoc());
      this.RightDock.AddDockingReference(this.LeftDock, "Assets/Unity/UserInterface/General/DoubleArrowsLeft.svg", "Dock left".AsLoc());
    }
  }
}
