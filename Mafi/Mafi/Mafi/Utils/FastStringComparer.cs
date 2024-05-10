// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.FastStringComparer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Utils
{
  public class FastStringComparer : IEqualityComparer<string>
  {
    public static readonly FastStringComparer Instance;

    private FastStringComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool Equals(string x, string y)
    {
      if (x == null)
        return y == null;
      return y != null && x.Equals(y, StringComparison.Ordinal);
    }

    public int GetHashCode(string str) => (int) FarmHash.Hash32(str);

    static FastStringComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      FastStringComparer.Instance = new FastStringComparer();
    }
  }
}
