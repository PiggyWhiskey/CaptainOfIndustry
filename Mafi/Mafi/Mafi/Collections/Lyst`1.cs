// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Lyst`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections
{
  [GenerateSerializer(true, null, 0)]
  public sealed class Lyst<T> : 
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    IIndexable<T>,
    ICollectionWithCount,
    IReadOnlyCollection<T>,
    ILystNonGeneric
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_OmittedClearing;
    private static readonly string s_pcKey_SavedByTrimExcess;
    public const int MAX_ARRAY_LENGTH = 2146435071;
    /// <summary>
    /// Default capacity when the first item is added to an empty list.
    /// </summary>
    public const int DEFAULT_CAPACITY = 4;
    /// <summary>Storage for the items.</summary>
    private T[] m_items;
    /// <summary>Number of currently stored items.</summary>
    private int m_size;
    /// <summary>Whether zeroing-out of the array can be omitted.</summary>
    private readonly bool m_canOmitClearing;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private static void incVersion()
    {
    }

    /// <summary>
    /// Constructs a new List. The list is initially empty and has a capacity of zero. Upon adding the first element
    /// to the list the capacity is increased to 4, and then increased in multiples of two as required.
    /// </summary>
    public Lyst()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_items = Array.Empty<T>();
    }

    /// <param name="canOmitClearing">
    /// Whether this list can omit clearing of internal array when <see cref="M:Mafi.Collections.Lyst`1.Clear" /> is called or items are removed
    /// from the list. If this is set to true please keep in mind that any references inserted into the list may be
    /// referenced until this list is referenced and prevent them from being garbage collected.
    /// </param>
    public Lyst(bool canOmitClearing)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_items = Array.Empty<T>();
      this.m_canOmitClearing = canOmitClearing;
    }

    /// <summary>
    /// Constructs a List with a given initial <paramref name="capacity" />. The list is initially empty, but will
    /// have room for the given number of elements before any reallocations are required.
    /// </summary>
    public Lyst(int capacity, bool canOmitClearing = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Mafi.Assert.That<int>(capacity).IsNotNegative();
      this.m_items = capacity > 0 ? new T[capacity] : Array.Empty<T>();
      this.m_canOmitClearing = canOmitClearing;
    }

    /// <summary>
    /// Constructs a new List that contains all the elements in given sequence.
    /// </summary>
    public Lyst(IEnumerable<T> elements)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_items = Array.Empty<T>();
      this.AddRange(elements);
    }

    public Lyst(IEnumerable<T> elements, int initialCapacity)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_items = Array.Empty<T>();
      this.extendCapacity(initialCapacity);
      this.AddRange(elements);
    }

    private void validateAfterLoad()
    {
      if (this.m_items == null)
      {
        Mafi.Log.Error(string.Format("Invalid Lyst after load: Null backing array, count={0}.", (object) this.Count));
        this.m_items = Array.Empty<T>();
        this.Count = 0;
      }
      else if (this.Count < 0)
      {
        Mafi.Log.Error(string.Format("Invalid Lyst after load: Negative count `{0}`.", (object) this.Count));
        this.Count = 0;
      }
      else
      {
        if (this.Count <= this.m_items.Length)
          return;
        Mafi.Log.Error(string.Format("Invalid Lyst after load: Count `{0}` greater than array size `{1}`.", (object) this.Count, (object) this.m_items.Length));
        this.Count = this.m_items.Length;
      }
    }

    /// <summary>
    /// Gets or sets the capacity of this list. The capacity is the size of the internal array used to hold items.
    /// When set, the internal array of the list is reallocated to the given capacity.
    /// </summary>
    public int Capacity
    {
      get => this.m_items.Length;
      set
      {
        if (value < this.m_size)
          throw new ArgumentOutOfRangeException(nameof (Capacity));
        if (value > 0)
          Array.Resize<T>(ref this.m_items, value);
        else
          this.m_items = Array.Empty<T>();
      }
    }

    /// <summary>Gets or sets number of stored elements.</summary>
    public int Count
    {
      get => this.m_size;
      set => this.ChangeCount(value);
    }

    public void ChangeCount(int newSize)
    {
      if (newSize < 0)
        throw new ArgumentOutOfRangeException("Count");
      if (newSize < this.m_size)
      {
        if (!this.m_canOmitClearing)
          Array.Clear((Array) this.m_items, newSize, this.m_size - newSize);
      }
      else
        this.EnsureCapacity(newSize);
      this.m_size = newSize;
      Lyst<T>.incVersion();
    }

    /// <summary>Returns whether this Lyst is empty.</summary>
    public bool IsEmpty => this.m_size <= 0;

    /// <summary>Returns whether this Lyst is NOT empty.</summary>
    public bool IsNotEmpty => this.m_size > 0;

    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the first element at index 0 of this Lyst.
    /// It is callers responsibility to check whether this Lyst is not empty.
    /// </summary>
    public T First
    {
      get
      {
        if (this.m_size == 0)
          throw new InvalidOperationException("Getting First element of an empty Lyst.");
        return this.m_items[0];
      }
      set
      {
        if (this.m_size == 0)
          throw new InvalidOperationException("Setting First element of an empty Lyst.");
        this.m_items[0] = value;
        Lyst<T>.incVersion();
      }
    }

    /// <summary>
    /// Gets or sets the last element at index <c>Count - 1</c> of this Lyst.
    /// It is callers responsibility to check whether this Lyst is not empty.
    /// </summary>
    public T Last
    {
      get
      {
        return this.m_size != 0 ? this.m_items[this.m_size - 1] : throw new InvalidOperationException("Getting Last element of an empty Lyst.");
      }
      set
      {
        if (this.m_size == 0)
          throw new InvalidOperationException("Setting Last element of an empty Lyst.");
        this.m_items[this.m_size - 1] = value;
        Lyst<T>.incVersion();
      }
    }

    /// <summary>
    /// Returns second to the last element at index <c>Count - 2</c> of this Lyst. It is callers responsibility to
    /// check whether this Lyst is not empty.
    /// </summary>
    public T PreLast
    {
      get
      {
        return this.m_size > 1 ? this.m_items[this.m_size - 2] : throw new InvalidOperationException("Getting pre-last element of an empty Lyst.");
      }
      set
      {
        if (this.m_size <= 1)
          throw new InvalidOperationException("Setting pre-last element of an empty Lyst.");
        this.m_items[this.m_size - 2] = value;
        Lyst<T>.incVersion();
      }
    }

    /// <summary>Gets or sets the element at the given index.</summary>
    public T this[int index]
    {
      get
      {
        return (uint) index < (uint) this.m_size ? this.m_items[index] : throw new IndexOutOfRangeException(nameof (index));
      }
      set
      {
        if ((uint) index >= (uint) this.m_size)
          throw new IndexOutOfRangeException(nameof (index));
        this.m_items[index] = value;
        Lyst<T>.incVersion();
      }
    }

    public T this[uint index]
    {
      get
      {
        return index < (uint) this.m_size ? this.m_items[(int) index] : throw new IndexOutOfRangeException(nameof (index));
      }
      set
      {
        if (index >= (uint) this.m_size)
          throw new IndexOutOfRangeException(nameof (index));
        this.m_items[(int) index] = value;
        Lyst<T>.incVersion();
      }
    }

    public ref T GetRefAt(int index)
    {
      if ((uint) index >= (uint) this.m_size)
        throw new IndexOutOfRangeException(nameof (index));
      return ref this.m_items[index];
    }

    object ILystNonGeneric.this[int i]
    {
      get => (object) this[i];
      set => this[i] = (T) value;
    }

    /// <summary>Gets or sets random element.</summary>
    public T this[IRandom random]
    {
      get
      {
        switch (this.m_size)
        {
          case 0:
            throw new InvalidOperationException("Getting random element of an empty Lyst.");
          case 1:
            return this.m_items[0];
          default:
            return this.m_items[random.NextInt(this.m_size)];
        }
      }
      set
      {
        if (this.m_size == 0)
          throw new InvalidOperationException("Setting random element of an empty Lyst.");
        this.m_items[random.NextInt(this.m_size)] = value;
        Lyst<T>.incVersion();
      }
    }

    /// <summary>
    /// Returns slice from the internal backing array. Be extremely careful as any changes to the list may also
    /// propagate to the returned array.
    /// </summary>
    public ReadOnlyArraySlice<T> BackingArrayAsSlice
    {
      get => new ReadOnlyArraySlice<T>(this.m_items, 0, this.m_size);
    }

    public T[] GetBackingArray() => this.m_items;

    /// <summary>
    /// Adds the given object to the end of this list. The size of the list is increased by one. If required, the
    /// capacity of the list is doubled before adding the new element.
    /// </summary>
    public void Add(T item)
    {
      if (this.m_size >= this.m_items.Length)
        this.extendCapacity(this.m_size + 1);
      this.m_items[this.m_size] = item;
      ++this.m_size;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds two items to the en of this list. TODO: Rename to AddRange. This overload can cause subtle bugs.
    /// </summary>
    public void Add(T item1, T item2)
    {
      this.EnsureCapacity(this.m_size + 2);
      this.m_items[this.m_size] = item1;
      this.m_items[this.m_size + 1] = item2;
      this.m_size += 2;
      Lyst<T>.incVersion();
    }

    /// <summary>Adds three items to the en of this list.</summary>
    public void Add(T item1, T item2, T item3)
    {
      this.EnsureCapacity(this.m_size + 3);
      this.m_items[this.m_size] = item1;
      this.m_items[this.m_size + 1] = item2;
      this.m_items[this.m_size + 2] = item3;
      this.m_size += 3;
      Lyst<T>.incVersion();
    }

    /// <summary>Adds four items to the en of this list.</summary>
    public void Add(T item1, T item2, T item3, T item4)
    {
      this.EnsureCapacity(this.m_size + 4);
      this.m_items[this.m_size] = item1;
      this.m_items[this.m_size + 1] = item2;
      this.m_items[this.m_size + 2] = item3;
      this.m_items[this.m_size + 3] = item4;
      this.m_size += 4;
      Lyst<T>.incVersion();
    }

    public void Add(T item1, T item2, T item3, T item4, T item5)
    {
      this.EnsureCapacity(this.m_size + 5);
      this.m_items[this.m_size] = item1;
      this.m_items[this.m_size + 1] = item2;
      this.m_items[this.m_size + 2] = item3;
      this.m_items[this.m_size + 3] = item4;
      this.m_items[this.m_size + 4] = item5;
      this.m_size += 5;
      Lyst<T>.incVersion();
    }

    void ILystNonGeneric.Add(object value) => this.Add((T) value);

    /// <summary>
    /// Adds item to the list if it is not already present. This is O(n) operation.
    /// </summary>
    /// <returns>True if given item was added, false when it is already in the list.</returns>
    public bool AddIfNotPresent(T item)
    {
      if (this.Contains(item))
        return false;
      this.Add(item);
      return true;
    }

    public void AddAssertNew(T item) => this.Add(item);

    /// <summary>
    /// Adds the elements of the given collection to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(IEnumerable<T> items)
    {
      if (items == null)
        throw new ArgumentNullException(nameof (items));
      if (items is IList<T> list)
      {
        this.AddRange(list);
      }
      else
      {
        foreach (T obj in items)
          this.Add(obj);
      }
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given array to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(T[] array)
    {
      int length = array.Length;
      if (length == 0)
        return;
      this.EnsureCapacity(this.m_size + length);
      Array.Copy((Array) array, 0, (Array) this.m_items, this.m_size, length);
      this.m_size += length;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given array to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(ReadOnlyArraySlice<T> array)
    {
      int length = array.Length;
      if (length == 0)
        return;
      this.EnsureCapacity(this.m_size + length);
      array.CopyTo(this.m_items, this.m_size);
      this.m_size += length;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given array to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(ImmutableArray<T> array)
    {
      int length = array.Length;
      if (length == 0)
        return;
      this.EnsureCapacity(this.m_size + length);
      array.CopyTo(this.m_items, this.m_size);
      this.m_size += length;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// <see cref="M:Mafi.Collections.Lyst`1.AddRange(Mafi.Collections.ImmutableCollections.ImmutableArray{`0})" />
    /// </summary>
    public void AddRange(SmallImmutableArray<T> array)
    {
      int length = array.Length;
      if (length == 0)
        return;
      this.EnsureCapacity(this.m_size + length);
      array.CopyTo(this.m_items, this.m_size);
      this.m_size += length;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given list to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(Lyst<T> list)
    {
      int count = list.Count;
      if (count == 0)
        return;
      this.EnsureCapacity(this.m_size + count);
      Array.Copy((Array) list.m_items, 0, (Array) this.m_items, this.m_size, count);
      this.m_size += count;
      Lyst<T>.incVersion();
    }

    public void AddRange(Lyst<T> list, int fromIndex, int count)
    {
      if (count == 0)
        return;
      this.EnsureCapacity(this.m_size + count);
      Array.Copy((Array) list.m_items, fromIndex, (Array) this.m_items, this.m_size, count);
      this.m_size += count;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given list to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(IList<T> list)
    {
      int count = list.Count;
      if (count == 0)
        return;
      this.EnsureCapacity(this.m_size + count);
      for (int index = 0; index < count; ++index)
        this.m_items[this.m_size + index] = list[index];
      this.m_size += count;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds the elements of the given indexable to the end of this list. If required, the capacity of the list is
    /// increased to twice the previous capacity or the new size, whichever is larger.
    /// </summary>
    public void AddRange(IIndexable<T> indexable)
    {
      int count = indexable.Count;
      if (count == 0)
        return;
      this.EnsureCapacity(this.m_size + count);
      for (int index = 0; index < count; ++index)
        this.m_items[this.m_size + index] = indexable[index];
      this.m_size += count;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Adds given <paramref name="item" /> to the list <paramref name="count" /> times.
    /// </summary>
    public void AddRepeated(T item, int count)
    {
      Mafi.Assert.That<int>(count).IsNotNegative();
      this.EnsureCapacity(this.m_size + count);
      for (int index = 0; index < count; ++index)
        this.m_items[this.m_size++] = item;
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Searches a section of the list for a given element using a binary search algorithm. Elements of the list are
    /// compared to the search value using the given IComparer interface. If comparer is null, elements of the list
    /// are compared to the search value using the IComparable interface, which in that case must be implemented by
    /// all elements of the list and the given search value. This method assumes that the given section of the list
    /// is already sorted; if this is not the case, the result will be incorrect.
    /// 
    /// The method returns the index of the given value in the list. If the list does not contain the given value,
    /// the method returns a negative integer. The bitwise complement operator (~) can be applied to a negative
    /// result to produce the index of the first element (if any) that is larger than the given search value. This is
    /// also the index at which the search value should be inserted into the list in order for the list to remain
    /// sorted.
    /// 
    /// Note that if there are repeated elements, index of any element from the repeated sequence is returned.
    /// 
    /// The method uses the Array.BinarySearch method to perform the search.
    /// </summary>
    [Pure]
    public int BinarySearch(int index, int count, T item, [CanBeNull] IComparer<T> comparer)
    {
      return Array.BinarySearch<T>(this.m_items, index, count, item, comparer);
    }

    [Pure]
    public int BinarySearch(T item) => this.BinarySearch(0, this.Count, item, (IComparer<T>) null);

    [Pure]
    public int BinarySearch(T item, IComparer<T> comparer)
    {
      return this.BinarySearch(0, this.Count, item, comparer);
    }

    /// <summary>Clears the contents of List.</summary>
    public void Clear()
    {
      if (this.m_size == 0)
        return;
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_items, 0, this.m_size);
      this.m_size = 0;
      Lyst<T>.incVersion();
    }

    public Lyst<T> ClearAndReturn()
    {
      this.Clear();
      return this;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (this.m_size == 0)
        return;
      Array.Copy((Array) this.m_items, 0, (Array) array, arrayIndex, this.m_size);
    }

    /// <summary>
    /// Ensures that the capacity of this list is at least the given minimum value. If the current capacity of the
    /// list is less than min, the capacity is increased to twice the current capacity or to min, whichever is
    /// larger.
    /// </summary>
    /// <remarks>
    /// This method is intentionally split to two which allows in-lining of the fast and common scenario when no
    /// extension is needed. This also allows to perform the extension of the capacity directly when we already know
    /// that it is needed.
    /// </remarks>
    public void EnsureCapacity(int minCapacity)
    {
      if (minCapacity <= this.m_items.Length)
        return;
      this.extendCapacity(minCapacity);
    }

    private void extendCapacity(int minCapacity)
    {
      int num = this.m_items.Length == 0 ? 4 : this.m_items.Length * 2;
      if ((uint) num > 2146435071U)
      {
        Mafi.Log.Error("Allocating max length Lyst with ~2G elements. Is this an error?");
        num = 2146435071;
      }
      if (num < minCapacity)
        num = minCapacity;
      this.Capacity = num;
    }

    /// <summary>
    /// Contains returns true if the specified element is in the List. It does a linear, O(n) search. Equality is
    /// determined by calling item.Equals().
    /// </summary>
    [Pure]
    public bool Contains(T item)
    {
      if ((object) item == null)
      {
        for (int index = 0; index < this.m_size; ++index)
        {
          if ((object) this.m_items[index] == null)
            return true;
        }
      }
      else
      {
        EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        for (int index = 0; index < this.m_size; ++index)
        {
          if (equalityComparer.Equals(this.m_items[index], item))
            return true;
        }
      }
      return false;
    }

    [Pure]
    public bool Contains(Predicate<T> predicate)
    {
      for (int index = 0; index < this.m_size; ++index)
      {
        if (predicate(this.m_items[index]))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Whether given predicate returns true on any item in the list. Returns false if the list is empty.
    /// </summary>
    [Pure]
    public bool Exists(Predicate<T> match) => this.FindIndex(match) >= 0;

    [Pure]
    public T Find(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
          return this.m_items[index];
      }
      return default (T);
    }

    [Pure]
    public Lyst<T> FindAll(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      Lyst<T> all = new Lyst<T>();
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
          all.Add(this.m_items[index]);
      }
      return all;
    }

    [Pure]
    public int FindIndex(Predicate<T> match)
    {
      if (match == null)
      {
        Mafi.Log.Error("FindIndex: Null match function.");
        return -1;
      }
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
          return index;
      }
      return -1;
    }

    /// <summary>
    /// Overload with an extra parameter that can be used in the <paramref name="match" /> function
    /// to avoid allocating lambda.
    /// </summary>
    [Pure]
    public int FindIndex<TParam>(TParam param, Func<T, TParam, bool> match)
    {
      if (match == null)
      {
        Mafi.Log.Error("FindIndex: Null match function.");
        return -1;
      }
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index], param))
          return index;
      }
      return -1;
    }

    [Pure]
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
      if (count <= 0)
        return -1;
      int num = startIndex + count;
      if (startIndex < 0)
        startIndex = 0;
      if (num > this.m_size)
        num = this.m_size;
      if (match == null)
      {
        Mafi.Log.Error("FindIndex: Null match function.");
        return -1;
      }
      for (int index = startIndex; index < num; ++index)
      {
        if (match(this.m_items[index]))
          return index;
      }
      return -1;
    }

    [Pure]
    public T FindLast(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      for (int index = this.m_size - 1; index >= 0; --index)
      {
        if (match(this.m_items[index]))
          return this.m_items[index];
      }
      return default (T);
    }

    [Pure]
    public int FindLastIndex(Predicate<T> match)
    {
      return this.FindLastIndex(this.m_size - 1, this.m_size, match);
    }

    [Pure]
    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
      return this.FindLastIndex(startIndex, startIndex + 1, match);
    }

    [Pure]
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      if (this.m_size == 0)
      {
        if (startIndex != -1)
          throw new ArgumentOutOfRangeException(nameof (startIndex));
      }
      else if ((uint) startIndex >= (uint) this.m_size)
        throw new ArgumentOutOfRangeException(nameof (startIndex));
      if (count < 0 || startIndex - count + 1 < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      int num = startIndex - count;
      for (int lastIndex = startIndex; lastIndex > num; --lastIndex)
      {
        if (match(this.m_items[lastIndex]))
          return lastIndex;
      }
      return -1;
    }

    /// <summary>Calls given action on every element.</summary>
    public void ForEach(Action<T> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      for (int index = 0; index < this.m_size; ++index)
        action(this.m_items[index]);
    }

    /// <summary>
    /// Calls function on every element and ignores its return value.
    /// </summary>
    public void ForEach<TRet>(Func<T, TRet> func)
    {
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      for (int index = 0; index < this.m_size; ++index)
      {
        TRet ret = func(this.m_items[index]);
      }
    }

    public void ForEach<TParam>(TParam param, Action<T, TParam> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      for (int index = 0; index < this.m_size; ++index)
        action(this.m_items[index], param);
    }

    /// <summary>
    /// Calls given action on every element and clears the list.
    /// </summary>
    public void ForEachAndClear(Action<T> action)
    {
      this.ForEach(action);
      this.Clear();
    }

    /// <summary>
    /// Calls given action on every element while ignoring return value from the function. Clears the list after the
    /// iteration.
    /// </summary>
    public void ForEachAndClear<TRet>(Func<T, TRet> func)
    {
      this.ForEach<TRet>(func);
      this.Clear();
    }

    public void ForEachAndClear<TParam>(TParam param, Action<T, TParam> action)
    {
      this.ForEach<TParam>(param, action);
      this.Clear();
    }

    /// <summary>
    /// Returns the index of the first occurrence of a given value in a range of this list. The list is searched
    /// forwards from beginning to end. The elements of the list are compared to the given value using the
    /// Object.Equals method.
    /// 
    /// This method uses the Array.IndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int IndexOf(T item) => Array.IndexOf<T>(this.m_items, item, 0, this.m_size);

    /// <summary>
    /// Returns the index of the first occurrence of a given value in a range of this list. The list is searched
    /// forwards, starting at index and ending at count number of elements. The elements of the list are compared to
    /// the given value using the Object.Equals method.
    /// 
    /// This method uses the Array.IndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int IndexOf(T item, int index)
    {
      if (index > this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      return Array.IndexOf<T>(this.m_items, item, index, this.m_size - index);
    }

    /// <summary>
    /// Returns the index of the first occurrence of a given value in a range of this list. The list is searched
    /// forwards, starting at index and up to count number of elements. The elements of the list are compared to the
    /// given value using the Object.Equals method.
    /// 
    /// This method uses the Array.IndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int IndexOf(T item, int index, int count)
    {
      if (index > this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0 || index > this.m_size - count)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Array.IndexOf<T>(this.m_items, item, index, count);
    }

    /// <summary>
    /// Inserts an element into this list at a given index. The size of the list is increased by one. If required,
    /// the capacity of the list is doubled before inserting the new element.
    /// </summary>
    public void Insert(int index, T item)
    {
      if ((uint) index > (uint) this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (this.m_size >= this.m_items.Length)
        this.extendCapacity(this.m_size + 1);
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + 1, this.m_size - index);
      this.m_items[index] = item;
      ++this.m_size;
      Lyst<T>.incVersion();
    }

    void ILystNonGeneric.Insert(int index, object item) => this.Insert(index, (T) item);

    /// <summary>
    /// Returns the index of the last occurrence of a given value in a range of this list. The list is searched
    /// backwards, starting at the end and ending at the first element in the list. The elements of the list are
    /// compared to the given value using the Object.Equals method.
    /// 
    /// This method uses the Array.LastIndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int LastIndexOf(T item)
    {
      return this.m_size != 0 ? this.LastIndexOf(item, this.m_size - 1, this.m_size) : -1;
    }

    /// <summary>
    /// Returns the index of the last occurrence of a given value in a range of this list. The list is searched
    /// backwards, starting at index and ending at the first element in the list. The elements of the list are
    /// compared to the given value using the Object.Equals method.
    /// 
    /// This method uses the Array.LastIndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int LastIndexOf(T item, int index) => this.LastIndexOf(item, index, index + 1);

    /// <summary>
    /// Returns the index of the last occurrence of a given value in a range of this list. The list is searched
    /// backwards, starting at index and up to count elements. The elements of the list are compared to the given
    /// value using the Object.Equals method.
    /// 
    /// This method uses the Array.LastIndexOf method to perform the search.
    /// </summary>
    [Pure]
    public int LastIndexOf(T item, int index, int count)
    {
      if (this.Count != 0 && index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (this.Count != 0 && count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (this.m_size == 0)
        return -1;
      if (index >= this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count > index + 1)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Array.LastIndexOf<T>(this.m_items, item, index, count);
    }

    /// <summary>
    /// Moves item at given index to the start and shuffles all the items in between keeping their order.
    /// </summary>
    public void MoveToStart(int index)
    {
      T obj = this.m_items[index];
      for (int index1 = index; index1 > 0; --index1)
        this.m_items[index1] = this.m_items[index1 - 1];
      this.m_items[0] = obj;
    }

    /// <summary>
    /// Tries to remove given element from the list and returns true if the element was removed. Otherwise, returns
    /// false and the collection was not changed.
    /// </summary>
    public bool Remove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAt(index);
      return true;
    }

    public void RemoveAndAssert(T item)
    {
      Mafi.Assert.That<bool>(this.Remove(item)).IsTrue(string.Format("Failed to remove from list: {0}", (object) item));
    }

    /// <summary>
    /// This method removes all items which matches the predicate. The complexity is O(n).
    /// </summary>
    public int RemoveAll(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      int index1 = 0;
      while (index1 < this.m_size && !match(this.m_items[index1]))
        ++index1;
      if (index1 >= this.m_size)
        return 0;
      int index2 = index1 + 1;
      while (index2 < this.m_size)
      {
        while (index2 < this.m_size && match(this.m_items[index2]))
          ++index2;
        if (index2 < this.m_size)
          this.m_items[index1++] = this.m_items[index2++];
      }
      Array.Clear((Array) this.m_items, index1, this.m_size - index1);
      int num = this.m_size - index1;
      this.m_size = index1;
      Lyst<T>.incVersion();
      return num;
    }

    /// <summary>
    /// This method removes the first item which matches the predicate. The complexity is O(n).
    /// </summary>
    public bool RemoveFirst(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      for (int destinationIndex = 0; destinationIndex < this.m_size; ++destinationIndex)
      {
        if (match(this.m_items[destinationIndex]))
        {
          --this.m_size;
          if (destinationIndex < this.m_size)
            Array.Copy((Array) this.m_items, destinationIndex + 1, (Array) this.m_items, destinationIndex, this.m_size - destinationIndex);
          this.m_items[this.m_size] = default (T);
          Lyst<T>.incVersion();
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Removes the element at the given index. The size of the list is decreased by one. This is O(n) operation. If
    /// order of the items is not significant consider using more efficient <see cref="M:Mafi.Collections.Lyst`1.RemoveAtReplaceWithLast(System.Int32)" />.
    /// </summary>
    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.m_size;
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index + 1, (Array) this.m_items, index, this.m_size - index);
      this.m_items[this.m_size] = default (T);
      Lyst<T>.incVersion();
    }

    public bool TryRemove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAt(index);
      return true;
    }

    /// <summary>
    /// Removes the element at the given index and replaces it with the last item. The size of the list is decreased
    /// by one. This is O(1) operation.
    /// </summary>
    public void RemoveAtReplaceWithLast(int index)
    {
      if ((uint) index >= (uint) this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.m_size;
      if (index < this.m_size)
        this.m_items[index] = this.m_items[this.m_size];
      this.m_items[this.m_size] = default (T);
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Tries to remove given element. Returns whether the element was removed. If removed, its place is replaced
    /// with the last item. This is more efficient that <see cref="M:Mafi.Collections.Lyst`1.Remove(`0)" />.
    /// </summary>
    public bool TryRemoveReplaceLast(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAtReplaceWithLast(index);
      return true;
    }

    /// <summary>
    /// Returns and removes last item. This is O(1) operation.
    /// </summary>
    public T PopLast()
    {
      if (this.m_size == 0)
        throw new InvalidOperationException("Popping last from an empty Lyst.");
      --this.m_size;
      T obj = this.m_items[this.m_size];
      this.m_items[this.m_size] = default (T);
      Lyst<T>.incVersion();
      return obj;
    }

    /// <summary>Removes a range of elements from this list.</summary>
    public void RemoveRange(int index, int count)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (this.m_size - index < count)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count <= 0)
        return;
      this.m_size -= count;
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index + count, (Array) this.m_items, index, this.m_size - index);
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_items, this.m_size, count);
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Removes all items for which given predicate is true. Order of remaining elements is preserved.
    /// Returns the number of removed elements.
    /// </summary>
    public int RemoveWhere(Predicate<T> pred)
    {
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.m_size; ++index1)
      {
        T obj = this.m_items[index1];
        if (!pred(obj))
        {
          this.m_items[index2] = obj;
          ++index2;
        }
      }
      int length = index1 - index2;
      this.m_size -= length;
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_items, this.m_size, length);
      return length;
    }

    /// <summary>
    /// Similar to <see cref="M:Mafi.Collections.Lyst`1.RemoveWhere(System.Predicate{`0})" /> but also allows passing a value to the predicate
    /// to avoid allocations.
    /// </summary>
    public int RemoveWhere<TExtraValue>(TExtraValue value, Func<T, TExtraValue, bool> pred)
    {
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.m_size; ++index1)
      {
        T obj = this.m_items[index1];
        if (!pred(obj, value))
        {
          this.m_items[index2] = obj;
          ++index2;
        }
      }
      int length = index1 - index2;
      this.m_size -= length;
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_items, this.m_size, length);
      return length;
    }

    public int RemoveWhere(Predicate<T> pred, Lyst<T> removedItems)
    {
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.m_size; ++index1)
      {
        T obj = this.m_items[index1];
        if (pred(obj))
        {
          removedItems.Add(obj);
        }
        else
        {
          this.m_items[index2] = obj;
          ++index2;
        }
      }
      int length = index1 - index2;
      this.m_size -= length;
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_items, this.m_size, length);
      return length;
    }

    public int RemoveDuplicates()
    {
      Set<T> items = new Set<T>(this.Count);
      return this.RemoveWhere((Predicate<T>) (x => !items.Add(x)));
    }

    /// <summary>Reverses all elements in this list.</summary>
    public void Reverse() => this.Reverse(0, this.Count);

    /// <summary>
    /// Reverses the elements in a range of this list. Following a call to this method, an element in the range given
    /// by index and count which was previously located at index i will now be located at index index + (index
    /// + count - i - 1).
    /// </summary>
    public void Reverse(int index, int count)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (this.m_size - index < count)
        throw new ArgumentOutOfRangeException(nameof (count));
      T[] items = this.m_items;
      int index1 = index;
      for (int index2 = index + count - 1; index1 < index2; --index2)
      {
        T obj = items[index1];
        items[index1] = items[index2];
        items[index2] = obj;
        ++index1;
      }
      Lyst<T>.incVersion();
    }

    public Lyst<T> GetRange(int fromIndex, int count)
    {
      Lyst<T> range = new Lyst<T>(count, this.m_canOmitClearing);
      Array.Copy((Array) this.m_items, fromIndex, (Array) range.m_items, 0, count);
      range.m_size = count;
      return range;
    }

    /// <summary>
    /// Sorts the elements in this list. Uses the default comparer and Array.Sort.
    /// </summary>
    public void Sort()
    {
      Array.Sort<T>(this.m_items, 0, this.Count);
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Sorts the elements in this list. Uses Array.Sort with the provided comparer.
    /// </summary>
    public void Sort(IComparer<T> comparer)
    {
      Mafi.Assert.That<IComparer<T>>(comparer).IsNotNull<IComparer<T>>();
      Array.Sort<T>(this.m_items, 0, this.Count, comparer);
      Lyst<T>.incVersion();
    }

    /// <summary>
    /// Sorts the elements in a section of this list. The sort compares the elements to each other using the given
    /// IComparer interface. If comparer is null, the elements are compared to each other using the IComparable
    /// interface, which in that case must be implemented by all elements of the list.
    /// 
    /// This method uses the Array.Sort method to sort the elements.
    /// </summary>
    public void Sort(int index, int count, IComparer<T> comparer)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (this.m_size - index < count)
        throw new ArgumentOutOfRangeException(nameof (count));
      Array.Sort<T>(this.m_items, index, count, comparer);
      Lyst<T>.incVersion();
    }

    public void Sort(Comparison<T> comparison)
    {
      if (comparison == null)
        throw new ArgumentNullException(nameof (comparison));
      if (this.m_size <= 0)
        return;
      Array.Sort<T>(this.m_items, 0, this.m_size, (IComparer<T>) new ComparisonComparer<T>(comparison));
    }

    public void Shuffle(IRandom random) => random.Shuffle<T>(this.m_items, 0, this.m_size);

    /// <summary>
    /// Swaps internal data and size with other Lyst. This is handy when swapping whole references of lysts is not
    /// possible.
    /// </summary>
    public void SwapDataWith(Lyst<T> other)
    {
      Mafi.Assert.That<bool>(this.m_canOmitClearing).IsEqualTo<bool>(other.m_canOmitClearing, "Swapping Lysts with incompatible 'canOmitClearing' flag.");
      Swap.Them<T[]>(ref this.m_items, ref other.m_items);
      Swap.Them<int>(ref this.m_size, ref other.m_size);
    }

    /// <summary>
    /// Returns an array containing the contents of the List. This requires copying the List, which is an O(n)
    /// operation.
    /// </summary>
    [Pure]
    public T[] ToArray(int startIndex = 0)
    {
      int length = this.m_size - startIndex;
      if (this.m_size - startIndex <= 0)
        return Array.Empty<T>();
      T[] destinationArray = new T[length];
      Array.Copy((Array) this.m_items, startIndex, (Array) destinationArray, 0, length);
      return destinationArray;
    }

    [Pure]
    public TResult[] ToArray<TResult>(Func<T, TResult> selector)
    {
      if (this.m_size <= 0)
        return Array.Empty<TResult>();
      TResult[] array = new TResult[this.m_size];
      for (int index = 0; index < this.m_size; ++index)
        array[index] = selector(this.m_items[index]);
      return array;
    }

    /// <summary>
    /// Returns an array containing the contents of the List. This requires copying the List, which is an O(n)
    /// operation. If the Lyst is full ( <see cref="P:Mafi.Collections.Lyst`1.Count" /> == <see cref="P:Mafi.Collections.Lyst`1.Capacity" />) then this operation is O(1)
    /// and does not involve any copying.
    /// </summary>
    public T[] ToArrayAndClear()
    {
      if (this.m_size <= 0)
        return Array.Empty<T>();
      if (this.m_size == this.Capacity)
      {
        T[] items = this.m_items;
        this.m_items = Array.Empty<T>();
        this.m_size = 0;
        Lyst<T>.incVersion();
        return items;
      }
      T[] destinationArray = new T[this.m_size];
      Array.Copy((Array) this.m_items, 0, (Array) destinationArray, 0, this.m_size);
      this.Clear();
      return destinationArray;
    }

    /// <summary>
    /// Maps this list using given function. This operation is not lazy like LINQ methods but is more efficient.
    /// </summary>
    [Pure]
    public Lyst<TResult> ToLyst<TResult>(Func<T, TResult> mapper)
    {
      if (this.m_size <= 0)
        return new Lyst<TResult>();
      Lyst<TResult> lyst = new Lyst<TResult>()
      {
        Count = this.m_size
      };
      for (int index = 0; index < this.m_size; ++index)
        lyst[index] = mapper(this[index]);
      return lyst;
    }

    /// <summary>
    /// Sets the capacity of this list to the size of the list. This method can be used to minimize a list's memory
    /// overhead once it is known that no new elements will be added to the list. To completely clear a list and
    /// release all memory referenced by the list, execute the following statements:
    /// 
    /// list.Clear(); list.TrimExcess();
    /// </summary>
    public void TrimExcess() => this.Capacity = this.m_size;

    /// <summary>
    /// Returns immutable array representation of this lyst by making a copy.
    /// </summary>
    [Pure]
    public ImmutableArray<T> ToImmutableArray()
    {
      if (this.m_size <= 0)
        return ImmutableArray<T>.Empty;
      T[] objArray = new T[this.m_size];
      Array.Copy((Array) this.m_items, 0, (Array) objArray, 0, this.m_size);
      return new ImmutableArray<T>(objArray);
    }

    [Pure]
    public ImmutableArray<TResult> ToImmutableArray<TResult>(Func<T, TResult> selector)
    {
      if (this.m_size <= 0)
        return ImmutableArray<TResult>.Empty;
      ImmutableArrayBuilder<TResult> immutableArrayBuilder = new ImmutableArrayBuilder<TResult>(this.m_size);
      for (int i = 0; i < this.m_size; ++i)
        immutableArrayBuilder[i] = selector(this.m_items[i]);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    [Pure]
    public ImmutableArray<TResult> ToImmutableArrayAndClear<TResult>(Func<T, TResult> selector)
    {
      ImmutableArray<TResult> immutableArray = this.ToImmutableArray<TResult>(selector);
      this.Clear();
      return immutableArray;
    }

    /// <summary>
    /// Returns immutable array representation and clears this Lyst. If the Lyst is full ( <see cref="P:Mafi.Collections.Lyst`1.Count" /> ==
    /// <see cref="P:Mafi.Collections.Lyst`1.Capacity" />) then this operation is O(1) and does not involve any copying.
    /// </summary>
    public ImmutableArray<T> ToImmutableArrayAndClear()
    {
      if (this.m_size <= 0)
        return ImmutableArray<T>.Empty;
      T[] objArray = new T[this.m_size];
      Array.Copy((Array) this.m_items, 0, (Array) objArray, 0, this.m_size);
      this.Clear();
      return new ImmutableArray<T>(objArray);
    }

    public SmallImmutableArray<T> ToSmallImmutableArrayAndClear()
    {
      if (this.m_size != 1)
        return new SmallImmutableArray<T>(this.ToImmutableArrayAndClear());
      SmallImmutableArray<T> immutableArrayAndClear = new SmallImmutableArray<T>(this.First);
      this.Clear();
      return immutableArrayAndClear;
    }

    [Pure]
    public PooledArray<T> ToPooledArrayAndClear()
    {
      if (this.m_size <= 0)
        return PooledArray<T>.Empty;
      PooledArray<T> pooled = PooledArray<T>.GetPooled(this.m_size);
      pooled.CopyFrom(this.m_items, this.m_size);
      this.Clear();
      return pooled;
    }

    [Pure]
    public ImmutableArray<TResult> TakeImmutableArray<TResult>(int count, Func<T, TResult> selector)
    {
      if (this.m_size < count)
        throw new ArgumentException("Not enough elements to take.");
      if (count == 0)
        return ImmutableArray<TResult>.Empty;
      ImmutableArrayBuilder<TResult> immutableArrayBuilder = new ImmutableArrayBuilder<TResult>(count);
      for (int i = 0; i < count; ++i)
        immutableArrayBuilder[i] = selector(this.m_items[i]);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public TResult[] Select<TResult>(Func<T, TResult> selector)
    {
      TResult[] resultArray = new TResult[this.m_size];
      for (int index = 0; index < this.m_size; ++index)
        resultArray[index] = selector(this.m_items[index]);
      return resultArray;
    }

    protected void SerializeData(BlobWriter writer)
    {
      writer.WriteIntNotNegative(this.m_size);
      writer.WriteBool(this.m_canOmitClearing);
      writer.WriteArray<T>(this.m_items, this.m_size);
    }

    protected void DeserializeData(BlobReader reader)
    {
      this.m_size = reader.ReadIntNotNegative();
      ReflectionUtils.SetField<Lyst<T>>(this, "m_canOmitClearing", (object) reader.ReadBool());
      this.m_items = reader.ReadArray<T>(this.m_size);
      this.validateAfterLoad();
    }

    public override string ToString()
    {
      return string.Format("Count: {0}, capacity: {1}", (object) this.Count, (object) this.Capacity);
    }

    [DebuggerStepThrough]
    [Pure]
    public Lyst<T>.Enumerator GetEnumerator() => new Lyst<T>.Enumerator(this);

    [Pure]
    [DebuggerStepThrough]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return (IEnumerator<T>) new Lyst<T>.EnumeratorClass(this);
    }

    [DebuggerStepThrough]
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Lyst<T>.EnumeratorClass(this);

    [DebuggerStepThrough]
    [Pure]
    IndexableEnumerator<T> IIndexable<T>.GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }

    [Pure]
    [DebuggerStepThrough]
    IEnumerable<T> IIndexable<T>.AsEnumerable() => (IEnumerable<T>) this;

    public static void Serialize(Lyst<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Lyst<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Lyst<T>.s_serializeDataDelayedAction);
    }

    public static Lyst<T> Deserialize(BlobReader reader)
    {
      Lyst<T> lyst;
      if (reader.TryStartClassDeserialization<Lyst<T>>(out lyst))
        reader.EnqueueDataDeserialization((object) lyst, Lyst<T>.s_deserializeDataDelayedAction);
      return lyst;
    }

    static Lyst()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Lyst<T>.s_pcKey = "Lyst<" + typeof (T).Name + ">: ";
      Lyst<T>.s_pcKey_OmittedClearing = Lyst<T>.s_pcKey + "OmittedClearing";
      Lyst<T>.s_pcKey_SavedByTrimExcess = Lyst<T>.s_pcKey + "SavedByTrimExcess";
      Lyst<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Lyst<T>) obj).SerializeData(writer));
      Lyst<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Lyst<T>) obj).DeserializeData(reader));
    }

    /// <summary>
    /// Struct enumerator that is optimized for fast and allocation-free iteration through the Lyst.
    /// </summary>
    /// <remarks>
    /// Due to optimizations this iterator does not behave the same as <see cref="T:System.Collections.Generic.List`1.Enumerator" />, namely:
    /// 1. Throws an exception when <see cref="P:Mafi.Collections.Lyst`1.Enumerator.Current" /> is called before <see cref="M:Mafi.Collections.Lyst`1.Enumerator.MoveNext" /> or when <see cref="M:Mafi.Collections.Lyst`1.Enumerator.MoveNext" /> returned false. This is not limiting any valid usage since iterator should not be even
    /// called in those cases.
    /// 2. Value of <see cref="P:Mafi.Collections.Lyst`1.Enumerator.Current" /> is not cached but it is directly returned using index. Typical
    /// implementation will call the <see cref="P:Mafi.Collections.Lyst`1.Enumerator.Current" /> only once after <see cref="M:Mafi.Collections.Lyst`1.Enumerator.MoveNext" /> returned true so
    /// caching is waste of resources and memory. This makes the iterator struct significantly smaller.
    /// 3. Does not implement <see cref="T:System.Collections.IEnumerator" /> so it is never boxed and it does NOT implement <see cref="T:System.IDisposable" />. We want the iterator to inline when we do foreach and to not result in a try/finally
    /// frame in the client.
    /// </remarks>
    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly Lyst<T> m_lyst;
      private int m_index;

      internal Enumerator(Lyst<T> lyst)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_lyst = lyst;
        this.m_index = -1;
      }

      public bool MoveNext() => ++this.m_index < this.m_lyst.m_size;

      public T Current => this.m_lyst[this.m_index];
    }

    /// <summary>
    /// Separate enumerator class that implements <see cref="T:System.Collections.Generic.IEnumerator`1" />.
    /// </summary>
    /// <remarks>
    /// By implementing this enumerator as a class we avoid boxing. Has the same properties as <see cref="T:Mafi.Collections.Lyst`1.Enumerator" />.
    /// </remarks>
    [DebuggerStepThrough]
    private class EnumeratorClass : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly Lyst<T> m_lyst;
      private int m_index;

      internal EnumeratorClass(Lyst<T> lyst)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_index = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_lyst = lyst;
      }

      public bool MoveNext() => ++this.m_index < this.m_lyst.m_size;

      public T Current => this.m_lyst[this.m_index];

      object IEnumerator.Current => (object) this.m_lyst[this.m_index];

      public void Reset() => this.m_index = -1;

      public void Dispose()
      {
      }
    }
  }
}
