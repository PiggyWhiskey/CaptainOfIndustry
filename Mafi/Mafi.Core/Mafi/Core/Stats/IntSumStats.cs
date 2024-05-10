// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.IntSumStats
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
  public sealed class IntSumStats : ItemSumStats<long>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(IntSumStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IntSumStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IntSumStats.s_serializeDataDelayedAction);
    }

    public static IntSumStats Deserialize(BlobReader reader)
    {
      IntSumStats intSumStats;
      if (reader.TryStartClassDeserialization<IntSumStats>(out intSumStats))
        reader.EnqueueDataDeserialization((object) intSumStats, IntSumStats.s_deserializeDataDelayedAction);
      return intSumStats;
    }

    public IntSumStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
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

    static IntSumStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IntSumStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      IntSumStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
