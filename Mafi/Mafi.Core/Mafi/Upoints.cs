// Decompiled with JetBrains decompiler
// Type: Mafi.Upoints
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
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct Upoints : IEquatable<Upoints>, IComparable<Upoints>
  {
    public readonly Fix32 Value;
    private static readonly LocStr1Plural UnityStr_Format;

    public static Upoints Zero => new Upoints();

    public static Upoints MinValue => new Upoints(Fix32.MinValue);

    public static Upoints MaxValue => new Upoints(Fix32.MaxValue);

    public static Upoints Epsilon => new Upoints(Fix32.Epsilon);

    public Upoints(Fix32 value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public Upoints(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = Fix32.FromInt(value);
    }

    public static Upoints FromFraction(long numerator, long denominator)
    {
      return new Upoints(Fix32.FromFraction(numerator, denominator));
    }

    /// <summary>Returns absolute value of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Upoints Abs => new Upoints(this.Value.Abs());

    /// <summary>Returns sign of this value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sign => this.Value.Sign();

    /// <summary>Returns minimum of this and given values.</summary>
    [Pure]
    public Upoints Min(Upoints rhs) => new Upoints(this.Value.Min(rhs.Value));

    /// <summary>Returns maximum of this and given values.</summary>
    [Pure]
    public Upoints Max(Upoints rhs) => new Upoints(this.Value.Max(rhs.Value));

    /// <summary>Clamps this value to given minimum and maximum.</summary>
    [Pure]
    public Upoints Clamp(Upoints min, Upoints max)
    {
      return new Upoints(this.Value.Clamp(min.Value, max.Value));
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
    public Upoints ScaledBy(Percent scale) => new Upoints(scale.Apply(this.Value));

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Upoints other) => this.Value.IsNear(other.Value);

    /// <summary>
    /// Whether this and given values are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Upoints other, Upoints tolerance)
    {
      return this.Value.IsNear(other.Value, tolerance.Value);
    }

    [Pure]
    public Upoints Lerp(Upoints other, Percent t) => new Upoints(this.Value.Lerp(other.Value, t));

    [Pure]
    public Upoints Lerp(Upoints other, Fix32 t) => new Upoints(this.Value.Lerp(other.Value, t));

    [Pure]
    public Upoints Lerp(Upoints other, Fix32 t, Fix32 scale)
    {
      return new Upoints(this.Value.Lerp(other.Value, t, scale));
    }

    [Pure]
    public Upoints SmoothStep(Upoints other, Fix32 t)
    {
      return new Upoints(this.Value.SmoothStep(other.Value, t));
    }

    [Pure]
    public Upoints EaseIn(Upoints other, Fix32 t) => new Upoints(this.Value.EaseIn(other.Value, t));

    [Pure]
    public Upoints EaseOut(Upoints other, Fix32 t)
    {
      return new Upoints(this.Value.EaseOut(other.Value, t));
    }

    [Pure]
    public Upoints EaseInOut(Upoints other, Fix32 t)
    {
      return new Upoints(this.Value.EaseInOut(other.Value, t));
    }

    /// <summary>Returns average of this and given value.</summary>
    [Pure]
    public Upoints Average(Upoints other) => new Upoints((this.Value + other.Value) / 2);

    /// <summary>Returns squared value.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Squared => this.Value.MultAsFix64(this.Value);

    public override string ToString() => this.Value.ToString();

    public bool Equals(Upoints other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is Upoints other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(Upoints other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(Upoints lhs, Upoints rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(Upoints lhs, Upoints rhs) => lhs.Value != rhs.Value;

    public static bool operator <(Upoints lhs, Upoints rhs) => lhs.Value < rhs.Value;

    public static bool operator <=(Upoints lhs, Upoints rhs) => lhs.Value <= rhs.Value;

    public static bool operator >(Upoints lhs, Upoints rhs) => lhs.Value > rhs.Value;

    public static bool operator >=(Upoints lhs, Upoints rhs) => lhs.Value >= rhs.Value;

    public static Upoints operator -(Upoints rhs) => new Upoints(-rhs.Value);

    public static Upoints operator +(Upoints lhs, Upoints rhs)
    {
      return new Upoints(lhs.Value + rhs.Value);
    }

    public static Upoints operator -(Upoints lhs, Upoints rhs)
    {
      return new Upoints(lhs.Value - rhs.Value);
    }

    public static Upoints operator *(Upoints lhs, Fix32 rhs) => new Upoints(lhs.Value * rhs);

    public static Upoints operator *(Fix32 lhs, Upoints rhs) => new Upoints(lhs * rhs.Value);

    public static Upoints operator *(Upoints lhs, int rhs) => new Upoints(lhs.Value * rhs);

    public static Upoints operator *(int lhs, Upoints rhs) => new Upoints(lhs * rhs.Value);

    public static Upoints operator /(Upoints lhs, Fix32 rhs) => new Upoints(lhs.Value / rhs);

    public static Upoints operator /(Upoints lhs, int rhs) => new Upoints(lhs.Value / rhs);

    public static void Serialize(Upoints value, BlobWriter writer)
    {
      Fix32.Serialize(value.Value, writer);
    }

    public static Upoints Deserialize(BlobReader reader) => new Upoints(Fix32.Deserialize(reader));

    public int GetCanonicalRoundedValue() => this.Value.ToIntRounded();

    [Pure]
    public string Format() => QuantityFormatter.FormatNumber(this.Value.ToFix64());

    [Pure]
    public string Format1Dec() => this.Value.ToStringRounded(1);

    [Pure]
    public LocStrFormatted FormatWithUnitySuffix()
    {
      return Upoints.UnityStr_Format.Format(QuantityFormatter.FormatNumber(this.Value.ToFix64()), this.Value.ToFix64());
    }

    [Pure]
    public string FormatForceDigits()
    {
      return !(this.Value.Abs() < Fix32.One) ? this.Value.ToStringRoundedAdaptive(3) : this.Value.ToStringRounded();
    }

    [Pure]
    public Mafi.Quantity GetQuantityRounded() => new Mafi.Quantity(this.Value.RawValue);

    static Upoints()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Upoints.UnityStr_Format = Loc.Str1Plural(nameof (UnityStr_Format), "{0} Unity", "{0} Unity", "virtual & abstract 'currency' expressing how people 'pull together' to achieve higher goals, example: '1 Unity'");
    }
  }
}
