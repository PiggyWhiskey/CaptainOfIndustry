// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapEntityUpgradeCmd
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
  public class WorldMapEntityUpgradeCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;

    public static void Serialize(WorldMapEntityUpgradeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapEntityUpgradeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapEntityUpgradeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static WorldMapEntityUpgradeCmd Deserialize(BlobReader reader)
    {
      WorldMapEntityUpgradeCmd entityUpgradeCmd;
      if (reader.TryStartClassDeserialization<WorldMapEntityUpgradeCmd>(out entityUpgradeCmd))
        reader.EnqueueDataDeserialization((object) entityUpgradeCmd, WorldMapEntityUpgradeCmd.s_deserializeDataDelayedAction);
      return entityUpgradeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapEntityUpgradeCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    public WorldMapEntityUpgradeCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    static WorldMapEntityUpgradeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapEntityUpgradeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      WorldMapEntityUpgradeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
