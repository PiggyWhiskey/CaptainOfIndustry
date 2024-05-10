// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.Machine
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
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  public class Machine : 
    LayoutEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IAnimatedEntity,
    IUnityConsumingEntity,
    IEntityWithLogisticsControl,
    IElectricityConsumingEntity,
    IComputingConsumingEntity,
    IEntityWithSimUpdate,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithBoost,
    IEntityWithSound,
    IEntityWithCloneableConfig,
    IEntityWithEmission,
    IUpgradableEntity,
    IRecipeExecutorForUi,
    IEntityWithWorkers
  {
    private static readonly ThreadLocal<Machine.CacheContext> s_cache;
    private MachineProto m_proto;
    private Option<IElectricityConsumer> m_electricityConsumer;
    private Option<IComputingConsumer> m_computingConsumer;
    /// <summary>Utilization from the last tick where we worked.</summary>
    private Percent m_lastWorkUtilization;
    private readonly Lyst<RecipeProto> m_recipesAssigned;
    [DoNotSave(0, null)]
    private int m_assignedRecipesCount;
    /// <summary>
    /// Provides great perf benefit. Instead of iterating over regular recipes we
    /// use this list of structs that also point directly to the corresponding
    /// buffers. Thanks to this we skip lots of dictionary calls and hops on RecipeProto.
    /// </summary>
    [DoNotSaveCreateNewOnLoad(null, 0)]
    protected LystStruct<Machine.RecipeWrapper> m_recipesFast;
    /// <summary>
    /// Recipe result that is being worked on. No other recipe can be started until this one is empty.
    /// </summary>
    internal Machine.RecipeResult m_recipeResult;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly IProductsManager m_productsManager;
    private readonly VirtualBuffersMap m_virtualBuffersMap;
    private EntityNotificator m_noRecipeSelectedNotif;
    private EntityNotificator m_entityBoostedNotif;
    private EntityNotificatorWithProtoParam m_needsTransportNotif;
    [NewInSaveVersion(140, null, null, null, null)]
    private Duration m_lowPowerChargeLeft;
    [NewInSaveVersion(140, null, null, null, null)]
    private Duration m_lowComputingChargeLeft;
    [NewInSaveVersion(140, null, "Percent.Hundred", null, null)]
    private Percent m_speedFactorBase;
    [NewInSaveVersion(140, null, "Percent.Hundred", null, null)]
    private Percent m_speedFactorSecondary;
    [DoNotSave(0, null)]
    private Percent m_speedOnLowPower;
    [DoNotSave(0, null)]
    private Percent m_speedOnLowComputing;
    private static readonly Percent SPEED_WHEN_BROKEN;
    private static readonly Duration MaxDurationWithNoPower;
    private static readonly Duration MaxDurationWithNoComputing;
    [DoNotSave(0, null)]
    private int m_inputPortsCount;
    [DoNotSave(0, null)]
    private int m_outputPortsCount;
    /// <summary>
    /// If we fail to start work on new recipe due to lack of input / output we set this to true.
    /// Thanks to this, in the next sim we can skip the expensive checking for recipes. Every-time a buffer
    /// receives more quantity (an input one) or sends away some quantity (an output one) it sets this
    /// back to false which means we try recipes again. This is also invalidated on recipes change.
    /// We do not need to save this and in fact we would have to invalidate it anyway because it can
    /// happen that some update lowers a recipe input requirements.
    /// </summary>
    [DoNotSave(0, null)]
    private bool m_hasNoNeedToSearchForRecipe;
    /// <summary>
    /// The last status we had when we set m_hasNoNeedToSearchForRecipe to true. This is either full output
    /// or not enough input.
    /// </summary>
    [DoNotSave(0, null)]
    private Machine.State m_statusWhenNoNeedForRecipe;
    /// <summary>
    /// This gets set to true if all output buffers were empty during last send attempt. This saves some perf
    /// on having to hop on each buffer and checking that there is nothing to save anyway. This gets invalidated
    /// anytime we store more products into output buffers or when a new port gets connected. We also don't
    /// save this as there is no advantage in doing so (we just try to send products once after we load).
    /// </summary>
    [DoNotSave(0, null)]
    private bool m_allConnectedOutputsBuffersEmpty;
    private readonly Option<IInputBufferPriorityProvider> m_customStrategy;
    private Option<ProductProto> m_productInNeedOfTransport;
    private readonly IProperty<bool> m_forceRunEnabled;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<RecipeProto> m_recipesWithFullOutput;
    private LystStruct<Machine.MachineInputBuffer> m_inputBuffers;
    private LystStruct<Machine.MachineOutputBuffer> m_outputBuffers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private static Machine.CacheContext Cache => Machine.s_cache.Value;

    [DoNotSave(0, null)]
    public MachineProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public IUpgrader Upgrader { get; private set; }

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.MachineSoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.MachineSoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    bool IEntityWithSound.IsSoundOn => this.WorkedThisTick;

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionWhenRunning.HasValue ? new float?() : new float?(this.WorkedThisTick ? (float) this.Prototype.EmissionWhenRunning.Value : 0.0f);
      }
    }

    public Upoints MaxMonthlyUnityConsumed => this.MonthlyUnityConsumed;

    public Upoints MonthlyUnityConsumed
    {
      get
      {
        return !this.IsBoostRequested || !this.BoostCost.HasValue ? Upoints.Zero : this.BoostCost.Value;
      }
    }

    public Proto.ID UpointsCategoryId => IdsCore.UpointsCategories.Boost;

    public override bool IsCargoAffectedByGeneralPriority => true;

    public Machine.State CurrentState { get; private set; }

    public bool CanDisableLogisticsInput => this.m_inputPortsCount > 0;

    public bool CanDisableLogisticsOutput => this.m_outputPortsCount > 0;

    public EntityLogisticsMode LogisticsInputMode { get; private set; }

    public EntityLogisticsMode LogisticsOutputMode { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Electricity PowerRequired => this.Prototype.ConsumedPowerPerTick;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.As<IElectricityConsumerReadonly>();
    }

    Computing IComputingConsumingEntity.ComputingRequired => this.Prototype.ComputingConsumed;

    public Option<IComputingConsumerReadonly> ComputingConsumer
    {
      get => this.m_computingConsumer.As<IComputingConsumerReadonly>();
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => this.CurrentState != Machine.State.Working;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    /// <summary>
    /// Whether the player request boost. That doesn't mean that the boost is active if there isn't enough unity.
    /// </summary>
    public bool IsBoostRequested { get; private set; }

    public bool IsBoosted { get; private set; }

    public Upoints? BoostCost => this.Prototype.BoostCost;

    [RenamedInVersion(140, "BoostedUnityConsumer")]
    public Option<Mafi.Core.Population.UnityConsumer> UnityConsumer { get; private set; }

    /// <summary>
    /// Last recipe this entity has worked on (this might be old value if the entity didn't work for a longer time).
    /// </summary>
    public Option<RecipeProto> LastRecipeInProgress { get; private set; }

    /// <summary>
    /// This allows to return progress for a super-fast recipes that take 1 tick. The reason is that fast recipes
    /// have m_recipeResult always None. Which would when calculating progress return 0. It is also correct because
    /// the entity really worked that tick (so even for slow entity it is correct. Another thing is that even machine
    /// has progress it doesn't mean that it actually runs. For instance if output ports are full, machine stops but
    /// progress is still 100%. For animations this is how to check that machine actually stopped to work.
    /// </summary>
    public bool WorkedThisTick => this.CurrentState == Machine.State.Working;

    /// <summary>Progress on the current recipe.</summary>
    /// <remarks>
    /// If recipe takes 1 tick and is repeated all the time, you will get 1.0f from this method every time you ask.
    /// </remarks>
    public Percent ProgressPerc
    {
      get
      {
        if (this.m_recipeResult.HasResult)
          return Percent.FromRatio(this.m_recipeResult.DurationDone.Ticks, this.m_recipeResult.Duration.Ticks);
        return !this.WorkedThisTick ? Percent.Zero : Percent.Hundred;
      }
    }

    /// <summary>Number of performed ticks on current recipe so far.</summary>
    public Duration RecipeProductionTicks
    {
      get => !this.m_recipeResult.HasResult ? Duration.Zero : this.m_recipeResult.DurationDone;
    }

    public Percent Utilization => !this.WorkedThisTick ? Percent.Zero : this.m_lastWorkUtilization;

    public IIndexable<RecipeProto> RecipesAssigned
    {
      get => (IIndexable<RecipeProto>) this.m_recipesAssigned;
    }

    /// <summary>
    /// Note: speed factor does not take boost into the account.
    /// </summary>
    [NewInSaveVersion(140, null, "Percent.Hundred", null, null)]
    public Percent SpeedFactor { get; private set; }

    public Percent DurationMultiplier => Percent.Hundred / this.SpeedFactor;

    /// <summary>
    /// Introduced for maintenance depot which outputs its products into a global limited virtual buffer.
    /// The machine optimization is not able to handle that case and once it would reach full output buffer
    /// it would never try to send products again.
    /// </summary>
    protected virtual bool UsesVirtualLimitedBuffers => false;

    public Machine(
      EntityId id,
      MachineProto proto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap virtualBuffersMap,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IUnityConsumerFactory unityConsumerFactory,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_recipesAssigned = new Lyst<RecipeProto>();
      this.m_recipeResult = new Machine.RecipeResult();
      // ISSUE: reference to a compiler-generated field
      this.\u003CSpeedFactor\u003Ek__BackingField = Percent.Hundred;
      this.m_speedFactorBase = Percent.Hundred;
      this.m_speedFactorSecondary = Percent.Hundred;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto.CheckNotNull<MachineProto>();
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_virtualBuffersMap = virtualBuffersMap;
      this.m_productsManager = this.Context.ProductsManager;
      this.updateProperties();
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumerIfNeeded((IElectricityConsumingEntity) this, this.m_electricityConsumer);
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumerIfNeeded((IComputingConsumingEntity) this, this.m_computingConsumer);
      this.Upgrader = upgraderFactory.CreateInstance<MachineProto, Machine>(this, proto);
      this.m_customStrategy = (Option<IInputBufferPriorityProvider>) (proto.IsWasteDisposal ? (IInputBufferPriorityProvider) Machine.WasteInputPortPriorityProvider.Instance : (IInputBufferPriorityProvider) null);
      this.m_forceRunEnabled = this.Context.PropertiesDb.GetProperty<bool>(IdsCore.PropertyIds.ForceRunAllMachinesEnabled);
      this.m_noRecipeSelectedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoRecipeSelected);
      this.m_entityBoostedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.EntityIsBoosted);
      this.m_needsTransportNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NeedsTransportConnected);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.UnityConsumer = Option<Mafi.Core.Population.UnityConsumer>.None;
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      if (proto.DisableLogisticsByDefault)
      {
        this.LogisticsInputMode = EntityLogisticsMode.Off;
        this.LogisticsOutputMode = EntityLogisticsMode.Off;
      }
      this.updatePortsCount();
      Lyst<RecipeProto> recipes = Machine.Cache.Recipes.ClearAndReturn();
      foreach (RecipeProto recipe in this.Prototype.Recipes)
      {
        if (unlockedProtosDb.IsUnlocked((Proto) recipe))
          recipes.Add(recipe);
      }
      if (this.Prototype.UseAllRecipesAtStartOrAfterUnlock)
        this.AssignRecipes((IEnumerable<RecipeProto>) recipes);
      else if (recipes.Count == 1)
        this.AssignRecipe(recipes.First);
      recipes.Clear();
    }

    private void updateProperties()
    {
      this.m_speedOnLowPower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.MachineSpeedOnLowPower);
      this.m_speedOnLowComputing = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.MachineSpeedOnLowComputing);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      if (this.IsDestroyed)
        return;
      this.updateProperties();
      Lyst<RecipeProto> lyst = Machine.Cache.Recipes.ClearAndReturn();
      foreach (RecipeProto recipeProto in this.m_recipesAssigned)
      {
        if (recipeProto.IsNotAvailable)
          lyst.Add(recipeProto);
      }
      foreach (RecipeProto recipeProto in lyst)
        this.m_recipesAssigned.Remove(recipeProto);
      this.rebuildRecipes(true);
      this.updatePortsCount();
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumerIfNeeded((IElectricityConsumingEntity) this, this.m_electricityConsumer);
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumerIfNeeded((IComputingConsumingEntity) this, this.m_computingConsumer);
      if (!this.IsBoostRequested)
        return;
      Mafi.Core.Population.UnityConsumer valueOrNull = this.UnityConsumer.ValueOrNull;
      if ((valueOrNull != null ? (valueOrNull.MonthlyUnity.IsNotPositive ? 1 : 0) : 0) == 0)
        return;
      this.UnityConsumer.Value?.RefreshUnityConsumed();
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    public void GetAllMissingInputs(Set<ProductProto> result)
    {
      result.Clear();
      foreach (Machine.RecipeWrapper recipeWrapper in this.m_recipesFast)
      {
        foreach (Machine.RecipeProductQuantity allInput in recipeWrapper.AllInputs)
        {
          if (allInput.Buffer.Quantity < allInput.Quantity)
            result.Add(allInput.Buffer.Product);
        }
      }
    }

    private void updatePortsCount()
    {
      ImmutableArray<IoPort> ports = this.Ports;
      this.m_inputPortsCount = ports.Count((Func<IoPort, bool>) (x => x.Type == IoPortType.Input));
      ports = this.Ports;
      this.m_outputPortsCount = ports.Count((Func<IoPort, bool>) (x => x.Type == IoPortType.Output));
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.updatePortsCount();
    }

    protected override void OnPortConnectionChanged(IoPort ourPort)
    {
      base.OnPortConnectionChanged(ourPort);
      if (ourPort.Type == IoPortType.Input)
      {
        this.onInputPortConnectionChanged(ourPort);
      }
      else
      {
        if (ourPort.Type != IoPortType.Output)
          return;
        this.onOutputPortConnectionChanged(ourPort);
      }
    }

    void IEntityWithSimUpdate.SimUpdate() => this.SimUpdateInternal();

    protected virtual void SimUpdateInternal()
    {
      if (this.IsNotEnabled)
      {
        stepDisabled(this.Maintenance.Status.IsBroken ? Machine.State.Broken : Machine.State.Paused);
        this.m_noRecipeSelectedNotif.Deactivate((IEntity) this);
      }
      else
      {
        this.m_noRecipeSelectedNotif.NotifyIff(this.m_assignedRecipesCount == 0, (IEntity) this);
        if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        {
          stepDisabled(Machine.State.NotEnoughWorkers);
        }
        else
        {
          Percent speedFactor = Percent.Hundred;
          if (this.m_computingConsumer.HasValue && !this.m_computingConsumer.CanConsume())
          {
            if (this.m_speedOnLowComputing.IsPositive && this.m_lowComputingChargeLeft.IsPositive)
            {
              speedFactor = speedFactor.Min(this.m_speedOnLowComputing);
              this.m_lowComputingChargeLeft -= Duration.OneTick;
            }
            else
            {
              stepDisabled(Machine.State.NotEnoughComputing);
              return;
            }
          }
          int num;
          if (this.IsBoostRequested)
          {
            Mafi.Core.Population.UnityConsumer valueOrNull = this.UnityConsumer.ValueOrNull;
            num = valueOrNull != null ? (valueOrNull.CanWork() ? 1 : 0) : 0;
          }
          else
            num = 0;
          this.IsBoosted = num != 0;
          if (this.m_electricityConsumer.HasValue && !this.m_electricityConsumer.Value.CanConsume() && !this.IsBoosted)
          {
            if (this.m_speedOnLowPower.IsPositive && this.m_lowPowerChargeLeft.IsPositive)
            {
              speedFactor = speedFactor.Min(this.m_speedOnLowPower);
              this.m_lowPowerChargeLeft -= Duration.OneTick;
            }
            else
            {
              stepDisabled(Machine.State.NotEnoughPower);
              return;
            }
          }
          this.m_needsTransportNotif.NotifyIff((Proto) this.m_productInNeedOfTransport.ValueOrNull, this.m_productInNeedOfTransport.HasValue, (IEntity) this);
          if (this.Maintenance.ShouldSlowDown())
            speedFactor = speedFactor.Min(Machine.SPEED_WHEN_BROKEN);
          this.setSecondarySpeedFactor(speedFactor);
          bool startedNewWorkThisTick;
          Machine.State state = this.updateWorkOnRecipes(out startedNewWorkThisTick);
          if (state != Machine.State.Working)
          {
            if (this.CurrentState == Machine.State.Working)
              this.AnimationStatesProvider.Pause();
            this.CurrentState = state;
            this.sendOutputs();
          }
          else
          {
            this.CurrentState = state;
            if (startedNewWorkThisTick)
            {
              this.AnimationStatesProvider.Start(this.m_recipeResult.Duration);
            }
            else
            {
              Percent percent1 = this.Utilization;
              Percent percent2 = percent1.ScaleBy(this.SpeedFactor);
              AnimationStatesProvider animationStatesProvider = this.AnimationStatesProvider;
              Percent utilization = this.IsBoosted ? percent2.ScaleBy(150.Percent()) : percent2;
              percent1 = this.ProgressPerc;
              Percent progress = percent1.IsNearHundred ? Percent.Zero : this.ProgressPerc;
              animationStatesProvider.Step(utilization, progress);
            }
            if (this.m_electricityConsumer.TryConsume() && this.m_speedOnLowPower.IsPositive && this.m_lowPowerChargeLeft < Machine.MaxDurationWithNoPower)
              this.m_lowPowerChargeLeft += 2.Ticks();
            if (this.m_computingConsumer.TryConsume() && this.m_speedOnLowComputing.IsPositive && this.m_lowComputingChargeLeft < Machine.MaxDurationWithNoComputing)
              this.m_lowComputingChargeLeft += 2.Ticks();
            this.sendOutputs();
          }
        }
      }

      void stepDisabled(Machine.State state)
      {
        if (this.CurrentState != Machine.State.Working)
          this.AnimationStatesProvider.Pause();
        this.CurrentState = state;
        this.m_needsTransportNotif.Deactivate((IEntity) this);
      }
    }

    public void SetBoosted(bool isBoosted)
    {
      if (isBoosted && !this.BoostCost.HasValue)
        Assert.Fail(string.Format("Cannot boost '{0}', not allowed!", (object) this.Prototype.Id));
      this.m_entityBoostedNotif.NotifyIff(isBoosted, (IEntity) this);
      this.IsBoostRequested = isBoosted;
      if (this.IsBoostRequested && this.UnityConsumer.IsNone)
        this.UnityConsumer = (Option<Mafi.Core.Population.UnityConsumer>) this.Context.UnityConsumerFactory.CreateConsumer((IUnityConsumingEntity) this);
      else if (!this.IsBoostRequested && this.UnityConsumer.HasValue)
      {
        this.UnityConsumer.Value.Destroy();
        this.UnityConsumer = (Option<Mafi.Core.Population.UnityConsumer>) Option.None;
      }
      if (!this.UnityConsumer.HasValue || !this.UnityConsumer.Value.MonthlyUnity.IsNotPositive)
        return;
      this.UnityConsumer.Value.RefreshUnityConsumed();
      Assert.That<Upoints>(this.UnityConsumer.Value.MonthlyUnity).IsPositive();
    }

    private Machine.State updateWorkOnRecipes(out bool startedNewWorkThisTick)
    {
      startedNewWorkThisTick = false;
      if (this.m_recipeResult.IsEmpty)
      {
        Percent machineUtilization;
        Machine.State newWork = this.TryGetNewWork(out machineUtilization);
        if (!this.m_recipeResult.HasResult)
          return newWork;
        startedNewWorkThisTick = true;
        this.LastRecipeInProgress = this.m_recipeResult.Recipe;
        this.m_lastWorkUtilization = machineUtilization;
      }
      this.m_recipeResult.DurationDone = (this.m_recipeResult.DurationDone + (this.IsBoosted ? 2.Ticks() : Duration.OneTick)).Min(this.m_recipeResult.Duration);
      return this.RecipeProductionTicks < this.m_recipeResult.Duration ? Machine.State.Working : this.tryPushFinishedRecipeToBuffers();
    }

    private Machine.State tryPushFinishedRecipeToBuffers()
    {
      bool flag = true;
      for (int index = 0; index < this.m_recipeResult.ProducedOutputs.Count; ++index)
      {
        ProductQuantity producedOutput = this.m_recipeResult.ProducedOutputs[index];
        if (producedOutput.IsNotEmpty)
        {
          Quantity quantity = this.m_recipeResult.Buffers[index].StoreAsMuchAs(producedOutput.Quantity);
          this.m_recipeResult.ProducedOutputs[index] = producedOutput.Product.WithQuantity(quantity);
          flag &= quantity == Quantity.Zero;
        }
      }
      if (!flag)
        return Machine.State.OutputFull;
      this.m_recipeResult.Clear();
      return Machine.State.Working;
    }

    private Machine.State canStartRecipe(
      Machine.RecipeWrapper recipe,
      out Percent utilization,
      out int multiplier)
    {
      utilization = Percent.Zero;
      multiplier = 0;
      if (recipe.MinUtilization == Percent.Hundred)
      {
        if (!recipe.CanRemoveFromInputs())
          return Machine.State.NotEnoughInput;
        if (!recipe.CanStoreToOutputs())
          return Machine.State.OutputFull;
        utilization = Percent.Hundred;
        multiplier = 1;
        return Machine.State.Working;
      }
      int num1 = int.MaxValue;
      foreach (Machine.RecipeProductQuantity allInput in recipe.AllInputs)
      {
        int num2 = allInput.Buffer.Quantity.Min(allInput.Quantity).Value / allInput.BaseFraction.Value;
        if (num2 <= 0)
          return Machine.State.NotEnoughInput;
        num1 = num1.Min(num2);
      }
      if (Percent.FromRatio(num1, recipe.QuantitiesGcd) < recipe.MinUtilization)
        return Machine.State.NotEnoughInput;
      foreach (Machine.RecipeProductQuantity recipeProductQuantity in recipe.OutputsAtStart)
      {
        int num3 = recipeProductQuantity.Buffer.UsableCapacity.Min(recipeProductQuantity.Quantity).Value / recipeProductQuantity.BaseFraction.Value;
        num1 = num1.Min(num3);
      }
      foreach (Machine.RecipeProductQuantity recipeProductQuantity in recipe.OutputsAtEnd)
      {
        int num4 = recipeProductQuantity.Buffer.UsableCapacity.Min(recipeProductQuantity.Quantity).Value / recipeProductQuantity.BaseFraction.Value;
        num1 = num1.Min(num4);
      }
      if (num1 <= 0)
        return Machine.State.OutputFull;
      Assert.That<int>(num1).IsLessOrEqual(recipe.QuantitiesGcd);
      utilization = Percent.FromRatio(num1, recipe.QuantitiesGcd);
      if (utilization < recipe.MinUtilization)
        return Machine.State.OutputFull;
      multiplier = num1;
      return Machine.State.Working;
    }

    protected virtual Machine.State TryGetNewWork(out Percent machineUtilization)
    {
      machineUtilization = Percent.Zero;
      if (this.m_recipesFast.IsEmpty)
        return Machine.State.NoRecipes;
      if (this.m_hasNoNeedToSearchForRecipe)
        return this.m_statusWhenNoNeedForRecipe;
      this.m_recipesWithFullOutput.Clear();
      Machine.RecipeWrapper? nullable = new Machine.RecipeWrapper?();
      int num = 1;
      Machine.State newWork = Machine.State.Working;
      foreach (Machine.RecipeWrapper recipe in this.m_recipesFast)
      {
        Percent utilization;
        int multiplier;
        Machine.State state = this.canStartRecipe(recipe, out utilization, out multiplier);
        if (newWork != Machine.State.OutputFull || state != Machine.State.NotEnoughInput)
          newWork = state;
        if (state == Machine.State.OutputFull && recipe.RecipesWithSameOutputs.IsNotEmpty)
          this.m_recipesWithFullOutput.Add(recipe.Recipe);
        else if (state == Machine.State.Working && !(utilization <= machineUtilization))
        {
          if (this.m_recipesWithFullOutput.IsNotEmpty && recipe.RecipesWithSameOutputs.IsNotEmpty)
          {
            bool flag = false;
            foreach (RecipeProto recipeProto in this.m_recipesWithFullOutput)
            {
              if (recipe.RecipesWithSameOutputs.Contains(recipeProto))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              continue;
          }
          machineUtilization = utilization;
          nullable = new Machine.RecipeWrapper?(recipe);
          num = multiplier;
          if (utilization == Percent.Hundred)
            break;
        }
      }
      if (!nullable.HasValue)
      {
        this.m_hasNoNeedToSearchForRecipe = !this.UsesVirtualLimitedBuffers;
        this.m_statusWhenNoNeedForRecipe = newWork;
        return newWork;
      }
      Machine.RecipeWrapper recipeWrapper = nullable.Value;
      Lyst<ProductQuantity> inputs = Machine.Cache.InputsUsed.ClearAndReturn();
      Lyst<ProductQuantity> outputs = Machine.Cache.OutputsCreated.ClearAndReturn();
      foreach (Machine.RecipeProductQuantity allInput in recipeWrapper.AllInputs)
      {
        Quantity quantity1 = num * allInput.BaseFraction;
        Quantity quantity2 = allInput.Buffer.RemoveAsMuchAs(quantity1);
        inputs.Add(allInput.Buffer.Product.WithQuantity(quantity2));
        Assert.That<Quantity>(quantity2).IsEqualTo(quantity1);
      }
      foreach (Machine.RecipeProductQuantity recipeProductQuantity in recipeWrapper.OutputsAtStart)
      {
        Quantity quantity3 = num * recipeProductQuantity.BaseFraction;
        Quantity quantity4 = recipeProductQuantity.Buffer.StoreAsMuchAsReturnStored(quantity3);
        outputs.Add(recipeProductQuantity.Buffer.Product.WithQuantity(quantity4));
        Assert.That<Quantity>(quantity4).IsEqualTo(quantity3);
      }
      if (this.m_recipeResult.HasResult)
      {
        Log.Error("Previous result not cleared?");
        this.m_recipeResult.Clear();
      }
      this.m_recipeResult.SetRecipe(recipeWrapper.Recipe, this.DurationMultiplier);
      for (int index = 0; index < recipeWrapper.OutputsAtEnd.Length; ++index)
      {
        IProductBuffer buffer = recipeWrapper.OutputsAtEnd[index].Buffer;
        Quantity quantity = num * recipeWrapper.OutputsAtEnd[index].BaseFraction;
        ProductQuantity productQuantity = buffer.Product.WithQuantity(quantity);
        outputs.Add(productQuantity);
        this.m_recipeResult.ProducedOutputs.Add(productQuantity);
        this.m_recipeResult.Buffers.Add(buffer);
      }
      this.m_productsManager.ReportProductsTransformation((IIndexable<ProductQuantity>) inputs, (IIndexable<ProductQuantity>) outputs, recipeWrapper.Recipe.ProductsDestroyReason, CreateReason.Produced, recipeWrapper.Recipe.DisableSourceProductsConversionLoss);
      return Machine.State.Working;
    }

    public void AssignRecipes(IEnumerable<RecipeProto> recipes)
    {
      foreach (RecipeProto recipe in recipes)
      {
        if (!this.m_recipesAssigned.AddIfNotPresent(recipe))
        {
          Log.Error("Recipe already added!");
          return;
        }
      }
      this.rebuildRecipes(false);
    }

    public void AssignRecipe(RecipeProto recipe)
    {
      if (recipe.IsNotAvailable)
        Log.Error("Recipe not available!");
      else if (!this.m_recipesAssigned.AddIfNotPresent(recipe))
        Log.Error("Recipe already added!");
      else
        this.rebuildRecipes(false);
    }

    public void RemoveAssignedRecipe(RecipeProto recipe)
    {
      if (!this.m_recipesAssigned.Remove(recipe))
        Log.Error("Recipe not found!");
      else
        this.rebuildRecipes(false);
    }

    public void ClearAssignedRecipes()
    {
      this.m_recipesAssigned.Clear();
      this.rebuildRecipes(false);
    }

    /// <summary>
    /// Set fullRebuild = true, if Proto changed or game was loaded.
    /// </summary>
    private void rebuildRecipes(bool fullRebuild)
    {
      this.m_hasNoNeedToSearchForRecipe = false;
      this.rebuildOutputBuffers(fullRebuild);
      this.rebuildInputBuffers(fullRebuild);
      this.m_assignedRecipesCount = this.m_recipesAssigned.Count;
      this.m_recipesFast.Clear();
      Lyst<RecipeProto> lyst = Machine.Cache.Recipes.ClearAndReturn();
      foreach (RecipeProto recipe in this.m_recipesAssigned)
      {
        foreach (RecipeProto recipeProto in this.m_recipesAssigned)
        {
          RecipeProto otherRecipe = recipeProto;
          if (!((Proto) recipe == (Proto) otherRecipe) && otherRecipe.AllOutputs.Length == recipe.AllOutputs.Length && recipe.AllOutputs.All((Func<RecipeOutput, bool>) (output => otherRecipe.AllOutputs.Any((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) output.Product)))))
            lyst.Add(otherRecipe);
        }
        this.m_recipesFast.Add(new Machine.RecipeWrapper(recipe, this, lyst.ToImmutableArrayAndClear()));
      }
      lyst.Clear();
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      if (this.LogisticsInputMode == mode)
        return;
      this.LogisticsInputMode = mode;
      if (this.LogisticsInputMode == EntityLogisticsMode.Off)
      {
        foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
          inputBuffer.UnregisterFromLogistics(true);
      }
      else
      {
        foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
        {
          if (!inputBuffer.IsNotUsedByCurrentRecipes)
            inputBuffer.RegisterBufferToLogistics(true);
        }
      }
    }

    private void onInputPortConnectionChanged(IoPort port)
    {
      if (port.IsConnected || this.LogisticsInputMode != EntityLogisticsMode.Auto)
        return;
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
      {
        if (!(inputBuffer.Product.Type != port.ShapePrototype.AllowedProductType) && !inputBuffer.IsNotUsedByCurrentRecipes)
          inputBuffer.RegisterBufferToLogistics(true);
      }
    }

    private void rebuildInputBuffers(bool isUpgrade)
    {
      if (isUpgrade)
      {
        Set<ProductProto> set = Machine.Cache.ProductsSet.ClearAndReturn();
        foreach (RecipeProto recipe in this.Prototype.Recipes)
          set.AddRange(recipe.AllInputs.Select<ProductProto>((Func<RecipeInput, ProductProto>) (x => x.Product)));
        Lyst<Machine.MachineInputBuffer> lyst = Machine.Cache.InputBuffers.ClearAndReturn();
        foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
        {
          if (!set.Contains(inputBuffer.Product))
            lyst.Add(inputBuffer);
        }
        foreach (Machine.MachineInputBuffer buffer in lyst)
        {
          this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) buffer);
          this.m_inputBuffers.TryRemoveReplaceLast(buffer);
        }
      }
      Set<ProductProto> set1 = Machine.Cache.ProductsSet.ClearAndReturn();
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
      {
        if (!inputBuffer.IsNotUsedByCurrentRecipes)
          set1.Add(inputBuffer.Product);
      }
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
        inputBuffer.MinCapacity = Quantity.Zero;
      foreach (RecipeProto recipeProto in this.m_recipesAssigned)
      {
        foreach (RecipeInput allInput in recipeProto.AllInputs)
        {
          if (!(allInput.Product.Type == VirtualProductProto.ProductType))
          {
            foreach (IoPortTemplate port in allInput.Ports)
              Assert.That<IoPortType>(port.Type).IsEqualTo<IoPortType>(IoPortType.Input);
            Machine.MachineInputBuffer result;
            if (!this.tryGetInputBuffer(allInput.Product, out result))
            {
              result = new Machine.MachineInputBuffer(Quantity.One, allInput.Product, this);
              this.m_inputBuffers.Add(result);
            }
            result.MinCapacity = result.MinCapacity.Max(allInput.Quantity);
          }
        }
      }
      Lyst<Machine.MachineInputBuffer> lyst1 = Machine.Cache.InputBuffers.ClearAndReturn();
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
      {
        if (inputBuffer.IsNotUsedByCurrentRecipes)
        {
          if (inputBuffer.IsEmpty)
            lyst1.Add(inputBuffer);
          else
            inputBuffer.UnregisterFromLogistics(false);
        }
        else if (!set1.Contains(inputBuffer.Product) && this.LogisticsInputMode != EntityLogisticsMode.Off)
          inputBuffer.RegisterBufferToLogistics(false);
      }
      foreach (Machine.MachineInputBuffer machineInputBuffer in lyst1)
      {
        machineInputBuffer.Destroy();
        this.m_inputBuffers.TryRemoveReplaceLast(machineInputBuffer);
      }
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
        inputBuffer.UpdateCapacity();
    }

    private bool tryGetInputBuffer(ProductProto product, out Machine.MachineInputBuffer result)
    {
      foreach (Machine.MachineInputBuffer inputBuffer in this.m_inputBuffers)
      {
        if ((Proto) inputBuffer.Product == (Proto) product)
        {
          result = inputBuffer;
          return true;
        }
      }
      result = (Machine.MachineInputBuffer) null;
      return false;
    }

    /// <summary>Called by ports to store products.</summary>
    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      Machine.MachineInputBuffer result;
      if (pq.IsEmpty || !this.IsEnabled || !this.tryGetInputBuffer(pq.Product, out result) || result.IsNotUsedByCurrentRecipes)
        return pq.Quantity;
      if (this.LogisticsInputMode == EntityLogisticsMode.Auto)
        result.UnregisterFromLogistics(true);
      return result.StoreAsMuchAs(pq.Quantity);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      if (this.LogisticsOutputMode == mode)
        return;
      this.LogisticsOutputMode = mode;
      if (this.LogisticsOutputMode == EntityLogisticsMode.Off)
      {
        foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
          outputBuffer.UnregisterFromLogistics(true);
      }
      else
      {
        foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
          outputBuffer.RegisterBufferToLogistics(true);
      }
      this.m_hasNoNeedToSearchForRecipe = false;
    }

    /// <summary>
    /// Sends products in buffers through output ports, if possible.
    /// </summary>
    private void sendOutputs()
    {
      if (this.m_allConnectedOutputsBuffersEmpty && this.m_productInNeedOfTransport.IsNone)
        return;
      bool flag = true;
      Option<ProductProto> option = Option<ProductProto>.None;
      foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
      {
        if (!outputBuffer.IsEmpty)
        {
          if (!outputBuffer.IsAnyPortConnected)
          {
            if (!outputBuffer.IsRegisteredToLogistics)
              option = (Option<ProductProto>) outputBuffer.Product;
          }
          else
          {
            outputBuffer.Send();
            flag &= outputBuffer.IsEmpty;
          }
        }
      }
      this.m_allConnectedOutputsBuffersEmpty = flag;
      this.m_productInNeedOfTransport = option;
    }

    private bool tryGetOutputBuffer(ProductProto product, out Machine.MachineOutputBuffer result)
    {
      foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
      {
        if ((Proto) outputBuffer.Product == (Proto) product)
        {
          result = outputBuffer;
          return true;
        }
      }
      result = (Machine.MachineOutputBuffer) null;
      return false;
    }

    private void rebuildOutputBuffers(bool isUpgrade)
    {
      if (isUpgrade)
      {
        Set<ProductProto> set = Machine.Cache.ProductsSet.ClearAndReturn();
        foreach (RecipeProto recipe in this.Prototype.Recipes)
          set.AddRange(recipe.AllOutputs.Select<ProductProto>((Func<RecipeOutput, ProductProto>) (x => x.Product)));
        Lyst<Machine.MachineOutputBuffer> lyst = Machine.Cache.OutputBuffers.ClearAndReturn();
        foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
        {
          if (!set.Contains(outputBuffer.Product))
            lyst.Add(outputBuffer);
        }
        foreach (Machine.MachineOutputBuffer buffer in lyst)
          this.clearAndRemoveOutputBuffer(buffer);
        lyst.Clear();
      }
      foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
      {
        outputBuffer.MinCapacity = Quantity.Zero;
        outputBuffer.ClearAllPorts();
      }
      foreach (RecipeProto recipeProto in this.m_recipesAssigned)
      {
        foreach (RecipeOutput allOutput in recipeProto.AllOutputs)
        {
          if (!(allOutput.Product.Type == VirtualProductProto.ProductType))
          {
            Machine.MachineOutputBuffer result;
            if (!this.tryGetOutputBuffer(allOutput.Product, out result))
            {
              result = new Machine.MachineOutputBuffer(Quantity.One, allOutput.Product, this.m_productsManager, this);
              this.m_outputBuffers.Add(result);
            }
            result.MinCapacity = result.MinCapacity.Max(allOutput.Quantity);
            foreach (IoPortTemplate port in allOutput.Ports)
            {
              IoPortTemplate portProto = port;
              Assert.That<IoPortType>(portProto.Type).IsEqualTo<IoPortType>(IoPortType.Output);
              IoPort orDefault = this.Ports.FindOrDefault((Predicate<IoPort>) (x => (int) x.Name == (int) portProto.Name));
              if (orDefault != null)
                result.AddPort(orDefault);
            }
          }
        }
      }
      Lyst<Machine.MachineOutputBuffer> lyst1 = Machine.Cache.OutputBuffers.ClearAndReturn();
      foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
      {
        if (outputBuffer.IsNotUsedByCurrentRecipes && outputBuffer.Quantity.IsZero)
        {
          lyst1.Add(outputBuffer);
        }
        else
        {
          if (this.LogisticsOutputMode != EntityLogisticsMode.Off && !outputBuffer.IsAnyPortConnected)
            outputBuffer.RegisterBufferToLogistics(false);
          outputBuffer.UpdateCapacity();
        }
      }
      foreach (Machine.MachineOutputBuffer buffer in lyst1)
        this.clearAndRemoveOutputBuffer(buffer);
      lyst1.Clear();
    }

    private void clearAndRemoveOutputBuffer(Machine.MachineOutputBuffer buffer)
    {
      this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) buffer);
      this.m_outputBuffers.TryRemoveReplaceLast(buffer);
      if (!this.m_recipeResult.HasResult)
        return;
      foreach (IProductBuffer buffer1 in this.m_recipeResult.Buffers)
      {
        if (buffer == buffer1)
        {
          this.destroyRecipeResult();
          break;
        }
      }
    }

    private void onOutputPortConnectionChanged(IoPort port)
    {
      foreach (Machine.MachineOutputBuffer outputBuffer in this.m_outputBuffers)
        outputBuffer.OnSomePortConnectionChanged(port);
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    protected override void OnUpgradeDone(IEntityProto oldProto, IEntityProto newProto)
    {
      base.OnUpgradeDone(oldProto, newProto);
      Set<RecipeProto> items = new Set<RecipeProto>();
      foreach (RecipeProto recipeProto1 in this.m_recipesAssigned)
      {
        RecipeProto recipe = recipeProto1;
        RecipeProto recipeProto2 = this.Prototype.Recipes.FirstOrDefault<RecipeProto>((Predicate<RecipeProto>) (x => MachineUtils.AreRecipesSame(x, recipe)));
        if ((Proto) recipeProto2 != (Proto) null)
          items.Add(recipeProto2);
      }
      this.destroyRecipeResult();
      this.m_recipesAssigned.Clear();
      this.m_recipesAssigned.AddRange((IEnumerable<RecipeProto>) items);
      this.rebuildRecipes(true);
      if (this.IsBoostRequested && !this.Prototype.BoostCost.HasValue)
        this.SetBoosted(false);
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumerIfNeeded((IElectricityConsumingEntity) this, this.m_electricityConsumer);
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumerIfNeeded((IComputingConsumingEntity) this, this.m_computingConsumer);
    }

    private void destroyRecipeResult()
    {
      if (!this.m_recipeResult.HasResult)
        return;
      foreach (ProductQuantity producedOutput in this.m_recipeResult.ProducedOutputs)
        this.m_productsManager.ClearProductNoChecks(producedOutput);
      this.m_recipeResult.Clear();
    }

    protected override void OnDestroy()
    {
      this.destroyRecipeResult();
      this.m_recipesAssigned.Clear();
      foreach (IProductBuffer inputBuffer in this.m_inputBuffers)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(inputBuffer);
      this.m_inputBuffers.Clear();
      foreach (IProductBuffer outputBuffer in this.m_outputBuffers)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(outputBuffer);
      this.m_outputBuffers.Clear();
      base.OnDestroy();
    }

    public void ReorderRecipe(RecipeProto recipe, int indexDiff)
    {
      if (indexDiff != 1 && indexDiff != -1)
        return;
      int index1 = this.m_recipesAssigned.IndexOf(recipe);
      int index2 = index1 + indexDiff;
      if (index1 < 0 || index2 < 0 || index1 >= this.m_recipesAssigned.Count || index2 >= this.m_recipesAssigned.Count)
      {
        Log.Error(string.Format("Indices '{0}','{1}','{2}' out of range: 0 - {3}", (object) indexDiff, (object) index1, (object) index2, (object) this.m_recipesAssigned.Count));
      }
      else
      {
        RecipeProto recipeProto1 = this.m_recipesAssigned[index1];
        RecipeProto recipeProto2 = this.m_recipesAssigned[index2];
        this.m_recipesAssigned[index2] = recipeProto1;
        this.m_recipesAssigned[index1] = recipeProto2;
        Machine.RecipeWrapper recipeWrapper1 = this.m_recipesFast[index1];
        Machine.RecipeWrapper recipeWrapper2 = this.m_recipesFast[index2];
        this.m_recipesFast[index2] = recipeWrapper1;
        this.m_recipesFast[index1] = recipeWrapper2;
      }
    }

    public bool HasClearProductsActionFor(IRecipeForUi recipe)
    {
      foreach (RecipeProto recipeProto in this.m_recipesAssigned)
      {
        if (recipeProto == recipe)
          return false;
      }
      foreach (RecipeProduct userVisibleInput in recipe.AllUserVisibleInputs)
      {
        if (this.tryClearBufferForInput(userVisibleInput.Product, true))
          return true;
      }
      foreach (RecipeProduct userVisibleOutput in recipe.AllUserVisibleOutputs)
      {
        if (this.tryClearBufferForOutput(userVisibleOutput.Product, true))
          return true;
      }
      return false;
    }

    public void ClearProductsForRecipe(IRecipeForUi recipe)
    {
      foreach (RecipeProduct userVisibleInput in recipe.AllUserVisibleInputs)
        this.tryClearBufferForInput(userVisibleInput.Product, false);
      foreach (RecipeProduct userVisibleOutput in recipe.AllUserVisibleOutputs)
        this.tryClearBufferForOutput(userVisibleOutput.Product, false);
    }

    private bool tryClearBufferForInput(ProductProto product, bool simulateOnly)
    {
      Machine.MachineInputBuffer result;
      if (product.Type == VirtualProductProto.ProductType || !this.tryGetInputBuffer(product, out result) || !result.IsNotUsedByCurrentRecipes)
        return false;
      if (simulateOnly)
        return result.IsNotEmpty;
      this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) result);
      this.m_inputBuffers.TryRemoveReplaceLast(result);
      return true;
    }

    private bool tryClearBufferForOutput(ProductProto product, bool simulateOnly)
    {
      Machine.MachineOutputBuffer result;
      if (product.Type == VirtualProductProto.ProductType || !this.tryGetOutputBuffer(product, out result) || !result.IsNotUsedByCurrentRecipes)
        return false;
      if (simulateOnly)
        return result.IsNotEmpty;
      this.clearAndRemoveOutputBuffer(result);
      return true;
    }

    public virtual void AddToConfig(EntityConfigData data)
    {
      if (!this.RecipesAssigned.IsNotEmpty<RecipeProto>())
        return;
      data.Recipes = new ImmutableArray<RecipeProto>?(this.RecipesAssigned.ToImmutableArray<RecipeProto>());
    }

    public virtual void ApplyConfig(EntityConfigData data)
    {
      if (data.Prototype.IsNone || !UpgradeHelper.AreProtosInSameUpgradeChain((Proto) this.Prototype, data.Prototype.Value))
        return;
      ImmutableArray<RecipeProto> immutableArray = data.Recipes ?? ImmutableArray<RecipeProto>.Empty;
      this.m_recipesAssigned.Clear();
      foreach (RecipeProto recipeProto1 in immutableArray)
      {
        RecipeProto recipe = recipeProto1;
        RecipeProto recipeProto2 = this.Prototype.Recipes.FirstOrDefault<RecipeProto>((Predicate<RecipeProto>) (x => MachineUtils.AreRecipesSame(x, recipe)));
        if ((Proto) recipeProto2 != (Proto) null)
          this.m_recipesAssigned.Add(recipeProto2);
      }
      this.rebuildRecipes(false);
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsEnabled)
        return;
      this.m_vehicleBuffersRegistry.ClearAndCancelAllJobs((IStaticEntity) this);
    }

    public Quantity GetInputQuantityFor(ProductProto product)
    {
      Machine.MachineInputBuffer result;
      return !this.tryGetInputBuffer(product, out result) ? Quantity.Zero : result.Quantity;
    }

    public Quantity GetInputCapacityFor(ProductProto product)
    {
      Machine.MachineInputBuffer result;
      return !this.tryGetInputBuffer(product, out result) ? Quantity.Zero : result.Capacity;
    }

    public Quantity GetOutputQuantityFor(ProductProto product)
    {
      Machine.MachineOutputBuffer result;
      return !this.tryGetOutputBuffer(product, out result) ? Quantity.Zero : result.Quantity;
    }

    public Quantity GetOutputCapacityFor(ProductProto product)
    {
      Machine.MachineOutputBuffer result;
      return !this.tryGetOutputBuffer(product, out result) ? Quantity.Zero : result.Capacity;
    }

    public Percent ProgressOnRecipe(IRecipeForUi recipe)
    {
      return this.LastRecipeInProgress.ValueOrNull != recipe ? Percent.Zero : this.ProgressPerc;
    }

    public Duration GetTargetDurationFor(IRecipeForUi recipe)
    {
      Duration duration = recipe.Duration.ScaledBy(this.DurationMultiplier);
      return !this.IsBoosted ? duration : duration / 2;
    }

    protected void SetBaseSpeedFactor(Percent speedFactorBase)
    {
      if (this.m_speedFactorBase == speedFactorBase)
        return;
      if (speedFactorBase.IsNotPositive)
      {
        Log.Error(string.Format("Non positive base speed factor {0} is not allowed!", (object) speedFactorBase));
      }
      else
      {
        this.m_speedFactorBase = speedFactorBase;
        this.updateSpeedFactor();
      }
    }

    private void setSecondarySpeedFactor(Percent speedFactor)
    {
      if (this.m_speedFactorSecondary == speedFactor)
        return;
      if (speedFactor.IsNotPositive)
      {
        Log.Error(string.Format("Non positive speed factor {0} is not allowed!", (object) speedFactor));
      }
      else
      {
        this.m_speedFactorSecondary = speedFactor;
        this.updateSpeedFactor();
      }
    }

    private void updateSpeedFactor()
    {
      Percent percent = this.m_speedFactorBase.ScaleBy(this.m_speedFactorSecondary);
      if (!this.m_recipeResult.HasResult)
      {
        this.SpeedFactor = percent;
      }
      else
      {
        if (!this.SpeedFactor.IsNearHundred)
          this.m_recipeResult.DurationDone = this.m_recipeResult.DurationDone.ScaledBy(this.SpeedFactor);
        this.SpeedFactor = percent;
        this.m_recipeResult.Duration = this.m_recipeResult.Recipe.Value.Duration.ScaledBy(this.DurationMultiplier);
        this.m_recipeResult.DurationDone = this.m_recipeResult.DurationDone.ScaledBy(this.DurationMultiplier);
      }
    }

    public virtual LocStrFormatted GetSlowDownMessageForUi()
    {
      if (this.SpeedFactor >= Percent.Hundred)
        return LocStrFormatted.Empty;
      string str = TrCore.SpeedReduced__Machine.Format(this.SpeedFactor.ToStringRounded()).Value;
      IElectricityConsumerReadonly valueOrNull1 = this.ElectricityConsumer.ValueOrNull;
      if ((valueOrNull1 != null ? (valueOrNull1.NotEnoughPower ? 1 : 0) : 0) != 0)
        str += string.Format("\n- {0}", (object) TrCore.EntityStatus__LowPower);
      IComputingConsumerReadonly valueOrNull2 = this.ComputingConsumer.ValueOrNull;
      if ((valueOrNull2 != null ? (valueOrNull2.NotEnoughComputing ? 1 : 0) : 0) != 0)
        str += string.Format("\n- {0}", (object) TrCore.EntityStatus__NoComputing);
      if (this.Maintenance.Status.IsBroken)
        str += string.Format("\n- {0}", (object) TrCore.EntityStatus__Broken);
      return new LocStrFormatted(str);
    }

    internal Option<IProductBufferReadOnly> GetInternalOutputBufferFor(ProductProto product)
    {
      Machine.MachineOutputBuffer result;
      return this.tryGetOutputBuffer(product, out result) ? (Option<IProductBufferReadOnly>) (IProductBufferReadOnly) result : Option<IProductBufferReadOnly>.None;
    }

    public static void Serialize(Machine value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Machine>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Machine.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      Option<Mafi.Core.Population.UnityConsumer>.Serialize(this.UnityConsumer, writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteBool(this.IsBoosted);
      writer.WriteBool(this.IsBoostRequested);
      Option<RecipeProto>.Serialize(this.LastRecipeInProgress, writer);
      writer.WriteInt((int) this.LogisticsInputMode);
      writer.WriteInt((int) this.LogisticsOutputMode);
      Option<IComputingConsumer>.Serialize(this.m_computingConsumer, writer);
      Option<IInputBufferPriorityProvider>.Serialize(this.m_customStrategy, writer);
      Option<IElectricityConsumer>.Serialize(this.m_electricityConsumer, writer);
      EntityNotificator.Serialize(this.m_entityBoostedNotif, writer);
      writer.WriteGeneric<IProperty<bool>>(this.m_forceRunEnabled);
      LystStruct<Machine.MachineInputBuffer>.Serialize(this.m_inputBuffers, writer);
      Percent.Serialize(this.m_lastWorkUtilization, writer);
      Duration.Serialize(this.m_lowComputingChargeLeft, writer);
      Duration.Serialize(this.m_lowPowerChargeLeft, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_needsTransportNotif, writer);
      EntityNotificator.Serialize(this.m_noRecipeSelectedNotif, writer);
      LystStruct<Machine.MachineOutputBuffer>.Serialize(this.m_outputBuffers, writer);
      Option<ProductProto>.Serialize(this.m_productInNeedOfTransport, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<MachineProto>(this.m_proto);
      Machine.RecipeResult.Serialize(this.m_recipeResult, writer);
      Lyst<RecipeProto>.Serialize(this.m_recipesAssigned, writer);
      Percent.Serialize(this.m_speedFactorBase, writer);
      Percent.Serialize(this.m_speedFactorSecondary, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      VirtualBuffersMap.Serialize(this.m_virtualBuffersMap, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Percent.Serialize(this.SpeedFactor, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Machine Deserialize(BlobReader reader)
    {
      Machine machine;
      if (reader.TryStartClassDeserialization<Machine>(out machine))
        reader.EnqueueDataDeserialization((object) machine, Machine.s_deserializeDataDelayedAction);
      return machine;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.UnityConsumer = Option<Mafi.Core.Population.UnityConsumer>.Deserialize(reader);
      this.CurrentState = (Machine.State) reader.ReadInt();
      this.IsBoosted = reader.ReadBool();
      this.IsBoostRequested = reader.ReadBool();
      this.LastRecipeInProgress = Option<RecipeProto>.Deserialize(reader);
      this.LogisticsInputMode = (EntityLogisticsMode) reader.ReadInt();
      this.LogisticsOutputMode = (EntityLogisticsMode) reader.ReadInt();
      this.m_computingConsumer = Option<IComputingConsumer>.Deserialize(reader);
      reader.SetField<Machine>(this, "m_customStrategy", (object) Option<IInputBufferPriorityProvider>.Deserialize(reader));
      this.m_electricityConsumer = Option<IElectricityConsumer>.Deserialize(reader);
      this.m_entityBoostedNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<Machine>(this, "m_forceRunEnabled", (object) reader.ReadGenericAs<IProperty<bool>>());
      this.m_inputBuffers = LystStruct<Machine.MachineInputBuffer>.Deserialize(reader);
      this.m_lastWorkUtilization = Percent.Deserialize(reader);
      this.m_lowComputingChargeLeft = reader.LoadedSaveVersion >= 140 ? Duration.Deserialize(reader) : new Duration();
      this.m_lowPowerChargeLeft = reader.LoadedSaveVersion >= 140 ? Duration.Deserialize(reader) : new Duration();
      this.m_needsTransportNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_noRecipeSelectedNotif = EntityNotificator.Deserialize(reader);
      this.m_outputBuffers = LystStruct<Machine.MachineOutputBuffer>.Deserialize(reader);
      this.m_productInNeedOfTransport = Option<ProductProto>.Deserialize(reader);
      reader.SetField<Machine>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<MachineProto>();
      this.m_recipeResult = Machine.RecipeResult.Deserialize(reader);
      reader.SetField<Machine>(this, "m_recipesAssigned", (object) Lyst<RecipeProto>.Deserialize(reader));
      this.m_recipesFast = new LystStruct<Machine.RecipeWrapper>();
      this.m_recipesWithFullOutput = new LystStruct<RecipeProto>();
      this.m_speedFactorBase = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : Percent.Hundred;
      this.m_speedFactorSecondary = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : Percent.Hundred;
      reader.SetField<Machine>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<Machine>(this, "m_virtualBuffersMap", (object) VirtualBuffersMap.Deserialize(reader));
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.SpeedFactor = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : Percent.Hundred;
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<Machine>(this, "initSelf", InitPriority.Normal);
    }

    static Machine()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Machine.s_cache = new ThreadLocal<Machine.CacheContext>((Func<Machine.CacheContext>) (() => new Machine.CacheContext()));
      Machine.SPEED_WHEN_BROKEN = 50.Percent();
      Machine.MaxDurationWithNoPower = 8.Months();
      Machine.MaxDurationWithNoComputing = 8.Months();
      Machine.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Machine.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      None,
      Broken,
      Paused,
      NotEnoughWorkers,
      NotEnoughPower,
      NotEnoughComputing,
      NotEnoughInput,
      InvalidPlacement,
      OutputFull,
      NoRecipes,
      Working,
    }

    [GenerateSerializer(false, null, 0)]
    internal struct RecipeResult
    {
      public LystStruct<ProductQuantity> ProducedOutputs;
      public LystStruct<IProductBuffer> Buffers;
      public Duration Duration;
      public Duration DurationDone;

      public bool HasResult => this.Recipe.HasValue;

      public bool IsEmpty => this.Recipe.IsNone;

      public Option<RecipeProto> Recipe { get; private set; }

      public RecipeResult()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ProducedOutputs = new LystStruct<ProductQuantity>();
        this.Buffers = new LystStruct<IProductBuffer>();
        this.Duration = Duration.Zero;
        this.DurationDone = Duration.Zero;
        this.Recipe = Option<RecipeProto>.None;
      }

      public void Clear()
      {
        this.ProducedOutputs.Clear();
        this.Buffers.Clear();
        this.Recipe = Option<RecipeProto>.None;
        this.Duration = Duration.Zero;
        this.DurationDone = Duration.Zero;
      }

      public void SetRecipe(RecipeProto recipe, Percent durationMultiplier)
      {
        Assert.That<bool>(this.IsEmpty).IsTrue();
        this.Recipe = (Option<RecipeProto>) recipe;
        this.Duration = recipe.Duration.ScaledBy(durationMultiplier);
      }

      public static void Serialize(Machine.RecipeResult value, BlobWriter writer)
      {
        LystStruct<IProductBuffer>.Serialize(value.Buffers, writer);
        Duration.Serialize(value.Duration, writer);
        Duration.Serialize(value.DurationDone, writer);
        LystStruct<ProductQuantity>.Serialize(value.ProducedOutputs, writer);
        Option<RecipeProto>.Serialize(value.Recipe, writer);
      }

      public static Machine.RecipeResult Deserialize(BlobReader reader)
      {
        return new Machine.RecipeResult()
        {
          Buffers = LystStruct<IProductBuffer>.Deserialize(reader),
          Duration = Duration.Deserialize(reader),
          DurationDone = Duration.Deserialize(reader),
          ProducedOutputs = LystStruct<ProductQuantity>.Deserialize(reader),
          Recipe = Option<RecipeProto>.Deserialize(reader)
        };
      }
    }

    /// <summary>
    /// We are using normal priority for waste to not bother logistics. IF anything needs to get
    /// rid of waste it will have high prio set on its own.
    /// </summary>
    [GenerateSerializer(false, "Instance", 0)]
    public class WasteInputPortPriorityProvider : IInputBufferPriorityProvider
    {
      public static readonly Machine.WasteInputPortPriorityProvider Instance;

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        return new BufferStrategy(15, new Quantity?(buffer.Capacity / 2));
      }

      public static void Serialize(Machine.WasteInputPortPriorityProvider value, BlobWriter writer)
      {
        writer.WriteBool(value != null);
      }

      public static Machine.WasteInputPortPriorityProvider Deserialize(BlobReader reader)
      {
        return !reader.ReadBool() ? (Machine.WasteInputPortPriorityProvider) null : Machine.WasteInputPortPriorityProvider.Instance;
      }

      public WasteInputPortPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static WasteInputPortPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Machine.WasteInputPortPriorityProvider.Instance = new Machine.WasteInputPortPriorityProvider();
      }
    }

    /// <summary>
    /// Provides caches for one-time or init-on-load operations.
    /// Instead of having these in memory for each storage we share them.
    /// WARNING: Clear them before using them!
    /// </summary>
    private class CacheContext
    {
      public readonly Lyst<ProductQuantity> InputsUsed;
      public readonly Lyst<ProductQuantity> OutputsCreated;
      public readonly Lyst<RecipeProto> Recipes;
      public readonly Lyst<Machine.MachineOutputBuffer> OutputBuffers;
      public readonly Lyst<Machine.MachineInputBuffer> InputBuffers;
      public readonly Set<ProductProto> ProductsSet;

      public CacheContext()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.InputsUsed = new Lyst<ProductQuantity>(8);
        this.OutputsCreated = new Lyst<ProductQuantity>(8);
        this.Recipes = new Lyst<RecipeProto>(32);
        this.OutputBuffers = new Lyst<Machine.MachineOutputBuffer>(32);
        this.InputBuffers = new Lyst<Machine.MachineInputBuffer>(32);
        this.ProductsSet = new Set<ProductProto>(32);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    [GenerateSerializer(false, null, 0)]
    public sealed class MachineInputBuffer : ProductBuffer, IInputBufferPriorityProvider
    {
      [DoNotSave(0, null)]
      public Quantity MinCapacity;
      private readonly Machine m_entity;
      private bool m_isRegisteredToLogistics;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public bool IsNotUsedByCurrentRecipes => this.MinCapacity.IsZero;

      public MachineInputBuffer(Quantity capacity, ProductProto product, Machine entity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_entity = entity;
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        base.OnQuantityChanged(diff);
        if (!diff.IsPositive)
          return;
        this.m_entity.m_hasNoNeedToSearchForRecipe = false;
      }

      public void RegisterBufferToLogistics(bool updateCapacity)
      {
        if (this.m_isRegisteredToLogistics)
          return;
        this.m_entity.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this.m_entity, (IProductBuffer) this, this.m_entity.m_customStrategy.ValueOrNull ?? (IInputBufferPriorityProvider) this);
        this.m_isRegisteredToLogistics = true;
        if (!updateCapacity)
          return;
        this.UpdateCapacity();
      }

      public void UnregisterFromLogistics(bool updateCapacity)
      {
        if (!this.m_isRegisteredToLogistics)
          return;
        this.m_entity.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) this);
        this.m_isRegisteredToLogistics = false;
        if (!updateCapacity)
          return;
        this.UpdateCapacity();
      }

      public void UpdateCapacity()
      {
        if (this.MinCapacity.IsZero)
          return;
        this.ForceNewCapacityTo(!this.m_entity.Prototype.BuffersMultiplier.HasValue ? (!this.m_isRegisteredToLogistics ? this.MinCapacity : (4 * this.MinCapacity).Max(2 * TruckCaps.SmallTruckCapacity)) : this.MinCapacity * this.m_entity.Prototype.BuffersMultiplier.Value);
      }

      BufferStrategy IInputBufferPriorityProvider.GetInputPriority(
        IProductBuffer buffer,
        Quantity pendingQuantity)
      {
        return new BufferStrategy(this.m_entity.GeneralPriority, new Quantity?(buffer.Capacity / 2));
      }

      public override void Destroy()
      {
        this.UnregisterFromLogistics(false);
        base.Destroy();
      }

      public static void Serialize(Machine.MachineInputBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Machine.MachineInputBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Machine.MachineInputBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        Machine.Serialize(this.m_entity, writer);
        writer.WriteBool(this.m_isRegisteredToLogistics);
      }

      public static Machine.MachineInputBuffer Deserialize(BlobReader reader)
      {
        Machine.MachineInputBuffer machineInputBuffer;
        if (reader.TryStartClassDeserialization<Machine.MachineInputBuffer>(out machineInputBuffer))
          reader.EnqueueDataDeserialization((object) machineInputBuffer, Machine.MachineInputBuffer.s_deserializeDataDelayedAction);
        return machineInputBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<Machine.MachineInputBuffer>(this, "m_entity", (object) Machine.Deserialize(reader));
        this.m_isRegisteredToLogistics = reader.ReadBool();
      }

      static MachineInputBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Machine.MachineInputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        Machine.MachineInputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public sealed class MachineOutputBuffer : GlobalOutputBuffer, IOutputBufferPriorityProvider
    {
      /// <summary>
      /// Index of port used as last output port, -1 at start. This is done as previous port index instead of next
      /// port index, so that after load the index is automatically checked before usage without perf impact.
      /// </summary>
      private int m_prevUsedPort;
      [DoNotSave(0, null)]
      public Quantity MinCapacity;
      [DoNotSaveCreateNewOnLoad(null, 0)]
      private readonly Lyst<IoPort> m_ports;
      /// <summary>
      /// Vast majority of recipes have a single output port per product. There is few exceptions
      /// such as cooling towers. So having single port as first class citizen improves
      /// performance (~5-10% of sim update for machine will full outputs).
      /// </summary>
      [DoNotSave(0, null)]
      private IoPortData m_portFast;
      [DoNotSave(0, null)]
      private LystStruct<IoPortData> m_portsFast;
      private bool m_madeRegistrationToLogistics;
      private readonly Machine m_entity;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      [DoNotSave(0, null)]
      public bool IsAnyPortConnected { get; private set; }

      public bool IsNotUsedByCurrentRecipes => this.MinCapacity.IsZero;

      /// <summary>
      /// IsRegisteredToLogistics can be false when m_madeRegistrationToLogistics is true.
      /// Reason can be products that can't be transported. But we store that we made the
      /// request to make sure we don't apply for it every single time.
      /// </summary>
      public bool IsRegisteredToLogistics { get; private set; }

      public MachineOutputBuffer(
        Quantity capacity,
        ProductProto product,
        IProductsManager productsManager,
        Machine entity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_prevUsedPort = -1;
        this.m_ports = new Lyst<IoPort>();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product, productsManager, 15, (IEntity) entity);
        this.m_entity = entity;
      }

      private void recreatePorts()
      {
        if (this.m_ports.Count == 1)
        {
          this.m_portFast = new IoPortData(this.m_ports.First);
          this.m_portsFast.Clear();
        }
        else if (this.m_ports.Count > 1)
        {
          this.m_portFast = IoPortData.Invalid;
          this.m_portsFast.Clear();
          foreach (IoPort port in this.m_ports)
            this.m_portsFast.Add(new IoPortData(port));
        }
        else
        {
          this.m_portFast = IoPortData.Invalid;
          this.m_portsFast.Clear();
        }
      }

      public void AddPort(IoPort port)
      {
        if (this.m_ports.Contains(port))
          return;
        this.m_ports.Add(port);
        this.IsAnyPortConnected |= port.IsConnected;
        this.recreatePorts();
        this.m_entity.m_allConnectedOutputsBuffersEmpty = false;
      }

      public void ClearAllPorts()
      {
        this.m_ports.Clear();
        this.m_portFast = new IoPortData();
        this.m_portsFast.Clear();
        this.IsAnyPortConnected = false;
      }

      public void Send()
      {
        if (this.m_portFast.IsValid)
        {
          sendToPort(this.m_portFast);
        }
        else
        {
          if (!this.m_portsFast.IsNotEmpty)
            return;
          for (int index = 0; index < this.m_portsFast.Count && this.Quantity.IsPositive; ++index)
          {
            this.m_prevUsedPort = (this.m_prevUsedPort + 1) % this.m_portsFast.Count;
            sendToPort(this.m_portsFast[this.m_prevUsedPort]);
          }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void sendToPort(IoPortData port)
        {
          Quantity quantity = this.Quantity - port.SendAsMuchAs(new ProductQuantity(this.Product, this.Quantity));
          if (!quantity.IsPositive)
            return;
          this.RemoveExactly(quantity);
          if (this.m_entity.LogisticsOutputMode != EntityLogisticsMode.Auto || !this.IsRegisteredToLogistics)
            return;
          this.UnregisterFromLogistics(true);
        }
      }

      public void RegisterBufferToLogistics(bool updateCapacity)
      {
        if (this.IsRegisteredToLogistics)
          return;
        this.IsRegisteredToLogistics = this.m_entity.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this.m_entity, (IProductBuffer) this, (IOutputBufferPriorityProvider) this, true);
        this.m_madeRegistrationToLogistics = true;
        if (!updateCapacity)
          return;
        this.UpdateCapacity();
      }

      public void UnregisterFromLogistics(bool updateCapacity)
      {
        if (!this.IsRegisteredToLogistics)
          return;
        this.m_entity.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) this);
        this.IsRegisteredToLogistics = false;
        this.m_madeRegistrationToLogistics = false;
        if (!updateCapacity)
          return;
        this.UpdateCapacity();
      }

      public void UpdateCapacity()
      {
        if (this.MinCapacity.IsZero)
          return;
        this.ForceNewCapacityTo(!this.m_entity.Prototype.BuffersMultiplier.HasValue ? (!this.IsRegisteredToLogistics ? this.MinCapacity * 2 : (8 * this.MinCapacity).Max(2 * TruckCaps.SmallTruckCapacity)) : this.MinCapacity * this.m_entity.Prototype.BuffersMultiplier.Value);
      }

      public void OnSomePortConnectionChanged(IoPort port)
      {
        if (!this.m_ports.Contains(port))
          return;
        this.recreatePorts();
        bool flag = this.m_ports.Any<IoPort>((Predicate<IoPort>) (x => x.IsConnected));
        if (flag == this.IsAnyPortConnected)
          return;
        this.IsAnyPortConnected = flag;
        if (this.IsAnyPortConnected)
        {
          if (!this.Quantity.IsPositive)
            return;
          this.m_entity.m_allConnectedOutputsBuffersEmpty = false;
        }
        else
        {
          if (this.m_entity.LogisticsOutputMode != EntityLogisticsMode.Auto || this.m_madeRegistrationToLogistics)
            return;
          this.RegisterBufferToLogistics(true);
        }
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        if (diff.IsPositive)
          this.m_entity.m_allConnectedOutputsBuffersEmpty = false;
        else
          this.m_entity.m_hasNoNeedToSearchForRecipe = false;
        base.OnQuantityChanged(diff);
      }

      BufferStrategy IOutputBufferPriorityProvider.GetOutputPriority(OutputPriorityRequest request)
      {
        return new BufferStrategy(this.m_entity.GeneralPriority, new Quantity?(request.Buffer.Capacity / 2));
      }

      public override void Destroy()
      {
        this.m_ports.Clear();
        this.UnregisterFromLogistics(false);
        base.Destroy();
      }

      public static void Serialize(Machine.MachineOutputBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Machine.MachineOutputBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Machine.MachineOutputBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteBool(this.IsRegisteredToLogistics);
        Machine.Serialize(this.m_entity, writer);
        writer.WriteBool(this.m_madeRegistrationToLogistics);
        writer.WriteInt(this.m_prevUsedPort);
      }

      public static Machine.MachineOutputBuffer Deserialize(BlobReader reader)
      {
        Machine.MachineOutputBuffer machineOutputBuffer;
        if (reader.TryStartClassDeserialization<Machine.MachineOutputBuffer>(out machineOutputBuffer))
          reader.EnqueueDataDeserialization((object) machineOutputBuffer, Machine.MachineOutputBuffer.s_deserializeDataDelayedAction);
        return machineOutputBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        this.IsRegisteredToLogistics = reader.ReadBool();
        reader.SetField<Machine.MachineOutputBuffer>(this, "m_entity", (object) Machine.Deserialize(reader));
        this.m_madeRegistrationToLogistics = reader.ReadBool();
        reader.SetField<Machine.MachineOutputBuffer>(this, "m_ports", (object) new Lyst<IoPort>());
        this.m_prevUsedPort = reader.ReadInt();
      }

      static MachineOutputBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Machine.MachineOutputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        Machine.MachineOutputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }

    protected readonly struct RecipeWrapper
    {
      public readonly RecipeProto Recipe;
      public readonly Percent MinUtilization;
      public readonly int QuantitiesGcd;
      public readonly ImmutableArray<Machine.RecipeProductQuantity> AllInputs;
      public readonly ImmutableArray<Machine.RecipeProductQuantity> OutputsAtStart;
      public readonly ImmutableArray<Machine.RecipeProductQuantity> OutputsAtEnd;
      public readonly ImmutableArray<RecipeProto> RecipesWithSameOutputs;

      public RecipeWrapper(
        RecipeProto recipe,
        Machine entity,
        ImmutableArray<RecipeProto> recipesWithSameOutputs)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Recipe = recipe;
        this.MinUtilization = recipe.MinUtilization;
        this.QuantitiesGcd = recipe.QuantitiesGcd;
        this.AllInputs = recipe.AllInputs.Map<Machine.RecipeProductQuantity>((Func<RecipeInput, Machine.RecipeProductQuantity>) (x => new Machine.RecipeProductQuantity(recipe, x.Quantity, resolveInputBuffer(x.Product))));
        this.OutputsAtStart = recipe.OutputsAtStart.Map<Machine.RecipeProductQuantity>((Func<RecipeOutput, Machine.RecipeProductQuantity>) (x => new Machine.RecipeProductQuantity(recipe, x.Quantity, resolveOutputBuffer(x.Product))));
        this.OutputsAtEnd = recipe.OutputsAtEnd.Map<Machine.RecipeProductQuantity>((Func<RecipeOutput, Machine.RecipeProductQuantity>) (x => new Machine.RecipeProductQuantity(recipe, x.Quantity, resolveOutputBuffer(x.Product))));
        this.RecipesWithSameOutputs = recipesWithSameOutputs;

        IProductBuffer resolveInputBuffer(ProductProto product)
        {
          if (product.Type == VirtualProductProto.ProductType)
            return entity.m_virtualBuffersMap.GetBuffer(product, (IEntity) entity).Value;
          Machine.MachineInputBuffer result;
          entity.tryGetInputBuffer(product, out result);
          Assert.That<Machine.MachineInputBuffer>(result).IsNotNull<Machine.MachineInputBuffer>();
          return (IProductBuffer) result;
        }

        IProductBuffer resolveOutputBuffer(ProductProto product)
        {
          if (product.Type == VirtualProductProto.ProductType)
            return entity.m_virtualBuffersMap.GetBuffer(product, (IEntity) entity).Value;
          Machine.MachineOutputBuffer result;
          entity.tryGetOutputBuffer(product, out result);
          Assert.That<Machine.MachineOutputBuffer>(result).IsNotNull<Machine.MachineOutputBuffer>();
          return (IProductBuffer) result;
        }
      }

      [Pure]
      public bool CanRemoveFromInputs()
      {
        foreach (Machine.RecipeProductQuantity allInput in this.AllInputs)
        {
          if (!allInput.Buffer.CanRemove(allInput.Quantity))
            return false;
        }
        return true;
      }

      [Pure]
      public bool CanStoreToOutputs()
      {
        foreach (Machine.RecipeProductQuantity recipeProductQuantity in this.OutputsAtStart)
        {
          if (!recipeProductQuantity.Buffer.CanStore(recipeProductQuantity.Quantity))
            return false;
        }
        foreach (Machine.RecipeProductQuantity recipeProductQuantity in this.OutputsAtEnd)
        {
          if (!recipeProductQuantity.Buffer.CanStore(recipeProductQuantity.Quantity))
            return false;
        }
        return true;
      }
    }

    protected readonly struct RecipeProductQuantity
    {
      public readonly Quantity Quantity;
      public readonly Quantity BaseFraction;
      public readonly IProductBuffer Buffer;

      public RecipeProductQuantity(RecipeProto recipe, Quantity quantity, IProductBuffer buffer)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Quantity = quantity;
        this.Buffer = buffer;
        this.BaseFraction = this.Quantity / recipe.QuantitiesGcd;
      }
    }
  }
}
