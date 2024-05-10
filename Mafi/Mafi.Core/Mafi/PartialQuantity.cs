// Decompiled with JetBrains decompiler
// Type: Mafi.PartialQuantity
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
  /// Represents partial discrete quantity as fixed point decimal number with precision of 1/256.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct PartialQuantity : IEquatable<PartialQuantity>, IComparable<PartialQuantity>
  {
    public readonly Fix32 Value;

    public static PartialQuantity Zero => new PartialQuantity();

    public static PartialQuantity MinValue => new PartialQuantity(Fix32.MinValue);

    public static PartialQuantity MaxValue => new PartialQuantity(Fix32.MaxValue);

    public static PartialQuantity Epsilon => new PartialQuantity(Fix32.Epsilon);

    public PartialQuantity(Fix32 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public PartialQuantity(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(value);
    }

    public static PartialQuantity FromFraction(long numerator, long denominator)
    {
      return new PartialQuantity(Fix32.FromFraction(numerator, denominator));
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public PartialQuantity Abs => new PartialQuantity(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public PartialQuantity Min(PartialQuantity rhs)
    {
      return new PartialQuantity(this.Value.Min(rhs.Value));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public PartialQuantity Max(PartialQuantity rhs)
    {
      return new PartialQuantity(this.Value.Max(rhs.Value));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public PartialQuantity Clamp(PartialQuantity min, PartialQuantity max)
    {
      return new PartialQuantity(this.Value.Clamp(min.Value, max.Value));
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
    public PartialQuantity ScaledBy(Percent scale) => new PartialQuantity(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(PartialQuantity other) => this.Value.IsNear(other.Value);

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(PartialQuantity other, PartialQuantity tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    [Pure]
    public PartialQuantity Lerp(PartialQuantity other, Percent t)
    {
      return new PartialQuantity(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public PartialQuantity Lerp(PartialQuantity other, Fix32 t)
    {
      return new PartialQuantity(this.Value.Lerp(other.Value, t));
    }

    [Pure]
    public PartialQuantity Lerp(PartialQuantity other, Fix32 t, Fix32 scale)
    {
      return new PartialQuantity(this.Value.Lerp(other.Value, t, scale));
    }

    [Pure]
    public PartialQuantity SmoothStep(PartialQuantity other, Fix32 t)
    {
      return new PartialQuantity(this.Value.SmoothStep(other.Value, t));
    }

    [Pure]
    public PartialQuantity EaseIn(PartialQuantity other, Fix32 t)
    {
      return new PartialQuantity(this.Value.EaseIn(other.Value, t));
    }

    [Pure]
    public PartialQuantity EaseOut(PartialQuantity other, Fix32 t)
    {
      return new PartialQuantity(this.Value.EaseOut(other.Value, t));
    }

    [Pure]
    public PartialQuantity EaseInOut(PartialQuantity other, Fix32 t)
    {
      return new PartialQuantity(this.Value.EaseInOut(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public PartialQuantity Average(PartialQuantity other)
    {
      return new PartialQuantity((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value.MultAsFix64(this.Value);

    public override string ToString() => this.Value.ToString();

    public bool Equals(PartialQuantity other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is PartialQuantity other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(PartialQuantity other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(PartialQuantity lhs, PartialQuantity rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static PartialQuantity operator -(PartialQuantity rhs)
    {
      return new PartialQuantity(-rhs.Value);
    }

    public static PartialQuantity operator +(PartialQuantity lhs, PartialQuantity rhs)
    {
      return new PartialQuantity(lhs.Value + rhs.Value);
    }

    public static PartialQuantity operator -(PartialQuantity lhs, PartialQuantity rhs)
    {
      return new PartialQuantity(lhs.Value - rhs.Value);
    }

    public static PartialQuantity operator *(PartialQuantity lhs, Fix32 rhs)
    {
      return new PartialQuantity(lhs.Value * rhs);
    }

    public static PartialQuantity operator *(Fix32 lhs, PartialQuantity rhs)
    {
      return new PartialQuantity(lhs * rhs.Value);
    }

    public static PartialQuantity operator *(PartialQuantity lhs, int rhs)
    {
      return new PartialQuantity(lhs.Value * rhs);
    }

    public static PartialQuantity operator *(int lhs, PartialQuantity rhs)
    {
      return new PartialQuantity(lhs * rhs.Value);
    }

    public static PartialQuantity operator /(PartialQuantity lhs, Fix32 rhs)
    {
      return new PartialQuantity(lhs.Value / rhs);
    }

    public static PartialQuantity operator /(PartialQuantity lhs, int rhs)
    {
      return new PartialQuantity(lhs.Value / rhs);
    }

    public static void Serialize(PartialQuantity value, BlobWriter writer)
    {
      Fix32.Serialize(value.Value, writer);
    }

    public static PartialQuantity Deserialize(BlobReader reader)
    {
      return new PartialQuantity(Fix32.Deserialize(reader));
    }

    public static PartialQuantity One => new PartialQuantity(Quantity.One);

    public Quantity IntegerPart => new Quantity(this.Value.IntegerPart);

    public PartialQuantity FractionalPart => new PartialQuantity(this.Value.FractionalPart);

    public PartialQuantity(Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(quantity.Value);
    }

    public static PartialQuantity FromFraction(Quantity numerator, long denominator)
    {
      return new PartialQuantity(Fix32.FromFraction((long) numerator.Value, denominator));
    }

    [Pure]
    public Quantity ToQuantityRounded() => new Quantity(this.Value.ToIntRounded());

    [Pure]
    public Quantity ToQuantityCeiled() => new Quantity(this.Value.ToIntCeiled());

    [Pure]
    public PartialQuantity InverslyScaledBy(Percent percent)
    {
      return new PartialQuantity(percent.ApplyInverse(this.Value));
    }

    [Pure]
    public string ToStringRounded(int decimalDigits = 2)
    {
      return this.Value.ToStringRounded(decimalDigits);
    }

    public static bool operator ==(PartialQuantity lhs, Quantity rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(PartialQuantity lhs, Quantity rhs) => lhs.Value != rhs.Value;

    public static bool operator <(PartialQuantity lhs, Quantity rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(PartialQuantity lhs, Quantity rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(PartialQuantity lhs, Quantity rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(PartialQuantity lhs, Quantity rhs) => lhs.Value >= rhs.Value;

    public static bool operator ==(Quantity lhs, PartialQuantity rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Quantity lhs, PartialQuantity rhs) => lhs.Value != rhs.Value;

    public static bool operator <(Quantity lhs, PartialQuantity rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Quantity lhs, PartialQuantity rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(Quantity lhs, PartialQuantity rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Quantity lhs, PartialQuantity rhs) => lhs.Value >= rhs.Value;
  }
}
