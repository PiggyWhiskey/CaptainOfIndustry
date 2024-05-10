// Decompiled with JetBrains decompiler
// Type: RTG.GizmoDblAxisScaleDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoDblAxisScaleDrag3D : GizmoPlaneDrag3D
  {
    private GizmoDblAxisScaleDrag3D.WorkData _workData;
    private float _accumSnapDrag0;
    private float _accumSnapDrag1;
    private float _scale0;
    private float _scale1;
    private float _relativeScale0;
    private float _relativeScale1;
    private float _totalScale0;
    private float _totalScale1;
    private Vector3 _scaleDragAxis;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Scale;

    public int AxisIndex0 => this._workData.AxisIndex0;

    public int AxisIndex1 => this._workData.AxisIndex1;

    public float RelativeScale0 => this._relativeScale0;

    public float RelativeScale1 => this._relativeScale1;

    public float TotalScale0 => this._totalScale0;

    public float TotalScale1 => this._totalScale1;

    public void SetWorkData(GizmoDblAxisScaleDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
      this._scale0 = 1f;
      this._scale1 = 1f;
      this._scaleDragAxis = ((workData.Axis0 + workData.Axis1) * 0.5f).normalized;
    }

    protected override Plane CalculateDragPlane()
    {
      return new Plane(Vector3.Cross(this._workData.Axis0, this._workData.Axis1).normalized, this._workData.DragOrigin);
    }

    protected override void CalculateDragValues()
    {
      float num1 = this._planeDragSession.DragDelta.Dot(this._scaleDragAxis);
      float num2 = num1;
      float snapStep1;
      float snapStep2 = snapStep1 = this._workData.SnapStep;
      float num3;
      float num4 = num3 = 1f;
      if (this.CanSnap())
      {
        this._relativeDragScale = Vector3.one;
        this._accumSnapDrag0 += num1;
        if (SnapMath.CanExtractSnap(snapStep2, this._accumSnapDrag0))
        {
          float scale0 = this._scale0;
          this._scale0 += SnapMath.ExtractSnap(snapStep2, ref this._accumSnapDrag0);
          this._totalScale0 = this._scale0 / num4;
          this._relativeScale0 = this._scale0 / scale0;
          this._relativeDragScale[this._workData.AxisIndex0] = this._relativeScale0;
        }
        this._accumSnapDrag1 += num2;
        if (SnapMath.CanExtractSnap(snapStep1, this._accumSnapDrag1))
        {
          float scale1 = this._scale1;
          this._scale1 += SnapMath.ExtractSnap(snapStep1, ref this._accumSnapDrag1);
          this._totalScale1 = this._scale1 / num3;
          this._relativeScale1 = this._scale1 / scale1;
          this._relativeDragScale[this._workData.AxisIndex1] = this._relativeScale1;
        }
      }
      else
      {
        this._accumSnapDrag0 = 0.0f;
        this._accumSnapDrag1 = 0.0f;
        float scale0 = this._scale0;
        this._scale0 += num1 * this.Sensitivity;
        this._totalScale0 = this._scale0 / num4;
        this._relativeScale0 = this._scale0 / scale0;
        this._relativeDragScale[this._workData.AxisIndex0] = this._relativeScale0;
        float scale1 = this._scale1;
        this._scale1 += num2 * this.Sensitivity;
        this._totalScale1 = this._scale1 / num3;
        this._relativeScale1 = this._scale1 / scale1;
        this._relativeDragScale[this._workData.AxisIndex1] = this._relativeScale1;
      }
      this._totalDragScale[this._workData.AxisIndex0] = this._totalScale0;
      this._totalDragScale[this._workData.AxisIndex1] = this._totalScale1;
    }

    protected override void OnSessionEnd()
    {
      this._accumSnapDrag0 = 0.0f;
      this._accumSnapDrag1 = 0.0f;
      this._relativeScale0 = 1f;
      this._relativeScale1 = 1f;
      this._totalScale0 = 1f;
      this._totalScale1 = 1f;
    }

    public GizmoDblAxisScaleDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._relativeScale0 = 1f;
      this._relativeScale1 = 1f;
      this._totalScale0 = 1f;
      this._totalScale1 = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public int AxisIndex0;
      public int AxisIndex1;
      public Vector3 DragOrigin;
      public Vector3 Axis0;
      public Vector3 Axis1;
      public float SnapStep;
    }
  }
}
