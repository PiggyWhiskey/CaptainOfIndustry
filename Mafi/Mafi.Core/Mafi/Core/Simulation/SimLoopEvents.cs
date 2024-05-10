// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.SimLoopEvents
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class SimLoopEvents : ISimLoopEvents, ISimStepProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Event m_updateAfterCmdProc;
    private readonly Event m_updateAfterSync;
    private readonly Event m_updateStart;
    private readonly Event m_update;
    private readonly Event m_updateEnd;
    [DoNotSave(0, null)]
    private EventNonSaveable m_updateEndForUi;
    private readonly Event m_sync;
    private readonly Event m_beforeSave;

    public static void Serialize(SimLoopEvents value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SimLoopEvents>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SimLoopEvents.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteInt((int) this.CurrentState);
      SimStep.Serialize(this.CurrentStep, writer);
      writer.WriteBool(this.IsInSimLoop);
      Event.Serialize(this.m_beforeSave, writer);
      Event.Serialize(this.m_sync, writer);
      Event.Serialize(this.m_update, writer);
      Event.Serialize(this.m_updateAfterCmdProc, writer);
      Event.Serialize(this.m_updateAfterSync, writer);
      Event.Serialize(this.m_updateEnd, writer);
      Event.Serialize(this.m_updateStart, writer);
    }

    public static SimLoopEvents Deserialize(BlobReader reader)
    {
      SimLoopEvents simLoopEvents;
      if (reader.TryStartClassDeserialization<SimLoopEvents>(out simLoopEvents))
        reader.EnqueueDataDeserialization((object) simLoopEvents, SimLoopEvents.s_deserializeDataDelayedAction);
      return simLoopEvents;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.CurrentState = (SimLoopState) reader.ReadInt();
      this.CurrentStep = SimStep.Deserialize(reader);
      this.IsInSimLoop = reader.ReadBool();
      reader.SetField<SimLoopEvents>(this, "m_beforeSave", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_sync", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_update", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_updateAfterCmdProc", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_updateAfterSync", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_updateEnd", (object) Event.Deserialize(reader));
      reader.SetField<SimLoopEvents>(this, "m_updateStart", (object) Event.Deserialize(reader));
      this.initSelf();
    }

    public SimStep CurrentStep { get; private set; }

    [DoNotSave(0, null)]
    public SimStep StepsSinceLoad { get; private set; }

    public SimLoopState CurrentState { get; private set; }

    [DoNotSave(0, null)]
    public bool IsSimPaused { get; private set; }

    [DoNotSave(0, null)]
    public int SimSpeedMult { get; private set; }

    public bool IsInSimLoop { get; private set; }

    public IEvent UpdateAfterCmdProc => (IEvent) this.m_updateAfterCmdProc;

    public IEvent UpdateAfterSync => (IEvent) this.m_updateAfterSync;

    public IEvent UpdateStart => (IEvent) this.m_updateStart;

    public IEvent Update => (IEvent) this.m_update;

    public IEvent UpdateEnd => (IEvent) this.m_updateEnd;

    public IEventNonSaveable UpdateEndForUi => (IEventNonSaveable) this.m_updateEndForUi;

    public IEvent Sync => (IEvent) this.m_sync;

    public IEvent BeforeSave => (IEvent) this.m_beforeSave;

    int ISimStepProvider.CurrentSimStep => this.CurrentStep.Value;

    bool ISimStepProvider.IsTerminated => this.CurrentState == SimLoopState.Terminated;

    public SimLoopEvents()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CSimSpeedMult\u003Ek__BackingField = 1;
      this.m_updateAfterCmdProc = new Event();
      this.m_updateAfterSync = new Event();
      this.m_updateStart = new Event();
      this.m_update = new Event();
      this.m_updateEnd = new Event();
      this.m_sync = new Event(ThreadType.Main);
      this.m_beforeSave = new Event(ThreadType.Main);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.initSelf();
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initSelf()
    {
      this.CurrentState = SimLoopState.None;
      this.m_updateEndForUi = new EventNonSaveable(ThreadType.Sim);
    }

    internal void Initialize() => this.CurrentStep = SimStep.One;

    internal void InvokeSync()
    {
      Assert.That<SimStep>(this.CurrentStep).IsPositive();
      Assert.That<SimLoopState>(this.CurrentState).IsEqualTo<SimLoopState>(SimLoopState.None);
      Assert.That<bool>(this.IsInSimLoop).IsFalse();
      this.CurrentState = SimLoopState.Sync;
      this.m_sync.InvokeTraced("SLE");
      this.CurrentState = SimLoopState.None;
    }

    internal void InvokeGameStep(IInputCommandsProcessor inputCommandsProcessor)
    {
      Assert.That<SimStep>(this.CurrentStep).IsPositive();
      Assert.That<SimLoopState>(this.CurrentState).IsEqualTo<SimLoopState>(SimLoopState.None);
      Assert.That<bool>(this.IsInSimLoop).IsFalse();
      this.IsInSimLoop = true;
      this.CurrentState = SimLoopState.CommandsProcessing;
      inputCommandsProcessor.ProcessCommands();
      this.m_updateAfterCmdProc.InvokeTraced("SLE");
      this.CurrentState = SimLoopState.None;
      if (this.IsSimPaused)
      {
        this.CurrentState = SimLoopState.UpdateEndForUi;
        this.m_updateEndForUi.InvokeTraced("SLE");
        this.CurrentState = SimLoopState.None;
        this.IsInSimLoop = false;
      }
      else
      {
        this.CurrentState = SimLoopState.UpdateAfterSync;
        this.m_updateAfterSync.InvokeTraced("SLE");
        this.CurrentState = SimLoopState.None;
        for (int index = 0; index < this.SimSpeedMult; ++index)
        {
          this.CurrentStep += SimStep.One;
          this.StepsSinceLoad += SimStep.One;
          if (index > 0)
          {
            this.CurrentState = SimLoopState.CommandsProcessing;
            inputCommandsProcessor.ProcessCommands();
            this.m_updateAfterCmdProc.InvokeTraced("SLE");
          }
          this.CurrentState = SimLoopState.UpdateStart;
          this.m_updateStart.InvokeTraced("SLE");
          this.CurrentState = SimLoopState.Update;
          this.m_update.InvokeTraced("SLE");
          this.CurrentState = SimLoopState.UpdateEnd;
          this.m_updateEnd.InvokeTraced("SLE");
        }
        this.CurrentState = SimLoopState.UpdateEndForUi;
        this.m_updateEndForUi.InvokeTraced("SLE");
        this.CurrentState = SimLoopState.None;
        this.IsInSimLoop = false;
      }
    }

    public void SetSimPause(bool isPaused)
    {
      if (this.IsInSimLoop && this.CurrentState != SimLoopState.CommandsProcessing)
        Log.Error("Sim pause can be only set in commands processing phase or outside of sim");
      else
        this.IsSimPaused = isPaused;
    }

    public void SetSimSpeed(int speedMult)
    {
      if (this.IsInSimLoop && this.CurrentState != SimLoopState.CommandsProcessing)
        Log.Error("Sim pause can be only set in commands processing phase or outside of sim");
      else
        this.SimSpeedMult = speedMult.CheckWithinIncl(1, 20);
    }

    internal void SetTerminated()
    {
      Assert.That<SimLoopState>(this.CurrentState).IsEqualTo<SimLoopState>(SimLoopState.None);
      Assert.That<bool>(this.IsInSimLoop).IsFalse();
      this.CurrentState = SimLoopState.Terminated;
    }

    internal void InvokeBeforeSave(bool fromTest = false)
    {
      if (!fromTest)
        Assert.That<SimLoopState>(this.CurrentState).IsEqualTo<SimLoopState>(SimLoopState.Sync);
      this.m_beforeSave.InvokeTraced("SLE");
    }

    static SimLoopEvents()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SimLoopEvents.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SimLoopEvents) obj).SerializeData(writer));
      SimLoopEvents.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SimLoopEvents) obj).DeserializeData(reader));
    }
  }
}
