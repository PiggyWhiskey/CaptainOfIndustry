// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.Hospital
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
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class Hospital : 
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
    IMaintainedEntity,
    IEntityWithPorts,
    IElectricityConsumingEntity,
    IEntityWithLogisticsControl,
    IEntityWithMultipleProductsToAssign,
    ILayoutEntity,
    IAnimatedEntity,
    IEntityWithEmission,
    IEntityWithSimUpdate
  {
    /// <summary>
    /// Multiplier of medical supplies when a disease is active.
    /// </summary>
    private static readonly Percent SUPPLIES_CONSUMPTION_MULTIPLIER_DURING_DISEASE;
    private HospitalProto m_proto;
    private readonly Lyst<Option<ProductBuffer>> m_buffersPerSlot;
    private readonly Lyst<PartialQuantity> m_partialBuffersPerSlot;
    [DoNotSave(0, null)]
    private Percent m_consumptionMultiplier;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private Lyst<int> m_buffersIndicesSorted;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private readonly ProtosDb m_protosDb;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PopNeedProto ProvidedNeed => this.Prototype.PopsNeed;

    [DoNotSave(0, null)]
    public HospitalProto Prototype
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

    public Electricity PowerRequired => this.Prototype.PowerRequired;

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionIntensity.HasValue ? new float?() : new float?(this.CurrentState == Hospital.State.Working ? (float) this.Prototype.EmissionIntensity.Value : 0.0f);
      }
    }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public MaintenanceCosts MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public bool IsIdleForMaintenance => this.CurrentState != Hospital.State.Working;

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Hospital.State CurrentState { get; private set; }

    public Option<Mafi.Core.Buildings.Settlements.Settlement> Settlement { get; private set; }

    public IIndexable<Option<ProductBuffer>> BuffersPerSlot
    {
      get => (IIndexable<Option<ProductBuffer>>) this.m_buffersPerSlot;
    }

    [DoNotSave(0, null)]
    public Percent UnityProductionMultiplier { get; private set; }

    public int BuffersCount => this.Prototype.BuffersCount;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    [DoNotSave(0, null)]
    public ImmutableArray<ProductProto> SupportedProducts { get; private set; }

    public Hospital(
      EntityId id,
      HospitalProto proto,
      TileTransform transform,
      EntityContext context,
      ProtosDb protosDb,
      PopsHealthManager popsHealthManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IAssetTransactionManager assetTransactionManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_buffersPerSlot = new Lyst<Option<ProductBuffer>>();
      this.m_partialBuffersPerSlot = new Lyst<PartialQuantity>();
      this.m_buffersIndicesSorted = new Lyst<int>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_protosDb = protosDb;
      this.m_popsHealthManager = popsHealthManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_electricityConsumer = context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.updateProperties();
      this.Upgrader = upgraderFactory.CreateInstance<HospitalProto, Hospital>(this, this.Prototype);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      for (int index = 0; index < this.Prototype.BuffersCount; ++index)
      {
        this.m_buffersPerSlot.Add((Option<ProductBuffer>) Option.None);
        this.m_partialBuffersPerSlot.Add(PartialQuantity.Zero);
      }
      this.initProducts();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initProducts()
    {
      this.SupportedProducts = this.m_protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.GetParam<MedicalSuppliesParam>().HasValue)).ToImmutableArray<ProductProto>();
      this.recalculateSortedBuffers();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void updateProperties()
    {
      this.UnityProductionMultiplier = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.UnityProductionMultiplier);
      this.m_consumptionMultiplier = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.SettlementConsumptionMultiplier);
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    private void recalculateSortedBuffers()
    {
      this.m_buffersIndicesSorted.Clear();
      for (int index = 0; index < this.m_buffersPerSlot.Count; ++index)
      {
        if (this.m_buffersPerSlot[index].HasValue)
          this.m_buffersIndicesSorted.Add(index);
      }
      this.m_buffersIndicesSorted = this.m_buffersIndicesSorted.OrderByDescending<int, Percent>((Func<int, Percent>) (x => this.m_buffersPerSlot[x].Value.Product.GetParam<MedicalSuppliesParam>().Value.MortalityDeductionWhenProvided)).ToLyst<int>();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      if (this.CurrentState == Hospital.State.Working)
        this.AnimationStatesProvider.Step(Percent.Hundred, Percent.Zero);
      else
        this.AnimationStatesProvider.Pause();
    }

    private Hospital.State updateState()
    {
      if (!this.IsEnabledNowIgnoreUpgrade)
        return !this.Maintenance.Status.IsBroken ? Hospital.State.Paused : Hospital.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return Hospital.State.MissingWorkers;
      if (!this.m_electricityConsumer.TryConsume())
        return Hospital.State.NotEnoughPower;
      bool flag = false;
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (option.HasValue && option.Value.IsNotEmpty)
        {
          flag = true;
          break;
        }
      }
      return !flag ? Hospital.State.MissingInput : Hospital.State.Working;
    }

    public void SetProduct(Option<ProductProto> product, int bufferSlot, bool skipIfClearing)
    {
      if (bufferSlot < 0 || bufferSlot >= this.m_buffersPerSlot.Count)
        Assert.Fail(string.Format("Provided buffet slot '{0}' is out of range in {1}!", (object) bufferSlot, (object) this.Prototype.Id));
      else if (product.HasValue && product.Value.GetParam<MedicalSuppliesParam>().IsNone)
      {
        Log.Error(string.Format("Cannot add product '{0}' as it has no medical param", (object) product.Value));
      }
      else
      {
        Option<ProductBuffer> currentBuffer = this.m_buffersPerSlot[bufferSlot];
        if (product.IsNone)
        {
          if (!currentBuffer.HasValue || skipIfClearing)
            return;
          clearProduct();
          this.recalculateSortedBuffers();
        }
        else if (!this.SupportedProducts.Contains(product.Value))
          Assert.Fail(string.Format("Product '{0}' is not supported in {1}", (object) product.Value, (object) this.Prototype.Id));
        else if (currentBuffer.IsNone)
        {
          createNewBuffer();
        }
        else
        {
          if (currentBuffer.Value.Product == product || currentBuffer.Value.Quantity.IsPositive & skipIfClearing)
            return;
          clearProduct();
          createNewBuffer();
        }

        void clearProduct()
        {
          this.m_autoLogisticsHelper.RemoveInputBuffer((IProductBuffer) currentBuffer.Value);
          this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) currentBuffer.Value);
          this.m_buffersPerSlot[bufferSlot] = (Option<ProductBuffer>) Option.None;
          this.m_partialBuffersPerSlot[bufferSlot] = PartialQuantity.Zero;
        }
      }

      void createNewBuffer()
      {
        GlobalInputBuffer buffer = new GlobalInputBuffer(this.Prototype.CapacityPerBuffer, product.Value, this.Context.ProductsManager, 5, (IEntity) this);
        this.m_buffersPerSlot[bufferSlot] = (Option<ProductBuffer>) (ProductBuffer) buffer;
        this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) buffer);
        this.recalculateSortedBuffers();
      }
    }

    public Option<IProductBuffer> GetBuffer(int bufferSlot)
    {
      return bufferSlot < 0 || bufferSlot >= this.m_buffersPerSlot.Count ? Option<IProductBuffer>.None : this.m_buffersPerSlot[bufferSlot].As<IProductBuffer>();
    }

    public Quantity GetCapacity(int bufferSlot) => this.Prototype.CapacityPerBuffer;

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (!this.IsEnabled)
        return pq.Quantity;
      Quantity quantity = pq.Quantity;
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (!option.IsNone)
        {
          ProductBuffer bufferReceived = option.Value;
          if ((Proto) bufferReceived.Product == (Proto) pq.Product)
          {
            quantity = bufferReceived.StoreAsMuchAs(quantity);
            this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) bufferReceived);
            if (quantity.IsNotPositive)
              break;
          }
        }
      }
      return quantity;
    }

    public PartialQuantity GetSettlementSuppliesNeedPerMonth()
    {
      return this.Prototype.GetSuppliesConsumedQuantityFromPopDays(this.Settlement.Value.Population * 30, this.m_consumptionMultiplier).ScaledBy(this.m_popsHealthManager.CurrentDisease.HasValue ? Hospital.SUPPLIES_CONSUMPTION_MULTIPLIER_DURING_DISEASE : Percent.Hundred);
    }

    /// <summary>
    /// Returns the amount of pops it did not manage to satisfy.
    /// </summary>
    public int TrySatisfyNeedOnNewDay(
      int popsToSatisfy,
      int settlementPopulation,
      out Percent healthProvided,
      out Upoints upointsGenerated,
      out Percent mortalityDeduction)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse();
      healthProvided = Percent.Zero;
      upointsGenerated = Upoints.Zero;
      mortalityDeduction = Percent.Zero;
      if (this.CurrentState != Hospital.State.Working)
        return popsToSatisfy;
      if (popsToSatisfy == 0)
        return 0;
      foreach (int index in this.m_buffersIndicesSorted)
      {
        if (index >= this.m_buffersPerSlot.Count)
          Log.Error(string.Format("Invalid index '{0}' to buffer with '{1}' elements.", (object) index, (object) this.m_buffersPerSlot.Count));
        else if (this.m_buffersPerSlot[index].IsNone)
        {
          Log.Error(string.Format("Invalid index '{0}' to null buffer.", (object) index));
        }
        else
        {
          ProductBuffer productBuffer = this.m_buffersPerSlot[index].Value;
          PartialQuantity partialQuantity1 = this.m_partialBuffersPerSlot[index];
          if (productBuffer.IsNotEmpty && partialQuantity1.IntegerPart.IsPositive)
          {
            Quantity quantity = productBuffer.RemoveAsMuchAs(partialQuantity1.IntegerPart);
            if (quantity.IsPositive)
            {
              this.Settlement.Value.TransformProductIntoWaste(productBuffer.Product, quantity);
              partialQuantity1 -= quantity.AsPartial;
              this.m_partialBuffersPerSlot[index] = partialQuantity1;
            }
          }
          MedicalSuppliesParam paramValue;
          if (!productBuffer.IsEmpty && productBuffer.Product.TryGetParam<MedicalSuppliesParam>(out paramValue))
          {
            PartialQuantity partialQuantity2 = partialQuantity1 + this.Prototype.GetSuppliesConsumedQuantityFromPopDays(popsToSatisfy, this.m_consumptionMultiplier).ScaledBy(this.m_popsHealthManager.CurrentDisease.HasValue ? Hospital.SUPPLIES_CONSUMPTION_MULTIPLIER_DURING_DISEASE : Percent.Hundred);
            this.m_partialBuffersPerSlot[index] = partialQuantity2;
            Percent percent = Percent.FromRatio(popsToSatisfy, settlementPopulation);
            healthProvided = paramValue.HealthPointsWhenProvided.ScaleBy(percent);
            upointsGenerated = paramValue.GetUnityWhenProvided(this.UnityProductionMultiplier).ScaledBy(percent);
            mortalityDeduction = paramValue.MortalityDeductionWhenProvided.ScaleBy(percent);
            return 0;
          }
        }
      }
      return popsToSatisfy;
    }

    public void SetSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      Assert.That<Option<Mafi.Core.Buildings.Settlements.Settlement>>(this.Settlement).IsNone<Mafi.Core.Buildings.Settlements.Settlement>();
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
    }

    public void ReplaceSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      Assert.That<Option<Mafi.Core.Buildings.Settlements.Settlement>>(this.Settlement).HasValue<Mafi.Core.Buildings.Settlements.Settlement>();
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
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
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (option.HasValue)
          this.m_assetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) option.Value);
      }
      base.OnDestroy();
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        Assert.That<int>(this.Prototype.Upgrade.NextTier.Value.BuffersCount).IsGreaterOrEqual(this.Prototype.BuffersCount);
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        Lyst<Option<ProductBuffer>> list = new Lyst<Option<ProductBuffer>>();
        for (int index = 0; index < this.Prototype.BuffersCount; ++index)
        {
          if (index < this.m_buffersPerSlot.Count)
          {
            Option<ProductBuffer> option = this.m_buffersPerSlot[index];
            if (option.HasValue)
            {
              option.Value.IncreaseCapacityTo(this.Prototype.CapacityPerBuffer);
              list.Add(option);
              continue;
            }
          }
          list.Add(Option<ProductBuffer>.None);
          this.m_partialBuffersPerSlot.Add(PartialQuantity.Zero);
        }
        this.m_buffersPerSlot.Clear();
        this.m_buffersPerSlot.AddRange(list);
      }
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => false;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode => EntityLogisticsMode.Off;

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    public static void Serialize(Hospital value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Hospital>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Hospital.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactionManager);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      Lyst<Option<ProductBuffer>>.Serialize(this.m_buffersPerSlot, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      Lyst<PartialQuantity>.Serialize(this.m_partialBuffersPerSlot, writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<HospitalProto>(this.m_proto);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Option<Mafi.Core.Buildings.Settlements.Settlement>.Serialize(this.Settlement, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Hospital Deserialize(BlobReader reader)
    {
      Hospital hospital;
      if (reader.TryStartClassDeserialization<Hospital>(out hospital))
        reader.EnqueueDataDeserialization((object) hospital, Hospital.s_deserializeDataDelayedAction);
      return hospital;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentState = (Hospital.State) reader.ReadInt();
      reader.SetField<Hospital>(this, "m_assetTransactionManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<Hospital>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      this.m_buffersIndicesSorted = new Lyst<int>();
      reader.SetField<Hospital>(this, "m_buffersPerSlot", (object) Lyst<Option<ProductBuffer>>.Deserialize(reader));
      reader.SetField<Hospital>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<Hospital>(this, "m_partialBuffersPerSlot", (object) Lyst<PartialQuantity>.Deserialize(reader));
      reader.SetField<Hospital>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      this.m_proto = reader.ReadGenericAs<HospitalProto>();
      reader.RegisterResolvedMember<Hospital>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<Hospital>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<Hospital>(this, "initProducts", InitPriority.Normal);
      reader.RegisterInitAfterLoad<Hospital>(this, "updateProperties", InitPriority.Normal);
    }

    static Hospital()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Hospital.SUPPLIES_CONSUMPTION_MULTIPLIER_DURING_DISEASE = 150.Percent();
      Hospital.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Hospital.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      MissingInput,
      MissingWorkers,
      NotEnoughPower,
    }
  }
}
