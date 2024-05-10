// Decompiled with JetBrains decompiler
// Type: RTG.GizmoSglAxisOffsetDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoSglAxisOffsetDrag3D : GizmoPlaneDrag3D
  {
    private float _accumSnapDrag;
    private GizmoSglAxisOffsetDrag3D.WorkData _workData;

    public Vector3 Axis => this._workData.Axis;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Offset;

    public void SetWorkData(GizmoSglAxisOffsetDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
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
        this._relativeDragOffset = Vector3.zero;
        this._accumSnapDrag += num;
        if (SnapMath.CanExtractSnap(this._workData.SnapStep, this._accumSnapDrag))
          this._relativeDragOffset = this._workData.Axis * SnapMath.ExtractSnap(this._workData.SnapStep, ref this._accumSnapDrag);
      }
      else
      {
        this._accumSnapDrag = 0.0f;
        this._relativeDragOffset = this._workData.Axis * num * this.Sensitivity;
      }
      this._totalDragOffset = this._totalDragOffset + this._relativeDragOffset;
    }

    protected override void OnSessionEnd() => this._accumSnapDrag = 0.0f;

    public GizmoSglAxisOffsetDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public Vector3 DragOrigin;
      public Vector3 Axis;
      public float SnapStep;
    }
  }
}
