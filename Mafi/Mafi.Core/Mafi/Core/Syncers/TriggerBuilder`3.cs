// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.TriggerBuilder`3
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  public class TriggerBuilder<T1, T2, T3>
  {
    private readonly UpdaterBuilder m_updaterBuilder;
    private readonly ISyncer<T1> m_syncer1;
    private readonly ISyncer<T2> m_syncer2;
    private readonly ISyncer<T3> m_syncer3;

    public TriggerBuilder(
      UpdaterBuilder updaterBuilder,
      ISyncer<T1> syncer1,
      ISyncer<T2> syncer2,
      ISyncer<T3> syncer3)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_updaterBuilder = updaterBuilder.CheckNotNull<UpdaterBuilder>();
      this.m_syncer1 = syncer1.CheckNotNull<ISyncer<T1>>();
      this.m_syncer2 = syncer2.CheckNotNull<ISyncer<T2>>();
      this.m_syncer3 = syncer3.CheckNotNull<ISyncer<T3>>();
    }

    public UpdaterBuilder Do(Action<T1, T2, T3> callback)
    {
      this.m_updaterBuilder.AddTrigger((ITrigger) new TriggerBuilder<T1, T2, T3>.Trigger(this.m_syncer1, this.m_syncer2, this.m_syncer3, callback));
      return this.m_updaterBuilder;
    }

    [MustUseReturnValue]
    public TriggerBuilder<T1, T2, T3, T4> Observe<T4>(Func<T4> provider)
    {
      return this.m_updaterBuilder.For<T1, T2, T3, T4>(this.m_syncer1, this.m_syncer2, this.m_syncer3, this.m_updaterBuilder.CreateSyncer<T4>(provider));
    }

    private class Trigger : ITrigger
    {
      private readonly ISyncer<T1> m_syncer1;
      private readonly ISyncer<T2> m_syncer2;
      private readonly ISyncer<T3> m_syncer3;
      private readonly Action<T1, T2, T3> m_callback;

      public Trigger(
        ISyncer<T1> syncer1,
        ISyncer<T2> syncer2,
        ISyncer<T3> syncer3,
        Action<T1, T2, T3> callback)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_syncer1 = syncer1.CheckNotNull<ISyncer<T1>>();
        this.m_syncer2 = syncer2.CheckNotNull<ISyncer<T2>>();
        this.m_syncer3 = syncer3.CheckNotNull<ISyncer<T3>>();
        this.m_callback = callback.CheckNotNull<Action<T1, T2, T3>>();
      }

      void ITrigger.RenderUpdate()
      {
        if (!this.m_syncer1.HasChanged && !this.m_syncer2.HasChanged && !this.m_syncer3.HasChanged)
          return;
        this.m_callback(this.m_syncer1.GetValueAndReset(), this.m_syncer2.GetValueAndReset(), this.m_syncer3.GetValueAndReset());
      }
    }
  }
}
