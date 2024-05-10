// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ElectricityGeneratorFromProduct
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
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
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class ElectricityGeneratorFromProduct : 
    LayoutEntity,
    IElectricityGeneratingEntity,
    IElectricityGenerator,
    IEntity,
    IIsSafeAsHashKey,
    IInputBufferPriorityProvider,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IMaintainedEntity,
    IAnimatedEntity,
    IEntityWithSound,
    IEntityWithLogisticsControl,
    IEntityWithParticles,
    IEntityWithSimUpdate,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private bool m_workedLastTrick;
    public readonly ElectricityGeneratorFromProductProto Prototype;
    private readonly IProductsManager m_productsManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private Fix32 m_durationLeftTicks;
    private readonly ProductBuffer m_inputBuffer;
    private readonly Option<IProductBuffer> m_outputBuffer;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;

    public static void Serialize(ElectricityGeneratorFromProduct value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityGeneratorFromProduct>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityGeneratorFromProduct.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      Percent.Serialize(this.CurrentFuelUsage, writer);
      writer.WriteGeneric<IElectricityGeneratorRegistrator>(this.ElectricityGenerator);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      Fix32.Serialize(this.m_durationLeftTicks, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      ProductBuffer.Serialize(this.m_inputBuffer, writer);
      Option<IProductBuffer>.Serialize(this.m_outputBuffer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteBool(this.m_workedLastTrick);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<ElectricityGeneratorFromProductProto>(this.Prototype);
    }

    public static ElectricityGeneratorFromProduct Deserialize(BlobReader reader)
    {
      ElectricityGeneratorFromProduct generatorFromProduct;
      if (reader.TryStartClassDeserialization<ElectricityGeneratorFromProduct>(out generatorFromProduct))
        reader.EnqueueDataDeserialization((object) generatorFromProduct, ElectricityGeneratorFromProduct.s_deserializeDataDelayedAction);
      return generatorFromProduct;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentFuelUsage = Percent.Deserialize(reader);
      this.ElectricityGenerator = reader.ReadGenericAs<IElectricityGeneratorRegistrator>();
      reader.SetField<ElectricityGeneratorFromProduct>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      this.m_durationLeftTicks = Fix32.Deserialize(reader);
      reader.SetField<ElectricityGeneratorFromProduct>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      reader.SetField<ElectricityGeneratorFromProduct>(this, "m_inputBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<ElectricityGeneratorFromProduct>(this, "m_outputBuffer", (object) Option<IProductBuffer>.Deserialize(reader));
      reader.SetField<ElectricityGeneratorFromProduct>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_workedLastTrick = reader.ReadBool();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.SetField<ElectricityGeneratorFromProduct>(this, "Prototype", (object) reader.ReadGenericAs<ElectricityGeneratorFromProductProto>());
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
      get => this.IsEnabled && this.m_workedLastTrick && this.CurrentFuelUsage.IsPositive;
    }

    public IElectricityGeneratorRegistrator ElectricityGenerator { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => !this.m_workedLastTrick;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public bool AreParticlesEnabled
    {
      get => this.IsEnabled && this.m_workedLastTrick && this.CurrentFuelUsage.IsPositive;
    }

    public Electricity MaxGenerationCapacity
    {
      get => !this.IsEnabled ? Electricity.Zero : this.Prototype.OutputElectricity;
    }

    public override bool CanBePaused => true;

    public override bool IsCargoAffectedByGeneralPriority => true;

    public Percent CurrentProductLeft
    {
      get => Percent.FromRatio(this.m_durationLeftTicks, (Fix32) this.Prototype.Duration.Ticks);
    }

    public Percent CurrentFuelUsage { get; private set; }

    public IProductBufferReadOnly InputBuffer => (IProductBufferReadOnly) this.m_inputBuffer;

    public Option<IProductBufferReadOnly> OutputBuffer
    {
      get => this.m_outputBuffer.As<IProductBufferReadOnly>();
    }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public ElectricityGeneratorFromProduct(
      EntityId id,
      ElectricityGeneratorFromProductProto proto,
      TileTransform transform,
      EntityContext context,
      IElectricityGeneratorRegistratorFactory generatorRegistratorFactory,
      IVirtualBuffersMap virtualBuffersMap,
      IProductsManager productsManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory,
      IFuelStatsCollector fuelStatsCollector)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_productsManager = productsManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_inputBuffer = new ProductBuffer(proto.BufferCapacityMultiplier * proto.InputProduct.Quantity, proto.InputProduct.Product);
      this.ElectricityGenerator = generatorRegistratorFactory.CreateAndRegisterFor((IElectricityGeneratingEntity) this, this.Prototype.GenerationPriority);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) this, (IOutputBufferPriorityProvider) new StaticPriorityProvider(BufferStrategy.Ignore), vehicleBuffersRegistry);
      this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) this.m_inputBuffer);
      if (!proto.OutputProduct.IsNotEmpty)
        return;
      ProductQuantity outputProduct = proto.OutputProduct;
      if (outputProduct.Product.Type == VirtualProductProto.ProductType)
      {
        this.m_outputBuffer = virtualBuffersMap.GetBuffer(outputProduct.Product, (IEntity) this);
      }
      else
      {
        this.m_outputBuffer = (Option<IProductBuffer>) (IProductBuffer) new ProductBuffer(this.Prototype.BufferCapacityMultiplier * outputProduct.Quantity, outputProduct.Product);
        this.m_autoLogisticsHelper.AddOutputBuffer(this.m_outputBuffer.Value);
      }
    }

    public void SimUpdate()
    {
      if (!this.IsEnabled || this.m_outputBuffer.IsNone || this.m_outputBuffer.Value.IsEmpty() || this.Prototype.OutputProduct.Product.Type == VirtualProductProto.ProductType)
        return;
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
      {
        connectedOutputPort.SendAsMuchAsFromBuffer(this.m_outputBuffer.Value);
        this.m_autoLogisticsHelper.OnProductSentToPort(this.m_outputBuffer.Value);
      }
    }

    public Electricity GetCurrentMaxGeneration(out bool canGenerate)
    {
      canGenerate = this.IsEnabled && (this.m_durationLeftTicks >= Fix32.One || this.m_inputBuffer.IsNotEmpty) && Entity.HasWorkers((IEntityWithWorkers) this);
      if (canGenerate)
        return this.Prototype.OutputElectricity;
      this.CurrentFuelUsage = Percent.Zero;
      this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
      this.m_workedLastTrick = false;
      return Electricity.Zero;
    }

    public Electricity GenerateAsMuchAs(Electricity freeCapacity, Electricity currentMaxGeneration)
    {
      if (this.m_durationLeftTicks < Fix32.One)
      {
        IProductBuffer valueOrNull = this.m_outputBuffer.ValueOrNull;
        bool flag = valueOrNull == null || valueOrNull.CanStore(this.Prototype.OutputProduct.Quantity);
        if (!this.m_inputBuffer.CanRemove(this.Prototype.InputProduct.Quantity) || !flag)
        {
          this.CurrentFuelUsage = Percent.Zero;
          this.m_workedLastTrick = false;
          this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
          return Electricity.Zero;
        }
        Quantity quantity1 = this.m_inputBuffer.RemoveAsMuchAs(this.Prototype.InputProduct.Quantity);
        if (this.m_outputBuffer.HasValue)
        {
          Quantity quantity2 = this.m_outputBuffer.Value.StoreAsMuchAsReturnStored(this.Prototype.OutputProduct.Quantity);
          if (quantity2.IsPositive)
            this.m_productsManager.ProductCreated(this.m_outputBuffer.Value.Product, quantity2, CreateReason.Produced);
        }
        if (this.Prototype.ProductDestroyReason == DestroyReason.UsedAsFuel)
          this.m_fuelStatsCollector.ReportFuelUseAndDestroy(this.Prototype.InputProduct.Product, quantity1, FuelUsedBy.PowerGenerator);
        else
          this.m_productsManager.ProductDestroyed(this.Prototype.InputProduct.Product, quantity1, this.Prototype.ProductDestroyReason);
        this.m_durationLeftTicks += (Fix32) this.Prototype.Duration.Ticks;
      }
      this.CurrentFuelUsage = Percent.FromRatio(freeCapacity.Value, this.Prototype.OutputElectricity.Value);
      Assert.That<Percent>(this.CurrentFuelUsage).IsWithin0To100PercIncl();
      this.m_durationLeftTicks -= this.CurrentFuelUsage.ToFix32();
      this.AnimationStatesProvider.Step(this.CurrentFuelUsage, Percent.Zero);
      this.m_workedLastTrick = true;
      return freeCapacity;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return new BufferStrategy(this.GeneralPriority, new Quantity?(buffer.Capacity / 2));
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || (Proto) pq.Product != (Proto) this.m_inputBuffer.Product)
        return pq.Quantity;
      this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) this.m_inputBuffer);
      return this.m_inputBuffer.StoreAsMuchAs(pq.Quantity);
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

    static ElectricityGeneratorFromProduct()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ElectricityGeneratorFromProduct.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      ElectricityGeneratorFromProduct.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
