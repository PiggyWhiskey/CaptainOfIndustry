// Decompiled with JetBrains decompiler
// Type: Mafi.MafiMath
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using Mafi.Random;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

#nullable disable
namespace Mafi
{
  public static class MafiMath
  {
    private static readonly string[] ROUNDING_STRS;
    /// <summary>
    /// Any two floats around zero that differ less than this tolerance are considered "near".
    /// </summary>
    public const float DEFAULT_FLOAT_TOLERANCE = 0.0001f;
    /// <inheritdoc cref="F:Mafi.MafiMath.TAU" />
    public const double TAU_D = 6.2831853071795862;
    public const double SQRT2 = 1.4142135623730951;
    /// <summary>
    /// Tau τ = 2 π. It is the "right" circle constant superior to π, see https://tauday.com/tau-manifesto
    /// and https://www.youtube.com/watch?v=H69YH5TnNXI for details.
    /// </summary>
    public const float TAU = 6.28318548f;
    /// <summary>
    /// Multiplicative constant that converts degrees to radians.
    /// </summary>
    public const float DEG_2_RAD = 0.0174532924f;
    /// <summary>
    /// Multiplicative constant that converts radians to degrees to radians.
    /// </summary>
    public const float RAD_2_DEG = 57.29578f;
    public const double RAD_2_DEG_D = 57.295779513082323;
    private static readonly Fix32[] SMALL_SQRT_CACHE;
    public static readonly Fix32 ONE_MINUS_TAU_OVER_4;
    public static readonly Fix32 ONE_MINUS_TAU_OVER_2;
    public static readonly Fix32 TAU_OVER_2_MINUS_2;
    private static readonly byte[] s_multiplyDeBruijnBitPosition32;

    /// <summary>
    /// Whether given integer is a power of two and not zero. If you don't care about zero use more efficient <see cref="M:Mafi.MafiMath.IsPowerOfTwoOrZero(System.Int32)" /> function.
    /// PERF: Very efficient.
    /// </summary>
    [Pure]
    public static bool IsPowerOfTwo(this int number) => number != 0 && (number & number - 1) == 0;

    /// <summary>
    /// Returns true if given integer is a power of two or zero. This is more efficient than the <see cref="M:Mafi.MafiMath.IsPowerOfTwo(System.Int32)" /> function.
    /// PERF: Very efficient.
    /// </summary>
    [Pure]
    public static bool IsPowerOfTwoOrZero(int number) => (number & number - 1) == 0;

    /// <summary>
    /// Returns the nearest higher power of two or zero. If the number is already a power of two, it stays the same.
    /// </summary>
    /// <remarks>http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2Float</remarks>
    [Pure]
    public static int CeilToPowerOfTwoOrZero(this int number)
    {
      --number;
      number |= number >> 1;
      number |= number >> 2;
      number |= number >> 4;
      number |= number >> 8;
      number |= number >> 16;
      return number + 1;
    }

    /// <summary>
    /// Computes greatest common divisor of the arguments according to Euclidean algorithm
    /// https://en.wikipedia.org/wiki/Euclidean_algorithm
    /// </summary>
    /// <remarks>If both parameters are zero, returns zero.</remarks>
    [Pure]
    public static int Gcd(int a, int b)
    {
      Assert.That<int>(a).IsNotNegative();
      Assert.That<int>(b).IsNotNegative();
      while (b != 0)
      {
        int num = b;
        b = a % b;
        a = num;
      }
      return a;
    }

    [Pure]
    public static long Gcd(long a, long b)
    {
      Assert.That<long>(a).IsNotNegative();
      Assert.That<long>(b).IsNotNegative();
      while (b != 0L)
      {
        long num = b;
        b = a % b;
        a = num;
      }
      return a;
    }

    /// <summary>
    /// Computes greatest common divisor for a sequence of numbers. Create you own version not using IEnumerable for
    /// performance critical usage.
    /// </summary>
    [Pure]
    public static int Gcd(IEnumerable<int> numbers)
    {
      using (IEnumerator<int> enumerator = numbers.GetEnumerator())
      {
        if (!enumerator.MoveNext())
        {
          Assert.Fail("GCD called on an empty sequence.");
          return 0;
        }
        int a = enumerator.Current;
        while (enumerator.MoveNext())
          a = MafiMath.Gcd(a, enumerator.Current);
        return a;
      }
    }

    /// <summary>
    /// Linearly interpolates integer between <paramref name="from" /> and <paramref name="to" /> based on parameter
    /// <paramref name="t" />. The <paramref name="t" /> goes from 0 to <paramref name="scale" /> (inclusive).
    /// </summary>
    /// <remarks>This function uses longs to prevent overflows.</remarks>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="t">Interpolation parameter [0, <paramref name="scale" />)</param>
    /// <param name="scale">The scale of interpolation parameter.</param>
    [Pure]
    public static int Lerp(this int from, int to, long t, long scale)
    {
      return from + (int) ((long) (to - from) * t / scale);
    }

    [Pure]
    public static Fix32 LerpToFix32(this int from, int to, Percent t)
    {
      return (Fix32) from + t.ApplyToFix32(to - from);
    }

    /// <summary>
    /// Linearly interpolates integer between <paramref name="from" /> and <paramref name="to" /> based on parameter
    /// <paramref name="t" />. The <paramref name="t" /> goes from 0 to 1.
    /// </summary>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="t">Interpolation parameter [0, 1]</param>
    [Pure]
    public static int Lerp(this int from, int to, Percent t) => from + t.Apply(to - from);

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> based on parameter <paramref name="t" />. The <paramref name="t" /> goes from 0 to 1.
    /// </summary>
    [Pure]
    public static float Lerp(this float from, float to, float t) => from + (to - from) * t;

    [Pure]
    public static float Lerp(this float from, float to, Percent t) => from + t.Apply(to - from);

    [Pure]
    public static byte LerpStepAtLeastOne(this byte value, byte otherValue, Percent percent)
    {
      int num = (int) value * 100000 + ((int) otherValue - (int) value) * percent.RawValue;
      if ((int) value < (int) otherValue)
        num += 99999;
      return (byte) (num / 100000);
    }

    /// <summary>
    /// "Symmetric quadratic" interpolation between two integers. Interpolates integer between <paramref name="from" /> and <paramref name="to" /> based on time parameter <paramref name="t" />. The <paramref name="t" /> goes from 0 to <paramref name="scale" /> (inclusive).
    /// 
    /// The number added to <paramref name="from" /> grows quadratically in <paramref name="t" /> until it reaches (
    /// <paramref name="from" /> + <paramref name="to" />) / 2 at t = <paramref name="scale" /> / 2, after reaching
    /// this point it starts to grow according to reversed quadratic function with origin at ( <paramref name="scale" />, <paramref name="to" />).
    /// 
    /// This means that as <paramref name="t" /> is closer to 0 or scale the interpolation function has slower growth
    /// with fastest growth at t = scale / 2. The speed of growth (first derivative) of the interpolation function
    /// is symmetrical about <paramref name="scale" /> / 2.
    /// </summary>
    /// <remarks>This function uses longs to prevent overflows.</remarks>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="t">Interpolation parameter [0, <paramref name="scale" />]</param>
    /// <param name="scale">The scale of interpolation parameter.</param>
    [Pure]
    public static int Sqerp(int from, int to, long t, long scale)
    {
      int num1 = to - from;
      int num2 = num1 / 2;
      long num3 = scale / 2L;
      if (t <= num3)
        return from + (int) ((long) num2 * t * t / (num3 * num3));
      t = scale - t;
      long num4 = scale - num3;
      int num5 = num1 - num2;
      return from + num1 - (int) ((long) num5 * t * t / (num4 * num4));
    }

