// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ComparisonComparer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  internal sealed class ComparisonComparer<T> : Comparer<T>
  {
    private readonly Comparison<T> m_comparison;

    public ComparisonComparer(Comparison<T> comparison)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_comparison = comparison;
    }

    public override int Compare(T x, T y) => this.m_comparison(x, y);
  }
}
