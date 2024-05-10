// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarmPortProductResolver
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
namespace Mafi.Core.Buildings.Farms
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class AnimalFarmPortProductResolver : PortProductResolverBase<AnimalFarm>
  {
    public AnimalFarmPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      AnimalFarm entity,
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
      AnimalFarmProto animalFarmProto = (AnimalFarmProto) proto;
      if (portSpec.Type == IoPortType.Input)
      {
        if (portSpec.Shape.AllowedProductType == animalFarmProto.FoodPerAnimalPerMonth.Product.Type)
          return ImmutableArray.Create<ProductProto>(animalFarmProto.FoodPerAnimalPerMonth.Product);
        if (portSpec.Shape.AllowedProductType == animalFarmProto.WaterPerAnimalPerMonth.Product.Type)
          return ImmutableArray.Create<ProductProto>(animalFarmProto.WaterPerAnimalPerMonth.Product);
        Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
        return ImmutableArray<ProductProto>.Empty;
      }
      if (portSpec.Type == IoPortType.Output)
      {
        if ((int) portSpec.Name == (int) animalFarmProto.CarcassOutpotPortName)
          return ImmutableArray.Create<ProductProto>(animalFarmProto.CarcassProto);
        if (animalFarmProto.ProducedPerAnimalPerMonth.HasValue && (int) portSpec.Name != (int) animalFarmProto.CarcassOutpotPortName && portSpec.Shape.AllowedProductType == animalFarmProto.ProducedPerAnimalPerMonth.Value.Product.Type)
          return ImmutableArray.Create<ProductProto>(animalFarmProto.ProducedPerAnimalPerMonth.Value.Product);
        Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
        return ImmutableArray<ProductProto>.Empty;
      }
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
