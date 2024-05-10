// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Ai.ScriptedAiConfigs
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ai.Scripted;
using Mafi.Core.Map;
using Mafi.Core.Prototypes;
using System.Linq;

#nullable disable
namespace Mafi.Base.Ai
{
  public static class ScriptedAiConfigs
  {
    public static ScriptedAiPlayerConfig GetScriptedAiPlayerConfigForDefaultTestMap()
    {
      return new ScriptedAiPlayerConfig()
      {
        VehicleDepotPosition = new Tile2i(70, -20),
        FirstBuildingPosition = new Tile2i(60, 0),
        FirstBuildingPositionAlt = new Tile2i(100, 120),
        BuildingsGridSpacing = new RelTile1i(5)
      };
    }

    public static TestMapGeneratorConfig GetDefaultTestMapConfig(
      int extraCells = 0,
      bool extraCliffCellsOnTop = false,
      HeightTilesI? cliffHeight = null)
    {
      TestMapGeneratorConfig defaultTestMapConfig = new TestMapGeneratorConfig();
      defaultTestMapConfig.CellSurfaceIds = ImmutableArray.Create<MapCellSurfaceGeneratorProto.ID>(Ids.CellSurfaces.Rock, Ids.CellSurfaces.Grass, Ids.CellSurfaces.Grass).AddRange(Enumerable.Repeat<MapCellSurfaceGeneratorProto.ID>(Ids.CellSurfaces.Grass, extraCells));
      defaultTestMapConfig.CliffProtoId = Ids.TerrainMaterials.Rock;
      defaultTestMapConfig.PrimaryForestProtoId = new Proto.ID?(Ids.Forests.ConiferForest);
      defaultTestMapConfig.SecondaryForestProtoId = new Proto.ID?(Ids.Forests.ConiferForest);
      defaultTestMapConfig.MineableResources = ImmutableArray.Create<TestMapResource>(new TestMapResource(Ids.TerrainMaterials.IronOre), new TestMapResource(Ids.TerrainMaterials.Coal), new TestMapResource(Ids.TerrainMaterials.Sand), new TestMapResource(Ids.TerrainMaterials.CopperOre), new TestMapResource(Ids.TerrainMaterials.Limestone));
      TestMapGeneratorConfig mapGeneratorConfig = defaultTestMapConfig;
      TestMapResource testMapResource1 = new TestMapResource(Ids.TerrainMaterials.Coal, new Percent?(150.Percent()));
      Proto.ID copperOre = Ids.TerrainMaterials.CopperOre;
      Percent? size1 = new Percent?(150.Percent());
      Percent? depth1 = new Percent?();
      Percent? nullable = new Percent?();
      Percent? height1 = nullable;
      TestMapResource testMapResource2 = new TestMapResource(copperOre, size1, depth1, height1);
      Proto.ID goldOre = Ids.TerrainMaterials.GoldOre;
      nullable = new Percent?(-200.Percent());
      Percent? size2 = new Percent?();
      Percent? depth2 = new Percent?();
      Percent? height2 = nullable;
      TestMapResource testMapResource3 = new TestMapResource(goldOre, size2, depth2, height2);
      TestMapResource testMapResource4 = new TestMapResource(Ids.TerrainMaterials.IronOre, new Percent?(150.Percent()));
      ImmutableArray<TestMapResource> immutableArray = ImmutableArray.Create<TestMapResource>(testMapResource1, testMapResource2, testMapResource3, testMapResource4);
      mapGeneratorConfig.MineableResourcesOnCliff = immutableArray;
      defaultTestMapConfig.LandHeight = new HeightTilesI(2);
      defaultTestMapConfig.OceanHeight = MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT;
      defaultTestMapConfig.ExtraTopCellsHeight = cliffHeight ?? new HeightTilesI(12);
      defaultTestMapConfig.ExtraTopCellsSurfaceId = extraCliffCellsOnTop ? new MapCellSurfaceGeneratorProto.ID?(Ids.CellSurfaces.Grass) : new MapCellSurfaceGeneratorProto.ID?();
      return defaultTestMapConfig;
    }
  }
}
