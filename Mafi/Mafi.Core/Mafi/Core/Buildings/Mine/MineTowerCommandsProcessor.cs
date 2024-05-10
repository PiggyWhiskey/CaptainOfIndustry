// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowerCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MineTowerCommandsProcessor : 
    ICommandProcessor<MineTowerAreaChangeCmd>,
    IAction<MineTowerAreaChangeCmd>,
    ICommandProcessor<AddProductToDumpCmd>,
    IAction<AddProductToDumpCmd>,
    ICommandProcessor<RemoveProductToDumpCmd>,
    IAction<RemoveProductToDumpCmd>,
    ICommandProcessor<AddProductToNotifyIfCannotDumpCmd>,
    IAction<AddProductToNotifyIfCannotDumpCmd>,
    ICommandProcessor<RemoveProductToNotifyIfCannotDumpCmd>,
    IAction<RemoveProductToNotifyIfCannotDumpCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ITerrainDumpingManager m_dumpingManager;

    public MineTowerCommandsProcessor(
      ProtosDb protosDb,
      EntitiesManager entitiesManager,
      ITerrainDumpingManager dumpingManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_entitiesManager = entitiesManager;
      this.m_dumpingManager = dumpingManager;
    }

    public void Invoke(MineTowerAreaChangeCmd cmd)
    {
      MineTower entity;
      if (!this.m_entitiesManager.TryGetEntity<MineTower>(cmd.MineTowerId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", (object) cmd.MineTowerId));
      }
      else
      {
        entity.SetNewArea(cmd.Area);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(AddProductToDumpCmd cmd)
    {
      LooseProductProto proto;
      if (!this.m_protosDb.TryGetProto<LooseProductProto>((Proto.ID) cmd.ProductId, out proto))
        cmd.SetResultError(string.Format("Failed to get product {0}.", (object) cmd.ProductId));
      else if (!cmd.MineTowerId.HasValue)
      {
        this.m_dumpingManager.AddProductToDump(proto);
        cmd.SetResultSuccess();
      }
      else
      {
        MineTower entity;
        if (!this.m_entitiesManager.TryGetEntity<MineTower>(cmd.MineTowerId.Value, out entity))
        {
          cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", (object) cmd.MineTowerId));
        }
        else
        {
          entity.AddProductToDump(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(RemoveProductToDumpCmd cmd)
    {
      LooseProductProto proto;
      if (!this.m_protosDb.TryGetProto<LooseProductProto>((Proto.ID) cmd.ProductId, out proto))
        cmd.SetResultError(string.Format("Failed to get product {0}.", (object) cmd.ProductId));
      else if (!cmd.MineTowerId.HasValue)
      {
        this.m_dumpingManager.RemoveProductToDump(proto);
        cmd.SetResultSuccess();
      }
      else
      {
        MineTower entity;
        if (!this.m_entitiesManager.TryGetEntity<MineTower>(cmd.MineTowerId.Value, out entity))
        {
          cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", (object) cmd.MineTowerId.Value));
        }
        else
        {
          entity.RemoveProductToDump(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(AddProductToNotifyIfCannotDumpCmd cmd)
    {
      LooseProductProto proto;
      if (!this.m_protosDb.TryGetProto<LooseProductProto>((Proto.ID) cmd.ProductId, out proto))
      {
        cmd.SetResultError(string.Format("Failed to get product {0}.", (object) cmd.ProductId));
      }
      else
      {
        MineTower entity;
        if (!this.m_entitiesManager.TryGetEntity<MineTower>(cmd.MineTowerId, out entity))
        {
          cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", (object) cmd.MineTowerId));
        }
        else
        {
          entity.AddProductToNotifyIfCannotGetRidOff(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(RemoveProductToNotifyIfCannotDumpCmd cmd)
    {
      LooseProductProto proto;
      if (!this.m_protosDb.TryGetProto<LooseProductProto>((Proto.ID) cmd.ProductId, out proto))
      {
        cmd.SetResultError(string.Format("Failed to get product {0}.", (object) cmd.ProductId));
      }
      else
      {
        MineTower entity;
        if (!this.m_entitiesManager.TryGetEntity<MineTower>(cmd.MineTowerId, out entity))
        {
          cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", (object) cmd.MineTowerId.Value));
        }
        else
        {
          entity.RemoveProductToNotifyIfCannotGetRidOff(proto);
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
