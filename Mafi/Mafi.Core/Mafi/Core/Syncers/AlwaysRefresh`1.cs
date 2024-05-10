// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.AlwaysRefresh`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  public sealed class AlwaysRefresh<T> : 
    IReadOnlyCollectionComprator<T>,
    ICollectionComparator<T, IReadOnlyCollection<T>>,
    IIndexableComparator<T>,
    ICollectionComparator<T, IIndexable<T>>,
    IEnumerableComparator<T>,
    ICollectionComparator<T, IEnumerable<T>>
  {
    public static readonly AlwaysRefresh<T> Instance;

    public AlwaysRefresh()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool AreSame(IReadOnlyCollection<T> collection, Lyst<T> lastKnown) => false;

    public bool AreSame(IIndexable<T> collection, Lyst<T> lastKnown) => false;

    public bool AreSame(IEnumerable<T> collection, Lyst<T> lastKnown) => false;

    static AlwaysRefresh()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AlwaysRefresh<T>.Instance = new AlwaysRefresh<T>();
    }
  }
}
