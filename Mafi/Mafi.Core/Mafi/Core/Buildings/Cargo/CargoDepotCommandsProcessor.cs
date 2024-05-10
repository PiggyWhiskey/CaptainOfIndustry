// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepotCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Contracts;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoDepotCommandsProcessor : 
    ICommandProcessor<CargoDepotModuleClearProductCmd>,
    IAction<CargoDepotModuleClearProductCmd>,
    ICommandProcessor<CargoDepotCheatFullFuelCmd>,
    IAction<CargoDepotCheatFullFuelCmd>,
    ICommandProcessor<CargoDepotSetFuelSliderStepCmd>,
    IAction<CargoDepotSetFuelSliderStepCmd>,
    ICommandProcessor<CargoDepotModuleSetProductCmd>,
    IAction<CargoDepotModuleSetProductCmd>,
    ICommandProcessor<CargoDepotAssignContractCmd>,
    IAction<CargoDepotAssignContractCmd>,
    ICommandProcessor<CargoShipDepartNowCmd>,
    IAction<CargoShipDepartNowCmd>,
    ICommandProcessor<CargoShipSetFuelSaverCmd>,
    IAction<CargoShipSetFuelSaverCmd>,
    ICommandProcessor<CargoShipPayWithUnityIfOutOfFuelCmd>,
    IAction<CargoShipPayWithUnityIfOutOfFuelCmd>,
    ICommandProcessor<CargoShipReplaceFuelCmd>,
    IAction<CargoShipReplaceFuelCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;
    private readonly IProductsManager m_productsManager;
    private readonly ContractsManager m_contractsManager;

    public CargoDepotCommandsProcessor(
      EntitiesManager entitiesManager,
      ProtosDb protosDb,
      IProductsManager productsManager,
      ContractsManager contractsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
      this.m_productsManager = productsManager;
      this.m_contractsManager = contractsManager;
    }

    public void Invoke(CargoDepotModuleClearProductCmd cmd)
    {
      CargoDepotModule entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoDepotModule>(cmd.ModuleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo depot module with id {0}.", (object) cmd.ModuleId));
      }
      else
      {
        entity.ClearAssignedProduct();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoDepotCheatFullFuelCmd cmd)
    {
      Option<CargoDepot> entity = this.m_entitiesManager.GetEntity<CargoDepot>(cmd.CargoDepotId);
      if (entity.IsNone)
      {
        cmd.SetResultError(string.Format("Failed to find cargo depot with id {0}.", (object) cmd.CargoDepotId));
      }
      else
      {
        CargoDepot cargoDepot = entity.Value;
        Quantity usableCapacity = cargoDepot.FuelBuffer.UsableCapacity;
        cargoDepot.Cheat_FuelExactly(usableCapacity);
        this.m_productsManager.ProductCreated(cargoDepot.FuelBuffer.Product, usableCapacity, CreateReason.Cheated);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoDepotSetFuelSliderStepCmd cmd)
    {
      CargoDepot entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoDepot>(cmd.CargoDepotId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo depot with id {0}.", (object) cmd.CargoDepotId));
      }
      else
      {
        entity.UpdateFuelImportExportStep(cmd.ImportStep, cmd.ExportStep);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoDepotModuleSetProductCmd cmd)
    {
      CargoDepotModule entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoDepotModule>(cmd.ModuleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo depot module with id {0}.", (object) cmd.ModuleId));
      }
      else
      {
        Option<ProductProto> option = this.m_protosDb.Get<ProductProto>((Proto.ID) cmd.ProductId);
        if (option.IsNone)
          cmd.SetResultError(string.Format("Failed to find product with id {0}.", (object) cmd.ProductId));
        else if (entity.AssignProduct(option.Value))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError("Failed to assign product.");
      }
    }

    public void Invoke(CargoDepotAssignContractCmd cmd)
    {
      CargoDepot entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoDepot>(cmd.CargoDepotId, out entity))
        cmd.SetResultError(string.Format("Failed to find cargo depot with id {0}.", (object) cmd.CargoDepotId));
      else if (!cmd.ContractId.HasValue)
      {
        entity.ClearContract();
        cmd.SetResultSuccess();
      }
      else
      {
        ContractProto proto;
        if (!this.m_protosDb.TryGetProto<ContractProto>(cmd.ContractId.Value, out proto))
          cmd.SetResultError(string.Format("Failed to find contract with id {0}.", (object) cmd.ContractId));
        else if (!this.m_contractsManager.IsEstablished(proto))
        {
          cmd.SetResultError(string.Format("Contract {0} is not established.", (object) cmd.ContractId));
        }
        else
        {
          LocStrFormatted errorReason;
          if (!entity.CanAssignContract(proto, out errorReason))
          {
            cmd.SetResultError(errorReason.Value);
          }
          else
          {
            entity.AssignContract(proto);
            cmd.SetResultSuccess();
          }
        }
      }
    }

    public void Invoke(CargoShipDepartNowCmd nowCmd)
    {
      CargoShip entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoShip>(nowCmd.CargoShipId, out entity))
      {
        nowCmd.SetResultError(string.Format("Failed to find cargo ship with id {0}.", (object) nowCmd.CargoShipId));
      }
      else
      {
        entity.RequestDeparture();
        nowCmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoShipSetFuelSaverCmd cmd)
    {
      CargoShip entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoShip>(cmd.CargoShipId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo ship with id {0}.", (object) cmd.CargoShipId));
      }
      else
      {
        entity.SetFuelReductionEnabled(cmd.IsFuelSaverEnabled);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoShipPayWithUnityIfOutOfFuelCmd cmd)
    {
      CargoShip entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoShip>(cmd.CargoShipId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo ship with id {0}.", (object) cmd.CargoShipId));
      }
      else
      {
        entity.SetPayWithUnityIfOutOfFuel(cmd.PayWithUnity);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(CargoShipReplaceFuelCmd cmd)
    {
      CargoShip entity;
      if (!this.m_entitiesManager.TryGetEntity<CargoShip>(cmd.CargoShipId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find cargo ship with id {0}.", (object) cmd.CargoShipId));
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.FuelId, out proto))
        {
          cmd.SetResultError(string.Format("Failed to find product with id {0}.", (object) cmd.FuelId));
        }
        else
        {
          entity.SetNewFuelType(proto);
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
