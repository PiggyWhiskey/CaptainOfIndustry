// Decompiled with JetBrains decompiler
// Type: Mafi.BattleTriggerPriority
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct BattleTriggerPriority : 
    IEquatable<BattleTriggerPriority>,
    IComparable<BattleTriggerPriority>
  {
    public static readonly BattleTriggerPriority Highest;
    public static readonly BattleTriggerPriority Fleet;
    public static readonly BattleTriggerPriority Building;
    public static readonly BattleTriggerPriority Lowest;
    /// <summary>Marks triggers that do not support priority.</summary>
    public static readonly BattleTriggerPriority NotSupported;
    public readonly int Value;

    public static BattleTriggerPriority Zero => new BattleTriggerPriority();

    public static BattleTriggerPriority MinValue => new BattleTriggerPriority(int.MinValue);

    public static BattleTriggerPriority MaxValue => new BattleTriggerPriority(int.MaxValue);

    public BattleTriggerPriority(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public BattleTriggerPriority Abs => new BattleTriggerPriority(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public BattleTriggerPriority Min(BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(this.Value.Min(rhs.Value));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public BattleTriggerPriority Max(BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(this.Value.Max(rhs.Value));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public BattleTriggerPriority Clamp(BattleTriggerPriority min, BattleTriggerPriority max)
    {
      return new BattleTriggerPriority(this.Value.Clamp(min.Value, max.Value));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Value == 0;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Value != 0;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Value > 0;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Value <= 0;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Value < 0;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Value >= 0;

    [Pure]
    public BattleTriggerPriority ScaledBy(Percent scale)
    {
      return new BattleTriggerPriority(scale.Apply(this.Value));
    }

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(BattleTriggerPriority other, BattleTriggerPriority tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public BattleTriggerPriority Lerp(BattleTriggerPriority other, int t, int scale)
    {
      return new BattleTriggerPriority(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public BattleTriggerPriority Lerp(BattleTriggerPriority other, Percent t)
    {
      return new BattleTriggerPriority(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public BattleTriggerPriority Average(BattleTriggerPriority other)
    {
      return new BattleTriggerPriority((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(BattleTriggerPriority other) => this.Value == other.Value;

    public override bool Equals(object obj)
    {
      return obj is BattleTriggerPriority other && this.Equals(other);
    }

    public override int GetHashCode() => this.Value;

    public int CompareTo(BattleTriggerPriority other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(BattleTriggerPriority lhs, BattleTriggerPriority rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static BattleTriggerPriority operator -(BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(-rhs.Value);
    }

    public static BattleTriggerPriority operator +(
      BattleTriggerPriority lhs,
      BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(lhs.Value + rhs.Value);
    }

    public static BattleTriggerPriority operator -(
      BattleTriggerPriority lhs,
      BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(lhs.Value - rhs.Value);
    }

    public static BattleTriggerPriority operator *(BattleTriggerPriority lhs, int rhs)
    {
      return new BattleTriggerPriority(lhs.Value * rhs);
    }

    public static BattleTriggerPriority operator *(int lhs, BattleTriggerPriority rhs)
    {
      return new BattleTriggerPriority(lhs * rhs.Value);
    }

    public static BattleTriggerPriority operator /(BattleTriggerPriority lhs, int rhs)
    {
      return new BattleTriggerPriority(lhs.Value / rhs);
    }

    public static void Serialize(BattleTriggerPriority value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static BattleTriggerPriority Deserialize(BlobReader reader)
    {
      return new BattleTriggerPriority(reader.ReadInt());
    }

    static BattleTriggerPriority()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BattleTriggerPriority.Highest = new BattleTriggerPriority(0);
      BattleTriggerPriority.Fleet = new BattleTriggerPriority(10);
      BattleTriggerPriority.Building = new BattleTriggerPriority(20);
      BattleTriggerPriority.Lowest = new BattleTriggerPriority(100);
      BattleTriggerPriority.NotSupported = new BattleTriggerPriority(1000);
    }
  }
}
