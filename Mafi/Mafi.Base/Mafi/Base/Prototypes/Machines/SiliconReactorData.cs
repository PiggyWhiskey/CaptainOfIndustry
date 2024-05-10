// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SiliconReactorData
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
  internal class SiliconReactorData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Silicon reactor", Ids.Machines.SiliconReactor).Description("Purifies silicon using hydrogen. The processed silicon can be used in electronics.", "short description of a machine").SetCost(Costs.Machines.SiliconReactor).SetElectricityConsumption(80.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("B@>[3][3][3]      ", "A'>[3][3][3][1]>#X", "   [3][3][3]      ").SetPrefabPath("Assets/Base/Machines/Electronics/SiliconReactor.prefab").SetMachineSound("Assets/Base/Machines/Electronics/SiliconReactor/SiliconReactorSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(Duration.FromKeyframes(240))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Silicon treatment", Ids.Recipes.SiliconTreatment, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.MoltenSilicon).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Hydrogen).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.PolySilicon).BuildAndAdd();
    }

    public SiliconReactorData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
