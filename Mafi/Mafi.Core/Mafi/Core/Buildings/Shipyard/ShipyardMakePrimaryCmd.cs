// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipyardMakePrimaryCmd
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
  public class ShipyardMakePrimaryCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipyardMakePrimaryCmd(EntityId shipyardId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
    }

    public static void Serialize(ShipyardMakePrimaryCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipyardMakePrimaryCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipyardMakePrimaryCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipyardMakePrimaryCmd Deserialize(BlobReader reader)
    {
      ShipyardMakePrimaryCmd shipyardMakePrimaryCmd;
      if (reader.TryStartClassDeserialization<ShipyardMakePrimaryCmd>(out shipyardMakePrimaryCmd))
        reader.EnqueueDataDeserialization((object) shipyardMakePrimaryCmd, ShipyardMakePrimaryCmd.s_deserializeDataDelayedAction);
      return shipyardMakePrimaryCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipyardMakePrimaryCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipyardMakePrimaryCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipyardMakePrimaryCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipyardMakePrimaryCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
