// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.ITerrainDesignationsManagerInternal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Vehicles.Jobs;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface ITerrainDesignationsManagerInternal : ITerrainDesignationsManager
  {
    void OnManagedTowersChanged(TerrainDesignation designation);

    void OnReachabilityChanged(TerrainDesignation designation);

    /// <summary>Return new number of jobs assigned.</summary>
    bool TryAddAssignedJob(
      TerrainDesignation designation,
      IVehicleJob job,
      out int numberOfJobsAssigned);

    /// <summary>
    /// Return new number of jobs assigned. Does assert if remove fails.
    /// </summary>
    int RemoveAssignedJob(TerrainDesignation designation, IVehicleJob job);

    /// <summary>Return jobs assigned or empty IIndexable.</summary>
    IIndexable<IVehicleJob> GetAssignedJobsFor(TerrainDesignation designation);
  }
}
