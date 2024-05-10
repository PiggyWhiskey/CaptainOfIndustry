// Decompiled with JetBrains decompiler
// Type: Mafi.BoolAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class BoolAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsTrue(this Assertion<bool> actual, string message = "")
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly(this Assertion<bool> actual, string message = "")
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T0>(this Assertion<bool> actual, string message, T0 arg0)
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsTrue_DebugOnly<T0, T1>(
      this Assertion<bool> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", message.FormatInvariant((object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsTrue<T0>(this Assertion<bool> actual, string message, T0 arg0)
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsTrue<T0, T1>(
      this Assertion<bool> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsTrue<T0, T1, T2>(
      this Assertion<bool> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'false' but value 'true' was expected.", string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFalse(this Assertion<bool> actual, string message = "")
    {
      if (!actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'true' but value 'false' was expected.", message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsFalse_DebugOnly(this Assertion<bool> actual, string message = "")
    {
      if (!actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'true' but value 'false' was expected.", message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFalse<T0>(this Assertion<bool> actual, string message, T0 arg0)
    {
      if (!actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'true' but value 'false' was expected.", string.Format(message, (object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFalse<T0, T1>(
      this Assertion<bool> actual,
      string message,
      T0 arg0,
      T1 arg1)
    {
      if (!actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'true' but value 'false' was expected.", string.Format(message, (object) arg0, (object) arg1));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFalse<T0, T1, T2>(
      this Assertion<bool> actual,
      string message,
      T0 arg0,
      T1 arg1,
      T2 arg2)
    {
      if (!actual.Value)
        return;
      Assert.FailAssertion("Bool value is 'true' but value 'false' was expected.", string.Format(message, (object) arg0, (object) arg1, (object) arg2));
    }
  }
}
