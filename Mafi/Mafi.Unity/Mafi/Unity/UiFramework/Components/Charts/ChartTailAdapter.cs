// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.ChartTailAdapter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  /// <summary>
  /// Takes last N elements from chart's underlying data + provides caching.
  /// </summary>
  public class ChartTailAdapter : ILabelsProvider
  {
    private readonly ICalendar m_calendar;
    private readonly Lyst<long> m_cachedData;
    private StatsDataRange m_currentDataRange;
    private ItemStats m_stats;

    public IIndexable<long> ChartData => (IIndexable<long>) this.m_cachedData;

    /// <summary>
    /// Horizontal labels are always from 1 and up, so we can just cache them.
    /// </summary>
    public ILabelsProvider HorizontalLabels => (ILabelsProvider) this;

    public ChartTailAdapter(ICalendar calendar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_cachedData = new Lyst<long>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar;
    }

    public void RefreshData(ItemStats stats, StatsDataRange range)
    {
      this.m_stats = stats;
      this.m_cachedData.Clear();
      this.m_currentDataRange = range;
      if (!stats.HasAnyNonZeroData)
        return;
      stats.GetLatestData(range, this.m_cachedData);
    }

    public LocStrFormatted GetLabelAt(int i)
    {
      bool flag;
      switch (this.m_currentDataRange)
      {
        case StatsDataRange.Last120Days:
        case StatsDataRange.Last120Months:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
        return new LocStrFormatted((-i - 1).ToStringCached());
      if (this.m_currentDataRange == StatsDataRange.Last100Years)
        return new LocStrFormatted((this.m_calendar.CurrentDate.Year - i - 1).ToStringCached());
      return i == this.m_cachedData.Count - 1 ? new LocStrFormatted((this.m_stats.YearCollectionStarted - 1).ToStringCached()) : new LocStrFormatted((this.m_calendar.CurrentDate.Year - (this.m_calendar.CurrentDate.Year - 1) % 25 - 1 - i * 25).ToStringCached());
    }
  }
}
