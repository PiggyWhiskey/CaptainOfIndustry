// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.IReadOnlySet`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  public interface IReadOnlySet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
  {
    IEqualityComparer<T> Comparer { get; }

    bool Contains(T item);

    bool TryGetValue(T equalValue, out T actualValue);
  }
}
