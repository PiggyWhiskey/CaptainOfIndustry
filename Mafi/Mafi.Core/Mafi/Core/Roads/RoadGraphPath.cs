// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadGraphPath
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Roads
{
  public class RoadGraphPath
  {
    public readonly ImmutableArray<RoadPathSegment> Path;
    public readonly Tile2i? StartTile;
    public readonly Tile2i GoalTile;
    public readonly Fix32 TotalDistance;

    public RoadGraphPath(
      ImmutableArray<RoadPathSegment> path,
      Tile2i? startTile,
      Tile2i goalTile,
      Fix32 totalDistance)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Path = path;
      this.StartTile = startTile;
      this.GoalTile = goalTile;
      this.TotalDistance = totalDistance;
    }
  }
}
