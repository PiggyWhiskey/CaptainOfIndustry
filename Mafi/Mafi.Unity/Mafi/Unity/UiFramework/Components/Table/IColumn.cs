// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.IColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  /// <summary>
  /// General interface for column implemenation used in <see cref="T:Mafi.Unity.UiFramework.Components.Table.Table" />
  /// </summary>
  public interface IColumn
  {
    /// <summary>
    /// Title that should be used in the header. Ignored if <see cref="P:Mafi.Unity.UiFramework.Components.Table.IColumn.MergeWithPrevious" /> is true.
    /// </summary>
    string Title { get; }

    /// <summary>Fixed width of the column.</summary>
    int Width { get; }

    /// <summary>
    /// Whether the header of this column should be merged with previsou column. Also no divider will be shown
    /// between merged columns.
    /// </summary>
    bool MergeWithPrevious { get; }

    /// <summary>Adds new row to the column.</summary>
    void AddRow(bool highlighted);

    /// <summary>Removes last row from the column.</summary>
    void RemoveLastRow();

    /// <summary>
    /// Returns GameObject for the given row that should be used in the table.
    /// </summary>
    GameObject GetGameObject(int rowIndex);

    /// <summary>Custom offset of the view in table cell.</summary>
    Offset InnerOffset();
  }
}
