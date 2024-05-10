// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Environment;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.NuclearReactors
{
  /// <summary>
  /// Nuclear reactor that produces large amounts of steam.
  /// </summary>
  /// <remarks>
  /// Reactor can start operation only when it is has people, is fully maintained, and is filled with
  /// <see cref="F:Mafi.Core.Factory.NuclearReactors.NuclearReactor.MIN_FUEL_FOR_OPERATION" /> of nuclear fuel. We require maintenance because reactor gets damaged
  /// during meltdown.
  /// 
  /// 
  /// Fuel creates heat that is used to produce steam. If there is too much heat, emergency cooling is used to
  /// prevent overheating. If heat reaches over <see cref="P:Mafi.Core.Factory.NuclearReactors.NuclearReactor.MeltdownAtHeat" /> meltdown occurs, steam production
  /// stops and reactor is being shut down. When reactor heat is above <see cref="P:Mafi.Core.Factory.NuclearReactors.NuclearReactor.MeltdownAtHeat" />, every
  /// <see cref="F:Mafi.Core.Factory.NuclearReactors.NuclearReactor.MELTDOWN_OVERHEAT_DAMAGE_INTERVAL" /> following happens:
  ///  1) Fuel is wasted.
  ///  2) Maintenance is lost.
  ///  3) Radiation escapes.
  /// This means that the worse meltdown is the more is lost in process. Very short lasting meltdown won't cause
  /// much damage but completely uncontrolled meltdown (high power, no emergency cooling) may cause a lot of damage
  /// and release a lot of radiation.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public class NuclearReactor : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithPorts,
    IEntityWithSimpleLogisticsControl,
    IRecipeExecutorForUi,
    IOutputBufferPriorityProvider,
    IInputBufferPriorityProvider,
    IEntityWithEmission,
    IComputingConsumingEntity,
    IEntityWithSimUpdate,
    IEntityWithSound
  {
    public const int FUEL_CAPACITY = 40;
    public const int MIN_FUEL_FOR_OPERATION = 16;
    public const int HEAT_PER_POWER_LEVEL_PER_TICK = 100;
    public const int HEAT_REMOVED_IN_MELTDOWN_PER_TICK = 100;
    public const int SELF_COOLING_PER_TICK = 20;
    public const int HEAT_EXCHANGER_HEAT_CAPACITY_MULT = 2;
    public static readonly Duration POWER_LEVEL_HEAT_CAPACITY_DURATION;
    public static readonly Percent MAX_POWER_INCREASE_PER_TICK;
    public static readonly Percent MAX_POWER_DECREASE_PER_TICK;
    public static readonly Duration MELTDOWN_OVERHEAT_DAMAGE_INTERVAL;
    public static readonly Percent DECREASE_MAINTENANCE_MELTDOWN_DAMAGE;
    public static readonly int POWER_LEVEL_HEAT_CAPACITY;
    public static readonly int SELF_COOLING_HEAT_MARGIN;
    private NuclearReactorProto m_proto;
    private readonly IProductsManager m_productsManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private readonly IRadiationManager m_radiationManager;
    private readonly IComputingConsumer m_computingConsumer;
    private int m_heatPerOutput;
    private int m_highHeatForCurrentPower;
    /// <summary>Heat in exchanger ready to convert water into steam.</summary>
    private int m_heatInExchanger;
    private int m_maxHeatInExchanger;
    private int m_heatInEmergencyCooler;
    private Duration m_meltdownEventRemaining;
    [DoNotSave(129, null)]
    private readonly Queueue<NuclearReactorProto.FuelData> m_loadedFuel;
    private NuclearReactorProto.FuelData? m_currentFuel;
    private Percent m_fuelRemainingLife;
    private int m_currentFuelRecipeIndex;
    private readonly ProductBuffer m_waterInBuffer;
    private readonly ProductBuffer m_steamOutBuffer;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_steamOutputPorts;
    private int m_nextOutPortIndex;
    private readonly Lyst<KeyValuePair<ProductProto, ProductBuffer>> m_fuelInBuffers;
    private readonly Lyst<KeyValuePair<ProductProto, ProductBuffer>> m_spentFuelBuffers;
    [DoNotSave(0, null)]
    private IoPortData m_fuelOutputPort;
    private readonly ProductBuffer m_coolantInBuffer;
    private readonly ProductBuffer m_coolantOutBuffer;
    [DoNotSave(0, null)]
    private IoPortData? m_coolantOutputPort;
    private readonly Lyst<NuclearReactorProto.FuelData> m_allowedFuel;
    private int m_automationIncAtHeatInExchanger;
    private int m_automationDecAtHeatInExchanger;
    private EntityNotificator m_isInMeltdownNotif;
    private EntityNotificator m_lacksMaintenanceNotif;
    private readonly Option<ProductBuffer> m_enrichmentInputBuffer;
    private PartialQuantity m_enrichmentInputBufferToPartial;
    private readonly Option<ProductBuffer> m_enrichmentOutputBuffer;
    private PartialQuantity m_enrichmentOutputBufferPartial;
    [DoNotSave(0, null)]
    private IoPortData m_enrichedOutputPort;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int MaxPowerLevel => this.Prototype.MaxPowerLevel;

    public Percent MaxPowerLevelPercent => this.MaxPowerLevel * Percent.Hundred;

    public int MeltdownAtHeat
    {
      get => (this.MaxPowerLevel + 1) * NuclearReactor.POWER_LEVEL_HEAT_CAPACITY;
    }

    [DoNotSave(0, null)]
    public NuclearReactorProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
        this.recomputeValuesBasedOnProto();
      }
    }

    public override bool CanBePaused => this.CurrentPowerLevel.IsNotPositive;

    public IUpgrader Upgrader { get; private set; }

    public event Action<Quantity> OnSpentFuelGenerated;

    public LogisticsControl LogisticsInputControl => LogisticsControl.Enabled;

    public LogisticsControl LogisticsOutputControl => LogisticsControl.Enabled;

    public bool IsLogisticsInputDisabled { get; private set; }

    public bool IsLogisticsOutputDisabled { get; private set; }

    public override bool IsCargoAffectedByGeneralPriority => true;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => false;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public NuclearReactor.State CurrentState { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    [DoNotSave(0, null)]
    public ImmutableArray<IRecipeForUi> ActiveRecipes { get; private set; }

    [DoNotSave(0, null)]
    public ImmutableArray<IRecipeForUi> AllRecipes { get; private set; }

    public float? EmissionIntensity
    {
      get
      {
        return new float?(this.CurrentPowerLevel.IsPositive ? (this.CurrentPowerLevel / this.MaxPowerLevelPercent).Apply(this.Prototype.Graphics.MaxEmissionIntensity) : 0.0f);
      }
    }

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.SoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    public bool IsSoundOn
    {
      get
      {
        return this.IsEnabled && this.CurrentState == NuclearReactor.State.Working && this.CurrentPowerLevel.IsPositive;
      }
    }

    public Computing ComputingRequired
    {
      get
      {
        return !this.IsAutomaticPowerRegulationEnabled ? Computing.Zero : this.Prototype.ComputingConsumed;
      }
    }

    public Option<IComputingConsumerReadonly> ComputingConsumer
    {
      get => this.m_computingConsumer.SomeOption<IComputingConsumerReadonly>();
    }

    public Percent DurationMultiplier => Percent.Hundred;

    /// <summary>
    /// Heat currently stored in reactor. One power level will generate <see cref="F:Mafi.Core.Factory.NuclearReactors.NuclearReactor.HEAT_PER_POWER_LEVEL_PER_TICK" />
    /// units of heat per tick.
    /// </summary>
    public int HeatAmount { get; private set; }

    public int OptimalHeatForCurrentPower { get; private set; }

    public int StartEmergencyCoolingAtHeat { get; private set; }

    public bool IsInMeltdown { get; private set; }

    /// <summary>Current power level where 100% is one power level.</summary>
    public Percent CurrentPowerLevel { get; private set; }

    /// <summary>
    /// Target power level where 100% is one power level, set by the player.
    /// This will be automatically set to zero if meltdown happens.
    /// </summary>
    public Percent TargetPowerLevel { get; private set; }

    public IProductBuffer CoolantInBuffer => (IProductBuffer) this.m_coolantInBuffer;

    public IProductBuffer CoolantOutBuffer => (IProductBuffer) this.m_coolantOutBuffer;

    /// <summary>
    /// Specifies input fuel protos that can be loaded into the reactor. Order defines fuel usage priority.
    /// </summary>
    public IIndexable<NuclearReactorProto.FuelData> AllowedFuel
    {
      get => (IIndexable<NuclearReactorProto.FuelData>) this.m_allowedFuel;
    }

    public bool IsAutomaticPowerRegulationSupported => this.Prototype.ComputingConsumed.IsPositive;

    public bool IsAutomaticPowerRegulationEnabled { get; private set; }

    bool IRecipeExecutorForUi.IsBoosted => false;

    bool IRecipeExecutorForUi.WorkedThisTick
    {
      get => this.CurrentPowerLevel.IsPositive || this.CurrentState == NuclearReactor.State.Idle;
    }

    public Option<IProductBufferReadOnly> EnrichmentInputBuffer
    {
      get => this.m_enrichmentInputBuffer.As<IProductBufferReadOnly>();
    }

    public Option<IProductBufferReadOnly> EnrichmentOutputBuffer
    {
      get => this.m_enrichmentOutputBuffer.As<IProductBufferReadOnly>();
    }

    public NuclearReactor(
      EntityId id,
      NuclearReactorProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAssetTransactionManager assetTransactionManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IRadiationManager radiationManager,
      INotificationsManager notifManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_loadedFuel = new Queueue<NuclearReactorProto.FuelData>();
      this.m_allowedFuel = new Lyst<NuclearReactorProto.FuelData>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_productsManager = productsManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_radiationManager = radiationManager;
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumer((IComputingConsumingEntity) this);
      this.Upgrader = upgraderFactory.CreateInstance<NuclearReactorProto, NuclearReactor>(this, proto);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_isInMeltdownNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.NuclearReactorInMeltdown);
      this.m_lacksMaintenanceNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.NuclearReactorLacksMaintenance);
      Quantity capacity1 = proto.WaterInPerPowerLevel.Quantity * 2;
      this.m_waterInBuffer = new ProductBuffer(capacity1, proto.WaterInPerPowerLevel.Product);
      this.m_steamOutBuffer = new ProductBuffer(capacity1, proto.SteamOutPerPowerLevel.Product);
      this.m_coolantInBuffer = new ProductBuffer(capacity1, proto.CoolantIn);
      this.m_coolantOutBuffer = new ProductBuffer(capacity1, proto.CoolantOut);
      int length = proto.FuelPairs.Length;
      this.m_fuelInBuffers = new Lyst<KeyValuePair<ProductProto, ProductBuffer>>(length);
      this.m_spentFuelBuffers = new Lyst<KeyValuePair<ProductProto, ProductBuffer>>(length);
      foreach (NuclearReactorProto.FuelData fuelPair in this.Prototype.FuelPairs)
      {
        Quantity capacity2 = 40.Quantity();
        NuclearReactor.RadioactiveProductBuffer radioactiveProductBuffer1 = new NuclearReactor.RadioactiveProductBuffer(capacity2, fuelPair.FuelInProto, this.m_radiationManager);
        this.m_fuelInBuffers.Add<ProductProto, ProductBuffer>(radioactiveProductBuffer1.Product, (ProductBuffer) radioactiveProductBuffer1);
        NuclearReactor.RadioactiveProductBuffer radioactiveProductBuffer2 = new NuclearReactor.RadioactiveProductBuffer(capacity2, fuelPair.SpentFuelOutProto, this.m_radiationManager);
        this.m_spentFuelBuffers.Add<ProductProto, ProductBuffer>(radioactiveProductBuffer2.Product, (ProductBuffer) radioactiveProductBuffer2);
      }
      if (proto.Enrichment.HasValue)
      {
        NuclearReactorProto.EnrichmentData enrichmentData = proto.Enrichment.Value;
        this.m_enrichmentInputBuffer = (Option<ProductBuffer>) new ProductBuffer(enrichmentData.BuffersCapacity, enrichmentData.InputProduct);
        this.m_enrichmentOutputBuffer = (Option<ProductBuffer>) new ProductBuffer(enrichmentData.BuffersCapacity, enrichmentData.OutputProduct);
      }
      this.reInitPorts();
      this.m_allowedFuel.AddRange(this.Prototype.FuelPairs);
      this.refreshRecipes();
      this.updateLogisticsInputReg();
      this.updateLogisticsOutputReg();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      this.refreshRecipes();
      if (saveVersion >= 129)
        return;
      foreach (NuclearReactorProto.FuelData fuelData in this.m_loadedFuel)
      {
        NuclearReactorProto.FuelData data = fuelData;
        this.m_fuelInBuffers.Where<KeyValuePair<ProductProto, ProductBuffer>>((Func<KeyValuePair<ProductProto, ProductBuffer>, bool>) (x => (Proto) x.Key == (Proto) data.FuelInProto)).Select<KeyValuePair<ProductProto, ProductBuffer>, ProductBuffer>((Func<KeyValuePair<ProductProto, ProductBuffer>, ProductBuffer>) (x => x.Value)).FirstOrDefault<ProductBuffer>()?.StoreAllIgnoreCapacity(Quantity.One);
        this.m_productsManager.ProductCreated(data.FuelInProto, Quantity.One, CreateReason.General);
      }
      this.m_loadedFuel.Clear();
    }

    private void reInitPorts()
    {
      this.m_coolantOutputPort = new IoPortData?(this.ConnectedOutputPorts.FirstOrDefault((Func<IoPortData, bool>) (x => (int) x.Name == (int) this.Prototype.CoolantOutPort)));
      this.m_steamOutputPorts = this.ConnectedOutputPorts.Filter((Predicate<IoPortData>) (x => this.Prototype.SteamOutPorts.Contains<char>(x.Name)));
      this.m_fuelOutputPort = this.ConnectedOutputPorts.FirstOrDefault((Func<IoPortData, bool>) (x => (int) x.Name == (int) this.Prototype.FuelOutPort));
      if (!this.Prototype.Enrichment.HasValue)
        return;
      this.m_enrichedOutputPort = this.ConnectedOutputPorts.FirstOrDefault((Func<IoPortData, bool>) (x => (int) x.Name == (int) this.Prototype.Enrichment.Value.OutPort));
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.reInitPorts();
    }

    public void SetTargetPowerLevel(Percent target)
    {
      if (target.IsNegative)
      {
        Log.Warning(string.Format("Setting invalid power level {0}.", (object) target));
        target = Percent.Zero;
      }
      if (target > this.MaxPowerLevelPercent)
      {
        Log.Warning(string.Format("Setting invalid power level {0}.", (object) target));
        target = this.MaxPowerLevelPercent;
      }
      if (this.IsInMeltdown)
        return;
      bool flag1 = target.IsPositive;
      if (flag1)
      {
        bool flag2;
        switch (this.ConstructionState)
        {
          case ConstructionState.InConstruction:
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
          case ConstructionState.PendingDeconstruction:
          case ConstructionState.InDeconstruction:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (flag1)
        return;
      this.TargetPowerLevel = target;
      this.OptimalHeatForCurrentPower = this.TargetPowerLevel.ApplyCeiled(NuclearReactor.POWER_LEVEL_HEAT_CAPACITY);
      this.m_highHeatForCurrentPower = this.OptimalHeatForCurrentPower + NuclearReactor.SELF_COOLING_HEAT_MARGIN;
      this.StartEmergencyCoolingAtHeat = (this.OptimalHeatForCurrentPower + this.MeltdownAtHeat) / 2;
      this.refreshRecipes();
    }

    public void SetAutomatedRegulation(bool regulateAutomatically)
    {
      if (this.IsAutomaticPowerRegulationEnabled == regulateAutomatically)
        return;
      if (!this.IsAutomaticPowerRegulationSupported)
      {
        Log.Warning("Automated regulation is not possible on this reactor.");
      }
      else
      {
        this.IsAutomaticPowerRegulationEnabled = regulateAutomatically;
        this.m_computingConsumer.OnComputingRequiredChanged();
      }
    }

    public bool IsAllowedFuel(ProductProto fuelProto)
    {
      return this.m_allowedFuel.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) fuelProto));
    }

    public bool TrySetAllowedFuel(ProductProto fuelProto, bool isAllowed)
    {
      if (this.Prototype.FuelPairs.Length <= 1)
        return false;
      if (!isAllowed)
      {
        if (!this.m_allowedFuel.RemoveFirst((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) fuelProto)))
          return false;
        ProductBuffer buffer;
        if (this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(fuelProto, out buffer))
          this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) buffer);
        this.refreshActiveRecipes();
        return true;
      }
      if (this.m_allowedFuel.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) fuelProto)))
        return true;
      ImmutableArray<NuclearReactorProto.FuelData> immutableArray = this.m_allowedFuel.ToImmutableArray();
      this.m_allowedFuel.Clear();
      bool flag = false;
      foreach (NuclearReactorProto.FuelData fuelPair in this.Prototype.FuelPairs)
      {
        if (immutableArray.Contains(fuelPair))
          this.m_allowedFuel.Add(fuelPair);
        else if ((Proto) fuelPair.FuelInProto == (Proto) fuelProto)
        {
          this.m_allowedFuel.Add(fuelPair);
          flag = true;
          ProductBuffer buffer;
          if (!this.IsLogisticsInputDisabled && this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(fuelProto, out buffer))
            this.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) this);
        }
      }
      this.refreshActiveRecipes();
      return flag;
    }

    private void recomputeValuesBasedOnProto()
    {
      Assert.That<Duration>(this.m_proto.ProcessDuration).IsPositive();
      int num = 100 * this.m_proto.ProcessDuration.Ticks;
      Assert.That<Quantity>(this.m_proto.SteamOutPerPowerLevel.Quantity).IsEqualTo(this.m_proto.WaterInPerPowerLevel.Quantity, "Reactor water input and steam output should match.");
      Assert.That<int>(num % this.m_proto.SteamOutPerPowerLevel.Quantity.Value).IsZero<Quantity, Percent>("Steam output {0} should divide {1}.", this.m_proto.SteamOutPerPowerLevel.Quantity, this.m_fuelRemainingLife);
      this.m_heatPerOutput = num / this.m_proto.SteamOutPerPowerLevel.Quantity.Value;
      this.m_maxHeatInExchanger = num * 2;
      this.m_automationIncAtHeatInExchanger = 5 * this.m_maxHeatInExchanger / 10;
      this.m_automationDecAtHeatInExchanger = 8 * this.m_maxHeatInExchanger / 10;
    }

    private void refreshRecipes()
    {
      Percent powerLevel = this.TargetPowerLevel.Max(Percent.Hundred);
      ImmutableArrayBuilder<IRecipeForUi> immutableArrayBuilder = new ImmutableArrayBuilder<IRecipeForUi>(this.Prototype.FuelPairs.Length);
      for (int index = 0; index < this.Prototype.FuelPairs.Length; ++index)
        immutableArrayBuilder[index] = (IRecipeForUi) new NuclearReactor.Recipe(this.Prototype, this.Prototype.FuelPairs[index], powerLevel);
      this.AllRecipes = immutableArrayBuilder.GetImmutableArrayAndClear();
      this.refreshActiveRecipes();
      if (!this.m_currentFuel.HasValue)
        return;
      this.m_currentFuelRecipeIndex = this.AllRecipes.IndexOf((Predicate<IRecipeForUi>) (x => x.Id == this.m_currentFuel.Value.FuelInProto.Id));
    }

    private void refreshActiveRecipes()
    {
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>(this.m_allowedFuel.Count);
      foreach (IRecipeForUi allRecipe in this.AllRecipes)
      {
        IRecipeForUi recipe = allRecipe;
        if (this.m_allowedFuel.Contains((Predicate<NuclearReactorProto.FuelData>) (x => x.FuelInProto.Id == recipe.Id)))
          lyst.Add(recipe);
      }
      this.ActiveRecipes = lyst.ToImmutableArray();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      NuclearReactor.State state = NuclearReactor.State.None;
      if (this.IsNotEnabled)
        state = this.Maintenance.Status.IsBroken ? NuclearReactor.State.Broken : NuclearReactor.State.Paused;
      else if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        state = NuclearReactor.State.NotEnoughWorkers;
      else
        this.trySendOutputs();
      Percent powerAndSetState = this.getTargetPowerAndSetState(ref state);
      this.CurrentState = state;
      if (this.CurrentPowerLevel != powerAndSetState)
        this.CurrentPowerLevel += (powerAndSetState - this.CurrentPowerLevel).Clamp(-NuclearReactor.MAX_POWER_DECREASE_PER_TICK, NuclearReactor.MAX_POWER_INCREASE_PER_TICK);
      Percent percent;
      if (this.IsInMeltdown)
      {
        Assert.That<int>(this.m_heatInExchanger).IsZero();
        int heatAmount = this.HeatAmount;
        percent = this.CurrentPowerLevel;
        int num = percent.Apply(100);
        this.HeatAmount = heatAmount + num;
        this.HeatAmount -= 100;
        if (this.HeatAmount <= 0)
        {
          percent = this.CurrentPowerLevel;
          if (percent.IsNotPositive)
          {
            this.HeatAmount = 0;
            this.m_heatInEmergencyCooler = 0;
            this.IsInMeltdown = false;
          }
        }
      }
      else
      {
        if (this.CurrentPowerLevel.IsPositive)
          this.generateHeatAndConsumeFuel();
        else if (this.HeatAmount > 0)
        {
          if (this.HeatAmount >= 20)
            this.HeatAmount -= 20;
          else
            this.HeatAmount = 0;
        }
        if (this.m_heatInExchanger < this.m_maxHeatInExchanger)
        {
          int num = 100 * this.HeatAmount / NuclearReactor.POWER_LEVEL_HEAT_CAPACITY;
          this.m_heatInExchanger += num;
          this.HeatAmount -= num;
        }
        if (this.m_heatInExchanger >= this.m_heatPerOutput && this.m_waterInBuffer.IsNotEmpty && this.m_steamOutBuffer.IsNotFull)
        {
          this.m_heatInExchanger -= this.m_heatPerOutput;
          this.m_waterInBuffer.RemoveExactly(Quantity.One);
          this.m_productsManager.ProductDestroyed(this.m_waterInBuffer.Product, Quantity.One, DestroyReason.General);
          this.m_steamOutBuffer.StoreExactly(Quantity.One);
          this.m_productsManager.ProductCreated(this.m_steamOutBuffer.Product, Quantity.One, CreateReason.Produced);
        }
      }
      if (this.HeatAmount > this.StartEmergencyCoolingAtHeat || this.IsInMeltdown)
      {
        if (this.m_heatInEmergencyCooler < this.m_maxHeatInExchanger)
        {
          percent = this.CurrentPowerLevel;
          percent = percent.Max(100.Percent());
          int num = this.HeatAmount.Min(percent.Apply(100));
          this.m_heatInEmergencyCooler += num;
          this.HeatAmount -= num;
        }
        int num1 = this.m_heatInEmergencyCooler / this.m_heatPerOutput;
        if (num1 >= 1)
        {
          Quantity quantity = num1.Quantity().Min(this.m_coolantInBuffer.Quantity).Min(this.m_coolantOutBuffer.UsableCapacity);
          if (quantity.IsPositive)
          {
            this.m_heatInEmergencyCooler -= quantity.Value * this.m_heatPerOutput;
            this.m_coolantInBuffer.RemoveExactly(quantity);
            this.m_productsManager.ProductDestroyed(this.m_coolantInBuffer.Product, quantity, DestroyReason.General);
            this.m_coolantOutBuffer.StoreExactly(quantity);
            this.m_productsManager.ProductCreated(this.m_coolantOutBuffer.Product, quantity, CreateReason.Produced);
          }
        }
      }
      else if (this.m_heatInEmergencyCooler > 0)
      {
        int num = this.m_heatInEmergencyCooler.Min(100);
        this.m_heatInEmergencyCooler -= num;
        percent = this.CurrentPowerLevel;
        if (percent.IsPositive)
          this.HeatAmount += num;
      }
      if (this.HeatAmount > this.MeltdownAtHeat)
      {
        if (this.IsInMeltdown)
        {
          this.m_meltdownEventRemaining -= Duration.OneTick;
          if (this.m_meltdownEventRemaining <= Duration.Zero)
          {
            this.m_meltdownEventRemaining = NuclearReactor.MELTDOWN_OVERHEAT_DAMAGE_INTERVAL;
            this.doMeltdownDamage();
          }
        }
        else
          this.startMeltdown();
        if (this.Prototype.LeakRadiationOnMeltdown)
          this.m_radiationManager.ReportReactorRadiationLeak();
      }
      this.m_isInMeltdownNotif.NotifyIff(this.IsInMeltdown, (IEntity) this);
      this.m_lacksMaintenanceNotif.NotifyIff(!this.IsInMeltdown && this.CurrentState == NuclearReactor.State.NotEnoughMaintenance, (IEntity) this);
    }

    /// <summary>
    /// Returns target power level based on reactor state. This also loads fuel if necessary.
    /// </summary>
    private Percent getTargetPowerAndSetState(ref NuclearReactor.State state)
    {
      if (this.IsInMeltdown)
      {
        state = NuclearReactor.State.Meltdown;
        return Percent.Zero;
      }
      if (state != NuclearReactor.State.None)
        return Percent.Zero;
      if (this.CurrentPowerLevel.IsZero && this.TargetPowerLevel.IsPositive && this.Maintenance.Status.CurrentBreakdownChance.IsPositive)
      {
        state = NuclearReactor.State.NotEnoughMaintenance;
        return Percent.Zero;
      }
      if (!this.m_currentFuel.HasValue)
      {
        if (this.m_allowedFuel.IsEmpty)
        {
          state = NuclearReactor.State.NoRecipes;
          return Percent.Zero;
        }
        if (this.getFuelQuantityInBuffers().Value < 16)
        {
          state = NuclearReactor.State.NotEnoughInput;
          return Percent.Zero;
        }
        bool flag = false;
        foreach (NuclearReactorProto.FuelData fuelData in this.m_allowedFuel)
        {
          ProductBuffer productBuffer;
          if (!this.m_spentFuelBuffers.TryGetValue<ProductProto, ProductBuffer>(fuelData.SpentFuelOutProto, out productBuffer))
            Log.Error(string.Format("No output buffer for '{0}'.", (object) fuelData.SpentFuelOutProto));
          else if (productBuffer.IsFull)
          {
            flag = true;
          }
          else
          {
            ProductBuffer buffer;
            if (!this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(fuelData.FuelInProto, out buffer))
              Log.Error(string.Format("No input buffer for '{0}'.", (object) fuelData.FuelInProto));
            else if (buffer.IsNotEmpty)
            {
              buffer.RemoveExactly(Quantity.One);
              this.m_productsManager.ProductDestroyed(buffer.Product, Quantity.One, DestroyReason.General);
              this.m_currentFuel = new NuclearReactorProto.FuelData?(fuelData);
              this.m_fuelRemainingLife = Percent.Hundred * this.m_currentFuel.Value.Duration.Ticks;
              this.m_currentFuelRecipeIndex = this.AllRecipes.IndexOf((Predicate<IRecipeForUi>) (x => x.Id == this.m_currentFuel.Value.FuelInProto.Id));
              break;
            }
          }
        }
        if (!this.m_currentFuel.HasValue)
        {
          state = flag ? NuclearReactor.State.OutputFull : NuclearReactor.State.NotEnoughInput;
          return Percent.Zero;
        }
      }
      if (this.TargetPowerLevel.IsNotPositive)
      {
        state = NuclearReactor.State.Idle;
        return Percent.Zero;
      }
      state = NuclearReactor.State.Working;
      Percent powerAndSetState = this.TargetPowerLevel;
      if (this.IsAutomaticPowerRegulationEnabled)
      {
        if (!this.m_computingConsumer.CanConsume())
        {
          powerAndSetState = this.CurrentPowerLevel;
          state = NuclearReactor.State.NotEnoughComputing;
        }
        else
        {
          if (this.m_heatInExchanger < this.m_automationIncAtHeatInExchanger)
            powerAndSetState = (this.CurrentPowerLevel + NuclearReactor.MAX_POWER_INCREASE_PER_TICK).Min(this.TargetPowerLevel);
          else if (this.m_heatInExchanger > this.m_automationDecAtHeatInExchanger)
            powerAndSetState = (this.CurrentPowerLevel - NuclearReactor.MAX_POWER_DECREASE_PER_TICK).Max(Percent.Hundred);
          this.m_computingConsumer.ConsumeAndAssert();
        }
      }
      return powerAndSetState;
    }

    private void generateHeatAndConsumeFuel()
    {
      int num = this.CurrentPowerLevel.Apply(100);
      if (this.HeatAmount > this.m_highHeatForCurrentPower)
        num -= 20;
      this.HeatAmount += num;
      if (!this.m_currentFuel.HasValue)
        return;
      this.m_fuelRemainingLife -= this.CurrentPowerLevel;
      if (!this.m_fuelRemainingLife.IsNotPositive)
        return;
      this.unloadSpentFuel(this.m_currentFuel.Value.SpentFuelOutProto);
      this.performEnrichmentIfCan();
      this.m_currentFuel = new NuclearReactorProto.FuelData?();
      this.m_fuelRemainingLife = Percent.Zero;
      this.m_currentFuelRecipeIndex = -1;
    }

    private void unloadSpentFuel(ProductProto fuelProto)
    {
      ProductBuffer productBuffer;
      if (!this.m_spentFuelBuffers.TryGetValue<ProductProto, ProductBuffer>(fuelProto, out productBuffer))
      {
        Log.Error(string.Format("No output buffer for '{0}'.", (object) fuelProto));
      }
      else
      {
        productBuffer.StoreAllIgnoreCapacity(Quantity.One);
        Action<Quantity> spentFuelGenerated = this.OnSpentFuelGenerated;
        if (spentFuelGenerated != null)
          spentFuelGenerated(Quantity.One);
        this.m_productsManager.ProductCreated(productBuffer.Product, Quantity.One, CreateReason.Produced);
      }
    }

    private void performEnrichmentIfCan()
    {
      if (this.m_enrichmentInputBuffer.IsNone || this.Prototype.Enrichment.IsNone || this.m_enrichmentOutputBufferPartial.IntegerPart.IsPositive)
        return;
      PartialQuantity processedPerLevel = this.Prototype.Enrichment.Value.ProcessedPerLevel;
      PartialQuantity partialQuantity = processedPerLevel - this.m_enrichmentInputBufferToPartial;
      partialQuantity = partialQuantity.Abs;
      Quantity quantityCeiled = partialQuantity.ToQuantityCeiled();
      if (!this.m_enrichmentInputBuffer.Value.CanRemove(quantityCeiled))
        return;
      Quantity quantity = this.m_enrichmentInputBuffer.Value.RemoveAsMuchAs(quantityCeiled);
      this.m_enrichmentInputBufferToPartial += quantity.AsPartial;
      this.m_productsManager.ProductDestroyed(this.m_enrichmentInputBuffer.Value.Product, quantity, DestroyReason.General);
      if (this.m_enrichmentInputBufferToPartial < processedPerLevel)
      {
        Log.Error("Unexpected state");
      }
      else
      {
        this.m_enrichmentInputBufferToPartial -= processedPerLevel;
        this.m_enrichmentOutputBufferPartial += processedPerLevel;
      }
    }

    private void sendEnrichedOutputIfCan()
    {
      if (this.m_enrichmentOutputBuffer.IsNone)
        return;
      ProductBuffer buffer = this.m_enrichmentOutputBuffer.Value;
      if (this.m_enrichmentOutputBufferPartial.IntegerPart.IsPositive)
      {
        Quantity quantity = buffer.StoreAsMuchAsReturnStored(this.m_enrichmentOutputBufferPartial.IntegerPart);
        if (quantity.IsPositive)
        {
          this.m_productsManager.ProductCreated(buffer.Product, quantity, CreateReason.Produced);
          this.m_enrichmentOutputBufferPartial -= quantity.AsPartial;
        }
      }
      if (!this.m_enrichedOutputPort.IsValid)
        return;
      this.m_enrichedOutputPort.SendAsMuchAsFromBuffer((IProductBuffer) buffer);
    }

    private void startMeltdown()
    {
      if (this.IsInMeltdown)
      {
        Log.Error("Meltdown is already started.");
      }
      else
      {
        this.IsInMeltdown = true;
        this.TargetPowerLevel = Percent.Zero;
        this.OptimalHeatForCurrentPower = 0;
        this.m_highHeatForCurrentPower = 0;
        this.StartEmergencyCoolingAtHeat = 0;
        if (this.m_currentFuel.HasValue)
        {
          if (!this.Prototype.DestroyFuelOnMeltdown)
            this.unloadSpentFuel(this.m_currentFuel.Value.SpentFuelOutProto);
          this.m_currentFuel = new NuclearReactorProto.FuelData?();
          this.m_fuelRemainingLife = Percent.Zero;
          this.m_currentFuelRecipeIndex = -1;
        }
        this.m_fuelRemainingLife = Percent.Zero;
        this.HeatAmount += this.m_heatInExchanger;
        this.m_heatInExchanger = 0;
        this.doMeltdownDamage();
        this.m_meltdownEventRemaining = NuclearReactor.MELTDOWN_OVERHEAT_DAMAGE_INTERVAL;
      }
    }

    private void doMeltdownDamage()
    {
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer1 in this.m_fuelInBuffers)
      {
        KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer = fuelInBuffer1;
        if (fuelInBuffer.Value.IsNotEmpty)
        {
          fuelInBuffer.Value.RemoveExactly(1.Quantity());
          this.m_productsManager.ProductDestroyed(fuelInBuffer.Key, 1.Quantity(), DestroyReason.General);
          if (!this.Prototype.DestroyFuelOnMeltdown)
          {
            ProductProto fuelProto = this.Prototype.FuelPairs.Where((Func<NuclearReactorProto.FuelData, bool>) (x => (Proto) x.FuelInProto == (Proto) fuelInBuffer.Key)).Select<NuclearReactorProto.FuelData, ProductProto>((Func<NuclearReactorProto.FuelData, ProductProto>) (x => x.SpentFuelOutProto)).FirstOrDefault<ProductProto>();
            if ((Proto) fuelProto != (Proto) null)
            {
              this.unloadSpentFuel(fuelProto);
              break;
            }
            break;
          }
          break;
        }
      }
      NuclearReactorProto.EnrichmentData valueOrNull1 = this.Prototype.Enrichment.ValueOrNull;
      if ((valueOrNull1 != null ? (valueOrNull1.DestroyContentOnMeltdown ? 1 : 0) : 0) != 0)
      {
        ProductBuffer valueOrNull2 = this.m_enrichmentInputBuffer.ValueOrNull;
        if (valueOrNull2 != null && valueOrNull2.IsNotEmpty)
        {
          valueOrNull2.RemoveExactly(1.Quantity());
          this.m_productsManager.ProductDestroyed(valueOrNull2.Product, Quantity.One, DestroyReason.Cleared);
        }
        ProductBuffer valueOrNull3 = this.m_enrichmentOutputBuffer.ValueOrNull;
        if (valueOrNull3 != null && valueOrNull3.IsNotEmpty)
        {
          valueOrNull3.RemoveExactly(1.Quantity());
          this.m_productsManager.ProductDestroyed(valueOrNull3.Product, Quantity.One, DestroyReason.Cleared);
        }
        this.m_enrichmentInputBufferToPartial = PartialQuantity.Zero;
        this.m_enrichmentOutputBufferPartial = PartialQuantity.Zero;
      }
      this.Maintenance.DecreaseBy(NuclearReactor.DECREASE_MAINTENANCE_MELTDOWN_DAMAGE);
    }

    private void trySendOutputs()
    {
      if (this.m_coolantOutBuffer.IsNotEmpty)
      {
        ref IoPortData? local = ref this.m_coolantOutputPort;
        if (local.HasValue)
          local.GetValueOrDefault().SendAsMuchAsFromBuffer((IProductBuffer) this.m_coolantOutBuffer);
      }
      if (this.m_steamOutBuffer.IsNotEmpty)
      {
        for (int index = 0; index < this.m_steamOutputPorts.Length; ++index)
        {
          this.m_nextOutPortIndex = (this.m_nextOutPortIndex + 1) % this.m_steamOutputPorts.Length;
          this.m_steamOutputPorts[this.m_nextOutPortIndex].SendAsMuchAsFromBuffer((IProductBuffer) this.m_steamOutBuffer);
          if (this.m_steamOutBuffer.IsEmpty)
            break;
        }
      }
      foreach (KeyValuePair<ProductProto, ProductBuffer> spentFuelBuffer in this.m_spentFuelBuffers)
      {
        if (!spentFuelBuffer.Value.IsEmpty && this.m_fuelOutputPort.IsValid)
          this.m_fuelOutputPort.SendAsMuchAsFromBuffer((IProductBuffer) spentFuelBuffer.Value);
      }
      this.sendEnrichedOutputIfCan();
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (!this.IsEnabled)
        return pq.Quantity;
      if ((int) sourcePort.Name == (int) this.Prototype.CoolantInPort)
        return this.m_coolantInBuffer.StoreAsMuchAs(pq);
      if (this.Prototype.WaterInPorts.Contains<char>(sourcePort.Name))
        return this.m_waterInBuffer.StoreAsMuchAs(pq);
      if ((int) sourcePort.Name == (int) this.Prototype.FuelInPort)
      {
        ProductBuffer productBuffer;
        return this.IsInMeltdown || !this.m_allowedFuel.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) pq.Product)) || !this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(pq.Product, out productBuffer) ? pq.Quantity : productBuffer.StoreAsMuchAs(pq);
      }
      if (!this.Prototype.Enrichment.HasValue || (int) sourcePort.Name != (int) this.Prototype.Enrichment.Value.InPort || this.IsInMeltdown)
        return pq.Quantity;
      ProductBuffer valueOrNull = this.m_enrichmentInputBuffer.ValueOrNull;
      return valueOrNull == null ? pq.Quantity : valueOrNull.StoreAsMuchAs(pq);
    }

    protected override void OnDestroy()
    {
      this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_waterInBuffer);
      this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_steamOutBuffer);
      this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_coolantInBuffer);
      this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_coolantOutBuffer);
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
        this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) fuelInBuffer.Value);
      foreach (KeyValuePair<ProductProto, ProductBuffer> spentFuelBuffer in this.m_spentFuelBuffers)
        this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) spentFuelBuffer.Value);
      this.m_enrichmentInputBufferToPartial = PartialQuantity.Zero;
      this.m_enrichmentOutputBufferPartial = PartialQuantity.Zero;
      if (this.m_enrichmentInputBuffer.HasValue)
        this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_enrichmentInputBuffer.Value);
      if (this.m_enrichmentOutputBuffer.HasValue)
        this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_enrichmentOutputBuffer.Value);
      base.OnDestroy();
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return !this.CurrentPowerLevel.IsPositive ? EntityValidationResult.Success : EntityValidationResult.CreateError("TODO: Reactor is not off.");
    }

    public override void StartDeconstructionIfCan()
    {
      this.SetTargetPowerLevel(Percent.Zero);
      base.StartDeconstructionIfCan();
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      if (this.CurrentPowerLevel.IsPositive || this.TargetPowerLevel.IsPositive || this.HeatAmount > 0)
      {
        errorMessage = (LocStrFormatted) TrCore.NuclearReactor__DisableBeforeUpgrade;
        return false;
      }
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      this.Prototype = this.Prototype.Upgrade.NextTier.Value;
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
      {
        KeyValuePair<ProductProto, ProductBuffer> kvp = fuelInBuffer;
        if (!this.Prototype.FuelPairs.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) kvp.Key)))
        {
          this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) kvp.Value);
          this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) kvp.Value);
        }
      }
      foreach (KeyValuePair<ProductProto, ProductBuffer> spentFuelBuffer in this.m_spentFuelBuffers)
      {
        KeyValuePair<ProductProto, ProductBuffer> kvp = spentFuelBuffer;
        if (!this.Prototype.FuelPairs.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.SpentFuelOutProto == (Proto) kvp.Key)))
        {
          this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) kvp.Value);
          this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) kvp.Value);
        }
      }
      ImmutableArray<KeyValuePair<ProductProto, ProductBuffer>> immutableArray1 = this.m_fuelInBuffers.ToImmutableArray();
      this.m_fuelInBuffers.Clear();
      ImmutableArray<KeyValuePair<ProductProto, ProductBuffer>> immutableArray2 = this.m_spentFuelBuffers.ToImmutableArray();
      this.m_spentFuelBuffers.Clear();
      foreach (NuclearReactorProto.FuelData fuelPair in this.Prototype.FuelPairs)
      {
        Quantity capacity = 40.Quantity();
        ProductBuffer productBuffer1;
        if (!immutableArray1.TryGetValue<ProductProto, ProductBuffer>(fuelPair.FuelInProto, out productBuffer1))
          productBuffer1 = (ProductBuffer) new NuclearReactor.RadioactiveProductBuffer(capacity, fuelPair.FuelInProto, this.m_radiationManager);
        this.m_fuelInBuffers.Add<ProductProto, ProductBuffer>(productBuffer1.Product, productBuffer1);
        ProductBuffer productBuffer2;
        if (!immutableArray2.TryGetValue<ProductProto, ProductBuffer>(fuelPair.SpentFuelOutProto, out productBuffer2))
          productBuffer2 = (ProductBuffer) new NuclearReactor.RadioactiveProductBuffer(capacity, fuelPair.SpentFuelOutProto, this.m_radiationManager);
        this.m_spentFuelBuffers.Add<ProductProto, ProductBuffer>(productBuffer2.Product, productBuffer2);
      }
      this.m_allowedFuel.Clear();
      this.m_allowedFuel.AddRange(this.Prototype.FuelPairs);
      this.refreshRecipes();
    }

    bool IRecipeExecutorForUi.HasClearProductsActionFor(IRecipeForUi recipe) => false;

    Percent IRecipeExecutorForUi.ProgressOnRecipe(IRecipeForUi recipe)
    {
      return !this.m_currentFuel.HasValue || this.AllRecipes.IndexOf(recipe) != this.m_currentFuelRecipeIndex ? Percent.Zero : Percent.Hundred - Percent.FromRatio(this.m_fuelRemainingLife.IntegerPart, this.m_currentFuel.Value.Duration.Ticks);
    }

    Duration IRecipeExecutorForUi.GetTargetDurationFor(IRecipeForUi recipe) => recipe.Duration;

    Quantity IRecipeExecutorForUi.GetInputQuantityFor(ProductProto product)
    {
      if ((Proto) product == (Proto) this.m_waterInBuffer.Product)
        return this.m_waterInBuffer.Quantity;
      ProductBuffer productBuffer;
      return !this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(product, out productBuffer) ? Quantity.Zero : productBuffer.Quantity;
    }

    Quantity IRecipeExecutorForUi.GetOutputQuantityFor(ProductProto product)
    {
      if ((Proto) product == (Proto) this.m_steamOutBuffer.Product)
        return this.m_steamOutBuffer.Quantity;
      ProductBuffer productBuffer;
      return !this.m_spentFuelBuffers.TryGetValue<ProductProto, ProductBuffer>(product, out productBuffer) ? Quantity.Zero : productBuffer.Quantity;
    }

    Quantity IRecipeExecutorForUi.GetInputCapacityFor(ProductProto product)
    {
      if ((Proto) product == (Proto) this.m_waterInBuffer.Product)
        return this.m_waterInBuffer.Capacity;
      ProductBuffer productBuffer;
      return !this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(product, out productBuffer) ? Quantity.Zero : productBuffer.Capacity;
    }

    Quantity IRecipeExecutorForUi.GetOutputCapacityFor(ProductProto product)
    {
      if ((Proto) product == (Proto) this.m_steamOutBuffer.Product)
        return this.m_steamOutBuffer.Capacity;
      ProductBuffer productBuffer;
      return !this.m_spentFuelBuffers.TryGetValue<ProductProto, ProductBuffer>(product, out productBuffer) ? Quantity.Zero : productBuffer.Capacity;
    }

    public void SetLogisticsInputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsInputDisabled == isDisabled)
        return;
      this.IsLogisticsInputDisabled = isDisabled;
      this.updateLogisticsInputReg();
    }

    public void SetLogisticsOutputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsOutputDisabled == isDisabled)
        return;
      this.IsLogisticsOutputDisabled = isDisabled;
      this.updateLogisticsOutputReg();
    }

    private void updateLogisticsInputReg()
    {
      if (this.IsLogisticsInputDisabled)
      {
        foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
          this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) fuelInBuffer.Value);
      }
      else
      {
        foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
          this.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this, (IProductBuffer) fuelInBuffer.Value, (IInputBufferPriorityProvider) this);
      }
    }

    private void updateLogisticsOutputReg()
    {
      if (this.IsLogisticsInputDisabled)
      {
        foreach (KeyValuePair<ProductProto, ProductBuffer> spentFuelBuffer in this.m_spentFuelBuffers)
          this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) spentFuelBuffer.Value);
      }
      else
      {
        foreach (KeyValuePair<ProductProto, ProductBuffer> spentFuelBuffer in this.m_spentFuelBuffers)
          this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this, (IProductBuffer) spentFuelBuffer.Value, (IOutputBufferPriorityProvider) this);
      }
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      ProductBuffer productBuffer;
      return this.m_spentFuelBuffers.TryGetValue<ProductProto, ProductBuffer>(request.Buffer.Product, out productBuffer) ? new BufferStrategy(this.GeneralPriority, new Quantity?(productBuffer.Capacity / 2)) : BufferStrategy.Ignore;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      ProductBuffer productBuffer;
      return this.m_fuelInBuffers.TryGetValue<ProductProto, ProductBuffer>(buffer.Product, out productBuffer) ? new BufferStrategy(this.GeneralPriority, new Quantity?(productBuffer.Capacity / 2)) : BufferStrategy.Ignore;
    }

    /// <summary>Returns info only on fuel that is currently allowed.</summary>
    private Quantity getFuelQuantityInBuffers()
    {
      Quantity zero = Quantity.Zero;
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
        zero += fuelInBuffer.Value.Quantity;
      return zero;
    }

    public void GetFuelQuantities(Lyst<ProductQuantity> resultToFill)
    {
      resultToFill.Clear();
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
      {
        KeyValuePair<ProductProto, ProductBuffer> pair = fuelInBuffer;
        if (pair.Value.IsNotEmpty || this.m_allowedFuel.Any<NuclearReactorProto.FuelData>((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) pair.Key)))
          resultToFill.Add(pair.Key.WithQuantity(pair.Value.Quantity));
      }
    }

    /// <summary>Returns info only on fuel that is currently allowed.</summary>
    public Quantity GetFuelCapacityInBuffers()
    {
      Quantity zero = Quantity.Zero;
      foreach (KeyValuePair<ProductProto, ProductBuffer> fuelInBuffer in this.m_fuelInBuffers)
      {
        KeyValuePair<ProductProto, ProductBuffer> pair = fuelInBuffer;
        if (this.m_allowedFuel.Contains((Predicate<NuclearReactorProto.FuelData>) (x => (Proto) x.FuelInProto == (Proto) pair.Key)))
          zero += pair.Value.Capacity;
        else if (pair.Value.Quantity.IsPositive)
          zero += pair.Value.Capacity;
      }
      return zero;
    }

    public static void Serialize(NuclearReactor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NuclearReactor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NuclearReactor.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.CurrentPowerLevel, writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteInt(this.HeatAmount);
      writer.WriteBool(this.IsAutomaticPowerRegulationEnabled);
      writer.WriteBool(this.IsInMeltdown);
      writer.WriteBool(this.IsLogisticsInputDisabled);
      writer.WriteBool(this.IsLogisticsOutputDisabled);
      Lyst<NuclearReactorProto.FuelData>.Serialize(this.m_allowedFuel, writer);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactionManager);
      writer.WriteInt(this.m_automationDecAtHeatInExchanger);
      writer.WriteInt(this.m_automationIncAtHeatInExchanger);
      writer.WriteGeneric<IComputingConsumer>(this.m_computingConsumer);
      ProductBuffer.Serialize(this.m_coolantInBuffer, writer);
      ProductBuffer.Serialize(this.m_coolantOutBuffer, writer);
      writer.WriteNullableStruct<NuclearReactorProto.FuelData>(this.m_currentFuel);
      writer.WriteInt(this.m_currentFuelRecipeIndex);
      Option<ProductBuffer>.Serialize(this.m_enrichmentInputBuffer, writer);
      PartialQuantity.Serialize(this.m_enrichmentInputBufferToPartial, writer);
      Option<ProductBuffer>.Serialize(this.m_enrichmentOutputBuffer, writer);
      PartialQuantity.Serialize(this.m_enrichmentOutputBufferPartial, writer);
      Lyst<KeyValuePair<ProductProto, ProductBuffer>>.Serialize(this.m_fuelInBuffers, writer);
      Percent.Serialize(this.m_fuelRemainingLife, writer);
      writer.WriteInt(this.m_heatInEmergencyCooler);
      writer.WriteInt(this.m_heatInExchanger);
      writer.WriteInt(this.m_heatPerOutput);
      writer.WriteInt(this.m_highHeatForCurrentPower);
      EntityNotificator.Serialize(this.m_isInMeltdownNotif, writer);
      EntityNotificator.Serialize(this.m_lacksMaintenanceNotif, writer);
      writer.WriteInt(this.m_maxHeatInExchanger);
      Duration.Serialize(this.m_meltdownEventRemaining, writer);
      writer.WriteInt(this.m_nextOutPortIndex);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<NuclearReactorProto>(this.m_proto);
      writer.WriteGeneric<IRadiationManager>(this.m_radiationManager);
      Lyst<KeyValuePair<ProductProto, ProductBuffer>>.Serialize(this.m_spentFuelBuffers, writer);
      ProductBuffer.Serialize(this.m_steamOutBuffer, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      ProductBuffer.Serialize(this.m_waterInBuffer, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteInt(this.OptimalHeatForCurrentPower);
      writer.WriteInt(this.StartEmergencyCoolingAtHeat);
      Percent.Serialize(this.TargetPowerLevel, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static NuclearReactor Deserialize(BlobReader reader)
    {
      NuclearReactor nuclearReactor;
      if (reader.TryStartClassDeserialization<NuclearReactor>(out nuclearReactor))
        reader.EnqueueDataDeserialization((object) nuclearReactor, NuclearReactor.s_deserializeDataDelayedAction);
      return nuclearReactor;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentPowerLevel = Percent.Deserialize(reader);
      this.CurrentState = (NuclearReactor.State) reader.ReadInt();
      this.HeatAmount = reader.ReadInt();
      this.IsAutomaticPowerRegulationEnabled = reader.ReadBool();
      this.IsInMeltdown = reader.ReadBool();
      this.IsLogisticsInputDisabled = reader.ReadBool();
      this.IsLogisticsOutputDisabled = reader.ReadBool();
      reader.SetField<NuclearReactor>(this, "m_allowedFuel", (object) Lyst<NuclearReactorProto.FuelData>.Deserialize(reader));
      reader.SetField<NuclearReactor>(this, "m_assetTransactionManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      this.m_automationDecAtHeatInExchanger = reader.ReadInt();
      this.m_automationIncAtHeatInExchanger = reader.ReadInt();
      reader.SetField<NuclearReactor>(this, "m_computingConsumer", (object) reader.ReadGenericAs<IComputingConsumer>());
      reader.SetField<NuclearReactor>(this, "m_coolantInBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<NuclearReactor>(this, "m_coolantOutBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_currentFuel = reader.ReadNullableStruct<NuclearReactorProto.FuelData>();
      this.m_currentFuelRecipeIndex = reader.ReadInt();
      reader.SetField<NuclearReactor>(this, "m_enrichmentInputBuffer", (object) Option<ProductBuffer>.Deserialize(reader));
      this.m_enrichmentInputBufferToPartial = PartialQuantity.Deserialize(reader);
      reader.SetField<NuclearReactor>(this, "m_enrichmentOutputBuffer", (object) Option<ProductBuffer>.Deserialize(reader));
      this.m_enrichmentOutputBufferPartial = PartialQuantity.Deserialize(reader);
      reader.SetField<NuclearReactor>(this, "m_fuelInBuffers", (object) Lyst<KeyValuePair<ProductProto, ProductBuffer>>.Deserialize(reader));
      this.m_fuelRemainingLife = Percent.Deserialize(reader);
      this.m_heatInEmergencyCooler = reader.ReadInt();
      this.m_heatInExchanger = reader.ReadInt();
      this.m_heatPerOutput = reader.ReadInt();
      this.m_highHeatForCurrentPower = reader.ReadInt();
      this.m_isInMeltdownNotif = EntityNotificator.Deserialize(reader);
      this.m_lacksMaintenanceNotif = EntityNotificator.Deserialize(reader);
      if (reader.LoadedSaveVersion < 129)
        reader.SetField<NuclearReactor>(this, "m_loadedFuel", (object) Queueue<NuclearReactorProto.FuelData>.Deserialize(reader));
      this.m_maxHeatInExchanger = reader.ReadInt();
      this.m_meltdownEventRemaining = Duration.Deserialize(reader);
      this.m_nextOutPortIndex = reader.ReadInt();
      reader.SetField<NuclearReactor>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<NuclearReactorProto>();
      reader.SetField<NuclearReactor>(this, "m_radiationManager", (object) reader.ReadGenericAs<IRadiationManager>());
      reader.SetField<NuclearReactor>(this, "m_spentFuelBuffers", (object) Lyst<KeyValuePair<ProductProto, ProductBuffer>>.Deserialize(reader));
      reader.SetField<NuclearReactor>(this, "m_steamOutBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<NuclearReactor>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<NuclearReactor>(this, "m_waterInBuffer", (object) ProductBuffer.Deserialize(reader));
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.OptimalHeatForCurrentPower = reader.ReadInt();
      this.StartEmergencyCoolingAtHeat = reader.ReadInt();
      this.TargetPowerLevel = Percent.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<NuclearReactor>(this, "initSelf", InitPriority.Normal);
    }

    static NuclearReactor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NuclearReactor.POWER_LEVEL_HEAT_CAPACITY_DURATION = 20.Seconds();
      NuclearReactor.MAX_POWER_INCREASE_PER_TICK = 2.Percent();
      NuclearReactor.MAX_POWER_DECREASE_PER_TICK = 1.Percent();
      NuclearReactor.MELTDOWN_OVERHEAT_DAMAGE_INTERVAL = 3.Seconds();
      NuclearReactor.DECREASE_MAINTENANCE_MELTDOWN_DAMAGE = 3.Percent();
      NuclearReactor.POWER_LEVEL_HEAT_CAPACITY = NuclearReactor.POWER_LEVEL_HEAT_CAPACITY_DURATION.Ticks * 100;
      NuclearReactor.SELF_COOLING_HEAT_MARGIN = NuclearReactor.POWER_LEVEL_HEAT_CAPACITY / 5;
      NuclearReactor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      NuclearReactor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public class Recipe : IRecipeForUi
    {
      public Proto.ID Id { get; }

      public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

      public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

      public Duration Duration { get; }

      public Recipe(
        NuclearReactorProto proto,
        NuclearReactorProto.FuelData fuelData,
        Percent powerLevel)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Id = (Proto.ID) fuelData.FuelInProto.Id;
        Quantity quantity1 = fuelData.Duration.Ticks * proto.WaterInPerPowerLevel.Quantity / proto.ProcessDuration.Ticks;
        Quantity quantity2 = fuelData.Duration.Ticks * proto.SteamOutPerPowerLevel.Quantity / proto.ProcessDuration.Ticks;
        this.AllUserVisibleInputs = ImmutableArray.Create<RecipeInput>(new RecipeInput(proto.WaterInPerPowerLevel.Product, quantity1), new RecipeInput(fuelData.FuelInProto, Quantity.One));
        this.AllUserVisibleOutputs = ImmutableArray.Create<RecipeOutput>(new RecipeOutput(proto.SteamOutPerPowerLevel.Product, quantity2), new RecipeOutput(fuelData.SpentFuelOutProto, Quantity.One));
        this.Duration = Duration.FromTicks(powerLevel.ApplyInverse((Fix32) fuelData.Duration.Ticks).ToIntRounded());
      }
    }

    public enum State
    {
      None,
      /// <summary>Broken due to maintenance</summary>
      Broken,
      Paused,
      /// <summary>Reactor is overheated and is cooling down.</summary>
      Meltdown,
      NotEnoughWorkers,
      NotEnoughComputing,
      /// <summary>Reactor cannot start due to maintenance.</summary>
      NotEnoughMaintenance,
      /// <summary>Not enough fuel in reactor to start working.</summary>
      NotEnoughInput,
      OutputFull,
      NoRecipes,
      Idle,
      Working,
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class RadioactiveProductBuffer : ProductBuffer
    {
      private readonly IRadiationManager m_radiationManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public RadioactiveProductBuffer(
        Quantity capacity,
        ProductProto product,
        IRadiationManager radiationManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_radiationManager = radiationManager;
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        this.m_radiationManager.ReportSafelyStoredQuantityChange(this.Product, diff);
      }

      public static void Serialize(NuclearReactor.RadioactiveProductBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<NuclearReactor.RadioactiveProductBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, NuclearReactor.RadioactiveProductBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteGeneric<IRadiationManager>(this.m_radiationManager);
      }

      public static NuclearReactor.RadioactiveProductBuffer Deserialize(BlobReader reader)
      {
        NuclearReactor.RadioactiveProductBuffer radioactiveProductBuffer;
        if (reader.TryStartClassDeserialization<NuclearReactor.RadioactiveProductBuffer>(out radioactiveProductBuffer))
          reader.EnqueueDataDeserialization((object) radioactiveProductBuffer, NuclearReactor.RadioactiveProductBuffer.s_deserializeDataDelayedAction);
        return radioactiveProductBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<NuclearReactor.RadioactiveProductBuffer>(this, "m_radiationManager", (object) reader.ReadGenericAs<IRadiationManager>());
      }

      static RadioactiveProductBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        NuclearReactor.RadioactiveProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        NuclearReactor.RadioactiveProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
