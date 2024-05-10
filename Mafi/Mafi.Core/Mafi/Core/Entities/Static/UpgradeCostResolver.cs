// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.UpgradeCostResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Helps to resolve upgrade costs.
  /// 
  /// Here is a simple example:
  /// Current entity cost: 20 CP
  /// Upgraded entity cost: 20 CP2
  /// There is a following recipe: 20 CP + 10 Kittens =&gt; 10 CP2
  /// The new upgrade cost will be: 10 CP2 + 10 Kittens
  /// - because the previous 20 CP can convert to 10 CP2 + 10 Kittens
  /// - this avoid previous situation where upgrading entity was more expensive than rebuilding it entirely
  /// 
  /// However there are quite few limits, e.g. we can't at this stage solve 20 CP -&gt; 20 CP3
  /// as this requires two hops. It would also get kinda strange with all the semi-products
  /// piling up. So try to avoid jumps like this.
  /// 
  /// We also only try to resolve product marked with <see cref="T:Mafi.Core.Entities.Static.AllowProductDiscountInUpgrade" />.
  /// The reason is that we are not interested in resolving things like lab equipment that is part
  /// of labs cost.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UpgradeCostResolver
  {
    private readonly Dict<ProductProto, Quantity> m_costWasted;
    private readonly Dict<ProductProto, Quantity> m_costToPay;
    private readonly Dict<ProductProto, Quantity> m_resultDict;
    private readonly Lyst<ProductQuantity> m_resultPrice;
    private readonly ImmutableArray<RecipeProto> m_allRecipes;
    private readonly Set<ProductProto> m_productsToDiscount;

    public UpgradeCostResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_costWasted = new Dict<ProductProto, Quantity>();
      this.m_costToPay = new Dict<ProductProto, Quantity>();
      this.m_resultDict = new Dict<ProductProto, Quantity>();
      this.m_resultPrice = new Lyst<ProductQuantity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_allRecipes = protosDb.All<RecipeProto>().ToImmutableArray<RecipeProto>();
      this.m_productsToDiscount = protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.TryGetParam<AllowProductDiscountInUpgrade>(out AllowProductDiscountInUpgrade _))).ToSet<ProductProto>();
    }

    public AssetValue GetUpgradeCost(
      AssetValue currentCost,
      AssetValue upgradedEntityCost,
      bool doNotIncludeSemiProducts = false)
    {
      this.m_costWasted.Clear();
      this.m_costToPay.Clear();
      this.m_resultDict.Clear();
      this.m_resultPrice.Clear();
      foreach (ProductQuantity product in currentCost.Products)
        this.m_costWasted[product.Product] = product.Quantity;
      foreach (ProductQuantity product in upgradedEntityCost.Products)
      {
        Quantity rhs;
        if (this.m_costWasted.TryGetValue(product.Product, out rhs))
        {
          Quantity quantity = product.Quantity.Min(rhs);
          this.m_costWasted[product.Product] = rhs - quantity;
          this.m_costToPay.Add(product.Product, product.Quantity - quantity);
        }
        else
          this.m_costToPay.Add(product.Product, product.Quantity);
      }
      if (this.m_costWasted.All<KeyValuePair<ProductProto, Quantity>>((Func<KeyValuePair<ProductProto, Quantity>, bool>) (x => x.Value.IsNotPositive)))
      {
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_costToPay)
        {
          if (keyValuePair.Value.IsPositive)
            this.m_resultPrice.Add(keyValuePair.Key.WithQuantity(keyValuePair.Value));
        }
        return new AssetValue(this.m_resultPrice.ToImmutableArray());
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_costToPay)
      {
        ProductProto toPayProduct = keyValuePair.Key;
        Quantity rhs1 = keyValuePair.Value;
        if (!rhs1.IsNotPositive)
        {
          if (!this.m_productsToDiscount.Contains(toPayProduct))
          {
            this.m_resultDict[toPayProduct] = rhs1 + this.m_resultDict.GetOrCreate<ProductProto, Quantity>(toPayProduct, (Func<Quantity>) (() => Quantity.Zero));
          }
          else
          {
            IEnumerable<RecipeProto> recipeProtos = this.m_allRecipes.Where((Func<RecipeProto, bool>) (x => x.AllOutputs.Length == 1 && (Proto) x.AllOutputs.First.Product == (Proto) toPayProduct));
            bool flag = false;
            foreach (RecipeProto recipeProto in recipeProtos)
            {
              foreach (RecipeInput allInput1 in recipeProto.AllInputs)
              {
                Quantity quantity1;
                if (this.m_costWasted.TryGetValue(allInput1.Product, out quantity1))
                {
                  if (quantity1.IsPositive)
                  {
                    Quantity quantity2 = recipeProto.AllOutputs.First.Quantity;
                    Quantity quantity3 = (quantity1.Value * quantity2 / allInput1.Quantity.Value).Min(rhs1);
                    this.m_costWasted[allInput1.Product] = quantity1 - quantity3 * allInput1.Quantity.Value / quantity2.Value;
                    Quantity quantity4 = rhs1 - quantity3;
                    if (quantity4.IsPositive)
                      this.m_resultDict[toPayProduct] = quantity4 + this.m_resultDict.GetOrCreate<ProductProto, Quantity>(toPayProduct, (Func<Quantity>) (() => Quantity.Zero));
                    if (!doNotIncludeSemiProducts)
                    {
                      foreach (RecipeInput allInput2 in recipeProto.AllInputs)
                      {
                        if (!((Proto) allInput2.Product == (Proto) allInput1.Product))
                        {
                          Quantity quantity5 = (quantity3.Value * allInput2.Quantity / quantity2.Value).Min(rhs1);
                          Quantity rhs2;
                          if (quantity5.IsPositive && this.m_costWasted.TryGetValue(allInput2.Product, out rhs2))
                          {
                            Quantity quantity6 = quantity5.Min(rhs2);
                            this.m_costWasted[allInput2.Product] = rhs2 - quantity6;
                            quantity5 -= quantity6;
                          }
                          if (quantity5.IsPositive)
                            this.m_resultDict[allInput2.Product] = quantity5 + this.m_resultDict.GetOrCreate<ProductProto, Quantity>(allInput2.Product, (Func<Quantity>) (() => Quantity.Zero));
                        }
                      }
                    }
                  }
                  flag = true;
                  break;
                }
              }
              if (flag)
                break;
            }
            if (!flag)
              this.m_resultDict[toPayProduct] = rhs1 + this.m_resultDict.GetOrCreate<ProductProto, Quantity>(toPayProduct, (Func<Quantity>) (() => Quantity.Zero));
          }
        }
      }
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_resultDict)
        this.m_resultPrice.Add(keyValuePair.Key.WithQuantity(keyValuePair.Value));
      return new AssetValue(this.m_resultPrice.ToImmutableArray());
    }
  }
}
