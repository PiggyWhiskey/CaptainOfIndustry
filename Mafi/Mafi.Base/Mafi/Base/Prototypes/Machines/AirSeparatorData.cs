// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.AirSeparatorData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class AirSeparatorData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Air separator", Ids.Machines.AirSeparator).Description("Performs a cryogenic distillation process at temperatures reaching -200 °C to separate atmospheric air into its primary components - oxygen and nitrogen.", "short description of a machine").SetCost(Costs.Machines.AirSeparator).SetElectricityConsumption(250.Kw()).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h + 2;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "[4][4][4][4][4][4][4][5][1]   ", "[4][4][4][4][9][9][9][6][6]>@X", "[4][4][4][9][9![9![9![9![3]   ", "[4][4][4][9][9![9![9![5][3]   ", "[4][4][4][4][9][9][9][6][6]>@Y", "[4][4][4][4][4][4][4][5][1]   ").SetPrefabPath("Assets/Base/Machines/MetalWorks/AirSeparator.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()))).AddParticleParams(ParticlesParams.Loop("Steam")).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Air separation", Ids.Recipes.AirSeparation, machine).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Oxygen, "Y").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Nitrogen, "X").BuildAndAdd();
    }

    public AirSeparatorData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
