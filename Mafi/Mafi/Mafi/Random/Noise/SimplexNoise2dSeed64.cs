// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dSeed64
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
  public readonly struct SimplexNoise2dSeed64
  {
    [EditorLabel(null, "Random offset for the noise. This number should not be an integer or zero. A random decimal number between -8 and 8 is ideal. ", false, false)]
    [EditorRange(-16.0, 16.0)]
    public readonly Fix64 SeedX;
    [EditorRange(-16.0, 16.0)]
    [EditorLabel(null, "Random offset for the noise. This number should not be an integer or zero. A random decimal number between -8 and 8 is ideal. ", false, false)]
    public readonly Fix64 SeedY;

    public static void Serialize(SimplexNoise2dSeed64 value, BlobWriter writer)
    {
      Fix64.Serialize(value.SeedX, writer);
      Fix64.Serialize(value.SeedY, writer);
    }

    public static SimplexNoise2dSeed64 Deserialize(BlobReader reader)
    {
      return new SimplexNoise2dSeed64(Fix64.Deserialize(reader), Fix64.Deserialize(reader));
    }

    public static SimplexNoise2dSeed64 Invalid => new SimplexNoise2dSeed64();

    public bool IsValid => this.SeedX.IsNotZero || this.SeedY.IsNotZero;

    /// <summary>
    /// Ideal values for seeds are around 1, but not integers!
    /// </summary>
    [LoadCtor]
    public SimplexNoise2dSeed64(Fix64 seedX, Fix64 seedY)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.SeedX = seedX;
      this.SeedY = seedY;
    }

    [Pure]
    public SimplexNoise2dSeed64 CheckIsValid()
    {
      if (this.IsValid)
        return this;
      Log.Error("Uninitialized simplex noise seed.");
      return new SimplexNoise2dSeed64(Fix64.Tau, Fix64.Sqrt2);
    }

    [Pure]
    public SimplexNoise2dSeed64 EnsureIsValid()
    {
      return !this.IsValid ? new SimplexNoise2dSeed64(Fix64.Tau, Fix64.Sqrt2) : this;
    }

    [Pure]
    public SimplexNoise2dSeed ToFix32()
    {
      return new SimplexNoise2dSeed(this.SeedX.ToFix32(), this.SeedY.ToFix32());
    }

    public static SimplexNoise2dSeed64 FromRngSeed(ulong value)
    {
      value = value.GetRngSeed();
      return new SimplexNoise2dSeed64(Fix32.FromRaw((int) value & 16777215) - Fix64.Eight, Fix32.FromRaw((int) (value >> 32) & 16777215) - Fix64.Eight);
    }
  }
}
