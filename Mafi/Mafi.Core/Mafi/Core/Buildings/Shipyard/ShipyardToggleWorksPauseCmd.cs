// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardToggleWorksPauseCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GenerateSerializer(false, null, 0)]
  public class ShipyardToggleWorksPauseCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardToggleWorksPauseCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = EntityId.Invalid;
    }

    public ShipyardToggleWorksPauseCmd(EntityId shipyardId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
    }

    public static void Serialize(ShipyardToggleWorksPauseCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardToggleWorksPauseCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardToggleWorksPauseCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipyardToggleWorksPauseCmd Deserialize(BlobReader reader)
    {
      ShipyardToggleWorksPauseCmd toggleWorksPauseCmd;
      if (reader.TryStartClassDeserialization<ShipyardToggleWorksPauseCmd>(out toggleWorksPauseCmd))
        reader.EnqueueDataDeserialization((object) toggleWorksPauseCmd, ShipyardToggleWorksPauseCmd.s_deserializeDataDelayedAction);
      return toggleWorksPauseCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardToggleWorksPauseCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipyardToggleWorksPauseCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardToggleWorksPauseCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardToggleWorksPauseCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
