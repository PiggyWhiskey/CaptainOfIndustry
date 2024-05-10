// Decompiled with JetBrains decompiler
// Type: Mafi.SmallImmutableArrayAssertionExtensions
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
  public static class SmallImmutableArrayAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<SmallImmutableArray<T>> actual, string message = "")
    {
      if (!actual.Value.IsNotEmpty)
        return;
      Assert.FailAssertion(string.Format("SmallImmutableArray<{0}> is not empty but expected empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<SmallImmutableArray<T>> actual, string message = "")
    {
      if (!actual.Value.IsEmpty)
        return;
      Assert.FailAssertion(string.Format("SmallImmutableArray<{0}> is empty but expected non-empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T>(
      this Assertion<SmallImmutableArray<T>> actual,
      int expectedLength,
      string message = "")
    {
      if (actual.Value.Length == expectedLength)
        return;
      Assert.FailAssertion(string.Format("SmallImmutableArray<{0}> has length {1} but length {2} was expected.", (object) typeof (T), (object) actual.Value.Length, (object) expectedLength), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<SmallImmutableArray<T>> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      int index = 0;
      for (int length = actual.Value.Length; index < length; ++index)
      {
        T obj = actual.Value[index];
        if (!predicate(obj))
        {
          Assert.FailAssertion(string.Format("Predicate failed for element '{0}' at index {1} in SmallImmutableArray<{2}>.", (object) obj, (object) index, (object) typeof (T)), message);
          break;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T, TValue>(
      this Assertion<SmallImmutableArray<T>> actual,
      Func<T, TValue> selector,
      string message = "")
    {
      int num1 = 0;
      Dict<TValue, int> dict = new Dict<TValue, int>();
      foreach (TValue key in actual.Value.Map<TValue>(selector))
      {
        int num2;
        if (dict.TryGetValue(key, out num2))
        {
          Assert.FailAssertion(string.Format("All elements of SmallImmutableArray<{0}> are not distinct. Element '{1}' was found at indices {2} and {3}.", (object) typeof (T), (object) key, (object) num1, (object) num2), message);
          break;
        }
        dict[key] = num1;
        ++num1;
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void AreDistinct_DebugOnly<T>(
      this Assertion<SmallImmutableArray<T>> actual,
      string message = "")
    {
    }
  }
}
