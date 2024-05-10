// Decompiled with JetBrains decompiler
// Type: Mafi.HashSetAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi
{
  public static class HashSetAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<HashSet<T>> actual, string message = "")
    {
      if (actual.Value.Count == 0)
        return;
      Assert.FailAssertion(string.Format("HashSet<{0}> expected to be empty but it contains '{1}' values.", (object) typeof (T), (object) actual.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<HashSet<T>> actual, string message = "")
    {
      if (actual.Value.Count > 0)
        return;
      Assert.FailAssertion(string.Format("HashSet<{0}> expected not empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void Contains<T>(this Assertion<HashSet<T>> set, T expected, string message = "")
    {
      if (set.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("HashSet<{0}> expected to contain value '{1}' but the HashSet is empty.", (object) typeof (T), (object) expected), message);
      }
      else
      {
        if (set.Value.Contains(expected))
          return;
        Assert.FailAssertion(string.Format("HashSet<{0}> expected to contain value '{1}' but the value was not found (Count={2}).", (object) typeof (T), (object) expected, (object) set.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContains<T>(this Assertion<HashSet<T>> set, T expected, string message = "")
    {
      if (!set.Value.Contains(expected))
        return;
      Assert.FailAssertion(string.Format("HashSet<{0}> expected to not contain value '{1}' but the value was found (Count={2}).", (object) typeof (T), (object) expected, (object) set.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContains<T>(
      this Assertion<HashSet<T>> set,
      Func<T, bool> predicate,
      string message = "")
    {
      if (!set.Value.Any<T>(predicate))
        return;
      Assert.FailAssertion(string.Format("HashSet<{0}> has an unexpected value reported by predicate (Count={1}).", (object) typeof (T), (object) set.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContains<T, T0, T1>(
      this Assertion<HashSet<T>> set,
      Func<T, bool> predicate,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (!set.Value.Any<T>(predicate))
        return;
      Assert.FailAssertion(string.Format("HashSet<{0}> has an unexpected value reported by predicate (Count={1}).", (object) typeof (T), (object) set.Value.Count), message.FormatInvariant((object) arg0, (object) arg1));
    }
  }
}
