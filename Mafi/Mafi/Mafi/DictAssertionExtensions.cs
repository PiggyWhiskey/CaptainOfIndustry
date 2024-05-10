// Decompiled with JetBrains decompiler
// Type: Mafi.DictAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi
{
  public static class DictAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      string message = "")
    {
      if (dict.Value.Count == 0)
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to be empty but contains {2} elements.", (object) typeof (TKey), (object) typeof (TValue), (object) dict.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      string message = "")
    {
      if (dict.Value.Count != 0)
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to be not empty.", (object) typeof (TKey), (object) typeof (TValue)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void ContainsKey<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      TKey key,
      string message = "")
    {
      if (dict.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the Dictionary is empty.", (object) typeof (TKey), (object) typeof (TValue), (object) key), message);
      }
      else
      {
        if (dict.Value.ContainsKey(key))
          return;
        Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the key was not found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void ContainsKey_DebugOnly<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      TKey key,
      string message = "")
    {
      if (dict.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the Dictionary is empty.", (object) typeof (TKey), (object) typeof (TValue), (object) key), message);
      }
      else
      {
        if (dict.Value.ContainsKey(key))
          return;
        Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the key was not found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void ContainsKeyValue<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      TKey key,
      TValue expectedValue,
      string message = "")
    {
      if (dict.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the Dictionary is empty.", (object) typeof (TKey), (object) typeof (TValue), (object) key), message);
      }
      else
      {
        TValue x;
        if (!dict.Value.TryGetValue(key, out x))
        {
          Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to contain key '{2}' but the key was not found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), message);
        }
        else
        {
          if (EqualityComparer<TValue>.Default.Equals(x, expectedValue))
            return;
          Assert.FailAssertion(string.Format("Dict<{0},{1}>[{2}] expected to contain value '{3}' but the value '{4}' found instead (Count={5}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) expectedValue, (object) x, (object) dict.Value.Count), message);
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContainsKey<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      TKey key,
      string message = "")
    {
      if (!dict.Value.ContainsKey(key))
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to not contain key '{2}' but the key was found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContainsKey<TKey, TValue, T0>(
      this Assertion<Dict<TKey, TValue>> dict,
      TKey key,
      string message,
      T0 arg0)
    {
      if (!dict.Value.ContainsKey(key))
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to not contain key '{2}' but the key was found (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) key, (object) dict.Value.Count), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContainsValue_DebugOnly<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      TValue value,
      string message = "")
    {
      if (!dict.Value.ContainsValue(value))
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to not contain a value '{2}' (Count={3}).", (object) typeof (TKey), (object) typeof (TValue), (object) value, (object) dict.Value.Count), string.Format(message));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContainsValue_DebugOnly<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> dict,
      Predicate<TValue> predicate,
      string message = "")
    {
      if (dict.Value.All<KeyValuePair<TKey, TValue>>((Func<KeyValuePair<TKey, TValue>, bool>) (x => !predicate(x.Value))))
        return;
      Assert.FailAssertion(string.Format("Dict<{0},{1}> expected to not contain a value based on given predicate (Count={2}).", (object) typeof (TKey), (object) typeof (TValue), (object) dict.Value.Count), string.Format(message));
    }
  }
}
