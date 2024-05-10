// Decompiled with JetBrains decompiler
// Type: Mafi.Fix32
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
  /// Fixed-point 32-bit number that has 10 fractional bits. Hence, the lowest representable value is 2^-10 ~= 0.001
  /// and the largest value is 2^21 ~= 2M.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("{DebugString,nq}")]
  public readonly struct Fix32 : 
    IEquatable<Fix32>,
    IComparable<Fix32>,
    IEquatable<int>,
    IComparable<int>
  {
    /// <summary>
    /// Number of fractional bits. Cannot be less than 8 and must be a multiply of two.
    /// </summary>
    public const int FRACTIONAL_BITS = 10;
    public const int FRACTION_RANGE = 1024;
    internal const int FRACTION_MASK = 1023;
    public const int MAX_DIGITS_PRECISION = 4;
    internal const int ONE_RAW = 1024;
    public static readonly Fix32 Zero;
    public static readonly Fix32 One;
    public static readonly Fix32 Quarter;
    public static readonly Fix32 Half;
    public static readonly Fix32 Two;
    public static readonly Fix32 Three;
    public static readonly Fix32 Four;
    public static readonly Fix32 Eight;
    /// <summary>
    /// The minimal distance between any two consecutive Fix32 numbers. This is equal to 2^-10 = 1/1024 ~ 0.00097.
    /// </summary>
    public static readonly Fix32 Epsilon;
    /// <summary>
    /// Epsilon used for "near" checks. Approximately equal to 0.0078.
    /// </summary>
    public static readonly Fix32 EpsilonNear;
    /// <summary>Smallest representable value.</summary>
    public static readonly Fix32 MinValue;
    /// <summary>Largest representable value.</summary>
    public static readonly Fix32 MaxValue;
    /// <summary>Smallest representable integer (with no fraction).</summary>
    public static readonly Fix32 MinIntValue;
    /// <summary>Largest representable integer (with no fraction).</summary>
    public static readonly Fix32 MaxIntValue;
    public static readonly Fix32 Tau;
    public static readonly Fix32 Sqrt2;
    public static readonly Fix32 Sqrt3;
    public static readonly Fix32 Sqrt5;
    /// <summary>1/√2 = cos(45 deg) = sin(45 deg) = 0.707....</summary>
    public static readonly Fix32 OneOverSqrt2;
    public static readonly Fix32 OneOverSqrt3;
    public static readonly Fix32 OneOverSqrt5;
    /// <summary>The actual stored value.</summary>
    public readonly int RawValue;

    public static void Serialize(Fix32 value, BlobWriter writer) => writer.WriteInt(value.RawValue);

    public static Fix32 Deserialize(BlobReader reader) => new Fix32(reader.ReadInt());

    [LoadCtor]
    private Fix32(int rawValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.RawValue = rawValue;
    }

    public static Fix32 FromRaw(int rawValue) => new Fix32(rawValue);

    public static Fix32 FromInt(int value)
    {
      if (value < -2097152)
      {
        Mafi.Log.Error(string.Format("Overflow while creating Fix32 from int {0}. Returning {1}.", (object) value, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (value <= 2097151)
        return new Fix32(value << 10);
      Mafi.Log.Error(string.Format("Overflow while creating Fix32 from int {0}. Returning {1}.", (object) value, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    public static Fix32 FromInt(ushort value) => new Fix32((int) value << 10);

    public static Fix32 FromInt(short value) => new Fix32((int) value << 10);

    public static Fix32 FromInt(byte value) => new Fix32((int) value << 10);

    public static Fix32 FromInt(sbyte value) => new Fix32((int) value << 10);

    /// <summary>
    /// This is faster than <see cref="M:Mafi.Fix32.FromInt(System.Int32)" /> but can be used only when the int surely fits Fix32 range.
    /// </summary>
    public static Fix32 FromIntUnclamped(int value) => new Fix32(value << 10);

    /// <summary>
    /// Creates Fix32 from a fraction rounding to the closest valid Fix32 value (towards zero).
    /// </summary>
    /// <remarks>
    /// Arguments are of type <c>long</c> since we need this type internally, but the result of the division must be
    /// in the Fix32 range.
    /// </remarks>
    public static Fix32 FromFraction(long numerator, long denominator)
    {
      if (denominator == 0L)
      {
        Mafi.Log.Error("Creating Fix32 from fraction with zero denominator.");
        return Fix32.Zero;
      }
      if (denominator < 0L)
      {
        numerator = -numerator;
        denominator = -denominator;
      }
      long num = numerator << 10;
      long rawValue = (num + (num > 0L ? denominator / 2L : -denominator / 2L)) / denominator;
      if (rawValue < (long) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while creating Fix32 from fraction {0}/{1}. Returning {2}.", (object) numerator, (object) denominator, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Fix32((int) rawValue);
      Mafi.Log.Error(string.Format("Overflow while creating Fix32 from fraction {0}/{1}. Returning {2}.", (object) numerator, (object) denominator, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    /// <summary>
    /// Created Fix32 from an integer and fraction. Keep in mind that this will be only precise when the denominator
    /// is a power of two less or equal to 1024. Otherwise, rounding towards zero will occur. Some arguments are of
    /// type <c>long</c> since we need this type internally, but the result of the division must be in the Fix32
    /// range.
    /// </summary>
    public static Fix32 FromIntAndFraction(int integer, long numerator, long denominator)
    {
      numerator += (long) integer * denominator;
      return Fix32.FromFraction(numerator, denominator);
    }

    /// <summary>
    /// Creates Fix32 from a double value by rounding to the nearest fraction of 1/1024.
    /// </summary>
    public static Fix32 FromFloat(float value)
    {
      if (float.IsNaN(value))
      {
        Mafi.Log.Error("Error while creating Fix32 from float NaN. Returning 0.");
        return new Fix32();
      }
      value = (float) Math.Round((double) (value * 1024f));
      if ((double) value < (double) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while creating Fix32 from float {0}. Returning {1}.", (object) value, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if ((double) value <= 2147483648.0)
        return new Fix32((int) value);
      Mafi.Log.Error(string.Format("Overflow while creating Fix32 from float {0}. Returning {1}.", (object) value, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    /// <summary>
    /// Faster than <see cref="M:Mafi.Fix32.FromFloat(System.Single)" /> but does not guard against any special cases or overflows.
    /// Use only when certain that the given value is in range of <see cref="T:Mafi.Fix32" />.
    /// </summary>
    public static Fix32 FromFloatNoChecks(float value)
    {
      return new Fix32((int) Math.Round((double) (value * 1024f)));
    }

    public static bool TryCreateFromDouble(double value, out Fix32 result)
    {
      result = new Fix32();
      if (double.IsNaN(value))
        return false;
      value = Math.Round(value * 1024.0);
      if (value < (double) int.MinValue || value > (double) int.MaxValue)
        return false;
      result = new Fix32((int) value);
      return true;
    }

    public static Fix32 FromDouble(double value)
    {
      if (double.IsNaN(value))
      {
        Mafi.Log.Error("Error while creating Fix32 from double NaN. Returning 0.");
        return new Fix32();
      }
      value = Math.Round(value * 1024.0);
      if (value < (double) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while creating Fix32 from double {0}. Returning {1}.", (object) value, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (value <= (double) int.MaxValue)
        return new Fix32((int) value);
      Mafi.Log.Error(string.Format("Overflow while creating Fix32 from double {0}. Returning {1}.", (object) value, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    public static Fix32 FromDoubleCeiled(double value)
    {
      if (double.IsNaN(value))
      {
        Mafi.Log.Error("Error while creating Fix32 from double NaN. Returning 0.");
        return new Fix32();
      }
      value = Math.Ceiling(value * 1024.0);
      if (value < (double) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while creating Fix32 from double {0}. Returning {1}.", (object) value, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (value <= (double) int.MaxValue)
        return new Fix32((int) value);
      Mafi.Log.Error(string.Format("Overflow while creating Fix32 from double {0}. Returning {1}.", (object) value, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.RawValue == 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.RawValue != 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.RawValue > 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.RawValue <= 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.RawValue < 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.RawValue >= 0;

    /// <summary>
    /// Whether this number is integer and has no fractional part.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsInteger => (this.RawValue & 1023) == 0;

    /// <summary>
    /// Returns integer part of this value discarding any fraction. Invariant: x == x.IntegerPart +
    /// x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int IntegerPart => (this.RawValue >= 0 ? this.RawValue : this.RawValue + 1023) >> 10;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int IntegerPartNonNegative => this.RawValue >> 10;

    /// <summary>
    /// Fractional part. Invariant: x == x.IntegerPart + x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 FractionalPart
    {
      get
      {
        return new Fix32(this.RawValue - ((this.RawValue >= 0 ? this.RawValue : this.RawValue + 1023) & -1024));
      }
    }

    /// <summary>
    /// More efficient than <see cref="P:Mafi.Fix32.FractionalPart" /> but works only for non-negative numbers.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 FractionalPartNonNegative => new Fix32(this.RawValue & 1023);

    /// <summary>
    /// Returns value divided by two very efficiently (uses a bit shift). Use with caution, this works for all values
    /// except for <see cref="F:Mafi.Fix32.MinValue" /> (the smallest representable value).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 HalfFast => new Fix32(this.RawValue >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 DivBy4Fast => new Fix32(this.RawValue >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 DivBy8Fast => new Fix32(this.RawValue >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 DivBy16Fast => new Fix32(this.RawValue >> 4);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 DivBy32Fast => new Fix32(this.RawValue >> 5);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 DivBy64Fast => new Fix32(this.RawValue >> 6);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Times2Fast => new Fix32(this.RawValue << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Times4Fast => new Fix32(this.RawValue << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Times5Fast => new Fix32(this.RawValue + (this.RawValue << 2));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Times8Fast => new Fix32(this.RawValue << 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Times12Fast => new Fix32((this.RawValue << 3) + (this.RawValue << 2));

    [Pure]
    public Fix32 Rounded()
    {
      return new Fix32((int) ((long) this.RawValue + 512L + (this.RawValue >= 0 ? 0L : -1L) & -1024L));
    }

    [Pure]
    [OnlyForSaveCompatibility(null)]
    public int ToIntRounded()
    {
      return (int) ((long) this.RawValue + 512L + (this.RawValue >= 0 ? 0L : -1L) >> 10);
    }

    [Pure]
    public int ToIntRoundedNonNegative() => (int) ((long) this.RawValue + 512L >> 10);

    /// <summary>
    /// Rounds up with the probability equal to fractional part.
    /// </summary>
    public int ToIntRoundedProbabilistically(IRandom rng) => rng.RoundProbabilistically(this);

    /// <summary>
    /// Returns floored int. PERF: This is the cheapest rounding.
    /// </summary>
    [Pure]
    public int ToIntFloored() => this.RawValue >> 10;

    [Pure]
    public Fix32 Floored() => new Fix32(this.RawValue & -1024);

    [Pure]
    public int ToIntCeiled() => this.RawValue + 1023 >> 10;

    [Pure]
    public float ToFloat() => (float) this.RawValue / 1024f;

    [Pure]
    public double ToDouble() => (double) this.RawValue / 1024.0;

    [Pure]
    public Fix64 ToFix64() => Fix64.FromFix32(this);

    /// <summary>
    /// Converts given Fix32 to percent. Value 1.0 becomes 100%.
    /// </summary>
    [Pure]
    public Percent ToPercent() => Percent.FromFix32(this);

    [Pure]
    public bool Equals(Fix32 other) => this.RawValue == other.RawValue;

    [Pure]
    public int CompareTo(Fix32 other) => this.RawValue.CompareTo(other.RawValue);

    [Pure]
    public bool Equals(int other) => this.IsInteger && this.RawValue >> 10 == other;

    [Pure]
    public int CompareTo(int other)
    {
      if (other < -2097152)
        return 1;
      return other > 2097151 ? -1 : this.RawValue.CompareTo(other << 10);
    }

    [Pure]
    public override bool Equals(object obj) => obj is Fix32 fix32 && fix32 == this;

    [Pure]
    public override int GetHashCode() => this.RawValue;

    [Pure]
    public string ToStringRounded(int decimalDigits = 2)
    {
      Assert.That<int>(decimalDigits).IsLessOrEqual(4, "Fix32 has no more than 4 digits of precision.");
      return decimalDigits <= 0 ? this.ToIntRounded().ToString() : this.ToDouble().ToStringDecDigits(decimalDigits);
    }

    [Pure]
    public string ToStringRoundedAdaptive(int significantDigits = 2)
    {
      if (this < 1 && this > -1)
        return this.ToStringRounded(significantDigits);
      if (this < 10 && this > -10)
        return this.ToStringRounded(significantDigits - 1);
      return this < 100 && this > -100 ? this.ToStringRounded(significantDigits - 2) : this.ToStringRounded(significantDigits - 3);
    }

    [Pure]
    public string ToStringFractionReduced()
    {
      Fix32 fractionalPart = this.FractionalPart;
      int num = MafiMath.Gcd(fractionalPart.Abs().RawValue, 1024);
      return string.Format("{0} {1} {2}/{3}", (object) this.IntegerPart, fractionalPart.IsNegative ? (object) "-" : (object) "+", (object) (this.FractionalPart.Abs().RawValue / num), (object) (1024 / num));
    }

    [Pure]
    public override string ToString()
    {
      return this.ToFloat().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugString => this.ToStringRounded(4) ?? "";

    [Pure]
    public Fix32 Abs() => new Fix32(this.RawValue.Abs());

    [Pure]
    public int Sign() => this.RawValue.Sign();

    [Pure]
    public Fix64 Squared() => this.MultAsFix64(this);

    [Pure]
    public Fix32 SquaredAsFix32() => this * this;

    [Pure]
    public Fix64 MultAsFix64(Fix32 rhs)
    {
      return Fix64.FromRaw((long) this.RawValue * (long) rhs.RawValue);
    }

    [Pure]
    public Fix32 Sqrt() => Fix32.FromDouble(Math.Sqrt(this.ToDouble()));

    [Pure]
    public Fix32 Pow(Fix32 exponent)
    {
      return Fix32.FromDouble(Math.Pow(this.ToDouble(), exponent.ToDouble()));
    }

    [Pure]
    public Fix32 Exp() => Fix32.FromDouble(Math.Exp(this.ToDouble()));

    [Pure]
    public Fix32 Log() => Fix32.FromDouble(Math.Log(this.ToDouble()));

    [Pure]
    public Fix32 Log(Fix32 logBase)
    {
      return Fix32.FromDouble(Math.Log(this.ToDouble(), logBase.ToDouble()));
    }

    [Pure]
    public Fix32 Cos() => Fix32.FromDouble(Math.Cos(this.ToDouble()));

    [Pure]
    public Fix32 Sin() => Fix32.FromDouble(Math.Sin(this.ToDouble()));

    [Pure]
    public Fix32 Tan() => Fix32.FromDouble(Math.Tan(this.ToDouble()));

    [Pure]
    public Fix32 Atan() => Fix32.FromDouble(Math.Atan(this.ToDouble()));

    [Pure]
    public Fix32 Sinh() => Fix32.FromDouble(Math.Sinh(this.ToDouble()));

    [Pure]
    public Fix32 Cosh() => Fix32.FromDouble(Math.Cosh(this.ToDouble()));

    [Pure]
    public Fix32 Tanh() => Fix32.FromDouble(Math.Tanh(this.ToDouble()));

    [Pure]
    public Fix32 Coth() => this.Cosh() / this.Sinh();

    [Pure]
    public static Fix32 Exp(Fix32 value) => Fix32.FromDouble(Math.Exp(value.ToDouble()));

    [Pure]
    public Fix32 Modulo(Fix32 mod) => new Fix32(this.RawValue.Modulo(mod.RawValue));

    [Pure]
    public Fix32 Min(Fix32 other) => !(this <= other) ? other : this;

    [Pure]
    public Fix32 Max(Fix32 other) => !(this >= other) ? other : this;

    [Pure]
    public Fix32 Clamp(Fix32 min, Fix32 max)
    {
      if (this <= min)
        return min;
      return !(this >= max) ? this : max;
    }

    [Pure]
    public Fix32 Clamp01() => new Fix32(this.RawValue.Clamp(0, 1024));

    [Pure]
    public bool IsNear(Fix32 other)
    {
      Fix32 fix32 = this - other;
      return fix32 >= -Fix32.EpsilonNear && fix32 <= Fix32.EpsilonNear;
    }

    [Pure]
    public bool IsNear(Fix32 other, Fix32 tolerance)
    {
      Fix32 fix32 = this - other;
      return fix32 >= -tolerance && fix32 <= tolerance;
    }

    [Pure]
    public bool IsNear(Fix32 other, Percent tolerance)
    {
      return Percent.FromRatio(this, other).IsNear(Percent.Hundred, tolerance);
    }

    [Pure]
    public bool IsNearZero() => this >= -Fix32.EpsilonNear && this <= Fix32.EpsilonNear;

    [Pure]
    public Fix32 ScaledBy(Percent p) => p.Apply(this);

    /// <summary>
    /// Linearly interpolates integer between this and <paramref name="to" /> based on parameter <paramref name="t" />. Consider using overload with scale to increase precision.
    /// </summary>
    [Pure]
    public Fix32 Lerp(Fix32 to, Fix32 t) => this + (to - this) * t;

    [Pure]
    public Fix32 Lerp(Fix32 to, Percent t) => this + t.Apply(to - this);

    /// <summary>
    /// Linearly interpolates integer between this and <paramref name="to" /> based on parameter <paramref name="t" />. The <paramref name="t" /> goes from 0 to <paramref name="scale" /> (inclusive).
    /// </summary>
    [Pure]
    public Fix32 Lerp(Fix32 to, Fix32 t, Fix32 scale)
    {
      return this + (to - this).MultAsFix64(t).DivToFix32(scale);
    }

    [Pure]
    public Fix32 Lerp(Fix32 to, Fix32 t, Fix64 scale)
    {
      return this + (to - this).MultAsFix64(t).DivToFix32(scale);
    }

    /// <summary>
    /// Smoothly interpolates value <paramref name="from" /> to value <paramref name="to" /> based on time parameter
    /// <paramref name="t" /> that must be in range [0, 1]. This version is unclamped as it does clamp <paramref name="t" /> to [0, 1] but assumes that caller ensures that.
    /// </summary>
    [Pure]
    public Fix32 SmoothStep(Fix32 to, Fix32 t)
    {
      t = t * t * t * (t * (t * 6f.ToFix32() - 15f.ToFix32()) + 10f.ToFix32());
      return Fix32.One - t + to * t;
    }

    /// <summary>
    /// Smooth transition similar to <see cref="M:Mafi.Fix32.Lerp(Mafi.Fix32,Mafi.Fix32)" /> that is faster at the start and slower at the end.
    /// </summary>
    [Pure]
    public Fix32 EaseIn(Fix32 to, Fix32 t) => this.Lerp(to, (t * Fix32.Tau * Fix32.Quarter).Sin());

    /// <summary>
    /// Smooth transition similar to <see cref="M:Mafi.Fix32.Lerp(Mafi.Fix32,Mafi.Fix32)" /> that is slower at the start and faster at the end.
    /// </summary>
    [Pure]
    public Fix32 EaseOut(Fix32 to, Fix32 t)
    {
      return this.Lerp(to, Fix32.One - (t * Fix32.Tau * Fix32.Quarter).Cos());
    }

    /// <summary>
    /// Slow start and slow end with fast transition in the middle using bezier curve.
    /// </summary>
    [Pure]
    public Fix32 EaseInOut(Fix32 to, Fix32 t) => this.Lerp(to, this.EaseInOut(t));

    /// <summary>
    /// Slow start and slow end with fast transition in the middle using bezier curve.
    /// </summary>
    [Pure]
    public Fix32 EaseInOut(Fix32 t) => t * t * (3f.ToFix32() - 2f.ToFix32() * t);

    public static implicit operator Fix32(int i) => Fix32.FromInt(i);

    public static implicit operator Fix32(ushort i) => Fix32.FromInt(i);

    public static implicit operator Fix32(short i) => Fix32.FromInt(i);

    public static implicit operator Fix32(byte i) => Fix32.FromInt(i);

    public static implicit operator Fix32(sbyte i) => Fix32.FromInt(i);

    public static bool operator ==(Fix32 lhs, Fix32 rhs) => lhs.RawValue == rhs.RawValue;

    public static bool operator !=(Fix32 lhs, Fix32 rhs) => lhs.RawValue != rhs.RawValue;

    public static bool operator >(Fix32 lhs, Fix32 rhs) => lhs.RawValue > rhs.RawValue;

    public static bool operator >=(Fix32 lhs, Fix32 rhs) => lhs.RawValue >= rhs.RawValue;

    public static bool operator <(Fix32 lhs, Fix32 rhs) => lhs.RawValue < rhs.RawValue;

    public static bool operator <=(Fix32 lhs, Fix32 rhs) => lhs.RawValue <= rhs.RawValue;

    public static bool operator ==(Fix32 lhs, int rhs) => (long) lhs.RawValue == (long) rhs << 10;

    public static bool operator !=(Fix32 lhs, int rhs) => (long) lhs.RawValue != (long) rhs << 10;

    public static bool operator >(Fix32 lhs, int rhs) => (long) lhs.RawValue > (long) rhs << 10;

    public static bool operator >=(Fix32 lhs, int rhs) => (long) lhs.RawValue >= (long) rhs << 10;

    public static bool operator <(Fix32 lhs, int rhs) => (long) lhs.RawValue < (long) rhs << 10;

    public static bool operator <=(Fix32 lhs, int rhs) => (long) lhs.RawValue <= (long) rhs << 10;

    public static bool operator ==(int lhs, Fix32 rhs) => (long) lhs << 10 == (long) rhs.RawValue;

    public static bool operator !=(int lhs, Fix32 rhs) => (long) lhs << 10 != (long) rhs.RawValue;

    public static bool operator >(int lhs, Fix32 rhs) => (long) lhs << 10 > (long) rhs.RawValue;

    public static bool operator >=(int lhs, Fix32 rhs) => (long) lhs << 10 >= (long) rhs.RawValue;

    public static bool operator <(int lhs, Fix32 rhs) => (long) lhs << 10 < (long) rhs.RawValue;

    public static bool operator <=(int lhs, Fix32 rhs) => (long) lhs << 10 <= (long) rhs.RawValue;

    public static Fix32 operator +(Fix32 value) => value;

    public static Fix32 operator -(Fix32 value) => new Fix32(-value.RawValue);

    public static Fix32 operator +(Fix32 lhs, Fix32 rhs) => new Fix32(lhs.RawValue + rhs.RawValue);

    public static Fix32 operator -(Fix32 lhs, Fix32 rhs) => new Fix32(lhs.RawValue - rhs.RawValue);

    public static Fix32 operator *(Fix32 lhs, Fix32 rhs)
    {
      long rawValue = (long) lhs.RawValue * (long) rhs.RawValue >> 10;
      if (rawValue > (long) int.MaxValue)
      {
        Mafi.Log.Error(string.Format("Fix32 overflow: {0} * {1} > {2}.", (object) lhs, (object) rhs, (object) Fix32.MaxValue));
        return Fix32.MaxValue;
      }
      if (rawValue >= (long) int.MinValue)
        return new Fix32((int) rawValue);
      Mafi.Log.Error(string.Format("Fix32 overflow: {0} * {1} < {2}.", (object) lhs, (object) rhs, (object) Fix32.MinValue));
      return Fix32.MinValue;
    }

    public static Fix32 operator *(Fix32 lhs, int rhs)
    {
      long rawValue = (long) lhs.RawValue * (long) rhs;
      if (rawValue > (long) int.MaxValue)
      {
        Mafi.Log.Error(string.Format("Fix32 overflow: {0} * {1} > {2}.", (object) lhs, (object) rhs, (object) Fix32.MaxValue));
        return Fix32.MaxValue;
      }
      if (rawValue >= (long) int.MinValue)
        return new Fix32((int) rawValue);
      Mafi.Log.Error(string.Format("Fix32 overflow: {0} * {1} < {2}.", (object) lhs, (object) rhs, (object) Fix32.MinValue));
      return Fix32.MinValue;
    }

    public static Fix32 operator *(int lhs, Fix32 rhs) => rhs * lhs;

    [Pure]
    public Fix32 MultByUnchecked(Fix32 rhs)
    {
      return new Fix32((int) ((long) this.RawValue * (long) rhs.RawValue >> 10));
    }

    [Pure]
    public Fix32 MultByUnchecked(int rhs) => new Fix32(this.RawValue * rhs);

    public static Fix32 operator /(Fix32 lhs, Fix32 rhs)
    {
      if (rhs.IsZero)
      {
        Mafi.Log.Error("Div by zero.");
        return Fix32.Zero;
      }
      if (rhs.IsNegative)
      {
        lhs = -lhs;
        rhs = -rhs;
      }
      long num = (long) lhs.RawValue << 10;
      long rawValue = (num + (num > 0L ? (long) (rhs.RawValue >> 1) : (long) (-rhs.RawValue >> 1))) / (long) rhs.RawValue;
      if (rawValue < (long) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while dividing {0}/{1}. Returning {2}.", (object) lhs, (object) rhs, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Fix32((int) rawValue);
      Mafi.Log.Error(string.Format("Overflow while dividing {0}/{1}. Returning {2}.", (object) lhs, (object) rhs, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    public static Fix32 operator /(Fix32 lhs, int rhs)
    {
      if (rhs == 0)
      {
        Mafi.Log.Error("Div by zero.");
        return Fix32.Zero;
      }
      if (rhs < 0)
      {
        lhs = -lhs;
        rhs = -rhs;
      }
      int num = lhs < 0 ? -(rhs >> 1) : rhs >> 1;
      return new Fix32((int) (((long) lhs.RawValue + (long) num) / (long) rhs));
    }

    public static Fix32 operator /(int lhs, Fix32 rhs)
    {
      if (rhs.IsZero)
      {
        Mafi.Log.Error("Div by zero.");
        return Fix32.Zero;
      }
      if (rhs.IsNegative)
      {
        lhs = -lhs;
        rhs = -rhs;
      }
      long num = (long) lhs << 20;
      long rawValue = (num + (num > 0L ? (long) (rhs.RawValue >> 1) : (long) (-rhs.RawValue >> 1))) / (long) rhs.RawValue;
      if (rawValue < (long) int.MinValue)
      {
        Mafi.Log.Error(string.Format("Overflow while dividing {0}/{1}. Returning {2}.", (object) lhs, (object) rhs, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Fix32((int) rawValue);
      Mafi.Log.Error(string.Format("Overflow while dividing {0}/{1}. Returning {2}.", (object) lhs, (object) rhs, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    [Pure]
    public Fix64 DivToFix64(Fix32 rhs)
    {
      if (rhs.IsZero)
      {
        Mafi.Log.Error("Div by zero.");
        return Fix64.Zero;
      }
      Fix32 fix32 = this;
      if (rhs.IsNegative)
      {
        fix32 = -fix32;
        rhs = -rhs;
      }
      long num = (long) fix32.RawValue << 20;
      return Fix64.FromRaw((num + (num > 0L ? (long) (rhs.RawValue >> 1) : (long) (-rhs.RawValue >> 1))) / (long) rhs.RawValue);
    }

    [Pure]
    public Fix32 DivByPositiveUncheckedUnrounded(Fix32 rhs)
    {
      return new Fix32((int) (((long) this.RawValue << 10) / (long) rhs.RawValue));
    }

    public static Fix32 operator %(Fix32 lhs, Fix32 rhs) => new Fix32(lhs.RawValue % rhs.RawValue);

    public static Fix32 operator <<(Fix32 lhs, int rhs) => new Fix32(lhs.RawValue << rhs);

    public static Fix32 operator >>(Fix32 lhs, int rhs) => new Fix32(lhs.RawValue >> rhs);

    static Fix32()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Fix32.Zero = new Fix32(0);
      Fix32.One = new Fix32(1024);
      Fix32.Quarter = new Fix32(256);
      Fix32.Half = new Fix32(512);
      Fix32.Two = new Fix32(2048);
      Fix32.Three = new Fix32(3072);
      Fix32.Four = new Fix32(4096);
      Fix32.Eight = new Fix32(8192);
      Fix32.Epsilon = new Fix32(1);
      Fix32.EpsilonNear = new Fix32(8);
      Fix32.MinValue = new Fix32(int.MinValue);
      Fix32.MaxValue = new Fix32(int.MaxValue);
      Fix32.MinIntValue = new Fix32(int.MinValue);
      Fix32.MaxIntValue = new Fix32(2147482624);
      Fix32.Tau = Fix32.FromDouble(2.0 * Math.PI);
      Fix32.Sqrt2 = Fix32.FromDouble(1.4142135623730951);
      Fix32.Sqrt3 = Fix32.FromDouble(1.7320508075688772);
      Fix32.Sqrt5 = Fix32.FromDouble(2.23606797749979);
      Fix32.OneOverSqrt2 = Fix32.FromDouble(0.70710678118654757);
      Fix32.OneOverSqrt3 = Fix32.FromDouble(0.57735026918962584);
      Fix32.OneOverSqrt5 = Fix32.FromDouble(0.44721359549995793);
    }
  }
}
