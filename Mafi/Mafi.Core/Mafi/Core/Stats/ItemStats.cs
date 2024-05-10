// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.ItemStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Threading;

#nullable disable
namespace Mafi.Core.Stats
{
  [GenerateSerializer(true, null, 0)]
  public abstract class ItemStats : IItemStatsEvents, IDataValuesFormatter
  {
    public const int QUARTER_CENTURY = 25;
    public const int MAX_YEARS_OF_ANNUAL_DATA = 100;
    public const int MAX_MONTHS_OF_MONTHLY_DATA = 120;
    public const int MAX_DAYS_OF_DAILY_DATA = 120;
    [OnlyForSaveCompatibility(null)]
    private static readonly ThreadLocal<Lyst<long>> LOAD_CACHE;
    private Option<StatsManager> m_manager;
    protected RleSequence QuarterCenturiesData;
    protected RleSequence LastNYearsData;
    protected RleSequence LastNMonthsData;
    protected RleSequence LastNDaysData;

    public bool HasAnyData => this.m_manager.IsNone;

    public bool HasAnyNonZeroData
    {
      get
      {
        return this.CurrentValue != 0L || this.LastNYearsData.HasAnyNonZeroData || this.LastNMonthsData.HasAnyNonZeroData || this.LastNDaysData.HasAnyNonZeroData || this.QuarterCenturiesData.HasAnyNonZeroData;
      }
    }

    /// <summary>
    /// Whether this event is daily on monthly. For daily events,
    /// </summary>
    public bool IsMonthlyEvent { get; private set; }

    public bool AreAnnualDataFull => this.LastNYearsData.Count == 100;

    protected long CurrentValue { get; private set; }

    protected long ThisYearValue { get; private set; }

    protected long LifetimeValue { get; private set; }

    protected long LastDayValue => this.LastNDaysData.NewestValueOrDefault;

    protected long LastMonthValue => this.LastNMonthsData.NewestValueOrDefault;

    protected long LastYearValue => this.LastNYearsData.NewestValueOrDefault;

    internal int YearCollectionStarted { get; private set; }

    protected long FirstYearValue { get; private set; }

    internal int StoredValuesCount
    {
      get
      {
        return this.LastNYearsData.CompressedCount + this.LastNMonthsData.CompressedCount + this.LastNDaysData.CompressedCount;
      }
    }

    /// <param name="isMonthlyEvent">If true, this event will be updated only monthly, daily data will be empty.</param>
    /// <param name="manager">If set, event will be updated automatically, otherwise, owner is responsible
    /// for calling methods from <see cref="T:Mafi.Core.Stats.IItemStatsEvents" />.</param>
    protected ItemStats(bool isMonthlyEvent, Option<StatsManager> manager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IsMonthlyEvent = isMonthlyEvent;
      this.m_manager = manager;
    }

    protected void UpdateCurrentValue(long newValue, long newThisYearValue, long newLifetimeValue)
    {
      this.CurrentValue = newValue;
      this.ThisYearValue = newThisYearValue;
      this.LifetimeValue = newLifetimeValue;
      if (!this.m_manager.HasValue)
        return;
      this.m_manager.Value.RegisterStats(this);
      this.m_manager = Option<StatsManager>.None;
    }

    public abstract LocStrFormatted FormatValue(long value);

    public virtual void GetRangeAndTicksMax(
      long minValue,
      long maxValue,
      int recommendedCount,
      out long rangeMax,
      out long tickSize)
    {
      ItemStats.GetRangeAndTicksMaxBase(minValue, maxValue, recommendedCount, out rangeMax, out tickSize);
    }

