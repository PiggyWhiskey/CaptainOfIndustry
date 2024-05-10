// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ProductBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Buffer for a limited quantity of anything. Handles safe deposit and take methods that always ensure correctness
  /// of the operations.
  /// </summary>
  [DebuggerDisplay("{Product}={Quantity}/{Capacity}")]
  [GenerateSerializer(false, null, 0)]
  public class ProductBuffer : IProductBuffer, IProductBufferReadOnly
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsFull => this.UsableCapacity.IsNotPositive;

    public bool IsNotFull => this.UsableCapacity.IsPositive;

    public bool IsEmpty => this.Quantity.IsNotPositive;

    public bool IsNotEmpty => this.Quantity.IsPositive;

    /// <summary>Quantity stored in the buffer.</summary>
    public Quantity Quantity { get; private set; }

    public ProductQuantity ProductQuantity => this.Product.WithQuantity(this.Quantity);

    /// <summary>Total capacity of the buffer.</summary>
    public Quantity Capacity { get; private set; }

    /// <summary>Product stored in the buffer.</summary>
    public ProductProto Product { get; private set; }

    public Quantity UsableCapacity => (this.Capacity - this.Quantity).Max(Quantity.Zero);

    [NewInSaveVersion(128, null, null, null, null)]
    public bool IsDestroyed { get; set; }

    public ProductBuffer(Quantity capacity, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Capacity = capacity.CheckNotNegative();
      this.Product = product.CheckNotNull<ProductProto>();
    }

    /// <summary>
    /// Whether the given product and quantity can be stored to this buffer.
    /// </summary>
    public bool CanStore(ProductQuantity productQuantity)
    {
      return !((Proto) productQuantity.Product != (Proto) this.Product) && productQuantity.Quantity <= this.UsableCapacity;
    }

    /// <summary>
    /// Whether the given quantity can be stored to this buffer.
    /// </summary>
    public bool CanStore(Quantity quantity) => quantity <= this.UsableCapacity;

    /// <summary>
    /// Stores as much given quantity as possible. Returns quantity that was not able to fit to this buffer.
    /// </summary>
    public Quantity StoreAsMuchAs(Quantity quantity)
    {
      if (quantity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying to store negative amount {0} to ProductBuffer with {1}.", (object) quantity, (object) this.Product));
        return Quantity.Zero;
      }
      if (this.Quantity.IsNegative)
      {
        Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        this.Quantity = Quantity.Zero;
        return Quantity.Zero;
      }
      if (this.IsDestroyed)
        Mafi.Log.Error(string.Format("Storing product '{0}' into a destroyed buffer.", (object) this.Product));
      Quantity diff = quantity < this.UsableCapacity ? quantity : this.UsableCapacity;
      this.Quantity += diff;
      if (diff.IsPositive)
        this.OnQuantityChanged(diff);
      return quantity - diff;
    }

    /// <summary>
    /// This is used between storages to avoid reporting quantities via global stats.
    /// Returns quantity that was not able to fit to this buffer.
    /// </summary>
    public Quantity StoreAsMuchAs_DoNotReport(Quantity quantity)
    {
      if (quantity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying to store negative amount {0} to ProductBuffer with {1}.", (object) quantity, (object) this.Product));
        return Quantity.Zero;
      }
      if (this.Quantity.IsNegative)
      {
        Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        this.Quantity = Quantity.Zero;
        return Quantity.Zero;
      }
      if (this.IsDestroyed)
        Mafi.Log.Error(string.Format("Storing product '{0}' into a destroyed buffer.", (object) this.Product));
      Quantity quantity1 = quantity < this.UsableCapacity ? quantity : this.UsableCapacity;
      this.Quantity += quantity1;
      return quantity - quantity1;
    }

    /// <summary>
    /// Stores as much quantity from given <see cref="P:Mafi.Core.Entities.Static.ProductBuffer.ProductQuantity" /> as possible. Returns quantity that was not
    /// able to fit to this buffer wrapped in <see cref="P:Mafi.Core.Entities.Static.ProductBuffer.ProductQuantity" /> with corresponding product.
    /// </summary>
    public Quantity StoreAsMuchAs(ProductQuantity productQuantity)
    {
      return (Proto) productQuantity.Product != (Proto) this.Product ? productQuantity.Quantity : this.StoreAsMuchAs(productQuantity.Quantity);
    }

    public void StoreAllIgnoreCapacity(Quantity quantity)
    {
      this.Quantity += quantity;
      this.OnQuantityChanged(quantity);
    }

    public void IncreaseCapacityTo(Quantity newCapacity)
    {
      if (newCapacity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying set negative capacity {0} to ProductBuffer with {1}.", (object) newCapacity, (object) this.Product));
        newCapacity = Quantity.Zero;
      }
      if (this.Capacity >= newCapacity)
        return;
      Quantity capacity = this.Capacity;
      this.Capacity = newCapacity;
      this.OnCapacityChanged(this.Capacity - capacity);
    }

    /// <summary>
    /// This will force a new capacity which can also cause the buffer to overflow.
    /// </summary>
    public void ForceNewCapacityTo(Quantity newCapacity)
    {
      if (newCapacity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying set negative capacity {0} to ProductBuffer with {1}.", (object) newCapacity, (object) this.Product));
        newCapacity = Quantity.Zero;
      }
      Quantity diff = newCapacity - this.Capacity;
      this.Capacity = newCapacity;
      this.OnCapacityChanged(diff);
    }

    public void SetCapacity(Quantity capacity)
    {
      if (this.Quantity > capacity)
        Mafi.Log.Error("Failed to set capacity, too small new capacity.");
      else
        this.Capacity = capacity;
    }

    /// <summary>
    /// Tries to reduce the capacity to the given one. If there is more quantity it will shrink to it at least.
    /// </summary>
    public void SetCapacityAsLessAs(Quantity capacity)
    {
      this.Capacity = this.Quantity.Max(capacity);
    }

    public void RemoveAndReduceCapacity(Quantity quantity)
    {
      if (this.Quantity < quantity)
      {
        Mafi.Log.Error("Failed to remove and set capacity, not enough products.");
      }
      else
      {
        this.Quantity -= quantity;
        this.Capacity -= quantity;
        this.OnQuantityChanged(-quantity);
      }
    }

    /// <summary>
    /// Whether the requested quantity can be removed from this buffer.
    /// </summary>
    public bool CanRemove(Quantity quantity) => quantity <= this.Quantity;

    /// <summary>
    /// Removes as much quantity as possible with regards to given max quantity constraint. Returns how much was
    /// removed.
    /// </summary>
    public Quantity RemoveAsMuchAs(Quantity maxQuantity)
    {
      if (maxQuantity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying to remove negative amount {0} from ProductBuffer with {1}.", (object) maxQuantity, (object) this.Product));
        return Quantity.Zero;
      }
      if (this.Quantity.IsNegative)
      {
        Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        this.Quantity = Quantity.Zero;
        return Quantity.Zero;
      }
      Quantity quantity = this.Quantity <= maxQuantity ? this.Quantity : maxQuantity;
      if (quantity.IsPositive)
      {
        this.Quantity -= quantity;
        this.OnQuantityChanged(-quantity);
      }
      return quantity;
    }

    /// <summary>
    /// This is used between storages to avoid reporting quantities via global stats.
    /// Returns how much was removed.
    /// </summary>
    public Quantity RemoveAsMuchAs_DoNotReport(Quantity maxQuantity)
    {
      if (maxQuantity.IsNegative)
      {
        Mafi.Log.Warning(string.Format("Trying to remove negative amount {0} from ProductBuffer with {1}.", (object) maxQuantity, (object) this.Product));
        return Quantity.Zero;
      }
      if (this.Quantity.IsNegative)
      {
        Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        this.Quantity = Quantity.Zero;
        return Quantity.Zero;
      }
      Quantity quantity = this.Quantity <= maxQuantity ? this.Quantity : maxQuantity;
      this.Quantity -= quantity;
      return quantity;
    }

    public Quantity RemoveAll()
    {
      if (this.Quantity.IsNotPositive)
      {
        if (this.Quantity.IsNegative)
          Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        return Quantity.Zero;
      }
      Quantity quantity = this.Quantity;
      this.Quantity = Quantity.Zero;
      this.OnQuantityChanged(-quantity);
      return quantity;
    }

    /// <summary>Clears the buffer. Return how much was removed.</summary>
    public Quantity Clear()
    {
      if (this.Quantity.IsNotPositive)
      {
        if (this.Quantity.IsNegative)
          Mafi.Log.Error(string.Format("Product buffer with {0} has negative quantity {1}. Resetting to zero.", (object) this.Product, (object) this.Quantity));
        return Quantity.Zero;
      }
      Quantity quantity = this.Quantity;
      this.Quantity = Quantity.Zero;
      this.OnQuantityChanged(-quantity);
      return quantity;
    }

    protected virtual void OnQuantityChanged(Quantity diff)
    {
    }

    protected virtual void OnCapacityChanged(Quantity diff)
    {
    }

    public virtual void Destroy()
    {
      this.Clear();
      this.IsDestroyed = true;
    }

    public static void Serialize(ProductBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProductBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProductBuffer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Quantity.Serialize(this.Capacity, writer);
      writer.WriteBool(this.IsDestroyed);
      writer.WriteGeneric<ProductProto>(this.Product);
      Quantity.Serialize(this.Quantity, writer);
    }

    public static ProductBuffer Deserialize(BlobReader reader)
    {
      ProductBuffer productBuffer;
      if (reader.TryStartClassDeserialization<ProductBuffer>(out productBuffer))
        reader.EnqueueDataDeserialization((object) productBuffer, ProductBuffer.s_deserializeDataDelayedAction);
      return productBuffer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Capacity = Quantity.Deserialize(reader);
      this.IsDestroyed = reader.LoadedSaveVersion >= 128 && reader.ReadBool();
      this.Product = reader.ReadGenericAs<ProductProto>();
      this.Quantity = Quantity.Deserialize(reader);
    }

    static ProductBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
      ProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
    }
  }
}
