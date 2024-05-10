// Decompiled with JetBrains decompiler
// Type: Mafi.AllImplementationsOf`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public sealed class AllImplementationsOf<T>
  {
    public static readonly AllImplementationsOf<T> Empty;
    public readonly ImmutableArray<T> Implementations;

    public AllImplementationsOf(ImmutableArray<T> impls)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Implementations = impls;
    }

    public AllImplementationsOf(params T[] impls)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Implementations = ((ICollection<T>) impls).ToImmutableArray<T>();
    }

    static AllImplementationsOf()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      AllImplementationsOf<T>.Empty = new AllImplementationsOf<T>(ImmutableArray<T>.Empty);
    }
  }
}