    [Pure]
    public static Fix32 Sqerp(Fix32 from, Fix32 to, Fix32 t, Fix32 scale)
    {
      Fix32 fix32_1 = to - from;
      Fix32 fix32_2 = fix32_1 / 2;
      Fix32 fix32_3 = scale / 2;
      if (!(t > fix32_3))
        return from + (fix32_2 * t.Squared()).DivToFix32(fix32_3.Squared());
      t = scale - t;
      Fix32 fix32_4 = scale - fix32_3;
      Fix32 fix32_5 = fix32_1 - fix32_2;
      return from + fix32_1 - (fix32_5 * t.Squared()).DivToFix32(fix32_4.Squared());
    }

    [Pure]
    public static float Round(this float value, int decimals)
    {
      return (float) Math.Round((double) value, decimals);
    }

    [Pure]
    public static double Round(this double value, int decimals) => Math.Round(value, decimals);

    [Pure]
    public static int RoundToInt(this float value) => (int) Math.Round((double) value);

    [Pure]
    public static int RoundToInt(this double value) => (int) Math.Round(value);

    [Pure]
    public static long RoundToLong(this float value) => (long) Math.Round((double) value);

    [Pure]
    public static long RoundToLong(this double value) => (long) Math.Round(value);

    [Pure]
    public static int CeilToInt(this float value) => (int) Math.Ceiling((double) value);

    [Pure]
    public static int CeilToInt(this double value) => (int) Math.Ceiling(value);

    [Pure]
    public static long CeilToLong(this double value) => (long) Math.Ceiling(value);

    [Pure]
    public static int FloorToInt(this float x)
    {
      int num = (int) x;
      return (double) x >= (double) num ? num : num - 1;
    }

    [Pure]
    public static long FloorToLong(this float x)
    {
      long num = (long) x;
      return (double) x >= (double) num ? num : num - 1L;
    }

    [Pure]
    public static int FloorToInt(this double x)
    {
      int num = (int) x;
      return x >= (double) num ? num : num - 1;
    }

    [Pure]
    public static long FloorToLong(this double x)
    {
      long num = (long) x;
      return x >= (double) num ? num : num - 1L;
    }

    /// <summary>
    /// Formats a float to a given number of significant digits.
    /// </summary>
    /// <example>
    /// <code>
    /// 0.086 -› "0.09" (digits = 1)
    /// 0.00030908 -› "0.00031" (digits = 2)
    /// 1239451.0 -› "1240000" (digits = 3)
    /// 5084611353.0 -› "5085000000" (digits = 4)
    /// 0.00000000000000000846113537656557 -› "0.00000000000000000846114" (digits = 6)
    /// 50.8437 -› "50.84" (digits = 4)
    /// 50.846 -› "50.85" (digits = 4)
    /// 990.0 -› "1000" (digits = 1)
    /// -5488.0 -› "-5000" (digits = 1)
    /// -990.0 -› "-1000" (digits = 1)
    /// 0.0000789 -› "0.000079" (digits = 2)
    /// 123456 -&gt; "120,000" (digits = 2, thousandsSeparator = true)
    /// </code>
    /// </example>
    /// <remarks>https://stackoverflow.com/questions/374316/round-a-double-to-x-significant-figures</remarks>
    [Pure]
    public static string RoundToSigDigits(
      this float number,
      int digits,
      bool hideTrailingZeros = false,
      bool alwaysShowDecimalSeparator = false,
      bool thousandsSeparator = false)
    {
      if (float.IsNaN(number) || float.IsInfinity(number))
        return number.ToString();
      string str1 = "";
      string str2 = "0";
      string str3 = "";
      if ((double) number != 0.0)
      {
        if (digits < 1)
          throw new ArgumentException("The digits parameter must be greater than zero.");
        if ((double) number < 0.0)
        {
          str1 = "-";
          number = number.Abs();
        }
        string str4 = string.Format("{0:" + new string('#', digits) + "E0}", (object) number);
        string str5 = str4.Substring(0, digits);
        int repeatCount = int.Parse(str4.Substring(digits + 1));
        StringBuilder stringBuilder = new StringBuilder(str5);
        if (hideTrailingZeros)
        {
          while (stringBuilder[stringBuilder.Length - 1] == '0')
          {
            --stringBuilder.Length;
            ++repeatCount;
          }
        }
        int startIndex;
        if (stringBuilder.Length + repeatCount < 1)
        {
          stringBuilder.Insert(0, "0", 1 - stringBuilder.Length - repeatCount);
          startIndex = 1;
        }
        else if (repeatCount > 0)
        {
          stringBuilder.Append('0', repeatCount);
          if (thousandsSeparator)
          {
            string numberGroupSeparator = LocalizationManager.CurrentCultureInfo.NumberFormat.NumberGroupSeparator;
            for (int index = stringBuilder.Length - 3; index > 0; index -= 3)
              stringBuilder.Insert(index, numberGroupSeparator);
          }
          startIndex = stringBuilder.Length;
        }
        else
          startIndex = stringBuilder.Length + repeatCount;
        str2 = stringBuilder.ToString();
        if (startIndex < str2.Length)
        {
          str3 = str2.Substring(startIndex);
          str2 = str2.Remove(startIndex);
        }
      }
      string sigDigits = str1 + str2;
      if (str3 == "")
      {
        if (alwaysShowDecimalSeparator)
        {
          string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
          sigDigits = sigDigits + decimalSeparator + "0";
        }
      }
      else
      {
        string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        sigDigits = sigDigits + decimalSeparator + str3;
      }
      return sigDigits;
    }

    /// <summary>
    /// Formats a float to have at least the specified number of digits.
    /// Note: Outputs at most <c>max(significantDigits, 4)</c> digits.
    /// </summary>
    /// <remarks>
    /// For significantDigits = 2:
    /// <code>
    /// 0.1234 =&gt; 0.12
    /// 1.234 =&gt; 1.2
    /// 12.34 =&gt; 12
    /// 123.4 =&gt; 123
    /// 1234 =&gt; 1234
    /// </code>
    /// </remarks>
    [Pure]
    public static string ToStringRoundedAdaptive(this float number, int significantDigits = 2)
    {
      if ((double) number < 10.0 && (double) number > -10.0)
        return number.RoundToSigDigits(significantDigits, false, false, false);
      if ((double) number < 100.0 && (double) number > -100.0)
        return number.RoundToSigDigits(significantDigits.Max(2), false, false, false);
      return (double) number < 1000.0 && (double) number > -1000.0 ? number.RoundToSigDigits(significantDigits.Max(3), false, false, false) : number.RoundToSigDigits(significantDigits.Max(4), false, false, false);
    }

