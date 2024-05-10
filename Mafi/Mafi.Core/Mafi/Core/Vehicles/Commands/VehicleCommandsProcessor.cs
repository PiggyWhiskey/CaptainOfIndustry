// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.VehicleCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Input;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class VehicleCommandsProcessor : 
    ICommandProcessor<RemoveVehicleFromBuildQueueCmd>,
    IAction<RemoveVehicleFromBuildQueueCmd>,
    ICommandProcessor<AddVehicleToBuildQueueCmd>,
    IAction<AddVehicleToBuildQueueCmd>,
    ICommandProcessor<QuickBuildCurrentVehicleCmd>,
    IAction<QuickBuildCurrentVehicleCmd>,
    ICommandProcessor<AssignVehicleToEntityCmd>,
    IAction<AssignVehicleToEntityCmd>,
    ICommandProcessor<AssignVehicleTypeToEntityCmd>,
    IAction<AssignVehicleTypeToEntityCmd>,
    ICommandProcessor<UnassignVehicleFromEntityCmd>,
    IAction<UnassignVehicleFromEntityCmd>,
    ICommandProcessor<UnassignVehicleCmd>,
    IAction<UnassignVehicleCmd>,
    ICommandProcessor<ScrapVehicleCmd>,
    IAction<ScrapVehicleCmd>,
    ICommandProcessor<ReplaceVehicleCmd>,
    IAction<ReplaceVehicleCmd>,
    ICommandProcessor<CancelReplaceVehicleCmd>,
    IAction<CancelReplaceVehicleCmd>,
    ICommandProcessor<RecoverVehicleCmd>,
    IAction<RecoverVehicleCmd>,
    ICommandProcessor<CreateAndSpawnVehicle>,
    IAction<CreateAndSpawnVehicle>,
    ICommandProcessor<RemoveAndDestroyVehicle>,
    IAction<RemoveAndDestroyVehicle>,
    ICommandProcessor<FinishVehicleBuildCmd>,
    IAction<FinishVehicleBuildCmd>,
    ICommandProcessor<TogglePartialTrucksLoadCmd>,
    IAction<TogglePartialTrucksLoadCmd>,
    ICommandProcessor<ExcavatorTogglePreferredProductCmd>,
    IAction<ExcavatorTogglePreferredProductCmd>,
    ICommandProcessor<NavigateVehicleToPositionCmd>,
    IAction<NavigateVehicleToPositionCmd>,
    ICommandProcessor<DiscardVehicleCargoCmd>,
    IAction<DiscardVehicleCargoCmd>,
    ICommandProcessor<VehicleCheatProductCmd>,
    IAction<VehicleCheatProductCmd>
  {
    public static readonly Upoints COST_TO_DISCARD_CARGO;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;
    private readonly VehiclesManager m_vehiclesManager;
    private readonly VehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly VehicleJobId.Factory m_jobIdFactory;
    private readonly EntitiesCreator m_entitiesCreator;
    private readonly NavigateToJob.Factory m_navigateToJobFactory;
    private readonly VehicleGoalsFactory m_vehicleGoalsFactory;
    private readonly IUpointsManager m_upointsManager;
    private readonly IAssetTransactionManager m_assetManager;

    public VehicleCommandsProcessor(
      VehiclesManager vehiclesManager,
      VehicleBuffersRegistry vehicleBuffersRegistry,
      EntitiesManager entitiesManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      VehicleJobId.Factory jobIdFactory,
      EntitiesCreator entitiesCreator,
      NavigateToJob.Factory navigateToJobFactory,
      VehicleGoalsFactory vehicleGoalsFactory,
      IUpointsManager upointsManager,
      IAssetTransactionManager assetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager.CheckNotNull<EntitiesManager>();
      this.m_protosDb = protosDb.CheckNotNull<ProtosDb>();
      this.m_vehiclesManager = vehiclesManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_jobIdFactory = jobIdFactory;
      this.m_entitiesCreator = entitiesCreator;
      this.m_navigateToJobFactory = navigateToJobFactory;
      this.m_vehicleGoalsFactory = vehicleGoalsFactory;
      this.m_upointsManager = upointsManager;
      this.m_assetManager = assetManager;
    }

    void IAction<RemoveVehicleFromBuildQueueCmd>.Invoke(RemoveVehicleFromBuildQueueCmd cmd)
    {
      Option<VehicleDepotBase> entityOrLog = this.m_entitiesManager.GetEntityOrLog<VehicleDepotBase>(cmd.VehicleDepotId);
      if (entityOrLog.IsNone)
      {
        cmd.SetResultError(string.Format("Failed to find vehicle depot '{0}'.", (object) cmd.VehicleDepotId));
      }
      else
      {
        entityOrLog.Value.RemoveVehicleFromBuildOrReplaceQueue(cmd.Index);
        cmd.SetResultSuccess();
      }
    }

    void IAction<AddVehicleToBuildQueueCmd>.Invoke(AddVehicleToBuildQueueCmd cmd)
    {
      DrivingEntityProto proto;
      if (!this.m_protosDb.TryGetProto<DrivingEntityProto>((Proto.ID) cmd.ProtoId, out proto))
        cmd.SetResultError(false, string.Format("Unknown proto '{0}'.", (object) cmd.ProtoId));
      else if (!this.m_unlockedProtosDb.IsUnlocked((Proto) proto))
      {
        cmd.SetResultError(string.Format("Proto '{0}' is not unlocked.", (object) cmd.ProtoId));
      }
      else
      {
        VehicleDepotBase entity;
        if (!this.m_entitiesManager.TryGetEntity<VehicleDepotBase>(cmd.VehicleDepotId, out entity))
        {
          cmd.SetResultError(string.Format("Vehicle depot '{0}' was not found.", (object) cmd.VehicleDepotId));
        }
        else
        {
          for (int index = 0; index < cmd.Count; ++index)
          {
            if (!entity.AddVehicleToBuildQueue(proto))
            {
              cmd.SetResultError(string.Format("Failed to queue vehicle '{0}' at depot '{1}'.", (object) cmd.ProtoId, (object) cmd.VehicleDepotId));
              return;
            }
          }
          cmd.SetResultSuccess();
        }
      }
    }

    void IAction<QuickBuildCurrentVehicleCmd>.Invoke(QuickBuildCurrentVehicleCmd cmd)
    {
      VehicleDepotBase entity;
      if (!this.m_entitiesManager.TryGetEntity<VehicleDepotBase>(cmd.VehicleDepotId, out entity))
        cmd.SetResultError(string.Format("Vehicle depot '{0}' was not found.", (object) cmd.VehicleDepotId));
      else if (!entity.TryQuickBuildCurrentVehicle())
        cmd.SetResultError(string.Format("Failed to quick-build current vehicle at depot '{0}', ", (object) cmd.VehicleDepotId) + string.Format("build queue length is {0}.", (object) (entity.ReplaceQueue.Count + entity.BuildQueue.Count)));
      else
        cmd.SetResultSuccess();
    }

    public void Invoke(AssignVehicleToEntityCmd cmd)
    {
      Vehicle entity1;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        IEntityAssignedWithVehicles entity2;
        if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedWithVehicles>(cmd.EntityId, out entity2))
        {
          cmd.SetResultError(string.Format("Failed to find entity '{0}'.", (object) cmd.EntityId));
        }
        else
        {
          entity2.AssignVehicle(entity1);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(AssignVehicleTypeToEntityCmd cmd)
    {
      DynamicEntityProto proto;
      if (!this.m_protosDb.TryGetProto<DynamicEntityProto>((Proto.ID) cmd.VehicleId, out proto))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle proto '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        IEntityAssignedWithVehicles entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedWithVehicles>(cmd.EntityId, out entity))
          cmd.SetResultError(string.Format("Failed to find entity '{0}'.", (object) cmd.EntityId));
        else if (!entity.CanVehicleBeAssigned(proto))
        {
          cmd.SetResultError(string.Format("Cannot assign vehicle '{0}' - not supported.", (object) proto.Id));
        }
        else
        {
          for (int index = 0; index < cmd.Count; ++index)
            entity.AssignVehicle((IVehiclesManager) this.m_vehiclesManager, proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(UnassignVehicleFromEntityCmd cmd)
    {
      DynamicEntityProto proto;
      if (!this.m_protosDb.TryGetProto<DynamicEntityProto>((Proto.ID) cmd.VehicleId, out proto))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle proto '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        IEntityAssignedWithVehicles entity;
        if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedWithVehicles>(cmd.EntityId, out entity))
        {
          cmd.SetResultError(string.Format("Failed to find entity '{0}'.", (object) cmd.EntityId));
        }
        else
        {
          for (int index = 0; index < cmd.Count; ++index)
            entity.UnassignVehicle(proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(UnassignVehicleCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find entity '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        if (entity.AssignedTo.HasValue)
          entity.UnassignFrom(entity.AssignedTo.Value);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ScrapVehicleCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      else if (entity.TryRequestScrap())
        cmd.SetResultSuccess();
      else
        cmd.SetResultError("No depots?");
    }

    public void Invoke(ReplaceVehicleCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        DrivingEntityProto proto;
        if (!this.m_protosDb.TryGetProto<DrivingEntityProto>((Proto.ID) cmd.TargetProtoId, out proto))
          cmd.SetResultError(string.Format("Failed to find proto to replace with '{0}'.", (object) cmd.TargetProtoId));
        else if (entity.TryToRequestConstructionOfReplacement(proto))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError("No depots?");
      }
    }

    public void Invoke(CancelReplaceVehicleCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      else if (entity.CancelReplace(true))
        cmd.SetResultSuccess();
      else
        cmd.SetResultError("Failed to cancel replacement.");
    }

    public void Invoke(RecoverVehicleCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      else if (entity.TryRequestRecovery())
        cmd.SetResultSuccess();
      else
        cmd.SetResultError(string.Format("Failed to request recovery for vehicle #{0}, no depots? not enough unity?", (object) cmd.VehicleId));
    }

    public void Invoke(CreateAndSpawnVehicle cmd)
    {
      DrivingEntityProto proto;
      if (!this.m_protosDb.TryGetProto<DrivingEntityProto>((Proto.ID) cmd.ProtoId, out proto))
      {
        cmd.SetResultError(EntityId.Invalid, string.Format("Proto '{0}' was not found.", (object) cmd.ProtoId));
      }
      else
      {
        Vehicle vehicle;
        if (!this.m_entitiesCreator.TryCreateVehicle((DynamicGroundEntityProto) proto, out vehicle))
        {
          cmd.SetResultError(EntityId.Invalid, string.Format("Failed to instantiate vehicle '{0}'.", (object) proto.Id));
        }
        else
        {
          EntityValidationResult validationResult = this.m_entitiesManager.TryAddEntity((IEntity) vehicle);
          if (!validationResult.IsSuccess)
          {
            cmd.SetResultError(EntityId.Invalid, string.Format("Failed to add vehicle '{0}' to the world. Error: {1}", (object) proto, (object) validationResult.ErrorMessage));
            ((IEntityFriend) vehicle).Destroy();
          }
          else
          {
            vehicle.Spawn(cmd.SpawnLocation, AngleDegrees1f.Zero);
            if (cmd.EnqueueEmptyJob)
              vehicle.EnqueueJob((VehicleJob) new EmptyJob(this.m_jobIdFactory.GetNextId(), vehicle, Duration.MaxValue, true));
            cmd.SetResultSuccess(vehicle.Id);
          }
        }
      }
    }

    public void Invoke(RemoveAndDestroyVehicle cmd)
    {
      DynamicGroundEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<DynamicGroundEntity>(cmd.Id, out entity))
      {
        cmd.SetResultError(string.Format("Vehicle '{0}' was not found.", (object) cmd.Id));
      }
      else
      {
        entity.Despawn();
        EntityValidationResult validationResult = this.m_entitiesManager.TryRemoveAndDestroyEntity((IEntity) entity);
        if (validationResult.IsSuccess)
          cmd.SetResultSuccess();
        else
          cmd.SetResultError(validationResult.ErrorMessage);
      }
    }

    void IAction<FinishVehicleBuildCmd>.Invoke(FinishVehicleBuildCmd cmd)
    {
      VehicleDepotBase entity;
      if (!this.m_entitiesManager.TryGetEntity<VehicleDepotBase>(cmd.VehicleDepotId, out entity))
      {
        cmd.SetResultError(string.Format("Vehicle depot '{0}' was not found.", (object) cmd.VehicleDepotId));
      }
      else
      {
        entity.Cheat_FinishBuildOfCurrentVehicle();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ExcavatorTogglePreferredProductCmd cmd)
    {
      Excavator entity;
      if (!this.m_entitiesManager.TryGetEntity<Excavator>(cmd.ExcavatorId, out entity))
        cmd.SetResultError(string.Format("Excavator '{0}' was not found.", (object) cmd.ExcavatorId));
      else if (!cmd.ProductToMine.HasValue)
      {
        entity.SetPrioritizeProduct((Option<LooseProductProto>) Option.None);
        cmd.SetResultSuccess();
      }
      else
      {
        LooseProductProto proto;
        if (!this.m_protosDb.TryGetProto<LooseProductProto>((Proto.ID) cmd.ProductToMine.Value, out proto))
        {
          cmd.SetResultError(string.Format("Product '{0}' was not found.", (object) cmd.ProductToMine.Value));
        }
        else
        {
          entity.SetPrioritizeProduct((Option<LooseProductProto>) proto);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(TogglePartialTrucksLoadCmd cmd)
    {
      this.m_vehicleBuffersRegistry.AllowPartialTrucks = !this.m_vehicleBuffersRegistry.AllowPartialTrucks;
      cmd.SetResultSuccess();
    }

    public void Invoke(NavigateVehicleToPositionCmd cmd)
    {
      Vehicle entity;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        TilePositionVehicleGoal goal = this.m_vehicleGoalsFactory.CreateGoal(cmd.Position, 1.Tiles());
        if (entity.HasJobs)
          entity.CancelAllJobsAndResetState();
        if (entity.IsOnWayToDepotForReplacement)
          entity.CancelReplace(true);
        else if (entity.IsOnWayToDepotForScrap)
          entity.CancelScrap();
        this.m_navigateToJobFactory.EnqueueJob(entity, (IVehicleGoalFull) goal);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(NavigateVehicleToStaticEntityCmd cmd)
    {
      Vehicle entity1;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        IStaticEntity entity2;
        if (!this.m_entitiesManager.TryGetEntity<IStaticEntity>(cmd.StaticEntityId, out entity2))
        {
          cmd.SetResultError(string.Format("Failed to find static entity '{0}'.", (object) cmd.StaticEntityId));
        }
        else
        {
          StaticEntityVehicleGoal goal = this.m_vehicleGoalsFactory.CreateGoal(entity2);
          if (entity1.HasJobs)
            entity1.CancelAllJobsAndResetState();
          if (entity1.IsOnWayToDepotForReplacement)
            entity1.CancelReplace(true);
          else if (entity1.IsOnWayToDepotForScrap)
            entity1.CancelScrap();
          this.m_navigateToJobFactory.EnqueueJob(entity1, (IVehicleGoalFull) goal);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(NavigateVehicleToVehicleCmd cmd)
    {
      Vehicle entity1;
      if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.VehicleId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find vehicle '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        Vehicle entity2;
        if (!this.m_entitiesManager.TryGetEntity<Vehicle>(cmd.GoalVehicleId, out entity2))
        {
          cmd.SetResultError(string.Format("Failed to find goal vehicle '{0}'.", (object) cmd.GoalVehicleId));
        }
        else
        {
          DynamicEntityVehicleGoal goal = this.m_vehicleGoalsFactory.CreateGoal(entity2);
          if (entity1.HasJobs)
            entity1.CancelAllJobsAndResetState();
          if (entity1.IsOnWayToDepotForReplacement)
            entity1.CancelReplace(true);
          else if (entity1.IsOnWayToDepotForScrap)
            entity1.CancelScrap();
          this.m_navigateToJobFactory.EnqueueJob(entity1, (IVehicleGoalFull) goal);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(DiscardVehicleCargoCmd cmd)
    {
      Truck entity;
      if (!this.m_entitiesManager.TryGetEntity<Truck>(cmd.VehicleId, out entity))
        cmd.SetResultError(string.Format("Failed to find truck '{0}'.", (object) cmd.VehicleId));
      else if (!this.m_upointsManager.CanConsume(VehicleCommandsProcessor.COST_TO_DISCARD_CARGO))
      {
        cmd.SetResultError(string.Format("Cannot afford discard for '{0}'.", (object) cmd.VehicleId));
      }
      else
      {
        PooledArray<ProductQuantity> cargoToDiscard = entity.TryGetCargoToDiscard();
        if (cargoToDiscard.Length == 0)
        {
          cmd.SetResultError(string.Format("Nothing to discard for '{0}'.", (object) cmd.VehicleId));
        }
        else
        {
          foreach (ProductQuantity backing in cargoToDiscard.BackingArray)
            this.m_assetManager.StoreClearedProduct(backing);
          cargoToDiscard.ReturnToPool();
          this.m_upointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickRemove, VehicleCommandsProcessor.COST_TO_DISCARD_CARGO);
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(VehicleCheatProductCmd cmd)
    {
      Truck entity;
      if (!this.m_entitiesManager.TryGetEntity<Truck>(cmd.VehicleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find truck '{0}'.", (object) cmd.VehicleId));
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
          entity.Cheat_NewProductAndCancelJobs(proto, entity.Capacity);
          cmd.SetResultSuccess();
        }
      }
    }

    static VehicleCommandsProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleCommandsProcessor.COST_TO_DISCARD_CARGO = 0.1.Upoints();
    }
  }
}
