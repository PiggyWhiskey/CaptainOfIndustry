// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.AssemblyData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class AssemblyData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration = Duration.FromKeyframes(270);
      string[] strArray1 = new string[5]
      {
        "   [4][4][4][4][4][4]   ",
        "A#>[4][4][4][4][4][4]   ",
        "B#>[4][4][4][4][4][4]>#X",
        "C#>[4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4]   "
      };
      string[] strArray2 = new string[5]
      {
        "   [2][2][2][2][2][2]   ",
        "A#>[2][2][2][2][2][2]   ",
        "B#>[2][2][2][2][2][2]>#X",
        "C#>[2][2][2][2][2][2]   ",
        "   [2][2][2][2][2][2]   "
      };
      LocStr desc1 = Loc.Str(Ids.Machines.AssemblyRoboticT1.ToString() + "__desc", "Robotic assembly that is faster and can produce more advanced products.", "description of a machine");
      MachineProto machineProto1 = registrator.MachineProtoBuilder.Start("Assembly (robotic) II", Ids.Machines.AssemblyRoboticT2).Description(desc1).SetCost(Costs.Machines.AssemblyRoboticT2).SetElectricityConsumption(400.Kw()).DisableBoost().SetComputingConsumption(Computing.FromTFlops(8)).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray2).SetPrefabPath("Assets/Base/Machines/Assembly/RoboticT2.prefab").SetMachineSound("Assets/Base/Machines/Assembly/Robotic/AssemblerSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(totalDuration)).EnableSemiInstancedRendering().BuildAndAdd();
      MachineProto machineProto2 = registrator.MachineProtoBuilder.Start("Assembly (robotic)", Ids.Machines.AssemblyRoboticT1).Description(desc1).SetCost(Costs.Machines.AssemblyRoboticT1).SetElectricityConsumption(250.Kw()).DisableBoost().SetComputingConsumption(Computing.FromTFlops(4)).SetNextTier(machineProto1).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray2).SetPrefabPath("Assets/Base/Machines/Assembly/RoboticT1.prefab").SetMachineSound("Assets/Base/Machines/Assembly/Robotic/AssemblerSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(totalDuration)).EnableSemiInstancedRendering().SetProtoToCopyAnimationsFrom(Ids.Machines.AssemblyRoboticT2).AddMaterialSwapForAnimationsLoad("AssemblerT1", "AssemblerT2").BuildAndAdd();
      LocStr desc2 = Loc.Str(Ids.Machines.AssemblyElectrified.ToString() + "__desc", "Assembly line that is faster and can produce more advanced products.", "description of a machine");
      MachineProto machineProto3 = registrator.MachineProtoBuilder.Start("Assembly III", Ids.Machines.AssemblyElectrifiedT2).Description(desc2).SetCost(Costs.Machines.AssemblyElectrifiedT2).SetElectricityConsumption(150.Kw()).SetNextTier(machineProto2).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray1).SetPrefabPath("Assets/Base/Machines/Assembly/AssemblyT3.prefab").SetMachineSound("Assets/Base/Machines/Assembly/AssemblyT2/AssemblyT2Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(Duration.FromKeyframes(120))).EnableSemiInstancedRendering(((ICollection<string>) new string[1]
      {
        "sign"
      }).ToImmutableArray<string>()).AddSign().BuildAndAdd();
      MachineProto machineProto4 = registrator.MachineProtoBuilder.Start("Assembly II", Ids.Machines.AssemblyElectrified).Description(desc2).SetCost(Costs.Machines.AssemblyElectrified).SetElectricityConsumption(80.Kw()).SetNextTier(machineProto3).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray1).SetPrefabPath("Assets/Base/Machines/Assembly/AssemblyT2.prefab").SetMachineSound("Assets/Base/Machines/Assembly/AssemblyT2/AssemblyT2Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(Duration.FromKeyframes(120))).EnableSemiInstancedRendering(((ICollection<string>) new string[1]
      {
        "sign"
      }).ToImmutableArray<string>()).AddSign().BuildAndAdd();
      MachineProto machine = registrator.MachineProtoBuilder.Start("Assembly I", Ids.Machines.AssemblyManual).Description("Assembly line that produces basic products.", "short description of a machine").SetCost(Costs.Machines.AssemblyManual).SetElectricityConsumption(40.Kw()).SetNextTier(machineProto4).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray1).SetPrefabPath("Assets/Base/Machines/Assembly/AssemblyT1.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(Duration.FromKeyframes(80))).SetMachineSound("Assets/Base/Machines/Assembly/AssemblyT1/AssemblyT1Sound.prefab").EnableSemiInstancedRendering(((ICollection<string>) new string[1]
      {
        "sign"
      }).ToImmutableArray<string>()).AddSign().BuildAndAdd();
      registerCpWithBricks("CP assembly (T1)", Ids.Recipes.CpBricksAssemblyT1, machine, 40.Seconds(), 1);
      registerCpWithBricks("CP assembly (T2)", Ids.Recipes.CpBricksAssemblyT2, machineProto4, 20.Seconds(), 1);
      registerCpWithBricks("CP assembly (T3)", Ids.Recipes.CpBricksAssemblyT3, machineProto3, 10.Seconds(), 1);
      registerCpWithBricks("CP assembly (T4)", Ids.Recipes.CpBricksAssemblyT4, machineProto2, 10.Seconds(), 2);
      registerCp("CP assembly (T1)", Ids.Recipes.CpAssemblyT1, machine, 40.Seconds(), 1);
      registerCp("CP assembly (T2)", Ids.Recipes.CpAssemblyT2, machineProto4, 20.Seconds(), 1);
      registerCp("CP assembly (T3)", Ids.Recipes.CpAssemblyT3, machineProto3, 10.Seconds(), 1);
      registerCp("CP assembly (T4)", Ids.Recipes.CpAssemblyT4, machineProto2, 10.Seconds(), 2);
      registerCp2("CP2 assembly (T1)", Ids.Recipes.Cp2AssemblyT1, machine, 80.Seconds());
      registerCp2("CP2 assembly (T1)", Ids.Recipes.Cp2AssemblyT2, machineProto4, 40.Seconds());
      registerCp2("CP2 assembly (T2)", Ids.Recipes.Cp2AssemblyT3, machineProto3, 20.Seconds());
      registerCp2("CP2 assembly (T3)", Ids.Recipes.Cp2AssemblyT4, machineProto2, 10.Seconds());
      registerCp3("CP3 assembly (T1)", Ids.Recipes.Cp3AssemblyT1, machineProto4, 80.Seconds());
      registerCp3("CP3 assembly (T2)", Ids.Recipes.Cp3AssemblyT2, machineProto3, 40.Seconds());
      registerCp3("CP3 assembly (T3)", Ids.Recipes.Cp3AssemblyT3, machineProto2, 20.Seconds());
      registerCp4("CP4 assembly (T1)", Ids.Recipes.Cp4AssemblyElectrifiedT2, machineProto3, 80.Seconds());
      registerCp4("CP4 assembly (T2)", Ids.Recipes.Cp4AssemblyRoboticT1, machineProto2, 40.Seconds());
      registerCp4("CP4 assembly (T3)", Ids.Recipes.Cp4AssemblyRoboticT2, machineProto1, 20.Seconds());
      registerMechParts("Mech. parts assembly (T1)", Ids.Recipes.MechPartsAssemblyT1, machine, 40.Seconds());
      registerMechParts("Mech. parts assembly (T2)", Ids.Recipes.MechPartsAssemblyT2, machineProto4, 20.Seconds());
      registerMechParts("Mech. parts assembly (T3-1)", Ids.Recipes.MechPartsAssemblyT3Iron, machineProto3, 20.Seconds(), 2);
      registerMechPartsSteel("Mech. parts assembly (T3-2)", Ids.Recipes.MechPartsAssemblyT3, machineProto3, 20.Seconds());
      registerMechPartsSteel("Mech. parts assembly (T4)", Ids.Recipes.MechPartsAssemblyT4, machineProto2, 10.Seconds());
      registerVehicleParts("Vehicle parts assembly (T1)", Ids.Recipes.VehicleParts1AssemblyT1, machine, 80.Seconds());
      registerVehicleParts("Vehicle parts assembly (T2)", Ids.Recipes.VehicleParts1AssemblyT2, machineProto4, 40.Seconds());
      registerVehicleParts("Vehicle parts assembly (T3)", Ids.Recipes.VehicleParts1AssemblyT3, machineProto3, 20.Seconds());
      registerVehicleParts("Vehicle parts assembly (T4)", Ids.Recipes.VehicleParts1AssemblyT4, machineProto2, 10.Seconds());
      registerVehicleParts2("Vehicle parts 2 assembly (T1)", Ids.Recipes.VehicleParts2AssemblyT1, machineProto3, 40.Seconds());
      registerVehicleParts2("Vehicle parts 2 assembly (T2)", Ids.Recipes.VehicleParts2AssemblyT2, machineProto2, 20.Seconds());
      registerVehicleParts3("Vehicle parts 3 assembly (T1)", Ids.Recipes.VehicleParts3AssemblyT1, machineProto3, 40.Seconds());
      registerVehicleParts3("Vehicle parts 3 assembly (T2)", Ids.Recipes.VehicleParts3AssemblyT2, machineProto2, 20.Seconds());
      registerLabEquipment("Lab equipment assembly (T1)", Ids.Recipes.LabEquipment1AssemblyT1, machineProto4, 20.Seconds(), 1);
      registerLabEquipment("Lab equipment assembly (T2)", Ids.Recipes.LabEquipment1AssemblyT2, machineProto3, 10.Seconds(), 1);
      registerLabEquipment("Lab equipment assembly (T3)", Ids.Recipes.LabEquipment1AssemblyT3, machineProto2, 10.Seconds(), 2);
      registerLabEquipment2("Lab equipment assembly (T1)", Ids.Recipes.LabEquipment2AssemblyT1, machineProto3, 20.Seconds());
      registerLabEquipment2("Lab equipment assembly (T2)", Ids.Recipes.LabEquipment2AssemblyT2, machineProto2, 10.Seconds());
      registerLabEquipment3("Lab equipment 2 assembly (T2)", Ids.Recipes.LabEquipment3AssemblyT1, machineProto3, 40.Seconds());
      registerLabEquipment3("Lab equipment 2 assembly (T3)", Ids.Recipes.LabEquipment3AssemblyT2, machineProto2, 20.Seconds());
      registerLabEquipment4("Lab equipment 4 assembly (T3)", Ids.Recipes.LabEquipment4AssemblyT2, machineProto2, 40.Seconds());
      registerHouseholdGoods("Household goods assembly (T1)", Ids.Recipes.HouseholdGoodsAssemblyT1, machineProto4, 60.Seconds());
      registerHouseholdGoods("Household goods assembly (T2)", Ids.Recipes.HouseholdGoodsAssemblyT2, machineProto3, 30.Seconds());
      registerHouseholdGoods("Household goods assembly (T3)", Ids.Recipes.HouseholdGoodsAssemblyT3, machineProto2, 15.Seconds());
      registerHouseholdAppliances("Household appliances assembly (T1)", Ids.Recipes.HouseholdAppliancesAssemblyT1, machineProto3, 60.Seconds());
      registerHouseholdAppliances("Household appliances assembly (T2)", Ids.Recipes.HouseholdAppliancesAssemblyT2, machineProto2, 30.Seconds());
      registerHouseholdAppliances("Household appliances assembly (T3)", Ids.Recipes.HouseholdAppliancesAssemblyT3, machineProto1, 15.Seconds());
      registerPCB("PCB assembly (T1)", Ids.Recipes.PCBAssemblyT1, machineProto3, 40.Seconds(), 1);
      registerPCB("PCB assembly (T2)", Ids.Recipes.PCBAssemblyT2, machineProto2, 20.Seconds(), 1);
      registerPCB("PCB assembly (T3)", Ids.Recipes.PCBAssemblyT3, machineProto1, 20.Seconds(), 2);
      registerElectronics("Electronics assembly (T1)", Ids.Recipes.ElectronicsAssemblyT1, machine, 60.Seconds(), 1);
      registerElectronics("Electronics assembly (T2)", Ids.Recipes.ElectronicsAssemblyT2, machineProto4, 20.Seconds(), 1);
      registerElectronics("Electronics assembly (T3)", Ids.Recipes.ElectronicsAssemblyT3, machineProto3, 10.Seconds(), 1);
      registerElectronics("Electronics assembly (T4)", Ids.Recipes.ElectronicsAssemblyT4, machineProto2, 10.Seconds(), 2);
      registerElectronics("Electronics assembly (T5)", Ids.Recipes.ElectronicsAssemblyT5, machineProto1, 10.Seconds(), 4);
      registerElectronics2("Electronics 2 assembly (T1)", Ids.Recipes.Electronics2AssemblyT1, machineProto3, 40.Seconds(), 1);
      registerElectronics2("Electronics 2 assembly (T2)", Ids.Recipes.Electronics2AssemblyT2, machineProto2, 20.Seconds(), 1);
      registerElectronics2("Electronics 2 assembly (T3)", Ids.Recipes.Electronics2AssemblyT3, machineProto1, 20.Seconds(), 2);
      registerUraniumRods("Uranium rods (T1)", Ids.Recipes.UraniumRodsAssemblyT1, machineProto3, 80.Seconds());
      registerEnrichedUraniumAssembly("Uranium enriched (T1)", Ids.Recipes.UraniumEnrichedAssemblyT1, machineProto2, 20.Seconds());
      registerPolyCells("Solar cell assembly", Ids.Recipes.SolarCellAssemblyT1, machineProto3, 120.Seconds());
      registerPolyCells("Solar cell assembly", Ids.Recipes.SolarCellAssemblyT2, machineProto2, 60.Seconds());
      registerPolyCells("Solar cell assembly", Ids.Recipes.SolarCellAssemblyT3, machineProto1, 40.Seconds());
      registerMonoCells("Solar cell assembly", Ids.Recipes.SolarCellMonoAssemblyT1, machineProto1, 60.Seconds());
      registerElectronics3("Electronics 3 assembly (T2)", Ids.Recipes.Electronics3AssemblyRoboticT1, machineProto2, 40.Seconds());
      registerElectronics3("Electronics 3 assembly (T3)", Ids.Recipes.Electronics3AssemblyRoboticT2, machineProto1, 20.Seconds());
      registerServer("Server assembly (T2)", Ids.Recipes.ServerAssemblyT1, machineProto2, 80.Seconds());
      registerServer("Server assembly (T3)", Ids.Recipes.ServerAssemblyT2, machineProto1, 40.Seconds());
      registerConsumerElectronics("Consumer electronics assembly (T1)", Ids.Recipes.ConsumerElectronicsAssemblyT1, machineProto1, 40.Seconds());
      registerMedicalEquipment("Medical equipment assembly (T1)", Ids.Recipes.MedicalEquipmentAssemblyT1, machineProto3, 40.Seconds());
      registerMedicalEquipment("Medical equipment assembly (T2)", Ids.Recipes.MedicalEquipmentAssemblyT2, machineProto2, 20.Seconds());
      registerMedicalSupplies("Medical supplies assembly (T1)", Ids.Recipes.MedicalSuppliesAssemblyT1, machineProto3, 20.Seconds());
      registerMedicalSupplies("Medical supplies assembly (T2)", Ids.Recipes.MedicalSuppliesAssemblyT2, machineProto2, 10.Seconds());
      registerMedicalSupplies2("Medical supplies assembly (T1)", Ids.Recipes.MedicalSupplies2AssemblyT1, machineProto3, 20.Seconds());
      registerMedicalSupplies2("Medical supplies assembly (T2)", Ids.Recipes.MedicalSupplies2AssemblyT2, machineProto2, 10.Seconds());
      registerMedicalSupplies3("Medical supplies III assembly (T1)", Ids.Recipes.MedicalSupplies3AssemblyT1, machineProto3, 20.Seconds());
      registerMedicalSupplies3("Medical supplies III assembly (T2)", Ids.Recipes.MedicalSupplies3AssemblyT2, machineProto2, 10.Seconds());
      registerFoodPack("Food pack assembly (meat)", Ids.Recipes.FoodPackAssemblyMeat, machineProto3, 20.Seconds());
      registerFoodPack2("Food pack assembly (eggs)", Ids.Recipes.FoodPackAssemblyEggs, machineProto3, 20.Seconds());

      void registerCpWithBricks(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(multiplier * 3, Ids.Products.Iron).AddInput<RecipeProtoBuilder.State>(multiplier * 3, Ids.Products.Wood).AddInput<RecipeProtoBuilder.State>(multiplier * 4, Ids.Products.Bricks).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(multiplier * 4, Ids.Products.ConstructionParts).BuildAndAdd();
      }

      void registerCp(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(multiplier * 3, Ids.Products.Iron).AddInput<RecipeProtoBuilder.State>(multiplier * 3, Ids.Products.Wood).AddInput<RecipeProtoBuilder.State>(multiplier * 4, Ids.Products.ConcreteSlab).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(multiplier * 4, Ids.Products.ConstructionParts).BuildAndAdd();
      }

      void registerCp2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.ConstructionParts).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.ConstructionParts2).BuildAndAdd();
      }

      void registerCp3(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.ConstructionParts2).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.ConstructionParts3).BuildAndAdd();
      }

      void registerCp4(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.ConstructionParts3).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics2).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.ConstructionParts4).BuildAndAdd();
      }

      void registerMechParts(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier = 1)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(5 * multiplier, Ids.Products.Iron).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.MechanicalParts).BuildAndAdd();
      }

      void registerMechPartsSteel(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MechanicalParts).BuildAndAdd();
      }

      void registerVehicleParts(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Iron).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.VehicleParts).BuildAndAdd();
      }

      void registerVehicleParts2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.VehicleParts).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Glass).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.VehicleParts2).BuildAndAdd();
      }

      void registerVehicleParts3(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.VehicleParts2).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics2).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.VehicleParts3).BuildAndAdd();
      }

      void registerLabEquipment(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.Electronics).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8 * multiplier, Ids.Products.LabEquipment).BuildAndAdd();
      }

      void registerLabEquipment2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.LabEquipment).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Paper).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.LabEquipment2).BuildAndAdd();
      }

      void registerLabEquipment3(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.LabEquipment2).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics2).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.LabEquipment3).BuildAndAdd();
      }

      void registerLabEquipment4(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.LabEquipment3).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics3).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.LabEquipment4).BuildAndAdd();
      }

      void registerHouseholdGoods(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Glass).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Wood).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.HouseholdGoods).BuildAndAdd();
      }

      void registerHouseholdAppliances(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Electronics).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics2).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.HouseholdAppliances).BuildAndAdd();
      }

      void registerPCB(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * multiplier, Ids.Products.Glass).AddInput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.Plastic).AddInput<RecipeProtoBuilder.State>(2 * multiplier, Ids.Products.Copper).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8 * multiplier, Ids.Products.PCB).BuildAndAdd();
      }

      void registerElectronics(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int multiplier)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(multiplier, Ids.Products.Rubber).AddInput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.Copper).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4 * multiplier, Ids.Products.Electronics).BuildAndAdd();
      }

      void registerElectronics2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration,
        int mult)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.PCB).AddInput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.Electronics).AddInput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.PolySilicon).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.Electronics2).BuildAndAdd();
      }

      void registerUraniumRods(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.UraniumEnriched).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.UraniumRod).BuildAndAdd();
      }

      void registerEnrichedUraniumAssembly(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Plutonium).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched20).BuildAndAdd();
      }

      void registerPolyCells(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.PolySilicon).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Glass).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SolarCell).BuildAndAdd();
      }

      void registerMonoCells(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.SiliconWafer).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Glass).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SolarCellMono).BuildAndAdd();
      }

      void registerElectronics3(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Microchips).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics2).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics3).BuildAndAdd();
      }

      void registerServer(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Electronics3).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Server).BuildAndAdd();
      }

      void registerConsumerElectronics(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics3).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Plastic).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Steel).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.ConsumerElectronics).BuildAndAdd();
      }

      void registerMedicalEquipment(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Steel).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Plastic).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.MedicalEquipment).BuildAndAdd();
      }

      void registerMedicalSupplies(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.MedicalEquipment).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Disinfectant).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MedicalSupplies).BuildAndAdd();
      }

      void registerMedicalSupplies2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MedicalSupplies).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Antibiotics).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MedicalSupplies2).BuildAndAdd();
      }

      void registerMedicalSupplies3(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MedicalSupplies2).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Anesthetics).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Morphine).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MedicalSupplies3).BuildAndAdd();
      }

      void registerFoodPack(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Meat).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Bread).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Snack).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.FoodPack).BuildAndAdd();
      }

      void registerFoodPack2(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Eggs).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Bread).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Snack).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.FoodPack).BuildAndAdd();
      }
    }

    public AssemblyData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
