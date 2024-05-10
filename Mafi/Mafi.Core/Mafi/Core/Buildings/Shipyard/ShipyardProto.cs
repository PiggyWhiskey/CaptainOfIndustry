// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  public class ShipyardProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<ShipyardProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithReservedOcean,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto
  {
    public readonly bool CanRepair;
    public readonly Quantity CargoCapacity;
    public readonly ImmutableArray<string> DockingAnimationsPrefabPaths;

    public UpgradeData<ShipyardProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> ReservedOceanAreasSets { get; }

    public HeightTilesI MinGroundHeight => CargoDepotProto.MIN_GROUND_HEIGHT;

    public HeightTilesI MaxGroundHeight => CargoDepotProto.MAX_GROUND_HEIGHT;

    public ShipyardProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      bool canRepair,
      Quantity cargoCapacity,
      Option<ShipyardProto> nextTier,
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets,
      LayoutEntityProto.Gfx graphics,
      ImmutableArray<string> dockingAnimationsPrefabPaths,
      bool cannotBeBuiltByPlayer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      LayoutEntityProto.Gfx graphics1 = graphics;
      bool flag1 = cannotBeBuiltByPlayer;
      bool flag2 = cannotBeBuiltByPlayer;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      int num1 = flag1 ? 1 : 0;
      int num2 = flag2 ? 1 : 0;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, graphics1, constructionDurationPerProduct, boostCost, num1 != 0, num2 != 0, true);
      this.CanRepair = canRepair;
      this.CargoCapacity = cargoCapacity;
      this.Upgrade = new UpgradeData<ShipyardProto>(this, nextTier);
      this.ReservedOceanAreasSets = reservedOceanAreasSets;
      this.DockingAnimationsPrefabPaths = dockingAnimationsPrefabPaths;
    }

    public override Type EntityType => typeof (Mafi.Core.Buildings.Shipyard.Shipyard);
  }
}
