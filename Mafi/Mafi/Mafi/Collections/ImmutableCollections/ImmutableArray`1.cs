// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.ImmutableArray`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  /// <summary>
  /// A lightweight array wrapper that makes it immutable. This struct has zero memory overhead compared to a plain array.
  /// It is guaranteed that length and elements of this immutable array will never change, however, if this array stores
  /// mutable objects, they can be changed.
  /// </summary>
  /// <devremarks>
  /// This type has a documented contract of being exactly one reference-type field in size.
  /// 
  /// IMPORTANT NOTICE FOR MAINTAINERS AND REVIEWERS: This type should be thread-safe. As a struct, it cannot protect
  /// its own fields from being changed from one thread while its members are executing on other threads because
  /// structs can change *in place* simply by reassigning the field containing this struct. Therefore it is extremely
  /// important that ** Every member should only dereference <c>this</c> ONCE. ** If a member needs to reference the
  /// array field, that counts as a dereference of <c>this</c>. Calling other instance members (properties or methods)
  /// also counts as dereferencing <c>this</c>. Any member that needs to use <c>this</c> more than once must instead
  /// assign <c>this</c> to a local variable and use that for the rest of the code instead. This effectively copies the
  /// one field in the struct to a local variable so that it is insulated from other threads.
  /// </devremarks>
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("{IsValid ? \"Length=\" + Length : \"NULL\", nq}")]
  public readonly struct ImmutableArray<T> : IImmutableArray, IEquatable<ImmutableArray<T>>
  {
    /// <summary>
    /// An empty (initialized) instance of <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" />. This is not the same as <c>default</c> value
    /// of this struct which does not have the internal array set.
    /// </summary>
    public static readonly ImmutableArray<T> Empty;
    /// <summary>
    /// The backing field for this instance. References to this value should never be shared with outside code.
    /// </summary>
    private readonly T[] m_items;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> struct *without making a defensive copy*.
    /// Use this only when it is guaranteed that no other code has access to the given array.
    /// </summary>
    internal ImmutableArray(T[] items)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_items = items.CheckNotNull<T[]>();
    }

    /// <summary>
    /// Gets the element at the specified index in the read-only list.
    /// </summary>
    /// <remarks>
    /// We intentionally do not check this.array != null, and throw NullReferenceException if this is called while
    /// uninitialized. The reason for this is perf. Length and the indexer must be absolutely trivially implemented
    /// for the JIT optimization of removing array bounds checking to work.
    /// </remarks>
    public T this[int index] => this.m_items[index];

    /// <summary>Gets random element.</summary>
    public T this[IRandom random]
    {
      get
      {
        ImmutableArray<T> immutableArray = this;
        switch (immutableArray.Length)
        {
          case 0:
            throw new InvalidOperationException("Getting random element of an empty ImmutableArray.");
          case 1:
            return immutableArray.m_items[0];
          default:
            return immutableArray.m_items[random.NextInt(0, immutableArray.m_items.Length)];
        }
      }
    }

    /// <summary>Gets the length of array in the collection.</summary>
    /// <remarks>
    /// We intentionally do not check this.array != null, and throw NullReferenceException if this is called while
    /// uninitialized. The reason for this is perf. Length and the indexer must be absolutely trivially implemented
    /// for the JIT optimization of removing array bounds checking to work.
    /// </remarks>
    public int Length => this.m_items.Length;

    /// <summary>Index of the last element.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LastIndex => this.m_items.Length - 1;

    /// <summary>Whether this array is empty (or invalid).</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty => this.m_items == null || this.m_items.Length == 0;

    /// <summary>Whether this array is NOT empty (and valid).</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotEmpty => this.m_items != null && this.m_items.Length != 0;

    /// <summary>
    /// Gets the first element of the array. Throws exception if the array is null or empty.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T First => this.m_items[0];

    /// <summary>
    /// Gets the second element of the array. Throws exception if the array is null or length is not greater than
    /// one.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Second => this.m_items[1];

    /// <summary>
    /// Gets the last element of the array. Throws exception if the array is null or empty.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Last
    {
      get
      {
        ImmutableArray<T> immutableArray = this;
        return immutableArray.m_items[immutableArray.m_items.Length - 1];
      }
    }

    /// <summary>
    /// Gets the element before the last element of the array. Throws exception if the array is null or length is not
    /// greater than one.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T PreLast
    {
      get
      {
        ImmutableArray<T> immutableArray = this;
        return immutableArray.m_items[immutableArray.m_items.Length - 2];
      }
    }

    /// <summary>
    /// Returns this array as readonly array. This weakens the guarantees but it is safe as it is impossible to
    /// mutate the underlying array from <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" />.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ReadOnlyArray<T> AsReadOnlyArray => new ReadOnlyArray<T>(this.m_items);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ReadOnlyArraySlice<T> AsSlice => new ReadOnlyArraySlice<T>(this.m_items);

    /// <summary>
    /// Returns this array as indexable. This creates a new class instance to prevent boxing.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IIndexable<T> AsIndexable => (IIndexable<T>) new IndexableArray<T>(this.m_items);

    /// <summary>
    /// Whether this structs is valid and underlying array is not null. This should be always the case except when
    /// <c>default( <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" />)</c> is used or new array of <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> is
    /// created.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsValid => this.m_items != null;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotValid => this.m_items == null;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotValidOrEmpty => this.m_items == null || this.m_items.Length == 0;

    Type IImmutableArray.ElementType => typeof (T);

    /// <summary>
    /// Gets an untyped (mutable!) reference to the internal array. Accessing this boxes the struct.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Array IImmutableArray.Array => (Array) this.m_items;

    /// <summary>
    /// STOP: Do not use this! This method breaks Immutable array encapsulation and A LOT OF CODE depends on this.
    /// This should be used only in very special cases for performance reasons.
    /// </summary>
    internal T[] GetInternalArray_StrictlyReadonly_NoSharing() => this.m_items;

    /// <summary>
    /// Checks equality between two instances based on the internal array. Does NOT compare elements!
    /// </summary>
    public static bool operator ==(ImmutableArray<T> left, ImmutableArray<T> right)
    {
      return left.Equals(right);
    }

    /// <summary>
    /// Checks inequality between two instances based on the internal array. Does NOT compare elements!
    /// </summary>
    public static bool operator !=(ImmutableArray<T> left, ImmutableArray<T> right)
    {
      return !left.Equals(right);
    }

    /// <summary>
    /// Checks equality between two instances based on the internal array. Does NOT compare elements!
    /// </summary>
    public static bool operator ==(ImmutableArray<T>? left, ImmutableArray<T>? right)
    {
      return left.GetValueOrDefault().Equals(right.GetValueOrDefault());
    }

    /// <summary>
    /// Checks inequality between two instances based on the internal array. Does NOT compare elements!
    /// </summary>
    public static bool operator !=(ImmutableArray<T>? left, ImmutableArray<T>? right)
    {
      return !left.GetValueOrDefault().Equals(right.GetValueOrDefault());
    }

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item) => Array.IndexOf<T>(this.m_items, item);

    [Pure]
    public int IndexOf(Predicate<T> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      for (int index = 0; index < immutableArray.Length; ++index)
      {
        if (predicate(immutableArray[index]))
          return index;
      }
      return -1;
    }

    [Pure]
    public int IndexOf(Func<int, T, bool> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      for (int index = 0; index < immutableArray.Length; ++index)
      {
        if (predicate(index, immutableArray[index]))
          return index;
      }
      return -1;
    }

    [Pure]
    public int IndexOf<TArg>(TArg arg, Func<T, TArg, bool> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      for (int index = 0; index < immutableArray.Length; ++index)
      {
        if (predicate(immutableArray[index], arg))
          return index;
      }
      return -1;
    }

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex) => Array.IndexOf<T>(this.m_items, item, startIndex);

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex, int count)
    {
      return Array.IndexOf<T>(this.m_items, item, startIndex, count);
    }

    [Pure]
    public int? FirstIndexOf(Predicate<T> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      for (int index = 0; index < immutableArray.Length; ++index)
      {
        if (predicate(immutableArray.m_items[index]))
          return new int?(index);
      }
      return new int?();
    }

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item) => Array.LastIndexOf<T>(this.m_items, item);

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex)
    {
      return Array.LastIndexOf<T>(this.m_items, item, startIndex);
    }

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex, int count)
    {
      return Array.LastIndexOf<T>(this.m_items, item, startIndex, count);
    }

    /// <summary>
    /// Determines whether the specified item exists in the array.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns><c>true</c> if an equal value was found in the array; <c>false</c> otherwise.</returns>
    [Pure]
    public bool Contains(T item) => this.IndexOf(item) >= 0;

    [Pure]
    public bool Contains(Predicate<T> predicate)
    {
      foreach (T obj in this.m_items)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    [Pure]
    public T FindOrDefault(Predicate<T> predicate)
    {
      foreach (T orDefault in this.m_items)
      {
        if (predicate(orDefault))
          return orDefault;
      }
      return default (T);
    }

    [Pure]
    public Lyst<T> ToLyst()
    {
      Lyst<T> lyst = new Lyst<T>();
      lyst.AddRange(this);
      return lyst;
    }

    [Pure]
    public Lyst<TResult> ToLyst<TResult>(Func<T, TResult> selector)
    {
      ImmutableArray<T> immutableArray = this;
      Lyst<TResult> lyst = new Lyst<TResult>()
      {
        Count = immutableArray.Length
      };
      for (int index = 0; index < lyst.Count; ++index)
        lyst[index] = selector(immutableArray[index]);
      return lyst;
    }

    [Pure]
    public IReadOnlySet<T> ToReadonlySet()
    {
      return !this.IsEmpty ? (IReadOnlySet<T>) new Set<T>((IEnumerable<T>) this.m_items) : Set<T>.Empty;
    }

    /// <summary>
    /// Returns a copy of this immutable array as plain mutable array.
    /// </summary>
    [Pure]
    public T[] ToArray()
    {
      ImmutableArray<T> immutableArray = this;
      T[] destinationArray = new T[immutableArray.m_items.Length];
      Array.Copy((Array) immutableArray.m_items, (Array) destinationArray, destinationArray.Length);
      return destinationArray;
    }

    /// <summary>
    /// Returns a copy of this array mapped with given <paramref name="selector" /> function.
    /// </summary>
    [Pure]
    public TResult[] ToArray<TResult>(Func<T, TResult> selector)
    {
      ImmutableArray<T> immutableArray = this;
      TResult[] array = new TResult[immutableArray.m_items.Length];
      for (int index = 0; index < array.Length; ++index)
        array[index] = selector(immutableArray[index]);
      return array;
    }

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    public void CopyTo(T[] destination, int destinationIndex = 0)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
        Mafi.Log.Error("Attempting to copy from an invalid immutable array.");
      else
        Array.Copy((Array) immutableArray.m_items, 0, (Array) destination, destinationIndex, immutableArray.Length);
    }

    public void CopyTo(T[] destination, int dstIndex, int srcIndex, int count)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
        Mafi.Log.Error("Attempting to copy from an invalid immutable array.");
      else
        Array.Copy((Array) immutableArray.m_items, srcIndex, (Array) destination, dstIndex, count);
    }

    public static void WriteDataTo(ImmutableArray<byte> arr, BinaryWriter writer)
    {
      writer.Write(arr.m_items);
    }

    public static ImmutableArray<byte> ReadFrom(BinaryReader reader, int length)
    {
      byte[] numArray = new byte[length];
      reader.Read(numArray, 0, length);
      return new ImmutableArray<byte>(numArray);
    }

    /// <summary>
    /// Returns a new array with the specified value inserted at the specified position.
    /// </summary>
    /// <param name="index">The 0-based index into the array at which the new item should be added.</param>
    /// <param name="item">The item to insert at the start of the array.</param>
    /// <returns>A new array.</returns>
    [Pure]
    public ImmutableArray<T> Insert(int index, T item)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to insert into an invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      Mafi.Assert.That<int>(index).IsValidIndex(immutableArray.Length + 1);
      if (immutableArray.Length == 0)
        return ImmutableArray.Create<T>(item);
      T[] objArray = new T[immutableArray.Length + 1];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, index);
      objArray[index] = item;
      Array.Copy((Array) immutableArray.m_items, index, (Array) objArray, index + 1, immutableArray.Length - index);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>Inserts the specified values at the specified index.</summary>
    /// <param name="index">The index at which to insert the value.</param>
    /// <param name="items">The elements to insert.</param>
    /// <returns>The new immutable collection.</returns>
    [Pure]
    public ImmutableArray<T> InsertRange(int index, IEnumerable<T> items)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to insert range into an invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      Mafi.Assert.That<int>(index).IsValidIndex(immutableArray.Length + 1);
      if (!(items is List<T> objList))
        objList = items.ToList<T>();
      List<T> items1 = objList;
      Mafi.Assert.That<List<T>>(items1).IsNotNull<List<T>>();
      if (immutableArray.Length == 0)
        return ImmutableArray.CreateRange<T>((IEnumerable<T>) items1);
      int count = items1.Count;
      if (count == 0)
        return immutableArray;
      T[] objArray = new T[immutableArray.Length + count];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, index);
      int num = index;
      foreach (T obj in items1)
        objArray[num++] = obj;
      Array.Copy((Array) immutableArray.m_items, index, (Array) objArray, index + count, immutableArray.Length - index);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>Inserts the specified values at the specified index.</summary>
    /// <param name="index">The index at which to insert the value.</param>
    /// <param name="items">The elements to insert.</param>
    /// <returns>The new immutable collection.</returns>
    [Pure]
    public ImmutableArray<T> InsertRange(int index, ImmutableArray<T> items)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to insert range into an invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      if (items.IsNotValid)
      {
        Mafi.Log.Error("Attempting to insert range from invalid immutable array.");
        return immutableArray;
      }
      Mafi.Assert.That<int>(index).IsValidIndex(immutableArray.Length + 1);
      if (immutableArray.IsEmpty)
        return items;
      return items.IsEmpty ? immutableArray : immutableArray.InsertRange(index, (IEnumerable<T>) items.m_items);
    }

    /// <summary>
    /// Returns a new array with the specified value inserted at the end.
    /// </summary>
    /// <param name="item">The item to insert at the end of the array.</param>
    /// <returns>A new array.</returns>
    [Pure]
    public ImmutableArray<T> Add(T item)
    {
      ImmutableArray<T> immutableArray = this;
      return immutableArray.Length == 0 ? ImmutableArray.Create<T>(item) : immutableArray.Insert(immutableArray.Length, item);
    }

    /// <summary>Adds the specified values to this list.</summary>
    /// <param name="items">The values to add.</param>
    /// <returns>A new list with the elements added.</returns>
    [Pure]
    public ImmutableArray<T> AddRange(IEnumerable<T> items)
    {
      ImmutableArray<T> immutableArray = this;
      return immutableArray.InsertRange(immutableArray.Length, items);
    }

    /// <summary>
    /// Returns an array with the item at the specified position replaced.
    /// </summary>
    /// <param name="index">The index of the item to replace.</param>
    /// <param name="item">The new item.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public ImmutableArray<T> SetItem(int index, T item)
    {
      ImmutableArray<T> immutableArray = this;
      Mafi.Assert.That<int>(index).IsValidIndex(immutableArray.Length);
      T[] objArray = new T[immutableArray.Length];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, immutableArray.Length);
      objArray[index] = item;
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Returns an array with the element at the specified position removed.
    /// </summary>
    /// <param name="index">The 0-based index into the array for the element to omit from the returned array.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public ImmutableArray<T> RemoveAt(int index) => this.RemoveRange(index, 1);

    /// <summary>
    /// Returns an array with the elements at the specified position removed.
    /// </summary>
    /// <param name="index">The 0-based index into the array for the element to omit from the returned array.</param>
    /// <param name="length">The number of elements to remove.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public ImmutableArray<T> RemoveRange(int index, int length)
    {
      ImmutableArray<T> immutableArray = this;
      Mafi.Assert.That<int>(length).IsNotNegative();
      Mafi.Assert.That<int>(index + length).IsLessOrEqual(immutableArray.Length);
      if (length == 0)
        return immutableArray;
      T[] objArray = new T[immutableArray.Length - length];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, index);
      Array.Copy((Array) immutableArray.m_items, index + length, (Array) objArray, index, immutableArray.Length - index - length);
      return new ImmutableArray<T>(objArray);
    }

    [Pure]
    public ImmutableArray<T> GetRange(int fromIndex)
    {
      return this.GetRange(fromIndex, this.Length - fromIndex);
    }

    [Pure]
    public ImmutableArray<T> GetRange(int fromIndex, int length)
    {
      Mafi.Assert.That<int>(length).IsNotNegative();
      if (length <= 0)
        return ImmutableArray<T>.Empty;
      ImmutableArray<T> range = this;
      Mafi.Assert.That<int>(fromIndex + length).IsLessOrEqual(range.Length);
      if (length == this.Length)
      {
        Mafi.Assert.That<int>(fromIndex).IsZero();
        return range;
      }
      T[] objArray = new T[length];
      Array.Copy((Array) range.m_items, fromIndex, (Array) objArray, 0, length);
      return new ImmutableArray<T>(objArray);
    }

    [Pure]
    public ReadOnlyArraySlice<T> Slice(int startI)
    {
      ImmutableArray<T> immutableArray = this;
      return new ReadOnlyArraySlice<T>(immutableArray.m_items, startI, immutableArray.m_items.Length - startI);
    }

    [Pure]
    public ReadOnlyArraySlice<T> Slice(int startI, int count)
    {
      return new ReadOnlyArraySlice<T>(this.m_items, startI, count);
    }

    /// <summary>
    /// Returns an array with the elements at the specified position removed and <paramref name="value" /> added
    /// instead of them. See <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray`1.RemoveRange(System.Int32,System.Int32)" /> for more information.
    /// </summary>
    [Pure]
    public ImmutableArray<T> RemoveRangeAndAdd(int index, int length, T value)
    {
      ImmutableArray<T> immutableArray = this;
      Mafi.Assert.That<int>(length).IsNotNegative();
      Mafi.Assert.That<int>(index + length).IsLessOrEqual(immutableArray.Length);
      T[] objArray = new T[immutableArray.Length - length + 1];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, index);
      objArray[index] = value;
      Array.Copy((Array) immutableArray.m_items, index + length, (Array) objArray, index + 1, immutableArray.Length - index - length);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the specified predicate.
    /// Returns self if no elements were removed.
    /// </summary>
    /// <param name="match">
    /// The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to remove.
    /// </param>
    /// <returns>The new list.</returns>
    [Pure]
    public ImmutableArray<T> RemoveAll(Predicate<T> match)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to remove from invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      Mafi.Assert.That<Predicate<T>>(match).IsNotNull<Predicate<T>>();
      if (immutableArray.IsEmpty)
        return immutableArray;
      Lyst<int> indexesToRemove = (Lyst<int>) null;
      for (int index = 0; index < immutableArray.m_items.Length; ++index)
      {
        if (match(immutableArray.m_items[index]))
        {
          if (indexesToRemove == null)
            indexesToRemove = new Lyst<int>();
          indexesToRemove.Add(index);
        }
      }
      return indexesToRemove == null ? immutableArray : immutableArray.removeAtRange((ICollection<int>) indexesToRemove);
    }

    /// <summary>
    /// Similar to <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray`1.RemoveAll(System.Predicate{`0})" /> but with extra argument passed to the predicate to avoid allocations.
    /// </summary>
    [Pure]
    public ImmutableArray<T> RemoveAll<TArg>(TArg arg, Func<T, TArg, bool> match)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to remove from invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      Mafi.Assert.That<Func<T, TArg, bool>>(match).IsNotNull<Func<T, TArg, bool>>();
      if (immutableArray.IsEmpty)
        return immutableArray;
      Lyst<int> indexesToRemove = (Lyst<int>) null;
      for (int index = 0; index < immutableArray.m_items.Length; ++index)
      {
        if (match(immutableArray.m_items[index], arg))
        {
          if (indexesToRemove == null)
            indexesToRemove = new Lyst<int>();
          indexesToRemove.Add(index);
        }
      }
      return indexesToRemove == null ? immutableArray : immutableArray.removeAtRange((ICollection<int>) indexesToRemove);
    }

    [Pure]
    public ImmutableArray<T> Filter(Predicate<T> predicate)
    {
      return this.RemoveAll<Predicate<T>>(predicate, (Func<T, Predicate<T>, bool>) ((x, p) => !p(x)));
    }

    /// <summary>
    /// Returns indices of values that matched with the given predicate.
    /// </summary>
    [Pure]
    public ImmutableArray<int> GetIndices(Predicate<T> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to get indices from invalid immutable array.");
        return ImmutableArray<int>.Empty;
      }
      if (immutableArray.IsEmpty)
        return ImmutableArray<int>.Empty;
      Lyst<int> lyst = (Lyst<int>) null;
      for (int index = 0; index < immutableArray.m_items.Length; ++index)
      {
        if (predicate(immutableArray.m_items[index]))
        {
          if (lyst == null)
            lyst = new Lyst<int>();
          lyst.Add(index);
        }
      }
      return lyst == null ? ImmutableArray<int>.Empty : lyst.ToImmutableArray();
    }

    [Pure]
    public int[] GetIndicesArray(Predicate<T> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to get indices from invalid immutable array.");
        return Array.Empty<int>();
      }
      if (immutableArray.IsEmpty)
        return Array.Empty<int>();
      Lyst<int> lyst = (Lyst<int>) null;
      for (int index = 0; index < immutableArray.m_items.Length; ++index)
      {
        if (predicate(immutableArray.m_items[index]))
        {
          if (lyst == null)
            lyst = new Lyst<int>();
          lyst.Add(index);
        }
      }
      return lyst?.ToArray() ?? Array.Empty<int>();
    }

    /// <summary>
    /// Returns a sorted instance of this array. Returns self if it is already sorted.
    /// </summary>
    [Pure]
    public ImmutableArray<T> Sort(IComparer<T> comparer = null)
    {
      ImmutableArray<T> immutableArray = this;
      return immutableArray.Sort(0, immutableArray.Length, comparer);
    }

    /// <summary>
    /// Returns a sorted instance of this array. Returns self if it is already sorted.
    /// </summary>
    [Pure]
    public ImmutableArray<T> Sort(int index, int count, IComparer<T> comparer = null)
    {
      Mafi.Assert.That<int>(count).IsNotNegative();
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to sort an invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      if (count <= 1)
        return immutableArray;
      Mafi.Assert.That<int>(index).IsValidIndex(immutableArray.Length);
      Mafi.Assert.That<int>(index + count).IsLessOrEqual(immutableArray.Length);
      if (comparer == null)
        comparer = (IComparer<T>) Comparer<T>.Default;
      bool flag = false;
      int index1 = index + 1;
      for (int index2 = index + count; index1 < index2; ++index1)
      {
        if (comparer.Compare(immutableArray.m_items[index1 - 1], immutableArray.m_items[index1]) > 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return immutableArray;
      T[] objArray = new T[immutableArray.Length];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, immutableArray.Length);
      Array.Sort<T>(objArray, index, count, comparer);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>Returns a sorted instance of this array.</summary>
    [Pure]
    public ImmutableArray<T> Sort(Comparison<T> comparison)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsNotValid)
      {
        Mafi.Log.Error("Attempting to sort an invalid immutable array.");
        return ImmutableArray<T>.Empty;
      }
      int length = immutableArray.m_items.Length;
      bool flag = false;
      for (int index = 1; index < length; ++index)
      {
        if (comparison(immutableArray.m_items[index - 1], immutableArray.m_items[index]) > 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return immutableArray;
      T[] objArray = new T[immutableArray.Length];
      Array.Copy((Array) immutableArray.m_items, 0, (Array) objArray, 0, immutableArray.Length);
      Array.Sort<T>(objArray, comparison);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> for a specific element, using the
    /// <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> and by the specified object.
    /// </summary>
    /// <param name="value">The object to search for.</param>
    /// <returns>
    /// The index of the specified <paramref name="value" /> in the specified array, if <paramref name="value" /> is
    /// found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more
    /// elements in array, a negative number which is the bitwise complement of the index of the first element that
    /// is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in array, a negative number which is the bitwise
    /// complement of (the index of the last element plus 1).
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface, and the
    /// search encounters an element that does not implement the <see cref="T:System.IComparable`1" /> generic interface.
    /// </exception>
    [Pure]
    public int BinarySearch(T value) => Array.BinarySearch<T>(this.m_items, value);

    /// <summary>Returns an enumerator for the contents of the array.</summary>
    [Pure]
    [DebuggerStepThrough]
    public ImmutableArray<T>.Enumerator GetEnumerator()
    {
      return new ImmutableArray<T>.Enumerator(this.m_items);
    }

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    [DebuggerStepThrough]
    [Pure]
    public override int GetHashCode()
    {
      ImmutableArray<T> immutableArray = this;
      return immutableArray.m_items != null ? immutableArray.m_items.GetHashCode() : 0;
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    [DebuggerStepThrough]
    public override bool Equals(object obj)
    {
      return obj is IImmutableArray immutableArray && this.m_items == immutableArray.Array;
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
    /// </returns>
    [DebuggerStepThrough]
    [Pure]
    public bool Equals(ImmutableArray<T> other) => this.m_items == other.m_items;

    /// <summary>
    /// Whether all values of this and given array are equal according to the default equality.
    /// </summary>
    [Pure]
    public bool ValuesEquals(ImmutableArray<T> other)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.Length != other.Length)
        return false;
      if (immutableArray.m_items == other.m_items)
        return true;
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < immutableArray.Length; ++index)
      {
        if (!equalityComparer.Equals(immutableArray[index], other[index]))
          return false;
      }
      return true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> struct based on the contents of an existing
    /// instance, allowing a covariant static cast to efficiently reuse the existing array.
    /// </summary>
    /// <param name="items">The array to initialize the array with. No copy is made.</param>
    /// <remarks>
    /// Covariant up-casts from this method may be reversed by calling the <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray`1.As``1" />
    /// or <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray`1.CastArray``1" /> method.
    /// </remarks>
    public static ImmutableArray<T> CastUp<TDerived>(ImmutableArray<TDerived> items) where TDerived : class, T
    {
      return new ImmutableArray<T>((T[]) items.m_items);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" /> struct by casting the underlying array to
    /// an array of type <typeparam name="TOther" /> .
    /// </summary>
    /// <exception cref="T:System.InvalidCastException">Thrown if the cast is illegal.</exception>
    [Pure]
    public ImmutableArray<TOther> CastArray<TOther>() where TOther : class
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.m_items == null)
      {
        Mafi.Log.Error("CastArray() called on invalid immutable array.");
        return ImmutableArray<TOther>.Empty;
      }
      if (immutableArray.m_items.Length == 0)
        return ImmutableArray<TOther>.Empty;
      try
      {
        return new ImmutableArray<TOther>((TOther[]) immutableArray.m_items);
      }
      catch (InvalidCastException ex)
      {
        Mafi.Log.Exception((Exception) ex, "Failed to cast immutable array of '" + immutableArray.m_items.First<T>().GetType().Name + "' to '" + typeof (TOther).Name + "'.");
        return ImmutableArray<TOther>.Empty;
      }
    }

    /// <summary>
    /// Creates an immutable array for this array, cast to a different element type.
    /// </summary>
    /// <typeparam name="TOther">The type of array element to return.</typeparam>
    /// <returns>
    /// A struct typed for the base element type. If the cast fails, an instance is returned whose <see cref="P:Mafi.Collections.ImmutableCollections.ImmutableArray`1.IsValid" /> property returns <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Arrays of derived elements types can be cast to arrays of base element types without reallocating the array.
    /// These up-casts can be reversed via this same method, casting an array of base element types to their derived
    /// types. However, down-casting is only successful when it reverses a prior up-casting operation.
    /// </remarks>
    [Pure]
    public ImmutableArray<TOther> As<TOther>() where TOther : class
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.m_items == null)
      {
        Mafi.Log.Error("As() called on invalid immutable array.");
        return ImmutableArray<TOther>.Empty;
      }
      if (immutableArray.m_items.Length == 0)
        return ImmutableArray<TOther>.Empty;
      if (immutableArray.m_items is TOther[] items)
        return new ImmutableArray<TOther>(items);
      Mafi.Log.Error("Failed to reinterpret immutable array of '" + immutableArray.m_items.First<T>().GetType().Name + "' to '" + typeof (TOther).Name + "'.");
      return ImmutableArray<TOther>.Empty;
    }

    /// <summary>
    /// Filters the elements of this array to those assignable to the specified type. If you need just a cast use
    /// <see cref="M:Mafi.Collections.ImmutableCollections.ImmutableArray`1.CastArray``1" />.
    /// </summary>
    [Pure]
    public ImmutableArray<TResult> OfType<TResult>()
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.m_items == null || immutableArray.m_items.Length == 0)
        return ImmutableArray<TResult>.Empty;
      Lyst<TResult> lyst = new Lyst<TResult>();
      foreach (T obj in immutableArray.m_items)
      {
        if (obj is TResult result)
          lyst.Add(result);
      }
      return lyst.ToImmutableArrayAndClear();
    }

    [Pure]
    public Lyst<TResult> OfTypeToLyst<TResult>()
    {
      ImmutableArray<T> immutableArray = this;
      Lyst<TResult> lyst = new Lyst<TResult>();
      if (immutableArray.m_items == null || immutableArray.m_items.Length == 0)
        return lyst;
      foreach (T obj in immutableArray.m_items)
      {
        if (obj is TResult result)
          lyst.Add(result);
      }
      return lyst;
    }

    /// <summary>Returns new array that is reversed.</summary>
    [Pure]
    public ImmutableArray<T> Reversed()
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.Length <= 1)
        return immutableArray;
      T[] items = new T[immutableArray.Length];
      int num = immutableArray.Length - 1;
      for (int index = 0; index < items.Length; ++index)
        items[index] = immutableArray.m_items[num - index];
      return new ImmutableArray<T>(items);
    }

    /// <summary>
    /// Returns new array that is concatenation of this and given arrays.
    /// </summary>
    [Pure]
    public ImmutableArray<T> Concat(ImmutableArray<T> other)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return other;
      if (other.IsEmpty)
        return immutableArray;
      T[] objArray = new T[immutableArray.m_items.Length + other.m_items.Length];
      Array.Copy((Array) immutableArray.m_items, (Array) objArray, immutableArray.m_items.Length);
      Array.Copy((Array) other.m_items, 0, (Array) objArray, immutableArray.m_items.Length, other.m_items.Length);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Returns new array that is concatenation of this and given arrays.
    /// </summary>
    [Pure]
    public ReadOnlyArraySlice<T> Concat(ReadOnlyArraySlice<T> other)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return other;
      if (other.IsEmpty)
        return immutableArray.AsSlice;
      T[] objArray = new T[immutableArray.m_items.Length + other.Length];
      Array.Copy((Array) immutableArray.m_items, (Array) objArray, immutableArray.m_items.Length);
      other.CopyTo(objArray, immutableArray.m_items.Length);
      return objArray.AsSlice<T>();
    }

    [Pure]
    public ImmutableArray<T> Concat(ImmutableArray<T> other, int otherIndex, int length)
    {
      if (length == 0)
        return this;
      Mafi.Assert.That<int>(otherIndex + length).IsLessOrEqual(other.Length);
      ImmutableArray<T> immutableArray = this;
      T[] objArray = new T[immutableArray.m_items.Length + length];
      Array.Copy((Array) immutableArray.m_items, (Array) objArray, immutableArray.m_items.Length);
      Array.Copy((Array) other.m_items, otherIndex, (Array) objArray, immutableArray.m_items.Length, length);
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Returns this array as <see cref="T:System.Collections.Generic.IEnumerable`1" />.
    /// </summary>
    [Pure]
    public IEnumerable<T> AsEnumerable()
    {
      ImmutableArray<T> immutableArray = this;
      return immutableArray.m_items == null ? Enumerable.Empty<T>() : ((IEnumerable<T>) immutableArray.m_items).AsEnumerable<T>();
    }

    [Pure]
    public T FirstOrDefault() => ((IEnumerable<T>) this.m_items).FirstOrDefault<T>();

    /// <summary>
    /// Returns the first element of the sequence that satisfies a condition or a default value if no such element is
    /// found.
    /// </summary>
    [Pure]
    public T FirstOrDefault(Func<T, bool> predicate)
    {
      return ((IEnumerable<T>) this.m_items).FirstOrDefault<T>(predicate);
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more
    /// than one such element exists.
    /// </summary>
    [Pure]
    public T Single(Func<T, bool> predicate)
    {
      return ((IEnumerable<T>) this.m_items).Single<T>(predicate);
    }

    /// <summary>
    /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
    /// </summary>
    [Pure]
    public IEnumerable<T> Skip(int count) => ((IEnumerable<T>) this.m_items).Skip<T>(count);

    /// <summary>
    /// Maps this immutable array using given function. This operation is not lazy like LINQ methods but is very
    /// efficient.
    /// </summary>
    [Pure]
    public ImmutableArray<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return ImmutableArray<TResult>.Empty;
      TResult[] items = new TResult[immutableArray.Length];
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        items[index] = mapper(immutableArray[index]);
      return new ImmutableArray<TResult>(items);
    }

    [Pure]
    public ImmutableArray<TResult> Map<TResult, TArg>(TArg arg, Func<T, TArg, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return ImmutableArray<TResult>.Empty;
      TResult[] items = new TResult[immutableArray.Length];
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        items[index] = mapper(immutableArray[index], arg);
      return new ImmutableArray<TResult>(items);
    }

    [Pure]
    public ImmutableArray<TResult> Map<TResult, TArg1, TArg2>(
      TArg1 arg1,
      TArg2 arg2,
      Func<T, TArg1, TArg2, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return ImmutableArray<TResult>.Empty;
      TResult[] items = new TResult[immutableArray.Length];
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        items[index] = mapper(immutableArray[index], arg1, arg2);
      return new ImmutableArray<TResult>(items);
    }

    [Pure]
    public ImmutableArray<TResult> Map<TResult, TArg1, TArg2, TArg3>(
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      Func<T, TArg1, TArg2, TArg3, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return ImmutableArray<TResult>.Empty;
      TResult[] items = new TResult[immutableArray.Length];
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        items[index] = mapper(immutableArray[index], arg1, arg2, arg3);
      return new ImmutableArray<TResult>(items);
    }

    [Pure]
    public ImmutableArray<TResult> Map<TResult>(Func<T, int, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return ImmutableArray<TResult>.Empty;
      TResult[] items = new TResult[immutableArray.Length];
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        items[index] = mapper(immutableArray[index], index);
      return new ImmutableArray<TResult>(items);
    }

    /// <summary>
    /// Maps this immutable array using given function. This operation is not lazy like LINQ methods but is very
    /// efficient.
    /// </summary>
    [Pure]
    public TResult[] MapArray<TResult>(Func<T, TResult> selector)
    {
      return this.m_items.MapArray<T, TResult>(selector);
    }

    [Pure]
    public Lyst<TResult> MapList<TResult>(Func<T, TResult> mapper)
    {
      ImmutableArray<T> immutableArray = this;
      if (immutableArray.IsEmpty)
        return new Lyst<TResult>();
      Lyst<TResult> lyst = new Lyst<TResult>(immutableArray.Length);
      int index = 0;
      for (int length = immutableArray.Length; index < length; ++index)
        lyst.Add(mapper(immutableArray[index]));
      return lyst;
    }

    /// <summary>Calls given action on all elements of this array.</summary>
    public void ForEach(Action<T> action)
    {
      foreach (T obj in this.m_items)
        action(obj);
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    [Pure]
    public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
    {
      return ((IEnumerable<T>) this.m_items).Select<T, TResult>(selector);
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    [Pure]
    public IEnumerable<TResult> Select<TResult>(Func<T, int, TResult> selector)
    {
      return ((IEnumerable<T>) this.m_items).Select<T, TResult>(selector);
    }

    /// <summary>Selects all non-None values.</summary>
    [Pure]
    public IEnumerable<TResult> SelectValues<TResult>(Func<T, Option<TResult>> selector) where TResult : class
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TResult>) new ImmutableArray<T>.\u003CSelectValues\u003Ed__117<TResult>(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__selector = selector
      };
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    [Pure]
    public IEnumerable<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector)
    {
      return ((IEnumerable<T>) this.m_items).SelectMany<T, TResult>(selector);
    }

    [Pure]
    public TComp Min<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] items = this.m_items;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (items.Length == 0)
        throw new InvalidOperationException("Min: No elements in sequence.");
      TComp y = selector(items[0]);
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < items.Length; ++index)
      {
        TComp x = selector(items[index]);
        if (comparer.Compare(x, y) < 0)
          y = x;
      }
      return y;
    }

    [Pure]
    public TComp Max<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] items = this.m_items;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (items.Length == 0)
        throw new InvalidOperationException("Max: No elements in sequence.");
      TComp y = selector(items[0]);
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < items.Length; ++index)
      {
        T obj = items[index];
        TComp x = selector(obj);
        if (comparer.Compare(x, y) > 0)
          y = x;
      }
      return y;
    }

    [Pure]
    public T MinElement<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] items = this.m_items;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      T obj1 = items.Length != 0 ? items[0] : throw new InvalidOperationException("MinElement: No elements in sequence.");
      TComp y = selector(obj1);
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < items.Length; ++index)
      {
        T obj2 = items[index];
        TComp x = selector(obj2);
        if (comparer.Compare(x, y) < 0)
        {
          y = x;
          obj1 = obj2;
        }
      }
      return obj1;
    }

    [Pure]
    public T MaxElement<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] items = this.m_items;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      T obj1 = items.Length != 0 ? items[0] : throw new InvalidOperationException("MinElement: No elements in sequence.");
      TComp y = selector(obj1);
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < items.Length; ++index)
      {
        T obj2 = items[index];
        TComp x = selector(obj2);
        if (comparer.Compare(x, y) > 0)
        {
          y = x;
          obj1 = obj2;
        }
      }
      return obj1;
    }

    [Pure]
    public int MinIndex<TComp>(Func<T, TComp> selector) where TComp : IComparable<TComp>
    {
      T[] items = this.m_items;
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (items.Length == 0)
        throw new InvalidOperationException("MinIndex: No elements in sequence.");
      TComp y = selector(items[0]);
      int num = 0;
      Comparer<TComp> comparer = Comparer<TComp>.Default;
      for (int index = 1; index < items.Length; ++index)
      {
        T obj = items[index];
        TComp x = selector(obj);
        if (comparer.Compare(x, y) < 0)
        {
          y = x;
          num = index;
        }
      }
      return num;
    }

    [Pure]
    public int Count(Func<T, bool> predicate)
    {
      int num = 0;
      foreach (T obj in this.m_items)
      {
        if (predicate(obj))
          ++num;
      }
      return num;
    }

    [Pure]
    public int Sum(Func<T, int> selector)
    {
      int num = 0;
      foreach (T obj in this.m_items)
        num += selector(obj);
      return num;
    }

    [Pure]
    public Fix32 Sum(Func<T, Fix32> selector)
    {
      Fix32 zero = Fix32.Zero;
      foreach (T obj in this.m_items)
        zero += selector(obj);
      return zero;
    }

    /// <summary>Filters a sequence of values based on a predicate.</summary>
    [Pure]
    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
      return ((IEnumerable<T>) this.m_items).Where<T>(predicate);
    }

    [Pure]
    public T SampleRandomOrDefault(IRandom random)
    {
      ImmutableArray<T> immutableArray = this;
      switch (immutableArray.Length)
      {
        case 0:
          return default (T);
        case 1:
          return immutableArray.m_items[0];
        default:
          return immutableArray.m_items[random.NextInt(0, immutableArray.m_items.Length)];
      }
    }

    [Pure]
    public T SampleRandomOrDefault(IRandom random, Func<T, bool> predicate)
    {
      ImmutableArray<T> immutableArray = this;
      int maxValueExcl = 0;
      foreach (T obj in immutableArray)
      {
        if (predicate(obj))
          ++maxValueExcl;
      }
      int num = random.NextInt(maxValueExcl);
      foreach (T obj in immutableArray)
      {
        if (predicate(obj))
        {
          --num;
          if (num < 0)
            return obj;
        }
      }
      return default (T);
    }

    [Pure]
    public T SampleRandomWeighted(IRandom random, int weightsSum, Func<T, int> weightSelector)
    {
      return this.SampleRandomWeighted(random.NextInt(weightsSum), weightSelector);
    }

    [Pure]
    public T SampleRandomWeighted(int threshold, Func<T, int> weightSelector)
    {
      ImmutableArray<T> immutableArray = this;
      int num = 0;
      foreach (T obj in immutableArray)
      {
        num += weightSelector(obj);
        if (num > threshold)
          return obj;
      }
      Mafi.Log.Error("Failed to select weighted random. The given `threshold` value is likely too large.");
      return this.Last;
    }

    [Pure]
    public IEnumerable<Pair<T, TOther>> Zip<TOther>(ImmutableArray<TOther> other)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Pair<T, TOther>>) new ImmutableArray<T>.\u003CZip\u003Ed__132<TOther>(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__other = other
      };
    }

    /// <summary>
    /// Determines whether all elements of a sequence satisfy given predicate (without allocations).
    /// </summary>
    [Pure]
    public bool All(Func<T, bool> predicate)
    {
      foreach (T obj in this.m_items)
      {
        if (!predicate(obj))
          return false;
      }
      return true;
    }

    /// <summary>
    /// Determines whether any element of a sequence satisfy given predicate (without allocations).
    /// </summary>
    [Pure]
    public bool Any(Func<T, bool> predicate)
    {
      foreach (T obj in this.m_items)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    /// <summary>Applies an accumulator function over a sequence.</summary>
    [Pure]
    public T Aggregate(Func<T, T, T> func) => ((IEnumerable<T>) this.m_items).Aggregate<T>(func);

    /// <summary>
    /// Sorts the elements of a sequence in ascending order according to a key.
    /// </summary>
    [Pure]
    public IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector)
    {
      return (IEnumerable<T>) ((IEnumerable<T>) this.m_items).OrderBy<T, TKey>(keySelector);
    }

    [Pure]
    public T[] ConcatToArray(T[] other) => this.m_items.Concatenate<T>(other);

    /// <summary>
    /// Returns an array with items at the specified indexes removed.
    /// </summary>
    /// <param name="indexesToRemove">
    /// A **sorted set** of indexes to elements that should be omitted from the returned array.
    /// </param>
    /// <returns>The new array.</returns>
    [Pure]
    private ImmutableArray<T> removeAtRange(ICollection<int> indexesToRemove)
    {
      Mafi.Assert.That<ICollection<int>>(indexesToRemove).IsNotNull<ICollection<int>>();
      ImmutableArray<T> immutableArray = this;
      if (indexesToRemove.Count == 0)
        return immutableArray;
      int num1 = immutableArray.Length - indexesToRemove.Count;
      if (num1 <= 0)
      {
        Mafi.Assert.That<int>(num1).IsNotNegative();
        return ImmutableArray<T>.Empty;
      }
      T[] objArray = new T[immutableArray.Length - indexesToRemove.Count];
      int destinationIndex = 0;
      int num2 = 0;
      int expected = -1;
      foreach (int num3 in (IEnumerable<int>) indexesToRemove)
      {
        int length = expected == -1 ? num3 : num3 - expected - 1;
        Mafi.Assert.That<int>(num3).IsGreater(expected);
        Array.Copy((Array) immutableArray.m_items, destinationIndex + num2, (Array) objArray, destinationIndex, length);
        ++num2;
        destinationIndex += length;
        expected = num3;
      }
      Array.Copy((Array) immutableArray.m_items, destinationIndex + num2, (Array) objArray, destinationIndex, immutableArray.Length - (destinationIndex + num2));
      return new ImmutableArray<T>(objArray);
    }

    /// <summary>
    /// Allows tp type <c>ImmutableArray.Empty</c> instead of <c>ImmutableArray{T}.Empty</c>. This can in some cases
    /// improve readability of the code if <c>T</c> is very long.
    /// </summary>
    public static implicit operator ImmutableArray<T>(ImmutableArray.EmptyArray _)
    {
      return ImmutableArray<T>.Empty;
    }

    public static void Serialize(ImmutableArray<T> value, BlobWriter writer)
    {
      writer.WriteArray<T>(value.m_items);
    }

    public static ImmutableArray<T> Deserialize(BlobReader reader)
    {
      return new ImmutableArray<T>(reader.ReadArray<T>());
    }

    static ImmutableArray()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ImmutableArray<T>.Empty = new ImmutableArray<T>(Array.Empty<T>());
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
      private int m_index;

      internal Enumerator(T[] array)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_array = array.CheckNotNull<T[]>("Enumerating null array.");
        this.m_index = -1;
      }

      public T Current => this.m_array[this.m_index];

      public bool MoveNext() => ++this.m_index < this.m_array.Length;
    }
  }
}
