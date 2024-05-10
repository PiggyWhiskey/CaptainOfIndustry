// Decompiled with JetBrains decompiler
// Type: Mafi.LinqExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class LinqExtensions
  {
    public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
      this IEnumerable<TFirst> first,
      IEnumerable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TResult>) new LinqExtensions.\u003CZip\u003Ed__0<TFirst, TSecond, TResult>(-2)
      {
        \u003C\u003E3__first = first,
        \u003C\u003E3__second = second,
        \u003C\u003E3__resultSelector = resultSelector
      };
    }

    public static IEnumerable<Tuple<TFirst, TSecond>> Zip<TFirst, TSecond>(
      this IEnumerable<TFirst> first,
      IEnumerable<TSecond> second)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tuple<TFirst, TSecond>>) new LinqExtensions.\u003CZip\u003Ed__1<TFirst, TSecond>(-2)
      {
        \u003C\u003E3__first = first,
        \u003C\u003E3__second = second
      };
    }

    public static bool IsEnumerableEmpty<T>(this IEnumerable<T> first)
    {
      using (IEnumerator<T> enumerator = first.GetEnumerator())
        return !enumerator.MoveNext();
    }

    public static bool IsEnumerableNotEmpty<T>(this IEnumerable<T> first)
    {
      using (IEnumerator<T> enumerator = first.GetEnumerator())
        return enumerator.MoveNext();
    }

    /// <summary>
    /// Converts this enumerable to a new list. Consider using <see cref="M:Mafi.LinqExtensions.ToCleanLyst``1(System.Collections.Generic.IEnumerable{``0},Mafi.Collections.Lyst{``0})" /> if you already have
    /// instance that needs to be filled in.
    /// </summary>
    public static Lyst<T> ToLyst<T>(this IEnumerable<T> enumerable) => new Lyst<T>(enumerable);

    public static Lyst<T> ToLyst<T>(this IEnumerable<T> enumerable, int initialCapacity)
    {
      return new Lyst<T>(enumerable, initialCapacity);
    }

    /// <summary>
    /// Fills given list with this sequence. List is cleared before insertion. Returns the given list for ease of
    /// chaining.
    /// </summary>
    public static Lyst<T> ToCleanLyst<T>(this IEnumerable<T> enumerable, Lyst<T> list)
    {
      list.Clear();
      list.AddRange(enumerable);
      return list;
    }

    /// <summary>
    /// More effective than <c>.Where(predicate).ToLyst()</c>.
    /// </summary>
    public static Lyst<T> ToLyst<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
    {
      return enumerable.ToLyst<T>(predicate, new Lyst<T>());
    }

    /// <summary>
    /// More effective than <c>.Where(predicate).ToLyst()</c> and without result lyst allocation.
    /// </summary>
    public static Lyst<T> ToLyst<T>(
      this IEnumerable<T> enumerable,
      Predicate<T> predicate,
      Lyst<T> result)
    {
      foreach (T obj in enumerable)
      {
        if (predicate(obj))
          result.Add(obj);
      }
      return result;
    }

    public static Lyst<U> ToLystOf<T, U>(this IEnumerable<T> enumerable, Func<T, U> selector)
    {
      Lyst<U> lystOf = new Lyst<U>();
      foreach (T obj in enumerable)
        lystOf.Add(selector(obj));
      return lystOf;
    }

    /// <summary>
    /// Efficient version of Select that returns new array directly without any intermediate objects. Replaces
    /// <c>.Select(selector).ToArray()</c>.
    /// </summary>
    [Pure]
    public static TOut[] MapArray<TIn, TOut>(this TIn[] array, Func<TIn, TOut> mapper)
    {
      int length = array.Length;
      if (length == 0)
        return Array.Empty<TOut>();
      TOut[] outArray = new TOut[length];
      for (int index = 0; index < length; ++index)
        outArray[index] = mapper(array[index]);
      return outArray;
    }

    [Pure]
    public static TOut[] MapArray<TIn, TOut>(this TIn[] array, Func<TIn, int, TOut> mapper)
    {
      int length = array.Length;
      if (length == 0)
        return Array.Empty<TOut>();
      TOut[] outArray = new TOut[length];
      for (int index = 0; index < length; ++index)
        outArray[index] = mapper(array[index], index);
      return outArray;
    }

    public static float Median(this IEnumerable<float> source)
    {
      Lyst<float> lyst = source != null ? source.ToLyst<float>() : throw new ArgumentNullException(nameof (source));
      if (lyst.IsEmpty)
        throw new InvalidOperationException("Median called on sequence with no elements.");
      lyst.Sort();
      int index = lyst.Count / 2;
      return lyst.Count % 2 == 0 ? (float) (((double) lyst[index] + (double) lyst[index + 1]) / 2.0) : lyst[index];
    }

    public static T MinElement<T, TCmp>(this IEnumerable<T> source, Func<T, TCmp> selector) where TCmp : IComparable<TCmp>
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      TCmp y = default (TCmp);
      T obj1 = default (T);
      bool flag = false;
      Comparer<TCmp> comparer = Comparer<TCmp>.Default;
      foreach (T obj2 in source)
      {
        TCmp x = selector(obj2);
        if (flag)
        {
          if (comparer.Compare(x, y) < 1)
          {
            y = x;
            obj1 = obj2;
          }
        }
        else
        {
          y = x;
          obj1 = obj2;
          flag = true;
        }
      }
      if (flag)
        return obj1;
      throw new InvalidOperationException("MinElement: No elements in sequence.");
    }

    public static int MinIndex<T, TResult>(this IIndexable<T> source, Func<T, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (source.Count == 0)
        throw new InvalidOperationException("MinIndex: No elements in sequence.");
      TResult y = default (TResult);
      int num = -1;
      bool flag = false;
      Comparer<TResult> comparer = Comparer<TResult>.Default;
      int index = 0;
      for (int count = source.Count; index < count; ++index)
      {
        TResult x = selector(source[index]);
        if (flag)
        {
          if (comparer.Compare(x, y) < 0)
          {
            y = x;
            num = index;
          }
        }
        else
        {
          flag = true;
          y = x;
          num = index;
        }
      }
      if (!flag)
        throw new InvalidOperationException("MinIndex: No elements in sequence.");
      return num;
    }

    /// <summary>Effectively finds min and max in given sequence.</summary>
    /// <exception cref="T:System.InvalidOperationException">When given sequence is empty.</exception>
    public static MinMaxPair<T> MinMax<T>(this IEnumerable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      Comparer<T> comparer = Comparer<T>.Default;
      T obj1 = default (T);
      T obj2 = default (T);
      if ((object) obj1 == null)
      {
        foreach (T x in source)
        {
          if ((object) x != null)
          {
            if ((object) obj1 == null)
            {
              obj1 = x;
              obj2 = x;
            }
            else if (comparer.Compare(x, obj1) < 0)
              obj1 = x;
            else if (comparer.Compare(x, obj2) > 0)
              obj2 = x;
          }
        }
        if ((object) obj1 == null)
          Log.Error("MinMax: No elements in sequence.");
      }
      else
      {
        bool flag = false;
        foreach (T x in source)
        {
          if (flag)
          {
            if (comparer.Compare(x, obj1) < 0)
              obj1 = x;
            else if (comparer.Compare(x, obj2) > 0)
              obj2 = x;
          }
          else
          {
            obj1 = x;
            obj2 = x;
            flag = true;
          }
        }
        if (!flag)
          Log.Error("MinMax: No elements in sequence.");
      }
      return new MinMaxPair<T>(obj1, obj2);
    }

    public static MinMaxPair<T> MinMax<T, TElement>(
      this IEnumerable<TElement> source,
      Func<TElement, T> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      Comparer<T> comparer = Comparer<T>.Default;
      T obj1 = default (T);
      T obj2 = default (T);
      bool flag = false;
      foreach (TElement element in source)
      {
        T x = selector(element);
        if (flag)
        {
          if (comparer.Compare(x, obj1) < 0)
            obj1 = x;
          else if (comparer.Compare(x, obj2) > 0)
            obj2 = x;
        }
        else
        {
          obj1 = x;
          obj2 = x;
          flag = true;
        }
      }
      if (!flag)
        Log.Error("MinMax: No elements in sequence.");
      return new MinMaxPair<T>(obj1, obj2);
    }

    public static T MaxElement<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      int num1 = 0;
      T obj1 = default (T);
      bool flag = false;
      foreach (T obj2 in source)
      {
        int num2 = selector(obj2);
        if (flag)
        {
          if (num2 > num1)
          {
            num1 = num2;
            obj1 = obj2;
          }
        }
        else
        {
          num1 = num2;
          obj1 = obj2;
          flag = true;
        }
      }
      if (flag)
        return obj1;
      throw new InvalidOperationException("MaxElement: No elements in sequence.");
    }

    public static int MaxOrDefault<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
      int num1 = 0;
      bool flag = false;
      foreach (T obj in source)
      {
        int num2 = selector(obj);
        if (flag)
        {
          if (num2 > num1)
            num1 = num2;
        }
        else
        {
          num1 = num2;
          flag = true;
        }
      }
      return num1;
    }

    public static bool All(this IEnumerable<bool> source)
    {
      foreach (bool flag in source)
      {
        if (!flag)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Calls given action on all elements of the sequence and returns the sequence.
    /// </summary>
    public static IEnumerable<T> Call<T>(this IEnumerable<T> seq, Action<T> action)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<T>) new LinqExtensions.\u003CCall\u003Ed__20<T>(-2)
      {
        \u003C\u003E3__seq = seq,
        \u003C\u003E3__action = action
      };
    }

    /// <summary>
    /// Creates a dictionary straight from key-value pair sequence.
    /// </summary>
    public static Dict<TKey, TElement> ToDict<TKey, TElement>(
      this IEnumerable<KeyValuePair<TKey, TElement>> source)
    {
      Dict<TKey, TElement> dict = new Dict<TKey, TElement>();
      foreach (KeyValuePair<TKey, TElement> keyValuePair in source)
        dict.Add(keyValuePair.Key, keyValuePair.Value);
      return dict;
    }

    public static Dict<TKey, TValue> ToDict<TSeq, TKey, TValue>(
      this IEnumerable<TSeq> source,
      Func<TSeq, TKey> keySelector,
      Func<TSeq, TValue> valueSelector)
    {
      Dict<TKey, TValue> dict = new Dict<TKey, TValue>();
      foreach (TSeq seq in source)
        dict.Add(keySelector(seq), valueSelector(seq));
      return dict;
    }

    public static IEnumerable<T> Distinct<T, TKey>(
      this IEnumerable<T> source,
      Func<T, TKey> keySelector)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<T>) new LinqExtensions.\u003CDistinct\u003Ed__23<T, TKey>(-2)
      {
        \u003C\u003E3__source = source,
        \u003C\u003E3__keySelector = keySelector
      };
    }

    public static Set<T> ToSet<T>(this IEnumerable<T> source, bool assertDistinct = false)
    {
      if (!assertDistinct)
        return new Set<T>(source);
      Set<T> set = new Set<T>();
      foreach (T obj in source)
        set.AddAndAssertNew(obj);
      return set;
    }

    public static MeanAndStdDev AverageAndStdDev(this IEnumerable<float> values)
    {
      double num1 = 0.0;
      if (!(values is Lyst<float> lyst))
        lyst = values.ToLyst<float>();
      foreach (float num2 in lyst)
        num1 += (double) num2;
      double mean = num1 / (double) lyst.Count;
      double num3 = 0.0;
      foreach (double num4 in lyst)
      {
        double num5 = num4 - mean;
        num3 += num5 * num5;
      }
      return new MeanAndStdDev((float) mean, (float) Math.Sqrt(num3 / (double) lyst.Count));
    }

    public static MeanAndStdDev AverageAndStdDev(this IEnumerable<double> values)
    {
      double num1 = 0.0;
      if (!(values is Lyst<double> lyst))
        lyst = values.ToLyst<double>();
      foreach (double num2 in lyst)
        num1 += num2;
      double mean = num1 / (double) lyst.Count;
      double num3 = 0.0;
      foreach (double num4 in lyst)
      {
        double num5 = num4 - mean;
        num3 += num5 * num5;
      }
      return new MeanAndStdDev((float) mean, (float) Math.Sqrt(num3 / (double) lyst.Count));
    }

    public static DataStats ComputeStats(this IEnumerable<float> values)
    {
      Lyst<float> lyst = values.ToLyst<float>();
      if (lyst.IsEmpty)
        throw new InvalidOperationException("ComputeStats called on sequence with no elements.");
      lyst.Sort();
      int index = lyst.Count / 2;
      float median = lyst.Count % 2 == 0 ? (float) (((double) lyst[index] + (double) lyst[index + 1]) / 2.0) : lyst[index];
      double sum = 0.0;
      foreach (float num in lyst)
        sum += (double) num;
      double mean = sum / (double) lyst.Count;
      double num1 = 0.0;
      foreach (double num2 in lyst)
      {
        double num3 = num2 - mean;
        num1 += num3 * num3;
      }
      return new DataStats(lyst.First, lyst.Last, (float) sum, (float) mean, median, (float) lyst.Count, (float) Math.Sqrt(num1 / (double) lyst.Count));
    }
  }
}
