// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TreeMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.InstancedRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class TreeMb : MonoBehaviour
  {
    private static readonly int TINT_COLOR_SHADER_ID;
    private TreeId m_treeId;
    private TerrainTile m_tile;
    private Tile2f m_position;
    private float m_targetScale;
    private ITreesManager m_treeManager;
    private IGameLoopEvents m_gameLoopEvents;
    private ICalendar m_calendar;
    private Option<BoxCollider> m_collider;
    private bool m_destroyed;
    private float m_collapseTimer;
    private float m_angularVeloctiy;
    private Option<KeyValuePair<Renderer, Material>[]> m_originalMaterials;

    public TreeId TreeId => this.m_treeId;

    public bool IsBeingHarvested { get; private set; }

    public bool IsCollapsing { get; private set; }

    public string CutTreePrefabPath { get; private set; }

    public GameObject Lod0GameObject { get; private set; }

    public bool IsPreview { get; private set; }

    public void Initialize(
      TreeData tree,
      TerrainTile tile,
      ITreesManager treeManager,
      IGameLoopEvents gameLoopEvents,
      ICalendar calendar,
      string cutTreePrefabPath)
    {
      Assert.That<Tile2iSlim>(tree.Id.Position).IsEqualTo<Tile2iSlim>(tile.TileCoordSlim);
      this.m_tile = tile;
      this.m_treeId = tree.Id;
      this.m_position = tree.Position.Tile2i.CenterTile2f;
      this.m_treeManager = treeManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_calendar = calendar;
      this.CutTreePrefabPath = cutTreePrefabPath;
      Percent scale = tree.GetScale(this.m_calendar);
      this.m_targetScale = (Percent.FromRatio(1, 10) + scale * Percent.FromRatio(9, 10)).ToFloat();
      this.transform.rotation = Quaternion.AngleAxis(tree.Rotation.ToUnityAngleDegrees(), Vector3.up);
      this.transform.localScale = Vector3.one * this.m_targetScale;
      this.transform.position = this.m_position.ExtendHeight(this.m_tile.Height).ToVector3();
      this.Lod0GameObject = this.gameObject.GetFirstChildOrDefault((Predicate<GameObject>) (c => (UnityEngine.Object) c.gameObject.GetComponent<MeshRenderer>() != (UnityEngine.Object) null));
    }

    public void SetAsPreview()
    {
      this.IsPreview = true;
      this.transform.localScale = Vector3.one;
      this.m_collider = (Option<BoxCollider>) this.gameObject.GetComponent<BoxCollider>();
      if (!this.m_collider.IsNone)
        return;
      this.m_collider = (Option<BoxCollider>) this.gameObject.AddComponent<BoxCollider>();
      this.m_collider.Value.center = this.gameObject.ComputeMaxBounds().center - this.transform.position;
      this.m_collider.Value.size = this.gameObject.ComputeMaxBounds().size;
    }

    public void EnsureBlueprintMaterial(Material material, ColorRgba color)
    {
      if (!(bool) (UnityEngine.Object) material)
      {
        Log.Error("Trying to set invalid material.");
      }
      else
      {
        if (this.m_originalMaterials.IsNone)
          this.m_originalMaterials = (Option<KeyValuePair<Renderer, Material>[]>) ((IEnumerable<Renderer>) this.gameObject.GetComponentsInChildren<Renderer>()).Where<Renderer>((Func<Renderer, bool>) (x => !x.gameObject.HasTag(UnityTag.NoHi))).Where<Renderer>((Func<Renderer, bool>) (x => x.gameObject.layer == Layer.Unity00Default.ToId())).Select<Renderer, KeyValuePair<Renderer, Material>>((Func<Renderer, KeyValuePair<Renderer, Material>>) (r => Make.Kvp<Renderer, Material>(r, r.sharedMaterial))).ToArray<KeyValuePair<Renderer, Material>>();
        Lyst<KeyValuePair<Material, Material>> list = new Lyst<KeyValuePair<Material, Material>>();
        foreach (KeyValuePair<Renderer, Material> keyValuePair in this.m_originalMaterials.Value)
        {
          if ((bool) (UnityEngine.Object) keyValuePair.Key)
          {
            Material material1;
            if (!list.TryGetValue<Material, Material>(keyValuePair.Key.sharedMaterial, out material1))
            {
              material1 = InstancingUtils.InstantiateMaterialAndCopyTextures(material, keyValuePair.Key.sharedMaterial, true);
              material1.SetColor(TreeMb.TINT_COLOR_SHADER_ID, color.ToColor());
              list.Add<Material, Material>(keyValuePair.Key.sharedMaterial, material1);
            }
            keyValuePair.Key.sharedMaterial = material1;
          }
        }
      }
    }

    public void SyncUpdate(GameTime time)
    {
      TreeData treeData;
      if (!this.m_treeManager.Trees.TryGetValue(this.m_treeId, out treeData))
        return;
      Percent scale = treeData.GetScale(this.m_calendar);
      Percent percent = Percent.FromRatio(1, 10) + scale * Percent.FromRatio(9, 10);
      this.m_targetScale = percent.ToFloat();
      this.transform.localScale = Vector3.one * percent.ToFloat();
    }

    public void RenderUpdateCollapsing(GameTime time)
    {
      if (this.m_destroyed)
        return;
      float num = (float) ((double) time.GameSpeedMult * (double) time.DeltaTimeMs / 1000.0);
      this.m_collapseTimer += num;
      foreach (Renderer componentsInChild in this.GetComponentsInChildren<MeshRenderer>())
      {
        Material material = componentsInChild.material;
        if ((UnityEngine.Object) material != (UnityEngine.Object) null)
        {
          Color color = material.color with
          {
            a = ((float) (1.0 - (double) this.m_collapseTimer / 10.0)).Max(0.0f)
          };
          if ((double) color.a == 0.0)
          {
            this.gameObject.Destroy();
            return;
          }
          material.color = color;
        }
      }
      this.m_angularVeloctiy += 360f / 10f.Squared() * num;
      this.transform.Rotate(this.m_angularVeloctiy * num, 0.0f, 0.0f);
    }

    public void TriggerCollapse() => this.IsCollapsing = true;

    /// <summary>
    /// Sets the tree as being harvested, TreeMb that is being harvested is not going to be destroyed by <see cref="T:Mafi.Unity.Terrain.TreeRenderer" />.
    /// </summary>
    public void BeingHarvested()
    {
      Assert.That<bool>(this.IsBeingHarvested).IsFalse();
      this.IsBeingHarvested = true;
    }

    public void OnDestroy()
    {
      Assert.That<bool>(this.m_destroyed).IsFalse();
      this.m_destroyed = true;
    }

    public TreeMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TreeMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreeMb.TINT_COLOR_SHADER_ID = Shader.PropertyToID("_TintColor");
    }
  }
}
