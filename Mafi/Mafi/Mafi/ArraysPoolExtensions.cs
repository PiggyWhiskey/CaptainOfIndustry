// Decompiled with JetBrains decompiler
// Type: Mafi.ArraysPoolExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class ArraysPoolExtensions
  {
    /// <summary>
    /// Returns this array to the pool. Please make sure than only arrays that are no longer in use are returned.
    /// </summary>
    public static void ReturnToPool<T>(this T[] arr) => ArrayPool<T>.ReturnToPool(arr);
  }
}
