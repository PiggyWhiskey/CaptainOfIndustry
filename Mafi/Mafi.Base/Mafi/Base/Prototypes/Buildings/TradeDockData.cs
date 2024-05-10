// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.TradeDockData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class TradeDockData : IModData
  {
    public void RegisterData(ProtoRegistrator reg)
    {
      EntityLayout layoutOrThrow = new EntityLayoutParser(reg.PrototypesDb).ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[3]
      {
        new CustomLayoutToken("~0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-10, h, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("{0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? nullable = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(-10, heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-12, -10, LayoutTileConstraint.Ocean)))
      }), ((IEnumerable<string>) "~2!{2!{2!{2!                                 ".Repeat<string>(3)).Concat<string>((IEnumerable<string>) "~2!{2!{2!{2!{2!{2!{2!{4!{4!{4!{4!{4!{4!(4)(4)".Repeat<string>(10)).Concat<string>((IEnumerable<string>) "~2!{2!{2!{2!                                 ".Repeat<string>(3)).ToArray<string>());
      ProtosDb prototypesDb = reg.PrototypesDb;
      StaticEntityProto.ID tradeDock = Ids.Buildings.TradeDock;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Buildings.TradeDock, "Trading dock", "Allows trading of various goods with villages on the world map. All the products you trade are delivered into a trading dock. Also, trading can be a lifesaver if you run into a shortage of critical products, so it is advised to always have a trading dock available.");
      EntityLayout layout = layoutOrThrow;
      EntityCosts entityCosts = Costs.Buildings.TradeDock.MapToEntityCosts(reg);
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> allApproachesAreas = ShipyardData.AllApproachesAreas;
      RelTile3f prefabOrigin = new RelTile3f((Fix32) 6, (Fix32) 0, (Fix32) 0);
      ImmutableArray<ToolbarCategoryProto>? nullable1 = new ImmutableArray<ToolbarCategoryProto>?(reg.GetCategoriesProtos(Ids.ToolbarCategories.Docks));
      Option<string> customIconPath = new Option<string>();
      ColorRgba color = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories = nullable1;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics = new LayoutEntityProto.Gfx("Assets/Base/Buildings/TradeDock.prefab", prefabOrigin, customIconPath, color, visualizedLayers: visualizedLayers, categories: categories, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
      TradeDockProto proto = new TradeDockProto(tradeDock, str, layout, entityCosts, allApproachesAreas, graphics);
      prototypesDb.Add<TradeDockProto>(proto);
    }

    public TradeDockData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
