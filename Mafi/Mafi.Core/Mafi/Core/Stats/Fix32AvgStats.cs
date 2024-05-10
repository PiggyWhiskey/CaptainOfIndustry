// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.Fix32AvgStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Stats
{
  [GenerateSerializer(false, null, 0)]
  public sealed class Fix32AvgStats : ItemAvgStats<Fix32>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(Fix32AvgStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Fix32AvgStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Fix32AvgStats.s_serializeDataDelayedAction);
    }

    public static Fix32AvgStats Deserialize(BlobReader reader)
    {
      Fix32AvgStats fix32AvgStats;
      if (reader.TryStartClassDeserialization<Fix32AvgStats>(out fix32AvgStats))
        reader.EnqueueDataDeserialization((object) fix32AvgStats, Fix32AvgStats.s_deserializeDataDelayedAction);
      return fix32AvgStats;
    }

    public Fix32AvgStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public override long ValueToRaw(Fix32 value) => (long) value.RawValue;

    public override Fix32 RawToValue(long value) => Fix32.FromRaw((int) value);

    public override LocStrFormatted FormatValue(long value)
    {
      return new LocStrFormatted(Fix32.FromRaw((int) value).ToStringRoundedAdaptive());
    }

    public override void GetRangeAndTicksMax(
      long minValue,
      long maxValue,
      int recommendedCount,
      out long rangeMax,
      out long tickSize)
    {
      ItemStats.GetRangeAndTicksMaxForFix32(minValue, maxValue, recommendedCount, out rangeMax, out tickSize);
    }

    static Fix32AvgStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Fix32AvgStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      Fix32AvgStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
