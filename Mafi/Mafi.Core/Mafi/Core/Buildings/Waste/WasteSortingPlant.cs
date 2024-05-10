// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Waste.WasteSortingPlant
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
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Waste
{
  [GenerateSerializer(false, null, 0)]
  public class WasteSortingPlant : 
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
    IEntityWithLogisticsControl,
    IMaintainedEntity,
    IEntityWithPorts,
    IElectricityConsumingEntity,
    IAnimatedEntity,
    IEntityWithSimUpdate
  {
    private WasteSortingPlantProto m_proto;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly Dict<ProductProto, GlobalInputBuffer> m_inputBuffers;
    private readonly ICalendar m_calendar;
    private readonly IProductsManager m_productsManager;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private ProductQuantity m_pendingForRecycle;
    private readonly TickTimer m_recyclingTimer;
    private bool m_tempBuffersFull;
    [DoNotSaveCreateNewOnLoad("new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>()", 0)]
    private readonly Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> m_recyclingResultsCache;
    private Dict<ProductProto, WasteSortingPlant.RecycledProductData> m_recyclingOutputs;
    [DoNotSave(0, null)]
    private Lyst<IProductBuffer> m_outputBuffers;
    private int m_lastOutputIndex;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public WasteSortingPlantProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => this.CurrentState != WasteSortingPlant.State.Working;

    public override bool CanBePaused => true;

    public WasteSortingPlant.State CurrentState { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    public IReadOnlyCollection<IProductBuffer> InputBuffers
    {
      get => (IReadOnlyCollection<IProductBuffer>) this.m_inputBuffers.Values;
    }

    public IIndexable<IProductBuffer> OutputBuffers
    {
      get => (IIndexable<IProductBuffer>) this.m_outputBuffers;
    }

    public WasteSortingPlant(
      EntityId id,
      WasteSortingPlantProto proto,
      TileTransform transform,
      EntityContext context,
      ICalendar calendar,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_inputBuffers = new Dict<ProductProto, GlobalInputBuffer>();
      this.m_pendingForRecycle = ProductQuantity.None;
      this.m_recyclingTimer = new TickTimer();
      this.m_recyclingResultsCache = new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>();
      this.m_recyclingOutputs = new Dict<ProductProto, WasteSortingPlant.RecycledProductData>();
      this.m_outputBuffers = new Lyst<IProductBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_calendar = calendar;
      this.m_productsManager = productsManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Upgrader = upgraderFactory.CreateInstance<WasteSortingPlantProto, WasteSortingPlant>(this, this.Prototype);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      foreach (ProductQuantity supportedInput in this.Prototype.SupportedInputs)
      {
        GlobalInputBuffer buffer = new GlobalInputBuffer(this.Prototype.InputBuffersCapacity, supportedInput.Product, this.Context.ProductsManager, 5, (IEntity) this);
        this.m_inputBuffers.Add(supportedInput.Product, buffer);
        this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) buffer);
      }
      foreach (ProductProto product in context.ProtosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.IsRecyclable)))
        this.addRecycledDataFor(product);
      calendar.NewMonth.Add<WasteSortingPlant>(this, new Action(this.onNewMonth));
      this.AnimationStatesProvider.Start(this.Prototype.Duration);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_outputBuffers = this.m_recyclingOutputs.Values.Select<WasteSortingPlant.RecycledProductData, IProductBuffer>((Func<WasteSortingPlant.RecycledProductData, IProductBuffer>) (x => (IProductBuffer) x.Buffer)).ToLyst<IProductBuffer>();
    }

    private void onNewMonth()
    {
      foreach (WasteSortingPlant.RecycledProductData recycledProductData in this.m_recyclingOutputs.Values)
        recycledProductData.OnNewMonth();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      this.AnimationStatesProvider.Step(Percent.Hundred, this.m_recyclingTimer.PercentFinished);
      if (!this.IsEnabled)
        return;
      this.sentOutputs();
    }

    private WasteSortingPlant.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? WasteSortingPlant.State.Paused : WasteSortingPlant.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return WasteSortingPlant.State.MissingWorkers;
      return !this.m_electricityConsumer.TryConsume() ? WasteSortingPlant.State.NotEnoughPower : this.simStepRecycling();
    }

    private WasteSortingPlant.State simStepRecycling()
    {
      if (this.m_tempBuffersFull)
      {
        this.pushResultsToBuffers();
        return WasteSortingPlant.State.FullOutput;
      }
      if (this.m_pendingForRecycle.IsEmpty)
      {
        foreach (ProductQuantity supportedInput in this.Prototype.SupportedInputs)
        {
          ProductBuffer inputBuffer = (ProductBuffer) this.m_inputBuffers[supportedInput.Product];
          if (inputBuffer.CanRemove(supportedInput.Quantity))
          {
            inputBuffer.RemoveExactly(supportedInput.Quantity);
            this.m_pendingForRecycle = supportedInput;
            this.m_recyclingTimer.Start(this.Prototype.Duration);
            return WasteSortingPlant.State.Working;
          }
        }
        return WasteSortingPlant.State.MissingInput;
      }
      if (this.m_recyclingTimer.Decrement())
        return WasteSortingPlant.State.Working;
      this.m_productsManager.DestroyProductReturnRemovedSourceProducts(this.m_pendingForRecycle.Product, this.m_pendingForRecycle.Quantity, DestroyReason.General, this.m_recyclingResultsCache);
      this.m_pendingForRecycle = ProductQuantity.None;
      foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_recyclingResultsCache)
      {
        if (keyValuePair.Key.IsRecyclable)
        {
          WasteSortingPlant.RecycledProductData recycledProductData;
          if (!this.m_recyclingOutputs.TryGetValue(keyValuePair.Key, out recycledProductData))
            recycledProductData = this.addRecycledDataFor(keyValuePair.Key);
          recycledProductData.Add(keyValuePair.Value);
        }
      }
      this.m_recyclingResultsCache.Clear();
      this.pushResultsToBuffers();
      return WasteSortingPlant.State.Working;
    }

    private WasteSortingPlant.RecycledProductData addRecycledDataFor(ProductProto product)
    {
      GlobalOutputBuffer buffer = new GlobalOutputBuffer(this.Prototype.OutputBuffersCapacity, product, this.m_productsManager, 15, (IEntity) this);
      WasteSortingPlant.RecycledProductData recycledProductData = new WasteSortingPlant.RecycledProductData(buffer);
      this.m_recyclingOutputs.Add(buffer.Product, recycledProductData);
      this.m_outputBuffers.Add((IProductBuffer) recycledProductData.Buffer);
      return recycledProductData;
    }

    private void pushResultsToBuffers()
    {
      this.m_tempBuffersFull = false;
      foreach (KeyValuePair<ProductProto, WasteSortingPlant.RecycledProductData> recyclingOutput in this.m_recyclingOutputs)
      {
        ProductProto key = recyclingOutput.Key;
        WasteSortingPlant.RecycledProductData recycledProductData = recyclingOutput.Value;
        QuantityLarge integerPart = recycledProductData.PartialBuffer.IntegerPart;
        Quantity quantityClamped = integerPart.ToQuantityClamped();
        if (!quantityClamped.IsNotPositive)
        {
          Quantity quantity = quantityClamped - recycledProductData.Buffer.StoreAsMuchAs(quantityClamped);
          if (quantity.IsPositive)
          {
            this.m_productsManager.ProductCreated(key, quantity, CreateReason.Recycled);
            recycledProductData.PartialBuffer -= new PartialQuantityLarge(quantity);
          }
          integerPart = recycledProductData.PartialBuffer.IntegerPart;
          if (integerPart.IsPositive)
            this.m_tempBuffersFull = true;
        }
      }
    }

    private void sentOutputs()
    {
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
      {
        for (int index = 0; index < this.m_recyclingOutputs.Count; ++index)
        {
          this.m_lastOutputIndex = (this.m_lastOutputIndex + 1) % this.m_recyclingOutputs.Count;
          IProductBuffer outputBuffer = this.m_outputBuffers[this.m_lastOutputIndex];
          Quantity quantity1 = outputBuffer.Quantity;
          Quantity quantity2 = quantity1 - connectedOutputPort.SendAsMuchAs(outputBuffer.Product.WithQuantity(quantity1));
          if (quantity2.IsPositive)
          {
            outputBuffer.RemoveExactly(quantity2);
            this.m_autoLogisticsHelper.OnProductSentToPort(outputBuffer);
            break;
          }
        }
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (!this.IsEnabled)
        return pq.Quantity;
      GlobalInputBuffer bufferReceived;
      if (!this.m_inputBuffers.TryGetValue(pq.Product, out bufferReceived))
      {
        Log.Warning(string.Format("Unexpected input product at index '{0}' on sorting plant", (object) sourcePort.PortIndex));
        return pq.Quantity;
      }
      this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) bufferReceived);
      return bufferReceived.StoreAsMuchAs(pq);
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewMonth.Remove<WasteSortingPlant>(this, new Action(this.onNewMonth));
      foreach (IProductBuffer buffer in this.m_inputBuffers.Values)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(buffer);
      foreach (WasteSortingPlant.RecycledProductData recycledProductData in this.m_recyclingOutputs.Values)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) recycledProductData.Buffer);
      if (this.m_pendingForRecycle.IsNotEmpty)
        this.Context.AssetTransactionManager.StoreClearedProduct(this.m_pendingForRecycle);
      base.OnDestroy();
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => false;

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

    public PartialQuantityLarge GetRecycledLastMonth(ProductProto product)
    {
      WasteSortingPlant.RecycledProductData recycledProductData;
      return this.m_recyclingOutputs.TryGetValue(product, out recycledProductData) ? recycledProductData.RecycledLastMonth : PartialQuantityLarge.Zero;
    }

    public static void Serialize(WasteSortingPlant value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WasteSortingPlant>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WasteSortingPlant.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteInt((int) this.CurrentState);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      Dict<ProductProto, GlobalInputBuffer>.Serialize(this.m_inputBuffers, writer);
      writer.WriteInt(this.m_lastOutputIndex);
      ProductQuantity.Serialize(this.m_pendingForRecycle, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<WasteSortingPlantProto>(this.m_proto);
      Dict<ProductProto, WasteSortingPlant.RecycledProductData>.Serialize(this.m_recyclingOutputs, writer);
      TickTimer.Serialize(this.m_recyclingTimer, writer);
      writer.WriteBool(this.m_tempBuffersFull);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static WasteSortingPlant Deserialize(BlobReader reader)
    {
      WasteSortingPlant wasteSortingPlant;
      if (reader.TryStartClassDeserialization<WasteSortingPlant>(out wasteSortingPlant))
        reader.EnqueueDataDeserialization((object) wasteSortingPlant, WasteSortingPlant.s_deserializeDataDelayedAction);
      return wasteSortingPlant;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentState = (WasteSortingPlant.State) reader.ReadInt();
      reader.SetField<WasteSortingPlant>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<WasteSortingPlant>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<WasteSortingPlant>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<WasteSortingPlant>(this, "m_inputBuffers", (object) Dict<ProductProto, GlobalInputBuffer>.Deserialize(reader));
      this.m_lastOutputIndex = reader.ReadInt();
      this.m_pendingForRecycle = ProductQuantity.Deserialize(reader);
      reader.SetField<WasteSortingPlant>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<WasteSortingPlantProto>();
      this.m_recyclingOutputs = Dict<ProductProto, WasteSortingPlant.RecycledProductData>.Deserialize(reader);
      reader.SetField<WasteSortingPlant>(this, "m_recyclingResultsCache", (object) new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>());
      reader.SetField<WasteSortingPlant>(this, "m_recyclingTimer", (object) TickTimer.Deserialize(reader));
      this.m_tempBuffersFull = reader.ReadBool();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<WasteSortingPlant>(this, "initSelf", InitPriority.Normal);
    }

    static WasteSortingPlant()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WasteSortingPlant.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WasteSortingPlant.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
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

    [GenerateSerializer(false, null, 0)]
    private class RecycledProductData
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public GlobalOutputBuffer Buffer { get; private set; }

      public PartialQuantityLarge PartialBuffer { get; set; }

      public PartialQuantityLarge RecycledLastMonth { get; set; }

      public PartialQuantityLarge RecycledThisMonth { get; set; }

      public RecycledProductData(GlobalOutputBuffer buffer)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Buffer = buffer;
      }

      public void Add(PartialQuantityLarge toAdd)
      {
        this.PartialBuffer += toAdd;
        this.RecycledThisMonth += toAdd;
      }

      public void OnNewMonth()
      {
        this.RecycledLastMonth = this.RecycledThisMonth;
        this.RecycledThisMonth = PartialQuantityLarge.Zero;
      }

      public static void Serialize(WasteSortingPlant.RecycledProductData value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WasteSortingPlant.RecycledProductData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WasteSortingPlant.RecycledProductData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        GlobalOutputBuffer.Serialize(this.Buffer, writer);
        PartialQuantityLarge.Serialize(this.PartialBuffer, writer);
        PartialQuantityLarge.Serialize(this.RecycledLastMonth, writer);
        PartialQuantityLarge.Serialize(this.RecycledThisMonth, writer);
      }

      public static WasteSortingPlant.RecycledProductData Deserialize(BlobReader reader)
      {
        WasteSortingPlant.RecycledProductData recycledProductData;
        if (reader.TryStartClassDeserialization<WasteSortingPlant.RecycledProductData>(out recycledProductData))
          reader.EnqueueDataDeserialization((object) recycledProductData, WasteSortingPlant.RecycledProductData.s_deserializeDataDelayedAction);
        return recycledProductData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Buffer = GlobalOutputBuffer.Deserialize(reader);
        this.PartialBuffer = PartialQuantityLarge.Deserialize(reader);
        this.RecycledLastMonth = PartialQuantityLarge.Deserialize(reader);
        this.RecycledThisMonth = PartialQuantityLarge.Deserialize(reader);
      }

      static RecycledProductData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WasteSortingPlant.RecycledProductData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WasteSortingPlant.RecycledProductData) obj).SerializeData(writer));
        WasteSortingPlant.RecycledProductData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WasteSortingPlant.RecycledProductData) obj).DeserializeData(reader));
      }
    }
  }
}
