// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.MechPowerGeneratorFromProduct
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
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GenerateSerializer(false, null, 0)]
  public sealed class MechPowerGeneratorFromProduct : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IAnimatedEntity,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithSound,
    IUpgradableEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private MechPowerGeneratorFromProductProto m_proto;
    private readonly IProductsManager m_productsManager;
    private readonly IShaftBuffer m_shaftBuffer;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_connectedOutputPorts;
    private EntityNotificatorWithProtoParam m_needsTransportNotif;
    private MechPower m_mechPowerToOutput;
    private Percent m_currentEfficiency;
    private Quantity m_producedProductToOutput;
    private Quantity m_inputQuantityBuffer;

    public static void Serialize(MechPowerGeneratorFromProduct value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MechPowerGeneratorFromProduct>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MechPowerGeneratorFromProduct.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteBool(this.AutoBalance);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteBool(this.IsOffDueToAutoBalance);
      MechPower.Serialize(this.LossesLastTick, writer);
      Percent.Serialize(this.m_currentEfficiency, writer);
      Quantity.Serialize(this.m_inputQuantityBuffer, writer);
      MechPower.Serialize(this.m_mechPowerToOutput, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_needsTransportNotif, writer);
      Quantity.Serialize(this.m_producedProductToOutput, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<MechPowerGeneratorFromProductProto>(this.m_proto);
      writer.WriteGeneric<IShaftBuffer>(this.m_shaftBuffer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      MechPower.Serialize(this.PowerGeneratedLastTick, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static MechPowerGeneratorFromProduct Deserialize(BlobReader reader)
    {
      MechPowerGeneratorFromProduct generatorFromProduct;
      if (reader.TryStartClassDeserialization<MechPowerGeneratorFromProduct>(out generatorFromProduct))
        reader.EnqueueDataDeserialization((object) generatorFromProduct, MechPowerGeneratorFromProduct.s_deserializeDataDelayedAction);
      return generatorFromProduct;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.AutoBalance = reader.ReadBool();
      this.CurrentState = (MechPowerGeneratorFromProduct.State) reader.ReadInt();
      this.IsOffDueToAutoBalance = reader.ReadBool();
      this.LossesLastTick = MechPower.Deserialize(reader);
      this.m_currentEfficiency = Percent.Deserialize(reader);
      this.m_inputQuantityBuffer = Quantity.Deserialize(reader);
      this.m_mechPowerToOutput = MechPower.Deserialize(reader);
      this.m_needsTransportNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_producedProductToOutput = Quantity.Deserialize(reader);
      reader.SetField<MechPowerGeneratorFromProduct>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<MechPowerGeneratorFromProductProto>();
      reader.SetField<MechPowerGeneratorFromProduct>(this, "m_shaftBuffer", (object) reader.ReadGenericAs<IShaftBuffer>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.PowerGeneratedLastTick = MechPower.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    [DoNotSave(0, null)]
    public MechPowerGeneratorFromProductProto Prototype
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

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => this.PowerGeneratedLastTick.IsNotPositive;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.SoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    public bool IsSoundOn => this.IsEnabled && this.m_mechPowerToOutput.IsPositive;

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public MechPower PowerGeneratedLastTick { get; private set; }

    public Quantity OutputBufferQuantity => this.m_producedProductToOutput;

    public Quantity InputBufferQuantity => this.m_inputQuantityBuffer;

    public MechPower LossesLastTick { get; private set; }

    public MechPowerGeneratorFromProduct.State CurrentState { get; private set; }

    public bool AutoBalance { get; set; }

    public bool IsOffDueToAutoBalance { get; private set; }

    /// <summary>
    /// Efficiency based on <see cref="P:Mafi.Base.Prototypes.Machines.PowerGenerators.MechPowerGeneratorFromProduct.LossesLastTick" />.
    /// </summary>
    public Percent Efficiency
    {
      get
      {
        return !this.PowerGeneratedLastTick.IsPositive ? Percent.Zero : Percent.Hundred - Percent.FromRatio(this.LossesLastTick.Value, this.Prototype.MechPowerOutput.Value);
      }
    }

    public MechPowerGeneratorFromProduct(
      EntityId id,
      MechPowerGeneratorFromProductProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IShaftManager shaftManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_productsManager = productsManager;
      this.Upgrader = upgraderFactory.CreateInstance<MechPowerGeneratorFromProductProto, MechPowerGeneratorFromProduct>(this, proto);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_shaftBuffer = shaftManager.GetOrCreateShaftBufferFor((IEntityWithPorts) this, MechPower.Zero);
      this.m_needsTransportNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NeedsTransportConnected);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.reInitPorts();
    }

    private void reInitPorts()
    {
      this.m_connectedOutputPorts = this.Prototype.ProducedProduct.HasValue ? this.ConnectedOutputPorts.AsEnumerable().Where<IoPortData>((Func<IoPortData, bool>) (x => x.AllowedProductType == this.Prototype.ProducedProduct.Value.Product.Type)).ToImmutableArray<IoPortData>() : ImmutableArray<IoPortData>.Empty;
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.reInitPorts();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      bool needsTransportNotif = false;
      if (this.Prototype.ProducedProduct.HasValue && this.m_producedProductToOutput.IsPositive)
      {
        foreach (IoPortData connectedOutputPort in this.m_connectedOutputPorts)
          this.m_producedProductToOutput = connectedOutputPort.SendAsMuchAs(this.Prototype.ProducedProduct.Value.Product.WithQuantity(this.m_producedProductToOutput));
      }
      if (this.IsNotEnabled)
        stepNoInput(this.Maintenance.Status.IsBroken ? MechPowerGeneratorFromProduct.State.Broken : MechPowerGeneratorFromProduct.State.Paused);
      else if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        stepNoInput(MechPowerGeneratorFromProduct.State.NotEnoughWorkers);
      else if (this.Prototype.ProducedProduct.HasValue && this.m_producedProductToOutput > this.Prototype.ProducedProduct.Value.Quantity)
      {
        needsTransportNotif = !this.Prototype.ProducedProduct.Value.Product.CanBeLoadedOnTruck;
        stepNoInput(MechPowerGeneratorFromProduct.State.OutputFull);
      }
      else
      {
        if (this.m_mechPowerToOutput < this.Prototype.MechPowerOutput && this.m_inputQuantityBuffer >= this.Prototype.ConsumedProduct.Quantity)
        {
          this.m_inputQuantityBuffer -= this.Prototype.ConsumedProduct.Quantity;
          this.m_productsManager.ProductDestroyed(this.Prototype.ConsumedProduct.Product, this.Prototype.ConsumedProduct.Quantity, DestroyReason.General);
          MechPower mechPower = this.Prototype.MechPowerOutput * this.Prototype.RecipeDuration.Ticks;
          this.m_mechPowerToOutput += mechPower;
          this.m_productsManager.ProductCreated(this.m_shaftBuffer.Product, mechPower.Quantity, CreateReason.Produced);
          if (this.Prototype.ProducedProduct.HasValue)
          {
            this.m_producedProductToOutput += this.Prototype.ProducedProduct.Value.Quantity;
            this.m_productsManager.ProductCreated(this.Prototype.ProducedProduct.Value.Product, this.Prototype.ProducedProduct.Value.Quantity, CreateReason.Produced);
          }
        }
        if (this.m_mechPowerToOutput.IsNotPositive)
          stepNoInput(MechPowerGeneratorFromProduct.State.NotEnoughInput);
        else if (this.m_shaftBuffer.Shaft.IsDefaultNoCapacityShaft)
          stepNoInput(MechPowerGeneratorFromProduct.State.NoShaft);
        else if (this.m_shaftBuffer.Capacity.IsZero)
        {
          stepNoInput(MechPowerGeneratorFromProduct.State.Idle);
        }
        else
        {
          Assert.That<bool>(this.m_shaftBuffer.IsDestroyed).IsFalse("Stepping with destroyed shaft buffer.");
          if (this.IsOffDueToAutoBalance)
          {
            if (this.m_shaftBuffer.Shaft.CurrentInertia <= Mafi.Core.Factory.MechanicalPower.Shaft.SWITCHING_GEN_START_AT || !this.AutoBalance)
            {
              this.IsOffDueToAutoBalance = false;
            }
            else
            {
              stepNoInput(MechPowerGeneratorFromProduct.State.Idle);
              return;
            }
          }
          else if (this.AutoBalance && this.m_shaftBuffer.Shaft.CurrentInertia >= Mafi.Core.Factory.MechanicalPower.Shaft.SWITCHING_GEN_STOP_AT)
          {
            this.IsOffDueToAutoBalance = true;
            stepNoInput(MechPowerGeneratorFromProduct.State.Idle);
            return;
          }
          this.m_needsTransportNotif.Deactivate((IEntity) this);
          MechPower mechPower1 = this.m_mechPowerToOutput.Min(this.Prototype.MechPowerOutput);
          MechPower mechPower2 = mechPower1;
          if (this.m_currentEfficiency < Percent.Hundred)
          {
            this.m_currentEfficiency = (this.m_currentEfficiency + this.Prototype.EfficiencyIncPerTick).Min(Percent.Hundred);
            mechPower2 = mechPower2.ScaledBy(this.m_currentEfficiency);
          }
          MechPower mechPower3 = MechPower.FromQuantity(this.m_shaftBuffer.StoreAsMuchAs(mechPower2.Quantity));
          MechPower mechPower4 = mechPower2 - mechPower3;
          this.PowerGeneratedLastTick = mechPower4;
          this.LossesLastTick = mechPower1 - mechPower4;
          this.m_mechPowerToOutput -= mechPower1;
          this.m_productsManager.ProductDestroyed(this.m_shaftBuffer.Product, this.LossesLastTick.Quantity, DestroyReason.Wasted);
          this.CurrentState = this.PowerGeneratedLastTick.IsPositive ? MechPowerGeneratorFromProduct.State.Working : MechPowerGeneratorFromProduct.State.OutputFull;
          this.AnimationStatesProvider.Step(Percent.FromRatio(this.PowerGeneratedLastTick.Value, this.Prototype.MechPowerOutput.Value), Percent.Zero);
        }
      }

      void stepNoInput(MechPowerGeneratorFromProduct.State state)
      {
        this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
        this.CurrentState = state;
        if (this.m_currentEfficiency.IsPositive)
          this.m_currentEfficiency = (this.m_currentEfficiency - this.Prototype.EfficiencyDecPerTick).Max(Percent.Zero);
        this.PowerGeneratedLastTick = MechPower.Zero;
        ref EntityNotificatorWithProtoParam local1 = ref this.m_needsTransportNotif;
        ref readonly ProductQuantity? local2 = ref this.Prototype.ProducedProduct;
        ProductProto product = local2.HasValue ? local2.GetValueOrDefault().Product : (ProductProto) null;
        int num = needsTransportNotif ? 1 : 0;
        local1.NotifyIff((Proto) product, num != 0, (IEntity) this);
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || (Proto) pq.Product != (Proto) this.Prototype.ConsumedProduct.Product || pq.Quantity.IsNotPositive || this.m_inputQuantityBuffer >= this.Prototype.ConsumedProduct.Quantity)
        return pq.Quantity;
      Quantity rhs = this.Prototype.ConsumedProduct.Quantity - this.m_inputQuantityBuffer;
      Assert.That<Quantity>(rhs).IsPositive();
      Quantity quantity = pq.Quantity.Min(rhs);
      this.m_inputQuantityBuffer += quantity;
      return pq.Quantity - quantity;
    }

    private void clear()
    {
      this.m_productsManager.ProductDestroyed(this.m_shaftBuffer.Product, this.m_mechPowerToOutput.Quantity, DestroyReason.Cleared);
      this.m_mechPowerToOutput = MechPower.Zero;
      this.m_productsManager.ProductDestroyed(this.Prototype.ConsumedProduct.Product, this.m_inputQuantityBuffer, DestroyReason.Cleared);
      this.m_inputQuantityBuffer = Quantity.Zero;
      if (!this.Prototype.ProducedProduct.HasValue)
        return;
      this.m_productsManager.ProductDestroyed(this.Prototype.ProducedProduct.Value.Product, this.m_producedProductToOutput, DestroyReason.Cleared);
      this.m_producedProductToOutput = Quantity.Zero;
    }

    protected override void OnDestroy()
    {
      Assert.That<bool>(this.m_shaftBuffer.IsDestroyed).IsTrue();
      this.clear();
      base.OnDestroy();
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      this.clear();
      this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetAutoBalance(this.AutoBalance);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      this.AutoBalance = ((int) data.GetAutoBalance() ?? (this.AutoBalance ? 1 : 0)) != 0;
    }

    static MechPowerGeneratorFromProduct()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      MechPowerGeneratorFromProduct.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      MechPowerGeneratorFromProduct.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      None,
      Working,
      Idle,
      Broken,
      Paused,
      NotEnoughWorkers,
      OutputFull,
      NotEnoughInput,
      NoShaft,
    }
  }
}
