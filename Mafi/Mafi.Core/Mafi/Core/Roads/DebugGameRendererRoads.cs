// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.DebugGameRendererRoads
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.PathFinding;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  public static class DebugGameRendererRoads
  {
    public static DebugGameMapDrawing DrawRoadEntity(
      this DebugGameMapDrawing drawing,
      IRoadGraphEntity roadEntity,
      ColorRgba? lanesColor = null)
    {
      Tile2f roadOrigin = roadEntity.CenterTile.Xy.CornerTile2f;
      Fix32 laneExtent = RoadEntityProto.LANE_WIDTH_INNER.Value.HalfFast;
      drawing.DrawLayout(roadEntity.Prototype.Layout, roadEntity.Transform);
      ColorRgba color = lanesColor ?? ColorRgba.LightGray.SetA((byte) 192);
      for (int laneIndex = 0; laneIndex < roadEntity.RoadLanesCount; ++laneIndex)
      {
        RoadLaneTrajectory lane = roadEntity.GetTransformedRoadLane(laneIndex);
        drawing.DrawLine(lane.LaneCenterSamples.Select<Tile2f>((Func<RelTile3f, Tile2f>) (x => roadOrigin + x.Xy)), color);
        drawing.DrawLine(lane.LaneCenterSamples.Select<Tile2f>((Func<RelTile3f, int, Tile2f>) ((s, i) => roadOrigin + s.Xy + lane.LaneDirectionSamples[i].Xy.LeftOrthogonalVector * laneExtent)), color);
        drawing.DrawLine(lane.LaneCenterSamples.Select<Tile2f>((Func<RelTile3f, int, Tile2f>) ((s, i) => roadOrigin + s.Xy + lane.LaneDirectionSamples[i].Xy.RightOrthogonalVector * laneExtent)), color);
      }
      return drawing;
    }

    public static DebugGameMapDrawing DrawRoadGraphPath(
      this DebugGameMapDrawing drawing,
      ImmutableArray<RoadPathSegment> path,
      ColorRgba color,
      RelTile1f offset = default (RelTile1f),
      Tile2f? previousSegmentLastPt = null)
    {
      Tile2f? previousSegmentLastPt1 = previousSegmentLastPt;
      drawing.drawRoadGraphPath(path, color, offset, ref previousSegmentLastPt1);
      return drawing;
    }

    private static void drawRoadGraphPath(
      this DebugGameMapDrawing drawing,
      ImmutableArray<RoadPathSegment> path,
      ColorRgba color,
      RelTile1f offset,
      ref Tile2f? previousSegmentLastPt)
    {
      foreach (RoadPathSegment roadPathSegment in path)
      {
        RoadPathSegment segment = roadPathSegment;
        RoadLaneTrajectory traj = segment.Entity.GetTransformedRoadLane(segment.LaneIndex);
        int i = !previousSegmentLastPt.HasValue ? 1 : 0;
        Tile2f fromTile = previousSegmentLastPt ?? getSample(0);
        for (; i < traj.LaneCenterSamples.Length; ++i)
        {
          Tile2f sample = getSample(i);
          drawing.DrawLine(fromTile, sample, color, i > 1);
          fromTile = sample;
        }
        previousSegmentLastPt = new Tile2f?(fromTile);

        Tile2f getSample(int i)
        {
          Tile2f tile2f = segment.Entity.CenterTile.Xy.CornerTile2f + traj.LaneCenterSamples[i].Xy;
          RelTile2f relTile2f1 = traj.LaneDirectionSamples[i].Xy;
          relTile2f1 = relTile2f1.LeftOrthogonalVector;
          RelTile2f relTile2f2 = relTile2f1.Normalized * offset.Value;
          return tile2f + relTile2f2;
        }
      }
    }

    public static DebugGameMapDrawing DrawVehiclePath(
      this DebugGameMapDrawing drawing,
      [CanBeNull] IVehiclePathSegment firstSegment,
      VehiclePathFindingParams vehiclePfParams,
      ColorRgba color)
    {
      return drawing.DrawVehiclePathSegment(firstSegment, vehiclePfParams, color, true);
    }

    public static DebugGameMapDrawing DrawVehiclePathSegment(
      this DebugGameMapDrawing drawing,
      [CanBeNull] IVehiclePathSegment segment,
      VehiclePathFindingParams vehiclePfParams,
      ColorRgba color,
      bool drawAllFollowingSegments = false,
      Tile2f? previousSegmentLastPt = null)
    {
      if (segment == null)
        return drawing;
      if (segment is VehicleTerrainPathSegment terrainPathSegment)
      {
        if (terrainPathSegment.PathRawReversed.IsNotEmpty)
        {
          int num = !previousSegmentLastPt.HasValue ? 1 : 0;
          Tile2f fromTile = previousSegmentLastPt ?? vehiclePfParams.ConvertToCenterTileSpace(terrainPathSegment.PathRawReversed.Last);
          for (int count = terrainPathSegment.PathRawReversed.Count; num < count; ++num)
          {
            Tile2f centerTileSpace = vehiclePfParams.ConvertToCenterTileSpace(terrainPathSegment.PathRawReversed[count - num - 1]);
            drawing.DrawLine(fromTile, centerTileSpace, color, true);
            fromTile = centerTileSpace;
          }
          previousSegmentLastPt = new Tile2f?(fromTile);
        }
      }
      else if (segment is VehicleRoadPathSegment vehicleRoadPathSegment)
        drawing.drawRoadGraphPath(vehicleRoadPathSegment.Path, color, RelTile1f.Zero, ref previousSegmentLastPt);
      if (drawAllFollowingSegments && segment.NextSegment.HasValue)
        drawing.DrawVehiclePathSegment(segment.NextSegment.Value, vehiclePfParams, color, true, previousSegmentLastPt);
      return drawing;
    }
  }
}
