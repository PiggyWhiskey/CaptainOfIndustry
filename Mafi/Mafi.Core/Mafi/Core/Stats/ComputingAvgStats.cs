// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.ComputingAvgStats
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
  public sealed class ComputingAvgStats : ItemAvgStats<Computing>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(ComputingAvgStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ComputingAvgStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ComputingAvgStats.s_serializeDataDelayedAction);
    }

    public static ComputingAvgStats Deserialize(BlobReader reader)
    {
      ComputingAvgStats computingAvgStats;
      if (reader.TryStartClassDeserialization<ComputingAvgStats>(out computingAvgStats))
        reader.EnqueueDataDeserialization((object) computingAvgStats, ComputingAvgStats.s_deserializeDataDelayedAction);
      return computingAvgStats;
    }

    public ComputingAvgStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public override long ValueToRaw(Computing value) => (long) value.Value;

    public override Computing RawToValue(long value) => new Computing((int) value);

    public override LocStrFormatted FormatValue(long value)
    {
      return ComputingQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge(value));
    }

    static ComputingAvgStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ComputingAvgStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      ComputingAvgStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
