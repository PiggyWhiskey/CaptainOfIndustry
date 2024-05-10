// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WaterPumpsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class WaterPumpsData : IModData
  {
    public const int SMALL_PUMP_MAX_HEIGHT = 5;
    public const int LARGE_PUMP_MAX_HEIGHT = 11;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      OceanWaterPumpProto machine1 = prototypesDb.Add<OceanWaterPumpProto>(new OceanWaterPumpProto(Ids.Machines.OceanWaterPumpT1, Proto.CreateStrFormatDesc1((Proto.ID) Ids.Machines.OceanWaterPumpT1, "Seawater pump", "Pumps water from ocean. Works at the maximum height of {0} from the ocean level.", new LocStrFormatted(5.ToString()), "{0} is an integer specifying max height such as '5'"), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[4]
      {
        new CustomLayoutToken("-0~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h - 1, -h + 2, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("-0}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h - 1, -h + 2))),
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-13, -10, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("{P}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-5, 2, maxTerrainHeight: new int?(0))))
      }), "                                    [2][2]         ", "-6~-5}-4}-3}-2}-1}-1}{P}{2}{2}{P}{2}[2][2][2][2]>@X", "-6~-5}-4}-3}-2}-1}-1}{P}{2}{2}{P}{2}[2][2][2][2]   ", "                                 {1}[2][2][2][2]   "), Costs.Machines.OceanWaterPump.MapToEntityCosts(registrator), 100.Kw(), ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop()), ImmutableArray.Create<ImmutableArray<RectangleTerrainArea2iRelative>>(ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-17, -2), new RelTile2i(3, 4)))), new HeightTilesI(1), new HeightTilesI(5), new MachineProto.Gfx("Assets/Base/Machines/Pump/OceanWaterPumpT1.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesWater), useSemiInstancedRendering: true)));
      registrator.RecipeProtoBuilder.Start("Ocean water pumping", Ids.Recipes.OceanWaterPumping, (MachineProto) machine1).AddOutput<RecipeProtoBuilder.State>(18, Ids.Products.Seawater, "X").SetDuration(10.Seconds()).BuildAndAdd();
      OceanWaterPumpProto machine2 = prototypesDb.Add<OceanWaterPumpProto>(new OceanWaterPumpProto(Ids.Machines.OceanWaterPumpLarge, Proto.CreateStrFormatDesc1((Proto.ID) Ids.Machines.OceanWaterPumpLarge, "Seawater pump (tall)", "Larger pump that can be placed up to height of {0} from the ocean level. Requires more power to run.", new LocStrFormatted(11.ToString()), "{0} is an integer specifying max height such as '10'"), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[4]
      {
        new CustomLayoutToken("-0~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h - 6, -h - 3, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("-0}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h - 6, -h - 3))),
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-22, -19, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("{P}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-9, 2, maxTerrainHeight: new int?(0))))
      }), "                                                         {1}(2)[2][2][2]   ", "-6~-5}-4}-3}-2}-1}-5}-4}-3}-2}-1}{1}{1}{1}{1}{P}{2}{2}{P}{2}(2)[2][2][2]>@X", "-6~-5}-4}-3}-2}-1}-5}-4}-3}-2}-1}{1}{1}{1}{1}{P}{2}{2}{P}{2}(2)[2][2][2]   ", "                                                         {1}(2)[2][2][2]   "), Costs.Machines.OceanWaterPumpLarge.MapToEntityCosts(registrator), 300.Kw(), ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop()), ImmutableArray.Create<ImmutableArray<RectangleTerrainArea2iRelative>>(ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-25, -1), new RelTile2i(3, 4)))), new HeightTilesI(1), new HeightTilesI(11), new MachineProto.Gfx("Assets/Base/Machines/Pump/OceanWaterPumpT2.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesWater), useSemiInstancedRendering: true)));
      registrator.RecipeProtoBuilder.Start("Ocean water pumping II", Ids.Recipes.OceanWaterPumpingT2, (MachineProto) machine2).AddOutput<RecipeProtoBuilder.State>(18, Ids.Products.Seawater, "X").SetDuration(10.Seconds()).BuildAndAdd();
    }

    public WaterPumpsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