    [Pure]
    public static string ToStringDecDigits(this double value, int decimalDigits = 2)
    {
      Assert.That<int>(decimalDigits).IsLessOrEqual(4, "Fix32 has no more than 4 digits of precision.");
      if (decimalDigits <= 0)
        return value.RoundToInt().ToStringCached();
      if (decimalDigits > MafiMath.ROUNDING_STRS.Length)
        decimalDigits = MafiMath.ROUNDING_STRS.Length;
      return value.ToString(MafiMath.ROUNDING_STRS[decimalDigits - 1]);
    }

    [Pure]
    public static string RoundToSigDigits(
      this double number,
      int digits,
      bool hideTrailingZeros = false,
      bool alwaysShowDecimalSeparator = false,
      bool thousandsSeparator = false)
    {
      return ((float) number).RoundToSigDigits(digits, hideTrailingZeros, alwaysShowDecimalSeparator, thousandsSeparator);
    }

    [Pure]
    public static string RoundToSigDigits(
      this long number,
      int digits,
      bool hideTrailingZeros = false,
      bool alwaysShowDecimalSeparator = false,
      bool thousandsSeparator = false)
    {
      return ((float) number).RoundToSigDigits(digits, hideTrailingZeros, alwaysShowDecimalSeparator, thousandsSeparator);
    }

    [Pure]
    public static string RoundToSigDigits(
      this int number,
      int digits,
      bool hideTrailingZeros = false,
      bool alwaysShowDecimalSeparator = false,
      bool thousandsSeparator = false)
    {
      return ((float) number).RoundToSigDigits(digits, hideTrailingZeros, alwaysShowDecimalSeparator, thousandsSeparator);
    }

    [Pure]
    public static byte Clamp(this byte value, byte min, byte max)
    {
      if ((int) value <= (int) min)
        return min;
      return (int) value < (int) max ? value : max;
    }

    [Pure]
    public static ushort Clamp(this ushort value, ushort min, ushort max)
    {
      if ((int) value <= (int) min)
        return min;
      return (int) value < (int) max ? value : max;
    }

    [Pure]
    public static int Clamp(this int value, int min, int max)
    {
      if (value <= min)
        return min;
      return value < max ? value : max;
    }

    [Pure]
    public static float Clamp(this float value, float min, float max)
    {
      if ((double) value <= (double) min)
        return min;
      return (double) value < (double) max ? value : max;
    }

    [Pure]
    public static double Clamp(this double value, double min, double max)
    {
      if (value <= 0.0)
        return 0.0;
      return value < 1.0 ? value : 1.0;
    }

    [Pure]
    public static long Clamp(this long value, long min, long max)
    {
      if (value <= min)
        return min;
      return value < max ? value : max;
    }

    [Pure]
    public static float Clamp01(this float value)
    {
      if ((double) value <= 0.0)
        return 0.0f;
      return (double) value < 1.0 ? value : 1f;
    }

    [Pure]
    public static int FloorToMultipleOf(this int value, int multiple)
    {
      return value - value.Modulo(multiple);
    }

    [Pure]
    public static int CeilToMultipleOf(this int value, int multiple)
    {
      return (value + multiple - 1) / multiple * multiple;
    }

    [Pure]
    public static long CeilToMultipleOf(this long value, long multiple)
    {
      return (value + multiple - 1L) / multiple * multiple;
    }

    /// <summary>
    /// Rounds this integer value to a nearest multiple of given number.
    /// </summary>
    [Pure]
    public static int RoundToMultipleOf(this int value, int multiple)
    {
      return ((float) value / (float) multiple).RoundToInt() * multiple;
    }

    /// <summary>
    /// Rounds this float value to a nearest multiple of given number.
    /// </summary>
    [Pure]
    public static int RoundToMultipleOf(this float value, int multiple)
    {
      return (value / (float) multiple).RoundToInt() * multiple;
    }

    [Pure]
    public static float RoundToMultipleOf(this float value, float multiple)
    {
      return (float) (value / multiple).RoundToInt() * multiple;
    }

