// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Sorters.Sorter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Sorters
{
  [GenerateSerializer(false, null, 0)]
  public class Sorter : 
    LayoutEntity,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IElectricityConsumingEntity,
    IEntityWithGeneralPriority,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig
  {
    public const char SPECIAL_PORT_NAME = 'S';
    public static readonly Percent BUFFER_SCALE;
    public static readonly Duration MAX_DELAY;
    public static readonly Duration MEDIUM_DELAY;
    public static readonly Duration MIN_DELAY;
    private static readonly Duration CONSUME_POWER_FOR;
    public readonly SorterProto Prototype;
    private readonly Set<ProductProto> m_filteredProducts;
    private readonly IElectricityConsumer m_electricityConsumer;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private readonly ISimLoopEvents m_simLoopEvents;
    [DoNotSave(0, null)]
    private IoPortData? m_specialOutputPort;
    [DoNotSave(0, null)]
    private IoPortData? m_normalOutputPort;
    private readonly Queueue<ZipBuffProduct> m_outputBuffer;
    private int m_ticksLeft;
    [DoNotSave(0, null)]
    private Duration m_delay;
    private bool m_isMissingPower;
    private Duration m_consumePowerFor;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    /// <summary>Products that are filtered out to a special output.</summary>
    public IReadOnlySet<ProductProto> FilteredProducts
    {
      get => (IReadOnlySet<ProductProto>) this.m_filteredProducts;
    }

    public override bool IsGeneralPriorityVisible => !this.m_canWorkOnLowPower;

    Electricity IElectricityConsumingEntity.PowerRequired
    {
      get => !this.m_ignorePower ? this.Prototype.ElectricityConsumed : Electricity.Zero;
    }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    [DoNotSave(0, null)]
    public Quantity MaxBufferSize { get; private set; }

    [DoNotSave(0, null)]
    public Quantity QuantityInOutputBuffer { get; private set; }

    public Sorter(
      EntityId id,
      SorterProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_filteredProducts = new Set<ProductProto>();
      this.m_outputBuffer = new Queueue<ZipBuffProduct>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto.CheckNotNull<SorterProto>();
      this.m_simLoopEvents = simLoopEvents;
      this.updateProperties();
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.reInitPorts();
      this.initBuffers();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.updateProperties();
      this.initBuffers();
    }

    private void reInitPorts()
    {
      ImmutableArray<IoPort> immutableArray1 = this.Ports;
      IoPort port = immutableArray1.FirstOrDefault((Func<IoPort, bool>) (x => x.Name == 'S'));
      this.m_specialOutputPort = port != null ? new IoPortData?(new IoPortData(port)) : new IoPortData?();
      if (!this.m_specialOutputPort.HasValue)
        Log.Warning(string.Format("No special port found on sorter '{0}'.", (object) this.Prototype));
      immutableArray1 = this.Ports;
      immutableArray1 = immutableArray1.Filter((Predicate<IoPort>) (x => x.Type == IoPortType.Output && x.Name != 'S'));
      ImmutableArray<IoPortData> immutableArray2 = immutableArray1.Map<IoPortData>((Func<IoPort, IoPortData>) (x => new IoPortData(x)));
      if (immutableArray2.IsEmpty)
      {
        Log.Warning(string.Format("No normal port found on sorter '{0}'.", (object) this.Prototype));
      }
      else
      {
        if (immutableArray2.Length > 1)
          Log.Warning(string.Format("More than one normal ports found on sorter '{0}'.", (object) this.Prototype));
        this.m_normalOutputPort = new IoPortData?(immutableArray2.First);
      }
    }

    private void initBuffers()
    {
      this.recomputeBufferSizeAndDelay();
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        this.QuantityInOutputBuffer += zipBuffProduct.ProductQuantity.Quantity;
    }

    private void updateProperties()
    {
      this.m_canWorkOnLowPower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsCanWorkOnLowPower);
      if (this.m_canWorkOnLowPower)
        ((IEntityWithGeneralPriorityFriend) this).SetGeneralPriorityInternal(0);
      this.m_ignorePower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsIgnorePower);
      this.m_electricityConsumer?.OnPowerRequiredChanged();
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.reInitPorts();
      this.recomputeBufferSizeAndDelay();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled)
        return;
      while (this.QuantityInOutputBuffer.IsPositive && this.tryReleaseFirstProduct())
        this.m_consumePowerFor = Sorter.CONSUME_POWER_FOR;
      if (!this.m_consumePowerFor.IsPositive)
        return;
      this.m_isMissingPower = !this.m_electricityConsumer.TryConsume(this.m_canWorkOnLowPower);
      if (this.m_isMissingPower)
        return;
      this.m_consumePowerFor -= Duration.OneTick;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.m_consumePowerFor = Duration.OneTick;
    }

    public bool ToggleFilteredProduct(ProductProto product)
    {
      Assert.That<ProductProto>(product).IsNotNull<ProductProto>();
      if (!this.Prototype.AssignableProducts.Contains(product))
      {
        Log.Warning(string.Format("Trying to assign invalid type of product to sorter filter {0}", (object) product.Id));
        return false;
      }
      if (this.m_filteredProducts.Contains(product))
      {
        this.m_filteredProducts.Remove(product);
        return true;
      }
      this.m_filteredProducts.Add(product);
      return true;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || this.m_isMissingPower && !this.m_canWorkOnLowPower || this.QuantityInOutputBuffer >= this.MaxBufferSize)
        return pq.Quantity;
      this.m_outputBuffer.Enqueue(new ZipBuffProduct(pq, this.m_simLoopEvents.CurrentStep));
      this.QuantityInOutputBuffer += pq.Quantity;
      this.m_consumePowerFor = Sorter.CONSUME_POWER_FOR;
      return Quantity.Zero;
    }

    /// <summary>
    /// Sends content of the buffer to one of the output ports.
    /// </summary>
    private bool tryReleaseFirstProduct()
    {
      if (this.m_simLoopEvents.CurrentStep - this.m_outputBuffer.Peek().EnqueuedAtStep < this.m_delay)
        return false;
      ZipBuffProduct zipBuffProduct1 = this.m_outputBuffer.Dequeue();
      if (this.m_outputBuffer.IsNotEmpty)
      {
        ZipBuffProduct zipBuffProduct2 = this.m_outputBuffer.Peek();
        if (this.m_simLoopEvents.CurrentStep - zipBuffProduct2.EnqueuedAtStep >= this.m_delay && (Proto) zipBuffProduct2.ProductQuantity.Product == (Proto) zipBuffProduct1.ProductQuantity.Product)
        {
          this.m_outputBuffer.Dequeue();
          zipBuffProduct1 = new ZipBuffProduct((zipBuffProduct1.ProductQuantity.Quantity + zipBuffProduct2.ProductQuantity.Quantity).Of(zipBuffProduct1.ProductQuantity.Product), zipBuffProduct1.EnqueuedAtStep);
        }
      }
      ProductQuantity productQuantity = zipBuffProduct1.ProductQuantity;
      if (this.FilteredProducts.Contains(productQuantity.Product))
      {
        if (this.m_specialOutputPort.HasValue)
          this.m_specialOutputPort.Value.SendAsMuchAs(ref productQuantity);
      }
      else if (this.m_normalOutputPort.HasValue)
        this.m_normalOutputPort.Value.SendAsMuchAs(ref productQuantity);
      this.QuantityInOutputBuffer -= zipBuffProduct1.ProductQuantity.Quantity - productQuantity.Quantity;
      if (productQuantity.IsEmpty)
        return true;
      this.m_outputBuffer.EnqueueFirst(new ZipBuffProduct(productQuantity, zipBuffProduct1.EnqueuedAtStep));
      return false;
    }

    private void recomputeBufferSizeAndDelay()
    {
      PartialQuantity partialQuantity1 = PartialQuantity.Zero;
      foreach (IoPort port in this.Ports)
      {
        if (port.IsConnected)
          partialQuantity1 = partialQuantity1.Max(port.GetMaxThroughputPerTick());
      }
      this.m_delay = !(partialQuantity1.Value <= 0.5.ToFix32()) ? (!(partialQuantity1.Value <= 1.ToFix32()) ? Sorter.MIN_DELAY : Sorter.MEDIUM_DELAY) : Sorter.MAX_DELAY;
      PartialQuantity partialQuantity2 = this.m_delay.Ticks * partialQuantity1;
      partialQuantity2 = partialQuantity2.ScaledBy(Sorter.BUFFER_SCALE);
      this.MaxBufferSize = partialQuantity2.ToQuantityCeiled();
    }

    protected override void OnDestroy()
    {
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        this.Context.AssetTransactionManager.StoreClearedProduct(zipBuffProduct.ProductQuantity);
      this.m_outputBuffer.Clear();
      base.OnDestroy();
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetFilteredProducts(new ImmutableArray<ProductProto>?(this.m_filteredProducts.ToImmutableArray<ProductProto>()));
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      ImmutableArray<ProductProto>? filteredProducts = data.GetFilteredProducts();
      if (!filteredProducts.HasValue)
        return;
      this.m_filteredProducts.Clear();
      this.m_filteredProducts.AddRange(filteredProducts.Value.Where((Func<ProductProto, bool>) (x => this.Prototype.AssignableProducts.Contains(x))));
    }

    internal Option<string> VerifyStateCorrectness()
    {
      Quantity zero = Quantity.Zero;
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        zero += zipBuffProduct.ProductQuantity.Quantity;
      return zero != this.QuantityInOutputBuffer ? (Option<string>) string.Format("Quantity in the output buffer is {0} but cached value is {1}.", (object) zero, (object) this.QuantityInOutputBuffer) : Option<string>.None;
    }

    public static void Serialize(Sorter value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Sorter>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Sorter.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Duration.Serialize(this.m_consumePowerFor, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      Set<ProductProto>.Serialize(this.m_filteredProducts, writer);
      writer.WriteBool(this.m_isMissingPower);
      Queueue<ZipBuffProduct>.Serialize(this.m_outputBuffer, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt(this.m_ticksLeft);
      writer.WriteGeneric<SorterProto>(this.Prototype);
    }

    public static Sorter Deserialize(BlobReader reader)
    {
      Sorter sorter;
      if (reader.TryStartClassDeserialization<Sorter>(out sorter))
        reader.EnqueueDataDeserialization((object) sorter, Sorter.s_deserializeDataDelayedAction);
      return sorter;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_consumePowerFor = Duration.Deserialize(reader);
      reader.SetField<Sorter>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<Sorter>(this, "m_filteredProducts", (object) Set<ProductProto>.Deserialize(reader));
      this.m_isMissingPower = reader.ReadBool();
      reader.SetField<Sorter>(this, "m_outputBuffer", (object) Queueue<ZipBuffProduct>.Deserialize(reader));
      reader.SetField<Sorter>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_ticksLeft = reader.ReadInt();
      reader.SetField<Sorter>(this, "Prototype", (object) reader.ReadGenericAs<SorterProto>());
      reader.RegisterInitAfterLoad<Sorter>(this, "initSelf", InitPriority.Normal);
    }

    static Sorter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Sorter.BUFFER_SCALE = 150.Percent();
      Sorter.MAX_DELAY = 20.Ticks();
      Sorter.MEDIUM_DELAY = 10.Ticks();
      Sorter.MIN_DELAY = 5.Ticks();
      Sorter.CONSUME_POWER_FOR = 1.Seconds();
      Sorter.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Sorter.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
