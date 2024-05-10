// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [MemberRemovedInSaveVersion("CanQuickRepair", 140, typeof (bool), 0, false)]
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public sealed class MaintenanceConfig : IConfig, IMaintenanceConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Duration BufferMaxCapacity { get; set; }

    public Percent ReliabilityIssuesStartAt { get; set; }

    public Percent MaxBreakdownChance { get; set; }

    public Percent MaxReplenishSpeed { get; set; }

    public Percent IdleMaintenanceMultiplier { get; set; }

    public PartialQuantity BaseReplenishPerMonth { get; set; }

    public Duration BrokenDurationMin { get; set; }

    public Duration BrokenDurationMax { get; set; }

    public Percent DailyBreakdownChanceWhenShouldBeBroken { get; set; }

    public MaintenanceConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBufferMaxCapacity\u003Ek__BackingField = 24.Months();
      // ISSUE: reference to a compiler-generated field
      this.\u003CReliabilityIssuesStartAt\u003Ek__BackingField = 75.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxBreakdownChance\u003Ek__BackingField = 50.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxReplenishSpeed\u003Ek__BackingField = 400.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIdleMaintenanceMultiplier\u003Ek__BackingField = 33.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBaseReplenishPerMonth\u003Ek__BackingField = 2.Quantity().AsPartial;
      // ISSUE: reference to a compiler-generated field
      this.\u003CBrokenDurationMin\u003Ek__BackingField = 10.Days();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBrokenDurationMax\u003Ek__BackingField = 20.Days();
      // ISSUE: reference to a compiler-generated field
      this.\u003CDailyBreakdownChanceWhenShouldBeBroken\u003Ek__BackingField = 8.Percent();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public static void Serialize(MaintenanceConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MaintenanceConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MaintenanceConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      PartialQuantity.Serialize(this.BaseReplenishPerMonth, writer);
      Duration.Serialize(this.BrokenDurationMax, writer);
      Duration.Serialize(this.BrokenDurationMin, writer);
      Duration.Serialize(this.BufferMaxCapacity, writer);
      Percent.Serialize(this.DailyBreakdownChanceWhenShouldBeBroken, writer);
      Percent.Serialize(this.IdleMaintenanceMultiplier, writer);
      Percent.Serialize(this.MaxBreakdownChance, writer);
      Percent.Serialize(this.MaxReplenishSpeed, writer);
      Percent.Serialize(this.ReliabilityIssuesStartAt, writer);
    }

    public static MaintenanceConfig Deserialize(BlobReader reader)
    {
      MaintenanceConfig maintenanceConfig;
      if (reader.TryStartClassDeserialization<MaintenanceConfig>(out maintenanceConfig))
        reader.EnqueueDataDeserialization((object) maintenanceConfig, MaintenanceConfig.s_deserializeDataDelayedAction);
      return maintenanceConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.BaseReplenishPerMonth = PartialQuantity.Deserialize(reader);
      this.BrokenDurationMax = Duration.Deserialize(reader);
      this.BrokenDurationMin = Duration.Deserialize(reader);
      this.BufferMaxCapacity = Duration.Deserialize(reader);
      if (reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      this.DailyBreakdownChanceWhenShouldBeBroken = Percent.Deserialize(reader);
      this.IdleMaintenanceMultiplier = Percent.Deserialize(reader);
      this.MaxBreakdownChance = Percent.Deserialize(reader);
      this.MaxReplenishSpeed = Percent.Deserialize(reader);
      this.ReliabilityIssuesStartAt = Percent.Deserialize(reader);
    }

    static MaintenanceConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MaintenanceConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MaintenanceConfig) obj).SerializeData(writer));
      MaintenanceConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MaintenanceConfig) obj).DeserializeData(reader));
    }
  }
}
