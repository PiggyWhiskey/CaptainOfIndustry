// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainCursorVisual
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainCursorVisual
  {
    private bool m_isActivated;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly TerrainManager m_terrainManager;
    private Tile3i? m_position;
    private Tile3i? m_shownPosition;
    private bool m_needsUpdate;
    private readonly BuildableMb m_mb;
    private HeightTilesF m_height;
    private HeightTilesF m_heightPlusX;
    private HeightTilesF m_heightPlusY;
    private HeightTilesF m_heightPlusXy;

    public TerrainCursorVisual(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrainManager,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_terrainManager = terrainManager;
      this.m_mb = new GameObject(nameof (TerrainCursorVisual)).AddComponent<BuildableMb>();
      this.m_mb.GetComponent<MeshRenderer>().sharedMaterial = assetsDb.GetSharedMaterial("Assets/Core/Materials/VertexColor.mat");
      this.m_mb.gameObject.SetActive(false);
      simLoopEvents.UpdateStart.AddNonSaveable<TerrainCursorVisual>(this, new Action(this.simUpdateStart));
    }

    public void Activate()
    {
      if (this.m_isActivated)
        return;
      this.m_isActivated = true;
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<TerrainCursorVisual>(this, new Action<GameTime>(this.renderUpdate));
      this.m_mb.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
      if (!this.m_isActivated)
        return;
      this.m_isActivated = false;
      this.m_position = new Tile3i?();
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<TerrainCursorVisual>(this, new Action<GameTime>(this.renderUpdate));
      this.m_mb.gameObject.SetActive(false);
    }

    public void ShowAt(Tile3i position) => this.m_position = new Tile3i?(position);

    public void Hide() => this.m_position = new Tile3i?();

    private void simUpdateStart()
    {
      if (!this.m_isActivated)
        return;
      Tile3i? shownPosition = this.m_shownPosition;
      Tile3i? position = this.m_position;
      if ((shownPosition.HasValue == position.HasValue ? (shownPosition.HasValue ? (shownPosition.GetValueOrDefault() == position.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
        return;
      this.m_shownPosition = this.m_position;
      if (!this.m_shownPosition.HasValue)
        return;
      TerrainTile terrainTile = this.m_terrainManager[this.m_shownPosition.Value.Xy];
      this.m_height = terrainTile.Height;
      TerrainTile plusXneighbor = terrainTile.PlusXNeighbor;
      this.m_heightPlusX = plusXneighbor.Height;
      TerrainTile plusYneighbor = terrainTile.PlusYNeighbor;
      this.m_heightPlusY = plusYneighbor.Height;
      plusXneighbor = plusYneighbor.PlusXNeighbor;
      this.m_heightPlusXy = plusXneighbor.Height;
      this.m_needsUpdate = true;
    }

    private void renderUpdate(GameTime time)
    {
      if (!this.m_needsUpdate)
        return;
      this.m_needsUpdate = false;
      MeshBuilder instance = MeshBuilder.Instance;
      this.generateCursorMesh(instance);
      instance.GetMbUpdatePackageAndClear().Apply((IBuildable) this.m_mb);
    }

    private void generateCursorMesh(MeshBuilder builder)
    {
      if (!this.m_shownPosition.HasValue)
        return;
      Vector3 vector3 = this.m_shownPosition.Value.CornerTile3f.ToVector3();
      Color32 color = new Color32((byte) 64, (byte) 64, (byte) 64, byte.MaxValue);
      builder.AddBox(new Vector3(vector3.x, this.m_height.ToUnityUnits(), vector3.z), new Vector3(vector3.x + 2f, this.m_heightPlusX.ToUnityUnits(), vector3.z), 0.05f, color);
      builder.AddBox(new Vector3(vector3.x + 2f, this.m_heightPlusX.ToUnityUnits(), vector3.z), new Vector3(vector3.x + 2f, this.m_heightPlusXy.ToUnityUnits(), vector3.z + 2f), 0.05f, color);
      builder.AddBox(new Vector3(vector3.x + 2f, this.m_heightPlusXy.ToUnityUnits(), vector3.z + 2f), new Vector3(vector3.x, this.m_heightPlusY.ToUnityUnits(), vector3.z + 2f), 0.05f, color);
      builder.AddBox(new Vector3(vector3.x, this.m_heightPlusY.ToUnityUnits(), vector3.z + 2f), new Vector3(vector3.x, this.m_height.ToUnityUnits(), vector3.z), 0.05f, color);
      builder.AddAaWireBox(this.m_shownPosition.Value.ToCenterVector3(), RelTile3f.Half.ToVector3(), 0.05f, new Color32((byte) 64, (byte) 64, (byte) 192, byte.MaxValue));
    }
  }
}
