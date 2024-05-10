// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.GameSpeedCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal sealed class GameSpeedCommandsProcessor : 
    ICommandProcessor<SetSimPauseStateCmd>,
    IAction<SetSimPauseStateCmd>,
    ICommandProcessor<GameSpeedChangeCmd>,
    IAction<GameSpeedChangeCmd>
  {
    private readonly SimLoopEvents m_simLoopEvents;

    public GameSpeedCommandsProcessor(SimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simLoopEvents = simLoopEvents;
    }

    void IAction<SetSimPauseStateCmd>.Invoke(SetSimPauseStateCmd cmd)
    {
      this.m_simLoopEvents.SetSimPause(cmd.IsPaused);
      cmd.SetResultSuccess();
    }

    void IAction<GameSpeedChangeCmd>.Invoke(GameSpeedChangeCmd cmd)
    {
      this.m_simLoopEvents.SetSimSpeed(cmd.NewSpeedMultiplier);
      cmd.SetResultSuccess();
    }
  }
}
