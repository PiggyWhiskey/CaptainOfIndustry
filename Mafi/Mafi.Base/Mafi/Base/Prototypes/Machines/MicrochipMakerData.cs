// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.MicrochipMakerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class MicrochipMakerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration pauseAt = Duration.FromKeyframes((int) sbyte.MaxValue);
      Duration totalDuration = Duration.FromKeyframes(280);
      string enUs = "The most sophisticated manufacturing processes where a thin monocrystalline wafer is slowly transformed into a matrix of microchips. Chips are built from many layers where each layer has to be placed with nanometer precision. This is performed in a special chamber that employs ultraviolet technology - substances reacting with light to form the layers. Microchips typically go through lot of stages including washing and coating in between. It is good to start small and then expand. Small setups can be connected in form of a loop with a sorter.";
      LocStr desc = Loc.Str(Ids.Machines.MicrochipMachine.ToString() + "__desc", enUs, "description of a machine. For the curious ones :) => https://www.youtube.com/watch?v=g8Qav3vIv9s");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Microchip machine II", Ids.Machines.MicrochipMachineT2).Description(desc).SetCost(Costs.Machines.MicrochipMachineT2).SetElectricityConsumption(400.Kw()).DisableBoost().SetComputingConsumption(Computing.FromTFlops(6)).SetCategories(Ids.ToolbarCategories.Machines).SetLayout("      D@vF#vB#vC#vE@v      ", "   [2][2][2][2][2][2][2]   ", "A#>[2][2][3][3][3][2][2]>#X", "   [2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Electronics/MicrochipMachineT2.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).EnableSemiInstancedRendering().BuildAndAdd();
      MicrochipMakerData.createMicrochipRecipes(registrator, machineProto, 15.Seconds());
      MachineProto machine = registrator.MachineProtoBuilder.Start("Microchip machine", Ids.Machines.MicrochipMachine).Description(desc).SetCost(Costs.Machines.MicrochipMachine).SetElectricityConsumption(200.Kw()).DisableBoost().SetNextTier(machineProto).SetComputingConsumption(Computing.FromTFlops(3)).SetCategories(Ids.ToolbarCategories.Machines).SetLayout("      D@vF#vB#vC#vE@v      ", "   [2][2][2][2][2][2][2]   ", "A#>[2][2][3][3][3][2][2]>#X", "   [2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Electronics/MicrochipMachineT1.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).EnableSemiInstancedRendering().SetProtoToCopyAnimationsFrom(Ids.Machines.MicrochipMachineT2).AddMaterialSwapForAnimationsLoad("MicrochipMachineT1", "MicrochipMachineT2").BuildAndAdd();
      MicrochipMakerData.createMicrochipRecipes(registrator, machine, 30.Seconds());
    }

    private static void createMicrochipRecipes(
      ProtoRegistrator registrator,
      MachineProto machine,
      Duration duration)
    {
      ProductProto.ID[] idArray = new ProductProto.ID[11]
      {
        Ids.Products.MicrochipsStage1A,
        Ids.Products.MicrochipsStage1B,
        Ids.Products.MicrochipsStage1C,
        Ids.Products.MicrochipsStage2A,
        Ids.Products.MicrochipsStage2B,
        Ids.Products.MicrochipsStage2C,
        Ids.Products.MicrochipsStage3A,
        Ids.Products.MicrochipsStage3B,
        Ids.Products.MicrochipsStage3C,
        Ids.Products.MicrochipsStage4A,
        Ids.Products.MicrochipsStage4B
      };
      ProductProto.ID productId1 = Ids.Products.SiliconWafer;
      for (int stage = 0; stage < idArray.Length; ++stage)
      {
        ProductProto.ID productId2 = idArray[stage];
        RecipeProto.ID microchipManufacturingId = Ids.Recipes.GetMicrochipManufacturingId(stage, (Proto.ID) machine.Id);
        RecipeProtoBuilder.State builder = registrator.RecipeProtoBuilder.Start(string.Format("Microchip manufacturing stage {0}{1}", (object) (stage % 3 + 1), (object) (char) (65 + stage / 3)), microchipManufacturingId, machine).SetDuration(duration).AddInput<RecipeProtoBuilder.State>(stage == 0 ? 2 : 1, productId1, "A").AddOutput<RecipeProtoBuilder.State>(1, productId2, "X");
        RecipeProtoBuilder.State state;
        switch (stage % 3)
        {
          case 0:
            state = builder.AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Water);
            break;
          case 1:
            state = builder.AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Copper, "FBC").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Plastic, "FBC");
            break;
          case 2:
            state = builder.AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Gold, "FBC");
            break;
          default:
            throw new ProtoBuilderException(string.Format("Invalid Stage ID {0}", (object) stage));
        }
        state.BuildAndAdd();
        productId1 = productId2;
      }
      registrator.RecipeProtoBuilder.Start("Microchip manufacturing final stage", new RecipeProto.ID(string.Format("{0}_MicrochipProdFinalStage", (object) machine.Id)), machine).AddInput<RecipeProtoBuilder.State>(1, productId1, "A").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Gold, "FBC").SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Microchips, "X").BuildAndAdd();
    }

    public MicrochipMakerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
