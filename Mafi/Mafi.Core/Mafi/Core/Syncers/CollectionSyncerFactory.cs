// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.CollectionSyncerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  public class CollectionSyncerFactory
  {
    public static CollectionSyncer<TItem, TCollection> NewForEnumerable<TItem, TCollection>(
      Func<TCollection> collectionProvider,
      ICollectionComparator<TItem, TCollection> comparator)
      where TCollection : IEnumerable<TItem>
    {
      return new CollectionSyncer<TItem, TCollection>(collectionProvider, comparator, (Action<TCollection, Lyst<TItem>>) ((enumerable, lyst) => lyst.AddRange((IEnumerable<TItem>) enumerable)));
    }

    public static CollectionSyncer<TItem, TCollection> NewForIndexable<TItem, TCollection>(
      Func<TCollection> collectionProvider,
      ICollectionComparator<TItem, TCollection> comparator)
      where TCollection : IIndexable<TItem>
    {
      return new CollectionSyncer<TItem, TCollection>(collectionProvider, comparator, (Action<TCollection, Lyst<TItem>>) ((enumerable, lyst) => lyst.AddRange((IIndexable<TItem>) enumerable)));
    }

    public static CollectionSyncer<TItem, ImmutableArray<TItem>> NewForImmutable<TItem>(
      Func<ImmutableArray<TItem>> collectionProvider,
      ICollectionComparator<TItem, ImmutableArray<TItem>> comparator)
    {
      return new CollectionSyncer<TItem, ImmutableArray<TItem>>(collectionProvider, comparator, (Action<ImmutableArray<TItem>, Lyst<TItem>>) ((enumerable, lyst) => lyst.AddRange(enumerable)));
    }

    public static CollectionSyncer<TItem, ReadOnlyArraySlice<TItem>> NewForReadonlySlice<TItem>(
      Func<ReadOnlyArraySlice<TItem>> collectionProvider,
      ICollectionComparator<TItem, ReadOnlyArraySlice<TItem>> comparator)
    {
      return new CollectionSyncer<TItem, ReadOnlyArraySlice<TItem>>(collectionProvider, comparator, (Action<ReadOnlyArraySlice<TItem>, Lyst<TItem>>) ((enumerable, lyst) => lyst.AddRange(enumerable)));
    }

    public CollectionSyncerFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
