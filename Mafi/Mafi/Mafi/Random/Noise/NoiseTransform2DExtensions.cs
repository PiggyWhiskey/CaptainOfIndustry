// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.NoiseTransform2DExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public static class NoiseTransform2DExtensions
  {
    public static INoise2D Transform(this INoise2D noise, Noise2dTransformParams p)
    {
      if (p.FrequencyMult != Fix32.One)
        return (INoise2D) new Noise2dTransform(noise, p.Multiplier, p.Addend, p.FrequencyMult);
      if (!(p.Multiplier != Fix64.One))
        return (INoise2D) new Noise2dTransformAdd(noise, p.Addend);
      return !p.Addend.IsZero ? (INoise2D) new Noise2dTransformMultAdd(noise, p.Multiplier, p.Addend) : (INoise2D) new Noise2dTransformMult(noise, p.Multiplier);
    }

    public static INoise2D Min(this INoise2D noise, Fix64 maxValue)
    {
      return (INoise2D) new Noise2dTransformMin(noise, maxValue);
    }

    public static INoise2D Min(this INoise2D noise, INoise2D otherNoise)
    {
      return (INoise2D) new Noise2dTransformMinNoise(noise, otherNoise);
    }

    public static INoise2D Max(this INoise2D noise, Fix64 minValue)
    {
      return (INoise2D) new Noise2dTransformMax(noise, minValue);
    }

    public static INoise2D Max(this INoise2D noise, INoise2D otherNoise)
    {
      return (INoise2D) new Noise2dTransformMaxNoise(noise, otherNoise);
    }

    public static Noise2dTransformAdd Add(this INoise2D noise, Fix64 addend)
    {
      return new Noise2dTransformAdd(noise, addend);
    }

    public static Noise2dTransformMult Multiply(this INoise2D noise, Fix64 mult)
    {
      return new Noise2dTransformMult(noise, mult);
    }

    public static Noise2dTransformMultAdd MultAdd(this INoise2D noise, Fix64 mult, Fix64 addend)
    {
      return new Noise2dTransformMultAdd(noise, mult, addend);
    }
  }
}
