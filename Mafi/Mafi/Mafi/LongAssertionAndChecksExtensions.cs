// Decompiled with JetBrains decompiler
// Type: Mafi.LongAssertionAndChecksExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class LongAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<long> actual, long other, string message = "")
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<long> actual, long other, string message = "")
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<long> actual,
      long other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<long> actual, string message = "")
    {
      if (actual.Value == 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value == 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckZero(this long value)
    {
      if (value == 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected 0.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<long> actual, string message = "")
    {
      if (actual.Value != 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value != 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckNotZero(this long value, long validValue)
    {
      if (value != 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected NOT 0.", (object) value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<long> actual, long expected, string message = "")
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected < {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckLess(this long value, long other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<long> actual, long expected, string message = "")
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected <= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckLessOrEqual(this long value, long other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<long> actual, long expected, string message = "")
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected > {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckGreater(this long value, long other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(this Assertion<long> actual, long expected, string message = "")
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected >= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<long> actual,
      long expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckGreaterOrEqual(this long value, long other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<long> actual, string message = "")
    {
      if (actual.Value > 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value > 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckPositive(this long value)
    {
      if (value > 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected positive.", (object) value));
      return 1;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<long> actual, string message = "")
    {
      if (actual.Value <= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value <= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckNotPositive(this long value)
    {
      if (value <= 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected NOT positive.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<long> actual, string message = "")
    {
      if (actual.Value < 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value < 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckNegative(this long value)
    {
      if (value < 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected negative.", (object) value));
      return -1;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<long> actual, string message = "")
    {
      if (actual.Value >= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<long> actual, string message, T0 arg0)
    {
      if (actual.Value >= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<long> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= 0L)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckNotNegative(this long value)
    {
      if (value >= 0L)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, expected NOT negative.", (object) value));
      return 0;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckWithinIncl(this long value, long minIncl, long maxIncl)
    {
      if (value < minIncl)
      {
        Log.Error(string.Format("CHECK FAIL: Value of long is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<long> actual,
      long minInclusive,
      long maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static long CheckWithinExcl(this long value, long minIncl, long maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of long is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<long> actual,
      long expected,
      long tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<long> actual,
      long expected,
      long tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<long> actual,
      long expected,
      long tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<long> actual,
      long expected,
      long tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of long is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
