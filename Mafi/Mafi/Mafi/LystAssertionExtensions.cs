// Decompiled with JetBrains decompiler
// Type: Mafi.LystAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class LystAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T>(
      this Assertion<Lyst<T>> actual,
      int expectedLength,
      string message = "")
    {
      if (actual.Value == null)
      {
        Assert.FailAssertion(string.Format("Lyst of {0} is null but length {1} was expected.", (object) typeof (T), (object) expectedLength), message);
      }
      else
      {
        if (actual.Value.Count == expectedLength)
          return;
        Assert.FailAssertion(string.Format("Lyst of {0} has length {1} but length {2} was expected.", (object) typeof (T), (object) actual.Value.Count, (object) expectedLength), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<Lyst<T>> actual, string message = "")
    {
      if (actual.Value.IsEmpty)
        return;
      Assert.FailAssertion(string.Format("Lyst<{0}> expected to be empty but it contains '{1}' values.", (object) typeof (T), (object) actual.Value.Count), message + string.Format("\nFirst value: {0}", (object) actual.Value.First));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<Lyst<T>> actual, string message = "")
    {
      if (actual.Value.IsNotEmpty)
        return;
      Assert.FailAssertion(string.Format("Lyst<{0}> expected not empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(
      this Assertion<Lyst<T>> list,
      T expected,
      string message = "")
    {
      if (list.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("Lyst<{0}> expected to contain value '{1}' but it is empty.", (object) typeof (T), (object) expected), message);
      }
      else
      {
        if (list.Value.Contains(expected))
          return;
        Assert.FailAssertion(string.Format("Lyst<{0}> expected to contain value '{1}' but the value was not found (Count={2}).", (object) typeof (T), (object) expected, (object) list.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<Lyst<T>> list,
      T expected,
      string message = "")
    {
      int num = list.Value.IndexOf(expected);
      if (num < 0)
        return;
      Assert.FailAssertion(string.Format("Lyst<{0}> expected to not contain value '{1}' but the value was found at index {2} (Count={3}).", (object) typeof (T), (object) expected, (object) num, (object) list.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<Lyst<T>> list,
      Predicate<T> predicate,
      string message = "")
    {
      int index = list.Value.FindIndex(predicate);
      if (index < 0)
        return;
      Assert.FailAssertion(string.Format("Lyst<{0}> expected to not contain value based on give predicate but the value was found at index {1}: {2} (Count={3}).", (object) typeof (T), (object) index, (object) list.Value[index], (object) list.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T, T0, T1>(
      this Assertion<Lyst<T>> list,
      Predicate<T> predicate,
      string message,
      T0 arg0,
      T1 arg1)
    {
      int index = list.Value.FindIndex(predicate);
      if (index < 0)
        return;
      Assert.FailAssertion(string.Format("Lyst<{0}> expected to not contain value based on give predicate but the value was found at index {1}: {2} (Count={3}).", (object) typeof (T), (object) index, (object) list.Value[index], (object) list.Value.Count), message.FormatInvariant((object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<LystStruct<T>> list,
      T expected,
      string message = "")
    {
      int num = list.Value.IndexOf(expected);
      if (num < 0)
        return;
      Assert.FailAssertion(string.Format("LystStruct<{0}> expected to not contain value '{1}' but the value was found at index {2} (Count={3}).", (object) typeof (T), (object) expected, (object) num, (object) list.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T>(
      this Assertion<Lyst<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      foreach (T obj in actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of Lyst<{0}> but expected to succeed at least once.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<Lyst<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      Lyst<T> lyst = actual.Value;
      for (int index = 0; index < lyst.Count; ++index)
      {
        if (!predicate(lyst[index]))
          Assert.FailAssertion(string.Format("Predicate failed on index {0} (value={1}) of Lyst<{2}>.", (object) index, (object) lyst[index], (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void ContainsNoNulls_DebugOnly<T>(this Assertion<Lyst<T>> actual, string message = "") where T : class
    {
      int index = 0;
      for (int count = actual.Value.Count; index < count; ++index)
      {
        if ((object) actual.Value[index] == null)
        {
          Assert.FailAssertion(string.Format("Element at index {0} is null in Lyst<{1}>.", (object) index, (object) typeof (T)), message);
          break;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsOrderedAsc_DebugOnly<T>(this Assertion<Lyst<T>> indexable, string message = "")
    {
      Comparer<T> comparer = Comparer<T>.Default;
      for (int index = 1; index < indexable.Value.Count; ++index)
      {
        T x = indexable.Value[index - 1];
        T y = indexable.Value[index];
        if (comparer.Compare(x, y) >= 0)
          Assert.FailAssertion(string.Format("Lyst<{0}> not ordered in ascending order prevVal: {1}, currVal: {2}", (object) typeof (T), (object) x, (object) y), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndexFor<T>(this Assertion<int> actual, Lyst<T> list, string message = "")
    {
      if ((uint) actual.Value < (uint) list.Count)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for Lyst<{1}> of length {2}.", (object) actual.Value, (object) typeof (T), (object) list.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndexFor<T>(
      this Assertion<int> actual,
      LystStruct<T> list,
      string message = "")
    {
      if ((uint) actual.Value < (uint) list.Count)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for LystStruct<{1}> of length {2}.", (object) actual.Value, (object) typeof (T), (object) list.Count), message);
    }
  }
}