    protected static void GetRangeAndTicksMaxBase(
      long minValue,
      long maxValue,
      int recommendedCount,
      out long rangeMax,
      out long tickSize)
    {
      Assert.That<long>(minValue).IsZero("TODO: Make this work non-zero min and for negative cases when needed.");
      long num1 = maxValue;
      if (recommendedCount <= 2)
      {
        rangeMax = maxValue;
        tickSize = num1;
        Assert.That<long>(rangeMax).IsGreaterOrEqual(maxValue);
      }
      else if (num1 < (long) recommendedCount)
      {
        rangeMax = (long) recommendedCount;
        tickSize = 1L;
        Assert.That<long>(rangeMax).IsGreaterOrEqual(maxValue);
      }
      else
      {
        long d = num1 / (long) (recommendedCount - 1);
        if (d <= 1L)
        {
          rangeMax = maxValue;
          tickSize = 1L;
          Assert.That<long>(rangeMax).IsGreaterOrEqual(maxValue);
        }
        else
        {
          long num2 = (long) Math.Round(Math.Pow(10.0, Math.Floor(Math.Log10((double) d))));
          int num3 = recommendedCount + recommendedCount / 2;
          if (num1 / num2 < (long) num3)
          {
            tickSize = num2;
          }
          else
          {
            long num4 = num2 * 2L;
            tickSize = num1 / num4 >= (long) num3 ? num2 * 5L : num4;
          }
          long num5 = (num1 + tickSize - 1L) / tickSize;
          rangeMax = tickSize * num5;
          Assert.That<long>(rangeMax).IsGreaterOrEqual(maxValue);
        }
      }
    }

    protected static void GetRangeAndTicksMaxForFix32(
      long minValue,
      long maxValue,
      int recommendedCount,
      out long rangeMax,
      out long tickSize)
    {
      Fix32 fix32_1 = Fix32.FromRaw((int) minValue);
      Fix32 fix32_2 = Fix32.FromRaw((int) maxValue);
      Assert.That<long>(minValue).IsZero("TODO: Make this work non-zero min and for negative cases when needed.");
      if (fix32_2 <= 10)
      {
        ItemStats.GetRangeAndTicksMaxBase((long) (fix32_1 * 100).IntegerPart, (long) (fix32_2 * 100).IntegerPart, recommendedCount, out rangeMax, out tickSize);
        tickSize = (long) (Fix32.FromInt((int) tickSize) / 100).RawValue;
        rangeMax = (long) (Fix32.FromInt((int) rangeMax) / 100).RawValue;
        rangeMax = tickSize * (long) Math.Round((double) rangeMax / (double) tickSize);
      }
      else
      {
        ItemStats.GetRangeAndTicksMaxBase((long) fix32_1.IntegerPart, (long) fix32_2.IntegerPart, recommendedCount, out rangeMax, out tickSize);
        tickSize = (long) Fix32.FromInt((int) tickSize).RawValue;
        rangeMax = (long) Fix32.FromInt((int) rangeMax).RawValue;
      }
    }

    protected abstract long AggregateDaysToLastMonth();

    protected abstract long AggregateMonthsToLastYear();

    protected abstract long AggregateYears(int yearsToAggregate);

    public void GetLatestData(StatsDataRange range, Lyst<long> data)
    {
      switch (range)
      {
        case StatsDataRange.Last120Days:
          foreach (long num in this.LastNDaysData)
            data.Add(num);
          break;
        case StatsDataRange.Last120Months:
          foreach (long num in this.LastNMonthsData)
            data.Add(num);
          break;
        case StatsDataRange.Last100Years:
          foreach (long num in this.LastNYearsData)
          {
            data.Add(num);
            if (data.Count >= 100)
              break;
          }
          break;
        default:
          foreach (long num in this.QuarterCenturiesData)
            data.Add(num);
          if ((this.YearCollectionStarted - 1) % 25 == 0)
            break;
          data.Add(this.FirstYearValue);
          break;
      }
    }

    void IItemStatsEvents.OnNewDay()
    {
      if (this.LastNDaysData.Count >= 120)
        this.LastNDaysData.RemoveOldest();
      this.LastNDaysData.AddValue(this.CurrentValue);
      this.CurrentValue = 0L;
    }

