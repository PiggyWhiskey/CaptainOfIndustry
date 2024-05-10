// Decompiled with JetBrains decompiler
// Type: Mafi.Percent
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Represents percentage in base 10 with 5 decimal digits precision (xx.xxx%). Max values are +-2.1M%.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("{DebugString,nq}")]
  public readonly struct Percent : IEquatable<Percent>, IComparable<Percent>
  {
    /// <summary>Number of fractional decimal digits.</summary>
    internal const int FRAC_DEC_DIGITS = 5;
    internal const int FRACTION_RANGE = 100000;
    public static readonly Percent Tau;
    /// <summary>The actual stored value.</summary>
    public readonly int RawValue;

    public static void Serialize(Percent value, BlobWriter writer)
    {
      writer.WriteInt(value.RawValue);
    }

    public static Percent Deserialize(BlobReader reader) => new Percent(reader.ReadInt());

    public static Percent Zero => new Percent();

    public static Percent Epsilon => new Percent(1);

    public static Percent One => new Percent(1000);

    public static Percent Hundred => new Percent(100000);

    public static Percent Fifty => new Percent(50000);

    public static Percent MinValue => new Percent(int.MinValue);

    public static Percent MaxValue => new Percent(int.MaxValue);

    [LoadCtor]
    private Percent(int rawValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.RawValue = rawValue;
    }

    public static Percent FromRaw(int rawValue) => new Percent(rawValue);

    public static Percent FromFix32(Fix32 value)
    {
      long rawValue = (long) value.RawValue * 100000L / 1024L;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while creating Percent from Fix32 {0}. Returning {1}.", (object) value, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while creating Percent from Fix32 {0}. Returning {1}.", (object) value, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public static Percent FromFix64(Fix64 value)
    {
      long rawValue = (long) ((double) value.RawValue * 100000.0 / 1048576.0);
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while creating Percent from Fix64 {0}. Returning {1}.", (object) value, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while creating Percent from Fix64 {0}. Returning {1}.", (object) value, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public static Percent FromInt(int value) => new Percent(value * 100000);

    /// <summary>
    /// Creates percentage from int that is already pre-multiplied by 100. Value 1 represents 1%.
    /// </summary>
    public static Percent FromPercentVal(int value) => Percent.FromRatio(value, 100);

    public static Percent FromRatio(int numerator, int denominator)
    {
      return Percent.FromRatio((long) numerator, (long) denominator);
    }

    public static Percent FromRatio(long numerator, long denominator)
    {
      if (denominator == 0L)
      {
        Log.Error("Creating percentage from ratio with zero denominator.");
        return Percent.Zero;
      }
      if (denominator < 0L)
      {
        numerator = -numerator;
        denominator = -denominator;
      }
      long num = numerator * 100000L;
      long rawValue = (num + (num > 0L ? denominator / 2L : -denominator / 2L)) / denominator;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while creating Percent from fraction {0}/{1}. Returning {2}.", (object) numerator, (object) denominator, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while creating Percent from fraction {0}/{1}. Returning {2}.", (object) numerator, (object) denominator, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    /// <summary>
    /// More efficient than <see cref="M:Mafi.Percent.FromRatio(System.Int32,System.Int32)" /> but caller must be very sure that both arguments are not negative.
    /// </summary>
    public static Percent FromRatioNonNegative(long numerator, long denominator)
    {
      if (denominator == 0L)
      {
        Log.Error("Creating percentage from ratio with zero denominator.");
        return Percent.Zero;
      }
      long rawValue = (numerator * 100000L + (denominator >> 1)) / denominator;
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while creating Percent from positive fraction {0}/{1}.", (object) numerator, (object) denominator) + string.Format(" Returning {0}.", (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public static Percent FromRatio(Fix32 numerator, Fix32 denominator)
    {
      return Percent.FromRatio((long) numerator.RawValue, (long) denominator.RawValue);
    }

    public static Percent FromRatio(Fix64 numerator, Fix64 denominator)
    {
      return Percent.FromRatio(numerator.RawValue, denominator.RawValue);
    }

    public static Percent FromValueBetweenMinMax(int value, int min, int max)
    {
      return Percent.FromRatio(value - min, max - min);
    }

    public static Percent FromFloat(float value)
    {
      value = (float) Math.Round((double) (value * 100000f));
      Assert.That<bool>((double) value >= (double) int.MinValue && (double) value <= 2147483648.0).IsTrue<float>("Created Percent from float {0} is outside of its precision.", value);
      return new Percent((int) value);
    }

    public static Percent FromDouble(double value)
    {
      value = Math.Round(value * 100000.0);
      Assert.That<bool>(value >= (double) int.MinValue && value <= (double) int.MaxValue).IsTrue<double>("Created Percent from double {0} is outside of its precision.", value);
      return new Percent((int) value);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.RawValue == 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.RawValue != 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNearHundred => this.IsNear(Percent.Hundred, 0.1.Percent());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsPositive => this.RawValue > 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotPositive => this.RawValue <= 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNegative => this.RawValue < 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotNegative => this.RawValue >= 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsWithin0To100PercIncl => this.RawValue >= 0 && this.RawValue <= 100000;

    /// <summary>
    /// Returns integer part of this value discarding any fraction. Invariant: x == x.IntegerPart +
    /// x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int IntegerPart => (this.RawValue > 0 ? this.RawValue : this.RawValue + 99999) / 100000;

    /// <summary>
    /// Fractional part. Invariant: x == x.IntegerPart + x.FractionalPart.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Percent FractionalPart => this - Percent.FromRaw(this.IntegerPart * 100000);

    [Pure]
    public int ToIntRounded() => ((long) this.RawValue).RoundDivToInt(100000);

    [Pure]
    public int ToIntPercentRounded() => (100L * (long) this.RawValue).RoundDivToInt(100000);

    [Pure]
    public Fix32 ToFix32() => Fix32.FromRaw(((long) this.RawValue * 1024L).RoundDivToInt(100000));

    [Pure]
    public Fix64 ToFix64() => Fix64.FromRaw((long) this.RawValue * 1048576L / 100000L);

    [Pure]
    public Fix64 ToFix64Percent()
    {
      return Fix64.FromRaw(100L * (long) this.RawValue * 1048576L / 100000L);
    }

    [Pure]
    public float ToFloat() => (float) this.RawValue / 100000f;

    /// <summary>
    /// Returns this number as double. This conversion is loss-less.
    /// </summary>
    [Pure]
    public double ToDouble() => (double) this.RawValue / 100000.0;

    [Pure]
    public bool Equals(Percent other) => this.RawValue == other.RawValue;

    [Pure]
    public int CompareTo(Percent other) => this.RawValue.CompareTo(other.RawValue);

    [Pure]
    public override bool Equals(object obj) => obj is Percent other && this.Equals(other);

    [Pure]
    public override int GetHashCode() => this.RawValue;

    /// <summary>
    /// The same as <c>ToStringRounded(0)</c> but more efficient.
    /// </summary>
    [Pure]
    public string ToStringRounded() => this.ToIntPercentRounded().ToStringCached() + "%";

    [Pure]
    public string ToStringRounded(int decimalDigits)
    {
      return this.ToFix64Percent().ToStringRounded(decimalDigits) + "%";
    }

    [Pure]
    public string ToStringFractionReduced()
    {
      Percent fractionalPart = this.FractionalPart;
      int num = MafiMath.Gcd(fractionalPart.Abs().RawValue, 100000);
      return string.Format("{0}{1}{2}/{3}", (object) this.IntegerPart, fractionalPart.IsNegative ? (object) "-" : (object) "+", (object) (this.FractionalPart.Abs().RawValue / num), (object) (100000 / num));
    }

    [Pure]
    public override string ToString() => this.ToStringRounded(2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugString => this.ToString() + " (" + this.ToStringFractionReduced() + ")";

    [Pure]
    public Percent Abs() => new Percent(this.RawValue.Abs());

    [Pure]
    public int Sign() => this.RawValue.Sign();

    [Pure]
    public int Apply(int val)
    {
      long num = this.Apply((long) val);
      if (num < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) int.MinValue));
        return int.MinValue;
      }
      if (num <= (long) int.MaxValue)
        return (int) num;
      Log.Error(string.Format("Overflow while applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) int.MaxValue));
      return int.MaxValue;
    }

    [Pure]
    public long Apply(long val)
    {
      long num = val * (long) this.RawValue;
      return (num + (num >= 0L ? 50000L : -50000L)) / 100000L;
    }

    [Pure]
    public Fix32 ApplyToFix32(int val)
    {
      long num = (long) val * (long) this.RawValue * 1024L;
      long rawValue = (num + (num >= 0L ? 50000L : -50000L)) / 100000L;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) int.MinValue));
        return (Fix32) int.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return Fix32.FromRaw((int) rawValue);
      Log.Error(string.Format("Overflow while applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) int.MaxValue));
      return (Fix32) int.MaxValue;
    }

    [Pure]
    public Fix32 Apply(Fix32 val)
    {
      long num = (long) val.RawValue * (long) this.RawValue;
      long rawValue = (num + (num >= 0L ? 50000L : -50000L)) / 100000L;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error("Underflow in Percent.Apply.");
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return Fix32.FromRaw((int) rawValue);
      Log.Error("Overflow in Percent.Apply.");
      return Fix32.MaxValue;
    }

    /// <summary>
    /// Similar to <see cref="M:Mafi.Percent.Apply(Mafi.Fix32)" /> but performs no rounding and no overflow checks.
    /// </summary>
    [Pure]
    public Fix32 ApplyFast(Fix32 val)
    {
      return Fix32.FromRaw((int) ((long) val.RawValue * (long) this.RawValue / 100000L));
    }

    /// <summary>Perf: This is slow due to usage of BigInteger.</summary>
    [Pure]
    public Fix64 Apply(Fix64 val)
    {
      bool flag = ((val.RawValue ^ (long) this.RawValue) & long.MinValue) == 0L;
      ulong resultHi;
      ulong resultLo;
      MafiMath.Mul64To128((ulong) val.RawValue.Abs(), (ulong) this.RawValue.Abs(), out resultHi, out resultLo);
      MafiMath.Add64To128(50000UL, ref resultHi, ref resultLo);
      ulong result;
      if (!MafiMath.TryDiv128To64(resultHi, resultLo, 100000UL, out result))
      {
        Log.Error("Div by zero");
        return val;
      }
      if (((long) result & long.MinValue) != 0L)
      {
        Log.Error(string.Format("Percent mult overflow: {0} * {1}", (object) val, (object) this));
        return !flag ? Fix64.MinValue : Fix64.MaxValue;
      }
      long num = (long) result;
      return Fix64.FromRaw(flag ? num : -num);
    }

    /// <summary>
    /// Similar to <see cref="M:Mafi.Percent.Apply(Mafi.Fix64)" /> but performs no rounding and no overflow checks.
    /// Warning: May overflow for large values.
    /// </summary>
    [Pure]
    public Fix64 ApplyFast(Fix64 val)
    {
      return Fix64.FromRaw(val.RawValue * (long) this.RawValue / 100000L);
    }

    [Pure]
    public Percent Apply(Percent val) => this * val;

    [Pure]
    public float Apply(float val) => val * this.ToFloat();

    [Pure]
    public int ApplyCeiled(int val)
    {
      Assert.That<bool>(this.IsNotNegative).IsTrue();
      return (int) this.ApplyCeiled((long) val);
    }

    [Pure]
    public long ApplyCeiled(long val)
    {
      Assert.That<bool>(this.IsNotNegative).IsTrue("TODO");
      return (val * (long) this.RawValue + 99999L) / 100000L;
    }

    [Pure]
    public int ApplyInverseFloored(int val) => (int) this.ApplyInverseFloored((long) val);

    [Pure]
    public long ApplyInverseFloored(long val) => val * 100000L / (long) this.RawValue;

    [Pure]
    public int ApplyInverseCeiled(int val) => (int) this.ApplyInverseCeiled((long) val);

    [Pure]
    public long ApplyInverseCeiled(long val) => (val * 100000L + 99999L) / (long) this.RawValue;

    [Pure]
    public Fix32 ApplyInverse(Fix32 val)
    {
      if (this.IsZero)
      {
        Log.Error("Div by zero in 'Percent.ApplyInverse', returning zero.");
        return Fix32.Zero;
      }
      long num = (long) val.RawValue * 100000L;
      long rawValue = (num + (num >= 0L ? (long) (this.RawValue / 2) : (long) (-this.RawValue / 2))) / (long) this.RawValue;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while inversly applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) Fix32.MinValue));
        return Fix32.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return Fix32.FromRaw((int) rawValue);
      Log.Error(string.Format("Overflow while inversly applying Percent {0} to value {1}. Returning {2}.", (object) this, (object) val, (object) Fix32.MaxValue));
      return Fix32.MaxValue;
    }

    [Pure]
    public Percent ScaleBy(Percent percent) => this * percent;

    [Pure]
    public Percent Lerp(Percent other, Percent t)
    {
      return new Percent(this.RawValue.Lerp(other.RawValue, t));
    }

    [Pure]
    public Percent Lerp(Percent other, Percent t, Percent scale)
    {
      return new Percent(this.RawValue.Lerp(other.RawValue, (long) t.RawValue, (long) scale.RawValue));
    }

    [Pure]
    public Percent Lerp(Percent other, long t, long scale)
    {
      return new Percent(this.RawValue.Lerp(other.RawValue, t, scale));
    }

    /// <summary>Clamps this percentage to [0, 100] range (inclusive).</summary>
    [Pure]
    public Percent Clamp0To100() => new Percent(this.RawValue.Clamp(0, 100000));

    [Pure]
    public Percent Min(Percent rhs) => new Percent(this.RawValue.Min(rhs.RawValue));

    [Pure]
    public Percent Max(Percent rhs) => new Percent(this.RawValue.Max(rhs.RawValue));

    [Pure]
    public Percent Clamp(Percent min, Percent max)
    {
      return new Percent(this.RawValue.Clamp(min.RawValue, max.RawValue));
    }

    [Pure]
    public bool IsNear(Percent other, Percent tolerance)
    {
      return this.RawValue.IsNear(other.RawValue, tolerance.RawValue);
    }

    [Pure]
    public Percent Average(Percent other) => new Percent((this.RawValue + other.RawValue) / 2);

    [Pure]
    public Percent Squared() => this * this;

    [Pure]
    public Percent Sqrt() => Percent.FromDouble(Math.Sqrt(this.ToDouble()));

    [Pure]
    public Percent Pow(int exponent)
    {
      return Percent.FromDouble(Math.Pow(this.ToDouble(), (double) exponent));
    }

    [Pure]
    public Percent Pow(Percent exponent)
    {
      return Percent.FromDouble(Math.Pow(this.ToDouble(), exponent.ToDouble()));
    }

    [Pure]
    public Percent Pow(Fix32 exponent)
    {
      return Percent.FromDouble(Math.Pow(this.ToDouble(), exponent.ToDouble()));
    }

    [Pure]
    public Percent LogNatural() => Percent.FromDouble(Math.Log(this.ToDouble()));

    [Pure]
    public Percent Cos() => Percent.FromDouble(Math.Cos(this.ToDouble()));

    [Pure]
    public Percent Sin() => Percent.FromDouble(Math.Sin(this.ToDouble()));

    /// <summary>
    /// Returns value divided by two very efficiently (uses a bit shift). Use with caution, this works for all values
    /// except for <see cref="P:Mafi.Percent.MinValue" /> (the smallest representable value).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Percent HalfFast => new Percent(this.RawValue >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Percent TimesTwoFast => new Percent(this.RawValue << 1);

    public static bool operator ==(Percent lhs, Percent rhs) => lhs.RawValue == rhs.RawValue;

    public static bool operator !=(Percent lhs, Percent rhs) => lhs.RawValue != rhs.RawValue;

    public static bool operator <(Percent lhs, Percent rhs) => lhs.RawValue < rhs.RawValue;

    public static bool operator <=(Percent lhs, Percent rhs) => lhs.RawValue <= rhs.RawValue;

    public static bool operator >(Percent lhs, Percent rhs) => lhs.RawValue > rhs.RawValue;

    public static bool operator >=(Percent lhs, Percent rhs) => lhs.RawValue >= rhs.RawValue;

    public static Percent operator +(Percent value) => value;

    public static Percent operator -(Percent value) => new Percent(-value.RawValue);

    public static Percent operator +(Percent lhs, Percent rhs)
    {
      return new Percent(lhs.RawValue + rhs.RawValue);
    }

    public static Percent operator -(Percent lhs, Percent rhs)
    {
      return new Percent(lhs.RawValue - rhs.RawValue);
    }

    public static Percent operator *(Percent lhs, Percent rhs)
    {
      long rawValue = (long) lhs.RawValue * (long) rhs.RawValue / 100000L;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while multiplying percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while multiplying percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public static Percent operator *(Percent lhs, Fix64 rhs)
    {
      long rawValue = (long) lhs.RawValue * rhs.RawValue >> 20;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while multiplying percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while multiplying percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public static Percent operator /(Percent lhs, Percent rhs)
    {
      long rawValue = (long) lhs.RawValue * 100000L / (long) rhs.RawValue;
      if (rawValue < (long) int.MinValue)
      {
        Log.Error(string.Format("Overflow while dividing percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MinValue));
        return Percent.MinValue;
      }
      if (rawValue <= (long) int.MaxValue)
        return new Percent((int) rawValue);
      Log.Error(string.Format("Overflow while dividing percentages {0} and {1}. Returning {2}.", (object) lhs, (object) rhs, (object) Percent.MaxValue));
      return Percent.MaxValue;
    }

    public Fix32 DivAsFix32(Percent rhs) => (this / rhs).ToFix32();

    /// <summary>Returns 100% - this</summary>
    public Percent InverseTo100() => Percent.Hundred - this;

    public static Percent operator *(Percent lhs, int rhs) => new Percent(lhs.RawValue * rhs);

    public static Percent operator *(int lhs, Percent rhs) => new Percent(lhs * rhs.RawValue);

    public static Percent operator /(Percent lhs, int rhs) => new Percent(lhs.RawValue / rhs);

    public static Percent operator /(Percent lhs, Fix32 rhs)
    {
      return new Percent((int) (((long) lhs.RawValue << 10) / (long) rhs.RawValue));
    }

    static Percent()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Percent.Tau = new Percent(628318);
    }
  }
}
