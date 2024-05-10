// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Console.Commands.GeneralUiCommands
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Console.Commands
{
  /// <summary>
  /// Place for writing debug/testing console commands or commands that do not belong elsewhere. Public methods are
  /// automatically registered as commands.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class GeneralUiCommands
  {
    private readonly IGameConsole m_console;
    private readonly GameConsoleCommandsExecutor m_simulationCommands;

    public GeneralUiCommands(IGameConsole console, GameConsoleCommandsExecutor simulationCommands)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_console = console;
      this.m_simulationCommands = simulationCommands;
    }

    [ConsoleCommand(true, false, null, null)]
    private void help()
    {
      foreach (GameCommand gameCommand in (IEnumerable<GameCommand>) this.m_simulationCommands.Executor.Commands.Where<KeyValuePair<string, GameCommand>>((Func<KeyValuePair<string, GameCommand>, bool>) (x => x.Key == x.Value.CanonicalName)).Select<KeyValuePair<string, GameCommand>, GameCommand>((Func<KeyValuePair<string, GameCommand>, GameCommand>) (x => x.Value)).OrderBy<GameCommand, string>((Func<GameCommand, string>) (x => x.CanonicalName)))
      {
        this.m_console.WriteLine(gameCommand.CanonicalName + "    " + gameCommand.ParametersDocStr);
        if (!string.IsNullOrEmpty(gameCommand.Documentation))
          this.m_console.WriteLine("    " + gameCommand.Documentation);
      }
    }
  }
}
