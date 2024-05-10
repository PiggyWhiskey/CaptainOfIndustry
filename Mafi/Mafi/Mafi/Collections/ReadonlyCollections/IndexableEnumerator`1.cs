// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ReadonlyCollections.IndexableEnumerator`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections.ReadonlyCollections
{
  [DebuggerStepThrough]
  public struct IndexableEnumerator<T>
  {
    private readonly IIndexable<T> m_indexable;
    private int m_index;

    internal IndexableEnumerator(IIndexable<T> indexable)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_indexable = indexable;
      this.m_index = -1;
    }

    public bool MoveNext() => ++this.m_index < this.m_indexable.Count;

    public T Current => this.m_indexable[this.m_index];
  }
}
