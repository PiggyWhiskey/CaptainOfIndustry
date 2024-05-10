// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.RecyclingRatioIncreaseUnlocker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class RecyclingRatioIncreaseUnlocker : UnitUnlockerBase<RecyclingRatioIncreaseUnlock>
  {
    private readonly IProductsManager m_productsManager;

    public RecyclingRatioIncreaseUnlocker(IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
    }

    public override void Unlock(IIndexable<RecyclingRatioIncreaseUnlock> units)
    {
      foreach (RecyclingRatioIncreaseUnlock unit in units)
        this.m_productsManager.IncreaseRecyclingRatio(unit.RecyclingRatioIncrease);
    }
  }
}
