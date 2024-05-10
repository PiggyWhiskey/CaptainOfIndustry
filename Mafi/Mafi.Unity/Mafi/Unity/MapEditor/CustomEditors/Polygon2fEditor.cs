// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.CustomEditors.Polygon2fEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Terrain;
using Mafi.Numerics;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using Mafi.Unity.UserInterface;
using RTG;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.CustomEditors
{
  public class Polygon2fEditor : ICustomObjEditor
  {
    public static readonly Color LINE_COLOR_DEFAULT;
    public static readonly Color VERTEX_COLOR_ADD;
    private static readonly string HANDLE_MATERIAL;
    private readonly TerrainManager m_terrainManager;
    private readonly CameraController m_cameraController;
    private readonly LinesFactory m_linesFactory;
    private readonly CursorPickingManager m_pickingManager;
    private readonly AssetsDb m_assetsDb;
    private readonly Lyst<GameObject> m_vertexEditHandles;
    private readonly Lyst<LineWithColliderMb> m_edgesLines;
    private readonly Lyst<GameObject> m_edgeLineSplitters;
    private readonly Dict<GameObject, int> m_colliders;
    private readonly Dict<GameObject, ObjectTransformGizmo> m_transformGizmos;
    private Option<Polygon2fMutable> m_polygon;
    private Option<Func<Tile2f, HeightTilesF>> m_customHeightFn;
    private readonly Cursoor m_moveCursor;
    private bool m_handleDragged;
    private bool m_shouldUpdateDueToHandleCreation;

    public event Action<bool> OnChange;

    public Polygon2fEditor(
      TerrainManager terrainManager,
      CameraController cameraController,
      LinesFactory linesFactory,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      UiBuilder uiBuilder,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_vertexEditHandles = new Lyst<GameObject>();
      this.m_edgesLines = new Lyst<LineWithColliderMb>();
      this.m_edgeLineSplitters = new Lyst<GameObject>();
      this.m_colliders = new Dict<GameObject, int>();
      this.m_transformGizmos = new Dict<GameObject, ObjectTransformGizmo>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_cameraController = cameraController;
      this.m_linesFactory = linesFactory;
      this.m_pickingManager = cursorPickingManager;
      this.m_assetsDb = assetsDb;
      this.m_moveCursor = cursorManager.RegisterCursor(uiBuilder.Style.Cursors.Move);
      this.updatePolygon();
    }

    public void StartEdit(object o)
    {
      if (!(o is Polygon2fMutable polygon2fMutable))
      {
        Log.Error("Invalid object to edit.");
      }
      else
      {
        this.m_polygon = (Option<Polygon2fMutable>) polygon2fMutable;
        this.updatePolygon();
      }
    }

    public void ButtonPressed()
    {
      if (!this.m_polygon.HasValue)
        return;
      this.m_cameraController.PanTo(new Tile2f(this.m_polygon.Value.GetVerticesAveragePosition()));
    }

    public void Destroy()
    {
      this.m_polygon = Option<Polygon2fMutable>.None;
      foreach (Component edgesLine in this.m_edgesLines)
        edgesLine.gameObject.Destroy();
      this.m_edgesLines.Clear();
      foreach (GameObject edgeLineSplitter in this.m_edgeLineSplitters)
        edgeLineSplitter.Destroy();
      this.m_edgeLineSplitters.Clear();
      foreach (GameObject vertexEditHandle in this.m_vertexEditHandles)
        vertexEditHandle.Destroy();
      this.m_vertexEditHandles.Clear();
      this.m_colliders.Clear();
      this.m_moveCursor.SetTemporaryVisibility(false);
      foreach (KeyValuePair<GameObject, ObjectTransformGizmo> transformGizmo in this.m_transformGizmos)
        this.removeGizmo(transformGizmo.Key, transformGizmo.Value);
      this.m_transformGizmos.Clear();
    }

    private void addGizmo(GameObject go, ObjectTransformGizmo gizmo)
    {
      gizmo.Gizmo.SetEnabled(true);
      gizmo.SetTargetObject(go);
      gizmo.Gizmo.PostDragBegin += new GizmoPostDragBeginHandler(this.postDragBegin);
      gizmo.Gizmo.PostDragEnd += new GizmoPostDragEndHandler(this.postDragEnd);
      new ObjectTransformGizmo.ObjectRestrictions().SetCanMoveAlongAxis(1, false);
      gizmo.Gizmo.MoveGizmo.SharedLookAndFeel3D = (MoveGizmoLookAndFeel3D) null;
      gizmo.Gizmo.MoveGizmo.LookAndFeel3D.SetSliderVisible(1, AxisSign.Positive, false);
      gizmo.Gizmo.MoveGizmo.LookAndFeel3D.SetSliderCapVisible(1, AxisSign.Positive, false);
      gizmo.Gizmo.MoveGizmo.LookAndFeel3D.SetDblSliderVisible(PlaneId.XY, false);
      gizmo.Gizmo.MoveGizmo.LookAndFeel3D.SetDblSliderVisible(PlaneId.YZ, false);
      go.AddComponent<Polygon2fEditor.TransformGizmoListener>().SetParent(this);
    }

    private void removeGizmo(GameObject go, ObjectTransformGizmo gizmo)
    {
      gizmo.Gizmo.SetEnabled(false);
      gizmo.Gizmo.PostDragBegin -= new GizmoPostDragBeginHandler(this.postDragBegin);
      gizmo.Gizmo.PostDragEnd -= new GizmoPostDragEndHandler(this.postDragEnd);
      gizmo.SetTargetObjects((IEnumerable<GameObject>) null);
      UnityEngine.Object.Destroy((UnityEngine.Object) go.GetComponent<Polygon2fEditor.TransformGizmoListener>());
    }

    public void ValuesUpdated() => this.updatePolygon();

    private void updatePolygon(bool skipHandles = false)
    {
      if (this.m_polygon.IsNone)
        return;
      Polygon2fMutable polygon = this.m_polygon.Value;
      int count1 = this.m_edgesLines.Count;
      ReadOnlyArraySlice<Vector2f> vertices = polygon.Vertices;
      int length1 = vertices.Length;
      if (count1 != length1)
      {
        int count2 = this.m_edgesLines.Count;
        while (true)
        {
          int num = count2;
          vertices = polygon.Vertices;
          int length2 = vertices.Length;
          if (num < length2)
          {
            this.m_edgesLines.Add(createLine());
            GameObject splitterHandle = createSplitterHandle();
            this.m_edgeLineSplitters.Add(splitterHandle);
            this.m_colliders.Add(splitterHandle, -count2 - 1);
            GameObject handle = createHandle();
            ObjectTransformGizmo objectMoveGizmo;
            if (!this.m_transformGizmos.TryGetValue(handle, out objectMoveGizmo))
              objectMoveGizmo = MonoSingleton<RTGizmosEngine>.Get.CreateObjectMoveGizmo();
            this.addGizmo(handle, objectMoveGizmo);
            this.m_vertexEditHandles.Add(handle);
            this.m_transformGizmos.Add(handle, objectMoveGizmo);
            this.m_colliders.Add(handle, count2);
            ++count2;
          }
          else
            break;
        }
        int count3 = this.m_vertexEditHandles.Count;
        vertices = polygon.Vertices;
        int length3 = vertices.Length;
        int num1 = count3 - length3;
        for (int index = 0; index < num1; ++index)
        {
          GameObject gameObject1 = this.m_edgesLines.PopLast().gameObject;
          GameObject gameObject2 = this.m_edgeLineSplitters.PopLast().gameObject;
          this.m_colliders.Remove(gameObject2);
          gameObject1.Destroy();
          gameObject2.Destroy();
          GameObject gameObject3 = this.m_vertexEditHandles.PopLast();
          gameObject3.Destroy();
          this.m_colliders.Remove(gameObject3);
          ObjectTransformGizmo gizmo;
          if (this.m_transformGizmos.TryGetValue(gameObject3, out gizmo))
          {
            this.removeGizmo(gameObject3, gizmo);
            this.m_transformGizmos.Remove(gameObject3);
          }
        }
        skipHandles = false;
      }
      vertices = polygon.Vertices;
      Vector3 p1 = getPoint(vertices.Length - 1);
      int num2 = 0;
      while (true)
      {
        int num3 = num2;
        vertices = polygon.Vertices;
        int length4 = vertices.Length;
        if (num3 < length4)
        {
          Vector3 point = getPoint(num2);
          this.m_edgesLines[num2].SetPoints(p1, point);
          this.m_edgeLineSplitters[num2].transform.localPosition = (p1 + point) / 2f;
          p1 = point;
          if (!skipHandles)
          {
            this.m_vertexEditHandles[num2].transform.localPosition = point;
            ObjectTransformGizmo objectTransformGizmo;
            if (this.m_transformGizmos.TryGetValue(this.m_vertexEditHandles[num2], out objectTransformGizmo))
              objectTransformGizmo.RefreshPosition();
          }
          ++num2;
        }
        else
          break;
      }

      LineWithColliderMb createLine()
      {
        LineWithColliderMb line = new GameObject("PolygonLine").AddComponent<LineWithColliderMb>();
        line.Initialize(this.m_linesFactory.DefaultSharedMaterial, 4f);
        line.SetWidth(1f);
        line.SetColor(Polygon2fEditor.LINE_COLOR_DEFAULT);
        return line;
      }

      GameObject createSplitterHandle()
      {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        CapsuleCollider capsuleCollider = primitive.AddComponent<CapsuleCollider>();
        capsuleCollider.height = 1f;
        capsuleCollider.radius = 1f;
        primitive.transform.localScale = new Vector3(6f, 1f, 6f);
        primitive.GetComponent<Renderer>().material = this.m_assetsDb.GetSharedMaterial(Polygon2fEditor.HANDLE_MATERIAL);
        primitive.GetComponent<Renderer>().material.color = Polygon2fEditor.VERTEX_COLOR_ADD;
        return primitive;
      }

      GameObject createHandle()
      {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        CapsuleCollider capsuleCollider = primitive.AddComponent<CapsuleCollider>();
        capsuleCollider.height = 1f;
        capsuleCollider.radius = 1f;
        primitive.transform.localScale = new Vector3(6f, 1f, 6f);
        primitive.GetComponent<Renderer>().material = this.m_assetsDb.GetSharedMaterial(Polygon2fEditor.HANDLE_MATERIAL);
        return primitive;
      }

      Vector3 getPoint(int i)
      {
        Tile2f coord = new Tile2f(polygon.Vertices[i]);
        HeightTilesF height = this.m_terrainManager.IsValidCoord(coord) ? (this.m_customHeightFn.HasValue ? this.m_customHeightFn.Value(coord) : this.m_terrainManager.GetHeightOrOceanSurface(coord.Tile2i)) : HeightTilesF.Zero;
        return coord.ExtendHeight(height).ToVector3();
      }
    }

    public bool InputUpdate()
    {
      if (this.m_polygon.IsNone)
        return false;
      if (this.m_shouldUpdateDueToHandleCreation)
      {
        this.m_shouldUpdateDueToHandleCreation = false;
        this.updatePolygon();
        Action<bool> onChange = this.OnChange;
        if (onChange != null)
          onChange(true);
      }
      this.m_moveCursor.SetTemporaryVisibility(false);
      if (EventSystem.current.IsPointerOverGameObject() || !this.m_pickingManager.PickedGameObject.HasValue)
        return false;
      GameObject key = this.m_pickingManager.PickedGameObject.Value;
      int vertexIndex;
      if (this.m_colliders.TryGetValue(key, out vertexIndex))
      {
        if (vertexIndex >= 0)
        {
          if (this.processVertexHover(vertexIndex))
            return true;
        }
        else if (this.processEdgeHover(-vertexIndex - 1))
          return true;
      }
      int index = 0;
      while (index < this.m_vertexEditHandles.Count && !((UnityEngine.Object) this.m_vertexEditHandles[index] == (UnityEngine.Object) key))
        ++index;
      return false;
    }

    public void SetCustomHeightFunction(Func<Tile2f, HeightTilesF> heightFn)
    {
      this.m_customHeightFn = (Option<Func<Tile2f, HeightTilesF>>) heightFn;
      this.updatePolygon();
    }

    private bool processVertexHover(int vertexIndex)
    {
      if (this.m_polygon.Value.Vertices.Length <= this.m_polygon.Value.MinVertexCount || !Input.GetMouseButtonDown(1))
        return false;
      this.m_polygon.Value.RemoveVertexAt(vertexIndex);
      this.updatePolygon();
      Action<bool> onChange = this.OnChange;
      if (onChange != null)
        onChange(true);
      return true;
    }

    private bool processEdgeHover(int edgeIndex)
    {
      int index = (edgeIndex - 1 + this.m_edgesLines.Count) % this.m_edgesLines.Count;
      if (this.m_polygon.Value.Vertices.Length + 1 >= this.m_polygon.Value.MaxVertexCount)
        return false;
      if (Input.GetMouseButtonDown(0))
      {
        this.m_polygon.Value.InsertVertexAt(Fix32.Half * (this.m_polygon.Value.Vertices[index] + this.m_polygon.Value.Vertices[(index + 1) % this.m_polygon.Value.Vertices.Length]), index + 1);
        this.m_shouldUpdateDueToHandleCreation = true;
        return true;
      }
      this.m_moveCursor.ShowTemporary();
      return false;
    }

    private void postDragBegin(Gizmo gizmo, int handleId) => this.m_handleDragged = false;

    private void postDragEnd(Gizmo gizmo, int handleId)
    {
      if (!this.m_handleDragged)
        return;
      Action<bool> onChange = this.OnChange;
      if (onChange != null)
        onChange(true);
      this.m_handleDragged = false;
    }

    static Polygon2fEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Polygon2fEditor.LINE_COLOR_DEFAULT = new Color(0.5f, 0.5f, 1f);
      Polygon2fEditor.VERTEX_COLOR_ADD = new Color(0.4f, 0.8f, 0.4f);
      Polygon2fEditor.HANDLE_MATERIAL = "Assets/Core/Materials/PolygonEditHandle.mat";
    }

    public class TransformGizmoListener : MonoBehaviour, IRTTransformGizmoListener
    {
      private Polygon2fEditor m_parent;
      private readonly Lyst<Vector2f> m_polygonTmp;

      public void SetParent(Polygon2fEditor parent) => this.m_parent = parent;

      public bool OnCanBeTransformed(Gizmo transformGizmo)
      {
        return !this.m_parent.m_cameraController.IsInFreeLookMode;
      }

      public void OnTransformed(Gizmo transformGizmo)
      {
        this.m_polygonTmp.Clear();
        foreach (GameObject vertexEditHandle in this.m_parent.m_vertexEditHandles)
          this.m_polygonTmp.Add(vertexEditHandle.transform.localPosition.ToTile2f().Vector2f);
        this.m_parent.m_polygon.Value.Initialize((IEnumerable<Vector2f>) this.m_polygonTmp);
        this.m_parent.updatePolygon();
        Action<bool> onChange = this.m_parent.OnChange;
        if (onChange != null)
          onChange(false);
        this.m_parent.m_handleDragged = true;
      }

      public TransformGizmoListener()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_polygonTmp = new Lyst<Vector2f>(true);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
