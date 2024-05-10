// Decompiled with JetBrains decompiler
// Type: Mafi.ObjectPool2`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Dynamic object pool with max size limit. This class is NOT thread safe. If you intend to use it from multiple
  /// threads please use [ThreadStatic] to have an instance per thread and avoid any locking.
  /// </summary>
  public class ObjectPool2<T> where T : class
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_New;
    private static readonly string s_pcKey_Free;
    /// <summary>Maximum size of the pool.</summary>
    private readonly int m_maxSize;
    /// <summary>
    /// Object factory is stored for the lifetime of the pool. We will call this only when no objects are available
    /// in the pool.
    /// </summary>
    private readonly Func<ObjectPool2<T>, T> m_factory;
    /// <summary>Resets the object for the next use.</summary>
    private readonly Option<Action<T>> m_reset;
    /// <summary>Storage for the pooled objects.</summary>
    private readonly Lyst<T> m_items;

    public ObjectPool2(
      int maxSize,
      Func<ObjectPool2<T>, T> factory,
      Action<T> reset = null,
      int initialSize = 8)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(maxSize).IsPositive();
      this.m_maxSize = maxSize.CheckPositive();
      this.m_factory = factory.CheckNotNull<Func<ObjectPool2<T>, T>>();
      this.m_reset = (Option<Action<T>>) reset;
      this.m_items = new Lyst<T>(initialSize.Min(maxSize));
    }

    /// <summary>
    /// Returns an instance (either pooled or new one). The returned instance was reset using the reset function. It
    /// is responsibility of the caller to return instance to the pool.
    /// </summary>
    public T GetInstance()
    {
      return !this.m_items.IsNotEmpty ? this.getInstanceSlow() : this.m_items.PopLast();
    }

    private T getInstanceSlow()
    {
      T instanceSlow = this.m_factory(this);
      if (this.m_reset.HasValue)
        this.m_reset.Value(instanceSlow);
      return instanceSlow;
    }

    /// <summary>
    /// Returns objects to the pool.
    /// IMPORTANT: The caller is responsible to ensure that he is the only owner of returned instance and no other
    /// object have reference to returned object.
    /// </summary>
    public void ReturnInstance(ref T obj)
    {
      if ((object) obj == null)
      {
        Log.Error("Returning null (" + typeof (T).Name + ") to pool.");
      }
      else
      {
        this.returnInstance(obj);
        obj = default (T);
      }
    }

    public void ReturnInstance(ref Option<T> obj)
    {
      if (obj.IsNone)
        return;
      this.returnInstance(obj.Value);
      obj = Option<T>.None;
    }

    public void ReturnInstanceKeepReference(T obj)
    {
      if ((object) obj == null)
        Log.Error("Returning null (" + typeof (T).Name + ") to pool.");
      else
        this.returnInstance(obj);
    }

    public void Clear()
    {
      this.m_items.Clear();
      this.m_items.TrimExcess();
    }

    private void returnInstance(T obj)
    {
      if (this.m_reset.HasValue)
        this.m_reset.Value(obj);
      if (this.m_items.Count >= this.m_maxSize)
        return;
      this.m_items.Add(obj);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    private void validate(T obj)
    {
      foreach (T obj1 in this.m_items)
      {
        Assert.That<T>(obj1).IsNotNull<T>("Null stored in pool!");
        Assert.That<T>(obj1).IsNotEqualTo<T>(obj, "Returning instance to pool twice!");
      }
    }

    static ObjectPool2()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ObjectPool2<T>.s_pcKey = "ObjectPool2<" + typeof (T).Name + ">: ";
      ObjectPool2<T>.s_pcKey_New = ObjectPool2<T>.s_pcKey + "New";
      ObjectPool2<T>.s_pcKey_Free = ObjectPool2<T>.s_pcKey + "Free";
    }
  }
}
