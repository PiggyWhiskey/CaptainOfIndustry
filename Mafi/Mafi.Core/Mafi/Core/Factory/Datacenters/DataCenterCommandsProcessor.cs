// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenterCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Datacenters
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class DataCenterCommandsProcessor : 
    ICommandProcessor<DataCenterToggleRackCmd>,
    IAction<DataCenterToggleRackCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public DataCenterCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(DataCenterToggleRackCmd cmd)
    {
      Option<ServerRackProto> orLog = this.m_protosDb.GetOrLog<ServerRackProto>(cmd.ServerRackId);
      Option<DataCenter> entityOrLog = this.m_entitiesManager.GetEntityOrLog<DataCenter>(cmd.DataCenterId);
      if (orLog.IsNone || entityOrLog.IsNone)
        cmd.SetResultError("Invalid server or datacenter rack.");
      else if (cmd.Difference > 0)
      {
        int num = 0;
        while (num < cmd.Difference && entityOrLog.Value.TryAddServerRack(orLog.Value))
          ++num;
        cmd.SetResultSuccess();
      }
      else if (cmd.Difference < 0)
      {
        int num = 0;
        while (num < cmd.Difference.Abs() && entityOrLog.Value.TryRemoveServerRack(orLog.Value))
          ++num;
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError("Failed to add / remove rack.");
    }
  }
}
