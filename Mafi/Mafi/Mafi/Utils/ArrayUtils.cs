// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.ArrayUtils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Utils
{
  public static class ArrayUtils
  {
    public const int MAX_ARRAY_LENGTH = 2146435071;
    public const int MAX_BYTE_ARRAY_LENGTH = 2147483591;

    public static void EnsureLengthPow2KeepContents<T>(ref T[] arr, int length)
    {
      if (arr == null)
      {
        arr = new T[length.CeilToPowerOfTwoOrZero()];
      }
      else
      {
        if (arr.Length >= length)
          return;
        Array.Resize<T>(ref arr, length.CeilToPowerOfTwoOrZero());
      }
    }

    public static void EnsureLengthPow2NoCopy<T>(ref T[] arr, int length)
    {
      if (arr != null && arr.Length >= length)
        return;
      arr = new T[length.CeilToPowerOfTwoOrZero()];
    }
  }
}
