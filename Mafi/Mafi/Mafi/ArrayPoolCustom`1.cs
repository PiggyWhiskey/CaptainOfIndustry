// Decompiled with JetBrains decompiler
// Type: Mafi.ArrayPoolCustom`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Custom non-static array pool for classes that need to pool arrays but don't need thread safety.
  /// </summary>
  public class ArrayPoolCustom<T>
  {
    public const int MAX_ARRAY_SIZE = 100;
    private readonly int m_maxPoolSize;
    private readonly ObjectPool2<T[]>[] m_pools;

    public ArrayPoolCustom(int maxPoolSize)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_pools = new ObjectPool2<T[]>[101];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maxPoolSize = maxPoolSize;
    }

    /// <summary>Returns pooled array of requested size.</summary>
    public T[] Get(int size)
    {
      Assert.That<int>(size).IsNotNegative();
      if (size <= 0)
        return Array.Empty<T>();
      if (size >= 100)
        return new T[size];
      ObjectPool2<T[]> objectPool2 = this.m_pools[size];
      if (objectPool2 == null)
      {
        objectPool2 = new ObjectPool2<T[]>(this.m_maxPoolSize, (Func<ObjectPool2<T[]>, T[]>) (_ => new T[size]), (Action<T[]>) (arr =>
        {
          for (int index = 0; index < arr.Length; ++index)
            arr[index] = default (T);
        }));
        this.m_pools[size] = objectPool2;
      }
      return objectPool2.GetInstance();
    }

    public void ReturnToPool(T[] arr)
    {
      int length = arr.Length;
      if (length <= 0 || length >= 100)
        return;
      ObjectPool2<T[]> pool = this.m_pools[length];
    }
  }
}
