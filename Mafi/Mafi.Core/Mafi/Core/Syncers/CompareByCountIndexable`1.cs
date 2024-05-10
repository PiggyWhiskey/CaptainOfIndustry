// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.CompareByCountIndexable`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Core.Syncers
{
  /// <summary>
  /// <see cref="T:Mafi.Core.Syncers.CompareByCount`1" /> for more info.
  /// </summary>
  public sealed class CompareByCountIndexable<T> : 
    IIndexableComparator<T>,
    ICollectionComparator<T, IIndexable<T>>
  {
    public static readonly CompareByCountIndexable<T> Instance;

    private CompareByCountIndexable()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool AreSame(IIndexable<T> collection, Lyst<T> lastKnown)
    {
      return lastKnown.Count == collection.Count;
    }

    static CompareByCountIndexable()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CompareByCountIndexable<T>.Instance = new CompareByCountIndexable<T>();
    }
  }
}
