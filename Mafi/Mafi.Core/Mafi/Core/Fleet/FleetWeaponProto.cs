// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetWeaponProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Specifies a weapon family.</summary>
  public class FleetWeaponProto : DestructibleFleetPartProto
  {
    public readonly int Damage;
    public readonly int Range;
    public readonly int ReloadRounds;
    public readonly Percent AccuracyAtMinRange;
    public readonly Percent AccuracyAtMaxRange;

    public int AvgDamagePer10Rounds
    {
      get
      {
        return this.AccuracyAtMinRange.Average(this.AccuracyAtMaxRange).Apply(10 * this.Damage / this.ReloadRounds);
      }
    }

    public FleetWeaponProto(
      FleetWeaponProto.ID id,
      string name,
      AssetValue costs,
      int maxHp,
      int damage,
      int range,
      int reloadRounds,
      Percent accuracyAtMinRange,
      Percent accuracyAtMaxRange,
      int extraCrewNeeded,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntityPartProto.ID id1 = (FleetEntityPartProto.ID) id;
      Proto.Str str = Proto.CreateStr((Proto.ID) id, name, "", "ship part upgrade");
      AssetValue assetValue = costs;
      int maxHp1 = maxHp;
      int hitWeight = maxHp;
      FleetEntityGfx fleetEntityGfx = graphics;
      int extraCrewNeeded1 = extraCrewNeeded;
      FleetEntityGfx graphics1 = fleetEntityGfx;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, str, assetValue, maxHp1, hitWeight, extraCrewNeeded1, graphics1);
      this.Damage = damage.CheckPositive();
      this.Range = range.CheckPositive();
      this.ReloadRounds = reloadRounds.CheckPositive();
      this.AccuracyAtMinRange = accuracyAtMinRange;
      this.AccuracyAtMaxRange = accuracyAtMaxRange;
    }

    public override void ApplyTo(FleetEntity entity)
    {
      base.ApplyTo(entity);
      entity.AddWeapon(this);
    }

    public override void RemoveFrom(FleetEntity entity)
    {
      base.RemoveFrom(entity);
      entity.RemoveWeapon(this);
    }

    public override void ApplyToStats(FleetEntityStats stats)
    {
      base.ApplyToStats(stats);
      stats.HitPoints += this.MaxHp;
      stats.MaxWeaponRange = stats.MaxWeaponRange.Max(this.Range);
      stats.AvgDamage += this.AvgDamagePer10Rounds;
    }

    public FleetWeaponProto CloneWithNewId(
      FleetWeaponProto.ID id,
      string name,
      FleetEntityGfx graphics)
    {
      FleetWeaponProto.ID id1 = id;
      AssetValue assetValue = this.Value;
      string name1 = name;
      AssetValue costs = assetValue;
      int damage1 = this.Damage;
      int range1 = this.Range;
      int reloadRounds1 = this.ReloadRounds;
      int maxHp = this.MaxHp;
      int damage2 = damage1;
      int range2 = range1;
      int reloadRounds2 = reloadRounds1;
      Percent accuracyAtMinRange = this.AccuracyAtMinRange;
      Percent accuracyAtMaxRange = this.AccuracyAtMaxRange;
      int bonusValue = this.ExtraCrew.BonusValue;
      FleetEntityGfx graphics1 = graphics;
      return new FleetWeaponProto(id1, name1, costs, maxHp, damage2, range2, reloadRounds2, accuracyAtMinRange, accuracyAtMaxRange, bonusValue, graphics1);
    }

    public FleetWeaponProto.ID Id => new FleetWeaponProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    [ManuallyWrittenSerialization]
    public new readonly struct ID : IEquatable<FleetWeaponProto.ID>, IComparable<FleetWeaponProto.ID>
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
      public static implicit operator FleetEntityPartProto.ID(FleetWeaponProto.ID id)
      {
        return new FleetEntityPartProto.ID(id.Value);
      }

      public static bool operator ==(FleetEntityPartProto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetWeaponProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityPartProto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetWeaponProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(FleetWeaponProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetWeaponProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetWeaponProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetWeaponProto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetWeaponProto.ID lhs, FleetWeaponProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is FleetWeaponProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(FleetWeaponProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(FleetWeaponProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(FleetWeaponProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static FleetWeaponProto.ID Deserialize(BlobReader reader)
      {
        return new FleetWeaponProto.ID(reader.ReadString());
      }
    }
  }
}
