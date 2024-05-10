// Decompiled with JetBrains decompiler
// Type: Mafi.QuantityLarge
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
  /// <summary>Immutable discrete quantity for large quantities.</summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct QuantityLarge : IEquatable<QuantityLarge>, IComparable<QuantityLarge>
  {
    public readonly long Value;

    public static QuantityLarge Zero => new QuantityLarge();

    public static QuantityLarge MinValue => new QuantityLarge(long.MinValue);

    public static QuantityLarge MaxValue => new QuantityLarge(long.MaxValue);

    public QuantityLarge(long value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public QuantityLarge Abs => new QuantityLarge(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public QuantityLarge Min(QuantityLarge rhs) => new QuantityLarge(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public QuantityLarge Max(QuantityLarge rhs) => new QuantityLarge(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public QuantityLarge Clamp(QuantityLarge min, QuantityLarge max)
    {
      return new QuantityLarge(this.Value.Clamp(min.Value, max.Value));
    }

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.Value == 0L;

    /// <summary>Whether this value is equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.Value != 0L;

    /// <summary>Whether this value is greater than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.Value > 0L;

    /// <summary>Whether this value is less or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.Value <= 0L;

    /// <summary>Whether this value is less than zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.Value < 0L;

    /// <summary>Whether this value is greater or equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.Value >= 0L;

    [Pure]
    public QuantityLarge ScaledBy(Percent scale) => new QuantityLarge(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(QuantityLarge other, QuantityLarge tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public QuantityLarge Average(QuantityLarge other)
    {
      return new QuantityLarge((this.Value + other.Value) / 2L);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => this.Value * this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(QuantityLarge other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is QuantityLarge other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(QuantityLarge other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value != rhs.Value;

    public static bool operator <(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(QuantityLarge lhs, QuantityLarge rhs) => lhs.Value >= rhs.Value;

    public static QuantityLarge operator -(QuantityLarge rhs) => new QuantityLarge(-rhs.Value);

    public static QuantityLarge operator +(QuantityLarge lhs, QuantityLarge rhs)
    {
      return new QuantityLarge(lhs.Value + rhs.Value);
    }

    public static QuantityLarge operator -(QuantityLarge lhs, QuantityLarge rhs)
    {
      return new QuantityLarge(lhs.Value - rhs.Value);
    }

    public static QuantityLarge operator *(QuantityLarge lhs, long rhs)
    {
      return new QuantityLarge(lhs.Value * rhs);
    }

    public static QuantityLarge operator *(long lhs, QuantityLarge rhs)
    {
      return new QuantityLarge(lhs * rhs.Value);
    }

    public static QuantityLarge operator *(QuantityLarge lhs, int rhs)
    {
      return new QuantityLarge(lhs.Value * (long) rhs);
    }

    public static QuantityLarge operator *(int lhs, QuantityLarge rhs)
    {
      return new QuantityLarge((long) lhs * rhs.Value);
    }

    public static QuantityLarge operator /(QuantityLarge lhs, long rhs)
    {
      return new QuantityLarge(lhs.Value / rhs);
    }

    public static QuantityLarge operator /(QuantityLarge lhs, int rhs)
    {
      return new QuantityLarge(lhs.Value / (long) rhs);
    }

    public static void Serialize(QuantityLarge value, BlobWriter writer)
    {
      writer.WriteLong(value.Value);
    }

    public static QuantityLarge Deserialize(BlobReader reader)
    {
      return new QuantityLarge(reader.ReadLong());
    }

    public static QuantityLarge One => new QuantityLarge(1L);

    public static QuantityLarge operator +(QuantityLarge lhs, Quantity rhs)
    {
      return new QuantityLarge(lhs.Value + (long) rhs.Value);
    }

    public static QuantityLarge operator +(Quantity lhs, QuantityLarge rhs)
    {
      return new QuantityLarge((long) lhs.Value + rhs.Value);
    }

    public static QuantityLarge operator -(QuantityLarge lhs, Quantity rhs)
    {
      return new QuantityLarge(lhs.Value - (long) rhs.Value);
    }

    public static bool operator ==(Quantity lhs, QuantityLarge rhs)
    {
      return (long) lhs.Value == rhs.Value;
    }

    public static bool operator !=(Quantity lhs, QuantityLarge rhs)
    {
      return (long) lhs.Value != rhs.Value;
    }

    public static bool operator <(Quantity lhs, QuantityLarge rhs) => (long) lhs.Value < rhs.Value;

    public static bool operator <=(Quantity lhs, QuantityLarge rhs)
    {
      return (long) lhs.Value <= rhs.Value;
    }

    public static bool operator >(Quantity lhs, QuantityLarge rhs) => (long) lhs.Value > rhs.Value;

    public static bool operator >=(Quantity lhs, QuantityLarge rhs)
    {
      return (long) lhs.Value >= rhs.Value;
    }

    public static implicit operator QuantityLarge(Quantity quantity)
    {
      return new QuantityLarge((long) quantity.Value);
    }

    public Quantity? ToQuantity()
    {
      return this > (QuantityLarge) Quantity.MaxValue ? new Quantity?() : new Quantity?(new Quantity((int) this.Value));
    }

    public Quantity ToQuantityClamped()
    {
      return !(this > (QuantityLarge) Quantity.MaxValue) ? new Quantity((int) this.Value) : Quantity.MaxValue;
    }
  }
}
