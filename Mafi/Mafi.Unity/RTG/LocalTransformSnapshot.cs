// Decompiled with JetBrains decompiler
// Type: RTG.LocalTransformSnapshot
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
  public class LocalTransformSnapshot
  {
    private Transform _transform;
    private Transform _parentTransform;
    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private Vector3 _localScale;

    public Transform Transform => this._transform;

    public static List<LocalTransformSnapshot> GetSnapshotCollection(
      IEnumerable<GameObject> gameObjects)
    {
      if (gameObjects == null)
        return new List<LocalTransformSnapshot>();
      List<LocalTransformSnapshot> snapshotCollection = new List<LocalTransformSnapshot>(20);
      foreach (GameObject gameObject in gameObjects)
      {
        if ((Object) gameObject != (Object) null)
        {
          LocalTransformSnapshot transformSnapshot = new LocalTransformSnapshot();
          transformSnapshot.Snapshot(gameObject.transform);
          snapshotCollection.Add(transformSnapshot);
        }
      }
      return snapshotCollection;
    }

    public void Snapshot(Transform transform)
    {
      if ((Object) transform == (Object) null)
        return;
      this._transform = transform;
      this._parentTransform = transform.parent;
      this._localPosition = transform.localPosition;
      this._localRotation = transform.localRotation;
      this._localScale = transform.localScale;
    }

    public bool SameAs(Transform transform)
    {
      return (Object) this._parentTransform == (Object) transform.parent && this._localPosition == transform.localPosition && this._localRotation == transform.localRotation && this._localScale == transform.localScale;
    }

    public void Apply()
    {
      if ((Object) this._transform == (Object) null)
        return;
      if ((Object) this._parentTransform != (Object) null)
      {
        this._transform.localPosition = this._localPosition;
        this._transform.localRotation = this._localRotation;
        this._transform.localScale = this._localScale;
      }
      else
      {
        this._transform.position = this._localPosition;
        this._transform.rotation = this._localRotation;
        this._transform.localScale = this._localScale;
      }
    }

    public LocalTransformSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
