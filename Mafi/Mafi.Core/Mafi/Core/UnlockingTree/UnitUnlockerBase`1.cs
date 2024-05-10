// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.UnitUnlockerBase`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using System;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  public abstract class UnitUnlockerBase<TUnit> : IUnitUnlocker<TUnit>, IUnitUnlocker where TUnit : IUnlockNodeUnit
  {
    public Type UnlockedType { get; }

    public abstract void Unlock(IIndexable<TUnit> units);

    protected UnitUnlockerBase()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.UnlockedType = typeof (TUnit);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
