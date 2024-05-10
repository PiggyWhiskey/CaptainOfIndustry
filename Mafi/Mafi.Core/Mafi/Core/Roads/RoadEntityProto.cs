// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Curves;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Roads
{
  public class RoadEntityProto : RoadEntityProtoBase
  {
    public static readonly RelTile1f LANE_WIDTH_OUTER;
    public static readonly RelTile1f DOUBLE_LANE_CENTER_OFFSET;
    public static readonly RelTile1f LANE_WIDTH_INNER;
    public static readonly ThicknessTilesF RAMP_HEIGHT_DELTA;
    private static readonly EntityLayoutParams LAYOUT_PARAMS;

    public override Type EntityType => typeof (RoadEntity);

    public RoadEntityProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<RoadLaneSpec> lanesSpecs,
      ImmutableArray<RoadLaneMetadata> lanesData,
      ImmutableArray<RoadLaneTrajectory> lanesTrajectories,
      RoadEntityProtoBase.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, lanesSpecs, lanesData, lanesTrajectories, graphics);
    }

    public static bool TryCreateLanes(
      ImmutableArray<RoadLaneSpec> lanes,
      out ImmutableArray<RoadLaneMetadata> lanesData,
      out ImmutableArray<RoadLaneTrajectory> trajData,
      out string error)
    {
      ImmutableArrayBuilder<RoadLaneMetadata> immutableArrayBuilder1 = new ImmutableArrayBuilder<RoadLaneMetadata>(lanes.Length);
      ImmutableArrayBuilder<RoadLaneTrajectory> immutableArrayBuilder2 = new ImmutableArrayBuilder<RoadLaneTrajectory>(lanes.Length);
      for (int index = 0; index < lanes.Length; ++index)
      {
        RoadLaneTrajectory resultTraj;
        if (!RoadEntityProto.tryComputeLaneTrajectory(lanes[index], out resultTraj, out error))
          return false;
        immutableArrayBuilder2[index] = resultTraj;
        RoadLaneMetadata roadLaneMetadata;
        if (!RoadEntityProto.tryComputeRoadLaneMetadata(lanes[index], resultTraj, out roadLaneMetadata, out error))
          return false;
        immutableArrayBuilder1[index] = roadLaneMetadata;
      }
      lanesData = immutableArrayBuilder1.GetImmutableArrayAndClear();
      trajData = immutableArrayBuilder2.GetImmutableArrayAndClear();
      error = "";
      return true;
    }

    public static bool TryCreateProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      ImmutableArray<RoadLaneSpec> lanes,
      RoadEntityProtoBase.Gfx graphics,
      EntityLayoutParser layoutParser,
      out RoadEntityProto result,
      out string error)
    {
      result = (RoadEntityProto) null;
      ImmutableArray<RoadLaneMetadata> lanesData1;
      ImmutableArray<RoadLaneTrajectory> trajData;
      if (!RoadEntityProto.TryCreateLanes(lanes, out lanesData1, out trajData, out error))
        return false;
      RelTile2i minLayoutTileCoord;
      EntityLayout layout = RoadEntityProto.createLayout(trajData, layoutParser, out minLayoutTileCoord);
      RelTile2f offset = layout.TransformRelative(RelTile2i.Zero, TileTransform.Identity).RelTile2f;
      offset -= minLayoutTileCoord;
      Dict<CubicBezierCurve3f, CubicBezierCurve3f> translatedCurves = new Dict<CubicBezierCurve3f, CubicBezierCurve3f>();
      lanes = lanes.Map<RoadLaneSpec>((Func<RoadLaneSpec, RoadLaneSpec>) (x => new RoadLaneSpec(createdTranslatedCopy(x.TrajectoryCurve), x.CustomZCurve, x.TrajectoryOffset, x.IsReversed)));
      ImmutableArray<RoadLaneMetadata> lanesData2 = lanesData1.Map<RoadLaneMetadata>((Func<RoadLaneMetadata, RoadLaneMetadata>) (x => new RoadLaneMetadata(x.StartPosition + offset, x.EndPosition + offset, x.StartDirection, x.EndDirection, x.LaneLength)));
      trajData = trajData.Map<RoadLaneTrajectory>((Func<RoadLaneTrajectory, RoadLaneTrajectory>) (x => new RoadLaneTrajectory(x.LaneCenterSamples.Map<RelTile3f>((Func<RelTile3f, RelTile3f>) (s => s + offset)), x.LaneDirectionSamples, x.SegmentLengthsPrefixSums)));
      result = new RoadEntityProto(id, strings, layout, EntityCosts.None, lanes, lanesData2, trajData, graphics);
      error = "";
      return true;

      CubicBezierCurve3f createdTranslatedCopy(CubicBezierCurve3f curve)
      {
        CubicBezierCurve3f cubicBezierCurve3f;
        if (!translatedCurves.TryGetValue(curve, out cubicBezierCurve3f))
        {
          cubicBezierCurve3f = curve.CreatedTranslatedCopy(offset.Vector2f);
          translatedCurves.Add(curve, cubicBezierCurve3f);
        }
        return cubicBezierCurve3f;
      }
    }

    private static EntityLayout createLayout(
      ImmutableArray<RoadLaneTrajectory> lanes,
      EntityLayoutParser layoutParser,
      out RelTile2i minLayoutTileCoord)
    {
      Dict<RelTile2i, int> minOccupiedLayoutTiles = new Dict<RelTile2i, int>();
      Dict<RelTile2i, int> maxOccupiedLayoutTiles = new Dict<RelTile2i, int>();
      RelTile2f epsilon = new RelTile2f((1.0 / 16.0).ToFix32(), (1.0 / 16.0).ToFix32());
      Fix32 halfFast = RoadEntityProto.LANE_WIDTH_INNER.Value.HalfFast;
      foreach (RoadLaneTrajectory lane in lanes)
      {
        int index = 0;
        for (int length = lane.LaneCenterSamples.Length; index < length; ++index)
        {
          RelTile3f laneCenterSample = lane.LaneCenterSamples[index];
          record(laneCenterSample);
          RelTile2f relTile2f1 = lane.LaneDirectionSamples[index].Xy;
          RelTile2f relTile2f2 = relTile2f1.Normalized * halfFast;
          record(laneCenterSample + relTile2f2.LeftOrthogonalVector);
          record(laneCenterSample + relTile2f2.RightOrthogonalVector);
          if (index > 0)
          {
            RelTile3f relTile3f = laneCenterSample.Average(lane.LaneCenterSamples[index - 1]);
            ref RelTile2f local = ref relTile2f2;
            relTile2f1 = lane.LaneDirectionSamples[index - 1].Xy;
            RelTile2f rhs = relTile2f1.Normalized * halfFast;
            relTile2f1 = local.Average(rhs);
            relTile2f2 = relTile2f1.Normalized;
            record(relTile3f + relTile2f2.LeftOrthogonalVector);
            record(relTile3f + relTile2f2.RightOrthogonalVector);
          }
        }
      }
      Lyst<RelTile3i> lyst = new Lyst<RelTile3i>();
      foreach (KeyValuePair<RelTile2i, int> keyValuePair in minOccupiedLayoutTiles)
      {
        int num;
        if (maxOccupiedLayoutTiles.TryGetValue(keyValuePair.Key, out num))
          lyst.Add(new RelTile3i(keyValuePair.Key, keyValuePair.Value.Min(num)));
      }
      RelTile2i relTile2i1 = RelTile2i.MaxValue;
      RelTile2i relTile2i2 = RelTile2i.MinValue;
      foreach (RelTile3i relTile3i in lyst)
      {
        relTile2i1 = relTile2i1.Min(relTile3i.Xy);
        relTile2i2 = relTile2i2.Max(relTile3i.Xy);
      }
      RelTile2i relTile2i3 = relTile2i2 - relTile2i1 + 1;
      int[][] array = new int[relTile2i3.Y][];
      for (int index1 = 0; index1 < relTile2i3.Y; ++index1)
      {
        int[] numArray = new int[relTile2i3.X];
        array[index1] = numArray;
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          numArray[index2] = int.MaxValue;
      }
      foreach (RelTile3i relTile3i in lyst)
      {
        if (relTile3i.Z < 0)
          throw new ProtoBuilderException("Road layout height should not go below 0.");
        RelTile2i relTile2i4 = relTile3i.Xy - relTile2i1;
        array[relTile2i4.Y][relTile2i4.X] = array[relTile2i4.Y][relTile2i4.X].Min(relTile3i.Z);
      }
      string[] strArray = array.MapArray<int[], string>((Func<int[], string>) (x => ((IEnumerable<string>) x.MapArray<int, string>((Func<int, string>) (h =>
      {
        if (h == int.MaxValue)
          return "   ";
        return h != -1 ? string.Format("={0}=", (object) (h + 1)) : "_1_";
      }))).JoinStrings()));
      Array.Reverse((Array) strArray);
      minLayoutTileCoord = relTile2i1;
      return layoutParser.ParseLayoutOrThrow(RoadEntityProto.LAYOUT_PARAMS, strArray);

      void record(RelTile3f t)
      {
        bool exists1;
        ref int local1 = ref minOccupiedLayoutTiles.GetRefValue((t.Xy + epsilon).RelTile2iFloored, out exists1);
        local1 = exists1 ? local1.Min(t.Z.ToIntFloored()) : t.Z.ToIntFloored();
        bool exists2;
        ref int local2 = ref maxOccupiedLayoutTiles.GetRefValue((t.Xy - epsilon).RelTile2iFloored, out exists2);
        local2 = exists2 ? local2.Max(t.Z.ToIntFloored()) : t.Z.ToIntFloored();
      }
    }

    private static bool tryComputeLaneTrajectory(
      RoadLaneSpec laneSpec,
      out RoadLaneTrajectory resultTraj,
      out string error)
    {
      CubicBezierCurve3f trajectoryCurve = laneSpec.TrajectoryCurve;
      int intCeiled = (trajectoryCurve.ApproximateCurveLength(10) * 10 / trajectoryCurve.SegmentsCount).ToIntCeiled();
      CubicBezierCurve3fSamplerCustom uniformSamplerCustom = trajectoryCurve.GetUniformSamplerCustom(intCeiled, (Func<CubicBezierCurve3f, int, Percent, Vector3f>) ((c, segmentI, t) =>
      {
        Vector3f laneTrajectory = c.SampleSegment(segmentI, t);
        if (laneSpec.TrajectoryOffset.IsZero)
          return laneTrajectory;
        Vector3f rhs = c.SampleSegmentDerivative(segmentI, t);
        Vector3f vector3f = Vector3f.UnitZ.Cross(rhs);
        Assert.That<Vector3f>(vector3f).IsNotZero();
        return laneTrajectory + vector3f.Normalized * laneSpec.TrajectoryOffset.Value;
      }));
      Fix32 curveLengthApprox = uniformSamplerCustom.CurveLengthApprox;
      int intRounded = (curveLengthApprox / RoadEntity.DISCRETIZATION_STEP.Value).ToIntRounded();
      ImmutableArrayBuilder<RelTile3f> immutableArrayBuilder1 = new ImmutableArrayBuilder<RelTile3f>(intRounded + 1);
      immutableArrayBuilder1[0] = new RelTile3f(uniformSamplerCustom.SampleCurveT(Percent.Zero));
      immutableArrayBuilder1[intRounded] = new RelTile3f(uniformSamplerCustom.SampleCurveT(Percent.Hundred));
      ImmutableArrayBuilder<RelTile3f> immutableArrayBuilder2 = new ImmutableArrayBuilder<RelTile3f>(intRounded + 1);
      immutableArrayBuilder2[0] = new RelTile3f(trajectoryCurve.SampleDerivative(Percent.Zero).Normalized);
      immutableArrayBuilder2[intRounded] = new RelTile3f(trajectoryCurve.SampleDerivative(Percent.Hundred).Normalized);
      for (int i = 1; i < intRounded; ++i)
      {
        Fix32 arcLength = curveLengthApprox * i / intRounded;
        Percent curveT = uniformSamplerCustom.GetCurveT(arcLength);
        immutableArrayBuilder1[i] = new RelTile3f(uniformSamplerCustom.SampleCurveT(curveT));
        immutableArrayBuilder2[i] = new RelTile3f(trajectoryCurve.SampleDerivative(curveT).Normalized);
      }
      CubicBezierCurve3f valueOrNull = laneSpec.CustomZCurve.ValueOrNull;
      RelTile3f relTile3f1;
      if (valueOrNull != null)
      {
        CubicBezierCurve3fSampler uniformSampler = valueOrNull.GetUniformSampler(intCeiled);
        immutableArrayBuilder1[0] = immutableArrayBuilder1[0].SetZ(valueOrNull.Sample(Percent.Zero).Z);
        immutableArrayBuilder1[intRounded] = immutableArrayBuilder1[intRounded].SetZ(valueOrNull.Sample(Percent.Hundred).Z);
        immutableArrayBuilder2[0] = immutableArrayBuilder2[0].SetZ(valueOrNull.SampleDerivative(Percent.Zero).Normalized.Z).Normalized;
        ref ImmutableArrayBuilder<RelTile3f> local1 = ref immutableArrayBuilder2;
        int i1 = intRounded;
        relTile3f1 = immutableArrayBuilder2[intRounded].SetZ(valueOrNull.SampleDerivative(Percent.Hundred).Normalized.Z);
        RelTile3f normalized1 = relTile3f1.Normalized;
        local1[i1] = normalized1;
        for (int i2 = 1; i2 < intRounded; ++i2)
        {
          Fix32 arcLength = curveLengthApprox * i2 / intRounded;
          Percent curveT = uniformSampler.GetCurveT(arcLength);
          ref ImmutableArrayBuilder<RelTile3f> local2 = ref immutableArrayBuilder1;
          int i3 = i2;
          relTile3f1 = immutableArrayBuilder1[i2];
          RelTile3f relTile3f2 = relTile3f1.SetZ(valueOrNull.Sample(curveT).Z);
          local2[i3] = relTile3f2;
          ref ImmutableArrayBuilder<RelTile3f> local3 = ref immutableArrayBuilder2;
          int i4 = i2;
          relTile3f1 = immutableArrayBuilder2[i2];
          relTile3f1 = relTile3f1.SetZ(trajectoryCurve.SampleDerivative(curveT).Normalized.Z);
          RelTile3f normalized2 = relTile3f1.Normalized;
          local3[i4] = normalized2;
        }
      }
      if (laneSpec.IsReversed)
      {
        immutableArrayBuilder1.Reverse();
        immutableArrayBuilder2.Reverse();
        for (int i = 0; i < immutableArrayBuilder2.Length; ++i)
          immutableArrayBuilder2[i] = -immutableArrayBuilder2[i];
      }
      ImmutableArray<RelTile3f> immutableArrayAndClear1 = immutableArrayBuilder1.GetImmutableArrayAndClear();
      ImmutableArray<RelTile3f> immutableArrayAndClear2 = immutableArrayBuilder2.GetImmutableArrayAndClear();
      ImmutableArrayBuilder<RelTile1f> immutableArrayBuilder3 = new ImmutableArrayBuilder<RelTile1f>(immutableArrayAndClear1.Length);
      for (int index = 1; index < immutableArrayAndClear1.Length; ++index)
      {
        ref ImmutableArrayBuilder<RelTile1f> local = ref immutableArrayBuilder3;
        int i = index;
        RelTile1f relTile1f1 = immutableArrayBuilder3[index - 1];
        relTile3f1 = immutableArrayAndClear1[index - 1] - immutableArrayAndClear1[index];
        RelTile1f relTile1f2 = relTile3f1.Length.Tiles();
        RelTile1f relTile1f3 = relTile1f1 + relTile1f2;
        local[i] = relTile1f3;
      }
      resultTraj = new RoadLaneTrajectory(immutableArrayAndClear1, immutableArrayAndClear2, immutableArrayBuilder3.GetImmutableArrayAndClear());
      error = "";
      return true;
    }

    private static bool tryComputeRoadLaneMetadata(
      RoadLaneSpec lane,
      RoadLaneTrajectory trajectory,
      out RoadLaneMetadata roadLaneMetadata,
      out string error)
    {
      RelTile3f resultPt1;
      RelTile3f resultPt2;
      RoadGraphNodeDirection resultDir1;
      RoadGraphNodeDirection resultDir2;
      if (!tryValidatePt(trajectory.LaneCenterSamples.First, out resultPt1, out error) || !tryValidatePt(trajectory.LaneCenterSamples.Last, out resultPt2, out error) || !tryValidateDir(trajectory.LaneDirectionSamples.First, out resultDir1, out error) || !tryValidateDir(trajectory.LaneDirectionSamples.Last, out resultDir2, out error))
      {
        roadLaneMetadata = new RoadLaneMetadata();
        return false;
      }
      roadLaneMetadata = new RoadLaneMetadata(resultPt1, resultPt2, resultDir1, resultDir2, trajectory.SegmentLengthsPrefixSums.Last);
      error = "";
      return true;

      static bool tryValidatePt(RelTile3f pt, out RelTile3f resultPt, out string error)
      {
        resultPt = new RelTile3f();
        RelTile3i relTile3iRounded = pt.Times2Fast.RelTile3iRounded;
        if (!relTile3iRounded.Vector3f.IsNear(pt.Vector3f.Times2Fast))
        {
          error = string.Format("Lane point {0} is not a multiple of 0.5.", (object) pt);
          return false;
        }
        if (!pt.HeightTiles1f.Value.IsNear((Fix32) pt.HeightTiles1f.Value.IntegerPart))
        {
          error = string.Format("Z coordinate of point {0} is not an integer.", (object) pt);
          return false;
        }
        resultPt = relTile3iRounded.CornerRelTile3f.HalfFast;
        error = "";
        return true;
      }

      static bool tryValidateDir(
        RelTile3f direction,
        out RoadGraphNodeDirection resultDir,
        out string error)
      {
        resultDir = new RoadGraphNodeDirection(direction.Signs);
        if (!resultDir.DirectionSigns.Xy.Angle.IsNear(direction.Xy.Angle))
        {
          error = string.Format("Road direction has invalid X-Y direction {0} {1}, ", (object) direction.Xy, (object) direction.Xy.Angle) + string.Format("signs: {0} {1}.", (object) resultDir.DirectionSigns.Xy, (object) resultDir.DirectionSigns.Xy.Angle);
          return false;
        }
        ThicknessTilesF thicknessTilesF = direction.Z * RoadEntityProto.RAMP_HEIGHT_DELTA;
        if (thicknessTilesF.IsNotZero && !thicknessTilesF.Abs.IsNear(RoadEntityProto.RAMP_HEIGHT_DELTA))
        {
          error = string.Format("Road direction has invalid height step {0}, ", (object) direction.Z) + string.Format("expected: {0}.", (object) (Fix32.One / RoadEntityProto.RAMP_HEIGHT_DELTA.Value));
          return false;
        }
        error = "";
        return true;
      }
    }

    static RoadEntityProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RoadEntityProto.LANE_WIDTH_OUTER = 2.0.Tiles();
      RoadEntityProto.DOUBLE_LANE_CENTER_OFFSET = 1.0.Tiles();
      RoadEntityProto.LANE_WIDTH_INNER = 1.875.Tiles();
      RoadEntityProto.RAMP_HEIGHT_DELTA = 0.25.TilesThick();
      RoadEntityProto.LAYOUT_PARAMS = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[2]
      {
        new CustomLayoutToken("=0=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h, h + 3))),
        new CustomLayoutToken("_0_", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: h, terrainSurfaceHeight: new int?(0))))
      });
    }
  }
}
