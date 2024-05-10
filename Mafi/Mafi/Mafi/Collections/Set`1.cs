// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Set`1
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
using System.Security;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Hash-based set of unique elements.
  /// 
  /// Mafi changes and additions:
  /// * Implements IReadOnlySet.
  /// * Added methods: AddAndAssertNew, AddRange, ForEach, ForEachAndClear
  /// * Asserts instead of some Exceptions and Debug/Contract calls.
  /// * Removed support for a custom comparer since we have no good way to save it for now.
  /// 
  /// Not: Based on MIT-licensed implementation from .NET core:
  /// https://github.com/microsoft/referencesource/blob/master/System.Core/System/Collections/Generic/HashSet.cs
  /// 
  /// Implementation notes: This uses an array-based implementation similar to Dictionary{T}, using a buckets array to
  /// map hash values to the Slots array. Items in the Slots array that hash to the same value are chained together
  /// through the "next" indices.
  /// 
  /// The capacity is always prime; so during resizing, the capacity is chosen as the next prime greater than double
  /// the last capacity.
  /// 
  /// The underlying data structures are lazily initialized. Because of the observation that, in practice, hashtables
  /// tend to contain only a few elements, the initial capacity is set very small (3 elements) unless the ctor with a
  /// collection is used.
  /// 
  /// The +/- 1 modifications in methods that add, check for containment, etc allow us to distinguish a hash code of 0
  /// from an uninitialized bucket. This saves us from having to reset each bucket to -1 when resizing. See Contains,
  /// for example.
  /// 
  /// Set methods such as UnionWith, IntersectWith, ExceptWith, and SymmetricExceptWith modify this set.
  /// 
  /// Some operations can perform faster if we can assume "other" contains unique elements according to this equality
  /// comparer. The only times this is efficient to check is if other is a hashset. Note that checking that it's a
  /// hashset alone doesn't suffice; we also have to check that the hashset is using the same equality comparer. If
  /// other has a different equality comparer, it will have unique elements according to its own equality comparer, but
  /// not necessarily according to ours. Therefore, to go these optimized routes we check that other is a hashset using
  /// the same equality comparer.
  /// 
  /// A HashSet with no elements has the properties of the empty set. (See IsSubset, etc. for special empty set
  /// checks.)
  /// 
  /// A couple of methods have a special case if other is this (e.g. SymmetricExceptWith). If we didn't have these
  /// checks, we could be iterating over the set and modifying at the same time.
  /// </summary>
  [GenerateSerializer(true, null, 0)]
  public sealed class Set<T> : 
    ISet<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    IReadOnlySet<T>,
    IReadOnlyCollection<T>,
    ICollectionWithCount
  {
    private static readonly IReadOnlySet<T> s_emptySet;
    private int[] m_buckets;
    private Set<T>.Slot[] m_slots;
    private int m_count;
    private int m_version;
    private int m_lastIndex;
    private int m_freeList;
    private readonly IEqualityComparer<T> m_comparer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static IReadOnlySet<T> Empty
    {
      get
      {
        Assert.That<int>(Set<T>.s_emptySet.Count).IsEqualTo(0);
        return Set<T>.s_emptySet;
      }
    }

    /// <summary>Number of elements in this hashset</summary>
    public int Count => this.m_count;

    public bool IsEmpty => this.m_count <= 0;

    public bool IsNotEmpty => this.m_count > 0;

    public int Capacity
    {
      get
      {
        int[] buckets = this.m_buckets;
        return buckets == null ? 0 : buckets.Length;
      }
    }

    public Set(int capacity = 0, IEqualityComparer<T> comparer = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(capacity).IsNotNegative<int>("Invalid set capacity {0}", capacity);
      this.m_comparer = comparer ?? (IEqualityComparer<T>) EqualityComparer<T>.Default;
      if (capacity <= 0)
        return;
      this.initialize(capacity);
    }

    /// <summary>
    /// Implementation Notes: Since resizes are relatively expensive (require rehashing), this attempts to minimize
    /// the need to resize by setting the initial capacity based on size of collection.
    /// </summary>
    public Set(IEnumerable<T> collection)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector();
      int capacity;
      switch (collection)
      {
        case Set<T> set when Set<T>.areEqualityComparersEqual(this, set):
          this.copyFrom(set);
          return;
        case ICollection<T> objs:
          capacity = objs.Count;
          break;
        default:
          capacity = 0;
          break;
      }
      this.initialize(capacity);
      foreach (T obj in collection)
        this.addIfNotPresent(obj);
      if (this.m_count <= 0 || this.m_slots.Length / this.m_count <= 3)
        return;
      this.TrimExcess();
    }

    public Set(Lyst<T> collection)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector();
      this.initialize(collection.Count);
      foreach (T obj in collection)
        this.addIfNotPresent(obj);
      if (this.m_count <= 0 || this.m_slots.Length / this.m_count <= 3)
        return;
      this.TrimExcess();
    }

    public Set(ImmutableArray<T> collection)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector();
      this.initialize(collection.Length);
      foreach (T obj in collection)
        this.addIfNotPresent(obj);
      if (this.m_count <= 0 || this.m_slots.Length / this.m_count <= 3)
        return;
      this.TrimExcess();
    }

    private void copyFrom(Set<T> source)
    {
      int count = source.m_count;
      if (count == 0)
        return;
      int length = source.m_buckets.Length;
      if (HashHelpers.ExpandPrime(count + 1) >= length)
      {
        this.m_buckets = (int[]) source.m_buckets.Clone();
        this.m_slots = (Set<T>.Slot[]) source.m_slots.Clone();
        this.m_lastIndex = source.m_lastIndex;
        this.m_freeList = source.m_freeList;
      }
      else
      {
        int lastIndex = source.m_lastIndex;
        Set<T>.Slot[] slots = source.m_slots;
        this.initialize(count);
        int index1 = 0;
        for (int index2 = 0; index2 < lastIndex; ++index2)
        {
          int hashCode = slots[index2].hashCode;
          if (hashCode >= 0)
          {
            this.addValue(index1, hashCode, slots[index2].value);
            ++index1;
          }
        }
        Assert.That<int>(index1).IsEqualTo(count);
        this.m_lastIndex = index1;
      }
      this.m_count = count;
    }

    /// <summary>
    /// Add item to this hashset. This is the explicit implementation of the ICollection{T} interface. The other Add
    /// method returns bool indicating whether item was added.
    /// </summary>
    void ICollection<T>.Add(T item) => this.addIfNotPresent(item);

    /// <summary>
    /// Remove all items from this set. This clears the elements but not the underlying buckets and slots array.
    /// Follow this call by TrimExcess to release these.
    /// </summary>
    public void Clear()
    {
      if (this.m_lastIndex > 0)
      {
        Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>("m_buckets was null but m_lastIndex > 0");
        Array.Clear((Array) this.m_slots, 0, this.m_lastIndex);
        Array.Clear((Array) this.m_buckets, 0, this.m_buckets.Length);
        this.m_lastIndex = 0;
        this.m_count = 0;
        this.m_freeList = -1;
      }
      ++this.m_version;
    }

    public Set<T> ClearAndReturn()
    {
      this.Clear();
      return this;
    }

    /// <summary>Returns whether this hashset contains the given item.</summary>
    public bool Contains(T item)
    {
      if (this.m_buckets == null)
      {
        Assert.That<Set<T>.Slot[]>(this.m_slots).IsNull<Set<T>.Slot[]>("Accessing loaded set that was not initialized yet.");
        return false;
      }
      int hashCode = this.internalGetHashCode(item);
      for (int index = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index >= 0; index = this.m_slots[index].next)
      {
        if (this.m_slots[index].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index].value, item))
          return true;
      }
      return false;
    }

    public bool ContainsAny(Lyst<T> items)
    {
      int index = 0;
      for (int count = items.Count; index < count; ++index)
      {
        if (this.Contains(items[index]))
          return true;
      }
      return false;
    }

    public bool ContainsAny(IIndexable<T> items)
    {
      int index = 0;
      for (int count = items.Count; index < count; ++index)
      {
        if (this.Contains(items[index]))
          return true;
      }
      return false;
    }

    public bool ContainsAny(IEnumerable<T> items)
    {
      foreach (T obj in items)
      {
        if (this.Contains(obj))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Copy items in this hashset to array, starting at arrayIndex
    /// </summary>
    /// <param name="array">array to add items to</param>
    /// <param name="arrayIndex">index to start at</param>
    public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this.m_count);

    /// <summary>Remove item from this hashset</summary>
    /// <param name="item">item to remove</param>
    /// <returns>true if removed; false if not (i.e. if the item wasn't in the HashSet)</returns>
    public bool Remove(T item)
    {
      if (this.m_buckets == null)
      {
        Assert.That<Set<T>.Slot[]>(this.m_slots).IsNull<Set<T>.Slot[]>("Removing from a loaded set that was not initialized yet.");
        return false;
      }
      int hashCode = this.internalGetHashCode(item);
      int index1 = hashCode % this.m_buckets.Length;
      int index2 = -1;
      for (int index3 = this.m_buckets[index1] - 1; index3 >= 0; index3 = this.m_slots[index3].next)
      {
        if (this.m_slots[index3].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index3].value, item))
        {
          if (index2 < 0)
            this.m_buckets[index1] = this.m_slots[index3].next + 1;
          else
            this.m_slots[index2].next = this.m_slots[index3].next;
          this.m_slots[index3].hashCode = -1;
          this.m_slots[index3].value = default (T);
          this.m_slots[index3].next = this.m_freeList;
          --this.m_count;
          ++this.m_version;
          if (this.m_count == 0)
          {
            this.m_lastIndex = 0;
            this.m_freeList = -1;
          }
          else
            this.m_freeList = index3;
          return true;
        }
        index2 = index3;
      }
      return false;
    }

    public void RemoveAndAssert(T item)
    {
      this.Remove(item).AssertTrue("Failed to remove item from Set.");
    }

    public void RemoveRange(IEnumerable<T> values)
    {
      foreach (T obj in values)
        this.Remove(obj);
    }

    public void RemoveRangeAndAssert(IEnumerable<T> values)
    {
      foreach (T obj in values)
        this.Remove(obj).AssertTrue("Failed to remove item from Set.");
    }

    public void RemoveRangeAndAssert(Lyst<T> values)
    {
      foreach (T obj in values)
        this.Remove(obj).AssertTrue("Failed to remove item from Set.");
    }

    /// <summary>Whether this is readonly</summary>
    bool ICollection<T>.IsReadOnly => false;

    public Set<T>.Enumerator GetEnumerator() => new Set<T>.Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) new Set<T>.Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Set<T>.Enumerator(this);

    /// <summary>
    /// Add item to this HashSet. Returns bool indicating whether item was added (won't be added if already present)
    /// </summary>
    /// <returns>true if added, false if already present</returns>
    public bool Add(T item) => this.addIfNotPresent(item);

    public void AddAndAssertNew(T item)
    {
      Assert.That<bool>(this.addIfNotPresent(item)).IsTrue<T>("Failed to add '{0}' to a set, already present.", item);
    }

    public void AddRange(IEnumerable<T> elements)
    {
      foreach (T element in elements)
        this.addIfNotPresent(element);
    }

    public void AddRange(ImmutableArray<T> elements)
    {
      foreach (T element in elements)
        this.addIfNotPresent(element);
    }

    /// <summary>
    /// Searches the set for a given value and returns the equal value it finds, if any.
    /// </summary>
    /// <param name="equalValue">The value to search for.</param>
    /// <param name="actualValue">
    /// The value from the set that the search found, or the default value of <typeparamref name="T" /> when the
    /// search yielded no match.
    /// </param>
    /// <returns>A value indicating whether the search was successful.</returns>
    /// <remarks>
    /// This can be useful when you want to reuse a previously stored reference instead of a newly constructed one
    /// (so that more sharing of references can occur) or to look up a value that has more complete data than the
    /// value you currently have, although their comparer functions indicate they are equal.
    /// </remarks>
    public bool TryGetValue(T equalValue, out T actualValue)
    {
      if (this.m_buckets != null)
      {
        int index = this.internalIndexOf(equalValue);
        if (index >= 0)
        {
          actualValue = this.m_slots[index].value;
          return true;
        }
      }
      actualValue = default (T);
      return false;
    }

    /// <summary>
    /// Calls function on every element.
    /// 
    /// PERF: Avoid this with capturing lambdas in perf sensitive code.
    /// </summary>
    public void ForEach(Action<T> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      foreach (T obj in this)
        action(obj);
    }

    /// <summary>
    /// Calls function on every element and ignores its return value.
    /// 
    /// PERF: Avoid this with capturing lambdas in perf sensitive code.
    /// </summary>
    public void ForEach<TRet>(Func<T, TRet> func)
    {
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      foreach (T obj in this)
      {
        TRet ret = func(obj);
      }
    }

    /// <summary>
    /// Calls given action on every element and clears the list.
    /// 
    /// PERF: Avoid this with capturing lambdas in perf sensitive code.
    /// </summary>
    public void ForEachAndClear(Action<T> action)
    {
      this.ForEach(action);
      this.Clear();
    }

    /// <summary>
    /// Calls given action on every element while ignoring return value from the function. Clears the list after the
    /// iteration.
    /// 
    /// PERF: Avoid this with capturing lambdas in perf sensitive code.
    /// </summary>
    public void ForEachAndClear<TRet>(Func<T, TRet> func)
    {
      this.ForEach<TRet>(func);
      this.Clear();
    }

    /// <summary>
    /// Take the union of this HashSet with other. Modifies this set.
    /// 
    /// Implementation note: GetSuggestedCapacity (to increase capacity in advance avoiding multiple resizes ended up
    /// not being useful in practice; quickly gets to the point where it's a wasteful check.
    /// </summary>
    /// <param name="other">enumerable with items to add</param>
    public void UnionWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      foreach (T obj in other)
        this.addIfNotPresent(obj);
    }

    /// <summary>
    /// Takes the intersection of this set with other. Modifies this set.
    /// 
    /// Implementation Notes: We get better perf if other is a hashset using same equality comparer, because we get
    /// constant contains check in other. Resulting cost is O(n1) to iterate over this.
    /// 
    /// If we can't go above route, iterate over the other and mark intersection by checking contains in this. Then
    /// loop over and delete any unmarked elements. Total cost is n2+n1.
    /// 
    /// Attempts to return early based on counts alone, using the property that the intersection of anything with the
    /// empty set is the empty set.
    /// </summary>
    /// <param name="other">enumerable with items to add</param>
    public void IntersectWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        return;
      if (other is ICollection<T> objs)
      {
        if (objs.Count == 0)
        {
          this.Clear();
          return;
        }
        if (other is Set<T> set && Set<T>.areEqualityComparersEqual(this, set))
        {
          this.intersectWithHashSetWithSameEc(set);
          return;
        }
      }
      this.intersectWithEnumerable(other);
    }

    /// <summary>
    /// Remove items in other from this set. Modifies this set.
    /// </summary>
    /// <param name="other">enumerable with items to remove</param>
    public void ExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        return;
      if (other == this)
      {
        this.Clear();
      }
      else
      {
        foreach (T obj in other)
          this.Remove(obj);
      }
    }

    /// <summary>
    /// Takes symmetric difference (XOR) with other and this set. Modifies this set.
    /// </summary>
    /// <param name="other">enumerable with items to XOR</param>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        this.UnionWith(other);
      else if (other == this)
        this.Clear();
      else if (other is Set<T> set && Set<T>.areEqualityComparersEqual(this, set))
        this.symmetricExceptWithUniqueHashSet(set);
      else
        this.symmetricExceptWithEnumerable(other);
    }

    /// <summary>
    /// Checks if this is a subset of other.
    /// 
    /// Implementation Notes: The following properties are used up-front to avoid element-wise checks:
    /// 1. If this is the empty set, then it's a subset of anything, including the empty set
    /// 2. If other has unique elements according to this equality comparer, and this has more elements than other,
    /// then it can't be a subset.
    /// 
    /// Furthermore, if other is a hashset using the same equality comparer, we can use a faster element-wise check.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true if this is a subset of other; false if not</returns>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        return true;
      if (other is Set<T> set && Set<T>.areEqualityComparersEqual(this, set))
        return this.m_count <= set.Count && this.isSubsetOfHashSetWithSameEc(set);
      Set<T>.ElementCount elementCount = this.checkUniqueAndUnfoundElements(other, false);
      return elementCount.uniqueCount == this.m_count && elementCount.unfoundCount >= 0;
    }

    /// <summary>
    /// Checks if this is a proper subset of other (i.e. strictly contained in)
    /// 
    /// Implementation Notes: The following properties are used up-front to avoid element-wise checks:
    /// 1. If this is the empty set, then it's a proper subset of a set that contains at least one element, but it's
    /// not a proper subset of the empty set.
    /// 2. If other has unique elements according to this equality comparer, and this has &gt;= the number of
    /// elements in other, then this can't be a proper subset.
    /// 
    /// Furthermore, if other is a hashset using the same equality comparer, we can use a faster element-wise check.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true if this is a proper subset of other; false if not</returns>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (other is ICollection<T> objs)
      {
        if (this.m_count == 0)
          return objs.Count > 0;
        if (other is Set<T> set && Set<T>.areEqualityComparersEqual(this, set))
          return this.m_count < set.Count && this.isSubsetOfHashSetWithSameEc(set);
      }
      Set<T>.ElementCount elementCount = this.checkUniqueAndUnfoundElements(other, false);
      return elementCount.uniqueCount == this.m_count && elementCount.unfoundCount > 0;
    }

    /// <summary>
    /// Checks if this is a superset of other
    /// 
    /// Implementation Notes: The following properties are used up-front to avoid element-wise checks:
    /// 1. If other has no elements (it's the empty set), then this is a superset, even if this is also the empty
    /// set.
    /// 2. If other has unique elements according to this equality comparer, and this has less than the number of
    /// elements in other, then this can't be a superset
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true if this is a superset of other; false if not</returns>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (other is ICollection<T> objs)
      {
        if (objs.Count == 0)
          return true;
        if (other is Set<T> set2 && Set<T>.areEqualityComparersEqual(this, set2) && set2.Count > this.m_count)
          return false;
      }
      return this.containsAllElements(other);
    }

    /// <summary>
    /// Checks if this is a proper superset of other (i.e. other strictly contained in this)
    /// 
    /// Implementation Notes: This is slightly more complicated than above because we have to keep track if there was
    /// at least one element not contained in other.
    /// 
    /// The following properties are used up-front to avoid element-wise checks:
    /// 1. If this is the empty set, then it can't be a proper superset of any set, even if other is the empty set.
    /// 2. If other is an empty set and this contains at least 1 element, then this is a proper superset.
    /// 3. If other has unique elements according to this equality comparer, and other's count is greater than or
    /// equal to this count, then this can't be a proper superset
    /// 
    /// Furthermore, if other has unique elements according to this equality comparer, we can use a faster
    /// element-wise check.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true if this is a proper superset of other; false if not</returns>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        return false;
      if (other is ICollection<T> objs)
      {
        if (objs.Count == 0)
          return true;
        if (other is Set<T> set && Set<T>.areEqualityComparersEqual(this, set))
          return set.Count < this.m_count && this.containsAllElements((IEnumerable<T>) set);
      }
      Set<T>.ElementCount elementCount = this.checkUniqueAndUnfoundElements(other, true);
      return elementCount.uniqueCount < this.m_count && elementCount.unfoundCount == 0;
    }

    /// <summary>
    /// Checks if this set overlaps other (i.e. they share at least one item)
    /// </summary>
    /// <param name="other"></param>
    /// <returns>true if these have at least one common element; false if disjoint</returns>
    public bool Overlaps(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_count == 0)
        return false;
      foreach (T obj in other)
      {
        if (this.Contains(obj))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Checks if this and other contain the same elements. This is set equality: duplicates and order are ignored
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SetEquals(IEnumerable<T> other)
    {
      switch (other)
      {
        case null:
          throw new ArgumentNullException(nameof (other));
        case Set<T> set when Set<T>.areEqualityComparersEqual(this, set):
          return this.m_count == set.Count && this.containsAllElements((IEnumerable<T>) set);
        case ICollection<T> objs when this.m_count == 0 && objs.Count > 0:
          return false;
        default:
          Set<T>.ElementCount elementCount = this.checkUniqueAndUnfoundElements(other, true);
          return elementCount.uniqueCount == this.m_count && elementCount.unfoundCount == 0;
      }
    }

    public void CopyTo(T[] array) => this.CopyTo(array, 0, this.m_count);

    public void CopyTo(T[] array, int arrayIndex, int count)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (arrayIndex > array.Length || count > array.Length - arrayIndex)
        throw new ArgumentException("Given array is too small.", nameof (array));
      int num = 0;
      for (int index = 0; index < this.m_lastIndex && num < count; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          array[arrayIndex + num] = this.m_slots[index].value;
          ++num;
        }
      }
    }

    /// <summary>
    /// Remove elements that match specified predicate. Returns the number of elements removed
    /// </summary>
    public int RemoveWhere(Predicate<T> match, Action<T> removedValueCallback = null)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      int num = 0;
      for (int index = 0; index < this.m_lastIndex; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          T obj = this.m_slots[index].value;
          if (match(obj) && this.Remove(obj))
          {
            if (removedValueCallback != null)
              removedValueCallback(obj);
            ++num;
          }
        }
      }
      return num;
    }

    /// <summary>
    /// Allocation-free sampling of random set element in O(n).
    /// </summary>
    public T SampleRandomKeyOrDefault(IRandom random)
    {
      if (this.IsEmpty)
        return default (T);
      int num = random.NextInt(this.Count);
      foreach (T obj in this)
      {
        if (num <= 0)
          return obj;
        --num;
      }
      Assert.Fail("This should not happen.");
      return default (T);
    }

    /// <summary>
    /// Gets the IEqualityComparer that is used to determine equality of keys for the HashSet.
    /// </summary>
    public IEqualityComparer<T> Comparer => this.m_comparer;

    /// <summary>
    /// Sets the capacity of this set to the size of the set (rounded up to nearest prime), unless count is 0, in
    /// which case we release references.
    /// 
    /// This method can be used to minimize a set's memory overhead once it is known that no new elements will be
    /// added to the set. To completely clear a set and release all memory referenced by the set, execute the
    /// following statements:
    /// 
    /// set.Clear(); set.TrimExcess();
    /// </summary>
    public void TrimExcess()
    {
      Assert.That<int>(this.m_count).IsNotNegative("m_count is negative");
      if (this.m_count == 0)
      {
        this.m_buckets = (int[]) null;
        this.m_slots = (Set<T>.Slot[]) null;
        ++this.m_version;
      }
      else
      {
        Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>("m_buckets was null but m_count > 0");
        int prime = HashHelpers.GetPrime(this.m_count);
        Set<T>.Slot[] slotArray = new Set<T>.Slot[prime];
        int[] numArray = new int[prime];
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_lastIndex; ++index2)
        {
          if (this.m_slots[index2].hashCode >= 0)
          {
            slotArray[index1] = this.m_slots[index2];
            int index3 = slotArray[index1].hashCode % prime;
            slotArray[index1].next = numArray[index3] - 1;
            numArray[index3] = index1 + 1;
            ++index1;
          }
        }
        Assert.That<int>(slotArray.Length).IsLessOrEqual(this.m_slots.Length, "capacity increased after TrimExcess");
        this.m_lastIndex = index1;
        this.m_slots = slotArray;
        this.m_buckets = numArray;
        this.m_freeList = -1;
      }
    }

    /// <summary>
    /// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime greater than or equal to
    /// capacity.
    /// </summary>
    private void initialize(int capacity)
    {
      Assert.That<int[]>(this.m_buckets).IsNull<int[]>("Initialize was called but m_buckets was non-null");
      int prime = HashHelpers.GetPrime(capacity);
      this.m_buckets = new int[prime];
      this.m_slots = new Set<T>.Slot[prime];
      this.m_freeList = -1;
    }

    /// <summary>
    /// Expand to new capacity. New capacity is next prime greater than or equal to suggested size. This is called
    /// when the underlying array is filled. This performs no defragmentation, allowing faster execution; note that
    /// this is reasonable since AddIfNotPresent attempts to insert new elements in re-opened spots.
    /// </summary>
    private void increaseCapacity()
    {
      Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>("IncreaseCapacity called on a set with no elements");
      int newSize = HashHelpers.ExpandPrime(this.m_count);
      if (newSize <= this.m_count)
        throw new ArgumentException("Capacity overflow");
      this.setCapacity(newSize, false);
    }

    /// <summary>
    /// Set the underlying buckets array to size newSize and rehash. Note that newSize
    /// *must* be a prime. It is very likely that you want to call IncreaseCapacity() instead of this method.
    /// </summary>
    private void setCapacity(int newSize, bool forceNewHashCodes)
    {
      Assert.That<bool>(HashHelpers.IsPrime(newSize)).IsTrue("New size is not prime!");
      Set<T>.Slot[] destinationArray = new Set<T>.Slot[newSize];
      if (this.m_slots != null)
        Array.Copy((Array) this.m_slots, 0, (Array) destinationArray, 0, this.m_lastIndex);
      if (forceNewHashCodes)
      {
        for (int index = 0; index < this.m_lastIndex; ++index)
        {
          if (destinationArray[index].hashCode != -1)
            destinationArray[index].hashCode = this.internalGetHashCode(destinationArray[index].value);
        }
      }
      int[] numArray = new int[newSize];
      for (int index1 = 0; index1 < this.m_lastIndex; ++index1)
      {
        int index2 = destinationArray[index1].hashCode % newSize;
        destinationArray[index1].next = numArray[index2] - 1;
        numArray[index2] = index1 + 1;
      }
      this.m_slots = destinationArray;
      this.m_buckets = numArray;
    }

    /// <summary>
    /// Adds value to HashSet if not contained already Returns true if added and false if already present
    /// </summary>
    private bool addIfNotPresent(T value)
    {
      if (this.m_buckets == null)
      {
        Assert.That<Set<T>.Slot[]>(this.m_slots).IsNull<Set<T>.Slot[]>("Adding to a loaded set that was not initialized yet.");
        this.initialize(0);
      }
      int hashCode = this.internalGetHashCode(value);
      int index1 = hashCode % this.m_buckets.Length;
      for (int index2 = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index2 >= 0; index2 = this.m_slots[index2].next)
      {
        if (this.m_slots[index2].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index2].value, value))
          return false;
      }
      int index3;
      if (this.m_freeList >= 0)
      {
        index3 = this.m_freeList;
        this.m_freeList = this.m_slots[index3].next;
      }
      else
      {
        if (this.m_lastIndex == this.m_slots.Length)
        {
          this.increaseCapacity();
          index1 = hashCode % this.m_buckets.Length;
        }
        index3 = this.m_lastIndex;
        ++this.m_lastIndex;
      }
      this.m_slots[index3].hashCode = hashCode;
      this.m_slots[index3].value = value;
      this.m_slots[index3].next = this.m_buckets[index1] - 1;
      this.m_buckets[index1] = index3 + 1;
      ++this.m_count;
      ++this.m_version;
      return true;
    }

    private void addValue(int index, int hashCode, T value)
    {
      int index1 = hashCode % this.m_buckets.Length;
      Assert.That<int>(this.m_freeList).IsEqualTo(-1);
      this.m_slots[index].hashCode = hashCode;
      this.m_slots[index].value = value;
      this.m_slots[index].next = this.m_buckets[index1] - 1;
      this.m_buckets[index1] = index + 1;
    }

    /// <summary>
    /// Checks if this contains of other's elements. Iterates over other's elements and returns false as soon as it
    /// finds an element in other that's not in this. Used by SupersetOf, ProperSupersetOf, and SetEquals.
    /// </summary>
    private bool containsAllElements(IEnumerable<T> other)
    {
      foreach (T obj in other)
      {
        if (!this.Contains(obj))
          return false;
      }
      return true;
    }

    /// <summary>
    /// Implementation Notes: If other is a hashset and is using same equality comparer, then checking subset is
    /// faster. Simply check that each element in this is in other.
    /// 
    /// Note: if other doesn't use same equality comparer, then Contains check is invalid, which is why callers must
    /// take are of this.
    /// 
    /// If callers are concerned about whether this is a proper subset, they take care of that.
    /// </summary>
    private bool isSubsetOfHashSetWithSameEc(Set<T> other)
    {
      foreach (T obj in this)
      {
        if (!other.Contains(obj))
          return false;
      }
      return true;
    }

    /// <summary>
    /// If other is a hashset that uses same equality comparer, intersect is much faster because we can use other's
    /// Contains
    /// </summary>
    private void intersectWithHashSetWithSameEc(Set<T> other)
    {
      for (int index = 0; index < this.m_lastIndex; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          T obj = this.m_slots[index].value;
          if (!other.Contains(obj))
            this.Remove(obj);
        }
      }
    }

    /// <summary>
    /// Iterate over other. If contained in this, mark an element in bit array corresponding to its position in
    /// m_slots. If anything is unmarked (in bit array), remove it.
    /// 
    /// This attempts to allocate on the stack, if below StackAllocThreshold.
    /// </summary>
    [SecuritySafeCritical]
    private unsafe void intersectWithEnumerable(IEnumerable<T> other)
    {
      int lastIndex = this.m_lastIndex;
      int intArrayLength = BitHelper.ToIntArrayLength(lastIndex);
      BitHelper bitHelper;
      if (intArrayLength <= 100)
      {
        int* bitArrayPtr = stackalloc int[intArrayLength];
        bitHelper = new BitHelper(bitArrayPtr, intArrayLength);
      }
      else
        bitHelper = new BitHelper(new int[intArrayLength], intArrayLength);
      foreach (T obj in other)
      {
        int bitPosition = this.internalIndexOf(obj);
        if (bitPosition >= 0)
          bitHelper.MarkBit(bitPosition);
      }
      for (int bitPosition = 0; bitPosition < lastIndex; ++bitPosition)
      {
        if (this.m_slots[bitPosition].hashCode >= 0 && !bitHelper.IsMarked(bitPosition))
          this.Remove(this.m_slots[bitPosition].value);
      }
    }

    /// <summary>
    /// Used internally by set operations which have to rely on bit array marking. This is like Contains but returns
    /// index in slots array.
    /// </summary>
    private int internalIndexOf(T item)
    {
      Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>("m_buckets was null; callers should check first");
      int hashCode = this.internalGetHashCode(item);
      for (int index = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index >= 0; index = this.m_slots[index].next)
      {
        if (this.m_slots[index].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index].value, item))
          return index;
      }
      return -1;
    }

    /// <summary>
    /// if other is a set, we can assume it doesn't have duplicate elements, so use this
    /// technique: if can't remove, then it wasn't present in this set, so add.
    /// 
    /// As with other methods, callers take care of ensuring that other is a hashset using the same equality
    /// comparer.
    /// </summary>
    private void symmetricExceptWithUniqueHashSet(Set<T> other)
    {
      foreach (T obj in other)
      {
        if (!this.Remove(obj))
          this.addIfNotPresent(obj);
      }
    }

    /// <summary>
    /// Implementation notes:
    /// 
    /// Used for symmetric except when other isn't a HashSet. This is more tedious because other may contain
    /// duplicates. HashSet technique could fail in these situations:
    /// 1. Other has a duplicate that's not in this: HashSet technique would add then remove it.
    /// 2. Other has a duplicate that's in this: HashSet technique would remove then add it back. In general, its
    /// presence would be toggled each time it appears in other.
    /// 
    /// This technique uses bit marking to indicate whether to add/remove the item. If already present in collection,
    /// it will get marked for deletion. If added from other, it will get marked as something not to remove.
    /// </summary>
    [SecuritySafeCritical]
    private unsafe void symmetricExceptWithEnumerable(IEnumerable<T> other)
    {
      int lastIndex = this.m_lastIndex;
      int intArrayLength = BitHelper.ToIntArrayLength(lastIndex);
      BitHelper bitHelper1;
      BitHelper bitHelper2;
      if (intArrayLength <= 50)
      {
        int* bitArrayPtr1 = stackalloc int[intArrayLength];
        bitHelper1 = new BitHelper(bitArrayPtr1, intArrayLength);
        int* bitArrayPtr2 = stackalloc int[intArrayLength];
        bitHelper2 = new BitHelper(bitArrayPtr2, intArrayLength);
      }
      else
      {
        bitHelper1 = new BitHelper(new int[intArrayLength], intArrayLength);
        bitHelper2 = new BitHelper(new int[intArrayLength], intArrayLength);
      }
      foreach (T obj in other)
      {
        int location;
        if (this.addOrGetLocation(obj, out location))
          bitHelper2.MarkBit(location);
        else if (location < lastIndex && !bitHelper2.IsMarked(location))
          bitHelper1.MarkBit(location);
      }
      for (int bitPosition = 0; bitPosition < lastIndex; ++bitPosition)
      {
        if (bitHelper1.IsMarked(bitPosition))
          this.Remove(this.m_slots[bitPosition].value);
      }
    }

    /// <summary>
    /// Add if not already in hashset. Returns an out param indicating index where added. This is used by
    /// SymmetricExcept because it needs to know the following things:
    /// - whether the item was already present in the collection or added from other
    /// - where it's located (if already present, it will get marked for removal, otherwise marked for keeping)
    /// </summary>
    private bool addOrGetLocation(T value, out int location)
    {
      Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>("m_buckets was null; callers should check first");
      int hashCode = this.internalGetHashCode(value);
      int index1 = hashCode % this.m_buckets.Length;
      for (int index2 = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index2 >= 0; index2 = this.m_slots[index2].next)
      {
        if (this.m_slots[index2].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index2].value, value))
        {
          location = index2;
          return false;
        }
      }
      int index3;
      if (this.m_freeList >= 0)
      {
        index3 = this.m_freeList;
        this.m_freeList = this.m_slots[index3].next;
      }
      else
      {
        if (this.m_lastIndex == this.m_slots.Length)
        {
          this.increaseCapacity();
          index1 = hashCode % this.m_buckets.Length;
        }
        index3 = this.m_lastIndex;
        ++this.m_lastIndex;
      }
      this.m_slots[index3].hashCode = hashCode;
      this.m_slots[index3].value = value;
      this.m_slots[index3].next = this.m_buckets[index1] - 1;
      this.m_buckets[index1] = index3 + 1;
      ++this.m_count;
      ++this.m_version;
      location = index3;
      return true;
    }

    /// <summary>
    /// Determines counts that can be used to determine equality, subset, and superset. This is only used when other
    /// is an IEnumerable and not a HashSet. If other is a HashSet these properties can be checked faster without use
    /// of marking because we can assume other has no duplicates.
    /// 
    /// The following count checks are performed by callers:
    /// 1. Equals: checks if unfoundCount = 0 and uniqueFoundCount = m_count; i.e. everything in other is in this and
    /// everything in this is in other
    /// 2. Subset: checks if unfoundCount &gt;= 0 and uniqueFoundCount = m_count; i.e. other may have elements not in
    /// this and everything in this is in other
    /// 3. Proper subset: checks if unfoundCount &gt; 0 and uniqueFoundCount = m_count; i.e other must have at least
    /// one element not in this and everything in this is in other
    /// 4. Proper superset: checks if unfound count = 0 and uniqueFoundCount strictly less than m_count; i.e.
    /// everything in other was in this and this had at least one element not contained in other.
    /// 
    /// An earlier implementation used delegates to perform these checks rather than returning an ElementCount
    /// struct; however this was changed due to the perf overhead of delegates.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="returnIfUnfound">
    /// Allows us to finish faster for equals and proper superset because unfoundCount must be 0.
    /// </param>
    /// <returns></returns>
    [SecuritySafeCritical]
    private unsafe Set<T>.ElementCount checkUniqueAndUnfoundElements(
      IEnumerable<T> other,
      bool returnIfUnfound)
    {
      if (this.m_count == 0)
      {
        int num = 0;
        using (IEnumerator<T> enumerator = other.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            T current = enumerator.Current;
            ++num;
          }
        }
        Set<T>.ElementCount elementCount;
        elementCount.uniqueCount = 0;
        elementCount.unfoundCount = num;
        return elementCount;
      }
      Assert.That<bool>(this.m_buckets != null && this.m_count > 0).IsTrue("m_buckets was null but count greater than 0");
      int intArrayLength = BitHelper.ToIntArrayLength(this.m_lastIndex);
      BitHelper bitHelper;
      if (intArrayLength <= 100)
      {
        int* bitArrayPtr = stackalloc int[intArrayLength];
        bitHelper = new BitHelper(bitArrayPtr, intArrayLength);
      }
      else
        bitHelper = new BitHelper(new int[intArrayLength], intArrayLength);
      int num1 = 0;
      int num2 = 0;
      foreach (T obj in other)
      {
        int bitPosition = this.internalIndexOf(obj);
        if (bitPosition >= 0)
        {
          if (!bitHelper.IsMarked(bitPosition))
          {
            bitHelper.MarkBit(bitPosition);
            ++num2;
          }
        }
        else
        {
          ++num1;
          if (returnIfUnfound)
            break;
        }
      }
      Set<T>.ElementCount elementCount1;
      elementCount1.uniqueCount = num2;
      elementCount1.unfoundCount = num1;
      return elementCount1;
    }

    /// <summary>Copies this to an array.</summary>
    public T[] ToArray()
    {
      T[] array = new T[this.Count];
      this.CopyTo(array);
      return array;
    }

    public T[] ToArrayPooled()
    {
      T[] array = ArrayPool<T>.Get(this.Count);
      this.CopyTo(array);
      return array;
    }

    /// <summary>
    /// Internal method used for HashSetEqualityComparer. Compares set1 and set2 according to specified comparer.
    /// 
    /// Because items are hashed according to a specific equality comparer, we have to resort to n^2 search if
    /// they're using different equality comparers.
    /// </summary>
    internal static bool HashSetEquals(Set<T> set1, Set<T> set2, IEqualityComparer<T> comparer)
    {
      if (set1 == null)
        return set2 == null;
      if (set2 == null)
        return false;
      if (Set<T>.areEqualityComparersEqual(set1, set2))
      {
        if (set1.Count != set2.Count)
          return false;
        foreach (T obj in set2)
        {
          if (!set1.Contains(obj))
            return false;
        }
        return true;
      }
      foreach (T x in set2)
      {
        bool flag = false;
        foreach (T y in set1)
        {
          if (comparer.Equals(x, y))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Checks if equality comparers are equal. This is used for algorithms that can speed up if it knows the other
    /// item has unique elements. I.e. if they're using different equality comparers, then uniqueness assumption
    /// between sets break.
    /// </summary>
    private static bool areEqualityComparersEqual(Set<T> set1, Set<T> set2)
    {
      return set1.Comparer.Equals((object) set2.Comparer);
    }

    /// <summary>
    /// Workaround Comparers that throw ArgumentNullException for GetHashCode(null).
    /// </summary>
    /// <returns>hash code</returns>
    private int internalGetHashCode(T item)
    {
      return (object) item == null ? 0 : this.m_comparer.GetHashCode(item) & int.MaxValue;
    }

    public void SerializeData(BlobWriter writer)
    {
      Assert.That<bool>(typeof (T).IsValueType || typeof (T) == typeof (string) || typeof (T).IsAssignableTo(typeof (IIsSafeAsHashKey))).IsTrue<string>("Reference '{0}' cannot be a key of serialized set due to non-deterministic hash.", typeof (T).Name);
      Assert.That<IEqualityComparer<T>>(this.Comparer).IsEqualTo<IEqualityComparer<T>>((IEqualityComparer<T>) EqualityComparer<T>.Default, "Cannot serialize set with a custom comparer.");
      writer.WriteIntNotNegative(this.m_count);
      Action<T, BlobWriter> serializerFor = writer.GetSerializerFor<T>();
      foreach (T obj in this)
        serializerFor(obj, writer);
    }

    public void DeserializeData(BlobReader reader)
    {
      int length = reader.ReadIntNotNegative();
      reader.SetField<Set<T>>(this, "m_comparer", (object) EqualityComparer<T>.Default);
      if (length > 0)
      {
        this.m_slots = new Set<T>.Slot[length];
        Func<BlobReader, T> deserializerFor = reader.GetDeserializerFor<T>();
        for (int index = 0; index < length; ++index)
          this.m_slots[index].value = deserializerFor(reader);
      }
      reader.RegisterInitAfterLoad<Set<T>>(this, "initAfterLoad", InitPriority.Highest);
    }

    private void initAfterLoad()
    {
      if (this.m_slots == null)
        return;
      Set<T>.Slot[] slots = this.m_slots;
      this.initialize(slots.Length);
      int num = 0;
      foreach (Set<T>.Slot slot in slots)
      {
        if (this.addIfNotPresent(slot.value))
          ++num;
      }
      if (num == slots.Length)
        return;
      Log.Warning(string.Format("Set was loaded with {0} items but only {1} were unique.", (object) slots.Length, (object) num));
    }

    public override string ToString()
    {
      return string.Format("Count: {0}, capacity: {1}", (object) this.Count, (object) this.Capacity);
    }

    public static void Serialize(Set<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Set<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Set<T>.s_serializeDataDelayedAction);
    }

    public static Set<T> Deserialize(BlobReader reader)
    {
      Set<T> set;
      if (reader.TryStartClassDeserialization<Set<T>>(out set))
        reader.EnqueueDataDeserialization((object) set, Set<T>.s_deserializeDataDelayedAction);
      return set;
    }

    static Set()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Set<T>.s_emptySet = (IReadOnlySet<T>) new Set<T>();
      Set<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Set<T>) obj).SerializeData(writer));
      Set<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Set<T>) obj).DeserializeData(reader));
    }

    private struct ElementCount
    {
      internal int uniqueCount;
      internal int unfoundCount;
    }

    private struct Slot
    {
      internal int hashCode;
      internal int next;
      internal T value;
    }

    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly Set<T> m_set;
      private readonly int m_version;
      private int m_index;

      public T Current { get; private set; }

      internal Enumerator(Set<T> set)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_set = set;
        this.m_index = 0;
        this.m_version = set.m_version;
        this.Current = default (T);
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        if (this.m_version != this.m_set.m_version)
          throw new InvalidOperationException("Set changed while enumerating.");
        for (; this.m_index < this.m_set.m_lastIndex; ++this.m_index)
        {
          if (this.m_set.m_slots[this.m_index].hashCode >= 0)
          {
            this.Current = this.m_set.m_slots[this.m_index].value;
            ++this.m_index;
            return true;
          }
        }
        this.m_index = this.m_set.m_lastIndex + 1;
        this.Current = default (T);
        return false;
      }

      object IEnumerator.Current
      {
        get
        {
          if (this.m_index == 0 || this.m_index == this.m_set.m_lastIndex + 1)
            throw new InvalidOperationException("Invalid enum call");
          return (object) this.Current;
        }
      }

      void IEnumerator.Reset()
      {
        if (this.m_version != this.m_set.m_version)
          throw new InvalidOperationException("Set changed when before enumeration reset called.");
        this.m_index = 0;
        this.Current = default (T);
      }
    }
  }
}
