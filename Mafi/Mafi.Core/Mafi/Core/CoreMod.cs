// Decompiled with JetBrains decompiler
// Type: Mafi.Core.CoreMod
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Data;
using Mafi.Core.Environment;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Physics;
using Mafi.Numerics;
using Mafi.Random.Noise;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Mafi.Core
{
  public sealed class CoreMod : IMod, IModWithMaps
  {
    private bool m_prototypesRegistered;
    private bool m_dependenciesRegistered;
    private bool m_isInitialized;
    private readonly MapsLoadingHelper m_mapsHelper;

    public string Name => "Mafi-Core";

    public int Version => 0;

    public bool IsUiOnly => false;

    public Option<IConfig> ModConfig => this.Config.SomeOption<IConfig>();

    public CoreModConfig Config { get; }

    /// <summary>Core mod does not depend on any other mod.</summary>
    public CoreMod(CoreModConfig config)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_mapsHelper = new MapsLoadingHelper();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
    }

    /// <summary>
    /// Whether the mods registration and initialization was performed correctly.
    /// </summary>
    internal bool IsInitialized
    {
      get
      {
        return this.Config != null && this.m_prototypesRegistered && this.m_dependenciesRegistered && this.m_isInitialized;
      }
    }

    void IMod.RegisterPrototypes(ProtoRegistrator registrator)
    {
      Assert.That<CoreModConfig>(this.Config).IsNotNull<CoreModConfig>("Config not set!");
      Assert.That<bool>(this.m_prototypesRegistered).IsFalse("Double prototype registration!");
      Assert.That<bool>(this.m_dependenciesRegistered).IsFalse("Dependencies registered before prototypes!");
      Assert.That<bool>(this.m_isInitialized).IsFalse("Initialized before prototypes!");
      this.m_prototypesRegistered = true;
      registrator.RegisterData<TerrainDesignationsData>();
      registrator.RegisterData<SurfaceDesignationsData>();
      registrator.RegisterData<NotificationsData>();
      registrator.RegisterData<PropertiesData>();
      registrator.RegisterData<UpointsCategoriesData>();
      registrator.RegisterData<HealthPointsCategoriesData>();
      registrator.RegisterData<BirthRatesCategoriesData>();
      registrator.RegisterData<TechnologiesData>();
    }

    void IMod.RegisterDependencies(
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded)
    {
      Assert.That<CoreModConfig>(this.Config).IsNotNull<CoreModConfig>("Config not set!");
      Assert.That<bool>(this.m_prototypesRegistered).IsTrue("Prototypes not registered!");
      Assert.That<bool>(this.m_dependenciesRegistered).IsFalse("Double dependency registration!");
      Assert.That<bool>(this.m_isInitialized).IsFalse("Initialized before prototypes!");
      this.m_dependenciesRegistered = true;
      depBuilder.RegisterExtraResolver(ProtoDepAttribute.GetResolvingFunction(protosDb));
      if (gameWasLoaded)
        return;
      if (this.Config.DisableTerrainPhysics)
        depBuilder.RegisterDependency<NoTerrainPhysicsSimulator>().AsAllInterfaces();
      else
        depBuilder.RegisterDependency<TerrainPhysicsSimulator>().AsSelf().AsAllInterfaces();
      if (this.Config.DisableTerrainSurfaceSimulation)
        depBuilder.RegisterDependency<NoTerrainDisruptionSimulator>().AsAllInterfaces();
      else
        depBuilder.RegisterDependency<TerrainDisruptionSimulator>().AsSelf().AsAllInterfaces();
      if (!this.Config.DisablePathFinding)
        return;
      depBuilder.ClearRegistrations<IVehiclePathFinder>();
      depBuilder.RegisterDependency<DirectVehiclePathFinder>().AsAllInterfaces();
    }

    void IMod.EarlyInit(DependencyResolver resolver)
    {
      CoreMod.InitializeNoise2dParser(resolver.Resolve<ConfigurableNoise2dParser>());
    }

    void IMod.Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
      Assert.That<CoreModConfig>(this.Config).IsNotNull<CoreModConfig>("Config not set!");
      Assert.That<bool>(this.m_prototypesRegistered).IsTrue("Prototypes not registered!");
      Assert.That<bool>(this.m_dependenciesRegistered).IsTrue("Dependencies not registered!");
      Assert.That<bool>(this.m_isInitialized).IsFalse("Double mod initialization!");
      this.m_isInitialized = true;
      if (this.Config.LogCommandsAsCSharp)
        resolver.Resolve<ReplayManager>().StartLoggingCommandsAsCSharpTo(new StreamWriter(resolver.Resolve<IFileSystemHelper>().GetTimestampedFilePath(".cs", FileType.Replay)), resolver);
      if (!this.Config.AlwaysSunny)
        return;
      resolver.Resolve<WeatherManager>().Cheat_TrySetWeatherFixed("sunny").AssertTrue();
    }

    public IEnumerable<IWorldRegionMapPreviewData> GetMapPreviews(
      IFileSystemHelper fsHelper,
      ProtosDb protosDb,
      bool includeWip)
    {
      this.m_mapsHelper.Initialize(protosDb);
      this.m_mapsHelper.LoadMapPreviews(Path.GetFullPath("Maps"), fsHelper, includeWip);
      this.m_mapsHelper.LoadMapPreviews(fsHelper.GetDirPath(FileType.Map, false), fsHelper, includeWip);
      return this.m_mapsHelper.GetLoadedMapPreviews();
    }

    public bool TryGetMapData(
      IWorldRegionMapPreviewData preview,
      IFileSystemHelper fsHelper,
      ProtosDb protosDb,
      out IWorldRegionMapAdditionalData fullData,
      out WorldRegionMapFactoryConfig factoryConfig)
    {
      return this.m_mapsHelper.TryGetMapData(preview, out fullData, out factoryConfig);
    }

    public void ClearMapData() => this.m_mapsHelper.ClearMapData();

    public static void InitializeNoise2dParser(ConfigurableNoise2dParser parser)
    {
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) Fix32.Zero));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) Fix64.Zero));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) Vector2f.Zero));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) 5, (Fix32) 40)));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new SimplexNoise2dParams((Fix32) 0, (Fix32) 5, (Fix32) 40)));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new SimplexNoise2dSeed64(Fix64.Tau, Fix64.OneOverSqrt2)));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new Noise2dTransformParams((Fix64) 1L, (Fix64) 0L)));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent())));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new SteppedNoiseParams((Fix32) 3, (Fix32) 8)));
      parser.RegisterDefaultParameterFactory((Func<object>) (() => (object) new ExpBendNoiseParams(25.Percent(), (Fix32) 0)));
      parser.RegisterInitialStatement("ConstantNoise2D", "Outputs a constant value", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("value", typeof (Fix32))), (Func<object[], INoise2D>) (args => (INoise2D) new ConstantNoise2D((Fix32) args[0])));
      parser.RegisterInitialStatement("SimplexNoise2D", "Simplex 2D noise that produces random pattern of smooth hills and valleys.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (SimplexNoise2dParams)), new ConfigurableNoise2dParamSpec("seed", typeof (SimplexNoise2dSeed64))), (Func<object[], INoise2D>) (args => ((SimplexNoise2dParams) args[0]).CreateSimplexNoise2dSafe((SimplexNoise2dSeed64) args[1])));
      parser.RegisterInitialStatement("PointDistanceNoise", "Outputs distance to a point. This is always non-negative.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("point", typeof (Vector2f))), (Func<object[], INoise2D>) (args => (INoise2D) new PointDistanceNoise((Vector2f) args[0])));
      parser.RegisterInitialStatement("PolygonDistance", "Outputs distance from the polygon boundary. Returned distance is non-negative. Note that this function is faster to compute than signed polygon distance.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("polygon", typeof (Polygon2f))), (Func<object[], INoise2D>) (args => (INoise2D) new PolygonDistanceNoise((Polygon2f) args[0])));
      parser.RegisterInitialStatement("PolygonSignedDistance", "Outputs values based on a signed distance to the polygon boundary. Distances outside of the polygon are positive and inside are negative.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("polygon", typeof (Polygon2f))), (Func<object[], INoise2D>) (args => (INoise2D) new PolygonSignedDistanceNoise((Polygon2f) args[0])));
      parser.RegisterTransformStatement("Transform", "Transforms input noise by changing its mean, amplitude, and frequency.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (Noise2dTransformParams))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Transform((Noise2dTransformParams) args[0])));
      parser.RegisterTransformStatement("SumWithNoise", "Combines two noise functions together.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("otherNoise", typeof (INoise2D))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => (INoise2D) noise.Sum((INoise2D) args[0])));
      parser.RegisterTransformStatement("Min", "Returns the min of the input noise value and the given constant", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (Fix64))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Min((Fix64) args[0])));
      parser.RegisterTransformStatement("MinWithNoise", "Returns minimum of the two noise functions.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("otherNoise", typeof (INoise2D))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Min((INoise2D) args[0])));
      parser.RegisterTransformStatement("Max", "Returns the max of the input noise value and the given constant", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (Fix64))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Max((Fix64) args[0])));
      parser.RegisterTransformStatement("MaxWithNoise", "Returns maximum of the two noise functions.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("otherNoise", typeof (INoise2D))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Max((INoise2D) args[0])));
      parser.RegisterTransformStatement("Ridged", "Creates sharp ridges by taking an absolute value and inverting the sign of the values. The input must span both positive and negative values range.", ImmutableArray<ConfigurableNoise2dParamSpec>.Empty, (Func<INoise2D, object[], INoise2D>) ((noise, args) => (INoise2D) noise.Ridged()));
      parser.RegisterTransformStatement("Turbulence", "Combines input multiple times, each time with different frequency and amplitude based on the config.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (NoiseTurbulenceParams)), new ConfigurableNoise2dParamSpec("seed", typeof (SimplexNoise2dSeed64))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Turbulence(((SimplexNoise2dSeed64) args[1]).ToFix32(), (NoiseTurbulenceParams) args[0])));
      parser.RegisterTransformStatement("Stepped", "Creates steps.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (SteppedNoiseParams))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.Stepped((SteppedNoiseParams) args[0])));
      parser.RegisterTransformStatement("WarpCoords", "Warps input coordinates with a simplex 2D noise.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("warpNoiseParams", typeof (SimplexNoise2dParams)), new ConfigurableNoise2dParamSpec("warpNoiseSeed", typeof (SimplexNoise2dSeed64))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.WarpCoordsV2((SimplexNoise2dParams) args[0], (SimplexNoise2dSeed64) args[1])));
      parser.RegisterTransformStatement("WarpCoordsWithNoise", "Warps input coordinates with the given noise.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("warpNoise", typeof (INoise2D))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => (INoise2D) noise.WarpCoords((INoise2D) args[0])));
      parser.RegisterTransformStatement("SoftCap", "Softly caps values at the end value using the sine function. Distance between start and end is the transition areas. Works both ways for capping min and max values.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("params", typeof (SoftCapNoiseParams))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.SoftCap((SoftCapNoiseParams) args[0])));
      parser.RegisterTransformStatement("BendExp", "Smoothly 'bends' the values towards exponential increase for positive values and towards logarithmic 'slowdown' for negative values.", ImmutableArray.Create<ConfigurableNoise2dParamSpec>(new ConfigurableNoise2dParamSpec("bendParams", typeof (ExpBendNoiseParams))), (Func<INoise2D, object[], INoise2D>) ((noise, args) => noise.BendExp((ExpBendNoiseParams) args[0])));
    }
  }
}
