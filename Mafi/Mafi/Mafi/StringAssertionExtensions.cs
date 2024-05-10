// Decompiled with JetBrains decompiler
// Type: Mafi.StringAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class StringAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNullOrEmpty(this Assertion<string> actual, string message = "")
    {
      if (!string.IsNullOrEmpty(actual.Value))
        return;
      Assert.FailAssertion("String is " + (actual.Value == null ? "null" : "empty") + ".", message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNullOrEmpty<T0>(this Assertion<string> actual, string message, T0 arg0)
    {
      if (!string.IsNullOrEmpty(actual.Value))
        return;
      Assert.FailAssertion("String is " + (actual.Value == null ? "null" : "empty") + ".", message.FormatInvariant((object) arg0));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly(
      this Assertion<string> actual,
      string substring,
      string message = "")
    {
      if (actual.Value.Contains(substring, StringComparison.Ordinal))
        return;
      Assert.FailAssertion("String did not contain '" + substring + "'.", message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly(
      this Assertion<string> actual,
      string substring,
      string message = "")
    {
      if (!actual.Value.Contains(substring, StringComparison.Ordinal))
        return;
      Assert.FailAssertion("String contain '" + substring + "'.", message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void IsSingleLine_DebugOnly(this Assertion<string> actual, string message = "")
    {
      int num = actual.Value.IndexOf('\n');
      if (num < 0)
        return;
      Assert.FailAssertion(string.Format("String contains new line character at index {0}.", (object) num), message);
    }
  }
}
