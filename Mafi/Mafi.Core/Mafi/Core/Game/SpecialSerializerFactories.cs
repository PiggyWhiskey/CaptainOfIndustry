// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.SpecialSerializerFactories
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Game
{
  [SerializeAsGlobalDep]
  public sealed class SpecialSerializerFactories
  {
    public readonly ImmutableArray<ISpecialSerializerFactory> SpecialSerializersForConfigs;
    public readonly ImmutableArray<ISpecialSerializerFactory> SpecialSerializersForGame;

    private SpecialSerializerFactories(
      ImmutableArray<ISpecialSerializerFactory> specialSerializersForConfigs,
      ImmutableArray<ISpecialSerializerFactory> specialSerializersForGame)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SpecialSerializersForConfigs = specialSerializersForConfigs;
      this.SpecialSerializersForGame = specialSerializersForGame;
    }

    public static SpecialSerializerFactories CreateFromAssemblies(
      Set<Assembly> assemblies,
      ProtosDb protosDb,
      ImmutableArray<IConfig> configs)
    {
      ImmutableArray<Type> immutableArray = assemblies.SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (x => (IEnumerable<Type>) x.GetTypes())).Where<Type>((Func<Type, bool>) (x => x.IsClass && !x.IsAbstract && x.IsAssignableTo<ISpecialSerializerFactory>() && !x.IsAssignableTo<ISpecialSerializerFactoryCustom>())).ToImmutableArray<Type>();
      DependencyResolverBuilder dependencyResolverBuilder = new DependencyResolverBuilder();
      dependencyResolverBuilder.RegisterInstance<ProtosDb>(protosDb).AsSelf();
      foreach (IConfig config in configs)
        dependencyResolverBuilder.RegisterInstance<IConfig>(config).AsSelf();
      DependencyResolver resolver = dependencyResolverBuilder.BuildAndClear();
      ImmutableArray<ISpecialSerializerFactory> specialSerializersForGame = immutableArray.Map<ISpecialSerializerFactory>((Func<Type, ISpecialSerializerFactory>) (x => resolver.InstantiateAs<ISpecialSerializerFactory>(x, Array.Empty<object>())));
      resolver.TerminateAndClear();
      return new SpecialSerializerFactories(SpecialSerializerFactories.GetSerializersForConfigs(), specialSerializersForGame);
    }

    public static ImmutableArray<ISpecialSerializerFactory> GetSerializersForConfigs()
    {
      return ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new NoProtoAllowedSerializerFactory("Protos cannot be serialized in configs. Save their ID if you must.", "Protos cannot be deserialized from configs."));
    }
  }
}
