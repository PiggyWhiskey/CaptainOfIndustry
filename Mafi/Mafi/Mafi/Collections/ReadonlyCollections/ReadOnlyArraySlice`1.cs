// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.ReadOnlyArraySlice`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  /// <summary>
  /// A readonly array slice (a sub-range of another array) with O(1) indexable lookup time.
  /// </summary>
  /// <devremarks>
  /// IMPORTANT NOTICE FOR MAINTAINERS AND REVIEWERS: This type should be thread-safe. As a struct, it cannot protect
  /// its own fields from being changed from one thread while its members are executing on other threads because
  /// structs can change *in place* simply by reassigning the field containing this struct. Therefore it is extremely
  /// important that ** Every member should only dereference <c>this</c> ONCE. ** If a member needs to reference the
  /// array field, that counts as a dereference of <c>this</c>. Calling other instance members (properties or methods)
  /// also counts as dereferencing <c>this</c>. Any member that needs to use <c>this</c> more than once must instead
  /// assign <c>this</c> to a local variable and use that for the rest of the code instead. This effectively copies the
  /// one field in the struct to a local variable so that it is insulated from other threads.
  /// </devremarks>
  [DebuggerDisplay("Length={Length}")]
  public readonly struct ReadOnlyArraySlice<T>
  {
    /// <summary>
    /// An empty (initialized) instance of <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArraySlice`1" />.
    /// </summary>
    public static readonly ReadOnlyArraySlice<T> Empty;
    /// <summary>The underlying array.</summary>
    private readonly T[] m_array;
    /// <summary>Start index of this slice.</summary>
    private readonly int m_startIndex;
    /// <summary>Number of elements of this slice.</summary>
    private readonly int m_count;

    public ReadOnlyArraySlice(T[] array)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_array = array;
      this.m_startIndex = 0;
      this.m_count = array.Length;
    }

    public ReadOnlyArraySlice(T[] array, int startIndex, int count)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      if (count > 0)
      {
        Mafi.Assert.That<int>(startIndex).IsValidIndexFor<T>(array);
        Mafi.Assert.That<int>(startIndex + count).IsLessOrEqual(array.Length);
      }
      this.m_array = array;
      this.m_startIndex = startIndex;
      this.m_count = count;
    }

    /// <summary>
    /// Gets the element at the specified index in the array slice.
    /// </summary>
    public T this[int index]
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array[readOnlyArraySlice.m_startIndex + index];
      }
    }

    /// <summary>Gets the length of array in the collection.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Length => this.m_count;

    /// <summary>Whether this array is empty.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty => this.m_count <= 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotEmpty => this.m_count > 0;

    /// <summary>
    /// Whether this struct was initialized without an actual array instance.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNull => this.m_array == null;

    /// <summary>Whether this struct is empty or uninitialized.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNullOrEmpty
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array == null || readOnlyArraySlice.m_count == 0;
      }
    }

    /// <summary>
    /// Gets the first element of the array. Throws exception if the array is null or empty.
    /// </summary>
    public T First
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array[readOnlyArraySlice.m_startIndex];
      }
    }

    public T Second
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array[readOnlyArraySlice.m_startIndex + 1];
      }
    }

    /// <summary>
    /// Gets the last element of the array. Throws exception if the array is null or empty.
    /// </summary>
    public T Last
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array[readOnlyArraySlice.m_startIndex + readOnlyArraySlice.m_count - 1];
      }
    }

    public T PreLast
    {
      get
      {
        ReadOnlyArraySlice<T> readOnlyArraySlice = this;
        return readOnlyArraySlice.m_array[readOnlyArraySlice.m_startIndex + readOnlyArraySlice.m_count - 2];
      }
    }

    /// <summary>Returns an enumerator for the contents of the array.</summary>
    [Pure]
    public ReadOnlyArraySlice<T>.Enumerator GetEnumerator()
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      return new ReadOnlyArraySlice<T>.Enumerator(readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex, readOnlyArraySlice.m_count);
    }

    /// <summary>Returns new array that is reversed.</summary>
    [Pure]
    public ReadOnlyArraySlice<T> Reversed()
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      if (readOnlyArraySlice.IsEmpty)
        return readOnlyArraySlice;
      T[] array = new T[readOnlyArraySlice.m_count];
      int num = readOnlyArraySlice.m_startIndex + readOnlyArraySlice.m_count - 1;
      for (int index = 0; index < array.Length; ++index)
        array[index] = readOnlyArraySlice.m_array[num - index];
      return array.AsSlice<T>();
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    [Pure]
    public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TResult>) new ReadOnlyArraySlice<T>.\u003CSelect\u003Ed__28<TResult>(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__selector = selector
      };
    }

    [Pure]
    public ReadOnlyArraySlice<T> Slice(int startI)
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      Mafi.Assert.That<int>(startI).IsLess(readOnlyArraySlice.m_count);
      return readOnlyArraySlice.Slice(startI, readOnlyArraySlice.m_count - startI);
    }

    [Pure]
    public ReadOnlyArraySlice<T> Slice(int startI, int count)
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      return new ReadOnlyArraySlice<T>(readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex + startI, count);
    }

    [Pure]
    public TComp Max<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] array = this.m_array;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (this.m_count == 0)
        throw new InvalidOperationException("Max: No elements in sequence.");
      TComp y = selector(array[this.m_startIndex]);
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < this.m_count; ++index)
      {
        T obj = array[this.m_startIndex + index];
        TComp x = selector(obj);
        if (comparer.Compare(x, y) > 0)
          y = x;
      }
      return y;
    }

    [Pure]
    public T[] ToArray()
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      if (readOnlyArraySlice.Length == 0)
        return Array.Empty<T>();
      T[] destinationArray = new T[readOnlyArraySlice.m_count];
      Array.Copy((Array) readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex, (Array) destinationArray, 0, destinationArray.Length);
      return destinationArray;
    }

    [Pure]
    public T[] ToArray(int startI)
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      return readOnlyArraySlice.ToArray(startI, readOnlyArraySlice.m_count - startI);
    }

    [Pure]
    public T[] ToArray(int startI, int count)
    {
      if (count <= 0)
      {
        Mafi.Assert.That<int>(count).IsNotNegative();
        return Array.Empty<T>();
      }
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      T[] destinationArray = new T[count];
      Array.Copy((Array) readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex + startI, (Array) destinationArray, 0, count);
      return destinationArray;
    }

    [Pure]
    public ImmutableArray<T> ToImmutableArray() => ImmutableArray.CreateRange<T>(this);

    public void CopyTo(T[] dstArray, int dstIndex)
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      Array.Copy((Array) readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex, (Array) dstArray, dstIndex, readOnlyArraySlice.m_count);
    }

    public void CopyTo(T[] dstArray, int dstIndex, int srcIndex, int count)
    {
      ReadOnlyArraySlice<T> readOnlyArraySlice = this;
      Array.Copy((Array) readOnlyArraySlice.m_array, readOnlyArraySlice.m_startIndex + srcIndex, (Array) dstArray, dstIndex, count);
    }

    public IEnumerable<T> AsEnumerable()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<T>) new ReadOnlyArraySlice<T>.\u003CAsEnumerable\u003Ed__38(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    static ReadOnlyArraySlice()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ReadOnlyArraySlice<T>.Empty = new ReadOnlyArraySlice<T>(Array.Empty<T>());
    }

    /// <summary>An array enumerator.</summary>
    /// <remarks>
    /// It is important that this enumerator does NOT implement <see cref="T:System.IDisposable" />. We want the iterator to
    /// inline when we do foreach and to not result in a try/finally frame in the client.
    /// </remarks>
    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly T[] m_array;
      private readonly int m_count;
      private int m_index;

      internal Enumerator(T[] array, int startIndex, int count)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_array = array.CheckNotNull<T[]>();
        this.m_index = startIndex - 1;
        this.m_count = count + startIndex;
      }

      /// <summary>Gets the currently enumerated value.</summary>
      public T Current => this.m_array[this.m_index];

      /// <summary>Advances to the next value to be enumerated.</summary>
      /// <returns><c>true</c> if another item exists in the array; <c>false</c> otherwise.</returns>
      public bool MoveNext() => ++this.m_index < this.m_count;
    }
  }
}
