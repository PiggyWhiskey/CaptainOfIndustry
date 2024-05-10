// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesFAssertionAndChecksExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ThicknessTilesFAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message = "")
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message = "")
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message = "")
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message = "")
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected 0.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<ThicknessTilesF> actual, string message, T0 arg0)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckZero(this ThicknessTilesF value)
    {
      if (value.Value == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected 0.", (object) value.Value));
      return ThicknessTilesF.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not 0.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckNotZero(
      this ThicknessTilesF value,
      ThicknessTilesF validValue)
    {
      if (value.Value != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected NOT 0.", (object) value.Value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message = "")
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckLess(this ThicknessTilesF value, ThicknessTilesF other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message = "")
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckLessOrEqual(
      this ThicknessTilesF value,
      ThicknessTilesF other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message = "")
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckGreater(this ThicknessTilesF value, ThicknessTilesF other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message = "")
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckGreaterOrEqual(
      this ThicknessTilesF value,
      ThicknessTilesF other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected positive.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckPositive(this ThicknessTilesF value)
    {
      if (value.Value > 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected positive.", (object) value.Value));
      return new ThicknessTilesF(1);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not positive.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckNotPositive(this ThicknessTilesF value)
    {
      if (value.Value <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected NOT positive.", (object) value.Value));
      return ThicknessTilesF.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected negative.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckNegative(this ThicknessTilesF value)
    {
      if (value.Value < 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected negative.", (object) value.Value));
      return -new ThicknessTilesF(1);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<ThicknessTilesF> actual, string message = "")
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not negative.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckNotNegative(this ThicknessTilesF value)
    {
      if (value.Value >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, expected NOT negative.", (object) value.Value));
      return ThicknessTilesF.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckWithinIncl(
      this ThicknessTilesF value,
      ThicknessTilesF minIncl,
      ThicknessTilesF maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF minInclusive,
      ThicknessTilesF maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static ThicknessTilesF CheckWithinExcl(
      this ThicknessTilesF value,
      ThicknessTilesF minIncl,
      ThicknessTilesF maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of ThicknessTilesF is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with default tolerance.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      ThicknessTilesF tolerance,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      ThicknessTilesF tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      ThicknessTilesF tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      ThicknessTilesF expected,
      ThicknessTilesF tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<ThicknessTilesF> actual,
      Fix32 expected,
      Fix32 tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of ThicknessTilesF is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
