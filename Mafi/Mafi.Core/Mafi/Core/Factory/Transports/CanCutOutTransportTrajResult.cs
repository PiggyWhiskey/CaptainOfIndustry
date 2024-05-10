// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanCutOutTransportTrajResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct CanCutOutTransportTrajResult
  {
    /// <summary>New transport trajectory after cut at the start.</summary>
    public readonly Option<TransportTrajectory> StartSubTransport;
    /// <summary>Cut-out trajectory. Only set when requested.</summary>
    public readonly Option<TransportTrajectory> CutOutSubTransport;
    /// <summary>New transport trajectory after cut at the end.</summary>
    public readonly Option<TransportTrajectory> EndSubTransport;

    public CanCutOutTransportTrajResult(
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
      this.StartSubTransport = startSubTransport;
      this.CutOutSubTransport = cutOutSubTransport;
      this.EndSubTransport = endSubTransport;
    }
  }
}
