// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.TradeDockCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TradeDockCommandsProcessor : 
    ICommandProcessor<TradeDockToggleUnloadPriorityCmd>,
    IAction<TradeDockToggleUnloadPriorityCmd>
  {
    private readonly EntitiesManager m_entitiesManager;

    public TradeDockCommandsProcessor(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    void IAction<TradeDockToggleUnloadPriorityCmd>.Invoke(TradeDockToggleUnloadPriorityCmd cmd)
    {
      TradeDock entity;
      if (!this.m_entitiesManager.TryGetEntity<TradeDock>(cmd.TradeDockId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to get trade dock with ID {0}.", (object) cmd.TradeDockId));
      }
      else
      {
        entity.HasHighCargoUnloadPrio = !entity.HasHighCargoUnloadPrio;
        cmd.SetResultSuccess();
      }
    }
  }
}
