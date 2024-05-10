// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Dict`2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Key-value hash-based storage.
  /// 
  /// Taken and adopted from MIT-licensed C# reference source.
  /// 
  /// Changes:
  /// * Added a custom serializer.
  /// * Asserts instead of some Exceptions and Debug/Contract calls.
  /// * Removed support for a custom comparer since we have no good way to save it for now.
  /// * Removed not useful interfaces: IDictionary (non-generic - trash), IDictionary{TKey,TValue} (this one requires
  /// mutable `.Keys` and `.Values` collections which is nonsense), ISerializable, and IDeserializationCallback.
  /// * Removed not useful interfaces from enumerators: IDictionaryEnumerator
  /// * Removed not useful interfaces from Keys and Values collections: ICollection{TValue}, ICollection.
  /// </summary>
  [GenerateSerializer(true, null, 0)]
  [DebuggerDisplay("Count = {Count}")]
  public sealed class Dict<TKey, TValue> : 
    IReadOnlyDictionary<TKey, TValue>,
    IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IDictNonGeneric,
    ICollectionWithCount
  {
    private int[] m_buckets;
    private Dict<TKey, TValue>.Entry[] m_entries;
    private int m_count;
    private int m_version;
    private int m_freeList;
    private int m_freeCount;
    private Dict<TKey, TValue>.KeyCollection m_keys;
    private Dict<TKey, TValue>.ValueCollection m_values;
    private readonly IEqualityComparer<TKey> m_comparer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Count => this.m_count - this.m_freeCount;

    public bool IsEmpty => this.Count <= 0;

    public bool IsNotEmpty => this.Count > 0;

    public int Capacity
    {
      get
      {
        int[] buckets = this.m_buckets;
        return buckets == null ? 0 : buckets.Length;
      }
    }

    public IEqualityComparer<TKey> Comparer => this.m_comparer;

    public bool HasDefaultComparer => this.m_comparer == EqualityComparer<TKey>.Default;

    public Dict<TKey, TValue>.KeyCollection Keys
    {
      get => this.m_keys ?? (this.m_keys = new Dict<TKey, TValue>.KeyCollection(this));
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => (IEnumerable<TKey>) this.Keys;

    public Dict<TKey, TValue>.ValueCollection Values
    {
      get => this.m_values ?? (this.m_values = new Dict<TKey, TValue>.ValueCollection(this));
    }

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
    {
      get => (IEnumerable<TValue>) this.Values;
    }

    /// <summary>Ctor for loading.</summary>
    public Dict()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_comparer = (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
    }

    public Dict(IEqualityComparer<TKey> comparer)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_comparer = comparer.CheckNotNull<IEqualityComparer<TKey>>();
    }

    public Dict(int capacity, IEqualityComparer<TKey> comparer = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Mafi.Assert.That<int>(capacity).IsNotNegative();
      if (capacity > 0)
        this.initialize(capacity);
      this.m_comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
    }

    public Dict(
      IReadOnlyCollection<KeyValuePair<TKey, TValue>> collection)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(collection.Count);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) collection)
        this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    /// <summary>
    /// Returns a new cloned copy of this dictionary (except the comparer, that one is kept as the same instance).
    /// </summary>
    public Dict<TKey, TValue> DeepClone()
    {
      Dict<TKey, TValue> dict = new Dict<TKey, TValue>(this.m_comparer)
      {
        m_buckets = new int[this.m_buckets.Length],
        m_entries = new Dict<TKey, TValue>.Entry[this.m_entries.Length]
      };
      Array.Copy((Array) this.m_buckets, (Array) dict.m_buckets, this.m_buckets.Length);
      Array.Copy((Array) this.m_entries, (Array) dict.m_entries, this.m_entries.Length);
      dict.m_count = this.m_count;
      dict.m_freeList = this.m_freeList;
      dict.m_freeCount = this.m_freeCount;
      return dict;
    }

    public TValue this[TKey key]
    {
      get
      {
        int entry = this.findEntry(key);
        if (entry >= 0)
          return this.m_entries[entry].value;
        throw new KeyNotFoundException(string.Format("Key {0} was not found.", (object) key));
      }
      set => this.insert(key, value, false, out bool _, out int _);
    }

    object IDictNonGeneric.this[object key]
    {
      get => (object) this[(TKey) key];
      set => this[(TKey) key] = (TValue) value;
    }

    public void Add(TKey key, TValue value)
    {
      if (!this.insert(key, value, true, out bool _, out int _))
        throw new ArgumentException(string.Format("Adding already existing element key={0}, value={1}.", (object) key, (object) value));
    }

    public bool TryAdd(TKey key, TValue value)
    {
      return this.insert(key, value, true, out bool _, out int _);
    }

    public void AddAndAssertNew(TKey key, TValue value, string assertionMessage = "")
    {
      bool valueOverridden;
      Mafi.Assert.That<bool>(this.insert(key, value, false, out valueOverridden, out int _)).IsTrue();
      Mafi.Assert.That<bool>(valueOverridden).IsFalse<TKey, string>("Dict AddAndAssertNew: Duplicate key '{0}' found, value was overriden. {1}", key, assertionMessage);
    }

    public void AddIfNotPresent(TKey key, TValue value)
    {
      this.insert(key, value, true, out bool _, out int _);
    }

    void IDictNonGeneric.Add(object key, object value) => this.Add((TKey) key, (TValue) value);

    public void Clear()
    {
      if (this.m_count <= 0)
        return;
      for (int index = 0; index < this.m_buckets.Length; ++index)
        this.m_buckets[index] = -1;
      Array.Clear((Array) this.m_entries, 0, this.m_count);
      this.m_freeList = -1;
      this.m_count = 0;
      this.m_freeCount = 0;
      ++this.m_version;
    }

    public bool ContainsKey(TKey key) => this.findEntry(key) >= 0;

    public bool ContainsValue(TValue value)
    {
      if ((object) value == null)
      {
        for (int index = 0; index < this.m_count; ++index)
        {
          if (this.m_entries[index].hashCode >= 0 && (object) this.m_entries[index].value == null)
            return true;
        }
      }
      else
      {
        EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
        for (int index = 0; index < this.m_count; ++index)
        {
          if (this.m_entries[index].hashCode >= 0 && equalityComparer.Equals(this.m_entries[index].value, value))
            return true;
        }
      }
      return false;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
    {
      Mafi.Assert.That<KeyValuePair<TKey, TValue>[]>(array).IsNotNull<KeyValuePair<TKey, TValue>[]>();
      if (index < 0 || index > array.Length)
        throw new ArgumentException(nameof (index));
      if (array.Length - index < this.Count)
        throw new ArgumentException("Array is too small", nameof (array));
      int count = this.m_count;
      Dict<TKey, TValue>.Entry[] entries = this.m_entries;
      for (int index1 = 0; index1 < count; ++index1)
      {
        if (entries[index1].hashCode >= 0)
          array[index++] = new KeyValuePair<TKey, TValue>(entries[index1].key, entries[index1].value);
      }
    }

    public Dict<TKey, TValue>.Enumerator GetEnumerator() => new Dict<TKey, TValue>.Enumerator(this);

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<TKey, TValue>>) new Dict<TKey, TValue>.Enumerator(this);
    }

    IEnumerator<KeyValuePair<object, object>> IDictNonGeneric.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<object, object>>) new Dict<TKey, TValue>.EnumeratorNonGeneric(this);
    }

    private int findEntry(TKey key)
    {
      if (this.m_buckets == null)
      {
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNull<Dict<TKey, TValue>.Entry[]>("Accessing a loaded dict that was not initialized yet.");
        return -1;
      }
      int num = this.m_comparer.GetHashCode(key) & int.MaxValue;
      for (int entry = this.m_buckets[num % this.m_buckets.Length]; entry >= 0; entry = this.m_entries[entry].next)
      {
        if (this.m_entries[entry].hashCode == num && this.m_comparer.Equals(this.m_entries[entry].key, key))
          return entry;
      }
      return -1;
    }

    private void initialize(int capacity)
    {
      int prime = HashHelpers.GetPrime(capacity);
      this.m_buckets = new int[prime];
      for (int index = 0; index < this.m_buckets.Length; ++index)
        this.m_buckets[index] = -1;
      this.m_entries = new Dict<TKey, TValue>.Entry[prime];
      this.m_freeList = -1;
    }

    private bool insert(
      TKey key,
      TValue value,
      bool skipIfExists,
      out bool valueOverridden,
      out int insertedIndex)
    {
      if (this.m_buckets == null)
      {
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNull<Dict<TKey, TValue>.Entry[]>("Inserting to a loaded dict that was not initialized yet.");
        this.initialize(0);
      }
      int num = this.m_comparer.GetHashCode(key) & int.MaxValue;
      int index1 = num % this.m_buckets.Length;
      for (int index2 = this.m_buckets[index1]; index2 >= 0; index2 = this.m_entries[index2].next)
      {
        if (this.m_entries[index2].hashCode == num && this.m_comparer.Equals(this.m_entries[index2].key, key))
        {
          if (skipIfExists)
          {
            valueOverridden = false;
            insertedIndex = index2;
            return false;
          }
          this.m_entries[index2].value = value;
          ++this.m_version;
          valueOverridden = true;
          insertedIndex = index2;
          return true;
        }
      }
      int index3;
      if (this.m_freeCount > 0)
      {
        index3 = this.m_freeList;
        this.m_freeList = this.m_entries[index3].next;
        --this.m_freeCount;
      }
      else
      {
        if (this.m_count == this.m_entries.Length)
        {
          this.resize();
          index1 = num % this.m_buckets.Length;
        }
        index3 = this.m_count;
        ++this.m_count;
      }
      this.m_entries[index3].hashCode = num;
      this.m_entries[index3].next = this.m_buckets[index1];
      this.m_entries[index3].key = key;
      this.m_entries[index3].value = value;
      this.m_buckets[index1] = index3;
      ++this.m_version;
      insertedIndex = index3;
      valueOverridden = false;
      return true;
    }

    public void EnsureCapacity(int capacity)
    {
      Mafi.Assert.That<int>(capacity).IsPositive();
      if (this.m_buckets == null)
      {
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNull<Dict<TKey, TValue>.Entry[]>("Ensuring capacity of a loaded dict that was not initialized yet.");
        this.initialize(capacity);
      }
      if (capacity <= this.Capacity)
        return;
      this.resize(HashHelpers.GetPrime(capacity), false);
    }

    private void resize() => this.resize(HashHelpers.ExpandPrime(this.m_count), false);

    private void resize(int newSize, bool forceNewHashCodes)
    {
      Mafi.Assert.That<int>(newSize).IsGreaterOrEqual(this.m_entries.Length);
      int[] numArray = new int[newSize];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = -1;
      Dict<TKey, TValue>.Entry[] destinationArray = new Dict<TKey, TValue>.Entry[newSize];
      Array.Copy((Array) this.m_entries, 0, (Array) destinationArray, 0, this.m_count);
      if (forceNewHashCodes)
      {
        for (int index = 0; index < this.m_count; ++index)
        {
          if (destinationArray[index].hashCode != -1)
            destinationArray[index].hashCode = this.m_comparer.GetHashCode(destinationArray[index].key) & int.MaxValue;
        }
      }
      for (int index1 = 0; index1 < this.m_count; ++index1)
      {
        if (destinationArray[index1].hashCode >= 0)
        {
          int index2 = destinationArray[index1].hashCode % newSize;
          destinationArray[index1].next = numArray[index2];
          numArray[index2] = index1;
        }
      }
      this.m_buckets = numArray;
      this.m_entries = destinationArray;
    }

    public bool Remove(TKey key) => this.TryRemove(key, out TValue _);

    public void RemoveAndAssert(TKey key, string assertionMessage = "")
    {
      Mafi.Assert.That<bool>(this.TryRemove(key, out TValue _)).IsTrue<string>("Failed to remove value from dict. {0}", assertionMessage);
    }

    public bool TryRemove(TKey key, out TValue value)
    {
      if (this.m_buckets == null)
      {
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNull<Dict<TKey, TValue>.Entry[]>("Removing from a loaded dict that was not initialized yet.");
        value = default (TValue);
        return false;
      }
      int num = this.m_comparer.GetHashCode(key) & int.MaxValue;
      int index1 = num % this.m_buckets.Length;
      int index2 = -1;
      for (int index3 = this.m_buckets[index1]; index3 >= 0; index3 = this.m_entries[index3].next)
      {
        if (this.m_entries[index3].hashCode == num && this.m_comparer.Equals(this.m_entries[index3].key, key))
        {
          if (index2 < 0)
            this.m_buckets[index1] = this.m_entries[index3].next;
          else
            this.m_entries[index2].next = this.m_entries[index3].next;
          this.m_entries[index3].hashCode = -1;
          this.m_entries[index3].next = this.m_freeList;
          this.m_entries[index3].key = default (TKey);
          value = this.m_entries[index3].value;
          this.m_entries[index3].value = default (TValue);
          this.m_freeList = index3;
          ++this.m_freeCount;
          ++this.m_version;
          return true;
        }
        index2 = index3;
      }
      value = default (TValue);
      return false;
    }

    public int RemoveValues(
      Predicate<TValue> predicate,
      Action<KeyValuePair<TKey, TValue>> removedValueCallback = null)
    {
      int num = 0;
      for (int index = 0; index < this.m_count; ++index)
      {
        if (this.m_entries[index].hashCode >= 0)
        {
          TKey key = this.m_entries[index].key;
          if (predicate(this.m_entries[index].value))
          {
            if (removedValueCallback != null)
            {
              TValue obj;
              this.TryRemove(key, out obj).AssertTrue();
              removedValueCallback(Make.Kvp<TKey, TValue>(key, obj));
            }
            else
              this.RemoveAndAssert(key);
            ++num;
          }
        }
      }
      return num;
    }

    public int RemoveKeys(
      Predicate<TKey> predicate,
      Action<KeyValuePair<TKey, TValue>> removedValueCallback = null)
    {
      int num = 0;
      for (int index = 0; index < this.m_count; ++index)
      {
        if (this.m_entries[index].hashCode >= 0)
        {
          TKey key = this.m_entries[index].key;
          if (predicate(key))
          {
            if (removedValueCallback != null)
            {
              TValue obj;
              this.TryRemove(key, out obj).AssertTrue();
              removedValueCallback(Make.Kvp<TKey, TValue>(key, obj));
            }
            else
              this.RemoveAndAssert(key);
            ++num;
          }
        }
      }
      return num;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      int entry = this.findEntry(key);
      if (entry >= 0)
      {
        value = this.m_entries[entry].value;
        return true;
      }
      value = default (TValue);
      return false;
    }

    /// <summary>
    /// Returns ref to an existing element or adds a default value and returns ref for it.
    /// This method is advantageous in situations when we'd need to do <c>read-mutate-write</c> operations on structs
    /// and avoids one extra lookup since we can do just <c>getref-mutate</c>.
    /// </summary>
    public ref TValue GetRefValue(TKey key, out bool exists)
    {
      int insertedIndex;
      exists = !this.insert(key, default (TValue), true, out bool _, out insertedIndex);
      return ref this.m_entries[insertedIndex].value;
    }

    /// <summary>
    /// An efficient way to get the first value without creating an enumerator. Returned value is identical to the
    /// first value returned by the enumerator.
    /// </summary>
    public KeyValuePair<TKey, TValue> FirstOrDefault()
    {
      for (int index = 0; index < this.m_count; ++index)
      {
        if (this.m_entries[index].hashCode >= 0)
          return new KeyValuePair<TKey, TValue>(this.m_entries[index].key, this.m_entries[index].value);
      }
      return new KeyValuePair<TKey, TValue>();
    }

    /// <summary>Allocation-free sampling of random key in O(n).</summary>
    public TKey SampleRandomKeyOrDefault(IRandom random)
    {
      if (this.IsEmpty)
        return default (TKey);
      int num = random.NextInt(this.Count);
      foreach (TKey key in this.Keys)
      {
        if (num <= 0)
          return key;
        --num;
      }
      Mafi.Assert.Fail("This should not happen.");
      return default (TKey);
    }

    /// <summary>Allocation-free sampling of random value in O(n).</summary>
    public TValue SampleRandomValueOrDefault(IRandom random)
    {
      if (this.IsEmpty)
        return default (TValue);
      int num = random.NextInt(this.Count);
      foreach (TValue obj in this.Values)
      {
        if (num <= 0)
          return obj;
        --num;
      }
      Mafi.Assert.Fail("This should not happen.");
      return default (TValue);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new Dict<TKey, TValue>.Enumerator(this);
    }

    public void SerializeData(BlobWriter writer)
    {
      Mafi.Assert.That<bool>(typeof (TKey).IsValueType || typeof (TKey) == typeof (string) || typeof (TKey).IsAssignableTo(typeof (IIsSafeAsHashKey))).IsTrue<string, string>("Type '{0}' cannot be a key of serialized Dict<{0}, {1}> due to non-deterministic hash.", typeof (TKey).Name, typeof (TValue).Name);
      Mafi.Assert.That<IEqualityComparer<TKey>>(this.Comparer).IsEqualTo<IEqualityComparer<TKey>>((IEqualityComparer<TKey>) EqualityComparer<TKey>.Default, "Cannot serialize dict with a custom comparer.");
      writer.WriteIntNotNegative(this.Count);
      Action<TKey, BlobWriter> serializerFor1 = writer.GetSerializerFor<TKey>();
      Action<TValue, BlobWriter> serializerFor2 = writer.GetSerializerFor<TValue>();
      foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
      {
        serializerFor1(keyValuePair.Key, writer);
        serializerFor2(keyValuePair.Value, writer);
      }
    }

    public void DeserializeData(BlobReader reader)
    {
      this.m_count = reader.ReadIntNotNegative();
      reader.SetField<Dict<TKey, TValue>>(this, "m_comparer", (object) EqualityComparer<TKey>.Default);
      if (this.m_count > 0)
      {
        this.m_entries = new Dict<TKey, TValue>.Entry[this.m_count];
        Func<BlobReader, TKey> deserializerFor1 = reader.GetDeserializerFor<TKey>();
        Func<BlobReader, TValue> deserializerFor2 = reader.GetDeserializerFor<TValue>();
        for (int index = 0; index < this.m_count; ++index)
          this.m_entries[index] = new Dict<TKey, TValue>.Entry()
          {
            key = deserializerFor1(reader),
            value = deserializerFor2(reader)
          };
      }
      reader.RegisterInitAfterLoad<Dict<TKey, TValue>>(this, "initAfterLoad", InitPriority.Highest);
    }

    private void initAfterLoad()
    {
      if (this.m_count == 0)
      {
        Mafi.Assert.That<int[]>(this.m_buckets).IsNull<int[]>();
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNull<Dict<TKey, TValue>.Entry[]>();
      }
      else
      {
        Mafi.Assert.That<int[]>(this.m_buckets).IsNull<int[]>();
        Mafi.Assert.That<Dict<TKey, TValue>.Entry[]>(this.m_entries).IsNotNull<Dict<TKey, TValue>.Entry[]>();
        Mafi.Assert.That<int>(this.m_entries.Length).IsEqualTo(this.m_count);
        this.resize(HashHelpers.GetPrime(this.m_entries.Length), true);
        Mafi.Assert.That<int[]>(this.m_buckets).IsNotNull<int[]>();
        this.m_freeList = -1;
      }
    }

    public static void Serialize(Dict<TKey, TValue> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Dict<TKey, TValue>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Dict<TKey, TValue>.s_serializeDataDelayedAction);
    }

    public static Dict<TKey, TValue> Deserialize(BlobReader reader)
    {
      Dict<TKey, TValue> dict;
      if (reader.TryStartClassDeserialization<Dict<TKey, TValue>>(out dict))
        reader.EnqueueDataDeserialization((object) dict, Dict<TKey, TValue>.s_deserializeDataDelayedAction);
      return dict;
    }

    static Dict()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Dict<TKey, TValue>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Dict<TKey, TValue>) obj).SerializeData(writer));
      Dict<TKey, TValue>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Dict<TKey, TValue>) obj).DeserializeData(reader));
    }

    private struct Entry
    {
      public int hashCode;
      public int next;
      public TKey key;
      public TValue value;
    }

    [Serializable]
    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator
    {
      private readonly Dict<TKey, TValue> m_dictionary;
      private readonly int m_version;
      private int m_index;
      private KeyValuePair<TKey, TValue> m_current;

      internal Enumerator(Dict<TKey, TValue> dictionary)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_dictionary = dictionary;
        this.m_version = dictionary.m_version;
        this.m_index = 0;
        this.m_current = new KeyValuePair<TKey, TValue>();
      }

      public bool MoveNext()
      {
        if (this.m_version != this.m_dictionary.m_version)
          throw new InvalidOperationException("Dictionary changed during iteration.");
        for (; (uint) this.m_index < (uint) this.m_dictionary.m_count; ++this.m_index)
        {
          if (this.m_dictionary.m_entries[this.m_index].hashCode >= 0)
          {
            this.m_current = new KeyValuePair<TKey, TValue>(this.m_dictionary.m_entries[this.m_index].key, this.m_dictionary.m_entries[this.m_index].value);
            ++this.m_index;
            return true;
          }
        }
        this.m_index = this.m_dictionary.m_count + 1;
        this.m_current = new KeyValuePair<TKey, TValue>();
        return false;
      }

      public KeyValuePair<TKey, TValue> Current => this.m_current;

      public void Dispose()
      {
      }

      object IEnumerator.Current
      {
        get => (object) new KeyValuePair<TKey, TValue>(this.m_current.Key, this.m_current.Value);
      }

      void IEnumerator.Reset()
      {
        this.m_index = 0;
        this.m_current = new KeyValuePair<TKey, TValue>();
      }
    }

    public class EnumeratorNonGeneric : 
      IEnumerator<KeyValuePair<object, object>>,
      IDisposable,
      IEnumerator
    {
      private readonly Dict<TKey, TValue> m_dictionary;
      private readonly int m_version;
      private int m_index;
      private KeyValuePair<object, object> m_current;

      internal EnumeratorNonGeneric(Dict<TKey, TValue> dictionary)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_dictionary = dictionary;
        this.m_version = dictionary.m_version;
        this.m_index = 0;
        this.m_current = new KeyValuePair<object, object>();
      }

      public bool MoveNext()
      {
        if (this.m_version != this.m_dictionary.m_version)
          throw new InvalidOperationException("Dictionary changed during iteration.");
        for (; (uint) this.m_index < (uint) this.m_dictionary.m_count; ++this.m_index)
        {
          if (this.m_dictionary.m_entries[this.m_index].hashCode >= 0)
          {
            this.m_current = new KeyValuePair<object, object>((object) this.m_dictionary.m_entries[this.m_index].key, (object) this.m_dictionary.m_entries[this.m_index].value);
            ++this.m_index;
            return true;
          }
        }
        this.m_index = this.m_dictionary.m_count + 1;
        this.m_current = new KeyValuePair<object, object>();
        return false;
      }

      public KeyValuePair<object, object> Current => this.m_current;

      public void Dispose()
      {
      }

      object IEnumerator.Current
      {
        get => (object) new KeyValuePair<object, object>(this.m_current.Key, this.m_current.Value);
      }

      void IEnumerator.Reset()
      {
        this.m_index = 0;
        this.m_current = new KeyValuePair<object, object>();
      }
    }

    [DebuggerDisplay("Count = {Count}")]
    public sealed class KeyCollection : IReadOnlyCollection<TKey>, IEnumerable<TKey>, IEnumerable
    {
      private readonly Dict<TKey, TValue> m_dictionary;

      public KeyCollection(Dict<TKey, TValue> dictionary)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_dictionary = dictionary.CheckNotNull<Dict<TKey, TValue>>();
      }

      public Dict<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
      {
        return new Dict<TKey, TValue>.KeyCollection.Enumerator(this.m_dictionary);
      }

      public void CopyTo(TKey[] array, int index)
      {
        Mafi.Assert.That<TKey[]>(array).IsNotNull<TKey[]>();
        if (index < 0 || index > array.Length)
          throw new ArgumentException(nameof (index));
        if (array.Length - index < this.m_dictionary.Count)
          throw new ArgumentException("Array is too small", nameof (array));
        int count = this.m_dictionary.m_count;
        Dict<TKey, TValue>.Entry[] entries = this.m_dictionary.m_entries;
        for (int index1 = 0; index1 < count; ++index1)
        {
          if (entries[index1].hashCode >= 0)
            array[index++] = entries[index1].key;
        }
      }

      public int Count => this.m_dictionary.Count;

      IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
      {
        return (IEnumerator<TKey>) new Dict<TKey, TValue>.KeyCollection.Enumerator(this.m_dictionary);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) new Dict<TKey, TValue>.KeyCollection.Enumerator(this.m_dictionary);
      }

      public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
      {
        private readonly Dict<TKey, TValue> m_dictionary;
        private readonly int m_version;
        private int m_index;
        private TKey m_currentKey;

        internal Enumerator(Dict<TKey, TValue> dictionary)
        {
          MBiHIp97M4MqqbtZOh.RFLpSOptx();
          this.m_dictionary = dictionary;
          this.m_version = dictionary.m_version;
          this.m_index = 0;
          this.m_currentKey = default (TKey);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this.m_version != this.m_dictionary.m_version)
            throw new InvalidOperationException("Dictionary changed during iteration.");
          for (; (uint) this.m_index < (uint) this.m_dictionary.m_count; ++this.m_index)
          {
            if (this.m_dictionary.m_entries[this.m_index].hashCode >= 0)
            {
              this.m_currentKey = this.m_dictionary.m_entries[this.m_index].key;
              ++this.m_index;
              return true;
            }
          }
          this.m_index = this.m_dictionary.m_count + 1;
          this.m_currentKey = default (TKey);
          return false;
        }

        public TKey Current => this.m_currentKey;

        object IEnumerator.Current
        {
          get
          {
            if (this.m_index == 0 || this.m_index == this.m_dictionary.m_count + 1)
              throw new InvalidOperationException("Invalid Current call on enumerator.");
            return (object) this.m_currentKey;
          }
        }

        void IEnumerator.Reset()
        {
          if (this.m_version != this.m_dictionary.m_version)
            throw new InvalidOperationException("Dictionary changed during iteration.");
          this.m_index = 0;
          this.m_currentKey = default (TKey);
        }
      }
    }

    [DebuggerDisplay("Count = {Count}")]
    public sealed class ValueCollection : 
      IReadOnlyCollection<TValue>,
      IEnumerable<TValue>,
      IEnumerable
    {
      private readonly Dict<TKey, TValue> m_dictionary;

      public ValueCollection(Dict<TKey, TValue> dictionary)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_dictionary = dictionary.CheckNotNull<Dict<TKey, TValue>>();
      }

      public Dict<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
      {
        return new Dict<TKey, TValue>.ValueCollection.Enumerator(this.m_dictionary);
      }

      public void CopyTo(TValue[] array, int index)
      {
        Mafi.Assert.That<TValue[]>(array).IsNotNull<TValue[]>();
        if (index < 0 || index > array.Length)
          throw new ArgumentException(nameof (index));
        if (array.Length - index < this.m_dictionary.Count)
          throw new ArgumentException("Array is too small", nameof (array));
        int count = this.m_dictionary.m_count;
        Dict<TKey, TValue>.Entry[] entries = this.m_dictionary.m_entries;
        for (int index1 = 0; index1 < count; ++index1)
        {
          if (entries[index1].hashCode >= 0)
            array[index++] = entries[index1].value;
        }
      }

      public int Count => this.m_dictionary.Count;

      IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
      {
        return (IEnumerator<TValue>) new Dict<TKey, TValue>.ValueCollection.Enumerator(this.m_dictionary);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) new Dict<TKey, TValue>.ValueCollection.Enumerator(this.m_dictionary);
      }

      public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
      {
        private readonly Dict<TKey, TValue> m_dictionary;
        private readonly int m_version;
        private int m_index;
        private TValue m_currentValue;

        internal Enumerator(Dict<TKey, TValue> dictionary)
        {
          MBiHIp97M4MqqbtZOh.RFLpSOptx();
          this.m_dictionary = dictionary;
          this.m_version = dictionary.m_version;
          this.m_index = 0;
          this.m_currentValue = default (TValue);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this.m_version != this.m_dictionary.m_version)
            throw new InvalidOperationException("Dictionary changed during iteration.");
          for (; (uint) this.m_index < (uint) this.m_dictionary.m_count; ++this.m_index)
          {
            if (this.m_dictionary.m_entries[this.m_index].hashCode >= 0)
            {
              this.m_currentValue = this.m_dictionary.m_entries[this.m_index].value;
              ++this.m_index;
              return true;
            }
          }
          this.m_index = this.m_dictionary.m_count + 1;
          this.m_currentValue = default (TValue);
          return false;
        }

        public TValue Current => this.m_currentValue;

        object IEnumerator.Current
        {
          get
          {
            if (this.m_index == 0 || this.m_index == this.m_dictionary.m_count + 1)
              throw new InvalidOperationException("Invalid Current call on enumerator.");
            return (object) this.m_currentValue;
          }
        }

        void IEnumerator.Reset()
        {
          if (this.m_version != this.m_dictionary.m_version)
            throw new InvalidOperationException("Dictionary changed during iteration.");
          this.m_index = 0;
          this.m_currentValue = default (TValue);
        }
      }
    }
  }
}
