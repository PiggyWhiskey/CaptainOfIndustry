// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.SettlementServiceModulePortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SettlementServiceModulePortProductResolver : 
    PortProductResolverBase<SettlementServiceModule>
  {
    public SettlementServiceModulePortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      SettlementServiceModule entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      if (port.Type == IoPortType.Input)
        return ImmutableArray.Create<ProductProto>(entity.InputProduct.Product);
      if (port.Type == IoPortType.Output)
      {
        ProductQuantity? outputProduct = entity.OutputProduct;
        if (outputProduct.HasValue)
        {
          outputProduct = entity.OutputProduct;
          return ImmutableArray.Create<ProductProto>(outputProduct.Value.Product);
        }
      }
      Log.Error(string.Format("Unhandled port {0}.", (object) port));
      return ImmutableArray<ProductProto>.Empty;
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      SettlementModuleProto settlementModuleProto = (SettlementModuleProto) proto;
      if (portSpec.Type == IoPortType.Input)
        return ImmutableArray.Create<ProductProto>(settlementModuleProto.InputProduct);
      if (portSpec.Type == IoPortType.Output && settlementModuleProto.OutputProduct.HasValue)
        return ImmutableArray.Create<ProductProto>(settlementModuleProto.OutputProduct.Value);
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
