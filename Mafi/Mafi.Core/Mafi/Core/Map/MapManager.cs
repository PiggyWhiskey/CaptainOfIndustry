// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Manages an island map (collection of <see cref="T:Mafi.Core.Map.MapCell" />). Allows unlocking cells and schedules terrain
  /// creation under unlocked or available-to-unlock cells.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public sealed class MapManager
  {
    public readonly IslandMap Map;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MapManager(IslandMap islandMap)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Map = islandMap;
    }

    /// <summary>
    /// Returns cell that contains given tile coordinate. Note that for tiles that are exactly on the boundary this
    /// returns arbitrary but deterministic cell.
    /// </summary>
    public Option<MapCell> GetCellForTile(Tile2i tile)
    {
      foreach (MapCell cellForTile in this.Map.GetCellsOnChunk(tile.ChunkCoord2i))
      {
        if (cellForTile.Contains(tile))
          return (Option<MapCell>) cellForTile;
      }
      return Option<MapCell>.None;
    }

    private void simUpdate()
    {
    }

    public static void Serialize(MapManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MapManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MapManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer) => IslandMap.Serialize(this.Map, writer);

    public static MapManager Deserialize(BlobReader reader)
    {
      MapManager mapManager;
      if (reader.TryStartClassDeserialization<MapManager>(out mapManager))
        reader.EnqueueDataDeserialization((object) mapManager, MapManager.s_deserializeDataDelayedAction);
      return mapManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<MapManager>(this, "Map", (object) IslandMap.Deserialize(reader));
    }

    static MapManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MapManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MapManager) obj).SerializeData(writer));
      MapManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MapManager) obj).DeserializeData(reader));
    }
  }
}
