// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MbPool`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Specialized object pool for instances of <see cref="T:UnityEngine.MonoBehaviour" />.
  /// </summary>
  public class MbPool<T> where T : MonoBehaviour
  {
    private readonly GameObjectPool m_pool;
    private readonly Action<T> m_reset;

    public bool IsEmpty => this.m_pool.IsEmpty;

    public MbPool(string name, int size, Func<MbPool<T>, T> factory, Action<T> reset)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      MbPool<T> mbPool = this;
      this.m_reset = reset;
      this.m_pool = new GameObjectPool(name, size, (Func<GameObject>) (() => factory(mbPool).gameObject), (Action<GameObject>) (x => { }));
    }

    public MbPool(string name, int size, Func<T> factory, Action<T> reset)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(name, size, (Func<MbPool<T>, T>) (x => factory()), reset);
    }

    public T GetInstance() => this.m_pool.GetInstance().GetComponent<T>();

    /// <summary>
    /// Returns instance to the pool. It resets parent GO and sets it inactive. Given <paramref name="mb" /> is set to
    /// null.
    /// </summary>
    public void ReturnInstance(ref T mb)
    {
      GameObject gameObject = mb.gameObject;
      this.m_pool.ReturnInstance(ref gameObject);
      this.m_reset(mb);
      mb = default (T);
    }

    public void ReturnInstance(ref Option<T> mb)
    {
      if (mb.IsNone)
        return;
      GameObject gameObject = mb.Value.gameObject;
      this.m_pool.ReturnInstance(ref gameObject);
      this.m_reset(mb.Value);
      mb = (Option<T>) Option.None;
    }

    public void ReturnInstanceKeepReference(T mb) => this.ReturnInstance(ref mb);

    public void PreFill(int count) => this.m_pool.PreFill(count);
  }
}
