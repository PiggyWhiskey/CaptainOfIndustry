// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.NewMapStartLocationProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain.Generation;

#nullable disable
namespace Mafi.Core.Map
{
  [DependencyRegisteredManually("")]
  public class NewMapStartLocationProvider : IStartLocationProvider
  {
    public StartingLocation StartingLocation { get; }

    public NewMapStartLocationProvider(IWorldRegionMap map, StartingLocationConfig config)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (map.StartingLocations.IsEmpty<IStartingLocationV2>())
      {
        Log.Info("No starting locations.");
        this.StartingLocation = new StartingLocation(Tile2i.Zero + map.Size / 2, Direction90.PlusX);
      }
      else
      {
        int index = config.SetStartingLocationIndex;
        if (index < 0 || index >= map.StartingLocations.Count)
          index = 0;
        this.StartingLocation = map.StartingLocations[index].ToV1();
      }
    }
  }
}
