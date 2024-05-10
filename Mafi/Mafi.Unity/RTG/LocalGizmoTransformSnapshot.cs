// Decompiled with JetBrains decompiler
// Type: RTG.LocalGizmoTransformSnapshot
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
  public class LocalGizmoTransformSnapshot
  {
    private GizmoTransform _transform;
    private GizmoTransform _parentTransform;
    private Vector3 _localPosition3D;
    private Quaternion _localRotation3D;
    private Vector2 _localPosition2D;
    private float _localRotation2D_Degrees;

    public GizmoTransform Transform => this._transform;

    public static List<LocalGizmoTransformSnapshot> GetSnapshotCollection(IEnumerable<Gizmo> gizmos)
    {
      if (gizmos == null)
        return new List<LocalGizmoTransformSnapshot>();
      List<LocalGizmoTransformSnapshot> snapshotCollection = new List<LocalGizmoTransformSnapshot>(20);
      foreach (Gizmo gizmo in gizmos)
      {
        LocalGizmoTransformSnapshot transformSnapshot = new LocalGizmoTransformSnapshot();
        transformSnapshot.Snapshot(gizmo.Transform);
        snapshotCollection.Add(transformSnapshot);
      }
      return snapshotCollection;
    }

    public void Snapshot(GizmoTransform transform)
    {
      if (transform == null)
        return;
      this._transform = transform;
      this._parentTransform = transform.Parent;
      this._localPosition3D = transform.LocalPosition3D;
      this._localRotation3D = transform.LocalRotation3D;
      this._localPosition2D = transform.LocalPosition2D;
      this._localRotation2D_Degrees = transform.LocalRotation2DDegrees;
    }

    public void Apply()
    {
      if (this._transform == null)
        return;
      if (this._parentTransform != null)
      {
        this._transform.LocalPosition3D = this._localPosition3D;
        this._transform.LocalRotation3D = this._localRotation3D;
        this._transform.LocalPosition2D = this._localPosition2D;
        this._transform.LocalRotation2DDegrees = this._localRotation2D_Degrees;
      }
      else
      {
        this._transform.Position3D = this._localPosition3D;
        this._transform.Rotation3D = this._localRotation3D;
        this._transform.Position2D = this._localPosition2D;
        this._transform.Rotation2DDegrees = this._localRotation2D_Degrees;
      }
    }

    public LocalGizmoTransformSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
