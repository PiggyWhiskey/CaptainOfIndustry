// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.UpdaterBuilder
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
  public class UpdaterBuilder
  {
    private readonly Lyst<ISyncer> m_syncers;
    private readonly Lyst<ITrigger> m_triggers;
    private bool m_wasBuilt;

    public static UpdaterBuilder Start() => new UpdaterBuilder();

    private UpdaterBuilder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_syncers = new Lyst<ISyncer>();
      this.m_triggers = new Lyst<ITrigger>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public ISyncer<T> CreateSyncer<T>(Func<T> provider)
    {
      Syncer<T> syncer = new Syncer<T>(provider);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<T>) syncer;
    }

    public ISyncer<T> CreateSyncerNoCompare<T>(Func<T> provider)
    {
      NonComparingSyncer<T> syncerNoCompare = new NonComparingSyncer<T>(provider);
      this.m_syncers.Add((ISyncer) syncerNoCompare);
      return (ISyncer<T>) syncerNoCompare;
    }

    public ISyncer<Lyst<T>> CreateSyncer<T>(
      Func<IEnumerable<T>> collectionProvider,
      ICollectionComparator<T, IEnumerable<T>> comparator)
    {
      CollectionSyncer<T, IEnumerable<T>> syncer = CollectionSyncerFactory.NewForEnumerable<T, IEnumerable<T>>(collectionProvider, comparator);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<Lyst<T>>) syncer;
    }

    public ISyncer<Lyst<T>> CreateSyncer<T>(
      Func<IReadOnlyCollection<T>> collectionProvider,
      ICollectionComparator<T, IReadOnlyCollection<T>> comparator)
    {
      CollectionSyncer<T, IReadOnlyCollection<T>> syncer = CollectionSyncerFactory.NewForEnumerable<T, IReadOnlyCollection<T>>(collectionProvider, comparator);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<Lyst<T>>) syncer;
    }

    public ISyncer<Lyst<T>> CreateSyncer<T>(
      Func<IIndexable<T>> collectionProvider,
      ICollectionComparator<T, IIndexable<T>> comparator)
    {
      CollectionSyncer<T, IIndexable<T>> syncer = CollectionSyncerFactory.NewForIndexable<T, IIndexable<T>>(collectionProvider, comparator);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<Lyst<T>>) syncer;
    }

    public ISyncer<Lyst<T>> CreateSyncer<T>(
      Func<ImmutableArray<T>> collectionProvider,
      ICollectionComparator<T, ImmutableArray<T>> comparator)
    {
      CollectionSyncer<T, ImmutableArray<T>> syncer = CollectionSyncerFactory.NewForImmutable<T>(collectionProvider, comparator);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<Lyst<T>>) syncer;
    }

    public ISyncer<Lyst<T>> CreateSyncer<T>(
      Func<ReadOnlyArraySlice<T>> collectionProvider,
      ICollectionComparator<T, ReadOnlyArraySlice<T>> comparator)
    {
      CollectionSyncer<T, ReadOnlyArraySlice<T>> syncer = CollectionSyncerFactory.NewForReadonlySlice<T>(collectionProvider, comparator);
      this.m_syncers.Add((ISyncer) syncer);
      return (ISyncer<Lyst<T>>) syncer;
    }

    [MustUseReturnValue]
    public TriggerBuilder<T> Observe<T>(Func<T> provider)
    {
      return this.For<T>(this.CreateSyncer<T>(provider));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T> ObserveNoCompare<T>(Func<T> provider)
    {
      return this.For<T>(this.CreateSyncerNoCompare<T>(provider));
    }

    [MustUseReturnValue]
    public TriggerBuilder<Lyst<T>> Observe<T>(
      Func<IEnumerable<T>> collectionProvider,
      ICollectionComparator<T, IEnumerable<T>> comparator)
    {
      return this.For<Lyst<T>>(this.CreateSyncer<T>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<Lyst<T>> Observe<T>(
      Func<IReadOnlyCollection<T>> collectionProvider,
      ICollectionComparator<T, IReadOnlyCollection<T>> comparator)
    {
      return this.For<Lyst<T>>(this.CreateSyncer<T>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<Lyst<T>> Observe<T>(
      Func<IIndexable<T>> collectionProvider,
      ICollectionComparator<T, IIndexable<T>> comparator)
    {
      return this.For<Lyst<T>>(this.CreateSyncer<T>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<Lyst<T>> Observe<T>(
      Func<ImmutableArray<T>> collectionProvider,
      ICollectionComparator<T, ImmutableArray<T>> comparator)
    {
      return this.For<Lyst<T>>(this.CreateSyncer<T>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<Lyst<T>> Observe<T>(
      Func<ReadOnlyArraySlice<T>> collectionProvider,
      ICollectionComparator<T, ReadOnlyArraySlice<T>> comparator)
    {
      return this.For<Lyst<T>>(this.CreateSyncer<T>(collectionProvider, comparator));
    }

    [MustUseReturnValue]
    public TriggerBuilder<T> For<T>(ISyncer<T> syncer) => new TriggerBuilder<T>(this, syncer);

    [MustUseReturnValue]
    public TriggerBuilder<T1, T2> For<T1, T2>(ISyncer<T1> syncer1, ISyncer<T2> syncer2)
    {
      return new TriggerBuilder<T1, T2>(this, syncer1, syncer2);
    }

    [MustUseReturnValue]
    public TriggerBuilder<T1, T2, T3> For<T1, T2, T3>(
      ISyncer<T1> syncer1,
      ISyncer<T2> syncer2,
      ISyncer<T3> syncer3)
    {
      return new TriggerBuilder<T1, T2, T3>(this, syncer1, syncer2, syncer3);
    }

    [MustUseReturnValue]
    public TriggerBuilder<T1, T2, T3, T4> For<T1, T2, T3, T4>(
      ISyncer<T1> syncer1,
      ISyncer<T2> syncer2,
      ISyncer<T3> syncer3,
      ISyncer<T4> syncer4)
    {
      return new TriggerBuilder<T1, T2, T3, T4>(this, syncer1, syncer2, syncer3, syncer4);
    }

    [MustUseReturnValue]
    public TriggerBuilder<T1, T2, T3, T4, T5> For<T1, T2, T3, T4, T5>(
      ISyncer<T1> syncer1,
      ISyncer<T2> syncer2,
      ISyncer<T3> syncer3,
      ISyncer<T4> syncer4,
      ISyncer<T5> syncer5)
    {
      return new TriggerBuilder<T1, T2, T3, T4, T5>(this, syncer1, syncer2, syncer3, syncer4, syncer5);
    }

    public void DoOnSyncPeriodically(Action action, Duration? intervalMaybe = null)
    {
      Duration interval = intervalMaybe ?? Duration.OneTick;
      this.m_syncers.Add((ISyncer) new PeriodicSyncUpdateSyncer(action, interval));
    }

    internal void AddTrigger(ITrigger trigger)
    {
      this.m_triggers.Add(trigger.CheckNotNull<ITrigger>());
    }

    public IUiUpdater Build(SyncFrequency syncFrequency = SyncFrequency.Critical)
    {
      Assert.That<bool>(this.m_wasBuilt).IsFalse("UI updater was already built.");
      UpdaterBuilder.UiUpdater uiUpdater = new UpdaterBuilder.UiUpdater(syncFrequency, this.m_syncers.ToImmutableArrayAndClear(), this.m_triggers.ToImmutableArrayAndClear());
      this.m_wasBuilt = true;
      return (IUiUpdater) uiUpdater;
    }

    private class UiUpdater : IUiUpdater
    {
      private Option<Action> m_oneTimeCallback;
      private bool m_syncForOneTimeCallbackDone;
      private int m_syncCounter;
      private bool m_firstRenderAfterSync;
      private bool m_invalidate;
      private readonly int m_syncDelay;
      private readonly ImmutableArray<ISyncer> m_syncers;
      private readonly ImmutableArray<ITrigger> m_triggers;
      private readonly Lyst<IUiUpdater> m_childUpdaters;

      private static int getDelayFor(SyncFrequency frequency)
      {
        switch (frequency)
        {
          case SyncFrequency.Critical:
            return 0;
          case SyncFrequency.OncePerSec:
            return 7;
          case SyncFrequency.MoreThanSec:
            return 13;
          default:
            throw new ArgumentOutOfRangeException(nameof (frequency), (object) frequency, (string) null);
        }
      }

      internal UiUpdater(
        SyncFrequency syncFrequency,
        ImmutableArray<ISyncer> syncers,
        ImmutableArray<ITrigger> triggers)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_invalidate = true;
        this.m_childUpdaters = new Lyst<IUiUpdater>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_syncers = syncers.CheckNotDefaultStruct<ImmutableArray<ISyncer>>();
        this.m_triggers = triggers.CheckNotDefaultStruct<ImmutableArray<ITrigger>>();
        this.m_syncDelay = UpdaterBuilder.UiUpdater.getDelayFor(syncFrequency);
      }

      public void AddChildUpdater(IUiUpdater updater) => this.m_childUpdaters.Add(updater);

      public void RemoveChildUpdater(IUiUpdater updater)
      {
        this.m_childUpdaters.TryRemoveReplaceLast(updater);
      }

      public void ClearAllChildUpdaters() => this.m_childUpdaters.Clear();

      public void SyncUpdate()
      {
        if (this.m_syncCounter < this.m_syncDelay && !this.m_invalidate)
        {
          ++this.m_syncCounter;
        }
        else
        {
          this.m_syncCounter = 0;
          foreach (IUiUpdater childUpdater in this.m_childUpdaters)
            childUpdater.SyncUpdate();
          foreach (ISyncer syncer in this.m_syncers)
            syncer.SyncUpdate(this.m_invalidate);
          this.m_invalidate = false;
          this.m_firstRenderAfterSync = true;
          if (!this.m_oneTimeCallback.HasValue)
            return;
          this.m_syncForOneTimeCallbackDone = true;
        }
      }

      public void RenderUpdate()
      {
        if (!this.m_firstRenderAfterSync)
          return;
        this.m_firstRenderAfterSync = false;
        foreach (IUiUpdater childUpdater in this.m_childUpdaters)
          childUpdater.RenderUpdate();
        foreach (ITrigger trigger in this.m_triggers)
          trigger.RenderUpdate();
        if (!this.m_oneTimeCallback.HasValue || !this.m_syncForOneTimeCallbackDone)
          return;
        this.m_oneTimeCallback.Value();
        this.m_oneTimeCallback = Option<Action>.None;
      }

      public void SetOneTimeAfterSyncCallback(Action action)
      {
        this.m_syncForOneTimeCallbackDone = false;
        this.m_oneTimeCallback = (Option<Action>) action;
      }

      public void Invalidate()
      {
        this.m_invalidate = true;
        foreach (IUiUpdater childUpdater in this.m_childUpdaters)
          childUpdater.Invalidate();
      }
    }
  }
}
