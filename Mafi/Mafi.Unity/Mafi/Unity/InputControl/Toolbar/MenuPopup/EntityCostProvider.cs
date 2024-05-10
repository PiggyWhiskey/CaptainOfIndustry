// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.EntityCostProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  /// <summary>
  /// Only to be used in UI (otherwise construction would need to filter out unity).
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntityCostProvider
  {
    private readonly Dict<Type, IDynamicCostProvider> m_costProviders;

    public EntityCostProvider(
      AllImplementationsOf<IDynamicCostProvider> costProviders)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_costProviders = new Dict<Type, IDynamicCostProvider>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      foreach (IDynamicCostProvider implementation in costProviders.Implementations)
        this.m_costProviders.AddAndAssertNew(implementation.ManagedProtoType, implementation);
    }

    public AssetValue GetEntityCost(LayoutEntityProto proto)
    {
      AssetValue price = proto.Costs.Price;
      IDynamicCostProvider dynamicCostProvider;
      return this.m_costProviders.TryGetValue(proto.GetType(), out dynamicCostProvider) ? new AssetValue(price.Products.ToImmutableArray().Concat(dynamicCostProvider.GetDynamicCost().Products.ToImmutableArray())) : price;
    }
  }
}
