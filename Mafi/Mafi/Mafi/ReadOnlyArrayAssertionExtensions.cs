// Decompiled with JetBrains decompiler
// Type: Mafi.ReadOnlyArrayAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ReadonlyCollections;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ReadOnlyArrayAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<ReadOnlyArray<T>> actual, string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> is not initialized and internal array is null but expected empty.", (object) typeof (T)), message);
      }
      else
      {
        if (!actual.Value.IsNotEmpty)
          return;
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> is not empty but expected empty.", (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<ReadOnlyArray<T>> actual, string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), message);
      }
      else
      {
        if (!actual.Value.IsEmpty)
          return;
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> is empty but expected non-empty.", (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T>(
      this Assertion<ReadOnlyArray<T>> actual,
      int expectedLength,
      string message = "")
    {
      if (actual.Value.IsNotValid)
      {
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> is not initialized and internal array is null but expected non-empty.", (object) typeof (T)), message);
      }
      else
      {
        if (actual.Value.Length == expectedLength)
          return;
        Assert.FailAssertion(string.Format("ReadOnlyArray<{0}> has length {1} but length {2} was expected.", (object) typeof (T), (object) actual.Value.Length, (object) expectedLength), message);
      }
    }
  }
}
