// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.IReadOnlySetExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  public static class IReadOnlySetExtensions
  {
    public static T SampleRandomValueOrDefault<T>(this IReadOnlySet<T> set, IRandom random)
    {
      if (set.Count == 0)
        return default (T);
      int num = random.NextInt(set.Count);
      foreach (T obj in (IEnumerable<T>) set)
      {
        if (num <= 0)
          return obj;
        --num;
      }
      Assert.Fail("This should not happen.");
      return default (T);
    }
  }
}
