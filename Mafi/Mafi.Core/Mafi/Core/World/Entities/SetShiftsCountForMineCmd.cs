// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.SetShiftsCountForMineCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class SetShiftsCountForMineCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;
    public readonly int ShiftsCount;

    public static void Serialize(SetShiftsCountForMineCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetShiftsCountForMineCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetShiftsCountForMineCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.ShiftsCount);
    }

    public static SetShiftsCountForMineCmd Deserialize(BlobReader reader)
    {
      SetShiftsCountForMineCmd shiftsCountForMineCmd;
      if (reader.TryStartClassDeserialization<SetShiftsCountForMineCmd>(out shiftsCountForMineCmd))
        reader.EnqueueDataDeserialization((object) shiftsCountForMineCmd, SetShiftsCountForMineCmd.s_deserializeDataDelayedAction);
      return shiftsCountForMineCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetShiftsCountForMineCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetShiftsCountForMineCmd>(this, "ShiftsCount", (object) reader.ReadInt());
    }

    public SetShiftsCountForMineCmd(EntityId entityId, int shiftsCount)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.ShiftsCount = shiftsCount;
    }

    static SetShiftsCountForMineCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetShiftsCountForMineCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetShiftsCountForMineCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
