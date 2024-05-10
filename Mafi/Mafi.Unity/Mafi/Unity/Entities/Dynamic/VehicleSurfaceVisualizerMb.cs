// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.VehicleSurfaceVisualizerMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  internal class VehicleSurfaceVisualizerMb : MonoBehaviour
  {
    private VehicleSurfaceProvider m_vehicleSurfaceProvider;
    private TerrainManager m_terrainManager;
    private KeyValuePair<Vector3, Color>[] m_occupiedTiles;

    public void Initialize(
      VehicleSurfaceProvider vehicleSurfaceProvider,
      IGameLoopEvents gameLoopEvents,
      TerrainManager terrainManager)
    {
      this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
      this.m_terrainManager = terrainManager;
      vehicleSurfaceProvider.OnVehicleSurfaceChanged.AddNonSaveable<VehicleSurfaceVisualizerMb>(this, (Action<Tile2i>) (t => this.m_occupiedTiles = (KeyValuePair<Vector3, Color>[]) null));
      gameLoopEvents.SyncUpdate.AddNonSaveable<VehicleSurfaceVisualizerMb>(this, new Action<GameTime>(this.syncUpdate));
    }

    private void syncUpdate(GameTime time)
    {
      if (this.gameObject.activeSelf)
      {
        if (this.m_occupiedTiles != null)
          return;
        this.m_occupiedTiles = this.m_vehicleSurfaceProvider.EntityHeights.Select<KeyValuePair<Tile2i, VehicleSurfaceProvider.SurfaceHeights>, KeyValuePair<Vector3, Color>>((Func<KeyValuePair<Tile2i, VehicleSurfaceProvider.SurfaceHeights>, KeyValuePair<Vector3, Color>>) (kvp =>
        {
          HeightTilesF height;
          Color color;
          if (kvp.Value.Height == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT)
          {
            height = this.m_terrainManager.GetHeight(kvp.Key);
            color = Color.red;
          }
          else
          {
            height = kvp.Value.Height;
            color = Color.green;
          }
          return Make.Kvp<Vector3, Color>(kvp.Key.CornerTile2f.ExtendHeight(height).ToVector3(), color);
        })).ToArray<KeyValuePair<Vector3, Color>>();
      }
      else
        this.m_occupiedTiles = (KeyValuePair<Vector3, Color>[]) null;
    }

    private void OnDrawGizmos()
    {
      KeyValuePair<Vector3, Color>[] occupiedTiles = this.m_occupiedTiles;
      if (occupiedTiles == null)
        return;
      foreach (KeyValuePair<Vector3, Color> keyValuePair in occupiedTiles)
      {
        Gizmos.color = keyValuePair.Value;
        Gizmos.DrawSphere(keyValuePair.Key, 0.5f);
      }
    }

    public VehicleSurfaceVisualizerMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
