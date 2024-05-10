// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesF
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
  /// Normalized terrain thickness with one unit equal to one tile and origin at 0.
  /// </summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct ThicknessTilesF : IEquatable<ThicknessTilesF>, IComparable<ThicknessTilesF>
  {
    public readonly Fix32 Value;
    public static readonly ThicknessTilesF Quarter;
    public static readonly ThicknessTilesF Half;
    public static readonly ThicknessTilesF One;
    public static readonly ThicknessTilesF Two;

    public static ThicknessTilesF Zero => new ThicknessTilesF();

    public static ThicknessTilesF MinValue => new ThicknessTilesF(Fix32.MinValue);

    public static ThicknessTilesF MaxValue => new ThicknessTilesF(Fix32.MaxValue);

    public static ThicknessTilesF Epsilon => new ThicknessTilesF(Fix32.Epsilon);

    public ThicknessTilesF(Fix32 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public ThicknessTilesF(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(value);
    }

    public static ThicknessTilesF FromFraction(long numerator, long denominator)
    {
      return new ThicknessTilesF(Fix32.FromFraction(numerator, denominator));
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesF Abs => new ThicknessTilesF(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public ThicknessTilesF Min(ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(this.Value.Min(rhs.Value));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public ThicknessTilesF Max(ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(this.Value.Max(rhs.Value));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public ThicknessTilesF Clamp(ThicknessTilesF min, ThicknessTilesF max)
    {
      return new ThicknessTilesF(this.Value.Clamp(min.Value, max.Value));
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
    public ThicknessTilesF ScaledBy(Percent scale) => new ThicknessTilesF(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(ThicknessTilesF other) => this.Value.IsNear(other.Value);

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(ThicknessTilesF other, ThicknessTilesF tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    [Pure]
    public ThicknessTilesF Lerp(ThicknessTilesF other, Percent t)
    {
      return new ThicknessTilesF(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public ThicknessTilesF Lerp(ThicknessTilesF other, Fix32 t)
    {
      return new ThicknessTilesF(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public ThicknessTilesF Lerp(ThicknessTilesF other, Fix32 t, Fix32 scale)
    {
      return new ThicknessTilesF(this.Value.Lerp(other.Value, t, scale));
    }

    [Pure]
    public ThicknessTilesF SmoothStep(ThicknessTilesF other, Fix32 t)
    {
      return new ThicknessTilesF(this.Value.SmoothStep(other.Value, t));
    }

    [Pure]
    public ThicknessTilesF EaseIn(ThicknessTilesF other, Fix32 t)
    {
      return new ThicknessTilesF(this.Value.EaseIn(other.Value, t));
    }

    [Pure]
    public ThicknessTilesF EaseOut(ThicknessTilesF other, Fix32 t)
    {
      return new ThicknessTilesF(this.Value.EaseOut(other.Value, t));
    }

    [Pure]
    public ThicknessTilesF EaseInOut(ThicknessTilesF other, Fix32 t)
    {
      return new ThicknessTilesF(this.Value.EaseInOut(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public ThicknessTilesF Average(ThicknessTilesF other)
    {
      return new ThicknessTilesF((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value.MultAsFix64(this.Value);

    public override string ToString() => this.Value.ToString();

    public bool Equals(ThicknessTilesF other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is ThicknessTilesF other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(ThicknessTilesF other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static ThicknessTilesF operator -(ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(-rhs.Value);
    }

    public static ThicknessTilesF operator +(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(lhs.Value + rhs.Value);
    }

    public static ThicknessTilesF operator -(ThicknessTilesF lhs, ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(lhs.Value - rhs.Value);
    }

    public static ThicknessTilesF operator *(ThicknessTilesF lhs, Fix32 rhs)
    {
      return new ThicknessTilesF(lhs.Value * rhs);
    }

    public static ThicknessTilesF operator *(Fix32 lhs, ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(lhs * rhs.Value);
    }

    public static ThicknessTilesF operator *(ThicknessTilesF lhs, int rhs)
    {
      return new ThicknessTilesF(lhs.Value * rhs);
    }

    public static ThicknessTilesF operator *(int lhs, ThicknessTilesF rhs)
    {
      return new ThicknessTilesF(lhs * rhs.Value);
    }

    public static ThicknessTilesF operator /(ThicknessTilesF lhs, Fix32 rhs)
    {
      return new ThicknessTilesF(lhs.Value / rhs);
    }

    public static ThicknessTilesF operator /(ThicknessTilesF lhs, int rhs)
    {
      return new ThicknessTilesF(lhs.Value / rhs);
    }

    public static void Serialize(ThicknessTilesF value, BlobWriter writer)
    {
      Fix32.Serialize(value.Value, writer);
    }

    public static ThicknessTilesF Deserialize(BlobReader reader)
    {
      return new ThicknessTilesF(Fix32.Deserialize(reader));
    }

    public static ThicknessTilesF FromMeters(int meters)
    {
      return new ThicknessTilesF(meters.ToFix32() / 2);
    }

    public static ThicknessTilesF FromMeters(float meters)
    {
      return new ThicknessTilesF(meters.ToFix32() / 2);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesI CeiledThicknessTilesI => new ThicknessTilesI(this.Value.ToIntCeiled());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesI FlooredThicknessTilesI => new ThicknessTilesI(this.Value.ToIntFloored());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesI RoundedThicknessTilesI => new ThicknessTilesI(this.Value.ToIntRounded());

    /// <summary>
    /// Returns value divided by two very efficiently (uses a bit shift). Use with caution, this works for all values
    /// except for <see cref="P:Mafi.ThicknessTilesF.MinValue" /> (the smallest representable value).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesF HalfFast => new ThicknessTilesF(this.Value.HalfFast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Meters => this.Value * 2;

    /// <summary>
    /// Removes as much as possible from this thickness without going to negative values and returns the result.
    /// Given value to remove also stores remainder if there was not enough thickness to remove.
    /// </summary>
    [Pure]
    public ThicknessTilesF RemoveAsMuchAs(ref ThicknessTilesF amountAndRemainder)
    {
      amountAndRemainder = new ThicknessTilesF((this.Value - amountAndRemainder.Value).Max((Fix32) 0));
      return new ThicknessTilesF(this.Value - amountAndRemainder.Value);
    }

    [Pure]
    public ThicknessTilesF MultByUnchecked(Fix32 value)
    {
      return new ThicknessTilesF(this.Value.MultByUnchecked(value));
    }

    public static bool operator ==(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(ThicknessTilesF lhs, ThicknessTilesI rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static bool operator ==(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(ThicknessTilesI lhs, ThicknessTilesF rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    static ThicknessTilesF()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ThicknessTilesF.Quarter = new ThicknessTilesF(Fix32.Quarter);
      ThicknessTilesF.Half = new ThicknessTilesF(Fix32.Half);
      ThicknessTilesF.One = new ThicknessTilesF(Fix32.One);
      ThicknessTilesF.Two = new ThicknessTilesF(Fix32.Two);
    }
  }
}
