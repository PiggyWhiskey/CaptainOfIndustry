// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.PooledArray`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Collections
{
  public readonly struct PooledArray<T>
  {
    public static readonly PooledArray<T> Empty;
    public readonly T[] BackingArray;

    public bool IsValid => this.BackingArray != null;

    public int Length => this.BackingArray.Length;

    private PooledArray(T[] array)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.BackingArray = array;
    }

    public T this[int i]
    {
      get => this.BackingArray[i];
      set => this.BackingArray[i] = value;
    }

    /// <summary>
    /// Returns internal array to pool. It is callers responsibility to set this struct to default value to prevent
    /// accidental usage or multiple returns.
    /// </summary>
    /// <remarks>
    /// For efficiency we keep this struct readonly so that it can be used and accessed without defensive copies.
    /// Unfortunately, this prevents us from storing a state of whether this method was already called. But even if
    /// we could, it would not be fool proof since structs are copied by value everywhere.
    /// </remarks>
    public void ReturnToPool() => this.BackingArray.ReturnToPool<T>();

    public static PooledArray<T> GetPooled(int length)
    {
      return new PooledArray<T>(ArrayPool<T>.Get(length));
    }

    public void CopyFrom(T[] sourceArray, int size)
    {
      if (this.BackingArray == null)
        Log.Error("Copying from empty pooled array.");
      else
        Array.Copy((Array) sourceArray, 0, (Array) this.BackingArray, 0, size);
    }

    static PooledArray()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      PooledArray<T>.Empty = new PooledArray<T>(Array.Empty<T>());
    }
  }
}
