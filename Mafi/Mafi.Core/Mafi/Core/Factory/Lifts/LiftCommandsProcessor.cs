// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Lifts.LiftCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Lifts
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class LiftCommandsProcessor : ICommandProcessor<ReverseLiftCmd>, IAction<ReverseLiftCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public LiftCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(ReverseLiftCmd cmd)
    {
      Option<Lift> entityOrLog = this.m_entitiesManager.GetEntityOrLog<Lift>(cmd.LiftId);
      if (entityOrLog.IsNone)
        cmd.SetResultError("Failed to find lift.");
      else if (!entityOrLog.Value.ReversePorts())
        cmd.SetResultError("Failed to reverse lift ports.");
      else
        cmd.SetResultSuccess();
    }
  }
}
