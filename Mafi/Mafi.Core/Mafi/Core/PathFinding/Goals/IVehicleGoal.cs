// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.IVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  public interface IVehicleGoal
  {
    LocStrFormatted GoalName { get; }

    bool IsInitialized { get; }

    Tile3f GetGoalPosition();

    bool GetGoalTiles(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry);

    /// <summary>
    /// Whether goal is valid for given vehicle and can start navigating to it.
    /// This is called periodically by the owning vehicle.
    /// </summary>
    bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf);

    /// <summary>
    /// Called when pathfinding is finished. The goal index represents is related to the last call of
    /// <see cref="M:Mafi.Core.PathFinding.Goals.IVehicleGoal.GetGoalTiles(Mafi.Tile2i,Mafi.Core.PathFinding.VehiclePathFindingParams,Mafi.Collections.Lyst{Mafi.Tile2i},Mafi.Tile2i@,System.Int32,Mafi.RelTile1i)" />. If not set, found goal was not part of the original goal tiles.
    /// </summary>
    void NotifyGoalFound(Tile2i foundGoal);

    void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle);
  }
}
