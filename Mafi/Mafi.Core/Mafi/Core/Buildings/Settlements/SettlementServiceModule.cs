// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementServiceModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementServiceModule : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    ISettlementServiceModule,
    IAnimatedEntity,
    IEntityWithEmission,
    IEntityWithLogisticsControl,
    IElectricityConsumingEntity,
    IMaintainedEntity,
    IEntityWithSimUpdate,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    public readonly SettlementModuleProto Prototype;
    private readonly IElectricityConsumer m_electricityConsumer;
    private bool m_didSatisfyPopsInLastUpdate;
    private readonly SettlementsManager m_settlementManager;
    private readonly ICalendar m_calendar;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private PartialQuantity m_unconsumedInput;
    private PartialQuantity m_unconsumedOutput;
    private PopNeed m_popNeed;
    public Quantity TotalInputLastMonth;
    public Quantity TotalInputThisMonth;
    public Quantity TotalOutputLastMonth;
    public Quantity TotalOutputThisMonth;
    [DoNotSave(0, null)]
    private Percent m_consumptionMult;
    private readonly ProductBuffer m_inputBuffer;
    private readonly Option<ProductBuffer> m_outputBuffer;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private readonly IProductsManager m_productsManager;
    private int m_lastUsedPortIndex;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => this.CurrentState != SettlementServiceModule.State.Working;

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionIntensity.HasValue ? new float?() : new float?(this.CurrentState == SettlementServiceModule.State.Working ? (float) this.Prototype.EmissionIntensity.Value : 0.0f);
      }
    }

    public SettlementServiceModule.State CurrentState { get; private set; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => this.Prototype.OutputProduct.HasValue;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode
    {
      get => this.m_autoLogisticsHelper.LogisticsOutputMode;
    }

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    public override bool IsCargoAffectedByGeneralPriority => true;

    public Option<Mafi.Core.Buildings.Settlements.Settlement> Settlement { get; private set; }

    public PopNeedProto ProvidedNeed => this.Prototype.PopsNeed;

    public ProductQuantity InputProduct => this.m_inputBuffer.ProductQuantity;

    public Quantity InputProductCapacity => this.m_inputBuffer.Capacity;

    public ProductQuantity? OutputProduct => this.m_outputBuffer.ValueOrNull?.ProductQuantity;

    public Quantity OutputProductCapacity
    {
      get
      {
        ProductBuffer valueOrNull = this.m_outputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : __nonvirtual (valueOrNull.Capacity);
      }
    }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public SettlementServiceModule(
      EntityId id,
      SettlementModuleProto proto,
      TileTransform transform,
      EntityContext context,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      SettlementsManager settlementManager,
      ICalendar calendar,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_unconsumedInput = PartialQuantity.Zero;
      this.m_unconsumedOutput = PartialQuantity.Zero;
      this.m_lastUsedPortIndex = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_settlementManager = settlementManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.m_calendar = calendar;
      this.m_productsManager = this.Context.ProductsManager;
      this.updateProperties();
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      if (proto.StayConnectedToLogisticsByDefault)
        this.m_autoLogisticsHelper.SetLogisticsInputMode(EntityLogisticsMode.On);
      this.m_inputBuffer = (ProductBuffer) new GlobalInputBuffer(this.Prototype.InputBufferCapacity, this.Prototype.InputProduct, this.Context.ProductsManager, 5, (IEntity) this);
      this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) this.m_inputBuffer);
      if (this.Prototype.OutputProduct.HasValue)
      {
        this.m_outputBuffer = (Option<ProductBuffer>) (ProductBuffer) new GlobalOutputBuffer(this.Prototype.OutputBufferCapacity, this.Prototype.OutputProduct.Value, this.Context.ProductsManager, 15, (IEntity) this);
        this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) this.m_outputBuffer.Value);
      }
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      calendar.NewMonth.Add<SettlementServiceModule>(this, new Action(this.onNewMonth));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void updateProperties()
    {
      this.m_consumptionMult = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.SettlementConsumptionMultiplier);
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (!this.IsEnabled || !((Proto) pq.Product == (Proto) this.m_inputBuffer.Product))
        return pq.Quantity;
      this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) this.m_inputBuffer);
      return this.m_inputBuffer.StoreAsMuchAs(pq);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      if (this.CurrentState != SettlementServiceModule.State.Working && this.CurrentState != SettlementServiceModule.State.FullOutput)
      {
        this.AnimationStatesProvider.Pause();
      }
      else
      {
        if (!this.Prototype.AnimateOnlyWhenServicingPops || this.m_didSatisfyPopsInLastUpdate)
          this.AnimationStatesProvider.Step(Percent.Hundred, Percent.Zero);
        else
          this.AnimationStatesProvider.Pause();
        if (!this.m_outputBuffer.HasValue)
          return;
        for (int index = 0; index < this.ConnectedOutputPorts.Length && this.m_outputBuffer.Value.Quantity.IsPositive; ++index)
        {
          this.m_lastUsedPortIndex = (this.m_lastUsedPortIndex + 1) % this.ConnectedOutputPorts.Length;
          if (this.ConnectedOutputPorts[this.m_lastUsedPortIndex].SendAsMuchAsFromBuffer((IProductBuffer) this.m_outputBuffer.Value).IsPositive)
            this.m_autoLogisticsHelper.OnProductSentToPort((IProductBuffer) this.m_outputBuffer.Value);
        }
      }
    }

    private SettlementServiceModule.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? SettlementServiceModule.State.Paused : SettlementServiceModule.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return SettlementServiceModule.State.MissingWorkers;
      if (!this.m_electricityConsumer.TryConsume())
        return SettlementServiceModule.State.NotEnoughPower;
      Quantity quantity = this.m_inputBuffer.Quantity;
      if (quantity.IsZero)
        return SettlementServiceModule.State.MissingInput;
      if (this.OutputProduct.HasValue)
      {
        quantity = this.m_outputBuffer.Value.UsableCapacity;
        if (quantity.IsZero)
          return SettlementServiceModule.State.FullOutput;
      }
      return SettlementServiceModule.State.Working;
    }

    public void SetSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      Assert.That<Option<Mafi.Core.Buildings.Settlements.Settlement>>(this.Settlement).IsNone<Mafi.Core.Buildings.Settlements.Settlement>();
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
      this.m_popNeed = this.Settlement.Value.AllNeeds.Single((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) this.Prototype.PopsNeed));
    }

    public void ReplaceSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      Assert.That<Option<Mafi.Core.Buildings.Settlements.Settlement>>(this.Settlement).HasValue<Mafi.Core.Buildings.Settlements.Settlement>();
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
      this.m_popNeed = this.Settlement.Value.AllNeeds.Single((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) this.Prototype.PopsNeed));
    }

    public PartialQuantity GetTotalInputNeedPerMonth()
    {
      Mafi.Core.Buildings.Settlements.Settlement valueOrNull = this.Settlement.ValueOrNull;
      int population = valueOrNull != null ? valueOrNull.Population : 0;
      return this.Prototype.GetConsumedPerPopPerMonth(this.m_popNeed.IsFoodNeed ? population + this.m_settlementManager.NumberOfHomeless : population, this.m_consumptionMult).ScaledBy(this.m_popNeed.ConsumptionMultiplier);
    }

    public Fix32 GetMonthsOfSupply()
    {
      PartialQuantity inputNeedPerMonth = this.GetTotalInputNeedPerMonth();
      Quantity quantity = this.m_inputBuffer.Quantity;
      return inputNeedPerMonth.IsZero ? (Fix32) 0 : quantity.Value / inputNeedPerMonth.Value;
    }

    public PartialQuantity GetTotalOutputNeedPerMonth()
    {
      return !this.Prototype.OutputProduct.HasValue ? PartialQuantity.Zero : this.Prototype.GetProducedPerPopPerMonth(this.Settlement.Value.Population, this.m_consumptionMult).ScaledBy(this.m_popNeed.ConsumptionMultiplier);
    }

    /// <summary>
    /// Returns the amount of pops it did not manage to satisfy.
    /// </summary>
    public int TrySatisfyNeedOnNewDay(int popsToSatisfy)
    {
      this.m_didSatisfyPopsInLastUpdate = false;
      if (this.CurrentState != SettlementServiceModule.State.Working)
        return popsToSatisfy;
      if (popsToSatisfy == 0)
        return 0;
      PartialQuantity partialQuantity1 = new PartialQuantity(this.m_popNeed.ConsumptionMultiplier.Apply((this.Prototype.GetConsumedPerPopPerMonth(popsToSatisfy, this.m_consumptionMult) / 30).Value));
      Assert.That<PartialQuantity>(partialQuantity1).IsNotNegative("Consumption too low!");
      PartialQuantity partialQuantity2 = this.m_unconsumedInput + partialQuantity1;
      PartialQuantity partialQuantity3 = PartialQuantity.Zero;
      if (this.Prototype.OutputProduct.HasValue)
      {
        PartialQuantity partialQuantity4 = new PartialQuantity(this.m_popNeed.ConsumptionMultiplier.Apply((this.Prototype.GetProducedPerPopPerMonth(popsToSatisfy, this.m_consumptionMult) / 30).Value));
        Assert.That<PartialQuantity>(partialQuantity4).IsNotNegative("Production too low!");
        partialQuantity3 = this.m_unconsumedOutput + partialQuantity4;
      }
      Percent percent;
      if (partialQuantity2.IsZero)
      {
        percent = Percent.Hundred;
      }
      else
      {
        Quantity quantity = this.m_inputBuffer.Quantity;
        percent = quantity >= partialQuantity2.IntegerPart ? Percent.Hundred : Percent.FromRatio(quantity.Value, partialQuantity2.IntegerPart.Value);
      }
      Percent rhs;
      if (partialQuantity3.IsZero)
      {
        rhs = Percent.Hundred;
      }
      else
      {
        Quantity usableCapacity = this.m_outputBuffer.Value.UsableCapacity;
        rhs = usableCapacity >= partialQuantity3.IntegerPart ? Percent.Hundred : Percent.FromRatio(usableCapacity.Value, partialQuantity3.IntegerPart.Value);
      }
      Percent scale = percent.Min(rhs);
      Assert.That<Percent>(scale).IsWithin0To100PercIncl();
      if (scale.IsNotPositive)
        return popsToSatisfy;
      partialQuantity3 = partialQuantity3.ScaledBy(scale);
      partialQuantity2 = partialQuantity2.ScaledBy(scale);
      this.m_unconsumedInput = partialQuantity2.FractionalPart;
      Quantity integerPart1 = partialQuantity2.IntegerPart;
      if (integerPart1.IsPositive)
      {
        bool flag = !this.OutputProduct.HasValue;
        this.m_inputBuffer.RemoveExactly(integerPart1);
        if (flag)
          this.Settlement.Value.TransformProductIntoWaste(this.Prototype.InputProduct, integerPart1);
        else
          this.m_productsManager.ProductDestroyed(this.m_inputBuffer.Product, integerPart1, DestroyReason.Settlement);
        this.TotalInputThisMonth += integerPart1;
      }
      this.m_unconsumedOutput = partialQuantity3.FractionalPart;
      Quantity integerPart2 = partialQuantity3.IntegerPart;
      if (integerPart2.IsPositive)
      {
        Quantity quantity = integerPart2 - this.m_outputBuffer.Value.StoreAsMuchAs(integerPart2);
        this.m_productsManager.ProductCreated(this.m_outputBuffer.Value.Product, quantity, CreateReason.Settlement);
        Assert.That<Quantity>(quantity).IsEqualTo(integerPart2);
        this.TotalOutputThisMonth += quantity;
      }
      this.m_didSatisfyPopsInLastUpdate = true;
      return popsToSatisfy - scale.Apply(popsToSatisfy);
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    private void onNewMonth()
    {
      this.TotalInputLastMonth = this.TotalInputThisMonth;
      this.TotalInputThisMonth = Quantity.Zero;
      this.TotalOutputLastMonth = this.TotalOutputThisMonth;
      this.TotalOutputThisMonth = Quantity.Zero;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsEnabled)
        return;
      this.m_vehicleBuffersRegistry.ClearAndCancelAllJobs((IStaticEntity) this);
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewMonth.Remove<SettlementServiceModule>(this, new Action(this.onNewMonth));
      IAssetTransactionManager transactionManager = this.Context.AssetTransactionManager;
      transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_inputBuffer);
      if (this.m_outputBuffer.HasValue)
        transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_outputBuffer.Value);
      base.OnDestroy();
    }

    public static void Serialize(SettlementServiceModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementServiceModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementServiceModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteInt((int) this.CurrentState);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteBool(this.m_didSatisfyPopsInLastUpdate);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      ProductBuffer.Serialize(this.m_inputBuffer, writer);
      writer.WriteInt(this.m_lastUsedPortIndex);
      Option<ProductBuffer>.Serialize(this.m_outputBuffer, writer);
      PopNeed.Serialize(this.m_popNeed, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      SettlementsManager.Serialize(this.m_settlementManager, writer);
      PartialQuantity.Serialize(this.m_unconsumedInput, writer);
      PartialQuantity.Serialize(this.m_unconsumedOutput, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<SettlementModuleProto>(this.Prototype);
      Option<Mafi.Core.Buildings.Settlements.Settlement>.Serialize(this.Settlement, writer);
      Quantity.Serialize(this.TotalInputLastMonth, writer);
      Quantity.Serialize(this.TotalInputThisMonth, writer);
      Quantity.Serialize(this.TotalOutputLastMonth, writer);
      Quantity.Serialize(this.TotalOutputThisMonth, writer);
    }

    public static SettlementServiceModule Deserialize(BlobReader reader)
    {
      SettlementServiceModule settlementServiceModule;
      if (reader.TryStartClassDeserialization<SettlementServiceModule>(out settlementServiceModule))
        reader.EnqueueDataDeserialization((object) settlementServiceModule, SettlementServiceModule.s_deserializeDataDelayedAction);
      return settlementServiceModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentState = (SettlementServiceModule.State) reader.ReadInt();
      reader.SetField<SettlementServiceModule>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<SettlementServiceModule>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_didSatisfyPopsInLastUpdate = reader.ReadBool();
      reader.SetField<SettlementServiceModule>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<SettlementServiceModule>(this, "m_inputBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_lastUsedPortIndex = reader.ReadInt();
      reader.SetField<SettlementServiceModule>(this, "m_outputBuffer", (object) Option<ProductBuffer>.Deserialize(reader));
      this.m_popNeed = PopNeed.Deserialize(reader);
      reader.SetField<SettlementServiceModule>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<SettlementServiceModule>(this, "m_settlementManager", (object) SettlementsManager.Deserialize(reader));
      this.m_unconsumedInput = PartialQuantity.Deserialize(reader);
      this.m_unconsumedOutput = PartialQuantity.Deserialize(reader);
      reader.SetField<SettlementServiceModule>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.SetField<SettlementServiceModule>(this, "Prototype", (object) reader.ReadGenericAs<SettlementModuleProto>());
      this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.Deserialize(reader);
      this.TotalInputLastMonth = Quantity.Deserialize(reader);
      this.TotalInputThisMonth = Quantity.Deserialize(reader);
      this.TotalOutputLastMonth = Quantity.Deserialize(reader);
      this.TotalOutputThisMonth = Quantity.Deserialize(reader);
      reader.RegisterInitAfterLoad<SettlementServiceModule>(this, "updateProperties", InitPriority.Normal);
    }

    static SettlementServiceModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementServiceModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementServiceModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      MissingInput,
      MissingWorkers,
      NotEnoughPower,
      FullOutput,
    }
  }
}
