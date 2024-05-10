// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.Zipper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  [GenerateSerializer(false, null, 0)]
  public class Zipper : 
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
    public const int BUFFER_SCALE = 2;
    public static readonly Duration MAX_DELAY;
    public static readonly Duration MEDIUM_DELAY;
    public static readonly Duration MIN_DELAY;
    private static readonly Duration CONSUME_POWER_FOR;
    public readonly ZipperProto Prototype;
    private readonly ISimLoopEvents m_simLoopEvents;
    private bool m_isMissingPower;
    private Duration m_consumePowerFor;
    private Duration m_delay;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    /// <summary>
    /// Input buffer that can accept one product per port to make input prioritization logic possible.
    /// Indexed by port index.
    /// </summary>
    private readonly ProductQuantity[] m_inputBuffer;
    private int m_previousPriorityInputPortIndex;
    private int m_previousNonPriorityInputPortIndex;
    [DoNotSave(0, null)]
    private Option<int[]> m_priorityInputPortIndicesCache;
    [DoNotSave(0, null)]
    private Option<int[]> m_nonPriorityInputPortIndicesCache;
    private Option<QuantityLarge[]> m_inputCounts;
    [DoNotSave(0, null)]
    private int? m_minCountInputPortIndexCache;
    /// <summary>Internal buffer of products waiting for output.</summary>
    /// <remarks>
    /// |====================|====================|====================|
    /// |              any input allowed          |priority input only |
    /// |priority output only|           any output allowed            |
    /// 
    /// |»                  «|
    /// When buffer is in this range, any input is allowed (priority ports are preferred) but only priority output
    /// is allowed. This happens when there are not enough inputs. This range must be big enough to handle
    /// full throughput of priority output ports to no leak products to non-priority ports.
    /// 
    ///                      |»                  «|
    /// In the mid range, any inputs and outputs are allowed (priority ports are preferred). This happens when
    /// priority outputs cannot deal with the amount of products so non-priority ports output are being used.
    /// This range needs to be big enough to handle throughput of non-priority input and output ports
    /// 
    ///                                           |»                  «|
    /// In the last range, buffer is too full and only priority inputs are allowed. This happens when too many
    /// products are flowing in the buffer. This range has to be large enough to allow priority input
    /// throughput.
    /// </remarks>
    private readonly Queueue<ZipBuffProduct> m_outputBuffer;
    private Quantity m_onlyPriorityInputsAboveThreshold;
    private Quantity m_onlyPriorityOutputsBelowThreshold;
    private int m_previousPriorityOutputPortIndexCache;
    private int m_previousNonPriorityOutputPortIndexCache;
    [DoNotSave(0, null)]
    private Option<int[]> m_priorityOutputPortIndices;
    [DoNotSave(0, null)]
    private Option<int[]> m_nonPriorityOutputPortIndices;
    private Option<QuantityLarge[]> m_outputCounts;
    [DoNotSave(0, null)]
    private int? m_minCountOutputPortIndexCache;
    private readonly IElectricityConsumer m_electricityConsumer;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private readonly bool[] m_isPortPrioritized;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<IoPortData> m_allPorts;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public Quantity TotalQuantityInBuffers
    {
      get => this.QuantityInInputBuffer + this.QuantityInOutputBuffer;
    }

    public Quantity QuantityInInputBuffer { get; private set; }

    /// <summary>
    /// When set, all connected inputs will accept same amounts of products.
    /// </summary>
    public bool ForceEvenInputs { get; private set; }

    public Quantity QuantityInOutputBuffer { get; private set; }

    public Quantity MaxBufferSize { get; private set; }

    /// <summary>
    /// When set, all connected outputs will receive the same amounts of products.
    /// </summary>
    public bool ForceEvenOutputs { get; private set; }

    public bool IsEmpty => (this.QuantityInInputBuffer + this.QuantityInOutputBuffer).IsNotPositive;

    public override bool IsGeneralPriorityVisible => !this.m_canWorkOnLowPower;

    Electricity IElectricityConsumingEntity.PowerRequired
    {
      get => !this.m_ignorePower ? this.Prototype.ElectricityConsumed : Electricity.Zero;
    }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    public ReadOnlyArray<bool> IsPortPrioritizedArray
    {
      get => this.m_isPortPrioritized.AsReadOnlyArray<bool>();
    }

    protected override IoPortType? PortTypeOverride => new IoPortType?(IoPortType.Any);

    public Zipper(
      EntityId id,
      ZipperProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_outputBuffer = new Queueue<ZipBuffProduct>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_simLoopEvents = simLoopEvents;
      this.m_isPortPrioritized = new bool[this.Ports.Length];
      this.m_inputBuffer = new ProductQuantity[this.Ports.Length];
      for (int index = 0; index < this.m_inputBuffer.Length; ++index)
        this.m_inputBuffer[index] = ProductQuantity.None;
      this.updateProperties();
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.recreateFastPorts();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.updateProperties();

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

    private void recreateFastPorts()
    {
      this.m_allPorts.Clear();
      foreach (IoPort port in this.Ports)
        this.m_allPorts.Add(new IoPortData(port));
    }

    protected override void OnPortsLoadOrChange()
    {
      base.OnPortsLoadOrChange();
      this.recreateFastPorts();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled)
        return;
      Quantity quantity;
      while (true)
      {
        quantity = this.QuantityInOutputBuffer;
        if (quantity.IsPositive && this.tryReleaseFirstProduct())
          this.m_consumePowerFor = Zipper.CONSUME_POWER_FOR;
        else
          break;
      }
      quantity = this.QuantityInInputBuffer;
      if (quantity.IsPositive && (this.m_electricityConsumer.CanConsume(this.m_canWorkOnLowPower) || this.m_canWorkOnLowPower) && this.processInputProducts())
        this.m_consumePowerFor = Zipper.CONSUME_POWER_FOR;
      if (!this.m_consumePowerFor.IsPositive)
        return;
      this.m_isMissingPower = !this.m_electricityConsumer.TryConsume(this.m_canWorkOnLowPower);
      if (this.m_isMissingPower)
        return;
      this.m_consumePowerFor -= Duration.OneTick;
    }

    protected override void OnPortConnectionChanged(IoPort ourPort)
    {
      base.OnPortConnectionChanged(ourPort);
      this.m_priorityInputPortIndicesCache = Option<int[]>.None;
      this.m_nonPriorityInputPortIndicesCache = Option<int[]>.None;
      this.m_inputCounts = Option<QuantityLarge[]>.None;
      this.m_minCountInputPortIndexCache = new int?();
      this.m_priorityOutputPortIndices = Option<int[]>.None;
      this.m_nonPriorityOutputPortIndices = Option<int[]>.None;
      this.m_outputCounts = Option<QuantityLarge[]>.None;
      this.m_minCountOutputPortIndexCache = new int?();
      this.recomputeBufferSizeAndThresholds();
      if (!ourPort.IsNotConnected)
        return;
      int portIndex = (int) ourPort.PortIndex;
      if (!this.m_inputBuffer[portIndex].IsNotEmpty)
        return;
      this.moveInputToOutBuffer(portIndex);
    }

    /// <summary>
    /// Recomputes buffer size and thresholds based on the current port connections and priorities to have as
    /// small buffer as possible but not affect correct operation and throughput.
    /// </summary>
    private void recomputeBufferSizeAndThresholds()
    {
      PartialQuantity zero1 = PartialQuantity.Zero;
      PartialQuantity zero2 = PartialQuantity.Zero;
      PartialQuantity zero3 = PartialQuantity.Zero;
      PartialQuantity zero4 = PartialQuantity.Zero;
      foreach (IoPort port in this.Ports)
      {
        if (port.IsConnected)
        {
          PartialQuantity throughputPerTick = port.GetMaxThroughputPerTick();
          if (port.IsConnectedAsInput)
          {
            if (this.m_isPortPrioritized[(int) port.PortIndex])
              zero1 += throughputPerTick;
            else
              zero2 += throughputPerTick;
          }
          else if (port.IsConnectedAsOutput)
          {
            if (this.m_isPortPrioritized[(int) port.PortIndex])
              zero3 += throughputPerTick;
            else
              zero4 += throughputPerTick;
          }
          else
            Log.Error("Port connected but is not input or output.");
        }
      }
      PartialQuantity partialQuantity1 = (zero1 + zero2).Max(zero3 + zero4);
      this.m_delay = !(partialQuantity1.Value <= 0.5.ToFix32()) ? (!(partialQuantity1.Value <= 1.ToFix32()) ? Zipper.MIN_DELAY : Zipper.MEDIUM_DELAY) : Zipper.MAX_DELAY;
      int num = 2 * this.m_delay.Ticks;
      this.m_onlyPriorityOutputsBelowThreshold = (num * zero3).ToQuantityCeiled();
      PartialQuantity partialQuantity2 = num * zero4.Max(zero2);
      this.m_onlyPriorityInputsAboveThreshold = this.m_onlyPriorityOutputsBelowThreshold + partialQuantity2.ToQuantityCeiled();
      Quantity inputsAboveThreshold = this.m_onlyPriorityInputsAboveThreshold;
      partialQuantity2 = num * zero1;
      Quantity quantityCeiled = partialQuantity2.ToQuantityCeiled();
      this.MaxBufferSize = inputsAboveThreshold + quantityCeiled;
    }

    /// <summary>
    /// Returns whether requested port name was found and priority was set.
    /// </summary>
    public bool TrySetPortPriorityForPort(char portName, bool? isPrioritized)
    {
      int index = this.Ports.IndexOf((Predicate<IoPort>) (p => (int) p.Name == (int) portName));
      if (index < 0)
        return false;
      bool flag = ((int) isPrioritized ?? (!this.m_isPortPrioritized[index] ? 1 : 0)) != 0;
      if (this.m_isPortPrioritized[index] == flag)
        return true;
      this.m_isPortPrioritized[index] = flag;
      IoPort port = this.Ports[index];
      if (port.IsConnected)
      {
        if (port.IsConnectedAsInput)
        {
          this.m_priorityInputPortIndicesCache = Option<int[]>.None;
          this.m_nonPriorityInputPortIndicesCache = Option<int[]>.None;
          this.m_inputCounts = Option<QuantityLarge[]>.None;
          this.m_minCountInputPortIndexCache = new int?();
          this.ForceEvenInputs = false;
        }
        else if (port.IsConnectedAsOutput)
        {
          this.m_priorityOutputPortIndices = Option<int[]>.None;
          this.m_nonPriorityOutputPortIndices = Option<int[]>.None;
          this.m_outputCounts = Option<QuantityLarge[]>.None;
          this.m_minCountOutputPortIndexCache = new int?();
          this.ForceEvenOutputs = false;
        }
      }
      this.recomputeBufferSizeAndThresholds();
      return true;
    }

    public void SetForceEvenInputs(bool forceEvenInputs)
    {
      if (this.ForceEvenInputs == forceEvenInputs)
        return;
      this.ForceEvenInputs = forceEvenInputs;
      this.m_inputCounts = Option<QuantityLarge[]>.None;
      this.m_minCountInputPortIndexCache = new int?();
      this.m_priorityInputPortIndicesCache = Option<int[]>.None;
      this.m_nonPriorityInputPortIndicesCache = Option<int[]>.None;
      if (!forceEvenInputs)
        return;
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<IoPort> ports = this.Ports;
        int length = ports.Length;
        if (num < length)
        {
          ports = this.Ports;
          if (ports[index].IsConnectedAsInput)
            this.m_isPortPrioritized[index] = false;
          ++index;
        }
        else
          break;
      }
    }

    public void SetForceEvenOutputs(bool forceEvenOutputs)
    {
      if (this.ForceEvenOutputs == forceEvenOutputs)
        return;
      this.ForceEvenOutputs = forceEvenOutputs;
      this.m_outputCounts = Option<QuantityLarge[]>.None;
      this.m_minCountOutputPortIndexCache = new int?();
      this.m_priorityOutputPortIndices = Option<int[]>.None;
      this.m_nonPriorityOutputPortIndices = Option<int[]>.None;
      if (!forceEvenOutputs)
        return;
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<IoPort> ports = this.Ports;
        int length = ports.Length;
        if (num < length)
        {
          ports = this.Ports;
          if (ports[index].IsConnectedAsOutput)
            this.m_isPortPrioritized[index] = false;
          ++index;
        }
        else
          break;
      }
    }

    internal void PushProductsToBuffer(ProductQuantity pq)
    {
      this.m_outputBuffer.Enqueue(new ZipBuffProduct(pq, this.m_simLoopEvents.CurrentStep));
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || this.m_inputBuffer[(int) sourcePort.PortIndex].IsNotEmpty || this.m_isMissingPower && !this.m_canWorkOnLowPower)
        return pq.Quantity;
      this.m_inputBuffer[(int) sourcePort.PortIndex] = pq;
      this.QuantityInInputBuffer += pq.Quantity;
      return Quantity.Zero;
    }

    private void moveInputToOutBuffer(int index)
    {
      ProductQuantity productQuantity = this.m_inputBuffer[index];
      this.QuantityInInputBuffer -= productQuantity.Quantity;
      this.QuantityInOutputBuffer += productQuantity.Quantity;
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

    private bool processInputProducts()
    {
      if (this.QuantityInInputBuffer.IsNotPositive || this.QuantityInOutputBuffer >= this.MaxBufferSize)
        return false;
      int[] numArray1;
      int[] numArray2;
      if (this.m_priorityInputPortIndicesCache.HasValue)
      {
        numArray1 = this.m_priorityInputPortIndicesCache.Value;
        numArray2 = this.m_nonPriorityInputPortIndicesCache.Value;
      }
      else
      {
        this.m_priorityInputPortIndicesCache = (Option<int[]>) (numArray1 = this.ForceEvenInputs ? Array.Empty<int>() : this.Ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsInput && this.m_isPortPrioritized[(int) x.PortIndex])));
        this.m_nonPriorityInputPortIndicesCache = (Option<int[]>) (numArray2 = this.ForceEvenInputs ? this.Ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsInput)) : this.Ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsInput && !this.m_isPortPrioritized[(int) x.PortIndex])));
      }
      bool flag = false;
      if (numArray1.Length != 0)
      {
        int num = 0;
        for (int length = numArray1.Length; num < length; ++num)
        {
          int index1 = (this.m_previousPriorityInputPortIndex + 1) % length;
          this.m_previousPriorityInputPortIndex = index1;
          int index2 = numArray1[index1];
          if (this.m_inputBuffer[index2].IsNotEmpty)
          {
            this.moveInputToOutBuffer(index2);
            if (this.QuantityInOutputBuffer >= this.MaxBufferSize)
              return true;
            flag = true;
          }
        }
      }
      if (numArray2.Length == 0 || this.QuantityInOutputBuffer >= this.m_onlyPriorityInputsAboveThreshold)
        return flag;
      if (this.ForceEvenInputs)
      {
        QuantityLarge[] quantityLargeArray;
        if (this.m_inputCounts.HasValue)
          quantityLargeArray = this.m_inputCounts.Value;
        else
          this.m_inputCounts = (Option<QuantityLarge[]>) (quantityLargeArray = new QuantityLarge[numArray2.Length]);
        int num1 = 0;
        for (int length1 = numArray2.Length; num1 < length1; ++num1)
        {
          if (!this.m_minCountInputPortIndexCache.HasValue)
          {
            QuantityLarge quantityLarge1 = quantityLargeArray[0];
            int num2 = 0;
            int index = 1;
            for (int length2 = quantityLargeArray.Length; index < length2; ++index)
            {
              QuantityLarge quantityLarge2 = quantityLargeArray[index];
              if (quantityLarge2 < quantityLarge1)
              {
                quantityLarge1 = quantityLarge2;
                num2 = index;
              }
            }
            this.m_minCountInputPortIndexCache = new int?(num2);
          }
          int index3 = numArray2[this.m_minCountInputPortIndexCache.Value];
          if (this.m_inputBuffer[index3].IsNotEmpty)
          {
            quantityLargeArray[this.m_minCountInputPortIndexCache.Value] += this.m_inputBuffer[index3].Quantity;
            this.m_minCountInputPortIndexCache = new int?();
            this.moveInputToOutBuffer(index3);
            if (this.QuantityInOutputBuffer >= this.m_onlyPriorityInputsAboveThreshold)
              return true;
            flag = true;
          }
        }
      }
      else
      {
        int num = 0;
        for (int length = numArray2.Length; num < length; ++num)
        {
          int index4 = (this.m_previousNonPriorityInputPortIndex + 1) % length;
          this.m_previousNonPriorityInputPortIndex = index4;
          int index5 = numArray2[index4];
          if (this.m_inputBuffer[index5].IsNotEmpty)
          {
            this.moveInputToOutBuffer(index5);
            if (this.QuantityInOutputBuffer >= this.m_onlyPriorityInputsAboveThreshold)
              return true;
            flag = true;
          }
        }
      }
      return flag;
    }

    private bool tryReleaseFirstProduct()
    {
      if (this.m_outputBuffer.IsEmpty)
      {
        Log.Warning("Noting to release from zipper.");
        if (this.QuantityInOutputBuffer != Quantity.Zero)
        {
          Log.Error(string.Format("Invalid quantity in output buffer, should be zero but is {0}.", (object) this.QuantityInOutputBuffer));
          this.QuantityInOutputBuffer = Quantity.Zero;
        }
        return false;
      }
      if (this.m_simLoopEvents.CurrentStep - this.m_outputBuffer.Peek().EnqueuedAtStep < this.m_delay)
        return false;
      LystStruct<IoPortData> allPorts = this.m_allPorts;
      int[] numArray1;
      int[] numArray2;
      if (this.m_priorityOutputPortIndices.HasValue)
      {
        numArray1 = this.m_priorityOutputPortIndices.Value;
        numArray2 = this.m_nonPriorityOutputPortIndices.Value;
      }
      else
      {
        ImmutableArray<IoPort> ports;
        int[] numArray3;
        if (!this.ForceEvenOutputs)
        {
          ports = this.Ports;
          numArray3 = ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsOutput && this.m_isPortPrioritized[(int) x.PortIndex]));
        }
        else
          numArray3 = Array.Empty<int>();
        numArray1 = numArray3;
        this.m_priorityOutputPortIndices = (Option<int[]>) numArray3;
        int[] indicesArray;
        if (!this.ForceEvenOutputs)
        {
          ports = this.Ports;
          indicesArray = ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsOutput && !this.m_isPortPrioritized[(int) x.PortIndex]));
        }
        else
        {
          ports = this.Ports;
          indicesArray = ports.GetIndicesArray((Predicate<IoPort>) (x => x.IsConnectedAsOutput));
        }
        numArray2 = indicesArray;
        this.m_nonPriorityOutputPortIndices = (Option<int[]>) indicesArray;
      }
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
      if (numArray1.Length != 0)
      {
        int num = 0;
        for (int length = numArray1.Length; num < length; ++num)
        {
          int index1 = (this.m_previousPriorityOutputPortIndexCache + 1) % length;
          this.m_previousPriorityOutputPortIndexCache = index1;
          int index2 = numArray1[index1];
          Quantity quantity = productQuantity.Quantity;
          this.m_allPorts[index2].SendAsMuchAs(ref productQuantity);
          this.QuantityInOutputBuffer -= quantity - productQuantity.Quantity;
          if (productQuantity.IsEmpty)
            return true;
        }
      }
      if (numArray2.Length == 0 || this.QuantityInOutputBuffer <= this.m_onlyPriorityOutputsBelowThreshold)
      {
        this.m_outputBuffer.EnqueueFirst(new ZipBuffProduct(productQuantity, zipBuffProduct1.EnqueuedAtStep));
        return false;
      }
      bool flag = false;
      if (this.ForceEvenOutputs)
      {
        QuantityLarge[] quantityLargeArray;
        if (this.m_outputCounts.HasValue)
          quantityLargeArray = this.m_outputCounts.Value;
        else
          this.m_outputCounts = (Option<QuantityLarge[]>) (quantityLargeArray = new QuantityLarge[numArray2.Length]);
        if (!this.m_minCountOutputPortIndexCache.HasValue)
        {
          QuantityLarge quantityLarge1 = quantityLargeArray[0];
          int num = 0;
          int index = 1;
          for (int length = quantityLargeArray.Length; index < length; ++index)
          {
            QuantityLarge quantityLarge2 = quantityLargeArray[index];
            if (quantityLarge2 < quantityLarge1)
            {
              quantityLarge1 = quantityLarge2;
              num = index;
            }
          }
          this.m_minCountOutputPortIndexCache = new int?(num);
        }
        int index3 = numArray2[this.m_minCountOutputPortIndexCache.Value];
        Quantity quantity1 = productQuantity.Quantity;
        allPorts[index3].SendAsMuchAs(ref productQuantity);
        Quantity quantity2 = quantity1 - productQuantity.Quantity;
        if (quantity2.IsPositive)
        {
          this.QuantityInOutputBuffer -= quantity2;
          quantityLargeArray[this.m_minCountOutputPortIndexCache.Value] += quantity2;
          this.m_minCountOutputPortIndexCache = new int?();
          flag = true;
          if (productQuantity.IsEmpty)
            return true;
        }
      }
      else
      {
        int num = 0;
        for (int length = numArray2.Length; num < length; ++num)
        {
          int index4 = (this.m_previousNonPriorityOutputPortIndexCache + 1) % length;
          this.m_previousNonPriorityOutputPortIndexCache = index4;
          int index5 = numArray2[index4];
          Quantity quantity = productQuantity.Quantity;
          allPorts[index5].SendAsMuchAs(ref productQuantity);
          this.QuantityInOutputBuffer -= quantity - productQuantity.Quantity;
          if (productQuantity.IsEmpty)
            return true;
        }
      }
      this.m_outputBuffer.EnqueueFirst(new ZipBuffProduct(productQuantity, zipBuffProduct1.EnqueuedAtStep));
      return flag;
    }

    protected override void OnDestroy()
    {
      for (int index = 0; index < this.m_inputBuffer.Length; ++index)
      {
        this.Context.AssetTransactionManager.StoreClearedProduct(this.m_inputBuffer[index]);
        this.m_inputBuffer[index] = ProductQuantity.None;
      }
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        this.Context.AssetTransactionManager.StoreClearedProduct(zipBuffProduct.ProductQuantity);
      this.m_outputBuffer.Clear();
      base.OnDestroy();
    }

    internal Option<string> VerifyStateCorrectness()
    {
      Quantity zero1 = Quantity.Zero;
      foreach (ProductQuantity productQuantity in this.m_inputBuffer)
        zero1 += productQuantity.Quantity;
      if (zero1 != this.QuantityInInputBuffer)
        return (Option<string>) string.Format("Quantity in the input buffer is {0} but cached value is {1}.", (object) zero1, (object) this.QuantityInInputBuffer);
      Quantity zero2 = Quantity.Zero;
      foreach (ZipBuffProduct zipBuffProduct in this.m_outputBuffer)
        zero2 += zipBuffProduct.ProductQuantity.Quantity;
      return zero2 != this.QuantityInOutputBuffer ? (Option<string>) string.Format("Quantity in the output buffer is {0} but cached value is {1}.", (object) zero2, (object) this.QuantityInOutputBuffer) : Option<string>.None;
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      if (this.ForceEvenInputs)
        data.SetForceEvenInputs(this.ForceEvenInputs);
      if (this.ForceEvenOutputs)
        data.SetForceEvenOutputs(this.ForceEvenOutputs);
      data.SetPrioritizedPorts(((ICollection<bool>) this.m_isPortPrioritized).ToImmutableArray<bool>());
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      this.SetForceEvenInputs(data.GetForceEvenInputs().GetValueOrDefault());
      this.SetForceEvenOutputs(data.GetForceEvenOutputs().GetValueOrDefault());
      ImmutableArray<bool>? prioritizedPorts = data.GetPrioritizedPorts();
      if (!prioritizedPorts.HasValue)
        return;
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<bool> immutableArray = prioritizedPorts.Value;
        int length = immutableArray.Length;
        if (num < length && index < this.Ports.Length)
        {
          int name = (int) this.Ports[index].Name;
          immutableArray = prioritizedPorts.Value;
          bool? isPrioritized = new bool?(immutableArray[index]);
          this.TrySetPortPriorityForPort((char) name, isPrioritized);
          ++index;
        }
        else
          break;
      }
    }

    public static void Serialize(Zipper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Zipper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Zipper.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.ForceEvenInputs);
      writer.WriteBool(this.ForceEvenOutputs);
      Duration.Serialize(this.m_consumePowerFor, writer);
      Duration.Serialize(this.m_delay, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteArray<ProductQuantity>(this.m_inputBuffer);
      Option<QuantityLarge[]>.Serialize(this.m_inputCounts, writer);
      writer.WriteBool(this.m_isMissingPower);
      writer.WriteArray<bool>(this.m_isPortPrioritized);
      Quantity.Serialize(this.m_onlyPriorityInputsAboveThreshold, writer);
      Quantity.Serialize(this.m_onlyPriorityOutputsBelowThreshold, writer);
      Queueue<ZipBuffProduct>.Serialize(this.m_outputBuffer, writer);
      Option<QuantityLarge[]>.Serialize(this.m_outputCounts, writer);
      writer.WriteInt(this.m_previousNonPriorityInputPortIndex);
      writer.WriteInt(this.m_previousNonPriorityOutputPortIndexCache);
      writer.WriteInt(this.m_previousPriorityInputPortIndex);
      writer.WriteInt(this.m_previousPriorityOutputPortIndexCache);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      Quantity.Serialize(this.MaxBufferSize, writer);
      writer.WriteGeneric<ZipperProto>(this.Prototype);
      Quantity.Serialize(this.QuantityInInputBuffer, writer);
      Quantity.Serialize(this.QuantityInOutputBuffer, writer);
    }

    public static Zipper Deserialize(BlobReader reader)
    {
      Zipper zipper;
      if (reader.TryStartClassDeserialization<Zipper>(out zipper))
        reader.EnqueueDataDeserialization((object) zipper, Zipper.s_deserializeDataDelayedAction);
      return zipper;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ForceEvenInputs = reader.ReadBool();
      this.ForceEvenOutputs = reader.ReadBool();
      this.m_allPorts = new LystStruct<IoPortData>();
      this.m_consumePowerFor = Duration.Deserialize(reader);
      this.m_delay = Duration.Deserialize(reader);
      reader.SetField<Zipper>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<Zipper>(this, "m_inputBuffer", (object) reader.ReadArray<ProductQuantity>());
      this.m_inputCounts = Option<QuantityLarge[]>.Deserialize(reader);
      this.m_isMissingPower = reader.ReadBool();
      reader.SetField<Zipper>(this, "m_isPortPrioritized", (object) reader.ReadArray<bool>());
      this.m_onlyPriorityInputsAboveThreshold = Quantity.Deserialize(reader);
      this.m_onlyPriorityOutputsBelowThreshold = Quantity.Deserialize(reader);
      reader.SetField<Zipper>(this, "m_outputBuffer", (object) Queueue<ZipBuffProduct>.Deserialize(reader));
      this.m_outputCounts = Option<QuantityLarge[]>.Deserialize(reader);
      this.m_previousNonPriorityInputPortIndex = reader.ReadInt();
      this.m_previousNonPriorityOutputPortIndexCache = reader.ReadInt();
      this.m_previousPriorityInputPortIndex = reader.ReadInt();
      this.m_previousPriorityOutputPortIndexCache = reader.ReadInt();
      reader.SetField<Zipper>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.MaxBufferSize = Quantity.Deserialize(reader);
      reader.SetField<Zipper>(this, "Prototype", (object) reader.ReadGenericAs<ZipperProto>());
      this.QuantityInInputBuffer = Quantity.Deserialize(reader);
      this.QuantityInOutputBuffer = Quantity.Deserialize(reader);
      reader.RegisterInitAfterLoad<Zipper>(this, "initSelf", InitPriority.Normal);
    }

    static Zipper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Zipper.MAX_DELAY = 15.Ticks();
      Zipper.MEDIUM_DELAY = 10.Ticks();
      Zipper.MIN_DELAY = 5.Ticks();
      Zipper.CONSUME_POWER_FOR = 1.Seconds();
      Zipper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Zipper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
