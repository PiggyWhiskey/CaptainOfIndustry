// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.PooledArray
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Collections
{
  public static class PooledArray
  {
    public static PooledArray<T> GetPooled<T>(T value0)
    {
      PooledArray<T> pooled = PooledArray<T>.GetPooled(1);
      pooled[0] = value0;
      return pooled;
    }

    public static PooledArray<T> GetPooled<T>(T value0, T value1)
    {
      PooledArray<T> pooled = PooledArray<T>.GetPooled(2);
      pooled[0] = value0;
      pooled[1] = value1;
      return pooled;
    }

    public static PooledArray<T> GetPooled<T>(T value0, T value1, T value2)
    {
      PooledArray<T> pooled = PooledArray<T>.GetPooled(3);
      pooled[0] = value0;
      pooled[1] = value1;
      pooled[2] = value2;
      return pooled;
    }

    public static PooledArray<T> GetPooled<T>(T value0, T value1, T value2, T value3)
    {
      PooledArray<T> pooled = PooledArray<T>.GetPooled(4);
      pooled[0] = value0;
      pooled[1] = value1;
      pooled[2] = value2;
      pooled[3] = value3;
      return pooled;
    }

    public static PooledArray<T> GetPooled<T>(T value0, T value1, T value2, T value3, T value4)
    {
      PooledArray<T> pooled = PooledArray<T>.GetPooled(5);
      pooled[0] = value0;
      pooled[1] = value1;
      pooled[2] = value2;
      pooled[3] = value3;
      pooled[4] = value4;
      return pooled;
    }
  }
}
