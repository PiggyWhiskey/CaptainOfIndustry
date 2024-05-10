// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.NuclearReactors
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NuclearReactorCommandsProcessor : 
    ICommandProcessor<NuclearReactorSetPowerLevelCmd>,
    IAction<NuclearReactorSetPowerLevelCmd>,
    ICommandProcessor<NuclearReactorToggleAllowedFuelCmd>,
    IAction<NuclearReactorToggleAllowedFuelCmd>,
    ICommandProcessor<NuclearReactorToggleAutomaticRegulationCmd>,
    IAction<NuclearReactorToggleAutomaticRegulationCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public NuclearReactorCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(NuclearReactorSetPowerLevelCmd cmd)
    {
      NuclearReactor entity;
      if (!this.m_entitiesManager.TryGetEntity<NuclearReactor>(cmd.ReactorId, out entity))
      {
        cmd.SetResultError(string.Format("Reactor '{0}' was not found.", (object) cmd.ReactorId));
      }
      else
      {
        entity.SetTargetPowerLevel(cmd.PowerLevel * Percent.Hundred);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(NuclearReactorToggleAllowedFuelCmd cmd)
    {
      NuclearReactor entity;
      if (!this.m_entitiesManager.TryGetEntity<NuclearReactor>(cmd.ReactorId, out entity))
      {
        cmd.SetResultError(string.Format("Reactor '{0}' was not found.", (object) cmd.ReactorId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.FuelProtoId, out proto))
        {
          cmd.SetResultError(string.Format("Fuel proto '{0}' was not found.", (object) cmd.FuelProtoId));
        }
        else
        {
          bool flag = entity.IsAllowedFuel(proto);
          if (entity.TrySetAllowedFuel(proto, !flag))
            cmd.SetResultSuccess();
          else
            cmd.SetResultError(string.Format("Failed to set allowed fuel proto '{0}'.", (object) cmd.FuelProtoId));
        }
      }
    }

    public void Invoke(NuclearReactorToggleAutomaticRegulationCmd cmd)
    {
      NuclearReactor entity;
      if (!this.m_entitiesManager.TryGetEntity<NuclearReactor>(cmd.ReactorId, out entity))
        cmd.SetResultError(string.Format("Reactor '{0}' was not found.", (object) cmd.ReactorId));
      else if (!entity.IsAutomaticPowerRegulationSupported)
      {
        cmd.SetResultError(string.Format("Reactor '{0}' does not support auto regulation.", (object) cmd.ReactorId));
      }
      else
      {
        entity.SetAutomatedRegulation(!entity.IsAutomaticPowerRegulationEnabled);
        cmd.SetResultSuccess();
      }
    }
  }
}
