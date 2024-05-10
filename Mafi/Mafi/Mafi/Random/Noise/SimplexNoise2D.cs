// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2D
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
  [OnlyForSaveCompatibility(null)]
  [Obsolete("Has precision issues, use SimplexNoise2dV2 instead")]
  public sealed class SimplexNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Lookup table of 16 gradient vectors.</summary>
    /// <remarks>
    ///  Generated with:
    ///  <code>
    /// Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    /// int size = 16;
    /// for (int i = 0; i &lt; size; ++i) {
    /// double x = Math.Cos(2.0 * Math.PI * i / size);
    /// double y = Math.Sin(2.0 * Math.PI * i / size);
    /// Console.WriteLine("new Vector2f({0:0.0000000}f, {1:0.0000000}f),", x, y);
    /// }
    ///  </code>
    ///  </remarks>
    private static readonly Vector2f[] GRADS;
    /// <summary>
    /// Permutation table for simplex noise. Taken from literature.
    /// </summary>
    private static readonly byte[] PERM_TABLE;
    private static readonly Fix32 F2;
    private static readonly Fix32 G2;
    public readonly SimplexNoise2dSeed Seed;
    [DoNotSave(0, null)]
    private Fix64 m_mean64;
    [DoNotSave(0, null)]
    private Fix64 m_amplitudeScaled64;

    public static void Serialize(SimplexNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SimplexNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SimplexNoise2D.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.Amplitude, writer);
      Fix32.Serialize(this.MeanValue, writer);
      Fix32.Serialize(this.Period, writer);
      SimplexNoise2dSeed.Serialize(this.Seed, writer);
    }

    public static SimplexNoise2D Deserialize(BlobReader reader)
    {
      SimplexNoise2D simplexNoise2D;
      if (reader.TryStartClassDeserialization<SimplexNoise2D>(out simplexNoise2D))
        reader.EnqueueDataDeserialization((object) simplexNoise2D, SimplexNoise2D.s_deserializeDataDelayedAction);
      return simplexNoise2D;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.Amplitude = Fix32.Deserialize(reader);
      this.MeanValue = Fix32.Deserialize(reader);
      this.Period = Fix32.Deserialize(reader);
      reader.SetField<SimplexNoise2D>(this, "Seed", (object) SimplexNoise2dSeed.Deserialize(reader));
      this.initAfterLoad();
    }

    private static Vector2f getGrad(int index) => SimplexNoise2D.GRADS[index & 15];

    private static int getPerm(int index)
    {
      return (int) SimplexNoise2D.PERM_TABLE[index & (int) byte.MaxValue];
    }

    public Fix32 MeanValue { get; private set; }

    public Fix32 Amplitude { get; private set; }

    public Fix32 Period { get; private set; }

    public SimplexNoise2D(SimplexNoise2dSeed seed, SimplexNoise2dParams parameters)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(seed, parameters.MeanValue, parameters.Amplitude, parameters.Period);
    }

    public SimplexNoise2D(SimplexNoise2dSeed seed, Fix32 meanValue, Fix32 amplitude, Fix32 period)
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
      point = point.DivByPositiveUncheckedUnrounded(this.Period) + this.Seed.Vector2f;
      Vector2i flooredVector2i = (point + point.Sum.MultByUnchecked(SimplexNoise2D.F2)).FlooredVector2i;
      Vector2f vector2f1 = point - (flooredVector2i.Vector2f - SimplexNoise2D.G2.MultByUnchecked(flooredVector2i.Sum));
      Fix64 zero = Fix64.Zero;
      Fix64 fix64_1 = Fix64.Half - vector2f1.DotSelf;
      if (fix64_1.IsPositive)
      {
        Fix64 fix64_2 = fix64_1 * fix64_1;
        int perm = SimplexNoise2D.getPerm(flooredVector2i.X + SimplexNoise2D.getPerm(flooredVector2i.Y));
        zero += this.m_amplitudeScaled64 * fix64_2 * fix64_2 * vector2f1.Dot(SimplexNoise2D.getGrad(perm));
      }
      Vector2i vector2i = vector2f1.X > vector2f1.Y ? new Vector2i(1, 0) : new Vector2i(0, 1);
      Vector2f vector2f2 = vector2f1 - vector2i.Vector2fUnclamped + SimplexNoise2D.G2;
      Fix64 fix64_3 = Fix64.Half - vector2f2.DotSelf;
      if (fix64_3.IsPositive)
      {
        fix64_3 *= fix64_3;
        int perm = SimplexNoise2D.getPerm(flooredVector2i.X + vector2i.X + SimplexNoise2D.getPerm(flooredVector2i.Y + vector2i.Y));
        zero += this.m_amplitudeScaled64 * fix64_3 * fix64_3 * vector2f2.Dot(SimplexNoise2D.getGrad(perm));
      }
      Vector2f vector2f3 = vector2f1 + Fix32.Two * SimplexNoise2D.G2 - Fix32.One;
      Fix64 fix64_4 = Fix64.Half - vector2f3.DotSelf;
      if (fix64_4.IsPositive)
      {
        fix64_4 *= fix64_4;
        int perm = SimplexNoise2D.getPerm(flooredVector2i.X + 1 + SimplexNoise2D.getPerm(flooredVector2i.Y + 1));
        zero += this.m_amplitudeScaled64 * fix64_4 * fix64_4 * vector2f3.Dot(SimplexNoise2D.getGrad(perm));
      }
      return this.m_mean64 + zero;
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new SimplexNoise2D(random.NoiseSeed2d(), this.MeanValue, this.Amplitude, this.Period);
    }

    static SimplexNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SimplexNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SimplexNoise2D) obj).SerializeData(writer));
      SimplexNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SimplexNoise2D) obj).DeserializeData(reader));
      SimplexNoise2D.GRADS = new Vector2f[16]
      {
        new Vector2f(Fix32.FromDouble(1.0), Fix32.FromDouble(0.0)),
        new Vector2f(Fix32.FromDouble(0.9238795), Fix32.FromDouble(0.3826834)),
        new Vector2f(Fix32.FromDouble(0.7071068), Fix32.FromDouble(0.7071068)),
        new Vector2f(Fix32.FromDouble(0.3826834), Fix32.FromDouble(0.9238795)),
        new Vector2f(Fix32.FromDouble(0.0), Fix32.FromDouble(1.0)),
        new Vector2f(Fix32.FromDouble(-0.3826834), Fix32.FromDouble(0.9238795)),
        new Vector2f(Fix32.FromDouble(-0.7071068), Fix32.FromDouble(0.7071068)),
        new Vector2f(Fix32.FromDouble(-0.9238795), Fix32.FromDouble(0.3826834)),
        new Vector2f(Fix32.FromDouble(-1.0), Fix32.FromDouble(0.0)),
        new Vector2f(Fix32.FromDouble(-0.9238795), Fix32.FromDouble(-0.3826834)),
        new Vector2f(Fix32.FromDouble(-0.7071068), Fix32.FromDouble(-0.7071068)),
        new Vector2f(Fix32.FromDouble(-0.3826834), Fix32.FromDouble(-0.9238795)),
        new Vector2f(Fix32.FromDouble(0.0), Fix32.FromDouble(-1.0)),
        new Vector2f(Fix32.FromDouble(0.3826834), Fix32.FromDouble(-0.9238795)),
        new Vector2f(Fix32.FromDouble(0.7071068), Fix32.FromDouble(-0.7071068)),
        new Vector2f(Fix32.FromDouble(0.9238795), Fix32.FromDouble(-0.3826834))
      };
      SimplexNoise2D.PERM_TABLE = new byte[256]
      {
        (byte) 151,
        (byte) 160,
        (byte) 137,
        (byte) 91,
        (byte) 90,
        (byte) 15,
        (byte) 131,
        (byte) 13,
        (byte) 201,
        (byte) 95,
        (byte) 96,
        (byte) 53,
        (byte) 194,
        (byte) 233,
        (byte) 7,
        (byte) 225,
        (byte) 140,
        (byte) 36,
        (byte) 103,
        (byte) 30,
        (byte) 69,
        (byte) 142,
        (byte) 8,
        (byte) 99,
        (byte) 37,
        (byte) 240,
        (byte) 21,
        (byte) 10,
        (byte) 23,
        (byte) 190,
        (byte) 6,
        (byte) 148,
        (byte) 247,
        (byte) 120,
        (byte) 234,
        (byte) 75,
        (byte) 0,
        (byte) 26,
        (byte) 197,
        (byte) 62,
        (byte) 94,
        (byte) 252,
        (byte) 219,
        (byte) 203,
        (byte) 117,
        (byte) 35,
        (byte) 11,
        (byte) 32,
        (byte) 57,
        (byte) 177,
        (byte) 33,
        (byte) 88,
        (byte) 237,
        (byte) 149,
        (byte) 56,
        (byte) 87,
        (byte) 174,
        (byte) 20,
        (byte) 125,
        (byte) 136,
        (byte) 171,
        (byte) 168,
        (byte) 68,
        (byte) 175,
        (byte) 74,
        (byte) 165,
        (byte) 71,
        (byte) 134,
        (byte) 139,
        (byte) 48,
        (byte) 27,
        (byte) 166,
        (byte) 77,
        (byte) 146,
        (byte) 158,
        (byte) 231,
        (byte) 83,
        (byte) 111,
        (byte) 229,
        (byte) 122,
        (byte) 60,
        (byte) 211,
        (byte) 133,
        (byte) 230,
        (byte) 220,
        (byte) 105,
        (byte) 92,
        (byte) 41,
        (byte) 55,
        (byte) 46,
        (byte) 245,
        (byte) 40,
        (byte) 244,
        (byte) 102,
        (byte) 143,
        (byte) 54,
        (byte) 65,
        (byte) 25,
        (byte) 63,
        (byte) 161,
        (byte) 1,
        (byte) 216,
        (byte) 80,
        (byte) 73,
        (byte) 209,
        (byte) 76,
        (byte) 132,
        (byte) 187,
        (byte) 208,
        (byte) 89,
        (byte) 18,
        (byte) 169,
        (byte) 200,
        (byte) 196,
        (byte) 135,
        (byte) 130,
        (byte) 116,
        (byte) 188,
        (byte) 159,
        (byte) 86,
        (byte) 164,
        (byte) 100,
        (byte) 109,
        (byte) 198,
        (byte) 173,
        (byte) 186,
        (byte) 3,
        (byte) 64,
        (byte) 52,
        (byte) 217,
        (byte) 226,
        (byte) 250,
        (byte) 124,
        (byte) 123,
        (byte) 5,
        (byte) 202,
        (byte) 38,
        (byte) 147,
        (byte) 118,
        (byte) 126,
        byte.MaxValue,
        (byte) 82,
        (byte) 85,
        (byte) 212,
        (byte) 207,
        (byte) 206,
        (byte) 59,
        (byte) 227,
        (byte) 47,
        (byte) 16,
        (byte) 58,
        (byte) 17,
        (byte) 182,
        (byte) 189,
        (byte) 28,
        (byte) 42,
        (byte) 223,
        (byte) 183,
        (byte) 170,
        (byte) 213,
        (byte) 119,
        (byte) 248,
        (byte) 152,
        (byte) 2,
        (byte) 44,
        (byte) 154,
        (byte) 163,
        (byte) 70,
        (byte) 221,
        (byte) 153,
        (byte) 101,
        (byte) 155,
        (byte) 167,
        (byte) 43,
        (byte) 172,
        (byte) 9,
        (byte) 129,
        (byte) 22,
        (byte) 39,
        (byte) 253,
        (byte) 19,
        (byte) 98,
        (byte) 108,
        (byte) 110,
        (byte) 79,
        (byte) 113,
        (byte) 224,
        (byte) 232,
        (byte) 178,
        (byte) 185,
        (byte) 112,
        (byte) 104,
        (byte) 218,
        (byte) 246,
        (byte) 97,
        (byte) 228,
        (byte) 251,
        (byte) 34,
        (byte) 242,
        (byte) 193,
        (byte) 238,
        (byte) 210,
        (byte) 144,
        (byte) 12,
        (byte) 191,
        (byte) 179,
        (byte) 162,
        (byte) 241,
        (byte) 81,
        (byte) 51,
        (byte) 145,
        (byte) 235,
        (byte) 249,
        (byte) 14,
        (byte) 239,
        (byte) 107,
        (byte) 49,
        (byte) 192,
        (byte) 214,
        (byte) 31,
        (byte) 181,
        (byte) 199,
        (byte) 106,
        (byte) 157,
        (byte) 184,
        (byte) 84,
        (byte) 204,
        (byte) 176,
        (byte) 115,
        (byte) 121,
        (byte) 50,
        (byte) 45,
        (byte) 127,
        (byte) 4,
        (byte) 150,
        (byte) 254,
        (byte) 138,
        (byte) 236,
        (byte) 205,
        (byte) 93,
        (byte) 222,
        (byte) 114,
        (byte) 67,
        (byte) 29,
        (byte) 24,
        (byte) 72,
        (byte) 243,
        (byte) 141,
        (byte) 128,
        (byte) 195,
        (byte) 78,
        (byte) 66,
        (byte) 215,
        (byte) 61,
        (byte) 156,
        (byte) 180
      };
      SimplexNoise2D.F2 = Fix32.FromDouble(0.36602540378443865);
      SimplexNoise2D.G2 = Fix32.FromDouble(0.21132486540518711);
    }
  }
}
