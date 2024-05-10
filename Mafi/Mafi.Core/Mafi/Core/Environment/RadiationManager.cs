// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.RadiationManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using Mafi.Utils;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Environment
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class RadiationManager : IRadiationManager
  {
    public readonly QuantitySumStats Stats;
    private static readonly Percent GROWTH_PENALTY_PER_REACTOR;
    private static readonly Percent MAX_PENALTY_FROM_PRODUCTS;
    private static readonly int MOVING_AVG_WINDOW_SIZE;
    private readonly MovingAverageHelper m_lastMonthsPollutionQuantities;
    private readonly Dict<ProductProto, Quantity> m_safelyStoredProducts;
    private readonly Dict<ProductProto, ProductStats> m_stats;
    private readonly IProductsManager m_productsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private Percent m_growthPenaltyFromReactors;
    private Percent m_growthPenaltyFromReactorsPreviousMonth;
    private Percent m_growthPenaltyFromProducts;
    private int m_reactorLeaksCountThisMonth;
    private long m_productsRadiationTotalThisMonth;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Percent LastMonthPopsGrowthReduction
    {
      get => this.m_growthPenaltyFromReactors + this.m_growthPenaltyFromProducts;
    }

    public RadiationManager(
      ProtosDb protos,
      ICalendar calendar,
      IProductsManager productsManager,
      PopsHealthManager popsHealthManager,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastMonthsPollutionQuantities = new MovingAverageHelper(RadiationManager.MOVING_AVG_WINDOW_SIZE);
      this.m_safelyStoredProducts = new Dict<ProductProto, Quantity>();
      this.m_stats = new Dict<ProductProto, ProductStats>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_popsHealthManager = popsHealthManager;
      foreach (ProductProto productProto in protos.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.Radioactivity > 0)))
      {
        this.m_safelyStoredProducts.Add(productProto, Quantity.Zero);
        this.m_stats.Add(productProto, this.m_productsManager.GetStatsFor(productProto));
      }
      this.Stats = new QuantitySumStats((Option<StatsManager>) statsManager);
      calendar.NewMonth.Add<RadiationManager>(this, new Action(this.onNewMonth));
      calendar.NewDay.Add<RadiationManager>(this, new Action(this.onNewDay));
    }

    public void ReportSafelyStoredQuantityChange(ProductProto product, Quantity diff)
    {
      if (diff.IsZero || product.Radioactivity == 0)
        return;
      Quantity quantity1;
      if (!this.m_safelyStoredProducts.TryGetValue(product, out quantity1))
      {
        Log.Error(string.Format("Received product {0} which is not radioactive: {1} rad.", (object) product.Id, (object) product.Radioactivity));
      }
      else
      {
        Quantity quantity2 = quantity1 + diff;
        if (quantity2.IsNegative)
        {
          Log.Error(string.Format("Safely stored product {0} went negative: {1}", (object) product.Id, (object) quantity2));
          quantity2 = Quantity.Zero;
        }
        this.m_safelyStoredProducts[product] = quantity2;
      }
    }

    public void ReportReactorRadiationLeak() => ++this.m_reactorLeaksCountThisMonth;

    private long calculateTotalRadiationFromProducts()
    {
      long radiationFromProducts = 0;
      foreach (ProductStats productStats in this.m_stats.Values)
      {
        QuantityLarge quantityLarge = productStats.GlobalQuantity - this.m_safelyStoredProducts[productStats.Product].AsLarge;
        if (quantityLarge.IsNegative)
          Log.Error(string.Format("Not safely stored product {0} went negative: {1}", (object) productStats.Product.Id, (object) quantityLarge));
        else
          radiationFromProducts += quantityLarge.Value * (long) productStats.Product.Radioactivity;
      }
      return radiationFromProducts;
    }

    private void onNewDay()
    {
      this.m_productsRadiationTotalThisMonth += this.calculateTotalRadiationFromProducts();
    }

    private void onNewMonth()
    {
      this.m_growthPenaltyFromReactors = (this.m_reactorLeaksCountThisMonth.ToFix32() / 1.Months().Ticks).ToPercent().Apply(RadiationManager.GROWTH_PENALTY_PER_REACTOR) + this.m_growthPenaltyFromReactorsPreviousMonth * 3 / 4;
      if (this.m_growthPenaltyFromReactors < 0.1.Percent())
        this.m_growthPenaltyFromReactors = Percent.Zero;
      this.m_growthPenaltyFromReactorsPreviousMonth = this.m_growthPenaltyFromReactors;
      int num1 = ((int) (this.m_productsRadiationTotalThisMonth / 30L)).Min(80000);
      this.m_lastMonthsPollutionQuantities.AddItem(num1);
      int num2 = (this.m_lastMonthsPollutionQuantities.GetAvg() - 480).Max(0);
      this.m_growthPenaltyFromProducts = num2 <= 0 ? Percent.Zero : (num2 / 800.ToFix32() / 100).ToPercent().Min(RadiationManager.MAX_PENALTY_FROM_PRODUCTS);
      Quantity quantity = (this.m_growthPenaltyFromReactors.ToFix64Percent() * 800 / 2).ToIntRounded().Quantity();
      this.Stats.Add((QuantityLarge) (num1.Quantity() + quantity));
      if (this.LastMonthPopsGrowthReduction.IsPositive)
        this.m_popsHealthManager.AddBirthDecrease(IdsCore.BirthRateCategories.Radiation, this.LastMonthPopsGrowthReduction);
      this.m_reactorLeaksCountThisMonth = 0;
      this.m_productsRadiationTotalThisMonth = 0L;
    }

    public static void Serialize(RadiationManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RadiationManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RadiationManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.m_growthPenaltyFromProducts, writer);
      Percent.Serialize(this.m_growthPenaltyFromReactors, writer);
      Percent.Serialize(this.m_growthPenaltyFromReactorsPreviousMonth, writer);
      MovingAverageHelper.Serialize(this.m_lastMonthsPollutionQuantities, writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteLong(this.m_productsRadiationTotalThisMonth);
      writer.WriteInt(this.m_reactorLeaksCountThisMonth);
      Dict<ProductProto, Quantity>.Serialize(this.m_safelyStoredProducts, writer);
      Dict<ProductProto, ProductStats>.Serialize(this.m_stats, writer);
      QuantitySumStats.Serialize(this.Stats, writer);
    }

    public static RadiationManager Deserialize(BlobReader reader)
    {
      RadiationManager radiationManager;
      if (reader.TryStartClassDeserialization<RadiationManager>(out radiationManager))
        reader.EnqueueDataDeserialization((object) radiationManager, RadiationManager.s_deserializeDataDelayedAction);
      return radiationManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_growthPenaltyFromProducts = Percent.Deserialize(reader);
      this.m_growthPenaltyFromReactors = Percent.Deserialize(reader);
      this.m_growthPenaltyFromReactorsPreviousMonth = Percent.Deserialize(reader);
      reader.SetField<RadiationManager>(this, "m_lastMonthsPollutionQuantities", (object) MovingAverageHelper.Deserialize(reader));
      reader.SetField<RadiationManager>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<RadiationManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_productsRadiationTotalThisMonth = reader.ReadLong();
      this.m_reactorLeaksCountThisMonth = reader.ReadInt();
      reader.SetField<RadiationManager>(this, "m_safelyStoredProducts", (object) Dict<ProductProto, Quantity>.Deserialize(reader));
      reader.SetField<RadiationManager>(this, "m_stats", (object) Dict<ProductProto, ProductStats>.Deserialize(reader));
      reader.SetField<RadiationManager>(this, "Stats", (object) QuantitySumStats.Deserialize(reader));
    }

    static RadiationManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RadiationManager.GROWTH_PENALTY_PER_REACTOR = 4.Percent();
      RadiationManager.MAX_PENALTY_FROM_PRODUCTS = 2.5.Percent();
      RadiationManager.MOVING_AVG_WINDOW_SIZE = 4;
      RadiationManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RadiationManager) obj).SerializeData(writer));
      RadiationManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RadiationManager) obj).DeserializeData(reader));
    }
  }
}
