// Decompiled with JetBrains decompiler
// Type: Mafi.Electricity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Localization.Quantity;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct Electricity : IEquatable<Electricity>, IComparable<Electricity>
  {
    public readonly int Value;

    public static Electricity OneKw => new Electricity(1);

    public static Electricity FromKw(int kw) => new Electricity(kw);

    public static Electricity FromMw(int mw) => new Electricity(mw * 1000);

    public static Electricity FromMw(float mw) => new Electricity((mw * 1000f).RoundToInt());

    public static Electricity FromQuantity(Mafi.Quantity quantity)
    {
      return new Electricity(quantity.Value);
    }

    public static Electricity FromQuantity(int quantity) => new Electricity(quantity);

    public Mafi.Quantity Quantity => new Mafi.Quantity(this.Value);

    [Pure]
    public LocStrFormatted Format()
    {
      return ElectricityQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge((long) this.Value), ElectricityQuantityFormatter.GetFormatInfo(new QuantityLarge((long) this.Value)));
    }

    [Pure]
    public LocStrFormatted FormatNoUnits()
    {
      return ElectricityQuantityFormatter.Instance.FormatNumberOnly(new QuantityLarge((long) this.Value), ElectricityQuantityFormatter.GetFormatInfo(new QuantityLarge((long) this.Value)));
    }

    public static Electricity Zero => new Electricity();

    public static Electricity MinValue => new Electricity(int.MinValue);

    public static Electricity MaxValue => new Electricity(int.MaxValue);

    public Electricity(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Electricity Abs => new Electricity(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Electricity Min(Electricity rhs) => new Electricity(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Electricity Max(Electricity rhs) => new Electricity(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Electricity Clamp(Electricity min, Electricity max)
    {
      return new Electricity(this.Value.Clamp(min.Value, max.Value));
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
    public Electricity ScaledBy(Percent scale) => new Electricity(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Electricity other, Electricity tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Electricity Lerp(Electricity other, int t, int scale)
    {
      return new Electricity(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Electricity Lerp(Electricity other, Percent t)
    {
      return new Electricity(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Electricity Average(Electricity other)
    {
      return new Electricity((this.Value + other.Value) / 2);
    }

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(Electricity other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Electricity other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(Electricity other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Electricity lhs, Electricity rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Electricity lhs, Electricity rhs) => lhs.Value != rhs.Value;

    public static bool operator <(Electricity lhs, Electricity rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Electricity lhs, Electricity rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(Electricity lhs, Electricity rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Electricity lhs, Electricity rhs) => lhs.Value >= rhs.Value;

    public static Electricity operator -(Electricity rhs) => new Electricity(-rhs.Value);

    public static Electricity operator +(Electricity lhs, Electricity rhs)
    {
      return new Electricity(lhs.Value + rhs.Value);
    }

    public static Electricity operator -(Electricity lhs, Electricity rhs)
    {
      return new Electricity(lhs.Value - rhs.Value);
    }

    public static Electricity operator *(Electricity lhs, int rhs)
    {
      return new Electricity(lhs.Value * rhs);
    }

    public static Electricity operator *(int lhs, Electricity rhs)
    {
      return new Electricity(lhs * rhs.Value);
    }

    public static Electricity operator /(Electricity lhs, int rhs)
    {
      return new Electricity(lhs.Value / rhs);
    }

    public static void Serialize(Electricity value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static Electricity Deserialize(BlobReader reader) => new Electricity(reader.ReadInt());
  }
}
