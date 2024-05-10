// Decompiled with JetBrains decompiler
// Type: RTG.GizmoTransform
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoTransform
  {
    private bool _firingChanged3DEvent;
    private bool _firingChanged2DEvent;
    private Vector3 _position3D;
    private Vector3 _localPosition3D;
    private Quaternion _rotation3D;
    private Quaternion _localRotation3D;
    private Vector2 _position2D;
    private Vector2 _localPosition2D;
    private float _rotation2DDegrees;
    private Quaternion _rotation2D;
    private float _localRotation2DDegrees;
    private Quaternion _localRotation2D;
    private Vector3[] _axes3D;
    private Vector2[] _axes2D;
    private GizmoTransform _parent;
    private List<GizmoTransform> _children;

    public event GizmoEntityTransformChangedHandler Changed;

    public bool CanChange3D => !this._firingChanged3DEvent;

    public bool CanChange2D => !this._firingChanged2DEvent;

    public GizmoTransform Parent => this._parent;

    public int NumChildren => this._children.Count;

    public List<GizmoTransform> Children
    {
      get => new List<GizmoTransform>((IEnumerable<GizmoTransform>) this._children);
    }

    public Vector3 Right3D => this._axes3D[0];

    public Vector3 Up3D => this._axes3D[1];

    public Vector3 Look3D => this._axes3D[2];

    public Vector2 Right2D => this._axes2D[0];

    public Vector2 Up2D => this._axes2D[1];

    public Vector3 Position3D
    {
      get => this._position3D;
      set
      {
        if (!this.CanChange3D || !(this._position3D != value))
          return;
        this.ChangePosition3D(value);
      }
    }

    public Vector2 Position2D
    {
      get => this._position2D;
      set
      {
        if (!this.CanChange2D || !(this._position2D != value))
          return;
        this.ChangePosition2D(value);
      }
    }

    public Quaternion Rotation3D
    {
      get => this._rotation3D;
      set
      {
        if (!this.CanChange3D || !(this._rotation3D.eulerAngles != value.eulerAngles))
          return;
        this.ChangeRotation3D(value);
      }
    }

    public Quaternion Rotation2D => this._rotation2D;

    public float Rotation2DDegrees
    {
      get => this._rotation2DDegrees;
      set
      {
        if (!this.CanChange2D || (double) this._rotation2DDegrees == (double) value)
          return;
        this.ChangeRotation2D(value);
      }
    }

    public Vector3 LocalPosition3D
    {
      get => this._localPosition3D;
      set
      {
        if (!this.CanChange3D || !(this._localPosition3D != value))
          return;
        this.ChangeLocalPosition3D(value);
      }
    }

    public Vector2 LocalPosition2D
    {
      get => this._localPosition2D;
      set
      {
        if (!this.CanChange2D || !(this._localPosition2D != value))
          return;
        this.ChangeLocalPosition2D(value);
      }
    }

    public Quaternion LocalRotation3D
    {
      get => this._localRotation3D;
      set
      {
        if (!this.CanChange3D || !(this._localRotation3D.eulerAngles != value.eulerAngles))
          return;
        this.ChangeLocalRotation3D(value);
      }
    }

    public Quaternion LocalRotation2D => this._localRotation2D;

    public float LocalRotation2DDegrees
    {
      get => this._localRotation2DDegrees;
      set
      {
        if (!this.CanChange2D || (double) this._localRotation2DDegrees == (double) value)
          return;
        this.ChangeLocalRotation2D(value);
      }
    }

    public GizmoTransform()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._rotation3D = Quaternion.identity;
      this._localRotation3D = Quaternion.identity;
      this._rotation2D = Quaternion.identity;
      this._localRotation2D = Quaternion.identity;
      this._axes3D = new Vector3[3];
      this._axes2D = new Vector2[2];
      this._children = new List<GizmoTransform>(10);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Update3DAxes();
      this.Update2DAxes();
    }

    public static List<GizmoTransform> FilterParentsOnly(IEnumerable<GizmoTransform> transforms)
    {
      if (transforms == null)
        return new List<GizmoTransform>();
      List<GizmoTransform> gizmoTransformList = new List<GizmoTransform>(10);
      foreach (GizmoTransform transform1 in transforms)
      {
        bool flag = false;
        foreach (GizmoTransform transform2 in transforms)
        {
          if (transform2 != transform1 && transform1.IsChildOf(transform2))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          gizmoTransformList.Add(transform1);
      }
      return gizmoTransformList;
    }

    public PlaneQuadrantId Get3DQuadrantFacingCamera(PlaneId planeId, Camera camera)
    {
      int firstAxisIndex = PlaneIdHelper.PlaneIdToFirstAxisIndex(planeId);
      int secondAxisIndex = PlaneIdHelper.PlaneIdToSecondAxisIndex(planeId);
      AxisSign axisSign1 = AxisSign.Positive;
      AxisSign axisSign2 = AxisSign.Positive;
      Vector3 axis3D1 = this.GetAxis3D(firstAxisIndex, axisSign1);
      Vector3 axis3D2 = this.GetAxis3D(secondAxisIndex, axisSign2);
      if (!camera.IsPointFacingCamera(this.Position3D, axis3D1))
        axisSign1 = AxisSign.Negative;
      if (!camera.IsPointFacingCamera(this.Position3D, axis3D2))
        axisSign2 = AxisSign.Negative;
      return PlaneIdHelper.GetQuadrantFromAxesSigns(planeId, axisSign1, axisSign2);
    }

    public void Rotate3D(Quaternion rotation) => this.Rotation3D = rotation * this._rotation3D;

    public void Rotate2D(float rotation) => this.Rotation2DDegrees += rotation;

    public void Rotate2D(Quaternion rotation) => this.Rotate2D(rotation.ConvertTo2DRotation());

    public Vector3 TransformVector3D(Vector3 vec) => this._rotation3D * vec;

    public Vector2 TransformVector2D(Vector2 vec) => (Vector2) (this._rotation2D * (Vector3) vec);

    public Vector3 TransformNormal3D(Vector3 normal) => (this._rotation3D * normal).normalized;

    public Vector2 TransformNormal2D(Vector2 normal)
    {
      return (Vector2) (this._rotation2D * (Vector3) normal).normalized;
    }

    public Vector3 InverseTransformNormal3D(Vector3 normal)
    {
      return (Quaternion.Inverse(this._rotation3D) * normal).normalized;
    }

    public Vector2 InverseTransformNormal2D(Vector2 normal)
    {
      return (Vector2) (Quaternion.Inverse(this._rotation2D) * (Vector3) normal).normalized;
    }

    public Vector3 TransformPoint3D(Vector3 point) => this._rotation3D * point + this._position3D;

    public Vector2 TransformPoint2D(Vector2 point)
    {
      return (Vector2) (this._rotation2D * (Vector3) point) + this._position2D;
    }

    public Vector3 InverseTransformPoint3D(Vector3 point)
    {
      return Quaternion.Inverse(this._rotation3D) * (point - this._position3D);
    }

    public Vector2 InverseTransformPoint2D(Vector2 point)
    {
      return (Vector2) (Quaternion.Inverse(this._rotation2D) * (Vector3) (point - this._position2D));
    }

    public void AlignAxis3D(int axisIndex, AxisSign axisSign, Vector3 axis)
    {
      if (!this.CanChange3D)
        return;
      Vector3 axis3D1 = this.GetAxis3D(axisIndex, axisSign);
      Vector3 axis3D2 = this.GetAxis3D((axisIndex + 1) % 3, axisSign);
      this.Rotation3D = QuaternionEx.FromToRotation3D(axis3D1, axis, axis3D2) * this._rotation3D;
    }

    public void AlignAxis2D(int axisIndex, AxisSign axisSign, Vector2 axis)
    {
      if (!this.CanChange3D)
        return;
      this.ChangeRotation2D(this._rotation2DDegrees + QuaternionEx.FromToRotation2D(this.GetAxis2D(axisIndex, axisSign), axis).ConvertTo2DRotation());
    }

    public bool IsChildOf(GizmoTransform transform)
    {
      GizmoTransform parent = this.Parent;
      while (parent != transform && parent != null)
        parent = parent.Parent;
      return parent != null;
    }

    public void SetParent(GizmoTransform newParent)
    {
      if (!this.CanChange3D || this._parent == newParent)
        return;
      this._parent?._children.Remove(this);
      this._parent = newParent;
      this._parent._children.Add(this);
      this.OnParentChanged();
    }

    public Vector3 GetAxis3D(AxisDescriptor axisDesc)
    {
      Vector3 axis3D = this._axes3D[axisDesc.Index];
      if (axisDesc.IsNegative)
        axis3D = -axis3D;
      return axis3D;
    }

    public Vector3 GetAxis3D(int axisIndex, AxisSign axisSign)
    {
      Vector3 axis3D = this._axes3D[axisIndex];
      if (axisSign == AxisSign.Negative)
        axis3D = -axis3D;
      return axis3D;
    }

    public Vector2 GetAxis2D(AxisDescriptor axisDesc)
    {
      Vector2 axis2D = this._axes2D[axisDesc.Index];
      if (axisDesc.IsNegative)
        axis2D = -axis2D;
      return axis2D;
    }

    public Vector2 GetAxis2D(int axisIndex, AxisSign axisSign)
    {
      Vector2 axis2D = this._axes2D[axisIndex];
      if (axisSign == AxisSign.Negative)
        axis2D = -axis2D;
      return axis2D;
    }

    public Vector3[] GetAxes3D() => this._axes3D.Clone() as Vector3[];

    public Vector2[] GetAxes2D() => this._axes2D.Clone() as Vector2[];

    public Plane GetPlane3D(PlaneId planeId, PlaneQuadrantId planeQuadrantId)
    {
      PlaneDescriptor planeDescriptor = new PlaneDescriptor(planeId, planeQuadrantId);
      return new Plane(Vector3.Normalize(Vector3.Cross(this.GetAxis3D(planeDescriptor.FirstAxisDescriptor), this.GetAxis3D(planeDescriptor.SecondAxisDescriptor))), this.Position3D);
    }

    public Plane GetPlane3D(PlaneDescriptor planeDesc)
    {
      return new Plane(Vector3.Normalize(Vector3.Cross(this.GetAxis3D(planeDesc.FirstAxisDescriptor), this.GetAxis3D(planeDesc.SecondAxisDescriptor))), this.Position3D);
    }

    private void ChangePosition3D(Vector3 position)
    {
      this._position3D = position;
      this.OnPosition3DChanged();
    }

    private void ChangePosition2D(Vector2 position)
    {
      this._position2D = position;
      this.OnPosition2DChanged();
    }

    private void ChangeRotation3D(Quaternion rotation)
    {
      this._rotation3D = QuaternionEx.Normalize(rotation);
      this.OnRotation3DChanged();
    }

    private void ChangeRotation2D(float rotation)
    {
      this._rotation2DDegrees = rotation % 360f;
      this._rotation2D = QuaternionEx.Normalize(Quaternion.AngleAxis(this._rotation2DDegrees, Vector3.forward));
      this.OnRotation2DChanged();
    }

    private void ChangeRotation2D(Quaternion rotation)
    {
      this._rotation2D = QuaternionEx.Normalize(rotation);
      this.OnRotation2DChanged();
    }

    private void ChangeLocalPosition3D(Vector3 localPosition)
    {
      this._localPosition3D = localPosition;
      this.OnLocalPosition3DChanged();
    }

    private void ChangeLocalPosition2D(Vector2 localPosition)
    {
      this._localPosition2D = localPosition;
      this.OnLocalPosition2DChanged();
    }

    private void ChangeLocalRotation3D(Quaternion localRotation)
    {
      this._localRotation3D = QuaternionEx.Normalize(localRotation);
      this.OnLocalRotation3DChanged();
    }

    private void ChangeLocalRotation2D(float localRotation)
    {
      this._localRotation2DDegrees = localRotation;
      this._localRotation2D = QuaternionEx.Normalize(Quaternion.AngleAxis(this._localRotation2DDegrees, Vector3.forward));
      this.OnLocalRotation2DChanged();
    }

    private void ChangeLocalRotation2D(Quaternion localRotation)
    {
      this._localRotation2D = QuaternionEx.Normalize(localRotation);
      this.OnLocalRotation2DChanged();
    }

    private void OnParentChanged()
    {
      if (this._parent == null)
      {
        this._localPosition3D = this._position3D;
        this._localRotation3D = this._rotation3D;
        this._localPosition2D = this._position2D;
        this._localRotation2D = this._rotation2D;
        this._localRotation2DDegrees = this._rotation2DDegrees;
      }
      else
      {
        this._localPosition3D = Quaternion.Inverse(this._parent._rotation3D) * (this._position3D - this._parent._position3D);
        this._localRotation3D = QuaternionEx.Normalize(Quaternion.Inverse(this._parent._rotation3D) * this._rotation3D);
        this._localPosition2D = (Vector2) (Quaternion.Inverse(this._parent._rotation2D) * (Vector3) (this._position2D - this._parent._position2D));
        this._localRotation2D = QuaternionEx.Normalize(Quaternion.Inverse(this._parent._rotation2D) * this._rotation2D);
        this._localRotation2DDegrees = this._localRotation2D.ConvertTo2DRotation();
      }
      this.UpdateChildTransforms3D();
      this.UpdateChildTransforms2D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.ParentChange, GizmoDimension.None));
    }

    private void OnPosition3DChanged()
    {
      this._localPosition3D = this._parent != null ? Quaternion.Inverse(this._parent._rotation3D) * (this._position3D - this._parent._position3D) : this._position3D;
      this.UpdateChildTransforms3D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim3D));
    }

    private void OnPosition2DChanged()
    {
      this._localPosition2D = this._parent != null ? (Vector2) (Quaternion.Inverse(this._parent._rotation2D) * (Vector3) (this._position2D - this._parent._position2D)) : this._position2D;
      this.UpdateChildTransforms2D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim2D));
    }

    private void OnLocalPosition3DChanged()
    {
      this._position3D = this._parent != null ? this._parent.Rotation3D * this._localPosition3D + this._parent.Position3D : this._localPosition3D;
      this.UpdateChildTransforms3D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim3D));
    }

    private void OnLocalPosition2DChanged()
    {
      this._position2D = this._parent != null ? (Vector2) (this._parent.Rotation2D * (Vector3) this._localPosition2D) + this._parent.Position2D : this._localPosition2D;
      this.UpdateChildTransforms2D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim2D));
    }

    private void OnRotation3DChanged()
    {
      this._localRotation3D = this._parent != null ? QuaternionEx.Normalize(Quaternion.Inverse(this._parent._rotation3D) * this._rotation3D) : this._rotation3D;
      this.Update3DAxes();
      this.UpdateChildTransforms3D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim3D));
    }

    private void OnRotation2DChanged()
    {
      this._localRotation2D = this._parent != null ? QuaternionEx.Normalize(Quaternion.Inverse(this._parent._rotation2D) * this._rotation2D) : this._rotation2D;
      this._localRotation2DDegrees = this._localRotation2D.ConvertTo2DRotation();
      this.Update2DAxes();
      this.UpdateChildTransforms2D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim2D));
    }

    private void OnLocalRotation3DChanged()
    {
      this._rotation3D = this._parent != null ? QuaternionEx.Normalize(this._parent._rotation3D * this._localRotation3D) : this._localRotation3D;
      this.Update3DAxes();
      this.UpdateChildTransforms3D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim3D));
    }

    private void OnLocalRotation2DChanged()
    {
      this._rotation2D = this._parent != null ? QuaternionEx.Normalize(this._parent._rotation2D * this._localRotation2D) : this._localRotation2D;
      this._rotation2DDegrees = this._rotation2D.ConvertTo2DRotation();
      this.Update2DAxes();
      this.UpdateChildTransforms2D();
      this.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim2D));
    }

    private void UpdateChildTransforms3D()
    {
      foreach (GizmoTransform child in this._children)
      {
        child._position3D = this._rotation3D * child._localPosition3D + this._position3D;
        child._rotation3D = QuaternionEx.Normalize(this._rotation3D * child._localRotation3D);
        child.Update3DAxes();
        child.UpdateChildTransforms3D();
        child.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim3D));
      }
    }

    private void UpdateChildTransforms2D()
    {
      foreach (GizmoTransform child in this._children)
      {
        Vector2 vector2 = (Vector2) (this._rotation2D * (Vector3) child._localPosition2D);
        child._position2D = vector2 + this._position2D;
        child._rotation2D = QuaternionEx.Normalize(this._rotation2D * child._localRotation2D);
        child._rotation2DDegrees = child._rotation2D.ConvertTo2DRotation();
        child.Update2DAxes();
        child.UpdateChildTransforms2D();
        child.OnChanged(new GizmoTransform.ChangeData(GizmoTransform.ChangeReason.TRSChange, GizmoDimension.Dim2D));
      }
    }

    private void OnChanged(GizmoTransform.ChangeData changeData)
    {
      if (changeData.TRSDimension == GizmoDimension.Dim3D)
      {
        this._firingChanged3DEvent = true;
        if (this.Changed != null)
          this.Changed(this, changeData);
        this._firingChanged3DEvent = false;
      }
      else
      {
        this._firingChanged2DEvent = true;
        if (this.Changed != null)
          this.Changed(this, changeData);
        this._firingChanged2DEvent = false;
      }
    }

    private void Update3DAxes()
    {
      Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, this._rotation3D, Vector3.one);
      this._axes3D[0] = matrix.GetNormalizedAxis(0);
      this._axes3D[1] = matrix.GetNormalizedAxis(1);
      this._axes3D[2] = matrix.GetNormalizedAxis(2);
    }

    private void Update2DAxes()
    {
      Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, this._rotation2D, Vector3.one);
      this._axes2D[0] = (Vector2) matrix.GetNormalizedAxis(0);
      this._axes2D[1] = (Vector2) matrix.GetNormalizedAxis(1);
    }

    public enum ChangeReason
    {
      TRSChange,
      ParentChange,
    }

    public struct ChangeData
    {
      public GizmoTransform.ChangeReason ChangeReason;
      public GizmoDimension TRSDimension;

      public ChangeData(GizmoTransform.ChangeReason changeReason, GizmoDimension trsDimension)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.ChangeReason = changeReason;
        this.TRSDimension = trsDimension;
      }
    }
  }
}
