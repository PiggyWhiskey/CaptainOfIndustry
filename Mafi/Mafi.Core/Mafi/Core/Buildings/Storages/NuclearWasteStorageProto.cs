// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.NuclearWasteStorageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public class NuclearWasteStorageProto : StorageProto, IProtoWithUiRecipes
  {
    public readonly int? EmissionIntensity;
    public readonly Quantity RetiredWasteCapacity;

    public override Type EntityType => typeof (NuclearWasteStorage);

    public IIndexable<IRecipeForUi> Recipes { get; private set; }

    public NuclearWasteStorageProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Func<ProductProto, bool> productsFilter,
      Mafi.Core.Products.ProductType? productType,
      Quantity capacity,
      Quantity retiredWasteCapacity,
      EntityCosts costs,
      Option<StorageProto> nextTier,
      LayoutEntityProto.Gfx graphics,
      int? emissionIntensity,
      Electricity powerConsumedForProductsExchange,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, productsFilter, productType, capacity, costs, 4.Quantity(), 1.Seconds() / 5, powerConsumedForProductsExchange, nextTier, graphics, tags);
      this.RetiredWasteCapacity = retiredWasteCapacity;
      this.EmissionIntensity = emissionIntensity;
      if (this.RetiredWasteCapacity.IsNotPositive)
        throw new ProtoBuilderException("RetiredWasteCapacity is not positive!");
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>();
      foreach (ProductProto product in protosDb.Filter<ProductProto>((Func<ProductProto, bool>) (x => x.Radioactivity > 0)))
      {
        RadioactiveWasteParam paramValue;
        if (product.TryGetParam<RadioactiveWasteParam>(out paramValue) && paramValue.YearsUntilSafeToDispose > 0)
        {
          ProductProto orThrow = protosDb.GetOrThrow<ProductProto>((Proto.ID) paramValue.TransferIntoProduct);
          RecipeForUiData recipeForUiData = new RecipeForUiData(paramValue.YearsUntilSafeToDispose.Years(), ImmutableArray.Create<RecipeInput>(new RecipeInput(product, 1.Quantity())), ImmutableArray.Create<RecipeOutput>(new RecipeOutput(orThrow, 1.Quantity())));
          lyst.Add((IRecipeForUi) recipeForUiData);
        }
      }
      this.Recipes = (IIndexable<IRecipeForUi>) lyst;
    }
  }
}
