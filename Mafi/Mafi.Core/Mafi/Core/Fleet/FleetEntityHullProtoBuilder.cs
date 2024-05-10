// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityHullProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Fleet
{
  public sealed class FleetEntityHullProtoBuilder : IProtoBuilder
  {
    private static readonly Proto.Str GUNS_SLOT_GROUP_FRONT;
    private static readonly Proto.Str GUNS_SLOT_GROUP_REAR;
    private static readonly Proto.Str ENGINE_SLOT_GROUP;
    private static readonly Proto.Str ARMOR_SLOT_GROUP;
    private static readonly Proto.Str FUEL_TANK_SLOT_GROUP;
    private static readonly Proto.Str BRIDGE_SLOT_GROUP;

    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public FleetEntityHullProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public FleetEntityHullProtoBuilder.State Start(string name, FleetEntityHullProto.ID id)
    {
      return new FleetEntityHullProtoBuilder.State(this, id, name);
    }

    static FleetEntityHullProtoBuilder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntityHullProtoBuilder.GUNS_SLOT_GROUP_FRONT = Proto.CreateStr(new Proto.ID("ShipSlotGroupGunsFront"), "Guns (front)", "", "title of ship upgrade slot");
      FleetEntityHullProtoBuilder.GUNS_SLOT_GROUP_REAR = Proto.CreateStr(new Proto.ID("ShipSlotGroupGunsRear"), "Guns (rear)", "", "title of ship upgrade slot");
      FleetEntityHullProtoBuilder.ENGINE_SLOT_GROUP = Proto.CreateStr(new Proto.ID("ShipSlotGroupEngine"), "Engine", "", "title of ship upgrade slot");
      FleetEntityHullProtoBuilder.ARMOR_SLOT_GROUP = Proto.CreateStr(new Proto.ID("ShipSlotGroupArmor"), "Armor", "", "title of ship upgrade slot");
      FleetEntityHullProtoBuilder.FUEL_TANK_SLOT_GROUP = Proto.CreateStr(new Proto.ID("ShipSlotGroupFuelTank"), "Fuel tank", "", "title of ship upgrade slot");
      FleetEntityHullProtoBuilder.BRIDGE_SLOT_GROUP = Proto.CreateStr(new Proto.ID("ShipSlotGroupBridge"), "Bridge", "", "title of ship upgrade slot");
    }

    public class State : ProtoBuilderState<FleetEntityHullProtoBuilder.State>
    {
      private readonly FleetEntityHullProto.ID m_id;
      private readonly string m_name;
      private AssetValue? m_hullValue;
      private int? m_hullHp;
      private Option<FleetEntityHullProto.Gfx> m_hullGraphics;
      private int? m_battlePriority;
      private int? m_hitChanceWeight;
      private int m_extraRoundsToEscape;
      private readonly Lyst<FleetEntitySlotProto> m_slotGroups;

      public State(
        FleetEntityHullProtoBuilder builder,
        FleetEntityHullProto.ID id,
        string name,
        string translationComment = "HIDE")
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_slotGroups = new Lyst<FleetEntitySlotProto>();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (Proto.ID) id, name, translationComment);
        this.m_id = id;
        this.m_name = name;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetHullHp(int hp)
      {
        this.m_hullHp = new int?(hp);
        return this;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetHullValue(AssetValue value)
      {
        this.m_hullValue = new AssetValue?(value);
        return this;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetHullGraphics(FleetEntityHullProto.Gfx graphics)
      {
        this.m_hullGraphics = (Option<FleetEntityHullProto.Gfx>) graphics;
        return this;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetBattlePriority(int battlePriority)
      {
        this.m_battlePriority = new int?(battlePriority);
        return this;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetHitChanceWeight(int hitChanceWeight)
      {
        this.m_hitChanceWeight = new int?(hitChanceWeight);
        return this;
      }

      [MustUseReturnValue]
      public FleetEntityHullProtoBuilder.State SetExtraRoundsToEscape(int extraRoundsToEscape)
      {
        this.m_extraRoundsToEscape = extraRoundsToEscape;
        return this;
      }

      public FleetEntityHullProtoBuilder.State AddSlotGroup(
        Proto.ID groupId,
        FleetEntitySlotProto.SlotType type,
        Proto.Str strings,
        FleetEntityPartProto.ID[] eligibleParts,
        string goToShowIfEmpty = null)
      {
        ImmutableArray<FleetEntityPartProto> immutableArray = ((IEnumerable<FleetEntityPartProto.ID>) eligibleParts).Select<FleetEntityPartProto.ID, FleetEntityPartProto>((Func<FleetEntityPartProto.ID, FleetEntityPartProto>) (x => this.Builder.ProtosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) x))).ToImmutableArray<FleetEntityPartProto>();
        this.m_slotGroups.Add(this.Builder.ProtosDb.Add<FleetEntitySlotProto>(new FleetEntitySlotProto(groupId, strings, type, immutableArray, new FleetEntitySlotProto.Gfx((Option<string>) goToShowIfEmpty))));
        return this;
      }

      public FleetEntityHullProtoBuilder.State AddWeaponsGroup(
        int capacity,
        bool isFront,
        FleetEntityPartProto.ID[] eligibleParts,
        string goToShowIfEmpty = null)
      {
        for (int index = 0; index < capacity; ++index)
          this.AddSlotGroup(new Proto.ID(string.Format("{0}GunsGroup_{1}", (object) this.m_id.Value, (object) this.m_slotGroups.Count)), FleetEntitySlotProto.SlotType.Weapons, isFront ? FleetEntityHullProtoBuilder.GUNS_SLOT_GROUP_FRONT : FleetEntityHullProtoBuilder.GUNS_SLOT_GROUP_REAR, eligibleParts, goToShowIfEmpty);
        return this;
      }

      public FleetEntityHullProtoBuilder.State AddEngineUpgradeGroup(
        FleetEntityPartProto.ID[] eligibleParts)
      {
        return this.AddSlotGroup(new Proto.ID(this.m_id.Value + "EngineUpgradeGroup"), FleetEntitySlotProto.SlotType.Engines, FleetEntityHullProtoBuilder.ENGINE_SLOT_GROUP, eligibleParts);
      }

      public FleetEntityHullProtoBuilder.State AddHullUpgradesGroup(
        int capacity,
        FleetEntityPartProto.ID[] eligibleParts)
      {
        for (int index = 0; index < capacity; ++index)
        {
          ImmutableArray<FleetEntityPartProto> immutableArray = ((IEnumerable<FleetEntityPartProto.ID>) eligibleParts).Select<FleetEntityPartProto.ID, FleetEntityPartProto>((Func<FleetEntityPartProto.ID, FleetEntityPartProto>) (x => this.Builder.ProtosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) x))).ToImmutableArray<FleetEntityPartProto>();
          this.m_slotGroups.Add(this.Builder.ProtosDb.Add<FleetEntitySlotProto>(new FleetEntitySlotProto(new Proto.ID(string.Format("{0}HullUpgradesGroup_{1}", (object) this.m_id.Value, (object) index)), FleetEntityHullProtoBuilder.ARMOR_SLOT_GROUP, FleetEntitySlotProto.SlotType.HullUpgrades, immutableArray, FleetEntitySlotProto.Gfx.Empty)));
        }
        return this;
      }

      public FleetEntityHullProtoBuilder.State AddRadarUpgradeGroup(
        FleetEntityPartProto.ID[] eligibleParts)
      {
        return this.AddSlotGroup(new Proto.ID(this.m_id.Value + "RadarUpgradeGroup"), FleetEntitySlotProto.SlotType.Radars, FleetEntityHullProtoBuilder.BRIDGE_SLOT_GROUP, eligibleParts);
      }

      public FleetEntityHullProtoBuilder.State AddFuelTankUpgradeGroup(
        FleetEntityPartProto.ID[] eligibleParts)
      {
        ImmutableArray<FleetEntityPartProto> immutableArray = ((IEnumerable<FleetEntityPartProto.ID>) eligibleParts).Select<FleetEntityPartProto.ID, FleetEntityPartProto>((Func<FleetEntityPartProto.ID, FleetEntityPartProto>) (x => this.Builder.ProtosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) x))).ToImmutableArray<FleetEntityPartProto>();
        this.m_slotGroups.Add(this.Builder.ProtosDb.Add<FleetEntitySlotProto>(new FleetEntitySlotProto(new Proto.ID(this.m_id.Value + "FuelTankUpgradeGroup"), FleetEntityHullProtoBuilder.FUEL_TANK_SLOT_GROUP, FleetEntitySlotProto.SlotType.FuelTankUpgrades, immutableArray, FleetEntitySlotProto.Gfx.Empty)));
        return this;
      }

      public FleetEntityHullProto BuildAndAdd()
      {
        return this.AddToDb<FleetEntityHullProto>(new FleetEntityHullProto(this.m_id, new Proto.Str(this.Name, this.DescShort), this.ValueOrThrow<AssetValue>(this.m_hullValue, "Hull value"), this.ValueOrThrow<int>(this.m_hullHp, "Hull HP"), this.ValueOrThrow<int>(this.m_battlePriority, "BattlePriority"), this.ValueOrThrow<int>(this.m_hitChanceWeight, "HitChanceWeight"), this.m_extraRoundsToEscape, this.m_slotGroups.ToImmutableArray(), this.ValueOrThrow<FleetEntityHullProto.Gfx>(this.m_hullGraphics, "Hull graphics")));
      }
    }
  }
}
