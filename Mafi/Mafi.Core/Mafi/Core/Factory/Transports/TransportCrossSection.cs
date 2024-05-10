// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportCrossSection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Gfx;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Defines visual representation of the transport and height of transported products. Cross section is procedurally
  /// extruded among transport curve to form a transport. Cross section can be consisted from more disconnected
  /// components so that's why vertices are stored in 2D arrays.
  /// </summary>
  public readonly struct TransportCrossSection
  {
    /// <summary>
    /// Relative vertices of transport cross-section that are static and do not move with the transport.
    /// The origin [0, 0] is at coordinate [middle of transport, transport surface].
    /// </summary>
    public readonly ImmutableArray<ImmutableArray<CrossSectionVertex>> StaticCrossSectionParts;
    /// <summary>
    /// Relative vertices of transport cross section that move with the transport.
    /// The origin [0, 0] is at coordinate [middle of transport, transport surface].
    /// </summary>
    public readonly ImmutableArray<ImmutableArray<CrossSectionVertex>> MovingCrossSectionParts;

    public static TransportCrossSection Empty
    {
      get
      {
        return new TransportCrossSection(ImmutableArray<ImmutableArray<CrossSectionVertex>>.Empty, ImmutableArray<ImmutableArray<CrossSectionVertex>>.Empty);
      }
    }

    public TransportCrossSection(
      ImmutableArray<ImmutableArray<CrossSectionVertex>> staticCrossSectionParts,
      ImmutableArray<ImmutableArray<CrossSectionVertex>> movingCrossSectionParts)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.StaticCrossSectionParts = staticCrossSectionParts.CheckNotDefaultStruct<ImmutableArray<ImmutableArray<CrossSectionVertex>>>();
      this.MovingCrossSectionParts = movingCrossSectionParts.CheckNotDefaultStruct<ImmutableArray<ImmutableArray<CrossSectionVertex>>>();
    }
  }
}
