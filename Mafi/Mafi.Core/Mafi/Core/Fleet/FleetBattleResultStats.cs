// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetBattleResultStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  [GenerateSerializer(false, null, 0)]
  public class FleetBattleResultStats
  {
    public readonly string Name;
    public readonly int DamageDone;
    public readonly int DamageMissed;
    public readonly int DamageTaken;
    public readonly int ArmorDamageReduction;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FleetBattleResultStats(
      string name,
      int damageDone,
      int damageMissed,
      int damageTaken,
      int armorDamageReduction)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name.CheckNotNullOrEmpty();
      this.DamageDone = damageDone.CheckNotNegative();
      this.DamageMissed = damageMissed.CheckNotNegative();
      this.DamageTaken = damageTaken.CheckNotNegative();
      this.ArmorDamageReduction = armorDamageReduction.CheckNotNegative();
    }

    public static void Serialize(FleetBattleResultStats value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetBattleResultStats>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetBattleResultStats.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.ArmorDamageReduction);
      writer.WriteInt(this.DamageDone);
      writer.WriteInt(this.DamageMissed);
      writer.WriteInt(this.DamageTaken);
      writer.WriteString(this.Name);
    }

    public static FleetBattleResultStats Deserialize(BlobReader reader)
    {
      FleetBattleResultStats battleResultStats;
      if (reader.TryStartClassDeserialization<FleetBattleResultStats>(out battleResultStats))
        reader.EnqueueDataDeserialization((object) battleResultStats, FleetBattleResultStats.s_deserializeDataDelayedAction);
      return battleResultStats;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FleetBattleResultStats>(this, "ArmorDamageReduction", (object) reader.ReadInt());
      reader.SetField<FleetBattleResultStats>(this, "DamageDone", (object) reader.ReadInt());
      reader.SetField<FleetBattleResultStats>(this, "DamageMissed", (object) reader.ReadInt());
      reader.SetField<FleetBattleResultStats>(this, "DamageTaken", (object) reader.ReadInt());
      reader.SetField<FleetBattleResultStats>(this, "Name", (object) reader.ReadString());
    }

    static FleetBattleResultStats()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetBattleResultStats.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FleetBattleResultStats) obj).SerializeData(writer));
      FleetBattleResultStats.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FleetBattleResultStats) obj).DeserializeData(reader));
    }
  }
}
