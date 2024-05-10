// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.LystStructExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  public static class LystStructExtensions
  {
    public static bool ContainsKey<TKey, TValue>(
      this LystStruct<KeyValuePair<TKey, TValue>> list,
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

    public static TValue GetValueOrDefault<TKey, TValue>(
      this LystStruct<KeyValuePair<TKey, TValue>> list,
      TKey key)
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
          keyValuePair = list[index];
          return keyValuePair.Value;
        }
      }
      return default (TValue);
    }

    public static bool TryGetValue<TKey, TValue>(
      this LystStruct<KeyValuePair<TKey, TValue>> list,
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
      this LystStruct<KeyValuePair<TKey, TValue>> list,
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

    public static ref KeyValuePair<TKey, TValue> GetValueAsRef<TKey, TValue>(
      ref this LystStruct<KeyValuePair<TKey, TValue>> list,
      TKey key)
    {
      EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Count; ++index)
      {
        if (equalityComparer.Equals(list[index].Key, key))
          return ref list.GetRefAt(index);
      }
      list.Add(Make.Kvp<TKey, TValue>(key, default (TValue)));
      return ref list.GetRefAt(list.Count - 1);
    }

    public static void Add<TKey, TValue>(
      ref this LystStruct<KeyValuePair<TKey, TValue>> list,
      TKey key,
      TValue value)
    {
      list.Add(Make.Kvp<TKey, TValue>(key, value));
    }

    public static void AddOrSetValue<TKey, TValue>(
      ref this LystStruct<KeyValuePair<TKey, TValue>> list,
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

    public static bool Remove<TKey, TValue>(
      ref this LystStruct<KeyValuePair<TKey, TValue>> list,
      TKey key)
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

    public static string JoinStrings(this LystStruct<string> lyst, string separator)
    {
      return string.Join(separator, lyst.GetBackingArray(), 0, lyst.Count);
    }
  }
}
