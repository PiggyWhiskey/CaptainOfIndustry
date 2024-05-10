// Decompiled with JetBrains decompiler
// Type: Mafi.ImmutableArrayAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ImmutableArrayAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<ImmutableArray<T>> actual, string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null but expected empty.", (object) typeof (T)), message);
      }
      else
      {
        if (!actual.Value.IsNotEmpty)
          return;
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not empty but expected empty.", (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<ImmutableArray<T>> actual, string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), message);
      }
      else
      {
        if (!actual.Value.IsEmpty)
          return;
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is empty but expected non-empty.", (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T, T0>(
      this Assertion<ImmutableArray<T>> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), message.FormatInvariant((object) arg0));
      }
      else
      {
        if (!actual.Value.IsEmpty)
          return;
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is empty but expected non-empty.", (object) typeof (T)), message.FormatInvariant((object) arg0));
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T>(
      this Assertion<ImmutableArray<T>> actual,
      int expectedLength,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), message);
      }
      else
      {
        if (actual.Value.Length == expectedLength)
          return;
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> has length {1} but length {2} was expected.", (object) typeof (T), (object) actual.Value.Length, (object) expectedLength), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T, T0>(
      this Assertion<ImmutableArray<T>> actual,
      int expectedLength,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), string.Format(message, (object) arg0));
      }
      else
      {
        if (actual.Value.Length == expectedLength)
          return;
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> has length {1} but length {2} was expected.", (object) typeof (T), (object) actual.Value.Length, (object) expectedLength), string.Format(message, (object) arg0));
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsValidIndexFor<T>(
      this Assertion<int> actual,
      ImmutableArray<T> array,
      string message = "")
    {
      if ((uint) actual.Value < (uint) array.Length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for ImmutableArray<{1}> of length {2}.", (object) actual.Value, (object) typeof (T), (object) array.Length), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      T value,
      string message = "")
    {
      if (actual.Value.Contains(value))
        return;
      Assert.FailAssertion(string.Format("ImmutableArray<{0}> does not contain value: {1}", (object) typeof (T), (object) value), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T, T0>(
      this Assertion<ImmutableArray<T>> actual,
      T value,
      string message,
      T0 arg0)
    {
      if (actual.Value.Contains(value))
        return;
      Assert.FailAssertion(string.Format("ImmutableArray<{0}> does not contain value: {1}", (object) typeof (T), (object) value), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      Predicate<T> predicate,
      string message = "")
    {
      if (actual.Value.Contains(predicate))
        return;
      Assert.FailAssertion(string.Format("ImmutableArray<{0}> does not contain value specified by a predicate.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      T value,
      string message = "")
    {
      if (!actual.Value.Contains(value))
        return;
      Assert.FailAssertion(string.Format("ImmutableArray<{0}> contains value: {1}", (object) typeof (T), (object) value), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null.", (object) typeof (T)), message);
      }
      else
      {
        int index = 0;
        for (int length = actual.Value.Length; index < length; ++index)
        {
          T obj = actual.Value[index];
          if (!predicate(obj))
          {
            Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in ImmutableArray<{2}>.", (object) obj, (object) index, (object) typeof (T)), message);
            break;
          }
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, int, bool> predicate,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null.", (object) typeof (T)), message);
      }
      else
      {
        int index = 0;
        for (int length = actual.Value.Length; index < length; ++index)
        {
          T obj = actual.Value[index];
          if (!predicate(obj, index))
          {
            Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in ImmutableArray<{2}>.", (object) obj, (object) index, (object) typeof (T)), message);
            break;
          }
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      foreach (T obj in actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of ImmutableArray<{0}> but expected to succeed at least once.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Any_DebugOnly<T, T0>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, bool> predicate,
      string message,
      T0 arg0)
    {
      foreach (T obj in actual.Value)
      {
        if (predicate(obj))
          return;
      }
      Assert.FailAssertion(string.Format("Predicate failed for all elements of ImmutableArray<{0}> but expected to succeed at least once.", (object) typeof (T)), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, TValue>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, TValue> selector,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null.", (object) typeof (T)), message);
      }
      else
      {
        int num1 = 0;
        Dict<TValue, int> dict = new Dict<TValue, int>(actual.Value.Length);
        foreach (TValue key in actual.Value.Map<TValue>(selector))
        {
          int num2;
          if (dict.TryGetValue(key, out num2))
          {
            Assert.FailAssertion(string.Format("All elements of ImmutableArray<{0}> are not distinct. Element '{1}' was found at indices {2} and {3}.", (object) typeof (T), (object) key, (object) num1, (object) num2), message);
            break;
          }
          dict[key] = num1;
          ++num1;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, T0>(
      this Assertion<ImmutableArray<T>> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null.", (object) typeof (T)), message.FormatInvariant((object) arg0));
      }
      else
      {
        int num1 = 0;
        Dict<T, int> dict = new Dict<T, int>(actual.Value.Length);
        foreach (T key in actual.Value)
        {
          int num2;
          if (dict.TryGetValue(key, out num2))
          {
            Assert.FailAssertion(string.Format("All elements of ImmutableArray<{0}> are not distinct. Element '{1}' was found at indices {2} and {3}.", (object) typeof (T), (object) key, (object) num1, (object) num2), message.FormatInvariant((object) arg0));
            break;
          }
          dict[key] = num1;
          ++num1;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      string message = "")
    {
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsSorted_DebugOnly<T>(
      this Assertion<ImmutableArray<T>> actual,
      Func<T, int> selector,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not initialized and internal array is null.", (object) typeof (T)), message);
      }
      else
      {
        if (actual.Value.IsEmpty)
          return;
        int num1 = selector(actual.Value[0]);
        int num2 = 1;
        for (int length = actual.Value.Length; num2 < length; ++num2)
        {
          int num3 = selector(actual.Value[1]);
          if (num3 < num1)
          {
            Assert.FailAssertion(string.Format("ImmutableArray<{0}> is not sorted between indices {1} and {2}, ", (object) typeof (T), (object) (num2 - 1), (object) num2) + string.Format("values {0} !<= {1} (length {2}).", (object) num1, (object) num3, (object) length), message);
            break;
          }
          num1 = num3;
        }
      }
    }
  }
}
