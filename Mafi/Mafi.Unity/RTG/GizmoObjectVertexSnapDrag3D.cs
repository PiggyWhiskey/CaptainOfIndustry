// Decompiled with JetBrains decompiler
// Type: RTG.GizmoObjectVertexSnapDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoObjectVertexSnapDrag3D : GizmoDragSession
  {
    private IEnumerable<GameObject> _targetObjects;
    private Vector3 _snapPivot;
    private bool _isActive;
    private List<GameObject> _destinationObjects;
    private GizmoObjectVertexSnapSettings _settings;
    private List<GameObject> _visibleObjectBuffer;

    public override bool IsActive => this._isActive;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Offset;

    public Vector3 SnapPivot => this._snapPivot;

    public GizmoObjectVertexSnapSettings Settings
    {
      set
      {
        if (value == null)
          return;
        this._settings = value;
      }
    }

    public void SetTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      if (this.IsActive)
        return;
      this._targetObjects = targetObjects;
    }

    public bool SelectSnapPivotPoint(Gizmo gizmo)
    {
      return this._targetObjects != null && !this.IsActive && this.GetWorldPointClosestToInputDevice(gizmo.FocusCamera, this._targetObjects, out this._snapPivot);
    }

    protected override bool DoBeginSession()
    {
      if (this._targetObjects == null)
        return false;
      this._isActive = true;
      return true;
    }

    protected override bool DoUpdateSession()
    {
      this.GatherDestinationObjects();
      return true;
    }

    protected override void DoEndSession()
    {
      this._isActive = false;
      this._destinationObjects.Clear();
    }

    protected override void CalculateDragValues()
    {
      Camera targetCamera = MonoSingleton<RTFocusCamera>.Get.TargetCamera;
      this._relativeDragOffset = Vector3.zero;
      if (this._destinationObjects.Count != 0 && this._settings.CanSnapToObjectVerts)
      {
        Vector3 point;
        if (this.GetWorldPointClosestToInputDevice(targetCamera, (IEnumerable<GameObject>) this._destinationObjects, out point))
          this._relativeDragOffset = point - this._snapPivot;
      }
      else if (this._settings.CanSnapToGrid)
      {
        XZGridRayHit xzGridRayHit = MonoSingleton<RTScene>.Get.RaycastSceneGridIfVisible(MonoSingleton<RTInputDevice>.Get.Device.GetRay(targetCamera));
        if (xzGridRayHit != null)
        {
          List<Vector3> centerAndCorners = MonoSingleton<RTSceneGrid>.Get.CellFromWorldPoint(xzGridRayHit.HitPoint).GetCenterAndCorners();
          int pointClosestToPoint = Vector3Ex.GetPointClosestToPoint(centerAndCorners, xzGridRayHit.HitPoint);
          if (pointClosestToPoint >= 0)
            this._relativeDragOffset = centerAndCorners[pointClosestToPoint] - this._snapPivot;
        }
      }
      this._snapPivot += this._relativeDragOffset;
      this._totalDragOffset = this._totalDragOffset + this._relativeDragOffset;
    }

    protected bool GetWorldPointClosestToInputDevice(
      Camera focusCamera,
      IEnumerable<GameObject> gameObjects,
      out Vector3 point)
    {
      point = Vector3.zero;
      if (gameObjects == null || !MonoSingleton<RTInputDevice>.Get.Device.HasPointer())
        return false;
      Vector2 positionYaxisUp = (Vector2) MonoSingleton<RTInputDevice>.Get.Device.GetPositionYAxisUp();
      float num = float.MaxValue;
      bool closestToInputDevice = false;
      foreach (GameObject gameObject in gameObjects)
      {
        Mesh mesh = gameObject.GetMesh();
        if ((UnityEngine.Object) mesh != (UnityEngine.Object) null)
        {
          MeshVertexChunkCollection vertexChunkCollection = Singleton<MeshVertexChunkCollectionDb>.Get[mesh];
          if (vertexChunkCollection != null)
          {
            Matrix4x4 localToWorldMatrix = gameObject.transform.localToWorldMatrix;
            List<MeshVertexChunk> chunksHoveredByPoint = vertexChunkCollection.GetWorldChunksHoveredByPoint((Vector3) positionYaxisUp, localToWorldMatrix, focusCamera);
            if (chunksHoveredByPoint.Count == 0)
            {
              MeshVertexChunk closestToScreenPt = vertexChunkCollection.GetWorldVertChunkClosestToScreenPt(positionYaxisUp, localToWorldMatrix, focusCamera);
              if (closestToScreenPt != null && closestToScreenPt.VertexCount != 0)
                chunksHoveredByPoint.Add(closestToScreenPt);
            }
            foreach (MeshVertexChunk meshVertexChunk in chunksHoveredByPoint)
            {
              Vector3 closestToScreenPt = meshVertexChunk.GetWorldVertClosestToScreenPt(positionYaxisUp, localToWorldMatrix, focusCamera);
              Vector2 screenPoint = (Vector2) focusCamera.WorldToScreenPoint(closestToScreenPt);
              float sqrMagnitude = (positionYaxisUp - screenPoint).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num)
              {
                num = sqrMagnitude;
                point = closestToScreenPt;
                closestToInputDevice = true;
              }
            }
          }
        }
        else
        {
          OBB obb = ObjectBounds.CalcSpriteWorldOBB(gameObject);
          if (obb.IsValid)
          {
            List<Vector3> centerAndCornerPoints = obb.GetCenterAndCornerPoints();
            List<Vector2> screenPoints = focusCamera.ConvertWorldToScreenPoints(centerAndCornerPoints);
            int pointClosestToPoint = Vector2Ex.GetPointClosestToPoint(screenPoints, positionYaxisUp);
            if (pointClosestToPoint >= 0)
            {
              Vector2 vector2 = screenPoints[pointClosestToPoint];
              float sqrMagnitude = (positionYaxisUp - vector2).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num)
              {
                num = sqrMagnitude;
                point = centerAndCornerPoints[pointClosestToPoint];
                closestToInputDevice = true;
              }
            }
          }
        }
      }
      return closestToInputDevice;
    }

    protected bool CanUseObjectAsSnapDestination(GameObject gameObject)
    {
      return this._settings.IsLayerSnapDestination(gameObject.layer);
    }

    private void GatherDestinationObjects()
    {
      Camera focusCamera = MonoSingleton<RTFocusCamera>.Get.TargetCamera;
      this._destinationObjects.Clear();
      IInputDevice device = MonoSingleton<RTInputDevice>.Get.Device;
      if (!device.HasPointer())
        return;
      Vector2 inputDevicePos = (Vector2) device.GetPositionYAxisUp();
      ObjectBounds.QueryConfig boundsQConfig = new ObjectBounds.QueryConfig();
      boundsQConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      boundsQConfig.NoVolumeSize = Vector3Ex.FromValue(1E-05f);
      MonoSingleton<RTFocusCamera>.Get.GetVisibleObjects(this._visibleObjectBuffer);
      List<GameObject> targetObjects = new List<GameObject>(this._targetObjects);
      this._visibleObjectBuffer.RemoveAll((Predicate<GameObject>) (a => targetObjects.Contains(a) || !ObjectBounds.CalcScreenRect(a, focusCamera, boundsQConfig).Contains(inputDevicePos) || targetObjects.FindAll((Predicate<GameObject>) (b => a.transform.IsChildOf(b.transform))).Count != 0));
      foreach (GameObject gameObject in this._visibleObjectBuffer)
      {
        if (this.CanUseObjectAsSnapDestination(gameObject))
        {
          switch (gameObject.GetGameObjectType())
          {
            case GameObjectType.Mesh:
            case GameObjectType.Sprite:
              this._destinationObjects.Add(gameObject);
              continue;
            default:
              continue;
          }
        }
      }
    }

    public GizmoObjectVertexSnapDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._destinationObjects = new List<GameObject>();
      this._settings = new GizmoObjectVertexSnapSettings();
      this._visibleObjectBuffer = new List<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
