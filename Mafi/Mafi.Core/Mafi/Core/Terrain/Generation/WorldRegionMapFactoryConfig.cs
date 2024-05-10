// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMapFactoryConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public sealed class WorldRegionMapFactoryConfig : IConfig
  {
    public Type FactoryType { get; private set; }

    public Option<object> Config { get; private set; }

    public WorldRegionMapFactoryConfig(Type factoryType, Option<object> config)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Type>(factoryType).IsAssignableTo<IWorldRegionMapFactory>("Invalid map generator type.");
      this.FactoryType = factoryType;
      this.Config = config;
    }
  }
}
