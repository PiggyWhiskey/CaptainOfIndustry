// Decompiled with JetBrains decompiler
// Type: Mafi.AngleDegrees1fAssertionAndChecksExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class AngleDegrees1fAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message = "")
    {
      if (actual.Value.Degrees == other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees == other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees == other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees == other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message = "")
    {
      if (actual.Value.Degrees == other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees == other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees == other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees == other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message = "")
    {
      if (actual.Value.Degrees != other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees != other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees != other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees != other.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message = "")
    {
      if (actual.Value.Degrees != other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees != other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees != other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees != other)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not equal to {1}.", (object) actual.Value.Degrees, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees == 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected 0.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<AngleDegrees1f> actual, string message, T0 arg0)
    {
      if (actual.Value.Degrees == 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees == 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees == 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckZero(this AngleDegrees1f value)
    {
      if (value.Degrees == 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected 0.", (object) value.Degrees));
      return AngleDegrees1f.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees != 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not 0.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees != 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees != 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees != 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not 0.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckNotZero(this AngleDegrees1f value, AngleDegrees1f validValue)
    {
      if (value.Degrees != 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected NOT 0.", (object) value.Degrees));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message = "")
    {
      if (actual.Value.Degrees < expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees < expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees < expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees < expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Degrees < expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees < expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees < expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees < expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected < {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckLess(this AngleDegrees1f value, AngleDegrees1f other)
    {
      if (value < other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message = "")
    {
      if (actual.Value.Degrees <= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees <= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees <= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees <= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Degrees <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees <= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected <= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckLessOrEqual(this AngleDegrees1f value, AngleDegrees1f other)
    {
      if (value <= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message = "")
    {
      if (actual.Value.Degrees > expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees > expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees > expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees > expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Degrees > expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees > expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees > expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees > expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected > {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckGreater(this AngleDegrees1f value, AngleDegrees1f other)
    {
      if (value > other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message = "")
    {
      if (actual.Value.Degrees >= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= expected.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message = "")
    {
      if (actual.Value.Degrees >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= expected)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected >= {1}.", (object) actual.Value.Degrees, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckGreaterOrEqual(
      this AngleDegrees1f value,
      AngleDegrees1f other)
    {
      if (value >= other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees > 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected positive.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees > 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees > 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees > 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckPositive(this AngleDegrees1f value)
    {
      if (value.Degrees > 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected positive.", (object) value.Degrees));
      return AngleDegrees1f.OneDegree;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not positive.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees <= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not positive.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckNotPositive(this AngleDegrees1f value)
    {
      if (value.Degrees <= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected NOT positive.", (object) value.Degrees));
      return AngleDegrees1f.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees < 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected negative.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees < 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees < 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees < 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckNegative(this AngleDegrees1f value)
    {
      if (value.Degrees < 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected negative.", (object) value.Degrees));
      return -AngleDegrees1f.OneDegree;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<AngleDegrees1f> actual, string message = "")
    {
      if (actual.Value.Degrees >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not negative.", (object) actual.Value.Degrees), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= 0)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected not negative.", (object) actual.Value.Degrees), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckNotNegative(this AngleDegrees1f value)
    {
      if (value.Degrees >= 0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, expected NOT negative.", (object) value.Degrees));
      return AngleDegrees1f.Zero;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message = "")
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees <= maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees <= maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees <= maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees <= maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees <= maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckWithinIncl(
      this AngleDegrees1f value,
      AngleDegrees1f minIncl,
      AngleDegrees1f maxIncl)
    {
      if (!(value >= minIncl))
      {
        Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if (value <= maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message = "")
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees < maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees < maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees < maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f minInclusive,
      AngleDegrees1f maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= minInclusive.Degrees && actual.Value.Degrees < maxInclusive.Degrees)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message = "")
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      Fix32 minInclusive,
      Fix32 maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.Degrees >= minInclusive && actual.Value.Degrees < maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value.Degrees, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static AngleDegrees1f CheckWithinExcl(
      this AngleDegrees1f value,
      AngleDegrees1f minIncl,
      AngleDegrees1f maxExcl)
    {
      if (value >= minIncl && value < maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of AngleDegrees1f is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message = "")
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      AngleDegrees1f tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      AngleDegrees1f tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      AngleDegrees1f tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<AngleDegrees1f> actual,
      AngleDegrees1f expected,
      AngleDegrees1f tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of AngleDegrees1f is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
