// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.BatchAddSurfaceDecalCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public class BatchAddSurfaceDecalCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ImmutableArray<TileSurfaceCopyPasteData> Data;

    public static void Serialize(BatchAddSurfaceDecalCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BatchAddSurfaceDecalCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BatchAddSurfaceDecalCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<TileSurfaceCopyPasteData>.Serialize(this.Data, writer);
    }

    public static BatchAddSurfaceDecalCmd Deserialize(BlobReader reader)
    {
      BatchAddSurfaceDecalCmd addSurfaceDecalCmd;
      if (reader.TryStartClassDeserialization<BatchAddSurfaceDecalCmd>(out addSurfaceDecalCmd))
        reader.EnqueueDataDeserialization((object) addSurfaceDecalCmd, BatchAddSurfaceDecalCmd.s_deserializeDataDelayedAction);
      return addSurfaceDecalCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<BatchAddSurfaceDecalCmd>(this, "Data", (object) ImmutableArray<TileSurfaceCopyPasteData>.Deserialize(reader));
    }

    public BatchAddSurfaceDecalCmd(ImmutableArray<TileSurfaceCopyPasteData> data)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Data = data;
    }

    static BatchAddSurfaceDecalCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BatchAddSurfaceDecalCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      BatchAddSurfaceDecalCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
