// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.QuantitySumStats
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
  public sealed class QuantitySumStats : ItemSumStats<QuantityLarge>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(QuantitySumStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<QuantitySumStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, QuantitySumStats.s_serializeDataDelayedAction);
    }

    public static QuantitySumStats Deserialize(BlobReader reader)
    {
      QuantitySumStats quantitySumStats;
      if (reader.TryStartClassDeserialization<QuantitySumStats>(out quantitySumStats))
        reader.EnqueueDataDeserialization((object) quantitySumStats, QuantitySumStats.s_deserializeDataDelayedAction);
      return quantitySumStats;
    }

    public QuantitySumStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public override long ValueToRaw(QuantityLarge value) => value.Value;

    public override QuantityLarge RawToValue(long value) => new QuantityLarge(value);

    public override LocStrFormatted FormatValue(long value)
    {
      return IntegerSiSuffixFormatter.FormatNumber(value);
    }

    static QuantitySumStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      QuantitySumStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      QuantitySumStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
