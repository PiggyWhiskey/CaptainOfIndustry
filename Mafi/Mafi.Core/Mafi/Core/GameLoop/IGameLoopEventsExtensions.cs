// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.IGameLoopEventsExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.GameLoop
{
  public static class IGameLoopEventsExtensions
  {
    public static void RegisterNewGameCreated(
      this INewGameCreatedEvents gle,
      object registrator,
      Action fastCallback)
    {
      gle.RegisterNewGameCreated(registrator, IGameLoopEventsExtensions.makeEnumerator(fastCallback));
    }

    public static void RegisterNewGameInitialized(
      this IGameLoopEvents gle,
      object registrator,
      Action fastCallback)
    {
      gle.RegisterNewGameInitialized(registrator, IGameLoopEventsExtensions.makeEnumerator(fastCallback));
    }

    public static void RegisterRendererInitState(
      this IGameLoopEvents gle,
      object registrator,
      Action fastCallback)
    {
      gle.RegisterRendererInitState(registrator, IGameLoopEventsExtensions.makeEnumerator(fastCallback));
    }

    private static IEnumerator<string> makeEnumerator(Action action)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new IGameLoopEventsExtensions.\u003CmakeEnumerator\u003Ed__3(0)
      {
        action = action
      };
    }
  }
}
