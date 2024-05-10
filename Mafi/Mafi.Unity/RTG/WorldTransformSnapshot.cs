// Decompiled with JetBrains decompiler
// Type: RTG.WorldTransformSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class WorldTransformSnapshot
  {
    private Vector3 _worldPosition;
    private Quaternion _worldRotation;
    private Vector3 _worldScale;

    public Vector3 WorldPosition => this._worldPosition;

    public Quaternion WorldRotation => this._worldRotation;

    public Vector3 WorldScale => this._worldScale;

    public void Snaphot(Transform transform)
    {
      if ((Object) transform == (Object) null)
        return;
      this._worldPosition = transform.position;
      this._worldRotation = transform.rotation;
      this._worldScale = transform.lossyScale;
    }

    public bool SameAs(Transform transform)
    {
      return this._worldPosition == transform.position && this._worldRotation == transform.rotation && this._worldScale == transform.lossyScale;
    }

    public WorldTransformSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
