// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.EdictsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Population.Edicts;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class EdictsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      string comment1 = "policy / edict which can enabled by the player in their Captain's office. {0}=1.5%";
      string comment2 = "policy / edict which can enabled by the player in their Captain's office. {0}=15%";
      string comment3 = "policy / edict which can enabled by the player in their Captain's office.";
      EdictCategoryProto category1 = prototypesDb.Add<EdictCategoryProto>(new EdictCategoryProto(Ids.EdictCategories.Population, Proto.CreateStr(Ids.EdictCategories.Population, "Population edicts")));
      EdictCategoryProto category2 = prototypesDb.Add<EdictCategoryProto>(new EdictCategoryProto(Ids.EdictCategories.Industry, Proto.CreateStr(Ids.EdictCategories.Industry, "Industrial edicts")));
      LocStr descShort = Loc.Str(Ids.Edicts.GrowthPause.ToOldId() + "__desc", "Disables natural pop growth", comment3);
      LocStr nameFormatted1 = Loc.Str(Ids.Edicts.GrowthPause.ToOldId() + "__name", "Growth pause", comment3);
      ProtosDb protosDb1 = prototypesDb;
      Proto.ID growthPause = Ids.Edicts.GrowthPause;
      Proto.Str strFromLocalized1 = Proto.CreateStrFromLocalized(Ids.Edicts.GrowthPause, (LocStrFormatted) nameFormatted1, descShort);
      EdictCategoryProto category3 = category1;
      Upoints monthlyUpointsCost1 = 0.Upoints();
      Type edictImplementation1 = typeof (PopulationGrowthPauseEdict);
      bool? nullable1 = new bool?(false);
      EdictProto.Gfx graphics1 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/PopGrowthPause.svg");
      bool? isGeneratingUnity1 = nullable1;
      PopsGrowthPauseEdictProto proto1 = new PopsGrowthPauseEdictProto(growthPause, strFromLocalized1, category3, monthlyUpointsCost1, edictImplementation1, graphics1, isGeneratingUnity1);
      protosDb1.Add<PopsGrowthPauseEdictProto>(proto1);
      LocStr1 locStr1_1 = Loc.Str1(Ids.Edicts.PopsBoostT1.ToOldId() + "__desc", "Pops growth increased by {0}", comment1);
      LocStr title1 = Loc.Str(Ids.Edicts.PopsBoostT1.ToOldId() + "__name", "Growth boost", comment3);
      Percent growthBoost1 = 0.4.Percent();
      PopsBoostEdictProto previousTier1 = prototypesDb.Add<PopsBoostEdictProto>(new PopsBoostEdictProto(Ids.Edicts.PopsBoostT1, Proto.CreateStrFromLocalized(Ids.Edicts.PopsBoostT1, formatWithNumeral(title1, 0), locStr1_1.Format(growthBoost1)), category1, 1.Upoints(), typeof (PopsBoostEdict), growthBoost1, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/PopGrowth.svg")));
      Percent growthBoost2 = 0.2.Percent();
      PopsBoostEdictProto previousTier2 = prototypesDb.Add<PopsBoostEdictProto>(new PopsBoostEdictProto(Ids.Edicts.PopsBoostT2, Proto.CreateStrFromLocalized(Ids.Edicts.PopsBoostT2, formatWithNumeral(title1, 1), locStr1_1.Format(growthBoost2)), category1, 1.Upoints(), typeof (PopsBoostEdict), growthBoost2, (Option<EdictProto>) (EdictProto) previousTier1, new EdictProto.Gfx("Assets/Base/Icons/Edicts/PopGrowth.svg")));
      Percent growthBoost3 = 0.2.Percent();
      prototypesDb.Add<PopsBoostEdictProto>(new PopsBoostEdictProto(Ids.Edicts.PopsBoostT3, Proto.CreateStrFromLocalized(Ids.Edicts.PopsBoostT3, formatWithNumeral(title1, 2), locStr1_1.Format(growthBoost3)), category1, 1.Upoints(), typeof (PopsBoostEdict), growthBoost3, (Option<EdictProto>) (EdictProto) previousTier2, new EdictProto.Gfx("Assets/Base/Icons/Edicts/PopGrowth.svg")));
      LocStr1 locStr1_2 = Loc.Str1(Ids.Edicts.PopsEviction.ToOldId() + "__desc", "Every month, {0} of the population will have to pack their things and leave the island", comment1);
      LocStr nameFormatted2 = Loc.Str(Ids.Edicts.PopsEviction.ToOldId() + "__name", "Eviction", comment3);
      Percent monthlyEvictionRate = 4.Percent();
      prototypesDb.Add<PopsEvictionEdictProto>(new PopsEvictionEdictProto(Ids.Edicts.PopsEviction, Proto.CreateStrFromLocalized(Ids.Edicts.PopsEviction, (LocStrFormatted) nameFormatted2, locStr1_2.Format(monthlyEvictionRate)), category1, 2.Upoints(), typeof (PopsEvictionEdict), monthlyEvictionRate, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/Eviction.svg")));
      Percent diseaseReduction = 40.Percent();
      Percent workersToWithhold = 20.Percent();
      LocStr2 locStr2_1 = Loc.Str2(Ids.Edicts.PopsQuarantine.ToOldId() + "__desc", "Quarantines {0} of the total workforce to reduce effects of any ongoing disease by {1}.", "policy / edict which can enabled by the player in their Captain's office. example values: {0}=15%, {1}=20%");
      LocStr nameFormatted3 = Loc.Str(Ids.Edicts.PopsQuarantine.ToOldId() + "__name", "Quarantine", comment3);
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Edicts.PopsQuarantine.ToString() + "__desc", locStr2_1.Format(workersToWithhold.ToStringRounded(), diseaseReduction.ToStringRounded()).Value);
      prototypesDb.Add<PopsQuarantineEdictProto>(new PopsQuarantineEdictProto(Ids.Edicts.PopsQuarantine, Proto.CreateStrFromLocalized(Ids.Edicts.PopsQuarantine, (LocStrFormatted) nameFormatted3, alreadyLocalizedStr), category1, 1.Upoints(), typeof (PopsQuarantineEdict), diseaseReduction, workersToWithhold, new EdictProto.Gfx("Assets/Base/Icons/Edicts/Quarantine.svg")));
      string propertyGroup1 = Ids.Edicts.FuelReduction.Value;
      LocStr title2 = Loc.Str(Ids.Edicts.FuelReduction.ToOldId() + "__name", "Vehicles fuel saver", comment3);
      LocStr1 locStr1_3 = Loc.Str1(Ids.Edicts.FuelReduction.ToOldId() + "__desc", "Vehicles fuel consumption reduced by {0}", comment2);
      Percent percent1 = 15.Percent();
      EdictWithPropertiesProto previousTier3 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.FuelReduction, Proto.CreateStrFromLocalized(Ids.Edicts.FuelReduction, formatWithNumeral(title2, 0), locStr1_3.Format(percent1)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier, -percent1)), propertyGroup1, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FuelReduced.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.FuelReductionT2, Proto.CreateStrFromLocalized(Ids.Edicts.FuelReductionT2, formatWithNumeral(title2, 1), locStr1_3.Format(percent1)), category2, 2.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier, -percent1)), propertyGroup1, (Option<EdictProto>) (EdictProto) previousTier3, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FuelReduced.svg")));
      string propertyGroup2 = Ids.Edicts.ShipFuelReduction.Value;
      LocStr nameFormatted4 = Loc.Str(Ids.Edicts.ShipFuelReduction.ToOldId() + "__name", "Ships fuel saver", comment3);
      LocStr1 locStr1_4 = Loc.Str1(Ids.Edicts.ShipFuelReduction.ToOldId() + "__desc", "Cargo ship fuel consumption reduced by {0}", comment2);
      Percent percent2 = 10.Percent();
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.ShipFuelReduction, Proto.CreateStrFromLocalized(Ids.Edicts.ShipFuelReduction, (LocStrFormatted) nameFormatted4, locStr1_4.Format(percent2)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier, -percent2)), propertyGroup2, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FuelReduced.svg")));
      string propertyGroup3 = Ids.Edicts.ShipFuelReduction.Value;
      LocStr title3 = Loc.Str(Ids.Edicts.TruckCapacityIncrease.ToOldId() + "__name", "Overloaded trucks", comment3);
      LocStr2 locStr2_2 = Loc.Str2(Ids.Edicts.TruckCapacityIncrease.ToOldId() + "__desc", "Trucks can get overloaded by {0} but they require extra {1} maintenance", comment2);
      Percent percent3 = 20.Percent();
      Percent percent4 = 15.Percent();
      EdictWithPropertiesProto previousTier4 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.TruckCapacityIncrease, Proto.CreateStrFromLocalized(Ids.Edicts.TruckCapacityIncrease, formatWithNumeral(title3, 0), locStr2_2.Format(percent4, percent3)), category2, 0.5.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.TrucksCapacityMultiplier, percent4), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.TrucksMaintenanceMultiplier, percent3)), propertyGroup3, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/TrucksCapacity.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.TruckCapacityIncreaseT2, Proto.CreateStrFromLocalized(Ids.Edicts.TruckCapacityIncreaseT2, formatWithNumeral(title3, 1), locStr2_2.Format(percent4, percent3)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.TrucksCapacityMultiplier, percent4), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.TrucksMaintenanceMultiplier, percent3)), propertyGroup3, (Option<EdictProto>) (EdictProto) previousTier4, new EdictProto.Gfx("Assets/Base/Icons/Edicts/TrucksCapacity.svg")));
      Percent percent5 = 20.Percent();
      LocStr title4 = Loc.Str(Ids.Edicts.FoodConsumptionReduction.ToOldId() + "__name", "Food saver", comment3);
      LocStr1 locStr1_5 = Loc.Str1(Ids.Edicts.FoodConsumptionReduction.ToOldId() + "__desc", "Food consumption reduced by {0}", comment2);
      FoodConsumptionEdictProto previousTier5 = prototypesDb.Add<FoodConsumptionEdictProto>(new FoodConsumptionEdictProto(Ids.Edicts.FoodConsumptionReduction, Proto.CreateStrFromLocalized(Ids.Edicts.FoodConsumptionReduction, formatWithNumeral(title4, 0), locStr1_5.Format(percent5)), category1, 1.Upoints(), typeof (FoodConsumptionEdict), -percent5, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FoodReduced.png")));
      Percent percent6 = 10.Percent();
      prototypesDb.Add<FoodConsumptionEdictProto>(new FoodConsumptionEdictProto(Ids.Edicts.FoodConsumptionReductionT2, Proto.CreateStrFromLocalized(Ids.Edicts.FoodConsumptionReductionT2, formatWithNumeral(title4, 1), locStr1_5.Format(percent6)), category1, 1.Upoints(), typeof (FoodConsumptionEdict), -percent6, (Option<EdictProto>) (EdictProto) previousTier5, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FoodReduced.png")));
      string propertyGroup4 = Ids.Edicts.HealthBonus.Value;
      LocStr title5 = Loc.Str(Ids.Edicts.HealthBonus.ToOldId() + "__name", "Health boost", comment3);
      LocStr1 locStr1_6 = Loc.Str1(Ids.Edicts.HealthBonus.ToOldId() + "__desc", "Increase safety conditions to get extra +{0} health points.", comment2);
      Percent percent7 = 10.Percent();
      EdictWithPropertiesProto previousTier6 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.HealthBonus, Proto.CreateStrFromLocalized(Ids.Edicts.HealthBonus, formatWithNumeral(title5, 0), locStr1_6.Format(percent7.ToIntPercentRounded().ToString())), category1, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.BaseHealthDiffEdicts, percent7)), propertyGroup4, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/HealthBoost.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.HealthBonusT2, Proto.CreateStrFromLocalized(Ids.Edicts.HealthBonusT2, formatWithNumeral(title5, 1), locStr1_6.Format(percent7.ToIntPercentRounded().ToString())), category1, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.BaseHealthDiffEdicts, percent7)), propertyGroup4, (Option<EdictProto>) (EdictProto) previousTier6, new EdictProto.Gfx("Assets/Base/Icons/Edicts/HealthBoost.svg")));
      Percent consumptionDiff1 = 25.Percent();
      LocStr title6 = Loc.Str(Ids.Edicts.FoodConsumptionIncrease.ToOldId() + "__name", "Plenty of food", comment3);
      LocStr1 locStr1_7 = Loc.Str1(Ids.Edicts.FoodConsumptionIncrease.ToOldId() + "__desc", "Food consumption increased by {0}", comment2);
      FoodConsumptionEdictProto consumptionEdictProto = prototypesDb.Add<FoodConsumptionEdictProto>(new FoodConsumptionEdictProto(Ids.Edicts.FoodConsumptionIncrease, Proto.CreateStrFromLocalized(Ids.Edicts.FoodConsumptionIncrease, formatWithNumeral(title6, 0), locStr1_7.Format(consumptionDiff1)), category1, -1.Upoints(), typeof (FoodConsumptionEdict), consumptionDiff1, (Option<EdictProto>) Option.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg")));
      ProtosDb protosDb2 = prototypesDb;
      Proto.ID consumptionIncreaseT2_1 = Ids.Edicts.FoodConsumptionIncreaseT2;
      Proto.Str strFromLocalized2 = Proto.CreateStrFromLocalized(Ids.Edicts.FoodConsumptionIncreaseT2, formatWithNumeral(title6, 1), locStr1_7.Format(consumptionDiff1));
      EdictCategoryProto category4 = category1;
      Upoints monthlyUpointsCost2 = -1.5.Upoints();
      Type edictImplementation2 = typeof (FoodConsumptionEdict);
      Percent consumptionDiff2 = consumptionDiff1;
      Option<EdictProto> previousTier7 = (Option<EdictProto>) (EdictProto) consumptionEdictProto;
      EdictProto.Gfx graphics2 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? nullable2 = new bool?();
      bool? isGeneratingUnity2 = nullable2;
      FoodConsumptionEdictProto proto2 = new FoodConsumptionEdictProto(consumptionIncreaseT2_1, strFromLocalized2, category4, monthlyUpointsCost2, edictImplementation2, consumptionDiff2, previousTier7, graphics2, isGeneratingUnity2);
      protosDb2.Add<FoodConsumptionEdictProto>(proto2);
      Percent consumptionDiff3 = 25.Percent();
      Percent consumptionDiff4 = 35.Percent();
      Percent consumptionDiff5 = 50.Percent();
      Percent unityDiff = 20.Percent();
      string str1 = Ids.Edicts.HouseholdGoodsConsumptionIncrease.Value;
      PopNeedProto orThrow1 = prototypesDb.GetOrThrow<PopNeedProto>(IdsCore.PopNeeds.HouseholdGoodsNeed);
      LocStr title7 = Loc.Str(Ids.Edicts.HouseholdGoodsConsumptionIncrease.ToOldId() + "__name", "More household goods", comment3);
      LocStr2 locStr2_3 = Loc.Str2(Ids.Edicts.HouseholdGoodsConsumptionIncrease.ToOldId() + "__desc", "Household goods consumption increased by {0}, unity given for it increased by {1}", comment2);
      ProtosDb protosDb3 = prototypesDb;
      Proto.ID consumptionIncrease1 = Ids.Edicts.HouseholdGoodsConsumptionIncrease;
      Proto.Str strFromLocalized3 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdGoodsConsumptionIncrease, formatWithNumeral(title7, 0), locStr2_3.Format(consumptionDiff3, unityDiff));
      EdictCategoryProto category5 = category1;
      Upoints monthlyUpointsCost3 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed1 = createPropsForNeed(orThrow1, consumptionDiff3, unityDiff);
      string propertyGroup5 = str1;
      Option<EdictProto> none1 = Option<EdictProto>.None;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics3 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity3 = nullable2;
      EdictWithPropertiesProto proto3 = new EdictWithPropertiesProto(consumptionIncrease1, strFromLocalized3, category5, monthlyUpointsCost3, propsForNeed1, propertyGroup5, none1, graphics3, isGeneratingUnity3);
      EdictWithPropertiesProto withPropertiesProto1 = protosDb3.Add<EdictWithPropertiesProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      Proto.ID consumptionIncreaseT2_2 = Ids.Edicts.HouseholdGoodsConsumptionIncreaseT2;
      Proto.Str strFromLocalized4 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdGoodsConsumptionIncreaseT2, formatWithNumeral(title7, 1), locStr2_3.Format(consumptionDiff4, unityDiff));
      EdictCategoryProto category6 = category1;
      Upoints monthlyUpointsCost4 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed2 = createPropsForNeed(orThrow1, consumptionDiff4, unityDiff);
      string propertyGroup6 = str1;
      Option<EdictProto> previousTier8 = (Option<EdictProto>) (EdictProto) withPropertiesProto1;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics4 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity4 = nullable2;
      EdictWithPropertiesProto proto4 = new EdictWithPropertiesProto(consumptionIncreaseT2_2, strFromLocalized4, category6, monthlyUpointsCost4, propsForNeed2, propertyGroup6, previousTier8, graphics4, isGeneratingUnity4);
      EdictWithPropertiesProto withPropertiesProto2 = protosDb4.Add<EdictWithPropertiesProto>(proto4);
      ProtosDb protosDb5 = prototypesDb;
      Proto.ID consumptionIncreaseT3_1 = Ids.Edicts.HouseholdGoodsConsumptionIncreaseT3;
      Proto.Str strFromLocalized5 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdGoodsConsumptionIncreaseT3, formatWithNumeral(title7, 2), locStr2_3.Format(consumptionDiff5, unityDiff));
      EdictCategoryProto category7 = category1;
      Upoints monthlyUpointsCost5 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed3 = createPropsForNeed(orThrow1, consumptionDiff5, unityDiff);
      string propertyGroup7 = str1;
      Option<EdictProto> previousTier9 = (Option<EdictProto>) (EdictProto) withPropertiesProto2;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics5 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity5 = nullable2;
      EdictWithPropertiesProto proto5 = new EdictWithPropertiesProto(consumptionIncreaseT3_1, strFromLocalized5, category7, monthlyUpointsCost5, propsForNeed3, propertyGroup7, previousTier9, graphics5, isGeneratingUnity5);
      protosDb5.Add<EdictWithPropertiesProto>(proto5);
      string str2 = Ids.Edicts.HouseholdAppliancesConsumptionIncrease.Value;
      PopNeedProto orThrow2 = prototypesDb.GetOrThrow<PopNeedProto>(IdsCore.PopNeeds.HouseholdAppliancesNeed);
      LocStr title8 = Loc.Str(Ids.Edicts.HouseholdAppliancesConsumptionIncrease.ToOldId() + "__name", "More household appliances", comment3);
      LocStr2 locStr2_4 = Loc.Str2(Ids.Edicts.HouseholdAppliancesConsumptionIncrease.ToOldId() + "__desc", "Household appliances consumption increased by {0}, unity given for it increased by {1}", comment2);
      ProtosDb protosDb6 = prototypesDb;
      Proto.ID consumptionIncrease2 = Ids.Edicts.HouseholdAppliancesConsumptionIncrease;
      Proto.Str strFromLocalized6 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdAppliancesConsumptionIncrease, formatWithNumeral(title8, 0), locStr2_4.Format(consumptionDiff3, unityDiff));
      EdictCategoryProto category8 = category1;
      Upoints monthlyUpointsCost6 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed4 = createPropsForNeed(orThrow2, consumptionDiff3, unityDiff);
      string propertyGroup8 = str2;
      Option<EdictProto> none2 = Option<EdictProto>.None;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics6 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity6 = nullable2;
      EdictWithPropertiesProto proto6 = new EdictWithPropertiesProto(consumptionIncrease2, strFromLocalized6, category8, monthlyUpointsCost6, propsForNeed4, propertyGroup8, none2, graphics6, isGeneratingUnity6);
      EdictWithPropertiesProto withPropertiesProto3 = protosDb6.Add<EdictWithPropertiesProto>(proto6);
      ProtosDb protosDb7 = prototypesDb;
      Proto.ID consumptionIncreaseT2_3 = Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT2;
      Proto.Str strFromLocalized7 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT2, formatWithNumeral(title8, 1), locStr2_4.Format(consumptionDiff3, unityDiff));
      EdictCategoryProto category9 = category1;
      Upoints monthlyUpointsCost7 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed5 = createPropsForNeed(orThrow2, consumptionDiff3, unityDiff);
      string propertyGroup9 = str2;
      Option<EdictProto> previousTier10 = (Option<EdictProto>) (EdictProto) withPropertiesProto3;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics7 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity7 = nullable2;
      EdictWithPropertiesProto proto7 = new EdictWithPropertiesProto(consumptionIncreaseT2_3, strFromLocalized7, category9, monthlyUpointsCost7, propsForNeed5, propertyGroup9, previousTier10, graphics7, isGeneratingUnity7);
      EdictWithPropertiesProto withPropertiesProto4 = protosDb7.Add<EdictWithPropertiesProto>(proto7);
      ProtosDb protosDb8 = prototypesDb;
      Proto.ID consumptionIncreaseT3_2 = Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT3;
      Proto.Str strFromLocalized8 = Proto.CreateStrFromLocalized(Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT3, formatWithNumeral(title8, 2), locStr2_4.Format(consumptionDiff5, unityDiff));
      EdictCategoryProto category10 = category1;
      Upoints monthlyUpointsCost8 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed6 = createPropsForNeed(orThrow2, consumptionDiff5, unityDiff);
      string propertyGroup10 = str2;
      Option<EdictProto> previousTier11 = (Option<EdictProto>) (EdictProto) withPropertiesProto4;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics8 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity8 = nullable2;
      EdictWithPropertiesProto proto8 = new EdictWithPropertiesProto(consumptionIncreaseT3_2, strFromLocalized8, category10, monthlyUpointsCost8, propsForNeed6, propertyGroup10, previousTier11, graphics8, isGeneratingUnity8);
      protosDb8.Add<EdictWithPropertiesProto>(proto8);
      string str3 = Ids.Edicts.ConsumerElectronicsConsumptionIncrease.Value;
      PopNeedProto orThrow3 = prototypesDb.GetOrThrow<PopNeedProto>(IdsCore.PopNeeds.ConsumerElectronicsNeed);
      LocStr title9 = Loc.Str(Ids.Edicts.ConsumerElectronicsConsumptionIncrease.ToOldId() + "__name", "More consumer electronics", comment3);
      LocStr2 locStr2_5 = Loc.Str2(Ids.Edicts.ConsumerElectronicsConsumptionIncrease.ToOldId() + "__desc", "Consumer electronics consumption increased by {0}, unity given for it increased by {1}", comment2);
      ProtosDb protosDb9 = prototypesDb;
      Proto.ID consumptionIncrease3 = Ids.Edicts.ConsumerElectronicsConsumptionIncrease;
      Proto.Str strFromLocalized9 = Proto.CreateStrFromLocalized(Ids.Edicts.ConsumerElectronicsConsumptionIncrease, formatWithNumeral(title9, 0), locStr2_5.Format(consumptionDiff3, unityDiff));
      EdictCategoryProto category11 = category1;
      Upoints monthlyUpointsCost9 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed7 = createPropsForNeed(orThrow3, consumptionDiff3, unityDiff);
      string propertyGroup11 = str3;
      Option<EdictProto> none3 = Option<EdictProto>.None;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics9 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity9 = nullable2;
      EdictWithPropertiesProto proto9 = new EdictWithPropertiesProto(consumptionIncrease3, strFromLocalized9, category11, monthlyUpointsCost9, propsForNeed7, propertyGroup11, none3, graphics9, isGeneratingUnity9);
      EdictWithPropertiesProto withPropertiesProto5 = protosDb9.Add<EdictWithPropertiesProto>(proto9);
      ProtosDb protosDb10 = prototypesDb;
      Proto.ID consumptionIncreaseT2_4 = Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT2;
      Proto.Str strFromLocalized10 = Proto.CreateStrFromLocalized(Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT2, formatWithNumeral(title9, 1), locStr2_5.Format(consumptionDiff3, unityDiff));
      EdictCategoryProto category12 = category1;
      Upoints monthlyUpointsCost10 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed8 = createPropsForNeed(orThrow3, consumptionDiff3, unityDiff);
      string propertyGroup12 = str3;
      Option<EdictProto> previousTier12 = (Option<EdictProto>) (EdictProto) withPropertiesProto5;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics10 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity10 = nullable2;
      EdictWithPropertiesProto proto10 = new EdictWithPropertiesProto(consumptionIncreaseT2_4, strFromLocalized10, category12, monthlyUpointsCost10, propsForNeed8, propertyGroup12, previousTier12, graphics10, isGeneratingUnity10);
      EdictWithPropertiesProto withPropertiesProto6 = protosDb10.Add<EdictWithPropertiesProto>(proto10);
      ProtosDb protosDb11 = prototypesDb;
      Proto.ID consumptionIncreaseT3_3 = Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT3;
      Proto.Str strFromLocalized11 = Proto.CreateStrFromLocalized(Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT3, formatWithNumeral(title9, 2), locStr2_5.Format(consumptionDiff5, unityDiff));
      EdictCategoryProto category13 = category1;
      Upoints monthlyUpointsCost11 = 0.Upoints();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propsForNeed9 = createPropsForNeed(orThrow3, consumptionDiff5, unityDiff);
      string propertyGroup13 = str3;
      Option<EdictProto> previousTier13 = (Option<EdictProto>) (EdictProto) withPropertiesProto6;
      nullable2 = new bool?(true);
      EdictProto.Gfx graphics11 = new EdictProto.Gfx("Assets/Base/Icons/Edicts/UnityIncreased.svg");
      bool? isGeneratingUnity11 = nullable2;
      EdictWithPropertiesProto proto11 = new EdictWithPropertiesProto(consumptionIncreaseT3_3, strFromLocalized11, category13, monthlyUpointsCost11, propsForNeed9, propertyGroup13, previousTier13, graphics11, isGeneratingUnity11);
      protosDb11.Add<EdictWithPropertiesProto>(proto11);
      string propertyGroup14 = Ids.Edicts.MaintenanceReduction.Value;
      LocStr title10 = Loc.Str(Ids.Edicts.MaintenanceReduction.ToOldId() + "__name", "Maintenance reducer", comment3);
      LocStr1 locStr1_8 = Loc.Str1(Ids.Edicts.MaintenanceReduction.ToOldId() + "__desc", "Maintenance reduced by {0}", comment2);
      Percent percent8 = 15.Percent();
      EdictWithPropertiesProto previousTier14 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.MaintenanceReduction, Proto.CreateStrFromLocalized(Ids.Edicts.MaintenanceReduction, formatWithNumeral(title10, 0), locStr1_8.Format(percent8)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier, -percent8)), propertyGroup14, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/MaintenanceReduced.svg")));
      Percent percent9 = 10.Percent();
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.MaintenanceReductionT2, Proto.CreateStrFromLocalized(Ids.Edicts.MaintenanceReductionT2, formatWithNumeral(title10, 1), locStr1_8.Format(percent9)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier, -percent9)), propertyGroup14, (Option<EdictProto>) (EdictProto) previousTier14, new EdictProto.Gfx("Assets/Base/Icons/Edicts/MaintenanceReduced.svg")));
      string propertyGroup15 = Ids.Edicts.RecyclingIncrease.Value;
      LocStr1 locStr1_9 = Loc.Str1(Ids.Edicts.RecyclingIncrease.ToOldId() + "__desc", "Recycling efficiency increased by {0}", comment2);
      LocStr title11 = Loc.Str(Ids.Edicts.RecyclingIncrease.ToOldId() + "__name", "Recycling increase", comment3);
      Percent percent10 = 20.Percent();
      EdictWithPropertiesProto previousTier15 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.RecyclingIncrease, Proto.CreateStrFromLocalized(Ids.Edicts.RecyclingIncrease, formatWithNumeral(title11, 0), locStr1_9.Format(percent10)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.RecyclingRatioDiff, percent10)), propertyGroup15, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/RecyclingIncrease.svg")));
      Percent percent11 = 15.Percent();
      EdictWithPropertiesProto previousTier16 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.RecyclingIncreaseT2, Proto.CreateStrFromLocalized(Ids.Edicts.RecyclingIncreaseT2, formatWithNumeral(title11, 1), locStr1_9.Format(percent11)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.RecyclingRatioDiff, percent11)), propertyGroup15, (Option<EdictProto>) (EdictProto) previousTier15, new EdictProto.Gfx("Assets/Base/Icons/Edicts/RecyclingIncrease.svg")));
      Percent percent12 = 10.Percent();
      EdictWithPropertiesProto previousTier17 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.RecyclingIncreaseT3, Proto.CreateStrFromLocalized(Ids.Edicts.RecyclingIncreaseT3, formatWithNumeral(title11, 2), locStr1_9.Format(percent12)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.RecyclingRatioDiff, percent12)), propertyGroup15, (Option<EdictProto>) (EdictProto) previousTier16, new EdictProto.Gfx("Assets/Base/Icons/Edicts/RecyclingIncrease.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.RecyclingIncreaseT4, Proto.CreateStrFromLocalized(Ids.Edicts.RecyclingIncreaseT4, formatWithNumeral(title11, 3), locStr1_9.Format(percent12)), category2, 2.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.RecyclingRatioDiff, percent12)), propertyGroup15, (Option<EdictProto>) (EdictProto) previousTier17, new EdictProto.Gfx("Assets/Base/Icons/Edicts/RecyclingIncrease.svg")));
      string propertyGroup16 = Ids.Edicts.FarmYieldIncrease.Value;
      LocStr title12 = Loc.Str(Ids.Edicts.FarmYieldIncrease.ToOldId() + "__name", "Farming boost", comment3);
      LocStr2 locStr2_6 = Loc.Str2(Ids.Edicts.FarmYieldIncrease.ToOldId() + "__desc", "Farm yield increased by {0}, water demands by {1}", comment2);
      Percent percent13 = 10.Percent();
      Percent percent14 = 10.Percent();
      EdictWithPropertiesProto previousTier18 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.FarmYieldIncrease, Proto.CreateStrFromLocalized(Ids.Edicts.FarmYieldIncrease, formatWithNumeral(title12, 0), locStr2_6.Format(percent13, percent14)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmYieldMultiplier, percent13), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, percent14)), propertyGroup16, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FarmingBoost.svg")));
      EdictWithPropertiesProto previousTier19 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.FarmYieldIncreaseT2, Proto.CreateStrFromLocalized(Ids.Edicts.FarmYieldIncreaseT2, formatWithNumeral(title12, 1), locStr2_6.Format(percent13, percent14)), category2, 1.5.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmYieldMultiplier, percent13), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, percent14)), propertyGroup16, (Option<EdictProto>) (EdictProto) previousTier18, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FarmingBoost.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.FarmYieldIncreaseT3, Proto.CreateStrFromLocalized(Ids.Edicts.FarmYieldIncreaseT3, formatWithNumeral(title12, 2), locStr2_6.Format(percent13, percent14)), category2, 2.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmYieldMultiplier, percent13), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, percent14)), propertyGroup16, (Option<EdictProto>) (EdictProto) previousTier19, new EdictProto.Gfx("Assets/Base/Icons/Edicts/FarmingBoost.svg")));
      string propertyGroup17 = Ids.Edicts.WaterConsumptionReduction.Value;
      LocStr title13 = Loc.Str(Ids.Edicts.WaterConsumptionReduction.ToOldId() + "__name", "Water saver", comment3);
      LocStr1 locStr1_10 = Loc.Str1(Ids.Edicts.WaterConsumptionReduction.ToOldId() + "__desc", "Reduces water consumed in settlements and farms by {0}", comment2);
      Percent percent15 = 20.Percent();
      EdictWithPropertiesProto previousTier20 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.WaterConsumptionReduction, Proto.CreateStrFromLocalized(Ids.Edicts.WaterConsumptionReduction, formatWithNumeral(title13, 0), locStr1_10.Format(percent15)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(Ids.Properties.SettlementWaterConsumptionMultiplier, -percent15), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, -percent15)), propertyGroup17, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/WaterSaver.svg")));
      Percent percent16 = 10.Percent();
      EdictWithPropertiesProto previousTier21 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.WaterConsumptionReductionT2, Proto.CreateStrFromLocalized(Ids.Edicts.WaterConsumptionReductionT2, formatWithNumeral(title13, 1), locStr1_10.Format(percent16)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(Ids.Properties.SettlementWaterConsumptionMultiplier, -percent16), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, -percent16)), propertyGroup17, (Option<EdictProto>) (EdictProto) previousTier20, new EdictProto.Gfx("Assets/Base/Icons/Edicts/WaterSaver.svg")));
      Percent percent17 = 5.Percent();
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.WaterConsumptionReductionT3, Proto.CreateStrFromLocalized(Ids.Edicts.WaterConsumptionReductionT3, formatWithNumeral(title13, 2), locStr1_10.Format(percent17)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(Ids.Properties.SettlementWaterConsumptionMultiplier, -percent17), Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier, -percent17)), propertyGroup17, (Option<EdictProto>) (EdictProto) previousTier21, new EdictProto.Gfx("Assets/Base/Icons/Edicts/WaterSaver.svg")));
      string propertyGroup18 = Ids.Edicts.SolarPowerIncrease.Value;
      LocStr title14 = Loc.Str(Ids.Edicts.SolarPowerIncrease.ToOldId() + "__name", "Clean panels", comment3);
      LocStr1 locStr1_11 = Loc.Str1(Ids.Edicts.SolarPowerIncrease.ToOldId() + "__desc", "A demanding cleaning procedure for solar panels, increasing their power output by {0}.", comment2);
      Percent percent18 = 5.Percent();
      Percent percent19 = 10.Percent();
      Percent percent20 = 15.Percent();
      EdictWithPropertiesProto previousTier22 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.SolarPowerIncrease, Proto.CreateStrFromLocalized(Ids.Edicts.SolarPowerIncrease, formatWithNumeral(title14, 0), locStr1_11.Format(percent18)), category2, 0.5.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.SolarPowerMultiplier, percent18)), propertyGroup18, Option<EdictProto>.None, new EdictProto.Gfx("Assets/Base/Icons/Edicts/SolarBoost.svg")));
      EdictWithPropertiesProto previousTier23 = prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.SolarPowerIncreaseT2, Proto.CreateStrFromLocalized(Ids.Edicts.SolarPowerIncreaseT2, formatWithNumeral(title14, 1), locStr1_11.Format(percent19)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.SolarPowerMultiplier, percent19)), propertyGroup18, (Option<EdictProto>) (EdictProto) previousTier22, new EdictProto.Gfx("Assets/Base/Icons/Edicts/SolarBoost.svg")));
      prototypesDb.Add<EdictWithPropertiesProto>(new EdictWithPropertiesProto(Ids.Edicts.SolarPowerIncreaseT3, Proto.CreateStrFromLocalized(Ids.Edicts.SolarPowerIncreaseT3, formatWithNumeral(title14, 2), locStr1_11.Format(percent20)), category2, 1.Upoints(), ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(IdsCore.PropertyIds.SolarPowerMultiplier, percent20)), propertyGroup18, (Option<EdictProto>) (EdictProto) previousTier23, new EdictProto.Gfx("Assets/Base/Icons/Edicts/SolarBoost.svg")));

      static LocStrFormatted formatWithNumeral(LocStr title, int index)
      {
        return new LocStrFormatted(string.Format("{0} {1}", (object) title, (object) EdictProto.ROMAN_NUMERALS[index]));
      }

      static ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> createPropsForNeed(
        PopNeedProto popNeed,
        Percent consumptionDiff,
        Percent unityDiff)
      {
        return ImmutableArray.Create<KeyValuePair<PropertyId<Percent>, Percent>>(Make.Kvp<PropertyId<Percent>, Percent>(popNeed.ConsumptionMultiplierProperty.Value, consumptionDiff), Make.Kvp<PropertyId<Percent>, Percent>(popNeed.UnityMultiplierProperty.Value, unityDiff));
      }
    }

    public EdictsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
