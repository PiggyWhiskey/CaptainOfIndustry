// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.DictExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Collections
{
  public static class DictExtensions
  {
    public static TValue GetValueOrCreateDefault<TKey, TValue>(
      this Dict<TKey, TValue> dict,
      TKey key)
      where TValue : new()
    {
      TValue valueOrCreateDefault;
      if (!dict.TryGetValue(key, out valueOrCreateDefault))
      {
        valueOrCreateDefault = new TValue();
        dict.Add(key, valueOrCreateDefault);
      }
      return valueOrCreateDefault;
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key)
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? default (TValue) : obj;
    }

    public static TValue GetValueOr<TKey, TValue>(
      this Dict<TKey, TValue> dict,
      TKey key,
      TValue defaultValue)
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? defaultValue : obj;
    }

    public static TValue? GetValueOrNull<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key) where TValue : struct
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? new TValue?() : new TValue?(obj);
    }

    /// <summary>
    /// Increments value at given key or adds value 1. Returns the new value.
    /// Together with <see cref="M:Mafi.Collections.DictExtensions.DecAndRemoveAtZero``1(Mafi.Collections.Dict{``0,System.Int32},``0)" /> can be used for counting using dictionaries.
    /// </summary>
    public static int IncOrInsert1<TKey>(this Dict<TKey, int> dict, TKey key)
    {
      ref int local = ref dict.GetRefValue(key, out bool _);
      ++local;
      return local;
    }

    public static int AddOrInsert<TKey>(this Dict<TKey, int> dict, TKey key, int value)
    {
      ref int local = ref dict.GetRefValue(key, out bool _);
      local += value;
      return local;
    }

    public static long AddOrInsert<TKey>(this Dict<TKey, long> dict, TKey key, long value)
    {
      ref long local = ref dict.GetRefValue(key, out bool _);
      local += value;
      return local;
    }

    /// <summary>
    /// Decrements value and removes key when 0 is reached. Asserts that key exist. Returns the new value.
    /// Together with <see cref="M:Mafi.Collections.DictExtensions.IncOrInsert1``1(Mafi.Collections.Dict{``0,System.Int32},``0)" /> can be used for counting using dictionaries.
    /// </summary>
    public static int DecAndRemoveAtZero<TKey>(this Dict<TKey, int> dict, TKey key)
    {
      int num1;
      if (!dict.TryGetValue(key, out num1))
      {
        Assert.Fail("Key not present. Decrement going negative?");
        return -1;
      }
      int num2 = num1 - 1;
      if (num2 <= 0)
        dict.Remove(key);
      else
        dict[key] = num2;
      return num2;
    }

    public static int RemoveValues<K, V>(this Dict<K, V> dict, Predicate<V> predicate)
    {
      Lyst<K> lyst = dict.Where<KeyValuePair<K, V>>((Func<KeyValuePair<K, V>, bool>) (kvp => predicate(kvp.Value))).Select<KeyValuePair<K, V>, K>((Func<KeyValuePair<K, V>, K>) (kvp => kvp.Key)).ToLyst<K>();
      foreach (K key in lyst)
        dict.Remove(key);
      return lyst.Count;
    }

    public static V GetOrCreate<K, V>(this Dict<K, V> dict, K key, Func<V> creator)
    {
      V v;
      return dict.TryGetValue(key, out v) ? v : creator();
    }

    /// <summary>
    /// DANGER: Make sure you use static lambdas (those use only the key as a dynamic argument) otherwise you will cause
    /// allocations of lambdas for each call of this method no matter what.
    /// </summary>
    public static V GetOrAdd<K, V>(this Dict<K, V> dict, K key, Func<K, V> creator)
    {
      V orAdd1;
      if (dict.TryGetValue(key, out orAdd1))
        return orAdd1;
      V orAdd2 = creator(key);
      dict.Add(key, orAdd2);
      return orAdd2;
    }
  }
}
