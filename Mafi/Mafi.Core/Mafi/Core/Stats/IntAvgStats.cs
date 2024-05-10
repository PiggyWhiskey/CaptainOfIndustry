// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.IntAvgStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Localization.Quantity;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Stats
{
  [GenerateSerializer(false, null, 0)]
  public sealed class IntAvgStats : ItemAvgStats<long>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(IntAvgStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IntAvgStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IntAvgStats.s_serializeDataDelayedAction);
    }

    public static IntAvgStats Deserialize(BlobReader reader)
    {
      IntAvgStats intAvgStats;
      if (reader.TryStartClassDeserialization<IntAvgStats>(out intAvgStats))
        reader.EnqueueDataDeserialization((object) intAvgStats, IntAvgStats.s_deserializeDataDelayedAction);
      return intAvgStats;
    }

    public IntAvgStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public override long ValueToRaw(long value) => value;

    public override long RawToValue(long value) => value;

    public override LocStrFormatted FormatValue(long value)
    {
      return IntegerSiSuffixFormatter.FormatNumber(value);
    }

    static IntAvgStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IntAvgStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      IntAvgStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
