// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.LineWorldMapGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.Linq;

#nullable disable
namespace Mafi.Core.World
{
  public class LineWorldMapGenerator : IWorldMapGenerator
  {
    private readonly int m_count;
    private readonly int m_spacing;
    private readonly bool m_allExplored;

    public LineWorldMapGenerator(int count, int spacing, bool allExplored = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_allExplored = allExplored;
      this.m_count = count.CheckPositive();
      this.m_spacing = spacing.CheckPositive();
    }

    public WorldMap CreateWorldMap()
    {
      WorldMap worldMap = new WorldMap();
      WorldMapLocation loc1 = (WorldMapLocation) null;
      for (int index = 0; index < this.m_count; ++index)
      {
        WorldMapLocation worldMapLocation = new WorldMapLocation(index.ToString(), new Vector2i(index * this.m_spacing, 0));
        if (this.m_allExplored)
          worldMapLocation.SetState(WorldMapLocationState.Explored);
        worldMap.AddLocation(worldMapLocation);
        if (loc1 != null)
          worldMap.AddConnection(loc1, worldMapLocation);
        loc1 = worldMapLocation;
      }
      worldMap.SetHomeLocation(worldMap.Locations.First<WorldMapLocation>());
      return worldMap;
    }
  }
}
