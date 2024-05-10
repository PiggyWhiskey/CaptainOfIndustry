// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.MachinePortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MachinePortProductResolver : PortProductResolverBase<Machine>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly Lyst<RecipeProto> m_recipesCache;

    public MachinePortProductResolver(ProtosDb protosDb, UnlockedProtosDb unlockedProtosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_recipesCache = new Lyst<RecipeProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      Machine entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      if (!considerAllUnlockedRecipes && (!fallbackToUnlockedIfNoRecipesAssigned || !entity.RecipesAssigned.IsEmpty<RecipeProto>()))
        return MachinePortProductResolver.computeMapping(entity.RecipesAssigned, port.Spec);
      this.m_recipesCache.Clear();
      foreach (RecipeProto recipe in entity.Prototype.Recipes)
      {
        if (this.m_unlockedProtosDb.IsUnlocked((Proto) recipe))
          this.m_recipesCache.Add(recipe);
      }
      return MachinePortProductResolver.computeMapping((IIndexable<RecipeProto>) this.m_recipesCache, port.Spec);
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      return MachinePortProductResolver.computeMapping(((MachineProto) proto).Recipes, portSpec);
    }

    private static ImmutableArray<ProductProto> computeMapping(
      IIndexable<RecipeProto> recipes,
      PortSpec portSpec)
    {
      Lyst<ProductProto> lyst = new Lyst<ProductProto>();
      foreach (RecipeProto recipe in recipes)
      {
        if (portSpec.Type == IoPortType.Input)
        {
          foreach (RecipeInput allInput in recipe.AllInputs)
          {
            if (MachinePortProductResolver.anyPortMatches(portSpec, (RecipeProduct) allInput))
              lyst.AddIfNotPresent(allInput.Product);
          }
        }
        else if (portSpec.Type == IoPortType.Output)
        {
          foreach (RecipeOutput allOutput in recipe.AllOutputs)
          {
            if (MachinePortProductResolver.anyPortMatches(portSpec, (RecipeProduct) allOutput))
              lyst.AddIfNotPresent(allOutput.Product);
          }
        }
      }
      return lyst.ToImmutableArrayAndClear();
    }

    private static bool anyPortMatches(PortSpec port, RecipeProduct recipeProduct)
    {
      foreach (IoPortTemplate port1 in recipeProduct.Ports)
      {
        if ((int) port1.Spec.Name == (int) port.Name && (Proto) port1.Spec.Shape == (Proto) port.Shape)
          return true;
      }
      return false;
    }
  }
}
