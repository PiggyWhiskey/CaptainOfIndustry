// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.SimulationBackgroundTask
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  internal class SimulationBackgroundTask : IBackgroundTask
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly SimLoopEvents m_simLoopEvents;
    private readonly InputScheduler m_inputScheduler;

    public static void Serialize(SimulationBackgroundTask value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SimulationBackgroundTask>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SimulationBackgroundTask.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      InputScheduler.Serialize(this.m_inputScheduler, writer);
      SimLoopEvents.Serialize(this.m_simLoopEvents, writer);
    }

    public static SimulationBackgroundTask Deserialize(BlobReader reader)
    {
      SimulationBackgroundTask simulationBackgroundTask;
      if (reader.TryStartClassDeserialization<SimulationBackgroundTask>(out simulationBackgroundTask))
        reader.EnqueueDataDeserialization((object) simulationBackgroundTask, SimulationBackgroundTask.s_deserializeDataDelayedAction);
      return simulationBackgroundTask;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SimulationBackgroundTask>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      reader.SetField<SimulationBackgroundTask>(this, "m_simLoopEvents", (object) SimLoopEvents.Deserialize(reader));
    }

    public SimulationBackgroundTask(
      IGameLoopEvents gameLoopEvents,
      SimLoopEvents simLoopEvents,
      InputScheduler inputScheduler)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simLoopEvents = simLoopEvents;
      this.m_inputScheduler = inputScheduler;
      gameLoopEvents.RegisterNewGameCreated((object) this, new Action(this.m_simLoopEvents.Initialize));
    }

    public void PerformSync() => this.m_simLoopEvents.InvokeSync();

    public void PerformWork()
    {
      this.m_simLoopEvents.InvokeGameStep((IInputCommandsProcessor) this.m_inputScheduler);
    }

    public void Terminated() => this.m_simLoopEvents.SetTerminated();

    static SimulationBackgroundTask()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SimulationBackgroundTask.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SimulationBackgroundTask) obj).SerializeData(writer));
      SimulationBackgroundTask.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SimulationBackgroundTask) obj).DeserializeData(reader));
    }
  }
}
