// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ConstructionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Handles construction and deconstruction of static entities.
  /// </summary>
  /// <remarks>
  /// Static entity construction has following states and events:
  /// <code>
  /// ConstructionState.NotInitialized
  /// * Initialize() =&gt; NotStarted
  /// 
  /// ConstructionState.NotStarted
  /// * StartConstruction() =&gt; InConstruction
  /// * MarkConstructed() =&gt; Constructed with EntityConstructed invoked
  /// 
  /// ConstructionState.InConstruction
  /// * ContinueConstruction() =&gt; InConstruction -or- Constructed (EntityConstructed invoked)
  /// * StartDeconstruction() =&gt; InDeconstruction -or- Deconstructed (no EntityStartedDeconstruction)
  /// * MarkConstructed() =&gt; Constructed (EntityConstructed invoked)
  /// 
  /// ConstructionState.Constructed
  /// * StartDeconstruction() =&gt; InDeconstruction (EntityStartedDeconstruction invoked)
  /// * MarkDeconstructed() =&gt; Deconstructed (EntityStartedDeconstruction invoked if not already in deconstruction)
  /// 
  /// ConstructionState.InDeconstruction
  /// * ContinueDeconstruction() =&gt; InDeconstruction -or- Deconstructed
  /// * StartConstruction() =&gt; InConstruction -or- Constructed (EntityConstructed invoked)
  /// * MarkDeconstructed() =&gt; Deconstructed
  /// </code>
  /// Note that <c>EntityConstructed</c> and <c>EntityStartedDeconstruction</c> events are made symmetric.
  /// This means that the <c>EntityStartedDeconstruction</c> event is only invoked when the entity is entering
  /// deconstruction from constructed state.
  /// </remarks>
  [MemberRemovedInSaveVersion("DeconstructionRatio", 140, typeof (Percent), 0, false)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ConstructionManager : 
    IConstructionManager,
    ICommandProcessor<FinishBuildOfStaticEntityCmd>,
    IAction<FinishBuildOfStaticEntityCmd>,
    ICommandProcessor<SetConstructionPriorityCmd>,
    IAction<SetConstructionPriorityCmd>,
    ICommandProcessor<SetConstructionPausedCmd>,
    IAction<SetConstructionPausedCmd>
  {
    public static readonly Duration EXTRA_CONSTRUCTION_DURATION;
    private static readonly Percent PROGRESS_ON_QUICK_BUILD;
    private readonly EntitiesManager m_entitiesManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly GlobalPrioritiesManager m_prioritiesManager;
    private readonly IProductsManager m_productsManager;
    private readonly IAssetTransactionManager m_assetManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffRegConstruction;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly PlanningModeManager m_planningModeManager;
    private readonly UpointsManager m_upointsManager;
    private readonly LazyResolve<TerrainManager> m_terrainManager;
    private readonly UpgradesManager m_upgradesManager;
    private readonly LazyResolve<ITreesManager> m_treeManager;
    private readonly LazyResolve<TerrainPropsManager> m_terrainPropsManager;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_deconstructionRefundMult;
    private readonly Event<IStaticEntity> m_entityConstructed;
    private readonly Event<IStaticEntity, bool> m_entityPauseStateChanged;
    private readonly Event<IStaticEntity> m_onResetConstructionAnimationState;
    private readonly Event<IStaticEntity> m_entityConstructionNearlyFinished;
    private readonly Event<IStaticEntity> m_entityStartedDeconstruction;
    private readonly Event<IStaticEntity, ConstructionState> m_entityConstructionStateChanged;
    private readonly Dict<IStaticEntity, EntityConstructionProgress> m_ongoingConstructions;
    private readonly Dict<IStaticEntity, EntityConstructionProgress> m_ongoingDeconstructions;
    private readonly Lyst<IStaticEntity> m_pendingDeconstructionsToStart;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<IStaticEntity> m_iterationCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Percent DeconstructionRatio => this.m_deconstructionRefundMult.Value;

    public IEvent<IStaticEntity> EntityConstructed
    {
      get => (IEvent<IStaticEntity>) this.m_entityConstructed;
    }

    public IEvent<IStaticEntity, bool> EntityPauseStateChanged
    {
      get => (IEvent<IStaticEntity, bool>) this.m_entityPauseStateChanged;
    }

    public IEvent<IStaticEntity> OnResetConstructionAnimationState
    {
      get => (IEvent<IStaticEntity>) this.m_onResetConstructionAnimationState;
    }

    /// <summary>
    /// Invoked when construction passes over (or deconstruction passes below) a threshold.
    /// This should be the point where construction cubes are fully covering the entity so rendering can for
    /// example swap models.
    /// 
    /// Note: This event will not be invoked if construction manager does not get to see this progress transition
    /// or if construction state is also changing on the same tick.
    /// </summary>
    public IEvent<IStaticEntity> EntityConstructionNearlyFinished
    {
      get => (IEvent<IStaticEntity>) this.m_entityConstructionNearlyFinished;
    }

    public IEvent<IStaticEntity> EntityStartedDeconstruction
    {
      get => (IEvent<IStaticEntity>) this.m_entityStartedDeconstruction;
    }

    public IEvent<IStaticEntity, ConstructionState> EntityConstructionStateChanged
    {
      get => (IEvent<IStaticEntity, ConstructionState>) this.m_entityConstructionStateChanged;
    }

    public ConstructionManager(
      EntitiesManager entitiesManager,
      INotificationsManager notificationsManager,
      GlobalPrioritiesManager prioritiesManager,
      ISimLoopEvents simLoopEvents,
      IPropertiesDb propsDb,
      IProductsManager productsManager,
      IAssetTransactionManager assetManager,
      IVehicleBuffersRegistry vehicleBuffRegConstruction,
      IInstaBuildManager instaBuildManager,
      PlanningModeManager planningModeManager,
      UpointsManager upointsManager,
      LazyResolve<TerrainManager> terrainManager,
      UpgradesManager upgradesManager,
      LazyResolve<ITreesManager> treeManager,
      LazyResolve<TerrainPropsManager> terrainPropsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_entityPauseStateChanged = new Event<IStaticEntity, bool>();
      this.m_onResetConstructionAnimationState = new Event<IStaticEntity>();
      this.m_entityConstructionNearlyFinished = new Event<IStaticEntity>();
      this.m_entityConstructionStateChanged = new Event<IStaticEntity, ConstructionState>();
      this.m_ongoingConstructions = new Dict<IStaticEntity, EntityConstructionProgress>();
      this.m_ongoingDeconstructions = new Dict<IStaticEntity, EntityConstructionProgress>();
      this.m_pendingDeconstructionsToStart = new Lyst<IStaticEntity>();
      this.m_iterationCache = new Lyst<IStaticEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_notificationsManager = notificationsManager;
      this.m_prioritiesManager = prioritiesManager;
      this.m_productsManager = productsManager;
      this.m_assetManager = assetManager;
      this.m_vehicleBuffRegConstruction = vehicleBuffRegConstruction;
      this.m_instaBuildManager = instaBuildManager;
      this.m_planningModeManager = planningModeManager;
      this.m_upointsManager = upointsManager;
      this.m_terrainManager = terrainManager;
      this.m_upgradesManager = upgradesManager;
      this.m_treeManager = treeManager;
      this.m_terrainPropsManager = terrainPropsManager;
      this.m_entityConstructed = new Event<IStaticEntity>();
      this.m_entityStartedDeconstruction = new Event<IStaticEntity>();
      this.m_deconstructionRefundMult = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.DeconstructionRefundMultiplier);
      this.m_entitiesManager.StaticEntityRemoved.Add<ConstructionManager>(this, new Action<IStaticEntity>(this.onEntityRemoved));
      simLoopEvents.Update.Add<ConstructionManager>(this, new Action(this.simUpdate));
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<ConstructionManager>(this, "m_deconstructionRefundMult", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.DeconstructionRefundMultiplier));
    }

    private void simUpdate()
    {
      this.m_iterationCache.Clear();
      this.m_iterationCache.AddRange((IEnumerable<IStaticEntity>) this.m_ongoingConstructions.Keys);
      foreach (IStaticEntity staticEntity in this.m_iterationCache)
        this.continueConstruction(staticEntity, this.m_ongoingConstructions[staticEntity]);
      this.m_iterationCache.Clear();
      this.m_iterationCache.AddRange(this.m_pendingDeconstructionsToStart);
      foreach (IStaticEntity staticEntity in this.m_iterationCache)
      {
        if (staticEntity.ConstructionState != ConstructionState.PendingDeconstruction)
          this.m_pendingDeconstructionsToStart.Remove(staticEntity);
        else if (staticEntity.CanMoveFromPendingDeconstruction())
          staticEntity.StartDeconstructionIfCan();
      }
      this.m_iterationCache.Clear();
      this.m_iterationCache.AddRange((IEnumerable<IStaticEntity>) this.m_ongoingDeconstructions.Keys);
      foreach (IStaticEntity staticEntity in this.m_iterationCache)
        this.continueDeconstruction(staticEntity, this.m_ongoingDeconstructions[staticEntity]);
    }

    public AssetValue GetSellValue(AssetValue originalValue)
    {
      return originalValue.Apply(this.DeconstructionRatio);
    }

    public void Initialize(IStaticEntity staticEntity) => this.StartConstruction(staticEntity);

    public void ResetConstructionAnimationState(IStaticEntity entity)
    {
      this.m_onResetConstructionAnimationState.Invoke(entity);
    }

    public void IncreaseDeconstructionRatio(Percent increase)
    {
    }

    private void onEntityRemoved(IStaticEntity entity)
    {
      this.m_productsManager.ProductDestroyed(this.CancelConstructionAndReturnBuffers(entity), DestroyReason.Construction);
      if (entity.ConstructionState != ConstructionState.Deconstructed)
        this.SetTerrainUnderCustom(entity, 0, 1, 1, true, entity.DoNotAdjustTerrainDuringConstruction);
      Tile3i centerTile = entity.CenterTile;
      TerrainManager terrainManager = this.m_terrainManager.Value;
      Assert.That<ImmutableArray<OccupiedTileRelative>>(entity.OccupiedTiles).IsNotEmpty<OccupiedTileRelative, IStaticEntity>("Empty occupied tiles for entity '{0}'", entity);
    }

    public void StartConstruction(IStaticEntity staticEntity)
    {
      AssetValue preExistingProducts = AssetValue.Empty;
      Percent? currentStepsPerc = new Percent?();
      if (staticEntity.ConstructionState == ConstructionState.PendingDeconstruction)
      {
        this.m_pendingDeconstructionsToStart.TryRemove(staticEntity);
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Constructed);
      }
      else
      {
        bool allowFreeRebuild = false;
        EntityConstructionProgress constructionProgress1;
        if (this.m_ongoingDeconstructions.TryGetValue(staticEntity, out constructionProgress1))
        {
          currentStepsPerc = new Percent?(constructionProgress1.Progress);
          preExistingProducts = constructionProgress1.GetAssetValueOfBuffers();
          bool flag = true;
          foreach (ProductBuffer constructionBuffer in constructionProgress1.ConstructionBuffers)
          {
            flag &= constructionBuffer.IsFull();
            constructionBuffer.Clear();
          }
          this.destroyDeconstructionState(staticEntity, constructionProgress1, new DestroyReason?());
          if (flag && constructionProgress1.AllowFreeRebuild)
            allowFreeRebuild = true;
          if (constructionProgress1.CurrentSteps >= constructionProgress1.MaxSteps & allowFreeRebuild)
          {
            this.finishConstruction(staticEntity, constructionProgress1);
            foreach (ProductQuantity product in preExistingProducts.Products)
              this.m_productsManager.ProductDestroyed(product, DestroyReason.Construction);
            return;
          }
        }
        ImmutableArray<ProductBuffer> buffers;
        if (allowFreeRebuild)
        {
          buffers = ImmutableArray<ProductBuffer>.Empty;
          this.m_productsManager.ProductDestroyed(preExistingProducts, DestroyReason.Construction);
        }
        else
        {
          buffers = staticEntity.ConstructionCost.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (pq =>
          {
            Assert.That<ProductQuantity>(pq).IsNotEmpty("Empty static entity cost.");
            ProductBuffer buffer = new ProductBuffer(pq.Quantity, pq.Product);
            if (preExistingProducts.IsNotEmpty)
            {
              Quantity quantity = preExistingProducts.GetQuantityOf(pq.Product).Min(buffer.Capacity);
              buffer.StoreExactly(quantity);
              preExistingProducts -= quantity.Of(pq.Product);
            }
            return buffer;
          })).ToImmutableArray();
          if (preExistingProducts.IsNotEmpty)
          {
            Log.Warning(string.Format("Failed to store all products from deconstruction buffers of '{0}' ", (object) staticEntity) + string.Format("into construction buffers, extra: {0}", (object) preExistingProducts));
            this.m_productsManager.ProductDestroyed(preExistingProducts, DestroyReason.Cleared);
          }
        }
        EntityConstructionProgress constructionProgress2 = new EntityConstructionProgress(staticEntity, this.m_notificationsManager, this.m_prioritiesManager, buffers, allowFreeRebuild ? AssetValue.Empty : staticEntity.ConstructionCost.TakeNonVirtualOnly(), staticEntity.Prototype.ConstructionDurationPerProduct, ConstructionManager.EXTRA_CONSTRUCTION_DURATION, currentStepsPerc: currentStepsPerc, allowFreeRebuild: allowFreeRebuild);
        this.m_ongoingConstructions.Add(staticEntity, constructionProgress2);
        if (this.m_planningModeManager.IsPlanningModeEnabled && !(staticEntity is TransportPillar) && !staticEntity.Prototype.DoNotStartConstructionAutomatically)
        {
          constructionProgress2.IsPaused = true;
          this.m_entityPauseStateChanged.Invoke(staticEntity, true);
        }
        else
          constructionProgress2.RegisterBuffers(this.m_vehicleBuffRegConstruction);
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.InConstruction);
      }
    }

    private void continueConstruction(IStaticEntity staticEntity, EntityConstructionProgress state)
    {
      Assert.That<bool>(staticEntity.IsDestroyed).IsFalse();
      Assert.That<ConstructionState>(staticEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.InConstruction);
      if (state.IsPaused)
        return;
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        state.CheatFull(this.m_productsManager);
        state.MakeFinished();
        finishConstructionInternal();
      }
      else
      {
        bool isNearlyFinished = state.IsNearlyFinished;
        if (!state.TryMakeStep())
          return;
        this.SetTerrainUnder(staticEntity, (ConstructionProgress) state);
        if (state.IsDone)
        {
          finishConstructionInternal();
        }
        else
        {
          if (isNearlyFinished == state.IsNearlyFinished)
            return;
          this.m_entityConstructionNearlyFinished.Invoke(state.Entity);
        }
      }

      void finishConstructionInternal()
      {
        Assert.That<int>(state.CurrentSteps).IsEqualTo(state.MaxSteps);
        this.destroyConstructionState(staticEntity, state, new DestroyReason?(DestroyReason.Construction));
        this.finishConstruction(staticEntity, state);
      }
    }

    private void finishConstruction(
      IStaticEntity staticEntity,
      EntityConstructionProgress state,
      bool doNotAdjustTerrainHeight = false)
    {
      Assert.That<Dict<IStaticEntity, EntityConstructionProgress>>(this.m_ongoingConstructions).NotContainsKey<IStaticEntity, EntityConstructionProgress>(staticEntity);
      this.SetTerrainUnder(staticEntity, (ConstructionProgress) state, doNotAdjustTerrainHeight);
      this.SetConstructionStateForEntity(staticEntity, ConstructionState.Constructed);
      this.m_entityConstructed.Invoke(staticEntity);
    }

    public void MarkConstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotAdjustTerrainHeight = false)
    {
      Assert.That<Dict<IStaticEntity, EntityConstructionProgress>>(this.m_ongoingDeconstructions).NotContainsKey<IStaticEntity, EntityConstructionProgress>(staticEntity);
      EntityConstructionProgress state;
      if (this.m_ongoingConstructions.TryGetValue(staticEntity, out state))
      {
        this.destroyConstructionState(staticEntity, state, new DestroyReason?(DestroyReason.Construction), true);
      }
      else
      {
        if (staticEntity is IUpgradableEntity entity && this.m_upgradesManager.TryFinishUpgradeImmediately(entity, false, out string _))
          return;
        Assert.Fail(string.Format("Entity construction was not started: {0}", (object) staticEntity));
        state = new EntityConstructionProgress(staticEntity, this.m_notificationsManager, this.m_prioritiesManager, ImmutableArray<ProductBuffer>.Empty, AssetValue.Empty, Duration.Zero, 1.Ticks());
      }
      state.SetProgressRaw(Percent.Hundred);
      Assert.That<bool>(state.IsDone).IsTrue();
      state.SetTerrainDisruptionDuringConstruction(disableTerrainDisruption);
      this.finishConstruction(staticEntity, state, doNotAdjustTerrainHeight);
    }

    public void MarkDeconstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotRecoverTerrainHeight = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove)
    {
      Assert.That<Dict<IStaticEntity, EntityConstructionProgress>>(this.m_ongoingConstructions).NotContainsKey<IStaticEntity, EntityConstructionProgress>(staticEntity);
      EntityConstructionProgress state;
      if (this.m_ongoingDeconstructions.TryGetValue(staticEntity, out state))
      {
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
        this.destroyDeconstructionState(staticEntity, state, new DestroyReason?(DestroyReason.Construction), new EntityRemoveReason?(entityRemoveReason));
      }
      else
      {
        Assert.Fail(string.Format("Entity deconstruction was not started: {0}", (object) staticEntity));
        state = new EntityConstructionProgress(staticEntity, this.m_notificationsManager, this.m_prioritiesManager, ImmutableArray<ProductBuffer>.Empty, AssetValue.Empty, Duration.Zero, 1.Ticks(), true);
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Constructed);
        this.m_entityStartedDeconstruction.Invoke(staticEntity);
      }
      state.SetProgressRaw(Percent.Zero);
      Assert.That<bool>(state.IsDone).IsTrue();
      state.SetTerrainDisruptionDuringConstruction(disableTerrainDisruption);
      this.SetTerrainUnder(staticEntity, (ConstructionProgress) state, doNotRecoverTerrainHeight);
    }

    public void MarkForPendingDeconstruction(IStaticEntity staticEntity)
    {
      Assert.That<ConstructionState>(staticEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.Constructed);
      if (this.m_pendingDeconstructionsToStart.Contains(staticEntity))
      {
        Assert.Fail(string.Format("Deconstruction already pending for {0}", (object) staticEntity));
      }
      else
      {
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.PendingDeconstruction);
        this.m_pendingDeconstructionsToStart.Add(staticEntity);
      }
    }

    private void destroyConstructionState(
      IStaticEntity staticEntity,
      EntityConstructionProgress state,
      DestroyReason? reportRemovedProductsReason,
      bool ignoreNonFullBuffers = false)
    {
      this.m_ongoingConstructions.Remove(staticEntity);
      foreach (ProductBuffer constructionBuffer in state.ConstructionBuffers)
      {
        if (reportRemovedProductsReason.HasValue)
        {
          Assert.That<bool>(ignoreNonFullBuffers | constructionBuffer.IsFull()).IsTrue("Construction did not full-up its buffer.");
          this.m_productsManager.ProductDestroyed(constructionBuffer.Product, constructionBuffer.Quantity, reportRemovedProductsReason.Value);
        }
        constructionBuffer.Clear();
        constructionBuffer.SetCapacity(Quantity.Zero);
      }
      if (state.IsPaused)
      {
        if (!staticEntity.IsDestroyed && state.CurrentSteps > 0)
        {
          state.IsPaused = false;
          this.m_entityPauseStateChanged.Invoke(staticEntity, false);
        }
      }
      else
        state.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
      state.Destroy();
    }

    public void StartDeconstruction(
      IStaticEntity staticEntity,
      bool doNotCreateProducts = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove,
      bool createEmptyBuffersWithCapacity = false)
    {
      if (this.m_ongoingDeconstructions.ContainsKey(staticEntity))
      {
        Log.Error(string.Format("Deconstruction for entity '{0}' already started.", (object) staticEntity));
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.InDeconstruction);
      }
      else
      {
        if (!this.m_pendingDeconstructionsToStart.TryRemove(staticEntity))
          Assert.That<ConstructionState>(staticEntity.ConstructionState).IsNotEqualTo<ConstructionState>(ConstructionState.PendingDeconstruction);
        if (staticEntity is IUpgradableEntity entity)
          this.m_upgradesManager.CancelUpgradeIfNeeded(entity);
        if (staticEntity.IsPaused)
          staticEntity.SetPaused(false);
        bool flag;
        switch (staticEntity.ConstructionState)
        {
          case ConstructionState.NotInitialized:
          case ConstructionState.NotStarted:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        if (flag)
        {
          this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
          this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) staticEntity, entityRemoveReason);
        }
        else
        {
          Percent? currentStepsPerc = new Percent?();
          EntityConstructionProgress constructionProgress1;
          AssetValue totalCost;
          bool isNewConstruction;
          if (this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress1))
          {
            currentStepsPerc = new Percent?(constructionProgress1.Progress);
            totalCost = constructionProgress1.GetAssetValueOfBuffers();
            this.destroyConstructionState(staticEntity, constructionProgress1, doNotCreateProducts ? new DestroyReason?(DestroyReason.Construction) : new DestroyReason?(), true);
            if (constructionProgress1.CurrentSteps == 0 && totalCost.IsEmpty)
            {
              this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
              this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) staticEntity, entityRemoveReason);
              return;
            }
            if (constructionProgress1.AllowFreeRebuild)
            {
              Assert.That<ImmutableArray<IProductBufferReadOnly>>(constructionProgress1.Buffers).IsEmpty<IProductBufferReadOnly>();
              Assert.That<bool>(totalCost.IsEmpty).IsTrue();
              this.SetConstructionStateForEntity(staticEntity, ConstructionState.InDeconstruction);
              this.m_entityStartedDeconstruction.Invoke(staticEntity);
              totalCost = this.GetSellValue(staticEntity.Value.TakeNonVirtualOnly());
              isNewConstruction = true;
            }
            else
              isNewConstruction = false;
          }
          else
          {
            this.SetConstructionStateForEntity(staticEntity, ConstructionState.InDeconstruction);
            this.m_entityStartedDeconstruction.Invoke(staticEntity);
            totalCost = this.GetSellValue(staticEntity.Value.TakeNonVirtualOnly());
            isNewConstruction = true;
          }
          Assert.That<AssetValue>(totalCost).HasAllValuesPositive();
          ImmutableArray<ProductBuffer> buffers = !doNotCreateProducts || createEmptyBuffersWithCapacity ? totalCost.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (pq =>
          {
            ProductBuffer buffer = new ProductBuffer(pq.Quantity, pq.Product);
            if (!doNotCreateProducts)
            {
              buffer.StoreExactly(pq.Quantity);
              if (isNewConstruction)
                this.m_productsManager.ProductCreated(pq, CreateReason.Deconstruction);
            }
            return buffer;
          })).ToImmutableArray() : ImmutableArray<ProductBuffer>.Empty;
          EntityConstructionProgress constructionProgress2 = new EntityConstructionProgress(staticEntity, this.m_notificationsManager, this.m_prioritiesManager, buffers, totalCost, staticEntity.Prototype.ConstructionDurationPerProduct, ConstructionManager.EXTRA_CONSTRUCTION_DURATION, true, currentStepsPerc: currentStepsPerc, allowFreeRebuild: isNewConstruction);
          constructionProgress2.RegisterBuffers(this.m_vehicleBuffRegConstruction);
          this.m_ongoingDeconstructions.Add(staticEntity, constructionProgress2);
          this.SetConstructionStateForEntity(staticEntity, ConstructionState.InDeconstruction);
        }
      }
    }

    private void continueDeconstruction(
      IStaticEntity staticEntity,
      EntityConstructionProgress state)
    {
      Assert.That<bool>(staticEntity.IsDestroyed).IsFalse();
      Assert.That<ConstructionState>(staticEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.InDeconstruction);
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        state.CheatEmpty(this.m_productsManager);
        state.MakeFinished();
        finishDeconstruction();
      }
      else
      {
        bool isNearlyFinished = state.IsNearlyFinished;
        if (!state.TryMakeStep())
          return;
        this.SetTerrainUnder(staticEntity, (ConstructionProgress) state);
        if (state.IsDone)
        {
          Assert.That<int>(state.CurrentSteps).IsZero();
          finishDeconstruction();
        }
        else
        {
          if (isNearlyFinished == state.IsNearlyFinished)
            return;
          this.m_entityConstructionNearlyFinished.Invoke(state.Entity);
        }
      }

      void finishDeconstruction()
      {
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
        this.SetTerrainUnder(staticEntity, (ConstructionProgress) state);
        this.destroyDeconstructionState(staticEntity, state, new DestroyReason?(), new EntityRemoveReason?(EntityRemoveReason.Remove));
      }
    }

    private void destroyDeconstructionState(
      IStaticEntity staticEntity,
      EntityConstructionProgress state,
      DestroyReason? reportRemovedProductsReason,
      EntityRemoveReason? destroyEntity = null)
    {
      this.m_ongoingDeconstructions.RemoveAndAssert(staticEntity);
      foreach (ProductBuffer constructionBuffer in state.ConstructionBuffers)
      {
        if (reportRemovedProductsReason.HasValue)
          this.m_productsManager.ProductDestroyed(constructionBuffer.Product, constructionBuffer.Quantity, reportRemovedProductsReason.Value);
        else
          Assert.That<bool>(constructionBuffer.IsEmpty()).IsTrue("Deconstruction buffer not empty");
        constructionBuffer.Clear();
      }
      if (state.IsPaused)
      {
        state.IsPaused = false;
        this.m_entityPauseStateChanged.Invoke(staticEntity, false);
      }
      else
        state.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
      if (destroyEntity.HasValue)
      {
        Assert.That<ConstructionState>(staticEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.Deconstructed);
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) staticEntity, destroyEntity.Value);
      }
      state.Destroy();
    }

    internal void SetTerrainUnder(
      IStaticEntity staticEntity,
      ConstructionProgress state,
      bool doNotAdjustTerrainHeight = false)
    {
      this.SetTerrainUnderCustom(staticEntity, state.CurrentSteps, state.AlreadyProcessedSteps, state.MaxSteps, state.TerrainDisruptionDisabled, doNotAdjustTerrainHeight | staticEntity.DoNotAdjustTerrainDuringConstruction);
      state.AlreadyProcessedSteps = state.CurrentSteps;
    }

    internal void SetTerrainUnderCustom(
      IStaticEntity staticEntity,
      int currentSteps,
      int alreadyProcessedSteps,
      int maxSteps,
      bool disableTerrainDisruption,
      bool doNotAdjustTerrainHeight = false)
    {
      if (currentSteps > maxSteps)
      {
        Log.Warning(string.Format("Current Steps {0} > max steps {1}", (object) currentSteps, (object) maxSteps));
        currentSteps = maxSteps;
      }
      if (alreadyProcessedSteps > maxSteps)
      {
        Log.Warning(string.Format("Already processed steps {0} > max steps {1}", (object) currentSteps, (object) maxSteps));
        alreadyProcessedSteps = maxSteps;
      }
      Tile3i centerTile = staticEntity.CenterTile;
      TerrainManager terrainManager = this.m_terrainManager.Value;
      ImmutableArray<OccupiedVertexRelative> occupiedVertices = staticEntity.OccupiedVertices;
      Assert.That<ImmutableArray<OccupiedVertexRelative>>(occupiedVertices).IsNotEmpty<OccupiedVertexRelative, IStaticEntity>("Empty occupied vertices for entity '{0}'", staticEntity);
      int num1 = currentSteps * occupiedVertices.Length / maxSteps;
      int num2 = alreadyProcessedSteps * occupiedVertices.Length / maxSteps;
      if (num1 != num2)
      {
        int num3 = (num1 * 2).Min(occupiedVertices.Length);
        int num4 = (num2 * 2).Min(occupiedVertices.Length);
        int num5 = occupiedVertices.Length / 2;
        if (num1 > num2)
        {
          if (!disableTerrainDisruption)
          {
            for (int index = num4; index < num3; ++index)
            {
              if (occupiedVertices[index].FromHeightRel.IsNotPositive)
              {
                Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedVertices[index].RelCoord);
                terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
              }
            }
          }
          if (num5 >= num2 && num5 < num1)
          {
            foreach (OccupiedVertexRelative occupiedVertexRelative in occupiedVertices)
            {
              Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedVertexRelative.RelCoord);
              if (!doNotAdjustTerrainHeight && occupiedVertexRelative.TerrainHeightSlim.HasValue)
              {
                terrainManager.SetHeightPreserveRelativeLayers(tileAndIndex, (centerTile.Height + occupiedVertexRelative.TerrainHeightSlim.Value.AsThicknessTilesI).HeightTilesF);
                terrainManager.StopTerrainPhysicsSimulationAt(tileAndIndex);
              }
              if (occupiedVertexRelative.TerrainMaterial.HasValue)
                terrainManager.DumpMaterial_NoHeightChange(tileAndIndex, new TerrainMaterialThicknessSlim(occupiedVertexRelative.TerrainMaterial.Value, ThicknessTilesF.One));
            }
          }
        }
        else
        {
          if (!disableTerrainDisruption)
          {
            for (int index = num4 - 1; index >= num3; --index)
            {
              if (occupiedVertices[index].FromHeightRel.IsNotPositive)
              {
                Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedVertices[index].RelCoord);
                terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
              }
            }
          }
          if (num5 < num2 && num5 >= num1)
          {
            foreach (OccupiedVertexRelative occupiedVertexRelative in occupiedVertices)
            {
              Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedVertexRelative.RelCoord);
              if (occupiedVertexRelative.TerrainMaterial.HasValue)
                terrainManager.FindAndRemoveFirstLayer(tileAndIndex, occupiedVertexRelative.TerrainMaterial.Value, ThicknessTilesF.One);
              if (!doNotAdjustTerrainHeight && occupiedVertexRelative.TerrainHeightAfterDeconstructionSlim.HasValue)
              {
                terrainManager.SetHeightPreserveRelativeLayers(tileAndIndex, (centerTile.Height + occupiedVertexRelative.TerrainHeightAfterDeconstructionSlim.Value.AsThicknessTilesI).HeightTilesF);
                terrainManager.StopTerrainPhysicsSimulationAt(tileAndIndex);
              }
            }
          }
        }
      }
      ImmutableArray<OccupiedTileRelative> occupiedTiles = staticEntity.OccupiedTiles;
      Assert.That<ImmutableArray<OccupiedTileRelative>>(occupiedTiles).IsNotEmpty<OccupiedTileRelative, IStaticEntity>("Empty occupied tiles for entity '{0}'", staticEntity);
      int num6 = currentSteps * occupiedTiles.Length / maxSteps;
      int num7 = alreadyProcessedSteps * occupiedTiles.Length / maxSteps;
      ITreesManager treesManager = this.m_treeManager.Value;
      TerrainPropsManager terrainPropsManager = this.m_terrainPropsManager.Value;
      if (num6 == num7)
        return;
      if (num6 > num7)
      {
        bool flag = staticEntity is ILayoutEntity layoutEntity && layoutEntity.Prototype.Layout.LayoutParams.EnforceEmptySurface;
        for (int index = num7; index < num6; ++index)
        {
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index];
          Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedTileRelative.RelCoord);
          treesManager.RemoveStumpAtTile(tileAndIndex.TileCoord);
          terrainPropsManager.TryRemovePropAtTile(tileAndIndex.TileCoord, false);
          if (!terrainManager.HasTileSurface(tileAndIndex.Index))
          {
            if (!occupiedTileRelative.TileSurface.IsPhantom)
              terrainManager.SetCustomSurface(tileAndIndex, TileSurfaceData.CreateFlat(centerTile.Height + occupiedTileRelative.TileSurfaceRelHeight, occupiedTileRelative.TileSurface, new Direction90(), true));
          }
          else if (occupiedTileRelative.TileSurface.IsPhantom & flag)
          {
            this.m_assetManager.StoreClearedProduct(terrainManager.GetTileSurface(tileAndIndex.Index).ResolveToProto(this.m_terrainManager.Value).CostPerTile);
            terrainManager.ClearCustomSurface(tileAndIndex);
          }
        }
      }
      else
      {
        for (int index = num7 - 1; index >= num6; --index)
        {
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index];
          if (!occupiedTileRelative.TileSurface.IsPhantom)
          {
            Tile2iAndIndex tileAndIndex = terrainManager.ExtendTileIndex(centerTile.Xy + occupiedTileRelative.RelCoord);
            if (terrainManager.GetTileSurface(tileAndIndex.Index).IsAutoPlaced)
              terrainManager.ClearCustomSurface(tileAndIndex);
          }
        }
      }
    }

    public AssetValue FillConstructionBuffersWith(
      IStaticEntity staticEntity,
      AssetValue value,
      Percent? targetProgress = null)
    {
      EntityConstructionProgress constructionProgress;
      if (!this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress) && !this.m_ongoingDeconstructions.TryGetValue(staticEntity, out constructionProgress))
      {
        Log.Error(string.Format("Failed to fill construction buffers of '{0}'. ", (object) staticEntity) + "Construction was not started or was already finished.");
        return value;
      }
      AssetValue assetValue = constructionProgress.FillBuffersWith(value);
      if (targetProgress.HasValue)
        constructionProgress.SetProgressRawClamped(targetProgress.Value);
      return assetValue;
    }

    public AssetValue EmptyConstructionBuffers(IStaticEntity staticEntity, Percent? targetProgress = null)
    {
      EntityConstructionProgress constructionProgress;
      if (!this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress) && !this.m_ongoingDeconstructions.TryGetValue(staticEntity, out constructionProgress))
      {
        Log.Error(string.Format("Failed to clear construction buffers of  '{0}'. ", (object) staticEntity) + "Construction was not started or was already finished.");
        return AssetValue.Empty;
      }
      AssetValueBuilder pooledInstance = AssetValueBuilder.GetPooledInstance();
      foreach (ProductBuffer constructionBuffer in constructionProgress.ConstructionBuffers)
      {
        if (!constructionBuffer.IsEmpty())
        {
          pooledInstance.Add(constructionBuffer.Product, constructionBuffer.Quantity);
          constructionBuffer.Clear();
        }
      }
      if (targetProgress.HasValue)
        constructionProgress.SetProgressRawClamped(targetProgress.Value);
      return pooledInstance.GetAssetValueAndReturnToPool();
    }

    private Option<EntityConstructionProgress> getConstructionProgressInternal(
      IStaticEntity staticEntity)
    {
      if (staticEntity.ConstructionState == ConstructionState.Constructed)
        return (Option<EntityConstructionProgress>) Option.None;
      bool flag;
      switch (staticEntity.ConstructionState)
      {
        case ConstructionState.PreparingUpgrade:
        case ConstructionState.BeingUpgraded:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        EntityConstructionProgress state;
        if (staticEntity is IUpgradableEntity entity && this.m_upgradesManager.TryGetConstructionState(entity, out state))
          return (Option<EntityConstructionProgress>) state;
        Assert.Fail(string.Format("Non upgradable entity {0} is being upgraded!", (object) staticEntity.Prototype.Id));
        return (Option<EntityConstructionProgress>) Option.None;
      }
      EntityConstructionProgress constructionProgress;
      return this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress) || this.m_ongoingDeconstructions.TryGetValue(staticEntity, out constructionProgress) ? (Option<EntityConstructionProgress>) constructionProgress : (Option<EntityConstructionProgress>) Option.None;
    }

    public Option<IEntityConstructionProgress> GetConstructionProgress(IStaticEntity staticEntity)
    {
      return this.getConstructionProgressInternal(staticEntity).As<IEntityConstructionProgress>();
    }

    public AssetValue CancelConstructionAndReturnBuffers(IStaticEntity staticEntity)
    {
      if (this.m_pendingDeconstructionsToStart.TryRemove(staticEntity))
      {
        Assert.That<ConstructionState>(staticEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.PreparingUpgrade);
        return AssetValue.Empty;
      }
      EntityConstructionProgress state;
      if (this.m_ongoingConstructions.TryGetValue(staticEntity, out state))
      {
        AssetValue assetValue = state.RemoveAssetValueFromBuffers();
        this.destroyConstructionState(staticEntity, state, new DestroyReason?());
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
        return assetValue;
      }
      if (this.m_ongoingDeconstructions.TryGetValue(staticEntity, out state))
      {
        AssetValue assetValue = state.RemoveAssetValueFromBuffers();
        this.destroyDeconstructionState(staticEntity, state, new DestroyReason?());
        this.SetConstructionStateForEntity(staticEntity, ConstructionState.Deconstructed);
        return assetValue;
      }
      bool flag;
      switch (staticEntity.ConstructionState)
      {
        case ConstructionState.PreparingUpgrade:
        case ConstructionState.BeingUpgraded:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag && staticEntity is IUpgradableEntity entity ? this.m_upgradesManager.CancelUpgradeAndReturnBuffers(entity) : AssetValue.Empty;
    }

    public AssetValue GetDeconstructionValueFor(IStaticEntity staticEntity)
    {
      EntityConstructionProgress constructionProgress;
      return this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress) ? this.GetSellValue(constructionProgress.GetAssetValueOfBuffers()) : this.GetSellValue(staticEntity.Value.TakeNonVirtualOnly());
    }

    public ImmutableArray<IProductBufferReadOnly> GetConstructionBuffers(IStaticEntity staticEntity)
    {
      EntityConstructionProgress constructionProgress;
      return this.m_ongoingConstructions.TryGetValue(staticEntity, out constructionProgress) || this.m_ongoingDeconstructions.TryGetValue(staticEntity, out constructionProgress) ? constructionProgress.ConstructionBuffers.As<IProductBufferReadOnly>() : ImmutableArray<IProductBufferReadOnly>.Empty;
    }

    public ImmutableArray<IStaticEntity> GetAllPausedConstructions()
    {
      return this.m_ongoingConstructions.Values.Where<EntityConstructionProgress>((Func<EntityConstructionProgress, bool>) (x => x.IsPaused)).Select<EntityConstructionProgress, IStaticEntity>((Func<EntityConstructionProgress, IStaticEntity>) (x => x.Entity)).Concat<IStaticEntity>(this.m_ongoingDeconstructions.Values.Where<EntityConstructionProgress>((Func<EntityConstructionProgress, bool>) (x => x.IsPaused)).Select<EntityConstructionProgress, IStaticEntity>((Func<EntityConstructionProgress, IStaticEntity>) (x => x.Entity))).ToImmutableArray<IStaticEntity>();
    }

    public bool TryPerformQuickDeliveryOrRemoval(EntityId entityId)
    {
      IStaticEntity entity;
      return this.m_entitiesManager.TryGetEntity<IStaticEntity>(entityId, out entity) && this.TryPerformQuickDeliveryOrRemoval(entity, true);
    }

    public bool TryPerformQuickDeliveryOrRemoval(IStaticEntity entity, bool payWithUnity)
    {
      EntityConstructionProgress constructionProgress;
      bool flag;
      if (this.m_ongoingConstructions.TryGetValue(entity, out constructionProgress))
      {
        if (payWithUnity)
        {
          flag = constructionProgress.TryPerformQuickBuild(this.m_assetManager, (IUpointsManager) this.m_upointsManager, this.m_vehicleBuffRegConstruction);
        }
        else
        {
          constructionProgress.CheatFull(this.m_productsManager);
          constructionProgress.SetAdjustedProgressTo(ConstructionManager.PROGRESS_ON_QUICK_BUILD);
          flag = true;
        }
      }
      else if (this.m_ongoingDeconstructions.TryGetValue(entity, out constructionProgress))
      {
        if (payWithUnity)
        {
          flag = constructionProgress.PerformQuickRemove(this.m_assetManager, (IUpointsManager) this.m_upointsManager, this.m_vehicleBuffRegConstruction, true);
        }
        else
        {
          foreach (ProductBuffer constructionBuffer in constructionProgress.ConstructionBuffers)
          {
            this.m_productsManager.ProductDestroyed(constructionBuffer.Product, constructionBuffer.Quantity, DestroyReason.Cheated);
            constructionBuffer.RemoveAll();
          }
          constructionProgress.SetAdjustedProgressTo(ConstructionManager.PROGRESS_ON_QUICK_BUILD);
          flag = true;
        }
      }
      else
      {
        if (!(entity is IUpgradableEntity entity1))
          return false;
        flag = this.m_upgradesManager.TryFinishUpgradeStateIfExists(entity1, payWithUnity, out string _);
      }
      if (flag)
      {
        this.TrySetConstructionPause(entity, false);
        if (!entity.IsConstructed)
          this.ResetConstructionAnimationState(entity);
      }
      return flag;
    }

    void IAction<FinishBuildOfStaticEntityCmd>.Invoke(FinishBuildOfStaticEntityCmd cmd)
    {
      IStaticEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IStaticEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(EntityId.Invalid, "Failed to perform quick build, entity not found.");
      else if (this.TryPerformQuickDeliveryOrRemoval(entity, cmd.PayWithUnity))
        cmd.SetResultSuccess(entity.Id);
      else
        cmd.SetResultError(EntityId.Invalid, "Failed to perform quick action.");
    }

    void IAction<SetConstructionPriorityCmd>.Invoke(SetConstructionPriorityCmd cmd)
    {
      IStaticEntity entity1;
      if (!this.m_entitiesManager.TryGetEntity<IStaticEntity>(cmd.EntityId, out entity1))
      {
        cmd.SetResultError(EntityId.Invalid, "Failed to perform quick build, entity not found.");
      }
      else
      {
        EntityConstructionProgress constructionProgress;
        if (this.m_ongoingConstructions.TryGetValue(entity1, out constructionProgress))
        {
          constructionProgress.SetStandardPriorityOverride(cmd.Priority);
          cmd.SetResultSuccess(entity1.Id);
        }
        else if (this.m_ongoingDeconstructions.TryGetValue(entity1, out constructionProgress))
        {
          constructionProgress.SetStandardPriorityOverride(cmd.Priority);
          cmd.SetResultSuccess(entity1.Id);
        }
        else
        {
          if (entity1 is IUpgradableEntity entity2)
          {
            Option<EntityConstructionProgress> stateIfExists = this.m_upgradesManager.TryGetStateIfExists(entity2);
            if (stateIfExists.HasValue)
            {
              stateIfExists.Value.SetStandardPriorityOverride(cmd.Priority);
              cmd.SetResultSuccess(entity1.Id);
              return;
            }
          }
          cmd.SetResultError(EntityId.Invalid, "Entity is not in construction.");
        }
      }
    }

    public void TrySetPaused(IStaticEntity entity)
    {
      Option<EntityConstructionProgress> progressInternal = this.getConstructionProgressInternal(entity);
      if (progressInternal.IsNone)
        return;
      EntityConstructionProgress constructionProgress = progressInternal.Value;
      if (constructionProgress.IsPaused)
        return;
      constructionProgress.IsPaused = true;
      constructionProgress.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
      this.m_entityPauseStateChanged.Invoke(entity, constructionProgress.IsPaused);
    }

    public void Invoke(SetConstructionPausedCmd cmd)
    {
      IStaticEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IStaticEntity>(cmd.EntityId, out entity))
      {
        cmd.SetResultError("Failed to perform construction (un)pause.");
      }
      else
      {
        Option<EntityConstructionProgress> progressInternal = this.getConstructionProgressInternal(entity);
        if (progressInternal.IsNone)
        {
          cmd.SetResultError(string.Format("Failed to pause construction for entity '{0}'. ", (object) entity) + "Construction was not started or was already finished.");
        }
        else
        {
          EntityConstructionProgress constructionProgress = progressInternal.Value;
          if (cmd.IsPaused.HasValue)
          {
            int num1 = constructionProgress.IsPaused ? 1 : 0;
            bool? isPaused = cmd.IsPaused;
            int num2 = isPaused.GetValueOrDefault() ? 1 : 0;
            if (num1 == num2 & isPaused.HasValue)
            {
              cmd.SetResultSuccess();
              return;
            }
            constructionProgress.IsPaused = cmd.IsPaused.Value;
          }
          else
            constructionProgress.IsPaused = !constructionProgress.IsPaused;
          if (constructionProgress.IsPaused)
            constructionProgress.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
          else
            constructionProgress.RegisterBuffers(this.m_vehicleBuffRegConstruction);
          this.m_entityPauseStateChanged.Invoke(entity, constructionProgress.IsPaused);
          cmd.SetResultSuccess();
        }
      }
    }

    public bool TrySetConstructionPause(IStaticEntity entity, bool pause)
    {
      Option<EntityConstructionProgress> progressInternal = this.getConstructionProgressInternal(entity);
      if (progressInternal.IsNone)
        return false;
      EntityConstructionProgress constructionProgress = progressInternal.Value;
      if (constructionProgress.IsPaused == pause)
        return false;
      constructionProgress.IsPaused = pause;
      if (constructionProgress.IsPaused)
        constructionProgress.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
      else
        constructionProgress.RegisterBuffers(this.m_vehicleBuffRegConstruction);
      this.m_entityPauseStateChanged.Invoke(entity, constructionProgress.IsPaused);
      return true;
    }

    public void SetConstructionStateForEntity(IStaticEntity entity, ConstructionState state)
    {
      if (entity.ConstructionState == state || entity.IsDestroyed)
        return;
      entity.SetConstructionState(state);
      this.m_entityConstructionStateChanged.Invoke(entity, state);
    }

    public static void Serialize(ConstructionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ConstructionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ConstructionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_deconstructionRefundMult);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Event<IStaticEntity>.Serialize(this.m_entityConstructed, writer);
      Event<IStaticEntity>.Serialize(this.m_entityConstructionNearlyFinished, writer);
      Event<IStaticEntity, ConstructionState>.Serialize(this.m_entityConstructionStateChanged, writer);
      Event<IStaticEntity, bool>.Serialize(this.m_entityPauseStateChanged, writer);
      Event<IStaticEntity>.Serialize(this.m_entityStartedDeconstruction, writer);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      Dict<IStaticEntity, EntityConstructionProgress>.Serialize(this.m_ongoingConstructions, writer);
      Dict<IStaticEntity, EntityConstructionProgress>.Serialize(this.m_ongoingDeconstructions, writer);
      Event<IStaticEntity>.Serialize(this.m_onResetConstructionAnimationState, writer);
      Lyst<IStaticEntity>.Serialize(this.m_pendingDeconstructionsToStart, writer);
      PlanningModeManager.Serialize(this.m_planningModeManager, writer);
      GlobalPrioritiesManager.Serialize(this.m_prioritiesManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      LazyResolve<TerrainManager>.Serialize(this.m_terrainManager, writer);
      LazyResolve<TerrainPropsManager>.Serialize(this.m_terrainPropsManager, writer);
      LazyResolve<ITreesManager>.Serialize(this.m_treeManager, writer);
      UpgradesManager.Serialize(this.m_upgradesManager, writer);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffRegConstruction);
    }

    public static ConstructionManager Deserialize(BlobReader reader)
    {
      ConstructionManager constructionManager;
      if (reader.TryStartClassDeserialization<ConstructionManager>(out constructionManager))
        reader.EnqueueDataDeserialization((object) constructionManager, ConstructionManager.s_deserializeDataDelayedAction);
      return constructionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      if (reader.LoadedSaveVersion < 140)
        Percent.Deserialize(reader);
      reader.SetField<ConstructionManager>(this, "m_assetManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<ConstructionManager>(this, "m_deconstructionRefundMult", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<ConstructionManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_entityConstructed", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_entityConstructionNearlyFinished", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_entityConstructionStateChanged", (object) Event<IStaticEntity, ConstructionState>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_entityPauseStateChanged", (object) Event<IStaticEntity, bool>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_entityStartedDeconstruction", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<ConstructionManager>(this, "m_iterationCache", (object) new Lyst<IStaticEntity>());
      reader.SetField<ConstructionManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<ConstructionManager>(this, "m_ongoingConstructions", (object) Dict<IStaticEntity, EntityConstructionProgress>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_ongoingDeconstructions", (object) Dict<IStaticEntity, EntityConstructionProgress>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_onResetConstructionAnimationState", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_pendingDeconstructionsToStart", (object) Lyst<IStaticEntity>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_planningModeManager", (object) PlanningModeManager.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_prioritiesManager", (object) GlobalPrioritiesManager.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<ConstructionManager>(this, "m_terrainManager", (object) LazyResolve<TerrainManager>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_terrainPropsManager", (object) LazyResolve<TerrainPropsManager>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_treeManager", (object) LazyResolve<ITreesManager>.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_upgradesManager", (object) UpgradesManager.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<ConstructionManager>(this, "m_vehicleBuffRegConstruction", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.RegisterInitAfterLoad<ConstructionManager>(this, "initSelf", InitPriority.Normal);
    }

    static ConstructionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ConstructionManager.EXTRA_CONSTRUCTION_DURATION = 5.Seconds();
      ConstructionManager.PROGRESS_ON_QUICK_BUILD = 95.Percent();
      ConstructionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ConstructionManager) obj).SerializeData(writer));
      ConstructionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ConstructionManager) obj).DeserializeData(reader));
    }
  }
}
