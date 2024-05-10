// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IManagedVehiclePathFindingTask
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PathFinding
{
  public interface IManagedVehiclePathFindingTask : IVehiclePathFindingTask
  {
    /// <summary>
    /// Whether this task is waiting in the path-finder's queue for processing.
    /// </summary>
    bool IsWaitingForProcessing { get; }

    /// <summary>
    /// Whether this task is being currently processed by the path finder.
    /// </summary>
    bool IsBeingProcessed { get; }

    RelTile1i GetGoalOrthogonalDistance();

    void ResetToStart();

    void SetIsWaitingForProcessing(IVehiclePathFindingManager manager);

    void SetIsBeingProcessed();

    void SetResult(IVehiclePathFinder vehiclePathFinder, VehiclePfResultStatus status);
  }
}
