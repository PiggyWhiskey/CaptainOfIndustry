// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.GameRunner
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Logging;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Mafi.Core.GameLoop
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  internal class GameRunner : IGameIdProvider
  {
    public const int MAX_GAME_SPEED = 20;
    private Fix32 m_simStepExtraMs;
    private int m_nextSimSpeedMult;
    [DoNotSave(0, null)]
    private bool m_syncedIsPaused;
    [DoNotSave(0, null)]
    private int m_syncedGameSpeed;
    [DoNotSave(0, null)]
    private int m_longTaskSteps;
    private int m_currentLongTaskSteps;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly GameTime m_gameTime;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Stopwatch m_stopwatch;
    private readonly InputScheduler m_inputScheduler;
    private readonly SimulationBackgroundTask m_simTask;
    [DoNotSave(0, null)]
    private BackgroundTaskRunner m_simRunner;
    private readonly GameLoopEvents m_gameLoopEvents;
    private readonly DependencyResolver m_resolver;
    private readonly SimLoopEvents m_simLoopEvents;
    [DoNotSave(0, null)]
    private Fix32 m_timeSinceLastSimUpdateMs;
    [DoNotSave(0, null)]
    private Fix32 m_currSimUpdateDurationMs;
    [DoNotSave(0, null)]
    private Percent m_interpolationMult;
    [DoNotSave(0, null)]
    private readonly bool m_wasNotLoaded;
    private readonly Event<Fix32, bool> m_onUpdateStart;
    private readonly Event m_onSyncStart;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Sets name of the main thread for assertions.</summary>
    public static void TrySetMainThreadName()
    {
      if (Thread.CurrentThread.Name != null)
        return;
      Thread.CurrentThread.Name = "Main";
    }

    /// <summary>
    /// Number of sim updates performed from the start of the game.
    /// </summary>
    public int SimUpdateCount { get; private set; }

    [DoNotSave(0, null)]
    public int SimStepsSinceLoad { get; private set; }

    /// <summary>Total game duration in milliseconds.</summary>
    public Fix64 GameDurationMs { get; private set; }

    [DoNotSave(0, null)]
    public Fix64 GameDurationSinceLoadMs { get; private set; }

    /// <summary>
    /// Current length of sim update. It is sum of <see cref="F:Mafi.Core.GameTime.DEFAULT_SIM_STEP_DURATION_MS" /> and <see cref="P:Mafi.Core.GameLoop.GameRunner.ExtraSimUpdateMs" />.
    /// </summary>
    public Fix32 SimUpdateDurationMs => this.m_currSimUpdateDurationMs;

    /// <summary>
    /// Extra duration of sim step for slowing the game down when sim is too slow. Only positive values are allowed.
    /// </summary>
    public Fix32 ExtraSimUpdateMs
    {
      get => this.m_simStepExtraMs;
      set => this.m_simStepExtraMs = value.CheckNotNegative();
    }

    /// <summary>Number of sim steps for long-running tasks.</summary>
    public int LongTaskSteps
    {
      get => this.m_longTaskSteps;
      set => this.m_longTaskSteps = value.CheckPositive();
    }

    [DoNotSave(0, null)]
    public bool IsInitialized { get; private set; }

    [DoNotSave(0, null)]
    public bool IsTerminated { get; private set; }

    public bool WasLoaded => !this.m_wasNotLoaded;

    /// <summary>
    /// Invoked at the very beginning of <see cref="M:Mafi.Core.GameLoop.GameRunner.Update(Mafi.Fix32)" />. This event is very special, do not use it if you don't
    /// have a really good reason to do so.
    /// </summary>
    internal IEvent<Fix32, bool> OnUpdateStart => (IEvent<Fix32, bool>) this.m_onUpdateStart;

    /// <summary>
    /// Invoked at the very beginning of Sync. This event is very special, do not use it if you don't
    /// have a really good reason to do so.
    /// </summary>
    internal IEvent OnSyncStart => (IEvent) this.m_onSyncStart;

    /// <summary>
    /// Whether game simulation is run synchronously in single (main) thread or the simulation runs in its own
    /// background thread. Can be dynamically toggled during the game (hi-tech!, super cool for debugging!). In
    /// production, sim should always run in background thread of course.
    /// </summary>
    public bool RunSimulationInBackgroundThread { get; set; }

    [DoNotSave(0, null)]
    public TimeSpan LatestSyncDuration { get; private set; }

    [DoNotSave(0, null)]
    public TimeSpan LatestUpdateDuration { get; private set; }

    [DoNotSave(0, null)]
    public TimeSpan LatestInputUpdateDuration { get; private set; }

    [DoNotSave(0, null)]
    public TimeSpan LatestRenderUpdateDuration { get; private set; }

    public TimeSpan LatestSimUpdateDuration => this.m_simRunner.LastWorkDuration;

    public bool LatestSimUpdateWasOvertime => this.m_simRunner.WasOverTime;

    public TimeSpan LatestSimUpdateOvertimeDuration => this.m_simRunner.LastOvertimeDuration;

    public long GameId { get; private set; }

    public DateTime GameStartedAtUtc { get; private set; }

    public string GameStartedAtVersion { get; private set; }

    [DoNotSave(0, null)]
    public long SessionId { get; private set; }

    /// <summary>
    /// Creates new instance of game runner and starts all background threads. Must be called on the main thread.
    /// </summary>
    public GameRunner(
      IGameRunnerConfig config,
      InputScheduler inputScheduler,
      SimulationBackgroundTask simTask,
      SimLoopEvents simLoopEvents,
      GameLoopEvents gameLoopEvents,
      DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_nextSimSpeedMult = 1;
      this.m_longTaskSteps = 5;
      this.m_gameTime = new GameTime();
      this.m_stopwatch = new Stopwatch();
      this.m_currSimUpdateDurationMs = (Fix32) 100;
      this.m_interpolationMult = Percent.Hundred;
      this.m_wasNotLoaded = true;
      this.m_onUpdateStart = new Event<Fix32, bool>(ThreadType.Main);
      this.m_onSyncStart = new Event(ThreadType.Main);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simTask = simTask;
      this.m_inputScheduler = inputScheduler;
      this.m_simLoopEvents = simLoopEvents;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_resolver = resolver;
      this.RunSimulationInBackgroundThread = !config.DisableSimulationBackgroundThread;
      this.GameStartedAtUtc = DateTime.UtcNow;
      this.GameStartedAtVersion = "0.6.3a";
      this.initSelf();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      Mafi.Log.Info(string.Format("Sim thread enabled: {0}", (object) this.RunSimulationInBackgroundThread));
      this.m_stopwatch.Start();
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.SimOnly, (ulong) ~(uint) Environment.TickCount, (ulong) DateTime.Now.Ticks);
      random.Jump();
      this.SessionId = random.NextLong();
      if (this.GameId == 0L)
        this.GameId = random.NextLong();
      Mafi.Log.Info(string.Format("Game ID: {0}; Session ID: {1}, started at v{2}", (object) this.GameId, (object) this.SessionId, (object) this.GameStartedAtVersion));
    }

    /// <summary>
    /// Initializes the game. Should be called on the main thread.
    /// </summary>
    public IEnumerator<string> Initialize(bool gameWasLoaded)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameRunner.\u003CInitialize\u003Ed__105(0)
      {
        \u003C\u003E4__this = this,
        gameWasLoaded = gameWasLoaded
      };
    }

    /// <summary>
    /// Terminates the game and simulation thread. Should be called on the main thread. This game runner instance is
    /// unusable after this call.
    /// </summary>
    public void Terminate()
    {
      Mafi.Assert.That<bool>(this.IsInitialized).IsTrue("Not initialized!");
      if (this.IsTerminated)
      {
        Mafi.Log.Error("Already terminated!");
      }
      else
      {
        this.m_simRunner.Terminate();
        ThreadAssert.Disable();
        StateAssert.Disable();
        this.m_gameLoopEvents.InvokeTerminate();
        this.IsTerminated = true;
      }
    }

    public void Update(Fix32 deltaTimeMs)
    {
      Mafi.Assert.That<Fix32>(deltaTimeMs).IsPositive();
      Mafi.Assert.That<bool>(this.IsInitialized).IsTrue("Stepping not initialized game!");
      Mafi.Assert.That<bool>(this.IsTerminated).IsFalse("Stepping already terminated game!");
      TimeSpan elapsed1 = this.m_stopwatch.Elapsed;
      bool flag = this.m_timeSinceLastSimUpdateMs + deltaTimeMs >= this.m_currSimUpdateDurationMs;
      this.m_onUpdateStart.Invoke(deltaTimeMs, flag);
      Percent percent1 = Percent.FromRatio(this.m_timeSinceLastSimUpdateMs, this.m_currSimUpdateDurationMs);
      Mafi.Assert.That<Percent>(percent1).IsWithin0To100PercIncl();
      this.m_timeSinceLastSimUpdateMs += deltaTimeMs;
      float deltaTimeMs1 = deltaTimeMs.ToFloat();
      Percent percent2 = Percent.FromRatio(this.m_timeSinceLastSimUpdateMs, this.m_currSimUpdateDurationMs);
      Percent deltaStepsApprox = percent2 - percent1;
      Mafi.Assert.That<Percent>(deltaStepsApprox).IsPositive();
      Fix32 sinceLastSimUpdateMs = this.m_timeSinceLastSimUpdateMs;
      Percent relativeT;
      Percent absoluteT;
      if (flag)
      {
        Fix32 updateDurationMs = this.m_currSimUpdateDurationMs;
        this.performSyncAndStartSim(deltaTimeMs1);
        Mafi.Assert.That<Percent>(percent2).IsGreaterOrEqual(Percent.Hundred);
        this.m_timeSinceLastSimUpdateMs -= updateDurationMs;
        sinceLastSimUpdateMs -= updateDurationMs;
        if (!this.m_syncedIsPaused)
        {
          this.GameDurationMs += updateDurationMs;
          this.GameDurationSinceLoadMs += updateDurationMs;
        }
        if (this.m_timeSinceLastSimUpdateMs >= updateDurationMs)
          this.m_timeSinceLastSimUpdateMs = (Fix32) 0;
        Percent percent3 = Percent.Hundred - percent1;
        Mafi.Assert.That<Percent>(percent3).IsNotNegative();
        Percent percent4 = Percent.Hundred + percent3;
        Mafi.Assert.That<Percent>(percent4).IsGreaterOrEqual(Percent.Hundred);
        this.m_interpolationMult = Percent.Hundred / percent4;
        Mafi.Assert.That<Percent>(this.m_interpolationMult).IsLessOrEqual(Percent.Hundred);
        relativeT = (deltaStepsApprox * this.m_interpolationMult).Clamp0To100();
        absoluteT = Percent.FromRatio(this.m_timeSinceLastSimUpdateMs, updateDurationMs);
        if (sinceLastSimUpdateMs.IsZero)
          this.m_interpolationMult = Percent.Hundred;
      }
      else
      {
        Mafi.Assert.That<Percent>(percent2).IsLess(Percent.Hundred);
        Percent percent5 = Percent.Hundred - percent1;
        relativeT = (deltaStepsApprox / percent5 * this.m_interpolationMult).Clamp0To100();
        absoluteT = Percent.FromRatio(this.m_timeSinceLastSimUpdateMs, this.m_currSimUpdateDurationMs);
      }
      if (this.m_syncedIsPaused)
        this.m_gameTime.UpdatePaused(this.SimUpdateCount, this.SimStepsSinceLoad);
      else
        this.m_gameTime.Update(deltaTimeMs1, this.GameDurationMs + sinceLastSimUpdateMs, this.GameDurationSinceLoadMs + sinceLastSimUpdateMs, sinceLastSimUpdateMs, absoluteT, relativeT, this.m_syncedGameSpeed, this.SimUpdateCount, this.SimStepsSinceLoad, deltaStepsApprox);
      TimeSpan elapsed2 = this.m_stopwatch.Elapsed;
      this.performInputUpdate();
      this.LatestInputUpdateDuration = this.m_stopwatch.Elapsed - elapsed2;
      TimeSpan elapsed3 = this.m_stopwatch.Elapsed;
      this.performRenderUpdate();
      this.LatestRenderUpdateDuration = this.m_stopwatch.Elapsed - elapsed3;
      this.LatestUpdateDuration = this.m_stopwatch.Elapsed - elapsed1;
    }

    private void performInitSimUpdate()
    {
      this.m_simRunner.Sync();
      this.m_inputScheduler.CollectCommands();
      this.m_simRunner.PerformWork(false);
    }

    internal void BlockUntilSimStepDone() => this.m_simRunner.WaitForFinishWork();

    private void performSyncAndStartSim(float deltaTimeMs)
    {
      if (this.m_longTaskSteps >= this.m_currentLongTaskSteps)
        this.m_currentLongTaskSteps = 0;
      this.m_simRunner.WaitForFinishWork();
      TimeSpan elapsed = this.m_stopwatch.Elapsed;
      this.m_syncedIsPaused = this.m_simLoopEvents.IsSimPaused;
      this.m_syncedGameSpeed = this.m_simLoopEvents.SimSpeedMult;
      this.SimUpdateCount = this.m_simLoopEvents.CurrentStep.Value;
      this.SimStepsSinceLoad = this.m_simLoopEvents.StepsSinceLoad.Value;
      this.m_currSimUpdateDurationMs = (Fix32) 100 + this.m_simStepExtraMs;
      this.m_gameTime.UpdateForSync(deltaTimeMs, this.m_syncedIsPaused, this.m_syncedGameSpeed, this.SimUpdateCount, this.SimStepsSinceLoad, this.m_currSimUpdateDurationMs);
      this.m_onSyncStart.Invoke();
      this.m_simRunner.Sync();
      this.performSyncUpdate();
      this.m_inputScheduler.CollectCommands();
      this.LatestSyncDuration = this.m_stopwatch.Elapsed - elapsed;
      this.m_simRunner.PerformWork(this.RunSimulationInBackgroundThread);
    }

    private void performSyncUpdate()
    {
      try
      {
        this.m_gameLoopEvents.InvokeSyncUpdateStart(this.m_gameTime);
        this.m_gameLoopEvents.InvokeSyncUpdate(this.m_gameTime);
        this.m_gameLoopEvents.InvokeSyncUpdateEnd(this.m_gameTime);
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, ex.GetType().Name + " thrown during sync update: " + ex.Message);
      }
    }

    private void performInputUpdate()
    {
      try
      {
        this.m_gameLoopEvents.InvokeInputUpdate(this.m_gameTime);
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, ex.GetType().Name + " thrown during input update: " + ex.Message);
      }
    }

    private void performRenderUpdate()
    {
      try
      {
        this.m_gameLoopEvents.InvokeRenderUpdate(this.m_gameTime);
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, ex.GetType().Name + " thrown during render: " + ex.Message);
      }
    }

    public static void Serialize(GameRunner value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameRunner>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameRunner.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.ExtraSimUpdateMs, writer);
      Fix64.Serialize(this.GameDurationMs, writer);
      writer.WriteLong(this.GameId);
      writer.WriteDateTime(this.GameStartedAtUtc);
      writer.WriteString(this.GameStartedAtVersion);
      writer.WriteInt(this.LongTaskSteps);
      writer.WriteInt(this.m_currentLongTaskSteps);
      GameLoopEvents.Serialize(this.m_gameLoopEvents, writer);
      InputScheduler.Serialize(this.m_inputScheduler, writer);
      writer.WriteInt(this.m_nextSimSpeedMult);
      Event.Serialize(this.m_onSyncStart, writer);
      Event<Fix32, bool>.Serialize(this.m_onUpdateStart, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      SimLoopEvents.Serialize(this.m_simLoopEvents, writer);
      Fix32.Serialize(this.m_simStepExtraMs, writer);
      SimulationBackgroundTask.Serialize(this.m_simTask, writer);
      writer.WriteBool(this.RunSimulationInBackgroundThread);
      writer.WriteInt(this.SimUpdateCount);
    }

    public static GameRunner Deserialize(BlobReader reader)
    {
      GameRunner gameRunner;
      if (reader.TryStartClassDeserialization<GameRunner>(out gameRunner))
        reader.EnqueueDataDeserialization((object) gameRunner, GameRunner.s_deserializeDataDelayedAction);
      return gameRunner;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ExtraSimUpdateMs = Fix32.Deserialize(reader);
      this.GameDurationMs = Fix64.Deserialize(reader);
      this.GameId = reader.ReadLong();
      this.GameStartedAtUtc = reader.ReadDateTime();
      this.GameStartedAtVersion = reader.ReadString();
      this.LongTaskSteps = reader.ReadInt();
      this.m_currentLongTaskSteps = reader.ReadInt();
      reader.SetField<GameRunner>(this, "m_gameLoopEvents", (object) GameLoopEvents.Deserialize(reader));
      reader.SetField<GameRunner>(this, "m_gameTime", (object) new GameTime());
      reader.SetField<GameRunner>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      this.m_nextSimSpeedMult = reader.ReadInt();
      reader.SetField<GameRunner>(this, "m_onSyncStart", (object) Event.Deserialize(reader));
      reader.SetField<GameRunner>(this, "m_onUpdateStart", (object) Event<Fix32, bool>.Deserialize(reader));
      reader.SetField<GameRunner>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<GameRunner>(this, "m_simLoopEvents", (object) SimLoopEvents.Deserialize(reader));
      this.m_simStepExtraMs = Fix32.Deserialize(reader);
      reader.SetField<GameRunner>(this, "m_simTask", (object) SimulationBackgroundTask.Deserialize(reader));
      reader.SetField<GameRunner>(this, "m_stopwatch", (object) new Stopwatch());
      this.RunSimulationInBackgroundThread = reader.ReadBool();
      this.SimUpdateCount = reader.ReadInt();
      reader.RegisterInitAfterLoad<GameRunner>(this, "initSelf", InitPriority.Normal);
    }

    static GameRunner()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameRunner.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameRunner) obj).SerializeData(writer));
      GameRunner.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameRunner) obj).DeserializeData(reader));
    }
  }
}
