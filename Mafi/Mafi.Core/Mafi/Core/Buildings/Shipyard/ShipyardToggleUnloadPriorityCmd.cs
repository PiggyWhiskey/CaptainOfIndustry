// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardToggleUnloadPriorityCmd
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
  public class ShipyardToggleUnloadPriorityCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardToggleUnloadPriorityCmd(EntityId shipyardId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
    }

    public static void Serialize(ShipyardToggleUnloadPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardToggleUnloadPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardToggleUnloadPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipyardToggleUnloadPriorityCmd Deserialize(BlobReader reader)
    {
      ShipyardToggleUnloadPriorityCmd unloadPriorityCmd;
      if (reader.TryStartClassDeserialization<ShipyardToggleUnloadPriorityCmd>(out unloadPriorityCmd))
        reader.EnqueueDataDeserialization((object) unloadPriorityCmd, ShipyardToggleUnloadPriorityCmd.s_deserializeDataDelayedAction);
      return unloadPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardToggleUnloadPriorityCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipyardToggleUnloadPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardToggleUnloadPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardToggleUnloadPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
