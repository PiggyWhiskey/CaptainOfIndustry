// Decompiled with JetBrains decompiler
// Type: RTG.RTSceneGrid
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
  [Serializable]
  public class RTSceneGrid : MonoSingleton<RTSceneGrid>, IXZGrid
  {
    [SerializeField]
    private SceneGridHotkeys _hotkeys;
    [SerializeField]
    private XZGridSettings _settings;
    [SerializeField]
    private XZGridLookAndFeel _lookAndFeel;
    private List<Camera> _renderIgnoreCameras;

    public Quaternion Rotation => Quaternion.Euler(this.Settings.RotationAngles);

    public Vector3 Right => this.Rotation * Vector3.right;

    public Vector3 Look => this.Rotation * Vector3.forward;

    public Vector3 Normal => this.Rotation * Vector3.up;

    public Plane WorldPlane => new Plane(this.Normal, this.Normal * this.YOffset);

    public Matrix4x4 WorldMatrix
    {
      get => Matrix4x4.TRS(this.Normal * this.YOffset, this.Rotation, Vector3.one);
    }

    public float YOffset
    {
      get => this.Settings.YOffset;
      set => this.Settings.YOffset = value;
    }

    public SceneGridHotkeys Hotkeys => this._hotkeys;

    public XZGridSettings Settings => this._settings;

    public XZGridLookAndFeel LookAndFeel => this._lookAndFeel;

    public void Initialize_SystemCall()
    {
      MonoSingleton<RTInputDevice>.Get.Device.DoubleTap += new InputDeviceDoubleTapHandler(this.OnInputDeviceDoubleTap);
    }

    public bool IsRenderIgnoreCamera(Camera camera) => this._renderIgnoreCameras.Contains(camera);

    public void AddRenderIgnoreCamera(Camera camera)
    {
      if (this.IsRenderIgnoreCamera(camera))
        return;
      this._renderIgnoreCameras.Add(camera);
    }

    public void RemoveRenderIgnoreCamera(Camera camera) => this._renderIgnoreCameras.Remove(camera);

    public XZGridCell CellFromWorldPoint(Vector3 worldPoint)
    {
      return XZGridCell.FromPoint(worldPoint, this._settings.CellSizeX, this._settings.CellSizeZ, (IXZGrid) this);
    }

    public bool Raycast(Ray ray, out float t) => this.WorldPlane.Raycast(ray, out t);

    public void Update_SystemCall()
    {
      if (!this.Settings.IsVisible)
        return;
      if (this.Hotkeys.GridUp.IsActiveInFrame())
        this.MoveUp();
      else if (this.Hotkeys.GridDown.IsActiveInFrame())
        this.MoveDown();
      if (MonoSingleton<RTInputDevice>.Get.Device.DidDoubleTap || !MonoSingleton<RTInputDevice>.Get.Device.WasButtonPressedInCurrentFrame(0) || !this.Hotkeys.SnapToCursorPickPoint.IsActive())
        return;
      SceneRaycastHit sceneHitForGridSnap = this.GetSceneHitForGridSnap();
      if (!sceneHitForGridSnap.WasAnObjectHit)
        return;
      this.SnapToObjectHitPoint(sceneHitForGridSnap.ObjectHit, RTSceneGrid.SnapToPointMode.Exact);
    }

    public void Render_SystemCall(Camera renderCamera)
    {
      if (!this.Settings.IsVisible || this.IsRenderIgnoreCamera(renderCamera))
        return;
      AABB volumeAabb = renderCamera.CalculateVolumeAABB();
      Matrix4x4 matrix = Matrix4x4.TRS(this.WorldPlane.ProjectPoint(volumeAabb.Center), this.Rotation, (volumeAabb.Size * 2f) with
      {
        y = 1f
      });
      if (this.LookAndFeel.UseCellFading)
      {
        float cellFadeZoom = this.CalculateCellFadeZoom(renderCamera);
        int p1 = MathEx.GetNumDigits((int) cellFadeZoom) - 1;
        int p2 = p1 + 1;
        float num1 = Mathf.Pow(10f, (float) p1);
        float num2 = Mathf.Pow(10f, (float) p2);
        Color lineColor = this.LookAndFeel.LineColor;
        float num3 = (float) (((double) num2 - (double) cellFadeZoom) / ((double) num2 - (double) num1));
        float num4 = 1f - num3;
        Material xzGridPlane = Singleton<MaterialPool>.Get.XZGrid_Plane;
        xzGridPlane.SetFloat("_CamFarPlaneDist", renderCamera.farClipPlane);
        xzGridPlane.SetVector("_CamWorldPos", (Vector4) renderCamera.transform.position);
        xzGridPlane.SetVector("_GridOrigin", (Vector4) Vector3.zero);
        xzGridPlane.SetVector("_GridRight", (Vector4) this.Right);
        xzGridPlane.SetVector("_GridLook", (Vector4) this.Look);
        xzGridPlane.SetMatrix("_TransformMatrix", matrix);
        lineColor.a = this.LookAndFeel.LineColor.a * num3;
        if ((double) lineColor.a != 0.0)
        {
          xzGridPlane.SetFloat("_CellSizeX", this.Settings.CellSizeX * num1);
          xzGridPlane.SetFloat("_CellSizeZ", this.Settings.CellSizeZ * num1);
          xzGridPlane.SetColor("_LineColor", lineColor);
          xzGridPlane.SetPass(0);
          Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXZ, matrix);
        }
        lineColor.a = this.LookAndFeel.LineColor.a * num4;
        if ((double) lineColor.a == 0.0)
          return;
        xzGridPlane.SetFloat("_CellSizeX", this.Settings.CellSizeX * num2);
        xzGridPlane.SetFloat("_CellSizeZ", this.Settings.CellSizeZ * num2);
        xzGridPlane.SetColor("_LineColor", lineColor);
        xzGridPlane.SetPass(0);
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXZ, matrix);
      }
      else
      {
        Material xzGridPlane = Singleton<MaterialPool>.Get.XZGrid_Plane;
        xzGridPlane.SetFloat("_CamFarPlaneDist", renderCamera.farClipPlane);
        xzGridPlane.SetVector("_CamWorldPos", (Vector4) renderCamera.transform.position);
        xzGridPlane.SetMatrix("_TransformMatrix", matrix);
        xzGridPlane.SetFloat("_CellSizeX", this.Settings.CellSizeX);
        xzGridPlane.SetFloat("_CellSizeZ", this.Settings.CellSizeZ);
        xzGridPlane.SetColor("_LineColor", this.LookAndFeel.LineColor);
        xzGridPlane.SetVector("_GridOrigin", (Vector4) Vector3.zero);
        xzGridPlane.SetVector("_GridRight", (Vector4) this.Right);
        xzGridPlane.SetVector("_GridLook", (Vector4) this.Look);
        xzGridPlane.SetPass(0);
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXZ, matrix);
      }
    }

    private void MoveUp()
    {
      if (!this.Settings.IsVisible)
        return;
      this.YOffset += this.Settings.UpDownStep;
    }

    private void MoveDown()
    {
      if (!this.Settings.IsVisible)
        return;
      this.YOffset -= this.Settings.UpDownStep;
    }

    private float CalculateCellFadeZoom(Camera camera)
    {
      return this.WorldPlane.GetAbsDistanceToPoint(camera.transform.position);
    }

    private SceneRaycastHit GetSceneHitForGridSnap()
    {
      return MonoSingleton<RTScene>.Get.Raycast(MonoSingleton<RTInputDevice>.Get.Device.GetRay(MonoSingleton<RTFocusCamera>.Get.TargetCamera), SceneRaycastPrecision.BestFit, new SceneRaycastFilter()
      {
        AllowedObjectTypes = {
          GameObjectType.Mesh
        }
      });
    }

    private void OnInputDeviceDoubleTap(IInputDevice inputDevice, Vector2 position)
    {
      if (!this.Hotkeys.SnapToCursorPickPoint.IsActive())
        return;
      SceneRaycastHit sceneHitForGridSnap = this.GetSceneHitForGridSnap();
      if (!sceneHitForGridSnap.WasAnObjectHit)
        return;
      this.SnapToObjectHitPoint(sceneHitForGridSnap.ObjectHit, RTSceneGrid.SnapToPointMode.ClosestExtremity);
    }

    private void SnapToObjectHitPoint(
      GameObjectRayHit objectHit,
      RTSceneGrid.SnapToPointMode snapMode)
    {
      if (snapMode == RTSceneGrid.SnapToPointMode.Exact)
      {
        this.YOffset = new Plane(this.Normal, Vector3.zero).GetDistanceToPoint(objectHit.HitPoint);
      }
      else
      {
        OBB obb = ObjectBounds.CalcWorldOBB(objectHit.HitObject, new ObjectBounds.QueryConfig()
        {
          ObjectTypes = GameObjectType.Mesh
        });
        if (!obb.IsValid)
          return;
        Plane plane = new Plane(this.Normal, obb.Center);
        Vector3 center = obb.Center;
        List<Vector3> points = BoxMath.CalcBoxCornerPoints(obb.Center, obb.Size, obb.Rotation);
        if ((double) Mathf.Sign(plane.GetDistanceToPoint(objectHit.HitPoint)) > 0.0)
        {
          int furthestPtInFront = plane.GetFurthestPtInFront(points);
          if (furthestPtInFront >= 0)
            center = points[furthestPtInFront];
        }
        else
        {
          int furthestPtBehind = plane.GetFurthestPtBehind(points);
          if (furthestPtBehind >= 0)
            center = points[furthestPtBehind];
        }
        this.YOffset = new Plane(this.Normal, Vector3.zero).GetDistanceToPoint(center);
      }
    }

    public RTSceneGrid()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._hotkeys = new SceneGridHotkeys();
      this._settings = new XZGridSettings();
      this._lookAndFeel = new XZGridLookAndFeel();
      this._renderIgnoreCameras = new List<Camera>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private enum SnapToPointMode
    {
      Exact,
      ClosestExtremity,
    }
  }
}
