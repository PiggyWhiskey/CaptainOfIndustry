// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntitiesCloneConfigHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntitiesCloneConfigHelper
  {
    [ThreadStatic]
    private static Lyst<TransportPillar> s_pillars;
    public readonly ConfigSerializationContext ConfigContext;
    private readonly ElectricityManager m_electricityManager;
    private readonly ConstructionManager m_constructionManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TransportsManager m_transportsManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly Dict<EntityId, EntityId> m_entitiesMapCache;
    private readonly Set<EntityId> m_failedToMap;

    public EntitiesCloneConfigHelper(
      ConfigSerializationContext configSerializationContext,
      ElectricityManager electricityManager,
      ConstructionManager constructionManager,
      EntitiesManager entitiesManager,
      TransportsManager transportsManager,
      IVehiclesManager vehiclesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_entitiesMapCache = new Dict<EntityId, EntityId>();
      this.m_failedToMap = new Set<EntityId>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigContext = configSerializationContext;
      this.m_electricityManager = electricityManager;
      this.m_constructionManager = constructionManager;
      this.m_entitiesManager = entitiesManager;
      this.m_transportsManager = transportsManager;
      this.m_vehiclesManager = vehiclesManager;
    }

    internal void ApplyConfigToAll(
      IIndexable<Option<IEntity>> newEntities,
      IIndexable<EntityConfigData> data)
    {
      if (newEntities.Count != data.Count)
      {
        Log.Error(string.Format("Count mismatch {0} != {1}", (object) newEntities.Count, (object) data.Count));
      }
      else
      {
        this.m_entitiesMapCache.Clear();
        this.m_failedToMap.Clear();
        Option<IEntity> newEntity;
        for (int index = 0; index < newEntities.Count; ++index)
        {
          EntityId? originalEntityId = data[index].OriginalEntityId;
          if (originalEntityId.HasValue)
          {
            newEntity = newEntities[index];
            if (newEntity.HasValue)
              this.m_entitiesMapCache.Add(originalEntityId.Value, newEntities[index].Value.Id);
            else
              this.m_failedToMap.Add(originalEntityId.Value);
          }
        }
        for (int index = 0; index < newEntities.Count; ++index)
          data[index].MapEntitiesIds(this.m_entitiesMapCache, this.m_failedToMap);
        for (int index = 0; index < newEntities.Count; ++index)
        {
          newEntity = newEntities[index];
          if (newEntity.HasValue)
            this.ApplyConfigTo(data[index], newEntities[index].Value);
        }
      }
    }

    /// <summary>
    /// Note: This method should not be used when placing multiple entities
    /// that might have assignments between each other, for that use ApplyConfigToAll.
    /// </summary>
    public void ApplyConfigTo(EntityConfigData data, IEntity targetEntity)
    {
      if (data.EntityType != targetEntity.GetType())
        return;
      if (targetEntity.CanBePaused)
        targetEntity.SetPaused(data.IsPaused.GetValueOrDefault());
      switch (targetEntity)
      {
        case IEntityWithLogisticsControl logisticsControl1:
          EntityLogisticsMode? logisticsInputMode = data.LogisticsInputMode;
          if (logisticsInputMode.HasValue)
            logisticsControl1.SetLogisticsInputMode(logisticsInputMode.Value);
          EntityLogisticsMode? logisticsOutputMode = data.LogisticsOutputMode;
          if (logisticsOutputMode.HasValue)
          {
            logisticsControl1.SetLogisticsOutputMode(logisticsOutputMode.Value);
            break;
          }
          break;
        case IEntityWithSimpleLogisticsControl logisticsControl2:
          bool? logisticsInputDisabled = data.IsLogisticsInputDisabled;
          if (logisticsInputDisabled.HasValue)
            logisticsControl2.SetLogisticsInputDisabled(logisticsInputDisabled.Value);
          bool? logisticsOutputDisabled = data.IsLogisticsOutputDisabled;
          if (logisticsOutputDisabled.HasValue)
          {
            logisticsControl2.SetLogisticsOutputDisabled(logisticsOutputDisabled.Value);
            break;
          }
          break;
      }
      ImmutableArray<Proto>? assignedVehicles = data.AssignedVehicles;
      if (assignedVehicles.HasValue && targetEntity is IEntityAssignedWithVehicles entity1)
      {
        foreach (Proto proto in assignedVehicles.Value)
        {
          if (proto is DynamicGroundEntityProto groundEntityProto && entity1.CanVehicleBeAssigned((DynamicEntityProto) groundEntityProto))
            entity1.AssignVehicle(this.m_vehiclesManager, (DynamicEntityProto) groundEntityProto);
        }
      }
      if (!(targetEntity is Transport) && targetEntity is IEntityWithGeneralPriority entity2 && entity2.IsGeneralPriorityVisible)
      {
        int? generalPriority = data.GeneralPriority;
        if (generalPriority.HasValue)
          entity2.SetGeneralPriority(generalPriority.Value);
      }
      if (targetEntity is IEntityWithMultipleProductsToAssign productsToAssign)
      {
        ImmutableArray<Option<ProductProto>>? multipleBuffers = data.MultipleBuffers;
        if (multipleBuffers.HasValue)
        {
          ImmutableArray<Option<ProductProto>> immutableArray = multipleBuffers.Value;
          if (immutableArray.Length == productsToAssign.BuffersPerSlot.Count)
          {
            for (int index = 0; index < immutableArray.Length; ++index)
              productsToAssign.SetProduct(immutableArray[index], index, true);
          }
        }
      }
      if (targetEntity is IElectricityGeneratingEntity entity3)
      {
        int? generationPriority1 = data.ElectricityGenerationPriority;
        if (generationPriority1.HasValue)
        {
          int generationPriority2 = entity3.ElectricityGenerator.GenerationPriority;
          int? nullable = generationPriority1;
          int valueOrDefault = nullable.GetValueOrDefault();
          if (!(generationPriority2 == valueOrDefault & nullable.HasValue))
            this.m_electricityManager.SetGenerationPriorityFor((IEntity) entity3, generationPriority1.Value);
        }
        bool valueOrDefault1 = data.IsElectricitySurplusGenerator.GetValueOrDefault();
        this.m_electricityManager.SetIsSurplusGenerator(entity3, valueOrDefault1);
      }
      if (targetEntity is IElectricityConsumingEntity consumer && consumer.ElectricityConsumer.HasValue && consumer.IsGeneralPriorityVisible)
      {
        bool valueOrDefault = data.IsElectricitySurplusConsumer.GetValueOrDefault();
        this.m_electricityManager.SetIsSurplusConsumer(consumer, valueOrDefault);
      }
      if (data.CustomTitle.HasValue && targetEntity.GetType() == data.EntityType && targetEntity is IEntityWithCustomTitle entity4)
        entity4.SetCustomTitle(data.CustomTitle.Value);
      if (targetEntity is IEntityAssignedAsOutput entity7)
      {
        ImmutableArray<EntityId>? assignedInputs = data.AssignedInputs;
        if (assignedInputs.HasValue)
        {
          foreach (IEntityAssignedAsInput entity5 in entity7.AssignedInputs.ToArray<IEntityAssignedAsInput>())
          {
            if (!assignedInputs.Value.Contains(entity5.Id))
            {
              entity5.UnassignStaticOutputEntity(entity7);
              entity7.UnassignStaticInputEntity(entity5);
            }
          }
          foreach (EntityId id in assignedInputs.Value)
          {
            IEntityAssignedAsInput entity6;
            if (this.m_entitiesManager.TryGetEntity<IEntityAssignedAsInput>(id, out entity6) && entity6.CanBeAssignedWithOutput(entity7) && entity7.CanBeAssignedWithInput(entity6))
            {
              entity6.AssignStaticOutputEntity(entity7);
              entity7.AssignStaticInputEntity(entity6);
            }
          }
        }
      }
      if (targetEntity is IEntityAssignedAsInput entity10)
      {
        bool? nonAssignedOutput = data.AllowNonAssignedOutput;
        if (nonAssignedOutput.HasValue)
          entity10.AllowNonAssignedOutput = nonAssignedOutput.Value;
        ImmutableArray<EntityId>? assignedOutputs = data.AssignedOutputs;
        if (assignedOutputs.HasValue)
        {
          foreach (IEntityAssignedAsOutput entity8 in entity10.AssignedOutputs.ToArray<IEntityAssignedAsOutput>())
          {
            if (!assignedOutputs.Value.Contains(entity8.Id))
            {
              entity8.UnassignStaticInputEntity(entity10);
              entity10.UnassignStaticOutputEntity(entity8);
            }
          }
          foreach (EntityId id in assignedOutputs.Value)
          {
            IEntityAssignedAsOutput entity9;
            if (this.m_entitiesManager.TryGetEntity<IEntityAssignedAsOutput>(id, out entity9) && entity9.CanBeAssignedWithInput(entity10) && entity10.CanBeAssignedWithOutput(entity9))
            {
              entity9.AssignStaticInputEntity(entity10);
              entity10.AssignStaticOutputEntity(entity9);
            }
          }
        }
      }
      if (targetEntity is IStaticEntity entity11)
      {
        bool? constructionPaused = data.IsConstructionPaused;
        if (constructionPaused.HasValue && constructionPaused.Value)
          this.m_constructionManager.TrySetPaused(entity11);
      }
      if (!(targetEntity is IEntityWithCloneableConfig withCloneableConfig))
        return;
      try
      {
        withCloneableConfig.ApplyConfig(data);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "ApplyConfigTo failed for entity '" + targetEntity.GetTitle() + "'");
      }
    }

    public EntityConfigData CreateConfigFrom(IEntity sourceEntity)
    {
      EntityConfigData data = new EntityConfigData(sourceEntity.Id, (IProto) sourceEntity.Prototype, this.ConfigContext);
      if (sourceEntity is ILayoutEntity layoutEntity1)
        data.Transform = new TileTransform?(layoutEntity1.Transform);
      if (sourceEntity is Transport transport)
      {
        data.Trajectory = (Option<TransportTrajectory>) transport.Trajectory;
        if (EntitiesCloneConfigHelper.s_pillars == null)
          EntitiesCloneConfigHelper.s_pillars = new Lyst<TransportPillar>();
        EntitiesCloneConfigHelper.s_pillars.Clear();
        transport.TransportManager.FindAttachedPillars(transport, EntitiesCloneConfigHelper.s_pillars);
        if (EntitiesCloneConfigHelper.s_pillars.Count > 0)
        {
          data.Pillars = new ImmutableArray<Tile2i>?(EntitiesCloneConfigHelper.s_pillars.ToImmutableArray<Tile2i>((Func<TransportPillar, Tile2i>) (x => x.CenterTile.Xy)));
          EntitiesCloneConfigHelper.s_pillars.Clear();
        }
      }
      if (sourceEntity.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated && sourceEntity is LayoutEntity layoutEntity2)
      {
        if (EntitiesCloneConfigHelper.s_pillars == null)
          EntitiesCloneConfigHelper.s_pillars = new Lyst<TransportPillar>();
        EntitiesCloneConfigHelper.s_pillars.Clear();
        this.m_transportsManager.FindAttachedPillars(layoutEntity2, EntitiesCloneConfigHelper.s_pillars);
        if (EntitiesCloneConfigHelper.s_pillars.Count > 0)
        {
          data.Pillars = new ImmutableArray<Tile2i>?(EntitiesCloneConfigHelper.s_pillars.ToImmutableArray<Tile2i>((Func<TransportPillar, Tile2i>) (x => x.CenterTile.Xy)));
          EntitiesCloneConfigHelper.s_pillars.Clear();
        }
      }
      if (sourceEntity.CanBePaused && sourceEntity.IsPaused)
        data.IsPaused = new bool?(true);
      if (sourceEntity is IEntityWithLogisticsControl logisticsControl2)
      {
        data.LogisticsInputMode = new EntityLogisticsMode?(logisticsControl2.LogisticsInputMode);
        data.LogisticsOutputMode = new EntityLogisticsMode?(logisticsControl2.LogisticsOutputMode);
      }
      else if (sourceEntity is IEntityWithSimpleLogisticsControl logisticsControl1)
      {
        data.IsLogisticsInputDisabled = new bool?(logisticsControl1.IsLogisticsInputDisabled);
        data.IsLogisticsOutputDisabled = new bool?(logisticsControl1.IsLogisticsOutputDisabled);
      }
      if (sourceEntity is IEntityAssignedWithVehicles entity && entity.VehiclesTotal() > 0)
        data.AssignedVehicles = new ImmutableArray<Proto>?(entity.AllVehicles.Select<Vehicle, Proto>((Func<Vehicle, Proto>) (x => (Proto) x.Prototype)).ToImmutableArray<Proto>());
      if (sourceEntity is IEntityWithGeneralPriority withGeneralPriority && withGeneralPriority.IsGeneralPriorityVisible)
        data.GeneralPriority = new int?(withGeneralPriority.GeneralPriority);
      if (sourceEntity is IEntityWithMultipleProductsToAssign productsToAssign)
        data.MultipleBuffers = new ImmutableArray<Option<ProductProto>>?(productsToAssign.BuffersPerSlot.Select<Option<ProductBuffer>, Option<ProductProto>>((Func<Option<ProductBuffer>, Option<ProductProto>>) (x =>
        {
          ProductProto product = x.ValueOrNull?.Product;
          return product == null ? Option<ProductProto>.None : (Option<ProductProto>) product;
        })).ToImmutableArray<Option<ProductProto>>());
      if (sourceEntity is IElectricityGeneratingEntity generatingEntity)
      {
        data.ElectricityGenerationPriority = new int?(generatingEntity.ElectricityGenerator.GenerationPriority);
        if (generatingEntity.ElectricityGenerator.IsSurplusGenerator)
          data.IsElectricitySurplusGenerator = new bool?(true);
      }
      if (sourceEntity is IElectricityConsumingEntity electricityConsumingEntity && electricityConsumingEntity.IsGeneralPriorityVisible)
      {
        IElectricityConsumerReadonly valueOrNull = electricityConsumingEntity.ElectricityConsumer.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.IsSurplusConsumer ? 1 : 0) : 0) != 0)
          data.IsElectricitySurplusConsumer = new bool?(true);
      }
      if (sourceEntity is IEntityWithCustomTitle entityWithCustomTitle)
        data.CustomTitle = entityWithCustomTitle.CustomTitle;
      if (sourceEntity is IEntityAssignedAsInput entityAssignedAsInput)
      {
        data.AllowNonAssignedOutput = new bool?(entityAssignedAsInput.AllowNonAssignedOutput);
        if (entityAssignedAsInput.AssignedOutputs.IsNotEmpty<IEntityAssignedAsOutput>())
          data.AssignedOutputs = new ImmutableArray<EntityId>?(entityAssignedAsInput.AssignedOutputs.Select<IEntityAssignedAsOutput, EntityId>((Func<IEntityAssignedAsOutput, EntityId>) (x => x.Id)).ToImmutableArray<EntityId>());
      }
      if (sourceEntity is IEntityAssignedAsOutput assignedAsOutput && assignedAsOutput.AssignedInputs.IsNotEmpty<IEntityAssignedAsInput>())
        data.AssignedInputs = new ImmutableArray<EntityId>?(assignedAsOutput.AssignedInputs.Select<IEntityAssignedAsInput, EntityId>((Func<IEntityAssignedAsInput, EntityId>) (x => x.Id)).ToImmutableArray<EntityId>());
      if (sourceEntity is IStaticEntity staticEntity)
      {
        EntityConfigData entityConfigData = data;
        IEntityConstructionProgress valueOrNull = staticEntity.ConstructionProgress.ValueOrNull;
        bool? nullable = new bool?(valueOrNull != null && valueOrNull.IsPaused);
        entityConfigData.IsConstructionPaused = nullable;
      }
      if (sourceEntity is IEntityWithCloneableConfig withCloneableConfig)
      {
        try
        {
          withCloneableConfig.AddToConfig(data);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "CreateConfigFrom failed for entity '" + sourceEntity.GetTitle() + "'");
        }
      }
      return data;
    }

    public bool TryCopyConfigFromTo(IEntity sourceEntity, IEntity targetEntity)
    {
      if (sourceEntity.GetType() != targetEntity.GetType())
        return false;
      if (sourceEntity == targetEntity)
        return true;
      this.ApplyConfigTo(this.CreateConfigFrom(sourceEntity), targetEntity);
      return true;
    }
  }
}
