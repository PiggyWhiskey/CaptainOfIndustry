// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.Crop
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.PropertiesDb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_noWaterLostYieldPercSum", 43, typeof (Percent), 0, false)]
  public class Crop
  {
    public readonly CropProto Prototype;
    public readonly Percent ConsumedFertilityPerDay;
    private readonly PartialQuantity m_consumedWaterPerDay;
    public readonly ProductQuantity ProductProduced;
    public readonly bool HasFertilityPenalty;
    private Percent m_yieldPercSum;
    private IProperty<Percent> m_yieldMultiplier;
    private Percent m_yieldMultiplierSum;
    private IProperty<Percent> m_waterConsumptionMultiplier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PartialQuantity ConsumedWaterPerDay
    {
      get => this.m_consumedWaterPerDay.ScaledBy(this.m_waterConsumptionMultiplier.Value);
    }

    public bool IsStarted => this.DaysRemaining < this.Prototype.DaysToGrow;

    /// <summary>Used for UI to show crops growth.</summary>
    public Percent GrowthPercent
    {
      get
      {
        return Percent.FromRatio(this.Prototype.DaysToGrow - this.DaysRemaining, this.Prototype.DaysToGrow);
      }
    }

    public Percent DrynessPercent
    {
      get
      {
        return !this.Prototype.DaysToSurviveWithNoWater.HasValue ? Percent.Zero : Percent.FromRatio(this.DaysMissingWater, this.Prototype.DaysToSurviveWithNoWater.Value);
      }
    }

    public bool WillDrySoon
    {
      get
      {
        int? surviveWithNoWater = this.Prototype.DaysToSurviveWithNoWater;
        int daysMissingWater = this.DaysMissingWater;
        int? nullable = surviveWithNoWater.HasValue ? new int?(surviveWithNoWater.GetValueOrDefault() - daysMissingWater) : new int?();
        int num = 14;
        return nullable.GetValueOrDefault() < num & nullable.HasValue;
      }
    }

    public int DaysRemaining { get; private set; }

    public bool IsMissingWater { get; private set; }

    public int DaysMissingWater { get; private set; }

    public int TotalDaysWithoutWater { get; private set; }

    public bool HarvestWillYieldProducts => this.GrowthPercent >= Farm.NO_YIELD_BEFORE_GROWTH_PERC;

    /// <summary>
    /// Yield of this crop. This has valid value only after harvest (when <see cref="P:Mafi.Core.Buildings.Farms.Crop.HarvestReason" />
    /// is not <see cref="F:Mafi.Core.Buildings.Farms.CropHarvestReason.None" />).
    /// </summary>
    public ProductQuantity Yield { get; private set; }

    public CropHarvestReason HarvestReason { get; private set; }

    /// <summary>
    /// How much fertility has affected the final yield. Negative percentage means that some yield was lost because
    /// fertility was below 100%, positive value means that fertility was above 100% on average. This value is only
    /// valid after harvest.
    /// </summary>
    public Percent YieldDeltaDueToFertility { get; private set; }

    /// <summary>
    /// How much has a yield multiplier affected the final yield. This comes typically from edicts.
    /// </summary>
    public Percent YieldDeltaDueToBonusMultiplier { get; private set; }

    /// <summary>How much yield was lost due to lack of water.</summary>
    public Percent YieldLostDueToLackOfWater { get; private set; }

    /// <summary>How much yield was lost due to premature harvest.</summary>
    public Percent YieldLostDueToPrematureHarvest { get; private set; }

    /// <summary>
    /// How many days was crop waiting for water before growth started.
    /// </summary>
    public int DaysWaitingForWaterBeforeGrowthStart { get; private set; }

    public Crop(CropProto proto, FarmProto farm, IPropertiesDb propsDb, Percent fertilityPenalty)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CYield\u003Ek__BackingField = ProductQuantity.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = proto;
      this.DaysRemaining = this.Prototype.DaysToGrow;
      this.m_consumedWaterPerDay = proto.GetConsumedWaterPerDay(farm);
      this.ProductProduced = proto.GetProductProduced(farm);
      Percent consumedFertilityPerDay = proto.GetConsumedFertilityPerDay(farm);
      this.ConsumedFertilityPerDay = consumedFertilityPerDay + consumedFertilityPerDay.ScaleBy(fertilityPenalty);
      this.HasFertilityPenalty = fertilityPenalty.IsPositive;
      this.m_yieldMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmYieldMultiplier);
      this.m_waterConsumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier);
    }

    public Percent GetYieldSoFar() => this.m_yieldPercSum / this.Prototype.DaysToGrow;

    public void ReportWaitingForWater() => ++this.DaysWaitingForWaterBeforeGrowthStart;

    public void RecordGrowthDay(Percent fertility)
    {
      Assert.That<Percent>(fertility).IsNotNegative();
      Assert.That<int>(this.DaysRemaining).IsPositive("Crop is overgrown");
      Assert.That<CropHarvestReason>(this.HarvestReason).IsEqualTo<CropHarvestReason>(CropHarvestReason.None, "Crop was already harvested.");
      --this.DaysRemaining;
      this.IsMissingWater = false;
      this.m_yieldPercSum += fertility;
      this.m_yieldMultiplierSum += this.m_yieldMultiplier.Value;
      if (this.DaysMissingWater <= 0)
        return;
      this.DaysMissingWater = (this.DaysMissingWater - 3).Max(0);
    }

    public void RecordGrowthDayNoWater(Percent fertility, out bool driedOut)
    {
      Assert.That<int>(this.DaysRemaining).IsPositive("Crop is overgrown");
      Assert.That<CropHarvestReason>(this.HarvestReason).IsEqualTo<CropHarvestReason>(CropHarvestReason.None, "Crop was already harvested.");
      --this.DaysRemaining;
      this.IsMissingWater = true;
      ++this.DaysMissingWater;
      ++this.TotalDaysWithoutWater;
      this.m_yieldMultiplierSum += this.m_yieldMultiplier.Value;
      ref bool local = ref driedOut;
      int daysMissingWater = this.DaysMissingWater;
      int? surviveWithNoWater = this.Prototype.DaysToSurviveWithNoWater;
      int valueOrDefault = surviveWithNoWater.GetValueOrDefault();
      int num = daysMissingWater >= valueOrDefault & surviveWithNoWater.HasValue ? 1 : 0;
      local = num != 0;
    }

    public ProductQuantity Harvest(CropHarvestReason harvestReason)
    {
      Assert.That<ProductQuantity>(this.Yield).IsEmpty("Crop was already harvested?");
      Assert.That<CropHarvestReason>(this.HarvestReason).IsEqualTo<CropHarvestReason>(CropHarvestReason.None, "Crop was already harvested?");
      this.HarvestReason = harvestReason;
      if (this.HarvestWillYieldProducts)
      {
        Percent percent1 = this.m_yieldPercSum / this.Prototype.DaysToGrow * this.GrowthPercent;
        Percent percent2 = this.m_yieldMultiplierSum / this.Prototype.DaysToGrow;
        this.Yield = this.ProductProduced.ScaledBy(percent1).ScaledBy(percent2);
        int num = this.Prototype.DaysToGrow - this.TotalDaysWithoutWater - this.DaysRemaining;
        this.YieldDeltaDueToFertility = num > 0 ? this.m_yieldPercSum / num - Percent.Hundred : Percent.Zero;
        this.YieldLostDueToLackOfWater = Percent.FromRatio(this.TotalDaysWithoutWater, this.Prototype.DaysToGrow);
        this.YieldDeltaDueToBonusMultiplier = (percent2 - Percent.Hundred).Max(Percent.Zero);
        this.YieldLostDueToPrematureHarvest = Percent.Hundred - this.GrowthPercent;
      }
      else
      {
        this.Yield = ProductQuantity.NoneOf(this.ProductProduced.Product);
        this.YieldDeltaDueToFertility = harvestReason == CropHarvestReason.PrematureNoFertility ? -Percent.Hundred : Percent.Zero;
        this.YieldLostDueToLackOfWater = harvestReason == CropHarvestReason.PrematureNoWater ? Percent.Hundred : Percent.Zero;
        this.YieldDeltaDueToBonusMultiplier = Percent.Zero;
        this.YieldLostDueToPrematureHarvest = Percent.Hundred;
      }
      return this.Yield;
    }

    /// <summary>Returns yield if crop was harvested now.</summary>
    public Quantity GetHarvestEstimate()
    {
      return this.ProductProduced.Quantity.ScaledBy(this.m_yieldPercSum / this.Prototype.DaysToGrow * (this.HarvestWillYieldProducts ? this.GrowthPercent : Percent.Zero));
    }

    public static void Serialize(Crop value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Crop>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Crop.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.ConsumedFertilityPerDay, writer);
      writer.WriteInt(this.DaysMissingWater);
      writer.WriteInt(this.DaysRemaining);
      writer.WriteInt(this.DaysWaitingForWaterBeforeGrowthStart);
      writer.WriteInt((int) this.HarvestReason);
      writer.WriteBool(this.HasFertilityPenalty);
      writer.WriteBool(this.IsMissingWater);
      PartialQuantity.Serialize(this.m_consumedWaterPerDay, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_waterConsumptionMultiplier);
      writer.WriteGeneric<IProperty<Percent>>(this.m_yieldMultiplier);
      Percent.Serialize(this.m_yieldMultiplierSum, writer);
      Percent.Serialize(this.m_yieldPercSum, writer);
      ProductQuantity.Serialize(this.ProductProduced, writer);
      writer.WriteGeneric<CropProto>(this.Prototype);
      writer.WriteInt(this.TotalDaysWithoutWater);
      ProductQuantity.Serialize(this.Yield, writer);
      Percent.Serialize(this.YieldDeltaDueToBonusMultiplier, writer);
      Percent.Serialize(this.YieldDeltaDueToFertility, writer);
      Percent.Serialize(this.YieldLostDueToLackOfWater, writer);
      Percent.Serialize(this.YieldLostDueToPrematureHarvest, writer);
    }

    public static Crop Deserialize(BlobReader reader)
    {
      Crop crop;
      if (reader.TryStartClassDeserialization<Crop>(out crop))
        reader.EnqueueDataDeserialization((object) crop, Crop.s_deserializeDataDelayedAction);
      return crop;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Crop>(this, "ConsumedFertilityPerDay", (object) Percent.Deserialize(reader));
      this.DaysMissingWater = reader.ReadInt();
      this.DaysRemaining = reader.ReadInt();
      this.DaysWaitingForWaterBeforeGrowthStart = reader.ReadInt();
      this.HarvestReason = (CropHarvestReason) reader.ReadInt();
      reader.SetField<Crop>(this, "HasFertilityPenalty", (object) reader.ReadBool());
      this.IsMissingWater = reader.ReadBool();
      reader.SetField<Crop>(this, "m_consumedWaterPerDay", (object) PartialQuantity.Deserialize(reader));
      if (reader.LoadedSaveVersion < 43)
        Percent.Deserialize(reader);
      this.m_waterConsumptionMultiplier = reader.ReadGenericAs<IProperty<Percent>>();
      this.m_yieldMultiplier = reader.ReadGenericAs<IProperty<Percent>>();
      this.m_yieldMultiplierSum = Percent.Deserialize(reader);
      this.m_yieldPercSum = Percent.Deserialize(reader);
      reader.SetField<Crop>(this, "ProductProduced", (object) ProductQuantity.Deserialize(reader));
      reader.SetField<Crop>(this, "Prototype", (object) reader.ReadGenericAs<CropProto>());
      this.TotalDaysWithoutWater = reader.ReadInt();
      this.Yield = ProductQuantity.Deserialize(reader);
      this.YieldDeltaDueToBonusMultiplier = Percent.Deserialize(reader);
      this.YieldDeltaDueToFertility = Percent.Deserialize(reader);
      this.YieldLostDueToLackOfWater = Percent.Deserialize(reader);
      this.YieldLostDueToPrematureHarvest = Percent.Deserialize(reader);
    }

    static Crop()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Crop.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Crop) obj).SerializeData(writer));
      Crop.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Crop) obj).DeserializeData(reader));
    }
  }
}
