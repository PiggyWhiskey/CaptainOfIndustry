// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardSetRepairingCmd
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
  public class ShipyardSetRepairingCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    public readonly bool IsRepairing;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardSetRepairingCmd(bool isRepairing)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = EntityId.Invalid;
      this.IsRepairing = isRepairing;
    }

    public ShipyardSetRepairingCmd(EntityId shipyardId, bool isRepairing)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
      this.IsRepairing = isRepairing;
    }

    public static void Serialize(ShipyardSetRepairingCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardSetRepairingCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardSetRepairingCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsRepairing);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipyardSetRepairingCmd Deserialize(BlobReader reader)
    {
      ShipyardSetRepairingCmd shipyardSetRepairingCmd;
      if (reader.TryStartClassDeserialization<ShipyardSetRepairingCmd>(out shipyardSetRepairingCmd))
        reader.EnqueueDataDeserialization((object) shipyardSetRepairingCmd, ShipyardSetRepairingCmd.s_deserializeDataDelayedAction);
      return shipyardSetRepairingCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardSetRepairingCmd>(this, "IsRepairing", (object) reader.ReadBool());
      reader.SetField<ShipyardSetRepairingCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipyardSetRepairingCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardSetRepairingCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardSetRepairingCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
