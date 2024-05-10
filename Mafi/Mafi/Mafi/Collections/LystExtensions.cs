// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.LystExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Extensions that use type constraints that cannot be used on <see cref="T:Mafi.Collections.Lyst`1" /> directly.
  /// </summary>
  public static class LystExtensions
  {
    /// <summary>
    /// Returns value at requested index or None if index is invalid.
    /// </summary>
    public static Option<T> AtOrNone<T>(this Lyst<T> list, int index) where T : class
    {
      return index >= 0 && index < list.Count ? (Option<T>) list[index] : Option<T>.None;
    }

    public static bool ContainsKey<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key)
    {
      EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        if (equalityComparer.Equals(list[index].Key, key))
          return true;
      }
      return false;
    }

    public static bool TryGetValue<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key,
      out TValue value)
    {
      EqualityComparer<TKey> equalityComparer1 = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        EqualityComparer<TKey> equalityComparer2 = equalityComparer1;
        KeyValuePair<TKey, TValue> keyValuePair = list[index];
        TKey key1 = keyValuePair.Key;
        TKey y = key;
        if (equalityComparer2.Equals(key1, y))
        {
          ref TValue local = ref value;
          keyValuePair = list[index];
          TValue obj = keyValuePair.Value;
          local = obj;
          return true;
        }
      }
      value = default (TValue);
      return false;
    }

    public static bool TryGetValue<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key,
      out TValue value,
      out int index)
    {
      EqualityComparer<TKey> equalityComparer1 = EqualityComparer<TKey>.Default;
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        EqualityComparer<TKey> equalityComparer2 = equalityComparer1;
        KeyValuePair<TKey, TValue> keyValuePair = list[index1];
        TKey key1 = keyValuePair.Key;
        TKey y = key;
        if (equalityComparer2.Equals(key1, y))
        {
          ref TValue local = ref value;
          keyValuePair = list[index1];
          TValue obj = keyValuePair.Value;
          local = obj;
          index = index1;
          return true;
        }
      }
      value = default (TValue);
      index = -1;
      return false;
    }

    public static void Add<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key,
      TValue value)
    {
      list.Add(Make.Kvp<TKey, TValue>(key, value));
    }

    public static void AddAndAssertNew<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key,
      TValue value)
    {
      EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        if (equalityComparer.Equals(list[index].Key, key))
        {
          Assert.Fail(string.Format("List already includes key '{0}'.", (object) key));
          break;
        }
      }
      list.Add(Make.Kvp<TKey, TValue>(key, value));
    }

    public static void AddOrSetValue<TKey, TValue>(
      this Lyst<KeyValuePair<TKey, TValue>> list,
      TKey key,
      TValue value)
    {
      EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        if (equalityComparer.Equals(list[index].Key, key))
        {
          list[index] = Make.Kvp<TKey, TValue>(key, value);
          return;
        }
      }
      list.Add(Make.Kvp<TKey, TValue>(key, value));
    }

    public static bool Remove<TKey, TValue>(this Lyst<KeyValuePair<TKey, TValue>> list, TKey key)
    {
      EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        if (equalityComparer.Equals(list[index].Key, key))
        {
          list.RemoveAt(index);
          return true;
        }
      }
      return false;
    }
  }
}
