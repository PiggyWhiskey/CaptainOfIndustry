// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RuinedBuildings.RuinsCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.Buildings.RuinedBuildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RuinsCommandsProcessor : ICommandProcessor<RuinsScrapCmd>, IAction<RuinsScrapCmd>
  {
    private readonly EntitiesManager m_entitiesManager;

    public RuinsCommandsProcessor(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    private bool tryGetRuins(EntityId id, out Ruins ruins, out string error)
    {
      if (id.IsValid)
      {
        if (!this.m_entitiesManager.TryGetEntity<Ruins>(id, out ruins))
        {
          error = string.Format("Failed to get ruins with ID {0}.", (object) id);
          ruins = (Ruins) null;
          return false;
        }
      }
      else
      {
        Lyst<Ruins> lyst = this.m_entitiesManager.GetAllEntitiesOfType<Ruins>().ToLyst<Ruins>();
        if (lyst.Count != 1)
        {
          error = string.Format("Failed to find single ruins in the game, count: {0}", (object) lyst.Count);
          ruins = (Ruins) null;
          return false;
        }
        ruins = lyst.First;
      }
      error = "";
      return true;
    }

    void IAction<RuinsScrapCmd>.Invoke(RuinsScrapCmd cmd)
    {
      Ruins ruins;
      string error;
      if (this.tryGetRuins(cmd.RuinsId, out ruins, out error))
      {
        ruins.SetScrapEnabled(cmd.IsScrapping);
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }
  }
}
