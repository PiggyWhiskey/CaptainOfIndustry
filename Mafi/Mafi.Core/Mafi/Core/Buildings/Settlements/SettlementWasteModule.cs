// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementWasteModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementWasteModule : 
    StorageBase,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithSimUpdate
  {
    public readonly SettlementWasteModuleProto Prototype;
    private readonly IProductsManager m_productsManager;
    private EntityGeneralPriorityProvider m_outputPriorityProvider;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    protected override bool IoDisabled => base.IoDisabled || !this.IsOperational;

    protected override bool IsOperational
    {
      get => base.IsOperational && ((IEntityWithWorkers) this).HasWorkersCached;
    }

    public SettlementWasteModule.State CurrentState { get; private set; }

    public override bool CanBePaused => true;

    public override LogisticsControl LogisticsOutputControl => LogisticsControl.Enabled;

    public Quantity StoredQuantity => this.Buffer.Value.Quantity;

    private bool IsNotWorking => this.CurrentState != SettlementWasteModule.State.Working;

    public SettlementWasteModule(
      EntityId id,
      SettlementWasteModuleProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageBaseProto) proto, transform, context, simLoopEvents, vehicleBuffersRegistry);
      this.Prototype = proto;
      this.m_productsManager = this.Context.ProductsManager;
      this.m_outputPriorityProvider = new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this);
      Assert.That<Option<LogisticsBuffer>>(this.Buffer).IsNone<LogisticsBuffer>();
      Assert.That<bool>(this.TryAssignProduct(proto.ProductAccepted)).IsTrue("Failed to assign water product to harvester.");
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      return (LogisticsBuffer) new GlobalLogisticsOutputBuffer(this.Prototype.Capacity, product, this.Context.ProductsManager, 10, (IEntity) this, true);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      bool isOperational = this.IsOperational;
      this.CurrentState = this.updateState();
      if (isOperational != this.IsOperational)
      {
        this.UpdateLogisticsInputReg();
        this.UpdateLogisticsOutputReg();
      }
      if (!this.IsEnabled)
        return;
      this.SendAllOutputsIfCan();
    }

    public override bool IsProductSupported(ProductProto product)
    {
      return (Proto) product == (Proto) this.Prototype.ProductAccepted;
    }

    private SettlementWasteModule.State updateState()
    {
      if (!this.IsEnabled)
        return SettlementWasteModule.State.Paused;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return SettlementWasteModule.State.MissingWorkers;
      return this.Buffer.Value.UsableCapacity.IsZero ? SettlementWasteModule.State.FullOutput : SettlementWasteModule.State.Working;
    }

    public Quantity GetCapacityLeft()
    {
      return this.IsNotWorking ? Quantity.Zero : this.Buffer.Value.UsableCapacity;
    }

    public Quantity StoreAsMuchAsAndReport(Quantity landfill)
    {
      if (this.IsNotWorking)
        return landfill;
      Quantity quantity = landfill - this.Buffer.Value.StoreAsMuchAs(landfill);
      if (quantity.IsPositive)
        this.m_productsManager.ProductCreated(this.Prototype.ProductAccepted, quantity, CreateReason.Settlement);
      return landfill - quantity;
    }

    public Quantity StoreAsMuchAs(Quantity landfill)
    {
      return this.IsNotWorking ? landfill : this.Buffer.Value.StoreAsMuchAs(landfill);
    }

    protected override Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IInputBufferPriorityProvider>) Option.None;
    }

    protected override Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IOutputBufferPriorityProvider>) (IOutputBufferPriorityProvider) this.m_outputPriorityProvider;
    }

    public static void Serialize(SettlementWasteModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementWasteModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementWasteModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      EntityGeneralPriorityProvider.Serialize(this.m_outputPriorityProvider, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<SettlementWasteModuleProto>(this.Prototype);
    }

    public static SettlementWasteModule Deserialize(BlobReader reader)
    {
      SettlementWasteModule settlementWasteModule;
      if (reader.TryStartClassDeserialization<SettlementWasteModule>(out settlementWasteModule))
        reader.EnqueueDataDeserialization((object) settlementWasteModule, SettlementWasteModule.s_deserializeDataDelayedAction);
      return settlementWasteModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (SettlementWasteModule.State) reader.ReadInt();
      this.m_outputPriorityProvider = EntityGeneralPriorityProvider.Deserialize(reader);
      reader.SetField<SettlementWasteModule>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<SettlementWasteModule>(this, "Prototype", (object) reader.ReadGenericAs<SettlementWasteModuleProto>());
    }

    static SettlementWasteModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementWasteModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementWasteModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Working,
      MissingWorkers,
      FullOutput,
    }
  }
}
