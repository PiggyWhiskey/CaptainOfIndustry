// Decompiled with JetBrains decompiler
// Type: Mafi.FrequencyAssertionAndChecksExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class FrequencyAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Frequency> actual, Frequency other, string message = "")
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<Frequency> actual, int other, string message = "")
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<Frequency> actual,
      Frequency other,
      string message = "")
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != other.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<Frequency> actual, int other, string message = "")
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != other)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not equal to {1}.", (object) actual.Value.Ticks, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected 0.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks == 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckZero(this Frequency value)
    {
      if (value.Ticks == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected 0.", (object) value.Ticks));
      return Frequency.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not 0.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks != 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not 0.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckNotZero(this Frequency value, Frequency validValue)
    {
      if (value.Ticks != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected NOT 0.", (object) value.Ticks));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Frequency> actual, Frequency expected, string message = "")
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<Frequency> actual, int expected, string message = "")
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected < {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckLess(this Frequency value, Frequency other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message = "")
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<Frequency> actual,
      int expected,
      string message = "")
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected <= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckLessOrEqual(this Frequency value, Frequency other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message = "")
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<Frequency> actual, int expected, string message = "")
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected > {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckGreater(this Frequency value, Frequency other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message = "")
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= expected.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<Frequency> actual,
      int expected,
      string message = "")
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected >= {1}.", (object) actual.Value.Ticks, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckGreaterOrEqual(this Frequency value, Frequency other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected positive.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks > 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not positive.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not positive.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckNotPositive(this Frequency value)
    {
      if (value.Ticks <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected NOT positive.", (object) value.Ticks));
      return Frequency.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected negative.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks < 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not negative.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected not negative.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckNotNegative(this Frequency value)
    {
      if (value.Ticks >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, expected NOT negative.", (object) value.Ticks));
      return Frequency.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks <= maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckWithinIncl(
      this Frequency value,
      Frequency minIncl,
      Frequency maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency minInclusive,
      Frequency maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive.Ticks && actual.Value.Ticks < maxInclusive.Ticks)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message = "")
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int minInclusive,
      int maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks >= minInclusive && actual.Value.Ticks < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Ticks, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static Frequency CheckWithinExcl(
      this Frequency value,
      Frequency minIncl,
      Frequency maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of Frequency is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Frequency> actual,
      Frequency expected,
      Frequency tolerance,
      string message = "")
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Frequency> actual,
      Frequency expected,
      Frequency tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Frequency> actual,
      Frequency expected,
      Frequency tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Frequency> actual,
      Frequency expected,
      Frequency tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsNear(expected.Ticks, tolerance.Ticks))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Frequency> actual,
      int expected,
      int tolerance,
      string message = "")
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<Frequency> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<Frequency> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<Frequency> actual,
      int expected,
      int tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected near {1} with tolerance {2}.", (object) actual.Value.Ticks, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo(this Assertion<Frequency> actual, string message = "")
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected a power of 2.", (object) actual.Value.Ticks), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0>(this Assertion<Frequency> actual, string message, T0 arg0)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPowerOfTwo<T0, T1, T2>(
      this Assertion<Frequency> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Ticks.IsPowerOfTwo())
        return;
      Assert.FailAssertion(string.Format("Value of Frequency is {0}, expected a power of 2.", (object) actual.Value.Ticks), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
