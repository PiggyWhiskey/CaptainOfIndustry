// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.RemoveSurfaceDesignationsCmd
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
  public class RemoveSurfaceDesignationsCmd : InputCommand<ImmutableArray<Tile2i>>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ImmutableArray<Tile2i> Origins;
    public readonly RectangleTerrainArea2i Area;

    public static void Serialize(RemoveSurfaceDesignationsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveSurfaceDesignationsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveSurfaceDesignationsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      ImmutableArray<Tile2i>.Serialize(this.Origins, writer);
    }

    public static RemoveSurfaceDesignationsCmd Deserialize(BlobReader reader)
    {
      RemoveSurfaceDesignationsCmd surfaceDesignationsCmd;
      if (reader.TryStartClassDeserialization<RemoveSurfaceDesignationsCmd>(out surfaceDesignationsCmd))
        reader.EnqueueDataDeserialization((object) surfaceDesignationsCmd, RemoveSurfaceDesignationsCmd.s_deserializeDataDelayedAction);
      return surfaceDesignationsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveSurfaceDesignationsCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
      reader.SetField<RemoveSurfaceDesignationsCmd>(this, "Origins", (object) ImmutableArray<Tile2i>.Deserialize(reader));
    }

    public RemoveSurfaceDesignationsCmd(ImmutableArray<Tile2i> origins, RectangleTerrainArea2i area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Origins = origins;
      this.Area = area;
    }

    static RemoveSurfaceDesignationsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveSurfaceDesignationsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<ImmutableArray<Tile2i>>) obj).SerializeData(writer));
      RemoveSurfaceDesignationsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<ImmutableArray<Tile2i>>) obj).DeserializeData(reader));
    }
  }
}
