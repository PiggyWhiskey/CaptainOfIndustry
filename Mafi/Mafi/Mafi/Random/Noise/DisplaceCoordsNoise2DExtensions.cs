// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.DisplaceCoordsNoise2DExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  public static class DisplaceCoordsNoise2DExtensions
  {
    public static WarpCoordsNoise WarpCoords(this INoise2D noise, INoise2D warpNoise)
    {
      return new WarpCoordsNoise(noise, warpNoise);
    }

    [Obsolete("Uses old simplex noise, use WarpCoordsV2 instead.")]
    [OnlyForSaveCompatibility(null)]
    public static INoise2D WarpCoords(
      this INoise2D noise,
      SimplexNoise2dParams warpNoiseParams,
      SimplexNoise2dSeed warpNoiseSeed)
    {
      return !warpNoiseParams.MeanValue.IsZero || !warpNoiseParams.Amplitude.IsZero ? (INoise2D) new WarpCoordsNoise(noise, (INoise2D) new SimplexNoise2D(warpNoiseSeed, warpNoiseParams)) : noise;
    }

    [OnlyForSaveCompatibility(null)]
    public static INoise2D WarpCoordsV2(
      this INoise2D noise,
      SimplexNoise2dParams warpNoiseParams,
      SimplexNoise2dSeed64 warpNoiseSeed)
    {
      return (warpNoiseParams.Amplitude.IsZero || warpNoiseParams.Period.IsZero) && warpNoiseParams.MeanValue.IsZero ? noise : (INoise2D) new WarpCoordsNoise(noise, (INoise2D) new SimplexNoise2dV2(warpNoiseParams, warpNoiseSeed));
    }
  }
}
