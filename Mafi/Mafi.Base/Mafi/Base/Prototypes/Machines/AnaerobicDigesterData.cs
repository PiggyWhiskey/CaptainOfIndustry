// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.AnaerobicDigesterData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class AnaerobicDigesterData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto digester = registrator.MachineProtoBuilder.Start("Anaerobic digester", Ids.Machines.AnaerobicDigester).Description("Performs a process in which microorganisms break down biodegradable material in the absence of oxygen to produce fuels and fertilizer.", "short description of a machine").SetElectricityConsumption(50.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetCost(Costs.Machines.AnaerobicDigester).SetLayout("      [4][4][4][4][4][4][2]   ", "   [2][4][5][5][5][5][4][2]>~X", "A~>[4][5][5][5][5][5][4][2]   ", "B#>[4][5][5][5][5][5][4][2]   ", "   [2][4][5][5][5][5][4][2]>@Y", "      [4][4][4][4][4][4]      ").SetPrefabPath("Assets/Base/Machines/Oil/AnaerobicDigester.prefab").SetMachineSound("Assets/Base/Machines/Oil/AnaerobicDigester/AnaerobicDigesterSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      addDigestion(Ids.Recipes.SludgeDigestion, Ids.Products.Sludge, 40.Seconds(), 12, 5, 1);
      addDigestion(Ids.Recipes.PotatoDigestion, Ids.Products.Potato, 60.Seconds(), 14, 6, 1);
      addDigestion(Ids.Recipes.VegetablesDigestion, Ids.Products.Vegetables, 60.Seconds(), 14, 6, 1);
      addDigestion(Ids.Recipes.FruitDigestion, Ids.Products.Fruit, 90.Seconds(), 12, 10, 1);
      addDigestion(Ids.Recipes.PoppyDigestion, Ids.Products.Poppy, 60.Seconds(), 14, 6, 1);
      addDigestion(Ids.Recipes.WheatDigestion, Ids.Products.Wheat, 90.Seconds(), 11, 10, 1);
      addDigestion(Ids.Recipes.MeatTrimmingsDigestion, Ids.Products.MeatTrimmings, 120.Seconds(), 8, 4, 2);

      void addDigestion(
        RecipeProto.ID id,
        ProductProto.ID productIn,
        Duration duration,
        int quantityIn,
        int gasOut,
        int compostOut)
      {
        registrator.RecipeProtoBuilder.Start("Digestion", id, digester).AddInput<RecipeProtoBuilder.State>(quantityIn, productIn).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(gasOut, Ids.Products.FuelGas, "Y").AddOutput<RecipeProtoBuilder.State>(compostOut, Ids.Products.Compost, "X").BuildAndAdd();
      }
    }

    public AnaerobicDigesterData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
