// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanCutOutTransportAtResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// A successful result of operation that cuts out a tile from a transport, resulting in two smaller transports.
  /// </summary>
  public readonly struct CanCutOutTransportAtResult
  {
    /// <summary>
    /// Tile that was cut out. This is of course one of tiles of <see cref="F:Mafi.Core.Factory.Transports.CanCutOutTransportAtResult.ReplacedTransport" />.
    /// </summary>
    public readonly Tile3i CutOutPosition;
    /// <summary>Original transport that was cut into two.</summary>
    public readonly Transport ReplacedTransport;
    /// <summary>New transport trajectory after cut at the start.</summary>
    public readonly Option<TransportTrajectory> StartSubTransport;
    /// <summary>New transport trajectory after cut at the end.</summary>
    public readonly Option<TransportTrajectory> EndSubTransport;

    public CanCutOutTransportAtResult(
      Tile3i cutOutPosition,
      Transport replacedTransport,
      Option<TransportTrajectory> startSubTransport,
      Option<TransportTrajectory> endSubTransport)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      if (startSubTransport.HasValue)
        Assert.That<Tile3i>(startSubTransport.Value.Pivots.Last + startSubTransport.Value.EndDirection).IsEqualTo<Tile3i>(cutOutPosition, "Cut-out transport at start does not point to the cut-out position.");
      if (endSubTransport.HasValue)
        Assert.That<Tile3i>(endSubTransport.Value.Pivots.First + endSubTransport.Value.StartDirection).IsEqualTo<Tile3i>(cutOutPosition, "Cut-out transport at end does not point to the cut-out position.");
      this.CutOutPosition = cutOutPosition;
      this.ReplacedTransport = replacedTransport;
      this.StartSubTransport = startSubTransport;
      this.EndSubTransport = endSubTransport;
    }

    public CanCutOutTransportResult ExtendToFullCutOutResult()
    {
      return new CanCutOutTransportResult(this.CutOutPosition, this.CutOutPosition, this.ReplacedTransport, this.StartSubTransport, Option<TransportTrajectory>.None, this.EndSubTransport);
    }
  }
}
