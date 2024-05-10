// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportsConstructionHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Helper class for computing how a new transport can be joined with existing ones. Only trajectory validation
  /// is performed by this class. No collision, terrain, or pillars validation is done.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportsConstructionHelper
  {
    [ThreadStatic]
    private static Lyst<PillarLayerSpec> s_pillarLayersTmp;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly LazyResolve<TransportsManager> m_transportsManager;
    private readonly ProtosDb m_protosDb;
    private readonly IIoPortsManager m_portsManager;
    private readonly TerrainManager m_terrainManager;
    private readonly Lyst<Tile3i> m_tilesTmp;
    private BitMap m_supportedTilesTmp;

    public TransportsConstructionHelper(
      TerrainOccupancyManager occupancyManager,
      LazyResolve<TransportsManager> transportsManager,
      ProtosDb protosDb,
      IIoPortsManager portsManager,
      TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tilesTmp = new Lyst<Tile3i>(true);
      this.m_supportedTilesTmp = new BitMap(64);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_occupancyManager = occupancyManager;
      this.m_transportsManager = transportsManager;
      this.m_protosDb = protosDb;
      this.m_portsManager = portsManager;
      this.m_terrainManager = terrainManager;
    }

    public bool CanBuildOrJoinTransport(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      IReadOnlySet<Tile2i> pillarHints,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      out CanBuildTransportResult result,
      out LocStrFormatted error,
      bool ignorePillars = false,
      Predicate<Transport> ignoreForJoining = null,
      bool disallowMiniZipAtStart = false,
      bool disallowMiniZipAtEnd = false,
      bool skipExtraPillarsForBetterVisuals = false)
    {
      Tile3i tile3i;
      if (pivots.Length >= 2)
      {
        tile3i = pivots.First;
        Tile2i xy1 = tile3i.Xy;
        tile3i = pivots.Second;
        Tile2i xy2 = tile3i.Xy;
        if (xy1 == xy2)
          disallowMiniZipAtStart = true;
        tile3i = pivots.Last;
        Tile2i xy3 = tile3i.Xy;
        tile3i = pivots.PreLast;
        Tile2i xy4 = tile3i.Xy;
        if (xy3 == xy4)
          disallowMiniZipAtEnd = true;
      }
      Option<TransportTrajectory> newTrajectory;
      bool wasReversed;
      CanPlaceMiniZipperAtResult? miniZipJoinResultAtStart;
      CanPlaceMiniZipperAtResult? miniZipJoinResultAtEnd;
      CanChangeDirectionResult? changeDirectionNearStart;
      CanChangeDirectionResult? changeDirectionNearEnd;
      bool flag = this.canBuildOrJoinTransport(proto, pivots.AsSlice, disablePortSnapping, startDirection, endDirection, out newTrajectory, out wasReversed, out miniZipJoinResultAtStart, out miniZipJoinResultAtEnd, out changeDirectionNearStart, out changeDirectionNearEnd, out error, ignoreForJoining, disallowMiniZipAtStart, disallowMiniZipAtEnd);
      if (wasReversed)
        Swap.Them<bool>(ref disallowMiniZipAtStart, ref disallowMiniZipAtEnd);
      AssetValue newTransportValue = AssetValue.Empty;
      Option<IoPort> a = Option<IoPort>.None;
      Option<IoPort> b1 = Option<IoPort>.None;
      ImmutableArray<Tile3i> supportedTiles = ImmutableArray<Tile3i>.Empty;
      MiniZipperAtResult? result1 = new MiniZipperAtResult?();
      MiniZipperAtResult? nullable = new MiniZipperAtResult?();
      MiniZipperAtResult? b2;
      if (newTrajectory.HasValue)
      {
        TransportTrajectory trajectory = newTrajectory.Value;
        if (trajectory.Pivots.Length == 1)
        {
          if (!miniZipJoinResultAtStart.HasValue && !miniZipJoinResultAtEnd.HasValue && !disallowMiniZipAtStart && !disallowMiniZipAtEnd && this.m_transportsManager.Value.IsMiniZipperPlacementNeeded(proto, trajectory.Pivots.First, out result1))
            newTrajectory = Option<TransportTrajectory>.None;
          else
            result1 = new MiniZipperAtResult?();
          b2 = new MiniZipperAtResult?();
        }
        else
        {
          MiniZipperAtResult? result2;
          ReadOnlyArraySlice<Tile3i> newPivots1;
          LocStrFormatted error1;
          string error2;
          if (!miniZipJoinResultAtStart.HasValue && !disallowMiniZipAtStart && this.m_transportsManager.Value.IsMiniZipperPlacementNeeded(proto, trajectory.Pivots.First, out result2) && TransportHelper.TryCutFirstTileFromPivots(trajectory.Pivots.AsSlice, out newPivots1, out error1) && TransportTrajectory.TryCreateFromPivots(proto, newPivots1.ToImmutableArray(), new RelTile3i?(trajectory.Pivots.First - newPivots1.First), new RelTile3i?(trajectory.EndDirection), out trajectory, out error2))
          {
            newTrajectory = (Option<TransportTrajectory>) trajectory;
            result1 = result2;
          }
          else
            result1 = new MiniZipperAtResult?();
          if (!miniZipJoinResultAtEnd.HasValue && !disallowMiniZipAtEnd)
          {
            if (result1.HasValue)
            {
              tile3i = pivots.First;
              if (tile3i.DistanceSqrTo(trajectory.Pivots.Last) <= 1L)
                goto label_21;
            }
            MiniZipperAtResult? result3;
            ReadOnlyArraySlice<Tile3i> newPivots2;
            if (this.m_transportsManager.Value.IsMiniZipperPlacementNeeded(proto, trajectory.Pivots.Last, out result3) && TransportHelper.TryCutLastTileFromPivots(trajectory.Pivots.AsSlice, out newPivots2, out error1) && TransportTrajectory.TryCreateFromPivots(proto, newPivots2.ToImmutableArray(), new RelTile3i?(trajectory.StartDirection), new RelTile3i?(trajectory.Pivots.Last - newPivots2.Last), out trajectory, out error2))
            {
              newTrajectory = (Option<TransportTrajectory>) trajectory;
              b2 = result3;
              goto label_22;
            }
          }
label_21:
          b2 = new MiniZipperAtResult?();
        }
label_22:
        ImmutableArray<Tile3i> pivots1 = trajectory.Pivots;
        newTransportValue = proto.GetPriceFor(pivots1);
        if (!result1.HasValue)
        {
          Direction903d direction903d = trajectory.StartDirection.ToDirection903d();
          IoPort port;
          if (this.m_portsManager.TryGetPortAt(pivots1.First + direction903d.ToTileDirection(), -direction903d, proto.PortsShape, out port))
            a = (Option<IoPort>) port;
        }
        if (!b2.HasValue)
        {
          Direction903d direction903d = trajectory.EndDirection.ToDirection903d();
          IoPort port;
          if (this.m_portsManager.TryGetPortAt(pivots1.Last + direction903d.ToTileDirection(), -direction903d, proto.PortsShape, out port))
            b1 = (Option<IoPort>) port;
        }
        if (!ignorePillars && !this.TryFindPillarsForTrajectory(trajectory, pillarHints, out supportedTiles, out int _, out int _, skipExtraPillarsForBetterVisuals))
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__NoPillars;
          flag = false;
        }
        if (!this.CheckTerrainCollision(trajectory))
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__TerrainCollision;
          flag = false;
        }
      }
      else
      {
        result1 = new MiniZipperAtResult?();
        b2 = new MiniZipperAtResult?();
      }
      if (wasReversed)
      {
        Swap.Them<Option<IoPort>>(ref a, ref b1);
        Swap.Them<MiniZipperAtResult?>(ref result1, ref b2);
      }
      result = new CanBuildTransportResult(pivots, startDirection, endDirection, newTrajectory, wasReversed, newTransportValue, supportedTiles, result1, b2, miniZipJoinResultAtStart, miniZipJoinResultAtEnd, changeDirectionNearStart, changeDirectionNearEnd, a, b1);
      return flag && (!newTrajectory.HasValue || this.validateConnectedPorts(newTrajectory.Value, proto.PortsShape, out error));
    }

    private bool validateConnectedPorts(
      TransportTrajectory newTraj,
      IoPortShapeProto portShape,
      out LocStrFormatted error)
    {
      if (getPortTypeFor(newTraj.Pivots.First, newTraj.StartDirection) == IoPortType.Input)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__IncompatiblePortAtStart;
        return false;
      }
      if (getPortTypeFor(newTraj.Pivots.Last, newTraj.EndDirection) == IoPortType.Output)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__IncompatiblePortAtEnd;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;

      IoPortType getPortTypeFor(Tile3i position, RelTile3i direction)
      {
        IoPort port;
        bool flag1 = this.m_portsManager.TryGetPortAt(position + direction, -direction.ToDirection903d(), portShape, out port);
        if (flag1)
        {
          bool flag2;
          switch (port.OwnerEntity.ConstructionState)
          {
            case ConstructionState.PendingDeconstruction:
            case ConstructionState.InDeconstruction:
              flag2 = true;
              break;
            default:
              flag2 = false;
              break;
          }
          flag1 = !flag2;
        }
        return !flag1 ? IoPortType.Any : port.Type;
      }
    }

    /// <summary>
    /// Checks whether a transport with given parameters is buildable. This also considers joining the transport
    /// with other transports via mini-zipper or changing existing transports end direction.
    /// </summary>
    private bool canBuildOrJoinTransport(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      bool disablePortSnapping,
      Direction903d? startDirection,
      Direction903d? endDirection,
      out Option<TransportTrajectory> newTrajectory,
      out bool wasReversed,
      out CanPlaceMiniZipperAtResult? miniZipJoinResultAtStart,
      out CanPlaceMiniZipperAtResult? miniZipJoinResultAtEnd,
      out CanChangeDirectionResult? changeDirectionNearStart,
      out CanChangeDirectionResult? changeDirectionNearEnd,
      out LocStrFormatted error,
      Predicate<Transport> ignoreForJoining = null,
      bool disallowMiniZipAtStart = false,
      bool disallowMiniZipAtEnd = false)
    {
      newTrajectory = Option<TransportTrajectory>.None;
      wasReversed = false;
      miniZipJoinResultAtStart = new CanPlaceMiniZipperAtResult?();
      miniZipJoinResultAtEnd = new CanPlaceMiniZipperAtResult?();
      changeDirectionNearStart = new CanChangeDirectionResult?();
      changeDirectionNearEnd = new CanChangeDirectionResult?();
      if (pivots.IsEmpty)
      {
        Log.InfoDebug("Transport with no pivots");
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        return false;
      }
      if (pivots.Length >= 2)
      {
        for (int index = 1; index < pivots.Length; ++index)
        {
          RelTile2i relTile2i = pivots[index - 1].Xy - pivots[index].Xy;
          if (relTile2i.X != 0 && relTile2i.Y != 0)
          {
            Log.InfoDebug("Transport (deltaX != 0) && (deltaY != 0)");
            error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
            return false;
          }
        }
      }
      foreach (Tile3i pivot in pivots)
      {
        if (this.m_terrainManager.IsOffLimitsOrInvalid(pivot.Tile2i))
        {
          error = (LocStrFormatted) TrCore.AdditionError__OutsideOfMap;
          return false;
        }
      }
      Transport entity1 = (Transport) null;
      Transport entity2 = (Transport) null;
      this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(pivots.First, out entity1, this.m_transportsManager.Value.IgnorePillarsPredicate);
      if (disallowMiniZipAtStart && entity1 != null)
      {
        if (entity1.StartInputPort.Position == pivots.First)
        {
          if (entity1.StartInputPort.IsConnected)
            entity1 = (Transport) null;
        }
        else if (entity1.EndOutputPort.Position == pivots.First)
        {
          if (entity1.EndOutputPort.IsConnected)
            entity1 = (Transport) null;
        }
        else
          entity1 = (Transport) null;
      }
      this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(pivots.Last, out entity2, this.m_transportsManager.Value.IgnorePillarsPredicate);
      if (disallowMiniZipAtEnd && entity2 != null)
      {
        if (entity2.StartInputPort.Position == pivots.Last)
        {
          if (entity2.StartInputPort.IsConnected)
            entity2 = (Transport) null;
        }
        else if (entity2.EndOutputPort.Position == pivots.Last)
        {
          if (entity2.EndOutputPort.IsConnected)
            entity2 = (Transport) null;
        }
        else
          entity2 = (Transport) null;
      }
      if (ignoreForJoining != null)
      {
        if (entity1 != null && ignoreForJoining(entity1))
          entity1 = (Transport) null;
        if (entity2 != null && ignoreForJoining(entity2))
          entity2 = (Transport) null;
      }
      bool flag1 = entity1 != null;
      bool canBeReversedInTheFuture1;
      if (flag1)
      {
        switch (entity1.ConstructionState)
        {
          case ConstructionState.PendingDeconstruction:
          case ConstructionState.InDeconstruction:
          case ConstructionState.Deconstructed:
            canBeReversedInTheFuture1 = true;
            break;
          default:
            canBeReversedInTheFuture1 = false;
            break;
        }
        flag1 = canBeReversedInTheFuture1;
      }
      if (flag1)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      bool flag2 = entity2 != null;
      if (flag2)
      {
        switch (entity2.ConstructionState)
        {
          case ConstructionState.PendingDeconstruction:
          case ConstructionState.InDeconstruction:
          case ConstructionState.Deconstructed:
            canBeReversedInTheFuture1 = true;
            break;
          default:
            canBeReversedInTheFuture1 = false;
            break;
        }
        flag2 = canBeReversedInTheFuture1;
      }
      if (flag2)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      if (entity2 == null)
      {
        if (entity1 != null)
          return this.CanJoinTo(proto, pivots, startDirection, endDirection, entity1, true, true, disablePortSnapping, out wasReversed, out canBeReversedInTheFuture1, out newTrajectory, out miniZipJoinResultAtStart, out changeDirectionNearStart, out error);
        TransportTrajectory trajectory;
        if (!this.m_transportsManager.Value.TryCreateTrajectorySnapToPorts(proto, pivots, startDirection, endDirection, true, disablePortSnapping, out trajectory, out wasReversed, out error))
          return false;
        newTrajectory = (Option<TransportTrajectory>) trajectory;
        return true;
      }
      if (entity1 == null)
        return this.CanJoinTo(proto, pivots, startDirection, endDirection, entity2, false, true, disablePortSnapping, out wasReversed, out canBeReversedInTheFuture1, out newTrajectory, out miniZipJoinResultAtEnd, out changeDirectionNearEnd, out error);
      if (entity1 == entity2)
      {
        if (pivots.Length == 1)
        {
          if ((Proto) proto.PortsShape != (Proto) entity1.Prototype.PortsShape)
          {
            error = (LocStrFormatted) TrCore.TrAdditionError__TypesNoMatch;
            return false;
          }
          if (entity1.Trajectory.Pivots.Length == 1)
          {
            error = LocStrFormatted.Empty;
            return true;
          }
          if (entity1.StartInputPort.Position == pivots.First)
          {
            if (entity1.StartInputPort.IsNotConnected)
            {
              error = LocStrFormatted.Empty;
              return true;
            }
            if (entity1.StartInputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__NoMiniZipper;
              return false;
            }
          }
          if (entity1.EndOutputPort.Position == pivots.First)
          {
            if (entity1.EndOutputPort.IsNotConnected)
            {
              error = LocStrFormatted.Empty;
              return true;
            }
            if (entity1.EndOutputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__NoMiniZipper;
              return false;
            }
          }
          CanPlaceMiniZipperAtResult result;
          if (!this.CanPlaceMiniZipperAt(entity1, pivots.First, out result, out error))
            return false;
          miniZipJoinResultAtStart = new CanPlaceMiniZipperAtResult?(result);
          return true;
        }
        error = (LocStrFormatted) TrCore.TrAdditionError__Loop;
        return false;
      }
      Assert.That<int>(pivots.Length).IsGreaterOrEqual(2);
      bool canBeReversedInTheFuture2;
      if (!this.CanJoinTo(proto, pivots, startDirection, endDirection, entity1, true, true, disablePortSnapping, out wasReversed, out canBeReversedInTheFuture2, out newTrajectory, out miniZipJoinResultAtStart, out changeDirectionNearStart, out error))
        return false;
      if (newTrajectory.IsNone)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
        return false;
      }
      Direction903d direction903d = newTrajectory.Value.StartDirection.ToDirection903d();
      bool wasReversed1;
      bool flag3 = this.CanJoinTo(proto, newTrajectory.Value.Pivots.AsSlice, new Direction903d?(direction903d), new Direction903d?(), entity2, wasReversed, canBeReversedInTheFuture2, disablePortSnapping, out wasReversed1, out canBeReversedInTheFuture1, out newTrajectory, out miniZipJoinResultAtEnd, out changeDirectionNearEnd, out error);
      Assert.That<bool>(wasReversed & wasReversed1).IsFalse("Transport was reversed twice?");
      wasReversed ^= wasReversed1;
      if (!flag3 || !miniZipJoinResultAtStart.HasValue || !miniZipJoinResultAtEnd.HasValue || miniZipJoinResultAtStart.Value.CutOutResult.CutOutPosition.ManhattanDistanceTo(miniZipJoinResultAtEnd.Value.CutOutResult.CutOutPosition) > 1)
        return flag3;
      error = TrCore.TrAdditionError__TooCloseToOtherMiniZipper.Format(miniZipJoinResultAtStart.Value.ZipperProto.Strings.Name.TranslatedString);
      return false;
    }

    public bool CheckTerrainCollision(TransportTrajectory trajectory)
    {
      foreach (OccupiedTileRange occupiedTile in trajectory.OccupiedTiles)
      {
        HeightTilesI nonCollidingHeight = TransportHelper.GetLowestNonCollidingHeight(this.m_terrainManager[(Tile2i) occupiedTile.Position]);
        if (occupiedTile.From < nonCollidingHeight)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Determines whether the given pivots can be joined to the given existing transport either at start
    /// (of the pivots array) or at the end (based on <paramref name="atStart" />).
    /// This assumes that the transports are overlapping at start/end.
    /// 
    /// There are two cases on connection:
    /// 1) Connection via mini-zipper. In this case the <paramref name="miniZipJoinResult" /> will contain details
    /// of the split of the existing transport and the <paramref name="newTraj" /> will contain the new trajectory
    /// which will be shorter by the one tile where mini-zipper was placed.
    /// 2) Soft-join with neighboring transport by orienting current and existing transport's directions towards
    /// each other so that they will be merged once they are both built. This happens when they meet at the end.
    /// In this case the <paramref name="changeDirectionResult" /> will contain details about change of direction of
    /// the existing transport and the <paramref name="newTraj" /> will contain the new trajectory which will be
    /// shorter by the one tile where the transport overlap.
    /// 
    /// When false is returned, <paramref name="error" /> will contain useful info about the error.
    /// </summary>
    internal bool CanJoinTo(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      Direction903d? startDirection,
      Direction903d? endDirection,
      Transport existingTransport,
      bool atStart,
      bool canBeReversedNow,
      bool disablePortSnapping,
      out bool wasReversed,
      out bool canBeReversedInTheFuture,
      out Option<TransportTrajectory> newTraj,
      out CanPlaceMiniZipperAtResult? miniZipJoinResult,
      out CanChangeDirectionResult? changeDirectionResult,
      out LocStrFormatted error)
    {
      newTraj = Option<TransportTrajectory>.None;
      miniZipJoinResult = new CanPlaceMiniZipperAtResult?();
      changeDirectionResult = new CanChangeDirectionResult?();
      canBeReversedInTheFuture = true;
      wasReversed = false;
      error = LocStrFormatted.Empty;
      string error1;
      if (pivots.Length == 1)
      {
        if (endDirection.HasValue && existingTransport.EndPosition == pivots.First && this.CanChangeDirectionOf(existingTransport, endDirection.Value, false, out changeDirectionResult) || startDirection.HasValue && existingTransport.StartPosition == pivots.First && this.CanChangeDirectionOf(existingTransport, startDirection.Value, true, out changeDirectionResult))
          return true;
        CanPlaceMiniZipperAtResult result;
        if (this.CanPlaceMiniZipperAt(existingTransport, pivots.First, out result, out error))
        {
          miniZipJoinResult = new CanPlaceMiniZipperAtResult?(result);
          return true;
        }
        TransportTrajectory trajectory;
        if (TransportTrajectory.TryCreateFromPivots(proto, pivots.ToImmutableArray(), startDirection.HasValue ? new RelTile3i?(startDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), endDirection.HasValue ? new RelTile3i?(endDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), out trajectory, out error1))
          newTraj = (Option<TransportTrajectory>) trajectory;
        return false;
      }
      Assert.That<int>(pivots.Length).IsGreaterOrEqual(2);
      Tile3i? nullable1 = new Tile3i?();
      Direction903d? nullable2 = new Direction903d?();
      bool atStart1 = false;
      ReadOnlyArraySlice<Tile3i> newPivots;
      Direction903d direction903d;
      Direction903d? a;
      Direction903d? b;
      if (atStart)
      {
        if (!TransportHelper.TryCutFirstTileFromPivots(pivots, out newPivots, out error))
          return false;
        RelTile3i resolvedStartDir = pivots.First - pivots.Second;
        direction903d = resolvedStartDir.ToDirection903d();
        a = new Direction903d?(direction903d);
        b = endDirection;
        if (existingTransport.Trajectory.Pivots.Length == 1)
        {
          if ((Proto) existingTransport.Prototype != (Proto) proto)
          {
            error = (LocStrFormatted) TrCore.TrAdditionError__TypesNoMatch;
            return false;
          }
          if (existingTransport.StartInputPort.IsConnected && existingTransport.EndOutputPort.IsConnected)
          {
            nullable1 = new Tile3i?(pivots.First);
          }
          else
          {
            if (!b.HasValue)
            {
              RelTile3i resolvedEndDir;
              this.m_transportsManager.Value.ComputeTrajDirectionsSnapToPorts(proto, newPivots, a, new Direction903d?(), out resolvedStartDir, out resolvedEndDir, disablePortSnapping);
              b = new Direction903d?(resolvedEndDir.ToDirection903d());
            }
            IoPort port;
            if (canBeReversedNow && this.m_portsManager.TryGetPortAt(newPivots.Last + b.Value.ToTileDirection(), -b.Value, proto.PortsShape, out port) && port.Type == IoPortType.Output)
            {
              nullable2 = new Direction903d?(-direction903d);
              atStart1 = true;
              canBeReversedInTheFuture = false;
              wasReversed = true;
            }
            else
            {
              nullable2 = new Direction903d?(-direction903d);
              atStart1 = false;
              canBeReversedInTheFuture = false;
            }
          }
        }
        else if (existingTransport.EndPosition == pivots.First)
        {
          if (existingTransport.EndOutputPort.IsConnected || (Proto) existingTransport.Prototype != (Proto) proto)
          {
            if (direction903d.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            if (existingTransport.EndOutputPort.IsConnected && existingTransport.EndOutputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            nullable1 = new Tile3i?(pivots.First);
          }
          else
          {
            nullable2 = new Direction903d?(-direction903d);
            atStart1 = false;
            canBeReversedInTheFuture = false;
          }
        }
        else if (existingTransport.StartPosition == pivots.First)
        {
          if (existingTransport.StartInputPort.IsConnected || (Proto) existingTransport.Prototype != (Proto) proto)
          {
            if (direction903d.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            if (existingTransport.StartInputPort.IsConnected && existingTransport.StartInputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            nullable1 = new Tile3i?(pivots.First);
          }
          else
          {
            if (!canBeReversedNow)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__IncompatibleDirection;
              return false;
            }
            nullable2 = new Direction903d?(-direction903d);
            atStart1 = true;
            canBeReversedInTheFuture = false;
            wasReversed = true;
          }
        }
        else
        {
          if (direction903d.DirectionVector.Z != 0)
          {
            error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
            return false;
          }
          nullable1 = new Tile3i?(pivots.First);
        }
      }
      else
      {
        if (!TransportHelper.TryCutLastTileFromPivots(pivots, out newPivots, out error))
          return false;
        RelTile3i resolvedEndDir = pivots.Last - pivots.PreLast;
        direction903d = resolvedEndDir.ToDirection903d();
        a = startDirection;
        b = new Direction903d?(direction903d);
        if (existingTransport.Trajectory.Pivots.Length == 1)
        {
          if ((Proto) existingTransport.Prototype != (Proto) proto)
          {
            error = (LocStrFormatted) TrCore.TrAdditionError__TypesNoMatch;
            return false;
          }
          if (existingTransport.StartInputPort.IsConnected && existingTransport.EndOutputPort.IsConnected)
          {
            nullable1 = new Tile3i?(pivots.Last);
          }
          else
          {
            if (!a.HasValue)
            {
              RelTile3i resolvedStartDir;
              this.m_transportsManager.Value.ComputeTrajDirectionsSnapToPorts(proto, newPivots, new Direction903d?(), b, out resolvedStartDir, out resolvedEndDir, disablePortSnapping);
              a = new Direction903d?(resolvedStartDir.ToDirection903d());
            }
            IoPort port;
            if (canBeReversedNow && this.m_portsManager.TryGetPortAt(newPivots.First + a.Value.ToTileDirection(), -a.Value, proto.PortsShape, out port) && port.Type == IoPortType.Input)
            {
              nullable2 = new Direction903d?(-direction903d);
              atStart1 = false;
              canBeReversedInTheFuture = false;
              wasReversed = true;
            }
            else
            {
              nullable2 = new Direction903d?(-direction903d);
              atStart1 = true;
              canBeReversedInTheFuture = false;
            }
          }
        }
        else if (existingTransport.StartPosition == pivots.Last)
        {
          if (existingTransport.StartInputPort.IsConnected || (Proto) existingTransport.Prototype != (Proto) proto)
          {
            if (direction903d.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            if (existingTransport.StartInputPort.IsConnected && existingTransport.StartInputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            nullable1 = new Tile3i?(pivots.Last);
          }
          else
          {
            nullable2 = new Direction903d?(-direction903d);
            atStart1 = true;
            canBeReversedInTheFuture = false;
          }
        }
        else if (existingTransport.EndPosition == pivots.Last)
        {
          if (existingTransport.EndOutputPort.IsConnected || (Proto) existingTransport.Prototype != (Proto) proto)
          {
            if (direction903d.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            if (existingTransport.EndOutputPort.IsConnected && existingTransport.EndOutputPort.ExpectedConnectedPortDirection.DirectionVector.Z != 0)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
              return false;
            }
            nullable1 = new Tile3i?(pivots.Last);
          }
          else
          {
            if (!canBeReversedNow)
            {
              error = (LocStrFormatted) TrCore.TrAdditionError__IncompatibleDirection;
              return false;
            }
            nullable2 = new Direction903d?(-direction903d);
            atStart1 = false;
            canBeReversedInTheFuture = false;
            wasReversed = true;
          }
        }
        else
        {
          if (direction903d.DirectionVector.Z != 0)
          {
            error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
            return false;
          }
          nullable1 = new Tile3i?(pivots.Last);
        }
      }
      Assert.That<bool>((Proto) existingTransport.Prototype == (Proto) proto || nullable1.HasValue).IsTrue();
      Assert.That<bool>(nullable1.HasValue != nullable2.HasValue).IsTrue();
      bool flag = true;
      if (nullable2.HasValue)
      {
        if (!this.CanChangeDirectionOf(existingTransport, nullable2.Value, atStart1, out changeDirectionResult))
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
          flag = false;
        }
      }
      else if (nullable1.HasValue)
      {
        CanPlaceMiniZipperAtResult result;
        if (this.CanPlaceMiniZipperAt(existingTransport, nullable1.Value, out result, out error, new Direction903d?(-direction903d)))
          miniZipJoinResult = new CanPlaceMiniZipperAtResult?(result);
        else
          flag = false;
      }
      if (flag)
      {
        if (wasReversed)
        {
          newPivots = newPivots.Reversed();
          Swap.Them<Direction903d?>(ref a, ref b);
        }
        TransportTrajectory trajectory;
        bool wasReversed1;
        LocStrFormatted error2;
        if (!this.m_transportsManager.Value.TryCreateTrajectorySnapToPorts(proto, newPivots, a, b, canBeReversedInTheFuture, disablePortSnapping, out trajectory, out wasReversed1, out error2))
        {
          if (error.IsEmptyOrNull)
            error = error2;
          return false;
        }
        newTraj = (Option<TransportTrajectory>) trajectory;
        wasReversed ^= wasReversed1;
      }
      else
      {
        TransportTrajectory trajectory;
        if (TransportTrajectory.TryCreateFromPivots(proto, pivots.ToImmutableArray(), startDirection.HasValue ? new RelTile3i?(startDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), endDirection.HasValue ? new RelTile3i?(endDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), out trajectory, out error1))
          newTraj = (Option<TransportTrajectory>) trajectory;
      }
      if (!((Proto) existingTransport.Prototype.PortsShape != (Proto) proto.PortsShape))
        return flag;
      error = (LocStrFormatted) TrCore.TrAdditionError__TypesNoMatch;
      return false;
    }

    /// <summary>
    /// Whether start/end direction of the given transport can be changed to the given direction.
    /// </summary>
    internal bool CanChangeDirectionOf(
      Transport existingTransport,
      Direction903d newDirection,
      bool atStart,
      out CanChangeDirectionResult? changeDirectionResult)
    {
      changeDirectionResult = new CanChangeDirectionResult?();
      if ((atStart ? existingTransport.StartInputPort : existingTransport.EndOutputPort).IsConnected)
        return false;
      if (existingTransport.Trajectory.Pivots.Length == 1)
      {
        Direction903d direction903d = atStart ? existingTransport.EndDirection : existingTransport.StartDirection;
        if (newDirection == direction903d)
          return false;
      }
      else
      {
        ImmutableArray<Tile3i> pivots = existingTransport.Trajectory.Pivots;
        if (atStart)
        {
          Tile3i tile3i = pivots.Second;
          Tile2i xy1 = tile3i.Xy;
          tile3i = pivots.First;
          Tile2i xy2 = tile3i.Xy;
          RelTile2i relTile2i = xy1 - xy2;
          if (relTile2i.IsNotZero)
          {
            Direction903d as3d = relTile2i.ToDirection90().As3d;
            if (newDirection == as3d || pivots.Second.Z != pivots.First.Z && newDirection != -as3d)
              return false;
          }
          else
          {
            Direction903d direction903d = (pivots.Second - pivots.First).ToDirection903d();
            if (newDirection == direction903d)
              return false;
          }
        }
        else
        {
          Tile3i tile3i = pivots.PreLast;
          Tile2i xy3 = tile3i.Xy;
          tile3i = pivots.Last;
          Tile2i xy4 = tile3i.Xy;
          RelTile2i relTile2i = xy3 - xy4;
          if (relTile2i.IsNotZero)
          {
            Direction903d as3d = relTile2i.ToDirection90().As3d;
            if (newDirection == as3d || pivots.PreLast.Z != pivots.Last.Z && newDirection != -as3d)
              return false;
          }
          else
          {
            Direction903d direction903d = (pivots.PreLast - pivots.Last).ToDirection903d();
            if (newDirection == direction903d)
              return false;
          }
        }
      }
      changeDirectionResult = new CanChangeDirectionResult?(new CanChangeDirectionResult(existingTransport, newDirection, atStart));
      return true;
    }

    public static bool CanCutOutTransport(
      Transport transport,
      Tile3i cutFromPosition,
      Tile3i cutToPosition,
      bool expandCutOverRampsInsteadOfFailing,
      bool createCutOutTrajectory,
      out CanCutOutTransportResult result,
      out LocStrFormatted error,
      bool doNotCreateNonCutOutTrajs = false)
    {
      CanCutOutTransportTrajResult result1;
      bool flag = transport.Trajectory.CanCutOut(cutFromPosition, cutToPosition, expandCutOverRampsInsteadOfFailing, createCutOutTrajectory, out result1, out error, doNotCreateNonCutOutTrajs);
      result = new CanCutOutTransportResult(cutFromPosition, cutToPosition, transport, result1.StartSubTransport, result1.CutOutSubTransport, result1.EndSubTransport);
      return flag;
    }

    public static bool CanCutOutTransportAt(
      Transport transport,
      Tile3i positionToCutOut,
      out CanCutOutTransportAtResult result,
      out LocStrFormatted error)
    {
      if (transport.Trajectory.Pivots.Length == 1 && (!transport.StartInputPort.IsConnected || !transport.EndOutputPort.IsConnected))
      {
        if (transport.Trajectory.Pivots.First == positionToCutOut)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        }
        else
        {
          Log.Warning(string.Format("Cannot cut transport from position {0} that is not on the transport.", (object) positionToCutOut));
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransportCut;
        }
        result = new CanCutOutTransportAtResult();
        return false;
      }
      CanCutOutTransportResult result1;
      if (TransportsConstructionHelper.CanCutOutTransport(transport, positionToCutOut, positionToCutOut, false, false, out result1, out error))
      {
        result = new CanCutOutTransportAtResult(positionToCutOut, transport, result1.StartSubTransport, result1.EndSubTransport);
        return true;
      }
      result = new CanCutOutTransportAtResult();
      return false;
    }

    /// <summary>
    /// Determines whether mini-zipper can be placed on transport at the given position.
    /// </summary>
    public bool CanPlaceMiniZipperAt(
      Transport transport,
      Tile3i miniZipperPosition,
      out CanPlaceMiniZipperAtResult result,
      out LocStrFormatted error,
      Direction903d? bannedMiniZipperConnection = null)
    {
      Option<MiniZipperProto> option = this.m_protosDb.Get<MiniZipperProto>((Proto.ID) IdsCore.Transports.GetMiniZipperIdFor(transport.Prototype.PortsShape.Id));
      if (option.IsNone)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__NoMiniZipper;
        result = new CanPlaceMiniZipperAtResult();
        return false;
      }
      if (this.m_terrainManager.IsOffLimitsOrInvalid(miniZipperPosition.Tile2i))
      {
        error = (LocStrFormatted) TrCore.AdditionError__OutsideOfMap;
        result = new CanPlaceMiniZipperAtResult();
        return false;
      }
      if (!transport.IsFlatAround(miniZipperPosition))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
        result = new CanPlaceMiniZipperAtResult();
        return false;
      }
      CanCutOutTransportAtResult result1;
      if (!TransportsConstructionHelper.CanCutOutTransportAt(transport, miniZipperPosition, out result1, out error))
      {
        result = new CanPlaceMiniZipperAtResult();
        return false;
      }
      Assert.That<bool>(result1.StartSubTransport.IsNone || result1.StartSubTransport.Value.Pivots.Last.Z == miniZipperPosition.Z).IsTrue();
      Assert.That<bool>(result1.EndSubTransport.IsNone || result1.EndSubTransport.Value.Pivots.First.Z == miniZipperPosition.Z).IsTrue();
      if (bannedMiniZipperConnection.HasValue)
      {
        if (result1.StartSubTransport.HasValue && -result1.StartSubTransport.Value.EndDirection.ToDirection903d() == bannedMiniZipperConnection.Value)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
          result = new CanPlaceMiniZipperAtResult();
          return false;
        }
        if (result1.EndSubTransport.HasValue && -result1.EndSubTransport.Value.StartDirection.ToDirection903d() == bannedMiniZipperConnection.Value)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__InvalidConnection;
          result = new CanPlaceMiniZipperAtResult();
          return false;
        }
      }
      MiniZipper miniZipper;
      if (this.areAnyMiniZippersAround(miniZipperPosition, out miniZipper))
      {
        error = TrCore.TrAdditionError__TooCloseToOtherMiniZipper.Format(miniZipper.Prototype.Strings.Name.TranslatedString);
        result = new CanPlaceMiniZipperAtResult();
        return false;
      }
      error = LocStrFormatted.Empty;
      result = new CanPlaceMiniZipperAtResult(result1, option.Value);
      return true;
    }

    private bool areAnyMiniZippersAround(Tile3i position, out MiniZipper miniZipper)
    {
      Predicate<EntityId> pillarsPredicate = this.m_transportsManager.Value.IgnorePillarsPredicate;
      return this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(position.IncrementX, out miniZipper, pillarsPredicate) || this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(position.IncrementY, out miniZipper, pillarsPredicate) || this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(position.DecrementX, out miniZipper, pillarsPredicate) || this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(position.DecrementY, out miniZipper, pillarsPredicate);
    }

    /// <summary>
    /// Finds supporting pillars for given trajectory. Returns whether the entire trajectory was successfully
    /// supported by pillars and the found supported tiles are at <paramref name="supportedTiles" />.
    /// </summary>
    /// <remarks>
    /// Rules for pillars placement are as follows:
    /// 1) Try to reuse all existing pillars along the trajectory that can be extended to the required height.
    /// 2) If there are no reused pillars on the first or last <c>ceil(support_radius / 2)</c> tiles, try to place
    ///    extra new pillars there even if the tiles are already supported (start testing from start/end, moving
    ///    inwards). This is to avoid visually unsupported transports that have long ends without pillars.
    /// 3) Add new pillars to all unsupported tiles, trying to support as many tiles as possible at once by finding
    ///    the first unsupported tile at index <c>i</c> and trying to place a pillar furthest forward first (at
    ///    <c>i + support_radius</c>), then trying to lower all the way to <c>i - support_radius</c>.
    /// </remarks>
    public bool TryFindPillarsForTrajectory(
      TransportTrajectory trajectory,
      IReadOnlySet<Tile2i> pillarHints,
      out ImmutableArray<Tile3i> supportedTiles,
      out int newPillarsCount,
      out int reusedPillarsCount,
      bool skipExtraPillarsForBetterVisuals = false)
    {
      if (!trajectory.TransportProto.NeedsPillars)
      {
        supportedTiles = ImmutableArray<Tile3i>.Empty;
        newPillarsCount = 0;
        reusedPillarsCount = 0;
        return true;
      }
      this.m_tilesTmp.Clear();
      int newPillarsCountLocal = 0;
      reusedPillarsCount = 0;
      ImmutableArray<TransportSupportableTile> suppTiles = trajectory.SupportableTiles;
      Assert.That<ImmutableArray<TransportSupportableTile>>(suppTiles).IsNotEmpty<TransportSupportableTile>();
      if (suppTiles.Length > this.m_supportedTilesTmp.Size)
        this.m_supportedTilesTmp = new BitMap(suppTiles.Length + 16);
      else
        this.m_supportedTilesTmp.ClearBitsAtLeast(suppTiles.Length);
      int maxSupportRadius = trajectory.TransportProto.MaxPillarSupportRadius.Value;
      int startEndSupportRadius = maxSupportRadius.CeilDiv(2);
      bool startHasSupport = false;
      bool endHasSupport = false;
      bool canBeSupportedByGround = !trajectory.TransportProto.NeedsPillarsAtGround;
      for (int index = 0; index < suppTiles.Length; ++index)
      {
        TransportSupportableTile transportSupportableTile = suppTiles[index];
        Tile3i position = transportSupportableTile.Position;
        if (canBeSupportedByGround && TransportHelper.IsAtGroundWithNoNeedForPillarBelow(this.m_terrainManager[position.Xy], position.Height))
          markSupportedAt(index);
        else if (transportSupportableTile.PillarAttachmentType != TransportPillarAttachmentType.NoAttachment && this.m_transportsManager.Value.CanExtendPillarAt(position.Xy, position.Height, out TransportPillar _, out ThicknessTilesI _))
        {
          markSupportedAt(index);
          this.m_tilesTmp.Add(position);
          ++reusedPillarsCount;
        }
      }
      if (pillarHints.IsNotEmpty<Tile2i>())
      {
        for (int index = 0; index < suppTiles.Length; ++index)
        {
          TransportSupportableTile transportSupportableTile = suppTiles[index];
          if (transportSupportableTile.PillarAttachmentType != TransportPillarAttachmentType.NoAttachment && pillarHints.Contains(transportSupportableTile.Position.Xy))
            tryAddNewPillar(index);
        }
      }
      if (!skipExtraPillarsForBetterVisuals)
      {
        if (!startHasSupport)
        {
          int num = startEndSupportRadius.Min(suppTiles.Length - 1);
          for (int suppTileIndex = 0; suppTileIndex < num; ++suppTileIndex)
          {
            if (tryAddNewPillar(suppTileIndex))
            {
              Assert.That<bool>(startHasSupport).IsTrue();
              break;
            }
          }
        }
        if (!endHasSupport)
        {
          int num = (suppTiles.Length - startEndSupportRadius).Max(0);
          for (int suppTileIndex = suppTiles.Length - 1; suppTileIndex >= num; --suppTileIndex)
          {
            if (tryAddNewPillar(suppTileIndex))
            {
              Assert.That<bool>(endHasSupport).IsTrue();
              break;
            }
          }
        }
        ImmutableArray<TransportTileMetadata?> occupiedTilesMetadata = trajectory.OccupiedTilesMetadata;
        for (int index = 0; index < suppTiles.Length; ++index)
        {
          if (!this.m_supportedTilesTmp.IsSet(index))
          {
            TransportTileMetadata? nullable = occupiedTilesMetadata[suppTiles[index].OccupiedTileIndex];
            if (nullable.HasValue && (nullable.Value.StartType != (TransportStartEndType) -(int) nullable.Value.EndType || nullable.Value.StartDirection != -nullable.Value.EndDirection) && tryAddNewPillar(index))
              index += maxSupportRadius - 1;
          }
        }
      }
      for (int index1 = 0; index1 < suppTiles.Length; ++index1)
      {
        if (!this.m_supportedTilesTmp.IsSet(index1))
        {
          int num1 = (index1 + maxSupportRadius).Min(suppTiles.Length - 1);
          int num2 = (index1 - maxSupportRadius).Max(0);
          bool flag = false;
          for (int index2 = num1; index2 >= num2; --index2)
          {
            if (!this.m_supportedTilesTmp.IsSet(index2) && tryAddNewPillar(index2))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            for (int suppTileIndex = num1; suppTileIndex >= num2; --suppTileIndex)
            {
              if (tryAddNewPillar(suppTileIndex))
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            supportedTiles = ImmutableArray<Tile3i>.Empty;
            newPillarsCount = newPillarsCountLocal;
            if (DebugGameRendererConfig.SaveTransportPillarsSupportFailures)
            {
              MinMaxPair<int> minMaxPair1 = trajectory.Pivots.Select<int>((Func<Tile3i, int>) (x => x.X)).MinMax<int>();
              MinMaxPair<int> minMaxPair2 = trajectory.Pivots.Select<int>((Func<Tile3i, int>) (x => x.Y)).MinMax<int>();
              DebugGameRenderer.DrawGameImage(new Tile2i(minMaxPair1.Min, minMaxPair2.Min) - 10, new Tile2i(minMaxPair1.Max, minMaxPair2.Max) + 10).DrawLine(trajectory.Pivots.Select<Tile2f>((Func<Tile3i, Tile2f>) (x => x.Xy.CenterTile2f)), ColorRgba.Blue).HighlightTiles(suppTiles.AsEnumerable().Where<TransportSupportableTile>((Func<TransportSupportableTile, int, bool>) ((x, i) => this.m_supportedTilesTmp.IsSet(i))).Select<TransportSupportableTile, Tile2i>((Func<TransportSupportableTile, Tile2i>) (x => x.Position.Xy)), ColorRgba.Red).HighlightTiles((IEnumerable<Tile2i>) this.m_tilesTmp.Select<Tile2i>((Func<Tile3i, Tile2i>) (x => x.Xy)), ColorRgba.Green).SaveMapAsTga("TransportPillarsSupportFailures");
            }
            return false;
          }
        }
      }
      newPillarsCount = newPillarsCountLocal;
      supportedTiles = this.m_tilesTmp.ToImmutableArrayAndClear();
      return true;

      void markSupportedAt(int i)
      {
        int num1 = (i - maxSupportRadius).Max(0);
        int num2 = (i + maxSupportRadius).Min(suppTiles.Length - 1);
        for (int index = num1; index <= num2; ++index)
          this.m_supportedTilesTmp.SetBit(index);
        startHasSupport |= i < startEndSupportRadius;
        endHasSupport |= i >= suppTiles.Length - startEndSupportRadius;
      }

      bool tryAddNewPillar(int suppTileIndex)
      {
        TransportSupportableTile transportSupportableTile = suppTiles[suppTileIndex];
        if (canBeSupportedByGround && TransportHelper.IsAtGroundWithNoNeedForPillarBelow(this.m_terrainManager[transportSupportableTile.Position.Xy], transportSupportableTile.Position.Height) || transportSupportableTile.PillarAttachmentType == TransportPillarAttachmentType.NoAttachment)
          return false;
        TransportsManager transportsManager = this.m_transportsManager.Value;
        Tile3i position = transportSupportableTile.Position;
        Tile2i xy = position.Xy;
        position = transportSupportableTile.Position;
        HeightTilesI height = position.Height;
        HeightTilesI heightTilesI;
        ref HeightTilesI local1 = ref heightTilesI;
        ThicknessTilesI thicknessTilesI;
        ref ThicknessTilesI local2 = ref thicknessTilesI;
        if (!transportsManager.CanBuildPillarAt(xy, height, out local1, out local2))
          return false;
        markSupportedAt(suppTileIndex);
        this.m_tilesTmp.Add(transportSupportableTile.Position);
        ++newPillarsCountLocal;
        return true;
      }
    }

    /// <summary>
    /// Checks whether the given transport is properly supported around the given position. The position may not be
    /// an occupied tile of the transport, in which case the closest supportable position will be checked.
    /// </summary>
    public void FindUnsupportedRegionAround(
      TransportTrajectory trajectory,
      Tile3i position,
      out bool positionIsUnsupported,
      out int unsupportedStartIndex,
      out int unsupportedEndIndex)
    {
      ImmutableArray<TransportSupportableTile> supportableTiles = trajectory.SupportableTiles;
      long num1 = long.MaxValue;
      int index1 = -1;
      for (int index2 = 0; index2 < supportableTiles.Length; ++index2)
      {
        Tile3i position1 = supportableTiles[index2].Position;
        long num2 = position1.DistanceSqrTo(position);
        if (num2 < num1)
        {
          num1 = num2;
          index1 = index2;
          if (num2 == 0L)
            break;
        }
        else if (num2 == 1L && position1.Xy == position.Xy)
        {
          index1 = index2;
          break;
        }
      }
      unsupportedStartIndex = unsupportedEndIndex = index1;
      if (index1 < 0)
      {
        Log.Error("No supportable tiles");
        positionIsUnsupported = true;
      }
      else
      {
        bool flag = !trajectory.TransportProto.NeedsPillarsAtGround;
        TransportSupportableTile transportSupportableTile1 = supportableTiles[index1];
        if (flag && TransportHelper.IsAtGroundWithNoNeedForPillarBelow(this.m_terrainManager[transportSupportableTile1.Position.Xy], transportSupportableTile1.Position.Height))
        {
          positionIsUnsupported = false;
        }
        else
        {
          Tile3i position2;
          TransportPillar transportPillar;
          if (transportSupportableTile1.PillarAttachmentType != TransportPillarAttachmentType.NoAttachment)
          {
            TransportsManager transportsManager = this.m_transportsManager.Value;
            position2 = transportSupportableTile1.Position;
            Tile2i xy = position2.Xy;
            position2 = transportSupportableTile1.Position;
            HeightTilesI height = position2.Height;
            ref TransportPillar local = ref transportPillar;
            if (transportsManager.HasPillarAt(xy, height, out local))
            {
              positionIsUnsupported = false;
              return;
            }
          }
          positionIsUnsupported = true;
          for (int index3 = index1 - 1; index3 >= 0; --index3)
          {
            TransportSupportableTile transportSupportableTile2 = supportableTiles[index3];
            if (flag)
            {
              TerrainManager terrainManager = this.m_terrainManager;
              position2 = transportSupportableTile2.Position;
              Tile2i xy = position2.Xy;
              TerrainTile tile = terrainManager[xy];
              position2 = transportSupportableTile2.Position;
              HeightTilesI height = position2.Height;
              if (TransportHelper.IsAtGroundWithNoNeedForPillarBelow(tile, height))
                break;
            }
            if (transportSupportableTile2.PillarAttachmentType != TransportPillarAttachmentType.NoAttachment)
            {
              TransportsManager transportsManager = this.m_transportsManager.Value;
              position2 = transportSupportableTile2.Position;
              Tile2i xy = position2.Xy;
              position2 = transportSupportableTile2.Position;
              HeightTilesI height = position2.Height;
              ref TransportPillar local = ref transportPillar;
              if (transportsManager.HasPillarAt(xy, height, out local))
                break;
            }
            unsupportedStartIndex = index3;
          }
          for (int index4 = index1 + 1; index4 < supportableTiles.Length; ++index4)
          {
            TransportSupportableTile transportSupportableTile3 = supportableTiles[index4];
            if (flag)
            {
              TerrainManager terrainManager = this.m_terrainManager;
              position2 = transportSupportableTile3.Position;
              Tile2i xy = position2.Xy;
              TerrainTile tile = terrainManager[xy];
              position2 = transportSupportableTile3.Position;
              HeightTilesI height = position2.Height;
              if (TransportHelper.IsAtGroundWithNoNeedForPillarBelow(tile, height))
                break;
            }
            if (transportSupportableTile3.PillarAttachmentType != TransportPillarAttachmentType.NoAttachment)
            {
              TransportsManager transportsManager = this.m_transportsManager.Value;
              position2 = transportSupportableTile3.Position;
              Tile2i xy = position2.Xy;
              position2 = transportSupportableTile3.Position;
              HeightTilesI height = position2.Height;
              ref TransportPillar local = ref transportPillar;
              if (transportsManager.HasPillarAt(xy, height, out local))
                break;
            }
            unsupportedEndIndex = index4;
          }
        }
      }
    }

    public PillarVisualsSpec ComputePillarVisuals(
      Tile3i basePosition,
      ThicknessTilesI height,
      Option<TransportTrajectory> extraTrajectory = default (Option<TransportTrajectory>),
      bool makeNotConstructed = false)
    {
      if (TransportsConstructionHelper.s_pillarLayersTmp == null)
        TransportsConstructionHelper.s_pillarLayersTmp = new Lyst<PillarLayerSpec>(TransportPillarProto.MAX_PILLAR_HEIGHT.Value);
      else
        TransportsConstructionHelper.s_pillarLayersTmp.Clear();
      bool flag = false;
      int addedZ = 0;
      for (int index = height.Value; addedZ < index; ++addedZ)
      {
        Option<TransportProto> attachedTransport = Option<TransportProto>.None;
        TransportPillarAttachmentType attachmentType = TransportPillarAttachmentType.NoAttachment;
        Rotation90 attachmentRotation = Rotation90.Deg0;
        uint flags = 0;
        bool hasBeams = addedZ + 1 < height.Value;
        if (hasBeams)
          flags |= 31U;
        Tile3i position = basePosition.AddZ(addedZ);
        IStaticEntity entity;
        if (this.m_occupancyManager.TryGetOccupyingEntityAt<IStaticEntity>(basePosition.AddZ(addedZ), out entity, this.m_transportsManager.Value.IgnorePillarsPredicate))
        {
          if (entity is MiniZipper)
            flags &= 4294967265U;
          else if (entity.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated && entity is LayoutEntity layoutEntity)
          {
            flag = ((flag ? 1 : 0) | (layoutEntity.IsConstructed ? 1 : (layoutEntity.IsBeingUpgraded ? 1 : 0))) != 0;
            flags &= 4294967265U;
          }
          else if (entity is Transport transport)
          {
            processTraj(transport.Trajectory);
            flag = ((flag ? 1 : 0) | (transport.IsConstructed ? 1 : (transport.IsBeingUpgraded ? 1 : 0))) != 0;
          }
        }
        else if (extraTrajectory.HasValue)
          processTraj(extraTrajectory.Value);
        TransportsConstructionHelper.s_pillarLayersTmp.Add(new PillarLayerSpec(attachedTransport, attachmentType, attachmentRotation, (byte) flags));

        void processTraj(TransportTrajectory traj)
        {
          ImmutableArray<TransportSupportableTile> supportableTiles = traj.SupportableTiles;
          int index = supportableTiles.IndexOf<Tile3i>(position, (Func<TransportSupportableTile, Tile3i, bool>) ((x, p) => x.Position == p));
          if (index < 0)
            return;
          attachedTransport = (Option<TransportProto>) traj.TransportProto;
          supportableTiles = traj.SupportableTiles;
          TransportSupportableTile transportSupportableTile = supportableTiles[index];
          attachmentType = transportSupportableTile.PillarAttachmentType;
          attachmentRotation = transportSupportableTile.AttachmentRotation;
          if (transportSupportableTile.AttachmentFlipY)
            flags |= 32U;
          if (!hasBeams)
            return;
          TransportTileMetadata transportTileMetadata = traj.OccupiedTilesMetadata[transportSupportableTile.OccupiedTileIndex].Value;
          if (transportTileMetadata.StartDirection == Direction903d.PlusX | transportTileMetadata.EndDirection == Direction903d.PlusX)
            flags &= 4294967293U;
          if (transportTileMetadata.StartDirection == Direction903d.PlusY | transportTileMetadata.EndDirection == Direction903d.PlusY)
            flags &= 4294967291U;
          if (transportTileMetadata.StartDirection == Direction903d.MinusX | transportTileMetadata.EndDirection == Direction903d.MinusX)
            flags &= 4294967287U;
          if (!(transportTileMetadata.StartDirection == Direction903d.MinusY | transportTileMetadata.EndDirection == Direction903d.MinusY))
            return;
          flags &= 4294967279U;
        }
      }
      return new PillarVisualsSpec(basePosition, TransportsConstructionHelper.s_pillarLayersTmp.ToPooledArrayAndClear(), flag && !makeNotConstructed);
    }
  }
}
