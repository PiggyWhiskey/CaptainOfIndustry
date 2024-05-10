// Decompiled with JetBrains decompiler
// Type: BoltDecoration
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using ut12pZTdSUNA6wM24P;

#nullable disable
public class BoltDecoration : Row
{
  public BoltDecoration()
  {
    xxhJUtQyC9HnIshc6H.OukgcisAbr();
    Px? gap = new Px?();
    // ISSUE: explicit constructor call
    base.\u002Ector(gap: gap);
    this.JustifyItemsCenter<BoltDecoration>().BorderBottom<BoltDecoration>(1, new ColorRgba?(Theme.PrimaryColor)).PaddingBottom<BoltDecoration>(4.pt()).MarginBottom<BoltDecoration>(4.pt()).MarginLeftRight<BoltDecoration>(1.pt()).RelativePosition<BoltDecoration>();
    Img component = new Img("Assets/Unity/UserInterface/Window/HexDetail.png").Width<Img>(new Px?(34.px()));
    gap = new Px?(-28.px());
    Px? top = new Px?();
    Px? right = new Px?();
    Px? bottom = gap;
    Px? left = new Px?();
    this.Add((UiComponent) component.AbsolutePosition<Img>(top, right, bottom, left));
  }
}
