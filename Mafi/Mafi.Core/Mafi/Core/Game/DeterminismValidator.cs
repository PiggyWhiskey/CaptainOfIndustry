// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.DeterminismValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.SaveGame;
using Mafi.Core.Simulation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace Mafi.Core.Game
{
  internal class DeterminismValidator
  {
    private readonly IDeterminismValidatorConfig m_config;
    private readonly DependencyResolver m_resolver;
    private readonly GameRunner m_runner;
    private readonly InputScheduler m_inputScheduler;
    private readonly SaveManager m_saveManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ISimLoopEvents m_simLoopEvents;
    private DependencyResolver m_validationGameResolver;
    private GameRunner m_validationGameRunner;
    private IInputScheduler m_validationInputScheduler;
    private SaveManager m_validationSaveManager;
    private ISimLoopEvents m_validationSimLoopEvents;
    private int m_checkedStep;
    private readonly Queueue<KeyValuePair<IInputCommand, IInputCommand>> m_commandsToValidate;
    private readonly Stopwatch m_stopwatch;
    private double m_lastSaveDurationMs;
    private int m_lastSaveSize;

    private bool IsActive => this.m_validationGameResolver != null;

    public DeterminismValidator(
      IDeterminismValidatorConfig config,
      DependencyResolver resolver,
      GameRunner runner,
      InputScheduler inputScheduler,
      SaveManager saveManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_checkedStep = -1;
      this.m_commandsToValidate = new Queueue<KeyValuePair<IInputCommand, IInputCommand>>();
      this.m_stopwatch = new Stopwatch();
      this.m_lastSaveSize = 1048576;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (!config.DeterminismValidationEnabled)
        return;
      this.m_config = config;
      this.m_resolver = resolver;
      this.m_runner = runner;
      this.m_inputScheduler = inputScheduler;
      this.m_saveManager = saveManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_simLoopEvents = simLoopEvents;
      this.m_gameLoopEvents.RegisterNewGameCreated((object) this, this.newGameCreated());
      this.m_gameLoopEvents.Terminate.AddNonSaveable<DeterminismValidator>(this, new Action(this.disableFurtherValidation));
      this.m_runner.OnUpdateStart.AddNonSaveable<DeterminismValidator>(this, new Action<Fix32, bool>(this.runnerUpdateStart));
      if (this.m_config.DeterminismDisableCommandsForwarding)
        return;
      this.m_inputScheduler.OnCommandScheduled.AddNonSaveable<DeterminismValidator>(this, new Action<IInputCommand>(this.commandScheduled));
    }

    private void runnerUpdateStart(Fix32 dt, bool willDoSimUpdate)
    {
      Mafi.Assert.That<bool>(this.IsActive).IsTrue();
      this.verifyStateIfNeeded(willDoSimUpdate);
      if (!this.IsActive)
        return;
      this.m_validationGameRunner.Update(dt);
    }

    private void verifyStateIfNeeded(bool willDoSimUpdate)
    {
      Mafi.Assert.That<bool>(this.IsActive).IsTrue();
      if (!willDoSimUpdate)
        return;
      Mafi.Assert.That<int>(this.m_checkedStep).IsNotEqualTo(this.m_simLoopEvents.CurrentStep.Value);
      this.validateCmds();
      if (this.m_checkedStep != -1 && this.m_config.DeterminismValidationFrequencySteps > Duration.OneTick && this.m_simLoopEvents.CurrentStep.Value % this.m_config.DeterminismValidationFrequencySteps.Ticks != 0)
        return;
      this.m_runner.BlockUntilSimStepDone();
      this.m_validationGameRunner.BlockUntilSimStepDone();
      this.verifyState();
      this.m_checkedStep = this.m_simLoopEvents.CurrentStep.Value;
    }

    private void validateCmds()
    {
      while (this.m_commandsToValidate.IsNotEmpty && this.m_commandsToValidate.First.Key.IsProcessedAndSynced)
      {
        KeyValuePair<IInputCommand, IInputCommand> keyValuePair = this.m_commandsToValidate.Dequeue();
        IInputCommand key = keyValuePair.Key;
        IInputCommand other = keyValuePair.Value;
        if (!key.ResultEqualTo(other))
        {
          Mafi.Log.Error(string.Format("Determinism validation FAIL at sim step {0}. ", (object) this.m_simLoopEvents.CurrentStep) + "Result of command `" + key.GetType().Name + "` is not matching:\n" + string.Format("Curr data: {0}\n", key.GetResultObject()) + string.Format("Verif data: {0}", key.GetResultObject()));
          this.disableFurtherValidation();
          break;
        }
      }
    }

    private void commandScheduled(IInputCommand cmd)
    {
      Mafi.Assert.That<bool>(this.m_config.DeterminismDisableCommandsForwarding).IsFalse();
      if (cmd.IsVerificationCmd)
        return;
      Mafi.Assert.That<bool>(this.IsActive).IsTrue();
      IInputCommand cmd1 = cmd.ShallowCloneWithoutResult();
      this.m_commandsToValidate.Enqueue(Make.Kvp<IInputCommand, IInputCommand>(cmd, cmd1));
      this.m_validationInputScheduler.ScheduleInputCmd<IInputCommand>(cmd1);
    }

    private void disableFurtherValidation()
    {
      if (!this.IsActive)
        return;
      this.m_runner.OnUpdateStart.RemoveNonSaveable<DeterminismValidator>(this, new Action<Fix32, bool>(this.runnerUpdateStart));
      if (!this.m_config.DeterminismDisableCommandsForwarding)
        this.m_inputScheduler.OnCommandScheduled.RemoveNonSaveable<DeterminismValidator>(this, new Action<IInputCommand>(this.commandScheduled));
      Mafi.Log.Info("Terminating determinism validation game.");
      this.m_validationGameRunner.Terminate();
      this.m_validationGameResolver.TerminateAndClear();
      this.m_validationGameResolver = (DependencyResolver) null;
      this.m_validationGameRunner = (GameRunner) null;
      this.m_validationInputScheduler = (IInputScheduler) null;
      this.m_validationSaveManager = (SaveManager) null;
    }

    private IEnumerator<string> newGameCreated()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new DeterminismValidator.\u003CnewGameCreated\u003Ed__25(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void verifyState()
    {
      Mafi.Assert.That<SimStep>(this.m_simLoopEvents.CurrentStep).IsEqualTo(this.m_validationSimLoopEvents.CurrentStep, "Step mismatch");
      this.m_stopwatch.Restart();
      MemoryStream memoryStream1 = new MemoryStream()
      {
        Capacity = this.m_lastSaveSize
      };
      this.m_saveManager.SaveGameRaw((Stream) memoryStream1);
      memoryStream1.Seek(0L, SeekOrigin.Begin);
      this.m_lastSaveSize = (int) (memoryStream1.Length + 1024L);
      MemoryStream memoryStream2 = new MemoryStream()
      {
        Capacity = this.m_lastSaveSize
      };
      this.m_validationSaveManager.SaveGameRaw((Stream) memoryStream2);
      memoryStream2.Seek(0L, SeekOrigin.Begin);
      this.m_stopwatch.Stop();
      this.m_lastSaveDurationMs = this.m_stopwatch.Elapsed.TotalMilliseconds;
      bool flag = memoryStream1.Length == memoryStream2.Length;
      if (flag)
      {
        byte[] buffer1 = new byte[4096];
        byte[] buffer2 = new byte[buffer1.Length];
        for (int index1 = 0; flag && (long) index1 < memoryStream1.Length; ++index1)
        {
          int num = memoryStream1.Read(buffer1, 0, buffer1.Length);
          int other = memoryStream2.Read(buffer2, 0, buffer2.Length);
          Mafi.Assert.That<int>(num).IsEqualTo(other);
          for (int index2 = 0; index2 < num; ++index2)
          {
            if ((int) buffer1[index2] != (int) buffer2[index2])
            {
              flag = false;
              break;
            }
          }
        }
      }
      if (flag)
        return;
      bool currGameDone = false;
      AutoResetEvent currGameDoneAre = new AutoResetEvent(false);
      AutoResetEvent continueAutoResetEvent1 = new AutoResetEvent(false);
      DeterminismValidator.ValidationStream currGameStream = new DeterminismValidator.ValidationStream(currGameDoneAre, continueAutoResetEvent1);
      Thread thread1 = new Thread((ThreadStart) (() =>
      {
        ThreadAssert.Disable();
        this.m_saveManager.SaveGameRaw((Stream) currGameStream);
        currGameDone = true;
        currGameDoneAre.Set();
      }))
      {
        Name = "DV: Current game"
      };
      thread1.Start();
      bool verifGameDone = false;
      AutoResetEvent verifGameDoneAre = new AutoResetEvent(false);
      AutoResetEvent continueAutoResetEvent2 = new AutoResetEvent(false);
      DeterminismValidator.ValidationStream verifGameStream = new DeterminismValidator.ValidationStream(verifGameDoneAre, continueAutoResetEvent2);
      Thread thread2 = new Thread((ThreadStart) (() =>
      {
        ThreadAssert.Disable();
        this.m_validationSaveManager.SaveGameRaw((Stream) verifGameStream);
        verifGameDone = true;
        verifGameDoneAre.Set();
      }))
      {
        Name = "DV: Verif game"
      };
      thread2.Start();
      while (!currGameDone && !verifGameDone)
      {
        currGameDoneAre.WaitOne();
        verifGameDoneAre.WaitOne();
        if (this.verifyData(currGameStream, verifGameStream))
        {
          continueAutoResetEvent1.Set();
          continueAutoResetEvent2.Set();
        }
        else
          break;
      }
      Mafi.Assert.That<bool>(currGameDone).IsEqualTo<bool>(verifGameDone);
      currGameStream.SkipToEnd();
      verifGameStream.SkipToEnd();
      continueAutoResetEvent1.Set();
      continueAutoResetEvent2.Set();
      thread1.Join();
      thread2.Join();
      Mafi.Assert.That<bool>(currGameDone).IsTrue();
      Mafi.Assert.That<bool>(verifGameDone).IsTrue();
      this.disableFurtherValidation();
    }

    private bool verifyData(
      DeterminismValidator.ValidationStream currStream,
      DeterminismValidator.ValidationStream verifStream)
    {
      Mafi.Assert.That<long>(currStream.Position).IsEqualTo(verifStream.Position);
      if (currStream.ReceivedBufferLength != verifStream.ReceivedBufferLength)
      {
        Mafi.Log.Error(string.Format("Determinism validation FAIL at sim step {0} at stream position ", (object) this.m_simLoopEvents.CurrentStep) + string.Format("{0}: Received data of length {1}, expected ", (object) currStream.Position, (object) currStream.ReceivedBufferLength) + string.Format("{0}.\n", (object) verifStream.ReceivedBufferLength) + "Curr data: " + printBuff(currStream) + "\nVerif data: " + printBuff(verifStream));
        return false;
      }
      int receivedBufferLength = currStream.ReceivedBufferLength;
      for (int index = 0; index < receivedBufferLength; ++index)
      {
        byte num1 = currStream.ReceivedBuffer[index + currStream.ReceivedBufferOffset];
        byte num2 = verifStream.ReceivedBuffer[index + verifStream.ReceivedBufferOffset];
        if ((int) num1 != (int) num2)
        {
          Mafi.Log.Error(string.Format("Determinism validation FAIL at sim step {0} at stream position ", (object) this.m_simLoopEvents.CurrentStep) + string.Format("{0}: Byte {1} of current write op was {2}, expected {3}.\n", (object) currStream.Position, (object) index, (object) num1, (object) num2) + "Curr data: " + printBuff(currStream) + "\nVerif data: " + printBuff(verifStream));
          return false;
        }
      }
      return true;

      static string printBuff(DeterminismValidator.ValidationStream s)
      {
        return BitConverter.ToString(s.ReceivedBuffer, s.ReceivedBufferOffset, s.ReceivedBufferLength);
      }
    }

    private class ValidationStream : Stream
    {
      private long m_position;
      private bool m_skipToEnd;
      private readonly AutoResetEvent m_doneAutoResetEvent;
      private readonly AutoResetEvent m_continueAutoResetEvent;
      public byte[] ReceivedBuffer;
      public int ReceivedBufferOffset;
      public int ReceivedBufferLength;

      public override bool CanRead => false;

      public override bool CanSeek => false;

      public override bool CanWrite => true;

      public override long Length => this.Position;

      public override long Position
      {
        get => this.m_position;
        set => throw new InvalidOperationException("Verification stream cannot change position.");
      }

      public ValidationStream(
        AutoResetEvent doneAutoResetEvent,
        AutoResetEvent continueAutoResetEvent)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_doneAutoResetEvent = doneAutoResetEvent;
        this.m_continueAutoResetEvent = continueAutoResetEvent;
      }

      public void SkipToEnd() => this.m_skipToEnd = true;

      public override void Flush()
      {
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
        throw new InvalidOperationException("Verification stream cannot seek.");
      }

      public override void SetLength(long value)
      {
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
        throw new InvalidOperationException("Verification stream is write-only.");
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        if (this.m_skipToEnd)
          return;
        this.ReceivedBuffer = buffer;
        this.ReceivedBufferOffset = offset;
        this.ReceivedBufferLength = count;
        this.m_position += (long) count;
        this.m_doneAutoResetEvent.Set();
        this.m_continueAutoResetEvent.WaitOne();
      }
    }
  }
}
