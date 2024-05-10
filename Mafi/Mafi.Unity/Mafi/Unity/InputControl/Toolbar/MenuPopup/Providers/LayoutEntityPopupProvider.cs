// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.LayoutEntityPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  internal class LayoutEntityPopupProvider : PopupProviderBase<LayoutEntityProto>
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly Lyst<IRecipeForUi> m_unlockedRecipes;
    private readonly Lyst<ProductProto> m_unlockedProducts;

    public LayoutEntityPopupProvider(UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unlockedRecipes = new Lyst<IRecipeForUi>();
      this.m_unlockedProducts = new Lyst<ProductProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    protected override void PopulateView(
      MenuPopupView view,
      LayoutEntityProto proto,
      bool isForResearch)
    {
      view.SetTitle((LocStrFormatted) proto.Strings.Name);
      view.SetPrice(proto);
      view.SetDescription((LocStrFormatted) proto.Strings.DescShort);
      if (proto.Costs.Workers > 0)
        view.AddWorkers(proto.Costs.Workers);
      if (proto is IProtoWithPowerConsumption powerConsumption && powerConsumption.ElectricityConsumed.IsPositive)
        view.AddPowerConsumption(powerConsumption.ElectricityConsumed);
      if (proto is IProtoWithUnityConsumption unityConsumption && unityConsumption.UnityMonthlyCost.IsPositive)
        view.AddUpointsConsumption(unityConsumption.UnityMonthlyCost);
      if (proto is IProtoWithComputingConsumption computingConsumption && computingConsumption.ComputingConsumed.IsPositive)
        view.AddComputingConsumption(computingConsumption.ComputingConsumed);
      if (proto is StorageProto storageProto)
      {
        this.m_unlockedProducts.Clear();
        foreach (ProductProto storableProduct in (IEnumerable<ProductProto>) storageProto.StorableProducts)
        {
          if (this.m_unlockedProtosDb.IsUnlocked((IProto) storableProduct))
            this.m_unlockedProducts.Add(storableProduct);
        }
        view.SetProducts(this.m_unlockedProducts, (LocStrFormatted) Tr.SupportedProducts);
      }
      if (proto is CargoDepotModuleProto depotModuleProto)
        view.SetThroughputPer60(depotModuleProto.QuantityPerExchange, depotModuleProto.DurationPerExchange);
      if (!isForResearch && proto is IProtoWithRecipes protoWithRecipes)
      {
        this.m_unlockedRecipes.Clear();
        foreach (RecipeProto recipe in protoWithRecipes.Recipes)
        {
          if (this.m_unlockedProtosDb.IsUnlocked((IProto) recipe))
            this.m_unlockedRecipes.Add((IRecipeForUi) recipe);
        }
        view.SetRecipes(this.m_unlockedRecipes);
      }
      if (!isForResearch && proto is IProtoWithUiRecipes protoWithUiRecipes)
      {
        this.m_unlockedRecipes.Clear();
        foreach (IRecipeForUi recipe in protoWithUiRecipes.Recipes)
          this.m_unlockedRecipes.Add(recipe);
        view.SetRecipes(this.m_unlockedRecipes);
      }
      if (isForResearch || !(proto is IProtoWithUiRecipe protoWithUiRecipe))
        return;
      this.m_unlockedRecipes.Clear();
      this.m_unlockedRecipes.Add(protoWithUiRecipe.Recipe);
      view.SetRecipes(this.m_unlockedRecipes);
    }
  }
}
