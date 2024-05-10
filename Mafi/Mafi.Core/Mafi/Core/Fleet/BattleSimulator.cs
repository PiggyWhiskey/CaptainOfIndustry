// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.BattleSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>
  /// Battle simulator resolves battles using turn-based simulation.
  /// </summary>
  /// <remarks>
  /// To evaluate a battle, use <see cref="M:Mafi.Core.Fleet.BattleSimulator.StartBattle(Mafi.Core.Fleet.BattleFleet,Mafi.Core.Fleet.BattleFleet)" /> method.
  /// 
  /// State of a battle is held in the <see cref="T:Mafi.Core.Fleet.BattleState" /> class so that the simulator can evaluate the battle in
  /// time-sliced manner over multiple game steps. Only one battle can be happening at a time and is accessible via
  /// <see cref="P:Mafi.Core.Fleet.BattleSimulator.OngoingBattle" />.
  /// 
  /// Battle is evaluated in rounds. Each round, every entity from both fleets gets to perform an action using <see cref="M:Mafi.Core.Fleet.FleetEntity.StepBattle(Mafi.Core.Fleet.BattleFleet,Mafi.Core.Fleet.BattleState)" />. Order of entities each round is fixed and based on <see cref="P:Mafi.Core.Fleet.FleetEntity.BattlePriority" />. Note that defender has a small priority advantage <see cref="P:Mafi.Core.Fleet.IBattleSimConfig.DefenderExtraBattlePriority" />.
  /// 
  /// Battle ends when at least one fleet has no entities with <see cref="P:Mafi.Core.Fleet.FleetEntity.CanContinueBattle" />. There is a
  /// safety counter <see cref="P:Mafi.Core.Fleet.IBattleSimConfig.MaxBattleRounds" /> to prevent potential infinite battles. In this case
  /// or in case of draw (which currently should not happen), the defender is considered a winner.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class BattleSimulator : IBattleSimulator
  {
    public readonly IBattleSimConfig Config;
    private readonly IRandom m_randomTemplate;
    private readonly Lyst<PlayersBattleResult> m_allPlayersResults;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<BattleState> OngoingBattle { get; private set; }

    public IIndexable<PlayersBattleResult> AllBattleResults
    {
      get => (IIndexable<PlayersBattleResult>) this.m_allPlayersResults;
    }

    public BattleSimulator(
      ISimLoopEvents simLoopEvents,
      IBattleSimConfig config,
      RandomProvider randomProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_allPlayersResults = new Lyst<PlayersBattleResult>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
      this.m_randomTemplate = randomProvider.GetSimRandomFor((object) this);
      simLoopEvents.Update.Add<BattleSimulator>(this, new Action(this.simUpdate));
    }

    public Option<IBattleState> StartBattle(BattleFleet attacker, BattleFleet defender)
    {
      if (this.OngoingBattle.HasValue)
      {
        Log.Error("A battle is already ongoing.");
        return Option<IBattleState>.None;
      }
      if (attacker == defender)
      {
        Log.Error("Battle with itself.");
        return Option<IBattleState>.None;
      }
      if (attacker.IsHuman == defender.IsHuman)
      {
        Log.Error("Battle between same factions: " + (attacker.IsHuman ? "" : "not-") + "human");
        return Option<IBattleState>.None;
      }
      if (attacker.IsInBattle)
      {
        Log.Error("Attacker is already in battle.");
        return Option<IBattleState>.None;
      }
      if (defender.IsInBattle)
      {
        Log.Error("Defender is already in battle.");
        return Option<IBattleState>.None;
      }
      this.m_randomTemplate.Jump();
      BattleState state = new BattleState(this, attacker, defender, this.m_randomTemplate.Clone());
      Assert.That<Option<BattleResult>>(state.Result).IsNone<BattleResult>();
      this.initializeBattle(state);
      if (state.Attacker.HasAliveEntities())
      {
        if (!state.Defender.HasAliveEntities())
          this.finalizeBattle(state, state.Attacker);
      }
      else if (state.Defender.HasAliveEntities())
        this.finalizeBattle(state, state.Defender);
      else
        this.finalizeBattle(state, state.Defender);
      return (Option<IBattleState>) (IBattleState) state;
    }

    private void simUpdate()
    {
      if (this.OngoingBattle.IsNone)
        return;
      BattleState state = this.OngoingBattle.Value;
      Option<BattleFleet> option = this.stepBattle(state);
      if (!option.HasValue)
        return;
      this.finalizeBattle(state, option.Value);
      this.OngoingBattle = Option<BattleState>.None;
    }

    /// <summary>Simulates one battle round for all ongoing battles.</summary>
    private Option<BattleFleet> stepBattle(BattleState state)
    {
      Assert.That<Option<BattleResult>>(state.Result).IsNone<BattleResult>("Battle already finished.");
      ((IBattleAware) state.Defender).NextBattleRoundStarted(state);
      ((IBattleAware) state.Attacker).NextBattleRoundStarted(state);
      bool flag1 = state.Attacker.CanContinueBattle();
      bool flag2 = state.Defender.CanContinueBattle();
      if (!flag1)
      {
        state.LogBattleMessage(!flag2 ? "Battle resulted in a draw. Defending fleet won." : "Defending fleet won.");
        return (Option<BattleFleet>) state.Defender;
      }
      if (!flag2)
      {
        state.LogBattleMessage("Attacking fleet won.");
        return (Option<BattleFleet>) state.Attacker;
      }
      Assert.That<int>(state.BattleSortedEntities.Count).IsEqualTo(state.Attacker.Entities.Count + state.Defender.Entities.Count, "Invalid `BattleSortedEntities` array.");
      foreach (FleetEntity battleSortedEntity in state.BattleSortedEntities)
      {
        if (battleSortedEntity.CanContinueBattle)
        {
          BattleFleet enemyFleet = state.GetEnemyFleet(battleSortedEntity.Fleet);
          battleSortedEntity.StepBattle(enemyFleet, state);
        }
      }
      ++state.BattleRound;
      if (state.BattleRound <= this.Config.MaxBattleRounds)
        return Option<BattleFleet>.None;
      Log.Error("Battle is too long, terminating. Possible infinite battle?");
      state.LogBattleMessage("Battle is too long. Defender won.");
      return (Option<BattleFleet>) state.Defender;
    }

    private BattleFleet simulateFortressFireBattle(BattleState state)
    {
      Assert.That<Option<BattleResult>>(state.Result).IsNone<BattleResult>("Battle already finished.");
      Assert.That<int>(state.BattleSortedEntities.Count).IsEqualTo(state.Attacker.Entities.Count);
      foreach (FleetEntity battleSortedEntity in state.BattleSortedEntities)
      {
        if (battleSortedEntity.CanContinueBattle)
          battleSortedEntity.FireAllWeaponsAt(state.Defender, state);
      }
      return state.Defender.HasAliveEntities() ? state.Defender : state.Attacker;
    }

    private void initializeBattle(BattleState state)
    {
      Assert.That<Option<BattleState>>(this.OngoingBattle).IsNone<BattleState>();
      state.LogBattleMessage("Battle has started.");
      state.InitializeBattle();
      this.OngoingBattle = (Option<BattleState>) state;
    }

    private void finalizeBattle(BattleState state, BattleFleet winner)
    {
      Assert.That<Option<BattleState>>(this.OngoingBattle).IsEqualTo<BattleState>(state);
      state.GetEnemyFleet(winner);
      state.LogBattleMessage("Battle has ended.");
      state.FinalizeBattle(winner);
      Assert.That<Option<BattleResult>>(state.Result).HasValue<BattleResult>();
      if (state.Result.Value.PlayerResult.HasValue)
        this.m_allPlayersResults.Add(state.Result.Value.PlayerResult.Value);
      state.ResetBattle();
      this.OngoingBattle = Option<BattleState>.None;
    }

    public static void Serialize(BattleSimulator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BattleSimulator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BattleSimulator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IBattleSimConfig>(this.Config);
      Lyst<PlayersBattleResult>.Serialize(this.m_allPlayersResults, writer);
      writer.WriteGeneric<IRandom>(this.m_randomTemplate);
      Option<BattleState>.Serialize(this.OngoingBattle, writer);
    }

    public static BattleSimulator Deserialize(BlobReader reader)
    {
      BattleSimulator battleSimulator;
      if (reader.TryStartClassDeserialization<BattleSimulator>(out battleSimulator))
        reader.EnqueueDataDeserialization((object) battleSimulator, BattleSimulator.s_deserializeDataDelayedAction);
      return battleSimulator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BattleSimulator>(this, "Config", (object) reader.ReadGenericAs<IBattleSimConfig>());
      reader.SetField<BattleSimulator>(this, "m_allPlayersResults", (object) Lyst<PlayersBattleResult>.Deserialize(reader));
      reader.SetField<BattleSimulator>(this, "m_randomTemplate", (object) reader.ReadGenericAs<IRandom>());
      this.OngoingBattle = Option<BattleState>.Deserialize(reader);
    }

    static BattleSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BattleSimulator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BattleSimulator) obj).SerializeData(writer));
      BattleSimulator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BattleSimulator) obj).DeserializeData(reader));
    }
  }
}
