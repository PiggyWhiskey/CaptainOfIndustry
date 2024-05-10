// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadConnectionPointProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct RoadConnectionPointProto
  {
    public readonly RelTile3i PositionRelative;
    public readonly RelTile3i DirectionSigns;
    public readonly RoadConnectionType Type;

    public RoadConnectionPointProto(
      RelTile3i positionRelative,
      RelTile3i directionSigns,
      RoadConnectionType type)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.PositionRelative = positionRelative;
      this.DirectionSigns = directionSigns;
      this.Type = type;
    }
  }
}
