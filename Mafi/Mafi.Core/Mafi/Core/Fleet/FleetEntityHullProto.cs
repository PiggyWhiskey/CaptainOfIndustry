// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityHullProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>
  /// Hull represents the main part of the entity. If hull is destroyed, the whole entity is destroyed.
  /// </summary>
  public class FleetEntityHullProto : DestructibleFleetPartProto
  {
    public readonly FleetEntityHullProto.Gfx Graphics;
    public readonly ImmutableArray<FleetEntitySlotProto> SlotGroups;
    public static readonly Duration RepairDurationPerProduct;
    /// <summary>
    /// Battle priority determines order of ships turns in a battle round.
    /// </summary>
    public readonly int BattlePriority;
    /// <summary>
    /// Weight that determines chance to be picked as a target when ship is switching targets.
    /// </summary>
    public readonly int HitChanceWeight;
    /// <summary>Extra rounds to escape for this ship.</summary>
    public readonly int ExtraRoundsToEscape;

    public FleetEntityHullProto(
      FleetEntityHullProto.ID id,
      Proto.Str strings,
      AssetValue value,
      int maxHp,
      int battlePriority,
      int hitChanceWeight,
      int extraRoundsToEscape,
      ImmutableArray<FleetEntitySlotProto> slotGroups,
      FleetEntityHullProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((FleetEntityPartProto.ID) id, strings, value, maxHp, maxHp, 0, (FleetEntityGfx) graphics);
      this.SlotGroups = slotGroups;
      this.Graphics = graphics.CheckNotNull<FleetEntityHullProto.Gfx>();
      this.BattlePriority = battlePriority.CheckPositive();
      this.HitChanceWeight = hitChanceWeight.CheckPositive();
      this.ExtraRoundsToEscape = extraRoundsToEscape;
    }

    public override void ApplyToStats(FleetEntityStats stats)
    {
      base.ApplyToStats(stats);
      stats.HitPoints += this.MaxHp;
    }

    public FleetEntityHullProto.ID Id => new FleetEntityHullProto.ID(base.Id.Value);

    static FleetEntityHullProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntityHullProto.RepairDurationPerProduct = 2.Ticks();
    }

    public new class Gfx : FleetEntityGfx
    {
      public static readonly FleetEntityHullProto.Gfx Empty;
      /// <summary>
      /// Defines the width of the ship in the icon (from left).
      /// </summary>
      public readonly Percent IconContentWidth;
      /// <summary>
      /// Defines the percent of height (from top) which is unoccupied.
      /// </summary>
      public readonly Percent IconContentTopOffset;

      public Gfx(string iconPath, Percent iconContentWidth, Percent iconContentTopOffset)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(iconPath);
        this.IconContentWidth = iconContentWidth;
        this.IconContentTopOffset = iconContentTopOffset;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FleetEntityHullProto.Gfx.Empty = new FleetEntityHullProto.Gfx("EMPTY", Percent.Zero, Percent.Zero);
      }
    }

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<FleetEntityHullProto.ID>,
      IComparable<FleetEntityHullProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Fleet.FleetEntityPartProto.ID" />.
      /// </summary>
      public static implicit operator FleetEntityPartProto.ID(FleetEntityHullProto.ID id)
      {
        return new FleetEntityPartProto.ID(id.Value);
      }

      public static bool operator ==(FleetEntityPartProto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetEntityHullProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityPartProto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityHullProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(FleetEntityHullProto.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetEntityHullProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityHullProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetEntityHullProto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityHullProto.ID lhs, FleetEntityHullProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is FleetEntityHullProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(FleetEntityHullProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(FleetEntityHullProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(FleetEntityHullProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static FleetEntityHullProto.ID Deserialize(BlobReader reader)
      {
        return new FleetEntityHullProto.ID(reader.ReadString());
      }
    }
  }
}
