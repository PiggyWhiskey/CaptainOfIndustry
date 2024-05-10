// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.NuclearReprocessingPlantData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class NuclearReprocessingPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Nuclear reprocessing plant", Ids.Machines.NuclearFuelReprocessingPlant).Description("A complex facility which reprocesses radioactive material by isolating fission products (material that is no longer fissile and would slow down reaction if left in a reactor). Isolated waste decays faster, allowing it to be disposed of in a reasonable time span. The isolated waste is vitrified using molten glass into a solid form for easier storage.", "short description of a machine").SetCost(Costs.Machines.ReprocessingPlant).SetElectricityConsumption(1200.Kw()).SetComputingConsumption(Computing.FromTFlops(16)).SetCategories(Ids.ToolbarCategories.MachinesElectricity).SetLayout("   [4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]>@J", "A'>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ", "B~>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ", "   [4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]>#X", "C@>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ", "D@>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]>#Y", "   [4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ", "E#>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]>#Z", "F#>[4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ", "   [4][4][4][4][4][4][4][4][6][6][6][6][4][4][4][4][4][4][4][4][4]   ").SetPrefabPath("Assets/Base/Machines/PowerPlant/ReprocessingPlant.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(Duration.FromKeyframes(385), new Percent?(90.Percent()), new Duration?(4.Seconds()), "Assembly")).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), stateName: "Crusher")).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Spent fuel reprocessing", Ids.Recipes.SpentFuelReprocessing, machine).AddInput<RecipeProtoBuilder.State>(18, Ids.Products.SpentFuel).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.MoltenGlass).SetDurationSeconds(60).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.UraniumReprocessed, "X").AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Plutonium, "Y").AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.FissionProduct, "Z").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Spent fuel to blanket", Ids.Recipes.SpentFuelToBlanket, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SpentFuel).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.MoltenGlass).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.BlanketFuel).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.FissionProduct, "Z").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Spent mox to blanket", Ids.Recipes.SpentMoxToBlanket, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SpentMox).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.MoltenGlass).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.BlanketFuel).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.FissionProduct, "Z").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Core fuel reprocessing", Ids.Recipes.CoreFuelReprocessing, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.CoreFuelDirty).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.MoltenGlass).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).SetDurationSeconds(60).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.CoreFuel).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.FissionProduct, "Z").BuildAndAdd();
    }

    public NuclearReprocessingPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
