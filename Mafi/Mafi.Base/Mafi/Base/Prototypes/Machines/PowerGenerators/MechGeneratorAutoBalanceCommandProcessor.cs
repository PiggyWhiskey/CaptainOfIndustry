// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.MechGeneratorAutoBalanceCommandProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MechGeneratorAutoBalanceCommandProcessor : 
    ICommandProcessor<ToggleMechGeneratorAutoBalanceCmd>,
    IAction<ToggleMechGeneratorAutoBalanceCmd>
  {
    private readonly IEntitiesManager m_entitiesManager;

    public MechGeneratorAutoBalanceCommandProcessor(IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    public void Invoke(ToggleMechGeneratorAutoBalanceCmd cmd)
    {
      MechPowerGeneratorFromProduct entity;
      if (this.m_entitiesManager.TryGetEntity<MechPowerGeneratorFromProduct>(cmd.EntityId, out entity))
      {
        entity.AutoBalance = !entity.AutoBalance;
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(string.Format("Failed to find entity '{0}' as '{1}'.", (object) cmd.EntityId, (object) "MechPowerGeneratorFromProduct"));
    }
  }
}
