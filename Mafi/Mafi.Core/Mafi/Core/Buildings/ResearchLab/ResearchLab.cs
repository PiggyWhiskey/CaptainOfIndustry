// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.ResearchLab.ResearchLab
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.ResearchLab
{
  [GenerateSerializer(false, null, 0)]
  public class ResearchLab : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IMaintainedEntity,
    IUnityConsumingEntity,
    IEntityWithEmission,
    IAnimatedEntity,
    IElectricityConsumingEntity,
    IEntityWithPorts,
    IEntityWithLogisticsControl,
    IComputingConsumingEntity,
    IEntityWithSimUpdate,
    IInputBufferPriorityProvider,
    IOutputBufferPriorityProvider
  {
    private ResearchLabProto m_proto;
    private readonly VirtualBuffersMap m_buffersMap;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly IComputingConsumer m_computingConsumer;
    private readonly ResearchManager m_researchManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IProductsManager m_productsManager;
    private readonly ICalendar m_calendar;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    [RenamedInVersion(140, "UnityConsumerInternal")]
    private Mafi.Core.Population.UnityConsumer m_unityConsumer;
    private bool m_wasOperationalLastSim;
    /// <summary>
    /// Set used to gather products provided by connected IVehicleOutputEntities.
    /// </summary>
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<ProductProto> m_providedProductsCache;
    private EntityNotificator m_cannotResearchAdvancedTech;
    private EntityNotificator m_missingInputProductsNotif;
    private Option<GlobalInputBuffer> m_inputBuffer;
    private Option<GlobalOutputBuffer> m_outputBuffer;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private TickTimer m_researchStepLeft;
    private readonly TechnologyProto m_recyclingTech;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public ResearchLabProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public Mafi.Core.Buildings.ResearchLab.ResearchLab.State CurrentState { get; private set; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public Fix32 ResearchPointsPerMonth
    {
      get
      {
        return this.Prototype.StepsPerRecipe * (1.Months().Ticks / this.Prototype.DurationForRecipe.Ticks);
      }
    }

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    Computing IComputingConsumingEntity.ComputingRequired => this.Prototype.ComputingConsumed;

    public Option<IComputingConsumerReadonly> ComputingConsumer
    {
      get => this.m_computingConsumer.CreateOption<IComputingConsumerReadonly>();
    }

    /// <summary>
    /// Only one research process can be assigned at the same time.
    /// </summary>
    public Option<ResearchNodeProto> CurrentResearch { get; private set; }

    public Upoints MaxMonthlyUnityConsumed => this.Prototype.UnityMonthlyCost;

    public Upoints MonthlyUnityConsumed
    {
      get => !this.CurrentResearch.HasValue ? Upoints.Zero : this.Prototype.UnityMonthlyCost;
    }

    public Proto.ID UpointsCategoryId => this.Prototype.UpointsCategory.Id;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    public bool NotEnoughPower { get; private set; }

    public Option<Mafi.Core.Population.UnityConsumer> UnityConsumer
    {
      get => (Option<Mafi.Core.Population.UnityConsumer>) this.m_unityConsumer;
    }

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionIntensity.HasValue ? new float?() : new float?(this.m_wasOperationalLastSim ? (float) this.Prototype.EmissionIntensity.Value : 0.0f);
      }
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => this.CurrentState != Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsBlockedOnAdvancedResearch => this.m_cannotResearchAdvancedTech.IsActive;

    public Option<IProductBuffer> InputBuffer => this.m_inputBuffer.As<IProductBuffer>();

    public Option<IProductBuffer> OutputBuffer => this.m_outputBuffer.As<IProductBuffer>();

    public ResearchLab(
      EntityId id,
      ResearchLabProto proto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap virtualBuffers,
      ResearchManager researchManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IVehiclesManager vehiclesManager,
      IProductsManager productsManager,
      INotificationsManager notifManager,
      ICalendar calendar,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IUnityConsumerFactory unityConsumerFactory,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_providedProductsCache = new Set<ProductProto>();
      this.m_researchStepLeft = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumer((IComputingConsumingEntity) this);
      this.m_buffersMap = virtualBuffers;
      this.m_researchManager = researchManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_vehiclesManager = vehiclesManager;
      this.m_productsManager = productsManager;
      this.m_calendar = calendar;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_recyclingTech = protosDb.GetOrThrow<TechnologyProto>(IdsCore.Technology.Recycling);
      this.Upgrader = upgraderFactory.CreateInstance<ResearchLabProto, Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, this.Prototype);
      this.m_cannotResearchAdvancedTech = notifManager.CreateNotificatorFor(IdsCore.Notifications.LabCannotResearchHigherTech);
      this.m_missingInputProductsNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.LabMissingInputProducts);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) this, (IOutputBufferPriorityProvider) this, vehicleBuffersRegistry);
      this.reCreateBuffersIfNeeded();
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.m_unityConsumer = unityConsumerFactory.CreateConsumer((IUnityConsumingEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      if (this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_recyclingTech))
        return;
      this.m_unlockedProtosDb.OnUnlockedSetChanged.Add<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, new Action(this.onProtoUnlocked));
    }

    private void onProtoUnlocked()
    {
      if (!this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_recyclingTech))
        return;
      this.reCreateBuffersIfNeeded();
      this.m_unlockedProtosDb.OnUnlockedSetChanged.Remove<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, new Action(this.onProtoUnlocked));
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || this.m_inputBuffer.IsNone || (Proto) this.m_inputBuffer.Value.Product != (Proto) pq.Product)
        return pq.Quantity;
      GlobalInputBuffer bufferReceived = this.m_inputBuffer.Value;
      this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) bufferReceived);
      return bufferReceived.StoreAsMuchAs(pq);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateResearch();
      if (this.m_unityConsumer.MonthlyUnity != this.MonthlyUnityConsumed)
        this.m_unityConsumer.RefreshUnityConsumed();
      this.m_cannotResearchAdvancedTech.NotifyIff(this.CurrentState == Mafi.Core.Buildings.ResearchLab.ResearchLab.State.ResearchTooDifficult, (IEntity) this);
      this.m_missingInputProductsNotif.NotifyIff(this.CurrentState == Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingInput, (IEntity) this);
      if (this.CurrentState == Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working)
        this.AnimationStatesProvider.Step(Percent.Hundred, Percent.Zero);
      else
        this.AnimationStatesProvider.Pause();
      if (!this.IsEnabled)
        return;
      this.sentOutputs();
    }

    private Mafi.Core.Buildings.ResearchLab.ResearchLab.State updateResearch()
    {
      Option<ResearchNodeProto> currentResearch;
      if (this.m_researchManager.CurrentResearch.IsNone)
      {
        currentResearch = this.CurrentResearch;
        if (currentResearch.HasValue)
          this.removeResearch();
      }
      Mafi.Core.Buildings.ResearchLab.ResearchLab.State state = this.updateState();
      this.m_wasOperationalLastSim = state == Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working;
      if (this.m_researchManager.CurrentResearch.IsNone)
        state = Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Idle;
      if (state != Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working || checkChangedResearch())
        return state;
      if (this.m_researchStepLeft.Decrement())
      {
        this.m_electricityConsumer.ConsumeAndAssert();
        this.m_computingConsumer.ConsumeAndAssert();
        return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working;
      }
      if (this.m_researchStepLeft.Ticks.Ticks == 0)
        this.m_researchManager.ReportResearchStepsDone(this.Prototype.StepsPerRecipe);
      if (this.m_researchManager.CurrentResearch.IsNone)
      {
        currentResearch = this.CurrentResearch;
        if (currentResearch.HasValue)
        {
          this.removeResearch();
          state = Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Idle;
          return state;
        }
      }
      return checkChangedResearch() ? state : startResearchStep();

      bool checkChangedResearch()
      {
        ResearchNodeProto proto = this.m_researchManager.CurrentResearch.Value.Proto;
        if (!(proto != this.CurrentResearch))
          return false;
        if (this.CurrentResearch.HasValue)
          this.removeResearch();
        if (!this.Prototype.CanResearchIfRequiredLabIs(this.m_researchManager.CurrentResearch.Value.LabRequired))
        {
          state = Mafi.Core.Buildings.ResearchLab.ResearchLab.State.ResearchTooDifficult;
          return true;
        }
        if (this.m_researchStepLeft.Ticks.Ticks == 0)
        {
          state = startResearchStep();
          if (state != Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working)
            return true;
        }
        this.CurrentResearch = (Option<ResearchNodeProto>) proto;
        return true;
      }

      Mafi.Core.Buildings.ResearchLab.ResearchLab.State startResearchStep()
      {
        if (this.m_inputBuffer.HasValue)
        {
          GlobalInputBuffer globalInputBuffer = this.m_inputBuffer.Value;
          if (!globalInputBuffer.CanRemove(this.Prototype.ConsumedPerRecipe.Quantity))
            return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingInput;
          Quantity quantity1 = globalInputBuffer.RemoveAsMuchAs(this.Prototype.ConsumedPerRecipe.Quantity);
          Quantity quantity2 = !this.m_outputBuffer.HasValue ? Quantity.Zero : this.Prototype.ProducedPerRecipe.Quantity - this.m_outputBuffer.Value.StoreAsMuchAs(this.Prototype.ProducedPerRecipe.Quantity);
          if (quantity2.IsPositive)
            this.m_productsManager.ReportProductsTransformation(new ProductQuantity(this.Prototype.ConsumedPerRecipe.Product, quantity1), new ProductQuantity(this.Prototype.ProducedPerRecipe.Product, quantity2), DestroyReason.Research, CreateReason.Research);
          else
            this.m_productsManager.ProductDestroyed(globalInputBuffer.Product, quantity1, DestroyReason.Research);
        }
        this.m_researchStepLeft.Start(this.Prototype.DurationForRecipe);
        return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working;
      }
    }

    private void sentOutputs()
    {
      if (this.m_outputBuffer.IsNone || this.m_outputBuffer.Value.IsEmpty)
        return;
      GlobalOutputBuffer globalOutputBuffer = this.m_outputBuffer.Value;
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
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

    private Mafi.Core.Buildings.ResearchLab.ResearchLab.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Paused : Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Broken;
      if (!this.m_unityConsumer.CanWork())
        return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughUpoints;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingWorkers;
      if (!this.m_electricityConsumer.CanConsume())
        return Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughPower;
      return !this.m_computingConsumer.CanConsume() ? Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughComputing : Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working;
    }

    private void removeResearch()
    {
      Assert.That<Option<ResearchNodeProto>>(this.CurrentResearch).HasValue<ResearchNodeProto>();
      this.CurrentResearch = (Option<ResearchNodeProto>) Option.None;
    }

    protected override void OnDestroy()
    {
      if (this.m_inputBuffer.HasValue)
      {
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_inputBuffer.Value);
        this.m_inputBuffer = (Option<GlobalInputBuffer>) Option.None;
      }
      if (this.m_outputBuffer.HasValue)
      {
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_outputBuffer.Value);
        this.m_outputBuffer = (Option<GlobalOutputBuffer>) Option.None;
      }
      if (!this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_recyclingTech))
        this.m_unlockedProtosDb.OnUnlockedSetChanged.Remove<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, new Action(this.onProtoUnlocked));
      base.OnDestroy();
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        this.reCreateBuffersIfNeeded();
      }
    }

    private void reCreateBuffersIfNeeded()
    {
      if (this.m_inputBuffer.HasValue && (Proto) this.Prototype.ConsumedPerRecipe.Product != (Proto) this.m_inputBuffer.Value.Product)
      {
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_inputBuffer.Value);
        this.m_autoLogisticsHelper.RemoveInputBuffer((IProductBuffer) this.m_inputBuffer.Value);
        this.m_inputBuffer = (Option<GlobalInputBuffer>) Option.None;
      }
      if (this.Prototype.ConsumedPerRecipe.IsNotEmpty && (Proto) this.m_inputBuffer.ValueOrNull?.Product != (Proto) this.Prototype.ConsumedPerRecipe.Product)
      {
        this.m_inputBuffer = (Option<GlobalInputBuffer>) new GlobalInputBuffer(this.Prototype.InputBufferCapacity, this.Prototype.ConsumedPerRecipe.Product, this.Context.ProductsManager, 5, (IEntity) this);
        this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) this.m_inputBuffer.Value);
      }
      if (this.m_outputBuffer.HasValue && (Proto) this.Prototype.ProducedPerRecipe.Product != (Proto) this.m_outputBuffer.Value.Product)
      {
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_outputBuffer.Value);
        this.m_autoLogisticsHelper.RemoveOutputBuffer((IProductBuffer) this.m_outputBuffer.Value);
        this.m_outputBuffer = (Option<GlobalOutputBuffer>) Option.None;
      }
      if (!this.Prototype.ProducedPerRecipe.IsNotEmpty || !((Proto) this.m_outputBuffer.ValueOrNull?.Product != (Proto) this.Prototype.ProducedPerRecipe.Product) || !this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_recyclingTech))
        return;
      this.m_outputBuffer = (Option<GlobalOutputBuffer>) new GlobalOutputBuffer(this.Prototype.OutputBufferCapacity, this.Prototype.ProducedPerRecipe.Product, this.m_productsManager, 15, (IEntity) this);
      this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) this.m_outputBuffer.Value);
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return new BufferStrategy(this.GeneralPriority, new Quantity?(buffer.Capacity / 2));
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return new BufferStrategy(this.GeneralPriority, new Quantity?(request.Buffer.Capacity / 2));
    }

    public bool CanDisableLogisticsInput => this.m_inputBuffer.HasValue;

    public bool CanDisableLogisticsOutput => this.m_outputBuffer.HasValue;

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

    public static void Serialize(Mafi.Core.Buildings.ResearchLab.ResearchLab value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.ResearchLab.ResearchLab>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.ResearchLab.ResearchLab.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      Option<ResearchNodeProto>.Serialize(this.CurrentResearch, writer);
      writer.WriteInt((int) this.CurrentState);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      VirtualBuffersMap.Serialize(this.m_buffersMap, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      EntityNotificator.Serialize(this.m_cannotResearchAdvancedTech, writer);
      writer.WriteGeneric<IComputingConsumer>(this.m_computingConsumer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      Option<GlobalInputBuffer>.Serialize(this.m_inputBuffer, writer);
      EntityNotificator.Serialize(this.m_missingInputProductsNotif, writer);
      Option<GlobalOutputBuffer>.Serialize(this.m_outputBuffer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<ResearchLabProto>(this.m_proto);
      writer.WriteGeneric<TechnologyProto>(this.m_recyclingTech);
      ResearchManager.Serialize(this.m_researchManager, writer);
      TickTimer.Serialize(this.m_researchStepLeft, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
      writer.WriteBool(this.m_wasOperationalLastSim);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteBool(this.NotEnoughPower);
      Mafi.Core.Population.UnityConsumer.Serialize(this.m_unityConsumer, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Mafi.Core.Buildings.ResearchLab.ResearchLab Deserialize(BlobReader reader)
    {
      Mafi.Core.Buildings.ResearchLab.ResearchLab researchLab;
      if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.ResearchLab.ResearchLab>(out researchLab))
        reader.EnqueueDataDeserialization((object) researchLab, Mafi.Core.Buildings.ResearchLab.ResearchLab.s_deserializeDataDelayedAction);
      return researchLab;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentResearch = Option<ResearchNodeProto>.Deserialize(reader);
      this.CurrentState = (Mafi.Core.Buildings.ResearchLab.ResearchLab.State) reader.ReadInt();
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_buffersMap", (object) VirtualBuffersMap.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_cannotResearchAdvancedTech = EntityNotificator.Deserialize(reader);
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_computingConsumer", (object) reader.ReadGenericAs<IComputingConsumer>());
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_inputBuffer = Option<GlobalInputBuffer>.Deserialize(reader);
      this.m_missingInputProductsNotif = EntityNotificator.Deserialize(reader);
      this.m_outputBuffer = Option<GlobalOutputBuffer>.Deserialize(reader);
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<ResearchLabProto>();
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_providedProductsCache", (object) new Set<ProductProto>());
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_recyclingTech", (object) reader.ReadGenericAs<TechnologyProto>());
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_researchManager", (object) ResearchManager.Deserialize(reader));
      this.m_researchStepLeft = TickTimer.Deserialize(reader);
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<Mafi.Core.Buildings.ResearchLab.ResearchLab>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      this.m_wasOperationalLastSim = reader.ReadBool();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.NotEnoughPower = reader.ReadBool();
      this.m_unityConsumer = Mafi.Core.Population.UnityConsumer.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static ResearchLab()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Core.Buildings.ResearchLab.ResearchLab.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Mafi.Core.Buildings.ResearchLab.ResearchLab.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      Idle,
      MissingInput,
      MissingWorkers,
      NotEnoughUpoints,
      NotEnoughPower,
      NotEnoughComputing,
      ResearchTooDifficult,
    }
  }
}
