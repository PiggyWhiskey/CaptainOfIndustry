// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.BoilersData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class BoilersData : IModData
  {
    public const int WATER_IN = 8;
    public const int STEAM_OUT = 8;
    public static readonly Duration DURATION;

    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine1 = registrator.MachineProtoBuilder.Start("Boiler", Ids.Machines.BoilerCoal).Description("Produces high pressure steam by burning loose products (such as coal) to boil water.", "short description of a machine").SetCost(Costs.Machines.Boiler).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [2][2][2][2][2]   ", "B@>[3][3][3][3][3]   ", "A~>[3][3][3][3][3]>@X", "   [3][3][3][3][3]   ", "         v@Y         ").SetPrefabPath("Assets/Base/Machines/Water/BoilerCoal.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).AddParticleParams(ParticlesParams.Loop("Sparks")).SetMachineSound("Assets/Base/Machines/Water/BoilerCoal/CoalBoiler_Sound.prefab").SetEmissionWhenWorking(3).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationCoal, machine1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationWood, machine1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Woodchips).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationBiomass, machine1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.Biomass).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationAnimalFeed, machine1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.AnimalFeed).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust, "Y").BuildAndAdd();
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Boiler (gas)", Ids.Machines.BoilerGas).Description("Produces high pressure steam by burning gas.", "short description of a machine").SetCost(Costs.Machines.BoilerGas).SetCategories(Ids.ToolbarCategories.MachinesWater).DisableLogisticsByDefault().UseAllRecipesAtStartOrAfterUnlock().SetLayout("   [2][2][2][2][2]   ", "B@>[3][3][3][3][3]   ", "A@>[3][3][3][3][3]>@X", "   [3][3][3][3][3]   ", "         v@Y         ").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetPrefabPath("Assets/Base/Machines/Water/BoilerGas.prefab").SetEmissionWhenWorking(3).SetMachineSound("Assets/Base/Machines/Water/BoilerCoal/CoalBoiler_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationHeavyOil, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(6, Ids.Products.HeavyOil, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(15, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationMediumOil, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(9, Ids.Products.MediumOil, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(15, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationLightOil, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(9, Ids.Products.LightOil, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationNaphtha, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(9, Ids.Products.Naphtha, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationEthanol, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(9, Ids.Products.Ethanol, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationFuelGas, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(12, Ids.Products.FuelGas, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationHydrogen, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water, "B").AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Hydrogen, "A").SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SteamHi, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.SteamDepleted, "Y").BuildAndAdd();
      MachineProto machine3 = registrator.MachineProtoBuilder.Start("Boiler (electric)", Ids.Machines.BoilerElectric).Description("Produces high pressure steam by boiling water. Basically a giant kettle. But not recommended for tea preparation.", "short description of a machine").SetCost(Costs.Machines.BoilerElectric).SetElectricityConsumption(8000.Kw()).DisableBoost().SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [2][2][2][2][2]   ", "B@>[3][3][3][3][3]   ", "   [3][3][3][3][3]>@X", "   [3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/Water/BoilerElectric.prefab").SetMachineSound("Assets/Base/Machines/Water/BoilerElectric/BoilerElectricSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      Assert.That<int>(0).IsZero();
      Assert.That<int>(0).IsZero();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationElectric, machine3).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.SteamHi, "X").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steam generation", Ids.Recipes.SteamGenerationSpElectric, machine3).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water).SetDuration(BoilersData.DURATION).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamSp, "X").BuildAndAdd();
    }

    public BoilersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BoilersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      BoilersData.DURATION = 10.Seconds();
    }
  }
}
