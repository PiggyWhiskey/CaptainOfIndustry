// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.RemoveAndDestroyVehicle
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class RemoveAndDestroyVehicle : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId Id;

    public static void Serialize(RemoveAndDestroyVehicle value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveAndDestroyVehicle>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveAndDestroyVehicle.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.Id, writer);
    }

    public static RemoveAndDestroyVehicle Deserialize(BlobReader reader)
    {
      RemoveAndDestroyVehicle andDestroyVehicle;
      if (reader.TryStartClassDeserialization<RemoveAndDestroyVehicle>(out andDestroyVehicle))
        reader.EnqueueDataDeserialization((object) andDestroyVehicle, RemoveAndDestroyVehicle.s_deserializeDataDelayedAction);
      return andDestroyVehicle;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveAndDestroyVehicle>(this, "Id", (object) EntityId.Deserialize(reader));
    }

    public RemoveAndDestroyVehicle(EntityId id)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = id;
    }

    static RemoveAndDestroyVehicle()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveAndDestroyVehicle.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RemoveAndDestroyVehicle.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
