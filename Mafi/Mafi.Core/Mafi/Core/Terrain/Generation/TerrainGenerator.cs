// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [OnlyForSaveCompatibility(null)]
  [GenerateSerializer(false, null, 0)]
  public sealed class TerrainGenerator : ITerrainGenerator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int REPORT_PROGRESS_STEPS = 20;
    private readonly MapManager m_mapManager;
    private readonly TerrainGeneratorConfig m_config;
    private readonly DependencyResolver m_resolver;

    public static void Serialize(TerrainGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainGenerator.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      TerrainGeneratorConfig.Serialize(this.m_config, writer);
      MapManager.Serialize(this.m_mapManager, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
    }

    public static TerrainGenerator Deserialize(BlobReader reader)
    {
      TerrainGenerator terrainGenerator;
      if (reader.TryStartClassDeserialization<TerrainGenerator>(out terrainGenerator))
        reader.EnqueueDataDeserialization((object) terrainGenerator, TerrainGenerator.s_deserializeDataDelayedAction);
      return terrainGenerator;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainGenerator>(this, "m_config", (object) TerrainGeneratorConfig.Deserialize(reader));
      reader.SetField<TerrainGenerator>(this, "m_mapManager", (object) MapManager.Deserialize(reader));
      reader.SetField<TerrainGenerator>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
    }

    public int TerrainWidth => this.m_mapManager.Map.TerrainWidth;

    public int TerrainHeight => this.m_mapManager.Map.TerrainHeight;

    public TerrainMaterialProto Bedrock => this.m_mapManager.Map.Bedrock;

    public bool DoNotCreateOcean => this.m_mapManager.Map.Config.DoNotCreateOcean;

    public ImmutableArray<Chunk2i> TerrainChunkCoords => this.m_mapManager.Map.Chunks;

    public TerrainGenerator(
      MapManager mapManager,
      TerrainGeneratorConfig config,
      DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_resolver = resolver;
      this.m_mapManager = mapManager;
    }

    public IEnumerator<string> GenerateTerrain(
      RelTile2i terrainSize,
      Dict<Chunk2i, ChunkTerrainData> resultChunks)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TerrainGenerator.\u003CGenerateTerrain\u003Ed__21(0)
      {
        \u003C\u003E4__this = this,
        terrainSize = terrainSize,
        resultChunks = resultChunks
      };
    }

    public IEnumerator<string> PostProcessTerrain(TerrainManager terrain, bool gameIsBeingLoaded)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TerrainGenerator.\u003CPostProcessTerrain\u003Ed__22(0)
      {
        \u003C\u003E4__this = this,
        terrain = terrain,
        gameIsBeingLoaded = gameIsBeingLoaded
      };
    }

    public Lyst<ChunkTerrainData> RegenerateChunkNoPostProcess(IEnumerable<Chunk2i> coords)
    {
      if (this.m_config.TerrainChunkGeneratorType == (Type) null)
        throw new Exception("No terrain chunk generator was set at 'TerrainGeneratorConfig'.");
      object obj = this.m_resolver.Instantiate(this.m_config.TerrainChunkGeneratorType);
      ITerrainChunkGenerator chunkGenerator = obj as ITerrainChunkGenerator;
      if (chunkGenerator == null)
        throw new Exception("Configured terrain chunk generator '" + obj.GetType().Name + "' does not implement 'ITerrainChunkGenerator'.");
      chunkGenerator.InitializeTerrainGeneration(this.m_mapManager.Map);
      return coords.AsParallel<Chunk2i>().Select<Chunk2i, ChunkTerrainData>((Func<Chunk2i, ChunkTerrainData>) (x =>
      {
        ChunkTerrainData chunk = chunkGenerator.GenerateChunk(x);
        TerrainGenerator.cleanEmptyLayers(chunk);
        return chunk;
      })).ToLyst<ChunkTerrainData>();
    }

    private static void cleanEmptyLayers(ChunkTerrainData chunk)
    {
      foreach (TileTerrainData tileTerrainData in chunk.Data)
        tileTerrainData.Products.RemoveWhere((Predicate<TerrainMaterialThicknessSlim>) (x => x.IsEmpty));
    }

    static TerrainGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainGenerator) obj).SerializeData(writer));
      TerrainGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainGenerator) obj).DeserializeData(reader));
    }
  }
}
