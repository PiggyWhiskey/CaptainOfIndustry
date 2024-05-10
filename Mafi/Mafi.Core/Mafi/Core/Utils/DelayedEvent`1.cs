// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.DelayedEvent`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.GameLoop;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  public class DelayedEvent<T>
  {
    private readonly IGameLoopEvents m_gameEvents;
    private Lyst<T> m_argOnSimThread;

    public event Action<T> OnSync;

    public DelayedEvent(IGameLoopEvents gameEvents, Action<Action<T>> eventRegistrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_argOnSimThread = new Lyst<T>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameEvents = gameEvents;
      eventRegistrator((Action<T>) (arg => this.m_argOnSimThread.Add(arg)));
      gameEvents.SyncUpdate.AddNonSaveable<DelayedEvent<T>>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void UnRegister()
    {
      this.m_gameEvents.SyncUpdate.RemoveNonSaveable<DelayedEvent<T>>(this, new Action<GameTime>(this.syncUpdate));
    }

    private void syncUpdate(GameTime time)
    {
      if (!this.m_argOnSimThread.IsNotEmpty)
        return;
      for (int index = 0; index < this.m_argOnSimThread.Count; ++index)
      {
        Action<T> onSync = this.OnSync;
        if (onSync != null)
          onSync(this.m_argOnSimThread[index]);
      }
      this.m_argOnSimThread.Clear();
    }
  }
}
