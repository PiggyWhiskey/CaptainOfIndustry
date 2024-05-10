// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.MiniZipper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  /// <summary>
  /// Mini-zipper is simplified version of zipper. Since there are many of these in the factory the code is optimized
  /// and does not simply derive from a Zipper.
  /// </summary>
  /// <remarks>
  /// Mini-zipper does not use electricity to make is even more simple. Electricity is used by the connected
  /// transports which should be enough.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public sealed class MiniZipper : 
    LayoutEntityBase,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityWithSimUpdate
  {
    public readonly MiniZipperProto Prototype;
    private readonly ITransportsPredicates m_transportsPredicates;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TransportsManager m_transportsManager;
    private readonly Queueue<ZipBuffProduct> m_outputBuffer;
    private Quantity m_quantityInInputBuffer;
    private Quantity m_quantityInOutputBuffer;
    private Quantity m_maxBufferSize;
    private Duration m_delay;
    private int m_lastUsedInputPortIndex;
    private int m_lastUsedOutputPortIndex;
    private readonly ProductQuantity[] m_inputBuffer;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_connectedInputPortsCache;
    [DoNotSave(0, null)]
    private ImmutableArray<IoPortData> m_connectedOutputPortsCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => false;

    public ImmutableArray<IoPort> Ports { get; private set; }

    public Quantity TotalQuantityInBuffers
    {
      get => this.m_quantityInInputBuffer + this.m_quantityInOutputBuffer;
    }

    public Quantity MaxBufferSize
    {
      get => this.m_maxBufferSize + this.m_connectedInputPortsCache.Length.Quantity();
    }

    public int OutputPortsConnected => this.m_connectedOutputPortsCache.Length;

    public MiniZipper(
      EntityId id,
      MiniZipperProto proto,
      TileTransform transform,
      EntityContext context,
      ITransportsPredicates transportsPredicates,
      ISimLoopEvents simLoopEvents,
      TransportsManager transportsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_outputBuffer = new Queueue<ZipBuffProduct>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto.CheckNotNull<MiniZipperProto>();
      this.m_transportsPredicates = transportsPredicates;
      this.m_simLoopEvents = simLoopEvents;
      this.m_transportsManager = transportsManager;
      this.createPorts();
      this.m_inputBuffer = new ProductQuantity[this.Ports.Length];
      for (int index = 0; index < this.m_inputBuffer.Length; ++index)
        this.m_inputBuffer[index] = ProductQuantity.None;
      this.recomputePortInfo();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initialize() => this.recomputePortInfo();

    private void createPorts()
    {
      this.Ports = this.Prototype.Ports.Map<IoPort>((Func<IoPortTemplate, int, IoPort>) ((x, i) => IoPort.CreateFor<MiniZipper>(this.Context.PortIdFactory.GetNextId(), this, this.Prototype.Layout, this.Transform, x, i, new IoPortType?(IoPortType.Any))));
    }

    protected override void OnUpgradeDone(IEntityProto oldProto, IEntityProto newProto)
    {
      base.OnUpgradeDone(oldProto, newProto);
      foreach (IoPort port in this.Ports)
        this.Context.IoPortsManager.DisconnectAndRemove(port);
      this.createPorts();
      foreach (IoPort port in this.Ports)
        this.Context.IoPortsManager.AddPortAndTryConnect(port);
    }

    protected override void OnAddedToWorld(EntityAddReason reason)
    {
      base.OnAddedToWorld(reason);
      if (this.ConstructionState != ConstructionState.NotInitialized)
        return;
      foreach (IoPort port in this.Ports)
      {
        if (port.ConnectedPort.HasValue && port.ConnectedPort.Value.OwnerEntity.IsConstructed)
        {
          this.StartConstructionIfNotStarted();
          break;
        }
      }
    }

    public void OnPortConnectionChanged(IoPort ourPort, IoPort otherPort)
    {
      this.recomputePortInfo();
      if (!ourPort.IsNotConnected)
        return;
      int portIndex = (int) ourPort.PortIndex;
      if (!this.m_inputBuffer[portIndex].IsNotEmpty)
        return;
      this.moveInputToOutBuffer(portIndex);
    }

    private void recomputePortInfo()
    {
      ImmutableArray<IoPort> immutableArray = this.Ports;
      immutableArray = immutableArray.Filter((Predicate<IoPort>) (x => x.IsConnectedAsInput));
      this.m_connectedInputPortsCache = immutableArray.Map<IoPortData>((Func<IoPort, IoPortData>) (x => new IoPortData(x)));
      immutableArray = this.Ports;
      immutableArray = immutableArray.Filter((Predicate<IoPort>) (x => x.IsConnectedAsOutput));
      this.m_connectedOutputPortsCache = immutableArray.Map<IoPortData>((Func<IoPort, IoPortData>) (x => new IoPortData(x)));
      this.recomputeBufferSizeAndThresholds();
    }

    public void SimUpdate()
    {
      if (this.IsNotEnabled || this.m_connectedOutputPortsCache.IsEmpty)
        return;
      if (this.m_quantityInOutputBuffer < this.MaxBufferSize)
      {
        int num = 0;
        for (int length = this.m_connectedInputPortsCache.Length; num < length; ++num)
        {
          int index = (this.m_lastUsedInputPortIndex + 1) % length;
          this.m_lastUsedInputPortIndex = index;
          IoPortData ioPortData = this.m_connectedInputPortsCache[index];
          if (!this.m_inputBuffer[(int) ioPortData.PortIndex].IsEmpty)
          {
            this.moveInputToOutBuffer((int) ioPortData.PortIndex);
            if (this.m_quantityInOutputBuffer >= this.MaxBufferSize)
              break;
          }
        }
      }
      if (this.m_quantityInOutputBuffer.IsNotPositive)
        return;
      this.tryReleaseFirstProduct();
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled)
      {
        if (this.ConstructionState == ConstructionState.NotInitialized)
        {
          Log.Warning("Mini-zipper was not constructed when it received products.");
          this.StartConstructionIfNotStarted();
          Assert.That<ConstructionState>(this.ConstructionState).IsNotEqualTo<ConstructionState>(ConstructionState.NotInitialized);
        }
        return pq.Quantity;
      }
      if (this.m_inputBuffer[(int) sourcePort.PortIndex].IsNotEmpty)
        return pq.Quantity;
      this.m_inputBuffer[(int) sourcePort.PortIndex] = pq;
      this.m_quantityInInputBuffer += pq.Quantity;
      return Quantity.Zero;
    }

    private void moveInputToOutBuffer(int index)
    {
      ProductQuantity productQuantity = this.m_inputBuffer[index];
      this.m_quantityInInputBuffer -= productQuantity.Quantity;
      this.m_quantityInOutputBuffer += productQuantity.Quantity;
      this.m_inputBuffer[index] = ProductQuantity.None;
      if (this.m_outputBuffer.IsNotEmpty)
      {
        ZipBuffProduct last = this.m_outputBuffer.Last;
        if (last.EnqueuedAtStep == this.m_simLoopEvents.CurrentStep && (Proto) last.ProductQuantity.Product == (Proto) productQuantity.Product)
        {
          this.m_outputBuffer.PopLast();
          productQuantity = productQuantity.WithNewQuantity(last.ProductQuantity.Quantity + productQuantity.Quantity);
        }
      }
      this.m_outputBuffer.Enqueue(new ZipBuffProduct(productQuantity, this.m_simLoopEvents.CurrentStep));
    }

    internal void PushProductsToBuffer(ProductQuantity pq)
    {
      this.m_outputBuffer.Enqueue(new ZipBuffProduct(pq, this.m_simLoopEvents.CurrentStep));
    }

    private void tryReleaseFirstProduct()
    {
      if (this.m_outputBuffer.IsEmpty)
      {
        Log.Error(string.Format("Invalid state, m_outputBuffer is empty but quantityInOutputBuffer is {0}", (object) this.m_quantityInOutputBuffer));
        this.m_quantityInOutputBuffer = Quantity.Zero;
      }
      else
      {
        ZipBuffProduct zipBuffProduct = this.m_outputBuffer.Peek();
        if (this.m_simLoopEvents.CurrentStep - zipBuffProduct.EnqueuedAtStep < this.m_delay)
          return;
        ImmutableArray<IoPortData> outputPortsCache = this.m_connectedOutputPortsCache;
        ProductQuantity productQuantity = zipBuffProduct.ProductQuantity;
        int num = 0;
        for (int length = outputPortsCache.Length; num < length; ++num)
        {
          int index = (this.m_lastUsedOutputPortIndex + 1) % length;
          this.m_lastUsedOutputPortIndex = index;
          outputPortsCache[index].SendAsMuchAs(ref productQuantity);
          if (productQuantity.IsEmpty)
          {
            this.m_quantityInOutputBuffer -= zipBuffProduct.ProductQuantity.Quantity;
            this.m_outputBuffer.Dequeue();
            return;
          }
        }
        Assert.That<Quantity>(productQuantity.Quantity).IsLessOrEqual(zipBuffProduct.ProductQuantity.Quantity);
        if (!(productQuantity.Quantity < zipBuffProduct.ProductQuantity.Quantity))
          return;
        this.m_outputBuffer.GetRefFirst() = new ZipBuffProduct(productQuantity, zipBuffProduct.EnqueuedAtStep);
        this.m_quantityInOutputBuffer -= zipBuffProduct.ProductQuantity.Quantity - productQuantity.Quantity;
      }
    }

    private void recomputeBufferSizeAndThresholds()
    {
      PartialQuantity zero1 = PartialQuantity.Zero;
      PartialQuantity zero2 = PartialQuantity.Zero;
      foreach (IoPort port in this.Ports)
      {
        if (port.IsConnected)
        {
          PartialQuantity throughputPerTick = port.GetMaxThroughputPerTick();
          if (port.IsConnectedAsInput)
            zero1 += throughputPerTick;
          else if (port.IsConnectedAsOutput)
            zero2 += throughputPerTick;
          else
            Log.Error("Port connected but is not input or output.");
        }
      }
      PartialQuantity partialQuantity = zero1.Min(zero2);
      this.m_delay = !(partialQuantity.Value <= Fix32.One) ? (!(partialQuantity.Value <= Fix32.Three) ? 2.Ticks() : 5.Ticks()) : 10.Ticks();
      this.m_maxBufferSize = (3 * this.m_delay.Ticks / 2 * partialQuantity).ToQuantityCeiled();
    }

    public void GetAllBufferedProducts(
      Lyst<KeyValuePair<ProductProto, Quantity>> aggregatedProducts)
    {
      foreach (ProductQuantity pq in this.m_inputBuffer)
      {
        if (pq.IsNotEmpty)
          aggregate(pq);
      }
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

    protected override void OnDestroy()
    {
      IAssetTransactionManager transactionManager = this.Context.AssetTransactionManager;
      for (int index = 0; index < this.m_inputBuffer.Length; ++index)
      {
        transactionManager.StoreClearedProduct(this.m_inputBuffer[index]);
        this.m_inputBuffer[index] = ProductQuantity.None;
      }
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
      {
        if (zipBuffProduct.ProductQuantity.IsNotEmpty)
          transactionManager.StoreClearedProduct(zipBuffProduct.ProductQuantity);
      }
      this.m_outputBuffer.Clear();
      base.OnDestroy();
    }

    public static void Serialize(MiniZipper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MiniZipper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MiniZipper.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Duration.Serialize(this.m_delay, writer);
      writer.WriteArray<ProductQuantity>(this.m_inputBuffer);
      writer.WriteInt(this.m_lastUsedInputPortIndex);
      writer.WriteInt(this.m_lastUsedOutputPortIndex);
      Quantity.Serialize(this.m_maxBufferSize, writer);
      Queueue<ZipBuffProduct>.Serialize(this.m_outputBuffer, writer);
      Quantity.Serialize(this.m_quantityInInputBuffer, writer);
      Quantity.Serialize(this.m_quantityInOutputBuffer, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TransportsManager.Serialize(this.m_transportsManager, writer);
      writer.WriteGeneric<ITransportsPredicates>(this.m_transportsPredicates);
      ImmutableArray<IoPort>.Serialize(this.Ports, writer);
      writer.WriteGeneric<MiniZipperProto>(this.Prototype);
    }

    public static MiniZipper Deserialize(BlobReader reader)
    {
      MiniZipper miniZipper;
      if (reader.TryStartClassDeserialization<MiniZipper>(out miniZipper))
        reader.EnqueueDataDeserialization((object) miniZipper, MiniZipper.s_deserializeDataDelayedAction);
      return miniZipper;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_delay = Duration.Deserialize(reader);
      reader.SetField<MiniZipper>(this, "m_inputBuffer", (object) reader.ReadArray<ProductQuantity>());
      this.m_lastUsedInputPortIndex = reader.ReadInt();
      this.m_lastUsedOutputPortIndex = reader.ReadInt();
      this.m_maxBufferSize = Quantity.Deserialize(reader);
      reader.SetField<MiniZipper>(this, "m_outputBuffer", (object) Queueue<ZipBuffProduct>.Deserialize(reader));
      this.m_quantityInInputBuffer = Quantity.Deserialize(reader);
      this.m_quantityInOutputBuffer = Quantity.Deserialize(reader);
      reader.SetField<MiniZipper>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<MiniZipper>(this, "m_transportsManager", (object) TransportsManager.Deserialize(reader));
      reader.SetField<MiniZipper>(this, "m_transportsPredicates", (object) reader.ReadGenericAs<ITransportsPredicates>());
      this.Ports = ImmutableArray<IoPort>.Deserialize(reader);
      reader.SetField<MiniZipper>(this, "Prototype", (object) reader.ReadGenericAs<MiniZipperProto>());
      reader.RegisterInitAfterLoad<MiniZipper>(this, "initialize", InitPriority.Normal);
    }

    static MiniZipper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MiniZipper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      MiniZipper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
