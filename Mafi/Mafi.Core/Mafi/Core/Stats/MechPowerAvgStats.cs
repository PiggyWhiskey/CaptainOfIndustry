// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.MechPowerAvgStats
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
  public sealed class MechPowerAvgStats : ItemAvgStats<MechPower>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(MechPowerAvgStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MechPowerAvgStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MechPowerAvgStats.s_serializeDataDelayedAction);
    }

    public static MechPowerAvgStats Deserialize(BlobReader reader)
    {
      MechPowerAvgStats mechPowerAvgStats;
      if (reader.TryStartClassDeserialization<MechPowerAvgStats>(out mechPowerAvgStats))
        reader.EnqueueDataDeserialization((object) mechPowerAvgStats, MechPowerAvgStats.s_deserializeDataDelayedAction);
      return mechPowerAvgStats;
    }

    public MechPowerAvgStats(Option<StatsManager> manager, bool isMonthlyEvent = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(manager, isMonthlyEvent);
    }

    public override long ValueToRaw(MechPower value) => (long) value.Value;

    public override MechPower RawToValue(long value) => new MechPower((int) value);

    public override LocStrFormatted FormatValue(long value)
    {
      return ElectricityQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge(value));
    }

    static MechPowerAvgStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MechPowerAvgStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ItemStats) obj).SerializeData(writer));
      MechPowerAvgStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ItemStats) obj).DeserializeData(reader));
    }
  }
}
