// Decompiled with JetBrains decompiler
// Type: Mafi.IRandomExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Text;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// This extensions class provides convenience methods for all implementations of <see cref="T:Mafi.IRandom" /> interface
  /// without enforcing the implementation to the base classes.
  /// </summary>
  public static class IRandomExtensions
  {
    /// <summary>Seeds this generator with a string.</summary>
    public static void Seed(this IRandom random, string seed)
    {
      Assert.That<string>(seed).IsNotNullOrEmpty();
      random.Seed(Encoding.UTF8.GetBytes(seed));
    }

    /// <summary>Seeds this generator with pne or two ints.</summary>
    public static void SeedFast(this IRandom random, int seed1, int seed2 = 0)
    {
      random.SeedFast((ulong) (uint) seed1 | (ulong) (uint) seed2 << 32);
    }

    /// <summary>Seeds this generator with three or four ints.</summary>
    public static void SeedFast(this IRandom random, int seed1, int seed2, int seed3, int seed4 = 0)
    {
      random.SeedFast((ulong) (uint) seed1 | (ulong) (uint) seed2 << 32, (ulong) (uint) seed3 | (ulong) (uint) seed4 << 32);
    }

    public static sbyte NextSByte(this IRandom random) => (sbyte) random.NextUlong();

    public static byte NextByte(this IRandom random) => (byte) random.NextUlong();

    public static short NextShort(this IRandom random) => (short) random.NextUlong();

    public static ushort NextUShort(this IRandom random) => (ushort) random.NextUlong();

    /// <summary>
    /// Returns pseudo-random uniformly distributed 32-bit unsigned value.
    /// </summary>
    /// <remarks>Note that this can return value 0 even if the original random generator cannot return 0.</remarks>
    public static uint NextUint(this IRandom random) => (uint) random.NextUlong();

    /// <summary>
    /// Returns pseudo-random uniformly distributed 32-bit signed value.
    /// </summary>
    /// <remarks>Note that this can return value 0 even if the original random generator cannot return 0.</remarks>
    public static int NextInt(this IRandom random) => (int) random.NextUlong();

    /// <summary>
    /// Returns pseudo-random uniformly distributed non-negative 31-bit value (sign bit is always 0).
    /// </summary>
    /// <remarks>Note that this can return value 0 even if the original random generator cannot return 0.</remarks>
    public static int NextIntNotNegative(this IRandom random)
    {
      return (int) ((long) random.NextUlong() & (long) int.MaxValue);
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed value between 0 (inclusive) and <paramref name="maxValueExcl" />
    /// (exclusive).
    /// </summary>
    public static int NextInt(this IRandom random, int maxValueExcl)
    {
      return (int) random.NextLong((long) maxValueExcl);
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed value between <paramref name="minValueIncl" /> (inclusive) and
    /// <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static int NextInt(this IRandom random, int minValueIncl, int maxValueExcl)
    {
      return (int) random.NextLong((long) minValueIncl, (long) maxValueExcl);
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed value between <paramref name="minValueIncl" /> (inclusive) and
    /// <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static uint NextUint(this IRandom random, uint minValueIncl, uint maxValueExcl)
    {
      return (uint) random.NextLong((long) minValueIncl, (long) maxValueExcl);
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed non-negative 63-bit value (sign bit is always 0).
    /// </summary>
    /// <remarks>Note that this can return value 0 even if the original random generator cannot return 0.</remarks>
    public static long NextLong(this IRandom random) => (long) (random.NextUlong() >> 1);

    public static long NextLong(this IRandom random, long maxValueExcl)
    {
      return (long) (random.NextDouble() * (double) maxValueExcl);
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed value between <paramref name="minValueIncl" /> (inclusive) and
    /// <paramref name="maxValueExcl" /> (exclusive). Note that this function cannot handle entire range of long. For
    /// ranges larger than 53 bits some values may not be possible to obtain.
    /// </summary>
    public static long NextLong(this IRandom random, long minValueIncl, long maxValueExcl)
    {
      Assert.That<long>(minValueIncl).IsLessOrEqual(maxValueExcl);
      Assert.That<bool>(minValueIncl != long.MinValue || maxValueExcl != long.MaxValue).IsTrue("This overload cannot handle full range of longs.");
      long num = maxValueExcl - minValueIncl;
      return (long) (random.NextDouble() * (double) num) + minValueIncl;
    }

    public static RngSeed64 NextSeed64(this IRandom random) => new RngSeed64(random.NextUlong());

    /// <summary>
    /// Returns pseudo-random uniformly distributed single-precision floating point value in range [0, 1), that is
    /// zero inclusive and one exclusive.
    /// </summary>
    public static float NextFloat(this IRandom random)
    {
      return (float) ((uint) random.NextUlong() & 4294967040U) * 2.32830644E-10f;
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed single-precision floating point value in between <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static float NextFloat(this IRandom random, float minValueIncl, float maxValueExcl)
    {
      Assert.That<float>(minValueIncl).IsLessOrEqual(maxValueExcl);
      return random.NextFloat() * (maxValueExcl - minValueIncl) + minValueIncl;
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed double-precision floating point value in range [0, 1), that is
    /// zero inclusive and one exclusive.
    /// </summary>
    public static double NextDouble(this IRandom random)
    {
      return (double) (random.NextUlong() & 18446744073709549568UL) * 5.4210108624275222E-20;
    }

    /// <summary>
    /// Returns pseudo-random uniformly distributed double-precision floating point value in between <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static double NextDouble(this IRandom random, double minValueIncl, double maxValueExcl)
    {
      Assert.That<bool>(minValueIncl <= maxValueExcl).IsTrue();
      return random.NextDouble() * (maxValueExcl - minValueIncl) + minValueIncl;
    }

    /// <summary>Returns true 50% of a time.</summary>
    public static bool NextBool(this IRandom random) => random.NextUlong() < (ulong) long.MaxValue;

    /// <summary>
    /// Returns true <paramref name="probability" />% of times (by random). While probability should be between 0 and
    /// 1, it can be out of range and the results are as expected - returns always false for probability below 0 and
    /// always true for probability above 1.
    /// </summary>
    public static bool TestProbability(this IRandom random, float probability)
    {
      return (double) random.NextFloat() < (double) probability;
    }

    public static bool TestProbability(this IRandom random, Percent probability)
    {
      return random.NextPercent() < probability;
    }

    /// <summary>
    /// Note that Fix32 has only 10 bits for values in between 0 and 1.
    /// </summary>
    public static bool TestProbability(this IRandom random, Fix32 probability)
    {
      return random.NextFix32Between01() < probability;
    }

    /// <summary>
    /// Rounds up with the probability equal to fractional part.
    /// </summary>
    public static int RoundProbabilistically(this IRandom random, Fix32 value)
    {
      Fix32 fix32 = value.Floored();
      return random.TestProbability(value - fix32) ? (fix32 + Fix32.One).ToIntFloored() : fix32.ToIntFloored();
    }

    /// <summary>
    /// Fills given buffer with random data. Consider using more effective <see cref="M:Mafi.IRandomExtensions.Generate(Mafi.IRandom,System.UInt32[])" />
    /// or <see cref="M:Mafi.IRandomExtensions.Generate(Mafi.IRandom,System.UInt64[])" />.
    /// </summary>
    public static void Generate(this IRandom random, byte[] buffer)
    {
      int num1 = buffer.Length / 8;
      int index1 = 0;
      int num2 = 0;
      while (num2 < num1)
      {
        ulong num3 = random.NextUlong();
        buffer[index1] = (byte) num3;
        buffer[index1 + 1] = (byte) (num3 >> 8);
        buffer[index1 + 2] = (byte) (num3 >> 16);
        buffer[index1 + 3] = (byte) (num3 >> 24);
        buffer[index1 + 4] = (byte) (num3 >> 32);
        buffer[index1 + 5] = (byte) (num3 >> 40);
        buffer[index1 + 6] = (byte) (num3 >> 48);
        buffer[index1 + 7] = (byte) (num3 >> 56);
        ++num2;
        index1 += 8;
      }
      int num4 = buffer.Length - num1 * 8;
      Assert.That<int>(num4).IsLess(8);
      ulong num5 = random.NextUlong();
      for (int index2 = 0; index2 < num4; ++index2)
        buffer[index1 + index2] = (byte) (num5 >> 8 * index2);
    }

    /// <summary>Fills given buffer with random data.</summary>
    public static void Generate(this IRandom random, ushort[] buffer)
    {
      int num1 = buffer.Length / 4;
      int index1 = 0;
      int num2 = 0;
      while (num2 < num1)
      {
        ulong num3 = random.NextUlong();
        buffer[index1] = (ushort) num3;
        buffer[index1 + 1] = (ushort) (num3 >> 16);
        buffer[index1 + 2] = (ushort) (num3 >> 32);
        buffer[index1 + 3] = (ushort) (num3 >> 48);
        ++num2;
        index1 += 4;
      }
      int num4 = buffer.Length - num1 * 4;
      Assert.That<int>(num4).IsLess(4);
      ulong num5 = random.NextUlong();
      for (int index2 = 0; index2 < num4; ++index2)
        buffer[index1 + index2] = (ushort) (num5 >> 16 * index2);
    }

    /// <summary>Fills given buffer with random data.</summary>
    public static void Generate(this IRandom random, uint[] buffer)
    {
      int num1 = buffer.Length / 2;
      int index = 0;
      int num2 = 0;
      while (num2 < num1)
      {
        ulong num3 = random.NextUlong();
        buffer[index] = (uint) num3;
        buffer[index + 1] = (uint) (num3 >> 32);
        ++num2;
        index += 2;
      }
      if (buffer.Length - num1 * 2 != 1)
        return;
      buffer[buffer.Length + 1] = random.NextUint();
    }

    /// <summary>
    /// Fills given buffer with random data. This method is very effective, more effective than <see cref="M:Mafi.IRandomExtensions.Generate(Mafi.IRandom,System.Byte[])" /> overload.
    /// </summary>
    public static void Generate(this IRandom random, ulong[] buffer)
    {
      int length = buffer.Length;
      for (int index = 0; index < length; ++index)
        buffer[index] = random.NextUlong();
    }

    /// <summary>Randomly shuffles given array.</summary>
    public static void Shuffle<T>(this IRandom random, T[] array)
    {
      random.Shuffle<T>(array, 0, array.Length);
    }

    /// <summary>Randomly shuffles given array.</summary>
    public static void Shuffle<T>(
      this IRandom random,
      T[] array,
      int startIndexIncl,
      int endIndexExcl)
    {
      for (int index1 = endIndexExcl - 1; index1 > startIndexIncl; --index1)
      {
        int index2 = random.NextInt(startIndexIncl, index1 + 1);
        T obj = array[index1];
        array[index1] = array[index2];
        array[index2] = obj;
      }
    }

    /// <summary>
    /// Returns a random element from the given collection or default when collection is empty.
    /// </summary>
    /// <returns>Returns default value (null) if the collection is empty.</returns>
    public static T GetRandomElementOrDefault<T>(this IRandom random, IIndexable<T> collection)
    {
      if (collection.Count == 0)
        return default (T);
      int index = random.NextInt(0, collection.Count);
      return collection[index];
    }

    /// <summary>
    /// Returns pseudo-random single-precision floating point value from normal distribution with mean 0 and std.
    /// deviation 1.
    /// </summary>
    public static Percent NextGaussian(this IRandom random) => random.NextGaussians().First;

    /// <summary>
    /// Returns pseudo-random single-precision floating point value from normal distribution with given mean and
    /// std. deviation.
    /// </summary>
    public static Percent NextGaussian(this IRandom random, Percent mean, Percent stdDev)
    {
      return mean + stdDev * random.NextGaussian();
    }

    /// <summary>
    /// Returns two independent pseudo-random single-precision floating point value from normal distribution with
    /// given mean 0 and std. deviation 1.
    /// </summary>
    public static Pair<Percent, Percent> NextGaussians(this IRandom random)
    {
      int num = 0;
      do
      {
        Fix64 fix64_1 = random.NextFix64Between01().Times2Fast - Fix64.One;
        Fix64 fix64_2 = random.NextFix64Between01().Times2Fast - Fix64.One;
        Fix64 fix64_3 = fix64_1.Squared() + fix64_2.Squared();
        if (!(fix64_3 >= Fix64.One) && !(fix64_3 <= Fix64.EpsilonNear))
        {
          Fix64 fix64_4 = (-fix64_3.LogNatural().Times2Fast / fix64_3).Sqrt();
          return Pair.Create<Percent, Percent>(Percent.FromFix64(fix64_1 * fix64_4), Percent.FromFix64(fix64_2 * fix64_4));
        }
      }
      while (++num < 20);
      Log.Error("Failed to generate random normally distributed value");
      return Pair.Create<Percent, Percent>(Percent.Zero, Percent.Zero);
    }

    public static Pair<float, float> NextGaussiansFloats(this IRandom random)
    {
      int num1 = 0;
      do
      {
        float num2 = (float) (2.0 * (double) random.NextFloat() - 1.0);
        float num3 = (float) (2.0 * (double) random.NextFloat() - 1.0);
        float num4 = (float) ((double) num2 * (double) num2 + (double) num3 * (double) num3);
        if ((double) num4 < 1.0 && (double) num4 > 0.0)
        {
          float num5 = (-2f * MafiMath.Log(num4) / num4).Sqrt();
          return Pair.Create<float, float>(num2 * num5, num3 * num5);
        }
      }
      while (++num1 < 20);
      Log.Error("Failed to generate random normally distributed value");
      return Pair.Create<float, float>(0.0f, 0.0f);
    }

    /// <summary>
    /// Returns pseudo-random single-precision floating point value from normal distribution with given and std.
    /// deviation. Returned value will never be outside of -<paramref name="maxDeviation" /> and <paramref name="maxDeviation" />.
    /// </summary>
    public static Percent NextGaussianTrunc(this IRandom random, Percent maxDeviation)
    {
      if (maxDeviation.IsZero)
        return Percent.Zero;
      if (maxDeviation < Percent.Fifty)
      {
        Assert.Fail(string.Format("Max deviation {0} must be greater than 0.5! This method uses sample rejection ", (object) maxDeviation) + "sampling and it is not effective for smaller deviations!");
        maxDeviation = Percent.Fifty;
      }
      int num = 0;
      do
      {
        Pair<Percent, Percent> pair = random.NextGaussians();
        if (pair.First >= -maxDeviation && pair.First <= maxDeviation)
          return pair.First;
        if (pair.Second >= -maxDeviation && pair.Second <= maxDeviation)
          return pair.Second;
      }
      while (++num < 20);
      Log.Error("Failed to generate random normally distributed value within specified deviation.");
      return Percent.Zero;
    }

    /// <summary>
    /// Returns a sample from a circle by choosing random angle and radius. This sampling is not uniform and is
    /// biased toward the centers.
    /// </summary>
    public static Vector2f SampleCircleCenterBiased(
      this IRandom random,
      Vector2f center,
      Fix32 minRadius,
      Fix32 maxRadius)
    {
      Fix32 fix32 = random.NextFix32(minRadius, maxRadius);
      AngleDegrees1f angleDegrees1f = random.NextAngle();
      return center + fix32 * angleDegrees1f.DirectionVector;
    }

    public static int IndexFor<T>(this IRandom random, IIndexable<T> indexable)
    {
      int count = indexable.Count;
      return count > 0 ? random.NextInt(count) : throw new InvalidOperationException("Getting random index of an empty " + indexable.GetType().Name + "<" + typeof (T).Name + ">.");
    }
  }
}
