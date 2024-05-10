// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Heap`1
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
  [GenerateSerializer(true, null, 0)]
  public sealed class Heap<T> : 
    IIndexable<KeyValuePair<Fix32, T>>,
    ICollectionWithCount,
    IEnumerable<KeyValuePair<Fix32, T>>,
    IEnumerable,
    IReadOnlyCollection<KeyValuePair<Fix32, T>>
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_ElementsOmittedClearing;
    /// <summary>Array that contains heap.</summary>
    private KeyValuePair<Fix32, T>[] m_heap;
    private readonly bool m_canOmitClearing;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Number of nodes in this queue.</summary>
    public int Count { get; private set; }

    public bool IsEmpty => this.Count == 0;

    public bool IsNotEmpty => this.Count > 0;

    private static int leftChildIndex(int i) => i * 2 + 1;

    private static int rightChildIndex(int i) => i * 2 + 2;

    private static int parentIndex(int i) => i - 1 >> 1;

    public Heap(bool canOmitClearing = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_heap = Array.Empty<KeyValuePair<Fix32, T>>();
      this.m_canOmitClearing = canOmitClearing;
    }

    public Heap(int initialSize, bool canOmitClearing = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_heap = new KeyValuePair<Fix32, T>[initialSize];
      this.m_canOmitClearing = canOmitClearing;
    }

    public KeyValuePair<Fix32, T> this[int index]
    {
      get
      {
        return (uint) index < (uint) this.Count ? this.m_heap[index] : throw new IndexOutOfRangeException(string.Format("Indexing heap with {0} elements with index {1}.", (object) this.Count, (object) index));
      }
      private set => this.m_heap[index] = value;
    }

    public void Push(Fix32 key, T value)
    {
      if (this.Count + 1 >= this.m_heap.Length)
      {
        Mafi.Assert.That<int>(this.m_heap.Length * 2).IsLess(1073741823, "Crazy big heap?");
        Array.Resize<KeyValuePair<Fix32, T>>(ref this.m_heap, Math.Max(this.m_heap.Length * 2, 32));
      }
      this.m_heap[this.Count] = new KeyValuePair<Fix32, T>(key, value);
      this.heapifyUp(this.Count);
      ++this.Count;
    }

    public bool Contains(T value)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (this.m_heap[index].Value.Equals((object) value))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Removes the highest lowest keyed instance of value. Returns true if successful. O(n).
    /// </summary>
    public bool TryRemove(T value)
    {
      if (this.Count <= 0)
      {
        Mafi.Log.Error("Removing from an empty heap.");
        return false;
      }
      int nodeIndex = 0;
      while (nodeIndex < this.Count && !this.m_heap[nodeIndex].Value.Equals((object) value))
        ++nodeIndex;
      if (nodeIndex == this.Count)
        return false;
      --this.Count;
      if (this.Count > 0)
      {
        bool flag = this.m_heap[this.Count].Key < this.m_heap[nodeIndex].Key;
        this.m_heap[nodeIndex] = this.m_heap[this.Count];
        if (flag)
          this.heapifyUp(nodeIndex);
        else
          this.heapifyDown(nodeIndex);
      }
      this.m_heap[this.Count] = new KeyValuePair<Fix32, T>();
      return true;
    }

    public Fix32 PeekMinKey()
    {
      if (this.Count > 0)
        return this.m_heap[0].Key;
      Mafi.Log.Error("Peeking in an empty heap.");
      return new Fix32();
    }

    public KeyValuePair<Fix32, T> PeekMin()
    {
      if (this.Count > 0)
        return this.m_heap[0];
      Mafi.Log.Error("Peeking in an empty heap.");
      return new KeyValuePair<Fix32, T>();
    }

    public KeyValuePair<Fix32, T> PopMin()
    {
      if (this.Count <= 0)
      {
        Mafi.Log.Error("Removing from an empty heap.");
        return new KeyValuePair<Fix32, T>();
      }
      --this.Count;
      KeyValuePair<Fix32, T> keyValuePair = this.m_heap[0];
      if (this.Count > 0)
      {
        this.m_heap[0] = this.m_heap[this.Count];
        this.heapifyDown(0);
      }
      this.m_heap[this.Count] = new KeyValuePair<Fix32, T>();
      return keyValuePair;
    }

    public T PopMin(out Fix32 cost)
    {
      if (this.Count <= 0)
      {
        Mafi.Log.Error("Removing from an empty heap.");
        cost = new Fix32();
        return default (T);
      }
      --this.Count;
      KeyValuePair<Fix32, T> keyValuePair = this.m_heap[0];
      if (this.Count > 0)
      {
        this.m_heap[0] = this.m_heap[this.Count];
        this.heapifyDown(0);
      }
      this.m_heap[this.Count] = new KeyValuePair<Fix32, T>();
      cost = keyValuePair.Key;
      return keyValuePair.Value;
    }

    /// <summary>Clears the contents of Heap.</summary>
    public void Clear()
    {
      if (this.Count == 0)
        return;
      if (!this.m_canOmitClearing)
        Array.Clear((Array) this.m_heap, 0, this.Count);
      this.Count = 0;
    }

    public void EnsureCapacity(int minCapacity)
    {
      Mafi.Assert.That<int>(minCapacity).IsNotNegative();
      if (minCapacity <= 0)
        return;
      int newSize = 32;
      while (newSize <= minCapacity)
        newSize *= 2;
      if (this.m_heap.Length >= newSize)
        return;
      Array.Resize<KeyValuePair<Fix32, T>>(ref this.m_heap, newSize);
    }

    /// <summary>Returns currently stored elements as an array slice.</summary>
    public ReadOnlyArraySlice<KeyValuePair<Fix32, T>> GetElements()
    {
      return this.m_heap.Length != 0 ? new ReadOnlyArraySlice<KeyValuePair<Fix32, T>>(this.m_heap, 0, this.Count) : ReadOnlyArraySlice<KeyValuePair<Fix32, T>>.Empty;
    }

    /// <summary>
    /// Whether this heap is in valid state and all nodes satisfy the heap condition.
    /// </summary>
    public bool IsInValidState()
    {
      if (this.m_heap.Length == 0)
        return this.Count == 0;
      for (int i = 0; i < this.m_heap.Length; ++i)
      {
        Fix32 key = this.m_heap[i].Key;
        if (i >= this.Count)
        {
          if (!this.m_canOmitClearing && key != new Fix32())
            return false;
        }
        else if (Heap<T>.leftChildIndex(i) < this.Count && (this.m_heap[Heap<T>.leftChildIndex(i)].Key < key || Heap<T>.rightChildIndex(i) < this.Count && this.m_heap[Heap<T>.rightChildIndex(i)].Key < key))
          return false;
      }
      return true;
    }

    private void heapifyUp(int nodeIndex)
    {
      KeyValuePair<Fix32, T> keyValuePair1 = this.m_heap[nodeIndex];
      for (int index = Heap<T>.parentIndex(nodeIndex); index >= 0; index = Heap<T>.parentIndex(nodeIndex))
      {
        KeyValuePair<Fix32, T> keyValuePair2 = this.m_heap[index];
        if (keyValuePair1.Key >= keyValuePair2.Key)
          break;
        this.m_heap[index] = keyValuePair1;
        this.m_heap[nodeIndex] = keyValuePair2;
        nodeIndex = index;
      }
    }

    private void heapifyDown(int nodeIndex)
    {
      KeyValuePair<Fix32, T> keyValuePair1 = this.m_heap[nodeIndex];
      while (true)
      {
        bool flag = true;
        KeyValuePair<Fix32, T> keyValuePair2 = keyValuePair1;
        int index1 = nodeIndex;
        int index2 = Heap<T>.leftChildIndex(nodeIndex);
        if (index2 <= this.Count)
        {
          KeyValuePair<Fix32, T> keyValuePair3 = this.m_heap[index2];
          if (keyValuePair3.Key < keyValuePair2.Key)
          {
            keyValuePair2 = keyValuePair3;
            index1 = index2;
            flag = false;
          }
          int index3 = index2 + 1;
          if (index3 <= this.Count)
          {
            KeyValuePair<Fix32, T> keyValuePair4 = this.m_heap[index3];
            if (keyValuePair4.Key < keyValuePair2.Key)
            {
              keyValuePair2 = keyValuePair4;
              index1 = index3;
              flag = false;
            }
          }
          if (!flag)
          {
            this.m_heap[nodeIndex] = keyValuePair2;
            this.m_heap[index1] = keyValuePair1;
            nodeIndex = index1;
          }
          else
            goto label_10;
        }
        else
          break;
      }
      return;
label_10:;
    }

    public void UpdateAllPriorities(Func<T, Fix32> priorityFn)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        KeyValuePair<Fix32, T> keyValuePair = this.m_heap[index];
        this.m_heap[index] = new KeyValuePair<Fix32, T>(priorityFn(keyValuePair.Value), keyValuePair.Value);
      }
      for (int nodeIndex = 0; nodeIndex < this.Count; ++nodeIndex)
        this.heapifyUp(nodeIndex);
    }

    [DebuggerStepThrough]
    [Pure]
    public Heap<T>.Enumerator GetEnumerator() => new Heap<T>.Enumerator(this);

    [DebuggerStepThrough]
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Heap<T>.EnumeratorClass(this);

    [Pure]
    [DebuggerStepThrough]
    IEnumerator<KeyValuePair<Fix32, T>> IEnumerable<KeyValuePair<Fix32, T>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<Fix32, T>>) new Heap<T>.EnumeratorClass(this);
    }

    [DebuggerStepThrough]
    [Pure]
    IndexableEnumerator<KeyValuePair<Fix32, T>> IIndexable<KeyValuePair<Fix32, T>>.GetEnumerator()
    {
      return new IndexableEnumerator<KeyValuePair<Fix32, T>>((IIndexable<KeyValuePair<Fix32, T>>) this);
    }

    [Pure]
    [DebuggerStepThrough]
    IEnumerable<KeyValuePair<Fix32, T>> IIndexable<KeyValuePair<Fix32, T>>.AsEnumerable()
    {
      return (IEnumerable<KeyValuePair<Fix32, T>>) this;
    }

    public void SerializeData(BlobWriter writer)
    {
      writer.WriteIntNotNegative(this.Count);
      writer.WriteBool(this.m_canOmitClearing);
      writer.WriteArray<KeyValuePair<Fix32, T>>(this.m_heap, this.Count);
    }

    public void DeserializeData(BlobReader reader)
    {
      this.Count = reader.ReadIntNotNegative();
      reader.SetField<Heap<T>>(this, "m_canOmitClearing", (object) reader.ReadBool());
      this.m_heap = reader.ReadArray<KeyValuePair<Fix32, T>>(this.Count);
    }

    public override string ToString()
    {
      return string.Format("Count: {0}, min: {1}, capacity {2}", (object) this.Count, this.Count > 0 ? (object) this.m_heap[1] : (object) "n/a", (object) (this.m_heap.Length - 1));
    }

    public static void Serialize(Heap<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Heap<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Heap<T>.s_serializeDataDelayedAction);
    }

    public static Heap<T> Deserialize(BlobReader reader)
    {
      Heap<T> heap;
      if (reader.TryStartClassDeserialization<Heap<T>>(out heap))
        reader.EnqueueDataDeserialization((object) heap, Heap<T>.s_deserializeDataDelayedAction);
      return heap;
    }

    static Heap()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Heap<T>.s_pcKey = "Heap<" + typeof (T).Name + ">: ";
      Heap<T>.s_pcKey_ElementsOmittedClearing = Heap<T>.s_pcKey + "ElementsOmittedClearing";
      Heap<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Heap<T>) obj).SerializeData(writer));
      Heap<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Heap<T>) obj).DeserializeData(reader));
    }

    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly Heap<T> m_heap;
      private int m_index;

      internal Enumerator(Heap<T> heap)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_heap = heap;
        this.m_index = -1;
      }

      public bool MoveNext() => ++this.m_index < this.m_heap.Count;

      public KeyValuePair<Fix32, T> Current => this.m_heap[this.m_index];
    }

    [DebuggerStepThrough]
    private class EnumeratorClass : IEnumerator<KeyValuePair<Fix32, T>>, IDisposable, IEnumerator
    {
      private readonly Heap<T> m_heap;
      private int m_index;

      internal EnumeratorClass(Heap<T> heap)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_index = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_heap = heap;
      }

      public bool MoveNext() => ++this.m_index < this.m_heap.Count;

      public KeyValuePair<Fix32, T> Current => this.m_heap[this.m_index];

      object IEnumerator.Current => (object) this.m_heap[this.m_index];

      public void Reset() => this.m_index = -1;

      public void Dispose()
      {
      }
    }
  }
}
