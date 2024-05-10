// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.DelayedEventExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.GameLoop;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  public static class DelayedEventExtensions
  {
    public static DelayedEvent<T> RegisterDelayedRenderEvent<T>(
      this IGameLoopEvents gameLoop,
      Action<Action<T>> eventRegistrator,
      Action<T> onSyncEvent)
    {
      DelayedEvent<T> delayedEvent = new DelayedEvent<T>(gameLoop, eventRegistrator);
      delayedEvent.OnSync += onSyncEvent;
      return delayedEvent;
    }

    public static DelayedEvent<T1, T2> RegisterDelayedRenderEvent<T1, T2>(
      this IGameLoopEvents gameLoop,
      Action<Action<T1, T2>> eventRegistrator,
      Action<T1, T2> onSyncEvent)
    {
      DelayedEvent<T1, T2> delayedEvent = new DelayedEvent<T1, T2>(gameLoop, eventRegistrator);
      delayedEvent.OnSync += onSyncEvent;
      return delayedEvent;
    }
  }
}
