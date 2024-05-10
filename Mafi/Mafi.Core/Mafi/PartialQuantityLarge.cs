// Decompiled with JetBrains decompiler
// Type: Mafi.PartialQuantityLarge
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
  /// <summary>Represents partial discrete quantity using Fix64.</summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct PartialQuantityLarge : 
    IEquatable<PartialQuantityLarge>,
    IComparable<PartialQuantityLarge>
  {
    public readonly Fix64 Value;

    public static PartialQuantityLarge Zero => new PartialQuantityLarge();

    public static PartialQuantityLarge MinValue => new PartialQuantityLarge(Fix64.MinValue);

    public static PartialQuantityLarge MaxValue => new PartialQuantityLarge(Fix64.MaxValue);

    public PartialQuantityLarge(Fix64 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public PartialQuantityLarge Abs => new PartialQuantityLarge(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public PartialQuantityLarge Min(PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(this.Value.Min(rhs.Value));
    }

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public PartialQuantityLarge Max(PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(this.Value.Max(rhs.Value));
    }

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public PartialQuantityLarge Clamp(PartialQuantityLarge min, PartialQuantityLarge max)
    {
      return new PartialQuantityLarge(this.Value.Clamp(min.Value, max.Value));
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
    public PartialQuantityLarge ScaledBy(Percent scale)
    {
      return new PartialQuantityLarge(scale.Apply(this.Value));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public PartialQuantityLarge Average(PartialQuantityLarge other)
    {
      return new PartialQuantityLarge((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value * this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(PartialQuantityLarge other) => this.Value == other.Value;

    public override bool Equals(object obj)
    {
      return obj is PartialQuantityLarge other && this.Equals(other);
    }

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(PartialQuantityLarge other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value < rhs.Value;
    }

    public static bool operator <=(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value > rhs.Value;
    }

    public static bool operator >=(PartialQuantityLarge lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static PartialQuantityLarge operator -(PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(-rhs.Value);
    }

    public static PartialQuantityLarge operator +(
      PartialQuantityLarge lhs,
      PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(lhs.Value + rhs.Value);
    }

    public static PartialQuantityLarge operator -(
      PartialQuantityLarge lhs,
      PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(lhs.Value - rhs.Value);
    }

    public static PartialQuantityLarge operator *(PartialQuantityLarge lhs, Fix64 rhs)
    {
      return new PartialQuantityLarge(lhs.Value * rhs);
    }

    public static PartialQuantityLarge operator *(Fix64 lhs, PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(lhs * rhs.Value);
    }

    public static PartialQuantityLarge operator *(PartialQuantityLarge lhs, int rhs)
    {
      return new PartialQuantityLarge(lhs.Value * rhs);
    }

    public static PartialQuantityLarge operator *(int lhs, PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(lhs * rhs.Value);
    }

    public static PartialQuantityLarge operator /(PartialQuantityLarge lhs, Fix64 rhs)
    {
      return new PartialQuantityLarge(lhs.Value / rhs);
    }

    public static PartialQuantityLarge operator /(PartialQuantityLarge lhs, int rhs)
    {
      return new PartialQuantityLarge(lhs.Value / rhs);
    }

    public static void Serialize(PartialQuantityLarge value, BlobWriter writer)
    {
      Fix64.Serialize(value.Value, writer);
    }

    public static PartialQuantityLarge Deserialize(BlobReader reader)
    {
      return new PartialQuantityLarge(Fix64.Deserialize(reader));
    }

    public QuantityLarge IntegerPart => new QuantityLarge(this.Value.IntegerPart);

    public PartialQuantity AsPartial => new PartialQuantity(this.Value.ToFix32());

    public PartialQuantityLarge(Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix64.FromInt(quantity.Value);
    }

    public PartialQuantityLarge(QuantityLarge value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix64.FromLong(value.Value);
    }

    public static bool operator ==(Quantity lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(Quantity lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public static bool operator <(Quantity lhs, PartialQuantityLarge rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Quantity lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value <= rhs.Value;
    }

    public static bool operator >(Quantity lhs, PartialQuantityLarge rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Quantity lhs, PartialQuantityLarge rhs)
    {
      return lhs.Value >= rhs.Value;
    }

    public static PartialQuantityLarge operator +(Quantity lhs, PartialQuantityLarge rhs)
    {
      return new PartialQuantityLarge(lhs.Value + rhs.Value);
    }

    public static PartialQuantityLarge operator +(PartialQuantityLarge lhs, Quantity rhs)
    {
      return new PartialQuantityLarge(lhs.Value + rhs.Value);
    }

    public static PartialQuantityLarge operator -(PartialQuantityLarge lhs, Quantity rhs)
    {
      return new PartialQuantityLarge(lhs.Value - rhs.Value);
    }
  }
}
