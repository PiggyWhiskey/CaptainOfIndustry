// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ObjectIds.ReferenceEqualityComparer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Mafi.Serialization.ObjectIds
{
  /// <summary>
  /// Equality comparer comparing objects by their references.
  /// </summary>
  public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
  {
    public static readonly IEqualityComparer<T> Instance;

    private ReferenceEqualityComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    bool IEqualityComparer<T>.Equals(T x, T y) => (object) x == (object) y;

    int IEqualityComparer<T>.GetHashCode(T obj) => RuntimeHelpers.GetHashCode((object) obj);

    static ReferenceEqualityComparer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ReferenceEqualityComparer<T>.Instance = (IEqualityComparer<T>) new ReferenceEqualityComparer<T>();
    }
  }
}
