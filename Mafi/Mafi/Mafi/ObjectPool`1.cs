// Decompiled with JetBrains decompiler
// Type: Mafi.ObjectPool`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Generic implementation of object pooling pattern with predefined pool size limit. The main purpose is that
  /// limited number of frequently used objects can be kept in the pool for further recycling.
  /// 
  /// This class is thread safe and lock-free ( <see cref="T:System.Threading.Interlocked" /> operations are used).
  /// </summary>
  /// <remarks>
  /// 1) It is not the goal to keep all returned objects. Pool is not meant for storage. If there is no space in the
  /// pool, extra returned objects will be dropped.
  /// 
  /// 2) It is implied that if object was obtained from a pool, the caller will return it back in a relatively short
  /// time. Keeping checked out objects for long durations is ok, but reduces usefulness of pooling. Just new up your
  /// own.
  /// 
  /// Not returning objects to the pool in not detrimental to the pool's work, but is a bad practice.
  /// Rationale: If there is no intent for reusing the object, do not use pool - just use "new".
  /// </remarks>
  public class ObjectPool<T> where T : class
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_New;
    private static readonly string s_pcKey_Free;
    private static readonly string s_pcKey_CachedReturnFast;
    private static readonly string s_pcKey_CachedReturnSlow;
    private static readonly string s_pcKey_StoreFast;
    private static readonly string s_pcKey_StoreSlow;
    /// <summary>
    /// Object factory is stored for the lifetime of the pool. We will call this only when pool needs to expand.
    /// </summary>
    private readonly Func<T> m_factory;
    /// <summary>Resets the object for the next use.</summary>
    private readonly Action<T> m_reset;
    /// <summary>
    /// Storage for the pool objects. The first item is stored in a dedicated field because we expect to be able to
    /// satisfy most requests from it.
    /// </summary>
    private readonly ObjectPool<T>.Element[] m_items;
    private T m_firstItem;

    /// <summary>
    /// Creates a new thread-safe lock-free object pool. The total size should not be too small to be able to pool
    /// enough object but also not big since we perform linear search for available objects.
    /// </summary>
    public ObjectPool(int size, Func<T> factory, Action<T> reset)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(size).IsPositive();
      this.m_factory = factory.CheckNotNull<Func<T>>();
      this.m_reset = reset.CheckNotNull<Action<T>>();
      this.m_items = new ObjectPool<T>.Element[size - 1];
    }

    /// <summary>
    /// Note that this may be imprecise due to threads (no locking).
    /// </summary>
    public int CountStoredInstances()
    {
      int num = (object) this.m_firstItem != null ? 1 : 0;
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        if ((object) this.m_items[index].Value != null)
          ++num;
      }
      return num;
    }

    /// <summary>
    /// Returns an instance (either from pool or new one). The returned instance was resetted using reset function.
    /// NOTE: The caller should perform operation and then return the object to the pool. Instances obtained from the
    /// pool should have very short life-span. For long-living objects create an instance using new.
    /// </summary>
    /// <remarks>
    /// Search strategy is a simple linear probing which is chosen for it cache-friendliness. Note that Free will try
    /// to store recycled objects close to the start thus statistically reducing how far we will typically search.
    /// </remarks>
    public T GetInstance()
    {
      T comparand = this.m_firstItem;
      if ((object) comparand == null || (object) comparand != (object) Interlocked.CompareExchange<T>(ref this.m_firstItem, default (T), comparand))
        comparand = this.getInstanceSlow();
      return comparand;
    }

    private T getInstanceSlow()
    {
      ObjectPool<T>.Element[] items = this.m_items;
      for (int index = 0; index < items.Length; ++index)
      {
        T comparand = items[index].Value;
        if ((object) comparand != null && (object) comparand == (object) Interlocked.CompareExchange<T>(ref items[index].Value, default (T), comparand))
          return comparand;
      }
      T instanceSlow = this.m_factory();
      this.m_reset(instanceSlow);
      return instanceSlow;
    }

    /// <summary>
    /// Returns objects to the pool.
    /// IMPORTANT: The caller is responsible to ensure that he is the only owner of returned instance and no other
    /// object have reference to returned object.
    /// </summary>
    /// <remarks>
    /// Search strategy is a simple linear probing which is chosen for it cache-friendliness. Note that Free will try
    /// to store recycled objects close to the start thus statistically reducing how far we will typically search in
    /// Allocate.
    /// </remarks>
    public void ReturnInstance(T obj)
    {
      this.m_reset(obj);
      if ((object) this.m_firstItem == null)
        this.m_firstItem = obj;
      else
        this.returnInstanceSlow(obj);
    }

    public void Clear() => Array.Clear((Array) this.m_items, 0, this.m_items.Length);

    private void returnInstanceSlow(T obj)
    {
      ObjectPool<T>.Element[] items = this.m_items;
      for (int index = 0; index < items.Length; ++index)
      {
        if ((object) items[index].Value == null)
        {
          items[index].Value = obj;
          break;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    private void validate(T obj)
    {
      Assert.That<T>(obj).IsNotNull<T>("Returning null to pool!");
      Assert.That<T>(this.m_firstItem).IsNotEqualTo<T>(obj, "Returning instance to pool twice!");
      foreach (ObjectPool<T>.Element element in this.m_items)
      {
        T obj1 = element.Value;
        if ((object) obj1 == null)
          break;
        Assert.That<T>(obj1).IsNotEqualTo<T>(obj, "Returning instance to pool twice!");
      }
    }

    static ObjectPool()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ObjectPool<T>.s_pcKey = "ObjectPool<" + typeof (T).Name + ">: ";
      ObjectPool<T>.s_pcKey_New = ObjectPool<T>.s_pcKey + "New";
      ObjectPool<T>.s_pcKey_Free = ObjectPool<T>.s_pcKey + "Free";
      ObjectPool<T>.s_pcKey_CachedReturnFast = ObjectPool<T>.s_pcKey + "CachedReturn Fast";
      ObjectPool<T>.s_pcKey_CachedReturnSlow = ObjectPool<T>.s_pcKey + "CachedReturn Slow";
      ObjectPool<T>.s_pcKey_StoreFast = ObjectPool<T>.s_pcKey + "Store Fast";
      ObjectPool<T>.s_pcKey_StoreSlow = ObjectPool<T>.s_pcKey + "Store Slow";
    }

    /// <summary>
    /// Struct for holding the instances so we can do interlocked compare-exchange operation.
    /// </summary>
    [DebuggerDisplay("{Value,nq}")]
    private struct Element
    {
      internal T Value;
    }
  }
}
