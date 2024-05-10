// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Ramps.VehicleRamp
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Ramps
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleRamp : LayoutEntityBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => false;

    public VehicleRamp(
      EntityId id,
      LayoutEntityProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, proto, transform, context);
    }

    public static void Serialize(VehicleRamp value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleRamp>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleRamp.s_serializeDataDelayedAction);
    }

    public static VehicleRamp Deserialize(BlobReader reader)
    {
      VehicleRamp vehicleRamp;
      if (reader.TryStartClassDeserialization<VehicleRamp>(out vehicleRamp))
        reader.EnqueueDataDeserialization((object) vehicleRamp, VehicleRamp.s_deserializeDataDelayedAction);
      return vehicleRamp;
    }

    static VehicleRamp()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleRamp.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      VehicleRamp.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
