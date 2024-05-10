// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapSettlementAdoptPopsCmd
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
  public class WorldMapSettlementAdoptPopsCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;
    public readonly int PopsCount;

    public static void Serialize(WorldMapSettlementAdoptPopsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapSettlementAdoptPopsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapSettlementAdoptPopsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.PopsCount);
    }

    public static WorldMapSettlementAdoptPopsCmd Deserialize(BlobReader reader)
    {
      WorldMapSettlementAdoptPopsCmd settlementAdoptPopsCmd;
      if (reader.TryStartClassDeserialization<WorldMapSettlementAdoptPopsCmd>(out settlementAdoptPopsCmd))
        reader.EnqueueDataDeserialization((object) settlementAdoptPopsCmd, WorldMapSettlementAdoptPopsCmd.s_deserializeDataDelayedAction);
      return settlementAdoptPopsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapSettlementAdoptPopsCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<WorldMapSettlementAdoptPopsCmd>(this, "PopsCount", (object) reader.ReadInt());
    }

    public WorldMapSettlementAdoptPopsCmd(EntityId entityId, int popsCount)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.PopsCount = popsCount;
    }

    static WorldMapSettlementAdoptPopsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapSettlementAdoptPopsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      WorldMapSettlementAdoptPopsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
