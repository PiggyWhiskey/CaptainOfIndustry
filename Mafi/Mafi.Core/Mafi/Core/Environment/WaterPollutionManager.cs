// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.WaterPollutionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Core.Environment
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class WaterPollutionManager : IVirtualBufferProvider
  {
    public readonly QuantitySumStats Stats;
    private readonly WaterPollutionManager.PollutedWaterProductBuffer m_pollutedWaterBuffer;
    private readonly IProductsManager m_productsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly ProductProto m_pollutedWater;
    /// <summary>
    /// We expect that reasonably early mid game settlements has approx 400 pops (20 waste water per month)
    /// and approx. 0-24 unused sour water. That leaves us to 40 as a nice spot from which we start reducing
    /// pop growth for pollution. Anything close to 90 gets 0.5% growth penalty which is close to devastating
    /// however that requires settlement with 7-800 pops + some extra waste, at that point waste treatment
    /// is expected. So we might need to increase the grow even a bit more.
    /// </summary>
    private static readonly Percent HEALTH_PENALTY_PER_QUANTITY;
    private static readonly Percent MAX_HEALTH_PENALTY;
    /// <summary>
    /// From heavily fertilized farm we can except 40+ fertility per year above allowance. In case of 10 farms
    /// that is 400 * 2 =&gt; 800 quantity ending up with 10% of pollution per year. That makes 0.66% per month on average.
    /// </summary>
    private static readonly int MOVING_AVG_WINDOW_SIZE;
    private readonly MovingAverageHelper m_lastMonthsPollutionQuantities;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_waterPollutionMultiplier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<ProductProto> ProvidedProducts { get; private set; }

    public Percent LastMonthHealthPenalty { get; private set; }

    public IProductBufferReadOnly Buffer => (IProductBufferReadOnly) this.m_pollutedWaterBuffer;

    public WaterPollutionManager(
      ICalendar calendar,
      IPropertiesDb propertiesDb,
      IProductsManager productsManager,
      PopsHealthManager popsHealthManager,
      [ProtoDep("Product_Virtual_PollutedWater")] ProductProto pollutedWater,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastMonthsPollutionQuantities = new MovingAverageHelper(WaterPollutionManager.MOVING_AVG_WINDOW_SIZE);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_pollutedWater = pollutedWater;
      this.m_pollutedWaterBuffer = new WaterPollutionManager.PollutedWaterProductBuffer(Quantity.MaxValue, this.m_pollutedWater, this);
      this.m_waterPollutionMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.WaterPollutionMultiplier);
      this.ProvidedProducts = ImmutableArray.Create<ProductProto>(pollutedWater);
      this.Stats = new QuantitySumStats((Option<StatsManager>) statsManager);
      calendar.NewMonth.Add<WaterPollutionManager>(this, new Action(this.onNewMonth));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<WaterPollutionManager>(this, "m_waterPollutionMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.WaterPollutionMultiplier));
    }

    private void onNewMonth()
    {
      this.m_lastMonthsPollutionQuantities.AddItem(this.m_pollutedWaterBuffer.Quantity.Value);
      if (this.m_pollutedWaterBuffer.IsNotEmpty())
      {
        this.m_productsManager.ProductDestroyed(this.m_pollutedWater, this.m_pollutedWaterBuffer.Quantity, DestroyReason.Wasted);
        this.m_pollutedWaterBuffer.Clear();
      }
      int avg = this.m_lastMonthsPollutionQuantities.GetAvg();
      if (avg > 0)
      {
        int num = avg.ScaledByRounded(this.m_waterPollutionMultiplier.Value);
        this.LastMonthHealthPenalty = (WaterPollutionManager.HEALTH_PENALTY_PER_QUANTITY * num).Min(WaterPollutionManager.MAX_HEALTH_PENALTY);
        this.m_popsHealthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.WaterPollution, this.LastMonthHealthPenalty);
      }
      else
        this.LastMonthHealthPenalty = Percent.Zero;
    }

    public Option<IProductBuffer> GetBuffer(ProductProto product, Option<IEntity> entity)
    {
      return !((Proto) product == (Proto) this.m_pollutedWater) ? Option<IProductBuffer>.None : (Option<IProductBuffer>) (IProductBuffer) this.m_pollutedWaterBuffer;
    }

    public static void Serialize(WaterPollutionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaterPollutionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaterPollutionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.LastMonthHealthPenalty, writer);
      MovingAverageHelper.Serialize(this.m_lastMonthsPollutionQuantities, writer);
      writer.WriteGeneric<ProductProto>(this.m_pollutedWater);
      WaterPollutionManager.PollutedWaterProductBuffer.Serialize(this.m_pollutedWaterBuffer, writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_waterPollutionMultiplier);
      ImmutableArray<ProductProto>.Serialize(this.ProvidedProducts, writer);
      QuantitySumStats.Serialize(this.Stats, writer);
    }

    public static WaterPollutionManager Deserialize(BlobReader reader)
    {
      WaterPollutionManager pollutionManager;
      if (reader.TryStartClassDeserialization<WaterPollutionManager>(out pollutionManager))
        reader.EnqueueDataDeserialization((object) pollutionManager, WaterPollutionManager.s_deserializeDataDelayedAction);
      return pollutionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.LastMonthHealthPenalty = Percent.Deserialize(reader);
      reader.SetField<WaterPollutionManager>(this, "m_lastMonthsPollutionQuantities", (object) MovingAverageHelper.Deserialize(reader));
      reader.SetField<WaterPollutionManager>(this, "m_pollutedWater", (object) reader.ReadGenericAs<ProductProto>());
      reader.SetField<WaterPollutionManager>(this, "m_pollutedWaterBuffer", (object) WaterPollutionManager.PollutedWaterProductBuffer.Deserialize(reader));
      reader.SetField<WaterPollutionManager>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<WaterPollutionManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<WaterPollutionManager>(this, "m_waterPollutionMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      this.ProvidedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      reader.SetField<WaterPollutionManager>(this, "Stats", (object) QuantitySumStats.Deserialize(reader));
      reader.RegisterInitAfterLoad<WaterPollutionManager>(this, "initSelf", InitPriority.Normal);
    }

    static WaterPollutionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaterPollutionManager.HEALTH_PENALTY_PER_QUANTITY = 0.11.Percent();
      WaterPollutionManager.MAX_HEALTH_PENALTY = 40.Percent();
      WaterPollutionManager.MOVING_AVG_WINDOW_SIZE = 4;
      WaterPollutionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaterPollutionManager) obj).SerializeData(writer));
      WaterPollutionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaterPollutionManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class PollutedWaterProductBuffer : ProductBuffer
    {
      private readonly WaterPollutionManager m_manager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public PollutedWaterProductBuffer(
        Quantity capacity,
        ProductProto product,
        WaterPollutionManager manager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_manager = manager;
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        if (!diff.IsPositive)
          return;
        this.m_manager.Stats.Add((QuantityLarge) diff);
      }

      public static void Serialize(
        WaterPollutionManager.PollutedWaterProductBuffer value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WaterPollutionManager.PollutedWaterProductBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WaterPollutionManager.PollutedWaterProductBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        WaterPollutionManager.Serialize(this.m_manager, writer);
      }

      public static WaterPollutionManager.PollutedWaterProductBuffer Deserialize(BlobReader reader)
      {
        WaterPollutionManager.PollutedWaterProductBuffer waterProductBuffer;
        if (reader.TryStartClassDeserialization<WaterPollutionManager.PollutedWaterProductBuffer>(out waterProductBuffer))
          reader.EnqueueDataDeserialization((object) waterProductBuffer, WaterPollutionManager.PollutedWaterProductBuffer.s_deserializeDataDelayedAction);
        return waterProductBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<WaterPollutionManager.PollutedWaterProductBuffer>(this, "m_manager", (object) WaterPollutionManager.Deserialize(reader));
      }

      static PollutedWaterProductBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WaterPollutionManager.PollutedWaterProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        WaterPollutionManager.PollutedWaterProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
