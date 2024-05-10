// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadLaneMetadata
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct RoadLaneMetadata
  {
    public readonly RelTile3f StartPosition;
    public readonly RelTile3f EndPosition;
    public readonly RoadGraphNodeDirection StartDirection;
    public readonly RoadGraphNodeDirection EndDirection;
    public readonly RelTile1f LaneLength;

    public RoadLaneMetadata(
      RelTile3f startPosition,
      RelTile3f endPosition,
      RoadGraphNodeDirection startDirection,
      RoadGraphNodeDirection endDirection,
      RelTile1f laneLength)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.StartPosition = startPosition;
      this.EndPosition = endPosition;
      this.StartDirection = startDirection;
      this.EndDirection = endDirection;
      this.LaneLength = laneLength;
    }
  }
}
