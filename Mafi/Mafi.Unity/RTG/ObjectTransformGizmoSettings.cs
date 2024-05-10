// Decompiled with JetBrains decompiler
// Type: RTG.ObjectTransformGizmoSettings
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class ObjectTransformGizmoSettings : Settings
  {
    [SerializeField]
    private int _transformableLayers;
    private HashSet<GameObject> _nonTransformableObjects;

    public int TransformableLayers
    {
      get => this._transformableLayers;
      set => this._transformableLayers = value;
    }

    public bool IsLayerTransformable(int objectLayer)
    {
      return LayerEx.IsLayerBitSet(this._transformableLayers, objectLayer);
    }

    public void SetLayerTransformable(int objectLayer, bool isTransformable)
    {
      if (isTransformable)
        this._transformableLayers = LayerEx.SetLayerBit(this._transformableLayers, objectLayer);
      else
        this._transformableLayers = LayerEx.ClearLayerBit(this._transformableLayers, objectLayer);
    }

    public bool IsObjectTransformable(GameObject gameObject)
    {
      return !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && !this._nonTransformableObjects.Contains(gameObject);
    }

    public void SetObjectTransformable(GameObject gameObject, bool isTransformable)
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return;
      if (isTransformable)
        this._nonTransformableObjects.Remove(gameObject);
      else
        this._nonTransformableObjects.Add(gameObject);
    }

    public void SetObjectCollectionTransformable(
      List<GameObject> gameObjectCollection,
      bool areTransformable)
    {
      foreach (GameObject gameObject in gameObjectCollection)
        this.SetObjectTransformable(gameObject, areTransformable);
    }

    public ObjectTransformGizmoSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._transformableLayers = -1;
      this._nonTransformableObjects = new HashSet<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
