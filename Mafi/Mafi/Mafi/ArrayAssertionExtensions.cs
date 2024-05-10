// Decompiled with JetBrains decompiler
// Type: Mafi.ArrayAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ArrayAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<T[]> actual, string message = "")
    {
      if (actual.Value == null)
      {
        Assert.FailAssertion(string.Format("Array of {0} is null but it was expected to be non-empty.", (object) typeof (T)), message);
      }
      else
      {
        if (actual.Value.Length != 0)
          return;
        Assert.FailAssertion(string.Format("Array of {0} is empty but it was expected to be non-empty.", (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T, T0>(this Assertion<T[]> actual, string message, T0 arg0)
    {
      if (actual.Value == null)
      {
        Assert.FailAssertion(string.Format("Array of {0} is null but it was expected to be non-empty.", (object) typeof (T)), message.FormatInvariant((object) arg0));
      }
      else
      {
        if (actual.Value.Length != 0)
          return;
        Assert.FailAssertion(string.Format("Array of {0} is empty but it was expected to be non-empty.", (object) typeof (T)), message.FormatInvariant((object) arg0));
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasLength<T>(this Assertion<T[]> actual, int expectedLength, string message = "")
    {
      if (actual.Value == null)
      {
        Assert.FailAssertion(string.Format("Array of {0} is null but array of length {1} was expected.", (object) typeof (T), (object) expectedLength), message);
      }
      else
      {
        if (actual.Value.Length == expectedLength)
          return;
        Assert.FailAssertion(string.Format("Array of {0} has length {1} but array of length {2} was expected.", (object) typeof (T), (object) actual.Value.Length, (object) expectedLength), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndexFor<T>(this Assertion<int> actual, T[] array, string message = "")
    {
      if ((uint) actual.Value < (uint) array.Length)
        return;
      Assert.FailAssertion(string.Format("Index {0} is not valid for {1}[] of length {2}.", (object) actual.Value, (object) typeof (T), (object) array.Length), message);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void All_DebugOnly<T>(
      this Assertion<T[]> actual,
      Func<T, bool> predicate,
      string message = "")
    {
      T[] objArray = actual.Value;
      for (int index = 0; index < objArray.Length; ++index)
      {
        if (!predicate(objArray[index]))
          Assert.FailAssertion(string.Format("Predicate failed on index {0} (value={1}) of {2} array.", (object) index, (object) objArray[index], (object) typeof (T)), message);
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    public static void Contains_DebugOnly<T>(this Assertion<T[]> actual, T value, string message = "")
    {
      if (actual.Value == null)
      {
        Assert.FailAssertion(string.Format("Array of {0} is null but expected to contain '{1}'.", (object) typeof (T), (object) value), message);
      }
      else
      {
        foreach (T obj in actual.Value)
        {
          if (obj.Equals((object) value))
            return;
        }
        Assert.FailAssertion(string.Format("Array of {0} (length {1}) does not contain '{2}'.", (object) typeof (T), (object) actual.Value, (object) value), message);
      }
    }
  }
}
