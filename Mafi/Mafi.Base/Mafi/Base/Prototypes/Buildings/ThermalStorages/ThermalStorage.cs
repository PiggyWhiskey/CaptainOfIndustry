// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStorage
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings.ThermalStorages
{
  [GenerateSerializer(false, null, 0)]
  public sealed class ThermalStorage : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig,
    IElectricityConsumingEntity,
    IEntityWithAlertBelow,
    IEntityWithStorageAlert
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Percent NOTIFICATION_HYSTERESIS;
    private ThermalStorageProto m_proto;
    private readonly IProductsManager m_productsManager;
    private Fix32 m_heatLossAccumulator;
    private Option<ProductProto> m_assignedProductToSave;
    private Quantity m_productToOutput;
    private Quantity m_depletedProductToOutput;
    [DoNotSave(0, null)]
    private IoPortData m_productOutputPort;
    [DoNotSave(0, null)]
    private IoPortData m_depletedProductOutputPort;
    private readonly IElectricityConsumer m_electricityConsumer;
    private EntityNotificatorWithProtoParam m_supplyToLowNotif;

    public static void Serialize(ThermalStorage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ThermalStorage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ThermalStorage.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.AlertWhenBelow, writer);
      writer.WriteBool(this.AlertWhenBelowEnabled);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteInt(this.HeatStored);
      Option<ProductProto>.Serialize(this.m_assignedProductToSave, writer);
      Quantity.Serialize(this.m_depletedProductToOutput, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      Fix32.Serialize(this.m_heatLossAccumulator, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Quantity.Serialize(this.m_productToOutput, writer);
      writer.WriteGeneric<ThermalStorageProto>(this.m_proto);
      EntityNotificatorWithProtoParam.Serialize(this.m_supplyToLowNotif, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
    }

    public static ThermalStorage Deserialize(BlobReader reader)
    {
      ThermalStorage thermalStorage;
      if (reader.TryStartClassDeserialization<ThermalStorage>(out thermalStorage))
        reader.EnqueueDataDeserialization((object) thermalStorage, ThermalStorage.s_deserializeDataDelayedAction);
      return thermalStorage;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AlertWhenBelow = Percent.Deserialize(reader);
      this.AlertWhenBelowEnabled = reader.ReadBool();
      this.CurrentState = (ThermalStorage.State) reader.ReadInt();
      this.HeatStored = reader.ReadInt();
      this.m_assignedProductToSave = Option<ProductProto>.Deserialize(reader);
      this.m_depletedProductToOutput = Quantity.Deserialize(reader);
      reader.SetField<ThermalStorage>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_heatLossAccumulator = Fix32.Deserialize(reader);
      reader.SetField<ThermalStorage>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_productToOutput = Quantity.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<ThermalStorageProto>();
      this.m_supplyToLowNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.RegisterInitAfterLoad<ThermalStorage>(this, "initSelf", InitPriority.High);
    }

    [DoNotSave(0, null)]
    public ThermalStorageProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => this.HeatStored <= 0;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public ThermalStorage.State CurrentState { get; private set; }

    public Percent PercentFull
    {
      get
      {
        return this.HeatCapacity > 0 ? Percent.FromRatio(this.HeatStored, this.HeatCapacity) : Percent.Zero;
      }
    }

    private int HeatCapacityLeft => (this.HeatCapacity - this.HeatStored).Max(0);

    [DoNotSave(0, null)]
    public int HeatCapacity { get; private set; }

    public int HeatStored { get; private set; }

    public bool AreAlertsAvailable => this.AssignedProduct.HasValue;

    public bool AlertWhenBelowEnabled { get; set; }

    public Percent AlertWhenBelow { get; private set; }

    [DoNotSave(0, null)]
    public ThermalStorageProto.ProductData? AssignedProduct { get; private set; }

    public Electricity PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.CreateOption<IElectricityConsumerReadonly>();
    }

    public ThermalStorage(
      EntityId id,
      ThermalStorageProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CAlertWhenBelow\u003Ek__BackingField = Percent.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_productsManager = productsManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_supplyToLowNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.StorageSupplyTooLow);
      this.reInitPorts();
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf()
    {
      if (!this.m_assignedProductToSave.HasValue)
        return;
      this.assignProductInternal(this.m_assignedProductToSave.Value);
    }

    private void reInitPorts()
    {
      this.m_productOutputPort = this.ConnectedOutputPorts.AsEnumerable().FirstOrDefault<IoPortData>((Func<IoPortData, bool>) (x => (int) x.Name != (int) this.Prototype.DepletedProductPort));
      this.m_depletedProductOutputPort = this.ConnectedOutputPorts.AsEnumerable().FirstOrDefault<IoPortData>((Func<IoPortData, bool>) (x => (int) x.Name == (int) this.Prototype.DepletedProductPort));
      if (!this.m_depletedProductOutputPort.IsValid)
        return;
      Assert.That<ProductType>(this.m_depletedProductOutputPort.AllowedProductType).IsEqualTo<ProductType>(this.Prototype.DepletedProduct.Type);
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.reInitPorts();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      if (this.AssignedProduct.HasValue && this.IsEnabled)
      {
        Percent percent = this.AlertWhenBelow;
        if (this.m_supplyToLowNotif.IsActive)
          percent = (percent + ThermalStorage.NOTIFICATION_HYSTERESIS).Min(Percent.Hundred);
        this.m_supplyToLowNotif.NotifyIff((Proto) this.Prototype.HeatProduct, this.AlertWhenBelowEnabled && this.PercentFull <= percent, (IEntity) this);
      }
      else
        this.m_supplyToLowNotif.Deactivate((IEntity) this);
      if (this.CurrentState != ThermalStorage.State.Working && this.HeatStored > 0)
      {
        this.m_heatLossAccumulator += this.HeatCapacity.ScaledByToFix32(this.Prototype.HeatLossPerMonthIfNotOperating) / Duration.OneMonth.Ticks;
        if (this.m_heatLossAccumulator.IntegerPart > 0)
        {
          this.HeatStored = (this.HeatStored - this.m_heatLossAccumulator.IntegerPart).Max(0);
          this.m_heatLossAccumulator -= (Fix32) this.m_heatLossAccumulator.IntegerPart;
        }
      }
      this.sendOutputs();
    }

    private ThermalStorage.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? ThermalStorage.State.Paused : ThermalStorage.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return ThermalStorage.State.NotEnoughWorkers;
      return this.HeatStored > 0 && !this.m_electricityConsumer.TryConsume() ? ThermalStorage.State.NotEnoughPower : ThermalStorage.State.Working;
    }

    private void sendOutputs()
    {
      if (this.m_productOutputPort.IsValid && this.m_productToOutput.IsPositive)
      {
        ThermalStorageProto.ProductData? assignedProduct = this.AssignedProduct;
        if (assignedProduct.HasValue)
        {
          assignedProduct = this.AssignedProduct;
          ThermalStorageProto.ProductData productData = assignedProduct.Value;
          Quantity quantity = this.m_productToOutput - this.m_productOutputPort.SendAsMuchAs(productData.Product.WithQuantity(this.m_productToOutput));
          if (quantity.IsPositive)
          {
            this.m_productToOutput -= quantity;
            this.m_productsManager.ProductCreated(productData.Product, quantity, CreateReason.General);
          }
        }
      }
      if (!this.m_depletedProductOutputPort.IsValid || !this.m_depletedProductToOutput.IsPositive)
        return;
      Quantity quantity1 = this.m_depletedProductToOutput - this.m_depletedProductOutputPort.SendAsMuchAs(this.Prototype.DepletedProduct.WithQuantity(this.m_depletedProductToOutput));
      if (!quantity1.IsPositive)
        return;
      this.m_depletedProductToOutput -= quantity1;
      this.m_productsManager.ProductCreated(this.Prototype.DepletedProduct, quantity1, CreateReason.General);
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.CurrentState != ThermalStorage.State.Working || pq.Quantity.IsNotPositive)
        return pq.Quantity;
      ThermalStorageProto.ProductData? assignedProduct = this.AssignedProduct;
      if (!assignedProduct.HasValue && (Proto) pq.Product != (Proto) this.Prototype.ProductToCharge)
        this.AssignProduct(pq.Product);
      assignedProduct = this.AssignedProduct;
      if (!assignedProduct.HasValue)
        return pq.Quantity;
      assignedProduct = this.AssignedProduct;
      ThermalStorageProto.ProductData productData = assignedProduct.Value;
      if ((Proto) pq.Product == (Proto) productData.Product)
      {
        if ((int) sourcePort.Name == (int) this.Prototype.ProductToChargePort || this.m_depletedProductToOutput.IsNotZero)
          return pq.Quantity;
        Quantity quantity = (this.HeatCapacityLeft / productData.HeatCreatedPerOneInput).Quantity().Min(pq.Quantity);
        if (!quantity.IsPositive)
          return pq.Quantity;
        this.HeatStored += quantity.Value * productData.HeatCreatedPerOneInput;
        this.m_depletedProductToOutput = quantity;
        this.m_productsManager.ProductDestroyed(pq.Product, quantity, DestroyReason.General);
        return pq.Quantity - quantity;
      }
      if ((Proto) pq.Product == (Proto) this.Prototype.ProductToCharge && (int) sourcePort.Name == (int) this.Prototype.ProductToChargePort && !this.m_productToOutput.IsNotZero)
      {
        Quantity quantity = (this.HeatStored / productData.HeatConsumedPerOneOutput).Quantity().Min(pq.Quantity);
        if (quantity.IsPositive)
        {
          this.HeatStored -= quantity.Value * productData.HeatConsumedPerOneOutput;
          this.m_productToOutput = quantity;
          this.m_productsManager.ProductDestroyed(pq.Product, quantity, DestroyReason.General);
          return pq.Quantity - quantity;
        }
      }
      return pq.Quantity;
    }

    public void AssignProduct(ProductProto product) => this.assignProductInternal(product);

    private void assignProductInternal(ProductProto product)
    {
      if (this.AssignedProduct.HasValue && (Proto) this.AssignedProduct.Value.Product == (Proto) product)
        return;
      ThermalStorageProto.ProductData productData = this.Prototype.SupportedProducts.FirstOrDefault((Func<ThermalStorageProto.ProductData, bool>) (x => (Proto) x.Product == (Proto) product));
      if (productData.HeatCreatedPerOneInput <= 0)
        return;
      this.AssignedProduct = new ThermalStorageProto.ProductData?(productData);
      this.m_assignedProductToSave = (Option<ProductProto>) product;
      this.HeatCapacity = this.Prototype.Capacity.Value * this.AssignedProduct.Value.HeatConsumedPerOneOutput;
      this.HeatStored = this.HeatStored.Min(this.HeatCapacity);
      this.m_productToOutput = Quantity.Zero;
      this.m_depletedProductToOutput = Quantity.Zero;
    }

    private void clear()
    {
      this.AssignedProduct = new ThermalStorageProto.ProductData?();
      this.m_assignedProductToSave = Option<ProductProto>.None;
      this.m_productToOutput = Quantity.Zero;
      this.m_depletedProductToOutput = Quantity.Zero;
      this.HeatStored = 0;
      this.HeatCapacity = 0;
    }

    protected override void OnDestroy()
    {
      this.clear();
      base.OnDestroy();
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetStorageStoredProduct(this.m_assignedProductToSave);
      if (this.AlertWhenBelowEnabled)
        data.SetAlertWhenBelowEnabled(this.AlertWhenBelowEnabled);
      if (!(this.AlertWhenBelow != Percent.Zero))
        return;
      data.SetAlertWhenBelow(this.AlertWhenBelow);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      Option<ProductProto> storageStoredProduct = data.GetStorageStoredProduct();
      if (storageStoredProduct.HasValue && storageStoredProduct != this.m_assignedProductToSave)
        this.AssignProduct(storageStoredProduct.Value);
      this.AlertWhenBelowEnabled = data.GetAlertWhenBelowEnabled().GetValueOrDefault();
      this.AlertWhenBelow = data.GetAlertWhenBelow() ?? Percent.Zero;
    }

    public void SetAlertBelow(Percent below)
    {
      if (below < Percent.Zero)
        return;
      this.AlertWhenBelow = below;
    }

    public void SetAlertBelowEnabled(bool isEnabled) => this.AlertWhenBelowEnabled = isEnabled;

    public Option<IRecipeForUi> GetChargingRecipe()
    {
      if (!this.AssignedProduct.HasValue)
        return Option<IRecipeForUi>.None;
      foreach (IRecipeForUi recipe in this.Prototype.Recipes)
      {
        if ((Proto) recipe.AllUserVisibleInputs.First.Product == (Proto) this.AssignedProduct.Value.Product)
          return recipe.SomeOption<IRecipeForUi>();
      }
      return Option<IRecipeForUi>.None;
    }

    public Option<IRecipeForUi> GetDischargingRecipeFor()
    {
      if (!this.AssignedProduct.HasValue)
        return Option<IRecipeForUi>.None;
      foreach (IRecipeForUi recipe in this.Prototype.Recipes)
      {
        if ((Proto) recipe.AllUserVisibleOutputs.First.Product == (Proto) this.AssignedProduct.Value.Product)
          return recipe.SomeOption<IRecipeForUi>();
      }
      return Option<IRecipeForUi>.None;
    }

    static ThermalStorage()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ThermalStorage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      ThermalStorage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      ThermalStorage.NOTIFICATION_HYSTERESIS = Percent.FromPercentVal(5);
    }

    public enum State
    {
      None,
      Working,
      Broken,
      Paused,
      NotEnoughWorkers,
      NotEnoughPower,
    }
  }
}
