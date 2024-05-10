// Decompiled with JetBrains decompiler
// Type: RTG.ObjectPool
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
  public class ObjectPool
  {
    private GameObject _sourceObject;
    private ObjectPool.GrowMode _growMode;
    private int _growAmount;
    private List<GameObject> _pooledObjects;
    private Transform _pooledParent;

    public ObjectPool.GrowMode PoolGrowMode
    {
      get => this._growMode;
      set => this._growMode = value;
    }

    public int GrowAmount
    {
      get => this._growAmount;
      set => this._growAmount = Mathf.Max(1, value);
    }

    public ObjectPool(GameObject sourceObject, int numPooled, ObjectPool.GrowMode growMode)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._growMode = ObjectPool.GrowMode.ByAmount;
      this._growAmount = 50;
      this._pooledObjects = new List<GameObject>(100);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sourceObject = sourceObject;
      this._growMode = growMode;
    }

    public void SetPooledObjectsParent(Transform parent)
    {
      this._pooledParent = parent;
      foreach (GameObject pooledObject in this._pooledObjects)
      {
        if ((Object) pooledObject != (Object) null && !pooledObject.activeSelf)
          pooledObject.transform.SetParent(parent, false);
      }
    }

    public GameObject GetPooledObject()
    {
      foreach (GameObject pooledObject in this._pooledObjects)
      {
        if ((Object) pooledObject != (Object) null && !pooledObject.activeSelf)
        {
          pooledObject.SetActive(true);
          if ((Object) this._pooledParent != (Object) null)
            pooledObject.transform.SetParent(this._pooledParent, false);
          return pooledObject;
        }
      }
      if (this.PoolGrowMode == ObjectPool.GrowMode.None)
        return (GameObject) null;
      int count = this._pooledObjects.Count;
      this.Grow();
      GameObject pooledObject1 = this._pooledObjects[count];
      pooledObject1.SetActive(true);
      if ((Object) this._pooledParent != (Object) null)
        pooledObject1.transform.SetParent(this._pooledParent, false);
      return pooledObject1;
    }

    public void MarkAsUnused(GameObject gameObject)
    {
      if (!((Object) gameObject != (Object) null))
        return;
      gameObject.SetActive(false);
    }

    public void MarkAllAsUnused()
    {
      foreach (GameObject pooledObject in this._pooledObjects)
        this.MarkAsUnused(pooledObject);
    }

    private void Grow()
    {
      if (this.PoolGrowMode == ObjectPool.GrowMode.None)
        return;
      if (this.PoolGrowMode == ObjectPool.GrowMode.ByAmount)
      {
        for (int index = 0; index < this.GrowAmount; ++index)
          this.CreatePooledObject();
      }
      else
      {
        if (this.PoolGrowMode != ObjectPool.GrowMode.Increment)
          return;
        this.CreatePooledObject();
      }
    }

    private GameObject CreatePooledObject()
    {
      GameObject pooledObject = Object.Instantiate<GameObject>(this._sourceObject);
      this._pooledObjects.Add(pooledObject);
      if ((Object) this._pooledParent != (Object) null)
        pooledObject.transform.SetParent(this._pooledParent, false);
      pooledObject.SetActive(false);
      return pooledObject;
    }

    public enum GrowMode
    {
      None,
      Increment,
      ByAmount,
    }
  }
}
