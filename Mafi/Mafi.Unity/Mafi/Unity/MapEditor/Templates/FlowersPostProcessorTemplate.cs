// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.FlowersPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class FlowersPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<FlowersPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Flowers";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public FlowersPostProcessorTemplate(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public override FlowersPostProcessor CreateNewFeature(IRandom rng)
    {
      FlowersPostProcessor.Configuration configMutable = new FlowersPostProcessor.Configuration();
      FlowersPostProcessor.FlowersConfig defaultConfig1 = FlowersPostProcessor.FlowersConfig.CreateDefaultConfig(this.m_protosDb);
      defaultConfig1.FlowerMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.FlowersWhite);
      defaultConfig1.SpawnMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass);
      defaultConfig1.SpawnMaterialMaxDepth = 0.25.TilesThick();
      defaultConfig1.SpawnProbability = 0.05.Percent();
      configMutable.FlowersConfigs.Add(defaultConfig1);
      FlowersPostProcessor.FlowersConfig defaultConfig2 = FlowersPostProcessor.FlowersConfig.CreateDefaultConfig(this.m_protosDb);
      defaultConfig2.FlowerMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.FlowersYellowLush);
      defaultConfig2.SpawnMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.GrassLush);
      defaultConfig2.SpawnMaterialMaxDepth = 0.25.TilesThick();
      defaultConfig2.SpawnMaterialMinThickness = 0.75.TilesThick();
      defaultConfig2.SpawnProbability = 0.08.Percent();
      configMutable.FlowersConfigs.Add(defaultConfig2);
      FlowersPostProcessor.FlowersConfig defaultConfig3 = FlowersPostProcessor.FlowersConfig.CreateDefaultConfig(this.m_protosDb);
      defaultConfig3.FlowerMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.FlowersRed);
      defaultConfig3.SpawnMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass);
      defaultConfig3.SpawnProbability = 0.05.Percent();
      configMutable.FlowersConfigs.Add(defaultConfig3);
      FlowersPostProcessor.FlowersConfig defaultConfig4 = FlowersPostProcessor.FlowersConfig.CreateDefaultConfig(this.m_protosDb);
      defaultConfig4.FlowerMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.FlowersPurpleLush);
      defaultConfig4.SpawnMaterial = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.GrassLush);
      defaultConfig4.SpawnMaterialMaxDepth = 0.25.TilesThick();
      defaultConfig4.SpawnMaterialMinThickness = 0.75.TilesThick();
      defaultConfig4.SpawnProbability = 0.08.Percent();
      configMutable.FlowersConfigs.Add(defaultConfig4);
      return new FlowersPostProcessor(configMutable);
    }
  }
}
