// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanBuildTransportResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Ports.Io;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// A successful or unsuccessful result of transport build operation. All fields with `AtStart` and `AtEnd` are
  /// referring to the requested trajectory direction. The result might be reversed.
  /// </summary>
  public class CanBuildTransportResult
  {
    /// <summary>Original pivots request that resulted in this result.</summary>
    public readonly ImmutableArray<Tile3i> RequestPivots;
    /// <summary>Requested start direction.</summary>
    public readonly Direction903d? RequestStartDirection;
    /// <summary>Requested end direction.</summary>
    public readonly Direction903d? RequestEndDirection;
    /// <summary>
    /// New trajectory. Note that due to possible merges (mini-zippers), this trajectory
    /// may be subset of the original request. This may be None if the request has error, but should be always set
    /// for successful results.
    /// </summary>
    public readonly Option<TransportTrajectory> NewTrajectory;
    /// <summary>
    /// Whether original pivots in resulting trajectory were reversed.
    /// </summary>
    public readonly bool PivotsWereReversed;
    /// <summary>Asset value or new trajectory.</summary>
    public readonly AssetValue NewTransportValue;
    /// <summary>
    /// List of verified pillar positions that will be able to support the <see cref="F:Mafi.Core.Factory.Transports.CanBuildTransportResult.NewTrajectory" />.
    /// </summary>
    public readonly ImmutableArray<Tile3i> SupportedTiles;
    /// <summary>
    /// Mini-zipper at start that is not splitting any transport.
    /// </summary>
    public readonly MiniZipperAtResult? MiniZipperAtStart;
    /// <summary>
    /// Mini-zipper at end that is not splitting any transport.
    /// </summary>
    public readonly MiniZipperAtResult? MiniZipperAtEnd;
    /// <summary>
    /// If set, new transport will be joined via mini-zipper at the start
    /// </summary>
    public readonly CanPlaceMiniZipperAtResult? MiniZipJoinResultAtStart;
    /// <summary>
    /// If set, new transport will be joined via mini-zipper at the end
    /// </summary>
    public readonly CanPlaceMiniZipperAtResult? MiniZipJoinResultAtEnd;
    public readonly CanChangeDirectionResult? ChangeDirectionNearStart;
    public readonly CanChangeDirectionResult? ChangeDirectionNearEnd;
    public readonly Option<IoPort> PortAtStart;
    public readonly Option<IoPort> PortAtEnd;

    public CanBuildTransportResult(
      ImmutableArray<Tile3i> requestPivots,
      Direction903d? requestStartDirection,
      Direction903d? requestEndDirection,
      Option<TransportTrajectory> newTrajectory,
      bool pivotsWereReversed,
      AssetValue newTransportValue,
      ImmutableArray<Tile3i> supportedTiles,
      MiniZipperAtResult? miniZipperAtStart,
      MiniZipperAtResult? miniZipperAtEnd,
      CanPlaceMiniZipperAtResult? miniZipJoinResultAtStart,
      CanPlaceMiniZipperAtResult? miniZipJoinResultAtEnd,
      CanChangeDirectionResult? changeDirectionNearStart,
      CanChangeDirectionResult? changeDirectionNearEnd,
      Option<IoPort> portAtStart,
      Option<IoPort> portAtEnd)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RequestPivots = requestPivots;
      this.RequestStartDirection = requestStartDirection;
      this.RequestEndDirection = requestEndDirection;
      this.NewTrajectory = newTrajectory;
      this.PivotsWereReversed = pivotsWereReversed;
      this.NewTransportValue = newTransportValue;
      this.SupportedTiles = supportedTiles;
      this.MiniZipperAtStart = miniZipperAtStart;
      this.MiniZipperAtEnd = miniZipperAtEnd;
      this.MiniZipJoinResultAtStart = miniZipJoinResultAtStart;
      this.MiniZipJoinResultAtEnd = miniZipJoinResultAtEnd;
      this.ChangeDirectionNearStart = changeDirectionNearStart;
      this.ChangeDirectionNearEnd = changeDirectionNearEnd;
      this.PortAtStart = portAtStart;
      this.PortAtEnd = portAtEnd;
    }
  }
}
