// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.UpgradesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class UpgradesManager : 
    IUpgradesManager,
    ICommandProcessor<UpgradeEntityCmd>,
    IAction<UpgradeEntityCmd>
  {
    private static readonly Percent PROGRESS_ON_QUICK_BUILD_UPGRADE;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly GlobalPrioritiesManager m_prioritiesManager;
    private readonly IProductsManager m_productsManager;
    private readonly IAssetTransactionManager m_assetManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffRegConstruction;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly UpointsManager m_upointsManager;
    private readonly LazyResolve<ConstructionManager> m_constructionManager;
    private readonly Dict<IUpgradableEntity, EntityConstructionProgress> m_ongoingUpgrades;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<IUpgradableEntity> m_pendingUpgradesToRemove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public UpgradesManager(
      IEntitiesManager entitiesManager,
      ISimLoopEvents simLoopEvents,
      INotificationsManager notificationsManager,
      GlobalPrioritiesManager prioritiesManager,
      IProductsManager productsManager,
      IAssetTransactionManager assetManager,
      IVehicleBuffersRegistry vehicleBuffRegConstruction,
      IInstaBuildManager instaBuildManager,
      UpointsManager upointsManager,
      LazyResolve<ConstructionManager> constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_ongoingUpgrades = new Dict<IUpgradableEntity, EntityConstructionProgress>();
      this.m_pendingUpgradesToRemove = new Lyst<IUpgradableEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_notificationsManager = notificationsManager;
      this.m_prioritiesManager = prioritiesManager;
      this.m_productsManager = productsManager;
      this.m_assetManager = assetManager;
      this.m_vehicleBuffRegConstruction = vehicleBuffRegConstruction;
      this.m_instaBuildManager = instaBuildManager;
      this.m_upointsManager = upointsManager;
      this.m_constructionManager = constructionManager;
      simLoopEvents.Update.Add<UpgradesManager>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      foreach (KeyValuePair<IUpgradableEntity, EntityConstructionProgress> ongoingUpgrade in this.m_ongoingUpgrades)
      {
        if (this.continueUpgrade(ongoingUpgrade.Key, ongoingUpgrade.Value))
          this.m_pendingUpgradesToRemove.Add(ongoingUpgrade.Key);
      }
      this.m_pendingUpgradesToRemove.ForEachAndClear((Action<IUpgradableEntity>) (e => this.m_ongoingUpgrades.RemoveAndAssert(e)));
    }

    /// <summary>Returns true if upgrade should be removed.</summary>
    private bool continueUpgrade(IUpgradableEntity entity, EntityConstructionProgress state)
    {
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.PreparingUpgrade:
        case ConstructionState.BeingUpgraded:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
      {
        this.cancelUpgrade(entity, state);
        return true;
      }
      if (entity.IsDestroyed)
      {
        Log.Error("Continuing upgrade of destructed entity.");
        return true;
      }
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        state.CheatFull(this.m_productsManager);
        state.MakeFinished();
        finishUpgrade();
        return true;
      }
      if (!state.TryMakeStep())
        return false;
      if (entity.ConstructionState != ConstructionState.BeingUpgraded && state.IsInExtraStepPhase)
        this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.BeingUpgraded);
      if (!state.IsDone)
        return false;
      Assert.That<int>(state.CurrentSteps).IsEqualTo(state.MaxSteps);
      finishUpgrade();
      return true;

      void finishUpgrade()
      {
        Assert.That<Percent>(state.Progress).IsEqualTo(Percent.Hundred);
        foreach (ProductBuffer constructionBuffer in state.ConstructionBuffers)
        {
          Assert.That<bool>(this.m_instaBuildManager.IsInstaBuildEnabled || constructionBuffer.IsFull()).IsTrue("Construction did not full-up its buffer.");
          this.m_productsManager.ProductDestroyed(constructionBuffer.Product, constructionBuffer.Quantity, DestroyReason.Construction);
          constructionBuffer.Clear();
          constructionBuffer.SetCapacity(Quantity.Zero);
        }
        state.UnregisterBuffers(this.m_vehicleBuffRegConstruction);
        Assert.That<int>(state.AlreadyProcessedSteps).IsEqualTo(0, "Terran should not be processed during upgrade.");
        this.m_constructionManager.Value.SetTerrainUnderCustom((IStaticEntity) entity, 0, 1, 1, true, entity.DoNotAdjustTerrainDuringConstruction);
        this.m_entitiesManager.TryUpgradeEntity(entity);
        this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.Constructed);
        this.m_constructionManager.Value.SetTerrainUnderCustom((IStaticEntity) entity, 1, 0, 1, true, entity.DoNotAdjustTerrainDuringConstruction);
        state.Destroy();
      }
    }

    public bool TryGetConstructionState(
      IUpgradableEntity entity,
      out EntityConstructionProgress state)
    {
      return this.m_ongoingUpgrades.TryGetValue(entity, out state);
    }

    public bool CancelUpgradeIfNeeded(IUpgradableEntity entity)
    {
      EntityConstructionProgress state;
      if (!this.m_ongoingUpgrades.TryGetValue(entity, out state))
        return false;
      this.cancelUpgrade(entity, state);
      this.m_ongoingUpgrades.Remove(entity);
      return true;
    }

    private void cancelUpgrade(IUpgradableEntity entity, EntityConstructionProgress state)
    {
      state.ClearAndDestroyBuffers(this.m_assetManager, this.m_vehicleBuffRegConstruction);
      this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.Constructed);
      entity.Upgrader.UpgradeCanceled();
    }

    /// <summary>
    /// This just moves progress, a sim step still needs to be performed to finish the upgrade.
    /// </summary>
    internal bool TryFinishUpgradeStateIfExists(
      IUpgradableEntity entity,
      bool payWithUnity,
      out string error)
    {
      EntityConstructionProgress constructionProgress;
      if (!this.m_ongoingUpgrades.TryGetValue(entity, out constructionProgress))
      {
        error = "Failed to perform quick build, entity is not being upgraded.";
        return false;
      }
      if (payWithUnity)
      {
        if (!constructionProgress.TryPerformQuickBuild(this.m_assetManager, (IUpointsManager) this.m_upointsManager, this.m_vehicleBuffRegConstruction))
        {
          error = "Failed to perform quick build, not enough Unity.";
          return false;
        }
        this.m_constructionManager.Value.TrySetConstructionPause((IStaticEntity) entity, false);
      }
      else
      {
        constructionProgress.CheatFull(this.m_productsManager);
        constructionProgress.SetAdjustedProgressTo(UpgradesManager.PROGRESS_ON_QUICK_BUILD_UPGRADE);
      }
      error = "";
      return true;
    }

    internal Option<EntityConstructionProgress> TryGetStateIfExists(IUpgradableEntity entity)
    {
      EntityConstructionProgress constructionProgress;
      return !this.m_ongoingUpgrades.TryGetValue(entity, out constructionProgress) ? Option<EntityConstructionProgress>.None : (Option<EntityConstructionProgress>) constructionProgress;
    }

    public bool TryFinishUpgradeImmediately(
      IUpgradableEntity entity,
      bool payWithUnity,
      out string error)
    {
      if (!this.TryFinishUpgradeStateIfExists(entity, payWithUnity, out error))
        return false;
      this.m_ongoingUpgrades[entity].MakeFinished();
      if (this.continueUpgrade(entity, this.m_ongoingUpgrades[entity]))
      {
        this.m_ongoingUpgrades.Remove(entity);
        return true;
      }
      error = "Fail to finish upgrade.";
      Log.Error(error);
      return false;
    }

    public void Invoke(UpgradeEntityCmd cmd)
    {
      IUpgradableEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IUpgradableEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("Failed to find upgradable entity with id '{0}'.", (object) cmd.EntityId));
      else if (!this.TryStartUpgrade(entity))
        cmd.SetResultError(string.Format("Upgrade for '{0}' is not available or not possible.", (object) entity));
      else
        cmd.SetResultSuccess();
    }

    public bool TryStartUpgrade(IUpgradableEntity entity)
    {
      if (entity.IsDestroyed || !entity.IsUpgradeAvailable(out LocStrFormatted _) || entity.ConstructionState != ConstructionState.Constructed)
        return false;
      this.startUpgrade(entity);
      return true;
    }

    public AssetValue CancelUpgradeAndReturnBuffers(IUpgradableEntity entity)
    {
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.PreparingUpgrade:
        case ConstructionState.BeingUpgraded:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
      {
        Assert.Fail(string.Format("No upgrade in progress, state was {0}", (object) entity.ConstructionState));
        return AssetValue.Empty;
      }
      EntityConstructionProgress constructionProgress;
      if (!this.m_ongoingUpgrades.TryRemove(entity, out constructionProgress))
      {
        Assert.Fail("Failed to remove construction state while upgrading an entity.");
        return AssetValue.Empty;
      }
      AssetValueBuilder pooledInstance = AssetValueBuilder.GetPooledInstance();
      foreach (ProductBuffer constructionBuffer in constructionProgress.ConstructionBuffers)
      {
        this.m_vehicleBuffRegConstruction.UnregisterInputBufferAndAssert((IProductBuffer) constructionBuffer);
        pooledInstance.Add(constructionBuffer.Product, constructionBuffer.Quantity);
        constructionBuffer.Clear();
        constructionBuffer.SetCapacity(Quantity.Zero);
      }
      this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.Constructed);
      return pooledInstance.GetAssetValueAndReturnToPool();
    }

    public AssetValue FillUpgradeBuffersWith(
      IUpgradableEntity entity,
      AssetValue value,
      Percent? targetProgress)
    {
      EntityConstructionProgress constructionProgress;
      if (!this.m_ongoingUpgrades.TryGetValue(entity, out constructionProgress))
      {
        Log.Error(string.Format("Failed to fill upgrade buffers of '{0}'.", (object) entity));
        return value;
      }
      AssetValue assetValue = constructionProgress.FillBuffersWith(value);
      if (targetProgress.HasValue)
        constructionProgress.SetAdjustedProgressTo(targetProgress.Value);
      if (constructionProgress.AreBuffersFull() && entity.ConstructionState != ConstructionState.BeingUpgraded && constructionProgress.IsInExtraStepPhase)
        this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.BeingUpgraded);
      return assetValue;
    }

    private void startUpgrade(IUpgradableEntity entity)
    {
      AssetValue constructionCostToUpgrade = entity.Upgrader.ConstructionCostToUpgrade;
      ImmutableArray<ProductBuffer> immutableArray = constructionCostToUpgrade.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (pq =>
      {
        Assert.That<ProductQuantity>(pq).IsNotEmpty("Empty static entity cost.");
        return new ProductBuffer(pq.Quantity, pq.Product);
      })).ToImmutableArray();
      EntityConstructionProgress constructionProgress = new EntityConstructionProgress((IStaticEntity) entity, this.m_notificationsManager, this.m_prioritiesManager, immutableArray, constructionCostToUpgrade, Duration.OneTick, ConstructionManager.EXTRA_CONSTRUCTION_DURATION + constructionCostToUpgrade.GetQuantitySum().Value * entity.Prototype.ConstructionDurationPerProduct, isUpgrade: true);
      constructionProgress.RegisterBuffers(this.m_vehicleBuffRegConstruction);
      this.m_ongoingUpgrades.Add(entity, constructionProgress);
      this.m_constructionManager.Value.SetConstructionStateForEntity((IStaticEntity) entity, ConstructionState.PreparingUpgrade);
      entity.Upgrader.UpgradeStarted();
    }

    public static void Serialize(UpgradesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpgradesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpgradesManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetManager);
      LazyResolve<ConstructionManager>.Serialize(this.m_constructionManager, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      Dict<IUpgradableEntity, EntityConstructionProgress>.Serialize(this.m_ongoingUpgrades, writer);
      GlobalPrioritiesManager.Serialize(this.m_prioritiesManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffRegConstruction);
    }

    public static UpgradesManager Deserialize(BlobReader reader)
    {
      UpgradesManager upgradesManager;
      if (reader.TryStartClassDeserialization<UpgradesManager>(out upgradesManager))
        reader.EnqueueDataDeserialization((object) upgradesManager, UpgradesManager.s_deserializeDataDelayedAction);
      return upgradesManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UpgradesManager>(this, "m_assetManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<UpgradesManager>(this, "m_constructionManager", (object) LazyResolve<ConstructionManager>.Deserialize(reader));
      reader.SetField<UpgradesManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<UpgradesManager>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<UpgradesManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<UpgradesManager>(this, "m_ongoingUpgrades", (object) Dict<IUpgradableEntity, EntityConstructionProgress>.Deserialize(reader));
      reader.SetField<UpgradesManager>(this, "m_pendingUpgradesToRemove", (object) new Lyst<IUpgradableEntity>());
      reader.SetField<UpgradesManager>(this, "m_prioritiesManager", (object) GlobalPrioritiesManager.Deserialize(reader));
      reader.SetField<UpgradesManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<UpgradesManager>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<UpgradesManager>(this, "m_vehicleBuffRegConstruction", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
    }

    static UpgradesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpgradesManager.PROGRESS_ON_QUICK_BUILD_UPGRADE = 80.Percent();
      UpgradesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpgradesManager) obj).SerializeData(writer));
      UpgradesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpgradesManager) obj).DeserializeData(reader));
    }
  }
}
