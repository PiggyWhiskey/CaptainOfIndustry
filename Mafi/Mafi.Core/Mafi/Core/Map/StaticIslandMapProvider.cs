// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.StaticIslandMapProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  public class StaticIslandMapProvider : IIslandMapGenerator
  {
    private readonly StaticIslandMapProviderConfig m_config;
    private readonly IResolver m_resolver;
    private Option<IslandMap> m_generatedMap;

    public string Name { get; private set; }

    public StaticIslandMapProvider(StaticIslandMapProviderConfig config, IResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CName\u003Ek__BackingField = "StaticIslandMap_NotYetGenerated";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_resolver = resolver;
    }

    public IEnumerator<string> GenerateIslandMapTimeSliced()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new StaticIslandMapProvider.\u003CGenerateIslandMapTimeSliced\u003Ed__8(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public IslandMap GetMapAndClear()
    {
      Assert.That<Option<IslandMap>>(this.m_generatedMap).HasValue<IslandMap>("No map was generated.");
      IslandMap mapAndClear = this.m_generatedMap.Value;
      this.m_generatedMap = (Option<IslandMap>) Option.None;
      return mapAndClear;
    }
  }
}
