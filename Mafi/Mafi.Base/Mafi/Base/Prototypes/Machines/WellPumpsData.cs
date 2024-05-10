// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WellPumpsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.WellPumps;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class WellPumpsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto orThrow = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CrudeOil);
      registrator.PrototypesDb.Add<VirtualResourceProductProto>(new VirtualResourceProductProto(IdsCore.Products.VirtualCrudeOil, orThrow.Strings, orThrow, true, new VirtualResourceProductProto.Gfx(ColorRgba.Black, 20.0.TilesThick())));
      LocStr locStr1 = Loc.Str("GroundReserveTooltip__Oil", "Shows the overall status of the ground reserve of crude oil in this deposit. This is a limited resource. We need to find a new one before we deplete it. Long-term source of crude oil needs to be found in the world map.", "tooltip");
      WellPumpProto machine1 = registrator.WellPumpProtoBuilder.Start("Oil pump", locStr1.TranslatedString, Ids.Machines.OilPump).Description("Pumps crude oil from underground.", "short description of a machine").SetLayout("[3][4][4][3][3]", "[3][4][4][4][4]", "[3][4][4][3][3]", "            v@O").SetPrefabPath("Assets/Base/Machines/Pump/OilPump.prefab").SetCost(Costs.Machines.OilPump).SetElectricityConsumption(80.Kw()).SetMinedProduct(IdsCore.Products.VirtualCrudeOil).SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetMachineSound("Assets/Base/Machines/Pump/OilPump/OilPump_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      machine1.AddParam((IProtoParam) DisableQuickBuildParam.Instance);
      registrator.RecipeProtoBuilder.Start("Oil pumping", Ids.Recipes.OilGroundPumping, (MachineProto) machine1).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.CrudeOil, "O").SetDuration(20.Seconds()).BuildAndAdd();
      registrator.PrototypesDb.Add<VirtualResourceProductProto>(new VirtualResourceProductProto(IdsCore.Products.Groundwater, Proto.CreateStr(IdsCore.Products.Groundwater, "Groundwater"), registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Water), false, new VirtualResourceProductProto.Gfx((ColorRgba) 8296183, 5.0.TilesThick())));
      LocStr locStr2 = Loc.Str("GroundReserveTooltip__Groundwater", "Shows the overall status of the reserve of groundwater. Groundwater is replenished during rain and can temporarily run out if pumped out too much.", "tooltip");
      LocStr desc = Loc.Str(Ids.Machines.LandWaterPump.Value + "__desc", "Pumps water from the ground deposit which is replenished during rain. Has to be built on top of a groundwater deposit.", "description of ground water pump");
      WellPumpProto machine2 = registrator.WellPumpProtoBuilder.Start("Groundwater pump", locStr2.TranslatedString, Ids.Machines.LandWaterPump).Description(desc).SetCost(Costs.Machines.LandWaterPump).SetElectricityConsumption(120.Kw()).SetMinedProduct(IdsCore.Products.Groundwater).NotifyWhenBelow(40.Percent()).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("[2][7][7][2]   ", "[2][7][7][2]   ", "[2][7][7][2]   ", "[2][4][4][2]>@X", "   [2][2][2]   ", "   [2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Pump/LandWaterPump.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetMachineSound("Assets/Base/Machines/Pump/LandWaterPump/LandWaterPump_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      machine2.AddParam((IProtoParam) DisableQuickBuildParam.Instance);
      registrator.RecipeProtoBuilder.Start("Water pumping", Ids.Recipes.LandWaterPumping, (MachineProto) machine2).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Water).SetDurationSeconds(10).BuildAndAdd();
      WellInjectionPumpProto machine3 = new WellInjectionPumpProtoBuilder(registrator).Start("Gas injection pump", Ids.Machines.GasInjectionPump).Description("Provides permanent disposal of gases such as carbon dioxide by dissolving them in a liquid and injecting them under pressure into the ground. Has no pollution effect. Can be built only on top of a limestone deposit.", "short description of a machine").SetCost(Costs.Machines.GasInjectionPump).SetRequiredTerrainProduct(Ids.Products.Limestone).SetElectricityConsumption(400.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("[2][7][7][2]   ", "[2][7][7][2]   ", "[2][7][7][2]   ", "[2][4][4][2]X@<", "   [2][2][2]   ", "   [2][2][2]Y@<").SetPrefabPath("Assets/Base/Machines/Pump/InjectionPump.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetMachineSound("Assets/Base/Machines/Pump/LandWaterPump/LandWaterPump_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Co2 injection", Ids.Recipes.CarbonDioxideInjection, (MachineProto) machine3).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Seawater).SetDurationSeconds(10).BuildAndAdd();
    }

    public WellPumpsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
