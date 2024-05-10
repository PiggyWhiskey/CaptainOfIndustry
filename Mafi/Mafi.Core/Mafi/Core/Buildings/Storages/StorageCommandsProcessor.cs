// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageCommandsProcessor
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
namespace Mafi.Core.Buildings.Storages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class StorageCommandsProcessor : 
    ICommandProcessor<StorageSetSliderStepCmd>,
    IAction<StorageSetSliderStepCmd>,
    ICommandProcessor<StorageSetProductCmd>,
    IAction<StorageSetProductCmd>,
    ICommandProcessor<StorageClearProductCmd>,
    IAction<StorageClearProductCmd>,
    ICommandProcessor<StorageCheatProductCmd>,
    IAction<StorageCheatProductCmd>,
    ICommandProcessor<StorageToggleGodModeCmd>,
    IAction<StorageToggleGodModeCmd>,
    ICommandProcessor<StorageCheatClearProductCmd>,
    IAction<StorageCheatClearProductCmd>,
    ICommandProcessor<StorageDisableLogisticsToggleCmd>,
    IAction<StorageDisableLogisticsToggleCmd>,
    ICommandProcessor<StorageAlertSetThresholdCmd>,
    IAction<StorageAlertSetThresholdCmd>,
    ICommandProcessor<StorageAlertSetEnabledCmd>,
    IAction<StorageAlertSetEnabledCmd>,
    ICommandProcessor<ToggleEnforceAssignedVehiclesForEntityCmd>,
    IAction<ToggleEnforceAssignedVehiclesForEntityCmd>,
    ICommandProcessor<StorageQuickRemoveProductCmd>,
    IAction<StorageQuickRemoveProductCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public StorageCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(StorageSetSliderStepCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        if (cmd.ImportStep >= 0)
          entity.SetImportStep(cmd.ImportStep);
        if (cmd.ExportStep >= 0)
          entity.SetExportStep(cmd.ExportStep);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(StorageSetProductCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product proto '{0}' was not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.AssignProduct(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(StorageClearProductCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.ToggleClearProduct();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(StorageCheatProductCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Product proto '{0}' was not found.", (object) cmd.ProductId));
        }
        else
        {
          entity.Cheat_NewProduct(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(StorageCheatClearProductCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.Cheat_ForceClear();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(StorageToggleGodModeCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.Cheat_SetGodMode(!entity.IsGodModeEnabled);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(StorageDisableLogisticsToggleCmd cmd)
    {
      IEntityWithSimpleLogisticsControl entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityWithSimpleLogisticsControl>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Entity '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        if (cmd.IsInput)
          entity.SetLogisticsInputDisabled(cmd.IsDisabled);
        else
          entity.SetLogisticsOutputDisabled(cmd.IsDisabled);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ToggleEnforceAssignedVehiclesForEntityCmd cmd)
    {
      IEntityEnforcingAssignedVehicles entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityEnforcingAssignedVehicles>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Entity '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.SetEnforceAssignedVehicles(cmd.IsEnforced);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(StorageAlertSetThresholdCmd cmd)
    {
      if (cmd.IsAbove)
      {
        IEntityWithAlertAbove entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityWithAlertAbove>(cmd.StorageId, out entity))
        {
          cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
          return;
        }
        entity.SetAlertAbove(cmd.Value);
      }
      else
      {
        IEntityWithAlertBelow entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityWithAlertBelow>(cmd.StorageId, out entity))
        {
          cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
          return;
        }
        entity.SetAlertBelow(cmd.Value);
      }
      cmd.SetResultSuccess();
    }

    public void Invoke(StorageAlertSetEnabledCmd cmd)
    {
      if (cmd.IsAbove)
      {
        IEntityWithAlertAbove entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityWithAlertAbove>(cmd.StorageId, out entity))
        {
          cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
          return;
        }
        entity.SetAlertAboveEnabled(cmd.IsEnabled);
      }
      else
      {
        IEntityWithAlertBelow entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityWithAlertBelow>(cmd.StorageId, out entity))
        {
          cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
          return;
        }
        entity.SetAlertBelowEnabled(cmd.IsEnabled);
      }
      cmd.SetResultSuccess();
    }

    public void Invoke(StorageQuickRemoveProductCmd cmd)
    {
      Storage entity;
      if (!this.m_entitiesManager.TryGetEntity<Storage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.QuickRemoveProduct();
        cmd.SetResultSuccess();
      }
    }
  }
}
