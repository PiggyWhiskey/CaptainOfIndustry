// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ArrayDataStorageSlim`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Stores elements in array. Resulting array may have "holes" from removed elements with value of <c>default</c>.
  /// It guarantees that inserted elements index never changes and the internal array is never larger
  /// than the number of stored elements. This class is the same as <c>ArrayDataStorage</c> except that
  /// it uses short as indices instead of int. This makes it lighter on memory, but less versatile.
  /// 
  /// This datastructure supports storing values of <c>default</c>.
  /// 
  /// Default value of this struct is a valid empty array and it is ok to Add directly to it.
  /// 
  /// Note that we store removed indices as <see cref="T:System.UInt16" /> in order to save memory.
  /// Thus, the total number of stored elements should NOT exceed <see cref="F:System.UInt16.MaxValue" /> (65k).
  /// </summary>
  public struct ArrayDataStorageSlim<T>
  {
    private LystStruct<T> m_data;
    private LystStruct<ushort> m_emptyIndices;

    /// <summary>Number of stored non-empty elements.</summary>
    public int Count { get; private set; }

    public readonly int Capacity => this.m_data.Capacity;

    public readonly bool IsEmpty => this.Count <= 0;

    public readonly bool IsNotEmpty => this.Count > 0;

    /// <summary>
    /// The highest count that has (or had) a valid item. Note that removing elements in random order will
    /// not decrease this value. You can use <see cref="M:Mafi.Collections.ArrayDataStorageSlim`1.RecomputeHighestUsedCount" /> to update this to the highest
    /// valid element.
    /// </summary>
    public readonly int HighestUsedCount => this.m_data.Count;

    internal readonly int EmptyIndicesCount => this.m_emptyIndices.Count;

    public ReadOnlyArraySlice<T> Data => this.m_data.BackingArrayAsSlice;

    public ArrayDataStorageSlim(int capacity)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_data = new LystStruct<T>(capacity);
      this.Count = 0;
      this.m_emptyIndices = new LystStruct<ushort>();
    }

    /// <summary>
    /// Indexer with unchecked array access (for performance).
    /// </summary>
    public T this[int index]
    {
      readonly get => this.m_data[index];
      set => this.m_data[index] = value;
    }

    public ref T GetRefValue(int index) => ref this.m_data.GetRefAt(index);

    /// <summary>
    /// Adds an item and returns its index. This is O(1) operation (amortized).
    /// </summary>
    public ushort Add(T item)
    {
      ushort index;
      if (this.m_emptyIndices.IsNotEmpty)
      {
        index = this.m_emptyIndices.PopLast();
        this.m_data[(int) index] = item;
      }
      else
      {
        index = (ushort) this.m_data.Count;
        this.m_data.Add(item);
      }
      ++this.Count;
      return index;
    }

    /// <summary>Updates an existing item.</summary>
    public void UpdateAt(ushort index, T item)
    {
      if ((int) index >= this.m_data.Count)
        Log.Error(string.Format("Failed to update at index {0} (count: {1}).", (object) index, (object) this.m_data.Count));
      else
        this.m_data[(int) index] = item;
    }

    /// <summary>
    /// Removes stored element at the given index and resets backing array value at that index to <c>default</c>.
    /// This is O(1) operation (amortized).
    /// </summary>
    public void Remove(ushort index)
    {
      if ((int) index >= this.m_data.Count)
      {
        Log.Error(string.Format("Invalid index {0}, data length: {1}", (object) index, (object) this.m_data.Count));
      }
      else
      {
        if ((int) index + 1 == this.m_data.Count)
        {
          this.m_data.PopLast();
        }
        else
        {
          this.m_data[(int) index] = default (T);
          this.m_emptyIndices.Add(index);
        }
        --this.Count;
        if (this.Count != 0)
          return;
        this.m_data.Clear();
        this.m_emptyIndices.Clear();
      }
    }

    /// <summary>
    /// In case there were many elements removed from the end, recomputing <see cref="P:Mafi.Collections.ArrayDataStorageSlim`1.HighestUsedCount" /> might make
    /// it lower. This is O(n log n) operation where n is the number of empty indices lower than
    /// <see cref="P:Mafi.Collections.ArrayDataStorageSlim`1.HighestUsedCount" />.
    /// </summary>
    public void RecomputeHighestUsedCount()
    {
      if (this.m_emptyIndices.IsEmpty)
        return;
      this.m_emptyIndices.Sort();
      while (this.m_emptyIndices.IsNotEmpty && (int) this.m_emptyIndices.Last == this.m_data.Count - 1)
      {
        int num = (int) this.m_emptyIndices.PopLast();
        this.m_data.PopLast();
      }
    }

    public readonly T[] GetBackingArray() => this.m_data.GetBackingArray();

    public void Clear()
    {
      this.Count = 0;
      this.m_data.ClearSkipZeroingMemory();
      this.m_emptyIndices.ClearSkipZeroingMemory();
    }

    /// <summary>
    /// Clears all data at and after the given index.
    /// WARNING: This "forgets" information about empty indices in the non-cleared part, call this only if the
    /// values until the given index are valid.
    /// </summary>
    public void ClearFromIndex(int index)
    {
      if (this.Count <= index)
        return;
      this.Count = index;
      this.m_data.ClearFromIndexSkipZeroingMemory(index);
      this.m_emptyIndices.ClearSkipZeroingMemory();
    }

    public void EnsureCapacity(int minCapacity) => this.m_data.EnsureCapacity(minCapacity);
  }
}
