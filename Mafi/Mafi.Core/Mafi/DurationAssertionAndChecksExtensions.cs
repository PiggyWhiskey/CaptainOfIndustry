// Decompiled with JetBrains decompiler
// Type: Mafi.DurationAssertionAndChecksExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class DurationAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Duration> actual, Duration other, string message = "")
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Duration> actual, int other, string message = "")
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<Duration> actual,
      Duration other,
      string message = "")
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<Duration> actual, int other, string message = "")
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Duration> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected 0.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckZero(this Duration value)
    {
      if (value.Ticks == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected 0.", (object) value.Ticks));
      return Duration.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not 0.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckNotZero(this Duration value, Duration validValue)
    {
      if (value.Ticks != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected NOT 0.", (object) value.Ticks));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Duration> actual, Duration expected, string message = "")
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Duration> actual, int expected, string message = "")
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckLess(this Duration value, Duration other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<Duration> actual,
      Duration expected,
      string message = "")
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<Duration> actual, int expected, string message = "")
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckLessOrEqual(this Duration value, Duration other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<Duration> actual,
      Duration expected,
      string message = "")
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<Duration> actual, int expected, string message = "")
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckGreater(this Duration value, Duration other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Duration> actual,
      Duration expected,
      string message = "")
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Duration> actual,
      int expected,
      string message = "")
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Duration> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckGreaterOrEqual(this Duration value, Duration other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected positive.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckPositive(this Duration value)
    {
      if (value.Ticks > 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected positive.", (object) value.Ticks));
      return Duration.OneTick;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not positive.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckNotPositive(this Duration value)
    {
      if (value.Ticks <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected NOT positive.", (object) value.Ticks));
      return Duration.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected negative.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckNegative(this Duration value)
    {
      if (value.Ticks < 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected negative.", (object) value.Ticks));
      return -Duration.OneTick;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not negative.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckNotNegative(this Duration value)
    {
      if (value.Ticks >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, expected NOT negative.", (object) value.Ticks));
      return Duration.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckWithinIncl(this Duration value, Duration minIncl, Duration maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration minInclusive,
      Duration maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Duration> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Duration CheckWithinExcl(this Duration value, Duration minIncl, Duration maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Duration is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Duration> actual,
      Duration expected,
      Duration tolerance,
      string message = "")
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Duration> actual,
      Duration expected,
      Duration tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Duration> actual,
      Duration expected,
      Duration tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Duration> actual,
      Duration expected,
      Duration tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Duration> actual,
      int expected,
      int tolerance,
      string message = "")
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Duration> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Duration> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Duration> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo(this Assertion<Duration> actual, string message = "")
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected a power of 2.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0>(this Assertion<Duration> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1, T2>(
      this Assertion<Duration> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Duration is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
