// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.MechPowerGeneratorFromProductResolver
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MechPowerGeneratorFromProductResolver : 
    PortProductResolverBase<MechPowerGeneratorFromProduct>
  {
    private readonly ImmutableArray<ProductProto> m_mechanicalPower;

    public MechPowerGeneratorFromProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
      this.m_mechanicalPower = ImmutableArray.Create<ProductProto>(protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.MechanicalPower));
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      MechPowerGeneratorFromProduct entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return this.GetPortProduct((IEntityProto) entity.Prototype, port.Spec);
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      MechPowerGeneratorFromProductProto fromProductProto = (MechPowerGeneratorFromProductProto) proto;
      if (portSpec.Shape.Id == Ids.IoPortShapes.Shaft)
        return this.m_mechanicalPower;
      if (portSpec.Type == IoPortType.Input)
        return ImmutableArray.Create<ProductProto>(fromProductProto.ConsumedProduct.Product);
      if (portSpec.Type == IoPortType.Output)
        return fromProductProto.ProducedProduct.HasValue ? ImmutableArray.Create<ProductProto>(fromProductProto.ProducedProduct.Value.Product) : ImmutableArray<ProductProto>.Empty;
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
