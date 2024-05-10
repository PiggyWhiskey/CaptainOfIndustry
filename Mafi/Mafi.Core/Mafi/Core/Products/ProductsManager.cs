// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>
  /// Handles statistics for all products in the game. Gets notified every time a product quantity is created or
  /// removed.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class ProductsManager : IProductsManager
  {
    private readonly LazyResolve<IAssetTransactionManager> m_assetManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    public static readonly Percent RECYCLING_RATIO_BASE;
    private static readonly Percent RECYCLING_RATIO_MAX;
    private readonly IProperty<Percent> m_recyclingRatioDiff;
    private Percent m_passiveRecyclingIncreases;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IAssetTransactionManager AssetManager => this.m_assetManager.Value;

    /// <summary>Note: You can index into this array with slim ID.</summary>
    public ImmutableArray<Mafi.Core.Products.ProductStats> ProductStats { get; private set; }

    public ProductsSlimIdManager SlimIdManager { get; private set; }

    public Percent RecyclingRatio
    {
      get
      {
        return (ProductsManager.RECYCLING_RATIO_BASE + this.m_passiveRecyclingIncreases + this.m_recyclingRatioDiff.Value).Min(ProductsManager.RECYCLING_RATIO_MAX);
      }
    }

    public ProductsManager(
      ICalendar calendar,
      ProductsSlimIdManager productsSlimIdManager,
      PropsDb propsDb,
      UnlockedProtosDb unlockedProtosDb,
      StatsManager statsManager,
      LazyResolve<IAssetTransactionManager> assetsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductsManager productsManager = this;
      this.SlimIdManager = productsSlimIdManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.ProductStats = productsSlimIdManager.ManagedProtos.Map<Mafi.Core.Products.ProductStats>((Func<ProductProto, Mafi.Core.Products.ProductStats>) (x => new Mafi.Core.Products.ProductStats(statsManager, (IProductsManager) productsManager, x)));
      this.m_recyclingRatioDiff = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.RecyclingRatioDiff);
      calendar.NewDay.Add<ProductsManager>(this, new Action(this.onNewDay));
      this.m_assetManager = assetsManager;
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(DependencyResolver resolver)
    {
      ImmutableArray<ProductProto> managedProtos = this.SlimIdManager.ManagedProtos;
      int index = 0;
      ImmutableArray<Mafi.Core.Products.ProductStats> productStats;
      while (true)
      {
        int num = index;
        productStats = this.ProductStats;
        int length = productStats.Length;
        if (num < length)
        {
          productStats = this.ProductStats;
          if ((Proto) productStats[index].Product != (Proto) managedProtos[index])
          {
            string str1 = string.Format("Products changed after load at index {0}: ", (object) index);
            productStats = this.ProductStats;
            string str2 = string.Format("{0} (saved) != {1} (new)", (object) productStats[index].Product, (object) managedProtos[index]);
            Log.Error(str1 + str2);
          }
          ++index;
        }
        else
          break;
      }
      int length1 = managedProtos.Length;
      productStats = this.ProductStats;
      int length2 = productStats.Length;
      if (length1 <= length2)
        return;
      Log.Info("New products detected, expanding product stats.");
      StatsManager statsManager = resolver.Resolve<StatsManager>();
      Lyst<Mafi.Core.Products.ProductStats> lyst = new Lyst<Mafi.Core.Products.ProductStats>(managedProtos.Length);
      lyst.AddRange(this.ProductStats);
      productStats = this.ProductStats;
      for (int length3 = productStats.Length; length3 < managedProtos.Length; ++length3)
        lyst.Add(new Mafi.Core.Products.ProductStats(statsManager, (IProductsManager) this, managedProtos[length3]));
      this.ProductStats = lyst.ToImmutableArrayAndClear();
    }

    private void onNewDay()
    {
      foreach (Mafi.Core.Products.ProductStats productStat in this.ProductStats)
      {
        if (productStat.GlobalQuantity.IsPositive)
          this.m_unlockedProtosDb.Unlock((Proto) productStat.Product);
        productStat.GlobalQuantityStats.Add(productStat.GlobalQuantity);
        productStat.StoredQuantityTotalStats.Add(productStat.StoredQuantityTotal);
      }
    }

    public void IncreaseRecyclingRatio(Percent percent)
    {
      if (!percent.IsPositive)
        return;
      this.m_passiveRecyclingIncreases += percent;
    }

    public void ReportProductsTransformation(
      IIndexable<ProductQuantity> inputs,
      IIndexable<ProductQuantity> outputs,
      DestroyReason destroyReason,
      CreateReason createReason,
      bool disableSourceProductsConversionLoss = false)
    {
      Quantity zero = Quantity.Zero;
      foreach (ProductQuantity output in outputs)
      {
        if (output.Product.TrackSourceProducts)
          zero += output.Quantity;
      }
      foreach (ProductQuantity output in outputs)
      {
        Mafi.Core.Products.ProductStats productStat = this.ProductStats[(int) output.Product.SlimId.Value];
        productStat.Ϝ_RecordProduction(output.Quantity, createReason);
        if (zero.IsPositive && output.Product.TrackSourceProducts)
        {
          Percent multiplierDueToOtherOutputs = Percent.FromRatio(output.Quantity.Value, zero.Value);
          Percent recyclingRatio = !disableSourceProductsConversionLoss && output.Product.TryGetParam<ApplyRecyclingRatioOnSourcesParam>(out ApplyRecyclingRatioOnSourcesParam _) ? this.RecyclingRatio : Percent.Hundred;
          productStat.F_TransferSourcesFromInputs(output.Quantity, inputs, multiplierDueToOtherOutputs, recyclingRatio);
        }
      }
      foreach (ProductQuantity input in inputs)
      {
        Mafi.Core.Products.ProductStats productStat = this.ProductStats[(int) input.Product.SlimId.Value];
        if (productStat.GlobalQuantity.IsPositive)
        {
          Percent percentageToRemove = Percent.FromRatio((long) input.Quantity.Value, productStat.GlobalQuantity.Value);
          productStat.F_RemoveSourceProducts(percentageToRemove);
        }
        productStat.Ϝ_RecordUsage(input.Quantity, destroyReason);
      }
    }

    public void ProductCreated(
      ProductProto product,
      Quantity quantity,
      IIndexable<ProductQuantity> sources,
      CreateReason reason)
    {
      Mafi.Core.Products.ProductStats productStat = this.ProductStats[(int) product.SlimId.Value];
      productStat.Ϝ_RecordProduction(quantity, reason);
      productStat.F_AddSourceProducts(sources);
    }

    public void DestroyProductReturnRemovedSourceProducts(
      ProductProto product,
      Quantity quantity,
      DestroyReason reason,
      Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> result)
    {
      Assert.That<Quantity>(quantity).IsNotNegative();
      if (quantity.IsNotPositive)
        return;
      if ((Proto) product == (Proto) null)
      {
        Log.Error("Null product destroyed.");
      }
      else
      {
        Assert.That<ProductProto>(product).IsNotNullOrPhantom<ProductProto>();
        Mafi.Core.Products.ProductStats productStat = this.ProductStats[(int) product.SlimId.Value];
        productStat.F_RemoveSourcesForQuantityReturnResult(quantity, result);
        productStat.Ϝ_RecordUsage(quantity, reason);
      }
    }

    /// <remarks>Keep thread safe.</remarks>
    public Mafi.Core.Products.ProductStats GetStatsFor(ProductProto proto)
    {
      return this.ProductStats[(int) proto.SlimId.Value];
    }

    public void ProductCreated(ProductProto product, Quantity quantity, CreateReason reason)
    {
      if ((Proto) product == (Proto) null || product.IsPhantom)
        Log.Error("Null or phantom product created.");
      else
        this.ProductStats[(int) product.SlimId.Value].ProductCreated(quantity, reason);
    }

    public void ProductDestroyed(ProductProto product, Quantity quantity, DestroyReason reason)
    {
      if ((Proto) product == (Proto) null || product.IsPhantom)
        Log.Error("Null or phantom product created.");
      else
        this.ProductStats[(int) product.SlimId.Value].ProductDestroyed(quantity, reason);
    }

    public void ProductDestroyed(ProductSlimId slimId, Quantity quantity, DestroyReason reason)
    {
      this.ProductDestroyed(this.SlimIdManager.ResolveOrPhantom(slimId), quantity, reason);
    }

    public void ReportStorageCapacityChange(ProductProto product, Quantity capacityChange)
    {
      Assert.That<ProductProto>(product).IsNotNullOrPhantom<ProductProto>();
      this.ProductStats[(int) product.SlimId.Value].StorageCapacityChange(capacityChange);
    }

    public bool CanBeCleared(ProductProto product) => true;

    public void ClearProduct(ProductProto product, Quantity quantity)
    {
      Assert.That<bool>(this.CanBeCleared(product)).IsTrue("Clearing an un-clearable product!");
      this.ProductDestroyed(product, quantity, DestroyReason.Cleared);
    }

    public void ClearProductNoChecks(ProductProto product, Quantity quantity)
    {
      this.ProductDestroyed(product, quantity, DestroyReason.Cleared);
    }

    public static void Serialize(ProductsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProductsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProductsManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      LazyResolve<IAssetTransactionManager>.Serialize(this.m_assetManager, writer);
      Percent.Serialize(this.m_passiveRecyclingIncreases, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_recyclingRatioDiff);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      ImmutableArray<Mafi.Core.Products.ProductStats>.Serialize(this.ProductStats, writer);
      ProductsSlimIdManager.Serialize(this.SlimIdManager, writer);
    }

    public static ProductsManager Deserialize(BlobReader reader)
    {
      ProductsManager productsManager;
      if (reader.TryStartClassDeserialization<ProductsManager>(out productsManager))
        reader.EnqueueDataDeserialization((object) productsManager, ProductsManager.s_deserializeDataDelayedAction);
      return productsManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<ProductsManager>(this, "m_assetManager", (object) LazyResolve<IAssetTransactionManager>.Deserialize(reader));
      this.m_passiveRecyclingIncreases = Percent.Deserialize(reader);
      reader.SetField<ProductsManager>(this, "m_recyclingRatioDiff", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<ProductsManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      this.ProductStats = ImmutableArray<Mafi.Core.Products.ProductStats>.Deserialize(reader);
      this.SlimIdManager = ProductsSlimIdManager.Deserialize(reader);
      reader.RegisterInitAfterLoad<ProductsManager>(this, "initAfterLoad", InitPriority.Normal);
    }

    static ProductsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductsManager.RECYCLING_RATIO_BASE = 25.Percent();
      ProductsManager.RECYCLING_RATIO_MAX = 100.Percent();
      ProductsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductsManager) obj).SerializeData(writer));
      ProductsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductsManager) obj).DeserializeData(reader));
    }
  }
}
