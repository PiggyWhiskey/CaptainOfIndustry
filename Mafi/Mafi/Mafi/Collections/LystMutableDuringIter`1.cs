// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.LystMutableDuringIter`1
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

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// A simple dynamic list that can be mutated during iteration. New members added during iteration will not be
  /// iterated over and removed items that were not yet processed will be skipped.
  /// </summary>
  [GenerateSerializer(true, null, 0)]
  public class LystMutableDuringIter<T> : 
    IEnumerable<T>,
    IEnumerable,
    IIndexable<T>,
    ICollectionWithCount,
    IReadOnlyCollection<T>
  {
    [ThreadStatic]
    private static ObjectPool2<LystMutableDuringIter<T>.EnumeratorClass> s_enumeratorsPool;
    private T[] m_items;
    private int m_size;
    [DoNotSave(0, null)]
    private Option<LystMutableDuringIter<T>.EnumeratorClass> m_activeEnumerator;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Count => this.m_size;

    public LystMutableDuringIter(int capacity = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(capacity).IsNotNegative();
      this.m_items = capacity > 0 ? new T[capacity] : Array.Empty<T>();
      this.m_size = 0;
    }

    public LystMutableDuringIter(IEnumerable<T> values)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector();
      if (values is IReadOnlyCollection<T> objs)
        this.extendCapacity(objs.Count);
      foreach (T obj in values)
        this.Add(obj);
    }

    /// <summary>
    /// Gets or sets an element at given index. Ongoing enumeration will see results of setting a new element.
    /// </summary>
    [DoNotSave(0, null)]
    public T this[int index]
    {
      get
      {
        return (uint) index < (uint) this.m_size ? this.m_items[index] : throw new ArgumentOutOfRangeException(nameof (index));
      }
      set
      {
        if ((uint) index >= (uint) this.m_size)
          throw new ArgumentOutOfRangeException(nameof (index));
        this.m_items[index] = value;
      }
    }

    [Pure]
    public int IndexOf(T item) => Array.IndexOf<T>(this.m_items, item, 0, this.m_size);

    public void Add(T item)
    {
      if (this.m_size == this.m_items.Length)
        this.extendCapacity(this.m_size + 1);
      this.m_items[this.m_size] = item;
      ++this.m_size;
    }

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
      Assert.That<bool>(this.Remove(item)).IsTrue(string.Format("Failed to remove from list: {0}", (object) item));
    }

    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.m_size)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.m_size;
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index + 1, (Array) this.m_items, index, this.m_size - index);
      this.m_items[this.m_size] = default (T);
      if (!this.m_activeEnumerator.HasValue)
        return;
      this.m_activeEnumerator.Value.NotifyElementDeletedAt(index);
    }

    [Pure]
    public int FindIndex(Predicate<T> match)
    {
      if (match == null)
      {
        Log.Error("FindIndex: Null match function.");
        return -1;
      }
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
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
        Log.Error("FindIndex: Null match function.");
        return -1;
      }
      for (int index = startIndex; index < num; ++index)
      {
        if (match(this.m_items[index]))
          return index;
      }
      return -1;
    }

    /// <summary>Clears the contents of List.</summary>
    public void Clear()
    {
      if (this.m_size == 0)
        return;
      Array.Clear((Array) this.m_items, 0, this.m_size);
      this.m_size = 0;
    }

    private void extendCapacity(int minCapacity)
    {
      int newSize = this.m_items.Length == 0 ? 4 : this.m_items.Length * 2;
      if ((uint) newSize > 2146435071U)
      {
        Assert.Fail("Lyst was just allocated to max size, is that supposed to happen?.");
        newSize = 2146435071;
      }
      if (newSize < minCapacity)
        newSize = minCapacity;
      Array.Resize<T>(ref this.m_items, newSize);
    }

    public IEnumerator<T> GetEnumerator()
    {
      if (LystMutableDuringIter<T>.s_enumeratorsPool == null)
        LystMutableDuringIter<T>.s_enumeratorsPool = new ObjectPool2<LystMutableDuringIter<T>.EnumeratorClass>(16, (Func<ObjectPool2<LystMutableDuringIter<T>.EnumeratorClass>, LystMutableDuringIter<T>.EnumeratorClass>) (pool => new LystMutableDuringIter<T>.EnumeratorClass()));
      LystMutableDuringIter<T>.EnumeratorClass instance = LystMutableDuringIter<T>.s_enumeratorsPool.GetInstance();
      instance.Initialize(this);
      return (IEnumerator<T>) instance;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IndexableEnumerator<T> IIndexable<T>.GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }

    public IEnumerable<T> AsEnumerable() => (IEnumerable<T>) this;

    protected void SerializeData(BlobWriter writer)
    {
      writer.WriteIntNotNegative(this.m_size);
      writer.WriteArray<T>(this.m_items, this.m_size);
    }

    protected void DeserializeData(BlobReader reader)
    {
      this.m_size = reader.ReadIntNotNegative();
      this.m_items = reader.ReadArray<T>(this.m_size);
    }

    public static void Serialize(LystMutableDuringIter<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LystMutableDuringIter<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LystMutableDuringIter<T>.s_serializeDataDelayedAction);
    }

    public static LystMutableDuringIter<T> Deserialize(BlobReader reader)
    {
      LystMutableDuringIter<T> mutableDuringIter;
      if (reader.TryStartClassDeserialization<LystMutableDuringIter<T>>(out mutableDuringIter))
        reader.EnqueueDataDeserialization((object) mutableDuringIter, LystMutableDuringIter<T>.s_deserializeDataDelayedAction);
      return mutableDuringIter;
    }

    static LystMutableDuringIter()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LystMutableDuringIter<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LystMutableDuringIter<T>) obj).SerializeData(writer));
      LystMutableDuringIter<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LystMutableDuringIter<T>) obj).DeserializeData(reader));
    }

    private class EnumeratorClass : IEnumerator<T>, IDisposable, IEnumerator
    {
      private LystMutableDuringIter<T> m_lyst;
      private int m_index;
      private int m_maxIndex;
      private Option<LystMutableDuringIter<T>.EnumeratorClass> m_previousEnumerator;

      internal void Initialize(LystMutableDuringIter<T> lyst)
      {
        Assert.That<Option<LystMutableDuringIter<T>.EnumeratorClass>>(lyst.m_activeEnumerator).IsNotEqualTo<LystMutableDuringIter<T>.EnumeratorClass>(this);
        Assert.That<LystMutableDuringIter<T>>(this.m_lyst).IsNull<LystMutableDuringIter<T>>("Pooling issue?");
        this.m_lyst = lyst;
        this.m_index = -1;
        this.m_maxIndex = lyst.Count;
        this.m_previousEnumerator = lyst.m_activeEnumerator;
        lyst.m_activeEnumerator = (Option<LystMutableDuringIter<T>.EnumeratorClass>) this;
      }

      public bool MoveNext() => ++this.m_index < this.m_maxIndex;

      public T Current => this.m_lyst.m_items[this.m_index];

      object IEnumerator.Current => (object) this.m_lyst.m_items[this.m_index];

      public void Reset() => this.m_index = -1;

      public void Dispose()
      {
        Assert.That<Option<LystMutableDuringIter<T>.EnumeratorClass>>(this.m_lyst.m_activeEnumerator).IsEqualTo<LystMutableDuringIter<T>.EnumeratorClass>(this, "Outer enumerator finished first?");
        this.m_lyst.m_activeEnumerator = this.m_previousEnumerator;
        this.m_previousEnumerator = Option<LystMutableDuringIter<T>.EnumeratorClass>.None;
        this.m_lyst = (LystMutableDuringIter<T>) null;
        if (LystMutableDuringIter<T>.s_enumeratorsPool != null)
          LystMutableDuringIter<T>.s_enumeratorsPool.ReturnInstanceKeepReference(this);
        else
          Assert.Fail("Disposing from different thread than created?");
      }

      internal void NotifyElementDeletedAt(int index)
      {
        --this.m_maxIndex;
        if (index <= this.m_index)
          --this.m_index;
        if (!this.m_previousEnumerator.HasValue)
          return;
        this.m_previousEnumerator.Value.NotifyElementDeletedAt(index);
      }

      internal int GetNumberOfNestedEnumerators()
      {
        int nestedEnumerators = 1;
        LystMutableDuringIter<T>.EnumeratorClass enumeratorClass = this;
        while (enumeratorClass != enumeratorClass.m_previousEnumerator.ValueOrNull && (enumeratorClass = enumeratorClass.m_previousEnumerator.ValueOrNull) != null)
          ++nestedEnumerators;
        return nestedEnumerators;
      }

      public EnumeratorClass()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
