// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.HillFeatureTemplate
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
  public class HillFeatureTemplate : TerrainFeatureTemplateBase<PolygonTerrainFeatureGenerator>
  {
    private readonly ProtosDb m_protosDb;
    private readonly HillFeatureTemplate.HillFeatureType m_type;
    private readonly Proto.ID? m_terrainMaterialId;
    private readonly bool m_isNotResource;

    public override string Name { get; }

    public override Proto.ID CategoryId { get; }

    public override Option<string> IconAssetPath { get; }

    public override float Order { get; }

    public HillFeatureTemplate(
      string name,
      HillFeatureTemplate.HillFeatureType type,
      ProtosDb protosDb,
      Option<string> iconAssetPath = default (Option<string>),
      Proto.ID? terrainMaterialId = null,
      Proto.ID? categoryId = null,
      float order = 0.0f,
      bool isNotResource = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.CategoryId = categoryId ?? IdsUnity.TerrainFeatureTemplates.TerrainFeatures;
      this.IconAssetPath = iconAssetPath;
      this.m_protosDb = protosDb;
      this.m_type = type;
      this.m_terrainMaterialId = terrainMaterialId;
      this.m_isNotResource = isNotResource;
      this.Order = order;
    }

    public override PolygonTerrainFeatureGenerator CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      Fix32 z = position.Z;
      int period = 80;
      int octavesCount = 4;
      bool flag1 = false;
      bool flag2 = false;
      int num1 = 3;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      Percent hundred = Percent.Hundred;
      Percent percent = Percent.Hundred;
      int num2 = 60;
      int num3 = 100;
      Option<string> none = (Option<string>) Option.None;
      SoftCapNoiseParams? nullable = new SoftCapNoiseParams?();
      bool isNotResource = this.m_isNotResource;
      int amplitude;
      double num4;
      Fix32 fix32;
      int num5;
      switch (this.m_type)
      {
        case HillFeatureTemplate.HillFeatureType.GrandMountain:
          amplitude = 10;
          num4 = -0.8;
          fix32 = (Fix32) 50;
          num1 = 5;
          flag2 = true;
          flag3 = true;
          num5 = 40;
          num3 = 40;
          percent = Percent.Fifty;
          nullable = new SoftCapNoiseParams?(new SoftCapNoiseParams((Fix32) 0, (Fix32) 30));
          z += (Fix32) 15;
          break;
        case HillFeatureTemplate.HillFeatureType.SmallMountain:
          amplitude = 8;
          num4 = -0.6;
          fix32 = (Fix32) 20;
          flag1 = true;
          flag3 = true;
          num5 = 30;
          num3 = 15;
          percent = Percent.Fifty;
          break;
        case HillFeatureTemplate.HillFeatureType.RollingHills:
          amplitude = 5;
          period = 80;
          num4 = -0.2;
          fix32 = (Fix32) 30;
          octavesCount = 2;
          num1 = 5;
          num5 = 5;
          num3 = 5;
          nullable = new SoftCapNoiseParams?(new SoftCapNoiseParams((Fix32) 0, (Fix32) 5));
          break;
        case HillFeatureTemplate.HillFeatureType.Dunes:
          amplitude = 3;
          period = 140;
          num4 = -0.2;
          fix32 = (Fix32) 40;
          octavesCount = 4;
          num5 = 16;
          num3 = 10;
          flag4 = true;
          nullable = new SoftCapNoiseParams?(new SoftCapNoiseParams((Fix32) 0, (Fix32) 10));
          break;
        case HillFeatureTemplate.HillFeatureType.Beach:
          amplitude = 3;
          period = 140;
          num2 = 0;
          num4 = -0.2;
          fix32 = (Fix32) 40;
          octavesCount = 4;
          flag4 = true;
          nullable = new SoftCapNoiseParams?(new SoftCapNoiseParams((Fix32) 0, (Fix32) 5));
          num5 = 5;
          break;
        case HillFeatureTemplate.HillFeatureType.SparseRidges:
          amplitude = 10;
          period = 60;
          num2 = 20;
          num4 = -1.0;
          fix32 = (Fix32) 40;
          octavesCount = 4;
          flag1 = true;
          nullable = new SoftCapNoiseParams?(new SoftCapNoiseParams((Fix32) -2, (Fix32) 0));
          num5 = 10;
          flag4 = true;
          break;
        default:
          amplitude = 12;
          num4 = -0.8;
          fix32 = (Fix32) 40;
          num1 = 5;
          flag1 = true;
          flag3 = true;
          num5 = 40;
          num3 = 20;
          percent = Percent.Fifty;
          break;
      }
      TextConfigurableNoise2dFactory configurableNoise2dFactory1 = new TextConfigurableNoise2dFactory();
      if (flag2)
      {
        configurableNoise2dFactory1.Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tdistanceCapParams : SoftCapNoiseParams\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\tbendParams : ExpBendNoiseParams\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> SoftCap(distanceCapParams)\r\n\t|> BendExp(bendParams)\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Ridged\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)";
        configurableNoise2dFactory1.Parameters.Add("bendParams", (object) new ExpBendNoiseParams(40.Percent(), Fix32.Zero));
      }
      else
        configurableNoise2dFactory1.Configuration = !flag1 ? "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tdistanceCapParams : SoftCapNoiseParams\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> SoftCap(distanceCapParams)\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)" : "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tdistanceCapParams : SoftCapNoiseParams\r\n\tbaseHeightParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> SoftCap(distanceCapParams)\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(baseHeightParams, seed)\r\n\t|> Ridged\r\n\t|> Turbulence(turbulenceParams, seed)\r\n\t|> SumWithNoise(polygonDistance)";
      configurableNoise2dFactory1.Parameters.Add("polygonDistanceTransform", (object) new Noise2dTransformParams(num4.ToFix64(), Fix64.Zero));
      configurableNoise2dFactory1.Parameters.Add("distanceCapParams", (object) (nullable ?? new SoftCapNoiseParams((Fix32) 0, (Fix32) 0)));
      configurableNoise2dFactory1.Parameters.Add("baseHeightParams", (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) amplitude, (Fix32) period));
      configurableNoise2dFactory1.Parameters.Add("turbulenceParams", (object) new NoiseTurbulenceParams(octavesCount, 192.Percent(), 50.Percent()));
      configurableNoise2dFactory1.Parameters.Add("seed", (object) rng.NoiseSeed2d64());
      TextConfigurableNoise2dFactory configurableNoise2dFactory2 = new TextConfigurableNoise2dFactory();
      configurableNoise2dFactory2.Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\tsurfaceThicknessParams : SimplexNoise2dParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nfinalNoise\r\n\tSimplexNoise2D(surfaceThicknessParams, seed)";
      configurableNoise2dFactory2.Parameters.Add("surfaceThicknessParams", (object) new SimplexNoise2dParams(0.25.ToFix32(), 0.5.ToFix32(), (Fix32) 20));
      configurableNoise2dFactory2.Parameters.Add("seed", (object) rng.NoiseSeed2d64());
      TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_terrainMaterialId ?? Ids.TerrainMaterials.Rock);
      PolygonTerrainFeatureGenerator.Configuration initialConfig = new PolygonTerrainFeatureGenerator.Configuration()
      {
        MaxInfluenceDistance = num2.Tiles(),
        Polygon = new Polygon3fMutable(clampZMinMax: 2048),
        BelowSurfaceExtraHeight = hundred,
        BelowSurfaceMaxDepth = num5.TilesThick().ThicknessTilesF,
        ShapeInversionDepth = num3.TilesThick().ThicknessTilesF,
        HeightFn = (INoise2dFactory) configurableNoise2dFactory1,
        TerrainMaterial = orThrow,
        TerrainBlendHeightRange = num1.Tiles().RelTile1f,
        UndergroundDepthMult = percent,
        SurfaceCoverThicknessFn = (INoise2dFactory) configurableNoise2dFactory2,
        SurfaceCoverMaterial = flag3 ? (Option<TerrainMaterialProto>) this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Rock) : Option<TerrainMaterialProto>.None,
        OnlyPlaceOnTopAboveGround = flag4,
        OnlyReplaceExistingMaterials = flag5,
        IgnoreAsResource = isNotResource
      };
      Vector2f vector2f1 = position.Xy.Vector2f;
      Polygon3fMutable polygon = initialConfig.Polygon;
      Vector3f[] items = new Vector3f[4];
      Vector2f vector2f2 = vector2f1 + new Vector2f(-fix32, -fix32);
      items[0] = vector2f2.ExtendZ(z);
      vector2f2 = vector2f1 + new Vector2f(fix32, -fix32);
      items[1] = vector2f2.ExtendZ(z);
      vector2f2 = vector2f1 + new Vector2f(fix32, fix32);
      items[2] = vector2f2.ExtendZ(z);
      vector2f2 = vector2f1 + new Vector2f(-fix32, fix32);
      items[3] = vector2f2.ExtendZ(z);
      \u003C\u003Ez__ReadOnlyArray<Vector3f> vertices = new \u003C\u003Ez__ReadOnlyArray<Vector3f>(items);
      polygon.Initialize((IEnumerable<Vector3f>) vertices);
      return new PolygonTerrainFeatureGenerator(initialConfig, none);
    }

    public enum HillFeatureType
    {
      Default,
      GrandMountain,
      MediumMountain,
      SmallMountain,
      RollingHills,
      Dunes,
      Beach,
      SparseRidges,
    }
  }
}
