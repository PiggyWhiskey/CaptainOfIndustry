// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ExhaustScrubberData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class ExhaustScrubberData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr1 locStr1 = Loc.Str1(Ids.Machines.ExhaustScrubber.ToString() + "__desc", "Filters {0}% of pollutants from hot exhaust gasses by extracting useful resources.", "{0} is a number, used like for instance '75%'");
      MachineProto machine = registrator.MachineProtoBuilder.Start("Exhaust scrubber", Ids.Machines.ExhaustScrubber).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Machines.ExhaustScrubber.ToString() + "_formatted", locStr1.Format(75.ToString()).Value)).SetCost(Costs.Machines.FiltrationStation).SetElectricityConsumption(200.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("      [2][3][3][3][3][9][3]      ", "A@>[2][3][3][3][3][3][9][3]      ", "B@>[2][3][3][3][3][5][5][5]      ", "C@>[2][3][3][3][3][5][5][5][1]>@Y", "D@>[2][3][3][3][3][5][5][5]      ", "      [2][3][3][3][3][3]         ", "               v~Z   v@X         ").AddParticleParams(ParticlesParams.Loop("Smoke", true)).SetPrefabPath("Assets/Base/Machines/MetalWorks/FiltrationStation.prefab").SetMachineSound("Assets/Base/Machines/MetalWorks/FiltrationStation/FiltrationStationSound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Exhaust filtering", Ids.Recipes.ExhaustFiltering, machine).AddInput<RecipeProtoBuilder.State>(30, Ids.Products.Exhaust).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDuration(10.Seconds()).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Sulfur, "Z").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide, "X").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.SteamLo, "Y").AddAirPollutionFromExhaust<RecipeProtoBuilder.State>(4.Quantity()).BuildAndAdd();
    }

    public ExhaustScrubberData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
