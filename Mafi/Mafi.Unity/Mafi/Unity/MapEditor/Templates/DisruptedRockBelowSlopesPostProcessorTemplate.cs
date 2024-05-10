// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.DisruptedRockBelowSlopesPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class DisruptedRockBelowSlopesPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<MixedSurfaceMaterialsPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Disrupted below slopes";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public DisruptedRockBelowSlopesPostProcessorTemplate(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public override MixedSurfaceMaterialsPostProcessor CreateNewFeature(IRandom rng)
    {
      return new MixedSurfaceMaterialsPostProcessor(new MixedSurfaceMaterialsPostProcessor.Configuration()
      {
        SourceMaterialProto = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Rock),
        ReplacedMaterialProto = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.RockDisrupted),
        MinReplacedThickness = 0.1.TilesThick(),
        MaxReplacedThickness = 4.0.TilesThick(),
        ReplacedThicknessFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\tvalue : Fix32\r\n\r\npolygonNoise\r\n\tConstantNoise2D(value)",
          Parameters = {
            {
              "value",
              (object) 4.ToFix32()
            }
          }
        },
        MaxDepthSearched = 5.0.TilesThick(),
        SlopeRestrictionStart = 0.1.ToFix32(),
        SlopeRestrictionEnd = 0.3.ToFix32(),
        SlopeTestDistance = 4.Tiles()
      });
    }
  }
}
