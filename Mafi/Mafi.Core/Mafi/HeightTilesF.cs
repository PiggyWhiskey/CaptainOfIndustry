// Decompiled with JetBrains decompiler
// Type: Mafi.HeightTilesF
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
  /// Normalized absolute height with one unit equal to one tile and origin at 0.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct HeightTilesF : IEquatable<HeightTilesF>, IComparable<HeightTilesF>
  {
    public readonly Fix32 Value;

    public static HeightTilesF Zero => new HeightTilesF();

    public static HeightTilesF MinValue => new HeightTilesF(Fix32.MinValue);

    public static HeightTilesF MaxValue => new HeightTilesF(Fix32.MaxValue);

    public static HeightTilesF Epsilon => new HeightTilesF(Fix32.Epsilon);

    public HeightTilesF(Fix32 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public HeightTilesF(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(value);
    }

    public static HeightTilesF FromFraction(long numerator, long denominator)
    {
      return new HeightTilesF(Fix32.FromFraction(numerator, denominator));
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesF Abs => new HeightTilesF(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public HeightTilesF Min(HeightTilesF rhs) => new HeightTilesF(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public HeightTilesF Max(HeightTilesF rhs) => new HeightTilesF(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public HeightTilesF Clamp(HeightTilesF min, HeightTilesF max)
    {
      return new HeightTilesF(this.Value.Clamp(min.Value, max.Value));
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

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(HeightTilesF other) => this.Value.IsNear(other.Value);

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(HeightTilesF other, HeightTilesF tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    [Pure]
    public HeightTilesF Lerp(HeightTilesF other, Percent t)
    {
      return new HeightTilesF(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public HeightTilesF Lerp(HeightTilesF other, Fix32 t)
    {
      return new HeightTilesF(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public HeightTilesF Lerp(HeightTilesF other, Fix32 t, Fix32 scale)
    {
      return new HeightTilesF(this.Value.Lerp(other.Value, t, scale));
    }

    [Pure]
    public HeightTilesF SmoothStep(HeightTilesF other, Fix32 t)
    {
      return new HeightTilesF(this.Value.SmoothStep(other.Value, t));
    }

    [Pure]
    public HeightTilesF EaseIn(HeightTilesF other, Fix32 t)
    {
      return new HeightTilesF(this.Value.EaseIn(other.Value, t));
    }

    [Pure]
    public HeightTilesF EaseOut(HeightTilesF other, Fix32 t)
    {
      return new HeightTilesF(this.Value.EaseOut(other.Value, t));
    }

    [Pure]
    public HeightTilesF EaseInOut(HeightTilesF other, Fix32 t)
    {
      return new HeightTilesF(this.Value.EaseInOut(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public HeightTilesF Average(HeightTilesF other)
    {
      return new HeightTilesF((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value.MultAsFix64(this.Value);

    public override string ToString() => this.Value.ToString();

    public bool Equals(HeightTilesF other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is HeightTilesF other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(HeightTilesF other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value != rhs.Value;

    public static bool operator <(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(HeightTilesF lhs, HeightTilesF rhs) => lhs.Value >= rhs.Value;

    public static HeightTilesF operator -(HeightTilesF rhs) => new HeightTilesF(-rhs.Value);

    public static void Serialize(HeightTilesF value, BlobWriter writer)
    {
      Fix32.Serialize(value.Value, writer);
    }

    public static HeightTilesF Deserialize(BlobReader reader)
    {
      return new HeightTilesF(Fix32.Deserialize(reader));
    }

    public static HeightTilesF One => new HeightTilesF(Fix32.One);

    public static HeightTilesF Half => new HeightTilesF(Fix32.Half);

    public static HeightTilesF Quarter => new HeightTilesF(Fix32.Quarter);

    /// <summary>
    /// Converts floating point height to integer height (vertical tile index).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI HeightI => new HeightTilesI(this.Value.ToIntFloored());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI TilesHeightCeiled => new HeightTilesI(this.Value.ToIntCeiled());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI TilesHeightFloored => new HeightTilesI(this.Value.ToIntFloored());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI TilesHeightRounded => new HeightTilesI(this.Value.ToIntRounded());

    /// <summary>
    /// Missing thickness to fill current vertical tile. Range of this value is [0, 1). Value 0 means that the
    /// current vertical tile is fully completed. Otherwise it is number of discrete units that are missing.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesF MissingThicknessToTile => this.TilesHeightCeiled.HeightTilesF - this;

    public static HeightTilesF operator +(HeightTilesF lhs, ThicknessTilesF rhs)
    {
      return new HeightTilesF(lhs.Value + rhs.Value);
    }

    public static HeightTilesF operator +(ThicknessTilesF lhs, HeightTilesF rhs)
    {
      return new HeightTilesF(lhs.Value + rhs.Value);
    }

    public static ThicknessTilesF operator -(HeightTilesF lhs, HeightTilesF rhs)
    {
      return new ThicknessTilesF(lhs.Value - rhs.Value);
    }

    public static HeightTilesF operator -(HeightTilesF lhs, ThicknessTilesF rhs)
    {
      return new HeightTilesF(lhs.Value - rhs.Value);
    }

    public static HeightTilesF operator +(HeightTilesF lhs, HeightTilesI rhs)
    {
      return new HeightTilesF(lhs.Value + (Fix32) rhs.Value);
    }

    public static ThicknessTilesF operator -(HeightTilesF lhs, HeightTilesI rhs)
    {
      return new ThicknessTilesF(lhs.Value - (Fix32) rhs.Value);
    }

    public static bool operator ==(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value != rhs.Value;

    public static bool operator <(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(HeightTilesF lhs, HeightTilesI rhs) => lhs.Value >= rhs.Value;

    public static bool operator ==(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value != rhs.Value;

    public static bool operator <(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(HeightTilesI lhs, HeightTilesF rhs) => lhs.Value >= rhs.Value;
  }
}
