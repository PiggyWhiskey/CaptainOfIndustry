// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ButtonRow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ButtonRow : Button, IFlexComponent, IUiComponent
  {
    Option<IGapHandler> IFlexComponent.GapHandler { get; set; }

    public ButtonRow(Action onClick = null, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(onClick, outer, inner);
      this.Class<ButtonRow>(Cls.row);
    }

    public IFlexDecorator GetFlexDecorator()
    {
      return (IFlexDecorator) FlexDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    bool IFlexComponent.GetIsRow() => true;
  }
}
