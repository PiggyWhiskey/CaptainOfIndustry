// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.Settlement
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class Settlement : IEntityObserverForEnabled, IEntityObserver, IEntityObserverForUpgrade
  {
    public readonly ImmutableArray<PopNeed> AllNeeds;
    private readonly Lyst<SettlementHousingModule> m_housingModules;
    private readonly Lyst<ISettlementSquareModule> m_squareModules;
    [DoNotSaveCreateNewOnLoad("new Lyst<ISettlementModuleForNeedProto>()", 0)]
    private readonly Lyst<ISettlementModuleForNeedProto> m_unlockedModulesCache;
    [DoNotSaveCreateNewOnLoad("new Lyst<SettlementHousingModuleProto>()", 0)]
    private readonly Lyst<SettlementHousingModuleProto> m_unlockedHousesCache;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<bool> m_withholdWorkersOnStarvation;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_unityProductionMultiplier;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_consumptionMultiplier;
    private PopNeed m_foodNeed;
    private PopNeed m_healthcareNeed;
    private bool m_isDestroyed;
    private readonly ProductProto m_recyclablesProto;
    private readonly ProductProto m_landfillProto;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IConstructionManager m_constructionManager;
    private readonly IProductsManager m_productsManager;
    private readonly PopsHealthManager m_healthManager;
    private readonly UpointsManager m_upointsManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<PopNeedProto> m_satisfiedNeedsCache;
    private static readonly Percent MAX_WORKERS_WITHHELD_ON_STARVATION;
    private static readonly Upoints UNITY_PENALTY_FOR_STARVATION;
    private readonly Lyst<SettlementFoodModule> m_foodModules;
    private int m_lastUsedFoodModuleIndex;
    private Percent m_foodCategoriesSatisfactionSumPerMonth;
    private ImmutableArray<Settlement.FoodCategoryData> m_foodCategories;
    private readonly Dict<ProductProto, Settlement.FoodData> m_foodTypesMap;
    public static readonly Percent HealthBonusPerFoodCategory;
    private readonly Lyst<Hospital> m_hospitals;
    private int m_lastUsedHospitalIndex;
    private Percent m_healthcareSumLastMonth;
    public Percent DiseaseMortalityDeductionLastDay;
    private PartialQuantity m_landfillInSettlementPartial;
    private Quantity m_landfillInSettlementReported;
    private PartialQuantityLarge m_bioWasteInSettlement;
    /// <summary>
    /// If settlement gets over this capacity we consider it to be polluted.
    /// </summary>
    private static readonly Quantity LANDFILL_CAPACITY_PER100_POPS;
    private static readonly Quantity MIN_LANDFILL_CAPACITY;
    private static readonly Percent HEALTH_PENALTY_PER_ONE_PERCENT_OF_EXTRA_LANDFILL;
    private static readonly Percent HEALTH_PENALTY_FROM_WASTE_MAX;
    public readonly Percent BioWasteReductionMultiplier;
    public readonly Fix64 WoodToBiomassMultiplier;
    public readonly Percent RecyclableToLandfillMultiplier;
    private Percent m_healthPenaltySumThisMonth;
    private readonly Lyst<SettlementWasteModule> m_landfillModules;
    private int m_lastUsedLandfillIndex;
    private readonly Lyst<SettlementWasteModule> m_bioWasteModules;
    private int m_lastUsedBioWasteIndex;
    private readonly Lyst<SettlementWasteModule> m_recyclableModules;
    private int m_lastUsedRecyclableIndex;
    [DoNotSaveCreateNewOnLoad("new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>()", 0)]
    private readonly Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> m_sourceProductsCache;
    private Dict<ProductProto, PartialQuantityLarge> m_recyclingBuffers;
    [DoNotSaveCreateNewOnLoad("new Dict<ProductProto, PartialQuantityLarge>()", 0)]
    private Dict<ProductProto, PartialQuantityLarge> m_recyclingBuffersCache;
    [DoNotSaveCreateNewOnLoad("new Lyst<ProductQuantity>()", 0)]
    private readonly Lyst<ProductQuantity> m_tempResultsCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int TotalHousingCapacity { get; private set; }

    public Upoints UpointsCapacity { get; private set; }

    public int Population { get; private set; }

    public int FreeHousingCapacity => this.TotalHousingCapacity - this.Population;

    public int AmountStarvedToDeathLastMonth { get; private set; }

    public bool ArePeopleStarving { get; private set; }

    public IReadOnlyCollection<SettlementHousingModule> HousingModules
    {
      get => (IReadOnlyCollection<SettlementHousingModule>) this.m_housingModules;
    }

    public IReadOnlyCollection<ISettlementSquareModule> SquareModules
    {
      get => (IReadOnlyCollection<ISettlementSquareModule>) this.m_squareModules;
    }

    public int MonthsOfFood { get; private set; }

    public Percent ConsumptionMultiplier => this.m_consumptionMultiplier.Value;

    public Settlement(
      SettlementsManager settlementsManager,
      IEntitiesManager entitiesManager,
      IConstructionManager constructionManager,
      IProductsManager productsManager,
      PopsHealthManager healthManager,
      UpointsManager upointsManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IPropertiesDb propsDb,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_housingModules = new Lyst<SettlementHousingModule>();
      this.m_squareModules = new Lyst<ISettlementSquareModule>();
      this.m_unlockedModulesCache = new Lyst<ISettlementModuleForNeedProto>();
      this.m_unlockedHousesCache = new Lyst<SettlementHousingModuleProto>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMonthsOfFood\u003Ek__BackingField = 99;
      this.m_satisfiedNeedsCache = new Set<PopNeedProto>();
      this.m_foodModules = new Lyst<SettlementFoodModule>();
      this.m_foodTypesMap = new Dict<ProductProto, Settlement.FoodData>();
      this.m_hospitals = new Lyst<Hospital>();
      this.m_healthcareSumLastMonth = Percent.Zero;
      this.DiseaseMortalityDeductionLastDay = Percent.Zero;
      this.m_bioWasteInSettlement = PartialQuantityLarge.Zero;
      this.BioWasteReductionMultiplier = 12.Percent();
      this.WoodToBiomassMultiplier = 0.8.ToFix64();
      this.RecyclableToLandfillMultiplier = 25.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBaseLandfillPerPopPerDay\u003Ek__BackingField = new PartialQuantity(0.0005.ToFix32());
      this.m_landfillModules = new Lyst<SettlementWasteModule>();
      this.m_bioWasteModules = new Lyst<SettlementWasteModule>();
      this.m_recyclableModules = new Lyst<SettlementWasteModule>();
      this.m_sourceProductsCache = new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>();
      this.m_recyclingBuffers = new Dict<ProductProto, PartialQuantityLarge>();
      this.m_recyclingBuffersCache = new Dict<ProductProto, PartialQuantityLarge>();
      this.m_tempResultsCache = new Lyst<ProductQuantity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settlementsManager = settlementsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_constructionManager = constructionManager;
      this.m_productsManager = productsManager;
      this.m_healthManager = healthManager;
      this.m_upointsManager = upointsManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      constructionManager.EntityStartedDeconstruction.Add<Settlement>(this, new Action<IStaticEntity>(this.entityDeconstructStarted));
      entitiesManager.EntityRemoved.Add<Settlement>(this, new Action<IEntity>(this.entityRemoved));
      this.m_withholdWorkersOnStarvation = propsDb.GetProperty<bool>(IdsCore.PropertyIds.CanWithholdWorkersOnStarvation);
      this.m_unityProductionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.UnityProductionMultiplier);
      this.m_consumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.SettlementConsumptionMultiplier);
      this.m_unlockedProtosDb.OnUnlockedSetChanged.Add<Settlement>(this, new Action(this.onProtosUnlocked));
      this.AllNeeds = protosDb.All<PopNeedProto>().Select<PopNeedProto, PopNeed>((Func<PopNeedProto, PopNeed>) (x => new PopNeed(x, propsDb, statsManager))).ToImmutableArray<PopNeed>();
      this.m_foodNeed = this.AllNeeds.Where((Func<PopNeed, bool>) (x => x.IsFoodNeed)).First<PopNeed>();
      this.m_healthcareNeed = this.AllNeeds.Where((Func<PopNeed, bool>) (x => x.IsHealthcareNeed)).First<PopNeed>();
      this.buildFoodData(protosDb);
      this.m_recyclablesProto = protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Recyclables);
      this.m_landfillProto = protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Waste);
      settlementsManager.AddSettlement(this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<Settlement>(this, "m_withholdWorkersOnStarvation", (object) resolver.Resolve<IPropertiesDb>().GetProperty<bool>(IdsCore.PropertyIds.CanWithholdWorkersOnStarvation));
      ReflectionUtils.SetField<Settlement>(this, "m_unityProductionMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.UnityProductionMultiplier));
      ReflectionUtils.SetField<Settlement>(this, "m_consumptionMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.SettlementConsumptionMultiplier));
    }

    public void MergeIn(Settlement other)
    {
      this.TotalHousingCapacity += other.TotalHousingCapacity;
      other.TotalHousingCapacity = 0;
      this.UpointsCapacity += other.UpointsCapacity;
      other.UpointsCapacity = Upoints.Zero;
      this.Population += other.Population;
      other.Population = 0;
      this.AmountStarvedToDeathLastMonth += other.AmountStarvedToDeathLastMonth;
      this.ArePeopleStarving |= other.ArePeopleStarving;
      this.mergeInSettlementFood(other);
      this.mergeInSettlementWaste(other);
      this.mergeInSettlementHospitals(other);
      foreach (ISettlementSquareModule squareModule in other.m_squareModules.ToArray())
      {
        other.removeSquareModuleInternal(squareModule);
        this.addSquareModuleInternal(squareModule);
      }
      Assert.That<Lyst<SettlementHousingModule>>(other.m_housingModules).IsEmpty<SettlementHousingModule>();
      Assert.That<Lyst<ISettlementSquareModule>>(other.m_squareModules).IsEmpty<ISettlementSquareModule>();
      foreach (PopNeed allNeed in other.AllNeeds)
      {
        PopNeed otherNeed = allNeed;
        this.AllNeeds.Where((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) otherNeed.Proto)).First<PopNeed>().MergeIn(otherNeed, this);
      }
      this.redistributePopsInHousings();
      other.destroy();
    }

    private Percent trySatisfyNeed(PopNeed need)
    {
      int population = this.Population;
      if (population <= 0)
        return Percent.Zero;
      int popsToSatisfy = population;
      for (int index = 0; index < need.ModulesProvidingTheNeed.Count; ++index)
      {
        need.LastUsedModuleIndex = (need.LastUsedModuleIndex + 1) % need.ModulesProvidingTheNeed.Count;
        popsToSatisfy = need.ModulesProvidingTheNeed[need.LastUsedModuleIndex].TrySatisfyNeedOnNewDay(popsToSatisfy);
        if (popsToSatisfy <= 0)
          break;
      }
      return Percent.FromRatio(population - popsToSatisfy, population);
    }

    public void OnNewDay(
      int islandPopulationWithoutHomeless,
      int homelessToFeed,
      out int homelessNotFed)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      this.m_satisfiedNeedsCache.Clear();
      Percent percent1 = islandPopulationWithoutHomeless > 0 ? Percent.FromRatio(this.Population, islandPopulationWithoutHomeless) : Percent.Zero;
      this.feedPopsOnNewDay(homelessToFeed, percent1, out homelessNotFed);
      foreach (PopNeed allNeed in this.AllNeeds)
      {
        if (!allNeed.IsFoodNeed && !allNeed.IsHealthcareNeed)
        {
          allNeed.PercentSatisfiedAfterLastUpdate = this.trySatisfyNeed(allNeed);
          allNeed.WasNotFullySatisfiedLastDay = this.Population <= 0 || !allNeed.PercentSatisfiedAfterLastUpdate.IsNear(Percent.Hundred, Percent.Epsilon);
          if (!allNeed.WasNotFullySatisfiedLastDay)
            this.m_satisfiedNeedsCache.Add(allNeed.Proto);
        }
      }
      this.updateWasteOnNewDay(percent1);
      Percent hundred = Percent.Hundred;
      Percent scale = Percent.Hundred;
      if (this.Population > 0)
      {
        foreach (SettlementHousingModule housingModule in this.m_housingModules)
        {
          if (!housingModule.Prototype.UnityIncreases.IsEmpty)
          {
            Percent percent2 = scale;
            KeyValuePair<ImmutableArray<PopNeedProto>, Percent> keyValuePair = housingModule.Prototype.UnityIncreases.First;
            Percent percent3 = keyValuePair.Value * housingModule.Population / this.Population;
            scale = percent2 + percent3;
            housingModule.AchievedUnityIncreaseIndexLastUpdate = indexOfUnityBoostAchieved(housingModule);
            if (housingModule.AchievedUnityIncreaseIndexLastUpdate >= 0)
            {
              keyValuePair = housingModule.Prototype.UnityIncreases[housingModule.AchievedUnityIncreaseIndexLastUpdate];
              Percent percent4 = keyValuePair.Value;
              hundred += percent4 * housingModule.Population / this.Population;
            }
          }
        }
      }
      this.updateHospitalsOnNewDay(islandPopulationWithoutHomeless, hundred);
      Percent globalMultiplier = this.m_unityProductionMultiplier.Value;
      foreach (PopNeed allNeed in this.AllNeeds)
      {
        Upoints unityMultiplied = allNeed.GetUnityMultiplied(globalMultiplier);
        if (!unityMultiplied.IsZero)
        {
          Percent satisfiedAfterLastUpdate = allNeed.PercentSatisfiedAfterLastUpdate;
          PopNeed popNeed1 = allNeed;
          Upoints upoints1 = unityMultiplied.ScaledBy(satisfiedAfterLastUpdate);
          upoints1 = upoints1.ScaledBy(percent1);
          Upoints upoints2 = upoints1.ScaledBy(hundred);
          popNeed1.UnityAfterLastUpdate = upoints2;
          PopNeed popNeed2 = allNeed;
          upoints1 = unityMultiplied.ScaledBy(percent1);
          Upoints upoints3 = upoints1.ScaledBy(scale);
          popNeed2.PossibleMaxAfterLastUpdate = upoints3;
          allNeed.DailyRecords.Add(new DailyUpointsRecord(satisfiedAfterLastUpdate, allNeed.UnityAfterLastUpdate, allNeed.PossibleMaxAfterLastUpdate));
        }
      }
      foreach (Settlement.FoodData foodData in this.m_foodTypesMap.Values)
      {
        Upoints possibleMax = this.GetMaxUnityProvidedFor(foodData.Prototype, new Percent?(percent1)).Value.Upoints();
        if (foodData.WasSatisfiedLastUpdate)
        {
          Upoints unity = possibleMax;
          foodData.UpointsGivenLastDay = unity;
          foodData.DailyRecords.Add(new DailyUpointsRecord(Percent.Hundred, unity, possibleMax));
        }
        else
        {
          foodData.UpointsGivenLastDay = Upoints.Zero;
          foodData.DailyRecords.Add(new DailyUpointsRecord(Percent.Hundred, Upoints.Zero, possibleMax));
        }
      }

      int indexOfUnityBoostAchieved(SettlementHousingModule module)
      {
        for (int index = 0; index < module.Prototype.UnityIncreases.Length; ++index)
        {
          KeyValuePair<ImmutableArray<PopNeedProto>, Percent> unityIncrease = module.Prototype.UnityIncreases[index];
          bool flag = true;
          foreach (PopNeedProto popNeedProto in unityIncrease.Key)
          {
            if (!this.m_satisfiedNeedsCache.Contains(popNeedProto))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return index;
        }
        return -1;
      }
    }

    public Upoints GetMaxUnityProvidedFor(FoodProto foodProto, Percent? popRatio = null)
    {
      if (!popRatio.HasValue)
      {
        int populationWithoutHomeless = this.m_settlementsManager.GetTotalPopulationWithoutHomeless();
        popRatio = new Percent?(populationWithoutHomeless > 0 ? Percent.FromRatio(this.Population, populationWithoutHomeless) : Percent.Zero);
      }
      return foodProto.GetUnityProvided(this.m_unityProductionMultiplier.Value).ScaledBy(popRatio.Value);
    }

    public void OnNewMonth(int islandPopulationWithoutHomeless)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      this.updateStarvationOnNewMonth();
      Percent percent1 = islandPopulationWithoutHomeless > 0 ? Percent.FromRatio(this.Population, islandPopulationWithoutHomeless) : Percent.Zero;
      foreach (Settlement.FoodData foodData in this.m_foodTypesMap.Values)
      {
        Upoints zero1 = Upoints.Zero;
        Upoints zero2 = Upoints.Zero;
        foreach (DailyUpointsRecord dailyRecord in foodData.DailyRecords)
        {
          zero1 += dailyRecord.Unity;
          zero2 += dailyRecord.PossibleMax;
        }
        Upoints generated = zero1 / 30;
        Upoints upoints = zero2 / 30;
        foodData.DailyRecords.Clear();
        if (generated.IsPositive)
          this.m_upointsManager.GenerateUnity(this.m_foodNeed.Proto.UpointsCategory.Id, generated, new Upoints?(upoints), new LocStr?(foodData.Prototype.Product.Strings.Name));
      }
      foreach (PopNeed allNeed in this.AllNeeds)
      {
        Upoints zero3 = Upoints.Zero;
        Upoints zero4 = Upoints.Zero;
        Percent zero5 = Percent.Zero;
        foreach (DailyUpointsRecord dailyRecord in allNeed.DailyRecords)
        {
          zero3 += dailyRecord.Unity;
          zero4 += dailyRecord.PossibleMax;
          zero5 += dailyRecord.PercentSatisfied;
        }
        allNeed.SetPercentSatisfiedLastMonth(zero5 / 30);
        Upoints generated = zero3 / 30;
        Upoints upoints = zero4 / 30;
        if (allNeed.Proto.HealthGiven.HasValue)
        {
          PopNeedProto.HealthData healthData = allNeed.Proto.HealthGiven.Value;
          Percent percent2 = healthData.Diff.ScaleBy(allNeed.PercentSatisfiedLastMonth).ScaleBy(percent1);
          if (percent2.IsPositive)
            this.m_healthManager.AddHealthIncrease(healthData.HealthPointsCategory.Id, percent2, new Percent?(healthData.Diff.ScaleBy(percent1)));
          else if (percent2.IsNegative)
            this.m_healthManager.AddHealthDecrease(healthData.HealthPointsCategory.Id, percent2, new Percent?(healthData.Diff.ScaleBy(percent1)));
        }
        if (generated.IsPositive || allNeed.ShouldBeShown)
        {
          allNeed.ShouldBeShown = true;
          this.m_upointsManager.GenerateUnity(allNeed.Proto.UpointsCategory.Id, generated, new Upoints?(upoints), new LocStr?(allNeed.Proto.Strings.Name));
        }
        allNeed.DailyRecords.Clear();
      }
      this.updateFoodOnNewMonth();
      this.updateWasteOnNewMonth();
      Percent increase = this.m_healthcareSumLastMonth / 30;
      if (increase.IsPositive)
        this.m_healthManager.AddHealthIncrease(IdsCore.HealthPointsCategories.Healthcare, increase);
      this.m_healthcareSumLastMonth = Percent.Zero;
      if (islandPopulationWithoutHomeless <= 0)
        return;
      Upoints zero = Upoints.Zero;
      foreach (SettlementHousingModule housingModule in this.m_housingModules)
        zero += housingModule.GetUpointsForDecorations() * housingModule.Population / islandPopulationWithoutHomeless;
      if (!zero.IsPositive)
        return;
      this.m_upointsManager.GenerateUnity(IdsCore.UpointsCategories.SettlementQuality, zero, new Upoints?(zero), new LocStr?(TrCore.UpointsCategory__Decorations));
    }

    private void onProtosUnlocked()
    {
      this.m_unlockedModulesCache.Clear();
      this.m_unlockedHousesCache.Clear();
      this.m_unlockedModulesCache.AddRange(this.m_unlockedProtosDb.AllUnlocked<ISettlementModuleForNeedProto>());
      this.m_unlockedHousesCache.AddRange(this.m_unlockedProtosDb.AllUnlocked<SettlementHousingModuleProto>());
      foreach (PopNeed allNeed in this.AllNeeds)
      {
        PopNeed need = allNeed;
        if (!need.ShouldBeShown)
          need.ShouldBeShown = this.m_unlockedModulesCache.Any<ISettlementModuleForNeedProto>((Predicate<ISettlementModuleForNeedProto>) (x => (Proto) x.PopsNeed == (Proto) need.Proto)) || this.m_unlockedHousesCache.Any<SettlementHousingModuleProto>((Predicate<SettlementHousingModuleProto>) (x => x.UnityIncreases.IsNotEmpty && x.UnityIncreases.First.Key.Contains(need.Proto)));
      }
    }

    private void removeStarvedPops(int amountStarved)
    {
      amountStarved = this.Population.Min(amountStarved);
      Percent reduction = Percent.FromRatio(amountStarved, this.m_settlementsManager.GetTotalPopulation());
      this.m_settlementsManager.RemovePopFromSettlement(amountStarved, this);
      this.m_healthManager.AddBirthDecrease(IdsCore.BirthRateCategories.Starvation, reduction, wasAlreadyApplied: true);
    }

    /// <summary>Returns amount of accommodated people.</summary>
    public int TryAccomodate(int population)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      if (this.FreeHousingCapacity <= 0)
        return 0;
      int num = population.Min(this.FreeHousingCapacity);
      this.setPopulation(this.Population + num);
      return num;
    }

    public int TryRemove(int population)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      int num = population.Min(this.Population);
      this.setPopulation(this.Population - num);
      return num;
    }

    private void setPopulation(int newPopulation)
    {
      Assert.That<int>(newPopulation).IsNotNegative();
      Assert.That<int>(newPopulation).IsLessOrEqual(this.TotalHousingCapacity);
      bool flag = this.Population != newPopulation;
      this.Population = newPopulation;
      if (flag)
        this.redistributePopsInHousings();
      if (newPopulation >= this.NumberOfWorkersWithheld)
        return;
      this.NumberOfWorkersWithheld = newPopulation;
      this.m_settlementsManager.UpdateWithheldWorkers();
    }

    private void redistributePopsInHousings()
    {
      int num = 0;
      Upoints zero = Upoints.Zero;
      foreach (SettlementHousingModule housingModule in this.m_housingModules)
      {
        Assert.That<bool>(housingModule.IsDestroyed).IsFalse();
        if (!housingModule.IsNotEnabled())
        {
          num += housingModule.Capacity;
          zero += housingModule.UpointsCapacity;
        }
      }
      bool flag = this.TotalHousingCapacity != num;
      this.TotalHousingCapacity = num;
      this.UpointsCapacity = zero;
      if (flag)
        this.m_settlementsManager.RecalculateValues();
      int population1 = this.Population;
      foreach (SettlementHousingModule settlementHousingModule in (IEnumerable<SettlementHousingModule>) this.m_housingModules.OrderBy<SettlementHousingModule, int>((Func<SettlementHousingModule, int>) (x => x.Capacity)))
      {
        int population2 = population1.Min(settlementHousingModule.Capacity);
        settlementHousingModule.SetPopulation(population2);
        population1 -= population2;
      }
      if (population1 > 0)
      {
        this.Population -= population1;
        this.m_settlementsManager.ReturnPopsBack(population1);
      }
      Dict<PopNeedProto, Percent> dict = new Dict<PopNeedProto, Percent>();
      foreach (SettlementHousingModule housingModule in this.m_housingModules)
      {
        foreach (KeyValuePair<PopNeedProto, Percent> needsIncrease in (IEnumerable<KeyValuePair<PopNeedProto, Percent>>) housingModule.Prototype.NeedsIncreases)
        {
          if (housingModule.Population != 0)
          {
            Percent percent = needsIncrease.Value * housingModule.Population / this.Population;
            dict.GetRefValue(needsIncrease.Key, out bool _) += percent;
          }
        }
      }
      foreach (PopNeed allNeed in this.AllNeeds)
        allNeed.NeedIncreaseFromHousing = dict.GetOrCreate<PopNeedProto, Percent>(allNeed.Proto, (Func<Percent>) (() => Percent.Zero));
    }

    private void destroy()
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      Assert.That<Lyst<SettlementHousingModule>>(this.m_housingModules).IsEmpty<SettlementHousingModule>();
      Assert.That<Lyst<ISettlementSquareModule>>(this.m_squareModules).IsEmpty<ISettlementSquareModule>();
      if (this.m_landfillInSettlementReported.IsPositive)
      {
        this.m_productsManager.ProductDestroyed(this.m_landfillProto, this.m_landfillInSettlementReported, DestroyReason.Cleared);
        this.m_landfillInSettlementReported = Quantity.Zero;
      }
      this.m_constructionManager.EntityStartedDeconstruction.Remove<Settlement>(this, new Action<IStaticEntity>(this.entityDeconstructStarted));
      this.m_entitiesManager.EntityRemoved.Remove<Settlement>(this, new Action<IEntity>(this.entityRemoved));
      this.m_settlementsManager.RemoveSettlement(this);
      this.m_isDestroyed = true;
    }

    private void entityDeconstructStarted(IEntity entity)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
    }

    private void entityRemoved(IEntity entity)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      if (this.tryRemoveLandfillModule(entity) || this.tryRemoveFoodModule(entity) || this.tryRemoveHospitalModule(entity))
        return;
      ISettlementServiceModule serviceModule = entity as ISettlementServiceModule;
      if (serviceModule != null)
        this.AllNeeds.FirstOrDefault((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) serviceModule.ProvidedNeed)).ModulesProvidingTheNeed.Remove(serviceModule);
      if (!(entity is ISettlementSquareModule squareModule) || !this.m_squareModules.Contains(squareModule))
        return;
      this.RemoveSquareModule(squareModule);
    }

    public void AddSlotModule(IStaticEntity entity)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      if (this.tryAddLandfillModule(entity) || this.tryAddFoodModule(entity) || this.tryAddHospitalModule(entity))
        return;
      ISettlementServiceModule serviceModule = entity as ISettlementServiceModule;
      if (serviceModule != null)
      {
        serviceModule.SetSettlement(this);
        this.AllNeeds.FirstOrDefault((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) serviceModule.ProvidedNeed)).ModulesProvidingTheNeed.AddAssertNew(serviceModule);
      }
      else
        Log.Error(string.Format("The give module {0} is not supported", (object) entity.Prototype));
    }

    public void AddSquareModule(ISettlementSquareModule squareModule)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      this.addSquareModuleInternal(squareModule);
      if (!(squareModule is SettlementHousingModule))
        return;
      this.redistributePopsInHousings();
    }

    public void RemoveSquareModule(ISettlementSquareModule squareModule)
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      this.removeSquareModuleInternal(squareModule);
      if (squareModule is SettlementHousingModule)
        this.redistributePopsInHousings();
      if (!this.m_squareModules.IsEmpty)
        return;
      this.destroy();
    }

    private void addSquareModuleInternal(ISettlementSquareModule squareModule)
    {
      Assert.That<Option<Settlement>>(squareModule.Settlement).IsEqualTo<Settlement>((Settlement) null);
      squareModule.SetSettlement(this);
      this.m_squareModules.Add(squareModule);
      if (!(squareModule is SettlementHousingModule settlementHousingModule))
        return;
      this.m_housingModules.AddAssertNew(settlementHousingModule);
      settlementHousingModule.AddObserver((IEntityObserver) this);
    }

    private void removeSquareModuleInternal(ISettlementSquareModule squareModule)
    {
      Assert.That<Option<Settlement>>(squareModule.Settlement).IsEqualTo<Settlement>(this);
      squareModule.ClearSettlement();
      this.m_squareModules.RemoveAndAssert(squareModule);
      if (!(squareModule is SettlementHousingModule settlementHousingModule))
        return;
      this.m_housingModules.RemoveAndAssert(settlementHousingModule);
      settlementHousingModule.RemoveObserver((IEntityObserver) this);
    }

    void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
    {
      if (!(entity is SettlementHousingModule))
        return;
      this.redistributePopsInHousings();
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      if (!(entity is SettlementHousingModule))
        return;
      this.redistributePopsInHousings();
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      Assert.That<bool>(((IEnumerable<IEntity>) this.m_housingModules).Contains<IEntity>(entity)).IsFalse();
      entity.RemoveObserver((IEntityObserver) this);
    }

    public IIndexable<SettlementFoodModule> AllFoodModules
    {
      get => (IIndexable<SettlementFoodModule>) this.m_foodModules;
    }

    public bool HasNoFoodModule { get; private set; }

    private int NonSatisfiedFoodDebtDaysPops { get; set; }

    private Duration FoodDebtDuration { get; set; }

    public IReadOnlyDictionary<ProductProto, Settlement.FoodData> FoodTypesMap
    {
      get => (IReadOnlyDictionary<ProductProto, Settlement.FoodData>) this.m_foodTypesMap;
    }

    public Percent NominalHealthLastDayFromFood { get; private set; }

    /// <summary>
    /// Percentage of food categories satisfied. E.g. 250% mean we satisfied 2.5 categories.
    /// </summary>
    public Percent FoodCategoriesWithHealthSatisfaction { get; private set; }

    public int CategoriesWithHealthBenefitTotal { get; private set; }

    [NewInSaveVersion(140, null, null, null, null)]
    public int NumberOfWorkersWithheld { get; private set; }

    private bool tryAddFoodModule(IStaticEntity entity)
    {
      if (!(entity is SettlementFoodModule settlementFoodModule))
        return false;
      settlementFoodModule.SetSettlement(this);
      this.m_foodModules.AddAssertNew(settlementFoodModule);
      this.HasNoFoodModule = this.m_foodModules.IsEmpty;
      return true;
    }

    private bool tryRemoveFoodModule(IEntity entity)
    {
      if (!(entity is SettlementFoodModule settlementFoodModule))
        return false;
      this.m_foodModules.Remove(settlementFoodModule);
      this.HasNoFoodModule = this.m_foodModules.IsEmpty;
      return true;
    }

    private void mergeInSettlementFood(Settlement other)
    {
      foreach (SettlementFoodModule foodModule in other.m_foodModules)
      {
        foodModule.ReplaceSettlement(this);
        this.m_foodModules.AddAssertNew(foodModule);
      }
      other.m_foodModules.Clear();
      this.HasNoFoodModule = this.HasNoFoodModule && other.HasNoFoodModule;
    }

    private void feedPopsOnNewDay(
      int homelessToFeed,
      Percent populationRatio,
      out int homelessNotFed)
    {
      Assert.That<int>(homelessToFeed).IsNotNegative();
      Percent consumptionMultiplier = this.ConsumptionMultiplier;
      foreach (SettlementFoodModule foodModule in this.m_foodModules)
      {
        if (foodModule.IsOperational)
        {
          foreach (Option<ProductBuffer> option in foodModule.BuffersPerSlot)
          {
            Settlement.FoodData foodData;
            if (!option.IsNone && !option.Value.IsEmpty && this.m_foodTypesMap.TryGetValue(option.Value.Product, out foodData))
              foodData.AddSupplyTemp(option.Value.Quantity, consumptionMultiplier);
          }
        }
      }
      int num1 = this.Population.ScaledByRounded(this.m_foodNeed.ConsumptionMultiplier) + this.NonSatisfiedFoodDebtDaysPops;
      Percent healthCategoriesSatisfaction1;
      int num2 = this.feedPopsFromCategories(num1, out healthCategoriesSatisfaction1);
      int numerator = num1 - num2;
      Percent healthCategoriesSatisfaction2;
      if (healthCategoriesSatisfaction1 > Percent.Hundred)
      {
        healthCategoriesSatisfaction2 = healthCategoriesSatisfaction1 / Percent.Hundred;
        this.NominalHealthLastDayFromFood = healthCategoriesSatisfaction2.Apply(Settlement.HealthBonusPerFoodCategory);
      }
      else
        this.NominalHealthLastDayFromFood = Percent.Zero;
      this.m_foodCategoriesSatisfactionSumPerMonth += populationRatio.Apply(this.NominalHealthLastDayFromFood);
      this.FoodCategoriesWithHealthSatisfaction = healthCategoriesSatisfaction1;
      homelessNotFed = homelessToFeed <= 0 ? 0 : this.feedPopsFromCategories(homelessToFeed, out healthCategoriesSatisfaction2);
      Assert.That<int>(num2).IsNotNegative();
      Assert.That<int>(numerator).IsNotNegative();
      long num3 = 0;
      foreach (Settlement.FoodData foodData in this.m_foodTypesMap.Values)
      {
        if (foodData.PopsAssignedTemp > 0)
          foodData.AddFoodConsumptionFromAssignedPops(consumptionMultiplier);
        num3 += (long) foodData.PopsDaysSupplyLeftTemp;
        foodData.WasSatisfiedLastUpdate = foodData.PopsAssignedTemp > 0;
        foodData.ResetTempData();
      }
      int num4 = this.Population.ScaledByRounded(this.m_foodNeed.ConsumptionMultiplier);
      this.MonthsOfFood = num4 > 0 ? (int) (num3 / 30L / (long) num4) : 99;
      for (int index = 0; index < this.m_foodModules.Count; ++index)
      {
        this.m_lastUsedFoodModuleIndex = (this.m_lastUsedFoodModuleIndex + 1) % this.m_foodModules.Count;
        SettlementFoodModule foodModule = this.m_foodModules[this.m_lastUsedFoodModuleIndex];
        if (foodModule.IsOperational)
        {
          foreach (Option<ProductBuffer> option in foodModule.BuffersPerSlot)
          {
            Settlement.FoodData foodData;
            if (!option.IsNone && !option.Value.IsEmpty && this.m_foodTypesMap.TryGetValue(option.Value.Product, out foodData) && foodData.FoodLeftToConsume.IntegerPart.IsPositive)
            {
              Quantity quantity = option.Value.RemoveAsMuchAs(foodData.FoodLeftToConsume.IntegerPart);
              foodData.FoodLeftToConsume -= quantity.AsPartial;
              if (quantity.IsPositive)
                this.TransformProductIntoWaste(option.Value.Product, quantity);
            }
          }
        }
      }
      this.m_foodNeed.PercentSatisfiedAfterLastUpdate = num1 > 0 ? Percent.FromRatio(numerator, num1) : Percent.Zero;
      this.NonSatisfiedFoodDebtDaysPops = num2;
      if (num2 > 0)
        this.FoodDebtDuration += Duration.OneDay;
      else
        this.FoodDebtDuration = Duration.Zero;
    }

    private void updateFoodOnNewMonth()
    {
      Percent increase = this.m_foodCategoriesSatisfactionSumPerMonth / 30;
      if (increase.IsPositive)
        this.m_healthManager.AddHealthIncrease(IdsCore.HealthPointsCategories.Food, increase);
      this.m_foodCategoriesSatisfactionSumPerMonth = Percent.Zero;
    }

    /// <summary>
    ///  Return the number of pops that were not fed.
    /// 
    /// categoriesSatisfaction gives 100% for each satisfied category, so
    /// - 250% means 2.5 category was satisfied
    /// </summary>
    private int feedPopsFromCategories(int popsToFeed, out Percent healthCategoriesSatisfaction)
    {
      healthCategoriesSatisfaction = Percent.Zero;
      if (popsToFeed <= 0)
        return popsToFeed;
      int num1 = this.m_foodCategories.Count((Func<Settlement.FoodCategoryData, bool>) (x => x.PopsDaysSupplyLeftTemp > 0));
      bool flag = true;
      for (; popsToFeed > 0 && num1 > 0; num1 = this.m_foodCategories.Count((Func<Settlement.FoodCategoryData, bool>) (x => x.PopsDaysSupplyLeftTemp > 0)))
      {
        int intCeiled = (popsToFeed / num1.ToFix32()).ToIntCeiled();
        foreach (Settlement.FoodCategoryData foodCategory in this.m_foodCategories)
        {
          int popsToFeed1 = intCeiled.Min(popsToFeed);
          int num2 = this.feedFromCategory(foodCategory, popsToFeed1);
          int numerator = popsToFeed1 - num2;
          popsToFeed -= numerator;
          if (flag && foodCategory.Prototype.HasHealthBenefit)
            healthCategoriesSatisfaction += Percent.FromRatio(numerator, intCeiled);
          if (popsToFeed <= 0)
          {
            Assert.That<int>(popsToFeed).IsZero();
            break;
          }
        }
        flag = false;
      }
      return popsToFeed;
    }

    /// <summary>Return the number of pops that were not fed.</summary>
    private int feedFromCategory(Settlement.FoodCategoryData category, int popsToFeed)
    {
      for (int index = category.FoodTypes.Count((Func<Settlement.FoodData, bool>) (x => x.PopsDaysSupplyLeftTemp > 0)); popsToFeed > 0 && index > 0; index = category.FoodTypes.Count((Func<Settlement.FoodData, bool>) (x => x.PopsDaysSupplyLeftTemp > 0)))
      {
        int intCeiled = (popsToFeed / index.ToFix32()).ToIntCeiled();
        foreach (Settlement.FoodData foodType in category.FoodTypes)
        {
          int num = foodType.PopsDaysSupplyLeftTemp.Min(intCeiled).Min(popsToFeed);
          foodType.AddPopsAssignedTemp(num);
          popsToFeed -= num;
          if (popsToFeed <= 0)
          {
            Assert.That<int>(popsToFeed).IsZero();
            break;
          }
        }
      }
      return popsToFeed;
    }

    private void buildFoodData(ProtosDb protosDb)
    {
      ImmutableArray<FoodProto> foods = protosDb.All<FoodProto>().ToImmutableArray<FoodProto>();
      this.m_foodCategories = protosDb.All<FoodCategoryProto>().Select<FoodCategoryProto, Settlement.FoodCategoryData>((Func<FoodCategoryProto, Settlement.FoodCategoryData>) (x => new Settlement.FoodCategoryData(x, foods.Where((Func<FoodProto, bool>) (f => (Proto) f.FoodCategory == (Proto) x))))).ToImmutableArray<Settlement.FoodCategoryData>();
      this.CategoriesWithHealthBenefitTotal = this.m_foodCategories.Count((Func<Settlement.FoodCategoryData, bool>) (x => x.Prototype.HasHealthBenefit));
      foreach (Settlement.FoodCategoryData foodCategory in this.m_foodCategories)
      {
        foreach (Settlement.FoodData foodType in foodCategory.FoodTypes)
        {
          if (!this.m_foodTypesMap.TryAdd(foodType.Prototype.Product, foodType))
            Assert.Fail(string.Format("There is more than one FoodProto for {0}. Throwing {1} away!", (object) foodType.Prototype.Product, (object) foodType.Prototype));
        }
      }
    }

    private void updateStarvationOnNewMonth()
    {
      this.AmountStarvedToDeathLastMonth = 0;
      this.ArePeopleStarving = false;
      updateStarvation();
      bool arePopsStarving;
      this.ArePeopleStarving = arePopsStarving;
      int numerator = 0;
      int popsToBeStarvedToDeath;
      if (this.ArePeopleStarving && popsToBeStarvedToDeath > 0)
      {
        if (popsToBeStarvedToDeath > this.Population)
          popsToBeStarvedToDeath = this.Population;
        if (this.m_withholdWorkersOnStarvation.Value)
        {
          int denominator = this.Population.ScaledByRounded(Settlement.MAX_WORKERS_WITHHELD_ON_STARVATION);
          numerator = (this.NumberOfWorkersWithheld + popsToBeStarvedToDeath / 2).Min(denominator).Max(1);
          int populationWithoutHomeless = this.m_settlementsManager.GetTotalPopulationWithoutHomeless();
          Upoints unity = Settlement.UNITY_PENALTY_FOR_STARVATION.ScaledBy(Percent.FromRatio(this.Population, populationWithoutHomeless)).ScaledBy(Percent.FromRatio(numerator, denominator));
          this.m_upointsManager.ConsumeAsMuchAs(IdsCore.UpointsCategories.Starvation, unity, new Option<IEntity>(), new LocStr?());
        }
        else
        {
          this.removeStarvedPops(popsToBeStarvedToDeath);
          this.AmountStarvedToDeathLastMonth += popsToBeStarvedToDeath;
        }
      }
      if (this.NumberOfWorkersWithheld == numerator)
        return;
      this.NumberOfWorkersWithheld = numerator;
      this.m_settlementsManager.UpdateWithheldWorkers();

      void updateStarvation()
      {
        popsToBeStarvedToDeath = 0;
        if (this.FoodDebtDuration.IsNotPositive)
          arePopsStarving = false;
        else if (this.Population == 0)
        {
          this.FoodDebtDuration = Duration.Zero;
          this.NonSatisfiedFoodDebtDaysPops = 0;
          arePopsStarving = false;
        }
        else
        {
          int months = 1;
          Percent percent1 = 70.Percent();
          Percent percent2 = 5.Percent();
          arePopsStarving = true;
          this.NonSatisfiedFoodDebtDaysPops = percent1.Apply(this.NonSatisfiedFoodDebtDaysPops);
          if (this.FoodDebtDuration < Duration.FromMonths(months))
            return;
          int val = this.NonSatisfiedFoodDebtDaysPops.CeilDiv(30);
          popsToBeStarvedToDeath = percent2.ApplyCeiled(val);
        }
      }
    }

    public IIndexable<Hospital> AllHospitals => (IIndexable<Hospital>) this.m_hospitals;

    private bool tryAddHospitalModule(IStaticEntity entity)
    {
      if (!(entity is Hospital hospital))
        return false;
      hospital.SetSettlement(this);
      this.m_hospitals.AddAssertNew(hospital);
      return true;
    }

    private bool tryRemoveHospitalModule(IEntity entity)
    {
      if (!(entity is Hospital hospital))
        return false;
      this.m_hospitals.Remove(hospital);
      return true;
    }

    private void mergeInSettlementHospitals(Settlement other)
    {
      foreach (Hospital hospital in other.m_hospitals)
      {
        hospital.ReplaceSettlement(this);
        this.m_hospitals.AddAssertNew(hospital);
      }
      other.m_hospitals.Clear();
    }

    private void updateHospitalsOnNewDay(
      int islandPopulationWithoutHomeless,
      Percent boostFromHousing)
    {
      this.DiseaseMortalityDeductionLastDay = Percent.Zero;
      Percent zero1 = Percent.Zero;
      Upoints zero2 = Upoints.Zero;
      int popsToSatisfy = this.Population;
      for (int index = 0; index < this.m_hospitals.Count && popsToSatisfy > 0; ++index)
      {
        this.m_lastUsedHospitalIndex = (this.m_lastUsedHospitalIndex + 1) % this.m_hospitals.Count;
        Percent healthProvided;
        Upoints upointsGenerated;
        Percent mortalityDeduction;
        popsToSatisfy = this.m_hospitals[this.m_lastUsedHospitalIndex].TrySatisfyNeedOnNewDay(popsToSatisfy, this.Population, out healthProvided, out upointsGenerated, out mortalityDeduction);
        zero1 += healthProvided;
        this.DiseaseMortalityDeductionLastDay += mortalityDeduction;
        zero2 += upointsGenerated;
      }
      int numerator = this.Population - popsToSatisfy;
      Percent percent1 = this.Population > 0 ? Percent.FromRatio(numerator, this.Population) : Percent.Zero;
      if (islandPopulationWithoutHomeless == 0)
      {
        this.m_healthcareNeed.PercentSatisfiedAfterLastUpdate = Percent.Zero;
        this.m_healthcareNeed.WasNotFullySatisfiedLastDay = true;
      }
      else
      {
        this.m_healthcareNeed.PercentSatisfiedAfterLastUpdate = percent1;
        this.m_healthcareNeed.WasNotFullySatisfiedLastDay = numerator != this.Population;
      }
      if (!this.m_healthcareNeed.WasNotFullySatisfiedLastDay)
        this.m_satisfiedNeedsCache.Add(this.m_healthcareNeed.Proto);
      Percent percent2 = islandPopulationWithoutHomeless > 0 ? Percent.FromRatio(this.Population, islandPopulationWithoutHomeless) : Percent.Zero;
      this.m_healthcareNeed.UnityAfterLastUpdate = zero2.ScaledBy(percent2).ScaledBy(boostFromHousing);
      this.m_healthcareNeed.PossibleMaxAfterLastUpdate = this.m_healthcareNeed.UnityAfterLastUpdate;
      this.m_healthcareNeed.DailyRecords.Add(new DailyUpointsRecord(this.m_healthcareNeed.PercentSatisfiedAfterLastUpdate, this.m_healthcareNeed.UnityAfterLastUpdate, this.m_healthcareNeed.PossibleMaxAfterLastUpdate));
      this.m_healthcareSumLastMonth += zero1.ScaleBy(percent2);
    }

    public PartialQuantity LandfillInSettlement
    {
      get => this.m_landfillInSettlementReported.AsPartial + this.m_landfillInSettlementPartial;
    }

    public Quantity LandfillLimitForCurrentPopulation
    {
      get
      {
        return (this.TotalHousingCapacity * Settlement.LANDFILL_CAPACITY_PER100_POPS.Value / 100).Quantity().Max(Settlement.MIN_LANDFILL_CAPACITY);
      }
    }

    public Percent NominalHealthPenaltyFromWasteLastDay { get; private set; }

    public PartialQuantity BaseLandfillPerPopPerDay { get; private set; }

    public IIndexable<SettlementWasteModule> AllLandfillModules
    {
      get => (IIndexable<SettlementWasteModule>) this.m_landfillModules;
    }

    public IIndexable<SettlementWasteModule> BioWasteModules
    {
      get => (IIndexable<SettlementWasteModule>) this.m_bioWasteModules;
    }

    public IIndexable<SettlementWasteModule> RecyclablesModules
    {
      get => (IIndexable<SettlementWasteModule>) this.m_recyclableModules;
    }

    private bool tryAddLandfillModule(IStaticEntity entity)
    {
      if (!(entity is SettlementWasteModule settlementWasteModule))
        return false;
      if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Waste)
        this.m_landfillModules.AddAssertNew(settlementWasteModule);
      else if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Biomass)
        this.m_bioWasteModules.AddAssertNew(settlementWasteModule);
      else if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Recyclables)
        this.m_recyclableModules.AddAssertNew(settlementWasteModule);
      else
        Log.Error(string.Format("Unknown type of waste module {0}, ", (object) settlementWasteModule.Prototype) + string.Format("product {0} not recognized.", (object) settlementWasteModule.Prototype.ProductAccepted));
      return true;
    }

    private bool tryRemoveLandfillModule(IEntity entity)
    {
      if (!(entity is SettlementWasteModule settlementWasteModule))
        return false;
      if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Waste)
        this.m_landfillModules.Remove(settlementWasteModule);
      else if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Biomass)
        this.m_bioWasteModules.Remove(settlementWasteModule);
      else if (settlementWasteModule.Prototype.ProductAccepted.Id == IdsCore.Products.Recyclables)
        this.m_recyclableModules.Remove(settlementWasteModule);
      else
        Log.Error(string.Format("Unknown type of waste module {0}, ", (object) settlementWasteModule.Prototype) + string.Format("product {0} not recognized.", (object) settlementWasteModule.Prototype.ProductAccepted));
      return true;
    }

    private void mergeInSettlementWaste(Settlement other)
    {
      other.m_landfillModules.ForEachAndClear<Lyst<SettlementWasteModule>>(this.m_landfillModules, (Action<SettlementWasteModule, Lyst<SettlementWasteModule>>) ((i, x) => x.AddAssertNew(i)));
      other.m_bioWasteModules.ForEachAndClear<Lyst<SettlementWasteModule>>(this.m_bioWasteModules, (Action<SettlementWasteModule, Lyst<SettlementWasteModule>>) ((i, x) => x.AddAssertNew(i)));
      other.m_recyclableModules.ForEachAndClear<Lyst<SettlementWasteModule>>(this.m_recyclableModules, (Action<SettlementWasteModule, Lyst<SettlementWasteModule>>) ((i, x) => x.AddAssertNew(i)));
      this.m_bioWasteInSettlement += other.m_bioWasteInSettlement;
      other.m_bioWasteInSettlement = PartialQuantityLarge.Zero;
      this.m_landfillInSettlementPartial += other.m_landfillInSettlementPartial;
      other.m_landfillInSettlementPartial = PartialQuantity.Zero;
      this.m_landfillInSettlementReported += other.m_landfillInSettlementReported;
      other.m_landfillInSettlementReported = Quantity.Zero;
      this.updatePartialWaste();
    }

    private void updatePartialWaste()
    {
      Quantity integerPart = this.m_landfillInSettlementPartial.IntegerPart;
      if (!integerPart.IsPositive)
        return;
      this.m_landfillInSettlementReported += integerPart;
      this.m_landfillInSettlementPartial -= integerPart.AsPartial;
      this.m_productsManager.ProductCreated(this.m_landfillProto, integerPart, CreateReason.Settlement);
    }

    public void Test_SetBaseLandfillPerPopPerDay(PartialQuantity partialQuantity)
    {
      this.BaseLandfillPerPopPerDay = partialQuantity;
    }

    public void TransformProductIntoWaste(ProductProto product, Quantity quantity)
    {
      this.m_productsManager.DestroyProductReturnRemovedSourceProducts(product, quantity, DestroyReason.Settlement, this.m_sourceProductsCache);
      foreach (KeyValuePair<ProductProto, PartialQuantityLarge> keyValuePair in this.m_sourceProductsCache)
      {
        ProductProto key = keyValuePair.Key;
        if (key.Id == IdsCore.Products.Biomass)
          this.m_bioWasteInSettlement += keyValuePair.Value.ScaledBy(this.BioWasteReductionMultiplier);
        else if (key.Id == IdsCore.Products.Wood)
          this.m_bioWasteInSettlement += keyValuePair.Value * this.WoodToBiomassMultiplier;
        else if (key.Id == IdsCore.Products.Waste)
          this.m_landfillInSettlementPartial += keyValuePair.Value.AsPartial;
        else if (key.IsRecyclable)
          this.m_recyclingBuffers.GetRefValue(keyValuePair.Key, out bool _) += keyValuePair.Value;
      }
      this.updatePartialWaste();
    }

    private void updateWasteOnNewDay(Percent populationRatio)
    {
      if (this.m_bioWasteInSettlement.IntegerPart.IsPositive)
      {
        Quantity quantityClamped = this.m_bioWasteInSettlement.IntegerPart.ToQuantityClamped();
        this.m_bioWasteInSettlement -= (quantityClamped - this.pushBioWaste(quantityClamped)).AsPartialQuantityLarge();
      }
      Quantity quantity1 = 0.Quantity();
      foreach (SettlementWasteModule recyclableModule in this.m_recyclableModules)
        quantity1 += recyclableModule.GetCapacityLeft();
      Quantity quantity2 = 0.Quantity();
      Quantity quantity3 = 0.Quantity();
      this.m_tempResultsCache.Clear();
      this.m_recyclingBuffersCache.Clear();
      foreach (KeyValuePair<ProductProto, PartialQuantityLarge> recyclingBuffer in this.m_recyclingBuffers)
      {
        Quantity quantityClamped = (recyclingBuffer.Value.IntegerPart / 2).ToQuantityClamped();
        if (quantityClamped.IsNotPositive)
        {
          this.m_recyclingBuffersCache.Add(recyclingBuffer.Key, recyclingBuffer.Value);
        }
        else
        {
          this.m_recyclingBuffersCache.Add(recyclingBuffer.Key, recyclingBuffer.Value - new PartialQuantityLarge(quantityClamped * 2));
          Quantity quantity4 = (quantity1 - quantity2).Min(quantityClamped);
          if (quantity4 < quantityClamped)
            quantity3 += quantityClamped - quantity4;
          if (!quantity4.IsNotPositive)
          {
            quantity2 += quantity4;
            this.m_tempResultsCache.Add(new ProductQuantity(recyclingBuffer.Key, quantity4 * 2));
          }
        }
      }
      Swap.Them<Dict<ProductProto, PartialQuantityLarge>>(ref this.m_recyclingBuffers, ref this.m_recyclingBuffersCache);
      if (quantity2.IsPositive)
      {
        this.pushRecyclables(quantity2);
        this.m_productsManager.ProductCreated(this.m_recyclablesProto, quantity2, (IIndexable<ProductQuantity>) this.m_tempResultsCache, CreateReason.Settlement);
      }
      if (this.m_bioWasteInSettlement.IntegerPart.IsPositive)
      {
        this.m_landfillInSettlementPartial += this.m_bioWasteInSettlement.AsPartial;
        this.m_bioWasteInSettlement = PartialQuantityLarge.Zero;
      }
      if (quantity3.IsPositive)
        this.m_landfillInSettlementPartial += quantity3.ScaledBy(this.RecyclableToLandfillMultiplier).AsPartial;
      this.m_landfillInSettlementPartial += this.BaseLandfillPerPopPerDay * this.Population;
      this.updatePartialWaste();
      if (this.m_landfillInSettlementReported.IsPositive)
      {
        Quantity settlementReported = this.m_landfillInSettlementReported;
        this.m_landfillInSettlementReported -= settlementReported - this.pushWasteToLandfill(settlementReported);
      }
      Percent percent = this.Population == 0 || this.m_landfillInSettlementReported.IsZero ? Percent.Zero : Percent.FromRatio(this.m_landfillInSettlementReported.Value, this.LandfillLimitForCurrentPopulation.Value);
      this.NominalHealthPenaltyFromWasteLastDay = !(percent > 100.Percent()) ? Percent.Zero : ((percent - Percent.Hundred) * Settlement.HEALTH_PENALTY_PER_ONE_PERCENT_OF_EXTRA_LANDFILL * 100).Min(Settlement.HEALTH_PENALTY_FROM_WASTE_MAX);
      this.m_healthPenaltySumThisMonth += this.NominalHealthPenaltyFromWasteLastDay.ScaleBy(populationRatio);
    }

    private void updateWasteOnNewMonth()
    {
      Percent reduction = this.m_healthPenaltySumThisMonth / 30;
      if (reduction.IsPositive)
        this.m_healthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.WasteInSettlement, reduction);
      this.m_healthPenaltySumThisMonth = Percent.Zero;
    }

    /// <summary>Returns how much was not stored</summary>
    private Quantity pushWasteToLandfill(Quantity landfillToStore)
    {
      for (int index = 0; index < this.m_landfillModules.Count; ++index)
      {
        this.m_lastUsedLandfillIndex = (this.m_lastUsedLandfillIndex + 1) % this.m_landfillModules.Count;
        landfillToStore = this.m_landfillModules[this.m_lastUsedLandfillIndex].StoreAsMuchAs(landfillToStore);
        if (landfillToStore.IsNotPositive)
          break;
      }
      return landfillToStore;
    }

    /// <summary>Returns how much was not stored</summary>
    private Quantity pushBioWaste(Quantity toStore)
    {
      for (int index = 0; index < this.m_bioWasteModules.Count; ++index)
      {
        this.m_lastUsedBioWasteIndex = (this.m_lastUsedBioWasteIndex + 1) % this.m_bioWasteModules.Count;
        toStore = this.m_bioWasteModules[this.m_lastUsedBioWasteIndex].StoreAsMuchAsAndReport(toStore);
        if (toStore.IsNotPositive)
          break;
      }
      return toStore;
    }

    /// <summary>Returns how much was not stored</summary>
    private void pushRecyclables(Quantity toStore)
    {
      for (int index = 0; index < this.m_recyclableModules.Count; ++index)
      {
        this.m_lastUsedRecyclableIndex = (this.m_lastUsedRecyclableIndex + 1) % this.m_recyclableModules.Count;
        toStore = this.m_recyclableModules[this.m_lastUsedRecyclableIndex].StoreAsMuchAs(toStore);
        if (toStore.IsNotPositive)
          break;
      }
      Assert.That<Quantity>(toStore).IsZero();
    }

    public static void Serialize(Settlement value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Settlement>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Settlement.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<PopNeed>.Serialize(this.AllNeeds, writer);
      writer.WriteInt(this.AmountStarvedToDeathLastMonth);
      writer.WriteBool(this.ArePeopleStarving);
      PartialQuantity.Serialize(this.BaseLandfillPerPopPerDay, writer);
      Percent.Serialize(this.BioWasteReductionMultiplier, writer);
      writer.WriteInt(this.CategoriesWithHealthBenefitTotal);
      Percent.Serialize(this.DiseaseMortalityDeductionLastDay, writer);
      Percent.Serialize(this.FoodCategoriesWithHealthSatisfaction, writer);
      Duration.Serialize(this.FoodDebtDuration, writer);
      writer.WriteBool(this.HasNoFoodModule);
      PartialQuantityLarge.Serialize(this.m_bioWasteInSettlement, writer);
      Lyst<SettlementWasteModule>.Serialize(this.m_bioWasteModules, writer);
      writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_consumptionMultiplier);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      ImmutableArray<Settlement.FoodCategoryData>.Serialize(this.m_foodCategories, writer);
      Percent.Serialize(this.m_foodCategoriesSatisfactionSumPerMonth, writer);
      Lyst<SettlementFoodModule>.Serialize(this.m_foodModules, writer);
      PopNeed.Serialize(this.m_foodNeed, writer);
      Dict<ProductProto, Settlement.FoodData>.Serialize(this.m_foodTypesMap, writer);
      PopNeed.Serialize(this.m_healthcareNeed, writer);
      Percent.Serialize(this.m_healthcareSumLastMonth, writer);
      PopsHealthManager.Serialize(this.m_healthManager, writer);
      Percent.Serialize(this.m_healthPenaltySumThisMonth, writer);
      Lyst<Hospital>.Serialize(this.m_hospitals, writer);
      Lyst<SettlementHousingModule>.Serialize(this.m_housingModules, writer);
      writer.WriteBool(this.m_isDestroyed);
      PartialQuantity.Serialize(this.m_landfillInSettlementPartial, writer);
      Quantity.Serialize(this.m_landfillInSettlementReported, writer);
      Lyst<SettlementWasteModule>.Serialize(this.m_landfillModules, writer);
      writer.WriteGeneric<ProductProto>(this.m_landfillProto);
      writer.WriteInt(this.m_lastUsedBioWasteIndex);
      writer.WriteInt(this.m_lastUsedFoodModuleIndex);
      writer.WriteInt(this.m_lastUsedHospitalIndex);
      writer.WriteInt(this.m_lastUsedLandfillIndex);
      writer.WriteInt(this.m_lastUsedRecyclableIndex);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Lyst<SettlementWasteModule>.Serialize(this.m_recyclableModules, writer);
      writer.WriteGeneric<ProductProto>(this.m_recyclablesProto);
      Dict<ProductProto, PartialQuantityLarge>.Serialize(this.m_recyclingBuffers, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Lyst<ISettlementSquareModule>.Serialize(this.m_squareModules, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_unityProductionMultiplier);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<IProperty<bool>>(this.m_withholdWorkersOnStarvation);
      writer.WriteInt(this.MonthsOfFood);
      Percent.Serialize(this.NominalHealthLastDayFromFood, writer);
      Percent.Serialize(this.NominalHealthPenaltyFromWasteLastDay, writer);
      writer.WriteInt(this.NonSatisfiedFoodDebtDaysPops);
      writer.WriteInt(this.NumberOfWorkersWithheld);
      writer.WriteInt(this.Population);
      Percent.Serialize(this.RecyclableToLandfillMultiplier, writer);
      writer.WriteInt(this.TotalHousingCapacity);
      Upoints.Serialize(this.UpointsCapacity, writer);
      Fix64.Serialize(this.WoodToBiomassMultiplier, writer);
    }

    public static Settlement Deserialize(BlobReader reader)
    {
      Settlement settlement;
      if (reader.TryStartClassDeserialization<Settlement>(out settlement))
        reader.EnqueueDataDeserialization((object) settlement, Settlement.s_deserializeDataDelayedAction);
      return settlement;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Settlement>(this, "AllNeeds", (object) ImmutableArray<PopNeed>.Deserialize(reader));
      this.AmountStarvedToDeathLastMonth = reader.ReadInt();
      this.ArePeopleStarving = reader.ReadBool();
      this.BaseLandfillPerPopPerDay = PartialQuantity.Deserialize(reader);
      reader.SetField<Settlement>(this, "BioWasteReductionMultiplier", (object) Percent.Deserialize(reader));
      this.CategoriesWithHealthBenefitTotal = reader.ReadInt();
      this.DiseaseMortalityDeductionLastDay = Percent.Deserialize(reader);
      this.FoodCategoriesWithHealthSatisfaction = Percent.Deserialize(reader);
      this.FoodDebtDuration = Duration.Deserialize(reader);
      this.HasNoFoodModule = reader.ReadBool();
      this.m_bioWasteInSettlement = PartialQuantityLarge.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_bioWasteModules", (object) Lyst<SettlementWasteModule>.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
      reader.SetField<Settlement>(this, "m_consumptionMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<Settlement>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      this.m_foodCategories = ImmutableArray<Settlement.FoodCategoryData>.Deserialize(reader);
      this.m_foodCategoriesSatisfactionSumPerMonth = Percent.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_foodModules", (object) Lyst<SettlementFoodModule>.Deserialize(reader));
      this.m_foodNeed = PopNeed.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_foodTypesMap", (object) Dict<ProductProto, Settlement.FoodData>.Deserialize(reader));
      this.m_healthcareNeed = PopNeed.Deserialize(reader);
      this.m_healthcareSumLastMonth = Percent.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_healthManager", (object) PopsHealthManager.Deserialize(reader));
      this.m_healthPenaltySumThisMonth = Percent.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_hospitals", (object) Lyst<Hospital>.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_housingModules", (object) Lyst<SettlementHousingModule>.Deserialize(reader));
      this.m_isDestroyed = reader.ReadBool();
      this.m_landfillInSettlementPartial = PartialQuantity.Deserialize(reader);
      this.m_landfillInSettlementReported = Quantity.Deserialize(reader);
      reader.SetField<Settlement>(this, "m_landfillModules", (object) Lyst<SettlementWasteModule>.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_landfillProto", (object) reader.ReadGenericAs<ProductProto>());
      this.m_lastUsedBioWasteIndex = reader.ReadInt();
      this.m_lastUsedFoodModuleIndex = reader.ReadInt();
      this.m_lastUsedHospitalIndex = reader.ReadInt();
      this.m_lastUsedLandfillIndex = reader.ReadInt();
      this.m_lastUsedRecyclableIndex = reader.ReadInt();
      reader.SetField<Settlement>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<Settlement>(this, "m_recyclableModules", (object) Lyst<SettlementWasteModule>.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_recyclablesProto", (object) reader.ReadGenericAs<ProductProto>());
      this.m_recyclingBuffers = Dict<ProductProto, PartialQuantityLarge>.Deserialize(reader);
      this.m_recyclingBuffersCache = new Dict<ProductProto, PartialQuantityLarge>();
      reader.SetField<Settlement>(this, "m_satisfiedNeedsCache", (object) new Set<PopNeedProto>());
      reader.SetField<Settlement>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_sourceProductsCache", (object) new Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>>());
      reader.SetField<Settlement>(this, "m_squareModules", (object) Lyst<ISettlementSquareModule>.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_tempResultsCache", (object) new Lyst<ProductQuantity>());
      reader.SetField<Settlement>(this, "m_unityProductionMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<Settlement>(this, "m_unlockedHousesCache", (object) new Lyst<SettlementHousingModuleProto>());
      reader.SetField<Settlement>(this, "m_unlockedModulesCache", (object) new Lyst<ISettlementModuleForNeedProto>());
      reader.SetField<Settlement>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<Settlement>(this, "m_withholdWorkersOnStarvation", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<bool>>() : (object) (IProperty<bool>) null);
      this.MonthsOfFood = reader.ReadInt();
      this.NominalHealthLastDayFromFood = Percent.Deserialize(reader);
      this.NominalHealthPenaltyFromWasteLastDay = Percent.Deserialize(reader);
      this.NonSatisfiedFoodDebtDaysPops = reader.ReadInt();
      this.NumberOfWorkersWithheld = reader.LoadedSaveVersion >= 140 ? reader.ReadInt() : 0;
      this.Population = reader.ReadInt();
      reader.SetField<Settlement>(this, "RecyclableToLandfillMultiplier", (object) Percent.Deserialize(reader));
      this.TotalHousingCapacity = reader.ReadInt();
      this.UpointsCapacity = Upoints.Deserialize(reader);
      reader.SetField<Settlement>(this, "WoodToBiomassMultiplier", (object) Fix64.Deserialize(reader));
      reader.RegisterInitAfterLoad<Settlement>(this, "initSelf", InitPriority.Normal);
    }

    static Settlement()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Settlement.MAX_WORKERS_WITHHELD_ON_STARVATION = 30.Percent();
      Settlement.UNITY_PENALTY_FOR_STARVATION = 2.Upoints();
      Settlement.HealthBonusPerFoodCategory = 4.Percent();
      Settlement.LANDFILL_CAPACITY_PER100_POPS = 20.Quantity();
      Settlement.MIN_LANDFILL_CAPACITY = 100.Quantity();
      Settlement.HEALTH_PENALTY_PER_ONE_PERCENT_OF_EXTRA_LANDFILL = 0.2.Percent();
      Settlement.HEALTH_PENALTY_FROM_WASTE_MAX = 40.Percent();
      Settlement.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Settlement) obj).SerializeData(writer));
      Settlement.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Settlement) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class FoodCategoryData
    {
      public readonly FoodCategoryProto Prototype;
      public readonly ImmutableArray<Settlement.FoodData> FoodTypes;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public int PopDaysSupplyTemp { get; private set; }

      public int PopsAssignedTemp { get; private set; }

      public int PopsDaysSupplyLeftTemp => this.PopDaysSupplyTemp - this.PopsAssignedTemp;

      public FoodCategoryData(FoodCategoryProto prototype, IEnumerable<FoodProto> foodProtos)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Prototype = prototype;
        this.FoodTypes = foodProtos.Select<FoodProto, Settlement.FoodData>((Func<FoodProto, Settlement.FoodData>) (x => new Settlement.FoodData(x, this))).ToImmutableArray<Settlement.FoodData>();
      }

      public void OnPopDaysSupplyTempDiff(int diff) => this.PopDaysSupplyTemp += diff;

      public void OnPopsAssignedTempDiff(int diff) => this.PopsAssignedTemp += diff;

      public static void Serialize(Settlement.FoodCategoryData value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Settlement.FoodCategoryData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Settlement.FoodCategoryData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ImmutableArray<Settlement.FoodData>.Serialize(this.FoodTypes, writer);
        writer.WriteInt(this.PopDaysSupplyTemp);
        writer.WriteInt(this.PopsAssignedTemp);
        writer.WriteGeneric<FoodCategoryProto>(this.Prototype);
      }

      public static Settlement.FoodCategoryData Deserialize(BlobReader reader)
      {
        Settlement.FoodCategoryData foodCategoryData;
        if (reader.TryStartClassDeserialization<Settlement.FoodCategoryData>(out foodCategoryData))
          reader.EnqueueDataDeserialization((object) foodCategoryData, Settlement.FoodCategoryData.s_deserializeDataDelayedAction);
        return foodCategoryData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Settlement.FoodCategoryData>(this, "FoodTypes", (object) ImmutableArray<Settlement.FoodData>.Deserialize(reader));
        this.PopDaysSupplyTemp = reader.ReadInt();
        this.PopsAssignedTemp = reader.ReadInt();
        reader.SetField<Settlement.FoodCategoryData>(this, "Prototype", (object) reader.ReadGenericAs<FoodCategoryProto>());
      }

      static FoodCategoryData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Settlement.FoodCategoryData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Settlement.FoodCategoryData) obj).SerializeData(writer));
        Settlement.FoodCategoryData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Settlement.FoodCategoryData) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class FoodData
    {
      public readonly FoodProto Prototype;
      public readonly Settlement.FoodCategoryData Category;
      public PartialQuantity FoodLeftToConsume;
      public readonly Lyst<DailyUpointsRecord> DailyRecords;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      /// <summary>
      /// Supply per popDay. So 300 means that 10 pops can survive for 30 days.
      /// </summary>
      public int PopDaysSupplyTemp { get; private set; }

      public int PopsAssignedTemp { get; private set; }

      public Quantity SupplyTemp { get; private set; }

      public Quantity SupplyLeft { get; private set; }

      public PartialQuantity EstimatedMonthlyConsumption { get; private set; }

      public Upoints UpointsGivenLastDay { get; set; }

      public int PopsDaysSupplyLeftTemp => this.PopDaysSupplyTemp - this.PopsAssignedTemp;

      public bool WasSatisfiedLastUpdate { get; set; }

      public FoodData(FoodProto prototype, Settlement.FoodCategoryData category)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.DailyRecords = new Lyst<DailyUpointsRecord>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Prototype = prototype;
        this.Category = category;
      }

      public void AddFoodConsumptionFromAssignedPops(Percent consumptionMult)
      {
        PartialQuantity partialQuantity = this.PopsAssignedTemp <= 0 ? PartialQuantity.Zero : this.Prototype.GetConsumedQuantityFromPopDays(this.PopsAssignedTemp, consumptionMult);
        this.EstimatedMonthlyConsumption = partialQuantity * 30;
        this.FoodLeftToConsume += partialQuantity;
      }

      public void AddSupplyTemp(Quantity quantity, Percent consumptionMult)
      {
        Assert.That<Quantity>(quantity).IsNotNegative();
        this.SupplyTemp += quantity;
        int daysFromQuantity = this.Prototype.GetPopDaysFromQuantity(quantity, consumptionMult);
        this.PopDaysSupplyTemp += daysFromQuantity;
        this.Category.OnPopDaysSupplyTempDiff(daysFromQuantity);
      }

      public void AddPopsAssignedTemp(int value)
      {
        Assert.That<int>(value).IsNotNegative();
        this.PopsAssignedTemp += value;
        this.Category.OnPopsAssignedTempDiff(value);
      }

      public void ResetTempData()
      {
        this.Category.OnPopDaysSupplyTempDiff(-this.PopDaysSupplyTemp);
        this.Category.OnPopsAssignedTempDiff(-this.PopsAssignedTemp);
        this.PopDaysSupplyTemp = 0;
        this.PopsAssignedTemp = 0;
        this.SupplyLeft = (this.SupplyTemp - this.FoodLeftToConsume.IntegerPart).Max(Quantity.Zero);
        this.SupplyTemp = Quantity.Zero;
      }

      public static void Serialize(Settlement.FoodData value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Settlement.FoodData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Settlement.FoodData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Settlement.FoodCategoryData.Serialize(this.Category, writer);
        Lyst<DailyUpointsRecord>.Serialize(this.DailyRecords, writer);
        PartialQuantity.Serialize(this.EstimatedMonthlyConsumption, writer);
        PartialQuantity.Serialize(this.FoodLeftToConsume, writer);
        writer.WriteInt(this.PopDaysSupplyTemp);
        writer.WriteInt(this.PopsAssignedTemp);
        writer.WriteGeneric<FoodProto>(this.Prototype);
        Quantity.Serialize(this.SupplyLeft, writer);
        Quantity.Serialize(this.SupplyTemp, writer);
        Upoints.Serialize(this.UpointsGivenLastDay, writer);
        writer.WriteBool(this.WasSatisfiedLastUpdate);
      }

      public static Settlement.FoodData Deserialize(BlobReader reader)
      {
        Settlement.FoodData foodData;
        if (reader.TryStartClassDeserialization<Settlement.FoodData>(out foodData))
          reader.EnqueueDataDeserialization((object) foodData, Settlement.FoodData.s_deserializeDataDelayedAction);
        return foodData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Settlement.FoodData>(this, "Category", (object) Settlement.FoodCategoryData.Deserialize(reader));
        reader.SetField<Settlement.FoodData>(this, "DailyRecords", (object) Lyst<DailyUpointsRecord>.Deserialize(reader));
        this.EstimatedMonthlyConsumption = PartialQuantity.Deserialize(reader);
        this.FoodLeftToConsume = PartialQuantity.Deserialize(reader);
        this.PopDaysSupplyTemp = reader.ReadInt();
        this.PopsAssignedTemp = reader.ReadInt();
        reader.SetField<Settlement.FoodData>(this, "Prototype", (object) reader.ReadGenericAs<FoodProto>());
        this.SupplyLeft = Quantity.Deserialize(reader);
        this.SupplyTemp = Quantity.Deserialize(reader);
        this.UpointsGivenLastDay = Upoints.Deserialize(reader);
        this.WasSatisfiedLastUpdate = reader.ReadBool();
      }

      static FoodData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Settlement.FoodData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Settlement.FoodData) obj).SerializeData(writer));
        Settlement.FoodData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Settlement.FoodData) obj).DeserializeData(reader));
      }
    }
  }
}
