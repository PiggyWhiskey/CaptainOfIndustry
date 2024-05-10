// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.CompareFixedOrder`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  /// <summary>
  /// Compares two collections value by value in fixed order. Be careful when using this! This is expensive. Optimally
  /// try to use <see cref="T:Mafi.Core.Syncers.CompareByCount`1" /> or at least set <see cref="F:Mafi.Core.Syncers.SyncFrequency.MoreThanSec" /> interval on
  /// your updater.
  /// </summary>
  public sealed class CompareFixedOrder<T> : 
    IEnumerableComparator<T>,
    ICollectionComparator<T, IEnumerable<T>>,
    IIndexableComparator<T>,
    ICollectionComparator<T, IIndexable<T>>,
    IImmutableComparator<T>,
    ICollectionComparator<T, ImmutableArray<T>>,
    IReadOnlyArraySliceComparator<T>,
    ICollectionComparator<T, ReadOnlyArraySlice<T>>
  {
    public static readonly CompareFixedOrder<T> Instance;
    private static IEqualityComparer<T> s_comparer;

    private CompareFixedOrder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (CompareFixedOrder<T>.s_comparer != null)
        return;
      CompareFixedOrder<T>.s_comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
    }

    public bool AreSame(IEnumerable<T> collection, Lyst<T> lastKnown)
    {
      int index = 0;
      foreach (T y in collection)
      {
        if (lastKnown.Count <= index || !CompareFixedOrder<T>.s_comparer.Equals(lastKnown[index], y))
          return false;
        ++index;
      }
      return lastKnown.Count == index;
    }

    public bool AreSame(IIndexable<T> collection, Lyst<T> lastKnown)
    {
      int count = collection.Count;
      if (count != lastKnown.Count)
        return false;
      for (int index = 0; index < count; ++index)
      {
        if (!CompareFixedOrder<T>.s_comparer.Equals(lastKnown[index], collection[index]))
          return false;
      }
      return true;
    }

    public bool AreSame(ImmutableArray<T> collection, Lyst<T> lastKnown)
    {
      int length = collection.Length;
      if (collection.Length != lastKnown.Count)
        return false;
      for (int index = 0; index < length; ++index)
      {
        if (!CompareFixedOrder<T>.s_comparer.Equals(lastKnown[index], collection[index]))
          return false;
      }
      return true;
    }

    public bool AreSame(ReadOnlyArraySlice<T> collection, Lyst<T> lastKnown)
    {
      int length = collection.Length;
      if (collection.Length != lastKnown.Count)
        return false;
      for (int index = 0; index < length; ++index)
      {
        if (!CompareFixedOrder<T>.s_comparer.Equals(lastKnown[index], collection[index]))
          return false;
      }
      return true;
    }

    static CompareFixedOrder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CompareFixedOrder<T>.Instance = new CompareFixedOrder<T>();
    }
  }
}
