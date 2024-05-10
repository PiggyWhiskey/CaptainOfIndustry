// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapMine
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapMine : 
    WorldMapRepairableEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IUnityConsumingEntity,
    IUpgradableWorldEntity,
    IWorldMapEntity,
    IEntityWithCustomTitle,
    IEntityWithGeneralPriorityFriend
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration DURATION_TO_UPGRADE;
    private static readonly Percent NO_UNITY_PROD_PENALTY1;
    private static readonly Percent NO_UNITY_PROD_PENALTY2;
    private static readonly Duration NO_UNITY_PROD_PENALTY2_MONTHS;
    private static readonly Percent NO_UNITY_PROD_PENALTY3;
    private static readonly Duration NO_UNITY_PROD_PENALTY3_MONTHS;
    private static readonly Duration DURATION_CAPACITY;
    private WorldMapMineProto m_proto;
    private readonly IUpointsManager m_upointsManager;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly TickTimer m_timer;
    private readonly ProductBuffer m_buffer;
    [RenamedInVersion(140, "UnityConsumer")]
    private readonly Mafi.Core.Population.UnityConsumer m_unityConsumer;
    [DoNotSave(0, null)]
    private Quantity m_depositQuantity;
    [DoNotSave(0, null)]
    private bool m_isReserveUnlimitedGlobally;
    [OnlyForSaveCompatibility(null)]
    [DoNotSave(140, null)]
    [RenamedInVersion(140, "QuantityAvailable")]
    [Obsolete]
    private Quantity? m_quantityAvailableObsolete;
    /// <summary>
    /// Does not include consumption when resource was unlimited.
    /// </summary>
    [NewInSaveVersion(140, null, null, null, null)]
    private Quantity m_consumedFromLimited;
    [DoNotSave(0, null)]
    private bool m_canRunWithoutUnity;
    [NewInSaveVersion(140, null, null, null, null)]
    private Duration m_durationWorkedOnLowUnity;

    public static void Serialize(WorldMapMine value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapMine>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapMine.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      Option<string>.Serialize(this.CustomTitle, writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteInt(this.Level);
      ProductBuffer.Serialize(this.m_buffer, writer);
      Quantity.Serialize(this.m_consumedFromLimited, writer);
      Duration.Serialize(this.m_durationWorkedOnLowUnity, writer);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      writer.WriteGeneric<WorldMapMineProto>(this.m_proto);
      TickTimer.Serialize(this.m_timer, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteInt(this.ProductionStep);
      Mafi.Core.Population.UnityConsumer.Serialize(this.m_unityConsumer, writer);
      writer.WriteBool(this.WorkedLastTick);
      writer.WriteInt(this.WorkersNeeded);
    }

    public static WorldMapMine Deserialize(BlobReader reader)
    {
      WorldMapMine worldMapMine;
      if (reader.TryStartClassDeserialization<WorldMapMine>(out worldMapMine))
        reader.EnqueueDataDeserialization((object) worldMapMine, WorldMapMine.s_deserializeDataDelayedAction);
      return worldMapMine;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (WorldMapMine.State) reader.ReadInt();
      this.CustomTitle = Option<string>.Deserialize(reader);
      this.GeneralPriority = reader.ReadInt();
      this.Level = reader.ReadInt();
      reader.SetField<WorldMapMine>(this, "m_buffer", (object) ProductBuffer.Deserialize(reader));
      this.m_consumedFromLimited = reader.LoadedSaveVersion >= 140 ? Quantity.Deserialize(reader) : new Quantity();
      this.m_durationWorkedOnLowUnity = reader.LoadedSaveVersion >= 140 ? Duration.Deserialize(reader) : new Duration();
      reader.SetField<WorldMapMine>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<WorldMapMine>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      this.m_proto = reader.ReadGenericAs<WorldMapMineProto>();
      reader.SetField<WorldMapMine>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<WorldMapMine>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.ProductionStep = reader.ReadInt();
      if (reader.LoadedSaveVersion < 140)
        this.m_quantityAvailableObsolete = reader.ReadNullableStruct<Quantity>();
      reader.SetField<WorldMapMine>(this, "m_unityConsumer", (object) Mafi.Core.Population.UnityConsumer.Deserialize(reader));
      this.WorkedLastTick = reader.ReadBool();
      this.WorkersNeeded = reader.ReadInt();
      reader.RegisterInitAfterLoad<WorldMapMine>(this, "initSelf", InitPriority.Low);
    }

    public Option<string> CustomTitle { get; set; }

    public override LocStrFormatted DefaultTitle => this.Prototype.GetTitleFor(this.Level);

    public static Quantity GetMinCapacityForLevel(int level)
    {
      Quantity quantity1 = 2 * 440.Quantity();
      Quantity rhs = 8 * 440.Quantity();
      Quantity quantity2 = level * quantity1;
      quantity2 = quantity2.Min(rhs);
      return quantity2.ScaledBy((100 + level * 10).Percent());
    }

    public override bool CanBePaused => this.IsRepaired;

    /// <summary>Current level of this mine.</summary>
    public int Level { get; private set; }

    /// <summary>Max production steps allowed for mine at this level.</summary>
    public int MaxProductionSteps => this.Level;

    /// <summary>
    /// Current production step that determines output of this mine.
    /// </summary>
    public int ProductionStep { get; private set; }

    [DoNotSave(0, null)]
    public WorldMapMineProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (EntityProto) value;
      }
    }

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public int WorkersNeeded { get; private set; }

    protected override bool IsEnabledNow
    {
      get => !this.IsPaused && this.IsRepaired && this.Maintenance.CanWork();
    }

    public bool WorkedLastTick { get; private set; }

    public ProductProto Product => this.Prototype.ProducedProductPerStep.Product;

    public Percent ProgressDone
    {
      get
      {
        return Percent.FromRatio(this.Prototype.ProductionDuration.Ticks - this.m_timer.Ticks.Ticks, this.Prototype.ProductionDuration.Ticks);
      }
    }

    public IProductBufferReadOnly Buffer => (IProductBufferReadOnly) this.m_buffer;

    public Option<Mafi.Core.Population.UnityConsumer> UnityConsumer
    {
      get => (Option<Mafi.Core.Population.UnityConsumer>) this.m_unityConsumer;
    }

    public WorldMapMine.State CurrentState { get; private set; }

    public Upoints MaxMonthlyUnityConsumed => this.MonthlyUnityConsumed;

    public Upoints MonthlyUnityConsumed
    {
      get => this.Prototype.MonthlyUpointsPerLevel * this.ProductionStep;
    }

    public Proto.ID UpointsCategoryId => this.Prototype.UpointsCategory.Id;

    bool IMaintainedEntity.IsIdleForMaintenance => this.CurrentState != WorldMapMine.State.Working;

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts
    {
      get => this.Prototype.CostPerLevel(this.ProductionStep).Maintenance;
    }

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public override AssetValue CostToRepair => this.Prototype.Costs.Price;

    public Quantity? QuantityAvailable
    {
      get
      {
        return !this.IsReserveUnlimited ? new Quantity?((this.m_depositQuantity - this.m_consumedFromLimited).Max(Quantity.Zero)) : new Quantity?();
      }
    }

    private bool IsReserveUnlimited
    {
      get => this.m_isReserveUnlimitedGlobally || !this.Prototype.QuantityAvailable.HasValue;
    }

    public AssetValue PriceToUpgrade
    {
      get => this.Prototype.CostPerLevel(this.Level + this.Prototype.LevelsPerUpgrade).Price;
    }

    public LocStrFormatted UpgradeTitle
    {
      get => this.Prototype.GetTitleFor(this.Level + this.Prototype.LevelsPerUpgrade);
    }

    public bool UpgradeExists => this.Level < this.Prototype.MaxLevel;

    public string UpgradeIcon => this.Prototype.Graphics.IconPath;

    public int GeneralPriority { get; private set; }

    public virtual bool IsCargoAffectedByGeneralPriority => false;

    public virtual bool IsGeneralPriorityVisible => this.IsRepaired;

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    public WorldMapMine(
      EntityId entityId,
      WorldMapMineProto proto,
      WorldMapLocation location,
      EntityContext context,
      WorldMapManager worldMapManager,
      IUpointsManager upointsManager,
      IInstaBuildManager instaBuildManager,
      IProductsManager productsManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(entityId, (WorldMapEntityProto) proto, location, context, worldMapManager, instaBuildManager, productsManager);
      this.Prototype = proto;
      this.m_upointsManager = upointsManager;
      this.m_instaBuildManager = instaBuildManager;
      this.m_notificationsManager = notificationsManager;
      this.updateProperties();
      this.Level = proto.Level;
      this.m_buffer = new ProductBuffer(this.getCapacityForCurrentLevel(), proto.ProducedProductPerStep.Product);
      this.m_timer.Start(this.Prototype.ProductionDuration);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_unityConsumer = this.Context.UnityConsumerFactory.CreateConsumer((IUnityConsumingEntity) this);
      this.OnConstructionDone.Add<WorldMapMine>(this, new Action<IWorldMapRepairableEntity>(this.onRepaired));
      this.SetProductionStep(this.MaxProductionSteps);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(int saveVersion)
    {
      this.m_buffer.IncreaseCapacityTo(this.getCapacityForCurrentLevel());
      this.updateProperties();
      if (saveVersion >= 140 || !this.m_quantityAvailableObsolete.HasValue || !this.Prototype.QuantityAvailable.HasValue)
        return;
      Percent scale = this.Context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.WorldMinesReserveMultiplier).Value;
      Quantity quantity = this.Prototype.QuantityAvailable.Value;
      quantity = quantity.ScaledBy(scale) - this.m_quantityAvailableObsolete.Value;
      this.m_consumedFromLimited = quantity.Max(Quantity.Zero);
    }

    private void updateProperties()
    {
      this.m_canRunWithoutUnity = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.WorldMinesCanRunWithoutUnity);
      this.m_isReserveUnlimitedGlobally = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.UnlimitedWorldMines);
      if (!this.Prototype.QuantityAvailable.HasValue)
        return;
      this.m_depositQuantity = this.Prototype.QuantityAvailable.Value.ScaledBy(this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.WorldMinesReserveMultiplier));
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    private Quantity getCapacityForCurrentLevel()
    {
      Quantity rhs = WorldMapMine.DURATION_CAPACITY.Ticks / this.Prototype.ProductionDuration.Ticks * this.Prototype.ProducedProductPerStep.Quantity * this.MaxProductionSteps;
      return WorldMapMine.GetMinCapacityForLevel(this.Level).Max(rhs).Value.CeilToMultipleOf(10).Quantity();
    }

    private void onRepaired(IWorldMapRepairableEntity obj)
    {
      this.OnConstructionDone.Remove<WorldMapMine>(this, new Action<IWorldMapRepairableEntity>(this.onRepaired));
      this.m_notificationsManager.NotifyOnce<WorldMapMineProto>(IdsCore.Notifications.WorldEntityRepaired, this.Prototype);
    }

    public void StartUpgrade()
    {
      if (!this.IsUpgradeAvailable(out LocStrFormatted _))
        return;
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.UpgradeSelf();
        this.m_onConstructed.Invoke((IWorldMapRepairableEntity) this);
      }
      else
      {
        AssetValue priceToUpgrade = this.PriceToUpgrade;
        ImmutableArray<ProductBuffer> immutableArray = priceToUpgrade.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (x => new ProductBuffer(x.Quantity, x.Product))).ToImmutableArray();
        int num = priceToUpgrade.GetQuantitySum().Value;
        Duration durationPerProduct = num <= 0 ? Duration.OneTick : Duration.OneTick.Max(WorldMapMine.DURATION_TO_UPGRADE / num);
        this.SetUpgradeProgress(new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, immutableArray, priceToUpgrade, durationPerProduct, Duration.Zero));
      }
    }

    protected override void SimUpdateInternal()
    {
      base.SimUpdateInternal();
      this.WorkedLastTick = false;
      if (!this.IsEnabled)
      {
        if (this.Maintenance.Status.IsBroken)
          this.CurrentState = WorldMapMine.State.Broken;
        this.CurrentState = WorldMapMine.State.Paused;
      }
      else
      {
        Quantity rhs = this.QuantityAvailable ?? Quantity.MaxValue;
        if (rhs.IsNotPositive)
        {
          this.CurrentState = WorldMapMine.State.ResourceDepleted;
          this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
        }
        else
        {
          if (!this.m_unityConsumer.CanWork())
          {
            if (this.m_canRunWithoutUnity)
            {
              this.m_durationWorkedOnLowUnity += Duration.OneTick;
            }
            else
            {
              this.CurrentState = WorldMapMine.State.NotEnoughUnity;
              this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
              return;
            }
          }
          else if (this.m_durationWorkedOnLowUnity.IsPositive)
            this.m_durationWorkedOnLowUnity -= Duration.OneTick;
          if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
            this.CurrentState = WorldMapMine.State.NotEnoughWorkers;
          else if (this.m_buffer.IsFull())
          {
            this.CurrentState = WorldMapMine.State.FullStorage;
          }
          else
          {
            this.CurrentState = WorldMapMine.State.Working;
            this.WorkedLastTick = true;
            if (this.m_timer.Decrement())
              return;
            Quantity quantity1 = this.Prototype.ProducedProductPerStep.Quantity * this.ProductionStep;
            Quantity quantity2 = this.IsReserveUnlimited ? quantity1 : quantity1.Min(rhs);
            Quantity quantity3 = quantity2;
            Percent productionPenalty = this.GetProductionPenalty();
            if (productionPenalty.IsPositive)
              quantity2 = quantity2.ScaledBy(productionPenalty.InverseTo100());
            if ((quantity2 - this.m_buffer.StoreAsMuchAs(quantity2)).IsPositive && !this.IsReserveUnlimited)
              this.m_consumedFromLimited += quantity3;
            this.m_timer.Start(this.Prototype.ProductionDuration);
          }
        }
      }
    }

    public Percent GetProductionPenalty()
    {
      if (!this.m_unityConsumer.NotEnoughUnity || !this.m_durationWorkedOnLowUnity.IsPositive)
        return Percent.Zero;
      if (this.m_durationWorkedOnLowUnity > WorldMapMine.NO_UNITY_PROD_PENALTY3_MONTHS)
        return WorldMapMine.NO_UNITY_PROD_PENALTY3;
      return this.m_durationWorkedOnLowUnity > WorldMapMine.NO_UNITY_PROD_PENALTY2_MONTHS ? WorldMapMine.NO_UNITY_PROD_PENALTY2 : WorldMapMine.NO_UNITY_PROD_PENALTY1;
    }

    protected override void OnUpgradeProgressDone() => this.UpgradeSelf();

    /// <summary>Returns how much was removed.</summary>
    public Quantity RemoveAsMuchAs(Quantity quantity) => this.m_buffer.RemoveAsMuchAs(quantity);

    public bool IsUpgradeAvailable(out LocStrFormatted error)
    {
      error = LocStrFormatted.Empty;
      return this.IsRepaired && this.UpgradeExists;
    }

    public bool IsUpgradeVisible()
    {
      return this.IsRepaired && this.UpgradeExists && !this.IsBeingUpgraded;
    }

    public bool UpgradeSelf()
    {
      if (!this.IsUpgradeAvailable(out LocStrFormatted _))
        return false;
      this.Level += this.Prototype.LevelsPerUpgrade;
      this.m_buffer.IncreaseCapacityTo(this.getCapacityForCurrentLevel());
      return true;
    }

    public void SetProductionStep(int step)
    {
      if (step < 0 || step > this.MaxProductionSteps)
      {
        Log.Error(string.Format("Production step '{0}' out of range (0, {1})", (object) step, (object) this.MaxProductionSteps));
      }
      else
      {
        this.ProductionStep = step;
        this.SetPaused(this.ProductionStep == 0);
        EntityCosts entityCosts = this.Prototype.CostPerLevel(this.ProductionStep);
        this.m_unityConsumer.RefreshUnityConsumed();
        this.Maintenance.RefreshMaintenanceCost();
        this.WorkersNeeded = entityCosts.Workers;
        if (!((IEntityWithWorkers) this).HasWorkersCached)
          return;
        this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
      }
    }

    public override void SetPaused(bool isPaused)
    {
      if (!isPaused && this.ProductionStep == 0)
        this.SetProductionStep(1);
      else
        base.SetPaused(isPaused);
    }

    static WorldMapMine()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapMine.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WorldMapMine.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      WorldMapMine.DURATION_TO_UPGRADE = 60.Seconds();
      WorldMapMine.NO_UNITY_PROD_PENALTY1 = 20.Percent();
      WorldMapMine.NO_UNITY_PROD_PENALTY2 = 40.Percent();
      WorldMapMine.NO_UNITY_PROD_PENALTY2_MONTHS = 4.Months();
      WorldMapMine.NO_UNITY_PROD_PENALTY3 = 60.Percent();
      WorldMapMine.NO_UNITY_PROD_PENALTY3_MONTHS = 6.Months();
      WorldMapMine.DURATION_CAPACITY = 12.Minutes();
    }

    public enum State
    {
      None,
      Broken,
      Paused,
      NotEnoughWorkers,
      NotEnoughUnity,
      ResourceDepleted,
      FullStorage,
      Working,
    }
  }
}
