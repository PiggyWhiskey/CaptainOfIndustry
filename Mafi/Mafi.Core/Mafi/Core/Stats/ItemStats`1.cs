// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.ItemStats`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Core.Stats
{
  public abstract class ItemStats<T> : ItemStats
  {
    /// <summary>Total for the last (finished) day.</summary>
    public T LastDay => this.RawToValue(this.LastDayValue);

    /// <summary>Total for the last (finished) month.</summary>
    public T LastMonth => this.RawToValue(this.LastMonthValue);

    /// <summary>Total for the current ongoing month (updated daily).</summary>
    public T ThisYear => this.RawToValue(this.ThisYearValue);

    /// <summary>Total for the last (finished) year.</summary>
    public T LastYear => this.RawToValue(this.LastYearValue);

    public T Lifetime => this.RawToValue(this.LifetimeValue);

    protected ItemStats(Option<StatsManager> manager, bool isMonthlyEvent)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(isMonthlyEvent, manager);
    }

    public abstract long ValueToRaw(T value);

    public abstract T RawToValue(long value);

    public void GetLastNDays(int maxCount, Lyst<T> values)
    {
      foreach (long num in this.LastNDaysData)
      {
        --maxCount;
        if (maxCount < 0)
          break;
        values.Add(this.RawToValue(num));
      }
    }

    public void GetLastNMonths(int maxCount, Lyst<T> values)
    {
      foreach (long num in this.LastNMonthsData)
      {
        --maxCount;
        if (maxCount < 0)
          break;
        values.Add(this.RawToValue(num));
      }
    }

    public void GetLastNYears(int maxCount, Lyst<T> values)
    {
      foreach (long num in this.LastNYearsData)
      {
        --maxCount;
        if (maxCount < 0)
          break;
        values.Add(this.RawToValue(num));
      }
    }
  }
}
