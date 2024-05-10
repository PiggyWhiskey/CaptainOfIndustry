// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapCargoManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Entities;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class WorldMapCargoManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly WorldMapManager m_worldMapManager;
    private readonly Set<CargoShip> m_shipsReserved;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductProto, Quantity> m_minesQuantitiesTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductProto, Quantity> m_minesCapacitiesTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductProto, Quantity> m_shipCapacitiesTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductProto, Quantity> m_shipUsableCapacitiesTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductProto, Quantity> m_reservedCapacitiesTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<ProductQuantity> m_productsToExchangeTemp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<ProductProto> m_productsCache;

    public static void Serialize(WorldMapCargoManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapCargoManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapCargoManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<CargoShip>.Serialize(this.m_shipsReserved, writer);
      WorldMapManager.Serialize(this.m_worldMapManager, writer);
    }

    public static WorldMapCargoManager Deserialize(BlobReader reader)
    {
      WorldMapCargoManager worldMapCargoManager;
      if (reader.TryStartClassDeserialization<WorldMapCargoManager>(out worldMapCargoManager))
        reader.EnqueueDataDeserialization((object) worldMapCargoManager, WorldMapCargoManager.s_deserializeDataDelayedAction);
      return worldMapCargoManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WorldMapCargoManager>(this, "m_minesCapacitiesTemp", (object) new Dict<ProductProto, Quantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_minesQuantitiesTemp", (object) new Dict<ProductProto, Quantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_productsCache", (object) new Set<ProductProto>());
      reader.SetField<WorldMapCargoManager>(this, "m_productsToExchangeTemp", (object) new Lyst<ProductQuantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_reservedCapacitiesTemp", (object) new Dict<ProductProto, Quantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_shipCapacitiesTemp", (object) new Dict<ProductProto, Quantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_shipsReserved", (object) Set<CargoShip>.Deserialize(reader));
      reader.SetField<WorldMapCargoManager>(this, "m_shipUsableCapacitiesTemp", (object) new Dict<ProductProto, Quantity>());
      reader.SetField<WorldMapCargoManager>(this, "m_worldMapManager", (object) WorldMapManager.Deserialize(reader));
    }

    public WorldMapCargoManager(WorldMapManager worldMapManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_shipsReserved = new Set<CargoShip>();
      this.m_minesQuantitiesTemp = new Dict<ProductProto, Quantity>();
      this.m_minesCapacitiesTemp = new Dict<ProductProto, Quantity>();
      this.m_shipCapacitiesTemp = new Dict<ProductProto, Quantity>();
      this.m_shipUsableCapacitiesTemp = new Dict<ProductProto, Quantity>();
      this.m_reservedCapacitiesTemp = new Dict<ProductProto, Quantity>();
      this.m_productsToExchangeTemp = new Lyst<ProductQuantity>();
      this.m_productsCache = new Set<ProductProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_worldMapManager = worldMapManager;
    }

    /// <summary>
    /// This will pair this ship and all world entities and figure out what is the maximal capacity percentage utilization.
    /// So if we have capacity for 100 oil and there is mine with 30 we get 30% utilization. Because this is max this means
    /// that no other module has more than 30% utilization. This server to figure out whether ship should depart or not.
    /// 
    /// Note: This takes some time so don't call it too often.
    /// </summary>
    public Percent CalculateCapacityUtilization(CargoShip ship)
    {
      this.m_productsToExchangeTemp.Clear();
      return this.calculateCargoToTransfer(ship, this.m_productsToExchangeTemp, false);
    }

    public void ReserveCargoForShip(CargoShip ship) => this.m_shipsReserved.Add(ship);

    public bool RemoveShipFromReservation(CargoShip ship) => this.m_shipsReserved.Remove(ship);

    private Percent calculateCargoToTransfer(
      CargoShip ship,
      Lyst<ProductQuantity> outProductsToExchange,
      bool ignoreReservations)
    {
      Assert.That<Lyst<ProductQuantity>>(outProductsToExchange).IsEmpty<ProductQuantity>();
      this.m_minesQuantitiesTemp.Clear();
      this.m_minesCapacitiesTemp.Clear();
      this.m_shipCapacitiesTemp.Clear();
      this.m_shipUsableCapacitiesTemp.Clear();
      this.m_reservedCapacitiesTemp.Clear();
      if (ship.NonEmptyModules.IsEmpty<CargoShipModule>())
        return Percent.Zero;
      this.getShipCapacities(ship, this.m_shipCapacitiesTemp);
      this.getShipUsableCapacities(ship, this.m_shipUsableCapacitiesTemp);
      this.getMinesQuantities(this.m_minesQuantitiesTemp);
      this.getEnabledMinesCapacities(this.m_minesCapacitiesTemp);
      if (!ignoreReservations)
      {
        foreach (CargoShip ship1 in this.m_shipsReserved)
          this.getShipUsableCapacities(ship1, this.m_reservedCapacitiesTemp);
      }
      Percent cargoToTransfer = Percent.Zero;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_shipUsableCapacitiesTemp)
      {
        ProductProto key = keyValuePair.Key;
        Quantity rhs1 = keyValuePair.Value;
        Quantity quantity1;
        if (this.m_minesQuantitiesTemp.TryGetValue(key, out quantity1))
        {
          Quantity quantity2 = this.m_shipCapacitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key);
          Quantity valueOrDefault1 = this.m_reservedCapacitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key);
          Quantity valueOrDefault2 = this.m_minesCapacitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key);
          Quantity quantity3 = (quantity1 - valueOrDefault1).Max(Quantity.Zero).Min(rhs1);
          if (valueOrDefault2.IsPositive)
            quantity2 = quantity2.Min(valueOrDefault2);
          Percent rhs2 = Percent.FromRatio(quantity3.Value, quantity2.Value);
          outProductsToExchange.Add(key.WithQuantity(quantity3));
          cargoToTransfer = cargoToTransfer.Max(rhs2);
        }
      }
      return cargoToTransfer;
    }

    public void GetAvailableWorldCargo(
      CargoShip ship,
      Lyst<WorldMapCargoManager.WorldCargoData> result)
    {
      result.Clear();
      if (ship.CargoDepot.ContractAssigned.HasValue)
        return;
      this.m_productsCache.Clear();
      foreach (CargoShipModule nonEmptyModule in ship.NonEmptyModules)
      {
        if (nonEmptyModule.StoredProduct.HasValue)
          this.m_productsCache.Add(nonEmptyModule.StoredProduct.Value);
      }
      if (this.m_productsCache.IsEmpty)
        return;
      this.m_reservedCapacitiesTemp.Clear();
      foreach (CargoShip ship1 in this.m_shipsReserved)
      {
        if (ship != ship1)
          this.getShipUsableCapacities(ship1, this.m_reservedCapacitiesTemp);
      }
      this.m_minesQuantitiesTemp.Clear();
      this.getMinesQuantities(this.m_minesQuantitiesTemp);
      this.m_minesCapacitiesTemp.Clear();
      this.getMinesCapacities(this.m_minesCapacitiesTemp);
      foreach (ProductProto key in this.m_productsCache)
      {
        Quantity valueOrDefault = this.m_minesCapacitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key);
        Quantity quantity = (this.m_minesQuantitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key) - this.m_reservedCapacitiesTemp.GetValueOrDefault<ProductProto, Quantity>(key)).Max(Quantity.Zero);
        result.Add(new WorldMapCargoManager.WorldCargoData()
        {
          Product = key,
          Quantity = quantity,
          Capacity = valueOrDefault
        });
      }
    }

    private void getShipUsableCapacities(CargoShip ship, Dict<ProductProto, Quantity> result)
    {
      foreach (CargoShipModule nonEmptyModule in ship.NonEmptyModules)
      {
        if (nonEmptyModule.CanAcceptCargo() && !nonEmptyModule.UsableCapacity.IsZero)
          result.AddQuantity<ProductProto>(nonEmptyModule.StoredProduct.Value, nonEmptyModule.UsableCapacity);
      }
    }

    private void getShipCapacities(CargoShip ship, Dict<ProductProto, Quantity> result)
    {
      foreach (CargoShipModule nonEmptyModule in ship.NonEmptyModules)
      {
        if (nonEmptyModule.CanAcceptCargo())
          result.AddQuantity<ProductProto>(nonEmptyModule.StoredProduct.Value, nonEmptyModule.Capacity);
      }
    }

    private void getMinesQuantities(Dict<ProductProto, Quantity> result)
    {
      foreach (WorldMapMine mine in (IEnumerable<WorldMapMine>) this.m_worldMapManager.Mines)
      {
        if (!mine.Buffer.IsEmpty())
          result.AddQuantity<ProductProto>(mine.Product, mine.Buffer.Quantity);
      }
    }

    private void getMinesCapacities(Dict<ProductProto, Quantity> result)
    {
      foreach (WorldMapMine mine in (IEnumerable<WorldMapMine>) this.m_worldMapManager.Mines)
      {
        if (mine.IsRepaired)
          result.AddQuantity<ProductProto>(mine.Product, mine.Buffer.Capacity);
      }
    }

    private void getEnabledMinesCapacities(Dict<ProductProto, Quantity> result)
    {
      foreach (WorldMapMine mine in (IEnumerable<WorldMapMine>) this.m_worldMapManager.Mines)
      {
        if (mine.IsEnabled)
          result.AddQuantity<ProductProto>(mine.Product, mine.Buffer.Capacity);
      }
    }

    public void LoadCargo(CargoShip ship)
    {
      if (!this.m_shipsReserved.Remove(ship))
      {
        Log.Error("Failed to remove reserved ship.");
      }
      else
      {
        this.m_productsToExchangeTemp.Clear();
        this.calculateCargoToTransfer(ship, this.m_productsToExchangeTemp, true);
        foreach (ProductQuantity productQuantity in this.m_productsToExchangeTemp)
        {
          Quantity quantity1 = productQuantity.Quantity;
          Quantity quantity2 = productQuantity.Quantity;
          foreach (WorldMapMine worldMapMine in (IEnumerable<WorldMapMine>) this.m_worldMapManager.Mines.OrderByDescending<WorldMapMine, Quantity>((Func<WorldMapMine, Quantity>) (x => x.Buffer.Quantity)))
          {
            if ((Proto) worldMapMine.Buffer.Product == (Proto) productQuantity.Product)
              quantity1 -= worldMapMine.RemoveAsMuchAs(quantity1);
            if (quantity1.IsZero)
              break;
          }
          foreach (CargoShipModule nonEmptyModule in ship.NonEmptyModules)
          {
            if (nonEmptyModule.CanAcceptCargo() && !((Proto) nonEmptyModule.StoredProduct.ValueOrNull != (Proto) productQuantity.Product))
            {
              Quantity quantity3 = quantity2 - nonEmptyModule.StoreAsMuchAs(productQuantity.Product.WithQuantity(quantity2));
              quantity2 -= quantity3;
            }
          }
          if (quantity2 != Quantity.Zero || quantity1 != Quantity.Zero)
            Log.Error(string.Format("Pairing failed for {0}. Expected both toStore='{1}' and toRemove='{2}' to be zero.", (object) productQuantity.Product, (object) quantity2, (object) quantity1));
        }
      }
    }

    static WorldMapCargoManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapCargoManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldMapCargoManager) obj).SerializeData(writer));
      WorldMapCargoManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldMapCargoManager) obj).DeserializeData(reader));
    }

    public struct WorldCargoData
    {
      public ProductProto Product;
      public Quantity Quantity;
      public Quantity Capacity;
    }
  }
}
