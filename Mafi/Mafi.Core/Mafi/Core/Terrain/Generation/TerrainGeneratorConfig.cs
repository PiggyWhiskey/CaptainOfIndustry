// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGeneratorConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public sealed class TerrainGeneratorConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private bool m_terrainAlreadyGenerated;

    public static void Serialize(TerrainGeneratorConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainGeneratorConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainGeneratorConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.m_terrainAlreadyGenerated);
      writer.WriteGeneric<Type>(this.TerrainChunkGeneratorType);
    }

    public static TerrainGeneratorConfig Deserialize(BlobReader reader)
    {
      TerrainGeneratorConfig terrainGeneratorConfig;
      if (reader.TryStartClassDeserialization<TerrainGeneratorConfig>(out terrainGeneratorConfig))
        reader.EnqueueDataDeserialization((object) terrainGeneratorConfig, TerrainGeneratorConfig.s_deserializeDataDelayedAction);
      return terrainGeneratorConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.m_terrainAlreadyGenerated = reader.ReadBool();
      this.TerrainChunkGeneratorType = reader.ReadGenericAs<Type>();
    }

    public Type TerrainChunkGeneratorType { get; private set; }

    public TerrainGeneratorConfig(Type terrainGeneratorType)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetTerrainChunkGeneratorType(terrainGeneratorType);
    }

    public void SetTerrainChunkGeneratorType<T>() where T : ITerrainChunkGenerator
    {
      Assert.That<bool>(this.m_terrainAlreadyGenerated).IsFalse("Terrain was already generated.");
      this.TerrainChunkGeneratorType = typeof (T);
    }

    public void SetTerrainChunkGeneratorType(Type terrainGeneratorType)
    {
      Assert.That<bool>(this.m_terrainAlreadyGenerated).IsFalse("Terrain was already generated.");
      Assert.That<Type>(terrainGeneratorType).IsAssignableTo<ITerrainChunkGenerator>("Invalid map generator type.");
      this.TerrainChunkGeneratorType = terrainGeneratorType;
    }

    public void MarkTerrainAlreadyGenerated() => this.m_terrainAlreadyGenerated = true;

    static TerrainGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainGeneratorConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainGeneratorConfig) obj).SerializeData(writer));
      TerrainGeneratorConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainGeneratorConfig) obj).DeserializeData(reader));
    }
  }
}
