// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameConsoleCommandsExecutor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using System;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Console
{
  /// <summary>
  /// Handles automatic commands registration and execution via game console.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GameConsoleCommandsExecutor : 
    ICommandProcessor<GameConsoleCmd>,
    IAction<GameConsoleCmd>
  {
    public readonly GameCommandsExecutor Executor;
    private readonly IGameConsole m_console;
    private readonly DependencyResolver m_resolver;
    private readonly IInputScheduler m_inputScheduler;
    private readonly Lyst<string> m_commandsInvokedDuringSync;

    public GameConsoleCommandsExecutor(
      IGameConsole console,
      GameCommandsExecutor gameCommandsExecutor,
      IGameLoopEvents gameLoopEvents,
      DependencyResolver resolver,
      IInputScheduler inputScheduler)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_commandsInvokedDuringSync = new Lyst<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_console = console;
      this.Executor = gameCommandsExecutor;
      this.m_resolver = resolver;
      this.m_inputScheduler = inputScheduler;
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.scanAllConsoleCommands));
      gameLoopEvents.SyncUpdate.AddNonSaveable<GameConsoleCommandsExecutor>(this, new Action<GameTime>(this.syncUpdate));
    }

    private void scanAllConsoleCommands()
    {
      foreach (object resolvedInstance in this.m_resolver.AllResolvedInstances)
        this.ScanObjectForConsoleCommands(resolvedInstance);
      Log.Info(string.Format("Registered {0} console commands.", (object) this.Executor.Commands.Values.Distinct<GameCommand>().Count<GameCommand>()));
    }

    /// <summary>
    /// By default, all instantiated global dependencies are scanned. Use this function to scan additional objects
    /// for console commands.
    /// </summary>
    public void ScanObjectForConsoleCommands(object target)
    {
      foreach (MethodInfo method1 in target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        ConsoleCommandAttribute customAttribute = method1.GetCustomAttribute<ConsoleCommandAttribute>();
        if (customAttribute != null)
        {
          GameCommandsExecutor executor = this.Executor;
          object target1 = target;
          MethodInfo method2 = method1;
          string documentation = customAttribute.Documentation;
          bool invokeOnMainThread = customAttribute.InvokeOnMainThread;
          bool invokeDuringSync = customAttribute.InvokeDuringSync;
          string valueOrNull = customAttribute.CustomCommandName.ValueOrNull;
          int num1 = invokeOnMainThread ? 1 : 0;
          int num2 = invokeDuringSync ? 1 : 0;
          executor.RegisterCommand(target1, method2, documentation, valueOrNull, num1 != 0, num2 != 0);
        }
      }
    }

    private void syncUpdate(GameTime obj)
    {
      if (!this.m_commandsInvokedDuringSync.IsNotEmpty)
        return;
      foreach (string commandText in this.m_commandsInvokedDuringSync)
        this.tryExecuteToConsole(commandText);
      this.m_commandsInvokedDuringSync.Clear();
    }

    /// <summary>
    /// Executes command directly if it should run on main thread or schedules its execution on sim thread.
    /// </summary>
    public bool ExecuteOrSchedule(string commandText)
    {
      GameCommand command;
      string errorMessage;
      if (!this.Executor.TryParseCommand(commandText, out command, out errorMessage, out string _))
      {
        this.m_console.WriteError(errorMessage);
        return false;
      }
      if (command.InvokeDuringSync)
      {
        this.m_commandsInvokedDuringSync.Add(commandText);
        return true;
      }
      if (command.InvokeOnMainThread)
        return this.tryExecuteToConsole(commandText);
      this.m_inputScheduler.ScheduleInputCmd<GameConsoleCmd>(new GameConsoleCmd(commandText));
      return true;
    }

    private bool tryExecuteToConsole(string commandText)
    {
      Log.Info(commandText);
      GameCommandResult gameCommandResult = this.Executor.TryExecute(commandText);
      if (gameCommandResult.ErrorMessage.HasValue)
        this.m_console.WriteError(gameCommandResult.ErrorMessage.Value);
      if (gameCommandResult.Result.HasValue)
        this.m_console.WriteLine(string.Format(" -> {0}", gameCommandResult.Result.Value));
      return gameCommandResult.ErrorMessage.IsNone;
    }

    void IAction<GameConsoleCmd>.Invoke(GameConsoleCmd cmd)
    {
      if (this.tryExecuteToConsole(cmd.Command))
        cmd.SetResultSuccess();
      else
        cmd.SetResultError("Failed to execute command: " + cmd.Command);
    }
  }
}
