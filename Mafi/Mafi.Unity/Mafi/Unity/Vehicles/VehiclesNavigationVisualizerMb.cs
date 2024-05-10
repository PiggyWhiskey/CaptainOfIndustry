// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.VehiclesNavigationVisualizerMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.GameLoop;
using Mafi.Core.PathFinding;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Unity.Entities.Dynamic;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class VehiclesNavigationVisualizerMb : MonoBehaviour
  {
    private IGameLoopEvents m_gameLoopEvents;
    private IVehiclesManager m_vehiclesManager;
    private TerrainManager m_terrainManager;
    private readonly Lyst<Vector3> m_vehicleOutlineLines;
    private readonly Lyst<Vector3> m_driveTargetLines;
    private readonly Lyst<ImmutableArray<Vector3>> m_paths;

    public void Initialize(
      IGameLoopEvents gameLoopEvents,
      IVehiclesManager vehiclesManager,
      TerrainManager terrainManager)
    {
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_vehiclesManager = vehiclesManager;
      this.m_terrainManager = terrainManager;
    }

    public void OnEnable()
    {
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<VehiclesNavigationVisualizerMb>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void OnDisable()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<VehiclesNavigationVisualizerMb>(this, new Action<GameTime>(this.syncUpdate));
    }

    private void syncUpdate(GameTime time)
    {
      this.m_vehicleOutlineLines.Clear();
      this.m_paths.Clear();
      this.m_driveTargetLines.Clear();
      foreach (Mafi.Core.Entities.Dynamic.Vehicle allVehicle in (IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>) this.m_vehiclesManager.AllVehicles)
      {
        Vector3 frontLeftWheel;
        Vector3 frontRightWheel;
        Vector3 rearLeftWheel;
        Vector3 rearRightWheel;
        DynamicGroundEntityMb.ComputeWheelPositions((DynamicGroundEntity) allVehicle, out frontLeftWheel, out frontRightWheel, out rearLeftWheel, out rearRightWheel);
        this.m_vehicleOutlineLines.Add(allVehicle.Position3f.ToVector3());
        this.m_vehicleOutlineLines.Add(frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel);
        VehiclePathFindingParams pathFindingParams = allVehicle.PathFindingParams;
        Tile2f? target = allVehicle.Target;
        if (target.HasValue)
        {
          Gizmos.color = Color.red;
          Vector3 vector3_1 = allVehicle.Position3f.AddZ(Fix32.Half).ToVector3();
          target = allVehicle.Target;
          Vector3 vector3_2 = target.Value.ExtendZ((Fix32) 0).ToVector3() with
          {
            y = vector3_1.y
          };
          this.m_driveTargetLines.Add(vector3_1, vector3_2);
        }
        int num = allVehicle.TrackExploredTiles ? 1 : 0;
      }
    }

    public void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      foreach (ImmutableArray<Vector3> path in this.m_paths)
      {
        for (int index = 1; index < path.Length; ++index)
          Gizmos.DrawLine(path[index - 1], path[index]);
      }
      Gizmos.color = Color.yellow;
      for (int index = 0; index < this.m_vehicleOutlineLines.Count; index += 5)
      {
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 1], this.m_vehicleOutlineLines[index]);
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 2], this.m_vehicleOutlineLines[index]);
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 1], this.m_vehicleOutlineLines[index + 2]);
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 2], this.m_vehicleOutlineLines[index + 4]);
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 4], this.m_vehicleOutlineLines[index + 3]);
        Gizmos.DrawLine(this.m_vehicleOutlineLines[index + 3], this.m_vehicleOutlineLines[index + 1]);
      }
      Gizmos.color = Color.red;
      for (int index = 0; index < this.m_driveTargetLines.Count; index += 2)
        Gizmos.DrawLine(this.m_driveTargetLines[index], this.m_driveTargetLines[index + 1]);
    }

    public VehiclesNavigationVisualizerMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_vehicleOutlineLines = new Lyst<Vector3>(true);
      this.m_driveTargetLines = new Lyst<Vector3>(true);
      this.m_paths = new Lyst<ImmutableArray<Vector3>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
