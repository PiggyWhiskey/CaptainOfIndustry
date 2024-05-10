// Decompiled with JetBrains decompiler
// Type: RTG.DirectionalLightGizmo3D
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
  public class DirectionalLightGizmo3D : GizmoBehaviour
  {
    private Light _targetLight;
    private Vector3 _pickedWorldSnapPoint;
    private GizmoCap2D _dirSnapTick;
    private List<Vector3> _sourceCirclePoints;
    private List<Vector3> _lightRayEmissionPoints;
    private SceneRaycastFilter _raycastFilter;
    private GizmoSglAxisOffsetDrag3D _dummyDragSession;
    private GizmoSglAxisOffsetDrag3D.WorkData _dummySessionWorkData;
    private Light3DSnapshot _preChangeSnapshot;
    private Light3DSnapshot _postChangeSnapshot;
    private DirectionalLightGizmo3DLookAndFeel _lookAndFeel;
    private DirectionalLightGizmo3DLookAndFeel _sharedLookAndFeel;

    public DirectionalLightGizmo3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public DirectionalLightGizmo3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public Light TargetLight => this._targetLight;

    public void SetTargetLight(Light targetLight) => this._targetLight = targetLight;

    public bool OwnsHandle(int handleId) => handleId == this._dirSnapTick.HandleId;

    public override void OnAttached()
    {
      this._dirSnapTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.DirectionSnapCap);
      this._dirSnapTick.DragSession = (IGizmoDragSession) this._dummyDragSession;
      this.SetupSharedLookAndFeel();
      this._sourceCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1f, Vector3.right, Vector3.up, 100);
      this._raycastFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
      this._raycastFilter.AllowedObjectTypes.Add(GameObjectType.Terrain);
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      this.Gizmo.Transform.Position3D = this._targetLight.transform.position;
      this.UpdateTicks(this.Gizmo.GetWorkCamera());
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (!this.OwnsHandle(handleId) || !this.IsTargetReady())
        return;
      this._dummySessionWorkData.Axis = Vector3.one;
      this._dummyDragSession.SetWorkData(this._dummySessionWorkData);
      this._preChangeSnapshot.Snapshot(this._targetLight);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this.IsTargetReady())
        return;
      if (handleId == this._dirSnapTick.HandleId)
        this.SnapDirection();
      this.UpdateTicks(this.Gizmo.GetWorkCamera());
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      this._postChangeSnapshot.Snapshot(this._targetLight);
      new Light3DChangedAction(this._preChangeSnapshot, this._postChangeSnapshot).Execute();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      Vector3 position = this._targetLight.transform.position;
      float num1 = camera.EstimateZoomFactor(position);
      float num2 = num1 * this.LookAndFeel.SourceCircleRadius;
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateTicks(camera);
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(this.LookAndFeel.SourceCircleBorderColor);
      get.SetPass(0);
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(position, this._targetLight.transform.rotation, new Vector3(num2, num2, 1f)));
      GLRenderer.DrawLines3D(this._sourceCirclePoints);
      GL.PopMatrix();
      if (this.Gizmo.DragHandleId == this._dirSnapTick.HandleId)
      {
        get.SetColor(this.LookAndFeel.DirSnapSegmentColor);
        get.SetPass(0);
        GLRenderer.DrawLine3D(this._targetLight.transform.position, this._pickedWorldSnapPoint);
      }
      get.SetColor(this.LookAndFeel.LightRaysColor);
      get.SetPass(0);
      Vector3 forward = this._targetLight.transform.forward;
      this.GenerateLightRayEmissionPoints(camera);
      float num3 = num1 * this.LookAndFeel.LightRayLength;
      foreach (Vector3 rayEmissionPoint in this._lightRayEmissionPoints)
        GLRenderer.DrawLine3D(rayEmissionPoint, rayEmissionPoint + forward * num3);
      this._dirSnapTick.Render(camera);
    }

    private void UpdateTicks(Camera camera)
    {
      Plane plane = new Plane(camera.transform.forward, camera.transform.position);
      if (this.Gizmo.DragHandleId == this._dirSnapTick.HandleId)
      {
        if ((double) plane.GetDistanceToPoint(this._pickedWorldSnapPoint) > 0.0)
          this._dirSnapTick.SetVisible(true);
        else
          this._dirSnapTick.SetVisible(false);
        this._dirSnapTick.Position = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(this._pickedWorldSnapPoint);
      }
      else
      {
        Vector3 position = this._targetLight.transform.position;
        if ((double) plane.GetDistanceToPoint(position) > 0.0)
          this._dirSnapTick.SetVisible(true);
        else
          this._dirSnapTick.SetVisible(false);
        this._dirSnapTick.Position = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(position);
      }
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel.ConnectDirSnapTickLookAndFeel(this._dirSnapTick);
    }

    private void GenerateLightRayEmissionPoints(Camera camera)
    {
      Vector3 position = this._targetLight.transform.position;
      float num1 = camera.EstimateZoomFactor(position) * this.LookAndFeel.SourceCircleRadius;
      Vector3 right = this._targetLight.transform.right;
      Vector3 up = this._targetLight.transform.up;
      this._lightRayEmissionPoints.Clear();
      float num2 = 360f / (float) (this.LookAndFeel.NumLightRays - 1);
      for (int index = 0; index < this.LookAndFeel.NumLightRays; ++index)
      {
        float f = (float) ((double) num2 * (double) index * (Math.PI / 180.0));
        this._lightRayEmissionPoints.Add(position + right * Mathf.Sin(f) * num1 + up * Mathf.Cos(f) * num1);
      }
    }

    private void SnapDirection()
    {
      SceneRaycastHit sceneRaycastHit = MonoSingleton<RTScene>.Get.Raycast(MonoSingleton<RTInputDevice>.Get.Device.GetRay(this.Gizmo.GetWorkCamera()), SceneRaycastPrecision.BestFit, this._raycastFilter);
      if (sceneRaycastHit.WasAnObjectHit)
      {
        this._pickedWorldSnapPoint = sceneRaycastHit.ObjectHit.HitPoint;
      }
      else
      {
        if (!sceneRaycastHit.WasGridHit)
          return;
        this._pickedWorldSnapPoint = sceneRaycastHit.GridHit.HitPoint;
      }
      Vector3 vector3 = this._pickedWorldSnapPoint - this._targetLight.transform.position;
      if ((double) vector3.magnitude <= 9.9999997473787516E-05)
        return;
      this._targetLight.transform.Align(vector3.normalized, TransformAxis.PositiveZ);
    }

    private bool IsTargetReady()
    {
      return (UnityEngine.Object) this._targetLight != (UnityEngine.Object) null && this._targetLight.enabled && this._targetLight.gameObject.activeSelf && this._targetLight.type == LightType.Directional;
    }

    public DirectionalLightGizmo3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._lightRayEmissionPoints = new List<Vector3>();
      this._raycastFilter = new SceneRaycastFilter();
      this._dummyDragSession = new GizmoSglAxisOffsetDrag3D();
      this._preChangeSnapshot = new Light3DSnapshot();
      this._postChangeSnapshot = new Light3DSnapshot();
      this._lookAndFeel = new DirectionalLightGizmo3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
