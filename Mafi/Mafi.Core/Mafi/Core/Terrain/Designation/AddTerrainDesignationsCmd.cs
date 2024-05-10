// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.AddTerrainDesignationsCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public class AddTerrainDesignationsCmd : InputCommand<ImmutableArray<Tile2i>>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Proto.ID ProtoId;
    public readonly ImmutableArray<DesignationData> Data;

    public static void Serialize(AddTerrainDesignationsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddTerrainDesignationsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddTerrainDesignationsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<DesignationData>.Serialize(this.Data, writer);
      Proto.ID.Serialize(this.ProtoId, writer);
    }

    public static AddTerrainDesignationsCmd Deserialize(BlobReader reader)
    {
      AddTerrainDesignationsCmd terrainDesignationsCmd;
      if (reader.TryStartClassDeserialization<AddTerrainDesignationsCmd>(out terrainDesignationsCmd))
        reader.EnqueueDataDeserialization((object) terrainDesignationsCmd, AddTerrainDesignationsCmd.s_deserializeDataDelayedAction);
      return terrainDesignationsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddTerrainDesignationsCmd>(this, "Data", (object) ImmutableArray<DesignationData>.Deserialize(reader));
      reader.SetField<AddTerrainDesignationsCmd>(this, "ProtoId", (object) Proto.ID.Deserialize(reader));
    }

    public AddTerrainDesignationsCmd(Proto.ID protoId, ImmutableArray<DesignationData> data)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.Data = data;
    }

    public override bool ResultEqualTo(ImmutableArray<Tile2i> other)
    {
      return this.Result.ValuesEquals(other);
    }

    static AddTerrainDesignationsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddTerrainDesignationsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<ImmutableArray<Tile2i>>) obj).SerializeData(writer));
      AddTerrainDesignationsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<ImmutableArray<Tile2i>>) obj).DeserializeData(reader));
    }
  }
}
