// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CoolingTowerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class CoolingTowerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration durationTicks = 10.Seconds();
      LocStr desc = Loc.Str(Ids.Machines.CoolingTowerT1.ToString() + "__desc", "Improves water efficiency of a power plant by recovering some steam back to water.", "description of a machine");
      MachineProto machine1 = registrator.MachineProtoBuilder.Start("Cooling tower", Ids.Machines.CoolingTowerT1).Description(desc).SetCost(Costs.Machines.CoolingTower).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [4][4][4][4][4]   ", "   [4][7][7][7][4]   ", "A@>[4][7][7][7][4]>@X", "   [4][7][7][7][4]   ", "   [4][4][4][4][4]   ").AddParticleParams(ParticlesParams.Loop("Steam", true)).UseAllRecipesAtStartOrAfterUnlock().DisableLogisticsByDefault().SetPrefabPath("Assets/Base/Machines/PowerPlant/CoolingTowerT1.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Hi-press steam condensation", Ids.Recipes.SteamHpCondensation, machine1).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.SteamHi).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Water).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Lo-press steam condensation", Ids.Recipes.SteamLpCondensation, machine1).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.SteamLo).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Water).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Depleted steam condensation", Ids.Recipes.SteamDepletedCondensation, machine1).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.SteamDepleted).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Water).BuildAndAdd();
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Cooling tower (large)", Ids.Machines.CoolingTowerT2).Description(desc).SetCost(Costs.Machines.CoolingTowerT2).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 2 * h + 1;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "      [1![1![2![2![2![1![1!      ", "   [1![2![2![5![5![5![2![2![1!   ", "   [1![2![5![5![5![5![5![2![1!   ", "A@>[2![5![5![5![5![5![5![5![2!>@X", "B@>[2![5![5![5![5![5![5![5![2!>@Y", "C@>[2![5![5![5![5![5![5![5![2!>@Z", "   [1![2![5![5![5![5![5![2![1!   ", "   [1![2![2![5![5![5![2![2![1!   ", "      [1![1![2![2![2![1![1!      ").AddParticleParams(ParticlesParams.Loop("Steam", true)).UseAllRecipesAtStartOrAfterUnlock().DisableLogisticsByDefault().SetPrefabPath("Assets/Base/Machines/PowerPlant/CoolingTowerT2.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Super-press steam condensation", Ids.Recipes.SteamSpCondensationT2, machine2).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.SteamSp).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Water).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Hi-press steam condensation", Ids.Recipes.SteamHpCondensationT2, machine2).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.SteamHi).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.Water).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Lo-press steam condensation", Ids.Recipes.SteamLpCondensationT2, machine2).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.SteamLo).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Water).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Depleted steam condensation", Ids.Recipes.SteamDepletedCondensationT2, machine2).EnablePartialExecution(25.Percent()).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.SteamDepleted).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Water).BuildAndAdd();
    }

    public CoolingTowerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
