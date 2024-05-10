// Decompiled with JetBrains decompiler
// Type: RTG.GizmoSglAxisScaleDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoSglAxisScaleDrag3D : GizmoPlaneDrag3D
  {
    private float _accumSnapDrag;
    private GizmoSglAxisScaleDrag3D.WorkData _workData;
    private float _scale;
    private float _relativeScale;
    private float _totalScale;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Scale;

    public int AxisIndex => this._workData.AxisIndex;

    public float RelativeScale => this._relativeScale;

    public float TotalScale => this._totalScale;

    public void SetWorkData(GizmoSglAxisScaleDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
      this._scale = this._workData.EntityScale;
    }

    protected override Plane CalculateDragPlane()
    {
      return PlaneEx.GetCameraFacingAxisSlicePlane(this._workData.DragOrigin, this._workData.Axis, MonoSingleton<RTFocusCamera>.Get.TargetCamera);
    }

    protected override void CalculateDragValues()
    {
      float num = this._planeDragSession.DragDelta.Dot(this._workData.Axis);
      if (this.CanSnap())
      {
        this._relativeDragScale = Vector3.one;
        this._accumSnapDrag += num;
        if (SnapMath.CanExtractSnap(this._workData.SnapStep, this._accumSnapDrag))
        {
          float snap = SnapMath.ExtractSnap(this._workData.SnapStep, ref this._accumSnapDrag);
          float scale = this._scale;
          this._scale += snap;
          this._totalScale = this._scale / this._workData.EntityScale;
          this._relativeScale = this._scale / scale;
          this._relativeDragScale[this._workData.AxisIndex] = this._relativeScale;
        }
      }
      else
      {
        this._accumSnapDrag = 0.0f;
        float scale = this._scale;
        this._scale += num * this.Sensitivity;
        this._totalScale = this._scale / this._workData.EntityScale;
        this._relativeScale = this._scale / scale;
        this._relativeDragScale[this._workData.AxisIndex] = this._relativeScale;
      }
      this._totalDragScale[this._workData.AxisIndex] = this._totalScale;
    }

    protected override void OnSessionEnd()
    {
      this._accumSnapDrag = 0.0f;
      this._relativeScale = 1f;
      this._totalScale = 1f;
      this._scale = 1f;
    }

    public GizmoSglAxisScaleDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._relativeScale = 1f;
      this._totalScale = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public int AxisIndex;
      public Vector3 DragOrigin;
      public Vector3 Axis;
      public float SnapStep;
      public float EntityScale;
    }
  }
}
