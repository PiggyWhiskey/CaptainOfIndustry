// Decompiled with JetBrains decompiler
// Type: RTG.GizmoSglAxisRotationDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoSglAxisRotationDrag3D : GizmoScreenDrag
  {
    private float _accumSnapDrag;
    private Plane _rotationPlane;
    private Vector3 _screenDragCircleTangent;
    private GizmoSglAxisRotationDrag3D.WorkData _workData;
    private bool _adjustRotationForAbsSnap;
    private float _relativeRotation;
    private float _totalRotation;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Rotation;

    public float RelativeRotation => this._relativeRotation;

    public float TotalRotation => this._totalRotation;

    public Plane RotationPlane => this._rotationPlane;

    public void SetWorkData(GizmoSglAxisRotationDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
      this._rotationPlane = new Plane(this._workData.Axis, this._workData.RotationPlanePos);
    }

    protected override void CalculateDragValues()
    {
      this._relativeRotation = Vector2.Dot((Vector2) this._screenDragCircleTangent, (Vector2) MonoSingleton<RTInputDevice>.Get.Device.GetFrameDelta()) * this.Sensitivity;
      if ((double) this._relativeRotation == 0.0)
      {
        this._relativeDragRotation = Quaternion.identity;
      }
      else
      {
        if (this.CanSnap())
        {
          this._accumSnapDrag += this._relativeRotation;
          this._accumSnapDrag %= 360f;
          if (this._workData.SnapMode == GizmoSnapMode.Absolute && this._adjustRotationForAbsSnap)
          {
            NumSnapSteps numSnapSteps = SnapMath.CalculateNumSnapSteps(this._workData.SnapStep, this._totalRotation);
            float totalRotation = this._totalRotation;
            this._totalRotation = (double) numSnapSteps.AbsFracSteps >= 0.5 ? (float) (numSnapSteps.AbsIntNumSteps + 1) * this._workData.SnapStep * Mathf.Sign(this._totalRotation) : (float) numSnapSteps.AbsIntNumSteps * this._workData.SnapStep * Mathf.Sign(this._totalRotation);
            this._relativeRotation = this._totalRotation - totalRotation;
            this._accumSnapDrag = 0.0f;
            this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation, this._workData.Axis);
            this._adjustRotationForAbsSnap = false;
          }
          else if (SnapMath.CanExtractSnap(this._workData.SnapStep, this._accumSnapDrag))
          {
            this._relativeRotation = SnapMath.ExtractSnap(this._workData.SnapStep, ref this._accumSnapDrag);
            this._totalRotation += this._relativeRotation;
            this._totalRotation %= 360f;
            this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation, this._workData.Axis);
          }
          else
            this._relativeDragRotation = Quaternion.identity;
        }
        else
        {
          this._accumSnapDrag = 0.0f;
          this._adjustRotationForAbsSnap = true;
          this._totalRotation += this._relativeRotation;
          this._totalRotation %= 360f;
          this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation, this._workData.Axis);
        }
        this._totalDragRotation = Quaternion.AngleAxis(this._totalRotation, this._workData.Axis);
      }
    }

    protected override void OnSessionBegin()
    {
      Camera targetCamera = MonoSingleton<RTFocusCamera>.Get.TargetCamera;
      this._adjustRotationForAbsSnap = false;
      if (MonoSingleton<RTGizmosEngine>.Get.HoveredGizmo.HoverInfo.HandleDimension == GizmoDimension.Dim3D)
      {
        Ray ray = MonoSingleton<RTInputDevice>.Get.Device.GetRay(targetCamera);
        float enter;
        if (this._rotationPlane.Raycast(ray, out enter))
        {
          Vector3 point = ray.GetPoint(enter);
          Vector3 vector3 = Vector3.Cross(this._workData.Axis, point - this._workData.RotationPlanePos);
          Vector2 screenPoint = (Vector2) targetCamera.WorldToScreenPoint(point);
          this._screenDragCircleTangent = (Vector3) ((Vector2) targetCamera.WorldToScreenPoint(point + vector3) - screenPoint).normalized;
        }
        else
        {
          Vector2 screenPoint = (Vector2) targetCamera.WorldToScreenPoint(this._workData.RotationPlanePos);
          this._screenDragCircleTangent = (Vector3) ((Vector2) targetCamera.WorldToScreenPoint(this._workData.RotationPlanePos + this._workData.Axis) - screenPoint).normalized;
          this._screenDragCircleTangent = (Vector3) new Vector2(-this._screenDragCircleTangent.y, this._screenDragCircleTangent.x);
          if ((double) Vector2.Dot((Vector2) this._screenDragCircleTangent, Vector2.right) < -9.9999997473787516E-06)
          {
            this._screenDragCircleTangent = -this._screenDragCircleTangent;
          }
          else
          {
            if ((double) Vector2.Dot((Vector2) this._screenDragCircleTangent, Vector2.up) >= -9.9999997473787516E-06)
              return;
            this._screenDragCircleTangent = -this._screenDragCircleTangent;
          }
        }
      }
      else
        this._screenDragCircleTangent = (Vector3) ((Vector2) MonoSingleton<RTInputDevice>.Get.Device.GetPositionYAxisUp() - (Vector2) targetCamera.WorldToScreenPoint(this._workData.RotationPlanePos)).GetNormal();
    }

    protected override void OnSessionEnd()
    {
      this._accumSnapDrag = 0.0f;
      this._screenDragCircleTangent = (Vector3) Vector2.zero;
      this._adjustRotationForAbsSnap = false;
      this._totalRotation = 0.0f;
      this._relativeRotation = 0.0f;
    }

    public GizmoSglAxisRotationDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public Vector3 RotationPlanePos;
      public Vector3 Axis;
      public GizmoSnapMode SnapMode;
      public float SnapStep;
    }
  }
}
