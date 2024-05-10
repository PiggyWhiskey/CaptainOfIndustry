// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.ItemMaxStats`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Stats
{
  public abstract class ItemMaxStats<T> : ItemStats<T>
  {
    protected ItemMaxStats(Option<StatsManager> manager, bool isMonthlyEvent)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public void Add(T value)
    {
      long raw = this.ValueToRaw(value);
      this.UpdateCurrentValue(this.CurrentValue.Max(raw), this.ThisYearValue.Max(raw), this.LifetimeValue.Max(raw));
    }

    protected override long AggregateYears(int yearsToAggregate)
    {
      return this.LastNYearsData.GetMaxOfLastNValues(yearsToAggregate);
    }

    protected override long AggregateDaysToLastMonth()
    {
      return this.LastNDaysData.GetMaxOfLastNValues(30);
    }

    protected override long AggregateMonthsToLastYear()
    {
      return this.LastNMonthsData.GetMaxOfLastNValues(12);
    }
  }
}
