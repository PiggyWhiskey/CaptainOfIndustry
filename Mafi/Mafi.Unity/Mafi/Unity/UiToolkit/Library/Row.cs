// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Row
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Row : UiComponentDecorated<VisualElement>, IFlexComponent, IUiComponent
  {
    Option<IGapHandler> IFlexComponent.GapHandler { get; set; }

    public Row(Outer outer = null, Inner inner = null, Px? gap = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new VisualElement(), outer, inner);
      this.Class<Row>(Cls.row);
      this.InnerElement.name = nameof (Row);
      if (!gap.HasValue)
        return;
      this.Gap<Row>(gap);
    }

    public Row(Px gap)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(gap: new Px?(gap));
    }

    public IFlexDecorator GetFlexDecorator()
    {
      return (IFlexDecorator) FlexDecorator.GetSharedInstance(this.InnerElement);
    }

    bool IFlexComponent.GetIsRow() => true;
  }
}
