// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Settlements.SettlementsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Settlements
{
  public class SettlementsData : IModData
  {
    public const int HOUSING_SIZE = 28;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Electricity);
      ProductProto orThrow2 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.CleanWater);
      ProductProto orThrow3 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.WasteWater);
      ProductProto orThrow4 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.HouseholdGoods);
      ProductProto orThrow5 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.HouseholdAppliances);
      ProductProto orThrow6 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConsumerElectronics);
      EntityLayoutParams layoutParams1 = new EntityLayoutParams(enforceEmptySurface: true);
      Proto.ID id = new Proto.ID("UpointsStatsCat_Services");
      UpointsStatsCategoryProto statsCategory = registrator.PrototypesDb.Add<UpointsStatsCategoryProto>(new UpointsStatsCategoryProto(id, Proto.CreateStr(id, "Services", ""), new UpointsStatsCategoryProto.Gfx("Assets/Unity/UserInterface/Toolbar/Settlement.svg")));
      string translationComment = "name of a settlement service provided to pops";
      UpointsCategoryProto upointsCategory1 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.Food, "Assets/Unity/UserInterface/Toolbar/Farms.svg", (Option<UpointsStatsCategoryProto>) statsCategory, true));
      registrator.PrototypesDb.Add<PopNeedProto>(new PopNeedProto(IdsCore.PopNeeds.Food, Proto.CreateStr(IdsCore.PopNeeds.Food, "Food", "", translationComment), 1.Upoints(), upointsCategory1, new PopNeedProto.HealthData?(), new PropertyId<Percent>?(IdsCore.PropertyIds.FoodConsumptionMultiplier), new PropertyId<Percent>?(), new PopNeedProto.Gfx("Assets/Unity/UserInterface/Toolbar/Farms.svg")));
      UpointsCategoryProto upointsCategory2 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.PowerNeed, orThrow1.Graphics.IconPath, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      PopNeedProto need1 = registrator.PrototypesDb.Add<PopNeedProto>(new PopNeedProto(IdsCore.PopNeeds.PowerNeed, Proto.CreateStr(IdsCore.PopNeeds.PowerNeed, "Electricity", "", translationComment), 1.2.Upoints(), upointsCategory2, new PopNeedProto.HealthData?(), new PropertyId<Percent>?(), new PropertyId<Percent>?(), new PopNeedProto.Gfx(orThrow1.Graphics.IconPath)));
      HealthPointsCategoryProto category = registrator.PrototypesDb.Add<HealthPointsCategoryProto>(new HealthPointsCategoryProto(IdsCore.PopNeeds.WaterNeed, orThrow2.Graphics.IconPath));
      UpointsCategoryProto upointsCategoryProto1 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.WaterNeed, orThrow2.Graphics.IconPath, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      Proto.ID waterNeed = IdsCore.PopNeeds.WaterNeed;
      Proto.Str str1 = Proto.CreateStr(IdsCore.PopNeeds.WaterNeed, "Water", "", translationComment);
      Upoints unity1 = 1.Upoints();
      UpointsCategoryProto upointsCategory3 = upointsCategoryProto1;
      PopNeedProto.HealthData? healthGiven1 = new PopNeedProto.HealthData?(new PopNeedProto.HealthData(10.Percent(), category));
      PropertyId<Percent>? consumptionMultiplierProperty1 = new PropertyId<Percent>?(Ids.Properties.SettlementWaterConsumptionMultiplier);
      PropertyId<Percent>? nullable1 = new PropertyId<Percent>?();
      PropertyId<Percent>? unityMultiplierProperty1 = nullable1;
      PopNeedProto.Gfx graphics1 = new PopNeedProto.Gfx(orThrow2.Graphics.IconPath);
      PopNeedProto proto1 = new PopNeedProto(waterNeed, str1, unity1, upointsCategory3, healthGiven1, consumptionMultiplierProperty1, unityMultiplierProperty1, graphics1);
      PopNeedProto popNeedProto1 = prototypesDb2.Add<PopNeedProto>(proto1);
      UpointsCategoryProto upointsCategoryProto2 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.HouseholdGoodsNeed, orThrow4.Graphics.IconPath, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      ProtosDb prototypesDb3 = registrator.PrototypesDb;
      Proto.ID householdGoodsNeed = IdsCore.PopNeeds.HouseholdGoodsNeed;
      Proto.Str str2 = Proto.CreateStr(IdsCore.PopNeeds.HouseholdGoodsNeed, "Household goods", "", translationComment);
      Upoints unity2 = 1.4.Upoints();
      UpointsCategoryProto upointsCategory4 = upointsCategoryProto2;
      PopNeedProto.HealthData? healthGiven2 = new PopNeedProto.HealthData?();
      nullable1 = new PropertyId<Percent>?(Ids.Properties.HouseholdGoodsUnityMultiplier);
      PropertyId<Percent>? consumptionMultiplierProperty2 = new PropertyId<Percent>?(Ids.Properties.HouseholdGoodsConsumptionMultiplier);
      PropertyId<Percent>? unityMultiplierProperty2 = nullable1;
      PopNeedProto.Gfx graphics2 = new PopNeedProto.Gfx(orThrow4.Graphics.IconPath);
      PopNeedProto proto2 = new PopNeedProto(householdGoodsNeed, str2, unity2, upointsCategory4, healthGiven2, consumptionMultiplierProperty2, unityMultiplierProperty2, graphics2);
      PopNeedProto needProto1 = prototypesDb3.Add<PopNeedProto>(proto2);
      UpointsCategoryProto upointsCategory5 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.HouseholdAppliancesNeed, orThrow5.Graphics.IconPath, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      PopNeedProto needProto2 = registrator.PrototypesDb.Add<PopNeedProto>(new PopNeedProto(IdsCore.PopNeeds.HouseholdAppliancesNeed, Proto.CreateStr(IdsCore.PopNeeds.HouseholdAppliancesNeed, "Household appliances", "", translationComment), 1.4.Upoints(), upointsCategory5, new PopNeedProto.HealthData?(), new PropertyId<Percent>?(Ids.Properties.HouseholdAppliancesConsumptionMultiplier), new PropertyId<Percent>?(Ids.Properties.HouseholdAppliancesUnityMultiplier), new PopNeedProto.Gfx(orThrow5.Graphics.IconPath)));
      UpointsCategoryProto upointsCategory6 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.ConsumerElectronicsNeed, orThrow6.Graphics.IconPath, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      PopNeedProto needProto3 = registrator.PrototypesDb.Add<PopNeedProto>(new PopNeedProto(IdsCore.PopNeeds.ConsumerElectronicsNeed, Proto.CreateStr(IdsCore.PopNeeds.ConsumerElectronicsNeed, "Consumer electronics", "", translationComment), 1.8.Upoints(), upointsCategory6, new PopNeedProto.HealthData?(), new PropertyId<Percent>?(Ids.Properties.ConsumerElectronicsConsumptionMultiplier), new PropertyId<Percent>?(Ids.Properties.ConsumerElectronicsUnityMultiplier), new PopNeedProto.Gfx(orThrow6.Graphics.IconPath)));
      string str3 = "Assets/Unity/UserInterface/General/Healthcare.svg";
      UpointsCategoryProto upointsCategory7 = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto(IdsCore.PopNeeds.HealthCareNeed, str3, (Option<UpointsStatsCategoryProto>) statsCategory, true));
      PopNeedProto popNeedProto2 = registrator.PrototypesDb.Add<PopNeedProto>(new PopNeedProto(IdsCore.PopNeeds.HealthCareNeed, Proto.CreateStr(IdsCore.PopNeeds.HealthCareNeed, "Hospitals", "", translationComment), 0.Upoints(), upointsCategory7, new PopNeedProto.HealthData?(), new PropertyId<Percent>?(), new PropertyId<Percent>?(), new PopNeedProto.Gfx(str3)));
      LocStr1 locStr1_1 = Loc.Str1(Ids.Buildings.HousingT2.ToString() + "__desc", "Advanced housing for {0} people that provides more comfort. It can also provide a monthly Unity increase if the housing is provided with required services.", "housing description, for instance {0}=120");
      int maxOccupants1 = 230;
      LocStr locStr = Loc.Str(Ids.Buildings.Housing.ToString() + "_AttachmentDesc__desc", "Housing can be either attached to an existing settlement to become part of it (and benefit from all the already provided services such as food). Or it can be placed independently to establish  itself as a new settlement, in that case you need to attach it with service modules.", "explaining how settlement housing can be placed");
      SettlementHousingModuleProto nextTier1 = registrator.HouseProtoBuilder.Start("Housing III", Ids.Buildings.HousingT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.HousingT3.ToString() + "_formatted", locStr1_1.Format(maxOccupants1.ToString()).Value + " " + locStr.ToString())).SetCost(Costs.Buildings.HousingT3).SetCapacity(maxOccupants1).SetUpointsCapacity(18.Upoints()).AddNeedIncrease(need1, 20.Percent()).AddNeedIncrease(popNeedProto1, 10.Percent()).AddUnityIncrease(50.Percent(), popNeedProto1, need1).AddUnityIncrease(100.Percent(), popNeedProto1, need1, needProto1).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(layoutParams1, Enumerable.Repeat<string>(Enumerable.Repeat<string>("(4)", 28).JoinStrings(), 28).ToArray<string>()).SetPrefabPath("Assets/Base/Settlements/T3/SettlementT3_1.prefab").SetPrefabPath("Assets/Base/Settlements/T3/SettlementT3_2.prefab").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-style1-placeholder.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-style2-placeholder.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-RedBricks.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-GrayPlaster.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-BlackBricks.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-RedBricks.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-BlackBricks.mat").AddHouseMaterial("Assets/Base/Settlements/T3/SettT3-GrayPlaster.mat").BuildAndAdd();
      int maxOccupants2 = 130;
      SettlementHousingModuleProto nextTier2 = registrator.HouseProtoBuilder.Start("Housing II", Ids.Buildings.HousingT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.HousingT2.ToString() + "_formatted", locStr1_1.Format(maxOccupants2.ToString()).Value + " " + locStr.ToString())).SetCost(Costs.Buildings.HousingT2).SetNextTier(nextTier1).SetCapacity(maxOccupants2).SetUpointsCapacity(12.Upoints()).AddNeedIncrease(need1, 10.Percent()).AddNeedIncrease(popNeedProto1, 5.Percent()).AddUnityIncrease(50.Percent(), popNeedProto1, need1).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(layoutParams1, Enumerable.Repeat<string>(Enumerable.Repeat<string>("(4)", 28).JoinStrings(), 28).ToArray<string>()).SetPrefabPath("Assets/Base/Settlements/T2/SettlementT2_1.prefab").SetPrefabPath("Assets/Base/Settlements/T2/SettlementT2_2.prefab").AddHouseMaterial("Assets/Base/Settlements/T2/SettT2-Gray.mat").AddHouseMaterial("Assets/Base/Settlements/T2/SettT2-Red.mat").AddHouseMaterial("Assets/Base/Settlements/T2/SettT2-Blue.mat").AddHouseMaterial("Assets/Base/Settlements/T2/SettT2-Yellow.mat").BuildAndAdd();
      LocStr1 locStr1_2 = Loc.Str1(Ids.Buildings.Housing.ToString() + "__desc", "Primitive housing for {0} people made of shipping containers.", "housing description, for instance {0}=60");
      int maxOccupants3 = 70;
      registrator.HouseProtoBuilder.Start("Housing", Ids.Buildings.Housing).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.Housing.ToString() + "_formatted", locStr1_2.Format(maxOccupants3.ToString()).Value + " " + locStr.ToString())).SetCost(Costs.Buildings.HousingT1).SetCapacity(maxOccupants3).SetUpointsCapacity(8.Upoints()).SetNextTier(nextTier2).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(layoutParams1, Enumerable.Repeat<string>(Enumerable.Repeat<string>("(4)", 28).JoinStrings(), 28).ToArray<string>()).SetPrefabPath("Assets/Base/Settlements/T1/SettlementT1_1.prefab").SetPrefabPath("Assets/Base/Settlements/T1/SettlementT1_2.prefab").AddHouseMaterial("Assets/Base/Settlements/T1/SettT1-Red.mat").AddHouseMaterial("Assets/Base/Settlements/T1/SettT1-Blue.mat").AddHouseMaterial("Assets/Base/Settlements/T1/SettT1-Orange.mat").BuildAndAdd();
      ProtosDb protosDb1 = prototypesDb1;
      StaticEntityProto.ID settlementFoodModuleT2 = Ids.Buildings.SettlementFoodModuleT2;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementFoodModuleT2, "Food market II", "Provides food to the attached settlement. This one can store multiple types of products.");
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]<A~", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]<B#", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]   ");
      EntityCosts entityCosts1 = Costs.Buildings.SettlementFoodModuleT2.MapToEntityCosts(registrator);
      Quantity capacityPerBuffer1 = 400.Quantity();
      Option<SettlementFoodModuleProto> none1 = Option<SettlementFoodModuleProto>.None;
      Option<string> none2 = Option<string>.None;
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/FoodMarketT2.prefab", customIconPath: none2, categories: categories);
      SettlementFoodModuleProto proto3 = new SettlementFoodModuleProto(settlementFoodModuleT2, str4, layoutOrThrow1, entityCosts1, 2, capacityPerBuffer1, none1, graphics3);
      SettlementFoodModuleProto settlementFoodModuleProto = protosDb1.Add<SettlementFoodModuleProto>(proto3);
      ProtosDb protosDb2 = prototypesDb1;
      StaticEntityProto.ID settlementFoodModule = Ids.Buildings.SettlementFoodModule;
      Proto.Str str5 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementFoodModule, "Food market", "Provides food to the attached settlement. A settlement needs to have at least one food market attached otherwise its population will starve.");
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]<A~", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]<B#", "[4][4][4][4][4][4][4][4][4][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]   ");
      EntityCosts entityCosts2 = Costs.Buildings.SettlementFoodModule.MapToEntityCosts(registrator);
      Quantity capacityPerBuffer2 = 400.Quantity();
      Option<SettlementFoodModuleProto> nextTier3 = (Option<SettlementFoodModuleProto>) settlementFoodModuleProto;
      Option<string> none3 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics4 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/FoodMarket.prefab", customIconPath: none3, categories: categories);
      SettlementFoodModuleProto proto4 = new SettlementFoodModuleProto(settlementFoodModule, str5, layoutOrThrow2, entityCosts2, 1, capacityPerBuffer2, nextTier3, graphics4);
      protosDb2.Add<SettlementFoodModuleProto>(proto4);
      ProductProto orThrow7 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.HouseholdGoods);
      registrator.SettlementModuleProtoBuilder.Start("Household goods module", Ids.Buildings.SettlementHouseholdGoodsModule).Description("Provides household goods to the attached settlement which generates extra Unity.").SetNeed(needProto1).SetCost(Costs.Buildings.SettlementHouseholdGoodsModule).SetElectricityConsumed(150.Kw()).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<A#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<B#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ").SetInput(orThrow7, 0.011.ToFix32(), 240).SetPrefabPath("Assets/Base/Settlements/HouseholdGoodsModule.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(60.Percent()))).SetEmissionIntensity(2).BuildAndAdd();
      ProductProto orThrow8 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.HouseholdAppliances);
      registrator.SettlementModuleProtoBuilder.Start("Household appliances module", Ids.Buildings.SettlementHouseholdAppliancesModule).Description("Provides appliances electronics to the attached settlement which generates extra Unity.").SetNeed(needProto2).SetCost(Costs.Buildings.SettlementHouseholdAppliancesModule).SetElectricityConsumed(200.Kw()).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<A#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<B#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ").SetInput(orThrow8, 0.008.ToFix32(), 240).SetPrefabPath("Assets/Base/Settlements/HouseholdAppliancesModule.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(60.Percent()))).SetEmissionIntensity(3).BuildAndAdd();
      ProductProto orThrow9 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConsumerElectronics);
      registrator.SettlementModuleProtoBuilder.Start("Consumer electronics module", Ids.Buildings.SettlementConsumerElectronicsModule).Description("Provides consumer electronics to the attached settlement which generates extra Unity.").SetNeed(needProto3).SetCost(Costs.Buildings.SettlementConsumerElectronicsModule).SetElectricityConsumed(350.Kw()).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<A#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<B#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ").SetInput(orThrow9, 0.004.ToFix32(), 240).SetPrefabPath("Assets/Base/Settlements/ConsumerElectronicsModule.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(60.Percent()))).SetEmissionIntensity(3).BuildAndAdd();
      registrator.SettlementModuleProtoBuilder.Start("Water facility", Ids.Buildings.SettlementWaterModule).Description("Provides fresh water to the attached settlement and returns waste water that needs to be disposed of. Providing fresh water to a settlement generates extra Unity and reduces health risks.").SetNeed(popNeedProto1).SetCost(Costs.Buildings.SettlementWaterModule).SetElectricityConsumed(100.Kw()).SetCategories(Ids.ToolbarCategories.Housing).SetLayout(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][8][8][8][8][8][4][4]   ", "[4][4][4][4][8][8][8][8][8][4][4]<A@", "[4][4][4][4][8][8][8][8][8][4][4]<B@", "[4][4][4][4][8][8][8][8][8][4][4]   ", "[4][4][4][4][4][4][4][4][4][4][4]>X@", "[4][4][4][4][4][4][4][4][4][4][4]>Y@", "[4][4][4][4][4][4][4][4][4][4][4]   ").SetPrefabPath("Assets/Base/Settlements/WaterModule.prefab").SetInput(orThrow2, 0.06.ToFix32(), 120).SetOutput(orThrow3, 0.05.ToFix32(), 120).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(60.Percent()))).AnimateOnlyWhenServicingPops().BuildAndAdd();
      ProtosDb protosDb3 = prototypesDb1;
      StaticEntityProto.ID settlementPowerModule = Ids.Buildings.SettlementPowerModule;
      Proto.Str str6 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementPowerModule, "Transformer", "Provides electricity to the attached settlement which generates extra Unity.");
      EntityLayout layoutOrThrow3 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][5][5][5][4][4]", "[4][4][4][4][4][4][5][5][5][4][4]", "[4][4][4][4][4][4][5][5][5][4][4]");
      EntityCosts entityCosts3 = Costs.Buildings.SettlementPowerModule.MapToEntityCosts(registrator);
      PopNeedProto need2 = need1;
      Fix32 fix32_1 = 1.2.ToFix32();
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics5 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/PowerModule.prefab", categories: categories);
      SettlementTransformerProto proto5 = new SettlementTransformerProto(settlementPowerModule, str6, layoutOrThrow3, entityCosts3, need2, fix32_1, graphics5);
      protosDb3.Add<SettlementTransformerProto>(proto5);
      ProductProto orThrow10 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Waste);
      ProtosDb protosDb4 = prototypesDb1;
      StaticEntityProto.ID settlementLandfillModule = Ids.Buildings.SettlementLandfillModule;
      Proto.Str str7 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementLandfillModule, "Waste collection", "Collects general waste from the attached settlement to prevent it from piling up in the settlement and causing health concerns.");
      EntityLayout layoutOrThrow4 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]>A~", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]>B~", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ");
      EntityCosts entityCosts4 = Costs.Buildings.SettlementLandfillModule.MapToEntityCosts(registrator);
      ProductProto productAccepted1 = orThrow10;
      Quantity capacity1 = 200.Quantity();
      Option<string> none4 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics6 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/WasteModule.prefab", customIconPath: none4, categories: categories);
      SettlementWasteModuleProto proto6 = new SettlementWasteModuleProto(settlementLandfillModule, str7, layoutOrThrow4, entityCosts4, productAccepted1, capacity1, graphics6);
      protosDb4.Add<SettlementWasteModuleProto>(proto6);
      ProductProto orThrow11 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables);
      LocStr1 locStr1_3 = Loc.Str1(Ids.Buildings.SettlementRecyclablesModule.ToString() + "__descToFormat", "Collects recyclables from the attached settlement. Recyclables are generated only from products that you provide to the settlement (e.g. {0}). If a settlement does not have this module built, all recyclables end up in general waste.", "{0} can be household goods for instance");
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.SettlementRecyclablesModule.ToString() + "__desc", locStr1_3.Format(orThrow4.Strings.Name).Value);
      ProtosDb protosDb5 = prototypesDb1;
      StaticEntityProto.ID recyclablesModule = Ids.Buildings.SettlementRecyclablesModule;
      Proto.Str str8 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementRecyclablesModule, "Recyclables collection", alreadyLocalizedStr);
      EntityLayout layoutOrThrow5 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]>A~", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]>B~", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ");
      EntityCosts entityCosts5 = Costs.Buildings.SettlementRecyclablesModule.MapToEntityCosts(registrator);
      ProductProto productAccepted2 = orThrow11;
      Quantity capacity2 = 200.Quantity();
      Option<string> none5 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics7 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/RecyclablesModule.prefab", customIconPath: none5, categories: categories);
      SettlementWasteModuleProto proto7 = new SettlementWasteModuleProto(recyclablesModule, str8, layoutOrThrow5, entityCosts5, productAccepted2, capacity2, graphics7);
      protosDb5.Add<SettlementWasteModuleProto>(proto7);
      ProductProto orThrow12 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Biomass);
      ProtosDb protosDb6 = prototypesDb1;
      StaticEntityProto.ID settlementBiomassModule = Ids.Buildings.SettlementBiomassModule;
      Proto.Str str9 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementBiomassModule, "Biomass collection", "Collects organic waste (leftovers from food typically). If a settlement does not have this module built, all the organic waste ends up in general waste.");
      EntityLayoutParser layoutParser = registrator.LayoutParser;
      Proto.ID? hardenedFloorSurfaceId = new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths);
      int? nullable2 = new int?();
      int? customCollapseVerticesThreshold = nullable2;
      ThicknessIRange? customPlacementRange = new ThicknessIRange?();
      Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = new Option<IEnumerable<KeyValuePair<char, int>>>();
      EntityLayoutParams layoutParams2 = new EntityLayoutParams(hardenedFloorSurfaceId: hardenedFloorSurfaceId, customCollapseVerticesThreshold: customCollapseVerticesThreshold, customPlacementRange: customPlacementRange, customPortHeights: customPortHeights);
      string[] strArray = new string[7]
      {
        "[4][4][4][4][5][5][5][5][5][5][4]   ",
        "[4][4][4][4][5][5][5][5][5][5][4]   ",
        "[4][4][4][4][5][5][5][5][5][5][4]>A~",
        "[4][4][4][4][5][5][5][5][5][5][4]   ",
        "[4][4][4][4][5][5][5][5][5][5][4]>B~",
        "[4][4][4][4][5][5][5][5][5][5][4]   ",
        "[4][4][4][4][5][5][5][5][5][5][4]   "
      };
      EntityLayout layoutOrThrow6 = layoutParser.ParseLayoutOrThrow(layoutParams2, strArray);
      EntityCosts entityCosts6 = Costs.Buildings.SettlementBiomassModule.MapToEntityCosts(registrator);
      ProductProto productAccepted3 = orThrow12;
      Quantity capacity3 = 200.Quantity();
      Option<string> none6 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics8 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/BiomassModule.prefab", customIconPath: none6, categories: categories);
      SettlementWasteModuleProto proto8 = new SettlementWasteModuleProto(settlementBiomassModule, str9, layoutOrThrow6, entityCosts6, productAccepted3, capacity3, graphics8);
      protosDb6.Add<SettlementWasteModuleProto>(proto8);
      prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.MedicalSupplies).AddParam((IProtoParam) new MedicalSuppliesParam(0.6.Upoints(), 15.Percent(), 0.6.Percent()));
      prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.MedicalSupplies2).AddParam((IProtoParam) new MedicalSuppliesParam(0.8.Upoints(), 20.Percent(), 1.Percent()));
      prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.MedicalSupplies3).AddParam((IProtoParam) new MedicalSuppliesParam(1.2.Upoints(), 25.Percent(), 1.4.Percent()));
      ProtosDb protosDb7 = prototypesDb1;
      StaticEntityProto.ID clinic = Ids.Buildings.Clinic;
      Proto.Str str10 = Proto.CreateStr((Proto.ID) Ids.Buildings.Clinic, "Clinic", "Provides healthcare that increases overall health of your population, gives extra unity and also reduces negative effects of diseases. This building needs to be provided with medical supplies in order to work.");
      EntityLayout layoutOrThrow7 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.SettlementPaths)), "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<A#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]<B#", "[4][4][4][4][5][5][5][5][5][5][4]   ", "[4][4][4][4][5][5][5][5][5][5][4]   ");
      EntityCosts entityCosts7 = Costs.Buildings.Hospital.MapToEntityCosts(registrator);
      PopNeedProto need3 = popNeedProto2;
      Electricity powerRequired = 60.Kw();
      Quantity capacityPerBuffer3 = 200.Quantity();
      Fix32 fix32_2 = 0.55.ToFix32();
      Option<HospitalProto> none7 = Option<HospitalProto>.None;
      nullable2 = new int?(4);
      ImmutableArray<AnimationParams> animationParams = ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(45.Percent())));
      int? emissionIntensity = nullable2;
      Option<string> none8 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics9 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/Clinic.prefab", customIconPath: none8, categories: categories);
      HospitalProto proto9 = new HospitalProto(clinic, str10, layoutOrThrow7, entityCosts7, need3, powerRequired, 2, capacityPerBuffer3, fix32_2, none7, animationParams, emissionIntensity, graphics9);
      protosDb7.Add<HospitalProto>(proto9);
    }

    public SettlementsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
