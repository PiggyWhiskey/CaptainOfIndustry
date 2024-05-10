// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.PolygonTreesPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Core.Terrain.Trees;
using Mafi.Numerics;
using Mafi.Random.Noise;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public class PolygonTreesPostProcessorTemplate : 
    TerrainFeatureTemplateBase<PolygonTreeGeneratorPostProcessor>
  {
    private readonly ImmutableArray<(Proto.ID, int)> m_treeProtos;
    private readonly ProtosDb m_protosDb;
    private readonly bool m_isSparse;
    private readonly Proto.ID? m_limitToMaterial;
    private readonly ThicknessTilesF? m_minFarmableMaterialThickness;

    public override string Name { get; }

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Plants;

    public override Option<string> IconAssetPath { get; }

    public PolygonTreesPostProcessorTemplate(
      string name,
      ImmutableArray<(Proto.ID, int)> treeProtos,
      Option<string> iconAssetPath,
      ProtosDb protosDb,
      bool isSparse = false,
      Proto.ID? limitToMaterial = null,
      ThicknessTilesF? minFarmableMaterialThickness = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.IconAssetPath = iconAssetPath;
      this.m_treeProtos = treeProtos;
      this.m_protosDb = protosDb;
      this.m_isSparse = isSparse;
      this.m_limitToMaterial = limitToMaterial;
      this.m_minFarmableMaterialThickness = minFarmableMaterialThickness;
    }

    public override PolygonTreeGeneratorPostProcessor CreateNewFeatureAt(
      Tile3f position,
      IRandom rng)
    {
      PolygonTreeGeneratorPostProcessor.Configuration initialConfig = new PolygonTreeGeneratorPostProcessor.Configuration()
      {
        Trees = this.m_treeProtos.ToLyst<TreeWithWeight>((Func<(Proto.ID, int), TreeWithWeight>) (x => new TreeWithWeight(this.m_protosDb.GetOrThrow<TreeProto>(x.Item1), x.Item2))),
        Polygon = new Polygon2fMutable(),
        SpacingFunction = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\t# polygon : Polygon2i (external)\r\n\ttreeSpacingParams : SimplexNoise2dParams\r\n\tpolygonDistanceTransform : Noise2dTransformParams\r\n\tmaxParams : Fix64\r\n\tseed : SimplexNoise2dSeed64\r\n\r\npolygonDistance\r\n\tPolygonSignedDistance(polygon)\r\n\t|> Transform(polygonDistanceTransform)\r\n\t|> Max(maxParams)\r\n\r\nreturn\r\n\tSimplexNoise2D(treeSpacingParams, seed)\r\n\t|> SumWithNoise(polygonDistance)",
          Parameters = {
            {
              "polygonDistanceTransform",
              (object) new Noise2dTransformParams(0.05.ToFix64(), (Fix64) 3L)
            },
            {
              "maxParams",
              (object) Fix64.Zero
            },
            {
              "treeSpacingParams",
              (object) (this.m_isSparse ? new SimplexNoise2dParams((Fix32) 12, (Fix32) 10, (Fix32) 80) : new SimplexNoise2dParams(4.ToFix32(), (Fix32) 4, (Fix32) 80))
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            }
          }
        },
        SpawnFunction = (INoise2dFactory) new TextConfigurableNoise2dFactory()
        {
          Configuration = "parameters\r\n\tspawnNoiseParams : SimplexNoise2dParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nreturn\r\n\tSimplexNoise2D(spawnNoiseParams, seed)",
          Parameters = {
            {
              "spawnNoiseParams",
              (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) 10, (Fix32) 1)
            },
            {
              "seed",
              (object) rng.NoiseSeed2d64()
            }
          }
        },
        MaxInfluenceDistance = 60.Tiles(),
        MinSpacing = 3.0.Tiles(),
        MaxSpacing = 8.0.Tiles(),
        SlopeCheckDistance = 3
      };
      if (this.m_minFarmableMaterialThickness.HasValue)
        initialConfig.MinFarmableMaterialThickness = this.m_minFarmableMaterialThickness.Value;
      if (this.m_limitToMaterial.HasValue)
        initialConfig.LimitToMaterialProto = (Option<TerrainMaterialProto>) this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_limitToMaterial.Value);
      initialConfig.MaxHeightDelta = new HeightTilesF(initialConfig.SlopeCheckDistance / 2);
      Vector2f vector2f = position.Xy.Vector2f;
      initialConfig.Polygon.Initialize((IEnumerable<Vector2f>) new \u003C\u003Ez__ReadOnlyArray<Vector2f>(new Vector2f[4]
      {
        vector2f + new Vector2f((Fix32) -30, (Fix32) -30),
        vector2f + new Vector2f((Fix32) 30, (Fix32) -30),
        vector2f + new Vector2f((Fix32) 30, (Fix32) 30),
        vector2f + new Vector2f((Fix32) -30, (Fix32) 30)
      }));
      return new PolygonTreeGeneratorPostProcessor(initialConfig);
    }
  }
}
