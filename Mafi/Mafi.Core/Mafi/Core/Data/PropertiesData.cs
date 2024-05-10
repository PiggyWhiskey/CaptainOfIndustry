// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.PropertiesData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.PropertiesDb;

#nullable disable
namespace Mafi.Core.Data
{
  internal class PropertiesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.TrucksCapacityMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.TrucksMaintenanceMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.FuelConsumptionDisabled, false));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.UnityProductionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.SettlementConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.MiningMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.ResearchStepsMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.DeconstructionRefundMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.ConstructionCostsMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.QuickActionsUnityCostMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.FoodConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.CanWithholdWorkersOnStarvation, false));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.BaseHealthMultiplier, Percent.Hundred, PercentPropertyProto.PropertyType.Diff));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.BaseHealthDiffEdicts, Percent.Zero, PercentPropertyProto.PropertyType.Diff));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.RainYieldMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.ForceRunAllMachinesEnabled, false));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.RecyclingRatioDiff, Percent.Zero, PercentPropertyProto.PropertyType.Diff));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.SolarPowerMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.FarmYieldMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.TreesGrowthSpeed, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.DiseaseEffectsMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.TradeVolumeMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.LogisticsCanWorkOnLowPower, true));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.LogisticsIgnorePower, false));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.SlowDownIfBroken, false));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.MachineSpeedOnLowPower, Percent.Zero));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.MachineSpeedOnLowComputing, Percent.Zero));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.GroundWaterPumpSpeedWhenDepleted, Percent.Zero));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.GroundWaterReplenishWhenLow, Percent.Zero));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.ShipsCanUseUnityIfOutOfFuel, false));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.VehicleSlowDownOnLowFuel, false));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.WorldMinesCanRunWithoutUnity, false));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.UnlimitedWorldMines, false));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.WorldMinesReserveMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.ContractsProfitMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.DiseaseMortalityMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.WaterPollutionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.AirPollutionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(IdsCore.PropertyIds.LandfillPollutionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<BooleanPropertyProto>(new BooleanPropertyProto(IdsCore.PropertyIds.OreSortingEnabled, false));
    }

    public PropertiesData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
