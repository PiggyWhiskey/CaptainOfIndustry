// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapEntityCancelRepairCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapEntityCancelRepairCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;

    public static void Serialize(WorldMapEntityCancelRepairCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapEntityCancelRepairCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapEntityCancelRepairCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static WorldMapEntityCancelRepairCmd Deserialize(BlobReader reader)
    {
      WorldMapEntityCancelRepairCmd entityCancelRepairCmd;
      if (reader.TryStartClassDeserialization<WorldMapEntityCancelRepairCmd>(out entityCancelRepairCmd))
        reader.EnqueueDataDeserialization((object) entityCancelRepairCmd, WorldMapEntityCancelRepairCmd.s_deserializeDataDelayedAction);
      return entityCancelRepairCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapEntityCancelRepairCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    public WorldMapEntityCancelRepairCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    static WorldMapEntityCancelRepairCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapEntityCancelRepairCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      WorldMapEntityCancelRepairCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
