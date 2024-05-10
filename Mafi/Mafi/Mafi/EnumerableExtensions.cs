// Decompiled with JetBrains decompiler
// Type: Mafi.EnumerableExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi
{
  public static class EnumerableExtensions
  {
    [Obsolete("Causes hidden allocation.")]
    public static T SampleRandomOrDefault<T>(this IEnumerable<T> seq, IRandom random)
    {
      Lyst<T> lyst = seq.ToLyst<T>();
      return !lyst.IsEmpty ? lyst[random] : default (T);
    }

    public static T SampleRandomWeighted<T>(
      this IEnumerable<T> seq,
      IRandom random,
      int weightsSum,
      Func<T, int> weightSelector)
    {
      Assert.That<int>(weightsSum).IsPositive();
      int num1 = random.NextInt(weightsSum);
      int num2 = 0;
      foreach (T obj in seq)
      {
        num2 += weightSelector(obj);
        if (num2 > num1)
          return obj;
      }
      Log.Error("Failed to select weighted random. Supplied `weightsSum` is most likely wrong.");
      return seq.First<T>();
    }

    public static T SampleRandomWeighted<T>(
      this IEnumerable<KeyValuePair<int, T>> seq,
      IRandom random,
      int weightsSum)
    {
      Assert.That<int>(weightsSum).IsPositive();
      int num1 = random.NextInt(weightsSum);
      int num2 = 0;
      foreach (KeyValuePair<int, T> keyValuePair in seq)
      {
        num2 += keyValuePair.Key;
        if (num2 > num1)
          return keyValuePair.Value;
      }
      Log.Error("Failed to select weighted random. Supplied `weightsSum` is most likely wrong.");
      return seq.First<KeyValuePair<int, T>>().Value;
    }

    public static int SampleIndexRandomWeighted(
      this IEnumerable<int> seq,
      IRandom random,
      int weightsSum)
    {
      Assert.That<int>(weightsSum).IsPositive();
      int num1 = random.NextInt(weightsSum);
      int num2 = 0;
      int num3 = 0;
      foreach (int num4 in seq)
      {
        num2 += num4;
        if (num2 > num1)
          return num3;
        ++num3;
      }
      Log.Error("Failed to select weighted random. Supplied `weightsSum` is most likely wrong.");
      return 0;
    }

    public static T SampleRandomWeightedOrDefault<T>(
      this IEnumerable<T> seq,
      IRandom random,
      Func<T, int> weightSelector)
    {
      Lyst<T> lyst = seq.ToLyst<T>();
      if (lyst.IsEmpty)
        return default (T);
      int maxValueExcl = lyst.Sum<T>(weightSelector);
      Assert.That<int>(maxValueExcl).IsPositive();
      int num1 = random.NextInt(maxValueExcl);
      int num2 = 0;
      foreach (T obj in lyst)
      {
        num2 += weightSelector(obj);
        if (num2 > num1)
          return obj;
      }
      Log.Error("Failed to select weighted random. The `weightSelector` is most likely inconsistent.");
      return lyst[0];
    }

    public static T SampleRandomWeightedOrDefault<T>(
      this IEnumerable<KeyValuePair<int, T>> seq,
      IRandom random)
    {
      Lyst<KeyValuePair<int, T>> lyst = seq.ToLyst<KeyValuePair<int, T>>();
      if (lyst.IsEmpty)
        return default (T);
      int maxValueExcl = lyst.Sum<KeyValuePair<int, T>>((Func<KeyValuePair<int, T>, int>) (x => x.Key));
      Assert.That<int>(maxValueExcl).IsPositive();
      int num1 = random.NextInt(maxValueExcl);
      int num2 = 0;
      foreach (KeyValuePair<int, T> keyValuePair in lyst)
      {
        num2 += keyValuePair.Key;
        if (num2 > num1)
          return keyValuePair.Value;
      }
      Log.Error("Failed to select weighted random. The `weightSelector` is most likely inconsistent.");
      return lyst[0].Value;
    }

    public static IEnumerable<int> SelectIndicesWhere<T>(
      this IEnumerable<T> seq,
      Func<T, bool> predicate)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<int>) new EnumerableExtensions.\u003CSelectIndicesWhere\u003Ed__6<T>(-2)
      {
        \u003C\u003E3__seq = seq,
        \u003C\u003E3__predicate = predicate
      };
    }

    public static IEnumerable<TKey> SelectKeys<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> seq)
    {
      return seq.Select<KeyValuePair<TKey, TValue>, TKey>((Func<KeyValuePair<TKey, TValue>, TKey>) (kvp => kvp.Key));
    }

    public static IEnumerable<TValue> SelectValues<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> seq)
    {
      return seq.Select<KeyValuePair<TKey, TValue>, TValue>((Func<KeyValuePair<TKey, TValue>, TValue>) (kvp => kvp.Value));
    }

    public static TValue[] Repeat<TValue>(this TValue value, int count)
    {
      if (count <= 0)
      {
        Assert.That<int>(count).IsNotNegative();
        return Array.Empty<TValue>();
      }
      TValue[] objArray = new TValue[count];
      for (int index = 0; index < objArray.Length; ++index)
        objArray[index] = value;
      return objArray;
    }

    public static IEnumerable<TResult> TryGet<TResult, TValue>(
      this IEnumerable<TValue> seq,
      EnumerableExtensions.TryGetDelegate<TValue, TResult> tryGet)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TResult>) new EnumerableExtensions.\u003CTryGet\u003Ed__11<TResult, TValue>(-2)
      {
        \u003C\u003E3__seq = seq,
        \u003C\u003E3__tryGet = tryGet
      };
    }

    public delegate bool TryGetDelegate<TValue, TResult>(TValue value, out TResult result);
  }
}
