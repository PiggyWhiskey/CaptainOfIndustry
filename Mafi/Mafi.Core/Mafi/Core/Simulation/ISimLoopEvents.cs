// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.ISimLoopEvents
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Simulation
{
  /// <summary>
  /// Simulation events for anybody to subscribe. All events are called on the simulation thread.
  /// 
  /// The order of events called is following:
  /// 0) Input commands are processed
  /// 1) UpdateStart
  /// 2) Update
  /// 3) UpdateEnd
  /// </summary>
  /// <remarks>
  /// The sim loop events are invoked as follows:
  /// <code>
  /// |	(ProcessCommands)
  /// |	UpdateAfterCmdProc()
  /// |
  /// |	if (not paused) {
  /// |		UpdateAfterSync()
  /// |
  /// |		for (number of sim steps based on game speed) {
  /// |			if (current step &gt; 0) {
  /// |				(ProcessCommands)
  /// |				UpdateAfterCmdProc()
  /// |			}
  /// |			UpdateStart()
  /// |			Update()
  /// |			UpdateEnd()
  /// |		}
  /// |	}
  /// |
  /// |	UpdateEndForUi()
  /// </code>
  /// </remarks>
  public interface ISimLoopEvents
  {
    /// <summary>Current sim step.</summary>
    SimStep CurrentStep { get; }

    /// <summary>
    /// State of sim loop. What method is currently being invoked. This is useful for assertions and debugging.
    /// </summary>
    SimLoopState CurrentState { get; }

    /// <summary>
    /// Whether the simulation is paused. In such case only commands are executed.
    /// </summary>
    bool IsSimPaused { get; }

    /// <summary>Game speed multiplier.</summary>
    int SimSpeedMult { get; }

    /// <summary>
    /// Whether the simulation loop is active (commands execution included).
    /// </summary>
    bool IsInSimLoop { get; }

    /// <summary>
    /// Called after commands were processed. Note that this will be also called during pause.
    /// </summary>
    IEvent UpdateAfterCmdProc { get; }

    /// <summary>
    /// Called once after sync. Unlike other events, this one is called only after sync. This makes difference when
    /// there is more sim steps performed between syncs.
    /// </summary>
    IEvent UpdateAfterSync { get; }

    /// <summary>
    /// Called at the very beginning of the simulation loop right after commands are processed. Invoked on the
    /// simulation thread.
    /// </summary>
    IEvent UpdateStart { get; }

    /// <summary>
    /// Called after <see cref="P:Mafi.Core.Simulation.ISimLoopEvents.UpdateStart" /> and should be used for core computations. Invoked on the simulation
    /// thread.
    /// </summary>
    IEvent Update { get; }

    /// <summary>
    /// Called after <see cref="P:Mafi.Core.Simulation.ISimLoopEvents.Update" /> as the very last event before sync state. This can be used for exchange of
    /// computed data between simulation components or for preparation of the synchronization operation. Invoked on
    /// the simulation thread.
    /// </summary>
    IEvent UpdateEnd { get; }

    /// <summary>
    /// Called at the end of update and should be used only in UI. Unlike other events, this one is invoked even if
    /// the game is paused so that UI can process stuff during pause. Subscribers MUST NOT change game state here!
    /// </summary>
    IEventNonSaveable UpdateEndForUi { get; }

    /// <summary>
    /// Called when the sim thread is in sync with the main thread before any other actions are called. This is a
    /// place where to read values and swap buffers.
    /// </summary>
    IEvent Sync { get; }

    /// <summary>
    /// Invoked just before the game is saved. This can be used to adjust game state before saving to make it more
    /// efficient. Keep in mind that this may be invoked even if the game is not actually being saved for determinism
    /// purposes (for example during replay). Invoked during <see cref="F:Mafi.Core.Simulation.SimLoopState.Sync" />.
    /// </summary>
    IEvent BeforeSave { get; }
  }
}
