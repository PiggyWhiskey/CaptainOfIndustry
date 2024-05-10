// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.FlatTerrainGenerator
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
  [DependencyRegisteredManually("")]
  [GenerateSerializer(false, null, 0)]
  public class FlatTerrainGenerator : ITerrainGenerator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FlatTerrainGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FlatTerrainGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FlatTerrainGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<TerrainMaterialProto>(this.Bedrock);
      writer.WriteBool(this.DoNotCreateOcean);
      HeightTilesF.Serialize(this.Height, writer);
      writer.WriteInt(this.TerrainHeight);
      writer.WriteInt(this.TerrainWidth);
    }

    public static FlatTerrainGenerator Deserialize(BlobReader reader)
    {
      FlatTerrainGenerator terrainGenerator;
      if (reader.TryStartClassDeserialization<FlatTerrainGenerator>(out terrainGenerator))
        reader.EnqueueDataDeserialization((object) terrainGenerator, FlatTerrainGenerator.s_deserializeDataDelayedAction);
      return terrainGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Bedrock = reader.ReadGenericAs<TerrainMaterialProto>();
      this.DoNotCreateOcean = reader.ReadBool();
      this.Height = HeightTilesF.Deserialize(reader);
      this.TerrainHeight = reader.ReadInt();
      this.TerrainWidth = reader.ReadInt();
    }

    public HeightTilesF Height { get; private set; }

    public int TerrainWidth { get; private set; }

    public int TerrainHeight { get; private set; }

    public TerrainMaterialProto Bedrock { get; private set; }

    public bool DoNotCreateOcean { get; private set; }

    public FlatTerrainGenerator(
      HeightTilesF height,
      TerrainMaterialProto bedrock,
      int terrainWidth,
      int terrainHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Height = height;
      this.Bedrock = bedrock;
      this.TerrainWidth = terrainWidth;
      this.TerrainHeight = terrainHeight;
    }

    public IEnumerator<string> GenerateTerrain(
      RelTile2i terrainSize,
      Dict<Chunk2i, ChunkTerrainData> resultChunks)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new FlatTerrainGenerator.\u003CGenerateTerrain\u003Ed__27(0)
      {
        \u003C\u003E4__this = this,
        terrainSize = terrainSize,
        resultChunks = resultChunks
      };
    }

    public IEnumerator<string> PostProcessTerrain(TerrainManager terrain, bool gameIsBeingLoaded)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new FlatTerrainGenerator.\u003CPostProcessTerrain\u003Ed__28(0);
    }

    public Lyst<ChunkTerrainData> RegenerateChunkNoPostProcess(IEnumerable<Chunk2i> coords)
    {
      throw new NotImplementedException();
    }

    static FlatTerrainGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FlatTerrainGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlatTerrainGenerator) obj).SerializeData(writer));
      FlatTerrainGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlatTerrainGenerator) obj).DeserializeData(reader));
    }
  }
}
