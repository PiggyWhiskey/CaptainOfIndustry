// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  /// <summary>
  /// A readonly array with O(1) indexable lookup time. This provides weaker guarantee than <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> since this array's elements are allowed to be mutated by classes that have a reference
  /// to the underlying array.
  /// </summary>
  /// <typeparam name="T">The type of element stored by the array.</typeparam>
  /// <example>
  /// A typical example of usage is providing public read-only view of a private array.
  /// <code>
  /// class Example {
  /// private readonly int[] m_numbers = new int[10];
  /// 
  /// public readonly ReadOnlyArray&lt;int&gt; Numbers =&gt; m_numbers.AsReadOnlyArray();
  /// 
  /// public void DoWork() {
  /// m_numbers[5] += 42; // This change will be propagated to all owners of ReadOnlyArray.
  /// }
  /// }
  /// </code>
  /// </example>
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
  [DebuggerDisplay("Length={Length}")]
  public struct ReadOnlyArray<T> : IReadOnlyArray, IEquatable<ReadOnlyArray<T>>
  {
    /// <summary>
    /// An empty (initialized) instance of <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" />.
    /// </summary>
    public static readonly ReadOnlyArray<T> Empty;
    /// <summary>
    /// The backing field for this instance. References to this value should never be shared with outside code.
    /// </summary>
    private readonly T[] m_array;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> struct *without making a defensive copy*.
    /// </summary>
    /// <param name="items">The array to use. May be null for "default" arrays.</param>
    public ReadOnlyArray(T[] items)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_array = items;
    }

    /// <summary>
    /// Gets the element at the specified index in the read-only list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index in the read-only list.</returns>
    public T this[int index] => this.m_array[index];

    /// <summary>Gets the length of array in the collection.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Length => this.m_array.Length;

    /// <summary>Whether this array is empty.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty => this.m_array.Length == 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotEmpty => this.m_array.Length != 0;

    /// <summary>
    /// Whether this struct was initialized without an actual array instance.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotValid => this.m_array == null;

    /// <summary>Whether this struct is empty or uninitialized.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNullOrEmpty
    {
      get
      {
        ReadOnlyArray<T> readOnlyArray = this;
        return readOnlyArray.m_array == null || readOnlyArray.m_array.Length == 0;
      }
    }

    /// <summary>
    /// Gets the first element of the array. Throws exception if the array is null or empty.
    /// </summary>
    public T First => this.m_array[0];

    public T Second => this.m_array[1];

    /// <summary>
    /// Gets the last element of the array. Throws exception if the array is null or empty.
    /// </summary>
    public T Last
    {
      get
      {
        ReadOnlyArray<T> readOnlyArray = this;
        return readOnlyArray.m_array[readOnlyArray.m_array.Length - 1];
      }
    }

    public T PreLast
    {
      get
      {
        ReadOnlyArray<T> readOnlyArray = this;
        return readOnlyArray.m_array[readOnlyArray.m_array.Length - 2];
      }
    }

    /// <summary>
    /// Returns this array as indexable. This creates a new class instance to prevent boxing.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IIndexable<T> AsIndexable => (IIndexable<T>) new IndexableArray<T>(this.m_array);

    /// <summary>Gets an untyped boxed reference to the array.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Array IReadOnlyArray.Array => (Array) this.m_array;

    public ReadOnlyArraySlice<T> AsSlice => new ReadOnlyArraySlice<T>(this.m_array);

    /// <summary>Checks equality between two instances.</summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns><c>true</c> if the values' underlying arrays are reference equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
    {
      return left.Equals(right);
    }

    /// <summary>Checks inequality between two instances.</summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns>
    /// <c>true</c> if the values' underlying arrays are reference not equal; <c>false</c> otherwise.
    /// </returns>
    public static bool operator !=(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
    {
      return !left.Equals(right);
    }

    /// <summary>Checks equality between two instances.</summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns><c>true</c> if the values' underlying arrays are reference equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(ReadOnlyArray<T>? left, ReadOnlyArray<T>? right)
    {
      return left.GetValueOrDefault().Equals(right.GetValueOrDefault());
    }

    /// <summary>Checks inequality between two instances.</summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns>
    /// <c>true</c> if the values' underlying arrays are reference not equal; <c>false</c> otherwise.
    /// </returns>
    public static bool operator !=(ReadOnlyArray<T>? left, ReadOnlyArray<T>? right)
    {
      return !left.GetValueOrDefault().Equals(right.GetValueOrDefault());
    }

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item) => Array.IndexOf<T>(this.m_array, item);

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex) => Array.IndexOf<T>(this.m_array, item, startIndex);

    /// <summary>Searches the array for the specified item.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex, int count)
    {
      return Array.IndexOf((Array) this.m_array, (object) startIndex, count);
    }

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item) => Array.LastIndexOf<T>(this.m_array, item);

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex)
    {
      return Array.LastIndexOf((Array) this.m_array, (object) startIndex);
    }

    /// <summary>Searches the array for the specified item in reverse.</summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex, int count)
    {
      return Array.LastIndexOf((Array) this.m_array, (object) startIndex, count);
    }

    /// <summary>
    /// Determines whether the specified item exists in the array.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns><c>true</c> if an equal value was found in the array; <c>false</c> otherwise.</returns>
    [Pure]
    public bool Contains(T item) => this.IndexOf(item) >= 0;

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    /// <param name="destination">The array to copy to.</param>
    public void CopyTo(T[] destination)
    {
      ReadOnlyArray<T> readOnlyArray = this;
      readOnlyArray.throwNullRefIfNotInitialized();
      Array.Copy((Array) readOnlyArray.m_array, 0, (Array) destination, 0, readOnlyArray.Length);
    }

    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> for a specific element, using the
    /// <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> and by the specified object.
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
    public int BinarySearch(T value) => Array.BinarySearch<T>(this.m_array, value);

    /// <summary>Returns an enumerator for the contents of the array.</summary>
    [Pure]
    public ReadOnlyArray<T>.Enumerator GetEnumerator()
    {
      return new ReadOnlyArray<T>.Enumerator(this.m_array);
    }

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    [Pure]
    public override int GetHashCode()
    {
      ReadOnlyArray<T> readOnlyArray = this;
      return readOnlyArray.m_array != null ? readOnlyArray.m_array.GetHashCode() : 0;
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public override bool Equals(object obj)
    {
      return obj is IReadOnlyArray readOnlyArray && this.m_array == readOnlyArray.Array;
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
    /// </returns>
    [Pure]
    public bool Equals(ReadOnlyArray<T> other) => this.m_array == other.m_array;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> struct based on the contents of an existing
    /// instance, allowing a covariant static cast to efficiently reuse the existing array.
    /// </summary>
    /// <param name="items">The array to initialize the array with. No copy is made.</param>
    /// <remarks>
    /// Covariant up-casts from this method may be reversed by calling the <see cref="M:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1.As``1" />
    /// or <see cref="M:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1.CastArray``1" /> method.
    /// </remarks>
    public static ReadOnlyArray<T> CastUp<TDerived>(ReadOnlyArray<TDerived> items) where TDerived : class, T
    {
      return new ReadOnlyArray<T>((T[]) items.m_array);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1" /> struct by casting the underlying array to an
    /// array of type <typeparam name="TOther" /> .
    /// </summary>
    /// <exception cref="T:System.InvalidCastException">Thrown if the cast is illegal.</exception>
    [Pure]
    public ReadOnlyArray<TOther> CastArray<TOther>() where TOther : class
    {
      return new ReadOnlyArray<TOther>((TOther[]) this.m_array);
    }

    /// <summary>
    /// Creates an immutable array for this array, cast to a different element type.
    /// </summary>
    /// <typeparam name="TOther">The type of array element to return.</typeparam>
    /// <returns>
    /// A struct typed for the base element type. If the cast fails, an instance is returned whose <see cref="P:Mafi.Collections.ReadonlyCollections.ReadOnlyArray`1.IsNotValid" /> property returns <c>true</c>.
    /// </returns>
    /// <remarks>
    /// Arrays of derived elements types can be cast to arrays of base element types without reallocating the array.
    /// These up-casts can be reversed via this same method, casting an array of base element types to their derived
    /// types. However, down-casting is only successful when it reverses a prior up-casting operation.
    /// </remarks>
    [Pure]
    public ReadOnlyArray<TOther> As<TOther>() where TOther : class
    {
      return new ReadOnlyArray<TOther>(this.m_array as TOther[]);
    }

    /// <summary>
    /// Filters the elements of this array to those assignable to the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
    /// <returns>
    /// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence of type <typeparamref name="TResult" />.
    /// </returns>
    [Pure]
    public IEnumerable<TResult> OfType<TResult>()
    {
      ReadOnlyArray<T> readOnlyArray = this;
      return readOnlyArray.m_array == null || readOnlyArray.m_array.Length == 0 ? Enumerable.Empty<TResult>() : readOnlyArray.m_array.OfType<TResult>();
    }

    /// <summary>
    /// Returns this array as <see cref="T:System.Collections.Generic.IEnumerable`1" />.
    /// </summary>
    [Pure]
    public IEnumerable<T> AsEnumerable() => ((IEnumerable<T>) this.m_array).AsEnumerable<T>();

    /// <summary>
    /// Returns the first element of the sequence that satisfies a condition or a default value if no such element is
    /// found.
    /// </summary>
    [Pure]
    public T FirstOrDefault(Func<T, bool> predicate)
    {
      return ((IEnumerable<T>) this.m_array).FirstOrDefault<T>(predicate);
    }

    /// <summary>
    /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
    /// </summary>
    [Pure]
    public IEnumerable<T> Skip(int count) => ((IEnumerable<T>) this.m_array).Skip<T>(count);

    /// <summary>
    /// Map this array to a new read-only array using a mapper function.
    /// </summary>
    [Pure]
    public ReadOnlyArray<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
      return new ReadOnlyArray<TResult>(this.m_array.MapArray<T, TResult>(mapper));
    }

    /// <summary>
    /// Map this array to a new mutable array using a mapper function.
    /// </summary>
    [Pure]
    public TResult[] MapArray<TResult>(Func<T, TResult> mapper)
    {
      return this.m_array.MapArray<T, TResult>(mapper);
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    [Pure]
    public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
    {
      return ((IEnumerable<T>) this.m_array).Select<T, TResult>(selector);
    }

    /// <summary>Filters a sequence of values based on a predicate.</summary>
    [Pure]
    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
      return ((IEnumerable<T>) this.m_array).Where<T>(predicate);
    }

    /// <summary>Applies an accumulator function over a sequence.</summary>
    [Pure]
    public T Aggregate(Func<T, T, T> func) => ((IEnumerable<T>) this.m_array).Aggregate<T>(func);

    /// <summary>
    /// Sorts the elements of a sequence in ascending order according to a key.
    /// </summary>
    [Pure]
    public IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector)
    {
      return (IEnumerable<T>) ((IEnumerable<T>) this.m_array).OrderBy<T, TKey>(keySelector);
    }

    [Pure]
    public T[] ConcatToArray(T[] other) => this.m_array.Concatenate<T>(other);

    /// <summary>
    /// Throws a null reference exception if the array field is null.
    /// </summary>
    private void throwNullRefIfNotInitialized()
    {
      int length = this.m_array.Length;
    }

    static ReadOnlyArray()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ReadOnlyArray<T>.Empty = new ReadOnlyArray<T>(Array.Empty<T>());
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
        this.m_array = array.CheckNotNull<T[]>();
        this.m_index = -1;
      }

      public T Current => this.m_array[this.m_index];

      public bool MoveNext() => ++this.m_index < this.m_array.Length;
    }
  }
}
