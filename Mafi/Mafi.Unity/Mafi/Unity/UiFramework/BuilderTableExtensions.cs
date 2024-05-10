// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderTableExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Table;
using Mafi.Unity.UserInterface;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class BuilderTableExtensions
  {
    public static Mafi.Unity.UiFramework.Components.Table.Table NewTable(this UiBuilder builder)
    {
      return new Mafi.Unity.UiFramework.Components.Table.Table(builder);
    }

    public static TxtColumn AddTextColumn(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      string title,
      int width,
      bool mergeWithPrevious = false,
      TextAnchor textAlignment = TextAnchor.MiddleLeft,
      TextStyle? textStyle = null)
    {
      TxtColumn txtColumn = new TxtColumn(table, table.ColumnsCount, title, width, mergeWithPrevious, textAlignment, textStyle);
      table.AddColumn((IColumn) txtColumn);
      return txtColumn;
    }

    public static BtnColumn AddButtonColumn(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      string title,
      int width,
      bool mergeWithPrevious = false)
    {
      BtnColumn btnColumn = new BtnColumn(table, table.ColumnsCount, title, width, mergeWithPrevious);
      table.AddColumn((IColumn) btnColumn);
      return btnColumn;
    }

    public static QuantityColumn AddQuantityColumn(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      string title,
      int width,
      bool mergeWithPrevious = false)
    {
      QuantityColumn quantityColumn = new QuantityColumn(table, table.ColumnsCount, title, width, mergeWithPrevious);
      table.AddColumn((IColumn) quantityColumn);
      return quantityColumn;
    }

    public static QuantityColumn AddQuantityColumn(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      LocStrFormatted title,
      int width,
      bool mergeWithPrevious = false)
    {
      return table.AddQuantityColumn(title.Value, width, mergeWithPrevious);
    }

    public static Fix32Column AddFix32Column(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      string title,
      int width,
      bool mergeWithPrevious = false)
    {
      Fix32Column fix32Column = new Fix32Column(table, table.ColumnsCount, title, width, mergeWithPrevious);
      table.AddColumn((IColumn) fix32Column);
      return fix32Column;
    }

    public static ProductsColumn AddProductColumn(
      this Mafi.Unity.UiFramework.Components.Table.Table table,
      string title,
      int width,
      bool mergeWithPrevious = false,
      TextStyle? textStyle = null)
    {
      ProductsColumn productsColumn = new ProductsColumn(table, table.ColumnsCount, title, width, mergeWithPrevious, textStyle);
      table.AddColumn((IColumn) productsColumn);
      return productsColumn;
    }
  }
}
