// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IVehiclePathFindingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public interface IVehiclePathFindingManager
  {
    int QueueLength { get; }

    IPathabilityProvider PathabilityProvider { get; }

    /// <summary>
    /// Enqueues given task for path finding. Low priority numbers means higher priority. Each sim step
    /// we reduce priority by 1. This can be used to prevent low priority pathing from blocking higher
    /// priority pathing while ensuring the low priority pathing still completes.
    /// </summary>
    void EnqueueTask(IManagedVehiclePathFindingTask task, int priority);

    /// <summary>Aborts given task and removes it from the queue.</summary>
    void AbortTask(IManagedVehiclePathFindingTask task);

    /// <summary>
    /// Returns home entity for given vehicle. Lost vehicles will return to its home entity. Returns None if there
    /// is no home entity available.
    /// </summary>
    Option<IStaticEntity> GetHomeEntityFor(Vehicle vehicle);

    /// <summary>
    /// Finds closest valid position for given pf params. This does not have to be the very closest empty space
    /// but rather some position that is known to be valid, like previously visited or explored position.
    /// This is used to unstuck vehicles.
    /// </summary>
    Tile2i? FindClosestValidPfNode(
      Tile2i coord,
      VehiclePathFindingParams pfParams,
      Predicate<PfNode> predicate = null);
  }
}
