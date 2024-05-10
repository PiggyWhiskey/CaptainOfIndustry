// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.DescComparer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  public sealed class DescComparer<T> : IComparer<T>
  {
    public static readonly DescComparer<T> Default;
    private readonly Comparer<T> m_comparer;

    public DescComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_comparer = Comparer<T>.Default;
    }

    public int Compare(T x, T y) => this.m_comparer.Compare(y, x);

    static DescComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      DescComparer<T>.Default = new DescComparer<T>();
    }
  }
}
