// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Queueue`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// A simple Queue of generic objects. Internally it is implemented as a circular buffer, so Enqueue is amortized
  /// O(1) but can be O(n). Dequeue is always O(1).
  /// </summary>
  /// <remarks>
  /// Compared to original <see cref="T:System.Collections.Generic.Queue`1" /> there are following changes:
  /// * Constructor <see cref="M:Mafi.Collections.Queueue`1.#ctor(System.Boolean,System.Int32)" /> that sets <see cref="F:Mafi.Collections.Queueue`1.OmitClearing" />. When set, clearing and
  /// popping do not zero-out internal array and is O(1). This is beneficial when structs are stored or stored
  /// references are not needed to be GCed. Use with care.
  /// * Is sealed.
  /// * Implements <see cref="T:Mafi.Collections.ReadonlyCollections.IIndexable`1" />.
  /// * Indexer <see cref="P:Mafi.Collections.Queueue`1.Item(System.Int32)" /> that indexes from the front of the queue (index 0 == peek).
  /// * Member <see cref="P:Mafi.Collections.Queueue`1.IsEmpty" />.
  /// * Read/write <see cref="P:Mafi.Collections.Queueue`1.Capacity" />.
  /// * Method <see cref="M:Mafi.Collections.Queueue`1.EnsureCapacity(System.Int32)" />.
  /// * Method <see cref="M:Mafi.Collections.Queueue`1.Reverse" /> that reverses the queue in-place without allocations.
  /// * Method <see cref="M:Mafi.Collections.Queueue`1.TryRemove(`0)" /> that cab remove arbitrary element.
  /// * Efficient struct and class based Enumerators with condition-less access to Current no Dispose.
  /// </remarks>
  [DebuggerDisplay("Count={Count}")]
  [GenerateSerializer(true, null, 0)]
  public sealed class Queueue<T> : 
    IIndexable<T>,
    ICollectionWithCount,
    IEnumerable<T>,
    IEnumerable,
    IReadOnlyCollection<T>
  {
    public const int DEFAULT_CAPACITY = 4;
    private T[] m_array;
    /// <summary>Number of elements if the Queue.</summary>
    private int m_count;
    /// <summary>
    /// The index from which to dequeue if the queue isn't empty.
    /// </summary>
    private int m_head;
    /// <summary>
    /// The index at which to enqueue if the queue isn't full.
    /// </summary>
    private int m_tail;
    /// <summary>
    /// Whether zeroing-out of elements can be omitted when removing or clearing.
    /// </summary>
    public readonly bool OmitClearing;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private void incVersion()
    {
    }

    /// <summary>
    /// Creates a queue with a specific initial <paramref name="capacity" />. The initial <paramref name="capacity" />
    /// must be a non-negative number. By default an empty queue is created.
    /// </summary>
    public Queueue(int capacity = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Mafi.Assert.That<int>(capacity).IsNotNegative();
      this.m_array = capacity <= 0 ? Array.Empty<T>() : new T[capacity];
    }

    /// <param name="omitClearing">
    /// Whether this queue can omit clearing of internal array when <see cref="M:Mafi.Collections.Queueue`1.Clear" /> is called or items are
    /// removed from the queue. If this is set to true please keep in mind that any references inserted into the
    /// queue may be referenced until this queue is referenced and prevent them from being garbage collected.
    /// </param>
    /// <param name="capacity">Initial capacity</param>
    public Queueue(bool omitClearing, int capacity = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(capacity);
      this.OmitClearing = omitClearing;
    }

    private void validateAfterLoad()
    {
      if (this.m_array == null)
      {
        Mafi.Log.Error(string.Format("Invalid Queue after load: Null backing array, count={0}.", (object) this.m_count));
        this.m_array = Array.Empty<T>();
        this.m_count = 0;
      }
      else if (this.m_count < 0)
      {
        Mafi.Log.Error(string.Format("Invalid Queue after load: Negative count `{0}`.", (object) this.m_count));
        this.m_count = 0;
      }
      else
      {
        if (this.m_count <= this.m_array.Length)
          return;
        Mafi.Log.Error(string.Format("Invalid Queue after load: Count `{0}` greater than array size `{1}`.", (object) this.m_count, (object) this.m_array.Length));
        this.m_count = this.m_array.Length;
      }
    }

    /// <summary>Returns i-th element from the front of the queue.</summary>
    public T this[int index]
    {
      get
      {
        if ((uint) index > (uint) this.m_count)
          throw new IndexOutOfRangeException(string.Format("Indexing to queue of size {0} with index {1}.", (object) this.m_count, (object) index));
        return this.m_array[(this.m_head + index) % this.m_array.Length];
      }
      set
      {
        if ((uint) index > (uint) this.m_count)
          Mafi.Log.Error(string.Format("Indexing to queue of size {0} with index {1}.", (object) this.m_count, (object) index));
        else
          this.m_array[(this.m_head + index) % this.m_array.Length] = value;
      }
    }

    public ref T GetRefFirst()
    {
      Mafi.Assert.That<int>(this.m_count).IsPositive("Called `GetRefFirst` on an empty Queue.");
      return ref this.m_array[this.m_head];
    }

    public ref T GetRefLast()
    {
      Mafi.Assert.That<int>(this.m_count).IsPositive("Called `GetRefFirst` on an empty Queue.");
      return ref this.m_array[(this.m_head + this.m_count - 1) % this.m_array.Length];
    }

    public ref T GetRefAt(int index)
    {
      if (index > this.m_count)
        throw new IndexOutOfRangeException(string.Format("Indexing to queue of size {0} with index {1}.", (object) this.m_count, (object) index));
      return ref this.m_array[(this.m_head + index) % this.m_array.Length];
    }

    /// <summary>Number of elements in the Queue.</summary>
    /// <remarks>
    /// Do not implement as auto-property since this member is virtual due to IIndexable interface.
    /// </remarks>
    public int Count => this.m_count;

    /// <summary>Whether the queue is empty.</summary>
    public bool IsEmpty => this.m_count <= 0;

    /// <summary>Whether the queue is not empty.</summary>
    public bool IsNotEmpty => this.m_count > 0;

    /// <summary>
    /// Whether the queue storage is stored on one consecutive place (is not split across the array's boundary).
    /// WARNING: For perf reasons this returns false for en empty queue.
    /// </summary>
    /// <remarks>
    /// If this is true, the <c>m_count</c> starts at <c>m_head</c>.
    /// 
    /// Otherwise, there are <c>m_array.Length - m_head</c> elements starting at <c>m_head</c> and then <c>m_tail</c>
    /// elements starting from <c>0</c>.
    /// </remarks>
    private bool IsStoredConsecutively => this.m_head < this.m_tail || this.m_tail == 0;

    /// <summary>
    /// Returns the first element of the queue (the same as <see cref="M:Mafi.Collections.Queueue`1.Peek" />).
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the queue is empty.</exception>
    public T First
    {
      get
      {
        if (this.m_count <= 0)
          throw new InvalidOperationException("Called `First` on an empty Queue.");
        return this.m_array[this.m_head];
      }
    }

    /// <summary>
    /// Returns the last element at index <c>Count - 1</c> of this Queue.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the queue is empty.</exception>
    public T Last
    {
      get
      {
        if (this.m_count <= 0)
          throw new InvalidOperationException("Called `Last` on an empty Queue.");
        return this.m_array[(this.m_head + this.m_count - 1) % this.m_array.Length];
      }
    }

    /// <summary>
    /// Sets capacity to requested value. This value must be greater or equal to <see cref="P:Mafi.Collections.Queueue`1.Count" />.
    /// </summary>
    public int Capacity
    {
      get => this.m_array.Length;
      set
      {
        if (value == this.m_array.Length)
          return;
        if (value < this.m_count)
        {
          Mafi.Log.Warning("Cannot reduce capacity below current count.");
        }
        else
        {
          if (value <= 0)
          {
            if (value < 0)
              Mafi.Log.Warning("Trying to set queue capacity to negative value.");
            this.m_array = Array.Empty<T>();
            this.m_head = 0;
            this.m_tail = 0;
          }
          else
          {
            T[] destinationArray = new T[value];
            if (this.m_count > 0)
            {
              if (this.IsStoredConsecutively)
              {
                Array.Copy((Array) this.m_array, this.m_head, (Array) destinationArray, 0, this.m_count);
              }
              else
              {
                Mafi.Assert.That<int>(this.m_tail).IsWithinExcl(0, this.m_array.Length, "Bad tail!");
                int length = this.m_array.Length - this.m_head;
                if (length < 0)
                {
                  Mafi.Log.Error(string.Format("Bad head {0}, len {1}", (object) this.m_head, (object) this.m_array.Length));
                  length = 0;
                }
                Array.Copy((Array) this.m_array, this.m_head, (Array) destinationArray, 0, length);
                Array.Copy((Array) this.m_array, 0, (Array) destinationArray, this.m_array.Length - this.m_head, this.m_tail);
              }
            }
            this.m_array = destinationArray;
            this.m_head = 0;
            this.m_tail = this.m_count == value ? 0 : this.m_count;
          }
          this.incVersion();
        }
      }
    }

    /// <summary>
    /// Returns the object at the head of the queue. The object remains in the queue. If the queue is empty, this
    /// method throws an InvalidOperationException.
    /// </summary>
    [Pure]
    public T Peek()
    {
      if (this.m_count <= 0)
        throw new InvalidOperationException("Called `Peek` on an empty Queue.");
      return this.m_array[this.m_head];
    }

    [Pure]
    public T PeekOrDefault() => this.m_count > 0 ? this.m_array[this.m_head] : default (T);

    /// <summary>Adds given item to the end of this queue.</summary>
    public void Enqueue(T item)
    {
      if (this.m_count >= this.m_array.Length)
      {
        Mafi.Assert.That<int>(this.m_count).IsEqualTo(this.m_array.Length, "Adding to Queue that has count > capacity.");
        this.Capacity = this.m_array.Length == 0 ? 4 : 2 * this.m_array.Length;
      }
      Mafi.Assert.That<int>(this.m_tail).IsWithinExcl(0, this.m_array.Length, "Bad tail!");
      this.m_array[this.m_tail] = item;
      this.moveNext(ref this.m_tail);
      ++this.m_count;
      this.incVersion();
    }

    /// <summary>
    /// Adds given items to the end of this queue. This is equal as calling <see cref="M:Mafi.Collections.Queueue`1.Enqueue(`0)" /> in on each element
    /// in order.
    /// </summary>
    public void EnqueueRange(IEnumerable<T> items)
    {
      if (items is IIndexable<T> indexable)
      {
        this.EnsureCapacity(this.m_count + indexable.Count);
        int count = indexable.Count;
        for (int index = 0; index < count; ++index)
        {
          this.m_array[this.m_tail] = indexable[index];
          this.moveNext(ref this.m_tail);
        }
        this.m_count += count;
        this.incVersion();
      }
      else
      {
        foreach (T obj in items)
          this.Enqueue(obj);
      }
    }

    /// <summary>Adds given item to the start of this queue.</summary>
    public void EnqueueFirst(T item)
    {
      if (this.m_count >= this.m_array.Length)
        this.Capacity = this.m_count <= 0 ? 4 : 2 * this.m_array.Length;
      this.moveBack(ref this.m_head);
      this.m_array[this.m_head] = item;
      ++this.m_count;
      this.incVersion();
    }

    /// <summary>
    /// Enqueues the given item at an index from the front of the queue. Enqueueing at index 0 adds the item to the
    /// front of the queue (the same as <see cref="M:Mafi.Collections.Queueue`1.EnqueueFirst(`0)" /> call), index equal to elements count adds it at
    /// the back of the queue (same as <see cref="M:Mafi.Collections.Queueue`1.Enqueue(`0)" /> call).
    /// </summary>
    public void EnqueueAt(T item, int index)
    {
      Mafi.Assert.That<int>(index).IsWithinIncl(0, this.m_count);
      if (index <= 0)
        this.EnqueueFirst(item);
      else if (index >= this.m_count)
      {
        this.Enqueue(item);
      }
      else
      {
        this.EnsureCapacity(this.m_count + 1);
        int sourceIndex = (this.m_head + index) % this.m_array.Length;
        if (sourceIndex == this.m_tail)
        {
          this.m_array[this.m_tail] = item;
          this.moveNext(ref this.m_tail);
        }
        else if (sourceIndex < this.m_tail)
        {
          Array.Copy((Array) this.m_array, sourceIndex, (Array) this.m_array, sourceIndex + 1, this.m_tail - sourceIndex);
          this.moveNext(ref this.m_tail);
        }
        else
        {
          Array.Copy((Array) this.m_array, this.m_head, (Array) this.m_array, this.m_head - 1, sourceIndex - this.m_head);
          --this.m_head;
          --sourceIndex;
        }
        this.m_array[sourceIndex] = item;
        ++this.m_count;
        this.incVersion();
      }
    }

    /// <summary>
    /// Removes the object at the head of the queue and returns it.
    /// </summary>
    public T Dequeue()
    {
      if (this.m_count <= 0)
        throw new InvalidOperationException("Called `Dequeue` on an empty Queue.");
      T obj = this.m_array[this.m_head];
      this.m_array[this.m_head] = default (T);
      this.moveNext(ref this.m_head);
      --this.m_count;
      this.incVersion();
      return obj;
    }

    /// <summary>
    /// Removes all Objects from the queue. Internal array stays the same size.
    /// </summary>
    public void Clear()
    {
      if (this.m_count != 0)
      {
        if (!this.OmitClearing)
        {
          if (this.IsStoredConsecutively)
          {
            Array.Clear((Array) this.m_array, this.m_head, this.m_count);
          }
          else
          {
            Array.Clear((Array) this.m_array, this.m_head, this.m_array.Length - this.m_head);
            Array.Clear((Array) this.m_array, 0, this.m_tail);
          }
        }
        this.m_count = 0;
      }
      this.m_head = 0;
      this.m_tail = 0;
      this.incVersion();
    }

    /// <summary>
    /// Returns actual index of an element in the Queue. Note that this index should be used directly at <see cref="F:Mafi.Collections.Queueue`1.m_array" />. It is invalid to use indexer with this index.
    /// </summary>
    private int indexOf(T item)
    {
      if (this.m_count <= 0)
        return -1;
      if (this.IsStoredConsecutively)
        return Array.IndexOf<T>(this.m_array, item, this.m_head, this.m_count);
      int num = Array.IndexOf<T>(this.m_array, item, this.m_head, this.m_array.Length - this.m_head);
      return num < 0 ? Array.IndexOf<T>(this.m_array, item, 0, this.m_tail) : num;
    }

    /// <summary>
    /// Whether queue contains at least one object equal to item.
    /// </summary>
    [Pure]
    public bool Contains(T item) => this.indexOf(item) >= 0;

    /// <summary>
    /// Iterates over the objects in the queue, returning an array of the objects in the Queue, or an empty array if
    /// the queue is empty. The order of elements in the array is first in to last in, the same order produced by
    /// successive calls to Dequeue.
    /// </summary>
    [Pure]
    public T[] ToArray()
    {
      if (this.m_count <= 0)
        return Array.Empty<T>();
      T[] destinationArray = new T[this.m_count];
      if (this.IsStoredConsecutively)
      {
        Array.Copy((Array) this.m_array, this.m_head, (Array) destinationArray, 0, this.m_count);
      }
      else
      {
        Array.Copy((Array) this.m_array, this.m_head, (Array) destinationArray, 0, this.m_array.Length - this.m_head);
        Array.Copy((Array) this.m_array, 0, (Array) destinationArray, this.m_array.Length - this.m_head, this.m_tail);
      }
      return destinationArray;
    }

    /// <summary>
    /// Ensures that the capacity is at least <paramref name="minCapacity" />.
    /// </summary>
    public void EnsureCapacity(int minCapacity)
    {
      if (minCapacity <= this.m_array.Length)
        return;
      int num = this.m_array.Length == 0 ? 4 : this.m_array.Length * 2;
      if ((uint) num > 2146435071U)
      {
        Mafi.Log.Warning("Allocating max length Queueue with ~2G elements. Is this an error?");
        num = 2146435071;
      }
      if (num < minCapacity)
        num = minCapacity;
      this.Capacity = num;
    }

    /// <summary>
    /// Increments the index wrapping it if necessary. Inverse operation to <see cref="M:Mafi.Collections.Queueue`1.moveBack(System.Int32@)" />.
    /// </summary>
    private void moveNext(ref int index)
    {
      int num = index + 1;
      index = num < this.m_array.Length ? num : 0;
    }

    /// <summary>
    /// Decrements the index wrapping it if necessary. Inverse operation to <see cref="M:Mafi.Collections.Queueue`1.moveNext(System.Int32@)" />.
    /// </summary>
    private void moveBack(ref int index)
    {
      int num = index - 1;
      index = num >= 0 ? num : this.m_array.Length - 1;
    }

    /// <summary>Removes excess allocated memory.</summary>
    public void TrimExcess()
    {
      if (this.m_count >= (int) ((double) this.m_array.Length * 0.89999997615814209))
        return;
      this.Capacity = this.m_count;
    }

    public T PeekLast()
    {
      if (this.m_count <= 0)
        throw new InvalidOperationException("Called `PeekLast` on an empty Queue.");
      return this.m_array[this.m_tail > 0 ? this.m_tail - 1 : this.m_array.Length - 1];
    }

    public T PopLast()
    {
      if (this.m_count <= 0)
        throw new InvalidOperationException("Called `PopLast` on an empty Queue.");
      this.moveBack(ref this.m_tail);
      T obj = this.m_array[this.m_tail];
      this.m_array[this.m_tail] = default (T);
      --this.m_count;
      return obj;
    }

    /// <summary>
    /// Tries to remove given element from the queue. Returns true if element was removed.
    /// </summary>
    public bool TryRemove(T value)
    {
      int rawIndex = this.indexOf(value);
      if (rawIndex < 0)
        return false;
      this.removeAtRaw(rawIndex);
      return true;
    }

    /// <summary>
    /// Removes element at the given index. Elements are indexed from the head of the queue (oldest first).
    /// Note: Unlike <see cref="M:Mafi.Collections.Queueue`1.PopAt(System.Int32)" />, this won't throw an exception when invalid index is given, but it will log
    /// error so make sure the given index is valid.
    /// </summary>
    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.m_count)
        Mafi.Log.Error(string.Format("Failed to remove at {0}, count = {1}, ignoring.", (object) index, (object) this.m_count));
      else
        this.removeAtRaw((this.m_head + index) % this.m_array.Length);
    }

    /// <summary>
    /// Removes and returns element at given index. Elements are indexed from the head of the queue (oldest first).
    /// </summary>
    public T PopAt(int index)
    {
      if ((uint) index >= (uint) this.m_count)
        throw new ArgumentException(string.Format("Invalid index {0} during RemoveAt (count = {1}).", (object) index, (object) this.m_count));
      int rawIndex = (this.m_head + index) % this.m_array.Length;
      T obj = this.m_array[rawIndex];
      this.removeAtRaw(rawIndex);
      return obj;
    }

    /// <summary>
    /// Removes element at given raw index. Caller MUST ensure that the queue is not empty.
    /// </summary>
    private void removeAtRaw(int rawIndex)
    {
      this.m_array[rawIndex] = default (T);
      if (rawIndex >= this.m_head)
      {
        Array.Copy((Array) this.m_array, this.m_head, (Array) this.m_array, this.m_head + 1, rawIndex - this.m_head);
        this.moveNext(ref this.m_head);
      }
      else
      {
        Mafi.Assert.That<int>(rawIndex).IsWithinExcl(0, this.m_tail);
        Mafi.Assert.That<int>(this.m_tail).IsPositive();
        Array.Copy((Array) this.m_array, rawIndex + 1, (Array) this.m_array, rawIndex, this.m_tail - rawIndex - 1);
        this.moveBack(ref this.m_tail);
      }
      --this.m_count;
      this.incVersion();
    }

    /// <summary>
    /// Reverses the queue in-place in O(n) with no extra allocations.
    /// </summary>
    /// <remarks>
    /// Reverse could be done in O(1) if we allowed reversing direction of the internal representation. Unfortunately
    /// that would complicate all other operations.
    /// </remarks>
    public void Reverse()
    {
      if (this.m_count <= 0)
        return;
      if (this.IsStoredConsecutively)
      {
        Array.Reverse((Array) this.m_array, this.m_head, this.m_count);
      }
      else
      {
        int length = this.m_array.Length;
        int head = this.m_head;
        int index;
        for (index = this.m_tail - 1; head < length && index >= 0; --index)
        {
          Swap.Them<T>(ref this.m_array[head], ref this.m_array[index]);
          ++head;
        }
        bool flag1 = index == -1;
        bool flag2 = head == length;
        if (flag1)
        {
          if (flag2)
            return;
          Array.Reverse((Array) this.m_array, head, length - head);
        }
        else
        {
          if (!flag2)
            return;
          Array.Reverse((Array) this.m_array, 0, index + 1);
        }
      }
    }

    protected void SerializeData(BlobWriter writer)
    {
      if (this.m_count < 0)
      {
        Mafi.Log.Error("Trying to write queue with negative count, setting count to 0.");
        this.m_count = 0;
      }
      if (this.m_count > this.m_array.Length)
      {
        Mafi.Log.Error("Trying to write queue with count larger than array length, setting count to array length.");
        this.m_count = this.m_array.Length;
      }
      writer.WriteIntNotNegative(this.m_array.Length);
      writer.WriteIntNotNegative(this.m_count);
      writer.WriteBool(this.OmitClearing);
      Action<T, BlobWriter> serializerFor = writer.GetSerializerFor<T>();
      for (int index = 0; index < this.m_count; ++index)
        serializerFor(this.m_array[(this.m_head + index) % this.m_array.Length], writer);
    }

    protected void DeserializeData(BlobReader reader)
    {
      this.m_array = new T[reader.ReadIntNotNegative()];
      this.m_count = reader.ReadIntNotNegative();
      reader.SetField<Queueue<T>>(this, "OmitClearing", (object) reader.ReadBool());
      Func<BlobReader, T> deserializerFor = reader.GetDeserializerFor<T>();
      for (int index = 0; index < this.m_count; ++index)
        this.m_array[index] = deserializerFor(reader);
      this.m_tail = this.m_count == this.m_array.Length ? 0 : this.m_count;
      this.validateAfterLoad();
    }

    [DebuggerStepThrough]
    [Pure]
    public Queueue<T>.Enumerator GetEnumerator() => new Queueue<T>.Enumerator(this);

    [DebuggerStepThrough]
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Queueue<T>.EnumeratorClass(this);

    [Pure]
    [DebuggerStepThrough]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return (IEnumerator<T>) new Queueue<T>.EnumeratorClass(this);
    }

    [DebuggerStepThrough]
    [Pure]
    IndexableEnumerator<T> IIndexable<T>.GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }

    [DebuggerStepThrough]
    [Pure]
    IEnumerable<T> IIndexable<T>.AsEnumerable() => (IEnumerable<T>) this;

    public static void Serialize(Queueue<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Queueue<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Queueue<T>.s_serializeDataDelayedAction);
    }

    public static Queueue<T> Deserialize(BlobReader reader)
    {
      Queueue<T> queueue;
      if (reader.TryStartClassDeserialization<Queueue<T>>(out queueue))
        reader.EnqueueDataDeserialization((object) queueue, Queueue<T>.s_deserializeDataDelayedAction);
      return queueue;
    }

    static Queueue()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Queueue<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Queueue<T>) obj).SerializeData(writer));
      Queueue<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Queueue<T>) obj).DeserializeData(reader));
    }

    /// <summary>
    /// Struct enumerator that is optimized for fast and allocation-free iteration through the Lyst.
    /// </summary>
    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly Queueue<T> m_queue;
      private int m_index;

      internal Enumerator(Queueue<T> queue)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_queue = queue;
        this.m_index = -1;
      }

      public bool MoveNext() => ++this.m_index < this.m_queue.m_count;

      public T Current => this.m_queue[this.m_index];
    }

    /// <summary>
    /// Separate enumerator class that implements <see cref="T:System.Collections.Generic.IEnumerator`1" />.
    /// </summary>
    /// <remarks>By implementing this enumerator as a class we avoid boxing.</remarks>
    [DebuggerStepThrough]
    private class EnumeratorClass : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly Queueue<T> m_queue;
      private int m_index;

      internal EnumeratorClass(Queueue<T> queue)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_index = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_queue = queue;
      }

      public bool MoveNext() => ++this.m_index < this.m_queue.m_count;

      public T Current => this.m_queue[this.m_index];

      object IEnumerator.Current => (object) this.Current;

      public void Reset() => this.m_index = -1;

      public void Dispose()
      {
      }
    }
  }
}
