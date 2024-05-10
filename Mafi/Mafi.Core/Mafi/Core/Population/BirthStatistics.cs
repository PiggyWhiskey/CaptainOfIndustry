// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.BirthStatistics
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population
{
  [GenerateSerializer(false, null, 0)]
  public class BirthStatistics
  {
    public Percent BirthRateThisMonth;
    public Percent BirthRateThisMonthAlreadyApplied;
    public Percent BirthRateLastMonth;
    private Lyst<BirthStatistics.Entry> m_lastMonthRecords;
    private Lyst<BirthStatistics.Entry> m_thisMonthRecords;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IIndexable<BirthStatistics.Entry> LastMonthRecords
    {
      get => (IIndexable<BirthStatistics.Entry>) this.m_lastMonthRecords;
    }

    public void AddReduction(
      BirthRateCategoryProto category,
      Percent reduction,
      Percent max,
      bool wasAlreadyApplied)
    {
      Assert.That<Percent>(reduction).IsNotNegative();
      Assert.That<Percent>(max).IsNotNegative();
      Assert.That<Percent>(reduction).IsLessOrEqual(max);
      this.BirthRateThisMonth -= reduction;
      if (wasAlreadyApplied)
        this.BirthRateThisMonthAlreadyApplied -= reduction;
      this.m_thisMonthRecords.Add(new BirthStatistics.Entry(category, -reduction, -max));
    }

    public void AddIncrease(BirthRateCategoryProto category, Percent increase, Percent max)
    {
      Assert.That<Percent>(increase).IsNotNegative();
      this.BirthRateThisMonth += increase;
      this.m_thisMonthRecords.Add(new BirthStatistics.Entry(category, increase, max));
    }

    internal void OnNewMonthEnd()
    {
      Swap.Them<Lyst<BirthStatistics.Entry>>(ref this.m_lastMonthRecords, ref this.m_thisMonthRecords);
      this.BirthRateLastMonth = this.BirthRateThisMonth;
      this.BirthRateThisMonth = Percent.Zero;
      this.BirthRateThisMonthAlreadyApplied = Percent.Zero;
      this.m_thisMonthRecords.Clear();
    }

    public static void Serialize(BirthStatistics value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BirthStatistics>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BirthStatistics.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.BirthRateLastMonth, writer);
      Percent.Serialize(this.BirthRateThisMonth, writer);
      Percent.Serialize(this.BirthRateThisMonthAlreadyApplied, writer);
      Lyst<BirthStatistics.Entry>.Serialize(this.m_lastMonthRecords, writer);
      Lyst<BirthStatistics.Entry>.Serialize(this.m_thisMonthRecords, writer);
    }

    public static BirthStatistics Deserialize(BlobReader reader)
    {
      BirthStatistics birthStatistics;
      if (reader.TryStartClassDeserialization<BirthStatistics>(out birthStatistics))
        reader.EnqueueDataDeserialization((object) birthStatistics, BirthStatistics.s_deserializeDataDelayedAction);
      return birthStatistics;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BirthRateLastMonth = Percent.Deserialize(reader);
      this.BirthRateThisMonth = Percent.Deserialize(reader);
      this.BirthRateThisMonthAlreadyApplied = Percent.Deserialize(reader);
      this.m_lastMonthRecords = Lyst<BirthStatistics.Entry>.Deserialize(reader);
      this.m_thisMonthRecords = Lyst<BirthStatistics.Entry>.Deserialize(reader);
    }

    public BirthStatistics()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastMonthRecords = new Lyst<BirthStatistics.Entry>();
      this.m_thisMonthRecords = new Lyst<BirthStatistics.Entry>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BirthStatistics()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BirthStatistics.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BirthStatistics) obj).SerializeData(writer));
      BirthStatistics.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BirthStatistics) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct Entry
    {
      public readonly Percent Change;
      public readonly Percent Max;
      public readonly BirthRateCategoryProto Category;

      public Entry(BirthRateCategoryProto category, Percent change, Percent max)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Change = change;
        this.Category = category;
        this.Max = max;
      }

      public static void Serialize(BirthStatistics.Entry value, BlobWriter writer)
      {
        writer.WriteGeneric<BirthRateCategoryProto>(value.Category);
        Percent.Serialize(value.Change, writer);
        Percent.Serialize(value.Max, writer);
      }

      public static BirthStatistics.Entry Deserialize(BlobReader reader)
      {
        return new BirthStatistics.Entry(reader.ReadGenericAs<BirthRateCategoryProto>(), Percent.Deserialize(reader), Percent.Deserialize(reader));
      }
    }
  }
}
