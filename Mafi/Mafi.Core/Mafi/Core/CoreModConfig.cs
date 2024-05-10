// Decompiled with JetBrains decompiler
// Type: Mafi.Core.CoreModConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Fleet;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class CoreModConfig : 
    IModConfig,
    IConfig,
    IUnlockedProtosConfig,
    IBattleSimConfig,
    IGameRunnerConfig,
    IDeterminismValidatorConfig,
    IPopulationConfig,
    IInstaBuildConfig,
    ILogisticsConfig,
    ITracingConfig,
    IGodModeConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<string> LoadedWorldMapName { get; set; }

    /// <summary>
    /// Whether terrain physics should be disabled. Terrain with no physics simulation will not fall under steep
    /// angles.
    /// </summary>
    public bool DisableTerrainPhysics { get; set; }

    /// <summary>
    /// Whether terrain surface simulation should be disabled. Terrain with no physics surface simulation will not
    /// process disrupted tiles events.
    /// </summary>
    public bool DisableTerrainSurfaceSimulation { get; set; }

    /// <summary>
    /// Whether vehicle path-finding should be disabled. With no path-finding all vehicles will just travel on
    /// straight lines ignoring any obstacles.
    /// </summary>
    public bool DisablePathFinding { get; set; }

    public bool DisableMultiThreadTerrainGeneration { get; set; }

    /// <summary>
    /// Whether auto-unlock of boundary cells should be disabled. This is handy for tests.
    /// </summary>
    public bool DisableBoundaryCellAutoUnlock { get; set; }

    /// <summary>Whether to generate resources on the terrain.</summary>
    public bool DisableResourcesGeneration { get; set; }

    public Option<string> LoadedIslandMapName { get; set; }

    /// <summary>
    /// Whether to disable generation of terrain chunks for locked cells. This dramatically speeds up start time and
    /// is handy when debugging.
    /// </summary>
    public bool DisableLockedCellsTerrainGeneration { get; set; }

    /// <summary>
    /// Whether all locked protos should be unlocked from the beginning of the game.
    /// </summary>
    public bool ShouldUnlockAllProtosOnInit { get; set; }

    public bool LogCommandsAsCSharp { get; set; }

    public bool IsInstaBuildEnabled { get; set; }

    [DoNotSave(0, null)]
    public bool IsGodModeEnabled { get; set; }

    public bool DisableSimulationBackgroundThread { get; set; }

    public bool DeterminismValidationEnabled { get; set; }

    public Duration DeterminismValidationFrequencySteps { get; set; }

    public bool DeterminismDisableCommandsForwarding { get; set; }

    public int DefenderExtraBattlePriority { get; set; }

    public int MaxBattleRounds { get; set; }

    public int StartingExtraFleetDistance { get; set; }

    public int PossibleEscapeDistance { get; set; }

    public Percent ShipEscapeHpThreshold { get; set; }

    public int BaseRoundsToEscape { get; set; }

    public Percent ChanceForSameEntityRepeatedFire { get; set; }

    public Percent ChanceForDisabledEnemyFire { get; set; }

    public Percent ExtraMissChanceWhenEscaping { get; set; }

    public Percent MaxArmorReduction { get; set; }

    public Percent RecoverableHpMultiplier { get; set; }

    public Percent HullDamageMultWhenPartIsHit { get; set; }

    public int StartingPopulation { get; set; }

    public bool SaveTraceOnSimOvertime { get; set; }

    public Duration SaveTraceOnSimOvertimeMinDelay { get; set; }

    public Duration SaveTimingLogPeriod { get; set; }

    public int InitialVehiclesCap { get; set; }

    public bool AlwaysSunny { get; set; }

    [InitAfterLoad(InitPriority.High)]
    private void initAfterLoad(int saveVersion)
    {
      if (saveVersion >= 109)
        return;
      this.InitialVehiclesCap = 60;
    }

    public static void Serialize(CoreModConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CoreModConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CoreModConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AlwaysSunny);
      writer.WriteInt(this.BaseRoundsToEscape);
      Percent.Serialize(this.ChanceForDisabledEnemyFire, writer);
      Percent.Serialize(this.ChanceForSameEntityRepeatedFire, writer);
      writer.WriteInt(this.DefenderExtraBattlePriority);
      writer.WriteBool(this.DeterminismDisableCommandsForwarding);
      writer.WriteBool(this.DeterminismValidationEnabled);
      Duration.Serialize(this.DeterminismValidationFrequencySteps, writer);
      writer.WriteBool(this.DisableBoundaryCellAutoUnlock);
      writer.WriteBool(this.DisableLockedCellsTerrainGeneration);
      writer.WriteBool(this.DisableMultiThreadTerrainGeneration);
      writer.WriteBool(this.DisablePathFinding);
      writer.WriteBool(this.DisableResourcesGeneration);
      writer.WriteBool(this.DisableSimulationBackgroundThread);
      writer.WriteBool(this.DisableTerrainPhysics);
      writer.WriteBool(this.DisableTerrainSurfaceSimulation);
      Percent.Serialize(this.ExtraMissChanceWhenEscaping, writer);
      Percent.Serialize(this.HullDamageMultWhenPartIsHit, writer);
      writer.WriteInt(this.InitialVehiclesCap);
      writer.WriteBool(this.IsInstaBuildEnabled);
      Option<string>.Serialize(this.LoadedIslandMapName, writer);
      Option<string>.Serialize(this.LoadedWorldMapName, writer);
      writer.WriteBool(this.LogCommandsAsCSharp);
      Percent.Serialize(this.MaxArmorReduction, writer);
      writer.WriteInt(this.MaxBattleRounds);
      writer.WriteInt(this.PossibleEscapeDistance);
      Percent.Serialize(this.RecoverableHpMultiplier, writer);
      Duration.Serialize(this.SaveTimingLogPeriod, writer);
      writer.WriteBool(this.SaveTraceOnSimOvertime);
      Duration.Serialize(this.SaveTraceOnSimOvertimeMinDelay, writer);
      Percent.Serialize(this.ShipEscapeHpThreshold, writer);
      writer.WriteBool(this.ShouldUnlockAllProtosOnInit);
      writer.WriteInt(this.StartingExtraFleetDistance);
      writer.WriteInt(this.StartingPopulation);
    }

    public static CoreModConfig Deserialize(BlobReader reader)
    {
      CoreModConfig coreModConfig;
      if (reader.TryStartClassDeserialization<CoreModConfig>(out coreModConfig))
        reader.EnqueueDataDeserialization((object) coreModConfig, CoreModConfig.s_deserializeDataDelayedAction);
      return coreModConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.AlwaysSunny = reader.ReadBool();
      this.BaseRoundsToEscape = reader.ReadInt();
      this.ChanceForDisabledEnemyFire = Percent.Deserialize(reader);
      this.ChanceForSameEntityRepeatedFire = Percent.Deserialize(reader);
      this.DefenderExtraBattlePriority = reader.ReadInt();
      this.DeterminismDisableCommandsForwarding = reader.ReadBool();
      this.DeterminismValidationEnabled = reader.ReadBool();
      this.DeterminismValidationFrequencySteps = Duration.Deserialize(reader);
      this.DisableBoundaryCellAutoUnlock = reader.ReadBool();
      this.DisableLockedCellsTerrainGeneration = reader.ReadBool();
      this.DisableMultiThreadTerrainGeneration = reader.ReadBool();
      this.DisablePathFinding = reader.ReadBool();
      this.DisableResourcesGeneration = reader.ReadBool();
      this.DisableSimulationBackgroundThread = reader.ReadBool();
      this.DisableTerrainPhysics = reader.ReadBool();
      this.DisableTerrainSurfaceSimulation = reader.ReadBool();
      this.ExtraMissChanceWhenEscaping = Percent.Deserialize(reader);
      this.HullDamageMultWhenPartIsHit = Percent.Deserialize(reader);
      this.InitialVehiclesCap = reader.ReadInt();
      this.IsInstaBuildEnabled = reader.ReadBool();
      this.LoadedIslandMapName = Option<string>.Deserialize(reader);
      this.LoadedWorldMapName = Option<string>.Deserialize(reader);
      this.LogCommandsAsCSharp = reader.ReadBool();
      this.MaxArmorReduction = Percent.Deserialize(reader);
      this.MaxBattleRounds = reader.ReadInt();
      this.PossibleEscapeDistance = reader.ReadInt();
      this.RecoverableHpMultiplier = Percent.Deserialize(reader);
      this.SaveTimingLogPeriod = Duration.Deserialize(reader);
      this.SaveTraceOnSimOvertime = reader.ReadBool();
      this.SaveTraceOnSimOvertimeMinDelay = Duration.Deserialize(reader);
      this.ShipEscapeHpThreshold = Percent.Deserialize(reader);
      this.ShouldUnlockAllProtosOnInit = reader.ReadBool();
      this.StartingExtraFleetDistance = reader.ReadInt();
      this.StartingPopulation = reader.ReadInt();
      reader.RegisterInitAfterLoad<CoreModConfig>(this, "initAfterLoad", InitPriority.High);
    }

    public CoreModConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CDeterminismValidationFrequencySteps\u003Ek__BackingField = 10.Seconds();
      // ISSUE: reference to a compiler-generated field
      this.\u003CDefenderExtraBattlePriority\u003Ek__BackingField = 10;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxBattleRounds\u003Ek__BackingField = 1000;
      // ISSUE: reference to a compiler-generated field
      this.\u003CStartingExtraFleetDistance\u003Ek__BackingField = 4;
      // ISSUE: reference to a compiler-generated field
      this.\u003CPossibleEscapeDistance\u003Ek__BackingField = 15;
      // ISSUE: reference to a compiler-generated field
      this.\u003CShipEscapeHpThreshold\u003Ek__BackingField = 25.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBaseRoundsToEscape\u003Ek__BackingField = 25;
      // ISSUE: reference to a compiler-generated field
      this.\u003CChanceForSameEntityRepeatedFire\u003Ek__BackingField = 60.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CChanceForDisabledEnemyFire\u003Ek__BackingField = 40.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CExtraMissChanceWhenEscaping\u003Ek__BackingField = 40.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxArmorReduction\u003Ek__BackingField = 80.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CRecoverableHpMultiplier\u003Ek__BackingField = 40.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CHullDamageMultWhenPartIsHit\u003Ek__BackingField = 50.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CStartingPopulation\u003Ek__BackingField = 90;
      // ISSUE: reference to a compiler-generated field
      this.\u003CSaveTraceOnSimOvertimeMinDelay\u003Ek__BackingField = Duration.FromSec(30);
      // ISSUE: reference to a compiler-generated field
      this.\u003CInitialVehiclesCap\u003Ek__BackingField = 60;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CoreModConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CoreModConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CoreModConfig) obj).SerializeData(writer));
      CoreModConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CoreModConfig) obj).DeserializeData(reader));
    }
  }
}
