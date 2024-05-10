// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using jOgQY3RGtH5fd9qQao;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Map;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Mafi.Core.Game
{
  internal static class GameBuilder
  {
    private static readonly Regex s_validName;

    internal static bool IsOptionalDlc(ModInfoRaw modInfo)
    {
      return modInfo.TypeStr.Contains("Mafi.Supporter", StringComparison.Ordinal);
    }

    /// <summary>
    /// Builds a game instance with given mods and configs in time-sliced manner.
    /// </summary>
    public static IEnumerator<string> BuildNewGameTimeSliced(
      StartNewGameArgs startNewArgs,
      bool enableThreadAndStateAsserts,
      Action<DependencyResolver> setResolver = null,
      Action<ImmutableArray<IMod>> initMods = null,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnBeforeMods = null,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnAfterMods = null,
      Action<DependencyResolverBuilder, ProtosDb, ImmutableArray<IMod>, Set<Assembly>> customResolverBuildFnAfterMods = null,
      Action<DependencyResolver> beforeInitFn = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameBuilder.\u003CBuildNewGameTimeSliced\u003Ed__2(0)
      {
        startNewArgs = startNewArgs,
        enableThreadAndStateAsserts = enableThreadAndStateAsserts,
        setResolver = setResolver,
        initMods = initMods,
        protosDbRegFnBeforeMods = protosDbRegFnBeforeMods,
        protosDbRegFnAfterMods = protosDbRegFnAfterMods,
        customResolverBuildFnAfterMods = customResolverBuildFnAfterMods,
        beforeInitFn = beforeInitFn
      };
    }

    /// <summary>Builds a game from saved data.</summary>
    public static IEnumerator<string> BuildLoadedGameTimeSliced(
      LoadGameArgs loadArgs,
      bool enableThreadAndStateAsserts,
      Func<ImmutableArray<ModInfoRaw>, ImmutableArray<ModData>> filterLoadedMods,
      Action<DependencyResolver> setResolver = null,
      Action<ImmutableArray<IMod>, ImmutableArray<IConfig>> initMods = null,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnBeforeMods = null,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnAfterMods = null,
      Action<DependencyResolverBuilder, ProtosDb, ImmutableArray<IMod>> customResolverBuildFnAfterMods = null,
      Action<DependencyResolver> beforeInitFn = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameBuilder.\u003CBuildLoadedGameTimeSliced\u003Ed__3(0)
      {
        loadArgs = loadArgs,
        enableThreadAndStateAsserts = enableThreadAndStateAsserts,
        filterLoadedMods = filterLoadedMods,
        setResolver = setResolver,
        initMods = initMods,
        protosDbRegFnBeforeMods = protosDbRegFnBeforeMods,
        protosDbRegFnAfterMods = protosDbRegFnAfterMods,
        customResolverBuildFnAfterMods = customResolverBuildFnAfterMods,
        beforeInitFn = beforeInitFn
      };
    }

    private static IslandMap createIslandMap(
      ImmutableArray<IConfig> configs,
      ProtosDb protosDb,
      RandomProvider randomProvider)
    {
      IslandMapGeneratorConfig config1;
      if (!configs.TryGetConfig<IslandMapGeneratorConfig>(out config1))
        throw new Exception("Config 'IslandMapGeneratorConfig' was not found.");
      if (config1.IslandMapGeneratorType == (Type) null)
        throw new Exception("The 'IslandMapGeneratorType' of 'IslandMapGeneratorConfig' was not set.");
      DependencyResolverBuilder dependencyResolverBuilder = new DependencyResolverBuilder();
      dependencyResolverBuilder.RegisterInstance<ProtosDb>(protosDb).AsSelf();
      dependencyResolverBuilder.RegisterInstance<RandomProvider>(randomProvider).AsSelf();
      foreach (IConfig config2 in configs)
      {
        DependencyResolverBuilder.DependencyInstanceRegistrar<IConfig> instanceRegistrar = dependencyResolverBuilder.RegisterInstance<IConfig>(config2);
        instanceRegistrar = instanceRegistrar.AsSelf();
        instanceRegistrar.As<IConfig>();
      }
      DependencyResolver dependencyResolver = dependencyResolverBuilder.BuildAndClear();
      object obj = dependencyResolver.Instantiate(config1.IslandMapGeneratorType);
      if (!(obj is IIslandMapGenerator islandMapGenerator))
        throw new Exception("Configured map generator '" + obj.GetType().Name + "' does not implement 'IIslandMapGenerator' interface.");
      islandMapGenerator.GenerateIslandMapTimeSliced().EnumerateToTheEnd<string>();
      dependencyResolver.TerminateAndClear();
      config1.MarkIslandMapAlreadyGenerated();
      return islandMapGenerator.GetMapAndClear();
    }

    private static (IWorldRegionMap, IWorldRegionMapPreviewData) createWorldRegionMap(
      ImmutableArray<IConfig> configs,
      ProtosDb protosDb,
      RandomProvider randomProvider)
    {
      WorldRegionMapFactoryConfig config1;
      if (!configs.TryGetConfig<WorldRegionMapFactoryConfig>(out config1))
        throw new Exception("Config 'WorldRegionMapFactoryConfig' was not found.");
      if (config1.FactoryType == (Type) null)
        throw new Exception("The 'FactoryType' of 'WorldRegionMapFactoryConfig' was not set.");
      DependencyResolverBuilder dependencyResolverBuilder = new DependencyResolverBuilder();
      dependencyResolverBuilder.RegisterInstance<ProtosDb>(protosDb).AsSelf();
      dependencyResolverBuilder.RegisterInstance<RandomProvider>(randomProvider).AsSelf();
      foreach (IConfig config2 in configs)
      {
        DependencyResolverBuilder.DependencyInstanceRegistrar<IConfig> instanceRegistrar = dependencyResolverBuilder.RegisterInstance<IConfig>(config2);
        instanceRegistrar = instanceRegistrar.AsSelf();
        instanceRegistrar.As<IConfig>();
      }
      DependencyResolver dependencyResolver = dependencyResolverBuilder.BuildAndClear();
      object obj = dependencyResolver.Instantiate(config1.FactoryType);
      if (!(obj is IWorldRegionMapFactory regionMapFactory))
        throw new Exception("Configured map generator '" + obj.GetType().Name + "' does not implement 'IWorldRegionMapFactory' interface.");
      regionMapFactory.GenerateIslandMapTimeSliced(config1.Config).EnumerateToTheEnd<Percent>();
      dependencyResolver.TerminateAndClear();
      IWorldRegionMapPreviewData previewData;
      return (regionMapFactory.GetMapAndClear(out previewData), previewData);
    }

    private static IEnumerator<string> initializeGame(
      ImmutableArray<IMod> mods,
      DependencyResolver resolver,
      bool gameWasLoaded,
      bool enableThreadAndStateAsserts,
      Action<DependencyResolver> beforeInitFn = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameBuilder.\u003CinitializeGame\u003Ed__6(0)
      {
        mods = mods,
        resolver = resolver,
        gameWasLoaded = gameWasLoaded,
        enableThreadAndStateAsserts = enableThreadAndStateAsserts,
        beforeInitFn = beforeInitFn
      };
    }

    public static ImmutableArray<T> InstantiateConfigsOrThrow<T>(
      Set<Assembly> assembliesToScan,
      IEnumerable<IConfig> existingConfigs)
      where T : class, IConfig
    {
      Lyst<(Assembly, Exception)> failedAssemblies;
      ImmutableArray<T> immutableArray = GameBuilder.TryInstantiateConfigs<T>(assembliesToScan, existingConfigs, out failedAssemblies);
      if (failedAssemblies.IsNotEmpty)
        throw failedAssemblies.First.Item2;
      return immutableArray;
    }

    /// <summary>
    /// Instantiates all classes implementing <see cref="T:Mafi.Core.Game.IConfig" /> from assemblies of all given mod types.
    /// </summary>
    public static ImmutableArray<T> TryInstantiateConfigs<T>(
      Set<Assembly> assembliesToScan,
      IEnumerable<IConfig> existingConfigs,
      out Lyst<(Assembly, Exception)> failedAssemblies)
      where T : class, IConfig
    {
      DependencyResolverBuilder dependencyResolverBuilder = new DependencyResolverBuilder();
      failedAssemblies = new Lyst<(Assembly, Exception)>();
      Set<Type> set = new Set<Type>();
      foreach (IConfig existingConfig in existingConfigs)
      {
        if (set.Add(existingConfig.GetType()))
        {
          DependencyResolverBuilder.DependencyInstanceRegistrar<IConfig> instanceRegistrar = dependencyResolverBuilder.RegisterInstance<IConfig>(existingConfig);
          instanceRegistrar = instanceRegistrar.AsSelf();
          instanceRegistrar.AsAllInterfaces();
        }
        else
          Log.Error("Duplicate config type '" + existingConfig.GetType().Name + "', skipping.");
      }
      foreach (Assembly assembly in assembliesToScan)
      {
        try
        {
          foreach (Type type in assembly.GetTypes())
          {
            if (type.IsClass && !type.IsAbstract && type.IsAssignableTo<T>() && !dependencyResolverBuilder.IsAnythingRegisteredAs(type) && type.GetCustomAttribute<GlobalDependencyAttribute>() != null)
            {
              DependencyResolverBuilder.DependencyRegistrar dependencyRegistrar = dependencyResolverBuilder.RegisterDependency(type);
              dependencyRegistrar = dependencyRegistrar.AsSelf();
              dependencyRegistrar.AsAllInterfaces();
            }
          }
        }
        catch (Exception ex)
        {
          failedAssemblies.Add((assembly, ex));
        }
      }
      DependencyResolver dependencyResolver = dependencyResolverBuilder.BuildAndClear();
      ImmutableArray<T> implementations = dependencyResolver.ResolveAll<T>().Implementations;
      dependencyResolver.TerminateAndClear();
      Log.Info("Configs instantiated: " + implementations.Map<string>((Func<T, string>) (x => x.GetType().Name)).JoinStrings(", "));
      return implementations;
    }

    /// <summary>
    /// Tries to instantiate mods from <paramref name="allModTypes" />. If that fails, tries to at least instantiate
    /// mods from <paramref name="coreModTypes" />. If that also fails, exception is thrown. If some mods are
    /// successfully instantiated, tries to register all their protos. All mods that failed to be instantiated or their
    /// protos registration failed are returned in the <paramref name="failedModTypes" /> parameter.
    /// </summary>
    public static ImmutableArray<IMod> TryInstantiateModsAndProtos(
      ImmutableArray<ModData> allMods,
      ProtoRegistrator protosRegistrator)
    {
      Set<Assembly> set = new Set<Assembly>();
      set.Add(typeof (DependencyResolver).Assembly);
      foreach (Assembly assembly in allMods.Map<Assembly>((Func<ModData, Assembly>) (x => x.ModType.Assembly)))
        set.Add(assembly);
      Set<Assembly> assembliesToScan = set;
      Lyst<(Assembly, Exception)> failedAssemblies;
      ImmutableArray<IModConfig> configs = GameBuilder.TryInstantiateConfigs<IModConfig>(assembliesToScan, (IEnumerable<IConfig>) Enumerable.Empty<IModConfig>(), out failedAssemblies);
      LocalizationManager.ScanForStaticLocStrFields((IEnumerable<Assembly>) assembliesToScan);
      if (failedAssemblies.IsNotEmpty)
      {
        ImmutableArray<ModData> immutableArray = allMods.FilterCoreMods();
        foreach ((Assembly assembly, Exception exception) in failedAssemblies)
        {
          if (immutableArray.Filter((Predicate<ModData>) (x => x.ModType.Assembly == assembly)).IsNotEmpty)
            throw exception;
          foreach (ModData modData in allMods.Filter((Predicate<ModData>) (x => x.ModType.Assembly == assembly)))
            modData.SetLoadFail(exception);
        }
      }
      allMods = allMods.Filter((Predicate<ModData>) (x => !x.FailedToLoad));
      ImmutableArray<IMod> mods = GameBuilder.TryInstantiateMods(allMods, configs);
      foreach ((IMod mod, Exception exception) in GameBuilder.RegisterModsPrototypes(mods, protosRegistrator, true))
      {
        allMods.FirstOrDefault((Func<ModData, bool>) (x => x.ModType == mod.GetType()))?.SetLoadFail(exception);
        mods = mods.RemoveAt(mods.IndexOf(mod));
      }
      foreach (IMod mod1 in mods)
      {
        IMod mod = mod1;
        allMods.FirstOrDefault((Func<ModData, bool>) (x => x.ModType == mod.GetType()))?.SetLoadSuccess(mod.Version);
      }
      return mods;
    }

    public static ImmutableArray<IMod> InstantiateModsOrThrow(
      ImmutableArray<ModData> modsData,
      ImmutableArray<IModConfig> configs)
    {
      ImmutableArray<IMod> immutableArray = GameBuilder.TryInstantiateMods(modsData, configs);
      if (modsData.Any((Func<ModData, bool>) (x => x.Exception.HasValue)))
        throw modsData.FirstOrDefault((Func<ModData, bool>) (x => x.Exception.HasValue)).Exception.Value;
      return immutableArray;
    }

    /// <summary>
    /// Instantiates all mods from given list of types. Returned mods are in topological order - all dependencies of
    /// a mod come before the mod itself. Note that there are usually more topological orderings of the mods graph
    /// and any valid ordering can be returned.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Game.GameBuilderException">Throws when core mod fails to be instantiated.</exception>
    public static ImmutableArray<IMod> TryInstantiateMods(
      ImmutableArray<ModData> mods,
      ImmutableArray<IModConfig> configs)
    {
      return (ImmutableArray<IMod>) OBqe2IUAeSpOmlOQ4O.TyOaFSuuHy(0, new object[2]
      {
        (object) mods,
        (object) configs
      }, (object) null)[0];
    }

    private static bool validateModType(
      Type modType,
      ImmutableArray<ModData> allModTypes,
      DependencyResolver configsResolver,
      out string error,
      out ConstructorInfo constructor)
    {
      constructor = (ConstructorInfo) null;
      try
      {
        if (!modType.IsAssignableTo<IMod>())
        {
          error = string.Format("Mod '{0}' is not derived from '{1}'.", (object) modType, (object) typeof (IMod));
          return false;
        }
        if (!modType.IsSealed)
        {
          error = string.Format("Mod '{0}' is not sealed, derived mods are not supported.", (object) modType);
          return false;
        }
        ConstructorInfo[] constructors = modType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        if (constructors.Length == 0)
        {
          error = string.Format("Mod '{0}' does not have public constructor.", (object) modType);
          return false;
        }
        if (constructors.Length > 1)
        {
          error = string.Format("Mod '{0}' have more than one public constructor.", (object) modType);
          return false;
        }
        ParameterInfo[] parameters = constructors[0].GetParameters();
        foreach (ParameterInfo parameterInfo in parameters)
        {
          ParameterInfo pi = parameterInfo;
          if (pi.ParameterType.IsAssignableTo<IMod>())
          {
            if (!allModTypes.Any((Func<ModData, bool>) (x => x.ModType == pi.ParameterType)))
            {
              error = string.Format("Mod '{0}' cannot be instantiated because it requires ", (object) modType) + string.Format("a mod type '{0}' which is not installed.", (object) pi.ParameterType);
              return false;
            }
          }
          else if (pi.ParameterType.IsAssignableTo<IModConfig>())
          {
            if (configsResolver.TryResolve(pi.ParameterType).IsNone)
            {
              error = string.Format("Mod '{0}' cannot be instantiated because it requires ", (object) modType) + string.Format("a config of type '{0}' which is not present. You might need to ", (object) pi.ParameterType) + "mark your mod config with `[GlobalDependency(RegistrationMode.AsEverything)]` in order to get it automatically instantiated.";
              return false;
            }
          }
          else
          {
            error = string.Format("Mod '{0}' has invalid parameter of type '{1}'. ", (object) modType, (object) pi.ParameterType) + "Only parameters of types 'IMod' and 'IModConfig' are supported.";
            return false;
          }
        }
        constructor = constructors[0];
        error = "";
        return true;
      }
      catch (Exception ex)
      {
        error = string.Format("Mod '{0}' cannot be loaded: {1} ({2}).", (object) modType, (object) ex.Message, (object) ex.GetType().Name);
        return false;
      }
    }

    private static bool validateMod(IMod mod, out string error)
    {
      string name = mod.Name;
      if (string.IsNullOrWhiteSpace(name))
      {
        error = string.Format("Mod '{0}' has null or empty name.", (object) mod.GetType());
        return false;
      }
      if (GameBuilder.s_validName.Match(name).Length != name.Length)
      {
        error = string.Format("Name '{0}' of mod '{1}' is invalid.", (object) name, (object) mod.GetType());
        return false;
      }
      error = "";
      return true;
    }

    /// <summary>
    /// Registers dependencies and custom registrations of all given mods.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Game.GameBuilderException">On error.</exception>
    public static DependencyResolverBuilder RegisterModDependenciesOrThrow(
      ImmutableArray<IMod> mods,
      ProtosDb protosDb,
      ImmutableArray<IConfig> configs,
      IFileSystemHelper fsHelper,
      bool gameWasLoaded)
    {
      Assert.That<bool>(protosDb.IsReadonly).IsTrue("Protos DB should be locked at this stage.");
      DependencyResolverBuilder depBuilder = new DependencyResolverBuilder();
      depBuilder.RegisterInstance<ProtosDb>(protosDb).AsSelf();
      depBuilder.RegisterInstance<IFileSystemHelper>(fsHelper).As<IFileSystemHelper>();
      foreach (IConfig config in configs)
      {
        DependencyResolverBuilder.DependencyInstanceRegistrar<IConfig> instanceRegistrar = depBuilder.RegisterInstance<IConfig>(config);
        instanceRegistrar = instanceRegistrar.AsSelf();
        instanceRegistrar.AsAllInterfaces();
      }
      GameBuilder.registerDependenciesOrThrow(mods, depBuilder, protosDb, gameWasLoaded);
      depBuilder.SetShouldSerializePredicate((Predicate<Type>) (t => !t.IsAssignableTo<IConfig>()));
      return depBuilder;
    }

    /// <summary>
    /// Initializes all given mods with given resolver. Throws <see cref="T:Mafi.Core.Game.GameBuilderException" /> if any exception is
    /// throw during the initialization process.
    /// </summary>
    public static void InitializeModsOrThrow(
      ImmutableArray<IMod> mods,
      DependencyResolver resolver,
      bool gameWasLoaded)
    {
      foreach (IMod mod in mods)
      {
        try
        {
          mod.Initialize(resolver, gameWasLoaded);
        }
        catch (Exception ex)
        {
          throw new GameBuilderException(string.Format("An exception was thrown while initializing mod '{0}' ({1})", (object) mod.Name, (object) mod.GetType()), ex);
        }
      }
    }

    /// <summary>
    /// Registers all prototypes of all given mods. Throws <see cref="T:Mafi.Core.Game.GameBuilderException" /> if any exception is
    /// throw during the registration process when noThrow == false, otherwise captures and returns a map of
    /// failing mod to its exception string.
    /// </summary>
    public static Lyst<(IMod, Exception)> RegisterModsPrototypes(
      ImmutableArray<IMod> mods,
      ImmutableArray<IConfig> configs,
      ProtosDb protosDb,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnBeforeMods = null,
      Action<ProtoRegistrator, ImmutableArray<IMod>> protosDbRegFnAfterMods = null,
      bool noThrow = false)
    {
      ProtoRegistrator protoRegistrator = new ProtoRegistrator(protosDb, configs);
      if (protosDbRegFnBeforeMods != null)
        protosDbRegFnBeforeMods(protoRegistrator, mods);
      Lyst<(IMod, Exception)> lyst = GameBuilder.RegisterModsPrototypes(mods, protoRegistrator, noThrow);
      if (protosDbRegFnAfterMods != null)
        protosDbRegFnAfterMods(protoRegistrator, mods);
      ((IProtosDbFriend) protoRegistrator.PrototypesDb).LockAndInitializeProtos();
      return lyst;
    }

    /// <summary>
    /// Registers all prototypes of all given mods. Throws <see cref="T:Mafi.Core.Game.GameBuilderException" /> if any exception is
    /// throw during the registration process when noThrow == false, otherwise captures and returns a map of
    /// failing mod to its exception string.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Game.GameBuilderException">On error if noThrow == false.</exception>
    public static Lyst<(IMod, Exception)> RegisterModsPrototypes(
      ImmutableArray<IMod> mods,
      ProtoRegistrator protoRegistrator,
      bool noThrow = false)
    {
      Lyst<(IMod, Exception)> lyst = new Lyst<(IMod, Exception)>();
      foreach (IMod mod in mods)
      {
        protoRegistrator.PrototypesDb.SetActiveMod(mod);
        try
        {
          mod.RegisterPrototypes(protoRegistrator);
        }
        catch (Exception ex)
        {
          if (noThrow)
            lyst.Add((mod, ex));
          else
            throw;
        }
      }
      return lyst;
    }

    /// <summary>
    /// Registers all global and manual dependencies of all given mods. Throws <see cref="T:Mafi.Core.Game.GameBuilderException" /> if
    /// any exception is throw during the registration process.
    /// </summary>
    private static void registerDependenciesOrThrow(
      ImmutableArray<IMod> mods,
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded)
    {
      Assert.That<bool>(protosDb.IsReadonly).IsTrue();
      foreach (IMod mod in mods)
      {
        DependencyResolverBuilder.DependencyInstanceRegistrar<IMod> instanceRegistrar = depBuilder.RegisterInstance<IMod>(mod);
        instanceRegistrar = instanceRegistrar.AsSelf();
        instanceRegistrar.AsAllInterfaces();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      depBuilder.RegisterAllGlobalDependencies(typeof (DependencyResolver).Assembly, GameBuilder.\u003C\u003EO.\u003C0\u003E__notConfigPredicate ?? (GameBuilder.\u003C\u003EO.\u003C0\u003E__notConfigPredicate = new Predicate<Type>(notConfigPredicate)));
      foreach (IMod mod in mods)
      {
        try
        {
          Assembly assembly = mod.GetType().Assembly;
          if (!assembly.FullName.StartsWith("Mafi.Tests"))
          {
            if (!assembly.FullName.StartsWith("Mafi.IntegrationTests"))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              depBuilder.RegisterAllGlobalDependencies(assembly, GameBuilder.\u003C\u003EO.\u003C0\u003E__notConfigPredicate ?? (GameBuilder.\u003C\u003EO.\u003C0\u003E__notConfigPredicate = new Predicate<Type>(notConfigPredicate)));
            }
          }
        }
        catch (Exception ex)
        {
          throw new GameBuilderException("An exception was thrown while registering global dependencies of " + string.Format("mod '{0}' ({1}) from assembly '{2}'.", (object) mod.Name, (object) mod.GetType(), (object) mod.GetType().Assembly.FullName), ex);
        }
      }
      foreach (IMod mod in mods)
      {
        try
        {
          mod.RegisterDependencies(depBuilder, protosDb, gameWasLoaded);
        }
        catch (Exception ex)
        {
          throw new GameBuilderException("An exception was thrown while registering manual dependencies " + string.Format("of mod '{0}' ({1}).", (object) mod.Name, (object) mod.GetType()), ex);
        }
      }

      static bool notConfigPredicate(Type t) => !t.IsAssignableTo<IConfig>();
    }

    /// <summary>
    /// Goes through all given mods in order and finds all that implement <see cref="T:Mafi.Core.Mods.IRegistrationMod`1" /> and
    /// calls the <see cref="M:Mafi.Core.Mods.IRegistrationMod`1.Register(Mafi.Collections.ImmutableCollections.ImmutableArray{`0},Mafi.Core.Mods.RegistrationContext)" /> method with all matching mods.
    /// </summary>
    private static void registerCustomHandlers(
      ImmutableArray<IMod> mods,
      RegistrationContext context)
    {
      foreach (IMod mod in mods)
      {
        foreach (Type registrationInterface in mod.GetType().GetInterfaces())
        {
          if (registrationInterface.IsGenericType && registrationInterface.GetGenericTypeDefinition() == typeof (IRegistrationMod<>))
            GameBuilder.registerCustomMod(mod, registrationInterface, mods, context);
        }
      }
    }

    /// <summary>
    /// Calls <see cref="M:Mafi.Core.Mods.IRegistrationMod`1.Register(Mafi.Collections.ImmutableCollections.ImmutableArray{`0},Mafi.Core.Mods.RegistrationContext)" /> on given mod instance with given arguments.
    /// </summary>
    private static void registerCustomMod(
      IMod registrationMod,
      Type registrationInterface,
      ImmutableArray<IMod> allMods,
      RegistrationContext context)
    {
      Assert.That<bool>(registrationInterface.GetGenericTypeDefinition() == typeof (IRegistrationMod<>)).IsTrue();
      Type type = registrationInterface.GetGenericArguments().First<Type>();
      Lyst<object> lyst = allMods.Where(new Func<IMod, bool>(type.IsInstanceOfType)).Cast<object>().ToLyst<object>();
      if (lyst.IsEmpty)
      {
        Log.Info(string.Format("No mods available for registration of '{0}'.", (object) type));
      }
      else
      {
        MethodInfo method = registrationInterface.GetMethod("Register");
        Assert.That<MethodInfo>(method).IsNotNull<MethodInfo>();
        object[] andInit = ArrayPool<object>.GetAndInit(ImmutableArray.CreateGeneric(type, (IIndexable<object>) lyst), (object) context);
        method.Invoke((object) registrationMod, andInit);
        andInit.ReturnToPool<object>();
      }
    }

    static GameBuilder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameBuilder.s_validName = new Regex("[a-zA-Z0-9][a-zA-Z0-9_ +\\-()&#^*]*", RegexOptions.Compiled);
    }
  }
}
