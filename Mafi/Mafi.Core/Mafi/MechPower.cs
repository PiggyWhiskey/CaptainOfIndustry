// Decompiled with JetBrains decompiler
// Type: Mafi.MechPower
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
  public readonly struct MechPower : IEquatable<MechPower>, IComparable<MechPower>
  {
    public readonly int Value;

    public static MechPower Zero => new MechPower();

    public static MechPower MinValue => new MechPower(int.MinValue);

    public static MechPower MaxValue => new MechPower(int.MaxValue);

    public MechPower(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public MechPower Abs => new MechPower(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public MechPower Min(MechPower rhs) => new MechPower(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public MechPower Max(MechPower rhs) => new MechPower(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public MechPower Clamp(MechPower min, MechPower max)
    {
      return new MechPower(this.Value.Clamp(min.Value, max.Value));
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
    public MechPower ScaledBy(Percent scale) => new MechPower(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(MechPower other, MechPower tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public MechPower Lerp(MechPower other, int t, int scale)
    {
      return new MechPower(this.Value.Lerp(other.Value, (long) t, (long) scale));
    }

    /// <summary>Returns linearly iterpolated value.</summary>
    [Pure]
    public MechPower Lerp(MechPower other, Percent t)
    {
      return new MechPower(this.Value.Lerp(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public MechPower Average(MechPower other) => new MechPower((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Squared => (long) this.Value * (long) this.Value;

    public override string ToString() => this.Value.ToString();

    public bool Equals(MechPower other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is MechPower other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    public int CompareTo(MechPower other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(MechPower lhs, MechPower rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(MechPower lhs, MechPower rhs) => lhs.Value != rhs.Value;

    public static bool operator <(MechPower lhs, MechPower rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(MechPower lhs, MechPower rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(MechPower lhs, MechPower rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(MechPower lhs, MechPower rhs) => lhs.Value >= rhs.Value;

    public static MechPower operator -(MechPower rhs) => new MechPower(-rhs.Value);

    public static MechPower operator +(MechPower lhs, MechPower rhs)
    {
      return new MechPower(lhs.Value + rhs.Value);
    }

    public static MechPower operator -(MechPower lhs, MechPower rhs)
    {
      return new MechPower(lhs.Value - rhs.Value);
    }

    public static MechPower operator *(MechPower lhs, int rhs) => new MechPower(lhs.Value * rhs);

    public static MechPower operator *(int lhs, MechPower rhs) => new MechPower(lhs * rhs.Value);

    public static MechPower operator /(MechPower lhs, int rhs) => new MechPower(lhs.Value / rhs);

    public static void Serialize(MechPower value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static MechPower Deserialize(BlobReader reader) => new MechPower(reader.ReadInt());

    public static MechPower OneKw => new MechPower(1);

    public static MechPower FromKw(int kw) => new MechPower(kw);

    public static MechPower FromMw(int mw) => new MechPower(mw * 1000);

    public static MechPower FromQuantity(Mafi.Quantity quantity) => new MechPower(quantity.Value);

    public Mafi.Quantity Quantity => new Mafi.Quantity(this.Value);

    public Fix32 ToMwSeconds => this.Value.Over(1000 * Duration.OneSecond.Ticks);

    [Pure]
    public LocStrFormatted Format()
    {
      return ElectricityQuantityFormatter.Instance.FormatNumberAndUnitOnly(new QuantityLarge((long) this.Value), ElectricityQuantityFormatter.GetFormatInfo(new QuantityLarge((long) this.Value)));
    }
  }
}
