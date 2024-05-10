// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GameObjectPool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Specialized object pool for instances of <see cref="T:UnityEngine.GameObject" />. Unlike <see cref="T:Mafi.ObjectPool`1" /> this class
  /// is not thread safe and is prepared to store items more long term.
  /// </summary>
  public class GameObjectPool
  {
    private readonly Lyst<GameObject> m_pool;
    private readonly Func<GameObject> m_factory;
    private readonly Action<GameObject> m_reset;
    private readonly string m_pcKey_ReturnNew;
    private readonly string m_pcKey_ReturnPooled;
    private readonly string m_pcKey_StoreDestroy;
    private readonly string m_pcKey_StorePooled;

    public bool IsEmpty => this.m_pool.IsEmpty;

    public GameObjectPool(
      string name,
      int size,
      Func<GameObject> factory,
      Action<GameObject> reset)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(size).IsPositive();
      this.m_pool = new Lyst<GameObject>(size);
      this.m_reset = reset.CheckNotNull<Action<GameObject>>();
      this.m_factory = factory.CheckNotNull<Func<GameObject>>();
      string str = "GameObjectPool<" + name + ">: ";
      this.m_pcKey_ReturnNew = str + "Return New";
      this.m_pcKey_ReturnPooled = str + "Return Pooled";
      this.m_pcKey_StoreDestroy = str + "Store Destroy";
      this.m_pcKey_StorePooled = str + "Store Pooled";
    }

    public GameObject GetInstance()
    {
      GameObject instance;
      if (this.m_pool.IsEmpty)
      {
        instance = this.m_factory();
        this.m_reset(instance);
      }
      else
      {
        instance = this.m_pool.PopLast();
        Assert.That<bool>(instance.activeSelf).IsFalse("Somebody activated GO in the pool, don't touch my tralala!");
      }
      instance.SetActive(true);
      return instance;
    }

    /// <summary>
    /// Returns instance to the pool. It resets parent of given GO and sets it inactive. Given <paramref name="go" />
    /// is set to null.
    /// </summary>
    public void ReturnInstance(ref GameObject go)
    {
      if (!(bool) (UnityEngine.Object) go)
      {
        Log.Error("Returning invalid object to pool!");
      }
      else
      {
        go.SetActive(false);
        go.transform.SetParent((Transform) null, false);
        if (this.m_pool.Count >= this.m_pool.Capacity)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) go);
        }
        else
        {
          this.m_reset(go);
          this.m_pool.Add(go);
          go = (GameObject) null;
        }
      }
    }

    public void PreFill(int count)
    {
      this.m_pool.EnsureCapacity(count);
      for (int count1 = this.m_pool.Count; count1 < count; ++count1)
      {
        GameObject gameObject = this.m_factory();
        gameObject.SetActive(false);
        this.m_reset(gameObject);
        this.m_pool.Add(gameObject);
      }
    }
  }
}
