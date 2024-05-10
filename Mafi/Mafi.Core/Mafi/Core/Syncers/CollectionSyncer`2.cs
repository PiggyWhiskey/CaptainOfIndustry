// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.CollectionSyncer`2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  public class CollectionSyncer<TItem, TCollection> : ISyncer<Lyst<TItem>>, ISyncer
  {
    private readonly Action<TCollection, Lyst<TItem>> m_listFiller;
    private readonly Lyst<TItem> m_value;
    private readonly ICollectionComparator<TItem, TCollection> m_comparator;
    private readonly Func<TCollection> m_collectionProvider;

    public bool HasChanged { get; private set; }

    /// <summary>
    /// Use <see cref="T:Mafi.Core.Syncers.CollectionSyncerFactory" /> instead this ctor.
    /// </summary>
    public CollectionSyncer(
      Func<TCollection> collectionProvider,
      ICollectionComparator<TItem, TCollection> comparator,
      Action<TCollection, Lyst<TItem>> listFiller)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_value = new Lyst<TItem>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_listFiller = listFiller;
      this.m_collectionProvider = collectionProvider.CheckNotNull<Func<TCollection>>();
      this.m_comparator = comparator.CheckNotNull<ICollectionComparator<TItem, TCollection>>();
    }

    void ISyncer.SyncUpdate(bool invalidate)
    {
      TCollection collection = this.m_collectionProvider();
      bool flag = invalidate || !this.m_comparator.AreSame(collection, this.m_value);
      this.HasChanged |= flag;
      if (!flag)
        return;
      this.m_value.Clear();
      this.m_listFiller(collection, this.m_value);
    }

    public Lyst<TItem> GetValueAndReset()
    {
      this.HasChanged = false;
      return this.m_value;
    }
  }
}
