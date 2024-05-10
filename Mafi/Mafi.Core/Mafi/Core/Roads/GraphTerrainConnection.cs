// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.GraphTerrainConnection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct GraphTerrainConnection
  {
    public readonly int RoadNodeId;
    public readonly Tile2iSlim TerrainTile;
    public readonly bool IsFromTerrainToRoad;

    public GraphTerrainConnection(int roadNodeId, Tile2iSlim terrainTile, bool isFromTerrainToRoad)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RoadNodeId = roadNodeId;
      this.TerrainTile = terrainTile;
      this.IsFromTerrainToRoad = isFromTerrainToRoad;
    }

    public override string ToString()
    {
      return !this.IsFromTerrainToRoad ? string.Format("From road node {0} to terrain {1}", (object) this.RoadNodeId, (object) this.TerrainTile) : string.Format("From terrain {0} to road node {1}", (object) this.TerrainTile, (object) this.RoadNodeId);
    }
  }
}
