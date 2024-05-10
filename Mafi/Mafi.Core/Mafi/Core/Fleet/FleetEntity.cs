// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>
  /// An instance of fleet entity that has state.
  /// 
  /// Fleet entity is made of parts that give weapons or upgrades.
  /// </summary>
  [DebuggerDisplay("{Name}, fleet: {Fleet.Name}")]
  [GenerateSerializer(false, null, 0)]
  public class FleetEntity : IBattleAware, IFleetEntityFriend
  {
    public static readonly Percent RepairCostForValue;
    public static readonly Percent RepairCostForValueDestroyedPart;
    public static Fix32 DistancePerStepInBattle;
    public readonly UpgradableInt CrewNeeded;
    /// <summary>World map distance per step.</summary>
    public Fix32 DistancePerStep;
    /// <summary>World map distance per 1 unit of fuel.</summary>
    public Fix32 DistancePerFuel;
    private readonly Event<Quantity> m_onFuelCapacityChange;
    private readonly Lyst<FleetEntitySlot> m_slots;
    private readonly Lyst<FleetWeapon> m_weapons;
    public readonly UpgradableInt Armor;
    private readonly Lyst<IBattleAware> m_battleAwareParts;
    private readonly Lyst<DestructibleFleetPart> m_destructibleParts;
    [DoNotSave(0, null)]
    private Lyst<KeyValuePair<int, DestructibleFleetPart>> m_destructiblePartsWeighted;
    [DoNotSave(0, null)]
    private int m_destructiblePartsWeightedSum;
    public Option<FleetEntity> PreviouslyAttackedEntity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public LocStr Name => this.Hull.Proto.Strings.Name;

    public UpgradableInt BattlePriority => this.Hull.BattlePriority;

    public UpgradableInt HitChanceWeight => this.Hull.HitChanceWeight;

    public UpgradableInt ExtraRoundsToEscape => this.Hull.ExtraRoundsToEscape;

    public Quantity FuelTankCapacity { get; private set; }

    public int MinCrewNeeded => this.CrewNeeded.GetValue();

    public FleetHull Hull { get; private set; }

    public IEvent<Quantity> OnFuelCapacityChange => (IEvent<Quantity>) this.m_onFuelCapacityChange;

    public IIndexable<FleetEntitySlot> Slots => (IIndexable<FleetEntitySlot>) this.m_slots;

    public IIndexable<FleetWeapon> Weapons => (IIndexable<FleetWeapon>) this.m_weapons;

    public IIndexable<DestructibleFleetPart> DestructibleParts
    {
      get => (IIndexable<DestructibleFleetPart>) this.m_destructibleParts;
    }

    public BattleFleet Fleet { get; private set; }

    public bool IsInBattle { get; private set; }

    public bool IsDestroyed
    {
      get
      {
        if (this.Fleet.IsHuman)
          return false;
        return this.Hull.IsDestroyed || this.HasNoUsableWeapon;
      }
    }

    public bool IsNotDestroyed => !this.IsDestroyed;

    public bool HasNoUsableWeapon
    {
      get => this.m_weapons.All<FleetWeapon>((Func<FleetWeapon, bool>) (x => x.IsDestroyed));
    }

    public bool CanEscape => this.Fleet.IsHuman;

    public bool IsEscaping => this.RoundsToEscape > 0;

    public int RoundsToEscape { get; private set; }

    public bool HasEscaped { get; private set; }

    public bool HasNotEscaped => !this.HasEscaped;

    public bool IsFacingLeft => !this.Fleet.IsHuman;

    public bool CanContinueBattle => this.IsNotDestroyed && this.HasNotEscaped;

    public Fix32 Position { get; set; }

    public FleetEntity(
      FleetEntityHullProto hullProto,
      ImmutableArray<FleetWeaponProto> weaponsToAdd,
      ImmutableArray<FleetEntityPartProto> armorsToAdd,
      FleetEnginePartProto engineToAdd = null,
      FleetBridgePartProto bridgeToAdd = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(hullProto, engineToAdd, bridgeToAdd);
      foreach (FleetWeaponProto newPart in weaponsToAdd)
      {
        FleetEntitySlot group = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == FleetEntitySlotProto.SlotType.Weapons && x.ExistingPart.IsNone));
        if (group == null)
        {
          Mafi.Log.Error(string.Format("No weapons slot defined on '{0}'", (object) hullProto.Id));
          return;
        }
        this.setPart(group, (Option<FleetEntityPartProto>) (FleetEntityPartProto) newPart);
      }
      foreach (FleetEntityPartProto newPart in armorsToAdd)
      {
        FleetEntitySlot group = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == FleetEntitySlotProto.SlotType.HullUpgrades && x.ExistingPart.IsNone));
        if (group == null)
        {
          Mafi.Log.Error(string.Format("No armor slot defined on '{0}'", (object) hullProto.Id));
          break;
        }
        this.setPart(group, (Option<FleetEntityPartProto>) newPart);
      }
    }

    public FleetEntity(
      FleetEntityHullProto hullProto,
      FleetEnginePartProto engineToAdd,
      FleetBridgePartProto bridgeToAdd)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.CrewNeeded = new UpgradableInt(0);
      this.m_onFuelCapacityChange = new Event<Quantity>();
      this.m_slots = new Lyst<FleetEntitySlot>();
      this.m_weapons = new Lyst<FleetWeapon>();
      this.Armor = new UpgradableInt(0);
      this.m_battleAwareParts = new Lyst<IBattleAware>();
      this.m_destructibleParts = new Lyst<DestructibleFleetPart>();
      this.m_destructiblePartsWeighted = new Lyst<KeyValuePair<int, DestructibleFleetPart>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.setHull(hullProto);
      if ((Proto) engineToAdd != (Proto) null)
      {
        FleetEntitySlot group = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == FleetEntitySlotProto.SlotType.Engines));
        if (group == null)
        {
          Mafi.Log.Error(string.Format("No engine slot defined on '{0}'", (object) hullProto.Id));
          return;
        }
        this.setPart(group, (Option<FleetEntityPartProto>) (FleetEntityPartProto) engineToAdd);
      }
      if (!((Proto) bridgeToAdd != (Proto) null))
        return;
      FleetEntitySlot group1 = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == FleetEntitySlotProto.SlotType.Radars));
      if (group1 == null)
        Mafi.Log.Error(string.Format("No bridge slot defined on '{0}'", (object) hullProto.Id));
      else
        this.setPart(group1, (Option<FleetEntityPartProto>) (FleetEntityPartProto) bridgeToAdd);
    }

    public void AddFuelCapacity(Quantity diff)
    {
      Mafi.Assert.That<Quantity>(this.FuelTankCapacity + diff).IsNotNegative();
      this.FuelTankCapacity += diff;
      this.m_onFuelCapacityChange.Invoke(this.FuelTankCapacity);
    }

    public void PerformModfications(FleetEntityModificationRequest modRequest, ProtosDb protosDb)
    {
      foreach (SlotModification slotModification in modRequest.SlotsData)
        this.replacePart(slotModification.SlotId, slotModification.Part);
      if (!modRequest.HullId.HasValue)
        return;
      FleetEntityHullProto.ID? hullId = modRequest.HullId;
      FleetEntityHullProto.ID id = this.Hull.Proto.Id;
      if ((hullId.HasValue ? (hullId.GetValueOrDefault() != id ? 1 : 0) : 1) == 0)
        return;
      this.setHull(protosDb.GetOrThrow<FleetEntityHullProto>((Proto.ID) modRequest.HullId.Value));
    }

    public Quantity GetFuelCostFromDistance(Fix32 totalDist)
    {
      return FleetEntity.GetFuelCostFromDistance(totalDist, this.DistancePerFuel);
    }

    public static Quantity GetFuelCostFromDistance(Fix32 totalDist, Fix32 distancePerFuel)
    {
      return new Quantity((totalDist / distancePerFuel).ToIntCeiled());
    }

    public int GetBattleScore()
    {
      FleetEntityStats stats = new FleetEntityStats();
      this.Hull.Proto.ApplyToStats(stats);
      foreach (FleetEntitySlot slot in this.m_slots)
      {
        if (slot.ExistingPart.HasValue)
          slot.ExistingPart.Value.ApplyToStats(stats);
      }
      return stats.GetBattleScore();
    }

    private void setHull(FleetEntityHullProto hullProto)
    {
      Lyst<FleetEntitySlot> lyst = new Lyst<FleetEntitySlot>((IEnumerable<FleetEntitySlot>) this.m_slots);
      this.Hull = new FleetHull(hullProto);
      this.m_weapons.Clear();
      this.m_battleAwareParts.Clear();
      this.m_slots.Clear();
      foreach (FleetEntitySlotProto slotGroup in hullProto.SlotGroups)
        this.m_slots.Add(new FleetEntitySlot(slotGroup));
      foreach (FleetEntitySlot fleetEntitySlot in lyst)
      {
        FleetEntitySlot oldSlotGroup = fleetEntitySlot;
        FleetEntitySlot group = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == oldSlotGroup.Proto.TypeOfSlot));
        if (group == null)
          Mafi.Log.Error(string.Format("Failed to find new corresponding group for '{0}'", (object) oldSlotGroup.Proto.TypeOfSlot));
        else if (oldSlotGroup.ExistingPart.HasValue)
          this.setPart(group, oldSlotGroup.ExistingPart);
      }
      this.m_destructibleParts.Add((DestructibleFleetPart) this.Hull);
      this.m_battleAwareParts.Add((IBattleAware) this.Hull);
      this.updateWeights();
      this.m_onFuelCapacityChange.Invoke(this.FuelTankCapacity);
    }

    private void setPart(FleetEntitySlot group, Option<FleetEntityPartProto> newPart)
    {
      if (group.ExistingPart == newPart)
        return;
      if (group.ExistingPart.HasValue)
        group.ExistingPart.Value.RemoveFrom(this);
      if (newPart.HasValue)
        newPart.Value.ApplyTo(this);
      group.ExistingPart = newPart;
    }

    private void replacePart(Proto.ID slotId, FleetEntityPartProto.ID? partId)
    {
      FleetEntitySlot group = this.m_slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.Id == slotId));
      if (group == null)
      {
        Mafi.Log.Error(string.Format("Slot group with id '{0}' not found!", (object) slotId));
      }
      else
      {
        Option<FleetEntityPartProto> existingPart = group.ExistingPart;
        if (existingPart.IsNone && !partId.HasValue)
          return;
        if (existingPart.HasValue)
        {
          FleetEntityPartProto.ID id = existingPart.Value.Id;
          FleetEntityPartProto.ID? nullable = partId;
          if ((nullable.HasValue ? (id == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            return;
        }
        if (!partId.HasValue)
        {
          this.setPart(group, (Option<FleetEntityPartProto>) Option.None);
        }
        else
        {
          FleetEntityPartProto newPart = group.Proto.EligibleItems.FirstOrDefault((Func<FleetEntityPartProto, bool>) (x =>
          {
            FleetEntityPartProto.ID id = x.Id;
            FleetEntityPartProto.ID? nullable = partId;
            return nullable.HasValue && id == nullable.GetValueOrDefault();
          }));
          if ((Proto) newPart == (Proto) null)
            Mafi.Log.Error(string.Format("Received part '{0}' that is not eligible for slot '{1}'!", (object) partId, (object) slotId));
          else
            this.setPart(group, (Option<FleetEntityPartProto>) newPart);
        }
      }
    }

    private void updateWeights()
    {
      this.m_destructiblePartsWeighted.Clear();
      this.m_destructiblePartsWeightedSum = 0;
      foreach (DestructibleFleetPart destructiblePart in this.m_destructibleParts)
      {
        this.m_destructiblePartsWeighted.Add(Make.Kvp<int, DestructibleFleetPart>(destructiblePart.HitWeight.GetValue(), destructiblePart));
        this.m_destructiblePartsWeightedSum += destructiblePart.HitWeight.GetValue();
      }
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad()
    {
      this.m_destructiblePartsWeighted = new Lyst<KeyValuePair<int, DestructibleFleetPart>>();
      this.updateWeights();
    }

    public int GetAvgDamagePer10Rounds()
    {
      return this.m_weapons.Sum<FleetWeapon>((Func<FleetWeapon, int>) (w => w.AvgDamagePer10Rounds));
    }

    public void AddWeapon(FleetWeaponProto weaponProto)
    {
      FleetWeapon fleetWeapon = new FleetWeapon(weaponProto, this);
      this.m_weapons.Add(fleetWeapon);
      this.m_destructibleParts.Add((DestructibleFleetPart) fleetWeapon);
      this.m_battleAwareParts.Add((IBattleAware) fleetWeapon);
      this.updateWeights();
    }

    public void RemoveWeapon(FleetWeaponProto removedProto)
    {
      FleetWeapon fleetWeapon = this.m_weapons.FirstOrDefault<FleetWeapon>((Predicate<FleetWeapon>) (x => (Proto) x.Proto == (Proto) removedProto));
      if (fleetWeapon == null)
      {
        Mafi.Log.Error("Failed to remove the weapon!");
      }
      else
      {
        this.m_weapons.TryRemoveReplaceLast(fleetWeapon);
        this.m_destructibleParts.TryRemoveReplaceLast((DestructibleFleetPart) fleetWeapon);
        this.m_battleAwareParts.TryRemoveReplaceLast((IBattleAware) fleetWeapon);
        this.updateWeights();
      }
    }

    void IFleetEntityFriend.SetFleet(BattleFleet fleet)
    {
      Mafi.Assert.That<BattleFleet>(this.Fleet).IsNull<BattleFleet>();
      this.Fleet = fleet;
    }

    public void InitializeBattle(BattleState state)
    {
      Mafi.Assert.That<BattleFleet>(this.Fleet).IsNotNull<BattleFleet>("No fleet!");
      Mafi.Assert.That<bool>(this.IsInBattle).IsFalse();
      Mafi.Assert.That<Option<FleetEntity>>(this.PreviouslyAttackedEntity).IsNone<FleetEntity>();
      this.IsInBattle = true;
      foreach (IBattleAware battleAwarePart in this.m_battleAwareParts)
        battleAwarePart.InitializeBattle(state);
    }

    public void NextBattleRoundStarted(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      foreach (IBattleAware battleAwarePart in this.m_battleAwareParts)
        battleAwarePart.NextBattleRoundStarted(state);
    }

    public void StepBattle(BattleFleet enemyFleet, BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      Mafi.Assert.That<bool>(this.CanContinueBattle).IsTrue();
      Mafi.Assert.That<bool>(this.Fleet.IsInBattle).IsTrue();
      Mafi.Assert.That<BattleFleet>(this.Fleet).IsNotEqualTo<BattleFleet>(enemyFleet);
      foreach (FleetWeapon weapon in this.m_weapons)
        weapon.FiredLastSim = false;
      if (this.IsEscaping)
      {
        if (this.IsFacingLeft)
          this.Position += 2 * FleetEntity.DistancePerStepInBattle;
        else
          this.Position -= 2 * FleetEntity.DistancePerStepInBattle;
        --this.RoundsToEscape;
        if (this.RoundsToEscape > 0)
          return;
        this.HasEscaped = true;
        state.LogBattleMessage(string.Format("Ship `{0}` has escaped with {1} HP left.", (object) this.Name, (object) this.Hull.CurrentHp));
      }
      else
      {
        bool hasNoUsableWeapon = this.m_weapons.All<FleetWeapon>((Func<FleetWeapon, bool>) (x => x.IsDestroyed));
        if (this.CanEscape && this.testEscape(enemyFleet, state, hasNoUsableWeapon))
        {
          this.startEscaping(state);
        }
        else
        {
          if (hasNoUsableWeapon || !enemyFleet.Entities.Where<FleetEntity>((Func<FleetEntity, bool>) (e => e.IsNotDestroyed)).Any<FleetEntity>())
            return;
          foreach (FleetWeapon weapon in this.m_weapons)
          {
            if (weapon.IsReadyToFire)
            {
              Option<FleetEntity> option = this.chooseEnemy(enemyFleet, weapon, state);
              if (option.HasValue)
              {
                weapon.FiredLastSim = true;
                weapon.FireAt(option.Value, state);
              }
              this.PreviouslyAttackedEntity = option;
            }
          }
          if (this.Weapons.Where<FleetWeapon>((Func<FleetWeapon, bool>) (x => x.IsNotDestroyed)).All<FleetWeapon>((Func<FleetWeapon, bool>) (w => enemyFleet.Entities.Any<FleetEntity>((Predicate<FleetEntity>) (e => e.IsNotDestroyed && w.IsInRange(e))))))
            return;
          if (this.IsFacingLeft)
            this.Position -= FleetEntity.DistancePerStepInBattle;
          else
            this.Position += FleetEntity.DistancePerStepInBattle;
        }
      }
    }

    private bool testEscape(BattleFleet enemyFleet, BattleState state, bool hasNoUsableWeapon)
    {
      if (hasNoUsableWeapon)
      {
        state.LogBattleMessage(string.Format("Ship {0} started escaping because it has no usable weapons.", (object) this.Name));
        return true;
      }
      if (!(this.Hull.HpPercent < state.BattleSimConfig.ShipEscapeHpThreshold) || this.Hull.CurrentHp >= enemyFleet.GetHpSum() * 3)
        return false;
      state.LogBattleMessage(string.Format("Ship {0} started escaping due to its low HP.", (object) this.Name));
      return true;
    }

    public void FireAllWeaponsAt(BattleFleet enemyFleet, BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      Mafi.Assert.That<bool>(this.CanContinueBattle).IsTrue();
      Mafi.Assert.That<bool>(this.Fleet.IsInBattle).IsTrue();
      Mafi.Assert.That<BattleFleet>(this.Fleet).IsNotEqualTo<BattleFleet>(enemyFleet);
      Mafi.Assert.That<bool>(this.IsEscaping).IsFalse();
      foreach (FleetWeapon weapon in this.m_weapons)
      {
        if (weapon.IsReadyToFire)
        {
          Option<FleetEntity> option = this.chooseEnemy(enemyFleet, weapon, state);
          if (option.HasValue)
            weapon.FireAt(option.Value, state);
        }
      }
    }

    private Option<FleetEntity> chooseEnemy(
      BattleFleet enemyFleet,
      FleetWeapon weapon,
      BattleState state)
    {
      if (this.PreviouslyAttackedEntity.HasValue)
      {
        FleetEntity entity = this.PreviouslyAttackedEntity.Value;
        if (!entity.CanContinueBattle || !weapon.IsInRange(entity))
          this.PreviouslyAttackedEntity = Option<FleetEntity>.None;
        else if (state.Random.TestProbability(state.BattleSimConfig.ChanceForSameEntityRepeatedFire))
          return (Option<FleetEntity>) entity;
      }
      FleetEntity fleetEntity = enemyFleet.Entities.Where<FleetEntity>((Func<FleetEntity, bool>) (e => e.IsNotDestroyed && weapon.IsInRange(e))).Select<FleetEntity, KeyValuePair<int, FleetEntity>>((Func<FleetEntity, KeyValuePair<int, FleetEntity>>) (e => Make.Kvp<int, FleetEntity>(e.HitChanceWeight.GetValue(), e))).SampleRandomWeightedOrDefault<FleetEntity>(state.Random);
      return fleetEntity == null ? Option<FleetEntity>.None : (Option<FleetEntity>) fleetEntity;
    }

    private void startEscaping(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsEscaping).IsFalse();
      Mafi.Assert.That<bool>(this.CanEscape).IsTrue();
      this.RoundsToEscape = state.BattleSimConfig.BaseRoundsToEscape + this.ExtraRoundsToEscape.GetValue();
    }

    public int? TakeImpactFrom(int damage, FleetWeapon weapon, BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsNotDestroyed).IsTrue();
      if (this.IsEscaping && state.Random.TestProbability(state.BattleSimConfig.ExtraMissChanceWhenEscaping))
        return new int?();
      int num1 = 0;
      DestructibleFleetPart destructibleFleetPart = this.m_destructiblePartsWeighted.SampleRandomWeighted<DestructibleFleetPart>(state.Random, this.m_destructiblePartsWeightedSum);
      if (destructibleFleetPart != this.Hull && destructibleFleetPart.IsNotDestroyed)
        num1 += destructibleFleetPart.TakeDamage(damage, this.Armor.GetValue(), state.BattleSimConfig);
      if (num1 > 0 && this.Hull.CurrentHp >= this.Hull.MaxHp.GetValue())
        this.Hull.SetHp(this.Hull.MaxHp.GetValue().ScaledByRounded(99.Percent()));
      int num2 = num1 + this.Hull.TakeDamage(damage, this.Armor.GetValue(), state.BattleSimConfig);
      if (this.IsDestroyed)
        state.LogBattleMessage(string.Format("Entity `{0}` was destroyed by `{1}`.", (object) this.Name, (object) weapon.OwningEntity.Name));
      return new int?(num2);
    }

    public void FinalizeBattle(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsTrue();
      this.IsInBattle = false;
      foreach (IBattleAware battleAwarePart in this.m_battleAwareParts)
        battleAwarePart.FinalizeBattle(state);
    }

    public void ResetBattle(BattleState state)
    {
      Mafi.Assert.That<bool>(this.IsInBattle).IsFalse();
      this.RoundsToEscape = 0;
      this.HasEscaped = false;
      this.PreviouslyAttackedEntity = Option<FleetEntity>.None;
      foreach (IBattleAware battleAwarePart in this.m_battleAwareParts)
        battleAwarePart.ResetBattle(state);
    }

    public AssetValue GetRepairCost()
    {
      if (this.IsDestroyed)
        return AssetValue.Empty;
      Percent percent = Percent.Hundred - Percent.FromRatio(this.Hull.CurrentHp, this.Hull.MaxHp.GetValue());
      AssetValue assetValue = this.Hull.Proto.Value;
      foreach (FleetEntitySlot slot in this.m_slots)
      {
        if (slot.ExistingPart.HasValue)
        {
          FleetEntityPartProto fleetEntityPartProto = slot.ExistingPart.Value;
          if (!(fleetEntityPartProto is DestructibleFleetPartProto))
            assetValue += fleetEntityPartProto.Value;
        }
      }
      AssetValue empty = AssetValue.Empty;
      foreach (DestructibleFleetPart destructiblePart in this.m_destructibleParts)
      {
        if (destructiblePart != this.Hull)
          empty += destructiblePart.GetRepairCost();
      }
      return empty + assetValue.ScaledByCeiled(percent).ScaledByCeiled(FleetEntity.RepairCostForValue);
    }

    public void Repair()
    {
      this.Hull.SetHp(this.Hull.MaxHp.GetValue());
      foreach (DestructibleFleetPart destructiblePart in this.m_destructibleParts)
      {
        destructiblePart.Repair();
        Mafi.Assert.That<Percent>(destructiblePart.HpPercent).IsEqualTo(Percent.Hundred);
      }
    }

    public static void Serialize(FleetEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetEntity.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      UpgradableInt.Serialize(this.Armor, writer);
      UpgradableInt.Serialize(this.CrewNeeded, writer);
      Fix32.Serialize(this.DistancePerFuel, writer);
      Fix32.Serialize(this.DistancePerStep, writer);
      BattleFleet.Serialize(this.Fleet, writer);
      Quantity.Serialize(this.FuelTankCapacity, writer);
      writer.WriteBool(this.HasEscaped);
      FleetHull.Serialize(this.Hull, writer);
      writer.WriteBool(this.IsInBattle);
      Lyst<IBattleAware>.Serialize(this.m_battleAwareParts, writer);
      Lyst<DestructibleFleetPart>.Serialize(this.m_destructibleParts, writer);
      Event<Quantity>.Serialize(this.m_onFuelCapacityChange, writer);
      Lyst<FleetEntitySlot>.Serialize(this.m_slots, writer);
      Lyst<FleetWeapon>.Serialize(this.m_weapons, writer);
      Fix32.Serialize(this.Position, writer);
      Option<FleetEntity>.Serialize(this.PreviouslyAttackedEntity, writer);
      writer.WriteInt(this.RoundsToEscape);
    }

    public static FleetEntity Deserialize(BlobReader reader)
    {
      FleetEntity fleetEntity;
      if (reader.TryStartClassDeserialization<FleetEntity>(out fleetEntity))
        reader.EnqueueDataDeserialization((object) fleetEntity, FleetEntity.s_deserializeDataDelayedAction);
      return fleetEntity;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FleetEntity>(this, "Armor", (object) UpgradableInt.Deserialize(reader));
      reader.SetField<FleetEntity>(this, "CrewNeeded", (object) UpgradableInt.Deserialize(reader));
      this.DistancePerFuel = Fix32.Deserialize(reader);
      this.DistancePerStep = Fix32.Deserialize(reader);
      this.Fleet = BattleFleet.Deserialize(reader);
      this.FuelTankCapacity = Quantity.Deserialize(reader);
      this.HasEscaped = reader.ReadBool();
      this.Hull = FleetHull.Deserialize(reader);
      this.IsInBattle = reader.ReadBool();
      reader.SetField<FleetEntity>(this, "m_battleAwareParts", (object) Lyst<IBattleAware>.Deserialize(reader));
      reader.SetField<FleetEntity>(this, "m_destructibleParts", (object) Lyst<DestructibleFleetPart>.Deserialize(reader));
      reader.SetField<FleetEntity>(this, "m_onFuelCapacityChange", (object) Event<Quantity>.Deserialize(reader));
      reader.SetField<FleetEntity>(this, "m_slots", (object) Lyst<FleetEntitySlot>.Deserialize(reader));
      reader.SetField<FleetEntity>(this, "m_weapons", (object) Lyst<FleetWeapon>.Deserialize(reader));
      this.Position = Fix32.Deserialize(reader);
      this.PreviouslyAttackedEntity = Option<FleetEntity>.Deserialize(reader);
      this.RoundsToEscape = reader.ReadInt();
      reader.RegisterInitAfterLoad<FleetEntity>(this, "initAfterLoad", InitPriority.Normal);
    }

    static FleetEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntity.RepairCostForValue = 35.Percent();
      FleetEntity.RepairCostForValueDestroyedPart = 60.Percent();
      FleetEntity.DistancePerStepInBattle = 0.05.ToFix32();
      FleetEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FleetEntity) obj).SerializeData(writer));
      FleetEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FleetEntity) obj).DeserializeData(reader));
    }
  }
}
