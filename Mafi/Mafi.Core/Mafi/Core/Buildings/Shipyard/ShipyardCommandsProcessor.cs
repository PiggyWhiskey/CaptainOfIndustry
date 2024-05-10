// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ShipyardCommandsProcessor : 
    ICommandProcessor<ShipyardSetRepairingCmd>,
    IAction<ShipyardSetRepairingCmd>,
    ICommandProcessor<ShipyardToggleUnloadPriorityCmd>,
    IAction<ShipyardToggleUnloadPriorityCmd>,
    ICommandProcessor<ShipyardCheatFullFuelCmd>,
    IAction<ShipyardCheatFullFuelCmd>,
    ICommandProcessor<ShipayardSetFuelSliderStepCmd>,
    IAction<ShipayardSetFuelSliderStepCmd>,
    ICommandProcessor<ShipyardToggleAutoRepairCmd>,
    IAction<ShipyardToggleAutoRepairCmd>,
    ICommandProcessor<ShipyardWorldEntityConstructionToggle>,
    IAction<ShipyardWorldEntityConstructionToggle>,
    ICommandProcessor<ShipyardToggleWorksPauseCmd>,
    IAction<ShipyardToggleWorksPauseCmd>,
    ICommandProcessor<ShipyardMakePrimaryCmd>,
    IAction<ShipyardMakePrimaryCmd>,
    ICommandProcessor<ShipyardDiscardProductCmd>,
    IAction<ShipyardDiscardProductCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly WorldMapManager m_worldMapManager;
    private readonly ProtosDb m_protosDb;
    private readonly IProductsManager m_productsManager;

    public ShipyardCommandsProcessor(
      EntitiesManager entitiesManager,
      WorldMapManager worldMapManager,
      ProtosDb protosDb,
      IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_worldMapManager = worldMapManager;
      this.m_protosDb = protosDb;
      this.m_productsManager = productsManager;
    }

    private bool tryGetShipyard(EntityId id, out Mafi.Core.Buildings.Shipyard.Shipyard shipyard, out string error)
    {
      if (id.IsValid)
      {
        if (!this.m_entitiesManager.TryGetEntity<Mafi.Core.Buildings.Shipyard.Shipyard>(id, out shipyard))
        {
          error = string.Format("Failed to get shipyard with ID {0}.", (object) id);
          shipyard = (Mafi.Core.Buildings.Shipyard.Shipyard) null;
          return false;
        }
      }
      else
      {
        Lyst<Mafi.Core.Buildings.Shipyard.Shipyard> lyst = this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Buildings.Shipyard.Shipyard>().ToLyst<Mafi.Core.Buildings.Shipyard.Shipyard>();
        if (lyst.Count != 1)
        {
          error = string.Format("Failed to find single shipyard in the game, count: {0}", (object) lyst.Count);
          shipyard = (Mafi.Core.Buildings.Shipyard.Shipyard) null;
          return false;
        }
        shipyard = lyst.First;
      }
      error = "";
      return true;
    }

    void IAction<ShipyardSetRepairingCmd>.Invoke(ShipyardSetRepairingCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        shipyard.SetRepairEnabled(cmd.IsRepairing);
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipyardToggleUnloadPriorityCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        shipyard.HasHighCargoUnloadPrio = !shipyard.HasHighCargoUnloadPrio;
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipyardCheatFullFuelCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        Quantity usableCapacity = shipyard.FuelBuffer.UsableCapacity;
        shipyard.Cheat_FuelExactly(usableCapacity);
        this.m_productsManager.ProductCreated(shipyard.FuelBuffer.Product, usableCapacity, CreateReason.Cheated);
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipayardSetFuelSliderStepCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        shipyard.UpdateFuelImportExportStep(cmd.ImportStep, cmd.ExportStep);
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipyardWorldEntityConstructionToggle cmd)
    {
      IWorldMapEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IWorldMapEntity>(cmd.WorldEntityIdToConstruct, out entity))
        cmd.SetResultError(string.Format("No world map entity with id '{0}' found!", (object) cmd.WorldEntityIdToConstruct));
      else if (entity is IWorldMapRepairableEntity worldMapEntity)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
        string error;
        if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
        {
          shipyard.ToggleCargoLoadFor(worldMapEntity);
          cmd.SetResultSuccess();
        }
        else
          cmd.SetResultError(error);
      }
      else
        cmd.SetResultError(string.Format("World map entity with id '{0}' is not repairable!", (object) cmd.WorldEntityIdToConstruct));
    }

    public void Invoke(ShipyardToggleWorksPauseCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        shipyard.ToggleWorksPause();
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipyardToggleAutoRepairCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        shipyard.IsAutoRepairEnabled = !shipyard.IsAutoRepairEnabled;
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(error);
    }

    public void Invoke(ShipyardMakePrimaryCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard1;
      string error;
      if (!this.tryGetShipyard(cmd.ShipyardId, out shipyard1, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        Mafi.Core.Buildings.Shipyard.Shipyard shipyard2 = this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Buildings.Shipyard.Shipyard>().FirstOrDefault<Mafi.Core.Buildings.Shipyard.Shipyard>((Func<Mafi.Core.Buildings.Shipyard.Shipyard, bool>) (x => x.DockedFleet.HasValue));
        if (shipyard2 == null)
          cmd.SetResultError("Primary shipyard not found!");
        else if (shipyard2 == shipyard1)
        {
          cmd.SetResultError("Shipyard already assigned");
        }
        else
        {
          shipyard2.DockedFleet.Value.MoveToNewShipyard(shipyard1);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(ShipyardDiscardProductCmd cmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      string error;
      if (!this.tryGetShipyard(cmd.ShipyardId, out shipyard, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        ProductProto proto;
        if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
        {
          cmd.SetResultError(string.Format("Failed to find product with id '{0}'", (object) cmd.ProductId));
        }
        else
        {
          shipyard.TryToDiscardCargo(proto);
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
