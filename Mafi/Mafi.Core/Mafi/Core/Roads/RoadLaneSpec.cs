// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadLaneSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Curves;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct RoadLaneSpec
  {
    public readonly CubicBezierCurve3f TrajectoryCurve;
    public readonly Option<CubicBezierCurve3f> CustomZCurve;
    /// <summary>
    /// Trajectory offset from the curve (positive number offsets to the left from the curve direction).
    /// </summary>
    public readonly RelTile1f TrajectoryOffset;
    /// <summary>
    /// Whether the final trajectory has reversed direction compared to the curve.
    /// </summary>
    public readonly bool IsReversed;
    public readonly bool IsHidden;

    public RoadLaneSpec(
      CubicBezierCurve3f trajectoryCurve,
      Option<CubicBezierCurve3f> customZCurve = default (Option<CubicBezierCurve3f>),
      RelTile1f trajectoryOffset = default (RelTile1f),
      bool isReversed = false,
      bool isHidden = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TrajectoryCurve = trajectoryCurve;
      this.CustomZCurve = customZCurve;
      this.TrajectoryOffset = trajectoryOffset;
      this.IsReversed = isReversed;
      this.IsHidden = isHidden;
    }
  }
}
