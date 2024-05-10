// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Stak`1
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
  /// A simple stack of objects. Internally it is implemented as an array, so Push is amortized O(1) but can be O(n).
  /// Pop is O(1).
  /// </summary>
  /// <remarks>
  /// Compared to original <see cref="T:System.Collections.Generic.Stack`1" /> there are following changes:
  /// * Constructor <see cref="M:Mafi.Collections.Stak`1.#ctor(System.Boolean,System.Int32)" /> that sets <see cref="F:Mafi.Collections.Stak`1.OmitClearing" />. When set, clearing and
  /// popping do not zero-out internal array and is O(1). This is beneficial when structs are stored or stored
  /// references are not needed to be GCed. Use with care.
  /// * Is sealed.
  /// * Implements <see cref="T:Mafi.Collections.ReadonlyCollections.IIndexable`1" />.
  /// * Indexer <see cref="P:Mafi.Collections.Stak`1.Item(System.Int32)" /> that indexes from the top of the stack.
  /// * Member <see cref="P:Mafi.Collections.Stak`1.IsEmpty" />.
  /// * Read/write <see cref="P:Mafi.Collections.Stak`1.Capacity" />.
  /// * Method <see cref="M:Mafi.Collections.Stak`1.EnsureCapacity(System.Int32)" />.
  /// * Methods <see cref="M:Mafi.Collections.Stak`1.PushRange(Mafi.Collections.ReadonlyCollections.IIndexable{`0})" /> and <see cref="M:Mafi.Collections.Stak`1.PushRangeReversed(Mafi.Collections.ReadonlyCollections.IIndexable{`0})" /> for convenient adding of multiple objects.
  /// * Enumerator with condition-less access to Current and without Dispose method to allow optimizations in foreach
  /// cycles.
  /// TODO: Cleanup and document the code, write tests.
  /// </remarks>
  [GenerateSerializer(true, null, 0)]
  public sealed class Stak<T> : IIndexable<T>, ICollectionWithCount, IEnumerable<T>, IEnumerable
  {
    private T[] m_array;
    private int m_count;
    public readonly bool OmitClearing;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private void incVersion()
    {
    }

    /// <summary>
    /// Creates a stack with a specific initial <paramref name="capacity" />. The initial <paramref name="capacity" />
    /// must be a non-negative number. By default an empty stack is created.
    /// </summary>
    public Stak(int capacity = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Mafi.Assert.That<int>(capacity).IsNotNegative();
      this.m_array = capacity <= 0 ? Array.Empty<T>() : new T[capacity];
    }

    /// <param name="omitClearing">
    /// Whether this stack can omit clearing of internal array when <see cref="M:Mafi.Collections.Stak`1.Clear" /> is called or items are
    /// removed from the stack. If this is set to true please keep in mind that any references inserted into the
    /// stack may be referenced until this stack is referenced and prevent them from being garbage collected.
    /// </param>
    /// <param name="capacity">Initial capacity</param>
    public Stak(bool omitClearing, int capacity = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(capacity);
      this.OmitClearing = omitClearing;
    }

    public T this[int index]
    {
      get
      {
        int index1 = this.m_count - 1 - index;
        return (uint) index1 < (uint) this.m_count ? this.m_array[index1] : throw new IndexOutOfRangeException(string.Format("Indexing to stack of size {0} with index {1}.", (object) this.m_count, (object) index));
      }
    }

    public int Count => this.m_count;

    public bool IsEmpty => this.m_count == 0;

    public bool IsNotEmpty => this.m_count > 0;

    /// <summary>Gets or sets capacity of this stack.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// When setting capacity to the smaller value than <see cref="P:Mafi.Collections.Stak`1.Count" />.
    /// </exception>
    public int Capacity
    {
      get => this.m_array.Length;
      set
      {
        if (value < this.m_count)
          throw new ArgumentOutOfRangeException(nameof (Capacity));
        if (value > 0)
          Array.Resize<T>(ref this.m_array, value);
        else
          this.m_array = Array.Empty<T>();
        this.incVersion();
      }
    }

    /// <summary>
    /// Returns the first element of this stack. This is equivalent to <see cref="M:Mafi.Collections.Stak`1.Peek" />.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the stack is empty.</exception>
    public T First
    {
      get
      {
        return this.m_count != 0 ? this.m_array[this.m_count - 1] : throw new InvalidOperationException("Called `First` on an empty Stack.");
      }
    }

    /// <summary>
    /// Returns the last element of the stack (the one that would be popped last).
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the stack is empty.</exception>
    public T Last
    {
      get
      {
        if (this.m_count == 0)
          throw new InvalidOperationException("Called `First` on an empty Stack.");
        return this.m_array[0];
      }
    }

    /// <summary>
    /// Removes all items from this Stack. Capacity remains unchanged.
    /// </summary>
    public void Clear()
    {
      if (this.m_count != 0)
      {
        if (!this.OmitClearing)
          Array.Clear((Array) this.m_array, 0, this.m_count);
        this.m_count = 0;
      }
      this.incVersion();
    }

    [Pure]
    public bool Contains(T item)
    {
      int count = this.m_count;
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      while (count-- > 0)
      {
        if ((object) item == null)
        {
          if ((object) this.m_array[count] == null)
            return true;
        }
        else if ((object) this.m_array[count] != null && equalityComparer.Equals(this.m_array[count], item))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Returns the top object on the stack without removing it.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the stack is empty.</exception>
    [Pure]
    public T Peek()
    {
      return this.m_count != 0 ? this.m_array[this.m_count - 1] : throw new InvalidOperationException("Peek on empty stack.");
    }

    /// <summary>
    /// Pops an item from the top of the stack. If the stack is empty, Pop throws an InvalidOperationException.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">When the stack is empty.</exception>
    public T Pop()
    {
      if (this.m_count == 0)
        throw new InvalidOperationException("Pop on empty stack.");
      this.incVersion();
      --this.m_count;
      T obj = this.m_array[this.m_count];
      this.m_array[this.m_count] = default (T);
      return obj;
    }

    /// <summary>Pushes an item to the top of the stack.</summary>
    public void Push(T item)
    {
      if (this.m_count == this.m_array.Length)
        this.Capacity = this.m_count == 0 ? 4 : 2 * this.m_array.Length;
      this.m_array[this.m_count++] = item;
      this.incVersion();
    }

    /// <summary>
    /// Pushes given values on the stack so that the last element will be on top of the stack. This is semantically
    /// equivalent to call of <see cref="M:Mafi.Collections.Stak`1.Push(`0)" /> on all values in order (first to last), but more effective.
    /// </summary>
    public void PushRange(IIndexable<T> values)
    {
      int count1 = this.m_count;
      int count2 = values.Count;
      this.EnsureCapacity(count1 + count2);
      for (int index = 0; index < count2; ++index)
        this.m_array[count1 + index] = values[index];
      this.m_count += count2;
    }

    /// <summary>
    /// Pushes given values on the stack so that the first element will be on top of the stack. This is semantically
    /// equivalent to call of <see cref="M:Mafi.Collections.Stak`1.Push(`0)" /> on all values in reversed order (last to first), but more effective.
    /// </summary>
    public void PushRangeReversed(IIndexable<T> values)
    {
      int count1 = this.m_count;
      int count2 = values.Count;
      this.EnsureCapacity(count1 + count2);
      for (int index = 0; index < count2; ++index)
        this.m_array[count1 + index] = values[count2 - index - 1];
      this.m_count += count2;
    }

    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.m_count)
        throw new ArgumentOutOfRangeException(nameof (index));
      int destinationIndex = this.m_count - 1 - index;
      --this.m_count;
      if (destinationIndex < this.m_count)
        Array.Copy((Array) this.m_array, destinationIndex + 1, (Array) this.m_array, destinationIndex, this.m_count - destinationIndex);
      this.m_array[this.m_count] = default (T);
      this.incVersion();
    }

    /// <summary>Copies the stack into an array.</summary>
    public void CopyTo(T[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0 || arrayIndex > array.Length)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), (object) arrayIndex, "");
      if (array.Length - arrayIndex < this.m_count)
        throw new ArgumentException(nameof (arrayIndex));
      int num1 = 0;
      int num2 = arrayIndex + this.m_count;
      for (int index = 0; index < this.m_count; ++index)
        array[--num2] = this.m_array[num1++];
    }

    public void EnsureCapacity(int minCapacity)
    {
      if (minCapacity <= this.m_array.Length)
        return;
      int num = this.m_array.Length == 0 ? 4 : this.m_array.Length * 2;
      if ((uint) num > 2146435071U)
        num = 2146435071;
      if (num < minCapacity)
        num = minCapacity;
      this.Capacity = num;
    }

    public void TrimExcess()
    {
      if (this.m_count == this.m_array.Length)
        return;
      Array.Resize<T>(ref this.m_array, this.m_count);
      this.incVersion();
    }

    /// <summary>
    /// Copies the Stack to an array, in the same order Pop would return the items.
    /// </summary>
    [Pure]
    public T[] ToArray()
    {
      if (this.m_count == 0)
        return Array.Empty<T>();
      T[] array = new T[this.m_count];
      for (int index = 0; index < this.m_count; ++index)
        array[index] = this.m_array[this.m_count - index - 1];
      return array;
    }

    [Pure]
    [DebuggerStepThrough]
    public Stak<T>.Enumerator GetEnumerator() => new Stak<T>.Enumerator(this);

    [DebuggerStepThrough]
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Stak<T>.EnumeratorClass(this);

    [DebuggerStepThrough]
    [Pure]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return (IEnumerator<T>) new Stak<T>.EnumeratorClass(this);
    }

    [Pure]
    [DebuggerStepThrough]
    IndexableEnumerator<T> IIndexable<T>.GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }

    [DebuggerStepThrough]
    [Pure]
    IEnumerable<T> IIndexable<T>.AsEnumerable() => (IEnumerable<T>) this;

    public override string ToString()
    {
      return string.Format("Count: {0}, capacity: {1}", (object) this.Count, (object) this.Capacity);
    }

    public void SerializeData(BlobWriter writer)
    {
      writer.WriteIntNotNegative(this.m_count);
      writer.WriteBool(this.OmitClearing);
      writer.WriteArray<T>(this.m_array, this.m_count);
    }

    public void DeserializeData(BlobReader reader)
    {
      this.m_count = reader.ReadIntNotNegative();
      reader.SetField<Stak<T>>(this, "OmitClearing", (object) reader.ReadBool());
      this.m_array = reader.ReadArray<T>(this.m_count);
    }

    public static void Serialize(Stak<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Stak<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Stak<T>.s_serializeDataDelayedAction);
    }

    public static Stak<T> Deserialize(BlobReader reader)
    {
      Stak<T> stak;
      if (reader.TryStartClassDeserialization<Stak<T>>(out stak))
        reader.EnqueueDataDeserialization((object) stak, Stak<T>.s_deserializeDataDelayedAction);
      return stak;
    }

    static Stak()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Stak<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Stak<T>) obj).SerializeData(writer));
      Stak<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Stak<T>) obj).DeserializeData(reader));
    }

    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly Stak<T> m_stack;
      private int m_index;

      internal Enumerator(Stak<T> stack)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_stack = stack;
        this.m_index = -1;
      }

      public bool MoveNext() => ++this.m_index < this.m_stack.m_count;

      public T Current => this.m_stack[this.m_index];
    }

    [DebuggerStepThrough]
    private class EnumeratorClass : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly Stak<T> m_stack;
      private int m_index;

      internal EnumeratorClass(Stak<T> stack)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_index = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_stack = stack;
      }

      public bool MoveNext() => ++this.m_index < this.m_stack.m_count;

      public T Current => this.m_stack[this.m_index];

      object IEnumerator.Current => (object) this.Current;

      public void Reset() => this.m_index = -1;

      public void Dispose()
      {
      }
    }
  }
}
