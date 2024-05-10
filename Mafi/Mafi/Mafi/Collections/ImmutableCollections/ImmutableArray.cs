// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.ImmutableArray
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  /// <summary>
  /// A set of initialization methods for instances of <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" />.
  /// </summary>
  public static class ImmutableArray
  {
    public static readonly ImmutableArray.EmptyArray Empty;

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> with the specified element as its only member.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the array.</typeparam>
    /// <param name="item">The element to store in the array.</param>
    /// <returns>A 1-element array.</returns>
    public static ImmutableArray<T> Create<T>(T item)
    {
      return new ImmutableArray<T>(new T[1]{ item });
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> with the specified elements.
    /// </summary>
    public static ImmutableArray<T> Create<T>(T item1, T item2)
    {
      return new ImmutableArray<T>(new T[2]{ item1, item2 });
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> with the specified elements.
    /// </summary>
    public static ImmutableArray<T> Create<T>(T item1, T item2, T item3)
    {
      return new ImmutableArray<T>(new T[3]
      {
        item1,
        item2,
        item3
      });
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> with the specified elements.
    /// </summary>
    public static ImmutableArray<T> Create<T>(T item1, T item2, T item3, T item4)
    {
      return new ImmutableArray<T>(new T[4]
      {
        item1,
        item2,
        item3,
        item4
      });
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> populated with the contents of the specified sequence.
    /// </summary>
    public static ImmutableArray<T> CreateRange<T>(IEnumerable<T> items)
    {
      if (items != null)
        return new ImmutableArray<T>(items.ToArray<T>());
      Log.Error("CreateRange called with null, returning empty.");
      return ImmutableArray<T>.Empty;
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> populated with the contents of the specified sequence.
    /// </summary>
    public static ImmutableArray<T> CreateRange<T>(ReadOnlyArraySlice<T> items)
    {
      return new ImmutableArray<T>(items.ToArray());
    }

    /// <summary>
    /// Creates an <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> from given items.
    /// </summary>
    public static ImmutableArray<T> Create<T>(params T[] items)
    {
      if (items != null)
        return ImmutableArray.CreateDefensiveCopy<T>(items);
      Assert.Fail("Creating immutable array from null.");
      return ImmutableArray<T>.Empty;
    }

    /// <summary>
    /// Creates 2D <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> from given 2D array.
    /// </summary>
    public static ImmutableArray<ImmutableArray<T>> Create<T>(T[][] arrays)
    {
      if (arrays == null)
      {
        Assert.Fail("Creating immutable 2D array from null.");
        return ImmutableArray.Create<ImmutableArray<T>>(ImmutableArray<T>.Empty);
      }
      ImmutableArray<T>[] immutableArrayArray = new ImmutableArray<T>[arrays.Length];
      for (int index = 0; index < immutableArrayArray.Length; ++index)
        immutableArrayArray[index] = ImmutableArray.Create<T>(arrays[index]);
      return ImmutableArray.Create<ImmutableArray<T>>(immutableArrayArray);
    }

    /// <summary>
    /// Enumerates a sequence exactly once and produces an immutable array of its contents.
    /// </summary>
    public static ImmutableArray<TSource> ToImmutableArray<TSource>(this IEnumerable<TSource> items)
    {
      return ImmutableArray.CreateRange<TSource>(items);
    }

    /// <summary>Creates an immutable array from given collection.</summary>
    public static ImmutableArray<TSource> ToImmutableArray<TSource>(this ICollection<TSource> items)
    {
      if (items.Count == 0)
        return (ImmutableArray<TSource>) ImmutableArray.Empty;
      TSource[] sourceArray = new TSource[items.Count];
      items.CopyTo(sourceArray, 0);
      return new ImmutableArray<TSource>(sourceArray);
    }

    /// <summary>
    /// Creates an immutable array from given read-only collection.
    /// </summary>
    public static ImmutableArray<TResult> ToImmutableArray<TSource, TResult>(
      this IReadOnlyCollection<TSource> items,
      Func<TSource, TResult> selector)
    {
      TResult[] items1 = new TResult[items.Count];
      int num = 0;
      foreach (TSource source in (IEnumerable<TSource>) items)
        items1[num++] = selector(source);
      return new ImmutableArray<TResult>(items1);
    }

    /// <summary>Creates an immutable array from given memory stream.</summary>
    public static ImmutableArray<byte> ToImmutableArray(this MemoryStream stream)
    {
      return new ImmutableArray<byte>(stream.ToArray());
    }

    /// <summary>
    /// Creates an immutable array from given collection and clears the collection.
    /// </summary>
    public static ImmutableArray<TSource> ToImmutableArrayAndClear<TSource>(
      this ICollection<TSource> items)
    {
      ImmutableArray<TSource> immutableArray = ImmutableArray.ToImmutableArray<TSource>(items);
      items.Clear();
      return immutableArray;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> struct.
    /// </summary>
    internal static ImmutableArray<T> CreateDefensiveCopy<T>(T[] items)
    {
      Assert.That<T[]>(items).IsNotNull<T[]>();
      if (items.Length == 0)
        return ImmutableArray<T>.Empty;
      T[] objArray = new T[items.Length];
      Array.Copy((Array) items, 0, (Array) objArray, 0, items.Length);
      return new ImmutableArray<T>(objArray);
    }

    public static ImmutableArray<T> FromRange<T>(int length, Func<int, T> genFunc)
    {
      T[] items = new T[length];
      for (int index = 0; index < length; ++index)
        items[index] = genFunc(index);
      return new ImmutableArray<T>(items);
    }

    public static ImmutableArray<T> FromRepeated<T>(int length, T value)
    {
      T[] items = new T[length];
      for (int index = 0; index < length; ++index)
        items[index] = value;
      return new ImmutableArray<T>(items);
    }

    /// <summary>
    /// Creates generic <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> with given types and values. It does create a defensive copy
    /// of values. It is callers responsibility to ensure that all given values can be assigned to the <paramref name="type" />.
    /// </summary>
    public static object CreateGeneric(Type type, IIndexable<object> values)
    {
      Type[] andInit1 = ArrayPool<Type>.GetAndInit(type);
      Type type1 = typeof (ImmutableArray<>).MakeGenericType(andInit1);
      andInit1.ReturnToPool<Type>();
      Array instance = Array.CreateInstance(type, values.Count);
      for (int index = 0; index < values.Count; ++index)
        instance.SetValue(values[index], index);
      Type[] andInit2 = ArrayPool<Type>.GetAndInit(instance.GetType());
      ConstructorInfo constructor = type1.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, andInit2, (ParameterModifier[]) null);
      andInit2.ReturnToPool<Type>();
      Assert.That<ConstructorInfo>(constructor).IsNotNull<ConstructorInfo>();
      object[] andInit3 = ArrayPool<object>.GetAndInit((object) instance);
      object generic = constructor.Invoke((object) null, andInit3);
      andInit3.ReturnToPool<object>();
      return generic;
    }

    static ImmutableArray()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ImmutableArray.Empty = ImmutableArray.EmptyArray.Instance;
    }

    /// <summary>
    /// Allows tp type <c>ImmutableArray.Empty</c> instead of <c>ImmutableArray{T}.Empty</c>. This can in some cases
    /// improve readability of the code if <c>T</c> is very long.
    /// </summary>
    public sealed class EmptyArray
    {
      public static readonly ImmutableArray.EmptyArray Instance;

      private EmptyArray()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static EmptyArray()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        ImmutableArray.EmptyArray.Instance = new ImmutableArray.EmptyArray();
      }
    }
  }
}
