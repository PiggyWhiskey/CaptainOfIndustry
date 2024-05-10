// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntitiesCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class EntitiesCommandsProcessor : 
    ICommandProcessor<BatchCreateStaticEntitiesCmd>,
    IAction<BatchCreateStaticEntitiesCmd>,
    ICommandProcessor<CreateStaticEntityCmd>,
    IAction<CreateStaticEntityCmd>,
    ICommandProcessor<StartDeconstructionOfStaticEntityCmd>,
    IAction<StartDeconstructionOfStaticEntityCmd>,
    ICommandProcessor<StartDeconstructionOfTransportSubSectionsCmd>,
    IAction<StartDeconstructionOfTransportSubSectionsCmd>,
    ICommandProcessor<ToggleEnabledCmd>,
    IAction<ToggleEnabledCmd>,
    ICommandProcessor<SetEntityEnabledCmd>,
    IAction<SetEntityEnabledCmd>,
    ICommandProcessor<AssignStaticEntityCmd>,
    IAction<AssignStaticEntityCmd>,
    ICommandProcessor<UnassignStaticEntityCmd>,
    IAction<UnassignStaticEntityCmd>,
    ICommandProcessor<ToggleStaticEntityConstructionCmd>,
    IAction<ToggleStaticEntityConstructionCmd>,
    ICommandProcessor<ToggleConstructionPriorityCmd>,
    IAction<ToggleConstructionPriorityCmd>,
    ICommandProcessor<EntityDisableLogisticsToggleCmd>,
    IAction<EntityDisableLogisticsToggleCmd>,
    ICommandProcessor<SetGeneralPriorityCmd>,
    IAction<SetGeneralPriorityCmd>,
    ICommandProcessor<SetCustomPriorityCmd>,
    IAction<SetCustomPriorityCmd>,
    ICommandProcessor<AssignProductToSlotCmd>,
    IAction<AssignProductToSlotCmd>,
    ICommandProcessor<CloneConfigBetweenEntitiesCmd>,
    IAction<CloneConfigBetweenEntitiesCmd>,
    ICommandProcessor<SpendUpointsOnEntitiesCmd>,
    IAction<SpendUpointsOnEntitiesCmd>,
    ICommandProcessor<TryTransformEntityCmd>,
    IAction<TryTransformEntityCmd>,
    ICommandProcessor<ToggleEnabledGroupCmd>,
    IAction<ToggleEnabledGroupCmd>,
    ICommandProcessor<SetEntityNameCmd>,
    IAction<SetEntityNameCmd>,
    ICommandProcessor<ToggleImportRouteEnforcementCmd>,
    IAction<ToggleImportRouteEnforcementCmd>
  {
    public static readonly Upoints UnityPerSurfaceTile;
    private readonly EntitiesManager m_entitiesManager;
    private readonly DependencyResolver m_resolver;
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly EntitiesCreator m_entitiesCreator;
    private readonly UpgradesManager m_upgradesManager;
    private readonly ConstructionManager m_constructionManager;
    private readonly EntitiesCloneConfigHelper m_configCloneHelper;
    private readonly TransportsManager m_transportsManager;
    private readonly TerrainManager m_terrainManager;
    private readonly TransportsCommandsProcessor m_transportsCommandsProcessor;
    private readonly TransportsConstructionHelper m_transportsConstructionHelper;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly AssetTransactionManager m_assetTransactionManager;
    private readonly SurfaceDesignationsManager m_surfaceDesignationsManager;
    private readonly ITreePlantingManager m_treePlantingManager;
    private readonly TerrainSurfaceManager m_terrainSurfaceManager;
    private readonly LayoutEntityAddRequestFactory m_addRequestFactory;
    private readonly IUpointsManager m_upointsManager;
    private readonly Lyst<IStaticEntity> m_entitiesTmp;
    private readonly Lyst<IStaticEntity> m_entitiesForQuickDeliverTmp;
    private readonly Lyst<IEntityWithBoost> m_entitiesForBoost;

    public EntitiesCommandsProcessor(
      EntitiesManager entitiesManager,
      DependencyResolver resolver,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      EntitiesCreator entitiesCreator,
      UpgradesManager upgradesManager,
      ConstructionManager constructionManager,
      EntitiesCloneConfigHelper configCloneHelper,
      TransportsManager transportsManager,
      TerrainManager terrainManager,
      TransportsCommandsProcessor transportsCommandsProcessor,
      TransportsConstructionHelper transportsConstructionHelper,
      TerrainOccupancyManager occupancyManager,
      AssetTransactionManager assetTransactionManager,
      SurfaceDesignationsManager surfaceDesignationsManager,
      ITreePlantingManager treePlantingManager,
      TerrainSurfaceManager terrainSurfaceManager,
      LayoutEntityAddRequestFactory addRequestFactory,
      IUpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_entitiesTmp = new Lyst<IStaticEntity>();
      this.m_entitiesForQuickDeliverTmp = new Lyst<IStaticEntity>();
      this.m_entitiesForBoost = new Lyst<IEntityWithBoost>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_resolver = resolver;
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_entitiesCreator = entitiesCreator;
      this.m_upgradesManager = upgradesManager;
      this.m_constructionManager = constructionManager;
      this.m_configCloneHelper = configCloneHelper;
      this.m_transportsManager = transportsManager;
      this.m_terrainManager = terrainManager;
      this.m_transportsCommandsProcessor = transportsCommandsProcessor;
      this.m_transportsConstructionHelper = transportsConstructionHelper;
      this.m_occupancyManager = occupancyManager;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_surfaceDesignationsManager = surfaceDesignationsManager;
      this.m_treePlantingManager = treePlantingManager;
      this.m_terrainSurfaceManager = terrainSurfaceManager;
      this.m_addRequestFactory = addRequestFactory;
      this.m_upointsManager = upointsManager;
    }

    public void Invoke(BatchCreateStaticEntitiesCmd cmd)
    {
      Lyst<Option<IEntity>> newEntities = new Lyst<Option<IEntity>>();
      Option<string> option = (Option<string>) Option.None;
      foreach (EntityConfigData config in cmd.ConfigData)
      {
        Proto valueOrNull = config.Prototype.ValueOrNull;
        if (valueOrNull == (Proto) null)
        {
          option = (Option<string>) "Proto in config is none";
          newEntities.Add((Option<IEntity>) Option.None);
        }
        else if (valueOrNull is TransportProto)
        {
          string error;
          if (!this.m_transportsCommandsProcessor.TryBuildTransportFromConfig(config, cmd.ApplyConfiguration, cmd.IsFree, out error))
            option = (Option<string>) (error ?? "Failed to create transport");
          newEntities.Add((Option<IEntity>) Option.None);
        }
        else
        {
          TileTransform? transform = config.Transform;
          if (!transform.HasValue)
          {
            option = (Option<string>) string.Format("Transform missing for proto {0}", (object) valueOrNull.Id);
            newEntities.Add((Option<IEntity>) Option.None);
          }
          else
          {
            switch (valueOrNull)
            {
              case StaticEntityProto proto1:
                bool buildMiniZippers = cmd.BuildMiniZippers == BuildMiniZippersMode.Always || cmd.BuildMiniZippers == BuildMiniZippersMode.DeferToProto && proto1 is ILayoutEntityProto layoutEntityProto && layoutEntityProto.AutoBuildMiniZippers;
                StaticEntity resultEntity;
                string error;
                if (!this.createStaticEntityProto(proto1, transform.Value, buildMiniZippers, cmd.IsFree, out resultEntity, out error))
                {
                  option = (Option<string>) error;
                  newEntities.Add((Option<IEntity>) Option.None);
                  continue;
                }
                newEntities.Add((Option<IEntity>) (IEntity) resultEntity);
                continue;
              case TreeProto proto2:
                ITreePlantingManager treePlantingManager = this.m_treePlantingManager;
                Tile2f xy = transform.Value.Position.CenterTile3f.Xy;
                AngleSlim zero = AngleSlim.Zero;
                Percent? baseScale = new Percent?(Percent.Hundred);
                if (!treePlantingManager.TryAddManualTree(proto2, xy, zero, baseScale))
                  option = (Option<string>) "Failed to add tree";
                newEntities.Add((Option<IEntity>) Option.None);
                continue;
              default:
                option = (Option<string>) string.Format("Unknown proto type {0}, {1}", (object) valueOrNull.Id, (object) valueOrNull.GetType().Name);
                newEntities.Add((Option<IEntity>) Option.None);
                continue;
            }
          }
        }
      }
      if (cmd.ApplyConfiguration)
        this.m_configCloneHelper.ApplyConfigToAll((IIndexable<Option<IEntity>>) newEntities, cmd.ConfigData.AsIndexable);
      if (option.HasValue)
        cmd.SetResultError(option.Value);
      else
        cmd.SetResultSuccess();
    }

    public void Invoke(CreateStaticEntityCmd cmd)
    {
      StaticEntityProto proto;
      if (!this.m_protosDb.TryGetProto<StaticEntityProto>((Proto.ID) cmd.ProtoId, out proto))
      {
        cmd.SetResultError(EntityId.Invalid, string.Format("Unknown proto '{0}'", (object) cmd.ProtoId));
      }
      else
      {
        StaticEntity resultEntity;
        string error;
        if (!this.createStaticEntityProto(proto, cmd.Transform, false, cmd.IsFree, out resultEntity, out error))
          cmd.SetResultError(EntityId.Invalid, error);
        else
          cmd.SetResultSuccess(resultEntity.Id);
      }
    }

    private void buildMiniZippersAtPorts(StaticEntity entity)
    {
      if (entity.Prototype is MiniZipperProto)
        Log.Error("Attempting to build miniZippers from miniZipper");
      else if (entity is IEntityWithPorts entityWithPorts)
      {
        foreach (IoPort port in entityWithPorts.Ports)
        {
          Tile3i connectedPortCoord = port.ExpectedConnectedPortCoord;
          Transport entity1;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(connectedPortCoord, out entity1) && !((Proto) entity1.Prototype.PortsShape != (Proto) port.ShapePrototype) && !(entity1.EndOutputPort.ConnectedPort == port) && !(entity1.StartInputPort.ConnectedPort == port))
          {
            RelTile3i relTile3i;
            string str;
            if (entity1.EndOutputPort.Position == connectedPortCoord && entity1.EndOutputPort.IsNotConnected && port.Type == IoPortType.Input)
            {
              Transport transport = entity1;
              relTile3i = port.Position - connectedPortCoord;
              Direction903d direction903d = relTile3i.ToDirection903d();
              ref string local = ref str;
              if (transport.TryChangeDirectionAtEnd(direction903d, out local))
                continue;
            }
            if (entity1.StartInputPort.Position == connectedPortCoord && entity1.StartInputPort.IsNotConnected && port.Type == IoPortType.Output)
            {
              Transport transport = entity1;
              relTile3i = port.Position - connectedPortCoord;
              Direction903d direction903d = relTile3i.ToDirection903d();
              ref string local = ref str;
              if (transport.TryChangeDirectionAtStart(direction903d, out local))
                continue;
            }
            LocStrFormatted error;
            if (this.m_transportsConstructionHelper.CanPlaceMiniZipperAt(entity1, connectedPortCoord, out CanPlaceMiniZipperAtResult _, out error))
              this.m_transportsManager.TryBuildOrJoinTransport(entity1.Prototype, ImmutableArray.Create<Tile3i>(connectedPortCoord), new Direction903d?(), new Direction903d?(), false, out Option<Transport> _, out error);
          }
        }
      }
      else
        Log.Error(string.Format("Attempting to build miniZippers from entity that does not derive from IEntityWithPorts {0}.", (object) entity.Prototype));
    }

    private bool createStaticEntityProto(
      StaticEntityProto proto,
      TileTransform transform,
      bool buildMiniZippers,
      bool isFree,
      out StaticEntity resultEntity,
      out string error)
    {
      resultEntity = (StaticEntity) null;
      if (!this.m_unlockedProtosDb.IsUnlocked((Proto) proto))
      {
        error = string.Format("Proto '{0}' is not unlocked.", (object) proto.Id);
        return false;
      }
      StaticEntity entity;
      if (!this.m_entitiesCreator.TryCreateStaticEntity(proto, transform, out entity))
      {
        error = string.Format("Failed to instantiate entity '{0}'.", (object) proto.Id);
        return false;
      }
      if (entity == null)
      {
        error = string.Format("Failed to instantiate entity '{0}'.", (object) proto.Id);
        return false;
      }
      EntityValidationResult validationResult = this.m_entitiesManager.TryAddEntity((IEntity) entity);
      if (validationResult.IsError)
      {
        ((IEntityFriend) entity).Destroy();
        error = string.Format("Failed to add entity '{0}' to the world. {1}", (object) proto.Id, (object) validationResult.ErrorMessage);
        return false;
      }
      if (buildMiniZippers)
        this.buildMiniZippersAtPorts(entity);
      if (isFree)
        entity.MakeFullyConstructed();
      resultEntity = entity;
      error = "";
      return true;
    }

    public void Invoke(StartDeconstructionOfStaticEntityCmd cmd)
    {
      StaticEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<StaticEntity>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to remove static entity {0}.", (object) cmd.EntityId));
      }
      else
      {
        EntityValidationResult validationResult = this.m_entitiesManager.CanRemoveEntity((IEntity) entity, cmd.RemoveReason);
        if (validationResult.IsError)
        {
          cmd.SetResultError(false, string.Format("Failed to remove entity '{0}' from the world. {1}", (object) entity, (object) validationResult.ErrorMessage));
        }
        else
        {
          entity.StartDeconstructionIfCan();
          cmd.SetResultSuccess();
        }
      }
    }

    public void Invoke(StartDeconstructionOfTransportSubSectionsCmd cmd)
    {
      if (cmd.RemovedSubSections.IsEmpty)
      {
        cmd.SetResultSuccess();
      }
      else
      {
        int num1 = 0;
        foreach (Pair<Tile3i, Tile3i> removedSubSection in cmd.RemovedSubSections)
        {
          Transport entity1;
          Transport entity2;
          Option<Transport> deconstructedSubTransport;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(removedSubSection.First, out entity1) && (!(removedSubSection.First != removedSubSection.Second) || this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(removedSubSection.Second, out entity2) && entity1 == entity2) && this.m_transportsManager.TryDeconstructSubTransport(entity1, removedSubSection.First, removedSubSection.Second, out Option<Transport> _, out deconstructedSubTransport, out Option<Transport> _, out LocStrFormatted _))
          {
            ++num1;
            if (cmd.QuickRemoveWithUnity && deconstructedSubTransport.HasValue)
            {
              this.m_assetTransactionManager.StoreValue(deconstructedSubTransport.Value.ClearAndReturnTransportedProducts(), new CreateReason?());
              this.m_constructionManager.TryPerformQuickDeliveryOrRemoval((IStaticEntity) deconstructedSubTransport.Value, true);
            }
          }
        }
        if (num1 < cmd.RemovedSubSections.Length)
        {
          int num2 = cmd.RemovedSubSections.Length - num1;
          cmd.SetResultError(string.Format("Failed to remove {0}/{1} sub-sections ", (object) num2, (object) cmd.RemovedSubSections.Length) + string.Format("of transport {0}.", (object) cmd.TransportId));
        }
        else
          cmd.SetResultSuccess();
      }
    }

    public void Invoke(CloneConfigBetweenEntitiesCmd cmd)
    {
      IEntity entity1;
      if (!this.m_entitiesManager.TryGetEntity<IEntity>(cmd.SourceEntityId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find source entity {0}.", (object) cmd.SourceEntityId));
      }
      else
      {
        IEntity entity2;
        if (!this.m_entitiesManager.TryGetEntity<IEntity>(cmd.TargetEntityId, out entity2))
          cmd.SetResultError(string.Format("Failed to find target entity {0}.", (object) cmd.SourceEntityId));
        else if (this.m_configCloneHelper.TryCopyConfigFromTo(entity1, entity2))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError("Failed to copy entity configuration");
      }
    }

    public void Invoke(ToggleEnabledCmd cmd)
    {
      IEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("Failed to find entity '{0}' to toggle enabled on it.", (object) cmd.EntityId));
      else if (!entity.CanBePaused)
      {
        cmd.SetResultError(string.Format("Entity '{0}' does not support pause toggle.", (object) entity));
      }
      else
      {
        entity.SetPaused(!entity.IsPaused);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ToggleEnabledGroupCmd cmd)
    {
      Lyst<IEntity> lyst = cmd.EntityIds.MapList<IEntity>((Func<EntityId, IEntity>) (x => this.m_entitiesManager.GetEntity(x).ValueOrNull));
      lyst.RemoveWhere((Predicate<IEntity>) (x => x == null));
      bool flag1 = false;
      bool flag2 = false;
      if (!cmd.PauseOnly)
      {
        foreach (IEntity entity1 in lyst)
        {
          if (entity1 is IStaticEntity entity2 && !entity2.IsConstructed)
          {
            flag2 = true;
            flag1 |= this.m_constructionManager.TrySetConstructionPause(entity2, false);
          }
        }
      }
      if (flag1)
      {
        cmd.SetResultSuccess();
      }
      else
      {
        foreach (IEntity entity3 in lyst)
        {
          if (entity3 is IStaticEntity entity4 && !entity4.IsConstructed)
          {
            flag2 = true;
            flag1 |= this.m_constructionManager.TrySetConstructionPause(entity4, true);
          }
        }
        if (flag1)
          cmd.SetResultSuccess();
        else if (flag2)
        {
          cmd.SetResultError("Nothing to pause / unpause");
        }
        else
        {
          if (!cmd.PauseOnly)
          {
            foreach (IEntity entity in lyst)
            {
              if (entity.IsPaused)
              {
                entity.SetPaused(!entity.IsPaused);
                flag1 = true;
              }
            }
          }
          if (flag1)
          {
            cmd.SetResultSuccess();
          }
          else
          {
            bool flag3 = lyst.Count == 1;
            foreach (IEntity entity in lyst)
            {
              if (!entity.IsPaused && entity.CanBePaused && (flag3 || !(entity is Transport)) && (!(entity is IStaticEntity staticEntity) || staticEntity.IsConstructed))
              {
                entity.SetPaused(!entity.IsPaused);
                flag1 = true;
              }
            }
            if (flag1)
              cmd.SetResultSuccess();
            else
              cmd.SetResultError("Nothing to pause / unpause");
          }
        }
      }
    }

    public void Invoke(SetEntityEnabledCmd cmd)
    {
      IEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("Failed to find entity '{0}' to toggle enabled on it.", (object) cmd.EntityId));
      else if (!entity.CanBePaused)
      {
        cmd.SetResultError(string.Format("Entity '{0}' does not support pause toggle.", (object) entity));
      }
      else
      {
        entity.SetPaused(!cmd.IsEnabled);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(AssignStaticEntityCmd cmd)
    {
      IEntityAssignedAsOutput entity1;
      if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedAsOutput>(cmd.FirstEntityId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find entity with id '{0}' to assign.", (object) cmd.FirstEntityId));
      }
      else
      {
        IEntityAssignedAsInput entity2;
        if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedAsInput>(cmd.SecondEntityId, out entity2))
          cmd.SetResultError(string.Format("Failed to find entity with id '{0}' to assign.", (object) cmd.SecondEntityId));
        else if (entity1 == entity2)
          cmd.SetResultError("Given two entities are identical.");
        else if (entity2.CanBeAssignedWithOutput(entity1) && entity1.CanBeAssignedWithInput(entity2))
        {
          entity2.AssignStaticOutputEntity(entity1);
          entity1.AssignStaticInputEntity(entity2);
          cmd.SetResultSuccess();
        }
        else
          cmd.SetResultError("Entities are not compatible.");
      }
    }

    public void Invoke(UnassignStaticEntityCmd cmd)
    {
      IEntityAssignedAsOutput entity1;
      if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedAsOutput>(cmd.FirstEntityId, out entity1))
      {
        cmd.SetResultError(string.Format("Failed to find entity with id '{0}' to assign.", (object) cmd.FirstEntityId));
      }
      else
      {
        IEntityAssignedAsInput entity2;
        if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedAsInput>(cmd.SecondEntityId, out entity2))
          cmd.SetResultError(string.Format("Failed to find entity with id '{0}' to assign.", (object) cmd.SecondEntityId));
        else if (entity1 != entity2)
        {
          entity2.UnassignStaticOutputEntity(entity1);
          entity1.UnassignStaticInputEntity(entity2);
          cmd.SetResultSuccess();
        }
        else
          cmd.SetResultError("Given two entities are identical.");
      }
    }

    public void Invoke(ToggleImportRouteEnforcementCmd cmd)
    {
      IEntityAssignedAsInput entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityAssignedAsInput>(cmd.EntityToToggleId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find entity with id '{0}' to assign.", (object) cmd.EntityToToggleId));
      }
      else
      {
        entity.AllowNonAssignedOutput = !entity.AllowNonAssignedOutput;
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ToggleStaticEntityConstructionCmd cmd)
    {
      StaticEntity entity1;
      if (!this.m_entitiesManager.TryGetEntity<StaticEntity>(cmd.EntityId, out entity1))
        cmd.SetResultError(string.Format("Failed to remove static entity '{0}'.", (object) cmd.EntityId));
      else if (entity1.IsConstructed)
      {
        cmd.SetResultError(string.Format("Cannot toggle already constructed '{0}').", (object) entity1));
      }
      else
      {
        bool flag;
        switch (entity1.ConstructionState)
        {
          case ConstructionState.PendingDeconstruction:
          case ConstructionState.InDeconstruction:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        if (flag)
        {
          entity1.AbortDeconstruction();
          cmd.SetResultSuccess();
        }
        else if (entity1 is IUpgradableEntity entity2 && this.m_upgradesManager.CancelUpgradeIfNeeded(entity2))
          cmd.SetResultSuccess();
        else if (entity1.ConstructionState == ConstructionState.InConstruction)
        {
          EntityValidationResult validationResult = this.m_entitiesManager.CanRemoveEntity((IEntity) entity1, EntityRemoveReason.Remove);
          if (validationResult.IsError)
          {
            cmd.SetResultError(string.Format("Canceling construction of '{0}' is forbidden - {1}.", (object) entity1, (object) validationResult.ErrorMessage));
          }
          else
          {
            entity1.StartDeconstructionIfCan();
            cmd.SetResultSuccess();
          }
        }
        else
          cmd.SetResultError(string.Format("No valid construction state of '{0}' to toggle it ", (object) entity1) + string.Format("(has state = '{0}').", (object) entity1.ConstructionState));
      }
    }

    public void Invoke(ToggleConstructionPriorityCmd cmd)
    {
      StaticEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<StaticEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("Failed to find a static entity '{0}'.", (object) entity));
      else if (entity.IsConstructed && !entity.IsBeingUpgraded)
        cmd.SetResultError(string.Format("Cannot toggle already constructed '{0}'.", (object) entity));
      else if (entity.ConstructionProgress.IsNone)
      {
        cmd.SetResultError(string.Format("Cannot toggle entity that has no construction state set '{0}'.", (object) entity));
      }
      else
      {
        entity.ConstructionProgress.Value.SetPriority(!entity.ConstructionProgress.Value.IsPriority);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(EntityDisableLogisticsToggleCmd cmd)
    {
      IEntityWithLogisticsControl entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityWithLogisticsControl>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) cmd.EntityId));
      }
      else
      {
        if (cmd.IsInput)
          entity.SetLogisticsInputMode(cmd.Mode);
        else
          entity.SetLogisticsOutputMode(cmd.Mode);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(SetGeneralPriorityCmd cmd)
    {
      IEntityWithGeneralPriority entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityWithGeneralPriority>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) cmd.EntityId));
      }
      else
      {
        entity.SetGeneralPriority(cmd.Priority);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(SetCustomPriorityCmd cmd)
    {
      IEntityWithCustomPriority entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityWithCustomPriority>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) cmd.EntityId));
      }
      else
      {
        entity.SetCustomPriority(cmd.PriorityId, cmd.Priority);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(AssignProductToSlotCmd cmd)
    {
      IEntityWithMultipleProductsToAssign entity;
      if (!this.m_entitiesManager.TryGetEntity<IEntityWithMultipleProductsToAssign>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) cmd.EntityId));
      }
      else
      {
        if (cmd.ProductId.HasValue)
        {
          ProductProto proto;
          if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId.Value, out proto))
          {
            cmd.SetResultError(string.Format("Unknown product '{0}'", (object) cmd.ProductId));
            return;
          }
          entity.SetProduct((Option<ProductProto>) proto, cmd.Slot, false);
        }
        else
          entity.SetProduct(Option<ProductProto>.None, cmd.Slot, false);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(SpendUpointsOnEntitiesCmd cmd)
    {
      this.m_entitiesTmp.Clear();
      this.m_entitiesForQuickDeliverTmp.Clear();
      this.m_entitiesForBoost.Clear();
      foreach (EntityId entityId in cmd.EntityIds)
      {
        IStaticEntity entity;
        if (this.m_entitiesManager.TryGetEntity<IStaticEntity>(entityId, out entity))
          this.m_entitiesTmp.Add(entity);
      }
      bool hasSurfacesToModify = false;
      if (cmd.Area.HasValue)
      {
        foreach (Tile2iAndIndex enumerateTilesAndIndex in cmd.Area.Value.EnumerateTilesAndIndices(this.m_terrainManager))
        {
          Option<SurfaceDesignation> designationAt = this.m_surfaceDesignationsManager.GetDesignationAt(enumerateTilesAndIndex.TileCoord);
          if (designationAt.HasValue && designationAt.Value.IsNotFulfilled)
          {
            hasSurfacesToModify = true;
            break;
          }
        }
      }
      bool enableBoost;
      EntitiesCommandsProcessor.EvaluateUnitySpending(hasSurfacesToModify, this.m_entitiesTmp, this.m_entitiesForQuickDeliverTmp, this.m_entitiesForBoost, out enableBoost, out int _);
      bool flag = false;
      foreach (IStaticEntity entity in this.m_entitiesForQuickDeliverTmp)
        flag |= this.m_constructionManager.TryPerformQuickDeliveryOrRemoval(entity, true);
      foreach (IEntityWithBoost entityWithBoost in this.m_entitiesForBoost)
      {
        if (entityWithBoost.IsBoostRequested != enableBoost)
        {
          flag = true;
          entityWithBoost.SetBoosted(enableBoost);
        }
      }
      int num = 0;
      if (hasSurfacesToModify)
        num = this.quickDeliverRemoveSurfaces(cmd.Area.Value);
      if (flag || num > 0)
        cmd.SetResultSuccess();
      else
        cmd.SetResultError("Nothing to boost or construct");
      this.m_entitiesTmp.Clear();
      this.m_entitiesForQuickDeliverTmp.Clear();
      this.m_entitiesForBoost.Clear();
    }

    private int quickDeliverRemoveSurfaces(RectangleTerrainArea2i area)
    {
      Upoints upoints = this.m_upointsManager.Quantity.Upoints();
      Upoints zero1 = Upoints.Zero;
      Upoints zero2 = Upoints.Zero;
      int num = 0;
      foreach (Tile2iAndIndex enumerateTilesAndIndex in area.EnumerateTilesAndIndices(this.m_terrainManager))
      {
        if (!(upoints < EntitiesCommandsProcessor.UnityPerSurfaceTile))
        {
          Option<SurfaceDesignation> designationAt = this.m_surfaceDesignationsManager.GetDesignationAt(enumerateTilesAndIndex.TileCoord);
          if (!designationAt.IsNone && !designationAt.Value.IsFulfilled)
          {
            SurfaceDesignation designation = designationAt.Value;
            if (!designation.IsPlacing)
            {
              if (clearSurfaceTileIfCan(enumerateTilesAndIndex))
              {
                upoints -= EntitiesCommandsProcessor.UnityPerSurfaceTile;
                zero2 += EntitiesCommandsProcessor.UnityPerSurfaceTile;
                ++num;
              }
            }
            else
            {
              bool flag = clearSurfaceTileIfCan(enumerateTilesAndIndex);
              TerrainTileSurfaceProto proto = designation.GetSurfaceAt(enumerateTilesAndIndex.TileCoord).ResolveToProto(this.m_terrainManager);
              if (this.m_assetTransactionManager.CanRemoveProduct(proto.CostPerTile) && this.m_surfaceDesignationsManager.TryAddSurface(enumerateTilesAndIndex, designation))
              {
                this.m_assetTransactionManager.TryRemoveProduct(proto.CostPerTile, new DestroyReason?(DestroyReason.Construction));
                flag = true;
              }
              if (flag)
              {
                upoints -= EntitiesCommandsProcessor.UnityPerSurfaceTile;
                zero1 += EntitiesCommandsProcessor.UnityPerSurfaceTile;
                ++num;
              }
            }
          }
        }
        else
          break;
      }
      if (zero1.IsPositive)
        this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.QuickBuild, zero1);
      if (zero2.IsPositive)
        this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.QuickRemove, zero2);
      return num;

      bool clearSurfaceTileIfCan(Tile2iAndIndex tileCoord)
      {
        TileSurfaceData tileSurfaceData;
        if (!this.m_terrainManager.TryGetTileSurface(tileCoord.Index, out tileSurfaceData) || tileSurfaceData.IsAutoPlaced)
          return false;
        TerrainTileSurfaceProto proto = tileSurfaceData.ResolveToProto(this.m_terrainManager);
        this.m_surfaceDesignationsManager.ClearCustomSurface(tileCoord, out bool _);
        this.m_assetTransactionManager.StoreProduct(proto.CostPerTile, new CreateReason?(CreateReason.Deconstruction));
        return true;
      }
    }

    public static void EvaluateUnitySpending(
      bool hasSurfacesToModify,
      Lyst<IStaticEntity> entities,
      Lyst<IStaticEntity> forQuickDeliver,
      Lyst<IEntityWithBoost> forBoost,
      out bool enableBoost,
      out int noOfEntitiesBannedForDelivery)
    {
      bool flag = false;
      noOfEntitiesBannedForDelivery = 0;
      foreach (IStaticEntity entity in entities)
      {
        if (entity != null && !entity.IsDestroyed)
        {
          if (entity.ConstructionProgress.HasValue && !entity.ConstructionProgress.Value.IsAllowedToFinish())
          {
            if (!entity.ConstructionProgress.Value.IsDeconstruction && entity.Prototype.HasParam<DisableQuickBuildParam>())
              ++noOfEntitiesBannedForDelivery;
            else
              forQuickDeliver.Add(entity);
          }
          else if (forQuickDeliver.IsEmpty && !hasSurfacesToModify && entity is IEntityWithBoost entityWithBoost && entityWithBoost.BoostCost.HasValue)
          {
            flag |= entityWithBoost.IsBoostRequested;
            forBoost.Add(entityWithBoost);
          }
        }
      }
      if (forQuickDeliver.IsNotEmpty)
        forBoost.Clear();
      enableBoost = !flag;
    }

    public void Invoke(TryTransformEntityCmd cmd)
    {
      ILayoutEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<ILayoutEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("Failed to find static entity {0}.", (object) cmd.EntityId));
      else if (entity.Prototype.CannotBeReflected && cmd.Flip)
      {
        cmd.SetResultError("Cannot flip, not allowed");
      }
      else
      {
        EntityValidationResult validationResult1 = this.m_entitiesManager.CanCutEntity((IStaticEntity) entity);
        if (validationResult1.IsError)
        {
          cmd.SetResultError(validationResult1.ErrorMessage);
        }
        else
        {
          TileTransform transform = new TileTransform(entity.Transform.Position, cmd.Rotate ? entity.Transform.Rotation.RotatedMinus90 : entity.Transform.Rotation, cmd.Flip ? !entity.Transform.IsReflected : entity.Transform.IsReflected);
          StaticEntity entity1;
          if (!this.m_entitiesCreator.TryCreateStaticEntity((StaticEntityProto) entity.Prototype, transform, out entity1))
            cmd.SetResultError(string.Format("Failed to instantiate entity '{0}'.", (object) entity.Id));
          else if (!(entity1 is ILayoutEntity layoutEntity))
          {
            cmd.SetResultError(string.Format("'{0}' ({1}) is not a layout entity.", (object) entity1, (object) entity1.GetType().Name));
          }
          else
          {
            ILayoutEntityProto prototype = (ILayoutEntityProto) entity.Prototype;
            bool enableMiniZipperPlacement = prototype != null && prototype.AutoBuildMiniZippers;
            EntityValidationResult validationResult2 = this.m_entitiesManager.CanAdd((IEntityAddRequest) this.m_addRequestFactory.CreateRequestFor<LayoutEntityProto>(layoutEntity.Prototype, new EntityAddRequestData(layoutEntity.Transform, enableMiniZipperPlacement, (Predicate<EntityId>) (id => id == entity.Id)), EntityAddReason.Move));
            if (validationResult2.IsError)
            {
              cmd.SetResultError(validationResult2.ErrorMessage);
              ((IEntityFriend) entity1).Destroy();
            }
            else
            {
              EntityConfigData configFrom = this.m_configCloneHelper.CreateConfigFrom((IEntity) entity);
              this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) entity, EntityRemoveReason.Remove);
              EntityValidationResult validationResult3 = this.m_entitiesManager.TryAddEntity((IEntity) entity1);
              if (validationResult3.IsError)
              {
                ((IEntityFriend) entity1).Destroy();
                cmd.SetResultError(validationResult3.ErrorMessage);
              }
              else
              {
                if (enableMiniZipperPlacement)
                  this.buildMiniZippersAtPorts(entity1);
                this.m_configCloneHelper.ApplyConfigTo(configFrom, (IEntity) entity1);
                cmd.SetResultSuccess();
              }
            }
          }
        }
      }
    }

    public void Invoke(SetEntityNameCmd cmd)
    {
      Entity entity1;
      if (!this.m_entitiesManager.TryGetEntity<Entity>(cmd.EntityId, out entity1))
        cmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) cmd.EntityId));
      else if (!(entity1 is IEntityWithCustomTitle entity2))
      {
        cmd.SetResultError(string.Format("Entity '{0}' cannot be renamed.", (object) cmd.EntityId));
      }
      else
      {
        entity2.SetCustomTitle(cmd.Title);
        cmd.SetResultSuccess();
      }
    }

    static EntitiesCommandsProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntitiesCommandsProcessor.UnityPerSurfaceTile = 0.02.Upoints();
    }
  }
}
