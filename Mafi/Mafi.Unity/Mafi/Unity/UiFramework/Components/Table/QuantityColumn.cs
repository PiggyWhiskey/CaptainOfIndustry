// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.QuantityColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  /// <summary>Column that displays quantity.</summary>
  public class QuantityColumn : Column<ProductQuantityLarge, Txt>
  {
    public QuantityColumn(Mafi.Unity.UiFramework.Components.Table.Table view, int index, string title, int width, bool mergeWithPrevious)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(view, index, title, width, mergeWithPrevious);
    }

    protected override Txt CreateView(UiBuilder builder, bool highlighted)
    {
      return builder.NewTxt("Column " + this.Index.ToString()).SetAlignment(TextAnchor.MiddleRight).SetText("0").SetTextStyle(highlighted ? builder.Style.Statistics.HighligtedText : builder.Style.Panel.Text);
    }

    protected override GameObject ResolveGameObject(Txt view) => view.GameObject;

    protected override ProductQuantityLarge DefaultValue() => ProductQuantityLarge.None;

    protected override ProductQuantityLarge UpdateCell(
      int rowIndex,
      Txt view,
      ProductQuantityLarge oldValue,
      ProductQuantityLarge newValue)
    {
      if (oldValue.Quantity != newValue.Quantity)
        view.SetText(newValue.FormatNumberAndUnitOnly());
      return newValue;
    }
  }
}
