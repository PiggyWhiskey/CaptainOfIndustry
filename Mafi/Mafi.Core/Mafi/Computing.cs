// Decompiled with JetBrains decompiler
// Type: Mafi.Computing
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
  /// <summary>Computing power in TFlops.</summary>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct Computing : IEquatable<Computing>, IComparable<Computing>
  {
    public readonly int Value;

    public static Computing FromTFlops(int tflops) => new Computing(tflops);

    public static Computing FromQuantity(Mafi.Quantity quantity) => new Computing(quantity.Value);

    public Mafi.Quantity Quantity => new Mafi.Quantity(this.Value);

    [Pure]
    public LocStrFormatted Format()
    {
      return ComputingQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge((long) this.Value), ComputingQuantityFormatter.GetFormatInfo(new QuantityLarge((long) this.Value)));
    }

    [Pure]
    public LocStrFormatted FormatShort()
    {
      return ComputingQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge((long) this.Value), ComputingQuantityFormatter.GetFormatInfoShort(new QuantityLarge((long) this.Value)));
    }

    [Pure]
    public LocStrFormatted FormatNoUnits()
    {
      return ComputingQuantityFormatter.Instance.FormatNumberOnly(new QuantityLarge((long) this.Value), ComputingQuantityFormatter.GetFormatInfo(new QuantityLarge((long) this.Value)));
    }

    public static Computing Zero => new Computing();

    public static Computing MinValue => new Computing(int.MinValue);

    public static Computing MaxValue => new Computing(int.MaxValue);

    public Computing(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Computing Abs => new Computing(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Computing Min(Computing rhs) => new Computing(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Computing Max(Computing rhs) => new Computing(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Computing Clamp(Computing min, Computing max)
    {
      return new Computing(this.Value.Clamp(min.Value, max.Value));
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
    public Computing ScaledBy(Percent scale) => new Computing(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Computing other, Computing tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Computing Lerp(Computing other, int t, int scale)
    {
      return new Computing(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public Computing Lerp(Computing other, Percent t)
    {
      return new Computing(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Computing Average(Computing other) => new Computing((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(Computing other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Computing other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(Computing other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Computing lhs, Computing rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Computing lhs, Computing rhs) => lhs.Value != rhs.Value;

    public static bool operator <(Computing lhs, Computing rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Computing lhs, Computing rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(Computing lhs, Computing rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Computing lhs, Computing rhs) => lhs.Value >= rhs.Value;

    public static Computing operator -(Computing rhs) => new Computing(-rhs.Value);

    public static Computing operator +(Computing lhs, Computing rhs)
    {
      return new Computing(lhs.Value + rhs.Value);
    }

    public static Computing operator -(Computing lhs, Computing rhs)
    {
      return new Computing(lhs.Value - rhs.Value);
    }

    public static Computing operator *(Computing lhs, int rhs) => new Computing(lhs.Value * rhs);

    public static Computing operator *(int lhs, Computing rhs) => new Computing(lhs * rhs.Value);

    public static Computing operator /(Computing lhs, int rhs) => new Computing(lhs.Value / rhs);

    public static void Serialize(Computing value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static Computing Deserialize(BlobReader reader) => new Computing(reader.ReadInt());
  }
}
