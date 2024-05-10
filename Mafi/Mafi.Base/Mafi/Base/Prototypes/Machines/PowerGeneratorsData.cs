// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGeneratorsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class PowerGeneratorsData : IModData
  {
    public static readonly MechPower MECH_GEN_T1_MAX_INPUT;
    public static readonly Electricity MECH_GEN_T1_MAX_OUTPUT;
    public static readonly MechPower MECH_GEN_T2_MAX_INPUT;
    public static readonly Electricity MECH_GEN_T2_MAX_OUTPUT;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.MechanicalPower);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Electricity);
      ElectricityGeneratorFromMechPowerProto fromMechPowerProto1 = registrator.PrototypesDb.Add<ElectricityGeneratorFromMechPowerProto>(new ElectricityGeneratorFromMechPowerProto((StaticEntityProto.ID) Ids.Machines.PowerGeneratorT1, Proto.CreateStr((Proto.ID) Ids.Machines.PowerGeneratorT1, "Power generator", "Converts mechanical power to electricity. The slower it spins the lower its efficiency is."), registrator.LayoutParser.ParseLayoutOrThrow("   [2][2]   ", "   [3][3]   ", "P|>[3][3]>|Q", "   [3][3]   ", "   [2][2]   "), Costs.Machines.PowerGeneratorT1.MapToEntityCosts(registrator), orThrow1, PowerGeneratorsData.MECH_GEN_T1_MAX_INPUT, orThrow2, PowerGeneratorsData.MECH_GEN_T1_MAX_OUTPUT, 5, Mafi.Core.Factory.MechanicalPower.Shaft.OUTPUT_SCALE_MIN - 1.Percent(), ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()), true)), new ElectricityGeneratorFromMechPowerProto.Gfx("Assets/Base/Machines/PowerPlant/Generator.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/Generator/GeneratorSound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      fromMechPowerProto1.AddParam((IProtoParam) new ShaftInertiaProtoParam(fromMechPowerProto1.InputMechPower, 2.Seconds()));
      ElectricityGeneratorFromMechPowerProto fromMechPowerProto2 = registrator.PrototypesDb.Add<ElectricityGeneratorFromMechPowerProto>(new ElectricityGeneratorFromMechPowerProto((StaticEntityProto.ID) Ids.Machines.PowerGeneratorT2, Proto.CreateStr((Proto.ID) Ids.Machines.PowerGeneratorT2, "Power generator (large)", "Optimized power generator with lower friction and better efficiency. The slower it spins the lower its efficiency is."), registrator.LayoutParser.ParseLayoutOrThrow("   [2][2][3][3][3][2][2]   ", "   [3][3][3][3][3][3][3]   ", "P|>[3][3][3][3][3][3][3]>|Q", "   [3][3][3][3][3][3][3]   ", "   [2][2][2][2][2][2][2]   "), Costs.Machines.PowerGeneratorT2.MapToEntityCosts(registrator), orThrow1, PowerGeneratorsData.MECH_GEN_T2_MAX_INPUT, orThrow2, PowerGeneratorsData.MECH_GEN_T2_MAX_OUTPUT, 5, Mafi.Core.Factory.MechanicalPower.Shaft.OUTPUT_SCALE_MIN - 5.Percent(), ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()), true)), new ElectricityGeneratorFromMechPowerProto.Gfx("Assets/Base/Machines/PowerPlant/GeneratorT2.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/GeneratorT2/GeneratorT2Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      fromMechPowerProto2.AddParam((IProtoParam) new ShaftInertiaProtoParam(fromMechPowerProto2.InputMechPower, 2.Seconds()));
      LocStr1 locStr1 = Loc.Str1(Ids.Machines.Flywheel.Value + "__desc", "Flywheel is able to store {0} worth of mechanical power as inertia of a spinning mass. It slowly loses power only if all other entities on the same shaft are idle (not consuming or producing mechanical power).", "description of flywheel, {0} = '120 MW-seconds'");
      registrator.PrototypesDb.Add<FlyWheelEntityProto>(new FlyWheelEntityProto((StaticEntityProto.ID) Ids.Machines.Flywheel, Proto.CreateStr((Proto.ID) Ids.Machines.Flywheel, "Flywheel", LocalizationManager.CreateAlreadyLocalizedStr(locStr1.Id + "_formatted", locStr1.Format(TrCore.MwSec__Unit.Format(500.ToString()).Value).Value)), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("-0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 3, terrainSurfaceHeight: new int?(-h))))
      }), "   [3][3][3][3][3][3][3]   ", "   [3]-1]-1]-1]-1]-1][3]   ", "   [3]-1]-1]-1]-1]-1][3]   ", "P|>[3]-1]-1]-1]-1]-1][3]>|Q", "   [3]-1]-1]-1]-1]-1][3]   ", "   [3]-1]-1]-1]-1]-1][3]   ", "   [3][3][3][3][3][3][3]   "), Costs.Machines.Flywheel.MapToEntityCosts(registrator), ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(100.Percent()), true)), new FlyWheelEntityProto.Gfx("Assets/Base/Machines/PowerPlant/Flywheel.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/Flywheel/FlywheelSound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true))).AddParam((IProtoParam) new ShaftInertiaProtoParam(1.MwMech(), 500.Seconds()));
    }

    public PowerGeneratorsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static PowerGeneratorsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PowerGeneratorsData.MECH_GEN_T1_MAX_INPUT = 3000.KwMech();
      PowerGeneratorsData.MECH_GEN_T1_MAX_OUTPUT = 2000.Kw();
      PowerGeneratorsData.MECH_GEN_T2_MAX_INPUT = 18000.KwMech();
      PowerGeneratorsData.MECH_GEN_T2_MAX_OUTPUT = 15000.Kw();
    }
  }
}
