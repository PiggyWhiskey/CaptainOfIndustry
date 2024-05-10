// Decompiled with JetBrains decompiler
// Type: Mafi.Indexable`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Helper class that offers empty indexable instance for convenience.
  /// </summary>
  public class Indexable<T> : IIndexable<T>, ICollectionWithCount
  {
    /// <summary>Empty indexable instance for convenience.</summary>
    public static readonly IIndexable<T> Empty;

    private Indexable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public T this[int index]
    {
      get => throw new InvalidOperationException("Indexing into an empty IIndexable.");
    }

    public int Count => 0;

    public IndexableEnumerator<T> GetEnumerator()
    {
      return new IndexableEnumerator<T>((IIndexable<T>) this);
    }

    public IEnumerable<T> AsEnumerable() => Enumerable.Empty<T>();

    static Indexable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Indexable<T>.Empty = (IIndexable<T>) new Indexable<T>();
    }
  }
}
