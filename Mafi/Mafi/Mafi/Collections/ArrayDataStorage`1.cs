// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ArrayDataStorage`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Stores elements in array. Resulting array may have "holes" from removed elements with value of <c>default</c>.
  /// It guarantees that inserted elements index never changes and the internal array is never larger
  /// than the number of stored elements. This class is the same as <c>ArrayDataStorageSlim</c> except that
  /// it uses int as indices instead of short. This makes it heavier on memory, but more versatile.
  /// 
  /// This datastructure supports storing values of <c>default</c>.
  /// 
  /// Default value of this struct is a valid empty array and it is ok to Add directly to it.
  /// </summary>
  public struct ArrayDataStorage<T>
  {
    private LystStruct<T> m_data;
    private LystStruct<int> m_emptyIndices;

    /// <summary>Number of stored non-empty elements.</summary>
    public int Count { get; private set; }

    public readonly int Capacity => this.m_data.Capacity;

    public readonly bool IsEmpty => this.Count <= 0;

    public readonly bool IsNotEmpty => this.Count > 0;

    /// <summary>
    /// The highest count that has (or had) a valid item. Note that removing elements in random order will
    /// not decrease this value. You can use <see cref="M:Mafi.Collections.ArrayDataStorage`1.RecomputeHighestUsedCount" /> to update this to the highest
    /// valid element.
    /// </summary>
    public readonly int HighestUsedCount => this.m_data.Count;

    internal readonly int EmptyIndicesCount => this.m_emptyIndices.Count;

    public ArrayDataStorage(int capacity)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_data = new LystStruct<T>(capacity);
      this.Count = 0;
      this.m_emptyIndices = new LystStruct<int>();
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
    public int Add(T item)
    {
      int index;
      if (this.m_emptyIndices.IsNotEmpty)
      {
        index = this.m_emptyIndices.PopLast();
        this.m_data[index] = item;
      }
      else
      {
        index = this.m_data.Count;
        this.m_data.Add(item);
      }
      ++this.Count;
      return index;
    }

    /// <summary>Updates an existing item.</summary>
    public void UpdateAt(int index, T item)
    {
      if (index >= this.m_data.Count)
        Log.Error(string.Format("Failed to update at index {0} (count: {1}).", (object) index, (object) this.m_data.Count));
      else
        this.m_data[index] = item;
    }

    /// <summary>
    /// Removes stored element at the given index and resets backing array value at that index to <c>default</c>.
    /// This is O(1) operation (amortized).
    /// </summary>
    public void Remove(int index)
    {
      if (index >= this.m_data.Count)
      {
        Log.Error(string.Format("Invalid index {0}, data length: {1}", (object) index, (object) this.m_data.Count));
      }
      else
      {
        if (index + 1 == this.m_data.Count)
        {
          this.m_data.PopLast();
        }
        else
        {
          this.m_data[index] = default (T);
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
    /// In case there were many elements removed from the end, recomputing <see cref="P:Mafi.Collections.ArrayDataStorage`1.HighestUsedCount" /> might make
    /// it lower. This is O(n log n) operation where n is the number of empty indices lower than
    /// <see cref="P:Mafi.Collections.ArrayDataStorage`1.HighestUsedCount" />.
    /// </summary>
    public void RecomputeHighestUsedCount()
    {
      if (this.m_emptyIndices.IsEmpty)
        return;
      this.m_emptyIndices.Sort();
      while (this.m_emptyIndices.IsNotEmpty && this.m_emptyIndices.Last == this.m_data.Count - 1)
      {
        this.m_emptyIndices.PopLast();
        this.m_data.PopLast();
      }
    }

    public readonly T[] GetBackingArray() => this.m_data.GetBackingArray();

    public void Clear()
    {
      this.Count = 0;
      this.m_data.Clear();
      this.m_emptyIndices.Clear();
    }

    public void EnsureCapacity(int minCapacity) => this.m_data.EnsureCapacity(minCapacity);
  }
}
