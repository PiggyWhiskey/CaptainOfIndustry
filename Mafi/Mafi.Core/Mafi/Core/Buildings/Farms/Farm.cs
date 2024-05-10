// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.Farm
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.RainwaterHarvesters;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Environment;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  public class Farm : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithSound,
    IEntityWithLogisticsControl,
    IMaintainedEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig,
    IEntityWithPorts
  {
    /// <summary>Amount of slots in crops schedule.</summary>
    public const int CROP_SLOTS_COUNT = 4;
    /// <summary>
    /// How many days can farm stay without workers or paused until crops die.
    /// </summary>
    public const int MAX_DAYS_DISABLED = 15;
    public const char INPUT_WATER_PORT_NAME = 'A';
    public const char INPUT_FERTILIZER_PORT_NAME = 'B';
    /// <summary>
    /// Extra fertility cost when the same crop is growing after itself, in other words it is a penalty for not
    /// rotating crops. This means less efficient farms at the start of the game and forces player to either rotate
    /// crops or pay extra fertilizer.
    /// 
    /// This does not apply to crops that give fertility.
    /// </summary>
    public static readonly Percent FERTILITY_PENALTY_FOR_SAME_CROP;
    /// <summary>
    /// When farm fertility is above 100%, natural degradation is multiplied by this value.
    /// This slows fertility degradation down for over-fertilized fields.
    /// On the other hand, crop demand will raise by
    /// <see cref="F:Mafi.Core.Buildings.Farms.Farm.CROP_FERTILITY_DEMAND_MULT_WHEN_FERTILITY_ABOVE_100" />.
    /// </summary>
    public static readonly Percent FERTILITY_REPLENISH_MULT_WHEN_ABOVE_100;
    /// <summary>
    /// When farm fertility is above 100%, crop demand will be multiplied by this number,
    /// scaled by the difference from 100%.
    /// This makes crops consume more and more fertility on over-fertilized farms.
    /// </summary>
    public static readonly Percent CROP_FERTILITY_DEMAND_MULT_WHEN_FERTILITY_ABOVE_100;
    /// <summary>
    /// Farm starts at 80% fertility to discourage rebuilding farms to gain fertility.
    /// </summary>
    public static readonly Percent STARTING_FERTILITY;
    /// <summary>
    /// When crop is harvester prematurely, crop yield will be zero if it is grown less than this threshold.
    /// 
    /// In UI, the harvest button should be disabled when growth percentage is below this.
    /// </summary>
    public static readonly Percent NO_YIELD_BEFORE_GROWTH_PERC;
    private static readonly Quantity SOIL_WATER_CAPACITY;
    private static readonly Quantity IMPORTED_WATER_CAPACITY;
    private static readonly Quantity FERTILIZER_CAPACITY;
    public const int FERTILITY_SLIDER_STEPS = 14;
    public static readonly Percent FERTILITY_PER_SLIDER_STEP;
    public static readonly Percent MAX_FERTILITY_SLIDER_VALUE;
    /// <summary>
    /// NOTE: Irrigation needs to keep space in soil buffer so heavy rain has a chance to replenish it.
    /// Otherwise rain has lowered benefit which means that dry season would have reduced impact
    /// on later difficulty and we don't want that. So keep space and make it rain!
    /// </summary>
    private static readonly Percent IRRIGATION_START;
    private static readonly Percent IRRIGATION_STOP;
    private static readonly Percent IRRIGATION_STOP_ON_EMPTY_CROP;
    private static readonly Quantity MIN_IRRIGATION_PER_DAY;
    private FarmProto m_proto;
    private readonly ProductBuffer m_soilWaterBuffer;
    private PartialQuantity m_waterToConsume;
    private PartialQuantity m_evaporatedWater;
    private readonly ProductBuffer m_importedWaterBuffer;
    private readonly Dict<ProductProto, GlobalOutputBuffer> m_outputBuffers;
    private Percent m_partialFertilizerBuffer;
    private Percent m_avgFertilityUsedPerDay;
    private bool m_hasWorkers;
    private int m_consecutiveDaysDisabled;
    private readonly Option<CropProto>[] m_schedule;
    private EntityNotificator m_noCropSelectedNotif;
    private EntityNotificatorWithProtoParam m_cropWillDryNotif;
    private EntityNotificatorWithProtoParam m_cropLacksMaintenanceNotif;
    private EntityNotificator m_lowFertilityNotif;
    private readonly IProductsManager m_productsManager;
    private readonly ICalendar m_calendar;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private readonly RainHarvestingHelper m_rainHarvestingHelper;
    private readonly IWeatherManager m_weatherManager;
    private readonly IProperty<bool> m_forceRunEnabled;
    private readonly ProductQuantity[] m_avgYieldPerYear;
    private readonly IProperty<Percent> m_waterConsumptionMult;
    private readonly IProperty<Percent> m_yieldMult;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public FarmProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public bool IsIdleForMaintenance => this.CurrentState != Farm.State.Growing;

    public Farm.State CurrentState { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    /// <summary>Currently planted crop.</summary>
    public Option<Crop> CurrentCrop { get; private set; }

    /// <summary>Previously harvested crop.</summary>
    public Option<Crop> PreviousCrop { get; private set; }

    public bool NotifyOnFullBuffer { get; set; }

    public IProductBufferReadOnly SoilWaterBuffer
    {
      get => (IProductBufferReadOnly) this.m_soilWaterBuffer;
    }

    public PartialQuantity AvgWaterNeedPerMonth { get; private set; }

    public IProductBufferReadOnly ImportedWaterBuffer
    {
      get => (IProductBufferReadOnly) this.m_importedWaterBuffer;
    }

    public IReadOnlyCollection<IProductBuffer> OutputBuffers
    {
      get => (IReadOnlyCollection<IProductBuffer>) this.m_outputBuffers.Values;
    }

    /// <summary>
    /// Current farm fertility. Crop final yield is affected by this value daily.
    /// </summary>
    public Percent Fertility { get; private set; }

    public Percent NaturalFertilityEquilibrium { get; private set; }

    public Percent NaturalReplenishPerDay { get; private set; }

    public Quantity StoredFertilizerCount { get; private set; }

    public Quantity StoredFertilizerCapacity { get; private set; }

    public Percent MaxFertilityProvidedByFertilizer { get; private set; }

    public Percent FertilityPerFertilizer { get; private set; }

    public int FertilityTargetIndex { get; private set; }

    public Percent FertilityTargetValue { get; private set; }

    /// <summary>
    /// Extra fertility needed per day in order to reach <see cref="P:Mafi.Core.Buildings.Farms.Farm.FertilityTargetValue" />.
    /// </summary>
    public Percent FertilityNeededPerDay { get; private set; }

    public ReadOnlyArray<Option<CropProto>> Schedule
    {
      get => this.m_schedule.AsReadOnlyArray<Option<CropProto>>();
    }

    public int ActiveScheduleIndex { get; private set; }

    public int LifetimePlantedCropsCount { get; private set; }

    public bool IsIrrigating { get; private set; }

    public bool ShouldAnimate => this.IsEnabled && this.m_hasWorkers && this.CurrentCrop.HasValue;

    public bool IsSoundOn => this.IsIrrigating;

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.SprinklerSoundPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SprinklerSoundPath.Value, SoundSignificance.Normal));
      }
    }

    public int TotalRotationDurationDays { get; private set; }

    /// <summary>
    /// Yield estimate based on the current rotation and <see cref="P:Mafi.Core.Buildings.Farms.Farm.FertilityTargetValue" />. This does NOT account
    /// for the current <see cref="P:Mafi.Core.Buildings.Farms.Farm.Fertility" />.
    /// </summary>
    public ReadOnlyArray<ProductQuantity> AvgYieldPerYear
    {
      get => this.m_avgYieldPerYear.AsReadOnlyArray<ProductQuantity>();
    }

    public Farm(
      EntityId id,
      FarmProto proto,
      TileTransform transform,
      EntityContext context,
      UnlockedProtosDb unlockedProtosDb,
      IWeatherManager weatherManager,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CNotifyOnFullBuffer\u003Ek__BackingField = true;
      this.m_outputBuffers = new Dict<ProductProto, GlobalOutputBuffer>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxFertilityProvidedByFertilizer\u003Ek__BackingField = Percent.Hundred;
      this.m_schedule = new Option<CropProto>[4];
      this.m_avgYieldPerYear = new ProductQuantity[4];
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_weatherManager = weatherManager;
      this.m_productsManager = productsManager;
      this.m_calendar = calendar;
      this.m_noCropSelectedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoCropToGrow);
      this.m_cropWillDryNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CropWillDrySoon);
      this.m_cropLacksMaintenanceNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CropLacksMaintenance);
      this.m_lowFertilityNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.LowFarmFertility);
      this.Fertility = Farm.STARTING_FERTILITY;
      this.StoredFertilizerCapacity = proto.HasIrrigationAndFertilizerSupport ? Farm.FERTILIZER_CAPACITY : Quantity.Zero;
      ProductProto product = proto.WaterCollectedPerDay.Product;
      this.m_soilWaterBuffer = new ProductBuffer(Farm.SOIL_WATER_CAPACITY, product);
      this.m_importedWaterBuffer = new ProductBuffer(this.Prototype.HasIrrigationAndFertilizerSupport ? Farm.IMPORTED_WATER_CAPACITY : Quantity.Zero, product);
      this.Upgrader = upgraderFactory.CreateInstance<FarmProto, Farm>(this, this.Prototype);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new KeepEmptyGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      this.m_autoLogisticsHelper.SetLogisticsInputMode(EntityLogisticsMode.Off);
      this.m_rainHarvestingHelper = new RainHarvestingHelper(weatherManager, calendar, (IEntity) this, (IProductBuffer) this.m_soilWaterBuffer, this.Prototype.WaterCollectedPerDay.Quantity, context.PropertiesDb);
      this.m_forceRunEnabled = context.PropertiesDb.GetProperty<bool>(IdsCore.PropertyIds.ForceRunAllMachinesEnabled);
      this.m_waterConsumptionMult = context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier);
      this.m_waterConsumptionMult.OnChange.Add<Farm>(this, new Action<Percent>(this.onMultiplierChanged));
      this.m_yieldMult = context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmYieldMultiplier);
      this.m_yieldMult.OnChange.Add<Farm>(this, new Action<Percent>(this.onMultiplierChanged));
      this.recomputeRotationStats();
      calendar.NewDay.Add<Farm>(this, new Action(this.onNewDay));
      calendar.NewMonth.Add<Farm>(this, new Action(this.onNewMonth));
      if (unlockedProtosDb.IsUnlocked(context.ProtosDb.GetOrThrow<Proto>(IdsCore.Technology.CropRotation)))
        return;
      CropProto crop = unlockedProtosDb.AllUnlocked<CropProto>().FirstOrDefault<CropProto>((Func<CropProto, bool>) (x => x.PlantByDefault));
      if (!((Proto) crop != (Proto) null))
        return;
      this.AssignCropToSlot((Option<CropProto>) crop, 0);
    }

    public ProductQuantity GetCurrentYieldEstimate()
    {
      if (this.CurrentCrop.IsNone)
        return ProductQuantity.None;
      Crop crop = this.CurrentCrop.Value;
      Percent yieldSoFar = crop.GetYieldSoFar();
      Percent percent = Percent.FromRatio(crop.DaysRemaining, crop.Prototype.DaysToGrow) * this.Fertility;
      return crop.ProductProduced.ScaledBy(yieldSoFar + percent).ScaledBy(this.m_yieldMult.Value);
    }

    private void onMultiplierChanged(Percent val) => this.recomputeRotationStats();

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        bool flag = !this.Prototype.HasIrrigationAndFertilizerSupport && this.Prototype.Upgrade.NextTier.Value.HasIrrigationAndFertilizerSupport;
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        if (flag)
        {
          this.m_importedWaterBuffer.IncreaseCapacityTo(Farm.IMPORTED_WATER_CAPACITY);
          this.StoredFertilizerCapacity = Farm.FERTILIZER_CAPACITY;
        }
        this.recomputeRotationStats();
      }
    }

    private void onNewMonth()
    {
    }

    private void onNewDay()
    {
      bool isIrrigating = this.IsIrrigating;
      this.IsIrrigating = false;
      bool flag1;
      if (this.IsNotEnabled)
      {
        this.CurrentState = Farm.State.Paused;
        flag1 = true;
      }
      else if (!this.m_hasWorkers)
      {
        this.CurrentState = Farm.State.NotEnoughWorkers;
        flag1 = true;
      }
      else
        flag1 = false;
      if (this.IsConstructed || this.IsBeingUpgraded)
      {
        this.NaturalReplenishPerDay = this.getNaturalReplenishPerDayAt(this.Fertility);
        if (this.m_soilWaterBuffer.IsEmpty && this.NaturalReplenishPerDay.IsPositive)
          this.NaturalReplenishPerDay /= 2;
        this.Fertility += this.NaturalReplenishPerDay;
      }
      if ((this.CurrentCrop.IsNone || this.CurrentCrop.Value.Prototype.IsEmptyCrop) && this.m_soilWaterBuffer.IsNotEmpty())
      {
        this.m_evaporatedWater += this.Prototype.WaterEvaporationPerDay;
        if (this.m_soilWaterBuffer.PercentFull() > Percent.Fifty)
          this.m_evaporatedWater += this.Prototype.WaterEvaporationPerDay;
        if (this.m_evaporatedWater >= PartialQuantity.One)
        {
          this.m_soilWaterBuffer.RemoveExactly(Quantity.One);
          this.m_evaporatedWater -= PartialQuantity.One;
          this.m_productsManager.ProductDestroyed(this.m_soilWaterBuffer.Product, Quantity.One, DestroyReason.Wasted);
        }
      }
      if (flag1)
      {
        if (!this.CurrentCrop.HasValue || this.CurrentCrop.Value.Prototype.IsEmptyCrop || this.ConstructionState == ConstructionState.BeingUpgraded)
          return;
        ++this.m_consecutiveDaysDisabled;
        if (this.m_consecutiveDaysDisabled <= 15)
          return;
        Crop crop = this.CurrentCrop.Value;
        this.harvestCropNow(crop, CropHarvestReason.PrematureLackOfMaintenance);
        if (!this.IsEnabled)
          return;
        this.Context.NotificationsManager.NotifyOnce<Proto>(IdsCore.Notifications.CropDiedNoMaintenance, (Proto) crop.Prototype, (IEntity) this);
      }
      else
      {
        this.m_consecutiveDaysDisabled = 0;
        if (this.CurrentCrop.IsNone)
        {
          Option<CropProto> option = Option<CropProto>.None;
          for (int index1 = 0; index1 < this.m_schedule.Length; ++index1)
          {
            int index2 = (this.ActiveScheduleIndex + index1) % this.m_schedule.Length;
            option = (Option<CropProto>) this.m_schedule[index2].ValueOrNull;
            if (option.HasValue)
            {
              this.ActiveScheduleIndex = index2;
              break;
            }
          }
          if (option.IsNone)
          {
            this.CurrentState = Farm.State.NoCropSelected;
            return;
          }
          CropProto proto = option.Value;
          Percent fertilityPenalty = !proto.ConsumedFertilityPerDay.IsPositive || !this.PreviousCrop.HasValue || !((Proto) proto == (Proto) this.PreviousCrop.Value.Prototype) ? Percent.Zero : Farm.FERTILITY_PENALTY_FOR_SAME_CROP;
          this.CurrentCrop = (Option<Crop>) new Crop(proto, this.Prototype, this.Context.PropertiesDb, fertilityPenalty);
          ++this.LifetimePlantedCropsCount;
        }
        Crop crop = this.CurrentCrop.Value;
        Assert.That<int>(crop.DaysRemaining).IsPositive();
        Quantity quantity1 = this.m_soilWaterBuffer.Capacity;
        quantity1 = quantity1.ScaledBy(Farm.IRRIGATION_START);
        PartialQuantity partialQuantity = quantity1.AsPartial;
        if (!crop.IsStarted)
          partialQuantity = partialQuantity.Max(crop.ConsumedWaterPerDay * 10);
        bool flag2 = isIrrigating && this.m_soilWaterBuffer.PercentFull() < (crop.Prototype.IsEmptyCrop ? Farm.IRRIGATION_STOP_ON_EMPTY_CROP : Farm.IRRIGATION_STOP) || this.m_soilWaterBuffer.Quantity < partialQuantity;
        Percent percent1 = this.m_weatherManager.RainIntensity;
        if (percent1.IsZero & flag2 && this.Maintenance.CanWork())
        {
          quantity1 = (crop.ConsumedWaterPerDay * 2).ToQuantityCeiled();
          Quantity rhs = quantity1.Max(Farm.MIN_IRRIGATION_PER_DAY);
          ProductBuffer importedWaterBuffer = this.m_importedWaterBuffer;
          quantity1 = this.m_soilWaterBuffer.UsableCapacity;
          Quantity maxQuantity = quantity1.Min(rhs);
          Quantity quantity2 = importedWaterBuffer.RemoveAsMuchAs(maxQuantity);
          this.m_soilWaterBuffer.StoreExactly(quantity2);
          this.IsIrrigating = quantity2.IsPositive;
        }
        if (crop.ConsumedFertilityPerDay.IsPositive)
        {
          quantity1 = this.StoredFertilizerCount;
          if ((quantity1.IsPositive || this.m_partialFertilizerBuffer.IsPositive) && this.Maintenance.CanWork())
          {
            percent1 = this.FertilityTargetValue;
            Percent percent2 = percent1.Min(this.MaxFertilityProvidedByFertilizer);
            if (this.Fertility < percent2)
            {
              Percent percent3 = percent2 - this.Fertility;
              if (percent3 > this.m_partialFertilizerBuffer)
              {
                quantity1 = this.StoredFertilizerCount;
                if (quantity1.IsPositive)
                {
                  this.m_partialFertilizerBuffer += this.FertilityPerFertilizer;
                  this.StoredFertilizerCount -= Quantity.One;
                  if (percent3 > this.m_partialFertilizerBuffer)
                    percent3 = this.m_partialFertilizerBuffer;
                }
                else
                  percent3 = this.m_partialFertilizerBuffer;
              }
              this.m_partialFertilizerBuffer -= percent3;
              this.Fertility += percent3;
            }
          }
        }
        this.CurrentState = this.growCrop(crop);
        if (this.Maintenance.CanWork())
          return;
        this.CurrentState = Farm.State.Broken;
      }
    }

    /// <summary>
    /// Returns natural fertility replenish rate per day for the given soil fertility level.
    /// </summary>
    private Percent getNaturalReplenishPerDayAt(Percent fertility)
    {
      Percent replenishPerDayAt = (Percent.Hundred - fertility) * this.Prototype.FertilityReplenishPerDay;
      if (replenishPerDayAt.IsNegative)
        replenishPerDayAt = replenishPerDayAt.ScaleBy(Farm.FERTILITY_REPLENISH_MULT_WHEN_ABOVE_100);
      return replenishPerDayAt;
    }

    private Farm.State growCrop(Crop crop)
    {
      if (crop.ConsumedWaterPerDay > this.m_waterToConsume)
      {
        Quantity quantityCeiled = (crop.ConsumedWaterPerDay - this.m_waterToConsume).ToQuantityCeiled();
        if (this.m_soilWaterBuffer.TryRemove(quantityCeiled))
        {
          this.m_productsManager.ProductDestroyed(this.m_soilWaterBuffer.Product.WithQuantity(quantityCeiled), DestroyReason.Farms);
          this.m_waterToConsume += quantityCeiled.AsPartial;
        }
      }
      if (!crop.IsStarted)
      {
        if (crop.ConsumedWaterPerDay * 10 > this.m_waterToConsume + this.m_soilWaterBuffer.Quantity.AsPartial)
        {
          crop.ReportWaitingForWater();
          return Farm.State.NotEnoughWater;
        }
        if (this.Fertility < crop.Prototype.MinFertilityToStartGrowth)
          return Farm.State.LowFertility;
      }
      Farm.State state;
      if (crop.ConsumedWaterPerDay > this.m_waterToConsume)
      {
        bool driedOut;
        crop.RecordGrowthDayNoWater(this.Fertility, out driedOut);
        if (driedOut)
        {
          this.harvestCropNow(crop, CropHarvestReason.PrematureNoWater);
          this.Context.NotificationsManager.NotifyOnce<Proto>(IdsCore.Notifications.CropDiedNoWater, (Proto) crop.Prototype, (IEntity) this);
          return Farm.State.NotEnoughWater;
        }
        state = Farm.State.NotEnoughWater;
      }
      else
      {
        crop.RecordGrowthDay(this.Fertility);
        this.m_waterToConsume -= crop.ConsumedWaterPerDay;
        Percent percent = crop.ConsumedFertilityPerDay;
        if (percent.IsPositive)
        {
          if (this.Fertility > Percent.Hundred)
            percent += percent * (this.Fertility - Percent.Hundred) * Farm.CROP_FERTILITY_DEMAND_MULT_WHEN_FERTILITY_ABOVE_100;
          else if (percent > this.Fertility)
          {
            this.harvestCropNow(crop, CropHarvestReason.PrematureNoFertility);
            this.Context.NotificationsManager.NotifyOnce<Proto>(IdsCore.Notifications.CropDiedNoFertility, (Proto) crop.Prototype, (IEntity) this);
            return Farm.State.LowFertility;
          }
        }
        else if (this.Fertility > Percent.Hundred)
          percent = percent.ScaleBy(Farm.FERTILITY_REPLENISH_MULT_WHEN_ABOVE_100);
        this.Fertility -= percent;
        state = Farm.State.Growing;
      }
      if (crop.DaysRemaining <= 0)
        this.harvestCropNow(crop, CropHarvestReason.HarvestedNormally);
      return state;
    }

    public void HarvestNow()
    {
      if (!this.CurrentCrop.HasValue)
        return;
      this.harvestCropNow(this.CurrentCrop.Value, CropHarvestReason.PrematureHarvestedByPlayer);
    }

    private void harvestCropNow(Crop crop, CropHarvestReason harvestReason)
    {
      ProductQuantity productQuantity = crop.Harvest(harvestReason);
      if (productQuantity.IsNotEmpty)
      {
        ProductBuffer outputBufferFor = this.getOrCreateOutputBufferFor(productQuantity.Product);
        outputBufferFor.IncreaseCapacityTo(productQuantity.Quantity + TruckCaps.LargeTruckCapacity);
        Quantity quantity = outputBufferFor.StoreAsMuchAsReturnStored(productQuantity.Quantity);
        if (quantity.IsPositive)
          this.m_productsManager.ProductCreated(productQuantity.Product.WithQuantity(quantity), CreateReason.Produced);
        if (quantity < productQuantity.Quantity && this.NotifyOnFullBuffer)
          this.Context.NotificationsManager.NotifyOnce<Proto>(IdsCore.Notifications.CropCouldNotBeStored, (Proto) crop.Prototype, (IEntity) this);
      }
      if (!crop.Prototype.IsEmptyCrop)
        this.PreviousCrop = (Option<Crop>) crop;
      this.CurrentCrop = Option<Crop>.None;
      this.CurrentState = Farm.State.None;
      this.ActiveScheduleIndex = (this.ActiveScheduleIndex + 1) % this.m_schedule.Length;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (pq.Quantity.IsNotPositive || this.IsNotEnabled)
        return pq.Quantity;
      if (sourcePort.Name == 'A')
        return !((Proto) pq.Product == (Proto) this.m_importedWaterBuffer.Product) ? pq.Quantity : this.m_importedWaterBuffer.StoreAsMuchAs(pq);
      if (sourcePort.Name == 'B')
      {
        FertilizerProductParam paramValue;
        if (this.StoredFertilizerCount >= this.StoredFertilizerCapacity || !pq.Product.TryGetParam<FertilizerProductParam>(out paramValue))
          return pq.Quantity;
        Quantity quantity = (this.StoredFertilizerCapacity - this.StoredFertilizerCount).Min(pq.Quantity);
        this.mixInNewFertilizer(quantity, paramValue);
        this.m_productsManager.ProductDestroyed(pq.Product, quantity, DestroyReason.Farms);
        return pq.Quantity - quantity;
      }
      Log.Warning(string.Format("Unexpected input port at index '{0}' on farm", (object) sourcePort.PortIndex));
      return pq.Quantity;
    }

    private void mixInNewFertilizer(Quantity quantity, FertilizerProductParam paramValue)
    {
      Quantity quantity1 = this.StoredFertilizerCount + quantity;
      Percent percent = (this.MaxFertilityProvidedByFertilizer * this.StoredFertilizerCount.Value + paramValue.MaxFertility * quantity.Value) / quantity1.Value;
      this.FertilityPerFertilizer = (this.FertilityPerFertilizer * this.StoredFertilizerCount.Value + paramValue.FertilityPerQuantity * quantity.Value) / quantity1.Value;
      this.StoredFertilizerCount = quantity1;
      if (!(percent != this.MaxFertilityProvidedByFertilizer))
        return;
      this.MaxFertilityProvidedByFertilizer = percent;
      this.recomputeYieldEstimates();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsEnabled)
        this.m_hasWorkers = Entity.HasWorkers((IEntityWithWorkers) this);
      this.m_noCropSelectedNotif.NotifyIff(this.IsEnabled && this.CurrentState == Farm.State.NoCropSelected, (IEntity) this);
      this.m_lowFertilityNotif.NotifyIff(this.IsEnabled && this.CurrentState == Farm.State.LowFertility, (IEntity) this);
      if (this.CurrentCrop.HasValue && this.ConstructionState == ConstructionState.Constructed && !this.IsPaused)
      {
        Crop crop = this.CurrentCrop.Value;
        this.m_cropLacksMaintenanceNotif.NotifyIff((Proto) crop.Prototype, this.m_consecutiveDaysDisabled > 0, (IEntity) this);
        this.m_cropWillDryNotif.NotifyIff((Proto) crop.Prototype, !this.m_cropLacksMaintenanceNotif.IsActive && crop.WillDrySoon, (IEntity) this);
      }
      else
      {
        this.m_cropLacksMaintenanceNotif.Deactivate((IEntity) this);
        this.m_cropWillDryNotif.Deactivate((IEntity) this);
      }
      Option<ProductBuffer> option = (Option<ProductBuffer>) Option.None;
      foreach (GlobalOutputBuffer globalOutputBuffer in this.m_outputBuffers.Values)
      {
        if (globalOutputBuffer.IsEmpty())
        {
          option = (Option<ProductBuffer>) (ProductBuffer) globalOutputBuffer;
        }
        else
        {
          foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
          {
            if (!(connectedOutputPort.AllowedProductType != globalOutputBuffer.Product.Type))
            {
              Quantity quantity1 = globalOutputBuffer.Quantity;
              Quantity quantity2 = quantity1 - connectedOutputPort.SendAsMuchAs(globalOutputBuffer.Product.WithQuantity(quantity1));
              if (quantity2.IsPositive)
              {
                globalOutputBuffer.RemoveExactly(quantity2);
                this.m_autoLogisticsHelper.OnProductSentToPort((IProductBuffer) globalOutputBuffer);
              }
              if (globalOutputBuffer.IsEmpty())
                break;
            }
          }
        }
      }
      if (!option.HasValue)
        return;
      this.m_outputBuffers.Remove(option.Value.Product);
      this.m_autoLogisticsHelper.RemoveOutputBuffer((IProductBuffer) option.Value);
      option.Value.Destroy();
    }

    public void AssignCropToSlot(Option<CropProto> crop, int slot)
    {
      if (slot < 0 || slot >= this.m_schedule.Length)
        Log.Error(string.Format("Invalid crop slot index {0}!", (object) slot));
      else if (crop.HasValue && crop.Value.RequiresGreenhouse && !this.Prototype.IsGreenhouse)
      {
        Log.Error(string.Format("Crop '{0}' requires a greenhouse and '{1}' is not a greenhouse", (object) crop.Value.Id, (object) this.Prototype.Id));
      }
      else
      {
        this.m_schedule[slot] = crop;
        this.recomputeRotationStats();
      }
    }

    public void SetFertilityTarget(Percent fertility)
    {
      this.FertilityTargetIndex = (fertility.ToFix32() / Farm.FERTILITY_PER_SLIDER_STEP.ToFix32()).ToIntRounded().Clamp(0, 14);
      this.FertilityTargetValue = this.FertilityTargetIndex * Farm.FERTILITY_PER_SLIDER_STEP;
      this.recomputeYieldEstimates();
    }

    private void recomputeRotationStats()
    {
      this.TotalRotationDurationDays = 0;
      CropProto cropProto1 = (CropProto) null;
      for (int index = this.m_schedule.Length - 1; index >= 0; --index)
      {
        if (this.m_schedule[index].HasValue && !this.m_schedule[index].Value.IsEmptyCrop)
        {
          cropProto1 = this.m_schedule[index].Value;
          break;
        }
      }
      if ((Proto) cropProto1 == (Proto) null)
      {
        this.NaturalFertilityEquilibrium = Percent.Hundred;
        this.m_avgFertilityUsedPerDay = Percent.Zero;
        this.recomputeYieldEstimates();
      }
      else
      {
        Percent zero1 = Percent.Zero;
        PartialQuantity zero2 = PartialQuantity.Zero;
        for (int index = 0; index < this.m_schedule.Length; ++index)
        {
          if (!this.m_schedule[index].IsNone)
          {
            CropProto cropProto2 = this.m_schedule[index].Value;
            bool flag = (Proto) cropProto1 == (Proto) cropProto2;
            this.TotalRotationDurationDays += cropProto2.DaysToGrow;
            Percent consumedFertilityPerDay = cropProto2.GetConsumedFertilityPerDay(this.Prototype);
            if (consumedFertilityPerDay.IsPositive & flag)
              consumedFertilityPerDay += consumedFertilityPerDay * Farm.FERTILITY_PENALTY_FOR_SAME_CROP;
            zero1 += consumedFertilityPerDay * cropProto2.DaysToGrow;
            zero2 += cropProto2.GetConsumedWaterPerDay(this.Prototype).ScaledBy(this.m_waterConsumptionMult.Value) * cropProto2.DaysToGrow;
            if (!cropProto2.IsEmptyCrop)
              cropProto1 = cropProto2;
          }
        }
        this.AvgWaterNeedPerMonth = zero2 * 30 / this.TotalRotationDurationDays;
        this.m_avgFertilityUsedPerDay = zero1 / this.TotalRotationDurationDays;
        this.NaturalFertilityEquilibrium = (Percent.Hundred - this.m_avgFertilityUsedPerDay / this.Prototype.FertilityReplenishPerDay).Max(Percent.Zero);
        this.recomputeYieldEstimates();
      }
    }

    /// <summary>
    /// Recomputes yield estimates `m_avgYieldPerYear` and `FertilityNeededPerDay`.
    /// Needs to be recomputed when any of these change:
    /// * m_schedule
    /// * NaturalFertilityEquilibrium, m_avgFertilityUsedPerDay
    /// * FertilitySliderValue
    /// * MaxFertilityProvidedByFertilizer
    /// </summary>
    private void recomputeYieldEstimates()
    {
      for (int index = 0; index < this.m_avgYieldPerYear.Length; ++index)
        this.m_avgYieldPerYear[index] = ProductQuantity.None;
      int num = 0;
      foreach (Option<CropProto> option in this.m_schedule)
      {
        if (!option.IsNone)
        {
          CropProto cropProto = option.Value;
          ProductQuantity productProduced = cropProto.GetProductProduced(this.Prototype);
          if (!productProduced.IsEmpty)
          {
            Percent percent1 = Percent.FromRatio(360, cropProto.DaysToGrow);
            Percent percent2 = Percent.FromRatio(cropProto.DaysToGrow, this.TotalRotationDurationDays);
            Percent fertilityEquilibrium = this.NaturalFertilityEquilibrium;
            ref Percent local = ref fertilityEquilibrium;
            Quantity quantity1 = this.StoredFertilizerCount;
            Percent rhs = quantity1.IsPositive ? this.FertilityTargetValue.Min(this.MaxFertilityProvidedByFertilizer) : this.FertilityTargetValue;
            Percent percent3 = local.Max(rhs);
            quantity1 = productProduced.Quantity.ScaledBy(percent1 * percent3 * percent2);
            Quantity quantity2 = quantity1.ScaledBy(this.m_yieldMult.Value);
            int index1 = -1;
            for (int index2 = 0; index2 < num; ++index2)
            {
              if ((Proto) this.m_avgYieldPerYear[index2].Product == (Proto) productProduced.Product)
              {
                index1 = index2;
                quantity2 += this.m_avgYieldPerYear[index2].Quantity;
                break;
              }
            }
            if (index1 < 0)
            {
              index1 = num;
              ++num;
            }
            this.m_avgYieldPerYear[index1] = new ProductQuantity(productProduced.Product, quantity2);
          }
        }
      }
      if (this.FertilityTargetValue <= this.NaturalFertilityEquilibrium)
      {
        this.FertilityNeededPerDay = Percent.Zero;
      }
      else
      {
        Percent fertilityUsedPerDay = this.m_avgFertilityUsedPerDay;
        if (this.FertilityTargetValue > Percent.Hundred && fertilityUsedPerDay.IsPositive)
          fertilityUsedPerDay += fertilityUsedPerDay * (this.FertilityTargetValue - Percent.Hundred) * Farm.CROP_FERTILITY_DEMAND_MULT_WHEN_FERTILITY_ABOVE_100;
        Percent replenishPerDayAt = this.getNaturalReplenishPerDayAt(this.FertilityTargetValue);
        this.FertilityNeededPerDay = fertilityUsedPerDay - replenishPerDayAt;
      }
    }

    public void ForceNextSlot()
    {
      if (this.CurrentCrop.HasValue)
        this.harvestCropNow(this.CurrentCrop.Value, CropHarvestReason.PrematureClearedByPlayer);
      else
        this.ActiveScheduleIndex = (this.ActiveScheduleIndex + 1) % this.m_schedule.Length;
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDay.Remove<Farm>(this, new Action(this.onNewDay));
      this.m_calendar.NewMonth.Remove<Farm>(this, new Action(this.onNewMonth));
      this.CurrentCrop = Option<Crop>.None;
      this.m_productsManager.ClearProduct(this.m_soilWaterBuffer.Product, this.m_soilWaterBuffer.Clear());
      this.m_productsManager.ClearProduct(this.m_importedWaterBuffer.Product, this.m_importedWaterBuffer.Clear());
      foreach (IProductBuffer buffer in this.m_outputBuffers.Values)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(buffer);
      this.m_waterConsumptionMult.OnChange.Remove<Farm>(this, new Action<Percent>(this.onMultiplierChanged));
      this.m_yieldMult.OnChange.Remove<Farm>(this, new Action<Percent>(this.onMultiplierChanged));
      base.OnDestroy();
    }

    private ProductBuffer getOrCreateOutputBufferFor(ProductProto product)
    {
      GlobalOutputBuffer outputBufferFor;
      if (this.m_outputBuffers.TryGetValue(product, out outputBufferFor))
        return (ProductBuffer) outputBufferFor;
      GlobalOutputBuffer buffer = new GlobalOutputBuffer(0.Quantity(), product, this.m_productsManager, 15, (IEntity) this);
      this.m_outputBuffers.Add(buffer.Product, buffer);
      this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) buffer);
      return (ProductBuffer) buffer;
    }

    public bool CanDisableLogisticsInput => false;

    public bool CanDisableLogisticsOutput => true;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode
    {
      get => this.m_autoLogisticsHelper.LogisticsOutputMode;
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetFertilityTarget(this.FertilityTargetValue);
      data.NotifyOnFullBuffer = new bool?(this.NotifyOnFullBuffer);
      data.SetCropSchedule(new ImmutableArray<Option<CropProto>>?(((ICollection<Option<CropProto>>) this.m_schedule).ToImmutableArray<Option<CropProto>>()));
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      Percent? fertilityTarget = data.GetFertilityTarget();
      if (fertilityTarget.HasValue)
        this.SetFertilityTarget(fertilityTarget.Value);
      bool? notifyOnFullBuffer = data.NotifyOnFullBuffer;
      if (notifyOnFullBuffer.HasValue)
        this.NotifyOnFullBuffer = notifyOnFullBuffer.Value;
      ImmutableArray<Option<CropProto>>? cropSchedule = data.GetCropSchedule();
      if (!cropSchedule.HasValue || !this.Context.UnlockedProtosDb.IsUnlocked(this.Context.ProtosDb.GetOrThrow<Proto>(IdsCore.Technology.CropRotation)))
        return;
      for (int index = 0; index < cropSchedule.Value.Length; ++index)
      {
        Option<CropProto> crop = cropSchedule.Value[index];
        if (!crop.HasValue || !crop.Value.RequiresGreenhouse || this.Prototype.IsGreenhouse)
          this.AssignCropToSlot(crop, index);
      }
    }

    public static void Serialize(Farm value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Farm>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Farm.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.ActiveScheduleIndex);
      PartialQuantity.Serialize(this.AvgWaterNeedPerMonth, writer);
      Option<Crop>.Serialize(this.CurrentCrop, writer);
      writer.WriteInt((int) this.CurrentState);
      Percent.Serialize(this.Fertility, writer);
      Percent.Serialize(this.FertilityNeededPerDay, writer);
      Percent.Serialize(this.FertilityPerFertilizer, writer);
      writer.WriteInt(this.FertilityTargetIndex);
      Percent.Serialize(this.FertilityTargetValue, writer);
      writer.WriteBool(this.IsIrrigating);
      writer.WriteInt(this.LifetimePlantedCropsCount);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      Percent.Serialize(this.m_avgFertilityUsedPerDay, writer);
      writer.WriteArray<ProductQuantity>(this.m_avgYieldPerYear);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteInt(this.m_consecutiveDaysDisabled);
      EntityNotificatorWithProtoParam.Serialize(this.m_cropLacksMaintenanceNotif, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_cropWillDryNotif, writer);
      PartialQuantity.Serialize(this.m_evaporatedWater, writer);
      writer.WriteGeneric<IProperty<bool>>(this.m_forceRunEnabled);
      writer.WriteBool(this.m_hasWorkers);
      ProductBuffer.Serialize(this.m_importedWaterBuffer, writer);
      EntityNotificator.Serialize(this.m_lowFertilityNotif, writer);
      EntityNotificator.Serialize(this.m_noCropSelectedNotif, writer);
      Dict<ProductProto, GlobalOutputBuffer>.Serialize(this.m_outputBuffers, writer);
      Percent.Serialize(this.m_partialFertilizerBuffer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<FarmProto>(this.m_proto);
      RainHarvestingHelper.Serialize(this.m_rainHarvestingHelper, writer);
      writer.WriteArray<Option<CropProto>>(this.m_schedule);
      ProductBuffer.Serialize(this.m_soilWaterBuffer, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_waterConsumptionMult);
      PartialQuantity.Serialize(this.m_waterToConsume, writer);
      writer.WriteGeneric<IWeatherManager>(this.m_weatherManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_yieldMult);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Percent.Serialize(this.MaxFertilityProvidedByFertilizer, writer);
      Percent.Serialize(this.NaturalFertilityEquilibrium, writer);
      Percent.Serialize(this.NaturalReplenishPerDay, writer);
      writer.WriteBool(this.NotifyOnFullBuffer);
      Option<Crop>.Serialize(this.PreviousCrop, writer);
      Quantity.Serialize(this.StoredFertilizerCapacity, writer);
      Quantity.Serialize(this.StoredFertilizerCount, writer);
      writer.WriteInt(this.TotalRotationDurationDays);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Farm Deserialize(BlobReader reader)
    {
      Farm farm;
      if (reader.TryStartClassDeserialization<Farm>(out farm))
        reader.EnqueueDataDeserialization((object) farm, Farm.s_deserializeDataDelayedAction);
      return farm;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ActiveScheduleIndex = reader.ReadInt();
      this.AvgWaterNeedPerMonth = PartialQuantity.Deserialize(reader);
      this.CurrentCrop = Option<Crop>.Deserialize(reader);
      this.CurrentState = (Farm.State) reader.ReadInt();
      this.Fertility = Percent.Deserialize(reader);
      this.FertilityNeededPerDay = Percent.Deserialize(reader);
      this.FertilityPerFertilizer = Percent.Deserialize(reader);
      this.FertilityTargetIndex = reader.ReadInt();
      this.FertilityTargetValue = Percent.Deserialize(reader);
      this.IsIrrigating = reader.ReadBool();
      this.LifetimePlantedCropsCount = reader.ReadInt();
      reader.SetField<Farm>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      this.m_avgFertilityUsedPerDay = Percent.Deserialize(reader);
      reader.SetField<Farm>(this, "m_avgYieldPerYear", (object) reader.ReadArray<ProductQuantity>());
      reader.SetField<Farm>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_consecutiveDaysDisabled = reader.ReadInt();
      this.m_cropLacksMaintenanceNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_cropWillDryNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_evaporatedWater = PartialQuantity.Deserialize(reader);
      reader.SetField<Farm>(this, "m_forceRunEnabled", (object) reader.ReadGenericAs<IProperty<bool>>());
      this.m_hasWorkers = reader.ReadBool();
      reader.SetField<Farm>(this, "m_importedWaterBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_lowFertilityNotif = EntityNotificator.Deserialize(reader);
      this.m_noCropSelectedNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<Farm>(this, "m_outputBuffers", (object) Dict<ProductProto, GlobalOutputBuffer>.Deserialize(reader));
      this.m_partialFertilizerBuffer = Percent.Deserialize(reader);
      reader.SetField<Farm>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<FarmProto>();
      reader.SetField<Farm>(this, "m_rainHarvestingHelper", (object) RainHarvestingHelper.Deserialize(reader));
      reader.SetField<Farm>(this, "m_schedule", (object) reader.ReadArray<Option<CropProto>>());
      reader.SetField<Farm>(this, "m_soilWaterBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<Farm>(this, "m_waterConsumptionMult", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_waterToConsume = PartialQuantity.Deserialize(reader);
      reader.SetField<Farm>(this, "m_weatherManager", (object) reader.ReadGenericAs<IWeatherManager>());
      reader.SetField<Farm>(this, "m_yieldMult", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.MaxFertilityProvidedByFertilizer = Percent.Deserialize(reader);
      this.NaturalFertilityEquilibrium = Percent.Deserialize(reader);
      this.NaturalReplenishPerDay = Percent.Deserialize(reader);
      this.NotifyOnFullBuffer = reader.ReadBool();
      this.PreviousCrop = Option<Crop>.Deserialize(reader);
      this.StoredFertilizerCapacity = Quantity.Deserialize(reader);
      this.StoredFertilizerCount = Quantity.Deserialize(reader);
      this.TotalRotationDurationDays = reader.ReadInt();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static Farm()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Farm.FERTILITY_PENALTY_FOR_SAME_CROP = 50.Percent();
      Farm.FERTILITY_REPLENISH_MULT_WHEN_ABOVE_100 = 20.Percent();
      Farm.CROP_FERTILITY_DEMAND_MULT_WHEN_FERTILITY_ABOVE_100 = 200.Percent();
      Farm.STARTING_FERTILITY = 80.Percent();
      Farm.NO_YIELD_BEFORE_GROWTH_PERC = 50.Percent();
      Farm.SOIL_WATER_CAPACITY = 50.Quantity();
      Farm.IMPORTED_WATER_CAPACITY = 60.Quantity();
      Farm.FERTILIZER_CAPACITY = 40.Quantity();
      Farm.FERTILITY_PER_SLIDER_STEP = 10.Percent();
      Farm.MAX_FERTILITY_SLIDER_VALUE = Farm.FERTILITY_PER_SLIDER_STEP * 14;
      Farm.IRRIGATION_START = 10.Percent();
      Farm.IRRIGATION_STOP = 65.Percent();
      Farm.IRRIGATION_STOP_ON_EMPTY_CROP = 35.Percent();
      Farm.MIN_IRRIGATION_PER_DAY = 1.Quantity();
      Farm.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Farm.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      None,
      Paused,
      Broken,
      NotEnoughWorkers,
      NoCropSelected,
      NotEnoughWater,
      LowFertility,
      Growing,
    }
  }
}
