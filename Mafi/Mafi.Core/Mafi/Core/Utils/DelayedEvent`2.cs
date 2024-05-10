// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.DelayedEvent`2
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
  public class DelayedEvent<T1, T2>
  {
    private readonly IGameLoopEvents m_gameEvents;
    private Lyst<T1> m_arg1OnSimThread;
    private Lyst<T2> m_arg2OnSimThread;

    public event Action<T1, T2> OnSync;

    public DelayedEvent(IGameLoopEvents gameEvents, Action<Action<T1, T2>> eventRegistrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_arg1OnSimThread = new Lyst<T1>();
      this.m_arg2OnSimThread = new Lyst<T2>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameEvents = gameEvents;
      eventRegistrator((Action<T1, T2>) ((arg1, arg2) =>
      {
        this.m_arg1OnSimThread.Add(arg1);
        this.m_arg2OnSimThread.Add(arg2);
      }));
      gameEvents.SyncUpdate.AddNonSaveable<DelayedEvent<T1, T2>>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void UnRegister()
    {
      this.m_gameEvents.SyncUpdate.RemoveNonSaveable<DelayedEvent<T1, T2>>(this, new Action<GameTime>(this.syncUpdate));
    }

    private void syncUpdate(GameTime time)
    {
      if (this.m_arg1OnSimThread.IsNotEmpty)
      {
        for (int index = 0; index < this.m_arg1OnSimThread.Count; ++index)
        {
          Action<T1, T2> onSync = this.OnSync;
          if (onSync != null)
            onSync(this.m_arg1OnSimThread[index], this.m_arg2OnSimThread[index]);
        }
      }
      this.m_arg1OnSimThread.Clear();
      this.m_arg2OnSimThread.Clear();
    }
  }
}
