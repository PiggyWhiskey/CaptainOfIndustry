// Decompiled with JetBrains decompiler
// Type: Mafi.QuantityAssertionAndChecksExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class QuantityAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Quantity> actual, Quantity other, string message = "")
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Quantity> actual, int other, string message = "")
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<Quantity> actual,
      Quantity other,
      string message = "")
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != other.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<Quantity> actual, int other, string message = "")
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != other)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not equal to {1}.", (object) actual.Value.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected 0.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckZero(this Quantity value)
    {
      if (value.Value == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected 0.", (object) value.Value));
      return Quantity.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not 0.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not 0.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckNotZero(this Quantity value, Quantity validValue)
    {
      if (value.Value != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected NOT 0.", (object) value.Value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Quantity> actual, Quantity expected, string message = "")
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Quantity> actual, int expected, string message = "")
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected < {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckLess(this Quantity value, Quantity other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message = "")
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<Quantity> actual, int expected, string message = "")
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected <= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckLessOrEqual(this Quantity value, Quantity other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message = "")
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<Quantity> actual, int expected, string message = "")
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected > {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckGreater(this Quantity value, Quantity other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message = "")
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= expected.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Quantity> actual,
      int expected,
      string message = "")
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected >= {1}.", (object) actual.Value.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckGreaterOrEqual(this Quantity value, Quantity other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected positive.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckPositive(this Quantity value)
    {
      if (value.Value > 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected positive.", (object) value.Value));
      return Quantity.One;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not positive.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not positive.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckNotPositive(this Quantity value)
    {
      if (value.Value <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected NOT positive.", (object) value.Value));
      return Quantity.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected negative.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckNegative(this Quantity value)
    {
      if (value.Value < 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected negative.", (object) value.Value));
      return -Quantity.One;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not negative.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected not negative.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckNotNegative(this Quantity value)
    {
      if (value.Value >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, expected NOT negative.", (object) value.Value));
      return Quantity.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value <= maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckWithinIncl(this Quantity value, Quantity minIncl, Quantity maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity minInclusive,
      Quantity maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive.Value && actual.Value.Value < maxInclusive.Value)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value >= minInclusive && actual.Value.Value < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Quantity CheckWithinExcl(this Quantity value, Quantity minIncl, Quantity maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Quantity is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Quantity> actual,
      Quantity expected,
      Quantity tolerance,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Quantity> actual,
      Quantity expected,
      Quantity tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Quantity> actual,
      Quantity expected,
      Quantity tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Quantity> actual,
      Quantity expected,
      Quantity tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected.Value, tolerance.Value))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Quantity> actual,
      int expected,
      int tolerance,
      string message = "")
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Quantity> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Quantity> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Quantity> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo(this Assertion<Quantity> actual, string message = "")
    {
      if (actual.Value.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected a power of 2.", (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0>(this Assertion<Quantity> actual, string message, T0 arg0)
    {
      if (actual.Value.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected a power of 2.", (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected a power of 2.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1, T2>(
      this Assertion<Quantity> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Value.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Quantity is {0}, expected a power of 2.", (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
