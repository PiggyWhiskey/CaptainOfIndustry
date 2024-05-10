// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.AssetTransactionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Economy
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class AssetTransactionManager : IAssetTransactionManager
  {
    private readonly IProductsManager m_productsManager;
    private readonly Lyst<IOverflowProductsStorage> m_overflowStorages;
    private readonly Lyst<AssetTransactionManager.GlobalBuffer> m_providerBuffers;
    private readonly Lyst<AssetTransactionManager.GlobalBuffer> m_receiverBuffers;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<AssetTransactionManager.GlobalBuffer> m_buffersCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AssetTransactionManager(IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_overflowStorages = new Lyst<IOverflowProductsStorage>();
      this.m_providerBuffers = new Lyst<AssetTransactionManager.GlobalBuffer>();
      this.m_receiverBuffers = new Lyst<AssetTransactionManager.GlobalBuffer>();
      this.m_buffersCache = new Lyst<AssetTransactionManager.GlobalBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      Dict<ProductProto, Quantity> dict = new Dict<ProductProto, Quantity>();
      foreach (AssetTransactionManager.GlobalBuffer globalBuffer in this.m_providerBuffers.ToArray())
      {
        IEntity valueOrNull = globalBuffer.Entity.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.IsDestroyed ? 1 : 0) : 0) != 0)
        {
          Log.Error(string.Format("Removed buffer that belonged to a destroyed entity {0}", (object) globalBuffer.Entity.Value));
          this.m_providerBuffers.Remove(globalBuffer);
        }
        else
          dict.GetRefValue(globalBuffer.Buffer.Product, out bool _) += globalBuffer.Buffer.Quantity;
      }
      foreach (AssetTransactionManager.GlobalBuffer globalBuffer in this.m_receiverBuffers.ToArray())
      {
        IEntity valueOrNull = globalBuffer.Entity.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.IsDestroyed ? 1 : 0) : 0) != 0)
        {
          Log.Error(string.Format("Removed buffer that belonged to a destroyed entity {0}", (object) globalBuffer.Entity.Value));
          this.m_receiverBuffers.Remove(globalBuffer);
        }
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in dict)
      {
        ProductStats statsFor = this.m_productsManager.GetStatsFor(keyValuePair.Key);
        if (statsFor.StoredAvailableQuantity != (QuantityLarge) keyValuePair.Value)
        {
          Quantity quantityClamped = (keyValuePair.Value.AsLarge - statsFor.StoredAvailableQuantity).ToQuantityClamped();
          if (saveVersion < 104)
          {
            Log.Info(string.Format("Repaired quantity mismatch for {0}, (in buffers) {1}", (object) statsFor.Product, (object) keyValuePair.Value) + string.Format(" != {0} (in stats)", (object) statsFor.StoredAvailableQuantity));
            if (quantityClamped.IsPositive)
              this.m_productsManager.ProductCreated(statsFor.Product, quantityClamped, CreateReason.General);
            else
              this.m_productsManager.ProductDestroyed(statsFor.Product, quantityClamped.Abs, DestroyReason.Cleared);
            statsFor.StoredAvailableQuantityChange(quantityClamped);
          }
          else if (saveVersion < 128 && quantityClamped.IsNegative && quantityClamped.Abs.Value <= 32)
          {
            this.m_productsManager.ProductDestroyed(statsFor.Product, quantityClamped.Abs, DestroyReason.Cleared);
            statsFor.StoredAvailableQuantityChange(quantityClamped);
            Log.Info(string.Format("Repaired quantity mismatch for {0}, (in buffers) {1}", (object) statsFor.Product, (object) keyValuePair.Value) + string.Format(" != {0} (in stats)", (object) statsFor.StoredAvailableQuantity));
          }
        }
      }
    }

    public Quantity GetAvailableQuantityForRemoval(ProductProto product)
    {
      return this.m_productsManager.GetStatsFor(product).StoredAvailableQuantity.ToQuantity() ?? Quantity.MaxValue;
    }

    public bool CanRemoveProduct(ProductQuantity pq)
    {
      if (pq.IsEmpty)
        return true;
      Quantity quantity = pq.Quantity;
      foreach (AssetTransactionManager.GlobalBuffer providerBuffer in this.m_providerBuffers)
      {
        if (!((Proto) providerBuffer.Buffer.Product != (Proto) pq.Product))
        {
          quantity -= providerBuffer.Buffer.Quantity;
          if (quantity.IsNotPositive)
            return true;
        }
      }
      return quantity.IsNotPositive;
    }

    public bool TryRemoveProduct(ProductQuantity pq, DestroyReason? reason)
    {
      if (pq.IsEmpty)
        return true;
      if (!this.CanRemoveProduct(pq))
        return false;
      Lyst<AssetTransactionManager.GlobalBuffer> sortedBuffers = this.fillCacheWithSortedProviders(pq.Product);
      Assert.That<Quantity>(this.removeAsMuchAsInternal(pq, reason, sortedBuffers)).IsNotPositive();
      return true;
    }

    /// <summary>Returns what was removed.</summary>
    public Quantity RemoveAsMuchAs(ProductQuantity pq, DestroyReason? reason)
    {
      if (pq.IsEmpty)
        return Quantity.Zero;
      Lyst<AssetTransactionManager.GlobalBuffer> sortedBuffers = this.fillCacheWithSortedProviders(pq.Product);
      return pq.Quantity - this.removeAsMuchAsInternal(pq, reason, sortedBuffers);
    }

    /// <summary>Returns what was not removed</summary>
    private Quantity removeAsMuchAsInternal(
      ProductQuantity pq,
      DestroyReason? reason,
      Lyst<AssetTransactionManager.GlobalBuffer> sortedBuffers)
    {
      Quantity maxQuantity = pq.Quantity;
      foreach (AssetTransactionManager.GlobalBuffer sortedBuffer in sortedBuffers)
      {
        if (!maxQuantity.IsNotPositive)
          maxQuantity = sortedBuffer.Buffer.RemoveAsMuchAsReturnLeft(maxQuantity);
        else
          break;
      }
      if (reason.HasValue)
        this.m_productsManager.ProductDestroyed(pq.WithNewQuantity(pq.Quantity - maxQuantity), reason.Value);
      return maxQuantity;
    }

    private Lyst<AssetTransactionManager.GlobalBuffer> fillCacheWithSortedProviders(
      ProductProto product)
    {
      this.m_buffersCache.Clear();
      foreach (AssetTransactionManager.GlobalBuffer providerBuffer in this.m_providerBuffers)
      {
        if ((Proto) providerBuffer.Buffer.Product == (Proto) product)
          this.m_buffersCache.Add(providerBuffer);
      }
      this.m_buffersCache.Sort((IComparer<AssetTransactionManager.GlobalBuffer>) AssetTransactionManager.GlobalBuffer.Comparator.INSTANCE);
      return this.m_buffersCache;
    }

    public void StoreProduct(ProductQuantity pq, CreateReason? reason)
    {
      if (pq.IsEmpty)
        return;
      if (reason.HasValue)
        this.m_productsManager.ProductCreated(pq, reason.Value);
      this.m_buffersCache.Clear();
      foreach (AssetTransactionManager.GlobalBuffer receiverBuffer in this.m_receiverBuffers)
      {
        if ((Proto) receiverBuffer.Buffer.Product == (Proto) pq.Product)
        {
          IEntity valueOrNull = receiverBuffer.Entity.ValueOrNull;
          if (valueOrNull != null)
          {
            if (valueOrNull.IsDestroyed)
            {
              Log.Error(string.Format("Forgot to remove {0} buffer for destroyed entity '{1}'?", (object) pq.Product, (object) valueOrNull));
              continue;
            }
            if (receiverBuffer.Buffer is ProductBuffer buffer && buffer.IsDestroyed)
            {
              Log.Error(string.Format("Trying to store {0} into a destroyed buffer? (entity: '{1}')?", (object) pq.Product, (object) valueOrNull));
              continue;
            }
            if (valueOrNull.IsNotEnabled())
              continue;
          }
          if (!(receiverBuffer.Buffer is LogisticsBuffer buffer1) || !buffer1.CleaningMode)
            this.m_buffersCache.Add(receiverBuffer);
        }
      }
      this.m_buffersCache.Sort((IComparer<AssetTransactionManager.GlobalBuffer>) AssetTransactionManager.GlobalBuffer.Comparator.INSTANCE);
      Quantity quantity = pq.Quantity;
      foreach (AssetTransactionManager.GlobalBuffer globalBuffer in this.m_buffersCache)
      {
        if (!quantity.IsNotPositive)
          quantity = globalBuffer.Buffer.StoreAsMuchAs(quantity);
        else
          break;
      }
      if (quantity.IsNotPositive)
        return;
      if (this.m_overflowStorages.IsEmpty)
        this.m_productsManager.ProductDestroyed(pq.WithNewQuantity(quantity), DestroyReason.Cleared);
      else
        this.m_overflowStorages.First.StoreProduct(pq.WithNewQuantity(quantity));
    }

    public void StoreClearedProduct(ProductQuantity productQuantity)
    {
      if (productQuantity.IsEmpty)
        return;
      if (productQuantity.Product.CanBeDiscarded)
        this.m_productsManager.ProductDestroyed(productQuantity, DestroyReason.Cleared);
      else
        this.StoreProduct(productQuantity, new CreateReason?());
    }

    public void AddGlobalOutput(IProductBuffer buffer, int priority, Option<IEntity> entity)
    {
      this.addToList(this.m_providerBuffers, buffer, priority, entity);
    }

    public void RemoveGlobalOutput(IProductBuffer buffer)
    {
      this.removeFromList(this.m_providerBuffers, buffer);
    }

    public void AddGlobalInput(IProductBuffer buffer, int priority, Option<IEntity> entity)
    {
      this.addToList(this.m_receiverBuffers, buffer, priority, entity);
    }

    public void RemoveGlobalInput(IProductBuffer buffer)
    {
      this.removeFromList(this.m_receiverBuffers, buffer);
    }

    private void addToList(
      Lyst<AssetTransactionManager.GlobalBuffer> list,
      IProductBuffer buffer,
      int priority,
      Option<IEntity> entity)
    {
      foreach (AssetTransactionManager.GlobalBuffer globalBuffer in list)
      {
        if (globalBuffer.Buffer == buffer)
        {
          Log.Error(string.Format("Buffer '{0}' was already registered for {1}", (object) buffer.Product, (object) globalBuffer.Entity.ValueOrNull?.ToString()));
          return;
        }
      }
      list.Add(new AssetTransactionManager.GlobalBuffer(buffer, priority, entity));
    }

    private void removeFromList(
      Lyst<AssetTransactionManager.GlobalBuffer> list,
      IProductBuffer buffer)
    {
      int index = list.FindIndex((Predicate<AssetTransactionManager.GlobalBuffer>) (x => x.Buffer == buffer));
      if (index < 0)
        Log.Error(string.Format("Failed to remove '{0}' buffer, it wasn't found", (object) buffer.Product));
      else
        list.RemoveAtReplaceWithLast(index);
    }

    /// <summary>
    /// Can be called multiple times to replace (e.g. due upgrade).
    /// </summary>
    public void AddOverflowProductsStorage(IOverflowProductsStorage storage)
    {
      if (this.m_overflowStorages.Contains(storage))
        return;
      this.m_overflowStorages.Add(storage);
    }

    public void TryRemoveOverflowStorage(IOverflowProductsStorage shipyard)
    {
      this.m_overflowStorages.TryRemoveReplaceLast(shipyard);
    }

    public static void Serialize(AssetTransactionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssetTransactionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssetTransactionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<IOverflowProductsStorage>.Serialize(this.m_overflowStorages, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Lyst<AssetTransactionManager.GlobalBuffer>.Serialize(this.m_providerBuffers, writer);
      Lyst<AssetTransactionManager.GlobalBuffer>.Serialize(this.m_receiverBuffers, writer);
    }

    public static AssetTransactionManager Deserialize(BlobReader reader)
    {
      AssetTransactionManager transactionManager;
      if (reader.TryStartClassDeserialization<AssetTransactionManager>(out transactionManager))
        reader.EnqueueDataDeserialization((object) transactionManager, AssetTransactionManager.s_deserializeDataDelayedAction);
      return transactionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<AssetTransactionManager>(this, "m_buffersCache", (object) new Lyst<AssetTransactionManager.GlobalBuffer>());
      reader.SetField<AssetTransactionManager>(this, "m_overflowStorages", (object) Lyst<IOverflowProductsStorage>.Deserialize(reader));
      reader.SetField<AssetTransactionManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<AssetTransactionManager>(this, "m_providerBuffers", (object) Lyst<AssetTransactionManager.GlobalBuffer>.Deserialize(reader));
      reader.SetField<AssetTransactionManager>(this, "m_receiverBuffers", (object) Lyst<AssetTransactionManager.GlobalBuffer>.Deserialize(reader));
      reader.RegisterInitAfterLoad<AssetTransactionManager>(this, "initSelf", InitPriority.Low);
    }

    static AssetTransactionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssetTransactionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AssetTransactionManager) obj).SerializeData(writer));
      AssetTransactionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AssetTransactionManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private readonly struct GlobalBuffer
    {
      public readonly IProductBuffer Buffer;
      public readonly int Priority;
      public readonly Option<IEntity> Entity;

      public GlobalBuffer(IProductBuffer buffer, int priority, Option<IEntity> entity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Buffer = buffer;
        this.Priority = priority;
        this.Entity = entity;
      }

      public static void Serialize(AssetTransactionManager.GlobalBuffer value, BlobWriter writer)
      {
        writer.WriteGeneric<IProductBuffer>(value.Buffer);
        writer.WriteInt(value.Priority);
        Option<IEntity>.Serialize(value.Entity, writer);
      }

      public static AssetTransactionManager.GlobalBuffer Deserialize(BlobReader reader)
      {
        return new AssetTransactionManager.GlobalBuffer(reader.ReadGenericAs<IProductBuffer>(), reader.ReadInt(), Option<IEntity>.Deserialize(reader));
      }

      internal class Comparator : IComparer<AssetTransactionManager.GlobalBuffer>
      {
        public static readonly AssetTransactionManager.GlobalBuffer.Comparator INSTANCE;

        public int Compare(
          AssetTransactionManager.GlobalBuffer x,
          AssetTransactionManager.GlobalBuffer y)
        {
          return x.Priority.CompareTo(y.Priority);
        }

        public Comparator()
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          // ISSUE: explicit constructor call
          base.\u002Ector();
        }

        static Comparator()
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          AssetTransactionManager.GlobalBuffer.Comparator.INSTANCE = new AssetTransactionManager.GlobalBuffer.Comparator();
        }
      }
    }
  }
}
