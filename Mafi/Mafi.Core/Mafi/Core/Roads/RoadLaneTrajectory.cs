// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadLaneTrajectory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct RoadLaneTrajectory
  {
    public readonly ImmutableArray<RelTile3f> LaneCenterSamples;
    public readonly ImmutableArray<RelTile3f> LaneDirectionSamples;
    public readonly ImmutableArray<RelTile1f> SegmentLengthsPrefixSums;

    public RoadLaneTrajectory(
      ImmutableArray<RelTile3f> laneCenterSamples,
      ImmutableArray<RelTile3f> laneDirectionSamples,
      ImmutableArray<RelTile1f> segmentLengthsPrefixSums)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.LaneCenterSamples = laneCenterSamples;
      this.LaneDirectionSamples = laneDirectionSamples;
      this.SegmentLengthsPrefixSums = segmentLengthsPrefixSums;
    }

    public override string ToString()
    {
      return string.Format("Traj {0} samples from {1} ", (object) this.LaneCenterSamples.Length, (object) this.LaneCenterSamples.First) + string.Format("to {0}, len {1}", (object) this.LaneCenterSamples.Last, (object) this.SegmentLengthsPrefixSums.Last);
    }
  }
}
