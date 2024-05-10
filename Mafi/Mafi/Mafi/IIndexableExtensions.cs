// Decompiled with JetBrains decompiler
// Type: Mafi.IIndexableExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi
{
  public static class IIndexableExtensions
  {
    /// <summary>
    /// Returns whether this indexable is empty. I know, very surprising.
    /// </summary>
    public static bool IsEmpty<T>(this IIndexable<T> indexable) => indexable.Count == 0;

    /// <summary>Returns whether this indexable is not empty.</summary>
    public static bool IsNotEmpty<T>(this IIndexable<T> indexable) => indexable.Count > 0;

    /// <summary>
    /// Returns the first element at index 0 of this indexable. It is callers responsibility to check whether this
    /// indexable is not empty.
    /// </summary>
    public static T First<T>(this IIndexable<T> indexable) => indexable[0];

    public static T FirstOrDefault<T>(this IIndexable<T> indexable)
    {
      return indexable.Count <= 0 ? default (T) : indexable[0];
    }

    public static T FirstOrDefault<T>(this IIndexable<T> indexable, Predicate<T> predicate)
    {
      int index = 0;
      for (int count = indexable.Count; index < count; ++index)
      {
        if (predicate(indexable[index]))
          return indexable[index];
      }
      return default (T);
    }

    public static int? FirstIndexOf<T>(this IIndexable<T> indexable, Predicate<T> predicate)
    {
      int count = indexable.Count;
      for (int index = 0; index < count; ++index)
      {
        if (predicate(indexable[index]))
          return new int?(index);
      }
      return new int?();
    }

    /// <summary>
    /// Returns the last element at index <c>Count - 1</c> of this indexable. It is callers responsibility to check
    /// whether this indexable is not empty.
    /// </summary>
    public static T Last<T>(this IIndexable<T> indexable) => indexable[indexable.Count - 1];

    public static T LastOrDefault<T>(this IIndexable<T> indexable)
    {
      return indexable.Count != 0 ? indexable[indexable.Count - 1] : default (T);
    }

    public static Option<T> LastMaybe<T>(this IIndexable<T> indexable) where T : class
    {
      return indexable.Count != 0 ? (Option<T>) indexable[indexable.Count - 1] : Option<T>.None;
    }

    public static IEnumerable<TResult> Select<T, TResult>(
      this IIndexable<T> indexable,
      Func<T, TResult> selector)
    {
      return indexable.AsEnumerable().Select<T, TResult>(selector);
    }

    public static IEnumerable<TResult> Select<T, TResult>(
      this IIndexable<T> indexable,
      Func<T, int, TResult> selector)
    {
      return indexable.AsEnumerable().Select<T, TResult>(selector);
    }

    public static int IndexOf<T>(this IIndexable<T> indexable, T item)
    {
      int count = indexable.Count;
      if ((object) item == null)
      {
        for (int index = 0; index < count; ++index)
        {
          if ((object) indexable[index] == null)
            return index;
        }
        return -1;
      }
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < count; ++index)
      {
        if (equalityComparer.Equals(indexable[index], item))
          return index;
      }
      return -1;
    }

    /// <summary>
    /// Finds an index of given item, using a selector to select value from list first.
    /// Perf: We do not take a predicate to avoid capturing lambdas.
    /// </summary>
    public static int IndexOf<T, U>(
      this IIndexable<T> indexable,
      U item,
      Func<T, U> selector,
      IEqualityComparer<U> comparer = null)
    {
      int count = indexable.Count;
      if ((object) item == null)
      {
        for (int index = 0; index < count; ++index)
        {
          if ((object) selector(indexable[index]) == null)
            return index;
        }
        return -1;
      }
      IEqualityComparer<U> equalityComparer = comparer ?? (IEqualityComparer<U>) EqualityComparer<U>.Default;
      for (int index = 0; index < count; ++index)
      {
        if (equalityComparer.Equals(selector(indexable[index]), item))
          return index;
      }
      return -1;
    }

    public static bool Contains<T>(this IIndexable<T> indexable, T item)
    {
      int count = indexable.Count;
      if ((object) item == null)
      {
        for (int index = 0; index < count; ++index)
        {
          if ((object) indexable[index] == null)
            return true;
        }
        return false;
      }
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < count; ++index)
      {
        if (equalityComparer.Equals(indexable[index], item))
          return true;
      }
      return false;
    }

    public static bool Contains<T>(this IIndexable<T> indexable, Predicate<T> predicate)
    {
      int count = indexable.Count;
      for (int index = 0; index < count; ++index)
      {
        if (predicate(indexable[index]))
          return true;
      }
      return false;
    }

    public static bool Any<T>(this IIndexable<T> indexable, Predicate<T> predicate)
    {
      int index = 0;
      for (int count = indexable.Count; index < count; ++index)
      {
        if (predicate(indexable[index]))
          return true;
      }
      return false;
    }

    public static int Sum<T>(this IIndexable<T> indexable, Func<T, int> selector)
    {
      int num = 0;
      int index = 0;
      for (int count = indexable.Count; index < count; ++index)
        num += selector(indexable[index]);
      return num;
    }

    public static float Sum<T>(this IIndexable<T> indexable, Func<T, float> selector)
    {
      float num = 0.0f;
      int index = 0;
      for (int count = indexable.Count; index < count; ++index)
        num += selector(indexable[index]);
      return num;
    }

    /// <summary>Creates new immutable array from this indexable.</summary>
    public static ImmutableArray<T> ToImmutableArray<T>(this IIndexable<T> indexable)
    {
      ImmutableArrayBuilder<T> immutableArrayBuilder = new ImmutableArrayBuilder<T>(indexable.Count);
      int num = 0;
      for (int count = indexable.Count; num < count; ++num)
        immutableArrayBuilder[num] = indexable[num];
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public static ImmutableArray<U> ToImmutableArray<T, U>(
      this IIndexable<T> indexable,
      Func<T, U> selector)
    {
      ImmutableArrayBuilder<U> immutableArrayBuilder = new ImmutableArrayBuilder<U>(indexable.Count);
      int num = 0;
      for (int count = indexable.Count; num < count; ++num)
        immutableArrayBuilder[num] = selector(indexable[num]);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public static T GetWeightedRandomElement<T>(
      this IIndexable<T> indexable,
      IRandom random,
      Func<T, int> weightSelector)
    {
      int maxValueExcl = 0;
      foreach (T obj in indexable)
        maxValueExcl += weightSelector(obj);
      int num1 = random.NextInt(maxValueExcl);
      int num2 = 0;
      foreach (T weightedRandomElement in indexable)
      {
        num2 += weightSelector(weightedRandomElement);
        if (num1 < num2)
          return weightedRandomElement;
      }
      throw new InvalidOperationException("Rand weight must be less than total weight by definition.");
    }

    public static IEnumerable<T> Where<T>(this IIndexable<T> indexable, Func<T, bool> predicate)
    {
      return indexable.AsEnumerable().Where<T>(predicate);
    }

    public static IEnumerable<Tuple<TFirst, TSecond>> Zip<TFirst, TSecond>(
      this IIndexable<TFirst> first,
      IIndexable<TSecond> second)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tuple<TFirst, TSecond>>) new IIndexableExtensions.\u003CZip\u003Ed__22<TFirst, TSecond>(-2)
      {
        \u003C\u003E3__first = first,
        \u003C\u003E3__second = second
      };
    }
  }
}
