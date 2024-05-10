﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.Fix32SumStats
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
  public sealed class Fix32SumStats : ItemSumStats<Fix32>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(Fix32SumStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Fix32SumStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Fix32SumStats.s_serializeDataDelayedAction);
    }

    public static Fix32SumStats Deserialize(BlobReader reader)
    {
      Fix32SumStats fix32SumStats;
      if (reader.TryStartClassDeserialization<Fix32SumStats>(out fix32SumStats))
        reader.EnqueueDataDeserialization((object) fix32SumStats, Fix32SumStats.s_deserializeDataDelayedAction);
      return fix32SumStats;
    }

    public Fix32SumStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
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

    static Fix32SumStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Fix32SumStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      Fix32SumStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
