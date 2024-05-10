// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.TriggerBuilder`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  public class TriggerBuilder<T>
  {
    private readonly UpdaterBuilder m_updaterBuilder;
    private readonly ISyncer<T> m_syncer;

    public TriggerBuilder(UpdaterBuilder updaterBuilder, ISyncer<T> syncer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_updaterBuilder = updaterBuilder.CheckNotNull<UpdaterBuilder>();
      this.m_syncer = syncer.CheckNotNull<ISyncer<T>>();
    }

    public UpdaterBuilder Do(Action<T> callback)
    {
      this.m_updaterBuilder.AddTrigger((ITrigger) new TriggerBuilder<T>.Trigger(this.m_syncer, callback));
      return this.m_updaterBuilder;
    }

    [MustUseReturnValue]
    public TriggerBuilder<T, T2> Observe<T2>(Func<T2> provider)
    {
      return this.m_updaterBuilder.For<T, T2>(this.m_syncer, this.m_updaterBuilder.CreateSyncer<T2>(provider));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T, T2> ObserveNoCompare<T2>(Func<T2> provider)
    {
      return this.m_updaterBuilder.For<T, T2>(this.m_syncer, this.m_updaterBuilder.CreateSyncerNoCompare<T2>(provider));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T, Lyst<T2>> Observe<T2>(
      Func<IIndexable<T2>> collectionProvider,
      ICollectionComparator<T2, IIndexable<T2>> comparator)
    {
      return this.m_updaterBuilder.For<T, Lyst<T2>>(this.m_syncer, this.m_updaterBuilder.CreateSyncer<T2>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T, Lyst<T2>> Observe<T2>(
      Func<IReadOnlyCollection<T2>> collectionProvider,
      ICollectionComparator<T2, IReadOnlyCollection<T2>> comparator)
    {
      return this.m_updaterBuilder.For<T, Lyst<T2>>(this.m_syncer, this.m_updaterBuilder.CreateSyncer<T2>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T, Lyst<T2>> Observe<T2>(
      Func<IEnumerable<T2>> collectionProvider,
      ICollectionComparator<T2, IEnumerable<T2>> comparator)
    {
      return this.m_updaterBuilder.For<T, Lyst<T2>>(this.m_syncer, this.m_updaterBuilder.CreateSyncer<T2>(collectionProvider, comparator));
    }

    private class Trigger : ITrigger
    {
      private readonly ISyncer<T> m_syncer;
      private readonly Action<T> m_callback;

      public Trigger(ISyncer<T> syncer, Action<T> callback)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_syncer = syncer.CheckNotNull<ISyncer<T>>();
        this.m_callback = callback.CheckNotNull<Action<T>>();
      }

      void ITrigger.RenderUpdate()
      {
        if (!this.m_syncer.HasChanged)
          return;
        this.m_callback(this.m_syncer.GetValueAndReset());
      }
    }
  }
}
