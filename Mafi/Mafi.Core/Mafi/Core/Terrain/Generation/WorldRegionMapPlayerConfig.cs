// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMapPlayerConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [Obsolete]
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public class WorldRegionMapPlayerConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(WorldRegionMapPlayerConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldRegionMapPlayerConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldRegionMapPlayerConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.SetStartingLocationIndex);
    }

    public static WorldRegionMapPlayerConfig Deserialize(BlobReader reader)
    {
      WorldRegionMapPlayerConfig regionMapPlayerConfig;
      if (reader.TryStartClassDeserialization<WorldRegionMapPlayerConfig>(out regionMapPlayerConfig))
        reader.EnqueueDataDeserialization((object) regionMapPlayerConfig, WorldRegionMapPlayerConfig.s_deserializeDataDelayedAction);
      return regionMapPlayerConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.SetStartingLocationIndex = reader.ReadInt();
    }

    public int SetStartingLocationIndex { get; set; }

    public WorldRegionMapPlayerConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WorldRegionMapPlayerConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldRegionMapPlayerConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldRegionMapPlayerConfig) obj).SerializeData(writer));
      WorldRegionMapPlayerConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldRegionMapPlayerConfig) obj).DeserializeData(reader));
    }
  }
}
