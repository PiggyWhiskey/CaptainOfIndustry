// Decompiled with JetBrains decompiler
// Type: Mafi.ArrayPool`1
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
  /// Pools for small arrays, currently up to size 255. Larger arrays are always allocated and never stored. For
  /// details see <see cref="T:Mafi.ObjectPool`1" />.
  /// </summary>
  public static class ArrayPool<T>
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_Get;
    private static readonly string s_pcKey_NewPool;
    private static readonly string s_pcKey_Empty;
    private static readonly string s_pcKey_NewTooLarge;
    private static readonly string s_pcKey_Return;
    private static readonly string s_pcKey_ReturnEmptyOrLarge;
    public const int POOL_SIZE = 8;
    public const int MAX_ARRAY_SIZE = 255;
    private static readonly ObjectPool<T[]>[] s_pools;

    /// <summary>Returns pooled array of requested size.</summary>
    public static T[] Get(int size)
    {
      Assert.That<int>(size).IsNotNegative();
      ObjectPool<T[]>[] pools = ArrayPool<T>.s_pools;
      if (size <= 0)
        return Array.Empty<T>();
      if (size >= pools.Length)
        return new T[size];
      ObjectPool<T[]> objectPool = pools[size];
      if (objectPool == null)
      {
        objectPool = new ObjectPool<T[]>(8, (Func<T[]>) (() => new T[size]), (Action<T[]>) (arr => Array.Clear((Array) arr, 0, size)));
        pools[size] = objectPool;
      }
      return objectPool.GetInstance();
    }

    /// <summary>
    /// Returns pooled array of length 1 with given item at index 0.
    /// </summary>
    public static T[] GetAndInit(T value)
    {
      T[] andInit = ArrayPool<T>.Get(1);
      andInit[0] = value;
      return andInit;
    }

    /// <summary>
    /// Returns pooled array of length 2 with given items at indices 0 and 1 respectively.
    /// </summary>
    public static T[] GetAndInit(T value1, T value2)
    {
      T[] andInit = ArrayPool<T>.Get(2);
      andInit[0] = value1;
      andInit[1] = value2;
      return andInit;
    }

    /// <summary>
    /// Returns array to the pool. Please make sure than only arrays that are no longer in use are returned. The
    /// array being returned to pool should come from <see cref="T:Mafi.ArrayPool`1" />, but not necessarily.
    /// 
    /// TODO: This is slow because of Array.Clear and because of inefficient pool insertion. Improve this.
    /// </summary>
    public static void ReturnToPool(T[] arr)
    {
      if (arr == null)
      {
        Log.Error("Trying to return empty array to pool");
      }
      else
      {
        ObjectPool<T[]>[] pools = ArrayPool<T>.s_pools;
        int length = arr.Length;
        if (length <= 0 || length >= pools.Length || pools[length] == null)
          return;
        pools[length].ReturnInstance(arr);
      }
    }

    static ArrayPool()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ArrayPool<T>.s_pcKey = "ArrayPool<" + typeof (T).Name + ">: ";
      ArrayPool<T>.s_pcKey_Get = ArrayPool<T>.s_pcKey + "Get";
      ArrayPool<T>.s_pcKey_NewPool = ArrayPool<T>.s_pcKey + "New pool";
      ArrayPool<T>.s_pcKey_Empty = ArrayPool<T>.s_pcKey + "Empty";
      ArrayPool<T>.s_pcKey_NewTooLarge = ArrayPool<T>.s_pcKey + "New - too large";
      ArrayPool<T>.s_pcKey_Return = ArrayPool<T>.s_pcKey + "Return";
      ArrayPool<T>.s_pcKey_ReturnEmptyOrLarge = ArrayPool<T>.s_pcKey + "Return - empty or large";
      ArrayPool<T>.s_pools = new ObjectPool<T[]>[256];
    }
  }
}
