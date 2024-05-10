// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ScrollRow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ScrollRow : ScrollBase
  {
    public ScrollRow()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(ScrollViewMode.Horizontal);
      this.Element.verticalScrollerVisibility = ScrollerVisibility.Hidden;
    }

    public ScrollRow ScrollTo(float pos)
    {
      this.Element.scrollOffset = new Vector2(pos, 0.0f);
      return this;
    }
  }
}
