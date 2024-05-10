// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.IPathFindingVehicle
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public interface IPathFindingVehicle : 
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey
  {
    new EntityId Id { get; }

    VehiclePathFindingParams PathFindingParams { get; }

    bool TrackExploredTiles { get; }

    /// <summary>
    /// Current goal for navigation. The goal should NOT be cached since it is often pooled and the vehicle will
    /// clear the instance once new goal is set.
    /// </summary>
    Option<IVehicleGoal> NavigationGoal { get; }

    IPathFindingResult PathFindingResult { get; }

    int NavigationFailedStreak { get; }

    /// <summary>
    /// Fills given list with goal tiles for current goal. Some goals will always return the same tiles but some may
    /// return different tiles so this should be called as close to actual path-finding as possible.
    /// </summary>
    /// <returns>Whether this task is already completed.</returns>
    bool GetGoalTiles(
      IVehicleGoalFull vehicleGoal,
      int retryNumber,
      RelTile1i extraTolerancePerRetry,
      Tile2f? customStartTile,
      out Tile2f startTile,
      Lyst<Tile2i> extraStartTiles,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile);

    void SetUnreachableGoal(IVehicleGoal unreachableGoal);

    void ClearUnreachableGoal();
  }
}
