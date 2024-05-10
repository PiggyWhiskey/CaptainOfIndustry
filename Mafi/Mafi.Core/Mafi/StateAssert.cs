// Decompiled with JetBrains decompiler
// Type: Mafi.StateAssert
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class StateAssert
  {
    [ThreadStatic]
    private static bool s_isDisabled;
    [ThreadStatic]
    private static IGameLoopEvents s_gameLoopEvents;
    [ThreadStatic]
    private static ISimLoopEvents s_simLoopEvents;

    internal static void Enable(IGameLoopEvents gameLoopEvents, ISimLoopEvents simLoopEvents)
    {
      Assert.That<IGameLoopEvents>(StateAssert.s_gameLoopEvents).IsNull<IGameLoopEvents, string>("State assert was already setup on thead: {0}", ThreadUtils.ThreadNameFast);
      StateAssert.s_isDisabled = false;
      StateAssert.s_gameLoopEvents = gameLoopEvents.CheckNotNull<IGameLoopEvents>();
      StateAssert.s_simLoopEvents = simLoopEvents.CheckNotNull<ISimLoopEvents>();
    }

    internal static void Reset()
    {
      StateAssert.s_isDisabled = false;
      StateAssert.s_gameLoopEvents = (IGameLoopEvents) null;
      StateAssert.s_simLoopEvents = (ISimLoopEvents) null;
    }

    public static void ResetIfNotDisabled()
    {
      if (StateAssert.s_isDisabled)
        return;
      StateAssert.Reset();
    }

    internal static void Disable() => StateAssert.s_isDisabled = true;

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInGameState(GameLoopState state, string message = null)
    {
      if (StateAssert.s_isDisabled)
        return;
      if (StateAssert.s_gameLoopEvents == null)
        Assert.Fail("State asserts were not setup for this thread: " + ThreadUtils.ThreadNameFast + ".");
      else
        Assert.That<GameLoopState>(StateAssert.s_gameLoopEvents.CurrentState).IsEqualTo<GameLoopState>(state, message ?? "Invalid game state.");
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsInSimState(SimLoopState state, string message = null)
    {
      if (StateAssert.s_isDisabled)
        return;
      if (StateAssert.s_simLoopEvents == null)
      {
        Assert.Fail("State asserts were not setup for this thread: " + ThreadUtils.ThreadNameFast + ".");
      }
      else
      {
        if (StateAssert.s_gameLoopEvents.CurrentState <= GameLoopState.RendererInitState)
          return;
        Assert.That<SimLoopState>(StateAssert.s_simLoopEvents.CurrentState).IsEqualTo<SimLoopState>(state, message ?? "Invalid sim state.");
      }
    }
  }
}
