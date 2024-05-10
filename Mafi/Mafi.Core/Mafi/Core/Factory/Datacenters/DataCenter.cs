// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Datacenters
{
  [GenerateSerializer(false, null, 0)]
  public class DataCenter : 
    LayoutEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithWorkers,
    IElectricityConsumingEntity,
    IEntityWithSimUpdate,
    IComputingGenerator,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithPorts
  {
    public readonly DataCenterProto Prototype;
    private Duration m_recipeProductionDuration;
    private readonly Dict<ServerRackProto, int> m_racksCounts;
    private readonly IProductsManager m_productsManager;
    private readonly IAssetTransactionManager m_assetTransactions;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly IComputingManager m_computingManager;
    private readonly ProductBuffer m_coolantInBuffer;
    private readonly ProductBuffer m_coolantOutBuffer;
    private PartialQuantity m_coolantInTempBuffer;
    private PartialQuantity m_coolantOutTempBuffer;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_coolantOutPorts;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_coolantInPorts;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public DataCenter.State CurrentState { get; private set; }

    public override bool IsCargoAffectedByGeneralPriority => true;

    [DoNotSave(0, null)]
    public int RacksCount { get; private set; }

    [DoNotSave(0, null)]
    public Computing MaxComputingGenerationCapacity { get; private set; }

    [DoNotSave(0, null)]
    public Electricity PowerRequired { get; private set; }

    public IReadOnlyDictionary<ServerRackProto, int> ServersCounts
    {
      get => (IReadOnlyDictionary<ServerRackProto, int>) this.m_racksCounts;
    }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    /// <summary>Invoked after rack is added.</summary>
    public event Action<ServerRackProto> RackAdded;

    /// <summary>Invoked after rack is removed.</summary>
    public event Action<ServerRackProto> RackRemoved;

    bool IMaintainedEntity.IsIdleForMaintenance => this.RacksCount <= 0;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public MaintenanceCosts MaintenanceCosts { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public int WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IProductBufferReadOnly CoolantInBuffer
    {
      get => (IProductBufferReadOnly) this.m_coolantInBuffer;
    }

    public IProductBufferReadOnly CoolantOutBuffer
    {
      get => (IProductBufferReadOnly) this.m_coolantOutBuffer;
    }

    [DoNotSave(0, null)]
    public PartialQuantity CoolantInPerTick { get; private set; }

    [DoNotSave(0, null)]
    public PartialQuantity CoolantOutPerTick { get; private set; }

    public DataCenter(
      EntityId id,
      DataCenterProto datacenterProto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      IAssetTransactionManager assetTransactions,
      ProtosDb protosDb,
      IComputingManager computingManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_racksCounts = new Dict<ServerRackProto, int>();
      this.m_coolantInTempBuffer = PartialQuantity.Zero;
      this.m_coolantOutTempBuffer = PartialQuantity.Zero;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCoolantInPerTick\u003Ek__BackingField = PartialQuantity.Zero;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCoolantOutPerTick\u003Ek__BackingField = PartialQuantity.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) datacenterProto, transform, context);
      this.m_productsManager = productsManager;
      this.m_assetTransactions = assetTransactions;
      this.m_computingManager = computingManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Prototype = datacenterProto.CheckNotNull<DataCenterProto>();
      foreach (ServerRackProto key in protosDb.All<ServerRackProto>())
      {
        if (!this.m_racksCounts.ContainsKey(key))
          this.m_racksCounts.Add(key, 0);
      }
      this.m_coolantInBuffer = new ProductBuffer(this.Prototype.CoolantCapacity, this.Prototype.CoolantIn);
      this.m_coolantOutBuffer = new ProductBuffer(this.Prototype.CoolantCapacity, this.Prototype.CoolantOut);
      this.MaintenanceCosts = this.Prototype.Costs.Maintenance;
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.reInitPorts();
      this.rebuildRacksData(false);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.rebuildRacksData(true);

    private void reInitPorts()
    {
      this.m_coolantOutPorts = this.ConnectedOutputPorts.Filter((Predicate<IoPortData>) (x => x.AllowedProductType == this.m_coolantOutBuffer.Product.Type && this.Prototype.CoolantOutPorts.Contains(x.Name)));
      this.m_coolantInPorts = this.ConnectedOutputPorts.Filter((Predicate<IoPortData>) (x => x.AllowedProductType == this.m_coolantInBuffer.Product.Type && !this.Prototype.CoolantOutPorts.Contains(x.Name)));
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.reInitPorts();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      if (!this.IsEnabled)
        return;
      this.sendOutputs();
    }

    public Computing GenerateComputing()
    {
      return this.CurrentState == DataCenter.State.Working ? this.MaxComputingGenerationCapacity : Computing.Zero;
    }

    private void sendOutputs()
    {
      foreach (IoPortData coolantOutPort in this.m_coolantOutPorts)
      {
        if (this.m_coolantOutBuffer.IsNotEmpty)
        {
          Quantity quantity1 = this.m_coolantOutBuffer.Quantity;
          Quantity quantity2 = quantity1 - coolantOutPort.SendAsMuchAs(this.m_coolantOutBuffer.Product.WithQuantity(quantity1));
          if (quantity2.IsPositive)
            this.m_coolantOutBuffer.RemoveExactly(quantity2);
        }
      }
      foreach (IoPortData coolantInPort in this.m_coolantInPorts)
      {
        if (this.m_coolantInBuffer.PercentFull() > 10.Percent())
        {
          Quantity one = Quantity.One;
          Quantity quantity = one - coolantInPort.SendAsMuchAs(this.m_coolantInBuffer.Product.WithQuantity(one));
          if (quantity.IsPositive)
            this.m_coolantInBuffer.RemoveExactly(quantity);
        }
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled)
        return pq.Quantity;
      if ((Proto) pq.Product == (Proto) this.m_coolantInBuffer.Product)
      {
        if (this.Prototype.CoolantInPorts.Contains(sourcePort.Name))
          return this.m_coolantInBuffer.StoreAsMuchAs(pq);
      }
      else if ((Proto) pq.Product == (Proto) this.m_coolantOutBuffer.Product && this.Prototype.CoolantOutPorts.Contains(sourcePort.Name))
        return this.m_coolantOutBuffer.StoreAsMuchAs(pq);
      return pq.Quantity;
    }

    private DataCenter.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? DataCenter.State.Paused : DataCenter.State.Broken;
      if (this.RacksCount == 0)
        return DataCenter.State.NoRacks;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return DataCenter.State.NotEnoughWorkers;
      Quantity integerPart1 = this.m_coolantInTempBuffer.IntegerPart;
      if (integerPart1.IsPositive)
      {
        Quantity quantity = this.m_coolantInBuffer.RemoveAsMuchAs(this.m_coolantInTempBuffer.IntegerPart);
        if (quantity.IsPositive)
        {
          this.m_productsManager.ProductDestroyed(this.m_coolantInBuffer.Product, quantity, DestroyReason.General);
          this.m_coolantInTempBuffer -= quantity.AsPartial;
        }
      }
      integerPart1 = this.m_coolantOutTempBuffer.IntegerPart;
      if (integerPart1.IsPositive)
      {
        Quantity integerPart2 = this.m_coolantOutTempBuffer.IntegerPart;
        Quantity quantity = integerPart2 - this.m_coolantOutBuffer.StoreAsMuchAs(integerPart2);
        if (quantity.IsPositive)
        {
          this.m_productsManager.ProductCreated(this.m_coolantOutBuffer.Product, quantity, CreateReason.Produced);
          this.m_coolantOutTempBuffer -= quantity.AsPartial;
        }
      }
      integerPart1 = this.m_coolantInTempBuffer.IntegerPart;
      if (integerPart1.IsPositive || this.m_coolantInBuffer.IsEmpty)
        return DataCenter.State.NotEnoughCoolant;
      integerPart1 = this.m_coolantOutTempBuffer.IntegerPart;
      if (integerPart1.IsPositive)
        return DataCenter.State.FullOutput;
      if (!this.m_electricityConsumer.TryConsume())
        return DataCenter.State.NotEnoughElectricity;
      this.m_coolantInTempBuffer += this.CoolantInPerTick;
      this.m_coolantOutTempBuffer += this.CoolantOutPerTick;
      return DataCenter.State.Working;
    }

    public int GetNumberOfRacksFor(ServerRackProto proto) => this.m_racksCounts[proto];

    private bool canAddServerRack(ServerRackProto proto)
    {
      if (!this.m_racksCounts.ContainsKey(proto))
      {
        Assert.Fail("The given server rack is not supported in this data center.");
        return false;
      }
      return this.IsConstructed && this.RacksCount < this.Prototype.RacksCapacity;
    }

    public bool AddServerRack(ServerRackProto proto)
    {
      if (!this.canAddServerRack(proto))
        return false;
      ++this.m_racksCounts[proto];
      this.rebuildRacksData(false);
      Action<ServerRackProto> rackAdded = this.RackAdded;
      if (rackAdded != null)
        rackAdded(proto);
      return true;
    }

    public bool TryAddServerRack(ServerRackProto rackProto)
    {
      if (!this.canAddServerRack(rackProto) || !this.m_assetTransactions.TryRemoveProduct(rackProto.ProductToAddThis, new DestroyReason?(DestroyReason.General)))
        return false;
      Assert.That<bool>(this.AddServerRack(rackProto)).IsTrue();
      return true;
    }

    public bool TryRemoveServerRack(ServerRackProto rackProto)
    {
      int num;
      if (!this.m_racksCounts.TryGetValue(rackProto, out num) || num == 0)
        return false;
      this.m_racksCounts[rackProto] = num - 1;
      this.m_assetTransactions.StoreProduct(rackProto.ProductToAddThis, new CreateReason?(CreateReason.Deconstruction));
      this.rebuildRacksData(false);
      Action<ServerRackProto> rackRemoved = this.RackRemoved;
      if (rackRemoved != null)
        rackRemoved(rackProto);
      return true;
    }

    public IEnumerable<KeyValuePair<ServerRackProto, int>> GetRackCounts()
    {
      return (IEnumerable<KeyValuePair<ServerRackProto, int>>) this.m_racksCounts;
    }

    protected override void OnDestroy()
    {
      foreach (KeyValuePair<ServerRackProto, int> racksCount in this.m_racksCounts)
      {
        ProductQuantity productToRemoveThis = racksCount.Key.ProductToRemoveThis;
        int num = racksCount.Value;
        if (num > 0)
          this.m_assetTransactions.StoreProduct(productToRemoveThis.WithNewQuantity(productToRemoveThis.Quantity * num), new CreateReason?(CreateReason.Deconstruction));
      }
      this.m_assetTransactions.ClearAndDestroyBuffer((IProductBuffer) this.m_coolantInBuffer);
      this.m_assetTransactions.ClearAndDestroyBuffer((IProductBuffer) this.m_coolantOutBuffer);
      base.OnDestroy();
    }

    private void rebuildRacksData(bool isGameLoad)
    {
      int num1 = 0;
      Electricity zero1 = Electricity.Zero;
      Computing zero2 = Computing.Zero;
      PartialQuantity zero3 = PartialQuantity.Zero;
      PartialQuantity zero4 = PartialQuantity.Zero;
      PartialQuantity maintenancePerMonth = this.Prototype.Costs.Maintenance.MaintenancePerMonth;
      foreach (KeyValuePair<ServerRackProto, int> racksCount in this.m_racksCounts)
      {
        ServerRackProto key = racksCount.Key;
        int num2 = racksCount.Value;
        num1 += num2;
        zero1 += num2 * key.ConsumedPowerPerTick;
        zero2 += num2 * key.CreatedComputingPerTick;
        zero3 += num2 * key.CoolantInPerMonth;
        zero4 += num2 * key.CoolantOutPerMonth;
        maintenancePerMonth += num2 * key.Maintenance;
      }
      this.RacksCount = num1;
      this.PowerRequired = zero1;
      this.m_electricityConsumer.OnPowerRequiredChanged();
      this.MaxComputingGenerationCapacity = zero2;
      this.CoolantInPerTick = zero3 / 1.Months().Ticks;
      this.CoolantOutPerTick = zero4 / 1.Months().Ticks;
      if (isGameLoad)
        return;
      this.MaintenanceCosts = new MaintenanceCosts(this.Prototype.Costs.Maintenance.Product, maintenancePerMonth);
      this.Maintenance.RefreshMaintenanceCost();
    }

    public static void Serialize(DataCenter value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DataCenter>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DataCenter.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactions);
      writer.WriteGeneric<IComputingManager>(this.m_computingManager);
      ProductBuffer.Serialize(this.m_coolantInBuffer, writer);
      PartialQuantity.Serialize(this.m_coolantInTempBuffer, writer);
      ProductBuffer.Serialize(this.m_coolantOutBuffer, writer);
      PartialQuantity.Serialize(this.m_coolantOutTempBuffer, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Dict<ServerRackProto, int>.Serialize(this.m_racksCounts, writer);
      Duration.Serialize(this.m_recipeProductionDuration, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      MaintenanceCosts.Serialize(this.MaintenanceCosts, writer);
      writer.WriteGeneric<DataCenterProto>(this.Prototype);
    }

    public static DataCenter Deserialize(BlobReader reader)
    {
      DataCenter dataCenter;
      if (reader.TryStartClassDeserialization<DataCenter>(out dataCenter))
        reader.EnqueueDataDeserialization((object) dataCenter, DataCenter.s_deserializeDataDelayedAction);
      return dataCenter;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (DataCenter.State) reader.ReadInt();
      reader.SetField<DataCenter>(this, "m_assetTransactions", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<DataCenter>(this, "m_computingManager", (object) reader.ReadGenericAs<IComputingManager>());
      reader.SetField<DataCenter>(this, "m_coolantInBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_coolantInTempBuffer = PartialQuantity.Deserialize(reader);
      reader.SetField<DataCenter>(this, "m_coolantOutBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_coolantOutTempBuffer = PartialQuantity.Deserialize(reader);
      reader.SetField<DataCenter>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<DataCenter>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<DataCenter>(this, "m_racksCounts", (object) Dict<ServerRackProto, int>.Deserialize(reader));
      this.m_recipeProductionDuration = Duration.Deserialize(reader);
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.MaintenanceCosts = MaintenanceCosts.Deserialize(reader);
      reader.SetField<DataCenter>(this, "Prototype", (object) reader.ReadGenericAs<DataCenterProto>());
      reader.RegisterInitAfterLoad<DataCenter>(this, "initSelf", InitPriority.Normal);
    }

    static DataCenter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DataCenter.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      DataCenter.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Working,
      NoRacks,
      Paused,
      Broken,
      NotEnoughWorkers,
      NotEnoughElectricity,
      NotEnoughCoolant,
      FullOutput,
    }
  }
}
