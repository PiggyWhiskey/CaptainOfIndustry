// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.RemoveSurfaceDecalCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public class RemoveSurfaceDecalCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly RectangleTerrainArea2i Area;

    public static void Serialize(RemoveSurfaceDecalCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveSurfaceDecalCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveSurfaceDecalCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
    }

    public static RemoveSurfaceDecalCmd Deserialize(BlobReader reader)
    {
      RemoveSurfaceDecalCmd removeSurfaceDecalCmd;
      if (reader.TryStartClassDeserialization<RemoveSurfaceDecalCmd>(out removeSurfaceDecalCmd))
        reader.EnqueueDataDeserialization((object) removeSurfaceDecalCmd, RemoveSurfaceDecalCmd.s_deserializeDataDelayedAction);
      return removeSurfaceDecalCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveSurfaceDecalCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
    }

    public RemoveSurfaceDecalCmd(RectangleTerrainArea2i area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = area;
    }

    static RemoveSurfaceDecalCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveSurfaceDecalCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RemoveSurfaceDecalCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
