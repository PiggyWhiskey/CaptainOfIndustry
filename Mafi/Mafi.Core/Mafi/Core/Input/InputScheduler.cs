// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.InputScheduler
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Input
{
  /// <summary>
  /// Collects input commands and invokes them in the right time. It also makes sure that input commands are correctly
  /// synchronized input between main and sim threads.
  /// </summary>
  /// <remarks>
  /// This class is a core of input processing system and cannot be replaced by a mod. The purpose of <see cref="T:Mafi.Core.Input.IInputScheduler" /> is to expose only some API to the users.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class InputScheduler : IInputScheduler, IInputCommandsProcessor
  {
    /// <summary>
    /// Temporary storage for commands scheduled from the main thread.
    /// </summary>
    private readonly Lyst<IInputCommand> m_mainCommands;
    /// <summary>
    /// Temporary storage for commands scheduled from the sim thread.
    /// </summary>
    private readonly Lyst<IInputCommand> m_simCommands;
    /// <summary>
    /// Commands waiting to be processed at the next processing event.
    /// </summary>
    private readonly Lyst<IInputCommand> m_commandsToProcess;
    private readonly Lyst<IInputCommand> m_verifCmds;
    private readonly DependencyResolver m_resolver;
    private readonly ISimLoopEvents m_simLoopEvents;
    /// <summary>New processed commands since last sync.</summary>
    private readonly Lyst<IInputCommand> m_newlyProcessedCommands;
    private readonly Event<IInputCommand> m_onCommandProcessed;
    private readonly Queueue<IInputCommand> m_replayCommandsToProcess;
    private readonly Event<IInputCommand> m_onCommandScheduled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// All processed commands. This contains info to reconstruct a replay of the game.
    /// </summary>
    [DoNotSave(0, null)]
    public int ProcessedCommandsInThisSessionAffectingSave { get; private set; }

    public IEvent<IInputCommand> OnCommandProcessed
    {
      get => (IEvent<IInputCommand>) this.m_onCommandProcessed;
    }

    internal bool HasQueuedCommands
    {
      get => this.m_mainCommands.IsNotEmpty || this.m_simCommands.IsNotEmpty;
    }

    /// <summary>
    /// Invoked at the very beginning of <see cref="M:Mafi.Core.Input.InputScheduler.ScheduleInputCmd``1(``0)" /> (on the same thread as scheduled
    /// command).
    /// </summary>
    internal IEvent<IInputCommand> OnCommandScheduled
    {
      get => (IEvent<IInputCommand>) this.m_onCommandScheduled;
    }

    public InputScheduler(DependencyResolver resolver, ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_mainCommands = new Lyst<IInputCommand>(true);
      this.m_simCommands = new Lyst<IInputCommand>(true);
      this.m_commandsToProcess = new Lyst<IInputCommand>(true);
      this.m_verifCmds = new Lyst<IInputCommand>(true);
      this.m_newlyProcessedCommands = new Lyst<IInputCommand>();
      this.m_replayCommandsToProcess = new Queueue<IInputCommand>();
      this.m_onCommandScheduled = new Event<IInputCommand>(ThreadType.Any);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simLoopEvents = simLoopEvents;
      this.m_resolver = resolver.CheckNotNull<DependencyResolver>();
      this.m_onCommandProcessed = new Event<IInputCommand>();
    }

    public T ScheduleInputCmd<T>(T cmd) where T : IInputCommand
    {
      Assert.That<bool>(cmd.IsProcessedAndSynced).IsFalse("Command already processed.");
      this.m_onCommandScheduled.Invoke((IInputCommand) cmd);
      if (cmd.IsVerificationCmd)
      {
        this.m_verifCmds.Add((IInputCommand) cmd);
        return cmd;
      }
      if (ThreadUtils.ThreadNameFast == "Simulation")
        this.m_mainCommands.Add((IInputCommand) cmd);
      else
        this.m_simCommands.Add((IInputCommand) cmd);
      return cmd;
    }

    /// <summary>
    /// Adds a given command with set result to the replay at the current sim step. The command is marked as
    /// processed in the process.
    /// </summary>
    internal void AddCmdToReplay(IInputCommand cmd)
    {
      ((IInputCommandFriend) cmd).MarkProcessed(this.m_simLoopEvents.CurrentStep);
      this.m_onCommandProcessed.Invoke(cmd);
      this.addProcessedCommand(cmd);
      this.m_newlyProcessedCommands.Add(cmd);
    }

    /// <summary>
    /// Collects commands from main and sim threads. This should be called in sync.
    /// </summary>
    internal void CollectCommands()
    {
      Assert.That<Lyst<IInputCommand>>(this.m_commandsToProcess).IsEmpty<IInputCommand>();
      foreach (IInputCommandFriend processedCommand in this.m_newlyProcessedCommands)
        processedCommand.Sync();
      this.m_newlyProcessedCommands.Clear();
      this.m_commandsToProcess.AddRange(this.m_mainCommands);
      this.m_mainCommands.Clear();
      this.m_commandsToProcess.AddRange(this.m_simCommands);
      this.m_simCommands.Clear();
    }

    /// <summary>
    /// Processes all commands. This should be called on the sim thread.
    /// </summary>
    public void ProcessCommands()
    {
      if (this.m_verifCmds.IsNotEmpty)
      {
        foreach (IInputCommand verifCmd in this.m_verifCmds)
          this.processCmd(verifCmd, true);
        this.m_verifCmds.Clear();
      }
      foreach (IInputCommand cmd in this.m_commandsToProcess)
      {
        Assert.That<bool>(cmd.IsVerificationCmd).IsFalse();
        this.processCmd(cmd, true);
      }
      this.m_commandsToProcess.Clear();
      while (this.m_replayCommandsToProcess.IsNotEmpty && this.m_replayCommandsToProcess.First.ProcessedAtStep <= this.m_simLoopEvents.CurrentStep)
      {
        Assert.That<SimStep>(this.m_replayCommandsToProcess.First.ProcessedAtStep).IsEqualTo(this.m_simLoopEvents.CurrentStep, "Replay command out or order. It is marked to be processed at step " + string.Format("{0} but currently sim is at step ", (object) this.m_replayCommandsToProcess.First.ProcessedAtStep) + string.Format("{0}.", (object) this.m_simLoopEvents.CurrentStep));
        IInputCommand cmd = this.m_replayCommandsToProcess.Dequeue();
        if (this.m_replayCommandsToProcess.IsEmpty)
          this.m_replayCommandsToProcess.TrimExcess();
        object resultObject = cmd.ResultSet ? cmd.GetResultObject() : (object) null;
        ((IInputCommandFriend) cmd).EraseProcessedInfo();
        this.processCmd(cmd, false);
        if (resultObject != null && !cmd.ResultObjectEqualTo(resultObject))
          Log.Error("Result from command " + cmd.GetType().Name + " at sim step " + string.Format("{0} is not the same as the one saved in ", (object) this.m_simLoopEvents.CurrentStep) + string.Format("replay cmd. Actual: {0} Expected: {1}", cmd.GetResultObject(), resultObject));
      }
    }

    private void processCmd(IInputCommand cmd, bool saveVerificationCmds)
    {
      try
      {
        this.m_resolver.InvokeActionHierarchy((object) cmd);
        ((IInputCommandFriend) cmd).MarkProcessed(this.m_simLoopEvents.CurrentStep);
        this.m_onCommandProcessed.Invoke(cmd);
      }
      catch (DependencyResolverException ex)
      {
        Log.Exception((Exception) ex, string.Format("Resolve failed when processing command: {0}. ", (object) cmd.GetType()) + "Make sure that command processor is registered for this command.");
        ((IInputCommandFriend) cmd).MarkFailed(this.m_simLoopEvents.CurrentStep);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, string.Format("Thrown when processing command: {0}", (object) cmd.GetType()));
        ((IInputCommandFriend) cmd).MarkFailed(this.m_simLoopEvents.CurrentStep);
      }
      if (!saveVerificationCmds && cmd.IsVerificationCmd)
        return;
      this.addProcessedCommand(cmd);
      this.m_newlyProcessedCommands.Add(cmd);
    }

    public void AddReplayCommands(IEnumerable<IInputCommand> commands)
    {
      Assert.That<SimStep>(this.m_simLoopEvents.CurrentStep).IsZero();
      if (this.m_replayCommandsToProcess.IsNotEmpty)
        Log.Error("Replay commands were already set.");
      else
        this.m_replayCommandsToProcess.EnqueueRange(commands);
    }

    private void addProcessedCommand(IInputCommand command)
    {
      if (!command.AffectsSaveState)
        return;
      ++this.ProcessedCommandsInThisSessionAffectingSave;
    }

    public static void Serialize(InputScheduler value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<InputScheduler>(value))
        return;
      writer.EnqueueDataSerialization((object) value, InputScheduler.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Lyst<IInputCommand>.Serialize(this.m_commandsToProcess, writer);
      Lyst<IInputCommand>.Serialize(this.m_mainCommands, writer);
      Lyst<IInputCommand>.Serialize(this.m_newlyProcessedCommands, writer);
      Event<IInputCommand>.Serialize(this.m_onCommandProcessed, writer);
      Event<IInputCommand>.Serialize(this.m_onCommandScheduled, writer);
      Queueue<IInputCommand>.Serialize(this.m_replayCommandsToProcess, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      Lyst<IInputCommand>.Serialize(this.m_simCommands, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      Lyst<IInputCommand>.Serialize(this.m_verifCmds, writer);
    }

    public static InputScheduler Deserialize(BlobReader reader)
    {
      InputScheduler inputScheduler;
      if (reader.TryStartClassDeserialization<InputScheduler>(out inputScheduler))
        reader.EnqueueDataDeserialization((object) inputScheduler, InputScheduler.s_deserializeDataDelayedAction);
      return inputScheduler;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<InputScheduler>(this, "m_commandsToProcess", (object) Lyst<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_mainCommands", (object) Lyst<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_newlyProcessedCommands", (object) Lyst<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_onCommandProcessed", (object) Event<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_onCommandScheduled", (object) Event<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_replayCommandsToProcess", (object) Queueue<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_simCommands", (object) Lyst<IInputCommand>.Deserialize(reader));
      reader.SetField<InputScheduler>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<InputScheduler>(this, "m_verifCmds", (object) Lyst<IInputCommand>.Deserialize(reader));
    }

    static InputScheduler()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      InputScheduler.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputScheduler) obj).SerializeData(writer));
      InputScheduler.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputScheduler) obj).DeserializeData(reader));
    }
  }
}
