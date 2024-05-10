// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.ImmutableArrayExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  public static class ImmutableArrayExtensions
  {
    /// <summary>
    /// Returns last element or <see cref="F:Mafi.Option`1.None" /> if the array is empty.
    /// </summary>
    /// <remarks>
    /// This needs to be an extension method because we need to make sure that the generic argument is a class.
    /// </remarks>
    public static Option<T> LastOrNone<T>(this ImmutableArray<T> array) where T : class
    {
      return !array.IsEmpty ? (Option<T>) array.Last : Option<T>.None;
    }

    public static bool TryGetValue<TKey, TValue>(
      this ImmutableArray<KeyValuePair<TKey, TValue>> list,
      TKey key,
      out TValue value)
    {
      EqualityComparer<TKey> equalityComparer1 = EqualityComparer<TKey>.Default;
      for (int index = 0; index < list.Length; ++index)
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
      this ImmutableArray<KeyValuePair<TKey, TValue>> list,
      TKey key,
      out TValue value,
      out int index)
    {
      EqualityComparer<TKey> equalityComparer1 = EqualityComparer<TKey>.Default;
      for (int index1 = 0; index1 < list.Length; ++index1)
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

    public static string JoinStrings(this ImmutableArray<string> lyst, string separator)
    {
      return string.Join(separator, lyst.GetInternalArray_StrictlyReadonly_NoSharing());
    }
  }
}
