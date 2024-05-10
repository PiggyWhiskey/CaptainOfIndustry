// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportTrajectory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Terrain;
using Mafi.Curves;
using Mafi.Localization;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Immutable representation of discretized transport trajectory.
  /// </summary>
  /// <remarks>
  /// This class computes its data on-demand because some use-cases (such as previews in UI) don't need some data
  /// such as the waypoints, which are actually quite expensive to compute. This also simplifies loading.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public sealed class TransportTrajectory
  {
    /// <summary>Proto of transport of this trajectory.</summary>
    public readonly TransportProto TransportProto;
    [DoNotSave(0, null)]
    private CubicBezierCurve3f m_curve;
    [DoNotSave(0, null)]
    private ImmutableArray<int> m_pivotSegmentIndices;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedTileRange> m_occupiedTiles;
    [DoNotSave(0, null)]
    private ImmutableArray<TransportTileMetadata?> m_occupiedTilesMetadata;
    [DoNotSave(0, null)]
    private ImmutableArray<TransportFlowIndicatorPose> m_flowIndicatorPoses;
    /// <summary>
    /// Pivots of this transport trajectory. Pivots must be orthogonal or zero in the horizontal plane
    /// (ie. X == 0, Y == 0 or XY==0). Adjacent pivots can have non-zero offset in Z, but that offset
    /// must not exceed the prototype's ZStepLength.
    /// </summary>
    public readonly ImmutableArray<Tile3i> Pivots;
    /// <summary>
    /// Direction of the first pivot. This direction always points away from the trajectory.
    /// </summary>
    public readonly RelTile3i StartDirection;
    /// <summary>
    /// Direction of the last pivot. This direction always points away from the trajectory.
    /// </summary>
    public readonly RelTile3i EndDirection;
    [DoNotSave(0, null)]
    private ImmutableArray<TransportWaypoint> m_waypoints;
    [DoNotSave(0, null)]
    private ImmutableArray<int> m_curveSegmentWaypointIndices;
    [DoNotSave(0, null)]
    private RelTile1f m_length;
    [DoNotSave(0, null)]
    private AssetValue? m_priceCache;
    [DoNotSave(0, null)]
    private ImmutableArray<TransportSupportableTile> m_supportableTiles;
    [ThreadStatic]
    private static Lyst<TransportWaypoint> s_waypointsTmp;
    /// <summary>
    /// Cache for the `TryGetLowPivotIndexFor` method. Do not use this anywhere else to keep the
    /// `TryGetLowPivotIndexFor` thread safe.
    /// </summary>
    [ThreadStatic]
    private static Lyst<OccupiedTileRange> s_tryGetLowPivotIndexForTilesTmp;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// The original curve for this trajectory. This is used by GFX that generates transport meshes which may use
    /// different discretization settings.
    /// </summary>
    public CubicBezierCurve3f Curve
    {
      get
      {
        if (this.m_curve == null)
          this.recomputeCurveOrThrow(out this.m_curve, out this.m_pivotSegmentIndices);
        return this.m_curve;
      }
    }

    /// <summary>
    /// Maps a pivot index to a curve segment index that contains the pivot.
    /// </summary>
    public ImmutableArray<int> PivotSegmentIndices
    {
      get
      {
        if (this.m_curve == null)
          this.recomputeCurveOrThrow(out this.m_curve, out this.m_pivotSegmentIndices);
        return this.m_pivotSegmentIndices;
      }
    }

    /// <summary>
    /// Occupied tiles of the transport. There may be more than one entry per 2D tile if transport is crossing
    /// above/below itself. This class does not guarantee no self-intersections.
    /// </summary>
    public ImmutableArray<OccupiedTileRange> OccupiedTiles
    {
      get
      {
        if (this.m_occupiedTiles.IsNotValid)
          this.m_occupiedTiles = TransportHelper.ComputeOccupiedTiles(this, out this.m_occupiedTilesMetadata);
        return this.m_occupiedTiles;
      }
    }

    /// <summary>
    /// Metadata for <see cref="P:Mafi.Core.Factory.Transports.TransportTrajectory.OccupiedTiles" />. If the metadata of a tile is null, that occupied tile is
    /// "helper" tile that just blocks area above/below pivot due to ramps. Such tile has no transport through it.
    /// </summary>
    public ImmutableArray<TransportTileMetadata?> OccupiedTilesMetadata
    {
      get
      {
        if (this.m_occupiedTiles.IsNotValid)
          this.m_occupiedTiles = TransportHelper.ComputeOccupiedTiles(this, out this.m_occupiedTilesMetadata);
        return this.m_occupiedTilesMetadata;
      }
    }

    public ImmutableArray<TransportFlowIndicatorPose> FlowIndicatorsPoses
    {
      get
      {
        if (this.m_flowIndicatorPoses.IsNotValid)
          this.m_flowIndicatorPoses = this.computeFlowIndicatorsPoses();
        return this.m_flowIndicatorPoses;
      }
    }

    /// <summary>
    /// Discretized waypoints of this trajectory. Distance between waypoints is equal to transport speed per tick.
    /// </summary>
    public ImmutableArray<TransportWaypoint> Waypoints
    {
      get
      {
        if (this.m_waypoints.IsNotValid)
          this.computeWaypoints();
        return this.m_waypoints;
      }
    }

    /// <summary>
    /// Curve segment <c>i</c> starts at waypoint given by this array on index <c>i</c> and ends at <c>i + 1</c>.
    /// </summary>
    public ImmutableArray<int> CurveSegmentWaypointIndices
    {
      get
      {
        if (this.m_waypoints.IsNotValid)
          this.computeWaypoints();
        return this.m_curveSegmentWaypointIndices;
      }
    }

    /// <summary>Total length of all segments</summary>
    public RelTile1f TrajectoryLength
    {
      get
      {
        if (this.m_waypoints.IsNotValid)
          this.computeWaypoints();
        return this.m_length;
      }
    }

    /// <summary>
    /// Max number of products on the transport, given that all products obey spacing.
    /// </summary>
    public int MaxProducts
    {
      get
      {
        if (this.m_waypoints.IsNotValid)
          this.computeWaypoints();
        return this.m_waypoints.Length.CeilDiv(this.TransportProto.ProductSpacingWaypoints);
      }
    }

    public AssetValue Price
    {
      get
      {
        if (!this.m_priceCache.HasValue)
          this.m_priceCache = new AssetValue?(this.TransportProto.GetPriceFor(this.Pivots));
        return this.m_priceCache.Value;
      }
    }

    /// <summary>
    /// Tiles ordered along the trajectory that need to be supported by pillars. A pillar will support all tiles
    /// around index <c>i</c> in radius <see cref="F:Mafi.Core.Factory.Transports.TransportProto.MaxPillarSupportRadius" /> indices.
    /// All pairs of neighboring tiles are at distance 1 to each other.
    /// 
    /// This differs from <see cref="P:Mafi.Core.Factory.Transports.TransportTrajectory.OccupiedTiles" /> since that array has some extra entries where
    /// with null metadata. All supportable tiles have valid metadata.
    /// </summary>
    public ImmutableArray<TransportSupportableTile> SupportableTiles
    {
      get
      {
        if (this.m_supportableTiles.IsNotValid)
        {
          Lyst<TransportSupportableTile> supportablePositions = new Lyst<TransportSupportableTile>();
          TransportHelper.ComputeSupportableAlongTrajectory(this, supportablePositions);
          this.m_supportableTiles = supportablePositions.ToImmutableArray();
        }
        return this.m_supportableTiles;
      }
    }

    private TransportTrajectory(
      TransportProto proto,
      CubicBezierCurve3f curve,
      ImmutableArray<int> pivotSegmentIndices,
      ImmutableArray<Tile3i> pivots,
      RelTile3i startDirection,
      RelTile3i endDirection)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportProto = proto;
      this.m_curve = curve;
      this.m_pivotSegmentIndices = pivotSegmentIndices;
      this.Pivots = pivots;
      this.StartDirection = startDirection;
      this.EndDirection = endDirection;
    }

    private TransportTrajectory(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      RelTile3i startDirection,
      RelTile3i endDirection)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportProto = proto;
      this.Pivots = pivots;
      this.StartDirection = startDirection;
      this.EndDirection = endDirection;
    }

    /// <summary>
    /// Re-computes curve, usually happens after load. If this fails, <see cref="T:System.Exception" /> is thrown.
    /// </summary>
    private void recomputeCurveOrThrow(
      out CubicBezierCurve3f curve,
      out ImmutableArray<int> pivotSegmentIndices)
    {
      string errorMessage;
      if (!TransportTrajectory.tryCreateCurveFromPivots(this.TransportProto, this.Pivots, this.StartDirection, this.EndDirection, out curve, out pivotSegmentIndices, out errorMessage, true))
        throw new Exception("Failed to re-compute curve from pivots: " + errorMessage);
    }

    /// <summary>
    /// Tries to create trajectory fom given pivots and optional start/end directions. If start/end directions are
    /// not given, they will be set based on transport direction on each end.
    /// </summary>
    /// <param name="pivots">Must not be empty.</param>
    /// <param name="startDirMaybe">Should be normalized (exactly one component equal to 1).</param>
    /// <param name="endDirMaybe">Should be normalized (exactly one component equal to 1).</param>
    /// <param name="trajectory">Result.</param>
    /// <param name="error">Error if false is returned.</param>
    /// <param name="ignoreSoftConstraints">If set, soft constraints such as z-step length are checked. Issue with
    /// these constraints does not make the trajectory invalid, but it won't conform to the proto rules.</param>
    /// <param name="allowDenormalizedStartEndDirections">Whether non-flat start/end are allowed. This should be only
    /// set for testing or creation of sub-meshes. No transport in game should exist with non-flat start/end.</param>
    public static bool TryCreateFromPivots(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      RelTile3i? startDirMaybe,
      RelTile3i? endDirMaybe,
      out TransportTrajectory trajectory,
      out string error,
      bool ignoreSoftConstraints = false,
      bool allowDenormalizedStartEndDirections = false)
    {
      if (startDirMaybe.HasValue)
        Assert.That<RelTile3i>(startDirMaybe.Value).IsNotZero();
      if (endDirMaybe.HasValue)
        Assert.That<RelTile3i>(endDirMaybe.Value).IsNotZero();
      RelTile3i startDirection;
      RelTile3i endDirection;
      TransportTrajectory.ComputeStartAndEndDirections(pivots.AsSlice, startDirMaybe, endDirMaybe, out startDirection, out endDirection);
      Assert.That<RelTile3i>(startDirection).IsNotZero();
      Assert.That<RelTile3i>(endDirection).IsNotZero();
      CubicBezierCurve3f curve;
      ImmutableArray<int> pivotSegmentIndices;
      if (!TransportTrajectory.tryCreateCurveFromPivots(proto, pivots, startDirection, endDirection, out curve, out pivotSegmentIndices, out error, ignoreSoftConstraints))
      {
        trajectory = (TransportTrajectory) null;
        return false;
      }
      if (!allowDenormalizedStartEndDirections)
      {
        startDirection = startDirection.SetXy(startDirection.Xy.Signs);
        endDirection = endDirection.SetXy(endDirection.Xy.Signs);
      }
      error = "";
      trajectory = new TransportTrajectory(proto, curve, pivotSegmentIndices, pivots, startDirection, endDirection);
      return true;
    }

    /// <summary>
    /// Creates transport curve. The curve represents a trajectory that is taken by the products. This method does
    /// all the necessary validation of the pivots.
    /// </summary>
    private static bool tryCreateCurveFromPivots(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      RelTile3i startDirection,
      RelTile3i endDirection,
      out CubicBezierCurve3f curve,
      out ImmutableArray<int> pivotSegmentIndices,
      out string errorMessage,
      bool ignoreSoftConstraints = false)
    {
      curve = (CubicBezierCurve3f) null;
      pivotSegmentIndices = ImmutableArray<int>.Empty;
      if (pivots.IsEmpty)
      {
        errorMessage = "No pivots.";
        return false;
      }
      if (pivots.Length == 1)
      {
        if (startDirection.IsParallelTo(endDirection))
        {
          errorMessage = "Start and end directions are identical for transport of length 1.";
          return false;
        }
      }
      else
      {
        RelTile3i other1 = pivots.Second - pivots.First;
        RelTile3i other2 = pivots.PreLast - pivots.Last;
        if (other1.IsZero)
        {
          errorMessage = string.Format("Repeated first two pivots {0}.", (object) pivots.First);
          return false;
        }
        if (other2.IsZero)
        {
          errorMessage = string.Format("Repeated last two pivots {0}.", (object) pivots.Last);
          return false;
        }
        if (startDirection.IsParallelTo(other1))
        {
          errorMessage = "Invalid start direction pointing into the transport.";
          return false;
        }
        if (endDirection.IsParallelTo(other2))
        {
          errorMessage = "Invalid end direction pointing into the transport.";
          return false;
        }
      }
      Vector3f vector3f = new Vector3f(Fix32.Half, Fix32.Half, proto.SurfaceRelativeHeight.Value);
      ImmutableArrayBuilder<int> immutableArrayBuilder = new ImmutableArrayBuilder<int>(pivots.Length);
      Vector3f currentWaypoint = vector3f + pivots[0].Vector3i;
      Vector3f ptInFront;
      TransportTrajectory.getPivotEndPoints(startDirection.Vector3f, out ptInFront, out Vector3f _);
      CubicBezierCurve3fBuilder curve1 = new CubicBezierCurve3fBuilder(currentWaypoint + ptInFront);
      RelTile3i other = startDirection;
      for (int index = 1; index < pivots.Length; ++index)
      {
        RelTile3i relTile3i = pivots[index - 1] - pivots[index];
        if (relTile3i.X == 0 == (relTile3i.Y == 0))
        {
          if (relTile3i.IsZero)
          {
            if (!ignoreSoftConstraints)
            {
              errorMessage = string.Format("Repeated pivot {0} at indices {1} and {2}.", (object) pivots[index], (object) (index - 1), (object) index);
              return false;
            }
            continue;
          }
          if (relTile3i.X != 0)
          {
            errorMessage = string.Format("Diagonal transports are not supported (pivots #{0} {1} ", (object) (index - 1), (object) pivots[index - 1]) + string.Format("and #{0} {1}).", (object) index, (object) pivots[index]);
            return false;
          }
        }
        else if (other.Xy.IsNotZero && relTile3i.Xy.IsAntiParallelTo(other.Xy) || relTile3i.IsAntiParallelTo(other))
        {
          errorMessage = string.Format("Segment between pivots {0} and {1} goes against the previous.", (object) (index - 1), (object) index);
          return false;
        }
        other = relTile3i;
        Vector3f nextWaypoint = vector3f + pivots[index].Vector3i;
        TransportTrajectory.extendCurve(curve1, currentWaypoint, nextWaypoint, proto.CornersSharpnessPercent, true);
        currentWaypoint = nextWaypoint;
        immutableArrayBuilder[index] = curve1.SegmentsCount;
      }
      TransportTrajectory.extendCurve(curve1, currentWaypoint, currentWaypoint + endDirection.Vector3f, proto.CornersSharpnessPercent, false);
      curve = curve1.CreateCurve();
      pivotSegmentIndices = immutableArrayBuilder.GetImmutableArrayAndClear();
      errorMessage = "";
      return true;
    }

    internal static bool TryCreateForAttachment(
      TransportPillarAttachmentType attachmentType,
      TransportProto proto,
      out TransportTrajectory traj,
      out string error)
    {
      traj = (TransportTrajectory) null;
      if (attachmentType == TransportPillarAttachmentType.NoAttachment)
      {
        error = "No attachment.";
        return false;
      }
      if (!proto.CanGoUpDown && attachmentType != TransportPillarAttachmentType.FlatToFlat_Turn)
      {
        error = "Ramp not allowed.";
        return false;
      }
      RelTile3i relTile3i1;
      switch (attachmentType)
      {
        case TransportPillarAttachmentType.NoAttachment:
          error = "No attachment.";
          return false;
        case TransportPillarAttachmentType.FlatToFlat_Straight:
        case TransportPillarAttachmentType.FlatToFlat_Turn:
        case TransportPillarAttachmentType.FlatToRampUp_Straight:
        case TransportPillarAttachmentType.FlatToRampUp_Turn:
        case TransportPillarAttachmentType.FlatToRampDown_Straight:
        case TransportPillarAttachmentType.FlatToRampDown_Turn:
        case TransportPillarAttachmentType.FlatToVertical:
        case TransportPillarAttachmentType.FlatToVertical_Down:
          relTile3i1 = new RelTile3i(-1, 0, 0);
          break;
        case TransportPillarAttachmentType.RampDownToRampUp_Turn:
          relTile3i1 = new RelTile3i(-proto.ZStepLength.Value, 0, -1);
          break;
        case TransportPillarAttachmentType.VerticalToVertical:
          relTile3i1 = new RelTile3i(0, 0, -1);
          break;
        default:
          error = string.Format("Attachment type not handled: {0}", (object) attachmentType);
          Log.Error(error);
          return false;
      }
      RelTile3i relTile3i2;
      switch (attachmentType)
      {
        case TransportPillarAttachmentType.FlatToFlat_Straight:
          relTile3i2 = new RelTile3i(1, 0, 0);
          break;
        case TransportPillarAttachmentType.FlatToFlat_Turn:
          relTile3i2 = new RelTile3i(0, 1, 0);
          break;
        case TransportPillarAttachmentType.RampDownToRampUp_Turn:
        case TransportPillarAttachmentType.FlatToRampUp_Turn:
          relTile3i2 = new RelTile3i(0, proto.ZStepLength.Value, 1);
          break;
        case TransportPillarAttachmentType.FlatToRampUp_Straight:
          relTile3i2 = new RelTile3i(proto.ZStepLength.Value, 0, 1);
          break;
        case TransportPillarAttachmentType.FlatToRampDown_Straight:
          relTile3i2 = new RelTile3i(proto.ZStepLength.Value, 0, -1);
          break;
        case TransportPillarAttachmentType.FlatToRampDown_Turn:
          relTile3i2 = new RelTile3i(0, proto.ZStepLength.Value, -1);
          break;
        case TransportPillarAttachmentType.FlatToVertical:
          relTile3i2 = new RelTile3i(0, 0, 1);
          break;
        case TransportPillarAttachmentType.VerticalToVertical:
          relTile3i2 = new RelTile3i(0, 0, 1);
          break;
        case TransportPillarAttachmentType.FlatToVertical_Down:
          relTile3i2 = new RelTile3i(0, 0, -1);
          break;
        default:
          error = string.Format("Attachment type not handled: {0}", (object) attachmentType);
          Log.Error(error);
          return false;
      }
      return TransportTrajectory.TryCreateFromPivots(proto, ImmutableArray.Create<Tile3i>(Tile3i.Zero), new RelTile3i?(relTile3i1), new RelTile3i?(relTile3i2), out traj, out error, true, true);
    }

    public bool TryReverse(out TransportTrajectory reversedTrajectory, out string error)
    {
      return TransportTrajectory.TryCreateFromPivots(this.TransportProto, this.Pivots.Reversed(), new RelTile3i?(this.EndDirection), new RelTile3i?(this.StartDirection), out reversedTrajectory, out error);
    }

    public bool TryChangeStartDirection(
      RelTile3i newStartDirection,
      out TransportTrajectory newTrajectory,
      out string error)
    {
      return TransportTrajectory.TryCreateFromPivots(this.TransportProto, this.Pivots, new RelTile3i?(newStartDirection), new RelTile3i?(this.EndDirection), out newTrajectory, out error);
    }

    public bool TryChangeEndDirection(
      RelTile3i newEndDirection,
      out TransportTrajectory newTrajectory,
      out string error)
    {
      return TransportTrajectory.TryCreateFromPivots(this.TransportProto, this.Pivots, new RelTile3i?(this.StartDirection), new RelTile3i?(newEndDirection), out newTrajectory, out error);
    }

    /// <summary>
    /// Optimizes pivots in-place by removing pivots from straight lines keeping only corners and ramps.
    /// </summary>
    public static void OptimizePivots(Lyst<Tile3i> pivots)
    {
      if (pivots.Count <= 1)
        return;
      RelTile3i relTile3i1 = normalizeDir(pivots[1] - pivots[0]);
      int index1 = 1;
      int index2 = 2;
      for (int count = pivots.Count; index2 < count; ++index2)
      {
        RelTile3i relTile3i2 = normalizeDir(pivots[index2] - pivots[index2 - 1]);
        if (relTile3i2 != relTile3i1)
        {
          pivots[index1] = pivots[index2 - 1];
          ++index1;
          relTile3i1 = relTile3i2;
        }
      }
      Assert.That<int>(index1).IsLess(pivots.Count);
      pivots[index1] = pivots.Last;
      pivots.Count = index1 + 1;

      static RelTile3i normalizeDir(RelTile3i dir)
      {
        return dir.Xy.IsZero ? new RelTile3i(0, 0, dir.Z.Sign()) : new RelTile3i(dir.X.Sign(), dir.Y.Sign(), dir.Z);
      }
    }

    private static void extendCurve(
      CubicBezierCurve3fBuilder curve,
      Vector3f currentWaypoint,
      Vector3f nextWaypoint,
      Percent controlPointsDistance,
      bool addStraightSegment)
    {
      Assert.That<Percent>(controlPointsDistance).IsWithin0To100PercIncl();
      Vector3f lastControlPoint = curve.LastControlPoint;
      Assert.That<Fix64>((lastControlPoint - currentWaypoint).LengthSqr).IsLessOrEqual(Fix64.FromFraction(3L, 4L), string.Format("Last: {0}, current, {1}", (object) lastControlPoint, (object) currentWaypoint));
      Vector3f ptInFront;
      Vector3f ptBehind;
      TransportTrajectory.getPivotEndPoints(nextWaypoint - currentWaypoint, out ptInFront, out ptBehind);
      Vector3f segEndPt1 = currentWaypoint + ptInFront;
      curve.AddSegment(lastControlPoint + (currentWaypoint - lastControlPoint) * controlPointsDistance, segEndPt1 + (currentWaypoint - segEndPt1) * controlPointsDistance, segEndPt1);
      if (!addStraightSegment)
        return;
      Vector3f segEndPt2 = nextWaypoint + ptBehind;
      if ((segEndPt1 - segEndPt2).IsNearZero())
        return;
      curve.AddStraightSegment(segEndPt2);
    }

    /// <summary>
    /// Computes end-point of pivot that are on the boundary of the pivot. Returned points are relative to the origin
    /// [0, 0, 0].
    /// </summary>
    /// <param name="directionToNext">Direction towards next pivot</param>
    /// <param name="ptInFront">Point that is at intersection of pivot boundary and direction vector.</param>
    /// <param name="ptBehind">Point that is at intersection of pivot boundary and negative direction vector.</param>
    private static void getPivotEndPoints(
      Vector3f directionToNext,
      out Vector3f ptInFront,
      out Vector3f ptBehind)
    {
      Assert.That<Vector3f>(directionToNext).IsNotNear(Vector3f.Zero, Fix32.EpsilonNear);
      Ray3f ray = new Ray3f(Vector3f.Zero, directionToNext);
      Percent tMin;
      int num1;
      Percent tMax;
      int num2;
      Assert.That<bool>(!ray.Direction.Xy.IsZero ? RayCaster.IntersectAabb(ray, new Aabb(new Vector3f(-Fix32.Half, -Fix32.Half, -Fix32.One), new Vector3f(Fix32.Half, Fix32.Half, Fix32.FromInt(10))), out tMin, out num2, out tMax, out num1) : RayCaster.IntersectAabb(ray, new Aabb(new Vector3f(-Fix32.One, -Fix32.One, -Fix32.Half), new Vector3f(Fix32.FromInt(10), Fix32.FromInt(10), Fix32.Half)), out tMin, out num1, out tMax, out num2)).IsTrue();
      Assert.That<Percent>(tMax).IsPositive();
      ptInFront = ray.GetPoint(tMax);
      ptBehind = ray.GetPoint(tMin);
    }

    /// <summary>
    /// Computes quaternion rotation based on position among a curve.
    /// </summary>
    private static TransportWaypointRotation ComputeRotation(
      CubicBezierCurve3f curve,
      int segment,
      Percent t)
    {
      Vector3f vector3f = curve.SampleSegmentDerivative(segment, t);
      AngleDegrees1f angleDegrees1f1;
      AngleDegrees1f angleDegrees1f2;
      if (vector3f.Normalized.Z.Abs().IsNear(Fix32.One, Fix32.Epsilon))
      {
        angleDegrees1f1 = AngleDegrees1f.Deg90;
        angleDegrees1f2 = AngleDegrees1f.Zero;
      }
      else
      {
        angleDegrees1f1 = vector3f.AngleBetween(vector3f.SetZ(Fix32.Zero));
        angleDegrees1f2 = vector3f.Xy.Angle;
      }
      if (vector3f.Z.IsPositive)
        angleDegrees1f1 = -angleDegrees1f1;
      return new TransportWaypointRotation(angleDegrees1f2.ToSlim(), angleDegrees1f1.ToSlim());
    }

    /// <summary>Returns start direction of requested pivot.</summary>
    public RelTile3i StartDirectionOf(int pivotIndex)
    {
      return pivotIndex != 0 ? this.Pivots[pivotIndex - 1] - this.Pivots[pivotIndex] : this.StartDirection;
    }

    /// <summary>Returns end direction of requested pivot.</summary>
    public RelTile3i EndDirectionOf(int pivotIndex)
    {
      return pivotIndex != this.Pivots.Length - 1 ? this.Pivots[pivotIndex + 1] - this.Pivots[pivotIndex] : this.EndDirection;
    }

    private ImmutableArray<TransportFlowIndicatorPose> computeFlowIndicatorsPoses()
    {
      if (this.TransportProto.Graphics.FlowIndicator.IsNone)
        return ImmutableArray<TransportFlowIndicatorPose>.Empty;
      Fix32 maxValue = this.TransportProto.Graphics.FlowIndicator.Value.PlacementGap.Value;
      Fix32 biasTowardEnds = TransportProto.Gfx.FlowIndicatorSpec.BIAS_TOWARD_ENDS;
      if (!this.TransportProto.Graphics.UseInstancedRendering)
        maxValue = Fix32.MaxValue;
      int num1 = 0;
      RelTile3i relTile3i;
      for (int index = 1; index < this.Pivots.Length; ++index)
      {
        int pivotSegmentIndex1 = this.PivotSegmentIndices[index - 1];
        int pivotSegmentIndex2 = this.PivotSegmentIndices[index];
        if (pivotSegmentIndex2 - pivotSegmentIndex1 > 1)
        {
          relTile3i = this.Pivots[index] - this.Pivots[index - 1];
          relTile3i = relTile3i.Signs;
          relTile3i = relTile3i.AbsValue;
          if (relTile3i.Sum == 1 && (this.Pivots[index] - this.Pivots[index - 1]).Z == 0)
          {
            int num2 = num1 + 1;
            Assert.That<int>(pivotSegmentIndex1 + 2).IsEqualTo(pivotSegmentIndex2);
            int segmentIndex = pivotSegmentIndex1 + 1;
            Assert.That<bool>(this.Curve.IsStraightSegment(segmentIndex)).IsTrue();
            Fix32 length = (this.Curve.SampleSegment64(segmentIndex, Percent.Zero) - this.Curve.SampleSegment64(segmentIndex, Percent.Hundred)).Length;
            num1 = num2 + (length / maxValue).ToIntFloored();
          }
        }
      }
      ImmutableArrayBuilder<TransportFlowIndicatorPose> immutableArrayBuilder = new ImmutableArrayBuilder<TransportFlowIndicatorPose>(num1);
      int i = 0;
      for (int index1 = 1; index1 < this.Pivots.Length; ++index1)
      {
        int pivotSegmentIndex3 = this.PivotSegmentIndices[index1 - 1];
        int pivotSegmentIndex4 = this.PivotSegmentIndices[index1];
        if (pivotSegmentIndex4 - pivotSegmentIndex3 > 1)
        {
          relTile3i = this.Pivots[index1] - this.Pivots[index1 - 1];
          relTile3i = relTile3i.Signs;
          relTile3i = relTile3i.AbsValue;
          if (relTile3i.Sum == 1 && (this.Pivots[index1] - this.Pivots[index1 - 1]).Z == 0)
          {
            if (i >= num1)
            {
              Log.Error("More pose indices than expected!");
              break;
            }
            Assert.That<int>(pivotSegmentIndex3 + 2).IsEqualTo(pivotSegmentIndex4);
            int num3 = pivotSegmentIndex3 + 1;
            Assert.That<bool>(this.Curve.IsStraightSegment(num3)).IsTrue();
            int num4 = 1 + ((this.Curve.SampleSegment64(num3, Percent.Zero) - this.Curve.SampleSegment64(num3, Percent.Hundred)).Length / maxValue).ToIntFloored();
            for (int index2 = 0; index2 < num4; ++index2)
            {
              if (i >= num1)
              {
                Log.Error("More pose indices than expected!");
                break;
              }
              Percent percent = Percent.FromRatio((Fix32) (index2 + 1) - biasTowardEnds, (Fix32) (num4 + 1) - biasTowardEnds * 2);
              immutableArrayBuilder[i] = new TransportFlowIndicatorPose(new Tile3f(this.Curve.SampleSegment64(num3, percent)), TransportTrajectory.ComputeRotation(this.Curve, num3, percent), percent, num3);
              ++i;
            }
          }
        }
      }
      Assert.That<int>(i).IsEqualTo(num1);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    /// <summary>
    /// Computes any missing start/end directions based on transport start/end direction.
    /// For a single-tile transport with no preferred directions, a direction along X axis is chosen.
    /// </summary>
    public static void ComputeStartAndEndDirections(
      ReadOnlyArraySlice<Tile3i> pivots,
      RelTile3i? startDirMaybe,
      RelTile3i? endDirMaybe,
      out RelTile3i startDirection,
      out RelTile3i endDirection)
    {
      RelTile2i relTile2i;
      if (startDirMaybe.HasValue)
        startDirection = startDirMaybe.Value;
      else if (pivots.Length >= 2 && (pivots.First.Xy - pivots.Second.Xy).IsNotZero)
      {
        ref RelTile3i local = ref startDirection;
        relTile2i = (pivots.First.Xy - pivots.Second.Xy).Signs;
        RelTile3i relTile3i = relTile2i.ExtendZ(0);
        local = relTile3i;
      }
      else
        startDirection = RelTile3i.Zero;
      if (endDirMaybe.HasValue)
        endDirection = endDirMaybe.Value;
      else if (pivots.Length >= 2)
      {
        relTile2i = pivots.Last.Xy - pivots.PreLast.Xy;
        if (relTile2i.IsNotZero)
        {
          ref RelTile3i local = ref endDirection;
          relTile2i = pivots.Last.Xy - pivots.PreLast.Xy;
          relTile2i = relTile2i.Signs;
          RelTile3i relTile3i = relTile2i.ExtendZ(0);
          local = relTile3i;
        }
        else
        {
          if (startDirection.IsZero)
          {
            startDirection = -RelTile3i.UnitX;
            endDirection = RelTile3i.UnitX;
          }
          else
            endDirection = startDirection.Z != 0 ? RelTile3i.UnitX : -startDirection;
          for (int index = pivots.Length - 1; index > 0; --index)
          {
            relTile2i = pivots[index].Xy - pivots[index - 1].Xy;
            if (relTile2i.IsNotZero)
            {
              ref RelTile3i local = ref endDirection;
              relTile2i = pivots[index].Xy - pivots[index - 1].Xy;
              relTile2i = relTile2i.Signs;
              RelTile3i relTile3i = relTile2i.ExtendZ(0);
              local = relTile3i;
              break;
            }
          }
          return;
        }
      }
      else
      {
        if (startDirection.IsZero)
        {
          startDirection = -RelTile3i.UnitX;
          endDirection = RelTile3i.UnitX;
          return;
        }
        if (startDirection.Z == 0)
        {
          endDirection = -startDirection;
          return;
        }
        endDirection = RelTile3i.UnitX;
        return;
      }
      if (!startDirection.IsZero)
        return;
      if (endDirection.Z == 0)
        startDirection = -endDirection;
      else
        startDirection = RelTile3i.UnitX;
    }

    private void computeWaypoints()
    {
      RelTile1f exactTrajectoryLength;
      this.m_waypoints = TransportTrajectory.ComputeWaypointsFromCurve(this.TransportProto, this.Curve, out this.m_curveSegmentWaypointIndices, out exactTrajectoryLength);
      if (this.m_waypoints.Length >= this.TransportProto.ProductSpacingWaypoints)
      {
        this.m_length = this.TransportProto.SpeedPerTick * (this.m_waypoints.Length - 1);
      }
      else
      {
        RelTile1f relTile1f = exactTrajectoryLength / (this.TransportProto.ProductSpacingWaypoints - 1);
        this.m_waypoints = TransportTrajectory.ComputeWaypointsFromCurve(this.TransportProto, this.Curve, out this.m_curveSegmentWaypointIndices, out RelTile1f _, new RelTile1f?(relTile1f));
        Assert.That<int>(this.m_waypoints.Length).IsEqualTo(this.TransportProto.ProductSpacingWaypoints);
        this.m_length = relTile1f * (this.m_waypoints.Length - 1);
      }
    }

    public static ImmutableArray<TransportWaypoint> ComputeWaypointsFromCurve(
      TransportProto transportProto,
      CubicBezierCurve3f curve,
      out ImmutableArray<int> curveSegmentWaypointIndices,
      out RelTile1f exactTrajectoryLength,
      RelTile1f? customTransportSpeed = null)
    {
      Lyst<TransportWaypoint> lyst = TransportTrajectory.s_waypointsTmp;
      if (lyst == null)
      {
        lyst = new Lyst<TransportWaypoint>(128, true);
        TransportTrajectory.s_waypointsTmp = lyst;
      }
      lyst.Clear();
      lyst.Add(new TransportWaypoint(new Tile3f(curve[0]), TransportTrajectory.ComputeRotation(curve, 0, Percent.Zero)));
      ImmutableArrayBuilder<int> immutableArrayBuilder = new ImmutableArrayBuilder<int>(curve.SegmentsCount + 1);
      RelTile1f zero = RelTile1f.Zero;
      RelTile1f expected = customTransportSpeed ?? transportProto.SpeedPerTick;
      int intCeiled = (2 / expected.Value).ToIntCeiled();
      Tile3f[] tile3fArray = new Tile3f[intCeiled];
      RelTile1f[] relTile1fArray = new RelTile1f[intCeiled];
      Percent[] percentArray = new Percent[intCeiled];
      int num = 0;
      for (int segmentsCount = curve.SegmentsCount; num < segmentsCount; ++num)
      {
        if (curve.IsStraightSegment(num))
        {
          TransportWaypointRotation rotation = TransportTrajectory.ComputeRotation(curve, num, Percent.Zero);
          Tile3f tile3f1 = new Tile3f(curve[num * 3]);
          Tile3f tile3f2 = new Tile3f(curve[num * 3 + 3]);
          RelTile1f relTile1f = new RelTile1f(tile3f1.DistanceTo(tile3f2));
          zero += relTile1f;
          while (zero >= expected)
          {
            zero -= expected;
            Percent t = Percent.FromRatio((relTile1f - zero).Value, relTile1f.Value);
            Tile3f position = tile3f1.Lerp(tile3f2, t);
            lyst.Add(new TransportWaypoint(position, rotation));
          }
        }
        else
        {
          for (int numerator = 0; numerator < tile3fArray.Length; ++numerator)
          {
            Percent t = Percent.FromRatio(numerator, tile3fArray.Length - 1);
            percentArray[numerator] = t;
            tile3fArray[numerator] = new Tile3f(curve.SampleSegment64(num, t));
            if (numerator > 0)
            {
              RelTile1f relTile1f = new RelTile1f(tile3fArray[numerator - 1].DistanceTo(tile3fArray[numerator]));
              relTile1fArray[numerator] = relTile1f;
            }
          }
          Assert.That<RelTile1f>(zero).IsLess(expected);
          for (int index = 1; index < tile3fArray.Length; ++index)
          {
            zero += relTile1fArray[index];
            while (zero >= expected)
            {
              zero -= expected;
              Percent t1 = Percent.FromRatio((relTile1fArray[index] - zero).Value, relTile1fArray[index].Value);
              Percent t2 = percentArray[index - 1].Lerp(percentArray[index], t1);
              lyst.Add(new TransportWaypoint(new Tile3f(curve.SampleSegment64(num, t2)), TransportTrajectory.ComputeRotation(curve, num, t2)));
            }
          }
        }
        immutableArrayBuilder[num + 1] = lyst.Count - 1;
        Assert.That<RelTile1f>(zero).IsWithinExcl(Fix32.Zero, expected.Value);
      }
      Tile3f position1 = new Tile3f(curve.SampleSegment64(curve.SegmentsCount - 1, 99.Percent()));
      TransportWaypointRotation rotation1 = TransportTrajectory.ComputeRotation(curve, curve.SegmentsCount - 1, 99.Percent());
      exactTrajectoryLength = (lyst.Count - 1) * expected + zero;
      if (zero < expected / 3)
      {
        lyst[lyst.Count - 1] = new TransportWaypoint(position1, rotation1);
      }
      else
      {
        lyst.Add(new TransportWaypoint(position1, rotation1));
        immutableArrayBuilder[curve.SegmentsCount] = lyst.Count - 1;
      }
      curveSegmentWaypointIndices = immutableArrayBuilder.GetImmutableArrayAndClear();
      return lyst.ToImmutableArrayAndClear();
    }

    public TransportTrajectory CreateCopyWithNewProto(TransportProto proto)
    {
      Assert.That<RelTile1i>(this.TransportProto.ZStepLength).IsEqualTo(proto.ZStepLength);
      Assert.That<bool>(this.TransportProto.CanGoUpDown).IsEqualTo<bool>(proto.CanGoUpDown);
      return new TransportTrajectory(proto, this.Curve, this.PivotSegmentIndices, this.Pivots, this.StartDirection, this.EndDirection);
    }

    /// <summary>
    /// Returns the lower index of a pivot surrounding transport segment at given coordinate, or index of the pivot
    /// itself. The <paramref name="isAtPivot" /> will be true when given position is a pivot and returned index is index of that pivot.
    /// Returns null when given coordinate is not on this transport.
    /// 
    /// This method is thread safe.
    /// </summary>
    public bool TryGetLowPivotIndexFor(Tile3i tile, out int index, out bool isAtPivot)
    {
      Tile3i start = this.Pivots.First;
      if (tile == start)
      {
        isAtPivot = true;
        index = 0;
        return true;
      }
      if (TransportTrajectory.s_tryGetLowPivotIndexForTilesTmp == null)
        TransportTrajectory.s_tryGetLowPivotIndexForTilesTmp = new Lyst<OccupiedTileRange>();
      Lyst<OccupiedTileRange> indexForTilesTmp = TransportTrajectory.s_tryGetLowPivotIndexForTilesTmp;
      int index1 = 1;
      int length = this.Pivots.Length;
      while (index1 < length)
      {
        Tile3i pivot = this.Pivots[index1];
        if (tile == pivot)
        {
          isAtPivot = true;
          index = index1;
          return true;
        }
        if ((pivot.Xy - start.Xy).LengthSqrInt != 1)
        {
          indexForTilesTmp.Clear();
          TransportHelper.ComputeOccupiedTilesForSegment(start, pivot, indexForTilesTmp);
          foreach (OccupiedTileRange occupiedTileRange in indexForTilesTmp)
          {
            if (!((Tile2i) occupiedTileRange.Position != tile.Xy) && tile.Height >= occupiedTileRange.From && tile.Height < occupiedTileRange.ToExcl)
            {
              isAtPivot = false;
              index = index1 - 1;
              return true;
            }
          }
        }
        ++index1;
        start = pivot;
      }
      isAtPivot = false;
      index = -1;
      return false;
    }

    public bool CanCutOut(
      Tile3i cutFromPosition,
      Tile3i cutToPosition,
      bool expandCutOverRampsInsteadOfFailing,
      bool createCutOutTrajectory,
      out CanCutOutTransportTrajResult result,
      out LocStrFormatted error,
      bool doNotCreateNonCutOutTrajs = false)
    {
      result = new CanCutOutTransportTrajResult();
      int index1;
      bool isAtPivot1;
      if (!this.TryGetLowPivotIndexFor(cutFromPosition, out index1, out isAtPivot1))
      {
        Log.Warning(string.Format("Cannot cut transport from position {0} that is not on the transport.", (object) cutFromPosition));
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
        return false;
      }
      int index2;
      bool isAtPivot2;
      if (!this.TryGetLowPivotIndexFor(cutToPosition, out index2, out isAtPivot2))
      {
        Log.Warning(string.Format("Cannot cut transport to position {0} that is not on the transport.", (object) cutToPosition));
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
        return false;
      }
      if (index2 < index1 || index2 == index1 && this.Pivots[index1].DistanceSqrTo(cutFromPosition) > this.Pivots[index2].DistanceSqrTo(cutToPosition))
      {
        Swap.Them<int>(ref index1, ref index2);
        Swap.Them<Tile3i>(ref cutFromPosition, ref cutToPosition);
        Swap.Them<bool>(ref isAtPivot1, ref isAtPivot2);
      }
      Tile3i pivot;
      bool flag1;
      ReadOnlyArraySlice<Tile3i> readOnlyArraySlice1;
      Direction903d direction1;
      int num1;
      RelTile2i relTile2i1;
      if (isAtPivot1 || this.Pivots[index1].Z != this.Pivots[index1 + 1].Z)
      {
        if (!expandCutOverRampsInsteadOfFailing && !isAtPivot1)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
          return false;
        }
        for (; index1 > 0; --index1)
        {
          if (this.Pivots[index1 - 1].Z != this.Pivots[index1].Z)
          {
            pivot = this.Pivots[index1 - 1];
            Tile2i xy1 = pivot.Xy;
            pivot = this.Pivots[index1];
            Tile2i xy2 = pivot.Xy;
            if (!(xy1 == xy2))
            {
              if (!expandCutOverRampsInsteadOfFailing)
              {
                error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
                return false;
              }
              continue;
            }
          }
          cutFromPosition = this.Pivots[index1];
          break;
        }
        flag1 = false;
        if (index1 == 0)
        {
          readOnlyArraySlice1 = ReadOnlyArraySlice<Tile3i>.Empty;
          direction1 = new Direction903d();
          num1 = 0;
        }
        else
        {
          RelTile3i relTile3i = this.Pivots[index1 - 1] - this.Pivots[index1];
          readOnlyArraySlice1 = this.Pivots.Slice(0, index1);
          direction1 = -relTile3i.ToDirection903d();
          num1 = index1;
          if (relTile3i.Sum.Abs() > 1)
          {
            Tile3i[] tile3iArray = new Tile3i[readOnlyArraySlice1.Length + 1];
            readOnlyArraySlice1.CopyTo(tile3iArray, 0);
            tile3iArray[readOnlyArraySlice1.Length] = cutFromPosition + relTile3i.Signs;
            readOnlyArraySlice1 = tile3iArray.AsSlice<Tile3i>();
          }
        }
      }
      else
      {
        Assert.That<int>(this.Pivots[index1].Z).IsEqualTo(cutFromPosition.Z);
        Tile2i xy3 = this.Pivots[index1].Xy;
        pivot = this.Pivots[index1 + 1];
        Tile2i xy4 = pivot.Xy;
        RelTile2i relTile2i2 = xy3 - xy4;
        readOnlyArraySlice1 = this.Pivots.Slice(0, index1 + 1);
        direction1 = -relTile2i2.ToDirection90().As3d;
        num1 = index1 + 1;
        flag1 = true;
        pivot = this.Pivots[index1];
        relTile2i1 = pivot.Xy - cutFromPosition.Xy;
        if (relTile2i1.Sum.Abs() > 1)
        {
          Tile3i[] tile3iArray = new Tile3i[readOnlyArraySlice1.Length + 1];
          readOnlyArraySlice1.CopyTo(tile3iArray, 0);
          tile3iArray[readOnlyArraySlice1.Length] = cutFromPosition.AddXy(relTile2i2.Signs);
          readOnlyArraySlice1 = tile3iArray.AsSlice<Tile3i>();
        }
      }
      bool flag2;
      ReadOnlyArraySlice<Tile3i> readOnlyArraySlice2;
      Direction903d direction2;
      int num2;
      if (isAtPivot2 || this.Pivots[index2].Z != this.Pivots[index2 + 1].Z)
      {
        if (!expandCutOverRampsInsteadOfFailing && !isAtPivot2)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
          return false;
        }
        for (; index2 + 1 < this.Pivots.Length; ++index2)
        {
          if (this.Pivots[index2].Z != this.Pivots[index2 + 1].Z)
          {
            pivot = this.Pivots[index2];
            Tile2i xy5 = pivot.Xy;
            pivot = this.Pivots[index2 + 1];
            Tile2i xy6 = pivot.Xy;
            if (!(xy5 == xy6))
            {
              if (!expandCutOverRampsInsteadOfFailing)
              {
                error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
                return false;
              }
              continue;
            }
          }
          cutToPosition = this.Pivots[index2];
          break;
        }
        flag2 = false;
        if (index2 + 1 >= this.Pivots.Length)
        {
          readOnlyArraySlice2 = ReadOnlyArraySlice<Tile3i>.Empty;
          direction2 = new Direction903d();
          num2 = this.Pivots.Length - 1;
        }
        else
        {
          RelTile3i relTile3i = this.Pivots[index2 + 1] - this.Pivots[index2];
          readOnlyArraySlice2 = this.Pivots.Slice(index2 + 1);
          direction2 = -relTile3i.ToDirection903d();
          num2 = index2;
          if (relTile3i.Sum.Abs() > 1)
          {
            Tile3i[] tile3iArray = new Tile3i[readOnlyArraySlice2.Length + 1];
            tile3iArray[0] = cutToPosition + relTile3i.Signs;
            readOnlyArraySlice2.CopyTo(tile3iArray, 1);
            readOnlyArraySlice2 = tile3iArray.AsSlice<Tile3i>();
          }
        }
      }
      else
      {
        Assert.That<int>(this.Pivots[index2].Z).IsEqualTo(cutToPosition.Z);
        pivot = this.Pivots[index2 + 1];
        Tile2i xy7 = pivot.Xy;
        pivot = this.Pivots[index2];
        Tile2i xy8 = pivot.Xy;
        RelTile2i relTile2i3 = xy7 - xy8;
        readOnlyArraySlice2 = this.Pivots.Slice(index2 + 1);
        direction2 = -relTile2i3.ToDirection90().As3d;
        num2 = index2;
        flag2 = true;
        pivot = this.Pivots[index2 + 1];
        relTile2i1 = pivot.Xy - cutToPosition.Xy;
        if (relTile2i1.Sum.Abs() > 1)
        {
          Tile3i[] tile3iArray = new Tile3i[readOnlyArraySlice2.Length + 1];
          tile3iArray[0] = cutToPosition.AddXy(relTile2i3.Signs);
          readOnlyArraySlice2.CopyTo(tile3iArray, 1);
          readOnlyArraySlice2 = tile3iArray.AsSlice<Tile3i>();
        }
      }
      Option<TransportTrajectory> startSubTransport = Option<TransportTrajectory>.None;
      if (readOnlyArraySlice1.IsNotEmpty && !doNotCreateNonCutOutTrajs)
      {
        TransportTrajectory trajectory;
        if (TransportTrajectory.TryCreateFromPivots(this.TransportProto, readOnlyArraySlice1.ToImmutableArray(), new RelTile3i?(this.StartDirection), new RelTile3i?(direction1.ToTileDirection()), out trajectory, out string _))
        {
          startSubTransport = (Option<TransportTrajectory>) trajectory;
        }
        else
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
          return false;
        }
      }
      Option<TransportTrajectory> endSubTransport = Option<TransportTrajectory>.None;
      if (readOnlyArraySlice2.IsNotEmpty && !doNotCreateNonCutOutTrajs)
      {
        TransportTrajectory trajectory;
        if (TransportTrajectory.TryCreateFromPivots(this.TransportProto, readOnlyArraySlice2.ToImmutableArray(), new RelTile3i?(direction2.ToTileDirection()), new RelTile3i?(this.EndDirection), out trajectory, out string _))
        {
          endSubTransport = (Option<TransportTrajectory>) trajectory;
        }
        else
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
          return false;
        }
      }
      Option<TransportTrajectory> cutOutSubTransport = Option<TransportTrajectory>.None;
      if (createCutOutTrajectory)
      {
        int count = num2 - num1 + 1;
        Assert.That<int>(count).IsNotNegative();
        ReadOnlyArraySlice<Tile3i> readOnlyArraySlice3;
        if (flag1)
        {
          Tile3i[] tile3iArray;
          if (flag2)
          {
            if (cutFromPosition == cutToPosition)
            {
              Assert.That<int>(count).IsZero();
              tile3iArray = new Tile3i[1]{ cutFromPosition };
            }
            else
            {
              tile3iArray = new Tile3i[count + 2];
              tile3iArray[0] = cutFromPosition;
              this.Pivots.CopyTo(tile3iArray, 1, num1, count);
              tile3iArray[tile3iArray.Length - 1] = cutToPosition;
            }
          }
          else
          {
            tile3iArray = new Tile3i[count + 1];
            tile3iArray[0] = cutFromPosition;
            this.Pivots.CopyTo(tile3iArray, 1, num1, count);
          }
          readOnlyArraySlice3 = tile3iArray.AsSlice<Tile3i>();
        }
        else if (flag2)
        {
          Tile3i[] tile3iArray = new Tile3i[count + 1];
          this.Pivots.CopyTo(tile3iArray, 0, num1, count);
          tile3iArray[tile3iArray.Length - 1] = cutToPosition;
          readOnlyArraySlice3 = tile3iArray.AsSlice<Tile3i>();
        }
        else
          readOnlyArraySlice3 = this.Pivots.Slice(num1, count);
        TransportTrajectory trajectory;
        string error1;
        if (TransportTrajectory.TryCreateFromPivots(this.TransportProto, readOnlyArraySlice3.ToImmutableArray(), new RelTile3i?(readOnlyArraySlice1.IsNotEmpty ? (-direction1).ToTileDirection() : this.StartDirection), new RelTile3i?(readOnlyArraySlice2.IsNotEmpty ? (-direction2).ToTileDirection() : this.EndDirection), out trajectory, out error1))
        {
          cutOutSubTransport = (Option<TransportTrajectory>) trajectory;
        }
        else
        {
          Log.Warning("Failed to create cut-out sub-trajectory after transport cut: " + error1);
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
          return false;
        }
      }
      error = LocStrFormatted.Empty;
      result = new CanCutOutTransportTrajResult(startSubTransport, cutOutSubTransport, endSubTransport);
      return true;
    }

    public void GetSubTrajectoriesInArea(
      RectangleTerrainArea2i area,
      Lyst<TransportTrajectory> subTrajectories,
      out bool entireTrajectoryIsInArea)
    {
      if (this.Pivots.IsEmpty)
      {
        entireTrajectoryIsInArea = false;
      }
      else
      {
        bool isInArea = area.ContainsTile(this.Pivots.First.Xy);
        if (this.Pivots.Length == 1)
        {
          entireTrajectoryIsInArea = isInArea;
        }
        else
        {
          Tile3i areaEnterPosition = this.Pivots.First;
          Tile3i lastAreaPosition = this.Pivots.First;
          Tile2i? waitTillCutEnd = new Tile2i?();
          Tile3i tile3i = this.Pivots.First;
          int index = 1;
          while (index < this.Pivots.Length)
          {
            Tile3i pivot = this.Pivots[index];
            if (tile3i.X == pivot.X)
            {
              int self = pivot.Y - tile3i.Y;
              int scale = self.Abs();
              int num = self.Sign();
              int t = 0;
              int y = tile3i.Y;
              while (t < scale)
              {
                checkTile(new Tile3i(pivot.X, y, tile3i.Z.Lerp(pivot.Z, (long) t, (long) scale)));
                ++t;
                y += num;
              }
            }
            else
            {
              int self = pivot.X - tile3i.X;
              int scale = self.Abs();
              int num = self.Sign();
              int t = 0;
              int x = tile3i.X;
              while (t < scale)
              {
                checkTile(new Tile3i(x, pivot.Y, tile3i.Z.Lerp(pivot.Z, (long) t, (long) scale)));
                ++t;
                x += num;
              }
            }
            ++index;
            tile3i = pivot;
          }
          checkTile(this.Pivots.Last);
          if (isInArea)
          {
            if (areaEnterPosition == this.Pivots.First)
            {
              entireTrajectoryIsInArea = true;
            }
            else
            {
              addCut(areaEnterPosition, this.Pivots.Last);
              entireTrajectoryIsInArea = false;
            }
          }
          else
            entireTrajectoryIsInArea = false;

          void addCut(Tile3i from, Tile3i to)
          {
            CanCutOutTransportTrajResult result;
            if (!this.CanCutOut(from, to, true, true, out result, out LocStrFormatted _, true) || !result.CutOutSubTransport.HasValue)
              return;
            subTrajectories.Add(result.CutOutSubTransport.Value);
            Tile2i xy = result.CutOutSubTransport.Value.Pivots.Last.Xy;
            if (!(xy != to.Xy))
              return;
            waitTillCutEnd = new Tile2i?(xy);
          }

          void checkTile(Tile3i position)
          {
            if (waitTillCutEnd.HasValue)
            {
              if (!(waitTillCutEnd.Value == position.Xy))
                return;
              waitTillCutEnd = new Tile2i?();
            }
            else if (isInArea == area.ContainsTile(position.Xy))
            {
              lastAreaPosition = position;
            }
            else
            {
              isInArea = !isInArea;
              if (isInArea)
                areaEnterPosition = lastAreaPosition = position;
              else
                addCut(areaEnterPosition, lastAreaPosition);
            }
          }
        }
      }
    }

    public bool IsInArea(RectangleTerrainArea2i area)
    {
      if (this.Pivots.IsEmpty || area.Size.X <= 0 || area.Size.Y <= 0)
        return false;
      if (area.ContainsTile(this.Pivots.First.Xy))
        return true;
      if (this.Pivots.Length == 1)
        return false;
      for (int index = 1; index < this.Pivots.Length; ++index)
      {
        Tile2i xy1 = this.Pivots[index].Xy;
        if (area.ContainsTile(xy1))
          return true;
        Tile2i xy2 = this.Pivots[index - 1].Xy;
        if (xy2.X == xy1.X)
        {
          int self = xy1.Y - xy2.Y;
          int num1 = self.Abs();
          if (num1 > area.Size.Y)
          {
            int num2 = num1 / area.Size.Y;
            int num3 = self.Sign() * area.Size.Y;
            int num4 = 0;
            int y = xy2.Y + num3;
            while (num4 < num2)
            {
              if (area.ContainsTile(new Tile2i(xy1.X, y)))
                return true;
              ++num4;
              y += num3;
            }
          }
        }
        else
        {
          int self = xy1.X - xy2.X;
          int num5 = self.Abs();
          if (num5 > area.Size.X)
          {
            int num6 = num5 / area.Size.X;
            int num7 = self.Sign() * area.Size.X;
            int num8 = 0;
            int x = xy2.X + num7;
            while (num8 < num6)
            {
              if (area.ContainsTile(new Tile2i(x, xy1.Y)))
                return true;
              ++num8;
              x += num7;
            }
          }
        }
      }
      return false;
    }

    public TransportTrajectory OffsetBy(RelTile3i offset)
    {
      return new TransportTrajectory(this.TransportProto, this.Pivots.Map<Tile3i, RelTile3i>(offset, (Func<Tile3i, RelTile3i, Tile3i>) ((x, o) => x + o)), this.StartDirection, this.EndDirection);
    }

    public static void Serialize(TransportTrajectory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TransportTrajectory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TransportTrajectory.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      RelTile3i.Serialize(this.EndDirection, writer);
      ImmutableArray<Tile3i>.Serialize(this.Pivots, writer);
      RelTile3i.Serialize(this.StartDirection, writer);
      writer.WriteGeneric<TransportProto>(this.TransportProto);
    }

    public static TransportTrajectory Deserialize(BlobReader reader)
    {
      TransportTrajectory transportTrajectory;
      if (reader.TryStartClassDeserialization<TransportTrajectory>(out transportTrajectory))
        reader.EnqueueDataDeserialization((object) transportTrajectory, TransportTrajectory.s_deserializeDataDelayedAction);
      return transportTrajectory;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<TransportTrajectory>(this, "EndDirection", (object) RelTile3i.Deserialize(reader));
      reader.SetField<TransportTrajectory>(this, "Pivots", (object) ImmutableArray<Tile3i>.Deserialize(reader));
      reader.SetField<TransportTrajectory>(this, "StartDirection", (object) RelTile3i.Deserialize(reader));
      reader.SetField<TransportTrajectory>(this, "TransportProto", (object) reader.ReadGenericAs<TransportProto>());
    }

    static TransportTrajectory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportTrajectory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TransportTrajectory) obj).SerializeData(writer));
      TransportTrajectory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TransportTrajectory) obj).DeserializeData(reader));
    }
  }
}
