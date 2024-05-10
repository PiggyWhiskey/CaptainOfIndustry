// Decompiled with JetBrains decompiler
// Type: Mafi.CollectionsExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class CollectionsExtensions
  {
    /// <summary>Returns the last element of the list effectively.</summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static T Last<T>(this List<T> list)
    {
      Assert.That<int>(list.Count).IsPositive();
      return list[list.Count - 1];
    }

    /// <summary>
    /// Returns the last element of the list or default value effectively.
    /// </summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static T LastOrDefault<T>(this List<T> list)
    {
      return list.Count == 0 ? default (T) : list[list.Count - 1];
    }

    public static bool IsEmpty<T>(this List<T> list) => list.Count == 0;

    public static SmallImmutableArray<T> ToSmallImmutableArrayAndClear<T>(this List<T> list)
    {
      ImmutableArrayBuilder<T> immutableArrayBuilder = new ImmutableArrayBuilder<T>(list.Count);
      for (int index = 0; index < list.Count; ++index)
        immutableArrayBuilder[index] = list[index];
      return new SmallImmutableArray<T>(immutableArrayBuilder.GetImmutableArrayAndClear());
    }

    [Pure]
    public static T SampleRandomOrDefault<T>(
      this IIndexable<T> seq,
      IRandom random,
      Func<T, bool> predicate)
    {
      int maxValueExcl = 0;
      foreach (T obj in seq)
      {
        if (predicate(obj))
          ++maxValueExcl;
      }
      int num = random.NextInt(maxValueExcl);
      foreach (T obj in seq)
      {
        if (predicate(obj))
        {
          --num;
          if (num < 0)
            return obj;
        }
      }
      return default (T);
    }
  }
}
