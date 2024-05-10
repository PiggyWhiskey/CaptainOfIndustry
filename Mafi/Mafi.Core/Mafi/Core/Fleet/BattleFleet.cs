// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.BattleFleet
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Fleet
{
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("Fleet {Name}")]
  public class BattleFleet : IBattleAware
  {
    public readonly string Name;
    public readonly bool IsHuman;
    private readonly Lyst<FleetEntity> m_entities;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Whether this fleet is currently in battle. No changes to the fleet can be made during battle.
    /// </summary>
    public bool IsInBattle { get; private set; }

    public bool IsNotInBattle => !this.IsInBattle;

    public IIndexable<FleetEntity> Entities => (IIndexable<FleetEntity>) this.m_entities;

    public BattleFleet(string name, bool isHuman)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name.CheckNotNullOrEmpty();
      this.IsHuman = isHuman;
      this.m_entities = new Lyst<FleetEntity>();
    }

    public bool HasAliveEntities()
    {
      return this.m_entities.Any<FleetEntity>((Predicate<FleetEntity>) (e => e.IsNotDestroyed));
    }

    public int GetHpSum()
    {
      int hpSum = 0;
      foreach (FleetEntity entity in this.Entities)
        hpSum += entity.Hull.CurrentHp;
      return hpSum;
    }

    public void AddEntity(FleetEntity entity)
    {
      Mafi.Assert.That<BattleFleet>(entity.Fleet).IsNull<BattleFleet>("Entity is already in a fleet.");
      Mafi.Assert.That<bool>(this.IsInBattle).IsFalse("Adding fleet Entity during battle.");
      this.m_entities.Add(entity);
      ((IFleetEntityFriend) entity).SetFleet(this);
    }

    public int GetBattleScore()
    {
      return this.Entities.Sum<FleetEntity>((Func<FleetEntity, int>) (x => x.GetBattleScore()));
    }

    public AssetValue GetRepairCost()
    {
      AssetValue empty = AssetValue.Empty;
      foreach (FleetEntity entity in this.Entities)
        empty += entity.GetRepairCost();
      return empty;
    }

    public void RepairAll()
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsFalse("Repairing fleet in battle.");
      foreach (FleetEntity entity in this.Entities)
        entity.Repair();
    }

    public void GetDamageStats(
      out int dmgDealt,
      out int missedDmg,
      out int dmgTaken,
      out int armorDmgReduction)
    {
      dmgDealt = 0;
      missedDmg = 0;
      dmgTaken = 0;
      armorDmgReduction = 0;
      foreach (FleetEntity entity in this.Entities)
      {
        foreach (FleetWeapon weapon in entity.Weapons)
        {
          dmgDealt = weapon.DamageDuringCurrentBattle;
          missedDmg = weapon.MissedDmgDuringCurrentBattle;
        }
        foreach (DestructibleFleetPart destructiblePart in entity.DestructibleParts)
        {
          dmgTaken += destructiblePart.DamageTakenDuringCurrentBattle;
          armorDmgReduction += destructiblePart.ArmorDamageReductionDuringCurrentBattle;
        }
      }
    }

    public bool CanContinueBattle()
    {
      return this.m_entities.Any<FleetEntity>((Predicate<FleetEntity>) (e => e.CanContinueBattle));
    }

    void IBattleAware.InitializeBattle(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsFalse();
      this.IsInBattle = true;
      foreach (FleetEntity entity in this.m_entities)
        entity.InitializeBattle(state);
    }

    void IBattleAware.NextBattleRoundStarted(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      foreach (FleetEntity entity in this.m_entities)
        entity.NextBattleRoundStarted(state);
    }

    void IBattleAware.FinalizeBattle(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      foreach (FleetEntity entity in this.m_entities)
        entity.FinalizeBattle(state);
    }

    void IBattleAware.ResetBattle(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      this.IsInBattle = false;
      foreach (FleetEntity entity in this.m_entities)
        entity.ResetBattle(state);
    }

    public static void Serialize(BattleFleet value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BattleFleet>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BattleFleet.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsHuman);
      writer.WriteBool(this.IsInBattle);
      Lyst<FleetEntity>.Serialize(this.m_entities, writer);
      writer.WriteString(this.Name);
    }

    public static BattleFleet Deserialize(BlobReader reader)
    {
      BattleFleet battleFleet;
      if (reader.TryStartClassDeserialization<BattleFleet>(out battleFleet))
        reader.EnqueueDataDeserialization((object) battleFleet, BattleFleet.s_deserializeDataDelayedAction);
      return battleFleet;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BattleFleet>(this, "IsHuman", (object) reader.ReadBool());
      this.IsInBattle = reader.ReadBool();
      reader.SetField<BattleFleet>(this, "m_entities", (object) Lyst<FleetEntity>.Deserialize(reader));
      reader.SetField<BattleFleet>(this, "Name", (object) reader.ReadString());
    }

    static BattleFleet()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BattleFleet.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BattleFleet) obj).SerializeData(writer));
      BattleFleet.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BattleFleet) obj).DeserializeData(reader));
    }
  }
}
