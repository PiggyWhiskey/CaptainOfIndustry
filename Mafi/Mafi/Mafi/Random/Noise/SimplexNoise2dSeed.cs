// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dSeed
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Random.Noise
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct SimplexNoise2dSeed
  {
    [EditorRange(-16.0, 16.0)]
    [EditorLabel(null, "Random offset for the noise. This number should not be an integer or zero. A random decimal number between -8 and 8 is ideal. ", false, false)]
    public readonly Fix32 SeedX;
    [EditorRange(-16.0, 16.0)]
    [EditorLabel(null, "Random offset for the noise. This number should not be an integer or zero. A random decimal number between -8 and 8 is ideal. ", false, false)]
    public readonly Fix32 SeedY;

    public static void Serialize(SimplexNoise2dSeed value, BlobWriter writer)
    {
      Fix32.Serialize(value.SeedX, writer);
      Fix32.Serialize(value.SeedY, writer);
    }

    public static SimplexNoise2dSeed Deserialize(BlobReader reader)
    {
      return new SimplexNoise2dSeed(Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public static SimplexNoise2dSeed Invalid => new SimplexNoise2dSeed();

    public Vector2f Vector2f => new Vector2f(this.SeedX, this.SeedY);

    public bool IsValid => this.SeedX.IsNotZero || this.SeedY.IsNotZero;

    /// <summary>
    /// Ideal values for seeds are around 1, but not integers!
    /// </summary>
    [LoadCtor]
    public SimplexNoise2dSeed(Fix32 seedX, Fix32 seedY)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.SeedX = seedX;
      this.SeedY = seedY;
    }

    [Pure]
    public SimplexNoise2dSeed CheckIsValid()
    {
      if (this.IsValid)
        return this;
      Log.Error("Uninitialized simplex noise seed.");
      return new SimplexNoise2dSeed(Fix32.Tau, Fix32.Sqrt2);
    }

    [Pure]
    public SimplexNoise2dSeed EnsureIsValid()
    {
      return !this.IsValid ? new SimplexNoise2dSeed(Fix32.Tau, Fix32.Sqrt2) : this;
    }

    public static SimplexNoise2dSeed FromRngSeed(ulong value)
    {
      value = value.GetRngSeed();
      return new SimplexNoise2dSeed(Fix32.FromRaw((int) value & 16383) - Fix32.Eight, Fix32.FromRaw((int) (value >> 32) & 16383) - Fix32.Eight);
    }
  }
}
