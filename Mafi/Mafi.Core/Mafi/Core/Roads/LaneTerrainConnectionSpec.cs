// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.LaneTerrainConnectionSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Roads
{
  public readonly struct LaneTerrainConnectionSpec
  {
    /// <summary>Terrain tile relative to the layout.</summary>
    public readonly RelTile2i LayoutTile;
    public readonly int LaneIndex;
    public readonly bool IsAtLaneStart;

    public LaneTerrainConnectionSpec(RelTile2i layoutTile, int laneIndex, bool isAtLaneStart)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.LayoutTile = layoutTile;
      this.LaneIndex = laneIndex;
      this.IsAtLaneStart = isAtLaneStart;
    }
  }
}
