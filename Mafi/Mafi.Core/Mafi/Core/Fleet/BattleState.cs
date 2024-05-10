// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.BattleState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Fleet
{
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("Battle '{Attacker.Name}' vs. '{Defender.Name}'")]
  public class BattleState : IBattleState
  {
    private readonly BattleSimulator m_battleSimulator;
    public readonly IRandom Random;
    private readonly Lyst<string> m_battleLog;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public BattleFleet Attacker { get; private set; }

    public BattleFleet Defender { get; private set; }

    public Option<BattleResult> Result { get; private set; }

    public int BattleRound { get; set; }

    /// <summary>
    /// Entities sorted by their <see cref="P:Mafi.Core.Fleet.FleetEntity.BattlePriority" />. Defender entities has bonus <see cref="P:Mafi.Core.Fleet.IBattleSimConfig.DefenderExtraBattlePriority" />.
    /// </summary>
    [DoNotSave(0, null)]
    public IIndexable<FleetEntity> BattleSortedEntities { get; private set; }

    public IIndexable<string> BattleLog => (IIndexable<string>) this.m_battleLog;

    public IBattleSimConfig BattleSimConfig => this.m_battleSimulator.Config;

    public BattleState(
      BattleSimulator battleSimulator,
      BattleFleet attacker,
      BattleFleet defender,
      IRandom random)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBattleSortedEntities\u003Ek__BackingField = Indexable<FleetEntity>.Empty;
      this.m_battleLog = new Lyst<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Mafi.Assert.That<BattleFleet>(attacker).IsNotEqualTo<BattleFleet>(defender);
      this.m_battleSimulator = battleSimulator.CheckNotNull<BattleSimulator>();
      this.Random = random.CheckNotNull<IRandom>();
      this.Attacker = attacker.CheckNotNull<BattleFleet>();
      this.Defender = defender.CheckNotNull<BattleFleet>();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void onSelfLoaded() => this.setBattleSortedEntities();

    private void setBattleSortedEntities()
    {
      this.BattleSortedEntities = (IIndexable<FleetEntity>) this.Attacker.Entities.AsEnumerable().Select<FleetEntity, KeyValuePair<int, FleetEntity>>((Func<FleetEntity, KeyValuePair<int, FleetEntity>>) (e => Make.Kvp<int, FleetEntity>(e.BattlePriority.GetValue(), e))).Concat<KeyValuePair<int, FleetEntity>>(this.Defender.Entities.AsEnumerable().Select<FleetEntity, KeyValuePair<int, FleetEntity>>((Func<FleetEntity, KeyValuePair<int, FleetEntity>>) (e => Make.Kvp<int, FleetEntity>(e.BattlePriority.GetValue() + this.m_battleSimulator.Config.DefenderExtraBattlePriority, e)))).OrderByDescending<KeyValuePair<int, FleetEntity>, int>((Func<KeyValuePair<int, FleetEntity>, int>) (x => x.Key)).Select<KeyValuePair<int, FleetEntity>, FleetEntity>((Func<KeyValuePair<int, FleetEntity>, FleetEntity>) (x => x.Value)).ToLyst<FleetEntity>();
    }

    public BattleFleet GetEnemyFleet(BattleFleet bf)
    {
      if (bf == this.Attacker)
        return this.Defender;
      Mafi.Assert.That<BattleFleet>(bf).IsEqualTo<BattleFleet>(this.Defender);
      return this.Attacker;
    }

    public void InitializeBattle()
    {
      Mafi.Assert.That<Option<BattleResult>>(this.Result).IsNone<BattleResult>();
      Mafi.Assert.That<bool>(this.Attacker.IsInBattle).IsFalse();
      Mafi.Assert.That<bool>(this.Defender.IsInBattle).IsFalse();
      this.setBattleSortedEntities();
      ((IBattleAware) this.Defender).InitializeBattle(this);
      ((IBattleAware) this.Attacker).InitializeBattle(this);
      int num = (this.BattleSortedEntities.AsEnumerable().Max<FleetEntity>((Func<FleetEntity, int>) (e => e.Weapons.AsEnumerable().MaxOrDefault<FleetWeapon>((Func<FleetWeapon, int>) (w => w.Range)))) + this.BattleSimConfig.StartingExtraFleetDistance) / 2;
      foreach (FleetEntity entity in this.Defender.Entities)
        entity.Position = (Fix32) -num;
      foreach (FleetEntity entity in this.Attacker.Entities)
        entity.Position = (Fix32) num;
      Mafi.Assert.That<bool>(this.Attacker.IsInBattle).IsTrue();
      Mafi.Assert.That<bool>(this.Defender.IsInBattle).IsTrue();
    }

    public void LogBattleMessage(string message) => this.m_battleLog.Add(message);

    public void FinalizeBattle(BattleFleet winner)
    {
      Mafi.Assert.That<Option<BattleResult>>(this.Result).IsNone<BattleResult>();
      ((IBattleAware) this.Defender).FinalizeBattle(this);
      ((IBattleAware) this.Attacker).FinalizeBattle(this);
      this.Result = (Option<BattleResult>) this.createBattleResult(winner);
    }

    public void ResetBattle()
    {
      Mafi.Assert.That<Option<BattleResult>>(this.Result).HasValue<BattleResult>();
      ((IBattleAware) this.Defender).ResetBattle(this);
      ((IBattleAware) this.Attacker).ResetBattle(this);
      this.BattleSortedEntities = Indexable<FleetEntity>.Empty;
    }

    private BattleResult createBattleResult(BattleFleet winner)
    {
      FleetBattleResultStats winnerStats = BattleState.collectFleetBattleStats(winner);
      BattleFleet enemyFleet = this.GetEnemyFleet(winner);
      FleetBattleResultStats loserStats = BattleState.collectFleetBattleStats(enemyFleet);
      Option<BattleFleet> option = !winner.IsHuman ? (!enemyFleet.IsHuman ? Option<BattleFleet>.None : (Option<BattleFleet>) enemyFleet) : (Option<BattleFleet>) winner;
      Option<PlayersBattleResult> playerResult = Option<PlayersBattleResult>.None;
      if (option.HasValue)
        playerResult = (Option<PlayersBattleResult>) new PlayersBattleResult(winner.IsHuman ? winnerStats : loserStats, winner.IsHuman ? loserStats : winnerStats, winner == option.Value, option.Value == this.Attacker, this.BattleRound);
      return new BattleResult(winner, enemyFleet, winnerStats, loserStats, this.m_battleLog.ToImmutableArray(), playerResult);
    }

    private static FleetBattleResultStats collectFleetBattleStats(BattleFleet fleet)
    {
      int dmgDealt;
      int missedDmg;
      int dmgTaken;
      int armorDmgReduction;
      fleet.GetDamageStats(out dmgDealt, out missedDmg, out dmgTaken, out armorDmgReduction);
      return new FleetBattleResultStats(fleet.Name, dmgDealt, missedDmg, dmgTaken, armorDmgReduction);
    }

    public static void Serialize(BattleState value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BattleState>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BattleState.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      BattleFleet.Serialize(this.Attacker, writer);
      writer.WriteInt(this.BattleRound);
      BattleFleet.Serialize(this.Defender, writer);
      Lyst<string>.Serialize(this.m_battleLog, writer);
      BattleSimulator.Serialize(this.m_battleSimulator, writer);
      writer.WriteGeneric<IRandom>(this.Random);
      Option<BattleResult>.Serialize(this.Result, writer);
    }

    public static BattleState Deserialize(BlobReader reader)
    {
      BattleState battleState;
      if (reader.TryStartClassDeserialization<BattleState>(out battleState))
        reader.EnqueueDataDeserialization((object) battleState, BattleState.s_deserializeDataDelayedAction);
      return battleState;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Attacker = BattleFleet.Deserialize(reader);
      this.BattleRound = reader.ReadInt();
      this.Defender = BattleFleet.Deserialize(reader);
      reader.SetField<BattleState>(this, "m_battleLog", (object) Lyst<string>.Deserialize(reader));
      reader.SetField<BattleState>(this, "m_battleSimulator", (object) BattleSimulator.Deserialize(reader));
      reader.SetField<BattleState>(this, "Random", (object) reader.ReadGenericAs<IRandom>());
      this.Result = Option<BattleResult>.Deserialize(reader);
      reader.RegisterInitAfterLoad<BattleState>(this, "onSelfLoaded", InitPriority.Normal);
    }

    static BattleState()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BattleState.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BattleState) obj).SerializeData(writer));
      BattleState.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BattleState) obj).DeserializeData(reader));
    }
  }
}
