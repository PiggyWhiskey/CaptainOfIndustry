// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.LystStruct`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// A struct version of <see cref="T:Mafi.Collections.Lyst`1" />. It should be used for private lists that do not need to be passed
  /// around as references.
  /// 
  /// WARNING: This is mutable struct, DO NOT DEFINE THIS AS A READONLY MEMBER and be careful to mutate the right copy.
  /// </summary>
  /// <remarks>
  /// It has following advantages:
  /// * Saves one object and a pointer-hop on every access.
  /// * Is equivalent of an array (8 B) and a count (4 B) in memory (it needs 8 B alignment though).
  /// * Does not need to be initialized. Default value is an equivalent of an empty list.
  /// 
  /// However, there are also some disadvantages:
  /// * Cannot be passed via interfaces like <see cref="T:Mafi.Collections.ReadonlyCollections.IIndexable`1" /> to avoid boxing.
  /// * Mutable struct has always dangers of mutating a copy instead of the stored value.
  /// * No version checking during enumeration.
  /// </remarks>
  [ManuallyWrittenSerialization]
  public struct LystStruct<T>
  {
    /// <summary>
    /// Default capacity when the first item is added to an empty list.
    /// </summary>
    public const int DEFAULT_CAPACITY = 4;
    public static readonly LystStruct<T> Empty;
    /// <summary>Storage for the items.</summary>
    private T[] m_items;

    /// <summary>Number of currently stored items.</summary>
    public int Count { get; private set; }

    public readonly int Capacity
    {
      get
      {
        T[] items = this.m_items;
        return items == null ? 0 : items.Length;
      }
    }

    public readonly bool IsEmpty => this.Count <= 0;

    public readonly bool IsNotEmpty => this.Count > 0;

    public LystStruct(int capacity)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_items = new T[capacity];
      this.Count = 0;
    }

    private void validateAfterLoad()
    {
      if (this.Count < 0)
      {
        Mafi.Log.Error(string.Format("Invalid LystStruct after load: Negative count `{0}`.", (object) this.Count));
        this.Count = 0;
      }
      else
      {
        if (this.Count <= 0)
          return;
        if (this.m_items == null)
        {
          Mafi.Log.Error(string.Format("Invalid LystStruct after load: Null backing array with non-zero count `{0}`.", (object) this.Count));
          this.Count = 0;
        }
        else
        {
          if (this.Count <= this.m_items.Length)
            return;
          Mafi.Log.Error(string.Format("Invalid LystStruct after load: Count `{0}` greater than ", (object) this.Count) + string.Format("array size `{0}`.", (object) this.m_items.Length));
          this.Count = this.m_items.Length;
        }
      }
    }

    /// <summary>
    /// Indexer with unchecked array access (for performance).
    /// </summary>
    public T this[int index]
    {
      readonly get => this.m_items[index];
      set => this.m_items[index] = value;
    }

    /// <summary>Gets or sets the first element at index 0.</summary>
    public T First
    {
      readonly get
      {
        return this.m_items != null && this.Count > 0 ? this.m_items[0] : throw new InvalidOperationException("Trying to get the first element from empty LystStruct");
      }
      set => this.m_items[0] = value;
    }

    /// <summary>
    /// Gets or sets the last element at index <c>Count - 1</c>.
    /// If the list is empty the <see cref="T:System.IndexOutOfRangeException" /> will be thrown.
    /// </summary>
    public T Last
    {
      readonly get
      {
        return this.m_items != null && this.Count > 0 ? this.m_items[this.Count - 1] : throw new InvalidOperationException("Trying to get the first element from empty LystStruct");
      }
      set => this.m_items[this.Count - 1] = value;
    }

    public readonly ref T GetRefAt(int index)
    {
      if ((uint) index >= (uint) this.Count)
        throw new IndexOutOfRangeException(nameof (index));
      return ref this.m_items[index];
    }

    /// <summary>
    /// Changes size (count) of the list, either removing elements or adding default values as needed.
    /// </summary>
    public void Resize(int newSize)
    {
      if (newSize > this.Count)
        this.EnsureCapacity(newSize);
      else if (newSize < this.Count)
      {
        if (newSize < 0)
        {
          Mafi.Log.Warning("Trying to resize LystStruct to negative size.");
          newSize = 0;
        }
        Array.Clear((Array) this.m_items, newSize, this.Count - newSize);
      }
      this.Count = newSize;
    }

    public void EnsureCapacity(int minCapacity)
    {
      if (this.m_items == null)
      {
        if ((uint) minCapacity > 2146435071U)
        {
          Mafi.Log.Error("Allocating max length LystStruct with ~2G elements. Is this an error?");
          minCapacity = 2146435071;
        }
        this.m_items = new T[minCapacity];
      }
      else
      {
        if (minCapacity <= this.m_items.Length)
          return;
        this.extendCapacity(minCapacity);
      }
    }

    private void extendCapacity(int minCapacity)
    {
      int newSize = this.m_items.Length == 0 ? 4 : this.m_items.Length * 2;
      if ((uint) newSize > 2146435071U)
      {
        Mafi.Log.Error("Allocating max length LystStruct with ~2G elements. Is this an error?");
        newSize = 2146435071;
      }
      if (newSize < minCapacity)
        newSize = minCapacity;
      Array.Resize<T>(ref this.m_items, newSize);
    }

    public void Add(T item)
    {
      if (this.m_items == null)
        this.m_items = new T[4];
      else if (this.Count >= this.m_items.Length)
        this.extendCapacity(this.Count + 1);
      this.m_items[this.Count] = item;
      ++this.Count;
    }

    public void AddAndAssertNew(T item) => this.Add(item);

    public void AddRange(LystStruct<T> list)
    {
      int count = list.Count;
      if (count == 0)
        return;
      this.EnsureCapacity(this.Count + count);
      Array.Copy((Array) list.GetBackingArray(), 0, (Array) this.m_items, this.Count, count);
      this.Count += count;
    }

    public void AddRange(Lyst<T> list)
    {
      int count = list.Count;
      if (count == 0)
        return;
      this.EnsureCapacity(this.Count + count);
      Array.Copy((Array) list.GetBackingArray(), 0, (Array) this.m_items, this.Count, count);
      this.Count += count;
    }

    public void AddRange(IEnumerable<T> list)
    {
      foreach (T obj in list)
        this.Add(obj);
    }

    public void Insert(int index, T item)
    {
      if ((uint) index > (uint) this.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (this.m_items == null)
        this.m_items = new T[4];
      else if (this.Count >= this.m_items.Length)
        this.extendCapacity(this.Count + 1);
      if (index < this.Count)
        Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + 1, this.Count - index);
      this.m_items[index] = item;
      ++this.Count;
    }

    public T PopLast()
    {
      if (this.m_items == null || this.Count <= 0)
        throw new InvalidOperationException("Popping last from an empty LystStruct.");
      --this.Count;
      T obj = this.m_items[this.Count];
      this.m_items[this.Count] = default (T);
      return obj;
    }

    public void PopLast(int count)
    {
      if (this.m_items == null || this.Count <= 0)
        throw new InvalidOperationException("Popping last from an empty LystStruct.");
      if (count > this.Count)
      {
        Mafi.Log.Error(string.Format("Trying to pop {0} out of {1}", (object) count, (object) this.Count));
        count = this.Count;
      }
      this.Count -= count;
      Array.Clear((Array) this.m_items, this.Count, count);
    }

    public readonly void Sort()
    {
      if (this.m_items == null || this.Count <= 0)
        return;
      Array.Sort<T>(this.m_items, 0, this.Count);
    }

    public readonly void Sort(IComparer<T> comparer)
    {
      if (this.m_items == null || this.Count <= 0)
        return;
      Mafi.Assert.That<IComparer<T>>(comparer).IsNotNull<IComparer<T>>();
      Array.Sort<T>(this.m_items, 0, this.Count, comparer);
    }

    public readonly void Sort(Comparison<T> comparison)
    {
      if (this.m_items == null || this.Count <= 0)
        return;
      Array.Sort<T>(this.m_items, 0, this.Count, (IComparer<T>) new ComparisonComparer<T>(comparison));
    }

    public void Clear()
    {
      if (this.Count == 0)
        return;
      Array.Clear((Array) this.m_items, 0, this.Count);
      this.Count = 0;
    }

    /// <summary>
    /// Sets all elements to their default value but keeps them in the list.
    /// </summary>
    public readonly void ClearElementsKeepCount()
    {
      Array.Clear((Array) this.m_items, 0, this.Count);
    }

    /// <summary>
    /// Much faster than <see cref="M:Mafi.Collections.LystStruct`1.Clear" /> but cleared elements are not zeroed-out. Use this only when <c>T</c>
    /// has no references (even nested).
    /// </summary>
    public void ClearSkipZeroingMemory() => this.Count = 0;

    public void ClearFromIndexSkipZeroingMemory(int index)
    {
      if (this.Count <= index)
        return;
      this.Count = index;
    }

    /// <summary>
    /// Retunrs backing array. Note that this array may get reallocated when the list gets mutated.
    /// </summary>
    public readonly T[] GetBackingArray() => this.m_items ?? Array.Empty<T>();

    /// <summary>
    /// Returns a slice of backing array with valid data only.
    /// </summary>
    public readonly ReadOnlyArraySlice<T> BackingArrayAsSlice
    {
      get => new ReadOnlyArraySlice<T>(this.m_items ?? Array.Empty<T>(), 0, this.Count);
    }

    public readonly bool Contains(T item)
    {
      if ((object) item == null)
      {
        for (int index = 0; index < this.Count; ++index)
        {
          if ((object) this.m_items[index] == null)
            return true;
        }
      }
      else
      {
        EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        for (int index = 0; index < this.Count; ++index)
        {
          if (equalityComparer.Equals(this.m_items[index], item))
            return true;
        }
      }
      return false;
    }

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

    /// <summary>
    /// Returns the index of the first occurrence of a given value in a range of this list. The list is searched
    /// forwards from beginning to end. The elements of the list are compared to the given value using the
    /// Object.Equals method.
    /// 
    /// This method uses the Array.IndexOf method to perform the search.
    /// </summary>
    [Pure]
    public readonly int IndexOf(T item)
    {
      return this.m_items != null ? Array.IndexOf<T>(this.m_items, item, 0, this.Count) : -1;
    }

    [Pure]
    public readonly int IndexOf<TValue>(TValue item, Func<T, TValue> selector)
    {
      if (this.m_items == null)
        return -1;
      EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
      for (int index = 0; index < this.Count; ++index)
      {
        if (equalityComparer.Equals(item, selector(this.m_items[index])))
          return index;
      }
      return -1;
    }

    [Pure]
    public readonly T FirstOrDefault(Func<T, bool> predicate = null)
    {
      if (this.m_items == null)
        return default (T);
      if (predicate == null)
        return this.Count < 1 ? default (T) : this.m_items[0];
      for (int index = 0; index < this.Count; ++index)
      {
        T obj = this.m_items[index];
        if (predicate(obj))
          return obj;
      }
      return default (T);
    }

    /// <summary>
    /// Removes the element at the given index. The size of the list is decreased by one. This is O(n) operation.
    /// </summary>
    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.Count || this.m_items == null)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.Count;
      if (index < this.Count)
        Array.Copy((Array) this.m_items, index + 1, (Array) this.m_items, index, this.Count - index);
      this.m_items[this.Count] = default (T);
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

    public int RemoveWhere(Predicate<T> pred)
    {
      if (this.m_items == null)
        return 0;
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.Count; ++index1)
      {
        T obj = this.m_items[index1];
        if (!pred(obj))
        {
          this.m_items[index2] = obj;
          ++index2;
        }
      }
      int length = index1 - index2;
      this.Count -= length;
      Array.Clear((Array) this.m_items, this.Count, length);
      return length;
    }

    public int RemoveWhere<TArg>(TArg arg, Func<T, TArg, bool> pred)
    {
      if (this.m_items == null)
        return 0;
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.Count; ++index1)
      {
        T obj = this.m_items[index1];
        if (!pred(obj, arg))
        {
          this.m_items[index2] = obj;
          ++index2;
        }
      }
      int length = index1 - index2;
      this.Count -= length;
      Array.Clear((Array) this.m_items, this.Count, length);
      return length;
    }

    /// <summary>
    /// Removes the element at the given index and replaces it with the last item. The size of the list is decreased
    /// by one. This is O(1) operation.
    /// </summary>
    public void RemoveAtReplaceWithLast(int index)
    {
      if ((uint) index >= (uint) this.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.Count;
      if (index < this.Count)
        this.m_items[index] = this.m_items[this.Count];
      this.m_items[this.Count] = default (T);
    }

    /// <summary>
    /// Tries to remove given element. Returns whether the element was removed. If removed, its place is replaced
    /// with the last item. This is more efficient that <see cref="M:Mafi.Collections.LystStruct`1.Remove(`0)" />.
    /// </summary>
    public bool TryRemoveReplaceLast(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAtReplaceWithLast(index);
      return true;
    }

    public void RemoveFirstN(int toRemove)
    {
      if (toRemove <= 0)
        return;
      if (toRemove >= this.Count)
      {
        this.Clear();
      }
      else
      {
        this.Count -= toRemove;
        Array.Copy((Array) this.m_items, toRemove, (Array) this.m_items, 0, this.Count);
      }
    }

    public readonly void Reverse()
    {
      if (this.m_items == null)
        return;
      T[] items = this.m_items;
      int index1 = 0;
      for (int index2 = this.Count - 1; index1 < index2; --index2)
      {
        T obj = items[index1];
        items[index1] = items[index2];
        items[index2] = obj;
        ++index1;
      }
    }

    public readonly Fix32 Sum(Func<T, Fix32> selector)
    {
      Fix32 fix32 = new Fix32();
      for (int index = 0; index < this.Count; ++index)
        fix32 += selector(this.m_items[index]);
      return fix32;
    }

    public readonly bool Any(Predicate<T> predicate)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (predicate(this.m_items[index]))
          return true;
      }
      return false;
    }

    public readonly IEnumerable<T> AsEnumerable() => this.BackingArrayAsSlice.AsEnumerable();

    /// <summary>
    /// Useful when this array is used rarely and keeping it in memory is more expensive than allocating it again.
    /// </summary>
    public void FreeUpBackingArrayIfEmpty()
    {
      if (this.Count != 0)
        return;
      this.m_items = (T[]) null;
    }

    [Pure]
    public readonly Lyst<T> ToLyst()
    {
      return this.m_items == null || this.Count <= 0 ? new Lyst<T>() : new Lyst<T>((IEnumerable<T>) this.m_items);
    }

    [Pure]
    public readonly T[] ToArray()
    {
      if (this.m_items == null || this.Count <= 0)
        return Array.Empty<T>();
      T[] destinationArray = new T[this.Count];
      Array.Copy((Array) this.m_items, 0, (Array) destinationArray, 0, this.Count);
      return destinationArray;
    }

    [Pure]
    public readonly TResult[] ToArray<TResult>(Func<T, TResult> selector)
    {
      if (this.m_items == null || this.Count <= 0)
        return Array.Empty<TResult>();
      int count = this.Count;
      TResult[] array = new TResult[count];
      for (int index = 0; index < count; ++index)
        array[index] = selector(this.m_items[index]);
      return array;
    }

    /// <summary>
    /// Returns an array containing the contents of the List. This requires copying the List, which is an O(n)
    /// operation.
    /// </summary>
    [Pure]
    public readonly T[] ToArray(int startIndex)
    {
      int length = this.Count - startIndex;
      if (length <= 0)
      {
        Mafi.Assert.That<int>(length).IsNotNegative<int>("Invalid start index: {0}", startIndex);
        return Array.Empty<T>();
      }
      T[] destinationArray = new T[length];
      Array.Copy((Array) this.m_items, startIndex, (Array) destinationArray, 0, length);
      return destinationArray;
    }

    [Pure]
    public readonly ImmutableArray<T> ToImmutableArray()
    {
      if (this.m_items == null || this.Count <= 0)
        return ImmutableArray<T>.Empty;
      ImmutableArrayBuilder<T> immutableArrayBuilder = new ImmutableArrayBuilder<T>(this.Count);
      for (int i = 0; i < this.Count; ++i)
        immutableArrayBuilder[i] = this.m_items[i];
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public readonly bool Validate()
    {
      if (this.Count < 0)
      {
        Mafi.Log.Error(string.Format("Negative count `{0}`.", (object) this.Count));
        return false;
      }
      if (this.Count > 0)
      {
        if (this.m_items == null)
        {
          Mafi.Log.Error(string.Format("Null backing array with non-zero count `{0}`.", (object) this.Count));
          return false;
        }
        if (this.Count > this.m_items.Length)
        {
          Mafi.Log.Error(string.Format("Count `{0}` greater than array size `{1}`.", (object) this.Count, (object) this.m_items.Length));
          return false;
        }
      }
      return true;
    }

    public static void Serialize(LystStruct<T> value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Count);
      if (value.Count <= 0)
        return;
      writer.WriteArray<T>(value.m_items, value.Count);
    }

    public static LystStruct<T> Deserialize(BlobReader reader)
    {
      int length = reader.ReadIntNotNegative();
      if (length <= 0)
        return LystStruct<T>.Empty;
      LystStruct<T> lystStruct = new LystStruct<T>();
      lystStruct.Count = length;
      lystStruct.m_items = reader.ReadArray<T>(length);
      lystStruct.validateAfterLoad();
      return lystStruct;
    }

    public override string ToString()
    {
      return string.Format("Count: {0}, capacity: {1}{2}", (object) this.Count, (object) this.Capacity, this.m_items == null ? (object) ", not allocated" : (object) "");
    }

    [DebuggerStepThrough]
    public readonly LystStruct<T>.Enumerator GetEnumerator() => new LystStruct<T>.Enumerator(this);

    static LystStruct() => MBiHIp97M4MqqbtZOh.RFLpSOptx();

    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly LystStruct<T> m_lyst;
      private int m_index;

      internal Enumerator(LystStruct<T> lyst)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_lyst = lyst;
        this.m_index = -1;
      }

      public bool MoveNext() => ++this.m_index < this.m_lyst.Count;

      public readonly T Current => this.m_lyst[this.m_index];
    }
  }
}
