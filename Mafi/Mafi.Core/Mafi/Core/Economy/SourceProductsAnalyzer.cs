// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.SourceProductsAnalyzer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Economy
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class SourceProductsAnalyzer
  {
    private readonly ImmutableArray<RecipeProto> m_allRecipes;
    private readonly Set<ProductProto> m_visitedProducts;
    private readonly Set<ProductProto> m_terminalProducts;
    private readonly Set<ProductProto> m_recipesWithProductsToSkip;
    private readonly Set<ProductProto> m_ignoreProducts;
    private readonly Dict<ProductProto, PartialQuantity> m_tempResult;
    private readonly Dict<ProductProto, ImmutableArray<PartialProductQuantity>> m_cache;

    public SourceProductsAnalyzer(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_visitedProducts = new Set<ProductProto>();
      this.m_terminalProducts = new Set<ProductProto>();
      this.m_recipesWithProductsToSkip = new Set<ProductProto>();
      this.m_ignoreProducts = new Set<ProductProto>();
      this.m_tempResult = new Dict<ProductProto, PartialQuantity>();
      this.m_cache = new Dict<ProductProto, ImmutableArray<PartialProductQuantity>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_allRecipes = protosDb.All<RecipeProto>().ToImmutableArray<RecipeProto>();
      string[] strArray1 = new string[24]
      {
        "Iron",
        "Copper",
        "Steel",
        "Sulfur",
        "Plastic",
        "Rubber",
        "Gold",
        "ConcreteSlab",
        "Wood",
        "Glass",
        "Water",
        "PolySilicon",
        "Acid",
        "Limestone",
        "Rock",
        "Ethanol",
        "Ammonia",
        "Diesel",
        "Coal",
        "Sugar",
        "HydrogenFluoride",
        "FoodPack",
        "Slag",
        "FuelGas"
      };
      foreach (string str in strArray1)
      {
        ProductProto proto;
        if (protosDb.TryGetProto<ProductProto>(new Proto.ID("Product_" + str), out proto))
          this.m_terminalProducts.Add(proto);
      }
      string[] strArray2 = new string[1]{ "Bricks" };
      foreach (string str in strArray2)
      {
        ProductProto proto;
        if (protosDb.TryGetProto<ProductProto>(new Proto.ID("Product_" + str), out proto))
          this.m_recipesWithProductsToSkip.Add(proto);
      }
      string[] strArray3 = new string[4]
      {
        "Water",
        "Oxygen",
        "Acid",
        "Slag"
      };
      foreach (string str in strArray3)
      {
        ProductProto proto;
        if (protosDb.TryGetProto<ProductProto>(new Proto.ID("Product_" + str), out proto))
          this.m_ignoreProducts.Add(proto);
      }
      foreach (ProductProto productProto in protosDb.All<ProductProto>())
      {
        if (productProto.Id.Value.Contains("Ore") || productProto.Id.Value.Contains("Oil"))
          this.m_terminalProducts.Add(productProto);
      }
    }

    public void GetSourceProductsFor(
      ProductProto product,
      Quantity requiredQuantity,
      Lyst<ProductQuantity> result,
      bool returnTerminalOnly = false)
    {
      ImmutableArray<PartialProductQuantity> immutableArray;
      if (!this.m_cache.TryGetValue(product, out immutableArray))
      {
        this.m_tempResult.Clear();
        this.getSourceProductsForInternal(product, 1.Quantity().AsPartial, this.m_tempResult);
        immutableArray = this.m_tempResult.Select<KeyValuePair<ProductProto, PartialQuantity>, PartialProductQuantity>((Func<KeyValuePair<ProductProto, PartialQuantity>, PartialProductQuantity>) (x => new PartialProductQuantity(x.Key, x.Value))).ToImmutableArray<PartialProductQuantity>();
        this.m_cache.Add(product, immutableArray);
      }
      result.Clear();
      foreach (PartialProductQuantity partialProductQuantity in immutableArray)
      {
        if (!this.m_ignoreProducts.Contains(partialProductQuantity.Product) && (!returnTerminalOnly || this.m_terminalProducts.Contains(partialProductQuantity.Product)))
        {
          ProductQuantity productQuantity = partialProductQuantity.Product.WithQuantity((requiredQuantity.Value * partialProductQuantity.Quantity).ToQuantityRounded());
          result.Add(productQuantity);
        }
      }
    }

    private void getSourceProductsForInternal(
      ProductProto product,
      PartialQuantity requiredQuantity,
      Dict<ProductProto, PartialQuantity> result)
    {
      if (this.m_terminalProducts.Contains(product))
        return;
      this.m_visitedProducts.Add(product);
      foreach (RecipeProto allRecipe in this.m_allRecipes)
      {
        RecipeOutput recipeOutput = allRecipe.AllOutputs.FirstOrDefault((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) product));
        if (recipeOutput != null)
        {
          Fix32 fix32 = requiredQuantity.Value / recipeOutput.Quantity.Value;
          if (!allRecipe.AllInputs.Any((Func<RecipeInput, bool>) (x => this.m_recipesWithProductsToSkip.Contains(x.Product))))
          {
            foreach (RecipeInput allInput in allRecipe.AllInputs)
            {
              if (!this.m_visitedProducts.Contains(allInput.Product))
              {
                PartialQuantity requiredQuantity1 = allInput.Quantity.AsPartial * fix32;
                result[allInput.Product] = result.GetOrCreate<ProductProto, PartialQuantity>(allInput.Product, (Func<PartialQuantity>) (() => PartialQuantity.Zero)) + requiredQuantity1;
                if (!this.m_terminalProducts.Contains(allInput.Product))
                  this.getSourceProductsForInternal(allInput.Product, requiredQuantity1, result);
              }
            }
            this.m_visitedProducts.Remove(product);
            return;
          }
        }
      }
      this.m_visitedProducts.Remove(product);
    }
  }
}
