// Decompiled with JetBrains decompiler
// Type: RTG.MeshTransform
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MeshTransform
  {
    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;

    public Vector3 Position => this._position;

    public Quaternion Rotation => this._rotation;

    public Vector3 Scale => this._scale;

    public MeshTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._position = position;
      this._rotation = rotation;
      this._scale = scale;
    }

    public MeshTransform(Transform transform)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._position = transform.position;
      this._rotation = transform.rotation;
      this._scale = transform.lossyScale;
    }

    public OBB InverseTransformOBB(OBB obb)
    {
      return new OBB(this.InverseTransformPoint(obb.Center), Vector3.Scale(this._scale.GetInverse(), obb.Size))
      {
        Rotation = Quaternion.Inverse(this._rotation) * obb.Rotation
      };
    }

    public Vector3 TransformPoint(Vector3 point)
    {
      return this._rotation * Vector3.Scale(point, this._scale) + this._position;
    }

    public Vector3 InverseTransformPoint(Vector3 point)
    {
      return Vector3.Scale(this._scale.GetInverse(), Quaternion.Inverse(this._rotation) * (point - this._position));
    }
  }
}
