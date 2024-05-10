// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.ReplaceMaterialFeatureTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.FeatureGenerators;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Numerics;
using Mafi.Random.Noise;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public class ReplaceMaterialFeatureTemplate : 
    TerrainFeatureTemplateBase<PolygonTerrainReplaceGenerator>
  {
    private readonly ProtosDb m_protosDb;
    private readonly ReplaceMaterialFeatureTemplate.Preset m_preset;
    private readonly Proto.ID? m_terrainMaterialId;
    private readonly float m_order;

    public override string Name { get; }

    public override Proto.ID CategoryId { get; }

    public override Option<string> IconAssetPath { get; }

    public override float Order => this.m_order;

    public ReplaceMaterialFeatureTemplate(
      string name,
      ProtosDb protosDb,
      ReplaceMaterialFeatureTemplate.Preset preset = ReplaceMaterialFeatureTemplate.Preset.Default,
      Option<string> iconAssetPath = default (Option<string>),
      Proto.ID? terrainMaterialId = null,
      float? order = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.CategoryId = IdsUnity.TerrainFeatureTemplates.Plants;
      this.IconAssetPath = iconAssetPath;
      this.m_protosDb = protosDb;
      this.m_preset = preset;
      this.m_terrainMaterialId = terrainMaterialId;
      this.m_order = (float) ((double) order ?? (double) base.Order);
    }

    public override PolygonTerrainReplaceGenerator CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      Fix64 multiplier = -0.05.ToFix64();
      Fix32 amplitude = 0.5.ToFix32();
      switch (this.m_preset)
      {
        case ReplaceMaterialFeatureTemplate.Preset.Flowers:
          multiplier = -0.1.ToFix64();
          amplitude = Fix32.One;
          break;
      }
      TextConfigurableNoise2dFactory configurableNoise2dFactory1 = new TextConfigurableNoise2dFactory();
      configurableNoise2dFactory1.Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tdistanceCapParams : SoftCapNoiseParams\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> SoftCap(distanceCapParams)\r\n\r\nreturn\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)";
      configurableNoise2dFactory1.Parameters.Add("polygonDistanceTransform", (object) new Noise2dTransformParams(multiplier, Fix64.Zero));
      configurableNoise2dFactory1.Parameters.Add("distanceCapParams", (object) new SoftCapNoiseParams((Fix32) 2, -0.5.ToFix32()));
      configurableNoise2dFactory1.Parameters.Add("baseHeightParams", (object) new SimplexNoise2dParams((Fix32) 0, amplitude, (Fix32) 10));
      configurableNoise2dFactory1.Parameters.Add("turbulenceParams", (object) new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent()));
      configurableNoise2dFactory1.Parameters.Add("seed", (object) rng.NoiseSeed2d64());
      TextConfigurableNoise2dFactory configurableNoise2dFactory2 = new TextConfigurableNoise2dFactory()
      {
        Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tsurfaceThicknessParams : SimplexNoise2dParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nreturn\r\n\tSimplexNoise2D(surfaceThicknessParams, seed)",
        Parameters = {
          {
            "surfaceThicknessParams",
            (object) new SimplexNoise2dParams(0.25.ToFix32(), 0.5.ToFix32(), (Fix32) 20)
          },
          {
            "distanceCapParams",
            (object) new SoftCapNoiseParams(0.0.ToFix32(), 1.ToFix32())
          },
          {
            "seed",
            (object) rng.NoiseSeed2d64()
          }
        }
      };
      TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_terrainMaterialId ?? Ids.TerrainMaterials.Rock);
      PolygonTerrainReplaceGenerator.Configuration initialConfig = new PolygonTerrainReplaceGenerator.Configuration()
      {
        MaxInfluenceDistance = 30.Tiles(),
        Polygon = new Polygon2fMutable(),
        ThicknessFn = (INoise2dFactory) configurableNoise2dFactory1,
        TerrainMaterial = orThrow,
        TerrainBlendHeightRange = 1.0.Tiles()
      };
      Vector2f vector2f = position.Xy.Vector2f;
      initialConfig.Polygon.Initialize((IEnumerable<Vector2f>) new \u003C\u003Ez__ReadOnlyArray<Vector2f>(new Vector2f[4]
      {
        vector2f + new Vector2f((Fix32) -20, (Fix32) -20),
        vector2f + new Vector2f((Fix32) 20, (Fix32) -20),
        vector2f + new Vector2f((Fix32) 20, (Fix32) 20),
        vector2f + new Vector2f((Fix32) -20, (Fix32) 20)
      }));
      return new PolygonTerrainReplaceGenerator(initialConfig);
    }

    public enum Preset
    {
      Default,
      BareDirt,
      LushGrass,
      Flowers,
    }
  }
}
