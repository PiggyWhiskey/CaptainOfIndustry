// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  [GenerateSerializer(false, null, 0)]
  public sealed class ProductStats
  {
    public readonly ProductProto Product;
    [DoNotSave(0, null)]
    private int m_negativeQuantityReportWaitCounter;
    public readonly IProductsManager ProductsManager;
    private readonly Dict<ProductProto, PartialQuantityLarge> m_sourceProducts;
    private readonly Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> m_iterationCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public QuantitySumStats UsedTotalStats { get; private set; }

    public QuantitySumStats UsedInDumpingStats { get; private set; }

    public QuantitySumStats UsedInConstructionStats { get; private set; }

    public QuantitySumStats UsedInMaintenanceStats { get; private set; }

    public QuantitySumStats UsedInSettlementStats { get; private set; }

    public QuantitySumStats UsedInResearchStats { get; private set; }

    public QuantitySumStats UsedInFarmsStats { get; private set; }

    public QuantitySumStats UsedInExportStats { get; private set; }

    public QuantitySumStats UsedAsFuelStats { get; private set; }

    public QuantitySumStats CreatedTotalStats { get; private set; }

    public QuantitySumStats CreatedByProduction { get; private set; }

    public QuantitySumStats CreatedByMiningStats { get; private set; }

    public QuantitySumStats CreatedByImportStats { get; private set; }

    public QuantitySumStats CreatedByDeconstructionStats { get; private set; }

    public QuantitySumStats CreatedByRecyclingStats { get; private set; }

    public QuantitySumStats CreatedByResearchStats { get; private set; }

    public QuantitySumStats CreatedBySettlementStats { get; private set; }

    public QuantityLarge CreatedByQuickTradeLifetime { get; private set; }

    public QuantityMaxStats GlobalQuantityStats { get; private set; }

    public QuantityMaxStats StoredQuantityTotalStats { get; private set; }

    /// <summary>Global storage capacity.</summary>
    public QuantityLarge StorageCapacity { get; private set; }

    /// <summary>
    /// Globally stored quantity that can be retrieved via assert manager.
    /// </summary>
    public QuantityLarge StoredAvailableQuantity { get; private set; }

    /// <summary>
    /// Globally stored quantity that cannot be retrieved via assert manager.
    /// </summary>
    public QuantityLarge StoredUnavailableQuantity { get; private set; }

    /// <summary>
    /// FYI: This is kinda tricky. We used to show globally stored quantity to the player
    /// but that was missing things like food because globally stored quantity is used
    /// also for quick buy and we don't want to remove food from settlement. So we started
    /// using global quantity but people found that confusing. So compromise is this.
    /// It shows globally stored quantity but also add in stored quantity that is not available
    /// for quick removal.
    /// </summary>
    public QuantityLarge StoredQuantityTotal
    {
      get => this.StoredAvailableQuantity + this.StoredUnavailableQuantity;
    }

    /// <summary>
    /// Total quantity of the product in the game without counting in terrain.
    /// </summary>
    public QuantityLarge GlobalQuantity { get; private set; }

    public ProductStats(
      StatsManager statsManager,
      IProductsManager productsManager,
      ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_sourceProducts = new Dict<ProductProto, PartialQuantityLarge>();
      this.m_iterationCache = new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductsManager = productsManager;
      this.Product = product.CheckNotNull<ProductProto>();
      this.UsedTotalStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInDumpingStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInConstructionStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInMaintenanceStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInSettlementStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInResearchStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInFarmsStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedInExportStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.UsedAsFuelStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedTotalStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByProduction = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByMiningStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByImportStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByDeconstructionStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByRecyclingStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedByResearchStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.CreatedBySettlementStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
      this.GlobalQuantityStats = new QuantityMaxStats((Option<StatsManager>) statsManager);
      this.StoredQuantityTotalStats = new QuantityMaxStats((Option<StatsManager>) statsManager);
    }

    public void GetProducedTotals(
      out QuantityLarge thisYear,
      out QuantityLarge lastYear,
      out QuantityLarge lifetime)
    {
      thisYear = this.CreatedTotalStats.ThisYear;
      lastYear = this.CreatedTotalStats.LastYear;
      lifetime = this.CreatedTotalStats.Lifetime;
    }

    public void GetConsumedTotals(
      out QuantityLarge thisYear,
      out QuantityLarge lastYear,
      out QuantityLarge lifetime)
    {
      thisYear = this.UsedTotalStats.ThisYear;
      lastYear = this.UsedTotalStats.LastYear;
      lifetime = this.UsedTotalStats.Lifetime;
    }

    public void Ϝ_RecordProduction(Quantity quantity, CreateReason reason)
    {
      if (quantity.IsNegative)
      {
        Log.Error("Cannot produce negative quantity!");
      }
      else
      {
        switch (reason)
        {
          case CreateReason.Imported:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByImportStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.MinedFromTerrain:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByMiningStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.Produced:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByProduction.Add((QuantityLarge) quantity);
            break;
          case CreateReason.General:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.QuickTrade:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByQuickTradeLifetime += quantity;
            break;
          case CreateReason.Deconstruction:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByDeconstructionStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.Settlement:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedBySettlementStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.Recycled:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByRecyclingStats.Add((QuantityLarge) quantity);
            break;
          case CreateReason.Research:
            this.CreatedTotalStats.Add((QuantityLarge) quantity);
            this.CreatedByResearchStats.Add((QuantityLarge) quantity);
            break;
        }
        this.GlobalQuantity += quantity.CheckNotNegative();
      }
    }

    public void Ϝ_RecordUsage(Quantity quantity, DestroyReason reason)
    {
      if (quantity.IsNegative)
      {
        Log.Error("Cannot destroy negative quantity!");
      }
      else
      {
        switch (reason)
        {
          case DestroyReason.DumpedOnTerrain:
            this.UsedInDumpingStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.General:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.UsedAsFuel:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedAsFuelStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.QuickTrade:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Export:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInExportStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Construction:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInConstructionStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Maintenance:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInMaintenanceStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Settlement:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInSettlementStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Research:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInResearchStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.Farms:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            this.UsedInFarmsStats.Add((QuantityLarge) quantity);
            break;
          case DestroyReason.LoanPayment:
            this.UsedTotalStats.Add((QuantityLarge) quantity);
            break;
        }
        this.GlobalQuantity -= quantity.CheckNotNegative();
        if (!this.GlobalQuantity.IsNegative)
          return;
        if (this.m_negativeQuantityReportWaitCounter > 0)
        {
          --this.m_negativeQuantityReportWaitCounter;
        }
        else
        {
          this.m_negativeQuantityReportWaitCounter = (int) (-this.GlobalQuantity.Value).Clamp(10L, 10000L);
          Log.Warning(string.Format("Global quantity of '{0}' has negative value {1},", (object) this.Product, (object) this.GlobalQuantity.Value) + string.Format(" destroy reason: {0}", (object) reason));
        }
      }
    }

    public void ProductCreated(Quantity quantity, CreateReason reason)
    {
      if (quantity.IsNotPositive)
      {
        Assert.That<Quantity>(quantity).IsNotNegative();
      }
      else
      {
        this.Ϝ_RecordProduction(quantity, reason);
        this.F_TransferSourcesFromInputs(quantity, Indexable<ProductQuantity>.Empty, Percent.Hundred, Percent.Hundred);
      }
    }

    public void ProductDestroyed(Quantity quantity, DestroyReason reason)
    {
      if (quantity.IsNotPositive)
      {
        Assert.That<Quantity>(quantity).IsNotNegative();
      }
      else
      {
        this.F_RemoveSourcesForQuantity(quantity);
        this.Ϝ_RecordUsage(quantity, reason);
      }
    }

    public void StorageCapacityChange(Quantity capacityChange)
    {
      this.StorageCapacity += capacityChange;
      Assert.That<QuantityLarge>(this.StorageCapacity).IsNotNegative();
    }

    public void StoredAvailableQuantityChange(Quantity quantityChange, bool updateCapacity = false)
    {
      if (this.StoredAvailableQuantity + quantityChange < QuantityLarge.Zero)
        return;
      this.StoredAvailableQuantity += quantityChange;
      if (!updateCapacity)
        return;
      this.StorageCapacity += quantityChange;
    }

    public void StoredUnavailableQuantityChange(Quantity quantityChange, bool updateCapacity = false)
    {
      if (this.StoredUnavailableQuantity + quantityChange < QuantityLarge.Zero)
        return;
      this.StoredUnavailableQuantity += quantityChange;
      if (!updateCapacity)
        return;
      this.StorageCapacity += quantityChange;
    }

    public IReadOnlyDictionary<ProductProto, PartialQuantityLarge> SourceProducts
    {
      get => (IReadOnlyDictionary<ProductProto, PartialQuantityLarge>) this.m_sourceProducts;
    }

    /// <summary>
    /// Note: Does not report any global quantity changes, only removes source products.
    /// </summary>
    public void F_RemoveSourcesForQuantity(Quantity removed)
    {
      if (this.m_sourceProducts.IsEmpty || this.GlobalQuantity.IsNotPositive)
        return;
      if (removed.IsNotPositive)
      {
        Assert.That<bool>(removed.IsNotNegative).IsTrue();
      }
      else
      {
        this.m_iterationCache.Clear();
        this.m_iterationCache.AddRange((IEnumerable<KeyValuePair<ProductProto, PartialQuantityLarge>>) this.m_sourceProducts);
        Percent scale = Percent.FromRatio((long) removed.Value, this.GlobalQuantity.Value);
        foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_iterationCache)
        {
          PartialQuantityLarge partialQuantityLarge1 = keyValuePair.Value;
          PartialQuantityLarge partialQuantityLarge2 = partialQuantityLarge1.ScaledBy(scale);
          if (!partialQuantityLarge2.IsNotPositive)
          {
            partialQuantityLarge1 = keyValuePair.Value - partialQuantityLarge2;
            PartialQuantityLarge partialQuantityLarge3 = partialQuantityLarge1.Max(PartialQuantityLarge.Zero);
            this.m_sourceProducts[keyValuePair.Key] = partialQuantityLarge3;
          }
        }
      }
    }

    /// <summary>
    /// Note: Does not report any global quantity changes, only adds source products.
    /// </summary>
    public void F_AddSourceProducts(IIndexable<ProductQuantity> recyclables)
    {
      foreach (ProductQuantity recyclable in recyclables)
        this.addSourceProduct(recyclable.Product, recyclable.Quantity.AsPartialQuantityLarge(), 100.Percent());
    }

    /// <summary>
    /// Note: Does not report any global quantity changes, only removes source products.
    /// </summary>
    public void F_RemoveSourcesForQuantityReturnResult(
      Quantity quantity,
      Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> result)
    {
      result.Clear();
      if (this.m_sourceProducts.IsEmpty || this.GlobalQuantity.IsNotPositive)
        return;
      if (quantity.IsNotPositive)
      {
        Assert.That<bool>(quantity.IsNotNegative).IsTrue();
      }
      else
      {
        Percent scale = Percent.FromRatio((long) quantity.Value, this.GlobalQuantity.Value);
        this.m_iterationCache.Clear();
        this.m_iterationCache.AddRange((IEnumerable<KeyValuePair<ProductProto, PartialQuantityLarge>>) this.m_sourceProducts);
        foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_iterationCache)
        {
          PartialQuantityLarge partialQuantityLarge1 = keyValuePair.Value;
          PartialQuantityLarge partialQuantityLarge2 = partialQuantityLarge1.ScaledBy(scale);
          if (!partialQuantityLarge2.IsNotPositive)
          {
            result.Add(new KeyValuePair<ProductProto, PartialQuantityLarge>(keyValuePair.Key, partialQuantityLarge2));
            partialQuantityLarge1 = keyValuePair.Value - partialQuantityLarge2;
            PartialQuantityLarge partialQuantityLarge3 = partialQuantityLarge1.Max(PartialQuantityLarge.Zero);
            this.m_sourceProducts[keyValuePair.Key] = partialQuantityLarge3;
          }
        }
      }
    }

    public void F_RemoveSourceProducts(Percent percentageToRemove)
    {
      if (percentageToRemove.IsZero || this.m_sourceProducts.IsEmpty)
        return;
      this.m_iterationCache.Clear();
      this.m_iterationCache.AddRange((IEnumerable<KeyValuePair<ProductProto, PartialQuantityLarge>>) this.m_sourceProducts);
      foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_iterationCache)
      {
        PartialQuantityLarge partialQuantityLarge1 = keyValuePair.Value;
        PartialQuantityLarge partialQuantityLarge2 = partialQuantityLarge1.ScaledBy(percentageToRemove);
        if (!partialQuantityLarge2.IsNotPositive)
        {
          partialQuantityLarge1 = keyValuePair.Value - partialQuantityLarge2;
          PartialQuantityLarge partialQuantityLarge3 = partialQuantityLarge1.Max(PartialQuantityLarge.Zero);
          this.m_sourceProducts[keyValuePair.Key] = partialQuantityLarge3;
        }
      }
    }

    /// <summary>
    /// Will take sources from the inputs and transfer them to this product.
    /// 
    /// NOTE: This does not change any global quantities and it will not remove source
    /// products that it took from its inputs.
    /// </summary>
    public void F_TransferSourcesFromInputs(
      Quantity outputQuantity,
      IIndexable<ProductQuantity> inputs,
      Percent multiplierDueToOtherOutputs,
      Percent recyclingRatio)
    {
      if (outputQuantity.IsNotPositive)
        Assert.That<bool>(outputQuantity.IsNotNegative).IsTrue();
      else if (this.Product.SourceProduct.IsNotEmpty)
      {
        this.addSourceProduct(this.Product.SourceProduct.Product, this.Product.SourceProduct.Quantity.AsPartialQuantityLarge() * outputQuantity.Value, this.Product.SourceProduct.Product.IsRecyclable ? recyclingRatio : Percent.Hundred);
      }
      else
      {
        foreach (ProductQuantity input in inputs)
        {
          ProductStats statsFor = this.ProductsManager.GetStatsFor(input.Product);
          if (!statsFor.m_sourceProducts.IsEmpty && !statsFor.GlobalQuantity.IsNotPositive)
          {
            Percent scale = Percent.FromRatio((long) input.Quantity.Value, statsFor.GlobalQuantity.Value);
            this.m_iterationCache.Clear();
            this.m_iterationCache.AddRange((IEnumerable<KeyValuePair<ProductProto, PartialQuantityLarge>>) statsFor.m_sourceProducts);
            foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_iterationCache)
            {
              PartialQuantityLarge toAdd = keyValuePair.Value.ScaledBy(scale);
              if (!toAdd.IsNotPositive)
                this.addSourceProduct(keyValuePair.Key, toAdd, keyValuePair.Key.IsRecyclable ? multiplierDueToOtherOutputs.Apply(recyclingRatio) : multiplierDueToOtherOutputs);
            }
          }
        }
      }
    }

    private void addSourceProduct(
      ProductProto product,
      PartialQuantityLarge toAdd,
      Percent multiplier)
    {
      if (toAdd.IsNotPositive)
        Assert.That<bool>(toAdd.IsNotNegative).IsTrue();
      else if (product.IsPhantom)
      {
        Assert.Fail(string.Format("Trying to add phantom to {0} source stats", (object) this.Product));
      }
      else
      {
        toAdd = toAdd.ScaledBy(multiplier);
        if (toAdd.IsNotPositive)
          return;
        PartialQuantityLarge zero;
        if (!this.m_sourceProducts.TryGetValue(product, out zero))
          zero = PartialQuantityLarge.Zero;
        zero += toAdd;
        this.m_sourceProducts[product] = zero;
      }
    }

    public static void Serialize(ProductStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProductStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProductStats.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      QuantitySumStats.Serialize(this.CreatedByDeconstructionStats, writer);
      QuantitySumStats.Serialize(this.CreatedByImportStats, writer);
      QuantitySumStats.Serialize(this.CreatedByMiningStats, writer);
      QuantitySumStats.Serialize(this.CreatedByProduction, writer);
      QuantityLarge.Serialize(this.CreatedByQuickTradeLifetime, writer);
      QuantitySumStats.Serialize(this.CreatedByRecyclingStats, writer);
      QuantitySumStats.Serialize(this.CreatedByResearchStats, writer);
      QuantitySumStats.Serialize(this.CreatedBySettlementStats, writer);
      QuantitySumStats.Serialize(this.CreatedTotalStats, writer);
      QuantityLarge.Serialize(this.GlobalQuantity, writer);
      QuantityMaxStats.Serialize(this.GlobalQuantityStats, writer);
      Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>.Serialize(this.m_iterationCache, writer);
      Dict<ProductProto, PartialQuantityLarge>.Serialize(this.m_sourceProducts, writer);
      writer.WriteGeneric<ProductProto>(this.Product);
      writer.WriteGeneric<IProductsManager>(this.ProductsManager);
      QuantityLarge.Serialize(this.StorageCapacity, writer);
      QuantityLarge.Serialize(this.StoredAvailableQuantity, writer);
      QuantityMaxStats.Serialize(this.StoredQuantityTotalStats, writer);
      QuantityLarge.Serialize(this.StoredUnavailableQuantity, writer);
      QuantitySumStats.Serialize(this.UsedAsFuelStats, writer);
      QuantitySumStats.Serialize(this.UsedInConstructionStats, writer);
      QuantitySumStats.Serialize(this.UsedInDumpingStats, writer);
      QuantitySumStats.Serialize(this.UsedInExportStats, writer);
      QuantitySumStats.Serialize(this.UsedInFarmsStats, writer);
      QuantitySumStats.Serialize(this.UsedInMaintenanceStats, writer);
      QuantitySumStats.Serialize(this.UsedInResearchStats, writer);
      QuantitySumStats.Serialize(this.UsedInSettlementStats, writer);
      QuantitySumStats.Serialize(this.UsedTotalStats, writer);
    }

    public static ProductStats Deserialize(BlobReader reader)
    {
      ProductStats productStats;
      if (reader.TryStartClassDeserialization<ProductStats>(out productStats))
        reader.EnqueueDataDeserialization((object) productStats, ProductStats.s_deserializeDataDelayedAction);
      return productStats;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.CreatedByDeconstructionStats = QuantitySumStats.Deserialize(reader);
      this.CreatedByImportStats = QuantitySumStats.Deserialize(reader);
      this.CreatedByMiningStats = QuantitySumStats.Deserialize(reader);
      this.CreatedByProduction = QuantitySumStats.Deserialize(reader);
      this.CreatedByQuickTradeLifetime = QuantityLarge.Deserialize(reader);
      this.CreatedByRecyclingStats = QuantitySumStats.Deserialize(reader);
      this.CreatedByResearchStats = QuantitySumStats.Deserialize(reader);
      this.CreatedBySettlementStats = QuantitySumStats.Deserialize(reader);
      this.CreatedTotalStats = QuantitySumStats.Deserialize(reader);
      this.GlobalQuantity = QuantityLarge.Deserialize(reader);
      this.GlobalQuantityStats = QuantityMaxStats.Deserialize(reader);
      reader.SetField<ProductStats>(this, "m_iterationCache", (object) Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>.Deserialize(reader));
      reader.SetField<ProductStats>(this, "m_sourceProducts", (object) Dict<ProductProto, PartialQuantityLarge>.Deserialize(reader));
      reader.SetField<ProductStats>(this, "Product", (object) reader.ReadGenericAs<ProductProto>());
      reader.SetField<ProductStats>(this, "ProductsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.StorageCapacity = QuantityLarge.Deserialize(reader);
      this.StoredAvailableQuantity = QuantityLarge.Deserialize(reader);
      this.StoredQuantityTotalStats = QuantityMaxStats.Deserialize(reader);
      this.StoredUnavailableQuantity = QuantityLarge.Deserialize(reader);
      this.UsedAsFuelStats = QuantitySumStats.Deserialize(reader);
      this.UsedInConstructionStats = QuantitySumStats.Deserialize(reader);
      this.UsedInDumpingStats = QuantitySumStats.Deserialize(reader);
      this.UsedInExportStats = QuantitySumStats.Deserialize(reader);
      this.UsedInFarmsStats = QuantitySumStats.Deserialize(reader);
      this.UsedInMaintenanceStats = QuantitySumStats.Deserialize(reader);
      this.UsedInResearchStats = QuantitySumStats.Deserialize(reader);
      this.UsedInSettlementStats = QuantitySumStats.Deserialize(reader);
      this.UsedTotalStats = QuantitySumStats.Deserialize(reader);
    }

    static ProductStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductStats) obj).SerializeData(writer));
      ProductStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductStats) obj).DeserializeData(reader));
    }
  }
}
