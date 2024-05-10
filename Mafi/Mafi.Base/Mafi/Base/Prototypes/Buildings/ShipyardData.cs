// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ShipyardData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Fleet;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class ShipyardData : IModData
  {
    public const int PADDING_SIZE = 16;
    public const int PADDING_WIDTH = 13;
    public const string SHIP_PAD = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
    public const string NOTHING_ = "";
    public static readonly ShipsAnimationData PerpendicularApproachAnimData;
    public static readonly ShipsAnimationData ParallelForwardApproachAnimData;
    public static readonly ShipsAnimationData ParallelBackwardApproachAnimData;
    public static ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> AllApproachesAreas;
    public static ImmutableArray<string> AllApproachesAnimationPrefabs;

    public static string[] CreatePadding(
      int size,
      string token,
      string layoutBase,
      bool isReversed)
    {
      string[] padding = new string[size];
      for (int index = 0; index < size; ++index)
      {
        int count = isReversed ? size - index : index + 1;
        if (count <= 3)
          count = 0;
        padding[index] = token.RepeatString(count) + "   ".RepeatString(size - count) + layoutBase;
      }
      return padding;
    }

    public void RegisterData(ProtoRegistrator reg)
    {
      EntityLayout layoutOrThrow = new EntityLayoutParser(reg.PrototypesDb).ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[3]
      {
        new CustomLayoutToken("~0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, i) => new LayoutTokenSpec(-10, i, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("{0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, i) =>
        {
          int heightToExcl = i;
          int? nullable = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(-10, heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, i) => new LayoutTokenSpec(-12, -10, LayoutTileConstraint.Ocean | LayoutTileConstraint.NoRubbleAfterCollapse)))
      }), ((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                            ", false)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(11)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{9!{9!{9!{9!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(3)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(27)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{9!{9!{9!{9!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(3)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(10)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)<@A".Repeat<string>(1)).Concat<string>((IEnumerable<string>) "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!{4!(4)(4)   ".Repeat<string>(5)).Concat<string>((IEnumerable<string>) ShipyardData.CreatePadding(16, "~~~", "                                                            ", true)).ToArray<string>());
      ShipyardProto nextTier = reg.PrototypesDb.Add<ShipyardProto>(new ShipyardProto(Ids.Buildings.Shipyard2, Proto.CreateStr((Proto.ID) Ids.Buildings.Shipyard2, "Shipyard", "Serves to refuel, repair, and modify our ship. Also handles loading and unloading ship's cargo. If a truck happens to have some cargo that cannot be delivered anywhere else, it can be delivered here."), layoutOrThrow, Costs.Buildings.Shipyard2.MapToEntityCosts(reg, true), true, 400.Quantity(), Option<ShipyardProto>.None, ShipyardData.AllApproachesAreas, new LayoutEntityProto.Gfx("Assets/Base/Buildings/Shipyard/ShipyardT2.prefab", new RelTile3f(8.5.ToFix32(), (Fix32) 0, (Fix32) 0), Option<string>.None, ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty), categories: new ImmutableArray<ToolbarCategoryProto>?(ImmutableArray.Create<ToolbarCategoryProto>(reg.PrototypesDb.GetOrThrow<ToolbarCategoryProto>(Ids.ToolbarCategories.Docks)))), ShipyardData.AllApproachesAnimationPrefabs, false));
      reg.PrototypesDb.Add<ShipyardProto>(new ShipyardProto(Ids.Buildings.Shipyard, Proto.CreateStr((Proto.ID) Ids.Buildings.Shipyard, "Shipyard (damaged)", "This shipyard is damaged and needs to be repaired to be able to fix our ship."), layoutOrThrow, Costs.Buildings.Shipyard.MapToEntityCosts(reg, true), false, 400.Quantity(), (Option<ShipyardProto>) nextTier, ShipyardData.AllApproachesAreas, new LayoutEntityProto.Gfx("Assets/Base/Buildings/Shipyard/ShipyardT1.prefab", new RelTile3f(8.5.ToFix32(), (Fix32) 0, (Fix32) 0), Option<string>.None, ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty)), ShipyardData.AllApproachesAnimationPrefabs, true));
      FleetEntityHullProto orThrow1 = reg.PrototypesDb.GetOrThrow<FleetEntityHullProto>((Proto.ID) Ids.Fleet.Hulls.Patrol);
      FleetEnginePartProto orThrow2 = reg.PrototypesDb.GetOrThrow<FleetEnginePartProto>((Proto.ID) Ids.Fleet.Engines.EngineT1);
      FleetBridgePartProto orThrow3 = reg.PrototypesDb.GetOrThrow<FleetBridgePartProto>((Proto.ID) Ids.Fleet.Bridges.BridgeT1);
      TravelingFleetProto.Gfx graphics = new TravelingFleetProto.Gfx("Assets/Base/Ships/BattleShip/Audio/Engine.prefab", "Assets/Base/Ships/BattleShip/Audio/Arrival.prefab", "Assets/Base/Ships/BattleShip/Audio/Departure.prefab");
      reg.PrototypesDb.Add<TravelingFleetProto>(new TravelingFleetProto(IdsCore.World.Fleet, Proto.CreateStr((Proto.ID) IdsCore.World.Fleet, "The Ship", translationComment: "default name of the main ship"), 100.Percent(), 80.Percent(), new RelTile2f(-27.5.ToFix32(), (Fix32) 0), 100, 30.Seconds(), orThrow1, orThrow2, orThrow3, graphics));
    }

    public ShipyardData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ShipyardData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ShipyardData.PerpendicularApproachAnimData = new ShipsAnimationData("Assets/Base/Ships/DockingPerpendicularAnimator.prefab", ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-135, -44), new RelTile2i(100, 84)), new RectangleTerrainArea2iRelative(new RelTile2i(-685, -21), new RelTile2i(550, 20)), new RectangleTerrainArea2iRelative(new RelTile2i(-685, 40), new RelTile2i(630, 20))));
      ShipyardData.ParallelForwardApproachAnimData = new ShipsAnimationData("Assets/Base/Ships/DockingParallelForwardAnimator.prefab", ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-111, -44), new RelTile2i(76, 234)), new RectangleTerrainArea2iRelative(new RelTile2i(-90, 190), new RelTile2i(50, 600))));
      ShipyardData.ParallelBackwardApproachAnimData = new ShipsAnimationData("Assets/Base/Ships/DockingParallelBackwardAnimator.prefab", ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-109, -195), new RelTile2i(74, 240)), new RectangleTerrainArea2iRelative(new RelTile2i(-108, -795), new RelTile2i(60, 600))));
      ShipyardData.AllApproachesAreas = ImmutableArray.Create<ImmutableArray<RectangleTerrainArea2iRelative>>(ShipyardData.PerpendicularApproachAnimData.ReservedOceanAreas, ShipyardData.ParallelForwardApproachAnimData.ReservedOceanAreas, ShipyardData.ParallelBackwardApproachAnimData.ReservedOceanAreas);
      ShipyardData.AllApproachesAnimationPrefabs = ImmutableArray.Create<string>(ShipyardData.PerpendicularApproachAnimData.AnimatorPrefabPath, ShipyardData.ParallelForwardApproachAnimData.AnimatorPrefabPath, ShipyardData.ParallelBackwardApproachAnimData.AnimatorPrefabPath);
    }
  }
}
