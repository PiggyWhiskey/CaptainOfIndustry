// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.ReplayManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Serialization;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Mafi.Core.Input
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class ReplayManager
  {
    private readonly IInputScheduler m_inputScheduler;
    [DoNotSave(0, null)]
    private Option<StreamWriter> m_commandsLog;
    private StringBuilder m_sb;
    private CSharpGen m_csGen;

    public ReplayManager(IInputScheduler inputScheduler, IGameLoopEvents gameLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputScheduler = inputScheduler;
      gameLoopEvents.Terminate.AddNonSaveable<ReplayManager>(this, (Action) (() => this.m_commandsLog.ValueOrNull?.Close()));
    }

    /// <summary>
    /// Starts logging commands as C# to given stream. This class takes the stream ownership and closes it on game
    /// termination.
    /// </summary>
    public void StartLoggingCommandsAsCSharpTo(StreamWriter sw, DependencyResolver resolver)
    {
    }

    private void logCommand(IInputCommand cmd)
    {
      if (this.m_commandsLog.IsNone)
        return;
      StreamWriter streamWriter = this.m_commandsLog.Value;
      if (!streamWriter.BaseStream.CanWrite)
      {
        Log.Warning("Stream is not writable. Stopping commands logging.");
        this.m_commandsLog = Option<StreamWriter>.None;
      }
      else
      {
        this.m_sb.Clear();
        ReplayManager.genCommandCode(cmd, this.m_csGen, this.m_sb);
        streamWriter.Write(this.m_sb.ToString());
        streamWriter.Flush();
      }
    }

    private static void genConfigsCode(
      CSharpGen csGen,
      DependencyResolver resolver,
      StringBuilder sb)
    {
      sb.AppendLine("// All mod configs.");
      foreach (IConfig implementation in resolver.ResolveAll<IConfig>().Implementations)
      {
        csGen.GenerateCsharpFor((object) implementation, sb, setAllPublicProperties: true);
        sb.AppendLine(",");
        sb.AppendLine();
      }
    }

    private static void genCommandCode(IInputCommand cmd, CSharpGen csGen, StringBuilder sb)
    {
      csGen.GenerateCsharpFor((object) cmd, sb);
      sb.AppendLine();
      sb.Append("\t.InitializeAsProcessed(");
      csGen.GenerateCsharpFor((object) cmd.ProcessedAtStep, sb);
      sb.Append(", ");
      csGen.GenerateCsharpFor(cmd.GetResultObject(), sb);
      sb.Append(", ");
      csGen.GenerateCsharpFor((object) cmd.ErrorMessage, sb);
      sb.AppendLine("),");
    }
  }
}
