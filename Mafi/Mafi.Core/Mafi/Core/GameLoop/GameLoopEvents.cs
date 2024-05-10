// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.GameLoopEvents
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.GameLoop
{
  /// <summary>
  /// Game loop events control the main loop of the game and are invoked on the main thread.
  /// 
  /// Inner game loop is running at the fastest possible rate which is usually the refresh rate of the monitor (60 Hz,
  /// can be 120 Hz). Frequency can slow down if the hardware is not fast enough.
  /// 
  /// All core game logic runs in the simulation thread and uses <see cref="T:Mafi.Core.Simulation.ISimLoopEvents" />.
  /// Game loop events should be used only for rendering front-end and user input.
  /// 
  /// All events are invoked on the main (rendering) thread.
  /// </summary>
  /// <remarks>
  /// The game loop events are called as follows:
  /// <code>
  /// if ( game is loaded from file ) {
  /// 	Run deserializers (constructors of deserialized classes are NOT called)
  /// 	Instantiate non-saved dependencies (via their constructors)
  /// 	Run methods marked with [InitAfterLoad]
  /// } else {
  /// 	Instantiate all registered dependencies  (via their constructors)
  /// 	NewGameCreated()
  /// 	while( any( INeedsSimUpdatesForInit.NeedsMoreSimUpdates ) ) {
  /// 		perform sim loop
  /// 	}
  /// 	NewGameInitialized()
  /// }
  /// 
  /// RendererInitState()
  /// 
  /// while ( not terminate game ) {
  /// 	if ( simulation update is due ) {
  /// 		SyncUpdateStart()
  /// 		SyncUpdate()
  /// 	}
  /// 
  /// 	InputUpdate()
  /// 	InputUpdateEnd()
  /// 
  /// 	RenderUpdate()
  /// }
  /// 
  /// Terminate()
  /// </code>
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GameLoopEvents : IGameLoopEvents, INewGameCreatedEvents
  {
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_syncUpdateStart;
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_syncUpdate;
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_syncUpdateEnd;
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_inputUpdate;
    [DoNotSave(0, null)]
    private EventNonSaveable m_inputUpdateEnd;
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_renderUpdate;
    [DoNotSave(0, null)]
    private EventNonSaveable<GameTime> m_renderUpdateEnd;
    [DoNotSave(0, null)]
    private EventNonSaveable m_terminate;
    [DoNotSave(0, null)]
    private EventNonSaveable m_onProjectChanged;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Queueue<Action> m_invokeInSyncNotSaved;
    [DoNotSave(0, null)]
    private Lyst<GameLoopEvents.EnumeratorCallback> m_newGameCreated;
    [DoNotSave(0, null)]
    private Lyst<GameLoopEvents.EnumeratorCallback> m_newGameInitialized;
    [DoNotSave(0, null)]
    private Lyst<INeedsSimUpdatesForInit> m_simUpdateInit;
    [DoNotSave(0, null)]
    private Lyst<GameLoopEvents.EnumeratorCallback> m_rendererInitState;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GameLoopState CurrentState { get; private set; }

    public GameLoopState LastState { get; private set; }

    public bool GameWasLoaded => this.SaveVersion.HasValue;

    [DoNotSave(0, null)]
    public int? SaveVersion { get; private set; }

    [DoNotSave(0, null)]
    public bool IsTerminated { get; private set; }

    public IEventNonSaveable<GameTime> SyncUpdateStart
    {
      get => (IEventNonSaveable<GameTime>) this.m_syncUpdateStart;
    }

    public IEventNonSaveable<GameTime> SyncUpdate
    {
      get => (IEventNonSaveable<GameTime>) this.m_syncUpdate;
    }

    public IEventNonSaveable<GameTime> SyncUpdateEnd
    {
      get => (IEventNonSaveable<GameTime>) this.m_syncUpdateEnd;
    }

    public IEventNonSaveable<GameTime> InputUpdate
    {
      get => (IEventNonSaveable<GameTime>) this.m_inputUpdate;
    }

    public IEventNonSaveable InputUpdateEnd => (IEventNonSaveable) this.m_inputUpdateEnd;

    public IEventNonSaveable<GameTime> RenderUpdate
    {
      get => (IEventNonSaveable<GameTime>) this.m_renderUpdate;
    }

    public IEventNonSaveable<GameTime> RenderUpdateEnd
    {
      get => (IEventNonSaveable<GameTime>) this.m_renderUpdateEnd;
    }

    public IEventNonSaveable Terminate => (IEventNonSaveable) this.m_terminate;

    public IEventNonSaveable OnProjectChanged => (IEventNonSaveable) this.m_onProjectChanged;

    public GameLoopEvents()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_invokeInSyncNotSaved = new Queueue<Action>();
      this.m_newGameCreated = new Lyst<GameLoopEvents.EnumeratorCallback>();
      this.m_newGameInitialized = new Lyst<GameLoopEvents.EnumeratorCallback>();
      this.m_simUpdateInit = new Lyst<INeedsSimUpdatesForInit>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.createEvents();
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initSelf(int saveVersion)
    {
      this.SaveVersion = new int?(saveVersion);
      this.createEvents();
    }

    private void createEvents()
    {
      this.m_rendererInitState = new Lyst<GameLoopEvents.EnumeratorCallback>();
      this.m_syncUpdateStart = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_syncUpdate = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_syncUpdateEnd = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_inputUpdate = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_inputUpdateEnd = new EventNonSaveable(ThreadType.Main);
      this.m_renderUpdate = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_renderUpdateEnd = new EventNonSaveable<GameTime>(ThreadType.Main);
      this.m_terminate = new EventNonSaveable(ThreadType.Main);
      this.m_onProjectChanged = new EventNonSaveable(ThreadType.Main);
    }

    void INewGameCreatedEvents.RegisterNewGameCreated(
      object registrator,
      IEnumerator<string> iterativeCallback)
    {
      if (this.m_newGameCreated == null)
        throw new InvalidOperationException("Registration of new 'NewGameCreated' event by '" + registrator.GetType().Name + "' " + string.Format("but the event was already invoked. Current state: {0}, last state: {1}", (object) this.CurrentState, (object) this.LastState));
      if (this.CurrentState == GameLoopState.NewGameCreated)
        throw new InvalidOperationException("Registration of new 'NewGameCreated' event by '" + registrator.GetType().Name + "' while the event is being invoked.");
      this.m_newGameCreated.Add(new GameLoopEvents.EnumeratorCallback(registrator, iterativeCallback));
    }

    void IGameLoopEvents.RegisterNewGameInitialized(
      object registrator,
      IEnumerator<string> iterativeCallback)
    {
      if (this.m_newGameInitialized == null)
        throw new InvalidOperationException("Registration of new 'NewGameInitialized' event by '" + registrator.GetType().Name + "' " + string.Format("but the event was already invoked. Current state: {0}, last state: {1}", (object) this.CurrentState, (object) this.LastState));
      if (this.CurrentState == GameLoopState.NewGameInitialized)
        throw new InvalidOperationException("Registration of new 'NewGameInitialized' event by '" + registrator.GetType().Name + "' while the event is being invoked.");
      this.m_newGameInitialized.Add(new GameLoopEvents.EnumeratorCallback(registrator, iterativeCallback));
    }

    public void RegisterInitSimUpdate(INeedsSimUpdatesForInit obj)
    {
      if (this.m_simUpdateInit == null)
        throw new InvalidOperationException("Registration of new 'InitSimUpdate' event by '" + obj.GetType().Name + "' " + string.Format("but the event was already invoked. Current state: {0}, last state: {1}", (object) this.CurrentState, (object) this.LastState));
      if (this.CurrentState != GameLoopState.None || this.LastState != GameLoopState.None)
        Log.Error("Cannot register sim init at this stage!");
      else
        this.m_simUpdateInit.Add(obj);
    }

    void IGameLoopEvents.RegisterRendererInitState(
      object registrator,
      IEnumerator<string> iterativeCallback)
    {
      if (this.m_rendererInitState == null)
        throw new InvalidOperationException("Registration of new 'RendererInitState' event by '" + registrator.GetType().Name + "' " + string.Format("but the event was already invoked. Current state: {0}, last state: {1}", (object) this.CurrentState, (object) this.LastState));
      if (this.CurrentState == GameLoopState.RendererInitState)
        throw new InvalidOperationException("Registration of new 'RendererInitState' event by '" + registrator.GetType().Name + "' while the event is being invoked.");
      this.m_rendererInitState.Add(new GameLoopEvents.EnumeratorCallback(registrator, iterativeCallback));
    }

    void IGameLoopEvents.InvokeInSyncNotSaved(Action action)
    {
      lock (this.m_invokeInSyncNotSaved)
        this.m_invokeInSyncNotSaved.Enqueue(action);
    }

    public IEnumerator<string> InvokeNewGameCreated()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameLoopEvents.\u003CInvokeNewGameCreated\u003Ed__58(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public IEnumerator<string> InvokeNewGameInitialized()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameLoopEvents.\u003CInvokeNewGameInitialized\u003Ed__59(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public bool NeedsMoreSimUpdates()
    {
      Assert.That<GameLoopState>(this.LastState).IsEqualTo<GameLoopState>(GameLoopState.NewGameCreated, "Needs more sim updates should be called only after new game was loaded.");
      if (this.m_simUpdateInit == null)
        return false;
      foreach (INeedsSimUpdatesForInit simUpdatesForInit in this.m_simUpdateInit)
      {
        if (simUpdatesForInit.FailedInit)
          return false;
        if (simUpdatesForInit.NeedsMoreSimUpdates)
          return true;
      }
      this.m_simUpdateInit = (Lyst<INeedsSimUpdatesForInit>) null;
      return false;
    }

    /// <summary>
    /// All registered classes for sim update init. Note that this will return null after sip update is finished.
    /// </summary>
    internal IEnumerable<INeedsSimUpdatesForInit> SimUpdateInit
    {
      get => (IEnumerable<INeedsSimUpdatesForInit>) this.m_simUpdateInit;
    }

    public IEnumerator<string> InvokeRendererInitializeState()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameLoopEvents.\u003CInvokeRendererInitializeState\u003Ed__63(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public void InvokeSyncUpdateStart(GameTime gameTime)
    {
      this.CurrentState = this.LastState = GameLoopState.SyncUpdateStart;
      if (this.m_invokeInSyncNotSaved.IsNotEmpty)
      {
        lock (this.m_invokeInSyncNotSaved)
        {
          while (this.m_invokeInSyncNotSaved.IsNotEmpty)
          {
            try
            {
              this.m_invokeInSyncNotSaved.Dequeue()();
            }
            catch (Exception ex)
            {
              Log.Exception(ex, "Thrown by invoke-in-sync action.");
            }
          }
        }
      }
      this.m_syncUpdateStart.InvokeTraced(gameTime, "GLE");
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeSyncUpdate(GameTime gameTime)
    {
      this.CurrentState = this.LastState = GameLoopState.SyncUpdate;
      this.m_syncUpdate.InvokeTraced(gameTime, "GLE");
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeSyncUpdateEnd(GameTime gameTime)
    {
      this.CurrentState = this.LastState = GameLoopState.SyncUpdateEnd;
      this.m_syncUpdateEnd.InvokeTraced(gameTime, "GLE");
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeInputUpdate(GameTime gameTime)
    {
      this.CurrentState = this.LastState = GameLoopState.InputUpdate;
      this.m_inputUpdate.InvokeTraced(gameTime, "GLE");
      this.CurrentState = GameLoopState.InputUpdateEnd;
      this.m_inputUpdateEnd.InvokeTraced("GLE");
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeRenderUpdate(GameTime gameTime)
    {
      this.CurrentState = this.LastState = GameLoopState.RenderUpdate;
      this.m_renderUpdate.InvokeTraced(gameTime, "GLE");
      this.CurrentState = this.LastState = GameLoopState.RenderUpdateEnd;
      this.m_renderUpdateEnd.InvokeTraced(gameTime, "GLE");
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeTerminate()
    {
      Assert.That<bool>(this.IsTerminated).IsFalse("Game was already terminated.");
      this.CurrentState = this.LastState = GameLoopState.Terminate;
      this.m_terminate.InvokeTraced("GLE");
      this.IsTerminated = true;
      this.CurrentState = GameLoopState.None;
    }

    public void InvokeOnProjectChanged() => this.m_onProjectChanged.Invoke();

    public static void Serialize(GameLoopEvents value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameLoopEvents>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameLoopEvents.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt((int) this.CurrentState);
      writer.WriteInt((int) this.LastState);
    }

    public static GameLoopEvents Deserialize(BlobReader reader)
    {
      GameLoopEvents gameLoopEvents;
      if (reader.TryStartClassDeserialization<GameLoopEvents>(out gameLoopEvents))
        reader.EnqueueDataDeserialization((object) gameLoopEvents, GameLoopEvents.s_deserializeDataDelayedAction);
      return gameLoopEvents;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CurrentState = (GameLoopState) reader.ReadInt();
      this.LastState = (GameLoopState) reader.ReadInt();
      reader.SetField<GameLoopEvents>(this, "m_invokeInSyncNotSaved", (object) new Queueue<Action>());
      this.initSelf(reader.LoadedSaveVersion);
    }

    static GameLoopEvents()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameLoopEvents.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameLoopEvents) obj).SerializeData(writer));
      GameLoopEvents.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameLoopEvents) obj).DeserializeData(reader));
    }

    private struct EnumeratorCallback
    {
      public readonly object Registrator;
      public readonly IEnumerator<string> Enumerator;

      public EnumeratorCallback(object registrator, IEnumerator<string> callback)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Registrator = registrator.CheckNotNull<object>();
        this.Enumerator = callback.CheckNotNull<IEnumerator<string>>();
      }
    }
  }
}
