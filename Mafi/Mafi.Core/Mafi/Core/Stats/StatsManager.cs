// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.StatsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Stats
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class StatsManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ProtosDb ProtosDb;
    private readonly Lyst<IItemStatsEvents> m_dailyUpdatedStats;
    private readonly Lyst<IItemStatsEvents> m_monthlyUpdatedStats;
    private readonly Lyst<IItemStatsEvents> m_yearlyUpdatedStats;
    [NewInSaveVersion(131, null, null, typeof (ICalendar), null)]
    private readonly ICalendar m_calendar;

    public static void Serialize(StatsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StatsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StatsManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      Lyst<IItemStatsEvents>.Serialize(this.m_dailyUpdatedStats, writer);
      Lyst<IItemStatsEvents>.Serialize(this.m_monthlyUpdatedStats, writer);
      Lyst<IItemStatsEvents>.Serialize(this.m_yearlyUpdatedStats, writer);
    }

    public static StatsManager Deserialize(BlobReader reader)
    {
      StatsManager statsManager;
      if (reader.TryStartClassDeserialization<StatsManager>(out statsManager))
        reader.EnqueueDataDeserialization((object) statsManager, StatsManager.s_deserializeDataDelayedAction);
      return statsManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<StatsManager>(this, "m_calendar", reader.LoadedSaveVersion >= 131 ? (object) reader.ReadGenericAs<ICalendar>() : (object) (ICalendar) null);
      if (reader.LoadedSaveVersion < 131)
        reader.RegisterResolvedMember<StatsManager>(this, "m_calendar", typeof (ICalendar), true);
      reader.SetField<StatsManager>(this, "m_dailyUpdatedStats", (object) Lyst<IItemStatsEvents>.Deserialize(reader));
      reader.SetField<StatsManager>(this, "m_monthlyUpdatedStats", (object) Lyst<IItemStatsEvents>.Deserialize(reader));
      reader.SetField<StatsManager>(this, "m_yearlyUpdatedStats", (object) Lyst<IItemStatsEvents>.Deserialize(reader));
      reader.RegisterResolvedMember<StatsManager>(this, "ProtosDb", typeof (ProtosDb), true);
    }

    internal IIndexable<IItemStatsEvents> RegisteredDailyStats
    {
      get => (IIndexable<IItemStatsEvents>) this.m_dailyUpdatedStats;
    }

    internal IIndexable<IItemStatsEvents> RegisteredMonthlyStats
    {
      get => (IIndexable<IItemStatsEvents>) this.m_monthlyUpdatedStats;
    }

    internal IIndexable<IItemStatsEvents> RegisteredYearlyStats
    {
      get => (IIndexable<IItemStatsEvents>) this.m_yearlyUpdatedStats;
    }

    public StatsManager(ICalendar calendar, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_dailyUpdatedStats = new Lyst<IItemStatsEvents>();
      this.m_monthlyUpdatedStats = new Lyst<IItemStatsEvents>();
      this.m_yearlyUpdatedStats = new Lyst<IItemStatsEvents>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtosDb = protosDb;
      this.m_calendar = calendar;
      calendar.NewDayEnd.Add<StatsManager>(this, new Action(this.onNewDay));
      calendar.NewMonthEnd.Add<StatsManager>(this, new Action(this.onNewMonth));
      calendar.NewYearEnd.Add<StatsManager>(this, new Action(this.onNewYear));
    }

    public void RegisterStats(ItemStats itemStats)
    {
      if (!itemStats.IsMonthlyEvent)
        this.m_dailyUpdatedStats.Add((IItemStatsEvents) itemStats);
      this.m_monthlyUpdatedStats.Add((IItemStatsEvents) itemStats);
      this.m_yearlyUpdatedStats.Add((IItemStatsEvents) itemStats);
    }

    /// <summary>
    /// Handy for cases where daily/monthly events are handled by some other class but yearly should be just
    /// updated on NewYearEnd calendar event.
    /// </summary>
    public void RegisterForYearlyUpdatesOnly(ItemStats itemStats)
    {
      this.m_yearlyUpdatedStats.Add((IItemStatsEvents) itemStats);
    }

    private void onNewDay()
    {
      foreach (IItemStatsEvents dailyUpdatedStat in this.m_dailyUpdatedStats)
        dailyUpdatedStat.OnNewDay();
    }

    private void onNewMonth()
    {
      foreach (IItemStatsEvents monthlyUpdatedStat in this.m_monthlyUpdatedStats)
        monthlyUpdatedStat.OnNewMonth();
    }

    private void onNewYear()
    {
      foreach (IItemStatsEvents yearlyUpdatedStat in this.m_yearlyUpdatedStats)
        yearlyUpdatedStat.OnNewYear(this.m_calendar.CurrentDate.Year);
    }

    static StatsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StatsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StatsManager) obj).SerializeData(writer));
      StatsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StatsManager) obj).DeserializeData(reader));
    }
  }
}
