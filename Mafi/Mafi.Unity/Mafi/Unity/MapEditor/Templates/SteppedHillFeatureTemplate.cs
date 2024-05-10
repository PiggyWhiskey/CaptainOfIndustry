// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.SteppedHillFeatureTemplate
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
  public class SteppedHillFeatureTemplate : 
    TerrainFeatureTemplateBase<PolygonTerrainFeatureGenerator>
  {
    private readonly float m_order;
    private readonly ProtosDb m_protosDb;
    private readonly Proto.ID? m_terrainMaterialId;
    private readonly int? m_sortingPriorityAdjustment;

    public override string Name { get; }

    public override Proto.ID CategoryId { get; }

    public override Option<string> IconAssetPath { get; }

    public override float Order => this.m_order;

    public SteppedHillFeatureTemplate(
      string name,
      ProtosDb protosDb,
      Option<string> iconAssetPath = default (Option<string>),
      Proto.ID? terrainMaterialId = null,
      Proto.ID? categoryId = null,
      float? order = null,
      int? sortingPriorityAdjustment = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.CategoryId = categoryId ?? IdsUnity.TerrainFeatureTemplates.TerrainFeatures;
      this.IconAssetPath = iconAssetPath;
      this.m_protosDb = protosDb;
      this.m_terrainMaterialId = terrainMaterialId;
      this.m_sortingPriorityAdjustment = sortingPriorityAdjustment;
      this.m_order = (float) ((double) order ?? (double) base.Order);
    }

    public override PolygonTerrainFeatureGenerator CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_terrainMaterialId ?? Ids.TerrainMaterials.Rock);
      PolygonTerrainFeatureGenerator.Configuration initialConfig = new PolygonTerrainFeatureGenerator.Configuration()
      {
        MaxInfluenceDistance = 60.Tiles(),
        Polygon = new Polygon3fMutable(clampZMinMax: 2048),
        BelowSurfaceExtraHeight = Percent.Hundred,
        BelowSurfaceMaxDepth = 20.0.TilesThick(),
        ShapeInversionDepth = 10.0.TilesThick(),
        HeightFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tdistanceCapParams : SoftCapNoiseParams\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tsteppedParams : SteppedNoiseParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> SoftCap(distanceCapParams)\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)\r\n\t|> Stepped(steppedParams)",
          Parameters = {
            {
              "polygonDistanceTransform",
              (object) new Noise2dTransformParams(-0.4.ToFix64(), Fix64.Zero)
            },
            {
              "distanceCapParams",
              (object) new SoftCapNoiseParams()
            },
            {
              "baseHeightParams",
              (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) 5, (Fix32) 80)
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            },
            {
              "turbulenceParams",
              (object) new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent())
            },
            {
              "steppedParams",
              (object) new SteppedNoiseParams((Fix32) 3, (Fix32) 8)
            }
          }
        },
        TerrainMaterial = orThrow,
        SurfaceCoverThicknessFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tsurfaceThicknessParams : SimplexNoise2dParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(surfaceThicknessParams, seed)",
          Parameters = {
            {
              "surfaceThicknessParams",
              (object) new SimplexNoise2dParams(0.25.ToFix32(), 0.5.ToFix32(), (Fix32) 20)
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            }
          }
        },
        SurfaceCoverMaterial = (Option<TerrainMaterialProto>) this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass),
        SortingPriorityAdjustment = this.m_sortingPriorityAdjustment.GetValueOrDefault()
      };
      Vector2f vector2f1 = position.Xy.Vector2f;
      Polygon3fMutable polygon = initialConfig.Polygon;
      Vector3f[] items = new Vector3f[4];
      Vector2f vector2f2 = vector2f1 + new Vector2f((Fix32) -20, (Fix32) -20);
      items[0] = vector2f2.ExtendZ(position.Z);
      vector2f2 = vector2f1 + new Vector2f((Fix32) 20, (Fix32) -20);
      items[1] = vector2f2.ExtendZ(position.Z);
      vector2f2 = vector2f1 + new Vector2f((Fix32) 20, (Fix32) 20);
      items[2] = vector2f2.ExtendZ(position.Z);
      vector2f2 = vector2f1 + new Vector2f((Fix32) -20, (Fix32) 20);
      items[3] = vector2f2.ExtendZ(position.Z);
      \u003C\u003Ez__ReadOnlyArray<Vector3f> vertices = new \u003C\u003Ez__ReadOnlyArray<Vector3f>(items);
      polygon.Initialize((IEnumerable<Vector3f>) vertices);
      return new PolygonTerrainFeatureGenerator(initialConfig);
    }
  }
}
