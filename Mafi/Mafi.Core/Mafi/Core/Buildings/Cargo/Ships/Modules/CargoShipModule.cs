// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.Modules.CargoShipModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoShipModule : ICargoShipModuleFriend
  {
    public readonly CargoShipModuleProto Prototype;
    public readonly CargoShip CargoShip;
    public readonly CargoDepotModule DepotModule;
    private Option<ProductBuffer> m_buffer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<ProductProto> StoredProduct
    {
      get
      {
        return !this.m_buffer.HasValue ? Option<ProductProto>.None : (Option<ProductProto>) this.m_buffer.Value.Product;
      }
    }

    public Quantity UsableCapacity
    {
      get => !this.m_buffer.HasValue ? Quantity.Zero : this.m_buffer.Value.UsableCapacity;
    }

    public Quantity Quantity
    {
      get => !this.m_buffer.HasValue ? Quantity.Zero : this.m_buffer.Value.Quantity;
    }

    public Quantity Capacity => this.Prototype.Capacity;

    public CargoShipModule(
      CargoShipModuleProto prototype,
      CargoShip cargoShip,
      CargoDepotModule depotModule)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = prototype;
      this.CargoShip = cargoShip.CheckNotNull<CargoShip>();
      this.DepotModule = depotModule.CheckNotNull<CargoDepotModule>();
      this.assignProduct(depotModule.StoredProduct);
      depotModule.OnProductStoredChanged.Add<CargoShipModule>(this, new Action(this.onDepotProductChanged));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.m_buffer.ValueOrNull?.IncreaseCapacityTo(this.Capacity);

    private void onDepotProductChanged() => this.assignProduct(this.DepotModule.StoredProduct);

    private void assignProduct(Option<ProductProto> product)
    {
      if (this.m_buffer.HasValue && product.HasValue)
      {
        if ((Proto) this.m_buffer.Value.Product == (Proto) product.Value)
          return;
        if (this.m_buffer.Value.IsNotEmpty())
        {
          Log.Error(string.Format("Ship cannot store {0} as it already contains {1}!", (object) product, (object) this.m_buffer.Value.Product));
          return;
        }
      }
      if (product.IsNone)
      {
        if (this.m_buffer.HasValue && this.m_buffer.Value.IsNotEmpty())
          Log.Error(string.Format("Ship cannot clear its {0} as it already contains positive quantity!", (object) this.m_buffer.Value.Product));
        else
          this.m_buffer = Option<ProductBuffer>.None;
      }
      else
        this.m_buffer = (Option<ProductBuffer>) new ProductBuffer(this.Capacity, product.Value);
    }

    void ICargoShipModuleFriend.RemoveExactly(Quantity quantity)
    {
      this.m_buffer.Value.RemoveExactly(quantity);
    }

    void ICargoShipModuleFriend.Destroy()
    {
      if (this.m_buffer.HasValue)
        this.m_buffer.Value.Clear();
      this.DepotModule.OnProductStoredChanged.Remove<CargoShipModule>(this, new Action(this.onDepotProductChanged));
      Assert.That<Quantity>(this.Quantity).IsZero("Destroying non-empty ship module!");
    }

    public bool CanAcceptCargo()
    {
      return this.StoredProduct.HasValue && this.DepotModule.CanAcceptCargo();
    }

    /// <summary>
    /// Returns quantity that was not able to fit to this buffer.
    /// </summary>
    public Quantity StoreAsMuchAs(ProductQuantity toStore)
    {
      if (!this.DepotModule.CanAcceptCargo())
        return toStore.Quantity;
      Assert.That<Option<ProductBuffer>>(this.m_buffer).HasValue<ProductBuffer>();
      if (!((Proto) this.m_buffer.Value.Product != (Proto) toStore.Product))
        return this.m_buffer.Value.StoreAsMuchAs(toStore);
      Log.Error(string.Format("Ship cannot store {0} as it already contains {1}!", (object) toStore.Product, (object) this.m_buffer.Value.Product));
      return toStore.Quantity;
    }

    public static void Serialize(CargoShipModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipModule.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      CargoShip.Serialize(this.CargoShip, writer);
      CargoDepotModule.Serialize(this.DepotModule, writer);
      Option<ProductBuffer>.Serialize(this.m_buffer, writer);
      writer.WriteGeneric<CargoShipModuleProto>(this.Prototype);
    }

    public static CargoShipModule Deserialize(BlobReader reader)
    {
      CargoShipModule cargoShipModule;
      if (reader.TryStartClassDeserialization<CargoShipModule>(out cargoShipModule))
        reader.EnqueueDataDeserialization((object) cargoShipModule, CargoShipModule.s_deserializeDataDelayedAction);
      return cargoShipModule;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CargoShipModule>(this, "CargoShip", (object) CargoShip.Deserialize(reader));
      reader.SetField<CargoShipModule>(this, "DepotModule", (object) CargoDepotModule.Deserialize(reader));
      this.m_buffer = Option<ProductBuffer>.Deserialize(reader);
      reader.SetField<CargoShipModule>(this, "Prototype", (object) reader.ReadGenericAs<CargoShipModuleProto>());
      reader.RegisterInitAfterLoad<CargoShipModule>(this, "initSelf", InitPriority.Normal);
    }

    static CargoShipModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CargoShipModule) obj).SerializeData(writer));
      CargoShipModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CargoShipModule) obj).DeserializeData(reader));
    }
  }
}
