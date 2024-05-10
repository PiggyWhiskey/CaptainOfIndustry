// Decompiled with JetBrains decompiler
// Type: Mafi.PercentAssertionAndChecksExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class PercentAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Percent> actual, Percent other, string message = "")
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<Percent> actual, Percent other, string message = "")
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckZero(this Percent value)
    {
      if (value.IsZero)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected 0.", (object) value));
      return Percent.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckNotZero(this Percent value, Percent validValue)
    {
      if (value.IsNotZero)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected NOT 0.", (object) value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Percent> actual, Percent expected, string message = "")
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected < {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckLess(this Percent value, Percent other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<Percent> actual,
      Percent expected,
      string message = "")
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected <= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckLessOrEqual(this Percent value, Percent other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<Percent> actual, Percent expected, string message = "")
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected > {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckGreater(this Percent value, Percent other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Percent> actual,
      Percent expected,
      string message = "")
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected >= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckGreaterOrEqual(this Percent value, Percent other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckPositive(this Percent value)
    {
      if (value.IsPositive)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected positive.", (object) value));
      return Percent.Hundred;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckNotPositive(this Percent value)
    {
      if (value.IsNotPositive)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected NOT positive.", (object) value));
      return Percent.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckNegative(this Percent value)
    {
      if (value.IsNegative)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected negative.", (object) value));
      return -Percent.Hundred;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<Percent> actual, string message = "")
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<Percent> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<Percent> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckNotNegative(this Percent value)
    {
      if (value.IsNotNegative)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, expected NOT negative.", (object) value));
      return Percent.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckWithinIncl(this Percent value, Percent minIncl, Percent maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent minInclusive,
      Percent maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Percent CheckWithinExcl(this Percent value, Percent minIncl, Percent maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Percent is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Percent> actual,
      Percent expected,
      Percent tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Percent> actual,
      Percent expected,
      Percent tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Percent> actual,
      Percent expected,
      Percent tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Percent> actual,
      Percent expected,
      Percent tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
