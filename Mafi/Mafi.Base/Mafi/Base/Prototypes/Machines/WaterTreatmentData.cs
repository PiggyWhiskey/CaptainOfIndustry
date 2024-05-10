// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WaterTreatmentData
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
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class WaterTreatmentData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase = registrator.MachineProtoBuilder.Start("Wastewater treatment", Ids.Machines.WaterTreatmentPlant).Description("Converts not so nice water to nice water. Just don't tell this to the people who drink it.", "short description of a machine").SetCost(Costs.Machines.WaterTreatmentPlant).SetElectricityConsumption(600.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("                  [3][3][3][3]>~X      ^@Y^@Z            ", "                  [3][3][3][3]         [3][3][3]         ", "         [3][3][3][3][3][3][3][3]      [3][3][3]D@<      ", "      [3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "      [3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "A@>[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]C~<", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "B@>[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "      [3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "      [3][3][3][3][3][3][3][3][3][3][3][3][3][3]         ", "         [3][3][3][3][3][3][3][3]                        ").SetPrefabPath("Assets/Base/Machines/Water/WasteWaterTreatment.prefab").SetMachineSound("Assets/Base/Machines/Gold/SettlingTank/SettlingTankSound.prefab");
      Duration totalDuration = Duration.FromKeyframes(600);
      Duration pauseAt = Duration.FromKeyframes(280);
      Percent? nullable = new Percent?(120.Percent());
      Duration pauseDuration = new Duration();
      Percent? baseSpeed = nullable;
      AnimationWithPauseParams animParams = AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt, pauseDuration, baseSpeed);
      MachineProto machine = builderStateBase.SetAnimationParams((AnimationParams) animParams).AddParticleParams(ParticlesParams.Timed("WasteInlet", Duration.Zero, 7.Seconds(), colorSelector: (Func<RecipeProto, ColorRgba>) (r => r.AllInputs.First.Product.Graphics.Color))).AddParticleParams(ParticlesParams.Timed("WasteParticles", 7.Seconds(), 13.Seconds())).AddParticleParams(ParticlesParams.Timed("SprinklingParticles", 8.Seconds(), 18.Seconds())).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Water treatment", Ids.Recipes.WaterTreatment, machine).AddInput<RecipeProtoBuilder.State>(80, Ids.Products.WasteWater, "AB").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Chlorine, "D").SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(40, Ids.Products.Water).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Sludge).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Water treatment (advanced)", Ids.Recipes.WaterTreatmentT2, machine).AddInput<RecipeProtoBuilder.State>(80, Ids.Products.WasteWater, "AB").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.FilterMedia).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Chlorine, "D").SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(60, Ids.Products.Water).AddOutput<RecipeProtoBuilder.State>(18, Ids.Products.Sludge).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Toxic slurry treatment", Ids.Recipes.ToxicSlurryTreatment, machine).AddInput<RecipeProtoBuilder.State>(36, Ids.Products.ToxicSlurry, "AB").AddInput<RecipeProtoBuilder.State>(2, Ids.Products.FilterMedia).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Brine, "D").SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Water).AddOutput<RecipeProtoBuilder.State>(24, Ids.Products.Slag).BuildAndAdd();
    }

    public WaterTreatmentData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
