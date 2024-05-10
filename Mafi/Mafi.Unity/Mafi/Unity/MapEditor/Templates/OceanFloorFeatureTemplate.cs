// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.OceanFloorFeatureTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.FeatureGenerators;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Random.Noise;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class OceanFloorFeatureTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<OceanFloorFeatureGenerator>
  {
    public override string Name => "Ocean floor";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public override OceanFloorFeatureGenerator CreateNewFeature(IRandom rng)
    {
      return new OceanFloorFeatureGenerator(new OceanFloorFeatureGenerator.Configuration()
      {
        HeightBiasFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\tbaseNoise : SimplexNoise2dParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(baseNoise, seed)",
          Parameters = {
            {
              "baseNoise",
              (object) new SimplexNoise2dParams((Fix32) -2, (Fix32) 3, (Fix32) 300)
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            }
          }
        }
      });
    }

    public OceanFloorFeatureTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
