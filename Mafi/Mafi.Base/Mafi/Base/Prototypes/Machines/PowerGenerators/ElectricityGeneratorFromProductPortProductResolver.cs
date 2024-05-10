// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ElectricityGeneratorFromProductPortProductResolver
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ElectricityGeneratorFromProductPortProductResolver : IPortProductResolverImpl
  {
    public Type ResolvedEntityType => typeof (ElectricityGeneratorFromProduct);

    public ImmutableArray<ProductProto> GetPortProduct(
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return this.GetPortProduct((IEntityProto) port.OwnerEntity.Prototype, port.Spec);
    }

    public ImmutableArray<ProductProto> GetPortProduct(IEntityProto proto, PortSpec portSpec)
    {
      ElectricityGeneratorFromProductProto fromProductProto = (ElectricityGeneratorFromProductProto) proto;
      return portSpec.Type == IoPortType.Output && fromProductProto.OutputProduct.IsNotEmpty ? ImmutableArray.Create<ProductProto>(((ElectricityGeneratorFromProductProto) proto).OutputProduct.Product) : ImmutableArray.Create<ProductProto>(((ElectricityGeneratorFromProductProto) proto).InputProduct.Product);
    }

    public ElectricityGeneratorFromProductPortProductResolver()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
