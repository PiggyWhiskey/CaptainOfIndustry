// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadWriteSwapLyst`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using System;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Writes to one list and reads from other one. Swap them on demand.
  /// </summary>
  /// <remarks>
  /// This data structure is very handy where sim thread is working on some things and writing them to one list while
  /// vis thread is reading changes from the last sim step. It also makes reads thread-safe and separated from writes,
  /// however, multiple writes are not thread safe!
  /// 
  /// This class should be exposed through <see cref="T:Mafi.Collections.IReadSwapLyst`1" /> interface.
  /// </remarks>
  public class ReadWriteSwapLyst<T> : IReadSwapLyst<T>, ICollectionWithCount
  {
    private Lyst<T> m_readList;
    private Lyst<T> m_writeList;

    /// <summary>Returns items count in current read list.</summary>
    public int Count => this.m_readList.Count;

    public bool IsEmpty => this.m_readList.IsEmpty;

    public IIndexable<T> ReadList => (IIndexable<T>) this.m_readList;

    /// <summary>Adds an item to the write list.</summary>
    public void Add(T item) => this.m_writeList.Add(item);

    /// <summary>
    /// Tries to remove an item from the write list (the one that is not exposed for reading). Returns true if the
    /// element was removed.
    /// </summary>
    public bool Remove(T item) => this.m_writeList.Remove(item);

    /// <summary>
    /// Swaps read and write lists and clears the new write list.
    /// </summary>
    public void SwapAndClear()
    {
      Swap.Them<Lyst<T>>(ref this.m_readList, ref this.m_writeList);
      this.m_writeList.Clear();
    }

    public Lyst<T>.Enumerator GetEnumerator() => this.m_readList.GetEnumerator();

    /// <summary>Performs given action on every item in the read list.</summary>
    public void ForEach(Action<T> action) => this.m_readList.ForEach(action);

    public ReadWriteSwapLyst()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_readList = new Lyst<T>();
      this.m_writeList = new Lyst<T>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
