// Decompiled with JetBrains decompiler
// Type: Mafi.OptionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Extensions that add convenient methods for existing types.
  /// </summary>
  public static class OptionExtensions
  {
    public static Option<TValue> SomeOption<TValue>(this TValue obj) where TValue : class
    {
      return Option<TValue>.Some(obj);
    }

    public static Option<TValue> CreateOption<TValue>(this TValue obj) where TValue : class
    {
      return Option<TValue>.Create(obj);
    }

    /// <summary>Returns stored value under given key or None.</summary>
    public static Option<TValue> Get<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key) where TValue : class
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? Option<TValue>.None : (Option<TValue>) obj;
    }

    /// <summary>
    /// Returns stored struct under given key or default instance if it does not exist.
    /// </summary>
    public static TValue GetStruct<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key) where TValue : struct
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? default (TValue) : obj;
    }

    /// <summary>
    /// Returns sequence of values ignoring elements that are None.
    /// </summary>
    public static IEnumerable<T> SelectValues<T>(this IEnumerable<Option<T>> seq) where T : class
    {
      return seq.Where<Option<T>>((Func<Option<T>, bool>) (x => x.HasValue)).Select<Option<T>, T>((Func<Option<T>, T>) (x => x.Value));
    }

    /// <summary>
    /// Returns sequence of values ignoring elements that are None.
    /// </summary>
    public static IEnumerable<T> SelectValues<T, U>(
      this IEnumerable<U> seq,
      Func<U, Option<T>> selector)
      where T : class
    {
      return seq.Select<U, Option<T>>(selector).Where<Option<T>>((Func<Option<T>, bool>) (x => x.HasValue)).Select<Option<T>, T>((Func<Option<T>, T>) (x => x.Value));
    }

    /// <summary>
    /// Returns sequence of values ignoring elements that are None.
    /// </summary>
    public static IEnumerable<T> SelectValues<T>(this ImmutableArray<Option<T>> seq) where T : class
    {
      return seq.Where((Func<Option<T>, bool>) (x => x.HasValue)).Select<Option<T>, T>((Func<Option<T>, T>) (x => x.Value));
    }

    public static IEnumerable<T> WhereValues<T>(
      this IEnumerable<Option<T>> seq,
      Func<T, bool> predicate)
      where T : class
    {
      return seq.Where<Option<T>>((Func<Option<T>, bool>) (x => x.HasValue)).Select<Option<T>, T>((Func<Option<T>, T>) (x => x.Value)).Where<T>(predicate);
    }

    public static IEnumerable<T> WhereValues<T>(
      this ImmutableArray<Option<T>> seq,
      Func<T, bool> predicate)
      where T : class
    {
      return seq.Where((Func<Option<T>, bool>) (x => x.HasValue)).Select<Option<T>, T>((Func<Option<T>, T>) (x => x.Value)).Where<T>(predicate);
    }
  }
}
