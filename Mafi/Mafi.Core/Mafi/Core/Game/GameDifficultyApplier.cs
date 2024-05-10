// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameDifficultyApplier
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Game
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class GameDifficultyApplier : 
    ICommandProcessor<ChangeGameDifficultyCmd>,
    IAction<ChangeGameDifficultyCmd>
  {
    public static readonly RelGameDate DifficultyChangeTimeout;
    [NewInSaveVersion(140, null, null, typeof (GameDifficultyConfig), null)]
    public readonly GameDifficultyConfig DifficultyConfig;
    [NewInSaveVersion(140, null, null, typeof (IPropertiesDb), null)]
    private readonly IPropertiesDb m_propertiesDb;
    [NewInSaveVersion(140, null, null, typeof (ICalendar), null)]
    private readonly ICalendar m_calendar;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Event m_onDifficultyChanged;
    [NewInSaveVersion(140, null, "new()", null, null)]
    public readonly GameDifficultyChangeLog ChangeLog;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GameDate CurrentDate => this.m_calendar.CurrentDate;

    public IEvent OnDifficultyChanged => (IEvent) this.m_onDifficultyChanged;

    public GameDifficultyApplier(
      GameDifficultyConfig difficultyConfig,
      IPropertiesDb propertiesDb,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onDifficultyChanged = new Event();
      this.ChangeLog = new GameDifficultyChangeLog();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DifficultyConfig = difficultyConfig;
      this.m_propertiesDb = propertiesDb;
      this.m_calendar = calendar;
      this.applyDifficultyToProperties();
    }

    [InitAfterLoad(InitPriority.Low)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion < 140)
        this.applyDifficultyToProperties();
      if (saveVersion >= 161)
        return;
      this.applyDelta(IdsCore.PropertyIds.ResearchStepsMultiplier, this.DifficultyConfig.ResearchCostDiff);
      this.applyDelta(IdsCore.PropertyIds.MiningMultiplier, this.DifficultyConfig.ResourceMiningDiff);
    }

    public void Invoke(ChangeGameDifficultyCmd cmd)
    {
      ImmutableArray<GameDifficultyOptionChange> diff = this.DifficultyConfig.GenerateDiff(cmd.NewDifficulty);
      if (diff.IsEmpty)
      {
        cmd.SetResultError("No delta to apply!");
      }
      else
      {
        GameDate currentDate = this.CurrentDate;
        foreach (GameDifficultyOptionChange optionChange in diff)
        {
          GameDate? lastChangeFor = this.ChangeLog.GetLastChangeFor(optionChange);
          if (lastChangeFor.HasValue && currentDate - lastChangeFor.Value < GameDifficultyApplier.DifficultyChangeTimeout)
          {
            Log.Error("Cannot change property " + optionChange.ValueMemberName + " as it was changed recently!");
            cmd.SetResultError("");
            return;
          }
        }
        this.ChangeLog.AddChangeSet(new GameDifficultyChangeLog.ChangeSet(currentDate, diff));
        this.DifficultyConfig.OverrideWithValuesFrom(cmd.NewDifficulty);
        this.applyDifficultyToProperties();
        this.m_onDifficultyChanged.Invoke();
        cmd.SetResultSuccess();
      }
    }

    public void Test__UpdateDifficulty()
    {
      this.applyDifficultyToProperties();
      this.m_onDifficultyChanged.Invoke();
    }

    private void applyDelta(PropertyId<Percent> id, Percent value)
    {
      this.m_propertiesDb.GetProperty<Percent>(id).AddOrSetModifier("GameDifficulty", value, (Option<string>) Property<Percent>.BASE_GROUP);
    }

    private void applyBoolean(PropertyId<bool> id, bool value, string group = null)
    {
      this.m_propertiesDb.GetProperty<bool>(id).AddOrSetModifier("GameDifficulty", value, PropertyModifiers.NO_GROUP);
    }

    private void applyDifficultyToProperties()
    {
      GameDifficultyConfig difficultyConfig = this.DifficultyConfig;
      this.applyDelta(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier, difficultyConfig.MaintenanceDiff);
      this.applyDelta(IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier, difficultyConfig.FuelConsumptionDiff);
      this.applyDelta(IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier, difficultyConfig.FuelConsumptionDiff);
      this.applyDelta(IdsCore.PropertyIds.RainYieldMultiplier, difficultyConfig.RainYieldDiff);
      this.applyDelta(IdsCore.PropertyIds.BaseHealthMultiplier, difficultyConfig.BaseHealthDiff);
      this.applyBoolean(IdsCore.PropertyIds.LogisticsCanWorkOnLowPower, difficultyConfig.PowerSetting != GameDifficultyConfig.LogisticsPowerSetting.ConsumeAlways);
      this.applyBoolean(IdsCore.PropertyIds.LogisticsIgnorePower, difficultyConfig.PowerSetting == GameDifficultyConfig.LogisticsPowerSetting.DoNotConsume);
      this.applyDelta(IdsCore.PropertyIds.TreesGrowthSpeed, difficultyConfig.TreesGrowthDiff);
      this.applyDelta(IdsCore.PropertyIds.UnityProductionMultiplier, difficultyConfig.UnityProductionDiff);
      this.applyDelta(IdsCore.PropertyIds.SettlementConsumptionMultiplier, difficultyConfig.SettlementConsumptionDiff);
      this.applyDelta(IdsCore.PropertyIds.FarmYieldMultiplier, difficultyConfig.FarmsYieldDiff);
      this.applyDelta(IdsCore.PropertyIds.ConstructionCostsMultiplier, difficultyConfig.ConstructionCostsDiff);
      this.applyBoolean(IdsCore.PropertyIds.UnlimitedWorldMines, difficultyConfig.WorldMinesUnlimited);
      this.applyDelta(IdsCore.PropertyIds.WorldMinesReserveMultiplier, difficultyConfig.WorldMinesUnlimited ? Percent.Hundred : difficultyConfig.WorldMinesReservesDiff);
      this.applyDelta(IdsCore.PropertyIds.DeconstructionRefundMultiplier, difficultyConfig.DeconstructionRefund == GameDifficultyConfig.DeconstructionRefundSetting.Full ? Percent.Zero : -20.Percent());
      this.applyBoolean(IdsCore.PropertyIds.ShipsCanUseUnityIfOutOfFuel, difficultyConfig.ShipsNoFuel == GameDifficultyConfig.ShipNoFuelSetting.RunOnUnity);
      this.applyBoolean(IdsCore.PropertyIds.CanWithholdWorkersOnStarvation, difficultyConfig.Starvation == GameDifficultyConfig.StarvationSetting.ReducedWorkforce);
      this.applyBoolean(IdsCore.PropertyIds.VehicleSlowDownOnLowFuel, difficultyConfig.VehiclesNoFuel == GameDifficultyConfig.VehiclesNoFuelSetting.SlowDown);
      bool flag = difficultyConfig.GroundwaterPumpLow == GameDifficultyConfig.GroundwaterPumpLowSetting.SlowDown;
      this.applyDelta(IdsCore.PropertyIds.GroundWaterPumpSpeedWhenDepleted, flag ? 40.Percent() : Percent.Zero);
      this.applyDelta(IdsCore.PropertyIds.GroundWaterReplenishWhenLow, flag ? 7.Percent() : 0.Percent());
      this.applyBoolean(IdsCore.PropertyIds.SlowDownIfBroken, difficultyConfig.ConsumerBroken == GameDifficultyConfig.ConsumerBrokenSetting.SlowDown);
      this.applyBoolean(IdsCore.PropertyIds.WorldMinesCanRunWithoutUnity, difficultyConfig.WorldMinesNoUnity == GameDifficultyConfig.WorldMinesNoUnitySetting.SlowDown);
      this.applyDelta(IdsCore.PropertyIds.MachineSpeedOnLowPower, difficultyConfig.PowerLow == GameDifficultyConfig.PowerLowSetting.SlowDown ? 60.Percent() : Percent.Zero);
      this.applyDelta(IdsCore.PropertyIds.MachineSpeedOnLowComputing, difficultyConfig.ComputingLow == GameDifficultyConfig.ComputingLowSetting.SlowDown ? 60.Percent() : Percent.Zero);
      this.applyDelta(IdsCore.PropertyIds.QuickActionsUnityCostMultiplier, difficultyConfig.QuickActionsCostDiff);
      this.applyDelta(IdsCore.PropertyIds.DiseaseMortalityMultiplier, difficultyConfig.DiseaseMortalityDiff);
      this.applyBoolean(IdsCore.PropertyIds.OreSortingEnabled, difficultyConfig.OreSorting == GameDifficultyConfig.OreSortingSetting.Enabled);
      this.applyDelta(IdsCore.PropertyIds.SolarPowerMultiplier, difficultyConfig.SolarPowerDiff);
      this.applyDelta(IdsCore.PropertyIds.AirPollutionMultiplier, difficultyConfig.PollutionDiff);
      this.applyDelta(IdsCore.PropertyIds.LandfillPollutionMultiplier, difficultyConfig.PollutionDiff);
      this.applyDelta(IdsCore.PropertyIds.WaterPollutionMultiplier, difficultyConfig.PollutionDiff);
      this.applyDelta(IdsCore.PropertyIds.ResearchStepsMultiplier, difficultyConfig.ResearchCostDiff);
      this.applyDelta(IdsCore.PropertyIds.MiningMultiplier, difficultyConfig.ResourceMiningDiff);
      this.applyDelta(IdsCore.PropertyIds.ContractsProfitMultiplier, difficultyConfig.ExtraContractsProfit);
    }

    public static void Serialize(GameDifficultyApplier value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameDifficultyApplier>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameDifficultyApplier.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      GameDifficultyChangeLog.Serialize(this.ChangeLog, writer);
      GameDifficultyConfig.Serialize(this.DifficultyConfig, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      Event.Serialize(this.m_onDifficultyChanged, writer);
      writer.WriteGeneric<IPropertiesDb>(this.m_propertiesDb);
    }

    public static GameDifficultyApplier Deserialize(BlobReader reader)
    {
      GameDifficultyApplier difficultyApplier;
      if (reader.TryStartClassDeserialization<GameDifficultyApplier>(out difficultyApplier))
        reader.EnqueueDataDeserialization((object) difficultyApplier, GameDifficultyApplier.s_deserializeDataDelayedAction);
      return difficultyApplier;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GameDifficultyApplier>(this, "ChangeLog", reader.LoadedSaveVersion >= 140 ? (object) GameDifficultyChangeLog.Deserialize(reader) : (object) new GameDifficultyChangeLog());
      reader.SetField<GameDifficultyApplier>(this, "DifficultyConfig", reader.LoadedSaveVersion >= 140 ? (object) GameDifficultyConfig.Deserialize(reader) : (object) (GameDifficultyConfig) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<GameDifficultyApplier>(this, "DifficultyConfig", typeof (GameDifficultyConfig), true);
      reader.SetField<GameDifficultyApplier>(this, "m_calendar", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<ICalendar>() : (object) (ICalendar) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<GameDifficultyApplier>(this, "m_calendar", typeof (ICalendar), true);
      reader.SetField<GameDifficultyApplier>(this, "m_onDifficultyChanged", reader.LoadedSaveVersion >= 140 ? (object) Event.Deserialize(reader) : (object) new Event());
      reader.SetField<GameDifficultyApplier>(this, "m_propertiesDb", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IPropertiesDb>() : (object) (IPropertiesDb) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<GameDifficultyApplier>(this, "m_propertiesDb", typeof (IPropertiesDb), true);
      reader.RegisterInitAfterLoad<GameDifficultyApplier>(this, "initSelf", InitPriority.Low);
    }

    static GameDifficultyApplier()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameDifficultyApplier.DifficultyChangeTimeout = RelGameDate.FromYears(10);
      GameDifficultyApplier.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameDifficultyApplier) obj).SerializeData(writer));
      GameDifficultyApplier.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameDifficultyApplier) obj).DeserializeData(reader));
    }
  }
}
