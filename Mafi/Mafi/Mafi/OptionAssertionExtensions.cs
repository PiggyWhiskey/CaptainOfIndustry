// Decompiled with JetBrains decompiler
// Type: Mafi.OptionAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class OptionAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void HasValue<T>(this Assertion<Option<T>> actual, string message = "") where T : class
    {
      if (actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> is None but expected to have a value.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasValue<T, T0>(this Assertion<Option<T>> actual, string message, T0 arg0) where T : class
    {
      if (actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> is None but expected to have a value.", (object) typeof (T)), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void HasValue_DebugOnly<T>(this Assertion<Option<T>> actual, string message = "") where T : class
    {
      if (actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> is None but expected to have a value.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNone<T>(this Assertion<Option<T>> actual, string message = "") where T : class
    {
      if (actual.Value.IsNone)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> has value '{1}' but expected to be None.", (object) typeof (T), (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNone<T, T0>(this Assertion<Option<T>> actual, string message, T0 arg0) where T : class
    {
      if (actual.Value.IsNone)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> has value '{1}' but expected to be None.", (object) typeof (T), (object) actual.Value.Value), string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNone<T, T0, T1>(
      this Assertion<Option<T>> actual,
      string message,
      T0 arg0,
      T1 arg1)
      where T : class
    {
      if (actual.Value.IsNone)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> has value '{1}' but expected to be None.", (object) typeof (T), (object) actual.Value.Value), string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsNone_DebugOnly<T>(this Assertion<Option<T>> actual, string message = "") where T : class
    {
      if (actual.Value.IsNone)
        return;
      Assert.FailAssertion(string.Format("Option<{0}> has value '{1}' but expected to be None.", (object) typeof (T), (object) actual.Value.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEqualTo<T>(this Assertion<Option<T>> actual, T expected, string message = "") where T : class
    {
      if (actual.Value == expected)
        return;
      Assert.FailAssertion(actual.Value.IsNone ? string.Format("Option<{0}> is None but value '{1}' was expected.", (object) typeof (T), (object) expected) : string.Format("Option<{0}> has value '{1}' but value '{2}' was expected.", (object) typeof (T), (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEqualTo<T>(
      this Assertion<Option<T>> actual,
      T expected,
      string message = "")
      where T : class
    {
      if (actual.Value != expected)
        return;
      Assert.FailAssertion(actual.Value.IsNone ? string.Format("Option<{0}> is None but value != '{1}' was expected.", (object) typeof (T), (object) expected) : string.Format("Option<{0}> has value '{1}' but value != '{2}' was expected.", (object) typeof (T), (object) actual.Value.Value, (object) expected), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasValue<T>(this Assertion<T?> actual, string message = "") where T : struct
    {
      if (actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("{0}? is null.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNone<T>(this Assertion<T?> actual, string message = "") where T : struct
    {
      if (!actual.Value.HasValue)
        return;
      Assert.FailAssertion(string.Format("{0}? has expected to be null but has value '{1}'.", (object) typeof (T), (object) actual.Value.Value), message);
    }
  }
}
