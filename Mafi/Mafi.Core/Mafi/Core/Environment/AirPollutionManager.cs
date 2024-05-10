// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.AirPollutionManager
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
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class AirPollutionManager : IVirtualBufferProvider
  {
    public readonly QuantitySumStats Stats;
    [NewInSaveVersion(140, null, null, null, null)]
    public readonly QuantitySumStats StatsVehicles;
    [NewInSaveVersion(140, null, null, null, null)]
    public readonly QuantitySumStats StatsShips;
    private readonly AirPollutionManager.PollutedAirProductBuffer m_pollutedAirBuffer;
    [NewInSaveVersion(140, null, null, null, null)]
    private PartialQuantity m_vehiclesPollutionBufferPartial;
    [NewInSaveVersion(140, null, null, null, null)]
    private PartialQuantity m_shipsPollutionBufferPartial;
    [NewInSaveVersion(140, null, null, null, null)]
    private Quantity m_vehiclesPollutionBuffer;
    [NewInSaveVersion(140, null, null, null, null)]
    private Quantity m_shipsPollutionBuffer;
    private readonly IProductsManager m_productsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly ProductProto m_pollutedAir;
    private static readonly Percent HEALTH_PENALTY_PER_QUANTITY;
    private static readonly Percent MAX_HEALTH_PENALTY;
    private static readonly int MOVING_AVG_WINDOW_SIZE;
    private readonly MovingAverageHelper m_lastMonthsPollutionQuantities;
    [NewInSaveVersion(140, null, "new MovingAverageHelper(MOVING_AVG_WINDOW_SIZE)", null, null)]
    private readonly MovingAverageHelper m_lastMonthsVehiclesPollutionQuantities;
    [NewInSaveVersion(140, null, "new MovingAverageHelper(MOVING_AVG_WINDOW_SIZE)", null, null)]
    private readonly MovingAverageHelper m_lastMonthsShipsPollutionQuantities;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_airPollutionMultiplier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<ProductProto> ProvidedProducts { get; private set; }

    public Percent LastMonthHealthPenalty { get; private set; }

    public AirPollutionManager(
      ICalendar calendar,
      IPropertiesDb propertiesDb,
      IProductsManager productsManager,
      PopsHealthManager popsHealthManager,
      [ProtoDep("Product_Virtual_PollutedAir")] ProductProto pollutedAir,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastMonthsPollutionQuantities = new MovingAverageHelper(AirPollutionManager.MOVING_AVG_WINDOW_SIZE);
      this.m_lastMonthsVehiclesPollutionQuantities = new MovingAverageHelper(AirPollutionManager.MOVING_AVG_WINDOW_SIZE);
      this.m_lastMonthsShipsPollutionQuantities = new MovingAverageHelper(AirPollutionManager.MOVING_AVG_WINDOW_SIZE);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_pollutedAir = pollutedAir;
      this.m_pollutedAirBuffer = new AirPollutionManager.PollutedAirProductBuffer(Quantity.MaxValue, this.m_pollutedAir, this);
      this.m_airPollutionMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.AirPollutionMultiplier);
      this.ProvidedProducts = ImmutableArray.Create<ProductProto>(pollutedAir);
      this.Stats = new QuantitySumStats((Option<StatsManager>) statsManager);
      this.StatsVehicles = new QuantitySumStats((Option<StatsManager>) statsManager);
      this.StatsShips = new QuantitySumStats((Option<StatsManager>) statsManager);
      calendar.NewMonth.Add<AirPollutionManager>(this, new Action(this.onNewMonth));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<AirPollutionManager>(this, "m_airPollutionMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.AirPollutionMultiplier));
      StatsManager manager = resolver.Resolve<StatsManager>();
      ReflectionUtils.SetField<AirPollutionManager>(this, "StatsVehicles", (object) new QuantitySumStats((Option<StatsManager>) manager));
      ReflectionUtils.SetField<AirPollutionManager>(this, "StatsShips", (object) new QuantitySumStats((Option<StatsManager>) manager));
    }

    private void onNewMonth()
    {
      this.m_lastMonthsVehiclesPollutionQuantities.AddItem(this.m_vehiclesPollutionBuffer.Value);
      this.m_vehiclesPollutionBuffer = Quantity.Zero;
      int avg1 = this.m_lastMonthsVehiclesPollutionQuantities.GetAvg();
      if (avg1 > 0)
      {
        int num = avg1.ScaledByRounded(this.m_airPollutionMultiplier.Value);
        Percent reduction = (AirPollutionManager.HEALTH_PENALTY_PER_QUANTITY * num).Min(AirPollutionManager.MAX_HEALTH_PENALTY);
        this.m_popsHealthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.AirPollutionVehicles, reduction);
      }
      this.m_lastMonthsShipsPollutionQuantities.AddItem(this.m_shipsPollutionBuffer.Value);
      this.m_shipsPollutionBuffer = Quantity.Zero;
      int avg2 = this.m_lastMonthsShipsPollutionQuantities.GetAvg();
      if (avg2 > 0)
      {
        int num = avg2.ScaledByRounded(this.m_airPollutionMultiplier.Value);
        Percent reduction = (AirPollutionManager.HEALTH_PENALTY_PER_QUANTITY * num).Min(AirPollutionManager.MAX_HEALTH_PENALTY);
        this.m_popsHealthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.AirPollutionShips, reduction);
      }
      this.m_lastMonthsPollutionQuantities.AddItem(this.m_pollutedAirBuffer.Quantity.Value);
      if (this.m_pollutedAirBuffer.IsNotEmpty())
      {
        this.m_productsManager.ProductDestroyed(this.m_pollutedAir, this.m_pollutedAirBuffer.Quantity, DestroyReason.Wasted);
        this.m_pollutedAirBuffer.Clear();
      }
      int avg3 = this.m_lastMonthsPollutionQuantities.GetAvg();
      if (avg3 > 0)
      {
        int num = avg3.ScaledByRounded(this.m_airPollutionMultiplier.Value);
        this.LastMonthHealthPenalty = (AirPollutionManager.HEALTH_PENALTY_PER_QUANTITY * num).Min(AirPollutionManager.MAX_HEALTH_PENALTY);
        this.m_popsHealthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.AirPollution, this.LastMonthHealthPenalty);
      }
      else
        this.LastMonthHealthPenalty = Percent.Zero;
    }

    public Option<IProductBuffer> GetBuffer(ProductProto product, Option<IEntity> entity)
    {
      return !((Proto) product == (Proto) this.m_pollutedAir) ? Option<IProductBuffer>.None : (Option<IProductBuffer>) (IProductBuffer) this.m_pollutedAirBuffer;
    }

    public void EmitVehiclePollution(PartialQuantity quantity)
    {
      this.m_vehiclesPollutionBufferPartial += quantity;
      Quantity integerPart = this.m_vehiclesPollutionBufferPartial.IntegerPart;
      if (!integerPart.IsPositive)
        return;
      this.m_vehiclesPollutionBuffer += integerPart;
      this.m_vehiclesPollutionBufferPartial -= integerPart.AsPartial;
      this.StatsVehicles.Add((QuantityLarge) integerPart);
    }

    public void EmitShipPollution(PartialQuantity quantity)
    {
      this.m_shipsPollutionBufferPartial += quantity;
      Quantity integerPart = this.m_shipsPollutionBufferPartial.IntegerPart;
      if (!integerPart.IsPositive)
        return;
      this.m_shipsPollutionBuffer += integerPart;
      this.m_shipsPollutionBufferPartial -= integerPart.AsPartial;
      this.StatsShips.Add((QuantityLarge) integerPart);
    }

    public static void Serialize(AirPollutionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AirPollutionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AirPollutionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.LastMonthHealthPenalty, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_airPollutionMultiplier);
      MovingAverageHelper.Serialize(this.m_lastMonthsPollutionQuantities, writer);
      MovingAverageHelper.Serialize(this.m_lastMonthsShipsPollutionQuantities, writer);
      MovingAverageHelper.Serialize(this.m_lastMonthsVehiclesPollutionQuantities, writer);
      writer.WriteGeneric<ProductProto>(this.m_pollutedAir);
      AirPollutionManager.PollutedAirProductBuffer.Serialize(this.m_pollutedAirBuffer, writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Quantity.Serialize(this.m_shipsPollutionBuffer, writer);
      PartialQuantity.Serialize(this.m_shipsPollutionBufferPartial, writer);
      Quantity.Serialize(this.m_vehiclesPollutionBuffer, writer);
      PartialQuantity.Serialize(this.m_vehiclesPollutionBufferPartial, writer);
      ImmutableArray<ProductProto>.Serialize(this.ProvidedProducts, writer);
      QuantitySumStats.Serialize(this.Stats, writer);
      QuantitySumStats.Serialize(this.StatsShips, writer);
      QuantitySumStats.Serialize(this.StatsVehicles, writer);
    }

    public static AirPollutionManager Deserialize(BlobReader reader)
    {
      AirPollutionManager pollutionManager;
      if (reader.TryStartClassDeserialization<AirPollutionManager>(out pollutionManager))
        reader.EnqueueDataDeserialization((object) pollutionManager, AirPollutionManager.s_deserializeDataDelayedAction);
      return pollutionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.LastMonthHealthPenalty = Percent.Deserialize(reader);
      reader.SetField<AirPollutionManager>(this, "m_airPollutionMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<AirPollutionManager>(this, "m_lastMonthsPollutionQuantities", (object) MovingAverageHelper.Deserialize(reader));
      reader.SetField<AirPollutionManager>(this, "m_lastMonthsShipsPollutionQuantities", reader.LoadedSaveVersion >= 140 ? (object) MovingAverageHelper.Deserialize(reader) : (object) new MovingAverageHelper(AirPollutionManager.MOVING_AVG_WINDOW_SIZE));
      reader.SetField<AirPollutionManager>(this, "m_lastMonthsVehiclesPollutionQuantities", reader.LoadedSaveVersion >= 140 ? (object) MovingAverageHelper.Deserialize(reader) : (object) new MovingAverageHelper(AirPollutionManager.MOVING_AVG_WINDOW_SIZE));
      reader.SetField<AirPollutionManager>(this, "m_pollutedAir", (object) reader.ReadGenericAs<ProductProto>());
      reader.SetField<AirPollutionManager>(this, "m_pollutedAirBuffer", (object) AirPollutionManager.PollutedAirProductBuffer.Deserialize(reader));
      reader.SetField<AirPollutionManager>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<AirPollutionManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_shipsPollutionBuffer = reader.LoadedSaveVersion >= 140 ? Quantity.Deserialize(reader) : new Quantity();
      this.m_shipsPollutionBufferPartial = reader.LoadedSaveVersion >= 140 ? PartialQuantity.Deserialize(reader) : new PartialQuantity();
      this.m_vehiclesPollutionBuffer = reader.LoadedSaveVersion >= 140 ? Quantity.Deserialize(reader) : new Quantity();
      this.m_vehiclesPollutionBufferPartial = reader.LoadedSaveVersion >= 140 ? PartialQuantity.Deserialize(reader) : new PartialQuantity();
      this.ProvidedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      reader.SetField<AirPollutionManager>(this, "Stats", (object) QuantitySumStats.Deserialize(reader));
      reader.SetField<AirPollutionManager>(this, "StatsShips", reader.LoadedSaveVersion >= 140 ? (object) QuantitySumStats.Deserialize(reader) : (object) (QuantitySumStats) null);
      reader.SetField<AirPollutionManager>(this, "StatsVehicles", reader.LoadedSaveVersion >= 140 ? (object) QuantitySumStats.Deserialize(reader) : (object) (QuantitySumStats) null);
      reader.RegisterInitAfterLoad<AirPollutionManager>(this, "initSelf", InitPriority.Normal);
    }

    static AirPollutionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AirPollutionManager.HEALTH_PENALTY_PER_QUANTITY = 0.044.Percent();
      AirPollutionManager.MAX_HEALTH_PENALTY = 40.Percent();
      AirPollutionManager.MOVING_AVG_WINDOW_SIZE = 4;
      AirPollutionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AirPollutionManager) obj).SerializeData(writer));
      AirPollutionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AirPollutionManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class PollutedAirProductBuffer : ProductBuffer
    {
      private readonly AirPollutionManager m_manager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public PollutedAirProductBuffer(
        Quantity capacity,
        ProductProto product,
        AirPollutionManager manager)
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
        AirPollutionManager.PollutedAirProductBuffer value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<AirPollutionManager.PollutedAirProductBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, AirPollutionManager.PollutedAirProductBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        AirPollutionManager.Serialize(this.m_manager, writer);
      }

      public static AirPollutionManager.PollutedAirProductBuffer Deserialize(BlobReader reader)
      {
        AirPollutionManager.PollutedAirProductBuffer airProductBuffer;
        if (reader.TryStartClassDeserialization<AirPollutionManager.PollutedAirProductBuffer>(out airProductBuffer))
          reader.EnqueueDataDeserialization((object) airProductBuffer, AirPollutionManager.PollutedAirProductBuffer.s_deserializeDataDelayedAction);
        return airProductBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<AirPollutionManager.PollutedAirProductBuffer>(this, "m_manager", (object) AirPollutionManager.Deserialize(reader));
      }

      static PollutedAirProductBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        AirPollutionManager.PollutedAirProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        AirPollutionManager.PollutedAirProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
