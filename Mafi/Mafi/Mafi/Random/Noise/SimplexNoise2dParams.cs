// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dParams
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
  [GenerateSerializer(false, null, 0)]
  public readonly struct SimplexNoise2dParams : IEquatable<SimplexNoise2dParams>
  {
    [EditorLabel(null, "Specifies the mean value.", false, false)]
    [EditorRange(-1000.0, 1000.0)]
    public readonly Fix32 MeanValue;
    [EditorLabel(null, "Specifies how much the value will fluctuate above and below the mean.", false, false)]
    [EditorRange(0.0, 1000.0)]
    public readonly Fix32 Amplitude;
    [EditorRange(0.0, 1000.0)]
    [EditorLabel(null, "Specifies approximate distance between peaks/valleys.", false, false)]
    public readonly Fix32 Period;

    public static void Serialize(SimplexNoise2dParams value, BlobWriter writer)
    {
      Fix32.Serialize(value.MeanValue, writer);
      Fix32.Serialize(value.Amplitude, writer);
      Fix32.Serialize(value.Period, writer);
    }

    public static SimplexNoise2dParams Deserialize(BlobReader reader)
    {
      return new SimplexNoise2dParams(Fix32.Deserialize(reader), Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public SimplexNoise2dParams(Fix32 meanValue, Fix32 amplitude, Fix32 period)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.MeanValue = meanValue;
      this.Amplitude = amplitude;
      this.Period = period;
    }

    public INoise2D CreateSimplexNoise2dSafe(SimplexNoise2dSeed64 seed)
    {
      return this.Period.IsZero || this.Amplitude.IsZero ? (INoise2D) new ConstantNoise2D(this.MeanValue) : (INoise2D) new SimplexNoise2dV2(this, seed.EnsureIsValid());
    }

    public static bool operator ==(SimplexNoise2dParams lhs, SimplexNoise2dParams rhs)
    {
      return lhs.MeanValue == rhs.MeanValue && lhs.Amplitude == rhs.Amplitude && lhs.Period == rhs.Period;
    }

    public static bool operator !=(SimplexNoise2dParams lhs, SimplexNoise2dParams rhs)
    {
      return !(lhs == rhs);
    }

    public bool Equals(SimplexNoise2dParams other) => this == other;

    public override bool Equals(object obj)
    {
      return obj is SimplexNoise2dParams other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<Fix32, Fix32, Fix32>(this.MeanValue, this.Amplitude, this.Period);
    }
  }
}
