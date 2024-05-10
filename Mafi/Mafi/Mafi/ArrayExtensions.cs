// Decompiled with JetBrains decompiler
// Type: Mafi.ArrayExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ReadonlyCollections;
using System;

#nullable disable
namespace Mafi
{
  public static class ArrayExtensions
  {
    public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

    /// <summary>Whether this array has zero length.</summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static bool IsEmpty<T>(this T[] array) => array.Length == 0;

    /// <summary>Whether this array has non-zero length.</summary>
    public static bool IsNotEmpty<T>(this T[] array) => array.Length != 0;

    /// <summary>Returns the first element of the array effectively.</summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static T First<T>(this T[] array)
    {
      Assert.That<int>(array.Length).IsPositive();
      return array[0];
    }

    /// <summary>Returns the last element of the array effectively.</summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static T Last<T>(this T[] array)
    {
      Assert.That<int>(array.Length).IsPositive();
      return array[array.Length - 1];
    }

    /// <summary>
    /// Returns the last element of the array or default value effectively.
    /// </summary>
    /// <remarks>This method will take precedence to LINQ when calling on arrays - exactly what we want.</remarks>
    public static T LastOrDefault<T>(this T[] array)
    {
      return array.Length == 0 ? default (T) : array[array.Length - 1];
    }

    /// <summary>Returns second last element of the array effectively.</summary>
    public static T PreLast<T>(this T[] array)
    {
      Assert.That<int>(array.Length - 1).IsPositive();
      return array[array.Length - 2];
    }

    /// <summary>Concatenates two given arrays to one.</summary>
    [Pure]
    public static T[] Concatenate<T>(this T[] array, T[] otherArray)
    {
      T[] destinationArray = new T[array.Length + otherArray.Length];
      Array.Copy((Array) array, 0, (Array) destinationArray, 0, array.Length);
      Array.Copy((Array) otherArray, 0, (Array) destinationArray, array.Length, otherArray.Length);
      return destinationArray;
    }

    [Pure]
    public static T[] CloneArray<T>(this T[] array)
    {
      T[] destinationArray = new T[array.Length];
      Array.Copy((Array) array, (Array) destinationArray, array.Length);
      return destinationArray;
    }

    [Pure]
    public static T[] RotatedCcw90AsRowMajor<T>(this T[] arrayRowMajor, int size)
    {
      return arrayRowMajor.RotatedCcw90AsRowMajor<T>(size, size);
    }

    [Pure]
    public static T[] RotatedCcw90AsRowMajor<T>(this T[] arrayRowMajor, int width, int height)
    {
      Assert.That<T[]>(arrayRowMajor).HasLength<T>(width * height);
      T[] objArray = new T[arrayRowMajor.Length];
      for (int index1 = 0; index1 < height; ++index1)
      {
        int num1 = index1 * width;
        for (int index2 = 0; index2 < width; ++index2)
        {
          int num2 = index1;
          int num3 = width - 1 - index2;
          objArray[num3 * height + num2] = arrayRowMajor[num1 + index2];
        }
      }
      return objArray;
    }

    public static void MultInPlaceWith(this float[] array, float[] mult)
    {
      int index = 0;
      for (int length = array.Length; index < length; ++index)
        array[index] *= mult[index];
    }

    [Pure]
    public static ReadOnlyArraySlice<T> AsSlice<T>(this T[] array)
    {
      return new ReadOnlyArraySlice<T>(array);
    }

    [Pure]
    public static ReadOnlyArraySlice<T> AsSlice<T>(this T[] array, int startIndex, int count)
    {
      return new ReadOnlyArraySlice<T>(array, startIndex, count);
    }

    public static T[] Slice<T>(this T[] array, int fromIndex)
    {
      return array.Slice<T>(fromIndex, array.Length - fromIndex);
    }

    public static T[] Slice<T>(this T[] array, int fromIndex, int count)
    {
      if (fromIndex + count > array.Length)
        count = array.Length - fromIndex;
      if (count <= 0)
        return Array.Empty<T>();
      T[] destinationArray = new T[count];
      Array.Copy((Array) array, fromIndex, (Array) destinationArray, 0, count);
      return destinationArray;
    }

    public static int? FirstIndexOf<T>(this T[] arr, Predicate<T> predicate)
    {
      int length = arr.Length;
      for (int index = 0; index < length; ++index)
      {
        if (predicate(arr[index]))
          return new int?(index);
      }
      return new int?();
    }

    public static IIndexable<T> AsIndexable<T>(this T[] arr)
    {
      return (IIndexable<T>) new IndexableArray<T>(arr);
    }
  }
}
