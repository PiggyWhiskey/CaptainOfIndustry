// Decompiled with JetBrains decompiler
// Type: RTG.GizmoTransformAxisMap3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoTransformAxisMap3D
  {
    private Vector3 _freeAxis;
    private AxisDescriptor _mappedAxisDesc;
    private GizmoTransform _transform;

    public AxisDescriptor MappedAxisDesc => this._mappedAxisDesc;

    public int MappedAxisIndex => this._mappedAxisDesc.Index;

    public AxisSign MappedAxisSign => this._mappedAxisDesc.Sign;

    public bool IsMapped => this._transform != null;

    public Vector3 Axis
    {
      get => this.IsMapped ? this._transform.GetAxis3D(this._mappedAxisDesc) : this._freeAxis;
    }

    public GizmoTransform Transform => this._transform;

    public void Map(GizmoTransform transform, int axisIndex, AxisSign axisSign)
    {
      if (transform == null)
        return;
      this._mappedAxisDesc = new AxisDescriptor(axisIndex, axisSign);
      this._transform = transform;
    }

    public void Unmap() => this._transform = (GizmoTransform) null;

    public void SetAxis(Vector3 axis)
    {
      if (this.IsMapped)
        this.SetMappedAxis(axis);
      else
        this.SetFreeAxis(axis);
    }

    public void SetMappedAxis(Vector3 axis)
    {
      if (!this.IsMapped)
        return;
      Vector3 axis3D = this._transform.GetAxis3D(0, AxisSign.Positive);
      if (this._mappedAxisDesc.Index == 0)
        axis3D = this._transform.GetAxis3D(1, AxisSign.Positive);
      this._transform.Rotate3D(QuaternionEx.FromToRotation3D(this.Axis, axis, axis3D));
    }

    public void SetFreeAxis(Vector3 axis) => this._freeAxis = axis.normalized;

    public GizmoTransformAxisMap3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._freeAxis = Vector3.right;
      this._mappedAxisDesc = new AxisDescriptor(0, AxisSign.Positive);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
