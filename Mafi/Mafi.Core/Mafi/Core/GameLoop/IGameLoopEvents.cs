// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.IGameLoopEvents
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.GameLoop
{
  public interface IGameLoopEvents : INewGameCreatedEvents
  {
    /// <summary>
    /// State of game loop. What method is currently being invoked. This is useful for assertions and debugging.
    /// </summary>
    GameLoopState CurrentState { get; }

    /// <summary>Last state that was set.</summary>
    GameLoopState LastState { get; }

    /// <summary>Whether the game was wither created or loaded.</summary>
    bool GameWasLoaded { get; }

    int? SaveVersion { get; }

    bool IsTerminated { get; }

    /// <summary>
    /// Registers given callback for invoking once per game lifetime just after initial sim updates are done. This is
    /// not called for loaded games.
    /// </summary>
    void RegisterNewGameInitialized(object registrator, IEnumerator<string> iterativeCallback);

    /// <summary>
    /// Registers given callback for invoking right before the main game loop. This should be used by renderers to
    /// fetch their state. Renderers should NOT be registered to any sim events or load any data before this event.
    /// </summary>
    void RegisterRendererInitState(object registrator, IEnumerator<string> iterativeCallback);

    /// <summary>
    /// Registers a class that needs to perform sim update before the game starts. Registration must happen before
    /// any game loop events are invoked, ideally in the class' constructor.
    /// </summary>
    void RegisterInitSimUpdate(INeedsSimUpdatesForInit obj);

    /// <summary>
    /// Invokes the given action at the start of next sync.
    /// This is handy when a sim-thread event needs to continue its work in sync without constant polling.
    /// </summary>
    void InvokeInSyncNotSaved(Action action);

    /// <summary>
    /// Called when simulation loop has finished and is in sync with the main thread. This event represents start of
    /// sync update critical section and should be used for preparations for <see cref="P:Mafi.Core.GameLoop.IGameLoopEvents.SyncUpdate" />. All renderers
    /// should be updated on <see cref="P:Mafi.Core.GameLoop.IGameLoopEvents.SyncUpdate" /> so this method can serve for preparation.
    /// </summary>
    IEventNonSaveable<GameTime> SyncUpdateStart { get; }

    /// <summary>
    /// Called when simulation loop has finished and is in sync with the main thread. This event serves for signal
    /// (or data) exchange between simulation and visualization. This event is in critical section and should be used
    /// for very basic operations such as reference swaps.
    /// </summary>
    IEventNonSaveable<GameTime> SyncUpdate { get; }

    /// <summary>
    /// Called when simulation loop has finished and is in sync with the main thread. This event represents end of
    /// sync update critical section and should be used to process results of <see cref="P:Mafi.Core.GameLoop.IGameLoopEvents.SyncUpdate" />.
    /// </summary>
    IEventNonSaveable<GameTime> SyncUpdateEnd { get; }

    /// <summary>
    /// Called every frame (~60 times per second) as a first callback in the loop that should process input from the
    /// player. Use game time to make continuous inputs frame rate independent.
    /// </summary>
    IEventNonSaveable<GameTime> InputUpdate { get; }

    /// <summary>
    /// Called every frame just after <see cref="P:Mafi.Core.GameLoop.IGameLoopEvents.InputUpdate" />. This may be used to process collected information
    /// during <see cref="P:Mafi.Core.GameLoop.IGameLoopEvents.InputUpdate" /> and prepare it for rendering.
    /// </summary>
    IEventNonSaveable InputUpdateEnd { get; }

    /// <summary>
    /// Called every frame. This should be used for updating renderers. Use game time to make continuous inputs frame
    /// rate independent.
    /// </summary>
    IEventNonSaveable<GameTime> RenderUpdate { get; }

    IEventNonSaveable<GameTime> RenderUpdateEnd { get; }

    /// <summary>Called once at the end of the game.</summary>
    IEventNonSaveable Terminate { get; }

    IEventNonSaveable OnProjectChanged { get; }
  }
}
