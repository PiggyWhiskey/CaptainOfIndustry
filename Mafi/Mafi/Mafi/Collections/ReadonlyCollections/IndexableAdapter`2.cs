// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.IndexableAdapter`2
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
  /// <summary>Presents mapped IIndexable as IIndexable.</summary>
  public class IndexableAdapter<TSource, TDest> : 
    IIndexable<TDest>,
    ICollectionWithCount,
    IEnumerable<TDest>,
    IEnumerable
  {
    /// <summary>Source data.</summary>
    private readonly IIndexable<TSource> m_source;
    /// <summary>
    /// Used to transform items in source to destination type.
    /// </summary>
    private readonly Func<TSource, TDest> m_adapter;

    public int Count => this.m_source.Count;

    public TDest this[int index] => this.m_adapter(this.m_source[index]);

    public IndexableAdapter(IIndexable<TSource> source, Func<TSource, TDest> adapter)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_source = source;
      this.m_adapter = adapter;
    }

    public IndexableEnumerator<TDest> GetEnumerator()
    {
      return new IndexableEnumerator<TDest>((IIndexable<TDest>) this);
    }

    public IEnumerable<TDest> AsEnumerable() => (IEnumerable<TDest>) this;

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new IndexableIenumerator<TDest>((IIndexable<TDest>) this);
    }

    IEnumerator<TDest> IEnumerable<TDest>.GetEnumerator()
    {
      return (IEnumerator<TDest>) new IndexableIenumerator<TDest>((IIndexable<TDest>) this);
    }
  }
}
