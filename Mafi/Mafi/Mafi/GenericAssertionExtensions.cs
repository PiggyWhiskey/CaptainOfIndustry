// Decompiled with JetBrains decompiler
// Type: Mafi.GenericAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class GenericAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsTrue<T>(this Assertion<T> actual, Predicate<T> predicate, string message = "")
    {
      if (predicate(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Predicate on '{0}' failed", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNull<T>(this Assertion<T> actual, string message = "") where T : class
    {
      if ((object) actual.Value == null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is not null but null was expected.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNull<T, T0>(this Assertion<T> actual, string message, T0 arg0) where T : class
    {
      if ((object) actual.Value == null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is not null but null was expected.", (object) typeof (T)), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNull<T>(this Assertion<T?> actual, string message = "") where T : struct
    {
      if (!actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is not null but null was expected.", (object) typeof (T?)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsNull_DebugOnly<T>(this Assertion<T> actual, string message = "") where T : class
    {
      if ((object) actual.Value == null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is not null but null was expected.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNull<T>(this Assertion<T> actual, string message = "") where T : class
    {
      if ((object) actual.Value != null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is null but expected != null.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsNotNull_DebugOnly<T>(this Assertion<T> actual, string message = "") where T : class
    {
      if ((object) actual.Value != null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is null but expected != null.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNull<T, T0>(this Assertion<T> actual, string message, T0 arg0) where T : class
    {
      if ((object) actual.Value != null)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is null but expected != null.", (object) typeof (T)), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNull<T>(this Assertion<T?> actual, string message = "") where T : struct
    {
      if (actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("Value of {0} is null but expected != null.", (object) typeof (T?)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T>(this Assertion<T> actual, T expected, string message = "")
    {
      if (EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsEqualTo_DebugOnly<T>(this Assertion<T> actual, T expected, string message = "")
    {
      if (EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsEqualTo_DebugOnly<T, T0>(
      this Assertion<T> actual,
      T expected,
      string message,
      T0 arg0)
    {
      if (EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T, T0>(
      this Assertion<T> actual,
      T expected,
      string message,
      T0 arg0)
    {
      if (EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T>(this Assertion<T> actual, T expected, string message = "")
    {
      if (!EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value != '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsNotEqualTo_DebugOnly<T>(
      this Assertion<T> actual,
      T expected,
      string message = "")
    {
      if (!EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value != '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T, T0>(
      this Assertion<T> actual,
      T expected,
      string message,
      T0 arg0)
    {
      if (!EqualityComparer<T>.Default.Equals(actual.Value, expected))
        return;
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but value != '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) expected), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotDefaultStruct<T>(this Assertion<T> actual, string message = "") where T : struct
    {
      if (!EqualityComparer<T>.Default.Equals(actual.Value, default (T)))
        return;
      Assert.FailAssertion(string.Format("Value of struct {0} is a default (empty) value.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsNotDefaultStruct_DebugOnly<T>(this Assertion<T> actual, string message = "") where T : struct
    {
      if (!EqualityComparer<T>.Default.Equals(actual.Value, default (T)))
        return;
      Assert.FailAssertion(string.Format("Value of struct {0} is a default (empty) value.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T>(this Assertion<T> actual, T other, string message = "") where T : IComparable<T>
    {
      if (actual.Value.CompareTo(other) < 0)
        return;
      Assert.FailAssertion(string.Format("{0} is not less than {1}", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T>(this Assertion<T> actual, T other, string message = "") where T : IComparable<T>
    {
      if (actual.Value.CompareTo(other) <= 0)
        return;
      Assert.FailAssertion(string.Format("{0} is not less or equal to {1}", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T>(this Assertion<T> actual, T other, string message = "") where T : IComparable<T>
    {
      if (actual.Value.CompareTo(other) > 0)
        return;
      Assert.FailAssertion(string.Format("{0} is not greater than {1}", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T>(this Assertion<T> actual, T other, string message = "") where T : IComparable<T>
    {
      if (actual.Value.CompareTo(other) >= 0)
        return;
      Assert.FailAssertion(string.Format("{0} is not greater or equal to {1}", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T>(
      this Assertion<T> actual,
      T lowIncl,
      T highExcl,
      string message = "")
      where T : IComparable<T>
    {
      if (actual.Value.CompareTo(lowIncl) >= 0 && actual.Value.CompareTo(highExcl) < 0)
        return;
      Assert.FailAssertion(string.Format("{0} is not within {1} (incl) and {2} (excl).", (object) actual.Value, (object) lowIncl, (object) highExcl), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrOneIsNull<T>(this Assertion<T?> actual, T? other, string message = "") where T : struct, IComparable<T>
    {
      if (!actual.Value.HasValue || !other.HasValue)
        return;
      Assert.That<T>(actual.Value.Value).IsLess<T>(other.Value, message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsEqualToAny_DebugOnly<T>(
      this Assertion<T> actual,
      params T[] possibleValues)
    {
      foreach (T possibleValue in possibleValues)
      {
        if (EqualityComparer<T>.Default.Equals(actual.Value, possibleValue))
          return;
      }
      Assert.FailAssertion(string.Format("Value of {0} is '{1}' but one of: '{2}' was expected.", (object) typeof (T), (object) actual.Value, (object) string.Join<T>(", ", (IEnumerable<T>) possibleValues)), "");
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T>(
      this Assertion<T> actual,
      Predicate<T> predicate,
      string message = "")
    {
      if (predicate(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Predicate on '{0}' ({1}) failed", (object) actual.Value, (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T, T0>(
      this Assertion<T> actual,
      Predicate<T> predicate,
      string message,
      T0 arg0)
    {
      if (predicate(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Predicate on '{0}' ({1}) failed", (object) actual.Value, (object) typeof (T)), message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T, T0, T1>(
      this Assertion<T> actual,
      Predicate<T> predicate,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (predicate(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Predicate on '{0}' ({1}) failed", (object) actual.Value, (object) typeof (T)), message.FormatInvariant((object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T, U>(
      this Assertion<T> actual,
      U extra,
      Func<T, U, bool> predicate,
      string message = "")
    {
      if (predicate(actual.Value, extra))
        return;
      Assert.FailAssertion(string.Format("Predicate on '{0}' ({1}) failed with argument {2}", (object) actual.Value, (object) typeof (T), (object) extra), message);
    }
  }
}
