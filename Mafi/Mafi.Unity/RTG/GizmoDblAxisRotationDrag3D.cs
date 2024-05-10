// Decompiled with JetBrains decompiler
// Type: RTG.GizmoDblAxisRotationDrag3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoDblAxisRotationDrag3D : GizmoScreenDrag
  {
    private GizmoDblAxisRotationDrag3D.WorkData _workData;
    private bool _adjustRotationForAbsSnap;
    private float _accumSnapDrag0;
    private float _accumSnapDrag1;
    private float _relativeRotation0;
    private float _relativeRotation1;
    private float _totalRotation0;
    private float _totalRotation1;

    public override GizmoDragChannel DragChannel => GizmoDragChannel.Rotation;

    public float RelativeRotation0 => this._relativeRotation0;

    public float RelativeRotation1 => this._relativeRotation1;

    public float TotalRotation0 => this._totalRotation0;

    public float TotalRotation1 => this._totalRotation1;

    public override bool IsActive => false;

    public void SetWorkData(GizmoDblAxisRotationDrag3D.WorkData workData)
    {
      if (this.IsActive)
        return;
      this._workData = workData;
    }

    protected override void CalculateDragValues()
    {
      Vector2 frameDelta = (Vector2) MonoSingleton<RTInputDevice>.Get.Device.GetFrameDelta();
      this._relativeRotation0 = Vector2.Dot(frameDelta, this._workData.ScreenAxis0) * this.Sensitivity;
      this._relativeRotation1 = Vector2.Dot(frameDelta, this._workData.ScreenAxis1) * this.Sensitivity;
      if ((double) this._relativeRotation0 == 0.0 && (double) this._relativeRotation1 == 0.0)
      {
        this._relativeDragRotation = Quaternion.identity;
      }
      else
      {
        if (this.CanSnap())
        {
          this._accumSnapDrag0 += this._relativeRotation0;
          this._accumSnapDrag0 %= 360f;
          this._accumSnapDrag1 += this._relativeRotation1;
          this._accumSnapDrag1 %= 360f;
          if (this._workData.SnapMode == GizmoSnapMode.Absolute && this._adjustRotationForAbsSnap)
          {
            NumSnapSteps numSnapSteps1 = SnapMath.CalculateNumSnapSteps(this._workData.SnapStep0, this._totalRotation0);
            float totalRotation0 = this._totalRotation0;
            this._totalRotation0 = (double) numSnapSteps1.AbsFracSteps >= 0.5 ? (float) (numSnapSteps1.AbsIntNumSteps + 1) * this._workData.SnapStep0 * Mathf.Sign(this._totalRotation0) : (float) numSnapSteps1.AbsIntNumSteps * this._workData.SnapStep0 * Mathf.Sign(this._totalRotation0);
            this._accumSnapDrag0 = 0.0f;
            this._relativeRotation0 = this._totalRotation0 - totalRotation0;
            NumSnapSteps numSnapSteps2 = SnapMath.CalculateNumSnapSteps(this._workData.SnapStep1, this._totalRotation1);
            float totalRotation1 = this._totalRotation1;
            this._totalRotation1 = (double) numSnapSteps2.AbsFracSteps >= 0.5 ? (float) (numSnapSteps2.AbsIntNumSteps + 1) * this._workData.SnapStep1 * Mathf.Sign(this._totalRotation1) : (float) numSnapSteps2.AbsIntNumSteps * this._workData.SnapStep1 * Mathf.Sign(this._totalRotation1);
            this._accumSnapDrag1 = 0.0f;
            this._relativeRotation1 = this._totalRotation1 - totalRotation1;
            this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation1, this._workData.Axis1) * Quaternion.AngleAxis(this._relativeRotation0, this._workData.Axis0);
            this._adjustRotationForAbsSnap = false;
          }
          else
          {
            this._relativeDragRotation = Quaternion.identity;
            if (SnapMath.CanExtractSnap(this._workData.SnapStep0, this._accumSnapDrag0))
            {
              this._relativeRotation0 = SnapMath.ExtractSnap(this._workData.SnapStep0, ref this._accumSnapDrag0);
              this._totalRotation0 += this._relativeRotation0;
              this._totalRotation0 %= 360f;
              this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation0, this._workData.Axis0);
            }
            if (SnapMath.CanExtractSnap(this._workData.SnapStep1, this._accumSnapDrag1))
            {
              this._relativeRotation1 = SnapMath.ExtractSnap(this._workData.SnapStep1, ref this._accumSnapDrag1);
              this._totalRotation1 += this._relativeRotation1;
              this._totalRotation1 %= 360f;
              this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation1, this._workData.Axis1) * this._relativeDragRotation;
            }
          }
        }
        else
        {
          this._adjustRotationForAbsSnap = true;
          this._accumSnapDrag0 = this._accumSnapDrag1 = 0.0f;
          this._totalRotation0 += this._relativeRotation0;
          this._totalRotation1 += this._relativeRotation1;
          this._relativeDragRotation = Quaternion.AngleAxis(this._relativeRotation1, this._workData.Axis1) * Quaternion.AngleAxis(this._relativeRotation0, this._workData.Axis0);
        }
        this._totalDragRotation = this._relativeDragRotation * this._totalDragRotation;
      }
    }

    protected override void OnSessionEnd()
    {
      this._accumSnapDrag0 = 0.0f;
      this._accumSnapDrag1 = 0.0f;
      this._relativeRotation0 = 0.0f;
      this._relativeRotation1 = 0.0f;
      this._totalRotation0 = 0.0f;
      this._totalRotation1 = 0.0f;
    }

    public GizmoDblAxisRotationDrag3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public struct WorkData
    {
      public Vector2 ScreenAxis0;
      public Vector2 ScreenAxis1;
      public Vector3 Axis0;
      public Vector3 Axis1;
      public GizmoSnapMode SnapMode;
      public float SnapStep0;
      public float SnapStep1;
    }
  }
}
