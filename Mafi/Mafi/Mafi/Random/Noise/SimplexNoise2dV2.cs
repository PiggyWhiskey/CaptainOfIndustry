// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dV2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  /// <summary>
  /// Simplex 2D noise. It is slightly better than Perlin noise because of less apparent structure and higher
  /// performance (in higher dimensions).
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class SimplexNoise2dV2 : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Lookup table of 32 gradient vectors.</summary>
    /// <remarks>
    /// Generated with:
    /// <code>
    /// Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    /// int size = 32;
    /// for (int i = 0; i &lt; size; ++i) {
    /// 	double angle = 2.0 * Math.PI * (i + 0.5) / size;
    /// 	double angleDeg = angle / Math.PI * 180;
    /// 	double x = Math.Cos(angle);
    /// 	double y = Math.Sin(angle);
    /// 	Console.WriteLine(
    /// 		"(Fix64.FromDouble({0:0.0000000}), Fix64.FromDouble({1:0.0000000})), // {2:#.00}°",
    /// 		x, y, angleDeg);
    /// }
    /// </code>
    /// </remarks>
    private static readonly (Fix64, Fix64)[] GRADS;
    /// <summary>
    /// Permutation table for simplex noise. This could be re-shuffled for each nose, but we choose to save memory
    /// and use the same table for all noise instances.
    /// </summary>
    private static readonly byte[] PERM_TABLE;
    private static readonly Fix64 F2;
    private static readonly Fix64 G2;
    private static readonly Fix64 TWO_G2_MINUS_ONE;
    public readonly SimplexNoise2dSeed64 Seed;
    [DoNotSave(0, null)]
    private Fix64 m_mean64;
    [DoNotSave(0, null)]
    private Fix64 m_amplitudeScaled64;

    public static void Serialize(SimplexNoise2dV2 value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SimplexNoise2dV2>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SimplexNoise2dV2.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.Amplitude, writer);
      Fix32.Serialize(this.MeanValue, writer);
      Fix32.Serialize(this.Period, writer);
      SimplexNoise2dSeed64.Serialize(this.Seed, writer);
    }

    public static SimplexNoise2dV2 Deserialize(BlobReader reader)
    {
      SimplexNoise2dV2 simplexNoise2dV2;
      if (reader.TryStartClassDeserialization<SimplexNoise2dV2>(out simplexNoise2dV2))
        reader.EnqueueDataDeserialization((object) simplexNoise2dV2, SimplexNoise2dV2.s_deserializeDataDelayedAction);
      return simplexNoise2dV2;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.Amplitude = Fix32.Deserialize(reader);
      this.MeanValue = Fix32.Deserialize(reader);
      this.Period = Fix32.Deserialize(reader);
      reader.SetField<SimplexNoise2dV2>(this, "Seed", (object) SimplexNoise2dSeed64.Deserialize(reader));
      this.initAfterLoad();
    }

    private static (Fix64, Fix64) getGrad(int index) => SimplexNoise2dV2.GRADS[index & 31];

    private static int getPerm(int index)
    {
      return (int) SimplexNoise2dV2.PERM_TABLE[index & (int) byte.MaxValue];
    }

    public Fix32 MeanValue { get; private set; }

    public Fix32 Amplitude { get; private set; }

    public Fix32 Period { get; private set; }

    public SimplexNoise2dV2(SimplexNoise2dParams parameters, SimplexNoise2dSeed64 seed)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(parameters.MeanValue, parameters.Amplitude, parameters.Period, seed);
    }

    public SimplexNoise2dV2(
      Fix32 meanValue,
      Fix32 amplitude,
      Fix32 period,
      SimplexNoise2dSeed64 seed)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Seed = seed.CheckIsValid();
      this.MeanValue = meanValue;
      this.Amplitude = amplitude;
      this.Period = period.CheckPositive();
      this.initAfterLoad();
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initAfterLoad()
    {
      this.m_mean64 = this.MeanValue.ToFix64();
      this.m_amplitudeScaled64 = this.Amplitude.ToFix64() * 99;
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64_1 = point.X.ToFix64().DivByPositiveUncheckedUnrounded(this.Period) + this.Seed.SeedX;
      Fix64 fix64_2 = point.Y.ToFix64().DivByPositiveUncheckedUnrounded(this.Period) + this.Seed.SeedY;
      Fix64 fix64_3 = (fix64_1 + fix64_2).MultByUnchecked(SimplexNoise2dV2.F2);
      int intFloored1 = (fix64_1 + fix64_3).ToIntFloored();
      int intFloored2 = (fix64_2 + fix64_3).ToIntFloored();
      Fix64 fix64_4 = SimplexNoise2dV2.G2.MultByUnchecked(intFloored1 + intFloored2);
      Fix64 fix64_5 = fix64_1 - intFloored1 + fix64_4;
      Fix64 fix64_6 = fix64_2 - intFloored2 + fix64_4;
      Fix64 fix64_7 = this.m_mean64;
      Fix64 rhs1 = Fix64.Half - fix64_5 * fix64_5 - fix64_6 * fix64_6;
      Fix64 fix64_8;
      if (rhs1.IsPositive)
      {
        rhs1 = rhs1.MultByUnchecked(rhs1);
        (Fix64, Fix64) grad = SimplexNoise2dV2.getGrad(SimplexNoise2dV2.getPerm(intFloored1 + SimplexNoise2dV2.getPerm(intFloored2)));
        Fix64 rhs2 = fix64_5.MultByUnchecked(grad.Item1) + fix64_6.MultByUnchecked(grad.Item2);
        Fix64 fix64_9 = fix64_7;
        fix64_8 = this.m_amplitudeScaled64.MultByUnchecked(rhs2);
        fix64_8 = fix64_8.MultByUnchecked(rhs1);
        Fix64 fix64_10 = fix64_8.MultByUnchecked(rhs1);
        fix64_7 = fix64_9 + fix64_10;
      }
      Vector2i vector2i = fix64_5 > fix64_6 ? new Vector2i(1, 0) : new Vector2i(0, 1);
      Fix64 fix64_11 = fix64_5 - vector2i.X + SimplexNoise2dV2.G2;
      Fix64 fix64_12 = fix64_6 - vector2i.Y + SimplexNoise2dV2.G2;
      Fix64 rhs3 = Fix64.Half - fix64_11 * fix64_11 - fix64_12 * fix64_12;
      if (rhs3.IsPositive)
      {
        rhs3 = rhs3.MultByUnchecked(rhs3);
        (Fix64, Fix64) grad = SimplexNoise2dV2.getGrad(SimplexNoise2dV2.getPerm(intFloored1 + vector2i.X + SimplexNoise2dV2.getPerm(intFloored2 + vector2i.Y)));
        Fix64 rhs4 = fix64_11.MultByUnchecked(grad.Item1) + fix64_12.MultByUnchecked(grad.Item2);
        Fix64 fix64_13 = fix64_7;
        fix64_8 = this.m_amplitudeScaled64.MultByUnchecked(rhs4);
        fix64_8 = fix64_8.MultByUnchecked(rhs3);
        Fix64 fix64_14 = fix64_8.MultByUnchecked(rhs3);
        fix64_7 = fix64_13 + fix64_14;
      }
      Fix64 fix64_15 = fix64_5 + SimplexNoise2dV2.TWO_G2_MINUS_ONE;
      Fix64 fix64_16 = fix64_6 + SimplexNoise2dV2.TWO_G2_MINUS_ONE;
      Fix64 rhs5 = Fix64.Half - fix64_15 * fix64_15 - fix64_16 * fix64_16;
      if (rhs5.IsPositive)
      {
        rhs5 = rhs5.MultByUnchecked(rhs5);
        (Fix64, Fix64) grad = SimplexNoise2dV2.getGrad(SimplexNoise2dV2.getPerm(intFloored1 + 1 + SimplexNoise2dV2.getPerm(intFloored2 + 1)));
        Fix64 rhs6 = fix64_15.MultByUnchecked(grad.Item1) + fix64_16.MultByUnchecked(grad.Item2);
        Fix64 fix64_17 = fix64_7;
        fix64_8 = this.m_amplitudeScaled64.MultByUnchecked(rhs6);
        fix64_8 = fix64_8.MultByUnchecked(rhs5);
        Fix64 fix64_18 = fix64_8.MultByUnchecked(rhs5);
        fix64_7 = fix64_17 + fix64_18;
      }
      return fix64_7;
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new SimplexNoise2dV2(this.MeanValue, this.Amplitude, this.Period, random.NoiseSeed2d64());
    }

    static SimplexNoise2dV2()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SimplexNoise2dV2.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SimplexNoise2dV2) obj).SerializeData(writer));
      SimplexNoise2dV2.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SimplexNoise2dV2) obj).DeserializeData(reader));
      SimplexNoise2dV2.GRADS = new (Fix64, Fix64)[32]
      {
        (Fix64.FromDouble(0.9951847), Fix64.FromDouble(0.0980171)),
        (Fix64.FromDouble(0.9569403), Fix64.FromDouble(0.2902847)),
        (Fix64.FromDouble(0.8819213), Fix64.FromDouble(0.4713967)),
        (Fix64.FromDouble(0.7730105), Fix64.FromDouble(0.6343933)),
        (Fix64.FromDouble(0.6343933), Fix64.FromDouble(0.7730105)),
        (Fix64.FromDouble(0.4713967), Fix64.FromDouble(0.8819213)),
        (Fix64.FromDouble(0.2902847), Fix64.FromDouble(0.9569403)),
        (Fix64.FromDouble(0.0980171), Fix64.FromDouble(0.9951847)),
        (Fix64.FromDouble(-0.0980171), Fix64.FromDouble(0.9951847)),
        (Fix64.FromDouble(-0.2902847), Fix64.FromDouble(0.9569403)),
        (Fix64.FromDouble(-0.4713967), Fix64.FromDouble(0.8819213)),
        (Fix64.FromDouble(-0.6343933), Fix64.FromDouble(0.7730105)),
        (Fix64.FromDouble(-0.7730105), Fix64.FromDouble(0.6343933)),
        (Fix64.FromDouble(-0.8819213), Fix64.FromDouble(0.4713967)),
        (Fix64.FromDouble(-0.9569403), Fix64.FromDouble(0.2902847)),
        (Fix64.FromDouble(-0.9951847), Fix64.FromDouble(0.0980171)),
        (Fix64.FromDouble(-0.9951847), Fix64.FromDouble(-0.0980171)),
        (Fix64.FromDouble(-0.9569403), Fix64.FromDouble(-0.2902847)),
        (Fix64.FromDouble(-0.8819213), Fix64.FromDouble(-0.4713967)),
        (Fix64.FromDouble(-0.7730105), Fix64.FromDouble(-0.6343933)),
        (Fix64.FromDouble(-0.6343933), Fix64.FromDouble(-0.7730105)),
        (Fix64.FromDouble(-0.4713967), Fix64.FromDouble(-0.8819213)),
        (Fix64.FromDouble(-0.2902847), Fix64.FromDouble(-0.9569403)),
        (Fix64.FromDouble(-0.0980171), Fix64.FromDouble(-0.9951847)),
        (Fix64.FromDouble(0.0980171), Fix64.FromDouble(-0.9951847)),
        (Fix64.FromDouble(0.2902847), Fix64.FromDouble(-0.9569403)),
        (Fix64.FromDouble(0.4713967), Fix64.FromDouble(-0.8819213)),
        (Fix64.FromDouble(0.6343933), Fix64.FromDouble(-0.7730105)),
        (Fix64.FromDouble(0.7730105), Fix64.FromDouble(-0.6343933)),
        (Fix64.FromDouble(0.8819213), Fix64.FromDouble(-0.4713967)),
        (Fix64.FromDouble(0.9569403), Fix64.FromDouble(-0.2902847)),
        (Fix64.FromDouble(0.9951847), Fix64.FromDouble(-0.0980171))
      };
      SimplexNoise2dV2.PERM_TABLE = new byte[256]
      {
        (byte) 36,
        (byte) 152,
        (byte) 173,
        (byte) 17,
        (byte) 207,
        (byte) 123,
        (byte) 237,
        (byte) 251,
        (byte) 37,
        (byte) 157,
        (byte) 51,
        (byte) 198,
        (byte) 122,
        (byte) 74,
        (byte) 19,
        (byte) 125,
        (byte) 163,
        (byte) 181,
        (byte) 27,
        (byte) 132,
        (byte) 78,
        (byte) 139,
        (byte) 107,
        (byte) 189,
        (byte) 31,
        (byte) 85,
        (byte) 35,
        (byte) 142,
        (byte) 201,
        (byte) 134,
        (byte) 138,
        (byte) 83,
        (byte) 154,
        (byte) 189,
        (byte) 230,
        (byte) 186,
        (byte) 225,
        (byte) 192,
        (byte) 250,
        (byte) 78,
        (byte) 214,
        (byte) 134,
        (byte) 162,
        (byte) 27,
        (byte) 6,
        (byte) 72,
        (byte) 94,
        (byte) 207,
        (byte) 221,
        (byte) 96,
        (byte) 4,
        (byte) 3,
        (byte) 46,
        (byte) 244,
        (byte) 54,
        (byte) 191,
        (byte) 12,
        (byte) 204,
        (byte) 178,
        (byte) 137,
        (byte) 55,
        (byte) 6,
        (byte) 178,
        (byte) 14,
        (byte) 67,
        (byte) 124,
        (byte) 14,
        (byte) 108,
        (byte) 121,
        (byte) 56,
        (byte) 203,
        (byte) 191,
        (byte) 55,
        (byte) 83,
        (byte) 212,
        (byte) 110,
        (byte) 159,
        (byte) 207,
        (byte) 109,
        (byte) 121,
        (byte) 216,
        (byte) 223,
        (byte) 89,
        (byte) 61,
        (byte) 245,
        (byte) 175,
        (byte) 29,
        (byte) 146,
        (byte) 20,
        (byte) 202,
        (byte) 39,
        (byte) 117,
        (byte) 23,
        (byte) 52,
        (byte) 152,
        (byte) 232,
        (byte) 5,
        (byte) 195,
        (byte) 220,
        (byte) 28,
        (byte) 148,
        (byte) 135,
        (byte) 155,
        (byte) 196,
        (byte) 157,
        (byte) 23,
        (byte) 220,
        (byte) 233,
        (byte) 191,
        (byte) 221,
        (byte) 105,
        (byte) 125,
        (byte) 73,
        (byte) 239,
        (byte) 183,
        (byte) 232,
        (byte) 22,
        (byte) 104,
        (byte) 198,
        (byte) 39,
        (byte) 206,
        byte.MaxValue,
        (byte) 173,
        (byte) 102,
        (byte) 250,
        (byte) 156,
        (byte) 129,
        (byte) 107,
        (byte) 153,
        (byte) 235,
        (byte) 57,
        (byte) 186,
        (byte) 171,
        (byte) 184,
        (byte) 242,
        (byte) 67,
        (byte) 222,
        (byte) 66,
        (byte) 190,
        (byte) 68,
        (byte) 254,
        (byte) 121,
        (byte) 186,
        (byte) 207,
        (byte) 177,
        (byte) 164,
        (byte) 251,
        (byte) 27,
        (byte) 176,
        (byte) 50,
        (byte) 96,
        (byte) 242,
        (byte) 186,
        (byte) 248,
        (byte) 95,
        (byte) 227,
        (byte) 72,
        (byte) 111,
        (byte) 205,
        (byte) 4,
        (byte) 208,
        (byte) 173,
        (byte) 1,
        (byte) 128,
        (byte) 164,
        (byte) 49,
        (byte) 61,
        (byte) 202,
        (byte) 140,
        (byte) 101,
        (byte) 12,
        (byte) 108,
        (byte) 152,
        (byte) 168,
        (byte) 193,
        (byte) 155,
        (byte) 7,
        (byte) 138,
        (byte) 117,
        (byte) 53,
        (byte) 20,
        (byte) 76,
        (byte) 24,
        (byte) 97,
        (byte) 210,
        (byte) 76,
        (byte) 180,
        (byte) 28,
        (byte) 107,
        (byte) 88,
        (byte) 61,
        (byte) 152,
        (byte) 233,
        (byte) 136,
        (byte) 51,
        (byte) 8,
        (byte) 217,
        (byte) 123,
        (byte) 37,
        (byte) 55,
        (byte) 198,
        (byte) 168,
        (byte) 56,
        (byte) 20,
        (byte) 10,
        (byte) 230,
        (byte) 0,
        (byte) 74,
        (byte) 214,
        (byte) 102,
        (byte) 124,
        (byte) 95,
        (byte) 120,
        (byte) 204,
        (byte) 221,
        (byte) 137,
        (byte) 8,
        (byte) 248,
        (byte) 11,
        (byte) 7,
        (byte) 192,
        (byte) 63,
        (byte) 129,
        (byte) 181,
        (byte) 3,
        (byte) 94,
        (byte) 153,
        (byte) 80,
        (byte) 187,
        (byte) 55,
        (byte) 219,
        (byte) 8,
        (byte) 235,
        (byte) 249,
        (byte) 207,
        (byte) 66,
        (byte) 88,
        (byte) 232,
        (byte) 163,
        (byte) 159,
        (byte) 48,
        (byte) 112,
        (byte) 218,
        (byte) 100,
        (byte) 172,
        (byte) 29,
        (byte) 217,
        (byte) 219,
        (byte) 227,
        (byte) 241,
        (byte) 50,
        (byte) 157,
        (byte) 41,
        (byte) 96,
        (byte) 64,
        (byte) 37
      };
      SimplexNoise2dV2.F2 = Fix64.FromDouble((Math.Sqrt(3.0) - 1.0) / 2.0);
      SimplexNoise2dV2.G2 = Fix64.FromDouble((3.0 - Math.Sqrt(3.0)) / 6.0);
      SimplexNoise2dV2.TWO_G2_MINUS_ONE = Fix64.FromDouble((3.0 - Math.Sqrt(3.0)) / 3.0 - 1.0);
    }
  }
}