    [Pure]
    public static bool IsNear(this byte value, byte targetValue, int tolerance)
    {
      int num = (int) value - (int) targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    [Pure]
    public static bool IsNear(this short value, short targetValue, int tolerance)
    {
      int num = (int) value - (int) targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    [Pure]
    public static bool IsNear(this ushort value, ushort targetValue, int tolerance)
    {
      int num = (int) value - (int) targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    /// <summary>
    /// Whether difference between <paramref name="value" /> and <paramref name="targetValue" /> is less or equal than
    /// <paramref name="tolerance" />.
    /// </summary>
    [Pure]
    public static bool IsNear(this int value, int targetValue, int tolerance)
    {
      Assert.That<int>(tolerance).IsNotNegative();
      int num = value - targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    [Pure]
    public static bool IsNear(this uint value, uint targetValue, uint tolerance)
    {
      long num = (long) value - (long) targetValue;
      return num >= (long) -tolerance && num <= (long) tolerance;
    }

    /// <summary>
    /// Whether difference between <paramref name="value" /> and <paramref name="targetValue" /> is less or equal than
    /// <paramref name="tolerance" />.
    /// </summary>
    [Pure]
    public static bool IsNear(this long value, long targetValue, long tolerance)
    {
      Assert.That<long>(tolerance).IsNotNegative();
      long num = value - targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    /// <summary>
    /// Whether difference between two given values around zero is less than <see cref="F:Mafi.MafiMath.DEFAULT_FLOAT_TOLERANCE" />.
    /// If the values are not around zero please use <see cref="M:Mafi.MafiMath.IsNear(System.Single,System.Single,System.Single)" /> with custom tolerance.
    /// </summary>
    [Pure]
    public static bool IsNear(this float value, float targetValue)
    {
      if ((double) value == (double) targetValue)
        return true;
      float num = value - targetValue;
      return (double) num > -9.9999997473787516E-05 && (double) num < 9.9999997473787516E-05;
    }

    /// <summary>
    /// Whether difference between two given values is less or equal to <paramref name="tolerance" />.
    /// </summary>
    [Pure]
    public static bool IsNear(this float value, float targetValue, float tolerance)
    {
      if ((double) value == (double) targetValue)
        return true;
      float num = value - targetValue;
      return (double) num >= -(double) tolerance && (double) num <= (double) tolerance;
    }

    /// <summary>
    /// Whether difference between two given values is less or equal to <paramref name="tolerance" />.
    /// </summary>
    [Pure]
    public static bool IsNear(this double value, double targetValue, double tolerance)
    {
      if (value == targetValue)
        return true;
      double num = value - targetValue;
      return num >= -tolerance && num <= tolerance;
    }

    /// <summary>
    /// Whether magnitude of this value is less than <see cref="F:Mafi.MafiMath.DEFAULT_FLOAT_TOLERANCE" />.
    /// </summary>
    [Pure]
    public static bool IsNearZero(this float value)
    {
      return (double) value > -9.9999997473787516E-05 && (double) value < 9.9999997473787516E-05;
    }

    /// <summary>
    /// Whether given <paramref name="value" /> is not infinity nor NaN.
    /// </summary>
    [Pure]
    public static bool IsFinite(this float value)
    {
      return !float.IsInfinity(value) && !float.IsNaN(value);
    }

    [Pure]
    public static float Sqrt(this float value) => (float) Math.Sqrt((double) value);

    [Pure]
    public static float Pow(float value, float exponent)
    {
      return (float) Math.Pow((double) value, (double) exponent);
    }

    [Pure]
    public static float Exp(float value) => (float) Math.Exp((double) value);

    /// <summary>Natural logarithm of base e.</summary>
    [Pure]
    public static float Log(float value) => (float) Math.Log((double) value);

    /// <summary>Logarithm of base 10.</summary>
    [Pure]
    public static float Log10(float value) => (float) Math.Log10((double) value);

    [Pure]
    public static float Sin(float radians) => (float) Math.Sin((double) radians);

    [Pure]
    public static float Cos(float radians) => (float) Math.Cos((double) radians);

    [Pure]
    public static float Tan(float radians) => (float) Math.Tan((double) radians);

    [Pure]
    public static float Atan(float value) => (float) Math.Atan((double) value);

    [Pure]
    public static AngleDegrees1f Atan2(int y, int x)
    {
      return (Math.Atan2((double) y, (double) x) * (180.0 / Math.PI)).Degrees();
    }

    [Pure]
    public static AngleDegrees1f Atan2(long y, long x)
    {
      return (Math.Atan2((double) y, (double) x) * (180.0 / Math.PI)).Degrees();
    }

    [Pure]
    public static AngleDegrees1f Atan2(Fix32 y, Fix32 x)
    {
      return (Math.Atan2(y.ToDouble(), x.ToDouble()) * (180.0 / Math.PI)).Degrees();
    }

    [Pure]
    public static AngleDegrees1f Atan2(Fix64 y, Fix64 x)
    {
      return (Math.Atan2(y.ToDouble(), x.ToDouble()) * (180.0 / Math.PI)).Degrees();
    }

    [Pure]
    public static AngleDegrees1f AcosClamped(Fix64 x)
    {
      return (Math.Acos(x.ToDouble().Clamp(-1.0, 1.0)) * (180.0 / Math.PI)).Degrees();
    }

    /// <summary>
    /// Smoothly interpolates between <paramref name="from" /> and <paramref name="to" /> based on <paramref name="t" />. This function has zero 1st and 2nd order derivatives at t = 0 and t = 1. Parameter <paramref name="t" /> is expected to be within [0, 1] range.
    /// </summary>
    [Pure]
    public static float SmoothInterpolate(float from, float to, float t)
    {
      t = (float) ((double) t * (double) t * (double) t * ((double) t * ((double) t * 6.0 - 15.0) + 10.0));
      return from + t * (to - from);
    }

    public static Fix32 SmoothInterpolate(Fix32 from, Fix32 to, Percent t)
    {
      t = (t * (t * 6 - Percent.FromInt(15)) + Percent.FromInt(10)) * t * t * t;
      return from + (to - from).ScaledBy(t);
    }

    /// <summary>
    /// Computes floored symmetrical version of modulo. Unlike C#'s remainder operator the result will be always
    /// non-negative like a modulo in algebra. See example for clarification.
    /// </summary>
    /// <example>
    ///  <code>
    /// Modulo(4, 3) = 1  // 4 % 3 == 1
    /// Modulo(3, 3) = 0  // 3 % 3 == 0
    /// Modulo(2, 3) = 2  // 2 % 3 == 2
    /// Modulo(1, 3) = 1  // 1 % 3 == 1
    /// Modulo(0, 3) = 0  // 0 % 3 == 0
    /// Modulo(-1, 3) = 2  // -1 % 3 == -1
    /// Modulo(-2, 3) = 1  // -2 % 3 == -2
    /// Modulo(-3, 3) = 0  // -3 % 3 == 0
    /// Modulo(-4, 3) = 2  // -4 % 3 == -1
    ///  </code>
    ///  </example>
    [Pure]
    public static int Modulo(this int value, int mod)
    {
      return (int) ((((long) value * (long) mod).Abs() + (long) value) % (long) mod);
    }

    /// <summary>
    /// Computes floored symmetrical version of modulo. Unlike C#'s remainder operator the result will be always
    /// non-negative like a modulo in algebra. See <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" /> for details.
    /// </summary>
    [Pure]
    public static float Modulo(this float value, float mod)
    {
      return value - mod * (float) (value / mod).FloorToInt();
    }

    /// <summary>
    /// Computes algebraic modulo (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />) in a very fast way assuming that the modulus
    /// is a power of two. It is up the caller to ensure that.
    /// </summary>
    [Pure]
    public static int ModuloPowerOfTwo(this int value, int modPowerOfTwo)
    {
      return value & modPowerOfTwo - 1;
    }

    /// <summary>
    /// Computes ceiled division. Unlike normal division operator in C# this always rounds up. See examples below.
    /// </summary>
    /// <example>
    ///  <code>
    /// CeilDiv(5, 2) = 3    //  5 / 2 == 2
    /// CeilDiv(-5, 2) = -2  // -5 / 2 = -2
    ///  </code>
    ///  </example>
    [Pure]
    public static int CeilDiv(this int value, int div) => ((float) value / (float) div).CeilToInt();

    [Pure]
    public static int CeilDivPositive(this int value, int div) => (value + div - 1) / div;

    [Pure]
    public static long CeilDiv(this long value, long div)
    {
      return ((double) value / (double) div).CeilToLong();
    }

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down. See examples below.
    /// </summary>
    /// <example>
    /// <code>
    /// FloorDiv(5, 2) = 2    //  5 / 2 == 2
    /// FloorDiv(-5, 2) = -3  // -5 / 2 == -2
    /// </code>
    /// </example>
    [Pure]
    public static int FloorDiv(this int value, int div)
    {
      return ((float) value / (float) div).FloorToInt();
    }

    /// <summary>
    /// Computes floor division by a power of two.
    /// WARNING: Given divisor is the exponent of the base two.
    /// </summary>
    [Pure]
    public static int FloorDivByExp2(this int value, int exponent) => value >> exponent;

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down. See <see cref="M:Mafi.MafiMath.FloorDiv(System.Int32,System.Int32)" /> for details.
    /// </summary>
    [Pure]
    public static int FloorDiv(this float value, float div) => (value / div).FloorToInt();

    [Pure]
    public static int FloorDiv(this double value, double div) => (value / div).FloorToInt();

    [Pure]
    public static long FloorDiv(this long value, long div)
    {
      return ((double) value / (double) div).FloorToLong();
    }

    [Pure]
    public static int RoundDiv(this int value, int div)
    {
      return ((float) value / (float) div).RoundToInt();
    }

    [Pure]
    public static long RoundDiv(this long value, int div)
    {
      return ((double) value / (double) div).RoundToLong();
    }

    [Pure]
    public static int RoundDivToInt(this long value, int div)
    {
      return (int) ((double) value / (double) div).RoundToLong();
    }

    /// <summary>
    /// Computes arc angle from <paramref name="arcLength" /> and circle <paramref name="radius" />.
    /// </summary>
    [Pure]
    public static AngleDegrees1f ArcAngle(Fix32 arcLength, Fix32 radius)
    {
      return AngleDegrees1f.FromRadians(arcLength / radius);
    }

    /// <summary>
    /// Computes arc length from <paramref name="arcAngle" /> and circle <paramref name="radius" />.
    /// </summary>
    [Pure]
    public static Fix32 ArcLength(AngleDegrees1f arcAngle, Fix32 radius)
    {
      return arcAngle.Radians * radius;
    }

    [Pure]
    public static byte Min(this byte self, byte value) => Math.Min(self, value);

    [Pure]
    public static byte Max(this byte self, byte value) => Math.Max(self, value);

    [Pure]
    public static short Min(this short self, short value) => Math.Min(self, value);

    [Pure]
    public static short Max(this short self, short value) => Math.Max(self, value);

    [Pure]
    public static ushort Min(this ushort self, ushort value) => Math.Min(self, value);

    [Pure]
    public static ushort Max(this ushort self, ushort value) => Math.Max(self, value);

    [Pure]
    public static int Min(this int self, int value) => Math.Min(self, value);

    [Pure]
    public static int Max(this int self, int value) => Math.Max(self, value);

    [Pure]
    public static int Abs(this int self)
    {
      int num = self >> 31;
      self ^= num;
      self -= num;
      return self;
    }

    [Pure]
    public static int Sign(this int self) => Math.Sign(self);

    [Pure]
    public static int Squared(this int self) => self * self;

    [Pure]
    public static float Squared(this float self) => self * self;

    [Pure]
    public static Fix32 Sqrt(this int self) => self.ToFix32().Sqrt();

    [Pure]
    public static long Min(this long self, long value) => Math.Min(self, value);

    [Pure]
    public static long Max(this long self, long value) => Math.Max(self, value);

    [Pure]
    public static long Abs(this long self)
    {
      long num = self >> 63;
      self ^= num;
      self -= num;
      return self;
    }

    [Pure]
    public static int Sign(this long self) => Math.Sign(self);

    [Pure]
    public static int SignNoZero(this long self) => self < 0L ? -1 : 1;

    public static Fix32 Sqrt(this long self) => self.ToFix64().SqrtToFix32();

    public static Fix64 Sqrt64(this long self) => self.ToFix64().Sqrt();

    [Pure]
    public static float Min(this float self, float value) => Math.Min(self, value);

    [Pure]
    public static float Max(this float self, float value) => Math.Max(self, value);

    [Pure]
    public static float Abs(this float self) => Math.Abs(self);

    [Pure]
    public static float Average(this float x, float y) => (float) (((double) x + (double) y) / 2.0);

    [Pure]
    public static int Sign(this float self) => Math.Sign(self);

    [Pure]
    public static int SignNoZero(this float self) => (double) self >= 0.0 ? 1 : -1;

    [Pure]
    public static ulong RotateLeft(this ulong value, int n) => value << n | value >> 64 - n;

    [Pure]
    public static Fix32 CircleRadiusFromArea(Fix32 area) => (area / (Fix32.Tau / 2)).Sqrt();

    [Pure]
    public static float CircleCircumferenceFromRadius(float radius) => 6.28318548f * radius;

    [Pure]
    public static float CircleArea(float radius) => 3.14159274f * radius * radius;

    /// <summary>
    /// Smoothly interpolates value <paramref name="from" /> to value <paramref name="to" /> based on time parameter
    /// <paramref name="t" /> that must be in range [0, 1]. This version is unclamped as it does clamp <paramref name="t" /> to [0, 1] but assumes that caller ensures that.
    /// </summary>
    [Pure]
    public static float SmoothStep(float from, float to, float t)
    {
      Assert.That<float>(t).IsWithin01Incl();
      t = (float) ((double) t * (double) t * (double) t * ((double) t * ((double) t * 6.0 - 15.0) + 10.0));
      return (float) ((double) from * (1.0 - (double) t) + (double) to * (double) t);
    }

    /// <summary>
    /// Smooth transition similar to <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> that is faster at the start and slower at the end.
    /// </summary>
    [Pure]
    public static float EaseIn(float from, float to, float t)
    {
      Assert.That<float>(t).IsWithin01Incl();
      return from.Lerp(to, MafiMath.Sin((float) ((double) t * 6.2831854820251465 * 0.25)));
    }

    /// <summary>
    /// Smooth transition similar to <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> that is slower at the start and faster at the end.
    /// </summary>
    [Pure]
    public static float EaseOut(float from, float to, float t)
    {
      Assert.That<float>(t).IsWithin01Incl();
      return from.Lerp(to, 1f - MafiMath.Cos((float) ((double) t * 6.2831854820251465 * 0.25)));
    }

    /// <summary>
    /// Slow start and slow end with fast transition in the middle using bezier curve.
    /// </summary>
    [Pure]
    public static float EaseInOut(float from, float to, float t)
    {
      return from.Lerp(to, MafiMath.EaseInOut(t));
    }

    /// <summary>
    /// Slow start and slow end with fast transition in the middle using bezier curve.
    /// </summary>
    [Pure]
    public static float EaseInOut(float t)
    {
      Assert.That<float>(t).IsWithin01Incl();
      return (float) ((double) t * (double) t * (3.0 - 2.0 * (double) t));
    }

    /// <summary>
    /// Transforms values so that the function is flat around zero. This only affects values lower than 1.
    /// </summary>
    /// <remarks>
    /// This is done by transforming values from 1 to <c>1 - τ/4</c> via cosine function to make is smoothly approach
    /// zero. So <c>[1, 1 - τ/4]</c> maps to <c>[1, 0]</c> where at 0 the derivative is 0.
    /// Then, values <c>[1 - τ/4, 1 - τ/2]</c> will be mapped via cosine to <c>[0, -1]</c>, recovering the slope back
    /// to original. From <c>1 - τ/2</c> (output of -1) the function has the same shape as original, but shifted up by
    /// <c>2 - τ/2 ~ 1.14</c>.
    /// 
    /// <example>
    /// Ease in: 1 - cos(x + pi/2 - 1) for 1 - pi/2 &lt; x &lt; 1
    /// Recover: cos(x + pi/2 - 1) - 1 for 1 - pi &lt; x &lt; 1 - pi/2
    /// Shifted: x + pi - 2 for x &lt; 1 - pi
    /// </example>
    /// </remarks>
    public static Fix32 SmoothApproachZero(this Fix32 value)
    {
      if (value >= Fix32.One)
        return value;
      if (value <= MafiMath.ONE_MINUS_TAU_OVER_2)
        return value + MafiMath.TAU_OVER_2_MINUS_2;
      Fix32 fix32 = (value - MafiMath.ONE_MINUS_TAU_OVER_4).Cos();
      return !(value > MafiMath.ONE_MINUS_TAU_OVER_4) ? fix32 - Fix32.One : Fix32.One - fix32;
    }

    public static Fix32 SmoothApproachZero(this Fix32 value, Fix32 transitionSize)
    {
      return (value / transitionSize).SmoothApproachZero() * transitionSize;
    }

    /// <summary>
    /// Interpolates between <paramref name="v1" /> and <paramref name="v2" /> using cubic spline.
    /// </summary>
    [Pure]
    public static Fix32 CubicInterpolate(Fix32 v0, Fix32 v1, Fix32 v2, Fix32 v3, Percent t)
    {
      Fix32 fix32 = 2 * v0 - 5 * v1 + 4 * v2 - v3;
      Fix32 val = 3 * (v1 - v2) + v3 - v0;
      return v1 + Fix32.Half * t.Apply(v2 - v0 + t.Apply(fix32 + t.Apply(val)));
    }

    /// <summary>
    /// Pre-computes cubic interpolation coefficients for more efficient interpolation when the interpolated
    /// points are constant and only the interpolation constant changes.
    /// </summary>
    [Pure]
    public static CubicCoeffs PrecomputeCubicCoeffs(Fix32 v0, Fix32 v1, Fix32 v2, Fix32 v3)
    {
      return new CubicCoeffs(2 * v0 - 5 * v1 + 4 * v2 - v3, 3 * (v1 - v2) + v3 - v0, v1, v2 - v0);
    }

    /// <summary>Warning: Not deterministic!</summary>
    [Pure]
    public static float CubicInterpolate(float v0, float v1, float v2, float v3, float t)
    {
      float num1 = (float) (2.0 * (double) v0 - 5.0 * (double) v1 + 4.0 * (double) v2) - v3;
      float num2 = (float) (3.0 * ((double) v1 - (double) v2)) + v3 - v0;
      return v1 + (float) (0.5 * (double) t * ((double) v2 - (double) v0 + (double) t * ((double) num1 + (double) t * (double) num2)));
    }

    [Pure]
    public static Fix32 MonotoneCubicInterpolate(
      Fix32 v0,
      Fix32 v1,
      Fix32 v2,
      Fix32 v3,
      Percent t)
    {
      Fix32 fix32 = v1 - v0;
      Fix32 rhs1 = v2 - v1;
      Fix32 rhs2 = v3 - v2;
      Fix64 fix64_1 = fix32.MultAsFix64(rhs1);
      Fix64 fix64_2 = fix64_1.IsNotPositive ? Fix64.Zero : fix64_1 / (fix32 + rhs1);
      Fix64 fix64_3 = rhs1.MultAsFix64(rhs2);
      Fix64 fix64_4 = fix64_3.IsNotPositive ? Fix64.Zero : fix64_3 / (rhs1 + rhs2);
      Fix64 val = fix64_2 + fix64_4 - rhs1 - rhs1;
      Fix64 fix64_5 = rhs1 - fix64_2 - val;
      return v1 + t.Apply(fix64_2 + t.Apply(fix64_5 + t.Apply(val))).ToFix32();
    }

    /// <summary>
    /// Pre-computes cubic interpolation coefficients for more efficient interpolation when the interpolated
    /// points are constant and only the interpolation constant changes.
    /// </summary>
    [Pure]
    public static MonotoneCubicCoeffs PrecomputeMonotoneCubicCoeffs(
      Fix32 v0,
      Fix32 v1,
      Fix32 v2,
      Fix32 v3)
    {
      Fix32 fix32 = v1 - v0;
      Fix32 rhs1 = v2 - v1;
      Fix32 rhs2 = v3 - v2;
      Fix64 fix64_1 = fix32.MultAsFix64(rhs1);
      Fix64 c1 = fix64_1.IsNotPositive ? Fix64.Zero : fix64_1 / (fix32 + rhs1);
      Fix64 fix64_2 = rhs1.MultAsFix64(rhs2);
      Fix64 fix64_3 = fix64_2.IsNotPositive ? Fix64.Zero : fix64_2 / (rhs1 + rhs2);
      Fix64 c3 = c1 + fix64_3 - rhs1 - rhs1;
      Fix64 c2 = rhs1 - c1 - c3;
      return new MonotoneCubicCoeffs(v1, c1, c2, c3);
    }

    /// <summary>
    /// Returns index of set least significant bit in given number.
    /// Returns value 32 if this is called on zero.
    /// </summary>
    [Pure]
    public static int GetFirstSetBitIndex(this uint x)
    {
      if (x != 0U)
        return (int) MafiMath.s_multiplyDeBruijnBitPosition32[((int) x & (int) -x) * 125613361 >>> 27];
      Mafi.Log.Error("Function `GetFirstSetBitIndex` called on value 0.");
      return 32;
    }

    /// <summary>
    /// Returns trailing zero bits count, or an index (from LSB) of the first set bit.
    /// Returns value 64 if this is called on zero.
    /// </summary>
    [Pure]
    public static int GetFirstSetBitIndex(this ulong x)
    {
      int firstSetBitIndex = 0;
      if (((long) x & (long) uint.MaxValue) == 0L)
      {
        firstSetBitIndex += 32;
        x >>= 32;
      }
      if (((long) x & (long) ushort.MaxValue) == 0L)
      {
        firstSetBitIndex += 16;
        x >>= 16;
      }
      if (((long) x & (long) byte.MaxValue) == 0L)
      {
        firstSetBitIndex += 8;
        x >>= 8;
      }
      if (((long) x & 15L) == 0L)
      {
        firstSetBitIndex += 4;
        x >>= 4;
      }
      if (((long) x & 3L) == 0L)
      {
        firstSetBitIndex += 2;
        x >>= 2;
      }
      if (((long) x & 1L) == 0L)
        ++firstSetBitIndex;
      if (x == 0UL)
      {
        Mafi.Log.Error("Function `GetFirstSetBitIndex` called on value 0.");
        ++firstSetBitIndex;
      }
      return firstSetBitIndex;
    }

    [Pure]
    public static string AsBin(this ulong value) => Convert.ToString((long) value, 2);

    [Pure]
    public static string AsBin(this ulong value, int padLeft)
    {
      return Convert.ToString((long) value, 2).PadLeft(padLeft, '0');
    }

    /// <summary>
    /// Iterates a discrete points on a circle with given radius. A circle with zero radius will have one point.
    /// 
    /// Uses modified Midpoint algorithm: https://en.wikipedia.org/wiki/Midpoint_circle_algorithm
    /// </summary>
    public static void IterateCirclePoints(int radius, Action<int, int> action)
    {
      Assert.That<int>(radius).IsNotNegative();
      if (radius <= 0)
      {
        if (radius == 0)
          action(0, 0);
        else
          Assert.Fail("Radius expected not negative.");
      }
      else
      {
        int num1 = radius;
        int num2 = 0;
        int num3 = 0;
        while (num1 > num2)
        {
          action(num1, num2);
          action(num2, num1);
          action(-num1, num2);
          action(-num2, -num1);
          if (num2 > 0)
          {
            action(-num2, num1);
            action(-num1, -num2);
            action(num2, -num1);
            action(num1, -num2);
          }
          if (num3 <= 0)
          {
            ++num2;
            num3 += 2 * num2 + 1;
          }
          if (num3 > 0)
          {
            --num1;
            num3 -= 2 * num1 + 1;
          }
        }
        if (num1 != num2)
          return;
        action(num1, num1);
        action(-num1, num1);
        action(-num1, -num1);
        action(num1, -num1);
      }
    }

    /// <summary>
    /// Computes the number of set bits in the given 64 bit integer using only 12 arithmetic operations.
    /// </summary>
    public static int CountSetBits(this ulong x)
    {
      x -= x >> 1 & 6148914691236517205UL;
      x = (ulong) (((long) x & 3689348814741910323L) + ((long) (x >> 2) & 3689348814741910323L));
      x = (ulong) ((long) x + (long) (x >> 4) & 1085102592571150095L);
      return (int) (x * 72340172838076673UL >> 56);
    }

    /// <summary>
    /// Computes the number of set bits in the given 32 bit integer using only 12 arithmetic operations.
    /// </summary>
    public static int CountSetBits(this uint x)
    {
      x -= x >> 1 & 1431655765U;
      x = (uint) (((int) x & 858993459) + ((int) (x >> 2) & 858993459));
      x = (uint) ((int) x + (int) (x >> 4) & 252645135);
      return (int) (x * 16843009U >> 24);
    }

    public static Fix32 FastSqrtSmallInt(this int smallInt)
    {
      return smallInt >= MafiMath.SMALL_SQRT_CACHE.Length ? smallInt.Sqrt() : MafiMath.SMALL_SQRT_CACHE[smallInt];
    }

    /// <summary>
    /// Divides 128-bit unsigned integer represented as two 64-bit unsigned integers.
    /// </summary>
    public static bool TryDiv128To64(
      ulong dividendHi,
      ulong dividendLo,
      ulong divisor,
      out ulong result)
    {
      if (dividendHi >= divisor)
      {
        result = ulong.MaxValue;
        return false;
      }
      int num1 = MafiMath.LeadingZerosBinaryCount(divisor);
      if (num1 != 0)
      {
        divisor <<= num1;
        dividendHi <<= num1;
        dividendHi |= dividendLo >> 64 - num1;
        dividendLo <<= num1;
      }
      ulong num2 = dividendHi / (divisor >> 32);
      ulong num3 = dividendHi % (divisor >> 32);
      while (num2 >> 32 != 0UL || (ulong) (((long) num2 & (long) uint.MaxValue) * ((long) divisor & (long) uint.MaxValue)) > (num3 << 32 | dividendLo >> 32))
      {
        --num2;
        num3 += divisor >> 32;
        if (num3 >> 32 != 0UL)
          break;
      }
      ulong num4 = num2 & (ulong) uint.MaxValue;
      ulong num5 = (ulong) (((long) dividendHi << 32 | (long) (dividendLo >> 32)) - (long) num4 * (long) divisor);
      ulong num6 = num5 / (divisor >> 32);
      ulong num7 = num5 % (divisor >> 32);
      while (num6 >> 32 != 0UL || (ulong) (((long) num6 & (long) uint.MaxValue) * ((long) divisor & (long) uint.MaxValue)) > (ulong) ((long) num7 << 32 | (long) dividendLo & (long) uint.MaxValue))
      {
        --num6;
        num7 += divisor >> 32;
        if (num7 >> 32 != 0UL)
          break;
      }
      ulong num8 = num6 & (ulong) uint.MaxValue;
      result = num4 << 32 | num8;
      return true;
    }

    /// <summary>
    /// Multiplier two 64-bit unsigned integers to one 128-bit integer represented as two 64-bit values.
    /// </summary>
    public static void Mul64To128(ulong x, ulong y, out ulong resultHi, out ulong resultLo)
    {
      ulong num1 = (ulong) (uint) x;
      ulong num2 = x >> 32;
      ulong num3 = (ulong) (uint) y;
      ulong num4 = y >> 32;
      ulong num5 = num2 * num4;
      ulong num6 = num1 * num4;
      ulong num7 = num2 * num3;
      ulong num8 = num1 * num3;
      ulong num9 = (ulong) ((long) num7 + (long) (num8 >> 32) + ((long) num6 & (long) uint.MaxValue));
      resultHi = num5 + (num9 >> 32) + (num6 >> 32);
      resultLo = (ulong) ((long) num9 << 32 | (long) num8 & (long) uint.MaxValue);
    }

    /// <summary>
    /// Adds a 64-bit value to 128-bit integer represented as two 64-bit values.
    /// </summary>
    public static void Add64To128(ulong add, ref ulong hi, ref ulong lo)
    {
      ulong num = lo + add;
      if (num < lo)
        ++hi;
      lo = num;
    }

    /// <summary>Returns the number of leading zeros in binary form.</summary>
    public static int LeadingZerosBinaryCount(ulong x)
    {
      if (x == 0UL)
        return 64;
      int num = 0;
      if (x <= (ulong) uint.MaxValue)
      {
        num += 32;
        x <<= 32;
      }
      if (x <= 281474976710655UL)
      {
        num += 16;
        x <<= 16;
      }
      if (x <= 72057594037927935UL)
      {
        num += 8;
        x <<= 8;
      }
      if (x <= 1152921504606846975UL)
      {
        num += 4;
        x <<= 4;
      }
      if (x <= 4611686018427387903UL)
      {
        num += 2;
        x <<= 2;
      }
      if (x <= (ulong) long.MaxValue)
        ++num;
      return num;
    }

    /// <summary>
    /// Packs two positive floats in range [0, 8) to one. Each packed float has 1/256 precision.
    /// </summary>
    public static float PackTwoSmallPositiveFloats(float x, float y)
    {
      return x * 0.125f + (float) (int) ((double) y * 256.0);
    }

    /// <summary>
    /// Unpacks float from <see cref="M:Mafi.MafiMath.PackTwoSmallPositiveFloats(System.Single,System.Single)" />.
    /// </summary>
    public static void UnPackTwoSmallPositiveFloats(this float value, out float x, out float y)
    {
      float num = (float) (int) value;
      x = (float) (((double) value - (double) num) * 8.0);
      y = num / 256f;
    }

    /// <summary>
    /// Converts perspective camera's field of view and object size (along that FOV) to distance from the
    /// center of the object so that the object will span the entire FOV of the camera.
    /// </summary>
    public static float SizeAlongFovToDistance(float sizeAlongFov, float fovDeg)
    {
      return sizeAlongFov * 0.5f / MafiMath.Tan(fovDeg * ((float) Math.PI / 360f));
    }

    public static float DistanceToSizeAlongFov(float distance, float fovDeg)
    {
      return 2f * distance * MafiMath.Tan(fovDeg * ((float) Math.PI / 360f));
    }

    /// <summary>
    /// Returns a sum of first <paramref name="n" /> elements in geometric series <c>x + x * r + x * r^2, ...</c>
    /// </summary>
    public static Percent GeometricSum(Percent firstElement, Percent ratio, int n)
    {
      return firstElement * (Percent.Hundred - ratio.Pow(n)) / (Percent.Hundred - ratio);
    }

    public static Fix32 Sigmoid(this Fix32 value)
    {
      return (1.0 / (Math.Exp(-value.ToDouble()) + 1.0)).ToFix32();
    }

    public static ulong GetRngSeed(this int value) => ((ulong) (uint) value).GetRngSeed();

    public static ulong GetRngSeed(this string value)
    {
      switch (value)
      {
        case null:
          return 165743654654265;
        case "":
          return 865225341654035425;
        default:
          ulong state = (ulong) (uint) value.Length | (ulong) value[0] << 32;
          return SplitMix64Generator.NextUlongStateless(ref state);
      }
    }

    public static ulong GetRngSeed(this ulong value)
    {
      return SplitMix64Generator.NextUlongStateless(ref value);
    }

    public static RngSeed64 GetRngSeedFromCurrentTime()
    {
      return new RngSeed64(((ulong) Stopwatch.GetTimestamp()).GetRngSeed());
    }

    /// <summary>
    /// Returns the radius of a circle that intersects all three given points. If the given points lie on a line,
    /// <see cref="F:Mafi.Fix32.MaxValue" /> is returned.
    /// </summary>
    public static Fix32 FindCircle(Vector2f p1, Vector2f p2, Vector2f p3, out Vector2f center)
    {
      Vector2f vector2f = p1 - p2;
      Vector2f other = p1 - p3;
      Fix64 fix64_1 = p1.X.Squared();
      Fix64 fix64_2 = p1.Y.Squared();
      Fix64 fix64_3 = fix64_1 - p2.X.Squared();
      Fix64 fix64_4 = fix64_2 - p2.Y.Squared();
      Fix64 fix64_5 = fix64_1 - p3.X.Squared();
      Fix64 fix64_6 = fix64_2 - p3.Y.Squared();
      Fix64 times2Fast = vector2f.PseudoCross(other).Times2Fast;
      if (times2Fast.IsNearZero())
      {
        center = p1;
        return Fix32.MaxValue;
      }
      Fix32 fix32_1 = (fix64_3 * other.Y + fix64_4 * other.Y - fix64_5 * vector2f.Y - fix64_6 * vector2f.Y).DivToFix32(times2Fast);
      Fix32 fix32_2 = (fix64_5 * vector2f.X + fix64_6 * vector2f.X - fix64_3 * other.X - fix64_4 * other.X).DivToFix32(times2Fast);
      center = new Vector2f(fix32_1, fix32_2);
      return (fix64_1 + fix64_2 + fix32_2.Squared() + fix32_1.Squared() - (fix32_2 * p1.Y + fix32_1 * p1.X).Times2Fast).SqrtToFix32();
    }

    static MafiMath()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      MafiMath.ROUNDING_STRS = new string[8]
      {
        "0.0",
        "0.00",
        "0.000",
        "0.0000",
        "0.00000",
        "0.000000",
        "0.0000000",
        "0.00000000"
      };
      MafiMath.SMALL_SQRT_CACHE = new Fix32[65]
      {
        (Fix32) 0,
        (Fix32) 1,
        2.Sqrt(),
        3.Sqrt(),
        (Fix32) 2,
        5.Sqrt(),
        6.Sqrt(),
        7.Sqrt(),
        8.Sqrt(),
        (Fix32) 3,
        10.Sqrt(),
        11.Sqrt(),
        12.Sqrt(),
        13.Sqrt(),
        14.Sqrt(),
        15.Sqrt(),
        (Fix32) 4,
        17.Sqrt(),
        18.Sqrt(),
        19.Sqrt(),
        20.Sqrt(),
        21.Sqrt(),
        22.Sqrt(),
        23.Sqrt(),
        24.Sqrt(),
        (Fix32) 5,
        26.Sqrt(),
        27.Sqrt(),
        28.Sqrt(),
        29.Sqrt(),
        30.Sqrt(),
        31.Sqrt(),
        32.Sqrt(),
        33.Sqrt(),
        34.Sqrt(),
        35.Sqrt(),
        (Fix32) 6,
        37.Sqrt(),
        38.Sqrt(),
        39.Sqrt(),
        40.Sqrt(),
        41.Sqrt(),
        42.Sqrt(),
        43.Sqrt(),
        44.Sqrt(),
        45.Sqrt(),
        46.Sqrt(),
        47.Sqrt(),
        48.Sqrt(),
        (Fix32) 7,
        50.Sqrt(),
        51.Sqrt(),
        52.Sqrt(),
        53.Sqrt(),
        54.Sqrt(),
        55.Sqrt(),
        56.Sqrt(),
        57.Sqrt(),
        58.Sqrt(),
        59.Sqrt(),
        60.Sqrt(),
        61.Sqrt(),
        62.Sqrt(),
        63.Sqrt(),
        (Fix32) 8
      };
      MafiMath.ONE_MINUS_TAU_OVER_4 = Fix32.One - Fix32.Tau / 4;
      MafiMath.ONE_MINUS_TAU_OVER_2 = Fix32.One - Fix32.Tau / 2;
      MafiMath.TAU_OVER_2_MINUS_2 = Fix32.Tau / 2 - Fix32.Two;
      MafiMath.s_multiplyDeBruijnBitPosition32 = new byte[32]
      {
        (byte) 0,
        (byte) 1,
        (byte) 28,
        (byte) 2,
        (byte) 29,
        (byte) 14,
        (byte) 24,
        (byte) 3,
        (byte) 30,
        (byte) 22,
        (byte) 20,
        (byte) 15,
        (byte) 25,
        (byte) 17,
        (byte) 4,
        (byte) 8,
        (byte) 31,
        (byte) 27,
        (byte) 13,
        (byte) 23,
        (byte) 21,
        (byte) 19,
        (byte) 16,
        (byte) 7,
        (byte) 26,
        (byte) 12,
        (byte) 18,
        (byte) 6,
        (byte) 11,
        (byte) 5,
        (byte) 10,
        (byte) 9
      };
    }
  }
}
