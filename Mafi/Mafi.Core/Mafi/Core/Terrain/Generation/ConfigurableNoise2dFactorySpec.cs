// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ConfigurableNoise2dFactorySpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public class ConfigurableNoise2dFactorySpec
  {
    public Dict<string, Type> Parameters;
    public ImmutableArray<ConfigurableNoise2dFactorySpec.Block> StatementsBlocks;

    public ConfigurableNoise2dFactorySpec(
      Dict<string, Type> parameters,
      ImmutableArray<ConfigurableNoise2dFactorySpec.Block> statementsBlocks)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Parameters = parameters;
      this.StatementsBlocks = statementsBlocks;
    }

    public readonly struct Block
    {
      public readonly string Name;
      public readonly ImmutableArray<Pair<string, ImmutableArray<string>>> Statements;

      public Block(
        string name,
        ImmutableArray<Pair<string, ImmutableArray<string>>> statements)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Name = name;
        this.Statements = statements;
      }
    }
  }
}
