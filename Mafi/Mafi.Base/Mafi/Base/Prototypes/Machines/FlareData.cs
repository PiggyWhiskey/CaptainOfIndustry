// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FlareData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class FlareData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Flare", Ids.Machines.Flare).Description("Can burn combustible liquids and gasses but produces pollution.", "short description of a machine").SetCost(Costs.Machines.Flare).SetCategories(Ids.ToolbarCategories.Waste).SetAsWasteDisposal().DisableLogisticsByDefault().UseAllRecipesAtStartOrAfterUnlock().SetLayout("A@>[8]").AddParticleParams(ParticlesParams.Loop("FireSmokeDark", recipesSelector: new Func<RecipeProto, bool>(isDarkSmoke))).AddParticleParams(ParticlesParams.Loop("FireSmokeGray", recipesSelector: new Func<RecipeProto, bool>(isGraySmoke))).AddParticleParams(ParticlesParams.Loop("FireClean", recipesSelector: (Func<RecipeProto, bool>) (r => !isDarkSmoke(r) && !isGraySmoke(r)))).SetPrefabPath("Assets/Base/Machines/Waste/Flare.prefab").SetMachineSound("Assets/Base/Machines/Oil/Flares/FlareSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      this.addDisposal(registrator, machine, Ids.Recipes.FlareDiesel, Ids.Products.Diesel, 12, 10);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareHeavyOil, Ids.Products.HeavyOil, 8, 10);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareLightOil, Ids.Products.LightOil, 12, 8);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareNaphtha, Ids.Products.Naphtha, 12, 8);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareEthanol, Ids.Products.Ethanol, 12, 4);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareFuelGas, Ids.Products.FuelGas, 16, 4);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareAmmonia, Ids.Products.Ammonia, 12, 8);
      this.addDisposal(registrator, machine, Ids.Recipes.FlareHydrogen, Ids.Products.Hydrogen, 16, 0);

      static bool isDarkSmoke(RecipeProto r)
      {
        return r.Id == Ids.Recipes.FlareDiesel || r.Id == Ids.Recipes.FlareHeavyOil;
      }

      static bool isGraySmoke(RecipeProto r)
      {
        return r.Id == Ids.Recipes.FlareLightOil || r.Id == Ids.Recipes.FlareNaphtha;
      }
    }

    private void addDisposal(
      ProtoRegistrator registrator,
      MachineProto machine,
      RecipeProto.ID recipeId,
      ProductProto.ID productId,
      int quantityIn,
      int pollution)
    {
      string translatedString = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) productId).Strings.Name.TranslatedString;
      RecipeProtoBuilder.State builder = registrator.RecipeProtoBuilder.Start(translatedString + " disposal", recipeId, machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(quantityIn, productId).EnablePartialExecution(1.Percent()).SetDurationSeconds(20);
      if (pollution > 0)
        builder = builder.AddAirPollution<RecipeProtoBuilder.State>(pollution.Quantity());
      builder.BuildAndAdd();
    }

    public FlareData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
