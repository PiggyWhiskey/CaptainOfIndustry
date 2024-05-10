// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.IndexableIenumerator`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  /// <summary>
  /// Enumerator that can be used to implement AsEnumerable on any class implementing <see cref="T:Mafi.Collections.ReadonlyCollections.IIndexable`1" />.
  /// The class can then implement <see cref="T:System.Collections.Generic.IEnumerable`1" /> too and its GetEnumerator implementation for
  /// IEnumerable then returns this enumerator. The enumerator is implemented as class because it is used through the
  /// <see cref="T:System.Collections.Generic.IEnumerator`1" /> interface anyway.
  /// </summary>
  public class IndexableIenumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
  {
    private int m_index;
    private readonly IIndexable<T> m_indexable;

    public T Current => this.m_indexable[this.m_index];

    object IEnumerator.Current => (object) this.Current;

    public IndexableIenumerator(IIndexable<T> indexable)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_index = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_indexable = indexable;
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      ++this.m_index;
      return this.m_index <= this.m_indexable.Count;
    }

    public void Reset() => this.m_index = -1;
  }
}
