// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowerCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ForestryTowerCommandsProcessor : 
    ICommandProcessor<ForestryTowerAreaChangeCmd>,
    IAction<ForestryTowerAreaChangeCmd>,
    ICommandProcessor<ForestryTowerSetCutPercentageCmd>,
    IAction<ForestryTowerSetCutPercentageCmd>,
    ICommandProcessor<ForestryTowerSetTreeProtoCmd>,
    IAction<ForestryTowerSetTreeProtoCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly EntitiesManager m_entitiesManager;

    public ForestryTowerCommandsProcessor(ProtosDb protosDb, EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_entitiesManager = entitiesManager;
    }

    public void Invoke(ForestryTowerAreaChangeCmd cmd)
    {
      ForestryTower entity;
      if (!this.m_entitiesManager.TryGetEntity<ForestryTower>(cmd.ForestryTowerId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to get forestry tower with ID {0}.", (object) cmd.ForestryTowerId));
      }
      else
      {
        entity.SetNewArea(cmd.Area);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ForestryTowerSetCutPercentageCmd cmd)
    {
      ForestryTower entity;
      if (!this.m_entitiesManager.TryGetEntity<ForestryTower>(cmd.ForestryTowerId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to get forestry tower with ID {0}.", (object) cmd.ForestryTowerId));
      }
      else
      {
        entity.SetCutAtPercentage(cmd.CutPercent);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ForestryTowerSetTreeProtoCmd cmd)
    {
      ForestryTower entity;
      if (!this.m_entitiesManager.TryGetEntity<ForestryTower>(cmd.ForestryTowerId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to get forestry tower with ID {0}.", (object) cmd.ForestryTowerId));
      }
      else
      {
        TreePlantingGroupProto proto;
        if (!this.m_protosDb.TryGetProto<TreePlantingGroupProto>(cmd.GroupProtoId, out proto))
        {
          cmd.SetResultError(string.Format("Failed to get product {0}.", (object) cmd.GroupProtoId));
        }
        else
        {
          if (entity.TreeTypes.IsNotEmpty<KeyValuePair<TreePlantingGroupProto, int>>())
            entity.RemoveTreeType(entity.TreeTypes.First<KeyValuePair<TreePlantingGroupProto, int>>().Key);
          entity.AddTreeType(proto);
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
