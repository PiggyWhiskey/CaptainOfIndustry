// Decompiled with JetBrains decompiler
// Type: Mafi.QueueueAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class QueueueAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<Queueue<T>> queue, string message = "")
    {
      if (queue.Value.IsEmpty)
        return;
      Assert.FailAssertion(string.Format("Queueue<{0}> expected to be empty but it contains '{1}' values.", (object) typeof (T), (object) queue.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<Queueue<T>> queue, string message = "")
    {
      if (queue.Value.IsNotEmpty)
        return;
      Assert.FailAssertion(string.Format("Queueue<{0}> expected not empty.", (object) typeof (T)), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(
      this Assertion<Queueue<T>> queue,
      T expected,
      string message = "")
    {
      if (queue.Value.Count == 0)
      {
        Assert.FailAssertion(string.Format("Queueue<{0}> expected to contain value '{1}' but it is empty.", (object) typeof (T), (object) expected), message);
      }
      else
      {
        if (queue.Value.Contains(expected))
          return;
        Assert.FailAssertion(string.Format("Queueue<{0}> expected to contain value '{1}' but the value was not found (Count={2}).", (object) typeof (T), (object) expected, (object) queue.Value.Count), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void NotContains_DebugOnly<T>(
      this Assertion<Queueue<T>> queue,
      T expected,
      string message = "")
    {
      if (!queue.Value.Contains(expected))
        return;
      Assert.FailAssertion(string.Format("Queueue<{0}> expected to not contain value '{1}'.", (object) typeof (T), (object) expected), message);
    }
  }
}
