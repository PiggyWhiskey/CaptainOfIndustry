// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleCargo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleCargo : IVehicleCargo
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int MAX_MIXED_PRODUCT_COUNT = 3;
    [ThreadStatic]
    private static Lyst<ProductQuantity> s_planTmp;
    private readonly Lyst<KeyValuePair<ProductProto, Quantity>> m_data;
    private Quantity m_totalQuantity;

    public static void Serialize(VehicleCargo value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleCargo>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleCargo.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<KeyValuePair<ProductProto, Quantity>>.Serialize(this.m_data, writer);
      Quantity.Serialize(this.m_totalQuantity, writer);
    }

    public static VehicleCargo Deserialize(BlobReader reader)
    {
      VehicleCargo vehicleCargo;
      if (reader.TryStartClassDeserialization<VehicleCargo>(out vehicleCargo))
        reader.EnqueueDataDeserialization((object) vehicleCargo, VehicleCargo.s_deserializeDataDelayedAction);
      return vehicleCargo;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleCargo>(this, "m_data", (object) Lyst<KeyValuePair<ProductProto, Quantity>>.Deserialize(reader));
      this.m_totalQuantity = Quantity.Deserialize(reader);
    }

    public bool IsEmpty => this.TotalQuantity.IsZero;

    public bool IsNotEmpty => this.TotalQuantity.IsNotZero;

    public Quantity TotalQuantity => this.m_totalQuantity;

    public ProductQuantity FirstOrPhantom
    {
      get => this.m_data.Count <= 0 ? ProductQuantity.None : this.first();
    }

    public int Count => this.m_data.Count;

    public Lyst<KeyValuePair<ProductProto, Quantity>>.Enumerator GetEnumerator()
    {
      return this.m_data.GetEnumerator();
    }

    public void GetCargoProducts(Lyst<ProductQuantity> cacheToPopulate)
    {
      cacheToPopulate.Clear();
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_data)
        cacheToPopulate.Add(new ProductQuantity(keyValuePair.Key, keyValuePair.Value));
    }

    /// <summary>
    /// Gets the quantity of the given product. Returns 0 if not found.
    /// </summary>
    public Quantity GetQuantityOf(ProductProto product)
    {
      Quantity quantity;
      return this.m_data.TryGetValue<ProductProto, Quantity>(product, out quantity) ? quantity : Quantity.Zero;
    }

    private ProductQuantity first()
    {
      Assert.That<int>(this.m_data.Count).IsNotZero();
      KeyValuePair<ProductProto, Quantity> keyValuePair = this.m_data[0];
      ProductProto key = keyValuePair.Key;
      keyValuePair = this.m_data[0];
      Quantity quantity = keyValuePair.Value;
      return new ProductQuantity(key, quantity);
    }

    public bool CanAdd(ProductProto product)
    {
      if (this.m_data.Count == 0 || this.HasProduct(product))
        return true;
      return this.m_data.Count != 3 && this.m_data[0].Key.IsMixable && product.IsMixable;
    }

    public bool HasProduct(ProductProto product)
    {
      return this.m_data.ContainsKey<ProductProto, Quantity>(product);
    }

    public void Add(ProductProto product, Quantity quantity)
    {
      Assert.That<bool>(this.CanAdd(product)).IsTrue();
      Quantity quantity1;
      int index;
      if (this.m_data.TryGetValue<ProductProto, Quantity>(product, out quantity1, out index))
        this.m_data[index] = new KeyValuePair<ProductProto, Quantity>(product, quantity1 + quantity);
      else
        this.m_data.Add<ProductProto, Quantity>(product, quantity);
      this.m_totalQuantity += quantity;
    }

    public void Add(ProductQuantity pq) => this.Add(pq.Product, pq.Quantity);

    /// <summary>
    /// Use this if you want to do updates to the Cargo inside of a loop over the cargo.
    /// After the loop concludes, be sure to call ExecutePlan
    /// </summary>
    public void PlanAdd(ProductQuantity pq)
    {
      if (VehicleCargo.s_planTmp == null)
        VehicleCargo.s_planTmp = new Lyst<ProductQuantity>();
      VehicleCargo.s_planTmp.Add(pq);
    }

    public void RemoveExactly(ProductQuantity pq) => this.RemoveExactly(pq.Product, pq.Quantity);

    public void RemoveExactly(ProductProto product, Quantity removedQuantity)
    {
      Quantity quantity1;
      int index;
      if (!this.m_data.TryGetValue<ProductProto, Quantity>(product, out quantity1, out index))
      {
        Log.Warning(string.Format("Tried to remove pq that doesn't exist in cargo '{0}', '{1}'", (object) product, (object) removedQuantity));
      }
      else
      {
        if (removedQuantity > quantity1)
        {
          Log.Warning(string.Format("Attempting to remove {0} of `{1}` ", (object) removedQuantity, (object) product) + string.Format("while cargo only contains {0}.", (object) quantity1));
          removedQuantity = quantity1;
        }
        Quantity quantity2 = quantity1 - removedQuantity;
        this.m_data[index] = new KeyValuePair<ProductProto, Quantity>(product, quantity1 - removedQuantity);
        this.m_totalQuantity -= removedQuantity;
        Assert.That<Quantity>(this.m_totalQuantity).IsNotNegative("Total quantity went negative.");
        if (!quantity2.IsZero)
          return;
        this.m_data.Remove<ProductProto, Quantity>(product);
      }
    }

    /// <inheritdoc cref="M:Mafi.Core.Vehicles.VehicleCargo.TryRemoveAsMuchAs(Mafi.Core.Products.ProductProto,Mafi.Quantity)" />
    /// .
    public Quantity TryRemoveAsMuchAs(ProductQuantity pq)
    {
      return this.TryRemoveAsMuchAs(pq.Product, pq.Quantity);
    }

    /// <summary>
    /// Tries to remove quantity, removes amount that was removed.
    /// </summary>
    public Quantity TryRemoveAsMuchAs(ProductProto product, Quantity removedQuantity)
    {
      Quantity quantity1;
      int index;
      if (removedQuantity.IsNotPositive || !this.m_data.TryGetValue<ProductProto, Quantity>(product, out quantity1, out index))
        return Quantity.Zero;
      if (removedQuantity > quantity1)
        removedQuantity = quantity1;
      Quantity quantity2 = quantity1 - removedQuantity;
      this.m_data[index] = Make.Kvp<ProductProto, Quantity>(product, quantity1 - removedQuantity);
      this.m_totalQuantity -= removedQuantity;
      Assert.That<Quantity>(this.m_totalQuantity).IsNotNegative("Total quantity went negative.");
      if (quantity2.IsZero)
        this.m_data.Remove<ProductProto, Quantity>(product);
      return removedQuantity;
    }

    /// <summary>
    /// Use this if you want to do updates to the Cargo inside of a loop over the cargo.
    /// After the loop concludes, be sure to call ExecutePlan
    /// </summary>
    public void PlanRemove(ProductQuantity pq)
    {
      if (VehicleCargo.s_planTmp == null)
        VehicleCargo.s_planTmp = new Lyst<ProductQuantity>();
      VehicleCargo.s_planTmp.Add(new ProductQuantity(pq.Product, -pq.Quantity));
      Assert.That<int>(VehicleCargo.s_planTmp.Count).IsLessOrEqual(10, "Larger plan than I expect we'll ever have");
    }

    /// <summary>
    /// Use this if you want to do updates to the Cargo inside of a loop over the cargo.
    /// Expected to have a plan generated by PlanAdd and PlanRemove.
    /// </summary>
    public void ExecutePlan()
    {
      if (VehicleCargo.s_planTmp == null)
        VehicleCargo.s_planTmp = new Lyst<ProductQuantity>();
      foreach (ProductQuantity pq in VehicleCargo.s_planTmp)
      {
        if (pq.Quantity.IsNegative)
          this.RemoveExactly(new ProductQuantity(pq.Product, -pq.Quantity));
        else
          this.Add(pq);
      }
      VehicleCargo.s_planTmp.Clear();
    }

    public void Clear()
    {
      this.m_totalQuantity = Quantity.Zero;
      this.m_data.Clear();
    }

    public void ClearCargoImmediately(IAssetTransactionManager assetTransactionManager)
    {
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_data)
        assetTransactionManager.StoreClearedProduct(new ProductQuantity(keyValuePair.Key, keyValuePair.Value));
      this.m_totalQuantity = Quantity.Zero;
      this.m_data.Clear();
    }

    public void ClearCargoImmediately(
      IAssetTransactionManager assetTransactionManager,
      ProductProto product)
    {
      Quantity quantity;
      if (this.m_data.TryGetValue<ProductProto, Quantity>(product, out quantity))
      {
        assetTransactionManager.StoreClearedProduct(new ProductQuantity(product, quantity));
        this.RemoveExactly(product, quantity);
      }
      else
        Log.Error(string.Format("Tried to clear product that doesn't exist in cargo '{0}'", (object) product));
    }

    public override string ToString()
    {
      return ((IEnumerable<string>) this.m_data.ToArray<string>((Func<KeyValuePair<ProductProto, Quantity>, string>) (x => string.Format("{0} of {1}", (object) x.Value, (object) x.Key.Id.Value)))).JoinStrings(", ");
    }

    public VehicleCargo()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_data = new Lyst<KeyValuePair<ProductProto, Quantity>>(3);
      this.m_totalQuantity = Quantity.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleCargo()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleCargo.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleCargo) obj).SerializeData(writer));
      VehicleCargo.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleCargo) obj).DeserializeData(reader));
    }
  }
}
