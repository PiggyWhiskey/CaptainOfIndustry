// Decompiled with JetBrains decompiler
// Type: Mafi.Fix64
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Fixed-point 64-bit number that has 20 fractional bits. Hence, the lowest representable value is 2^-10 ~=
  /// 0.000001 and the largest value is 2^43 ~= 8e12.
  /// </summary>
  [DebuggerDisplay("{DebugString,nq}")]
  [GenerateSerializer(false, null, 0)]
  public readonly struct Fix64 : IEquatable<Fix64>, IComparable<Fix64>
  {
    /// <summary>Number of fractional bits.</summary>
    internal const int FRACTIONAL_BITS = 20;
    internal const long FRACTION_RANGE = 1048576;
    internal const long FRACTION_MASK = 1048575;
    public const int MAX_DIGITS_PRECISION = 6;
    internal const long ONE_RAW = 1048576;
    /// <summary>
    /// This is the only correct epsilon to use to check whether an original value is near 1 by checking its square.
    /// </summary>
    public static readonly Fix64 EpsilonFix32NearOneSqr;
    public static readonly Fix64 Tau;
    public static readonly Fix64 TauOver4;
    public static readonly Fix64 Sqrt2;
    /// <summary>1/√2 = cos(45 deg) = sin(45 deg) = 0.707....</summary>
    public static readonly Fix64 OneOverSqrt2;
    /// <summary>The actual stored value.</summary>
    public readonly long RawValue;

    public static void Serialize(Fix64 value, BlobWriter writer)
    {
      writer.WriteLong(value.RawValue);
    }

    public static Fix64 Deserialize(BlobReader reader) => new Fix64(reader.ReadLong());

    public static Fix64 Zero => new Fix64(0L);

    public static Fix64 Half => new Fix64(524288L);

    public static Fix64 One => new Fix64(1048576L);

    public static Fix64 Two => new Fix64(2097152L);

    public static Fix64 Three => new Fix64(3145728L);

    public static Fix64 Four => new Fix64(4194304L);

    public static Fix64 Eight => new Fix64(8388608L);

    /// <summary>
    /// The minimal distance between any two consecutive Fix64 numbers. Approximately equal to 0.00000095.
    /// </summary>
    public static Fix64 Epsilon => new Fix64(1L);

    /// <summary>
    /// Epsilon used for "near" checks. Approximately equal to 0.000015.
    /// </summary>
    public static Fix64 EpsilonNear => new Fix64(16L);

    /// <summary>Smallest representable value.</summary>
    public static Fix64 MinValue => new Fix64(long.MinValue);

    /// <summary>Largest representable value.</summary>
    public static Fix64 MaxValue => new Fix64(long.MaxValue);

    /// <summary>Smallest representable integer (with no fraction).</summary>
    public static Fix64 MinIntValue => new Fix64(long.MinValue);

    /// <summary>Largest representable integer (with no fraction).</summary>
    public static Fix64 MaxIntValue => new Fix64(9223372036853727232L);

    [LoadCtor]
    private Fix64(long rawValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.RawValue = rawValue;
    }

    public static Fix64 FromRaw(long rawValue) => new Fix64(rawValue);

    public static Fix64 FromInt(int value) => new Fix64((long) value << 20);

    public static Fix64 FromLong(long value)
    {
      Assert.That<long>(value).IsWithinIncl<long>(-8796093022208L, 8796093022207L, "Overflow while creating Fix64 from {0}.", value);
      return new Fix64(value << 20);
    }

    public static Fix64 FromFix32(Fix32 fix32) => new Fix64((long) fix32.RawValue << 10);

    public static bool TryCreateFromDouble(double value, out Fix64 result)
    {
      result = new Fix64();
      if (double.IsNaN(value))
        return false;
      value = Math.Round(value * 1048576.0);
      if (value < (double) long.MinValue || value > (double) long.MaxValue)
        return false;
      result = new Fix64((long) value);
      return true;
    }

    public static Fix64 FromDouble(double value)
    {
      if (double.IsNaN(value))
      {
        Log.Error("Error while creating Fix64 from double NaN. Returning 0.");
        return new Fix64();
      }
      value = Math.Round(value * 1048576.0);
      if (value < (double) long.MinValue)
      {
        Log.Error(string.Format("Overflow while creating Fix64 from double {0}. Returning {1}.", (object) value, (object) Fix64.MinValue));
        return Fix64.MinValue;
      }
      if (value <= (double) long.MaxValue)
        return new Fix64((long) value);
      Log.Error(string.Format("Overflow while creating Fix64 from double {0}. Returning {1}.", (object) value, (object) Fix64.MaxValue));
      return Fix64.MaxValue;
    }

    public static Fix64 FromFraction(long numerator, long denominator)
    {
      if (denominator != 0L)
        return new Fix64((numerator << 20) / denominator);
      Log.Error("Creating Fix64 from fraction with zero denominator.");
      return Fix64.Zero;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.RawValue == 0L;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.RawValue != 0L;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.RawValue > 0L;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.RawValue <= 0L;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.RawValue < 0L;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.RawValue >= 0L;

    /// <summary>
    /// Whether this number is integer and has no fractional part.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsInteger => (this.RawValue & 1048575L) == 0L;

    /// <summary>
    /// Returns integer part of this value discarding any fraction. Invariant: x == x.IntegerPart +
    /// x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long IntegerPart
    {
      get => (this.RawValue > 0L ? this.RawValue : this.RawValue + 1048575L) >> 20;
    }

    /// <summary>
    /// Fractional part. Invariant: x == x.IntegerPart + x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 FractionalPart
    {
      get
      {
        return new Fix64(this.RawValue - ((this.RawValue > 0L ? this.RawValue : this.RawValue + 1048575L) & -1048576L));
      }
    }

    /// <summary>
    /// Returns value divided by two very efficiently (uses a bit shift). Use with caution, this works for all values
    /// except for <see cref="P:Mafi.Fix64.MinValue" /> (the smallest representable value).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 HalfFast => new Fix64(this.RawValue >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Times2Fast => new Fix64(this.RawValue << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Times4Fast => new Fix64(this.RawValue << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Times8Fast => new Fix64(this.RawValue << 3);

    [Pure]
    public int ToIntRounded() => (int) this.ToLongRounded();

    [OnlyForSaveCompatibility(null)]
    [Pure]
    public long ToLongRounded() => this.RawValue + 524288L + (this.RawValue >= 0L ? 0L : -1L) >> 20;

    [Pure]
    public long ToLongRoundedPositive() => this.RawValue + 524288L >> 20;

    [Pure]
    public int ToIntFloored() => (int) (this.RawValue >> 20);

    [Pure]
    public long ToLongFloored() => this.RawValue >> 20;

    [Pure]
    public int ToIntCeiled() => (int) (this.RawValue + 1048575L >> 20);

    [Pure]
    public long ToLongCeiled() => this.RawValue + 1048575L >> 20;

    [Pure]
    public Percent ToPercent() => Percent.FromFix64(this);

    [Pure]
    public Fix32 ToFix32()
    {
      long rawValue = (this.RawValue + (this.RawValue >= 0L ? 512L : -512L)) / 1024L;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while creating Fix64 {0} to Fix32. Returning {1}.", (object) this, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return Fix32.FromRaw((int) rawValue);
      Log.Error(string.Format("Overflow while creating Fix64 {0} to Fix32. Returning {1}.", (object) this, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    [Pure]
    public float ToFloat() => (float) this.RawValue / 1048576f;

    [Pure]
    public double ToDouble() => (double) this.RawValue / 1048576.0;

    [Pure]
    public bool Equals(Fix64 other) => this.RawValue == other.RawValue;

    [Pure]
    public int CompareTo(Fix64 other) => this.RawValue.CompareTo(other.RawValue);

    [Pure]
    public override bool Equals(object obj) => obj is Fix64 fix64 && fix64 == this;

    [Pure]
    public override int GetHashCode()
    {
      return Hash.Combine<long, long>(this.RawValue >> 32, this.RawValue & (long) uint.MaxValue);
    }

    [Pure]
    public string ToStringRounded(int decimalDigits = 2)
    {
      Assert.That<int>(decimalDigits).IsLessOrEqual(8, "Fix64 has no more than 8 digits of precision.");
      return decimalDigits <= 0 ? this.ToIntRounded().ToString() : this.ToDouble().ToStringDecDigits(decimalDigits);
    }

    [Pure]
    public string ToStringFraction()
    {
      return string.Format("{0} {1}/{2}", (object) this.IntegerPart, (object) this.FractionalPart.RawValue, (object) 1048576L);
    }

    [Pure]
    public string ToStringFractionReduced()
    {
      Fix64 fractionalPart = this.FractionalPart;
      long num = MafiMath.Gcd(this.FractionalPart.Abs().RawValue, 1048576L);
      return string.Format("{0} {1} {2}/{3}", (object) this.IntegerPart, fractionalPart.IsNegative ? (object) "-" : (object) "+", (object) (this.FractionalPart.Abs().RawValue / num), (object) (1048576L / num));
    }

    [Pure]
    public override string ToString()
    {
      return this.ToDouble().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugString
    {
      get => this.ToDouble().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    [Pure]
    public Fix64 Abs() => new Fix64(this.RawValue.Abs());

    [Pure]
    public Fix64 Floored() => new Fix64(this.RawValue & -1048576L);

    [Pure]
    public int Sign()
    {
      if (this.RawValue > 0L)
        return 1;
      return this.RawValue != 0L ? -1 : 0;
    }

    [Pure]
    public Fix64 Squared() => this * this;

    [Pure]
    public Fix64 Sqrt() => Fix64.FromDouble(Math.Sqrt(this.ToDouble()));

    [Pure]
    public Fix32 SqrtToFix32() => Fix32.FromDouble(Math.Sqrt(this.ToDouble()));

    [Pure]
    public Fix64 LogNatural() => Fix64.FromDouble(Math.Log(this.ToDouble()));

    [Pure]
    public Fix64 Exp() => Fix64.FromDouble(Math.Exp(this.ToDouble()));

    [Pure]
    public Fix64 ExpClamped(double maxValue)
    {
      return Fix64.FromDouble(Math.Min(maxValue, Math.Exp(this.ToDouble())));
    }

    [Pure]
    public Fix64 Cos() => Fix64.FromDouble(Math.Cos(this.ToDouble()));

    [Pure]
    public Fix64 Sin() => Fix64.FromDouble(Math.Sin(this.ToDouble()));

    [Pure]
    public Fix64 Tan() => Fix64.FromDouble(Math.Tan(this.ToDouble()));

    [Pure]
    public Fix64 Sinh() => Fix64.FromDouble(Math.Sinh(this.ToDouble()));

    [Pure]
    public Fix64 Cosh() => Fix64.FromDouble(Math.Cosh(this.ToDouble()));

    [Pure]
    public Fix64 Tanh() => Fix64.FromDouble(Math.Tanh(this.ToDouble()));

    [Pure]
    public Fix64 Coth() => this.Cosh() / this.Sinh();

    [Pure]
    public Fix64 Pow(Fix64 exponent)
    {
      return Fix64.FromDouble(Math.Pow(this.ToDouble(), exponent.ToDouble()));
    }

    [Pure]
    public Fix32 DivToFix32(int value) => (this / value).ToFix32();

    [Pure]
    public Fix32 DivToFix32(Fix32 fix32) => (this / fix32).ToFix32();

    [Pure]
    public Fix32 DivToFix32(Fix64 fix64) => (this / fix64).ToFix32();

    [Pure]
    public Fix64 Min(Fix64 other) => !(this <= other) ? other : this;

    [Pure]
    public Fix64 Max(Fix64 other) => !(this >= other) ? other : this;

    [Pure]
    public Fix64 Clamp(Fix64 min, Fix64 max)
    {
      if (this <= min)
        return min;
      return !(this >= max) ? this : max;
    }

    [Pure]
    public Fix64 Clamp01() => new Fix64(this.RawValue.Clamp(0L, 1048576L));

    [Pure]
    public bool IsNear(Fix64 other)
    {
      Fix64 fix64 = this - other;
      return fix64 >= -Fix64.EpsilonNear && fix64 <= Fix64.EpsilonNear;
    }

    [Pure]
    public bool IsNear(Fix64 other, Fix64 tolerance)
    {
      Fix64 fix64 = this - other;
      return fix64 >= -tolerance && fix64 <= tolerance;
    }

    [Pure]
    public bool IsNearZero() => this >= -Fix64.EpsilonNear && this <= Fix64.EpsilonNear;

    [Pure]
    public Fix64 ScaledBy(Percent p) => p.Apply(this);

    [Pure]
    public Fix64 Lerp(Fix64 to, Fix64 t) => this + (to - this) * t;

    [Pure]
    public Fix64 Lerp(Fix64 to, Percent t) => this + t.Apply(to - this);

    [Pure]
    public Fix64 Lerp(Fix64 to, Fix64 t, Fix64 scale) => this + (to - this) * t / scale;

    public static implicit operator Fix64(long i) => Fix64.FromLong(i);

    public static bool operator ==(Fix64 lhs, Fix64 rhs) => lhs.RawValue == rhs.RawValue;

    public static bool operator !=(Fix64 lhs, Fix64 rhs) => lhs.RawValue != rhs.RawValue;

    public static bool operator >(Fix64 lhs, Fix64 rhs) => lhs.RawValue > rhs.RawValue;

    public static bool operator >=(Fix64 lhs, Fix64 rhs) => lhs.RawValue >= rhs.RawValue;

    public static bool operator <(Fix64 lhs, Fix64 rhs) => lhs.RawValue < rhs.RawValue;

    public static bool operator <=(Fix64 lhs, Fix64 rhs) => lhs.RawValue <= rhs.RawValue;

    public static bool operator ==(Fix64 lhs, long rhs) => lhs == rhs.ToFix64();

    public static bool operator ==(long lhs, Fix64 rhs) => lhs.ToFix64() == rhs;

    public static bool operator !=(Fix64 lhs, long rhs) => lhs != rhs.ToFix64();

    public static bool operator !=(long lhs, Fix64 rhs) => lhs.ToFix64() != rhs;

    public static bool operator >(Fix64 lhs, long rhs) => lhs > rhs.ToFix64();

    public static bool operator >(long lhs, Fix64 rhs) => lhs.ToFix64() > rhs;

    public static bool operator >=(Fix64 lhs, long rhs) => lhs >= rhs.ToFix64();

    public static bool operator >=(long lhs, Fix64 rhs) => lhs.ToFix64() >= rhs;

    public static bool operator <(Fix64 lhs, long rhs) => lhs < rhs.ToFix64();

    public static bool operator <(long lhs, Fix64 rhs) => lhs.ToFix64() < rhs;

    public static bool operator <=(Fix64 lhs, long rhs) => lhs <= rhs.ToFix64();

    public static bool operator <=(long lhs, Fix64 rhs) => lhs.ToFix64() <= rhs;

    public static bool operator ==(Fix64 lhs, int rhs) => lhs == rhs.ToFix64();

    public static bool operator ==(int lhs, Fix64 rhs) => lhs.ToFix64() == rhs;

    public static bool operator !=(Fix64 lhs, int rhs) => lhs != rhs.ToFix64();

    public static bool operator !=(int lhs, Fix64 rhs) => lhs.ToFix64() != rhs;

    public static bool operator >(Fix64 lhs, int rhs) => lhs > rhs.ToFix64();

    public static bool operator >(int lhs, Fix64 rhs) => lhs.ToFix64() > rhs;

    public static bool operator >=(Fix64 lhs, int rhs) => lhs >= rhs.ToFix64();

    public static bool operator >=(int lhs, Fix64 rhs) => lhs.ToFix64() >= rhs;

    public static bool operator <(Fix64 lhs, int rhs) => lhs < rhs.ToFix64();

    public static bool operator <(int lhs, Fix64 rhs) => lhs.ToFix64() < rhs;

    public static bool operator <=(Fix64 lhs, int rhs) => lhs <= rhs.ToFix64();

    public static bool operator <=(int lhs, Fix64 rhs) => lhs.ToFix64() <= rhs;

    public static Fix64 operator +(Fix64 value) => value;

    public static Fix64 operator -(Fix64 value) => new Fix64(-value.RawValue);

    public static Fix64 operator +(Fix64 lhs, Fix64 rhs) => new Fix64(lhs.RawValue + rhs.RawValue);

    public static Fix64 operator -(Fix64 lhs, Fix64 rhs) => new Fix64(lhs.RawValue - rhs.RawValue);

    public static Fix64 operator *(Fix64 lhs, Fix64 rhs)
    {
      long num = rhs.RawValue ^ lhs.RawValue;
      ulong resultHi;
      ulong resultLo;
      MafiMath.Mul64To128((ulong) lhs.RawValue.Abs(), (ulong) rhs.RawValue.Abs(), out resultHi, out resultLo);
      if (resultHi >= 524288UL)
      {
        Log.Error(string.Format("Fix64 mult overflow: {0} * {1}", (object) lhs, (object) rhs));
        return lhs.Sign() * rhs.Sign() < 0 ? Fix64.MinValue : Fix64.MaxValue;
      }
      long rawValue = (long) resultHi << 44 | (long) (resultLo >> 20);
      return num >= 0L ? new Fix64(rawValue) : new Fix64(-rawValue);
    }

    /// <summary>Will overflow if the result has more than 40 bits.</summary>
    [Pure]
    public Fix64 MultByUnchecked(Fix64 rhs) => new Fix64(this.RawValue * rhs.RawValue >> 20);

    /// <summary>Will overflow if the result has more than 50 bits.</summary>
    [Pure]
    public Fix64 MultByUnchecked(Fix32 rhs) => new Fix64(this.RawValue * (long) rhs.RawValue >> 10);

    [Pure]
    public Fix64 MultByUnchecked(int rhs) => new Fix64(this.RawValue * (long) rhs);

    public static Fix64 operator /(Fix64 lhs, Fix64 rhs)
    {
      if (rhs.IsZero)
      {
        Log.Error("Fix64 div by zero.");
        return Fix64.Zero;
      }
      if (lhs.IsZero)
        return Fix64.Zero;
      long num1 = rhs.RawValue ^ lhs.RawValue;
      ulong num2 = (ulong) lhs.RawValue.Abs();
      ulong dividendLo = num2 << 20;
      ulong result;
      if (!MafiMath.TryDiv128To64(num2 >> 44, dividendLo, (ulong) rhs.RawValue.Abs(), out result) || ((long) result & long.MinValue) != 0L)
      {
        Log.Error("Fix64 div overflow.");
        return lhs.Sign() * rhs.Sign() < 0 ? Fix64.MinValue : Fix64.MaxValue;
      }
      long rawValue = (long) result;
      return num1 >= 0L ? new Fix64(rawValue) : new Fix64(-rawValue);
    }

    /// <summary>
    /// Fast division with no checks. Will overflow for values greater than ~8 billion.
    /// </summary>
    [Pure]
    public Fix64 DivByPositiveUncheckedUnrounded(Fix32 rhs)
    {
      return new Fix64((this.RawValue << 10) / (long) rhs.RawValue);
    }

    /// <summary>
    /// Fast division with no checks. Will overflow for values greater than ~8 million.
    /// </summary>
    [Pure]
    public Fix64 DivByPositiveUncheckedUnrounded(Fix64 rhs)
    {
      return new Fix64((this.RawValue << 20) / rhs.RawValue);
    }

    public static Fix64 operator +(Fix64 lhs, int rhs) => lhs + Fix64.FromInt(rhs);

    public static Fix64 operator +(int lhs, Fix64 rhs) => Fix64.FromInt(lhs) + rhs;

    public static Fix64 operator -(Fix64 lhs, int rhs) => lhs - Fix64.FromInt(rhs);

    public static Fix64 operator -(int lhs, Fix64 rhs) => Fix64.FromInt(lhs) - rhs;

    public static Fix64 operator *(Fix64 lhs, int rhs) => new Fix64(lhs.RawValue * (long) rhs);

    public static Fix64 operator *(int lhs, Fix64 rhs) => rhs * lhs;

    public static Fix64 operator /(Fix64 lhs, int rhs)
    {
      if (rhs != 0)
        return new Fix64(lhs.RawValue / (long) rhs);
      Log.Error("Fix64 div by zero.");
      return Fix64.Zero;
    }

    public static Fix64 operator /(int lhs, Fix64 rhs) => Fix64.FromInt(lhs) / rhs;

    public static Fix64 operator +(Fix64 lhs, Fix32 rhs) => lhs + rhs.ToFix64();

    public static Fix64 operator +(Fix32 lhs, Fix64 rhs) => lhs.ToFix64() + rhs;

    public static Fix64 operator -(Fix64 lhs, Fix32 rhs) => lhs - rhs.ToFix64();

    public static Fix64 operator -(Fix32 lhs, Fix64 rhs) => lhs.ToFix64() - rhs;

    public static Fix64 operator *(Fix64 lhs, Fix32 rhs) => lhs * rhs.ToFix64();

    public static Fix64 operator *(Fix32 lhs, Fix64 rhs) => lhs.ToFix64() * rhs;

    public static Fix64 operator /(Fix64 lhs, Fix32 rhs) => lhs / rhs.ToFix64();

    public static Fix64 operator /(Fix32 lhs, Fix64 rhs) => lhs.ToFix64() / rhs;

    static Fix64()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Fix64.EpsilonFix32NearOneSqr = ((Fix32) 1 + Fix32.EpsilonNear).Squared() - 1;
      Fix64.Tau = Fix64.FromDouble(2.0 * Math.PI);
      Fix64.TauOver4 = Fix64.FromDouble(Math.PI / 2.0);
      Fix64.Sqrt2 = Fix64.FromDouble(1.4142135623730951);
      Fix64.OneOverSqrt2 = Fix64.FromDouble(0.70710678118654757);
    }
  }
}
