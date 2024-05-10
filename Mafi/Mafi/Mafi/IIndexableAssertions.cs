// Decompiled with JetBrains decompiler
// Type: Mafi.IIndexableAssertions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ReadonlyCollections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class IIndexableAssertions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<IIndexable<T>> indexable, string message = "")
    {
      if (indexable.Value.IsEmpty<T>())
        return;
      Assert.FailAssertion(string.Format("IIndexable<{0}> expected to be empty but it contains '{1}' values.", (object) typeof (T), (object) indexable.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<IIndexable<T>> indexable, string message = "")
    {
      if (indexable.Value.IsNotEmpty<T>())
        return;
      Assert.FailAssertion(string.Format("IIndexable<{0}> expected not empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(
      this Assertion<IIndexable<T>> indexable,
      T expected,
      string message = "")
    {
      if (indexable.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("IIndexable<{0}> expected to contain value '{1}' but it is empty.", (object) typeof (T), (object) expected), message);
      }
      else
      {
        if (indexable.Value.Contains<T>(expected))
          return;
        Assert.FailAssertion(string.Format("IIndexable<{0}> expected to contain value '{1}' but the value was not found (Count={2}).", (object) typeof (T), (object) expected, (object) indexable.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<IIndexable<T>> indexable,
      T value,
      string message = "")
    {
      if (!indexable.Value.Contains<T>(value))
        return;
      Assert.FailAssertion(string.Format("IIndexable<{0}> contains value: {1}", (object) typeof (T), (object) value), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void ValuesAreEqualTo_DebugOnly<T>(
      this Assertion<IIndexable<T>> indexable,
      IIndexable<T> expected,
      string message = "")
    {
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      if (indexable.Value.Count != expected.Count)
      {
        int num = indexable.Value.Count.Min(expected.Count);
        for (int index = 0; index < num; ++index)
        {
          if (!equalityComparer.Equals(indexable.Value[index], expected[index]))
            Assert.FailAssertion(string.Format("Non-matching IIndexable<{0}> has length {1} but ", (object) typeof (T), (object) indexable.Value.Count) + string.Format("{0} was expected. The first mismatch is at index {1} with value", (object) expected.Count, (object) index) + string.Format(" '{0}' but '{1}' was expected.", (object) indexable.Value[index], (object) expected[index]), message);
        }
      }
      else
      {
        for (int index = 0; index < expected.Count; ++index)
        {
          if (!equalityComparer.Equals(indexable.Value[index], expected[index]))
            Assert.FailAssertion(string.Format("Non-matching IIndexable<{0}>. The first mismatch is at index {1} with value ", (object) typeof (T), (object) index) + string.Format(" '{0}' but '{1}' was expected.", (object) indexable.Value[index], (object) expected[index]), message);
        }
      }
    }
  }
}
