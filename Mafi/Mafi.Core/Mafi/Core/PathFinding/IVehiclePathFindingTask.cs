// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IVehiclePathFindingTask
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public interface IVehiclePathFindingTask
  {
    IPathFindingVehicle Vehicle { get; }

    VehiclePathFindingParams PathFindingParams { get; }

    int MaxRetries { get; }

    RelTile1i ExtraTolerancePerRetry { get; }

    bool AllowSimplePathOnly { get; }

    bool NavigateClosebyIsSufficient { get; }

    RelTile1f MaxNavigateClosebyDistance { get; }

    ThicknessTilesF MaxNavigateClosebyHeightDifference { get; }

    bool HasResult { get; }

    /// <summary>
    /// Start tiles. Only valid after `InitializeStartAndGoals` call.
    /// </summary>
    IIndexable<Tile2i> StartTiles { get; }

    Tile2i DistanceEstimationStartTile { get; }

    /// <summary>
    /// Goal tiles. Only valid after `InitializeStartAndGoals` call.
    /// </summary>
    IIndexable<Tile2i> GoalTiles { get; }

    Tile2i DistanceEstimationGoalTile { get; }

    /// <summary>
    /// Initializes goal tiles (discards previously stored ones). Note that for retires &gt; 0 only goals should
    /// be updated.
    /// </summary>
    /// <returns>Whether this task is already completed.</returns>
    bool InitializeStartAndGoals(int retryNumber);
  }
}
