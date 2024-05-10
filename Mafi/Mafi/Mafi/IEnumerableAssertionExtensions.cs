// Decompiled with JetBrains decompiler
// Type: Mafi.IEnumerableAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Serialization.ObjectIds;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class IEnumerableAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<IEnumerable<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      int num = 0;
      foreach (T obj in actual.Value)
      {
        if (!predicate(obj))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in IEnumerable<{2}>.", (object) obj, (object) num, (object) typeof (T)), message);
          break;
        }
        ++num;
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T, T0>(
      this Assertion<IEnumerable<T>> actual,
      Func<T, bool> predicate,
      string message,
      T0 arg0)
    {
      int num = 0;
      foreach (T obj in actual.Value)
      {
        if (!predicate(obj))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in IEnumerable<{2}>.", (object) obj, (object) num, (object) typeof (T)), message.FormatInvariant((object) arg0));
          break;
        }
        ++num;
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<TKey, TValue>(
      this Assertion<IReadOnlyDictionary<TKey, TValue>> actual,
      Func<KeyValuePair<TKey, TValue>, bool> predicate,
      string message = "")
    {
      int num = 0;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) actual.Value)
      {
        if (!predicate(keyValuePair))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in IDict<{2}, {3}>.", (object) keyValuePair, (object) num, (object) typeof (TKey), (object) typeof (TValue)), message);
          break;
        }
        ++num;
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<TKey, TValue>(
      this Assertion<Dict<TKey, TValue>> actual,
      Func<KeyValuePair<TKey, TValue>, bool> predicate,
      string message = "")
    {
      int num = 0;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in actual.Value)
      {
        if (!predicate(keyValuePair))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in IDict<{2}, {3}>.", (object) keyValuePair, (object) num, (object) typeof (TKey), (object) typeof (TValue)), message);
          break;
        }
        ++num;
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<IList<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      IList<T> objList = actual.Value;
      for (int index = 0; index < objList.Count; ++index)
      {
        T obj = objList[index];
        if (!predicate(obj))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in IEnumerable<{2}>.", (object) obj, (object) index, (object) typeof (T)), message);
          break;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T>(
      this Assertion<IList<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      foreach (T obj in (IEnumerable<T>) actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of Lyst<{0}> but expected to succeed at least once.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T>(
      this Assertion<IEnumerable<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      foreach (T obj in actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of IEnumerable<{0}> but expected to succeed at least once.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T, T0>(
      this Assertion<IEnumerable<T>> actual,
      Func<T, bool> predicate,
      string message,
      T0 arg0)
    {
      foreach (T obj in actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of IEnumerable<{0}> but expected to succeed at least once.", (object) typeof (T)), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T>(this Assertion<Lyst<T>> actual, string message = "")
    {
      string message1;
      if (IEnumerableAssertionExtensions.areDistinctImpl<T, T>((IEnumerable<T>) actual.Value, (Func<T, T>) (x => x), out message1))
        return;
      Assert.FailAssertion(message1, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, U>(
      this Assertion<Lyst<T>> actual,
      Func<T, U> selector,
      string message = "")
    {
      string message1;
      if (IEnumerableAssertionExtensions.areDistinctImpl<T, U>((IEnumerable<T>) actual.Value, selector, out message1))
        return;
      Assert.FailAssertion(message1, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T>(
      this Assertion<IEnumerable<T>> actual,
      string message = "")
    {
      string message1;
      if (IEnumerableAssertionExtensions.areDistinctImpl<T, T>(actual.Value, (Func<T, T>) (x => x), out message1))
        return;
      Assert.FailAssertion(message1, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, U>(
      this Assertion<IEnumerable<T>> actual,
      Func<T, U> selector,
      string message = "")
    {
      string message1;
      if (IEnumerableAssertionExtensions.areDistinctImpl<T, U>(actual.Value, selector, out message1))
        return;
      Assert.FailAssertion(message1, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, T0>(
      this Assertion<IEnumerable<T>> actual,
      string message,
      T0 arg0)
    {
      string message1;
      if (IEnumerableAssertionExtensions.areDistinctImpl<T, T>(actual.Value, (Func<T, T>) (x => x), out message1))
        return;
      Assert.FailAssertion(message1, message.FormatInvariant((object) arg0));
    }

    private static bool areDistinctImpl<T, U>(
      IEnumerable<T> seq,
      Func<T, U> selector,
      out string message)
    {
      int num1 = 0;
      Dict<U, int> dict = new Dict<U, int>(ReferenceEqualityComparer<U>.Instance);
      foreach (T obj in seq)
      {
        U key = selector(obj);
        int num2;
        if (dict.TryGetValue(key, out num2))
        {
          message = string.Format("Elements of {0} ('{1}') are not distinct. Element '{2}' (selected as '{3}')", (object) seq.GetType(), (object) typeof (T), (object) obj, (object) key) + string.Format(" was found at indices {0} and {1}.", (object) num1, (object) num2);
          return false;
        }
        dict[key] = num1;
        ++num1;
      }
      message = (string) null;
      return true;
    }
  }
}
