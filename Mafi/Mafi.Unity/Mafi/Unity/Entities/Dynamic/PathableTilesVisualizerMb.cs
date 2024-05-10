// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.PathableTilesVisualizerMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.InputControl;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  internal class PathableTilesVisualizerMb : MonoBehaviour
  {
    public bool ShowPathableTiles;
    public int AllTilesCount;
    public int ChangedTilesCount;
    private ISimLoopEvents m_simLoopEvents;
    private TerrainManager m_terrain;
    private TerrainCursor m_terrainCursor;
    private bool m_isEnabled;
    private bool m_tilesChanged;
    private Lyst<Vector3> m_nonPathableVectors;
    private bool m_printTileInfo;

    public void Initialize(
      ISimLoopEvents simLoopEvents,
      TerrainManager terrain,
      TerrainCursor terrainCursor)
    {
      this.m_simLoopEvents = simLoopEvents.CheckNotNull<ISimLoopEvents>();
      this.m_terrain = terrain.CheckNotNull<TerrainManager>();
      this.m_terrainCursor = terrainCursor.CheckNotNull<TerrainCursor>();
    }

    [PublicAPI]
    private void Update()
    {
      if (!this.ShowPathableTiles)
      {
        if (!this.m_isEnabled)
          return;
        this.disable();
      }
      else
      {
        if (!this.m_isEnabled)
          this.enable();
        this.m_printTileInfo = this.m_printTileInfo || Input.GetMouseButtonDown(0);
      }
    }

    [PublicAPI]
    private void OnDrawGizmos()
    {
      if (this.m_nonPathableVectors == null)
        return;
      Vector3 size = new Vector3(1f, 1f, 1f);
      Gizmos.color = Time.time.RoundToInt() % 2 == 0 ? Color.red : Color.gray;
      foreach (Vector3 nonPathableVector in this.m_nonPathableVectors)
        Gizmos.DrawCube(nonPathableVector, size);
    }

    private void syncUpdate()
    {
      if (this.m_nonPathableVectors == null)
      {
        Lyst<Vector3> lyst = new Lyst<Vector3>(1024);
        foreach (Tile2iAndIndex enumerateAllTile in this.m_terrain.EnumerateAllTiles())
        {
          if (this.m_terrain.IsBlockingVehicles(enumerateAllTile.Index))
            lyst.Add(this.m_terrain.ExtendHeight(enumerateAllTile.TileCoord.CenterTile2f).ToVector3());
        }
        this.m_nonPathableVectors = lyst;
      }
      if (!this.m_printTileInfo)
        return;
      this.m_printTileInfo = false;
      if (!this.m_terrainCursor.HasValue)
      {
        Log.Info("Unable to find corresponding tile.");
      }
      else
      {
        TerrainTile tile = this.m_terrainCursor.Tile;
        if (this.m_terrain.IsBlockingVehicles(tile.DataIndex))
          Log.Info("Non-pathable reasons: \r\n" + this.m_terrain.Debug_ExplainFlags(tile.DataIndex));
        else
          Log.Info("Tile is pathable.");
      }
    }

    private void enable()
    {
      this.m_isEnabled = true;
      this.m_simLoopEvents.Sync.AddNonSaveable<PathableTilesVisualizerMb>(this, new Action(this.syncUpdate));
      this.m_terrainCursor.Activate();
    }

    private void disable()
    {
      this.m_simLoopEvents.Sync.RemoveNonSaveable<PathableTilesVisualizerMb>(this, new Action(this.syncUpdate));
      this.m_nonPathableVectors = (Lyst<Vector3>) null;
      this.m_isEnabled = false;
      this.m_terrainCursor.Deactivate();
    }

    public PathableTilesVisualizerMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
