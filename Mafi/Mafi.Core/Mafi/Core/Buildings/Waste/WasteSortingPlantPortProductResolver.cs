// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Waste.WasteSortingPlantPortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Waste
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class WasteSortingPlantPortProductResolver : PortProductResolverBase<WasteSortingPlant>
  {
    public WasteSortingPlantPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      WasteSortingPlant entity,
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
      WasteSortingPlantProto sortingPlantProto = (WasteSortingPlantProto) proto;
      if (portSpec.Type == IoPortType.Input)
        return sortingPlantProto.SupportedInputs.Where((Func<ProductQuantity, bool>) (x => x.Product.Type == portSpec.Shape.AllowedProductType)).Select<ProductQuantity, ProductProto>((Func<ProductQuantity, ProductProto>) (x => x.Product)).ToImmutableArray<ProductProto>();
      if (portSpec.Type == IoPortType.Output)
      {
        IRecipeForUi recipeForUi = sortingPlantProto.Recipes.First<IRecipeForUi>();
        return recipeForUi == null ? ImmutableArray<ProductProto>.Empty : recipeForUi.AllUserVisibleOutputs.Map<ProductProto>((Func<RecipeOutput, ProductProto>) (x => x.Product));
      }
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
