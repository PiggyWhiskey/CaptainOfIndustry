// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FarmsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Weather;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class FarmsData : IModData
  {
    /// <summary>
    /// How much of missing fertility is replenished every day.
    /// WARNING: This value hugely affects farms balance.
    /// </summary>
    public static readonly Percent FARM_FERTILITY_REPLENISH;
    public static readonly PartialQuantity WATER_PER_RAINY_DAY;
    private static readonly RelTile2i FARM_SIZE;
    private static readonly RelTile2i BARN_T12_SIZE;
    private static readonly RelTile2i BARN_T3_SIZE;
    private static readonly RelTile2i BARN_T4_SIZE;
    private static readonly RelTile2i PUMP_SIZE;
    private static readonly RelTile2i PUMP_SIZE_T4;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ImmutableArray<ToolbarCategoryProto> immutableArray = ImmutableArray.Create<ToolbarCategoryProto>(prototypesDb.GetOrThrow<ToolbarCategoryProto>(Ids.ToolbarCategories.MachinesFood));
      RectangleTerrainArea2i rectangleTerrainArea2i1 = new RectangleTerrainArea2i(new Tile2i(FarmsData.FARM_SIZE.X - FarmsData.BARN_T12_SIZE.X, 0), FarmsData.BARN_T12_SIZE);
      RectangleTerrainArea2i rectangleTerrainArea2i2 = new RectangleTerrainArea2i(new Tile2i(FarmsData.FARM_SIZE.X - FarmsData.BARN_T3_SIZE.X, 0), FarmsData.BARN_T3_SIZE);
      RectangleTerrainArea2i rectangleTerrainArea2i3 = new RectangleTerrainArea2i(new Tile2i(FarmsData.FARM_SIZE.X - FarmsData.BARN_T4_SIZE.X, 0), FarmsData.BARN_T4_SIZE);
      RectangleTerrainArea2i rectangleTerrainArea2i4 = new RectangleTerrainArea2i(new Tile2i(0, 0), FarmsData.PUMP_SIZE);
      RectangleTerrainArea2i rectangleTerrainArea2i5 = new RectangleTerrainArea2i(new Tile2i(0, 0), FarmsData.PUMP_SIZE_T4);
      RectangleTerrainArea2i rectangleTerrainArea2i6 = new RectangleTerrainArea2i(new Tile2i(0, 10), new RelTile2i(FarmsData.FARM_SIZE.X * 5 / 6, 1));
      RectangleTerrainArea2i rectangleTerrainArea2i7 = new RectangleTerrainArea2i(new Tile2i(0, 30), rectangleTerrainArea2i6.Size);
      RectangleTerrainArea2i rectangleTerrainArea2i8 = new RectangleTerrainArea2i(new Tile2i(0, 0), new RelTile2i(2, 30));
      RectangleTerrainArea2i rectangleTerrainArea2i9 = new RectangleTerrainArea2i(new Tile2i(0, 10), new RelTile2i(FarmsData.FARM_SIZE.X, 1));
      RectangleTerrainArea2i rectangleTerrainArea2i10 = new RectangleTerrainArea2i(new Tile2i(0, 20), new RelTile2i(FarmsData.FARM_SIZE.X, 1));
      RectangleTerrainArea2i rectangleTerrainArea2i11 = new RectangleTerrainArea2i(new Tile2i(0, 30), new RelTile2i(FarmsData.FARM_SIZE.X, 1));
      RectangleTerrainArea2i rectangleTerrainArea2i12 = new RectangleTerrainArea2i(new Tile2i(FarmsData.FARM_SIZE.X - 1, 0), new RelTile2i(1, FarmsData.FARM_SIZE.Y));
      RectangleTerrainArea2i rectangleTerrainArea2i13 = new RectangleTerrainArea2i(new Tile2i(0, 0), new RelTile2i(1, FarmsData.FARM_SIZE.Y));
      RectangleTerrainArea2i rectangleTerrainArea2i14 = new RectangleTerrainArea2i(new Tile2i(0, FarmsData.FARM_SIZE.Y - 1), new RelTile2i(FarmsData.FARM_SIZE.X, 1));
      RectangleTerrainArea2i rectangleTerrainArea2i15 = new RectangleTerrainArea2i(new Tile2i(0, 0), new RelTile2i(FarmsData.FARM_SIZE.X, 1));
      PartialProductQuantity partialProductQuantity = FarmsData.WATER_PER_RAINY_DAY.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Water));
      Percent percent1 = 50.Percent();
      Percent percent2 = 25.Percent();
      LocStr2 locStr2 = Loc.Str2(Ids.Buildings.FarmT3.ToString() + "__desc", "Has {0} increased crop yield compared to the basic farm. Crops also require {1} extra water and fertility.", "example values: {0}=30%, {1}=15%");
      LocStr alreadyLocalizedStr1 = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.FarmT4.ToString() + "_formatted", locStr2.Format(percent1.ToStringRounded(), percent2.ToStringRounded()).Value);
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID farmT4 = Ids.Buildings.FarmT4;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.FarmT4, "Greenhouse II", alreadyLocalizedStr1);
      EntityLayout layout1 = FarmsData.createLayout(6, 5, true, registrator, rectangleTerrainArea2i3, rectangleTerrainArea2i5, rectangleTerrainArea2i12, rectangleTerrainArea2i13, rectangleTerrainArea2i14, rectangleTerrainArea2i15);
      EntityCosts entityCosts1 = Costs.Buildings.FarmT4.MapToEntityCosts(registrator);
      PartialProductQuantity waterCollectedPerDay1 = partialProductQuantity;
      Percent fertilityReplenish1 = FarmsData.FARM_FERTILITY_REPLENISH;
      Percent yieldMultiplier1 = 100.Percent() + percent1;
      Percent demandsMultiplier1 = 100.Percent() + percent2;
      PartialQuantity waterEvaporationPerDay1 = 0.1.Quantity();
      Option<FarmProto> none1 = Option<FarmProto>.None;
      Duration? nullable1 = new Duration?(2 * LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT);
      Option<string> none2 = Option<string>.None;
      Option<string> none3 = Option<string>.None;
      ImmutableArray<RelTile2f> cropsPositions1 = FarmsData.createCropsPositions(0.5.Tiles(), rectangleTerrainArea2i3.ExtendBy(1), rectangleTerrainArea2i5.ExtendBy(1), rectangleTerrainArea2i13, rectangleTerrainArea2i12);
      Option<string> sprinklerPrefabPath1 = none2;
      Option<string> sprinklerSoundPath1 = none3;
      ImmutableArray<ToolbarCategoryProto>? nullable2 = new ImmutableArray<ToolbarCategoryProto>?(immutableArray);
      RelTile3f prefabOrigin1 = new RelTile3f();
      Option<string> customIconPath1 = new Option<string>();
      ColorRgba color1 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers1 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories1 = nullable2;
      FarmProto.Gfx graphics1 = new FarmProto.Gfx("Assets/Base/Buildings/Farms/FarmT4.prefab", cropsPositions1, sprinklerPrefabPath1, sprinklerSoundPath1, prefabOrigin1, customIconPath1, color1, visualizedLayers: visualizedLayers1, categories: categories1);
      Duration? constructionDurationPerProduct1 = nullable1;
      FarmProto proto1 = new FarmProto(farmT4, str1, layout1, entityCosts1, waterCollectedPerDay1, fertilityReplenish1, yieldMultiplier1, demandsMultiplier1, true, true, waterEvaporationPerDay1, none1, graphics1, constructionDurationPerProduct1);
      FarmProto farmProto1 = protosDb1.Add<FarmProto>(proto1);
      Percent percent3 = 25.Percent();
      Percent percent4 = 12.5.Percent();
      LocStr alreadyLocalizedStr2 = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.FarmT3.ToString() + "_formatted", locStr2.Format(percent3.ToStringRounded(), percent4.ToStringRounded()).Value);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID farmT3 = Ids.Buildings.FarmT3;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.FarmT3, "Greenhouse", alreadyLocalizedStr2);
      EntityLayout layout2 = FarmsData.createLayout(6, 5, true, registrator, rectangleTerrainArea2i2, rectangleTerrainArea2i4, rectangleTerrainArea2i12, rectangleTerrainArea2i13, rectangleTerrainArea2i14, rectangleTerrainArea2i15, rectangleTerrainArea2i9, rectangleTerrainArea2i10, rectangleTerrainArea2i11);
      EntityCosts entityCosts2 = Costs.Buildings.FarmT3.MapToEntityCosts(registrator);
      PartialProductQuantity waterCollectedPerDay2 = partialProductQuantity;
      Percent fertilityReplenish2 = FarmsData.FARM_FERTILITY_REPLENISH;
      Percent yieldMultiplier2 = 100.Percent() + percent3;
      Percent demandsMultiplier2 = 100.Percent() + percent4;
      PartialQuantity waterEvaporationPerDay2 = 0.1.Quantity();
      Option<FarmProto> nextTier1 = (Option<FarmProto>) farmProto1;
      nullable1 = new Duration?(2 * LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT);
      Option<string> none4 = Option<string>.None;
      Option<string> none5 = Option<string>.None;
      ImmutableArray<RelTile2f> cropsPositions2 = FarmsData.createCropsPositions(0.5.Tiles(), rectangleTerrainArea2i2.ExtendBy(1), rectangleTerrainArea2i9, rectangleTerrainArea2i10, rectangleTerrainArea2i11, rectangleTerrainArea2i4.ExtendBy(1), rectangleTerrainArea2i13, rectangleTerrainArea2i12);
      Option<string> sprinklerPrefabPath2 = none4;
      Option<string> sprinklerSoundPath2 = none5;
      nullable2 = new ImmutableArray<ToolbarCategoryProto>?(immutableArray);
      RelTile3f prefabOrigin2 = new RelTile3f();
      Option<string> customIconPath2 = new Option<string>();
      ColorRgba color2 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers2 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories2 = nullable2;
      FarmProto.Gfx graphics2 = new FarmProto.Gfx("Assets/Base/Buildings/Farms/FarmT3.prefab", cropsPositions2, sprinklerPrefabPath2, sprinklerSoundPath2, prefabOrigin2, customIconPath2, color2, visualizedLayers: visualizedLayers2, categories: categories2);
      Duration? constructionDurationPerProduct2 = nullable1;
      FarmProto proto2 = new FarmProto(farmT3, str2, layout2, entityCosts2, waterCollectedPerDay2, fertilityReplenish2, yieldMultiplier2, demandsMultiplier2, true, true, waterEvaporationPerDay2, nextTier1, graphics2, constructionDurationPerProduct2);
      FarmProto farmProto2 = protosDb2.Add<FarmProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      StaticEntityProto.ID farmT2 = Ids.Buildings.FarmT2;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.FarmT2, "Irrigated Farm", "Irrigated farm that can be connected to a source of water or fertilizer. That could be useful.");
      EntityLayout layout3 = FarmsData.createLayout(2, 3, true, registrator, rectangleTerrainArea2i1, rectangleTerrainArea2i4);
      EntityCosts entityCosts3 = Costs.Buildings.FarmT2.MapToEntityCosts(registrator);
      PartialProductQuantity waterCollectedPerDay3 = partialProductQuantity;
      Percent fertilityReplenish3 = FarmsData.FARM_FERTILITY_REPLENISH;
      Percent yieldMultiplier3 = 100.Percent();
      Percent demandsMultiplier3 = 100.Percent();
      PartialQuantity waterEvaporationPerDay3 = 0.2.Quantity();
      Option<FarmProto> nextTier2 = (Option<FarmProto>) farmProto2;
      nullable1 = new Duration?(3 * LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT);
      Option<string> option1 = (Option<string>) "Assets/Base/Buildings/Farms/FarmSprinkler.prefab";
      Option<string> option2 = (Option<string>) "Assets/Base/Buildings/Farms/FarmSprinklers_Sound.prefab";
      ImmutableArray<RelTile2f> cropsPositions3 = FarmsData.createCropsPositions(0.5.Tiles(), rectangleTerrainArea2i1.ExtendBy(1), rectangleTerrainArea2i6, rectangleTerrainArea2i7, rectangleTerrainArea2i4.ExtendBy(1), rectangleTerrainArea2i8);
      Option<string> sprinklerPrefabPath3 = option1;
      Option<string> sprinklerSoundPath3 = option2;
      nullable2 = new ImmutableArray<ToolbarCategoryProto>?(immutableArray);
      RelTile3f prefabOrigin3 = new RelTile3f();
      Option<string> customIconPath3 = new Option<string>();
      ColorRgba color3 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers3 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories3 = nullable2;
      FarmProto.Gfx graphics3 = new FarmProto.Gfx("Assets/Base/Buildings/Farms/FarmT2.prefab", cropsPositions3, sprinklerPrefabPath3, sprinklerSoundPath3, prefabOrigin3, customIconPath3, color3, visualizedLayers: visualizedLayers3, categories: categories3, useSemiInstancedRendering: true, disableEmptyChildrenStripping: true);
      Duration? constructionDurationPerProduct3 = nullable1;
      FarmProto proto3 = new FarmProto(farmT2, str3, layout3, entityCosts3, waterCollectedPerDay3, fertilityReplenish3, yieldMultiplier3, demandsMultiplier3, true, false, waterEvaporationPerDay3, nextTier2, graphics3, constructionDurationPerProduct3);
      FarmProto farmProto3 = protosDb3.Add<FarmProto>(proto3);
      string descShort = "Allows growing various crops. Can be used for food production. This farm depends on rain only. To supply water from external source it needs to be upgraded.";
      ProtosDb protosDb4 = prototypesDb;
      StaticEntityProto.ID farmT1 = Ids.Buildings.FarmT1;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.FarmT1, "Farm", descShort);
      EntityLayout layout4 = FarmsData.createLayout(2, 3, false, registrator, rectangleTerrainArea2i1);
      EntityCosts entityCosts4 = Costs.Buildings.Farm.MapToEntityCosts(registrator);
      PartialProductQuantity waterCollectedPerDay4 = partialProductQuantity;
      Percent fertilityReplenish4 = FarmsData.FARM_FERTILITY_REPLENISH;
      Percent yieldMultiplier4 = 100.Percent();
      Percent demandsMultiplier4 = 100.Percent();
      PartialQuantity waterEvaporationPerDay4 = 0.2.Quantity();
      Option<FarmProto> nextTier3 = (Option<FarmProto>) farmProto3;
      nullable1 = new Duration?(3 * LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT);
      Option<string> none6 = Option<string>.None;
      Option<string> none7 = Option<string>.None;
      ImmutableArray<RelTile2f> cropsPositions4 = FarmsData.createCropsPositions(0.5.Tiles(), rectangleTerrainArea2i1.ExtendBy(1));
      Option<string> sprinklerPrefabPath4 = none6;
      Option<string> sprinklerSoundPath4 = none7;
      nullable2 = new ImmutableArray<ToolbarCategoryProto>?(immutableArray);
      RelTile3f prefabOrigin4 = new RelTile3f();
      Option<string> customIconPath4 = new Option<string>();
      ColorRgba color4 = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers4 = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories4 = nullable2;
      FarmProto.Gfx graphics4 = new FarmProto.Gfx("Assets/Base/Buildings/Farms/FarmT1.prefab", cropsPositions4, sprinklerPrefabPath4, sprinklerSoundPath4, prefabOrigin4, customIconPath4, color4, visualizedLayers: visualizedLayers4, categories: categories4, useSemiInstancedRendering: true);
      Duration? constructionDurationPerProduct4 = nullable1;
      FarmProto proto4 = new FarmProto(farmT1, str4, layout4, entityCosts4, waterCollectedPerDay4, fertilityReplenish4, yieldMultiplier4, demandsMultiplier4, false, false, waterEvaporationPerDay4, nextTier3, graphics4, constructionDurationPerProduct4);
      protosDb4.Add<FarmProto>(proto4);
    }

    private static EntityLayout createLayout(
      int farmHeight,
      int barnHeight,
      bool includeInPorts,
      ProtoRegistrator registrator,
      params RectangleTerrainArea2i[] concreteAreas)
    {
      string str1 = string.Format(":{0}:", (object) farmHeight);
      string str2 = string.Format(":{0}]", (object) barnHeight);
      string[][] source = new string[FarmsData.FARM_SIZE.Y][];
      for (int y = 0; y < FarmsData.FARM_SIZE.Y; ++y)
      {
        string[] strArray = new string[FarmsData.FARM_SIZE.X + 2];
        source[y] = strArray;
        strArray[0] = "   ";
        strArray[FarmsData.FARM_SIZE.X + 1] = "   ";
        for (int x = 0; x < FarmsData.FARM_SIZE.X; ++x)
          strArray[x + 1] = !((IEnumerable<RectangleTerrainArea2i>) concreteAreas).Any<RectangleTerrainArea2i>((Func<RectangleTerrainArea2i, bool>) (a => a.ContainsTile(new Tile2i(x, FarmsData.FARM_SIZE.Y - 1 - y)))) ? str1 : str2;
      }
      source[FarmsData.FARM_SIZE.Y - 3][FarmsData.FARM_SIZE.X + 1] = ">~Y";
      source[FarmsData.FARM_SIZE.Y - 2][FarmsData.FARM_SIZE.X + 1] = ">#X";
      if (includeInPorts)
      {
        source[FarmsData.FARM_SIZE.Y - 4][0] = "A@>";
        source[FarmsData.FARM_SIZE.Y - 2][0] = "B@>";
      }
      return registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[2]
      {
        new CustomLayoutToken(":0:", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(Ids.TerrainMaterials.FarmGround);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = nullable;
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.NoRubbleAfterCollapse, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken(":0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable1 = new Proto.ID?(Ids.TerrainMaterials.FarmGround);
          Proto.ID? nullable2 = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = nullable1;
          Proto.ID? surfaceId = nullable2;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }, enforceEmptySurface: true), ((IEnumerable<string[]>) source).Select<string[], string>((Func<string[], string>) (x => ((IEnumerable<string>) x).JoinStrings())).ToArray<string>());
    }

    private static ImmutableArray<RelTile2f> createCropsPositions(
      RelTile1f cropSpacing,
      params RectangleTerrainArea2i[] noCropsAreas)
    {
      Lyst<RelTile2f> lyst = new Lyst<RelTile2f>(FarmsData.FARM_SIZE.ProductInt);
      for (int index = 1; index < FarmsData.FARM_SIZE.Y - 1; ++index)
      {
        RelTile1f x = 1.0.Tiles();
        while (x.Value <= FarmsData.FARM_SIZE.X - 1)
        {
          RelTile2f relTile2f = new RelTile2f(x, new RelTile1f((Fix32) index + Fix32.Half));
          Tile2f t = Tile2f.Zero + relTile2f;
          if (!((IEnumerable<RectangleTerrainArea2i>) noCropsAreas).Any<RectangleTerrainArea2i>((Func<RectangleTerrainArea2i, bool>) (a => a.Contains(t))))
            lyst.Add(relTile2f);
          x += cropSpacing;
        }
      }
      return lyst.ToImmutableArrayAndClear();
    }

    public FarmsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FarmsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FarmsData.FARM_FERTILITY_REPLENISH = 1.Percent();
      FarmsData.WATER_PER_RAINY_DAY = 3.0.Quantity().InverslyScaledBy(WeatherData.RAINY_WEATHER_RAIN_INTENSITY);
      FarmsData.FARM_SIZE = new RelTile2i(61, 41);
      FarmsData.BARN_T12_SIZE = new RelTile2i(5, 5);
      FarmsData.BARN_T3_SIZE = new RelTile2i(8, 10);
      FarmsData.BARN_T4_SIZE = new RelTile2i(8, 12);
      FarmsData.PUMP_SIZE = new RelTile2i(3, 10);
      FarmsData.PUMP_SIZE_T4 = new RelTile2i(3, 12);
    }
  }
}
