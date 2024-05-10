// Decompiled with JetBrains decompiler
// Type: Mafi.Vornoi.VornoiCell
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Vornoi
{
  /// <summary>
  /// Vornoi cell represented as counter-clockwise polygon and cell center.
  /// </summary>
  public class VornoiCell
  {
    /// <summary>
    /// Index of the center point of this cell. This indexes array of center points which is different from perimeter
    /// points.
    /// </summary>
    public readonly int PointIndex;
    /// <summary>
    /// Indices of vornoi points around perimeter of this cell in counterclockwise. An edge [i, i + 1] is shared with
    /// neighbor at index i.
    /// </summary>
    public readonly ImmutableArray<int> Perimeter;
    /// <summary>
    /// Neighbors represented as point indices to their center point. The neighbors are in counterclockwise order. If
    /// <see cref="F:Mafi.Vornoi.VornoiCell.IsOnBoundary" /> is true, not all neighbors are valid cells.
    /// </summary>
    public readonly ImmutableArray<int> NeighborsPointIndices;
    /// <summary>
    /// Whether this cell is on the boundary. This is true iff some neighbors are not valid cells.
    /// </summary>
    public readonly bool IsOnBoundary;

    public VornoiCell(
      int pointIndex,
      ImmutableArray<int> perimeter,
      ImmutableArray<int> neighborsPointIndices,
      bool isOnBoundary)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PointIndex = pointIndex.CheckNotNegative();
      this.Perimeter = perimeter.CheckNotEmpty<int>();
      this.NeighborsPointIndices = neighborsPointIndices.CheckNotEmpty<int>();
      this.IsOnBoundary = isOnBoundary;
    }
  }
}
