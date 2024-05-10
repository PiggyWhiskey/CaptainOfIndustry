// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.ToggleEnforceAssignedVehiclesForEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleEnforceAssignedVehiclesForEntityCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly bool IsEnforced;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleEnforceAssignedVehiclesForEntityCmd(
      IEntityEnforcingAssignedVehicles storage,
      bool isEnforced)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, isEnforced);
    }

    public ToggleEnforceAssignedVehiclesForEntityCmd(EntityId storageId, bool isEnforced)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.IsEnforced = isEnforced;
    }

    public static void Serialize(ToggleEnforceAssignedVehiclesForEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleEnforceAssignedVehiclesForEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleEnforceAssignedVehiclesForEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsEnforced);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static ToggleEnforceAssignedVehiclesForEntityCmd Deserialize(BlobReader reader)
    {
      ToggleEnforceAssignedVehiclesForEntityCmd vehiclesForEntityCmd;
      if (reader.TryStartClassDeserialization<ToggleEnforceAssignedVehiclesForEntityCmd>(out vehiclesForEntityCmd))
        reader.EnqueueDataDeserialization((object) vehiclesForEntityCmd, ToggleEnforceAssignedVehiclesForEntityCmd.s_deserializeDataDelayedAction);
      return vehiclesForEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleEnforceAssignedVehiclesForEntityCmd>(this, "IsEnforced", (object) reader.ReadBool());
      reader.SetField<ToggleEnforceAssignedVehiclesForEntityCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static ToggleEnforceAssignedVehiclesForEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleEnforceAssignedVehiclesForEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleEnforceAssignedVehiclesForEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
