// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMapBaseConfig
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
  [GenerateSerializer(false, null, 0)]
  public class WorldRegionMapBaseConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(WorldRegionMapBaseConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldRegionMapBaseConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldRegionMapBaseConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      MapEdgeType.Serialize(this.MapEdgeType, writer);
      MapOffLimitsSize.Serialize(this.OffLimitsSize, writer);
    }

    public static WorldRegionMapBaseConfig Deserialize(BlobReader reader)
    {
      WorldRegionMapBaseConfig regionMapBaseConfig;
      if (reader.TryStartClassDeserialization<WorldRegionMapBaseConfig>(out regionMapBaseConfig))
        reader.EnqueueDataDeserialization((object) regionMapBaseConfig, WorldRegionMapBaseConfig.s_deserializeDataDelayedAction);
      return regionMapBaseConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.MapEdgeType = MapEdgeType.Deserialize(reader);
      this.OffLimitsSize = MapOffLimitsSize.Deserialize(reader);
    }

    public MapEdgeType MapEdgeType { get; set; }

    public MapOffLimitsSize OffLimitsSize { get; set; }

    public WorldRegionMapBaseConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003COffLimitsSize\u003Ek__BackingField = MapOffLimitsSize.Default;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WorldRegionMapBaseConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldRegionMapBaseConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldRegionMapBaseConfig) obj).SerializeData(writer));
      WorldRegionMapBaseConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldRegionMapBaseConfig) obj).DeserializeData(reader));
    }
  }
}
