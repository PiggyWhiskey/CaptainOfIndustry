// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.CompareByCount`1
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
  /// <summary>
  /// Very efficient collections comparator using count but also very error prone. This works only with a very hard
  /// guarantee that collection is changed only by adding / removing values. So if you swap the observed collection
  /// with different one but with same size this won't pick it up. Currently we can use it in the UI because we
  /// invalidate onShow which forces the collection re-compute. That guarantees that only user actions can change the
  /// collection. And for user it is impossible to change more than one item per sync update (add recipe, vehicle,
  /// product).
  /// </summary>
  public sealed class CompareByCount<T> : 
    IReadOnlyCollectionComprator<T>,
    ICollectionComparator<T, IReadOnlyCollection<T>>,
    IIndexableComparator<T>,
    ICollectionComparator<T, IIndexable<T>>
  {
    public static readonly CompareByCount<T> Instance;

    private CompareByCount()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool AreSame(IReadOnlyCollection<T> collection, Lyst<T> lastKnown)
    {
      return lastKnown.Count == collection.Count;
    }

    public bool AreSame(IIndexable<T> collection, Lyst<T> lastKnown)
    {
      return lastKnown.Count == collection.Count;
    }

    static CompareByCount()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CompareByCount<T>.Instance = new CompareByCount<T>();
    }
  }
}
