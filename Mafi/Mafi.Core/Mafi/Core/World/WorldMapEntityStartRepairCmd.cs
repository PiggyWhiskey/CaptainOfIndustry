// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapEntityStartRepairCmd
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
  public class WorldMapEntityStartRepairCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;

    public static void Serialize(WorldMapEntityStartRepairCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapEntityStartRepairCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapEntityStartRepairCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static WorldMapEntityStartRepairCmd Deserialize(BlobReader reader)
    {
      WorldMapEntityStartRepairCmd entityStartRepairCmd;
      if (reader.TryStartClassDeserialization<WorldMapEntityStartRepairCmd>(out entityStartRepairCmd))
        reader.EnqueueDataDeserialization((object) entityStartRepairCmd, WorldMapEntityStartRepairCmd.s_deserializeDataDelayedAction);
      return entityStartRepairCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapEntityStartRepairCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    public WorldMapEntityStartRepairCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    static WorldMapEntityStartRepairCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapEntityStartRepairCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      WorldMapEntityStartRepairCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
