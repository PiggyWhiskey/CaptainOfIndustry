// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IVehiclePathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Vehicles;
using Mafi.PathFinding;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public interface IVehiclePathFinder
  {
    int CurrentPfId { get; }

    int TotalStepsCount { get; }

    Tile2i DistanceEstimationStartCoord { get; }

    Tile2i DistanceEstimationGoalCoord { get; }

    IPathabilityProvider PathabilityProvider { get; }

    VehiclePathFinderInitResult InitVehiclePathFinding(
      IVehiclePathFindingTask task,
      ref int stepsLeft);

    PathFinderResult ContinueVehiclePathFinding(
      ref int stepsLeft,
      bool newTaskStartedThisSimStep,
      IVehicleSurfaceProvider surfaceProvider,
      bool isFinalAttempt);

    /// <summary>
    /// Adds new goal nodes to the ongoing or failed path finding run. This can be used to extend search without
    /// starting completely new rerun.
    /// </summary>
    bool? TryExtendGoals();

    /// <summary>
    /// Reconstructs found path. This should be only called when either
    /// <see cref="M:Mafi.Core.PathFinding.IVehiclePathFinder.InitVehiclePathFinding(Mafi.Core.PathFinding.IVehiclePathFindingTask,System.Int32@)" /> returned <see cref="F:Mafi.Core.PathFinding.VehiclePathFinderInitResult.GoalAlreadyReached" />, or
    /// <see cref="M:Mafi.Core.PathFinding.IVehiclePathFinder.ContinueVehiclePathFinding(System.Int32@,System.Boolean,Mafi.Core.Vehicles.IVehicleSurfaceProvider,System.Boolean)" /> returned <see cref="F:Mafi.PathFinding.PathFinderResult.PathFound" />.
    /// </summary>
    bool TryReconstructFoundPath(out IVehiclePathSegment firstPathSegment, out Tile2i goalTileRaw);

    /// <summary>
    /// Reconstructs all explored nodes. Can be called at any stage of PF.
    /// </summary>
    void GetExploredTiles(Lyst<ExploredPfNode> exploredTiles);

    void ResetState();

    /// <summary>
    /// Finds closest valid position for given pf params. This does not have to be the very closest empty space
    /// but rather some position that is known to be valid, like previously visited or explored position.
    /// This is used to unstuck vehicles.
    /// </summary>
    Tile2i? FindClosestValidPosition(
      Tile2i coord,
      VehiclePathFindingParams pfParams,
      Predicate<PfNode> predicate = null);
  }
}
