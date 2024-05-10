// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.FlatPlateauFeatureTemplate
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
  public class FlatPlateauFeatureTemplate : 
    TerrainFeatureTemplateBase<PolygonSurfaceFeatureGenerator>
  {
    private readonly ProtosDb m_protosDb;
    private readonly Proto.ID? m_surfaceMaterialId;
    private readonly bool m_includeSurfaceNoise;

    public override string Name { get; }

    public override Proto.ID CategoryId { get; }

    public override Option<string> IconAssetPath { get; }

    public override float Order { get; }

    public FlatPlateauFeatureTemplate(
      string name,
      ProtosDb protosDb,
      Option<string> iconAssetPath = default (Option<string>),
      Proto.ID? surfaceMaterialId = null,
      Proto.ID? categoryId = null,
      int order = 0,
      bool includeSurfaceNoise = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.CategoryId = categoryId ?? IdsUnity.TerrainFeatureTemplates.TerrainFeatures;
      this.IconAssetPath = iconAssetPath;
      this.m_protosDb = protosDb;
      this.m_surfaceMaterialId = surfaceMaterialId;
      this.m_includeSurfaceNoise = includeSurfaceNoise;
      this.Order = (float) order;
    }

    public override PolygonSurfaceFeatureGenerator CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      if (position.Z < 2)
        position = position.SetZ((Fix32) 2);
      return this.CreateNewFeatureAt(position, rng, 40);
    }

    public PolygonSurfaceFeatureGenerator CreateNewFeatureAt(
      Tile3f position,
      IRandom rng,
      int size)
    {
      TextConfigurableNoise2dFactory configurableNoise2dFactory1 = new TextConfigurableNoise2dFactory();
      configurableNoise2dFactory1.Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tcliffsHeightFnParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(cliffsHeightFnParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)";
      configurableNoise2dFactory1.Parameters.Add("polygonDistanceTransform", (object) new Noise2dTransformParams((Fix64) -1L, (Fix64) 0L));
      configurableNoise2dFactory1.Parameters.Add("cliffsHeightFnParams", (object) new SimplexNoise2dParams((Fix32) 10, (Fix32) 16, (Fix32) 100));
      configurableNoise2dFactory1.Parameters.Add("seed", (object) rng.NoiseSeed2d64());
      configurableNoise2dFactory1.Parameters.Add("turbulenceParams", (object) new NoiseTurbulenceParams(4, 192.Percent(), 50.Percent()));
      TextConfigurableNoise2dFactory configurableNoise2dFactory2 = new TextConfigurableNoise2dFactory();
      if (this.m_includeSurfaceNoise)
      {
        configurableNoise2dFactory2.Configuration = "parameters\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nreturn\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Ridged\r\n\t|> Turbulence(turbulenceParams, seed)";
        configurableNoise2dFactory2.Parameters.Add("baseHeightParams", (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) 4, (Fix32) 50));
        configurableNoise2dFactory2.Parameters.Add("turbulenceParams", (object) new NoiseTurbulenceParams(4, 192.Percent(), 50.Percent()));
        configurableNoise2dFactory2.Parameters.Add("seed", (object) rng.NoiseSeed2d64());
      }
      else
      {
        configurableNoise2dFactory2.Configuration = "parameters\r\n\tbaseHeight : Fix32\r\n\r\nfinalNoise\r\n\tConstantNoise2D(baseHeight)";
        configurableNoise2dFactory2.Parameters.Add("baseHeight", (object) Fix32.Zero);
      }
      TextConfigurableNoise2dFactory configurableNoise2dFactory3 = new TextConfigurableNoise2dFactory();
      configurableNoise2dFactory3.Configuration = "parameters\r\n\tmaxThickness : Fix32\r\n\r\nfinalNoise\r\n\tConstantNoise2D(maxThickness)";
      configurableNoise2dFactory3.Parameters.Add("maxThickness", (object) 5.ToFix32());
      TerrainMaterialProto orThrow1 = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_surfaceMaterialId ?? Ids.TerrainMaterials.Grass);
      TerrainMaterialProto orThrow2 = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Rock);
      PolygonSurfaceFeatureGenerator.Configuration initialConfig = new PolygonSurfaceFeatureGenerator.Configuration()
      {
        Polygon = new Polygon2fMutable(),
        BaseHeightFn = (INoise2dFactory) configurableNoise2dFactory1,
        BaseMaterial = orThrow2,
        MaxInfluenceDistance = 80.Tiles(),
        BaseHeight = HeightTilesI.Zero,
        AllowNonIntegerSurfaceHeights = this.m_includeSurfaceNoise,
        SurfaceHeightFn = (INoise2dFactory) configurableNoise2dFactory2,
        SurfaceMaterial = orThrow1,
        SurfaceDepthMult = Fix32.One,
        MaxSurfaceThicknessFn = (INoise2dFactory) configurableNoise2dFactory3
      };
      Vector2f vector2f = position.Xy.Vector2f;
      initialConfig.Polygon.Initialize((IEnumerable<Vector2f>) new \u003C\u003Ez__ReadOnlyArray<Vector2f>(new Vector2f[4]
      {
        vector2f + new Vector2f((Fix32) -size, (Fix32) -size),
        vector2f + new Vector2f((Fix32) size, (Fix32) -size),
        vector2f + new Vector2f((Fix32) size, (Fix32) size),
        vector2f + new Vector2f((Fix32) -size, (Fix32) size)
      }));
      return new PolygonSurfaceFeatureGenerator(initialConfig);
    }
  }
}
