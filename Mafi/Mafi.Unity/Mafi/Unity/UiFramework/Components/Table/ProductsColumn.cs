// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.ProductsColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  public class ProductsColumn : Column<ProductProto, ProductCell>
  {
    private readonly TextStyle? m_textStyle;

    public ProductsColumn(
      Mafi.Unity.UiFramework.Components.Table.Table view,
      int index,
      string title,
      int width,
      bool mergeWithPrevious,
      TextStyle? textStyle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(view, index, title, width, mergeWithPrevious);
      this.m_textStyle = textStyle;
    }

    protected override ProductCell CreateView(UiBuilder builder, bool highlighted)
    {
      TextStyle textStyle = this.m_textStyle ?? builder.Style.Panel.Text;
      return new ProductCell(builder, "Column " + this.Index.ToString(), 15, 5).SetTextStyle(highlighted ? builder.Style.Statistics.HighligtedText : textStyle).SetTextAlignment(TextAnchor.MiddleLeft);
    }

    protected override GameObject ResolveGameObject(ProductCell view) => view.GameObject;

    protected override ProductProto DefaultValue() => (ProductProto) null;

    protected override ProductProto UpdateCell(
      int rowIndex,
      ProductCell cell,
      ProductProto oldValue,
      ProductProto newValue)
    {
      cell.SetProduct((Option<ProductProto>) newValue);
      return newValue;
    }
  }
}
