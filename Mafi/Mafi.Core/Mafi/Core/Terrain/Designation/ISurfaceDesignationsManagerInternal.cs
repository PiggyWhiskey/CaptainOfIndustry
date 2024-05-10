// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.ISurfaceDesignationsManagerInternal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Vehicles.Jobs;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface ISurfaceDesignationsManagerInternal : ISurfaceDesignationsManager
  {
    TerrainManager TerrainManager { get; }

    /// <summary>Return jobs assigned or empty IIndexable.</summary>
    IIndexable<IVehicleJobWithOwner> GetAssignedJobsFor(SurfaceDesignation designation);
  }
}
