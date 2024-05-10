// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.ChangedTilesVisualizerMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  internal class ChangedTilesVisualizerMb : MonoBehaviour
  {
    public bool ShowChangedTiles;
    public int AllTilesCount;
    public int ChangedTilesCount;
    private ISimLoopEvents m_simLoopEvents;
    private TerrainManager m_terrain;
    private bool m_isEnabled;
    private Vector3[] m_changedTilesPositions;

    public void Initialize(ISimLoopEvents simLoopEvents, TerrainManager terrain)
    {
      this.m_simLoopEvents = simLoopEvents.CheckNotNull<ISimLoopEvents>();
      this.m_terrain = terrain.CheckNotNull<TerrainManager>();
    }

    [PublicAPI]
    private void Update()
    {
      if (!this.ShowChangedTiles)
      {
        if (!this.m_isEnabled)
          return;
        this.disable();
      }
      else
      {
        if (this.m_isEnabled)
          return;
        this.enable();
      }
    }

    [PublicAPI]
    private void OnDrawGizmos()
    {
      if (this.m_changedTilesPositions == null)
        return;
      Vector3 size = new Vector3(1f, 1f, 1f);
      Gizmos.color = Time.time.RoundToInt() % 2 == 0 ? Color.red : Color.gray;
      foreach (Vector3 changedTilesPosition in this.m_changedTilesPositions)
        Gizmos.DrawCube(changedTilesPosition, size);
    }

    private void syncUpdate()
    {
      LystStruct<Vector3> lystStruct = new LystStruct<Vector3>();
      foreach (Tile2iAndIndex enumerateAllTile in this.m_terrain.EnumerateAllTiles())
      {
        if (this.m_terrain.IsChanged(enumerateAllTile.Index))
          lystStruct.Add(enumerateAllTile.TileCoord.CenterTile2f.ExtendHeight(this.m_terrain.GetHeight(enumerateAllTile.Index)).ToVector3());
      }
      this.AllTilesCount = this.m_terrain.TerrainTilesCount;
      this.ChangedTilesCount = lystStruct.Count;
      this.m_changedTilesPositions = lystStruct.ToArray();
      this.m_simLoopEvents.Sync.RemoveNonSaveable<ChangedTilesVisualizerMb>(this, new Action(this.syncUpdate));
    }

    private void enable()
    {
      this.m_isEnabled = true;
      this.m_simLoopEvents.Sync.AddNonSaveable<ChangedTilesVisualizerMb>(this, new Action(this.syncUpdate));
    }

    private void disable()
    {
      this.m_changedTilesPositions = (Vector3[]) null;
      this.m_isEnabled = false;
    }

    public ChangedTilesVisualizerMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
