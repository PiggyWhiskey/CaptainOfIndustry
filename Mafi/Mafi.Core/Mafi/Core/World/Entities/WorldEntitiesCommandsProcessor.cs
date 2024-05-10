// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldEntitiesCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.World.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class WorldEntitiesCommandsProcessor : 
    ICommandProcessor<SetShiftsCountForMineCmd>,
    IAction<SetShiftsCountForMineCmd>
  {
    private readonly EntitiesManager m_entitiesManager;

    public WorldEntitiesCommandsProcessor(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    public void Invoke(SetShiftsCountForMineCmd cmd)
    {
      WorldMapMine entity;
      if (!this.m_entitiesManager.TryGetEntity<WorldMapMine>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown entity '{0}'", (object) cmd.EntityId));
      }
      else
      {
        entity.SetProductionStep(cmd.ShiftsCount);
        cmd.SetResultSuccess();
      }
    }
  }
}
