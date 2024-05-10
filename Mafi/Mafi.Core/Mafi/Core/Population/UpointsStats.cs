// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UpointsStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Population
{
  [GenerateSerializer(false, null, 0)]
  public class UpointsStats
  {
    private readonly Dict<UpointsStatsCategoryProto, Fix32SumStats> m_generatedStats;
    private readonly Dict<UpointsStatsCategoryProto, Fix32SumStats> m_consumedStats;
    public readonly Fix32AvgStats GeneratedTotalStats;
    public readonly Fix32AvgStats ConsumedTotalStats;
    private readonly Lyst<UpointsStats.Entry> m_thisMonthRecords;
    /// <summary>
    /// This avoids recording 1 unique demand multiple times per month.
    /// </summary>
    private readonly Set<IEntity> m_alreadyRegisteredConsumers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IReadOnlyDictionary<UpointsStatsCategoryProto, Fix32SumStats> GeneratedStats
    {
      get => (IReadOnlyDictionary<UpointsStatsCategoryProto, Fix32SumStats>) this.m_generatedStats;
    }

    public IReadOnlyDictionary<UpointsStatsCategoryProto, Fix32SumStats> ConsumedStats
    {
      get => (IReadOnlyDictionary<UpointsStatsCategoryProto, Fix32SumStats>) this.m_consumedStats;
    }

    public IIndexable<UpointsStats.Entry> ThisMonthRecords
    {
      get => (IIndexable<UpointsStats.Entry>) this.m_thisMonthRecords;
    }

    public UpointsStats(ProtosDb protosDb, StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_generatedStats = new Dict<UpointsStatsCategoryProto, Fix32SumStats>();
      this.m_consumedStats = new Dict<UpointsStatsCategoryProto, Fix32SumStats>();
      this.m_thisMonthRecords = new Lyst<UpointsStats.Entry>();
      this.m_alreadyRegisteredConsumers = new Set<IEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GeneratedTotalStats = new Fix32AvgStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.GeneratedTotalStats);
      this.ConsumedTotalStats = new Fix32AvgStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.ConsumedTotalStats);
      foreach (UpointsStatsCategoryProto key in protosDb.All<UpointsCategoryProto>().Select<UpointsCategoryProto, UpointsStatsCategoryProto>((Func<UpointsCategoryProto, UpointsStatsCategoryProto>) (x => x.StatsCategory)).Distinct<UpointsStatsCategoryProto>())
      {
        Fix32SumStats fix32SumStats1 = new Fix32SumStats(Option<StatsManager>.None, true);
        this.m_generatedStats[key] = fix32SumStats1;
        statsManager.RegisterForYearlyUpdatesOnly((ItemStats) fix32SumStats1);
        Fix32SumStats fix32SumStats2 = new Fix32SumStats(Option<StatsManager>.None, true);
        this.m_consumedStats[key] = fix32SumStats2;
        statsManager.RegisterForYearlyUpdatesOnly((ItemStats) fix32SumStats2);
      }
    }

    public void AddNewDemand(
      UpointsCategoryProto category,
      Upoints consumed,
      Upoints max,
      Option<IEntity> consumer,
      LocStr? extraTitle)
    {
      if (max.IsNotPositive && consumed.IsNotPositive)
        return;
      Assert.That<Upoints>(consumed).IsNotNegative();
      Assert.That<Upoints>(max).IsNotNegative();
      Assert.That<Upoints>(consumed).IsLessOrEqual(max);
      if (consumer.HasValue)
      {
        if (this.m_alreadyRegisteredConsumers.Contains(consumer.Value))
          return;
        this.m_alreadyRegisteredConsumers.Add(consumer.Value);
      }
      this.m_thisMonthRecords.Add(new UpointsStats.Entry(category, -consumed, -max, extraTitle ?? category.Title));
      if (!consumed.IsPositive)
        return;
      Fix32SumStats fix32SumStats;
      if (!category.IgnoreInStats && this.m_consumedStats.TryGetValue(category.StatsCategory, out fix32SumStats))
        fix32SumStats.Add(consumed.Value);
      this.ConsumedTotalStats.AddToCurrent(consumed.Value);
    }

    public void AddNewProduction(
      UpointsCategoryProto category,
      Upoints generated,
      Upoints max,
      LocStr? extraTitle)
    {
      Assert.That<Upoints>(generated).IsNotNegative();
      this.m_thisMonthRecords.Add(new UpointsStats.Entry(category, generated, max, extraTitle ?? category.Title));
      if (!generated.IsPositive)
        return;
      Fix32SumStats fix32SumStats;
      if (!category.IgnoreInStats && this.m_generatedStats.TryGetValue(category.StatsCategory, out fix32SumStats))
        fix32SumStats.Add(generated.Value);
      this.GeneratedTotalStats.AddToCurrent(generated.Value);
    }

    internal void ClearOnNewMonthStart()
    {
      this.m_thisMonthRecords.Clear();
      this.m_alreadyRegisteredConsumers.Clear();
      ((IItemStatsEvents) this.GeneratedTotalStats).OnNewMonth();
      ((IItemStatsEvents) this.ConsumedTotalStats).OnNewMonth();
      foreach (IItemStatsEvents itemStatsEvents in this.m_generatedStats.Values)
        itemStatsEvents.OnNewMonth();
      foreach (IItemStatsEvents itemStatsEvents in this.m_consumedStats.Values)
        itemStatsEvents.OnNewMonth();
    }

    public static void Serialize(UpointsStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpointsStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpointsStats.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32AvgStats.Serialize(this.ConsumedTotalStats, writer);
      Fix32AvgStats.Serialize(this.GeneratedTotalStats, writer);
      Set<IEntity>.Serialize(this.m_alreadyRegisteredConsumers, writer);
      Dict<UpointsStatsCategoryProto, Fix32SumStats>.Serialize(this.m_consumedStats, writer);
      Dict<UpointsStatsCategoryProto, Fix32SumStats>.Serialize(this.m_generatedStats, writer);
      Lyst<UpointsStats.Entry>.Serialize(this.m_thisMonthRecords, writer);
    }

    public static UpointsStats Deserialize(BlobReader reader)
    {
      UpointsStats upointsStats;
      if (reader.TryStartClassDeserialization<UpointsStats>(out upointsStats))
        reader.EnqueueDataDeserialization((object) upointsStats, UpointsStats.s_deserializeDataDelayedAction);
      return upointsStats;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UpointsStats>(this, "ConsumedTotalStats", (object) Fix32AvgStats.Deserialize(reader));
      reader.SetField<UpointsStats>(this, "GeneratedTotalStats", (object) Fix32AvgStats.Deserialize(reader));
      reader.SetField<UpointsStats>(this, "m_alreadyRegisteredConsumers", (object) Set<IEntity>.Deserialize(reader));
      reader.SetField<UpointsStats>(this, "m_consumedStats", (object) Dict<UpointsStatsCategoryProto, Fix32SumStats>.Deserialize(reader));
      reader.SetField<UpointsStats>(this, "m_generatedStats", (object) Dict<UpointsStatsCategoryProto, Fix32SumStats>.Deserialize(reader));
      reader.SetField<UpointsStats>(this, "m_thisMonthRecords", (object) Lyst<UpointsStats.Entry>.Deserialize(reader));
    }

    static UpointsStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpointsStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpointsStats) obj).SerializeData(writer));
      UpointsStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpointsStats) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct Entry
    {
      public readonly Upoints Exchanged;
      public readonly Upoints Max;
      public readonly LocStr Title;
      public readonly UpointsCategoryProto Category;

      public Entry(UpointsCategoryProto category, Upoints exchanged, Upoints max, LocStr title)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Category = category;
        this.Exchanged = exchanged;
        this.Title = title;
        this.Max = max;
      }

      public static void Serialize(UpointsStats.Entry value, BlobWriter writer)
      {
        writer.WriteGeneric<UpointsCategoryProto>(value.Category);
        Upoints.Serialize(value.Exchanged, writer);
        Upoints.Serialize(value.Max, writer);
        LocStr.Serialize(value.Title, writer);
      }

      public static UpointsStats.Entry Deserialize(BlobReader reader)
      {
        return new UpointsStats.Entry(reader.ReadGenericAs<UpointsCategoryProto>(), Upoints.Deserialize(reader), Upoints.Deserialize(reader), LocStr.Deserialize(reader));
      }
    }
  }
}
