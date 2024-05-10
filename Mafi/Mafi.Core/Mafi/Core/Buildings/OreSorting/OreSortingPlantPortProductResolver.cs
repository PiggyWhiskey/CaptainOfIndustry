// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.OreSortingPlantPortProductResolver
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
namespace Mafi.Core.Buildings.OreSorting
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class OreSortingPlantPortProductResolver : PortProductResolverBase<OreSortingPlant>
  {
    public OreSortingPlantPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      OreSortingPlant entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return port.Type == IoPortType.Output ? entity.GetPortProducts(port) : ImmutableArray<ProductProto>.Empty;
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
