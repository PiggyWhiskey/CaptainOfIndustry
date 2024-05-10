// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.DummyTerrainGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  internal class DummyTerrainGenerator : ITerrainGenerator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(DummyTerrainGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DummyTerrainGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DummyTerrainGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<TerrainMaterialProto>(this.Bedrock);
      writer.WriteBool(this.DoNotCreateOcean);
      writer.WriteInt(this.TerrainHeight);
      writer.WriteInt(this.TerrainWidth);
    }

    public static DummyTerrainGenerator Deserialize(BlobReader reader)
    {
      DummyTerrainGenerator terrainGenerator;
      if (reader.TryStartClassDeserialization<DummyTerrainGenerator>(out terrainGenerator))
        reader.EnqueueDataDeserialization((object) terrainGenerator, DummyTerrainGenerator.s_deserializeDataDelayedAction);
      return terrainGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Bedrock = reader.ReadGenericAs<TerrainMaterialProto>();
      this.DoNotCreateOcean = reader.ReadBool();
      this.TerrainHeight = reader.ReadInt();
      this.TerrainWidth = reader.ReadInt();
    }

    public int TerrainWidth { get; set; }

    public int TerrainHeight { get; set; }

    public TerrainMaterialProto Bedrock { get; set; }

    public bool DoNotCreateOcean { get; set; }

    public IEnumerator<string> GenerateTerrain(
      RelTile2i terrainSize,
      Dict<Chunk2i, ChunkTerrainData> resultChunks)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<string> PostProcessTerrain(TerrainManager terrain, bool gameIsBeingLoaded)
    {
      throw new NotImplementedException();
    }

    public Lyst<ChunkTerrainData> RegenerateChunkNoPostProcess(IEnumerable<Chunk2i> coords)
    {
      throw new NotImplementedException();
    }

    public DummyTerrainGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBedrock\u003Ek__BackingField = TerrainMaterialProto.PhantomTerrainMaterialProto;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static DummyTerrainGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DummyTerrainGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DummyTerrainGenerator) obj).SerializeData(writer));
      DummyTerrainGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DummyTerrainGenerator) obj).DeserializeData(reader));
    }
  }
}
