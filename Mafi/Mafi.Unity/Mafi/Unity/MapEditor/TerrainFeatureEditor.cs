// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.TerrainFeatureEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Unity.MapEditor.Previews;
using Mafi.Unity.Terrain;
using Mafi.Unity.UserInterface;
using RTG;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class TerrainFeatureEditor
  {
    private static readonly Fix32 HANDLE_HEIGHT;
    private static readonly int MAIN_TEX_SHADER_ID;
    private static readonly int COLOR_SHADER_ID;
    private static readonly int ICON_COLOR_SHADER_ID;
    private static readonly int SELECTED_COLOR_SHADER_ID;
    public readonly IEditableTerrainFeature TerrainFeature;
    private readonly PointPreviewRenderer m_pointPreviewRenderer;
    private readonly Mafi.Unity.MapEditor.MapEditor m_mapEditor;
    private RectangleTerrainArea2i m_boundingBoxTiles;
    private readonly Area2iRenderer m_boundingBoxRenderer;
    private Option<GameObject> m_handleGo;
    private readonly Option<TerrainCircleRenderer> m_boundingRadiusRenderer;
    private ObjectTransformGizmo m_transformGizmo;
    private readonly Dict<Chunk2i, GameObject> m_heightPreviews;
    private readonly Dict<Chunk2i, GameObject> m_heightTopBottomPreviews;
    private readonly AssetsDb m_assetsDb;
    private MeshRenderer m_meshRenderer;
    private bool m_handleDragged;
    private bool m_isActive;

    public TerrainFeatureEditor(
      IEditableTerrainFeature terrainFeature,
      LinesFactory linesFactory,
      PointPreviewRenderer pointPreviewRenderer,
      Mafi.Unity.MapEditor.MapEditor mapEditor,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_heightPreviews = new Dict<Chunk2i, GameObject>();
      this.m_heightTopBottomPreviews = new Dict<Chunk2i, GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainFeature = terrainFeature;
      this.m_pointPreviewRenderer = pointPreviewRenderer;
      this.m_mapEditor = mapEditor;
      this.m_assetsDb = assetsDb;
      RectangleTerrainArea2i? boundingBox = terrainFeature.GetBoundingBox();
      this.m_boundingBoxRenderer = new Area2iRenderer(linesFactory);
      this.m_boundingBoxRenderer.SetColor(new Color(0.2f, 1f, 0.2f));
      this.m_boundingBoxRenderer.SetWidth(0.5f);
      this.m_boundingBoxRenderer.Hide();
      HandleData? handleData = terrainFeature.GetHandleData();
      if (!handleData.HasValue)
        return;
      if (boundingBox.HasValue)
      {
        this.m_boundingBoxRenderer.SetArea(this.m_boundingBoxTiles, mapEditor.TerrainManager);
        this.m_boundingBoxTiles = boundingBox.Value;
      }
      if (!(terrainFeature is IEditableTerrainFeatureWithDisplayedRadius withDisplayedRadius))
        return;
      RelTile1i radius = withDisplayedRadius.Radius;
      TerrainCircleRenderer terrainCircleRenderer = new TerrainCircleRenderer(linesFactory);
      terrainCircleRenderer.SetColor(new Color(0.2f, 1f, 0.2f));
      terrainCircleRenderer.SetWidth(0.5f);
      HeightTilesF height = mapEditor.TerrainManager.IsOffLimitsOrInvalid(handleData.Value.Position.Tile2i) ? HeightTilesF.Zero : mapEditor.TerrainManager.GetHeight(handleData.Value.Position.Tile2i);
      terrainCircleRenderer.SetCircle(handleData.Value.Position, radius, height);
      terrainCircleRenderer.Hide();
      this.m_boundingRadiusRenderer = (Option<TerrainCircleRenderer>) terrainCircleRenderer;
    }

    public bool TryGetHandleObject(Mafi.Unity.MapEditor.MapEditor editor, out GameObject handleGo)
    {
      if (this.m_handleGo.HasValue)
      {
        handleGo = this.m_handleGo.Value;
        return true;
      }
      if (!this.TerrainFeature.GetHandleData().HasValue)
      {
        handleGo = (GameObject) null;
        return false;
      }
      handleGo = editor.AssetsDb.GetClonedPrefabOrEmptyGo("Assets/Unity/MapEditor/IconPinBillboard.prefab");
      this.m_handleGo = (Option<GameObject>) handleGo;
      handleGo.name = "Editor handle: " + this.TerrainFeature.Name;
      this.m_meshRenderer = handleGo.GetComponent<MeshRenderer>();
      this.m_meshRenderer.sharedMaterial = this.m_meshRenderer.material;
      this.m_meshRenderer.sharedMaterial.renderQueue = 3051;
      this.m_transformGizmo = MonoSingleton<RTGizmosEngine>.Get.CreateObjectUniversalGizmo();
      this.m_transformGizmo.SetCanAffectScale(false);
      this.m_transformGizmo.SetCanAffectRotation(false);
      this.m_transformGizmo.Gizmo.SetEnabled(false);
      for (int axisIndex = 0; axisIndex < 3; ++axisIndex)
      {
        this.m_transformGizmo.Gizmo.UniversalGizmo.SharedLookAndFeel3D = (UniversalGizmoLookAndFeel3D) null;
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScNegativeSliderVisible(axisIndex, false);
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScPositiveSliderVisible(axisIndex, false);
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScSliderVisible(axisIndex, AxisSign.Positive, false);
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScSliderCapVisible(axisIndex, AxisSign.Positive, false);
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScSliderVisible(axisIndex, AxisSign.Negative, false);
        this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScSliderCapVisible(axisIndex, AxisSign.Negative, false);
      }
      this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScMidCapVisible(false);
      this.UpdateHandle();
      return true;
    }

    public void Activate()
    {
      if (this.m_isActive)
        return;
      this.m_isActive = true;
      this.m_boundingBoxRenderer.Show();
      if (this.m_boundingRadiusRenderer.HasValue)
        this.m_boundingRadiusRenderer.Value.Show();
      if (this.TerrainFeature is ITerrainFeatureWithPreview terrainFeature2)
        terrainFeature2.InitializePreview(this.m_mapEditor.Resolver);
      else if (this.TerrainFeature is IPostProcessorWithPreview terrainFeature1)
        terrainFeature1.InitializePreview(this.m_mapEditor.Resolver);
      if (this.m_handleGo.HasValue)
      {
        this.m_transformGizmo.Gizmo.SetEnabled(true);
        this.m_transformGizmo.SetTargetObject(this.m_handleGo.Value);
        ObjectTransformGizmo.ObjectRestrictions restrictions = new ObjectTransformGizmo.ObjectRestrictions();
        if (this.TerrainFeature.Is2D)
        {
          restrictions.SetCanMoveAlongAxis(1, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.SharedLookAndFeel3D = (UniversalGizmoLookAndFeel3D) null;
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetMvSliderVisible(1, AxisSign.Positive, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetMvSliderCapVisible(1, AxisSign.Positive, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetMvDblSliderVisible(PlaneId.XY, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetMvDblSliderVisible(PlaneId.YZ, false);
        }
        if (this.TerrainFeature.CanRotate)
        {
          this.m_transformGizmo.SetCanAffectRotation(true);
          this.m_transformGizmo.Gizmo.UniversalGizmo.SharedLookAndFeel3D = (UniversalGizmoLookAndFeel3D) null;
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtAxisVisible(0, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtAxisVisible(2, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.SharedSettings3D = (UniversalGizmoSettings3D) null;
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtCamLookSliderVisible(false);
        }
        else
        {
          this.m_transformGizmo.SetCanAffectRotation(false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.SharedLookAndFeel3D = (UniversalGizmoLookAndFeel3D) null;
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtAxisVisible(0, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtAxisVisible(1, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtAxisVisible(2, false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtMidCapVisible(false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtRotationArcVisible(false);
          this.m_transformGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetRtCamLookSliderVisible(false);
        }
        this.m_transformGizmo.RegisterObjectRestrictions(this.m_handleGo.Value, restrictions);
        this.m_handleGo.Value.AddComponent<TerrainFeatureEditor.TransformGizmoListener>().SetParent(this);
        this.m_transformGizmo.Gizmo.PostDragBegin += new GizmoPostDragBeginHandler(this.gizmoOnPostDragBegin);
        this.m_transformGizmo.Gizmo.PostDragEnd += new GizmoPostDragEndHandler(this.gizmoOnPostDragEnd);
      }
      this.UpdateHandle();
    }

    public void Deactivate()
    {
      if (!this.m_isActive)
        return;
      this.m_isActive = false;
      this.ClearPreview();
      this.m_boundingBoxRenderer.Hide();
      if (this.m_boundingRadiusRenderer.HasValue)
        this.m_boundingRadiusRenderer.Value.Hide();
      if (this.m_handleGo.HasValue)
      {
        this.m_transformGizmo.Gizmo.SetEnabled(false);
        this.m_transformGizmo.SetTargetObjects((IEnumerable<GameObject>) null);
        this.m_transformGizmo.Gizmo.PostDragBegin -= new GizmoPostDragBeginHandler(this.gizmoOnPostDragBegin);
        this.m_transformGizmo.Gizmo.PostDragEnd -= new GizmoPostDragEndHandler(this.gizmoOnPostDragEnd);
        Object.Destroy((Object) this.m_handleGo.Value.GetComponent<TerrainFeatureEditor.TransformGizmoListener>());
      }
      this.UpdateHandle();
    }

    public void UpdateHandle()
    {
      HandleData? handleData1 = this.TerrainFeature.GetHandleData();
      if (!handleData1.HasValue)
        return;
      if (this.m_handleGo.HasValue)
      {
        HandleData handleData2 = handleData1.Value;
        if (handleData2.Height.HasValue)
        {
          Transform transform = this.m_handleGo.Value.transform;
          Tile2f position = handleData1.Value.Position;
          ref Tile2f local = ref position;
          handleData2 = handleData1.Value;
          HeightTilesF height = handleData2.Height.Value;
          Vector3 vector3 = local.ExtendHeight(height).AddZ(TerrainFeatureEditor.HANDLE_HEIGHT).ToVector3();
          transform.localPosition = vector3;
        }
        else
          this.m_handleGo.Value.transform.localPosition = this.m_mapEditor.TerrainManager.ExtendHeightOrDefault(handleData1.Value.Position).AddZ(TerrainFeatureEditor.HANDLE_HEIGHT).ToVector3();
        this.m_transformGizmo.RefreshPosition();
        handleData2 = handleData1.Value;
        if (handleData2.IconAssetPath.HasValue)
          this.m_meshRenderer.sharedMaterial.SetTexture(TerrainFeatureEditor.MAIN_TEX_SHADER_ID, (Texture) this.m_assetsDb.GetSharedTexture(handleData1.Value.IconAssetPath.Value));
        Material sharedMaterial = this.m_handleGo.Value.GetComponent<MeshRenderer>()?.sharedMaterial;
        if ((Object) sharedMaterial != (Object) null)
        {
          sharedMaterial.SetColor(TerrainFeatureEditor.COLOR_SHADER_ID, handleData1.Value.Color.ToColor());
          sharedMaterial.SetColor(TerrainFeatureEditor.ICON_COLOR_SHADER_ID, handleData1.Value.IconColor.ToColor());
          sharedMaterial.SetColor(TerrainFeatureEditor.SELECTED_COLOR_SHADER_ID, this.m_isActive ? Color.green : Color.clear);
        }
      }
      if (!this.m_boundingRadiusRenderer.HasValue || !(this.TerrainFeature is IEditableTerrainFeatureWithDisplayedRadius terrainFeature))
        return;
      TerrainManager terrainManager1 = this.m_mapEditor.TerrainManager;
      Tile2f position1 = handleData1.Value.Position;
      Tile2i tile2i1 = position1.Tile2i;
      HeightTilesF heightTilesF;
      if (!terrainManager1.IsOffLimitsOrInvalid(tile2i1))
      {
        TerrainManager terrainManager2 = this.m_mapEditor.TerrainManager;
        position1 = handleData1.Value.Position;
        Tile2i tile2i2 = position1.Tile2i;
        heightTilesF = terrainManager2.GetHeight(tile2i2);
      }
      else
        heightTilesF = HeightTilesF.Zero;
      HeightTilesF height1 = heightTilesF;
      this.m_boundingRadiusRenderer.Value.SetCircle(handleData1.Value.Position, terrainFeature.Radius, height1);
    }

    public void UpdatePreview(
      TerrainManager terrainManager,
      IIndexable<ITerrainPostProcessorV2> terrainPostProcessors,
      int timeBudgetMs)
    {
      RectangleTerrainArea2i? boundingBox = this.TerrainFeature.GetBoundingBox();
      if (!boundingBox.HasValue)
        return;
      foreach (GameObject gameObject in this.m_heightPreviews.Values)
        gameObject.SetActive(false);
      foreach (GameObject gameObject in this.m_heightTopBottomPreviews.Values)
        gameObject.SetActive(false);
      IEnumerable<ITerrainFeaturePreview> previews;
      if (this.TerrainFeature is ITerrainFeatureWithPreview terrainFeature1)
      {
        if (!terrainFeature1.TryGetPreviews(this.m_mapEditor.Resolver, timeBudgetMs, out previews))
          return;
      }
      else
      {
        if (!(this.TerrainFeature is IPostProcessorWithPreview terrainFeature))
          return;
        foreach (ITerrainPostProcessorV2 terrainPostProcessor in terrainPostProcessors)
        {
          if (terrainPostProcessor is IPostProcessorWithPreview processorWithPreview)
            processorWithPreview.InitializePreview(this.m_mapEditor.Resolver);
        }
        if (!terrainFeature.TryGetPreviewsWithContextFromPrevious(this.m_mapEditor.Resolver, terrainPostProcessors, timeBudgetMs, out previews))
          return;
      }
      this.m_pointPreviewRenderer.Clear();
      foreach (ITerrainFeaturePreview terrainFeaturePreview in previews)
      {
        if (boundingBox.Value.OverlapsWith(terrainFeaturePreview.Chunk.Area))
        {
          switch (terrainFeaturePreview)
          {
            case HeightmapFeaturePreviewChunkData data1:
              GameObject gameObject1;
              HeightmapFeaturePreviewMb featurePreviewMb1;
              if (!this.m_heightPreviews.TryGetValue(terrainFeaturePreview.Chunk, out gameObject1))
              {
                if (data1.Dirty)
                {
                  gameObject1 = new GameObject("Heightmap preview");
                  featurePreviewMb1 = gameObject1.AddComponent<HeightmapFeaturePreviewMb>();
                  featurePreviewMb1.Initialize(this.m_mapEditor.AssetsDb);
                  this.m_heightPreviews.Add(terrainFeaturePreview.Chunk, gameObject1);
                }
                else
                  continue;
              }
              else
                featurePreviewMb1 = gameObject1.GetComponent<HeightmapFeaturePreviewMb>();
              if ((Object) featurePreviewMb1 == (Object) null)
              {
                Log.Error("Heightmap preview mb unexpectedly null");
                continue;
              }
              if (data1.Dirty)
                featurePreviewMb1.UpdateData(data1);
              gameObject1.SetActive(true);
              continue;
            case HeightmapTopBottomPreviewChunkData data2:
              GameObject gameObject2;
              HeightmapTopBottomFeaturePreviewMb featurePreviewMb2;
              if (!this.m_heightTopBottomPreviews.TryGetValue(terrainFeaturePreview.Chunk, out gameObject2))
              {
                gameObject2 = new GameObject("Heightmap2 preview");
                featurePreviewMb2 = gameObject2.AddComponent<HeightmapTopBottomFeaturePreviewMb>();
                featurePreviewMb2.Initialize(this.m_mapEditor.AssetsDb);
                this.m_heightTopBottomPreviews.Add(terrainFeaturePreview.Chunk, gameObject2);
              }
              else
                featurePreviewMb2 = gameObject2.GetComponent<HeightmapTopBottomFeaturePreviewMb>();
              if ((Object) featurePreviewMb2 == (Object) null)
              {
                Log.Error("Heightmap2 preview mb unexpectedly null");
                continue;
              }
              if (data2.Dirty)
                featurePreviewMb2.UpdateData(data2);
              gameObject2.SetActive(true);
              continue;
            case PointFeaturePreviewChunkData previewChunkData:
              using (IEnumerator<Tile2iSlim> enumerator = previewChunkData.Points.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Tile2iSlim current = enumerator.Current;
                  int num = (int) this.m_pointPreviewRenderer.AddPreview(terrainManager.ExtendHeight(current.AsFull).Tile3i);
                }
                continue;
              }
            default:
              continue;
          }
        }
      }
    }

    public void ScaleHandle()
    {
      if (this.m_handleGo.IsNone)
        return;
      float num = ((float) (5.0 + ((double) (Vector3.Distance(this.m_mapEditor.CameraController.EyePosition, this.m_handleGo.Value.transform.position) * 0.1f).Max(5f) - 5.0) * 0.5)).Min(50f) * (1024f / (float) this.m_mapEditor.CameraController.Camera.pixelHeight) * UiScaleHelper.GetCurrentScaleFloat();
      this.m_handleGo.Value.transform.localScale = new Vector3(num, num, num);
    }

    public void ClearPreview()
    {
      foreach (Object @object in this.m_heightPreviews.Values)
        Object.Destroy(@object);
      foreach (Object @object in this.m_heightTopBottomPreviews.Values)
        Object.Destroy(@object);
      this.m_heightPreviews.Clear();
      this.m_heightTopBottomPreviews.Clear();
      this.m_pointPreviewRenderer.Clear();
    }

    /// <summary>
    /// Returns the actual bounding box that should be updated.
    /// When null is returned, this feature update had no effect on the terrain.
    /// </summary>
    public RectangleTerrainArea2i? NotifyFeatureUpdated(bool onlyUpdateEditor = false)
    {
      this.UpdateHandle();
      if (!onlyUpdateEditor)
      {
        this.TerrainFeature.Reset();
        this.TerrainFeature.ClearCaches();
      }
      RectangleTerrainArea2i? boundingBox = this.TerrainFeature.GetBoundingBox();
      if (!boundingBox.HasValue)
        return new RectangleTerrainArea2i?(this.m_mapEditor.TerrainManager.TerrainArea);
      if (boundingBox.Value.IsEmpty)
        return new RectangleTerrainArea2i?();
      RectangleTerrainArea2i rectangleTerrainArea2i = this.m_boundingBoxTiles.Union(boundingBox.Value);
      this.m_boundingBoxTiles = boundingBox.Value;
      this.m_boundingBoxRenderer.SetArea(boundingBox.Value, this.m_mapEditor.TerrainManager);
      return new RectangleTerrainArea2i?(rectangleTerrainArea2i);
    }

    private void gizmoOnPostDragBegin(Gizmo gizmo, int handleId) => this.m_handleDragged = false;

    private void gizmoOnPostDragEnd(Gizmo gizmo, int handleId)
    {
      if (!this.m_handleDragged)
        return;
      this.m_mapEditor.NotifySelectedFeatureWasUpdated((Option<string>) ("Moved: " + this.TerrainFeature.Name), this.NotifyFeatureUpdated());
    }

    public void Destroy()
    {
      this.Deactivate();
      this.m_boundingBoxRenderer.Destroy();
      if (this.m_handleGo.HasValue)
        this.m_handleGo.Value.Destroy();
      if (!this.m_boundingRadiusRenderer.HasValue)
        return;
      this.m_boundingRadiusRenderer.Value.Destroy();
    }

    static TerrainFeatureEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TerrainFeatureEditor.HANDLE_HEIGHT = (Fix32) 2;
      TerrainFeatureEditor.MAIN_TEX_SHADER_ID = Shader.PropertyToID("_MainTex");
      TerrainFeatureEditor.COLOR_SHADER_ID = Shader.PropertyToID("_Color");
      TerrainFeatureEditor.ICON_COLOR_SHADER_ID = Shader.PropertyToID("_IconOverlayColor");
      TerrainFeatureEditor.SELECTED_COLOR_SHADER_ID = Shader.PropertyToID("_SelectedColor");
    }

    public class TransformGizmoListener : MonoBehaviour, IRTTransformGizmoListener
    {
      private TerrainFeatureEditor m_parent;

      public void SetParent(TerrainFeatureEditor parent) => this.m_parent = parent;

      public bool OnCanBeTransformed(Gizmo transformGizmo)
      {
        return !this.m_parent.m_mapEditor.CameraController.IsInFreeLookMode;
      }

      public void OnTransformed(Gizmo transformGizmo)
      {
        RelTile3f relTile3f = transformGizmo.RelativeDragOffset.ToRelTile3f();
        AngleDegrees1f rotation = -transformGizmo.RelativeDragRotation.eulerAngles.y.Degrees();
        this.m_parent.TerrainFeature.TranslateBy(relTile3f);
        this.m_parent.TerrainFeature.RotateBy(rotation);
        if (relTile3f.IsZero && rotation.IsZero)
          return;
        this.m_parent.m_mapEditor.NotifySelectedFeatureWasUpdated(Option<string>.None, new RectangleTerrainArea2i?());
        this.m_parent.m_handleDragged = true;
      }

      public TransformGizmoListener()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
