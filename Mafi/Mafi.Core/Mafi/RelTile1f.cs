// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile1f
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
  /// <summary>Immutable 1D relative tile coordinate or distance.</summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct RelTile1f : IEquatable<RelTile1f>, IComparable<RelTile1f>
  {
    public readonly Fix32 Value;

    public static RelTile1f Zero => new RelTile1f();

    public static RelTile1f MinValue => new RelTile1f(Fix32.MinValue);

    public static RelTile1f MaxValue => new RelTile1f(Fix32.MaxValue);

    public static RelTile1f Epsilon => new RelTile1f(Fix32.Epsilon);

    public RelTile1f(Fix32 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public RelTile1f(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(value);
    }

    public static RelTile1f FromFraction(long numerator, long denominator)
    {
      return new RelTile1f(Fix32.FromFraction(numerator, denominator));
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile1f Abs => new RelTile1f(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public RelTile1f Min(RelTile1f rhs) => new RelTile1f(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public RelTile1f Max(RelTile1f rhs) => new RelTile1f(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public RelTile1f Clamp(RelTile1f min, RelTile1f max)
    {
      return new RelTile1f(this.Value.Clamp(min.Value, max.Value));
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
    public RelTile1f ScaledBy(Percent scale) => new RelTile1f(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(RelTile1f other) => this.Value.IsNear(other.Value);

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(RelTile1f other, RelTile1f tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    [Pure]
    public RelTile1f Lerp(RelTile1f other, Percent t)
    {
      return new RelTile1f(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public RelTile1f Lerp(RelTile1f other, Fix32 t)
    {
      return new RelTile1f(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public RelTile1f Lerp(RelTile1f other, Fix32 t, Fix32 scale)
    {
      return new RelTile1f(this.Value.Lerp(other.Value, t, scale));
    }

    [Pure]
    public RelTile1f SmoothStep(RelTile1f other, Fix32 t)
    {
      return new RelTile1f(this.Value.SmoothStep(other.Value, t));
    }

    [Pure]
    public RelTile1f EaseIn(RelTile1f other, Fix32 t)
    {
      return new RelTile1f(this.Value.EaseIn(other.Value, t));
    }

    [Pure]
    public RelTile1f EaseOut(RelTile1f other, Fix32 t)
    {
      return new RelTile1f(this.Value.EaseOut(other.Value, t));
    }

    [Pure]
    public RelTile1f EaseInOut(RelTile1f other, Fix32 t)
    {
      return new RelTile1f(this.Value.EaseInOut(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public RelTile1f Average(RelTile1f other) => new RelTile1f((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value.MultAsFix64(this.Value);

    public override string ToString() => this.Value.ToString();

    public bool Equals(RelTile1f other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is RelTile1f other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(RelTile1f other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(RelTile1f lhs, RelTile1f rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(RelTile1f lhs, RelTile1f rhs) => lhs.Value != rhs.Value;

    public static bool operator <(RelTile1f lhs, RelTile1f rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(RelTile1f lhs, RelTile1f rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(RelTile1f lhs, RelTile1f rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(RelTile1f lhs, RelTile1f rhs) => lhs.Value >= rhs.Value;

    public static RelTile1f operator -(RelTile1f rhs) => new RelTile1f(-rhs.Value);

    public static RelTile1f operator +(RelTile1f lhs, RelTile1f rhs)
    {
      return new RelTile1f(lhs.Value + rhs.Value);
    }

    public static RelTile1f operator -(RelTile1f lhs, RelTile1f rhs)
    {
      return new RelTile1f(lhs.Value - rhs.Value);
    }

    public static RelTile1f operator *(RelTile1f lhs, Fix32 rhs) => new RelTile1f(lhs.Value * rhs);

    public static RelTile1f operator *(Fix32 lhs, RelTile1f rhs) => new RelTile1f(lhs * rhs.Value);

    public static RelTile1f operator *(RelTile1f lhs, int rhs) => new RelTile1f(lhs.Value * rhs);

    public static RelTile1f operator *(int lhs, RelTile1f rhs) => new RelTile1f(lhs * rhs.Value);

    public static RelTile1f operator /(RelTile1f lhs, Fix32 rhs) => new RelTile1f(lhs.Value / rhs);

    public static RelTile1f operator /(RelTile1f lhs, int rhs) => new RelTile1f(lhs.Value / rhs);

    public static void Serialize(RelTile1f value, BlobWriter writer)
    {
      Fix32.Serialize(value.Value, writer);
    }

    public static RelTile1f Deserialize(BlobReader reader)
    {
      return new RelTile1f(Fix32.Deserialize(reader));
    }

    public static RelTile1f One => new RelTile1f(Fix32.One);

    public static RelTile1f Half => new RelTile1f(Fix32.Half);

    public static RelTile1f Quarter => new RelTile1f(Fix32.Quarter);

    /// <summary>
    /// Returns discrete distance per tick from given distance in tiles per second.
    /// </summary>
    [Pure]
    public static RelTile1f FromTilesPerSecond(double tilesPerSecond)
    {
      return new RelTile1f(Fix32.FromDouble(tilesPerSecond / 10.0));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ThicknessTilesF ThicknessTilesF => new ThicknessTilesF(this.Value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile1i RoundedRelTile1i => new RelTile1i(this.Value.ToIntRounded());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile1i CeiledRelTile1i => new RelTile1i(this.Value.ToIntCeiled());
  }
}
