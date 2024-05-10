// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.FuelStatsCollector
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Stats
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class FuelStatsCollector : IFuelStatsCollector
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [NewInSaveVersion(140, null, null, typeof (StatsManager), null)]
    private readonly StatsManager m_statsManager;
    private readonly IProductsManager m_productsManager;
    [Obsolete]
    [DoNotSave(140, null)]
    private readonly QuantitySumStats TotalConsumedInVehicles;
    [Obsolete]
    [DoNotSave(140, null)]
    private readonly QuantitySumStats TotalConsumedInCargoShips;
    [Obsolete]
    [DoNotSave(140, null)]
    private readonly QuantitySumStats TotalConsumedInBattleship;
    [DoNotSave(140, null)]
    [Obsolete]
    private readonly QuantitySumStats TotalConsumedInPowerGenerators;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private LystStruct<FuelStatsCollector.StatsPerProduct> m_statsPerProduct;

    public static void Serialize(FuelStatsCollector value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FuelStatsCollector>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FuelStatsCollector.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      StatsManager.Serialize(this.m_statsManager, writer);
      LystStruct<FuelStatsCollector.StatsPerProduct>.Serialize(this.m_statsPerProduct, writer);
    }

    public static FuelStatsCollector Deserialize(BlobReader reader)
    {
      FuelStatsCollector fuelStatsCollector;
      if (reader.TryStartClassDeserialization<FuelStatsCollector>(out fuelStatsCollector))
        reader.EnqueueDataDeserialization((object) fuelStatsCollector, FuelStatsCollector.s_deserializeDataDelayedAction);
      return fuelStatsCollector;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FuelStatsCollector>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<FuelStatsCollector>(this, "m_statsManager", reader.LoadedSaveVersion >= 140 ? (object) StatsManager.Deserialize(reader) : (object) (StatsManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<FuelStatsCollector>(this, "m_statsManager", typeof (StatsManager), true);
      this.m_statsPerProduct = reader.LoadedSaveVersion >= 140 ? LystStruct<FuelStatsCollector.StatsPerProduct>.Deserialize(reader) : new LystStruct<FuelStatsCollector.StatsPerProduct>();
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<FuelStatsCollector>(this, "TotalConsumedInBattleship", (object) QuantitySumStats.Deserialize(reader));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<FuelStatsCollector>(this, "TotalConsumedInCargoShips", (object) QuantitySumStats.Deserialize(reader));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<FuelStatsCollector>(this, "TotalConsumedInPowerGenerators", (object) QuantitySumStats.Deserialize(reader));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<FuelStatsCollector>(this, "TotalConsumedInVehicles", (object) QuantitySumStats.Deserialize(reader));
      reader.RegisterInitAfterLoad<FuelStatsCollector>(this, "initSelf", InitPriority.High);
    }

    public ReadOnlyArraySlice<FuelStatsCollector.StatsPerProduct> Stats
    {
      get => this.m_statsPerProduct.BackingArrayAsSlice;
    }

    public FuelStatsCollector(StatsManager statsManager, IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_statsManager = statsManager;
      this.m_productsManager = productsManager;
    }

    [InitAfterLoad(InitPriority.High)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      this.m_statsPerProduct.Add(new FuelStatsCollector.StatsPerProduct(resolver.Resolve<ProtosDb>().GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Diesel), this.TotalConsumedInVehicles, this.TotalConsumedInCargoShips, this.TotalConsumedInBattleship, this.TotalConsumedInPowerGenerators));
    }

    public void ReportFuelUseAndDestroy(ProductProto product, Quantity quantity, FuelUsedBy reason)
    {
      if (quantity.IsNotPositive)
      {
        Assert.That<Quantity>(quantity).IsNotNegative();
      }
      else
      {
        this.m_productsManager.ProductDestroyed(product, quantity, DestroyReason.UsedAsFuel);
        foreach (FuelStatsCollector.StatsPerProduct statsPerProduct in this.m_statsPerProduct)
        {
          if ((Proto) statsPerProduct.Product == (Proto) product)
          {
            statsPerProduct.ReportFuelUse(quantity, reason);
            return;
          }
        }
        FuelStatsCollector.StatsPerProduct statsPerProduct1 = new FuelStatsCollector.StatsPerProduct(product, this.m_statsManager);
        this.m_statsPerProduct.Add(statsPerProduct1);
        statsPerProduct1.ReportFuelUse(quantity, reason);
      }
    }

    static FuelStatsCollector()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FuelStatsCollector.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FuelStatsCollector) obj).SerializeData(writer));
      FuelStatsCollector.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FuelStatsCollector) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct StatsPerProduct
    {
      public readonly ProductProto Product;
      public readonly QuantitySumStats TotalConsumedInVehicles;
      public readonly QuantitySumStats TotalConsumedInCargoShips;
      public readonly QuantitySumStats TotalConsumedInBattleship;
      public readonly QuantitySumStats TotalConsumedInPowerGenerators;

      public static void Serialize(FuelStatsCollector.StatsPerProduct value, BlobWriter writer)
      {
        writer.WriteGeneric<ProductProto>(value.Product);
        QuantitySumStats.Serialize(value.TotalConsumedInVehicles, writer);
        QuantitySumStats.Serialize(value.TotalConsumedInCargoShips, writer);
        QuantitySumStats.Serialize(value.TotalConsumedInBattleship, writer);
        QuantitySumStats.Serialize(value.TotalConsumedInPowerGenerators, writer);
      }

      public static FuelStatsCollector.StatsPerProduct Deserialize(BlobReader reader)
      {
        return new FuelStatsCollector.StatsPerProduct(reader.ReadGenericAs<ProductProto>(), QuantitySumStats.Deserialize(reader), QuantitySumStats.Deserialize(reader), QuantitySumStats.Deserialize(reader), QuantitySumStats.Deserialize(reader));
      }

      public StatsPerProduct(ProductProto product, StatsManager statsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = product;
        this.TotalConsumedInVehicles = new QuantitySumStats((Option<StatsManager>) statsManager, true);
        this.TotalConsumedInCargoShips = new QuantitySumStats((Option<StatsManager>) statsManager, true);
        this.TotalConsumedInBattleship = new QuantitySumStats((Option<StatsManager>) statsManager, true);
        this.TotalConsumedInPowerGenerators = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      }

      [LoadCtor]
      public StatsPerProduct(
        ProductProto product,
        QuantitySumStats totalConsumedInVehicles,
        QuantitySumStats totalConsumedInCargoShips,
        QuantitySumStats totalConsumedInBattleship,
        QuantitySumStats totalConsumedInPowerGenerators)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = product;
        this.TotalConsumedInVehicles = totalConsumedInVehicles;
        this.TotalConsumedInCargoShips = totalConsumedInCargoShips;
        this.TotalConsumedInBattleship = totalConsumedInBattleship;
        this.TotalConsumedInPowerGenerators = totalConsumedInPowerGenerators;
      }

      public void ReportFuelUse(Quantity quantity, FuelUsedBy reason)
      {
        switch (reason)
        {
          case FuelUsedBy.Vehicle:
            this.TotalConsumedInVehicles.Add((QuantityLarge) quantity);
            break;
          case FuelUsedBy.CargoShip:
            this.TotalConsumedInCargoShips.Add((QuantityLarge) quantity);
            break;
          case FuelUsedBy.BattleShip:
            this.TotalConsumedInBattleship.Add((QuantityLarge) quantity);
            break;
          case FuelUsedBy.PowerGenerator:
            this.TotalConsumedInPowerGenerators.Add((QuantityLarge) quantity);
            break;
        }
      }
    }
  }
}
