// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditorScene
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain;
using Mafi.Base.Terrain.FeatureGenerators;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Core.Map;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.Generators;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Unity.Audio;
using Mafi.Unity.MapEditor.Templates;
using Mafi.Unity.Prototypes;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  /// <summary>
  /// Configures and builds the game so that it runs a map editor. This class does not directly handle any map-editing
  /// features.
  /// </summary>
  internal class MapEditorScene : IGameScene
  {
    private readonly IMain m_main;
    private readonly Option<StartNewGameArgs> m_newGameArgs;
    private readonly Option<LoadGameArgsFromMapFile> m_loadGameArgs;
    private readonly Option<LoadMapInstanceArgs> m_loadMapArgs;
    private DependencyResolver m_resolver;
    private GameRunner m_gameRunner;

    public event Action<DependencyResolver> ResolverCreated;

    public MapEditorScene(IMain main, StartNewGameArgs newGameArgs)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_newGameArgs = (Option<StartNewGameArgs>) newGameArgs.CheckNotNull<StartNewGameArgs>();
    }

    public MapEditorScene(IMain main, LoadGameArgsFromMapFile loadGameArgs)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_loadGameArgs = (Option<LoadGameArgsFromMapFile>) loadGameArgs.CheckNotNull<LoadGameArgsFromMapFile>();
    }

    public MapEditorScene(IMain main, LoadMapInstanceArgs mapGameArgs)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_loadMapArgs = (Option<LoadMapInstanceArgs>) mapGameArgs.CheckNotNull<LoadMapInstanceArgs>();
    }

    public IEnumerator<string> Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new MapEditorScene.\u003CInitialize\u003Ed__12(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void onResolverCreated(DependencyResolver resolver)
    {
      this.m_resolver = resolver;
      Action<DependencyResolver> resolverCreated = this.ResolverCreated;
      if (resolverCreated == null)
        return;
      resolverCreated(resolver);
    }

    private IEnumerator<string> initializeNewMap(StartNewGameArgs args)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new MapEditorScene.\u003CinitializeNewMap\u003Ed__14(0)
      {
        \u003C\u003E4__this = this,
        args = args
      };
    }

    private IEnumerator<string> loadMap(LoadGameArgsFromMapFile args)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new MapEditorScene.\u003CloadMap\u003Ed__15(0)
      {
        \u003C\u003E4__this = this,
        args = args
      };
    }

    private IEnumerator<string> loadMap(LoadMapInstanceArgs args)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new MapEditorScene.\u003CloadMap\u003Ed__16(0)
      {
        \u003C\u003E4__this = this,
        args = args
      };
    }

    private static StaticWorldRegionMapFactory.Config initializeConfigsForEditor(
      Lyst<IConfig> configs,
      string mapFileName = null)
    {
      MapEditorConfig config1 = getOrCreateConfig<MapEditorConfig>();
      if (mapFileName != null)
        config1.MapFileName = mapFileName;
      configs.Add((IConfig) new TerrainGeneratorV2Config());
      getOrCreateConfig<BaseModConfig>().BuildStartingFactory = false;
      getOrCreateConfig<TerrainManagerConfig>().EnableHeightSnapshotting = true;
      configs.Add((IConfig) GameDifficultyConfig.CreateConfigFor(GameDifficultyPreset.Normal));
      configs.Add((IConfig) TutorialsConfig.CreateConfig(false));
      StaticWorldRegionMapFactory.Config config2 = new StaticWorldRegionMapFactory.Config((IWorldRegionMap) null, (IWorldRegionMapPreviewData) null);
      configs.Add((IConfig) new WorldRegionMapFactoryConfig(typeof (StaticWorldRegionMapFactory), (Option<object>) (object) config2));
      configs.Add((IConfig) new StartingLocationConfig(0));
      return config2;

      T getOrCreateConfig<T>() where T : IConfig, new()
      {
        T config = (T) configs.FirstOrDefault<IConfig>((Predicate<IConfig>) (x => x is T));
        if ((object) config == null)
        {
          config = new T();
          configs.Add((IConfig) config);
        }
        return config;
      }
    }

    private static void registerEditorProtos(ProtoRegistrator reg, ImmutableArray<IMod> mods)
    {
      reg.PrototypesDb.SetActiveMod(mods.FirstOrDefault((Func<IMod, bool>) (x => x is UnityMod)));
      reg.RegisterData<TerrainEditorMenuCategoriesData>();
    }

    private void registerEditorDependencies(
      DependencyResolverBuilder depBuilder,
      Set<System.Reflection.Assembly> allAssemblies)
    {
      depBuilder.RegisterInstance<IMain>(this.m_main).As<IMain>();
      depBuilder.RegisterInstance<AssetsDb>(this.m_main.AssetsDb).AsSelf();
      depBuilder.RegisterInstance<AudioDb>(this.m_main.AudioDb).AsSelf();
      depBuilder.RegisterInstance<BackgroundMusicManager>(this.m_main.BackgroundMusicManager).AsSelf();
      depBuilder.RegisterDependency<StartingFactoryPlacer>().AsSelf();
      foreach (System.Reflection.Assembly allAssembly in allAssemblies)
      {
        depBuilder.RegisterAllTypesImplementing<IRegisterInMapEditor>(allAssembly, alsoRegisterAsSelf: true, alsoRegisterAsAllInterfaces: true);
        depBuilder.RegisterAllTypesWithAttribute<MapEditorTemplateFactoryAttribute>(allAssembly, true, true, shouldRegister: (Func<Type, MapEditorTemplateFactoryAttribute, bool>) ((t, attr) =>
        {
          if (t.IsAssignableTo<ITerrainFeatureTemplateFactory>())
            return true;
          Log.Warning("Type '" + t.Name + "' is marked with 'MapEditorTemplateFactoryAttribute' but it does not implement 'ITerrainFeatureTemplateFactory' interface, ignoring.");
          return false;
        }));
      }
    }

    public void Update(Fix32 deltaMs) => this.m_gameRunner.Update(deltaMs);

    public void Terminate()
    {
      this.m_gameRunner?.Terminate();
      this.m_resolver?.TerminateAndClear();
      StateAssert.Reset();
      ThreadAssert.Reset();
    }

    public void OnProjectChanged()
    {
      this.m_resolver.Resolve<GameLoopEvents>().InvokeOnProjectChanged();
    }

    private static WorldRegionMap createEmptyMap(ProtosDb protosDb)
    {
      WorldRegionMap emptyMap = new WorldRegionMap(new RelTile2i(512, 512), protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Bedrock));
      XorRsr128PlusGenerator rng = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 615416468UL, 4135436542654UL);
      PolygonSurfaceFeatureGenerator newFeatureAt = new FlatPlateauFeatureTemplate("flat", protosDb).CreateNewFeatureAt(new Tile3f((Fix32) 256, (Fix32) 256, (Fix32) 0), (IRandom) rng, 80);
      PolygonSurfaceFeatureGenerator.Configuration configMutable = newFeatureAt.ConfigMutable;
      configMutable.BaseHeight = 2.TilesHigh();
      configMutable.SurfaceMaterial = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass);
      emptyMap.TerrainFeatureGeneratorsList.Add((ITerrainFeatureGenerator) newFeatureAt);
      StartingLocationV2 startingLocationV2 = new StartingLocationV2(new StartingLocationV2.Configuration()
      {
        Position = new Tile3i(340, 256, 0)
      });
      emptyMap.StartingLocationsList.Add((IStartingLocationV2) startingLocationV2);
      return emptyMap;
    }

    private static WorldRegionMap loadLegacyMap(IslandMap oldMap, ProtosDb protosDb)
    {
      WorldRegionMap worldRegionMap = new WorldRegionMap(new RelTile2i(oldMap.TerrainWidth, oldMap.TerrainHeight), protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Bedrock));
      worldRegionMap.StartingLocationsList.Add((IStartingLocationV2) new StartingLocationV2(new StartingLocationV2.Configuration()
      {
        Position = oldMap.StartingLocation.Position.ExtendHeight(HeightTilesI.Zero),
        Direction = oldMap.StartingLocation.ShoreDirection
      }));
      XorRsr128PlusGenerator rng = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 615416468UL, 4135436542654UL);
      FlatPlateauFeatureTemplate plateauFeatureTemplate = new FlatPlateauFeatureTemplate("flat", protosDb);
      foreach (MapCell cell in oldMap.Cells)
      {
        if (!cell.IsOcean)
        {
          TerrainMaterialProto orThrow;
          if (cell.SurfaceGenerator.Proto.Id == Ids.CellSurfaces.Grass)
            orThrow = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass);
          else if (cell.SurfaceGenerator.Proto.Id == Ids.CellSurfaces.Rock)
            orThrow = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Rock);
          else
            continue;
          PolygonSurfaceFeatureGenerator newFeatureAt = plateauFeatureTemplate.CreateNewFeatureAt(Tile3f.Zero, (IRandom) rng);
          PolygonSurfaceFeatureGenerator.Configuration configMutable = newFeatureAt.ConfigMutable;
          Vector2f[] vertices = cell.PerimeterIndices.MapArray<Vector2f>((Func<int, Vector2f>) (i => oldMap.CellEdgePoints[i].Vector2f));
          configMutable.Polygon = new Polygon2fMutable();
          configMutable.Polygon.Initialize((IEnumerable<Vector2f>) vertices);
          configMutable.BaseHeight = cell.GroundHeight;
          configMutable.SurfaceMaterial = orThrow;
          worldRegionMap.TerrainFeatureGeneratorsList.Add((ITerrainFeatureGenerator) newFeatureAt);
        }
      }
      HillFeatureTemplate hillFeatureTemplate = new HillFeatureTemplate("", HillFeatureTemplate.HillFeatureType.SmallMountain, protosDb);
      Dict<Mafi.Core.Prototypes.Proto.ID, ITerrainFeatureTemplate> dict = new MineableResourcesFeaturesTemplates(protosDb).GetTemplatesForResources().ToDict<Mafi.Core.Prototypes.Proto.ID, ITerrainFeatureTemplate>();
      PolygonTreesPostProcessorTemplate processorTemplate = new PolygonTreesPostProcessorTemplate("", ImmutableArray.Create<(Mafi.Core.Prototypes.Proto.ID, int)>((Ids.Trees.FirTree, 8), (Ids.Trees.SpruceTree, 8), (Ids.Trees.OakTree, 8), (Ids.Trees.OakTreeDry, 1), (Ids.Trees.BirchTree, 8), (Ids.Trees.BirchTreeDry, 1), (Ids.Trees.MapleTree, 8), (Ids.Trees.MapleTreeDry, 1)), Option<string>.None, protosDb);
      foreach (ITerrainResourceGenerator terrainGenerator in oldMap.AllTerrainGenerators)
      {
        if (terrainGenerator is LineBlobTerrainResourceGenerator resourceGenerator2)
        {
          ITerrainFeatureTemplate terrainFeatureTemplate;
          ITerrainFeatureBase feature = !dict.TryGetValue(resourceGenerator2.ResourceProto.Id, out terrainFeatureTemplate) ? (ITerrainFeatureBase) hillFeatureTemplate.CreateNewFeatureAt(Tile3f.Zero, (IRandom) rng) : terrainFeatureTemplate.CreateNewFeatureAt(Tile3f.Zero, (IRandom) rng);
          if (feature is PolygonTerrainFeatureGenerator featureGenerator)
          {
            PolygonTerrainFeatureGenerator.Configuration configMutable = featureGenerator.ConfigMutable;
            configMutable.Polygon = new Polygon3fMutable(clampZMinMax: 2048);
            HeightTilesF heightTilesF = oldMap.GetClosestCell(resourceGenerator2.From.Average(resourceGenerator2.To)).GroundHeight.HeightTilesF + new ThicknessTilesF(resourceGenerator2.BaseNoiseParams.Amplitude.ScaledBy(resourceGenerator2.GroundLevelBias));
            Vector2f vector2f = resourceGenerator2.From.Vector2f;
            Vector3f vector3f1 = vector2f.ExtendZ((heightTilesF + resourceGenerator2.HeightBiasAtFromPoint).Value);
            vector2f = resourceGenerator2.To.Vector2f;
            Vector3f vector3f2 = vector2f.ExtendZ((heightTilesF + resourceGenerator2.HeightBiasAtToPoint).Value);
            vector2f = resourceGenerator2.To.Vector2f - resourceGenerator2.From.Vector2f;
            Vector3f vector3f3 = vector2f.ExtendZ((Fix32) 0);
            vector3f3 = vector3f3.Cross(new Vector3f((Fix32) 0, (Fix32) 0, (Fix32) 1));
            Vector3f normalized = vector3f3.Normalized;
            Fix32 fix32 = (Fix32) resourceGenerator2.ResourceRadius.Value;
            configMutable.Polygon.Initialize((IEnumerable<Vector3f>) new Vector3f[4]
            {
              vector3f1 - normalized * fix32,
              vector3f1 + normalized * fix32,
              vector3f2 + normalized * fix32,
              vector3f2 - normalized * fix32
            });
            configMutable.TerrainMaterial = resourceGenerator2.ResourceProto;
            configMutable.BelowSurfaceExtraHeight = resourceGenerator2.BelowSurfaceExtraHeight;
            configMutable.BelowSurfaceMaxDepth = resourceGenerator2.BelowSurfaceMaxDepth.ThicknessTilesF;
            configMutable.ShapeInversionDepth = resourceGenerator2.ShapeInversionDepth.ThicknessTilesF;
            configMutable.OnlyPlaceOnTopAboveGround = resourceGenerator2.OnlyPlaceOnTopAboveGround;
            configMutable.OnlyReplaceExistingMaterials = resourceGenerator2.OnlyReplaceExistingMaterials;
          }
          worldRegionMap.AddFeature(feature);
        }
        else if (terrainGenerator is TreesResourceGenerator resourceGenerator1)
        {
          PolygonTreeGeneratorPostProcessor newFeatureAt = processorTemplate.CreateNewFeatureAt(Tile3f.Zero, (IRandom) rng);
          PolygonTreeGeneratorPostProcessor.Configuration configMutable = newFeatureAt.ConfigMutable;
          configMutable.Polygon = new Polygon2fMutable();
          configMutable.Polygon.Initialize((IEnumerable<Vector2f>) new \u003C\u003Ez__ReadOnlyArray<Vector2f>(new Vector2f[3]
          {
            resourceGenerator1.From.Vector2f,
            resourceGenerator1.To.Vector2f,
            (resourceGenerator1.From.Average(resourceGenerator1.To) + (resourceGenerator1.To - resourceGenerator1.From).LeftOrthogonalVector / 4).Vector2f
          }));
          worldRegionMap.TerrainPostProcessorsList.Add((ITerrainPostProcessorV2) newFeatureAt);
        }
      }
      foreach (IVirtualTerrainResource virtualResource in oldMap.VirtualResources)
      {
        if (virtualResource is SimpleVirtualResource simpleVirtualResource)
          worldRegionMap.VirtualTerrainResourceGeneratorList.Add((IVirtualTerrainResourceGenerator) new VirtualResourceFeatureGenerator(new VirtualResourceFeatureGenerator.Configuration()
          {
            VirtualResource = simpleVirtualResource.Product,
            ConfiguredCapacity = simpleVirtualResource.ConfiguredCapacity,
            Position = simpleVirtualResource.Position.Xy.CenterTile2f,
            MaxRadius = simpleVirtualResource.MaxRadius
          }));
      }
      return worldRegionMap;
    }
  }
}
