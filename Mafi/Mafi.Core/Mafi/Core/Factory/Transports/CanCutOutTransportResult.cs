// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanCutOutTransportResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// A successful result of operation that cuts out a part of a transport, resulting in two smaller transports.
  /// </summary>
  public readonly struct CanCutOutTransportResult
  {
    /// <summary>
    /// The requested cut out start (note that this may not match the actual cut-out start).
    /// </summary>
    public readonly Tile3i CutOutFrom;
    /// <summary>
    /// The requested cut out end (note that this may not match the actual cut-out end).
    /// </summary>
    public readonly Tile3i CutOutTo;
    /// <summary>Original transport that was cut into two.</summary>
    public readonly Transport ReplacedTransport;
    /// <summary>New transport trajectory after cut at the start.</summary>
    public readonly Option<TransportTrajectory> StartSubTransport;
    /// <summary>Cut-out trajectory. Only set when requested.</summary>
    public readonly Option<TransportTrajectory> CutOutSubTransport;
    /// <summary>New transport trajectory after cut at the end.</summary>
    public readonly Option<TransportTrajectory> EndSubTransport;

    public CanCutOutTransportResult(
      Tile3i cutOutFrom,
      Tile3i cutOutTo,
      Transport replacedTransport,
      Option<TransportTrajectory> startSubTransport,
      Option<TransportTrajectory> cutOutSubTransport,
      Option<TransportTrajectory> endSubTransport)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      if (cutOutSubTransport.HasValue)
      {
        if (startSubTransport.HasValue)
          Assert.That<Tile3i>(startSubTransport.Value.Pivots.Last + startSubTransport.Value.EndDirection).IsEqualTo<Tile3i>(cutOutSubTransport.Value.Pivots.First, "Start sub-transport does not point to the cut-out start.");
        if (endSubTransport.HasValue)
          Assert.That<Tile3i>(endSubTransport.Value.Pivots.First + endSubTransport.Value.StartDirection).IsEqualTo<Tile3i>(cutOutSubTransport.Value.Pivots.Last, "End sub-transport does not point to the cut-out end.");
      }
      this.CutOutFrom = cutOutFrom;
      this.CutOutTo = cutOutTo;
      this.ReplacedTransport = replacedTransport;
      this.StartSubTransport = startSubTransport;
      this.CutOutSubTransport = cutOutSubTransport;
      this.EndSubTransport = endSubTransport;
    }

    public AssetValue ComputeCutOutValue()
    {
      AssetValue priceFor = this.ReplacedTransport.Trajectory.TransportProto.GetPriceFor(this.ReplacedTransport.Trajectory.Pivots);
      if (this.StartSubTransport.HasValue)
        priceFor -= this.StartSubTransport.Value.TransportProto.GetPriceFor(this.StartSubTransport.Value.Pivots);
      if (this.EndSubTransport.HasValue)
        priceFor -= this.EndSubTransport.Value.TransportProto.GetPriceFor(this.EndSubTransport.Value.Pivots);
      return priceFor.TakePositiveValuesOnly();
    }
  }
}
