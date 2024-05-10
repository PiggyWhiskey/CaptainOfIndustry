// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.CircularBuffer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Collections
{
  public class CircularBuffer<T> : 
    IEnumerable<T>,
    IEnumerable,
    IIndexable<T>,
    ICollectionWithCount,
    IReadOnlyCollection<T>
  {
    private int m_writeIndex;
    private readonly T[] m_buffer;

    /// <summary>Returns last added item.</summary>
    public T this[int index]
    {
      get
      {
        return this.m_buffer[(index + this.m_writeIndex + this.m_buffer.Length - 1) % this.m_buffer.Length];
      }
    }

    public int Count => this.m_buffer.Length;

    public CircularBuffer(int size)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_buffer = new T[size];
    }

    public void Add(T item)
    {
      this.m_buffer[this.m_writeIndex] = item;
      this.m_writeIndex = (this.m_writeIndex + 1) % this.m_buffer.Length;
    }

    public IEnumerable<T> AsEnumerable() => (IEnumerable<T>) this.m_buffer;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return ((IEnumerable<T>) this.m_buffer).AsEnumerable<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.m_buffer.GetEnumerator();

    IndexableEnumerator<T> IIndexable<T>.GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }
  }
}
