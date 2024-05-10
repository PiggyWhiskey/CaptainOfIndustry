// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.MixedGrassPostProcessorTemplate
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
using Mafi.Random.Noise;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class MixedGrassPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<MixedSurfaceMaterialsPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Mixed materials";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public MixedGrassPostProcessorTemplate(ProtosDb protosDb)
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
        SourceMaterialProto = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass),
        ReplacedMaterialProto = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.GrassLush),
        MinReplacedThickness = 0.1.TilesThick(),
        ReplacedThicknessFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\treplacedThicknessParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonNoise\r\n\tSimplexNoise2D(replacedThicknessParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)",
          Parameters = {
            {
              "replacedThicknessParams",
              (object) new SimplexNoise2dParams(0.25.ToFix32(), 1.25.ToFix32(), (Fix32) 300)
            },
            {
              "turbulenceParams",
              (object) new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent())
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            }
          }
        },
        MaxDepthSearched = 1.0.TilesThick()
      });
    }
  }
}
