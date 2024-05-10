// Decompiled with JetBrains decompiler
// Type: Mafi.CheckExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ImmutableCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  /// <summary>Helper extension methods for runtime checks.</summary>
  public static class CheckExtensions
  {
    internal static void ThrowException(string message) => throw new CheckException(message);

    /// <summary>
    /// Returns given <paramref name="value" /> if it is not null. Throws <see cref="T:Mafi.CheckException" /> otherwise.
    /// </summary>
    /// <remarks>
    /// We prefer to throw the exception immediately to reduce possibility of missing it and then getting the <see cref="T:Mafi.CheckException" /> somewhere in the program possibly much later. And this is true for both DEBUG and
    /// RELEASE modes. Nulls are EVIL!
    /// </remarks>
    /// <example>
    /// Intended use of this class is in assignments:
    /// <code>
    /// public void SomeMethod(string str) {
    /// m_str = str.CheckNotNull(); // Instead of just `m_str = str;`
    /// }
    /// </code>
    /// </example>
    [Pure]
    public static T CheckNotNull<T>(this T value, string message = "Fix it!") where T : class
    {
      if ((object) value == null)
        CheckExtensions.ThrowException(string.Format("Value of type '{0}' is null! {1}", (object) typeof (T), (object) message));
      return value;
    }

    /// <summary>
    /// Checks whether given string is not null or empty. If the check is positive the string is returned, otherwise
    /// <see cref="T:Mafi.CheckException" /> is thrown.
    /// </summary>
    [Pure]
    public static string CheckNotNullOrEmpty(this string str)
    {
      if (!string.IsNullOrEmpty(str))
        return str;
      Log.Error("String is null or empty.");
      return "";
    }

    /// <summary>
    /// Returns given <paramref name="array" /> if it is not null or empty. Otherwise throws <see cref="T:Mafi.CheckException" />.
    /// </summary>
    [Pure]
    public static T[] CheckNonEmpty<T>(this T[] array)
    {
      if (array == null || array.Length == 0)
        CheckExtensions.ThrowException("Array of " + typeof (T).Name + " is null or empty.");
      return array;
    }

    /// <summary>
    /// Returns given <paramref name="array" /> if it is not null or empty and all elements are not null. Otherwise
    /// throws <see cref="T:Mafi.CheckException" />.
    /// </summary>
    [Pure]
    public static T[] CheckNonEmptyOfNonNulls<T>(this T[] array) where T : class
    {
      if (array == null || array.Length == 0)
        CheckExtensions.ThrowException("Array of " + typeof (T).Name + " is null or empty.");
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        if ((object) array[index] == null)
          CheckExtensions.ThrowException(string.Format("Array of {0} is null on index {1}.", (object) typeof (T).Name, (object) index));
      }
      return array;
    }

    /// <summary>
    /// Checks whether given <paramref name="array" /> is not null and its length is greater or equal to <paramref name="minLength" />. If the check is positive the array is returned, otherwise <see cref="T:Mafi.CheckException" /> is
    /// thrown.
    /// </summary>
    [Pure]
    public static T[] CheckLength<T>(this T[] array, int minLength)
    {
      if (array == null)
        CheckExtensions.ThrowException(string.Format("Array of {0} with required length {1} is null.", (object) typeof (T).Name, (object) minLength));
      if (array.Length < minLength)
        CheckExtensions.ThrowException(string.Format("Array of {0} with required length {1} has length {2}.", (object) typeof (T).Name, (object) minLength, (object) array.Length));
      return array;
    }

    /// <summary>
    /// Returns given <paramref name="array" /> if it is not null or empty. Otherwise throws <see cref="T:Mafi.CheckException" />.
    /// </summary>
    [Pure]
    public static ImmutableArray<T> CheckNotEmpty<T>(this ImmutableArray<T> array)
    {
      if (array.IsNotValid)
        CheckExtensions.ThrowException("ImmutableArray<" + typeof (T).Name + "> is null!.");
      if (array.IsEmpty)
        CheckExtensions.ThrowException("ImmutableArray<" + typeof (T).Name + "> is empty!.");
      return array;
    }

    /// <summary>
    /// Checks whether given immutable <paramref name="array" /> is initialized (internal array non-null) and its
    /// length is greater or equal to <paramref name="minLength" />. If the check is positive the array is returned,
    /// otherwise <see cref="T:Mafi.CheckException" /> is thrown.
    /// </summary>
    [Pure]
    public static ImmutableArray<T> CheckLength<T>(this ImmutableArray<T> array, int minLength)
    {
      if (array.IsNotValid)
        CheckExtensions.ThrowException(string.Format("ImmutableArray<{0}> with required length {1} is null.", (object) typeof (T).Name, (object) minLength));
      if (array.Length < minLength)
        CheckExtensions.ThrowException(string.Format("ImmutableArray<{0}> with required length {1} has length {2}.", (object) typeof (T).Name, (object) minLength, (object) array.Length));
      return array;
    }

    /// <summary>
    /// Checks whether given struct is NOT equal to <c>default(T)</c>.
    /// </summary>
    [Pure]
    public static T CheckNotDefaultStruct<T>(this T value) where T : struct
    {
      if (EqualityComparer<T>.Default.Equals(value, default (T)))
        CheckExtensions.ThrowException("Value of struct " + typeof (T).Name + " is default (empty) instance.");
      return value;
    }
  }
}
