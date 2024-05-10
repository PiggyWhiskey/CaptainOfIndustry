// Decompiled with JetBrains decompiler
// Type: Mafi.ReadOnlyDictionaryAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ReadOnlyDictionaryAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void ContainsKey<TKey, TValue>(
      this Assertion<IReadOnlyDictionary<TKey, TValue>> dict,
      TKey key,
      string message = "")
    {
      if (dict.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("IReadOnlyDictionary<{0},{1}> expected to contain key '{2}' but the Dictionary is empty.", (object) typeof (TKey), (object) typeof (TValue), (object) key), message);
      }
      else
      {
        if (dict.Value.ContainsKey(key))
          return;
        Assert.FailAssertion(string.Format("IReadOnlyDictionary<{0},{1}> expected to contain key '{2}' but the key was not found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContainsKey<TKey, TValue>(
      this Assertion<IReadOnlyDictionary<TKey, TValue>> dict,
      TKey key,
      string message = "")
    {
      if (!dict.Value.ContainsKey(key))
        return;
      Assert.FailAssertion(string.Format("IReadOnlyDictionary<{0},{1}> expected to NOT contain key '{2}'.", (object) typeof (TKey), (object) typeof (TValue), (object) key), message);
    }
  }
}
