// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.IIndexable`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  /// <summary>Read-only collection that can be indexed.</summary>
  /// <remarks>Please use only on classes to prevent boxing.</remarks>
  [NotGlobalDependency]
  public interface IIndexable<T> : ICollectionWithCount
  {
    T this[int index] { get; }

    /// <summary>
    /// Returns struct-based enumerator for allocation-free iteration through indexable.
    /// </summary>
    IndexableEnumerator<T> GetEnumerator();

    IEnumerable<T> AsEnumerable();
  }
}
