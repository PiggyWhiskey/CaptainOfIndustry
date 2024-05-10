// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.PeriodicSyncUpdateSyncer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Utils;
using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  /// <summary>
  /// Syncer that doesn't store any value. Just invokes user specified action periodically on sync.
  /// </summary>
  public class PeriodicSyncUpdateSyncer : ISyncer
  {
    private readonly Duration m_interval;
    private readonly Action m_callback;
    private readonly TickTimer m_timer;

    public PeriodicSyncUpdateSyncer(Action callback, Duration interval)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_callback = callback.CheckNotNull<Action>();
      this.m_interval = interval;
      this.m_timer = new TickTimer();
    }

    public void SyncUpdate(bool invalidate)
    {
      if (this.m_timer.Decrement() && !invalidate)
        return;
      this.m_callback();
      this.m_timer.Start(this.m_interval);
    }
  }
}
