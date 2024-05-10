// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesI
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
  /// <summary>
  /// Normalized terrain thickness with one unit equal to one tile.
  /// </summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct ThicknessTilesI : IEquatable<ThicknessTilesI>, IComparable<ThicknessTilesI>
  {
    public readonly int Value;
    public static readonly ThicknessTilesI One;
    public static readonly ThicknessTilesI Two;

    public static ThicknessTilesI Zero => new ThicknessTilesI();

    public static ThicknessTilesI MinValue => new ThicknessTilesI(int.MinValue);

    public static ThicknessTilesI MaxValue => new ThicknessTilesI(int.MaxValue);

    public ThicknessTilesI(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesI Abs => new ThicknessTilesI(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public ThicknessTilesI Min(ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(this.Value.Min(rhs.Value));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public ThicknessTilesI Max(ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(this.Value.Max(rhs.Value));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public ThicknessTilesI Clamp(ThicknessTilesI min, ThicknessTilesI max)
    {
      return new ThicknessTilesI(this.Value.Clamp(min.Value, max.Value));
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
    public ThicknessTilesI ScaledBy(Percent scale) => new ThicknessTilesI(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(ThicknessTilesI other, ThicknessTilesI tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public ThicknessTilesI Lerp(ThicknessTilesI other, int t, int scale)
    {
      return new ThicknessTilesI(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public ThicknessTilesI Lerp(ThicknessTilesI other, Percent t)
    {
      return new ThicknessTilesI(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public ThicknessTilesI Average(ThicknessTilesI other)
    {
      return new ThicknessTilesI((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(ThicknessTilesI other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is ThicknessTilesI other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(ThicknessTilesI other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static ThicknessTilesI operator -(ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(-rhs.Value);
    }

    public static ThicknessTilesI operator +(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(lhs.Value + rhs.Value);
    }

    public static ThicknessTilesI operator -(ThicknessTilesI lhs, ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(lhs.Value - rhs.Value);
    }

    public static ThicknessTilesI operator *(ThicknessTilesI lhs, int rhs)
    {
      return new ThicknessTilesI(lhs.Value * rhs);
    }

    public static ThicknessTilesI operator *(int lhs, ThicknessTilesI rhs)
    {
      return new ThicknessTilesI(lhs * rhs.Value);
    }

    public static ThicknessTilesI operator /(ThicknessTilesI lhs, int rhs)
    {
      return new ThicknessTilesI(lhs.Value / rhs);
    }

    public static void Serialize(ThicknessTilesI value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static ThicknessTilesI Deserialize(BlobReader reader)
    {
      return new ThicknessTilesI(reader.ReadInt());
    }

    public ThicknessTilesF ThicknessTilesF => new ThicknessTilesF(this.Value);

    public ThicknessTilesISlim AsSlim => new ThicknessTilesISlim((sbyte) this.Value);

    public ThicknessTilesISemiSlim AsSemiSlim => new ThicknessTilesISemiSlim((short) this.Value);

    static ThicknessTilesI()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ThicknessTilesI.One = new ThicknessTilesI(1);
      ThicknessTilesI.Two = new ThicknessTilesI(2);
    }
  }
}
