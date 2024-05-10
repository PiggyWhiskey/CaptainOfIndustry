// Decompiled with JetBrains decompiler
// Type: RTG.GizmoUniformScaleDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoUniformScaleDrag3D : GizmoPlaneDrag3D
  {
    private GizmoUniformScaleDrag3D.WorkData _workData;
    private Vector3 _planeAxis0;
    private Vector3 _planeAxis1;
    private float _accumSnapDrag;
    private float _scale;
    private float _relativeScale;
    private float _totalScale;
    private Vector3 _scaleDragAxis;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Scale;

    public float TotalScale => this._totalScale;

    public float RelativeScale => this._relativeScale;

    public void SetWorkData(GizmoUniformScaleDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
      this._scale = 1f;
      this._scaleDragAxis = ((workData.CameraRight + workData.CameraUp) * 0.5f).normalized;
    }

    protected override Plane CalculateDragPlane()
    {
      this._planeAxis0 = this._workData.CameraRight;
      this._planeAxis1 = this._workData.CameraUp;
      return new Plane(Vector3.Cross(this._planeAxis0, this._planeAxis1).normalized, this._workData.DragOrigin);
    }

    protected override void CalculateDragValues()
    {
      if (this.CanSnap())
      {
        this._relativeDragScale = Vector3.one;
        this._accumSnapDrag += this._planeDragSession.DragDelta.Dot(this._scaleDragAxis);
        if (SnapMath.CanExtractSnap(this._workData.SnapStep, this._accumSnapDrag))
        {
          float snap = SnapMath.ExtractSnap(this._workData.SnapStep, ref this._accumSnapDrag);
          float scale = this._scale;
          this._scale += snap;
          this._relativeScale = this._scale / scale;
          this._totalScale = this._scale / 1f;
          this._relativeDragScale = Vector3Ex.FromValue(this._relativeScale);
        }
      }
      else
      {
        this._accumSnapDrag = 0.0f;
        float scale = this._scale;
        this._scale += this._planeDragSession.DragDelta.Dot(this._scaleDragAxis) * this.Sensitivity;
        this._relativeScale = this._scale / scale;
        this._totalScale = this._scale / 1f;
        this._relativeDragScale = Vector3Ex.FromValue(this._relativeScale);
      }
      this._totalDragScale = Vector3Ex.FromValue(this._totalScale);
    }

    protected override void OnSessionEnd()
    {
      this._relativeScale = 1f;
      this._totalScale = 1f;
    }

    public GizmoUniformScaleDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._relativeScale = 1f;
      this._totalScale = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public Vector3 CameraRight;
      public Vector3 CameraUp;
      public Vector3 DragOrigin;
      public float SnapStep;
    }
  }
}
