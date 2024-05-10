// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameDifficultyConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.World.Loans;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Game
{
  /// <summary>
  /// IMPORTANT!
  /// When adding new property make sure to
  /// - add it to AllOptions
  /// - add it to the NewGameWizard UI
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("PowerProductionDiff", 140, typeof (Percent), 0, false)]
  public class GameDifficultyConfig : IConfig
  {
    public static readonly ImmutableArray<GameMechanicApplier> EASY_MECHANICS;
    public static readonly ImmutableArray<GameMechanicApplier> HARD_MECHANICS;
    public static readonly LocStr GameDifficulty__CustomTitle;
    public static readonly LocStr GameDifficulty__EasyTitle;
    public static readonly LocStr GameDifficulty__EasyDescription;
    public static readonly LocStr GameDifficulty__EasyExplanation;
    public static readonly LocStr GameDifficulty__NormalTitle;
    public static readonly LocStr GameDifficulty__NormalDescription;
    public static readonly LocStr GameDifficulty__NormalExplanation;
    public static readonly LocStr GameDifficulty__AdmiralTitle;
    public static readonly LocStr GameDifficulty__AdmiralDescription;
    public static readonly LocStr GameDifficulty__AdmiralExplanation;
    public static readonly DiffSettingInfo<Percent> ExtraStartingMaterialInfo;
    private static readonly ImmutableArray<KeyValuePair<Percent, Percent>> MAINTENANCE_MIGRATION_UPDATE2;
    public static readonly DiffSettingInfo<Percent> MaintenanceDiffInfo;
    private static readonly ImmutableArray<KeyValuePair<Percent, Percent>> FUEL_CONSUMPTION_MIGRATION_UPDATE2;
    public static readonly DiffSettingInfo<Percent> FuelConsumptionDiffInfo;
    public static readonly DiffSettingInfo<Percent> RainYieldDiffInfo;
    public static readonly DiffSettingInfo<Percent> BaseHealthDiffInfo;
    private static readonly ImmutableArray<KeyValuePair<Percent, Percent>> RESOURCE_MINING_MIGRATION_UPDATE2;
    public static readonly DiffSettingInfo<Percent> ResourceMiningDiffInfo;
    public static readonly DiffSettingInfo<Percent> SettlementConsumptionDiffInfo;
    public static readonly DiffSettingInfo<Percent> WorldMinesReservesInfo;
    public static readonly DiffSettingInfo<Percent> FarmYieldInfo;
    public static readonly DiffSettingInfo<Percent> UnityProductionDiffInfo;
    public static readonly DiffSettingInfo<Percent> ConstructionCostsDiffInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.QuickRepairSetting> QuickRepairInfo;
    private static readonly LocStr WeatherOption_LessDry;
    private static readonly LocStr WeatherOption_Dry;
    public static readonly DiffSettingInfo<GameDifficultyConfig.WeatherDifficultySetting> WeatherDifficultyInfo;
    private static readonly LocStr PowerSetting__DoNotConsume;
    private static readonly LocStr PowerSetting__ConsumeIfCan;
    private static readonly LocStr PowerSetting__ConsumeAlways;
    public static readonly DiffSettingInfo<GameDifficultyConfig.LogisticsPowerSetting> PowerSettingInfo;
    private static readonly ImmutableArray<KeyValuePair<Percent, Percent>> TREES_GROWTH_MIGRATION_UPDATE2;
    public static readonly DiffSettingInfo<Percent> TreesGrowthInfo;
    public static readonly DiffSettingInfo<Percent> ExtraContractsProfitInfo;
    private static readonly LocStr RefundOption__Partial;
    private static readonly LocStr RefundOption__Full;
    public static readonly DiffSettingInfo<GameDifficultyConfig.DeconstructionRefundSetting> DeconstructionRefundInfo;
    private static readonly LocStr LoansSetting__Easy;
    private static readonly LocStr LoansSetting__Hard;
    public static readonly DiffSettingInfo<LoansDifficulty> LoansDifficultyInfo;
    private static readonly LocStr DiffOption__RunsOnUnity;
    private static readonly LocStr DiffOption__StopsWorking;
    private static readonly LocStr DiffOption__SlowsDown;
    private static readonly LocStr DiffOption__GraduallyStops;
    public static readonly DiffSettingInfo<GameDifficultyConfig.ShipNoFuelSetting> ShipsNoFuelInfo;
    private static readonly LocStr Option_ReducesThroughput;
    public static readonly DiffSettingInfo<GameDifficultyConfig.GroundwaterPumpLowSetting> GroundwaterPumpLowInfo;
    public static readonly DiffSettingInfo<Percent> ResearchCostDiffInfo;
    private static readonly LocStr StarvationMode__ReducedWorkers;
    private static readonly LocStr StarvationMode__Death;
    public static readonly DiffSettingInfo<GameDifficultyConfig.StarvationSetting> StarvationInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.WorldMinesNoUnitySetting> WorldMinesNoUnityInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.VehiclesNoFuelSetting> VehiclesNoFuelInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.ConsumerBrokenSetting> ConsumerBrokenInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.PowerLowSetting> PowerLowInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.ComputingLowSetting> ComputingLowInfo;
    public static readonly DiffSettingInfo<Percent> QuickActionsCostInfo;
    public static readonly DiffSettingInfo<Percent> DiseaseMortalityDiffInfo;
    public static readonly DiffSettingInfo<GameDifficultyConfig.OreSortingSetting> OreSortingInfo;
    public static readonly DiffSettingInfo<Percent> SolarPowerDiffInfo;
    public static readonly DiffSettingInfo<Percent> PollutionDiffInfo;
    public static ImmutableArray<IDiffSettingInfo> AllOptions;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<GameMechanicApplier> m_selectedMechanics;
    [DoNotSave(0, null)]
    public readonly GameDifficultyPreset? OriginalPreset;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private static LocStr title(string propName, string enTitle)
    {
      return Loc.Str("GameDiff__" + propName, enTitle, "Title of a setting that affects a new game difficulty. For instance 'amount of starting resources'. Check '_Tooltip' for more details explanation.");
    }

    private static LocStr desc(string propName, string enDesc)
    {
      return Loc.Str("GameDiff__" + propName + "_Tooltip", enDesc, "Setting that affects a new game difficulty. For instance 'amount of starting resources'.");
    }

    public static GameDifficultyConfig CreateConfigFor(
      GameDifficultyPreset difficulty,
      bool doNotAddMechanics = false)
    {
      switch (difficulty)
      {
        case GameDifficultyPreset.Easy:
          return GameDifficultyConfig.Easy(doNotAddMechanics);
        case GameDifficultyPreset.Normal:
          return GameDifficultyConfig.Normal(doNotAddMechanics);
        case GameDifficultyPreset.Hard:
          return GameDifficultyConfig.Hard(doNotAddMechanics);
        default:
          Log.Error(string.Format("Unknown difficulty {0}", (object) difficulty));
          return GameDifficultyConfig.Normal(doNotAddMechanics);
      }
    }

    public static GameDifficultyConfig[] AllPresets
    {
      get
      {
        return new GameDifficultyConfig[3]
        {
          GameDifficultyConfig.Easy(),
          GameDifficultyConfig.Normal(),
          GameDifficultyConfig.Hard()
        };
      }
    }

    public static GameDifficultyConfig Easy(bool doNotAddMechanics = false)
    {
      return new GameDifficultyConfig(new GameDifficultyPreset?(GameDifficultyPreset.Easy), GameDifficultyConfig.GameDifficulty__EasyTitle, GameDifficultyConfig.GameDifficulty__EasyDescription, GameDifficultyConfig.GameDifficulty__EasyExplanation, GameDifficultyConfig.ExtraStartingMaterialDefault, -30.Percent(), -50.Percent(), 25.Percent(), -20.Percent(), 20.Percent(), GameDifficultyConfig.WorldMinesReservesDefault, 40.Percent(), 100.Percent(), 25.Percent(), 25.Percent(), GameDifficultyConfig.WeatherDifficultySetting.Easy, 0.Percent(), GameDifficultyConfig.QuickRepairDefault, GameDifficultyConfig.PowerSettingDefault, 20.Percent(), GameDifficultyConfig.DeconstructionRefundDefault, LoansDifficulty.Easy, GameDifficultyConfig.ShipsNoFuelDefault, GameDifficultyConfig.GroundwaterPumpLowDefault, -25.Percent(), GameDifficultyConfig.StarvationSettingDefault, GameDifficultyConfig.WorldMinesNoUnityDefault, GameDifficultyConfig.VehiclesNoFuelDefault, GameDifficultyConfig.ConsumerBrokenModeDefault, GameDifficultyConfig.PowerLowDefault, GameDifficultyConfig.ComputingLowDefault, 0.Percent(), -100.Percent(), GameDifficultyConfig.OreSortingDefault, GameDifficultyConfig.SolarPowerDiffDefault, -25.Percent()).setInitialMechanics(doNotAddMechanics ? (ImmutableArray<GameMechanicApplier>) ImmutableArray.Empty : GameDifficultyConfig.EASY_MECHANICS);
    }

    public static GameDifficultyConfig Normal(bool doNotAddMechanics = false)
    {
      return new GameDifficultyConfig(new GameDifficultyPreset?(GameDifficultyPreset.Normal), GameDifficultyConfig.GameDifficulty__NormalTitle, GameDifficultyConfig.GameDifficulty__NormalDescription, GameDifficultyConfig.GameDifficulty__NormalExplanation, GameDifficultyConfig.ExtraStartingMaterialDefault, Percent.Zero, Percent.Zero, Percent.Zero, Percent.Zero, Percent.Zero, GameDifficultyConfig.WorldMinesReservesDefault, Percent.Zero, 0.Percent(), 0.Percent(), 0.Percent(), GameDifficultyConfig.WeatherDifficultySetting.Standard, Percent.Zero, GameDifficultyConfig.QuickRepairDefault, GameDifficultyConfig.PowerSettingDefault, 0.Percent(), GameDifficultyConfig.DeconstructionRefundDefault, LoansDifficulty.Medium, GameDifficultyConfig.ShipsNoFuelDefault, GameDifficultyConfig.GroundwaterPumpLowDefault, 0.Percent(), GameDifficultyConfig.StarvationSettingDefault, GameDifficultyConfig.WorldMinesNoUnityDefault, GameDifficultyConfig.VehiclesNoFuelDefault, GameDifficultyConfig.ConsumerBrokenModeDefault, GameDifficultyConfig.PowerLowDefault, GameDifficultyConfig.ComputingLowDefault, 0.Percent(), 0.Percent(), GameDifficultyConfig.OreSortingDefault, GameDifficultyConfig.SolarPowerDiffDefault, 0.Percent());
    }

    public static GameDifficultyConfig Hard(bool doNotAddMechanics = false)
    {
      return new GameDifficultyConfig(new GameDifficultyPreset?(GameDifficultyPreset.Hard), GameDifficultyConfig.GameDifficulty__AdmiralTitle, GameDifficultyConfig.GameDifficulty__AdmiralDescription, GameDifficultyConfig.GameDifficulty__AdmiralExplanation, GameDifficultyConfig.ExtraStartingMaterialDefault, 15.Percent(), 25.Percent(), -25.Percent(), 20.Percent(), 20.Percent(), GameDifficultyConfig.WorldMinesReservesDefault, -20.Percent(), 0.Percent(), -25.Percent(), -50.Percent(), GameDifficultyConfig.WeatherDifficultySetting.Dry, 25.Percent(), GameDifficultyConfig.QuickRepairDefault, GameDifficultyConfig.PowerSettingDefault, 0.Percent(), GameDifficultyConfig.DeconstructionRefundDefault, LoansDifficulty.Hard, GameDifficultyConfig.ShipsNoFuelDefault, GameDifficultyConfig.GroundwaterPumpLowDefault, 50.Percent(), GameDifficultyConfig.StarvationSettingDefault, GameDifficultyConfig.WorldMinesNoUnityDefault, GameDifficultyConfig.VehiclesNoFuelDefault, GameDifficultyConfig.ConsumerBrokenModeDefault, GameDifficultyConfig.PowerLowDefault, GameDifficultyConfig.ComputingLowDefault, 50.Percent(), 100.Percent(), GameDifficultyConfig.OreSortingDefault, GameDifficultyConfig.SolarPowerDiffDefault, 25.Percent()).setInitialMechanics(doNotAddMechanics ? (ImmutableArray<GameMechanicApplier>) ImmutableArray.Empty : GameDifficultyConfig.HARD_MECHANICS);
    }

    /// <summary>
    /// E.g. 100% will give twice more material to start with.
    /// </summary>
    public Percent ExtraStartingMaterial { get; private set; }

    public Percent ExtraStartingMaterialMult => 100.Percent() + this.ExtraStartingMaterial;

    public static Percent ExtraStartingMaterialDefault => 0.Percent();

    /// <summary>
    /// E.g. -20% will reduce maintenance, +10% will increase it
    /// </summary>
    public Percent MaintenanceDiff { get; private set; }

    /// <summary>
    /// E.g. -20% will reduce fuel usage for vehicles and cargo ships, +10% will increase it
    /// </summary>
    public Percent FuelConsumptionDiff { get; private set; }

    /// <summary>
    /// E.g. -20% will increase rain yield, +10% will reduce it
    /// </summary>
    public Percent RainYieldDiff { get; private set; }

    /// <summary>
    /// E.g. -20% will reduce base health, +10% will increase it
    /// </summary>
    public Percent BaseHealthDiff { get; private set; }

    /// <summary>
    /// E.g. -20% will mine 20% less material from same quantity, +10% will mine 10% more material
    /// </summary>
    public Percent ResourceMiningDiff { get; private set; }

    /// <summary>E.g. -20% will consume less, +10% will consume more</summary>
    public Percent SettlementConsumptionDiff { get; private set; }

    /// <summary>
    /// -20% reserve - world map mines, oil rigs, research labs consume 20% less.
    /// Try to keep multipliers of 20%.
    /// </summary>
    public Percent WorldMinesReservesDiff { get; private set; }

    public static Percent WorldMinesReservesDefault => Percent.Zero;

    public bool WorldMinesUnlimited => this.WorldMinesReservesDiff == Percent.MaxValue;

    /// <summary>
    /// -20% reserve - world map mines, oil rigs, research labs consume 20% less.
    /// Try to keep multipliers of 20%.
    /// </summary>
    public Percent FarmsYieldDiff { get; private set; }

    private static ImmutableArray<KeyValuePair<Percent, Percent>> FarmsYieldMigration_Update2
    {
      get => GameDifficultyConfig.TREES_GROWTH_MIGRATION_UPDATE2;
    }

    /// <summary>
    /// E.g. 20% will increase unity generated in settlements, -10% will reduce it
    /// </summary>
    public Percent UnityProductionDiff { get; private set; }

    /// <summary>
    /// E.g. 20% will increase construction costs, -20% will reduce them
    /// </summary>
    public Percent ConstructionCostsDiff { get; private set; }

    /// <summary>If true, player is allowed to quick repair</summary>
    public bool CanQuickRepair
    {
      get => this.QuickRepair == GameDifficultyConfig.QuickRepairSetting.Enabled;
    }

    public GameDifficultyConfig.QuickRepairSetting QuickRepair { get; private set; }

    private static GameDifficultyConfig.QuickRepairSetting QuickRepairDefault
    {
      get => GameDifficultyConfig.QuickRepairSetting.Enabled;
    }

    public GameDifficultyConfig.WeatherDifficultySetting WeatherDifficulty { get; private set; }

    public GameDifficultyConfig.LogisticsPowerSetting PowerSetting { get; private set; }

    private static GameDifficultyConfig.LogisticsPowerSetting PowerSettingDefault
    {
      get => GameDifficultyConfig.LogisticsPowerSetting.ConsumeIfPossible;
    }

    public Percent TreesGrowthDiff { get; private set; }

    [NewInSaveVersion(97, null, "Percent.Zero", null, null)]
    public Percent ExtraContractsProfit { get; private set; }

    [NewInSaveVersion(140, null, "DeconstructionRefundSetting.Partial", null, null)]
    public GameDifficultyConfig.DeconstructionRefundSetting DeconstructionRefund { get; private set; }

    public static GameDifficultyConfig.DeconstructionRefundSetting DeconstructionRefundDefault
    {
      get => GameDifficultyConfig.DeconstructionRefundSetting.Partial;
    }

    [NewInSaveVersion(140, null, "LoansDifficulty.Medium", null, null)]
    public LoansDifficulty LoansDifficulty { get; private set; }

    /// <summary>
    /// If true, player can spend Unity so cargo ships move around without fuel
    /// </summary>
    [NewInSaveVersion(140, null, "ShipNoFuelSetting.StopWorking", null, null)]
    public GameDifficultyConfig.ShipNoFuelSetting ShipsNoFuel { get; private set; }

    private static GameDifficultyConfig.ShipNoFuelSetting ShipsNoFuelDefault
    {
      get => GameDifficultyConfig.ShipNoFuelSetting.RunOnUnity;
    }

    [NewInSaveVersion(140, null, "GroundwaterPumpLowSetting.StopWorking", null, null)]
    public GameDifficultyConfig.GroundwaterPumpLowSetting GroundwaterPumpLow { get; private set; }

    private static GameDifficultyConfig.GroundwaterPumpLowSetting GroundwaterPumpLowDefault
    {
      get => GameDifficultyConfig.GroundwaterPumpLowSetting.SlowDown;
    }

    [NewInSaveVersion(140, null, "0.Percent()", null, null)]
    public Percent ResearchCostDiff { get; private set; }

    [NewInSaveVersion(140, null, "StarvationSetting.Death", null, null)]
    public GameDifficultyConfig.StarvationSetting Starvation { get; private set; }

    private static GameDifficultyConfig.StarvationSetting StarvationSettingDefault
    {
      get => GameDifficultyConfig.StarvationSetting.ReducedWorkforce;
    }

    [NewInSaveVersion(140, null, "WorldMinesNoUnitySetting.Stop", null, null)]
    public GameDifficultyConfig.WorldMinesNoUnitySetting WorldMinesNoUnity { get; private set; }

    private static GameDifficultyConfig.WorldMinesNoUnitySetting WorldMinesNoUnityDefault
    {
      get => GameDifficultyConfig.WorldMinesNoUnitySetting.Stop;
    }

    [NewInSaveVersion(140, null, "VehiclesNoFuelSetting.Stop", null, null)]
    public GameDifficultyConfig.VehiclesNoFuelSetting VehiclesNoFuel { get; private set; }

    private static GameDifficultyConfig.VehiclesNoFuelSetting VehiclesNoFuelDefault
    {
      get => GameDifficultyConfig.VehiclesNoFuelSetting.SlowDown;
    }

    [NewInSaveVersion(140, null, "ConsumerBrokenSetting.Stop", null, null)]
    public GameDifficultyConfig.ConsumerBrokenSetting ConsumerBroken { get; private set; }

    private static GameDifficultyConfig.ConsumerBrokenSetting ConsumerBrokenModeDefault
    {
      get => GameDifficultyConfig.ConsumerBrokenSetting.SlowDown;
    }

    [NewInSaveVersion(140, null, "PowerLowSetting.Stop", null, null)]
    public GameDifficultyConfig.PowerLowSetting PowerLow { get; private set; }

    private static GameDifficultyConfig.PowerLowSetting PowerLowDefault
    {
      get => GameDifficultyConfig.PowerLowSetting.Stop;
    }

    [NewInSaveVersion(140, null, "ComputingLowSetting.Stop", null, null)]
    public GameDifficultyConfig.ComputingLowSetting ComputingLow { get; private set; }

    private static GameDifficultyConfig.ComputingLowSetting ComputingLowDefault
    {
      get => GameDifficultyConfig.ComputingLowSetting.Stop;
    }

    [NewInSaveVersion(140, null, "0.Percent()", null, null)]
    public Percent QuickActionsCostDiff { get; private set; }

    [NewInSaveVersion(140, null, "0.Percent()", null, null)]
    public Percent DiseaseMortalityDiff { get; private set; }

    [NewInSaveVersion(140, null, "OreSortingSetting.Disabled", null, null)]
    public GameDifficultyConfig.OreSortingSetting OreSorting { get; private set; }

    private static GameDifficultyConfig.OreSortingSetting OreSortingDefault
    {
      get => GameDifficultyConfig.OreSortingSetting.Disabled;
    }

    [NewInSaveVersion(140, null, "0.Percent()", null, null)]
    public Percent SolarPowerDiff { get; private set; }

    private static Percent SolarPowerDiffDefault => Percent.Zero;

    [NewInSaveVersion(140, null, "0.Percent()", null, null)]
    public Percent PollutionDiff { get; private set; }

    public LocStr Name { get; private set; }

    public LocStr Description { get; private set; }

    public LocStr Explanation { get; private set; }

    public IIndexable<GameMechanicApplier> SelectedMechanics
    {
      get => (IIndexable<GameMechanicApplier>) this.m_selectedMechanics;
    }

    private GameDifficultyConfig(
      GameDifficultyPreset? originalPreset,
      LocStr name,
      LocStr description,
      LocStr explanation)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_selectedMechanics = new Lyst<GameMechanicApplier>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OriginalPreset = originalPreset;
      this.Name = name;
      this.Description = description;
      this.Explanation = explanation;
    }

    private GameDifficultyConfig(
      GameDifficultyPreset? originalPreset,
      LocStr name,
      LocStr description,
      LocStr explanation,
      Percent extraStartingMaterial,
      Percent fuelConsumptionDiff,
      Percent maintenanceDiff,
      Percent resourceMiningDiff,
      Percent settlementConsumptionDiff,
      Percent unityProductionDiff,
      Percent worldMinesReservesDiff,
      Percent rainYieldDiff,
      Percent baseHealthDiff,
      Percent farmsYieldDiff,
      Percent treeGrowthDiff,
      GameDifficultyConfig.WeatherDifficultySetting weatherDifficulty,
      Percent constructionCostsDiff,
      GameDifficultyConfig.QuickRepairSetting quickRepair,
      GameDifficultyConfig.LogisticsPowerSetting powerSetting,
      Percent extraContractsProfit,
      GameDifficultyConfig.DeconstructionRefundSetting deconstructionRefund,
      LoansDifficulty loansDifficulty,
      GameDifficultyConfig.ShipNoFuelSetting shipsNoFuel,
      GameDifficultyConfig.GroundwaterPumpLowSetting groundwaterPumpLow,
      Percent researchCostDiff,
      GameDifficultyConfig.StarvationSetting starvation,
      GameDifficultyConfig.WorldMinesNoUnitySetting worldMinesNoUnity,
      GameDifficultyConfig.VehiclesNoFuelSetting vehiclesNoFuel,
      GameDifficultyConfig.ConsumerBrokenSetting consumerBroken,
      GameDifficultyConfig.PowerLowSetting powerLow,
      GameDifficultyConfig.ComputingLowSetting computingLow,
      Percent quickActionsCostDiff,
      Percent diseaseMortalityDiff,
      GameDifficultyConfig.OreSortingSetting oreSorting,
      Percent solarPowerDiff,
      Percent pollutionDiff)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_selectedMechanics = new Lyst<GameMechanicApplier>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OriginalPreset = originalPreset;
      this.Name = name;
      this.Description = description;
      this.Explanation = explanation;
      this.ExtraStartingMaterial = extraStartingMaterial;
      this.FuelConsumptionDiff = fuelConsumptionDiff;
      this.MaintenanceDiff = maintenanceDiff;
      this.ResourceMiningDiff = resourceMiningDiff;
      this.SettlementConsumptionDiff = settlementConsumptionDiff;
      this.UnityProductionDiff = unityProductionDiff;
      this.WorldMinesReservesDiff = worldMinesReservesDiff;
      this.RainYieldDiff = rainYieldDiff;
      this.BaseHealthDiff = baseHealthDiff;
      this.FarmsYieldDiff = farmsYieldDiff;
      this.TreesGrowthDiff = treeGrowthDiff;
      this.WeatherDifficulty = weatherDifficulty;
      this.ConstructionCostsDiff = constructionCostsDiff;
      this.QuickRepair = quickRepair;
      this.PowerSetting = powerSetting;
      this.ExtraContractsProfit = extraContractsProfit;
      this.DeconstructionRefund = deconstructionRefund;
      this.LoansDifficulty = loansDifficulty;
      this.ShipsNoFuel = shipsNoFuel;
      this.GroundwaterPumpLow = groundwaterPumpLow;
      this.ResearchCostDiff = researchCostDiff;
      this.Starvation = starvation;
      this.WorldMinesNoUnity = worldMinesNoUnity;
      this.VehiclesNoFuel = vehiclesNoFuel;
      this.ConsumerBroken = consumerBroken;
      this.PowerLow = powerLow;
      this.ComputingLow = computingLow;
      this.QuickActionsCostDiff = quickActionsCostDiff;
      this.DiseaseMortalityDiff = diseaseMortalityDiff;
      this.OreSorting = oreSorting;
      this.SolarPowerDiff = solarPowerDiff;
      this.PollutionDiff = pollutionDiff;
    }

    private GameDifficultyConfig setInitialMechanics(ImmutableArray<GameMechanicApplier> mechanics)
    {
      Assert.That<Lyst<GameMechanicApplier>>(this.m_selectedMechanics).IsEmpty<GameMechanicApplier>();
      foreach (GameMechanicApplier mechanic in mechanics)
      {
        this.m_selectedMechanics.Add(mechanic);
        mechanic.ApplyTo(this);
      }
      return this;
    }

    /// <summary>
    /// In update 2 we changed difficulties percent ranges and this code migrate them.
    /// </summary>
    [OnlyForSaveCompatibility(null)]
    internal void MigrateForUpdate2()
    {
      this.TreesGrowthDiff = migrate(this.TreesGrowthDiff, GameDifficultyConfig.TREES_GROWTH_MIGRATION_UPDATE2);
      this.FarmsYieldDiff = migrate(this.FarmsYieldDiff, GameDifficultyConfig.FarmsYieldMigration_Update2);
      this.MaintenanceDiff = migrate(this.MaintenanceDiff, GameDifficultyConfig.MAINTENANCE_MIGRATION_UPDATE2);
      this.FuelConsumptionDiff = migrate(this.FuelConsumptionDiff, GameDifficultyConfig.FUEL_CONSUMPTION_MIGRATION_UPDATE2);
      this.ResourceMiningDiff = migrate(this.ResourceMiningDiff, GameDifficultyConfig.RESOURCE_MINING_MIGRATION_UPDATE2);

      static Percent migrate(
        Percent value,
        ImmutableArray<KeyValuePair<Percent, Percent>> table)
      {
        foreach (KeyValuePair<Percent, Percent> keyValuePair in table)
        {
          if (value == keyValuePair.Key)
            return keyValuePair.Value;
        }
        return value;
      }
    }

    public void AddMechanic(GameMechanicApplier mechanic)
    {
      if (this.m_selectedMechanics.Contains(mechanic))
        return;
      foreach (GameMechanicApplier conflict in mechanic.Conflicts)
      {
        if (this.m_selectedMechanics.Contains(conflict))
          this.RemoveMechanic(conflict, true);
      }
      foreach (GameMechanicApplier mechanic1 in this.m_selectedMechanics.ToArray())
      {
        if (mechanic1.Conflicts.Contains<GameMechanicApplier>(mechanic))
          this.RemoveMechanic(mechanic1, true);
      }
      foreach (GameMechanicApplier dependency in mechanic.Dependencies)
      {
        if (!this.m_selectedMechanics.Contains(dependency))
          this.AddMechanic(dependency);
      }
      this.m_selectedMechanics.Add(mechanic);
      this.reapplyAllMechanics();
    }

    public void ToggleMechanic(GameMechanicApplier mechanic)
    {
      if (this.m_selectedMechanics.Contains(mechanic))
        this.RemoveMechanic(mechanic);
      else
        this.AddMechanic(mechanic);
    }

    public void RemoveMechanic(GameMechanicApplier mechanic, bool doNotUpdate = false)
    {
      if (!this.m_selectedMechanics.Contains(mechanic))
        return;
      Queueue<GameMechanicApplier> queueue = new Queueue<GameMechanicApplier>();
      this.m_selectedMechanics.Remove(mechanic);
      queueue.Enqueue(mechanic);
      while (queueue.IsNotEmpty)
      {
        GameMechanicApplier gameMechanicApplier1 = queueue.PopLast();
        foreach (GameMechanicApplier gameMechanicApplier2 in this.m_selectedMechanics.ToArray())
        {
          if (gameMechanicApplier2.Dependencies.Contains<GameMechanicApplier>(gameMechanicApplier1))
          {
            queueue.Enqueue(gameMechanicApplier2);
            this.m_selectedMechanics.Remove(gameMechanicApplier2);
          }
        }
      }
      if (doNotUpdate)
        return;
      this.reapplyAllMechanics();
    }

    private void reapplyAllMechanics()
    {
      if (this.OriginalPreset.HasValue)
        this.OverrideWithValuesFrom(GameDifficultyConfig.CreateConfigFor(this.OriginalPreset.Value, true));
      this.setInitialMechanics(this.m_selectedMechanics.ToImmutableArrayAndClear());
    }

    public GameDifficultyConfig Clone()
    {
      GameDifficultyConfig difficultyConfig = new GameDifficultyConfig(this.OriginalPreset, this.Name, this.Description, this.Explanation);
      difficultyConfig.OverrideWithValuesFrom(this);
      return difficultyConfig;
    }

    public void OverrideWithValuesFrom(GameDifficultyConfig other)
    {
      foreach (IDiffSettingInfo allOption in GameDifficultyConfig.AllOptions)
        allOption.OverrideTargetWithSource(this, other);
    }

    public bool IsSameAs(GameDifficultyConfig other)
    {
      foreach (IDiffSettingInfo allOption in GameDifficultyConfig.AllOptions)
      {
        if (!allOption.AreSame(this, other))
          return false;
      }
      return true;
    }

    public ImmutableArray<GameDifficultyOptionChange> GenerateDiff(GameDifficultyConfig newConfig)
    {
      Lyst<GameDifficultyOptionChange> lyst = new Lyst<GameDifficultyOptionChange>();
      foreach (IDiffSettingInfo allOption in GameDifficultyConfig.AllOptions)
      {
        Option<GameDifficultyOptionChange> diff = allOption.GetDiff(this, newConfig);
        if (diff.HasValue)
          lyst.Add(diff.Value);
      }
      return lyst.ToImmutableArray();
    }

    private static EnumSettingInfo<T> createForEnum<T>(
      string memberName,
      LocStrFormatted title,
      LocStrFormatted tooltip,
      LocStrFormatted[] labels)
      where T : Enum
    {
      return new EnumSettingInfo<T>(memberName, title, tooltip, labels);
    }

    public static void Serialize(GameDifficultyConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameDifficultyConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameDifficultyConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.BaseHealthDiff, writer);
      writer.WriteInt((int) this.ComputingLow);
      Percent.Serialize(this.ConstructionCostsDiff, writer);
      writer.WriteInt((int) this.ConsumerBroken);
      writer.WriteInt((int) this.DeconstructionRefund);
      LocStr.Serialize(this.Description, writer);
      Percent.Serialize(this.DiseaseMortalityDiff, writer);
      LocStr.Serialize(this.Explanation, writer);
      Percent.Serialize(this.ExtraContractsProfit, writer);
      Percent.Serialize(this.ExtraStartingMaterial, writer);
      Percent.Serialize(this.FarmsYieldDiff, writer);
      Percent.Serialize(this.FuelConsumptionDiff, writer);
      writer.WriteInt((int) this.GroundwaterPumpLow);
      writer.WriteInt((int) this.LoansDifficulty);
      Percent.Serialize(this.MaintenanceDiff, writer);
      LocStr.Serialize(this.Name, writer);
      writer.WriteInt((int) this.OreSorting);
      Percent.Serialize(this.PollutionDiff, writer);
      writer.WriteInt((int) this.PowerLow);
      writer.WriteInt((int) this.PowerSetting);
      Percent.Serialize(this.QuickActionsCostDiff, writer);
      writer.WriteInt((int) this.QuickRepair);
      Percent.Serialize(this.RainYieldDiff, writer);
      Percent.Serialize(this.ResearchCostDiff, writer);
      Percent.Serialize(this.ResourceMiningDiff, writer);
      Percent.Serialize(this.SettlementConsumptionDiff, writer);
      writer.WriteInt((int) this.ShipsNoFuel);
      Percent.Serialize(this.SolarPowerDiff, writer);
      writer.WriteInt((int) this.Starvation);
      Percent.Serialize(this.TreesGrowthDiff, writer);
      Percent.Serialize(this.UnityProductionDiff, writer);
      writer.WriteInt((int) this.VehiclesNoFuel);
      writer.WriteInt((int) this.WeatherDifficulty);
      writer.WriteInt((int) this.WorldMinesNoUnity);
      Percent.Serialize(this.WorldMinesReservesDiff, writer);
    }

    public static GameDifficultyConfig Deserialize(BlobReader reader)
    {
      GameDifficultyConfig difficultyConfig;
      if (reader.TryStartClassDeserialization<GameDifficultyConfig>(out difficultyConfig))
        reader.EnqueueDataDeserialization((object) difficultyConfig, GameDifficultyConfig.s_deserializeDataDelayedAction);
      return difficultyConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BaseHealthDiff = Percent.Deserialize(reader);
      this.ComputingLow = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.ComputingLowSetting) reader.ReadInt() : GameDifficultyConfig.ComputingLowSetting.Stop;
      this.ConstructionCostsDiff = Percent.Deserialize(reader);
      this.ConsumerBroken = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.ConsumerBrokenSetting) reader.ReadInt() : GameDifficultyConfig.ConsumerBrokenSetting.Stop;
      this.DeconstructionRefund = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.DeconstructionRefundSetting) reader.ReadInt() : GameDifficultyConfig.DeconstructionRefundSetting.Partial;
      this.Description = LocStr.Deserialize(reader);
      this.DiseaseMortalityDiff = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : 0.Percent();
      this.Explanation = LocStr.Deserialize(reader);
      this.ExtraContractsProfit = reader.LoadedSaveVersion >= 97 ? Percent.Deserialize(reader) : Percent.Zero;
      this.ExtraStartingMaterial = Percent.Deserialize(reader);
      this.FarmsYieldDiff = Percent.Deserialize(reader);
      this.FuelConsumptionDiff = Percent.Deserialize(reader);
      this.GroundwaterPumpLow = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.GroundwaterPumpLowSetting) reader.ReadInt() : GameDifficultyConfig.GroundwaterPumpLowSetting.StopWorking;
      this.LoansDifficulty = reader.LoadedSaveVersion >= 140 ? (LoansDifficulty) reader.ReadInt() : LoansDifficulty.Medium;
      reader.SetField<GameDifficultyConfig>(this, "m_selectedMechanics", (object) new Lyst<GameMechanicApplier>());
      this.MaintenanceDiff = Percent.Deserialize(reader);
      this.Name = LocStr.Deserialize(reader);
      this.OreSorting = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.OreSortingSetting) reader.ReadInt() : GameDifficultyConfig.OreSortingSetting.Disabled;
      this.PollutionDiff = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : 0.Percent();
      this.PowerLow = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.PowerLowSetting) reader.ReadInt() : GameDifficultyConfig.PowerLowSetting.Stop;
      if (reader.LoadedSaveVersion < 140)
        Percent.Deserialize(reader);
      this.PowerSetting = (GameDifficultyConfig.LogisticsPowerSetting) reader.ReadInt();
      this.QuickActionsCostDiff = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : 0.Percent();
      this.QuickRepair = (GameDifficultyConfig.QuickRepairSetting) reader.ReadInt();
      this.RainYieldDiff = Percent.Deserialize(reader);
      this.ResearchCostDiff = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : 0.Percent();
      this.ResourceMiningDiff = Percent.Deserialize(reader);
      this.SettlementConsumptionDiff = Percent.Deserialize(reader);
      this.ShipsNoFuel = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.ShipNoFuelSetting) reader.ReadInt() : GameDifficultyConfig.ShipNoFuelSetting.StopWorking;
      this.SolarPowerDiff = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : 0.Percent();
      this.Starvation = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.StarvationSetting) reader.ReadInt() : GameDifficultyConfig.StarvationSetting.Death;
      this.TreesGrowthDiff = Percent.Deserialize(reader);
      this.UnityProductionDiff = Percent.Deserialize(reader);
      this.VehiclesNoFuel = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.VehiclesNoFuelSetting) reader.ReadInt() : GameDifficultyConfig.VehiclesNoFuelSetting.Stop;
      this.WeatherDifficulty = (GameDifficultyConfig.WeatherDifficultySetting) reader.ReadInt();
      this.WorldMinesNoUnity = reader.LoadedSaveVersion >= 140 ? (GameDifficultyConfig.WorldMinesNoUnitySetting) reader.ReadInt() : GameDifficultyConfig.WorldMinesNoUnitySetting.Stop;
      this.WorldMinesReservesDiff = Percent.Deserialize(reader);
    }

    static GameDifficultyConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameDifficultyConfig.EASY_MECHANICS = ImmutableArray.Create<GameMechanicApplier>(GameMechanics.Casual, GameMechanics.ResourcesBoost);
      GameDifficultyConfig.HARD_MECHANICS = ImmutableArray.Create<GameMechanicApplier>(GameMechanics.OreSorting, GameMechanics.Realism, GameMechanics.RealismPlus, GameMechanics.ReducedWorldMines);
      GameDifficultyConfig.GameDifficulty__CustomTitle = Loc.Str(nameof (GameDifficulty__CustomTitle), "Custom", "title of a custom game difficulty setting, in this setting the player can customize many game settings individually");
      GameDifficultyConfig.GameDifficulty__EasyTitle = Loc.Str(nameof (GameDifficulty__EasyTitle), "Sailor", "title of game difficulty setting (this is easy difficulty)");
      GameDifficultyConfig.GameDifficulty__EasyDescription = Loc.Str(nameof (GameDifficulty__EasyDescription), "Additional bonuses and mechanics make this highly recommended for new players", "description of game difficulty setting (this is easy difficulty)");
      GameDifficultyConfig.GameDifficulty__EasyExplanation = Loc.Str(nameof (GameDifficulty__EasyExplanation), "For players who want a smooth sail", "clear description of game difficulty setting (this is easy difficulty)");
      GameDifficultyConfig.GameDifficulty__NormalTitle = Loc.Str(nameof (GameDifficulty__NormalTitle), "Captain", "title of game difficulty setting (this is standard difficulty)");
      GameDifficultyConfig.GameDifficulty__NormalDescription = Loc.Str(nameof (GameDifficulty__NormalDescription), "An experience that balances consumption and production for more challenge", "description of game difficulty setting (this is standard difficulty)");
      GameDifficultyConfig.GameDifficulty__NormalExplanation = Loc.Str(nameof (GameDifficulty__NormalExplanation), "For players who seek some adventure", "clear description of game difficulty setting (this is standard difficulty)");
      GameDifficultyConfig.GameDifficulty__AdmiralTitle = Loc.Str(nameof (GameDifficulty__AdmiralTitle), "Admiral", "title of game difficulty setting (this is hard difficulty)");
      GameDifficultyConfig.GameDifficulty__AdmiralDescription = Loc.Str(nameof (GameDifficulty__AdmiralDescription), "A tough challenge with unforgiving mechanics for experienced Captains", "description of difficulty setting (this is hard difficulty)");
      GameDifficultyConfig.GameDifficulty__AdmiralExplanation = Loc.Str(nameof (GameDifficulty__AdmiralExplanation), "For experienced players only", "clear description of difficulty setting (this is hard difficulty)");
      GameDifficultyConfig.ExtraStartingMaterialInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (ExtraStartingMaterial), (LocStrFormatted) GameDifficultyConfig.title(nameof (ExtraStartingMaterial), "Extra starting materials"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (ExtraStartingMaterial), "Extra starting materials and materials returned when scrapping ruined buildings. Affects also extra size of island's crude oil deposits."), new Percent[3]
      {
        0.Percent(),
        40.Percent(),
        80.Percent()
      }, false);
      GameDifficultyConfig.MAINTENANCE_MIGRATION_UPDATE2 = ImmutableArray.Create<KeyValuePair<Percent, Percent>>(Make.Kvp<Percent, Percent>(-80.Percent(), -75.Percent()), Make.Kvp<Percent, Percent>(-60.Percent(), -50.Percent()), Make.Kvp<Percent, Percent>(-40.Percent(), -50.Percent()), Make.Kvp<Percent, Percent>(-20.Percent(), -25.Percent()), Make.Kvp<Percent, Percent>(20.Percent(), 25.Percent()), Make.Kvp<Percent, Percent>(40.Percent(), 50.Percent()));
      GameDifficultyConfig.MaintenanceDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (MaintenanceDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (MaintenanceDiff), "Maintenance consumption"), LocStrFormatted.Empty, new Percent[6]
      {
        -75.Percent(),
        -50.Percent(),
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent()
      }, true);
      GameDifficultyConfig.FUEL_CONSUMPTION_MIGRATION_UPDATE2 = ImmutableArray.Create<KeyValuePair<Percent, Percent>>(Make.Kvp<Percent, Percent>(-10.Percent(), -15.Percent()), Make.Kvp<Percent, Percent>(10.Percent(), 15.Percent()), Make.Kvp<Percent, Percent>(20.Percent(), 30.Percent()));
      GameDifficultyConfig.FuelConsumptionDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (FuelConsumptionDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (FuelConsumptionDiff), "Fuel consumption"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (FuelConsumptionDiff), "Affects fuel consumption of vehicles and cargo ships."), new Percent[5]
      {
        -30.Percent(),
        -15.Percent(),
        0.Percent(),
        15.Percent(),
        30.Percent()
      }, true);
      GameDifficultyConfig.RainYieldDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (RainYieldDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (RainYieldDiff), "Rainwater yield"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (RainYieldDiff), "Affects how much water is generated from rain."), new Percent[5]
      {
        -40.Percent(),
        -20.Percent(),
        0.Percent(),
        40.Percent(),
        80.Percent()
      }, false);
      GameDifficultyConfig.BaseHealthDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (BaseHealthDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (BaseHealthDiff), "Base health"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (BaseHealthDiff), "Affects the baseline health of your population."), new Percent[3]
      {
        0.Percent(),
        100.Percent(),
        200.Percent()
      }, false);
      GameDifficultyConfig.RESOURCE_MINING_MIGRATION_UPDATE2 = ImmutableArray.Create<KeyValuePair<Percent, Percent>>(Make.Kvp<Percent, Percent>(-10.Percent(), -15.Percent()), Make.Kvp<Percent, Percent>(10.Percent(), 15.Percent()), Make.Kvp<Percent, Percent>(30.Percent(), 25.Percent()));
      GameDifficultyConfig.ResourceMiningDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (ResourceMiningDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (ResourceMiningDiff), "Ore mining yield"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (ResourceMiningDiff), "Increases or reduces the mined amount of ores and sand. If increased, excavators will mine more ore from the same-sized deposit compared to what they would mine for standard setting."), new Percent[8]
      {
        -75.Percent(),
        -50.Percent(),
        -25.Percent(),
        -10.Percent(),
        0.Percent(),
        15.Percent(),
        25.Percent(),
        50.Percent()
      }, false);
      GameDifficultyConfig.SettlementConsumptionDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (SettlementConsumptionDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (SettlementConsumptionDiff), "Settlement consumption"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (SettlementConsumptionDiff), "Affects how much food, services, goods is consumed by your population."), new Percent[5]
      {
        -40.Percent(),
        -20.Percent(),
        0.Percent(),
        20.Percent(),
        40.Percent()
      }, true);
      GameDifficultyConfig.WorldMinesReservesInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (WorldMinesReservesDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (WorldMinesReservesDiff), "World mines deposits"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (WorldMinesReservesDiff), "Affects size of deposits in the world mines (for instance quartz mines or oil rigs)."), new Percent[8]
      {
        -80.Percent(),
        -60.Percent(),
        -40.Percent(),
        0.Percent(),
        100.Percent(),
        200.Percent(),
        500.Percent(),
        Percent.MaxValue
      }, false);
      GameDifficultyConfig.FarmYieldInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (FarmsYieldDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (FarmsYieldDiff), "Farms yield"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (FarmsYieldDiff), "Affects yield of all farms and greenhouses."), new Percent[5]
      {
        -50.Percent(),
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent()
      }, false);
      GameDifficultyConfig.UnityProductionDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (UnityProductionDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (UnityProductionDiff), "Unity generation"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (UnityProductionDiff), "Affects how much Unity is produced in settlements."), new Percent[3]
      {
        0.Percent(),
        20.Percent(),
        40.Percent()
      }, false);
      GameDifficultyConfig.ConstructionCostsDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (ConstructionCostsDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (ConstructionCostsDiff), "Construction costs"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (ConstructionCostsDiff), "Affects construction costs of entities such as machines, building, vehicles."), new Percent[5]
      {
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent(),
        100.Percent()
      }, true);
      GameDifficultyConfig.QuickRepairInfo = (DiffSettingInfo<GameDifficultyConfig.QuickRepairSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.QuickRepairSetting>(nameof (QuickRepair), (LocStrFormatted) GameDifficultyConfig.title(nameof (QuickRepair), "Quick repair"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) TrCore.Enabled,
        (LocStrFormatted) TrCore.Disabled
      });
      GameDifficultyConfig.WeatherOption_LessDry = Loc.Str(nameof (WeatherOption_LessDry), "Less dry", "sets weather configuration to be less dry than it normally is");
      GameDifficultyConfig.WeatherOption_Dry = Loc.Str(nameof (WeatherOption_Dry), "Dry", "sets weather configuration to be dry");
      GameDifficultyConfig.WeatherDifficultyInfo = (DiffSettingInfo<GameDifficultyConfig.WeatherDifficultySetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.WeatherDifficultySetting>(nameof (WeatherDifficulty), (LocStrFormatted) GameDifficultyConfig.title(nameof (WeatherDifficulty), "Weather"), LocStrFormatted.Empty, new LocStrFormatted[3]
      {
        (LocStrFormatted) GameDifficultyConfig.WeatherOption_LessDry,
        (LocStrFormatted) TrCore.OptionValStandard,
        (LocStrFormatted) GameDifficultyConfig.WeatherOption_Dry
      });
      GameDifficultyConfig.PowerSetting__DoNotConsume = Loc.Str(nameof (PowerSetting__DoNotConsume), "Never consume", "if set, belts will never consumer power");
      GameDifficultyConfig.PowerSetting__ConsumeIfCan = Loc.Str(nameof (PowerSetting__ConsumeIfCan), "Consume if can", "if set, belts will consume power if available");
      GameDifficultyConfig.PowerSetting__ConsumeAlways = Loc.Str(nameof (PowerSetting__ConsumeAlways), "Always consume", "if set, belts will always require power otherwise won't work");
      GameDifficultyConfig.PowerSettingInfo = (DiffSettingInfo<GameDifficultyConfig.LogisticsPowerSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.LogisticsPowerSetting>(nameof (PowerSetting), (LocStrFormatted) GameDifficultyConfig.title(nameof (PowerSetting), "Power for belts & storages"), Loc.Str2("GameDiff__PowerSetting_Tooltip", "Determines whether belts & storages will consume power and how. If power is set to '{0}', belts & storages will NOT work when low on power. If set to '{1}', power is consumed with the highest priority but belts & storages will still work despite having no power. Finally, power consumption for belts & storages can be disabled entirely.", "Setting that affects a new game difficulty. For instance 'amount of starting resources'. {0} - PowerSetting__ConsumeAlways, {1} - PowerSetting__ConsumeIfCan").Format(GameDifficultyConfig.PowerSetting__ConsumeAlways.TranslatedString, GameDifficultyConfig.PowerSetting__ConsumeIfCan.TranslatedString), new LocStrFormatted[3]
      {
        (LocStrFormatted) GameDifficultyConfig.PowerSetting__DoNotConsume,
        (LocStrFormatted) GameDifficultyConfig.PowerSetting__ConsumeIfCan,
        (LocStrFormatted) GameDifficultyConfig.PowerSetting__ConsumeAlways
      });
      GameDifficultyConfig.TREES_GROWTH_MIGRATION_UPDATE2 = ImmutableArray.Create<KeyValuePair<Percent, Percent>>(Make.Kvp<Percent, Percent>(-40.Percent(), -50.Percent()), Make.Kvp<Percent, Percent>(-20.Percent(), -25.Percent()), Make.Kvp<Percent, Percent>(20.Percent(), 25.Percent()), Make.Kvp<Percent, Percent>(40.Percent(), 50.Percent()));
      GameDifficultyConfig.TreesGrowthInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (TreesGrowthDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (TreesGrowthDiff), "Trees growth speed"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (TreesGrowthDiff), "Affects how fast trees grow."), new Percent[5]
      {
        -50.Percent(),
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent()
      }, false);
      GameDifficultyConfig.ExtraContractsProfitInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (ExtraContractsProfit), (LocStrFormatted) GameDifficultyConfig.title(nameof (ExtraContractsProfit), "Contracts profitability"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (ExtraContractsProfit), "Extra free goods received when trading via contracts."), new Percent[4]
      {
        0.Percent(),
        10.Percent(),
        20.Percent(),
        30.Percent()
      }, false);
      GameDifficultyConfig.RefundOption__Partial = Loc.Str(nameof (RefundOption__Partial), "Partial refund", "a difficulty option where player gets only a partial refund when deconstructing something");
      GameDifficultyConfig.RefundOption__Full = Loc.Str(nameof (RefundOption__Full), "Full refund", "a difficulty option where player gets a full (100%) refund when deconstructing something");
      GameDifficultyConfig.DeconstructionRefundInfo = (DiffSettingInfo<GameDifficultyConfig.DeconstructionRefundSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.DeconstructionRefundSetting>(nameof (DeconstructionRefund), (LocStrFormatted) GameDifficultyConfig.title(nameof (DeconstructionRefund), "Deconstruction refund"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (DeconstructionRefund), "Affects how much material is returned back when deconstructing buildings, machines and vehicles."), new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.RefundOption__Full,
        (LocStrFormatted) GameDifficultyConfig.RefundOption__Partial
      });
      GameDifficultyConfig.LoansSetting__Easy = Loc.Str(nameof (LoansSetting__Easy), "Lenient", "a difficulty option for loans, this is the easiest");
      GameDifficultyConfig.LoansSetting__Hard = Loc.Str(nameof (LoansSetting__Hard), "Firm", "a difficulty option for loans, this is the hardest (although not really severe)");
      GameDifficultyConfig.LoansDifficultyInfo = (DiffSettingInfo<LoansDifficulty>) GameDifficultyConfig.createForEnum<LoansDifficulty>(nameof (LoansDifficulty), (LocStrFormatted) GameDifficultyConfig.title(nameof (LoansDifficulty), "Loan conditions"), LocStrFormatted.Empty, new LocStrFormatted[3]
      {
        (LocStrFormatted) GameDifficultyConfig.LoansSetting__Easy,
        (LocStrFormatted) TrCore.OptionValStandard,
        (LocStrFormatted) GameDifficultyConfig.LoansSetting__Hard
      });
      GameDifficultyConfig.DiffOption__RunsOnUnity = Loc.Str(nameof (DiffOption__RunsOnUnity), "Runs on Unity", "a difficulty option where ships can run on unity if out of fuel");
      GameDifficultyConfig.DiffOption__StopsWorking = Loc.Str(nameof (DiffOption__StopsWorking), "Stops working", "a difficulty option where machines / vehicles stop when out of something (power, fuel)");
      GameDifficultyConfig.DiffOption__SlowsDown = Loc.Str(nameof (DiffOption__SlowsDown), "Slows down", "a difficulty option where machines / vehicles slow down instead of stopping when out of something (power, fuel)");
      GameDifficultyConfig.DiffOption__GraduallyStops = Loc.Str(nameof (DiffOption__GraduallyStops), "Gradually stops", "a difficulty option where machines / vehicles slow down and after a while stop instead of stopping abruptly when they run out of something (power)");
      GameDifficultyConfig.ShipsNoFuelInfo = (DiffSettingInfo<GameDifficultyConfig.ShipNoFuelSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.ShipNoFuelSetting>(nameof (ShipsNoFuel), (LocStrFormatted) GameDifficultyConfig.title(nameof (ShipsNoFuel), "Ship out of fuel"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__RunsOnUnity,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.Option_ReducesThroughput = Loc.Str(nameof (Option_ReducesThroughput), "Reduces throughput", " a difficulty option where pumps reduce their throughput when out of groundwater instead of stopping entirely");
      GameDifficultyConfig.GroundwaterPumpLowInfo = (DiffSettingInfo<GameDifficultyConfig.GroundwaterPumpLowSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.GroundwaterPumpLowSetting>(nameof (GroundwaterPumpLow), (LocStrFormatted) GameDifficultyConfig.title(nameof (GroundwaterPumpLow), "Pump out of water"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.Option_ReducesThroughput,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.ResearchCostDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (ResearchCostDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (ResearchCostDiff), "Research cost"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (ResearchCostDiff), "Adjusts the amount of time and resources required for each research task."), new Percent[3]
      {
        -25.Percent(),
        0.Percent(),
        50.Percent()
      }, true);
      GameDifficultyConfig.StarvationMode__ReducedWorkers = Loc.Str(nameof (StarvationMode__ReducedWorkers), "Reduced workforce", "");
      GameDifficultyConfig.StarvationMode__Death = Loc.Str(nameof (StarvationMode__Death), "Death by starvation", "");
      GameDifficultyConfig.StarvationInfo = (DiffSettingInfo<GameDifficultyConfig.StarvationSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.StarvationSetting>(nameof (Starvation), (LocStrFormatted) GameDifficultyConfig.title(nameof (Starvation), "Starvation effects"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.StarvationMode__ReducedWorkers,
        (LocStrFormatted) GameDifficultyConfig.StarvationMode__Death
      });
      GameDifficultyConfig.WorldMinesNoUnityInfo = (DiffSettingInfo<GameDifficultyConfig.WorldMinesNoUnitySetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.WorldMinesNoUnitySetting>(nameof (WorldMinesNoUnity), (LocStrFormatted) GameDifficultyConfig.title(nameof (WorldMinesNoUnity), "World mine out of Unity"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__SlowsDown,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.VehiclesNoFuelInfo = (DiffSettingInfo<GameDifficultyConfig.VehiclesNoFuelSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.VehiclesNoFuelSetting>(nameof (VehiclesNoFuel), (LocStrFormatted) GameDifficultyConfig.title(nameof (VehiclesNoFuel), "Vehicle out of fuel"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__SlowsDown,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.ConsumerBrokenInfo = (DiffSettingInfo<GameDifficultyConfig.ConsumerBrokenSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.ConsumerBrokenSetting>(nameof (ConsumerBroken), (LocStrFormatted) GameDifficultyConfig.title(nameof (ConsumerBroken), "Consumer out of maintenance"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__SlowsDown,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.PowerLowInfo = (DiffSettingInfo<GameDifficultyConfig.PowerLowSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.PowerLowSetting>(nameof (PowerLow), (LocStrFormatted) GameDifficultyConfig.title(nameof (PowerLow), "Consumer out of power"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__GraduallyStops,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.ComputingLowInfo = (DiffSettingInfo<GameDifficultyConfig.ComputingLowSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.ComputingLowSetting>(nameof (ComputingLow), (LocStrFormatted) GameDifficultyConfig.title(nameof (ComputingLow), "Consumer out of computing"), LocStrFormatted.Empty, new LocStrFormatted[2]
      {
        (LocStrFormatted) GameDifficultyConfig.DiffOption__GraduallyStops,
        (LocStrFormatted) GameDifficultyConfig.DiffOption__StopsWorking
      });
      GameDifficultyConfig.QuickActionsCostInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (QuickActionsCostDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (QuickActionsCostDiff), "Quick actions cost"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (QuickActionsCostDiff), "Adjusts the Unity cost for quick actions, like quick delivery"), new Percent[3]
      {
        -25.Percent(),
        0.Percent(),
        50.Percent()
      }, true);
      GameDifficultyConfig.DiseaseMortalityDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (DiseaseMortalityDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (DiseaseMortalityDiff), "Disease mortality rate"), LocStrFormatted.Empty, new Percent[3]
      {
        -100.Percent(),
        0.Percent(),
        100.Percent()
      }, true);
      GameDifficultyConfig.OreSortingInfo = (DiffSettingInfo<GameDifficultyConfig.OreSortingSetting>) GameDifficultyConfig.createForEnum<GameDifficultyConfig.OreSortingSetting>(nameof (OreSorting), (LocStrFormatted) GameDifficultyConfig.title(nameof (OreSorting), "Mixed ore sorting"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (OreSorting), "If enabled, mined mixed materials in trucks must be processed at a dedicated sorting facility"), new LocStrFormatted[2]
      {
        (LocStrFormatted) TrCore.Disabled,
        (LocStrFormatted) TrCore.Enabled
      });
      GameDifficultyConfig.SolarPowerDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (SolarPowerDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (SolarPowerDiff), "Solar power production"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (SolarPowerDiff), "Affects how much electricity is generated by solar panels"), new Percent[5]
      {
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent(),
        100.Percent()
      }, false);
      GameDifficultyConfig.PollutionDiffInfo = (DiffSettingInfo<Percent>) new PercentSettingInfo(nameof (PollutionDiff), (LocStrFormatted) GameDifficultyConfig.title(nameof (PollutionDiff), "Pollution impact"), (LocStrFormatted) GameDifficultyConfig.desc(nameof (PollutionDiff), "Affects pollution intensity."), new Percent[6]
      {
        -50.Percent(),
        -25.Percent(),
        0.Percent(),
        25.Percent(),
        50.Percent(),
        100.Percent()
      }, true);
      GameDifficultyConfig.AllOptions = ImmutableArray.Create<IDiffSettingInfo>((IDiffSettingInfo) GameDifficultyConfig.ExtraStartingMaterialInfo, (IDiffSettingInfo) GameDifficultyConfig.FuelConsumptionDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.MaintenanceDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.ResourceMiningDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.SettlementConsumptionDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.UnityProductionDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.WorldMinesReservesInfo, (IDiffSettingInfo) GameDifficultyConfig.RainYieldDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.BaseHealthDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.FarmYieldInfo, (IDiffSettingInfo) GameDifficultyConfig.TreesGrowthInfo, (IDiffSettingInfo) GameDifficultyConfig.WeatherDifficultyInfo, (IDiffSettingInfo) GameDifficultyConfig.ConstructionCostsDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.QuickRepairInfo, (IDiffSettingInfo) GameDifficultyConfig.PowerSettingInfo, (IDiffSettingInfo) GameDifficultyConfig.ExtraContractsProfitInfo, (IDiffSettingInfo) GameDifficultyConfig.DeconstructionRefundInfo, (IDiffSettingInfo) GameDifficultyConfig.LoansDifficultyInfo, (IDiffSettingInfo) GameDifficultyConfig.ShipsNoFuelInfo, (IDiffSettingInfo) GameDifficultyConfig.GroundwaterPumpLowInfo, (IDiffSettingInfo) GameDifficultyConfig.ResearchCostDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.StarvationInfo, (IDiffSettingInfo) GameDifficultyConfig.WorldMinesNoUnityInfo, (IDiffSettingInfo) GameDifficultyConfig.VehiclesNoFuelInfo, (IDiffSettingInfo) GameDifficultyConfig.ConsumerBrokenInfo, (IDiffSettingInfo) GameDifficultyConfig.PowerLowInfo, (IDiffSettingInfo) GameDifficultyConfig.ComputingLowInfo, (IDiffSettingInfo) GameDifficultyConfig.QuickActionsCostInfo, (IDiffSettingInfo) GameDifficultyConfig.DiseaseMortalityDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.OreSortingInfo, (IDiffSettingInfo) GameDifficultyConfig.SolarPowerDiffInfo, (IDiffSettingInfo) GameDifficultyConfig.PollutionDiffInfo);
      GameDifficultyConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameDifficultyConfig) obj).SerializeData(writer));
      GameDifficultyConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameDifficultyConfig) obj).DeserializeData(reader));
    }

    public enum QuickRepairSetting
    {
      Enabled,
      Disabled,
    }

    public enum WeatherDifficultySetting
    {
      Easy = -1, // 0xFFFFFFFF
      Standard = 0,
      Dry = 1,
    }

    public enum LogisticsPowerSetting
    {
      DoNotConsume = -1, // 0xFFFFFFFF
      ConsumeIfPossible = 0,
      ConsumeAlways = 1,
    }

    public enum DeconstructionRefundSetting
    {
      Full = -1, // 0xFFFFFFFF
      Partial = 0,
    }

    public enum ShipNoFuelSetting
    {
      RunOnUnity,
      StopWorking,
    }

    public enum GroundwaterPumpLowSetting
    {
      SlowDown,
      StopWorking,
    }

    public enum StarvationSetting
    {
      ReducedWorkforce,
      Death,
    }

    public enum WorldMinesNoUnitySetting
    {
      SlowDown,
      Stop,
    }

    public enum VehiclesNoFuelSetting
    {
      SlowDown,
      Stop,
    }

    public enum ConsumerBrokenSetting
    {
      SlowDown,
      Stop,
    }

    public enum PowerLowSetting
    {
      SlowDown,
      Stop,
    }

    public enum ComputingLowSetting
    {
      SlowDown,
      Stop,
    }

    public enum OreSortingSetting
    {
      Disabled,
      Enabled,
    }
  }
}