    void IItemStatsEvents.OnNewMonth()
    {
      if (this.LastNMonthsData.Count >= 120)
        this.LastNMonthsData.RemoveOldest();
      if (this.IsMonthlyEvent)
      {
        this.LastNMonthsData.AddValue(this.CurrentValue);
        this.CurrentValue = 0L;
      }
      else
        this.LastNMonthsData.AddValue(this.AggregateDaysToLastMonth());
    }

    void IItemStatsEvents.OnNewYear(int year)
    {
      long lastYear = this.AggregateMonthsToLastYear();
      this.addNewYear(year, lastYear);
      this.ThisYearValue = 0L;
    }

    private void addNewYear(int year, long value)
    {
      if (this.LastNYearsData.Count >= 100)
        this.LastNYearsData.RemoveOldest();
      this.LastNYearsData.AddValue(value);
      if (this.LastNYearsData.Count == 1)
      {
        this.YearCollectionStarted = year;
        this.FirstYearValue = value;
      }
      if ((year - 1) % 25 != 0)
        return;
      this.QuarterCenturiesData.AddValue(this.AggregateYears(25));
    }

    [OnlyForSaveCompatibility(null)]
    private void migrateToQuarterCentury(DependencyResolver resolver)
    {
      ICalendar calendar = resolver.Resolve<ICalendar>();
      Lyst<long> lyst = ItemStats.LOAD_CACHE.Value;
      lyst.Clear();
      foreach (long num in this.LastNYearsData)
        lyst.Add(num);
      lyst.Reverse();
      int year = calendar.CurrentDate.Year - this.LastNYearsData.Count + 1;
      this.LastNYearsData = new RleSequence();
      foreach (long num in lyst)
      {
        this.addNewYear(year, num);
        ++year;
      }
      lyst.Clear();
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsMonthlyEvent);
      writer.WriteBool(this.m_manager.HasValue);
      if (this.m_manager.HasValue)
      {
        StatsManager.Serialize(this.m_manager.Value, writer);
      }
      else
      {
        if (!this.IsMonthlyEvent)
          RleSequence.Serialize(this.LastNDaysData, writer);
        RleSequence.Serialize(this.LastNMonthsData, writer);
        RleSequence.Serialize(this.LastNYearsData, writer);
        RleSequence.Serialize(this.QuarterCenturiesData, writer);
        writer.WriteLong(this.CurrentValue);
        writer.WriteLong(this.ThisYearValue);
        writer.WriteLong(this.LifetimeValue);
        writer.WriteInt(this.YearCollectionStarted);
        writer.WriteLong(this.FirstYearValue);
      }
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsMonthlyEvent = reader.ReadBool();
      if (reader.ReadBool())
      {
        this.m_manager = (Option<StatsManager>) StatsManager.Deserialize(reader);
      }
      else
      {
        if (!this.IsMonthlyEvent)
          this.LastNDaysData = RleSequence.Deserialize(reader);
        this.LastNMonthsData = RleSequence.Deserialize(reader);
        this.LastNYearsData = RleSequence.Deserialize(reader);
        if (reader.LoadedSaveVersion >= 131)
          this.QuarterCenturiesData = RleSequence.Deserialize(reader);
        this.CurrentValue = reader.ReadLong();
        this.ThisYearValue = reader.ReadLong();
        this.LifetimeValue = reader.ReadLong();
        if (reader.LoadedSaveVersion >= 131)
        {
          this.YearCollectionStarted = reader.ReadInt();
          this.FirstYearValue = reader.ReadLong();
        }
        if (reader.LoadedSaveVersion >= 131)
          return;
        reader.RegisterInitAfterLoad<ItemStats>(this, "migrateToQuarterCentury", InitPriority.High);
      }
    }

    static ItemStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ItemStats.LOAD_CACHE = new ThreadLocal<Lyst<long>>((Func<Lyst<long>>) (() => new Lyst<long>()));
    }
  }
}
