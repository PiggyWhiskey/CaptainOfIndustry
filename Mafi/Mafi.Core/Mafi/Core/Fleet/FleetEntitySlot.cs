// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntitySlot
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
  public class FleetEntitySlot
  {
    public readonly FleetEntitySlotProto Proto;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Part that is currently assigned to this slot.</summary>
    public Option<FleetEntityPartProto> ExistingPart { get; set; }

    public FleetEntitySlot(FleetEntitySlotProto proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = proto;
    }

    public static void Serialize(FleetEntitySlot value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetEntitySlot>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetEntitySlot.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<FleetEntityPartProto>.Serialize(this.ExistingPart, writer);
      writer.WriteGeneric<FleetEntitySlotProto>(this.Proto);
    }

    public static FleetEntitySlot Deserialize(BlobReader reader)
    {
      FleetEntitySlot fleetEntitySlot;
      if (reader.TryStartClassDeserialization<FleetEntitySlot>(out fleetEntitySlot))
        reader.EnqueueDataDeserialization((object) fleetEntitySlot, FleetEntitySlot.s_deserializeDataDelayedAction);
      return fleetEntitySlot;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ExistingPart = Option<FleetEntityPartProto>.Deserialize(reader);
      reader.SetField<FleetEntitySlot>(this, "Proto", (object) reader.ReadGenericAs<FleetEntitySlotProto>());
    }

    static FleetEntitySlot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntitySlot.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FleetEntitySlot) obj).SerializeData(writer));
      FleetEntitySlot.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FleetEntitySlot) obj).DeserializeData(reader));
    }
  }
}
