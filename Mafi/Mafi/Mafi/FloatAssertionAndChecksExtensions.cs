// Decompiled with JetBrains decompiler
// Type: Mafi.FloatAssertionAndChecksExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class FloatAssertionAndChecksExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo(this Assertion<float> actual, float other, string message = "")
    {
      if ((double) actual.Value == (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value == (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value == (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T0, T1, T2>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value == (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo(this Assertion<float> actual, float other, string message = "")
    {
      if ((double) actual.Value != (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value != (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value != (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T0, T1, T2>(
      this Assertion<float> actual,
      float other,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value != (double) other)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not equal to {1}.", (object) actual.Value, (object) other), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value == 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value == 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value == 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value == 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckZero(this float value)
    {
      if ((double) value == 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected 0.", (object) value));
      return 0.0f;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value != 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not 0.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value != 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value != 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value != 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not 0.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckNotZero(this float value, float validValue)
    {
      if ((double) value != 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected NOT 0.", (object) value));
      return validValue;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNan(this Assertion<float> actual, string message = "")
    {
      if (float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected NaN.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNan<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if (float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected NaN.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNan<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected NaN.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNan<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected NaN.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNan(this Assertion<float> actual, string message = "")
    {
      if (!float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not NaN.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNan<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if (!float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not NaN.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNan<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (!float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not NaN.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNan<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (!float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not NaN.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInfinity(this Assertion<float> actual, string message = "")
    {
      if (float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected infinity.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInfinity<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if (float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected infinity.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInfinity<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected infinity.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInfinity<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected infinity.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotInfinity(this Assertion<float> actual, string message = "")
    {
      if (!float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not infinity.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotInfinity<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if (!float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not infinity.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotInfinity<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (!float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not infinity.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotInfinity<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (!float.IsInfinity(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not infinity.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFinite(this Assertion<float> actual, string message = "")
    {
      if (!float.IsInfinity(actual.Value) && !float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected a finite number.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFinite<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if (!float.IsInfinity(actual.Value) && !float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected a finite number.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFinite<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (!float.IsInfinity(actual.Value) && !float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected a finite number.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFinite<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (!float.IsInfinity(actual.Value) && !float.IsNaN(actual.Value))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected a finite number.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess(this Assertion<float> actual, float expected, string message = "")
    {
      if ((double) actual.Value < (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected < {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value < (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value < (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLess<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value < (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected < {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckLess(this float value, float other)
    {
      if ((double) value < (double) other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected < {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual(this Assertion<float> actual, float expected, string message = "")
    {
      if ((double) actual.Value <= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected <= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value <= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value <= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsLessOrEqual<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value <= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected <= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckLessOrEqual(this float value, float other)
    {
      if ((double) value <= (double) other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected <= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater(this Assertion<float> actual, float expected, string message = "")
    {
      if ((double) actual.Value > (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected > {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value > (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value > (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreater<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value > (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected > {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckGreater(this float value, float other)
    {
      if ((double) value > (double) other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected > {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual(
      this Assertion<float> actual,
      float expected,
      string message = "")
    {
      if ((double) actual.Value >= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected >= {1}.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value >= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value >= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsGreaterOrEqual<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value >= (double) expected)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected >= {1}.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckGreaterOrEqual(this float value, float other)
    {
      if ((double) value >= (double) other)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected >= {1}.", (object) value, (object) other));
      return other;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value > 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value > 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value > 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsPositive<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value > 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckPositive(this float value)
    {
      if ((double) value > 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected positive.", (object) value));
      return 1f;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value <= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not positive.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value <= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value <= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPositive<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value <= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not positive.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckNotPositive(this float value)
    {
      if ((double) value <= 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected NOT positive.", (object) value));
      return 0.0f;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value < 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value < 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value < 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNegative<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value < 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckNegative(this float value)
    {
      if ((double) value < 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected negative.", (object) value));
      return -1f;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value >= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not negative.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value >= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value >= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNegative<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value >= 0.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected not negative.", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckNotNegative(this float value)
    {
      if ((double) value >= 0.0)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, expected NOT negative.", (object) value));
      return 0.0f;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message = "")
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value <= (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value <= (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value <= (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinIncl<T0, T1, T2>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value <= (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (inclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckWithinIncl(this float value, float minIncl, float maxIncl)
    {
      if ((double) value < (double) minIncl)
      {
        Log.Error(string.Format("CHECK FAIL: Value of float is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
        return minIncl;
      }
      if ((double) value <= (double) maxIncl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, within {1} (inclusive) and {2} (inclusive).", (object) value, (object) minIncl, (object) maxIncl));
      return maxIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message = "")
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value < (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value < (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value < (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithinExcl<T0, T1, T2>(
      this Assertion<float> actual,
      float minInclusive,
      float maxInclusive,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value >= (double) minInclusive && (double) actual.Value < (double) maxInclusive)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within {1} (inclusive) and {2} (exclusive).", (object) actual.Value, (object) minInclusive, (object) maxInclusive), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Pure]
    public static float CheckWithinExcl(this float value, float minIncl, float maxExcl)
    {
      if ((double) value >= (double) minIncl && (double) value < (double) maxExcl)
        return value;
      Log.Error(string.Format("CHECK FAIL: Value of float is {0}, within {1} (inclusive) and {2} (exclusive).", (object) value, (object) minIncl, (object) maxExcl));
      return minIncl;
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin01Incl(this Assertion<float> actual, string message = "")
    {
      if ((double) actual.Value >= 0.0 && (double) actual.Value <= 1.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within 0 and 1 (inclusive).", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin01Incl<T0>(this Assertion<float> actual, string message, T0 arg0)
    {
      if ((double) actual.Value >= 0.0 && (double) actual.Value <= 1.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within 0 and 1 (inclusive).", (object) actual.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin01Incl<T0, T1>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if ((double) actual.Value >= 0.0 && (double) actual.Value <= 1.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within 0 and 1 (inclusive).", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin01Incl<T0, T1, T2>(
      this Assertion<float> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if ((double) actual.Value >= 0.0 && (double) actual.Value <= 1.0)
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected within 0 and 1 (inclusive).", (object) actual.Value), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(this Assertion<float> actual, float expected, string message = "")
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with default tolerance.", (object) actual.Value, (object) expected), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<float> actual,
      float expected,
      float tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0>(
      this Assertion<float> actual,
      float expected,
      float tolerance,
      string message,
      T0 arg0)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1>(
      this Assertion<float> actual,
      float expected,
      float tolerance,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear<T0, T1, T2>(
      this Assertion<float> actual,
      float expected,
      float tolerance,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Value of float is {0}, expected near {1} with tolerance {2}.", (object) actual.Value, (object) expected, (object) tolerance), string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
