// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.TradeDockProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [DebuggerDisplay("TradeDockProto: {Id}")]
  public class TradeDockProto : 
    LayoutEntityProto,
    IProtoWithReservedOcean,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto
  {
    public override Type EntityType => typeof (TradeDock);

    public ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> ReservedOceanAreasSets { get; }

    public HeightTilesI MinGroundHeight => CargoDepotProto.MIN_GROUND_HEIGHT;

    public HeightTilesI MaxGroundHeight => CargoDepotProto.MAX_GROUND_HEIGHT;

    public TradeDockProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics, isUnique: true, cannotBeReflected: true);
      this.ReservedOceanAreasSets = reservedOceanAreasSets;
    }
  }
}
