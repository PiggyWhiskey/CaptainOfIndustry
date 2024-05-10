// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.HealthStatistics
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Population
{
  [GenerateSerializer(false, null, 0)]
  public class HealthStatistics
  {
    public Percent HealthThisMonth;
    public Percent HealthLastMonth;
    private Lyst<HealthStatistics.Entry> m_lastMonthRecords;
    private Lyst<HealthStatistics.Entry> m_thisMonthRecords;
    private readonly Dict<HealthPointsCategoryProto, IntAvgStats> m_generatedStats;
    private readonly Dict<HealthPointsCategoryProto, IntAvgStats> m_consumedStats;
    public readonly IntAvgStats GeneratedTotalStats;
    public readonly IntAvgStats ConsumedTotalStats;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IIndexable<HealthStatistics.Entry> LastMonthRecords
    {
      get => (IIndexable<HealthStatistics.Entry>) this.m_lastMonthRecords;
    }

    public IReadOnlyDictionary<HealthPointsCategoryProto, IntAvgStats> GeneratedStats
    {
      get => (IReadOnlyDictionary<HealthPointsCategoryProto, IntAvgStats>) this.m_generatedStats;
    }

    public IReadOnlyDictionary<HealthPointsCategoryProto, IntAvgStats> ConsumedStats
    {
      get => (IReadOnlyDictionary<HealthPointsCategoryProto, IntAvgStats>) this.m_consumedStats;
    }

    public HealthStatistics(ProtosDb protosDb, StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastMonthRecords = new Lyst<HealthStatistics.Entry>();
      this.m_thisMonthRecords = new Lyst<HealthStatistics.Entry>();
      this.m_generatedStats = new Dict<HealthPointsCategoryProto, IntAvgStats>();
      this.m_consumedStats = new Dict<HealthPointsCategoryProto, IntAvgStats>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GeneratedTotalStats = new IntAvgStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.GeneratedTotalStats);
      this.ConsumedTotalStats = new IntAvgStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.ConsumedTotalStats);
      foreach (HealthPointsCategoryProto key in protosDb.All<HealthPointsCategoryProto>())
      {
        IntAvgStats intAvgStats1 = new IntAvgStats(Option<StatsManager>.None, true);
        this.m_generatedStats[key] = intAvgStats1;
        statsManager.RegisterForYearlyUpdatesOnly((ItemStats) intAvgStats1);
        IntAvgStats intAvgStats2 = new IntAvgStats(Option<StatsManager>.None, true);
        this.m_consumedStats[key] = intAvgStats2;
        statsManager.RegisterForYearlyUpdatesOnly((ItemStats) intAvgStats2);
      }
    }

    public void AddReduction(HealthPointsCategoryProto category, Percent reduction, Percent max)
    {
      Assert.That<Percent>(reduction).IsNotNegative();
      Assert.That<Percent>(max).IsNotNegative();
      Assert.That<Percent>(reduction).IsLessOrEqual(max);
      this.HealthThisMonth -= reduction;
      this.m_thisMonthRecords.Add(new HealthStatistics.Entry(category, -reduction, -max));
      if (!reduction.IsPositive)
        return;
      IntAvgStats intAvgStats;
      if (this.m_consumedStats.TryGetValue(category, out intAvgStats))
        intAvgStats.AddToCurrent((long) reduction.ToIntPercentRounded());
      this.ConsumedTotalStats.AddToCurrent((long) reduction.ToIntPercentRounded());
    }

    public void AddIncrease(HealthPointsCategoryProto category, Percent increase, Percent max)
    {
      Assert.That<Percent>(increase).IsNotNegative();
      this.HealthThisMonth += increase;
      this.m_thisMonthRecords.Add(new HealthStatistics.Entry(category, increase, max));
      if (!increase.IsPositive)
        return;
      IntAvgStats intAvgStats;
      if (this.m_generatedStats.TryGetValue(category, out intAvgStats))
        intAvgStats.AddToCurrent((long) increase.ToIntPercentRounded());
      this.GeneratedTotalStats.AddToCurrent((long) increase.ToIntPercentRounded());
    }

    internal void OnNewMonthEnd()
    {
      Swap.Them<Lyst<HealthStatistics.Entry>>(ref this.m_lastMonthRecords, ref this.m_thisMonthRecords);
      this.HealthLastMonth = this.HealthThisMonth;
      this.HealthThisMonth = Percent.Zero;
      this.m_thisMonthRecords.Clear();
      ((IItemStatsEvents) this.GeneratedTotalStats).OnNewMonth();
      ((IItemStatsEvents) this.ConsumedTotalStats).OnNewMonth();
      foreach (IItemStatsEvents itemStatsEvents in this.m_generatedStats.Values)
        itemStatsEvents.OnNewMonth();
      foreach (IItemStatsEvents itemStatsEvents in this.m_consumedStats.Values)
        itemStatsEvents.OnNewMonth();
    }

    public static void Serialize(HealthStatistics value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<HealthStatistics>(value))
        return;
      writer.EnqueueDataSerialization((object) value, HealthStatistics.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      IntAvgStats.Serialize(this.ConsumedTotalStats, writer);
      IntAvgStats.Serialize(this.GeneratedTotalStats, writer);
      Percent.Serialize(this.HealthLastMonth, writer);
      Percent.Serialize(this.HealthThisMonth, writer);
      Dict<HealthPointsCategoryProto, IntAvgStats>.Serialize(this.m_consumedStats, writer);
      Dict<HealthPointsCategoryProto, IntAvgStats>.Serialize(this.m_generatedStats, writer);
      Lyst<HealthStatistics.Entry>.Serialize(this.m_lastMonthRecords, writer);
      Lyst<HealthStatistics.Entry>.Serialize(this.m_thisMonthRecords, writer);
    }

    public static HealthStatistics Deserialize(BlobReader reader)
    {
      HealthStatistics healthStatistics;
      if (reader.TryStartClassDeserialization<HealthStatistics>(out healthStatistics))
        reader.EnqueueDataDeserialization((object) healthStatistics, HealthStatistics.s_deserializeDataDelayedAction);
      return healthStatistics;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<HealthStatistics>(this, "ConsumedTotalStats", (object) IntAvgStats.Deserialize(reader));
      reader.SetField<HealthStatistics>(this, "GeneratedTotalStats", (object) IntAvgStats.Deserialize(reader));
      this.HealthLastMonth = Percent.Deserialize(reader);
      this.HealthThisMonth = Percent.Deserialize(reader);
      reader.SetField<HealthStatistics>(this, "m_consumedStats", (object) Dict<HealthPointsCategoryProto, IntAvgStats>.Deserialize(reader));
      reader.SetField<HealthStatistics>(this, "m_generatedStats", (object) Dict<HealthPointsCategoryProto, IntAvgStats>.Deserialize(reader));
      this.m_lastMonthRecords = Lyst<HealthStatistics.Entry>.Deserialize(reader);
      this.m_thisMonthRecords = Lyst<HealthStatistics.Entry>.Deserialize(reader);
    }

    static HealthStatistics()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      HealthStatistics.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((HealthStatistics) obj).SerializeData(writer));
      HealthStatistics.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((HealthStatistics) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct Entry
    {
      public readonly Percent Change;
      public readonly Percent Max;
      public readonly HealthPointsCategoryProto Category;

      public Entry(HealthPointsCategoryProto category, Percent change, Percent max)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Change = change;
        this.Category = category;
        this.Max = max;
      }

      public static void Serialize(HealthStatistics.Entry value, BlobWriter writer)
      {
        writer.WriteGeneric<HealthPointsCategoryProto>(value.Category);
        Percent.Serialize(value.Change, writer);
        Percent.Serialize(value.Max, writer);
      }

      public static HealthStatistics.Entry Deserialize(BlobReader reader)
      {
        return new HealthStatistics.Entry(reader.ReadGenericAs<HealthPointsCategoryProto>(), Percent.Deserialize(reader), Percent.Deserialize(reader));
      }
    }
  }
}
