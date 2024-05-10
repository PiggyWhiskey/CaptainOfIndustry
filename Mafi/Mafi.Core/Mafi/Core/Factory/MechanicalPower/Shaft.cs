// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.MechanicalPower.Shaft
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Ports;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.MechanicalPower
{
  /// <summary>
  /// Represents a physical shaft with inertial equal to sum inertia of all connected components.
  /// 
  /// Shaft has a max throughput <see cref="F:Mafi.Core.Factory.MechanicalPower.ShaftManager.MAX_SHAFT_THROUGHPUT" />.
  /// </summary>
  /// <remarks>
  /// In order to make shaft input and output self-balancing, input and output is dynamically scaled and turned
  /// on/off based on total amount of stored power (inertia).
  /// 
  /// When there is less than 15% inertia, shaft output is turned off and only input is allowed. This allows the shaft
  /// to gain inertia all the way until 50% where output is turned off. To prevent constant oscillations between
  /// 15% and 50% when there is more demand than production, output is scaled based on the total inertia, being
  /// 25% at 15% of inertia, and 100% at 65% inertia.
  /// 
  /// Similar technique is used for input balancing, see diagram describing all the thresholds below.
  /// 
  /// <code>
  /// Inertia
  /// 0%        10%          25%       35%         50%                       80%              100%
  /// |==========|============|=========|===========|=========================|================|
  /// 
  /// Input scale:            .         .                                     |================|
  /// .                                 .                                    100%              0%
  /// Out: STOP «|............|» START  .
  /// Out scale: |============|         .                                     .                .
  /// .         25%          100%       .                                     .                .
  /// Work area for not enough mech power:                                    .                .
  /// »       »  |............|  «   «  .«   «   «   «   «                    .                .
  /// Work area for excess mech power:  .
  /// »       »       »       »       » .     »       »   »   »   »   »   »   |................|
  /// Work area for switching on/off mode:
  /// .                        TURN ON «|.....................................|» TURN OFF
  /// </code>
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public class Shaft : IShaft
  {
    internal static readonly Percent STOP_OUTPUT_BELOW;
    internal static readonly Percent START_OUTPUT_ABOVE;
    internal static readonly Percent OUTPUT_SCALE_MIN;
    internal static readonly Percent OUTPUT_SCALE_MIN_AT;
    internal static readonly Percent OUTPUT_SCALE_MAX_AT;
    internal static readonly Percent INPUT_SCALE_MAX_AT;
    internal static readonly Percent HUNDRED_MINUS_INPUT_SCALE_MAX_AT;
    internal static readonly Percent SWITCHING_GEN_STOP_AT;
    internal static readonly Percent SWITCHING_GEN_START_AT;
    internal static readonly Percent LOSSES_START_AT_INPUT_SCALE;
    internal static readonly MechPower LOSSES_PER_ENTITY;
    private readonly ProductBuffer m_insertBuffer;
    private readonly ProductBuffer m_removeBuffer;
    private readonly ProductBuffer m_inertiaBuffer;
    private readonly Set<IEntityWithPorts> m_connectedEntities;
    private Quantity m_totalRemoved;
    private Quantity m_totalStored;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Whether this shaft is default zero-throughput shaft. This shaft is used for entities that are not
    /// connected to any other entities on a shaft.
    /// </summary>
    public bool IsDefaultNoCapacityShaft => this.m_insertBuffer.Capacity.IsZero;

    public IProductBufferReadOnly InertiaBuffer => (IProductBufferReadOnly) this.m_inertiaBuffer;

    public int EntitiesCount => this.m_connectedEntities.Count;

    public IReadOnlySet<IEntityWithPorts> ConnectedEntities
    {
      get => (IReadOnlySet<IEntityWithPorts>) this.m_connectedEntities;
    }

    public MechPower TotalAvailablePower
    {
      get
      {
        return MechPower.FromQuantity(this.m_insertBuffer.Quantity + this.m_inertiaBuffer.Quantity + this.m_removeBuffer.Quantity);
      }
    }

    public bool OutputAllowed { get; private set; }

    public bool InputAllowed { get; private set; }

    public Percent CurrentInertia { get; private set; }

    public Percent CurrentInputScale { get; private set; }

    public Percent CurrentOutputScale { get; private set; }

    public Percent ThroughputUtilization { get; private set; }

    public bool IsDestroyed { get; private set; }

    public Quantity AvailableCapacity => this.m_insertBuffer.UsableCapacity;

    public Quantity TotalStoreCapacity => this.m_insertBuffer.Capacity;

    public Shaft(ProductProto product, Quantity maxThroughput)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_connectedEntities = new Set<IEntityWithPorts>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CInputAllowed\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_insertBuffer = new ProductBuffer(maxThroughput, product);
      this.m_removeBuffer = new ProductBuffer(maxThroughput, product);
      this.m_inertiaBuffer = new ProductBuffer(maxThroughput, product);
      if (!maxThroughput.IsNotPositive)
        return;
      this.OutputAllowed = false;
      this.InputAllowed = false;
    }

    public Quantity GetAvailableQuantity(Quantity maxRemove)
    {
      return !this.OutputAllowed ? Quantity.Zero : maxRemove.ScaledBy(this.CurrentOutputScale).Min(this.m_removeBuffer.Quantity);
    }

    public MechPower UpdateStart()
    {
      if (this.m_inertiaBuffer.Capacity.IsNotPositive)
        return MechPower.Zero;
      this.ThroughputUtilization = Percent.FromRatioNonNegative((long) this.m_totalStored.Value, (long) this.m_insertBuffer.Capacity.Value).Max(Percent.FromRatioNonNegative((long) this.m_totalRemoved.Value, (long) this.m_removeBuffer.Capacity.Value));
      Assert.That<Percent>(this.ThroughputUtilization).IsWithin0To100PercIncl();
      this.m_totalStored = Quantity.Zero;
      this.m_totalRemoved = Quantity.Zero;
      Assert.That<Quantity>(this.m_inertiaBuffer.Quantity).IsLessOrEqual(this.m_inertiaBuffer.Capacity);
      this.m_inertiaBuffer.StoreAllIgnoreCapacity(this.m_insertBuffer.RemoveAll());
      this.m_inertiaBuffer.StoreAllIgnoreCapacity(this.m_removeBuffer.RemoveAll());
      Quantity zero = Quantity.Zero;
      Percent percent1 = Percent.FromRatioNonNegative((long) this.m_inertiaBuffer.Quantity.Value, (long) this.m_inertiaBuffer.Capacity.Value);
      this.CurrentInertia = percent1.Min(Percent.Hundred);
      percent1 = this.CurrentInertia;
      if (percent1.IsNegative)
      {
        Log.Error(string.Format("Negative inertia detected, resetting to zero: {0} ", (object) this.CurrentInertia) + string.Format("({0}/{1}).", (object) this.m_inertiaBuffer.Quantity.Value, (object) this.m_inertiaBuffer.Capacity.Value));
        this.CurrentInertia = Percent.Zero;
      }
      Percent hundred = Percent.Hundred;
      Percent percent2 = (this.CurrentInertia - Shaft.INPUT_SCALE_MAX_AT) / Shaft.HUNDRED_MINUS_INPUT_SCALE_MAX_AT;
      Percent percent3 = percent2.Clamp0To100();
      this.CurrentInputScale = hundred - percent3;
      if (this.CurrentInputScale < Shaft.LOSSES_START_AT_INPUT_SCALE)
      {
        Percent scale = (Shaft.LOSSES_START_AT_INPUT_SCALE - this.CurrentInputScale) / Shaft.LOSSES_START_AT_INPUT_SCALE;
        Assert.That<Percent>(scale).IsWithin0To100PercIncl();
        zero += this.m_inertiaBuffer.RemoveAsMuchAs((Shaft.LOSSES_PER_ENTITY.Quantity * this.EntitiesCount).ScaledBy(scale));
      }
      else
      {
        percent2 = this.ThroughputUtilization;
        if (percent2.IsZero)
          zero += this.m_inertiaBuffer.RemoveAsMuchAs(Shaft.LOSSES_PER_ENTITY.Quantity * this.EntitiesCount);
      }
      if (this.OutputAllowed)
      {
        if (this.CurrentInertia < Shaft.STOP_OUTPUT_BELOW)
          this.OutputAllowed = false;
      }
      else if (this.CurrentInertia > Shaft.START_OUTPUT_ABOVE)
        this.OutputAllowed = true;
      if (this.OutputAllowed)
      {
        Percent scale = Shaft.OUTPUT_SCALE_MAX_AT - Shaft.OUTPUT_SCALE_MIN_AT;
        percent2 = Shaft.OUTPUT_SCALE_MIN.Lerp(Percent.Hundred, this.CurrentInertia - Shaft.OUTPUT_SCALE_MIN_AT, scale);
        this.CurrentOutputScale = percent2.Clamp(Shaft.OUTPUT_SCALE_MIN, Percent.Hundred);
        this.m_removeBuffer.StoreAsMuchAsFrom((IProductBuffer) this.m_inertiaBuffer, this.m_removeBuffer.Capacity);
      }
      else
        this.CurrentOutputScale = Percent.Zero;
      if (this.m_inertiaBuffer.Quantity > this.m_inertiaBuffer.Capacity)
        zero += this.m_inertiaBuffer.RemoveAsMuchAs(this.m_inertiaBuffer.Quantity - this.m_inertiaBuffer.Capacity);
      return MechPower.FromQuantity(zero);
    }

    private static MechPower getInertiaOf(IStaticEntity entity)
    {
      ShaftInertiaProtoParam paramValue;
      if (entity.Prototype.TryGetParam<ShaftInertiaProtoParam>(out paramValue))
        return paramValue.Inertia;
      Log.Warning(string.Format("Static entity of type '{0}' connected to shaft does not have proto param ", (object) entity.Prototype) + "ShaftInertiaProtoParam set.");
      return MechPower.Zero;
    }

    public void AddEntity(IEntityWithPorts entity)
    {
      Assert.That<Quantity>(this.m_inertiaBuffer.Capacity).IsPositive("Adding to zero-capacity shaft.");
      this.m_connectedEntities.AddAndAssertNew(entity);
    }

    public void AddEntities(IEnumerable<IEntityWithPorts> entities)
    {
      foreach (IEntityWithPorts entity in entities)
        this.AddEntity(entity);
    }

    public MechPower UpdateTotalInertia()
    {
      Quantity capacity = this.m_insertBuffer.Capacity;
      foreach (IStaticEntity connectedEntity in this.m_connectedEntities)
      {
        if (connectedEntity.IsConstructed)
          capacity += Shaft.getInertiaOf(connectedEntity).Quantity;
      }
      MechPower mechPower;
      if (this.m_inertiaBuffer.Quantity > capacity)
      {
        Quantity quantity = this.m_inertiaBuffer.Quantity - capacity;
        this.m_inertiaBuffer.RemoveExactly(quantity);
        mechPower = MechPower.FromQuantity(quantity);
      }
      else
        mechPower = MechPower.Zero;
      this.m_inertiaBuffer.SetCapacity(capacity);
      return mechPower;
    }

    public Quantity StoreAsMuchAs(Quantity quantity)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Tried to store power to already destroyed shaft!");
      if (!this.InputAllowed)
        return quantity;
      Quantity quantity1 = quantity.ScaledByCeiled(this.CurrentInputScale);
      Quantity quantity2 = quantity - quantity1 + this.m_insertBuffer.StoreAsMuchAs(quantity1);
      this.m_totalStored += quantity - quantity2;
      return quantity2;
    }

    public bool CanRemove(Quantity quantity)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Tried to get power from already destroyed shaft!");
      return this.OutputAllowed && this.m_removeBuffer.CanRemove(quantity);
    }

    public Quantity RemoveAsMuchAs(Quantity quantity, Quantity maxOutput)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Tried to get power from already destroyed shaft!");
      Quantity quantity1 = this.m_removeBuffer.RemoveAsMuchAs(this.GetRemoveAmount(quantity, maxOutput));
      this.m_totalRemoved += quantity1;
      return quantity1;
    }

    public Quantity GetRemoveAmount(Quantity quantity, Quantity maxOutput)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Tried to query remove power from already destroyed shaft!");
      return !this.OutputAllowed ? Quantity.Zero : maxOutput.ScaledBy(this.CurrentOutputScale).Min(quantity);
    }

    public void Destroy(IProductsManager productsManager)
    {
      productsManager.ClearProduct(this.m_insertBuffer.Product, this.m_insertBuffer.Quantity);
      this.m_insertBuffer.Clear();
      productsManager.ClearProduct(this.m_removeBuffer.Product, this.m_removeBuffer.Quantity);
      this.m_removeBuffer.Clear();
      productsManager.ClearProduct(this.m_inertiaBuffer.Product, this.m_inertiaBuffer.Quantity);
      this.m_inertiaBuffer.Clear();
      this.IsDestroyed = true;
    }

    public Quantity TransferStateTo(Lyst<Shaft> newShafts)
    {
      Assert.That<Lyst<Shaft>>(newShafts).IsNotEmpty<Shaft>();
      Quantity quantity1 = this.m_insertBuffer.RemoveAll();
      Quantity quantity2 = this.m_removeBuffer.RemoveAll();
      Quantity quantity3 = this.m_inertiaBuffer.RemoveAll();
      Quantity quantity4 = quantity1 / newShafts.Count;
      Quantity quantity5 = quantity2 / newShafts.Count;
      Quantity quantity6 = quantity3 / newShafts.Count;
      Quantity quantity7 = quantity1 + quantity2 + quantity3 - (quantity4 + quantity5 + quantity6) * newShafts.Count;
      foreach (Shaft newShaft in newShafts)
      {
        quantity7 = quantity7 + newShaft.m_insertBuffer.StoreAsMuchAs(quantity4) + newShaft.m_removeBuffer.StoreAsMuchAs(quantity5) + newShaft.m_inertiaBuffer.StoreAsMuchAs(quantity6);
        newShaft.InputAllowed = this.InputAllowed;
        newShaft.OutputAllowed = this.OutputAllowed;
      }
      Assert.That<Quantity>(quantity7).IsNotNegative();
      return quantity7;
    }

    public Quantity TransferStateTo(Shaft newShaft)
    {
      Quantity quantity = newShaft.m_insertBuffer.StoreAsMuchAs(this.m_insertBuffer.RemoveAll()) + newShaft.m_removeBuffer.StoreAsMuchAs(this.m_removeBuffer.RemoveAll()) + newShaft.m_inertiaBuffer.StoreAsMuchAs(this.m_inertiaBuffer.RemoveAll());
      newShaft.InputAllowed = this.InputAllowed;
      newShaft.OutputAllowed = this.OutputAllowed;
      return quantity;
    }

    public static void Serialize(Shaft value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Shaft>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Shaft.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.CurrentInertia, writer);
      Percent.Serialize(this.CurrentInputScale, writer);
      Percent.Serialize(this.CurrentOutputScale, writer);
      writer.WriteBool(this.InputAllowed);
      writer.WriteBool(this.IsDestroyed);
      Set<IEntityWithPorts>.Serialize(this.m_connectedEntities, writer);
      ProductBuffer.Serialize(this.m_inertiaBuffer, writer);
      ProductBuffer.Serialize(this.m_insertBuffer, writer);
      ProductBuffer.Serialize(this.m_removeBuffer, writer);
      Quantity.Serialize(this.m_totalRemoved, writer);
      Quantity.Serialize(this.m_totalStored, writer);
      writer.WriteBool(this.OutputAllowed);
      Percent.Serialize(this.ThroughputUtilization, writer);
    }

    public static Shaft Deserialize(BlobReader reader)
    {
      Shaft shaft;
      if (reader.TryStartClassDeserialization<Shaft>(out shaft))
        reader.EnqueueDataDeserialization((object) shaft, Shaft.s_deserializeDataDelayedAction);
      return shaft;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CurrentInertia = Percent.Deserialize(reader);
      this.CurrentInputScale = Percent.Deserialize(reader);
      this.CurrentOutputScale = Percent.Deserialize(reader);
      this.InputAllowed = reader.ReadBool();
      this.IsDestroyed = reader.ReadBool();
      reader.SetField<Shaft>(this, "m_connectedEntities", (object) Set<IEntityWithPorts>.Deserialize(reader));
      reader.SetField<Shaft>(this, "m_inertiaBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<Shaft>(this, "m_insertBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<Shaft>(this, "m_removeBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_totalRemoved = Quantity.Deserialize(reader);
      this.m_totalStored = Quantity.Deserialize(reader);
      this.OutputAllowed = reader.ReadBool();
      this.ThroughputUtilization = Percent.Deserialize(reader);
    }

    static Shaft()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Shaft.STOP_OUTPUT_BELOW = 10.Percent();
      Shaft.START_OUTPUT_ABOVE = 25.Percent();
      Shaft.OUTPUT_SCALE_MIN = 25.Percent();
      Shaft.OUTPUT_SCALE_MIN_AT = Shaft.STOP_OUTPUT_BELOW;
      Shaft.OUTPUT_SCALE_MAX_AT = Shaft.START_OUTPUT_ABOVE;
      Shaft.INPUT_SCALE_MAX_AT = 80.Percent();
      Shaft.HUNDRED_MINUS_INPUT_SCALE_MAX_AT = Percent.Hundred - Shaft.INPUT_SCALE_MAX_AT;
      Shaft.SWITCHING_GEN_STOP_AT = Shaft.INPUT_SCALE_MAX_AT;
      Shaft.SWITCHING_GEN_START_AT = Shaft.OUTPUT_SCALE_MAX_AT + 10.Percent();
      Shaft.LOSSES_START_AT_INPUT_SCALE = 25.Percent();
      Shaft.LOSSES_PER_ENTITY = 200.KwMech();
      Shaft.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Shaft) obj).SerializeData(writer));
      Shaft.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Shaft) obj).DeserializeData(reader));
    }
  }
}
