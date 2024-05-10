// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.AddSurfaceDesignationsCmd
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
  public class AddSurfaceDesignationsCmd : InputCommand<ImmutableArray<Tile2i>>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Proto.ID ProtoId;
    public readonly ImmutableArray<SurfaceDesignationData> Data;

    public static void Serialize(AddSurfaceDesignationsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddSurfaceDesignationsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddSurfaceDesignationsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<SurfaceDesignationData>.Serialize(this.Data, writer);
      Proto.ID.Serialize(this.ProtoId, writer);
    }

    public static AddSurfaceDesignationsCmd Deserialize(BlobReader reader)
    {
      AddSurfaceDesignationsCmd surfaceDesignationsCmd;
      if (reader.TryStartClassDeserialization<AddSurfaceDesignationsCmd>(out surfaceDesignationsCmd))
        reader.EnqueueDataDeserialization((object) surfaceDesignationsCmd, AddSurfaceDesignationsCmd.s_deserializeDataDelayedAction);
      return surfaceDesignationsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddSurfaceDesignationsCmd>(this, "Data", (object) ImmutableArray<SurfaceDesignationData>.Deserialize(reader));
      reader.SetField<AddSurfaceDesignationsCmd>(this, "ProtoId", (object) Proto.ID.Deserialize(reader));
    }

    public AddSurfaceDesignationsCmd(Proto.ID protoId, ImmutableArray<SurfaceDesignationData> data)
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

    static AddSurfaceDesignationsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddSurfaceDesignationsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<ImmutableArray<Tile2i>>) obj).SerializeData(writer));
      AddSurfaceDesignationsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<ImmutableArray<Tile2i>>) obj).DeserializeData(reader));
    }
  }
}
