// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadTerrainConnection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct RoadTerrainConnection
  {
    public readonly Tile2i TerrainTile;
    public readonly RoadGraphNodeKey RoadGraphNode;
    public readonly bool IsEntranceToRoadGraph;

    public RoadTerrainConnection(
      Tile2i terrainTile,
      RoadGraphNodeKey roadGraphNode,
      bool isEntranceToRoadGraph)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TerrainTile = terrainTile;
      this.RoadGraphNode = roadGraphNode;
      this.IsEntranceToRoadGraph = isEntranceToRoadGraph;
    }
  }
}
