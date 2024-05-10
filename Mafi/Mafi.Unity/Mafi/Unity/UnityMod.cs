// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UnityMod
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.MessageNotifications.Handlers;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiFramework;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public sealed class UnityMod : IMod
  {
    public string Name => "Mafi-Unity";

    public int Version => 0;

    public bool IsUiOnly => true;

    public Option<IConfig> ModConfig => this.Config.SomeOption<IConfig>();

    public UnityModConfig Config { get; }

    /// <summary>
    /// Unity mod depends on <see cref="T:Mafi.Core.CoreMod" />
    /// </summary>
    public UnityMod(CoreMod coreMod, BaseMod baseMod, UnityModConfig config)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
    }

    void IMod.RegisterPrototypes(ProtoRegistrator registrator)
    {
    }

    void IMod.RegisterDependencies(
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded)
    {
      if (this.Config.DisableUi)
      {
        depBuilder.ClearAllDepsImplementing<IUnityUi>();
        depBuilder.ClearAllDepsImplementing<IUiElement>();
        depBuilder.ClearAllDepsImplementing<GameOverNotificationHandler>();
        depBuilder.ClearAllDepsImplementing<LocationExploredNotificationHandler>();
        depBuilder.ClearAllDepsImplementing<NewMessageNotificationHandler>();
        depBuilder.ClearAllDepsImplementing<NewRefugeesNotificationHandler>();
        depBuilder.ClearAllDepsImplementing<ShipInBattleNotificationHandler>();
        depBuilder.ClearAllDepsImplementing<InspectorContext>();
      }
      if (!this.Config.DisableTerrainRendering)
        return;
      depBuilder.ClearRegistrations<TerrainRenderer>();
      DependencyResolverBuilder.DependencyRegistrar dependencyRegistrar = depBuilder.RegisterDependency<DummyTerrainRenderer>();
      dependencyRegistrar.AsAllInterfaces();
      depBuilder.ClearRegistrations<TreeRenderer>();
      dependencyRegistrar = depBuilder.RegisterDependency<DummyTreeRenderer>();
      dependencyRegistrar.AsAllInterfaces();
    }

    void IMod.EarlyInit(DependencyResolver resolver)
    {
    }

    void IMod.Initialize(DependencyResolver resolver, bool wasLoaded)
    {
      if (!this.Config.IncludeGenLayoutForEntities)
        return;
      resolver.Resolve<LayoutEntityModelFactory>().AlwaysIncludeGeneratedLayout = true;
    }
  }
}
