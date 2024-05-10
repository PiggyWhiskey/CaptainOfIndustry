// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetHull
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
  /// <summary>Actual hull instance in the fleet entity.</summary>
  [GenerateSerializer(false, null, 0)]
  public class FleetHull : DestructibleFleetPart
  {
    public readonly FleetEntityHullProto Proto;
    public readonly UpgradableInt BattlePriority;
    public readonly UpgradableInt HitChanceWeight;
    public readonly UpgradableInt ExtraRoundsToEscape;
    public readonly UpgradableInt RadarRange;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FleetHull(FleetEntityHullProto proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RadarRange = new UpgradableInt(0);
      // ISSUE: explicit constructor call
      base.\u002Ector((DestructibleFleetPartProto) proto);
      this.Proto = proto.CheckNotNull<FleetEntityHullProto>();
      this.BattlePriority = new UpgradableInt(proto.BattlePriority);
      this.HitChanceWeight = new UpgradableInt(proto.HitChanceWeight);
      this.ExtraRoundsToEscape = new UpgradableInt(proto.ExtraRoundsToEscape);
    }

    public void SetHp(int hp) => this.CurrentHp = hp.CheckWithinIncl(0, this.MaxHp.GetValue());

    public static void Serialize(FleetHull value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetHull>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetHull.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      UpgradableInt.Serialize(this.BattlePriority, writer);
      UpgradableInt.Serialize(this.ExtraRoundsToEscape, writer);
      UpgradableInt.Serialize(this.HitChanceWeight, writer);
      writer.WriteGeneric<FleetEntityHullProto>(this.Proto);
      UpgradableInt.Serialize(this.RadarRange, writer);
    }

    public static FleetHull Deserialize(BlobReader reader)
    {
      FleetHull fleetHull;
      if (reader.TryStartClassDeserialization<FleetHull>(out fleetHull))
        reader.EnqueueDataDeserialization((object) fleetHull, FleetHull.s_deserializeDataDelayedAction);
      return fleetHull;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FleetHull>(this, "BattlePriority", (object) UpgradableInt.Deserialize(reader));
      reader.SetField<FleetHull>(this, "ExtraRoundsToEscape", (object) UpgradableInt.Deserialize(reader));
      reader.SetField<FleetHull>(this, "HitChanceWeight", (object) UpgradableInt.Deserialize(reader));
      reader.SetField<FleetHull>(this, "Proto", (object) reader.ReadGenericAs<FleetEntityHullProto>());
      reader.SetField<FleetHull>(this, "RadarRange", (object) UpgradableInt.Deserialize(reader));
    }

    static FleetHull()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetHull.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DestructibleFleetPart) obj).SerializeData(writer));
      FleetHull.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DestructibleFleetPart) obj).DeserializeData(reader));
    }
  }
}
