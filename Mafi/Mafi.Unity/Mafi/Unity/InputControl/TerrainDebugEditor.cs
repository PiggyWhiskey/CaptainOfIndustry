// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainDebugEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.Terrain;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class TerrainDebugEditor
  {
    public TerrainDebugEditor(
      NewInstanceOf<TerrainCursor> cursor,
      TerrainManager terrainManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      TerrainRenderer terrainRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GameObject gameObject = new GameObject("DEBUG: Terrain editor");
      gameObject.SetActive(false);
      gameObject.AddComponent<TerrainDebugEditor.TerrainDebugEditorMb>().Initialize(cursor.Instance, terrainManager, gameLoopEvents, simLoopEvents, terrainRenderer);
    }

    private class TerrainDebugEditorMb : MonoBehaviour
    {
      public int SetMaterialIndex;
      public int SetMaterialDepthTiles;
      public int RelativeHeight;
      public int ToolRadius;
      public bool Flatten;
      public bool Disrupt;
      public bool ActivatePhysicsOnly;
      public bool InspectOnly;
      public int InfoPlacedSlimId;
      public int InfoPlacedTextureIndex;
      [TextArea(5, 20)]
      public string AvailableMaterials;
      [TextArea(5, 20)]
      public string CurrentTileInfo;
      [TextArea(3, 5)]
      public string Instructions;
      private TerrainCursor m_cursor;
      private TerrainManager m_terrainManager;
      private IGameLoopEvents m_gameLoopEvents;
      private ISimLoopEvents m_simLoopEvents;
      private TerrainRenderer m_terrainRenderer;
      private bool m_isEnabled;
      private Tile3f? m_currentTile;
      private bool m_performPlacement;

      public void Initialize(
        TerrainCursor cursor,
        TerrainManager terrainManager,
        IGameLoopEvents gameLoopEvents,
        ISimLoopEvents simLoopEvents,
        TerrainRenderer terrainRenderer)
      {
        this.m_cursor = cursor;
        this.m_terrainManager = terrainManager;
        this.m_gameLoopEvents = gameLoopEvents;
        this.m_simLoopEvents = simLoopEvents;
        this.m_terrainRenderer = terrainRenderer;
        this.gameObject.SetActive(false);
        this.m_isEnabled = false;
        this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<TerrainDebugEditor.TerrainDebugEditorMb>(this, new Action<GameTime>(this.sync));
      }

      private void Update()
      {
        if (this.m_cursor == null)
          return;
        this.m_currentTile = this.m_cursor.HasValue ? new Tile3f?(this.m_cursor.Tile3f) : new Tile3f?();
        this.m_performPlacement = ((this.m_performPlacement ? 1 : 0) | (!Input.GetMouseButtonDown(0) ? 0 : (Input.GetKey(KeyCode.LeftControl) ? 1 : (this.InspectOnly ? 1 : 0)))) != 0;
      }

      private void sync(GameTime time)
      {
        if (this.gameObject.activeSelf == this.m_isEnabled)
          return;
        this.m_isEnabled = this.gameObject.activeSelf;
        if (this.gameObject.activeSelf)
        {
          this.m_cursor.Activate();
          this.AvailableMaterials = this.m_terrainManager.TerrainMaterials.AsEnumerable().Select<TerrainMaterialProto, string>((Func<TerrainMaterialProto, int, string>) ((x, i) => string.Format("[{0}] {1}", (object) i, (object) x.Id))).JoinStrings("\n");
          this.m_simLoopEvents.UpdateEndForUi.AddNonSaveable<TerrainDebugEditor.TerrainDebugEditorMb>(this, new Action(this.simUpdate));
        }
        else
        {
          this.m_cursor.Deactivate();
          this.m_simLoopEvents.UpdateEndForUi.RemoveNonSaveable<TerrainDebugEditor.TerrainDebugEditorMb>(this, new Action(this.simUpdate));
        }
      }

      private Option<TerrainMaterialProto> getSelectedMaterial()
      {
        ImmutableArray<TerrainMaterialProto> terrainMaterials;
        int? nullable1;
        if (this.SetMaterialIndex >= 0)
        {
          int setMaterialIndex = this.SetMaterialIndex;
          terrainMaterials = this.m_terrainManager.TerrainMaterials;
          int length = terrainMaterials.Length;
          if (setMaterialIndex < length)
          {
            nullable1 = new int?(this.SetMaterialIndex);
            goto label_4;
          }
        }
        nullable1 = new int?();
label_4:
        int? nullable2 = nullable1;
        if (!nullable2.HasValue)
          return (Option<TerrainMaterialProto>) (TerrainMaterialProto) null;
        terrainMaterials = this.m_terrainManager.TerrainMaterials;
        TerrainMaterialProto selectedMaterial = terrainMaterials[nullable2.Value];
        if (this.Disrupt && selectedMaterial.DisruptedMaterialProto.HasValue)
          selectedMaterial = selectedMaterial.DisruptedMaterialProto.Value;
        return (Option<TerrainMaterialProto>) selectedMaterial;
      }

      private void simUpdate()
      {
        if (!this.m_currentTile.HasValue || !this.m_performPlacement)
          return;
        this.m_performPlacement = false;
        Tile2i xy = this.m_currentTile.Value.Tile3i.Xy;
        this.CurrentTileInfo = this.m_terrainManager.Debug_ExplainTileContents(xy);
        if (this.InspectOnly)
        {
          Log.Warning(this.CurrentTileInfo);
        }
        else
        {
          int num = this.ToolRadius.Max(1);
          RectangleTerrainArea2i rectangleTerrainArea2i = new RectangleTerrainArea2i(xy.AddXy(-(num - 1)), 2 * new RelTile2i(num, num) - 1);
          if (this.ActivatePhysicsOnly)
          {
            foreach (Tile2iAndIndex enumerateTilesAndIndex in rectangleTerrainArea2i.EnumerateTilesAndIndices(this.m_terrainManager))
              this.m_terrainManager.StartTerrainPhysicsSimulationAt(enumerateTilesAndIndex);
          }
          else
          {
            ThicknessTilesF thicknessTilesF1 = this.SetMaterialDepthTiles.Max(1).TilesThick().ThicknessTilesF;
            ThicknessTilesF thicknessTilesF2 = new ThicknessTilesF(this.RelativeHeight);
            Option<TerrainMaterialProto> selectedMaterial = this.getSelectedMaterial();
            HeightTilesF? nullable = this.Flatten ? new HeightTilesF?(this.m_terrainManager[xy].Height) : new HeightTilesF?();
            if (selectedMaterial.HasValue)
            {
              thicknessTilesF2 -= thicknessTilesF1;
              this.InfoPlacedSlimId = (int) selectedMaterial.Value.SlimId.Value;
              this.InfoPlacedTextureIndex = (int) this.m_terrainRenderer.GetTextureIndex(selectedMaterial.Value.SlimId);
            }
            foreach (TerrainTile enumerateTile in rectangleTerrainArea2i.EnumerateTiles(this.m_terrainManager))
            {
              HeightTilesF heightTilesF = nullable ?? enumerateTile.Height;
              enumerateTile.TerrainManager.SetHeight(enumerateTile.CoordAndIndex, heightTilesF + thicknessTilesF2);
              if (selectedMaterial.HasValue)
                enumerateTile.TerrainManager.DumpMaterial_NoHeightChange(enumerateTile.CoordAndIndex, new TerrainMaterialThicknessSlim(selectedMaterial.Value, thicknessTilesF1));
            }
          }
        }
      }

      private void OnDrawGizmos()
      {
        if (!this.m_currentTile.HasValue)
          return;
        Tile3i tile3i = this.m_currentTile.Value.Tile3i;
        if (this.InspectOnly)
        {
          Gizmos.DrawWireCube(tile3i.CornerTile3f.ToVector3(), new RelTile3f(Fix32.One, Fix32.One, Fix32.Half).ToVector3());
        }
        else
        {
          float unityUnits = (2 * new RelTile1i(this.ToolRadius.Max(1)).RelTile1f - 1.0.Tiles()).ToUnityUnits();
          Gizmos.color = Color.green;
          Gizmos.DrawWireCube(tile3i.CornerTile3f.ToVector3(), new Vector3(unityUnits, 1f, unityUnits));
        }
      }

      public TerrainDebugEditorMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.SetMaterialIndex = -1;
        this.SetMaterialDepthTiles = 2;
        this.ToolRadius = 6;
        this.Instructions = "Choose material index (see available materials for reference), relative height (0 means no change in height), and depth.\nTo perform the operation hold CTRL + left click. Turn on Gizmos to see the radius.\nTip: if you don't set material index, only relative height will be used to terraform terrain without any new materials deposited.";
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
