// Decompiled with JetBrains decompiler
// Type: Mafi.IntAssertionAndChecksExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class IntAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<int> actual, int other, string message = "")
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<int> actual, int other, string message = "")
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<int> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<int> actual, string message = "")
    {
      if (actual.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckZero(this int value)
    {
      if (value == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected 0.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<int> actual, string message = "")
    {
      if (actual.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckNotZero(this int value, int validValue)
    {
      if (value != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected NOT 0.", (object) value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<int> actual, int expected, string message = "")
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected < {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckLess(this int value, int other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<int> actual, int expected, string message = "")
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected <= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckLessOrEqual(this int value, int other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<int> actual, int expected, string message = "")
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected > {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckGreater(this int value, int other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(this Assertion<int> actual, int expected, string message = "")
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected >= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<int> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckGreaterOrEqual(this int value, int other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<int> actual, string message = "")
    {
      if (actual.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckPositive(this int value)
    {
      if (value > 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected positive.", (object) value));
      return 1;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<int> actual, string message = "")
    {
      if (actual.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckNotPositive(this int value)
    {
      if (value <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected NOT positive.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<int> actual, string message = "")
    {
      if (actual.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckNegative(this int value)
    {
      if (value < 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected negative.", (object) value));
      return -1;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<int> actual, string message = "")
    {
      if (actual.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckNotNegative(this int value)
    {
      if (value >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, expected NOT negative.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckWithinIncl(this int value, int minIncl, int maxIncl)
    {
      if (value < minIncl)
      {
        Log.Error(string.Format("CHECK FAIL: Value of int is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<int> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static int CheckWithinExcl(this int value, int minIncl, int maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of int is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<int> actual,
      int expected,
      int tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<int> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<int> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<int> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndex(this Assertion<int> actual, int length, string message = "")
    {
      if (actual.Value < length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for collection of length {1}.", (object) actual.Value, (object) length), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndex<T0>(
      this Assertion<int> actual,
      int length,
      string message,
      T0 arg0)
    {
      if (actual.Value < length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for collection of length {1}.", (object) actual.Value, (object) length), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndex<T0, T1>(
      this Assertion<int> actual,
      int length,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for collection of length {1}.", (object) actual.Value, (object) length), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndex<T0, T1, T2>(
      this Assertion<int> actual,
      int length,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for collection of length {1}.", (object) actual.Value, (object) length), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo(this Assertion<int> actual, string message = "")
    {
      if (actual.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected a power of 2.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0>(this Assertion<int> actual, string message, T0 arg0)
    {
      if (actual.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected a power of 2.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected a power of 2.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1, T2>(
      this Assertion<int> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of int is {0}, expected a power of 2.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
