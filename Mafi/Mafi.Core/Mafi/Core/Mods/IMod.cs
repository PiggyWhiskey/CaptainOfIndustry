// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.IMod
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Game;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Mods
{
  /// <summary>
  /// A class marked by this interface is a game mod - a set of functionalities and data for the game.
  /// 
  /// Every mod can have dependencies which are listed as arguments of its constructor. All dependent mods are
  /// instantiated before the child mod. Cyclic dependencies are not supported. Optional dependencies are supported
  /// through <see cref="T:Mafi.Option" /> class. All the registration and initialization methods are called in the topological
  /// order, that is, a method of a mod is called only after all of its dependencies were called so the mod can be sure
  /// that all of its dependencies are initialized first.
  /// 
  /// When mod is registered and instantiated the <see cref="M:Mafi.Core.Mods.IMod.RegisterPrototypes(Mafi.Core.Mods.ProtoRegistrator)" /> method is called. In this method the
  /// mod should use the provided <see cref="T:Mafi.Core.Mods.ProtoRegistrator" /> class to register prototypes. It is also allowed to
  /// remove existing prototypes or replace them with different ones.
  /// 
  /// Then, mod's assembly is scanned for all types that are marked with the <see cref="T:Mafi.GlobalDependencyAttribute" />.
  /// These types are automatically registered to the dependency resolver builder. Additional types or instances can be
  /// registered with <see cref="M:Mafi.Core.Mods.IMod.RegisterDependencies(Mafi.DependencyResolverBuilder,Mafi.Core.Prototypes.ProtosDb,System.Boolean)" /> method using the provided <see cref="T:Mafi.DependencyResolverBuilder" />. Note that automatic registration of global dependencies happens only one per
  /// assembly in case there are more than one mod per assembly.
  /// 
  /// All implemented methods should be implemented explicitly to avoid accidental call from dependent classes. All
  /// mods implementations should be sealed classes.
  /// </summary>
  [MultiDependency]
  public interface IMod
  {
    /// <summary>
    /// Human-readable name of the mod. This value will be showed in-game.
    /// </summary>
    string Name { get; }

    /// <summary>Version of the mod.</summary>
    int Version { get; }

    /// <summary>
    /// Whether this mod is UI only and does not affect game state. All non-UI mods should not depend on or use Unity
    /// and UI-only mods should not contribute to game state changes.
    /// </summary>
    /// <remarks>UI mods are not instantiated in headless games such as determinism verification.</remarks>
    bool IsUiOnly { get; }

    /// <summary>
    /// Mod config. If set, it's instance will be registered in the resolver.
    /// If mafi-serializable, it will be persisted though saves and thi property will be set after load if a config of
    /// the same type was loaded from the save file.
    /// </summary>
    Option<IConfig> ModConfig { get; }

    /// <summary>Register all prototypes of this mod.</summary>
    void RegisterPrototypes(ProtoRegistrator registrator);

    /// <summary>
    /// Registers all dependencies such as components or custom implementations of any dependencies that should
    /// override default behaviors.
    /// 
    /// All prototypes of all mods are registered and prototypes database is locked before this method is called.
    /// </summary>
    void RegisterDependencies(
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded);

    /// <summary>
    /// Early-initialization that is called right after the dependency resolver is created, before the map is
    /// even generated. Called for both new and loaded games.
    /// WARNING: During load, this is called after initial resolver load is finished, but before init-after-load
    /// methods marked with <see cref="T:Mafi.Serialization.InitAfterLoadAttribute" /> are called. Use this with care.
    /// </summary>
    void EarlyInit(DependencyResolver resolver);

    /// <summary>
    /// Called exactly once before the game starts and after all the mods are registered and dependency builder is
    /// created.
    /// </summary>
    /// <remarks>
    /// This is the only place where mods can do some pre-processing, initialization, and checking after the whole
    /// game is loaded and ready to start. We use this for loading of UI that cannot be done in any other step since
    /// not all protos are loaded.
    /// </remarks>
    void Initialize(DependencyResolver resolver, bool gameWasLoaded);
  }
}
