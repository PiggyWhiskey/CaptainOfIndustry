// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.SpaceProgram.RocketLaunchPadPortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.SpaceProgram
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RocketLaunchPadPortProductResolver : PortProductResolverBase<RocketLaunchPad>
  {
    private ImmutableArray<ProductProto> m_waterProducts;

    public RocketLaunchPadPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      RocketLaunchPad entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return this.getPortProduct(entity.Prototype, port.Spec);
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      return this.getPortProduct((RocketLaunchPadProto) proto, portSpec);
    }

    public ImmutableArray<ProductProto> getPortProduct(
      RocketLaunchPadProto proto,
      PortSpec portSpec)
    {
      if (!proto.WaterPortNames.Contains(portSpec.Name))
        return ImmutableArray<ProductProto>.Empty;
      if (this.m_waterProducts.IsNotValid)
        this.m_waterProducts = ImmutableArray.Create<ProductProto>(proto.WaterPerLaunch.Product);
      return this.m_waterProducts;
    }
  }
}
