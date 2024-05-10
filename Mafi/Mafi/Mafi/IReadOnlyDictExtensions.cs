// Decompiled with JetBrains decompiler
// Type: Mafi.IReadOnlyDictExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class IReadOnlyDictExtensions
  {
    public static TValue SampleRandomValueOrDefault<TKey, TValue>(
      this IReadOnlyDictionary<TKey, TValue> dict,
      IRandom random)
    {
      if (dict.Count == 0)
        return default (TValue);
      int num = random.NextInt(dict.Count);
      foreach (TValue obj in dict.Values)
      {
        if (num <= 0)
          return obj;
        --num;
      }
      Assert.Fail("This should not happen.");
      return default (TValue);
    }
  }
}
