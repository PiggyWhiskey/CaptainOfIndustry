// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Lifts.Lift
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
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
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Lifts
{
  [GenerateSerializer(false, null, 0)]
  public class Lift : 
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
    IAnimatedEntity
  {
    public static readonly Percent BUFFER_SCALE;
    public static readonly Duration DELAY_BASE;
    public static readonly Duration DELAY_PER_HEIGHT;
    private static readonly Duration CONSUME_POWER_FOR;
    private LiftProto m_proto;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly ISimLoopEvents m_simLoopEvents;
    [DoNotSave(0, null)]
    private IoPortData? m_outputPort;
    private readonly Queueue<ZipBuffProduct> m_outputBuffer;
    private int m_ticksLeft;
    [DoNotSave(0, null)]
    private Duration m_delay;
    private bool m_isMissingPower;
    private Duration m_consumePowerFor;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    [DoNotSave(0, null)]
    private Percent m_animationSpeed;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public LiftProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

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

    public Lift(
      EntityId id,
      LiftProto proto,
      TileTransform transform,
      IAnimationStateFactory animationStateFactory,
      EntityContext context,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_outputBuffer = new Queueue<ZipBuffProduct>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto.CheckNotNull<LiftProto>();
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
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
      immutableArray1 = immutableArray1.Filter((Predicate<IoPort>) (x => x.Type == IoPortType.Output));
      ImmutableArray<IoPortData> immutableArray2 = immutableArray1.Map<IoPortData>((Func<IoPort, IoPortData>) (x => new IoPortData(x)));
      if (immutableArray2.IsEmpty)
      {
        Log.Warning(string.Format("No output port found on sorter '{0}'.", (object) this.Prototype));
      }
      else
      {
        if (immutableArray2.Length > 1)
          Log.Warning(string.Format("More than one output ports found on sorter '{0}'.", (object) this.Prototype));
        this.m_outputPort = new IoPortData?(immutableArray2.First);
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
      {
        this.AnimationStatesProvider.Pause();
      }
      else
      {
        if (this.QuantityInOutputBuffer.IsPositive && this.m_consumePowerFor.IsPositive)
          this.AnimationStatesProvider.Step(this.m_animationSpeed, Percent.Zero);
        else
          this.AnimationStatesProvider.Pause();
        while (this.QuantityInOutputBuffer.IsPositive && this.tryReleaseFirstProduct())
          this.m_consumePowerFor = Lift.CONSUME_POWER_FOR;
        if (!this.m_consumePowerFor.IsPositive)
          return;
        this.m_isMissingPower = !this.m_electricityConsumer.TryConsume(this.m_canWorkOnLowPower);
        if (this.m_isMissingPower)
          return;
        this.m_consumePowerFor -= Duration.OneTick;
      }
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.m_consumePowerFor = Duration.OneTick;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || this.m_isMissingPower && !this.m_canWorkOnLowPower || this.QuantityInOutputBuffer >= this.MaxBufferSize)
        return pq.Quantity;
      this.m_outputBuffer.Enqueue(new ZipBuffProduct(pq, this.m_simLoopEvents.CurrentStep));
      this.QuantityInOutputBuffer += pq.Quantity;
      this.m_consumePowerFor = Lift.CONSUME_POWER_FOR;
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
      if (this.m_outputPort.HasValue)
        this.m_outputPort.Value.SendAsMuchAs(ref productQuantity);
      this.QuantityInOutputBuffer -= zipBuffProduct1.ProductQuantity.Quantity - productQuantity.Quantity;
      if (productQuantity.IsEmpty)
        return true;
      this.m_outputBuffer.EnqueueFirst(new ZipBuffProduct(productQuantity, zipBuffProduct1.EnqueuedAtStep));
      return false;
    }

    private void recomputeBufferSizeAndDelay()
    {
      PartialQuantity partialQuantity = PartialQuantity.MaxValue;
      foreach (IoPort port in this.Ports)
      {
        if (port.IsConnected)
          partialQuantity = partialQuantity.Min(port.GetMaxThroughputPerTick());
      }
      if (partialQuantity == PartialQuantity.MaxValue)
        partialQuantity = PartialQuantity.Zero;
      int num = !(partialQuantity.Value < 0.2.ToFix32()) ? (!(partialQuantity.Value < 0.6.ToFix32()) ? 3 : 2) : 1;
      this.m_animationSpeed = Percent.Hundred * num;
      if (this.Prototype.HeightDelta.IsNegative)
        this.m_animationSpeed = -this.m_animationSpeed;
      this.m_delay = (Lift.DELAY_BASE + Lift.DELAY_PER_HEIGHT * this.Prototype.HeightDelta.Value.Abs()) / num;
      int ticks = this.m_delay.Ticks;
      Fix32 fix32 = partialQuantity.Value.Min(0.75.ToFix32());
      fix32 = fix32.ScaledBy(Lift.BUFFER_SCALE);
      int intCeiled = fix32.ToIntCeiled();
      this.MaxBufferSize = new Quantity(ticks * intCeiled);
    }

    public void GetAllBufferedProducts(
      Lyst<KeyValuePair<ProductProto, Quantity>> aggregatedProducts)
    {
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        aggregate(zipBuffProduct.ProductQuantity);

      void aggregate(ProductQuantity pq)
      {
        Quantity quantity;
        int index;
        if (aggregatedProducts.TryGetValue<ProductProto, Quantity>(pq.Product, out quantity, out index))
          aggregatedProducts[index] = Make.Kvp<ProductProto, Quantity>(pq.Product, quantity + pq.Quantity);
        else
          aggregatedProducts.Add<ProductProto, Quantity>(pq.Product, pq.Quantity);
      }
    }

    public bool ReversePorts()
    {
      if (this.Ports.Length != 2)
      {
        Log.Error("Reversing of lift ports expecting exactly 2 ports.");
        return false;
      }
      ILiftProto newProto;
      if (!this.m_proto.TryGetHeightReversedProto(this.Context.ProtosDb, out newProto) || !(newProto is LiftProto liftProto))
      {
        Log.Error("Failed to get reverse height proto");
        return false;
      }
      ImmutableArrayBuilder<IoPort> immutableArrayBuilder = new ImmutableArrayBuilder<IoPort>(2);
      for (int index = 0; index < this.Ports.Length; ++index)
      {
        IoPort port = this.Ports[index];
        IoPort ioPort = new IoPort(this.Context.PortIdFactory.GetNextId(), (IEntityWithPorts) this, this.Ports[(index + 1) % 2].Spec, port.Position, port.Direction, (int) port.PortIndex);
        this.Context.IoPortsManager.DisconnectAndRemove(port);
        immutableArrayBuilder[index] = ioPort;
      }
      this.Ports = immutableArrayBuilder.GetImmutableArrayAndClear();
      foreach (IoPort port in this.Ports)
        this.Context.IoPortsManager.AddPortAndTryConnect(port);
      int count = this.m_outputBuffer.Count;
      for (int index = count - 1; index >= 0; --index)
      {
        ZipBuffProduct zipBuffProduct = this.m_outputBuffer[index];
        Duration duration = this.m_simLoopEvents.CurrentStep - zipBuffProduct.EnqueuedAtStep;
        this.m_outputBuffer.Enqueue(new ZipBuffProduct(zipBuffProduct.ProductQuantity, new SimStep((this.m_delay - duration).Ticks)));
      }
      for (int index = 0; index < count; ++index)
        this.m_outputBuffer.Dequeue();
      this.Prototype = liftProto;
      this.recomputeBufferSizeAndDelay();
      this.Context.EntitiesManager.InvokeOnEntityVisualChanged((IEntity) this);
      return true;
    }

    protected override void OnDestroy()
    {
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        this.Context.AssetTransactionManager.StoreClearedProduct(zipBuffProduct.ProductQuantity);
      this.m_outputBuffer.Clear();
      base.OnDestroy();
    }

    internal Option<string> VerifyStateCorrectness()
    {
      Quantity zero = Quantity.Zero;
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        zero += zipBuffProduct.ProductQuantity.Quantity;
      return zero != this.QuantityInOutputBuffer ? (Option<string>) string.Format("Quantity in the output buffer is {0} but cached value is {1}.", (object) zero, (object) this.QuantityInOutputBuffer) : Option<string>.None;
    }

    public static void Serialize(Lift value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Lift>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Lift.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      Duration.Serialize(this.m_consumePowerFor, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteBool(this.m_isMissingPower);
      Queueue<ZipBuffProduct>.Serialize(this.m_outputBuffer, writer);
      writer.WriteGeneric<LiftProto>(this.m_proto);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt(this.m_ticksLeft);
    }

    public static Lift Deserialize(BlobReader reader)
    {
      Lift lift;
      if (reader.TryStartClassDeserialization<Lift>(out lift))
        reader.EnqueueDataDeserialization((object) lift, Lift.s_deserializeDataDelayedAction);
      return lift;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.m_consumePowerFor = Duration.Deserialize(reader);
      reader.SetField<Lift>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_isMissingPower = reader.ReadBool();
      reader.SetField<Lift>(this, "m_outputBuffer", (object) Queueue<ZipBuffProduct>.Deserialize(reader));
      this.m_proto = reader.ReadGenericAs<LiftProto>();
      reader.SetField<Lift>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_ticksLeft = reader.ReadInt();
      reader.RegisterInitAfterLoad<Lift>(this, "initSelf", InitPriority.Normal);
    }

    static Lift()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Lift.BUFFER_SCALE = 150.Percent();
      Lift.DELAY_BASE = 1.Seconds();
      Lift.DELAY_PER_HEIGHT = 1.Seconds();
      Lift.CONSUME_POWER_FOR = 5.Seconds();
      Lift.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Lift.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
