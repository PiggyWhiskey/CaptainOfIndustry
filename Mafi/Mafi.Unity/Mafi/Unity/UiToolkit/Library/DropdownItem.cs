// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.DropdownItem
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
  public class DropdownItem : ButtonRow
  {
    public DropdownItem(UiComponent item, Action onClick, bool hideCheckmark = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<DropdownItem>(Cls.inputDropdown_option).ClassIff<DropdownItem>(Cls.noCheck, hideCheckmark).OnClick<DropdownItem>(onClick);
      UiComponent[] uiComponentArray = new UiComponent[3];
      UiComponent component = new UiComponent().Class<UiComponent>(Cls.inputDropdown_option_line);
      Px? nullable1 = new Px?(0.px());
      Px? top = new Px?(0.px());
      Px? nullable2 = new Px?(0.px());
      Px? right = new Px?();
      Px? bottom = nullable2;
      Px? left = nullable1;
      uiComponentArray[0] = component.AbsolutePosition<UiComponent>(top, right, bottom, left);
      uiComponentArray[1] = hideCheckmark ? (UiComponent) null : new UiComponent().Class<UiComponent>(Cls.inputDropdown_option_tick);
      uiComponentArray[2] = item;
      this.Add(uiComponentArray);
    }
  }
}
