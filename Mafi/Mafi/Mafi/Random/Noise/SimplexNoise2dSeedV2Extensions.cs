// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dSeedV2Extensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public static class SimplexNoise2dSeedV2Extensions
  {
    public static SimplexNoise2dSeed64 NoiseSeed2d64(this IRandom random)
    {
      return new SimplexNoise2dSeed64(random.NextFix64BetweenPlusMinus8Fast(), random.NextFix64BetweenPlusMinus8Fast());
    }

    public static SimplexNoise2dSeed64 ToNoiseSeed2d64(this ulong random)
    {
      random = random.GetRngSeed();
      return new SimplexNoise2dSeed64(Fix64.FromRaw((long) random & 16777215L) - Fix64.Eight, Fix64.FromRaw((long) (random >> 32) & 16777215L) - Fix64.Eight);
    }
  }
}
