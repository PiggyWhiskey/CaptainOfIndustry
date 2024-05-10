// Decompiled with JetBrains decompiler
// Type: Mafi.Fix32AssertionAndChecksExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class Fix32AssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Fix32> actual, Fix32 other, string message = "")
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<Fix32> actual, Fix32 other, string message = "")
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckZero(this Fix32 value)
    {
      if (value.IsZero)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected 0.", (object) value));
      return Fix32.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckNotZero(this Fix32 value, Fix32 validValue)
    {
      if (value.IsNotZero)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected NOT 0.", (object) value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Fix32> actual, Fix32 expected, string message = "")
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected < {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckLess(this Fix32 value, Fix32 other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<Fix32> actual, Fix32 expected, string message = "")
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected <= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckLessOrEqual(this Fix32 value, Fix32 other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<Fix32> actual, Fix32 expected, string message = "")
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected > {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckGreater(this Fix32 value, Fix32 other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected >= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckGreaterOrEqual(this Fix32 value, Fix32 other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckPositive(this Fix32 value)
    {
      if (value.IsPositive)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected positive.", (object) value));
      return Fix32.One;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotPositive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckNotPositive(this Fix32 value)
    {
      if (value.IsNotPositive)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected NOT positive.", (object) value));
      return Fix32.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckNegative(this Fix32 value)
    {
      if (value.IsNegative)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected negative.", (object) value));
      return -Fix32.One;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<Fix32> actual, string message = "")
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<Fix32> actual, string message, T0 arg0)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<Fix32> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNotNegative)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckNotNegative(this Fix32 value)
    {
      if (value.IsNotNegative)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, expected NOT negative.", (object) value));
      return Fix32.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckWithinIncl(this Fix32 value, Fix32 minIncl, Fix32 maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value >= minInclusive && actual.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Fix32 CheckWithinExcl(this Fix32 value, Fix32 minIncl, Fix32 maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Fix32 is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(this Assertion<Fix32> actual, Fix32 expected, string message = "")
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Fix32> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Fix32> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Fix32 is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
