// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.IUnitUnlocker`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>
  /// Unlocks units of type <see cref="!:TUnit" /> and their subclasses - if there aren't more specific unlockers for
  /// them. Implementation of the interface has to be registered with <see cref="T:Mafi.DependencyResolver" /> in order to be
  /// used for unlocking.
  /// </summary>
  public interface IUnitUnlocker<TUnit> : IUnitUnlocker where TUnit : IUnlockNodeUnit
  {
    void Unlock(IIndexable<TUnit> units);
  }
}
