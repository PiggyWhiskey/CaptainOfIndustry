// Decompiled with JetBrains decompiler
// Type: Mafi.DataStats
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  public class DataStats
  {
    public readonly float Min;
    public readonly float Max;
    public readonly float Sum;
    public readonly float Mean;
    public readonly float Median;
    public readonly float Count;
    public readonly float StdDev;

    public DataStats(
      float min,
      float max,
      float sum,
      float mean,
      float median,
      float count,
      float stdDev)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Min = min;
      this.Max = max;
      this.Sum = sum;
      this.Mean = mean;
      this.Median = median;
      this.Count = count;
      this.StdDev = stdDev;
    }

    public string ToString(string delim)
    {
      return string.Format("Min: {0}{1}Max: {2}{3}Sum: {4}{5}Mean: {6}{7}", (object) this.Min, (object) delim, (object) this.Max, (object) delim, (object) this.Sum, (object) delim, (object) this.Mean, (object) delim) + string.Format("Median: {0}{1}Count: {2}{3}StdDev: {4}", (object) this.Median, (object) delim, (object) this.Count, (object) delim, (object) this.StdDev);
    }

    public override string ToString() => this.ToString(", ");
  }
}
