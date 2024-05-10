// Decompiled with JetBrains decompiler
// Type: RTG.GizmoDblAxisOffsetDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoDblAxisOffsetDrag3D : GizmoPlaneDrag3D
  {
    private float _accumSnapDrag0;
    private float _accumSnapDrag1;
    private GizmoDblAxisOffsetDrag3D.WorkData _workData;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Offset;

    public void SetWorkData(GizmoDblAxisOffsetDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
    }

    protected override Plane CalculateDragPlane()
    {
      return new Plane(Vector3.Normalize(Vector3.Cross(this._workData.Axis0, this._workData.Axis1)), this._workData.DragOrigin);
    }

    protected override void CalculateDragValues()
    {
      float num1 = this._planeDragSession.DragDelta.Dot(this._workData.Axis0);
      float num2 = this._planeDragSession.DragDelta.Dot(this._workData.Axis1);
      if (this.CanSnap())
      {
        this._relativeDragOffset = Vector3.zero;
        this._accumSnapDrag0 += num1;
        this._accumSnapDrag1 += num2;
        if (SnapMath.CanExtractSnap(this._workData.SnapStep0, this._accumSnapDrag0))
          this._relativeDragOffset = this._relativeDragOffset + this._workData.Axis0 * SnapMath.ExtractSnap(this._workData.SnapStep0, ref this._accumSnapDrag0);
        if (SnapMath.CanExtractSnap(this._workData.SnapStep1, this._accumSnapDrag1))
          this._relativeDragOffset = this._relativeDragOffset + this._workData.Axis1 * SnapMath.ExtractSnap(this._workData.SnapStep1, ref this._accumSnapDrag1);
      }
      else
      {
        this._accumSnapDrag0 = 0.0f;
        this._accumSnapDrag1 = 0.0f;
        this._relativeDragOffset = this._planeDragSession.DragDelta * this.Sensitivity;
      }
      this._totalDragOffset = this._totalDragOffset + this._relativeDragOffset;
    }

    protected override void OnSessionEnd()
    {
      this._accumSnapDrag0 = 0.0f;
      this._accumSnapDrag1 = 0.0f;
    }

    public GizmoDblAxisOffsetDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public Vector3 DragOrigin;
      public Vector3 Axis0;
      public Vector3 Axis1;
      public float SnapStep0;
      public float SnapStep1;
    }
  }
}
