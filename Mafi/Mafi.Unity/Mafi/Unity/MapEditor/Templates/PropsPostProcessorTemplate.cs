// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.PropsPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Props;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class PropsPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<TerrainPropsPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Random props";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public PropsPostProcessorTemplate(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public override TerrainPropsPostProcessor CreateNewFeature(IRandom rng)
    {
      TerrainPropsPostProcessor.Configuration configMutable = new TerrainPropsPostProcessor.Configuration();
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig1 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on grass and dirt",
        PropMaterialOverride = (Option<TerrainMaterialProto>) getMaterial(Ids.TerrainMaterials.Rock),
        BelowPropMaterial = (Option<TerrainMaterialProto>) getMaterial(Ids.TerrainMaterials.DirtBare),
        SpawnProbability = 0.02.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.4.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick()
      };
      propSpawnConfig1.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.Grass), getMaterial(Ids.TerrainMaterials.ForestFloor), getMaterial(Ids.TerrainMaterials.DirtBare));
      propSpawnConfig1.SpawnedProps.Add(getProp(Ids.TerrainProps.Stone01), getProp(Ids.TerrainProps.Stone02), getProp(Ids.TerrainProps.Stone03));
      configMutable.SpawnConfigs.Add(propSpawnConfig1);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig2 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on sand",
        PropMaterialOverride = (Option<TerrainMaterialProto>) getMaterial(Ids.TerrainMaterials.Limestone),
        SpawnProbability = 0.1.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.4.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig2.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.Sand));
      propSpawnConfig2.SpawnedProps.Add(getProp(Ids.TerrainProps.Stone01), getProp(Ids.TerrainProps.Stone02), getProp(Ids.TerrainProps.Stone03));
      configMutable.SpawnConfigs.Add(propSpawnConfig2);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig3 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on coal",
        SpawnProbability = 0.2.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.3.TilesThick(),
        PlacementHeightOffset = -0.4.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = 1.5.TilesThick()
      };
      propSpawnConfig3.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.Coal));
      propSpawnConfig3.SpawnedProps.Add(getProp(Ids.TerrainProps.StoneSharp01));
      configMutable.SpawnConfigs.Add(propSpawnConfig3);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig4 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on rock and dirt",
        PropMaterialOverride = (Option<TerrainMaterialProto>) getMaterial(Ids.TerrainMaterials.Rock),
        SpawnProbability = 0.5.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.3.TilesThick(),
        PlacementHeightOffset = -0.4.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = 1.0.TilesThick()
      };
      propSpawnConfig4.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.DirtBare));
      propSpawnConfig4.SpawnedProps.Add(getProp(Ids.TerrainProps.Stone01), getProp(Ids.TerrainProps.Stone02), getProp(Ids.TerrainProps.Stone03));
      configMutable.SpawnConfigs.Add(propSpawnConfig4);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig5 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on ores",
        SpawnProbability = 1.Percent(),
        MinScale = 60.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.3.TilesThick(),
        PlacementHeightOffset = -0.5.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = 1.5.TilesThick()
      };
      propSpawnConfig5.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.Rock), getMaterial(Ids.TerrainMaterials.IronOre), getMaterial(Ids.TerrainMaterials.CopperOre), getMaterial(Ids.TerrainMaterials.GoldOre), getMaterial(Ids.TerrainMaterials.Limestone));
      propSpawnConfig5.SpawnedProps.Add(getProp(Ids.TerrainProps.Stone01), getProp(Ids.TerrainProps.Stone02), getProp(Ids.TerrainProps.Stone03));
      configMutable.SpawnConfigs.Add(propSpawnConfig5);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig6 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Rocks on ores",
        SpawnProbability = 2.Percent(),
        MinScale = 50.Percent(),
        MaxScale = 100.Percent(),
        MaxHeightDelta = 0.3.TilesThick(),
        PlacementHeightOffset = -0.5.TilesThick(),
        PlacementHeightRandom = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = 1.5.TilesThick()
      };
      propSpawnConfig6.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.RockDisrupted), getMaterial(Ids.TerrainMaterials.IronOreDisrupted), getMaterial(Ids.TerrainMaterials.CopperOreDisrupted), getMaterial(Ids.TerrainMaterials.GoldOreDisrupted), getMaterial(Ids.TerrainMaterials.LimestoneDisrupted));
      propSpawnConfig6.SpawnedProps.Add(getProp(Ids.TerrainProps.Stone01), getProp(Ids.TerrainProps.Stone02), getProp(Ids.TerrainProps.Stone03));
      configMutable.SpawnConfigs.Add(propSpawnConfig6);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig7 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes on grass",
        SpawnProbability = 0.05.Percent(),
        MinScale = 90.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick()
      };
      propSpawnConfig7.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.Grass), getMaterial(Ids.TerrainMaterials.GrassLush));
      propSpawnConfig7.SpawnedProps.Add(getProp(Ids.TerrainProps.BushMedium, 0, 3));
      configMutable.SpawnConfigs.Add(propSpawnConfig7);
      Percent percent = 4.Percent();
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig8 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes on red flowers",
        SpawnProbability = percent,
        MinScale = 80.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig8.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.FlowersYellowLush));
      propSpawnConfig8.SpawnedProps.Add(getProp(Ids.TerrainProps.BushSmall, 8, 9));
      configMutable.SpawnConfigs.Add(propSpawnConfig8);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig9 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes on red flowers",
        SpawnProbability = percent,
        MinScale = 90.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig9.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.FlowersPurpleLush));
      propSpawnConfig9.SpawnedProps.Add(getProp(Ids.TerrainProps.BushSmall, 10, 11));
      configMutable.SpawnConfigs.Add(propSpawnConfig9);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig10 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes on red flowers",
        SpawnProbability = percent,
        MinScale = 90.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig10.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.FlowersRed));
      propSpawnConfig10.SpawnedProps.Add(getProp(Ids.TerrainProps.BushSmall, 12, 13));
      configMutable.SpawnConfigs.Add(propSpawnConfig10);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig11 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes on white flowers",
        SpawnProbability = percent,
        MinScale = 90.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig11.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.FlowersWhite));
      propSpawnConfig11.SpawnedProps.Add(getProp(Ids.TerrainProps.BushSmall, 14, 15));
      configMutable.SpawnConfigs.Add(propSpawnConfig11);
      TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig12 = new TerrainPropsPostProcessor.PropSpawnConfig()
      {
        Name = "Bushes in forest",
        SpawnProbability = 1.Percent(),
        MinScale = 90.Percent(),
        MaxScale = 120.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        MaxSpawnMaterialDepth = ThicknessTilesF.Zero
      };
      propSpawnConfig12.SpawnMaterials.Add(getMaterial(Ids.TerrainMaterials.ForestFloor));
      propSpawnConfig12.SpawnedProps.Add(getProp(Ids.TerrainProps.BushSmall, 4, 7), getProp(Ids.TerrainProps.BushMedium, 4, 7));
      configMutable.SpawnConfigs.Add(propSpawnConfig12);
      return new TerrainPropsPostProcessor(configMutable);

      TerrainMaterialProto getMaterial(Proto.ID id)
      {
        return this.m_protosDb.GetOrThrow<TerrainMaterialProto>(id);
      }

      TerrainPropsPostProcessor.PropProtoWithVariant getProp(
        Proto.ID id,
        int minVariant = -1,
        int maxVariant = 0)
      {
        return new TerrainPropsPostProcessor.PropProtoWithVariant()
        {
          PropProto = this.m_protosDb.GetOrThrow<TerrainPropProto>(id),
          RestrictVariants = minVariant >= 0,
          MinVariantIndex = minVariant.Max(0),
          MaxVariantIndex = maxVariant
        };
      }
    }
  }
}
