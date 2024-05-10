// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Excavators.PartialMinedProductTracker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Excavators
{
  [GenerateSerializer(false, null, 0)]
  internal class PartialMinedProductTracker
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LystStruct<LooseProductQuantity> m_finalProducts;
    private LystStruct<KeyValuePair<ProductProto, PartialQuantity>> m_minedProducts;
    private Quantity m_maxCapacity;
    private Quantity m_usedCapacity;

    public static void Serialize(PartialMinedProductTracker value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PartialMinedProductTracker>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PartialMinedProductTracker.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      LystStruct<LooseProductQuantity>.Serialize(this.m_finalProducts, writer);
      Quantity.Serialize(this.m_maxCapacity, writer);
      LystStruct<KeyValuePair<ProductProto, PartialQuantity>>.Serialize(this.m_minedProducts, writer);
      Quantity.Serialize(this.m_usedCapacity, writer);
    }

    public static PartialMinedProductTracker Deserialize(BlobReader reader)
    {
      PartialMinedProductTracker minedProductTracker;
      if (reader.TryStartClassDeserialization<PartialMinedProductTracker>(out minedProductTracker))
        reader.EnqueueDataDeserialization((object) minedProductTracker, PartialMinedProductTracker.s_deserializeDataDelayedAction);
      return minedProductTracker;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_finalProducts = LystStruct<LooseProductQuantity>.Deserialize(reader);
      this.m_maxCapacity = Quantity.Deserialize(reader);
      this.m_minedProducts = LystStruct<KeyValuePair<ProductProto, PartialQuantity>>.Deserialize(reader);
      this.m_usedCapacity = Quantity.Deserialize(reader);
    }

    public bool IsFull => this.m_usedCapacity >= this.m_maxCapacity;

    internal void Reset(Quantity maxCapacity, VehicleCargo currentCargo)
    {
      this.m_minedProducts.ClearSkipZeroingMemory();
      this.m_maxCapacity = maxCapacity;
      this.m_usedCapacity = Quantity.Zero;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in currentCargo)
        this.m_minedProducts.Add<ProductProto, PartialQuantity>(keyValuePair.Key, PartialQuantity.Zero);
    }

    internal void AddMinedProduct(PartialProductQuantity deltaPq)
    {
      Assert.That<PartialQuantity>(deltaPq.Quantity).IsPositive();
      if (deltaPq.Quantity.IsNotPositive)
        return;
      if (this.m_minedProducts.ContainsKey<ProductProto, PartialQuantity>(deltaPq.Product))
      {
        ref KeyValuePair<ProductProto, PartialQuantity> local = ref this.m_minedProducts.GetValueAsRef<ProductProto, PartialQuantity>(deltaPq.Product);
        this.m_usedCapacity -= local.Value.ToQuantityRounded();
        local = Make.Kvp<ProductProto, PartialQuantity>(local.Key, local.Value + deltaPq.Quantity);
      }
      else
        this.m_minedProducts.Add<ProductProto, PartialQuantity>(deltaPq.Product, deltaPq.Quantity);
      Quantity usedCapacity1 = this.m_usedCapacity;
      PartialQuantity valueOrDefault = this.m_minedProducts.GetValueOrDefault<ProductProto, PartialQuantity>(deltaPq.Product);
      Quantity quantityRounded1 = valueOrDefault.ToQuantityRounded();
      this.m_usedCapacity = usedCapacity1 + quantityRounded1;
      if (this.m_usedCapacity > this.m_maxCapacity)
      {
        ref KeyValuePair<ProductProto, PartialQuantity> local1 = ref this.m_minedProducts.GetValueAsRef<ProductProto, PartialQuantity>(deltaPq.Product);
        valueOrDefault = local1.Value;
        Assert.That<PartialQuantity>(valueOrDefault.FractionalPart).IsNear(Fix32.Zero, Fix32.FromFraction(1L, 100L));
        Quantity usedCapacity2 = this.m_usedCapacity;
        valueOrDefault = local1.Value;
        Quantity quantityRounded2 = valueOrDefault.ToQuantityRounded();
        this.m_usedCapacity = usedCapacity2 - quantityRounded2;
        ref KeyValuePair<ProductProto, PartialQuantity> local2 = ref local1;
        ProductProto key = local1.Key;
        valueOrDefault = local1.Value;
        PartialQuantity asPartial = valueOrDefault.IntegerPart.AsPartial;
        KeyValuePair<ProductProto, PartialQuantity> keyValuePair = Make.Kvp<ProductProto, PartialQuantity>(key, asPartial);
        local2 = keyValuePair;
        Quantity usedCapacity3 = this.m_usedCapacity;
        valueOrDefault = local1.Value;
        Quantity quantityRounded3 = valueOrDefault.ToQuantityRounded();
        this.m_usedCapacity = usedCapacity3 + quantityRounded3;
      }
      Assert.That<Quantity>(this.m_usedCapacity).IsLessOrEqual(this.m_maxCapacity);
    }

    internal PartialQuantity MaxAllowedQuantityOf(ProductProto product)
    {
      if (this.m_minedProducts.ContainsKey<ProductProto, PartialQuantity>(product))
        return (this.m_maxCapacity - this.m_usedCapacity).AsPartial - this.m_minedProducts.GetValueOrDefault<ProductProto, PartialQuantity>(product).FractionalPart;
      return this.m_minedProducts.Count >= 3 ? PartialQuantity.Zero : (this.m_maxCapacity - this.m_usedCapacity).AsPartial;
    }

    internal LystStruct<LooseProductQuantity> FinalProductsReadonly()
    {
      Assert.That<Quantity>(this.m_usedCapacity).IsLessOrEqual(this.m_maxCapacity);
      this.m_finalProducts.ClearSkipZeroingMemory();
      foreach (KeyValuePair<ProductProto, PartialQuantity> minedProduct in this.m_minedProducts)
        this.m_finalProducts.Add(new LooseProductQuantity((LooseProductProto) minedProduct.Key, minedProduct.Value.ToQuantityRounded()));
      return this.m_finalProducts;
    }

    public PartialMinedProductTracker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static PartialMinedProductTracker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PartialMinedProductTracker.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PartialMinedProductTracker) obj).SerializeData(writer));
      PartialMinedProductTracker.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PartialMinedProductTracker) obj).DeserializeData(reader));
    }
  }
}
