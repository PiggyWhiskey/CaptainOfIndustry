// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.RemoveDesignationsCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public class RemoveDesignationsCmd : InputCommand<ImmutableArray<Tile2i>>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ImmutableArray<Tile2i> Origins;

    public static void Serialize(RemoveDesignationsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveDesignationsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveDesignationsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<Tile2i>.Serialize(this.Origins, writer);
    }

    public static RemoveDesignationsCmd Deserialize(BlobReader reader)
    {
      RemoveDesignationsCmd removeDesignationsCmd;
      if (reader.TryStartClassDeserialization<RemoveDesignationsCmd>(out removeDesignationsCmd))
        reader.EnqueueDataDeserialization((object) removeDesignationsCmd, RemoveDesignationsCmd.s_deserializeDataDelayedAction);
      return removeDesignationsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveDesignationsCmd>(this, "Origins", (object) ImmutableArray<Tile2i>.Deserialize(reader));
    }

    public RemoveDesignationsCmd(ImmutableArray<Tile2i> origins)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Origins = origins;
    }

    static RemoveDesignationsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveDesignationsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<ImmutableArray<Tile2i>>) obj).SerializeData(writer));
      RemoveDesignationsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<ImmutableArray<Tile2i>>) obj).DeserializeData(reader));
    }
  }
}
